truncate table PREM_ADJ_SURCHRG_DTL
truncate table PREM_ADJ_SURCHRG_DTL_AMT

if exists (select 1 from sysobjects 
		where name = 'fn_GetKYORFactor' and type = 'FN')
	drop function fn_GetKYORFactor
GO
set ansi_nulls off
GO
set quoted_identifier on
GO
---------------------------------------------------------------------
-----
-----	Proc Name:		fn_GetKYORFactor
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Suneel Kumar Mogali
-----
-----	Description:	This function returns KY,OR factor
-----
-----	Modified:	
-----
---------------------------------------------------------------------
CREATE function [dbo].[fn_GetKYORFactor]
  (	
	@STID int,
	@COMLID int	
  )
returns decimal(15,8)
--WITH SCHEMABINDING
as
begin
	 
   declare @fctr_rt decimal(15,8)
	if(@STID = 20) -- KY
	begin
		select 
		@fctr_rt = ky_fctr_rt
		from dbo.KY_OR_SETUP
		where eff_dt = 
			(
				select
				max(eff_dt)
				from dbo.KY_OR_SETUP
				where eff_dt <=
					(
					select
					pol_eff_dt
					from
					dbo.COML_AGMT
					where coml_agmt_id = @COMLID
					)
			)
	end
	else if (@STID = 40) -- OR
	begin
		select 
		@fctr_rt = or_fctr_rt
		from dbo.KY_OR_SETUP
		where eff_dt = 
			(
				select
				max(eff_dt)
				from dbo.KY_OR_SETUP
				where eff_dt <=
					(
					select
					pol_eff_dt
					from
					dbo.COML_AGMT
					where coml_agmt_id = @COMLID
					)
			)
	end

return @fctr_rt
	 
end

go

if object_id('fn_GetKYORFactor') is not null
	print 'Created function fn_GetKYORFactor'
else
	print 'Failed Creating function fn_GetKYORFactor'
go

if object_id('fn_GetKYORFactor') is not null
	grant exec on fn_GetKYORFactor to public
go




--KY-OR converion
INSERT INTO [dbo].[PREM_ADJ_SURCHRG_DTL]
           ([prem_adj_perd_id]
           ,[prem_adj_id]
           ,[custmr_id]
           ,[coml_agmt_id]
           ,[prem_adj_pgm_id]
           ,[st_id]
           ,[ln_of_bsn_id]
           ,[surchrg_cd_id]
           ,[surchrg_typ_id]
		   ,[post_trns_typ_id]
           ,[subj_paid_idnmty_amt]
           ,[subj_paid_exps_amt]
           ,[subj_resrv_idnmty_amt]
           ,[subj_resrv_exps_amt]
           ,[basic_amt]
           ,[std_subj_prem_amt]
           ,[ernd_retro_prem_amt]
           ,[prev_biled_ernd_retro_prem_amt]
           ,[retro_rslt]
           ,[addn_surchrg_asses_cmpnt]
           ,[tot_surchrg_asses_base]
           ,[surchrg_rt]
           ,[addn_rtn]
	   ,[tot_addn_rtn]
           ,[updt_user_id]
           ,[updt_dt]
           ,[crte_user_id]
           ,[crte_dt])
     select  prem_adj_perd_id,
					prem_adj_id,
					custmr_id,
					coml_agmt_id,
					prem_adj_pgm_id,
					st_id,
					ln_of_bsn_id,
					case when st_id = 20 then 566 when st_id = 40 then 567 end,
					case when st_id = 20 then 545 when st_id = 40 then 546 end,
					case when st_id = 20 then [dbo].[fn_GetSurchrgPostTrnsTypID]('816') when st_id = 40 then [dbo].[fn_GetSurchrgPostTrnsTypID]('836') end,
					subj_paid_idnmty_amt,
					subj_paid_exps_amt,
					subj_resrv_idnmty_amt,
					subj_resrv_exps_amt,
					basic_amt,
					case when prev_biled_ernd_retro_prem_amt is null then std_subj_prem_amt else NULL end,
					ernd_retro_prem_amt,
					prev_biled_ernd_retro_prem_amt,
					case when prev_biled_ernd_retro_prem_amt is null then (isnull(ernd_retro_prem_amt,0) - isnull(std_subj_prem_amt,0)) else (isnull(ernd_retro_prem_amt,0) - isnull(prev_biled_ernd_retro_prem_amt,0)) end,
					0,
					case when prev_biled_ernd_retro_prem_amt is null then (isnull(ernd_retro_prem_amt,0) - isnull(std_subj_prem_amt,0)) else (isnull(ernd_retro_prem_amt,0) - isnull(prev_biled_ernd_retro_prem_amt,0)) end,
					CASE when [dbo].[fn_GetKYORFactor](st_id,coml_agmt_id) is not null 
						 then [dbo].[fn_GetKYORFactor](st_id,coml_agmt_id) 
						 else 0.00 
						 end,
						ky_or_tot_due_amt,
						ky_or_tot_due_amt,
							null,
							null,
							999999,
							getdate()	
			from prem_adj_retro_dtl where KY_OR_TOT_DUE_AMT is not null


--------------------------------------------------------------------------
--NY-SIF


INSERT INTO [dbo].[PREM_ADJ_SURCHRG_DTL]
           ([prem_adj_perd_id]
           ,[prem_adj_id]
           ,[custmr_id]
           ,[coml_agmt_id]
           ,[prem_adj_pgm_id]
           ,[st_id]
           ,[ln_of_bsn_id]
           ,[surchrg_cd_id]
           ,[surchrg_typ_id]
		   ,[post_trns_typ_id]
           ,[subj_paid_idnmty_amt]
           ,[subj_paid_exps_amt]
           ,[subj_resrv_idnmty_amt]
           ,[subj_resrv_exps_amt]
           ,[basic_amt]
           ,[std_subj_prem_amt]
           ,[ernd_retro_prem_amt]
		   ,[prev_biled_ernd_retro_prem_amt]
           ,[retro_rslt]
           ,[addn_surchrg_asses_cmpnt]
           ,[tot_surchrg_asses_base]
           ,[surchrg_rt]
           ,[addn_rtn]
		   ,[tot_addn_rtn]
           ,[updt_user_id]
           ,[updt_dt]
           ,[crte_user_id]
           ,[crte_dt])
     select  prem_adj_perd_id,
					prem_adj_id,
					custmr_id,
					coml_agmt_id,
					prem_adj_pgm_id,
					35,
					428,
					581,
					560,
					[dbo].[fn_GetSurchrgPostTrnsTypID]('0932'),
					(select subj_paid_idnmty_amt from prem_adj_retro_dtl where prem_adj_id=pansir.prem_adj_id and prem_adj_perd_id=pansir.prem_adj_perd_id and coml_agmt_id=pansir.coml_agmt_id and ln_of_bsn_id=428 and st_id=35),
					(select subj_paid_exps_amt from prem_adj_retro_dtl where prem_adj_id=pansir.prem_adj_id and prem_adj_perd_id=pansir.prem_adj_perd_id and coml_agmt_id=pansir.coml_agmt_id and ln_of_bsn_id=428 and st_id=35),
					(select subj_resrv_idnmty_amt from prem_adj_retro_dtl where prem_adj_id=pansir.prem_adj_id and prem_adj_perd_id=pansir.prem_adj_perd_id and coml_agmt_id=pansir.coml_agmt_id and ln_of_bsn_id=428 and st_id=35),
					(select subj_resrv_exps_amt from prem_adj_retro_dtl where prem_adj_id=pansir.prem_adj_id and prem_adj_perd_id=pansir.prem_adj_perd_id and coml_agmt_id=pansir.coml_agmt_id and ln_of_bsn_id=428 and st_id=35),
					basic_dedtbl_prem_amt,
					ny_scnd_injr_fund_audt_amt,
					cnvt_tot_los_amt,
					NULL,
					NULL,
					NULL,
					NULL,
					ny_scnd_injr_fund_rt,
					curr_adj_amt,
					curr_adj_amt,
							null,
							null,
							999999,
							getdate()	
			from PREM_ADJ_NY_SCND_INJR_FUND pansir
			





