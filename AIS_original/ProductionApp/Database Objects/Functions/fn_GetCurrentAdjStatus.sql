
if exists (select 1 from sysobjects 
		where name = 'fn_GetCurrentAdjStatus' and type = 'FN')
	drop function fn_GetCurrentAdjStatus
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetCurrentAdjStatus
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Anil Thachapillil - 09/11/2008
-----
-----	Description:	Retrieves the Current Adjustment ID for a given Valuation and Customer 
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE FUNCTION fn_GetCurrentAdjStatus
(@ValDate nvarchar(12),@CustID int)
RETURNS int 
AS
BEGIN
declare @Return int

SELECT @Return = adj_sts_typ_id
FROM PREM_ADJ_STS
WHERE prem_adj_sts_id = (SELECT Max(prem_adj_sts_id) FROM PREM_ADJ
INNER JOIN PREM_ADJ_STS ON PREM_ADJ.prem_adj_id = PREM_ADJ_STS.prem_adj_id
WHERE PREM_ADJ.valn_dt = convert(VarChar(20), @ValDate, 101)
AND PREM_ADJ.reg_custmr_id=@CustID AND PREM_ADJ.fnl_invc_nbr_txt IS NULL)

return @Return
end

go

if object_id('fn_GetCurrentAdjStatus') is not null
	print 'Created function fn_GetCurrentAdjStatus'
else
	print 'Failed Creating Function fn_GetCurrentAdjStatus'
go

if object_id('fn_GetCurrentAdjStatus') is not null
	grant exec on fn_GetCurrentAdjStatus to public
go
