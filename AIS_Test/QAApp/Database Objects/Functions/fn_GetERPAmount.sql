if exists (select 1 from sysobjects 
		where name = 'fn_GetERPAmount' and type = 'FN')
	drop function fn_GetERPAmount
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetERPAmount
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	This function returns ERP amount
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetERPAmount]
  (	
	@PGMID int,
	@ADJID int	
  )
returns decimal(15,2)
--WITH SCHEMABINDING
as
begin
	declare @erpamount decimal(15,2)
	
	set @erpamount = 0
	
	
	select @erpamount = ernd_retro_prem_amt from prem_adj_perd where prem_adj_id = @ADJID and
	prem_adj_pgm_id = @PGMID and ernd_retro_prem_min_max_cd = 'ERP'

	return @erpamount
end

go

if object_id('fn_GetERPAmount') is not null
	print 'Created function fn_GetERPAmount'
else
	print 'Failed Creating Function fn_GetERPAmount'
go

if object_id('fn_GetERPAmount') is not null
	grant exec on fn_GetERPAmount to public
go
