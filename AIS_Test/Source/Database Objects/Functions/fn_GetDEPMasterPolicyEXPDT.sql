if exists (select 1 from sysobjects 
		where name = 'fn_GetDEPMasterPolicyEXPDT' and type = 'FN')
	drop function fn_GetDEPMasterPolicyEXPDT
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
----- Proc Name:  fn_GetDEPMasterPolicyEXPDT
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
CREATE function [dbo].[fn_GetDEPMasterPolicyEXPDT]
  (
 @PGMID int    
 )
returns DATETIME
--WITH SCHEMABINDING
as
begin
 declare @policy_exp_dt DATETIME
  
 set @policy_exp_dt = null 
 

 SELECT @policy_exp_dt=coml_agmt.planned_end_date
 FROM PREM_ADJ_PGM 
 INNER JOIN COML_AGMT ON COML_AGMT.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID 
 WHERE 
 PREM_ADJ_PGM.PREM_ADJ_PGM_ID = @PGMID 
 AND substring(COML_AGMT.POL_SYM_TXT,1,3) = 'DEP'

 return @policy_exp_dt
end

go

if object_id('fn_GetDEPMasterPolicyEXPDT') is not null
	print 'Created function fn_GetDEPMasterPolicyEXPDT'
else
	print 'Failed Creating Function fn_GetDEPMasterPolicyEXPDT'
go

if object_id('fn_GetDEPMasterPolicyEXPDT') is not null
	grant exec on fn_GetDEPMasterPolicyEXPDT to public
go
