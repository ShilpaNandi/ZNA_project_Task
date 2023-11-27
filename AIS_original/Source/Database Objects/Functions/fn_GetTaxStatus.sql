if exists (select 1 from sysobjects 
		where name = 'fn_GetTaxStatus' and type = 'FN')
	drop function fn_GetTaxStatus
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetTaxStatus
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
CREATE function [dbo].[fn_GetTaxStatus]
  (
	@ADJNO int,
	@PERDID int	  
	)
returns int
--WITH SCHEMABINDING
as
begin
	declare @status int
	
	set @status = 0
	
	
	
SELECT @status = case when count(PREM_ADJ_TAX_SETUP_ID) > 0  then 1 else 0 end from PREM_ADJ_TAX_SETUP
 where prem_adj_id = @ADJNO and prem_adj_perd_id = @PERDID 
    

	return @status
end

go

if object_id('fn_GetTaxStatus') is not null
	print 'Created function fn_GetTaxStatus'
else
	print 'Failed Creating Function fn_GetTaxStatus'
go

if object_id('fn_GetTaxStatus') is not null
	grant exec on fn_GetTaxStatus to public
go
