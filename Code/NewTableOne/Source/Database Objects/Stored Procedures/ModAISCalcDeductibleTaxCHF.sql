if exists (select 1 from sysobjects 
                where name = 'ModAISCalcDeductibleTaxCHF' and type = 'P')
        drop procedure ModAISCalcDeductibleTaxCHF
go
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcDeductibleTaxCHF
-----
-----	Version:	SQL Server 2012

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
-----   Created Date :  01/26/15 (AS part of CHF Project)

-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

-----               01/26/15	Venkat Kolimi
-----				Created Procedure

---------------------------------------------------------------------

CREATE procedure [dbo].[ModAISCalcDeductibleTaxCHF] 
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@delete_chf bit,
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
    save transaction ModAISCalcDeductibleTaxCHF
else
    begin transaction


begin try


		select 
		@prem_adj_valn_dt = valn_dt
		from dbo.PREM_ADJ
		where prem_adj_id = @premium_adjustment_id


    	 --Start of main loop which determines the calculations needed based on the tax setup we had for the 
		 --program period receieved from the calling environment
		declare chf_tax_base_cur cursor LOCAL FAST_FORWARD READ_ONLY 
		for 
		(select distinct
		dts.st_id,
		s.ln_of_bsn_id,
		ca.coml_agmt_id,
		tax_typ_id,
		dts.dedtbl_tax_cmpnt_id
		from dbo.prem_adj_parmet_dtl s
		inner join dbo.prem_adj_parmet_setup on  prem_adj_parmet_setup.prem_adj_parmet_setup_id=s.prem_adj_parmet_setup_id
		inner join dbo.COML_AGMT ca on (ca.coml_agmt_id=s.coml_agmt_id)
		inner join dbo.DEDTBL_TAX_SETUP dts on (dts.st_id=s.ssst_st_id) and (dts.ln_of_bsn_id=s.ln_of_bsn_id) and  (dts.pol_eff_dt <= ca.pol_eff_dt)
		inner join dbo.LKUP CMPNTLkup on (CMPNTLkup.lkup_id=dts.dedtbl_tax_cmpnt_id) and (CMPNTLkup.lkup_txt='CHF') and (CMPNTLkup.lkup_typ_id=54)
		where 
		prem_adj_parmet_setup.prem_adj_id=@premium_adjustment_id 
		and prem_adj_parmet_setup.adj_parmet_typ_id=398 
		and prem_adj_parmet_setup.prem_adj_pgm_id=@premium_adj_prog_id
		and prem_adj_parmet_setup.prem_adj_perd_id=@premium_adj_period_id
		and dts.actv_ind=1
		and s.ssst_amt<>0.00)

		open chf_tax_base_cur
		fetch chf_tax_base_cur into @state_id, @ln_of_bsn_id, @com_agm_id, @tax_typ_id,@component_id

		while @@Fetch_Status = 0
			begin
				begin
					if @debug = 1
					begin
				    print'*******************CHF TAX: START OF OUTER ITERATION*********' 
				    print'---------------Input Params-------------------' 

					print' @com_agm_id:- ' + convert(varchar(20), @com_agm_id)  
					print' @state_id:- ' + convert(varchar(20), @state_id)  
					print' @customer_id: ' + convert(varchar(20), @customer_id)
					print' @ln_of_bsn_id: ' + convert(varchar(20), @ln_of_bsn_id ) 
					print' @tax_typ_id: ' + convert(varchar(20), isnull(@tax_typ_id,0))  
					print' @component_id:- ' + convert(varchar(20), @component_id)  
					end
 
					
					--Start of inner loop to perform the state level calculations
					--declare tax_component_base_cur cursor LOCAL FAST_FORWARD READ_ONLY 
					--for 
					--select distinct dedtbl_tax_cmpnt_id from DEDTBL_TAX_SETUP
					--where ln_of_bsn_id=@ln_of_bsn_id
					--and tax_typ_id=@tax_typ_id
					--and st_id=@state_id 
					--and pol_eff_dt<=(select pol_eff_dt from coml_agmt where coml_agmt_id=@com_agm_id)
					--and actv_ind=1
					


					--open tax_component_base_cur
					--fetch tax_component_base_cur into @component_id

					--while @@Fetch_Status = 0
					--	begin
					--		begin
								if @debug = 1
								begin
								print'*******************TAX: START OF TAX RATE CALCULATIONS*********' 
								print' @component_id:- ' + convert(varchar(20), @component_id)  
								end

								set @tax_component_amt=NULL
								set @tax_rate=NULL
								set @tax_end_dt=NULL

								--Populating the calculated/required values in the output master table at state level.
								--this will retrun the primary key cretaed on that row which will be used as a refernce key in the child table 
while inserting records 
								if not exists(select * from dbo.PREM_ADJ_TAX_SETUP where prem_adj_perd_id = @premium_adj_period_id and 
prem_adj_id = @premium_adjustment_id and st_id = @state_id and ln_of_bsn_id=@ln_of_bsn_id)								
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
								select @prem_adj_tax_setup_id=prem_adj_tax_setup_id from dbo.PREM_ADJ_TAX_SETUP where prem_adj_perd_id = 
@premium_adj_period_id and prem_adj_id = @premium_adjustment_id and st_id = @state_id and ln_of_bsn_id=@ln_of_bsn_id
								end
					
								--fn_RetrieveTax_rt is called here to Retrieve the Tax Rate based on the goven parametrs
								exec @tax_rate = [dbo].[fn_RetrieveTax_rt]
 								@ln_of_bsn_id=@ln_of_bsn_id,
								@state_id=@state_id,
								@tax_typ_id=@tax_typ_id,
								@com_agm_id=@com_agm_id,
								@component_id=@component_id

								--fn_RetrieveTax_End_Date is called here to retrieve the tax Enda Date associated for the given combination to 
determine 
								--the calculations needed
								exec @tax_end_dt = [dbo].[fn_RetrieveTax_End_Date]
 								@ln_of_bsn_id=@ln_of_bsn_id,
								@state_id=@state_id,
								@tax_typ_id=@tax_typ_id,
								@com_agm_id=@com_agm_id,
								@component_id=@component_id
					
					
					if(@prem_adj_valn_dt < case when @tax_end_dt is null then cast('12/31/9999' as datetime)  else @tax_end_dt end )--if Tax End Date 
Validation
					begin
				
					--CHF
					select 
					@tax_component_amt=sum(papd.tot_amt) 
					from 
					dbo.PREM_ADJ_PARMET_DTL papd
					inner join dbo.PREM_ADJ_PARMET_SETUP paps on (paps.prem_adj_parmet_setup_id=papd.prem_adj_parmet_setup_id) and 
(paps.adj_parmet_typ_id=398)
					where
					papd.prem_adj_perd_id=@premium_adj_period_id
					and papd.prem_adj_id=@premium_adjustment_id
					and papd.ssst_st_id=@state_id
					and papd.ln_of_bsn_id=@ln_of_bsn_id
					and papd.prem_adj_pgm_id=@premium_adj_prog_id
					and papd.coml_agmt_id=@com_agm_id
					
					


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
						@adj_parmet_typ_id = 398  -- Adjustment Parameter Type for CHF

					
					select 
					@tax_amount=tax_amt 
					from 
					PREM_ADJ_TAX_DTL
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

					--end

					--fetch tax_component_base_cur into @component_id
					

		--	end --end of cursor tax_component_base_cur / while loop
		--close tax_component_base_cur
		--deallocate tax_component_base_cur

		



		select @sum_tax_amount=sum(isnull(tax_amt,0)) 
		from PREM_ADJ_TAX_DTL
		where st_id=@state_id 
		and ln_of_bsn_id=@ln_of_bsn_id
		and prem_adj_pgm_id=@premium_adj_prog_id
		and prem_adj_perd_id=@premium_adj_period_id
		and prem_adj_id=@premium_adjustment_id

		set @sum_tax_amount=NULL

		select @sum_tax_amount=sum(isnull(tax_amt,0)) 
		from PREM_ADJ_TAX_DTL
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

					fetch chf_tax_base_cur into @state_id, @ln_of_bsn_id, @com_agm_id, @tax_typ_id,@component_id
					
			end --end of cursor chf_tax_base_cur / while loop
		close chf_tax_base_cur
		deallocate chf_tax_base_cur
		
		update PREM_ADJ_TAX_SETUP
	    set tot_tax_amt=isnull(st_sls_serv_fee_tax,0)+ isnull(tot_tax_amt_st_lvl,0)
	    where prem_adj_pgm_id=@premium_adj_prog_id
		and prem_adj_perd_id=@premium_adj_period_id
		and prem_adj_id=@premium_adjustment_id


		--Inserting the calculated tax postings in to the PREM_ADJ_LOS_REIM_FUND_POST_TAX output table
		if(@delete_chf=1)
		begin --@delete_chf
		if not exists(select 1 from [dbo].[PREM_ADJ_LOS_REIM_FUND_POST_TAX] where [prem_adj_perd_id] = @premium_adj_period_id and [prem_adj_id] = @premium_adjustment_id 
and [dedtbl_tax_cmpnt_id] in(select lkup_id from dbo.LKUP where lkup_typ_id=54 and lkup_txt
='CHF'))
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

			

			end	--not exists
			end--@delete_chf



		
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
				@adj_parmet_typ_id = 398  -- Adjustment Parameter Type for CHF
			
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
			if(@delete_chf=1)
			begin --@delete_chf
			
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
			
			
			end--@delete_chf
			



			--Code to enter the postings which are in initial adjustment and not in subsequent.
      		if(@delete_chf=1)
 			begin --@delete_chf

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
				
			) as curr
			where prev.ln_of_bsn_id = curr.ln_of_bsn_id
			and prev.st_id = curr.st_id
			and prev.dedtbl_tax_cmpnt_id=curr.dedtbl_tax_cmpnt_id
		)
		
			
			end--@delete_chf

	
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
		rollback transaction ModAISCalcDeductibleTaxCHF
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
if object_id('ModAISCalcDeductibleTaxCHF') is not null
        print 'Created Procedure ModAISCalcDeductibleTaxCHF'
else
        print 'Failed Creating Procedure ModAISCalcDeductibleTaxCHF'
go

if object_id('ModAISCalcDeductibleTaxCHF') is not null
        grant exec on ModAISCalcDeductibleTaxCHF to  public
go

