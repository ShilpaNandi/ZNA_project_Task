
IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'GetAdjustmentInvoice' and TYPE = 'P')
	DROP PROC GetAdjustmentInvoice
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		GetAdjustmentInvoice
-----
-----	Version:		SQL Server 2005
-----
-----	Author :		Suneel Kumar Mogali
-----
-----	Description:	Returns data for given different parameters in invoice.
-----			
-----	On Exit:	
-----			
-----
-----	Modified:	
-----	11 July 2014 -- Rajaji -- Added company_cd column
----- 
---------------------------------------------------------------------
CREATE PROCEDURE [dbo].[GetAdjustmentInvoice]
@ADJNO INT,
@FLAG INT,
@HISTFLAG BIT,
@CMTCATGID INT
AS
BEGIN


-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
SET NOCOUNT ON;


BEGIN TRY

SELECT CASE @FLAG WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT 
WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT 
ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END "INVOICE NUMBER",
PREM_ADJ.FNL_INVC_DT "FINAL DATE",
CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), DRFT_INVC_DT,101)
 WHEN 2 THEN CONVERT(NVARCHAR(30), FNL_INVC_DT,101)
ELSE  CONVERT(NVARCHAR(30), DRFT_INVC_DT,101) END "INVOICE DATE",
CONVERT(NVARCHAR(30), PREM_ADJ_PGM.STRT_DT, 101) + '-' + CONVERT(NVARCHAR(30), PREM_ADJ_PGM.PLAN_END_DT,101) "PROGRAM PERIOD",
CONVERT(NVARCHAR(30), PREM_ADJ.VALN_DT, 101) "VALUATION DATE",
CONVERT(NVARCHAR(30), PREM_ADJ.INVC_DUE_DT, 101) "INVOICE DUE DATE",
CUSTMR.FULL_NM "INSURED NAME",
PREM_ADJ_PERD.ADJ_NBR_TXT "ADJUSTMENT NUMBER",
PREM_ADJ_PGM.BRKR_ID "BROKER ID",
CUSTMR1.FULL_NM "CUSTOMER NAME",
CUSTMR1.mstr_acct_ind "CUSTOMER REL ID",
CUSTMR.mstr_acct_ind "SORT MST IND",
dbo.fn_CheckRelAccounts(CUSTMR1.CUSTMR_ID) "REL ACCT IND",
POST_ADDR.ADDR_LN_1_TXT "CUSTOMER ADDRESS1",
POST_ADDR.ADDR_LN_2_TXT "CUSTOMER ADDRESS2",  
POST_ADDR.CITY_TXT "CUSTOMER CITY",
LKUP.attr_1_txt "CUSTOMER STATE",
PREM_ADJ_PGM.STRT_DT "STRT DT",
PREM_ADJ_PGM.PLAN_END_DT "END DT",
CASE WHEN LEN(POST_ADDR.POST_CD_TXT)=7 THEN POST_ADDR.POST_CD_TXT ELSE substring(POST_ADDR.POST_CD_TXT,1,5) + CASE WHEN substring(POST_ADDR.POST_CD_TXT,6,4) IS NULL OR substring(POST_ADDR.POST_CD_TXT,6,4) = '' THEN '' ELSE '-' END + substring(POST_ADDR.POST_CD_TXT,6,4)   END
 + ' ' + 
 CASE LKUP.lkup_txt WHEN 'Alberta' THEN 'Canada' 
 WHEN 'British Columbia' THEN 'Canada'
 WHEN 'Manitoba' THEN 'Canada'
 WHEN 'New Brunswick' THEN 'Canada'
 WHEN 'Newfoundland and Labrador' THEN 'Canada'
 WHEN 'Nova Scotia' THEN 'Canada'
 WHEN 'Northwest Territories' THEN 'Canada'
 WHEN 'Nunavut' THEN 'Canada'
 WHEN 'Ontario' THEN 'Canada'
 WHEN 'Prince Edward Island' THEN 'Canada'
 WHEN 'Quebec' THEN 'Canada'
 WHEN 'Saskatchewan' THEN 'Canada'
 WHEN 'Yukon' THEN 'Canada'
 ELSE '' END 
  
 "CUSTOMER ZIP",
PERS1.PERS_ID "BROKER PERS ID",
PREM_ADJ_PGM.BRKR_ID "BROKER ID",
EXTRNL_ORG.FULL_NAME "BROKER NAME",
POST_ADDR1.ADDR_LN_1_TXT "BROKER ADDRESS1",
POST_ADDR1.ADDR_LN_2_TXT "BROKER ADDRESS2",
POST_ADDR1.CITY_TXT "BROKER CITY",
--CASE WHEN CUSTMR.company_cd=733 THEN substring(POST_ADDR1.POST_CD_TXT,1,5) + CASE WHEN substring(POST_ADDR1.POST_CD_TXT,6,4) IS NULL OR substring(POST_ADDR1.POST_CD_TXT,6,4) = '' THEN '' ELSE '-' END + substring(POST_ADDR1.POST_CD_TXT,6,4) ELSE POST_ADDR1.POST_CD_TXT END
CASE WHEN LEN(POST_ADDR1.POST_CD_TXT)=7 THEN POST_ADDR1.POST_CD_TXT ELSE substring(POST_ADDR1.POST_CD_TXT,1,5) + CASE WHEN substring(POST_ADDR1.POST_CD_TXT,6,4) IS NULL OR substring(POST_ADDR1.POST_CD_TXT,6,4) = '' THEN '' ELSE '-' END + substring(POST_ADDR1.POST_CD_TXT,6,4)   END
 + ' ' + 
 CASE LKUP1.lkup_txt WHEN 'Alberta' THEN 'Canada' 
 WHEN 'British Columbia' THEN 'Canada'
 WHEN 'Manitoba' THEN 'Canada'
 WHEN 'New Brunswick' THEN 'Canada'
 WHEN 'Newfoundland and Labrador' THEN 'Canada'
 WHEN 'Nova Scotia' THEN 'Canada'
 WHEN 'Northwest Territories' THEN 'Canada'
 WHEN 'Nunavut' THEN 'Canada'
 WHEN 'Ontario' THEN 'Canada'
 WHEN 'Prince Edward Island' THEN 'Canada'
 WHEN 'Quebec' THEN 'Canada'
 WHEN 'Saskatchewan' THEN 'Canada'
 WHEN 'Yukon' THEN 'Canada'
  ELSE '' END 
 
 "BROKER ZIP",
LKUP1.attr_1_txt "BROKER STATE",
isnull(LKUP_CUST_Title.lkup_txt,'') + ' ' +PERS.FORENAME+' '+PERS.SURNAME "CUSTOMER CONTACT NAME",
isnull(LKUP_BRK_Title.lkup_txt,'') + ' ' + PERS1.FORENAME+' '+PERS1.SURNAME "BROKER CONTACT NAME",
PREM_ADJ_CMMNT.CMMNT_TXT "COMMENTS",
PREM_ADJ.REL_PREM_ADJ_ID "REL PREM ADJ ID",
PREM_ADJ.ADJ_RRSN_IND "REV INDICATOR",
dbo.[fn_GetPrevAdjId](PREM_ADJ.REL_PREM_ADJ_ID,2) "PREV INVOICE DATE",
CASE WHEN PREM_ADJ_PERD_TOT.INVC_ADJ_TYP_TXT = 'Retro' THEN 1 
WHEN PREM_ADJ_PERD_TOT.INVC_ADJ_TYP_TXT LIKE 'DEP%' THEN 2
WHEN PREM_ADJ_PERD_TOT.INVC_ADJ_TYP_TXT = 'WC Deductible' THEN 3
WHEN PREM_ADJ_PERD_TOT.INVC_ADJ_TYP_TXT = 'Loss Based Assessments' THEN 4
WHEN PREM_ADJ_PERD_TOT.INVC_ADJ_TYP_TXT = 'Claim Handling Fees' THEN 5
WHEN PREM_ADJ_PERD_TOT.INVC_ADJ_TYP_TXT = 'Kentucky Taxes' THEN 6
WHEN PREM_ADJ_PERD_TOT.INVC_ADJ_TYP_TXT = 'Oregon Taxes' THEN 7
WHEN PREM_ADJ_PERD_TOT.INVC_ADJ_TYP_TXT = 'Kentucky & Oregon Taxes' THEN 8
WHEN PREM_ADJ_PERD_TOT.INVC_ADJ_TYP_TXT = 'Residual Market Charge' THEN 9
WHEN PREM_ADJ_PERD_TOT.INVC_ADJ_TYP_TXT = 'NY Second Injury Fund' THEN 10
WHEN PREM_ADJ_PERD_TOT.INVC_ADJ_TYP_TXT = 'Loss Reimbursement Fund' THEN 12
WHEN PREM_ADJ_PERD_TOT.INVC_ADJ_TYP_TXT = 'State Sales & Service Tax' THEN 13
WHEN PREM_ADJ_PERD_TOT.INVC_ADJ_TYP_TXT = 'Loss Fund' THEN 14 
ELSE 11 END "ORDER",
PREM_ADJ_PERD_TOT.INVC_ADJ_TYP_TXT "ADJUSTMENT TYPE",
PREM_ADJ.ADJ_VOID_IND "VOID IND",
PREM_ADJ_PERD_TOT.TOT_AMT "AMOUNT BILLED",
dbo.fn_GetTotalforAdjInv(PREM_ADJ.PREM_ADJ_ID) "TOTAL AMOUNT BILLED",
--CASE WHEN dbo.fn_GetTotalforAdjInv(PREM_ADJ.PREM_ADJ_ID) < 0 
--THEN '$'+'('+Convert(varchar,Convert(money,-dbo.fn_GetTotalforAdjInv(PREM_ADJ.PREM_ADJ_ID)),1)+')' 
--ELSE '$'+Convert(varchar,Convert(money,dbo.fn_GetTotalforAdjInv(PREM_ADJ.PREM_ADJ_ID)),1) END "TOT AMT BROKER",
CASE WHEN dbo.fn_GetTotalforAdjInv(PREM_ADJ.PREM_ADJ_ID) < 0 
THEN '$'+'('+left(Convert(varchar,Convert(money,round(-dbo.fn_GetTotalforAdjInv(PREM_ADJ.PREM_ADJ_ID),0)),1),
charindex('.',Convert(varchar,Convert(money,round(-dbo.fn_GetTotalforAdjInv(PREM_ADJ.PREM_ADJ_ID),0)),1))-1)+')' 
ELSE '$'+left(Convert(varchar,Convert(money,round(dbo.fn_GetTotalforAdjInv(PREM_ADJ.PREM_ADJ_ID),0)),1),
charindex('.',Convert(varchar,Convert(money,round(dbo.fn_GetTotalforAdjInv(PREM_ADJ.PREM_ADJ_ID),0)),1))-1)
 END "TOT AMT BROKER",
CUSTMR_PERS_REL.ROL_ID "ROLE ID",
isnull(LKUP_ANS_Title.lkup_txt,'') + ' ' + PERS2.FORENAME+' '+PERS2.SURNAME "ANALYST NAME",

case when PERS2.PHONE_NBR_1_EXTNS IS NOT NULL AND PERS2.PHONE_NBR_1_EXTNS <> '' then
substring('(' + substring(PERS2.PHONE_NBR_1_TXT,1,3) + ') ' + 
substring(PERS2.PHONE_NBR_1_TXT,4,3) + '-' + 
substring(PERS2.PHONE_NBR_1_TXT,7,4),1,15) +' '+'ext: '+PERS2.PHONE_NBR_1_EXTNS 
ELSE 
substring('(' + substring(PERS2.PHONE_NBR_1_TXT,1,3) + ') ' + 
substring(PERS2.PHONE_NBR_1_TXT,4,3) + '-' + 
substring(PERS2.PHONE_NBR_1_TXT,7,4),1,15)  END
"ANYALYST PHONE NUMBER",

PERS2.EMAIL_TXT "ANALYST EMAIL",
isnull(LKUP_CREP_Title.lkup_txt,'') + ' ' +PERS3.FORENAME+' '+PERS3.SURNAME "COLLECTION REP NAME",

case when PERS3.PHONE_NBR_1_EXTNS IS NOT NULL AND PERS3.PHONE_NBR_1_EXTNS <> '' then
substring('(' + substring(PERS3.PHONE_NBR_1_TXT,1,3) + ') ' + 
substring(PERS3.PHONE_NBR_1_TXT,4,3) + '-' + 
substring(PERS3.PHONE_NBR_1_TXT,7,4),1,15) +' '+'ext: '+PERS3.PHONE_NBR_1_EXTNS 
ELSE 
substring('(' + substring(PERS3.PHONE_NBR_1_TXT,1,3) + ') ' + 
substring(PERS3.PHONE_NBR_1_TXT,4,3) + '-' + 
substring(PERS3.PHONE_NBR_1_TXT,7,4),1,15)  END "COLLECTION REP PHONE NUMBER",
PERS3.EMAIL_TXT "COLLECTION REP EMAIL",
PREM_ADJ.PREM_ADJ_ID "PREM ADJ ID",
PREM_ADJ_PERD.PREM_ADJ_PERD_ID "PREM ADJ PERD ID",
CUSTMR1.company_cd
FROM PREM_ADJ 
INNER JOIN PREM_ADJ_PGM ON 
PREM_ADJ_PGM.ACTV_IND = 1 
INNER JOIN PREM_ADJ_PERD ON PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID
AND PREM_ADJ_PERD.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID
INNER JOIN CUSTMR ON PREM_ADJ_PERD.CUSTMR_ID = CUSTMR.CUSTMR_ID
INNER JOIN CUSTMR CUSTMR1 ON CUSTMR1.CUSTMR_ID = PREM_ADJ.reg_custmr_id 
INNER JOIN PREM_ADJ_PERD_TOT ON PREM_ADJ_PERD_TOT.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID
AND PREM_ADJ_PERD_TOT.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
AND PREM_ADJ_PERD_TOT.CUSTMR_ID = PREM_ADJ_PERD.CUSTMR_ID
LEFT OUTER JOIN CUSTMR_PERS_REL ON CUSTMR_PERS_REL.CUSTMR_ID = CUSTMR1.CUSTMR_ID 
AND CUSTMR_PERS_REL.ROL_ID = 397
LEFT OUTER JOIN PERS ON PERS.PERS_ID = CUSTMR_PERS_REL.PERS_ID
AND CUSTMR_PERS_REL.ROL_ID = 397
LEFT OUTER JOIN POST_ADDR ON POST_ADDR.PERS_ID = PERS.PERS_ID
LEFT OUTER JOIN LKUP ON LKUP.LKUP_ID = POST_ADDR.ST_ID
LEFT OUTER JOIN EXTRNL_ORG ON EXTRNL_ORG.EXTRNL_ORG_ID = PREM_ADJ.BRKR_ID
LEFT OUTER JOIN PERS PERS1 ON PERS1.PERS_ID = dbo.fn_GetBrokerContactID(@ADJNO)
LEFT OUTER JOIN POST_ADDR POST_ADDR1 ON POST_ADDR1.PERS_ID = PERS1.PERS_ID
LEFT OUTER JOIN LKUP LKUP1 ON LKUP1.LKUP_ID = POST_ADDR1.ST_ID
LEFT OUTER JOIN PREM_ADJ_CMMNT ON PREM_ADJ.prem_adj_id = PREM_ADJ_CMMNT.prem_adj_id
AND PREM_ADJ_CMMNT.CMMNT_CATG_ID = @CMTCATGID
LEFT OUTER JOIN CUSTMR_PERS_REL ANALYST ON ANALYST.CUSTMR_ID = CUSTMR.CUSTMR_ID
AND ANALYST.ROL_ID = 366
LEFT OUTER JOIN PERS PERS2 ON PERS2.PERS_ID = ANALYST.PERS_ID
LEFT OUTER JOIN CUSTMR_PERS_REL COLLECTION_REP ON COLLECTION_REP.CUSTMR_ID = CUSTMR.CUSTMR_ID
AND COLLECTION_REP.ROL_ID = 364
LEFT OUTER JOIN PERS PERS3 ON PERS3.PERS_ID = COLLECTION_REP.PERS_ID 
INNER JOIN PREM_ADJ_PGM_STS ON PREM_ADJ_PGM_STS.PREM_ADJ_PGM_ID = PREM_ADJ_PGM.PREM_ADJ_PGM_ID
AND PREM_ADJ_PGM_STS.PGM_PERD_STS_TYP_ID = 342 AND PREM_ADJ_PGM_STS.STS_CHK_IND = 1 LEFT OUTER JOIN
LKUP AS LKUP_BRK_Title ON PERS1.prefx_ttl_id = LKUP_BRK_Title.lkup_id LEFT OUTER JOIN
LKUP AS LKUP_CREP_Title ON PERS3.prefx_ttl_id = LKUP_CREP_Title.lkup_id LEFT OUTER JOIN
dbo.LKUP AS LKUP_ANS_Title ON PERS2.prefx_ttl_id = LKUP_ANS_Title.lkup_id LEFT OUTER JOIN
dbo.LKUP AS LKUP_CUST_Title ON PERS.prefx_ttl_id = LKUP_CUST_Title.lkup_id

 WHERE PREM_ADJ.PREM_ADJ_ID =  @ADJNO
ORDER BY "STRT DT" DESC,"SORT MST IND" DESC,"INSURED NAME" DESC,"ORDER" ASC

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

IF OBJECT_ID('GetAdjustmentInvoice') IS NOT NULL
	PRINT 'CREATED PROCEDURE GetAdjustmentInvoice'
ELSE
	PRINT 'FAILED CREATING PROCEDURE GetAdjustmentInvoice'
GO

IF OBJECT_ID('GetAdjustmentInvoice') IS NOT NULL
	GRANT EXEC ON GetAdjustmentInvoice TO PUBLIC
GO





