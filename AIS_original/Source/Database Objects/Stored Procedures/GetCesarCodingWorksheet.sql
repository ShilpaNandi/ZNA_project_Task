
if exists (select 1 from sysobjects 
                where name = 'GetCesarCodingWorksheet' and type = 'P')
        drop procedure GetCesarCodingWorksheet
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sreedhar
-- Create date: <10th Dec 2008>
-- Modified : 12th March 2009 - Sreedhar
--			: 11th July 2014 - Rajaji - Added CompanyCode and CurrencyCode
-- Description:	This procedure helps to retrive Cesar Coding Worksheet Details
-- =============================================

CREATE PROCEDURE [dbo].[GetCesarCodingWorksheet] 
	-- Add the parameters for the stored procedure here

@ADJNO INT,
@FLAG INT,
@HISTFLAG BIT
AS
BEGIN
 -- SET NOCOUNT ON added to prevent extra result sets from
 -- interfering with SELECT statements.
 SET NOCOUNT ON;

BEGIN TRY

DECLARE @OldAdjID int
DECLARE @IsRevised bit
DECLARE @IsVoid bit
DECLARE @FnlInvNbr varchar(3)

select @OldAdjID=OldAdj.PREM_ADJ_ID,@IsRevised=OldAdj.ADJ_RRSN_IND,@IsVoid=OldAdj.adj_void_ind, @FnlInvNbr=isnull(SUBSTRING(Adj.fnl_invc_nbr_txt,1,3),'')
from PREM_ADJ OldAdj INNER JOIN PREM_ADJ Adj ON  OldAdj.PREM_ADJ_ID=Adj.REL_PREM_ADJ_ID WHERE Adj.PREM_ADJ_ID=@ADJNO

If (@OldAdjID>0) and (@IsRevised=1) and (@FnlInvNbr<>'RTV')
BEGIN

SELECT CASE @FLAG WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END
                       AS [INVOICE NUMBER], CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) WHEN 2 THEN CONVERT(NVARCHAR(30), 
                      FNL_INVC_DT, 101) ELSE CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) END AS [INVOICE DATE], CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) 
                      + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) AS [PROGRAM PERIOD], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 
                      101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], dbo.INT_ORG.full_name + '/' + dbo.INT_ORG.city_nm AS [BU/OFFICE], 
                      dbo.PREM_ADJ_PGM.brkr_id AS [BROKER ID], dbo.EXTRNL_ORG.full_name AS BROKER, 
          LKUP_AdjTyp.lkup_txt AS [ADJUSTMENT TYPE], 
                      dbo.PREM_ADJ_PERD.adj_nbr_txt AS [ADJUSTMENT NUMBER], dbo.PREM_ADJ.prem_adj_id AS [PREM ADJ ID], 
                      dbo.PREM_ADJ_PERD.prem_adj_pgm_id AS [PREM ADJ PGM ID], dbo.COML_AGMT.coml_agmt_id AS PolicyID, PREM_ADJ_RETRO_DTL.St_id as StateID,prem_adj_perd.custmr_id as CustmrID, 
                      --If Program Type is DEP then bring DEP Policy Number
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicy(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
                      RTRIM(dbo.COML_AGMT.pol_sym_txt) + ' ' + RTRIM(dbo.COML_AGMT.pol_nbr_txt) + '-' + dbo.COML_AGMT.pol_modulus_txt END AS [POLICY NUMBER], 
                      --If Program Type is DEP then bring DEP Policy Period
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicyPeriod(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
                      CONVERT(NVARCHAR(30), dbo.COML_AGMT.pol_eff_dt, 101) + ' - ' + CONVERT(NVARCHAR(30), dbo.COML_AGMT.planned_end_date, 101) END AS [POLICY PERIOD],                       
                      --If Program Type is DEP then bring DEP Policy State
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicyState(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
       LKUP_St.lkup_txt END AS State,        
       CASE WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='ERP' THEN 
       (isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0)) 
       WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='Max' THEN
       (isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)  
       - (isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0)*       
       [dbo].[fn_GetCesarRatioForMax](PREM_ADJ_RETRO_DTL.prem_adj_id,
       PREM_ADJ_RETRO_DTL.custmr_id,PREM_ADJ_RETRO_DTL.prem_adj_pgm_id))
       - isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0))
       WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='Min' THEN
       ((isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0))
       -isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0)) END AS [Retro Result],              
        isnull(dbo.[fn_GetPrevERPMinMaxCalValue](PREM_ADJ_RETRO_DTL.prem_adj_id, PREM_ADJ_RETRO_DTL.coml_agmt_id, PREM_ADJ_RETRO_DTL.st_id,PREM_ADJ_RETRO_DTL.CUSTMR_ID,PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID),0)
       AS Previous,  'False' as IsRevised,
       COMP_CODE.lkup_txt AS CompanyCode,
       CURR_CODE.lkup_txt AS CurrencyCode
Into #tmp_Current_CESAR_Revision_New
FROM         dbo.LKUP AS LKUP_St INNER JOIN
                      dbo.PREM_ADJ_RETRO_DTL INNER JOIN
                      dbo.COML_AGMT ON dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.COML_AGMT.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.custmr_id = dbo.COML_AGMT.custmr_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.coml_agmt_id = dbo.COML_AGMT.coml_agmt_id ON LKUP_St.lkup_id = dbo.PREM_ADJ_RETRO_DTL.st_id INNER JOIN
                      dbo.LKUP AS LKUP_AdjTyp INNER JOIN
                      dbo.PREM_ADJ_PGM ON LKUP_AdjTyp.lkup_id = dbo.PREM_ADJ_PGM.paid_incur_typ_id INNER JOIN
                      dbo.LKUP AS PGMTYP ON dbo.PREM_ADJ_PGM.pgm_typ_id = PGMTYP.lkup_id ON 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.PREM_ADJ_PGM.prem_adj_pgm_id INNER JOIN
                      dbo.EXTRNL_ORG INNER JOIN
                      dbo.PREM_ADJ INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id AND 
                      dbo.PREM_ADJ.reg_custmr_id = dbo.PREM_ADJ_PERD.reg_custmr_id INNER JOIN
                      dbo.CUSTMR ON dbo.PREM_ADJ_PERD.custmr_id = dbo.CUSTMR.custmr_id ON dbo.EXTRNL_ORG.extrnl_org_id = dbo.PREM_ADJ.brkr_id INNER JOIN
                      dbo.INT_ORG ON dbo.PREM_ADJ.bu_office_id = dbo.INT_ORG.int_org_id ON 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_perd_id = dbo.PREM_ADJ_PERD.prem_adj_perd_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.custmr_id = dbo.PREM_ADJ_PERD.custmr_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.PREM_ADJ_PERD.prem_adj_pgm_id
                      INNER JOIN LKUP COMP_CODE ON dbo.CUSTMR.company_cd = COMP_CODE.lkup_id
                      INNER JOIN LKUP CURR_CODE ON dbo.CUSTMR.currency_cd = CURR_CODE.lkup_id
WHERE PREM_ADJ.PREM_ADJ_ID =  @ADJNO 

select [PREM ADJ ID],CustmrID,[PREM ADJ PGM ID] into #tmp2_Revision_New from #tmp_Current_CESAR_Revision_New

create table #prevadjid_Revision_New(prev_adj_id int)

   declare @adjid_Revision_New int
   declare @custid_Revision_New int
   declare @pgmid_Revision_New int
   DECLARE @Prev_Adj_id_Revision_New int
   declare @prem_adj_id_cur_Revision_New CURSOR
   SET @prem_adj_id_cur_Revision_New = cursor LOCAL FAST_FORWARD READ_ONLY FOR select distinct [PREM ADJ ID],CustmrID,[PREM ADJ PGM ID] from #tmp2_Revision_New
   open @prem_adj_id_cur_Revision_New 
   fetch @prem_adj_id_cur_Revision_New into @adjid_Revision_New, @custid_Revision_New, @pgmid_Revision_New
   WHILE @@FETCH_STATUS = 0
   BEGIN  
    SET @Prev_Adj_id_Revision_New=dbo.fn_DeterminePrevValidAdj_Premium(@adjid_Revision_New,@custid_Revision_New,@pgmid_Revision_New)

    if @Prev_Adj_id_Revision_New>0 
    Begin
     insert into #prevadjid_Revision_New values (@Prev_Adj_id_Revision_New)
     Set @Prev_Adj_id_Revision_New=0
    END

   FETCH NEXT FROM @prem_adj_id_cur_Revision_New INTO  @adjid_Revision_New, @custid_Revision_New, @pgmid_Revision_New
   END
   CLOSE @prem_adj_id_cur_Revision_New
   DEALLOCATE @prem_adj_id_cur_Revision_New
   
SELECT CASE @FLAG WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END
                       AS [INVOICE NUMBER], CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) WHEN 2 THEN CONVERT(NVARCHAR(30), 
                      FNL_INVC_DT, 101) ELSE CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) END AS [INVOICE DATE], CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) 
                      + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) AS [PROGRAM PERIOD], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 
                      101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], dbo.INT_ORG.full_name + '/' + dbo.INT_ORG.city_nm AS [BU/OFFICE], 
                      dbo.PREM_ADJ_PGM.brkr_id AS [BROKER ID], dbo.EXTRNL_ORG.full_name AS BROKER, 
          LKUP_AdjTyp.lkup_txt AS [ADJUSTMENT TYPE], 
                      dbo.PREM_ADJ_PERD.adj_nbr_txt AS [ADJUSTMENT NUMBER], @ADJNO AS [PREM ADJ ID], 
                      dbo.PREM_ADJ_PERD.prem_adj_pgm_id AS [PREM ADJ PGM ID], dbo.COML_AGMT.coml_agmt_id AS PolicyID, PREM_ADJ_RETRO_DTL.St_id as StateID,prem_adj_perd.custmr_id as CustmrID, 
                      --If Program Type is DEP then bring DEP Policy Number
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicy(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
                      RTRIM(dbo.COML_AGMT.pol_sym_txt) + ' ' + RTRIM(dbo.COML_AGMT.pol_nbr_txt) + '-' + dbo.COML_AGMT.pol_modulus_txt END AS [POLICY NUMBER], 
                      --If Program Type is DEP then bring DEP Policy Period
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicyPeriod(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
                      CONVERT(NVARCHAR(30), dbo.COML_AGMT.pol_eff_dt, 101) + ' - ' + CONVERT(NVARCHAR(30), dbo.COML_AGMT.planned_end_date, 101) END AS [POLICY PERIOD],                       
                      --If Program Type is DEP then bring DEP Policy State
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicyState(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
       LKUP_St.lkup_txt END AS State,        
       0 AS [Retro Result],              
       CASE WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='ERP' THEN 
       (isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0)) 
       WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='Max' THEN
       (isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)  
       - (isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0)*       
       [dbo].[fn_GetCesarRatioForMax](PREM_ADJ_RETRO_DTL.prem_adj_id,
       PREM_ADJ_RETRO_DTL.custmr_id,PREM_ADJ_RETRO_DTL.prem_adj_pgm_id))
       - isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0))
       WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='Min' THEN
       ((isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0))
       -isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0)) END AS Previous,  'False' as IsRevised,
       COMP_CODE.lkup_txt AS CompanyCode,
       CURR_CODE.lkup_txt AS CurrencyCode
Into #tmp_Previous_CESAR_Revision_New
FROM         dbo.LKUP AS LKUP_St INNER JOIN
                      dbo.PREM_ADJ_RETRO_DTL INNER JOIN
                      dbo.COML_AGMT ON dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.COML_AGMT.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.custmr_id = dbo.COML_AGMT.custmr_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.coml_agmt_id = dbo.COML_AGMT.coml_agmt_id ON LKUP_St.lkup_id = dbo.PREM_ADJ_RETRO_DTL.st_id INNER JOIN
                      dbo.LKUP AS LKUP_AdjTyp INNER JOIN
                      dbo.PREM_ADJ_PGM ON LKUP_AdjTyp.lkup_id = dbo.PREM_ADJ_PGM.paid_incur_typ_id INNER JOIN
                      dbo.LKUP AS PGMTYP ON dbo.PREM_ADJ_PGM.pgm_typ_id = PGMTYP.lkup_id ON 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.PREM_ADJ_PGM.prem_adj_pgm_id INNER JOIN
                      dbo.EXTRNL_ORG INNER JOIN
                      dbo.PREM_ADJ INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id AND 
                      dbo.PREM_ADJ.reg_custmr_id = dbo.PREM_ADJ_PERD.reg_custmr_id INNER JOIN
                      dbo.CUSTMR ON dbo.PREM_ADJ_PERD.custmr_id = dbo.CUSTMR.custmr_id ON dbo.EXTRNL_ORG.extrnl_org_id = dbo.PREM_ADJ.brkr_id INNER JOIN
                      dbo.INT_ORG ON dbo.PREM_ADJ.bu_office_id = dbo.INT_ORG.int_org_id ON 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_perd_id = dbo.PREM_ADJ_PERD.prem_adj_perd_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.custmr_id = dbo.PREM_ADJ_PERD.custmr_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.PREM_ADJ_PERD.prem_adj_pgm_id
                      INNER JOIN LKUP COMP_CODE ON dbo.CUSTMR.company_cd = COMP_CODE.lkup_id
                      INNER JOIN LKUP CURR_CODE ON dbo.CUSTMR.currency_cd = CURR_CODE.lkup_id
WHERE PREM_ADJ.PREM_ADJ_ID in (select distinct prev_adj_id from #prevadjid_Revision_New) 

UPDATE #tmp_Previous_CESAR_Revision_New set #tmp_Previous_CESAR_Revision_New.[INVOICE NUMBER]= (select top 1 [INVOICE NUMBER] from #tmp_Current_CESAR_Revision_New
  where #tmp_Previous_CESAR_Revision_New.[PREM ADJ PGM ID] = #tmp_Current_CESAR_Revision_New.[PREM ADJ PGM ID] 
  and #tmp_Previous_CESAR_Revision_New.CustmrID = #tmp_Current_CESAR_Revision_New.CustmrID),
  #tmp_Previous_CESAR_Revision_New.[INVOICE DATE]= (select top 1 [INVOICE DATE] from #tmp_Current_CESAR_Revision_New
  where #tmp_Previous_CESAR_Revision_New.[PREM ADJ PGM ID] = #tmp_Current_CESAR_Revision_New.[PREM ADJ PGM ID] 
  and #tmp_Previous_CESAR_Revision_New.CustmrID = #tmp_Current_CESAR_Revision_New.CustmrID),
  #tmp_Previous_CESAR_Revision_New.[ADJUSTMENT NUMBER]= (select top 1 [ADJUSTMENT NUMBER] from #tmp_Current_CESAR_Revision_New
  where #tmp_Previous_CESAR_Revision_New.[PREM ADJ PGM ID] = #tmp_Current_CESAR_Revision_New.[PREM ADJ PGM ID] 
  and #tmp_Previous_CESAR_Revision_New.CustmrID = #tmp_Current_CESAR_Revision_New.CustmrID)
--------------------------------------------------------------Revision old--------------------------------

--UNION ALL
SELECT  PREM_ADJ.FNL_INVC_NBR_TXT  AS [INVOICE NUMBER],  CONVERT(NVARCHAR(30), FNL_INVC_DT, 101) AS [INVOICE DATE], CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) 
                      + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) AS [PROGRAM PERIOD], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 
                      101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], dbo.INT_ORG.full_name + '/' + dbo.INT_ORG.city_nm AS [BU/OFFICE], 
                      dbo.PREM_ADJ_PGM.brkr_id AS [BROKER ID], dbo.EXTRNL_ORG.full_name AS BROKER, 
          LKUP_AdjTyp.lkup_txt AS [ADJUSTMENT TYPE], 
                      dbo.PREM_ADJ_PERD.adj_nbr_txt AS [ADJUSTMENT NUMBER], dbo.PREM_ADJ.prem_adj_id AS [PREM ADJ ID], 
                      dbo.PREM_ADJ_PERD.prem_adj_pgm_id AS [PREM ADJ PGM ID], dbo.COML_AGMT.coml_agmt_id AS PolicyID, PREM_ADJ_RETRO_DTL.St_id as StateID,prem_adj_perd.custmr_id as CustmrID,
                      --If Program Type is DEP then bring DEP Policy Number
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicy(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
                      RTRIM(dbo.COML_AGMT.pol_sym_txt) + ' ' + RTRIM(dbo.COML_AGMT.pol_nbr_txt) + '-' + dbo.COML_AGMT.pol_modulus_txt END AS [POLICY NUMBER], 
                      --If Program Type is DEP then bring DEP Policy Period
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicyPeriod(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
                      CONVERT(NVARCHAR(30), dbo.COML_AGMT.pol_eff_dt, 101) + ' - ' + CONVERT(NVARCHAR(30), dbo.COML_AGMT.planned_end_date, 101) END AS [POLICY PERIOD],                       
                      --If Program Type is DEP then bring DEP Policy State
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicyState(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
       LKUP_St.lkup_txt END AS State,                       
                      CASE WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='ERP' THEN 
       (isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0)) 
       WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='Max' THEN
       (isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)  
       - (isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0)*       
       [dbo].[fn_GetCesarRatioForMax](PREM_ADJ_RETRO_DTL.prem_adj_id,
       PREM_ADJ_RETRO_DTL.custmr_id,PREM_ADJ_RETRO_DTL.prem_adj_pgm_id))
       - isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0))
       WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='Min' THEN
       ((isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0))
       -isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0))  END * -1 AS [Retro Result],              
       (isnull(dbo.[fn_GetPrevERPMinMaxCalValue](PREM_ADJ_RETRO_DTL.prem_adj_id, PREM_ADJ_RETRO_DTL.coml_agmt_id, PREM_ADJ_RETRO_DTL.st_id,
       PREM_ADJ_RETRO_DTL.CUSTMR_ID,PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID),0))* -1 AS Previous,'True' as IsRevised,
       COMP_CODE.lkup_txt AS CompanyCode,
       CURR_CODE.lkup_txt AS CurrencyCode
Into #tmp_Current_CESAR_Revision_Old
FROM         dbo.LKUP AS LKUP_St INNER JOIN
                      dbo.PREM_ADJ_RETRO_DTL INNER JOIN
                      dbo.COML_AGMT ON dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.COML_AGMT.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.custmr_id = dbo.COML_AGMT.custmr_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.coml_agmt_id = dbo.COML_AGMT.coml_agmt_id ON LKUP_St.lkup_id = dbo.PREM_ADJ_RETRO_DTL.st_id INNER JOIN
                      dbo.LKUP AS LKUP_AdjTyp INNER JOIN
                      dbo.PREM_ADJ_PGM ON LKUP_AdjTyp.lkup_id = dbo.PREM_ADJ_PGM.paid_incur_typ_id INNER JOIN
                      dbo.LKUP AS PGMTYP ON dbo.PREM_ADJ_PGM.pgm_typ_id = PGMTYP.lkup_id ON 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.PREM_ADJ_PGM.prem_adj_pgm_id INNER JOIN
                      dbo.EXTRNL_ORG INNER JOIN
                      dbo.PREM_ADJ INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id AND 
                      dbo.PREM_ADJ.reg_custmr_id = dbo.PREM_ADJ_PERD.reg_custmr_id INNER JOIN
                      dbo.CUSTMR ON dbo.PREM_ADJ_PERD.custmr_id = dbo.CUSTMR.custmr_id ON dbo.EXTRNL_ORG.extrnl_org_id = dbo.PREM_ADJ.brkr_id INNER JOIN
                      dbo.INT_ORG ON dbo.PREM_ADJ.bu_office_id = dbo.INT_ORG.int_org_id ON 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_perd_id = dbo.PREM_ADJ_PERD.prem_adj_perd_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.custmr_id = dbo.PREM_ADJ_PERD.custmr_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.PREM_ADJ_PERD.prem_adj_pgm_id
                      INNER JOIN LKUP COMP_CODE ON dbo.CUSTMR.company_cd = COMP_CODE.lkup_id
                      INNER JOIN LKUP CURR_CODE ON dbo.CUSTMR.currency_cd = CURR_CODE.lkup_id
WHERE PREM_ADJ.PREM_ADJ_ID =  @OldAdjID order by State

select [PREM ADJ ID],CustmrID,[PREM ADJ PGM ID] into #tmp2_Revision_Old from #tmp_Current_CESAR_Revision_Old

create table #prevadjid_Revision_Old(prev_adj_id int)

   declare @adjid_Revision_Old int
   declare @custid_Revision_Old int
   declare @pgmid_Revision_Old int
   DECLARE @Prev_Adj_id_Revision_Old int
   declare @prem_adj_id_cur_Revision_Old CURSOR
   SET @prem_adj_id_cur_Revision_Old = cursor LOCAL FAST_FORWARD READ_ONLY FOR select distinct [PREM ADJ ID],CustmrID,[PREM ADJ PGM ID] from #tmp2_Revision_Old
   open @prem_adj_id_cur_Revision_Old 
   fetch @prem_adj_id_cur_Revision_Old into @adjid_Revision_Old, @custid_Revision_Old, @pgmid_Revision_Old
   WHILE @@FETCH_STATUS = 0
   BEGIN  
    SET @Prev_Adj_id_Revision_Old=dbo.fn_DeterminePrevValidAdj_Premium(@adjid_Revision_Old,@custid_Revision_Old,@pgmid_Revision_Old)

    if @Prev_Adj_id_Revision_Old>0 
    Begin
     insert into #prevadjid_Revision_Old values (@Prev_Adj_id_Revision_Old)
     Set @Prev_Adj_id_Revision_Old=0
    END

   FETCH NEXT FROM @prem_adj_id_cur_Revision_Old INTO  @adjid_Revision_Old, @custid_Revision_Old, @pgmid_Revision_Old
   END
   CLOSE @prem_adj_id_cur_Revision_Old
   DEALLOCATE @prem_adj_id_cur_Revision_Old

SELECT  PREM_ADJ.FNL_INVC_NBR_TXT  AS [INVOICE NUMBER],  CONVERT(NVARCHAR(30), FNL_INVC_DT, 101) AS [INVOICE DATE], CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) 
                      + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) AS [PROGRAM PERIOD], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 
                      101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], dbo.INT_ORG.full_name + '/' + dbo.INT_ORG.city_nm AS [BU/OFFICE], 
                      dbo.PREM_ADJ_PGM.brkr_id AS [BROKER ID], dbo.EXTRNL_ORG.full_name AS BROKER, 
          LKUP_AdjTyp.lkup_txt AS [ADJUSTMENT TYPE], 
                      dbo.PREM_ADJ_PERD.adj_nbr_txt AS [ADJUSTMENT NUMBER], @OldAdjID AS [PREM ADJ ID], 
                      dbo.PREM_ADJ_PERD.prem_adj_pgm_id AS [PREM ADJ PGM ID], dbo.COML_AGMT.coml_agmt_id AS PolicyID, PREM_ADJ_RETRO_DTL.St_id as StateID,prem_adj_perd.custmr_id as CustmrID,
                      --If Program Type is DEP then bring DEP Policy Number
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicy(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
                      RTRIM(dbo.COML_AGMT.pol_sym_txt) + ' ' + RTRIM(dbo.COML_AGMT.pol_nbr_txt) + '-' + dbo.COML_AGMT.pol_modulus_txt END AS [POLICY NUMBER], 
                      --If Program Type is DEP then bring DEP Policy Period
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicyPeriod(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
                      CONVERT(NVARCHAR(30), dbo.COML_AGMT.pol_eff_dt, 101) + ' - ' + CONVERT(NVARCHAR(30), dbo.COML_AGMT.planned_end_date, 101) END AS [POLICY PERIOD],                       
                      --If Program Type is DEP then bring DEP Policy State
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicyState(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
       LKUP_St.lkup_txt END AS State,                       
                      0 AS [Retro Result],              
       CASE WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='ERP' THEN 
       (isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0)) 
       WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='Max' THEN
       (isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)  
       - (isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0)*       
       [dbo].[fn_GetCesarRatioForMax](PREM_ADJ_RETRO_DTL.prem_adj_id,
       PREM_ADJ_RETRO_DTL.custmr_id,PREM_ADJ_RETRO_DTL.prem_adj_pgm_id))
       - isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0))
       WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='Min' THEN
       ((isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0))
       -isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0))  END * -1 AS Previous,'True' as IsRevised,
       COMP_CODE.lkup_txt AS CompanyCode,
       CURR_CODE.lkup_txt AS CurrencyCode
Into #tmp_Previous_CESAR_Revision_Old
FROM         dbo.LKUP AS LKUP_St INNER JOIN
                      dbo.PREM_ADJ_RETRO_DTL INNER JOIN
                      dbo.COML_AGMT ON dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.COML_AGMT.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.custmr_id = dbo.COML_AGMT.custmr_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.coml_agmt_id = dbo.COML_AGMT.coml_agmt_id ON LKUP_St.lkup_id = dbo.PREM_ADJ_RETRO_DTL.st_id INNER JOIN
                      dbo.LKUP AS LKUP_AdjTyp INNER JOIN
                      dbo.PREM_ADJ_PGM ON LKUP_AdjTyp.lkup_id = dbo.PREM_ADJ_PGM.paid_incur_typ_id INNER JOIN
                      dbo.LKUP AS PGMTYP ON dbo.PREM_ADJ_PGM.pgm_typ_id = PGMTYP.lkup_id ON 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.PREM_ADJ_PGM.prem_adj_pgm_id INNER JOIN
                      dbo.EXTRNL_ORG INNER JOIN
                      dbo.PREM_ADJ INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id AND 
                      dbo.PREM_ADJ.reg_custmr_id = dbo.PREM_ADJ_PERD.reg_custmr_id INNER JOIN
                      dbo.CUSTMR ON dbo.PREM_ADJ_PERD.custmr_id = dbo.CUSTMR.custmr_id ON dbo.EXTRNL_ORG.extrnl_org_id = dbo.PREM_ADJ.brkr_id INNER JOIN
                      dbo.INT_ORG ON dbo.PREM_ADJ.bu_office_id = dbo.INT_ORG.int_org_id ON 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_perd_id = dbo.PREM_ADJ_PERD.prem_adj_perd_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.custmr_id = dbo.PREM_ADJ_PERD.custmr_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.PREM_ADJ_PERD.prem_adj_pgm_id
                      INNER JOIN LKUP COMP_CODE ON dbo.CUSTMR.company_cd = COMP_CODE.lkup_id
                      INNER JOIN LKUP CURR_CODE ON dbo.CUSTMR.currency_cd = CURR_CODE.lkup_id
WHERE PREM_ADJ.PREM_ADJ_ID in (select distinct prev_adj_id from #prevadjid_Revision_Old) order by State

UPDATE #tmp_Previous_CESAR_Revision_Old set #tmp_Previous_CESAR_Revision_Old.[INVOICE NUMBER]= (select top 1 [INVOICE NUMBER] from #tmp_Current_CESAR_Revision_Old
  where #tmp_Previous_CESAR_Revision_Old.[PREM ADJ PGM ID] = #tmp_Current_CESAR_Revision_Old.[PREM ADJ PGM ID] 
  and #tmp_Previous_CESAR_Revision_Old.CustmrID = #tmp_Current_CESAR_Revision_Old.CustmrID),
  #tmp_Previous_CESAR_Revision_Old.[INVOICE DATE]= (select top 1 [INVOICE DATE] from #tmp_Current_CESAR_Revision_Old
  where #tmp_Previous_CESAR_Revision_Old.[PREM ADJ PGM ID] = #tmp_Current_CESAR_Revision_Old.[PREM ADJ PGM ID] 
  and #tmp_Previous_CESAR_Revision_Old.CustmrID = #tmp_Current_CESAR_Revision_Old.CustmrID),
  #tmp_Previous_CESAR_Revision_Old.[ADJUSTMENT NUMBER]= (select top 1 [ADJUSTMENT NUMBER] from #tmp_Current_CESAR_Revision_Old
  where #tmp_Previous_CESAR_Revision_Old.[PREM ADJ PGM ID] = #tmp_Current_CESAR_Revision_Old.[PREM ADJ PGM ID] 
  and #tmp_Previous_CESAR_Revision_Old.CustmrID = #tmp_Current_CESAR_Revision_Old.CustmrID)

---New results
(SELECT [INVOICE NUMBER], [INVOICE DATE], [PROGRAM PERIOD], [VALUATION DATE], [INSURED NAME],[BU/OFFICE],[BROKER ID],BROKER, [ADJUSTMENT TYPE],
  [ADJUSTMENT NUMBER],[PREM ADJ ID],[PREM ADJ PGM ID], POLICYID, [POLICY NUMBER],[POLICY PERIOD],[State],[Retro Result],Previous, ISRevised,
  CompanyCode,CurrencyCode from #tmp_Current_CESAR_Revision_New
UNION ALL
  SELECT [INVOICE NUMBER], [INVOICE DATE], [PROGRAM PERIOD], [VALUATION DATE], [INSURED NAME],[BU/OFFICE],[BROKER ID],BROKER, [ADJUSTMENT TYPE],
  [ADJUSTMENT NUMBER],[PREM ADJ ID],a.[PREM ADJ PGM ID], a.POLICYID, [POLICY NUMBER],[POLICY PERIOD],[State],[Retro Result],Previous, ISRevised,
  CompanyCode,CurrencyCode from #tmp_Previous_CESAR_Revision_New a inner join (select policyid,[prem adj pgm id],stateid from #tmp_Previous_CESAR_Revision_New
except select policyid,[prem adj pgm id],stateid from #tmp_Current_CESAR_Revision_New 
INTERSECT 
  SELECT policyid,[prem adj pgm id],stateid from #tmp_Previous_CESAR_Revision_New)b 
  on a.policyid=b.policyid and a.[prem adj pgm id]=b.[prem adj pgm id] and a.stateid=b.stateid
  and a.policyid  in (select #tmp_Current_CESAR_Revision_New.policyid from #tmp_Current_CESAR_Revision_New))
UNION ALL
-----Old Results
(SELECT [INVOICE NUMBER], [INVOICE DATE], [PROGRAM PERIOD], [VALUATION DATE], [INSURED NAME],[BU/OFFICE],[BROKER ID],BROKER, [ADJUSTMENT TYPE],
  [ADJUSTMENT NUMBER],[PREM ADJ ID],[PREM ADJ PGM ID], POLICYID, [POLICY NUMBER],[POLICY PERIOD],[State],[Retro Result],Previous, ISRevised,
  CompanyCode,CurrencyCode from #tmp_Current_CESAR_Revision_Old
UNION ALL
  SELECT [INVOICE NUMBER], [INVOICE DATE], [PROGRAM PERIOD], [VALUATION DATE], [INSURED NAME],[BU/OFFICE],[BROKER ID],BROKER, [ADJUSTMENT TYPE],
  [ADJUSTMENT NUMBER],[PREM ADJ ID],a.[PREM ADJ PGM ID], a.POLICYID, [POLICY NUMBER],[POLICY PERIOD],[State],[Retro Result],Previous, ISRevised,
  CompanyCode,CurrencyCode from #tmp_Previous_CESAR_Revision_Old a inner join (select policyid,[prem adj pgm id],stateid from #tmp_Previous_CESAR_Revision_Old
except select policyid,[prem adj pgm id],stateid from #tmp_Current_CESAR_Revision_Old 
INTERSECT 
  SELECT policyid,[prem adj pgm id],stateid from #tmp_Previous_CESAR_Revision_Old)b 
  on a.policyid=b.policyid and a.[prem adj pgm id]=b.[prem adj pgm id] and a.stateid=b.stateid
  and a.policyid  in (select #tmp_Current_CESAR_Revision_Old.policyid from #tmp_Current_CESAR_Revision_Old))

drop table #tmp_Current_CESAR_Revision_New
drop table #tmp2_Revision_New
drop table #prevadjid_Revision_New
drop table #tmp_Previous_CESAR_Revision_New

drop table #tmp_Current_CESAR_Revision_Old
drop table #tmp2_Revision_Old
drop table #prevadjid_Revision_Old
drop table #tmp_Previous_CESAR_Revision_Old

END
ELSE If (@OldAdjID>0) and (@IsVoid=1) and (@FnlInvNbr='RTV')-- -------------------------------Voided------------------------
BEGIN

SELECT CASE @FLAG WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END
                       AS [INVOICE NUMBER], CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) WHEN 2 THEN CONVERT(NVARCHAR(30), 
                      FNL_INVC_DT, 101) ELSE CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) END AS [INVOICE DATE], CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) 
                      + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) AS [PROGRAM PERIOD], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 
                      101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], dbo.INT_ORG.full_name + '/' + dbo.INT_ORG.city_nm AS [BU/OFFICE], 
                      dbo.PREM_ADJ_PGM.brkr_id AS [BROKER ID], dbo.EXTRNL_ORG.full_name AS BROKER, 
          LKUP_AdjTyp.lkup_txt AS [ADJUSTMENT TYPE], 
                      dbo.PREM_ADJ_PERD.adj_nbr_txt AS [ADJUSTMENT NUMBER], dbo.PREM_ADJ.prem_adj_id AS [PREM ADJ ID], 
                      dbo.PREM_ADJ_PERD.prem_adj_pgm_id AS [PREM ADJ PGM ID], dbo.COML_AGMT.coml_agmt_id AS PolicyID, PREM_ADJ_RETRO_DTL.St_id as StateID,prem_adj_perd.custmr_id as CustmrID,
                      --If Program Type is DEP then bring DEP Policy Number
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicy(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
                      RTRIM(dbo.COML_AGMT.pol_sym_txt) + ' ' + RTRIM(dbo.COML_AGMT.pol_nbr_txt) + '-' + dbo.COML_AGMT.pol_modulus_txt END AS [POLICY NUMBER], 
                      --If Program Type is DEP then bring DEP Policy Period
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicyPeriod(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
                      CONVERT(NVARCHAR(30), dbo.COML_AGMT.pol_eff_dt, 101) + ' - ' + CONVERT(NVARCHAR(30), dbo.COML_AGMT.planned_end_date, 101) END AS [POLICY PERIOD],                       
                      --If Program Type is DEP then bring DEP Policy State
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicyState(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
       LKUP_St.lkup_txt END AS State,                       
                      CASE WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='ERP' THEN 
       (isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0)) 
       WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='Max' THEN
       (isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)  
       - (isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0)*       
       [dbo].[fn_GetCesarRatioForMax](PREM_ADJ_RETRO_DTL.prem_adj_id,
       PREM_ADJ_RETRO_DTL.custmr_id,PREM_ADJ_RETRO_DTL.prem_adj_pgm_id))
       - isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0))
       WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='Min' THEN
       ((isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0))
       -isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0))  END * -1 AS [Retro Result],              
       (isnull(dbo.[fn_GetPrevERPMinMaxCalValue](PREM_ADJ_RETRO_DTL.prem_adj_id, PREM_ADJ_RETRO_DTL.coml_agmt_id,
        PREM_ADJ_RETRO_DTL.st_id,PREM_ADJ_RETRO_DTL.CUSTMR_ID,PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID),0))* -1 AS Previous,'False' as IsRevised,
        COMP_CODE.lkup_txt AS CompanyCode,
        CURR_CODE.lkup_txt AS CurrencyCode
Into #tmp_Current_CESAR_Void 
FROM         dbo.LKUP AS LKUP_St INNER JOIN
                      dbo.PREM_ADJ_RETRO_DTL INNER JOIN
                      dbo.COML_AGMT ON dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.COML_AGMT.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.custmr_id = dbo.COML_AGMT.custmr_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.coml_agmt_id = dbo.COML_AGMT.coml_agmt_id ON LKUP_St.lkup_id = dbo.PREM_ADJ_RETRO_DTL.st_id INNER JOIN
                      dbo.LKUP AS LKUP_AdjTyp INNER JOIN
                      dbo.PREM_ADJ_PGM ON LKUP_AdjTyp.lkup_id = dbo.PREM_ADJ_PGM.paid_incur_typ_id INNER JOIN
                      dbo.LKUP AS PGMTYP ON dbo.PREM_ADJ_PGM.pgm_typ_id = PGMTYP.lkup_id ON 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.PREM_ADJ_PGM.prem_adj_pgm_id INNER JOIN
                      dbo.EXTRNL_ORG INNER JOIN
                      dbo.PREM_ADJ INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id AND 
                      dbo.PREM_ADJ.reg_custmr_id = dbo.PREM_ADJ_PERD.reg_custmr_id INNER JOIN
                      dbo.CUSTMR ON dbo.PREM_ADJ_PERD.custmr_id = dbo.CUSTMR.custmr_id ON dbo.EXTRNL_ORG.extrnl_org_id = dbo.PREM_ADJ.brkr_id INNER JOIN
                      dbo.INT_ORG ON dbo.PREM_ADJ.bu_office_id = dbo.INT_ORG.int_org_id ON 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_perd_id = dbo.PREM_ADJ_PERD.prem_adj_perd_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.custmr_id = dbo.PREM_ADJ_PERD.custmr_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.PREM_ADJ_PERD.prem_adj_pgm_id
                      INNER JOIN LKUP COMP_CODE ON dbo.CUSTMR.company_cd = COMP_CODE.lkup_id
                      INNER JOIN LKUP CURR_CODE ON dbo.CUSTMR.currency_cd = CURR_CODE.lkup_id
WHERE PREM_ADJ.PREM_ADJ_ID =  @AdjNO order by State

select [PREM ADJ ID],CustmrID,[PREM ADJ PGM ID] into #tmp2_Void from #tmp_Current_CESAR_Void

create table #prevadjid_Void (prev_adj_id int)
 
   declare @adjid_Void int
   declare @custid_Void int
   declare @pgmid_Void int
   DECLARE @Prev_Adj_id_Void int
   declare @prem_adj_id_cur_Void CURSOR
   SET @prem_adj_id_cur_Void = cursor LOCAL FAST_FORWARD READ_ONLY FOR select distinct [PREM ADJ ID],CustmrID,[PREM ADJ PGM ID] from #tmp2_Void
   open @prem_adj_id_cur_Void 
   fetch @prem_adj_id_cur_Void into @adjid_Void, @custid_Void, @pgmid_Void
   WHILE @@FETCH_STATUS = 0
   BEGIN  
    SET @Prev_Adj_id_Void=dbo.fn_DeterminePrevValidAdj_Premium(@adjid_Void,@custid_Void,@pgmid_Void)

    if @Prev_Adj_id_Void>0 
    Begin
     insert into #prevadjid_Void values (@Prev_Adj_id_Void)
     Set @Prev_Adj_id_Void=0
    END

   FETCH NEXT FROM @prem_adj_id_cur_Void INTO  @adjid_Void, @custid_Void, @pgmid_Void
   END
   CLOSE @prem_adj_id_cur_Void
   DEALLOCATE @prem_adj_id_cur_Void
   
SELECT CASE @FLAG WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END
                       AS [INVOICE NUMBER], CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) WHEN 2 THEN CONVERT(NVARCHAR(30), 
                      FNL_INVC_DT, 101) ELSE CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) END AS [INVOICE DATE], CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) 
                      + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) AS [PROGRAM PERIOD], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 
                      101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], dbo.INT_ORG.full_name + '/' + dbo.INT_ORG.city_nm AS [BU/OFFICE], 
                      dbo.PREM_ADJ_PGM.brkr_id AS [BROKER ID], dbo.EXTRNL_ORG.full_name AS BROKER, 
          LKUP_AdjTyp.lkup_txt AS [ADJUSTMENT TYPE], 
                      dbo.PREM_ADJ_PERD.adj_nbr_txt AS [ADJUSTMENT NUMBER], @AdjNO AS [PREM ADJ ID], 
                      dbo.PREM_ADJ_PERD.prem_adj_pgm_id AS [PREM ADJ PGM ID], dbo.COML_AGMT.coml_agmt_id AS PolicyID, PREM_ADJ_RETRO_DTL.St_id as StateID,prem_adj_perd.custmr_id as CustmrID,
                      --If Program Type is DEP then bring DEP Policy Number
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicy(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
                      RTRIM(dbo.COML_AGMT.pol_sym_txt) + ' ' + RTRIM(dbo.COML_AGMT.pol_nbr_txt) + '-' + dbo.COML_AGMT.pol_modulus_txt END AS [POLICY NUMBER], 
                      --If Program Type is DEP then bring DEP Policy Period
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicyPeriod(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
                      CONVERT(NVARCHAR(30), dbo.COML_AGMT.pol_eff_dt, 101) + ' - ' + CONVERT(NVARCHAR(30), dbo.COML_AGMT.planned_end_date, 101) END AS [POLICY PERIOD],                       
                      --If Program Type is DEP then bring DEP Policy State
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicyState(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
       LKUP_St.lkup_txt END AS State,                       
                      0 AS [Retro Result],              
       CASE WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='ERP' THEN 
       (isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0)) 
       WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='Max' THEN
       (isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)  
       - (isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0)*       
       [dbo].[fn_GetCesarRatioForMax](PREM_ADJ_RETRO_DTL.prem_adj_id,
       PREM_ADJ_RETRO_DTL.custmr_id,PREM_ADJ_RETRO_DTL.prem_adj_pgm_id))
       - isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0))
       WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='Min' THEN
       ((isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0))
       -isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0)) END * -1 AS Previous,'False' as IsRevised,
       COMP_CODE.lkup_txt AS CompanyCode,
       CURR_CODE.lkup_txt AS CurrencyCode
Into #tmp_Previous_CESAR_Void 
FROM         dbo.LKUP AS LKUP_St INNER JOIN
                      dbo.PREM_ADJ_RETRO_DTL INNER JOIN
                      dbo.COML_AGMT ON dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.COML_AGMT.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.custmr_id = dbo.COML_AGMT.custmr_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.coml_agmt_id = dbo.COML_AGMT.coml_agmt_id ON LKUP_St.lkup_id = dbo.PREM_ADJ_RETRO_DTL.st_id INNER JOIN
                      dbo.LKUP AS LKUP_AdjTyp INNER JOIN
                      dbo.PREM_ADJ_PGM ON LKUP_AdjTyp.lkup_id = dbo.PREM_ADJ_PGM.paid_incur_typ_id INNER JOIN
                      dbo.LKUP AS PGMTYP ON dbo.PREM_ADJ_PGM.pgm_typ_id = PGMTYP.lkup_id ON 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.PREM_ADJ_PGM.prem_adj_pgm_id INNER JOIN
                      dbo.EXTRNL_ORG INNER JOIN
                      dbo.PREM_ADJ INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id AND 
                      dbo.PREM_ADJ.reg_custmr_id = dbo.PREM_ADJ_PERD.reg_custmr_id INNER JOIN
                      dbo.CUSTMR ON dbo.PREM_ADJ_PERD.custmr_id = dbo.CUSTMR.custmr_id ON dbo.EXTRNL_ORG.extrnl_org_id = dbo.PREM_ADJ.brkr_id INNER JOIN
                      dbo.INT_ORG ON dbo.PREM_ADJ.bu_office_id = dbo.INT_ORG.int_org_id ON 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_perd_id = dbo.PREM_ADJ_PERD.prem_adj_perd_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.custmr_id = dbo.PREM_ADJ_PERD.custmr_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.PREM_ADJ_PERD.prem_adj_pgm_id
                      INNER JOIN LKUP COMP_CODE ON dbo.CUSTMR.company_cd = COMP_CODE.lkup_id
                      INNER JOIN LKUP CURR_CODE ON dbo.CUSTMR.currency_cd = CURR_CODE.lkup_id
WHERE PREM_ADJ.PREM_ADJ_ID in (select distinct prev_adj_id from #prevadjid_Void) order by State

UPDATE #tmp_Previous_CESAR_Void set #tmp_Previous_CESAR_Void.[INVOICE NUMBER]= (select top 1 [INVOICE NUMBER] from #tmp_Current_CESAR_Void
  where #tmp_Previous_CESAR_Void.[PREM ADJ PGM ID] = #tmp_Current_CESAR_Void.[PREM ADJ PGM ID] 
  and #tmp_Previous_CESAR_Void.CustmrID = #tmp_Current_CESAR_Void.CustmrID),
  #tmp_Previous_CESAR_Void.[INVOICE DATE]= (select top 1 [INVOICE DATE] from #tmp_Current_CESAR_Void
  where #tmp_Previous_CESAR_Void.[PREM ADJ PGM ID] = #tmp_Current_CESAR_Void.[PREM ADJ PGM ID] 
  and #tmp_Previous_CESAR_Void.CustmrID = #tmp_Current_CESAR_Void.CustmrID),
  #tmp_Previous_CESAR_Void.[ADJUSTMENT NUMBER]= (select top 1 [ADJUSTMENT NUMBER] from #tmp_Current_CESAR_Void
  where #tmp_Previous_CESAR_Void.[PREM ADJ PGM ID] = #tmp_Current_CESAR_Void.[PREM ADJ PGM ID] 
  and #tmp_Previous_CESAR_Void.CustmrID = #tmp_Current_CESAR_Void.CustmrID)

SELECT [INVOICE NUMBER], [INVOICE DATE], [PROGRAM PERIOD], [VALUATION DATE], [INSURED NAME],[BU/OFFICE],[BROKER ID],BROKER, [ADJUSTMENT TYPE],
  [ADJUSTMENT NUMBER],[PREM ADJ ID],[PREM ADJ PGM ID], POLICYID, [POLICY NUMBER],[POLICY PERIOD],[State],[Retro Result],Previous, ISRevised,
  CompanyCode,CurrencyCode from #tmp_Current_CESAR_Void
-- StateID, CustmrID,
UNION ALL 
  SELECT [INVOICE NUMBER], [INVOICE DATE], [PROGRAM PERIOD], [VALUATION DATE], [INSURED NAME],[BU/OFFICE],[BROKER ID],BROKER, [ADJUSTMENT TYPE],
  [ADJUSTMENT NUMBER],[PREM ADJ ID],a.[PREM ADJ PGM ID], a.POLICYID, [POLICY NUMBER],[POLICY PERIOD],[State],[Retro Result],Previous, ISRevised,
  CompanyCode,CurrencyCode from #tmp_Previous_CESAR_Void a inner join (select policyid,[prem adj pgm id],stateid from #tmp_Previous_CESAR_Void
except select policyid,[prem adj pgm id],stateid from #tmp_Current_CESAR_Void 
INTERSECT 
  SELECT policyid,[prem adj pgm id],stateid from #tmp_Previous_CESAR_Void)b 
  on a.policyid=b.policyid and a.[prem adj pgm id]=b.[prem adj pgm id] and a.stateid=b.stateid
  and a.policyid  in (select #tmp_Current_CESAR_Void.policyid from #tmp_Current_CESAR_Void)

drop table #tmp_Current_CESAR_Void
drop table #tmp2_Void
drop table #prevadjid_Void
drop table #tmp_Previous_CESAR_Void


END
ELSE --If (@OldAdjID=0) ---------------------------------------------------------------------------------------Initial-------
BEGIN

SELECT CASE @FLAG WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END
                       AS [INVOICE NUMBER], CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) WHEN 2 THEN CONVERT(NVARCHAR(30), 
                      FNL_INVC_DT, 101) ELSE CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) END AS [INVOICE DATE], CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) 
                      + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) AS [PROGRAM PERIOD], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 
                      101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], dbo.INT_ORG.full_name + '/' + dbo.INT_ORG.city_nm AS [BU/OFFICE], 
                      dbo.PREM_ADJ_PGM.brkr_id AS [BROKER ID], dbo.EXTRNL_ORG.full_name AS BROKER, 
          LKUP_AdjTyp.lkup_txt AS [ADJUSTMENT TYPE], 
                      dbo.PREM_ADJ_PERD.adj_nbr_txt AS [ADJUSTMENT NUMBER], dbo.PREM_ADJ.prem_adj_id AS [PREM ADJ ID], 
                      dbo.PREM_ADJ_PERD.prem_adj_pgm_id AS [PREM ADJ PGM ID], dbo.COML_AGMT.coml_agmt_id AS PolicyID, PREM_ADJ_RETRO_DTL.St_id as StateID,prem_adj_perd.custmr_id as CustmrID,
                      --If Program Type is DEP then bring DEP Policy Number
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicy(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
                      RTRIM(dbo.COML_AGMT.pol_sym_txt) + ' ' + RTRIM(dbo.COML_AGMT.pol_nbr_txt) + '-' + dbo.COML_AGMT.pol_modulus_txt END AS [POLICY NUMBER], 
                      --If Program Type is DEP then bring DEP Policy Period
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicyPeriod(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
                      CONVERT(NVARCHAR(30), dbo.COML_AGMT.pol_eff_dt, 101) + ' - ' + CONVERT(NVARCHAR(30), dbo.COML_AGMT.planned_end_date, 101) END AS [POLICY PERIOD],                       
                      --If Program Type is DEP then bring DEP Policy State
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicyState(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
       LKUP_St.lkup_txt END AS State,                       
                      CASE WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='ERP' THEN 
       (isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0)) 
       WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='Max' THEN
       (isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)  
       - (isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0)*       
       [dbo].[fn_GetCesarRatioForMax](PREM_ADJ_RETRO_DTL.prem_adj_id,
       PREM_ADJ_RETRO_DTL.custmr_id,PREM_ADJ_RETRO_DTL.prem_adj_pgm_id))
       - isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0))
       WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='Min' THEN
       ((isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0))
       -isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0)) END AS [Retro Result],
        isnull(dbo.[fn_GetPrevERPMinMaxCalValue](PREM_ADJ_RETRO_DTL.prem_adj_id, PREM_ADJ_RETRO_DTL.coml_agmt_id, PREM_ADJ_RETRO_DTL.st_id,PREM_ADJ_RETRO_DTL.CUSTMR_ID,PREM_ADJ_RETRO_DTL.PREM_ADJ_PGM_ID),0)
        AS Previous,'False' as IsRevised,
        COMP_CODE.lkup_txt AS CompanyCode,
        CURR_CODE.lkup_txt AS CurrencyCode
Into #tmp_Current_CESAR       
FROM         dbo.LKUP AS LKUP_St INNER JOIN
                      dbo.PREM_ADJ_RETRO_DTL INNER JOIN
                      dbo.COML_AGMT ON dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.COML_AGMT.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.custmr_id = dbo.COML_AGMT.custmr_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.coml_agmt_id = dbo.COML_AGMT.coml_agmt_id ON LKUP_St.lkup_id = dbo.PREM_ADJ_RETRO_DTL.st_id INNER JOIN
                      dbo.LKUP AS LKUP_AdjTyp INNER JOIN
                      dbo.PREM_ADJ_PGM ON LKUP_AdjTyp.lkup_id = dbo.PREM_ADJ_PGM.paid_incur_typ_id INNER JOIN
                      dbo.LKUP AS PGMTYP ON dbo.PREM_ADJ_PGM.pgm_typ_id = PGMTYP.lkup_id ON 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.PREM_ADJ_PGM.prem_adj_pgm_id INNER JOIN
                      dbo.EXTRNL_ORG INNER JOIN
                      dbo.PREM_ADJ INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id AND 
                      dbo.PREM_ADJ.reg_custmr_id = dbo.PREM_ADJ_PERD.reg_custmr_id INNER JOIN
                      dbo.CUSTMR ON dbo.PREM_ADJ_PERD.custmr_id = dbo.CUSTMR.custmr_id ON dbo.EXTRNL_ORG.extrnl_org_id = dbo.PREM_ADJ.brkr_id INNER JOIN
                      dbo.INT_ORG ON dbo.PREM_ADJ.bu_office_id = dbo.INT_ORG.int_org_id ON 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_perd_id = dbo.PREM_ADJ_PERD.prem_adj_perd_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.custmr_id = dbo.PREM_ADJ_PERD.custmr_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.PREM_ADJ_PERD.prem_adj_pgm_id
                      INNER JOIN LKUP COMP_CODE ON dbo.CUSTMR.company_cd = COMP_CODE.lkup_id
                      INNER JOIN LKUP CURR_CODE ON dbo.CUSTMR.currency_cd = CURR_CODE.lkup_id
WHERE PREM_ADJ.PREM_ADJ_ID =  @AdjNO order by State

select [PREM ADJ ID],CustmrID,[PREM ADJ PGM ID] into #tmp2 from #tmp_Current_CESAR

create table #prevadjid (prev_adj_id int)
 
   declare @adjid int
   declare @custid int
   declare @pgmid int
   DECLARE @Prev_Adj_id int
   declare @prem_adj_id_cur CURSOR
   SET @prem_adj_id_cur = cursor LOCAL FAST_FORWARD READ_ONLY FOR select distinct [PREM ADJ ID],CustmrID,[PREM ADJ PGM ID] from #tmp2
   open @prem_adj_id_cur 
   fetch @prem_adj_id_cur into @adjid, @custid, @pgmid
   WHILE @@FETCH_STATUS = 0
   BEGIN  
    SET @Prev_Adj_id=dbo.fn_DeterminePrevValidAdj_Premium(@adjid,@custid,@pgmid)

    if @Prev_Adj_id>0 
    Begin
     insert into #prevadjid values (@Prev_Adj_id)
     Set @Prev_Adj_id=0
    END

   FETCH NEXT FROM @prem_adj_id_cur INTO  @adjid, @custid, @pgmid
   END
   CLOSE @prem_adj_id_cur
   DEALLOCATE @prem_adj_id_cur
   
SELECT CASE @FLAG WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END
                       AS [INVOICE NUMBER], CASE @FLAG WHEN 1 THEN CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) WHEN 2 THEN CONVERT(NVARCHAR(30), 
                      FNL_INVC_DT, 101) ELSE CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) END AS [INVOICE DATE], CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) 
                      + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) AS [PROGRAM PERIOD], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 
                      101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], dbo.INT_ORG.full_name + '/' + dbo.INT_ORG.city_nm AS [BU/OFFICE], 
                      dbo.PREM_ADJ_PGM.brkr_id AS [BROKER ID], dbo.EXTRNL_ORG.full_name AS BROKER, 
          LKUP_AdjTyp.lkup_txt AS [ADJUSTMENT TYPE], 
                      dbo.PREM_ADJ_PERD.adj_nbr_txt AS [ADJUSTMENT NUMBER],@AdjNO AS [PREM ADJ ID], 
                      dbo.PREM_ADJ_PERD.prem_adj_pgm_id AS [PREM ADJ PGM ID], dbo.COML_AGMT.coml_agmt_id AS PolicyID, PREM_ADJ_RETRO_DTL.St_id as StateID,prem_adj_perd.custmr_id as CustmrID,
                      --If Program Type is DEP then bring DEP Policy Number
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicy(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
                      RTRIM(dbo.COML_AGMT.pol_sym_txt) + ' ' + RTRIM(dbo.COML_AGMT.pol_nbr_txt) + '-' + dbo.COML_AGMT.pol_modulus_txt END AS [POLICY NUMBER], 
                      --If Program Type is DEP then bring DEP Policy Period
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicyPeriod(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
                      CONVERT(NVARCHAR(30), dbo.COML_AGMT.pol_eff_dt, 101) + ' - ' + CONVERT(NVARCHAR(30), dbo.COML_AGMT.planned_end_date, 101) END AS [POLICY PERIOD],                       
                      --If Program Type is DEP then bring DEP Policy State
                      CASE WHEN substring(PGMTYP.LKUP_TXT,1,3) = 'DEP' THEN dbo.fn_GetDEPMasterPolicyState(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PGM_ID) ELSE
       LKUP_St.lkup_txt END AS State,                       
                      0 AS [Retro Result],              
       CASE WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='ERP' THEN 
       (isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0)) 
       WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='Max' THEN
       (isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)  
       - (isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0)*       
       [dbo].[fn_GetCesarRatioForMax](PREM_ADJ_RETRO_DTL.prem_adj_id,
       PREM_ADJ_RETRO_DTL.custmr_id,PREM_ADJ_RETRO_DTL.prem_adj_pgm_id))
       - isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0))
       WHEN PREM_ADJ_PERD.ernd_retro_prem_min_max_cd ='Min' THEN
       ((isnull(dbo.PREM_ADJ_RETRO_DTL.cesar_cd_tot_amt,0)-isnull(dbo.PREM_ADJ_RETRO_DTL.adj_dedtbl_wrk_comp_los_amt,0))
       -isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0)) END AS Previous,'False' as IsRevised,
       COMP_CODE.lkup_txt AS CompanyCode,
       CURR_CODE.lkup_txt AS CurrencyCode
Into #tmp_Previous_CESAR       
FROM         dbo.LKUP AS LKUP_St INNER JOIN      dbo.PREM_ADJ_RETRO_DTL INNER JOIN
                      dbo.COML_AGMT ON dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.COML_AGMT.prem_adj_pgm_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.custmr_id = dbo.COML_AGMT.custmr_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.coml_agmt_id = dbo.COML_AGMT.coml_agmt_id ON LKUP_St.lkup_id = dbo.PREM_ADJ_RETRO_DTL.st_id INNER JOIN
                      dbo.LKUP AS LKUP_AdjTyp INNER JOIN
                      dbo.PREM_ADJ_PGM ON LKUP_AdjTyp.lkup_id = dbo.PREM_ADJ_PGM.paid_incur_typ_id INNER JOIN
                      dbo.LKUP AS PGMTYP ON dbo.PREM_ADJ_PGM.pgm_typ_id = PGMTYP.lkup_id ON 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.PREM_ADJ_PGM.prem_adj_pgm_id INNER JOIN
                      dbo.EXTRNL_ORG INNER JOIN    dbo.PREM_ADJ INNER JOIN
                      dbo.PREM_ADJ_PERD ON dbo.PREM_ADJ.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id AND 
                      dbo.PREM_ADJ.reg_custmr_id = dbo.PREM_ADJ_PERD.reg_custmr_id INNER JOIN
                      dbo.CUSTMR ON dbo.PREM_ADJ_PERD.custmr_id = dbo.CUSTMR.custmr_id ON dbo.EXTRNL_ORG.extrnl_org_id = dbo.PREM_ADJ.brkr_id INNER JOIN
                      dbo.INT_ORG ON dbo.PREM_ADJ.bu_office_id = dbo.INT_ORG.int_org_id ON 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_perd_id = dbo.PREM_ADJ_PERD.prem_adj_perd_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_id = dbo.PREM_ADJ_PERD.prem_adj_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.custmr_id = dbo.PREM_ADJ_PERD.custmr_id AND 
                      dbo.PREM_ADJ_RETRO_DTL.prem_adj_pgm_id = dbo.PREM_ADJ_PERD.prem_adj_pgm_id
                      INNER JOIN LKUP COMP_CODE ON dbo.CUSTMR.company_cd = COMP_CODE.lkup_id
                      INNER JOIN LKUP CURR_CODE ON dbo.CUSTMR.currency_cd = CURR_CODE.lkup_id
WHERE PREM_ADJ.PREM_ADJ_ID in (select distinct prev_adj_id from #prevadjid) order by State
   
UPDATE #tmp_Previous_CESAR set #tmp_Previous_CESAR.[INVOICE NUMBER]= (select top 1 [INVOICE NUMBER] from #tmp_Current_CESAR
  where #tmp_Previous_CESAR.[PREM ADJ PGM ID] = #tmp_Current_CESAR.[PREM ADJ PGM ID] 
  and #tmp_Previous_CESAR.CustmrID = #tmp_Current_CESAR.CustmrID),
  #tmp_Previous_CESAR.[INVOICE DATE]= (select top 1 [INVOICE DATE] from #tmp_Current_CESAR
  where #tmp_Previous_CESAR.[PREM ADJ PGM ID] = #tmp_Current_CESAR.[PREM ADJ PGM ID] 
  and #tmp_Previous_CESAR.CustmrID = #tmp_Current_CESAR.CustmrID),
  #tmp_Previous_CESAR.[ADJUSTMENT NUMBER]= (select top 1 [ADJUSTMENT NUMBER] from #tmp_Current_CESAR
  where #tmp_Previous_CESAR.[PREM ADJ PGM ID] = #tmp_Current_CESAR.[PREM ADJ PGM ID] 
  and #tmp_Previous_CESAR.CustmrID = #tmp_Current_CESAR.CustmrID)

SELECT [INVOICE NUMBER], [INVOICE DATE], [PROGRAM PERIOD], [VALUATION DATE], [INSURED NAME],[BU/OFFICE],[BROKER ID],BROKER, [ADJUSTMENT TYPE],
  [ADJUSTMENT NUMBER],[PREM ADJ ID],[PREM ADJ PGM ID], POLICYID, [POLICY NUMBER],[POLICY PERIOD],[State],[Retro Result],Previous, ISRevised,
   CompanyCode,CurrencyCode from #tmp_Current_CESAR
-- StateID, CustmrID,
UNION ALL
  SELECT [INVOICE NUMBER], [INVOICE DATE], [PROGRAM PERIOD], [VALUATION DATE], [INSURED NAME],[BU/OFFICE],[BROKER ID],BROKER, [ADJUSTMENT TYPE],
  [ADJUSTMENT NUMBER],[PREM ADJ ID],a.[PREM ADJ PGM ID], a.POLICYID, [POLICY NUMBER],[POLICY PERIOD],[State],[Retro Result],Previous, ISRevised,
  CompanyCode,CurrencyCode from #tmp_Previous_CESAR a inner join (select policyid,[prem adj pgm id],stateid from #tmp_Previous_CESAR
except select policyid,[prem adj pgm id],stateid from #tmp_Current_CESAR 
INTERSECT 
  SELECT policyid,[prem adj pgm id],stateid from #tmp_Previous_CESAR)b 
  on a.policyid=b.policyid and a.[prem adj pgm id]=b.[prem adj pgm id] and a.stateid=b.stateid
  and a.policyid  in (select #tmp_Current_CESAR.policyid from #tmp_Current_CESAR)

drop table #tmp_Current_CESAR
drop table #tmp2
drop table #prevadjid
drop table #tmp_Previous_CESAR

END--------------------------------------------------------------------------------------------------------------Initial

END TRY
BEGIN CATCH

 SELECT 
    ERROR_NUMBER() AS ERRORNUMBER,
    ERROR_SEVERITY() AS ERRORSEVERITY,
    ERROR_STATE() AS ERRORSTATE,
    ERROR_PROCEDURE() AS ERRORPROCEDURE,
    ERROR_LINE() AS ERRORLINE,
    ERROR_MESSAGE() AS ERRORMESSAGE;


END CATCH
END

go

if object_id('GetCesarCodingWorksheet') is not null
        print 'Created Procedure GetCesarCodingWorksheet'
else
        print 'Failed Creating Procedure GetCesarCodingWorksheet'
go

if object_id('GetCesarCodingWorksheet') is not null
        grant exec on GetCesarCodingWorksheet to  public
go
