if exists (select 1 from sysobjects 
		where name = 'vwLSI_CHF_Interface' and type = 'V')
begin
	drop view vwLSI_CHF_Interface
end
go

set ansi_nulls on
go

set ansi_warnings on
go 

SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[vwLSI_CHF_Interface]
AS

select * from [LSI_CHF_UAT1].[dbo].[vwAIS_CHF_Interface]


GO

if object_id('vwLSI_CHF_Interface') is not null
        print 'Created Function vwLSI_CHF_Interface'
else
        print 'Failed Creating view vwLSI_CHF_Interface'
go

if object_id('vwLSI_CHF_Interface') is not null
        grant select on vwLSI_CHF_Interface to  public
go

