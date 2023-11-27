
if exists (select 1 from sysobjects 
		where name = 'fn_GetFactorsIBNR_LDF_For_ILRF' and type = 'FN')
	drop function fn_GetFactorsIBNR_LDF_For_ILRF
GO
set ansi_nulls off
GO
set quoted_identifier on
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetFactorsIBNR_LDF_For_ILRF
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Prabal Dhar 
-----
-----	Description: Retrieve IBNR and LDF factors for ILRF calculations.
-----
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification

---------------------------------------------------------------------
create function [dbo].[fn_GetFactorsIBNR_LDF_For_ILRF]
   (
	@p_com_agm_id int,
    @p_premium_adj_prog_id int,
	@p_customer_id int,
	@p_is_ibnr bit
	)
returns decimal(15, 8)
as
begin
	declare @factor decimal(15,8),
			@ibnr_rt decimal(15,8),
			@ldf_rt decimal(15,8),
			@is_ibnr bit,
			@ldf_ibnr_step_ind bit,
			@months_to_val int,
			@prem_non_prem_cd char(2)
	
	select 
	@ldf_ibnr_step_ind = los_dev_fctr_incur_but_not_rptd_step_ind  
	from dbo.COML_AGMT 
	where coml_agmt_id = @p_com_agm_id 
	and prem_adj_pgm_id = @p_premium_adj_prog_id 
	and custmr_id = @p_customer_id

	if @ldf_ibnr_step_ind = 0 --not IBNR / LDF step
	begin

		--check IBNR or LDF factor in COMM AGREEMENT table (mutually exclusive, one or the other NOT both); use the factor from COMM AGREEMENT table
		select 
		@ibnr_rt = incur_but_not_rptd_fctr_rt  
		from dbo.COML_AGMT 
		where coml_agmt_id = @p_com_agm_id 
		and prem_adj_pgm_id = @p_premium_adj_prog_id 
		and custmr_id = @p_customer_id

		--if @ibnr_rt is null -- not IBNR; should be LDF
		--begin
			--set @is_ibnr = 0
		--end
		--else -- should be IBNR
		--begin
			--set @is_ibnr = 1
		--end

		if @p_is_ibnr = 0 -- not IBNR; should be LDF
		begin
			select 
			@ldf_rt = los_dev_fctr_rt  
			from dbo.COML_AGMT 
			where coml_agmt_id = @p_com_agm_id 
			and prem_adj_pgm_id = @p_premium_adj_prog_id 
			and custmr_id = @p_customer_id

			set @factor = @ldf_rt

		end --end: @is_ibnr = 0
		else -- should be IBNR
		begin
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
		select @prem_non_prem_cd=pap.prem_non_prem_cd from prem_adj pa
		inner join prem_adj_perd pap on pap.prem_adj_id=pa.prem_adj_id
		inner join prem_adj_pgm pg on pg.prem_adj_pgm_id=pap.prem_adj_pgm_id
		where (pa.valn_dt=pg.nxt_valn_dt or pa.valn_dt=pg.nxt_valn_dt_non_prem_dt) 
		and pa.adj_can_ind<>1 
		and pa.adj_void_ind<>1
		and pa.adj_rrsn_ind<>1
		and substring(isnull(pa.fnl_invc_nbr_txt,''),1,3)<>'RTV'
		and pa.adj_sts_typ_id=346
		and pg.prem_adj_pgm_id=@p_premium_adj_prog_id
		
		if(@prem_non_prem_cd='P' or @prem_non_prem_cd='B')
		begin
		select @months_to_val = case when day(strt_dt)>15 then datediff(mm,strt_dt, nxt_valn_dt) else datediff(mm,strt_dt, nxt_valn_dt)+1 end
        from dbo.PREM_ADJ_PGM where prem_adj_pgm_id = @p_premium_adj_prog_id
		end
		else
		begin
		select @months_to_val = case when day(strt_dt)>15 then datediff(mm,strt_dt, nxt_valn_dt_non_prem_dt) else datediff(mm,strt_dt, nxt_valn_dt_non_prem_dt)+1 end
        from dbo.PREM_ADJ_PGM where prem_adj_pgm_id = @p_premium_adj_prog_id
		end
		
		select 
		@ibnr_rt = incur_but_not_rptd_fctr_rt 
		from dbo.STEPPED_FCTR 
		where coml_agmt_id = @p_com_agm_id 
		and prem_adj_pgm_id = @p_premium_adj_prog_id 
		and custmr_id = @p_customer_id 
		and actv_ind = 1
		and mms_to_valn_nbr 
		=  (
				select max( mms_to_valn_nbr ) 
				from  dbo.STEPPED_FCTR 
				where coml_agmt_id = @p_com_agm_id 
				and prem_adj_pgm_id = @p_premium_adj_prog_id 
				and custmr_id = @p_customer_id 
				and mms_to_valn_nbr = @months_to_val
				and actv_ind = 1
			)

		--if @ibnr_rt is null -- not IBNR; should be LDF
		--begin
			--set @is_ibnr = 0
		--end
		--else -- should be IBNR
		--begin
			--set @is_ibnr = 1
		--end

		if @p_is_ibnr = 0 --LDF
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
					and mms_to_valn_nbr = @months_to_val
					and actv_ind = 1
				)
			
			set @factor = @ldf_rt

		end
		else
		begin --IBNR
			set @factor = @ibnr_rt
		end

	end --end:LDF IBNR stepped (@ldf_ibnr_step_ind = 1)

	set @factor = isnull(@factor,1)

   return @factor 
end


go

if object_id('fn_GetFactorsIBNR_LDF_For_ILRF') is not null
	print 'Created function fn_GetFactorsIBNR_LDF_For_ILRF'
else
	print 'Failed Creating function fn_GetFactorsIBNR_LDF_For_ILRF'
go

if object_id('fn_GetFactorsIBNR_LDF_For_ILRF') is not null
	grant exec on fn_GetFactorsIBNR_LDF_For_ILRF to public
go




