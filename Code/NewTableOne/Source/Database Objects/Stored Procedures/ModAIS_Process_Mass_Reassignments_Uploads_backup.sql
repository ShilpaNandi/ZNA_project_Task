USE [AIS]
GO
/****** Object:  StoredProcedure [dbo].[ModAIS_Process_Mass_Reassignments_Uploads]    Script Date: 07/10/2014 17:39:26 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------            
-----            
----- PROC NAME:  ModAIS_Process_Mass_Reassignments_Uploads            
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
       
          
ALTER PROCEDURE [dbo].[ModAIS_Process_Mass_Reassignments_Uploads]               
@create_user_id VARCHAR(500),              
@dtUploadDateTime DATETIME,   
@AssignType int,           
@err_msg_op VARCHAR(1000) OUTPUT,              
@debug BIT = 0              
as                             
BEGIN              
    SET NOCOUNT ON              
              
 DECLARE @multiple_user_upload_stage_id INT,          
 @acct_id VARCHAR(500),             
 @bp_number VARCHAR(500),              
 @rol_id INT,              
 @user_nm VARCHAR(500),              
 @crte_usr_id VARCHAR(500),              
 @crte_dt DATETIME,              
 @Validate bit,   
 @intUserID int,    
 @intPersID int,    
 @intcustmr_id int,  
 @finc_pty_id varchar(MAX),  
 @validation_message VARCHAR(MAX),  
 @validation_message_blank VARCHAR(MAX), 
 @validation_message_user VARCHAR(MAX),
 @validation_message_Exist VARCHAR(MAX),          
 @txtErrorDesc_OUT VARCHAR(max),           
 @txtStatusMessage VARCHAR(Max),              
 @err_message varchar(500),        
 @trancount INT,  
 @I INT   
           
 if @debug = 1              
 begin              
 print 'Upload: Mass Reassignments Processing started'              
 end              
              
 set @trancount = @@trancount              
 --print @trancount              
 if @trancount >= 1              
  save transaction ModAIS_Process_Mass_Reassign           
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
 SELECT [multiple_user_upload_stage_id]  
      ,[acct_id]  
      ,[bp_number]  
      ,[rol_id]  
      ,[user_nm]  
      ,[crte_usr_id]  
      ,[crte_dt]  
      ,[validate]  
 FROM [dbo].[MASS_REASSIGN_UPLOAD_STAGE]  
 WHERE crte_usr_id=@create_user_id              
 AND crte_dt=@dtUploadDateTime          
                 
               
 open Mass_Reassignments_basic              
 fetch Mass_Reassignments_basic into @multiple_user_upload_stage_id,@acct_id,@bp_number,@rol_id,@user_nm,@crte_usr_id,@crte_dt,@Validate  
           
 --set @prem_adj_pgm_setup_id_tracker = 0              
 set @validation_message=''              
           
 while @@Fetch_Status = 0              
 begin              
  begin              
   if @debug = 1              
   begin          
            
     print'*******************Uploads: START OF ITERATION*********'               
     print'---------------Input Params-------------------'               
     print' @multiple_user_upload_stage_id:- ' + convert(varchar(20), @multiple_user_upload_stage_id)          
     print' @acct_id:- ' + convert(varchar(20), @acct_id)                  
     print' @bp_number:- ' + convert(varchar(50), @bp_number)                
     print' @rol_id: ' + convert(varchar(50), @rol_id)                
     print' @user_nm: ' + convert(varchar(50), @user_nm)              
     print' @crte_usr_id: ' + convert(varchar(20), @crte_usr_id )                
     print' @crte_dt: ' + convert(varchar(20), @crte_dt )               
     print' @Validate: ' + convert(varchar(50), @Validate ) 
     print' @AssignType: ' + convert(varchar(50), @AssignType )               
              
   end              
                
   SET @validation_message=''  
   SET @validation_message_blank=''            
                
   -----------------------------------------------------------------------------------------------------          
   --Validation Rule 1:          
   --Both Account id and BP Number can not be Blank.  
   -----------------------------------------------------------------------------------------------------          
   --Dheeraj   
   
    if(@AssignType=1 AND ISNULL(@acct_id,'')='') 
		SET @validation_message='Account Id' 
		 
   else if (@AssignType=2 AND ISNULL(@bp_number,'')='')              
		SET @validation_message='BP Number'        
                       
   if(@validation_message<>'')              
   begin --Blank data check : Failed              
    --SET @validation_message='Errors: '+CHAR(13)+CHAR(10)+@validation_message+' '+'fields can not be blank.'+CHAR(13)+CHAR(10)+'Please provide either Account id or BP Number.'              
    SET @validation_message_blank= CHAR(13)+CHAR(10)+@validation_message+' '+'is missing.'
		if @debug = 1              
		begin              
		 print 'Blank Data Validation Failed :'              
		 print' @validation_message_blank: ' + convert(varchar(MAX), @validation_message_blank)               
		end              
    end --Blank data check : Failed      
    ---------------------------------------------------------------------------------------          
    --Validation Rule 2:          
    --The Given user_nm is valid  
    ---------------------------------------------------------------------------------------          
        
    --Variable initialization              
    SET @validation_message='' 
    SET @validation_message_user=''             
    SET @intPersID=NULL           
         
    --Retrieving the @@intPersID based on @user_nm from the PERS table             
      
	 SELECT @intPersID=MAX(pers_id)  
	 FROM PERS   
	 WHERE forename+' '+surname =@user_nm      
        
        
    if(ISNULL(@intPersID,'')='')              
    SET @validation_message='User Name'              
        
                   
    if(@validation_message<>'')              
    begin --Un macthed data check : Failed              
     --SET @validation_message='Errors: '+CHAR(13)+CHAR(10)+ @validation_message+' '+'field is not having the valid Lookup data in PERS table or having blank value.'            
     
     If(ISNULL(@user_nm,'')='')
		SET @validation_message_user=@validation_message+' '+'is missing.'
     else     
       SET @validation_message_user=@validation_message+' '+'doesn''t exist.'
               
    if @debug = 1              
    begin              
     print 'User Name macthed data Validation Failed :'              
     print' @validation_message_user: ' + convert(varchar(MAX), @validation_message_user)               
    end    
    end --Un macthed data check : Failed         
    ---------------------------------------------------------------------------------------          
    --Validation Rule 3:          
    -- • The Given Account id is valid     
    ---------------------------------------------------------------------------------------          
               
    --Variable initialization              
    SET @validation_message=''
    SET @validation_message_Exist=''              
    SET @intcustmr_id=NULL   
    SET @finc_pty_id=NULL            
               
    --Retrieving the @intcustmr_id based on @acct_id from the Custmr table             
    SELECT @intcustmr_id=custmr_id           
    FROM CUSTMR           
    WHERE custmr_id=@acct_id     
      
    SELECT @finc_pty_id=finc_pty_id           
    FROM CUSTMR           
    WHERE finc_pty_id=@bp_number        
   
    --Dheeraj
    
      if(@AssignType=1 AND ISNULL(@intcustmr_id,'')='' AND @validation_message_blank='')
		SET @validation_message=CASE WHEN @validation_message='' THEN 'Custmer ID' ELSE @validation_message+',=Custmer ID' END 
	  else if(@AssignType=2 AND ISNULL(@finc_pty_id,'')='' AND @validation_message_blank='')
		SET @validation_message=CASE WHEN @validation_message='' THEN 'BP Number' ELSE @validation_message+',=BP Number' END              
             
               
    if(@validation_message<>'')              
    begin --Account Setup check : Failed              
    --SET @validation_message=CHAR(13)+CHAR(10)+ @validation_message+' '+'fields are not having the valid Lookup data in CUSTMR table.'   
    SET @validation_message_Exist= @validation_message+' '+'doesn''t exist.'   
    begin              
    print 'Account id macthed data Validation Failed :'            
    print' @validation_message_Exist: ' + convert(varchar(MAX), @validation_message_Exist)               
    end   
    end --Account Setup check : Failed   
    
    
    IF(@validation_message_blank<>'' OR @validation_message_user<>'' OR @validation_message_Exist<>'')
    BEGIN  --Validation Status Log
       --Add the logic for inserting the record into the error log              
    INSERT INTO [dbo].[MASS_REASSIGN_UPLOAD_STAGE_StatusLog]  
           (  
            [acct_id]  
           ,[bp_number]  
           ,[rol_id]  
           ,[user_nm]  
           ,[crte_usr_id]  
           ,[crte_dt]  
           ,[validate]  
           ,[txtStatus]  
           ,[txtErrorDesc])  
     VALUES  
           (  
           @acct_id,  
           @bp_number,  
           @rol_id,  
           @user_nm,  
           @crte_usr_id,  
           @crte_dt,  
           @Validate,  
           'Error',  
           @validation_message_blank +' '+@validation_message_user+' '+@validation_message_Exist  
             
           )                  
                    
    ----Skip this record as it is having blank or NULL values              
    fetch Mass_Reassignments_basic into @multiple_user_upload_stage_id,@acct_id,@bp_number,@rol_id,@user_nm,@crte_usr_id,@crte_dt,@Validate  
    continue              
                      
   END --Validation Status Log   
   --else               
   -- begin --Account Setup check  : Success              
    --Add the logic for further processing              
     select @intUserID=pers_id from PERS where external_reference=@crte_usr_id   
    ---------------------------------------------------------------------------------------          
          
    ---------------------------------------------------------------------------------------          
        
    --Variable initialization              
    SET @validation_message=''              
        
   IF(@Validate=0 AND ISNULL(@intcustmr_id,'')<>'')  
   BEGIN  
     
	   IF((SELECT COUNT(*) FROM [CUSTMR_PERS_REL] where custmr_id=@acct_id and rol_id=@rol_id)=0)        
		 BEGIN        
				INSERT INTO [CUSTMR_PERS_REL](custmr_id,pers_id,rol_id,crte_dt,crte_user_id)        
				SELECT @acct_id,@intPersID,@rol_id,GETDATE(),ISNULL(@intUserID,9999)     
				 if @debug = 1   
				 begin     
				 print convert(varchar(MAX),@rol_id)+' role is created successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@intPersID)+'.'    
				 end  
	       
		 END        
	  ELSE  
          BEGIN    
			 UPDATE [CUSTMR_PERS_REL]   
			 SET pers_id=@intPersID,        
			 [updt_user_id]=ISNULL(@intUserID,9999),  
			 [updt_dt]=GETDATE()   
			 WHERE custmr_id=@acct_id 
			 and  rol_id=@rol_id  
		 
			 if @debug = 1   
			 begin     
			 print convert(varchar(MAX),@rol_id)+'role is updated successfully for account id '+convert(varchar(MAX),@acct_id)+' with person id '+ convert(varchar(MAX),@intPersID)+'.'   
			 end   
		  END    
     
 END   
   
   
 IF(@Validate=0 AND ISNULL(@finc_pty_id,'')<>'')  
     BEGIN  
       
   DECLARE @CUSTID_FINC_PTY_ID table            
   (            
   ID INT IDENTITY(1,1),   
   CUSTID varchar(max)  
   )    
  
   INSERT INTO @CUSTID_FINC_PTY_ID(CUSTID)   
   SELECT DISTINCT custmr_id   
   FROM CUSTMR   
   WHERE LTRIM(RTRIM(finc_pty_id))= LTRIM(RTRIM(@finc_pty_id))   
  
   DECLARe @CNT int=(select count(*) from @CUSTID_FINC_PTY_ID)            
   SET @I=1      
  
   WHILE (@CNT>=@i)            
    BEGIN     
      
    SET @intcustmr_id=(SELECT CUSTID FROM @CUSTID_FINC_PTY_ID where ID=@I)  
    
    if @debug = 1              
    begin              
     print 'While loop for the BP Number :'     
     print' @I: ' + convert(varchar(MAX), @I)            
     print' @intcustmr_id: ' + convert(varchar(MAX), @intcustmr_id)               
     print' @rol_id: ' + convert(varchar(MAX), @rol_id)  
     print' @intPersID: ' + convert(varchar(MAX), @intPersID)  
    end   
      
      IF((SELECT COUNT(*) FROM [CUSTMR_PERS_REL] where custmr_id=@intcustmr_id and rol_id=@rol_id)=0)        
         BEGIN        
      INSERT INTO [CUSTMR_PERS_REL](custmr_id,pers_id,rol_id,crte_dt,crte_user_id)        
      SELECT @intcustmr_id,@intPersID,@rol_id,GETDATE(),ISNULL(@intUserID,9999)     
       if @debug = 1   
       begin     
       print convert(varchar(MAX),@rol_id)+' role is created successfully for account id '+convert(varchar(MAX),@intcustmr_id)+' with person id '+ convert(varchar(MAX),@intPersID)+'.'    
       end  
         
        END        
      ELSE        
      BEGIN       
       
        UPDATE [CUSTMR_PERS_REL]   
        SET pers_id=@intPersID,        
        [updt_user_id]=ISNULL(@intUserID,9999),  
        [updt_dt]=GETDATE()   
        WHERE custmr_id=@intcustmr_id 
        and  rol_id=@rol_id  
        
        if @debug = 1   
        begin     
        print convert(varchar(MAX),@rol_id)+'role is updated successfully for account id '+convert(varchar(MAX),@intcustmr_id)+' with person id '+ convert(varchar(MAX),@intPersID)+'.'   
        end   
      END    
       
    SET @I=@I+1            
    END     
    
    
     END   
             
    --end--Account Setup check  : Success           
           
        
    end              
    fetch Mass_Reassignments_basic into @multiple_user_upload_stage_id,@acct_id,@bp_number,@rol_id,@user_nm,@crte_usr_id,@crte_dt,@Validate  
    end --end of cursor Mass_Reassignments_basic / while loop             
        
    close Mass_Reassignments_basic              
    deallocate Mass_Reassignments_basic              
        
        
    if @debug = 1              
    begin              
    print 'Truncating the stage table MASS_REASSIGN_UPLOAD_STAGE'              
    end              
        
     DELETE FROM [dbo].[MASS_REASSIGN_UPLOAD_STAGE]   
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
    rollback transaction ModAIS_Process_Mass_Reassign          
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
          
