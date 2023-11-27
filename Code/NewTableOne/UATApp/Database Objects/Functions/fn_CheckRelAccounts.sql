
if exists (select 1 from sysobjects 
		where name = 'fn_CheckRelAccounts' and type = 'FN')
	drop function fn_CheckRelAccounts
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_CheckRelAccounts
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	Check whether it is having the related accounts or not
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE FUNCTION fn_CheckRelAccounts
(@custmrid int)

RETURNS int 
AS
BEGIN

declare @Return int
set @Return = 0

select @Return = 1 from custmr
where custmr_rel_id = @custmrid
and custmr_rel_actv_ind = 1


return @Return
end

go

if object_id('fn_CheckRelAccounts') is not null
	print 'Created function fn_CheckRelAccounts'
else
	print 'Failed function fn_CheckRelAccounts'
go

if object_id('fn_CheckRelAccounts') is not null
	grant exec on fn_CheckRelAccounts to public
go
