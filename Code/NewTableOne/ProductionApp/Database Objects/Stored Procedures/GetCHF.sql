
IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'GetCHF' and TYPE = 'P')
	DROP PROC GetCHF
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	PROC NAME:		GetCHF
-----
-----	VERSION:		SQL SERVER 2005
-----
-----	AUTHOR :		Sreedhar
-----
-----	DESCRIPTION:	RETURNS DATA WITH RESPECT TO THE GIVEN INVOICE NUMBER.
-----			
-----	ON EXIT:	
-----			
-----
-----	MODIFIED:	
-----			
----- 
---------------------------------------------------------------------

CREATE PROCEDURE [dbo].[GetCHF]
@ADJNO INT,
@FLAG INT,
@HISTFLAG BIT
AS
BEGIN

-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
SET NOCOUNT ON;
BEGIN TRY

SELECT     CASE @FLAG WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END
                       AS [INVOICE NUMBER], CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) WHEN 2 THEN CONVERT(NVARCHAR(30), 
                      FNL_INVC_DT, 101) ELSE CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) END AS [INVOICE DATE], CONVERT(NVARCHAR(30), 
                      dbo.PREM_ADJ_PGM.strt_dt, 101) + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) AS [PROGRAM PERIOD], 
                      CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], 
                      dbo.INT_ORG.full_name + '/' + dbo.INT_ORG.city_nm AS [BU/OFFICE], dbo.PREM_ADJ_PGM.brkr_id AS [BROKER ID], 
                      dbo.EXTRNL_ORG.full_name AS BROKER, 
					  CASE WHEN PREM_ADJ_PGM_SETUP.INCLD_ERND_RETRO_PREM_IND = 1 
					  THEN LKUP_AdjTyp.lkup_txt+'('+CASE WHEN PGMTYP.LKUP_TXT LIKE 'NON-DEP%' THEN 'Retro' 
					  ELSE PGMTYP.LKUP_TXT END +')'  ELSE 'CHF' END AS [ADJUSTMENT TYPE],
--LKUP_AdjTyp.lkup_txt+'('+CASE WHEN PGMTYP.LKUP_TXT LIKE 'NON-DEP%' THEN 'Retro' ELSE PGMTYP.LKUP_TXT END +')' AS [ADJUSTMENT TYPE], 
                      dbo.PREM_ADJ_PERD.adj_nbr_txt AS [ADJUSTMENT NUMBER], dbo.PREM_ADJ.prem_adj_id AS [PREM ADJ ID], 
                      dbo.PREM_ADJ_PERD.prem_adj_pgm_id AS [PREM ADJ PGM ID], dbo.fn_GetCHFPolicyList(dbo.PREM_ADJ_PGM_SETUP.prem_adj_pgm_setup_id, dbo.PREM_ADJ_PGM_SETUP.incld_ernd_retro_prem_ind) AS PolicyList, 
                      LKUP_LossType.lkup_txt AS LossType, dbo.PREM_ADJ_PGM_DTL.clm_hndl_fee_clm_rt_nbr AS [Claim Handling Fee], 
                      LKUP_clm_hndl_fee_basis.lkup_txt AS ClaimHandlFeeBasis, sum(dbo.PREM_ADJ_PGM_DTL.clm_hndl_fee_clmt_nbr) AS [Number of Claimants], 
                      dbo.PREM_ADJ_PGM_SETUP.incld_ernd_retro_prem_ind AS IsERP, 
                      (CASE [dbo].[fn_CheckFirstAdjustment](PREM_ADJ_PGM.PREM_ADJ_PGM_ID) WHEN 0 THEN  dbo.PREM_ADJ_PGM_SETUP.depst_amt	 
                      ELSE dbo.PREM_ADJ_PARMET_SETUP.clm_hndl_fee_prev_biled_amt END) AS [PRIOR DED CLAIM HANDLING FEE  TOTAL],
					  dbo.PREM_ADJ_CMMNT.CMMNT_TXT "COMMENTS"
FROM         dbo.PREM_ADJ INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id INNER JOIN
                      dbo.PREM_ADJ_PGM ON dbo.PREM_ADJ_PERD.prem_adj_pgm_id = dbo.PREM_ADJ_PGM.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_PERD.custmr_id = dbo.PREM_ADJ_PGM.custmr_id INNER JOIN
                      dbo.CUSTMR ON dbo.PREM_ADJ_PERD.custmr_id = dbo.CUSTMR.custmr_id INNER JOIN
                      dbo.EXTRNL_ORG ON dbo.PREM_ADJ.brkr_id = dbo.EXTRNL_ORG.extrnl_org_id INNER JOIN
                      dbo.INT_ORG ON dbo.PREM_ADJ.bu_office_id = dbo.INT_ORG.int_org_id INNER JOIN
                      dbo.LKUP AS LKUP_AdjTyp ON dbo.PREM_ADJ_PGM.paid_incur_typ_id = LKUP_AdjTyp.lkup_id INNER JOIN
                      dbo.PREM_ADJ_PGM_SETUP ON dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PGM_SETUP.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_PGM.custmr_id = dbo.PREM_ADJ_PGM_SETUP.custmr_id INNER JOIN
                      dbo.PREM_ADJ_PGM_DTL ON dbo.PREM_ADJ_PGM_SETUP.prem_adj_pgm_setup_id = dbo.PREM_ADJ_PGM_DTL.prem_adj_pgm_setup_id AND 
                      dbo.PREM_ADJ_PGM_SETUP.prem_adj_pgm_id = dbo.PREM_ADJ_PGM_DTL.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_PGM_SETUP.custmr_id = dbo.PREM_ADJ_PGM_DTL.custmr_id INNER JOIN
                      dbo.LKUP AS LKUP_LossType ON dbo.PREM_ADJ_PGM_DTL.clm_hndl_fee_los_typ_id = LKUP_LossType.lkup_id INNER JOIN
                      dbo.LKUP AS PGMTYP ON PGMTYP.lkup_id = dbo.PREM_ADJ_PGM.pgm_typ_id LEFT OUTER JOIN
                      dbo.PREM_ADJ_PARMET_SETUP ON dbo.PREM_ADJ_PERD.prem_adj_perd_id = dbo.PREM_ADJ_PARMET_SETUP.prem_adj_perd_id AND 
                      dbo.PREM_ADJ_PERD.prem_adj_id = dbo.PREM_ADJ_PARMET_SETUP.prem_adj_id AND 
                      dbo.PREM_ADJ_PGM_SETUP.prem_adj_pgm_setup_id = dbo.PREM_ADJ_PARMET_SETUP.prem_adj_pgm_setup_id INNER JOIN
                      dbo.LKUP AS LKUP_clm_hndl_fee_basis ON 
                      dbo.PREM_ADJ_PGM_SETUP.clm_hndl_fee_basis_id = LKUP_clm_hndl_fee_basis.lkup_id LEFT OUTER JOIN
                      dbo.PREM_ADJ_CMMNT ON dbo.PREM_ADJ.prem_adj_id = dbo.PREM_ADJ_CMMNT.prem_adj_id AND dbo.PREM_ADJ_CMMNT.cmmnt_catg_id = 315
WHERE     (dbo.PREM_ADJ.prem_adj_id = @ADJNO) AND (dbo.PREM_ADJ_PGM_SETUP.adj_parmet_typ_id = 398)
                      --AND (dbo.PREM_ADJ_PERD.prem_adj_perd_id in (SELECT MAX(prem_adj_perd_id) AS MaxPeriodID FROM  dbo.PREM_ADJ_PERD AS PERD WHERE (prem_adj_id = @ADJNO) group by Custmr_id))
					  AND (dbo.PREM_ADJ_PGM_SETUP.actv_ind = 1) AND (dbo.PREM_ADJ_PGM_DTL.actv_ind = 1)

GROUP BY LKUP_LossType.lkup_txt,PREM_ADJ.drft_invc_nbr_txt,PREM_ADJ.drft_invc_dt,PREM_ADJ_PGM.strt_dt,
PREM_ADJ_PGM.plan_end_dt,PREM_ADJ.valn_dt,CUSTMR.full_nm,INT_ORG.full_name,INT_ORG.city_nm,
PREM_ADJ_PGM.brkr_id,EXTRNL_ORG.full_name,PREM_ADJ_PGM_SETUP.incld_ernd_retro_prem_ind,
LKUP_clm_hndl_fee_basis.lkup_txt,LKUP_AdjTyp.lkup_txt,PGMTYP.LKUP_TXT,PREM_ADJ_PERD.adj_nbr_txt,
PREM_ADJ.prem_adj_id,PREM_ADJ_PERD.prem_adj_pgm_id,PREM_ADJ_PGM_SETUP.prem_adj_pgm_setup_id,
PREM_ADJ_PGM_DTL.clm_hndl_fee_clm_rt_nbr,
--PREM_ADJ_PGM_DTL.clm_hndl_fee_clmt_nbr,
PREM_ADJ_PGM.prem_adj_pgm_id,
PREM_ADJ_PGM_SETUP.depst_amt,PREM_ADJ_PARMET_SETUP.clm_hndl_fee_prev_biled_amt,PREM_ADJ_CMMNT.cmmnt_txt,
PREM_ADJ.FNL_INVC_NBR_TXT,PREM_ADJ.fnl_invc_dt

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

IF OBJECT_ID('GetCHF') IS NOT NULL
	PRINT 'CREATED PROCEDURE GetCHF'
ELSE
	PRINT 'FAILED CREATING PROCEDURE GetCHF'
GO

IF OBJECT_ID('GetCHF') IS NOT NULL
	GRANT EXEC ON GetCHF TO PUBLIC
GO
