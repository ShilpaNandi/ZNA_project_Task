if exists (select 1 from sysobjects 
		where name = 'fn_RetrieveLBA_Fnl_Overrid_Amt' and type = 'FN')
	drop function fn_RetrieveLBA_Fnl_Overrid_Amt
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_RetrieveLBA_Fnl_Overrid_Amt
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
Create function [dbo].[fn_RetrieveLBA_Fnl_Overrid_Amt]
   (
	@p_cust_id int,
	@p_prem_adj_prog_id int,
	@p_comm_agr_id int,
	@p_state_id int
	)
returns decimal(15, 2)
as
begin
	declare @fnl_overrid_amt decimal(15,2),
			@count int

	select @count = count(*) 
	from dbo.PREM_ADJ_PGM_SETUP s
	inner join PREM_ADJ_PGM_SETUP_POL ap on (s.custmr_id = ap.custmr_id ) and (s.prem_adj_pgm_id = ap.prem_adj_pgm_id) and (s.prem_adj_pgm_setup_id = ap.prem_adj_pgm_setup_id)
	inner join PREM_ADJ_PGM_DTL ad on  (ap.prem_adj_pgm_setup_id = ad.prem_adj_pgm_setup_id ) and (s.custmr_id = ad.custmr_id ) and (s.prem_adj_pgm_id = ad.prem_adj_pgm_id)
	where 
	s.custmr_id = @p_cust_id 
	and s.prem_adj_pgm_id = @p_prem_adj_prog_id
	and ap.coml_agmt_id = @p_comm_agr_id
	and ad.st_id = @p_state_id
	and s.actv_ind = 1
	and ad.actv_ind  = 1 
	and s.adj_parmet_typ_id = 401 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> LBA

	if @count = 0
		set @p_state_id = 3 --Look up value for 'All Other' state

	select @fnl_overrid_amt = ad.fnl_overrid_amt 
	from dbo.PREM_ADJ_PGM_SETUP s
	inner join PREM_ADJ_PGM_SETUP_POL ap on (s.custmr_id = ap.custmr_id ) and (s.prem_adj_pgm_id = ap.prem_adj_pgm_id) and (s.prem_adj_pgm_setup_id = ap.prem_adj_pgm_setup_id)
	inner join PREM_ADJ_PGM_DTL ad on  (ap.prem_adj_pgm_setup_id = ad.prem_adj_pgm_setup_id ) and (s.custmr_id = ad.custmr_id ) and (s.prem_adj_pgm_id = ad.prem_adj_pgm_id)
	where 
	s.custmr_id = @p_cust_id 
	and s.prem_adj_pgm_id = @p_prem_adj_prog_id
	and ap.coml_agmt_id = @p_comm_agr_id
	and ad.st_id = @p_state_id
	and s.actv_ind = 1
	and ad.actv_ind  = 1 
	and s.adj_parmet_typ_id = 401 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> LBA

	set @fnl_overrid_amt = isnull(@fnl_overrid_amt,0)

   return @fnl_overrid_amt
end

go

if object_id('fn_RetrieveLBA_Fnl_Overrid_Amt') is not null
	print 'Created function fn_RetrieveLBA_Fnl_Overrid_Amt'
else
	print 'Failed Creating function fn_RetrieveLBA_Fnl_Overrid_Amt'
go

if object_id('fn_RetrieveLBA_Fnl_Overrid_Amt') is not null
	grant exec on fn_RetrieveLBA_Fnl_Overrid_Amt to public
go

