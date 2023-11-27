
if exists (select 1 from sysobjects 
                where name = 'AddPREM_ADJCpy' and type = 'P')
        drop procedure AddPREM_ADJCpy
go
 
set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:	AddPREM_ADJCpy
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	Procedure creates a copy of the PREM_ADJ record.
-----	
-----	On Exit:
-----	
-----
-----	Modified:	11/01/2008  Development Team
-----                   Created SP
-----
---------------------------------------------------------------------
create procedure AddPREM_ADJCpy
      @select         smallint = 0, 
      @prem_adj_id    int,
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

	insert into PREM_ADJ
	(
            
            reg_custmr_id
           ,rel_prem_adj_id
           ,valn_dt
           ,drft_invc_nbr_txt
           ,drft_invc_dt
           ,drft_mailed_undrwrt_dt
           ,drft_intrnl_pdf_zdw_key_txt
           ,drft_extrnl_pdf_zdw_key_txt
           ,drft_cd_wrksht_pdf_zdw_key_txt
           ,fnl_invc_nbr_txt
           ,fnl_invc_dt
           ,fnl_mailed_undrwrt_dt
           ,fnl_intrnl_pdf_zdw_key_txt
           ,fnl_extrnl_pdf_zdw_key_txt
           ,fnl_cd_wrksht_pdf_zdw_key_txt
           ,fnl_mailed_brkr_dt
           ,undrwrt_not_reqr_ind
           ,invc_due_dt
           ,historical_adj_ind
           ,twenty_pct_qlty_cntrl_reqr_ind
           ,twenty_pct_qlty_cntrl_ind
           ,twenty_pct_qlty_cntrl_pers_id
           ,twenty_pct_qlty_cntrl_dt
           ,calc_adj_sts_cd
           ,adj_pendg_ind
           ,adj_pendg_rsn_id
           ,adj_rrsn_rsn_id
           ,adj_void_rsn_id
           ,adj_can_ind
           ,adj_void_ind
           ,adj_rrsn_ind
           ,void_rrsn_cmmnt_txt
           ,brkr_id
           ,bu_office_id
           ,adj_sts_typ_id
           ,adj_sts_eff_dt
           ,reconciler_revw_ind
           ,adj_qc_ind
           ,updt_user_id
           ,updt_dt
           ,crte_user_id
           ,crte_dt
        )
        select
            reg_custmr_id
           ,@prem_adj_id
           ,valn_dt
           ,drft_invc_nbr_txt
           ,drft_invc_dt
           ,drft_mailed_undrwrt_dt
           ,drft_intrnl_pdf_zdw_key_txt
           ,drft_extrnl_pdf_zdw_key_txt
           ,drft_cd_wrksht_pdf_zdw_key_txt
           ,fnl_invc_nbr_txt
           ,fnl_invc_dt
           ,fnl_mailed_undrwrt_dt
           ,fnl_intrnl_pdf_zdw_key_txt
           ,fnl_extrnl_pdf_zdw_key_txt
           ,fnl_cd_wrksht_pdf_zdw_key_txt
           ,fnl_mailed_brkr_dt
           ,undrwrt_not_reqr_ind
           ,invc_due_dt
           ,historical_adj_ind
           ,twenty_pct_qlty_cntrl_reqr_ind
           ,twenty_pct_qlty_cntrl_ind
           ,twenty_pct_qlty_cntrl_pers_id
           ,twenty_pct_qlty_cntrl_dt
           ,calc_adj_sts_cd
           ,adj_pendg_ind
           ,adj_pendg_rsn_id
           ,adj_rrsn_rsn_id
           ,adj_void_rsn_id
           ,adj_can_ind
           ,adj_void_ind
           ,adj_rrsn_ind
           ,void_rrsn_cmmnt_txt
           ,brkr_id
           ,bu_office_id
           ,adj_sts_typ_id
           ,adj_sts_eff_dt
           ,reconciler_revw_ind
           ,adj_qc_ind
           ,NULL
           ,NULL
           ,crte_user_id
           ,@ent_timestamp
     from PREM_ADJ
     where prem_adj_id = @prem_adj_id
 
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

if object_id('AddPREM_ADJCpy') is not null
        print 'Created Procedure AddPREM_ADJCpy'
else
        print 'Failed Creating Procedure AddPREM_ADJCpy'
go

if object_id('AddPREM_ADJCpy') is not null
        grant exec on AddPREM_ADJCpy to  public
go
 
