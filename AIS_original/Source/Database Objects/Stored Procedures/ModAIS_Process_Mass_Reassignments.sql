IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'ModAIS_Process_Mass_Reassignments' and TYPE = 'P')
	DROP PROC ModAIS_Process_Mass_Reassignments
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
            
      
---------------------------------------------------------------------            
-----            
----- PROC NAME:  ModAIS_Process_Mass_Reassignments            
-----            
----- VERSION:  SQL SERVER 2008            
-----            
----- AUTHOR :  Dheeraj Nadimpalli            
-----            
----- DESCRIPTION: For saving the responsibilities for given custmrs (As part of mass reassignment WO)       
----            
----- ON EXIT:             
-----               
-----            
----- MODIFIED:             
-----               
-----             
---------------------------------------------------------------------            
    
CREATE procedure [dbo].[ModAIS_Process_Mass_Reassignments]     
@create_user_id varchar(500),    
@dtUploadDateTime datetime,    
@err_msg_op varchar(1000) output,    
@debug bit = 0    
as    
    
begin    
 set nocount on    
    
declare @mass_reassign_stage_id int, 
  @acct_id int,
  @acct_setup_qc int,    
  @adj_qc_100 int,    
  @adj_qc_20 int,    
  @aries_qc int,    
  @crm_admn_anlst int,  
  @crm_col_splst int, 
  @cfs1 int, 
  @cfs2 int,    
  @lss_admin int,    
  @reconciler int,    
  @txtUsrNm varchar(500),  
  @datCreation datetime,
  @intUserID int,
  

  @validation_message varchar(MAX),    
  @txtErrorDesc_OUT varchar(max),    
  @txtStatusMessage varchar(Max),    
  @err_message varchar(500), 
  @trancount int    
    
if @debug = 1    
begin    
print 'Upload: Mass_Reassignments Processing started'    
end    
    
set @trancount = @@trancount    
--print @trancount    
if @trancount >= 1    
    save transaction sp_Process_Mass_Reassignments    
else    
    begin transaction    
    
    
begin try    
    
     
if @debug = 1    
begin    
    
print' @create_user_id:- ' + convert(varchar(500), @create_user_id)      
print' @dtUploadDateTime:- ' + convert(varchar(500), @dtUploadDateTime)      
    
end    
    
       
  declare Mass_Reassignments_basic cursor LOCAL FAST_FORWARD READ_ONLY     
  for     
  SELECT [mass_reassign_stage_id]
      ,[acct_id]
      ,[acct_setup_qc]
      ,[adj_qc_100]
      ,[adj_qc_20]
      ,[aries_qc]
      ,[crm_admn_anlst]
      ,[crm_col_splst]
      ,[cfs1]
      ,[cfs2]
      ,[lss_admin]
      ,[reconciler]
      ,[crte_user_id]
      ,[crte_dt]
  FROM [dbo].[MASS_REASSIGN_STAGE]
  WHERE crte_user_id=@create_user_id    
  AND crte_dt=@dtUploadDateTime
      
    
  open Mass_Reassignments_basic    
  fetch Mass_Reassignments_basic into @mass_reassign_stage_id,@acct_id,@acct_setup_qc,@adj_qc_100,@adj_qc_20,@aries_qc,@crm_admn_anlst,@crm_col_splst,@cfs1,@cfs2,@lss_admin,@reconciler,@txtUsrNm,@datCreation    
    
  --set @prem_adj_pgm_setup_id_tracker = 0    
    set @validation_message=''    
    select @intUserID=pers_id from PERS where external_reference=@txtUsrNm
    set @intuserid= isnull(@intuserid,0)
  while @@Fetch_Status = 0    
   begin    
    begin    
     if @debug = 1    
     begin    
         print'*******************Uploads: START OF ITERATION*********'     
         print'---------------Input Params-------------------'     
		 print' @mass_reassign_stage_id:- ' + convert(varchar(200), @mass_reassign_stage_id) 
		 print' @acct_id:- ' + convert(varchar(200), @acct_id)      
		 print' @acct_setup_qc:- ' + convert(varchar(200), @acct_setup_qc)      
		 print' @adj_qc_100: ' + convert(varchar(200), @adj_qc_100)      
		 print' @adj_qc_20: ' + convert(varchar(200), @adj_qc_20)    
		 print' @aries_qc: ' + convert(varchar(200), @aries_qc )      
		 print' @crm_admn_anlst: ' + convert(varchar(200), @crm_admn_anlst )     
		 print' @crm_col_splst: ' + convert(varchar(200), @crm_col_splst)      
		 print' @cfs1: ' + convert(varchar(200), @cfs1)     
		 print' @cfs2: ' + convert(varchar(200), @cfs2 )      
		 print' @lss_admin: ' + convert(varchar(200), @lss_admin )    
		 print' @reconciler: ' + convert(varchar(200), @reconciler )    
		 print' @txtUsrNm: ' + convert(varchar(200), @txtUsrNm )    
		 print' @datCreation: ' + convert(varchar(200), @datCreation )    
		   
     end    
         
     SET @validation_message=''
     
      if @debug = 1 
      begin       
      Print 'ACCOUNT SETUP QC Processing' 
      end
      IF(ISNULL(@acct_setup_qc,0)<>-1 AND (SELECT COUNT(*) FROM [CUSTMR_PERS_REL] where custmr_id=@acct_id and rol_id=359)=0)      
		   BEGIN      
				INSERT INTO [CUSTMR_PERS_REL](custmr_id,pers_id,rol_id,crte_dt,crte_user_id)      
				SELECT @acct_id,@acct_setup_qc,359,GETDATE(),@intUserID   
				 if @debug = 1 
				 begin   
				 print 'ACCOUNT SETUP QC is created successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@acct_setup_qc)+'.'  
				 end
     
		   END      
      ELSE IF(ISNULL(@acct_setup_qc,0)<>-1)      
            BEGIN      
			  UPDATE [CUSTMR_PERS_REL] 
			  SET pers_id=@acct_setup_qc,      
			  [updt_user_id]=@intUserID,
			  [updt_dt]=GETDATE() 
			  WHERE custmr_id=@acct_id and  rol_id=359
			  if @debug = 1 
				 begin   
				 print 'ACCOUNT SETUP QC is updated successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@acct_setup_qc)+'.' 
				 end 
			END   
			
	  if @debug = 1 
      begin       
      Print 'ADJUSTMENT QC 100% Processing' 
      end
      IF(ISNULL(@adj_qc_100,0)<>-1 AND (SELECT COUNT(*) FROM [CUSTMR_PERS_REL] where custmr_id=@acct_id and rol_id=360)=0)      
		   BEGIN      
				INSERT INTO [CUSTMR_PERS_REL](custmr_id,pers_id,rol_id,crte_dt,crte_user_id)      
				SELECT @acct_id,@adj_qc_100,360,GETDATE(),@intUserID   
				 if @debug = 1 
				 begin   
				 print 'ADJUSTMENT QC 100% is created successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@adj_qc_100)+'.'  
				 end
     
		   END      
      ELSE IF(ISNULL(@adj_qc_100,0)<>-1)      
            BEGIN      
			  UPDATE [CUSTMR_PERS_REL] 
			  SET pers_id=@adj_qc_100,      
			  [updt_user_id]=@intUserID,
			  [updt_dt]=GETDATE() 
			  WHERE custmr_id=@acct_id and  rol_id=360
			  if @debug = 1 
				 begin   
				 print 'ADJUSTMENT QC 100% is updated successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@adj_qc_100)+'.' 
				 end 
			END  
			
	  if @debug = 1 
      begin       
      Print 'ADJUSTMENT QC 20% Processing' 
      end
      IF(ISNULL(@adj_qc_20,0)<>-1 AND (SELECT COUNT(*) FROM [CUSTMR_PERS_REL] where custmr_id=@acct_id and rol_id=361)=0)      
		   BEGIN    
				INSERT INTO [CUSTMR_PERS_REL](custmr_id,pers_id,rol_id,crte_dt,crte_user_id)      
				SELECT @acct_id,@adj_qc_20,361,GETDATE(),@intUserID   
				 if @debug = 1 
				 begin   
				 print 'ADJUSTMENT QC 20% is created successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@adj_qc_20)+'.'  
				 end
     
		   END      
      ELSE IF(ISNULL(@adj_qc_20,0)<>-1)     
            BEGIN      
			  UPDATE [CUSTMR_PERS_REL] 
			  SET pers_id=@adj_qc_20,      
			  [updt_user_id]=@intUserID,
			  [updt_dt]=GETDATE() 
			  WHERE custmr_id=@acct_id and  rol_id=361
			  if @debug = 1 
				 begin   
				 print 'ADJUSTMENT QC 20% is updated successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@adj_qc_20)+'.' 
				 end 
			END  
        
        
     if @debug = 1 
      begin       
      Print 'ARiES QC Processing' 
      end
      IF(ISNULL(@aries_qc,0)<>-1 AND (SELECT COUNT(*) FROM [CUSTMR_PERS_REL] where custmr_id=@acct_id and rol_id=362)=0)      
		   BEGIN      
				INSERT INTO [CUSTMR_PERS_REL](custmr_id,pers_id,rol_id,crte_dt,crte_user_id)      
				SELECT @acct_id,@aries_qc,362,GETDATE(),@intUserID   
				 if @debug = 1 
				 begin   
				 print 'ARiES QC is created successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@aries_qc)+'.'  
				 end
     
		   END      
      ELSE  IF(ISNULL(@aries_qc,0)<>-1)    
            BEGIN      
			  UPDATE [CUSTMR_PERS_REL] 
			  SET pers_id=@aries_qc,      
			  [updt_user_id]=@intUserID,
			  [updt_dt]=GETDATE() 
			  WHERE custmr_id=@acct_id and  rol_id=362
			  if @debug = 1 
				 begin   
				 print 'ARiES QC is updated successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@aries_qc)+'.' 
				 end 
			END  
    
    
    if @debug = 1 
      begin       
      Print 'C&RM ADMIN ANALYST Processing' 
      end
      IF(ISNULL(@crm_admn_anlst,0)<>-1 AND (SELECT COUNT(*) FROM [CUSTMR_PERS_REL] where custmr_id=@acct_id and rol_id=363)=0)      
		   BEGIN      
				INSERT INTO [CUSTMR_PERS_REL](custmr_id,pers_id,rol_id,crte_dt,crte_user_id)      
				SELECT @acct_id,@crm_admn_anlst,363,GETDATE(),@intUserID   
				 if @debug = 1 
				 begin   
				 print 'C&RM ADMIN ANALYST is created successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@crm_admn_anlst)+'.'  
				 end
     
		   END      
      ELSE IF(ISNULL(@crm_admn_anlst,0)<>-1)     
            BEGIN      
			  UPDATE [CUSTMR_PERS_REL] 
			  SET pers_id=@crm_admn_anlst,      
			  [updt_user_id]=@intUserID,
			  [updt_dt]=GETDATE() 
			  WHERE custmr_id=@acct_id and  rol_id=363
			  if @debug = 1 
				 begin   
				 print 'C&RM ADMIN ANALYST is updated successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@crm_admn_anlst)+'.' 
				 end 
			END  
			
         
         if @debug = 1 
      begin       
      Print 'C&RM COLLECTION SPECIALIST Processing' 
      end
      IF(ISNULL(@crm_col_splst,0)<>-1 AND (SELECT COUNT(*) FROM [CUSTMR_PERS_REL] where custmr_id=@acct_id and rol_id=364)=0)      
		   BEGIN      
				INSERT INTO [CUSTMR_PERS_REL](custmr_id,pers_id,rol_id,crte_dt,crte_user_id)      
				SELECT @acct_id,@crm_col_splst,364,GETDATE(),@intUserID   
				 if @debug = 1 
				 begin   
				 print 'C&RM COLLECTION SPECIALIST is created successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@crm_col_splst)+'.'  
				 end
     
		   END      
      ELSE  IF(ISNULL(@crm_col_splst,0)<>-1)    
            BEGIN      
			  UPDATE [CUSTMR_PERS_REL] 
			  SET pers_id=@crm_col_splst,      
			  [updt_user_id]=@intUserID,
			  [updt_dt]=GETDATE() 
			  WHERE custmr_id=@acct_id and  rol_id=364
			  if @debug = 1 
				 begin   
				 print 'C&RM COLLECTION SPECIALIST is updated successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@crm_col_splst)+'.' 
				 end 
			END  
         
       
        
        
         if @debug = 1 
      begin       
      Print 'CFS1 Processing' 
      end
      IF(ISNULL(@cfs1,0)<>-1 AND (SELECT COUNT(*) FROM [CUSTMR_PERS_REL] where custmr_id=@acct_id and rol_id=365)=0)      
		   BEGIN      
				INSERT INTO [CUSTMR_PERS_REL](custmr_id,pers_id,rol_id,crte_dt,crte_user_id)      
				SELECT @acct_id,@cfs1,365,GETDATE(),@intUserID   
				 if @debug = 1 
				 begin   
				 print 'CFS1 is created successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@cfs1)+'.'  
				 end
     
		   END      
      ELSE IF(ISNULL(@cfs1,0)<>-1)     
            BEGIN      
			  UPDATE [CUSTMR_PERS_REL] 
			  SET pers_id=@cfs1,      
			  [updt_user_id]=@intUserID,
			  [updt_dt]=GETDATE() 
			  WHERE custmr_id=@acct_id and  rol_id=365
			  if @debug = 1 
				 begin   
				 print 'CFS1 is updated successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@cfs1)+'.' 
				 end 
			END  
			
			
			
			 if @debug = 1 
      begin       
      Print 'CFS2 Processing' 
      end
      IF(ISNULL(@cfs2,0)<>-1 AND (SELECT COUNT(*) FROM [CUSTMR_PERS_REL] where custmr_id=@acct_id and rol_id=366)=0)      
		   BEGIN      
				INSERT INTO [CUSTMR_PERS_REL](custmr_id,pers_id,rol_id,crte_dt,crte_user_id)      
				SELECT @acct_id,@cfs2,366,GETDATE(),@intUserID   
				 if @debug = 1 
				 begin   
				 print 'CFS2 is created successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@cfs2)+'.'  
				 end
     
		   END      
      ELSE IF(ISNULL(@cfs2,0)<>-1)     
            BEGIN      
			  UPDATE [CUSTMR_PERS_REL] 
			  SET pers_id=@cfs2,      
			  [updt_user_id]=@intUserID,
			  [updt_dt]=GETDATE() 
			  WHERE custmr_id=@acct_id and  rol_id=366
			  if @debug = 1 
				 begin   
				 print 'CFS2 is updated successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@cfs2)+'.' 
				 end 
			END  
         
         
          if @debug = 1 
      begin       
      Print 'LSS Admin Processing' 
      end
      IF(ISNULL(@lss_admin,0)<>-1 AND (SELECT COUNT(*) FROM [CUSTMR_PERS_REL] where custmr_id=@acct_id and rol_id=367)=0)      
		   BEGIN      
				INSERT INTO [CUSTMR_PERS_REL](custmr_id,pers_id,rol_id,crte_dt,crte_user_id)      
				SELECT @acct_id,@lss_admin,367,GETDATE(),@intUserID   
				 if @debug = 1 
				 begin   
				 print 'LSS Admin is created successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@lss_admin)+'.'  
				 end
     
		   END      
      ELSE IF(ISNULL(@lss_admin,0)<>-1)     
            BEGIN      
			  UPDATE [CUSTMR_PERS_REL] 
			  SET pers_id=@lss_admin,      
			  [updt_user_id]=@intUserID,
			  [updt_dt]=GETDATE() 
			  WHERE custmr_id=@acct_id and  rol_id=367
			  if @debug = 1 
				 begin   
				 print 'LSS Admin is updated successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@lss_admin)+'.' 
				 end 
			END  
			
			
			if @debug = 1 
      begin       
      Print 'RECONCILER Processing' 
      end
      IF(ISNULL(@reconciler,0)<>-1 AND (SELECT COUNT(*) FROM [CUSTMR_PERS_REL] where custmr_id=@acct_id and rol_id=368)=0)      
		   BEGIN      
				INSERT INTO [CUSTMR_PERS_REL](custmr_id,pers_id,rol_id,crte_dt,crte_user_id)      
				SELECT @acct_id,@reconciler,368,GETDATE(),@intUserID   
				 if @debug = 1 
				 begin   
				 print 'RECONCILER is created successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@reconciler)+'.'  
				 end
     
		   END      
      ELSE IF(ISNULL(@reconciler,0)<>-1)     
            BEGIN      
			  UPDATE [CUSTMR_PERS_REL] 
			  SET pers_id=@reconciler,      
			  [updt_user_id]=@intUserID,
			  [updt_dt]=GETDATE() 
			  WHERE custmr_id=@acct_id and  rol_id=368
			  if @debug = 1 
				 begin   
				 print 'RECONCILER is updated successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@reconciler)+'.' 
				 end 
			END  
            
         
    end    
     fetch Mass_Reassignments_basic into @mass_reassign_stage_id,@acct_id,@acct_setup_qc,@adj_qc_100,@adj_qc_20,@aries_qc,@crm_admn_anlst,@crm_col_splst,@cfs1,@cfs2,@lss_admin,@reconciler,@txtUsrNm,@datCreation    
   end --end of cursor Mass_Reassignments_basic / while loop    
  close Mass_Reassignments_basic    
  deallocate Mass_Reassignments_basic    
      
      
  
    
      
      
   if @debug = 1    
   begin    
    print 'Truncating the stage table MASS_REASSIGN_STAGE'    
   end    
      
 
  DELETE FROM [dbo].[MASS_REASSIGN_STAGE] 
  WHERE DATEDIFF(DAY, crte_dt, GETDATE()) > 2    
         
      
  --print '@trancount: ' + convert(varchar(30),@trancount)    
  if @trancount = 0    
   commit transaction    
    
end try    
begin catch    
    
 if @trancount = 0    
 begin    
  rollback transaction    
 end    
 else    
 begin    
  rollback transaction sp_Process_Mass_Reassignments    
 end    
    
     
 declare @err_msg varchar(500),@err_ln varchar(10),    
   @err_proc varchar(30),@err_no varchar(10)    
 select  @err_msg = error_message(),    
      @err_no = error_number(),    
      @err_proc = error_procedure(),    
   @err_ln = error_line()    
 set @err_msg = '- error no.:' + isnull(@err_no, ' ') + '; procedure:'     
  + isnull(@err_proc, ' ') + ';error line:' + isnull(@err_ln, ' ') + ';description:' + isnull(@err_msg, ' ' )    
 set @err_msg_op = @err_msg    
    
     
    
 select     
    error_number() AS ErrorNumber,    
    error_severity() AS ErrorSeverity,    
    error_state() as ErrorState,    
    error_procedure() as ErrorProcedure,    
    error_line() as ErrorLine,    
    error_message() as ErrorMessage    
    
 declare @err_sev varchar(10)    
    
 select  @err_msg = error_message(),    
      @err_no = error_number(),    
      @err_proc = error_procedure(),    
   @err_ln = error_line(),    
   @err_sev = error_severity()    
    
 RAISERROR (@err_msg, -- Message text.    
               @err_sev, -- Severity.    
               1 -- State.    
               )    
end catch    
    
    
end    

GO

IF OBJECT_ID('ModAIS_Process_Mass_Reassignments') IS NOT NULL
	PRINT 'CREATED PROCEDURE ModAIS_Process_Mass_Reassignments'
ELSE
	PRINT 'FAILED CREATING PROCEDURE ModAIS_Process_Mass_Reassignments'
GO

IF OBJECT_ID('ModAIS_Process_Mass_Reassignments') IS NOT NULL
	GRANT EXEC ON ModAIS_Process_Mass_Reassignments TO PUBLIC
GO
