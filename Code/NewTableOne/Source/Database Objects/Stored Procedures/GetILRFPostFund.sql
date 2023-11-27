if exists (select 1 from sysobjects where name = 'GetILRFPostFund' and type = 'P')
           drop procedure GetILRFPostFund
go
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO


---------------------------------------------------------------------  
-----  
----- PROC NAME: GetILRFPostFund  
-----  
----- VERSION: SQL SERVER 2005  
-----  
----- AUTHOR : VENKAT  
-----  
----- DESCRIPTION: RETURNS DATA WITH RESPECT TO THE GIVEN INVOICE NUMBER.  
-----     
----- ON EXIT:   
-----     
-----  
----- MODIFIED:   
-----     
-----   
---------------------------------------------------------------------  
CREATE PROCEDURE [dbo].[GetILRFPostFund]  
@ADJNO INT,  
@PREMADJPERDID INT,  
@CUSTMRID INT  
  
AS  
BEGIN  
  
-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.  
SET NOCOUNT ON;  
  
  
BEGIN TRY  
  
SELECT   
PREM_ADJ_LOS_REIM_FUND_POST.PREM_ADJ_LOS_REIM_FUND_POST_ID "PREM ADJ LOS REIM FUND POST ID",  
PREM_ADJ_LOS_REIM_FUND_POST.CUSTMR_ID "CUSTMR ID",  
PREM_ADJ_LOS_REIM_FUND_POST.RECV_TYP_ID "RECV TYP ID",  
PREM_ADJ_LOS_REIM_FUND_POST.CURR_AMT "CURRENT AMOUNT",  
PREM_ADJ_LOS_REIM_FUND_POST.AGGR_AMT "AGGR AMT",  
PREM_ADJ_LOS_REIM_FUND_POST.LIM_AMT "LIMITED",  
PREM_ADJ_LOS_REIM_FUND_POST.ADJ_PRIOR_YY_AMT "ADJ PRIOR YY AMT",  
PREM_ADJ_LOS_REIM_FUND_POST.PRIOR_YY_AMT "PRIOR YY AMT",  
PREM_ADJ_LOS_REIM_FUND_POST.POST_AMT "POST AMT",  
POST_TRNS_TYP.TRNS_NM_TXT "TRNS_NM_TXT",  
PREM_ADJ.PREM_ADJ_ID "PREM ADJ ID",  
PREM_ADJ_PERD.PREM_ADJ_PERD_ID "PREM ADJ PERD ID",  
POST_TRNS_TYP.TRNS_NM_TXT "DESCRIPTION",  
POST_TRNS_TYP.POST_TRNS_TYP_ID "POST TRANS TYP ID",  
CASE WHEN POST_TRNS_TYP.TRNS_NM_TXT = 'WC TPD Adjust - C&RM' THEN 1   
WHEN POST_TRNS_TYP.TRNS_NM_TXT = 'Auto TPD Adjust - C&RM' THEN 2  
WHEN POST_TRNS_TYP.TRNS_NM_TXT = 'GL TPD Adjust - C&RM' THEN 3  
WHEN POST_TRNS_TYP.TRNS_NM_TXT = 'WC LCF Adjust - C&RM' THEN 4  
WHEN POST_TRNS_TYP.TRNS_NM_TXT = 'Auto LCF Adjust - C&RM' THEN 5  
WHEN POST_TRNS_TYP.TRNS_NM_TXT = 'GL LCF Adjust - C&RM' THEN 6  
WHEN POST_TRNS_TYP.TRNS_NM_TXT = 'Reserves' THEN 7
WHEN POST_TRNS_TYP.TRNS_NM_TXT = 'CHF LF Adjustment  C&RM' THEN 8  
WHEN POST_TRNS_TYP.TRNS_NM_TXT = 'LBA Adjustment - C&RM' THEN 9  
ELSE 9 END "ORDER"  
FROM PREM_ADJ_LOS_REIM_FUND_POST INNER JOIN PREM_ADJ   
ON PREM_ADJ_LOS_REIM_FUND_POST.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID  
INNER JOIN PREM_ADJ_PERD ON PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID  
AND PREM_ADJ_PERD.PREM_ADJ_PERD_ID = PREM_ADJ_LOS_REIM_FUND_POST.PREM_ADJ_PERD_ID  
INNER JOIN POST_TRNS_TYP ON POST_TRNS_TYP.POST_TRNS_TYP_ID = PREM_ADJ_LOS_REIM_FUND_POST.RECV_TYP_ID  
WHERE PREM_ADJ.PREM_ADJ_ID =  @ADJNO AND PREM_ADJ_PERD.PREM_ADJ_PERD_ID=@PREMADJPERDID  
AND PREM_ADJ_LOS_REIM_FUND_POST.CUSTMR_ID=@CUSTMRID  
ORDER BY "ORDER" ASC  
  
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
if object_id('GetILRFPostFund') is not null
        print 'Created Procedure GetILRFPostFund'
else
        print 'Failed Creating Procedure GetILRFPostFund'
go

if object_id('GetILRFPostFund') is not null
        grant exec on GetILRFPostFund to  public
go

