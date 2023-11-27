
if exists (select 1 from sysobjects 
		where name = 'ModAISCalcSurchargeAssesments' and type = 'P')
	drop procedure ModAISCalcSurchargeAssesments
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcSurchargeAssesments
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to Calculate the 21 sutrcharges based on the surcharg setup.
-----                   In this stored procedure there will be two kinds of calculations based on the polciy effective date
-----                   if the policy effective date is less than the 07/01/2009 , then only old surcharge claulctions will be performed
-----					i.e. KY-836 ,OR-816 and NY-0932,if the policy effective date is beyond the 07/01/2009, the n all the surcharges will
-----                   be calculated based on the surchage setup.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----               07/19/2010	Venkat Kolimi
-----				Created Procedure
-----			   08/25/2010 Venkat Kolimi
-----              (TFS 12674)Other than KY, NY and OR states are not displaying if policy effective date is equal to 07/01/2009
-----			   Now we are verifying with the >= instead of > 07/01/2009
-----				09/17/2010 Venkat Kolimi	
-----				Now the NY-931 is using the NY-0932 in the initial calculation.
-----				09/28/2010 Venkat Kolimi	
-----				WA Policies are removed from the calculations.
-----				11/05/2010 Venkat Kolimi
-----				Fixed the Issue with Policy Disabled in case of second subsequnet.
					
----- TODO: Need to remove the lkup hard codes

---------------------------------------------------------------------

CREATE procedure [dbo].[ModAISCalcSurchargeAssesments] 
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@premium_adj_prog_id int,
@create_user_id int,
@err_msg_op varchar(1000) output,
@debug bit = 0
as

begin
	set nocount on

/********************************************************************
                  Variable declaration
*********************************************************************/
declare	@com_agm_id int,
		@state_id int,
		@ln_of_bsn_id int,
		@surcharge_type_id int,
		@post_trns_typ_id int,
		@scode_txt varchar(20),
		@surcharge_cd_id int,
		@cnt_prev_adjs int,
		@valid_record int,
		@subj_paid_idnmty_amt decimal(15,2),
		@subj_paid_exps_amt decimal(15,2),
		@subj_resrv_idnmty_amt decimal(15,2),
		@subj_resrv_exps_amt decimal(15,2),
		@basic_amt decimal(15,2),
		@adj_incur_ernd_retro_prem_amt decimal(15,2),
		@std_subj_prem_amt decimal(15,2),
		@prev_ernd_retro_prem_amt decimal(15,2),
		@prev_biled_ernd_retro_prem_amt decimal(15,2),
		@retro_result decimal(15,2),
		@addn_surchrg_asses_cmpnt decimal(15,2),
		@tot_surchrg_asses_base decimal(15,2),
		@surchrg_rt decimal(15,8),
		@addn_rtn decimal(15,2),
		@surcharg_cd_lkup_typ_id int,
		@surchrg_cd_id_0932 int,
		@surchrg_typ_id_0932 int,
		@surchrg_cd_id_931 int,
		@surchrg_typ_id_931 int,
		@ny_prem_disc_amt decimal(15,2),
		@prev_valid_adj_id int,
		@prev_valid_adj_perd_id int,
		@incld_all_surchrg_ind bit,
		@use_std_subj_prem_ind bit,
		@prem_adj_valn_dt datetime,
		@pol_eff_dt datetime,
		@err_message varchar(500),
		@trancount int


set @trancount = @@trancount
--print @trancount
if @trancount >= 1
    save transaction ModAISCalcSurchargeAssesments
else
    begin transaction


begin try


		select 
		@prem_adj_valn_dt = valn_dt
		from dbo.PREM_ADJ
		where prem_adj_id = @premium_adjustment_id
		
		--Retrieving the Surcharge code lkup type
		select 
		@surcharg_cd_lkup_typ_id=lkup_typ_id
		from dbo.LKUP_TYP
		where lkup_typ_nm_txt='SURCHARGE ASSESSMENT CODE'
		
		select @surchrg_cd_id_0932=lkup_id from lkup
		inner join lkup_typ on lkup.lkup_typ_id=lkup_typ.lkup_typ_id
		where lkup.lkup_txt='0932' and lkup.attr_1_txt='NY'
		and lkup_typ_nm_txt='SURCHARGE ASSESSMENT CODE'
		
		select @surchrg_typ_id_0932=lkup_id from lkup
		inner join lkup_typ on lkup.lkup_typ_id=lkup_typ.lkup_typ_id
		where lkup.lkup_txt='NY STATE ASSMT' and lkup.attr_1_txt='0932'
		and lkup_typ_nm_txt='SURCHARGES AND ASSESSMENTS'
		
		select @surchrg_cd_id_931=lkup_id from lkup
		inner join lkup_typ on lkup.lkup_typ_id=lkup_typ.lkup_typ_id
		where lkup.lkup_txt='931' and lkup.attr_1_txt='NY'
		and lkup_typ_nm_txt='SURCHARGE ASSESSMENT CODE'
		
		select @surchrg_typ_id_931=lkup_id from lkup
		inner join lkup_typ on lkup.lkup_typ_id=lkup_typ.lkup_typ_id
		where lkup.lkup_txt='NY WC SECURITY FUND' and lkup.attr_1_txt='931'
		and lkup_typ_nm_txt='SURCHARGES AND ASSESSMENTS'

		
		select 
		@incld_all_surchrg_ind=incld_all_surchrg_ind,
		@use_std_subj_prem_ind=use_std_subj_prem_ind
		from dbo.PREM_ADJ_PGM
		where prem_adj_pgm_id=@premium_adj_prog_id

		update dbo.PREM_ADJ_PERD
		set use_std_subj_prem_ind=@use_std_subj_prem_ind
		where prem_adj_perd_id=@premium_adj_period_id
		

		/***********************************************************
				START: Cursor surcharge calculations
		************************************************************/
		declare surcharge_base_cur cursor LOCAL FAST_FORWARD READ_ONLY 
		for 
		select 
		prem_adj_retro_dtl.coml_agmt_id,
		st_id,
		lkupScode.lkup_id as "surcharge_cd_id",
		lkupScode.lkup_txt,
		lkupStype.lkup_id as "surcharge_type_id",
		ln_of_bsn_id,
		subj_paid_idnmty_amt,
		subj_paid_exps_amt,
		subj_resrv_idnmty_amt,
		subj_resrv_exps_amt,
		basic_amt,
		cesar_cd_tot_amt,
		std_subj_prem_amt,
		prev_biled_ernd_retro_prem_amt
		from
		prem_adj_retro_dtl
		inner join lkup lkupState on lkupState.lkup_id=prem_adj_retro_dtl.st_id
		inner join lkup lkupScode on Rtrim(lkupScode.attr_1_txt)=Rtrim(lkupState.attr_1_txt)
		inner join lkup lkupStype on Rtrim(lkupStype.attr_1_txt)=Rtrim(lkupScode.lkup_txt)
		inner join coml_agmt coml on coml.coml_agmt_id=prem_adj_retro_dtl.coml_agmt_id
		where
		prem_adj_id=@premium_adjustment_id
		and prem_adj_retro_dtl.prem_adj_pgm_id=@premium_adj_prog_id
		and lkupState.lkup_typ_id=1
		and lkupScode.lkup_typ_id=@surcharg_cd_lkup_typ_id
		and coml.adj_typ_id in(64,65,67,69,71,72)

		open surcharge_base_cur
		fetch surcharge_base_cur into @com_agm_id,@state_id,@surcharge_cd_id,@scode_txt,@surcharge_type_id,@ln_of_bsn_id,@subj_paid_idnmty_amt,@subj_paid_exps_amt,@subj_resrv_idnmty_amt,
		@subj_resrv_exps_amt,@basic_amt,@adj_incur_ernd_retro_prem_amt,@std_subj_prem_amt,@prev_biled_ernd_retro_prem_amt

		while @@Fetch_Status = 0
			begin
				begin --Start Begin2
					if @debug = 1
					begin
				    print'*******************TAX: START OF OUTER ITERATION*********' 
				    print'---------------Input Params-------------------' 

					print' @com_agm_id:- ' + convert(varchar(20), @com_agm_id)  
					print' @state_id:- ' + convert(varchar(20), @state_id)  
					print' @customer_id: ' + convert(varchar(20), @customer_id)
					print' @ln_of_bsn_id: ' + convert(varchar(20), @ln_of_bsn_id ) 
					print' @scode_txt: ' + convert(varchar(20), @scode_txt) 
					print' @surcharge_cd_id: ' + convert(varchar(20), @surcharge_cd_id) 
					print' @surcharge_type_id: ' + convert(varchar(20), @surcharge_type_id)   
					print' @subj_paid_idnmty_amt:- ' + convert(varchar(20), @subj_paid_idnmty_amt)  
					print' @subj_paid_exps_amt:- ' + convert(varchar(20), @subj_paid_exps_amt)  
					print' @subj_resrv_idnmty_amt: ' + convert(varchar(20), @subj_resrv_idnmty_amt)
					print' @subj_resrv_exps_amt: ' + convert(varchar(20), @subj_resrv_exps_amt ) 
					print' @basic_amt: ' + convert(varchar(20), @basic_amt) 
					print' @adj_incur_ernd_retro_prem_amt: ' + convert(varchar(20), @adj_incur_ernd_retro_prem_amt)  
					print' @std_subj_prem_amt: ' + convert(varchar(20), @std_subj_prem_amt)  
					print' @prev_biled_ernd_retro_prem_amt: ' + convert(varchar(20), @prev_biled_ernd_retro_prem_amt)   
					end
 
					select 
					@pol_eff_dt=pol_eff_dt,
					@ny_prem_disc_amt=isnull(ny_prem_disc_amt,0)
					from dbo.coml_agmt 
					where coml_agmt_id=@com_agm_id

					
					/***********************************************************
					*  Function to get the valid post_trns_typ_id based on
					the post_trsn_setup for the surcahrge code.
					************************************************************/
					exec @post_trns_typ_id = [dbo].[fn_GetSurchrgPostTrnsTypID]
					@SurchargeCode=@scode_txt
					
					set @surchrg_rt=-1
					set @valid_record=0

					/***********************************************************
					--fn_RetrieveSurcharge_rt is called here to Retrieve the 
					  Surcharge Rate based on the given parametrs
					************************************************************/
					exec @surchrg_rt = [dbo].[fn_RetrieveSurcharge_rt]
					@ln_of_bsn_id=@ln_of_bsn_id,
					@state_id=@state_id,
					@surcharge_type_id=@surcharge_type_id,
					@com_agm_id=@com_agm_id,
					@surcharge_code_id=@surcharge_cd_id,
					@prem_adj_id=@premium_adjustment_id


					/*******************************************************************
					* The function call to Determine first adjustment fro this surcharge
					on this policy under the program period.
					********************************************************************/
					exec @cnt_prev_adjs = [dbo].[fn_DetermineFirstAdjustment_Surcharge]
					@premium_adj_prog_id = @premium_adj_prog_id,
					@coml_agmt_id=@com_agm_id,
					@st_id=@state_id,
					@ln_of_bsn_id=@ln_of_bsn_id,
					@surchrg_cd_id=@surcharge_cd_id,
					@surchrg_typ_id=@surcharge_type_id
	
					/********************************************************************
					* Determine previous valid adjustment to populate
					* corresponding previous erp amt
					**********************************************************************/
					exec @prev_valid_adj_id = [dbo].[fn_DeterminePrevValidAdj_Premium]
 						@current_premium_adjustment_id = @premium_adjustment_id,
						@customer_id = @customer_id,
						@premium_adj_prog_id = @premium_adj_prog_id



					/*****************************************************************
					* Retrieving the prior surcharge rate 
					*******************************************************************/
					if(@prev_valid_adj_id is not null and @subj_paid_idnmty_amt is null)
					begin
					select @surchrg_rt=surchrg_rt 
					from 
					prem_adj_surchrg_dtl 
					where prem_adj_id=@prev_valid_adj_id
					and prem_adj_pgm_id=@premium_adj_prog_id
					and coml_agmt_id=@com_agm_id
					and st_id=@state_id
					and ln_of_bsn_id=@ln_of_bsn_id
					and surchrg_cd_id=@surcharge_cd_id
					and surchrg_typ_id=@surcharge_type_id
					end


					/***************************************************************************************
					* Populating corresponding previous erp amt based on the Previous valid Adjustment ID
					****************************************************************************************/
					select
					@prev_biled_ernd_retro_prem_amt=round(cesar_cd_tot_amt,0)
					from dbo.PREM_ADJ_RETRO_DTL 
					where prem_adj_id = @prev_valid_adj_id
					and custmr_id = @customer_id
					and prem_adj_pgm_id = @premium_adj_prog_id
					and coml_agmt_id=@com_agm_id
					and st_id=@state_id
					and ln_of_bsn_id=@ln_of_bsn_id
					

					if @debug = 1
					begin
					print' @surchrg_rt: ' + convert(varchar(20), @surchrg_rt)  
					print' @cnt_prev_adjs: ' + convert(varchar(20), @cnt_prev_adjs) 
					print '@prev_valid_adj_id: ' + convert(varchar(20), @prev_valid_adj_id ) 
					print '@prev_biled_ernd_retro_prem_amt: ' + convert(varchar(20), @prev_biled_ernd_retro_prem_amt ) 
					end
			
					/************************************************************************** 
					Logic to pull the Prior retro result and additional surchrg asses cmpnt amount
					from the previous addjustment when the policy is disabled in the subsequent
					**************************************************************************/
					if(@prev_valid_adj_id is not null and @subj_paid_idnmty_amt is null and @prev_biled_ernd_retro_prem_amt is not null)
					begin
					select @prev_biled_ernd_retro_prem_amt=retro_rslt 
					from 
					prem_adj_surchrg_dtl 
					where prem_adj_id=@prev_valid_adj_id
					and prem_adj_pgm_id=@premium_adj_prog_id
					and coml_agmt_id=@com_agm_id
					and st_id=@state_id
					and ln_of_bsn_id=@ln_of_bsn_id
					and surchrg_cd_id=@surcharge_cd_id
					and surchrg_typ_id=@surcharge_type_id

					end

					/***************************************************************************************
					* Validating to policy effective date and incld_all_surchrg_ind to decide 
					the surchrge calculations needed for this policy and surchrge
					****************************************************************************************/
					if(isnull(@incld_all_surchrg_ind,0)=1 OR (@pol_eff_dt>=cast('07/01/2009' as datetime)) AND @surchrg_rt<>-1) 
					begin

					set @valid_record=1
					if(@use_std_subj_prem_ind=1 OR @cnt_prev_adjs=0)
					begin
							set @retro_result=round(isnull(@adj_incur_ernd_retro_prem_amt,0),0)-round(isnull(@std_subj_prem_amt,0),0)
							set @prev_biled_ernd_retro_prem_amt=NULL
							  
					end
					else
					begin
					set @retro_result=round(isnull(@adj_incur_ernd_retro_prem_amt,0),0)-round(isnull(@prev_biled_ernd_retro_prem_amt,0),0)
					set @std_subj_prem_amt=NULL
					end
						
					set @addn_surchrg_asses_cmpnt=0						
					
					if (@state_id=35 and @scode_txt='0932' and @cnt_prev_adjs=0)
					begin
						set @addn_surchrg_asses_cmpnt=@ny_prem_disc_amt		
					end
					else if (@state_id=35 and @scode_txt='931')
					begin
					--To Do: Need to validate this logic
					/***************************************************************************************
					* Populating corresponding @addn_surchrg_asses_cmpnt based on the NY-932 calculations.
					****************************************************************************************/
					select @addn_surchrg_asses_cmpnt=isnull(calc.addn_rtn,0)+isnull(review.other_surchrg_amt,0)
					from prem_adj_surchrg_dtl calc
					inner join prem_adj_surchrg_dtl_amt review on calc.ln_of_bsn_id = review.ln_of_bsn_id
					and calc.st_id = review.st_id
					and calc.coml_agmt_id = review.coml_agmt_id
					and calc.surchrg_typ_id = review.surchrg_typ_id
					and calc.surchrg_cd_id = review.surchrg_cd_id
					and calc.prem_adj_id=review.prem_adj_id
					and calc.prem_adj_pgm_id=review.prem_adj_pgm_id
					and calc.prem_adj_perd_id=review.prem_adj_perd_id
					where calc.prem_adj_id = @premium_adjustment_id
					and calc.prem_adj_perd_id = @premium_adj_period_id
					and calc.custmr_id = @customer_id
					and calc.coml_agmt_id=@com_agm_id
					and calc.st_id=@state_id
					and calc.ln_of_bsn_id=@ln_of_bsn_id
					and calc.surchrg_cd_id=@surchrg_cd_id_0932
					and calc.surchrg_typ_id=@surchrg_typ_id_0932

					end
					  
					
					/************************************************************************** 
					Logic to eliminate the record when the the policy is disabled in the 
					second subsequent
					**************************************************************************/
					if(@prev_valid_adj_id is not null and @subj_paid_idnmty_amt is null and @prev_biled_ernd_retro_prem_amt is null)
					begin
					 set @valid_record=0
					end
					/************************************************************************** 
					Logic to pull the Prior retro result and additional surchrg asses cmpnt amount
					from the previous addjustment when the policy is disabled in the subsequent
					**************************************************************************/
					if(@prev_valid_adj_id is not null and @subj_paid_idnmty_amt is null and @prev_biled_ernd_retro_prem_amt is not null)
					begin
					select @addn_surchrg_asses_cmpnt=addn_surchrg_asses_cmpnt 
					from 
					prem_adj_surchrg_dtl 
					where prem_adj_id=@prev_valid_adj_id
					and prem_adj_pgm_id=@premium_adj_prog_id
					and coml_agmt_id=@com_agm_id
					and st_id=@state_id
					and ln_of_bsn_id=@ln_of_bsn_id
					and surchrg_cd_id=@surcharge_cd_id
					and surchrg_typ_id=@surcharge_type_id
					end

					set @tot_surchrg_asses_base=isnull(@retro_result,0)+isnull(@addn_surchrg_asses_cmpnt,0)			
										 
					set @addn_rtn=isnull(@tot_surchrg_asses_base,0)*@surchrg_rt
					
					set @addn_rtn=round(@addn_rtn,0)

					if @debug = 1
					begin
				    print'---------------Calculated values for all the surcharges-------------------' 
					print' @retro_result: ' + convert(varchar(20), @retro_result) 
					print' @addn_surchrg_asses_cmpnt: ' + convert(varchar(20), @addn_surchrg_asses_cmpnt) 
					print' @tot_surchrg_asses_base: ' + convert(varchar(20), @tot_surchrg_asses_base) 
					print' @addn_rtn: ' + convert(varchar(20), @addn_rtn)
					end

					end
					else if(@surchrg_rt<>-1)
					begin 
					
					if(@scode_txt='0932' OR @scode_txt='816' OR @scode_txt='836')
					begin
					set @valid_record=1
					if(@use_std_subj_prem_ind=1 OR @cnt_prev_adjs=0)
					begin
							set @retro_result=round(isnull(@adj_incur_ernd_retro_prem_amt,0),0)-round(isnull(@std_subj_prem_amt,0),0)
							set @prev_biled_ernd_retro_prem_amt=NULL
					end
					else
					begin
					set @retro_result=round(isnull(@adj_incur_ernd_retro_prem_amt,0),0)-round(isnull(@prev_biled_ernd_retro_prem_amt,0),0)
					set @std_subj_prem_amt=NULL
					end
						
					set @addn_surchrg_asses_cmpnt=0						
					
					if (@state_id=35 and @scode_txt='0932' and @cnt_prev_adjs=0)
					begin
					set @addn_surchrg_asses_cmpnt=@ny_prem_disc_amt	
					end
					--Clauclations need to be performed for only these 6 accounts when the policy effective date is less than 07/01/2009
					if ((@scode_txt='0932') and (@customer_id not in(244,465,480,946,972,3026,3091)))
					begin
					set @valid_record=0
					end
					
					/************************************************************************** 
					Logic to eliminate the record when the the policy is disabled in the 
					second subsequent
					**************************************************************************/
					if(@prev_valid_adj_id is not null and @subj_paid_idnmty_amt is null and @prev_biled_ernd_retro_prem_amt is null)
					begin
					 set @valid_record=0
					end
					/************************************************************************** 
					Logic to pull the Prior retro result and additional surchrg asses cmpnt amount
					from the previous addjustment when the policy is disabled in the subsequent
					**************************************************************************/
					if(@prev_valid_adj_id is not null and @subj_paid_idnmty_amt is null and @prev_biled_ernd_retro_prem_amt is not null)
					begin
					
					select @addn_surchrg_asses_cmpnt=addn_surchrg_asses_cmpnt 
					from 
					prem_adj_surchrg_dtl 
					where prem_adj_id=@prev_valid_adj_id
					and prem_adj_pgm_id=@premium_adj_prog_id
					and coml_agmt_id=@com_agm_id
					and st_id=@state_id
					and ln_of_bsn_id=@ln_of_bsn_id
					and surchrg_cd_id=@surcharge_cd_id
					and surchrg_typ_id=@surcharge_type_id

					end
					set @tot_surchrg_asses_base=isnull(@retro_result,0)+isnull(@addn_surchrg_asses_cmpnt,0)
										
					set @addn_rtn=isnull(@tot_surchrg_asses_base,0)*@surchrg_rt
					
					set @addn_rtn=round(@addn_rtn,0)			

					end

					if @debug = 1
					begin
				    print'---------------Calculated values for the KY,OR and NY 0932 surcharges-------------------' 
					print' @retro_result: ' + convert(varchar(20), @retro_result) 
					print' @addn_surchrg_asses_cmpnt: ' + convert(varchar(20), @addn_surchrg_asses_cmpnt) 
					print' @tot_surchrg_asses_base: ' + convert(varchar(20), @tot_surchrg_asses_base) 
					print' @addn_rtn: ' + convert(varchar(20), @addn_rtn)
					end

					end
	

					/***************************************************************************************
					* Inserting the valid results into the Data base object
					****************************************************************************************/
					if(@surchrg_rt<>-1 and @valid_record=1)
					begin
					insert into PREM_ADJ_SURCHRG_DTL
					(
					prem_adj_perd_id,
					prem_adj_id,
					custmr_id,
					coml_agmt_id,
					prem_adj_pgm_id,
					st_id,
					ln_of_bsn_id,
					surchrg_cd_id,
					surchrg_typ_id,
					post_trns_typ_id,
					subj_paid_idnmty_amt,
					subj_paid_exps_amt,
					subj_resrv_idnmty_amt,
					subj_resrv_exps_amt,
					basic_amt,
					std_subj_prem_amt,
					ernd_retro_prem_amt,
					prev_biled_ernd_retro_prem_amt,
					retro_rslt,
					addn_surchrg_asses_cmpnt,
					tot_surchrg_asses_base,
					surchrg_rt,
					addn_rtn,
					updt_user_id,
					updt_dt,
					crte_user_id,
					crte_dt
					)
					VALUES
					(
					@premium_adj_period_id,
					@premium_adjustment_id,
					@customer_id,
					@com_agm_id,
					@premium_adj_prog_id,
					@state_id,
					@ln_of_bsn_id,
					@surcharge_cd_id,
					@surcharge_type_id,
					@post_trns_typ_id,
					@subj_paid_idnmty_amt,
					@subj_paid_exps_amt,
					@subj_resrv_idnmty_amt,
					@subj_resrv_exps_amt,
					@basic_amt,
					@std_subj_prem_amt,
					@adj_incur_ernd_retro_prem_amt,
					@prev_biled_ernd_retro_prem_amt,
					@retro_result,
					@addn_surchrg_asses_cmpnt,
					@tot_surchrg_asses_base,
					@surchrg_rt,
					@addn_rtn,
					NULL,
					NULL,
					@create_user_id,
					getdate()
					)
					end
					
				end  --End Begin2
					fetch surcharge_base_cur into @com_agm_id,@state_id,@surcharge_cd_id,@scode_txt,@surcharge_type_id,@ln_of_bsn_id,@subj_paid_idnmty_amt,@subj_paid_exps_amt,@subj_resrv_idnmty_amt,
					@subj_resrv_exps_amt,@basic_amt,@adj_incur_ernd_retro_prem_amt,@std_subj_prem_amt,@prev_biled_ernd_retro_prem_amt
					
			end --end of cursor surcharge_base_cur / while loop
		close surcharge_base_cur
		deallocate surcharge_base_cur

		/***********************************************************
				END: Cursor surcharge calculations
		************************************************************/


		/***************************************************************************************
		* Inserting the valid results into the Data base object so that users can adjust the amount
		in the adjustmnet review screen.
		****************************************************************************************/
--		if not exists(select 1 from [dbo].[PREM_ADJ_SURCHRG_DTL_AMT] where [prem_adj_perd_id] = @premium_adj_period_id and [prem_adj_id] = @premium_adjustment_id)
--		begin--not exists
		insert into dbo.PREM_ADJ_SURCHRG_DTL_AMT
		(
		prem_adj_perd_id,
		prem_adj_id,
		custmr_id,
		coml_agmt_id,
		prem_adj_pgm_id,
		st_id,
		ln_of_bsn_id,
		surchrg_cd_id,
		surchrg_typ_id,
		crte_user_id,
		crte_dt
		)
		select
		@premium_adj_period_id,
		@premium_adjustment_id,
		@customer_id,
		coml_agmt_id,
		@premium_adj_prog_id,
		st_id,
		ln_of_bsn_id,
		surchrg_cd_id,
		surchrg_typ_id,
		@create_user_id,
		getdate()
			from 
			(
			select
				custmr_id,
				prem_adj_id,
				prem_adj_perd_id,
				prem_adj_pgm_id,
				coml_agmt_id,
				surchrg_typ_id,
				surchrg_cd_id,
				ln_of_bsn_id,
				st_id
				from dbo.PREM_ADJ_SURCHRG_DTL
				where  prem_adj_id = @premium_adjustment_id
				and prem_adj_perd_id = @premium_adj_period_id
				and custmr_id = @customer_id
			) as calc
		where not exists
		(
			select * 
			from 
			(
				select
			    custmr_id,
				prem_adj_id,
				prem_adj_perd_id,
				prem_adj_pgm_id,
				coml_agmt_id,
				surchrg_typ_id,
				surchrg_cd_id,
				ln_of_bsn_id,
				st_id
				from dbo.PREM_ADJ_SURCHRG_DTL_AMT 
				where  prem_adj_id = @premium_adjustment_id
				and prem_adj_perd_id = @premium_adj_period_id
				and custmr_id = @customer_id
				
				
			) as review
			where calc.ln_of_bsn_id = review.ln_of_bsn_id
			and calc.st_id = review.st_id
			and calc.coml_agmt_id = review.coml_agmt_id
			and calc.surchrg_typ_id = review.surchrg_typ_id
			and calc.surchrg_cd_id = review.surchrg_cd_id
			
			
		)

--		end
		--As per the TFS Bug 12686,we have added the following code
		update prem_adj_surchrg_dtl 
		set addn_surchrg_asses_cmpnt=round(isnull(tm.tot_adtn_rtn,0),0),
		tot_surchrg_asses_base=round(isnull(retro_rslt,0),0)+round(isnull(tm.tot_adtn_rtn,0),0),						
		addn_rtn=round(((round(isnull(retro_rslt,0),0)+round(isnull(tm.tot_adtn_rtn,0),0))*surchrg_rt),0)
		from prem_adj_surchrg_dtl as p
		join
		(
		select 
		h.prem_adj_pgm_id,
		h.prem_adj_perd_id,
		h.prem_adj_id,
		h.coml_agmt_id,
		h.ln_of_bsn_id,
		h.surchrg_typ_id,
		h.surchrg_cd_id,
		h.st_id,
		isnull(h.addn_rtn,0)+isnull(review.other_surchrg_amt,0) as tot_adtn_rtn
		from prem_adj_surchrg_dtl h
		inner join prem_adj_surchrg_dtl_amt review on h.ln_of_bsn_id = review.ln_of_bsn_id
		and h.st_id = review.st_id
		and h.ln_of_bsn_id=review.ln_of_bsn_id
		and h.coml_agmt_id = review.coml_agmt_id
		and h.surchrg_typ_id = review.surchrg_typ_id
		and h.surchrg_cd_id = review.surchrg_cd_id
		and h.prem_adj_id=review.prem_adj_id
		and h.prem_adj_pgm_id=review.prem_adj_pgm_id
		and h.prem_adj_perd_id=review.prem_adj_perd_id
		where h.prem_adj_id = @premium_adjustment_id
		and h.prem_adj_perd_id = @premium_adj_period_id
		and h.custmr_id = @customer_id
		and h.st_id=35
		and h.surchrg_cd_id=@surchrg_cd_id_0932
		and h.surchrg_typ_id=@surchrg_typ_id_0932

		)  as tm
		on p.coml_agmt_id=tm.coml_agmt_id and p.prem_adj_pgm_id = tm.prem_adj_pgm_id 
		and p.prem_adj_perd_id=tm.prem_adj_perd_id 
		and p.st_id=tm.st_id
		and p.ln_of_bsn_id=tm.ln_of_bsn_id
		and p.prem_adj_id=tm.prem_adj_id
		where p.prem_adj_perd_id = @premium_adj_period_id
		and p.prem_adj_id = @premium_adjustment_id
		and p.prem_adj_pgm_id=@premium_adj_prog_id
		and p.custmr_id = @customer_id
		and p.st_id=35
		and p.surchrg_typ_id=@surchrg_typ_id_931
		and p.surchrg_cd_id=@surchrg_cd_id_931

	
		/***************************************************************************************
		  Updating the tot_addn_rtn so that postings will be generated based on this amount.
		****************************************************************************************/
		update prem_adj_surchrg_dtl
		set tot_addn_rtn=isnull(calc.addn_rtn,0)+isnull(review.other_surchrg_amt,0)
		from prem_adj_surchrg_dtl calc
		inner join prem_adj_surchrg_dtl_amt review on calc.ln_of_bsn_id = review.ln_of_bsn_id
		and calc.st_id = review.st_id
		and calc.coml_agmt_id = review.coml_agmt_id
		and calc.surchrg_typ_id = review.surchrg_typ_id
		and calc.surchrg_cd_id = review.surchrg_cd_id
		and calc.prem_adj_id=review.prem_adj_id
        and calc.prem_adj_pgm_id=review.prem_adj_pgm_id
		and calc.prem_adj_perd_id=review.prem_adj_perd_id
		where calc.prem_adj_id = @premium_adjustment_id
		and calc.prem_adj_perd_id = @premium_adj_period_id
		and calc.custmr_id = @customer_id

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
		rollback transaction ModAISCalcSurchargeAssesments
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

if object_id('ModAISCalcSurchargeAssesments') is not null
	print 'Created Procedure ModAISCalcSurchargeAssesments'
else
	print 'Failed Creating Procedure ModAISCalcSurchargeAssesments'
go

if object_id('ModAISCalcSurchargeAssesments') is not null
	grant exec on ModAISCalcSurchargeAssesments to public
go

