
if exists (select 1 from sysobjects 
		where name = 'ModAISCalcRML' and type = 'P')
	drop procedure ModAISCalcRML
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcRML
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to populate the output tables for RML with calculation results.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------

create procedure [dbo].[ModAISCalcRML] 
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@premium_adj_prog_id int,
@coml_agmt_id int,
@lob_id int,
@state_id int,
@prem_adj_retro_det_id int,
@incur_erp_amt  decimal(15,2),
@subj_paid_idnmty_amt  decimal(15,2), 
@subj_resrv_idnmty_amt  decimal(15,2), 
@subj_paid_exps_amt  decimal(15,2), 
@subj_resrv_exps_amt  decimal(15,2),
@lcf  decimal(15,8),
@basic  decimal(15,2),
@tm  decimal(15,8),
@create_user_id int
as

begin
	set nocount on

declare @rml_prev_bil_amt decimal(15,2),
		@cnt_prev_adjs int,
		@rml_fctr_rt decimal(15,8),
		@rml_amt decimal(15,2),
		@nyor_prev_bil_amt decimal(15,2),
		@std_sub_prem_amount decimal(15,2),
		@std_incur_erp_amt  decimal(15,2),
		@curr_rml_amount decimal(15,2),
		@final_override_amt decimal(15,2),
		@sub_aud_prem_ratio decimal(15,8),
		@basic_pre_prop_amt decimal(15,2),
--		@months_to_val int,
--		@months_elapsed smallint,
--		@first_adj_prem int,
		@prev_valid_adj_id int,
		@freq smallint,
		@next_val_date datetime,
		@trancount int


set @trancount = @@trancount
--print @trancount
if @trancount = 0 
	begin
	    begin transaction 
	end


begin try

	select 
	@std_sub_prem_amount = isnull(prem_amt,0)
	from dbo.SUBJ_PREM_AUDT
	where custmr_id = @customer_id
	and prem_adj_pgm_id = @premium_adj_prog_id
	and coml_agmt_id = @coml_agmt_id
	and st_id = @state_id
	and actv_ind = 1

	set @std_sub_prem_amount = isnull(@std_sub_prem_amount,0)	
--	if @debug = 1
--	begin
--	print 'RML - @std_sub_prem_amount: ' + convert(varchar(20), @std_sub_prem_amount)
--	end

	exec @rml_fctr_rt = dbo.fn_RetrieveRML
		@p_cust_id = @customer_id,
		@p_prem_adj_prog_id = @premium_adj_prog_id,
		@p_comm_agr_id = @coml_agmt_id,
		@p_lob_id =@lob_id,
		@p_state_id =@state_id
--	if @debug = 1
--	begin
--	print 'inside RML -  sp:-> @rml_fctr_rt: ' + convert(varchar(20), @rml_fctr_rt)
--	end

	if @rml_fctr_rt is not null
	begin
		
		select
		@basic_pre_prop_amt = case when isnull(tot_agmt_amt,0) > isnull(tot_audt_amt,0) then tot_agmt_amt else tot_audt_amt end
		from dbo.PREM_ADJ_PGM_RETRO
		where custmr_id = @customer_id
		and prem_adj_pgm_id = @premium_adj_prog_id
		and retro_elemt_typ_id = 334 --Lookup Type: RETROSPECTIVE ELEMENT TYPE; lookup value: Basic
		and retro_adj_fctr_aplcbl_ind = 0
		and actv_ind = 1

		if(@basic_pre_prop_amt is not null)
		begin

			exec @sub_aud_prem_ratio = [dbo].[fn_ComputeSubAudPremiumRatio]
				@customer_id = @customer_id,
				@premium_adj_prog_id = @premium_adj_prog_id,
				@coml_agmt_id = @coml_agmt_id,
				@state_id = @state_id 
		end

		set @std_incur_erp_amt = ((@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt + @subj_paid_exps_amt + @subj_resrv_exps_amt)*@lcf + @basic)*@tm

		set @curr_rml_amount = @std_incur_erp_amt * (@rml_fctr_rt)

		select @final_override_amt = ad.fnl_overrid_amt
		from dbo.PREM_ADJ_PGM_SETUP s 
		inner join PREM_ADJ_PGM_DTL ad on  (s.prem_adj_pgm_setup_id = ad.prem_adj_pgm_setup_id ) and (s.prem_adj_pgm_id = ad.prem_adj_pgm_id)
		where s.custmr_id = @customer_id 
		and s.prem_adj_pgm_id = @premium_adj_prog_id
		and s.adj_parmet_typ_id = 403 -- RML
		and ad.ln_of_bsn_id = @lob_id
		and ad.st_id = @state_id
		and ad.actv_ind = 1 

		if (@final_override_amt is not null)
			set @curr_rml_amount = @final_override_amt

		/*******************************************
		* Give credits for previously billed amounts
		********************************************/


		exec @prev_valid_adj_id = [dbo].[fn_DeterminePrevValidAdj_Premium]
			@current_premium_adjustment_id = @premium_adjustment_id,
			@customer_id = @customer_id,
			@premium_adj_prog_id = @premium_adj_prog_id
--		if @debug = 1
--		begin
--		print 'RML -  @std_incur_erp_amt: ' + convert(varchar(20), @std_incur_erp_amt)
--		print 'RML -  @std_sub_prem_amount: ' + convert(varchar(20), @std_sub_prem_amount)
--		print 'RML -  @curr_rml_amount: ' + convert(varchar(20), @curr_rml_amount)
--		end

		update PREM_ADJ_RETRO_DTL WITH (ROWLOCK)
		set ernd_retro_prem_amt = @std_incur_erp_amt,
			std_subj_prem_amt = @std_sub_prem_amount,
			rsdl_mkt_load_basic_fctr_rt = @sub_aud_prem_ratio/100,
			rsdl_mkt_load_fctr_rt = @rml_fctr_rt,
			rsdl_mkt_load_ernd_amt = @curr_rml_amount
		where prem_adj_retro_dtl_id = @prem_adj_retro_det_id

		--retrieve amounts from the previous valid adjustment
		update PREM_ADJ_RETRO_DTL WITH (ROWLOCK)
		set rsdl_mkt_load_prev_amt = src.rsdl_mkt_load_ernd_amt
		from dbo.PREM_ADJ_RETRO_DTL dd
		inner join dbo.PREM_ADJ_RETRO dh on (dd.prem_adj_retro_id = dh.prem_adj_retro_id) and (dd.prem_adj_perd_id = dh.prem_adj_perd_id) and (dd.prem_adj_id = dh.prem_adj_id) and (dd.custmr_id = dh.custmr_id)
		join 
		(
			select
			h.custmr_id,
			h.prem_adj_pgm_id,
			h.prem_adj_id,
			h.coml_agmt_id,
			d.st_id,
			d.rsdl_mkt_load_ernd_amt
			from dbo.PREM_ADJ_RETRO_DTL d
			inner join dbo.PREM_ADJ_RETRO h on (d.prem_adj_retro_id = h.prem_adj_retro_id) and (d.prem_adj_perd_id = h.prem_adj_perd_id) and (d.prem_adj_id = h.prem_adj_id) and (d.custmr_id = h.custmr_id)
			where  d.prem_adj_id = @prev_valid_adj_id
			and d.custmr_id = @customer_id
			and h.prem_adj_pgm_id = @premium_adj_prog_id
			and d.st_id = @state_id	

		) as src
		on dh.coml_agmt_id = src.coml_agmt_id
		and dd.st_id = src.st_id
		and dh.prem_adj_pgm_id = src.prem_adj_pgm_id
		and dh.custmr_id = src.custmr_id
		where dd.prem_adj_perd_id = @premium_adj_period_id
		and dd.prem_adj_id = @premium_adjustment_id
		and dd.custmr_id = @customer_id
		and dh.prem_adj_pgm_id = @premium_adj_prog_id
		and dh.coml_agmt_id = @coml_agmt_id
		and dd.st_id = @state_id
		and dd.prem_adj_retro_dtl_id = @prem_adj_retro_det_id

		update PREM_ADJ_RETRO_DTL WITH (ROWLOCK)
		set rsdl_mkt_load_tot_amt = round(isnull(rsdl_mkt_load_ernd_amt,0),0) - round(isnull(rsdl_mkt_load_prev_amt,0),0)
		where prem_adj_retro_dtl_id = @prem_adj_retro_det_id
	end --end of: if @rml_fctr_rt is not null

	--print '@trancount: ' + convert(varchar(30),@trancount)
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

if object_id('ModAISCalcRML') is not null
	print 'Created Procedure ModAISCalcRML'
else
	print 'Failed Creating Procedure ModAISCalcRML'
go

if object_id('ModAISCalcRML') is not null
	grant exec on ModAISCalcRML to public
go




