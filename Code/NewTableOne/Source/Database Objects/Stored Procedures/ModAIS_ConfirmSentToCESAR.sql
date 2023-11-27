IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'ModAIS_ConfirmSentToCESAR' and TYPE = 'P')
	DROP PROC ModAIS_ConfirmSentToCESAR
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------------------------------------------------
-----
----- Proc Name:  ModAIS_ConfirmSentToCESAR
-----
----- Version:  SQL Server 2008
-----
----- Created:  Dheeraj Nadimpalli
-----
----- Description: Updates statuses on successful FTP to CESAR
-----
----- Modified: 
-----
---------------------------------------------------------------------

CREATE procedure [dbo].[ModAIS_ConfirmSentToCESAR]

as

declare @trancount int  
  
set @trancount = @@trancount  
  
if @trancount = 0 
 begin
     begin transaction 
 end
    
begin try


UPDATE CESAR_CODING_HIST WITH (ROWLOCK) SET cesar_coding_sent_ind=1 WHERE cesar_coding_sent_ind=0
  

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

GO

IF OBJECT_ID('ModAIS_ConfirmSentToCESAR') IS NOT NULL
	PRINT 'CREATED PROCEDURE ModAIS_ConfirmSentToCESAR'
ELSE
	PRINT 'FAILED CREATING PROCEDURE ModAIS_ConfirmSentToCESAR'
GO

IF OBJECT_ID('ModAIS_ConfirmSentToCESAR') IS NOT NULL
	GRANT EXEC ON ModAIS_ConfirmSentToCESAR TO PUBLIC
GO