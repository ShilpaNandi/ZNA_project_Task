
if exists (select 1 from sysobjects 
		where name = 'fn_ComputePEOPayInRatio' and type = 'FN')
	drop function fn_ComputePEOPayInRatio
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_ComputePEOPayInRatio
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description:	This function  computes the ratio by which PEO Pay In amount defined at program
-----					period level need to be split proportionately by policy.
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_ComputePEOPayInRatio]
   (
		@premium_adj_perd_id int,
		@premium_adj_id int,
		@coml_agmt_id int
	)
returns decimal(15,8)
as
begin
	declare @dep_amount decimal(15,2),
			@sum_dep_amount decimal(15,2)


	select 
	@dep_amount = isnull(r.subj_depst_prem_amt,0) + isnull(r.non_subj_depst_prem_amt,0)
	from dbo.PREM_ADJ_RETRO r
	inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = r.coml_agmt_id)
	where prem_adj_id = @premium_adj_id
	and prem_adj_perd_id = @premium_adj_perd_id
	and r.coml_agmt_id = @coml_agmt_id
	--and adj_typ_id not in (63,70) -- Adjustment types for: 'Incurred DEP','Paid Loss DEP'
	and ca.actv_ind = 1

	set @dep_amount = isnull(@dep_amount,0)

	select  
	@sum_dep_amount = sum(isnull(r.subj_depst_prem_amt,0) + isnull(r.non_subj_depst_prem_amt,0))
	from dbo.PREM_ADJ_RETRO r
	inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = r.coml_agmt_id)
	where  prem_adj_id = @premium_adj_id
	and prem_adj_perd_id = @premium_adj_perd_id
	--and adj_typ_id not in (63,70) -- Adjustment types for: 'Incurred DEP','Paid Loss DEP'
	and ca.actv_ind = 1

	set @sum_dep_amount = isnull(@sum_dep_amount,0)


   return case when @sum_dep_amount <> 0 then (@dep_amount / @sum_dep_amount)*100 else 0 end
end


go

if object_id('fn_ComputePEOPayInRatio') is not null
	print 'Created function fn_ComputePEOPayInRatio'
else
	print 'Failed Creating function fn_ComputePEOPayInRatio'
go

if object_id('fn_ComputePEOPayInRatio') is not null
	grant exec on fn_ComputePEOPayInRatio to public
go


