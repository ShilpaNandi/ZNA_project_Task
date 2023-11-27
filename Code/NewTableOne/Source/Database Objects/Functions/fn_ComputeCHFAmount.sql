
if exists (select 1 from sysobjects 
		where name = 'fn_ComputeCHFAmount' and type = 'FN')
	drop function fn_ComputeCHFAmount
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_ComputeCHFAmount
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description: Computes CHF amount.
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_ComputeCHFAmount]
   (@ClaimNum int,
    @ClaimRate int)
returns int
--WITH SCHEMABINDING
as
begin
   return ( @ClaimNum * @ClaimRate )
end


go

if object_id('fn_ComputeCHFAmount') is not null
	print 'Created function fn_ComputeCHFAmount'
else
	print 'Failed function Procedure fn_ComputeCHFAmount'
go

if object_id('fn_ComputeCHFAmount') is not null
	grant exec on fn_ComputeCHFAmount to public
go




