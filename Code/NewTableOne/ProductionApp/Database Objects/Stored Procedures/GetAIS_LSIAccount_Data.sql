
if exists (select 1 from sysobjects 
		where name = 'GetAIS_LSIAccount_Data' and type = 'P')
	drop procedure GetAIS_LSIAccount_Data
go

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		GetAIS_LSIAccount_Data
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Anil Thachapillil - 08/19/2008
-----
-----	Description:	Retrieves LSI Account Details From the LSI.Account table
-----
-----	Modified:	
-----
---------------------------------------------------------------------

Create procedure [dbo].[GetAIS_LSIAccount_Data]
as

declare	@error  	int

SELECT  * FROM View_LSI_Account_Entity ORDER BY AccountName

select @error = @@error

return @error 
go

if object_id('GetAIS_LSIAccount_Data') is not null
	print 'Created Procedure GetAIS_LSIAccount_Data'
else
	print 'Failed Creating Procedure GetAIS_LSIAccount_Data'
go

if object_id('GetAIS_LSIAccount_Data') is not null
	grant exec on GetAIS_LSIAccount_Data to public
go




