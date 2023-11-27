
IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'GetMainReport' and TYPE = 'P')
	DROP PROC GetMainReport
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO


---------------------------------------------------------------------
-----
-----	PROC NAME:		GetMainReport
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

CREATE PROCEDURE [dbo].[GetMainReport]
@ADJNO INT,
@FLAG INT

AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
SET NOCOUNT ON;


BEGIN TRY
SELECT  CASE @FLAG WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT 
WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT 
ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END "INVOICE NUMBER",
PREM_ADJ.FNL_INVC_DT "FINAL DATE",
CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), DRFT_INVC_DT,101)
 WHEN 2 THEN CONVERT(NVARCHAR(30), FNL_INVC_DT,101)
ELSE  CONVERT(NVARCHAR(30), DRFT_INVC_DT,101) END "INVOICE DATE",
CONVERT(NVARCHAR(30), PREM_ADJ_PGM.STRT_DT, 101) + '-' + CONVERT(NVARCHAR(30), PREM_ADJ_PGM.PLAN_END_DT,101) "PROGRAM PERIOD",
CONVERT(NVARCHAR(30), PREM_ADJ.VALN_DT, 101) "VALUATION DATE",
CUSTMR.mstr_acct_ind "MSTR IND",
CUSTMR.FULL_NM "INSURED NAME",
INT_ORG.FULL_NAME+'/'+INT_ORG.CITY_NM "BU/OFFICE", 
PREM_ADJ.PREM_ADJ_ID "PREM ADJ ID",
PREM_ADJ_PERD.PREM_ADJ_PERD_ID "PREM ADJ PERD ID",
PREM_ADJ_PGM.PREM_ADJ_PGM_ID "PREM ADJ PGM ID"
FROM PREM_ADJ 
INNER JOIN PREM_ADJ_PGM ON PREM_ADJ_PGM.ACTV_IND = 1 
INNER JOIN PREM_ADJ_PERD ON PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID
AND PREM_ADJ_PERD.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID
INNER JOIN CUSTMR ON PREM_ADJ_PERD.CUSTMR_ID = CUSTMR.CUSTMR_ID 
INNER JOIN INT_ORG ON INT_ORG.INT_ORG_ID = PREM_ADJ_PGM.BSN_UNT_OFC_ID
INNER JOIN COML_AGMT ON COML_AGMT.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID
AND COML_AGMT.CUSTMR_ID = CUSTMR.CUSTMR_ID AND COML_AGMT.ACTV_IND = 1
INNER JOIN PREM_ADJ_PGM_STS ON PREM_ADJ_PGM_STS.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID
AND PREM_ADJ_PGM_STS.PGM_PERD_STS_TYP_ID = 342 AND PREM_ADJ_PGM_STS.STS_CHK_IND = 1
  WHERE PREM_ADJ.PREM_ADJ_ID =  @ADJNO 
ORDER BY PREM_ADJ_PGM.STRT_DT DESC


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

IF OBJECT_ID('GetMainReport') IS NOT NULL
	PRINT 'CREATED PROCEDURE GetMainReport'
ELSE
	PRINT 'FAILED CREATING PROCEDURE GetMainReport'
GO

IF OBJECT_ID('GetMainReport') IS NOT NULL
	GRANT EXEC ON GetMainReport TO PUBLIC
GO










