
if exists (select 1 from sysobjects 
                where name = 'AddPREM_ADJ_RETRO_DTLCpy' and type = 'P')
        drop procedure AddPREM_ADJ_RETRO_DTLCpy
go
 
set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	AddPREM_ADJ_RETRO_DTLCpy
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	Procedure creates a copy of the PREM_ADJ_RETRO_DTL record.
-----	
-----	On Exit:
-----	
-----
-----	Modified:	11/01/2008  Development Team
-----                   Created SP
-----
---------------------------------------------------------------------
create procedure AddPREM_ADJ_RETRO_DTLCpy
      @select         smallint = 0, 
      @prem_adj_id    int,
      @prem_adj_perd_id    int,
      @prem_adj_retro_id    int,
      @new_prem_adj_id    int,
      @new_prem_adj_perd_id    int,
      @new_prem_adj_retro_id    int

as
declare   @error      int,
          @trancount  int,
          @ent_timestamp  datetime


select    @trancount  = @@trancount,
          @ent_timestamp = getdate( )

if @trancount = 0
begin 
	begin transaction 
end 
 
begin try

	insert into PREM_ADJ_RETRO_DTL
	(
            
            prem_adj_retro_id
           ,prem_adj_perd_id
           ,prem_adj_id
           ,custmr_id
           ,coml_agmt_id
           ,prem_adj_pgm_id
           ,st_id
           ,ln_of_bsn_id
           ,subj_paid_idnmty_amt
           ,subj_paid_exps_amt
           ,subj_resrv_idnmty_amt
           ,subj_resrv_exps_amt
           ,prev_subj_paid_idnmty_amt
           ,prev_subj_resrv_idnmty_amt
           ,adj_dedtbl_wrk_comp_los_amt
           ,basic_amt
           ,clm_hndl_fee_amt
           ,los_base_asessment_amt
           ,los_conv_fctr_amt
           ,los_conv_fctr_rt
           ,non_conv_fee_amt
           ,prem_tax_amt
           ,othr_amt
           ,incur_ernd_retro_prem_amt
           ,adj_incur_ernd_retro_prem_amt
           ,paid_ernd_retro_prem_amt
           ,adj_paid_ernd_retro_prem_amt
           ,cash_flw_ben_amt
           ,prior_cash_flw_ben_amt
           ,exc_los_prem_amt
           ,los_dev_resrv_amt
           ,std_subj_prem_amt
           ,ernd_retro_prem_amt
           ,ky_or_tax_asses_amt
           ,ky_or_prev_tax_asses_amt
           ,rsdl_mkt_load_basic_fctr_rt
           ,rsdl_mkt_load_fctr_rt
           ,rsdl_mkt_load_ernd_amt
           ,rsdl_mkt_load_prev_amt
           ,rsdl_mkt_load_tot_amt
           ,rsdl_mkt_load_paid_amt
           ,ky_or_tot_due_amt
           ,biled_ernd_retro_prem_amt
           ,adj_cash_flw_ben_amt
           ,prev_biled_ernd_retro_prem_amt
           ,prem_asses_amt
           ,prev_std_subj_prem_amt
           ,cesar_cd_tot_amt
           ,updt_user_id
           ,updt_dt
           ,crte_user_id
           ,crte_dt
        )
        select
            @new_prem_adj_retro_id
           ,@new_prem_adj_perd_id
           ,@new_prem_adj_id
           ,custmr_id
           ,coml_agmt_id
           ,prem_adj_pgm_id
           ,st_id
           ,ln_of_bsn_id
           ,subj_paid_idnmty_amt
           ,subj_paid_exps_amt
           ,subj_resrv_idnmty_amt
           ,subj_resrv_exps_amt
           ,prev_subj_paid_idnmty_amt
           ,prev_subj_resrv_idnmty_amt
           ,adj_dedtbl_wrk_comp_los_amt
           ,basic_amt
           ,clm_hndl_fee_amt
           ,los_base_asessment_amt
           ,los_conv_fctr_amt
           ,los_conv_fctr_rt
           ,non_conv_fee_amt
           ,prem_tax_amt
           ,othr_amt
           ,incur_ernd_retro_prem_amt
           ,adj_incur_ernd_retro_prem_amt
           ,paid_ernd_retro_prem_amt
           ,adj_paid_ernd_retro_prem_amt
           ,cash_flw_ben_amt
           ,prior_cash_flw_ben_amt
           ,exc_los_prem_amt
           ,los_dev_resrv_amt
           ,std_subj_prem_amt
           ,ernd_retro_prem_amt
           ,ky_or_tax_asses_amt
           ,ky_or_prev_tax_asses_amt
           ,rsdl_mkt_load_basic_fctr_rt
           ,rsdl_mkt_load_fctr_rt
           ,rsdl_mkt_load_ernd_amt
           ,rsdl_mkt_load_prev_amt
           ,rsdl_mkt_load_tot_amt
           ,rsdl_mkt_load_paid_amt
           ,ky_or_tot_due_amt
           ,biled_ernd_retro_prem_amt
           ,adj_cash_flw_ben_amt
           ,prev_biled_ernd_retro_prem_amt
           ,prem_asses_amt
           ,prev_std_subj_prem_amt
           ,cesar_cd_tot_amt
           ,NULL
           ,NULL
           ,crte_user_id
           ,@ent_timestamp
     from PREM_ADJ_RETRO_DTL
     where prem_adj_id = @prem_adj_id
     and prem_adj_perd_id = @prem_adj_perd_id
     and prem_adj_retro_id = @prem_adj_retro_id
 
       if @trancount = 0
       begin
                commit transaction 
       end
end try
begin catch

  if @trancount = 0
  begin
        rollback transaction 
  end

	declare @err_sev varchar(10), 
	        @err_msg varchar(500), 
	        @err_no varchar(10) 


  select 
  error_number() AS ErrorNumber,
  error_severity() AS ErrorSeverity,
  error_state() as ErrorState,
  error_procedure() as ErrorProcedure,
  error_line() as ErrorLine,
  error_message() as ErrorMessage


	select  @err_msg = error_message(),
	        @err_no = error_number(),
		    @err_sev = error_severity()

		RAISERROR ( @err_msg, -- Message text. 
	                @err_sev, -- Severity. 
	                1 -- State. 
	               )
end catch

go

if object_id('AddPREM_ADJ_RETRO_DTLCpy') is not null
        print 'Created Procedure AddPREM_ADJ_RETRO_DTLCpy'
else
        print 'Failed Creating Procedure AddPREM_ADJ_RETRO_DTLCpy'
go

if object_id('AddPREM_ADJ_RETRO_DTLCpy') is not null
        grant exec on AddPREM_ADJ_RETRO_DTLCpy to  public
go
 
