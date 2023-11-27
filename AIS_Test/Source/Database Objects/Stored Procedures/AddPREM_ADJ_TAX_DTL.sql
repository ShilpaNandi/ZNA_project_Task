if exists (select 1 from sysobjects 
		where name = 'AddPREM_ADJ_TAX_DTL' and type = 'P')
	drop procedure AddPREM_ADJ_TAX_DTL
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	AddPREM_ADJ_TAX_DTL
-----
-----	Version:	SQL Server 2005

-----	Created:		CSC (Venkata Kolimi)

-----
-----	Description:	This stored procedure is used to populate the PREM_ADJ_TAX_DTL table with calculation results.
-----                   This stored procedure is used in the ModAISCalcDeductibleTax.sql to isnert the records based on the parameters passed to it
-----					while performing the calculations
-----
-----	On Exit:	
-----
-----   Created Date : 03/02/2010 (AS part of Texas tax Project)
			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------

CREATE procedure [dbo].[AddPREM_ADJ_TAX_DTL]
@prem_adj_tax_setup_id int, 
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@premium_adj_prog_id int,
@coml_agmt_id int,
@state_id int,
@lob_id int,
@dedtbl_tax_component_id int,
@tax_component_amt decimal(15,2),
@tax_rate decimal(15,8),
@tax_amt decimal(15,2),
@create_user_id int
as

begin
	set nocount on

declare @trancount int


set @trancount = @@trancount
--print @trancount
if @trancount = 0 
	begin
	    begin transaction 
	end


begin try
	--Inserting the records based on the parameters passed to it from the calling environment
	insert into PREM_ADJ_TAX_DTL
	(
		[prem_adj_tax_setup_id],
		[prem_adj_perd_id] ,
		[prem_adj_id],
		[custmr_id],
		[prem_adj_pgm_id],
		[coml_agmt_id],
		[st_id],
		[ln_of_bsn_id],
		[dedtbl_tax_cmpnt_id],
		[tax_cmpnt_amt],
		[tax_rt],
		[tax_amt],
		[crte_user_id]
	)
	values
	(
		@prem_adj_tax_setup_id,
		@premium_adj_period_id,
		@premium_adjustment_id,
		@customer_id,
		@premium_adj_prog_id,
		@coml_agmt_id,							
		@state_id,
		@lob_id,
		@dedtbl_tax_component_id,
		@tax_component_amt,
		@tax_rate,
		@tax_amt,
		@create_user_id
	)


	if @trancount = 0
		commit transaction

end try
begin catch

	if @trancount = 0  
	begin
		rollback transaction
	end

	
	declare @err_msg varchar(500),@err_ln varchar(10),
			@err_proc varchar(30),@err_no varchar(10)
	select  @err_msg = error_message(),
		    @err_no = error_number(),
		    @err_proc = error_procedure(),
			@err_ln = error_line()
	set @err_msg = '- error no.:' + isnull(@err_no, ' ') + '; procedure:' 
		+ isnull(@err_proc, ' ') + ';error line:' + isnull(@err_ln, ' ') + ';description:' + isnull(@err_msg, ' ' )
	--set @err_msg_op = @err_msg

	insert into [dbo].[APLCTN_STS_LOG]
   (
		[src_txt]
	   ,[sev_cd]
	   ,[shrt_desc_txt]
	   ,[full_desc_txt]
	   ,[custmr_id]
	   ,[prem_adj_id]
	   ,[crte_user_id]
	)
	values
    (
		'AIS Calculation Engine'
       ,'ERR'
       ,'Calculation error'
       ,'Error encountered during calculation of adjustment number: ' 
			+ convert(varchar(20),isnull(@premium_adjustment_id,0)) 
			+ ' for program number: ' 
			+ convert(varchar(20),isnull(@premium_adj_prog_id,0)) 
			+ ' associated with customer number: ' 
			+ convert(varchar(20),isnull(@customer_id,0))
			+ '. Error message: '
			+ isnull(@err_msg, ' ')
	   ,@customer_id
	   ,@premium_adjustment_id
       ,isnull(@create_user_id, 0)
	)


	select 
    error_number() AS ErrorNumber,
    error_severity() AS ErrorSeverity,
    error_state() as ErrorState,
    error_procedure() as ErrorProcedure,
    error_line() as ErrorLine,
    error_message() as ErrorMessage

	declare @err_sev varchar(10)

	select  @err_msg = error_message(),
		    @err_no = error_number(),
		    @err_proc = error_procedure(),
			@err_ln = error_line(),
			@err_sev = error_severity()

	RAISERROR (@err_msg, -- Message text.
               @err_sev, -- Severity.
               1 -- State.
               )

end catch


end

go

if object_id('AddPREM_ADJ_TAX_DTL') is not null
	print 'Created Procedure AddPREM_ADJ_TAX_DTL'
else
	print 'Failed Creating Procedure AddPREM_ADJ_TAX_DTL'
go

if object_id('AddPREM_ADJ_TAX_DTL') is not null
	grant exec on AddPREM_ADJ_TAX_DTL to public
go


