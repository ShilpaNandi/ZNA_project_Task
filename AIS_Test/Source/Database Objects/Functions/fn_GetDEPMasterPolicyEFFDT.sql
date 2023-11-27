if exists (select 1 from sysobjects 
		where name = 'fn_GetDEPMasterPolicyEFFDT' and type = 'FN')
	drop function fn_GetDEPMasterPolicyEFFDT
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
----- Proc Name:  fn_GetDEPMasterPolicyEFFDT
-----
----- Version:  SQL Server 2012
-----
----- Created:  CSC
-----
----- Description: Retrieves DEP Master policy
-----
----- Modified: 
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetDEPMasterPolicyEFFDT]
  (
 @PGMID int    
 )
returns DATETIME
--WITH SCHEMABINDING
as
begin
 declare @policy_eff_dt DATETIME
  
 set @policy_eff_dt = null 
 

 SELECT @policy_eff_dt=coml_agmt.pol_eff_dt
 FROM PREM_ADJ_PGM 
 INNER JOIN COML_AGMT ON COML_AGMT.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID 
 WHERE 
 PREM_ADJ_PGM.PREM_ADJ_PGM_ID = @PGMID 
 AND substring(COML_AGMT.POL_SYM_TXT,1,3) = 'DEP'

 return @policy_eff_dt
end

go

if object_id('fn_GetDEPMasterPolicyEFFDT') is not null
	print 'Created function fn_GetDEPMasterPolicyEFFDT'
else
	print 'Failed Creating Function fn_GetDEPMasterPolicyEFFDT'
go

if object_id('fn_GetDEPMasterPolicyEFFDT') is not null
	grant exec on fn_GetDEPMasterPolicyEFFDT to public
go
