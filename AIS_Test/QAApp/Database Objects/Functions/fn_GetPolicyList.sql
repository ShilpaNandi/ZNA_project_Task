if exists (select 1 from sysobjects 
		where name = 'fn_GetPolicyList' and type = 'FN')
	drop function fn_GetPolicyList
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetPolicyList
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	Retrieves the Current Adjustment ID for a given Valuation and Customer 
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetPolicyList]
   (
	@ADJNO int, 
	@PERDID int,
	@TYPNUM int   
	)
returns VARCHAR(8000)
--WITH SCHEMABINDING
as
begin
	declare @sstring varchar(8000)
	set @sstring = null
	select @sstring =Coalesce(@sstring + ', ', '') + COML_AGMT.POl_SYM_TXT + ' ' +
	RTRIM(COML_AGMT.POL_NBR_TXT) + '-' + COML_AGMT.POl_MODULUS_TXT from COML_AGMT 
	INNER JOIN PREM_ADJ_PGM ON COML_AGMT.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID
	INNER JOIN PREM_ADJ_PERD ON PREM_ADJ_PERD.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID
	INNER JOIN PREM_ADJ_PGM_STS ON PREM_ADJ_PGM_STS.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID
	WHERE PREM_ADJ_PERD.PREM_ADJ_ID =  @ADJNO AND PREM_ADJ_PGM.ACTV_IND = 1 
	AND PREM_ADJ_PGM_STS.PGM_PERD_STS_TYP_ID = 342
	AND PREM_ADJ_PERD.PREM_ADJ_PERD_ID = @PERDID AND COML_AGMT.ACTV_IND = 1
	AND dbo.fn_GetPolCheck(COML_AGMT.COML_AGMT_ID,PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,@TYPNUM) = 1
	return @sstring
end

go

if object_id('fn_GetPolicyList') is not null
	print 'Created function fn_GetPolicyList'
else
	print 'Failed Creating Function fn_GetPolicyList'
go

if object_id('fn_GetPolicyList') is not null
	grant exec on fn_GetPolicyList to public
go
