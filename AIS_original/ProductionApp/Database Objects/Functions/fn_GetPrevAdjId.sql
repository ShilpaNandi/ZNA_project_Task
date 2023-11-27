
if exists (select 1 from sysobjects 
		where name = 'fn_GetPrevAdjId' and type = 'FN')
	drop function fn_GetPrevAdjId
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetPrevAdjId
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description: This function retrieves the Related Adjustment Id
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_GetPrevAdjId]
   (@adjid int,
	@FLAG int)
returns nvarchar(30)
--WITH SCHEMABINDING
as
begin
   return (select CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), DRFT_INVC_DT,101)
 WHEN 2 THEN CONVERT(NVARCHAR(30), FNL_INVC_DT,101)
ELSE  CONVERT(NVARCHAR(30), DRFT_INVC_DT,101) END FROM PREM_ADJ
 WHERE PREM_ADJ_ID = @adjid AND ADJ_RRSN_IND = 1)
end



go

if object_id('fn_GetPrevAdjId') is not null
	print 'Created function fn_GetPrevAdjId'
else
	print 'Failed Creating function fn_GetPrevAdjId'
go

if object_id('fn_GetPrevAdjId') is not null
	grant exec on fn_GetPrevAdjId to public
go

