
if exists (select 1 from sysobjects 
		where name = 'fn_ComputeERPRatioByProgPerdForPaidPols' and type = 'FN')
	drop function fn_ComputeERPRatioByProgPerdForPaidPols
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_ComputeERPRatioByProgPerdForPaidPols
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description:	This function  computes the ratio by which ERP amounts 
-----                   need to be split proportionately by program period for 
-----					policies with Paid adjustment types.
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_ComputeERPRatioByProgPerdForPaidPols]
   (
		@premium_adj_perd_id int,
		@premium_adj_id int,
		@customer_id int,
		@premium_adj_prog_id int,
		@coml_agmt_id int,
		@state_id int
	)
returns decimal(15,8)
as
begin
	declare @erp_amount decimal(15,2),
			@sum_erp_amount decimal(15,2)


	select @erp_amount = isnull(d.adj_incur_ernd_retro_prem_amt,0)
	from dbo.PREM_ADJ_RETRO_DTL d
	--inner join dbo.PREM_ADJ_RETRO h on (d.prem_adj_retro_id = h.prem_adj_retro_id) and (d.prem_adj_perd_id = h.prem_adj_perd_id) and (d.prem_adj_id = h.prem_adj_id) and (d.custmr_id = h.custmr_id)
	inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
	where d.prem_adj_perd_id = @premium_adj_perd_id
	and d.prem_adj_id = @premium_adj_id
	and d.custmr_id = @customer_id
	and d.coml_agmt_id = @coml_agmt_id
	and d.st_id = @state_id
	and ca.adj_typ_id in (63,65,66,67,70,71,72,73) -- Adjustment type IDs for Paid and Incurred types



	set @erp_amount = isnull(@erp_amount,0)

	select @sum_erp_amount = sum(isnull(d.adj_incur_ernd_retro_prem_amt,0)) from 
	dbo.PREM_ADJ_RETRO_DTL d
	inner join dbo.PREM_ADJ_RETRO h on (d.prem_adj_retro_id = h.prem_adj_retro_id) and (d.prem_adj_perd_id = h.prem_adj_perd_id) and (d.prem_adj_id = h.prem_adj_id) and (d.custmr_id = h.custmr_id)
	inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = h.coml_agmt_id) and (ca.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ca.custmr_id = h.custmr_id)
	where d.prem_adj_perd_id = @premium_adj_perd_id
	and d.prem_adj_id = @premium_adj_id
	and d.custmr_id = @customer_id
	and h.prem_adj_pgm_id = @premium_adj_prog_id
	and ca.adj_typ_id in (63,65,66,67,70,71,72,73) -- Adjustment type IDs for Paid and Incurred types


	set @sum_erp_amount = isnull(@sum_erp_amount,0)

------------------------------------------------------------------


   return case when @sum_erp_amount <> 0 then (@erp_amount / @sum_erp_amount)*100 else 0 end
end


go

if object_id('fn_ComputeERPRatioByProgPerdForPaidPols') is not null
	print 'Created function fn_ComputeERPRatioByProgPerdForPaidPols'
else
	print 'Failed Creating function fn_ComputeERPRatioByProgPerdForPaidPols'
go

if object_id('fn_ComputeERPRatioByProgPerdForPaidPols') is not null
	grant exec on fn_ComputeERPRatioByProgPerdForPaidPols to public
go


