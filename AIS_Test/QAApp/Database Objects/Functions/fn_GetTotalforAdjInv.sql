if exists (select 1 from sysobjects 
		where name = 'fn_GetTotalforAdjInv' and type = 'FN')
	drop function fn_GetTotalforAdjInv
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetTotalforAdjInv
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	Retrieves the Total Amount
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetTotalforAdjInv]
  (
	@ADJNO int
	   
	)
returns DECIMAL(15,2)
--WITH SCHEMABINDING
as
begin
	declare @Amount decimal(15,2)
	
	set @Amount = null
	
	
	
SELECT @Amount = round(SUM(PREM_ADJ_PERD_TOT.TOT_AMT),0)	
    FROM PREM_ADJ_PERD 
	INNER JOIN PREM_ADJ_PERD_TOT ON PREM_ADJ_PERD_TOT.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID
	AND PREM_ADJ_PERD_TOT.PREM_ADJ_ID = PREM_ADJ_PERD.PREM_ADJ_ID 
	AND PREM_ADJ_PERD_TOT.CUSTMR_ID = PREM_ADJ_PERD.CUSTMR_ID
    WHERE PREM_ADJ_PERD.PREM_ADJ_ID = @ADJNO --AND
	--


	return @Amount
end

go

if object_id('fn_GetTotalforAdjInv') is not null
	print 'Created function fn_GetTotalforAdjInv'
else
	print 'Failed Creating Function fn_GetTotalforAdjInv'
go

if object_id('fn_GetTotalforAdjInv') is not null
	grant exec on fn_GetTotalforAdjInv to public
go
