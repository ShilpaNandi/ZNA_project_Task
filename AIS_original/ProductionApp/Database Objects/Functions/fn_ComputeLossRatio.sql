
if exists (select 1 from sysobjects 
		where name = 'fn_ComputeLossRatio' and type = 'FN')
	drop function fn_ComputeLossRatio
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_ComputeLossRatio
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description:	This function  computes the ratio by loss.
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_ComputeLossRatio]
   (
		@customer_id int,
		@premium_adj_prog_id int,
		@coml_agmt_id int,
		@state_id int,
		@premium_adjustment_id int
	)
returns decimal(15,8)
as
begin
	declare @loss_amount decimal(15,2),
			@sum_loss_amount decimal(15,2),
			@prem_adj_valn_dt datetime
			
	select 
	@prem_adj_valn_dt = valn_dt
	from dbo.PREM_ADJ
	where prem_adj_id = @premium_adjustment_id

	select
	@loss_amount =
	sum
	(
	isnull(subj_paid_idnmty_amt,0) +
	isnull(subj_paid_exps_amt,0) +
	isnull(subj_resrv_idnmty_amt,0) +
	isnull(subj_resrv_exps_amt,0)
	)
	from
	dbo.ARMIS_LOS_POL
	where prem_adj_pgm_id = @premium_adj_prog_id
	and custmr_id = @customer_id
	and coml_agmt_id = @coml_agmt_id
	and st_id = @state_id
	and valn_dt=@prem_adj_valn_dt
	and actv_ind = 1  
	and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id))

	set @loss_amount = isnull(@loss_amount,0)


	select 
	@sum_loss_amount = 
	sum
	(
		isnull(al.subj_paid_idnmty_amt,0) +
		isnull(al.subj_paid_exps_amt,0) +
		isnull(al.subj_resrv_idnmty_amt,0) +
		isnull(al.subj_resrv_exps_amt,0) 
	)
	from
	dbo.ARMIS_LOS_POL al
	join 
		(
			select
			d.custmr_id,
			d.prem_adj_pgm_id,
			d.prem_adj_id,
			d.coml_agmt_id,
			d.st_id
			from dbo.PREM_ADJ_RETRO_DTL d
			where d.prem_adj_id = @premium_adjustment_id
			and d.prem_adj_pgm_id = @premium_adj_prog_id

	) as tm
	on al.prem_adj_pgm_id=tm.prem_adj_pgm_id 
	and al.coml_agmt_id=tm.coml_agmt_id
	and al.st_id=tm.st_id
	where al.prem_adj_pgm_id = @premium_adj_prog_id
	and al.custmr_id = @customer_id
	--and coml_agmt_id = @coml_agmt_id
	and al.valn_dt=@prem_adj_valn_dt
	and al.actv_ind = 1  
	and ((al.prem_adj_id is null) or (al.prem_adj_id = @premium_adjustment_id))
    

	set @sum_loss_amount = isnull(@sum_loss_amount,0)

------------------------------------------------------------------


   return case when isnull(@sum_loss_amount,0) <> 0 then (@loss_amount / @sum_loss_amount)*100 else 0 end
end


go

if object_id('fn_ComputeLossRatio') is not null
	print 'Created function fn_ComputeLossRatio'
else
	print 'Failed Creating function fn_ComputeLossRatio'
go

if object_id('fn_ComputeLossRatio') is not null
	grant exec on fn_ComputeLossRatio to public
go


