if exists (select 1 from sysobjects 
		where name = 'fn_RetrieveTax_rt' and type = 'FN')
	drop function fn_RetrieveTax_rt
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_RetrieveTax_rt
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		CSC (Venkata Kolimi)
-----
-----	Description: This retrieves the Tax rate from setup for the passed input parameters.
-----				 This function will be used in the ModAISCalcDeductibleTax.sql stoired procedure to retrieve the Tax Rate
-----                for the given parameters like state,com_agmt_id,component_id,tax type and Line of Business.
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----              03/29/2010- By Venkata Kolimi
-----              Added "and dedtble_tax_cmpnt_id=@componentid" in the query to filter the results based on the @componentid while retrieving the Tax Rate

---------------------------------------------------------------------
CREATE function [dbo].[fn_RetrieveTax_rt]
   (
	@ln_of_bsn_id int,
	@state_id int,
	@tax_typ_id int,
	@com_agm_id int,
	@component_id int
	)
returns decimal(15, 8)
as
begin
	declare @tax_rt decimal(15,8)
			

	
	select @tax_rt=tax_rt from DEDTBL_TAX_SETUP
					where ln_of_bsn_id=@ln_of_bsn_id
					and st_id=@state_id 
					and tax_typ_id=@tax_typ_id
					and pol_eff_dt=(select max(pol_eff_dt)	from dbo.DEDTBL_TAX_SETUP where pol_eff_dt <=
					(
					select
					pol_eff_dt
					from
					dbo.COML_AGMT
					where coml_agmt_id = @com_agm_id
					)
					and ln_of_bsn_id=@ln_of_bsn_id
					and st_id=@state_id 
					and tax_typ_id=@tax_typ_id
					and dedtbl_tax_cmpnt_id=@component_id
					and actv_ind=1
					and (select Substring(lkup_txt,1,2) from lkup where lkup_id=@tax_typ_id)=(select attr_1_txt from lkup where lkup_id=@state_id)
					)		
					and actv_ind=1
					and dedtbl_tax_cmpnt_id=@component_id
					and (select Substring(lkup_txt,1,2) from lkup where lkup_id=@tax_typ_id)=(select attr_1_txt from lkup where lkup_id=@state_id)

	set @tax_rt = isnull(@tax_rt,0)

   return @tax_rt
end

go

if object_id('fn_RetrieveTax_rt') is not null
	print 'Created function fn_RetrieveTax_rt'
else
	print 'Failed Creating Function fn_RetrieveTax_rt'
go

if object_id('fn_RetrieveTax_rt') is not null
	grant exec on fn_GetPolicyList to public
go
