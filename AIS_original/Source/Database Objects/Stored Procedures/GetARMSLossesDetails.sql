if exists (select 1 from sysobjects 
                where name = 'GetARMSLossesDetails' and type = 'P')
        drop procedure GetARMSLossesDetails
go

set ansi_nulls off
go         

---------------------------------------------------------------------            
-----            
----- Proc Name:  GetARMSLossesDetails            
-----            
----- Version:  SQL Server 2012            
-----            
----- Author :  Dheeraj Nadimpalli            
-----            
----- Description: Returns data for given different parameters in ARMIS_LOS_POL.            
-----               
----- Modified:                
-----               
-----             
---------------------------------------------------------------------            
          
CREATE PROCEDURE [dbo].[GetARMSLossesDetails]          
@VALDT VARCHAR(100)=NULL,            
@ADJNO INT=NULL,          
@PREM_ADJ_PGM_ID varchar(1000)=NULL,          
@COML_AGM_IDS VARCHAR(1000)=NULL,          
@SYS_GEN BIT=NULL,
@CUSTMR_ID INT=NULL          
AS            
BEGIN           
          
BEGIN TRY           
            
SELECT DISTINCT CONVERT(VARCHAR(10),POL.VALN_DT,101) AS [VALUATION DATE],LOBTYP.LKUP_TXT AS LOB,LTRIM(RTRIM(COML.POL_SYM_TXT)) + ' ' + LTRIM(RTRIM(COML.POL_NBR_TXT)) + ' ' + LTRIM(RTRIM(COML.POL_MODULUS_TXT)) AS [POLICY NO],          
POL.CUSTMR_ID AS [CUSTOMER ID],CONVERT(VARCHAR(10),PGM.STRT_DT,101) AS [PROGRAM PERIOD EFF DATE],CONVERT(VARCHAR(10),PGM.PLAN_END_DT,101) [PROGRAM PERIOD EXP DATE], --SCGID,         
PGMTYP.LKUP_TXT AS [PROGRAM TYPE],STTYP.ATTR_1_TXT AS [STATE],CONVERT(VARCHAR(10),COML.POL_EFF_DT,101) AS [POLICY EFF DATE],CONVERT(VARCHAR(10),COML.PLANNED_END_DATE,101) AS [POLICY EXP DATE],          
'' AS SCGID,'' [Claim Status],CASE       
  WHEN POL.SYS_GENRT_IND = 1 THEN 'Yes'          
  WHEN POL.SYS_GENRT_IND = 0 THEN 'No'          
  ELSE ''          
END AS  [System Generated] ,paid_idnmty_amt [Total Paid Indemnity],paid_exps_amt [Total Paid Expense],      
 resrv_idnmty_amt [Total Reserved Indemnity],resrv_exps_amt [Total Reserved Expense] ,     
--ORGIN_CLM_NBR_TXT AS [CLAIM NO],ADDN_CLM_IND AS [ADDITIONAL CLAIM IND],ADDN_CLM_TXT AS [ADDITIONAL CLAIM],CLMT_NM AS [CLAIMANT NAME],          
--CLMSTATUS.LKUP_TXT [CLAIM STATUS],CONVERT(VARCHAR(10),COVG_TRIGR_DT,101) [COVERAGE TRIGGER DATE],LIM2_AMT [LIMIT 2],EXC.PAID_IDNMTY_AMT [TOTAL PAID INDEMNITY]          
--,EXC.PAID_EXPS_AMT [TOTAL PAID EXPENSE],EXC.RESRVD_IDNMTY_AMT [TOTAL RESERVED INDEMNITY],EXC.RESRVD_EXPS_AMT [TOTAL RESERVED EXPENSE],          
--EXC.NON_BILABL_PAID_IDNMTY_AMT [NONBILLABlE PAID INDEMNITY],EXC.NON_BILABL_PAID_EXPS_AMT [NONBILLABlE PAID EXPENSE],          
--EXC.NON_BILABL_RESRVD_IDNMTY_AMT [NONBILLABlE RESERVED INDEMNITY],EXC.NON_BILABL_RESRVD_EXPS_AMT [NONBILLABlE RESERVED EXPENSE],          
 PGM.STRT_DT
  
FROM ARMIS_LOS_POL POL      
INNER JOIN COML_AGMT COML ON COML.COML_AGMT_ID=POL.COML_AGMT_ID          
INNER JOIN PREM_ADJ_PGM PGM ON PGM.PREM_ADJ_PGM_ID = POL.PREM_ADJ_PGM_ID          
INNER JOIN LKUP PGMTYP ON PGM.PGM_TYP_ID=PGMTYP.LKUP_ID          
INNER JOIN LKUP STTYP ON STTYP.LKUP_ID=POL.ST_ID    
INNER JOIN LKUP LOBTYP ON LOBTYP.LKUP_ID = COML.covg_typ_id        
--INNER JOIN LKUP CLMSTATUS ON CLMSTATUS.LKUP_ID=POL.CLM_STS_ID
INNER JOIN PREM_ADJ ADJ ON POL.prem_adj_id = ADJ.prem_adj_id          
WHERE POL.ACTV_IND=1           
AND  POL.CUSTMR_ID = ISNULL(@CUSTMR_ID, POL.CUSTMR_ID)  
AND  POL.VALN_DT = ISNULL(@VALDT, POL.VALN_DT)  
AND ADJ.adj_sts_typ_id <> 347   
AND ADJ.ADJ_CAN_IND<>1 
AND ADJ.ADJ_VOID_IND<>1
AND ADJ.ADJ_RRSN_IND<>1
AND SUBSTRING(ADJ.FNL_INVC_NBR_TXT,1,3)<>'RTV'     
--AND  POL.PREM_ADJ_ID = ISNULL(@ADJNO, POL.PREM_ADJ_ID)          
--AND PGM.PREM_ADJ_PGM_ID = ISNULL(@PREM_ADJ_PGM_ID, PGM.PREM_ADJ_PGM_ID)       
AND (ISNULL(@PREM_ADJ_PGM_ID, '')='' OR PGM.PREM_ADJ_PGM_ID IN (SELECT ITEMS FROM fn_Getidsfromstring(@PREM_ADJ_PGM_ID,',')))        
AND (ISNULL(@COML_AGM_IDS, '')='' OR COML.COML_AGMT_ID IN (SELECT ITEMS FROM fn_Getidsfromstring(@COML_AGM_IDS,',')))           
AND POL.SYS_GENRT_IND = ISNULL(@SYS_GEN,POL.SYS_GENRT_IND)        
ORDER BY   PGM.STRT_DT DESC,[PROGRAM TYPE] ASC,[POLICY NO] ASC    
          
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
if object_id('GetARMSLossesDetails') is not null
        print 'Created Procedure GetARMSLossesDetails'
else
        print 'Failed Creating Procedure GetARMSLossesDetails'
go

if object_id('GetARMSLossesDetails') is not null
        grant exec on GetARMSLossesDetails to  public
go 
  