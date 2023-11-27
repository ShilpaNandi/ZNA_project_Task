if exists (select 1 from sysobjects where name = 'ModAISCalcDeductibleTax' and type = 'P')
           drop procedure ModAISCalcDeductibleTax
go
SET ANSI_NULLS OFF
GO


SET QUOTED_IDENTIFIER OFF
GO

--------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcDeductibleTax
-----
-----	Version:	SQL Server 2005

-----	Created:		CSC (Venkata Kolimi)

-----
-----	Description:	This stored procedure is used to Calculate the Tax amount based on the setup.
-----                   This procedure will be called in the calculation Driver to perform the Deductible Tax calculations.
-----                   This will take the input parameters such as adjustment id,program period id,custmer id, period id and userid
-----                   it will pick the data from the setup tables and popup the data to the output tables.
-----
-----	On Exit:	
-----			
-----
-----   Created Date : 11/27/09 (AS part of Texas tax Project)

-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

-----               11/27/09	Venkat Kolimi
-----				Created Procedure

-----               03/22/2010  Venkat Kolimi
-----               Removed the equal condition in Tax End data validation
-----               This is to fix the the issue "Incorrect values are displayed when the tax end date is equal to the valuation date  in the subsequent adjustment."(Bug 11467)

-----               03/31/2010 Venkat Kolimi
-----               Fixed the issue with Null values while Calculating the Total tax
-----               This is to fix the issue "When there are no GL losses for TX state a row is added in the Tax Posting grid for TX/GL with zero amount."(11485 Bug)

-----               04/01/2010 Venkat Kolimi
-----               corrected the subsequent posting logic to pull the prior values.
-----               This is to fix the issue "Posting amounts are incorrectly displayed for a tax exempted state in the  Subsequent adjustment."(11580 Bug)

-----               04/13/2010 Venkat Kolimi
-----               As per this issue we are not pulling the prior tax amounts when there is ALAE amount in the current adjsutment.
-----               This is to fix the issue "Tax previously required is wrongly pulled when loss is moved from taxable state to nontaxable state."(11584 Bug)


-----               06/30/2010 Venkat Kolimi
-----               As per this issue we are not pulling the ALAE amounts when there are no losses for that program period.
-----               This is to fix the issue "TT ALAE Amount Not Picked Up if Calc was not Completed."(12013 Bug)

-----               06/30/2010 Venkat Kolimi
-----               As per this issue we are pulling the amounts on the summary invoice when there are no calculations performed.
-----               This is to fix the issue "TT TC 2 - Tax Exempt adjustment has no Tax exhibit but Tax line is on Summary Invoice."(12018 Bug)


---------------------------------------------------------------------

CREATE procedure [dbo].[ModAISCalcDeductibleTax] 
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

declare	@com_agm_id int,
		@state_id int,
		@ln_of_bsn_id int,
		@tax_typ_id int,
		@tax_amount decimal(15,2),
		@tax_rate decimal(15,8),
		@component_id int,
		@lba_component_id int,
		@lcf_component_id int,
		@ibnr_component_id int,
		@ldf_component_id int,
		@component_txt varchar(20),
		@prem_adj_tax_setup_id int,
		@state_sales_service_fee_tax decimal(15,2),
		@lcf_amt decimal(15,2),
		@tax_component_amt decimal(15,2),
		@sum_tax_amount decimal(15,2),
		@prem_adj_valn_dt datetime,
		@tax_end_dt datetime,
		@incur_but_not_rptd_los_dev_fctr_id int,
		@prev_valid_adj_id int,
		@prev_valid_adj_perd_id int,
		@err_message varchar(500),
		@trancount int


set @trancount = @@trancount
--print @trancount
if @trancount >= 1
    save transaction ModAISCalcDeductibleTax
else
    begin transaction


begin try


		select 
		@prem_adj_valn_dt = valn_dt
		from dbo.PREM_ADJ
		where prem_adj_id = @premium_adjustment_id


       
		select @incur_but_not_rptd_los_dev_fctr_id = incur_but_not_rptd_los_dev_fctr_id 
		from prem_adj_pgm_setup 
		where adj_parmet_typ_id=400
		and prem_adj_pgm_id=@premium_adj_prog_id 

		 --Start of main loop which determines the calculations needed based on the tax setup we had for the 
		 --program period receieved from the calling environment
		declare tax_base_cur cursor LOCAL FAST_FORWARD READ_ONLY 
		for 
		(select distinct
		st_id,
		s.ln_of_bsn_id,
		coml_agmt_id,
		tax_typ_id
		from prem_adj_parmet_dtl s
		inner join prem_adj_parmet_setup on  prem_adj_parmet_setup.prem_adj_parmet_setup_id=s.prem_adj_parmet_setup_id
		inner join INCUR_LOS_REIM_FUND_TAX_SETUP ilrs on ilrs.prem_adj_pgm_id=prem_adj_parmet_setup.prem_adj_pgm_id 
		and (select attr_1_txt from lkup where lkup_id=ilrs.tax_typ_id)=(select attr_1_txt from lkup where 
		lkup_id=s.st_id) 
		and ilrs.ln_of_bsn_id=s.ln_of_bsn_id
		where 
		prem_adj_parmet_setup.prem_adj_id=@premium_adjustment_id 
		and prem_adj_parmet_setup.adj_parmet_typ_id=400 
		and prem_adj_parmet_setup.prem_adj_pgm_id=@premium_adj_prog_id
		and prem_adj_parmet_setup.prem_adj_perd_id=@premium_adj_period_id
		and ilrs.actv_ind=1
		and st_id not in(select st_id from TAX_EXMP_SETUP where prem_adj_pgm_id=@premium_adj_prog_id and actv_ind=1 )
		union
		select distinct
		dts.st_id,
		s.ln_of_bsn_id,
		coml_agmt_id,
		tax_typ_id
		from prem_adj_parmet_dtl s
		inner join prem_adj_parmet_setup on  prem_adj_parmet_setup.prem_adj_parmet_setup_id=s.prem_adj_parmet_setup_id
		inner join DEDTBL_TAX_SETUP dts on dts.st_id=s.st_id and dts.ln_of_bsn_id=s.ln_of_bsn_id
		where 
		prem_adj_parmet_setup.prem_adj_id=@premium_adjustment_id 
		and prem_adj_parmet_setup.adj_parmet_typ_id=400 
		and prem_adj_parmet_setup.prem_adj_pgm_id=@premium_adj_prog_id
		and prem_adj_parmet_setup.prem_adj_perd_id=@premium_adj_period_id
		and dts.actv_ind=1
		and dts.st_id not in(select st_id from TAX_EXMP_SETUP where prem_adj_pgm_id=@premium_adj_prog_id and actv_ind=1 ))

		open tax_base_cur
		fetch tax_base_cur into @state_id, @ln_of_bsn_id, @com_agm_id, @tax_typ_id

		while @@Fetch_Status = 0
			begin
				begin
					if @debug = 1
					begin
				    print'*******************TAX: START OF OUTER ITERATION*********' 
				    print'---------------Input Params-------------------' 

					print' @com_agm_id:- ' + convert(varchar(20), @com_agm_id)  
					print' @state_id:- ' + convert(varchar(20), @state_id)  
					print' @customer_id: ' + convert(varchar(20), @customer_id)
					print' @ln_of_bsn_id: ' + convert(varchar(20), @ln_of_bsn_id ) 
					print' @tax_typ_id: ' + convert(varchar(20), isnull(@tax_typ_id,0))  
					end
 
					--Need to modify for all LOB's
					--if(@ln_of_bsn_id=428)--WC LOB only we need to calculate tax
					--begin
					--fn_Retrieve_State_Sales_Service_Fee_Tax_Amt is called here to retrieve the state sales and service fee tax amount
					--for the given custmer,program period,tax type and Line of business
					exec @state_sales_service_fee_tax = [dbo].[fn_Retrieve_State_Sales_Service_Fee_Tax_Amt]
					@p_cust_id=@customer_id,
					@p_prem_adj_pgm_id=@premium_adj_prog_id,
					@p_tax_typ_id=@tax_typ_id,
					@p_ln_of_bsn_id=@ln_of_bsn_id


					--Start of inner loop to perform the state level calculations
					declare tax_component_base_cur cursor LOCAL FAST_FORWARD READ_ONLY 
					for 
					select distinct dedtbl_tax_cmpnt_id from DEDTBL_TAX_SETUP
					where ln_of_bsn_id=@ln_of_bsn_id
					and tax_typ_id=@tax_typ_id
					and st_id=@state_id 
					and pol_eff_dt<=(select pol_eff_dt from coml_agmt where coml_agmt_id=@com_agm_id)
					and actv_ind=1
					and dedtbl_tax_cmpnt_id<>(select lkup_id from dbo.LKUP where lkup_typ_id=54 and lkup_txt='CHF')
					


					open tax_component_base_cur
					fetch tax_component_base_cur into @component_id

					while @@Fetch_Status = 0
						begin
							begin
								if @debug = 1
								begin
								print'*******************TAX: START OF TAX RATE CALCULATIONS*********' 
								print' @component_id:- ' + convert(varchar(20), @component_id)  
								end

								set @tax_component_amt=NULL
								set @tax_rate=NULL
								set @tax_end_dt=NULL

								--Populating the calculated/required values in the output master table at state level.
								--this will retrun the primary key cretaed on that row which will be used as a refernce key in the child table while inserting records 
								if not exists(select * from dbo.PREM_ADJ_TAX_SETUP where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and st_id = @state_id and ln_of_bsn_id=@ln_of_bsn_id)								
								begin
								exec [dbo].[AddPREM_ADJ_TAX_SETUP] 
								@premium_adj_period_id ,
								@premium_adjustment_id ,
								@customer_id ,
								@premium_adj_prog_id ,
								@ln_of_bsn_id,
								@state_id ,
								@tax_typ_id,
								@state_sales_service_fee_tax,
								@create_user_id ,
								@prem_adj_tax_setup_id_op = @prem_adj_tax_setup_id output
								end--end of not exists
								else
								begin
								select @prem_adj_tax_setup_id=prem_adj_tax_setup_id from dbo.PREM_ADJ_TAX_SETUP where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and st_id = @state_id and ln_of_bsn_id=@ln_of_bsn_id
								end
					
								--fn_RetrieveTax_rt is called here to Retrieve the Tax Rate based on the goven parametrs
								exec @tax_rate = [dbo].[fn_RetrieveTax_rt]
 								@ln_of_bsn_id=@ln_of_bsn_id,
								@state_id=@state_id,
								@tax_typ_id=@tax_typ_id,
								@com_agm_id=@com_agm_id,
								@component_id=@component_id

								--fn_RetrieveTax_End_Date is called here to retrieve the tax Enda Date associated for the given combination to determine 
								--the calculations needed
								exec @tax_end_dt = [dbo].[fn_RetrieveTax_End_Date]
 								@ln_of_bsn_id=@ln_of_bsn_id,
								@state_id=@state_id,
								@tax_typ_id=@tax_typ_id,
								@com_agm_id=@com_agm_id,
								@component_id=@component_id
					
					--If the tax end date condition is not recahed we will perform the calculations
					select @lba_component_id=lkup_id from lkup where lkup_typ_id=54 and lkup_txt='LBA'
					select @lcf_component_id=lkup_id from lkup where lkup_typ_id=54 and lkup_txt='LCF'
					select @ibnr_component_id=lkup_id from lkup where lkup_typ_id=54 and lkup_txt='IBNR'
					select @ldf_component_id=lkup_id from lkup where lkup_typ_id=54 and lkup_txt='LDF'
					if(@prem_adj_valn_dt < case when @tax_end_dt is null then cast('12/31/9999' as datetime)  else @tax_end_dt end )--if Tax End Date Validation
					begin
					--select @component_txt=lkup_txt from lkup where lkup_id=@component_id
					if(@component_id=@lba_component_id)--LBA
					begin
					select @tax_component_amt=los_base_asses_amt from PREM_ADJ_PARMET_DTL
					where
					prem_adj_perd_id=@premium_adj_period_id
					and prem_adj_id=@premium_adjustment_id
					and st_id=@state_id
					and ln_of_bsn_id=@ln_of_bsn_id
					and prem_adj_pgm_id=@premium_adj_prog_id
					and coml_agmt_id=@com_agm_id
					end--End of LBA


					if(@component_id=@lcf_component_id)--LCF
					begin
					select @tax_component_amt=los_conv_fctr_amt from PREM_ADJ_PARMET_DTL
					where
					prem_adj_perd_id=@premium_adj_period_id
					and prem_adj_id=@premium_adjustment_id
					and st_id=@state_id
					and ln_of_bsn_id=@ln_of_bsn_id
					and prem_adj_pgm_id=@premium_adj_prog_id
					and coml_agmt_id=@com_agm_id
					end--end of LCF

					--if There is IBNR setup then only IBNR component will be applied with Tax,
					--this condition is verified by @incur_but_not_rptd_los_dev_fctr_id condition
					if(@component_id=@ibnr_component_id and @incur_but_not_rptd_los_dev_fctr_id=419)--IBNR
					begin
					select @tax_component_amt=los_dev_fctr_amt from PREM_ADJ_PARMET_DTL
					where
					prem_adj_perd_id=@premium_adj_period_id
					and prem_adj_id=@premium_adjustment_id
					and st_id=@state_id
					and ln_of_bsn_id=@ln_of_bsn_id
					and prem_adj_pgm_id=@premium_adj_prog_id
					and coml_agmt_id=@com_agm_id
					end--end of IBNR
			
					
					--if There is IBNR setup then only IBNR component will be applied with Tax,
					--this condition is verified by @incur_but_not_rptd_los_dev_fctr_id condition
					if(@component_id=@ldf_component_id and @incur_but_not_rptd_los_dev_fctr_id=420)--LDF
					begin
					select @tax_component_amt=los_dev_fctr_amt from PREM_ADJ_PARMET_DTL
					where
					prem_adj_perd_id=@premium_adj_period_id
					and prem_adj_id=@premium_adjustment_id
					and st_id=@state_id
					and ln_of_bsn_id=@ln_of_bsn_id
					and prem_adj_pgm_id=@premium_adj_prog_id
					and coml_agmt_id=@com_agm_id
					end--end of LDF

					set @tax_component_amt=round(@tax_component_amt,0)
					set @tax_amount=@tax_component_amt*@tax_rate--tax amount calculation

					End
					else --If Tax End Date reaches
					begin
					set @tax_component_amt=NULL  --There should not be any Tax Rate and @tax_component_amt
					set @tax_rate=NULL
					set @tax_amount=NULL
					--If Tax End Date Condition is reached we need to pull Tax Amount from Prior Val Date
					/***********************************************
					* DETERMINE PREVIOUS VALID ADJUSTMENT AND POPULATE
					* CORRESPONDING PREVIOUS ILRF AMT
					***********************************************/
					exec @prev_valid_adj_id = [dbo].[fn_DeterminePrevValidAdj_NonPremium]
 						@current_premium_adjustment_id = @premium_adjustment_id,
						@customer_id = @customer_id,
						@premium_adj_prog_id = @premium_adj_prog_id,
						@adj_parmet_typ_id = 400  -- Adjustment Parameter Type for ILRF

					
					select @tax_amount=tax_amt from PREM_ADJ_TAX_DTL
					where
					prem_adj_id=@prev_valid_adj_id
					and st_id=@state_id
					and ln_of_bsn_id=@ln_of_bsn_id
					and prem_adj_pgm_id=@premium_adj_prog_id
					and coml_agmt_id=@com_agm_id
					and dedtbl_tax_cmpnt_id=@component_id

					end

					set @tax_amount=round(@tax_amount,0) 
					
					--Populating output child table with calculated values
					exec [dbo].[AddPREM_ADJ_TAX_DTL] 
					@prem_adj_tax_setup_id,
					@premium_adj_period_id,
					@premium_adjustment_id,
					@customer_id,
					@premium_adj_prog_id,
					@com_agm_id,
					@state_id,
					@ln_of_bsn_id,
					@component_id,
					@tax_component_amt,
					@tax_rate,
					@tax_amount,
					@create_user_id

					end

					fetch tax_component_base_cur into @component_id
					

			end --end of cursor tax_component_base_cur / while loop
		close tax_component_base_cur
		deallocate tax_component_base_cur

		



		select @sum_tax_amount=sum(isnull(tax_amt,0)) from PREM_ADJ_TAX_DTL
		where st_id=@state_id 
		and ln_of_bsn_id=@ln_of_bsn_id
		and prem_adj_pgm_id=@premium_adj_prog_id
		and prem_adj_perd_id=@premium_adj_period_id
		and prem_adj_id=@premium_adjustment_id

		set @sum_tax_amount=NULL

		select @sum_tax_amount=sum(isnull(tax_amt,0)) from PREM_ADJ_TAX_DTL
		where st_id=@state_id 
		and ln_of_bsn_id=@ln_of_bsn_id
		and prem_adj_pgm_id=@premium_adj_prog_id
		and prem_adj_perd_id=@premium_adj_period_id
		and prem_adj_id=@premium_adjustment_id
		and tax_amt is not null
		--set @sum_tax_amount=isnull(@sum_tax_amount,0)


		update PREM_ADJ_TAX_SETUP
	    set tot_tax_amt_st_lvl=@sum_tax_amount
	    where st_id=@state_id 
		and ln_of_bsn_id=@ln_of_bsn_id
		and prem_adj_pgm_id=@premium_adj_prog_id
		and prem_adj_perd_id=@premium_adj_period_id
		and prem_adj_id=@premium_adjustment_id
	


		end

					fetch tax_base_cur into @state_id, @ln_of_bsn_id, @com_agm_id, @tax_typ_id
					
			end --end of cursor tax_base_cur / while loop
		close tax_base_cur
		deallocate tax_base_cur
		
		--As per the Bug fix 12018
		--Inserting the records which are not having the deductible tax setup and having the state sales service fee amount
		insert into PREM_ADJ_TAX_SETUP
		(
		[prem_adj_perd_id],
		[prem_adj_id],
		[custmr_id],
		[ln_of_bsn_id],
		[st_id],
		[prem_adj_pgm_id],
		[tax_typ_id],
		[st_sls_serv_fee_tax],
		[crte_user_id]
		)
		select 
		@premium_adj_period_id,
		@premium_adjustment_id,
		@customer_id,
		ln_of_bsn_id,
		st_id,
		@premium_adj_prog_id,
		tax_typ_id,
		tax_amt,
		@create_user_id
		from 
		(
			select
			ln_of_bsn_id,
			(select lkup_id from lkup where attr_1_txt=(select attr_1_txt from lkup where lkup_id=tax_typ_id) and lkup_typ_id=1) as st_id,
			tax_typ_id,
			tax_amt
			from dbo.INCUR_LOS_REIM_FUND_TAX_SETUP 
			where custmr_id = @customer_id
			and prem_adj_pgm_id = @premium_adj_prog_id	
			and actv_ind=1
			
		) as notused
		where not exists
		(
			select * 
			from 
			(
				select
				ln_of_bsn_id,
				st_id,
				tax_typ_id,
				st_sls_serv_fee_tax
				from dbo.PREM_ADJ_TAX_SETUP 
				where  prem_adj_perd_id = @premium_adj_period_id 
				and prem_adj_id = @premium_adjustment_id
				and prem_adj_pgm_id = @premium_adj_prog_id	
			) as used
			where notused.ln_of_bsn_id = used.ln_of_bsn_id
			and notused.st_id = used.st_id
			and notused.tax_typ_id=used.tax_typ_id
		)

		update PREM_ADJ_TAX_SETUP
	    set tot_tax_amt=isnull(st_sls_serv_fee_tax,0)+ isnull(tot_tax_amt_st_lvl,0)
	    where prem_adj_pgm_id=@premium_adj_prog_id
		and prem_adj_perd_id=@premium_adj_period_id
		and prem_adj_id=@premium_adjustment_id

		--Inserting the calculated tax postings in to the PREM_ADJ_LOS_REIM_FUND_POST_TAX output table
		if(@delete_ilrf=1)
		begin --@delete_ilrf
		if not exists(select 1 from [dbo].[PREM_ADJ_LOS_REIM_FUND_POST_TAX] where [prem_adj_perd_id] = @premium_adj_period_id and [prem_adj_id] = @premium_adjustment_id and [dedtbl_tax_cmpnt_id]<>(select lkup_id from dbo.LKUP where lkup_typ_id=54 and lkup_txt=
'CHF'))
		begin--not exists
		
		insert into [dbo].[PREM_ADJ_LOS_REIM_FUND_POST_TAX]
		   (
			[prem_adj_perd_id]
		   ,[prem_adj_id]
		   ,[custmr_id]
		   ,[crte_user_id]
		   ,[tax_typ_id]
		   ,[ln_of_bsn_id]
		   ,[st_id]
		   ,[dedtbl_tax_cmpnt_id]
		   ,[post_trns_typ_id]
		   ,[curr_amt]
		   ,[aggr_amt]
			)
			select
			ps.prem_adj_perd_id,
			ps.prem_adj_id,
			ps.custmr_id,
			@create_user_id,
			ps.tax_typ_id,
			ps.ln_of_bsn_id,
			ps.st_id,
			pd.dedtbl_tax_cmpnt_id,
			dbo.fn_GetPostTrnsTypID(ps.st_id),
			isnull(round(SUM(pd.tax_amt),0),0),
			0 
			from dbo.PREM_ADJ_TAX_SETUP ps
			inner join dbo.PREM_ADJ_TAX_DTL pd on ps.prem_adj_tax_setup_id=pd.prem_adj_tax_setup_id
			where ps.prem_adj_pgm_id=@premium_adj_prog_id
			and ps.prem_adj_id=@premium_adjustment_id
			and ps.prem_adj_perd_id=@premium_adj_period_id
			and tot_tax_amt_st_lvl is not null
			group by
			ps.prem_adj_perd_id,
			ps.prem_adj_id,
			ps.custmr_id,
			ps.tax_typ_id,
			ps.ln_of_bsn_id,
			ps.st_id,
			pd.dedtbl_tax_cmpnt_id

			insert into [dbo].[PREM_ADJ_LOS_REIM_FUND_POST_TAX]
		   (
			[prem_adj_perd_id]
		   ,[prem_adj_id]
		   ,[custmr_id]
		   ,[crte_user_id]
		   ,[tax_typ_id]
		   ,[ln_of_bsn_id]
		   ,[st_id]
		   ,[post_trns_typ_id]
		   ,[curr_amt]
		   ,[aggr_amt]
			)
			select
			prem_adj_perd_id,
			prem_adj_id,
			custmr_id,
			@create_user_id,
			NULL,
			ln_of_bsn_id,
			st_id,
			dbo.fn_GetPostTrnsTypID(st_id),
			isnull(st_sls_serv_fee_tax,0),
			0 
			from dbo.PREM_ADJ_TAX_SETUP 
			where prem_adj_pgm_id=@premium_adj_prog_id
			and prem_adj_id=@premium_adjustment_id
			and prem_adj_perd_id=@premium_adj_period_id
			and st_sls_serv_fee_tax is not null

			end	--not exists
			end--@delete_ilrf



		
			--TBD:Verify with users regarding determination of previos valid adjustment id
			--100 is initial having tax ,101 is subsequesnt not having tax, 102 is subsequent of 101 while pullling prior which one we need to consider
			/***********************************************
			* DETERMINE PREVIOUS VALID ADJUSTMENT AND POPULATE
			* CORRESPONDING PREVIOUS ILRF AMT
			***********************************************/
			exec @prev_valid_adj_id = [dbo].[fn_DeterminePrevValidAdj_NonPremium]
				@current_premium_adjustment_id = @premium_adjustment_id,
				@customer_id = @customer_id,
				@premium_adj_prog_id = @premium_adj_prog_id,
				@adj_parmet_typ_id = 400  -- Adjustment Parameter Type for ILRF
			
			select @prev_valid_adj_perd_id = prem_adj_perd_id from dbo.PREM_ADJ_PERD
			where prem_adj_id = @prev_valid_adj_id
			and prem_adj_pgm_id = @premium_adj_prog_id
		
			if @debug = 1
			begin
			print '@prev_valid_adj_id: ' + convert(varchar(20), @prev_valid_adj_id ) 
			print '@prev_valid_adj_perd_id: ' + convert(varchar(20), @prev_valid_adj_perd_id ) 
			end
		
			--TBD:Incase of subsequent only or not
			--Updating Prior values
			if(@delete_ilrf=1)
			begin --@delete_ilrf
			
			update dbo.[PREM_ADJ_LOS_REIM_FUND_POST_TAX]
			set prior_yy_amt = isnull(b.lim_amt,0), 
				adj_prior_yy_amt = isnull(b.lim_amt,0) 
			from dbo.[PREM_ADJ_LOS_REIM_FUND_POST_TAX] as a
			join 
			(
				select 
				tax_typ_id,
				st_id,
				ln_of_bsn_id,
				lim_amt
				from dbo.[PREM_ADJ_LOS_REIM_FUND_POST_TAX]
				where prem_adj_id = @prev_valid_adj_id
				and prem_adj_perd_id = @prev_valid_adj_perd_id
				and custmr_id = @customer_id
			) as b
			on a.tax_typ_id = b.tax_typ_id and a.st_id=b.st_id and a.ln_of_bsn_id=b.ln_of_bsn_id
			where prem_adj_perd_id = @premium_adj_period_id
			and prem_adj_id = @premium_adjustment_id
			and custmr_id = @customer_id

			--TBD:Incase of subsequent only or not
			--Updating Prior values
			update dbo.[PREM_ADJ_LOS_REIM_FUND_POST_TAX]
			set prior_yy_amt = isnull(b.lim_amt,0), 
				adj_prior_yy_amt = isnull(b.lim_amt,0) 
			from dbo.[PREM_ADJ_LOS_REIM_FUND_POST_TAX] as a
			join 
			(
				select 
				st_id,
				ln_of_bsn_id,
				lim_amt
				from dbo.[PREM_ADJ_LOS_REIM_FUND_POST_TAX]
				where prem_adj_id = @prev_valid_adj_id
				and prem_adj_perd_id = @prev_valid_adj_perd_id
				and custmr_id = @customer_id
				and tax_typ_id is null
			) as b
			on a.st_id=b.st_id and a.ln_of_bsn_id=b.ln_of_bsn_id
			where prem_adj_perd_id = @premium_adj_period_id
			and prem_adj_id = @premium_adjustment_id
			and custmr_id = @customer_id
			and tax_typ_id is null
			
			
			end--@delete_ilrf
			



			--Code to enter the postings which are in initial adjustment and not in subsequent.
      		if(@delete_ilrf=1)
 			begin --@delete_ilrf

			--This Block is to verify the tax amounts based on State and LOB from prior to current adjustments
			--if there is no tax calculated for a state for which there is tax calculation in the prior adjustmnets
			--Then that value will be insetred by having current value as null.
			insert into [dbo].[PREM_ADJ_LOS_REIM_FUND_POST_TAX]
		   (
			[prem_adj_perd_id]
		   ,[prem_adj_id]
		   ,[custmr_id]
		   ,[crte_user_id]
		   ,[tax_typ_id]
		   ,[ln_of_bsn_id]
		   ,[st_id]
		   ,[post_trns_typ_id]
		   ,[prior_yy_amt]
		   ,[adj_prior_yy_amt]
		   ,[dedtbl_tax_cmpnt_id]
			)
			select
			@premium_adj_period_id,
			@premium_adjustment_id,
			@customer_id,
			@create_user_id,
			tax_typ_id,
			ln_of_bsn_id,
			st_id,
		    dbo.fn_GetPostTrnsTypID(st_id),
			curr_amt,
			curr_amt,
		    dedtbl_tax_cmpnt_id
			from 
			(
			select
			custmr_id,
			prem_adj_id,
			prem_adj_perd_id,
			tax_typ_id,
			ln_of_bsn_id,
			st_id,
			dedtbl_tax_cmpnt_id,	
			curr_amt
			from dbo.PREM_ADJ_LOS_REIM_FUND_POST_TAX
			where  prem_adj_id = @prev_valid_adj_id
			and custmr_id = @customer_id
			and prem_adj_perd_id = @prev_valid_adj_perd_id	
			and tax_typ_id is not null
			and curr_amt is not null
			and [dedtbl_tax_cmpnt_id]<>(select lkup_id from dbo.LKUP where lkup_typ_id=54 and lkup_txt='CHF')
			) as prev
		where not exists
		(
			select * 
			from 
			(
				select
				custmr_id,
				prem_adj_id,
				prem_adj_perd_id,
				tax_typ_id,
				ln_of_bsn_id,
				st_id,	
				dedtbl_tax_cmpnt_id,
				post_amt
				from dbo.PREM_ADJ_LOS_REIM_FUND_POST_TAX 
				where  prem_adj_id = @premium_adjustment_id
				and prem_adj_perd_id = @premium_adj_period_id
				and custmr_id = @customer_id
				and tax_typ_id is not null
				and [dedtbl_tax_cmpnt_id]<>(select lkup_id from dbo.LKUP where lkup_typ_id=54 and lkup_txt='CHF')
				
			) as curr
			where prev.ln_of_bsn_id = curr.ln_of_bsn_id
			and prev.st_id = curr.st_id
			and prev.dedtbl_tax_cmpnt_id=curr.dedtbl_tax_cmpnt_id
		)
		
			--This Block is to verify the ALAE amounts based on State and LOB from prior to current adjustments
			--if there is no ALAE amount for a state for which there is ALAE Amount in the prior adjustmnets
			--Then that value will be insetred by having current value as null.
			insert into [dbo].[PREM_ADJ_LOS_REIM_FUND_POST_TAX]
		   (
			[prem_adj_perd_id]
		   ,[prem_adj_id]
		   ,[custmr_id]
		   ,[crte_user_id]
		   ,[tax_typ_id]
		   ,[ln_of_bsn_id]
		   ,[st_id]
		   ,[post_trns_typ_id]
		   ,[prior_yy_amt]
		   ,[adj_prior_yy_amt]
			)
			select
			@premium_adj_period_id,
			@premium_adjustment_id,
			@customer_id,
			@create_user_id,
			tax_typ_id,
			ln_of_bsn_id,
			st_id,
		    dbo.fn_GetPostTrnsTypID(st_id),
			curr_amt,
			curr_amt
			from 
			(
			select
			custmr_id,
			prem_adj_id,
			prem_adj_perd_id,
			tax_typ_id,
			ln_of_bsn_id,
			st_id,	
			curr_amt
			from dbo.PREM_ADJ_LOS_REIM_FUND_POST_TAX
			where  prem_adj_id = @prev_valid_adj_id
			and custmr_id = @customer_id
			and prem_adj_perd_id = @prev_valid_adj_perd_id	
			and tax_typ_id is null
			and curr_amt is not null
			) as prev
		where not exists
		(
			select * 
			from 
			(
				select
				custmr_id,
				prem_adj_id,
				prem_adj_perd_id,
				tax_typ_id,
				ln_of_bsn_id,
				st_id,	
				post_amt
				from dbo.PREM_ADJ_LOS_REIM_FUND_POST_TAX 
				where  prem_adj_id = @premium_adjustment_id
				and prem_adj_perd_id = @premium_adj_period_id
				and custmr_id = @customer_id
				and tax_typ_id is null
				
			) as curr
			where prev.ln_of_bsn_id = curr.ln_of_bsn_id
			and prev.st_id = curr.st_id
			
		)
			end--@delete_ilrf

	
			--Evaluate limited amount and posting amount
			update [dbo].[PREM_ADJ_LOS_REIM_FUND_POST_TAX]
			set lim_amt = isnull(curr_amt,0) + isnull(aggr_amt,0), 
				post_amt = round(isnull(curr_amt,0) + isnull(aggr_amt,0) - isnull(adj_prior_yy_amt,0),0) 
			where [prem_adj_perd_id] = @premium_adj_period_id
			and [prem_adj_id] = @premium_adjustment_id
			and [custmr_id] = @customer_id


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
		rollback transaction ModAISCalcDeductibleTax
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





GO

if object_id('ModAISCalcDeductibleTax') is not null
        print 'Created Procedure ModAISCalcDeductibleTax'
else
        print 'Failed Creating Procedure ModAISCalcDeductibleTax'
go

if object_id('ModAISCalcDeductibleTax') is not null
        grant exec on ModAISCalcDeductibleTax to  public
go

