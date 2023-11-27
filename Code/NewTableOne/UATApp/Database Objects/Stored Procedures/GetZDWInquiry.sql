
IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'GetZDWInquiry' and TYPE = 'P')
	DROP PROC GetZDWInquiry
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO


---------------------------------------------------------------------
-----
-----	Proc Name:		GetZDWInquiry
-----
-----	Version:		SQL Server 2005
-----
-----	Author :		Aakshi Kapoor
-----
-----	Description:	Returns data for given different parameters in ZDW Search.
-----			
-----	Modified:	   
-----			
----- 
---------------------------------------------------------------------
CREATE  PROCEDURE [dbo].[GetZDWInquiry]


AS
BEGIN

SET NOCOUNT ON;

BEGIN TRY


SELECT 
dbo.PREM_ADJ.prem_adj_id "PREM_ADJ_ID",
dbo.PREM_ADJ_PERD.prem_adj_perd_id "PREM_ADJ_PERD_ID",
dbo.PREM_ADJ_PGM.prem_adj_pgm_id "PREM_ADJ_PGM_ID",
dbo.PREM_ADJ_STS.adj_sts_typ_id "ADJ_STS_TYP_ID",
dbo.LKUP.lkup_txt "ADJUSTMENT_STATUS",  
dbo.CUSTMR.full_nm "ACCOUNT_NAME",
dbo.CUSTMR.custmr_id "ACCOUNT_NUMBER",
CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 101) "VALUATION_DATE", 
dbo.PREM_ADJ.drft_invc_nbr_txt "DRFT_INVOICE_NUMBER", 
dbo.PREM_ADJ.fnl_invc_nbr_txt "FNL_INVOICE_NUMBER", 
CONVERT(NVARCHAR(30), dbo.PREM_ADJ.fnl_invc_dt, 101) "FINAL_INVOICE_DATE",
CONVERT(NVARCHAR(30), dbo.PREM_ADJ.drft_invc_dt, 101) "DRAFT_INVOICE_DATE", 
dbo.fn_GetTotalforAdjInv(PREM_ADJ.PREM_ADJ_ID) "INVOICE_AMOUNT", 
CONVERT(NVARCHAR(30), dbo.PREM_ADJ.invc_due_dt, 101) "INVOICE_DATE",
dbo.PREM_ADJ.drft_intrnl_pdf_zdw_key_txt "DRAFT_INTERNAL_KEY", 
dbo.PREM_ADJ.drft_extrnl_pdf_zdw_key_txt "DRAFT_EXTERNAL_KEY", 
dbo.PREM_ADJ.drft_cd_wrksht_pdf_zdw_key_txt "DRAFT_CW_KEY",
dbo.PREM_ADJ.fnl_intrnl_pdf_zdw_key_txt "FNL_INTERNAL_KEY", 
dbo.PREM_ADJ.fnl_extrnl_pdf_zdw_key_txt "FNL_EXTERNAL_KEY", 
dbo.PREM_ADJ.fnl_cd_wrksht_pdf_zdw_key_txt "FNL_CW_KEY",
dbo.PREM_ADJ_PGM.pgm_typ_id "PGM_TYP_ID",
CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.fnl_adj_dt, 101) "FNL_ADJUSTMENT_DATE",
CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) "PGM_EFF_DATE", 
CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) "PGM_EXP_DATE",
dbo.EXTRNL_ORG.full_name "BROKER",
dbo.EXTRNL_ORG.extrnl_org_id "EXTORGID",
dbo.INT_ORG.int_org_id "INTORGID", 
dbo.INT_ORG.FULL_NAME+'/'+dbo.INT_ORG.CITY_NM "BU_OFFICE", 
dbo.INT_ORG.bsn_unt_cd "BU_UNIT_ID",
PERS_BCONTACT.forename+' '+PERS_BCONTACT.surname "BROKER_CONTACT",
dbo.PREM_ADJ_PGM.brkr_conctc_id "BROKER_CONTACT_ID",
(select  PERS_CFS2.forename from PERS AS PERS_CFS2 ,CUSTMR_PERS_REL AS CUSTMR_PERS_REL_CFS2 where
CUSTMR_PERS_REL_CFS2.pers_id=PERS_CFS2.pers_id and dbo.CUSTMR.custmr_id= CUSTMR_PERS_REL_CFS2.custmr_id and CUSTMR_PERS_REL_CFS2.rol_id = 366) AS "CFS2_NAME",
(select  PERS_CFS2.pers_id from PERS AS PERS_CFS2 ,CUSTMR_PERS_REL AS CUSTMR_PERS_REL_CFS2 where
CUSTMR_PERS_REL_CFS2.pers_id=PERS_CFS2.pers_id and dbo.CUSTMR.custmr_id= CUSTMR_PERS_REL_CFS2.custmr_id and CUSTMR_PERS_REL_CFS2.rol_id = 366) AS "CFS2_PERS_ID",
(select  PERS_INSURED.forename from PERS AS PERS_INSURED ,CUSTMR_PERS_REL AS CUSTMR_PERS_REL_INSURED where
CUSTMR_PERS_REL_INSURED.pers_id=PERS_INSURED.pers_id and dbo.CUSTMR.custmr_id= CUSTMR_PERS_REL_INSURED.custmr_id and CUSTMR_PERS_REL_INSURED.rol_id = 397 ) AS "INSURED_CONTACT",
Rtrim(dbo.COML_AGMT.pol_sym_txt)+Rtrim(dbo.COML_AGMT.pol_nbr_txt)+Rtrim(dbo.COML_AGMT.pol_modulus_txt) "POLICY_NO"

FROM dbo.PREM_ADJ   LEFT OUTER JOIN  
dbo.CUSTMR ON dbo.PREM_ADJ.reg_custmr_id = dbo.CUSTMR.custmr_id
LEFT OUTER JOIN dbo.PREM_ADJ_PERD ON  dbo.PREM_ADJ_PERD.prem_adj_id = dbo.PREM_ADJ.prem_adj_id 
LEFT OUTER JOIN dbo.PREM_ADJ_PGM ON  dbo.PREM_ADJ_PGM.prem_adj_pgm_id = dbo.PREM_ADJ_PERD.prem_adj_pgm_id
LEFT OUTER JOIN dbo.COML_AGMT ON  dbo.COML_AGMT.prem_adj_pgm_id = dbo.PREM_ADJ_PERD.prem_adj_pgm_id
LEFT OUTER JOIN dbo.PREM_ADJ_STS ON dbo.PREM_ADJ_STS.prem_adj_id=dbo.PREM_ADJ.prem_adj_id and dbo.PREM_ADJ_STS.prem_adj_sts_id in(select max(dbo.PREM_ADJ_STS.prem_adj_sts_id) from dbo.PREM_ADJ_STS where dbo.PREM_ADJ_STS.prem_adj_id=dbo.PREM_ADJ.prem_adj_id )
LEFT OUTER JOIN dbo.LKUP ON dbo.PREM_ADJ_STS.adj_sts_typ_id = dbo.LKUP.lkup_id
LEFT OUTER JOIN dbo.EXTRNL_ORG ON dbo.EXTRNL_ORG.extrnl_org_id = dbo.PREM_ADJ.brkr_id 
LEFT OUTER JOIN dbo.INT_ORG ON dbo.INT_ORG.int_org_id = dbo.PREM_ADJ.bu_office_id
LEFT OUTER JOIN dbo.PERS AS PERS_BCONTACT ON dbo.PREM_ADJ_PGM.brkr_conctc_id = PERS_BCONTACT .pers_id 
where dbo.COML_AGMT.custmr_id=dbo.CUSTMR.custmr_id

                       

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

IF OBJECT_ID('GetZDWInquiry') IS NOT NULL
	PRINT 'CREATED PROCEDURE GetZDWInquiry'
ELSE
	PRINT 'FAILED CREATING PROCEDURE GetZDWInquiry'
GO

IF OBJECT_ID('GetZDWInquiry') IS NOT NULL
	GRANT EXEC ON GetZDWInquiry TO PUBLIC
GO
