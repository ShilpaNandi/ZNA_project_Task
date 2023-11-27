
if exists (select 1 from sysobjects 
		where name = 'fn_ComputeLoss' and type = 'FN')
	drop function fn_ComputeLoss
GO

set ansi_nulls off
GO

set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_ComputeCHFAmount
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description: Computes CHF amount.
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_ComputeLoss]
   (
	@customer_id int,
    @premium_adj_prog_id int,
	@comm_agr_id int,
	@lba_adj_typ_id int,
	@incl_ibnr_ldf bit,
	@subj_paid_idnmty_amt  decimal(15, 8),
	@subj_resrv_idnmty_amt decimal(15, 8)
	)
returns decimal(15, 8)
--WITH SCHEMABINDING
as
begin
	declare @loss_amt decimal(15,8)
	
		if @lba_adj_typ_id = 187 -- LBA Adj Type: Incurred
		begin
			if @incl_ibnr_ldf = 1 --IBNR checked
			begin
				set @loss_amt = dbo.fn_ComputeLossIBNR_LDF(@lba_adj_typ_id , @comm_agr_id , @premium_adj_prog_id ,	@customer_id, @subj_paid_idnmty_amt , @subj_resrv_idnmty_amt )
			end --end: if @incl_ibnr_ldf = 1
			else --IBNR unchecked
			begin -- else: if @incl_ibnr_ldf = 1
				set @loss_amt = @subj_paid_idnmty_amt + @subj_resrv_idnmty_amt
			end --end: if @incl_ibnr_ldf = 0
		end -- end: if @lba_adj_typ_id = 187 
		else if @lba_adj_typ_id = 188 -- LBA Adj Type: Paid
		begin
			if @incl_ibnr_ldf = 1 --IBNR checked
			begin
				set @loss_amt = dbo.fn_ComputeLossIBNR_LDF(@lba_adj_typ_id , @comm_agr_id , @premium_adj_prog_id ,	@customer_id, @subj_paid_idnmty_amt , @subj_resrv_idnmty_amt )
			end --end: if @incl_ibnr_ldf = 1
			else --IBNR unchecked
			begin --else: if @incl_ibnr_ldf = 1
				set @loss_amt = @subj_paid_idnmty_amt 
				--set @amt_subj_lba_ft = @loss_amt 
			end --end:- else: to  if @incl_ibnr_ldf = 1
		end --end: if @lba_adj_typ_id = 188

   return @loss_amt
end


go

if object_id('fn_ComputeLoss') is not null
	print 'Created function fn_ComputeLoss'
else
	print 'Failed function Procedure fn_ComputeLoss'
go

if object_id('fn_ComputeLoss') is not null
	grant exec on fn_ComputeLoss to public
go


