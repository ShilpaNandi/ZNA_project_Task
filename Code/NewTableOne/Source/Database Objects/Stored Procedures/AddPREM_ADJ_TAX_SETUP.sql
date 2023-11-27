if exists (select 1 from sysobjects 
		where name = 'AddPREM_ADJ_TAX_SETUP' and type = 'P')
	drop procedure AddPREM_ADJ_TAX_SETUP
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	AddPREM_ADJ_TAX_SETUP
-----
-----	Version:	SQL Server 2005
-----
-----	Created:		CSC (Venkata Kolimi)

-----	Description:	This stored procedure is used to insert records to the PREM_ADJ_TAX_SETUP table.
-----                   This stored procedure is used in the ModAISCalcDeductibleTax.sql to isnert the records based on the parameters passed to it
-----					while performing the calculations
-----	On Exit:	
-----			
-----
-----   Created Date : 03/02/2010 (AS part of Texas tax Project)

-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------

CREATE procedure [dbo].[AddPREM_ADJ_TAX_SETUP] 
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@premium_adj_prog_id int,
@ln_of_bsn_id int,
@state_id int,
@tax_typ_id int,
@state_sales_service_fee_tax decimal(15,2),
@create_user_id int,
@prem_adj_tax_setup_id_op int output
as

begin
	set nocount on

declare	@trancount	int


set @trancount = @@trancount
--print @trancount
if @trancount = 0 
	begin
	    begin transaction 
	end


begin try

		--Inserting the records based on the parameters passed to it from the calling environment
		insert into [dbo].[PREM_ADJ_TAX_SETUP]
		(
		[prem_adj_perd_id]
	   ,[prem_adj_id]
	   ,[custmr_id]
	   ,[st_id]
	   ,[prem_adj_pgm_id]
	   ,[ln_of_bsn_id]
	   ,[tax_typ_id]
	   ,[st_sls_serv_fee_tax]
	   ,[crte_user_id]
		)
		values
		(
		@premium_adj_period_id,
		@premium_adjustment_id,
		@customer_id,
		@state_id,
		@premium_adj_prog_id,
		@ln_of_bsn_id,
		@tax_typ_id,
		@state_sales_service_fee_tax,
		@create_user_id					
		)

		set @prem_adj_tax_setup_id_op = @@identity
					

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

if object_id('AddPREM_ADJ_TAX_SETUP') is not null
	print 'Created Procedure AddPREM_ADJ_TAX_SETUP'
else
	print 'Failed Creating Procedure AddPREM_ADJ_TAX_SETUP'
go

if object_id('AddPREM_ADJ_TAX_SETUP') is not null
	grant exec on AddPREM_ADJ_TAX_SETUP to public
go
