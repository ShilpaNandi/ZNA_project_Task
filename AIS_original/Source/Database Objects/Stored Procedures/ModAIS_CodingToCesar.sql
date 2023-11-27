IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'ModAIS_CodingToCesar' and TYPE = 'P')
	DROP PROC ModAIS_CodingToCesar
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		ModAIS_CodingToCesar
-----
-----	Version:		SQL Server 2008
-----
-----	Created:		Venkat Kolimi
-----
-----	Description:	This procedure will populate the CESAR_CODING_HIST table with all the 
-----					codings for a given adjustment


---------------------------------------------------------------------

--Pass revision flag and adjustment ID. Revision flag will indicate if the amounts need to be reversed for a particular 
-- adjustment passed to it.

--1. To be called on finalization?. Not needed
--2. Call on revision of adjustment invoice?Not needed
--3. Call on void of adjustment invoice?Not needed


CREATE PROCEDURE [dbo].[ModAIS_CodingToCesar] 
@prem_adj_id int,
@create_user_id int,
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
    save transaction ModAIS_CodingToCesar
else
    begin transaction

begin try

	DECLARE
		@nErrorCode		     int,
		@sErrorMsg		     varchar(100),
		@ID			         int,
		@aplctn_sts_log_id   int,
		@Ceasr_last_run_dt   datetime,
		--@prem_adj_id         int,
		@historical_adj_ind  bit

	SET @nErrorCode = 0

	SELECT @aplctn_sts_log_id=MAX(aplctn_sts_log_id) 
	FROM APLCTN_STS_LOG
	WHERE shrt_desc_txt='CESAR data successfully loaded!'
	
	SELECT @Ceasr_last_run_dt=crte_dt
	FROM APLCTN_STS_LOG
	WHERE aplctn_sts_log_id=@aplctn_sts_log_id
	
		
	CREATE TABLE #Cesartemp(
				[INVOICE NUMBER] NVARCHAR(300), 
				[INVOICE DATE] NVARCHAR(300), 
				[PROGRAM PERIOD] NVARCHAR(300), 
				[VALUATION DATE] NVARCHAR(300), 
				[INSURED NAME] NVARCHAR(500),
				[BU/OFFICE] NVARCHAR(300),
				[BROKER ID] NVARCHAR(300),
				[BROKER] NVARCHAR(300),  
				[ADJUSTMENT TYPE] NVARCHAR(300),
				[ADJUSTMENT NUMBER] NVARCHAR(300),
				[PREM ADJ ID] INT,
				[PREM ADJ PGM ID] INT, 
				[POLICYID] INT, 
				[POLICY NUMBER] NVARCHAR(300),
				[POLICY PERIOD] NVARCHAR(300),
				[State] NVARCHAR(300),
				[Retro Result] decimal(15,2),
				Previous decimal(15,2), 
				ISRevised NVARCHAR(300),
				CompanyCode NVARCHAR(100),
                CurrencyCode NVARCHAR(100)
				)
				
	CREATE TABLE #Surchargetemp(
				[TOTROWCOUNT] INT,
				[INVOICE NUMBER] NVARCHAR(300), 
				[FINAL DATE] NVARCHAR(300),
				[INVOICE DATE] NVARCHAR(300), 
				[PROGRAM PERIOD] NVARCHAR(100), 
				[VALUATION DATE] NVARCHAR(300), 
				[INSURED NAME] NVARCHAR(200),
				[BROKER ID] NVARCHAR(300),
				[BROKER] NVARCHAR(500),  
				[ADJUSTMENT TYPE] NVARCHAR(300),
				[ADJUSTMENT NUMBER] NVARCHAR(300),
				[POLICY NUMBER] NVARCHAR(300),
				[POLICIES] NVARCHAR(MAX),
				[POLICY PERIOD] NVARCHAR(300),
				[STATE CD] NVARCHAR(300),
				[DESCRIPTION] NVARCHAR(MAX),
				[ABBREV] NVARCHAR(300),
				[BASIC AMT] decimal(15,2),
				[INCURRED LOSSES] decimal(15,2),
				[CURR ERP] decimal(15,2),
				[PREV ERP] decimal(15,2),
				[RETRO RESULT] decimal(15,2),
				[ADDN SURCHARGE ASSESSMENT COMP] decimal(15,2),
				[TOTAL SURCHARGE ASSESSMENT BASE] decimal(15,2),
				[FACTOR] decimal(15,2),
				[OTHER SURCHARGE AMOUNT] decimal(15,2),
				[TOTAL ADDITIONAL RETURN] decimal(15,2),
				[COMMENTS] NVARCHAR(MAX),
				[PREM ADJ ID] INT,
				[PREM ADJ PGM ID] INT, 
				ISRevised NVARCHAR(300)
				)

							select 
							@historical_adj_ind=historical_adj_ind
							FROM PREM_ADJ 
							where prem_adj_id=@prem_adj_id
			                
							if @debug = 1
							begin
							print'*******************CESAR CODING: START OF OUTER ITERATION*********' 
							print'---------------Input Params-------------------' 

							print' @prem_adj_id:- ' + convert(varchar(20), @prem_adj_id)  
							print' @historical_adj_ind:- ' + convert(varchar(20), @historical_adj_ind)  
							
							end
							print ' before truncate '
							TRUNCATE TABLE #Cesartemp
							TRUNCATE TABLE #Surchargetemp
							print ' after truncate '
							
							print ' before GetCesarCodingWorksheet '
							INSERT INTO #Cesartemp
                            exec dbo.GetCesarCodingWorksheet @prem_adj_id,2,@historical_adj_ind
                            
                            print ' before GetCesarCodingWorksheet #Surchargetemp'
                            INSERT INTO #Surchargetemp
                            exec dbo.GetSurchargesAssessments @prem_adj_id,2,0,@historical_adj_ind
							
							print ' after GetCesarCodingWorksheet '
							
							print ' before [CESAR_CODING_HIST] '
							
							
							SELECT 
									   prem_adj_perd_id,
									   prem_adj.prem_adj_id,
									   rel_prem_adj_id,
									   PREM_ADJ_PERD.custmr_id,
									   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') as "invc_nbr_txt",
									   ISNULL(CONVERT(char(8), ISNULL(PREM_ADJ.fnl_invc_dt,PREM_ADJ.crte_dt), 112), '') as "invc_dt" , 
									   CONVERT(char(10),
									   CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' 
									        THEN dbo.fn_GetDEPMasterPolicyNumber(PREM_ADJ_PERD.PREM_ADJ_PGM_ID) 
									        ELSE SUBSTRING(LTRIM(RTRIM(coml_agmt.pol_nbr_txt)),2, LEN(LTRIM(RTRIM(coml_agmt.pol_nbr_txt)))-1)
									        END) as "Policy_nbr_txt",
									   
									   CONVERT(char(5),
									   CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP'
									        THEN 'DEP'
									        ELSE coml_agmt.pol_sym_txt
									        END) as "Policy_Sym_txt",
									   
									   CONVERT(char(5),
									   CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' 
									        THEN dbo.fn_GetDEPMasterPolicyMod(PREM_ADJ_PERD.PREM_ADJ_PGM_ID) 
									        ELSE coml_agmt.pol_modulus_txt
									        END) as "Policy_mod_txt",

									   ISNULL(CONVERT(char(8), 
									   CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP'
									        THEN dbo.fn_GetDEPMasterPolicyEFFDT(PREM_ADJ_PERD.PREM_ADJ_PGM_ID)
											ELSE coml_agmt.pol_eff_dt
											END, 112), '') as "Policy_eff_dt", 
											
									   ISNULL(CONVERT(char(8), 
									   CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP'
									        THEN dbo.fn_GetDEPMasterPolicyEXPDT(PREM_ADJ_PERD.PREM_ADJ_PGM_ID)
									        ELSE coml_agmt.planned_end_date
									   END, 112), '') as "Policy_exp_dt", 

									   ISNULL(CONVERT(char(3),LKUP.attr_1_txt),'') as "State_txt",
									   ISNULL((#Cesartemp.[Retro Result]-#Cesartemp.[Previous]),0) as "Transaction_Amt",
									   CONVERT(char(5), #Cesartemp.CompanyCode) as "Company_txt",
									   CONVERT(char(5), #Cesartemp.CurrencyCode) as "Currency_txt"
										
									   into #Cesartemp2
									   FROM #Cesartemp
									   INNER JOIN PREM_ADJ ON prem_adj.prem_adj_id=#Cesartemp.[PREM ADJ ID]
									   INNER JOIN PREM_ADJ_PERD ON PREM_ADJ_PERD.prem_adj_id=prem_adj.prem_adj_id and PREM_ADJ_PERD.prem_adj_pgm_id=#Cesartemp.[PREM ADJ PGM ID]
									   INNER JOIN PREM_ADJ_PGM ON PREM_ADJ_PGM.prem_adj_pgm_id=PREM_ADJ_PERD.prem_adj_pgm_id
									   INNER JOIN CUSTMR ON CUSTMR.custmr_id=PREM_ADJ_PERD.custmr_id
									   INNER JOIN COML_AGMT ON COML_AGMT.coml_agmt_id=#Cesartemp.POLICYID
									   INNER JOIN LKUP ON LKUP.lkup_txt=#Cesartemp.State
									   INNER JOIN LKUP AS PGMTYP ON PREM_ADJ_PGM.pgm_typ_id = PGMTYP.lkup_id
									   WHERE #Cesartemp.CompanyCode='Z01' and CUSTMR.company_cd=733 and isnull(CUSTMR.custmr_test_ind ,0)<> 1
							
							
							
							
								INSERT INTO [dbo].[CESAR_CODING_HIST]
									   (
										[prem_adj_perd_id]
									   ,[prem_adj_id]
									   ,[rel_prem_adj_id]
									   ,[custmr_id]
									   ,[cesar_coding_sent_ind]
									   ,[invc_nbr_txt]
									   ,[invc_dt]
									   ,[Policy_nbr_txt]
									   ,[Policy_Sym_txt]
									   ,[Policy_mod_txt]
									   ,[Policy_eff_dt]
									   ,[Policy_exp_dt]
									   ,[State_txt]
									   ,[Transaction_Amt]
									   ,[Surcharge_Ass_cd]
									   ,[Company_txt]
									   ,[Currency_txt]
									   ,[updt_user_id]
									   ,[updt_dt]
									   ,[crte_user_id]
									   ,[crte_dt]
									   ,[actv_ind])
							           
							     SELECT 
									   prem_adj_perd_id,
									   prem_adj_id,
									   MAX(rel_prem_adj_id),
									   custmr_id,
									   0,
									   MAX(invc_nbr_txt),
									   MAX(invc_dt),
									   Policy_nbr_txt,
									   MAX(Policy_Sym_txt),
									   Policy_mod_txt,
									   MAX(Policy_eff_dt),
									   MAX(Policy_exp_dt),
									   State_txt,
									   ISNULL(CONVERT(char(20),SUM(ISNULL(Transaction_Amt,0))),''),
									   CONVERT(char(8), '') , 
									   MAX(Company_txt),
									   MAX(Currency_txt),
									   1,
									   GETDATE(),
									   1,
									   GETDATE(),
									   1
								FROM #Cesartemp2
							    GROUP BY 
									   prem_adj_id,
									   prem_adj_perd_id,
									   custmr_id,
									   Policy_nbr_txt,
									   Policy_mod_txt,
									   State_txt
									   
									   
							INSERT INTO [dbo].[CESAR_CODING_HIST]
									   (
										[prem_adj_perd_id]
									   ,[prem_adj_id]
									   ,[rel_prem_adj_id]
									   ,[custmr_id]
									   ,[cesar_coding_sent_ind]
									   ,[invc_nbr_txt]
									   ,[invc_dt]
									   ,[Policy_nbr_txt]
									   ,[Policy_Sym_txt]
									   ,[Policy_mod_txt]
									   ,[Policy_eff_dt]
									   ,[Policy_exp_dt]
									   ,[State_txt]
									   ,[Transaction_Amt]
									   ,[Surcharge_Ass_cd]
									   ,[Company_txt]
									   ,[Currency_txt]
									   ,[updt_user_id]
									   ,[updt_dt]
									   ,[crte_user_id]
									   ,[crte_dt]
									   ,[actv_ind])		   
									   SELECT
									   DISTINCT
									   prem_adj_perd_id,
									   prem_adj.prem_adj_id,
									   rel_prem_adj_id,
									   PREM_ADJ_PERD.custmr_id,
									   0,
									   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),''),
									   ISNULL(CONVERT(char(8), ISNULL(PREM_ADJ.fnl_invc_dt,PREM_ADJ.crte_dt), 112), '') , 
									   CONVERT(char(10),SUBSTRING(LTRIM(RTRIM(coml_agmt.pol_nbr_txt)),2, LEN(LTRIM(RTRIM(coml_agmt.pol_nbr_txt)))-1)),
									   CONVERT(char(5),coml_agmt.pol_sym_txt),
									   CONVERT(char(5),coml_agmt.pol_modulus_txt),
									   ISNULL(CONVERT(char(8), coml_agmt.pol_eff_dt, 112), '') , 
									   ISNULL(CONVERT(char(8), coml_agmt.planned_end_date, 112), '') , 
									   ISNULL(CONVERT(char(3),#Surchargetemp.[STATE CD]),''),
									   ISNULL(CONVERT(char(20),(#Surchargetemp.[TOTAL ADDITIONAL RETURN])),''),
									   CONVERT(char(8), LTRIM(RTRIM(REPLACE(REPLACE(REPLACE(#Surchargetemp.[STATE CD],'-',''),'*',''),' ','')))) , 
									   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(5), 'Z01') ELSE CONVERT(char(5), 'Z2C') END,
									   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END, 
										1,
										GETDATE(),
										1,
										GETDATE(),
										1
									   FROM #Surchargetemp
									   INNER JOIN PREM_ADJ ON prem_adj.prem_adj_id=#Surchargetemp.[PREM ADJ ID]
									   INNER JOIN PREM_ADJ_PERD ON PREM_ADJ_PERD.prem_adj_id=#Surchargetemp.[PREM ADJ ID] and PREM_ADJ_PERD.prem_adj_pgm_id=#Surchargetemp.[PREM ADJ PGM ID]
									   INNER JOIN CUSTMR ON CUSTMR.custmr_id=PREM_ADJ_PERD.custmr_id
									   INNER JOIN COML_AGMT ON (COML_AGMT.POL_SYM_TXT+COML_AGMT.POL_NBR_TXT+COML_AGMT.POL_MODULUS_TXT)=#Surchargetemp.[POLICY NUMBER] and COML_AGMT.prem_adj_pgm_id=#Surchargetemp.[PREM ADJ PGM ID]
									   WHERE CUSTMR.company_cd=733 and isnull(CUSTMR.custmr_test_ind ,0)<> 1

									print ' after [CESAR_CODING_HIST] '
									UPDATE PREM_ADJ SET sent_to_cesar=1 WHERE prem_adj_id = @prem_adj_id
							
							
									print 'Drop temp tables'
									Drop table #Cesartemp
									Drop table #Cesartemp2
									Drop table #Surchargetemp
					


			
			

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
		rollback transaction ModAIS_CodingToCesar
	end


	
	declare @err_msg varchar(500),@err_ln varchar(10),
			@err_proc varchar(30),@err_no varchar(10)
	select  @err_msg = error_message(),
		    @err_no = error_number(),
		    @err_proc = error_procedure(),
			@err_ln = error_line()
	set @err_msg = '- error no.:' + isnull(@err_no, ' ') + '; procedure:' + isnull(@err_proc, ' ') + ';error line:' + isnull(@err_ln, ' ') + ';description:' + isnull(@err_msg, ' ' )
	set @err_msg_output = @err_msg

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
		'AIS Calculation Engine'
       ,'ERR'
       ,'Calculation error'
       ,'Error encountered during calculation of adjustment number: ' 
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
GO

IF OBJECT_ID('ModAIS_CodingToCesar') IS NOT NULL
	PRINT 'CREATED PROCEDURE ModAIS_CodingToCesar'
ELSE
	PRINT 'FAILED CREATING PROCEDURE ModAIS_CodingToCesar'
GO

IF OBJECT_ID('ModAIS_CodingToCesar') IS NOT NULL
	GRANT EXEC ON ModAIS_CodingToCesar TO PUBLIC
GO