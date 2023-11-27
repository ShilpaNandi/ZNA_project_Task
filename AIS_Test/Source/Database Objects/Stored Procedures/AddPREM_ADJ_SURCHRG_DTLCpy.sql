if exists (select 1 from sysobjects 
		where name = 'AddPREM_ADJ_SURCHRG_DTLCpy' and type = 'P')
	drop procedure AddPREM_ADJ_SURCHRG_DTLCpy
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	AddPREM_ADJ_SURCHRG_DTLCpy
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	Procedure creates a copy of the PREM_ADJ_SURCHRG_DTL record.
-----	
-----	On Exit:
-----	
-----
-----	Modified:	01/05/2010  Development Team
-----                   Created SP
-----
---------------------------------------------------------------------
CREATE procedure [dbo].[AddPREM_ADJ_SURCHRG_DTLCpy]
      @select         smallint = 0, 
      @prem_adj_id    int,
      @prem_adj_perd_id    int,
      @new_prem_adj_id    int,
      @new_prem_adj_perd_id    int
     

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

			insert into PREM_ADJ_SURCHRG_DTL
			(
			 [prem_adj_perd_id]
			,[prem_adj_id]
			,[custmr_id]
			,[coml_agmt_id]
			,[prem_adj_pgm_id]
			,[st_id]
			,[ln_of_bsn_id]
			,[surchrg_cd_id]
			,[surchrg_typ_id]
			,[post_trns_typ_id]
			,[subj_paid_idnmty_amt]
			,[subj_paid_exps_amt]
			,[subj_resrv_idnmty_amt]
			,[subj_resrv_exps_amt]
			,[basic_amt]
			,[std_subj_prem_amt]
			,[ernd_retro_prem_amt]
			,[prev_biled_ernd_retro_prem_amt]
			,[retro_rslt]
			,[addn_surchrg_asses_cmpnt]
			,[tot_surchrg_asses_base]
			,[surchrg_rt]
			,[addn_rtn]
			,[tot_addn_rtn]
			,[updt_user_id]
			,[updt_dt]
			,[crte_user_id]
			,[crte_dt]
			)
			 select
			 @new_prem_adj_perd_id
			,@new_prem_adj_id
			,custmr_id
			,coml_agmt_id
			,prem_adj_pgm_id
			,st_id
			,ln_of_bsn_id
			,surchrg_cd_id
			,surchrg_typ_id
			,post_trns_typ_id
			,subj_paid_idnmty_amt
			,subj_paid_exps_amt
			,subj_resrv_idnmty_amt
			,subj_resrv_exps_amt
			,basic_amt
			,std_subj_prem_amt
			,ernd_retro_prem_amt
			,prev_biled_ernd_retro_prem_amt
			,retro_rslt
			,addn_surchrg_asses_cmpnt
			,tot_surchrg_asses_base
			,surchrg_rt
			,addn_rtn
			,tot_addn_rtn
			,NULL
			,NULL
			,crte_user_id
			,@ent_timestamp
			 from PREM_ADJ_SURCHRG_DTL
			 where prem_adj_id = @prem_adj_id
			 and prem_adj_perd_id = @prem_adj_perd_id
			 
		

	
 
	
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

if object_id('AddPREM_ADJ_SURCHRG_DTLCpy') is not null
	print 'Created Procedure AddPREM_ADJ_SURCHRG_DTLCpy'
else
	print 'Failed Creating Procedure AddPREM_ADJ_SURCHRG_DTLCpy'
go

if object_id('AddPREM_ADJ_SURCHRG_DTLCpy') is not null
	grant exec on AddPREM_ADJ_SURCHRG_DTLCpy to public
go
