
if exists (select 1 from sysobjects 
		where name = 'fn_GetStateID' and type = 'FN')
	drop function fn_GetStateID
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetStateID
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Anil Thachapillil - 09/11/2008
-----
-----	Description:		Retrieves the State ID for a given state code from the Lookup table.
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE FUNCTION fn_GetStateID
(@StateCode nvarchar(2))
RETURNS int 
AS
BEGIN
declare @Return int
select @Return = lkup_id from dbo.LKUP
where attr_1_txt=@StateCode
and lkup_typ_id in(select lkup_typ_id from lkup_typ where lkup_typ_nm_txt='STATE')


return @Return
end

go

if object_id('fn_GetStateID') is not null
	print 'Created function fn_GetStateID'
else
	print 'Failed function fn_GetStateID'
go

if object_id('fn_GetStateID') is not null
	grant exec on fn_GetStateID to public
go
