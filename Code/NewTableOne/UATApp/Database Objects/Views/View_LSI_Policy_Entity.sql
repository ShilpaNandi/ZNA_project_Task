
if exists (select 1 from sysobjects 
		where name = 'View_LSI_Policy_Entity' and type = 'V')
begin
	drop view View_LSI_Policy_Entity
end
go

set ansi_nulls on
go

set ansi_warnings on
go

---------------------------------------------------------------------
-----
-----	Proc Name:	View_LSI_Policy_Entity
-----
-----	Version:	SQL Server 2005
-----
-----	Created:   	CSC
-----
-----	Description:   Used by Calc Engine module to retrieve PLB data from
-----	               LSI database.  This view resides in the AIS Database
---------------------------------------------------------------------

create view [dbo].[View_LSI_Policy_Entity]
as

select * from LSI.dbo.Policy with(nolock)

go

if object_id('View_LSI_Policy_Entity') is not null
	print 'Created view View_LSI_Policy_Entity'
else
	print 'Failed Creating view View_LSI_Policy_Entity'
go


 