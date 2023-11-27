
if exists (select 1 from sysobjects 
                where name = 'GetResidualMSC' and type = 'P')
        drop procedure GetResidualMSC
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sreedhar
-- Create date: <24 Nov 2008>
-- Modified : 17th Feb2009 - Sreedhar
-- Description:	This procedure helps to retrive Residual Market Subsidy Charge output

-- =============================================

CREATE PROCEDURE [dbo].[GetResidualMSC]
@ADJNO INT,
@FLAG INT,
@REVFLAGPREV BIT,
@HISTFLAG BIT
AS
BEGIN

-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
SET NOCOUNT ON;

BEGIN TRY

DECLARE @OldAdjID int
DECLARE @IsRevised bit
DECLARE @IsVoid bit
DECLARE @FnlInvNbr varchar(3)

select @OldAdjID=OldAdj.PREM_ADJ_ID,@IsRevised=OldAdj.ADJ_RRSN_IND,@IsVoid=OldAdj.adj_void_ind, @FnlInvNbr=isnull(SUBSTRING(Adj.fnl_invc_nbr_txt,1,3),'')
from PREM_ADJ OldAdj INNER JOIN PREM_ADJ Adj ON  OldAdj.PREM_ADJ_ID=Adj.REL_PREM_ADJ_ID WHERE Adj.PREM_ADJ_ID=@ADJNO

If (@OldAdjID>0) and (@IsRevised=1) and (@FnlInvNbr<>'RTV')
BEGIN
if(@REVFLAGPREV = 0)
begin
SELECT  CASE @FLAG WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END "INVOICE NUMBER",
						PREM_ADJ.FNL_INVC_DT "FINAL DATE",
							CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), dbo.PREM_ADJ.DRFT_INVC_DT,101) WHEN 2 THEN CONVERT(NVARCHAR(30), dbo.PREM_ADJ.FNL_INVC_DT,101) ELSE  CONVERT(NVARCHAR(30), dbo.PREM_ADJ.DRFT_INVC_DT,101) END "INVOICE DATE",
					  CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) 
                      AS [PROGRAM PERIOD], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], 
                      dbo.INT_ORG.full_name + '/' + dbo.INT_ORG.city_nm AS [BU/OFFICE], dbo.PREM_ADJ_PGM.brkr_id AS [BROKER ID], 
                      dbo.EXTRNL_ORG.full_name AS BROKER, LKUP_AdjTyp.lkup_txt+'('+CASE WHEN PGMTYP.LKUP_TXT LIKE 'NON-DEP%' THEN 'Retro' ELSE PGMTYP.LKUP_TXT END +')' AS [ADJUSTMENT TYPE], 
                      dbo.PREM_ADJ_PERD.adj_nbr_txt AS [ADJUSTMENT NUMBER], dbo.PREM_ADJ.prem_adj_id AS [PREM ADJ ID], 
                      dbo.PREM_ADJ_PERD.prem_adj_pgm_id AS [PREM ADJ PGM ID], dbo.COML_AGMT.coml_agmt_id AS PolicyID, RTRIM(dbo.COML_AGMT.pol_sym_txt) 
                      + ' ' + RTRIM(dbo.COML_AGMT.pol_nbr_txt) + '-' + dbo.COML_AGMT.pol_modulus_txt AS [POLICY NUMBER], CONVERT(NVARCHAR(30), 
                      dbo.COML_AGMT.pol_eff_dt, 101) + ' - ' + CONVERT(NVARCHAR(30), dbo.COML_AGMT.planned_end_date, 101) AS [POLICY PERIOD], 
                      dbo.COML_AGMT.covg_typ_id AS LOFID, LKUP_LOB.lkup_txt AS [Line of Business], dbo.PREM_ADJ_RETRO.prem_adj_retro_id, 
                      LKUP_St.attr_1_txt AS State, dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt AS [Standard Premium], 
                      dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_basic_fctr_rt AS [Basic Factor], 
                      dbo.PREM_ADJ_RETRO_DTL.subj_paid_idnmty_amt + dbo.PREM_ADJ_RETRO_DTL.subj_paid_exps_amt + dbo.PREM_ADJ_RETRO_DTL.subj_resrv_idnmty_amt
                       + dbo.PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt AS [Incurred Losses], dbo.fn_RetrieveLCF(dbo.PREM_ADJ_PERD.custmr_id, 
                      dbo.PREM_ADJ_PGM.prem_adj_pgm_id, dbo.COML_AGMT.coml_agmt_id, dbo.PREM_ADJ_RETRO_DTL.ln_of_bsn_id, dbo.PREM_ADJ_RETRO_DTL.st_id) AS LCF, 
                      CASE dbo.PREM_ADJ_PGM.pgm_typ_id WHEN 451 THEN dbo.fn_RetrieveTM_ByPolState(COML_AGMT.custmr_id,COML_AGMT.prem_adj_pgm_id,COML_AGMT.coml_agmt_id,dbo.PREM_ADJ_RETRO_DTL.st_id)  
					  ELSE dbo.PREM_ADJ_PGM.tax_multi_fctr_rt END AS [Tax Multiplier], 
					  CASE dbo.PREM_ADJ_PGM.pgm_typ_id WHEN 451 THEN  dbo.PREM_ADJ_RETRO_DTL.incur_ernd_retro_prem_amt 
					  ELSE dbo.PREM_ADJ_RETRO_DTL.ernd_retro_prem_amt END AS [Earned Retro Prem], 
                      dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_fctr_rt AS [Residual Factor], dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_ernd_amt AS [Earned Residual], 
                      dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_prev_amt AS [Previous Residual], isnull(dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_ernd_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_prev_amt,0) AS [Net Result],'False' as IsRevised
FROM         dbo.PREM_ADJ INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id INNER JOIN
                      dbo.PREM_ADJ_PGM ON dbo.PREM_ADJ_PERD.prem_adj_pgm_id = dbo.PREM_ADJ_PGM.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_PERD.custmr_id = dbo.PREM_ADJ_PGM.custmr_id INNER JOIN
                      dbo.COML_AGMT ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.COML_AGMT.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_PGM.custmr_id = dbo.COML_AGMT.custmr_id and coml_agmt.actv_ind = 1 INNER JOIN
                      dbo.PREM_ADJ_RETRO ON dbo.COML_AGMT.prem_adj_pgm_id = dbo.PREM_ADJ_RETRO.prem_adj_pgm_id AND 
                      dbo.COML_AGMT.custmr_id = dbo.PREM_ADJ_RETRO.custmr_id AND dbo.COML_AGMT.coml_agmt_id = dbo.PREM_ADJ_RETRO.coml_agmt_id AND 
                      dbo.PREM_ADJ_PERD.prem_adj_id = dbo.PREM_ADJ_RETRO.prem_adj_id INNER JOIN
                      dbo.PREM_ADJ_RETRO_DTL ON dbo.PREM_ADJ_RETRO.custmr_id = dbo.PREM_ADJ_RETRO_DTL.custmr_id AND 
                      dbo.PREM_ADJ_RETRO.prem_adj_retro_id = dbo.PREM_ADJ_RETRO_DTL.prem_adj_retro_id INNER JOIN
                      dbo.CUSTMR ON dbo.PREM_ADJ_PERD.custmr_id = dbo.CUSTMR.custmr_id INNER JOIN
                      dbo.EXTRNL_ORG ON dbo.PREM_ADJ.brkr_id = dbo.EXTRNL_ORG.extrnl_org_id INNER JOIN
                      dbo.INT_ORG ON dbo.PREM_ADJ.bu_office_id = dbo.INT_ORG.int_org_id INNER JOIN
                      dbo.LKUP AS LKUP_LOB ON dbo.COML_AGMT.covg_typ_id = LKUP_LOB.lkup_id INNER JOIN
                      dbo.LKUP AS LKUP_AdjTyp ON dbo.PREM_ADJ_PGM.paid_incur_typ_id = LKUP_AdjTyp.lkup_id INNER JOIN
                      dbo.LKUP AS LKUP_St ON dbo.PREM_ADJ_RETRO_DTL.st_id = LKUP_St.lkup_id INNER JOIN
                      dbo.LKUP AS PGMTYP ON PGMTYP.lkup_id = dbo.PREM_ADJ_PGM.pgm_typ_id INNER JOIN
                      dbo.PREM_ADJ_PGM_SETUP ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PGM_SETUP.prem_adj_pgm_id
WHERE     (dbo.PREM_ADJ.prem_adj_id = @ADJNO) AND (dbo.PREM_ADJ_PGM_SETUP.adj_parmet_typ_id = 403) 
and prem_adj_pgm.pgm_typ_id <> 451 and PREM_ADJ_RETRO_DTL.rsdl_mkt_load_tot_amt IS not NULL
--order by State
UNION ALL

SELECT  PREM_ADJ.FNL_INVC_NBR_TXT AS "INVOICE NUMBER",
						PREM_ADJ.FNL_INVC_DT "FINAL DATE",CONVERT(NVARCHAR(30), FNL_INVC_DT, 101) AS [INVOICE DATE],
					  CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) 
                      AS [PROGRAM PERIOD], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], 
                      dbo.INT_ORG.full_name + '/' + dbo.INT_ORG.city_nm AS [BU/OFFICE], dbo.PREM_ADJ_PGM.brkr_id AS [BROKER ID], 
                      dbo.EXTRNL_ORG.full_name AS BROKER, LKUP_AdjTyp.lkup_txt+'('+CASE WHEN PGMTYP.LKUP_TXT LIKE 'NON-DEP%' THEN 'Retro' ELSE PGMTYP.LKUP_TXT END +')' AS [ADJUSTMENT TYPE], 
                      dbo.PREM_ADJ_PERD.adj_nbr_txt AS [ADJUSTMENT NUMBER], dbo.PREM_ADJ.prem_adj_id AS [PREM ADJ ID], 
                      dbo.PREM_ADJ_PERD.prem_adj_pgm_id AS [PREM ADJ PGM ID], dbo.COML_AGMT.coml_agmt_id AS PolicyID, RTRIM(dbo.COML_AGMT.pol_sym_txt) 
                      + ' ' + RTRIM(dbo.COML_AGMT.pol_nbr_txt) + '-' + dbo.COML_AGMT.pol_modulus_txt AS [POLICY NUMBER], CONVERT(NVARCHAR(30), 
                      dbo.COML_AGMT.pol_eff_dt, 101) + ' - ' + CONVERT(NVARCHAR(30), dbo.COML_AGMT.planned_end_date, 101) AS [POLICY PERIOD], 
                      dbo.COML_AGMT.covg_typ_id AS LOFID, LKUP_LOB.lkup_txt AS [Line of Business], dbo.PREM_ADJ_RETRO.prem_adj_retro_id, 
                      LKUP_St.attr_1_txt AS State, dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt AS [Standard Premium], 
                      dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_basic_fctr_rt AS [Basic Factor], 
                      dbo.PREM_ADJ_RETRO_DTL.subj_paid_idnmty_amt + dbo.PREM_ADJ_RETRO_DTL.subj_paid_exps_amt + dbo.PREM_ADJ_RETRO_DTL.subj_resrv_idnmty_amt
                       + dbo.PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt AS [Incurred Losses], dbo.fn_RetrieveLCF(dbo.PREM_ADJ_PERD.custmr_id, 
                      dbo.PREM_ADJ_PGM.prem_adj_pgm_id, dbo.COML_AGMT.coml_agmt_id, dbo.PREM_ADJ_RETRO_DTL.ln_of_bsn_id, dbo.PREM_ADJ_RETRO_DTL.st_id) AS LCF, 
                      CASE dbo.PREM_ADJ_PGM.pgm_typ_id WHEN 451 THEN dbo.fn_RetrieveTM_ByPolState(COML_AGMT.custmr_id,COML_AGMT.prem_adj_pgm_id,COML_AGMT.coml_agmt_id,dbo.PREM_ADJ_RETRO_DTL.st_id)  
					  ELSE dbo.PREM_ADJ_PGM.tax_multi_fctr_rt END AS [Tax Multiplier], 
					  CASE dbo.PREM_ADJ_PGM.pgm_typ_id WHEN 451 THEN  dbo.PREM_ADJ_RETRO_DTL.incur_ernd_retro_prem_amt 
					  ELSE dbo.PREM_ADJ_RETRO_DTL.ernd_retro_prem_amt END AS [Earned Retro Prem], 
                      dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_fctr_rt AS [Residual Factor], dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_ernd_amt AS [Earned Residual], 
                      dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_prev_amt AS [Previous Residual], (isnull(dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_ernd_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_prev_amt,0))* -1 AS [Net Result],'True' as IsRevised
FROM         dbo.PREM_ADJ INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id INNER JOIN
                      dbo.PREM_ADJ_PGM ON dbo.PREM_ADJ_PERD.prem_adj_pgm_id = dbo.PREM_ADJ_PGM.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_PERD.custmr_id = dbo.PREM_ADJ_PGM.custmr_id INNER JOIN
                      dbo.COML_AGMT ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.COML_AGMT.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_PGM.custmr_id = dbo.COML_AGMT.custmr_id and coml_agmt.actv_ind = 1 INNER JOIN
                      dbo.PREM_ADJ_RETRO ON dbo.COML_AGMT.prem_adj_pgm_id = dbo.PREM_ADJ_RETRO.prem_adj_pgm_id AND 
                      dbo.COML_AGMT.custmr_id = dbo.PREM_ADJ_RETRO.custmr_id AND dbo.COML_AGMT.coml_agmt_id = dbo.PREM_ADJ_RETRO.coml_agmt_id AND 
                      dbo.PREM_ADJ_PERD.prem_adj_id = dbo.PREM_ADJ_RETRO.prem_adj_id INNER JOIN
                      dbo.PREM_ADJ_RETRO_DTL ON dbo.PREM_ADJ_RETRO.custmr_id = dbo.PREM_ADJ_RETRO_DTL.custmr_id AND 
                      dbo.PREM_ADJ_RETRO.prem_adj_retro_id = dbo.PREM_ADJ_RETRO_DTL.prem_adj_retro_id INNER JOIN
                      dbo.CUSTMR ON dbo.PREM_ADJ_PERD.custmr_id = dbo.CUSTMR.custmr_id INNER JOIN
                      dbo.EXTRNL_ORG ON dbo.PREM_ADJ.brkr_id = dbo.EXTRNL_ORG.extrnl_org_id INNER JOIN
                      dbo.INT_ORG ON dbo.PREM_ADJ.bu_office_id = dbo.INT_ORG.int_org_id INNER JOIN
                      dbo.LKUP AS LKUP_LOB ON dbo.COML_AGMT.covg_typ_id = LKUP_LOB.lkup_id INNER JOIN
                      dbo.LKUP AS LKUP_AdjTyp ON dbo.PREM_ADJ_PGM.paid_incur_typ_id = LKUP_AdjTyp.lkup_id INNER JOIN
                      dbo.LKUP AS LKUP_St ON dbo.PREM_ADJ_RETRO_DTL.st_id = LKUP_St.lkup_id INNER JOIN
                      dbo.LKUP AS PGMTYP ON PGMTYP.lkup_id = dbo.PREM_ADJ_PGM.pgm_typ_id INNER JOIN
                      dbo.PREM_ADJ_PGM_SETUP ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PGM_SETUP.prem_adj_pgm_id
WHERE     (dbo.PREM_ADJ.prem_adj_id = @OldAdjID) AND (dbo.PREM_ADJ_PGM_SETUP.adj_parmet_typ_id = 403) 
and prem_adj_pgm.pgm_typ_id <> 451 and PREM_ADJ_RETRO_DTL.rsdl_mkt_load_tot_amt IS not NULL
order by State
end
else
begin
SELECT  CASE @FLAG WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END "INVOICE NUMBER",
						PREM_ADJ.FNL_INVC_DT "FINAL DATE",
							CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), dbo.PREM_ADJ.DRFT_INVC_DT,101) WHEN 2 THEN CONVERT(NVARCHAR(30), dbo.PREM_ADJ.FNL_INVC_DT,101) ELSE  CONVERT(NVARCHAR(30), dbo.PREM_ADJ.DRFT_INVC_DT,101) END "INVOICE DATE",
					  CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) 
                      AS [PROGRAM PERIOD], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], 
                      dbo.INT_ORG.full_name + '/' + dbo.INT_ORG.city_nm AS [BU/OFFICE], dbo.PREM_ADJ_PGM.brkr_id AS [BROKER ID], 
                      dbo.EXTRNL_ORG.full_name AS BROKER, LKUP_AdjTyp.lkup_txt+'('+CASE WHEN PGMTYP.LKUP_TXT LIKE 'NON-DEP%' THEN 'Retro' ELSE PGMTYP.LKUP_TXT END +')' AS [ADJUSTMENT TYPE], 
                      dbo.PREM_ADJ_PERD.adj_nbr_txt AS [ADJUSTMENT NUMBER], dbo.PREM_ADJ.prem_adj_id AS [PREM ADJ ID], 
                      dbo.PREM_ADJ_PERD.prem_adj_pgm_id AS [PREM ADJ PGM ID], dbo.COML_AGMT.coml_agmt_id AS PolicyID, RTRIM(dbo.COML_AGMT.pol_sym_txt) 
                      + ' ' + RTRIM(dbo.COML_AGMT.pol_nbr_txt) + '-' + dbo.COML_AGMT.pol_modulus_txt AS [POLICY NUMBER], CONVERT(NVARCHAR(30), 
                      dbo.COML_AGMT.pol_eff_dt, 101) + ' - ' + CONVERT(NVARCHAR(30), dbo.COML_AGMT.planned_end_date, 101) AS [POLICY PERIOD], 
                      dbo.COML_AGMT.covg_typ_id AS LOFID, LKUP_LOB.lkup_txt AS [Line of Business], dbo.PREM_ADJ_RETRO.prem_adj_retro_id, 
                      LKUP_St.attr_1_txt AS State, dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt AS [Standard Premium], 
                      dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_basic_fctr_rt AS [Basic Factor], 
                      dbo.PREM_ADJ_RETRO_DTL.subj_paid_idnmty_amt + dbo.PREM_ADJ_RETRO_DTL.subj_paid_exps_amt + dbo.PREM_ADJ_RETRO_DTL.subj_resrv_idnmty_amt
                       + dbo.PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt AS [Incurred Losses], dbo.fn_RetrieveLCF(dbo.PREM_ADJ_PERD.custmr_id, 
                      dbo.PREM_ADJ_PGM.prem_adj_pgm_id, dbo.COML_AGMT.coml_agmt_id, dbo.PREM_ADJ_RETRO_DTL.ln_of_bsn_id, dbo.PREM_ADJ_RETRO_DTL.st_id) AS LCF, 
                      CASE dbo.PREM_ADJ_PGM.pgm_typ_id WHEN 451 THEN dbo.fn_RetrieveTM_ByPolState(COML_AGMT.custmr_id,COML_AGMT.prem_adj_pgm_id,COML_AGMT.coml_agmt_id,dbo.PREM_ADJ_RETRO_DTL.st_id)  
					  ELSE dbo.PREM_ADJ_PGM.tax_multi_fctr_rt END AS [Tax Multiplier], 
					  CASE dbo.PREM_ADJ_PGM.pgm_typ_id WHEN 451 THEN  dbo.PREM_ADJ_RETRO_DTL.incur_ernd_retro_prem_amt 
					  ELSE dbo.PREM_ADJ_RETRO_DTL.ernd_retro_prem_amt END AS [Earned Retro Prem], 
                      dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_fctr_rt AS [Residual Factor], dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_ernd_amt AS [Earned Residual], 
                      dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_prev_amt AS [Previous Residual], isnull(dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_ernd_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_prev_amt,0) AS [Net Result],'False' as IsRevised
FROM         dbo.PREM_ADJ INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id INNER JOIN
                      dbo.PREM_ADJ_PGM ON dbo.PREM_ADJ_PERD.prem_adj_pgm_id = dbo.PREM_ADJ_PGM.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_PERD.custmr_id = dbo.PREM_ADJ_PGM.custmr_id INNER JOIN
                      dbo.COML_AGMT ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.COML_AGMT.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_PGM.custmr_id = dbo.COML_AGMT.custmr_id and coml_agmt.actv_ind = 1 INNER JOIN
                      dbo.PREM_ADJ_RETRO ON dbo.COML_AGMT.prem_adj_pgm_id = dbo.PREM_ADJ_RETRO.prem_adj_pgm_id AND 
                      dbo.COML_AGMT.custmr_id = dbo.PREM_ADJ_RETRO.custmr_id AND dbo.COML_AGMT.coml_agmt_id = dbo.PREM_ADJ_RETRO.coml_agmt_id AND 
                      dbo.PREM_ADJ_PERD.prem_adj_id = dbo.PREM_ADJ_RETRO.prem_adj_id INNER JOIN
                      dbo.PREM_ADJ_RETRO_DTL ON dbo.PREM_ADJ_RETRO.custmr_id = dbo.PREM_ADJ_RETRO_DTL.custmr_id AND 
                      dbo.PREM_ADJ_RETRO.prem_adj_retro_id = dbo.PREM_ADJ_RETRO_DTL.prem_adj_retro_id INNER JOIN
                      dbo.CUSTMR ON dbo.PREM_ADJ_PERD.custmr_id = dbo.CUSTMR.custmr_id INNER JOIN
                      dbo.EXTRNL_ORG ON dbo.PREM_ADJ.brkr_id = dbo.EXTRNL_ORG.extrnl_org_id INNER JOIN
                      dbo.INT_ORG ON dbo.PREM_ADJ.bu_office_id = dbo.INT_ORG.int_org_id INNER JOIN
                      dbo.LKUP AS LKUP_LOB ON dbo.COML_AGMT.covg_typ_id = LKUP_LOB.lkup_id INNER JOIN
                      dbo.LKUP AS LKUP_AdjTyp ON dbo.PREM_ADJ_PGM.paid_incur_typ_id = LKUP_AdjTyp.lkup_id INNER JOIN
                      dbo.LKUP AS LKUP_St ON dbo.PREM_ADJ_RETRO_DTL.st_id = LKUP_St.lkup_id INNER JOIN
                      dbo.LKUP AS PGMTYP ON PGMTYP.lkup_id = dbo.PREM_ADJ_PGM.pgm_typ_id INNER JOIN
                      dbo.PREM_ADJ_PGM_SETUP ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PGM_SETUP.prem_adj_pgm_id
WHERE     (dbo.PREM_ADJ.prem_adj_id = @ADJNO) AND (dbo.PREM_ADJ_PGM_SETUP.adj_parmet_typ_id = 403) 
and prem_adj_pgm.pgm_typ_id <> 451 and PREM_ADJ_RETRO_DTL.rsdl_mkt_load_tot_amt IS not NULL
order by State
end

END
ELSE If (@OldAdjID>0) and (@IsVoid=1) and (@FnlInvNbr='RTV') -- Voided
BEGIN

SELECT  CASE @FLAG WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END "INVOICE NUMBER",
						PREM_ADJ.FNL_INVC_DT "FINAL DATE",
							CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), dbo.PREM_ADJ.DRFT_INVC_DT,101) WHEN 2 THEN CONVERT(NVARCHAR(30), dbo.PREM_ADJ.FNL_INVC_DT,101) ELSE  CONVERT(NVARCHAR(30), dbo.PREM_ADJ.DRFT_INVC_DT,101) END "INVOICE DATE",
					  CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) 
                      AS [PROGRAM PERIOD], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], 
                      dbo.INT_ORG.full_name + '/' + dbo.INT_ORG.city_nm AS [BU/OFFICE], dbo.PREM_ADJ_PGM.brkr_id AS [BROKER ID], 
                      dbo.EXTRNL_ORG.full_name AS BROKER, LKUP_AdjTyp.lkup_txt+'('+CASE WHEN PGMTYP.LKUP_TXT LIKE 'NON-DEP%' THEN 'Retro' ELSE PGMTYP.LKUP_TXT END +')' AS [ADJUSTMENT TYPE], 
                      dbo.PREM_ADJ_PERD.adj_nbr_txt AS [ADJUSTMENT NUMBER], dbo.PREM_ADJ.prem_adj_id AS [PREM ADJ ID], 
                      dbo.PREM_ADJ_PERD.prem_adj_pgm_id AS [PREM ADJ PGM ID], dbo.COML_AGMT.coml_agmt_id AS PolicyID, RTRIM(dbo.COML_AGMT.pol_sym_txt) 
                      + ' ' + RTRIM(dbo.COML_AGMT.pol_nbr_txt) + '-' + dbo.COML_AGMT.pol_modulus_txt AS [POLICY NUMBER], CONVERT(NVARCHAR(30), 
                      dbo.COML_AGMT.pol_eff_dt, 101) + ' - ' + CONVERT(NVARCHAR(30), dbo.COML_AGMT.planned_end_date, 101) AS [POLICY PERIOD], 
                      dbo.COML_AGMT.covg_typ_id AS LOFID, LKUP_LOB.lkup_txt AS [Line of Business], dbo.PREM_ADJ_RETRO.prem_adj_retro_id, 
                      LKUP_St.attr_1_txt AS State, dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt AS [Standard Premium], 
                      dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_basic_fctr_rt AS [Basic Factor], 
                      dbo.PREM_ADJ_RETRO_DTL.subj_paid_idnmty_amt + dbo.PREM_ADJ_RETRO_DTL.subj_paid_exps_amt + dbo.PREM_ADJ_RETRO_DTL.subj_resrv_idnmty_amt
                       + dbo.PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt AS [Incurred Losses], dbo.fn_RetrieveLCF(dbo.PREM_ADJ_PERD.custmr_id, 
                      dbo.PREM_ADJ_PGM.prem_adj_pgm_id, dbo.COML_AGMT.coml_agmt_id, dbo.PREM_ADJ_RETRO_DTL.ln_of_bsn_id, dbo.PREM_ADJ_RETRO_DTL.st_id) AS LCF, 
                      CASE dbo.PREM_ADJ_PGM.pgm_typ_id WHEN 451 THEN dbo.fn_RetrieveTM_ByPolState(COML_AGMT.custmr_id,COML_AGMT.prem_adj_pgm_id,COML_AGMT.coml_agmt_id,dbo.PREM_ADJ_RETRO_DTL.st_id)  
					  ELSE dbo.PREM_ADJ_PGM.tax_multi_fctr_rt END AS [Tax Multiplier], 
					  CASE dbo.PREM_ADJ_PGM.pgm_typ_id WHEN 451 THEN  dbo.PREM_ADJ_RETRO_DTL.incur_ernd_retro_prem_amt 
					  ELSE dbo.PREM_ADJ_RETRO_DTL.ernd_retro_prem_amt END AS [Earned Retro Prem], 
                      dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_fctr_rt AS [Residual Factor], dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_ernd_amt AS [Earned Residual], 
                      dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_prev_amt AS [Previous Residual], (isnull(dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_ernd_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_prev_amt,0)) * -1 AS [Net Result],'False' as IsRevised
FROM         dbo.PREM_ADJ INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id INNER JOIN
                      dbo.PREM_ADJ_PGM ON dbo.PREM_ADJ_PERD.prem_adj_pgm_id = dbo.PREM_ADJ_PGM.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_PERD.custmr_id = dbo.PREM_ADJ_PGM.custmr_id INNER JOIN
                      dbo.COML_AGMT ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.COML_AGMT.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_PGM.custmr_id = dbo.COML_AGMT.custmr_id and coml_agmt.actv_ind = 1 INNER JOIN
                      dbo.PREM_ADJ_RETRO ON dbo.COML_AGMT.prem_adj_pgm_id = dbo.PREM_ADJ_RETRO.prem_adj_pgm_id AND 
                      dbo.COML_AGMT.custmr_id = dbo.PREM_ADJ_RETRO.custmr_id AND dbo.COML_AGMT.coml_agmt_id = dbo.PREM_ADJ_RETRO.coml_agmt_id AND 
                      dbo.PREM_ADJ_PERD.prem_adj_id = dbo.PREM_ADJ_RETRO.prem_adj_id INNER JOIN
                      dbo.PREM_ADJ_RETRO_DTL ON dbo.PREM_ADJ_RETRO.custmr_id = dbo.PREM_ADJ_RETRO_DTL.custmr_id AND 
                      dbo.PREM_ADJ_RETRO.prem_adj_retro_id = dbo.PREM_ADJ_RETRO_DTL.prem_adj_retro_id INNER JOIN
                      dbo.CUSTMR ON dbo.PREM_ADJ_PERD.custmr_id = dbo.CUSTMR.custmr_id INNER JOIN
                      dbo.EXTRNL_ORG ON dbo.PREM_ADJ.brkr_id = dbo.EXTRNL_ORG.extrnl_org_id INNER JOIN
                      dbo.INT_ORG ON dbo.PREM_ADJ.bu_office_id = dbo.INT_ORG.int_org_id INNER JOIN
                      dbo.LKUP AS LKUP_LOB ON dbo.COML_AGMT.covg_typ_id = LKUP_LOB.lkup_id INNER JOIN
                      dbo.LKUP AS LKUP_AdjTyp ON dbo.PREM_ADJ_PGM.paid_incur_typ_id = LKUP_AdjTyp.lkup_id INNER JOIN
                      dbo.LKUP AS LKUP_St ON dbo.PREM_ADJ_RETRO_DTL.st_id = LKUP_St.lkup_id INNER JOIN
                      dbo.LKUP AS PGMTYP ON PGMTYP.lkup_id = dbo.PREM_ADJ_PGM.pgm_typ_id INNER JOIN
                      dbo.PREM_ADJ_PGM_SETUP ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PGM_SETUP.prem_adj_pgm_id
WHERE     (dbo.PREM_ADJ.prem_adj_id = @ADJNO) AND (dbo.PREM_ADJ_PGM_SETUP.adj_parmet_typ_id = 403) 
and prem_adj_pgm.pgm_typ_id <> 451 and PREM_ADJ_RETRO_DTL.rsdl_mkt_load_tot_amt IS not NULL
order by State
END

ELSE --If (@OldAdjID=0) 
BEGIN
SELECT  CASE @FLAG WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END "INVOICE NUMBER",
						PREM_ADJ.FNL_INVC_DT "FINAL DATE",
							CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), dbo.PREM_ADJ.DRFT_INVC_DT,101) WHEN 2 THEN CONVERT(NVARCHAR(30), dbo.PREM_ADJ.FNL_INVC_DT,101) ELSE  CONVERT(NVARCHAR(30), dbo.PREM_ADJ.DRFT_INVC_DT,101) END "INVOICE DATE",
					  CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) 
                      AS [PROGRAM PERIOD], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], 
                      dbo.INT_ORG.full_name + '/' + dbo.INT_ORG.city_nm AS [BU/OFFICE], dbo.PREM_ADJ_PGM.brkr_id AS [BROKER ID], 
                      dbo.EXTRNL_ORG.full_name AS BROKER, LKUP_AdjTyp.lkup_txt+'('+CASE WHEN PGMTYP.LKUP_TXT LIKE 'NON-DEP%' THEN 'Retro' ELSE PGMTYP.LKUP_TXT END +')' AS [ADJUSTMENT TYPE], 
                      dbo.PREM_ADJ_PERD.adj_nbr_txt AS [ADJUSTMENT NUMBER], dbo.PREM_ADJ.prem_adj_id AS [PREM ADJ ID], 
                      dbo.PREM_ADJ_PERD.prem_adj_pgm_id AS [PREM ADJ PGM ID], dbo.COML_AGMT.coml_agmt_id AS PolicyID, RTRIM(dbo.COML_AGMT.pol_sym_txt) 
                      + ' ' + RTRIM(dbo.COML_AGMT.pol_nbr_txt) + '-' + dbo.COML_AGMT.pol_modulus_txt AS [POLICY NUMBER], CONVERT(NVARCHAR(30), 
                      dbo.COML_AGMT.pol_eff_dt, 101) + ' - ' + CONVERT(NVARCHAR(30), dbo.COML_AGMT.planned_end_date, 101) AS [POLICY PERIOD], 
                      dbo.COML_AGMT.covg_typ_id AS LOFID, LKUP_LOB.lkup_txt AS [Line of Business], dbo.PREM_ADJ_RETRO.prem_adj_retro_id, 
                      LKUP_St.attr_1_txt AS State, dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt AS [Standard Premium], 
                      dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_basic_fctr_rt AS [Basic Factor], 
                      dbo.PREM_ADJ_RETRO_DTL.subj_paid_idnmty_amt + dbo.PREM_ADJ_RETRO_DTL.subj_paid_exps_amt + dbo.PREM_ADJ_RETRO_DTL.subj_resrv_idnmty_amt
                       + dbo.PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt AS [Incurred Losses], dbo.fn_RetrieveLCF(dbo.PREM_ADJ_PERD.custmr_id, 
                      dbo.PREM_ADJ_PGM.prem_adj_pgm_id, dbo.COML_AGMT.coml_agmt_id, dbo.PREM_ADJ_RETRO_DTL.ln_of_bsn_id, dbo.PREM_ADJ_RETRO_DTL.st_id) AS LCF, 
                      CASE dbo.PREM_ADJ_PGM.pgm_typ_id WHEN 451 THEN dbo.fn_RetrieveTM_ByPolState(COML_AGMT.custmr_id,COML_AGMT.prem_adj_pgm_id,COML_AGMT.coml_agmt_id,dbo.PREM_ADJ_RETRO_DTL.st_id)  
					  ELSE dbo.PREM_ADJ_PGM.tax_multi_fctr_rt END AS [Tax Multiplier], 
					  CASE dbo.PREM_ADJ_PGM.pgm_typ_id WHEN 451 THEN  dbo.PREM_ADJ_RETRO_DTL.incur_ernd_retro_prem_amt 
					  ELSE dbo.PREM_ADJ_RETRO_DTL.ernd_retro_prem_amt END AS [Earned Retro Prem], 
                      dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_fctr_rt AS [Residual Factor], dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_ernd_amt AS [Earned Residual], 
                      dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_prev_amt AS [Previous Residual], isnull(dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_ernd_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.rsdl_mkt_load_prev_amt,0) AS [Net Result],'False' as IsRevised
FROM         dbo.PREM_ADJ INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id INNER JOIN
                      dbo.PREM_ADJ_PGM ON dbo.PREM_ADJ_PERD.prem_adj_pgm_id = dbo.PREM_ADJ_PGM.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_PERD.custmr_id = dbo.PREM_ADJ_PGM.custmr_id INNER JOIN
                      dbo.COML_AGMT ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.COML_AGMT.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_PGM.custmr_id = dbo.COML_AGMT.custmr_id and coml_agmt.actv_ind = 1 INNER JOIN
                      dbo.PREM_ADJ_RETRO ON dbo.COML_AGMT.prem_adj_pgm_id = dbo.PREM_ADJ_RETRO.prem_adj_pgm_id AND 
                      dbo.COML_AGMT.custmr_id = dbo.PREM_ADJ_RETRO.custmr_id AND dbo.COML_AGMT.coml_agmt_id = dbo.PREM_ADJ_RETRO.coml_agmt_id AND 
                      dbo.PREM_ADJ_PERD.prem_adj_id = dbo.PREM_ADJ_RETRO.prem_adj_id INNER JOIN
                      dbo.PREM_ADJ_RETRO_DTL ON dbo.PREM_ADJ_RETRO.custmr_id = dbo.PREM_ADJ_RETRO_DTL.custmr_id AND 
                      dbo.PREM_ADJ_RETRO.prem_adj_retro_id = dbo.PREM_ADJ_RETRO_DTL.prem_adj_retro_id INNER JOIN
                      dbo.CUSTMR ON dbo.PREM_ADJ_PERD.custmr_id = dbo.CUSTMR.custmr_id INNER JOIN
                      dbo.EXTRNL_ORG ON dbo.PREM_ADJ.brkr_id = dbo.EXTRNL_ORG.extrnl_org_id INNER JOIN
                      dbo.INT_ORG ON dbo.PREM_ADJ.bu_office_id = dbo.INT_ORG.int_org_id INNER JOIN
                      dbo.LKUP AS LKUP_LOB ON dbo.COML_AGMT.covg_typ_id = LKUP_LOB.lkup_id INNER JOIN
                      dbo.LKUP AS LKUP_AdjTyp ON dbo.PREM_ADJ_PGM.paid_incur_typ_id = LKUP_AdjTyp.lkup_id INNER JOIN
                      dbo.LKUP AS LKUP_St ON dbo.PREM_ADJ_RETRO_DTL.st_id = LKUP_St.lkup_id INNER JOIN
                      dbo.LKUP AS PGMTYP ON PGMTYP.lkup_id = dbo.PREM_ADJ_PGM.pgm_typ_id INNER JOIN
                      dbo.PREM_ADJ_PGM_SETUP ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PGM_SETUP.prem_adj_pgm_id
WHERE     (dbo.PREM_ADJ.prem_adj_id = @ADJNO) AND (dbo.PREM_ADJ_PGM_SETUP.adj_parmet_typ_id = 403) 
and prem_adj_pgm.pgm_typ_id <> 451 and PREM_ADJ_RETRO_DTL.rsdl_mkt_load_tot_amt IS not NULL
order by State

END

END TRY
BEGIN CATCH

	
	SELECT 
    ERROR_NUMBER() AS ERRORNUMBER,
    ERROR_SEVERITY() AS ERRORSEVERITY,
    ERROR_STATE() AS ERRORSTATE,
    ERROR_PROCEDURE() AS ERRORPROCEDURE,
    ERROR_LINE() AS ERRORLINE,
    ERROR_MESSAGE() AS ERRORMESSAGE;


END CATCH
END
go

if object_id('GetResidualMSC') is not null
        print 'Created Procedure GetResidualMSC'
else
        print 'Failed Creating Procedure GetResidualMSC'
go

if object_id('GetResidualMSC') is not null
        grant exec on GetResidualMSC to  public
go

