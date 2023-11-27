
if exists (select 1 from sysobjects 
		where name = 'fn_RetrieveLBA_Amt' and type = 'FN')
	drop function fn_RetrieveLBA_Amt
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_RetrieveLBA_Amt
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description: This retrieves the LBA result amount from output detail table for the passed input parameters.
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_RetrieveLBA_Amt]
   (
	@p_cust_id int,
	@p_prem_adj_id int,
	@p_prem_adj_perd_id int,
	@p_comm_agr_id int,
	@p_state_id int
	)
returns decimal(15, 8)
as
begin
	declare @result decimal(15,8)

	select 
	@result = isnull(sum(d.los_base_asses_amt),0)
	from 
	dbo.PREM_ADJ_PARMET_SETUP h
	inner join dbo.PREM_ADJ_PARMET_DTL d on (h.prem_adj_parmet_setup_id = d.prem_adj_parmet_setup_id) and (h.prem_adj_perd_id = d.prem_adj_perd_id) and (h.prem_adj_id = d.prem_adj_id) and (h.custmr_id = d.custmr_id)
	inner join dbo.PREM_ADJ_PGM_SETUP ip on (ip.prem_adj_pgm_setup_id = h.prem_adj_pgm_setup_id) and (ip.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ip.custmr_id = h.custmr_id)
	where d.custmr_id = @p_cust_id
	and d.prem_adj_id = @p_prem_adj_id
	and d.prem_adj_perd_id = @p_prem_adj_perd_id
	and d.coml_agmt_id = @p_comm_agr_id
	and d.st_id = @p_state_id
	and h.adj_parmet_typ_id = 401 -- Adjustment parameter type for LBA
	and ip.adj_parmet_typ_id = 401
	and ip.incld_ernd_retro_prem_ind = 1
	and ip.actv_ind = 1

    return @result
end


go

if object_id('fn_RetrieveLBA_Amt') is not null
	print 'Created function fn_RetrieveLBA_Amt'
else
	print 'Failed Creating function fn_RetrieveLBA_Amt'
go

if object_id('fn_RetrieveLBA_Amt') is not null
	grant exec on fn_RetrieveLBA_Amt to public
go


