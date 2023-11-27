if exists (select 1 from sysobjects 
		where name = 'ModAIS_Process_ReconReview' and type = 'P')
	drop procedure ModAIS_Process_ReconReview
go

set ansi_nulls off
go
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------
-----
-----Proc Name:	ModAIS_Process_ReconReview
-----
-----Version:	SQL Server 2012
-----
-----Description:
-----This stored procedure is used to update the status based on the rules for U/W approval
-----This is created as part of the SR "Remove Reconciler and Underwriter Review in AIS for SR359107"
-----while processing the QC approval process , we need to call the stored procedure
-----Need the check the BU at program period level
-----If any one of the program period is having empire BU , then we need to update the status to  U/W Review and status is "not Required". It should be ready for Finalize
-----If Not Empire BU, Then if the adjustment is one of the adjustment types(poliy level, any one policy)
-----Incurred DEP
-----Incurred Retro
-----Paid DEP
-----Paid Retro
-----Incurred Underlayer
-----Incurred WA 
-----and
-----any one program period BU is LSU BU 
-----then we need to update the status to  U/W Review and status is "not Required". It should be ready for Finalize
-----If it is not part of the above 6 adjustment types, User should complete QC draft and recon. While doing the recon approval we need to check wether any of the Program period belongs to LSU BU
-----If Yes , "then we need to update the status to  U/W Review and status is "not Required". It should be ready for Finalize"
-----If No,  regular process
-----If adjustment type is one of the 6 mentioned and not LSU BU, then regular process.

-----	On Exit:	

-----	12/14/2015  : Venkat K

-----As per the change order we are doing the below changes
-----We need to skip the Recon Review,Based on the initial or Subsequent adjustment 
-----Remove the validation Based on the Zurich Empire BU
-----
-----Modified:	MM/DD/YY	first & last name of modifier
-----
-----Description of Modification
-----
-----
---------------------------------------------------------------------


CREATE PROCEDURE [dbo].[ModAIS_Process_ReconReview] 

@prem_adj_id int,
@create_user_id int,
@create_date_time DATETIME,
@err_msg_output varchar(1000) output,
@debug bit = 0

AS
BEGIN

-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
SET NOCOUNT ON;

declare @trancount int,
		@Reverse int

set @trancount = @@trancount;

if @trancount >= 1

    save transaction ModAIS_Process_ReconReview
else
    begin transaction

begin try

	DECLARE

		@nErrorCode		     int,
		@sErrorMsg		     varchar(100),
		@ID			         int,
		@CUSTMR_ID           int,
		@Is_Initial          int,
		@Is_6_ADJ_TYPS       int,
		@Is_LSU_BU           int,
		@Is_PEO              int,
		@Is_RRR_Adj_Type     int,
		@Is_TPA_Paid_Retro   int,
		@ADJ_STS_ID          int

	-- @Is_RRR_Adj_Type (RRR stands for Recon Review required Adjustment types)(Loss Reimbursement Fund,Loss Fund(Esrow),Paid Retro-TPA,Paid Underlayer, Paid WA,LBA Deposit)
	SET @nErrorCode = 0

	SET @err_msg_output=''

	SELECT @CUSTMR_ID=REG_CUSTMR_ID  FROM PREM_ADJ WHERE PREM_ADJ_ID=@PREM_ADJ_ID

	SELECT @ADJ_STS_ID=ADJ_STS_TYP_ID  FROM PREM_ADJ WHERE PREM_ADJ_ID=@PREM_ADJ_ID

	--IF @IS_Initial>0 Than it is having atleast one of the Program Period with Intial adjustment

	SELECT @Is_Initial=COUNT(*) FROM PREM_ADJ pa
		INNER JOIN PREM_ADJ_PERD pap ON pap.PREM_ADJ_ID=pa.PREM_ADJ_ID
		INNER JOIN PREM_ADJ_PGM pag ON pag.PREM_ADJ_PGM_ID=pap.PREM_ADJ_PGM_ID
		WHERE pa.PREM_ADJ_ID=@PREM_ADJ_ID and pag.prev_valn_dt is null AND pag.prev_valn_dt_non_prem_dt is null

    --IF @IS_6_ADJ_TYPS>0 Than it is having atleast one of the 6 Adjustment types (Incurred DEP or Incurred Retro or Paid DEP or Paid Retro or Incurred Underlayer or Incurred WA (poliy level, any one policy))

    SELECT @Is_6_ADJ_TYPS=COUNT(*) FROM PREM_ADJ pa
			INNER JOIN PREM_ADJ_PERD pap ON pap.PREM_ADJ_ID=pa.PREM_ADJ_ID
			INNER JOIN PREM_ADJ_PGM pag ON pag.PREM_ADJ_PGM_ID=pap.PREM_ADJ_PGM_ID
			INNER JOIN COML_AGMT ca ON ca.PREM_ADJ_PGM_ID=pag.PREM_ADJ_PGM_ID
			WHERE pa.PREM_ADJ_ID=@PREM_ADJ_ID AND ca.ADJ_TYP_ID IN(63,65,70,71,66,67) AND CA.actv_ind=1

	--IF @Is_LSU_BU>0 Than it is having atleast one of the Program Period with LSU BU

	SELECT @Is_LSU_BU=COUNT(*) FROM PREM_ADJ pa
			INNER JOIN PREM_ADJ_PERD pap ON pap.PREM_ADJ_ID=pa.PREM_ADJ_ID
			INNER JOIN PREM_ADJ_PGM pag ON pag.PREM_ADJ_PGM_ID=pap.PREM_ADJ_PGM_ID
			INNER JOIN INT_ORG bu ON pag.BSN_UNT_OFC_ID=bu.int_org_id
			WHERE pa.PREM_ADJ_ID=@PREM_ADJ_ID and (bu.BSN_UNT_CD='LSU' OR bu.BSN_UNT_CD='CML')

	SELECT @Is_PEO=PEO_IND FROM CUSTMR WHERE CUSTMR_ID=@CUSTMR_ID


	SELECT @Is_RRR_Adj_Type=COUNT(*) FROM PREM_ADJ pa
			INNER JOIN PREM_ADJ_PERD pap ON pap.PREM_ADJ_ID=pa.PREM_ADJ_ID
			INNER JOIN PREM_ADJ_PGM pag ON pag.PREM_ADJ_PGM_ID=pap.PREM_ADJ_PGM_ID
			INNER JOIN COML_AGMT ca ON ca.PREM_ADJ_PGM_ID=pag.PREM_ADJ_PGM_ID
			WHERE pa.PREM_ADJ_ID=@PREM_ADJ_ID AND ca.ADJ_TYP_ID IN(68,62,72,73,448) AND CA.actv_ind=1

	SELECT @Is_TPA_Paid_Retro=COUNT(*) FROM PREM_ADJ pa
			INNER JOIN PREM_ADJ_PERD pap ON pap.PREM_ADJ_ID=pa.PREM_ADJ_ID
			INNER JOIN PREM_ADJ_PGM pag ON pag.PREM_ADJ_PGM_ID=pap.PREM_ADJ_PGM_ID
			INNER JOIN COML_AGMT ca ON ca.PREM_ADJ_PGM_ID=pag.PREM_ADJ_PGM_ID
			INNER JOIN CUSTMR c ON ca.custmr_id=c.custmr_id
			WHERE pa.PREM_ADJ_ID=@PREM_ADJ_ID AND ca.ADJ_TYP_ID=71 AND CA.actv_ind=1 AND c.thrd_pty_admin_funded_ind=1

	if @debug = 1              
    
	begin          

     print'---------------Derived Params-------------------'               

     print' @CUSTMR_ID:- ' + convert(varchar(50), @CUSTMR_ID)                  

     print' @ADJ_STS_ID:- ' + convert(varchar(50), @ADJ_STS_ID)                

     print' @Is_Initial: ' + convert(varchar(50), @Is_Initial) 
	 
	 print' @Is_PEO: ' + convert(varchar(50), @Is_PEO)                

     print' @Is_6_ADJ_TYPS: ' + convert(varchar(50), @Is_6_ADJ_TYPS)              

     print' @Is_LSU_BU: ' + convert(varchar(20), @Is_LSU_BU )                

     print' @prem_adj_id: ' + convert(varchar(20), @prem_adj_id )  
	 
	 print' @prem_adj_id: ' + convert(varchar(20), @create_date_time )  
	 
	 print' @Is_RRR_Adj_Type: ' + convert(varchar(50), @Is_RRR_Adj_Type)  
	  
	 print' @Is_TPA_Paid_Retro: ' + convert(varchar(50), @Is_TPA_Paid_Retro)               

   end 

	--NEW LOGIC : CHECK to See if any of the program period(program period level) under the adustment is Initial OR Adjustment is not having one of the 6 Adjustment Types Incurred DEP or Incurred Retro or Paid DEP or Paid Retro or Incurred Underlayer or Incurred WA (poliy level, any one policy)

	-- If it is Initial OR not have one of the Adjustment Types , Update the status to QC-DRAFT by entering the below Loop

    IF((@Is_Initial>0 OR @Is_6_ADJ_TYPS=0 OR @Is_PEO=1 OR @Is_RRR_Adj_Type>0 OR @Is_TPA_Paid_Retro>0) AND @ADJ_STS_ID=348)

	BEGIN  
	--YES(One of the Program Period with Initial Adjustment OR NOT having one of the Adjustment Types )

	--Perform the action for the Initial Adjustment Action OR  NOT having one of the Adjustment Types  (USed the OR because for the two cases we have the same status update)

	--If any one of the program period is having Initial adjustment OR NOT having one of the Adjustment Types, then we need to update the status to QC-DRAFT(350)
	
	Print 'Block1:Status update to QC DRAFT INVOICE(350). This will be done by C# Layer As per the exisisting Process. '

	SET @err_msg_output='QC DRAFT INVOICE'

	END

	ELSE--NO <Adjustment is having ALL the Subsequent Program Periods>  --if the adjustment type for atleast one policy is Incurred DEP or Incurred Retro or Paid DEP or Paid Retro or Incurred Underlayer or Incurred WA (poliy level, any one policy)
		  -- OR any of the program period is having the "LSU" BU

	    IF(
		   (@Is_Initial=0) AND (@Is_6_ADJ_TYPS>0) AND (@Is_LSU_BU>0) AND (@ADJ_STS_ID=348)
		 )
		 
		 BEGIN

		 --Perform the action for the LSU BU
		 --We need to update the status to  U/W Review and status is "not Required". It should be ready for Finalize

		 --STEP 1: Updating the prem_adj table

			UPDATE PREM_ADJ	WITH (ROWLOCK)

			SET 
			adj_sts_typ_id=353,
			adj_sts_eff_dt=@create_date_time,
			undrwrt_not_reqr_ind = 1, 
			adj_qc_ind = 1,
			--reconciler_revw_ind=1,
			UPDT_USER_ID = @CREATE_USER_ID, 
			UPDT_DT = @create_date_time

			WHERE PREM_ADJ_ID = @PREM_ADJ_ID

			--STEP 2: Inserting into prem_adj_sts table for U/W Status

			INSERT INTO DBO.[PREM_ADJ_STS](

						[PREM_ADJ_ID],
						[CUSTMR_ID],
						[ADJ_STS_TYP_ID],
						[EFF_DT],
						[CMMNT_TXT],
						[APRV_IND],
						[CRTE_DT],
						[CRTE_USER_ID])

			   VALUES(
						@PREM_ADJ_ID,
						@CUSTMR_ID,
						353,
						@create_date_time,
						'Auto Approval Process',
						1,
						@create_date_time,
						@CREATE_USER_ID)

			Print 'Block2:Status update to UW REVIEW(353) in this SP ONLY. '

	        SET @err_msg_output='UW REVIEW'

		 END

		 ELSE

		     IF(
		         (@Is_Initial=0) AND (@Is_6_ADJ_TYPS>0) AND (@Is_LSU_BU=0) AND (@ADJ_STS_ID=348)
		      )

			 BEGIN
			 --Perform the action for Non LSU and Non Empire BU's
			 --We need to update the status to  Recon Review . It should be ready for U/W Review

			 --STEP 1: Updating the prem_adj table

				UPDATE PREM_ADJ	WITH (ROWLOCK)

				SET 
				adj_sts_typ_id=351,
				adj_sts_eff_dt=@create_date_time,
				adj_qc_ind = 1,
				--reconciler_revw_ind=1,
				UPDT_USER_ID = @CREATE_USER_ID, 
				UPDT_DT = @create_date_time

				WHERE PREM_ADJ_ID = @PREM_ADJ_ID

			--STEP 2: Inserting into prem_adj_sts table for Recon status

				INSERT INTO DBO.[PREM_ADJ_STS](

							[PREM_ADJ_ID],
							[CUSTMR_ID],
							[QLTY_CNTRL_DT],
							[ADJ_STS_TYP_ID],
							[QLTY_CNTRL_PERS_ID],
							[EFF_DT],
							[CMMNT_TXT],
							[EXPI_DT],
							[APRV_IND],
							[UPDT_DT],
							[UPDT_USER_ID],
							[CRTE_DT],
							[CRTE_USER_ID])

				VALUES(
							@PREM_ADJ_ID,
							@CUSTMR_ID,
							@create_date_time,
							351,
							@CREATE_USER_ID,
							@create_date_time,
							'',
							@create_date_time,
							1,
							@create_date_time,
							@CREATE_USER_ID,
							@create_date_time,
							@CREATE_USER_ID)

				Print 'Block3:Status update to RECON REVIEW(351) in this SP ONLY. '

	            SET @err_msg_output='RECON REVIEW'

			 END

			 ELSE

		     IF(
		       (@Is_LSU_BU=0) AND (@ADJ_STS_ID=350)
		      )

			 BEGIN

			    Print 'Block4:Status update to RECON REVIEW(351). This will be done by C# Layer As per the exisisting Process. '
	            
				SET @err_msg_output='RECON REVIEW'

			 END

			 ELSE

			 IF(
		       (@Is_LSU_BU>0) AND (@ADJ_STS_ID=350)
		      )

			 BEGIN

			--STEP 1: Updating the prem_adj table

			UPDATE PREM_ADJ	WITH (ROWLOCK)

			SET 
			adj_sts_typ_id=353,
			adj_sts_eff_dt=@create_date_time,
			undrwrt_not_reqr_ind = 1, 
			reconciler_revw_ind=1,
			UPDT_USER_ID = @CREATE_USER_ID,
			UPDT_DT = @create_date_time

			WHERE PREM_ADJ_ID = @PREM_ADJ_ID


			--STEP 2: Inserting into prem_adj_sts table for U/W Status

			INSERT INTO DBO.[PREM_ADJ_STS](

						[PREM_ADJ_ID],
						[CUSTMR_ID],
						[ADJ_STS_TYP_ID],
						[EFF_DT],
						[CMMNT_TXT],
						[APRV_IND],
						[CRTE_DT],
						[CRTE_USER_ID])

			VALUES(
						@PREM_ADJ_ID,
						@CUSTMR_ID,
						353,
						@create_date_time,
						'Auto Approval Process',
						1,
						@create_date_time,
						@CREATE_USER_ID)

			 	Print 'Block5:Status update to UW REVIEW(353) in this SP ONLY. '

	            SET @err_msg_output='UW REVIEW'
			 END

			 --ELSE
			 --IF(
		  --      (@Is_6_ADJ_TYPS>0) AND (@Is_LSU_BU>0) AND (@ADJ_STS_ID=350)
		  --    )
			 --BEGIN
			 --   Print 'Block6:Status update to RECON REVIEW(351). This will be done by C# Layer As per the exisisting Process. '
	         -- SET @err_msg_output='RECON REVIEW'
			 --END
CancelFlg:

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
		rollback transaction ModAIS_Process_ReconReview
	end

	declare @err_msg varchar(500),@err_ln varchar(10),

			@err_proc varchar(30),@err_no varchar(10)


	select  @err_msg = error_message(),
		    @err_no = error_number(),
		    @err_proc = error_procedure(),
			@err_ln = error_line()


	set @err_msg = '- error no.:' + isnull(@err_no, ' ') + '; procedure:' + isnull(@err_proc, ' ') + ';error line:' + isnull(@err_ln, ' ') + ';description:' + isnull(@err_msg, ' ' )

	--set @err_msg_output = @err_msg

	insert into [dbo].[APLCTN_STS_LOG]
	(
		[src_txt]
	   ,[sev_cd]
	   ,[shrt_desc_txt]
	   ,[full_desc_txt]
	   ,[custmr_id]
	   ,[prem_adj_id]
	   ,[crte_user_id]
	)

	values
  (
		'AIS Recon Review Processing'
       ,'ERR'
       ,'Recon Review processing Error'
       ,'Error encountered during Recon Review Processing of adjustment number: ' 

			+ convert(varchar(20),isnull(@prem_adj_id,0)) 
			+ ' for program number: ' 
			+ convert(varchar(20),0) 
			+ ' associated with customer number: ' 
			+ convert(varchar(20),0)
			+ '. Error message: '
			+ isnull(@err_msg, ' ')
	   ,NULL
	   ,@prem_adj_id
      ,isnull(@create_user_id, 0)
	)

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

go

if object_id('ModAIS_Process_ReconReview') is not null
	print 'Created Procedure ModAIS_Process_ReconReview'
else
	print 'Failed Creating Procedure ModAIS_Process_ReconReview'
go

if object_id('ModAIS_Process_ReconReview') is not null
	grant exec on ModAIS_Process_ReconReview to public
go

