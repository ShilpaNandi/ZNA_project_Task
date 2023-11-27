
if exists (select 1 from sysobjects 
		where name = 'fn_CountOfStringInstances' and type = 'FN')
	drop function fn_CountOfStringInstances
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_CountOfStringInstances
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description:	This function  determines the formula section as applicable for an identifier. Replica of original function
-----					
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_CountOfStringInstances]
  ( @pInput VARCHAR(8000), @pSearchString VARCHAR(100) )
RETURNS INT
BEGIN

    RETURN (LEN(@pInput) - 
            LEN(REPLACE(@pInput, @pSearchString, ''))) /
            LEN(@pSearchString)

END
GO

if object_id('fn_CountOfStringInstances') is not null
	print 'Created function fn_CountOfStringInstances'
else
	print 'Failed Creating function fn_CountOfStringInstances'
go

if object_id('fn_CountOfStringInstances') is not null
	grant exec on fn_CountOfStringInstances to public
go


