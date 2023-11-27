
if exists (select 1 from sysobjects 
		where name = 'ModAISCalcKY_OR' and type = 'P')
	drop procedure ModAISCalcKY_OR
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcKY_OR
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to populate the output tables for KY & OR with calculation results.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------

create procedure [dbo].[ModAISCalcKY_OR] 
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@premium_adj_prog_id int,
@coml_agmt_id int,
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

declare @kyor_prev_bil_amt decimal(15,2),
		@cnt_prev_adjs int,
		@fctr_rt decimal(15,8),
		@kyor_amt decimal(15,2),
		@nyor_prev_bil_amt decimal(15,2),
		@std_sub_prem_amount decimal(15,2),
		@std_incur_erp_amt  decimal(15,2),
		@curr_kyor_amount decimal(15,2),
		@months_to_val int,
		@months_elapsed smallint,
		@first_adj_prem int,
		@prev_valid_adj_id int,
		@freq smallint,
		@next_val_date datetime,
		@trancount int,
		@err_message varchar(5000)


set @trancount = @@trancount
--print @trancount
if @trancount >= 1
    save transaction ModAISCalcKY_OR
else
    begin transaction


begin try

	--TODO: Handle All Other state
	select 
	@std_sub_prem_amount = isnull(prem_amt,0)
	from dbo.SUBJ_PREM_AUDT
	where custmr_id = @customer_id
	and prem_adj_pgm_id = @premium_adj_prog_id
	and coml_agmt_id = @coml_agmt_id
	and st_id = @state_id
	and actv_ind = 1

	--set @std_sub_prem_amount = isnull(@std_sub_prem_amount,0)	
	--the below code is to achieve "KY & OR taxes should NOT be calculated when there is loss but no "Sub Audit
	--Premium" for KY/OR. If this is true, no report should be generated."
	if (@std_sub_prem_amount is null) -- std_sub_prem_amount is NULL
		begin
			--set @err_message = 'KY_OR: KY & OR taxes should NOT be calculated when there is no "Sub Audit Premium" for KY/OR; Customer ID: ' + convert(varchar(20),@customer_id) + ';Program Period ID: '  + convert(varchar(20),@premium_adj_prog_id)
			rollback transaction ModAISCalcKY_OR
			--set @err_msg_op = @err_message
--			exec [dbo].[AddAPLCTN_STS_LOG] 
--				@premium_adjustment_id = @premium_adjustment_id,
--				@customer_id = @customer_id,
--				@premium_adj_prog_id = @premium_adj_prog_id,
--				@err_msg = @err_message,
--				@create_user_id = @create_user_id
			return
		end	
--	if @debug = 1
--	begin
--	print 'KY/OR - @std_sub_prem_amount: ' + convert(varchar(20), @std_sub_prem_amount)
--	end

	if(@state_id = 20) -- KY
	begin
		select 
		@fctr_rt = ky_fctr_rt
		from dbo.KY_OR_SETUP
		where eff_dt = 
			(
				select
				max(eff_dt)
				from dbo.KY_OR_SETUP
				where eff_dt <=
					(
					select
					pol_eff_dt
					from
					dbo.COML_AGMT
					where coml_agmt_id = @coml_agmt_id
					)
			)
	end
	else if (@state_id = 40) -- OR
	begin
		select 
		@fctr_rt = or_fctr_rt
		from dbo.KY_OR_SETUP
		where eff_dt = 
			(
				select
				max(eff_dt)
				from dbo.KY_OR_SETUP
				where eff_dt <=
					(
					select
					pol_eff_dt
					from
					dbo.COML_AGMT
					where coml_agmt_id = @coml_agmt_id
					)
			)
	end
--	if @debug = 1
--	begin
--	print 'inside KY/OR sp:-> @prem_adj_retro_det_id: ' + convert(varchar(20), @prem_adj_retro_det_id)
--	end

    --if factor values are not entered consider it as 1
    set @tm = isnull(@tm,1)
    set @lcf = isnull(@lcf,1)
	set @basic=isnull(@basic,0)

	--Changed the formulato match tax assesment amout with legacy KYOR Report(need to confirm with Anil)
	--set @curr_kyor_amount = (@incur_erp_amt  - @std_sub_prem_amount) * @fctr_rt

	--set @std_incur_erp_amt = ((@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt + @subj_paid_exps_amt + @subj_resrv_exps_amt)*@lcf + @basic)*@tm
	--@incur_erp_amt is placed with @std_incur_erp_amt in @curr_kyor_amount calculation
	set @std_incur_erp_amt = ((@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt + @subj_paid_exps_amt + @subj_resrv_exps_amt)*@lcf + @basic)*@tm
	set @curr_kyor_amount = (@std_incur_erp_amt  - @std_sub_prem_amount) * @fctr_rt

	/*******************************************
	* Give credits for previously billed amounts
	********************************************/

	--retrieve amounts from the previous adjustment

	exec @prev_valid_adj_id = [dbo].[fn_DeterminePrevValidAdj_Premium]
		@current_premium_adjustment_id = @premium_adjustment_id,
		@customer_id = @customer_id,
		@premium_adj_prog_id = @premium_adj_prog_id

	--TODO: evaluate prev NY/OR amount
--	if @debug = 1
--	begin
--	print 'KY/OR - @std_incur_erp_amt: ' + convert(varchar(20), @std_incur_erp_amt)
--	print 'KY/OR - @std_sub_prem_amount: ' + convert(varchar(20), @std_sub_prem_amount)
--	print 'KY/OR - @curr_kyor_amount: ' + convert(varchar(20), @curr_kyor_amount)
--	end

	update PREM_ADJ_RETRO_DTL WITH (ROWLOCK)
	set ernd_retro_prem_amt = @std_incur_erp_amt,
		std_subj_prem_amt = @std_sub_prem_amount,
		ky_or_tax_asses_amt = @curr_kyor_amount
	where prem_adj_retro_dtl_id = @prem_adj_retro_det_id

	update PREM_ADJ_RETRO_DTL WITH (ROWLOCK)
	set ky_or_prev_tax_asses_amt = src.ky_or_tax_asses_amt
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
		d.ky_or_tax_asses_amt
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
	set ky_or_tot_due_amt = round(isnull(ky_or_tax_asses_amt,0),0) - round(isnull(ky_or_prev_tax_asses_amt,0),0)
	where prem_adj_retro_dtl_id = @prem_adj_retro_det_id


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
		rollback transaction ModAISCalcKY_OR
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

if object_id('ModAISCalcKY_OR') is not null
	print 'Created Procedure ModAISCalcKY_OR'
else
	print 'Failed Creating Procedure ModAISCalcKY_OR'
go

if object_id('ModAISCalcKY_OR') is not null
	grant exec on ModAISCalcKY_OR to public
go




