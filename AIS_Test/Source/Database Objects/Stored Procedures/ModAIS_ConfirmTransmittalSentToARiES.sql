
if exists (select 1 from sysobjects 
		where name = 'ModAIS_ConfirmTransmittalSentToARiES' and type = 'P')
	drop procedure ModAIS_ConfirmTransmittalSentToARiES
go

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		ModAIS_ConfirmTransmittalSentToARiES
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Anil Nandakumar - 02/16/2009
-----
-----	Description:	Updates statuses on successful FTP to ARiES
-----
-----	Modified:	
-----
---------------------------------------------------------------------

CREATE procedure [dbo].[ModAIS_ConfirmTransmittalSentToARiES]  
@company_cd varchar(6)  
as  
  
declare @trancount int    
    
set @trancount = @@trancount    
    
if @trancount = 0   
 begin  
     begin transaction   
 end  
      
begin try  
  
  
DECLARE @zzccd_txt_Other char(4)  
SET @zzccd_txt_Other =CASE WHEN @company_cd = 'ZC2' THEN 'ZC2' ELSE NULL END  
  
SELECT company_cd=zzccd_txt INTO #ValidCompanyCodes FROM dbo.ARIES_TRNSMTL_HIST WHERE zzccd_txt = 'ZC2' AND @zzccd_txt_Other = 'ZC2'  
UNION  
SELECT company_cd=zzccd_txt FROM dbo.ARIES_TRNSMTL_HIST WHERE zzccd_txt <> 'ZC2' AND @zzccd_txt_Other IS NULL  
  
  
 UPDATE PREM_ADJ WITH (ROWLOCK) SET PREM_ADJ.adj_sts_typ_id=352, adj_sts_eff_dt = getdate( )  
 WHERE PREM_ADJ.prem_adj_id IN (  
 SELECT DISTINCT PREM_ADJ.prem_adj_id  
  FROM ARIES_TRNSMTL_HIST   
  INNER JOIN #ValidCompanyCodes c ON (  
     ARIES_TRNSMTL_HIST.zzccd_txt = c.company_cd  
     )  
   LEFT JOIN   
   PREM_ADJ ON (PREM_ADJ.PREM_ADJ_ID = ARIES_TRNSMTL_HIST.PREM_ADJ_ID   
   AND PREM_ADJ.FNL_INVC_NBR_TXT IS NOT NULL  
   AND PREM_ADJ.historical_adj_ind=0)  
 WHERE   
 DBO.fn_GetCurrentTransAdjStatus(PREM_ADJ.PREM_ADJ_ID,reg_custmr_id)=349  
 AND trnsmtl_sent_ind<>1)  
 --removed @company_cd  
  
 INSERT INTO PREM_ADJ_STS (prem_adj_id,custmr_id,adj_sts_typ_id,eff_dt,crte_user_id)  
 SELECT DISTINCT PREM_ADJ.prem_adj_id,PREM_ADJ.REG_custmr_id,352,GETDATE(),9999999  
  FROM ARIES_TRNSMTL_HIST   
  INNER JOIN #ValidCompanyCodes c ON (  
     ARIES_TRNSMTL_HIST.zzccd_txt = c.company_cd  
     )  
   LEFT JOIN   
   PREM_ADJ ON (PREM_ADJ.PREM_ADJ_ID = ARIES_TRNSMTL_HIST.PREM_ADJ_ID   
   AND PREM_ADJ.FNL_INVC_NBR_TXT IS NOT NULL  
   AND PREM_ADJ.historical_adj_ind=0)  
 WHERE DBO.fn_GetCurrentTransAdjStatus(PREM_ADJ.PREM_ADJ_ID,reg_custmr_id)=349  
 AND trnsmtl_sent_ind<>1   
 --AND zzccd_txt=@company_cd  
  
 --Bug Fix:11281 Updating the transmitted status to all the finalized adjustments   
 --which are not historical and which are not having any postings  
 UPDATE PREM_ADJ WITH (ROWLOCK) SET PREM_ADJ.adj_sts_typ_id=352, adj_sts_eff_dt = getdate( )  
 WHERE PREM_ADJ.prem_adj_id IN (  
 SELECT DISTINCT PREM_ADJ.PREM_ADJ_ID   
 FROM PREM_ADJ   
 WHERE   
 PREM_ADJ.FNL_INVC_NBR_TXT IS NOT NULL  
 AND PREM_ADJ.HISTORICAL_ADJ_IND=0  
 AND ADJ_STS_TYP_ID=349)  
  
  
 INSERT INTO PREM_ADJ_STS (prem_adj_id,custmr_id,adj_sts_typ_id,eff_dt,crte_user_id)  
 SELECT DISTINCT PREM_ADJ.prem_adj_id,PREM_ADJ.REG_custmr_id,352,GETDATE(),9999999  
 FROM PREM_ADJ   
 WHERE   
 PREM_ADJ.FNL_INVC_NBR_TXT IS NOT NULL  
 AND PREM_ADJ.HISTORICAL_ADJ_IND=0  
 AND DBO.fn_GetCurrentTransAdjStatus(PREM_ADJ.PREM_ADJ_ID,reg_custmr_id)=349  
  
--UPDATE TRANSMITTAL-SENT-STATUS TO TRUE  
 UPDATE ARIES_TRNSMTL_HIST WITH (ROWLOCK) SET trnsmtl_sent_ind=1,updt_dt=getdate( ),updt_user_id=9999999  
  FROM ARIES_TRNSMTL_HIST   
  INNER JOIN #ValidCompanyCodes c ON (  
     ARIES_TRNSMTL_HIST.zzccd_txt = c.company_cd  
     )  
   LEFT JOIN   
   PREM_ADJ ON (PREM_ADJ.PREM_ADJ_ID = ARIES_TRNSMTL_HIST.PREM_ADJ_ID   
   AND PREM_ADJ.FNL_INVC_NBR_TXT IS NOT NULL  
   AND PREM_ADJ.historical_adj_ind=0)  
 WHERE DBO.fn_GetCurrentTransAdjStatus(CASE WHEN ARIES_TRNSMTL_HIST.Post_cd <> 2   
 THEN ARIES_TRNSMTL_HIST.PREM_ADJ_ID ELSE ARIES_TRNSMTL_HIST.rel_prem_adj_id end,reg_custmr_id)=352  
 AND trnsmtl_sent_ind<>1   
 --AND zzccd_txt=@company_cd  
   
 --Bug Fix:11281  
 --UPDATE TRANSMITTAL-SENT-STATUS TO TRUE  
 --As per Aimee/Craig, when the ARiES job runs every night, whether or not there are postings associated   
 --with the adjustment, all the adjustments which are in FINAL INVOICE status should be changed to "TRANSMITTED" except historical adjustments  
 --And due to any issues in AIS, if the adjustment failed to transmit to ARiES,   
 --the adjustment status should not be changed to "TRANSMITTED".  
 UPDATE ARIES_TRNSMTL_HIST WITH (ROWLOCK) SET trnsmtl_sent_ind=1,updt_dt=getdate( ),updt_user_id=9999999  
  FROM ARIES_TRNSMTL_HIST   
  INNER JOIN #ValidCompanyCodes c ON (  
     ARIES_TRNSMTL_HIST.zzccd_txt = c.company_cd  
     )  
   LEFT JOIN   
   PREM_ADJ ON (PREM_ADJ.PREM_ADJ_ID = ARIES_TRNSMTL_HIST.REL_PREM_ADJ_ID   
   AND PREM_ADJ.FNL_INVC_NBR_TXT IS NOT NULL  
   AND PREM_ADJ.historical_adj_ind=0)  
 WHERE DBO.fn_GetCurrentTransAdjStatus(CASE WHEN ARIES_TRNSMTL_HIST.Post_cd <> 2   
 THEN ARIES_TRNSMTL_HIST.PREM_ADJ_ID ELSE ARIES_TRNSMTL_HIST.rel_prem_adj_id end,reg_custmr_id)=352  
 AND trnsmtl_sent_ind<>1   
 --AND zzccd_txt=@company_cd  
  
-- --SET TPA MANUAL POSTINGS TO COMPLETED  
 UPDATE ARIES_TRNSMTL_HIST WITH (ROWLOCK)   
 SET trnsmtl_sent_ind=1   
 WHERE PREM_ADJ_ID IS NULL   
 AND trnsmtl_sent_ind <> 1  
 AND zzccd_txt in(select company_cd from #ValidCompanyCodes)  
  
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
go

if object_id('ModAIS_ConfirmTransmittalSentToARiES') is not null
	print 'Created Procedure ModAIS_ConfirmTransmittalSentToARiES'
else
	print 'Failed Creating Procedure ModAIS_ConfirmTransmittalSentToARiES'
go

if object_id('ModAIS_ConfirmTransmittalSentToARiES') is not null
	grant exec on ModAIS_ConfirmTransmittalSentToARiES to public
go