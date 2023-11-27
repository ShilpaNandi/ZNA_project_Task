
IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'GetCalcInfoSecond' and TYPE = 'P')
	DROP PROC GetCalcInfoSecond
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	PROC NAME:		GetCalcInfoSecond
-----
-----	VERSION:		SQL SERVER 2005
-----
-----	AUTHOR :		SUNEEL KUMAR MOGALI
-----
-----	DESCRIPTION:	RETURNS DATA WITH RESPECT TO THE GIVEN ADJUSTMENT NUMBER.
-----			
-----	ON EXIT:	
-----			
-----
-----	MODIFIED:	Suneel - 23 Jan 2009 - Commented fn_GetLCFStateId and implemented subquery in where condition.
-----			
----- 
---------------------------------------------------------------------

CREATE PROCEDURE [dbo].[GetCalcInfoSecond]
@ADJNO INT,
@FLAG INT,
@HISTFLAG BIT
AS
BEGIN

-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
SET NOCOUNT ON;


BEGIN TRY

SELECT SUM(PREM_ADJ_RETRO_DTL.subj_paid_idnmty_amt) "Current Paid Loss",--
      SUM(PREM_ADJ_RETRO_DTL.subj_paid_exps_amt) "Current Paid ALAE",
      SUM(PREM_ADJ_RETRO_DTL.subj_resrv_idnmty_amt) "Current Reserve Loss",--
   SUM(PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt) "Current Reserve ALAE",
   COUNT(PREM_ADJ_RETRO_DTL.LOS_BASE_ASESSMENT_AMT) "LBA Count",
   COUNT(PREM_ADJ_RETRO_DTL.LOS_DEV_RESRV_AMT) "LDR Count",
      COUNT(PREM_ADJ_RETRO_DTL.RSDL_MKT_LOAD_TOT_AMT) "RML Count" ,
   COUNT(PREM_ADJ_RETRO_DTL.NON_CONV_FEE_AMT) "Non Conv Count",
   COUNT(PREM_ADJ_RETRO_DTL.OTHR_AMT) "Other Adj Count", 
   -SUM(PREM_ADJ_RETRO_DTL.cash_flw_ben_amt) "Cash Flow Benefit",
   SUM(PREM_ADJ_RETRO_DTL.adj_cash_flw_ben_amt) "Adj Cash Flow Benefit", 
   MSTR_ERND_RETRO_PREM_FRMLA.ernd_retro_prem_frmla_one_txt "Formula",
   PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID "PREM_ADJ_PGM_ID",
   isnull(PREM_ADJ_PGM.tax_multi_fctr_rt,1) "Factor rate",
   PREM_ADJ_RETRO_DTL.PREM_ADJ_ID "PREM_ADJ_ID",
   SUM(PREM_ADJ_RETRO_DTL.prem_tax_amt) "Tax Multiplier Amount",
   PREM_ADJ_CMMNT.CMMNT_TXT "COMMENTS",
   PREM_ADJ_PGM.pgm_typ_id "Pgm Typ Id", -- 451 for WA,
   dbo.fn_GetCFBFormula(PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID,PREM_ADJ_RETRO_DTL.PREM_ADJ_ID) "CFB Formula",    
   PREM_ADJ_PGM.paid_incur_typ_id "Paid Incurr Typ",
   dbo.fn_Get_LDF_IBNR_FactorsList(PREM_ADJ_RETRO_DTL.PREM_ADJ_ID,PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID) "LDF IBNR Factors"
INTO #tmpMain
FROM PREM_ADJ_RETRO_DTL INNER JOIN PREM_ADJ_PGM
ON PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID
AND PREM_ADJ_PGM.ACTV_IND = 1 
LEFT OUTER JOIN MSTR_ERND_RETRO_PREM_FRMLA ON MSTR_ERND_RETRO_PREM_FRMLA.mstr_ernd_retro_prem_frmla_id =
PREM_ADJ_PGM.mstr_ernd_retro_prem_frmla_id
INNER JOIN COML_AGMT ON COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO_DTL.COML_AGMT_ID AND COML_AGMT.ACTV_IND = 1
LEFT OUTER JOIN PREM_ADJ_CMMNT ON PREM_ADJ_RETRO_DTL.prem_adj_id = PREM_ADJ_CMMNT.prem_adj_id
AND PREM_ADJ_CMMNT.PREM_ADJ_PERD_ID = PREM_ADJ_RETRO_DTL.PREM_ADJ_PERD_ID 
AND PREM_ADJ_CMMNT.CMMNT_CATG_ID = 336
WHERE PREM_ADJ_RETRO_DTL.PREM_ADJ_ID = @ADJNO
GROUP BY PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID,MSTR_ERND_RETRO_PREM_FRMLA.ernd_retro_prem_frmla_one_txt,
PREM_ADJ_PGM.tax_multi_fctr_rt,PREM_ADJ_RETRO_DTL.PREM_ADJ_ID,PREM_ADJ_PGM.pgm_typ_id,
PREM_ADJ_PGM.paid_incur_typ_id,PREM_ADJ_CMMNT.CMMNT_TXT


SELECT SUM(ISNULL(PREM_ADJ_RETRO_DTL.subj_paid_idnmty_amt,0)+ISNULL(PREM_ADJ_RETRO_DTL.subj_paid_exps_amt,0)
  +ISNULL(PREM_ADJ_RETRO_DTL.subj_resrv_idnmty_amt,0)+ISNULL(PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt,0)) "Lim Loss and ALAE",
    SUM(PREM_ADJ_RETRO_DTL.los_conv_fctr_amt) "LCF Amount",
  MAX(isnull(PREM_ADJ_RETRO_DTL.los_conv_fctr_rt,1)) "LCF Rate",
  LOB.LKUP_TXT "LOB",
  ST.ATTR_1_TXT "STATE",
  ST.LKUP_ID "ST ID",
  PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID "Prem_Adj_Pgm_Id_ULAE",
     PREM_ADJ_RETRO_DTL.PREM_ADJ_ID "Prem_Adj_Id_ULAE"
INTO #tmpULAE1
FROM PREM_ADJ_RETRO_DTL 
INNER JOIN LKUP ST ON ST.LKUP_ID = PREM_ADJ_RETRO_DTL.ST_ID
INNER JOIN COML_AGMT ON COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO_DTL.COML_AGMT_ID AND COML_AGMT.ACTV_IND = 1
INNER JOIN LKUP LOB ON LOB.LKUP_ID = PREM_ADJ_RETRO_DTL.LN_OF_BSN_ID
WHERE PREM_ADJ_RETRO_DTL.PREM_ADJ_ID = @ADJNO
AND PREM_ADJ_RETRO_DTL.ST_ID IN 
(select PREM_ADJ_PGM_DTL.ST_ID
 from PREM_ADJ_PGM_DTL 
 INNER JOIN PREM_ADJ_PGM_SETUP ON PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID = 
 PREM_ADJ_PGM_DTL.PREM_ADJ_PGM_SETUP_ID 
 WHERE PREM_ADJ_PGM_DTL.PREM_ADJ_PGM_ID =  PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID 
 AND PREM_ADJ_PGM_DTL.LN_OF_BSN_ID = PREM_ADJ_RETRO_DTL.LN_OF_BSN_ID 
 AND PREM_ADJ_PGM_SETUP.adj_parmet_typ_id = 402 and PREM_ADJ_PGM_DTL.actv_ind = 1
 AND PREM_ADJ_PGM_DTL.ST_ID <> 3)
GROUP BY LOB.LKUP_TXT,ST.ATTR_1_TXT,PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID,
PREM_ADJ_RETRO_DTL.PREM_ADJ_ID,ST.LKUP_ID

SELECT PREM_ADJ_RETRO_DTL.PREM_ADJ_RETRO_DTL_ID "Prem_Adj_Retro_dtl_Id"
INTO #tmpULAE9
FROM PREM_ADJ_RETRO_DTL 
INNER JOIN LKUP ST ON ST.LKUP_ID = PREM_ADJ_RETRO_DTL.ST_ID
INNER JOIN COML_AGMT ON COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO_DTL.COML_AGMT_ID AND COML_AGMT.ACTV_IND = 1
INNER JOIN LKUP LOB ON LOB.LKUP_ID = PREM_ADJ_RETRO_DTL.LN_OF_BSN_ID
WHERE PREM_ADJ_RETRO_DTL.PREM_ADJ_ID = @ADJNO
AND PREM_ADJ_RETRO_DTL.ST_ID IN 
(select PREM_ADJ_PGM_DTL.ST_ID
 from PREM_ADJ_PGM_DTL 
 INNER JOIN PREM_ADJ_PGM_SETUP ON PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID = 
 PREM_ADJ_PGM_DTL.PREM_ADJ_PGM_SETUP_ID 
 WHERE PREM_ADJ_PGM_DTL.PREM_ADJ_PGM_ID =  PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID 
 AND PREM_ADJ_PGM_DTL.LN_OF_BSN_ID = PREM_ADJ_RETRO_DTL.LN_OF_BSN_ID 
 AND PREM_ADJ_PGM_SETUP.adj_parmet_typ_id = 402 and PREM_ADJ_PGM_DTL.actv_ind = 1
 AND PREM_ADJ_PGM_DTL.ST_ID <> 3)

SELECT SUM(ISNULL(PREM_ADJ_RETRO_DTL.subj_paid_idnmty_amt,0)+ISNULL(PREM_ADJ_RETRO_DTL.subj_paid_exps_amt,0)
  +ISNULL(PREM_ADJ_RETRO_DTL.subj_resrv_idnmty_amt,0)+ISNULL(PREM_ADJ_RETRO_DTL.subj_resrv_exps_amt,0)) "Lim Loss and ALAE",
    SUM(PREM_ADJ_RETRO_DTL.los_conv_fctr_amt) "LCF Amount",
  Max(isnull(PREM_ADJ_PGM_DTL.adj_fctr_rt,1)) "LCF Rate",
  LOB.LKUP_TXT "LOB",
  'AO' "STATE",
  3 "ST ID",
  PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID "Prem_Adj_Pgm_Id_ULAE",
     PREM_ADJ_RETRO_DTL.PREM_ADJ_ID "Prem_Adj_Id_ULAE"
INTO #tmpULAE2
FROM PREM_ADJ_RETRO_DTL 
INNER JOIN PREM_ADJ_PGM_DTL ON PREM_ADJ_PGM_DTL.PREM_ADJ_PGM_ID =  PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID
AND PREM_ADJ_PGM_DTL.actv_ind = 1 AND PREM_ADJ_PGM_DTL.LN_OF_BSN_ID = PREM_ADJ_RETRO_DTL.LN_OF_BSN_ID 
AND PREM_ADJ_PGM_DTL.ST_ID = 3
INNER JOIN PREM_ADJ_PGM_SETUP ON PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID = PREM_ADJ_PGM_DTL.PREM_ADJ_PGM_SETUP_ID 
AND PREM_ADJ_PGM_SETUP.adj_parmet_typ_id = 402
INNER JOIN LKUP ST ON ST.LKUP_ID = PREM_ADJ_PGM_DTL.ST_ID
INNER JOIN COML_AGMT ON COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO_DTL.COML_AGMT_ID AND COML_AGMT.ACTV_IND = 1
INNER JOIN LKUP LOB ON LOB.LKUP_ID = PREM_ADJ_PGM_DTL.LN_OF_BSN_ID
--INNER JOIN PREM_ADJ_PGM_SETUP_POL ON PREM_ADJ_PGM_SETUP_POL.COML_AGMT_ID = PREM_ADJ_RETRO_DTL.COML_AGMT_ID 
--AND PREM_ADJ_PGM_SETUP_POL.PREM_ADJ_PGM_SETUP_ID = PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID 
WHERE PREM_ADJ_RETRO_DTL.PREM_ADJ_ID = @ADJNO
AND dbo.fn_GetActiveLCF(PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID,3,LOB.LKUP_ID) = 1
and PREM_ADJ_RETRO_DTL.PREM_ADJ_RETRO_DTL_ID not in (select "Prem_Adj_Retro_dtl_Id" from #tmpULAE9)
GROUP BY LOB.LKUP_TXT,ST.ATTR_1_TXT,PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID,PREM_ADJ_RETRO_DTL.PREM_ADJ_ID,
PREM_ADJ_RETRO_DTL.ST_ID,ST.LKUP_ID

SELECT * 
INTO #tmpULAE
from #tmpULAE1
UNION ALL 
SELECT SUM("Lim Loss and ALAE") "Lim Loss and ALAE",SUM("LCF Amount") "LCF Amount", 
MAX("LCF Rate") "LCF Rate","LOB","STATE","ST ID","Prem_Adj_Pgm_Id_ULAE","Prem_Adj_Id_ULAE"
 from #tmpULAE2
GROUP BY "LOB","STATE","ST ID","Prem_Adj_Pgm_Id_ULAE","Prem_Adj_Id_ULAE"
ORDER BY "LOB"

SELECT PREM_ADJ_PGM_RETRO.expo_agmt_amt "Expo Agmt Amt Excess",
  PREM_ADJ_PGM_RETRO.retro_adj_fctr_rt "Retro factr Rt Excess",
        PREM_ADJ_PGM_RETRO.tot_agmt_amt "Tot Agr Excess",
  PREM_ADJ_PGM_RETRO.audt_expo_amt "Audit Expo Amt Excess",
  LKUP.LKUP_TXT "PER Excess",
  PREM_ADJ_PGM_RETRO.aggr_fctr_pct "Factr Agmt Excess",
  EXPOTYP.LKUP_TXT "Expo Type Excess",
        CASE WHEN ISNULL(PREM_ADJ_PGM_RETRO.audt_expo_amt,0) = 0 OR ISNULL(LKUP.LKUP_TXT,0) = 0
  OR ISNULL(PREM_ADJ_PGM_RETRO.aggr_fctr_pct,0) = 0 THEN 0 ELSE 
  ((ISNULL(PREM_ADJ_PGM_RETRO.audt_expo_amt,0)) /
  ISNULL(LKUP.LKUP_TXT,0)) * ISNULL(PREM_ADJ_PGM_RETRO.aggr_fctr_pct,0) END "Result Excess",
  PREM_ADJ_PGM_RETRO.prem_adj_pgm_id "Prem_Adj_Pgm_Id_Excess",
  'Adjustable at Rate per $'+ISNULL(LKUP.LKUP_TXT,'')+' of '+ISNULL(EXPOTYP.LKUP_TXT,'')+' :' "Left lbl Excess",
  '('+(CASE WHEN PREM_ADJ_PGM_RETRO.audt_expo_amt < 0 
THEN '$'+'('+left(Convert(varchar,Convert(money,round(-PREM_ADJ_PGM_RETRO.audt_expo_amt,0)),1),
charindex('.',Convert(varchar,Convert(money,round(-PREM_ADJ_PGM_RETRO.audt_expo_amt,0)),1))-1)+')' 
ELSE '$'+left(Convert(varchar,Convert(money,round(PREM_ADJ_PGM_RETRO.audt_expo_amt,0)),1),
charindex('.',Convert(varchar,Convert(money,round(PREM_ADJ_PGM_RETRO.audt_expo_amt,0)),1))-1)
 END)+'/'+ISNULL(LKUP.LKUP_TXT,'')+')'+'x'+ISNULL(CONVERT(VARCHAR(20),PREM_ADJ_PGM_RETRO.aggr_fctr_pct),'')+'=' "Right lbl Excess",
  RETROELEM.LKUP_TXT "Retro Elem Excess",
  PREM_ADJ_PGM_RETRO.no_lim_ind "no limit ind Excess",
  PREM_ADJ_PGM_RETRO.retro_adj_fctr_aplcbl_ind "Not applicable Excess"
INTO #tmpExcess
FROM PREM_ADJ_PGM_RETRO 
INNER JOIN PREM_ADJ_PERD ON PREM_ADJ_PERD.PREM_ADJ_PGM_ID = PREM_ADJ_PGM_RETRO.PREM_ADJ_PGM_ID
LEFT OUTER JOIN LKUP ON LKUP.LKUP_ID = PREM_ADJ_PGM_RETRO.expo_typ_incremnt_nbr_id
LEFT OUTER JOIN LKUP EXPOTYP ON EXPOTYP.LKUP_ID = PREM_ADJ_PGM_RETRO.expo_typ_id
LEFT OUTER JOIN LKUP RETROELEM ON RETROELEM.LKUP_ID = PREM_ADJ_PGM_RETRO.retro_elemt_typ_id
INNER JOIN PREM_ADJ_PGM ON PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PGM_RETRO.prem_adj_pgm_id 
AND PREM_ADJ_PGM.ACTV_IND = 1 
WHERE PREM_ADJ_PGM_RETRO.ACTV_IND = 1 AND RETROELEM.LKUP_TXT = 'Excess Loss Premium'
AND PREM_ADJ_PERD.PREM_ADJ_ID = @ADJNO

SELECT PREM_ADJ_PGM_RETRO.expo_agmt_amt "Expo Agmt Amt Basic",
  PREM_ADJ_PGM_RETRO.retro_adj_fctr_rt "Retro factr Rt Basic",
        PREM_ADJ_PGM_RETRO.tot_agmt_amt "Tot Agr Basic",
  PREM_ADJ_PGM_RETRO.audt_expo_amt "Audit Expo Amt Basic",
  LKUP.LKUP_TXT "PER Basic",
  PREM_ADJ_PGM_RETRO.aggr_fctr_pct "Factr Agmt Basic",
  EXPOTYP.LKUP_TXT "Expo Type Basic",
        CASE WHEN ISNULL(PREM_ADJ_PGM_RETRO.audt_expo_amt,0) = 0 OR ISNULL(LKUP.LKUP_TXT,0) = 0
  OR ISNULL(PREM_ADJ_PGM_RETRO.aggr_fctr_pct,0) = 0 THEN 0 ELSE 
  ((ISNULL(PREM_ADJ_PGM_RETRO.audt_expo_amt,0)) /
  ISNULL(LKUP.LKUP_TXT,0)) * ISNULL(PREM_ADJ_PGM_RETRO.aggr_fctr_pct,0) END "Result Basic",
  PREM_ADJ_PGM_RETRO.prem_adj_pgm_id "Prem_Adj_Pgm_Id_Basic",
  'Adjustable at Rate per $'+ISNULL(LKUP.LKUP_TXT,'')+' of '+ISNULL(EXPOTYP.LKUP_TXT,'')+' :' "Left lbl Basic",
  '('+(CASE WHEN PREM_ADJ_PGM_RETRO.audt_expo_amt < 0 
THEN '$'+'('+left(Convert(varchar,Convert(money,round(-PREM_ADJ_PGM_RETRO.audt_expo_amt,0)),1),
charindex('.',Convert(varchar,Convert(money,round(-PREM_ADJ_PGM_RETRO.audt_expo_amt,0)),1))-1)+')' 
ELSE '$'+left(Convert(varchar,Convert(money,round(PREM_ADJ_PGM_RETRO.audt_expo_amt,0)),1),
charindex('.',Convert(varchar,Convert(money,round(PREM_ADJ_PGM_RETRO.audt_expo_amt,0)),1))-1)
 END)+'/'+ISNULL(LKUP.LKUP_TXT,'')+')'+'x'+ISNULL(CONVERT(VARCHAR(20),PREM_ADJ_PGM_RETRO.aggr_fctr_pct),'')+'=' "Right lbl Basic",
  RETROELEM.LKUP_TXT "Retro Elem Basic",
  PREM_ADJ_PGM_RETRO.no_lim_ind "no limit ind Basic",
  PREM_ADJ_PGM_RETRO.retro_adj_fctr_aplcbl_ind "Not applicable Basic"
INTO #tmpBasic
FROM PREM_ADJ_PGM_RETRO 
INNER JOIN PREM_ADJ_PERD ON PREM_ADJ_PERD.PREM_ADJ_PGM_ID = PREM_ADJ_PGM_RETRO.PREM_ADJ_PGM_ID
LEFT OUTER JOIN LKUP ON LKUP.LKUP_ID = PREM_ADJ_PGM_RETRO.expo_typ_incremnt_nbr_id
LEFT OUTER JOIN LKUP EXPOTYP ON EXPOTYP.LKUP_ID = PREM_ADJ_PGM_RETRO.expo_typ_id
LEFT OUTER JOIN LKUP RETROELEM ON RETROELEM.LKUP_ID = PREM_ADJ_PGM_RETRO.retro_elemt_typ_id
INNER JOIN PREM_ADJ_PGM ON PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PGM_RETRO.prem_adj_pgm_id 
AND PREM_ADJ_PGM.ACTV_IND = 1 
WHERE PREM_ADJ_PGM_RETRO.ACTV_IND = 1 AND RETROELEM.LKUP_TXT = 'Basic'
AND PREM_ADJ_PERD.PREM_ADJ_ID = @ADJNO

SELECT PREM_ADJ_PGM_RETRO.expo_agmt_amt "Expo Agmt Amt Minimum",
  PREM_ADJ_PGM_RETRO.retro_adj_fctr_rt "Retro factr Rt Minimum",
        PREM_ADJ_PGM_RETRO.tot_agmt_amt "Tot Agr Minimum",
  PREM_ADJ_PGM_RETRO.audt_expo_amt "Audit Expo Amt Minimum",
  LKUP.LKUP_TXT "PER Minimum",
  PREM_ADJ_PGM_RETRO.aggr_fctr_pct "Factr Agmt Minimum",
  EXPOTYP.LKUP_TXT "Expo Type Minimum",
        CASE WHEN ISNULL(PREM_ADJ_PGM_RETRO.audt_expo_amt,0) = 0 OR ISNULL(LKUP.LKUP_TXT,0) = 0
  OR ISNULL(PREM_ADJ_PGM_RETRO.aggr_fctr_pct,0) = 0 THEN 0 ELSE 
  ((ISNULL(PREM_ADJ_PGM_RETRO.audt_expo_amt,0)) /
  ISNULL(LKUP.LKUP_TXT,0)) * ISNULL(PREM_ADJ_PGM_RETRO.aggr_fctr_pct,0) END "Result Minimum",
  PREM_ADJ_PGM_RETRO.prem_adj_pgm_id "Prem_Adj_Pgm_Id_Minimum",
  'Adjustable at Rate per $'+ISNULL(LKUP.LKUP_TXT,'')+' of '+ISNULL(EXPOTYP.LKUP_TXT,'')+' :' "Left lbl Minimum",
  '('+(CASE WHEN PREM_ADJ_PGM_RETRO.audt_expo_amt < 0 
THEN '$'+'('+left(Convert(varchar,Convert(money,round(-PREM_ADJ_PGM_RETRO.audt_expo_amt,0)),1),
charindex('.',Convert(varchar,Convert(money,round(-PREM_ADJ_PGM_RETRO.audt_expo_amt,0)),1))-1)+')' 
ELSE '$'+left(Convert(varchar,Convert(money,round(PREM_ADJ_PGM_RETRO.audt_expo_amt,0)),1),
charindex('.',Convert(varchar,Convert(money,round(PREM_ADJ_PGM_RETRO.audt_expo_amt,0)),1))-1)
 END)+'/'+ISNULL(LKUP.LKUP_TXT,'')+')'+'x'+ISNULL(CONVERT(VARCHAR(20),PREM_ADJ_PGM_RETRO.aggr_fctr_pct),'')+'=' "Right lbl Minimum",
  RETROELEM.LKUP_TXT "Retro Elem Minimum",
  PREM_ADJ_PGM_RETRO.no_lim_ind "no limit ind Minimum",
  PREM_ADJ_PGM_RETRO.retro_adj_fctr_aplcbl_ind "Not applicable Minimum"
INTO #tmpMinimum
FROM PREM_ADJ_PGM_RETRO 
INNER JOIN PREM_ADJ_PERD ON PREM_ADJ_PERD.PREM_ADJ_PGM_ID = PREM_ADJ_PGM_RETRO.PREM_ADJ_PGM_ID
LEFT OUTER JOIN LKUP ON LKUP.LKUP_ID = PREM_ADJ_PGM_RETRO.expo_typ_incremnt_nbr_id
LEFT OUTER JOIN LKUP EXPOTYP ON EXPOTYP.LKUP_ID = PREM_ADJ_PGM_RETRO.expo_typ_id
LEFT OUTER JOIN LKUP RETROELEM ON RETROELEM.LKUP_ID = PREM_ADJ_PGM_RETRO.retro_elemt_typ_id
INNER JOIN PREM_ADJ_PGM ON PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PGM_RETRO.prem_adj_pgm_id 
AND PREM_ADJ_PGM.ACTV_IND = 1 
WHERE PREM_ADJ_PGM_RETRO.ACTV_IND = 1 AND RETROELEM.LKUP_TXT = 'Minimum'
AND PREM_ADJ_PERD.PREM_ADJ_ID = @ADJNO

SELECT PREM_ADJ_PGM_RETRO.expo_agmt_amt "Expo Agmt Amt Maximum",
  PREM_ADJ_PGM_RETRO.retro_adj_fctr_rt "Retro factr Rt Maximum",
        PREM_ADJ_PGM_RETRO.tot_agmt_amt "Tot Agr Maximum",
  PREM_ADJ_PGM_RETRO.audt_expo_amt "Audit Expo Amt Maximum",
  LKUP.LKUP_TXT "PER Maximum",
  PREM_ADJ_PGM_RETRO.aggr_fctr_pct "Factr Agmt Maximum",
  EXPOTYP.LKUP_TXT "Expo Type Maximum",
        CASE WHEN ISNULL(PREM_ADJ_PGM_RETRO.audt_expo_amt,0) = 0 OR ISNULL(LKUP.LKUP_TXT,0) = 0
  OR ISNULL(PREM_ADJ_PGM_RETRO.aggr_fctr_pct,0) = 0 THEN 0 ELSE 
  ((ISNULL(PREM_ADJ_PGM_RETRO.audt_expo_amt,0)) /
  ISNULL(LKUP.LKUP_TXT,0)) * ISNULL(PREM_ADJ_PGM_RETRO.aggr_fctr_pct,0) END "Result Maximum",
  PREM_ADJ_PGM_RETRO.prem_adj_pgm_id "Prem_Adj_Pgm_Id_Maximum",
  'Adjustable at Rate per $'+ISNULL(LKUP.LKUP_TXT,'')+' of '+ISNULL(EXPOTYP.LKUP_TXT,'')+' :' "Left lbl Maximum",
  '('+(CASE WHEN PREM_ADJ_PGM_RETRO.audt_expo_amt < 0 
THEN '$'+'('+left(Convert(varchar,Convert(money,round(-PREM_ADJ_PGM_RETRO.audt_expo_amt,0)),1),
charindex('.',Convert(varchar,Convert(money,round(-PREM_ADJ_PGM_RETRO.audt_expo_amt,0)),1))-1)+')' 
ELSE '$'+left(Convert(varchar,Convert(money,round(PREM_ADJ_PGM_RETRO.audt_expo_amt,0)),1),
charindex('.',Convert(varchar,Convert(money,round(PREM_ADJ_PGM_RETRO.audt_expo_amt,0)),1))-1)
 END)+'/'+ISNULL(LKUP.LKUP_TXT,'')+')'+'x'+ISNULL(CONVERT(VARCHAR(20),PREM_ADJ_PGM_RETRO.aggr_fctr_pct),'')+'=' "Right lbl Maximum",
  RETROELEM.LKUP_TXT "Retro Elem Maximum",
  PREM_ADJ_PGM_RETRO.no_lim_ind "no limit ind Maximum",
  PREM_ADJ_PGM_RETRO.retro_adj_fctr_aplcbl_ind "Not applicable Maximum",
  PREM_ADJ_PGM_RETRO.EXPO_TYP_ID "Expo Typ Id" -- 128 Combined Elements
INTO #tmpMaximum
FROM PREM_ADJ_PGM_RETRO 
INNER JOIN PREM_ADJ_PERD ON PREM_ADJ_PERD.PREM_ADJ_PGM_ID = PREM_ADJ_PGM_RETRO.PREM_ADJ_PGM_ID
LEFT OUTER JOIN LKUP ON LKUP.LKUP_ID = PREM_ADJ_PGM_RETRO.expo_typ_incremnt_nbr_id
LEFT OUTER JOIN LKUP EXPOTYP ON EXPOTYP.LKUP_ID = PREM_ADJ_PGM_RETRO.expo_typ_id
LEFT OUTER JOIN LKUP RETROELEM ON RETROELEM.LKUP_ID = PREM_ADJ_PGM_RETRO.retro_elemt_typ_id
INNER JOIN PREM_ADJ_PGM ON PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PGM_RETRO.prem_adj_pgm_id 
AND PREM_ADJ_PGM.ACTV_IND = 1 
WHERE PREM_ADJ_PGM_RETRO.ACTV_IND = 1 AND RETROELEM.LKUP_TXT = 'Maximum'
AND PREM_ADJ_PERD.PREM_ADJ_ID = @ADJNO
 
SELECT * from #tmpMain 
LEFT OUTER JOIN #tmpULAE ON #tmpMain."PREM_ADJ_ID" = #tmpULAE."Prem_Adj_Id_ULAE" AND 
#tmpMain."PREM_ADJ_PGM_ID" = #tmpULAE."Prem_Adj_Pgm_Id_ULAE" 
AND #tmpULAE."Prem_Adj_Id_ULAE" IS NOT NULL
AND #tmpULAE."Prem_Adj_Pgm_Id_ULAE" IS NOT NULL
LEFT OUTER JOIN #tmpExcess ON #tmpMain."PREM_ADJ_PGM_ID" = #tmpExcess."Prem_Adj_Pgm_Id_Excess"
AND #tmpExcess."Prem_Adj_Pgm_Id_Excess" IS NOT NULL
LEFT OUTER JOIN #tmpBasic ON #tmpMain."PREM_ADJ_PGM_ID" = #tmpBasic."Prem_Adj_Pgm_Id_Basic"
AND #tmpBasic."Prem_Adj_Pgm_Id_Basic" IS NOT NULL
LEFT OUTER JOIN #tmpMinimum ON #tmpMain."PREM_ADJ_PGM_ID" = #tmpMinimum."Prem_Adj_Pgm_Id_Minimum"
AND #tmpMinimum."Prem_Adj_Pgm_Id_Minimum" IS NOT NULL
LEFT OUTER JOIN #tmpMaximum ON #tmpMain."PREM_ADJ_PGM_ID" = #tmpMaximum."Prem_Adj_Pgm_Id_Maximum"
AND #tmpMaximum."Prem_Adj_Pgm_Id_Maximum" IS NOT NULL

DROP table #tmpULAE1
DROP table #tmpULAE2
DROP table #tmpULAE
DROP table #tmpMain
DROP table #tmpExcess
DROP table #tmpBasic
DROP table #tmpMinimum
DROP table #tmpMaximum
drop table #tmpULAE9




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

GO

IF OBJECT_ID('GetCalcInfoSecond') IS NOT NULL
	PRINT 'CREATED PROCEDURE GetCalcInfoSecond'
ELSE
	PRINT 'FAILED CREATING PROCEDURE GetCalcInfoSecond'
GO

IF OBJECT_ID('GetCalcInfoSecond') IS NOT NULL
	GRANT EXEC ON GetCalcInfoSecond TO PUBLIC
GO










