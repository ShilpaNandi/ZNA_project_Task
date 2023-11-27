
if exists (select 1 from sysobjects 
		where name = 'fn_AISConvertToMoney' and type = 'FN')
	drop function fn_AISConvertToMoney
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		[fn_AISConvertToMoney]
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Anil Thachapillil
-----
-----	Description:	This function converts char to money. Helps prevent errors associated with the conversion.
-----
---------------------------------------------------------------------
create function [dbo].[fn_AISConvertToMoney]
(
    @amt VARCHAR(64)  
)  
RETURNS money  
BEGIN  
    IF RIGHT(@amt, 1) = '-'  -- Last positing holds the sign
        set  @amt = '-' + SUBSTRING(@amt, 1, (LEN(@amt)-1))  

return @amt
 
END  
GO  
 
if object_id('fn_AISConvertToMoney') is not null
	print 'Created function fn_AISConvertToMoney'
else
	print 'Failed Creating function fn_AISConvertToMoney'
go

if object_id('fn_AISConvertToMoney') is not null
	grant exec on fn_AISConvertToMoney to public
go

