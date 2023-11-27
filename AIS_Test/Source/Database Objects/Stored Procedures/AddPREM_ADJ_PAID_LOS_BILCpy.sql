
if exists (select 1 from sysobjects 
                where name = 'AddPREM_ADJ_PAID_LOS_BILCpy' and type = 'P')
        drop procedure AddPREM_ADJ_PAID_LOS_BILCpy
go
 
set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	AddPREM_ADJ_PAID_LOS_BILCpy
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	Procedure creates a copy of the PREM_ADJ_PAID_LOS_BIL record.
-----	
-----	On Exit:
-----	
-----
-----	Modified:	11/01/2008  Development Team
-----                   Created SP
-----
---------------------------------------------------------------------
create procedure AddPREM_ADJ_PAID_LOS_BILCpy
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

	insert into PREM_ADJ_PAID_LOS_BIL
	(
            
            prem_adj_perd_id
           ,prem_adj_id
           ,custmr_id
           ,lsi_pgm_typ_txt
           ,lsi_src
           ,lsi_valn_dt
           ,idnmty_amt
           ,adj_idnmty_amt
           ,exps_amt
           ,adj_exps_amt
           ,tot_paid_los_bil_amt
           ,adj_tot_paid_los_bil_amt
           ,ln_of_bsn_id
           ,coml_agmt_id
           ,cmmnt_txt
           ,prem_adj_pgm_id
           ,updt_user_id
           ,updt_dt
           ,crte_user_id
           ,crte_dt
        )
        select
            @new_prem_adj_perd_id
           ,@new_prem_adj_id
           ,custmr_id
           ,lsi_pgm_typ_txt
           ,lsi_src
           ,lsi_valn_dt
           ,idnmty_amt
           ,adj_idnmty_amt
           ,exps_amt
           ,adj_exps_amt
           ,tot_paid_los_bil_amt
           ,adj_tot_paid_los_bil_amt
           ,ln_of_bsn_id
           ,coml_agmt_id
           ,cmmnt_txt
           ,prem_adj_pgm_id
           ,NULL
           ,NULL
           ,crte_user_id
           ,@ent_timestamp
     from PREM_ADJ_PAID_LOS_BIL
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

if object_id('AddPREM_ADJ_PAID_LOS_BILCpy') is not null
        print 'Created Procedure AddPREM_ADJ_PAID_LOS_BILCpy'
else
        print 'Failed Creating Procedure AddPREM_ADJ_PAID_LOS_BILCpy'
go

if object_id('AddPREM_ADJ_PAID_LOS_BILCpy') is not null
        grant exec on AddPREM_ADJ_PAID_LOS_BILCpy to  public
go
 
