
if exists (select 1 from sysobjects 
		where name = 'ModAISCalcILRF' and type = 'P')
	drop procedure ModAISCalcILRF
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcILRF
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to populate the output tables for ILRF with calculation results.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	01/09/09	Prabal Dhar
-----			- Subtraction of 1 from the LCF factor
-----	Modified:	01/09/09	Prabal Dhar
-----			- Null handling added for posting transactions

---------------------------------------------------------------------

create procedure [dbo].[ModAISCalcILRF] 
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@delete_ilrf bit,
@premium_adj_prog_id int,
@create_user_id int,
@err_msg_op varchar(1000) output,
@debug bit = 0
as

begin
	set nocount on

declare	@use_paid_los_ind bit,
		@use_paid_alae_ind bit,
		@use_resrv_los_ind bit,
		@use_resv_alae_ind bit,
		@ldf_ibnr_lim_ind bit,
		@amt_subj_to_lba_ft decimal(15,2),
		@lba_result decimal(15,2),
		@amt_subj_to_lcf_ft decimal(15,2),
		@lcf_result decimal(15,2),
		@amt_subj_to_ldf_ft decimal(15,2),
		@amt_subj_to_ldf_lcf decimal(15,2),
		@amt_subj_to_ldf_lba decimal(15,2),
		@ldf_result decimal(15,2),
		@amt_subj_to_ibnr_ft decimal(15,2),
		@amt_subj_to_ibnr_lcf decimal(15,2),
		@amt_subj_to_ibnr_lba decimal(15,2),
		@ibnr_result decimal(15,2),
		@ibnr_ldf_ft_id int,
		@subj_paid_idnmty_amt decimal(15,2),
		@subj_paid_exps_amt decimal(15,2),
		@subj_resrv_idnmty_amt decimal(15,2),
		@subj_resrv_exps_amt decimal(15,2),
		@ilrf_prev_bil_amt decimal(15,2),
		@aggr_wc_tpd_amt decimal(15,2),
		@aggr_auto_tpd_amt decimal(15,2),
		@aggr_gl_tpd_amt decimal(15,2),
		@aggr_wc_lcf_amt decimal(15,2),
		@aggr_auto_lcf_amt decimal(15,2),
		@aggr_gl_lcf_amt decimal(15,2),
		@aggr_resv_amt decimal(15,2),
		@aggr_lba_amt decimal(15,2),
		@prior_wc_tpd_amt decimal(15,2),
		@prior_auto_tpd_amt decimal(15,2),
		@prior_gl_tpd_amt decimal(15,2),
		@prior_wc_lcf_amt decimal(15,2),
		@prior_auto_lcf_amt decimal(15,2),
		@prior_gl_lcf_amt decimal(15,2),
		@prior_resv_amt decimal(15,2),
		@prior_lba_amt decimal(15,2),
		@sum_non_reserve decimal(15,2),
		@com_agm_id int,
		@state_id int,
		@prem_adj_pgm_setup_id int,
		@prem_adj_pgm_setup_id_tracker int,
		@prem_adj_parmet_setup_id int,
		@lob varchar(10),
		@lob_id int,
		@lcf_fact decimal(15,8),
		@lba_fact decimal(15,8),
		@loss_amt decimal(15,2),
		@ibnr_amt decimal(15,2),
		@ldf_ibnr_amt decimal(15,2),
		@amt_subj_lba_ft decimal(15,2),
		@tot_amt decimal(15,2),
		@sum_loss_amt decimal(15,2),
		@sum_tot_amt decimal(15,2),
		@dep_amt decimal(15,2),
		@ibnr_rt decimal(15,8),
		@ldf_rt decimal(15,8),
		@ldf_ibnr_rt decimal(15,8),
		@ilrf_min_lim_ind bit,
		@ilrf_min_lim_amt decimal(15,2),
		@tot_curr_amt decimal(15,2),
		@count int,
		@counter int,
		@com_agr_id int,
		@cnt_prev_adjs int,
		@is_ibnr bit,
		@ldf_ibnr_step_ind bit,
		@ilrf_invc_lsi_ind bit,
		@months_to_val int,
		@first_adj_non_prem int,
		@prev_valid_adj_id int,
		@prev_valid_adj_perd_id int,
		@freq smallint,
		@months_elapsed smallint,
		@fst_adj_dt datetime,
		@next_val_date datetime,
		@prem_adj_valn_dt datetime,
		@pgm_period_valn_dt datetime,
		@exc_ldf_ibnr_amt decimal(15,2),
		@prior_yy_amt_tot decimal(15,2),
		@adj_prior_yy_amt_tot decimal(15,2),
		@lba_final_amt decimal(15,2),
		@lcf_aggr_cap_set_pgm_amt decimal(15,2),
		@lcf_aggr_cap_state_amt decimal(15,2),
		@sum_lcf_amt decimal(15,2),
		@sum_lcf_amt_used decimal(15,2),
		@lcf_state_amt decimal(15,2),
		@state_count int,
		@state_counter int,
		@err_message varchar(500),
		@trancount int

--Check if ILRF calc needs to be performed for this adjustment
select 
@prem_adj_valn_dt = valn_dt
from dbo.PREM_ADJ
where prem_adj_id = @premium_adjustment_id

select 
@pgm_period_valn_dt = nxt_valn_dt_non_prem_dt
from dbo.PREM_ADJ_PGM
where prem_adj_pgm_id = @premium_adj_prog_id

if @debug = 1
begin
print 'Before ILRF valuation date validation'
end

if (@prem_adj_valn_dt <> @pgm_period_valn_dt)
begin
				set @err_message = 'ILRF: Valuation date for this adjustment and the valuation date specified in the program period setup do not match' 
				+ ';customer ID: ' + convert(varchar(20),@customer_id) 
				+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
				set @err_msg_op = @err_message
				exec [dbo].[AddAPLCTN_STS_LOG] 
					@premium_adjustment_id = @premium_adjustment_id,
					@customer_id = @customer_id,
					@premium_adj_prog_id = @premium_adj_prog_id,
					@err_msg = @err_message,
					@create_user_id = @create_user_id

	return
end

if @debug = 1
begin
print 'ILRF: valuation date validation PASSED; START OF CALC'
end

set @trancount = @@trancount
--print @trancount
if @trancount >= 1
    save transaction ModAISCalcILRF
else
    begin transaction


begin try

	/**************************
	* Determine first adjustment
	**************************/

	--exec @cnt_prev_adjs = [dbo].[fn_DetermineFirstAdjustment]
		--@premium_adj_prog_id = @premium_adj_prog_id,
		--@adj_parmet_typ_id = 400 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> ILRF
		--10773 Fix
		exec @cnt_prev_adjs = [dbo].[fn_DetermineFirstAdjustment_ParmetType]
		@premium_adj_prog_id = @premium_adj_prog_id,
		@adj_parmet_typ_id = 400 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> ILRF


	if (@cnt_prev_adjs = 0) -- No existing adjustments; this is initial adjustment
	begin
		print 'initial adjustment'
		/*************************************************************
		* Additional check required for program period setup ->FIRST ADJ NON-PREMIUM 
		* before initiating ILRF calc. Only if inception to VAL DATE is 
		* more than this value, intiate LBA calc.
		*************************************************************/
--		select
--		@fst_adj_dt = fst_adj_non_prem_dt,
--		@next_val_date = nxt_valn_dt_non_prem_dt 
--		from dbo.PREM_ADJ_PGM 
--		where prem_adj_pgm_id = @premium_adj_prog_id
--
--		if (@fst_adj_dt <> @next_val_date)
--		begin
--			set @err_message = 'ILRF: First Adjustment Date(NP) is not equal to Next Valuation Date(NP)' 
--			+ ';customer ID: ' + convert(varchar(20),@customer_id) 
--			+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
--			rollback transaction ModAISCalcILRF
--			set @err_msg_op = @err_message
--			exec [dbo].[AddAPLCTN_STS_LOG] 
--				@premium_adjustment_id = @premium_adjustment_id,
--				@customer_id = @customer_id,
--				@premium_adj_prog_id = @premium_adj_prog_id,
--				@err_msg = @err_message,
--				@create_user_id = @create_user_id
--			return
--		end
	end
	else
	begin -- Subsequent adjustment / this is not initial adjustment
		select @months_elapsed = datediff(mm,prev_valn_dt_non_prem_dt,nxt_valn_dt_non_prem_dt), 
			   @freq = freq_non_prem_mms_cnt -- Non-premium frequency
		from dbo.PREM_ADJ_PGM 
		where prem_adj_pgm_id = @premium_adj_prog_id

		if @months_elapsed <> @freq
		begin
			set @err_message = 'ILRF: Difference between Next Valuation Date(NP) and Previous Valuation Date(NP) is not consistent with frequency for ILRF'
			+ ';customer ID: ' + convert(varchar(20),@customer_id) 
			+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
			rollback transaction ModAISCalcILRF
			set @err_msg_op = @err_message
			exec [dbo].[AddAPLCTN_STS_LOG] 
				@premium_adjustment_id = @premium_adjustment_id,
				@customer_id = @customer_id,
				@premium_adj_prog_id = @premium_adj_prog_id,
				@err_msg = @err_message,
				@create_user_id = @create_user_id
			return
		end
	end

	select @next_val_date = nxt_valn_dt_non_prem_dt
	from dbo.PREM_ADJ_PGM 
	where prem_adj_pgm_id = @premium_adj_prog_id

	if(getdate() < @next_val_date)
	begin
		set @err_message = 'ILRF: Current date is less than the Next Valuation Date(NP)'
		+ ';customer ID: ' + convert(varchar(20),@customer_id) 
		+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
		rollback transaction ModAISCalcILRF
			set @err_msg_op = @err_message
			exec [dbo].[AddAPLCTN_STS_LOG] 
				@premium_adjustment_id = @premium_adjustment_id,
				@customer_id = @customer_id,
				@premium_adj_prog_id = @premium_adj_prog_id,
				@err_msg = @err_message,
				@create_user_id = @create_user_id
		return
	end

			
	declare ilrf_base_cur cursor LOCAL FAST_FORWARD READ_ONLY 
		for 
		select
		s.prem_adj_pgm_setup_id,
		s.incur_but_not_rptd_los_dev_fctr_id,	
		al.coml_agmt_id,
		al.st_id,
		subj_paid_idnmty_amt,
		subj_paid_exps_amt,
		subj_resrv_idnmty_amt,
		subj_resrv_exps_amt
		from (
				select 				
				prem_adj_pgm_id, 				
				custmr_id, 				
				coml_agmt_id, 				
				st_id, 				
				isnull(sum(subj_paid_idnmty_amt),0) as subj_paid_idnmty_amt , 				
				isnull(sum(subj_paid_exps_amt),0) as   subj_paid_exps_amt ,			
				isnull(sum(subj_resrv_idnmty_amt),0) as subj_resrv_idnmty_amt , 				
				isnull(sum(subj_resrv_exps_amt),0) as subj_resrv_exps_amt 				
				from 				
				dbo.ARMIS_LOS_POL 				
				where prem_adj_pgm_id =@premium_adj_prog_id 				
				and custmr_id = @customer_id 				
				and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) 
				and valn_dt	= @prem_adj_valn_dt -- Triage # 67			
				and actv_ind = 1  
				group by  st_id,coml_agmt_id,custmr_id,prem_adj_pgm_id

			) al
		
		inner join PREM_ADJ_PGM_SETUP_POL ap on (al.coml_agmt_id = ap.coml_agmt_id ) and (al.prem_adj_pgm_id = ap.prem_adj_pgm_id)
		inner join coml_agmt ca on ca.coml_agmt_id = ap.coml_agmt_id and ca.actv_ind = 1
		inner join dbo.PREM_ADJ_PGM_SETUP s on (s.prem_adj_pgm_id = ap.prem_adj_pgm_id ) and (s.prem_adj_pgm_setup_id = ap.prem_adj_pgm_setup_id)
		--inner join dbo.INCUR_LOS_REIM_FUND_FRMLA frm on  (s.prem_adj_pgm_id = frm.prem_adj_pgm_id ) and (s.custmr_id =  frm.custmr_id )
		where 
		s.custmr_id = @customer_id 
		and s.prem_adj_pgm_id = @premium_adj_prog_id
		--and ((al.prem_adj_id is null) or (al.prem_adj_id = @premium_adjustment_id))
		and s.actv_ind = 1
		--and al.actv_ind = 1
		and s.adj_parmet_typ_id = 400 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> ILRF
		order by s.prem_adj_pgm_setup_id

		open ilrf_base_cur
		fetch ilrf_base_cur into @prem_adj_pgm_setup_id, @ibnr_ldf_ft_id, @com_agm_id, @state_id, 
			/*@paid_idnmty__amt , @paid_exps_amt , @resrv_idnmty_amt , @resrv_exps_amt ,*/ 
			@subj_paid_idnmty_amt, @subj_paid_exps_amt, @subj_resrv_idnmty_amt, @subj_resrv_exps_amt

		set @prem_adj_pgm_setup_id_tracker = 0


		while @@Fetch_Status = 0
			begin
				begin
					if @debug = 1
					begin
				    print'*******************ILRF: START OF ITERATION*********' 
				    print'---------------Input Params-------------------' 

					print' @prem_adj_pgm_setup_id: ' + convert(varchar(20), @prem_adj_pgm_setup_id)  
					print' @com_agm_id:- ' + convert(varchar(20), @com_agm_id)  
					print' @state_id:- ' + convert(varchar(20), @state_id)  
					print' @customer_id: ' + convert(varchar(20), @customer_id)
					print' @premium_adj_prog_id: ' + convert(varchar(20), @premium_adj_prog_id )  
--					print' @adj_fctr_rt: ' + convert(varchar(20), @adj_fctr_rt ) 
--					print' @fnl_overrid_amt: ' + convert(varchar(20), @fnl_overrid_amt )  
					print' @subj_paid_idnmty_amt: ' + convert(varchar(20), isnull(@subj_paid_idnmty_amt,0)) 
					print' @subj_paid_exps_amt: ' + convert(varchar(20), isnull(@subj_paid_exps_amt,0) )  
					print' @subj_resrv_idnmty_amt: ' + convert(varchar(20), isnull(@subj_resrv_idnmty_amt,0) )  
					print' @subj_resrv_exps_amt: ' + convert(varchar(20), isnull(@subj_resrv_exps_amt,0) )  
					end

					-- Handle potential null values
					set @subj_paid_idnmty_amt = isnull(@subj_paid_idnmty_amt,0)
					set @subj_paid_exps_amt = isnull(@subj_paid_exps_amt,0)
					set @subj_resrv_idnmty_amt = isnull(@subj_resrv_idnmty_amt,0)
					set @subj_resrv_exps_amt = isnull(@subj_resrv_exps_amt,0)
					set @lba_result = null
					set @lba_fact = 0
					


					if @prem_adj_pgm_setup_id_tracker <> @prem_adj_pgm_setup_id
					begin

						set @prem_adj_pgm_setup_id_tracker = @prem_adj_pgm_setup_id

						if not exists(select * from dbo.PREM_ADJ_PARMET_SETUP where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and prem_adj_pgm_setup_id = @prem_adj_pgm_setup_id )
						begin

							if @debug = 1
							begin
							print 'before insert to header'
							print' @premium_adj_period_id: ' + convert(varchar(20), isnull(@premium_adj_period_id,0)) 
							print' @premium_adjustment_id: ' + convert(varchar(20), isnull(@premium_adjustment_id,0) )  
							print' @prem_adj_pgm_setup_id: ' + convert(varchar(20), isnull(@prem_adj_pgm_setup_id,0) )  
							print' @premium_adj_prog_id: ' + convert(varchar(20), isnull(@premium_adj_prog_id,0) )  
							print' @customer_id: ' + convert(varchar(20), isnull(@customer_id,0) )  
							end

							exec [dbo].[AddPREM_ADJ_PARMET_SETUP] 
								@premium_adj_period_id ,
								@premium_adjustment_id ,
								@customer_id ,
								@prem_adj_pgm_setup_id ,
								@premium_adj_prog_id ,
								400, -- Lookup value for Adjustment Parameter type : "ILRF"
								@create_user_id ,
								@prem_adj_parmet_setup_id_op = @prem_adj_parmet_setup_id output
								--print '@prem_adj_parmet_setup_id.........: ' + convert(varchar(20),@prem_adj_parmet_setup_id)
								
						end
						else
						begin
							select @prem_adj_parmet_setup_id = prem_adj_parmet_setup_id from dbo.PREM_ADJ_PARMET_SETUP where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and prem_adj_pgm_setup_id = @prem_adj_pgm_setup_id 
						end
					end -- else of: if @prem_adj_pgm_setup_id_tracker <> @prem_adj_pgm_setup_id


					/**********************************************************
					* Retrieve ILRF indicators and apply factors in accordance
					* with the configured indicators.
					***********************************************************/

					--Determine excess LDF/IBNR amount
					select 
					@ldf_ibnr_lim_ind = los_dev_fctr_incur_but_not_rptd_incld_lim_ind 
					from dbo.COML_AGMT 
					where coml_agmt_id = @com_agm_id and actv_ind = 1

					if(@ldf_ibnr_lim_ind = 1)
					begin
						select @exc_ldf_ibnr_amt = round(isnull(sum(exc_ldf_ibnr_amt),0),0) 
						from ARMIS_LOS_POL 
						where prem_adj_pgm_id = @premium_adj_prog_id 
						and coml_agmt_id = @com_agm_id 
						and st_id = @state_id
						and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) 
					end


					--Determine line of business
					select @lob = attr_1_txt 
					from dbo.LKUP 
					where lkup_id = (
										select 
										covg_typ_id 
										from dbo.COML_AGMT 
										where coml_agmt_id = @com_agm_id
										and prem_adj_pgm_id = @premium_adj_prog_id
										and custmr_id = @customer_id
										and actv_ind = 1
									)
					and lkup_typ_id in (
											select 
											lkup_typ_id 
											from dbo.LKUP_TYP 
											where lkup_typ_nm_txt like 'LOB COVERAGE'
										)
					set @lob_id = dbo.fn_GetIDForLOB(@lob)
					if @debug = 1
					begin
					print 'retrieved @lob_id: ' + convert(varchar(20), @lob_id ) 
					end

					/**********
					* LBA
					**********/
					--Requirement ID: A.160.080
					if @lob_id = 428 -- Lookup Type: LOB Lookup ID: WC
					begin
						
						exec @lba_final_amt = dbo.fn_RetrieveLBA_Fnl_Overrid_Amt
						@p_cust_id = @customer_id,
						@p_prem_adj_prog_id = @premium_adj_prog_id,
						@p_comm_agr_id = @com_agm_id,
						@p_state_id = @state_id
						if @debug = 1
						begin
						print 'retrieved @lba_final_amt: ' + convert(varchar(20), @lba_final_amt ) 
						end
						
						exec @lba_fact = dbo.fn_RetrieveLBA_ft
						@p_cust_id = @customer_id,
						@p_prem_adj_prog_id = @premium_adj_prog_id,
						@p_comm_agr_id = @com_agm_id,
						@p_state_id = @state_id
						if @debug = 1
						begin
						print 'retrieved @lba_fact: ' + convert(varchar(20), @lba_fact ) 
						end
						
						if(@lba_final_amt=0)
						begin
						set @use_paid_los_ind = 0
						set @use_paid_alae_ind = 0
						set @use_resrv_los_ind = 0
						set @use_resv_alae_ind = 0

						select @use_paid_los_ind = [use_paid_los_ind]
							  ,@use_paid_alae_ind = [use_paid_aloc_los_adj_exps_ind]
							  ,@use_resrv_los_ind = [use_resrv_los_ind]
							  ,@use_resv_alae_ind = [use_resrv_aloc_los_adj_exps_ind]
						from dbo.INCUR_LOS_REIM_FUND_FRMLA
						where 
						prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and los_reim_fund_fctr_typ_id = 405 -- lookup type: LRF Factor Type lookup value: LBA

						set @amt_subj_to_lba_ft = 0
						if @use_paid_los_ind = 1
							set @amt_subj_to_lba_ft = @amt_subj_to_lba_ft + @subj_paid_idnmty_amt

						if @use_paid_alae_ind = 1
							set @amt_subj_to_lba_ft = @amt_subj_to_lba_ft + @subj_paid_exps_amt

						if @use_resrv_los_ind = 1
							set @amt_subj_to_lba_ft = @amt_subj_to_lba_ft + @subj_resrv_idnmty_amt

						if @use_resv_alae_ind = 1
							set @amt_subj_to_lba_ft = @amt_subj_to_lba_ft + @subj_resrv_exps_amt


						set @lba_result = @amt_subj_to_lba_ft * @lba_fact
						set @lba_result=round(@lba_result,0)
						if @debug = 1
						begin
						print 'computed @lba_result: ' + convert(varchar(20), @lba_result )
						end
						end
						else
						begin
						set @lba_result=round(@lba_final_amt,0)
						end
					end --end of: if @lob_id = 428

					/**********
					* LCF
					**********/
					set @use_paid_los_ind = 0
					set @use_paid_alae_ind = 0
					set @use_resrv_los_ind = 0
					set @use_resv_alae_ind = 0

					select @use_paid_los_ind = [use_paid_los_ind]
						  ,@use_paid_alae_ind = [use_paid_aloc_los_adj_exps_ind]
						  ,@use_resrv_los_ind = [use_resrv_los_ind]
						  ,@use_resv_alae_ind = [use_resrv_aloc_los_adj_exps_ind]
					from dbo.INCUR_LOS_REIM_FUND_FRMLA
					where 
					prem_adj_pgm_id = @premium_adj_prog_id
					and custmr_id = @customer_id
					and los_reim_fund_fctr_typ_id = 406 -- lookup type: LRF Factor Type lookup value: LCF

					set @amt_subj_to_lcf_ft = 0
					if @use_paid_los_ind = 1
						set @amt_subj_to_lcf_ft = @amt_subj_to_lcf_ft + @subj_paid_idnmty_amt

					if @use_paid_alae_ind = 1
						set @amt_subj_to_lcf_ft = @amt_subj_to_lcf_ft + @subj_paid_exps_amt

					if @use_resrv_los_ind = 1
						set @amt_subj_to_lcf_ft = @amt_subj_to_lcf_ft + @subj_resrv_idnmty_amt

					if @use_resv_alae_ind = 1
						set @amt_subj_to_lcf_ft = @amt_subj_to_lcf_ft + @subj_resrv_exps_amt

					exec @lcf_fact = dbo.fn_RetrieveLCF
						@p_cust_id = @customer_id,
						@p_prem_adj_prog_id = @premium_adj_prog_id,
						@p_comm_agr_id = @com_agm_id,
						@p_lob_id = @lob_id,
						@p_state_id = @state_id
					if @debug = 1
					begin
					print 'retrieved @lcf_fact: ' + convert(varchar(20), @lcf_fact ) 
					end

					set @lcf_result = @amt_subj_to_lcf_ft * (@lcf_fact -1)
					set @lcf_result=round(@lcf_result,0)
					if @debug = 1
					begin
					print 'computed @lcf_result: ' + convert(varchar(20), @lcf_result ) 
					end

					/********
					* LDF
					********/
					if @ibnr_ldf_ft_id = 420 -- lookup type: ILRF PARAMETER lookup: LDF
					begin
						set @is_ibnr=0
						exec @ldf_rt = dbo.[fn_GetFactorsIBNR_LDF_For_ILRF]
											@p_com_agm_id = @com_agm_id,
											@p_premium_adj_prog_id = @premium_adj_prog_id,
											@p_customer_id = @customer_id,
											@p_is_ibnr=@is_ibnr
						if @debug = 1
						begin
						print 'retrieved @ldf_rt: ' + convert(varchar(20), @ldf_rt ) 
						end

						--LDF ONLY
						set @use_paid_los_ind = 0
						set @use_paid_alae_ind = 0
						set @use_resrv_los_ind = 0
						set @use_resv_alae_ind = 0

						select @use_paid_los_ind = [use_paid_los_ind]
							  ,@use_paid_alae_ind = [use_paid_aloc_los_adj_exps_ind]
							  ,@use_resrv_los_ind = [use_resrv_los_ind]
							  ,@use_resv_alae_ind = [use_resrv_aloc_los_adj_exps_ind]
						from dbo.INCUR_LOS_REIM_FUND_FRMLA
						where 
						prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and los_reim_fund_fctr_typ_id = 408 -- lookup type: LRF Factor Type lookup value: LDF

--						print 'LDF ONLY evaluated @use_paid_los_ind: ' + convert(varchar(20), @use_paid_los_ind ) 
--						print 'LDF ONLY evaluated @use_paid_alae_ind: ' + convert(varchar(20), @use_paid_alae_ind ) 
--						print 'LDF ONLY evaluated @use_resrv_los_ind: ' + convert(varchar(20), @use_resrv_los_ind ) 
--						print 'LDF ONLY evaluated @use_resv_alae_ind: ' + convert(varchar(20), @use_resv_alae_ind ) 

						set @amt_subj_to_ldf_ft = 0

						if @use_paid_los_ind = 1
							set @amt_subj_to_ldf_ft = @amt_subj_to_ldf_ft + @subj_paid_idnmty_amt

						if @use_paid_alae_ind = 1
							set @amt_subj_to_ldf_ft = @amt_subj_to_ldf_ft + @subj_paid_exps_amt

						if @use_resrv_los_ind = 1
							set @amt_subj_to_ldf_ft = @amt_subj_to_ldf_ft + @subj_resrv_idnmty_amt

						if @use_resv_alae_ind = 1
							set @amt_subj_to_ldf_ft = @amt_subj_to_ldf_ft + @subj_resrv_exps_amt


						--LDF LCF
						set @use_paid_los_ind = 0
						set @use_paid_alae_ind = 0
						set @use_resrv_los_ind = 0
						set @use_resv_alae_ind = 0

						select @use_paid_los_ind = [use_paid_los_ind]
							  ,@use_paid_alae_ind = [use_paid_aloc_los_adj_exps_ind]
							  ,@use_resrv_los_ind = [use_resrv_los_ind]
							  ,@use_resv_alae_ind = [use_resrv_aloc_los_adj_exps_ind]
						from dbo.INCUR_LOS_REIM_FUND_FRMLA
						where 
						prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and los_reim_fund_fctr_typ_id = 443 -- lookup type: LRF Factor Type lookup value: LCF-LDF

						--check the indicators
--						print 'LDF LCF evaluated @use_paid_los_ind: ' + convert(varchar(20), @use_paid_los_ind ) 
--						print 'LDF LCF evaluated @use_paid_alae_ind: ' + convert(varchar(20), @use_paid_alae_ind ) 
--						print 'LDF LCF evaluated @use_resrv_los_ind: ' + convert(varchar(20), @use_resrv_los_ind ) 
--						print 'LDF LCF evaluated @use_resv_alae_ind: ' + convert(varchar(20), @use_resv_alae_ind ) 

						set @amt_subj_to_ldf_lcf = 0

						if @use_paid_los_ind = 1
							set @amt_subj_to_ldf_lcf = @amt_subj_to_ldf_lcf + @subj_paid_idnmty_amt

						if @use_paid_alae_ind = 1
							set @amt_subj_to_ldf_lcf = @amt_subj_to_ldf_lcf + @subj_paid_exps_amt

						if @use_resrv_los_ind = 1
							set @amt_subj_to_ldf_lcf = @amt_subj_to_ldf_lcf + @subj_resrv_idnmty_amt

						if @use_resv_alae_ind = 1
							set @amt_subj_to_ldf_lcf = @amt_subj_to_ldf_lcf + @subj_resrv_exps_amt

						--LDF LBA
						set @use_paid_los_ind = 0
						set @use_paid_alae_ind = 0
						set @use_resrv_los_ind = 0
						set @use_resv_alae_ind = 0

						select @use_paid_los_ind = [use_paid_los_ind]
							  ,@use_paid_alae_ind = [use_paid_aloc_los_adj_exps_ind]
							  ,@use_resrv_los_ind = [use_resrv_los_ind]
							  ,@use_resv_alae_ind = [use_resrv_aloc_los_adj_exps_ind]
						from dbo.INCUR_LOS_REIM_FUND_FRMLA
						where 
						prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and los_reim_fund_fctr_typ_id = 410 -- lookup type: LRF Factor Type lookup value: LBA-LDF

						set @amt_subj_to_ldf_lba = 0

						if @use_paid_los_ind = 1
							set @amt_subj_to_ldf_lba = @amt_subj_to_ldf_lba + @subj_paid_idnmty_amt

						if @use_paid_alae_ind = 1
							set @amt_subj_to_ldf_lba = @amt_subj_to_ldf_lba + @subj_paid_exps_amt

						if @use_resrv_los_ind = 1
							set @amt_subj_to_ldf_lba = @amt_subj_to_ldf_lba + @subj_resrv_idnmty_amt

						if @use_resv_alae_ind = 1
							set @amt_subj_to_ldf_lba = @amt_subj_to_ldf_lba + @subj_resrv_exps_amt

						if @debug = 1
						begin
						print 'computed @amt_subj_to_ldf_ft: ' + convert(varchar(20), @amt_subj_to_ldf_ft ) 
						print 'computed @amt_subj_to_ldf_lcf: ' + convert(varchar(20), @amt_subj_to_ldf_lcf ) 
						print 'computed @amt_subj_to_ldf_lba: ' + convert(varchar(20), @amt_subj_to_ldf_lba ) 
						end

						set @ldf_result = (@amt_subj_to_ldf_ft * (@ldf_rt - 1)) + (@amt_subj_to_ldf_lcf * (@ldf_rt - 1) * (@lcf_fact -1)) + (@amt_subj_to_ldf_lba *(@ldf_rt - 1) * @lba_fact)
						set @ldf_result=round(@ldf_result,0)
						if @debug = 1
						begin
						print 'computed @ldf_result: ' + convert(varchar(20), @ldf_result ) 
						end

						set @ldf_ibnr_amt = @ldf_result
						if(@ldf_ibnr_lim_ind = 1)
						begin
							set @ldf_ibnr_amt = @ldf_ibnr_amt - @exc_ldf_ibnr_amt
						end

						set @ldf_ibnr_rt = @ldf_rt
					end --end of: if @ibnr_ldf_ft_id = 420
					/********
					* IBNR
					********/
					if @ibnr_ldf_ft_id = 419 -- lookup type: ILRF PARAMETER lookup: IBNR
					begin
						set @is_ibnr=1
						exec @ibnr_rt = dbo.[fn_GetFactorsIBNR_LDF_For_ILRF]
											@p_com_agm_id = @com_agm_id,
											@p_premium_adj_prog_id = @premium_adj_prog_id,
											@p_customer_id = @customer_id,
											@p_is_ibnr=@is_ibnr
						if @debug = 1
						begin
						print 'retrieved @ibnr_rt: ' + convert(varchar(20), @ibnr_rt ) 
						end

						--IBNR ONLY
						set @use_paid_los_ind = 0
						set @use_paid_alae_ind = 0
						set @use_resrv_los_ind = 0
						set @use_resv_alae_ind = 0

						select @use_paid_los_ind = [use_paid_los_ind]
							  ,@use_paid_alae_ind = [use_paid_aloc_los_adj_exps_ind]
							  ,@use_resrv_los_ind = [use_resrv_los_ind]
							  ,@use_resv_alae_ind = [use_resrv_aloc_los_adj_exps_ind]
						from dbo.INCUR_LOS_REIM_FUND_FRMLA
						where 
						prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and los_reim_fund_fctr_typ_id = 407 -- lookup type: LRF Factor Type lookup value: IBNR

						set @amt_subj_to_ibnr_ft = 0

						if @use_paid_los_ind = 1
							set @amt_subj_to_ibnr_ft = @amt_subj_to_ibnr_ft + @subj_paid_idnmty_amt

						if @use_paid_alae_ind = 1
							set @amt_subj_to_ibnr_ft = @amt_subj_to_ibnr_ft + @subj_paid_exps_amt

						if @use_resrv_los_ind = 1
							set @amt_subj_to_ibnr_ft = @amt_subj_to_ibnr_ft + @subj_resrv_idnmty_amt

						if @use_resv_alae_ind = 1
							set @amt_subj_to_ibnr_ft = @amt_subj_to_ibnr_ft + @subj_resrv_exps_amt


						--IBNR LCF
						set @use_paid_los_ind = 0
						set @use_paid_alae_ind = 0
						set @use_resrv_los_ind = 0
						set @use_resv_alae_ind = 0

						select @use_paid_los_ind = [use_paid_los_ind]
							  ,@use_paid_alae_ind = [use_paid_aloc_los_adj_exps_ind]
							  ,@use_resrv_los_ind = [use_resrv_los_ind]
							  ,@use_resv_alae_ind = [use_resrv_aloc_los_adj_exps_ind]
						from dbo.INCUR_LOS_REIM_FUND_FRMLA
						where 
						prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and los_reim_fund_fctr_typ_id = 411 -- lookup type: LRF Factor Type lookup value: LCF-IBNR

						set @amt_subj_to_ibnr_lcf = 0

						if @use_paid_los_ind = 1
							set @amt_subj_to_ibnr_lcf = @amt_subj_to_ibnr_lcf + @subj_paid_idnmty_amt

						if @use_paid_alae_ind = 1
							set @amt_subj_to_ibnr_lcf = @amt_subj_to_ibnr_lcf + @subj_paid_exps_amt

						if @use_resrv_los_ind = 1
							set @amt_subj_to_ibnr_lcf = @amt_subj_to_ibnr_lcf + @subj_resrv_idnmty_amt

						if @use_resv_alae_ind = 1
							set @amt_subj_to_ibnr_lcf = @amt_subj_to_ibnr_lcf + @subj_resrv_exps_amt

						--IBNR LBA
						set @use_paid_los_ind = 0
						set @use_paid_alae_ind = 0
						set @use_resrv_los_ind = 0
						set @use_resv_alae_ind = 0

						select @use_paid_los_ind = [use_paid_los_ind]
							  ,@use_paid_alae_ind = [use_paid_aloc_los_adj_exps_ind]
							  ,@use_resrv_los_ind = [use_resrv_los_ind]
							  ,@use_resv_alae_ind = [use_resrv_aloc_los_adj_exps_ind]
						from dbo.INCUR_LOS_REIM_FUND_FRMLA
						where 
						prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and los_reim_fund_fctr_typ_id = 409 -- lookup type: LRF Factor Type lookup value: LBA-IBNR

						set @amt_subj_to_ibnr_lba = 0

						if @use_paid_los_ind = 1
							set @amt_subj_to_ibnr_lba = @amt_subj_to_ibnr_lba + @subj_paid_idnmty_amt

						if @use_paid_alae_ind = 1
							set @amt_subj_to_ibnr_lba = @amt_subj_to_ibnr_lba + @subj_paid_exps_amt

						if @use_resrv_los_ind = 1
							set @amt_subj_to_ibnr_lba = @amt_subj_to_ibnr_lba + @subj_resrv_idnmty_amt

						if @use_resv_alae_ind = 1
							set @amt_subj_to_ibnr_lba = @amt_subj_to_ibnr_lba + @subj_resrv_exps_amt


						set @ibnr_result = (@amt_subj_to_ibnr_ft * (@ibnr_rt -1)) + (@amt_subj_to_ibnr_lcf * (@ibnr_rt -1) * (@lcf_fact -1)) + (@amt_subj_to_ibnr_lba * (@ibnr_rt -1) * @lba_fact)
						set @ibnr_result=round(@ibnr_result,0)
						if @debug = 1
						begin
						print 'computed @ibnr_result: ' + convert(varchar(20), @ibnr_result ) 
						end

						set @ldf_ibnr_amt = @ibnr_result
						if(@ldf_ibnr_lim_ind = 1)
						begin
							set @ldf_ibnr_amt = @ldf_ibnr_amt - @exc_ldf_ibnr_amt
						end

						set @ldf_ibnr_rt = @ibnr_rt
					end --end of: if @ibnr_ldf_ft_id = 419

					set @tot_amt = isnull(@subj_paid_idnmty_amt,0) + isnull(@subj_paid_exps_amt,0) + 
								   isnull(@subj_resrv_idnmty_amt,0) + isnull(@subj_resrv_exps_amt,0) +
								   isnull(@lba_result,0) + isnull(@lcf_result,0) + isnull(@ldf_ibnr_amt,0)
					if @debug = 1
					begin
				    print'---------------Computed Values-------------------' 
					end

					set @lba_result = round(@lba_result ,0)
					set @lcf_result = round(@lcf_result ,0)
					set @ldf_ibnr_amt = round(@ldf_ibnr_amt ,0)
					set @tot_amt = round(@tot_amt ,0)

					if @debug = 1
					begin
					print' @lba_result:- ' + convert(varchar(20), @lba_result)
					print' @lcf_result:- ' + convert(varchar(20), @lcf_result)
					print' @ldf_ibnr_amt:- ' + convert(varchar(20), @ldf_ibnr_amt)
					print' @tot_amt:- ' + convert(varchar(20), @tot_amt)
					end
						
						exec [dbo].[AddPREM_ADJ_PARMET_DTL]
							@prem_adj_parmet_setup_id = @prem_adj_parmet_setup_id, 
							@premium_adj_period_id = @premium_adj_period_id,
							@premium_adjustment_id = @premium_adjustment_id,
							@customer_id = @customer_id,
							@premium_adj_prog_id = @premium_adj_prog_id,
							@coml_agmt_id = @com_agm_id,
							@state_id = @state_id,
							@lob_id = @lob_id,
							@loss_amt = @loss_amt,
							@paid_loss = @subj_paid_idnmty_amt,
							@paid_alae = @subj_paid_exps_amt,
							@resv_loss = @subj_resrv_idnmty_amt,
							@resv_alae = @subj_resrv_exps_amt,
							@lba_rt = @lba_fact,
							@lba_amt = @lba_result,
							@lcf_rt = @lcf_fact,
							@lcf_amt = @lcf_result,
							@ldf_rt = @ldf_ibnr_rt,
							@ldf_amt = @ldf_ibnr_amt,
							@total_amt = @tot_amt,
							@create_user_id = @create_user_id
							

				end
				fetch ilrf_base_cur into @prem_adj_pgm_setup_id,  @ibnr_ldf_ft_id, @com_agm_id, @state_id, 
					/*@paid_idnmty__amt , @paid_exps_amt , @resrv_idnmty_amt , @resrv_exps_amt ,*/ 
					@subj_paid_idnmty_amt, @subj_paid_exps_amt, @subj_resrv_idnmty_amt, @subj_resrv_exps_amt

			end --end of cursor ilrf_base_cur / while loop
		close ilrf_base_cur
		deallocate ilrf_base_cur
		
		
		--***************************************************************************************--
								--10774 Bug fix starts from here
		--***************************************************************************************--
		
		--retrieving @lcf_aggr_cap_set_pgm_amt from PREM_ADJ_PGM_SETUP
		select @lcf_aggr_cap_set_pgm_amt = isnull(los_conv_fctr_aggr_cap_amt,0) 
					from dbo.PREM_ADJ_PGM_SETUP 
					where 
					custmr_id = @customer_id 
					and prem_adj_pgm_id = @premium_adj_prog_id
					and actv_ind = 1
					and adj_parmet_typ_id = 402 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> LCF

		set @lcf_aggr_cap_set_pgm_amt = isnull(@lcf_aggr_cap_set_pgm_amt,0)
		
		--retrieving @sum_lcf_amt for this program period which is calculated using lcf factor and losses
		select @sum_lcf_amt=sum(los_conv_fctr_amt) from PREM_ADJ_PARMET_DTL
		where prem_adj_perd_id=@premium_adj_period_id 
		and prem_adj_id=@premium_adjustment_id 
		and custmr_id=@customer_id
		and prem_adj_pgm_id=@premium_adj_prog_id

		select @state_count=count(st_id) 
		from PREM_ADJ_PARMET_DTL
		where prem_adj_perd_id=@premium_adj_period_id 
		and prem_adj_id=@premium_adjustment_id 
		and custmr_id=@customer_id
		and prem_adj_pgm_id=@premium_adj_prog_id
		and los_conv_fctr_amt is not null 
		and los_conv_fctr_amt<>0

		set @state_counter=@state_count
		
		set @sum_lcf_amt_used=0

					--only if below two conditions are satisfied then only we need to perform this calculations
		
					if (@lcf_aggr_cap_set_pgm_amt <> 0 and (@lcf_aggr_cap_set_pgm_amt<@sum_lcf_amt))
					begin
					
					--**************************************************************************************--
								--Start of Second Iteration for dividing LCF cap amount in actual lcf ration
					--**************************************************************************************--
					declare ilrf_lcf_cap_base_cur cursor LOCAL FAST_FORWARD READ_ONLY 
					for 
					select 
					coml_agmt_id,
					st_id,
					paid_los_amt,
					paid_aloc_los_adj_exps_amt,
					resrv_los_amt,
					resrv_aloc_los_adj_exps_amt
					from PREM_ADJ_PARMET_DTL papd
					inner join PREM_ADJ_PARMET_SETUP on PREM_ADJ_PARMET_SETUP.prem_adj_parmet_setup_id=papd.prem_adj_parmet_setup_id
					where 
					papd.prem_adj_perd_id=@premium_adj_period_id
					and papd.prem_adj_id=@premium_adjustment_id
					and papd.custmr_id=@customer_id
					and papd.prem_adj_pgm_id=@premium_adj_prog_id
					and adj_parmet_typ_id=400
					

					open ilrf_lcf_cap_base_cur
					fetch ilrf_lcf_cap_base_cur into @com_agm_id, @state_id, 
					@subj_paid_idnmty_amt, @subj_paid_exps_amt, @subj_resrv_idnmty_amt, @subj_resrv_exps_amt

		while @@Fetch_Status = 0
			begin
				begin
			
					set @subj_paid_idnmty_amt = isnull(@subj_paid_idnmty_amt,0)
					set @subj_paid_exps_amt = isnull(@subj_paid_exps_amt,0)
					set @subj_resrv_idnmty_amt = isnull(@subj_resrv_idnmty_amt,0)
					set @subj_resrv_exps_amt = isnull(@subj_resrv_exps_amt,0)
					set @lba_result = null
					

				

					select @ibnr_ldf_ft_id=incur_but_not_rptd_los_dev_fctr_id from prem_adj_pgm_setup
					where prem_adj_pgm_id=@premium_adj_prog_id


					select 
					@ldf_ibnr_lim_ind = los_dev_fctr_incur_but_not_rptd_incld_lim_ind 
					from dbo.COML_AGMT 
					where coml_agmt_id = @com_agm_id and actv_ind = 1

					if(@ldf_ibnr_lim_ind = 1)
					begin
						select @exc_ldf_ibnr_amt = round(isnull(sum(exc_ldf_ibnr_amt),0),0) 
						from ARMIS_LOS_POL 
						where prem_adj_pgm_id = @premium_adj_prog_id 
						and coml_agmt_id = @com_agm_id 
						and st_id = @state_id
						and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) 
					end


					--Determine line of business
					select @lob = attr_1_txt 
					from dbo.LKUP 
					where lkup_id = (
										select 
										covg_typ_id 
										from dbo.COML_AGMT 
										where coml_agmt_id = @com_agm_id
										and prem_adj_pgm_id = @premium_adj_prog_id
										and custmr_id = @customer_id
										and actv_ind = 1
									)
					and lkup_typ_id in (
											select 
											lkup_typ_id 
											from dbo.LKUP_TYP 
											where lkup_typ_nm_txt like 'LOB COVERAGE'
										)
					set @lob_id = dbo.fn_GetIDForLOB(@lob)
					if @debug = 1
					begin
					print 'retrieved @lob_id in second pass: ' + convert(varchar(20), @lob_id ) 
					end

					/**********
					* LBA
					**********/
					
						select @lba_result=los_base_asses_amt from PREM_ADJ_PARMET_DTL
						where prem_adj_perd_id=@premium_adj_period_id 
						and prem_adj_id=@premium_adjustment_id 
						and custmr_id=@customer_id
						and prem_adj_pgm_id=@premium_adj_prog_id
						and coml_agmt_id=@com_agm_id
						and st_id=@state_id

												

					/**********
					* LCF
					**********/
					exec @lcf_aggr_cap_state_amt = [dbo].[fn_RetrieveLCF_Aggr_Cap_Amt]
 					@p_cust_id=@customer_id,
					@p_prem_adj_prog_id=@premium_adj_prog_id,
					@p_prem_adj_perd_id=@premium_adj_period_id,
					@p_prem_adj_id=@premium_adjustment_id,
					@p_coml_agmt_id=@com_agm_id,
					@p_state_id=@state_id,
					@p_sum_lcf_amt=@sum_lcf_amt,
					@p_sum_lcf_amt_used=@sum_lcf_amt_used,
					@p_state_counter=@state_counter

					set @lcf_result = @lcf_aggr_cap_state_amt
					set @lcf_result=round(@lcf_result,0)
					set @sum_lcf_amt_used=@sum_lcf_amt_used + isnull(@lcf_result,0)
					if @debug = 1
					begin
					print 'computed @lcf_result in second pass: ' + convert(varchar(20), @lcf_result ) 
					end

					/********
					* LDF
					********/
					if @ibnr_ldf_ft_id = 420 -- lookup type: ILRF PARAMETER lookup: LDF
					begin
						set @is_ibnr=0
						exec @ldf_rt = dbo.[fn_GetFactorsIBNR_LDF_For_ILRF]
											@p_com_agm_id = @com_agm_id,
											@p_premium_adj_prog_id = @premium_adj_prog_id,
											@p_customer_id = @customer_id,
											@p_is_ibnr=@is_ibnr
						if @debug = 1
						begin
						print 'retrieved @ldf_rt in second pass: ' + convert(varchar(20), @ldf_rt ) 
						end

						--LDF ONLY
						set @use_paid_los_ind = 0
						set @use_paid_alae_ind = 0
						set @use_resrv_los_ind = 0
						set @use_resv_alae_ind = 0

						select @use_paid_los_ind = [use_paid_los_ind]
							  ,@use_paid_alae_ind = [use_paid_aloc_los_adj_exps_ind]
							  ,@use_resrv_los_ind = [use_resrv_los_ind]
							  ,@use_resv_alae_ind = [use_resrv_aloc_los_adj_exps_ind]
						from dbo.INCUR_LOS_REIM_FUND_FRMLA
						where 
						prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and los_reim_fund_fctr_typ_id = 408 -- lookup type: LRF Factor Type lookup value: LDF

--						print 'LDF ONLY evaluated @use_paid_los_ind: ' + convert(varchar(20), @use_paid_los_ind ) 
--						print 'LDF ONLY evaluated @use_paid_alae_ind: ' + convert(varchar(20), @use_paid_alae_ind ) 
--						print 'LDF ONLY evaluated @use_resrv_los_ind: ' + convert(varchar(20), @use_resrv_los_ind ) 
--						print 'LDF ONLY evaluated @use_resv_alae_ind: ' + convert(varchar(20), @use_resv_alae_ind ) 

						set @amt_subj_to_ldf_ft = 0

						if @use_paid_los_ind = 1
							set @amt_subj_to_ldf_ft = @amt_subj_to_ldf_ft + @subj_paid_idnmty_amt

						if @use_paid_alae_ind = 1
							set @amt_subj_to_ldf_ft = @amt_subj_to_ldf_ft + @subj_paid_exps_amt

						if @use_resrv_los_ind = 1
							set @amt_subj_to_ldf_ft = @amt_subj_to_ldf_ft + @subj_resrv_idnmty_amt

						if @use_resv_alae_ind = 1
							set @amt_subj_to_ldf_ft = @amt_subj_to_ldf_ft + @subj_resrv_exps_amt


						--LDF LCF
						set @use_paid_los_ind = 0
						set @use_paid_alae_ind = 0
						set @use_resrv_los_ind = 0
						set @use_resv_alae_ind = 0

						select @use_paid_los_ind = [use_paid_los_ind]
							  ,@use_paid_alae_ind = [use_paid_aloc_los_adj_exps_ind]
							  ,@use_resrv_los_ind = [use_resrv_los_ind]
							  ,@use_resv_alae_ind = [use_resrv_aloc_los_adj_exps_ind]
						from dbo.INCUR_LOS_REIM_FUND_FRMLA
						where 
						prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and los_reim_fund_fctr_typ_id = 443 -- lookup type: LRF Factor Type lookup value: LCF-LDF

						--check the indicators
--						print 'LDF LCF evaluated @use_paid_los_ind: ' + convert(varchar(20), @use_paid_los_ind ) 
--						print 'LDF LCF evaluated @use_paid_alae_ind: ' + convert(varchar(20), @use_paid_alae_ind ) 
--						print 'LDF LCF evaluated @use_resrv_los_ind: ' + convert(varchar(20), @use_resrv_los_ind ) 
--						print 'LDF LCF evaluated @use_resv_alae_ind: ' + convert(varchar(20), @use_resv_alae_ind ) 

						set @amt_subj_to_ldf_lcf = 0

						if @use_paid_los_ind = 1
							set @amt_subj_to_ldf_lcf = @amt_subj_to_ldf_lcf + @subj_paid_idnmty_amt

						if @use_paid_alae_ind = 1
							set @amt_subj_to_ldf_lcf = @amt_subj_to_ldf_lcf + @subj_paid_exps_amt

						if @use_resrv_los_ind = 1
							set @amt_subj_to_ldf_lcf = @amt_subj_to_ldf_lcf + @subj_resrv_idnmty_amt

						if @use_resv_alae_ind = 1
							set @amt_subj_to_ldf_lcf = @amt_subj_to_ldf_lcf + @subj_resrv_exps_amt

						--LDF LBA
						set @use_paid_los_ind = 0
						set @use_paid_alae_ind = 0
						set @use_resrv_los_ind = 0
						set @use_resv_alae_ind = 0

						select @use_paid_los_ind = [use_paid_los_ind]
							  ,@use_paid_alae_ind = [use_paid_aloc_los_adj_exps_ind]
							  ,@use_resrv_los_ind = [use_resrv_los_ind]
							  ,@use_resv_alae_ind = [use_resrv_aloc_los_adj_exps_ind]
						from dbo.INCUR_LOS_REIM_FUND_FRMLA
						where 
						prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and los_reim_fund_fctr_typ_id = 410 -- lookup type: LRF Factor Type lookup value: LBA-LDF

						set @amt_subj_to_ldf_lba = 0

						if @use_paid_los_ind = 1
							set @amt_subj_to_ldf_lba = @amt_subj_to_ldf_lba + @subj_paid_idnmty_amt

						if @use_paid_alae_ind = 1
							set @amt_subj_to_ldf_lba = @amt_subj_to_ldf_lba + @subj_paid_exps_amt

						if @use_resrv_los_ind = 1
							set @amt_subj_to_ldf_lba = @amt_subj_to_ldf_lba + @subj_resrv_idnmty_amt

						if @use_resv_alae_ind = 1
							set @amt_subj_to_ldf_lba = @amt_subj_to_ldf_lba + @subj_resrv_exps_amt

						if @debug = 1
						begin
						print 'computed @amt_subj_to_ldf_ft in second pass: ' + convert(varchar(20), @amt_subj_to_ldf_ft ) 
						print 'computed @amt_subj_to_ldf_lcf in second pass: ' + convert(varchar(20), @amt_subj_to_ldf_lcf ) 
						print 'computed @amt_subj_to_ldf_lba in second pass: ' + convert(varchar(20), @amt_subj_to_ldf_lba ) 
						end

						set @ldf_result = (@amt_subj_to_ldf_ft * (@ldf_rt - 1)) + (@lcf_result * (@ldf_rt - 1) ) + (@amt_subj_to_ldf_lba *(@ldf_rt - 1) * @lba_fact)
						set @ldf_result=round(@ldf_result,0)
						if @debug = 1
						begin
						print 'computed @ldf_result in second pass: ' + convert(varchar(20), @ldf_result ) 
						end

						set @ldf_ibnr_amt = @ldf_result
						if(@ldf_ibnr_lim_ind = 1)
						begin
							set @ldf_ibnr_amt = @ldf_ibnr_amt - @exc_ldf_ibnr_amt
						end

						set @ldf_ibnr_rt = @ldf_rt
					end --end of: if @ibnr_ldf_ft_id = 420
					/********
					* IBNR
					********/
					if @ibnr_ldf_ft_id = 419 -- lookup type: ILRF PARAMETER lookup: IBNR
					begin
						set @is_ibnr=1
						exec @ibnr_rt = dbo.[fn_GetFactorsIBNR_LDF_For_ILRF]
											@p_com_agm_id = @com_agm_id,
											@p_premium_adj_prog_id = @premium_adj_prog_id,
											@p_customer_id = @customer_id,
											@p_is_ibnr=@is_ibnr
						if @debug = 1
						begin
						print 'retrieved @ibnr_rt in second pass: ' + convert(varchar(20), @ibnr_rt ) 
						end

						--IBNR ONLY
						set @use_paid_los_ind = 0
						set @use_paid_alae_ind = 0
						set @use_resrv_los_ind = 0
						set @use_resv_alae_ind = 0

						select @use_paid_los_ind = [use_paid_los_ind]
							  ,@use_paid_alae_ind = [use_paid_aloc_los_adj_exps_ind]
							  ,@use_resrv_los_ind = [use_resrv_los_ind]
							  ,@use_resv_alae_ind = [use_resrv_aloc_los_adj_exps_ind]
						from dbo.INCUR_LOS_REIM_FUND_FRMLA
						where 
						prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and los_reim_fund_fctr_typ_id = 407 -- lookup type: LRF Factor Type lookup value: IBNR

						set @amt_subj_to_ibnr_ft = 0

						if @use_paid_los_ind = 1
							set @amt_subj_to_ibnr_ft = @amt_subj_to_ibnr_ft + @subj_paid_idnmty_amt

						if @use_paid_alae_ind = 1
							set @amt_subj_to_ibnr_ft = @amt_subj_to_ibnr_ft + @subj_paid_exps_amt

						if @use_resrv_los_ind = 1
							set @amt_subj_to_ibnr_ft = @amt_subj_to_ibnr_ft + @subj_resrv_idnmty_amt

						if @use_resv_alae_ind = 1
							set @amt_subj_to_ibnr_ft = @amt_subj_to_ibnr_ft + @subj_resrv_exps_amt


						--IBNR LCF
						set @use_paid_los_ind = 0
						set @use_paid_alae_ind = 0
						set @use_resrv_los_ind = 0
						set @use_resv_alae_ind = 0

						select @use_paid_los_ind = [use_paid_los_ind]
							  ,@use_paid_alae_ind = [use_paid_aloc_los_adj_exps_ind]
							  ,@use_resrv_los_ind = [use_resrv_los_ind]
							  ,@use_resv_alae_ind = [use_resrv_aloc_los_adj_exps_ind]
						from dbo.INCUR_LOS_REIM_FUND_FRMLA
						where 
						prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and los_reim_fund_fctr_typ_id = 411 -- lookup type: LRF Factor Type lookup value: LCF-IBNR

						set @amt_subj_to_ibnr_lcf = 0

						if @use_paid_los_ind = 1
							set @amt_subj_to_ibnr_lcf = @amt_subj_to_ibnr_lcf + @subj_paid_idnmty_amt

						if @use_paid_alae_ind = 1
							set @amt_subj_to_ibnr_lcf = @amt_subj_to_ibnr_lcf + @subj_paid_exps_amt

						if @use_resrv_los_ind = 1
							set @amt_subj_to_ibnr_lcf = @amt_subj_to_ibnr_lcf + @subj_resrv_idnmty_amt

						if @use_resv_alae_ind = 1
							set @amt_subj_to_ibnr_lcf = @amt_subj_to_ibnr_lcf + @subj_resrv_exps_amt

						--IBNR LBA
						set @use_paid_los_ind = 0
						set @use_paid_alae_ind = 0
						set @use_resrv_los_ind = 0
						set @use_resv_alae_ind = 0

						select @use_paid_los_ind = [use_paid_los_ind]
							  ,@use_paid_alae_ind = [use_paid_aloc_los_adj_exps_ind]
							  ,@use_resrv_los_ind = [use_resrv_los_ind]
							  ,@use_resv_alae_ind = [use_resrv_aloc_los_adj_exps_ind]
						from dbo.INCUR_LOS_REIM_FUND_FRMLA
						where 
						prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and los_reim_fund_fctr_typ_id = 409 -- lookup type: LRF Factor Type lookup value: LBA-IBNR

						set @amt_subj_to_ibnr_lba = 0

						if @use_paid_los_ind = 1
							set @amt_subj_to_ibnr_lba = @amt_subj_to_ibnr_lba + @subj_paid_idnmty_amt

						if @use_paid_alae_ind = 1
							set @amt_subj_to_ibnr_lba = @amt_subj_to_ibnr_lba + @subj_paid_exps_amt

						if @use_resrv_los_ind = 1
							set @amt_subj_to_ibnr_lba = @amt_subj_to_ibnr_lba + @subj_resrv_idnmty_amt

						if @use_resv_alae_ind = 1
							set @amt_subj_to_ibnr_lba = @amt_subj_to_ibnr_lba + @subj_resrv_exps_amt


						set @ibnr_result = (@amt_subj_to_ibnr_ft * (@ibnr_rt -1)) + (@lcf_result * (@ibnr_rt -1)) + (@amt_subj_to_ibnr_lba * (@ibnr_rt -1) * @lba_fact)
						set @ibnr_result=round(@ibnr_result,0)
						if @debug = 1
						begin
						print 'computed @ibnr_result in second pass: ' + convert(varchar(20), @ibnr_result ) 
						end

						set @ldf_ibnr_amt = @ibnr_result
						if(@ldf_ibnr_lim_ind = 1)
						begin
							set @ldf_ibnr_amt = @ldf_ibnr_amt - @exc_ldf_ibnr_amt
						end

						set @ldf_ibnr_rt = @ibnr_rt
					end --end of: if @ibnr_ldf_ft_id = 419

					set @tot_amt = isnull(@subj_paid_idnmty_amt,0) + isnull(@subj_paid_exps_amt,0) + 
								   isnull(@subj_resrv_idnmty_amt,0) + isnull(@subj_resrv_exps_amt,0) +
								   isnull(@lba_result,0) + isnull(@lcf_result,0) + isnull(@ldf_ibnr_amt,0)
					if @debug = 1
					begin
				    print'---------------Computed Values-------------------' 
					end

					set @lba_result = round(@lba_result ,0)
					set @lcf_result = round(@lcf_result ,0)
					set @ldf_ibnr_amt = round(@ldf_ibnr_amt ,0)
					set @tot_amt = round(@tot_amt ,0)

					if @debug = 1
					begin
					print' @lba_result in second pass:- ' + convert(varchar(20), @lba_result)
					print' @lcf_result in second pass:- ' + convert(varchar(20), @lcf_result)
					print' @ldf_ibnr_amt in second pass:- ' + convert(varchar(20), @ldf_ibnr_amt)
					print' @tot_amt in second pass:- ' + convert(varchar(20), @tot_amt)
					end


					update PREM_ADJ_PARMET_DTL
					set los_conv_fctr_amt=@lcf_result,
					los_dev_fctr_amt=@ldf_ibnr_amt,
					tot_amt=@tot_amt
					where 
					prem_adj_perd_id=@premium_adj_period_id
					and prem_adj_id=@premium_adjustment_id
					and custmr_id=@customer_id
					and prem_adj_pgm_id=@premium_adj_prog_id
					and coml_agmt_id=@com_agm_id
					and st_id=@state_id

					select @lcf_state_amt=los_conv_fctr_amt from PREM_ADJ_PARMET_DTL
					where prem_adj_perd_id=@premium_adj_period_id 
					and prem_adj_id=@premium_adjustment_id 
					and custmr_id=@customer_id
					and prem_adj_pgm_id=@premium_adj_prog_id
					and coml_agmt_id=@com_agm_id
					and st_id=@state_id

					if(@lcf_state_amt is not null and @lcf_state_amt<>0)
					begin
					set @state_counter=@state_counter-1
					end

				end
				fetch ilrf_lcf_cap_base_cur into @com_agm_id, @state_id, 
					@subj_paid_idnmty_amt, @subj_paid_exps_amt, @subj_resrv_idnmty_amt, @subj_resrv_exps_amt

			end --end of cursor ilrf_base_cur / while loop
		close ilrf_lcf_cap_base_cur
		deallocate ilrf_lcf_cap_base_cur


					end--end of @lcf_aggr_cap_set_pgm_amt
		

		--***************************************************************************************--
								--10774 Bug fix End
		--***************************************************************************************--

		
		/***********************************************
		* DETERMINE PREVIOUS VALID ADJUSTMENT AND POPULATE
		* CORRESPONDING PREVIOUS ILRF AMT
		***********************************************/
		exec @prev_valid_adj_id = [dbo].[fn_DeterminePrevValidAdj_NonPremium]
 			@current_premium_adjustment_id = @premium_adjustment_id,
			@customer_id = @customer_id,
			@premium_adj_prog_id = @premium_adj_prog_id,
			@adj_parmet_typ_id = 400  -- Adjustment Parameter Type for ILRF

		--------begin: changes for bug # 10235---------------
		declare @cnt_states int
		declare @cnt_paramet_setup int
		declare @prem_adj_paramet_setup_id int
		set @cnt_paramet_setup=0
		set @cnt_states=0
		select  @cnt_states=count(st_id)
		from 
		dbo.PREM_ADJ_PARMET_DTL 
		where  prem_adj_id = @premium_adjustment_id
		and prem_adj_perd_id = @premium_adj_period_id
		and custmr_id = @customer_id
		and prem_adj_parmet_setup_id 
		in(select prem_adj_parmet_setup_id from dbo.PREM_ADJ_PARMET_SETUP 
		where 
		prem_adj_id=@premium_adjustment_id 
		and adj_parmet_typ_id=400
		and prem_adj_perd_id=@premium_adj_period_id
		)
if(@cnt_states=0)
begin
select @cnt_paramet_setup=count(*) from dbo.PREM_ADJ_PARMET_SETUP 
where prem_adj_id=@premium_adjustment_id 
and adj_parmet_typ_id=400 
and prem_adj_pgm_id=@premium_adj_prog_id
if(@cnt_paramet_setup>0)
begin
			
		select @prem_adj_paramet_setup_id = max(prem_adj_parmet_setup_id)
		from dbo.PREM_ADJ_PARMET_SETUP
		where  prem_adj_id = @premium_adjustment_id
		and prem_adj_perd_id = @premium_adj_period_id
		and custmr_id = @customer_id
		and adj_parmet_typ_id=400 
end
else
begin
	insert into dbo.PREM_ADJ_PARMET_SETUP
		(
		 [prem_adj_perd_id]
		,[prem_adj_id]
		,[custmr_id]
		,[prem_adj_pgm_setup_id]
		,[prem_adj_pgm_id]
		,[adj_parmet_typ_id]
		,[crte_user_id]
		,[crte_dt]
		)
		select
		@premium_adj_period_id,
		@premium_adjustment_id,
		@customer_id,
		prem_adj_pgm_setup_id,
		@premium_adj_prog_id,
		400,
		@create_user_id,
		getdate()
		from
		PREM_ADJ_PARMET_SETUP
		where  prem_adj_id = @prev_valid_adj_id
		and prem_adj_pgm_id = @premium_adj_prog_id
		and custmr_id = @customer_id
		and adj_parmet_typ_id=400

		select @prem_adj_paramet_setup_id = max(prem_adj_parmet_setup_id)
		from dbo.PREM_ADJ_PARMET_SETUP
		where  prem_adj_id = @premium_adjustment_id
		and prem_adj_perd_id = @premium_adj_period_id
		and custmr_id = @customer_id
		and adj_parmet_typ_id=400

end

insert into dbo.PREM_ADJ_PARMET_DTL
		(
		[prem_adj_parmet_setup_id]
		,[prem_adj_perd_id]
		,[prem_adj_id]
		,[custmr_id]
		,[prem_adj_pgm_id]
		,[coml_agmt_id]
		,[st_id]
		,[ln_of_bsn_id]
		,[crte_user_id]
		,[crte_dt]
		)
		select 
		@prem_adj_paramet_setup_id,
		@premium_adj_period_id,
		@premium_adjustment_id,
		@customer_id,
		@premium_adj_prog_id,
		coml_agmt_id,
		st_id,
		ln_of_bsn_id,
		@create_user_id,
		getdate()
		from 
		(
			select
			custmr_id,
			prem_adj_pgm_id,
			prem_adj_id,
			prem_adj_perd_id,
			coml_agmt_id,
			st_id,
			ln_of_bsn_id
			from dbo.PREM_ADJ_PARMET_DTL 
			where  prem_adj_id = @prev_valid_adj_id
			and custmr_id = @customer_id
			and prem_adj_pgm_id = @premium_adj_prog_id
			and prem_adj_parmet_setup_id 
			in(select prem_adj_parmet_setup_id from dbo.PREM_ADJ_PARMET_SETUP 
			where 
			prem_adj_id=@prev_valid_adj_id 
			and adj_parmet_typ_id=400 
			and prem_adj_pgm_id=@premium_adj_prog_id
			)
		) as prev
		where not exists
		(
			select * 
			from 
			(
				select
				custmr_id,
				prem_adj_pgm_id,
				prem_adj_id,
				coml_agmt_id,
				st_id,
				ln_of_bsn_id
				from dbo.PREM_ADJ_PARMET_DTL 
				where  prem_adj_id = @premium_adjustment_id
				and prem_adj_perd_id = @premium_adj_period_id
				and custmr_id = @customer_id
				and prem_adj_parmet_setup_id 
			in(select prem_adj_parmet_setup_id from dbo.PREM_ADJ_PARMET_SETUP 
			where 
			prem_adj_id=@premium_adjustment_id 
			and adj_parmet_typ_id=400 
			and prem_adj_perd_id=@premium_adj_period_id
			)
			) as curr
			where prev.coml_agmt_id = curr.coml_agmt_id
			and prev.st_id = curr.st_id
		)


end
--------end: changes for bug # 10235------------



		/**********************
		* ILRF Posting
		**********************/
		if(@delete_ilrf=1)
		begin
		if not exists(select 1 from [dbo].[PREM_ADJ_LOS_REIM_FUND_POST] where [prem_adj_perd_id] = @premium_adj_period_id and [prem_adj_id] = @premium_adjustment_id)
		begin

			insert into [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
		   (
			[prem_adj_perd_id]
		   ,[prem_adj_id]
		   ,[custmr_id]
		   ,[crte_user_id]
		   ,[recv_typ_id]
			)
			select
			@premium_adj_period_id,
			@premium_adjustment_id,
			@customer_id,
			@create_user_id,
			post_trns_typ_id 
			from dbo.POST_TRNS_TYP 
			where trns_typ_id = 328 --Transaction types associated with 'ILRF Deductible'.
			and post_trns_typ_id in (31,29,30,34,32,33,70,71)
			and actv_ind = 1

		end

		--Evaluate current amount
		select 
		@aggr_wc_tpd_amt = sum(case when (ln_of_bsn_id = 428) then isnull(d.paid_los_amt,0) + isnull(d.paid_aloc_los_adj_exps_amt,0) else 0 end) , -- as WC_TPD ,
		@aggr_auto_tpd_amt = sum(case when (ln_of_bsn_id = 426) then isnull(d.paid_los_amt,0) + isnull(d.paid_aloc_los_adj_exps_amt,0) else 0 end) , -- as Auto_TPD ,
		@aggr_gl_tpd_amt = sum(case when (ln_of_bsn_id = 427) then isnull(d.paid_los_amt,0) + isnull(d.paid_aloc_los_adj_exps_amt,0) else 0 end) , -- as GL_TPD, 
		@aggr_wc_lcf_amt = sum(case when (ln_of_bsn_id = 428) then isnull(d.los_conv_fctr_amt,0) else 0 end) , -- as WC_LCF, 
		@aggr_auto_lcf_amt = sum(case when (ln_of_bsn_id = 426) then isnull(d.los_conv_fctr_amt,0) else 0 end) , -- as Auto_LCF ,
		@aggr_gl_lcf_amt = sum(case when (ln_of_bsn_id = 427) then isnull(d.los_conv_fctr_amt,0) else 0 end) , -- as GL_LCF,
		@aggr_resv_amt = sum( isnull(d.los_dev_fctr_amt,0) + isnull(d.resrv_los_amt,0) + isnull(d.resrv_aloc_los_adj_exps_amt,0) )  , --as RESV,
		@aggr_lba_amt = sum( isnull(d.los_base_asses_amt,0) )  --as LBA
		from 
		PREM_ADJ_PARMET_DTL d
		join dbo.PREM_ADJ_PARMET_SETUP as s 
		on s.prem_adj_parmet_setup_id = d.prem_adj_parmet_setup_id
		and s.prem_adj_perd_id = d.prem_adj_perd_id
		and s.prem_adj_id = d.prem_adj_id
		where s.prem_adj_id = @premium_adjustment_id
		and s.prem_adj_perd_id = @premium_adj_period_id
		and s.custmr_id = @customer_id
		and s.adj_parmet_typ_id = 400

		update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
		set curr_amt = isnull(@aggr_wc_tpd_amt,0)
		where [prem_adj_perd_id] = @premium_adj_period_id
		and [prem_adj_id] = @premium_adjustment_id
		and [custmr_id] = @customer_id
		and recv_typ_id = 31 -- lookup type: RECEIVABLE TYPE, lookup value: WC TPD

		update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
		set curr_amt = isnull(@aggr_auto_tpd_amt,0)
		where [prem_adj_perd_id] = @premium_adj_period_id
		and [prem_adj_id] = @premium_adjustment_id
		and [custmr_id] = @customer_id
		and recv_typ_id = 29 -- lookup type: RECEIVABLE TYPE, lookup value: Auto TPD

		update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
		set curr_amt = isnull(@aggr_gl_tpd_amt,0)
		where [prem_adj_perd_id] = @premium_adj_period_id
		and [prem_adj_id] = @premium_adjustment_id
		and [custmr_id] = @customer_id
		and recv_typ_id = 30 -- lookup type: RECEIVABLE TYPE, lookup value: GL TPD

		update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
		set curr_amt = isnull(@aggr_wc_lcf_amt,0)
		where [prem_adj_perd_id] = @premium_adj_period_id
		and [prem_adj_id] = @premium_adjustment_id
		and [custmr_id] = @customer_id
		and recv_typ_id = 34 -- lookup type: RECEIVABLE TYPE, lookup value: WC LCF

		update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
		set curr_amt = isnull(@aggr_auto_lcf_amt,0)
		where [prem_adj_perd_id] = @premium_adj_period_id
		and [prem_adj_id] = @premium_adjustment_id
		and [custmr_id] = @customer_id
		and recv_typ_id = 32 -- lookup type: RECEIVABLE TYPE, lookup value: Auto LCF

		update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
		set curr_amt = isnull(@aggr_gl_lcf_amt,0)
		where [prem_adj_perd_id] = @premium_adj_period_id
		and [prem_adj_id] = @premium_adjustment_id
		and [custmr_id] = @customer_id
		and recv_typ_id = 33 -- lookup type: RECEIVABLE TYPE, lookup value: GL LCF


		update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
		set curr_amt = isnull(@aggr_resv_amt,0)
		where [prem_adj_perd_id] = @premium_adj_period_id
		and [prem_adj_id] = @premium_adjustment_id
		and [custmr_id] = @customer_id
		and recv_typ_id = 71 -- lookup type: RECEIVABLE TYPE, lookup value: Reserve

		update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
		set curr_amt = isnull(@aggr_lba_amt,0)
		where [prem_adj_perd_id] = @premium_adj_period_id
		and [prem_adj_id] = @premium_adjustment_id
		and [custmr_id] = @customer_id
		and recv_typ_id = 70 -- lookup type: RECEIVABLE TYPE, lookup value: LBA


		--Evaluate prior amount
		select @dep_amt = incur_los_reim_fund_initl_fund_amt,
			   @ilrf_invc_lsi_ind = incur_los_reim_fund_invc_lsi_ind
		from 
		dbo.PREM_ADJ_PGM_SETUP
		where
		prem_adj_pgm_id = @premium_adj_prog_id
		and custmr_id = @customer_id
		and adj_parmet_typ_id = 400 -- Adjustment Parameter Type for ILRF


		/*******************************************
		* Give credits for previously billed amounts
		********************************************/

		-- Handle initial deposit amount

		if (@cnt_prev_adjs = 0) -- No existing adjustments this is initial adjustment
		begin
			set @dep_amt = isnull(@dep_amt,0)
		end
		else
		begin -- This is not initial adjustment
			set @dep_amt = 0
		end

-- Older version version before using the function call below
--		select @prev_valid_adj_id =  max(pa.prem_adj_id) from dbo.PREM_ADJ pa
--		inner join dbo.PREM_ADJ_STS ps on (pa.prem_adj_id = ps.prem_adj_id) and (pa.custmr_id = ps.custmr_id)
--		where 
--		pa.valn_dt in
--		(
--			select max(valn_dt) 
--			from dbo.PREM_ADJ 
--			where valn_dt < (
--								select 
--								valn_dt 
--								from PREM_ADJ 
--								where prem_adj_id = @premium_adjustment_id
--							)
--							and custmr_id = @customer_id
--		)
--		and ps.adj_sts_typ_id in (349,352) -- Lookup IDs for applicable statuses: Final Invoice and Transmitted
--		and pa.custmr_id = @customer_id

		


		select @prev_valid_adj_perd_id = prem_adj_perd_id from dbo.PREM_ADJ_PERD
		where prem_adj_id = @prev_valid_adj_id
		and prem_adj_pgm_id = @premium_adj_prog_id

		if @debug = 1
		begin
		print '@prev_valid_adj_id: ' + convert(varchar(20), @prev_valid_adj_id ) 
		print '@prev_valid_adj_perd_id: ' + convert(varchar(20), @prev_valid_adj_perd_id ) 
		end

		if (@ilrf_invc_lsi_ind = 0) -- 'Invoice in LSI' indicator is not set
		begin
			-- Update prior values for the current adjustment from the (limited amount of) previous
			-- valid adjustment and its appropriate adjustment period
			update dbo.[PREM_ADJ_LOS_REIM_FUND_POST]
			set prior_yy_amt = isnull(b.lim_amt,0), --b.curr_amt,
				adj_prior_yy_amt = isnull(b.lim_amt,0) --b.curr_amt
			from dbo.[PREM_ADJ_LOS_REIM_FUND_POST] as a
			join 
			(
				select 
				recv_typ_id,
				lim_amt--curr_amt
				from dbo.[PREM_ADJ_LOS_REIM_FUND_POST]
				where prem_adj_id = @prev_valid_adj_id
				and prem_adj_perd_id = @prev_valid_adj_perd_id
				and custmr_id = @customer_id
			) as b
			on a.recv_typ_id = b.recv_typ_id
			where prem_adj_perd_id = @premium_adj_period_id
			and prem_adj_id = @premium_adjustment_id
			and custmr_id = @customer_id


			if (@cnt_prev_adjs = 0) -- No existing adjustments this is initial adjustment
			begin
				-- Initial deposit goes only to reserve
				update dbo.[PREM_ADJ_LOS_REIM_FUND_POST]
				set prior_yy_amt = @dep_amt,
				adj_prior_yy_amt = @dep_amt
				where [prem_adj_perd_id] = @premium_adj_period_id
				and [prem_adj_id] = @premium_adjustment_id
				and [custmr_id] = @customer_id
				and recv_typ_id = 71 -- lookup type: RECEIVABLE TYPE, lookup value: Reserve
			end
		end --end of: if (@ilrf_invc_lsi_ind = 0)
		else
		begin --else to: if (@ilrf_invc_lsi_ind = 0) / 'Invoice in LSI' indicator is set
				
			--Retrive prior amounts from LSI system.
			
			--Based on whether it is initial adjustment or not, the date range for ths.ValuationDate varies
			if (@cnt_prev_adjs = 0) -- No existing adjustments this is initial adjustment
			begin
				select 
				@prior_wc_tpd_amt = sum(case when (ths.LOB='WC') then ths.PaidLoss + ths.PaidALAE else 0 end),-- as WC_TPD ,
				@prior_auto_tpd_amt = sum(case when (ths.LOB='Auto') then ths.PaidLoss + ths.PaidALAE else 0 end),-- as AUTO_TPD ,
				@prior_gl_tpd_amt = sum(case when (ths.LOB='GL') then ths.PaidLoss + ths.PaidALAE else 0 end),-- as GL_TPD ,
				@prior_wc_lcf_amt = sum(case when (ths.LOB='WC') then ths.LCF else 0 end),-- as WC_LCF ,
				@prior_auto_lcf_amt = sum(case when (ths.LOB='Auto') then ths.LCF else 0 end),-- as as AUTO_LCF ,
				@prior_gl_lcf_amt = sum(case when (ths.LOB='GL') then ths.LCF else 0 end),-- as as GL_LCF ,
				@prior_resv_amt = sum(ths.ReservedLoss + ths.ReservedALAE), 
				@prior_lba_amt = sum(ths.LBA) 
				from dbo.PREM_ADJ pa  
				inner join dbo.PREM_ADJ_PERD pap on (pa.reg_custmr_id = pap.reg_custmr_id)
				inner join dbo.LSI_CUSTMR lc on (pa.reg_custmr_id = lc.custmr_id)
				left outer join View_LSI_TransmittalHistory_Standard_Entity ths with (NOLOCK) on (lc.lsi_acct_id = ths.fkAccountID)
				inner join dbo.PREM_ADJ_PGM pgm on (lc.custmr_id=pgm.custmr_id) and (pgm.prem_adj_pgm_id = pap.prem_adj_pgm_id)  
				left outer join View_LSI_Policy_Entity pol  with (NOLOCK) on (ths.fkPolicyID = pol.pkPolicyID)
				inner join dbo.COML_AGMT ca on (pa.reg_custmr_id = ca.custmr_id) and (pgm.prem_adj_pgm_id = ca.prem_adj_pgm_id) and (ca.pol_nbr_txt = substring(pol.PolicyNumber,1,7)) and ca.actv_ind = 1
				inner join dbo.PREM_ADJ_PGM_SETUP_POL apol on (apol.custmr_id = ca.custmr_id) and  (apol.prem_adj_pgm_id = ca.prem_adj_pgm_id) and (apol.coml_agmt_id = ca.coml_agmt_id)
				where 
				ths.ValuationDate >= ca.pol_eff_dt --pgm.strt_dt
				and ths.ValuationDate <= pa.valn_dt 
				and pol.InceptionDate >= pgm.los_sens_info_strt_dt -- policy start date 
				and pol.ExpirationDate <= pgm.los_sens_info_end_dt -- policy end date
				and ths.ProgramType <> 'SIR' 
				and pgm.actv_ind = 1
				and ca.actv_ind = 1
				and pa.reg_custmr_id = @customer_id 
				and pa.prem_adj_id = @premium_adjustment_id
				and pap.prem_adj_pgm_id = @premium_adj_prog_id
			end
			else
			begin
				select 
				@prior_wc_tpd_amt = sum(case when (ths.LOB='WC') then ths.PaidLoss + ths.PaidALAE else 0 end),-- as WC_TPD ,
				@prior_auto_tpd_amt = sum(case when (ths.LOB='Auto') then ths.PaidLoss + ths.PaidALAE else 0 end),-- as AUTO_TPD ,
				@prior_gl_tpd_amt = sum(case when (ths.LOB='GL') then ths.PaidLoss + ths.PaidALAE else 0 end),-- as GL_TPD ,
				@prior_wc_lcf_amt = sum(case when (ths.LOB='WC') then ths.LCF else 0 end),-- as WC_LCF ,
				@prior_auto_lcf_amt = sum(case when (ths.LOB='Auto') then ths.LCF else 0 end),-- as as AUTO_LCF ,
				@prior_gl_lcf_amt = sum(case when (ths.LOB='GL') then ths.LCF else 0 end),-- as as GL_LCF ,
				@prior_resv_amt = sum(ths.ReservedLoss + ths.ReservedALAE), 
				@prior_lba_amt = sum(ths.LBA) 
				from dbo.PREM_ADJ pa  
				inner join dbo.PREM_ADJ_PERD pap on (pa.reg_custmr_id = pap.reg_custmr_id)
				inner join dbo.LSI_CUSTMR lc on (pa.reg_custmr_id = lc.custmr_id)
				left outer join View_LSI_TransmittalHistory_Standard_Entity ths with (NOLOCK) on (lc.lsi_acct_id = ths.fkAccountID)
				inner join dbo.PREM_ADJ_PGM pgm on (lc.custmr_id=pgm.custmr_id) and (pgm.prem_adj_pgm_id = pap.prem_adj_pgm_id)  
				left outer join View_LSI_Policy_Entity pol  with (NOLOCK) on (ths.fkPolicyID = pol.pkPolicyID)
				inner join dbo.COML_AGMT ca on (pa.reg_custmr_id = ca.custmr_id) and (pgm.prem_adj_pgm_id = ca.prem_adj_pgm_id) and (ca.pol_nbr_txt = substring(pol.PolicyNumber,1,7)) and ca.actv_ind = 1
				inner join dbo.PREM_ADJ_PGM_SETUP_POL apol on (apol.custmr_id = ca.custmr_id) and  (apol.prem_adj_pgm_id = ca.prem_adj_pgm_id) and (apol.coml_agmt_id = ca.coml_agmt_id)
				where 
				ths.ValuationDate >= (select valn_dt from dbo.PREM_ADJ where prem_adj_id = @prev_valid_adj_id and reg_custmr_id = @customer_id) --pgm.strt_dt
				and ths.ValuationDate <= pa.valn_dt 
				and pol.InceptionDate >= pgm.los_sens_info_strt_dt -- policy start date 
				and pol.ExpirationDate <= pgm.los_sens_info_end_dt -- policy end date
				and ths.ProgramType <> 'SIR' 
				and pgm.actv_ind = 1
				and ca.actv_ind = 1
				and pa.reg_custmr_id = @customer_id 
				and pa.prem_adj_id = @premium_adjustment_id
				and pap.prem_adj_pgm_id = @premium_adj_prog_id
			end

				update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
					set prior_yy_amt = 0,
						adj_prior_yy_amt = 0
					where [prem_adj_perd_id] = @premium_adj_period_id
					and [prem_adj_id] = @premium_adjustment_id
					and [custmr_id] = @customer_id
					and recv_typ_id = 71 -- lookup type: RECEIVABLE TYPE, lookup value: Reserve

			-- Update prior values for the current adjustment from the (limited amount of) previous
			-- valid adjustment and its appropriate adjustment period
			update dbo.[PREM_ADJ_LOS_REIM_FUND_POST]
			set prior_yy_amt = isnull(b.lim_amt,0), --b.curr_amt,
				adj_prior_yy_amt = isnull(b.lim_amt,0) --b.curr_amt
			from dbo.[PREM_ADJ_LOS_REIM_FUND_POST] as a
			join 
			(
				select 
				recv_typ_id,
				lim_amt--curr_amt
				from dbo.[PREM_ADJ_LOS_REIM_FUND_POST]
				where prem_adj_id = @prev_valid_adj_id
				and prem_adj_perd_id = @prev_valid_adj_perd_id
				and custmr_id = @customer_id
			) as b
			on a.recv_typ_id = b.recv_typ_id
			where prem_adj_perd_id = @premium_adj_period_id
			and prem_adj_id = @premium_adjustment_id
			and custmr_id = @customer_id

			update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
			set prior_yy_amt = isnull(prior_yy_amt,0) + isnull(@prior_wc_tpd_amt,0),
				adj_prior_yy_amt = isnull(adj_prior_yy_amt,0) + isnull(@prior_wc_tpd_amt,0)
			where [prem_adj_perd_id] = @premium_adj_period_id
			and [prem_adj_id] = @premium_adjustment_id
			and [custmr_id] = @customer_id
			and recv_typ_id = 31 -- lookup type: RECEIVABLE TYPE, lookup value: WC TPD

			update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
			set prior_yy_amt = isnull(prior_yy_amt,0) + isnull(@prior_auto_tpd_amt,0),
				adj_prior_yy_amt = isnull(adj_prior_yy_amt,0) + isnull(@prior_auto_tpd_amt,0)
			where [prem_adj_perd_id] = @premium_adj_period_id
			and [prem_adj_id] = @premium_adjustment_id
			and [custmr_id] = @customer_id
			and recv_typ_id = 29 -- lookup type: RECEIVABLE TYPE, lookup value: Auto TPD

			update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
			set prior_yy_amt = isnull(prior_yy_amt,0) + isnull(@prior_gl_tpd_amt,0),
				adj_prior_yy_amt = isnull(adj_prior_yy_amt,0) + isnull(@prior_gl_tpd_amt,0)
			where [prem_adj_perd_id] = @premium_adj_period_id
			and [prem_adj_id] = @premium_adjustment_id
			and [custmr_id] = @customer_id
			and recv_typ_id = 30 -- lookup type: RECEIVABLE TYPE, lookup value: GL TPD

			update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
			set prior_yy_amt = isnull(prior_yy_amt,0) + isnull(@prior_wc_lcf_amt,0),
				adj_prior_yy_amt = isnull(adj_prior_yy_amt,0) + isnull(@prior_wc_lcf_amt,0)
			where [prem_adj_perd_id] = @premium_adj_period_id
			and [prem_adj_id] = @premium_adjustment_id
			and [custmr_id] = @customer_id
			and recv_typ_id = 34 -- lookup type: RECEIVABLE TYPE, lookup value: WC LCF

			update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
			set prior_yy_amt = isnull(prior_yy_amt,0) + isnull(@prior_auto_lcf_amt,0),
				adj_prior_yy_amt = isnull(adj_prior_yy_amt,0) + isnull(@prior_auto_lcf_amt,0)
			where [prem_adj_perd_id] = @premium_adj_period_id
			and [prem_adj_id] = @premium_adjustment_id
			and [custmr_id] = @customer_id
			and recv_typ_id = 32 -- lookup type: RECEIVABLE TYPE, lookup value: Auto LCF

			update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
			set prior_yy_amt = isnull(prior_yy_amt,0) + isnull(@prior_gl_lcf_amt,0),
				adj_prior_yy_amt = isnull(adj_prior_yy_amt,0) + isnull(@prior_gl_lcf_amt,0)
			where [prem_adj_perd_id] = @premium_adj_period_id
			and [prem_adj_id] = @premium_adjustment_id
			and [custmr_id] = @customer_id
			and recv_typ_id = 33 -- lookup type: RECEIVABLE TYPE, lookup value: GL LCF


			update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
			set prior_yy_amt = isnull(prior_yy_amt,0) + isnull(@prior_lba_amt,0),
				adj_prior_yy_amt = isnull(adj_prior_yy_amt,0) + isnull(@prior_lba_amt,0)
			where [prem_adj_perd_id] = @premium_adj_period_id
			and [prem_adj_id] = @premium_adjustment_id
			and [custmr_id] = @customer_id
			and recv_typ_id = 70 -- lookup type: RECEIVABLE TYPE, lookup value: LBA


			select @sum_non_reserve = sum(isnull(prior_yy_amt,0))
			from [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
			where [prem_adj_perd_id] = @premium_adj_period_id
			and [prem_adj_id] = @premium_adjustment_id
			and [custmr_id] = @customer_id
			and recv_typ_id in (31,29,30,34,32,33,70)
			
			--adding all receivable types
			select @prior_yy_amt_tot = sum(isnull(prior_yy_amt,0)),
			@adj_prior_yy_amt_tot=sum(isnull(adj_prior_yy_amt,0))
			from [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
			where [prem_adj_perd_id] = @premium_adj_period_id
			and [prem_adj_id] = @premium_adjustment_id
			and [custmr_id] = @customer_id
			and recv_typ_id in (31,29,30,34,32,33,70,71)
			

			update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
			set prior_yy_amt = @prior_yy_amt_tot /*+ isnull(@prior_resv_amt,0)*/ -  isnull(@sum_non_reserve,0), --Subtract sum of postings except reserve
				adj_prior_yy_amt = @adj_prior_yy_amt_tot /*+ isnull(@prior_resv_amt,0)*/ -  isnull(@sum_non_reserve,0) --Subtract sum of postings except reserve
			where [prem_adj_perd_id] = @premium_adj_period_id
			and [prem_adj_id] = @premium_adjustment_id
			and [custmr_id] = @customer_id
			and recv_typ_id = 71 -- lookup type: RECEIVABLE TYPE, lookup value: Reserve



			if (@cnt_prev_adjs = 0) -- No existing adjustments this is initial adjustment
			begin
				-- Initial deposit goes only to reserve;  deposit amount added for initial adjustment
				update dbo.[PREM_ADJ_LOS_REIM_FUND_POST]
				set prior_yy_amt = isnull(prior_yy_amt,0) + @dep_amt,
				adj_prior_yy_amt = isnull(adj_prior_yy_amt,0) + @dep_amt
				where [prem_adj_perd_id] = @premium_adj_period_id
				and [prem_adj_id] = @premium_adjustment_id
				and [custmr_id] = @customer_id
				and recv_typ_id = 71 -- lookup type: RECEIVABLE TYPE, lookup value: Reserve
			end

		end --end of: else to - if (@ilrf_invc_lsi_ind = 0) / 'Invoice in LSI' indicator is set

		-- Evaluate aggregate credit amount if applicable
		select 
		@ilrf_min_lim_ind = incur_los_reim_fund_unlim_minimium_lim_ind,
		@ilrf_min_lim_amt = incur_los_reim_fund_min_lim_amt
		from dbo.PREM_ADJ_PGM_SETUP
		where custmr_id = @customer_id
		and prem_adj_pgm_id = @premium_adj_prog_id
		and adj_parmet_typ_id = 400 -- Adjustment Parameter Type for ILRF
		
		-- Total of current amount
		select @tot_curr_amt = sum(isnull(curr_amt,0)) from PREM_ADJ_LOS_REIM_FUND_POST
		where [prem_adj_perd_id] = @premium_adj_period_id
		and [prem_adj_id] = @premium_adjustment_id
		and [custmr_id] = @customer_id

		-- Aggregate Credit for Reserve is evaluated only if total of current amt
		-- is less than min. limit amount and mim. limit unlimited checkbox is unchecked
		-- in the parameter setup of ILRF.
		if ((isnull(@tot_curr_amt,0) < isnull(@ilrf_min_lim_amt,0)) and (@ilrf_min_lim_ind = 0))
		begin
			if @ilrf_min_lim_amt > 0
				update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
				set aggr_amt = isnull(@ilrf_min_lim_amt,0) - isnull(@tot_curr_amt,0)
				where [prem_adj_perd_id] = @premium_adj_period_id
				and [prem_adj_id] = @premium_adjustment_id
				and [custmr_id] = @customer_id
				and [recv_typ_id] = 71 -- lookup type: RECEIVABLE TYPE, lookup value: Reserve
		end
				else
					begin
						if @ilrf_min_lim_amt > 0
							begin
								print 'else loop'
								update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
								set aggr_amt = NULL
								where [prem_adj_perd_id] = @premium_adj_period_id
								and [prem_adj_id] = @premium_adjustment_id
								and [custmr_id] = @customer_id
								and [recv_typ_id] = 71 -- lookup type: RECEIVABLE TYPE, lookup value: Reserve
							end
					end
		--Evaluate limited amount and posting amount
		update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST]
		set lim_amt = isnull(curr_amt,0) + isnull(aggr_amt,0), -- Limited amount
			post_amt = round(isnull(curr_amt,0) + isnull(aggr_amt,0) - isnull(adj_prior_yy_amt,0),0) -- Posting
		where [prem_adj_perd_id] = @premium_adj_period_id
		and [prem_adj_id] = @premium_adjustment_id
		and [custmr_id] = @customer_id

		end
		/**********************
		* ILRF output header
		**********************/

		/*******************************************
		* Give credits for previously billed amounts
		* after aggregation from the ILRF posting table.
		********************************************/

		update dbo.PREM_ADJ_PARMET_SETUP 
		set incur_los_reim_fund_amt = 
			(
				select sum(isnull(curr_amt,0)) 
				from PREM_ADJ_LOS_REIM_FUND_POST
				where [prem_adj_perd_id] = @premium_adj_period_id
				and [prem_adj_id] = @premium_adjustment_id
				and [custmr_id] = @customer_id
			),
			incur_los_reim_fund_lim_amt = 
			(
				select sum(isnull(lim_amt,0)) 
				from PREM_ADJ_LOS_REIM_FUND_POST
				where [prem_adj_perd_id] = @premium_adj_period_id
				and [prem_adj_id] = @premium_adjustment_id
				and [custmr_id] = @customer_id
			),
			incur_los_reim_fund_prevly_biled_amt = 
			(
				select sum(isnull(adj_prior_yy_amt,0)) 
				from PREM_ADJ_LOS_REIM_FUND_POST
				where [prem_adj_perd_id] = @premium_adj_period_id
				and [prem_adj_id] = @premium_adjustment_id
				and [custmr_id] = @customer_id
			),
			tot_amt = 
			(
				select sum(isnull(post_amt,0)) 
				from PREM_ADJ_LOS_REIM_FUND_POST
				where [prem_adj_perd_id] = @premium_adj_period_id
				and [prem_adj_id] = @premium_adjustment_id
				and [custmr_id] = @customer_id
			)			 
		where
		prem_adj_perd_id = @premium_adj_period_id
		and prem_adj_id = @premium_adjustment_id
		and custmr_id = @customer_id
		and adj_parmet_typ_id = 400 -- Adjustment Parameter Type for ILRF

		declare @rel_custmr_id int
		
		select @rel_custmr_id = custmr_id from prem_adj_pgm 
		where prem_adj_pgm_id = @premium_adj_prog_id

		--Update ARMIS LOS with Premium Adjustment ID
		update dbo.ARMIS_LOS_POL 
		set prem_adj_id = tm.prem_adj_id
		from dbo.ARMIS_LOS_POL as los
		join 
		(
			select
			h.custmr_id,
			h.prem_adj_pgm_id,
			h.prem_adj_id,
			d.coml_agmt_id,
			d.st_id
			from dbo.PREM_ADJ_PARMET_DTL d
			inner join dbo.PREM_ADJ_PARMET_SETUP h on (d.prem_adj_parmet_setup_id = h.prem_adj_parmet_setup_id) and (d.prem_adj_perd_id = h.prem_adj_perd_id) and (d.prem_adj_id = h.prem_adj_id) and (d.custmr_id = h.custmr_id)
			where d.prem_adj_perd_id = @premium_adj_period_id
			and d.prem_adj_id = @premium_adjustment_id
			and d.custmr_id = @rel_custmr_id
			and h.prem_adj_pgm_id = @premium_adj_prog_id
			and h.adj_parmet_typ_id = 400 -- Adjustment Parameter Type for ILRF
		) as tm
		on los.custmr_id = tm.custmr_id
		and los.prem_adj_pgm_id = tm.prem_adj_pgm_id
		and los.coml_agmt_id = tm.coml_agmt_id
		and los.st_id = tm.st_id
		where los.custmr_id = @customer_id
		and los.prem_adj_pgm_id = @premium_adj_prog_id
		and ((los.prem_adj_id is null) or (los.prem_adj_id = @premium_adjustment_id))
		and los.valn_dt	= @prem_adj_valn_dt -- Triage # 67
		and los.actv_ind = 1  

		--print '@trancount: ' + convert(varchar(30),@trancount)
		if @trancount = 0
			commit transaction 

end try
begin catch

	if @trancount = 0
	begin
		rollback transaction
	end
	else
	begin
		rollback transaction ModAISCalcILRF
	end


	
	declare @err_msg varchar(500),@err_ln varchar(10),
			@err_proc varchar(30),@err_no varchar(10)
	select  @err_msg = error_message(),
		    @err_no = error_number(),
		    @err_proc = error_procedure(),
			@err_ln = error_line()
	set @err_msg = '- error no.:' + isnull(@err_no, ' ') + '; procedure:' 
		+ isnull(@err_proc, ' ') + ';error line:' + isnull(@err_ln, ' ') + ';description:' + isnull(@err_msg, ' ' )
	set @err_msg_op = @err_msg

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

if object_id('ModAISCalcILRF') is not null
	print 'Created Procedure ModAISCalcILRF'
else
	print 'Failed Creating Procedure ModAISCalcILRF'
go

if object_id('ModAISCalcILRF') is not null
	grant exec on ModAISCalcILRF to public
go





