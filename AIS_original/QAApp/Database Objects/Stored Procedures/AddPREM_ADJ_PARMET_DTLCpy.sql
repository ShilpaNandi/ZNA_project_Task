
if exists (select 1 from sysobjects 
                where name = 'AddPREM_ADJ_PARMET_DTLCpy' and type = 'P')
        drop procedure AddPREM_ADJ_PARMET_DTLCpy
go
 
set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	AddPREM_ADJ_PARMET_DTLCpy
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	Procedure creates a copy of the PREM_ADJ_PARMET_DTL record.
-----	
-----	On Exit:
-----	
-----
-----	Modified:	11/01/2008  Development Team
-----                   Created SP
-----
---------------------------------------------------------------------
create procedure AddPREM_ADJ_PARMET_DTLCpy
      @select         smallint = 0, 
      @prem_adj_id    int,
      @prem_adj_perd_id    int,
      @prem_adj_parmet_setup_id    int,
      @new_prem_adj_id    int,
      @new_prem_adj_perd_id    int,
      @new_prem_adj_parmet_setup_id    int

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

	insert into PREM_ADJ_PARMET_DTL
	(
            
            prem_adj_parmet_setup_id
           ,prem_adj_perd_id
           ,prem_adj_id
           ,custmr_id
           ,st_id
           ,ln_of_bsn_id
           ,clm_hndl_fee_los_typ_id
           ,los_amt
           ,los_base_asses_rt
           ,tot_amt
           ,paid_los_amt
           ,paid_aloc_los_adj_exps_amt
           ,resrv_los_amt
           ,resrv_aloc_los_adj_exps_amt
           ,los_dev_fctr_amt
           ,los_dev_fctr_rt
           ,los_base_asses_amt
           ,los_conv_fctr_rt
           ,los_conv_fctr_amt
           ,coml_agmt_id
           ,prem_adj_pgm_id
           ,updt_user_id
           ,updt_dt
           ,crte_user_id
           ,crte_dt
        )
        select
            @new_prem_adj_parmet_setup_id
           ,@new_prem_adj_perd_id
           ,@new_prem_adj_id
           ,custmr_id
           ,st_id
           ,ln_of_bsn_id
           ,clm_hndl_fee_los_typ_id
           ,los_amt
           ,los_base_asses_rt
           ,tot_amt
           ,paid_los_amt
           ,paid_aloc_los_adj_exps_amt
           ,resrv_los_amt
           ,resrv_aloc_los_adj_exps_amt
           ,los_dev_fctr_amt
           ,los_dev_fctr_rt
           ,los_base_asses_amt
           ,los_conv_fctr_rt
           ,los_conv_fctr_amt
           ,coml_agmt_id
           ,prem_adj_pgm_id
           ,NULL
           ,NULL
           ,crte_user_id
           ,@ent_timestamp
     from PREM_ADJ_PARMET_DTL
     where prem_adj_id = @prem_adj_id
     and prem_adj_perd_id = @prem_adj_perd_id
     and prem_adj_parmet_setup_id = @prem_adj_parmet_setup_id
 
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

if object_id('AddPREM_ADJ_PARMET_DTLCpy') is not null
        print 'Created Procedure AddPREM_ADJ_PARMET_DTLCpy'
else
        print 'Failed Creating Procedure AddPREM_ADJ_PARMET_DTLCpy'
go

if object_id('AddPREM_ADJ_PARMET_DTLCpy') is not null
        grant exec on AddPREM_ADJ_PARMET_DTLCpy to  public
go
 
