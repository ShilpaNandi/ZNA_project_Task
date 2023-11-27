if exists (select 1 from sysobjects 
		where name = 'fn_Retrieve_State_Sales_Service_Fee_Tax_Amt' and type = 'FN')
	drop function fn_Retrieve_State_Sales_Service_Fee_Tax_Amt
GO

set ansi_nulls off
GO

set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_Retrieve_State_Sales_Service_Fee_Tax_Amt
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		CSC (Venkata Kolimi)
-----
-----	Description: This retrieves the state_sales_service_fee_tax amount from ILRF Tax Setup table for the passed input parameters.
-----				 This function will be used in the ModAISCalcDeductibleTax.sql stoired procedure to retrieve the state sales service fee amount
-----                for the given parameters like custmer,program perid,tax type and Line of Business.
-----
-----
-----   Created Date : 03/02/2010 (AS part of Texas tax Project)

-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----
-----              03/22/2010- By Venkata Kolimi
-----              return type changed to decimal(15,2) insted of decimal(15,8)
----			   The above change fix the TFS Bug ID:11435 


---------------------------------------------------------------------
CREATE function [dbo].[fn_Retrieve_State_Sales_Service_Fee_Tax_Amt]
   (
	@p_cust_id int,
	@p_prem_adj_pgm_id int,
	@p_tax_typ_id int,
	@p_ln_of_bsn_id int
	)
returns decimal(15,2)
as
begin
	declare @result decimal(15,2)

	select @result=tax_amt 
	from 
	INCUR_LOS_REIM_FUND_TAX_SETUP 
	where 
	prem_adj_pgm_id=@p_prem_adj_pgm_id 
	and tax_typ_id=@p_tax_typ_id 
	and ln_of_bsn_id=@p_ln_of_bsn_id
	and actv_ind=1

    return @result
end


go

if object_id('fn_Retrieve_State_Sales_Service_Fee_Tax_Amt') is not null
	print 'Created function fn_Retrieve_State_Sales_Service_Fee_Tax_Amt'
else
	print 'Failed function Procedure fn_Retrieve_State_Sales_Service_Fee_Tax_Amt'
go

if object_id('fn_Retrieve_State_Sales_Service_Fee_Tax_Amt') is not null
	grant exec on fn_Retrieve_State_Sales_Service_Fee_Tax_Amt to public
go



