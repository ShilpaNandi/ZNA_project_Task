if exists (select 1 from sysobjects 
		where name = 'ModAIS_Process_Copy_Losses_Policy_Upload' and type = 'P')
	drop procedure ModAIS_Process_Copy_Losses_Policy_Upload
go

set ansi_nulls off
go

---------------------------------------------------------------------
-----
----- PROC NAME:  ModAIS_Process_Copy_Losses_Policy_Upload
-----
----- VERSION:  SQL SERVER 2008
-----
----- AUTHOR :  CSC
-----
----- DESCRIPTION: To validate and insert/update ARMIS loss policies details for copy losses.
----
----- ON EXIT: 
-----   
-----
----- CREATED: 
-----   
----- 
---------------------------------------------------------------------

CREATE PROCEDURE [dbo].[ModAIS_Process_Copy_Losses_Policy_Upload]      
@create_user_id   VARCHAR(500),      
@dtUploadDateTime DATETIME,      
@err_msg_op       VARCHAR(1000) OUTPUT,      
@debug            BIT = 0      
AS      
  BEGIN      
      SET NOCOUNT ON      
      
 DECLARE @Valuation_Date     VARCHAR(MAX),      
   @LOB       VARCHAR(MAX),      
   @POLICY_NO      VARCHAR(MAX),      
   @CUSTMR_ID      VARCHAR(MAX),      
   @PGM_EFF_DT      VARCHAR(MAX),      
   @PGM_EXP_DT      VARCHAR(MAX),      
   @PGM_TYPE      VARCHAR(MAX),      
   @STATE       VARCHAR(MAX),      
   @POL_EFF_DT      VARCHAR(MAX),      
   @POL_EXP_DT      VARCHAR(MAX),      
   @SCGID       VARCHAR(MAX),      
   @PAID_IDNMTY_AMT    VARCHAR(MAX),      
   @PAID_EXPS_AMT     VARCHAR(MAX),      
   @RESRV_IDNMTY_AMT    VARCHAR(MAX),      
   @RESRV_EXPS_AMT     VARCHAR(MAX),      
   @SYS_GENRT_IND     VARCHAR(MAX),       
   @CRTE_USER_ID     VARCHAR(100),      
   @CRTE_DT      DATETIME,      
   @VALIDATE      BIT,      
   @LOSS_INFO_COPY_STAGE_ID  INT,      
   @Coml_Agmt_Id     INT,    
   @Covg_Typ_ID     INT,      
   @Count_PolicyNo     INT,      
   @CustmrID      INT,      
   @StateID      INT,      
   @Prem_Adj_Pgm_ID    INT,      
   @Prem_Adj_ID     INT,      
   @Adj_Status_ID     INT,      
   @Validation_Message       VARCHAR(max),      
   @Validation_Message_Valuation_Date    VARCHAR(max),      
   @Validation_Message_PolicyNo     VARCHAR(max),      
   @Validation_Message_CUSTMR_ID     VARCHAR(max),      
   @Validation_Message_PGM_EFF_DT     VARCHAR(max),      
   @Validation_Message_PGM_EXP_DT     VARCHAR(max),      
   @Validation_Message_State_ID     VARCHAR(max),      
   @Validation_Message_POL_EFF_DT     VARCHAR(max),      
   @Validation_Message_POL_EXP_DT     VARCHAR(max),      
   @Validation_Message_PGMPRD      VARCHAR(max),      
   @Validation_Message_COML_AGMT_ID    VARCHAR(max),      
   @Validation_Message_PREM_ADJ_ID     VARCHAR(max),      
   @Validation_Message_Total_Paid_Indemnity  VARCHAR(max),      
   @Validation_Message_Total_Paid_Expense   VARCHAR(max),      
   @Validation_Message_Total_Reserved_Indemnity VARCHAR(max),      
   @Validation_Message_Total_Reserved_Expense  VARCHAR(max),      
   @Validation_Message_System_Generated   VARCHAR(max),      
   @txtErrorDesc_OUT         VARCHAR(max),      
   @txtStatusMessage         VARCHAR(max),      
   @err_message              VARCHAR(500),      
   @trancount                INT,      
   @intUserID      INT,      
   @index       SMALLINT,      
   @index2       SMALLINT,       
   @LOBID      INT,      
   @Validation_Message_LOB_ID     VARCHAR(max)    
      
      IF @debug = 1      
        BEGIN      
            PRINT 'Upload: Loss Info Copy Stage Policy Processing started'      
        END      
      
      SET @trancount = @@trancount      
      
      --print @trancount                    
      IF @trancount >= 1      
        SAVE TRANSACTION Loss_Info_Copy_Stage_Policy      
      ELSE      
        BEGIN TRANSACTION      
      
      BEGIN try      
          IF @debug = 1      
            BEGIN      
                PRINT ' @create_user_id:- ' + CONVERT(VARCHAR(500), @create_user_id)      
                PRINT ' @dtUploadDateTime:- ' + CONVERT(VARCHAR(500), @dtUploadDateTime)      
            END      
      
          DECLARE LOSS_INFO_COPY_STAGE_POL_Basic CURSOR LOCAL FAST_FORWARD READ_ONLY      
          FOR      
            SELECT [Valuation_Date],      
     [LOB],      
     [POLICY_NO],      
     [CUSTMR_ID],      
     [PGM_EFF_DT],      
     [PGM_EXP_DT],      
     [PGM_TYPE],      
     [STATE],      
     [POL_EFF_DT],      
     [POL_EXP_DT],      
     [SCGID],      
     [PAID_IDNMTY_AMT],      
     [PAID_EXPS_AMT],      
     [RESRV_IDNMTY_AMT],      
     [RESRV_EXPS_AMT],      
     [SYS_GENRT_IND],      
     [CRTE_USER_ID],      
     [CRTE_DT],      
     [VALIDATE],      
     [LOSS_INFO_COPY_STAGE_ID]      
            FROM   [dbo].[LOSS_INFO_COPY_STAGE_POL]      
            WHERE  [CRTE_USER_ID] = @create_user_id      
                   AND [CRTE_DT] = @dtUploadDateTime      
      
          OPEN LOSS_INFO_COPY_STAGE_POL_Basic      
      
          FETCH LOSS_INFO_COPY_STAGE_POL_Basic INTO @Valuation_Date, @LOB, @POLICY_NO, @CUSTMR_ID, @PGM_EFF_DT, @PGM_EXP_DT,       
          @PGM_TYPE, @STATE, @POL_EFF_DT, @POL_EXP_DT, @SCGID, @PAID_IDNMTY_AMT, @PAID_EXPS_AMT, @RESRV_IDNMTY_AMT,       
          @RESRV_EXPS_AMT, @SYS_GENRT_IND, @CRTE_USER_ID, @CRTE_DT, @VALIDATE, @LOSS_INFO_COPY_STAGE_ID      
      
          WHILE @@Fetch_Status = 0      
            BEGIN      
                BEGIN      
                    IF @debug = 1      
                      BEGIN      
                          PRINT '******************* Uploads: START OF ITERATION *********'      
                          PRINT '---------------Input Params-------------------'      
                          PRINT ' @Valuation_Date:- ' + @Valuation_Date      
                          PRINT ' @LOB:- ' + @LOB      
                          PRINT ' @POLICY_NO: ' + @POLICY_NO      
                          PRINT ' @CUSTMR_ID: ' + @CUSTMR_ID      
                          PRINT ' @PGM_EFF_DT: ' + @PGM_EFF_DT      
                          PRINT ' @PGM_EXP_DT: ' + @PGM_EXP_DT      
                          PRINT ' @PGM_TYPE: '  + @PGM_TYPE      
                          PRINT ' @STATE: ' + @STATE      
                          PRINT ' @POL_EFF_DT: ' + @POL_EFF_DT      
                          PRINT ' @POL_EXP_DT: ' + @POL_EXP_DT      
                          PRINT ' @SCGID:- ' + @SCGID      
                          PRINT ' @PAID_IDNMTY_AMT: ' + @PAID_IDNMTY_AMT      
                          PRINT ' @PAID_EXPS_AMT: ' + @PAID_EXPS_AMT      
                          PRINT ' @RESRV_IDNMTY_AMT: ' + @RESRV_IDNMTY_AMT      
                          PRINT ' @RESRV_EXPS_AMT: ' + @RESRV_EXPS_AMT      
                          PRINT ' @SYS_GENRT_IND: '  + @SYS_GENRT_IND      
                          PRINT ' @CRTE_USER_ID: ' + CONVERT(VARCHAR(50), @CRTE_USER_ID)      
                          PRINT ' @CRTE_DT: ' + CONVERT(VARCHAR(50), @CRTE_DT)      
                          PRINT ' @VALIDATE: ' + CONVERT(VARCHAR(50), @VALIDATE)      
                          PRINT ' @LOSS_INFO_COPY_STAGE_ID: ' + CONVERT(VARCHAR(50), @LOSS_INFO_COPY_STAGE_ID)      
                      END      
      
                    SET @Validation_Message = ''      
                    SET @Validation_Message_Valuation_Date = NULL      
                    SET @Validation_Message_PolicyNo = NULL      
                    SET @Validation_Message_CUSTMR_ID = NULL      
                    SET @Validation_Message_PGM_EFF_DT = NULL      
                    SET @Validation_Message_PGM_EXP_DT = NULL      
                    SET @Validation_Message_State_ID = NULL      
                    SET @Validation_Message_POL_EFF_DT = NULL      
                    SET @Validation_Message_POL_EXP_DT = NULL      
                    SET @Validation_Message_PGMPRD = NULL      
					SET @Validation_Message_COML_AGMT_ID = NULL      
					SET @Validation_Message_PREM_ADJ_ID = NULL      
					SET @Validation_Message_Total_Paid_Indemnity = NULL      
					SET @Validation_Message_Total_Paid_Expense = NULL      
					SET @Validation_Message_Total_Reserved_Indemnity = NULL      
					SET @Validation_Message_Total_Reserved_Expense = NULL      
					SET @Validation_Message_System_Generated = NULL      
					SET @Validation_Message_LOB_ID=NULL    
      
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 1:                
                    --Valuation Date should not be blank      
                    -----------------------------------------------------------------------------------------------------                 
                    IF(ISNULL(@Valuation_Date, '') = '' OR ISDATE(@Valuation_Date) = 0)      
                      SET @Validation_Message = 'Valuation Date'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      IF(ISNULL(@Valuation_Date, '') = '')      
        BEGIN      
       SET @Validation_Message_Valuation_Date = @Validation_Message + ' is missing.'      
             
       IF @debug = 1      
         BEGIN      
      PRINT 'Valuation Date Blank Data Validation Failed :'      
        PRINT '@Validation_Message_Valuation_Date: ' + CONVERT(VARCHAR(max),@Validation_Message_Valuation_Date)      
         END      
        END        
      ELSE       
        BEGIN      
       SET @Validation_Message_Valuation_Date = @Validation_Message + ' is not a valid format.'      
             
       IF @debug = 1      
         BEGIN      
        PRINT 'Valuation Date Format Validation Failed :'      
        PRINT '@Validation_Message_Valuation_Date: ' + CONVERT(VARCHAR(max),@Validation_Message_Valuation_Date)      
         END      
        END      
                      END            
                          
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 2:                
                    --The given Policy No. should not be blank and should be valid        
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
     SET @Count_PolicyNo = NULL      
     SET @index = 0      
                    SET @index2 = 0      
                          
                    SET @index = CHARINDEX(' ',@POLICY_NO)      
     SET @index2 = CHARINDEX(' ',@POLICY_NO, @index + 1)      
           
     IF(@index > 0 AND @index2 > 0)      
     BEGIN      
      SELECT @Count_PolicyNo = COUNT(coml_agmt_id) FROM COML_AGMT WHERE pol_sym_txt = SUBSTRING (@POLICY_NO, 1 , @index - 1)      
       AND pol_nbr_txt = SUBSTRING (@POLICY_NO, @index + 1 , @index2 - (@index + 1))       
       AND pol_modulus_txt = SUBSTRING (@POLICY_NO, @index2 + 1 , LEN(@POLICY_NO)- @index2)      
     END      
           
     IF(@POLICY_NO IS NULL OR @Count_PolicyNo IS NULL OR @Count_PolicyNo <= 0)      
                      SET @Validation_Message = 'Policy No'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      IF(@POLICY_NO IS NULL)      
        BEGIN      
          SET @Validation_Message_PolicyNo = @Validation_Message + ' is missing.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Policy No Blank Data Validation Failed :'      
        PRINT '@Validation_Message_PolicyNo: ' + CONVERT(VARCHAR(max),@Validation_Message_PolicyNo)      
         END      
        END      
      ELSE      
        BEGIN         
       SET @Validation_Message_PolicyNo = @Validation_Message + ' doesn''t exist.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Policy No Matched Validation Failed :'      
        PRINT '@Validation_Message_PolicyNo: ' + CONVERT(VARCHAR(max),@Validation_Message_PolicyNo)      
         END      
        END      
                      END              
      
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 3:                
                    -- • The Given Customer id is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                    SET @CustmrID = NULL      
           
     IF(ISNUMERIC(@CUSTMR_ID) = 1)      
      SELECT @CustmrID = custmr_id FROM custmr WHERE  custmr_id = @CUSTMR_ID      
                          
                    IF(@CUSTMR_ID IS NULL OR @CustmrID IS NULL OR @CustmrID <= 0)      
                      SET @Validation_Message = 'Customer ID'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      IF(@CUSTMR_ID IS NULL)      
        BEGIN      
          SET @Validation_Message_CUSTMR_ID = @Validation_Message + ' is missing.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Customer ID Blank Data Validation Failed :'      
        PRINT '@Validation_Message_CUSTMR_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_CUSTMR_ID)      
         END      
        END      
      ELSE IF(ISNUMERIC(@CUSTMR_ID) = 0)        
        BEGIN      
          SET @Validation_Message_CUSTMR_ID = @Validation_Message + ' is not a valid format.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Customer ID Format Validation Failed :'      
        PRINT '@Validation_Message_CUSTMR_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_CUSTMR_ID)      
         END      
        END      
      ELSE      
        BEGIN         
       SET @Validation_Message_CUSTMR_ID = @Validation_Message + ' doesn''t exist.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Customer ID Matched Validation Failed :'      
        PRINT '@Validation_Message_CUSTMR_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_CUSTMR_ID)      
         END      
        END      
                      END      
                            
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 4:                
                    --Program Period Effective Date should not be blank      
                    -----------------------------------------------------------------------------------------------------        
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                                   
                    IF(ISNULL(@PGM_EFF_DT, '') = '' OR ISDATE(@PGM_EFF_DT) = 0)      
                      SET @Validation_Message = 'Program Period Eff Date'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      IF(ISNULL(@PGM_EFF_DT, '') = '')      
       BEGIN      
        SET @Validation_Message_PGM_EFF_DT = @Validation_Message + ' is missing.'      
      
        IF @debug = 1      
          BEGIN      
         PRINT 'Program Period Effective Date Blank Data Validation Failed :'      
         PRINT '@Validation_Message_PGM_EFF_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_PGM_EFF_DT)      
          END      
       END      
      ELSE      
       BEGIN      
        SET @Validation_Message_PGM_EFF_DT = @Validation_Message + ' is not a valid format.'      
      
   IF @debug = 1      
          BEGIN     
         PRINT 'Program Period Effective Date format Validation Failed :'      
         PRINT '@Validation_Message_PGM_EFF_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_PGM_EFF_DT)      
          END      
       END      
                      END       
                            
                    -----------------------------------------------------------------------------------------------------                
  --Validation Rule 5:                
                    --Program Period Expiration Date should not be blank      
                    -----------------------------------------------------------------------------------------------------        
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                                   
                    IF(ISNULL(@PGM_EXP_DT, '') = '' OR ISDATE(@PGM_EXP_DT) = 0)      
                      SET @Validation_Message = 'Program Period Exp Date'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN     
      IF(ISNULL(@PGM_EXP_DT, '') = '')      
        BEGIN      
       SET @Validation_Message_PGM_EXP_DT = @Validation_Message + ' is missing.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Program Period Expiration Date Blank Data Validation Failed :'      
        PRINT '@Validation_Message_PGM_EXP_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_PGM_EXP_DT)      
         END      
        END      
      ELSE      
        BEGIN      
       SET @Validation_Message_PGM_EXP_DT = @Validation_Message + ' is not a valid format.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Program Period Expiration Date format Validation Failed :'      
        PRINT '@Validation_Message_PGM_EXP_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_PGM_EXP_DT)      
         END      
        END      
                      END      
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 6:                
                    -- • The Given State id is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                    SET @StateID = NULL      
              
                    SELECT @StateID = LKUP.lkup_id FROM LKUP       
                    INNER JOIN LKUP_TYP TYP ON LKUP.lkup_typ_id = TYP.lkup_typ_id      
     WHERE TYP.lkup_typ_nm_txt = 'STATE' AND LKUP.attr_1_txt = @State      
              
                    IF(@State IS NULL OR @StateID IS NULL OR @StateID <= 0)      
                      SET @Validation_Message = 'State'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      IF(@State IS NULL)      
        BEGIN      
          SET @Validation_Message_State_ID = @Validation_Message + ' is missing.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'State Blank Data Validation Failed :'      
        PRINT '@Validation_Message_State_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_State_ID)      
         END      
        END      
      ELSE      
        BEGIN         
       SET @Validation_Message_State_ID = @Validation_Message + ' doesn''t exist.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'State Matched Validation Failed :'      
        PRINT '@Validation_Message_State_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_State_ID)      
         END      
        END      
                      END      
                          
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 7:                
                    --Policy Effective Date should not be blank      
                    -----------------------------------------------------------------------------------------------------        
                    --Variable initialization                    
                  SET @Validation_Message = ''      
                                   
                    IF(ISNULL(@POL_EFF_DT, '') = '' OR ISDATE(@POL_EFF_DT) = 0)      
                      SET @Validation_Message = 'Policy Eff Date'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
                      IF(ISNULL(@POL_EFF_DT, '') = '')      
        BEGIN      
                        SET @Validation_Message_POL_EFF_DT = @Validation_Message + ' is missing.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT 'Policy Effective Date Blank Data Validation Failed :'      
        PRINT '@Validation_Message_POL_EFF_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_POL_EFF_DT)      
         END      
        END      
     ELSE      
                      BEGIN      
                       SET @Validation_Message_POL_EFF_DT = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Policy Effective Date format Validation Failed :'      
       PRINT '@Validation_Message_POL_EFF_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_POL_EFF_DT)      
        END      
                      END      
      
                      END       
                          
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 8:                
                    --Policy Expiration Date should not be blank      
                    -----------------------------------------------------------------------------------------------------       
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                                    
                    IF(ISNULL(@POL_EXP_DT, '') = '' OR ISDATE(@POL_EFF_DT) = 0)      
                      SET @Validation_Message = 'Policy Exp Date'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
        IF(ISNULL(@POL_EXP_DT, '') = '')      
         BEGIN      
                         SET @Validation_Message_POL_EXP_DT = @Validation_Message + ' is missing.'      
      
        IF @debug = 1      
          BEGIN      
         PRINT 'Policy Expiration Date Blank Data Validation Failed :'      
         PRINT '@Validation_Message_POL_EXP_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_POL_EXP_DT)      
          END      
         END      
        ELSE      
         BEGIN      
                         SET @Validation_Message_POL_EXP_DT = @Validation_Message + ' is not a valid format.'      
      
        IF @debug = 1      
          BEGIN      
         PRINT 'Policy Expiration Date format Validation Failed :'      
         PRINT '@Validation_Message_POL_EXP_DT: ' + CONVERT(VARCHAR(max),@Validation_Message_POL_EXP_DT)      
          END      
         END      
                      END      
                            
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 9:                
                    -- A valid Policy should exist with given data      
                    -----------------------------------------------------------------------------------------------------       
     --Variable initialization                    
                    SET @Validation_Message = ''      
                    SET @Coml_Agmt_Id = NULL      
                    SET @Prem_Adj_Pgm_ID = NULL    
                    SET @Covg_Typ_ID = NULL      
                    SET @index = 0      
                    SET @index2 = 0      
                          
                    SET @index = CHARINDEX(' ',@POLICY_NO)   
     SET @index2 = CHARINDEX(' ',@POLICY_NO, @index + 1)      
            
     IF(@POL_EFF_DT IS NOT NULL AND ISDATE(@POL_EFF_DT) = 1 AND @POL_EXP_DT IS NOT NULL AND ISDATE(@POL_EXP_DT) = 1 AND @index > 0 AND @index2 > 0)      
     BEGIN          
      SELECT @Coml_Agmt_Id = coml_agmt_id, @Prem_Adj_Pgm_ID = prem_adj_pgm_id, @Covg_Typ_ID = covg_typ_id FROM COML_AGMT WHERE pol_sym_txt = SUBSTRING (@POLICY_NO, 1 , @index - 1)      
      AND pol_nbr_txt = SUBSTRING (@POLICY_NO, @index + 1 , @index2 - (@index + 1))       
      AND pol_modulus_txt = SUBSTRING (@POLICY_NO, @index2 + 1 , LEN(@POLICY_NO)- @index2)       
      AND custmr_id = @CustmrID AND pol_eff_dt = CONVERT(DATETIME,@POL_EFF_DT,101) AND planned_end_date = CONVERT(DATETIME,@POL_EXP_DT,101)       
     END      
                                    
                    IF((@POLICY_NO IS NOT NULL AND @Count_PolicyNo IS NOT NULL AND @Count_PolicyNo > 0) AND (@CustmrID IS NOT NULL AND @CustmrID > 0) AND      
                      (@Coml_Agmt_Id IS NULL OR @Coml_Agmt_Id <= 0))      
                      SET @Validation_Message = 'Policy'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_COML_AGMT_ID = @Validation_Message + ' doesn''t exist.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Policy Matched Validation Failed :'      
       PRINT '@Validation_Message_COML_AGMT_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_COML_AGMT_ID)      
        END      
                      END     
                          
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 2_New:                
                    -- • The Given LOB is valid           
                    ---------------------------------------------------------------------------------------                
                --Variable initialization                    
                    SET @Validation_Message = ''      
                    SET @LOBID = NULL      
              
     SELECT @LOBID = LKUP.lkup_id FROM LKUP       
                    INNER JOIN LKUP_TYP TYP ON LKUP.lkup_typ_id = TYP.lkup_typ_id      
     WHERE TYP.lkup_typ_nm_txt = 'LOB COVERAGE' AND LKUP.lkup_txt = @LOB      
              
                    IF(@LOB IS NULL OR @LOBID IS NULL OR @LOBID <= 0 OR @Covg_Typ_ID <> @LOBID)      
                      SET @Validation_Message = 'LOB'      
      
                    IF(@Validation_Message <> '')      
                    BEGIN      
      IF(@LOB IS NULL)      
      BEGIN      
       SET @Validation_Message_LOB_ID = @Validation_Message + ' is missing.'      
    
       IF @debug = 1      
       BEGIN      
        PRINT 'LOB Blank Data Validation Failed :'      
        PRINT '@Validation_Message_LOB_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_LOB_ID)      
       END      
      END      
      ELSE IF(@LOB IS NOT NULL AND (@LOBID IS NULL OR @LOBID <= 0))     
      BEGIN    
       SET @Validation_Message_LOB_ID = @Validation_Message + ' doesn''t exist.'      
    
       IF @debug = 1      
       BEGIN      
        PRINT 'LOB Matched Validation Failed :'      
        PRINT '@Validation_Message_LOB_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_LOB_ID)      
       END      
      END    
      ELSE    
      BEGIN    
       SET @Validation_Message_LOB_ID = @Validation_Message + ' is not assosiated with policy.'      
    
       IF @debug = 1      
       BEGIN      
        PRINT 'LOB Assosiation with Policy Failed :'      
        PRINT '@Validation_Message_LOB_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_LOB_ID)      
       END      
      END    
                    END    
                            
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 10:                
                    -- A valid Program Period should exist with given data      
                    -----------------------------------------------------------------------------------------------------       
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@PGM_EFF_DT IS NOT NULL AND ISDATE(@PGM_EFF_DT) = 1 AND @PGM_EXP_DT IS NOT NULL AND ISDATE(@PGM_EXP_DT) = 1)      
     BEGIN      
      IF NOT EXISTS(SELECT 1 FROM PREM_ADJ_PGM PGM       
      WHERE PGM.strt_dt = CONVERT(DATETIME,@PGM_EFF_DT,101) AND PGM.plan_end_dt = CONVERT(DATETIME,@PGM_EXP_DT,101)       
      AND PGM.custmr_id = @CustmrID AND PGM.actv_ind=1 AND PGM.PREM_ADJ_PGM_ID = @Prem_Adj_Pgm_ID)      
      BEGIN      
       SET @Prem_Adj_Pgm_ID = NULL      
      END      
     END      
                                    
                    IF((@Coml_Agmt_Id IS NOT NULL OR @Coml_Agmt_Id > 0) AND (@Prem_Adj_Pgm_ID IS NULL OR @Prem_Adj_Pgm_ID <= 0))      
                      SET @Validation_Message = 'Program Period'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_PGMPRD = @Validation_Message + ' doesn''t exist.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Program Period Matched Validation Failed :'      
       PRINT '@Validation_Message_PGMPRD: ' + CONVERT(VARCHAR(max),@Validation_Message_PGMPRD)      
        END      
           END      
                            
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 11:                
     -- A valid Adjustment should exist with given data      
                    -----------------------------------------------------------------------------------------------------       
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                    SET @Prem_Adj_ID = NULL      
                    SET @Adj_Status_ID = NULL      
            
     IF(@Valuation_Date IS NOT NULL AND ISDATE(@Valuation_Date) = 1)      
     BEGIN
	            
      SELECT @PREM_ADJ_ID = PREM_ADJ_ID, @ADJ_STATUS_ID = ADJ_STS_TYP_ID FROM PREM_ADJ       
      WHERE REG_CUSTMR_ID = @CUSTMRID 
	  AND VALN_DT = CONVERT(DATETIME,@VALUATION_DATE,101)      
	  AND ADJ_CAN_IND<>1 
	  AND ADJ_VOID_IND<>1
	  AND ADJ_RRSN_IND<>1
	  AND SUBSTRING(FNL_INVC_NBR_TXT,1,3)<>'RTV' 

     END      
                                    
                    IF(@Prem_Adj_ID IS NOT NULL AND @Prem_Adj_ID > 0 AND @Adj_Status_ID <> 346)      
                      SET @Validation_Message = 'Adjustment'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_PREM_ADJ_ID = @Validation_Message + ' not in CALC status.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Adjustment''s Status Type Validation Failed :'      
       PRINT '@Validation_Message_PREM_ADJ_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_PREM_ADJ_ID)      
        END      
                      END      
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 12:                
                    -- • The Given Total Paid Indemnity is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@PAID_IDNMTY_AMT IS NOT NULL AND @PAID_IDNMTY_AMT <> '' AND ISNUMERIC(@PAID_IDNMTY_AMT) = 0)      
      SET @Validation_Message = 'Total Paid Indemnity'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_Total_Paid_Indemnity = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Total Paid Indemnity Format Validation Failed :'      
       PRINT' @Validation_Message_Total_Paid_Indemnity: ' + CONVERT(VARCHAR(max),@Validation_Message_Total_Paid_Indemnity)      
        END      
                      END         
                           
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 13:                
                    -- • The Given Total Paid Expense is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@PAID_EXPS_AMT IS NOT NULL AND @PAID_EXPS_AMT <> '' AND ISNUMERIC(@PAID_EXPS_AMT) = 0)      
      SET @Validation_Message = 'Total Paid Expense'      
      
                    IF(@Validation_Message <> '')      
                    BEGIN      
      SET @Validation_Message_Total_Paid_Expense = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Total Paid Expense Format Validation Failed :'      
       PRINT '@Validation_Message_Total_Paid_Expense: ' + CONVERT(VARCHAR(max),@Validation_Message_Total_Paid_Expense)      
        END      
                      END        
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 14:                
                    -- • The Given Total Reserved Indemnity is valid           
                    ---------------------------------------------------------------------------------------                
        --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@RESRV_IDNMTY_AMT IS NOT NULL AND @RESRV_IDNMTY_AMT <> '' AND ISNUMERIC(@RESRV_IDNMTY_AMT) = 0)      
      SET @Validation_Message = 'Total Reserved Indemnity'      
      
     IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_Total_Reserved_Indemnity = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Total Reserved Indemnity Format Validation Failed :'      
       PRINT '@Validation_Message_Total_Reserved_Indemnity: ' + CONVERT(VARCHAR(max),@Validation_Message_Total_Reserved_Indemnity)      
        END      
                      END       
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 15:      
                    -- • The Given Total Reserved Expense is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@RESRV_EXPS_AMT IS NOT NULL AND @RESRV_EXPS_AMT <> '' AND ISNUMERIC(@RESRV_EXPS_AMT) = 0)      
      SET @Validation_Message = 'Total Reserved Expense'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_Total_Reserved_Expense = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Total Reserved Expense Format Validation Failed :'      
       PRINT '@Validation_Message_Total_Reserved_Expense: ' + CONVERT(VARCHAR(max),@Validation_Message_Total_Reserved_Expense)      
        END      
                      END      
                           
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 16:                
                    -- • The Given System Generated is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(NOT(LOWER(@SYS_GENRT_IND) = 'yes' OR LOWER(@SYS_GENRT_IND) = 'no' OR LOWER(@SYS_GENRT_IND) = 'true' OR LOWER(@SYS_GENRT_IND) = 'false'      
        OR @SYS_GENRT_IND = '1' OR @SYS_GENRT_IND = '0'))      
      SET @Validation_Message = 'System Generated'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_System_Generated = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'System Generated :'      
       PRINT '@Validation_Message_System_Generated: ' + CONVERT(VARCHAR(max),@Validation_Message_System_Generated)      
        END      
                      END      
                            
                    IF(ISNULL(@Validation_Message_Valuation_Date, '') <> '' OR ISNULL(@Validation_Message_PolicyNo, '') <> '' OR      
                       ISNULL(@Validation_Message_CUSTMR_ID, '') <> '' OR ISNULL(@Validation_Message_PGM_EFF_DT, '') <> '' OR      
                       ISNULL(@Validation_Message_PGM_EXP_DT, '') <> '' OR ISNULL(@Validation_Message_State_ID, '') <> '' OR      
                       ISNULL(@Validation_Message_POL_EFF_DT, '') <> '' OR ISNULL(@Validation_Message_POL_EXP_DT, '') <> '' OR      
                       ISNULL(@Validation_Message_PGMPRD, '') <> '' OR ISNULL(@Validation_Message_COML_AGMT_ID, '') <> '' OR      
                       ISNULL(@Validation_Message_PREM_ADJ_ID, '') <> '' OR ISNULL(@Validation_Message_Total_Paid_Indemnity, '') <> '' OR       
                       ISNULL(@Validation_Message_Total_Paid_Expense, '') <> '' OR ISNULL(@Validation_Message_Total_Reserved_Indemnity, '') <> '' OR      
                       ISNULL(@Validation_Message_Total_Reserved_Expense, '') <> '' OR ISNULL(@Validation_Message_System_Generated, '') <> ''    
                       OR ISNULL(@Validation_Message_LOB_ID, '') <> '')      
    BEGIN                     
      INSERT INTO [dbo].[LOSS_INFO_COPY_STAGE_POL_STATUSLOG]      
      (      
       [Valuation_Date], [LOB], [POLICY_NO], [CUSTMR_ID], [PGM_EFF_DT], [PGM_EXP_DT], [PGM_TYPE], [STATE],      
       [POL_EFF_DT], [POL_EXP_DT], [SCGID], [PAID_IDNMTY_AMT], [PAID_EXPS_AMT], [RESRV_IDNMTY_AMT], [RESRV_EXPS_AMT],      
       [SYS_GENRT_IND], [CRTE_USER_ID], [CRTE_DT], [VALIDATE], [TXTSTATUS], [TXTERRORDESC]      
      )      
                        VALUES            
                        (      
       @Valuation_Date, @LOB, @POLICY_NO, @CUSTMR_ID, @PGM_EFF_DT, @PGM_EXP_DT, @PGM_TYPE, @STATE, @POL_EFF_DT,       
       @POL_EXP_DT, @SCGID, @PAID_IDNMTY_AMT, @PAID_EXPS_AMT, @RESRV_IDNMTY_AMT, @RESRV_EXPS_AMT, @SYS_GENRT_IND,       
       @CRTE_USER_ID, @CRTE_DT, @VALIDATE, 'Error',      
       LTRIM(RTRIM(ISNULL(@Validation_Message_Valuation_Date + ' ','') + ISNULL(@Validation_Message_PolicyNo + ' ','') +       
       ISNULL(@Validation_Message_CUSTMR_ID + ' ','') + ISNULL(@Validation_Message_PGM_EFF_DT + ' ','') +       
       ISNULL(@Validation_Message_PGM_EXP_DT + ' ','') + ISNULL(@Validation_Message_State_ID + ' ','') +       
       ISNULL(@Validation_Message_POL_EFF_DT  + ' ','') + ISNULL(@Validation_Message_POL_EXP_DT + ' ','') +       
       ISNULL(@Validation_Message_PGMPRD + ' ','') + ISNULL(@Validation_Message_COML_AGMT_ID + ' ','') +      
       ISNULL(@Validation_Message_LOB_ID + ' ','') + ISNULL(@Validation_Message_PREM_ADJ_ID + ' ','') +     
       ISNULL(@Validation_Message_Total_Paid_Indemnity + ' ','') + ISNULL(@Validation_Message_Total_Paid_Expense + ' ','') +     
       ISNULL(@Validation_Message_Total_Reserved_Indemnity + ' ','') + ISNULL(@Validation_Message_Total_Reserved_Expense + ' ','') +     
       ISNULL(@Validation_Message_System_Generated,'')))      
                         )      
      
                         ----Skip this record as it is having blank or NULL values                    
                         FETCH LOSS_INFO_COPY_STAGE_POL_Basic INTO @Valuation_Date, @LOB, @POLICY_NO, @CUSTMR_ID, @PGM_EFF_DT, @PGM_EXP_DT,       
       @PGM_TYPE, @STATE, @POL_EFF_DT, @POL_EXP_DT, @SCGID, @PAID_IDNMTY_AMT, @PAID_EXPS_AMT, @RESRV_IDNMTY_AMT,       
       @RESRV_EXPS_AMT, @SYS_GENRT_IND, @CRTE_USER_ID, @CRTE_DT, @VALIDATE, @LOSS_INFO_COPY_STAGE_ID      
                          CONTINUE      
                      END       
                 
                    SELECT @intUserID = pers_id FROM pers WHERE external_reference = @CRTE_USER_ID      
      
                    ---------------------------------------------------------------------------------------                
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization      
                    SET @Validation_Message=''      
      
                    IF @debug = 1      
                      BEGIN      
                          PRINT 'Validation Completed successfully:Insert/Update progress'      
                          PRINT '@intUserID: ' + CONVERT(VARCHAR(max), @intUserID)      
                      END      
      
      
                    IF(@Validate = 0 AND @Coml_Agmt_Id IS NOT NULL AND @Coml_Agmt_Id > 0 AND @Prem_Adj_Pgm_ID IS NOT NULL AND @Prem_Adj_Pgm_ID > 0      
                    AND @CustmrID IS NOT NULL AND @CustmrID > 0 AND @StateID IS NOT NULL AND @StateID > 0)      
                      BEGIN      
                          IF((SELECT COUNT(*) FROM [ARMIS_LOS_POL] WHERE coml_agmt_id = @Coml_Agmt_Id AND prem_adj_pgm_id = @Prem_Adj_Pgm_ID AND      
        custmr_id = @CustmrID AND st_id = @StateID AND valn_dt = CONVERT(DATETIME,@Valuation_Date,101)       
        AND isnull(prem_adj_id,0) = isnull(@Prem_Adj_ID,0)) = 0)      
                            BEGIN      
                                IF @debug = 1      
                                  BEGIN      
                                      PRINT 'ARMIS_LOS_POL Insertion:'      
                                      PRINT '@Coml_Agmt_Id: ' + CONVERT(VARCHAR(max), @Coml_Agmt_Id)      
                                      PRINT '@Prem_Adj_Pgm_ID: ' + CONVERT(VARCHAR(max), @Prem_Adj_Pgm_ID)      
                              PRINT '@CustmrID: ' + CONVERT(VARCHAR(max), @CustmrID)      
                                      PRINT '@StateID: ' + CONVERT(VARCHAR(max), @StateID)      
                                      PRINT '@Prem_Adj_ID: ' + CONVERT(VARCHAR(max), @Prem_Adj_ID)      
                                  END      
      
        INSERT INTO [ARMIS_LOS_POL]      
        (      
         coml_agmt_id,      
         prem_adj_pgm_id,      
         custmr_id,      
         st_id,      
         valn_dt,      
         prem_adj_id,      
         suprt_serv_custmr_gp_id,      
         paid_idnmty_amt,      
         paid_exps_amt,      
         resrv_idnmty_amt,      
         resrv_exps_amt,      
         sys_genrt_ind,      
         crte_dt,      
         crte_user_id,      
         actv_ind,  
         copy_ind      
        )      
        VALUES      
        (      
         @Coml_Agmt_Id,      
         @Prem_Adj_Pgm_ID,      
         @CustmrID,      
         @StateID,      
         @Valuation_Date,      
         @Prem_Adj_ID,      
         CAST(@SCGID AS CHAR(10)),      
         CASE WHEN ISNULL(@PAID_IDNMTY_AMT,'') = '' THEN NULL      
         ELSE CAST(@PAID_IDNMTY_AMT AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@PAID_EXPS_AMT,'') = '' THEN NULL      
         ELSE CAST(@PAID_EXPS_AMT AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@RESRV_IDNMTY_AMT,'') = '' THEN NULL      
         ELSE CAST(@RESRV_IDNMTY_AMT AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@RESRV_EXPS_AMT,'') = '' THEN NULL      
         ELSE CAST(@RESRV_EXPS_AMT AS DECIMAL(15,2)) END,      
         --CASE WHEN @SYS_GENRT_IND IS NULL THEN NULL      
         --  WHEN LOWER(@SYS_GENRT_IND) = 'yes' OR LOWER(@SYS_GENRT_IND) = 'true' OR @SYS_GENRT_IND = '1'       
         --  THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END,   
         CAST(0 AS BIT),  
         GETDATE(),      
         ISNULL(@intUserID,9999),      
         CAST(1 AS BIT),  
         CAST(1 AS BIT)       
        )      
      
                                IF @debug = 1      
                                  BEGIN      
                                      PRINT 'Record is created successfully for ARMIS_LOS_POL table.'      
          END      
                            END      
                          ELSE      
                            BEGIN      
        IF @debug = 1      
          BEGIN      
           PRINT 'ARMIS_LOS_POL Updation:'      
                                      PRINT '@Coml_Agmt_Id: ' + CONVERT(VARCHAR(max), @Coml_Agmt_Id)      
  PRINT '@Prem_Adj_Pgm_ID: ' + CONVERT(VARCHAR(max), @Prem_Adj_Pgm_ID)      
                                      PRINT '@CustmrID: ' + CONVERT(VARCHAR(max), @CustmrID)      
                                      PRINT '@StateID: ' + CONVERT(VARCHAR(max), @StateID)      
                                      PRINT '@Prem_Adj_ID: ' + CONVERT(VARCHAR(max), @Prem_Adj_ID)      
          END      
      
        UPDATE [ARMIS_LOS_POL]      
        SET [suprt_serv_custmr_gp_id] = CAST(@SCGID AS CHAR(10)),      
         [paid_idnmty_amt] = CASE WHEN ISNULL(@PAID_IDNMTY_AMT,'') = '' THEN NULL      
                 ELSE CAST(@PAID_IDNMTY_AMT AS DECIMAL(15,2)) END,      
         [paid_exps_amt] = CASE WHEN ISNULL(@PAID_EXPS_AMT,'') = '' THEN NULL      
               ELSE CAST(@PAID_EXPS_AMT AS DECIMAL(15,2)) END,      
         [resrv_idnmty_amt] = CASE WHEN ISNULL(@RESRV_IDNMTY_AMT,'') = '' THEN NULL      
               ELSE CAST(@RESRV_IDNMTY_AMT AS DECIMAL(15,2)) END,      
         [resrv_exps_amt] = CASE WHEN ISNULL(@RESRV_EXPS_AMT,'') = '' THEN NULL      
                ELSE CAST(@RESRV_EXPS_AMT AS DECIMAL(15,2)) END,      
         --[sys_genrt_ind] = CASE WHEN @SYS_GENRT_IND IS NULL THEN NULL      
         --      WHEN LOWER(@SYS_GENRT_IND) = 'yes' OR LOWER(@SYS_GENRT_IND) = 'true' OR @SYS_GENRT_IND = '1'       
         --      THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END,    
         [sys_genrt_ind] = CAST(0 AS BIT),    
            [updt_user_id] = ISNULL(@intUserID, 9999),      
            [updt_dt] = GETDATE(),  
            copy_ind=1      
        WHERE coml_agmt_id = @Coml_Agmt_Id AND prem_adj_pgm_id = @Prem_Adj_Pgm_ID AND custmr_id = @CustmrID AND       
           st_id = @StateID AND isnull(prem_adj_id,0) = isnull(@Prem_Adj_ID,0)      
      
        IF @debug = 1      
          BEGIN      
           PRINT 'Record is updated successfully for ARMIS_LOS_POL table.'      
          END      
       END      
        
        EXEC ModAISLossLimitExcess @CustmrID,@Prem_Adj_Pgm_ID         
                      END               
                END      
      
                FETCH LOSS_INFO_COPY_STAGE_POL_Basic INTO @Valuation_Date, @LOB, @POLICY_NO, @CUSTMR_ID, @PGM_EFF_DT, @PGM_EXP_DT,       
    @PGM_TYPE, @STATE, @POL_EFF_DT, @POL_EXP_DT, @SCGID, @PAID_IDNMTY_AMT, @PAID_EXPS_AMT, @RESRV_IDNMTY_AMT,       
    @RESRV_EXPS_AMT, @SYS_GENRT_IND, @CRTE_USER_ID, @CRTE_DT, @VALIDATE, @LOSS_INFO_COPY_STAGE_ID      
            END      
          --end of cursor Mass_Reassignments_basic / while loop                   
          CLOSE LOSS_INFO_COPY_STAGE_POL_Basic      
      
          DEALLOCATE LOSS_INFO_COPY_STAGE_POL_Basic      
      
          IF @debug = 1      
            BEGIN      
                PRINT 'Truncating the stage table LOSS_INFO_COPY_STAGE_POL'      
            END      
      
          DELETE FROM [dbo].[LOSS_INFO_COPY_STAGE_POL]      
          WHERE  DATEDIFF(DAY, [CRTE_DT], GETDATE()) > 2      
         
    IF @debug = 1      
            BEGIN      
                PRINT '@trancount: ' + CAST(@trancount AS VARCHAR)      
            END      
          
          IF @trancount = 0      
            COMMIT TRANSACTION      
      END try      
      
      BEGIN catch      
          IF @trancount = 0      
            BEGIN      
                ROLLBACK TRANSACTION      
            END      
          ELSE      
            BEGIN      
                ROLLBACK TRANSACTION Loss_Info_Copy_Stage_Policy      
            END      
      
          DECLARE @err_msg  VARCHAR(500),      
                  @err_ln   VARCHAR(10),      
                  @err_proc VARCHAR(30),      
                  @err_no   VARCHAR(10)      
      
          SELECT @err_msg = Error_message(),      
                 @err_no = Error_number(),      
                 @err_proc = Error_procedure(),      
                 @err_ln = Error_line()      
      
          SET @err_msg = '- error no.:' + Isnull(@err_no, ' ')      
                         + '; procedure:' + Isnull(@err_proc, ' ')      
                         + ';error line:' + Isnull(@err_ln, ' ')      
                         + ';description:' + Isnull(@err_msg, ' ' )      
          SET @err_msg_op = @err_msg      
      
          SELECT Error_number()    AS ErrorNumber,      
                 Error_severity()  AS ErrorSeverity,      
                 Error_state()     AS ErrorState,      
                 Error_procedure() AS ErrorProcedure,      
                 Error_line()      AS ErrorLine,      
                 Error_message()   AS ErrorMessage      
      
          DECLARE @err_sev VARCHAR(10)      
      
          SELECT @err_msg = Error_message(),      
                 @err_no = Error_number(),      
                 @err_proc = Error_procedure(),      
                 @err_ln = Error_line(),      
                 @err_sev = Error_severity()      
      
          RAISERROR (@err_msg,-- Message text.                    
                     @err_sev,-- Severity.                    
                     1 -- State.                    
          )      
      END catch      
  END
GO


if object_id('ModAIS_Process_Copy_Losses_Policy_Upload') is not null
	print 'Created Procedure ModAIS_Process_Copy_Losses_Policy_Upload'
else
	print 'Failed Creating Procedure ModAIS_Process_Copy_Losses_Policy_Upload'
go

if object_id('ModAIS_Process_Copy_Losses_Policy_Upload') is not null
	grant exec on ModAIS_Process_Copy_Losses_Policy_Upload to public
go
