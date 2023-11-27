
if exists (select 1 from sysobjects 
		where name = 'AddPREM_ADJ_PARMET_DTL' and type = 'P')
	drop procedure AddPREM_ADJ_PARMET_DTL
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	AddPREM_ADJ_PARMET_DTL
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to populate the output tables for LBA with calculation results.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

----- TODO: Need to handle scenarios when master account has related sub-accounts
----- possibly in driver stored procedures.
---------------------------------------------------------------------

create procedure [dbo].[AddPREM_ADJ_PARMET_DTL]
@prem_adj_parmet_setup_id int, 
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@premium_adj_prog_id int,
@coml_agmt_id int,
@state_id int,
@lob_id int,
@loss_amt decimal(15,2),
@paid_loss decimal(15,2),
@paid_alae decimal(15,2),
@resv_loss decimal(15,2),
@resv_alae decimal(15,2),
@lba_rt decimal(15,8),
@lba_amt decimal(15,2),
@lcf_rt decimal(15,8),
@lcf_amt decimal(15,2),
@ldf_rt decimal(15,8),
@ldf_amt decimal(15,2),
@total_amt decimal(15,2),
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

	insert into PREM_ADJ_PARMET_DTL
	(
		[prem_adj_parmet_setup_id],
		[prem_adj_perd_id] ,
		[prem_adj_id],
		[custmr_id],
		[prem_adj_pgm_id],
		[coml_agmt_id],
		[st_id],
		[ln_of_bsn_id],
		[los_amt],
		[paid_los_amt],
		[paid_aloc_los_adj_exps_amt],
		[resrv_los_amt],
		[resrv_aloc_los_adj_exps_amt],
		[los_base_asses_rt],
		[los_base_asses_amt],
		[los_conv_fctr_rt],
		[los_conv_fctr_amt],
		[los_dev_fctr_rt],
		[los_dev_fctr_amt],
		[tot_amt],
		[crte_user_id]
	)
	values
	(
		@prem_adj_parmet_setup_id,
		@premium_adj_period_id,
		@premium_adjustment_id,
		@customer_id,
		@premium_adj_prog_id,
		@coml_agmt_id,							
		@state_id,
		@lob_id,
		@loss_amt,
		@paid_loss,
		@paid_alae,
		@resv_loss,
		@resv_alae,
		@lba_rt,
		@lba_amt,
		@lcf_rt,
		@lcf_amt,
		@ldf_rt,
		@ldf_amt,
		@total_amt,
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

if object_id('AddPREM_ADJ_PARMET_DTL') is not null
	print 'Created Procedure AddPREM_ADJ_PARMET_DTL'
else
	print 'Failed Creating Procedure AddPREM_ADJ_PARMET_DTL'
go

if object_id('AddPREM_ADJ_PARMET_DTL') is not null
	grant exec on AddPREM_ADJ_PARMET_DTL to public
go






