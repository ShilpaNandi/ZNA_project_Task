IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'ModAIS_LoadARMISData' and TYPE = 'P')
	DROP PROC ModAIS_LoadARMISData
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		ModAIS_LoadARMISData
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Anil Thachapillil & Santhosh Pinto - 09/11/2008
-----
-----	Description:	This procedure will limit the ARMIS loss data
-----					based on the various ALAE rules.		
-----
-----	Modified:		Anil Thachapillil 02/10/2008
-----
---------------------------------------------------------------------


CREATE PROCEDURE [dbo].[ModAIS_LoadARMISData] 
AS
BEGIN

-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
SET NOCOUNT ON;
--BEGIN TRANSACTION ARMIS_INTRFC

declare @trancount int


set @trancount = @@trancount;
print @trancount
if @trancount = 0 
	begin
	    begin transaction 
	end


begin try

/*##########################################################################
pol_expiry_dt in ARMIS files is always one day less. So add another day.
############################################################################*/
UPDATE ARMIS_INTRFC37 WITH (ROWLOCK) SET pol_expiry_dt = DATEADD(d, 1, pol_expiry_dt)
UPDATE ARMIS_INTRFC38 WITH (ROWLOCK) SET pol_expiry_dt = DATEADD(d, 1, pol_expiry_dt)

/*##########################################################################
STEP 0: 
Write all erros to the error table
1.  Those records with a entry in 38 but not in 37 - A.070.320
2.  Those records with no policies setup in AIS - A.070.220
3.  Those records tied to a valid adjustment in process (and Adjustment status not CALC) - A.070.200
The data is written to an error table - ARMIS_INTRFC38_ERR & ARMIS_INTRFC37_ERR
############################################################################*/

/*##########################################################################
1.  Those records with a entry in 38 but not in 37 - A.070.320
############################################################################*/
INSERT INTO ARMIS_INTRFC38_ERR (valn_dt,suprt_serv_custmr_gp_id,cls1_txt,cls2_txt,cls3_txt,cls4_txt,pol_eff_dt,
			pol_expiry_dt,data_src_txt,clm_nbr_txt,site_cd_txt,ln_of_bsn_txt,covg_txt,pol_nbr_txt,pol_eff_2_dt,
			paid_los_bimed_amt,paid_los_lt_amt,paid_exps_amt,resrvd_exps_amt,resrvd_los_bimed_amt,resrv_los_lt_amt,
			clmt_nm,st_txt,clm_sts_txt,covg_trigr_dt,orgin_clm_nbr_txt,reop_clm_nbr_txt,crte_dt,err_txt)
SELECT ARM38.valn_dt,ARM38.suprt_serv_custmr_gp_id,ARM38.cls1_txt,ARM38.cls2_txt,ARM38.cls3_txt,ARM38.cls4_txt,
			ARM38.pol_eff_dt,ARM38.pol_expiry_dt,ARM38.data_src_txt,ARM38.clm_nbr_txt,ARM38.site_cd_txt,
			ARM38.ln_of_bsn_txt,ARM38.covg_txt,ARM38.pol_nbr_txt,ARM38.pol_eff_2_dt,ARM38.paid_los_bimed_amt,
			ARM38.paid_los_lt_amt,ARM38.paid_exps_amt,ARM38.resrvd_exps_amt,ARM38.resrvd_los_bimed_amt,
			ARM38.resrv_los_lt_amt,ARM38.clmt_nm,ARM38.st_txt,ARM38.clm_sts_txt,ARM38.covg_trigr_dt,
			ARM38.orgin_clm_nbr_txt,ARM38.reop_clm_nbr_txt,ARM38.crte_dt,
			'Policy Number not found in File # 37' 
FROM 
	ARMIS_INTRFC38 ARM38 LEFT OUTER JOIN ARMIS_INTRFC37 ARM37
	ON ( 
			ARM38.valn_dt = ARM37.valn_dt
			AND left(ARM38.pol_nbr_txt,8) = left(ARM37.pol_nbr_txt,8) 
			AND ARM38.pol_eff_dt = ARM37.pol_eff_dt 
			AND ARM38.pol_expiry_dt = ARM37.pol_expiry_dt
		)
WHERE 
ARM37.pol_nbr_txt IS NULL

/*##########################################################################
2A.  Those records with no policies setup in AIS for file #37 - A.070.220
############################################################################*/
INSERT INTO ARMIS_INTRFC37_ERR (valn_dt,suprt_serv_custmr_gp_id,cls1_txt,cls2_txt,cls3_txt,cls4_txt,pol_eff_dt,pol_expiry_dt
			,ln_of_bsn_txt,pol_nbr_txt,st_txt,paid_exps_amt,paid_los_bimed_amt,paid_los_lt_amt,resrvd_bimed_amt,resrvd_lt_amt,resrvd_exps_amt
			,wcd_undrln_ind,crte_dt,err_txt)
SELECT AI37.valn_dt,AI37.suprt_serv_custmr_gp_id,AI37.cls1_txt,AI37.cls2_txt,AI37.cls3_txt,AI37.cls4_txt,AI37.pol_eff_dt,
			AI37.pol_expiry_dt,AI37.ln_of_bsn_txt,AI37.pol_nbr_txt,AI37.st_txt,AI37.paid_exps_amt,AI37.paid_los_bimed_amt,
			AI37.paid_los_lt_amt,AI37.resrvd_bimed_amt,AI37.resrvd_lt_amt,AI37.resrvd_exps_amt,AI37.wcd_undrln_ind,AI37.crte_dt,
			'Policy Number not setup in AIS or Policy Not Active - FILE 37' 
FROM ARMIS_INTRFC37 AI37 
	LEFT OUTER  JOIN COML_AGMT CAGT on (CAGT.pol_nbr_txt = AI37.pol_nbr_txt)  
	AND (CAGT.pol_eff_dt = AI37.pol_eff_dt)
	AND (CAGT.planned_end_date = AI37.pol_expiry_dt) 	-- A.070.180
	AND (CAGT.actv_ind = 1) 	-- A.070.180	
	AND (CAGT.los_sys_src_id =81)  -- A.070-210 LOOKUPTYPE/LOOKUP = LOSS SOURCE/ARMIS
WHERE CAGT.pol_nbr_txt IS  NULL	

/*##########################################################################
2B.  Those records with no policies setup in AIS for file #38 - A.070.220
############################################################################*/
INSERT INTO ARMIS_INTRFC38_ERR (valn_dt,suprt_serv_custmr_gp_id,cls1_txt,cls2_txt,cls3_txt,cls4_txt,pol_eff_dt,
			pol_expiry_dt,data_src_txt,clm_nbr_txt,site_cd_txt,ln_of_bsn_txt,covg_txt,pol_nbr_txt,pol_eff_2_dt,
			paid_los_bimed_amt,paid_los_lt_amt,paid_exps_amt,resrvd_exps_amt,resrvd_los_bimed_amt,resrv_los_lt_amt,
			clmt_nm,st_txt,clm_sts_txt,covg_trigr_dt,orgin_clm_nbr_txt,reop_clm_nbr_txt,crte_dt,err_txt)
SELECT AI38.valn_dt,AI38.suprt_serv_custmr_gp_id,AI38.cls1_txt,AI38.cls2_txt,AI38.cls3_txt,AI38.cls4_txt,
			AI38.pol_eff_dt,AI38.pol_expiry_dt,AI38.data_src_txt,AI38.clm_nbr_txt,AI38.site_cd_txt,
			AI38.ln_of_bsn_txt,AI38.covg_txt,AI38.pol_nbr_txt,AI38.pol_eff_2_dt,AI38.paid_los_bimed_amt,
			AI38.paid_los_lt_amt,AI38.paid_exps_amt,AI38.resrvd_exps_amt,AI38.resrvd_los_bimed_amt,
			AI38.resrv_los_lt_amt,AI38.clmt_nm,AI38.st_txt,AI38.clm_sts_txt,AI38.covg_trigr_dt,
			AI38.orgin_clm_nbr_txt,AI38.reop_clm_nbr_txt,AI38.crte_dt,
			'Policy Number not setup in AIS or Policy Not Active - FILE 38' 
FROM ARMIS_INTRFC38 AI38 
	LEFT OUTER  JOIN COML_AGMT CAGT on (CAGT.pol_nbr_txt = AI38.pol_nbr_txt)  
	AND (CAGT.pol_eff_dt = AI38.pol_eff_dt)
	AND (CAGT.planned_end_date = AI38.pol_expiry_dt) 	-- A.070.180
	AND (CAGT.actv_ind = 1) 	-- A.070.180	
	AND (CAGT.los_sys_src_id =81)  -- A.070-210 LOOKUPTYPE/LOOKUP = LOSS SOURCE/ARMIS
WHERE CAGT.pol_nbr_txt IS  NULL	

/*##########################################################################
3A.  Those records tied to a valid adjustment in process (and Adjustment status not CALC) - A.070.200
	should be written to an error table, with relevant message. - FILE 37
############################################################################*/
INSERT INTO ARMIS_INTRFC37_ERR (valn_dt,suprt_serv_custmr_gp_id,cls1_txt,cls2_txt,cls3_txt,cls4_txt,pol_eff_dt,pol_expiry_dt
			,ln_of_bsn_txt,pol_nbr_txt,st_txt,paid_exps_amt,paid_los_bimed_amt,paid_los_lt_amt,resrvd_bimed_amt,resrvd_lt_amt,resrvd_exps_amt
			,wcd_undrln_ind,crte_dt,err_txt)
SELECT AI37.valn_dt,AI37.suprt_serv_custmr_gp_id,AI37.cls1_txt,AI37.cls2_txt,AI37.cls3_txt,AI37.cls4_txt,AI37.pol_eff_dt,
			AI37.pol_expiry_dt,AI37.ln_of_bsn_txt,AI37.pol_nbr_txt,AI37.st_txt,AI37.paid_exps_amt,AI37.paid_los_bimed_amt,
			AI37.paid_los_lt_amt,AI37.resrvd_bimed_amt,AI37.resrvd_lt_amt,AI37.resrvd_exps_amt,AI37.wcd_undrln_ind,AI37.crte_dt,
			'Adjustment has been started and is not in CALC status - FILE 37' 
FROM ARMIS_INTRFC37 AI37 
	JOIN COML_AGMT CAGT ON (CAGT.pol_nbr_txt = AI37.pol_nbr_txt)  -- A.070.180
	AND (CAGT.pol_eff_dt = AI37.pol_eff_dt)-- A.070.180
	AND (CAGT.planned_end_date = AI37.pol_expiry_dt)-- A.070.180
	AND (CAGT.actv_ind = 1) -- A.070.180
	AND (los_sys_src_id =81) -- A.070-210 LOOKUPTYPE/LOOKUP = LOSS SOURCE/ARMIS
	AND (dbo.fn_GetCurrentAdjStatus(AI37.valn_dt,CAGT.custmr_id) <> 346) -- LOOKUPTYPE/LOOKUP = 'ADJUSTMENT STATUSES'/CALC

/*##########################################################################
3B.  Those records tied to a valid adjustment in process (and Adjustment status not CALC) - A.070.200
	should be written to an error table, with relevant message. - FILE 38
############################################################################*/

INSERT INTO ARMIS_INTRFC38_ERR (valn_dt,suprt_serv_custmr_gp_id,cls1_txt,cls2_txt,cls3_txt,cls4_txt,pol_eff_dt,
			pol_expiry_dt,data_src_txt,clm_nbr_txt,site_cd_txt,ln_of_bsn_txt,covg_txt,pol_nbr_txt,pol_eff_2_dt,
			paid_los_bimed_amt,paid_los_lt_amt,paid_exps_amt,resrvd_exps_amt,resrvd_los_bimed_amt,resrv_los_lt_amt,
			clmt_nm,st_txt,clm_sts_txt,covg_trigr_dt,orgin_clm_nbr_txt,reop_clm_nbr_txt,crte_dt,err_txt)
SELECT AI38.valn_dt,AI38.suprt_serv_custmr_gp_id,AI38.cls1_txt,AI38.cls2_txt,AI38.cls3_txt,AI38.cls4_txt,
			AI38.pol_eff_dt,AI38.pol_expiry_dt,AI38.data_src_txt,AI38.clm_nbr_txt,AI38.site_cd_txt,
			AI38.ln_of_bsn_txt,AI38.covg_txt,AI38.pol_nbr_txt,AI38.pol_eff_2_dt,AI38.paid_los_bimed_amt,
			AI38.paid_los_lt_amt,AI38.paid_exps_amt,AI38.resrvd_exps_amt,AI38.resrvd_los_bimed_amt,
			AI38.resrv_los_lt_amt,AI38.clmt_nm,AI38.st_txt,AI38.clm_sts_txt,AI38.covg_trigr_dt,
			AI38.orgin_clm_nbr_txt,AI38.reop_clm_nbr_txt,AI38.crte_dt,
			'Adjustment has been started and is not in CALC status - FILE 38' 
FROM  ARMIS_INTRFC38 AI38 
	JOIN COML_AGMT CAGT ON (CAGT.pol_nbr_txt = AI38.pol_nbr_txt)  -- A.070.180
	AND (CAGT.pol_eff_dt = AI38.pol_eff_dt)-- A.070.180
	AND (CAGT.planned_end_date = AI38.pol_expiry_dt)-- A.070.180
	AND (CAGT.actv_ind = 1) -- A.070.180
	AND (los_sys_src_id =81) -- A.070-210 LOOKUPTYPE/LOOKUP = LOSS SOURCE/ARMIS
	AND (dbo.fn_GetCurrentAdjStatus(AI38.valn_dt,CAGT.custmr_id) <> 346) -- LOOKUPTYPE/LOOKUP = 'ADJUSTMENT STATUSES'/CALC

/*##########################################################################
Start of ARMIS LOSS  processing
##########################################################################
STEP 1: Load a Temporary table (reflection of the ARMIS LOSS table),
with data from (37), for all valid policies(key=Pol #, Dt) 
############################################################################*/
--drop table #ARMIS_TMP_TBL
SELECT 
	 CAGT.aloc_los_adj_exps_typ_id
	,CAGT.aloc_los_adj_exps_capped_amt
	,CAGT.coml_agmt_id
	,CAGT.prem_adj_pgm_id
	,CAGT.custmr_id
	,CAGT.pol_nbr_txt
	,ARM37.pol_eff_dt
	,ARM37.pol_expiry_dt
	,prem_adj_id = NULL
	,ARM37.valn_dt
	,ARM37.suprt_serv_custmr_gp_id
	,paid_idnmty_amt = ISNULL(ARM37.paid_los_bimed_amt,0) + ISNULL(ARM37.paid_los_lt_amt,0) --A.070.250
	,paid_exps_amt = ISNULL(ARM37.paid_exps_amt,0)
	,resrv_idnmty_amt = ISNULL(ARM37.resrvd_bimed_amt,0) + ISNULL(resrvd_lt_amt,0) --A.070.270
	,resrv_exps_amt = ISNULL(ARM37.resrvd_exps_amt,0)
	,non_bilabl_paid_idnmty_amt  = 0
	,non_bilabl_paid_exps_amt = 0
	,non_bilabl_resrv_idnmty_amt = 0
	,non_bilabl_resrv_exps_amt = 0
	,subj_paid_idnmty_amt = 0
	,subj_paid_exps_amt = 0
	,subj_resrv_idnmty_amt = 0
	,subj_resrv_exps_amt = 0
	,exc_paid_idnmty_amt = 0
	,exc_paid_exps_amt = 0
	,exc_resrvd_idnmty_amt = 0
	,exc_resrv_exps_amt = 0
	,st_id = DBO.fn_GetStateID(ARM37.st_txt)
INTO #ARMIS_TMP_TBL
FROM ARMIS_INTRFC37 ARM37 
	JOIN COML_AGMT CAGT ON (CAGT.pol_nbr_txt = ARM37.pol_nbr_txt)  -- A.070.180
	AND (CAGT.pol_eff_dt = ARM37.pol_eff_dt)-- A.070.180
	AND (CAGT.planned_end_date = ARM37.pol_expiry_dt)-- A.070.180
	AND (CAGT.actv_ind = 1) -- A.070.180
	AND (los_sys_src_id =81) -- A.070-210 LOOKUPTYPE/LOOKUP = LOSS SOURCE/ARMIS
	--A.070.200 - DROP LOSES ONLY WHEN ADJ IS IN CALC STATUS OR ADJ HAS NOT BEEN STARTED.
	AND (dbo.fn_GetCurrentAdjStatus(ARM37.valn_dt,CAGT.custmr_id) = 346 -- LOOKUPTYPE/LOOKUP = 'ADJUSTMENT STATUSES'/CALC
	OR dbo.fn_GetCurrentAdjStatus(ARM37.valn_dt,CAGT.custmr_id) IS NULL)

/*#####################################################################
STEP 2: The following update statements apply all the rules listed in the PSD,
to calculate the subject Amounts At the POLICY/STATE Level
#######################################################################*/

UPDATE #ARMIS_TMP_TBL 
SET 
	subj_paid_idnmty_amt = --Subject Paid Indemnity
	CASE 
			--A.070.350
			WHEN List.aloc_los_adj_exps_typ_id IN (74,75,77,78,79,80)
				THEN ISNULL(List.paid_idnmty_amt,0)
			--A.070.360
			WHEN List.aloc_los_adj_exps_typ_id IN (76)
				THEN ISNULL(List.paid_idnmty_amt,0)
			--A.070.350		
			ELSE 0 
		END,

	subj_paid_exps_amt = --Subject Paid Expense
	CASE 
			--A.070.350
			WHEN List.aloc_los_adj_exps_typ_id IN (74,75,77,78,79,80)
				THEN ISNULL(List.paid_exps_amt,0)
			--A.070.360
			ELSE 0
		END,

	subj_resrv_idnmty_amt = --Subject Reserved Indemnity
	CASE 
			--A.070.350
			WHEN List.aloc_los_adj_exps_typ_id IN (74,75,77,78,79,80)
				THEN ISNULL(List.resrv_idnmty_amt,0)
			--A.070.360
			WHEN List.aloc_los_adj_exps_typ_id IN (76)
				THEN ISNULL(List.resrv_idnmty_amt,0)
			--A.070.350			
			ELSE 0 
		END,

	subj_resrv_exps_amt = --Subject Reserved Expense
	CASE 
			--A.070.350
			WHEN List.aloc_los_adj_exps_typ_id IN (74,75,77,78,79,80)
				THEN ISNULL(List.resrv_exps_amt,0)
			--A.070.360
			ELSE 0
		END,

	exc_paid_exps_amt = --Excess Paid Expense
	CASE 
			--A.070.360
			WHEN List.aloc_los_adj_exps_typ_id IN (76)
				THEN ISNULL(List.paid_exps_amt,0)
			--A.070.360		
			ELSE 0 
		END,

	exc_resrv_exps_amt = --Excess Reserved Expense
	CASE 
			--A.070.360
			WHEN List.aloc_los_adj_exps_typ_id IN (76)
				THEN ISNULL(List.resrv_exps_amt,0)
			--A.070.360			
			ELSE 0 
	END
FROM #ARMIS_TMP_TBL as attbl
INNER JOIN (SELECT pol_nbr_txt,pol_eff_dt,pol_expiry_dt , aloc_los_adj_exps_typ_id, paid_idnmty_amt,paid_exps_amt,resrv_idnmty_amt,resrv_exps_amt,st_id FROM(
SELECT pol_nbr_txt,pol_eff_dt,pol_expiry_dt , aloc_los_adj_exps_typ_id, paid_idnmty_amt,paid_exps_amt,resrv_idnmty_amt,resrv_exps_amt,st_id FROM #ARMIS_TMP_TBL) AS T) AS List 
ON (List.pol_nbr_txt = attbl.pol_nbr_txt AND List.pol_eff_dt= attbl.pol_eff_dt
 AND List.pol_expiry_dt= attbl.pol_expiry_dt AND List.st_id= attbl.st_id)

/*#################################################################
STEP 2A: A.070.330 - If the adjustment tied to the losses is in 
CALC status or if the adjustment has not been started YET 
(losses not tied to the adjustment YET), disable the losses 
(Policy/State And Excess Losses) before you take in new values. 

Disable losses only when the (policies/val date) in the new text file(s)
match with the existing (policies/val date)
###################################################################*/

UPDATE ARMIS_LOS_POL WITH (ROWLOCK) SET actv_ind=0 
FROM ARMIS_LOS_POL as attbl
INNER JOIN #ARMIS_TMP_TBL List ON (List.coml_agmt_id= attbl.coml_agmt_id  AND List.valn_dt=attbl.valn_dt)
WHERE 
	(dbo.fn_GetCurrentAdjStatus(attbl.valn_dt,attbl.custmr_id) = 346 -- LOOKUPTYPE/LOOKUP = 'ADJUSTMENT STATUSES'/CALC
	OR dbo.fn_GetCurrentAdjStatus(attbl.valn_dt,attbl.custmr_id) IS NULL) -- Adjustment not yet started

UPDATE ARMIS_LOS_EXC WITH (ROWLOCK) SET actv_ind=0 
FROM ARMIS_LOS_EXC as attbl
INNER JOIN #ARMIS_TMP_TBL List ON List.coml_agmt_id= attbl.coml_agmt_id 
WHERE 
(dbo.fn_GetCurrentAdjStatus(List.valn_dt,List.custmr_id) = 346 -- LOOKUPTYPE/LOOKUP = 'ADJUSTMENT STATUSES'/CALC
	OR dbo.fn_GetCurrentAdjStatus(List.valn_dt,List.custmr_id) IS NULL) -- Adjustment not yet started

/*#################################################################
STEP 3: Store the Results of the calculation in the Retro Loss table
###################################################################*/
Insert into ARMIS_LOS_POL 
(	  
	prem_adj_id
	,coml_agmt_id
	,prem_adj_pgm_id
	,custmr_id
	,valn_dt
	,suprt_serv_custmr_gp_id
	,paid_idnmty_amt
	,paid_exps_amt
	,resrv_idnmty_amt
	,resrv_exps_amt
	,non_bilabl_paid_idnmty_amt
	,non_bilabl_paid_exps_amt
	,subj_paid_idnmty_amt
	,subj_paid_exps_amt
	,subj_resrv_idnmty_amt
	,subj_resrv_exps_amt
	,exc_paid_idnmty_amt
	,exc_paid_exps_amt
	,exc_resrvd_idnmty_amt
	,exc_resrv_exps_amt
	,sys_genrt_ind
	,st_id
	,non_bilabl_resrv_idnmty_amt
	,non_bilabl_resrv_exps_amt
	,actv_ind
	,updt_user_id
	,updt_dt
	,crte_user_id
	,crte_dt
) 
SELECT 
	prem_adj_id,coml_agmt_id,prem_adj_pgm_id,custmr_id,valn_dt,suprt_serv_custmr_gp_id,paid_idnmty_amt,paid_exps_amt
	,resrv_idnmty_amt,resrv_exps_amt,non_bilabl_paid_idnmty_amt,non_bilabl_paid_exps_amt,subj_paid_idnmty_amt
	,subj_paid_exps_amt,subj_resrv_idnmty_amt,subj_resrv_exps_amt,exc_paid_idnmty_amt,exc_paid_exps_amt,
	exc_resrvd_idnmty_amt,exc_resrv_exps_amt,1 AS sys_genrt_ind,st_id,non_bilabl_resrv_idnmty_amt,non_bilabl_resrv_exps_amt
	,1 AS actv_ind,1 AS updt_user_id,GETDATE() AS updt_dt,1 AS crte_user_id,GETDATE() AS crte_dt
FROM #ARMIS_TMP_TBL

/*##########################################################################
Start of ARMIS LOSS Detail processing
############################################################################
STEP 4: Load a Temporary table (reflection of the ARMIS Loss DTL table),
with data from  (38), for all valid policies(key=Pol #, Dt). In this step 
the Subject and Excess amounts are not calculated (done in the next steps)
############################################################################*/
--DROP TABLE #ARMIS_TMP_TBL_DTL
Select 
	armis_los_pol_id = NULL
	,CAGT.coml_agmt_id
	,CAGT.prem_adj_pgm_id
	,CAGT.custmr_id
	,CAGT.aloc_los_adj_exps_typ_id
	,CAGT.aloc_los_adj_exps_capped_amt
	,CAGT.pol_nbr_txt
	,ARM38.pol_eff_dt
	,ARM38.pol_expiry_dt
	,CAGT.dedtbl_pol_lim_amt
	,orgin_clm_nbr_txt
	,clm_nbr_txt
	,NULL AS lim2_amt
	,site_cd_txt
	,covg_trigr_dt
	,clmt_nm
	,reop_clm_nbr_txt
	,paid_idnmty_amt = paid_los_bimed_amt + paid_los_lt_amt
	,paid_exps_amt 
	,resrvd_idnmty_amt = resrvd_los_bimed_amt + resrv_los_lt_amt
	,resrvd_exps_amt 
	,non_bilabl_paid_idnmty_amt = 0
	,non_bilabl_paid_exps_amt = 0
	,non_bilabl_resrvd_idnmty_amt = 0
	,non_bilabl_resrvd_exps_amt = 0
	,subj_paid_idnmty_amt = 0
	,subj_paid_exps_amt = 0
	,subj_resrvd_idnmty_amt = 0
	,subj_resrvd_exps_amt = 0
	,exc_paid_idnmty_amt = 0
	,exc_paid_exps_amt = 0
	,exc_resrvd_idnmty_amt = 0
	,exc_resrvd_exps_amt = 0
	,NULL AS addn_clm_txt
	,st_id = DBO.fn_GetStateID(ARM38.st_txt)
INTO #ARMIS_TMP_TBL_DTL
FROM ARMIS_INTRFC38 ARM38 
	JOIN COML_AGMT CAGT on (CAGT.pol_nbr_txt = ARM38.pol_nbr_txt)  
	AND (CAGT.pol_eff_dt = ARM38.pol_eff_dt)
	AND (CAGT.planned_end_date = ARM38.pol_expiry_dt) 	-- A.070.180
	AND (CAGT.actv_ind = 1) 	-- A.070.180	
	AND (los_sys_src_id =81) -- A.070-210
	AND (dbo.fn_GetCurrentAdjStatus(ARM38.valn_dt,CAGT.custmr_id) = 346 -- LOOKUPTYPE/LOOKUP = 'ADJUSTMENT STATUSES'/CALC
	OR dbo.fn_GetCurrentAdjStatus(ARM38.valn_dt,CAGT.custmr_id) IS NULL)
--SELECT * FROM #ARMIS_TMP_TBL_DTL

/*#####################################################################
STEP 5: The following update statements apply all the ALAE rules listed in 
the PSD,to calculate the subject and Excess Amounts at a Claim Level. The 
Excess Amounts calculated will be rolled up to the Policy State level in 
the Loss Table, the subject amounts will not be...
#####################################################################*/

UPDATE attbl --#ARMIS_TMP_TBL_DTL
SET 
	attbl.subj_paid_idnmty_amt = --Subject Paid Indemnity - A.080.100
	CASE 
			WHEN ((ISNULL(List.paid_idnmty_amt,0) - ISNULL(List.non_bilabl_paid_idnmty_amt,0)) >= ISNULL(List.dedtbl_pol_lim_amt,0))  AND (List.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80)) 
				THEN ISNULL(List.dedtbl_pol_lim_amt,0)  
			WHEN ((ISNULL(List.paid_idnmty_amt,0) - ISNULL(List.non_bilabl_paid_idnmty_amt,0)) < ISNULL(List.dedtbl_pol_lim_amt,0)  AND (List.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80))) 
				THEN (ISNULL(List.paid_idnmty_amt,0) - ISNULL(List.non_bilabl_paid_idnmty_amt,0))
			ELSE 0
	END
FROM #ARMIS_TMP_TBL_DTL as attbl
INNER JOIN #ARMIS_TMP_TBL_DTL List ON (List.pol_nbr_txt = attbl.pol_nbr_txt AND List.pol_eff_dt = attbl.pol_eff_dt  
AND List.pol_expiry_dt= attbl.pol_expiry_dt AND List.st_id= attbl.st_id)


UPDATE attbl -- #ARMIS_TMP_TBL_DTL
SET
	attbl.subj_paid_exps_amt = --Subject Paid Expense
	CASE
		--A.080.120
		WHEN List.aloc_los_adj_exps_typ_id IN (77) -- ALAE Handling Indicator  = Included (77)
		THEN
			CASE 
				WHEN ((ISNULL(List.paid_exps_amt,0) - ISNULL(List.non_bilabl_paid_exps_amt,0)) > (ISNULL(List.dedtbl_pol_lim_amt,0) - ISNULL(attbl.subj_paid_idnmty_amt,0))) THEN (ISNULL(List.dedtbl_pol_lim_amt,0) - ISNULL(attbl.subj_paid_idnmty_amt,0))  
				ELSE (ISNULL(List.paid_exps_amt,0) - ISNULL(List.non_bilabl_paid_exps_amt,0))
			END
		--A.080.130
		WHEN List.aloc_los_adj_exps_typ_id IN (75) -- ALAE Handling Indicator  = Excluded – Company Pays(75)
		THEN (ISNULL(List.paid_exps_amt,0) - ISNULL(List.non_bilabl_paid_exps_amt,0)) 

		--A.080.140
		WHEN List.aloc_los_adj_exps_typ_id IN (74) -- ALAE Handling Indicator  = ALAE Capped Amount(74)
		THEN
			CASE 
				WHEN ((ISNULL(List.paid_exps_amt,0) - ISNULL(List.non_bilabl_paid_exps_amt,0)) <= (ISNULL(List.aloc_los_adj_exps_capped_amt,0))) 
					THEN (ISNULL(List.paid_exps_amt,0) - ISNULL(List.non_bilabl_paid_exps_amt,0))  
				WHEN ((ISNULL(List.paid_exps_amt,0) - ISNULL(List.non_bilabl_paid_exps_amt,0)) > (ISNULL(List.aloc_los_adj_exps_capped_amt,0))) 
					THEN (ISNULL(List.aloc_los_adj_exps_capped_amt,0))  
				ELSE 0
			END

		--A.080.150
		WHEN List.aloc_los_adj_exps_typ_id IN (76) -- ALAE Handling Indicator  = Excluded – Zurich Pays(76)
		THEN (0) 

		--A.080.160
		WHEN List.aloc_los_adj_exps_typ_id IN (78) -- ALAE Handling Indicator  = Pro-Rata(78)
		THEN
			CASE 
				WHEN (((ISNULL(List.paid_idnmty_amt,0) - ISNULL(List.non_bilabl_paid_idnmty_amt,0)) + (ISNULL(List.resrvd_idnmty_amt,0) - ISNULL(List.non_bilabl_resrvd_idnmty_amt,0))) < (ISNULL(List.dedtbl_pol_lim_amt,0))) 
					THEN (ISNULL(List.paid_exps_amt,0) - ISNULL(List.non_bilabl_paid_exps_amt,0))  
				ELSE (ISNULL(List.paid_exps_amt,0) - ISNULL(List.non_bilabl_paid_exps_amt,0)) * (ISNULL(List.dedtbl_pol_lim_amt,0)/((ISNULL(List.paid_idnmty_amt,0) - ISNULL(List.non_bilabl_paid_idnmty_amt,0)) + (ISNULL(List.resrvd_idnmty_amt,0) - ISNULL(List.non_bilabl_resrvd_idnmty_amt,0))))
			END

		--A.080.170
		WHEN List.aloc_los_adj_exps_typ_id IN (80) -- ALAE Handling Indicator  = Pro-Rata Paid(80)
		THEN
			CASE 
				WHEN (((ISNULL(List.paid_idnmty_amt,0) - ISNULL(List.non_bilabl_paid_idnmty_amt,0))) < (ISNULL(List.dedtbl_pol_lim_amt,0))) THEN (ISNULL(List.paid_exps_amt,0) - ISNULL(List.non_bilabl_paid_exps_amt,0))  
				ELSE (ISNULL(List.paid_exps_amt,0) - ISNULL(List.non_bilabl_paid_exps_amt,0)) * (ISNULL(List.dedtbl_pol_lim_amt,0)/(ISNULL(List.paid_idnmty_amt,0) - ISNULL(List.non_bilabl_paid_idnmty_amt,0)))
			END

		--A.080.180
		WHEN List.aloc_los_adj_exps_typ_id IN (79) -- ALAE Handling Indicator  = Pro-Rata No Indemnity(79)
		THEN
			CASE 
				WHEN (ISNULL(attbl.subj_paid_idnmty_amt,0) = 0) THEN (ISNULL(List.paid_exps_amt,0) - ISNULL(List.non_bilabl_paid_exps_amt,0))
				WHEN (ISNULL(attbl.subj_paid_idnmty_amt,0) > 0) 
				THEN 
					CASE
						WHEN ((ISNULL(List.paid_idnmty_amt,0) - ISNULL(List.non_bilabl_paid_idnmty_amt,0)) + (ISNULL(List.resrvd_idnmty_amt,0) - ISNULL(List.non_bilabl_resrvd_idnmty_amt,0)) <  ISNULL(List.dedtbl_pol_lim_amt,0)) THEN (ISNULL(List.paid_exps_amt,0) - ISNULL(List.non_bilabl_paid_exps_amt,0))  
						--WHEN ((List.paid_idnmty_amt - List.non_bilabl_paid_idnmty_amt) + (List.resrvd_idnmty_amt - List.non_bilabl_resrvd_idnmty_amt) >= List.dedtbl_pol_lim_amt) THEN ((List.paid_exps_amt - List.non_bilabl_paid_exps_amt) + (List.resrvd_idnmty_amt - List.non_bilabl_resrvd_idnmty_amt)) * (List.dedtbl_pol_lim_amt/(List.paid_idnmty_amt - List.non_bilabl_paid_idnmty_amt))
						WHEN ((ISNULL(List.paid_idnmty_amt,0) - ISNULL(List.non_bilabl_paid_idnmty_amt,0)) + (ISNULL(List.resrvd_idnmty_amt,0) - ISNULL(List.non_bilabl_resrvd_idnmty_amt,0)) >= ISNULL(List.dedtbl_pol_lim_amt,0)) THEN ((ISNULL(List.paid_exps_amt,0) - ISNULL(List.non_bilabl_paid_exps_amt,0)) * (ISNULL(List.dedtbl_pol_lim_amt,0)/((ISNULL(List.paid_idnmty_amt,0) - ISNULL(List.non_bilabl_paid_idnmty_amt,0)) + (ISNULL(List.resrvd_idnmty_amt,0) - ISNULL(List.non_bilabl_resrvd_idnmty_amt,0)))))
						ELSE (0)
					END
			END
		ELSE 0 
	END
FROM #ARMIS_TMP_TBL_DTL as attbl
INNER JOIN #ARMIS_TMP_TBL_DTL List ON (List.pol_nbr_txt = attbl.pol_nbr_txt AND List.pol_eff_dt = attbl.pol_eff_dt  
AND List.pol_expiry_dt= attbl.pol_expiry_dt AND List.st_id= attbl.st_id)

UPDATE attbl -- #ARMIS_TMP_TBL_DTL
SET
	attbl.subj_resrvd_idnmty_amt = --Subject Reserve Indemnity
	CASE
		--A.080.200
		WHEN List.aloc_los_adj_exps_typ_id IN (77) -- ALAE Handling Indicator  = Included 
		THEN
			CASE 
				WHEN ((ISNULL(List.resrvd_idnmty_amt,0) - ISNULL(List.non_bilabl_resrvd_idnmty_amt,0)) >= (ISNULL(List.dedtbl_pol_lim_amt,0) - (ISNULL(attbl.subj_paid_idnmty_amt,0) + ISNULL(attbl.subj_paid_exps_amt,0)))) THEN (ISNULL(List.dedtbl_pol_lim_amt,0) - (ISNULL(attbl.subj_paid_idnmty_amt,0) + ISNULL(attbl.subj_paid_exps_amt,0)))  
				ELSE (ISNULL(List.resrvd_idnmty_amt,0) - ISNULL(List.non_bilabl_resrvd_idnmty_amt,0))
			END

		--A.080.210
		WHEN List.aloc_los_adj_exps_typ_id IN (74,75,76,78,79,80) -- ALAE Handling Indicator  = REMAINING 6
		THEN
			CASE 
				WHEN ((ISNULL(List.resrvd_idnmty_amt,0) - ISNULL(List.non_bilabl_resrvd_idnmty_amt,0)) >= (ISNULL(List.dedtbl_pol_lim_amt,0) - ISNULL(attbl.subj_paid_idnmty_amt,0))) THEN (ISNULL(List.dedtbl_pol_lim_amt,0) - ISNULL(attbl.subj_paid_idnmty_amt,0))  
				ELSE (ISNULL(List.resrvd_idnmty_amt,0) - ISNULL(List.non_bilabl_resrvd_idnmty_amt,0))
			END
		ELSE 0
	END
FROM #ARMIS_TMP_TBL_DTL as attbl
INNER JOIN #ARMIS_TMP_TBL_DTL List ON (List.pol_nbr_txt = attbl.pol_nbr_txt AND List.pol_eff_dt = attbl.pol_eff_dt  
AND List.pol_expiry_dt= attbl.pol_expiry_dt AND List.st_id= attbl.st_id)

UPDATE attbl -- #ARMIS_TMP_TBL_DTL
SET
	attbl.subj_resrvd_exps_amt = --Subject Reserve Expense
	CASE
		--A.080.230
		WHEN List.aloc_los_adj_exps_typ_id IN (77) -- ALAE Handling Indicator  = Included 
		THEN
			CASE 
				WHEN ((ISNULL(List.resrvd_exps_amt,0) - ISNULL(List.non_bilabl_resrvd_exps_amt,0)) >= (ISNULL(List.dedtbl_pol_lim_amt,0) - (ISNULL(attbl.subj_paid_idnmty_amt,0) + ISNULL(attbl.subj_paid_exps_amt,0) + ISNULL(attbl.subj_resrvd_idnmty_amt,0)))) THEN (ISNULL(List.dedtbl_pol_lim_amt,0) - (ISNULL(attbl.subj_paid_idnmty_amt,0) + ISNULL(attbl.subj_paid_exps_amt,0) + ISNULL(attbl.subj_resrvd_idnmty_amt,0)))  
				ELSE (ISNULL(List.resrvd_exps_amt,0) - ISNULL(List.non_bilabl_resrvd_exps_amt,0))
			END

		--A.080.240
		WHEN List.aloc_los_adj_exps_typ_id IN (75) -- ALAE Handling Indicator  = Excluded – Company Pays
		THEN (ISNULL(List.resrvd_exps_amt,0) - ISNULL(List.non_bilabl_resrvd_exps_amt,0)) 

		--A.080.250
		WHEN List.aloc_los_adj_exps_typ_id IN (76) -- ALAE Handling Indicator  = Excluded – Zurich Pays
		THEN (0) 

		--A.080.260
		WHEN List.aloc_los_adj_exps_typ_id IN (78) -- ALAE Handling Indicator  = Pro-Rata
		THEN
			CASE 
				WHEN (((ISNULL(List.paid_idnmty_amt,0) - ISNULL(List.non_bilabl_paid_idnmty_amt,0)) + (ISNULL(List.resrvd_idnmty_amt,0) - ISNULL(List.non_bilabl_resrvd_idnmty_amt,0))) < (ISNULL(List.dedtbl_pol_lim_amt,0))) THEN (ISNULL(List.resrvd_exps_amt,0) - ISNULL(List.non_bilabl_resrvd_exps_amt,0))  
				ELSE (ISNULL(List.resrvd_exps_amt,0) - ISNULL(List.non_bilabl_resrvd_exps_amt,0)) * (ISNULL(List.dedtbl_pol_lim_amt,0)/((ISNULL(List.paid_idnmty_amt,0) - ISNULL(List.non_bilabl_paid_idnmty_amt,0)) + (ISNULL(List.resrvd_idnmty_amt,0) - ISNULL(List.non_bilabl_resrvd_idnmty_amt,0))))
			END

		--A.080.270
		WHEN List.aloc_los_adj_exps_typ_id IN (80) -- ALAE Handling Indicator  = Pro-Rata Paid
		THEN
			CASE 
				WHEN (((ISNULL(List.paid_idnmty_amt,0) - ISNULL(List.non_bilabl_paid_idnmty_amt,0))) < (ISNULL(List.dedtbl_pol_lim_amt,0))) THEN (ISNULL(List.resrvd_exps_amt,0) - ISNULL(List.non_bilabl_resrvd_exps_amt,0))  
				ELSE (ISNULL(List.resrvd_exps_amt,0) - ISNULL(List.non_bilabl_resrvd_exps_amt,0)) * (ISNULL(List.dedtbl_pol_lim_amt,0)/(ISNULL(List.paid_idnmty_amt,0) - ISNULL(List.non_bilabl_paid_idnmty_amt,0)))
			END


		--A.080.280
		WHEN List.aloc_los_adj_exps_typ_id IN (79) -- ALAE Handling Indicator  = Pro-Rata No Indemnity
		THEN
			CASE 
				WHEN (ISNULL(attbl.subj_paid_idnmty_amt,0) = 0) THEN (ISNULL(List.resrvd_exps_amt,0) - ISNULL(List.non_bilabl_resrvd_exps_amt,0))
				WHEN (ISNULL(attbl.subj_paid_idnmty_amt,0) > 0) 
				THEN 
					CASE
						WHEN ((ISNULL(List.paid_idnmty_amt,0) - ISNULL(List.non_bilabl_paid_idnmty_amt,0)) + (ISNULL(List.resrvd_idnmty_amt,0) - ISNULL(List.non_bilabl_resrvd_idnmty_amt,0)) <  ISNULL(List.dedtbl_pol_lim_amt,0)) THEN (ISNULL(List.resrvd_exps_amt,0) - ISNULL(List.non_bilabl_resrvd_exps_amt,0))  
						WHEN (((ISNULL(List.paid_idnmty_amt,0) - ISNULL(List.non_bilabl_paid_idnmty_amt,0)) + (ISNULL(List.resrvd_idnmty_amt,0) - ISNULL(List.non_bilabl_resrvd_idnmty_amt,0))) >= ISNULL(List.dedtbl_pol_lim_amt,0)) THEN (ISNULL(List.resrvd_exps_amt,0) - ISNULL(List.non_bilabl_resrvd_exps_amt,0)) * (ISNULL(List.dedtbl_pol_lim_amt,0)/((ISNULL(List.paid_idnmty_amt,0) - ISNULL(List.non_bilabl_paid_idnmty_amt,0)) + (ISNULL(List.resrvd_idnmty_amt,0) - ISNULL(List.non_bilabl_resrvd_idnmty_amt,0))))
						ELSE 0
					END
			END

		--A.080.290
		WHEN List.aloc_los_adj_exps_typ_id IN (74) -- ALAE Handling Indicator  = ALAE Capped
		THEN
			CASE 
				WHEN ((ISNULL(List.resrvd_exps_amt,0) - ISNULL(List.non_bilabl_resrvd_exps_amt,0)) <= (ISNULL(List.aloc_los_adj_exps_capped_amt,0) - ISNULL(attbl.subj_paid_exps_amt,0))) THEN (ISNULL(List.resrvd_exps_amt,0) - ISNULL(List.non_bilabl_resrvd_exps_amt,0))  
				WHEN ((ISNULL(List.paid_exps_amt,0) - ISNULL(List.non_bilabl_paid_exps_amt,0)) > (ISNULL(List.aloc_los_adj_exps_capped_amt,0) - ISNULL(attbl.subj_paid_exps_amt,0))) THEN (ISNULL(List.aloc_los_adj_exps_capped_amt,0) - ISNULL(attbl.subj_paid_exps_amt,0))  
				ELSE 0
			END
		ELSE 0
	END
FROM #ARMIS_TMP_TBL_DTL as attbl
INNER JOIN #ARMIS_TMP_TBL_DTL List ON (List.pol_nbr_txt = attbl.pol_nbr_txt AND List.pol_eff_dt = attbl.pol_eff_dt  
AND List.pol_expiry_dt= attbl.pol_expiry_dt AND List.st_id= attbl.st_id)

--Now the Excess Amounts...
UPDATE attbl -- #ARMIS_TMP_TBL_DTL
SET 
	--A.080.110
		attbl.exc_paid_idnmty_amt =
		CASE
			WHEN List.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80) -- ALAE Handling Indicator  = ALL
			THEN (ISNULL(List.paid_idnmty_amt,0) - ISNULL(List.non_bilabl_paid_idnmty_amt,0) - ISNULL(attbl.subj_paid_idnmty_amt,0))
			ELSE 0
		END
FROM #ARMIS_TMP_TBL_DTL as attbl
INNER JOIN #ARMIS_TMP_TBL_DTL List ON (List.pol_nbr_txt = attbl.pol_nbr_txt AND List.pol_eff_dt = attbl.pol_eff_dt  
AND List.pol_expiry_dt= attbl.pol_expiry_dt AND List.st_id= attbl.st_id)

UPDATE attbl -- #ARMIS_TMP_TBL_DTL
SET 
	--A.080.190
		attbl.exc_paid_exps_amt = -- Excess Paid Expense
		CASE
			WHEN List.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80) -- ALAE Handling Indicator  = ALL
			THEN (ISNULL(List.paid_exps_amt,0) - ISNULL(List.non_bilabl_paid_exps_amt,0)) - ISNULL(attbl.subj_paid_exps_amt,0)
			ELSE 0
		END
FROM #ARMIS_TMP_TBL_DTL as attbl
INNER JOIN #ARMIS_TMP_TBL_DTL List ON (List.pol_nbr_txt = attbl.pol_nbr_txt AND List.pol_eff_dt = attbl.pol_eff_dt  
AND List.pol_expiry_dt= attbl.pol_expiry_dt AND List.st_id= attbl.st_id)


UPDATE attbl -- #ARMIS_TMP_TBL_DTL
SET 
	--A.080.220
		attbl.exc_resrvd_idnmty_amt = -- Excess Reserve Indemnity
		CASE
			WHEN List.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80) -- ALAE Handling Indicator  = ALL
			THEN (ISNULL(List.resrvd_idnmty_amt,0) - ISNULL(List.non_bilabl_resrvd_idnmty_amt,0) - ISNULL(attbl.subj_resrvd_idnmty_amt,0))
			ELSE 0
		END
FROM #ARMIS_TMP_TBL_DTL as attbl
INNER JOIN #ARMIS_TMP_TBL_DTL List ON (List.pol_nbr_txt = attbl.pol_nbr_txt AND List.pol_eff_dt = attbl.pol_eff_dt  
AND List.pol_expiry_dt= attbl.pol_expiry_dt AND List.st_id= attbl.st_id)

UPDATE attbl -- #ARMIS_TMP_TBL_DTL
SET 
	--A.080.300
		attbl.exc_resrvd_exps_amt =  -- Excess Reserve Expense
		CASE
			WHEN List.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,80) -- ALAE Handling Indicator  <> 79
			THEN (ISNULL(List.resrvd_exps_amt,0) - ISNULL(List.non_bilabl_resrvd_exps_amt,0) - ISNULL(attbl.subj_resrvd_exps_amt,0))
			ELSE 0
		END
FROM #ARMIS_TMP_TBL_DTL as attbl
INNER JOIN #ARMIS_TMP_TBL_DTL List ON (List.pol_nbr_txt = attbl.pol_nbr_txt AND List.pol_eff_dt = attbl.pol_eff_dt  
AND List.pol_expiry_dt= attbl.pol_expiry_dt AND List.st_id= attbl.st_id)

/*################################################################
STEP 6: Roll-up the Excess amounts from the #ARMIS_TMP_TBL_DTL table into the ARMIS_LOS_POL - By Policy, By State
The four Excess components are summed up at the policy/state level
##################################################################*/

--A.080.350,A.080.360,A.080.370,A.080.380
UPDATE ARMIS_LOS_POL WITH (ROWLOCK) SET exc_paid_idnmty_amt = ARMLosPol1.exc_paid_idnmty_amt,
exc_paid_exps_amt = ARMLosPol1.exc_paid_exps_amt,
exc_resrvd_idnmty_amt = ARMLosPol1.exc_resrvd_idnmty_amt,
exc_resrv_exps_amt = ARMLosPol1.exc_resrv_exps_amt
FROM
(
SELECT ISNULL(SUM(#ARMIS_TMP_TBL_DTL.exc_paid_idnmty_amt),0) AS exc_paid_idnmty_amt, --A.080.350
ISNULL(SUM(#ARMIS_TMP_TBL_DTL.exc_paid_exps_amt),0) AS exc_paid_exps_amt, --A.080.360
ISNULL(SUM(#ARMIS_TMP_TBL_DTL.exc_resrvd_idnmty_amt),0) AS exc_resrvd_idnmty_amt, --A.080.370
ISNULL(SUM(#ARMIS_TMP_TBL_DTL.exc_resrvd_exps_amt),0) AS exc_resrv_exps_amt, --A.080.380
st_id,coml_agmt_id,armis_los_pol_id
FROM #ARMIS_TMP_TBL_DTL
WHERE coml_agmt_id = #ARMIS_TMP_TBL_DTL.coml_agmt_id
AND st_id = #ARMIS_TMP_TBL_DTL.st_id
GROUP BY coml_agmt_id,st_id,armis_los_pol_id
) ARMLosPol1
INNER JOIN
ARMIS_LOS_POL ARMLosPol2 ON ((ARMLosPol1.coml_agmt_id = ARMLosPol2.coml_agmt_id) and (ARMLosPol1.st_id = ARMLosPol2.st_id)
and (ARMLosPol1.armis_los_pol_id = ARMLosPol2.armis_los_pol_id))


/*#################################################################
STEP 7: Update the Subject amounts on the Loss Table
###################################################################*/

Update ARMIS_LOS_POL WITH (ROWLOCK)
Set 
--A.080.390
subj_paid_idnmty_amt = ISNULL(paid_idnmty_amt,0) - ISNULL(non_bilabl_paid_idnmty_amt,0) - ISNULL(exc_paid_idnmty_amt,0),
--A.080.400
subj_paid_exps_amt =  ISNULL(paid_exps_amt,0) - ISNULL(non_bilabl_paid_exps_amt,0) - ISNULL(exc_paid_exps_amt,0),
--A.080.410
subj_resrv_idnmty_amt = ISNULL(resrv_idnmty_amt,0) - ISNULL(non_bilabl_resrv_idnmty_amt,0) - ISNULL(exc_resrvd_idnmty_amt,0),
--A.080.420
subj_resrv_exps_amt = ISNULL(resrv_exps_amt,0) - ISNULL(non_bilabl_resrv_exps_amt,0) - ISNULL(exc_resrv_exps_amt,0)

--select * from ARMIS_LOS_POL

/*#####################################################################
STEP 7: Update the Primary key from the LOSS table into the #ARMIS_TMP_TBL_DTL 
table to create the relationship
######################################################################*/
Update #ARMIS_TMP_TBL_DTL
SET armis_los_pol_id = (Select MAX(armis_los_pol_id) 
from ARMIS_LOS_POL
where coml_agmt_id = #ARMIS_TMP_TBL_DTL.coml_agmt_id
AND st_id = #ARMIS_TMP_TBL_DTL.st_id AND actv_ind=1)

/*#################################################################
STEP 8: Store the #ARMIS_TMP_TBL_DTL table into the ARMIS_LOS_EXC table
###################################################################*/

INSERT INTO [dbo].[ARMIS_LOS_EXC]
           (
	armis_los_pol_id 
	,coml_agmt_id
	,prem_adj_pgm_id
	,custmr_id
	,orgin_clm_nbr_txt
	,clm_nbr_txt
	,lim2_amt
	,site_cd_txt
	,covg_trigr_dt
	,clmt_nm
	,reop_clm_nbr_txt
	,paid_idnmty_amt 
	,paid_exps_amt 
	,resrvd_idnmty_amt 
	,resrvd_exps_amt 
	,non_bilabl_paid_idnmty_amt 
	,non_bilabl_paid_exps_amt 
	,non_bilabl_resrvd_idnmty_amt 
	,non_bilabl_resrvd_exps_amt 
	,subj_paid_idnmty_amt 
	,subj_paid_exps_amt
	,subj_resrvd_idnmty_amt 
	,subj_resrvd_exps_amt
	,exc_paid_idnmty_amt 
	,exc_paid_exps_amt 
	,exc_resrvd_idnmty_amt 
	,exc_resrvd_exps_amt 
	,addn_clm_txt
	,sys_genrt_ind	
	,actv_ind	
	,updt_user_id
	,updt_dt
	,crte_user_id
	,crte_dt
	)
SELECT 
	armis_los_pol_id 
	,coml_agmt_id
	,prem_adj_pgm_id
	,custmr_id
	,orgin_clm_nbr_txt
	,clm_nbr_txt
	,lim2_amt
	,site_cd_txt
	,covg_trigr_dt
	,clmt_nm
	,reop_clm_nbr_txt
	,paid_idnmty_amt 
	,paid_exps_amt 
	,resrvd_idnmty_amt 
	,resrvd_exps_amt 
	,non_bilabl_paid_idnmty_amt 
	,non_bilabl_paid_exps_amt 
	,non_bilabl_resrvd_idnmty_amt 
	,non_bilabl_resrvd_exps_amt 
	,subj_paid_idnmty_amt 
	,subj_paid_exps_amt
	,subj_resrvd_idnmty_amt 
	,subj_resrvd_exps_amt
	,exc_paid_idnmty_amt 
	,exc_paid_exps_amt 
	,exc_resrvd_idnmty_amt 
	,exc_resrvd_exps_amt 
	,addn_clm_txt
	,1	
	,1	
	,1
	,GETDATE()
	,1
	,GETDATE()
	FROM #ARMIS_TMP_TBL_DTL

DROP TABLE #ARMIS_TMP_TBL
DROP TABLE #ARMIS_TMP_TBL_DTL

if @trancount = 0
	commit transaction 

end try
begin catch

	if @trancount = 0  
	begin
		rollback transaction
	end
	
	declare @err_msg varchar(500),
			@err_sev varchar(10),
			@err_no varchar(10)

	select  @err_msg = error_message(),
		    @err_no = error_number(),
			@err_sev = error_severity()

	RAISERROR (@err_msg, -- Message text.
               @err_sev, -- Severity.
               1 -- State.
               )
               
end catch
END

go

if object_id('ModAIS_LoadARMISData') is not null
	print 'Created Procedure ModAIS_LoadARMISData'
else
	print 'Failed Creating Procedure ModAIS_LoadARMISData'
go

if object_id('ModAIS_LoadARMISData') is not null
	grant exec on ModAIS_LoadARMISData to public
go