
if exists (select 1 from sysobjects 
		where name = 'fn_ComputeLossIBNR_LDF' and type = 'FN')
	drop function fn_ComputeLossIBNR_LDF
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_ComputeLossIBNR_LDF
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description: This function computes the loss amount for IBNR and LDF.
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_ComputeLossIBNR_LDF]
   (
	@lba_adj_typ_id int,
	@com_agm_id int,
    @premium_adj_prog_id int,
	@customer_id int,
	@subj_paid_idnmty_amt decimal(15,2),
	@subj_resrv_idnmty_amt decimal(15,2)
	)
returns decimal(15, 8)
--WITH SCHEMABINDING
as
begin
	declare @loss_amt decimal(15,8),
			@ibnr_rt decimal(15,8),
			@ldf_rt decimal(15,8),
			@is_ibnr bit,
			@ldf_ibnr_step_ind bit,
			@months_to_val int
	
	select @ldf_ibnr_step_ind = los_dev_fctr_incur_but_not_rptd_step_ind  from dbo.COML_AGMT where coml_agmt_id = @com_agm_id and prem_adj_pgm_id = @premium_adj_prog_id and custmr_id = @customer_id
							if @ldf_ibnr_step_ind = 0 --not IBNR / LDF step
							begin

								--check IBNR or LDF factor in COMM AGREEMENT table (mutually exclusive, one or the other NOT both); use the factor from COMM AGREEMENT table
								select @ibnr_rt = incur_but_not_rptd_fctr_rt - 1  from dbo.COML_AGMT where coml_agmt_id = @com_agm_id and prem_adj_pgm_id = @premium_adj_prog_id and custmr_id = @customer_id
								if @ibnr_rt is null -- not IBNR; should be LDF
								begin
									set @is_ibnr = 0
								end
								else -- should be IBNR
								begin
									set @is_ibnr = 1
								end

								if @is_ibnr = 0 -- not IBNR; should be LDF
								begin
									select @ldf_rt = los_dev_fctr_rt - 1  from dbo.COML_AGMT where coml_agmt_id = @com_agm_id and prem_adj_pgm_id = @premium_adj_prog_id and custmr_id = @customer_id
									if @lba_adj_typ_id = 297 -- LBA Adj Type: Incurred
										set @loss_amt = (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt ) * @ldf_rt
									else if @lba_adj_typ_id = 298 -- LBA Adj Type: Paid
										set @loss_amt = (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt) * @ldf_rt

								end --end: @is_ibnr = 0
								else -- should be IBNR
								begin
									if @lba_adj_typ_id = 297 -- LBA Adj Type: Incurred
										set @loss_amt = (@subj_resrv_idnmty_amt  * @ibnr_rt) 
									else if @lba_adj_typ_id = 298 -- LBA Adj Type: Paid
										set @loss_amt = (@subj_resrv_idnmty_amt * @ibnr_rt)
									
									
								end --end:else to @is_ibnr = 0
							end --end:if @ldf_ibnr_step_ind = 0 
							else
							begin --LDF IBNR stepped (@ldf_ibnr_step_ind = 1)
								/****************************************************************
								* If LDF IBNR step indicator is true go to STEPPED FACTOR table, 
								* based on age of policy [from inception(program period start date) 
								* to valuation date for current adjustment]. 
								* Select stepped factors from STEPPED FACTOR table with matching 
								* 'months to val'. If actual age of policy in months more than any 
								* value in MON TO VAL use the highest MON TO VAL value in the 
								* STEPPED FACTOR table.
								*****************************************************************/

								select @months_to_val = datediff(mm,strt_dt, dateadd(d,1,nxt_valn_dt)) from dbo.PREM_ADJ_PGM where prem_adj_pgm_id = @premium_adj_prog_id
								select @ibnr_rt = incur_but_not_rptd_fctr_rt - 1 from dbo.STEPPED_FCTR where coml_agmt_id = @com_agm_id and prem_adj_pgm_id = @premium_adj_prog_id and custmr_id = @customer_id and mms_to_valn_nbr 
								= (select max( mms_to_valn_nbr ) from  dbo.STEPPED_FCTR where coml_agmt_id = @com_agm_id and prem_adj_pgm_id = @premium_adj_prog_id and custmr_id = @customer_id and mms_to_valn_nbr <= @months_to_val)

								if @ibnr_rt is null -- not IBNR; should be LDF
								begin
									set @is_ibnr = 0
								end
								else -- should be IBNR
								begin
									set @is_ibnr = 1
								end
						

								if @is_ibnr = 0 -- not IBNR; should be LDF
								begin
									select @ldf_rt = los_dev_fctr_rt - 1 from dbo.STEPPED_FCTR where coml_agmt_id = @com_agm_id and prem_adj_pgm_id = @premium_adj_prog_id and custmr_id = @customer_id and mms_to_valn_nbr 
									= (select max( mms_to_valn_nbr ) from  dbo.STEPPED_FCTR where coml_agmt_id = @com_agm_id and prem_adj_pgm_id = @premium_adj_prog_id and custmr_id = @customer_id and mms_to_valn_nbr <= @months_to_val)

									if @lba_adj_typ_id = 297 -- LBA Adj Type: Incurred
										set @loss_amt = (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt ) * @ldf_rt
									else if @lba_adj_typ_id = 298 -- LBA Adj Type: Paid
										set @loss_amt = (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt) * @ldf_rt

								end --end: @is_ibnr = 0
								else -- should be IBNR
								begin
									if @lba_adj_typ_id = 297 -- LBA Adj Type: Incurred
										set @loss_amt = (@subj_resrv_idnmty_amt  * @ibnr_rt) 
									else if @lba_adj_typ_id = 298 -- LBA Adj Type: Paid
										set @loss_amt = (@subj_resrv_idnmty_amt * @ibnr_rt)
									
									
								end --end:else to @is_ibnr = 0

							end --end:LDF IBNR stepped (@ldf_ibnr_step_ind = 1)

   return @loss_amt
end


go

if object_id('fn_ComputeLossIBNR_LDF') is not null
	print 'Created function fn_ComputeLossIBNR_LDF'
else
	print 'Failed Creating function fn_ComputeLossIBNR_LDF'
go

if object_id('fn_ComputeLossIBNR_LDF') is not null
	grant exec on fn_ComputeLossIBNR_LDF to public
go


