
if exists (select 1 from sysobjects 
		where name = 'ModAISCalcLBA' and type = 'P')
	drop procedure ModAISCalcLBA
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcLBA
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to populate the output tables for LBA with calculation results.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------

create procedure [dbo].[ModAISCalcLBA] 
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

declare @lba_adj_typ_id int,
		@incl_erp bit,
		@dep_amt decimal(15,2),
		@lba_prev_bil_amt decimal(15,2),
		@incl_ibnr_ldf bit,
		@adj_fctr_rt decimal(15,8),
		@fnl_overrid_amt decimal(15,2),
		@subj_paid_idnmty_amt decimal(15,2),
		@subj_resrv_idnmty_amt decimal(15,2),
		@cnt_prev_adjs int,
		@com_agm_id decimal(15,2),
		@state_id int,
		@prem_adj_pgm_setup_id int,
		@prem_adj_pgm_setup_id_tracker int,
		@prem_adj_parmet_setup_id int,
		@loss_amt decimal(15,2),
		@ldf_amt decimal(15,2),
		@ibnr_amt decimal(15,2),
		@amt_subj_lba_ft decimal(15,2),
		@lba_amt decimal(15,2),
		@sum_loss_amt decimal(15,2),
		@sum_tot_amt decimal(15,2),
		@ibnr_rt decimal(15,8),
		@ldf_rt decimal(15,8),
		@count int,
		@counter int,
		@com_agr_id int,
		@is_ibnr bit,
		@ldf_ibnr_step_ind bit,
		@incl_in_erp bit,
		@months_to_val int,
		@months_elapsed smallint,
		@first_adj int,
		@prev_valid_adj_id int,
		@freq smallint,
		@fst_adj_dt datetime,
		@next_val_date datetime,
		@pgm_setup_id int,
		@prem_adj_valn_dt datetime,
		@pgm_period_valn_dt datetime,
		@exc_ratio decimal(15,8),
		@exc_ldf_ibnr_amt decimal(15,2),
		@paid_incurred_losses decimal(15,2),
		@pgm_lkup_txt varchar(20),
		@err_message varchar(500),
		@trancount int


--Check if LBA calc needs to be performed for this adjustment
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
print 'Before LBA valuation date validation'
end

if (@prem_adj_valn_dt <> @pgm_period_valn_dt)
begin
				set @err_message = 'LBA: Valuation date for this adjustment and the valuation date specified in the program period setup do not match' 
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
print 'LBA: valuation date validation PASSED; START OF CALC'
end

set @trancount = @@trancount
--print @trancount
if @trancount >= 1
    save transaction ModAISCalcLBA
else
    begin transaction


begin try


	/**************************
	* Determine first adjustment
	**************************/

exec @cnt_prev_adjs = [dbo].[fn_DetermineFirstAdjustment]
	@premium_adj_prog_id = @premium_adj_prog_id,
	@adj_parmet_typ_id = 401 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> LBA


set @counter = 1
create table #pgm_setups
(
id int identity(1,1),
pgm_setup_id int,
incl_in_erp bit
)


create index ind ON #pgm_setups (id)

insert into #pgm_setups(pgm_setup_id,incl_in_erp)
select 
prem_adj_pgm_setup_id,
incld_ernd_retro_prem_ind
from dbo.PREM_ADJ_PGM_SETUP
where custmr_id = @customer_id
and prem_adj_pgm_id = @premium_adj_prog_id
and adj_parmet_typ_id = 401 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> LBA

select @count = count(*) from #pgm_setups

while @counter <= @count
begin


select 
@pgm_setup_id = pgm_setup_id,
@incl_in_erp = incl_in_erp
from #pgm_setups 
where id = @counter
if @debug = 1
begin
print' @pgm_setup_id:- ' + convert(varchar(20), @pgm_setup_id)  
print' @incl_in_erp:- ' + convert(varchar(20), @incl_in_erp)  
end

if @incl_in_erp = 0 -- Not included in ERP
begin
	if (@cnt_prev_adjs = 0) -- No existing adjustments; this is initial adjustment
	begin
		print 'initial adjustment'
		/*************************************************************
		* Additional check required for program period setup ->FIRST ADJ NON-PREMIUM 
		* before initiating LBA calc. Only if inception to VAL DATE is 
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
--			set @err_message = 'LBA: First Adjustment Date(NP) is not equal to Next Valuation Date(NP)' 
--			+ ';customer ID: ' + convert(varchar(20),@customer_id) 
--			+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
--			rollback transaction ModAISCalcLBA
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
			set @err_message = 'LBA: Difference between Next Valuation Date(NP) and Previous Valuation Date(NP) is not consistent with frequency for LBA'
			+ ';customer ID: ' + convert(varchar(20),@customer_id) 
			+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
			rollback transaction ModAISCalcLBA
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
		set @err_message = 'LBA: Current date is less than the Next Valuation(NP) Date'
		+ ';customer ID: ' + convert(varchar(20),@customer_id) 
		+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
		rollback transaction ModAISCalcLBA
		set @err_msg_op = @err_message
		exec [dbo].[AddAPLCTN_STS_LOG] 
			@premium_adjustment_id = @premium_adjustment_id,
			@customer_id = @customer_id,
			@premium_adj_prog_id = @premium_adj_prog_id,
			@err_msg = @err_message,
			@create_user_id = @create_user_id
		return
	end
end -- end of: if @incl_in_erp = 0
else if @incl_in_erp = 1 -- Included in ERP
begin
	if (@cnt_prev_adjs = 0) -- No existing adjustments; this is initial adjustment
	begin
		print 'initial adjustment'
		/*************************************************************
		* Additional check required for program period setup ->FIRST ADJ NON-PREMIUM 
		* before initiating LBA calc. Only if inception to VAL DATE is 
		* more than this value, intiate LBA calc.
		*************************************************************/
--		select
--		@fst_adj_dt = fst_adj_dt,
--		@next_val_date = nxt_valn_dt 
--		from dbo.PREM_ADJ_PGM 
--		where prem_adj_pgm_id = @premium_adj_prog_id
--
--		if (@fst_adj_dt <> @next_val_date)
--		begin
--			set @err_message = 'LBA: First Adjustment Date(P) is not equal to Next Valuation Date(P)'
--			+ ';customer ID: ' + convert(varchar(20),@customer_id) 
--			+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
--			rollback transaction ModAISCalcLBA
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
		select @months_elapsed = datediff(mm,prev_valn_dt,nxt_valn_dt), 
			   @freq = adj_freq_mms_intvrl_cnt -- Premium frequency
		from dbo.PREM_ADJ_PGM 
		where prem_adj_pgm_id = @premium_adj_prog_id

		if @months_elapsed <> @freq
		begin
			set @err_message = 'LBA: Difference between Next Valuation Date(P) and Previous Valuation Date(P) is not consistent with frequency for LBA'
			+ ';customer ID: ' + convert(varchar(20),@customer_id) 
			+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
			rollback transaction ModAISCalcLBA
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

	select @next_val_date = nxt_valn_dt
	from dbo.PREM_ADJ_PGM 
	where prem_adj_pgm_id = @premium_adj_prog_id

	if(getdate() < @next_val_date)
	begin
		set @err_message = 'LBA: Current date is less than the Next Valuation Date(P) for LBA'
		+ ';customer ID: ' + convert(varchar(20),@customer_id) 
		+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
		rollback transaction ModAISCalcLBA
		set @err_msg_op = @err_message
		exec [dbo].[AddAPLCTN_STS_LOG] 
			@premium_adjustment_id = @premium_adjustment_id,
			@customer_id = @customer_id,
			@premium_adj_prog_id = @premium_adj_prog_id,
			@err_msg = @err_message,
			@create_user_id = @create_user_id
		return
	end

end --end of: if @incl_in_erp = 1 


			
			
	declare lba_basic cursor LOCAL FAST_FORWARD READ_ONLY 
		for 
		select	distinct
		ls.coml_agmt_id,
		ls.st_id,
		d.prem_adj_pgm_setup_id,
		d.los_base_asses_adj_typ_id, -- LBA Adj. Type
		d.incld_ernd_retro_prem_ind, -- Included in ERP
		d.depst_amt, --Intitial deposit
		d.incld_incur_but_not_rptd_ind, -- Incl. IBNR/LDF
		d.adj_fctr_rt, --Factor
		d.fnl_overrid_amt, -- Final LBA Amt.
		ls.subj_paid_idnmty_amt ,
		ls.subj_resrv_idnmty_amt
		from
		(	
		select	
		ad.prem_adj_pgm_dtl_id,
		ap.coml_agmt_id,
		ad.st_id,
		s.prem_adj_pgm_setup_id,
		s.los_base_asses_adj_typ_id, -- LBA Adj. Type
		s.incld_ernd_retro_prem_ind, -- Included in ERP
		s.depst_amt, --Intitial deposit
		s.incld_incur_but_not_rptd_ind, -- Incl. IBNR/LDF
		ad.adj_fctr_rt, --Factor
		ad.fnl_overrid_amt, -- Final LBA Amt.
		al.subj_paid_idnmty_amt ,
		al.subj_resrv_idnmty_amt 
		from 
		(
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
		inner join dbo.PREM_ADJ_PGM_SETUP s on (s.prem_adj_pgm_id = ap.prem_adj_pgm_id ) and (s.prem_adj_pgm_setup_id = ap.prem_adj_pgm_setup_id)
		inner join PREM_ADJ_PGM_DTL ad on  (ap.prem_adj_pgm_setup_id = ad.prem_adj_pgm_setup_id ) and (al.st_id =  ad.st_id )
		where 
		s.custmr_id = @customer_id 
		and s.prem_adj_pgm_id = @premium_adj_prog_id
		--and ((al.prem_adj_id is null) or (al.prem_adj_id = 6))
		--and s.prem_adj_pgm_setup_id = @pgm_setup_id
		and s.actv_ind = 1
		--and al.actv_ind = 1
		and ad.actv_ind  = 1 
		and s.adj_parmet_typ_id = 401 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> LBA
		) as d
		right outer join
		(
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
		) ls on (d.st_id = ls.st_id) --and (d.coml_agmt_id = ls.coml_agmt_id)
		where ls.prem_adj_pgm_id = @premium_adj_prog_id
		--and ls.coml_agmt_id = d.coml_agmt_id
		--and d.prem_adj_pgm_setup_id = @pgm_setup_id
		and ls.coml_agmt_id in 
		(
			select pol.coml_agmt_id from dbo.PREM_ADJ_PGM_SETUP_POL pol
			inner join PREM_ADJ_PGM_SETUP stp on (pol.prem_adj_pgm_setup_id = stp.prem_adj_pgm_setup_id) and (pol.prem_adj_pgm_id = stp.prem_adj_pgm_id)
			inner join coml_agmt on coml_agmt.coml_agmt_id = pol.coml_agmt_id and coml_agmt.actv_ind = 1 
			and coml_agmt.adj_typ_id not in (68) and coml_agmt.covg_typ_id in (85,92)
			where pol.prem_adj_pgm_id = @premium_adj_prog_id
			and stp.adj_parmet_typ_id = 401 -- adj parameter setup for LBA
		)

		order by d.prem_adj_pgm_setup_id

		open lba_basic
		fetch lba_basic into @com_agm_id, @state_id, @prem_adj_pgm_setup_id, @lba_adj_typ_id, @incl_erp, @dep_amt, @incl_ibnr_ldf, @adj_fctr_rt, @fnl_overrid_amt, @subj_paid_idnmty_amt, @subj_resrv_idnmty_amt /*@pd_indm_amt, @pd_exp_amt, @res_indm_amt, @res_exp_amt*/

		set @prem_adj_pgm_setup_id_tracker = 0


		while @@Fetch_Status = 0
			begin
				begin
					if @debug = 1
					begin
				    print'*******************LBA: START OF ITERATION*********' 
				    print'---------------Input Params-------------------' 
					print' @com_agm_id:- ' + convert(varchar(20), @com_agm_id)  
					print' @lba_adj_typ_id:- ' + convert(varchar(20), @lba_adj_typ_id)  
					print' @incl_erp: ' + convert(varchar(20), @incl_erp)  
					print' @dep_amt: ' + convert(varchar(20), @dep_amt)
					print' @incl_ibnr_ldf: ' + convert(varchar(20), @incl_ibnr_ldf )  
					print' @adj_fctr_rt: ' + convert(varchar(20), @adj_fctr_rt ) 
					print' @fnl_overrid_amt: ' + convert(varchar(20), @fnl_overrid_amt )  
					print' @subj_paid_idnmty_amt: ' + convert(varchar(20), isnull(@subj_paid_idnmty_amt,0)) 
					--print' @subj_paid_exps_amt: ' + convert(varchar(20), isnull(@subj_paid_exps_amt,0) )  
					print' @subj_resrv_idnmty_amt: ' + convert(varchar(20), isnull(@subj_resrv_idnmty_amt,0) )  
					--print' @subj_resrv_exps_amt: ' + convert(varchar(20), isnull(@subj_resrv_exps_amt,0) )  
					end

					-- Handle potential null values
					set @subj_paid_idnmty_amt = isnull(@subj_paid_idnmty_amt,0)
					set @subj_resrv_idnmty_amt = isnull(@subj_resrv_idnmty_amt,0)
					set @dep_amt = isnull(@dep_amt,0)
	
				
					if @prem_adj_pgm_setup_id_tracker <> @prem_adj_pgm_setup_id
					begin
						if @adj_fctr_rt is null
						begin
							select @prem_adj_pgm_setup_id = ps.prem_adj_pgm_setup_id 
							from dbo.PREM_ADJ_PGM_SETUP_POL spol
							inner join dbo.PREM_ADJ_PGM_SETUP ps on (ps.prem_adj_pgm_setup_id = spol.prem_adj_pgm_setup_id) and (ps.prem_adj_pgm_id = spol.prem_adj_pgm_id)
							where ps.prem_adj_pgm_id = @premium_adj_prog_id
							and ps.adj_parmet_typ_id = 401 -- Adjustment parameter type for LBA
							and coml_agmt_id = @com_agm_id
						end

						if (@pgm_setup_id <> @prem_adj_pgm_setup_id) 
						begin
							fetch lba_basic into @com_agm_id, @state_id, @prem_adj_pgm_setup_id, @lba_adj_typ_id, @incl_erp, @dep_amt, @incl_ibnr_ldf, @adj_fctr_rt, @fnl_overrid_amt, @subj_paid_idnmty_amt, @subj_resrv_idnmty_amt 
							continue
						end


						set @prem_adj_pgm_setup_id_tracker = @prem_adj_pgm_setup_id

						if not exists(select * from dbo.PREM_ADJ_PARMET_SETUP where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and prem_adj_pgm_setup_id = @prem_adj_pgm_setup_id  and adj_parmet_typ_id = 401)
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
							/*
							insert into [dbo].[PREM_ADJ_PARMET_SETUP]
							(
							[prem_adj_perd_id]
						   ,[prem_adj_id]
						   ,[prem_adj_pgm_setup_id]
						   ,[prem_adj_pgm_id]
						   ,[custmr_id]
						   ,[adj_parmet_typ_id]
						   ,[crte_user_id]
							)
							values
							(
							@premium_adj_period_id,
							@premium_adjustment_id,
							@prem_adj_pgm_setup_id,
							@premium_adj_prog_id,
							@customer_id,
							401, -- Lookup value for Adjustment Parameter type : "LBA"
							@create_user_id					
							)
							set @prem_adj_parmet_setup_id = @@identity
							*/

							exec [dbo].[AddPREM_ADJ_PARMET_SETUP] 
								@premium_adj_period_id ,
								@premium_adjustment_id ,
								@customer_id ,
								@prem_adj_pgm_setup_id ,
								@premium_adj_prog_id ,
								401, -- Lookup value for Adjustment Parameter type : "LBA"
								@create_user_id ,
								@prem_adj_parmet_setup_id_op = @prem_adj_parmet_setup_id output
								--print '@prem_adj_parmet_setup_id.........: ' + convert(varchar(20),@prem_adj_parmet_setup_id)
						end --end of: if not exists(select * from dbo.PREM_ADJ_PARMET_SETUP where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and prem_adj_pgm_setup_id = @prem_adj_pgm_setup_id )
						else
						begin
							select @prem_adj_parmet_setup_id = prem_adj_parmet_setup_id from dbo.PREM_ADJ_PARMET_SETUP where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and prem_adj_pgm_setup_id = @prem_adj_pgm_setup_id 
						end

						if @adj_fctr_rt is null
						begin
							select @prem_adj_pgm_setup_id = ps.prem_adj_pgm_setup_id 
							from dbo.PREM_ADJ_PGM_SETUP_POL spol
							inner join dbo.PREM_ADJ_PGM_SETUP ps on (ps.prem_adj_pgm_setup_id = spol.prem_adj_pgm_setup_id) and (ps.prem_adj_pgm_id = spol.prem_adj_pgm_id)
							where ps.prem_adj_pgm_id = @premium_adj_prog_id
							and ps.adj_parmet_typ_id = 401 -- Adjustment parameter type for LBA
							and coml_agmt_id = @com_agm_id

							select @dep_amt = depst_amt, @lba_adj_typ_id = los_base_asses_adj_typ_id,
							@incl_ibnr_ldf = incld_incur_but_not_rptd_ind 
							from dbo.PREM_ADJ_PGM_SETUP 
							where prem_adj_pgm_setup_id = @prem_adj_pgm_setup_id


							select @adj_fctr_rt = adj_fctr_rt , 
							@fnl_overrid_amt = fnl_overrid_amt 
							from PREM_ADJ_PGM_DTL 
							where prem_adj_pgm_setup_id = @prem_adj_pgm_setup_id and st_id = 3 -- All other state
							and actv_ind = 1 
						end --end of: if @adj_fctr_rt is null

					end --end of: if @prem_adj_pgm_setup_id_tracker <> @prem_adj_pgm_setup_id

					if (@pgm_setup_id <> @prem_adj_pgm_setup_id) 
					begin
						fetch lba_basic into @com_agm_id, @state_id, @prem_adj_pgm_setup_id, @lba_adj_typ_id, @incl_erp, @dep_amt, @incl_ibnr_ldf, @adj_fctr_rt, @fnl_overrid_amt, @subj_paid_idnmty_amt, @subj_resrv_idnmty_amt 
						continue
					end
					if @debug = 1
					begin
					print'For AO state  @prem_adj_pgm_setup_id: ' + convert(varchar(20), @prem_adj_pgm_setup_id ) 
					print'For AO state  @adj_fctr_rt: ' + convert(varchar(20), @adj_fctr_rt ) 
					print'For AO state  @fnl_overrid_amt: ' + convert(varchar(20), @fnl_overrid_amt )  
					print'For AO state  @lba_adj_typ_id:- ' + convert(varchar(20), @lba_adj_typ_id)  
					print'For AO state  @incl_erp: ' + convert(varchar(20), @incl_erp)  
					print'For AO state  @dep_amt: ' + convert(varchar(20), @dep_amt)
					print'For AO state  @incl_ibnr_ldf: ' + convert(varchar(20), @incl_ibnr_ldf )  
					end

					if (@adj_fctr_rt is null) and (@fnl_overrid_amt is null)
					begin
						fetch lba_basic into @com_agm_id, @state_id, @prem_adj_pgm_setup_id, @lba_adj_typ_id, @incl_erp, @dep_amt, @incl_ibnr_ldf, @adj_fctr_rt, @fnl_overrid_amt, @subj_paid_idnmty_amt, @subj_resrv_idnmty_amt 
						continue
					end


					if @lba_adj_typ_id = 297 -- Lookup Type:"LBA Adj Type" Lookup Name: "Incurred"
					begin
						set @paid_incurred_losses = @subj_paid_idnmty_amt + @subj_resrv_idnmty_amt

						if @incl_ibnr_ldf = 1 --IBNR checked
						begin
							set @loss_amt = dbo.fn_ComputeLossIBNR_LDF(@lba_adj_typ_id , @com_agm_id , @premium_adj_prog_id ,	@customer_id, @subj_paid_idnmty_amt , @subj_resrv_idnmty_amt  )
						end --end: if @incl_ibnr_ldf = 1
						else --IBNR unchecked
						begin -- else: if @incl_ibnr_ldf = 1
							set @loss_amt = 0 --@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt
						end --end: if @incl_ibnr_ldf = 0
					end -- end: if @lba_adj_typ_id = 297 
					else if @lba_adj_typ_id = 298 -- Lookup Type:"LBA Adj Type" : Lookup Name: "Paid"
					begin
						set @paid_incurred_losses = @subj_paid_idnmty_amt

						if @incl_ibnr_ldf = 1 --IBNR checked
						begin
							set @loss_amt = dbo.fn_ComputeLossIBNR_LDF(@lba_adj_typ_id , @com_agm_id , @premium_adj_prog_id ,	@customer_id, @subj_paid_idnmty_amt , @subj_resrv_idnmty_amt  )
						end --end: if @incl_ibnr_ldf = 1
						else --IBNR unchecked
						begin --else: if @incl_ibnr_ldf = 1
							set @loss_amt = 0 --@subj_paid_idnmty_amt 
							--set @amt_subj_lba_ft = @loss_amt 
						end --end:- else: to  if @incl_ibnr_ldf = 1
					end --end: if @lba_adj_typ_id = 298

					set @loss_amt = isnull(@loss_amt,0)
					set @adj_fctr_rt = isnull(@adj_fctr_rt,0)
					if @debug = 1
					begin
				    print'---------------Computed Values-------------------' 
					print' @loss_amt:- ' + convert(varchar(20), @loss_amt)
					print' @adj_fctr_rt:- ' + convert(varchar(20), @adj_fctr_rt)
					end
					
					set @lba_amt = (@paid_incurred_losses + @loss_amt) * (@adj_fctr_rt)


					/*******************************************
					* This code would remove the excess LDF/IBNR 
					* portion when LDF and IBNR is included 
					* in LBA.
					********************************************/
					--Determine excess LDF/IBNR amount
					declare @ldf_ibnr_lim_ind bit

					select 
					@ldf_ibnr_lim_ind = los_dev_fctr_incur_but_not_rptd_incld_lim_ind 
					from dbo.COML_AGMT 
					where coml_agmt_id = @com_agm_id

					if ((@ldf_ibnr_lim_ind = 1) and (@incl_ibnr_ldf = 1))
					begin
						select @exc_ldf_ibnr_amt = isnull(sum(exc_ldf_ibnr_amt),0)  
						from ARMIS_LOS_POL 
						where prem_adj_pgm_id = @premium_adj_prog_id 
						and coml_agmt_id = @com_agm_id 
						and st_id = @state_id
						and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id))

						select @ibnr_rt = incur_but_not_rptd_fctr_rt  from dbo.COML_AGMT where coml_agmt_id = @com_agm_id and prem_adj_pgm_id = @premium_adj_prog_id and custmr_id = @customer_id
						if @ibnr_rt is null -- not IBNR; should be LDF
						begin
							set @is_ibnr = 0
						end
						else -- should be IBNR
						begin
							set @is_ibnr = 1
						end

						exec @exc_ratio = [dbo].[fn_ComputeLBAExcessRatioyPolicy]
  								@is_ibnr = @is_ibnr,
								@customer_id = @customer_id,
								@premium_adj_prog_id = @premium_adj_prog_id,
								@coml_agmt_id = @com_agm_id,
								@state_id = @state_id
						
						set @lba_amt = @lba_amt - (@exc_ldf_ibnr_amt * @exc_ratio)
					end


					--Handle Final Override Amount ( for MN )
					if @fnl_overrid_amt is not null
						set @lba_amt = @fnl_overrid_amt

					--This logic is for avoiding the duplicate records for the same state under DEP
					select 
					@pgm_lkup_txt = lk.lkup_txt
					from dbo.PREM_ADJ_PGM pgm
					inner join dbo.LKUP lk on (pgm.pgm_typ_id = lk.lkup_id)
					where 
					prem_adj_pgm_id = @premium_adj_prog_id
					and pgm.actv_ind = 1
					and lk.actv_ind = 1

					if(substring(isnull(@pgm_lkup_txt,''),1,3) = 'DEP')
					begin
					if @fnl_overrid_amt is not null
						set @lba_amt = @fnl_overrid_amt/(case when dbo.fn_GetDEPNumberPolicy(@premium_adjustment_id,@premium_adj_prog_id,
														@customer_id,@state_id) = 0 then 1 else dbo.fn_GetDEPNumberPolicy(@premium_adjustment_id,@premium_adj_prog_id,
														@customer_id,@state_id) end)
					end
					--End of logic
					

					set @lba_amt = isnull(@lba_amt ,0)
					set @lba_amt = round(@lba_amt ,0)
					if @debug = 1
					begin
					print' @lba_amt:- ' + convert(varchar(20), @lba_amt)
					end
						/*
						insert into PREM_ADJ_PARMET_DTL
						(
							[prem_adj_parmet_setup_id],
							[prem_adj_perd_id] ,
							[prem_adj_id],
							[custmr_id],
							[st_id],
							[los_amt]  ,
							[los_base_asses_rt] ,
							[tot_amt],
							[crte_user_id]
						)
						values
						(
							@prem_adj_parmet_setup_id,
							@premium_adj_period_id,
							@premium_adjustment_id,
							@customer_id,							
							@state_id,
							@loss_amt,
							@adj_fctr_rt,
							@lba_amt,
							@create_user_id
						)

						exec [dbo].[AddPREM_ADJ_PARMET_DTL]
							@prem_adj_parmet_setup_id , 
							@premium_adj_period_id ,
							@premium_adjustment_id ,
							@customer_id ,
							@premium_adj_prog_id ,
							@state_id ,
							@loss_amt ,
							@adj_fctr_rt ,
							@lba_amt ,
							@create_user_id 
						*/
						set @loss_amt = @paid_incurred_losses + @loss_amt

						exec [dbo].[AddPREM_ADJ_PARMET_DTL]
							@prem_adj_parmet_setup_id = @prem_adj_parmet_setup_id, 
							@premium_adj_period_id = @premium_adj_period_id,
							@premium_adjustment_id = @premium_adjustment_id,
							@customer_id = @customer_id,
							@premium_adj_prog_id = @premium_adj_prog_id,
							@coml_agmt_id = @com_agm_id,
							@state_id = @state_id,
							@lob_id = null,
							@loss_amt = @loss_amt,
							@paid_loss = null,
							@paid_alae = null,
							@resv_loss = null,
							@resv_alae = null,
							@lba_rt = @adj_fctr_rt,
							@lba_amt = @lba_amt,
							@lcf_rt = null,
							@lcf_amt = null,
							@ldf_rt = null,
							@ldf_amt = null,
							@total_amt = null,
							@create_user_id = @create_user_id


				end
				fetch lba_basic into @com_agm_id, @state_id, @prem_adj_pgm_setup_id, @lba_adj_typ_id, @incl_erp, @dep_amt, @incl_ibnr_ldf, @adj_fctr_rt, @fnl_overrid_amt, @subj_paid_idnmty_amt, @subj_resrv_idnmty_amt /*@pd_indm_amt, @pd_exp_amt, @res_indm_amt, @res_exp_amt*/
			end --end of cursor lba_basic / while loop
		close lba_basic
		deallocate lba_basic



		update dbo.PREM_ADJ_PARMET_SETUP
		set --los_amt = tm.sum_los_amt,
			los_base_asses_amt = tm.sum_lba_amt
		from dbo.PREM_ADJ_PARMET_SETUP as s
		join 
		(
			select 
			[prem_adj_parmet_setup_id],
			[prem_adj_perd_id] ,
			[prem_adj_id],
			sum([los_amt]) as sum_los_amt ,
			sum([los_base_asses_amt]) as sum_lba_amt 
			from 
			PREM_ADJ_PARMET_DTL
			where [prem_adj_id] = @premium_adjustment_id
			and [prem_adj_perd_id] = @premium_adj_period_id
			group by [prem_adj_parmet_setup_id], [prem_adj_perd_id],[prem_adj_id]
		) as tm
		on s.prem_adj_parmet_setup_id = tm.prem_adj_parmet_setup_id
		and s.prem_adj_perd_id = tm.prem_adj_perd_id
		and s.prem_adj_id = tm.prem_adj_id
		where s.adj_parmet_typ_id = 401
		and s.prem_adj_pgm_setup_id = @pgm_setup_id

		/*******************************************
		* Give credits for previously billed amounts
		********************************************/


		select @dep_amt = aps.depst_amt
		from 
		dbo.PREM_ADJ_PARMET_SETUP pas
		inner join dbo.PREM_ADJ_PGM_SETUP aps on aps.prem_adj_pgm_setup_id = pas.prem_adj_pgm_setup_id
		where
		pas.prem_adj_perd_id = @premium_adj_period_id
		and pas.prem_adj_id = @premium_adjustment_id
		and aps.incld_ernd_retro_prem_ind = 0
		and aps.adj_parmet_typ_id = 401 -- Adjustment Parameter Type for LBA

	
		--retrieve amounts from the previous adjustment
		exec @prev_valid_adj_id = [dbo].[fn_DeterminePrevValidAdj_NonPremium]
 			@current_premium_adjustment_id = @premium_adjustment_id,
			@customer_id = @customer_id,
			@premium_adj_prog_id = @premium_adj_prog_id,
			@adj_parmet_typ_id = 401 -- Adjustment Parameter Type for LBA

		select @lba_prev_bil_amt = isnull(stp.los_base_asses_amt,0) 
		from 
		dbo.PREM_ADJ_PARMET_SETUP stp
		inner join dbo.PREM_ADJ_PERD prd on (stp.prem_adj_perd_id = prd.prem_adj_perd_id) and (stp.prem_adj_id = prd.prem_adj_id)
		inner join dbo.PREM_ADJ_PGM_SETUP adp on (stp.prem_adj_pgm_setup_id = adp.prem_adj_pgm_setup_id)
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
		and stp.adj_parmet_typ_id = 401 -- Adjustment Parameter Type for LBA
		and adp.incld_ernd_retro_prem_ind = 0 -- not included in ERP

		if @debug = 1
		begin
		print '@lba_prev_bil_amt: ' + convert(varchar(20), @lba_prev_bil_amt)
		print '@dep_amt: ' + convert(varchar(20), @dep_amt)
		end

		update dbo.PREM_ADJ_PARMET_SETUP 
		set los_base_asses_prev_biled_amt = round(isnull(@lba_prev_bil_amt,0),0) ,
			los_base_asses_depst_amt = round(isnull(@dep_amt,0),0) ,
			tot_amt = round(los_base_asses_amt - isnull(@dep_amt,0) - isnull(@lba_prev_bil_amt,0),0)
		from 
		dbo.PREM_ADJ_PARMET_SETUP pas
		inner join dbo.PREM_ADJ_PGM_SETUP aps on aps.prem_adj_pgm_setup_id = pas.prem_adj_pgm_setup_id
		where
		pas.prem_adj_perd_id = @premium_adj_period_id
		and prem_adj_id = @premium_adjustment_id
		and aps.incld_ernd_retro_prem_ind = 0
		and aps.adj_parmet_typ_id = 401 -- Adjustment Parameter Type for LBA
		and pas.adj_parmet_typ_id = 401

		update dbo.PREM_ADJ_PARMET_SETUP 
		set tot_amt = los_base_asses_amt
		from 
		dbo.PREM_ADJ_PARMET_SETUP pas
		inner join dbo.PREM_ADJ_PGM_SETUP aps on aps.prem_adj_pgm_setup_id = pas.prem_adj_pgm_setup_id
		where
		pas.prem_adj_perd_id = @premium_adj_period_id
		and prem_adj_id = @premium_adjustment_id
		and aps.incld_ernd_retro_prem_ind = 1
		and aps.adj_parmet_typ_id = 401 -- Adjustment Parameter Type for LBA
		and pas.adj_parmet_typ_id = 401 -- Adjustment Parameter Type for LBA

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
			and h.adj_parmet_typ_id = 401 -- Adjustment Parameter Type for LBA
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

		if @debug = 1
		begin
		print '@pgm_setup_id:' + convert(varchar(20),@pgm_setup_id)
		print '---END pgm_setup while loop--------'
		end

		set @counter = @counter + 1
end --end of pgm_setup while loop

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
		rollback transaction ModAISCalcLBA
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

if object_id('ModAISCalcLBA') is not null
	print 'Created Procedure ModAISCalcLBA'
else
	print 'Failed Creating Procedure ModAISCalcLBA'
go

if object_id('ModAISCalcLBA') is not null
	grant exec on ModAISCalcLBA to public
go




