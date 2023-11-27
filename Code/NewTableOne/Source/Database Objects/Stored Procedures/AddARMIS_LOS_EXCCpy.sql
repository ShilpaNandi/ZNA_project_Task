
if exists (select 1 from sysobjects 
                where name = 'AddARMIS_LOS_EXCCpy' and type = 'P')
        drop procedure AddARMIS_LOS_EXCCpy
go
 
set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	AddARMIS_LOS_EXCCpy
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	Procedure creates a copy of the ARMIS_LOS_EXC record.
-----	
-----	On Exit:
-----	
-----
-----	Modified:	11/01/2008  Development Team
-----                   Created SP
-----
---------------------------------------------------------------------
create procedure AddARMIS_LOS_EXCCpy
      @select         smallint = 0, 
      @prem_adj_id    int,
      @armis_los_pol_id    int,
      @new_prem_adj_id    int,
      @new_armis_los_pol_id    int

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

	insert into ARMIS_LOS_EXC
	(
            
            armis_los_pol_id
           ,coml_agmt_id
           ,prem_adj_pgm_id
           ,custmr_id
           ,orgin_clm_nbr_txt
           ,clm_nbr_txt
           ,addn_clm_ind
           ,addn_clm_txt
           ,lim2_amt
           ,site_cd_txt
           ,covg_trigr_dt
           ,clmt_nm
           ,reop_clm_nbr_txt
           ,paid_idnmty_amt
           ,paid_exps_amt
           ,resrvd_idnmty_amt
           ,resrvd_exps_amt
           ,non_bilabl_paid_idnmty_amt
           ,non_bilabl_paid_exps_amt
           ,non_bilabl_resrvd_idnmty_amt
           ,non_bilabl_resrvd_exps_amt
           ,subj_paid_idnmty_amt
           ,subj_paid_exps_amt
           ,subj_resrvd_idnmty_amt
           ,subj_resrvd_exps_amt
           ,exc_paid_idnmty_amt
           ,exc_paid_exps_amt
           ,exc_resrvd_idnmty_amt
           ,exc_resrvd_exps_amt
           ,sys_genrt_ind
           ,clm_sts_id
           ,exc_ldf_ibnr_amt
           ,subj_ldf_ibnr_amt
           ,updt_user_id
           ,updt_dt
           ,crte_user_id
           ,crte_dt
           ,actv_ind
        )
        select
            @new_armis_los_pol_id
           ,coml_agmt_id
           ,prem_adj_pgm_id
           ,custmr_id
           ,orgin_clm_nbr_txt
           ,clm_nbr_txt
           ,addn_clm_ind
           ,addn_clm_txt
           ,lim2_amt
           ,site_cd_txt
           ,covg_trigr_dt
           ,clmt_nm
           ,reop_clm_nbr_txt
           ,paid_idnmty_amt
           ,paid_exps_amt
           ,resrvd_idnmty_amt
           ,resrvd_exps_amt
           ,non_bilabl_paid_idnmty_amt
           ,non_bilabl_paid_exps_amt
           ,non_bilabl_resrvd_idnmty_amt
           ,non_bilabl_resrvd_exps_amt
           ,subj_paid_idnmty_amt
           ,subj_paid_exps_amt
           ,subj_resrvd_idnmty_amt
           ,subj_resrvd_exps_amt
           ,exc_paid_idnmty_amt
           ,exc_paid_exps_amt
           ,exc_resrvd_idnmty_amt
           ,exc_resrvd_exps_amt
           ,sys_genrt_ind
           ,clm_sts_id
           ,exc_ldf_ibnr_amt
           ,subj_ldf_ibnr_amt
           ,NULL
           ,NULL
           ,crte_user_id
           ,@ent_timestamp
           ,actv_ind
     from ARMIS_LOS_EXC
     where armis_los_pol_id = @armis_los_pol_id
 
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

if object_id('AddARMIS_LOS_EXCCpy') is not null
        print 'Created Procedure AddARMIS_LOS_EXCCpy'
else
        print 'Failed Creating Procedure AddARMIS_LOS_EXCCpy'
go

if object_id('AddARMIS_LOS_EXCCpy') is not null
        grant exec on AddARMIS_LOS_EXCCpy to  public
go
 
