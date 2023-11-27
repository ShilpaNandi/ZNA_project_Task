/************************************ Spreadsheet changes script *******************************************************/


insert into LKUP (crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(getdate(),getdate(),1,1,26,'Internal Spreadsheet','')
insert into LKUP (crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(getdate(),getdate(),1,1,26,'External Spreadsheet','')


/*************************************** Stored Procedure Changes ************************************************************/

IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'GetEscrow' and TYPE = 'P')
	DROP PROC GetEscrow
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	PROC NAME:		GetEscrow
-----
-----	VERSION:		SQL SERVER 2005
-----
-----	AUTHOR :		SUNEEL KUMAR MOGALI
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

CREATE PROCEDURE [dbo].[GetEscrow]
@ADJNO INT,
@FLAG INT,
@HISTFLAG BIT
AS
BEGIN

-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
SET NOCOUNT ON;


BEGIN TRY

----This code is to retrieve list of policies with adjustment number
--declare @sstring varchar(8000)
--set @sstring = dbo.fn_GetPolicyList(@ADJNO)

SELECT CASE @FLAG WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT 
WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT 
ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END "INVOICE NUMBER",
PREM_ADJ.FNL_INVC_DT "FINAL DATE",
CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), DRFT_INVC_DT,101)
 WHEN 2 THEN CONVERT(NVARCHAR(30), FNL_INVC_DT,101)
ELSE  CONVERT(NVARCHAR(30), DRFT_INVC_DT,101) END "INVOICE DATE",
CONVERT(NVARCHAR(30), PREM_ADJ_PGM.STRT_DT, 101) + '-' + CONVERT(NVARCHAR(30), PREM_ADJ_PGM.PLAN_END_DT,101) "PROGRAM PERIOD",
CONVERT(NVARCHAR(30), PREM_ADJ.VALN_DT, 101) "VALUATION DATE",
CUSTMR.FULL_NM "INSURED NAME",
--INT_ORG.FULL_NAME+'/'+INT_ORG.CITY_NM "BU/OFFICE", 
PREM_ADJ_PGM.BRKR_ID "BROKER ID",
EXTRNL_ORG.FULL_NAME "BROKER",
--LKUP.LKUP_TXT+'('+CASE WHEN PGMTYP.LKUP_TXT LIKE 'NON-DEP%' THEN 'Retro' ELSE PGMTYP.LKUP_TXT END +')'  "ADJUSTMENT TYPE",
'Paid'  "ADJUSTMENT TYPE",
PREM_ADJ_PERD.ADJ_NBR_TXT "ADJUSTMENT NUMBER",
COML_AGMT.POL_SYM_TXT + ' ' + RTRIM(COML_AGMT.POL_NBR_TXT)+ '-' +COML_AGMT.POL_MODULUS_TXT "POLICY NUMBER",
--(CASE WHEN dbo.fn_GetPolicyList(@ADJNO,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,399) IS NULL THEN ' ' 
--ELSE RIGHT(dbo.fn_GetPolicyList(@ADJNO,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,399), 
--LEN(dbo.fn_GetPolicyList(@ADJNO,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,399))) END) "POLICIES",
ISNULL(LTRIM(RTRIM(dbo.fn_GetPolicyList(@ADJNO,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,399))),'') "POLICIES",
CONVERT(NVARCHAR(30), COML_AGMT.POL_EFF_DT,101) + '-' + CONVERT(NVARCHAR(30), COML_AGMT.PLANNED_END_DATE, 101) "POLICY PERIOD",
ISNULL(PREM_ADJ_PARMET_SETUP.ESCR_ADJ_PAID_LOS_AMT,0) "PAID LOSS BILLING",
PREM_ADJ_PARMET_SETUP.ESCR_PREVLY_BILED_AMT "ESCR FUND PREV REQ",
PREM_ADJ_PARMET_SETUP.ESCR_ADJ_AMT "ADJUSTED ESCROW FUND",
CASE WHEN PREM_ADJ_PARMET_SETUP.ESCR_ADJ_AMT <= 0 THEN 0 ELSE 
PREM_ADJ_PARMET_SETUP.ESCR_ADJ_AMT END "CURRENT ESCROW FUND",
PREM_ADJ_PARMET_SETUP.ESCR_AMT "AMOUNT BILLED",
PREM_ADJ_PARMET_SETUP.TOT_AMT "INVOICED AMT",
PREM_ADJ_PGM_SETUP.ESCR_PAID_LOS_BIL_MMS_NBR "MONTHS PRIOR",
ISNULL(PREM_ADJ_PGM_SETUP.ESCR_DVSR_NBR,0) "DIVISION",
ISNULL(PREM_ADJ_PGM_SETUP.ESCR_MMS_HELD_AMT,0) "MONTHS HELD FACTOR",
PREM_ADJ_CMMNT.CMMNT_TXT "COMMENTS",
PREM_ADJ.PREM_ADJ_ID "PREM ADJ ID",
PREM_ADJ_PGM.PREM_ADJ_PGM_ID "PREM ADJ PGM ID"
FROM PREM_ADJ 
INNER JOIN PREM_ADJ_PGM ON PREM_ADJ_PGM.ACTV_IND = 1 
INNER JOIN PREM_ADJ_PERD ON PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID
AND PREM_ADJ_PERD.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID
INNER JOIN CUSTMR ON PREM_ADJ_PERD.CUSTMR_ID = CUSTMR.CUSTMR_ID
INNER JOIN EXTRNL_ORG ON EXTRNL_ORG.EXTRNL_ORG_ID = PREM_ADJ_PGM.BRKR_ID
--INNER JOIN INT_ORG ON INT_ORG.INT_ORG_ID = PREM_ADJ_PGM.BSN_UNT_OFC_ID
INNER JOIN PREM_ADJ_PGM_SETUP ON PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID
AND PREM_ADJ_PGM_SETUP.CUSTMR_ID = CUSTMR.CUSTMR_ID
INNER JOIN PREM_ADJ_PARMET_SETUP ON PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = 
PREM_ADJ_PERD.PREM_ADJ_PERD_ID AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID
AND PREM_ADJ_PARMET_SETUP.CUSTMR_ID = CUSTMR.CUSTMR_ID 
AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_ID = PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID
AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID
INNER JOIN COML_AGMT ON COML_AGMT.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID
AND COML_AGMT.CUSTMR_ID = CUSTMR.CUSTMR_ID AND COML_AGMT.ACTV_IND = 1
INNER JOIN PREM_ADJ_PGM_SETUP_POL ON PREM_ADJ_PGM_SETUP_POL.PREM_ADJ_PGM_SETUP_ID =
PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID AND PREM_ADJ_PGM_SETUP_POL.PREM_ADJ_PGM_ID =
PREM_ADJ_PGM.PREM_ADJ_PGM_ID AND PREM_ADJ_PGM_SETUP_POL.CUSTMR_ID = CUSTMR.CUSTMR_ID
AND PREM_ADJ_PGM_SETUP_POL.COML_AGMT_ID = COML_AGMT.COML_AGMT_ID
INNER JOIN PREM_ADJ_PGM_STS ON PREM_ADJ_PGM_STS.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID
AND PREM_ADJ_PGM_STS.PGM_PERD_STS_TYP_ID = 342 AND PREM_ADJ_PGM_STS.STS_CHK_IND = 1
LEFT OUTER JOIN PREM_ADJ_CMMNT ON PREM_ADJ.prem_adj_id = PREM_ADJ_CMMNT.prem_adj_id
AND PREM_ADJ_CMMNT.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
AND PREM_ADJ_CMMNT.CMMNT_CATG_ID = 317
INNER JOIN LKUP on LKUP.LKUP_ID = PREM_ADJ_PGM.PAID_INCUR_TYP_ID
INNER JOIN LKUP PGMTYP ON PGMTYP.LKUP_ID = PREM_ADJ_PGM.PGM_TYP_ID
 WHERE PREM_ADJ.PREM_ADJ_ID =  @ADJNO and PREM_ADJ_PGM_SETUP.ADJ_PARMET_TYP_ID = 399



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

IF OBJECT_ID('GetEscrow') IS NOT NULL
	PRINT 'CREATED PROCEDURE GetEscrow'
ELSE
	PRINT 'FAILED CREATING PROCEDURE GetEscrow'
GO

IF OBJECT_ID('GetEscrow') IS NOT NULL
	GRANT EXEC ON GetEscrow TO PUBLIC
GO


