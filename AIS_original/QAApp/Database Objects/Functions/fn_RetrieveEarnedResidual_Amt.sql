
if exists (select 1 from sysobjects 
		where name = 'fn_RetrieveEarnedResidual_Amt' and type = 'FN')
	drop function fn_RetrieveEarnedResidual_Amt
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_RetrieveEarnedResidual_Amt
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description: This retrieves the Paid RML result amount from output detail table for the passed input parameters.
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_RetrieveEarnedResidual_Amt]
   (
	@premium_adj_period_id int,
	@premium_adjustment_id int,
	@customer_id int,
	@premium_adj_prog_id int,
	@coml_agmt_id int,
	@lob_id int,
	@state_id int,
	@subj_paid_idnmty_amt  decimal(15,2), 
	@subj_resrv_idnmty_amt  decimal(15,2), 
	@subj_paid_exps_amt  decimal(15,2), 
	@subj_resrv_exps_amt  decimal(15,2),
	@lcf  decimal(15,8),
	@lcf_result  decimal(15,2),
	@lba  decimal(15,2),
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



		exec @rml_fctr_rt = dbo.fn_RetrieveRML
			@p_cust_id = @customer_id,
			@p_prem_adj_prog_id = @premium_adj_prog_id,
			@p_comm_agr_id = @coml_agmt_id,
			@p_lob_id =@lob_id,
			@p_state_id =@state_id

		--print 'inside RML -  sp:-> @rml_fctr_rt: ' + convert(varchar(20), @rml_fctr_rt)
		set @rml_fctr_rt = isnull(@rml_fctr_rt,1)

		--Earned Residual = =((SPE+SRE)+LBA+LCF_RESULT)*RESIDUAL FACTOR
		set @curr_rml_amount =  ((@subj_paid_exps_amt + @subj_resrv_exps_amt)+ @lba + @lcf_result) * (@rml_fctr_rt - 1)

		set @curr_rml_amount = isnull(@curr_rml_amount,0)
		

	return @curr_rml_amount
end


go

if object_id('fn_RetrieveEarnedResidual_Amt') is not null
	print 'Created function fn_RetrieveEarnedResidual_Amt'
else
	print 'Failed Creating function fn_RetrieveEarnedResidual_Amt'
go

if object_id('fn_RetrieveEarnedResidual_Amt') is not null
	grant exec on fn_RetrieveEarnedResidual_Amt to public
go


