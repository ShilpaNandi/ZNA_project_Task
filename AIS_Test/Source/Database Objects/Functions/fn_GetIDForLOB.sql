
if exists (select 1 from sysobjects 
		where name = 'fn_GetIDForLOB' and type = 'FN')
	drop function fn_GetIDForLOB
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetIDForLOB
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description: This function retrieves the lookup id corresponding to a LOB.
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_GetIDForLOB]
   (@lob varchar(10))
returns int
--WITH SCHEMABINDING
as
begin
   return ( select lkup_id from dbo.LKUP where lkup_txt like @lob and 
lkup_typ_id in (select lkup_typ_id from dbo.LKUP_TYP where lkup_typ_nm_txt like 'LOB')
)
end



go

if object_id('fn_GetIDForLOB') is not null
	print 'Created function fn_GetIDForLOB'
else
	print 'Failed Creating function fn_GetIDForLOB'
go

if object_id('fn_GetIDForLOB') is not null
	grant exec on fn_GetIDForLOB to public
go

