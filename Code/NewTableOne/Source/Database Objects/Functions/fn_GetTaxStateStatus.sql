if exists (select 1 from sysobjects 
		where name = 'fn_GetTaxStateStatus' and type = 'FN')
	drop function fn_GetTaxStateStatus
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetTaxStateStatus
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	Check the status of the existence of state
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetTaxStateStatus]
  (
	@ADJNO int,
	@PERDID int,
	@LOBID int,
	@STID int    
	)
returns int
--WITH SCHEMABINDING
as
begin
	declare @status int
	
	set @status = 0
	
	
	
SELECT @status = case when count(PREM_ADJ_TAX_SETUP_ID) > 0  then 1 else 0 end from PREM_ADJ_TAX_SETUP
 where prem_adj_id = @ADJNO and prem_adj_perd_id = @PERDID and ln_of_bsn_id = @LOBID and st_id = @STID
    

	return @status
end

go

if object_id('fn_GetTaxStateStatus') is not null
	print 'Created function fn_GetTaxStateStatus'
else
	print 'Failed Creating Function fn_GetTaxStateStatus'
go

if object_id('fn_GetTaxStateStatus') is not null
	grant exec on fn_GetTaxStateStatus to public
go
