
if exists (select 1 from sysobjects 
		where name = 'ModAISCalcSubordProcs' and type = 'P')
	drop procedure ModAISCalcSubordProcs
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcSubordProcs
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used for invoking subordinate stored procs.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------

create procedure [dbo].[ModAISCalcSubordProcs] 
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@premium_adj_prog_id int,
@re_calc bit,
@create_user_id int
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
		@adj_tot_paid_los_bil_amt decimal(15,8),
		@div_by int,
		@plb_months int,
		@cnt_prev_adjs int,
		@prem_adj_parmet_setup_id int,
		@prem_adj_pgm_setup_id int,
		@trancount int


set @trancount = @@trancount

if @trancount = 0 
	begin
	    begin transaction 
	end


begin try

	exec dbo.ModAISCalcLSIPaidLossBilling
	@premium_adjustment_id = @premium_adjustment_id,
	@customer_id = @customer_id,
	@premium_adj_prog_id = @premium_adj_prog_id,
	@create_user_id = @create_user_id

	exec dbo.ModAISCalcCHF --AddPremAdjCHF 
	@premium_adj_period_id = @premium_adj_period_id,
	@premium_adjustment_id = @premium_adjustment_id,
	@customer_id = @customer_id,
	@premium_adj_prog_id = @premium_adj_prog_id,
	@updt_user_id = @create_user_id,
	@create_user_id = @create_user_id

	exec dbo.ModAISCalcLBA 
	@premium_adj_period_id = @premium_adj_period_id,
	@premium_adjustment_id = @premium_adjustment_id,
	@customer_id = @customer_id,
	@premium_adj_prog_id = @premium_adj_prog_id,
	@create_user_id = @create_user_id

	exec dbo.ModAISCalcILRF 
	@premium_adj_period_id = @premium_adj_period_id,
	@premium_adjustment_id = @premium_adjustment_id,
	@customer_id = @customer_id,
	@delete_ilrf=1,
	@premium_adj_prog_id = @premium_adj_prog_id,
	@create_user_id = @create_user_id

	exec ModAISCalcEscrow 
	@premium_adj_period_id = @premium_adj_period_id,
	@premium_adjustment_id = @premium_adjustment_id,
	@customer_id = @customer_id,
	@premium_adj_prog_id = @premium_adj_prog_id,
	@create_user_id = @create_user_id

	exec [dbo].[ModAISCalcERP] 
	@premium_adj_period_id = @premium_adj_period_id,
	@premium_adjustment_id = @premium_adjustment_id,
	@customer_id = @customer_id,
	@premium_adj_prog_id = @premium_adj_prog_id,
	@create_user_id = @create_user_id


	if @trancount = 0
		commit transaction

end try
begin catch

	if @trancount = 0  
	begin
		rollback transaction
	end

	declare @err_msg varchar(500)
	select @err_msg = error_message()

	insert into [dbo].[APLCTN_STS_LOG]
   (
		[src_txt]
	   ,[sev_cd]
	   ,[shrt_desc_txt]
	   ,[full_desc_txt]
	   ,[crte_user_id]
	)
	values
    (
		'AIS Calculation Engine'
       ,'Inf'
       ,'Calculation error'
       ,'Error encountered during calculation of adjustment number: ' 
			+ convert(varchar(20),isnull(@premium_adjustment_id,0)) 
			+ ' for program number: ' 
			+ convert(varchar(20),isnull(@premium_adj_prog_id,0)) 
			+ ' associated with customer number: ' 
			+ convert(varchar(20),isnull(@customer_id,0))
			+ '. Error message: '
			+ @err_msg
       ,@create_user_id
	)


	select 
    error_number() AS ErrorNumber,
    error_severity() AS ErrorSeverity,
    error_state() as ErrorState,
    error_procedure() as ErrorProcedure,
    error_line() as ErrorLine,
    error_message() as ErrorMessage

end catch


end



go

if object_id('ModAISCalcSubordProcs') is not null
	print 'Created Procedure ModAISCalcSubordProcs'
else
	print 'Failed Creating Procedure ModAISCalcSubordProcs'
go

if object_id('ModAISCalcSubordProcs') is not null
	grant exec on ModAISCalcSubordProcs to public
go


/*
---------Rounding-------------
end --end of cursor recalc_cur / while loop
			close recalc_cur
			deallocate recalc_cur

		end --end of: if ( (@recalc_lcf = 1) or (@recalc_minmax_for_paid = 1) )





		/*******************************
		* Rounding logic
		********************************/

		--Round Retro fields
		update dbo.[PREM_ADJ_RETRO_DTL]  WITH (ROWLOCK)
		   set  clm_hndl_fee_amt = round(clm_hndl_fee_amt,0) ,
--				basic_amt  = round(basic_amt,0) ,
--				exc_los_prem_amt = round(exc_los_prem_amt,0) ,
				los_base_asessment_amt = round(los_base_asessment_amt,0) ,
--				non_conv_fee_amt = round(non_conv_fee_amt,0) ,
				prem_tax_amt = round(prem_tax_amt,0) ,
--				othr_amt = round(othr_amt,0) ,
				los_conv_fctr_amt = round(los_conv_fctr_amt,0) ,
--				incur_ernd_retro_prem_amt = round(incur_ernd_retro_prem_amt,0),
--				adj_incur_ernd_retro_prem_amt = round(adj_incur_ernd_retro_prem_amt,0),
--				paid_ernd_retro_prem_amt = round(paid_ernd_retro_prem_amt,0),
--				adj_paid_ernd_retro_prem_amt = round(adj_paid_ernd_retro_prem_amt,0),
--				cash_flw_ben_amt = round(cash_flw_ben_amt,0),
				los_dev_resrv_amt = round(los_dev_resrv_amt,0),
--				adj_dedtbl_wrk_comp_los_amt = round(adj_dedtbl_wrk_comp_los_amt,0),
--				prior_cash_flw_ben_amt=round(prior_cash_flw_ben_amt,0),
--				std_subj_prem_amt = round(std_subj_prem_amt,0),
				prem_asses_amt = round(prem_asses_amt,0) ,
--				ernd_retro_prem_amt=round(ernd_retro_prem_amt,0),
--				ky_or_tax_asses_amt=round(ky_or_tax_asses_amt,0),
--				ky_or_prev_tax_asses_amt=round(ky_or_prev_tax_asses_amt,0),
--				rsdl_mkt_load_prev_amt=round(rsdl_mkt_load_prev_amt,0),
--				ky_or_tot_due_amt=round(ky_or_tot_due_amt,0),
--				biled_ernd_retro_prem_amt=round(biled_ernd_retro_prem_amt,0),
--				adj_cash_flw_ben_amt=round(adj_cash_flw_ben_amt,0),
--				prev_biled_ernd_retro_prem_amt=round(prev_biled_ernd_retro_prem_amt,0),
--				prev_std_subj_prem_amt=round(prev_std_subj_prem_amt,0),
				rsdl_mkt_load_ernd_amt = round(rsdl_mkt_load_tot_amt,0) 
--				rsdl_mkt_load_tot_amt = round(rsdl_mkt_load_tot_amt,0),
--				rsdl_mkt_load_paid_amt = round(rsdl_mkt_load_paid_amt,0)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id

			--Re-calculate adj_incur_erp_amt
			update dbo.[PREM_ADJ_RETRO_DTL]  WITH (ROWLOCK)
		   set  adj_incur_ernd_retro_prem_amt = 
				
				case when ( (ca.adj_typ_id = 66) or (ca.adj_typ_id = 73) or (ca.adj_typ_id = 67) or (ca.adj_typ_id = 72)) 
					then 0
				else
					 isnull(subj_paid_idnmty_amt,0)
				end  +
				isnull(subj_paid_exps_amt ,0) +
				case when ( (ca.adj_typ_id = 66) or (ca.adj_typ_id = 73) or (ca.adj_typ_id = 67) or (ca.adj_typ_id = 72)) 
					then 0
				else
					isnull(subj_resrv_idnmty_amt ,0)
				end  +
				isnull(subj_resrv_exps_amt ,0) +
				isnull(basic_amt,0)  +
				isnull(clm_hndl_fee_amt,0) +
				isnull(los_base_asessment_amt,0) +
				isnull(non_conv_fee_amt,0) +
				isnull(prem_tax_amt,0) +
				isnull(othr_amt,0) +
				isnull(los_conv_fctr_amt,0) +
				isnull(exc_los_prem_amt,0) +
				isnull(los_dev_resrv_amt,0)+
				case when ( (ca.adj_typ_id = 66) or (ca.adj_typ_id = 73)) 
					then isnull(rsdl_mkt_load_tot_amt,0)
				else
					0
				end + 
				isnull(prem_asses_amt,0)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id



		--End Rounding changes




		update dbo.PREM_ADJ_RETRO_DTL WITH (ROWLOCK)
		set biled_ernd_retro_prem_amt = adj_incur_ernd_retro_prem_amt
		where prem_adj_id = @premium_adjustment_id 
		and prem_adj_perd_id = @premium_adj_period_id 
		and biled_ernd_retro_prem_amt is null

		if(@paid_incur_type = 297) -- Incurred
		begin
			update dbo.PREM_ADJ_RETRO_DTL WITH (ROWLOCK)
			set cash_flw_ben_amt = null
			where prem_adj_id = @premium_adjustment_id 
			and prem_adj_perd_id = @premium_adj_period_id 
			and cash_flw_ben_amt = 0

			update dbo.PREM_ADJ_RETRO_DTL WITH (ROWLOCK)
			set adj_cash_flw_ben_amt = null
			where prem_adj_id = @premium_adjustment_id 
			and prem_adj_perd_id = @premium_adj_period_id 
			and adj_cash_flw_ben_amt = 0
		end




		/**************************************************
		* UPDATE PREM_ADJ_PERD TABLE WITH MIN / MAX INFO
		**************************************************/

------------------------------

--Adjustment numbering
						select 
						d.prem_adj_id,
						d.prem_adj_pgm_id,
						d.strt_dt,
						d.row_no,
						nl.adj_nbr_txt + ' Adjustment' as adj_text
						from
						(
							select 
							prd.prem_adj_id,pgm.prem_adj_pgm_id,pgm.strt_dt,dt.row_no 
							from dbo.PREM_ADJ_PERD prd
							inner join dbo.PREM_ADJ_PGM pgm on (prd.prem_adj_pgm_id = pgm.prem_adj_pgm_id) and (prd.custmr_id = pgm.custmr_id)
							inner join
							(
								select prd.prem_adj_id,pgm.strt_dt,row_number() over(order by pgm.strt_dt desc) as row_no 
								from dbo.PREM_ADJ_PERD prd
								inner join dbo.PREM_ADJ_PGM pgm on (prd.prem_adj_pgm_id = pgm.prem_adj_pgm_id) and (prd.custmr_id = pgm.custmr_id)
								where prd.prem_adj_id = 300000032--@prem_adj_id
								and pgm.actv_ind = 1
								group by prd.prem_adj_id,pgm.strt_dt
							) dt
							on (prd.prem_adj_id = dt.prem_adj_id) and (pgm.strt_dt = dt.strt_dt)
							where prd.prem_adj_id = 300000032--@prem_adj_id
							and pgm.actv_ind = 1
						) d
						inner join dbo.ADJ_NBR_LKUP nl on (d.row_no = nl.adj_numercal_nbr)
*/




