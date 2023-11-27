if exists (select 1 from sysobjects 
		where name = 'fn_GetDEPNumberPolicy' and type = 'FN')
	drop function fn_GetDEPNumberPolicy
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetDEPNumberPolicy
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	Returns number of duplicate state records in parameter dtl table
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetDEPNumberPolicy]
  (
	@ADJID int,
	@PGMID int,
	@CUSTID int,
	@STID int    
	)
returns int
--WITH SCHEMABINDING
as
begin
	declare @cnt int	
	set @cnt = 0	
	
	select  @cnt = count(armis_los_pol_id) from armis_los_pol where prem_adj_id = @ADJID 
	and prem_adj_pgm_id = @PGMID and custmr_id = @CUSTID and st_id = @STID and actv_ind = 1

	if @cnt = 0 
		set @cnt = 1

	return @cnt
end

go

if object_id('fn_GetDEPNumberPolicy') is not null
	print 'Created function fn_GetDEPNumberPolicy'
else
	print 'Failed Creating Function fn_GetDEPNumberPolicy'
go

if object_id('fn_GetDEPNumberPolicy') is not null
	grant exec on fn_GetDEPNumberPolicy to public
go
