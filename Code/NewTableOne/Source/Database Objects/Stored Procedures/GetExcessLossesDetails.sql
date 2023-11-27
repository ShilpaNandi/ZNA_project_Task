if exists (select 1 from sysobjects 
		where name = 'GetExcessLossesDetails' and type = 'P')
	drop procedure GetExcessLossesDetails
go

set ansi_nulls off
go  

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-----              
----- Proc Name:  GetExcessLossesDetails              
-----              
----- Version:  SQL Server 2012              
-----              
----- Author :  Dheeraj Nadimpalli              
-----              
----- Description: Returns data for given different parameters in ARMIS_LOS_EXC.              
-----                 
----- Modified:                  
-----                 
-----               
---------------------------------------------------------------------              
            
CREATE PROCEDURE [dbo].[GetExcessLossesDetails]            
@VALDT VARCHAR(100)=NULL,              
@ADJNO INT=NULL,            
@PREM_ADJ_PGM_ID VARCHAR(1000)=NULL,            
@COML_AGM_IDS VARCHAR(1000)=NULL,            
@SYS_GEN BIT=NULL,
@CUSTMR_ID INT=NULL             
AS              
BEGIN             
            
BEGIN TRY             
              
SELECT DISTINCT CONVERT(VARCHAR(10),POL.VALN_DT,101) AS [VALUATION DATE],LOBTYP.LKUP_TXT AS LOB,LTRIM(RTRIM(COML.POL_SYM_TXT)) + ' ' + LTRIM(RTRIM(COML.POL_NBR_TXT)) + ' ' + LTRIM(RTRIM(COML.POL_MODULUS_TXT)) AS [POLICY NO],            
EXC.CUSTMR_ID AS [CUSTOMER ID],CONVERT(VARCHAR(10),PGM.STRT_DT,101) AS [PROGRAM PERIOD EFF DATE],CONVERT(VARCHAR(10),PGM.PLAN_END_DT,101) [PROGRAM PERIOD EXP DATE],            
PGMTYP.LKUP_TXT AS [PROGRAM TYPE],STTYP.ATTR_1_TXT AS [STATE],CONVERT(VARCHAR(10),COML.POL_EFF_DT,101) AS [POLICY EFF DATE],CONVERT(VARCHAR(10),COML.PLANNED_END_DATE,101) AS [POLICY EXP DATE],            
CLM_NBR_TXT AS [CLAIM NO],ADDN_CLM_IND AS [ADDITIONAL CLAIM IND],ADDN_CLM_TXT AS [ADDITIONAL CLAIM],CLMT_NM AS [CLAIMANT NAME],            
CLMSTATUS.LKUP_TXT [CLAIM STATUS],CONVERT(VARCHAR(10),COVG_TRIGR_DT,101) [COVERAGE TRIGGER DATE],LIM2_AMT [LIMIT 2],EXC.PAID_IDNMTY_AMT [TOTAL PAID INDEMNITY]            
,EXC.PAID_EXPS_AMT [TOTAL PAID EXPENSE],EXC.RESRVD_IDNMTY_AMT [TOTAL RESERVED INDEMNITY],EXC.RESRVD_EXPS_AMT [TOTAL RESERVED EXPENSE],            
EXC.NON_BILABL_PAID_IDNMTY_AMT [NONBILLABLE PAID INDEMNITY],EXC.NON_BILABL_PAID_EXPS_AMT [NONBILLABLE PAID EXPENSE],            
EXC.NON_BILABL_RESRVD_IDNMTY_AMT [NONBILLABLE RESERVED INDEMNITY],EXC.NON_BILABL_RESRVD_EXPS_AMT [NONBILLABLE RESERVED EXPENSE],            
CASE            
  WHEN EXC.SYS_GENRT_IND = 1 THEN 'Yes'            
  WHEN EXC.SYS_GENRT_IND = 0 THEN 'No'            
  ELSE ''            
END AS  [SYSTEM GENERATED],
PGM.STRT_DT            
FROM ARMIS_LOS_EXC EXC            
INNER JOIN ARMIS_LOS_POL POL ON POL.ARMIS_LOS_POL_ID=EXC.ARMIS_LOS_POL_ID            
INNER JOIN COML_AGMT COML ON COML.COML_AGMT_ID=EXC.COML_AGMT_ID            
INNER JOIN PREM_ADJ_PGM PGM ON PGM.PREM_ADJ_PGM_ID = EXC.PREM_ADJ_PGM_ID            
INNER JOIN LKUP PGMTYP ON PGM.PGM_TYP_ID=PGMTYP.LKUP_ID            
INNER JOIN LKUP STTYP ON STTYP.LKUP_ID=POL.ST_ID            
INNER JOIN LKUP CLMSTATUS ON CLMSTATUS.LKUP_ID=EXC.CLM_STS_ID       
INNER JOIN LKUP LOBTYP ON LOBTYP.LKUP_ID = COML.covg_typ_id
INNER JOIN PREM_ADJ ADJ ON POL.prem_adj_id = ADJ.prem_adj_id         
WHERE EXC.ACTV_IND=1             
AND  POL.VALN_DT = ISNULL(@VALDT, POL.VALN_DT)   
AND  POL.CUSTMR_ID = ISNULL(@CUSTMR_ID, POL.CUSTMR_ID)
AND ADJ.adj_sts_typ_id <> 347 
AND ADJ.ADJ_CAN_IND<>1 
AND ADJ.ADJ_VOID_IND<>1
AND ADJ.ADJ_RRSN_IND<>1
AND SUBSTRING(ADJ.FNL_INVC_NBR_TXT,1,3)<>'RTV'          
--AND  POL.PREM_ADJ_ID = ISNULL(@ADJNO, POL.PREM_ADJ_ID)            
--AND PGM.PREM_ADJ_PGM_ID = ISNULL(@PREM_ADJ_PGM_ID, PGM.PREM_ADJ_PGM_ID)          
AND (ISNULL(@PREM_ADJ_PGM_ID, '')='' OR PGM.PREM_ADJ_PGM_ID IN (SELECT ITEMS FROM fn_Getidsfromstring(@PREM_ADJ_PGM_ID,',')))         
AND (ISNULL(@COML_AGM_IDS, '')='' OR COML.COML_AGMT_ID IN (SELECT ITEMS FROM fn_Getidsfromstring(@COML_AGM_IDS,',')))             
AND EXC.SYS_GENRT_IND = ISNULL(@SYS_GEN,EXC.SYS_GENRT_IND)            
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

if object_id('GetExcessLossesDetails') is not null
	print 'Created Procedure GetExcessLossesDetails'
else
	print 'Failed Creating Procedure GetExcessLossesDetails'
go

if object_id('GetExcessLossesDetails') is not null
	grant exec on GetExcessLossesDetails to public
go
