if exists (select 1 from sysobjects 
		where name = 'fn_GetTexasTaxPrevAmt' and type = 'FN')
	drop function fn_GetTexasTaxPrevAmt
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetTexasTaxPrevAmt
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	Retrieves the Tax Previously Required
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetTexasTaxPrevAmt]
  (
	@ADJNO int,
	@PERDID int    
	)
returns decimal(15,2)
--WITH SCHEMABINDING
as
begin
	declare @prevamt decimal(15,2)
	
	set @prevamt = 0
	
	
	
SELECT @prevamt = sum(adj_prior_yy_amt)	
    FROM PREM_ADJ_LOS_REIM_FUND_POST_TAX WHERE PREM_ADJ_LOS_REIM_FUND_POST_TAX.PREM_ADJ_ID = @ADJNO AND
	PREM_ADJ_LOS_REIM_FUND_POST_TAX.PREM_ADJ_PERD_ID = @PERDID 


	return @prevamt
end

go

if object_id('fn_GetTexasTaxPrevAmt') is not null
	print 'Created function fn_GetTexasTaxPrevAmt'
else
	print 'Failed Creating Function fn_GetTexasTaxPrevAmt'
go

if object_id('fn_GetTexasTaxPrevAmt') is not null
	grant exec on fn_GetTexasTaxPrevAmt to public
go
