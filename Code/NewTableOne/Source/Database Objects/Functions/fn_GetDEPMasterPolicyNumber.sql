if exists (select 1 from sysobjects 
		where name = 'fn_GetDEPMasterPolicyNumber' and type = 'FN')
	drop function fn_GetDEPMasterPolicyNumber
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
----- Proc Name:  fn_GetDEPMasterPolicyNumber
-----
----- Version:  SQL Server 2008
-----
----- Created:  CSC
-----
----- Description: Retrieves DEP Master policy
-----
----- Modified: 
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetDEPMasterPolicyNumber]
  (
 @PGMID int    
 )
returns varchar(25)
--WITH SCHEMABINDING
as
begin
 declare @policynumber Char(10)
  
 set @policynumber = null 
 
 --SUBSTRING(LTRIM(RTRIM(coml_agmt.pol_nbr_txt)), PATINDEX('%[^0 ]%', LTRIM(RTRIM(coml_agmt.pol_nbr_txt)) + ' '), LEN(LTRIM(RTRIM(coml_agmt.pol_nbr_txt))))
 SELECT @policynumber=SUBSTRING(LTRIM(RTRIM(coml_agmt.pol_nbr_txt)),2, LEN(LTRIM(RTRIM(coml_agmt.pol_nbr_txt)))-1)
 FROM PREM_ADJ_PGM 
 INNER JOIN COML_AGMT ON COML_AGMT.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID 
 WHERE 
 PREM_ADJ_PGM.PREM_ADJ_PGM_ID = @PGMID 
 AND substring(COML_AGMT.POL_SYM_TXT,1,3) = 'DEP'

 return @policynumber
end
go

if object_id('fn_GetDEPMasterPolicyNumber') is not null
	print 'Created function fn_GetDEPMasterPolicyNumber'
else
	print 'Failed Creating Function fn_GetDEPMasterPolicyNumber'
go

if object_id('fn_GetDEPMasterPolicyNumber') is not null
	grant exec on fn_GetDEPMasterPolicyNumber to public
go
