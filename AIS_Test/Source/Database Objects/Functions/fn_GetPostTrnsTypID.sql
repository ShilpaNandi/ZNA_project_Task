
if exists (select 1 from sysobjects 
		where name = 'fn_GetPostTrnsTypID' and type = 'FN')
	drop function fn_GetPostTrnsTypID
GO
set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetPostTrnsTypID
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		CSC (Venkata Kolimi)
-----
-----	Description:		Retrieves the PostTrnsTypID for a given State ID  from the POST_TRNS_TYP table.
-----						This function will be used in the ModAISCalcDeductibleTax.sql stoired procedure to retrieve the post Trns Type ID
-----						for the given parameter like stateID

-----   Created Date : 03/02/2010 (AS part of Texas tax Project)
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----
-----------------------------------------------------------------------------------------------------------------------
CREATE FUNCTION [dbo].[fn_GetPostTrnsTypID]
(@StateID int)
RETURNS int 
AS
BEGIN
declare @Return int,
		@StateCode varchar(10),
		@TaxDescription varchar(256),
		@trns_typ_id int

select @StateCode = attr_1_txt from dbo.LKUP
where lkup_id=@StateID

select @trns_typ_id=lkup_id from lkup 
where lkup_typ_id=28 and lkup_txt='DEDUCTIBLE TAX'

select @TaxDescription=lkup_txt from lkup
where lkup_typ_id=53 and attr_1_txt=@StateCode

select @Return=post_trns_typ_id from POST_TRNS_TYP 
where trns_typ_id=@trns_typ_id and trns_nm_txt=@TaxDescription and actv_ind=1

return @Return
end

go

if object_id('fn_GetPostTrnsTypID') is not null
	print 'Created function fn_GetPostTrnsTypID'
else
	print 'Failed Creating function fn_GetPostTrnsTypID'
go

if object_id('fn_GetPostTrnsTypID') is not null
	grant exec on fn_GetPostTrnsTypID to public
go


