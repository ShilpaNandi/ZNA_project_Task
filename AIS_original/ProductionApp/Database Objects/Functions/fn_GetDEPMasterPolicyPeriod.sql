if exists (select 1 from sysobjects 
		where name = 'fn_GetDEPMasterPolicyPeriod' and type = 'FN')
	drop function fn_GetDEPMasterPolicyPeriod
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetDEPMasterPolicyPeriod
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Sreedhar Bobbili
-----
-----	Description:	Retrieves DEP Master policy Period
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetDEPMasterPolicyPeriod]
  (
	@ADJNO int,
	@PGMID int    
	)
returns varchar(25)
--WITH SCHEMABINDING
as
begin
	declare @policyPeriod varchar(25)
	
	set @policyPeriod = null	
	
SELECT @policyPeriod = CONVERT(NVARCHAR(30),COML_AGMT.pol_eff_dt, 101) + ' - ' + CONVERT(NVARCHAR(30), COML_AGMT.planned_end_date, 101)		
    FROM PREM_ADJ_PERD INNER JOIN PREM_ADJ_PGM ON PREM_ADJ_PERD.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID AND
	PREM_ADJ_PERD.CUSTMR_ID = PREM_ADJ_PGM.CUSTMR_ID
	INNER JOIN COML_AGMT ON COML_AGMT.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID WHERE PREM_ADJ_PERD.PREM_ADJ_ID = @ADJNO AND
	PREM_ADJ_PERD.PREM_ADJ_PGM_ID = @PGMID AND substring(COML_AGMT.POL_SYM_TXT,1,3) = 'DEP'

	return @policyPeriod
end

go

if object_id('fn_GetDEPMasterPolicyPeriod') is not null
	print 'Created function fn_GetDEPMasterPolicyPeriod'
else
	print 'Failed Creating Function fn_GetDEPMasterPolicyPeriod'
go

if object_id('fn_GetDEPMasterPolicyPeriod') is not null
	grant exec on fn_GetDEPMasterPolicyPeriod to public
go
