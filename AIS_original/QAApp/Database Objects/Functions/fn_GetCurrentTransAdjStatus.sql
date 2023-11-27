
if exists (select 1 from sysobjects 
		where name = 'fn_GetCurrentTransAdjStatus' and type = 'FN')
	drop function fn_GetCurrentTransAdjStatus
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetCurrentTransAdjStatus
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Anil Thachapillil - 09/11/2008
-----
-----	Description:	Retrieves the Current adj_sts_typ_id for a given Valuation and Customer 
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE FUNCTION fn_GetCurrentTransAdjStatus
(@prem_adj_id nvarchar(12),@CustID int)
RETURNS int 
AS
BEGIN
declare @Return int

SELECT @Return = adj_sts_typ_id
FROM PREM_ADJ_STS
WHERE prem_adj_sts_id = (SELECT Max(prem_adj_sts_id) FROM PREM_ADJ
INNER JOIN PREM_ADJ_STS ON (PREM_ADJ.prem_adj_id = PREM_ADJ_STS.prem_adj_id AND PREM_ADJ.fnl_invc_nbr_txt IS NOT NULL)
WHERE PREM_ADJ.prem_adj_id = @prem_adj_id  
AND PREM_ADJ.reg_custmr_id=@CustID)

return @Return
end

go

if object_id('fn_GetCurrentTransAdjStatus') is not null
	print 'Created function fn_GetCurrentTransAdjStatus'
else
	print 'Failed Creating Function fn_GetCurrentTransAdjStatus'
go

if object_id('fn_GetCurrentTransAdjStatus') is not null
	grant exec on fn_GetCurrentTransAdjStatus to public
go
