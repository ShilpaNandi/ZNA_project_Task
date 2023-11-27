if exists (select 1 from sysobjects 
		where name = 'ModAISCalcCHF' and type = 'P')
	drop procedure ModAISCalcCHF
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcCHF
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to insert CHF calculation results.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----               02/02/2009 venkat kolimi
-----               inserted prem_adj_pgm_id into dbo.[PREM_ADJ_PARMET_DTL] table as it is not null field
---------------------------------------------------------------------

create procedure [dbo].[ModAISCalcCHF] 
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@premium_adj_prog_id int,
@updt_user_id int,
@create_user_id int,
@err_msg_op varchar(1000) output
as

begin
	set nocount on

declare @iden int,
		@count int,
		@counter int,
		@outer_count int,
		@outer_counter int,
		@cnt_prev_adjs int,
		@prev_valid_adj_id int,
		@setup_id int,
		@dep_amt decimal(15,2),
		@chf_prev_bil_amt decimal(15,2),
		@months_to_val int,
		@months_elapsed smallint,
		@fst_adj_dt datetime,
		@next_val_date datetime,
		@first_adj int,
		@freq smallint,
		@pgm_setup_id int,
		@incl_in_erp bit,
		@prem_adj_valn_dt datetime,
		@pgm_period_valn_dt datetime,
		@err_message varchar(500),
		@trancount int


--Check if CHF calc needs to be performed for this adjustment
select 
@prem_adj_valn_dt = valn_dt
from dbo.PREM_ADJ
where prem_adj_id = @premium_adjustment_id

select 
@pgm_period_valn_dt = nxt_valn_dt_non_prem_dt
from dbo.PREM_ADJ_PGM
where prem_adj_pgm_id = @premium_adj_prog_id

print 'Before CHF valuation date validation'

if (@prem_adj_valn_dt <> @pgm_period_valn_dt)
begin
				set @err_message = 'CHF: Valuation date for this adjustment and the valuation date specified in the program period setup do not match' 
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

print 'CHF: valuation date validation PASSED; START OF CALC'


set @trancount = @@trancount;
--print @trancount
if @trancount >= 1
    save transaction ModAISCalcCHF
else
    begin transaction
	
begin try


/**************************
* Determine first adjustment
**************************/

exec @cnt_prev_adjs = [dbo].[fn_DetermineFirstAdjustment]
	@premium_adj_prog_id = @premium_adj_prog_id,
	@adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> CHF


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
and adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> CHF

set @outer_counter = 1
select @outer_count = count(*) from #pgm_setups

while @outer_counter <= @outer_count
begin

select 
@pgm_setup_id = pgm_setup_id,
@incl_in_erp = incl_in_erp
from #pgm_setups 
where id = @outer_counter

print' @pgm_setup_id:- ' + convert(varchar(20), @pgm_setup_id)  
print' @incl_in_erp:- ' + convert(varchar(20), @incl_in_erp)  

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
		--select
		--@fst_adj_dt = fst_adj_non_prem_dt,
		--@next_val_date = nxt_valn_dt_non_prem_dt 
		--from dbo.PREM_ADJ_PGM 
		--where prem_adj_pgm_id = @premium_adj_prog_id

		--if (@fst_adj_dt <> @next_val_date)
		--begin
			--set @err_message = 'CHF: First Adjustment Date(NP) is not equal to Next Valuation Date(NP)' 
			--+ ';customer ID: ' + convert(varchar(20),@customer_id) 
			--+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
			--rollback transaction ModAISCalcCHF
			--set @err_msg_op = @err_message
			--exec [dbo].[AddAPLCTN_STS_LOG] 
				--@premium_adjustment_id = @premium_adjustment_id,
				--@customer_id = @customer_id,
				--@premium_adj_prog_id = @premium_adj_prog_id,
				--@err_msg = @err_message,
				--@create_user_id = @create_user_id
		--	return
	--	end

	end
	else
	begin -- Subsequent adjustment / this is not initial adjustment
		select @months_elapsed = datediff(mm,prev_valn_dt_non_prem_dt,nxt_valn_dt_non_prem_dt), 
			   @freq = freq_non_prem_mms_cnt -- Non-premium frequency
		from dbo.PREM_ADJ_PGM 
		where prem_adj_pgm_id = @premium_adj_prog_id

		if @months_elapsed <> @freq
		begin
			set @err_message = 'CHF: Difference between Next Valuation Date(NP) and Previous Valuation Date(NP) is not consistent with frequency for CHF'
			+ ';customer ID: ' + convert(varchar(20),@customer_id) 
			+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
			rollback transaction ModAISCalcCHF
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
		set @err_message = 'CHF: Current date is less than the Next Valuation Date(NP)'
		+ ';customer ID: ' + convert(varchar(20),@customer_id) 
		+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
		rollback transaction ModAISCalcCHF
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
	print 'initial adjustment check'
		/*************************************************************
		* Additional check required for program period setup ->FIRST ADJ NON-PREMIUM 
		* before initiating LBA calc. Only if inception to VAL DATE is 
		* more than this value, intiate LBA calc.
		*************************************************************/
		--select
		--@fst_adj_dt = fst_adj_dt,
		--@next_val_date = nxt_valn_dt 
		--from dbo.PREM_ADJ_PGM 
		--where prem_adj_pgm_id = @premium_adj_prog_id

		--if (@fst_adj_dt <> @next_val_date)
		--begin
			--set @err_message = 'CHF: First Adjustment Date(P) is not equal to Next Valuation Date(P)' 
			--+ ';customer ID: ' + convert(varchar(20),@customer_id) 
			--+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
			--rollback transaction ModAISCalcCHF
			--set @err_msg_op = @err_message
			--exec [dbo].[AddAPLCTN_STS_LOG] 
				--@premium_adjustment_id = @premium_adjustment_id,
				--@customer_id = @customer_id,
				--@premium_adj_prog_id = @premium_adj_prog_id,
				--@err_msg = @err_message,
				--@create_user_id = @create_user_id
			--return
		--end

	end
	else
	begin -- Subsequent adjustment / this is not initial adjustment
		select @months_elapsed = datediff(mm,prev_valn_dt,nxt_valn_dt), 
			   @freq = adj_freq_mms_intvrl_cnt -- Premium frequency
		from dbo.PREM_ADJ_PGM 
		where prem_adj_pgm_id = @premium_adj_prog_id

		if @months_elapsed <> @freq
		begin
			set @err_message = 'CHF: Difference between Next Valuation Date(P) and Previous Valuation Date(P) is not consistent with frequency for CHF'
			+ ';customer ID: ' + convert(varchar(20),@customer_id) 
			+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
			rollback transaction ModAISCalcCHF
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
		set @err_message = 'CHF: Current date is less than the Next Valuation Date(P)'
		+ ';customer ID: ' + convert(varchar(20),@customer_id) 
		+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
		rollback transaction ModAISCalcCHF
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


		create table #t_adj_setup
		(
			[id] [int] identity(1,1),
			[prem_adj_perd_id] [int],
			[prem_adj_id] [int] ,
			[prem_adj_pgm_setup_id] [int],
			[prem_adj_pgm_id] [int],
			[custmr_id] [int] ,
			[prem_adj_cmmnt_id] [int] ,
			[tot_amt] [decimal](15, 2) ,
			[adj_parmet_typ_id] [int] ,
			[updt_user_id] [int] ,
			[updt_dt] [datetime] ,
			[crte_user_id] [int] ,
			[crte_dt] [datetime] 
		)

		insert into #t_adj_setup
        (
			[prem_adj_perd_id]
		   ,[prem_adj_id]
		   ,[prem_adj_pgm_setup_id]
		   ,[prem_adj_pgm_id]
		   ,[custmr_id]
		   ,[prem_adj_cmmnt_id]
		   ,[tot_amt]
		   ,[adj_parmet_typ_id]
		   ,[updt_user_id]
		   ,[updt_dt]
		   ,[crte_user_id]
		   ,[crte_dt]
		)
		select 
		@premium_adj_period_id,
		@premium_adjustment_id,
		s.prem_adj_pgm_setup_id,
		s.prem_adj_pgm_id,
		s.custmr_id,
		NULL,  
		sum(dbo.fn_ComputeCHFAmount(isnull(sdtl.clm_hndl_fee_clmt_nbr,0) , isnull(sdtl.clm_hndl_fee_clm_rt_nbr,0))) as chf_amt,
		398, -- Adjustment Parameter Type ID for CHF
		@updt_user_id,
		getdate(),
		@create_user_id,
		getdate()
		from dbo.PREM_ADJ_PGM_SETUP s  
		inner join dbo.PREM_ADJ_PGM_DTL sdtl on (s.prem_adj_pgm_setup_id = sdtl.prem_adj_pgm_setup_id) and (s.custmr_id = sdtl.custmr_id) and (s.prem_adj_pgm_id = sdtl.prem_adj_pgm_id)
		where s.custmr_id = @customer_id
		and s.prem_adj_pgm_id = @premium_adj_prog_id
		and s.actv_ind = 1 
		and sdtl.actv_ind = 1  
		and s.adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> CHF
		and s.prem_adj_pgm_setup_id = @pgm_setup_id
		group by s.custmr_id, s.prem_adj_pgm_id, s.prem_adj_pgm_setup_id --sdtl.prem_adj_pgm_dtl_id

		--select * FROM #t_adj_setup
		set @counter = 1

		select @count = count(*) from #t_adj_setup
		print @count

		while @counter <= @count
			begin
				print @counter
				select 
				@setup_id = prem_adj_pgm_setup_id 
				from #t_adj_setup 
				where id = @counter

				--insert into the header output table
				insert into dbo.[PREM_ADJ_PARMET_SETUP]
				(
					[prem_adj_perd_id]
				   ,[prem_adj_id]
				   ,[prem_adj_pgm_setup_id]
				   ,[prem_adj_pgm_id]
				   ,[custmr_id]
				   --,[prem_adj_cmmnt_id]
				   ,[tot_amt]
				   ,[adj_parmet_typ_id]
				   ,[updt_user_id]
				   ,[updt_dt]
				   ,[crte_user_id]
				   ,[crte_dt]
				)
				select
					[prem_adj_perd_id]
				   ,[prem_adj_id]
				   ,[prem_adj_pgm_setup_id]
				   ,[prem_adj_pgm_id]
				   ,[custmr_id]
				   --,[prem_adj_cmmnt_id]
				   ,[tot_amt]
				   ,[adj_parmet_typ_id]
				   ,[updt_user_id]
				   ,[updt_dt]
				   ,[crte_user_id]
				   ,[crte_dt]
				from #t_adj_setup where id = @counter



				select @iden = @@identity

				--insert into the details table
				-- may need tweaking to specify policy info
				insert into dbo.[PREM_ADJ_PARMET_DTL]
				(
					[prem_adj_parmet_setup_id]
					,[prem_adj_perd_id]
					,[prem_adj_id]
					,[custmr_id]
					 --added by venkat
					,[prem_adj_pgm_id]
					,[coml_agmt_id]
					,[st_id]
					,[clm_hndl_fee_los_typ_id]
					,[tot_amt]
					,[updt_user_id]
					,[updt_dt]
					,[crte_user_id]
					,[crte_dt]
				)
				select 
				@iden,
				@premium_adj_period_id,
				@premium_adjustment_id,
				@customer_id,
				--added by venkat
                sdtl.prem_adj_pgm_id,
				ap.coml_agmt_id,
				sdtl.st_id,
				sdtl.clm_hndl_fee_los_typ_id,
				dbo.fn_ComputeCHFAmount(isnull(sdtl.clm_hndl_fee_clmt_nbr,0) , isnull(sdtl.clm_hndl_fee_clm_rt_nbr,0)) as chf_computed_by_fn,
				@updt_user_id,
				getdate(),
				@create_user_id,
				getdate()
				from dbo.PREM_ADJ_PGM_SETUP s  
				inner join dbo.PREM_ADJ_PGM_DTL sdtl on (s.prem_adj_pgm_setup_id = sdtl.prem_adj_pgm_setup_id) and (s.custmr_id = sdtl.custmr_id) and (s.prem_adj_pgm_id = sdtl.prem_adj_pgm_id)
				inner join PREM_ADJ_PGM_SETUP_POL ap on (s.prem_adj_pgm_setup_id = ap.prem_adj_pgm_setup_id ) and  (s.prem_adj_pgm_id = ap.prem_adj_pgm_id)
				where s.custmr_id = @customer_id
				and s.prem_adj_pgm_id = @premium_adj_prog_id
				and s.actv_ind = 1 
				and sdtl.actv_ind = 1  
				and s.adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMET CHF
				and s.prem_adj_pgm_setup_id = @setup_id
				
				set @counter = @counter + 1
			end --while loop
		drop table #t_adj_setup
	


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
		and aps.adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMET CHF

		if (@cnt_prev_adjs = 0) -- No existing adjustments this is initial adjustment
		begin
			set @dep_amt = isnull(@dep_amt,0)
		end
		else
		begin -- This is not initial adjustment
			set @dep_amt = 0
		end

	
		--retrieve amounts from the previous adjustment
		exec @prev_valid_adj_id = [dbo].[fn_DeterminePrevValidAdj_NonPremium]
 			@current_premium_adjustment_id = @premium_adjustment_id,
			@customer_id = @customer_id,
			@premium_adj_prog_id = @premium_adj_prog_id,
			@adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMET CHF

		select @chf_prev_bil_amt = isnull(sum( d.tot_amt ),0) + isnull(@dep_amt,0) --isnull(stp.tot_amt,0) + isnull(@dep_amt,0)
		from 
		dbo.PREM_ADJ_PARMET_SETUP stp
		inner join dbo.PREM_ADJ_PARMET_DTL d on (d.prem_adj_parmet_setup_id = stp.prem_adj_parmet_setup_id) and (d.prem_adj_perd_id = stp.prem_adj_perd_id) and (d.prem_adj_id = stp.prem_adj_id)
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
		and stp.adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMET CHF
		and adp.incld_ernd_retro_prem_ind = 0 -- not included in ERP
		group by d.coml_agmt_id


		print '@chf_prev_bil_amt: ' + convert(varchar(20), @chf_prev_bil_amt)
		print '@dep_amt: ' + convert(varchar(20), @dep_amt)
--
--		update dbo.PREM_ADJ_PARMET_SETUP WITH (ROWLOCK)
--		set clm_hndl_fee_prev_biled_amt = isnull(@chf_prev_bil_amt,0),
--			--los_base_asses_depst_amt = isnull(@dep_amt,0),
--			tot_amt = tot_amt - isnull(@dep_amt,0) - isnull(@chf_prev_bil_amt,0)
--		from 
--		dbo.PREM_ADJ_PARMET_SETUP pas
--		inner join dbo.PREM_ADJ_PGM_SETUP aps on aps.prem_adj_pgm_setup_id = pas.prem_adj_pgm_setup_id
--		where
--		pas.prem_adj_perd_id = @premium_adj_period_id
--		and prem_adj_id = @premium_adjustment_id
--		and aps.incld_ernd_retro_prem_ind = 0
--		and aps.adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMET CHF
--		and pas.adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMET CHF

		print '@pgm_setup_id:' + convert(varchar(20),@pgm_setup_id)
		print '---END pgm_setup while loop--------'
		

		set @outer_counter = @outer_counter + 1
end --end of pgm_setup while loop


		update dbo.PREM_ADJ_PARMET_SETUP WITH (ROWLOCK)
		set clm_hndl_fee_prev_biled_amt = isnull(@chf_prev_bil_amt,0),
			--los_base_asses_depst_amt = isnull(@dep_amt,0),
			tot_amt = tot_amt - isnull(@dep_amt,0) - isnull(@chf_prev_bil_amt,0)
		from 
		dbo.PREM_ADJ_PARMET_SETUP pas
		inner join dbo.PREM_ADJ_PGM_SETUP aps on aps.prem_adj_pgm_setup_id = pas.prem_adj_pgm_setup_id
		where
		pas.prem_adj_perd_id = @premium_adj_period_id
		and prem_adj_id = @premium_adjustment_id
		and aps.incld_ernd_retro_prem_ind = 0
		and aps.adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMET CHF
		and pas.adj_parmet_typ_id = 398 -- Adj. Parameter type ID for ADJUSTMENT PARAMET CHF
	
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
		rollback transaction ModAISCalcCHF
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

if object_id('ModAISCalcCHF') is not null
	print 'Created Procedure ModAISCalcCHF'
else
	print 'Failed Creating Procedure ModAISCalcCHF'
go

if object_id('ModAISCalcCHF') is not null
	grant exec on ModAISCalcCHF to public
go





