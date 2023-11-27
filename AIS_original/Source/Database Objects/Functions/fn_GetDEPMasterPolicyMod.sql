if exists (select 1 from sysobjects 
		where name = 'fn_GetDEPMasterPolicyMod' and type = 'FN')
	drop function fn_GetDEPMasterPolicyMod
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------
-----
----- Proc Name:  fn_GetDEPMasterPolicyMod
-----
----- Version:  SQL Server 2008
-----
----- Created:  CSC
-----
----- Description: Retrieves DEP Master policy Mod
-----
----- Modified: 
-----
---------------------------------------------------------------------
ALTER function [dbo].[fn_GetDEPMasterPolicyMod]
  (
 @PGMID int    
 )
returns varchar(25)
--WITH SCHEMABINDING
as
begin
 declare @policymod Char(5)
  
 set @policymod = null 
 
 SELECT @policymod=LTRIM(RTRIM(coml_agmt.pol_modulus_txt))
 FROM PREM_ADJ_PGM 
 INNER JOIN COML_AGMT ON COML_AGMT.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID 
 WHERE 
 PREM_ADJ_PGM.PREM_ADJ_PGM_ID = @PGMID 
 AND substring(COML_AGMT.POL_SYM_TXT,1,3) = 'DEP'

 return @policymod
end

go

if object_id('fn_GetDEPMasterPolicyMod') is not null
	print 'Created function fn_GetDEPMasterPolicyMod'
else
	print 'Failed Creating Function fn_GetDEPMasterPolicyMod'
go

if object_id('fn_GetDEPMasterPolicyMod') is not null
	grant exec on fn_GetDEPMasterPolicyMod to public
go
