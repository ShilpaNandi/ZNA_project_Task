
if exists (select 1 from sysobjects 
		where name = 'fn_GetFactorLDF' and type = 'FN')
	drop function fn_GetFactorLDF
GO
set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetFactorLDF
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Mogali
-----
-----	Description:	Retrieve LDF factors.
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_GetFactorLDF]
   (
	@p_com_agm_id int,
    @p_premium_adj_prog_id int,
	@p_customer_id int
	)
returns decimal(15, 8)
as
begin

declare @factor decimal(15,8),
		@ibnr_rt decimal(15,8),
		@ldf_rt decimal(15,8),
		--@is_ibnr bit,
		@ldf_ibnr_step_ind bit,
		@months_to_val int,
		@is_ibnr bit
set @is_ibnr = 0
	
	select 
	@ldf_ibnr_step_ind = los_dev_fctr_incur_but_not_rptd_step_ind  
	from dbo.COML_AGMT 
	where coml_agmt_id = @p_com_agm_id 
	and prem_adj_pgm_id = @p_premium_adj_prog_id 
	and custmr_id = @p_customer_id

	if @ldf_ibnr_step_ind = 0 --not IBNR / LDF step
	begin

		--check IBNR or LDF factor in COMM AGREEMENT table (mutually exclusive, one or the other NOT both); use the factor from COMM AGREEMENT table

		if @is_ibnr = 0 -- not IBNR; should be LDF
		begin
			select 
			@ldf_rt = los_dev_fctr_rt  
			from dbo.COML_AGMT 
			where coml_agmt_id = @p_com_agm_id 
			and prem_adj_pgm_id = @p_premium_adj_prog_id 
			and custmr_id = @p_customer_id

			set @factor = @ldf_rt
		end --end: @is_ibnr = 0
		else 
		begin -- should be IBNR
			select 
			@ibnr_rt = incur_but_not_rptd_fctr_rt  
			from dbo.COML_AGMT 
			where coml_agmt_id = @p_com_agm_id 
			and prem_adj_pgm_id = @p_premium_adj_prog_id 
			and custmr_id = @p_customer_id

			set @factor = @ibnr_rt
		end --end:else to @is_ibnr = 0
	end --end:if @ldf_ibnr_step_ind = 0 
	else
	begin --LDF IBNR stepped (@ldf_ibnr_step_ind = 1)
		/****************************************************************
		* If LDF IBNR step indicator is true go to STEPPED FACTOR table, 
		* based on age of policy [from inception(program period start date) 
		* to valuation date for current adjustment]. 
		* Select stepped factors from STEPPED FACTOR table with matching 
		* 'months to val'. If actual age of policy in months more than any 
		* value in MON TO VAL use the highest MON TO VAL value in the 
		* STEPPED FACTOR table.
		*****************************************************************/

		select 
		@months_to_val = CASE WHEN DATEPART(day, strt_dt) < DATEPART(day, nxt_valn_dt) THEN DATEDIFF(month, strt_dt, nxt_valn_dt) + 1
                       ELSE DATEDIFF(month, strt_dt, nxt_valn_dt) END 
		from dbo.PREM_ADJ_PGM 
		where prem_adj_pgm_id = @p_premium_adj_prog_id

		
		if @is_ibnr = 0 --LDF
		begin
			select 
			@ldf_rt = los_dev_fctr_rt 
			from dbo.STEPPED_FCTR 
			where coml_agmt_id = @p_com_agm_id 
			and prem_adj_pgm_id = @p_premium_adj_prog_id 
			and custmr_id = @p_customer_id 
			and actv_ind = 1
			and mms_to_valn_nbr 
			=  (
					select 
					max( mms_to_valn_nbr ) 
					from  dbo.STEPPED_FCTR 
					where coml_agmt_id = @p_com_agm_id 
					and prem_adj_pgm_id = @p_premium_adj_prog_id 
					and custmr_id = @p_customer_id 
					and mms_to_valn_nbr <= @months_to_val
					and actv_ind = 1
				)
			
			set @factor = @ldf_rt

		end
		else
		begin --IBNR

			select 
			@ibnr_rt = incur_but_not_rptd_fctr_rt 
			from dbo.STEPPED_FCTR 
			where coml_agmt_id = @p_com_agm_id 
			and prem_adj_pgm_id = @p_premium_adj_prog_id 
			and custmr_id = @p_customer_id 
			and actv_ind = 1
			and mms_to_valn_nbr 
			=  (
					select 
					max( mms_to_valn_nbr ) 
					from  dbo.STEPPED_FCTR 
					where coml_agmt_id = @p_com_agm_id 
					and prem_adj_pgm_id = @p_premium_adj_prog_id 
					and custmr_id = @p_customer_id 
					and mms_to_valn_nbr <= @months_to_val
					and actv_ind = 1
				)

			set @factor = @ibnr_rt
		end

	end --end:LDF IBNR stepped (@ldf_ibnr_step_ind = 1)

	--set @factor = isnull(@factor,1)

   return @factor 
end


go

if object_id('fn_GetFactorLDF') is not null
	print 'Created function fn_GetFactorLDF'
else
	print 'Failed Creating function fn_GetFactorLDF'
go

if object_id('fn_GetFactorLDF') is not null
	grant exec on fn_GetFactorLDF to public
go




