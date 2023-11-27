if exists (select 1 from sysobjects 
		where name = 'fn_GetSurchrgPostTrnsTypID' and type = 'FN')
	drop function fn_GetSurchrgPostTrnsTypID
GO
set ansi_nulls off
GO
set quoted_identifier on
GO


---------------------------------------------------------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetSurchrgPostTrnsTypID
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		CSC(venkat Kolimi)
-----
-----	Description:		Retrieves the PostTrnsTypID for a given Surcharge Code  from the POST_TRNS_TYP table.
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----
-----
-----------------------------------------------------------------------------------------------------------------------
CREATE FUNCTION [dbo].[fn_GetSurchrgPostTrnsTypID]
(
@SurchargeCode varchar(4)
)
RETURNS int 
AS
BEGIN
declare @Return int,
		@SurchargeDescription varchar(256),
		@Surcharge_Lkup_Typ_id int,
		@trns_typ_id int

select @Surcharge_Lkup_Typ_id=lkup_typ_id from lkup_typ where lkup_typ_nm_txt='SURCHARGES AND ASSESSMENTS'

select @trns_typ_id=lkup_id from lkup 
where lkup_typ_id=28 and lkup_txt='SURCHARGES AND ASSESSMENTS'

select @SurchargeDescription=lkup_txt from lkup
where lkup_typ_id=@Surcharge_Lkup_Typ_id and attr_1_txt=@SurchargeCode

select @Return=post_trns_typ_id from POST_TRNS_TYP 
where trns_typ_id=@trns_typ_id and trns_nm_txt=@SurchargeDescription and actv_ind=1

return @Return
end

go

if object_id('fn_GetSurchrgPostTrnsTypID') is not null
	print 'Created function fn_GetSurchrgPostTrnsTypID'
else
	print 'Failed Creating function fn_GetSurchrgPostTrnsTypID'
go

if object_id('fn_GetSurchrgPostTrnsTypID') is not null
	grant exec on fn_GetSurchrgPostTrnsTypID to public
go
