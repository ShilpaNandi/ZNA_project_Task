
IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'ModAIS_SendSummaryInvoiceToZDW' and TYPE = 'P')
	DROP PROC ModAIS_SendSummaryInvoiceToZDW
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO


---------------------------------------------------------------------
-----
----- Proc Name:  ModAIS_SendSummaryInvoiceToZDW
-----
----- Version:  SQL Server 2008
-----
----- Created:  Dheeraj Nadimpalli - 09/15/2014
-----
----- Description: Retrieves ZDW dunning adjustments
-----
----- Modified: 
-----
---------------------------------------------------------------------

ALTER procedure [dbo].[ModAIS_SendSummaryInvoiceToZDW]
@COMPANY_CD varchar(10) 
as

    
begin try

 SELECT prem_adj_id,fnl_invc_nbr_txt FROM PREM_ADJ 
 INNER JOIN CUSTMR ON PREM_ADJ.reg_custmr_id = CUSTMR.custmr_id
 inner join LKUP on lkup_id=company_cd
 WHERE ZDW_SENT_STATUS IS NULL AND adj_sts_typ_id=352
 AND lkup_txt=@COMPANY_CD AND ISNULL(CUSTMR.custmr_test_ind,0) <>1
   
  
end try  
begin catch  
     
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

GO

IF OBJECT_ID('ModAIS_SendSummaryInvoiceToZDW') IS NOT NULL
	PRINT 'CREATED PROCEDURE ModAIS_SendSummaryInvoiceToZDW'
ELSE
	PRINT 'FAILED CREATING PROCEDURE ModAIS_SendSummaryInvoiceToZDW'
GO

IF OBJECT_ID('ModAIS_SendSummaryInvoiceToZDW') IS NOT NULL
	GRANT EXEC ON ModAIS_SendSummaryInvoiceToZDW TO PUBLIC
GO