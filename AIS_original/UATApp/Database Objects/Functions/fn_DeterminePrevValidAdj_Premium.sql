
if exists (select 1 from sysobjects 
		where name = 'fn_DeterminePrevValidAdj_Premium' and type = 'FN')
	drop function fn_DeterminePrevValidAdj_Premium
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_DeterminePrevValidAdj_Premium
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description:	This function  determines the previous valid adjustment for ERP.
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_DeterminePrevValidAdj_Premium]
   (
		@current_premium_adjustment_id int,
		@customer_id int,
		@premium_adj_prog_id int
	)
returns int
as
begin
	declare @prev_valid_adj_id int

	/**************************
	* Determine previous valid adjustment
	**************************/
	select  @prev_valid_adj_id=max(pa.prem_adj_id)
	from dbo.PREM_ADJ pa
	inner join dbo.PREM_ADJ_STS ps on (pa.prem_adj_id = ps.prem_adj_id) and (pa.reg_custmr_id = ps.custmr_id)
	inner join dbo.PREM_ADJ_PERD prd on (pa.reg_custmr_id = prd.reg_custmr_id) and (pa.prem_adj_id = prd.prem_adj_id)
	where pa.valn_dt in
		(
			select max(valn_dt) 
			from dbo.PREM_ADJ adj
			inner join dbo.PREM_ADJ_RETRO op on /* (adj.reg_custmr_id = op.custmr_id) and */ (adj.prem_adj_id = op.prem_adj_id )
			where adj.valn_dt < (
								select 
								valn_dt 
								from PREM_ADJ 
								where prem_adj_id = @current_premium_adjustment_id
							)
			and op.custmr_id = @customer_id
			and op.prem_adj_pgm_id=@premium_adj_prog_id
			and adj.adj_sts_typ_id in (349,352)
			and adj.adj_can_ind<>1 
			and adj.adj_void_ind<>1
			
		)
	and ps.adj_sts_typ_id in (349,352) -- Lookup IDs for applicable statuses: Final Invoice and Transmitted
	and prd.prem_adj_pgm_id = @premium_adj_prog_id
	and prd.custmr_id = @customer_id 
    and pa.adj_can_ind<>1 
    and pa.adj_void_ind<>1
	
--	select @prev_valid_adj_id =  max(pa.prem_adj_id)
--	from dbo.PREM_ADJ pa
--	inner join dbo.PREM_ADJ_STS ps on (pa.prem_adj_id = ps.prem_adj_id) and (pa.reg_custmr_id = ps.custmr_id)
--	inner join dbo.PREM_ADJ_PERD prd on (pa.reg_custmr_id = prd.reg_custmr_id) and (pa.prem_adj_id = prd.prem_adj_id)
--	where pa.valn_dt in
--		(
--			select max(valn_dt) 
--			from dbo.PREM_ADJ adj
--			inner join dbo.PREM_ADJ_RETRO op on /* (adj.reg_custmr_id = op.custmr_id) and */ (adj.prem_adj_id = op.prem_adj_id )
--			where adj.valn_dt < (
--								select 
--								valn_dt 
--								from PREM_ADJ 
--								where prem_adj_id = @current_premium_adjustment_id
--							)
--			--and adj.reg_custmr_id = @customer_id
--		)
--	and ps.adj_sts_typ_id in (349,352) -- Lookup IDs for applicable statuses: Final Invoice and Transmitted
--	and prd.prem_adj_pgm_id = @premium_adj_prog_id
--	--and pa.reg_custmr_id = @customer_id
--	and prd.custmr_id = @customer_id
--	and pa.adj_can_ind<>1 
--    and pa.adj_void_ind<>1


   return @prev_valid_adj_id
end


go

if object_id('fn_DeterminePrevValidAdj_Premium') is not null
	print 'Created function fn_DeterminePrevValidAdj_Premium'
else
	print 'Failed Creating function fn_DeterminePrevValidAdj_Premium'
go

if object_id('fn_DeterminePrevValidAdj_Premium') is not null
	grant exec on fn_DeterminePrevValidAdj_Premium to public
go


