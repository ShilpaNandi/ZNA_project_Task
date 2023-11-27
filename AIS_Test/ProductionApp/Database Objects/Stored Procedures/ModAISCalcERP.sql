
if exists (select 1 from sysobjects 
		where name = 'ModAISCalcERP' and type = 'P')
	drop procedure ModAISCalcERP
go
 
set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcERP
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure performs calculation of ERP using dynamic formulas. Populates the output tables for ERP with calculation results.
-----					
-----
-----	On Exit:	
-----			
-----
-----	Modified:	01/09/09	Prabal Dhar
-----			- Exclude non-ERP adjustment types

---------------------------------------------------------------------

create procedure [dbo].[ModAISCalcERP] 
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@premium_adj_prog_id int,
@create_user_id int,
@err_msg_op varchar(1000) output,
@debug bit = 0
as

begin
	set nocount on

declare	@lob varchar(10),
		@lob_id int,
		@com_agm_id int,
		@com_agm_audt_id int,
		@state_id int,
		@subj_paid_idnmty_amt decimal(15,2),
		@subj_paid_exps_amt decimal(15,2),
		@subj_resrv_idnmty_amt decimal(15,2),
		@subj_resrv_exps_amt decimal(15,2),
		@basic_pre_prop_amt decimal(15,2),
		@elp_pre_prop_amt decimal(15,2),
		@ncf_pre_prop_amt decimal(15,2),
		@other_pre_prop_amt decimal(15,2),
		@lcf_aggr_cap_set_amt decimal(15,2),
		@lcf_aggr_cap_set_pgm_amt decimal(15,2),
		@chf_amt decimal(15,2),
		@sub_aud_prem_ratio decimal(15,8),
		@peo_payin_ratio decimal(15,8),
		@loss_ratio decimal(15,8),
		@erp_ratio decimal(15,8),
		@erp_ratio_by_pp decimal(15,8),
		@cfb_ratio_by_pp decimal(15,8),
		@sum_incur_erp_amt decimal(15,2),
		@target_inc_erp_amt decimal(15,2),
		@target_cfb_amt decimal(15,2),
		@sum_cfb_amt decimal(15,2),
		@tm_ft decimal(15,8),
		@stmt nvarchar(max),
		@frmla nvarchar(1000),
		@frmla_id int,
		@sub_formulas varchar(2000),
		@sub_formulas_temp varchar(500),
		@lcf_sub_formula varchar(500),
		@ldf_sub_formula varchar(1000),
		@paid_erp_frmla nvarchar(1000),
		@comp_list varchar(750),
		@frmla_comp varchar(30),
		@identifier_pos smallint,
		@counter smallint,
		@decl_str varchar(1500),
		@init_str varchar(2000),
		@updt_stmt varchar(1000),
		@func_mod_str varchar(4000),
		@has_chf bit,
		@has_ldf bit,
		@has_lcf bit,
		@has_basic bit,
		@has_ncf bit,
		@has_oa bit,
		@recalc_lcf bit,
		@is_max_unlimited bit,
		@min_applies bit,
		@max_applies bit,
		@erp_applies bit,
		@perd_erp_min_amt varchar(16),
		@perd_erp_max_amt varchar(16),
		@perd_erp_amt varchar(16),
		@perd_code varchar(16),
		@recalc_minmax_for_paid bit,
		@next_val_dt datetime,
		@conversion_dt datetime,
		@prev_valid_adj_id int,
		@prev_valid_adj_perd_id int,
		@count smallint,
		@cnt_prev_adjs smallint,
		@minmax_incurred_erp_by_pp_excl_wcloss_amt decimal(15,2),
		@minmax_incurred_erp_by_pp_amt decimal(15,2),
		@minmax_paid_erp_by_pp_amt decimal(15,2),
		@minmax_cfb_by_pp_amt decimal(15,2),
		@minmax_lcf_amt decimal(15,2),
		@max_amt decimal(15,2),
		@min_amt decimal(15,2),
		@limited_by_max_min decimal(15,2),
		@ded_max_amt decimal(15,2),
		@ded_min_amt decimal(15,2),
		@peo_pay_in_amt decimal(15,2),
		@paid_incur_type int,
		@pgm_lkup_txt varchar(20),
		@pgm_type smallint,
		@pgm_type_id int,
		@peo_com_agm_id int,
		@is_not_master smallint,
		@is_peo bit,
		@prem_adj_valn_dt datetime,
		@pgm_period_valn_dt datetime,
		@next_val_date datetime,
		@err_message varchar(5000),
		@policy_str varchar(4000),
		@PolicyIDList varchar(4000),
		@cnt int,
		@tax_formula varchar(1000),
		@trancount int,
		@is_elp bit,
		@cnt_losses int,
		@cnt_policies int
		

--Check if ERP calc needs to be performed for this adjustment
select 
@prem_adj_valn_dt = valn_dt
from dbo.PREM_ADJ
where prem_adj_id = @premium_adjustment_id

select 
@pgm_period_valn_dt = nxt_valn_dt
from dbo.PREM_ADJ_PGM
where prem_adj_pgm_id = @premium_adj_prog_id
if @debug = 1
begin
print 'Before ERP valuation date validation'
end
if (@prem_adj_valn_dt <> @pgm_period_valn_dt)
	return
if @debug = 1
begin
print 'ERP: valuation date validation PASSED; START OF CALC'
end

set @trancount = @@trancount
--print @trancount
if @trancount >= 1
    save transaction ModAISCalcERP
else
    begin transaction


begin try

	select @next_val_date = nxt_valn_dt
	from dbo.PREM_ADJ_PGM 
	where prem_adj_pgm_id = @premium_adj_prog_id

	if(getdate() < @next_val_date)
	begin
		set @err_message = 'ERP: Current date is less than the Next Valuation Date for ERP calculation'
		+ ';customer ID: ' + convert(varchar(20),@customer_id) 
		+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
		rollback transaction ModAISCalcERP
			set @err_msg_op = @err_message
			exec [dbo].[AddAPLCTN_STS_LOG] 
				@premium_adjustment_id = @premium_adjustment_id,
				@customer_id = @customer_id,
				@premium_adj_prog_id = @premium_adj_prog_id,
				@err_msg = @err_message,
				@create_user_id = @create_user_id
		return
	end



	
	/***********************************************************************
	* Added Validation to verify whether active retro polcies present under
	the program period or not,if no active policies rollback the ERP calculations-Bug Fix 11271
	***********************************************************************/
	
	select @cnt_policies=count(*) 
	from dbo.COML_AGMT 
	where prem_adj_pgm_id = @premium_adj_prog_id
	and adj_typ_id not in (62,64,68,69,448)
	and actv_ind=1
	
	print @cnt_policies
	if(@cnt_policies=0)
	begin
		rollback transaction ModAISCalcERP
		return
	end
	
	
	/***********************************************************************
	* DECOMPOSE THE MASTER FORMULA INTO CONSTITUENTS ALONG WITH DECLARATIONS
	***********************************************************************/
	
	select
	@frmla_id = fml.mstr_ernd_retro_prem_frmla_id,
	@frmla = fml.ernd_retro_prem_frmla_two_txt
	from dbo.PREM_ADJ_PGM pgm
	inner join dbo.MSTR_ERND_RETRO_PREM_FRMLA fml on (pgm.mstr_ernd_retro_prem_frmla_id = fml.mstr_ernd_retro_prem_frmla_id)
	where --pgm.custmr_id = @customer_id and
	pgm.prem_adj_pgm_id = @premium_adj_prog_id
	and pgm.actv_ind = 1
	and fml.actv_ind = 1

	if @frmla_id is null
	begin
		select @cnt = count(*) 
		from coml_agmt 
		where prem_adj_pgm_id = @premium_adj_prog_id
		and adj_typ_id not in (62,64,68,69,448) and actv_ind=1 -- Exclude all non-ERP adjustment types
		if(@cnt > 0)
		begin
			set @err_message = 'ERP: Formula not specified; ERP calculations cannot be performed; Customer ID: ' + convert(varchar(20),@customer_id) + ';Program Period ID: '  + convert(varchar(20),@premium_adj_prog_id)
			rollback transaction ModAISCalcERP
			set @err_msg_op = @err_message
			exec [dbo].[AddAPLCTN_STS_LOG] 
				@premium_adjustment_id = @premium_adjustment_id,
				@customer_id = @customer_id,
				@premium_adj_prog_id = @premium_adj_prog_id,
				@err_msg = @err_message,
				@create_user_id = @create_user_id
			return
		end

	end
	if @debug = 1
	begin
	print '@frmla: ' + @frmla
	end

	select
	@is_peo = peo_ind
	from dbo.CUSTMR
	where custmr_id = @customer_id


	if (@is_peo = 1) -- PEO adjustment
	begin
		if not exists(select 1 from dbo.COML_AGMT where prem_adj_pgm_id = @premium_adj_prog_id and mstr_peo_pol_ind = 1)
		begin
			set @err_message = 'ERP: In case of PEO, at least one policy should be specified as the master policy; Customer ID: ' + convert(varchar(20),@customer_id) + ';Program Period ID: '  + convert(varchar(20),@premium_adj_prog_id)
			rollback transaction ModAISCalcERP
			set @err_msg_op = @err_message
			exec [dbo].[AddAPLCTN_STS_LOG] 
				@premium_adjustment_id = @premium_adjustment_id,
				@customer_id = @customer_id,
				@premium_adj_prog_id = @premium_adj_prog_id,
				@err_msg = @err_message,
				@create_user_id = @create_user_id
			return
		end	
	end

	/***********************************************************************
	* Conversion from PAID to INCURRED if applicable
	***********************************************************************/
	
	select 
	@next_val_dt = nxt_valn_dt,
	@conversion_dt = 
		case when datepart(Day,strt_dt) >15
		then
		DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,incur_conv_mms_cnt, strt_dt))+1,0))  
		else
		DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,incur_conv_mms_cnt, strt_dt)),0))  
		end
	from dbo.PREM_ADJ_PGM
	where --custmr_id = @customer_id and
	prem_adj_pgm_id = @premium_adj_prog_id
	and actv_ind = 1

	select 
	@pgm_lkup_txt = lk.lkup_txt,
	@pgm_type_id = pgm.pgm_typ_id
	from dbo.PREM_ADJ_PGM pgm
	inner join dbo.LKUP lk on (pgm.pgm_typ_id = lk.lkup_id)
	where --custmr_id = @customer_id and
	prem_adj_pgm_id = @premium_adj_prog_id
	and pgm.actv_ind = 1
	and lk.actv_ind = 1

	if(substring(@pgm_lkup_txt,1,3) = 'DEP')
		set @pgm_type = 1 --DEP Program Type
	else
		set @pgm_type = 2 --NON-DEP Program Type

	if (@pgm_type = 1 ) --DEP Program Type
	begin
		if not exists (select * from dbo.COML_AGMT where prem_adj_pgm_id = @premium_adj_prog_id and pol_sym_txt = 'DEP')
		begin
			set @err_message = 'ERP: DEP program type has been selected but a master policy does not exist for this program period. Please designate a master policy and then proceed.' 
			+ ';customer ID: ' + convert(varchar(20),@customer_id) 
			+ ';Program Period ID: ' + convert(varchar(20),@premium_adj_prog_id) 
			rollback transaction ModAISCalcERP
			set @err_msg_op = @err_message
			exec [dbo].[AddAPLCTN_STS_LOG] 
				@premium_adjustment_id = @premium_adjustment_id,
				@customer_id = @customer_id,
				@premium_adj_prog_id = @premium_adj_prog_id,
				@err_msg = @err_message,
				@create_user_id = @create_user_id

			return
		end
	end

	--if (datediff(d,@next_val_dt,@conversion_dt) = 0)
	if(convert(varchar,@next_val_dt,101) = convert(varchar,@conversion_dt,101))
	begin
		if @debug = 1
		begin
		print 'val date equal to conv date; perform conversion'
		end
		update dbo.PREM_ADJ_PGM WITH (ROWLOCK)
			set paid_incur_typ_id = 297, -- Lookup type: "PAID/INCURRED (LBA ADJUSTMENT TYPE)"; lookup: "INCURRED"
			updt_dt=getdate()
		where prem_adj_pgm_id = @premium_adj_prog_id
		and paid_incur_typ_id = 298 -- Lookup type: "PAID/INCURRED (LBA ADJUSTMENT TYPE)"; lookup: "PAID"

		update dbo.COML_AGMT WITH (ROWLOCK)
			set adj_typ_id = 63, -- Incurred DEP
			updt_dt=getdate()
		where prem_adj_pgm_id = @premium_adj_prog_id
		and adj_typ_id = 70 -- Paid Loss DEP
	
		update dbo.COML_AGMT WITH (ROWLOCK)
			set adj_typ_id = 65, -- Incurred Loss Retro
			updt_dt=getdate()
		where prem_adj_pgm_id = @premium_adj_prog_id
		and adj_typ_id = 71 -- Paid Loss Retro

		update dbo.COML_AGMT WITH (ROWLOCK)
			set adj_typ_id = 67, -- Incurred Underlayer
			updt_dt=getdate()
		where prem_adj_pgm_id = @premium_adj_prog_id
		and adj_typ_id = 72 -- Paid Loss Underlayer

		update dbo.COML_AGMT WITH (ROWLOCK)
			set adj_typ_id = 66, -- Incurred Loss WA
			updt_dt=getdate()
		where prem_adj_pgm_id = @premium_adj_prog_id
		and adj_typ_id = 73 -- Paid Loss WA

--		update dbo.COML_AGMT WITH (ROWLOCK)
--			set adj_typ_id = 64 -- Incurred Loss Deductible
--		where prem_adj_pgm_id = @premium_adj_prog_id
--		and adj_typ_id = 69 -- Paid Loss Deductible

	end


	set @sub_formulas = ' '
	set @func_mod_str = ' '
	set @has_chf = 0
	set @has_ldf = 0
	set @has_lcf = 0
	set @has_basic = 0
	set @has_ncf = 0
	set @has_oa = 0
	set @is_elp = 0
	set @recalc_lcf = 0
	set @is_max_unlimited = 0
	set @recalc_minmax_for_paid = 1
	set @min_applies = 0
	set @max_applies = 0
	set @erp_applies = 0

	create table #params
	(
	id int ,
	val decimal(20,8)
	)

	insert into #params(id,val)
	values (1,@premium_adj_period_id)

	insert into #params(id,val)
	values (2,@premium_adjustment_id)

	insert into #params(id,val)
	values (3,@customer_id)

	insert into #params(id,val)
	values (4,@premium_adj_prog_id)

	insert into #params(id,val)
	values (5,@create_user_id)

	insert into #params(id,val)
	values (6,null) --@com_agm_id

	insert into #params(id,val)
	values (7,null) --@state_id

	insert into #params(id,val)
	values (8,null) --@subj_paid_idnmty_amt

	insert into #params(id,val)
	values (9,null) --@subj_paid_exps_amt

	insert into #params(id,val)
	values (10,null) --@subj_resrv_idnmty_amt

	insert into #params(id,val)
	values (11,null) --@subj_resrv_exps_amt

	insert into #params(id,val)
	values (12,null) --@lob_id

	insert into #params(id,val)
	values (13,null) --@basic

	select 
	@tm_ft = tax_multi_fctr_rt
	from dbo.PREM_ADJ_PGM
	where --custmr_id = @customer_id and
	prem_adj_pgm_id = @premium_adj_prog_id
	and actv_ind = 1

	set @tm_ft = isnull(@tm_ft,1)

	insert into #params(id,val)
	values (14,@tm_ft) --@tm_ft

	insert into #params(id,val)
	values (15,null) --@chf_amt

	insert into #params(id,val)
	values (16,null) --NCF

	insert into #params(id,val)
	values (17,null) --OA

	insert into #params(id,val)
	values (18,null) --LCF Aggregate cap

	insert into #params(id,val)
	values (19,null) --ELP

	insert into #params(id,val)
	values (20,null) --Incurred ERP MinMax differential

	insert into #params(id,val)
	values (21,null) --Adjust CFB MinMax differential
	
	insert into #params(id,val)
	values (22,null) --Has LDF

	insert into #params(id,val)
	values (23,null) --Has LCF
	
	insert into #params(id,val)
	values (24,null) --@lcf_aggr_cap_set_pgm_amt

	set @comp_list = 'PL,PALAE,LR,ALAER,IL,IALAE,Basic,CHF,LBA,'
	set @comp_list = @comp_list + 'PL,PALAE,RML,ELP,'
	set @comp_list =  @comp_list + 'OA,NCF,IBNR,LCF,LDF,TM,PASS'

	set @decl_str = ' '
	set @init_str = ' '

	;with Nums ( n ) AS (
    select 1 union all
    select 1 + n from Nums where n < (case when len(@frmla) > len(@comp_list) then len(@frmla) else len(@comp_list) end ) + 1 ) 
	select n  into #num from Nums
	option ( maxrecursion 2000 )

	set @counter = 1
	create table #frmla_parser
	(
	id int identity(1,1),
	frmla_comp varchar(30),
	identifier_pos smallint
	)

	create index ind ON #frmla_parser (id)

	insert into #frmla_parser(frmla_comp,identifier_pos)
	select 
	substring( ',' + @comp_list + ',', n + 1,charindex( ',', ',' + @comp_list + ',', n + 1 ) - n - 1 ) as "Comp",
	charindex( substring( ',' + @comp_list + ',', n + 1, charindex( ',', ',' + @comp_list + ',', n + 1 ) - n - 1 ), @frmla)
    from #num
    where substring( ',' + @comp_list + ',', n, 1 ) = ','
    and n < len( ',' + @comp_list + ',' )

	--select * from #frmla_parser

	select @lcf_aggr_cap_set_pgm_amt = isnull(los_conv_fctr_aggr_cap_amt,0) 
	from dbo.PREM_ADJ_PGM_SETUP 
	where 
	custmr_id = @customer_id 
	and prem_adj_pgm_id = @premium_adj_prog_id
	and actv_ind = 1
	and adj_parmet_typ_id = 402 -- Adj. Parameter type ID for ADJUSTMENT PARAMETER TYPE -> LCF

	set @lcf_aggr_cap_set_pgm_amt = isnull(@lcf_aggr_cap_set_pgm_amt,0)
	if @debug = 1
	begin
	print '@lcf_aggr_cap_set_pgm_amt:' + convert(varchar(20),@lcf_aggr_cap_set_pgm_amt)
	end

	if (@lcf_aggr_cap_set_pgm_amt > 0)
		set @recalc_lcf = 1

	select @count = count(*) from #frmla_parser
	--print @count

	while @counter <= @count
	begin

		select 
		@frmla_comp = frmla_comp,
		@identifier_pos = identifier_pos
		from #frmla_parser 
		where id = @counter
			
		if @identifier_pos <> 0
		begin
			--IL
			if @frmla_comp = 'IL'
			begin
				select @frmla = replace(@frmla, 'IL','@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt')
			end

			--IALAE
			if @frmla_comp = 'IALAE'
			begin
				select @frmla = replace(@frmla, 'IALAE','@subj_paid_exps_amt + @subj_resrv_exps_amt')
			end

			--PL
			if @frmla_comp = 'PL'
			begin
				select @frmla = replace(@frmla, 'PL','@subj_paid_idnmty_amt')
			end

			--PALAE
			if @frmla_comp = 'PALAE'
			begin
				select @frmla = replace(@frmla, 'PALAE','@subj_paid_exps_amt')
			end

			--LR
			if @frmla_comp = 'LR'
			begin
				select @frmla = replace(@frmla, 'LR','@subj_resrv_idnmty_amt')
			end

			--ALAER
			if @frmla_comp = 'ALAER'
			begin
				select @frmla = replace(@frmla, 'ALAER','@subj_resrv_exps_amt')
			end


			--Basic
			if @frmla_comp = 'Basic'
			begin
				select @frmla = replace(@frmla, 'Basic','isnull(@basic,0)')
				set @has_basic = 1
			end


			--ELP
			if @frmla_comp = 'ELP'
			begin
				select @frmla = replace(@frmla, 'ELP','isnull(@elp,0)')
				set @is_elp = 1
			end


			--CHF
			if @frmla_comp = 'CHF'
			begin
				set @has_chf = 1
				set @init_str = @init_str + ' select @chf_amt = val from #params where id = 15'

				select @frmla = replace(@frmla, 'CHF','isnull(@chf_amt,0)')

			end


			--LDF
			if @frmla_comp = 'LDF'
			begin
				set @has_ldf = 1
				set @init_str = @init_str + ' exec @ldf = [dbo].[fn_GetFactorsIBNR_LDF] @p_com_agm_id = @com_agm_id,@p_premium_adj_prog_id = @premium_adj_prog_id,@p_customer_id = @customer_id,@is_ibnr = 0'
				select @frmla = replace(@frmla, 'LDF','@ldf')

				exec @sub_formulas_temp = dbo.[fn_FindFormulaSectionERP]
					 @txtMainFormula = @frmla,
					 @identifier = 'LDF'

				set @sub_formulas = @sub_formulas + ' select @ldf_result = ' + '(' + @sub_formulas_temp + ')*(@ldf - 1) select @ibnr_ldf_result = @ldf_result'
				set @sub_formulas = @sub_formulas + ' if @ldf_ibnr_lim_ind = 1'
				set @sub_formulas = @sub_formulas + ' begin'
				set @sub_formulas = @sub_formulas + '	select @exc_ldf_ibnr_amt = sum(isnull(exc_ldf_ibnr_amt,0))  from ARMIS_LOS_POL where prem_adj_pgm_id = @premium_adj_prog_id and coml_agmt_id = @com_agm_id and st_id = @state_id and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) and valn_dt = @prem_adj_valn_dt and actv_ind = 1 '
				set @sub_formulas = @sub_formulas + '	select @subj_ldf_ibnr_amt = sum(isnull(subj_ldf_ibnr_amt,0))  from ARMIS_LOS_POL where prem_adj_pgm_id = @premium_adj_prog_id and coml_agmt_id = @com_agm_id and st_id = @state_id and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) and valn_dt = @prem_adj_valn_dt and actv_ind = 1 '
				set @sub_formulas = @sub_formulas + '	select @exc_ldf_ibnr_amt = isnull(@exc_ldf_ibnr_amt,0) '
				set @sub_formulas = @sub_formulas + '	select @subj_ldf_ibnr_amt = isnull(@subj_ldf_ibnr_amt,0) '

				set @sub_formulas = @sub_formulas + '   if (@has_lcf = 1)'
				set @sub_formulas = @sub_formulas + '	   select @ibnr_ldf_result = @ibnr_ldf_result - (@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf + @subj_ldf_ibnr_amt '
				set @sub_formulas = @sub_formulas + '   else'
				set @sub_formulas = @sub_formulas + '	   select @ibnr_ldf_result = @ibnr_ldf_result - @exc_ldf_ibnr_amt'
				set @sub_formulas = @sub_formulas + ' end'


				set @ldf_sub_formula = ' select @ldf_result = ' + '(' + @sub_formulas_temp + ')*(@ldf - 1)'
				set @ldf_sub_formula = @ldf_sub_formula + ' if @ldf_ibnr_lim_ind = 1'
				set @ldf_sub_formula = @ldf_sub_formula + ' begin'
				set @ldf_sub_formula = @ldf_sub_formula + '		select @exc_ldf_ibnr_amt = sum(isnull(exc_ldf_ibnr_amt,0))  from ARMIS_LOS_POL where prem_adj_pgm_id = @premium_adj_prog_id and coml_agmt_id = @com_agm_id and st_id = @state_id and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) and valn_dt = @prem_adj_valn_dt and actv_ind = 1 '
				set @ldf_sub_formula = @ldf_sub_formula + '	select @subj_ldf_ibnr_amt = sum(isnull(subj_ldf_ibnr_amt,0))  from ARMIS_LOS_POL where prem_adj_pgm_id = @premium_adj_prog_id and coml_agmt_id = @com_agm_id and st_id = @state_id and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) and valn_dt = @prem_adj_valn_dt and actv_ind = 1 '
				set @ldf_sub_formula = @ldf_sub_formula + '	select @exc_ldf_ibnr_amt = isnull(@exc_ldf_ibnr_amt,0) '
				set @ldf_sub_formula = @ldf_sub_formula + '	select @subj_ldf_ibnr_amt = isnull(@subj_ldf_ibnr_amt,0) '

				set @ldf_sub_formula = @ldf_sub_formula + '   if (@has_lcf = 1)'
				set @ldf_sub_formula = @ldf_sub_formula + '	   select @ibnr_ldf_result = @ibnr_ldf_result - (@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf + @subj_ldf_ibnr_amt '
				set @ldf_sub_formula = @ldf_sub_formula + '   else'
				set @ldf_sub_formula = @ldf_sub_formula + '	   select @ibnr_ldf_result = @ibnr_ldf_result - @exc_ldf_ibnr_amt'
				set @ldf_sub_formula = @ldf_sub_formula + ' end'
			end


			--IBNR
			if @frmla_comp = 'IBNR'
			begin
				set @init_str = @init_str + ' exec @ibnr = [dbo].[fn_GetFactorsIBNR_LDF] @p_com_agm_id = @com_agm_id,@p_premium_adj_prog_id = @premium_adj_prog_id,@p_customer_id = @customer_id,@is_ibnr = 1'
				select @frmla = replace(@frmla, 'IBNR','@ibnr')

				exec @sub_formulas_temp = dbo.[fn_FindFormulaSectionERP]
					 @txtMainFormula = @frmla,
					 @identifier = 'IBNR'
				set @sub_formulas = @sub_formulas + ' select @ibnr_result = ' + '(' + @sub_formulas_temp + ')*(@ibnr - 1) select @ibnr_ldf_result = @ibnr_result'
			
				set @sub_formulas = @sub_formulas + ' if @ldf_ibnr_lim_ind = 1'
				set @sub_formulas = @sub_formulas + ' begin'
				set @sub_formulas = @sub_formulas + '	select @exc_ldf_ibnr_amt = sum(isnull(exc_ldf_ibnr_amt,0))  from ARMIS_LOS_POL where prem_adj_pgm_id = @premium_adj_prog_id and coml_agmt_id = @com_agm_id and st_id = @state_id and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) and valn_dt = @prem_adj_valn_dt and actv_ind = 1 '
				set @sub_formulas = @sub_formulas + '	select @subj_ldf_ibnr_amt = sum(isnull(subj_ldf_ibnr_amt,0))  from ARMIS_LOS_POL where prem_adj_pgm_id = @premium_adj_prog_id and coml_agmt_id = @com_agm_id and st_id = @state_id and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) and valn_dt = @prem_adj_valn_dt and actv_ind = 1 '
				set @sub_formulas = @sub_formulas + '	select @exc_ldf_ibnr_amt = isnull(@exc_ldf_ibnr_amt,0) '
				set @sub_formulas = @sub_formulas + '	select @subj_ldf_ibnr_amt = isnull(@subj_ldf_ibnr_amt,0) '

				set @sub_formulas = @sub_formulas + '   if (@has_lcf = 1)'
				set @sub_formulas = @sub_formulas + '	   select @ibnr_ldf_result = @ibnr_ldf_result - (@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf + @subj_ldf_ibnr_amt '
				set @sub_formulas = @sub_formulas + '   else'
				set @sub_formulas = @sub_formulas + '	   select @ibnr_ldf_result = @ibnr_ldf_result - @exc_ldf_ibnr_amt'
				set @sub_formulas = @sub_formulas + ' end'
			end


			--LBA
			if @frmla_comp = 'LBA'
			begin
				set @init_str = @init_str + ' exec @lba_result = [dbo].[fn_RetrieveLBA_Amt] @p_cust_id = @customer_id,@p_prem_adj_id = @premium_adjustment_id,@p_prem_adj_perd_id = @premium_adj_period_id,@p_comm_agr_id = @com_agm_id,@p_state_id = @state_id'
				select @frmla = replace(@frmla, 'LBA','isnull(@lba_result,0)')
			end


			--RML
			if @frmla_comp = 'RML'
			begin
				set @init_str = @init_str + ' exec @rml = [dbo].[fn_RetrieveRML_Amt] @premium_adj_period_id = @premium_adj_period_id,@premium_adjustment_id = @premium_adjustment_id,@customer_id = @customer_id,@premium_adj_prog_id = @premium_adj_prog_id,@coml_agmt_id  = @com_agm_id,@lob_id = @lob_id,@state_id  = @state_id,@subj_paid_idnmty_amt  = @subj_paid_idnmty_amt,@subj_resrv_idnmty_amt  = @subj_resrv_idnmty_amt,@subj_paid_exps_amt  = @subj_paid_exps_amt,@subj_resrv_exps_amt  = @subj_resrv_exps_amt,@lcf = @lcf,@basic  = @basic,@tm  = @tm,@create_user_id  = @create_user_id'
				select @frmla = replace(@frmla, 'RML','@rml')
			end


			--NCF
			if @frmla_comp = 'NCF'
			begin
				set @has_ncf = 1
				set @init_str = @init_str + ' set @ncf_amt = 0'
				select @frmla = replace(@frmla, 'NCF','isnull(@ncf_amt,0)')
			end

			--Other Amount
			if @frmla_comp = 'OA'
			begin
				set @has_oa = 1
				set @init_str = @init_str + ' set @oa_amt = 0'
				select @frmla = replace(@frmla, 'OA','isnull(@oa_amt,0)')
			end


			--PASS
			if @frmla_comp = 'PASS'
			begin
				set @init_str = @init_str + ' exec @pa_ft = [dbo].[fn_RetrievePremiumAssessmentFactor] @p_cust_id = @customer_id,@p_prem_adj_prog_id = @premium_adj_prog_id,@p_comm_agr_id = @com_agm_id, @p_state_id = @state_id'

				select @frmla = replace(@frmla, 'PASS','@pa_ft')
				exec @sub_formulas_temp = dbo.[fn_FindFormulaSectionERP]
					 @txtMainFormula = @frmla,
					 @identifier = 'pa_ft'

				set @sub_formulas = @sub_formulas + ' exec @nullable_pa_ft = [dbo].[fn_RetrievePremiumAssessmentFactor] @p_cust_id = @customer_id,@p_prem_adj_prog_id = @premium_adj_prog_id,@p_comm_agr_id = @com_agm_id, @p_state_id = @state_id,@p_ret_null=1 '
				set @sub_formulas = @sub_formulas + ' select @pa_result = ' + '(' + @sub_formulas_temp + ')*(@nullable_pa_ft -1)'
			end



			--LCF
			if @frmla_comp = 'LCF'
			begin
				set @has_lcf = 1
				set @init_str = @init_str + ' exec @lcf = [dbo].[fn_RetrieveLCF] @p_cust_id = @customer_id,@p_prem_adj_prog_id = @premium_adj_prog_id,@p_comm_agr_id = @com_agm_id, @p_lob_id = @lob_id, @p_state_id = @state_id'
				set @init_str = @init_str + ' set @lcf_aggr_cap = 0'

				select @frmla = replace(@frmla, 'LCF','@lcf')
				-- May need to move out sub formula section below while loop
				exec @sub_formulas_temp = dbo.[fn_FindFormulaSectionERP]
					 @txtMainFormula = @frmla,
					 @identifier = 'LCF'
				set @sub_formulas = @sub_formulas + ' select @lcf_result = ' + '(' + @sub_formulas_temp + ')*(@lcf -1)'
				set @lcf_sub_formula = ' select @lcf_result = ' + '(' + @sub_formulas_temp + ')*(@lcf -1)'
				
				if (@recalc_lcf = 1)
					set @sub_formulas = @sub_formulas + ' set @lcf_result = @lcf_result - isnull(@lcf_aggr_cap,0)'

			end


			--TM
			if @frmla_comp = 'TM'
			begin
				--set @init_str = @init_str + ' set @tm = fn_RetrieveTM '
				select @frmla = replace(@frmla, 'TM','@tm')

				exec @sub_formulas_temp = dbo.[fn_FindFormulaSectionERP]
					 @txtMainFormula = @frmla,
					 @identifier = 'TM'
				if (@frmla_id = 30) 
				begin
					set @sub_formulas = @sub_formulas + ' select @tm_result = ' + '(' + @sub_formulas_temp + ')*(@tm - 1)'
				end
				
				set @tax_formula = ' select @tm_result = ' + '(' + @sub_formulas_temp + ')*(@tm - 1)'

--				if ((@frmla_id = 21) or (@frmla_id = 22)) -- Special handling for Underlayers and WA
--				begin
--					set @sub_formulas = @sub_formulas + '- (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*(@tm - 1)'
--				end
--				else 
--				if (@frmla_id = 36) -- Special handling for second Underlayer formula
--				begin
--					set @sub_formulas = @sub_formulas + '- (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*(@tm - 1) - (@subj_paid_exps_amt + @subj_resrv_exps_amt)*(@tm - 1)'
--				end
--				else 
				if (@frmla_id = 30) 
				begin
					set @sub_formulas = @sub_formulas + '- (@ldf_result)*(@tm - 1)'
				end

			end

		end --end of: if @identifier_pos <> 0

		set @counter = @counter + 1
	end --end of: while @counter <= @count

	set @decl_str = @decl_str + '@basic decimal(15,2),'
	set @decl_str = @decl_str + '@elp decimal(15,2),'
	set @decl_str = @decl_str + '@chf_amt decimal(15,2),'
	set @decl_str = @decl_str + '@ldf decimal(15, 8),@ldf_result decimal(15,2), @ibnr_ldf_result decimal(15,2),'
	set @decl_str = @decl_str + '@ibnr decimal(15, 8),@ibnr_result decimal(15,2),'
	set @decl_str = @decl_str + '@lba_result decimal(15,2),'
	set @decl_str = @decl_str + '@rml decimal(15,2),@paid_rml_amt decimal(15,2),@earned_resd_amt decimal(15,2),'
	set @decl_str = @decl_str + '@ncf_amt decimal(15,2),'
	set @decl_str = @decl_str + '@oa_amt decimal(15,2),'
	set @decl_str = @decl_str + '@subj_ldf_ibnr_amt decimal(15,2),@exc_ldf_ibnr_amt decimal(15,2),'
	set @decl_str = @decl_str + '@lcf decimal(15,8),@lcf_aggr_cap decimal(15,8),@lcf_result decimal(15,2),'
	set @decl_str = @decl_str + '@tm decimal(15,8),@tm_result decimal(15,2),@pa_ft decimal(15,8),@nullable_pa_ft decimal(15,8),@paid_ratio decimal(15,8),@pa_result decimal(15,2),'

	if( substring(@decl_str,len(@decl_str),1) = ',')
		set @decl_str = left(@decl_str, len(@decl_str) - 1)


	select @paid_erp_frmla = replace(@frmla, '@subj_resrv_idnmty_amt','0');
	select @paid_erp_frmla = replace(@paid_erp_frmla, '@subj_resrv_exps_amt','0');

	--Declarations
	set @stmt = 'declare @premium_adj_period_id int, @premium_adjustment_id int, @customer_id int, @premium_adj_prog_id int, @create_user_id int' 
	set @stmt = @stmt + ' ,@com_agm_id int, @state_id int, @lob_id int, @subj_paid_idnmty_amt decimal(15,2), @subj_paid_exps_amt decimal(15,2), @subj_resrv_idnmty_amt decimal(15,2), @subj_resrv_exps_amt decimal(15,2) '
	set @stmt = @stmt + ' ,@inc_erp_amt  decimal(15,2), @paid_erp_amt  decimal(15,2), @cfb_amt decimal(15,2), @has_ldf bit , @has_lcf bit , @prem_adj_retro_id int,@prem_adj_retro_dtl_id int '
	set @stmt = @stmt + ' ,@adj_typ_id  int,@ldf_ibnr_lim_ind bit,@adj_ded_wc_loss_amt decimal(15,2),@incur_erp_diff decimal(15,2),@adj_incur_erp_minmax decimal(15,2) '
	set @stmt = @stmt + ' ,@adj_cfb_diff decimal(15,2),@adj_cfb_minmax decimal(15,2),@is_not_master smallint,@std_sub_prem_amount decimal(15,2),@lcf_aggr_cap_amt decimal(15,2),@prem_adj_valn_dt datetime,'

	set @stmt = @stmt + @decl_str 


	--Initialization
	set @stmt = @stmt + ' select @premium_adj_period_id = convert(int,val) from #params where id = 1'
	set @stmt = @stmt + ' select @premium_adjustment_id = convert(int,val) from #params where id = 2'
	set @stmt = @stmt + ' select @customer_id = convert(int,val) from #params where id = 3'
	set @stmt = @stmt + ' select @premium_adj_prog_id = convert(int,val) from #params where id = 4'
	set @stmt = @stmt + ' select @create_user_id = convert(int,val) from #params where id = 5'
	set @stmt = @stmt + ' select @com_agm_id = convert(int,val) from #params where id = 6'
	set @stmt = @stmt + ' select @state_id = convert(int,val) from #params where id = 7'
	set @stmt = @stmt + ' select @subj_paid_idnmty_amt = val from #params where id = 8'
	set @stmt = @stmt + ' select @subj_paid_exps_amt = val from #params where id = 9'
	set @stmt = @stmt + ' select @subj_resrv_idnmty_amt = val from #params where id = 10'
	set @stmt = @stmt + ' select @subj_resrv_exps_amt = val from #params where id = 11'
	set @stmt = @stmt + ' select @lob_id = convert(int,val) from #params where id = 12'
	set @stmt = @stmt + ' select @basic = val from #params where id = 13'
	set @stmt = @stmt + ' select @tm = val from #params where id = 14'
	set @stmt = @stmt + ' select @elp = val from #params where id = 19'
	set @stmt = @stmt + ' select @has_ldf = val from #params where id = 22'
	set @stmt = @stmt + ' select @has_lcf = val from #params where id = 23'
	set @stmt = @stmt + ' select @adj_typ_id = adj_typ_id from dbo.COML_AGMT where coml_agmt_id = @com_agm_id'
	set @stmt = @stmt + ' select @ldf_ibnr_lim_ind = los_dev_fctr_incur_but_not_rptd_incld_lim_ind from dbo.COML_AGMT where coml_agmt_id = @com_agm_id'
	set @stmt = @stmt + ' select @is_not_master = case when pol_sym_txt = ' + char(39) + 'DEP' + char(39) + 'then 0 else 1 end from COML_AGMT where coml_agmt_id = @com_agm_id'
	set @stmt = @stmt + ' select @std_sub_prem_amount = isnull(prem_amt,0) from dbo.SUBJ_PREM_AUDT where custmr_id = @customer_id and prem_adj_pgm_id = @premium_adj_prog_id and coml_agmt_id = @com_agm_id and st_id = @state_id and actv_ind = 1'
	set @stmt = @stmt + ' select @lcf_aggr_cap_amt = val from #params where id = 24'
	set @stmt = @stmt + ' select @prem_adj_valn_dt = valn_dt from dbo.PREM_ADJ where prem_adj_id = @premium_adjustment_id '
	
	set @stmt = @stmt + @init_str

	set @stmt = @stmt + @tax_formula

	--Specify dynamic formula
	-- Lookup Type: 'Adjustment type'; lookup value: 'Paid Loss Retro'
	set @stmt = @stmt + ' if (@adj_typ_id = 71)' 
	set @stmt = @stmt + ' begin'

	if (@frmla_id = 30) -- No TM on LDF 
	begin
		set @stmt = @stmt + @ldf_sub_formula
		set @stmt = @stmt + '		select @inc_erp_amt = ' + @frmla + '- (@ldf_result)*(@tm - 1)'
		set @stmt = @stmt + '		select @paid_erp_amt = ' + @paid_erp_frmla + '- (@ldf_result)*(@tm - 1)'
	end
	else 
	begin
		set @stmt = @stmt + '      select @inc_erp_amt = ' + @frmla 
		set @stmt = @stmt + '	   select @paid_erp_amt = ' + @paid_erp_frmla 
	end

	-- Handling for LDF / IBNR limit indicator
	set @stmt = @stmt + ' if @ldf_ibnr_lim_ind = 1'
	set @stmt = @stmt + ' begin'
	set @stmt = @stmt + '	select @exc_ldf_ibnr_amt = sum(isnull(exc_ldf_ibnr_amt,0)), @subj_ldf_ibnr_amt =  sum(isnull(subj_ldf_ibnr_amt,0)) from ARMIS_LOS_POL where prem_adj_pgm_id = @premium_adj_prog_id and coml_agmt_id = @com_agm_id and st_id = @state_id and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) and valn_dt = @prem_adj_valn_dt and actv_ind = 1 '
	set @stmt = @stmt + '	select @exc_ldf_ibnr_amt = isnull(@exc_ldf_ibnr_amt,0) '
	set @stmt = @stmt + '	select @subj_ldf_ibnr_amt = isnull(@subj_ldf_ibnr_amt,0) '


	set @stmt = @stmt + '  if (@has_lcf = 1)'
	set @stmt = @stmt + '	select @inc_erp_amt = @inc_erp_amt - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf - @subj_ldf_ibnr_amt)*(@tm)'
	set @stmt = @stmt + '  else'
	set @stmt = @stmt + '	select @inc_erp_amt = @inc_erp_amt - @exc_ldf_ibnr_amt - @exc_ldf_ibnr_amt*(@tm - 1) '
	
	set @stmt = @stmt + '  if (@has_ldf = 1)'
	set @stmt = @stmt + '  begin'
	set @stmt = @stmt + '   exec @paid_ratio = [dbo].[fn_ComputePaidExcessRatioByPolicy] @customer_id = @customer_id,@premium_adj_prog_id = @premium_adj_prog_id,@prem_adj_id=@premium_adjustment_id,@coml_agmt_id = @com_agm_id,@state_id = @state_id'
	set @stmt = @stmt + '  if (@has_lcf = 1)'
	set @stmt = @stmt + '	select @paid_erp_amt = @paid_erp_amt - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf*@paid_ratio - @subj_ldf_ibnr_amt*@paid_ratio)*(@tm)'
	set @stmt = @stmt + '  else'
	set @stmt = @stmt + '	select @paid_erp_amt = @paid_erp_amt - (@exc_ldf_ibnr_amt*@paid_ratio) - (@exc_ldf_ibnr_amt*@paid_ratio)*(@tm - 1) '
	set @stmt = @stmt + '  end'


	set @stmt = @stmt + '  if (@has_lcf = 1)'
	set @stmt = @stmt + '	select @tm_result = @tm_result - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf - @subj_ldf_ibnr_amt)*(@tm - 1)'
	set @stmt = @stmt + '  else'
	set @stmt = @stmt + '	select @tm_result = @tm_result - @exc_ldf_ibnr_amt*(@tm - 1)'

	set @stmt = @stmt + ' end'

	set @stmt = @stmt + '	   select @cfb_amt = @inc_erp_amt - @paid_erp_amt'
	set @stmt = @stmt + ' end'
	-- Lookup Type: 'Adjustment type'; lookup value: 'Incurred Loss Retro'
	set @stmt = @stmt + ' else if (@adj_typ_id = 65)' 
	set @stmt = @stmt + ' begin'

	if (@frmla_id = 30) -- No TM on LDF 
	begin
		set @stmt = @stmt + @ldf_sub_formula
		set @stmt = @stmt + '		select @inc_erp_amt = ' + @frmla + '- (@ldf_result)*(@tm - 1)'
	end
	else 
	begin
		set @stmt = @stmt + '	   select @inc_erp_amt = ' + @frmla
	end 

	-- Handling for LDF / IBNR limit indicator
	set @stmt = @stmt + ' if @ldf_ibnr_lim_ind = 1'
	set @stmt = @stmt + ' begin'
	set @stmt = @stmt + '	select @exc_ldf_ibnr_amt = sum(isnull(exc_ldf_ibnr_amt,0)), @subj_ldf_ibnr_amt =  sum(isnull(subj_ldf_ibnr_amt,0)) from ARMIS_LOS_POL where prem_adj_pgm_id = @premium_adj_prog_id and coml_agmt_id = @com_agm_id and st_id = @state_id and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) and valn_dt = @prem_adj_valn_dt and actv_ind = 1 '
	set @stmt = @stmt + '	select @exc_ldf_ibnr_amt = isnull(@exc_ldf_ibnr_amt,0) '
	set @stmt = @stmt + '	select @subj_ldf_ibnr_amt = isnull(@subj_ldf_ibnr_amt,0) '

	set @stmt = @stmt + '  if (@has_lcf = 1)'
	set @stmt = @stmt + '	select @inc_erp_amt = @inc_erp_amt - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf - @subj_ldf_ibnr_amt)*(@tm)'
	set @stmt = @stmt + '  else'
	set @stmt = @stmt + '	select @inc_erp_amt = @inc_erp_amt - @exc_ldf_ibnr_amt - @exc_ldf_ibnr_amt*(@tm - 1) '

	set @stmt = @stmt + '  if (@has_lcf = 1)'
	set @stmt = @stmt + '	select @tm_result = @tm_result - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf - @subj_ldf_ibnr_amt)*(@tm - 1)'
	set @stmt = @stmt + '  else'
	set @stmt = @stmt + '	select @tm_result = @tm_result - @exc_ldf_ibnr_amt*(@tm - 1)'

	set @stmt = @stmt + ' end'


	set @stmt = @stmt + '	   select @paid_erp_amt = 0'
	set @stmt = @stmt + '	   select @cfb_amt = 0'
	set @stmt = @stmt + ' end'
	-- Lookup Type: 'Adjustment type'; lookup value: 'Paid Loss Underlayer'
	set @stmt = @stmt + ' else if (@adj_typ_id = 72) 
							begin'
	if (@frmla_id = 21) --Adjustable WCD - UNDERLAYER 
	begin
		set @stmt = @stmt + '		select @inc_erp_amt = ' + @frmla + '- (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*@tm'
		set @stmt = @stmt + '		select @paid_erp_amt = ' + @paid_erp_frmla + '- (@subj_paid_idnmty_amt)*@tm'
		set @stmt = @stmt + '		select @tm_result = @tm_result - (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*(@tm - 1)'
	end
	else if (@frmla_id = 36) --Underlayers with No TM on Expenses
	begin
		set @stmt = @stmt + '		select @inc_erp_amt = ' + @frmla + '- ((@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*@tm) - ((@subj_paid_exps_amt + @subj_resrv_exps_amt)*@tm-(@subj_paid_exps_amt + @subj_resrv_exps_amt))'
		set @stmt = @stmt + '		select @paid_erp_amt = ' + @paid_erp_frmla + '- ((@subj_paid_idnmty_amt )*@tm) - ((@subj_paid_exps_amt )*@tm-(@subj_paid_exps_amt ))'
		set @stmt = @stmt + '		select @tm_result = @tm_result - (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*(@tm - 1) - (@subj_paid_exps_amt + @subj_resrv_exps_amt)*(@tm - 1)'

		--10726 Bug Fix
		set @stmt = @stmt + '		select @inc_erp_amt = @inc_erp_amt - ((@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*(@lcf-1)*(@tm-1)) - ((@subj_paid_exps_amt + @subj_resrv_exps_amt)*(@lcf-1)*@tm-(@subj_paid_exps_amt + @subj_resrv_exps_amt)*(@lcf-1))'
		set @stmt = @stmt + '		select @paid_erp_amt = @paid_erp_amt - ((@subj_paid_idnmty_amt )*(@lcf-1)*(@tm-1)) - ((@subj_paid_exps_amt )*(@lcf-1)*@tm-(@subj_paid_exps_amt )*(@lcf-1))'
		set @stmt = @stmt + '		select @tm_result = @tm_result - (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*(@lcf-1)*(@tm - 1) - (@subj_paid_exps_amt + @subj_resrv_exps_amt)*(@lcf-1)*(@tm - 1)'
		--10726 Bug Fix	
	end


	-- Handling for LDF / IBNR limit indicator
	set @stmt = @stmt + ' if @ldf_ibnr_lim_ind = 1'
	set @stmt = @stmt + ' begin'
	set @stmt = @stmt + '	select @exc_ldf_ibnr_amt = sum(isnull(exc_ldf_ibnr_amt,0)), @subj_ldf_ibnr_amt =  sum(isnull(subj_ldf_ibnr_amt,0)) from ARMIS_LOS_POL where prem_adj_pgm_id = @premium_adj_prog_id and coml_agmt_id = @com_agm_id and st_id = @state_id and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) and valn_dt = @prem_adj_valn_dt and actv_ind = 1 '
	set @stmt = @stmt + '	select @exc_ldf_ibnr_amt = isnull(@exc_ldf_ibnr_amt,0) '
	set @stmt = @stmt + '	select @subj_ldf_ibnr_amt = isnull(@subj_ldf_ibnr_amt,0) '


	set @stmt = @stmt + '  if (@has_lcf = 1)'
	set @stmt = @stmt + '	select @inc_erp_amt = @inc_erp_amt - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf - @subj_ldf_ibnr_amt)*(@tm)'
	set @stmt = @stmt + '  else'
	set @stmt = @stmt + '	select @inc_erp_amt = @inc_erp_amt - @exc_ldf_ibnr_amt - @exc_ldf_ibnr_amt*(@tm - 1) '

	set @stmt = @stmt + '  if (@has_ldf = 1)'
	set @stmt = @stmt + '  begin'
	set @stmt = @stmt + '   exec @paid_ratio = [dbo].[fn_ComputePaidExcessRatioByPolicy] @customer_id = @customer_id,@premium_adj_prog_id = @premium_adj_prog_id,@prem_adj_id=@premium_adjustment_id,@coml_agmt_id = @com_agm_id,@state_id = @state_id'
	set @stmt = @stmt + '  if (@has_lcf = 1)'
	set @stmt = @stmt + '	select @paid_erp_amt = @paid_erp_amt - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf*@paid_ratio - @subj_ldf_ibnr_amt*@paid_ratio)*(@tm)'
	set @stmt = @stmt + '  else'
	set @stmt = @stmt + '	select @paid_erp_amt = @paid_erp_amt - (@exc_ldf_ibnr_amt*@paid_ratio) - (@exc_ldf_ibnr_amt*@paid_ratio)*(@tm - 1) '
	set @stmt = @stmt + '  end'

	set @stmt = @stmt + '  if (@has_lcf = 1)'
	set @stmt = @stmt + '	select @tm_result = @tm_result - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf - @subj_ldf_ibnr_amt)*(@tm - 1)'
	set @stmt = @stmt + '  else'
	set @stmt = @stmt + '	select @tm_result = @tm_result - @exc_ldf_ibnr_amt*(@tm - 1)'

	set @stmt = @stmt + ' end'


	set @stmt = @stmt + '		select @cfb_amt = (@inc_erp_amt - @paid_erp_amt) + @subj_resrv_idnmty_amt
								select @adj_ded_wc_loss_amt = @subj_paid_idnmty_amt + @subj_resrv_idnmty_amt
						 	end'
	-- Lookup Type: 'Adjustment type'; lookup value: 'Incurred Underlayer'
	set @stmt = @stmt + ' else if (@adj_typ_id = 67) 
							begin'
	--set @stmt = @stmt + '		select @inc_erp_amt = ' + @frmla + '- (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*@tm'
	if (@frmla_id = 21) --Adjustable WCD - UNDERLAYER 
	begin
		set @stmt = @stmt + '		select @inc_erp_amt = ' + @frmla + '- (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*@tm'
		set @stmt = @stmt + '		select @tm_result = @tm_result - (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*(@tm - 1)'
	end
	else if (@frmla_id = 36) --Underlayers with No TM on Expenses
	begin
		set @stmt = @stmt + '		select @inc_erp_amt = ' + @frmla + '- ((@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*@tm) - ((@subj_paid_exps_amt + @subj_resrv_exps_amt)*@tm-(@subj_paid_exps_amt + @subj_resrv_exps_amt))'
		set @stmt = @stmt + '		select @tm_result = @tm_result - (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*(@tm - 1) - (@subj_paid_exps_amt + @subj_resrv_exps_amt)*(@tm - 1)'

		--10726 Bug Fix
		set @stmt = @stmt + '		select @inc_erp_amt = @inc_erp_amt - ((@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*(@lcf-1)*(@tm-1)) - ((@subj_paid_exps_amt + @subj_resrv_exps_amt)*(@lcf-1)*@tm-(@subj_paid_exps_amt + @subj_resrv_exps_amt)*(@lcf-1))'
		set @stmt = @stmt + '		select @tm_result = @tm_result - (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*(@lcf-1)*(@tm - 1) - (@subj_paid_exps_amt + @subj_resrv_exps_amt)*(@lcf-1)*(@tm - 1)'
		--10726 Bug Fix
	end


	-- Handling for LDF / IBNR limit indicator
	set @stmt = @stmt + ' if @ldf_ibnr_lim_ind = 1'
	set @stmt = @stmt + ' begin'
	set @stmt = @stmt + '	select @exc_ldf_ibnr_amt = sum(isnull(exc_ldf_ibnr_amt,0)), @subj_ldf_ibnr_amt =  sum(isnull(subj_ldf_ibnr_amt,0)) from ARMIS_LOS_POL where prem_adj_pgm_id = @premium_adj_prog_id and coml_agmt_id = @com_agm_id and st_id = @state_id and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) and valn_dt = @prem_adj_valn_dt and actv_ind = 1 '
	set @stmt = @stmt + '	select @exc_ldf_ibnr_amt = isnull(@exc_ldf_ibnr_amt,0) '
	set @stmt = @stmt + '	select @subj_ldf_ibnr_amt = isnull(@subj_ldf_ibnr_amt,0) '


	set @stmt = @stmt + '  if (@has_lcf = 1)'
	set @stmt = @stmt + '	select @inc_erp_amt = @inc_erp_amt - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf - @subj_ldf_ibnr_amt)*(@tm)'
	set @stmt = @stmt + '  else'
	set @stmt = @stmt + '	select @inc_erp_amt = @inc_erp_amt - @exc_ldf_ibnr_amt - @exc_ldf_ibnr_amt*(@tm - 1) '

	set @stmt = @stmt + '  if (@has_lcf = 1)'
	set @stmt = @stmt + '	select @tm_result = @tm_result - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf - @subj_ldf_ibnr_amt)*(@tm - 1)'
	set @stmt = @stmt + '  else'
	set @stmt = @stmt + '	select @tm_result = @tm_result - @exc_ldf_ibnr_amt*(@tm - 1)'

	set @stmt = @stmt + ' end'

	set @stmt = @stmt + '		select @paid_erp_amt = 0
								select @cfb_amt = 0
								select @adj_ded_wc_loss_amt = @subj_paid_idnmty_amt + @subj_resrv_idnmty_amt
						 	end'
		-- Lookup Type: 'Adjustment type'; lookup value: 'Paid Loss DEP'
	--TODO: CLARIFY
	set @stmt = @stmt + ' else if (@adj_typ_id = 70)' 
	set @stmt = @stmt + ' begin'
	set @stmt = @stmt + '      select @inc_erp_amt = ' + @frmla 
	set @stmt = @stmt + '	   select @paid_erp_amt = ' + @paid_erp_frmla 

	-- Handling for LDF / IBNR limit indicator
	set @stmt = @stmt +	' if @ldf_ibnr_lim_ind = 1'
	set @stmt = @stmt +	' begin'
	set @stmt = @stmt + '	select @exc_ldf_ibnr_amt = sum(isnull(exc_ldf_ibnr_amt,0)), @subj_ldf_ibnr_amt =  sum(isnull(subj_ldf_ibnr_amt,0)) from ARMIS_LOS_POL where prem_adj_pgm_id = @premium_adj_prog_id and coml_agmt_id = @com_agm_id and st_id = @state_id and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) and valn_dt = @prem_adj_valn_dt and actv_ind = 1 '
	set @stmt = @stmt + '	select @exc_ldf_ibnr_amt = isnull(@exc_ldf_ibnr_amt,0) '
	set @stmt = @stmt + '	select @subj_ldf_ibnr_amt = isnull(@subj_ldf_ibnr_amt,0) '


	set @stmt = @stmt + '  if (@has_lcf = 1)'
	set @stmt = @stmt + '	select @inc_erp_amt = @inc_erp_amt - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf - @subj_ldf_ibnr_amt)*(@tm)'
	set @stmt = @stmt + '  else'
	set @stmt = @stmt + '	select @inc_erp_amt = @inc_erp_amt - @exc_ldf_ibnr_amt - @exc_ldf_ibnr_amt*(@tm - 1) '

	set @stmt = @stmt + '  if (@has_ldf = 1)'
	set @stmt = @stmt + '  begin'
	set @stmt = @stmt + '   exec @paid_ratio = [dbo].[fn_ComputePaidExcessRatioByPolicy] @customer_id = @customer_id,@premium_adj_prog_id = @premium_adj_prog_id,@prem_adj_id=@premium_adjustment_id,@coml_agmt_id = @com_agm_id,@state_id = @state_id'
	set @stmt = @stmt + '  if (@has_lcf = 1)'
	set @stmt = @stmt + '	select @paid_erp_amt = @paid_erp_amt - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf*@paid_ratio - @subj_ldf_ibnr_amt*@paid_ratio)*(@tm)'
	set @stmt = @stmt + '  else'
	set @stmt = @stmt + '	select @paid_erp_amt = @paid_erp_amt - (@exc_ldf_ibnr_amt*@paid_ratio) - (@exc_ldf_ibnr_amt*@paid_ratio)*(@tm - 1) '
	set @stmt = @stmt + '  end'


	set @stmt = @stmt + '  if (@has_lcf = 1)'
	set @stmt = @stmt + '	select @tm_result = @tm_result - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf - @subj_ldf_ibnr_amt)*(@tm - 1)'
	set @stmt = @stmt + '  else'
	set @stmt = @stmt + '	select @tm_result = @tm_result - @exc_ldf_ibnr_amt*(@tm - 1)'

	set @stmt = @stmt +	' end'


	set @stmt = @stmt + '	   select @cfb_amt = @inc_erp_amt - @paid_erp_amt'
	set @stmt = @stmt + ' end'
	-- Lookup Type: 'Adjustment type'; lookup value: 'Incurred DEP'
	set @stmt = @stmt + ' else if (@adj_typ_id = 63)' 
	set @stmt = @stmt + ' begin'
	set @stmt = @stmt + '	   select @inc_erp_amt = ' + @frmla 
	set @stmt = @stmt + '	   select @paid_erp_amt = 0'


	-- Handling for LDF / IBNR limit indicator
	set @stmt = @stmt + ' if @ldf_ibnr_lim_ind = 1'
	set @stmt = @stmt + ' begin'
	set @stmt = @stmt + '	select @exc_ldf_ibnr_amt = sum(isnull(exc_ldf_ibnr_amt,0)), @subj_ldf_ibnr_amt =  sum(isnull(subj_ldf_ibnr_amt,0)) from ARMIS_LOS_POL where prem_adj_pgm_id = @premium_adj_prog_id and coml_agmt_id = @com_agm_id and st_id = @state_id and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) and valn_dt = @prem_adj_valn_dt and actv_ind = 1 '
	set @stmt = @stmt + '	select @exc_ldf_ibnr_amt = isnull(@exc_ldf_ibnr_amt,0) '
	set @stmt = @stmt + '	select @subj_ldf_ibnr_amt = isnull(@subj_ldf_ibnr_amt,0) '


	set @stmt = @stmt + '  if (@has_lcf = 1)'
	set @stmt = @stmt + '	select @inc_erp_amt = @inc_erp_amt - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf - @subj_ldf_ibnr_amt)*(@tm)'
	set @stmt = @stmt + '  else'
	set @stmt = @stmt + '	select @inc_erp_amt = @inc_erp_amt - @exc_ldf_ibnr_amt - @exc_ldf_ibnr_amt*(@tm - 1) '

	set @stmt = @stmt + '  if (@has_lcf = 1)'
	set @stmt = @stmt + '	select @tm_result = @tm_result - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf - @subj_ldf_ibnr_amt)*(@tm - 1)'
	set @stmt = @stmt + '  else'
	set @stmt = @stmt + '	select @tm_result = @tm_result - @exc_ldf_ibnr_amt*(@tm - 1)'

	set @stmt = @stmt + ' end'

	set @stmt = @stmt + '	   select @cfb_amt = 0'
	set @stmt = @stmt + ' end'

	if (@pgm_type_id = 451) -- lkup type: PROGRAM TYPE; lookup value: WA ADJUSTMENT
	begin
		-- Lookup Type: 'Adjustment type'; lookup value: 'Paid Loss WA'
		set @stmt = @stmt + ' else if (@adj_typ_id = 73) 
								begin'
		set @stmt = @stmt + ' exec @tm = [dbo].[fn_RetrieveTM_ByPolState] @p_cust_id = @customer_id,@p_prem_adj_prog_id = @premium_adj_prog_id,@p_comm_agr_id = @com_agm_id, @p_state_id = @state_id'
		--Debug info
		--set @stmt = @stmt + ' print ' + char(39) + ' retrieved @tm BY STATE IN 73:= ' + char(39) + ' + convert(varchar(20),@tm);'

		if (@frmla_id = 22) --Adjustable WCD - WA
		begin
			set @stmt = @stmt + @lcf_sub_formula

			set @stmt = @stmt + ' exec @paid_rml_amt = [dbo].[fn_RetrievePaidRML_Amt] @premium_adj_period_id = @premium_adj_period_id,@premium_adjustment_id = @premium_adjustment_id,@customer_id = @customer_id,@premium_adj_prog_id = @premium_adj_prog_id,@coml_agmt_id  = @com_agm_id,@lob_id = @lob_id,@state_id  = @state_id,@subj_paid_idnmty_amt  = @subj_paid_idnmty_amt,@subj_resrv_idnmty_amt  = @subj_resrv_idnmty_amt,@subj_paid_exps_amt  = @subj_paid_exps_amt,@subj_resrv_exps_amt  = @subj_resrv_exps_amt,@lcf = @lcf,@lba  = @lba_result,@tm  = @tm,@create_user_id  = @create_user_id'
			set @stmt = @stmt + ' set @rml = @paid_rml_amt'
			--Debug info
			--set @stmt = @stmt + ' print ' + char(39) + ' For WA @paid_rml_amt:= ' + char(39) + ' + convert(varchar(20),@paid_rml_amt);'
			set @stmt = @stmt + '	select @paid_erp_amt = ' + @paid_erp_frmla + '- (@subj_paid_idnmty_amt)*@tm'

			set @stmt = @stmt + ' exec @earned_resd_amt = [dbo].[fn_RetrieveEarnedResidual_Amt] @premium_adj_period_id = @premium_adj_period_id,@premium_adjustment_id = @premium_adjustment_id,@customer_id = @customer_id,@premium_adj_prog_id = @premium_adj_prog_id,@coml_agmt_id  = @com_agm_id,@lob_id = @lob_id,@state_id  = @state_id,@subj_paid_idnmty_amt  = @subj_paid_idnmty_amt,@subj_resrv_idnmty_amt  = @subj_resrv_idnmty_amt,@subj_paid_exps_amt  = @subj_paid_exps_amt,@subj_resrv_exps_amt  = @subj_resrv_exps_amt,@lcf = @lcf,@lba  = @lba_result,@lcf_result  = @lcf_result,@create_user_id  = @create_user_id'
			set @stmt = @stmt + ' set @rml = @earned_resd_amt'
			--Debug info
			--set @stmt = @stmt + ' print ' + char(39) + ' For WA @earned_resd_amt:= ' + char(39) + ' + convert(varchar(20),@earned_resd_amt);'
			set @stmt = @stmt + '	select @inc_erp_amt = ' + @frmla + '- (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*@tm'

			set @stmt = @stmt + @tax_formula
			set @stmt = @stmt + '		select @tm_result = @tm_result - (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*(@tm - 1)'

		end


		-- Handling for LDF / IBNR limit indicator
		set @stmt = @stmt + ' if @ldf_ibnr_lim_ind = 1'
		set @stmt = @stmt + ' begin'
		set @stmt = @stmt + '	select @exc_ldf_ibnr_amt = sum(isnull(exc_ldf_ibnr_amt,0)), @subj_ldf_ibnr_amt =  sum(isnull(subj_ldf_ibnr_amt,0)) from ARMIS_LOS_POL where prem_adj_pgm_id = @premium_adj_prog_id and coml_agmt_id = @com_agm_id and st_id = @state_id and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) and valn_dt = @prem_adj_valn_dt and actv_ind = 1 '
		set @stmt = @stmt + '	select @exc_ldf_ibnr_amt = isnull(@exc_ldf_ibnr_amt,0) '
		set @stmt = @stmt + '	select @subj_ldf_ibnr_amt = isnull(@subj_ldf_ibnr_amt,0) '


		set @stmt = @stmt + '  if (@has_lcf = 1)'
		set @stmt = @stmt + '	select @inc_erp_amt = @inc_erp_amt - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf - @subj_ldf_ibnr_amt)*(@tm)'
		set @stmt = @stmt + '  else'
		set @stmt = @stmt + '	select @inc_erp_amt = @inc_erp_amt - @exc_ldf_ibnr_amt - @exc_ldf_ibnr_amt*(@tm - 1) '

		set @stmt = @stmt + '  if (@has_ldf = 1)'
		set @stmt = @stmt + '  begin'
		set @stmt = @stmt + '   exec @paid_ratio = [dbo].[fn_ComputePaidExcessRatioByPolicy] @customer_id = @customer_id,@premium_adj_prog_id = @premium_adj_prog_id,@prem_adj_id=@premium_adjustment_id,@coml_agmt_id = @com_agm_id,@state_id = @state_id'
		set @stmt = @stmt + '  if (@has_lcf = 1)'
		set @stmt = @stmt + '	select @paid_erp_amt = @paid_erp_amt - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf*@paid_ratio - @subj_ldf_ibnr_amt*@paid_ratio)*(@tm)'
		set @stmt = @stmt + '  else'
		set @stmt = @stmt + '	select @paid_erp_amt = @paid_erp_amt - (@exc_ldf_ibnr_amt*@paid_ratio) - (@exc_ldf_ibnr_amt*@paid_ratio)*(@tm - 1) '
		set @stmt = @stmt + '  end'

		set @stmt = @stmt + '  if (@has_lcf = 1)'
		set @stmt = @stmt + '	select @tm_result = @tm_result - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf - @subj_ldf_ibnr_amt)*(@tm - 1)'
		set @stmt = @stmt + '  else'
		set @stmt = @stmt + '	select @tm_result = @tm_result - @exc_ldf_ibnr_amt*(@tm - 1)'

		set @stmt = @stmt + ' end'


		set @stmt = @stmt + '		select @cfb_amt = (@inc_erp_amt - @paid_erp_amt) + @subj_resrv_idnmty_amt
									select @adj_ded_wc_loss_amt = @subj_paid_idnmty_amt + @subj_resrv_idnmty_amt
						 		end'
		-- Lookup Type: 'Adjustment type'; lookup value: 'Incurred Loss WA'
		set @stmt = @stmt + ' else if (@adj_typ_id = 66) 
								begin'
		set @stmt = @stmt + ' exec @tm = [dbo].[fn_RetrieveTM_ByPolState] @p_cust_id = @customer_id,@p_prem_adj_prog_id = @premium_adj_prog_id,@p_comm_agr_id = @com_agm_id, @p_state_id = @state_id'
		--Debug info
		--set @stmt = @stmt + ' print ' + char(39) + ' retrieved @tm BY STATE IN 66:= ' + char(39) + ' + convert(varchar(20),@tm);'

		if (@frmla_id = 22) --Adjustable WCD - WA
		begin
			set @stmt = @stmt + @lcf_sub_formula

			--set @stmt = @stmt + ' exec @paid_rml_amt = [dbo].[fn_RetrievePaidRML_Amt] @premium_adj_period_id = @premium_adj_period_id,@premium_adjustment_id = @premium_adjustment_id,@customer_id = @customer_id,@premium_adj_prog_id = @premium_adj_prog_id,@coml_agmt_id  = @com_agm_id,@lob_id = @lob_id,@state_id  = @state_id,@subj_paid_idnmty_amt  = @subj_paid_idnmty_amt,@subj_resrv_idnmty_amt  = @subj_resrv_idnmty_amt,@subj_paid_exps_amt  = @subj_paid_exps_amt,@subj_resrv_exps_amt  = @subj_resrv_exps_amt,@lcf = @lcf,@lba  = @lba_result,@tm  = @tm,@create_user_id  = @create_user_id'
			--Debug info
			--set @stmt = @stmt + ' print ' + char(39) + ' For WA @paid_rml_amt:= ' + char(39) + ' + convert(varchar(20),@paid_rml_amt);'

			set @stmt = @stmt + ' exec @earned_resd_amt = [dbo].[fn_RetrieveEarnedResidual_Amt] @premium_adj_period_id = @premium_adj_period_id,@premium_adjustment_id = @premium_adjustment_id,@customer_id = @customer_id,@premium_adj_prog_id = @premium_adj_prog_id,@coml_agmt_id  = @com_agm_id,@lob_id = @lob_id,@state_id  = @state_id,@subj_paid_idnmty_amt  = @subj_paid_idnmty_amt,@subj_resrv_idnmty_amt  = @subj_resrv_idnmty_amt,@subj_paid_exps_amt  = @subj_paid_exps_amt,@subj_resrv_exps_amt  = @subj_resrv_exps_amt,@lcf = @lcf,@lba  = @lba_result,@lcf_result  = @lcf_result,@create_user_id  = @create_user_id '
			set @stmt = @stmt + ' set @rml = @earned_resd_amt'

			set @stmt = @stmt + '	select @inc_erp_amt = ' + @frmla + '- (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*@tm'

			set @stmt = @stmt + @tax_formula
			set @stmt = @stmt + '		select @tm_result = @tm_result - (@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt)*(@tm - 1)'
		end


		-- Handling for LDF / IBNR limit indicator
		set @stmt = @stmt + ' if @ldf_ibnr_lim_ind = 1'
		set @stmt = @stmt + ' begin'
		set @stmt = @stmt + '	select @exc_ldf_ibnr_amt = sum(isnull(exc_ldf_ibnr_amt,0)), @subj_ldf_ibnr_amt =  sum(isnull(subj_ldf_ibnr_amt,0)) from ARMIS_LOS_POL where prem_adj_pgm_id = @premium_adj_prog_id and coml_agmt_id = @com_agm_id and st_id = @state_id and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) and valn_dt = @prem_adj_valn_dt and actv_ind = 1 '
		set @stmt = @stmt + '	select @exc_ldf_ibnr_amt = isnull(@exc_ldf_ibnr_amt,0) '
		set @stmt = @stmt + '	select @subj_ldf_ibnr_amt = isnull(@subj_ldf_ibnr_amt,0) '


		set @stmt = @stmt + '  if (@has_lcf = 1)'
		set @stmt = @stmt + '	select @inc_erp_amt = @inc_erp_amt - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf - @subj_ldf_ibnr_amt)*(@tm)'
		set @stmt = @stmt + '  else'
		set @stmt = @stmt + '	select @inc_erp_amt = @inc_erp_amt - @exc_ldf_ibnr_amt - @exc_ldf_ibnr_amt*(@tm - 1) '

		set @stmt = @stmt + '  if (@has_lcf = 1)'
		set @stmt = @stmt + '	select @tm_result = @tm_result - ((@exc_ldf_ibnr_amt + @subj_ldf_ibnr_amt)*@lcf - @subj_ldf_ibnr_amt)*(@tm - 1)'
		set @stmt = @stmt + '  else'
		set @stmt = @stmt + '	select @tm_result = @tm_result - @exc_ldf_ibnr_amt*(@tm - 1)'

		set @stmt = @stmt + ' end'


		set @stmt = @stmt + '		select @paid_erp_amt = 0
									select @cfb_amt = 0
									select @adj_ded_wc_loss_amt = @subj_paid_idnmty_amt + @subj_resrv_idnmty_amt
						 		end'

	end --end of: if (@pgm_type_id = 451)

	
	if (@lcf_aggr_cap_set_pgm_amt <>0) --Tax
		begin
			set @stmt = @stmt + '  if (@has_lcf = 1)'
			set @stmt = @stmt + ' begin'
			set @stmt = @stmt + @tax_formula
			set @stmt = @stmt + '		select @tm_result = @tm_result - ((@subj_paid_idnmty_amt + @subj_resrv_idnmty_amt + @subj_paid_exps_amt + @subj_resrv_exps_amt)*(@lcf-1)*(@tm-1))+((isnull(@lcf_aggr_cap_amt,0))*(@tm-1))'
			set @stmt = @stmt + ' end'

		end

	set @stmt = @stmt + ' set @incur_erp_diff = 0'
	set @stmt = @stmt + ' set @adj_incur_erp_minmax = @inc_erp_amt - isnull(@incur_erp_diff,0)'
	set @stmt = @stmt + ' set @adj_cfb_diff = 0'
	set @stmt = @stmt + ' set @adj_cfb_minmax = @cfb_amt - isnull(@adj_cfb_diff,0)'


	set @stmt = @stmt + @sub_formulas 

	--Persist calculation results in the database
	if(@pgm_type = 1) --DEP Program Type
	begin
		if @debug = 1
		begin
		print 'DEP PROG TYPE'
		end
		set @stmt = @stmt + ' if not exists(select * from dbo.PREM_ADJ_RETRO where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and prem_adj_pgm_id = @premium_adj_prog_id ) begin if(@is_not_master = 0) begin exec [dbo].[AddPREM_ADJ_RETRO] @premium_adj_period_id = @premium_adj_period_id,@premium_adjustment_id = @premium_adjustment_id,@customer_id = @customer_id,@coml_agmt_id = @com_agm_id,@premium_adj_prog_id = @premium_adj_prog_id,@create_user_id = @create_user_id,@prem_adj_retro_id_op = @prem_adj_retro_id  output end end else begin select @prem_adj_retro_id = prem_adj_retro_id from dbo.PREM_ADJ_RETRO where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and prem_adj_pgm_id = @premium_adj_prog_id end'
	end
	else
	begin
		if @debug = 1
		begin
		print 'NON-DEP PROG TYPE'
		end
		set @stmt = @stmt + ' if not exists(select * from dbo.PREM_ADJ_RETRO where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and coml_agmt_id = @com_agm_id )'
		set @stmt = @stmt + ' begin exec [dbo].[AddPREM_ADJ_RETRO] @premium_adj_period_id = @premium_adj_period_id,@premium_adjustment_id = @premium_adjustment_id,@customer_id = @customer_id,@coml_agmt_id = @com_agm_id,@premium_adj_prog_id = @premium_adj_prog_id,@create_user_id = @create_user_id,@prem_adj_retro_id_op = @prem_adj_retro_id  output end else begin select @prem_adj_retro_id = prem_adj_retro_id from dbo.PREM_ADJ_RETRO where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and coml_agmt_id = @com_agm_id end '
	end
	set @stmt = @stmt + ' exec [dbo].[AddPREM_ADJ_RETRO_DTL] @prem_adj_retro_id = @prem_adj_retro_id, @premium_adj_period_id = @premium_adj_period_id,@premium_adjustment_id = @premium_adjustment_id,@customer_id = @customer_id,@premium_adj_prog_id = @premium_adj_prog_id,@state_id = @state_id,@subj_paid_idnmty_amt = @subj_paid_idnmty_amt,@subj_paid_exps_amt = @subj_paid_exps_amt,@subj_resrv_idnmty_amt = @subj_resrv_idnmty_amt,@subj_resrv_exps_amt = @subj_resrv_exps_amt,@basic_amt = @basic,@incur_erp_amt = @inc_erp_amt,@paid_erp_amt = @paid_erp_amt,@cfb_amt = @cfb_amt,@create_user_id = @create_user_id,@elp_amt = @elp,@lcf_amt = @lcf_result,@tax_amt = @tm_result,@chf_amt = @chf_amt,@lba_amt = @lba_result,@ncf_amt = @ncf_amt,@other_amt = @oa_amt,@ibnr_ldf_result=@ibnr_ldf_result,@adj_ded_wc_loss_amt=@adj_ded_wc_loss_amt,@prem_adj_retro_dtl_id_op = @prem_adj_retro_dtl_id  output,@coml_agmt_id=@com_agm_id,@std_sub_prem_amount=@std_sub_prem_amount,@prem_asses_amount=@pa_result,@earned_resd_amt=@earned_resd_amt,@paid_rml_amt=@paid_rml_amt,@lob_id=@lob_id,@lcf=@lcf'

	--Functional Modules
	--set @func_mod_str = @func_mod_str + ' print ' + char(39) + ' IDENTITY @prem_adj_retro_dtl_id:= ' + char(39) + ' + convert(varchar(20),@prem_adj_retro_dtl_id);'
	set @func_mod_str = @func_mod_str + ' exec @lcf = [dbo].[fn_RetrieveLCF] @p_cust_id = @customer_id,@p_prem_adj_prog_id = @premium_adj_prog_id,@p_comm_agr_id = @com_agm_id, @p_lob_id = @lob_id, @p_state_id = @state_id'

	--Kentucky / Oregon
	set @func_mod_str = @func_mod_str + ' if( ((@state_id = 20) or (@state_id = 40)) and (@lob_id = 428) and ((@adj_typ_id = 71) or (@adj_typ_id = 65) or (@adj_typ_id = 67) or (@adj_typ_id = 72)) )'
	set @func_mod_str = @func_mod_str + '	begin'
	set @func_mod_str = @func_mod_str + '		exec [dbo].[ModAISCalcKY_OR] @premium_adj_period_id = @premium_adj_period_id,@premium_adjustment_id = @premium_adjustment_id,@customer_id = @customer_id,@premium_adj_prog_id = @premium_adj_prog_id,@coml_agmt_id  = @com_agm_id,@state_id  = @state_id,@prem_adj_retro_det_id  = @prem_adj_retro_dtl_id,@incur_erp_amt  = @inc_erp_amt,@subj_paid_idnmty_amt  = @subj_paid_idnmty_amt,@subj_resrv_idnmty_amt  = @subj_resrv_idnmty_amt,@subj_paid_exps_amt  = @subj_paid_exps_amt,@subj_resrv_exps_amt  = @subj_resrv_exps_amt,@lcf = @lcf,@basic  = @basic,@tm  = @tm,@create_user_id  = @create_user_id'
	set @func_mod_str = @func_mod_str + '	end'

	--RML
	if (@pgm_type_id <> 451) -- lkup type: PROGRAM TYPE; lookup value: WA ADJUSTMENT
	begin
		set @func_mod_str = @func_mod_str + ' exec [dbo].[ModAISCalcRML] @premium_adj_period_id = @premium_adj_period_id,@premium_adjustment_id = @premium_adjustment_id,@customer_id = @customer_id,@premium_adj_prog_id = @premium_adj_prog_id,@coml_agmt_id  = @com_agm_id,@lob_id = @lob_id,@state_id  = @state_id,@prem_adj_retro_det_id  = @prem_adj_retro_dtl_id,@incur_erp_amt  = @inc_erp_amt,@subj_paid_idnmty_amt  = @subj_paid_idnmty_amt,@subj_resrv_idnmty_amt  = @subj_resrv_idnmty_amt,@subj_paid_exps_amt  = @subj_paid_exps_amt,@subj_resrv_exps_amt  = @subj_resrv_exps_amt,@lcf = @lcf,@basic  = @basic,@tm  = @tm,@create_user_id  = @create_user_id'
	end
	else
	begin
		set @func_mod_str = @func_mod_str + ' exec [dbo].[ModAISCalcRML_WA] @premium_adj_period_id = @premium_adj_period_id,@premium_adjustment_id = @premium_adjustment_id,@customer_id = @customer_id,@premium_adj_prog_id = @premium_adj_prog_id,@coml_agmt_id  = @com_agm_id,@lob_id = @lob_id,@state_id  = @state_id,@prem_adj_retro_det_id  = @prem_adj_retro_dtl_id,@incur_erp_amt  = @inc_erp_amt,@subj_paid_idnmty_amt  = @subj_paid_idnmty_amt,@subj_resrv_idnmty_amt  = @subj_resrv_idnmty_amt,@subj_paid_exps_amt  = @subj_paid_exps_amt,@subj_resrv_exps_amt  = @subj_resrv_exps_amt,@lcf = @lcf,@basic  = @basic,@tm  = @tm,@create_user_id  = @create_user_id'
	end


	set @stmt = @stmt + @func_mod_str 

	--Debug info
	if @debug = 1
	begin
	set @stmt = @stmt + ' print ' + char(39) + ' computed @basic:= ' + char(39) + ' + convert(varchar(20),@basic);'
	set @stmt = @stmt + ' print ' + char(39) + ' computed @elp:= ' + char(39) + ' + convert(varchar(20),@elp);'
	set @stmt = @stmt + ' print ' + char(39) + ' computed @subj_paid_idnmty_amt:= ' + char(39) + ' + convert(varchar(20),@subj_paid_idnmty_amt);'
	set @stmt = @stmt + ' print ' + char(39) + ' computed @subj_paid_exps_amt:= ' + char(39) + ' + convert(varchar(20),@subj_paid_exps_amt);'
	set @stmt = @stmt + ' print ' + char(39) + ' retrieved factor @ldf:= ' + char(39) + ' + convert(varchar(20),@ldf);'
	set @stmt = @stmt + ' print ' + char(39) + ' retrieved factor @lcf:= ' + char(39) + ' + convert(varchar(20),@lcf);'
	set @stmt = @stmt + ' print ' + char(39) + ' retrieved factor @ibnr:= ' + char(39) + ' + convert(varchar(20),@ibnr);'
	set @stmt = @stmt + ' print ' + char(39) + ' retrieved @tm:= ' + char(39) + ' + convert(varchar(20),@tm);'
	set @stmt = @stmt + ' print ' + char(39) + ' retrieved @lcf:= ' + char(39) + ' + convert(varchar(20),@lcf);'
	set @stmt = @stmt + ' print ' + char(39) + ' retrieved @premium_adj_prog_id:= ' + char(39) + ' + convert(varchar(20),@premium_adj_prog_id);'

	set @stmt = @stmt + ' print ' + char(39) + ' computed @chf_amt:= ' + char(39) + ' + convert(varchar(20),@chf_amt);'
	set @stmt = @stmt + ' print ' + char(39) + ' computed @ncf_amt:= ' + char(39) + ' + convert(varchar(20),@ncf_amt);'
	set @stmt = @stmt + ' print ' + char(39) + ' computed @lba_result:= ' + char(39) + ' + convert(varchar(20),@lba_result);'
	set @stmt = @stmt + ' print ' + char(39) + ' computed @lcf_result:= ' + char(39) + ' + convert(varchar(20),@lcf_result);'
	set @stmt = @stmt + ' print ' + char(39) + ' computed @tm_result:= ' + char(39) + ' + convert(varchar(20),@tm_result);'
	set @stmt = @stmt + ' print ' + char(39) + ' computed @pa_result:= ' + char(39) + ' + convert(varchar(20),@pa_result);'
	set @stmt = @stmt + ' print ' + char(39) + ' computed @rml:= ' + char(39) + ' + convert(varchar(20),@rml);'
	set @stmt = @stmt + ' print ' + char(39) + ' computed @lcf_aggr_cap:= ' + char(39) + ' + convert(varchar(20),@lcf_aggr_cap);'
	set @stmt = @stmt + ' print ' + char(39) + ' computed @oa_amt:= ' + char(39) + ' + convert(varchar(20),@oa_amt);'
	set @stmt = @stmt + ' print ' + char(39) + ' computed @ibnr_ldf_result:= ' + char(39) + ' + convert(varchar(20),@ibnr_ldf_result);'
	set @stmt = @stmt + ' print ' + char(39) + ' computed @lcf_aggr_cap_amt:= ' + char(39) + ' + convert(varchar(20),@lcf_aggr_cap_amt);'
	
	set @stmt = @stmt + ' print ' + char(39) + ' computed @inc_erp_amt:= ' + char(39) + ' + convert(varchar(20),@inc_erp_amt);'
	set @stmt = @stmt + ' print ' + char(39) + ' computed @paid_erp_amt:= ' + char(39) + ' + convert(varchar(20),@paid_erp_amt);'
	set @stmt = @stmt + ' print ' + char(39) + ' computed @cfb_amt:= ' + char(39) + ' + convert(varchar(20),@cfb_amt);'

	print 'Final sql string: ' + @stmt
	select 'Final sql string: ' , @stmt
	end
	--Validation for existence for audit for associated policies
	declare @PolicyList table (  PolicyID varchar(50)  )
      
    set @PolicyIDList = ''
	if (@pgm_type = 1 ) --DEP Program Type
	begin
		-- Only DEP master policies have audits; DEP underlying policies need not have
		-- have audit assoicated with them.
		insert into @PolicyList
		select rtrim(ltrim(pol_sym_txt)) + '-' + rtrim(ltrim(pol_nbr_txt)) + '-' + rtrim(ltrim(pol_modulus_txt))
		from dbo.COML_AGMT
		where prem_adj_pgm_id = @premium_adj_prog_id
		and coml_agmt_id not in 
		(	
			select 
				spa.coml_agmt_id
				from dbo.COML_AGMT ca
				inner join dbo.COML_AGMT_AUDT caa on (ca.coml_agmt_id = caa.coml_agmt_id) and (ca.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (ca.custmr_id = caa.custmr_id)
				inner join dbo.SUBJ_PREM_AUDT spa on (spa.coml_agmt_audt_id = caa.coml_agmt_audt_id) and (spa.coml_agmt_id = caa.coml_agmt_id) and (spa.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (spa.custmr_id = caa.custmr_id)
				where ca.prem_adj_pgm_id =  @premium_adj_prog_id
				and ca.custmr_id = @customer_id
				and (caa.audt_revd_sts_ind = 0 or caa.audt_revd_sts_ind is null)
				and ca.actv_ind = 1
				and spa.actv_ind = 1
		)
		and coml_agmt_id in 
		(	
			select coml_agmt_id from dbo.COML_AGMT where prem_adj_pgm_id =  @premium_adj_prog_id and pol_sym_txt = 'DEP' and actv_ind = 1
		)
		and adj_typ_id not in (62,64,68,69,448) -- Exclude all non-ERP adjustment types
		and actv_ind = 1
		
		
		if @@ROWCOUNT > 0
			update @PolicyList
			set @PolicyIDList = ( @PolicyIDList + PolicyID + ', ' )
		if(len( @PolicyIDList ) > 1)
			set @policy_str = substring( @PolicyIDList, 1, ( len( @PolicyIDList ) - 1 ))

	end
	else
	begin  --Non-DEP Program Type
		insert into @PolicyList
		select rtrim(ltrim(pol_sym_txt)) + '-' + rtrim(ltrim(pol_nbr_txt)) + '-' + rtrim(ltrim(pol_modulus_txt))
		from dbo.COML_AGMT
		where prem_adj_pgm_id = @premium_adj_prog_id
		and coml_agmt_id not in 
		(	
			select caa.coml_agmt_id
			from dbo.COML_AGMT_AUDT caa
			inner join dbo.SUBJ_PREM_AUDT spa on (spa.coml_agmt_audt_id = caa.coml_agmt_audt_id) and (spa.coml_agmt_id = caa.coml_agmt_id) and (spa.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (spa.custmr_id = caa.custmr_id) and (spa.actv_ind = 1)
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = caa.coml_agmt_id) and (ca.actv_ind = 1)
			where ca.prem_adj_pgm_id = @premium_adj_prog_id
		)
		and adj_typ_id not in (62,64,68,69,448) -- Exclude all non-ERP adjustment types
		and actv_ind = 1

		if @@ROWCOUNT > 0
			update @PolicyList
			set @PolicyIDList = ( @PolicyIDList + PolicyID + ', ' )
		if(len( @PolicyIDList ) > 1)
			set @policy_str = substring( @PolicyIDList, 1, ( len( @PolicyIDList ) - 1 ))
	end

	--print @policy_str
	--TODO:

	if (@policy_str is not null)
	begin
		set @err_message = 'ERP: Audit (or) Subject premium does not exist for the following policies: '  + @policy_str  +  '; For customer ID: ' + convert(varchar(20),@customer_id) + ';Program Period ID: '  + convert(varchar(20),@premium_adj_prog_id)
        if (@pgm_type = 1 ) --DEP Program Type
           begin
		    rollback transaction ModAISCalcERP
           end
		set @err_msg_op = @err_message
		exec [dbo].[AddAPLCTN_STS_LOG] 
			@premium_adjustment_id = @premium_adjustment_id,
			@customer_id = @customer_id,
			@premium_adj_prog_id = @premium_adj_prog_id,
			@err_msg = @err_message,
			@create_user_id = @create_user_id
		if (@pgm_type = 1 ) --DEP Program Type
           begin
		    return
           end
		
	end	

	--Retrieve basic amount from setup
	select
	@basic_pre_prop_amt = case when isnull(tot_agmt_amt,0) > isnull(tot_audt_amt,0) then tot_agmt_amt else tot_audt_amt end
	from dbo.PREM_ADJ_PGM_RETRO
	where custmr_id = @customer_id
	and prem_adj_pgm_id = @premium_adj_prog_id
	and retro_elemt_typ_id = 334 --Lookup Type: RETROSPECTIVE ELEMENT TYPE; lookup value: Basic
	and retro_adj_fctr_aplcbl_ind = 0
	and actv_ind = 1

	if(@has_basic = 0)
	begin
		set @basic_pre_prop_amt = 0
	end

	set @basic_pre_prop_amt = isnull(@basic_pre_prop_amt,0)
	if @debug = 1
	begin
	print '@basic_pre_prop_amt: ' + convert(varchar(20),@basic_pre_prop_amt)
	end

	--Retrieve ELP amount from setup
	select
	@elp_pre_prop_amt = case when isnull(tot_agmt_amt,0) > isnull(tot_audt_amt,0) then tot_agmt_amt else tot_audt_amt end
	from dbo.PREM_ADJ_PGM_RETRO
	where custmr_id = @customer_id
	and prem_adj_pgm_id = @premium_adj_prog_id
	and retro_elemt_typ_id = 335 --Lookup Type: RETROSPECTIVE ELEMENT TYPE; lookup value: 'Excess Loss Premium'
	and retro_adj_fctr_aplcbl_ind = 0
	and actv_ind = 1

	if(@is_elp = 0)
	begin
		set @elp_pre_prop_amt = null
	end

	--set @elp_pre_prop_amt = isnull(@elp_pre_prop_amt,0)

	if (@has_chf = 1)
	begin
		select
		@chf_amt = isnull(o.tot_amt,0)
		from
		dbo.[PREM_ADJ_PARMET_SETUP] o
		inner join dbo.PREM_ADJ_PGM_SETUP i on (i.prem_adj_pgm_setup_id = o.prem_adj_pgm_setup_id) and (i.prem_adj_pgm_id = o.prem_adj_pgm_id) and (i.custmr_id = o.custmr_id) and (i.adj_parmet_typ_id = o.adj_parmet_typ_id)
		where o.custmr_id = @customer_id
		and o.prem_adj_pgm_id = @premium_adj_prog_id
		and o.prem_adj_id = @premium_adjustment_id
		and o.prem_adj_perd_id = @premium_adj_period_id
		and i.incld_ernd_retro_prem_ind = 1
		and o.adj_parmet_typ_id = 398 -- Adj. Parameter type ID for 'ADJUSTMENT PARAMET': CHF
		and i.actv_ind = 1
	end
			
	declare erp_base_cur cursor LOCAL FAST_FORWARD READ_ONLY 
		for 
		select  
		md.com_agr_id ,
		md.state_id,
		md.subj_paid_idnmty_amt ,
		md.subj_paid_exps_amt ,
		md.subj_resrv_idnmty_amt ,
		md.subj_resrv_exps_amt,
		case when c.pol_sym_txt = 'DEP' then 0 else 1 end as is_not_master
		from
		(
			select  
			case when aud.coml_agmt_id is not null then aud.coml_agmt_id else los.coml_agmt_id end as com_agr_id ,
			case when aud.st_id is not null then aud.st_id else los.st_id end as state_id,
			los.subj_paid_idnmty_amt ,
			los.subj_paid_exps_amt ,
			los.subj_resrv_idnmty_amt ,
			los.subj_resrv_exps_amt 
			from
			(
				select 
				spa.coml_agmt_id,
				spa.st_id,
				spa.prem_amt
				from dbo.COML_AGMT ca
				inner join dbo.COML_AGMT_AUDT caa on (ca.coml_agmt_id = caa.coml_agmt_id) and (ca.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (ca.custmr_id = caa.custmr_id)
				inner join dbo.SUBJ_PREM_AUDT spa on (spa.coml_agmt_audt_id = caa.coml_agmt_audt_id) and (spa.coml_agmt_id = caa.coml_agmt_id) and (spa.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (spa.custmr_id = caa.custmr_id)
				where ca.prem_adj_pgm_id = @premium_adj_prog_id
				and ca.custmr_id = @customer_id
				and (caa.audt_revd_sts_ind = 0 or caa.audt_revd_sts_ind is null)
				and ca.actv_ind = 1
				and spa.actv_ind = 1
			) aud
			full outer join 
			(
				select 				
				prem_adj_pgm_id, 				
				custmr_id, 				
				coml_agmt_id, 				
				st_id, 				
				isnull(sum(subj_paid_idnmty_amt),0) as subj_paid_idnmty_amt , 				
				isnull(sum(subj_paid_exps_amt),0) as   subj_paid_exps_amt ,			
				isnull(sum(subj_resrv_idnmty_amt),0) as subj_resrv_idnmty_amt , 				
				isnull(sum(subj_resrv_exps_amt),0) as subj_resrv_exps_amt 				
				from 				
				dbo.ARMIS_LOS_POL 				
				where prem_adj_pgm_id =@premium_adj_prog_id 				
				and custmr_id = @customer_id 				
				and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) 
				and valn_dt	= @prem_adj_valn_dt -- Triage # 67			
				and actv_ind = 1  
				group by  st_id,coml_agmt_id,custmr_id,prem_adj_pgm_id
			) los
			on (los.coml_agmt_id = aud.coml_agmt_id) and (los.st_id = aud.st_id)
		) md
		inner join COML_AGMT c on (md.com_agr_id = c.coml_agmt_id) and (c.actv_ind = 1)
		where c.adj_typ_id not in (62,64,68,69,448)
		order by is_not_master,com_agr_id


		open erp_base_cur
		fetch erp_base_cur into @com_agm_id, @state_id, 
			@subj_paid_idnmty_amt, @subj_paid_exps_amt, @subj_resrv_idnmty_amt, @subj_resrv_exps_amt, @is_not_master


		while @@Fetch_Status = 0
			begin
				begin
				    
					if @debug = 1
					begin
					print'*******************ERP CALC: START OF ITERATION IN FIRST PASS*********' 
				    print'---------------Input Params-------------------' 

					print' @com_agm_id:- ' + convert(varchar(20), @com_agm_id)  
					print' @state_id:- ' + convert(varchar(20), @state_id)  
					print' @customer_id: ' + convert(varchar(20), @customer_id)
					print' @premium_adj_prog_id: ' + convert(varchar(20), @premium_adj_prog_id )  
					end

					-- Handle potential null values
					set @subj_paid_idnmty_amt = isnull(@subj_paid_idnmty_amt,0)
					set @subj_paid_exps_amt = isnull(@subj_paid_exps_amt,0)
					set @subj_resrv_idnmty_amt = isnull(@subj_resrv_idnmty_amt,0)
					set @subj_resrv_exps_amt = isnull(@subj_resrv_exps_amt,0)
					set @basic_pre_prop_amt = isnull(@basic_pre_prop_amt,0)
					--set @elp_pre_prop_amt = isnull(@elp_pre_prop_amt,0)

					if @debug = 1
					begin
					print' @subj_paid_idnmty_amt: ' + convert(varchar(20), isnull(@subj_paid_idnmty_amt,0)) 
					print' @subj_paid_exps_amt: ' + convert(varchar(20), isnull(@subj_paid_exps_amt,0) )  
					print' @subj_resrv_idnmty_amt: ' + convert(varchar(20), isnull(@subj_resrv_idnmty_amt,0) )  
					print' @subj_resrv_exps_amt: ' + convert(varchar(20), isnull(@subj_resrv_exps_amt,0) )  
					end


					update #params 
						set val = @com_agm_id 
					where id = 6

					update #params 
						set val = @state_id 
					where id = 7

					update #params 
						set val = @subj_paid_idnmty_amt 
					where id = 8

					update #params 
						set val = @subj_paid_exps_amt 
					where id = 9

					update #params 
						set val = @subj_resrv_idnmty_amt 
					where id = 10

					update #params 
						set val = @subj_resrv_exps_amt 
					where id = 11

					update #params 
						set val = @has_ldf 
					where id = 22 --Has LDF

					update #params 
						set val = @has_lcf 
					where id = 23 --Has LCF

					/****************************
					* Determine line of business
					****************************/
					
					select @lob = attr_1_txt 
					from dbo.LKUP 
					where lkup_id = (
										select 
										covg_typ_id 
										from dbo.COML_AGMT 
										where coml_agmt_id = @com_agm_id
										and prem_adj_pgm_id = @premium_adj_prog_id
										and custmr_id = @customer_id
									)
					and lkup_typ_id in (
											select 
											lkup_typ_id 
											from dbo.LKUP_TYP 
											where lkup_typ_nm_txt like 'LOB COVERAGE'
										)
					set @lob_id = dbo.fn_GetIDForLOB(@lob)
					update #params 
						set val = @lob_id 
					where id = 12 --LOB
					if @debug = 1
					begin
					print 'retrieved @lob_id: ' + convert(varchar(20), @lob_id ) 
					end
					exec @sub_aud_prem_ratio = [dbo].[fn_ComputeSubAudPremiumRatio]
   																	@customer_id = @customer_id,
																	@premium_adj_prog_id = @premium_adj_prog_id,
																	@coml_agmt_id = @com_agm_id,
																	@state_id = @state_id 

					update #params 
						set val = @basic_pre_prop_amt * (@sub_aud_prem_ratio / 100)
					where id = 13 --Basic
					

					update #params 
						set val = @elp_pre_prop_amt * (@sub_aud_prem_ratio / 100)
					where id = 19 --ELP
					
					if (@has_lcf = 1)
						begin
						select @cnt_losses=count(*) from dbo.ARMIS_LOS_POL 
						where prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and valn_dt=@prem_adj_valn_dt
						and actv_ind = 1
						and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id))
						--TODO: Verify if proportion by policy or not
						if(@cnt_losses>0)
						begin
						exec @loss_ratio = [dbo].[fn_ComputeLossRatio]
								@customer_id = @customer_id,
								@premium_adj_prog_id = @premium_adj_prog_id,
								@coml_agmt_id = @com_agm_id,
								@state_id = @state_id,
								@premium_adjustment_id = @premium_adjustment_id			
						
						update #params 
							set val = @lcf_aggr_cap_set_pgm_amt * (@loss_ratio / 100)
						where id = 24 --@lcf_aggr_cap_set_pgm_amt

						end
--						else
--						begin
--						exec @sub_aud_prem_ratio = [dbo].[fn_ComputeSubAudPremiumRatio]
--   																	@customer_id = @customer_id,
--																	@premium_adj_prog_id = @premium_adj_prog_id,
--																	@coml_agmt_id = @com_agm_id,
--																	@state_id = @state_id 
--						update #params 
--							set val = @lcf_aggr_cap_set_pgm_amt * (@sub_aud_prem_ratio / 100)
--						where id = 24 --@lcf_aggr_cap_set_pgm_amt
--
--						end
					end

					
					if (@has_chf = 1)
					begin
						select @cnt_losses=count(*) from dbo.ARMIS_LOS_POL 
						where prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and valn_dt=@prem_adj_valn_dt
						and actv_ind = 1
						and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id))
						--TODO: Verify if proportion by policy or not
						if(@cnt_losses>0)
						begin
						exec @loss_ratio = [dbo].[fn_ComputeLossRatio]
								@customer_id = @customer_id,
								@premium_adj_prog_id = @premium_adj_prog_id,
								@coml_agmt_id = @com_agm_id,
								@state_id = @state_id,
								@premium_adjustment_id = @premium_adjustment_id			
						
						update #params 
							set val = @chf_amt * (@loss_ratio / 100)
						where id = 15 --CHF
																	
						end
						else
						begin
						exec @sub_aud_prem_ratio = [dbo].[fn_ComputeSubAudPremiumRatio]
   																	@customer_id = @customer_id,
																	@premium_adj_prog_id = @premium_adj_prog_id,
																	@coml_agmt_id = @com_agm_id,
																	@state_id = @state_id 
						update #params 
							set val = @chf_amt * (@sub_aud_prem_ratio / 100)
						where id = 15 --CHF
																	
						end
					end
	
					--select * from #params
				
					exec sp_executesql @stmt
					if @debug = 1
					begin			
				    print'---------------End of iteration-------------------' 
					end
				end
				fetch erp_base_cur into @com_agm_id, @state_id, 
				@subj_paid_idnmty_amt, @subj_paid_exps_amt, @subj_resrv_idnmty_amt, @subj_resrv_exps_amt, @is_not_master

			end --end of cursor erp_base_cur / while loop
		close erp_base_cur
		deallocate erp_base_cur


		select
		@minmax_lcf_amt = isnull(sum(d.los_conv_fctr_amt),0)
		from dbo.PREM_ADJ_RETRO_DTL d
		where  d.prem_adj_id = @premium_adjustment_id
		and d.custmr_id = @customer_id
		and d.prem_adj_perd_id = @premium_adj_period_id

		if ( @lcf_aggr_cap_set_pgm_amt > @minmax_lcf_amt)
			set @recalc_lcf = 0
		if @debug = 1
		begin
		print 'retrieved @minmax_lcf_amt: ' + convert(varchar(20),@minmax_lcf_amt)
		print 'after minmax: @recalc_lcf: ' + convert(varchar(20),@recalc_lcf)
		end
		
		set @updt_stmt = ' update dbo.PREM_ADJ_RETRO_DTL WITH (ROWLOCK) set adj_incur_ernd_retro_prem_amt = @inc_erp_amt,adj_paid_ernd_retro_prem_amt = @paid_erp_amt,prem_tax_amt = @tm_result,clm_hndl_fee_amt=@chf_amt, ' 

		if (@has_ncf = 1)
		begin
			select @stmt = replace(@stmt, ' set @ncf_amt = 0',' select @ncf_amt = val from #params where id = 16')
			set @updt_stmt = @updt_stmt + 'non_conv_fee_amt = @ncf_amt,'
		end

		if (@has_oa = 1)
		begin
			select @stmt = replace(@stmt, ' set @oa_amt = 0',' select @oa_amt = val from #params where id = 17')
			set @updt_stmt = @updt_stmt + 'othr_amt = @oa_amt,'
		end

		if (@recalc_lcf = 1)
		begin
			select @stmt = replace(@stmt, ' set @lcf_aggr_cap = 0',' select @lcf_aggr_cap = val from #params where id = 18')
			--set @updt_stmt = @updt_stmt + 'los_conv_fctr_amt = @lcf_result,'
			if @debug = 1
			begin
			print 'recalcing LCF'
			end
		end

		if (@recalc_minmax_for_paid = 1)
		begin
			select @stmt = replace(@stmt, ' set @incur_erp_diff = 0',' select @incur_erp_diff = val from #params where id = 20')
			select @stmt = replace(@stmt, ' set @adj_cfb_diff = 0',' select @adj_cfb_diff = val from #params where id = 21')

			--set @updt_stmt = @updt_stmt + 'los_conv_fctr_amt = @lcf_result,'
			if @debug = 1
			begin
			print 'recalcing MinMax'
			end
		end

		if( substring(@updt_stmt,len(@updt_stmt),1) = ',')
			set @updt_stmt = left(@updt_stmt, len(@updt_stmt) - 1)
	
		if(@pgm_type = 1) --DEP Program Type
		begin
			set @updt_stmt = @updt_stmt + ' where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and custmr_id = @customer_id and coml_agmt_id = @com_agm_id and st_id = @state_id'
		end
		else
		begin
			set @updt_stmt = @updt_stmt + ' from dbo.PREM_ADJ_RETRO_DTL d inner join dbo.PREM_ADJ_RETRO h on (d.prem_adj_retro_id = h.prem_adj_retro_id) and (d.prem_adj_perd_id = h.prem_adj_perd_id) and (d.prem_adj_id = h.prem_adj_id) and (d.custmr_id = h.custmr_id) where d.prem_adj_perd_id = @premium_adj_period_id and d.prem_adj_id = @premium_adjustment_id and d.custmr_id = @customer_id and h.coml_agmt_id = @com_agm_id and d.st_id = @state_id'
		end

		if (@pgm_type = 1) --DEP Program Type
			select @stmt = replace(@stmt, 'if not exists(select * from dbo.PREM_ADJ_RETRO where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and prem_adj_pgm_id = @premium_adj_prog_id ) begin if(@is_not_master = 0) begin exec [dbo].[AddPREM_ADJ_RETRO] @premium_adj_period_id = @premium_adj_period_id,@premium_adjustment_id = @premium_adjustment_id,@customer_id = @customer_id,@coml_agmt_id = @com_agm_id,@premium_adj_prog_id = @premium_adj_prog_id,@create_user_id = @create_user_id,@prem_adj_retro_id_op = @prem_adj_retro_id  output end end else begin select @prem_adj_retro_id = prem_adj_retro_id from dbo.PREM_ADJ_RETRO where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and prem_adj_pgm_id = @premium_adj_prog_id end exec [dbo].[AddPREM_ADJ_RETRO_DTL] @prem_adj_retro_id = @prem_adj_retro_id, @premium_adj_period_id = @premium_adj_period_id,@premium_adjustment_id = @premium_adjustment_id,@customer_id = @customer_id,@premium_adj_prog_id = @premium_adj_prog_id,@state_id = @state_id,@subj_paid_idnmty_amt = @subj_paid_idnmty_amt,@subj_paid_exps_amt = @subj_paid_exps_amt,@subj_resrv_idnmty_amt = @subj_resrv_idnmty_amt,@subj_resrv_exps_amt = @subj_resrv_exps_amt,@basic_amt = @basic,@incur_erp_amt = @inc_erp_amt,@paid_erp_amt = @paid_erp_amt,@cfb_amt = @cfb_amt,@create_user_id = @create_user_id,@elp_amt = @elp,@lcf_amt = @lcf_result,@tax_amt = @tm_result,@chf_amt = @chf_amt,@lba_amt = @lba_result,@ncf_amt = @ncf_amt,@other_amt = @oa_amt,@ibnr_ldf_result=@ibnr_ldf_result,@adj_ded_wc_loss_amt=@adj_ded_wc_loss_amt,@prem_adj_retro_dtl_id_op = @prem_adj_retro_dtl_id  output,@coml_agmt_id=@com_agm_id,@std_sub_prem_amount=@std_sub_prem_amount,@prem_asses_amount=@pa_result,@earned_resd_amt=@earned_resd_amt,@paid_rml_amt=@paid_rml_amt,@lob_id=@lob_id,@lcf=@lcf',@updt_stmt)
		else
			select @stmt = replace(@stmt, 'if not exists(select * from dbo.PREM_ADJ_RETRO where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and coml_agmt_id = @com_agm_id ) begin exec [dbo].[AddPREM_ADJ_RETRO] @premium_adj_period_id = @premium_adj_period_id,@premium_adjustment_id = @premium_adjustment_id,@customer_id = @customer_id,@coml_agmt_id = @com_agm_id,@premium_adj_prog_id = @premium_adj_prog_id,@create_user_id = @create_user_id,@prem_adj_retro_id_op = @prem_adj_retro_id  output end else begin select @prem_adj_retro_id = prem_adj_retro_id from dbo.PREM_ADJ_RETRO where prem_adj_perd_id = @premium_adj_period_id and prem_adj_id = @premium_adjustment_id and coml_agmt_id = @com_agm_id end  exec [dbo].[AddPREM_ADJ_RETRO_DTL] @prem_adj_retro_id = @prem_adj_retro_id, @premium_adj_period_id = @premium_adj_period_id,@premium_adjustment_id = @premium_adjustment_id,@customer_id = @customer_id,@premium_adj_prog_id = @premium_adj_prog_id,@state_id = @state_id,@subj_paid_idnmty_amt = @subj_paid_idnmty_amt,@subj_paid_exps_amt = @subj_paid_exps_amt,@subj_resrv_idnmty_amt = @subj_resrv_idnmty_amt,@subj_resrv_exps_amt = @subj_resrv_exps_amt,@basic_amt = @basic,@incur_erp_amt = @inc_erp_amt,@paid_erp_amt = @paid_erp_amt,@cfb_amt = @cfb_amt,@create_user_id = @create_user_id,@elp_amt = @elp,@lcf_amt = @lcf_result,@tax_amt = @tm_result,@chf_amt = @chf_amt,@lba_amt = @lba_result,@ncf_amt = @ncf_amt,@other_amt = @oa_amt,@ibnr_ldf_result=@ibnr_ldf_result,@adj_ded_wc_loss_amt=@adj_ded_wc_loss_amt,@prem_adj_retro_dtl_id_op = @prem_adj_retro_dtl_id  output,@coml_agmt_id=@com_agm_id,@std_sub_prem_amount=@std_sub_prem_amount,@prem_asses_amount=@pa_result,@earned_resd_amt=@earned_resd_amt,@paid_rml_amt=@paid_rml_amt,@lob_id=@lob_id,@lcf=@lcf',@updt_stmt)
		
		if @debug = 1
		begin
		print 'Before Second Pass: Re-formulated sql string: ' + @stmt
		select 'Before Second Pass: Re-formulated sql string: ' , @stmt
		end
		/********************************************************
		* If formulas contains Non-conversion fee, Other additonal
		* amount or LCF aggregate cap applies perform recalculation 
		* of results in second pass.
		*********************************************************/

		if ( (@has_ncf = 1) or (@has_oa = 1) )
		begin
	
			declare erp_sec_pass_cur cursor LOCAL FAST_FORWARD READ_ONLY 
			for 
			select  
			md.com_agr_id ,
			md.state_id,
			md.subj_paid_idnmty_amt ,
			md.subj_paid_exps_amt ,
			md.subj_resrv_idnmty_amt ,
			md.subj_resrv_exps_amt,
			case when c.pol_sym_txt = 'DEP' then 0 else 1 end as is_not_master
			from
			(
				select  
				case when aud.coml_agmt_id is not null then aud.coml_agmt_id else los.coml_agmt_id end as com_agr_id ,
				case when aud.st_id is not null then aud.st_id else los.st_id end as state_id,
				los.subj_paid_idnmty_amt ,
				los.subj_paid_exps_amt ,
				los.subj_resrv_idnmty_amt ,
				los.subj_resrv_exps_amt 
				from
				(
					select 
					spa.coml_agmt_id,
					spa.st_id,
					spa.prem_amt
					from dbo.COML_AGMT ca
					inner join dbo.COML_AGMT_AUDT caa on (ca.coml_agmt_id = caa.coml_agmt_id) and (ca.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (ca.custmr_id = caa.custmr_id)
					inner join dbo.SUBJ_PREM_AUDT spa on (spa.coml_agmt_audt_id = caa.coml_agmt_audt_id) and (spa.coml_agmt_id = caa.coml_agmt_id) and (spa.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (spa.custmr_id = caa.custmr_id)
					where ca.prem_adj_pgm_id = @premium_adj_prog_id
					and ca.custmr_id = @customer_id
					and (caa.audt_revd_sts_ind = 0 or caa.audt_revd_sts_ind is null)
					and ca.actv_ind = 1
					and spa.actv_ind = 1
				) aud
				full outer join 
				(
					select 				
					prem_adj_pgm_id, 				
					custmr_id, 				
					coml_agmt_id, 				
					st_id, 				
					isnull(sum(subj_paid_idnmty_amt),0) as subj_paid_idnmty_amt , 				
					isnull(sum(subj_paid_exps_amt),0) as   subj_paid_exps_amt ,			
					isnull(sum(subj_resrv_idnmty_amt),0) as subj_resrv_idnmty_amt , 				
					isnull(sum(subj_resrv_exps_amt),0) as subj_resrv_exps_amt 				
					from 				
					dbo.ARMIS_LOS_POL 				
					where prem_adj_pgm_id =@premium_adj_prog_id 				
					and custmr_id = @customer_id 				
					and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) 				
					and valn_dt	= @prem_adj_valn_dt -- Triage # 67			
					and actv_ind = 1  
					group by  st_id,coml_agmt_id,custmr_id,prem_adj_pgm_id
				) los
				on (los.coml_agmt_id = aud.coml_agmt_id) and (los.st_id = aud.st_id)
			) md
			inner join COML_AGMT c on (md.com_agr_id = c.coml_agmt_id) and (c.actv_ind = 1)
			where c.adj_typ_id not in (62,64,68,69,448)
			order by is_not_master,com_agr_id

			open erp_sec_pass_cur
			fetch erp_sec_pass_cur into @com_agm_id, @state_id, 
				@subj_paid_idnmty_amt, @subj_paid_exps_amt, @subj_resrv_idnmty_amt, @subj_resrv_exps_amt, @is_not_master

			while @@Fetch_Status = 0
				begin
					begin
					
						-- Handle potential null values
						set @subj_paid_idnmty_amt = isnull(@subj_paid_idnmty_amt,0)
						set @subj_paid_exps_amt = isnull(@subj_paid_exps_amt,0)
						set @subj_resrv_idnmty_amt = isnull(@subj_resrv_idnmty_amt,0)
						set @subj_resrv_exps_amt = isnull(@subj_resrv_exps_amt,0)
						set @basic_pre_prop_amt = isnull(@basic_pre_prop_amt,0)

						update #params 
							set val = @com_agm_id 
						where id = 6

						update #params 
							set val = @state_id 
						where id = 7

						update #params 
							set val = @subj_paid_idnmty_amt 
						where id = 8

						update #params 
							set val = @subj_paid_exps_amt 
						where id = 9

						update #params 
							set val = @subj_resrv_idnmty_amt 
						where id = 10

						update #params 
							set val = @subj_resrv_exps_amt 
						where id = 11

						update #params 
							set val = @has_ldf 
						where id = 22 --Has LDF

						update #params 
							set val = @has_lcf 
						where id = 23 --Has LCF


						/****************************
						* Determine line of business
						****************************/
						
						select @lob = attr_1_txt 
						from dbo.LKUP 
						where lkup_id = (
											select 
											covg_typ_id 
											from dbo.COML_AGMT 
											where coml_agmt_id = @com_agm_id
											and prem_adj_pgm_id = @premium_adj_prog_id
											and custmr_id = @customer_id
										)
						and lkup_typ_id in (
												select 
												lkup_typ_id 
												from dbo.LKUP_TYP 
												where lkup_typ_nm_txt like 'LOB COVERAGE'
											)
						set @lob_id = dbo.fn_GetIDForLOB(@lob)
						update #params 
							set val = @lob_id 
						where id = 12 --LOB
						if @debug = 1
						begin
						print 'retrieved @lob_id: ' + convert(varchar(20), @lob_id ) 
						end
						--Proportionately divide Basic amount
						exec @sub_aud_prem_ratio = [dbo].[fn_ComputeSubAudPremiumRatio]
   																		@customer_id = @customer_id,
																		@premium_adj_prog_id = @premium_adj_prog_id,
																		@coml_agmt_id = @com_agm_id,
																		@state_id = @state_id 

						update #params 
							set val = @basic_pre_prop_amt * (@sub_aud_prem_ratio / 100)
						where id = 13 --Basic

						
						update #params 
							set val = @elp_pre_prop_amt * (@sub_aud_prem_ratio / 100)
						where id = 19 --ELP

						if (@has_lcf = 1)
						begin
						select @cnt_losses=count(*) from dbo.ARMIS_LOS_POL 
						where prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and valn_dt=@prem_adj_valn_dt
						and actv_ind = 1
						and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id))
						--TODO: Verify if proportion by policy or not
						if(@cnt_losses>0)
						begin
						exec @loss_ratio = [dbo].[fn_ComputeLossRatio]
								@customer_id = @customer_id,
								@premium_adj_prog_id = @premium_adj_prog_id,
								@coml_agmt_id = @com_agm_id,
								@state_id = @state_id,
								@premium_adjustment_id = @premium_adjustment_id			
						
						update #params 
							set val = @lcf_aggr_cap_set_pgm_amt * (@loss_ratio / 100)
						where id = 24 --@lcf_aggr_cap_set_pgm_amt

						end
--						else
--						begin
--						exec @sub_aud_prem_ratio = [dbo].[fn_ComputeSubAudPremiumRatio]
--   																	@customer_id = @customer_id,
--																	@premium_adj_prog_id = @premium_adj_prog_id,
--																	@coml_agmt_id = @com_agm_id,
--																	@state_id = @state_id 
--						update #params 
--							set val = @lcf_aggr_cap_set_pgm_amt * (@sub_aud_prem_ratio / 100)
--						where id = 24 --@lcf_aggr_cap_set_pgm_amt
--
--						end
					end



						if (@has_chf = 1)
					begin
						select @cnt_losses=count(*) from dbo.ARMIS_LOS_POL 
						where prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and valn_dt=@prem_adj_valn_dt
						and actv_ind = 1
						and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id))
						--TODO: Verify if proportion by policy or not
						if(@cnt_losses>0)
						begin
						exec @loss_ratio = [dbo].[fn_ComputeLossRatio]
								@customer_id = @customer_id,
								@premium_adj_prog_id = @premium_adj_prog_id,
								@coml_agmt_id = @com_agm_id,
								@state_id = @state_id,
								@premium_adjustment_id = @premium_adjustment_id			
						
						update #params 
							set val = @chf_amt * (@loss_ratio / 100)
						where id = 15 --CHF
						end
						else
						begin
						exec @sub_aud_prem_ratio = [dbo].[fn_ComputeSubAudPremiumRatio]
   																	@customer_id = @customer_id,
																	@premium_adj_prog_id = @premium_adj_prog_id,
																	@coml_agmt_id = @com_agm_id,
																	@state_id = @state_id 
						update #params 
							set val = @chf_amt * (@sub_aud_prem_ratio / 100)
						where id = 15 --CHF
						end
					end

						if ((@has_ncf = 1) or (@has_oa = 1) )
						begin
							--Proportionately divide NCF / Other amount
							select 
							@ncf_pre_prop_amt = nonconv_amt,
							@other_pre_prop_amt = othr_pol_adj_amt
							from dbo.COML_AGMT
							where custmr_id = @customer_id
							and prem_adj_pgm_id = @premium_adj_prog_id
							and coml_agmt_id = @com_agm_id
							and actv_ind = 1

							--set @ncf_pre_prop_amt = isnull(@ncf_pre_prop_amt,0)
							--set @other_pre_prop_amt = isnull(@other_pre_prop_amt,0)


						end

	
						if ((@has_ncf = 1) or (@has_oa = 1))
						begin
							exec @erp_ratio = [dbo].[fn_ComputeERPRatioByPolicy]
											   			@premium_adj_perd_id = @premium_adj_period_id,
														@premium_adj_id = @premium_adjustment_id,
														@customer_id = @customer_id,
														@premium_adj_prog_id = @premium_adj_prog_id,
														@coml_agmt_id = @com_agm_id,
														@state_id = @state_id

						end


   						if @debug = 1
						begin	
						print '@ncf_pre_prop_amt: ' + convert(varchar(20),@ncf_pre_prop_amt)
						print '@erp_ratio: ' + convert(varchar(20),@erp_ratio)
						end
						if (@has_ncf = 1)
						begin
							update #params 
								set val = @ncf_pre_prop_amt * (@erp_ratio / 100)
							where id = 16 --NCF
						end

						if (@has_oa = 1)
						begin
							update #params 
								set val = @other_pre_prop_amt * (@erp_ratio / 100)
							where id = 17 --OA
						end


						--select * from #params
				
						exec sp_executesql @stmt
						if @debug = 1
						begin	
						print'---------------End of iteration-------------------' 
						end
					end
					fetch erp_sec_pass_cur into @com_agm_id, @state_id, 
					@subj_paid_idnmty_amt, @subj_paid_exps_amt, @subj_resrv_idnmty_amt, @subj_resrv_exps_amt, @is_not_master

				end --end of cursor erp_sec_pass_cur / while loop
			close erp_sec_pass_cur
			deallocate erp_sec_pass_cur

		end --end of: if ( (@has_ncf = 1) or (@has_oa = 1) )


	
		/********************************
		* Apply Maximum / Minimum rules
		*********************************/
		-- Evaluate @minmax_incurred_erp_by_pp_amt,@minmax_paid_erp_by_pp_amt,@max_amt,@min_amt
		select
		@paid_incur_type = paid_incur_typ_id
		from dbo.PREM_ADJ_PGM
		where custmr_id = @customer_id
		and prem_adj_pgm_id = @premium_adj_prog_id

		select
		@max_amt = 	case when isnull(tot_agmt_amt,0) > isnull(tot_audt_amt,0) then isnull(tot_agmt_amt,0) else isnull(tot_audt_amt,0) end 
		from dbo.PREM_ADJ_PGM_RETRO
		where custmr_id = @customer_id
		and prem_adj_pgm_id = @premium_adj_prog_id
		and retro_elemt_typ_id = 337 --Lookup Type: RETROSPECTIVE ELEMENT TYPE; lookup value: Maximum
		and retro_adj_fctr_aplcbl_ind = 0
		and no_lim_ind = 0
		and actv_ind = 1

		select
		@min_amt = case when isnull(tot_agmt_amt,0) > isnull(tot_audt_amt,0) then isnull(tot_agmt_amt,0) else isnull(tot_audt_amt,0) end
		from dbo.PREM_ADJ_PGM_RETRO
		where custmr_id = @customer_id
		and prem_adj_pgm_id = @premium_adj_prog_id
		and retro_elemt_typ_id = 338 --Lookup Type: RETROSPECTIVE ELEMENT TYPE; lookup value: Minimum
		and retro_adj_fctr_aplcbl_ind = 0
		--and no_lim_ind = 0
		and actv_ind = 1

		if
		(			
			((select no_lim_ind 
			from dbo.PREM_ADJ_PGM_RETRO
			where custmr_id = @customer_id
			and prem_adj_pgm_id = @premium_adj_prog_id
			and retro_elemt_typ_id = 337 --Lookup Type: RETROSPECTIVE ELEMENT TYPE; lookup value: Maximum
			and actv_ind = 1) = 1)
			or
			((select retro_adj_fctr_aplcbl_ind 
			from dbo.PREM_ADJ_PGM_RETRO
			where custmr_id = @customer_id
			and prem_adj_pgm_id = @premium_adj_prog_id
			and retro_elemt_typ_id = 337 --Lookup Type: RETROSPECTIVE ELEMENT TYPE; lookup value: Maximum
			and actv_ind = 1) = 1)
		)
		begin
			set @is_max_unlimited = 1
		end

		if ((@paid_incur_type = 298) or (@paid_incur_type = 297)) -- PAID and INCURRED Program Types
		begin
	
			
			/******************************************
			* Check adjustment types at policy level 
			* to ensure it is applied to PAID types only
			*******************************************/
			select
			--@minmax_incurred_erp_by_pp_amt = isnull(sum(case when ca.adj_typ_id = 72 then d.adj_incur_ernd_retro_prem_amt + d.adj_dedtbl_wrk_comp_los_amt else d.adj_incur_ernd_retro_prem_amt end ),0),
			@minmax_incurred_erp_by_pp_amt = 
			isnull(sum(
						case when ((ca.adj_typ_id = 72) or (ca.adj_typ_id = 73) or (ca.adj_typ_id = 66) or (ca.adj_typ_id = 67) ) -- Paid Loss Underlayer, Paid Loss WA, Incurred Loss Underlayer, Incurred Loss WA
						then 
							d.adj_incur_ernd_retro_prem_amt + d.adj_dedtbl_wrk_comp_los_amt 
						else 
							d.adj_incur_ernd_retro_prem_amt 
						end 
					   ),0),
			@minmax_paid_erp_by_pp_amt = 
			isnull(sum(
						case when ((ca.adj_typ_id = 72) or (ca.adj_typ_id = 73) or (ca.adj_typ_id = 66) or (ca.adj_typ_id = 67) ) -- Paid Loss Underlayer, Paid Loss WA, Incurred Loss Underlayer, Incurred Loss WA
						then 
							d.adj_paid_ernd_retro_prem_amt + d.subj_paid_idnmty_amt 
						else 
							d.adj_paid_ernd_retro_prem_amt 
						end 
					   ),0),
--isnull(sum(d.adj_paid_ernd_retro_prem_amt),0),
			@minmax_cfb_by_pp_amt = isnull(sum(d.cash_flw_ben_amt),0)
			from dbo.PREM_ADJ_RETRO_DTL d
			inner join dbo.PREM_ADJ_RETRO h on (d.prem_adj_retro_id = h.prem_adj_retro_id) and (d.prem_adj_perd_id = h.prem_adj_perd_id) and (d.prem_adj_id = h.prem_adj_id) and (d.custmr_id = h.custmr_id)
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = h.coml_agmt_id) and (ca.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ca.custmr_id = h.custmr_id) and (ca.actv_ind = 1)
			where  d.prem_adj_id = @premium_adjustment_id
			and d.custmr_id = @customer_id
			and h.prem_adj_pgm_id = @premium_adj_prog_id
			and ca.adj_typ_id in (63,65,66,67,70,71,72,73) -- Adjustment type IDs for Paid and Incurred types and removed deductibles.
			group by h.prem_adj_pgm_id
			if @debug = 1
			begin
			print '@minmax_incurred_erp_by_pp_amt: ' + convert(varchar(20),@minmax_incurred_erp_by_pp_amt)
			print '@minmax_paid_erp_by_pp_amt: ' + convert(varchar(20),@minmax_paid_erp_by_pp_amt)
			print '@minmax_cfb_by_pp_amt: ' + convert(varchar(20),@minmax_cfb_by_pp_amt)
			end
			if (			
					((select no_lim_ind 
					from dbo.PREM_ADJ_PGM_RETRO
					where custmr_id = @customer_id
					and prem_adj_pgm_id = @premium_adj_prog_id
					and retro_elemt_typ_id = 337 --Lookup Type: RETROSPECTIVE ELEMENT TYPE; lookup value: Maximum
					and actv_ind = 1) = 1)
					or
					((select retro_adj_fctr_aplcbl_ind 
					from dbo.PREM_ADJ_PGM_RETRO
					where custmr_id = @customer_id
					and prem_adj_pgm_id = @premium_adj_prog_id
					and retro_elemt_typ_id = 337 --Lookup Type: RETROSPECTIVE ELEMENT TYPE; lookup value: Maximum
					and actv_ind = 1) = 1)
					
				)
				begin
					set @max_amt = @minmax_incurred_erp_by_pp_amt
					set @is_max_unlimited = 1
				end


			if (			
					((select no_lim_ind 
					from dbo.PREM_ADJ_PGM_RETRO
					where custmr_id = @customer_id
					and prem_adj_pgm_id = @premium_adj_prog_id
					and retro_elemt_typ_id = 338 --Lookup Type: RETROSPECTIVE ELEMENT TYPE; lookup value: Miniimum
					and actv_ind = 1) = 1)
					or
					((select retro_adj_fctr_aplcbl_ind 
					from dbo.PREM_ADJ_PGM_RETRO
					where custmr_id = @customer_id
					and prem_adj_pgm_id = @premium_adj_prog_id
					and retro_elemt_typ_id = 338 --Lookup Type: RETROSPECTIVE ELEMENT TYPE; lookup value: Miniimum
					and actv_ind = 1) = 1)

				)
				begin
					set @min_amt = 0 --@minmax_incurred_erp_by_pp_amt
				end
			if @debug = 1
			begin
			print '@max_amt: ' + convert(varchar(20),@max_amt)
			print '@min_amt: ' + convert(varchar(20),@min_amt)
			end

			if (@is_max_unlimited = 0)  -- Max not unlimited
			begin
				if(@minmax_incurred_erp_by_pp_amt>=@max_amt)
				begin
					set @target_inc_erp_amt = @max_amt
					set @max_applies = 1
				end
				else
				begin

					if(@minmax_incurred_erp_by_pp_amt<=@min_amt)
					begin
						set @target_inc_erp_amt = @min_amt
						set @min_applies = 1
					end
					else
						set @erp_applies = 1
				end
			end
			else -- Max unlimited
			begin
				if(@minmax_incurred_erp_by_pp_amt<=@min_amt)
				begin
					set @target_inc_erp_amt = @min_amt
					set @min_applies = 1
				end
				else
				begin
--					if(@minmax_incurred_erp_by_pp_amt>=@max_amt)
--					begin
--						set @target_inc_erp_amt = @max_amt
--						set @max_applies = 1
--					end
--					else
						set @erp_applies = 1
						set @recalc_minmax_for_paid = 1
				end
			end
			if @debug = 1
			begin
			print '@target_inc_erp_amt: ' + convert(varchar(20),@target_inc_erp_amt)
			print '@recalc_minmax_for_paid: ' + convert(varchar(20),@recalc_minmax_for_paid)
			end

			--Evaluate limited by max / min
			if((@minmax_incurred_erp_by_pp_amt>=@max_amt and @minmax_paid_erp_by_pp_amt<=@max_amt) and (@minmax_incurred_erp_by_pp_amt>=@min_amt and @minmax_paid_erp_by_pp_amt>=@min_amt))
			begin
				set @limited_by_max_min = @minmax_incurred_erp_by_pp_amt-@max_amt
			end
			else
			begin
				if ((@minmax_incurred_erp_by_pp_amt>=@min_amt and @minmax_paid_erp_by_pp_amt>=@min_amt) and (@minmax_incurred_erp_by_pp_amt>=@max_amt and @minmax_paid_erp_by_pp_amt>=@max_amt))
				begin
					set @limited_by_max_min = @minmax_incurred_erp_by_pp_amt-@minmax_paid_erp_by_pp_amt
				end
				else
				begin 
					if((@minmax_incurred_erp_by_pp_amt<=@min_amt and @minmax_paid_erp_by_pp_amt<=@min_amt) and (@minmax_incurred_erp_by_pp_amt<=@max_amt and @minmax_paid_erp_by_pp_amt<=@max_amt))
					begin
						set @limited_by_max_min = @minmax_incurred_erp_by_pp_amt-@minmax_paid_erp_by_pp_amt
					end
					else 
					begin
						if(( @minmax_incurred_erp_by_pp_amt>=@min_amt and @minmax_paid_erp_by_pp_amt<=@min_amt) and (@minmax_incurred_erp_by_pp_amt<=@max_amt and @minmax_paid_erp_by_pp_amt<=@max_amt))
						begin
							set @limited_by_max_min = @min_amt-@minmax_paid_erp_by_pp_amt
						end
						else
						begin 
							if(@minmax_incurred_erp_by_pp_amt>@max_amt and @minmax_paid_erp_by_pp_amt>@max_amt)
							begin
								set @limited_by_max_min = @minmax_incurred_erp_by_pp_amt-@minmax_paid_erp_by_pp_amt
							end
							else
							begin
								if((@minmax_incurred_erp_by_pp_amt<=@max_amt and @minmax_paid_erp_by_pp_amt<=@max_amt) and (@minmax_incurred_erp_by_pp_amt>=@min_amt and @minmax_paid_erp_by_pp_amt>=@min_amt))
									set @limited_by_max_min = 0
								else
									set @limited_by_max_min = @minmax_incurred_erp_by_pp_amt-@minmax_paid_erp_by_pp_amt
							end
						end
					end
				end
			end
			
			declare @minmax_incurred_erp_by_pp_excl_wcloss_amt_target_erp decimal(15,2)

			select
			@minmax_incurred_erp_by_pp_excl_wcloss_amt_target_erp = 
			isnull(sum(
						case when ((ca.adj_typ_id = 72) or (ca.adj_typ_id = 73) or (ca.adj_typ_id = 66) or (ca.adj_typ_id = 67) ) -- Paid Loss Underlayer, Paid Loss WA, Incurred Loss Underlayer, Incurred Loss WA
						then 
							d.adj_incur_ernd_retro_prem_amt --+ d.adj_dedtbl_wrk_comp_los_amt 
						else 
							d.adj_incur_ernd_retro_prem_amt 
						end 
					   ),0),
			@minmax_incurred_erp_by_pp_excl_wcloss_amt = 
			isnull(sum(
						case when ((ca.adj_typ_id = 72) or (ca.adj_typ_id = 73) or (ca.adj_typ_id = 66) or (ca.adj_typ_id = 67) ) -- Paid Loss Underlayer, Paid Loss WA, Incurred Loss Underlayer, Incurred Loss WA
						then 
							d.adj_incur_ernd_retro_prem_amt + d.adj_dedtbl_wrk_comp_los_amt 
						else 
							d.adj_incur_ernd_retro_prem_amt 
						end 
					   ),0)
--isnull(sum(d.adj_incur_ernd_retro_prem_amt),0)
			from dbo.PREM_ADJ_RETRO_DTL d
			inner join dbo.PREM_ADJ_RETRO h on (d.prem_adj_retro_id = h.prem_adj_retro_id) and (d.prem_adj_perd_id = h.prem_adj_perd_id) and (d.prem_adj_id = h.prem_adj_id) and (d.custmr_id = h.custmr_id)
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = h.coml_agmt_id) and (ca.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ca.custmr_id = h.custmr_id) and (ca.actv_ind = 1)
			where  d.prem_adj_id = @premium_adjustment_id
			and d.custmr_id = @customer_id
			and h.prem_adj_pgm_id = @premium_adj_prog_id
			and ca.adj_typ_id in (63,65,66,67,70,71,72,73) -- Adjustment type IDs for Paid and Incurred types and removed deductibles.
			group by h.prem_adj_pgm_id
			if @debug = 1
			begin
			print '@minmax_incurred_erp_by_pp_excl_wcloss_amt: ' + convert(varchar(20),@minmax_incurred_erp_by_pp_excl_wcloss_amt)
			end
			if(@paid_incur_type = 298) -- Paid
				set @target_cfb_amt = (@minmax_paid_erp_by_pp_amt - @minmax_incurred_erp_by_pp_excl_wcloss_amt) + @limited_by_max_min
			else
				set @target_cfb_amt = 0

			--set @target_cfb_amt = ( @minmax_incurred_erp_by_pp_excl_wcloss_amt - @minmax_paid_erp_by_pp_amt ) + @limited_by_max_min

		end -- end of: if(@paid_incur_type = 298 or 297) --PAID / INCURRED
		else
		begin
			set @erp_applies = 1
			set @recalc_minmax_for_paid = 0
		end
		if @debug = 1
		begin
		print '@limited_by_max_min: ' + convert(varchar(20),@limited_by_max_min)
		print '@target_cfb_amt: ' + convert(varchar(20),@target_cfb_amt)
		end
		/******************************************************************
		* Recalculate LCF / Incurred ERP etc. based on MinMax, LCF aggr cap
		******************************************************************/
		select @stmt = replace(@stmt, 'non_conv_fee_amt = @ncf_amt','non_conv_fee_amt = non_conv_fee_amt')
		select @stmt = replace(@stmt, 'othr_amt = @oa_amt','othr_amt = othr_amt')
		if (@recalc_lcf = 1)
			select @stmt = replace(@stmt, 'adj_incur_ernd_retro_prem_amt = @inc_erp_amt,adj_paid_ernd_retro_prem_amt = @paid_erp_amt',' los_conv_fctr_amt = @lcf_result')
		else
			select @stmt = replace(@stmt, 'adj_incur_ernd_retro_prem_amt = @inc_erp_amt,adj_paid_ernd_retro_prem_amt = @paid_erp_amt',' los_conv_fctr_amt = los_conv_fctr_amt')


		if (@recalc_minmax_for_paid = 1)
		begin
			if(@pgm_type = 1) --DEP Program Type
				set @stmt = @stmt + ' update dbo.PREM_ADJ_RETRO_DTL WITH (ROWLOCK) set biled_ernd_retro_prem_amt = @adj_incur_erp_minmax,adj_cash_flw_ben_amt = @adj_cfb_minmax from dbo.PREM_ADJ_RETRO_DTL d inner join dbo.PREM_ADJ_PGM p on (p.prem_adj_pgm_id = d.prem_adj_pgm_id) where d.prem_adj_perd_id = @premium_adj_period_id and d.prem_adj_id = @premium_adjustment_id and d.custmr_id = @customer_id and d.coml_agmt_id = @com_agm_id and d.st_id = @state_id and p.paid_incur_typ_id in (298,297)'
			else
				set @stmt = @stmt + ' update dbo.PREM_ADJ_RETRO_DTL WITH (ROWLOCK) set biled_ernd_retro_prem_amt = @adj_incur_erp_minmax,adj_cash_flw_ben_amt = @adj_cfb_minmax from dbo.PREM_ADJ_RETRO_DTL d inner join dbo.PREM_ADJ_RETRO h on (d.prem_adj_retro_id = h.prem_adj_retro_id) and (d.prem_adj_perd_id = h.prem_adj_perd_id) and (d.prem_adj_id = h.prem_adj_id) and (d.custmr_id = h.custmr_id) inner join dbo.PREM_ADJ_PGM p on (p.prem_adj_pgm_id = h.prem_adj_pgm_id) where d.prem_adj_perd_id = @premium_adj_period_id and d.prem_adj_id = @premium_adjustment_id and d.custmr_id = @customer_id and h.coml_agmt_id = @com_agm_id and d.st_id = @state_id and p.paid_incur_typ_id in (298,297)'
		end
		if @debug = 1
		begin
		print 'Recalc third pass: Re-formulated sql string: ' + @stmt
		select 'Recalc third pass: Re-formulated sql string: ' , @stmt
		end
		if ( (@recalc_lcf = 1) or (@recalc_minmax_for_paid = 1) )
		begin

			declare recalc_cur cursor LOCAL FAST_FORWARD READ_ONLY 
			for 
			select  
			md.com_agr_id ,
			md.state_id,
			md.subj_paid_idnmty_amt ,
			md.subj_paid_exps_amt ,
			md.subj_resrv_idnmty_amt ,
			md.subj_resrv_exps_amt,
			case when c.pol_sym_txt = 'DEP' then 0 else 1 end as is_not_master
			from
			(
				select  
				case when aud.coml_agmt_id is not null then aud.coml_agmt_id else los.coml_agmt_id end as com_agr_id ,
				case when aud.st_id is not null then aud.st_id else los.st_id end as state_id,
				los.subj_paid_idnmty_amt ,
				los.subj_paid_exps_amt ,
				los.subj_resrv_idnmty_amt ,
				los.subj_resrv_exps_amt 
				from
				(
					select 
					spa.coml_agmt_id,
					spa.st_id,
					spa.prem_amt
					from dbo.COML_AGMT ca
					inner join dbo.COML_AGMT_AUDT caa on (ca.coml_agmt_id = caa.coml_agmt_id) and (ca.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (ca.custmr_id = caa.custmr_id)
					inner join dbo.SUBJ_PREM_AUDT spa on (spa.coml_agmt_audt_id = caa.coml_agmt_audt_id) and (spa.coml_agmt_id = caa.coml_agmt_id) and (spa.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (spa.custmr_id = caa.custmr_id)
					where ca.prem_adj_pgm_id = @premium_adj_prog_id
					and ca.custmr_id = @customer_id
					and (caa.audt_revd_sts_ind = 0 or caa.audt_revd_sts_ind is null)
					and ca.actv_ind = 1
					and spa.actv_ind = 1
				) aud
				full outer join 
				(
					select 				
					prem_adj_pgm_id, 				
					custmr_id, 				
					coml_agmt_id, 				
					st_id, 				
					isnull(sum(subj_paid_idnmty_amt),0) as subj_paid_idnmty_amt , 				
					isnull(sum(subj_paid_exps_amt),0) as   subj_paid_exps_amt ,			
					isnull(sum(subj_resrv_idnmty_amt),0) as subj_resrv_idnmty_amt , 				
					isnull(sum(subj_resrv_exps_amt),0) as subj_resrv_exps_amt 				
					from 				
					dbo.ARMIS_LOS_POL 				
					where prem_adj_pgm_id =@premium_adj_prog_id 				
					and custmr_id = @customer_id 				
					and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id)) 				
					and valn_dt	= @prem_adj_valn_dt -- Triage # 67			
					and actv_ind = 1  
					group by  st_id,coml_agmt_id,custmr_id,prem_adj_pgm_id  
				) los
				on (los.coml_agmt_id = aud.coml_agmt_id) and (los.st_id = aud.st_id)
			) md
			inner join COML_AGMT c on (md.com_agr_id = c.coml_agmt_id) and (c.actv_ind = 1)
			where c.adj_typ_id not in (62,64,68,69,448)
			order by is_not_master,com_agr_id

			open recalc_cur
			fetch recalc_cur into @com_agm_id, @state_id, 
				@subj_paid_idnmty_amt, @subj_paid_exps_amt, @subj_resrv_idnmty_amt, @subj_resrv_exps_amt, @is_not_master

			while @@Fetch_Status = 0
				begin
					begin
					
						-- Handle potential null values
						set @subj_paid_idnmty_amt = isnull(@subj_paid_idnmty_amt,0)
						set @subj_paid_exps_amt = isnull(@subj_paid_exps_amt,0)
						set @subj_resrv_idnmty_amt = isnull(@subj_resrv_idnmty_amt,0)
						set @subj_resrv_exps_amt = isnull(@subj_resrv_exps_amt,0)
						set @basic_pre_prop_amt = isnull(@basic_pre_prop_amt,0)

						update #params 
							set val = @com_agm_id 
						where id = 6

						update #params 
							set val = @state_id 
						where id = 7

						update #params 
							set val = @subj_paid_idnmty_amt 
						where id = 8

						update #params 
							set val = @subj_paid_exps_amt 
						where id = 9

						update #params 
							set val = @subj_resrv_idnmty_amt 
						where id = 10

						update #params 
							set val = @subj_resrv_exps_amt 
						where id = 11

						update #params 
							set val = @has_ldf 
						where id = 22 --Has LDF

						update #params 
							set val = @has_lcf 
						where id = 23 --Has LCF

						/****************************
						* Determine line of business
						****************************/
						
						select @lob = attr_1_txt 
						from dbo.LKUP 
						where lkup_id = (
											select 
											covg_typ_id 
											from dbo.COML_AGMT 
											where coml_agmt_id = @com_agm_id
											and prem_adj_pgm_id = @premium_adj_prog_id
											and custmr_id = @customer_id
										)
						and lkup_typ_id in (
												select 
												lkup_typ_id 
												from dbo.LKUP_TYP 
												where lkup_typ_nm_txt like 'LOB COVERAGE'
											)
						set @lob_id = dbo.fn_GetIDForLOB(@lob)
						update #params 
							set val = @lob_id 
						where id = 12 --LOB
						if @debug = 1
						begin
						print 'retrieved @lob_id: ' + convert(varchar(20), @lob_id ) 
						end
						--Proportionately divide Basic amount
						exec @sub_aud_prem_ratio = [dbo].[fn_ComputeSubAudPremiumRatio]
   																		@customer_id = @customer_id,
																		@premium_adj_prog_id = @premium_adj_prog_id,
																		@coml_agmt_id = @com_agm_id,
																		@state_id = @state_id 

						update #params 
							set val = @basic_pre_prop_amt * (@sub_aud_prem_ratio / 100)
						where id = 13 --Basic

						
						update #params 
							set val = @elp_pre_prop_amt * (@sub_aud_prem_ratio / 100)
						where id = 19 --ELP
					
						if (@has_lcf = 1)
						begin
						select @cnt_losses=count(*) from dbo.ARMIS_LOS_POL 
						where prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and valn_dt=@prem_adj_valn_dt
						and actv_ind = 1
						and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id))
						--TODO: Verify if proportion by policy or not
						if(@cnt_losses>0)
						begin
						exec @loss_ratio = [dbo].[fn_ComputeLossRatio]
								@customer_id = @customer_id,
								@premium_adj_prog_id = @premium_adj_prog_id,
								@coml_agmt_id = @com_agm_id,
								@state_id = @state_id,
								@premium_adjustment_id = @premium_adjustment_id			
						
						update #params 
							set val = @lcf_aggr_cap_set_pgm_amt * (@loss_ratio / 100)
						where id = 24 --@lcf_aggr_cap_set_pgm_amt

						end
--						else
--						begin
--						exec @sub_aud_prem_ratio = [dbo].[fn_ComputeSubAudPremiumRatio]
--   																	@customer_id = @customer_id,
--																	@premium_adj_prog_id = @premium_adj_prog_id,
--																	@coml_agmt_id = @com_agm_id,
--																	@state_id = @state_id 
--						update #params 
--							set val = @lcf_aggr_cap_set_pgm_amt * (@sub_aud_prem_ratio / 100)
--						where id = 24 --@lcf_aggr_cap_set_pgm_amt
--
--						end
					end



						if (@has_chf = 1)
					begin
						select @cnt_losses=count(*) from dbo.ARMIS_LOS_POL 
						where prem_adj_pgm_id = @premium_adj_prog_id
						and custmr_id = @customer_id
						and valn_dt=@prem_adj_valn_dt
						and actv_ind = 1
						and ((prem_adj_id is null) or (prem_adj_id = @premium_adjustment_id))
						--TODO: Verify if proportion by policy or not
						if(@cnt_losses>0)
						begin
						exec @loss_ratio = [dbo].[fn_ComputeLossRatio]
								@customer_id = @customer_id,
								@premium_adj_prog_id = @premium_adj_prog_id,
								@coml_agmt_id = @com_agm_id,
								@state_id = @state_id,
								@premium_adjustment_id = @premium_adjustment_id			
						
						update #params 
							set val = @chf_amt * (@loss_ratio / 100)
						where id = 15 --CHF
						end
						else
						begin
						exec @sub_aud_prem_ratio = [dbo].[fn_ComputeSubAudPremiumRatio]
   																	@customer_id = @customer_id,
																	@premium_adj_prog_id = @premium_adj_prog_id,
																	@coml_agmt_id = @com_agm_id,
																	@state_id = @state_id 
						update #params 
							set val = @chf_amt * (@sub_aud_prem_ratio / 100)
						where id = 15 --CHF
						end
					end

						if ((@has_ncf = 1) or (@has_oa = 1) )
						begin
							--Proportionately divide NCF / Other amount
							select 
							@ncf_pre_prop_amt = isnull(nonconv_amt,0),
							@other_pre_prop_amt = isnull(othr_pol_adj_amt,0)
							from dbo.COML_AGMT
							where custmr_id = @customer_id
							and prem_adj_pgm_id = @premium_adj_prog_id
							and coml_agmt_id = @com_agm_id
							and actv_ind = 1

							set @ncf_pre_prop_amt = isnull(@ncf_pre_prop_amt,0)
							set @other_pre_prop_amt = isnull(@other_pre_prop_amt,0)


						end
	
						if ((@has_ncf = 1) or (@has_oa = 1))
						begin
							exec @erp_ratio = [dbo].[fn_ComputeERPRatioByPolicy]
											   			@premium_adj_perd_id = @premium_adj_period_id,
														@premium_adj_id = @premium_adjustment_id,
														@customer_id = @customer_id,
														@premium_adj_prog_id = @premium_adj_prog_id,
														@coml_agmt_id = @com_agm_id,
														@state_id = @state_id

						end

   						if @debug = 1
						begin	
						print '@ncf_pre_prop_amt: ' + convert(varchar(20),@ncf_pre_prop_amt)
						print '@erp_ratio: ' + convert(varchar(20),@erp_ratio)
						end
						if (@has_ncf = 1)
						begin
							update #params 
								set val = @ncf_pre_prop_amt * (@erp_ratio / 100)
							where id = 16 --NCF
						end

						if (@has_oa = 1)
						begin
							update #params 
								set val = @other_pre_prop_amt * (@erp_ratio / 100)
							where id = 17 --OA
						end 							

						if (@recalc_lcf = 1)
						begin
							exec @erp_ratio_by_pp = [dbo].[fn_ComputeERPRatioByProgPerd]
											   		@premium_adj_perd_id = @premium_adj_period_id,
													@premium_adj_id = @premium_adjustment_id,
													@customer_id = @customer_id,
													@premium_adj_prog_id = @premium_adj_prog_id,
													@coml_agmt_id = @com_agm_id,
													@state_id = @state_id

							update #params 
								set val = (@minmax_lcf_amt - @lcf_aggr_cap_set_pgm_amt) * (@erp_ratio_by_pp / 100)
							where id = 18 ----LCF Aggregate cap
							if @debug = 1
							begin
							print 'recalcing LCF: calculating diff percentage'
							end
						end

						if (@recalc_minmax_for_paid = 1)
						begin

							declare @sum_erp_amount decimal(15,2)
							select @sum_erp_amount = sum(isnull(d.adj_incur_ernd_retro_prem_amt,0)) from 
							dbo.PREM_ADJ_RETRO_DTL d
							inner join dbo.PREM_ADJ_RETRO h on (d.prem_adj_retro_id = h.prem_adj_retro_id) and (d.prem_adj_perd_id = h.prem_adj_perd_id) and (d.prem_adj_id = h.prem_adj_id) and (d.custmr_id = h.custmr_id)
							inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = h.coml_agmt_id) and (ca.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ca.custmr_id = h.custmr_id) and (ca.actv_ind = 1)
							where d.prem_adj_perd_id = @premium_adj_period_id
							and d.prem_adj_id = @premium_adjustment_id
							and d.custmr_id = @customer_id
							and h.prem_adj_pgm_id = @premium_adj_prog_id
							and ca.adj_typ_id in (63,65,66,67,70,71,72,73) -- Adjustment type IDs for Paid and Incurred types


							exec @erp_ratio_by_pp = [dbo].[fn_ComputeERPRatioByProgPerdForPaidPols]
											   		@premium_adj_perd_id = @premium_adj_period_id,
													@premium_adj_id = @premium_adjustment_id,
													@customer_id = @customer_id,
													@premium_adj_prog_id = @premium_adj_prog_id,
													@coml_agmt_id = @com_agm_id,
													@state_id = @state_id

							if(@sum_erp_amount = 0)
							begin
								set @erp_ratio_by_pp = @sub_aud_prem_ratio
							end

							if @debug = 1
							begin
							print 'Before Inc. ERP diff -> @erp_ratio_by_pp: ' + convert(varchar(20),@erp_ratio_by_pp)
							end
							update #params 
								set val = (/*@minmax_incurred_erp_by_pp_amt*/@minmax_incurred_erp_by_pp_excl_wcloss_amt_target_erp - @target_inc_erp_amt) * (@erp_ratio_by_pp / 100)
							where id = 20 ----MinMax Incurred ERP differential

							exec @cfb_ratio_by_pp = [dbo].[fn_ComputeCFBRatioByProgPerdForPaidPols]
											   		@premium_adj_perd_id = @premium_adj_period_id,
													@premium_adj_id = @premium_adjustment_id,
													@customer_id = @customer_id,
													@premium_adj_prog_id = @premium_adj_prog_id,
													@coml_agmt_id = @com_agm_id,
													@state_id = @state_id

							update #params 
								set val = (@minmax_cfb_by_pp_amt - @target_cfb_amt) * (@cfb_ratio_by_pp / 100)
							where id = 21 ----MinMax CFB differential
						end
	
						--select * from #params
				
						exec sp_executesql @stmt
						if @debug = 1
						begin	
						print'---------------End of iteration-------------------' 
						end
					end
					fetch recalc_cur into @com_agm_id, @state_id, 
					@subj_paid_idnmty_amt, @subj_paid_exps_amt, @subj_resrv_idnmty_amt, @subj_resrv_exps_amt, @is_not_master

				end --end of cursor recalc_cur / while loop
			close recalc_cur
			deallocate recalc_cur

		end --end of: if ( (@recalc_lcf = 1) or (@recalc_minmax_for_paid = 1) )


		/*******************************
		* Rounding logic
		********************************/

		--Round Retro fields
		update dbo.[PREM_ADJ_RETRO_DTL]  WITH (ROWLOCK)
		   set  --clm_hndl_fee_amt = round(clm_hndl_fee_amt,0) ,
--				basic_amt  = round(basic_amt,0) ,
--				exc_los_prem_amt = round(exc_los_prem_amt,0) ,
				los_base_asessment_amt = round(los_base_asessment_amt,0) ,
--				non_conv_fee_amt = round(non_conv_fee_amt,0) ,
				--prem_tax_amt = round(prem_tax_amt,0) ,
--				othr_amt = round(othr_amt,0) ,
				--los_conv_fctr_amt = round(los_conv_fctr_amt,0) ,
--				incur_ernd_retro_prem_amt = round(incur_ernd_retro_prem_amt,0),
--				adj_incur_ernd_retro_prem_amt = round(adj_incur_ernd_retro_prem_amt,0),
--				paid_ernd_retro_prem_amt = round(paid_ernd_retro_prem_amt,0),
--				adj_paid_ernd_retro_prem_amt = round(adj_paid_ernd_retro_prem_amt,0),
				cash_flw_ben_amt = round(cash_flw_ben_amt,0),
				--los_dev_resrv_amt = round(los_dev_resrv_amt,0),
--				adj_dedtbl_wrk_comp_los_amt = round(adj_dedtbl_wrk_comp_los_amt,0),
--				prior_cash_flw_ben_amt=round(prior_cash_flw_ben_amt,0),
--				std_subj_prem_amt = round(std_subj_prem_amt,0),
				prem_asses_amt = round(prem_asses_amt,0) ,
--				ernd_retro_prem_amt=round(ernd_retro_prem_amt,0),
				ky_or_tax_asses_amt=round(ky_or_tax_asses_amt,0),
				ky_or_prev_tax_asses_amt=round(ky_or_prev_tax_asses_amt,0),
--				rsdl_mkt_load_prev_amt=round(rsdl_mkt_load_prev_amt,0),
--				ky_or_tot_due_amt=round(ky_or_tot_due_amt,0),
				cesar_cd_tot_amt=round(isnull(biled_ernd_retro_prem_amt,0),0),
				adj_cash_flw_ben_amt=round(adj_cash_flw_ben_amt,0),
--				prev_biled_ernd_retro_prem_amt=round(prev_biled_ernd_retro_prem_amt,0),
--				prev_std_subj_prem_amt=round(prev_std_subj_prem_amt,0),
				rsdl_mkt_load_ernd_amt = round(rsdl_mkt_load_ernd_amt,0) 
--				rsdl_mkt_load_tot_amt = round(rsdl_mkt_load_tot_amt,0),
--				rsdl_mkt_load_paid_amt = round(rsdl_mkt_load_paid_amt,0)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id

			declare @diffAmt_pt decimal(15,8)		
			declare @diffAmt_lcf decimal(15,8)
			declare @diffAmt_basic decimal(15,8)
			declare @diffAmt_chf decimal(15,8)
			declare @diffAmt_ncf decimal(15,8)
			declare @diffAmt_oa decimal(15,8)
			declare @diffAmt_exc decimal(15,8)
			declare @diffAmt_rml decimal(15,8)
			declare @diffAmt_ldr decimal(15,8)
			set @diffAmt_pt = 0
			set @diffAmt_lcf = 0
			set @diffAmt_basic = 0
			set @diffAmt_chf = 0
			set @diffAmt_ncf = 0
			set @diffAmt_oa = 0
			set @diffAmt_exc = 0
			set @diffAmt_rml = 0
			set @diffAmt_ldr = 0

			select @diffAmt_ldr = sum(isnull(los_dev_resrv_amt,0))-round(sum(isnull(los_dev_resrv_amt,0)),0)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id

			if @diffAmt_ldr <> 0
			begin
			select @diffAmt_ldr = @diffAmt_ldr/count(d.los_dev_resrv_amt)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id
			end	

			select @diffAmt_rml = sum(isnull(rsdl_mkt_load_tot_amt,0))-round(sum(isnull(rsdl_mkt_load_tot_amt,0)),0)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id

			if @diffAmt_rml <> 0
			begin
			select @diffAmt_rml = @diffAmt_rml/count(d.rsdl_mkt_load_tot_amt)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id
			end	

			select @diffAmt_exc = sum(isnull(exc_los_prem_amt,0))-round(sum(isnull(exc_los_prem_amt,0)),0)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id

			if @diffAmt_exc <> 0
			begin
			select @diffAmt_exc = @diffAmt_exc/count(d.exc_los_prem_amt)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id
			end			

			select @diffAmt_oa = sum(isnull(othr_amt,0))-round(sum(isnull(othr_amt,0)),0)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id

			if @diffAmt_oa <> 0
			begin
			select @diffAmt_oa = @diffAmt_oa/count(d.othr_amt)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id
			end			

			select @diffAmt_ncf = sum(isnull(non_conv_fee_amt,0))-round(sum(isnull(non_conv_fee_amt,0)),0)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id

			if @diffAmt_ncf <> 0
			begin
			select @diffAmt_ncf = @diffAmt_ncf/count(d.non_conv_fee_amt)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id
			end
			
			select @diffAmt_chf = sum(isnull(clm_hndl_fee_amt,0))-round(sum(isnull(clm_hndl_fee_amt,0)),0)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id

			if @diffAmt_chf <> 0
			begin
			select @diffAmt_chf = @diffAmt_chf/count(d.clm_hndl_fee_amt)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id
			end

			select @diffAmt_basic = sum(isnull(basic_amt,0))-round(sum(isnull(basic_amt,0)),0)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id

			if @diffAmt_basic <> 0
			begin
			select @diffAmt_basic = @diffAmt_basic/count(d.basic_amt)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id
			end

			select @diffAmt_pt = sum(isnull(prem_tax_amt,0))-round(sum(isnull(prem_tax_amt,0)),0)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id

			if @diffAmt_pt <> 0
			begin
			select @diffAmt_pt = @diffAmt_pt/count(d.prem_tax_amt)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id
			end

			select @diffAmt_lcf = sum(isnull(los_conv_fctr_amt,0))-round(sum(isnull(los_conv_fctr_amt,0)),0)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id

			if @diffAmt_lcf <> 0
			begin
			select @diffAmt_lcf = @diffAmt_lcf/count(d.los_conv_fctr_amt)
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id
			end

			--Re-calculate adj_incur_erp_amt
			update dbo.[PREM_ADJ_RETRO_DTL]  WITH (ROWLOCK)
		   set  adj_incur_ernd_retro_prem_amt = 
				
				case when ( (ca.adj_typ_id = 66) or (ca.adj_typ_id = 73) or (ca.adj_typ_id = 67) or (ca.adj_typ_id = 72)) 
					then 0
				else
					 isnull(subj_paid_idnmty_amt,0)
				end  +
				isnull(subj_paid_exps_amt ,0) +
				case when ( (ca.adj_typ_id = 66) or (ca.adj_typ_id = 73) or (ca.adj_typ_id = 67) or (ca.adj_typ_id = 72)) 
					then 0
				else
					isnull(subj_resrv_idnmty_amt ,0)
				end  +
				isnull(subj_resrv_exps_amt ,0) +
				isnull(basic_amt,0)  +
				isnull(clm_hndl_fee_amt,0) +
				isnull(los_base_asessment_amt,0) +
				isnull(non_conv_fee_amt,0) +
				isnull(prem_tax_amt,0) +
				isnull(othr_amt,0) +
				isnull(los_conv_fctr_amt,0) +
				isnull(exc_los_prem_amt,0) +
				isnull(los_dev_resrv_amt,0)+
				case when ( (ca.adj_typ_id = 66) or (ca.adj_typ_id = 73)) 
					then isnull(rsdl_mkt_load_tot_amt,0)
				else
					0
				end + 
				isnull(prem_asses_amt,0) - 
				case when los_conv_fctr_amt is not null then isnull(@diffAmt_lcf,0) else 0 end - 
				case when prem_tax_amt is not null then isnull(@diffAmt_pt,0) else 0 end -
				case when basic_amt is not null then isnull(@diffAmt_basic,0) else 0 end -
				case when clm_hndl_fee_amt is not null then isnull(@diffAmt_chf,0) else 0 end -
				case when non_conv_fee_amt is not null then isnull(@diffAmt_ncf,0) else 0 end - 	
				case when othr_amt is not null then isnull(@diffAmt_oa,0) else 0 end -
 				case when exc_los_prem_amt is not null then isnull(@diffAmt_exc,0) else 0 end -
				case when rsdl_mkt_load_tot_amt is not null then isnull(@diffAmt_rml,0) else 0 end -  
				case when los_dev_resrv_amt is not null then isnull(@diffAmt_ldr,0) else 0 end 
			from dbo.[PREM_ADJ_RETRO_DTL] d
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = d.coml_agmt_id) and (ca.prem_adj_pgm_id = d.prem_adj_pgm_id) and (ca.custmr_id = d.custmr_id)
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id

		if (@erp_applies = 1)
		begin
			update dbo.PREM_ADJ_RETRO_DTL WITH (ROWLOCK)
			set biled_ernd_retro_prem_amt = adj_incur_ernd_retro_prem_amt
			where prem_adj_id = @premium_adjustment_id 
			and prem_adj_perd_id = @premium_adj_period_id 
			--and biled_ernd_retro_prem_amt is null
		end

		declare @sum_coding_tot decimal(15,2),
				@sum_biled_erp_amt decimal(15,2)

		select @sum_coding_tot = sum(isnull(cesar_cd_tot_amt,0)),
			   @sum_biled_erp_amt = sum(isnull(biled_ernd_retro_prem_amt,0))
		from PREM_ADJ_RETRO_DTL
		where prem_adj_id = @premium_adjustment_id
		and prem_adj_perd_id = @premium_adj_period_id



		update dbo.PREM_ADJ_RETRO_DTL 
		set cesar_cd_tot_amt = round(isnull(cesar_cd_tot_amt,0) - (isnull(@sum_coding_tot,0) - isnull(@sum_biled_erp_amt,0) ),0)
		where prem_adj_id = @premium_adjustment_id
		and prem_adj_perd_id = @premium_adj_period_id
--				and coml_agmt_id in
--				(
--					select top 1 coml_agmt_id 
--					from dbo.PREM_ADJ_RETRO_DTL
--					where biled_ernd_retro_prem_amt
--					in 
--					(
--						select max(biled_ernd_retro_prem_amt)
--						from dbo.PREM_ADJ_RETRO_DTL 
--						where prem_adj_id = @premium_adjustment_id
--						and prem_adj_perd_id = @premium_adj_period_id
--
--					)
--					and prem_adj_id = @premium_adjustment_id
--					and prem_adj_perd_id = @premium_adj_period_id
--				)
		and prem_adj_retro_dtl_id in
		(

			select top 1 prem_adj_retro_dtl_id 
			from dbo.PREM_ADJ_RETRO_DTL
			where biled_ernd_retro_prem_amt
			in 
			(
				select max(biled_ernd_retro_prem_amt)
				from dbo.PREM_ADJ_RETRO_DTL 
				where prem_adj_id = @premium_adjustment_id
				and prem_adj_perd_id = @premium_adj_period_id

			)
			and prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id

		)



		--End rounding changes

		if(@paid_incur_type = 297) -- Incurred
		begin
			update dbo.PREM_ADJ_RETRO_DTL WITH (ROWLOCK)
			set cash_flw_ben_amt = null
			where prem_adj_id = @premium_adjustment_id 
			and prem_adj_perd_id = @premium_adj_period_id 
			and cash_flw_ben_amt = 0

			update dbo.PREM_ADJ_RETRO_DTL WITH (ROWLOCK)
			set adj_cash_flw_ben_amt = null
			where prem_adj_id = @premium_adjustment_id 
			and prem_adj_perd_id = @premium_adj_period_id 
			and adj_cash_flw_ben_amt = 0
		end

		/**************************************************
		* UPDATE PREM_ADJ_PERD TABLE WITH MIN / MAX INFO
		**************************************************/
		if @debug = 1
		begin
		print '@min_applies: ' + convert(varchar(20), @min_applies)		
		print '@max_applies: ' + convert(varchar(20), @max_applies)		
		print '@erp_applies: ' + convert(varchar(20), @erp_applies)		
		print '@is_max_unlimited: ' + convert(varchar(20), @is_max_unlimited)		
		end

		if (@min_applies = 1)
			set @perd_code = 'Min'
		else if (@max_applies = 1)
			set @perd_code = 'Max'
		else if (@erp_applies = 1)
			set @perd_code = 'ERP'

		set @perd_erp_max_amt = @max_amt
	
		set @perd_erp_min_amt = @min_amt
		
		/**************************************************
		* Bug fix for 10594 starts from here
		**************************************************/
		if((select retro_adj_fctr_aplcbl_ind 
					from dbo.PREM_ADJ_PGM_RETRO
					where custmr_id = @customer_id
					and prem_adj_pgm_id = @premium_adj_prog_id
					and retro_elemt_typ_id = 337 --Lookup Type: RETROSPECTIVE ELEMENT TYPE; lookup value: Maximum
					and actv_ind = 1) = 1)
		begin
		if(@perd_code='Max')
		begin
		set @perd_code = 'ERP'
		end
		end
		
		
		if((select retro_adj_fctr_aplcbl_ind 
					from dbo.PREM_ADJ_PGM_RETRO
					where custmr_id = @customer_id
					and prem_adj_pgm_id = @premium_adj_prog_id
					and retro_elemt_typ_id = 338 --Lookup Type: RETROSPECTIVE ELEMENT TYPE; lookup value: Manimum
					and actv_ind = 1) = 1)
		begin
		if(@perd_code='Min')
		begin
		set @perd_code = 'ERP'
		end
		end
		
		
		/**************************************************
		* Bug fix for 10594 ends here
		**************************************************/
		

		select 
		--@perd_erp_amt = isnull(sum(biled_ernd_retro_prem_amt),0) + isnull(sum(adj_dedtbl_wrk_comp_los_amt),0)
		@perd_erp_amt = case when @perd_code = 'ERP' then isnull(sum( biled_ernd_retro_prem_amt ),0) + isnull(sum( adj_dedtbl_wrk_comp_los_amt ),0) else isnull(sum( biled_ernd_retro_prem_amt),0)  end
		from dbo.PREM_ADJ_RETRO_DTL
		where prem_adj_id = @premium_adjustment_id 
		and prem_adj_perd_id = @premium_adj_period_id 

		update dbo.PREM_ADJ_PERD WITH (ROWLOCK)
		set ernd_retro_prem_min_amt = @perd_erp_min_amt,
			ernd_retro_prem_max_amt = @perd_erp_max_amt,
			ernd_retro_prem_amt = round(@perd_erp_amt,0),
			ernd_retro_prem_min_max_cd = @perd_code,
			ernd_retro_prem_unlim_ind = @is_max_unlimited,
			updt_dt=getdate()
		where prem_adj_id = @premium_adjustment_id 
		and prem_adj_perd_id = @premium_adj_period_id 


		/**************************************************
		* POPULATE FIELDS OF PREMIUM ADJUSTMENT RETRO TABLE
		**************************************************/


		/***********************************************
		* DETERMINE PREVIOUS VALID ADJUSTMENT AND POPULATE
		* CORRESPONDING PREVIOUS ERP AMT
		***********************************************/
		exec @prev_valid_adj_id = [dbo].[fn_DeterminePrevValidAdj_Premium]
 			@current_premium_adjustment_id = @premium_adjustment_id,
			@customer_id = @customer_id,
			@premium_adj_prog_id = @premium_adj_prog_id

		if @debug = 1
		begin
		print '@prev_valid_adj_id: ' + convert(varchar(20), @prev_valid_adj_id ) 
		end
			
			--------begin: changes for bug # 10351---------------
		declare @prem_adj_retro_id int
				
		select @prem_adj_retro_id = max(prem_adj_retro_id)
		from dbo.PREM_ADJ_RETRO
		where  prem_adj_id = @premium_adjustment_id
		and prem_adj_perd_id = @premium_adj_period_id
		and custmr_id = @customer_id


		insert into dbo.PREM_ADJ_RETRO_DTL
		(
		[prem_adj_retro_id]
		,[prem_adj_perd_id]
		,[prem_adj_id]
		,[custmr_id]
		,[prem_adj_pgm_id]
		,[coml_agmt_id]
		,[st_id]
		,[crte_user_id]
		)
		select 
		@prem_adj_retro_id,
		@premium_adj_period_id,
		@premium_adjustment_id,
		@customer_id,
		@premium_adj_prog_id,
		coml_agmt_id,
		st_id,
		@create_user_id
		from 
		(
			select
			custmr_id,
			prem_adj_pgm_id,
			prem_adj_id,
			prem_adj_perd_id,
			coml_agmt_id,
			st_id
			from dbo.PREM_ADJ_RETRO_DTL 
			where  prem_adj_id = @prev_valid_adj_id
			and custmr_id = @customer_id
			and prem_adj_pgm_id = @premium_adj_prog_id	
		) as prev
		where not exists
		(
			select * 
			from 
			(
				select
				custmr_id,
				prem_adj_pgm_id,
				prem_adj_id,
				coml_agmt_id,
				st_id
				from dbo.PREM_ADJ_RETRO_DTL 
				where  prem_adj_id = @premium_adjustment_id
				and prem_adj_perd_id = @premium_adj_period_id
				and custmr_id = @customer_id
			) as curr
			where prev.coml_agmt_id = curr.coml_agmt_id
			and prev.st_id = curr.st_id
		)

		--------end: changes for bug # 10351------------




			
		/**************************
		* Determine first adjustment
		**************************/
		select @cnt_prev_adjs = count(distinct pa.prem_adj_id) from dbo.PREM_ADJ pa
		inner join dbo.PREM_ADJ_STS ps on (pa.prem_adj_id = ps.prem_adj_id) and (pa.reg_custmr_id = ps.custmr_id)
		inner join dbo.PREM_ADJ_PERD prd on (pa.reg_custmr_id = prd.reg_custmr_id) and (pa.prem_adj_id = prd.prem_adj_id)
		inner join dbo.PREM_ADJ_RETRO op on /* (pa.reg_custmr_id = op.custmr_id) and */ (pa.prem_adj_id = op.prem_adj_id )
		where ps.adj_sts_typ_id in (349,352) -- Lookup IDs for applicable statuses: Final Invoice and Transmitted
		and prd.prem_adj_pgm_id = @premium_adj_prog_id
		and pa.adj_can_ind<>1 
		and pa.adj_void_ind<>1
		and pa.adj_rrsn_ind<>1
		and substring(isnull(pa.fnl_invc_nbr_txt,''),1,3)<>'RTV'

--		if (@cnt_prev_adjs <> 0) --  existing adjustments; this is not initial adjustment
--		begin

			-- Calculate the previous ERP amount by adjustment type id for 'Incurred loss retro','Paid loss retro'
			update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
			set prev_ernd_retro_prem_amt = (isnull(tm.sum_erp,0))
			from dbo.PREM_ADJ_RETRO as r
			join 
			(
				select
				h.prem_adj_pgm_id,
				h.coml_agmt_id,
				sum(isnull(d.biled_ernd_retro_prem_amt,0)) as sum_erp
				from dbo.PREM_ADJ_RETRO_DTL d
				inner join dbo.PREM_ADJ_RETRO h on (d.prem_adj_retro_id = h.prem_adj_retro_id) and (d.prem_adj_perd_id = h.prem_adj_perd_id) and (d.prem_adj_id = h.prem_adj_id) and (d.custmr_id = h.custmr_id)
				where  d.prem_adj_id = @prev_valid_adj_id
				and d.custmr_id = @customer_id
				and h.prem_adj_pgm_id = @premium_adj_prog_id
				group by h.prem_adj_pgm_id,h.coml_agmt_id
			) as tm
			on r.coml_agmt_id = tm.coml_agmt_id and r.prem_adj_pgm_id = tm.prem_adj_pgm_id
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = r.coml_agmt_id) and (ca.prem_adj_pgm_id = r.prem_adj_pgm_id) and (ca.custmr_id = r.custmr_id) and (ca.actv_ind = 1)
			where r.prem_adj_perd_id = @premium_adj_period_id
			and r.prem_adj_id = @premium_adjustment_id
			and r.custmr_id = @customer_id
			and ca.adj_typ_id in(65,71) -- Adjustment type IDs for: 'Incurred loss retro','Paid loss retro'

			-- Calculate the previous ERP amount by adjustment type id for 'Incurred underlayer','Paid loss underlayer','Incurred Loss WA','Paid loss WA'
			declare @prev_perd_code varchar(16)
			select @prev_perd_code = ernd_retro_prem_min_max_cd 
			from prem_adj_perd 
			where prem_adj_id = @prev_valid_adj_id 
			and prem_adj_pgm_id = @premium_adj_prog_id

			update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
			set prev_ernd_retro_prem_amt = ((case when @prev_perd_code = 'ERP' then isnull(tm.sum_erp,0) + isnull(tm.sum_adj_ded_wc_loss,0) else isnull(tm.sum_erp,0) end))
			from dbo.PREM_ADJ_RETRO as r
			join 
			(
				select
				h.prem_adj_pgm_id,
				h.coml_agmt_id,
				sum(isnull(d.biled_ernd_retro_prem_amt,0)) as sum_erp,
				sum(isnull(d.adj_dedtbl_wrk_comp_los_amt,0)) as sum_adj_ded_wc_loss
				from dbo.PREM_ADJ_RETRO_DTL d
				inner join dbo.PREM_ADJ_RETRO h on (d.prem_adj_retro_id = h.prem_adj_retro_id) and (d.prem_adj_perd_id = h.prem_adj_perd_id) and (d.prem_adj_id = h.prem_adj_id) and (d.custmr_id = h.custmr_id)
				where  d.prem_adj_id = @prev_valid_adj_id
				and d.custmr_id = @customer_id
				and h.prem_adj_pgm_id = @premium_adj_prog_id
				group by h.prem_adj_pgm_id,h.coml_agmt_id
			) as tm
			on r.coml_agmt_id = tm.coml_agmt_id and r.prem_adj_pgm_id = tm.prem_adj_pgm_id
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = r.coml_agmt_id) and (ca.prem_adj_pgm_id = r.prem_adj_pgm_id) and (ca.custmr_id = r.custmr_id) and (ca.actv_ind = 1)
			where r.prem_adj_perd_id = @premium_adj_period_id
			and r.prem_adj_id = @premium_adjustment_id
			and r.custmr_id = @customer_id
			and ca.adj_typ_id in(67,72,66,73) -- Adjustment type IDs for: 'Incurred underlayer','Paid loss underlayer','Incurred Loss WA','Paid loss WA'


			-- Calculate the previous ERP amount by adjustment type id for 'Incurred DEP','Paid loss DEP'
			update dbo.PREM_ADJ_RETRO  WITH (ROWLOCK)
			set prev_ernd_retro_prem_amt = (isnull(tm.sum_erp,0))
			from dbo.PREM_ADJ_RETRO as r
			join 
			(
				select
				prem_adj_pgm_id,
				--coml_agmt_id,
				sum(isnull(d.biled_ernd_retro_prem_amt,0)) as sum_erp
				from dbo.PREM_ADJ_RETRO_DTL d
				where  prem_adj_id = @prev_valid_adj_id
				and custmr_id = @customer_id
				and prem_adj_pgm_id = @premium_adj_prog_id
				group by prem_adj_pgm_id --,coml_agmt_id
			) as tm
			on /*r.coml_agmt_id = tm.coml_agmt_id and*/ r.prem_adj_pgm_id = tm.prem_adj_pgm_id
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = r.coml_agmt_id) and (ca.prem_adj_pgm_id = r.prem_adj_pgm_id) and (ca.custmr_id = r.custmr_id) and (ca.actv_ind = 1)
			where r.prem_adj_perd_id = @premium_adj_period_id
			and r.prem_adj_id = @premium_adjustment_id
			and r.custmr_id = @customer_id
			and ca.adj_typ_id in(63,70) -- Adjustment type IDs for: 'Incurred DEP','Paid loss DEP'


			-- Calculate the previous ERP amount by adjustment type id for 'Incurred Loss WA','Paid loss WA'


			/***********************************************
			* Update prior values for CFB, billed ERP.
			***********************************************/

			update PREM_ADJ_RETRO_DTL WITH (ROWLOCK)
			set prior_cash_flw_ben_amt = src.cash_flw_ben_amt,
				prev_std_subj_prem_amt = src.std_subj_prem_amt,
				rsdl_mkt_load_prev_amt = src.rsdl_mkt_load_ernd_amt,
				prev_biled_ernd_retro_prem_amt = src.biled_ernd_retro_prem_amt
			from dbo.PREM_ADJ_RETRO_DTL dd
			join 
			(
				select
				custmr_id,
				prem_adj_pgm_id,
				prem_adj_id,
				coml_agmt_id,
				st_id,
				biled_ernd_retro_prem_amt,
				std_subj_prem_amt,
				rsdl_mkt_load_ernd_amt,
				--cash_flw_ben_amt
				--case when @prev_perd_code = 'ERP' then cash_flw_ben_amt else -adj_cash_flw_ben_amt end as cash_flw_ben_amt
				case when dbo.fn_PriorCheckAdjCFB(PREM_ADJ_ID,PREM_ADJ_PGM_ID,
				PREM_ADJ_PERD_ID) = 1 then -ADJ_CASH_FLW_BEN_AMT else 
				CASH_FLW_BEN_AMT END as cash_flw_ben_amt
				from dbo.PREM_ADJ_RETRO_DTL 
				where  prem_adj_id = @prev_valid_adj_id
				and custmr_id = @customer_id
				and prem_adj_pgm_id = @premium_adj_prog_id	
	
			) as src
			on dd.coml_agmt_id = src.coml_agmt_id
			and dd.st_id = src.st_id
			and dd.prem_adj_pgm_id = src.prem_adj_pgm_id
			and dd.custmr_id = src.custmr_id
			where dd.prem_adj_perd_id = @premium_adj_period_id
			and dd.prem_adj_id = @premium_adjustment_id
			and dd.custmr_id = @customer_id
			and dd.prem_adj_pgm_id = @premium_adj_prog_id	

			-- Populate fields: prev_subj_paid_idnmty_amt,prev_subj_resrv_idnmty_amt to support ARIES transmittal
			-- ONLY FOR WC UNDERLAYER AND WA 
			update PREM_ADJ_RETRO_DTL WITH (ROWLOCK)
			set prev_subj_paid_idnmty_amt = src.subj_paid_idnmty_amt,
				prev_subj_resrv_idnmty_amt = src.subj_resrv_idnmty_amt
			from dbo.PREM_ADJ_RETRO_DTL dd
			join 
			(
				select
				custmr_id,
				prem_adj_pgm_id,
				prem_adj_id,
				coml_agmt_id,
				st_id,
				subj_paid_idnmty_amt,
				subj_resrv_idnmty_amt
				from dbo.PREM_ADJ_RETRO_DTL 
				where  prem_adj_id = @prev_valid_adj_id
				and custmr_id = @customer_id
				and prem_adj_pgm_id = @premium_adj_prog_id	
	
			) as src on dd.coml_agmt_id = src.coml_agmt_id and dd.st_id = src.st_id and dd.prem_adj_pgm_id = src.prem_adj_pgm_id and dd.custmr_id = src.custmr_id 
			inner join dbo.PREM_ADJ_RETRO h on (dd.prem_adj_retro_id = h.prem_adj_retro_id) and (dd.prem_adj_perd_id = h.prem_adj_perd_id) and (dd.prem_adj_id = h.prem_adj_id) and (dd.custmr_id = h.custmr_id)
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = h.coml_agmt_id) and (ca.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ca.custmr_id = h.custmr_id) and (ca.actv_ind = 1)
			where dd.prem_adj_perd_id = @premium_adj_period_id
			and dd.prem_adj_id = @premium_adjustment_id
			and dd.custmr_id = @customer_id
			and dd.prem_adj_pgm_id = @premium_adj_prog_id
			and ca.adj_typ_id in (66,67,72,73) -- Adjustment type IDs for: 'Incurred Loss WA','Incurred Underlayer','Paid loss WA','Paid Loss Underlayer'

			---------------begin: changes for bug# 10698---------------------
			--As pert of this bug we added prev_subj_paid_idnmty_amt,prev_subj_resrv_idnmty_amt fields to PREM_ADJ_RETRO table 
			--to eliminate the issues with missing states from prior to current adjustment.
			update PREM_ADJ_RETRO WITH (ROWLOCK)
			set prev_subj_paid_idnmty_amt=src.sum_subj_paid_idnmty_amt,
				prev_subj_resrv_idnmty_amt=src.sum_subj_resrv_idnmty_amt
			from dbo.PREM_ADJ_RETRO_DTL dd
			join 
			(
				select
				custmr_id,
				prem_adj_pgm_id,
				prem_adj_id,
				coml_agmt_id,
				sum(isnull(subj_paid_idnmty_amt,0)) as sum_subj_paid_idnmty_amt,
				sum(isnull(subj_resrv_idnmty_amt,0)) as sum_subj_resrv_idnmty_amt
				from dbo.PREM_ADJ_RETRO_DTL 
				where  prem_adj_id = @prev_valid_adj_id
				and custmr_id = @customer_id
				and prem_adj_pgm_id = @premium_adj_prog_id
				group by prem_adj_id,prem_adj_pgm_id,coml_agmt_id,custmr_id		
	
			) as src on dd.coml_agmt_id = src.coml_agmt_id and dd.prem_adj_pgm_id = src.prem_adj_pgm_id and dd.custmr_id = src.custmr_id 
			inner join dbo.PREM_ADJ_RETRO h on (dd.prem_adj_retro_id = h.prem_adj_retro_id) and (dd.prem_adj_perd_id = h.prem_adj_perd_id) and (dd.prem_adj_id = h.prem_adj_id) and (dd.custmr_id = h.custmr_id)
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = h.coml_agmt_id) and (ca.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ca.custmr_id = h.custmr_id) and (ca.actv_ind = 1)
			where dd.prem_adj_perd_id = @premium_adj_period_id
			and dd.prem_adj_id = @premium_adjustment_id
			and dd.custmr_id = @customer_id
			and dd.prem_adj_pgm_id = @premium_adj_prog_id
			and ca.adj_typ_id in (66,67,72,73)




			-- Populate post_idnmty_amt, post_resrv_idnmty_amt for ARIES transmittal
			if(convert(varchar,@next_val_dt,101) = convert(varchar,@conversion_dt,101))
			begin
			update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
			set post_idnmty_amt = isnull(tm.sum_subj_paid_idnmty_amt,0),
				post_resrv_idnmty_amt = isnull(tm.sum_subj_resrv_idnmty_amt,0)
			from dbo.PREM_ADJ_RETRO h 
			inner join 
			(
				select
				d.prem_adj_id,
				d.prem_adj_perd_id,
				ih.coml_agmt_id ,
				sum(isnull(subj_paid_idnmty_amt,0)) as sum_subj_paid_idnmty_amt,
				sum(isnull(subj_resrv_idnmty_amt,0)) as sum_subj_resrv_idnmty_amt
				from dbo.PREM_ADJ_RETRO_DTL d
				inner join dbo.PREM_ADJ_RETRO ih
				on (d.prem_adj_retro_id = ih.prem_adj_retro_id) and (d.prem_adj_perd_id = ih.prem_adj_perd_id) and (d.prem_adj_id = ih.prem_adj_id) and (d.custmr_id = ih.custmr_id)
				where d.prem_adj_perd_id = @premium_adj_period_id
				and d.prem_adj_id = @premium_adjustment_id
				and d.custmr_id = @customer_id
				group by d.prem_adj_id,d.prem_adj_perd_id,ih.coml_agmt_id		
			) as tm
			on (tm.prem_adj_perd_id = h.prem_adj_perd_id) and (tm.prem_adj_id = h.prem_adj_id) and (tm.coml_agmt_id = h.coml_agmt_id)
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = h.coml_agmt_id) and (ca.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ca.custmr_id = h.custmr_id) and (ca.actv_ind = 1)
			where h.prem_adj_perd_id = @premium_adj_period_id
			and h.prem_adj_id = @premium_adjustment_id
			and h.custmr_id = @customer_id
			and ca.adj_typ_id in(67,72,66,73)
			end
			else
			begin
			update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
			set post_idnmty_amt = isnull(tm.sum_subj_paid_idnmty_amt,0) - isnull(h.prev_subj_paid_idnmty_amt,0),
				post_resrv_idnmty_amt = isnull(tm.sum_subj_resrv_idnmty_amt,0) - isnull(h.prev_subj_resrv_idnmty_amt,0)
			from dbo.PREM_ADJ_RETRO h 
			inner join 
			(
				select
				d.prem_adj_id,
				d.prem_adj_perd_id,
				ih.coml_agmt_id ,
				sum(isnull(subj_paid_idnmty_amt,0)) as sum_subj_paid_idnmty_amt,
				sum(isnull(subj_resrv_idnmty_amt,0)) as sum_subj_resrv_idnmty_amt
				from dbo.PREM_ADJ_RETRO_DTL d
				inner join dbo.PREM_ADJ_RETRO ih
				on (d.prem_adj_retro_id = ih.prem_adj_retro_id) and (d.prem_adj_perd_id = ih.prem_adj_perd_id) and (d.prem_adj_id = ih.prem_adj_id) and (d.custmr_id = ih.custmr_id)
				where d.prem_adj_perd_id = @premium_adj_period_id
				and d.prem_adj_id = @premium_adjustment_id
				and d.custmr_id = @customer_id
				group by d.prem_adj_id,d.prem_adj_perd_id,ih.coml_agmt_id		
			) as tm
			on (tm.prem_adj_perd_id = h.prem_adj_perd_id) and (tm.prem_adj_id = h.prem_adj_id) and (tm.coml_agmt_id = h.coml_agmt_id)
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = h.coml_agmt_id) and (ca.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ca.custmr_id = h.custmr_id) and (ca.actv_ind = 1)
			where h.prem_adj_perd_id = @premium_adj_period_id
			and h.prem_adj_id = @premium_adjustment_id
			and h.custmr_id = @customer_id
			and ca.adj_typ_id in(67,72,66,73)
			end
			-------End changes for bug # 10698-----------------------------------------------------

		--Populate Paid Loss Billing amount from the PLB table

		--For DEP, PLB need to roll up to the master policiy in RETRO table
		-- even if in PLB table entries associated for the underlying policies.
		if (@pgm_type = 1 ) --DEP Program Type
		begin
			update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
			set paid_los_bil_amt = round(isnull(tm.total_plb_amt,0),0)
			from dbo.PREM_ADJ_RETRO as r
			join 
			(
				select
				prem_adj_perd_id,
				prem_adj_id,
				sum(adj_tot_paid_los_bil_amt) as total_plb_amt
				from dbo.PREM_ADJ_PAID_LOS_BIL
				where prem_adj_perd_id = @premium_adj_period_id
				and prem_adj_id = @premium_adjustment_id
				and custmr_id = @customer_id
				group by prem_adj_perd_id,prem_adj_id
			) as tm
			on r.prem_adj_perd_id = tm.prem_adj_perd_id
			and r.prem_adj_id = tm.prem_adj_id
			--and r.coml_agmt_id = tm.coml_agmt_id
			where r.custmr_id = @customer_id
			and r.prem_adj_perd_id = @premium_adj_period_id
			and r.prem_adj_id = @premium_adjustment_id  
		end
		else
		begin
			update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
			set paid_los_bil_amt = round(isnull(tm.total_plb_amt,0),0)
			from dbo.PREM_ADJ_RETRO as r
			join 
			(
				select
				coml_agmt_id,
				prem_adj_perd_id,
				prem_adj_id,
				sum(adj_tot_paid_los_bil_amt) as total_plb_amt
				from dbo.PREM_ADJ_PAID_LOS_BIL
				where prem_adj_perd_id = @premium_adj_period_id
				and prem_adj_id = @premium_adjustment_id
				and custmr_id = @customer_id
				group by coml_agmt_id,prem_adj_perd_id,prem_adj_id
			) as tm
			on r.prem_adj_perd_id = tm.prem_adj_perd_id
			and r.prem_adj_id = tm.prem_adj_id
			and r.coml_agmt_id = tm.coml_agmt_id
			where r.custmr_id = @customer_id
			and r.prem_adj_perd_id = @premium_adj_period_id
			and r.prem_adj_id = @premium_adjustment_id  
		end


		declare @prev_subj_depst_prem_amt decimal(15,2)
		declare @prev_ns_audt_prem_amt decimal(15,2)
		declare @prev_non_subj_depst_prem_amt decimal(15,2)
		declare @revised_subj_depst_prem_amt decimal(15,2)
		declare @revised_ns_audt_prem_amt decimal(15,2)
		declare @revised_non_subj_depst_prem_amt decimal(15,2)
		declare @audit_revised_subj_depst_prem_amt decimal(15,2)
		declare @audit_revised_ns_audt_prem_amt decimal(15,2)
		declare @audit_revised_non_subj_depst_prem_amt decimal(15,2)
		declare @historical_prev_subj_depst_prem_amt decimal(15,2)
		declare @historical_prev_ns_audt_prem_amt decimal(15,2)
		declare @historical_prev_non_subj_depst_prem_amt decimal(15,2)
		declare @rel_prem_adj_id int
		declare @revision_ind bit
		declare @rel_prem_adj_perd_id int
		declare @rel_prem_adj_pgm_id int
		declare @policy_cnt_prev int
		declare @historical_adj_ind int

		declare audit_cur cursor LOCAL FAST_FORWARD READ_ONLY 
		for 
		select 
		coml_agmt_id 
		from  prem_adj_retro 
		where prem_adj_id=@premium_adjustment_id
		and prem_adj_perd_id=@premium_adj_period_id

		open audit_cur
		fetch audit_cur into @com_agm_audt_id
		while @@Fetch_Status = 0
			begin
				begin
					set @prev_subj_depst_prem_amt=null
					set @prev_ns_audt_prem_amt=null
					set @prev_non_subj_depst_prem_amt=null
					set @revision_ind=null
					set @rel_prem_adj_id=null
					set @revised_subj_depst_prem_amt=null
					set @revised_ns_audt_prem_amt=null
					set @revised_non_subj_depst_prem_amt=null
					set @audit_revised_subj_depst_prem_amt=null
					set @audit_revised_ns_audt_prem_amt=null
					set @audit_revised_non_subj_depst_prem_amt=null
					set @historical_prev_subj_depst_prem_amt=null
					set @historical_prev_ns_audt_prem_amt=null
					set @historical_prev_non_subj_depst_prem_amt=null
		            if @debug = 1
					begin
					print'*******************AUDIT CALC: START OF ITERATION IN FIRST PASS*********' 
				    print'---------------Input Params-------------------' 

					print' @com_agm_id:- ' + convert(varchar(20), @com_agm_audt_id)  
					
					end
        
		select @revision_ind=adj_rrsn_ind from prem_adj where prem_adj_id 
						in(select rel_prem_adj_id from prem_adj where prem_adj_id=@premium_adjustment_id)
						and substring(fnl_invc_nbr_txt,1,3)<>'RTV'
        select @rel_prem_adj_id=rel_prem_adj_id from prem_adj where prem_adj_id=@premium_adjustment_id
		select @rel_prem_adj_pgm_id=prem_adj_pgm_id from prem_adj_perd where prem_adj_perd_id=@premium_adj_period_id
		select @rel_prem_adj_perd_id=prem_adj_perd_id from prem_adj_perd where prem_adj_pgm_id=@rel_prem_adj_pgm_id and prem_adj_id=@rel_prem_adj_id
		select @policy_cnt_prev=count(*) from prem_adj_retro where prem_adj_id=@prev_valid_adj_id and coml_agmt_id=@com_agm_audt_id
		select @historical_adj_ind=historical_adj_ind from prem_adj where prem_adj_id=@premium_adjustment_id



		select 
		@prev_ns_audt_prem_amt = caa.non_subj_audt_prem_amt
		from dbo.PREM_ADJ_RETRO par
		inner join dbo.COML_AGMT_AUDT caa on (par.coml_agmt_id = caa.coml_agmt_id) and (par.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (par.custmr_id = caa.custmr_id)
		inner join coml_agmt on coml_agmt.coml_agmt_id = par.coml_agmt_id and coml_agmt.actv_ind = 1
		where par.prem_adj_perd_id = @premium_adj_period_id
		and par.prem_adj_id = @premium_adjustment_id
		and par.custmr_id = @customer_id
		and caa.coml_agmt_id=@com_agm_audt_id
		and (caa.audt_revd_sts_ind = 1)
		and caa.crte_dt in
		(
			select 
			max(caa.crte_dt)
			from dbo.PREM_ADJ_RETRO par
			inner join dbo.COML_AGMT_AUDT caa on (par.coml_agmt_id = caa.coml_agmt_id) and (par.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (par.custmr_id = caa.custmr_id)
			inner join coml_agmt on coml_agmt.coml_agmt_id = par.coml_agmt_id and coml_agmt.actv_ind = 1
			where par.prem_adj_perd_id = @premium_adj_period_id
			and par.prem_adj_id = @premium_adjustment_id
			and par.custmr_id = @customer_id
			and caa.coml_agmt_id=@com_agm_audt_id
			and (caa.audt_revd_sts_ind = 1)
		)
		print' @prev_ns_audt_prem_amt:- ' + convert(varchar(20), @prev_ns_audt_prem_amt)  
		
		select 
		@prev_non_subj_depst_prem_amt = caa.non_subj_depst_prem_amt
		from dbo.PREM_ADJ_RETRO par
		inner join dbo.COML_AGMT_AUDT caa on (par.coml_agmt_id = caa.coml_agmt_id) and (par.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (par.custmr_id = caa.custmr_id)
		inner join coml_agmt on coml_agmt.coml_agmt_id = par.coml_agmt_id and coml_agmt.actv_ind = 1
		where par.prem_adj_perd_id = @premium_adj_period_id
		and par.prem_adj_id = @premium_adjustment_id
		and par.custmr_id = @customer_id
		and caa.coml_agmt_id=@com_agm_audt_id
		and (caa.audt_revd_sts_ind = 1)
		and caa.crte_dt in
		(
			select 
			max(caa.crte_dt)
			from dbo.PREM_ADJ_RETRO par
			inner join dbo.COML_AGMT_AUDT caa on (par.coml_agmt_id = caa.coml_agmt_id) and (par.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (par.custmr_id = caa.custmr_id)
			inner join coml_agmt on coml_agmt.coml_agmt_id = par.coml_agmt_id and coml_agmt.actv_ind = 1
			where par.prem_adj_perd_id = @premium_adj_period_id
			and par.prem_adj_id = @premium_adjustment_id
			and par.custmr_id = @customer_id
			and caa.coml_agmt_id=@com_agm_audt_id
			and (caa.audt_revd_sts_ind = 1)
		)
		print' @prev_non_subj_depst_prem_amt:- ' + convert(varchar(20), @prev_non_subj_depst_prem_amt)  
		
		select 
		@prev_subj_depst_prem_amt = caa.subj_depst_prem_amt
		from dbo.PREM_ADJ_RETRO par
		inner join dbo.COML_AGMT_AUDT caa on (par.coml_agmt_id = caa.coml_agmt_id) and (par.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (par.custmr_id = caa.custmr_id)
		inner join coml_agmt on coml_agmt.coml_agmt_id = par.coml_agmt_id and coml_agmt.actv_ind = 1
		where par.prem_adj_perd_id = @premium_adj_period_id
		and par.prem_adj_id = @premium_adjustment_id
		and par.custmr_id = @customer_id
		and caa.coml_agmt_id=@com_agm_audt_id
		and (caa.audt_revd_sts_ind = 1)
		and caa.crte_dt in
		(
			select 
			max(caa.crte_dt)
			from dbo.PREM_ADJ_RETRO par
			inner join dbo.COML_AGMT_AUDT caa on (par.coml_agmt_id = caa.coml_agmt_id) and (par.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (par.custmr_id = caa.custmr_id)
			inner join coml_agmt on coml_agmt.coml_agmt_id = par.coml_agmt_id and coml_agmt.actv_ind = 1
			where par.prem_adj_perd_id = @premium_adj_period_id
			and par.prem_adj_id = @premium_adjustment_id
			and par.custmr_id = @customer_id
			and caa.coml_agmt_id=@com_agm_audt_id
			and (caa.audt_revd_sts_ind = 1)
		)
		print' @prev_subj_depst_prem_amt:- ' + convert(varchar(20), @prev_subj_depst_prem_amt)  
		if (@cnt_prev_adjs = 0) --  This is initial adjustment
		begin
			--Populate fields in retro header table with values from Commercial Agreement Audit table
			if(@historical_adj_ind=1)
			begin
			select @historical_prev_subj_depst_prem_amt=subj_depst_prem_amt,
				   @historical_prev_ns_audt_prem_amt=non_subj_audt_prem_amt,
				   @historical_prev_non_subj_depst_prem_amt=non_subj_depst_prem_amt
				   from COML_AGMT_AUDT 
				   where custmr_id = @customer_id
					and coml_agmt_id=@com_agm_audt_id
					and audt_revd_sts_ind=1
					and adj_ind=1
					and crte_dt in
								(
									select 
									max(crte_dt)
									from COML_AGMT_AUDT 
									where custmr_id = @customer_id
									and coml_agmt_id=@com_agm_audt_id
									and audt_revd_sts_ind=1
									and adj_ind=1
								)

			
			update dbo.PREM_ADJ_RETRO --WITH (ROWLOCK)
			set subj_depst_prem_amt = case when (isnull(caa.subj_depst_prem_amt,0)-isnull(@historical_prev_subj_depst_prem_amt,0))<>0
											     then (isnull(caa.subj_depst_prem_amt,0)-isnull(@historical_prev_subj_depst_prem_amt,0))
												 else NULL end,
				non_subj_audt_prem_amt = case when (isnull(caa.non_subj_audt_prem_amt,0)-isnull(@historical_prev_ns_audt_prem_amt,0))<>0
													then (isnull(caa.non_subj_audt_prem_amt,0)-isnull(@historical_prev_ns_audt_prem_amt,0))
													else NULL end, 
				non_subj_depst_prem_amt = case when (isnull(caa.non_subj_depst_prem_amt,0)-isnull(@historical_prev_non_subj_depst_prem_amt,0))<>0
											         then (isnull(caa.non_subj_depst_prem_amt,0)-isnull(@historical_prev_non_subj_depst_prem_amt,0))
													 else NULL end
			from dbo.PREM_ADJ_RETRO par
			inner join dbo.COML_AGMT_AUDT caa on (par.coml_agmt_id = caa.coml_agmt_id) and (par.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (par.custmr_id = caa.custmr_id)
			where par.prem_adj_perd_id = @premium_adj_period_id
			and par.prem_adj_id = @premium_adjustment_id
			and par.custmr_id = @customer_id
			and caa.coml_agmt_id=@com_agm_audt_id
			and caa.adj_ind = 0 -- applies when not used in adj
			and (caa.audt_revd_sts_ind = 0 or caa.audt_revd_sts_ind is null)

			end
			else
			begin
			update dbo.PREM_ADJ_RETRO --WITH (ROWLOCK)
			set subj_depst_prem_amt = isnull(caa.subj_depst_prem_amt,0),
				non_subj_audt_prem_amt = isnull(caa.non_subj_audt_prem_amt,0), --- isnull(@prev_ns_audt_prem_amt,0),
				non_subj_depst_prem_amt = isnull(caa.non_subj_depst_prem_amt,0)
			from dbo.PREM_ADJ_RETRO par
			inner join dbo.COML_AGMT_AUDT caa on (par.coml_agmt_id = caa.coml_agmt_id) and (par.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (par.custmr_id = caa.custmr_id)
			where par.prem_adj_perd_id = @premium_adj_period_id
			and par.prem_adj_id = @premium_adjustment_id
			and par.custmr_id = @customer_id
			and caa.coml_agmt_id=@com_agm_audt_id
			and caa.adj_ind = 0 -- applies when not used in adj
			and (caa.audt_revd_sts_ind = 0 or caa.audt_revd_sts_ind is null) -- if revision flag checked (revised), use new subsequent adjustment
			end

		end
		else
         begin
			
			if(@prev_subj_depst_prem_amt is not null)
			begin
					select @audit_revised_subj_depst_prem_amt=caa.subj_depst_prem_amt 
							from dbo.PREM_ADJ_RETRO par
							inner join dbo.COML_AGMT_AUDT caa on (par.coml_agmt_id = caa.coml_agmt_id) and (par.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (par.custmr_id = caa.custmr_id)
							inner join coml_agmt on coml_agmt.coml_agmt_id = par.coml_agmt_id and coml_agmt.actv_ind = 1
							where par.prem_adj_perd_id = @premium_adj_period_id
							and par.prem_adj_id = @premium_adjustment_id
							and par.custmr_id = @customer_id
							and caa.coml_agmt_id=@com_agm_audt_id
							and caa.adj_ind = 0 -- applies when not used in adj
							and (caa.audt_revd_sts_ind = 0 or caa.audt_revd_sts_ind is null)
					if(@revision_ind=1 and @audit_revised_subj_depst_prem_amt is null)
						begin
							select @revised_subj_depst_prem_amt=par.subj_depst_prem_amt 
							from dbo.PREM_ADJ_RETRO par
							where par.prem_adj_perd_id = @rel_prem_adj_perd_id
							and par.prem_adj_id = @rel_prem_adj_id
							and par.custmr_id = @customer_id
							and par.coml_agmt_id=@com_agm_audt_id
							
							update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
							set subj_depst_prem_amt=@revised_subj_depst_prem_amt
							where prem_adj_perd_id = @premium_adj_period_id
							and prem_adj_id = @premium_adjustment_id
							and custmr_id = @customer_id
							and coml_agmt_id=@com_agm_audt_id

						end
					else
						begin			
					
					update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
					set subj_depst_prem_amt = case when (isnull(caa.subj_depst_prem_amt,0)-isnull(@prev_subj_depst_prem_amt,0))<>0
												then (isnull(caa.subj_depst_prem_amt,0)-isnull(@prev_subj_depst_prem_amt,0))
												else NULL end
						
					from dbo.PREM_ADJ_RETRO par
					inner join dbo.COML_AGMT_AUDT caa on (par.coml_agmt_id = caa.coml_agmt_id) and (par.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (par.custmr_id = caa.custmr_id)
					inner join coml_agmt on coml_agmt.coml_agmt_id = par.coml_agmt_id and coml_agmt.actv_ind = 1
					where par.prem_adj_perd_id = @premium_adj_period_id
					and par.prem_adj_id = @premium_adjustment_id
					and par.custmr_id = @customer_id
					and caa.coml_agmt_id=@com_agm_audt_id
					and caa.adj_ind = 0 -- applies when not used in adj
					and (caa.audt_revd_sts_ind = 0 or caa.audt_revd_sts_ind is null)
					end
			end
				else
                    begin
						if(@policy_cnt_prev<>0)
							begin
								update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
								set subj_depst_prem_amt = NULL
								where prem_adj_perd_id = @premium_adj_period_id
								and prem_adj_id = @premium_adjustment_id
								and custmr_id = @customer_id
								and coml_agmt_id=@com_agm_audt_id
							end
						else
							begin
								update dbo.PREM_ADJ_RETRO --WITH (ROWLOCK)
								set subj_depst_prem_amt = isnull(caa.subj_depst_prem_amt,0),
									non_subj_audt_prem_amt = isnull(caa.non_subj_audt_prem_amt,0), --- isnull(@prev_ns_audt_prem_amt,0),
									non_subj_depst_prem_amt = isnull(caa.non_subj_depst_prem_amt,0)
								from dbo.PREM_ADJ_RETRO par
								inner join dbo.COML_AGMT_AUDT caa on (par.coml_agmt_id = caa.coml_agmt_id) and (par.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (par.custmr_id = caa.custmr_id)
								inner join coml_agmt on coml_agmt.coml_agmt_id = par.coml_agmt_id and coml_agmt.actv_ind = 1
								where par.prem_adj_perd_id = @premium_adj_period_id
								and par.prem_adj_id = @premium_adjustment_id
								and par.custmr_id = @customer_id
								and caa.coml_agmt_id=@com_agm_audt_id
								and caa.adj_ind = 0 -- applies when not used in adj
								and (caa.audt_revd_sts_ind = 0 or caa.audt_revd_sts_ind is null)
							end
		
					end
			if(@prev_ns_audt_prem_amt is not null)
			begin
			select @audit_revised_ns_audt_prem_amt=caa.non_subj_audt_prem_amt 
							from dbo.PREM_ADJ_RETRO par
							inner join dbo.COML_AGMT_AUDT caa on (par.coml_agmt_id = caa.coml_agmt_id) and (par.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (par.custmr_id = caa.custmr_id)
							inner join coml_agmt on coml_agmt.coml_agmt_id = par.coml_agmt_id and coml_agmt.actv_ind = 1
							where par.prem_adj_perd_id = @premium_adj_period_id
							and par.prem_adj_id = @premium_adjustment_id
							and par.custmr_id = @customer_id
							and caa.coml_agmt_id=@com_agm_audt_id
							and caa.adj_ind = 0 -- applies when not used in adj
							and (caa.audt_revd_sts_ind = 0 or caa.audt_revd_sts_ind is null)
			if(@revision_ind=1 and @audit_revised_ns_audt_prem_amt is null)
						begin
							select @revised_ns_audt_prem_amt=par.non_subj_audt_prem_amt 
							from dbo.PREM_ADJ_RETRO par
							where par.prem_adj_perd_id = @rel_prem_adj_perd_id
							and par.prem_adj_id = @rel_prem_adj_id
							and par.custmr_id = @customer_id
							and par.coml_agmt_id=@com_agm_audt_id
							
							update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
							set non_subj_audt_prem_amt=@revised_ns_audt_prem_amt
							where prem_adj_perd_id = @premium_adj_period_id
							and prem_adj_id = @premium_adjustment_id
							and custmr_id = @customer_id
							and coml_agmt_id=@com_agm_audt_id

						end
					else
						begin		

			update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
			set non_subj_audt_prem_amt = case when (isnull(caa.non_subj_audt_prem_amt,0) - isnull(@prev_ns_audt_prem_amt,0))<>0
                                         then (isnull(caa.non_subj_audt_prem_amt,0) - isnull(@prev_ns_audt_prem_amt,0))
										 else NULL end
				
			from dbo.PREM_ADJ_RETRO par
			inner join dbo.COML_AGMT_AUDT caa on (par.coml_agmt_id = caa.coml_agmt_id) and (par.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (par.custmr_id = caa.custmr_id)
			inner join coml_agmt on coml_agmt.coml_agmt_id = par.coml_agmt_id and coml_agmt.actv_ind = 1
			where par.prem_adj_perd_id = @premium_adj_period_id
			and par.prem_adj_id = @premium_adjustment_id
			and par.custmr_id = @customer_id
			and caa.coml_agmt_id=@com_agm_audt_id
			and caa.adj_ind = 0 -- applies when not used in adj
			and (caa.audt_revd_sts_ind = 0 or caa.audt_revd_sts_ind is null)
				end
			end
				else
                    begin
						if(@policy_cnt_prev<>0)
							begin
									update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
									set non_subj_audt_prem_amt = NULL
									where prem_adj_perd_id = @premium_adj_period_id
									and prem_adj_id = @premium_adjustment_id
									and custmr_id = @customer_id
									and coml_agmt_id=@com_agm_audt_id
							end
						else
							begin
								update dbo.PREM_ADJ_RETRO --WITH (ROWLOCK)
								set subj_depst_prem_amt = isnull(caa.subj_depst_prem_amt,0),
									non_subj_audt_prem_amt = isnull(caa.non_subj_audt_prem_amt,0), --- isnull(@prev_ns_audt_prem_amt,0),
									non_subj_depst_prem_amt = isnull(caa.non_subj_depst_prem_amt,0)
								from dbo.PREM_ADJ_RETRO par
								inner join dbo.COML_AGMT_AUDT caa on (par.coml_agmt_id = caa.coml_agmt_id) and (par.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (par.custmr_id = caa.custmr_id)
								inner join coml_agmt on coml_agmt.coml_agmt_id = par.coml_agmt_id and coml_agmt.actv_ind = 1
								where par.prem_adj_perd_id = @premium_adj_period_id
								and par.prem_adj_id = @premium_adjustment_id
								and par.custmr_id = @customer_id
								and caa.coml_agmt_id=@com_agm_audt_id
								and caa.adj_ind = 0 -- applies when not used in adj
								and (caa.audt_revd_sts_ind = 0 or caa.audt_revd_sts_ind is null)
							end
					end
			
			if(@prev_non_subj_depst_prem_amt is not null)
			begin
			select @audit_revised_non_subj_depst_prem_amt=caa.non_subj_depst_prem_amt 
							from dbo.PREM_ADJ_RETRO par
							inner join dbo.COML_AGMT_AUDT caa on (par.coml_agmt_id = caa.coml_agmt_id) and (par.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (par.custmr_id = caa.custmr_id)
							inner join coml_agmt on coml_agmt.coml_agmt_id = par.coml_agmt_id and coml_agmt.actv_ind = 1
							where par.prem_adj_perd_id = @premium_adj_period_id
							and par.prem_adj_id = @premium_adjustment_id
							and par.custmr_id = @customer_id
							and caa.coml_agmt_id=@com_agm_audt_id
							and caa.adj_ind = 0 -- applies when not used in adj
							and (caa.audt_revd_sts_ind = 0 or caa.audt_revd_sts_ind is null)
			if(@revision_ind=1 and @audit_revised_non_subj_depst_prem_amt is null)
						begin
							select @revised_non_subj_depst_prem_amt=par.non_subj_depst_prem_amt 
							from dbo.PREM_ADJ_RETRO par
							where par.prem_adj_perd_id = @rel_prem_adj_perd_id
							and par.prem_adj_id = @rel_prem_adj_id
							and par.custmr_id = @customer_id
							and par.coml_agmt_id=@com_agm_audt_id
							
							update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
							set non_subj_depst_prem_amt=@revised_non_subj_depst_prem_amt
							where prem_adj_perd_id = @premium_adj_period_id
							and prem_adj_id = @premium_adjustment_id
							and custmr_id = @customer_id
							and coml_agmt_id=@com_agm_audt_id

						end
					else
						begin	
			update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
			set non_subj_depst_prem_amt = case when (isnull(caa.non_subj_depst_prem_amt,0)-isnull(@prev_non_subj_depst_prem_amt,0))<>0
										   then (isnull(caa.non_subj_depst_prem_amt,0)-isnull(@prev_non_subj_depst_prem_amt,0))
											else NULL end
			from dbo.PREM_ADJ_RETRO par
			inner join dbo.COML_AGMT_AUDT caa on (par.coml_agmt_id = caa.coml_agmt_id) and (par.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (par.custmr_id = caa.custmr_id)
			inner join coml_agmt on coml_agmt.coml_agmt_id = par.coml_agmt_id and coml_agmt.actv_ind = 1
			where par.prem_adj_perd_id = @premium_adj_period_id
			and par.prem_adj_id = @premium_adjustment_id
			and par.custmr_id = @customer_id
			and par.coml_agmt_id=@com_agm_audt_id
			and caa.adj_ind = 0 -- applies when not used in adj
			and (caa.audt_revd_sts_ind = 0 or caa.audt_revd_sts_ind is null)
			end
			end
				else
                    begin
						if(@policy_cnt_prev<>0)
							begin
								update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
								set non_subj_depst_prem_amt = NULL
								where prem_adj_perd_id = @premium_adj_period_id
								and prem_adj_id = @premium_adjustment_id
								and custmr_id = @customer_id
								and coml_agmt_id=@com_agm_audt_id
							end
						else
							begin
								update dbo.PREM_ADJ_RETRO --WITH (ROWLOCK)
								set subj_depst_prem_amt = isnull(caa.subj_depst_prem_amt,0),
									non_subj_audt_prem_amt = isnull(caa.non_subj_audt_prem_amt,0), --- isnull(@prev_ns_audt_prem_amt,0),
									non_subj_depst_prem_amt = isnull(caa.non_subj_depst_prem_amt,0)
								from dbo.PREM_ADJ_RETRO par
								inner join dbo.COML_AGMT_AUDT caa on (par.coml_agmt_id = caa.coml_agmt_id) and (par.prem_adj_pgm_id = caa.prem_adj_pgm_id) and (par.custmr_id = caa.custmr_id)
								inner join coml_agmt on coml_agmt.coml_agmt_id = par.coml_agmt_id and coml_agmt.actv_ind = 1
								where par.prem_adj_perd_id = @premium_adj_period_id
								and par.prem_adj_id = @premium_adjustment_id
								and par.custmr_id = @customer_id
								and caa.coml_agmt_id=@com_agm_audt_id
								and caa.adj_ind = 0 -- applies when not used in adj
								and (caa.audt_revd_sts_ind = 0 or caa.audt_revd_sts_ind is null)
							end
					end

		 end

			end
					fetch audit_cur into @com_agm_audt_id
				end --end of cursor audit_cur / while loop
			close audit_cur
			deallocate audit_cur
		--Populate Misc. Postings amount to Misc. Amount field
		update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
			set misc_amt = isnull(tm.sum_post_amt,0)
		from dbo.PREM_ADJ_RETRO h 
		inner join 
		(
			select 
			ret.coml_agmt_id,
			sum( isnull(mi.post_amt,0) ) as sum_post_amt
			from dbo.PREM_ADJ_RETRO ret
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = ret.coml_agmt_id) and ca.actv_ind = 1
			inner join dbo.PREM_ADJ_MISC_INVC mi on (ca.pol_sym_txt = mi.pol_sym_txt) and (ca.pol_nbr_txt = mi.pol_nbr_txt) and (ca.pol_modulus_txt = mi.pol_modulus_txt)
			inner join dbo.POST_TRNS_TYP ptt on (mi.post_trns_typ_id = ptt.post_trns_typ_id)
			where mi.prem_adj_perd_id = @premium_adj_period_id
			and ret.prem_adj_id = @premium_adjustment_id 
			and ptt.adj_sumry_ind = 1
			and ptt.pol_reqr_ind = 1 
			and ptt.post_ind is not null
			and ptt.trns_typ_id = 444
			and mi.actv_ind = 1 
			group by ret.coml_agmt_id
		) as tm
		on (tm.coml_agmt_id = h.coml_agmt_id)
		--inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = h.coml_agmt_id) and (ca.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ca.custmr_id = h.custmr_id)
		where h.prem_adj_perd_id = @premium_adj_period_id
		and h.prem_adj_id = @premium_adjustment_id
		and h.custmr_id = @customer_id

		if (@is_peo = 1) -- PEO adjustment
		begin
			select 
			@peo_pay_in_amt = peo_pay_in_amt
			from dbo.PREM_ADJ_PGM
			where prem_adj_pgm_id = @premium_adj_prog_id

			create table #peo_policies
			(
			id int identity(1,1),
			coml_agmt_id int
			)

			create index ind ON #peo_policies (id)

			insert into #peo_policies(coml_agmt_id)
			select r.coml_agmt_id
			from dbo.PREM_ADJ_RETRO r
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = r.coml_agmt_id) and ca.actv_ind = 1
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id
			--and adj_typ_id not in (63,70) -- Adjustment types for: 'Incurred DEP','Paid Loss DEP'

			select @count = count(*) from #peo_policies

			set @counter = 1

			while @counter <= @count
			begin
				select @peo_com_agm_id = coml_agmt_id
				from #peo_policies 
				where id = @counter
				if @debug = 1
				begin
				print '----PEO table-------'
				print '@peo_com_agm_id:' + convert(varchar(20),@peo_com_agm_id)
				end

				exec @peo_payin_ratio = [dbo].[fn_ComputePEOPayInRatio]
												@premium_adj_perd_id = @premium_adj_period_id,
												@premium_adj_id = @premium_adjustment_id,
												@coml_agmt_id = @peo_com_agm_id

				update dbo.PREM_ADJ_RETRO  WITH (ROWLOCK)
				set peo_pay_in_amt = @peo_pay_in_amt * (@peo_payin_ratio / 100)
				where prem_adj_id = @premium_adjustment_id
				and prem_adj_perd_id = @premium_adj_period_id
				and coml_agmt_id = @peo_com_agm_id

--				--TODO: CLARIFY
--				update dbo.PREM_ADJ_RETRO  WITH (ROWLOCK)
--				set subj_depst_prem_amt = NULL,
--					non_subj_depst_prem_amt = NULL
--				from dbo.PREM_ADJ_RETRO r
--				inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = r.coml_agmt_id)
--				where  r.prem_adj_id = @premium_adjustment_id
--				and r.prem_adj_perd_id = @premium_adj_period_id
--				and ca.coml_agmt_id = @peo_com_agm_id
--				--and adj_typ_id not in (63,70) -- Adjustment types for: 'Incurred DEP','Paid Loss DEP'

				set @counter = @counter + 1
			end

			--TODO: CLARIFY
			update dbo.PREM_ADJ_RETRO  WITH (ROWLOCK)
			set subj_depst_prem_amt = NULL,
				non_subj_depst_prem_amt = NULL
			from dbo.PREM_ADJ_RETRO r
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = r.coml_agmt_id) and ca.actv_ind = 1
			where  prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id
			--and adj_typ_id not in (63,70) -- Adjustment types for: 'Incurred DEP','Paid Loss DEP'

			

			-- PEO: Calculate the final invoice amount by adjustment type id for 'Incurred loss retro','Paid loss retro'
			update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
				set invc_amt = isnull(tm.sum_incur_erp_amt,0) + isnull(tm.sum_cfb_amt,0) - isnull(h.subj_depst_prem_amt,0)
				+ isnull(h.non_subj_audt_prem_amt,0) - isnull(h.non_subj_depst_prem_amt,0) - isnull(h.paid_los_bil_amt,0)
				- isnull(h.prev_ernd_retro_prem_amt,0) + isnull(h.misc_amt,0) + isnull(tm.sum_prior_cfb_amt,0) - isnull(h.peo_pay_in_amt,0)
			from dbo.PREM_ADJ_RETRO h 
			inner join 
			(
				select
				d.prem_adj_id,
				d.prem_adj_perd_id,
				ih.coml_agmt_id ,
				sum(isnull(cesar_cd_tot_amt,0)) as sum_incur_erp_amt,
				--sum(isnull(d.adj_cash_flw_ben_amt,0)) as sum_cfb_amt,
				case when dbo.fn_CheckAdjCFB(d.PREM_ADJ_ID,d.PREM_ADJ_PGM_ID,
				d.PREM_ADJ_PERD_ID) = 1 then (SUM(isnull(d.ADJ_CASH_FLW_BEN_AMT,0))) else 
				-(SUM(isnull(d.CASH_FLW_BEN_AMT,0))) END as sum_cfb_amt,
				sum(isnull(prior_cash_flw_ben_amt,0)) as sum_prior_cfb_amt
				from dbo.PREM_ADJ_RETRO_DTL d
				inner join dbo.PREM_ADJ_RETRO ih
				on (d.prem_adj_retro_id = ih.prem_adj_retro_id) and (d.prem_adj_perd_id = ih.prem_adj_perd_id) and (d.prem_adj_id = ih.prem_adj_id) and (d.custmr_id = ih.custmr_id)
				where d.prem_adj_perd_id = @premium_adj_period_id
				and d.prem_adj_id = @premium_adjustment_id
				and d.custmr_id = @customer_id
				group by d.prem_adj_id,d.prem_adj_perd_id,ih.coml_agmt_id,d.prem_adj_pgm_id		
			) as tm
			on (tm.prem_adj_perd_id = h.prem_adj_perd_id) and (tm.prem_adj_id = h.prem_adj_id) and (tm.coml_agmt_id = h.coml_agmt_id)
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = h.coml_agmt_id) and (ca.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ca.custmr_id = h.custmr_id) and (ca.actv_ind = 1)
			where h.prem_adj_perd_id = @premium_adj_period_id
			and h.prem_adj_id = @premium_adjustment_id
			and h.custmr_id = @customer_id
			and ca.adj_typ_id in(65,71) -- Adjustment type IDs for: 'Incurred loss retro','Paid loss retro'

			-- PEO: Calculate the final invoice amount by adjustment type id for 'Incurred underlayer','Paid loss underlayer','Incurred Loss WA','Paid loss WA'
			update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
				set invc_amt = isnull(tm.sum_incur_erp_amt,0) + isnull(tm.sum_cfb_amt,0) - isnull(h.subj_depst_prem_amt,0)
				+ isnull(h.non_subj_audt_prem_amt,0) - isnull(h.non_subj_depst_prem_amt,0) - isnull(h.paid_los_bil_amt,0)
				- isnull(h.prev_ernd_retro_prem_amt,0) + isnull(h.misc_amt,0) + isnull(tm.sum_prior_cfb_amt,0) + isnull(tm.sum_adj_ded_wc_loss_amt,0) - isnull(h.peo_pay_in_amt,0)
			from dbo.PREM_ADJ_RETRO h 
			inner join 
			(
				select
				d.prem_adj_id,
				d.prem_adj_perd_id,
				ih.coml_agmt_id ,
				sum(isnull(cesar_cd_tot_amt,0)) as sum_incur_erp_amt,
--				case when @perd_code = 'ERP' then sum(isnull(adj_dedtbl_wrk_comp_los_amt,0)) else 0 end as sum_adj_ded_wc_loss_amt,
				--bug 9698
				case when @perd_code = 'ERP' then sum(isnull(adj_dedtbl_wrk_comp_los_amt,0)) 
					else 0 end as sum_adj_ded_wc_loss_amt,
				case when dbo.fn_CheckAdjCFB(d.PREM_ADJ_ID,d.PREM_ADJ_PGM_ID,
				d.PREM_ADJ_PERD_ID) = 1 then (SUM(isnull(d.ADJ_CASH_FLW_BEN_AMT,0))) else 
				-(SUM(isnull(d.CASH_FLW_BEN_AMT,0))) END as sum_cfb_amt,
				sum(isnull(prior_cash_flw_ben_amt,0)) as sum_prior_cfb_amt
				from dbo.PREM_ADJ_RETRO_DTL d
				inner join dbo.PREM_ADJ_RETRO ih
				on (d.prem_adj_retro_id = ih.prem_adj_retro_id) and (d.prem_adj_perd_id = ih.prem_adj_perd_id) and (d.prem_adj_id = ih.prem_adj_id) and (d.custmr_id = ih.custmr_id)
				where d.prem_adj_perd_id = @premium_adj_period_id
				and d.prem_adj_id = @premium_adjustment_id
				and d.custmr_id = @customer_id
				group by d.prem_adj_id,d.prem_adj_perd_id,ih.coml_agmt_id,d.prem_adj_pgm_id		
			) as tm
			on (tm.prem_adj_perd_id = h.prem_adj_perd_id) and (tm.prem_adj_id = h.prem_adj_id) and (tm.coml_agmt_id = h.coml_agmt_id)
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = h.coml_agmt_id) and (ca.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ca.custmr_id = h.custmr_id) and (ca.actv_ind = 1)
			where h.prem_adj_perd_id = @premium_adj_period_id
			and h.prem_adj_id = @premium_adjustment_id
			and h.custmr_id = @customer_id
			and ca.adj_typ_id in(67,72,66,73) -- Adjustment type IDs for: 'Incurred underlayer','Paid loss underlayer','Incurred Loss WA','Paid loss WA'

			-- Calculate the final invoice amount by adjustment type id for 'Incurred DEP','Paid loss DEP'
			update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
				set invc_amt = isnull(tm.sum_incur_erp_amt,0) + isnull(tm.sum_cfb_amt,0) - isnull(h.subj_depst_prem_amt,0)
				+ isnull(h.non_subj_audt_prem_amt,0) - isnull(h.non_subj_depst_prem_amt,0) - isnull(h.paid_los_bil_amt,0)
				- isnull(h.prev_ernd_retro_prem_amt,0) + isnull(h.misc_amt,0) + isnull(tm.sum_prior_cfb_amt,0) - isnull(h.peo_pay_in_amt,0)
			from dbo.PREM_ADJ_RETRO h 
			inner join 
			(
				select
				prem_adj_id,
				prem_adj_perd_id,
				prem_adj_pgm_id,
				--coml_agmt_id 
				sum(isnull(cesar_cd_tot_amt,0)) as sum_incur_erp_amt,
				--sum(isnull(adj_cash_flw_ben_amt,0)) as sum_cfb_amt,
				case when dbo.fn_CheckAdjCFB(PREM_ADJ_ID,PREM_ADJ_PGM_ID,
				PREM_ADJ_PERD_ID) = 1 then (SUM(isnull(ADJ_CASH_FLW_BEN_AMT,0))) else 
				-(SUM(isnull(CASH_FLW_BEN_AMT,0))) END as sum_cfb_amt,
				sum(isnull(prior_cash_flw_ben_amt,0)) as sum_prior_cfb_amt
				from dbo.PREM_ADJ_RETRO_DTL 
				--inner join dbo.PREM_ADJ_RETRO ih on (d.prem_adj_retro_id = ih.prem_adj_retro_id) and (d.prem_adj_perd_id = ih.prem_adj_perd_id) and (d.prem_adj_id = ih.prem_adj_id) and (d.custmr_id = ih.custmr_id)
				where prem_adj_perd_id = @premium_adj_period_id
				and prem_adj_id = @premium_adjustment_id
				and custmr_id = @customer_id
				group by prem_adj_id,prem_adj_perd_id,prem_adj_pgm_id		
			) as tm
			on (tm.prem_adj_perd_id = h.prem_adj_perd_id) and (tm.prem_adj_id = h.prem_adj_id) and (tm.prem_adj_pgm_id = h.prem_adj_pgm_id)
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = h.coml_agmt_id) and (ca.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ca.custmr_id = h.custmr_id) and (ca.actv_ind = 1)
			where h.prem_adj_perd_id = @premium_adj_period_id
			and h.prem_adj_id = @premium_adjustment_id
			and h.custmr_id = @customer_id
			and ca.adj_typ_id in(63,70) -- Adjustment type IDs for: 'Incurred DEP','Paid loss DEP'

			drop table #peo_policies

		end --end of: PEO adjustment
		else
		begin -- Not PEO adjustment
			-- Calculate the final invoice amount by adjustment type id for 'Incurred loss retro','Paid loss retro'
			update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
				set invc_amt = isnull(tm.sum_incur_erp_amt,0) + isnull(tm.sum_cfb_amt,0) - isnull(h.subj_depst_prem_amt,0)
				+ isnull(h.non_subj_audt_prem_amt,0) - isnull(h.non_subj_depst_prem_amt,0) - isnull(h.paid_los_bil_amt,0)
				- isnull(h.prev_ernd_retro_prem_amt,0) + isnull(h.misc_amt,0) + isnull(tm.sum_prior_cfb_amt,0)
			from dbo.PREM_ADJ_RETRO h 
			inner join 
			(
				select
				d.prem_adj_id,
				d.prem_adj_perd_id,
				ih.coml_agmt_id ,
				sum(isnull(cesar_cd_tot_amt,0)) as sum_incur_erp_amt,
				--sum(isnull(d.adj_cash_flw_ben_amt,0)) as sum_cfb_amt,
				case when dbo.fn_CheckAdjCFB(d.PREM_ADJ_ID,d.PREM_ADJ_PGM_ID,
				d.PREM_ADJ_PERD_ID) = 1 then (SUM(isnull(d.ADJ_CASH_FLW_BEN_AMT,0))) else 
				-(SUM(isnull(d.CASH_FLW_BEN_AMT,0))) END as sum_cfb_amt,
				sum(isnull(prior_cash_flw_ben_amt,0)) as sum_prior_cfb_amt
				from dbo.PREM_ADJ_RETRO_DTL d
				inner join dbo.PREM_ADJ_RETRO ih
				on (d.prem_adj_retro_id = ih.prem_adj_retro_id) and (d.prem_adj_perd_id = ih.prem_adj_perd_id) and (d.prem_adj_id = ih.prem_adj_id) and (d.custmr_id = ih.custmr_id)
				where d.prem_adj_perd_id = @premium_adj_period_id
				and d.prem_adj_id = @premium_adjustment_id
				and d.custmr_id = @customer_id
				group by d.prem_adj_id,d.prem_adj_perd_id,ih.coml_agmt_id,d.prem_adj_pgm_id		
			) as tm
			on (tm.prem_adj_perd_id = h.prem_adj_perd_id) and (tm.prem_adj_id = h.prem_adj_id) and (tm.coml_agmt_id = h.coml_agmt_id)
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = h.coml_agmt_id) and (ca.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ca.custmr_id = h.custmr_id) and (ca.actv_ind = 1)
			where h.prem_adj_perd_id = @premium_adj_period_id
			and h.prem_adj_id = @premium_adjustment_id
			and h.custmr_id = @customer_id
			and ca.adj_typ_id in(65,71) -- Adjustment type IDs for: 'Incurred loss retro','Paid loss retro'

			-- Calculate the final invoice amount by adjustment type id for 'Incurred underlayer','Paid loss underlayer','Incurred Loss WA','Paid loss WA'
			update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
				set invc_amt = isnull(tm.sum_incur_erp_amt,0) + isnull(tm.sum_cfb_amt,0) - isnull(h.subj_depst_prem_amt,0)
				+ isnull(h.non_subj_audt_prem_amt,0) - isnull(h.non_subj_depst_prem_amt,0) - isnull(h.paid_los_bil_amt,0)
				- isnull(h.prev_ernd_retro_prem_amt,0) + isnull(h.misc_amt,0) + isnull(tm.sum_prior_cfb_amt,0) + isnull(tm.sum_adj_ded_wc_loss_amt,0)
			from dbo.PREM_ADJ_RETRO h 
			inner join 
			(
				select
				d.prem_adj_id,
				d.prem_adj_perd_id,
				ih.coml_agmt_id ,
				sum(isnull(cesar_cd_tot_amt,0)) as sum_incur_erp_amt,
				case when @perd_code = 'ERP' then sum(isnull(adj_dedtbl_wrk_comp_los_amt,0)) 
					else 0 end as sum_adj_ded_wc_loss_amt,
				case when dbo.fn_CheckAdjCFB(d.PREM_ADJ_ID,d.PREM_ADJ_PGM_ID,
				d.PREM_ADJ_PERD_ID) = 1 then (SUM(isnull(d.ADJ_CASH_FLW_BEN_AMT,0))) else 
				-(SUM(isnull(d.CASH_FLW_BEN_AMT,0))) END as sum_cfb_amt,
				sum(isnull(prior_cash_flw_ben_amt,0)) as sum_prior_cfb_amt
				from dbo.PREM_ADJ_RETRO_DTL d
				inner join dbo.PREM_ADJ_RETRO ih
				on (d.prem_adj_retro_id = ih.prem_adj_retro_id) and (d.prem_adj_perd_id = ih.prem_adj_perd_id) and (d.prem_adj_id = ih.prem_adj_id) and (d.custmr_id = ih.custmr_id)
				where d.prem_adj_perd_id = @premium_adj_period_id
				and d.prem_adj_id = @premium_adjustment_id
				and d.custmr_id = @customer_id
				group by d.prem_adj_id,d.prem_adj_perd_id,ih.coml_agmt_id,d.prem_adj_pgm_id		
			) as tm
			on (tm.prem_adj_perd_id = h.prem_adj_perd_id) and (tm.prem_adj_id = h.prem_adj_id) and (tm.coml_agmt_id = h.coml_agmt_id)
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = h.coml_agmt_id) and (ca.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ca.custmr_id = h.custmr_id) and (ca.actv_ind = 1)
			where h.prem_adj_perd_id = @premium_adj_period_id
			and h.prem_adj_id = @premium_adjustment_id
			and h.custmr_id = @customer_id
			and ca.adj_typ_id in(67,72,66,73) -- Adjustment type IDs for: 'Incurred underlayer','Paid loss underlayer','Incurred Loss WA','Paid loss WA'

			-- Calculate the final invoice amount by adjustment type id for 'Incurred DEP','Paid loss DEP'
			update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
				set invc_amt = isnull(tm.sum_incur_erp_amt,0) + isnull(tm.sum_cfb_amt,0) - isnull(h.subj_depst_prem_amt,0)
				+ isnull(h.non_subj_audt_prem_amt,0) - isnull(h.non_subj_depst_prem_amt,0) - isnull(h.paid_los_bil_amt,0)
				- isnull(h.prev_ernd_retro_prem_amt,0) + isnull(h.misc_amt,0) + isnull(tm.sum_prior_cfb_amt,0)
			from dbo.PREM_ADJ_RETRO h 
			inner join 
			(
				select
				prem_adj_id,
				prem_adj_perd_id,
				prem_adj_pgm_id,
				--coml_agmt_id 
				sum(isnull(cesar_cd_tot_amt,0)) as sum_incur_erp_amt,
				--sum(isnull(adj_cash_flw_ben_amt,0)) as sum_cfb_amt,
				case when dbo.fn_CheckAdjCFB(PREM_ADJ_ID,PREM_ADJ_PGM_ID,
				PREM_ADJ_PERD_ID) = 1 then (SUM(isnull(ADJ_CASH_FLW_BEN_AMT,0))) else 
				-(SUM(isnull(CASH_FLW_BEN_AMT,0))) END as sum_cfb_amt,
				sum(isnull(prior_cash_flw_ben_amt,0)) as sum_prior_cfb_amt
				from dbo.PREM_ADJ_RETRO_DTL 
				--inner join dbo.PREM_ADJ_RETRO ih on (d.prem_adj_retro_id = ih.prem_adj_retro_id) and (d.prem_adj_perd_id = ih.prem_adj_perd_id) and (d.prem_adj_id = ih.prem_adj_id) and (d.custmr_id = ih.custmr_id)
				where prem_adj_perd_id = @premium_adj_period_id
				and prem_adj_id = @premium_adjustment_id
				and custmr_id = @customer_id
				group by prem_adj_id,prem_adj_perd_id,prem_adj_pgm_id		
			) as tm
			on (tm.prem_adj_perd_id = h.prem_adj_perd_id) and (tm.prem_adj_id = h.prem_adj_id) and (tm.prem_adj_pgm_id = h.prem_adj_pgm_id)
			inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = h.coml_agmt_id) and (ca.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ca.custmr_id = h.custmr_id) and (ca.actv_ind = 1)
			where h.prem_adj_perd_id = @premium_adj_period_id
			and h.prem_adj_id = @premium_adjustment_id
			and h.custmr_id = @customer_id
			and ca.adj_typ_id in(63,70) -- Adjustment type IDs for: 'Incurred DEP','Paid loss DEP'


		end --end of: Not PEO adjustment
	 
----		-- Calculate the final invoice amount by adjustment type id for 'Incurred DEP','Paid loss DEP'
----		update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
----			set invc_amt = isnull(tm.sum_incur_erp_amt,0) - isnull(tm.sum_cfb_amt,0) - isnull(h.subj_depst_prem_amt,0)
----			+ isnull(h.non_subj_audt_prem_amt,0) - isnull(h.non_subj_depst_prem_amt,0) - isnull(h.paid_los_bil_amt,0)
----			- isnull(h.prev_ernd_retro_prem_amt,0) + isnull(h.misc_amt,0) + isnull(tm.sum_prior_cfb_amt,0)
----		from dbo.PREM_ADJ_RETRO h 
----		inner join 
----		(
----			select
----			prem_adj_id,
----			prem_adj_perd_id,
----			prem_adj_pgm_id,
----			--coml_agmt_id 
----			sum(isnull(biled_ernd_retro_prem_amt,0)) as sum_incur_erp_amt,
----			--sum(isnull(adj_cash_flw_ben_amt,0)) as sum_cfb_amt,
----			case when @perd_code = 'ERP' then sum(isnull(cash_flw_ben_amt,0)) else -sum(isnull(adj_cash_flw_ben_amt,0)) end as sum_cfb_amt,
----			sum(isnull(prior_cash_flw_ben_amt,0)) as sum_prior_cfb_amt
----			from dbo.PREM_ADJ_RETRO_DTL 
----			--inner join dbo.PREM_ADJ_RETRO ih on (d.prem_adj_retro_id = ih.prem_adj_retro_id) and (d.prem_adj_perd_id = ih.prem_adj_perd_id) and (d.prem_adj_id = ih.prem_adj_id) and (d.custmr_id = ih.custmr_id)
----			where prem_adj_perd_id = @premium_adj_period_id
----			and prem_adj_id = @premium_adjustment_id
----			and custmr_id = @customer_id
----			group by prem_adj_id,prem_adj_perd_id,prem_adj_pgm_id		
----		) as tm
----		on (tm.prem_adj_perd_id = h.prem_adj_perd_id) and (tm.prem_adj_id = h.prem_adj_id) and (tm.prem_adj_pgm_id = h.prem_adj_pgm_id)
----		inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = h.coml_agmt_id) and (ca.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ca.custmr_id = h.custmr_id)
----		where h.prem_adj_perd_id = @premium_adj_period_id
----		and h.prem_adj_id = @premium_adjustment_id
----		and h.custmr_id = @customer_id
----		and ca.adj_typ_id in(63,70) -- Adjustment type IDs for: 'Incurred DEP','Paid loss DEP'


		-- Roll up for ARIES transmittal
		update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
		set rsdl_mkt_load_tot_amt = isnull(tm.sum_rsdl_mkt_load_tot_amt,0),
			adj_cash_flw_ben_amt = isnull(tm.sum_adj_cash_flw_ben_amt,0)
		from dbo.PREM_ADJ_RETRO h 
		inner join 
		(
			select
			d.prem_adj_id,
			d.prem_adj_perd_id,
			ih.coml_agmt_id ,
			sum(isnull( d.rsdl_mkt_load_tot_amt,0)) as sum_rsdl_mkt_load_tot_amt,
			--sum(isnull( d.adj_cash_flw_ben_amt,0)) as sum_adj_cash_flw_ben_amt
			--case when @perd_code = 'ERP' then sum(isnull(d.cash_flw_ben_amt,0)) else -sum(isnull(d.adj_cash_flw_ben_amt,0)) end as sum_adj_cash_flw_ben_amt
			case when dbo.fn_CheckAdjCFB(d.PREM_ADJ_ID,d.PREM_ADJ_PGM_ID,
			d.PREM_ADJ_PERD_ID) = 1 then -(SUM(isnull(d.ADJ_CASH_FLW_BEN_AMT,0))) else 
			(SUM(isnull(d.CASH_FLW_BEN_AMT,0))) END as sum_adj_cash_flw_ben_amt
			from dbo.PREM_ADJ_RETRO_DTL d
			inner join dbo.PREM_ADJ_RETRO ih
			on (d.prem_adj_retro_id = ih.prem_adj_retro_id) and (d.prem_adj_perd_id = ih.prem_adj_perd_id) and (d.prem_adj_id = ih.prem_adj_id) and (d.custmr_id = ih.custmr_id)
			where d.prem_adj_perd_id = @premium_adj_period_id
			and d.prem_adj_id = @premium_adjustment_id
			and d.custmr_id = @customer_id
			group by d.prem_adj_id,d.prem_adj_perd_id,ih.coml_agmt_id,d.prem_adj_pgm_id		
		) as tm
		on (tm.prem_adj_perd_id = h.prem_adj_perd_id) and (tm.prem_adj_id = h.prem_adj_id) and (tm.coml_agmt_id = h.coml_agmt_id)
		--inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = h.coml_agmt_id) and (ca.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ca.custmr_id = h.custmr_id)
		where h.prem_adj_perd_id = @premium_adj_period_id
		and h.prem_adj_id = @premium_adjustment_id
		and h.custmr_id = @customer_id
		--and ca.adj_typ_id in(67,72,66,73) -- Adjustment type IDs for: 'Incurred underlayer','Paid loss underlayer','Incurred Loss WA','Paid loss WA'

		-- Roll up KY / OR amounts for ARIES transmittal
		update dbo.PREM_ADJ_RETRO WITH (ROWLOCK)
		set ky_tot_due_amt = isnull(tm.sum_ky_tot_due_amt,0),
			or_tot_due_amt = isnull(tm.sum_or_tot_due_amt,0)
		from dbo.PREM_ADJ_RETRO h 
		inner join 
		(
			select
			d.prem_adj_id,
			d.prem_adj_perd_id,
			ih.coml_agmt_id ,
			sum(case when (d.st_id = 20) then d.ky_or_tot_due_amt else 0 end) as sum_ky_tot_due_amt,
			sum(case when (d.st_id = 40) then d.ky_or_tot_due_amt else 0 end) as sum_or_tot_due_amt
			from dbo.PREM_ADJ_RETRO_DTL d
			inner join dbo.PREM_ADJ_RETRO ih
			on (d.prem_adj_retro_id = ih.prem_adj_retro_id) and (d.prem_adj_perd_id = ih.prem_adj_perd_id) and (d.prem_adj_id = ih.prem_adj_id) and (d.custmr_id = ih.custmr_id)
			where d.prem_adj_perd_id = @premium_adj_period_id
			and d.prem_adj_id = @premium_adjustment_id
			and d.custmr_id = @customer_id
			group by d.prem_adj_id,d.prem_adj_perd_id,ih.coml_agmt_id		
		) as tm
		on (tm.prem_adj_perd_id = h.prem_adj_perd_id) and (tm.prem_adj_id = h.prem_adj_id) and (tm.coml_agmt_id = h.coml_agmt_id)
		--inner join dbo.COML_AGMT ca on (ca.coml_agmt_id = h.coml_agmt_id) and (ca.prem_adj_pgm_id = h.prem_adj_pgm_id) and (ca.custmr_id = h.custmr_id)
		where h.prem_adj_perd_id = @premium_adj_period_id
		and h.prem_adj_id = @premium_adjustment_id
		and h.custmr_id = @customer_id
		--and ca.adj_typ_id in(67,72,66,73) -- Adjustment type IDs for: 'Incurred underlayer','Paid loss underlayer','Incurred Loss WA','Paid loss WA'

		--Rounding
		update dbo.PREM_ADJ_RETRO  WITH (ROWLOCK)
		set aries_tot_amt = round(invc_amt,0),
			adj_cash_flw_ben_amt = round(adj_cash_flw_ben_amt,0),
			post_idnmty_amt = round(post_idnmty_amt,0),
			post_resrv_idnmty_amt = round(post_resrv_idnmty_amt,0),
			rsdl_mkt_load_tot_amt = round(rsdl_mkt_load_tot_amt,0),
			ky_tot_due_amt = round(ky_tot_due_amt,0),
			or_tot_due_amt = round(or_tot_due_amt,0)
		where  prem_adj_id = @premium_adjustment_id
		and prem_adj_perd_id = @premium_adj_period_id

		declare 	@sum_aries_tot_amt decimal(15,2),
					@sum_invc_amt decimal(15,2)

		select @sum_aries_tot_amt = isnull(sum(aries_tot_amt),0),
			   @sum_invc_amt = isnull(sum(invc_amt),0)
		from PREM_ADJ_RETRO
		where prem_adj_id = @premium_adjustment_id
		and prem_adj_perd_id = @premium_adj_period_id

		update dbo.PREM_ADJ_RETRO 
			set aries_tot_amt = round(aries_tot_amt - (@sum_aries_tot_amt - @sum_invc_amt),0)
		where prem_adj_id = @premium_adjustment_id
		and prem_adj_perd_id = @premium_adj_period_id
		and coml_agmt_id in
		(
			select top 1 coml_agmt_id 
			from dbo.PREM_ADJ_RETRO
			where invc_amt
			in 
			(
				select max(invc_amt)
				from dbo.PREM_ADJ_RETRO 
				where prem_adj_id = @premium_adjustment_id
				and prem_adj_perd_id = @premium_adj_period_id

			)
			and prem_adj_id = @premium_adjustment_id
			and prem_adj_perd_id = @premium_adj_period_id
		)


		--update dbo.[PREM_ADJ_RETRO_DTL]  WITH (ROWLOCK)
		   --set  basic_amt  = basic_amt,
				--clm_hndl_fee_amt = round(clm_hndl_fee_amt,0) ,
				--los_base_asessment_amt = round(los_base_asessment_amt,0) ,
				--non_conv_fee_amt = round(non_conv_fee_amt,0) ,
				--prem_tax_amt = round(prem_tax_amt,0) ,
				--othr_amt = round(othr_amt,0) ,
				--los_conv_fctr_amt = round(los_conv_fctr_amt,0) ,
				--exc_los_prem_amt = round(exc_los_prem_amt,0) ,
				--incur_ernd_retro_prem_amt = round(incur_ernd_retro_prem_amt,0),
				--adj_incur_ernd_retro_prem_amt = round(adj_incur_ernd_retro_prem_amt,0),
				--paid_ernd_retro_prem_amt = round(paid_ernd_retro_prem_amt,0),
				--adj_paid_ernd_retro_prem_amt = round(adj_paid_ernd_retro_prem_amt,0),
				--cash_flw_ben_amt = round(cash_flw_ben_amt,0),
				--los_dev_resrv_amt = round(los_dev_resrv_amt,0),
				--adj_dedtbl_wrk_comp_los_amt = round(adj_dedtbl_wrk_comp_los_amt,0),
				--prior_cash_flw_ben_amt=round(prior_cash_flw_ben_amt,0),
				--std_subj_prem_amt = round(std_subj_prem_amt,0),
				--prem_asses_amt = round(prem_asses_amt,0),
				--ernd_retro_prem_amt=round(ernd_retro_prem_amt,0),
				--ky_or_tax_asses_amt=round(ky_or_tax_asses_amt,0),
				--ky_or_prev_tax_asses_amt=round(ky_or_prev_tax_asses_amt,0),
				--rsdl_mkt_load_prev_amt=round(rsdl_mkt_load_prev_amt,0),
				--ky_or_tot_due_amt=round(ky_or_tot_due_amt,0),
				--biled_ernd_retro_prem_amt=round(biled_ernd_retro_prem_amt,0),
				--adj_cash_flw_ben_amt=round(adj_cash_flw_ben_amt,0),
				--prev_biled_ernd_retro_prem_amt=round(prev_biled_ernd_retro_prem_amt,0),
				--prev_std_subj_prem_amt=round(prev_std_subj_prem_amt,0),
				--rsdl_mkt_load_ernd_amt = round(rsdl_mkt_load_ernd_amt,0),
				--rsdl_mkt_load_tot_amt = round(rsdl_mkt_load_tot_amt,0),
				--rsdl_mkt_load_paid_amt = round(rsdl_mkt_load_paid_amt,0)
			--where  prem_adj_id = @premium_adjustment_id
			--and prem_adj_perd_id = @premium_adj_period_id

		-- Combined Elements
		if exists(select 1 from dbo.COMB_ELEMTS where prem_adj_pgm_id = @premium_adj_prog_id)
		begin
			if not exists(select 1 from [dbo].[PREM_ADJ_COMB_ELEMTS] where [prem_adj_perd_id] = @premium_adj_period_id and [prem_adj_id] = @premium_adjustment_id)
			begin
				insert into [dbo].[PREM_ADJ_COMB_ELEMTS]
			   (
				[prem_adj_perd_id]
			   ,[prem_adj_id]
			   ,[custmr_id]
			   ,[crte_user_id]
			   )
			   values
			   (
				@premium_adj_period_id,
				@premium_adjustment_id,
				@customer_id,
				@create_user_id
				)
			end

			update dbo.PREM_ADJ_COMB_ELEMTS WITH (ROWLOCK)
			set retro_basic_prem_amt = isnull(tm.sum_basic,0),
				retro_los_fctr_amt = 
					case when pgm.paid_incur_typ_id = 297 --Incurred
					then
						case when pgm.tax_multi_fctr_rt <> 0
						then
							--(isnull(tm.sum_incur_erp_amt,0) - isnull(tm.sum_basic,0))/pgm.tax_multi_fctr_rt 
							(isnull(tm.sum_incur_erp_amt,0) - (isnull(tm.sum_basic,0)*pgm.tax_multi_fctr_rt))/pgm.tax_multi_fctr_rt
						else
							0
						end
					else
						case when pgm.tax_multi_fctr_rt <> 0
						then
							(isnull(tm.sum_incur_erp_amt,0) - isnull(tm.sum_cfb_amt,0) - (isnull(tm.sum_basic,0)*pgm.tax_multi_fctr_rt))/pgm.tax_multi_fctr_rt 
						else
							0
						end
					end,
				retro_tax_multi_rt = pgm.tax_multi_fctr_rt
			from dbo.PREM_ADJ_COMB_ELEMTS ce 
			inner join 
			(
				select
				prem_adj_id,
				prem_adj_perd_id,
				prem_adj_pgm_id,
				sum(isnull(biled_ernd_retro_prem_amt,0)) as sum_incur_erp_amt,
				--sum(isnull(adj_cash_flw_ben_amt,0)) as sum_cfb_amt,
--				case when @perd_code = 'ERP' then sum(isnull(cash_flw_ben_amt,0)) 
--					else -sum(isnull(adj_cash_flw_ben_amt,0)) end as sum_cfb_amt,
				case when dbo.fn_CheckAdjCFB(PREM_ADJ_ID,PREM_ADJ_PGM_ID,
				PREM_ADJ_PERD_ID) = 1 then -(SUM(isnull(ADJ_CASH_FLW_BEN_AMT,0))) else 
				(SUM(isnull(CASH_FLW_BEN_AMT,0))) END as sum_cfb_amt,
				sum(isnull(basic_amt,0)) as sum_basic
				from dbo.PREM_ADJ_RETRO_DTL 
				where prem_adj_perd_id = @premium_adj_period_id
				and prem_adj_id = @premium_adjustment_id
				and custmr_id = @customer_id
				group by prem_adj_id,prem_adj_perd_id,prem_adj_pgm_id		
			) as tm
			on (tm.prem_adj_perd_id = ce.prem_adj_perd_id) and (tm.prem_adj_id = ce.prem_adj_id) 
			inner join dbo.PREM_ADJ_PGM pgm on (pgm.prem_adj_pgm_id = tm.prem_adj_pgm_id) and (pgm.custmr_id = ce.custmr_id)
			where ce.prem_adj_perd_id = @premium_adj_period_id
			and ce.prem_adj_id = @premium_adjustment_id
			and ce.custmr_id = @customer_id

			select
			--@ded_max_amt = 	case when isnull(tot_agmt_amt,0) > isnull(tot_audt_amt,0) then isnull(tot_agmt_amt,0) else isnull(tot_audt_amt,0) end, 
			@ded_max_amt = 	isnull(tot_audt_amt,0), 
			@ded_min_amt = 	isnull(tot_agmt_amt,0)
			from dbo.PREM_ADJ_PGM_RETRO
			where custmr_id = @customer_id
			and prem_adj_pgm_id = @premium_adj_prog_id
			and retro_elemt_typ_id = 337 --Lookup Type: RETROSPECTIVE ELEMENT TYPE; lookup value: Maximum
			and retro_adj_fctr_aplcbl_ind = 0
			and no_lim_ind = 0
			and actv_ind = 1


			update dbo.PREM_ADJ_COMB_ELEMTS WITH (ROWLOCK)
			set retro_subtot_amt = (retro_basic_prem_amt + retro_los_fctr_amt)*retro_tax_multi_rt,
				dedtbl_max_amt = @ded_max_amt,
				dedtbl_min_amt = @ded_min_amt
			where prem_adj_perd_id = @premium_adj_period_id
			and prem_adj_id = @premium_adjustment_id
			and custmr_id = @customer_id

		end -- end of: if exists(select * from dbo.COMB_ELEMTS where prem_adj_pgm_id = @premium_adj_prog_id)

		declare @rel_custmr_id int
		
		select @rel_custmr_id = custmr_id from prem_adj_pgm 
		where prem_adj_pgm_id = @premium_adj_prog_id

		--Update ARMIS LOS with Premium Adjustment ID
		update dbo.ARMIS_LOS_POL  WITH (ROWLOCK)
		set prem_adj_id = tm.prem_adj_id
		from dbo.ARMIS_LOS_POL as los
		join 
		(
			select
			d.custmr_id,
			d.prem_adj_pgm_id,
			d.prem_adj_id,
			d.coml_agmt_id,
			d.st_id
			from dbo.PREM_ADJ_RETRO_DTL d
			where d.prem_adj_perd_id = @premium_adj_period_id
			and d.prem_adj_id = @premium_adjustment_id
			and d.custmr_id = @rel_custmr_id
			and d.prem_adj_pgm_id = @premium_adj_prog_id
		) as tm
		on los.custmr_id = tm.custmr_id
		and los.prem_adj_pgm_id = tm.prem_adj_pgm_id
		and los.coml_agmt_id = tm.coml_agmt_id
		and los.st_id = tm.st_id
		where los.custmr_id = @customer_id
		and los.prem_adj_pgm_id = @premium_adj_prog_id
		and ((los.prem_adj_id is null) or (los.prem_adj_id = @premium_adjustment_id))
		and los.valn_dt	= @prem_adj_valn_dt -- Triage # 67
		and los.actv_ind = 1  

		drop table #num
		drop table #frmla_parser
		drop table #params

		--print '@trancount: ' + convert(varchar(30),@trancount)
		if @trancount = 0
			commit transaction 

end try
begin catch

	if @trancount = 0
	begin
		rollback transaction
	end		
	else
	begin
		rollback transaction ModAISCalcERP
	end


	declare @err_msg varchar(500),@err_ln varchar(10),
			@err_proc varchar(30),@err_no varchar(10)
	select  @err_msg = error_message(),
		    @err_no = error_number(),
		    @err_proc = error_procedure(),
			@err_ln = error_line()
	set @err_msg = '- error no.:' + isnull(@err_no, ' ') + '; procedure:' 
		+ isnull(@err_proc, ' ') + ';error line:' + isnull(@err_ln, ' ') + ';description:' + isnull(@err_msg, ' ' )
	set @err_msg_op = @err_msg

	insert into [dbo].[APLCTN_STS_LOG]
   (
		[src_txt]
	   ,[sev_cd]
	   ,[shrt_desc_txt]
	   ,[full_desc_txt]
	   ,[custmr_id]
	   ,[prem_adj_id]
	   ,[crte_user_id]
	)
	values
    (
		'AIS Calculation Engine'
       ,'ERR'
       ,'Calculation error'
       ,'Error encountered during calculation of adjustment number: ' 
			+ convert(varchar(20),isnull(@premium_adjustment_id,0)) 
			+ ' for program number: ' 
			+ convert(varchar(20),isnull(@premium_adj_prog_id,0)) 
			+ ' associated with customer number: ' 
			+ convert(varchar(20),isnull(@customer_id,0))
			+ '. Error message: '
			+ isnull(@err_msg, ' ')
	   ,@customer_id
	   ,@premium_adjustment_id
       ,isnull(@create_user_id, 0)
	)


	select 
    error_number() AS ErrorNumber,
    error_severity() AS ErrorSeverity,
    error_state() as ErrorState,
    error_procedure() as ErrorProcedure,
    error_line() as ErrorLine,
    error_message() as ErrorMessage

	declare @err_sev varchar(10)

	select  @err_msg = error_message(),
		    @err_no = error_number(),
		    @err_proc = error_procedure(),
			@err_ln = error_line(),
			@err_sev = error_severity()

	RAISERROR (@err_msg, -- Message text.
               @err_sev, -- Severity.
               1 -- State.
               )
end catch


end


go

if object_id('ModAISCalcERP') is not null
	print 'Created Procedure ModAISCalcERP'
else
	print 'Failed Creating Procedure ModAISCalcERP'
go

if object_id('ModAISCalcERP') is not null
	grant exec on ModAISCalcERP to public
go





