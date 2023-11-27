
if exists (select 1 from sysobjects 
		where name = 'ModAISCalcEscrow' and type = 'P')
	drop procedure ModAISCalcEscrow
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcEscrow
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used for Escrow calculations.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

-----               30/01/2009
-----               depst_amt field retrieved from  PREM_ADJ_PGM_SETUP table replaced with escr_prev_amt field
-----                and stored in @depst_amt variable

---------------------------------------------------------------------

create procedure [dbo].[ModAISCalcEscrow] 
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@premium_adj_prog_id int,
@create_user_id int,
@err_msg_op varchar(1000) output
as

begin
	set nocount on

declare	@escrow_prev_bil_amt decimal(15,2),
		@months_held decimal(15,2),
		@div_by_amt decimal(15,2),
		@months_held_amt decimal(15,2),
		@previous_credits decimal(15,2),
		@esc_amt_billed decimal(15,2),
		@dep_amt decimal(15,2),
		@amt_adj_by_user decimal(15,2),
		@adj_tot_paid_los_bil_amt decimal(15,7),
		@adj_paid_los_bil_amt decimal(15,7),
		@adj_misc_inv_amt decimal(15,7),
		@div_by int,
		@plb_months int,
		@prev_valid_adj_id int,
		@cnt_prev_adjs int,
		@prem_adj_parmet_setup_id int,
		@prem_adj_pgm_setup_id int,
		@first_adj_non_prem int,
		@freq smallint,
		@months_elapsed smallint,
		@months_to_val smallint,
		@fst_adj_dt datetime,
		@next_val_date datetime,
		@prem_adj_valn_dt datetime,
		@pgm_period_valn_dt datetime,
		@escr_mnl_overrid_ind bit,
		@escr_tot_amt decimal(15,2),
		@err_message varchar(500),
		@trancount int

--Check if Escrow calc needs to be performed for this adjustment
select 
@prem_adj_valn_dt = valn_dt
from dbo.PREM_ADJ
where prem_adj_id = @premium_adjustment_id

select 
@pgm_period_valn_dt = nxt_valn_dt_non_prem_dt
from dbo.PREM_ADJ_PGM
where prem_adj_pgm_id = @premium_adj_prog_id

print 'Before Escrow valuation date validation'

if (@prem_adj_valn_dt <> @pgm_period_valn_dt)
begin
				set @err_message = 'Escrow: Valuation date for this adjustment and the valuation date specified in the program period setup do not match' 
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

print 'Escrow: valuation date validation PASSED; START OF CALC'


set @trancount = @@trancount
--print @trancount
if @trancount >= 1
    save transaction ModAISCalcEscrow
else
    begin transaction


begin try

		/**************************
		* Determine first adjustment
		**************************/

		exec @cnt_prev_adjs = [dbo].[fn_DetermineFirstAdjustment_ParmetType]
			@premium_adj_prog_id = @premium_adj_prog_id,
			@adj_parmet_typ_id = 399 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> ESCROW


		if (@cnt_prev_adjs = 0) -- No existing adjustments; this is initial adjustment
		begin
			print 'initial adjustment check'
			/*************************************************************
			* Additional check required for program period setup ->FIRST ADJ NON-PREMIUM 
			* before initiating Escrow calc. Only if inception to VAL DATE is 
			* more than this value, intiate LBA calc.
			*************************************************************/
--			select
--			@fst_adj_dt = fst_adj_non_prem_dt,
--			@next_val_date = nxt_valn_dt_non_prem_dt 
--			from dbo.PREM_ADJ_PGM 
--			where prem_adj_pgm_id = @premium_adj_prog_id
--
--			if (@fst_adj_dt <> @next_val_date)
--			begin
--				set @err_message = 'Escrow: First Adjustment Date(NP) is not equal to Next Valuation Date(NP)' 
--				+ ';customer ID: ' + convert(varchar(20),@customer_id) 
--				+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
--				rollback transaction ModAISCalcEscrow
--				set @err_msg_op = @err_message
--				exec [dbo].[AddAPLCTN_STS_LOG] 
--					@premium_adjustment_id = @premium_adjustment_id,
--					@customer_id = @customer_id,
--					@premium_adj_prog_id = @premium_adj_prog_id,
--					@err_msg = @err_message,
--					@create_user_id = @create_user_id
--				return
--			end
		end
		else
		begin -- Subsequent adjustment / this is not initial adjustment
			select @months_elapsed = datediff(mm,prev_valn_dt_non_prem_dt,nxt_valn_dt_non_prem_dt ), 
				   @freq = freq_non_prem_mms_cnt -- Non-premium frequency
			from dbo.PREM_ADJ_PGM 
			where prem_adj_pgm_id = @premium_adj_prog_id

			if @months_elapsed <> @freq
			begin
				set @err_message = 'Escrow: Difference between Next Valuation Date(NP) and Previous Valuation Date(NP) is not consistent with frequency for Escrow'
				+ ';customer ID: ' + convert(varchar(20),@customer_id) 
				+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
				rollback transaction ModAISCalcEscrow
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
			set @err_message = 'Escrow: Current date is less than the Next Valuation Date(NP)'
			+ ';customer ID: ' + convert(varchar(20),@customer_id) 
			+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
			rollback transaction ModAISCalcEscrow
			set @err_msg_op = @err_message
			exec [dbo].[AddAPLCTN_STS_LOG] 
				@premium_adjustment_id = @premium_adjustment_id,
				@customer_id = @customer_id,
				@premium_adj_prog_id = @premium_adj_prog_id,
				@err_msg = @err_message,
				@create_user_id = @create_user_id
			return
		end

		select
		@prem_adj_pgm_setup_id = prem_adj_pgm_setup_id, 
		@plb_months = escr_paid_los_bil_mms_nbr,
		@div_by = escr_dvsr_nbr,
		@months_held = escr_mms_held_amt,
		@dep_amt = escr_prev_amt
		from
		dbo.PREM_ADJ_PGM_SETUP 
		where 
		custmr_id = @customer_id 
		and prem_adj_pgm_id = @premium_adj_prog_id
		and actv_ind = 1
		and adj_parmet_typ_id = 399 -- Adjustment Parameter Type for Escrow

		if @prem_adj_pgm_setup_id is null
		begin
			rollback transaction ModAISCalcEscrow
			return
		end

		set @plb_months = (@plb_months - 1) * -1

		-- Pull amount from Paid Loss Billing table
		select 
		@adj_paid_los_bil_amt = sum(adj_tot_paid_los_bil_amt) 
		from dbo.PREM_ADJ_PAID_LOS_BIL plb --PREM_ADJ_LSI_LOS
		--inner join dbo.PREM_ADJ_PGM_SETUP_POL pol on (plb.coml_agmt_id = pol.coml_agmt_id) and (plb.custmr_id = pol.custmr_id)
		where 
		lsi_valn_dt <= (select valn_dt from dbo.PREM_ADJ where prem_adj_id = @premium_adjustment_id)
		and lsi_valn_dt >= (select dateadd(month,@plb_months,valn_dt) from dbo.PREM_ADJ where prem_adj_id = @premium_adjustment_id)
		and plb.prem_adj_id = @premium_adjustment_id
		and plb.prem_adj_perd_id = @premium_adj_period_id
		and plb.custmr_id = @customer_id

		--Pull amount from Misc. Invoice table
		select 
		@adj_misc_inv_amt = sum(m.post_amt)
		from dbo.PREM_ADJ_MISC_INVC m 
		inner join dbo.POST_TRNS_TYP p on (m.post_trns_typ_id = p.post_trns_typ_id)
		where m.prem_adj_id = @premium_adjustment_id
		and m.prem_adj_perd_id = @premium_adj_period_id
		and m.custmr_id = @customer_id
		and p.post_trns_typ_id = 1

		set @adj_paid_los_bil_amt = isnull(@adj_paid_los_bil_amt,0)
		set @adj_misc_inv_amt = isnull(@adj_misc_inv_amt,0)

		set @adj_tot_paid_los_bil_amt = @adj_paid_los_bil_amt + @adj_misc_inv_amt

		set @div_by_amt = @adj_tot_paid_los_bil_amt / @div_by
		set @months_held_amt = @div_by_amt * @months_held

		print '**********INTERMEDIATE VALUES**********************'
		print '@plb_months: ' + convert(varchar(20), @plb_months)
		print '@div_by: ' + convert(varchar(20), @div_by)
		print '@div_by_amt: ' + convert(varchar(20), @div_by_amt)
		print '@months_held: ' + convert(varchar(20), @months_held)
		print '@months_held_amt: ' + convert(varchar(20), @months_held_amt)


		exec [dbo].[AddPREM_ADJ_PARMET_SETUP] 
			@premium_adj_period_id ,
			@premium_adjustment_id ,
			@customer_id ,
			@prem_adj_pgm_setup_id ,
			@premium_adj_prog_id ,
			399, -- Lookup value for Adjustment Parameter type : "Escrow"
			@create_user_id ,
			@prem_adj_parmet_setup_id_op = @prem_adj_parmet_setup_id output


		/*******************************************
		* Give credits for previously billed amounts
		********************************************/

		-- Handle initial deposit amount

		if (@cnt_prev_adjs = 0) -- No existing adjustments this is initial adjustment
		begin
			select @dep_amt = escr_prev_amt
			from 
			dbo.PREM_ADJ_PGM_SETUP
			where prem_adj_pgm_id = @premium_adj_prog_id
			and custmr_id = @customer_id
			and adj_parmet_typ_id = 399 -- Adjustment Parameter Type for Escrow
		end
		else
		begin -- This is not initial adjustment
			set @dep_amt = 0
		end

		/**********************************************
		* Retrieve amounts from the previous adjustment
		**********************************************/
		exec @prev_valid_adj_id = [dbo].[fn_DeterminePrevValidAdj_NonPremium]
 			@current_premium_adjustment_id = @premium_adjustment_id,
			@customer_id = @customer_id,
			@premium_adj_prog_id = @premium_adj_prog_id,
			@adj_parmet_typ_id = 399 -- Adjustment Parameter Type for Escrow

		select @escrow_prev_bil_amt = isnull(stp.escr_adj_amt,0),
			   @amt_adj_by_user = isnull(stp.tot_amt,0) - isnull(stp.escr_amt,0) --isnull(stp.escr_amt,0) - isnull(stp.tot_amt,0)
		from 
		dbo.PREM_ADJ_PARMET_SETUP stp
		inner join dbo.PREM_ADJ_PERD prd on (stp.prem_adj_perd_id = prd.prem_adj_perd_id) and (stp.prem_adj_id = prd.prem_adj_id)
		--inner join dbo.PREM_ADJ_PGM_SETUP adp on (stp.prem_adj_pgm_setup_id = adp.prem_adj_pgm_setup_id)
		where stp.prem_adj_id = @prev_valid_adj_id
		/*
		stp.prem_adj_id in
		(
			select max(pa.prem_adj_id) from dbo.PREM_ADJ pa
			inner join dbo.PREM_ADJ_STS ps on (pa.prem_adj_id = ps.prem_adj_id) and (pa.custmr_id = ps.custmr_id)
			where 
			pa.valn_dt in
			(
				select max(valn_dt) 
				from dbo.PREM_ADJ 
				where valn_dt < (
									select 
									valn_dt 
									from PREM_ADJ 
									where prem_adj_id = @premium_adjustment_id
								)
								and custmr_id = @customer_id
			)
			and ps.adj_sts_typ_id in (349,352) -- Lookup IDs for applicable statuses: Final Invoice and Transmitted
			and pa.custmr_id = @customer_id
		)
		*/
		and prd.prem_adj_pgm_id = @premium_adj_prog_id
		and stp.adj_parmet_typ_id = 399 -- Adjustment Parameter Type for Escrow
		--and adp.incld_ernd_retro_prem_ind = 0 -- not included in ERP


		set @previous_credits = isnull(@escrow_prev_bil_amt,0) + isnull(@dep_amt,0) + isnull(@amt_adj_by_user,0)
		set @esc_amt_billed = @months_held_amt + (@previous_credits * -1)


		set @adj_tot_paid_los_bil_amt = round(@adj_tot_paid_los_bil_amt ,0)
		set @months_held_amt = round(@months_held_amt ,0)
		set @previous_credits = round(@previous_credits ,0)
		set @esc_amt_billed = round(@esc_amt_billed ,0)

		print '**********COMPUTED VALUES**********************'
		print '@adj_tot_paid_los_bil_amt: ' + convert(varchar(20), @adj_tot_paid_los_bil_amt)
		print '@months_held_amt: ' + convert(varchar(20), @months_held_amt)
		print '@escrow_prev_bil_amt: ' + convert(varchar(20), @escrow_prev_bil_amt)
		print '@previous_credits: ' + convert(varchar(20), @previous_credits)
		print '@esc_amt_billed: ' + convert(varchar(20), @esc_amt_billed)
		print '@dep_amt: ' + convert(varchar(20), @dep_amt)

		update dbo.PREM_ADJ_PARMET_SETUP WITH (ROWLOCK)
		set escr_adj_paid_los_amt = @adj_tot_paid_los_bil_amt,
			escr_adj_amt = @months_held_amt,
			escr_prevly_biled_amt = @previous_credits,
			escr_amt = @esc_amt_billed, --@months_held_amt + (@previous_credits * -1),
			tot_amt = @esc_amt_billed --@months_held_amt + (@previous_credits * -1)
		where
		prem_adj_perd_id = @premium_adj_period_id
		and prem_adj_id = @premium_adjustment_id
		and adj_parmet_typ_id = 399 -- Adjustment Parameter Type for Escrow

		
		--verify escr_mnl_overrid_ind to restrict the calc engine to over write user enterd tot_amt
		select @escr_mnl_overrid_ind=escr_mnl_overrid_ind from prem_adj_perd where prem_adj_perd_id=@premium_adj_period_id
		select @escr_tot_amt=escr_tot_amt from prem_adj_perd where prem_adj_perd_id=@premium_adj_period_id
		if(isnull(@escr_mnl_overrid_ind,0)<>1)
		begin
			update dbo.PREM_ADJ_PARMET_SETUP WITH (ROWLOCK)
			set tot_amt = @esc_amt_billed --@months_held_amt + (@previous_credits * -1)
			where
			prem_adj_perd_id = @premium_adj_period_id
			and prem_adj_id = @premium_adjustment_id
			and adj_parmet_typ_id = 399 -- Adjustment Parameter Type for Escrow
		end
		else
			begin
				update dbo.PREM_ADJ_PARMET_SETUP WITH (ROWLOCK)
				set tot_amt = @escr_tot_amt --@months_held_amt + (@previous_credits * -1)
				where
				prem_adj_perd_id = @premium_adj_period_id
				and prem_adj_id = @premium_adjustment_id
				and adj_parmet_typ_id = 399 -- Adjustment Parameter Type for Escrow
			end
--		update dbo.PREM_ADJ_PARMET_SETUP WITH (ROWLOCK)
--		set incur_los_reim_fund_prevly_biled_amt = @escrow_prev_bil_amt,
--			--los_base_asses_depst_amt = @dep_amt,
--			tot_amt = incur_los_reim_fund_lim_amt /*- @dep_amt*/ - @escrow_prev_bil_amt
--		from 
--		dbo.PREM_ADJ_PARMET_SETUP pas
--		inner join dbo.PREM_ADJ_PGM_SETUP aps on aps.prem_adj_pgm_setup_id = pas.prem_adj_pgm_setup_id
--		where
--		pas.prem_adj_perd_id = @premium_adj_period_id
--		and prem_adj_id = @premium_adjustment_id
--		--and aps.incld_ernd_retro_prem_ind = 0
--		and aps.adj_parmet_typ_id = 399 -- Adjustment Parameter Type for Escrow
--		and pas.adj_parmet_typ_id = 399

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
		rollback transaction ModAISCalcEscrow
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

if object_id('ModAISCalcEscrow') is not null
	print 'Created Procedure ModAISCalcEscrow'
else
	print 'Failed Creating Procedure ModAISCalcEscrow'
go

if object_id('ModAISCalcEscrow') is not null
	grant exec on ModAISCalcEscrow to public
go





