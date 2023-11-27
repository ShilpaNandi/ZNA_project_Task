if exists (select 1 from sysobjects 
		where name = 'AddPREM_ADJ_SURCHRG_DTL' and type = 'P')
	drop procedure AddPREM_ADJ_SURCHRG_DTL
go

set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	AddPREM_ADJ_SURCHRG_DTL
-----				
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to populate the table PREM_ADJ_SURCHRG_DTL.
-----
-----	On Exit:	
-----			
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----               07/19/2010	Venkat Kolimi
-----				Created Procedure

---------------------------------------------------------------------

CREATE procedure [dbo].[AddPREM_ADJ_SURCHRG_DTL]
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@com_agm_id int,
@premium_adj_prog_id int,
@state_id int,
@ln_of_bsn_id int,
@surcharge_cd_id int,
@surcharge_type_id int,
@post_trns_typ_id int,
@subj_paid_idnmty_amt decimal(15,2),
@subj_paid_exps_amt decimal(15,2),
@subj_resrv_idnmty_amt decimal(15,2),
@subj_resrv_exps_amt decimal(15,2),
@basic_amt decimal(15,2),
@std_subj_prem_amt decimal(15,2),
@adj_incur_ernd_retro_prem_amt decimal(15,2),
@prev_ernd_retro_prem_amt decimal(15,2),
@retro_result decimal(15,2),
@addn_surchrg_asses_cmpnt decimal(15,2),
@tot_surchrg_asses_base decimal(15,2),
@surchrg_rt decimal(15,8),
@addn_rtn decimal(15,2),
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
	
	
	

	insert into PREM_ADJ_SURCHRG_DTL
					(
					prem_adj_perd_id,
					prem_adj_id,
					custmr_id,
					coml_agmt_id,
					prem_adj_pgm_id,
					st_id,
					ln_of_bsn_id,
					surchrg_cd_id,
					surchrg_typ_id,
					post_trns_typ_id,
					subj_paid_idnmty_amt,
					subj_paid_exps_amt,
					subj_resrv_idnmty_amt,
					subj_resrv_exps_amt,
					basic_amt,
					std_subj_prem_amt,
					ernd_retro_prem_amt,
					prev_biled_ernd_retro_prem_amt,
					retro_rslt,
					addn_surchrg_asses_cmpnt,
					tot_surchrg_asses_base,
					surchrg_rt,
					addn_rtn,
					updt_user_id,
					updt_dt,
					crte_user_id,
					crte_dt
					)
					VALUES
					(
					@premium_adj_period_id,
					@premium_adjustment_id,
					@customer_id,
					@com_agm_id,
					@premium_adj_prog_id,
					@state_id,
					@ln_of_bsn_id,
					@surcharge_cd_id,
					@surcharge_type_id,
					@post_trns_typ_id,
					@subj_paid_idnmty_amt,
					@subj_paid_exps_amt,
					@subj_resrv_idnmty_amt,
					@subj_resrv_exps_amt,
					@basic_amt,
					@std_subj_prem_amt,
					@adj_incur_ernd_retro_prem_amt,
					@prev_ernd_retro_prem_amt,
					@retro_result,
					@addn_surchrg_asses_cmpnt,
					@tot_surchrg_asses_base,
					@surchrg_rt,
					@addn_rtn,
					NULL,
					NULL,
					@create_user_id,
					getdate()
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

if object_id('AddPREM_ADJ_SURCHRG_DTL') is not null
	print 'Created Procedure AddPREM_ADJ_SURCHRG_DTL'
else
	print 'Failed Creating Procedure AddPREM_ADJ_SURCHRG_DTL'
go

if object_id('AddPREM_ADJ_SURCHRG_DTL') is not null
	grant exec on AddPREM_ADJ_SURCHRG_DTL to public
go
