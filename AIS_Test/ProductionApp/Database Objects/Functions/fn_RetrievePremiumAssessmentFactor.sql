
if exists (select 1 from sysobjects 
		where name = 'fn_RetrievePremiumAssessmentFactor' and type = 'FN')
	drop function fn_RetrievePremiumAssessmentFactor
GO

set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_RetrievePremiumAssessmentFactor
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description: This retrieves the Premium Assessment factor from setup for the passed input parameters by policy/state.
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_RetrievePremiumAssessmentFactor]
   (
	@p_cust_id int,
	@p_prem_adj_prog_id int,
	@p_comm_agr_id int,
    --@p_lob_id int,
	@p_state_id int,
	@p_ret_null bit = 0
	)
returns decimal(15, 8)
as
begin
	declare @pa_factor decimal(15,8),
			@count int

	select @count = count(*) 
	from dbo.PREM_ADJ_PGM_SETUP s
	inner join PREM_ADJ_PGM_SETUP_POL ap on (s.custmr_id = ap.custmr_id ) and (s.prem_adj_pgm_id = ap.prem_adj_pgm_id) and (s.prem_adj_pgm_setup_id = ap.prem_adj_pgm_setup_id)
	inner join PREM_ADJ_PGM_DTL ad on  (ap.prem_adj_pgm_setup_id = ad.prem_adj_pgm_setup_id ) and (s.custmr_id = ad.custmr_id ) and (s.prem_adj_pgm_id = ad.prem_adj_pgm_id)
	where 
	s.custmr_id = @p_cust_id 
	and s.prem_adj_pgm_id = @p_prem_adj_prog_id
	and ap.coml_agmt_id = @p_comm_agr_id
	--and ad.ln_of_bsn_id = @p_lob_id
	and ad.st_id = @p_state_id
	and s.actv_ind = 1
	and ad.actv_ind  = 1 
	and s.adj_parmet_typ_id = 404 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> WA

	if @count = 0
		set @p_state_id = 3 --Look up value for 'All Other' state

	select @pa_factor = ad.prem_asses_rt
	from dbo.PREM_ADJ_PGM_SETUP s
	inner join PREM_ADJ_PGM_SETUP_POL ap on (s.custmr_id = ap.custmr_id ) and (s.prem_adj_pgm_id = ap.prem_adj_pgm_id) and (s.prem_adj_pgm_setup_id = ap.prem_adj_pgm_setup_id)
	inner join PREM_ADJ_PGM_DTL ad on  (ap.prem_adj_pgm_setup_id = ad.prem_adj_pgm_setup_id ) and (s.custmr_id = ad.custmr_id ) and (s.prem_adj_pgm_id = ad.prem_adj_pgm_id)
	where 
	s.custmr_id = @p_cust_id 
	and s.prem_adj_pgm_id = @p_prem_adj_prog_id
	and ap.coml_agmt_id = @p_comm_agr_id
	--and ad.ln_of_bsn_id = @p_lob_id
	and ad.st_id = @p_state_id
	and s.actv_ind = 1
	and ad.actv_ind  = 1 
	and s.adj_parmet_typ_id = 404 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> WA

	if @p_ret_null = 0
		set @pa_factor = isnull(@pa_factor,1)

-- The previous logic
--	set @pa_factor = isnull(@pa_factor,0)
--
	if(@pa_factor = 0)
		set @pa_factor = 1

   return @pa_factor
end


go

if object_id('fn_RetrievePremiumAssessmentFactor') is not null
	print 'Created function fn_RetrievePremiumAssessmentFactor'
else
	print 'Failed Creating function fn_RetrievePremiumAssessmentFactor'
go

if object_id('fn_RetrievePremiumAssessmentFactor') is not null
	grant exec on fn_RetrievePremiumAssessmentFactor to public
go


