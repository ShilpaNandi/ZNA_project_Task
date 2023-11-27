
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

Create procedure [dbo].[ModAIS_ConfirmTransmittalSentToARiES]
as

declare @trancount int  
  
set @trancount = @@trancount  
  
if @trancount = 0 
	begin
	    begin transaction 
	end
    
begin try

	UPDATE PREM_ADJ WITH (ROWLOCK) SET PREM_ADJ.adj_sts_typ_id=352, adj_sts_eff_dt = getdate( )
	WHERE PREM_ADJ.prem_adj_id IN (
	SELECT DISTINCT PREM_ADJ.prem_adj_id
	 FROM ARIES_TRNSMTL_HIST 
			LEFT JOIN 
			PREM_ADJ ON (PREM_ADJ.PREM_ADJ_ID = ARIES_TRNSMTL_HIST.PREM_ADJ_ID 
			AND PREM_ADJ.FNL_INVC_NBR_TXT IS NOT NULL
			AND PREM_ADJ.historical_adj_ind=0)
	WHERE DBO.fn_GetCurrentTransAdjStatus(PREM_ADJ.PREM_ADJ_ID,reg_custmr_id)=349
	AND trnsmtl_sent_ind<>1)

	INSERT INTO PREM_ADJ_STS (prem_adj_id,custmr_id,adj_sts_typ_id,eff_dt,crte_user_id)
	SELECT DISTINCT PREM_ADJ.prem_adj_id,PREM_ADJ.REG_custmr_id,352,GETDATE(),9999999
	 FROM ARIES_TRNSMTL_HIST 
			LEFT JOIN 
			PREM_ADJ ON (PREM_ADJ.PREM_ADJ_ID = ARIES_TRNSMTL_HIST.PREM_ADJ_ID 
			AND PREM_ADJ.FNL_INVC_NBR_TXT IS NOT NULL
			AND PREM_ADJ.historical_adj_ind=0)
	WHERE DBO.fn_GetCurrentTransAdjStatus(PREM_ADJ.PREM_ADJ_ID,reg_custmr_id)=349
	AND trnsmtl_sent_ind<>1

--UPDATE TRANSMITTAL-SENT-STATUS TO TRUE
	UPDATE ARIES_TRNSMTL_HIST WITH (ROWLOCK) SET trnsmtl_sent_ind=1,updt_dt=getdate( ),updt_user_id=9999999
	 FROM ARIES_TRNSMTL_HIST 
			LEFT JOIN 
			PREM_ADJ ON (PREM_ADJ.PREM_ADJ_ID = ARIES_TRNSMTL_HIST.PREM_ADJ_ID 
			AND PREM_ADJ.FNL_INVC_NBR_TXT IS NOT NULL
			AND PREM_ADJ.historical_adj_ind=0)
	WHERE DBO.fn_GetCurrentTransAdjStatus(CASE WHEN ARIES_TRNSMTL_HIST.Post_cd <> 2 
	THEN ARIES_TRNSMTL_HIST.PREM_ADJ_ID ELSE ARIES_TRNSMTL_HIST.rel_prem_adj_id end,reg_custmr_id)=352
	AND trnsmtl_sent_ind<>1

--	--SET TPA MANUAL POSTINGS TO COMPLETED
	UPDATE ARIES_TRNSMTL_HIST WITH (ROWLOCK) SET trnsmtl_sent_ind=1 WHERE PREM_ADJ_ID IS NULL AND trnsmtl_sent_ind <> 1

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




