if exists (select 1 from sysobjects 
		where name = 'fn_GetDEPMasterPolicy' and type = 'FN')
	drop function fn_GetDEPMasterPolicy
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetDEPMasterPolicy
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	Retrieves DEP Master policy
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetDEPMasterPolicy]
  (
	@ADJNO int,
	@PGMID int    
	)
returns varchar(25)
--WITH SCHEMABINDING
as
begin
	declare @policynumber varchar(25)
	
	set @policynumber = null	
	
SELECT @policynumber = COML_AGMT.POL_SYM_TXT + ' '+ RTRIM(COML_AGMT.POL_NBR_TXT)+ '-' +COML_AGMT.POL_MODULUS_TXT			
    FROM PREM_ADJ_PERD INNER JOIN PREM_ADJ_PGM ON PREM_ADJ_PERD.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID AND
	PREM_ADJ_PERD.CUSTMR_ID = PREM_ADJ_PGM.CUSTMR_ID
	INNER JOIN COML_AGMT ON COML_AGMT.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID WHERE PREM_ADJ_PERD.PREM_ADJ_ID = @ADJNO AND
	PREM_ADJ_PERD.PREM_ADJ_PGM_ID = @PGMID AND substring(COML_AGMT.POL_SYM_TXT,1,3) = 'DEP'

	return @policynumber
end

go

if object_id('fn_GetDEPMasterPolicy') is not null
	print 'Created function fn_GetDEPMasterPolicy'
else
	print 'Failed Creating Function fn_GetDEPMasterPolicy'
go

if object_id('fn_GetDEPMasterPolicy') is not null
	grant exec on fn_GetDEPMasterPolicy to public
go
