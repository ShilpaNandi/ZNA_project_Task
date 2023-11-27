if exists (select 1 from sysobjects 
		where name = 'fn_GetDEPMasterPolicyState' and type = 'FN')
	drop function fn_GetDEPMasterPolicyState
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetDEPMasterPolicyState
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Sreedhar Bobbili
-----
-----	Description:	Retrieves DEP Master policy State
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetDEPMasterPolicyState]
  (
	@ADJNO int,
	@PGMID int    
	)
returns varchar(25)
--WITH SCHEMABINDING
as
begin
	declare @DEPPolicyState varchar(25)
	
	set @DEPPolicyState = null	
	
SELECT @DEPPolicyState = LKUP.lkup_txt
    FROM PREM_ADJ_PERD INNER JOIN PREM_ADJ_PGM ON PREM_ADJ_PERD.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID AND
	PREM_ADJ_PERD.CUSTMR_ID = PREM_ADJ_PGM.CUSTMR_ID
	INNER JOIN COML_AGMT ON COML_AGMT.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID 
	LEFT OUTER JOIN LKUP ON COML_AGMT.dedtbl_prot_pol_st_id = LKUP.lkup_id	
	WHERE PREM_ADJ_PERD.PREM_ADJ_ID = @ADJNO AND 
	PREM_ADJ_PERD.PREM_ADJ_PGM_ID = @PGMID AND substring(COML_AGMT.POL_SYM_TXT,1,3) = 'DEP'

	return @DEPPolicyState

end

go

if object_id('fn_GetDEPMasterPolicyState') is not null
	print 'Created function fn_GetDEPMasterPolicyState'
else
	print 'Failed Creating Function fn_GetDEPMasterPolicyState'
go

if object_id('fn_GetDEPMasterPolicyState') is not null
	grant exec on fn_GetDEPMasterPolicyState to public
go
