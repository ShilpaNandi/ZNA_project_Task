
if exists (select 1 from sysobjects 
		where name = 'AddPREM_ADJ_RETRO_DTL' and type = 'P')
	drop procedure AddPREM_ADJ_RETRO_DTL
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	AddPREM_ADJ_RETRO_DTL
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to populate the table PREM_ADJ_RETRO_DTL.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------

create procedure [dbo].[AddPREM_ADJ_RETRO_DTL]
@prem_adj_retro_id int, 
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@premium_adj_prog_id int,
@state_id int,
@subj_paid_idnmty_amt decimal(15,2),
@subj_paid_exps_amt decimal(15,2),
@subj_resrv_idnmty_amt decimal(15,2),
@subj_resrv_exps_amt decimal(15,2),
@basic_amt decimal(15,2),
@incur_erp_amt decimal(15,2),
@paid_erp_amt decimal(15,2),
@cfb_amt decimal(15,2),
@create_user_id int,
@elp_amt decimal(15,2) = null,
@lcf_amt decimal(15,2) = null,
@tax_amt decimal(15,2) = null,
@chf_amt decimal(15,2) = null,
@lba_amt decimal(15,2) = null,
@ncf_amt decimal(15,2) = null,
@other_amt decimal(15,2) = null,
@ibnr_ldf_result decimal(15,2) = null,
@adj_ded_wc_loss_amt decimal(15,2) = null,
@coml_agmt_id int,
@std_sub_prem_amount decimal(15,2),
@prem_asses_amount decimal(15,2),
@earned_resd_amt decimal(15,2),
@paid_rml_amt decimal(15,2),
@lob_id int,
@lcf decimal(15,8),
@los_dev_fctr_rt decimal(15,8),
@incur_but_not_rptd_fctr_rt decimal(15,8),
@prem_adj_retro_dtl_id_op int output


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
 
 declare @pa_ft decimal(15,8)

 --exec @pa_ft = [dbo].[fn_RetrievePremiumAssessmentFactor] @p_cust_id = @customer_id,@p_prem_adj_prog_id = @premium_adj_prog_id,@p_comm_agr_id = @coml_agmt_id, @p_state_id = @state_id
 select @pa_ft = ad.prem_asses_rt
 from dbo.PREM_ADJ_PGM_SETUP s
 inner join PREM_ADJ_PGM_SETUP_POL ap on (s.custmr_id = ap.custmr_id ) and (s.prem_adj_pgm_id = ap.prem_adj_pgm_id) and (s.prem_adj_pgm_setup_id = ap.prem_adj_pgm_setup_id)
 inner join PREM_ADJ_PGM_DTL ad on  (ap.prem_adj_pgm_setup_id = ad.prem_adj_pgm_setup_id ) and (s.custmr_id = ad.custmr_id ) and (s.prem_adj_pgm_id = ad.prem_adj_pgm_id)
 where 
 s.custmr_id = @customer_id 
 and s.prem_adj_pgm_id = @premium_adj_prog_id
 and ap.coml_agmt_id = @coml_agmt_id
 --and ad.ln_of_bsn_id = @p_lob_id
 and ad.st_id = @state_id
 and s.actv_ind = 1
 and ad.actv_ind  = 1 
 and s.adj_parmet_typ_id = 404 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> WA

 if @pa_ft is null
 begin
  set @prem_asses_amount = null
 end

 insert into [dbo].[PREM_ADJ_RETRO_DTL]
  (
   [prem_adj_retro_id]
           ,[prem_adj_perd_id]
           ,[prem_adj_id]
           ,[custmr_id]
           ,[st_id]
           ,[subj_paid_idnmty_amt]
           ,[subj_paid_exps_amt]
           ,[subj_resrv_idnmty_amt]
           ,[subj_resrv_exps_amt]
           ,[basic_amt]
           ,[clm_hndl_fee_amt]
           ,[los_base_asessment_amt]
           ,[non_conv_fee_amt]
           ,[prem_tax_amt]
           ,[othr_amt]
     ,[los_conv_fctr_amt]
     ,[exc_los_prem_amt]
           ,[incur_ernd_retro_prem_amt]
           ,[adj_incur_ernd_retro_prem_amt]
           ,[paid_ernd_retro_prem_amt]
           ,[adj_paid_ernd_retro_prem_amt]
           ,[cash_flw_ben_amt]
     ,[los_dev_resrv_amt]
     ,[adj_dedtbl_wrk_comp_los_amt]
     ,[coml_agmt_id]
     ,[prem_adj_pgm_id]
     ,[std_subj_prem_amt]
     ,[prem_asses_amt]
     ,[rsdl_mkt_load_ernd_amt]
     ,[rsdl_mkt_load_paid_amt]
     ,[ln_of_bsn_id]
     ,[los_conv_fctr_rt]
	 ,[los_dev_fctr_rt]
	 ,[incur_but_not_rptd_fctr_rt]
           ,[crte_user_id]
  )
     values
        (
   @prem_adj_retro_id , 
   @premium_adj_period_id ,
   @premium_adjustment_id ,
   @customer_id ,
   @state_id ,
   @subj_paid_idnmty_amt ,
   @subj_paid_exps_amt ,
   @subj_resrv_idnmty_amt ,
   @subj_resrv_exps_amt ,
   @basic_amt ,
   @chf_amt ,
   @lba_amt ,
   @ncf_amt ,
   @tax_amt ,
   @other_amt ,
   @lcf_amt ,
   @elp_amt ,
            @incur_erp_amt,
            @incur_erp_amt,
      @paid_erp_amt,
      @paid_erp_amt,
   @cfb_amt,
   @ibnr_ldf_result,
   @adj_ded_wc_loss_amt,
   @coml_agmt_id,
   @premium_adj_prog_id,
   @std_sub_prem_amount,
   @prem_asses_amount,
   @earned_resd_amt,
   @paid_rml_amt,
   @lob_id,
   @lcf, 
   @los_dev_fctr_rt,
   @incur_but_not_rptd_fctr_rt,
            @create_user_id
  )

 set @prem_adj_retro_dtl_id_op = @@identity

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

if object_id('AddPREM_ADJ_RETRO_DTL') is not null
	print 'Created Procedure AddPREM_ADJ_RETRO_DTL'
else
	print 'Failed Creating Procedure AddPREM_ADJ_RETRO_DTL'
go

if object_id('AddPREM_ADJ_RETRO_DTL') is not null
	grant exec on AddPREM_ADJ_RETRO_DTL to public
go






