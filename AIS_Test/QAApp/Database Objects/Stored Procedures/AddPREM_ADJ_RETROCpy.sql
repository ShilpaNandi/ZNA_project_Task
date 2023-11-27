
if exists (select 1 from sysobjects 
                where name = 'AddPREM_ADJ_RETROCpy' and type = 'P')
        drop procedure AddPREM_ADJ_RETROCpy
go
 
set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	AddPREM_ADJ_RETROCpy
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	Procedure creates a copy of the PREM_ADJ_RETRO record.
-----	
-----	On Exit:
-----	
-----
-----	Modified:	11/01/2008  Development Team
-----                   Created SP
-----
---------------------------------------------------------------------
create procedure AddPREM_ADJ_RETROCpy
      @select         smallint = 0, 
      @prem_adj_id    int,
      @prem_adj_perd_id    int,
      @prem_adj_retro_id    int,
      @new_prem_adj_id    int,
      @new_prem_adj_perd_id    int,
      @identity       int output

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

	insert into PREM_ADJ_RETRO
	(
            
            prem_adj_perd_id
           ,prem_adj_id
           ,custmr_id
           ,coml_agmt_id
           ,prem_adj_pgm_id
           ,subj_depst_prem_amt
           ,non_subj_audt_prem_amt
           ,non_subj_depst_prem_amt
           ,paid_los_bil_amt
           ,prev_ernd_retro_prem_amt
           ,misc_amt
           ,ky_tot_due_amt
           ,or_tot_due_amt
           ,rsdl_mkt_load_tot_amt
           ,invc_amt
           ,peo_pay_in_amt
           ,adj_cash_flw_ben_amt
           ,post_idnmty_amt
           ,post_resrv_idnmty_amt
           ,aries_tot_amt
           ,updt_user_id
           ,updt_dt
           ,crte_user_id
           ,crte_dt
        )
        select
            @new_prem_adj_perd_id
           ,@new_prem_adj_id
           ,custmr_id
           ,coml_agmt_id
           ,prem_adj_pgm_id
           ,subj_depst_prem_amt
           ,non_subj_audt_prem_amt
           ,non_subj_depst_prem_amt
           ,paid_los_bil_amt
           ,prev_ernd_retro_prem_amt
           ,misc_amt
           ,ky_tot_due_amt
           ,or_tot_due_amt
           ,rsdl_mkt_load_tot_amt
           ,invc_amt
           ,peo_pay_in_amt
           ,adj_cash_flw_ben_amt
           ,post_idnmty_amt
           ,post_resrv_idnmty_amt
           ,aries_tot_amt
           ,NULL
           ,NULL
           ,crte_user_id
           ,@ent_timestamp
     from PREM_ADJ_RETRO
     where prem_adj_id = @prem_adj_id
     and prem_adj_perd_id = @prem_adj_perd_id
     and prem_adj_retro_id = @prem_adj_retro_id
 
	select  @identity = @@identity
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

if object_id('AddPREM_ADJ_RETROCpy') is not null
        print 'Created Procedure AddPREM_ADJ_RETROCpy'
else
        print 'Failed Creating Procedure AddPREM_ADJ_RETROCpy'
go

if object_id('AddPREM_ADJ_RETROCpy') is not null
        grant exec on AddPREM_ADJ_RETROCpy to  public
go
 
