
if exists (select 1 from sysobjects 
		where name = 'fn_RetrieveRML_Amt' and type = 'FN')
	drop function fn_RetrieveRML_Amt
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_RetrieveRML_Amt
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description: This retrieves the RML result amount from output detail table for the passed input parameters.
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_RetrieveRML_Amt]
   (
	@premium_adj_period_id int,
	@premium_adjustment_id int,
	@customer_id int,
	@premium_adj_prog_id int,
	@coml_agmt_id int,
	@lob_id int,
	@state_id int,
	--@prem_adj_retro_det_id int,
	--@incur_erp_amt  decimal(15,2),
	@subj_paid_idnmty_amt  decimal(15,2), 
	@subj_resrv_idnmty_amt  decimal(15,2), 
	@subj_paid_exps_amt  decimal(15,2), 
	@subj_resrv_exps_amt  decimal(15,2),
	@lcf  decimal(15,8),
	@basic  decimal(15,2),
	@tm  decimal(15,8),
	@create_user_id int
	)
returns decimal(15, 2)
as
begin
declare @rml_prev_bil_amt decimal(15,2),
		@cnt_prev_adjs int,
		@rml_fctr_rt decimal(15,8),
		@rml_amt decimal(15,2),
		@nyor_prev_bil_amt decimal(15,2),
		@std_sub_prem_amount decimal(15,2),
		@std_incur_erp_amt  decimal(15,2),
		@curr_rml_amount decimal(15,2),
		@sub_aud_prem_ratio decimal(15,8),
		@prev_valid_adj_id int,
		@freq smallint,
		@next_val_date datetime,
		@trancount int


		select 
		@std_sub_prem_amount = isnull(prem_amt,0)
		from dbo.SUBJ_PREM_AUDT
		where custmr_id = @customer_id
		and prem_adj_pgm_id = @premium_adj_prog_id
		and coml_agmt_id = @coml_agmt_id
		and st_id = @state_id
		and actv_ind = 1

		set @std_sub_prem_amount = isnull(@std_sub_prem_amount,0)	
		--print '@std_sub_prem_amount: ' + convert(varchar(20), @std_sub_prem_amount)

		exec @rml_fctr_rt = dbo.fn_RetrieveRML
			@p_cust_id = @customer_id,
			@p_prem_adj_prog_id = @premium_adj_prog_id,
			@p_comm_agr_id = @coml_agmt_id,
			@p_lob_id =@lob_id,
			@p_state_id =@state_id

		--print 'inside RML -  sp:-> @rml_fctr_rt: ' + convert(varchar(20), @rml_fctr_rt)

		if @rml_fctr_rt is not null
		begin

			exec @sub_aud_prem_ratio = [dbo].[fn_ComputeSubAudPremiumRatio]
				@customer_id = @customer_id,
				@premium_adj_prog_id = @premium_adj_prog_id,
				@coml_agmt_id = @coml_agmt_id,
				@state_id = @state_id 

			set @std_incur_erp_amt = ((@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt + @subj_paid_exps_amt + @subj_resrv_exps_amt)*@lcf + @basic)*@tm

			set @curr_rml_amount = @std_incur_erp_amt * @rml_fctr_rt
		end --end of: if @rml_fctr_rt is not null

		set @curr_rml_amount = isnull(@curr_rml_amount,0)
		

	return @curr_rml_amount
end


go

if object_id('fn_RetrieveRML_Amt') is not null
	print 'Created function fn_RetrieveRML_Amt'
else
	print 'Failed Creating function fn_RetrieveRML_Amt'
go

if object_id('fn_RetrieveRML_Amt') is not null
	grant exec on fn_RetrieveRML_Amt to public
go


