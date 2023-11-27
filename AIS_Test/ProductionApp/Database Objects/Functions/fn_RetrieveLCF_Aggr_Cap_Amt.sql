if exists (select 1 from sysobjects 
		where name = 'fn_RetrieveLCF_Aggr_Cap_Amt' and type = 'FN')
	drop function fn_RetrieveLCF_Aggr_Cap_Amt
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_RetrieveLCF_Aggr_Cap_Amt
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		CSC
-----
-----	Description: This retrieves the LBA Overrid amount from setup table.
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
CREATE function [dbo].[fn_RetrieveLCF_Aggr_Cap_Amt]
   (
	@p_cust_id int,
	@p_prem_adj_prog_id int,
	@p_prem_adj_perd_id int,
	@p_prem_adj_id int,
	@p_coml_agmt_id int,
	@p_state_id int,
	@p_sum_lcf_amt decimal(15,2),
	@p_sum_lcf_amt_used decimal(15,2),
	@p_state_counter int
	)
returns decimal(15, 2)
as
begin
	declare @lcf_aggr_cap_state_amt decimal(15,2),
			@lcf_aggr_cap_set_pgm_amt decimal(15,2),
			@sum_lcf_aggr_amt decimal(15,2),
			@lcf_amt decimal(15,2),
			@count int

	select @lcf_aggr_cap_set_pgm_amt = isnull(los_conv_fctr_aggr_cap_amt,0) 
	from dbo.PREM_ADJ_PGM_SETUP 
	where 
	custmr_id = @p_cust_id 
	and prem_adj_pgm_id = @p_prem_adj_prog_id
	and actv_ind = 1
	and adj_parmet_typ_id = 402 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> LCF

	set @lcf_aggr_cap_set_pgm_amt = isnull(@lcf_aggr_cap_set_pgm_amt,0)

--	select @sum_lcf_amt=sum(los_conv_fctr_amt) from PREM_ADJ_PARMET_DTL
--	where prem_adj_perd_id=@p_prem_adj_perd_id 
--	and prem_adj_id=@p_prem_adj_id 
--	and custmr_id=@p_cust_id
--	and prem_adj_pgm_id=@p_prem_adj_prog_id


	select @lcf_amt=los_conv_fctr_amt from PREM_ADJ_PARMET_DTL
	where prem_adj_perd_id=@p_prem_adj_perd_id 
	and prem_adj_id=@p_prem_adj_id 
	and custmr_id=@p_cust_id
	and prem_adj_pgm_id=@p_prem_adj_prog_id
	and coml_agmt_id=@p_coml_agmt_id
	and st_id=@p_state_id

	
	set @lcf_aggr_cap_state_amt=@lcf_aggr_cap_set_pgm_amt*(@lcf_amt/@p_sum_lcf_amt)

	if(@p_state_counter=1)
	begin
	
	set @lcf_aggr_cap_state_amt=(@lcf_aggr_cap_set_pgm_amt-(@p_sum_lcf_amt_used+(@lcf_aggr_cap_set_pgm_amt*(@lcf_amt/@p_sum_lcf_amt))))+(@lcf_aggr_cap_set_pgm_amt*(@lcf_amt/@p_sum_lcf_amt))

	end

	set @lcf_aggr_cap_state_amt = isnull(@lcf_aggr_cap_state_amt,0)

   return @lcf_aggr_cap_state_amt
end


go

if object_id('fn_RetrieveLCF_Aggr_Cap_Amt') is not null
	print 'Created function fn_RetrieveLCF_Aggr_Cap_Amt'
else
	print 'Failed Creating function fn_RetrieveLCF_Aggr_Cap_Amt'
go

if object_id('fn_RetrieveLCF_Aggr_Cap_Amt') is not null
	grant exec on fn_RetrieveLCF_Aggr_Cap_Amt to public
go


