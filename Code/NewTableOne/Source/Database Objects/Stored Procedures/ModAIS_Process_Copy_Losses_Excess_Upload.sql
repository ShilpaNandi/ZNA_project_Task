if exists (select 1 from sysobjects 
		where name = 'ModAIS_Process_Copy_Losses_Excess_Upload' and type = 'P')
	drop procedure ModAIS_Process_Copy_Losses_Excess_Upload
go

set ansi_nulls off
go

---------------------------------------------------------------------
-----
----- PROC NAME:  ModAIS_Process_Copy_Losses_Excess_Upload
-----
----- VERSION:  SQL SERVER 2008
-----
----- AUTHOR :  CSC
-----
----- DESCRIPTION: To validate and insert/update ARMIS  Excess losses details for copy losses.
----
----- ON EXIT: 
-----   
-----
----- CREATED: 
-----   
----- 
---------------------------------------------------------------------

CREATE PROCEDURE [dbo].[ModAIS_Process_Copy_Losses_Excess_Upload]      
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
   @CLAIMNO      VARCHAR(MAX),      
   @ADDITIONALCLAIMIND    VARCHAR(MAX),      
   @ADDITIONALCLAIM    VARCHAR(MAX),      
   @CLAIMANTNAME     VARCHAR(MAX),      
   @CLAIMSTATUS     VARCHAR(MAX),      
   @COVERAGETRIGGERDATE   VARCHAR(MAX),      
   @LIMIT       VARCHAR(MAX),      
   @TOTALPAIDINDEMNITY    VARCHAR(MAX),      
   @TOTALPAIDEXPENSE    VARCHAR(MAX),      
   @TOTALRESERVEDINDEMNITY   VARCHAR(MAX),      
   @TOTALRESERVEDEXPENSE   VARCHAR(MAX),      
   @NONBILLABLEPAIDINDEMNITY  VARCHAR(MAX),      
   @NONBILLABLEPAIDEXPENSE   VARCHAR(MAX),      
   @NONBILLABLERESERVEDINDEMNITY VARCHAR(MAX),      
   @NONBILLABLERESERVEDEXPENSE  VARCHAR(MAX),      
   @SYSTEMGENERATED    VARCHAR(MAX),       
   @CRTE_USER_ID           VARCHAR(100),      
   @CRTE_DT            DATETIME,      
   @VALIDATE            BIT,      
   @LOSS_INFO_COPY_STAGE_EXEC_ID   INT,      
   @Armis_los_pol_id         INT,      
   @LOSS_INFO_COPY_STAGE_ID INT,      
   @Coml_Agmt_Id     INT,      
   @Count_PolicyNo     INT,      
   @CustmrID      INT,      
   @StateID      INT,      
   @Prem_Adj_Pgm_ID    INT,      
   @Prem_Adj_ID     INT,      
   @ClaimStatusID     INT,      
   @Adj_Status_ID     INT,     
   @Covg_Typ_ID     INT,      
   @Validation_Message       VARCHAR(MAX),      
   @Validation_Message_Valuation_Date    VARCHAR(MAX),      
   @Validation_Message_PolicyNo     VARCHAR(MAX),      
   @Validation_Message_CUSTMR_ID     VARCHAR(MAX),      
   @Validation_Message_PGM_EFF_DT     VARCHAR(MAX),      
   @Validation_Message_PGM_EXP_DT     VARCHAR(MAX),      
   @Validation_Message_State_ID     VARCHAR(MAX),      
   @Validation_Message_POL_EFF_DT     VARCHAR(MAX),      
   @Validation_Message_POL_EXP_DT     VARCHAR(MAX),      
   @Validation_Message_PGMPRD      VARCHAR(MAX),      
   @Validation_Message_COML_AGMT_ID    VARCHAR(MAX),      
   @Validation_Message_PREM_ADJ_ID     VARCHAR(MAX),      
   @Validation_Message_LOSS_INFO_COPY_STAGE_ID  VARCHAR(MAX),      
   @Validation_Message_AdditionalClaimInd      VARCHAR(MAX),      
   @Validation_Message_ClaimStatus           VARCHAR(MAX),      
   @Validation_Message_CoverageTriggerDate   VARCHAR(MAX),      
   @Validation_Message_Limit2      VARCHAR(MAX),      
   @Validation_Message_Total_Paid_Indemnity  VARCHAR(MAX),      
   @Validation_Message_Total_Paid_Expense   VARCHAR(MAX),      
   @Validation_Message_Total_Reserved_Indemnity VARCHAR(MAX),      
   @Validation_Message_Total_Reserved_Expense  VARCHAR(MAX),      
   @Validation_Message_NonBillable_Paid_Indemnity   VARCHAR(MAX),      
   @Validation_Message_NonBillable_Paid_Expense    VARCHAR(MAX),      
   @Validation_Message_NonBillable_Reserved_Indemnity VARCHAR(MAX),      
   @Validation_Message_NonBillable_Reserved_Expense   VARCHAR(MAX),      
   @Validation_Message_System_Generated        VARCHAR(MAX),      
   @trancount  INT,      
   @intUserID  INT,      
   @index   SMALLINT,      
   @index2   SMALLINT,    
   @LOBID      INT,      
   @Validation_Message_LOB_ID     VARCHAR(max)      
      
      IF @debug = 1      
        BEGIN      
            PRINT 'Upload: Loss Info Copy Stage Policy Processing started'      
        END      
      
      SET @trancount = @@trancount      
      
      --print @trancount                    
      IF @trancount >= 1      
        SAVE TRANSACTION Loss_Info_Copy_Stage_Excess      
      ELSE      
        BEGIN TRANSACTION      
      
      BEGIN try      
          IF @debug = 1      
            BEGIN      
                PRINT ' @create_user_id:- ' + CONVERT(VARCHAR(500), @create_user_id)      
       PRINT ' @dtUploadDateTime:- ' + CONVERT(VARCHAR(500), @dtUploadDateTime)      
            END      
      
          DECLARE LOSS_INFO_COPY_STAGE_EXCESS_Basic CURSOR LOCAL FAST_FORWARD READ_ONLY      
          FOR      
            SELECT Valuation_Date,      
     LOB,      
     POLICY_NO,      
     CUSTMR_ID,      
     PGM_EFF_DT,      
     PGM_EXP_DT,      
     PGM_TYPE,      
     [STATE],      
     POL_EFF_DT,      
     POL_EXP_DT,      
     CLM_NBR_TXT,      
     ADDN_CLM_IND,      
     ADDN_CLM_TXT,      
     CLMT_NM,      
     CLM_STS_ID,      
     COVG_TRIGR_DT,      
     LIM2_AMT,      
     LOS_PAID_IDNMTY_AMT,      
     LOS_PAID_EXPS_AMT,      
     LOS_RESRV_IDNMTY_AMT,      
     LOS_RESRV_EXPS_AMT,      
     NON_BILABL_PAID_IDNMTY_AMT,      
     NON_BILABL_PAID_EXPS_AMT,      
     NON_BILABL_RESRVD_IDNMTY_AMT,      
     NON_BILABL_RESRVD_EXPS_AMT,      
     LOS_SYS_GENRT_IND,      
     CRTE_USER_ID,      
     CRTE_DT,      
     VALIDATE,      
     LOSS_INFO_COPY_STAGE_EXEC_ID      
            FROM   [dbo].[LOSS_INFO_COPY_STAGE_EXEC]      
            WHERE  [CRTE_USER_ID] = @create_user_id      
                   AND [CRTE_DT] = @dtUploadDateTime      
      
          OPEN LOSS_INFO_COPY_STAGE_EXCESS_Basic      
      
          FETCH LOSS_INFO_COPY_STAGE_EXCESS_Basic INTO @Valuation_Date, @LOB,@POLICY_NO,@CUSTMR_ID,@PGM_EFF_DT,@PGM_EXP_DT,@PGM_TYPE,@STATE,      
          @POL_EFF_DT,@POL_EXP_DT,@CLAIMNO,@ADDITIONALCLAIMIND,@ADDITIONALCLAIM,@CLAIMANTNAME,@CLAIMSTATUS,@COVERAGETRIGGERDATE,@LIMIT,@TOTALPAIDINDEMNITY,      
          @TOTALPAIDEXPENSE,@TOTALRESERVEDINDEMNITY,@TOTALRESERVEDEXPENSE,@NONBILLABLEPAIDINDEMNITY,@NONBILLABLEPAIDEXPENSE,@NONBILLABLERESERVEDINDEMNITY,      
          @NONBILLABLERESERVEDEXPENSE,@SYSTEMGENERATED,@CRTE_USER_ID,@CRTE_DT,@VALIDATE,@LOSS_INFO_COPY_STAGE_EXEC_ID      
      
          WHILE @@Fetch_Status = 0      
            BEGIN      
                BEGIN      
                    IF @debug = 1      
                      BEGIN      
                          PRINT '******************* Uploads: START OF ITERATION *********'      
                          PRINT '---------------Input Params-------------------'      
                          PRINT ' @Valuation_Date:- ' + @Valuation_Date      
                          PRINT ' @LOB:- ' + @LOB      
                          PRINT ' @POLICY_NO:- ' + @POLICY_NO      
                          PRINT ' @CUSTMR_ID: ' + @CUSTMR_ID      
                          PRINT ' @PGM_EFF_DT: ' + @PGM_EFF_DT      
                          PRINT ' @PGM_EXP_DT: ' + @PGM_EXP_DT      
                          PRINT ' @PGM_TYPE: '  + @PGM_TYPE      
                          PRINT ' @STATE: ' + @STATE      
                          PRINT ' @POL_EFF_DT: ' + @POL_EFF_DT      
                          PRINT ' @POL_EXP_DT:- ' + @POL_EXP_DT      
                          PRINT ' @CLAIMNO:- ' + @CLAIMNO      
                          PRINT ' @ADDITIONALCLAIMIND:- ' + @ADDITIONALCLAIMIND      
    PRINT ' @ADDITIONALCLAIM:- ' + @ADDITIONALCLAIM      
                          PRINT ' @CLAIMANTNAME:- ' + @CLAIMANTNAME      
                          PRINT ' @CLAIMSTATUS:- ' + @CLAIMSTATUS      
                          PRINT ' @COVERAGETRIGGERDATE:- ' + @COVERAGETRIGGERDATE      
                          PRINT ' @LIMIT:- ' + @LIMIT      
                          PRINT ' @TOTALPAIDINDEMNITY:- ' + @TOTALPAIDINDEMNITY      
						  PRINT ' @TOTALPAIDEXPENSE:- ' + @TOTALPAIDEXPENSE      
						  PRINT ' @TOTALRESERVEDINDEMNITY:- ' + @TOTALRESERVEDINDEMNITY      
						  PRINT ' @TOTALRESERVEDEXPENSE:- ' + @TOTALRESERVEDEXPENSE      
						  PRINT ' @NONBILLABLEPAIDINDEMNITY:- ' + @NONBILLABLEPAIDINDEMNITY      
						  PRINT ' @NONBILLABLEPAIDEXPENSE:- ' + @NONBILLABLEPAIDEXPENSE      
						  PRINT ' @NONBILLABLERESERVEDINDEMNITY:- ' + @NONBILLABLERESERVEDINDEMNITY      
						  PRINT ' @NONBILLABLERESERVEDEXPENSE:- ' + @NONBILLABLERESERVEDEXPENSE      
						  PRINT ' @SYSTEMGENERATED:- ' + @SYSTEMGENERATED      
                          PRINT ' @CRTE_USER_ID: ' + CONVERT(VARCHAR(50), @CRTE_USER_ID)      
						  PRINT ' @CRTE_DT: ' + CONVERT(VARCHAR(50), @CRTE_DT)      
                          PRINT ' @VALIDATE: ' + CONVERT(VARCHAR(50), @VALIDATE)      
                          PRINT ' @LOSS_INFO_COPY_STAGE_EXEC_ID: ' + CONVERT(VARCHAR(50), @LOSS_INFO_COPY_STAGE_EXEC_ID)      
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
					SET @Validation_Message_LOSS_INFO_COPY_STAGE_ID = NULL      
					SET @Validation_Message_AdditionalClaimInd = NULL      
					SET @Validation_Message_ClaimStatus = NULL      
					SET @Validation_Message_CoverageTriggerDate = NULL      
					SET @Validation_Message_Limit2 = NULL      
					SET @Validation_Message_Total_Paid_Indemnity = NULL      
					SET @Validation_Message_Total_Paid_Expense = NULL      
					SET @Validation_Message_Total_Reserved_Indemnity = NULL      
					SET @Validation_Message_Total_Reserved_Expense = NULL      
					SET @Validation_Message_NonBillable_Paid_Indemnity = NULL      
					SET @Validation_Message_NonBillable_Paid_Expense = NULL      
					SET @Validation_Message_NonBillable_Reserved_Indemnity = NULL      
					SET @Validation_Message_NonBillable_Reserved_Expense = NULL      
					SET @Validation_Message_System_Generated = NULL      
					SET @Validation_Message_LOB_ID=NULL    
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 1:                
                    --Valuation Date should not be blank      
                    -----------------------------------------------------------------------------------------------------                 
                    IF(ISNULL(@Valuation_Date, '') = '' OR ISDATE(@Valuation_Date) = 0)      
                      SET @Validation_Message = 'Valuation Date'      
      
                    IF( @Validation_Message <> '' )      
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
        PRINT @Validation_Message + ' Format Validation Failed :'      
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
        PRINT @Validation_Message + ' Blank Data Validation Failed :'      
        PRINT '@Validation_Message_PolicyNo: ' + CONVERT(VARCHAR(max),@Validation_Message_PolicyNo)      
         END      
        END      
      ELSE      
        BEGIN         
       SET @Validation_Message_PolicyNo = @Validation_Message + ' doesn''t exist.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT @Validation_Message + ' Matched Validation Failed :'      
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
        PRINT @Validation_Message + ' Blank Data Validation Failed :'      
        PRINT '@Validation_Message_CUSTMR_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_CUSTMR_ID)      
         END      
        END      
      ELSE IF(ISNUMERIC(@CUSTMR_ID) = 0)        
        BEGIN      
          SET @Validation_Message_CUSTMR_ID = @Validation_Message + ' is not a valid format.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT @Validation_Message + ' Format Validation Failed :'      
        PRINT '@Validation_Message_CUSTMR_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_CUSTMR_ID)      
         END      
        END      
      ELSE      
        BEGIN         
       SET @Validation_Message_CUSTMR_ID = @Validation_Message + ' doesn''t exist.'      
      
       IF @debug = 1      
         BEGIN      
        PRINT @Validation_Message + ' Matched Validation Failed :'      
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
              
                    SELECT @StateID = lkup_id FROM LKUP WHERE attr_1_txt = @State AND lkup_typ_id = 1      
                          
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
            
      SELECT @Prem_Adj_ID = prem_adj_id, @Adj_Status_ID = adj_sts_typ_id FROM PREM_ADJ       
      WHERE reg_custmr_id = @CustmrID 
	  AND valn_dt = CONVERT(DATETIME,@Valuation_Date,101) 
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
                            
                    -----------------------------------------------------------------------------------------------------               
                    --Validation Rule 12:                
                    -- A valid Loss Policy should exist with given data      
                    -----------------------------------------------------------------------------------------------------       
                    --Variable initialization                    
                    SET @Validation_Message = ''      
                    SET @Armis_los_pol_id = NULL      
                    SET @LOSS_INFO_COPY_STAGE_ID = NULL      
           
     IF(ISDATE(@Valuation_Date) = 1)      
     BEGIN           
      SELECT @Armis_los_pol_id = armis_los_pol_id FROM ARMIS_LOS_POL WHERE coml_agmt_id = @Coml_Agmt_Id AND prem_adj_pgm_id = @Prem_Adj_Pgm_ID      
      AND custmr_id = @CustmrID AND st_id = @StateID AND valn_dt = CONVERT(DATETIME,@Valuation_Date,101) AND isnull(prem_adj_id,0) = isnull(@Prem_Adj_ID,0)      
     END      
           
     IF(@Armis_los_pol_id IS NULL OR @Armis_los_pol_id <= 0)      
     BEGIN      
           
     SELECT @LOSS_INFO_COPY_STAGE_ID= LOSS_INFO_COPY_STAGE_ID FROM LOSS_INFO_COPY_STAGE_POL WHERE Valuation_Date=@Valuation_Date AND LOB=@LOB AND POLICY_NO=@POLICY_NO AND       
     CUSTMR_ID=@CUSTMR_ID AND PGM_EFF_DT=@PGM_EFF_DT AND PGM_EXP_DT=@PGM_EXP_DT AND PGM_TYPE=@PGM_TYPE AND [STATE]=@STATE      
     AND POL_EFF_DT=@POL_EFF_DT AND POL_EXP_DT=@POL_EXP_DT AND       
     NOT EXISTS (SELECT TOP 1 1 FROM LOSS_INFO_COPY_STAGE_POL_STATUSLOG WHERE Valuation_Date=@Valuation_Date AND LOB=@LOB AND POLICY_NO=@POLICY_NO AND       
     CUSTMR_ID=@CUSTMR_ID AND PGM_EFF_DT=@PGM_EFF_DT AND PGM_EXP_DT=@PGM_EXP_DT AND PGM_TYPE=@PGM_TYPE AND [STATE]=@STATE      
     AND POL_EFF_DT=@POL_EFF_DT AND POL_EXP_DT=@POL_EXP_DT AND CRTE_DT=@dtUploadDateTime)      
           
     END      
                                    
                    IF((@Armis_los_pol_id IS NULL OR @Armis_los_pol_id <= 0) AND (@LOSS_INFO_COPY_STAGE_ID IS NULL OR @LOSS_INFO_COPY_STAGE_ID <= 0))      
                      SET @Validation_Message = 'Loss Policy'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_LOSS_INFO_COPY_STAGE_ID = @Validation_Message + ' doesn''t exist.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Matched Validation Failed :'      
       PRINT '@Validation_Message_LOSS_INFO_COPY_STAGE_ID: ' + CONVERT(VARCHAR(max),@Validation_Message_LOSS_INFO_COPY_STAGE_ID)      
        END      
                      END                          
                          
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 13:                
                    -- • The Given Additional Claim Ind is valid           
                    ---------------------------------------------------------------------------------------     
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(NOT(LOWER(@ADDITIONALCLAIMIND) = 'true' OR LOWER(@ADDITIONALCLAIMIND) = 'false' OR LOWER(@ADDITIONALCLAIMIND) = 'yes' OR      
        LOWER(@ADDITIONALCLAIMIND) = 'no' OR @ADDITIONALCLAIMIND = '1' OR @ADDITIONALCLAIMIND = '0'))      
      SET @Validation_Message = 'Additional Claim Ind'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_AdditionalClaimInd = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Additional Claim Ind Format Validation Failed :'      
       PRINT '@Validation_Message_AdditionalClaimInd: ' + CONVERT(VARCHAR(max),@Validation_Message_AdditionalClaimInd)      
        END      
                      END         
                           
                    -----------------------------------------------------------------------------------------------------                
                    --Validation Rule 14:                
                    -- A valid Claim Status should exist with given data      
                    -----------------------------------------------------------------------------------------------------       
                    --Variable initialization                                      SET @Validation_Message = ''      
                    SET @ClaimStatusID = NULL      
           
     IF(@CLAIMSTATUS IS NOT NULL)      
     BEGIN           
      SELECT @ClaimStatusID = LKUP.lkup_id FROM LKUP       
      INNER JOIN LKUP_TYP TYP ON LKUP.lkup_typ_id = TYP.lkup_typ_id      
      WHERE TYP.lkup_typ_nm_txt = 'CLAIM STATUS' AND LKUP.lkup_txt = @CLAIMSTATUS      
     END      
                                    
                    IF(ISNULL(@CLAIMSTATUS,'') <> '' AND (@ClaimStatusID IS NULL OR @ClaimStatusID <= 0))      
                      SET @Validation_Message = 'Claim Status'      
      
         IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_ClaimStatus = @Validation_Message + ' doesn''t exist.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Matched Validation Failed :'      
       PRINT '@Validation_Message_ClaimStatus: ' + CONVERT(VARCHAR(max),@Validation_Message_ClaimStatus)      
        END      
                      END       
                
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 15:                
                    -- • The Given Coverage Trigger Date is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(ISNULL(@COVERAGETRIGGERDATE,'') <> '' AND ISDATE(@COVERAGETRIGGERDATE) = 0)      
      SET @Validation_Message = 'Coverage Trigger Date'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_CoverageTriggerDate = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT 'Coverage Trigger Date Format Validation Failed :'      
       PRINT '@Validation_Message_CoverageTriggerDate: ' + CONVERT(VARCHAR(max),@Validation_Message_CoverageTriggerDate)      
        END      
                      END        
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 16:                
                    -- • The Given Limit 2 is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(ISNULL(@LIMIT,'') <> '' AND ISNUMERIC(@LIMIT) = 0)      
      SET @Validation_Message = 'Limit 2'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_Limit2 = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Format Validation Failed :'      
       PRINT '@Validation_Message_Limit2: ' + CONVERT(VARCHAR(max),@Validation_Message_Limit2)      
        END      
                      END       
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 17:                
                    -- • The Given Total Paid Indemnity is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@TOTALPAIDINDEMNITY IS NOT NULL AND @TOTALPAIDINDEMNITY <> '' AND ISNUMERIC(@TOTALPAIDINDEMNITY) = 0)      
      SET @Validation_Message = 'Total Paid Indemnity'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_Total_Paid_Indemnity = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Format Validation Failed :'      
       PRINT '@Validation_Message_Total_Paid_Indemnity: ' + CONVERT(VARCHAR(max),@Validation_Message_Total_Paid_Indemnity)     
        END      
              END         
                           
             ---------------------------------------------------------------------------------------                
                    --Validation Rule 18:                
                    -- • The Given Total Paid Expense is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@TOTALPAIDEXPENSE IS NOT NULL AND @TOTALPAIDEXPENSE <> '' AND ISNUMERIC(@TOTALPAIDEXPENSE) = 0)      
      SET @Validation_Message = 'Total Paid Expense'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_Total_Paid_Expense = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Format Validation Failed :'      
       PRINT '@Validation_Message_Total_Paid_Expense: ' + CONVERT(VARCHAR(max),@Validation_Message_Total_Paid_Expense)      
        END      
                      END        
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 19:                
                    -- • The Given Total Reserved Indemnity is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@TOTALRESERVEDINDEMNITY IS NOT NULL AND @TOTALRESERVEDINDEMNITY <> '' AND ISNUMERIC(@TOTALRESERVEDINDEMNITY) = 0)      
      SET @Validation_Message = 'Total Reserved Indemnity'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_Total_Reserved_Indemnity = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Format Validation Failed :'      
       PRINT '@Validation_Message_Total_Reserved_Indemnity: ' + CONVERT(VARCHAR(max),@Validation_Message_Total_Reserved_Indemnity)      
        END      
                      END       
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 20:                
                    -- • The Given Total Reserved Expense is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@TOTALRESERVEDEXPENSE IS NOT NULL AND @TOTALRESERVEDEXPENSE <> '' AND ISNUMERIC(@TOTALRESERVEDEXPENSE) = 0)      
      SET @Validation_Message = 'Total Reserved Expense'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_Total_Reserved_Expense = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Format Validation Failed :'      
       PRINT '@Validation_Message_Total_Reserved_Expense: ' + CONVERT(VARCHAR(max),@Validation_Message_Total_Reserved_Expense)      
        END      
                      END      
                          
                    ---------------------------------------------------------------------------------------                
        --Validation Rule 21: 
                    -- • The Given NonBillable Paid Indemnity is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@NONBILLABLEPAIDINDEMNITY IS NOT NULL AND @NONBILLABLEPAIDINDEMNITY <> '' AND ISNUMERIC(@NONBILLABLEPAIDINDEMNITY) = 0)      
      SET @Validation_Message = 'NonBillable Paid Indemnity'      
      
                    IF(@Validation_Message <> '')      
        BEGIN      
      SET @Validation_Message_NonBillable_Paid_Indemnity = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Format Validation Failed :'      
       PRINT '@Validation_Message_NonBillable_Paid_Indemnity: ' + CONVERT(VARCHAR(max),@Validation_Message_NonBillable_Paid_Indemnity)      
        END      
                      END         
                           
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 22:                
                    -- • The Given NonBillable Paid Expense is valid           
                    ---------------------------------------------------------------------------------------                
     --Variable initialization         
                    SET @Validation_Message = ''      
           
     IF(@NONBILLABLEPAIDEXPENSE IS NOT NULL AND @NONBILLABLEPAIDEXPENSE <> '' AND ISNUMERIC(@NONBILLABLEPAIDEXPENSE) = 0)      
      SET @Validation_Message = 'NonBillable Paid Expense'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_NonBillable_Paid_Expense = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Format Validation Failed :'      
       PRINT '@Validation_Message_NonBillable_Paid_Expense: ' + CONVERT(VARCHAR(max),@Validation_Message_NonBillable_Paid_Expense)      
        END      
                      END        
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 23:                
                    -- • The Given NonBillable Reserved Indemnity is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@NONBILLABLERESERVEDINDEMNITY IS NOT NULL AND @NONBILLABLERESERVEDINDEMNITY <> '' AND ISNUMERIC(@NONBILLABLERESERVEDINDEMNITY) = 0)      
      SET @Validation_Message = 'NonBillable Reserved Indemnity'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_NonBillable_Reserved_Indemnity = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Format Validation Failed :'      
       PRINT '@Validation_Message_NonBillable_Reserved_Indemnity: ' + CONVERT(VARCHAR(max),@Validation_Message_NonBillable_Reserved_Indemnity)      
        END      
                      END       
                            
                    ---------------------------------------------------------------------------------------                
                    --Validation Rule 24:                
                    -- • The Given NonBillable Reserved Expense is valid   
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(@NONBILLABLERESERVEDEXPENSE IS NOT NULL AND @NONBILLABLERESERVEDEXPENSE <> '' AND ISNUMERIC(@NONBILLABLERESERVEDEXPENSE) = 0)      
      SET @Validation_Message = 'NonBillable Reserved Expense'      
      
                    IF(@Validation_Message <> '')      
                      BEGIN      
      SET @Validation_Message_NonBillable_Reserved_Expense = @Validation_Message + ' is not a valid format.'      
      
      IF @debug = 1      
        BEGIN      
       PRINT @Validation_Message + ' Format Validation Failed :'      
       PRINT '@Validation_Message_NonBillable_Reserved_Expense: ' + CONVERT(VARCHAR(max),@Validation_Message_NonBillable_Reserved_Expense)      
        END      
                      END      
                           
                           
                   
                    ---------------------------------------------------------------------------------------      
                    --Validation Rule 25:                
                    -- • The Given System Generated is valid           
                    ---------------------------------------------------------------------------------------                
                    --Variable initialization                    
                    SET @Validation_Message = ''      
           
     IF(NOT(LOWER(@SYSTEMGENERATED) = 'yes' OR LOWER(@SYSTEMGENERATED) = 'no' OR LOWER(@SYSTEMGENERATED) = 'true' OR LOWER(@SYSTEMGENERATED) = 'false'      
        OR @SYSTEMGENERATED = '1' OR @SYSTEMGENERATED = '0'))      
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
                       ISNULL(@Validation_Message_PREM_ADJ_ID, '') <> '' OR ISNULL(@Validation_Message_LOSS_INFO_COPY_STAGE_ID,'') <> '' OR       
                       ISNULL(@Validation_Message_AdditionalClaimInd, '') <> '' OR ISNULL(@Validation_Message_ClaimStatus, '') <> '' OR      
                       ISNULL(@Validation_Message_CoverageTriggerDate, '') <> '' OR ISNULL(@Validation_Message_Limit2, '') <> '' OR      
                       ISNULL(@Validation_Message_Total_Paid_Indemnity, '') <> '' OR ISNULL(@Validation_Message_Total_Paid_Expense, '') <> '' OR       
                       ISNULL(@Validation_Message_Total_Reserved_Indemnity, '') <> '' OR ISNULL(@Validation_Message_Total_Reserved_Expense, '') <> '' OR       
                       ISNULL(@Validation_Message_NonBillable_Paid_Indemnity, '') <> '' OR ISNULL(@Validation_Message_NonBillable_Paid_Expense, '') <> '' OR       
                       ISNULL(@Validation_Message_NonBillable_Reserved_Indemnity, '') <> '' OR ISNULL(@Validation_Message_NonBillable_Reserved_Expense, '') <> ''       
                       OR ISNULL(@Validation_Message_System_Generated, '') <> '' OR ISNULL(@Validation_Message_LOB_ID, '') <> '')      
                      BEGIN                     
      INSERT INTO [dbo].[LOSS_INFO_COPY_STAGE_EXEC_STATUSLOG]      
      (      
       [Valuation_Date],[LOB],[POLICY_NO],[CUSTMR_ID],[PGM_EFF_DT],[PGM_EXP_DT],[PGM_TYPE],[STATE],[POL_EFF_DT],      
       [POL_EXP_DT],[CLM_NBR_TXT],[ADDN_CLM_IND],[ADDN_CLM_TXT],[CLMT_NM],[CLM_STS_ID],[COVG_TRIGR_DT],[LIM2_AMT],      
       [LOS_PAID_IDNMTY_AMT],[LOS_PAID_EXPS_AMT],[LOS_RESRV_IDNMTY_AMT],[LOS_RESRV_EXPS_AMT],[NON_BILABL_PAID_IDNMTY_AMT],      
       [NON_BILABL_PAID_EXPS_AMT],[NON_BILABL_RESRVD_IDNMTY_AMT],[NON_BILABL_RESRVD_EXPS_AMT],[LOS_SYS_GENRT_IND],      
       [CRTE_USER_ID],[CRTE_DT],[VALIDATE],[TXTSTATUS],[TXTERRORDESC]      
      )      
                        VALUES            
       (      
       @Valuation_Date, @LOB, @POLICY_NO, @CUSTMR_ID, @PGM_EFF_DT, @PGM_EXP_DT, @PGM_TYPE, @STATE, @POL_EFF_DT,       
       @POL_EXP_DT, @CLAIMNO,@ADDITIONALCLAIMIND,@ADDITIONALCLAIM,@CLAIMANTNAME,@CLAIMSTATUS,@COVERAGETRIGGERDATE,@LIMIT,      
       @TOTALPAIDINDEMNITY,@TOTALPAIDEXPENSE,@TOTALRESERVEDINDEMNITY,@TOTALRESERVEDEXPENSE,@NONBILLABLEPAIDINDEMNITY,      
  @NONBILLABLEPAIDEXPENSE,@NONBILLABLERESERVEDINDEMNITY,@NONBILLABLERESERVEDEXPENSE,@SYSTEMGENERATED,@CRTE_USER_ID,      
       @CRTE_DT,@VALIDATE, 'Error',      
       LTRIM(RTRIM(ISNULL(@Validation_Message_Valuation_Date + ' ', '') + ISNULL(@Validation_Message_PolicyNo + ' ','') +       
       ISNULL(@Validation_Message_CUSTMR_ID + ' ','') + ISNULL(@Validation_Message_PGM_EFF_DT + ' ','') +       
       ISNULL(@Validation_Message_PGM_EXP_DT + ' ','') + ISNULL(@Validation_Message_State_ID + ' ','') +       
       ISNULL(@Validation_Message_POL_EFF_DT  + ' ','') + ISNULL(@Validation_Message_POL_EXP_DT + ' ','') +       
       ISNULL(@Validation_Message_PGMPRD + ' ','') + ISNULL(@Validation_Message_COML_AGMT_ID + ' ','') +      
       ISNULL(@Validation_Message_LOB_ID + ' ','') + ISNULL(@Validation_Message_PREM_ADJ_ID  + ' ','') +     
       ISNULL(@Validation_Message_LOSS_INFO_COPY_STAGE_ID + ' ','') + ISNULL(@Validation_Message_AdditionalClaimInd + ' ','') +     
       ISNULL(@Validation_Message_ClaimStatus + ' ','') + ISNULL(@Validation_Message_CoverageTriggerDate + ' ','') +     
       ISNULL(@Validation_Message_Limit2 + ' ','') + ISNULL(@Validation_Message_Total_Paid_Indemnity + ' ','') +     
       ISNULL(@Validation_Message_Total_Paid_Expense + ' ','') + ISNULL(@Validation_Message_Total_Reserved_Indemnity + ' ','') +     
       ISNULL(@Validation_Message_Total_Reserved_Expense + ' ','') + ISNULL(@Validation_Message_NonBillable_Paid_Indemnity + ' ','') +     
       ISNULL(@Validation_Message_NonBillable_Paid_Expense + ' ','') + ISNULL(@Validation_Message_NonBillable_Reserved_Indemnity + ' ','') +     
       ISNULL(@Validation_Message_NonBillable_Reserved_Expense + ' ','') + ISNULL(@Validation_Message_System_Generated,'')))      
                         )      
      
                         ----Skip this record as it is having blank or NULL values                    
                         FETCH LOSS_INFO_COPY_STAGE_EXCESS_Basic INTO @Valuation_Date, @LOB,@POLICY_NO,@CUSTMR_ID,@PGM_EFF_DT,@PGM_EXP_DT,@PGM_TYPE,@STATE,      
       @POL_EFF_DT,@POL_EXP_DT,@CLAIMNO,@ADDITIONALCLAIMIND,@ADDITIONALCLAIM,@CLAIMANTNAME,@CLAIMSTATUS,@COVERAGETRIGGERDATE,@LIMIT,      
       @TOTALPAIDINDEMNITY,@TOTALPAIDEXPENSE,@TOTALRESERVEDINDEMNITY,@TOTALRESERVEDEXPENSE,@NONBILLABLEPAIDINDEMNITY,@NONBILLABLEPAIDEXPENSE,      
       @NONBILLABLERESERVEDINDEMNITY,@NONBILLABLERESERVEDEXPENSE,@SYSTEMGENERATED,@CRTE_USER_ID,@CRTE_DT,@VALIDATE,@LOSS_INFO_COPY_STAGE_EXEC_ID      
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
      
      
                    IF(@Validate = 0 AND @Armis_los_pol_id IS NOT NULL AND @Armis_los_pol_id > 0 AND @Coml_Agmt_Id IS NOT NULL AND @Coml_Agmt_Id > 0      
                    AND @CustmrID IS NOT NULL AND @CustmrID > 0 AND @Prem_Adj_Pgm_ID IS NOT NULL AND @Prem_Adj_Pgm_ID > 0)      
                      BEGIN      
                          IF((SELECT COUNT(1) FROM [ARMIS_LOS_EXC] WHERE armis_los_pol_id = @Armis_los_pol_id AND coml_agmt_id = @Coml_Agmt_Id       
                              AND prem_adj_pgm_id = @Prem_Adj_Pgm_ID AND custmr_id = @CustmrID AND clm_nbr_txt = @CLAIMNO AND clmt_nm = @CLAIMANTNAME) = 0)      
                            BEGIN      
                                IF @debug = 1      
                                  BEGIN      
                        PRINT 'ARMIS_LOS_EXC Insertion:'      
                                 PRINT '@Armis_los_pol_id: ' + CONVERT(VARCHAR(max), @Armis_los_pol_id)      
                                      PRINT '@Coml_Agmt_Id: ' + CONVERT(VARCHAR(max), @Coml_Agmt_Id)      
                                      PRINT '@Prem_Adj_Pgm_ID: ' + CONVERT(VARCHAR(max), @Prem_Adj_Pgm_ID)      
                                      PRINT '@CustmrID: ' + CONVERT(VARCHAR(max), @CustmrID)      
                                      PRINT '@CLAIMNO: ' + CONVERT(VARCHAR(max), @CLAIMNO)      
                                      PRINT '@ADDITIONALCLAIMIND: ' + CONVERT(VARCHAR(max), @ADDITIONALCLAIMIND)      
                                      PRINT '@ADDITIONALCLAIM: ' + CONVERT(VARCHAR(max), @ADDITIONALCLAIM)      
                                      PRINT '@LIMIT: ' + CONVERT(VARCHAR(max), @LIMIT)      
                                      PRINT '@COVERAGETRIGGERDATE: ' + CONVERT(VARCHAR(max), @COVERAGETRIGGERDATE)      
                                      PRINT '@CLAIMANTNAME: ' + CONVERT(VARCHAR(max), @CLAIMANTNAME)      
                                      PRINT '@TOTALPAIDINDEMNITY: ' + CONVERT(VARCHAR(max), @TOTALPAIDINDEMNITY)      
                                      PRINT '@TOTALPAIDEXPENSE: ' + CONVERT(VARCHAR(max), @TOTALPAIDEXPENSE)      
                                      PRINT '@TOTALRESERVEDINDEMNITY: ' + CONVERT(VARCHAR(max), @TOTALRESERVEDINDEMNITY)      
           PRINT '@TOTALRESERVEDEXPENSE: ' + CONVERT(VARCHAR(max), @TOTALRESERVEDEXPENSE)      
                      PRINT '@NONBILLABLEPAIDINDEMNITY: ' + CONVERT(VARCHAR(max), @NONBILLABLEPAIDINDEMNITY)      
                                      PRINT '@NONBILLABLEPAIDEXPENSE: ' + CONVERT(VARCHAR(max), @NONBILLABLEPAIDEXPENSE)      
                                      PRINT '@NONBILLABLERESERVEDINDEMNITY: ' + CONVERT(VARCHAR(max), @NONBILLABLERESERVEDINDEMNITY)      
                                      PRINT '@NONBILLABLERESERVEDEXPENSE: ' + CONVERT(VARCHAR(max), @NONBILLABLERESERVEDEXPENSE)      
                                      PRINT '@SYSTEMGENERATED: ' + CONVERT(VARCHAR(max), @SYSTEMGENERATED)      
                                      PRINT '@ClaimStatusID: ' + CONVERT(VARCHAR(max), @ClaimStatusID)      
                                      PRINT '@intUserID: ' + CONVERT(VARCHAR(max), @intUserID)      
                                  END      
      
        INSERT INTO ARMIS_LOS_EXC      
        (      
         armis_los_pol_id,      
         coml_agmt_id,      
         prem_adj_pgm_id,      
         custmr_id,      
         clm_nbr_txt,      
         addn_clm_ind,      
         addn_clm_txt,      
         lim2_amt,      
         covg_trigr_dt,      
         clmt_nm,      
         paid_idnmty_amt,      
         paid_exps_amt,      
         resrvd_idnmty_amt,      
         resrvd_exps_amt,      
         non_bilabl_paid_idnmty_amt,      
         non_bilabl_paid_exps_amt,      
         non_bilabl_resrvd_idnmty_amt,      
         non_bilabl_resrvd_exps_amt,      
         sys_genrt_ind,      
         clm_sts_id,      
         crte_user_id,      
         crte_dt,      
         actv_ind,  
         copy_ind      
        )      
        VALUES      
        (      
         @Armis_los_pol_id,      
         @Coml_Agmt_Id,      
         @Prem_Adj_Pgm_ID,      
         @CustmrID,      
         @CLAIMNO,      
         CASE WHEN LOWER(@ADDITIONALCLAIMIND) = 'true' OR LOWER(@ADDITIONALCLAIMIND) = 'yes' OR @ADDITIONALCLAIMIND = '1'       
         THEN CAST(1 AS BIT)      
         ELSE CAST(0 AS BIT) END,      
         @ADDITIONALCLAIM,      
         CAST(@LIMIT AS DECIMAL(15,2)),      
         CASE WHEN ISNULL(@COVERAGETRIGGERDATE,'') = '' THEN NULL      
         ELSE CONVERT(DATETIME,@COVERAGETRIGGERDATE,101) END,      
         @CLAIMANTNAME,      
         CASE WHEN ISNULL(@TOTALPAIDINDEMNITY,'') = '' THEN NULL      
         ELSE CAST(@TOTALPAIDINDEMNITY AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@TOTALPAIDEXPENSE,'') = '' THEN NULL      
         ELSE CAST(@TOTALPAIDEXPENSE AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@TOTALRESERVEDINDEMNITY,'') = '' THEN NULL      
         ELSE CAST(@TOTALRESERVEDINDEMNITY AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@TOTALRESERVEDEXPENSE,'') = '' THEN NULL      
         ELSE CAST(@TOTALRESERVEDEXPENSE AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@NONBILLABLEPAIDINDEMNITY,'') = '' THEN NULL      
         ELSE CAST(@NONBILLABLEPAIDINDEMNITY AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@NONBILLABLEPAIDEXPENSE,'') = '' THEN NULL      
         ELSE CAST(@NONBILLABLEPAIDEXPENSE AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@NONBILLABLERESERVEDINDEMNITY,'') = '' THEN NULL      
         ELSE CAST(@NONBILLABLERESERVEDINDEMNITY AS DECIMAL(15,2)) END,      
         CASE WHEN ISNULL(@NONBILLABLERESERVEDEXPENSE,'') = '' THEN NULL      
         ELSE CAST(@NONBILLABLERESERVEDEXPENSE AS DECIMAL(15,2)) END,      
         --CASE WHEN @SYSTEMGENERATED IS NULL THEN NULL       
         --  WHEN LOWER(@SYSTEMGENERATED) = 'true' OR LOWER(@SYSTEMGENERATED) = 'yes' OR @SYSTEMGENERATED = '1'       
         --  THEN CAST(1 AS BIT)      
         --  ELSE CAST(0 AS BIT) END,     
         CAST(0 AS BIT),  
         @ClaimStatusID,      
         ISNULL(@intUserID,9999),      
         GETDATE(),      
         CAST(1 AS BIT),  
         CAST(1 AS BIT)       
        )      
      
                                IF @debug = 1      
                                  BEGIN      
                        PRINT 'Record is created successfully for ARMIS_LOS_EXC table.'      
          END      
                            END      
                          ELSE      
                            BEGIN      
        IF @debug = 1      
          BEGIN      
           PRINT 'ARMIS_LOS_POL Updation:'      
                               PRINT '@Armis_los_pol_id: ' + CONVERT(VARCHAR(max), @Armis_los_pol_id)      
                                      PRINT '@Coml_Agmt_Id: ' + CONVERT(VARCHAR(max), @Coml_Agmt_Id)      
                                  PRINT '@Prem_Adj_Pgm_ID: ' + CONVERT(VARCHAR(max), @Prem_Adj_Pgm_ID)      
                                      PRINT '@CustmrID: ' + CONVERT(VARCHAR(max), @CustmrID)      
                                      PRINT '@CLAIMNO: ' + CONVERT(VARCHAR(max), @CLAIMNO)      
                                      PRINT '@ADDITIONALCLAIMIND: ' + CONVERT(VARCHAR(max), @ADDITIONALCLAIMIND)      
                                      PRINT '@ADDITIONALCLAIM: ' + CONVERT(VARCHAR(max), @ADDITIONALCLAIM)      
                                      PRINT '@LIMIT: ' + CONVERT(VARCHAR(max), @LIMIT)      
                                      PRINT '@COVERAGETRIGGERDATE: ' + CONVERT(VARCHAR(max), @COVERAGETRIGGERDATE)      
                                      PRINT '@CLAIMANTNAME: ' + CONVERT(VARCHAR(max), @CLAIMANTNAME)      
                                      PRINT '@TOTALPAIDINDEMNITY: ' + CONVERT(VARCHAR(max), @TOTALPAIDINDEMNITY)      
                                      PRINT '@TOTALPAIDEXPENSE: ' + CONVERT(VARCHAR(max), @TOTALPAIDEXPENSE)      
                                      PRINT '@TOTALRESERVEDINDEMNITY: ' + CONVERT(VARCHAR(max), @TOTALRESERVEDINDEMNITY)      
                                      PRINT '@TOTALRESERVEDEXPENSE: ' + CONVERT(VARCHAR(max), @TOTALRESERVEDEXPENSE)      
                                      PRINT '@NONBILLABLEPAIDINDEMNITY: ' + CONVERT(VARCHAR(max), @NONBILLABLEPAIDINDEMNITY)      
                                      PRINT '@NONBILLABLEPAIDEXPENSE: ' + CONVERT(VARCHAR(max), @NONBILLABLEPAIDEXPENSE)      
                                      PRINT '@NONBILLABLERESERVEDINDEMNITY: ' + CONVERT(VARCHAR(max), @NONBILLABLERESERVEDINDEMNITY)      
                                      PRINT '@NONBILLABLERESERVEDEXPENSE: ' + CONVERT(VARCHAR(max), @NONBILLABLERESERVEDEXPENSE)      
                                      PRINT '@SYSTEMGENERATED: ' + CONVERT(VARCHAR(max), @SYSTEMGENERATED)      
                                      PRINT '@ClaimStatusID: ' + CONVERT(VARCHAR(max), @ClaimStatusID)      
                                      PRINT '@intUserID: ' + CONVERT(VARCHAR(max), @intUserID)      
          END      
      
        UPDATE [ARMIS_LOS_EXC]       
        SET clm_nbr_txt = @CLAIMNO,      
         addn_clm_ind = CASE WHEN LOWER(@ADDITIONALCLAIMIND) = 'true' OR LOWER(@ADDITIONALCLAIMIND) = 'yes' OR       
         @ADDITIONALCLAIMIND = '1' THEN CAST(1 AS BIT)      
         ELSE CAST(0 AS BIT) END,      
         addn_clm_txt = @ADDITIONALCLAIM,      
         lim2_amt = CAST(@LIMIT AS DECIMAL(15,2)),      
         covg_trigr_dt = CASE WHEN ISNULL(@COVERAGETRIGGERDATE,'') = '' THEN NULL      
         ELSE CONVERT(DATETIME,@COVERAGETRIGGERDATE,101) END,      
         clmt_nm = @CLAIMANTNAME,      
         paid_idnmty_amt = CASE WHEN ISNULL(@TOTALPAIDINDEMNITY,'') = '' THEN NULL      
         ELSE CAST(@TOTALPAIDINDEMNITY AS DECIMAL(15,2)) END,      
         paid_exps_amt = CASE WHEN ISNULL(@TOTALPAIDEXPENSE,'') = '' THEN NULL      
         ELSE CAST(@TOTALPAIDEXPENSE AS DECIMAL(15,2)) END,      
         resrvd_idnmty_amt = CASE WHEN ISNULL(@TOTALRESERVEDINDEMNITY,'') = '' THEN NULL      
         ELSE CAST(@TOTALRESERVEDINDEMNITY AS DECIMAL(15,2)) END,      
         resrvd_exps_amt = CASE WHEN ISNULL(@TOTALRESERVEDEXPENSE,'') = '' THEN NULL      
         ELSE CAST(@TOTALRESERVEDEXPENSE AS DECIMAL(15,2)) END,      
         non_bilabl_paid_idnmty_amt = CASE WHEN ISNULL(@NONBILLABLEPAIDINDEMNITY,'') = '' THEN NULL      
         ELSE CAST(@NONBILLABLEPAIDINDEMNITY AS DECIMAL(15,2)) END,      
         non_bilabl_paid_exps_amt = CASE WHEN ISNULL(@NONBILLABLEPAIDEXPENSE,'') = '' THEN NULL      
         ELSE CAST(@NONBILLABLEPAIDEXPENSE AS DECIMAL(15,2)) END,      
         non_bilabl_resrvd_idnmty_amt = CASE WHEN ISNULL(@NONBILLABLERESERVEDINDEMNITY,'') = '' THEN NULL      
         ELSE CAST(@NONBILLABLERESERVEDINDEMNITY AS DECIMAL(15,2)) END,      
         non_bilabl_resrvd_exps_amt = CASE WHEN ISNULL(@NONBILLABLERESERVEDEXPENSE,'') = '' THEN NULL      
         ELSE CAST(@NONBILLABLERESERVEDEXPENSE AS DECIMAL(15,2)) END,      
         --sys_genrt_ind = CASE WHEN @SYSTEMGENERATED IS NULL THEN NULL       
         --      WHEN LOWER(@SYSTEMGENERATED) = 'true' OR LOWER(@SYSTEMGENERATED) = 'yes' OR @SYSTEMGENERATED = '1'       
         --      THEN CAST(1 AS BIT)      
         --      ELSE CAST(0 AS BIT) END,   
         sys_genrt_ind = CAST(0 AS BIT),     
         clm_sts_id = @ClaimStatusID,      
         updt_user_id = ISNULL(@intUserID,9999),      
         updt_dt = GETDATE(),      
         actv_ind = 1,  
         copy_ind=1      
        WHERE armis_los_pol_id = @Armis_los_pol_id AND coml_agmt_id = @Coml_Agmt_Id AND prem_adj_pgm_id = @Prem_Adj_Pgm_ID       
        AND custmr_id = @CustmrID AND clm_nbr_txt = @CLAIMNO AND clmt_nm = @CLAIMANTNAME     
      
        IF @debug = 1      
          BEGIN      
           PRINT 'Record is updated successfully for ARMIS_LOS_EXC table.'      
          END      
       END      
        
        EXEC ModAISLossLimitExcess @CustmrID,@Prem_Adj_Pgm_ID          
                      END               
                END      
      
                FETCH LOSS_INFO_COPY_STAGE_EXCESS_Basic INTO @Valuation_Date, @LOB,@POLICY_NO,@CUSTMR_ID,@PGM_EFF_DT,@PGM_EXP_DT,@PGM_TYPE,@STATE,      
    @POL_EFF_DT,@POL_EXP_DT,@CLAIMNO,@ADDITIONALCLAIMIND,@ADDITIONALCLAIM,@CLAIMANTNAME,@CLAIMSTATUS,@COVERAGETRIGGERDATE,@LIMIT,@TOTALPAIDINDEMNITY,      
    @TOTALPAIDEXPENSE,@TOTALRESERVEDINDEMNITY,@TOTALRESERVEDEXPENSE,@NONBILLABLEPAIDINDEMNITY,@NONBILLABLEPAIDEXPENSE,@NONBILLABLERESERVEDINDEMNITY,      
    @NONBILLABLERESERVEDEXPENSE,@SYSTEMGENERATED,@CRTE_USER_ID,@CRTE_DT,@VALIDATE,@LOSS_INFO_COPY_STAGE_EXEC_ID      
            END      
          --end of cursor Mass_Reassignments_basic / while loop                   
          CLOSE LOSS_INFO_COPY_STAGE_EXCESS_Basic      
      
          DEALLOCATE LOSS_INFO_COPY_STAGE_EXCESS_Basic      
      
          IF @debug = 1      
            BEGIN      
                PRINT 'Truncating the stage table LOSS_INFO_COPY_STAGE_EXCESS'      
            END      
      
          DELETE FROM [dbo].[LOSS_INFO_COPY_STAGE_EXEC]      
          WHERE  DATEDIFF(DAY, [CRTE_DT], GETDATE()) > 2      
      
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
                ROLLBACK TRANSACTION Loss_Info_Copy_Stage_Excess      
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


if object_id('ModAIS_Process_Copy_Losses_Excess_Upload') is not null
	print 'Created Procedure ModAIS_Process_Copy_Losses_Excess_Upload'
else
	print 'Failed Creating Procedure ModAIS_Process_Copy_Losses_Excess_Upload'
go

if object_id('ModAIS_Process_Copy_Losses_Excess_Upload') is not null
	grant exec on ModAIS_Process_Copy_Losses_Excess_Upload to public
go