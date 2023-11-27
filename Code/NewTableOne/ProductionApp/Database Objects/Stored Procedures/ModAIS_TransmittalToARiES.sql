
IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'ModAIS_TransmittalToARiES' and TYPE = 'P')
	DROP PROC ModAIS_TransmittalToARiES
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		ModAIS_TransmittalToARiES
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Anil Nandakumar 11/18/2008
-----
-----	Description:	This procedure will populate the ARIES_TRNSMTL_HIST table with all the 
-----					transmittals for a given adjustment
---------------------------------------------------------------------

--Pass revision flag and adjustment ID. Revision flag will indicate if the amounts need to be reversed for a particular 
-- adjustment passed to it.

--1. To be called on calculate.
--2. Call on revision of adjustment invoice
--3. Call on void of adjustment invoice
--4. Call on cancel of adjustment invoice

CREATE PROCEDURE [dbo].[ModAIS_TransmittalToARiES] 
@prem_adj_id int,
@rel_prem_adj_id int,
@err_msg_output varchar(1000) output,
@Ind int -- 1=Normal, 2= Revision, 3= Void, 4= Cancel
AS
BEGIN

-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
SET NOCOUNT ON;

declare @trancount int,
		@Reverse int

set @trancount = @@trancount;

if @trancount = 0
begin
    begin transaction 
end

begin try

	DECLARE
		@nErrorCode		int,
		@sErrorMsg		varchar(100),
		@ID			int

	SET @nErrorCode = 0

		IF (@Ind=1)
			BEGIN
				SET @Reverse = 1
				DELETE  FROM ARIES_TRNSMTL_HIST with(rowlock)WHERE prem_adj_id=@prem_adj_id
			END
		  ELSE IF (@Ind=2)
			BEGIN
				SET @Reverse = -1
			END
		  ELSE IF (@Ind=3)
				SET @Reverse = -1
		  ELSE IF (@Ind=4)
			BEGIN				
				DELETE  FROM ARIES_TRNSMTL_HIST with(rowlock) WHERE prem_adj_id=@prem_adj_id
				goto CancelFlg
			END

--------------------------Start of 11162 Fix---------------------------------------------------------------------------------------------------
------We are calculating cesar coding amounts by  using the following logic and storing the results in temp table named #tmpCesar-----
------Based on the coding vlaues in #tmpCesar table we are sending the postings-------------------------------------------------------
------This will be applicable to :
									--1)Retro. Premium Adjustment - 1500/0111
									--2)Def - Retro Bill Reclass	2000/0108  
									--3)PEO ADJUSTMENTS (Rollup Retro. Premium Adjustment - 1500/0111 to PEO master policy level)
									--4)PEO ADJUSTMENTS (Rollup Def - Retro Bill Reclass	2000/0108 to PEO master policy level)
SELECT CASE 1 WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END
                       AS [INVOICE NUMBER], CASE 1 WHEN 1 THEN CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) WHEN 2 THEN CONVERT(NVARCHAR(30), 
                      FNL_INVC_DT, 101) ELSE CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) END AS [INVOICE DATE], CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) 
                      + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) AS [PROGRAM PERIOD], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 
                      101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], dbo.INT_ORG.full_name + '/' + dbo.INT_ORG.city_nm AS [BU/OFFICE], 
                      dbo.PREM_ADJ_PGM.brkr_id AS [BROKER ID], dbo.EXTRNL_ORG.full_name AS BROKER, 
				      LKUP_AdjTyp.lkup_txt+'('+CASE WHEN PGMTYP.LKUP_TXT LIKE 'NON-DEP%' THEN 'Retro' ELSE PGMTYP.LKUP_TXT END +')' AS [ADJUSTMENT TYPE], 
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
					   AS Previous,'False' as IsRevised
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
WHERE PREM_ADJ.PREM_ADJ_ID =  @prem_adj_id order by State

select [PREM ADJ ID],CustmrID,[PREM ADJ PGM ID] into #tmp2 from #tmp_Current_CESAR

create table #prevadjid	(prev_adj_id int)
	
			declare @adjid int
			declare @custid int
			declare @pgmid int
			DECLARE @Prev_Adj_id int
			declare @prem_adj_id_cur CURSOR
			SET @prem_adj_id_cur = CURSOR FAST_FORWARD FOR select distinct [PREM ADJ ID],CustmrID,[PREM ADJ PGM ID] from #tmp2
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
			
SELECT CASE 1 WHEN 1 THEN PREM_ADJ.DRFT_INVC_NBR_TXT WHEN 2 THEN PREM_ADJ.FNL_INVC_NBR_TXT ELSE PREM_ADJ.DRFT_INVC_NBR_TXT END
                       AS [INVOICE NUMBER], CASE 1 WHEN 1 THEN CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) WHEN 2 THEN CONVERT(NVARCHAR(30), 
                      FNL_INVC_DT, 101) ELSE CONVERT(NVARCHAR(30), DRFT_INVC_DT, 101) END AS [INVOICE DATE], CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.strt_dt, 101) 
                      + '-' + CONVERT(NVARCHAR(30), dbo.PREM_ADJ_PGM.plan_end_dt, 101) AS [PROGRAM PERIOD], CONVERT(NVARCHAR(30), dbo.PREM_ADJ.valn_dt, 
                      101) AS [VALUATION DATE], dbo.CUSTMR.full_nm AS [INSURED NAME], dbo.INT_ORG.full_name + '/' + dbo.INT_ORG.city_nm AS [BU/OFFICE], 
                      dbo.PREM_ADJ_PGM.brkr_id AS [BROKER ID], dbo.EXTRNL_ORG.full_name AS BROKER, 
				      LKUP_AdjTyp.lkup_txt+'('+CASE WHEN PGMTYP.LKUP_TXT LIKE 'NON-DEP%' THEN 'Retro' ELSE PGMTYP.LKUP_TXT END +')' AS [ADJUSTMENT TYPE], 
                      dbo.PREM_ADJ_PERD.adj_nbr_txt AS [ADJUSTMENT NUMBER],@prem_adj_id AS [PREM ADJ ID], 
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
					  -isnull(dbo.PREM_ADJ_RETRO_DTL.std_subj_prem_amt,0)) END AS Previous,'False' as IsRevised
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
		[ADJUSTMENT NUMBER],[PREM ADJ ID],[PREM ADJ PGM ID], POLICYID, [POLICY NUMBER],[POLICY PERIOD],[State],[Retro Result],Previous, [Retro Result]-Previous as "current",ISRevised INTO #tmpCesar from #tmp_Current_CESAR
-- StateID, CustmrID,
UNION ALL
		SELECT [INVOICE NUMBER], [INVOICE DATE], [PROGRAM PERIOD], [VALUATION DATE], [INSURED NAME],[BU/OFFICE],[BROKER ID],BROKER, [ADJUSTMENT TYPE],
		[ADJUSTMENT NUMBER],[PREM ADJ ID],a.[PREM ADJ PGM ID], a.POLICYID, [POLICY NUMBER],[POLICY PERIOD],[State],[Retro Result],Previous, [Retro Result]-Previous as "current",ISRevised from #tmp_Previous_CESAR a inner join (select policyid,[prem adj pgm id],stateid from #tmp_Previous_CESAR 
except select policyid,[prem adj pgm id],stateid from #tmp_Current_CESAR 
INTERSECT 
		SELECT policyid,[prem adj pgm id],stateid from #tmp_Previous_CESAR)b 
		on a.policyid=b.policyid and a.[prem adj pgm id]=b.[prem adj pgm id] and a.stateid=b.stateid
		and a.policyid  in (select #tmp_Current_CESAR.policyid from #tmp_Current_CESAR)

drop table #tmp_Current_CESAR
drop table #tmp2
drop table #prevadjid
drop table #tmp_Previous_CESAR

--select * from #tmpCesar

--drop table #tmpCesar
--------------------------------------End of 11162 Fix------------------------------------------------------------------------------------------------------------------


		--LBA Adjustment - C&RM	- 6000/0106 
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,
			pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,
			sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,	bewar_txt,optxt_txt,
			zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,
			zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,
			prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,
			trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT 
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0  
				THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')
			ELSE CONVERT(char(10), '') END  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), '')  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 					-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 					-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,			-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 			-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, (ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_amt,0) - 
			ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_prev_biled_amt,0)) * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, (ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_amt,0) - 
			ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_prev_biled_amt,0))  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*(ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_amt,0) - 
			ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_prev_biled_amt,0))  < 0) 
				THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22) 
			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(16), '') 
			ELSE 'LSI ' + CONVERT(varchar(12), LSI_CUSTMR.lsi_acct_id) END), 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '') 
			ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END), 		-- OPTXT(69)
			CONVERT(char(3), '') , 			-- ZZPOLSYMBOL(70)
			CONVERT(char(18), '') , 		-- ZZPOLICY(71)
			CONVERT(char(2), '') , 			-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			--(SR 318680)
			CONVERT(char(20), dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,401)),-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'LBA',0,@Ind,@rel_prem_adj_id
	FROM PREM_ADJ INNER JOIN 
		PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
		AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
		PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
		PREM_ADJ_PGM_SETUP ON (PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_ID AND
		PREM_ADJ_PGM_SETUP.incld_ernd_retro_prem_ind=0) INNER JOIN 
		CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN 
		LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id 
		AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
		POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
		PREM_ADJ_PARMET_SETUP ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
		AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
		AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_ID = PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID 
		AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
		AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id)
	WHERE PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=401
		AND POST_TRNS_TYP.POST_TRNS_TYP_ID=17
		AND POST_TRNS_TYP.actv_ind=1
		AND (ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_amt,0) - 
		ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_prev_biled_amt,0)) <> 0
		AND PREM_ADJ.prem_adj_id = @prem_adj_id


		--LBA Deposit	7250/0109 
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,
			pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,
			sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,	bewar_txt,optxt_txt,
			zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,
			zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,
			prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,
			Post_cd,rel_prem_adj_id)
		SELECT 
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0  
				THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')  
			ELSE CONVERT(char(10), '') END  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), '')  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 					-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 					-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,			-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 			-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_depst_amt,0) * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_depst_amt,0)  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*(ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_depst_amt,0))  = 0) 
			THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22) 

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(16), '') 
			ELSE 'LSI ' + CONVERT(varchar(12), LSI_CUSTMR.lsi_acct_id) END), 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '') 
			ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END), 		-- OPTXT(69)
			CONVERT(char(3), '') , 			-- ZZPOLSYMBOL(70)
			CONVERT(char(18), '') , 		-- ZZPOLICY(71)
			CONVERT(char(2), '') , 			-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			--(SR 318680)
			CONVERT(char(20), dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,401)), 		-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'LBA',0,@Ind,@rel_prem_adj_id
	FROM PREM_ADJ INNER JOIN 
		PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
		AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
		PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
		PREM_ADJ_PGM_SETUP ON (PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_ID AND
		PREM_ADJ_PGM_SETUP.incld_ernd_retro_prem_ind=0) INNER JOIN 
		CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN 
		LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
		POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
		PREM_ADJ_PARMET_SETUP ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
		AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
		AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_ID = PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID 
		AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id)
	WHERE PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=401
		AND POST_TRNS_TYP.POST_TRNS_TYP_ID=75
		AND POST_TRNS_TYP.actv_ind=1
		AND ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_depst_amt,0) <> 0
		AND PREM_ADJ.prem_adj_id = @prem_adj_id


		--LBA LF Adjustment - C&RM	7250/0111 
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,
			pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,
			sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,	bewar_txt,optxt_txt,
			zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,
			zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,
			prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,
			Post_cd,rel_prem_adj_id)
		SELECT 
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0  
			THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')  
			ELSE CONVERT(char(10), '') END  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), '')  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 					-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 					-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,			-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 			-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_depst_amt,0) * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_depst_amt,0)  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*(ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_depst_amt,0))  <> 0) 
			THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22) 

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(16), '') 
			ELSE 'LSI ' + CONVERT(varchar(12), LSI_CUSTMR.lsi_acct_id) END), 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '') 
			ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END), 		-- OPTXT(69)
			CONVERT(char(3), '') , 			-- ZZPOLSYMBOL(70)
			CONVERT(char(18), '') , 		-- ZZPOLICY(71)
			CONVERT(char(2), '') , 			-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			--(SR 318680)
			CONVERT(char(20), dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,401)),-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'LBA',0,@Ind,@rel_prem_adj_id
	FROM PREM_ADJ INNER JOIN 
		PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
		AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
		PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
		PREM_ADJ_PGM_SETUP ON (PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_ID AND
		PREM_ADJ_PGM_SETUP.incld_ernd_retro_prem_ind=0) INNER JOIN 
		CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN 
		LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
		POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
		PREM_ADJ_PARMET_SETUP ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
		AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
		AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_ID = PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID 
		AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
		AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id)
	WHERE PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=401
		AND POST_TRNS_TYP.POST_TRNS_TYP_ID=76
		AND POST_TRNS_TYP.actv_ind=1
		AND ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_depst_amt,0) <> 0
		AND PREM_ADJ.prem_adj_id = @prem_adj_id


		--LF/Escrow Adjustment-C&RM	- 7000/0105 
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,
			pygrp_txt,grkey_txt,endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,
			zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,
			prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,
			Post_cd,rel_prem_adj_id)
		SELECT 
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0  
			THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')  
			ELSE CONVERT(char(10), '') END  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), '')  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*PREM_ADJ_PARMET_SETUP.tot_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(16), '') 
			ELSE 'LSI ' + CONVERT(varchar(12), LSI_CUSTMR.lsi_acct_id) END), 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '') 
			ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END), 		-- OPTXT(69)
			CONVERT(char(3), '') , 			-- ZZPOLSYMBOL(70)
			CONVERT(char(18), '') , 		-- ZZPOLICY(71)
			CONVERT(char(2), '') , 			-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			--(SR 318680)
			CONVERT(char(20), dbo.fn_GetPolicyForEscrowAriesPostings(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID)),-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'ESCROW',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN 
			LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id 
			AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_PARMET_SETUP ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id)
		WHERE PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=399
			AND POST_TRNS_TYP.POST_TRNS_TYP_ID=19
			AND POST_TRNS_TYP.actv_ind=1
			AND PREM_ADJ_PARMET_SETUP.tot_amt > 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--LF/Escrow Adjustment-C&RM	7250/0105  
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,
			pygrp_txt,grkey_txt,endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,
			zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,
			prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,
			Post_cd,rel_prem_adj_id)
		SELECT 
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0  
			THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')  
			ELSE CONVERT(char(10), '') END  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), '')  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*PREM_ADJ_PARMET_SETUP.tot_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(16), '') 
			ELSE 'LSI ' + CONVERT(varchar(12), LSI_CUSTMR.lsi_acct_id) END), 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '') 
			ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END), 		-- OPTXT(69)
			CONVERT(char(3), '') , 			-- ZZPOLSYMBOL(70)
			CONVERT(char(18), '') , 		-- ZZPOLICY(71)
			CONVERT(char(2), '') , 			-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			--(SR 318680)
			CONVERT(char(20), dbo.fn_GetPolicyForEscrowAriesPostings(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID)),-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'ESCROW',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN 
			LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id 
			AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_PARMET_SETUP ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id)
		WHERE PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=399
			AND POST_TRNS_TYP.POST_TRNS_TYP_ID=20
			AND POST_TRNS_TYP.actv_ind=1
			AND PREM_ADJ_PARMET_SETUP.tot_amt < 0 -- When the escrow amount is <0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--Loss Fund/Escrow	7250/0101  
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,
			pygrp_txt,grkey_txt,endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,
			zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,
			prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,
			Post_cd,rel_prem_adj_id)
		SELECT 
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0  
			THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')  
			ELSE CONVERT(char(10), '') END  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), '')  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			--Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"
			CASE WHEN (@Reverse*PREM_ADJ_PARMET_SETUP.tot_amt  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(16), '') 
			ELSE 'LSI ' + CONVERT(varchar(12), LSI_CUSTMR.lsi_acct_id) END), 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '') 
			ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END), 		-- OPTXT(69)
			CONVERT(char(3), '') , 			-- ZZPOLSYMBOL(70)
			CONVERT(char(18), '') , 		-- ZZPOLICY(71)
			CONVERT(char(2), '') , 			-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			--(SR 318680)
			CONVERT(char(20), dbo.fn_GetPolicyForEscrowAriesPostings(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID)),-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'ESCROW',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN 
			LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id AND LSI_CUSTMR.actv_ind=1 
			AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_PARMET_SETUP ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id)
		WHERE PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=399
			AND POST_TRNS_TYP.POST_TRNS_TYP_ID=21
			AND POST_TRNS_TYP.actv_ind=1
			AND PREM_ADJ_PARMET_SETUP.tot_amt < 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--Retro. Premium Adjustment - 1500/0111 
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CONVERT(char(10), '')  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_RETRO.aries_tot_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.aries_tot_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*PREM_ADJ_RETRO.aries_tot_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), '') ,	 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), '') ,			-- OPTXT(69)
			CONVERT(char(3), RTRIM(pol_sym_txt)) , -- ZZPOLSYMBOL(70)
			CONVERT(char(18), RTRIM(pol_nbr_txt)) ,-- ZZPOLICY(71)
			CONVERT(char(2), pol_modulus_txt), 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			--CONVERT(char(20), '') , 		-- ZZVTREF4(88)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt), -- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'RETRO',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = COML_AGMT.PREM_ADJ_PGM_id) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID AND CUSTMR.peo_ind=0) INNER JOIN
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_RETRO ON (PREM_ADJ_RETRO.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_RETRO.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id 
			AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO.COML_AGMT_ID) 
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=18
			AND POST_TRNS_TYP.actv_ind=1
			AND COML_AGMT.adj_typ_id IN (63,65,66,67,70,71,72,73) 
			AND PREM_ADJ_RETRO.aries_tot_amt <> 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
			--11162 condition
			AND (select case when count(#tmpCesar."current")>0 then 1 else 0 end from #tmpCesar where #tmpCesar."PREM ADJ ID"=@prem_adj_id and #tmpCesar."PREM ADJ PGM ID"=PREM_ADJ_PGM.PREM_ADJ_PGM_ID and #tmpCesar."POLICYID"=COML_AGMT.COML_AGMT_ID and #tmpCesar."current"<>0)<>0
			

		--Def - Retro Bill Reclass	2000/0108  
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CONVERT(char(10), '')  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_RETRO.aries_tot_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.aries_tot_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			--Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"
			CASE WHEN (@Reverse*PREM_ADJ_RETRO.aries_tot_amt  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), '') ,	 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), '') ,			-- OPTXT(69)
			CONVERT(char(3), RTRIM(COML_AGMT.pol_sym_txt)) ,		-- ZZPOLSYMBOL(70)
			CONVERT(char(18), RTRIM(COML_AGMT.pol_nbr_txt)) ,		-- ZZPOLICY(71)
			CONVERT(char(2), COML_AGMT.pol_modulus_txt), 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  , 		-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'RETRO',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = COML_AGMT.PREM_ADJ_PGM_id) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID AND CUSTMR.peo_ind=0) INNER JOIN
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_RETRO ON (PREM_ADJ_RETRO.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_RETRO.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id 
			AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO.COML_AGMT_ID) 
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=72
			AND POST_TRNS_TYP.actv_ind=1
			AND COML_AGMT.adj_typ_id IN (63,65,66,67,70,71,72,73) 
			AND PREM_ADJ_RETRO.aries_tot_amt <> 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
			--11162 condition
			AND (select case when count(#tmpCesar."current")>0 then 1 else 0 end from #tmpCesar where #tmpCesar."PREM ADJ ID"=@prem_adj_id and #tmpCesar."PREM ADJ PGM ID"=PREM_ADJ_PGM.PREM_ADJ_PGM_ID and #tmpCesar."POLICYID"=COML_AGMT.COML_AGMT_ID and #tmpCesar."current"<>0)<>0
			
		
		--Retro Adjustment POD	2000/0104 
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CONVERT(char(10), '')  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_RETRO.adj_cash_flw_ben_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.adj_cash_flw_ben_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			--Because it is BALANCED ENTRY (re-class), we send -ve amounts when it is actually positive. ">" used instead of "<"
			CASE WHEN (@Reverse*PREM_ADJ_RETRO.adj_cash_flw_ben_amt  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), '') ,	 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), '') ,			-- OPTXT(69)
			CONVERT(char(3), RTRIM(COML_AGMT.pol_sym_txt)) ,		-- ZZPOLSYMBOL(70)
			CONVERT(char(18), RTRIM(COML_AGMT.pol_nbr_txt)) ,		-- ZZPOLICY(71)
			CONVERT(char(2), COML_AGMT.pol_modulus_txt), 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  ,		-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'RETRO',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = COML_AGMT.PREM_ADJ_PGM_id) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID AND CUSTMR.peo_ind=0) INNER JOIN
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_RETRO ON (PREM_ADJ_RETRO.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_RETRO.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id 
			AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO.COML_AGMT_ID) 
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=25
			AND POST_TRNS_TYP.actv_ind=1
			AND COML_AGMT.adj_typ_id IN (70,71,72,73) --ONLY PAID RETRO -- Added 71,72,73 bug 10046
			AND PREM_ADJ_RETRO.adj_cash_flw_ben_amt <> 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--Retro Adjustment POD	2000/0104 (DUE = VALDATE + 1 YEAR)
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CONVERT(char(10), '')  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_RETRO.adj_cash_flw_ben_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.adj_cash_flw_ben_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*PREM_ADJ_RETRO.adj_cash_flw_ben_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), dateadd(YEAR,1,PREM_ADJ.valn_dt), 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), '') ,	 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), '') ,			-- OPTXT(69)
			CONVERT(char(3), RTRIM(COML_AGMT.pol_sym_txt)) ,		-- ZZPOLSYMBOL(70)
			CONVERT(char(18), RTRIM(COML_AGMT.pol_nbr_txt)) ,		-- ZZPOLICY(71)
			CONVERT(char(2), COML_AGMT.pol_modulus_txt), 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  , 		-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'RETRO',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = COML_AGMT.PREM_ADJ_PGM_id) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID AND CUSTMR.peo_ind=0) INNER JOIN
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_RETRO ON (PREM_ADJ_RETRO.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_RETRO.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id 
			AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO.COML_AGMT_ID) 
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=25
			AND POST_TRNS_TYP.actv_ind=1
			AND COML_AGMT.adj_typ_id IN (70,71,72,73) --ONLY PAID RETRO -- Added 71,72,73 bug 10046
			AND PREM_ADJ_RETRO.adj_cash_flw_ben_amt <> 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--WC TPD Adjust - C&RM	6000/0122  
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0  
				THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')  
			ELSE CONVERT(char(10), '') END  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), '')  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_RETRO.post_idnmty_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.post_idnmty_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*PREM_ADJ_RETRO.post_idnmty_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(16), '') 
			ELSE 'LSI ' + CONVERT(varchar(12), LSI_CUSTMR.lsi_acct_id) END), 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '') 
			ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END), 		-- OPTXT(69)
			CONVERT(char(3),  '') ,			-- ZZPOLSYMBOL(70)
			CONVERT(char(18), '') ,			-- ZZPOLICY(71)
			CONVERT(char(2),  '') ,		 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			--(SR 318680)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt),-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'RETRO',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = COML_AGMT.PREM_ADJ_PGM_id) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID AND CUSTMR.peo_ind=0) LEFT JOIN
			LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id 
			AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_RETRO ON (PREM_ADJ_RETRO.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_RETRO.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id 
			AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO.COML_AGMT_ID) 
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=27
			AND POST_TRNS_TYP.actv_ind=1
			AND COML_AGMT.adj_typ_id IN (66,67) 
			AND PREM_ADJ.valn_dt<>isnull((case when datepart(Day,strt_dt) >15
		THEN
		DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,incur_conv_mms_cnt, strt_dt))+1,0))  
		ELSE
		DATEADD(D,-1,DATEADD(mm, DATEDIFF(m,0,dateadd(month,incur_conv_mms_cnt, strt_dt)),0))  
		END),0)
			AND PREM_ADJ_RETRO.post_idnmty_amt <> 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--TPD Clm Res. Req.- WC Adj	6000/0126   
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0  
			THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')  
			ELSE CONVERT(char(10), '') END  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), '')  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_RETRO.post_resrv_idnmty_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.post_resrv_idnmty_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*PREM_ADJ_RETRO.post_resrv_idnmty_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(16), '') 
			ELSE 'LSI ' + CONVERT(varchar(12), LSI_CUSTMR.lsi_acct_id) END), 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '') 
			ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END), 		-- OPTXT(69)
			CONVERT(char(3),  '') ,			-- ZZPOLSYMBOL(70)
			CONVERT(char(18), '') ,			-- ZZPOLICY(71)
			CONVERT(char(2),  '') ,		 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			--(SR 318680)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt), 		-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'RETRO',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = COML_AGMT.PREM_ADJ_PGM_id) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID AND CUSTMR.peo_ind=0) LEFT JOIN
			LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id 
			AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_RETRO ON (PREM_ADJ_RETRO.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_RETRO.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id 
			AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO.COML_AGMT_ID) 
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=28
			AND POST_TRNS_TYP.actv_ind=1
			AND COML_AGMT.adj_typ_id IN (66,67) 
			AND PREM_ADJ_RETRO.post_resrv_idnmty_amt > 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--TPD Clm Reserve Adj-WC	6250/0113    
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0  
			THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')  
			ELSE CONVERT(char(10), '') END  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), '')  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_RETRO.post_resrv_idnmty_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.post_resrv_idnmty_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*PREM_ADJ_RETRO.post_resrv_idnmty_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(16), '') 
			ELSE 'LSI ' + CONVERT(varchar(12), LSI_CUSTMR.lsi_acct_id) END), 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '') 
			ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END), 		-- OPTXT(69)
			CONVERT(char(3),  '') ,			-- ZZPOLSYMBOL(70)
			CONVERT(char(18), '') ,			-- ZZPOLICY(71)
			CONVERT(char(2),  '') ,		 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			--(SR 318680)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt),-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'RETRO',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = COML_AGMT.PREM_ADJ_PGM_id) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID AND CUSTMR.peo_ind=0) LEFT JOIN
			LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id 
			AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_RETRO ON (PREM_ADJ_RETRO.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_RETRO.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id 
			AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO.COML_AGMT_ID)  
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=73
			AND POST_TRNS_TYP.actv_ind=1
			AND COML_AGMT.adj_typ_id IN (66,67) 
			AND PREM_ADJ_RETRO.post_resrv_idnmty_amt < 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	
		--TPD Claim Reserve Recd-WC	6250/0110   (Reclass TPD Clm Reserve Adj-WC	6250/0113 )
		-- As this postings should not send to ARIES and need to shown on only ARIES Exhbit we are setting trnsmtl_sent_ind to 1
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0  
			THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')  
			ELSE CONVERT(char(10), '') END  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), '')  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_RETRO.post_resrv_idnmty_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.post_resrv_idnmty_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			--Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"
			CASE WHEN (@Reverse*PREM_ADJ_RETRO.post_resrv_idnmty_amt  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(16), '') 
			ELSE 'LSI ' + CONVERT(varchar(12), LSI_CUSTMR.lsi_acct_id) END), 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '') 
			ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END), 		-- OPTXT(69)
			CONVERT(char(3),  '') ,			-- ZZPOLSYMBOL(70)
			CONVERT(char(18), '') ,			-- ZZPOLICY(71)
			CONVERT(char(2),  '') ,		 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			--(SR 318680)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt),-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'RETRO',1,@Ind,@rel_prem_adj_id  
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = COML_AGMT.PREM_ADJ_PGM_id) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID AND CUSTMR.peo_ind=0) LEFT JOIN
			LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id 
			AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_RETRO ON (PREM_ADJ_RETRO.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_RETRO.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id 
			AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO.COML_AGMT_ID)  
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=62
			AND POST_TRNS_TYP.actv_ind=1
			AND COML_AGMT.adj_typ_id IN (66,67) 
			AND PREM_ADJ_RETRO.post_resrv_idnmty_amt < 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
			
			
	
		--Resid Mkt Chge Retro Adj	1500/0117  
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CONVERT(char(10), '')  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_RETRO.rsdl_mkt_load_tot_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.rsdl_mkt_load_tot_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*PREM_ADJ_RETRO.rsdl_mkt_load_tot_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), '') ,	 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), '') ,			-- OPTXT(69)
			CONVERT(char(3), RTRIM(COML_AGMT.pol_sym_txt)) ,		-- ZZPOLSYMBOL(70)
			CONVERT(char(18), RTRIM(COML_AGMT.pol_nbr_txt)) ,		-- ZZPOLICY(71)
			CONVERT(char(2), COML_AGMT.pol_modulus_txt), 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  , 		-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'RML',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = COML_AGMT.PREM_ADJ_PGM_id) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) INNER JOIN
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_RETRO ON (PREM_ADJ_RETRO.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_RETRO.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id 
			AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO.COML_AGMT_ID) 
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=26
			AND POST_TRNS_TYP.actv_ind=1
			AND COML_AGMT.adj_typ_id IN (63,65,67,70,71,72) 
			AND PREM_ADJ_RETRO.rsdl_mkt_load_tot_amt <> 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--Def - Retro Bill Reclass	2000/0108 (Reclass of Resid Mkt Chge Retro Adj	1500/0117 )
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CONVERT(char(10), '')  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_RETRO.rsdl_mkt_load_tot_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.rsdl_mkt_load_tot_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			--Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"
			CASE WHEN (@Reverse*PREM_ADJ_RETRO.rsdl_mkt_load_tot_amt  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), '') ,	 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), '') ,			-- OPTXT(69)
			CONVERT(char(3), RTRIM(COML_AGMT.pol_sym_txt)) ,		-- ZZPOLSYMBOL(70)
			CONVERT(char(18), RTRIM(COML_AGMT.pol_nbr_txt)) ,		-- ZZPOLICY(71)
			CONVERT(char(2), COML_AGMT.pol_modulus_txt), 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  ,		-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'RML',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = COML_AGMT.PREM_ADJ_PGM_id) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) INNER JOIN
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_RETRO ON (PREM_ADJ_RETRO.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_RETRO.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id 
			AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO.COML_AGMT_ID) 
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=72
			AND POST_TRNS_TYP.actv_ind=1
			AND COML_AGMT.adj_typ_id IN (63,65,67,70,71,72) 
			AND PREM_ADJ_RETRO.rsdl_mkt_load_tot_amt <> 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--KY T&A Retro Adjustment	4500/0122 
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CONVERT(char(10), '')  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_RETRO.ky_tot_due_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.ky_tot_due_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*PREM_ADJ_RETRO.ky_tot_due_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), '') ,	 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), '') ,			-- OPTXT(69)
			CONVERT(char(3), RTRIM(COML_AGMT.pol_sym_txt)) ,		-- ZZPOLSYMBOL(70)
			CONVERT(char(18), RTRIM(COML_AGMT.pol_nbr_txt)) ,		-- ZZPOLICY(71)
			CONVERT(char(2), COML_AGMT.pol_modulus_txt), 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  , 		-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'KENTUCKY TAXES',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = COML_AGMT.PREM_ADJ_PGM_id) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) INNER JOIN
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_RETRO ON (PREM_ADJ_RETRO.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_RETRO.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id 
			AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO.COML_AGMT_ID) 
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=22
			AND POST_TRNS_TYP.actv_ind=1
			AND COML_AGMT.adj_typ_id IN (63,65,66,67,70,71,72,73) 
			AND PREM_ADJ_RETRO.ky_tot_due_amt <> 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--Def - Retro Bill Reclass	2000/0108 (Reclass of KY T&A Retro Adjustment	4500/0122)
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CONVERT(char(10), '')  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_RETRO.ky_tot_due_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.ky_tot_due_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			--Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"
			CASE WHEN (@Reverse*PREM_ADJ_RETRO.ky_tot_due_amt  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), '') ,	 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), '') ,			-- OPTXT(69)
			CONVERT(char(3), RTRIM(COML_AGMT.pol_sym_txt)) ,		-- ZZPOLSYMBOL(70)
			CONVERT(char(18), RTRIM(COML_AGMT.pol_nbr_txt)) ,		-- ZZPOLICY(71)
			CONVERT(char(2), COML_AGMT.pol_modulus_txt), 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  , 		-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'KENTUCKY TAXES',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = COML_AGMT.PREM_ADJ_PGM_id) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) INNER JOIN
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_RETRO ON (PREM_ADJ_RETRO.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_RETRO.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id 
			AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO.COML_AGMT_ID) 
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=72
			AND POST_TRNS_TYP.actv_ind=1
			AND COML_AGMT.adj_typ_id IN (63,65,66,67,70,71,72,73) 
			AND PREM_ADJ_RETRO.ky_tot_due_amt <> 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--OR T&A Retro Adjustment	4500/0147 
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CONVERT(char(10), '')  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_RETRO.or_tot_due_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.or_tot_due_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*PREM_ADJ_RETRO.or_tot_due_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), '') ,	 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), '') ,			-- OPTXT(69)
			CONVERT(char(3), RTRIM(COML_AGMT.pol_sym_txt)) ,		-- ZZPOLSYMBOL(70)
			CONVERT(char(18), RTRIM(COML_AGMT.pol_nbr_txt)) ,		-- ZZPOLICY(71)
			CONVERT(char(2), COML_AGMT.pol_modulus_txt), 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  , 		-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'OREGON TAXES',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = COML_AGMT.PREM_ADJ_PGM_id) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) INNER JOIN
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_RETRO ON (PREM_ADJ_RETRO.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_RETRO.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id 
			AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO.COML_AGMT_ID) 
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=23
			AND POST_TRNS_TYP.actv_ind=1
			AND COML_AGMT.adj_typ_id IN (63,65,66,67,70,71,72,73) 
			AND PREM_ADJ_RETRO.or_tot_due_amt <> 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--Def - Retro Bill Reclass	2000/0108 (Reclass of OR T&A Retro Adjustment	4500/0147 )
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CONVERT(char(10), '')  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_RETRO.or_tot_due_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.or_tot_due_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			--Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"
			CASE WHEN (@Reverse*PREM_ADJ_RETRO.or_tot_due_amt  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), '') ,	 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), '') ,			-- OPTXT(69)
			CONVERT(char(3), RTRIM(COML_AGMT.pol_sym_txt)) ,		-- ZZPOLSYMBOL(70)
			CONVERT(char(18), RTRIM(COML_AGMT.pol_nbr_txt)) ,		-- ZZPOLICY(71)
			CONVERT(char(2), COML_AGMT.pol_modulus_txt), 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  , 		-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'OREGON TAXES',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = COML_AGMT.PREM_ADJ_PGM_id) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) INNER JOIN
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_RETRO ON (PREM_ADJ_RETRO.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_RETRO.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id 
			AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO.COML_AGMT_ID) 
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=72
			AND POST_TRNS_TYP.actv_ind=1
			AND COML_AGMT.adj_typ_id IN (63,65,66,67,70,71,72,73) 
			AND PREM_ADJ_RETRO.or_tot_due_amt <> 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--ILRF Adjustment - C&RM	7000/0107
 		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,
			pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,
			sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,	bewar_txt,optxt_txt,
			zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,
			zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,
			prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,
			Post_cd,rel_prem_adj_id)
		SELECT 
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0  
			THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')  
			ELSE CONVERT(char(10), '') END  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), '')  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*PREM_ADJ_PARMET_SETUP.tot_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(16), '') 
			ELSE 'LSI ' + CONVERT(varchar(12), LSI_CUSTMR.lsi_acct_id) END), 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '') 
			ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END), 		-- OPTXT(69)
			CONVERT(char(3), '') , 			-- ZZPOLSYMBOL(70)
			CONVERT(char(18), '') , 		-- ZZPOLICY(71)
			CONVERT(char(2), '') , 			-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			--(SR 318680)
			CONVERT(char(20), dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,400)),-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'ILRF',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN 
			LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id AND LSI_CUSTMR.actv_ind=1 
			AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_PARMET_SETUP ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id)
		WHERE PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=400 -- ILRF
			AND POST_TRNS_TYP.POST_TRNS_TYP_ID=35
			AND POST_TRNS_TYP.actv_ind=1
			AND PREM_ADJ_PARMET_SETUP.tot_amt > 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--ILRF Adjustment - C&RM	7250/0107 
 		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,
			pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,
			sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,	bewar_txt,optxt_txt,
			zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,
			zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,
			prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,
			Post_cd,rel_prem_adj_id)
		SELECT 
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0  
			THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')  
			ELSE CONVERT(char(10), '') END  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), '')  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*PREM_ADJ_PARMET_SETUP.tot_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(16), '') 
			ELSE 'LSI ' + CONVERT(varchar(12), LSI_CUSTMR.lsi_acct_id) END), 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '') 
			ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END), 		-- OPTXT(69)
			CONVERT(char(3), '') , 			-- ZZPOLSYMBOL(70)
			CONVERT(char(18), '') , 		-- ZZPOLICY(71)
			CONVERT(char(2), '') , 			-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			--(SR 318680)
			CONVERT(char(20), dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,400)),-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'ILRF',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN 
			LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id 
			AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_PARMET_SETUP ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id)
		WHERE PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=400 -- ILRF
			AND POST_TRNS_TYP.POST_TRNS_TYP_ID=37
			AND POST_TRNS_TYP.actv_ind=1
			AND PREM_ADJ_PARMET_SETUP.tot_amt < 0 -- WHEN ILRF Amount is <0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--Incurred Loss Reimb.Fund	7250/0100 
 		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,
			pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,
			sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,	bewar_txt,optxt_txt,
			zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,
			zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,
			prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,
			Post_cd,rel_prem_adj_id)
		SELECT 
		    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0  
			THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')  
			ELSE CONVERT(char(10), '') END  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), '')  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)), 
			--Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*PREM_ADJ_PARMET_SETUP.tot_amt  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(16), '') 
			ELSE 'LSI ' + CONVERT(varchar(12), LSI_CUSTMR.lsi_acct_id) END), 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '') 
			ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END), 		-- OPTXT(69)
			CONVERT(char(3), '') , 			-- ZZPOLSYMBOL(70)
			CONVERT(char(18), '') , 		-- ZZPOLICY(71)
			CONVERT(char(2), '') , 			-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			--(SR 318680)
			CONVERT(char(20), dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,400)),-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'ILRF',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN 
			LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id AND LSI_CUSTMR.actv_ind=1 
			AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_PARMET_SETUP ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id)
		WHERE PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=400 -- ILRF
			AND POST_TRNS_TYP.POST_TRNS_TYP_ID=36
			AND POST_TRNS_TYP.actv_ind=1
			AND PREM_ADJ_PARMET_SETUP.tot_amt < 0 -- WHEN ILRF Amount is <0 
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

	-- Auto TPD Adjust - C&RM	6000/0120 	
	-- GL TPD Adjust - C&RM		6000/0121
	-- WC TPD Adjust - C&RM		6000/0122
	-- Auto LCF Adjust - C&RM	6000/0123   
	-- GL LCF Adjust - C&RM		6000/0124  
	-- WC LCF Adjust - C&RM		6000/0125  
	-- LBA Adjustment - C&RM	6000/0106 
	INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
		pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
		gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
		endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
		vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
		emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,
		pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,
		sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,	bewar_txt,optxt_txt,
		zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
		yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
		zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,
		zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,
		prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
	SELECT 
	    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
		CONVERT(char(2), '01') , 		-- AKTYP(2)
		CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0  THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')  
		ELSE CONVERT(char(10), '') END  , 		-- GPART(3)
		CONVERT(char(20), '')  , 		-- GPART_EXT(4)
		CONVERT(char(20), '')  ,  		-- VTREF(5)
		CONVERT(char(12), '')  , 		-- POSNR(6)
		CONVERT(char(1), 'X')  , 		-- PSNGL(7)
		ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
		ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
		ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
		ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
		CONVERT(char(8), '') , 			-- PMEND(12)
		CONVERT(char(6), '') , 			-- PMEND_TIME(13)
		CONVERT(char(1), '') , 			-- RENEW(14)
		CONVERT(char(3), '') , 			-- RNEWX(15)

		CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
		CONVERT(char(4), '') , 			-- OPCCODE(17)
		CONVERT(char(4), '') , 			-- GSBER(18)
		CONVERT(char(4), '') , 			-- OPGSBER(19)
		CONVERT(char(2), '') , 			-- PRGRP(20)
		CONVERT(char(6), '') , 			-- VSARL_VX(21)

		CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
		CONVERT(bigint, PREM_ADJ_LOS_REIM_FUND_POST.post_amt  * 100)))) +
		CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_LOS_REIM_FUND_POST.post_amt  * 100)), 
		'000000000000000'), ' ', '0'), '-', '0')) + 
		CASE WHEN (@Reverse*PREM_ADJ_LOS_REIM_FUND_POST.post_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

		CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
		CONVERT(char(8), '') ,			-- ATFRD(24)
		CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
		CONVERT(char(1), '') , 			-- ENDTYPE(26)
		CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
		CONVERT(char(16), '') , 		-- AMOUNT_END(28)
		CONVERT(char(5), 'USD') , 		-- CURR(29)
		CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
		CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
		CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
		CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
		CONVERT(char(2), 'PP') , 		-- BLART(34)
		CONVERT(char(20), '') , 		-- VTRE2(35)
		CONVERT(char(20), '') , 		-- VTRE3(36)
		CONVERT(char(10), '') , 		-- VGPART2(37)
		CONVERT(char(10), '') , 		-- VGPART3(38)
		CONVERT(char(16), '') , 		-- GSFNR(39)
		CONVERT(char(6), '') , 			-- BELNR(40)
		CONVERT(char(2), '20') , 		-- BLTYP(41)
		CONVERT(char(10), '') , 		-- EMGPA(42)
		CONVERT(char(4), '') , 			-- EMBVT(43)
		CONVERT(char(10), '') ,			-- EMADR(44)
		CONVERT(char(1), 3) ,			-- PYMET(45)
		CONVERT(char(4), '') , 			-- PYBUK(46)
		ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
		CONVERT(char(8), '') , 			-- BUDAT(48)
		ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
		CONVERT(char(1), '') , 			-- DUN_REASON(50)
		CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
		CONVERT(char(1), '') , 			-- PAY_REASON(52)
		CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
		CONVERT(char(1), '') , 			-- CLR_REASON(54)
		CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
		CONVERT(char(10), '') , 		-- KOSTL(56)
		CONVERT(char(10), '') , 		-- PRCTR(57)
		CONVERT(char(10), '') , 		-- PYGRP(58)
		CONVERT(char(3), '') , 			-- GRKEY(59)
		CONVERT(char(1), '') , 			-- ENDREV(60)
		CONVERT(char(8), '') , 			-- STO_FROM(61)
		CONVERT(char(8), '') , 			-- STO_TO(62)
		CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
		CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
		CONVERT(char(6), '') , 			-- VBUND(65)
		CONVERT(char(16), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(16), '') 
		ELSE 'LSI ' + CONVERT(varchar(12), LSI_CUSTMR.lsi_acct_id) END), 		-- REFGF(66)
		CONVERT(char(6), '') , 			-- REFBL(67)
		CONVERT(char(3), '1') , 		-- BEWAR(68)
		CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '') 
		ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END), 		-- OPTXT(69)
		CONVERT(char(3), '') , 			-- ZZPOLSYMBOL(70)
		CONVERT(char(18), '') , 		-- ZZPOLICY(71)
		CONVERT(char(2), '') , 			-- ZZMOD(72)
		CONVERT(char(1), '') , 			-- ZZLSIERR(73)
		CONVERT(char(10), '') , 		-- ZZINVGRP(74)
		CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
		CONVERT(char(5), '') , 			-- YYLOB_CD(76)
		CONVERT(char(4), '') , 			-- YYAMY_CD(77)
		CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
		CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
		CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
		CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
		CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
		CONVERT(char(5), '') , 			-- ZZWWZDC(83)
		CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
		CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
		ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
		CONVERT(char(10), '') , 		-- ZZGPART4(87)
		--(SR 318680)
		CONVERT(char(20), dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,400)),-- ZZVTREF4(88)
		CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
		CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
		CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
		CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
		CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
		CONVERT(char(1), 1	  ),		-- updt_user_id
		getdate(),  -- updt_dt
		CONVERT(char(1), 1),			-- crte_user_id
		getdate(),  -- crte_dt
		CONVERT(char(1), 1)  ,			-- actv_ind
		PREM_ADJ_PERD.prem_adj_perd_id,
		PREM_ADJ_PERD.prem_adj_id,
		PREM_ADJ_PERD.custmr_id,
		POST_TRNS_TYP.post_trns_typ_id,
		'ILRF',0,@Ind,@rel_prem_adj_id
	FROM PREM_ADJ INNER JOIN 
		PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
		AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
		PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
		CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN 
		LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id 
		AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
		POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
		PREM_ADJ_PARMET_SETUP ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
		AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
		AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
		AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id) INNER JOIN
		PREM_ADJ_LOS_REIM_FUND_POST  ON (PREM_ADJ_LOS_REIM_FUND_POST.PREM_ADJ_PERD_ID = 
		PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID 
		AND PREM_ADJ_LOS_REIM_FUND_POST.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
		AND PREM_ADJ_LOS_REIM_FUND_POST.RECV_TYP_ID=POST_TRNS_TYP.POST_TRNS_TYP_ID)
	WHERE PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=400 -- ILRF
		AND POST_TRNS_TYP.POST_TRNS_TYP_ID<>71 -- DO NOT POST RESERVES #71
		AND POST_TRNS_TYP.actv_ind=1
		AND PREM_ADJ_LOS_REIM_FUND_POST.post_amt <> 0 
		AND PREM_ADJ.prem_adj_id = @prem_adj_id


		--NY Second Injury Fund Adj	4500/0159 
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
			CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CONVERT(char(10), '')  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_NY_SCND_INJR_FUND.curr_adj_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_NY_SCND_INJR_FUND.curr_adj_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*PREM_ADJ_NY_SCND_INJR_FUND.curr_adj_amt  < 0)  
			THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)
			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), '') ,	 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), '') ,			-- OPTXT(69)
			CONVERT(char(3), RTRIM(COML_AGMT.pol_sym_txt)) ,		-- ZZPOLSYMBOL(70)
			CONVERT(char(18), RTRIM(COML_AGMT.pol_nbr_txt)) ,		-- ZZPOLICY(71)
			CONVERT(char(2), COML_AGMT.pol_modulus_txt), 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  , 		-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'NY-SIF',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN 
			LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id AND LSI_CUSTMR.actv_ind=1 
			AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_NY_SCND_INJR_FUND ON (PREM_ADJ_NY_SCND_INJR_FUND.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_NY_SCND_INJR_FUND.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID  
			AND PREM_ADJ_NY_SCND_INJR_FUND.coml_agmt_id = COML_AGMT.coml_agmt_id
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id) 
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=24
			AND POST_TRNS_TYP.actv_ind=1
			AND PREM_ADJ_NY_SCND_INJR_FUND.curr_adj_amt <> 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--Def - Retro Bill Reclass	2000/0108  (Reclass for NY Second Injury Fund Adj	4500/0159 )
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
			CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CONVERT(char(10), '')  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_NY_SCND_INJR_FUND.curr_adj_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_NY_SCND_INJR_FUND.curr_adj_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			--Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"
			CASE WHEN (@Reverse*PREM_ADJ_NY_SCND_INJR_FUND.curr_adj_amt  > 0)  
			THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), '') ,	 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), '') ,			-- OPTXT(69)
			CONVERT(char(3), RTRIM(COML_AGMT.pol_sym_txt)) ,		-- ZZPOLSYMBOL(70)
			CONVERT(char(18), RTRIM(COML_AGMT.pol_nbr_txt)) ,		-- ZZPOLICY(71)
			CONVERT(char(2), COML_AGMT.pol_modulus_txt), 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)  , 		-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'NY-SIF',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN 
			LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id 
			AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_NY_SCND_INJR_FUND ON (PREM_ADJ_NY_SCND_INJR_FUND.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_NY_SCND_INJR_FUND.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID  
			AND PREM_ADJ_NY_SCND_INJR_FUND.coml_agmt_id = COML_AGMT.coml_agmt_id
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id) 
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=72
			AND POST_TRNS_TYP.actv_ind=1
			AND PREM_ADJ_NY_SCND_INJR_FUND.curr_adj_amt <> 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--Misc Postings
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
			CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)

			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN 
				CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0  
				THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')  
				ELSE CONVERT(char(10), '') END 		
			ELSE
				CONVERT(char(10), '')  
			END,							-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)

			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN 
				CONVERT(char(20), '') 
			ELSE 
				CONVERT(char(20), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)+
				RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)+PREM_ADJ_MISC_INVC.pol_modulus_txt) 
			END,  		-- VTREF(5)

			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)
			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_MISC_INVC.post_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_MISC_INVC.post_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*PREM_ADJ_MISC_INVC.post_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)

			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN
				CONVERT(char(16), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(16), '') 
				ELSE 'LSI ' + CONVERT(varchar(12), LSI_CUSTMR.lsi_acct_id) END) 
				ELSE  CONVERT(char(16), '') END, 		-- REFGF(66)
				CONVERT(char(6), '') , 			-- REFBL(67)
				CONVERT(char(3), '1') , 		-- BEWAR(68)
			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN
				CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '') 
				ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END) 
				ELSE  CONVERT(char(50), '') END, 		-- OPTXT(69)

			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) 
			THEN CONVERT(char(3), '') 	
			ELSE CONVERT(char(3), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)) END,					-- ZZPOLSYMBOL(70)
			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) 
			THEN CONVERT(char(3), '') 	
			ELSE CONVERT(char(18), RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)) END,					-- ZZPOLICY(71)
			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) 
			THEN CONVERT(char(3), '') 	
			ELSE CONVERT(char(2), PREM_ADJ_MISC_INVC.pol_modulus_txt) END,			-- ZZMOD(72)

			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)

			--CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN 
				--CONVERT(char(20), '') 
			--ELSE 
				--CONVERT(char(20), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)+
				--RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)+PREM_ADJ_MISC_INVC.pol_modulus_txt) 
			--END,  		-- ZZVTREF4(88)
			--(SR 318680)
			CONVERT(char(20),dbo.fn_GetPolicyForMiscAriesPostings(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,POST_TRNS_TYP.POST_TRNS_TYP_ID)),-- ZZVTREF4(88)


			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'MISC. POSTINGS',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN 
			LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id AND LSI_CUSTMR.actv_ind=1 
			AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_MISC_INVC ON (POST_TRNS_TYP.POST_TRNS_TYP_ID = PREM_ADJ_MISC_INVC.POST_TRNS_TYP_ID 
			AND PREM_ADJ_MISC_INVC.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id
			AND PREM_ADJ_MISC_INVC.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id AND PREM_ADJ_MISC_INVC.actv_ind=1) 
		WHERE POST_TRNS_TYP.TRNS_TYP_ID=444 -- MISCELLANEOUS POSTING
			AND POST_TRNS_TYP.actv_ind=1
			AND POST_TRNS_TYP.post_ind=1
			AND PREM_ADJ_MISC_INVC.post_amt <> 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--Misc Postings - Re-class
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
			CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)

			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN 
				CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0  
				THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')  
				ELSE CONVERT(char(10), '') END 		
			ELSE
				CONVERT(char(10), '')  
			END,							-- GPART(3)

			CONVERT(char(20), '')  , 		-- GPART_EXT(4)

			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN 
				CONVERT(char(20), '') 
			ELSE 
				CONVERT(char(20), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)+
				RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)+PREM_ADJ_MISC_INVC.pol_modulus_txt) 
			END,  		-- VTREF(5)

			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)
			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_MISC_INVC.post_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_MISC_INVC.post_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			--Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"
			CASE WHEN (@Reverse*PREM_ADJ_MISC_INVC.post_amt  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)

			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN
				CONVERT(char(16), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(16), '') 
				ELSE 'LSI ' + CONVERT(varchar(12), LSI_CUSTMR.lsi_acct_id) END) 
				ELSE  CONVERT(char(16), '') END, 		-- REFGF(66)
				CONVERT(char(6), '') , 			-- REFBL(67)
				CONVERT(char(3), '1') , 		-- BEWAR(68)
			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN
				CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '') 
				ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END) 
				ELSE  CONVERT(char(50), '') END, 		-- OPTXT(69)

			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) 
			THEN CONVERT(char(3), '') 	
			ELSE CONVERT(char(3), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)) END,					-- ZZPOLSYMBOL(70)
			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) 
			THEN CONVERT(char(3), '') 	
			ELSE CONVERT(char(18), RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)) END,					-- ZZPOLICY(71)
			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) 
			THEN CONVERT(char(3), '') 	
			ELSE CONVERT(char(2), PREM_ADJ_MISC_INVC.pol_modulus_txt) END,			-- ZZMOD(72)

			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)

			--CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN 
				--CONVERT(char(20), '') 
			--ELSE 
				--CONVERT(char(20), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)+
				--RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)+PREM_ADJ_MISC_INVC.pol_modulus_txt) 
			--END,  		-- ZZVTREF4(88)
			--(SR 318680)
			CONVERT(char(20),dbo.fn_GetPolicyForMiscAriesPostings(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,POST_TRNS_TYP.POST_TRNS_TYP_ID)),-- ZZVTREF4(88)

			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'MISC. POSTINGS',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN 
			LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id AND LSI_CUSTMR.actv_ind=1 
			AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_MISC_INVC ON (POST_TRNS_TYP.POST_TRNS_TYP_ID = PREM_ADJ_MISC_INVC.POST_TRNS_TYP_ID 
			AND PREM_ADJ_MISC_INVC.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id
			AND PREM_ADJ_MISC_INVC.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id AND PREM_ADJ_MISC_INVC.actv_ind=1) 
		WHERE POST_TRNS_TYP.TRNS_TYP_ID =444 -- MISCELLANEOUS POSTING
			AND POST_TRNS_TYP.POST_TRNS_TYP_ID IN (3) -- RECLASS MISC. POSTING
			AND POST_TRNS_TYP.actv_ind=1
			AND POST_TRNS_TYP.post_ind=1
			AND PREM_ADJ_MISC_INVC.post_amt <> 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--Misc Postings - Re-class 7250 0112 for posting 7250 0113 
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
			CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)

			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN 
				CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0  
				THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')  
				ELSE CONVERT(char(10), '') END 		
			ELSE
				CONVERT(char(10), '')  
			END,							-- GPART(3)

			CONVERT(char(20), '')  , 		-- GPART_EXT(4)

			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN 
				CONVERT(char(20), '') 
			ELSE 
				CONVERT(char(20), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)+
				RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)+PREM_ADJ_MISC_INVC.pol_modulus_txt) 
			END,  		-- VTREF(5)

			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 		-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)
			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_MISC_INVC.post_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_MISC_INVC.post_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			--Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"
			CASE WHEN (@Reverse*PREM_ADJ_MISC_INVC.post_amt  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)

			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN
				CONVERT(char(16), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(16), '') 
				ELSE 'LSI ' + CONVERT(varchar(12), LSI_CUSTMR.lsi_acct_id) END) 
				ELSE  CONVERT(char(16), '') END, 		-- REFGF(66)
				CONVERT(char(6), '') , 			-- REFBL(67)
				CONVERT(char(3), '1') , 		-- BEWAR(68)
			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN
				CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '') 
				ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END) 
				ELSE  CONVERT(char(50), '') END, 		-- OPTXT(69)

			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) 
				THEN CONVERT(char(3), '') 	
			ELSE CONVERT(char(3), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)) END,					-- ZZPOLSYMBOL(70)
			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) 
			THEN CONVERT(char(3), '') 	
			ELSE CONVERT(char(18), RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)) END,					-- ZZPOLICY(71)
			CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) 
			THEN CONVERT(char(3), '') 	
			ELSE CONVERT(char(2), PREM_ADJ_MISC_INVC.pol_modulus_txt) END,			-- ZZMOD(72)

			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)

			--CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN 
				--CONVERT(char(20), '') 
			--ELSE 
				--CONVERT(char(20), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)+
				--RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)+PREM_ADJ_MISC_INVC.pol_modulus_txt) 
			--END,  		-- ZZVTREF4(88)
			--(SR 318680)
			CONVERT(char(20),dbo.fn_GetPolicyForMiscAriesPostings(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,POST_TRNS_TYP.POST_TRNS_TYP_ID)),-- ZZVTREF4(88)

			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			79, --- HARDCODED RE-CLASS FOR 7250/0113
			'MISC. POSTINGS',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN 
			LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id AND LSI_CUSTMR.actv_ind=1 
			AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_MISC_INVC ON (POST_TRNS_TYP.POST_TRNS_TYP_ID = PREM_ADJ_MISC_INVC.POST_TRNS_TYP_ID 
			AND PREM_ADJ_MISC_INVC.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id
			AND PREM_ADJ_MISC_INVC.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id AND PREM_ADJ_MISC_INVC.actv_ind=1) 
		WHERE POST_TRNS_TYP.TRNS_TYP_ID =444 -- MISCELLANEOUS POSTING
			AND POST_TRNS_TYP.POST_TRNS_TYP_ID IN (77) -- RECLASS MISC. POSTING
			AND POST_TRNS_TYP.actv_ind=1
			AND POST_TRNS_TYP.post_ind=1
			AND PREM_ADJ_MISC_INVC.post_amt <> 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	

		--PEO ADJUSTMENTS (Rollup Retro. Premium Adjustment - 1500/0111 to PEO master policy level)
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
			CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CONVERT(char(10), '')  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 				-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 				-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)
			--CONVERT(char(16), PAPS.tot_amt) , 		-- AMOUNT_TOTAL(22)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, SUM(PREM_ADJ_RETRO.aries_tot_amt)  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, SUM(PREM_ADJ_RETRO.aries_tot_amt)  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*SUM(PREM_ADJ_RETRO.aries_tot_amt)  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), '') ,	 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), '') ,			-- OPTXT(69)
			CONVERT(char(3), CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 THEN SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),1,3) 
					else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),1,2) END) , -- ZZPOLSYMBOL(70)
			CONVERT(char(18),CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 THEN  SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),4,8)
					else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),3,8) end ) ,-- ZZPOLICY(71)
			CONVERT(char(2),CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 then  SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),12,2)
                    else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),11,2) end), 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			--CONVERT(char(20), '') , 		-- ZZVTREF4(88)
			CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id)), -- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'PEO',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID AND CUSTMR.peo_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_RETRO ON (PREM_ADJ_RETRO.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_RETRO.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id 
			AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO.COML_AGMT_ID) 
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=18
			AND POST_TRNS_TYP.actv_ind=1
			AND COML_AGMT.adj_typ_id IN (63,65,66,67,70,71,72,73) 
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
			--11162 condition
			AND (select case when count(#tmpCesar."current")>0 then 1 else 0 end from #tmpCesar where #tmpCesar."PREM ADJ ID"=@prem_adj_id and #tmpCesar."PREM ADJ PGM ID"=PREM_ADJ_PGM.PREM_ADJ_PGM_ID and #tmpCesar."POLICYID"=COML_AGMT.COML_AGMT_ID and #tmpCesar."current"<>0)<>0
		GROUP BY
			PREM_ADJ.valn_dt,PREM_ADJ_PGM.strt_dt,PREM_ADJ_PGM.plan_end_dt,
			POST_TRNS_TYP.main_nbr_txt,POST_TRNS_TYP.sub_nbr_txt,PREM_ADJ.invc_due_dt,
			PREM_ADJ.fnl_invc_dt,PREM_ADJ.fnl_invc_nbr_txt,POST_TRNS_TYP.comp_txt,
			PREM_ADJ_PERD.prem_adj_perd_id,PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,POST_TRNS_TYP.post_trns_typ_id,
			POST_TRNS_TYP.trns_nm_txt,PREM_ADJ_PGM.prem_adj_pgm_id
		HAVING SUM(PREM_ADJ_RETRO.aries_tot_amt) <> 0

		--PEO ADJUSTMENTS (Retro Adjustment POD	2000/0104 to PEO master policy level)
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
			CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CONVERT(char(10), '')  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 				-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 				-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			--Because it is BALANCED ENTRY (re-class), we send -ve amounts when it is actually positive. ">" used instead of "<"
			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, SUM(PREM_ADJ_RETRO.adj_cash_flw_ben_amt)  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, SUM(PREM_ADJ_RETRO.adj_cash_flw_ben_amt)  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*SUM(PREM_ADJ_RETRO.adj_cash_flw_ben_amt)  > 0)  
			THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), '') ,	 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), '') ,			-- OPTXT(69)
			CONVERT(char(3), CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 THEN SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),1,3) 
					else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),1,2) END) , -- ZZPOLSYMBOL(70)
			CONVERT(char(18),CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 THEN  SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),4,8)
					else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),3,8) end ) ,-- ZZPOLICY(71)
			CONVERT(char(2),CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 then  SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),12,2)
                    else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),11,2) end), 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))  , 		-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'PEO',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID AND CUSTMR.peo_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_RETRO ON (PREM_ADJ_RETRO.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_RETRO.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id 
			AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO.COML_AGMT_ID) 
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=25
			AND POST_TRNS_TYP.actv_ind=1
			AND COML_AGMT.adj_typ_id IN (70,71,72,73) --ONLY PAID RETRO -- Added 71,72,73 bug 10046
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
		GROUP BY
			PREM_ADJ.valn_dt,PREM_ADJ_PGM.strt_dt,PREM_ADJ_PGM.plan_end_dt,
			POST_TRNS_TYP.main_nbr_txt,POST_TRNS_TYP.sub_nbr_txt,PREM_ADJ.invc_due_dt,
			PREM_ADJ.fnl_invc_dt,PREM_ADJ.fnl_invc_nbr_txt,POST_TRNS_TYP.comp_txt,
			PREM_ADJ_PERD.prem_adj_perd_id,PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,POST_TRNS_TYP.post_trns_typ_id,
			POST_TRNS_TYP.trns_nm_txt,PREM_ADJ_PGM.prem_adj_pgm_id
		HAVING SUM(PREM_ADJ_RETRO.adj_cash_flw_ben_amt) <> 0

		--PEO ADJUSTMENTS (Retro Adjustment POD	2000/0104 to PEO master policy level)  (DUE = VALDATE + 1 YEAR)
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
			CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CONVERT(char(10), '')  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 				-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 				-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			--Because it is BALANCED ENTRY (re-class), we send -ve amounts when it is actually positive. ">" used instead of "<"
			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, SUM(PREM_ADJ_RETRO.adj_cash_flw_ben_amt)  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, SUM(PREM_ADJ_RETRO.adj_cash_flw_ben_amt)  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*SUM(PREM_ADJ_RETRO.adj_cash_flw_ben_amt)  < 0)  
			THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), dateadd(YEAR,1,PREM_ADJ.valn_dt), 112), '') , 	-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), '') ,	 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), '') ,			-- OPTXT(69)
			CONVERT(char(3), CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 THEN SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),1,3) 
					else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),1,2) END) , -- ZZPOLSYMBOL(70)
			CONVERT(char(18),CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 THEN  SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),4,8)
					else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),3,8) end ) ,-- ZZPOLICY(71)
			CONVERT(char(2),CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 then  SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),12,2)
                    else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),11,2) end), 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))  , 		-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'PEO',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID AND CUSTMR.peo_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_RETRO ON (PREM_ADJ_RETRO.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_RETRO.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id 
			AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO.COML_AGMT_ID) 
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=25
			AND POST_TRNS_TYP.actv_ind=1
			AND COML_AGMT.adj_typ_id IN (70,71,72,73) --ONLY PAID RETRO -- Added 71,72,73 bug 10046
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
	    GROUP BY
			PREM_ADJ.valn_dt,PREM_ADJ_PGM.strt_dt,PREM_ADJ_PGM.plan_end_dt,
			POST_TRNS_TYP.main_nbr_txt,POST_TRNS_TYP.sub_nbr_txt,PREM_ADJ.invc_due_dt,
			PREM_ADJ.fnl_invc_dt,PREM_ADJ.fnl_invc_nbr_txt,POST_TRNS_TYP.comp_txt,
			PREM_ADJ_PERD.prem_adj_perd_id,PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,POST_TRNS_TYP.post_trns_typ_id,
			POST_TRNS_TYP.trns_nm_txt,PREM_ADJ_PGM.prem_adj_pgm_id
		HAVING SUM(PREM_ADJ_RETRO.adj_cash_flw_ben_amt) <> 0


			--PEO ADJUSTMENTS (Rollup Def - Retro Bill Reclass	2000/0108 to PEO master policy level)  
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,
			pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,
			endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,
			bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,
			zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,
			custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)
		SELECT	
			CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CONVERT(char(10), '')  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 				-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 				-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     , 		-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 		-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, SUM(PREM_ADJ_RETRO.aries_tot_amt)  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, SUM(PREM_ADJ_RETRO.aries_tot_amt)  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*SUM(PREM_ADJ_RETRO.aries_tot_amt)  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), '') ,	 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), '') ,			-- OPTXT(69)
			CONVERT(char(3), CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 THEN SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),1,3) 
					else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),1,2) END) , -- ZZPOLSYMBOL(70)
			CONVERT(char(18),CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 THEN  SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),4,8)
					else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),3,8) end ) ,-- ZZPOLICY(71)
			CONVERT(char(2),CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 then  SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),12,2)
                    else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),11,2) end), 	-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))  , 		-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'PEO',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID AND CUSTMR.peo_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_RETRO ON (PREM_ADJ_RETRO.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_RETRO.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id 
			AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_RETRO.COML_AGMT_ID) 
		WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=72
			AND POST_TRNS_TYP.actv_ind=1
			AND COML_AGMT.adj_typ_id IN (63,65,66,67,70,71,72,73) 
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
			--11162 condition
			AND (select case when count(#tmpCesar."current")>0 then 1 else 0 end from #tmpCesar where #tmpCesar."PREM ADJ ID"=@prem_adj_id and #tmpCesar."PREM ADJ PGM ID"=PREM_ADJ_PGM.PREM_ADJ_PGM_ID and #tmpCesar."POLICYID"=COML_AGMT.COML_AGMT_ID and #tmpCesar."current"<>0)<>0
		GROUP BY
			PREM_ADJ.valn_dt,PREM_ADJ_PGM.strt_dt,PREM_ADJ_PGM.plan_end_dt,
			POST_TRNS_TYP.main_nbr_txt,POST_TRNS_TYP.sub_nbr_txt,PREM_ADJ.invc_due_dt,
			PREM_ADJ.fnl_invc_dt,PREM_ADJ.fnl_invc_nbr_txt,POST_TRNS_TYP.comp_txt,
			PREM_ADJ_PERD.prem_adj_perd_id,PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,POST_TRNS_TYP.post_trns_typ_id,
			POST_TRNS_TYP.trns_nm_txt,PREM_ADJ_PGM.prem_adj_pgm_id
		HAVING SUM(PREM_ADJ_RETRO.aries_tot_amt) <> 0

		--Clm Service Fee Adj-C&RM	8000/0130 
		INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,
			pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,
			gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,
			endtyp_txt,amt_ned_txt,	amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,
			vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,
			emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,
			pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,
			sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,	bewar_txt,optxt_txt,
			zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,
			yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,
			zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,
			zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,
			prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,
			Post_cd,rel_prem_adj_id)
		SELECT 
			CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
			CONVERT(char(2), '01') , 		-- AKTYP(2)
			CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0  
			THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')  
			ELSE CONVERT(char(10), '') END  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			CONVERT(char(20), '')  ,  		-- VTREF(5)
			CONVERT(char(12), '')  , 		-- POSNR(6)
			CONVERT(char(1), 'X')  , 		-- PSNGL(7)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 					-- PMTFR(8)
			ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') , 					-- PMTTO(9)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,			-- RISKFR(10)
			ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') , 			-- RISKTO(11)
			CONVERT(char(8), '') , 			-- PMEND(12)
			CONVERT(char(6), '') , 			-- PMEND_TIME(13)
			CONVERT(char(1), '') , 			-- RENEW(14)
			CONVERT(char(3), '') , 			-- RNEWX(15)

			CONVERT(char(4), POST_TRNS_TYP.COMP_TXT) ,		-- CCODE(16)
			CONVERT(char(4), '') , 			-- OPCCODE(17)
			CONVERT(char(4), '') , 			-- GSBER(18)
			CONVERT(char(4), '') , 			-- OPGSBER(19)
			CONVERT(char(2), '') , 			-- PRGRP(20)
			CONVERT(char(6), '') , 			-- VSARL_VX(21)

			CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15), 
			CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (@Reverse*PREM_ADJ_PARMET_SETUP.tot_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

			CONVERT(char(16), '') , 		-- AMOUNT_INST(23)
			CONVERT(char(8), '') ,			-- ATFRD(24)
			CONVERT(char(6), '') , 			-- ATFRD_TIME(25)
			CONVERT(char(1), '') , 			-- ENDTYPE(26)
			CONVERT(char(16), '') , 		-- AMOUNT_NEED(27)
			CONVERT(char(16), '') , 		-- AMOUNT_END(28)
			CONVERT(char(5), 'USD') , 		-- CURR(29)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- HVORG(30)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- TVORG(31)
			CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) , 			-- OPHVORG(32)
			CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) , 			-- OPTVORG(33)
			CONVERT(char(2), 'PP') , 		-- BLART(34)
			CONVERT(char(20), '') , 		-- VTRE2(35)
			CONVERT(char(20), '') , 		-- VTRE3(36)
			CONVERT(char(10), '') , 		-- VGPART2(37)
			CONVERT(char(10), '') , 		-- VGPART3(38)
			CONVERT(char(16), '') , 		-- GSFNR(39)
			CONVERT(char(6), '') , 			-- BELNR(40)
			CONVERT(char(2), '20') , 		-- BLTYP(41)
			CONVERT(char(10), '') , 		-- EMGPA(42)
			CONVERT(char(4), '') , 			-- EMBVT(43)
			CONVERT(char(10), '') ,			-- EMADR(44)
			CONVERT(char(1), '') ,			-- PYMET(45)
			CONVERT(char(4), '') , 			-- PYBUK(46)
			ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') , 		-- FAEDN(47)
			CONVERT(char(8), '') , 			-- BUDAT(48)
			ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') , 		-- BLDAT(49)
			CONVERT(char(1), '') , 			-- DUN_REASON(50)
			CONVERT(char(3), '') , 			-- DUN_REASON_DAYS(51)
			CONVERT(char(1), '') , 			-- PAY_REASON(52)
			CONVERT(char(3), '') , 			-- PAY_REASON_DAYS(53)
			CONVERT(char(1), '') , 			-- CLR_REASON(54)
			CONVERT(char(3), '') , 			-- CLR_REASON_DAYS(55)
			CONVERT(char(10), '') , 		-- KOSTL(56)
			CONVERT(char(10), '') , 		-- PRCTR(57)
			CONVERT(char(10), '') , 		-- PYGRP(58)
			CONVERT(char(3), '') , 			-- GRKEY(59)
			CONVERT(char(1), '') , 			-- ENDREV(60)
			CONVERT(char(8), '') , 			-- STO_FROM(61)
			CONVERT(char(8), '') , 			-- STO_TO(62)
			CONVERT(char(30), 'RT') , 		-- ORIGIN(63)
			CONVERT(char(1), '') , 			-- CHECKLEVEL(64)
			CONVERT(char(6), '') , 			-- VBUND(65)
			CONVERT(char(16), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(16), '') 
			ELSE 'LSI ' + CONVERT(varchar(12), LSI_CUSTMR.lsi_acct_id) END), 		-- REFGF(66)
			CONVERT(char(6), '') , 			-- REFBL(67)
			CONVERT(char(3), '1') , 		-- BEWAR(68)
			CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '') 
			ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END), 		-- OPTXT(69)
			CONVERT(char(3), '') , 			-- ZZPOLSYMBOL(70)
			CONVERT(char(18), '') , 		-- ZZPOLICY(71)
			CONVERT(char(2), '') , 			-- ZZMOD(72)
			CONVERT(char(1), '') , 			-- ZZLSIERR(73)
			CONVERT(char(10), '') , 		-- ZZINVGRP(74)
			CONVERT(char(12), '') , 		-- ZZPCLINKNR(75)
			CONVERT(char(5), '') , 			-- YYLOB_CD(76)
			CONVERT(char(4), '') , 			-- YYAMY_CD(77)
			CONVERT(char(5), '') , 			-- ZZSTATE_CD(78)
			CONVERT(char(6), '') , 			-- ZZLBV_CD(79)
			CONVERT(char(5), '') , 			-- ZZZTM_CD(80)
			CONVERT(char(10), '') ,			-- ZZREINS_CD(81)
			CONVERT(char(5), '') , 			-- ZZLIT_CD(82)
			CONVERT(char(5), '') , 			-- ZZWWZDC(83)
			CONVERT(char(1), '') , 			-- ZZCONVERTED_ITEM(84)
			CONVERT(char(2), '') , 			-- ZZCLEARING_IND(85)
			ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') , 		-- ZZEXTREF(86)
			CONVERT(char(10), '') , 		-- ZZGPART4(87)
			--(SR 318680)
			CONVERT(char(20), dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398)),-- ZZVTREF4(88)
			CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
			CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
			CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
			CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
			CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
			CONVERT(char(1), 1	  ),		-- updt_user_id
			getdate(),  -- updt_dt
			CONVERT(char(1), 1),			-- crte_user_id
			getdate(),  -- crte_dt
			CONVERT(char(1), 1)  ,			-- actv_ind
			PREM_ADJ_PERD.prem_adj_perd_id,
			PREM_ADJ_PERD.prem_adj_id,
			PREM_ADJ_PERD.custmr_id,
			POST_TRNS_TYP.post_trns_typ_id,
			'CHF',0,@Ind,@rel_prem_adj_id
		FROM PREM_ADJ INNER JOIN 
			PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN 
			PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN
			PREM_ADJ_PGM_SETUP ON (PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_ID AND
			PREM_ADJ_PGM_SETUP.incld_ernd_retro_prem_ind=0) INNER JOIN 
			CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN
			LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id 
			AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.prim_ind=1) INNER JOIN 
			POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN 
			PREM_ADJ_PARMET_SETUP ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID 
			AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID 
			AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_ID = PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID 
			AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id 
			AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id)
		WHERE PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=398
			AND POST_TRNS_TYP.POST_TRNS_TYP_ID=74
			AND POST_TRNS_TYP.actv_ind=1
			AND PREM_ADJ_PARMET_SETUP.tot_amt <> 0
			AND PREM_ADJ.prem_adj_id = @prem_adj_id
			
			drop table #tmpCesar

CancelFlg:
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

	if(@err_msg_output is not null)
		set @err_msg_output = 'Error encountered during creation of ARiES transmittals. Please review the error log and take corrective action as appropriate.'
               
end catch
END

go

if object_id('ModAIS_TransmittalToARiES') is not null
	print 'Created Procedure ModAIS_TransmittalToARiES'
else
	print 'Failed Creating Procedure ModAIS_TransmittalToARiES'
go

if object_id('ModAIS_TransmittalToARiES') is not null
	grant exec on ModAIS_TransmittalToARiES to public
go





