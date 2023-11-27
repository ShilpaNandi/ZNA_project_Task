
if exists (select 1 from sysobjects 
		where name = 'View_LSI_Account_Entity' and type = 'V')
begin
	drop view View_LSI_Account_Entity
end
go

set ansi_nulls on
go

set ansi_warnings on
go

---------------------------------------------------------------------
-----
-----	Proc Name:	View_LSI_Account_Entity
-----
-----	Version:	SQL Server 2005
-----
-----	Created:    CSC
-----
-----	Description:   Used by Retro Replacement application.
-----	               Retrieves LSI Accounts from LSI.Account table
-----                  This view resides in the AIS Database
---------------------------------------------------------------------

create view [dbo].[View_LSI_Account_Entity]
as


select 
pkAccountID as AccountID,
[Name] as AccountName
from   LSI.dbo.Account with(nolock)
where deleted=0

go

if object_id('View_LSI_Account_Entity') is not null
	print 'Created view View_LSI_Account_Entity'
else
	print 'Failed Creating view View_LSI_Account_Entity'
go


 