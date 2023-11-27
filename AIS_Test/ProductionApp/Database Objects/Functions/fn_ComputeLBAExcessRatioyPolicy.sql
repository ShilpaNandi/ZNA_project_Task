
if exists (select 1 from sysobjects 
		where name = 'fn_ComputeLBAExcessRatioyPolicy' and type = 'FN')
	drop function fn_ComputeLBAExcessRatioyPolicy
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_ComputeLBAExcessRatioyPolicy
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description:	This function  computes the ratio for LBA by which paid portion of excess to be split proportionately by policy in case LDF/IBNR limit indicator is checked.
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_ComputeLBAExcessRatioyPolicy]
   (
		@is_ibnr bit,
		@customer_id int,
		@premium_adj_prog_id int,
		@coml_agmt_id int,
		@state_id int
	)
returns decimal(15,8)
as
begin
	declare @sum_paid_portion_exc_loss decimal(15,2),
			@sum_paid_resv_portion_exc_loss decimal(15,2)

	if(@is_ibnr = 1) -- IBNR
	begin
		select @sum_paid_portion_exc_loss =
		sum(exc_resrvd_idnmty_amt)
		from dbo.ARMIS_LOS_POL 
		where 
		custmr_id = @customer_id
		and prem_adj_pgm_id = @premium_adj_prog_id
		and coml_agmt_id = @coml_agmt_id
		and st_id = @state_id
		group by 
		coml_agmt_id,
		st_id

		set @sum_paid_portion_exc_loss = isnull(@sum_paid_portion_exc_loss,0)

			  
		select @sum_paid_resv_portion_exc_loss =
		sum(exc_paid_idnmty_amt)+
		sum(exc_paid_exps_amt)+
		sum(exc_resrvd_idnmty_amt)+
		sum(exc_resrv_exps_amt) 
		from dbo.ARMIS_LOS_POL 
		where 
		custmr_id = @customer_id
		and prem_adj_pgm_id = @premium_adj_prog_id
		and coml_agmt_id = @coml_agmt_id
		and st_id = @state_id
		group by 
		coml_agmt_id,
		st_id

		set @sum_paid_resv_portion_exc_loss = isnull(@sum_paid_resv_portion_exc_loss,0)
	end
	else
	begin --LDF
		select @sum_paid_portion_exc_loss =
		sum(exc_paid_idnmty_amt)+
		sum(exc_resrvd_idnmty_amt)
		from dbo.ARMIS_LOS_POL 
		where 
		custmr_id = @customer_id
		and prem_adj_pgm_id = @premium_adj_prog_id
		and coml_agmt_id = @coml_agmt_id
		and st_id = @state_id
		group by 
		coml_agmt_id,
		st_id

		set @sum_paid_portion_exc_loss = isnull(@sum_paid_portion_exc_loss,0)

			  
		select @sum_paid_resv_portion_exc_loss =
		sum(exc_paid_idnmty_amt)+
		sum(exc_paid_exps_amt)+
		sum(exc_resrvd_idnmty_amt)+
		sum(exc_resrv_exps_amt) 
		from dbo.ARMIS_LOS_POL 
		where 
		custmr_id = @customer_id
		and prem_adj_pgm_id = @premium_adj_prog_id
		and coml_agmt_id = @coml_agmt_id
		and st_id = @state_id
		group by 
		coml_agmt_id,
		st_id

		set @sum_paid_resv_portion_exc_loss = isnull(@sum_paid_resv_portion_exc_loss,0)
	end

------------------------------------------------------------------


   return case when @sum_paid_resv_portion_exc_loss <> 0 then (@sum_paid_portion_exc_loss / @sum_paid_resv_portion_exc_loss) else 0 end
end


go

if object_id('fn_ComputeLBAExcessRatioyPolicy') is not null
	print 'Created function fn_ComputeLBAExcessRatioyPolicy'
else
	print 'Failed Creating function fn_ComputeLBAExcessRatioyPolicy'
go

if object_id('fn_ComputeLBAExcessRatioyPolicy') is not null
	grant exec on fn_ComputeLBAExcessRatioyPolicy to public
go


