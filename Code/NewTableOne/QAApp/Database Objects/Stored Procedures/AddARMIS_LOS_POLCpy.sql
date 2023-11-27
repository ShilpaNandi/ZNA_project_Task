
if exists (select 1 from sysobjects 
                where name = 'AddARMIS_LOS_POLCpy' and type = 'P')
        drop procedure AddARMIS_LOS_POLCpy
go
 
set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	AddARMIS_LOS_POLCpy
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	Procedure creates a copy of the ARMIS_LOS_POL record.
-----	
-----	On Exit:
-----	
-----
-----	Modified:	11/01/2008  Development Team
-----                   Created SP
-----
---------------------------------------------------------------------
create procedure AddARMIS_LOS_POLCpy
      @select         smallint = 0, 
      @prem_adj_id    int,
      @armis_los_pol_id    int,
      @new_prem_adj_id    int,
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

	insert into ARMIS_LOS_POL
	(
            
            coml_agmt_id
           ,prem_adj_pgm_id
           ,custmr_id
           ,st_id
           ,valn_dt
           ,prem_adj_id
           ,suprt_serv_custmr_gp_id
           ,paid_idnmty_amt
           ,paid_exps_amt
           ,resrv_idnmty_amt
           ,resrv_exps_amt
           ,non_bilabl_paid_idnmty_amt
           ,non_bilabl_paid_exps_amt
           ,non_bilabl_resrv_idnmty_amt
           ,non_bilabl_resrv_exps_amt
           ,subj_paid_idnmty_amt
           ,subj_paid_exps_amt
           ,subj_resrv_idnmty_amt
           ,subj_resrv_exps_amt
           ,exc_paid_idnmty_amt
           ,exc_paid_exps_amt
           ,exc_resrvd_idnmty_amt
           ,exc_resrv_exps_amt
           ,subj_ldf_ibnr_amt
           ,exc_ldf_ibnr_amt
           ,sys_genrt_ind
           ,updt_user_id
           ,updt_dt
           ,crte_user_id
           ,crte_dt
           ,actv_ind
        )
        select
            coml_agmt_id
           ,prem_adj_pgm_id
           ,custmr_id
           ,st_id
           ,valn_dt
           ,@new_prem_adj_id
           ,suprt_serv_custmr_gp_id
           ,paid_idnmty_amt
           ,paid_exps_amt
           ,resrv_idnmty_amt
           ,resrv_exps_amt
           ,non_bilabl_paid_idnmty_amt
           ,non_bilabl_paid_exps_amt
           ,non_bilabl_resrv_idnmty_amt
           ,non_bilabl_resrv_exps_amt
           ,subj_paid_idnmty_amt
           ,subj_paid_exps_amt
           ,subj_resrv_idnmty_amt
           ,subj_resrv_exps_amt
           ,exc_paid_idnmty_amt
           ,exc_paid_exps_amt
           ,exc_resrvd_idnmty_amt
           ,exc_resrv_exps_amt
           ,subj_ldf_ibnr_amt
           ,exc_ldf_ibnr_amt
           ,sys_genrt_ind
           ,NULL
           ,NULL
           ,crte_user_id
           ,@ent_timestamp
           ,actv_ind
     from ARMIS_LOS_POL
     where armis_los_pol_id = @armis_los_pol_id
 
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

if object_id('AddARMIS_LOS_POLCpy') is not null
        print 'Created Procedure AddARMIS_LOS_POLCpy'
else
        print 'Failed Creating Procedure AddARMIS_LOS_POLCpy'
go

if object_id('AddARMIS_LOS_POLCpy') is not null
        grant exec on AddARMIS_LOS_POLCpy to  public
go
 
