if exists (select 1 from sysobjects where name = 'ModAIS_TransmittalToARiES' and type = 'P')
           drop procedure ModAIS_TransmittalToARiES
go
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO


---------------------------------------------------------------------  
-----  
----- Proc Name:  ModAIS_TransmittalToARiES  
-----  
----- Version:  SQL Server 2005  
-----  
----- Created:  Anil Nandakumar 11/18/2008  
-----  
----- Description: This procedure will populate the ARIES_TRNSMTL_HIST table with all the   
-----     transmittals for a given adjustment  
-----               05/21/2010 Venkat Kolimi  
-----                Fixed  the issue 11712 i.e. now the 1500/0111 and 2000/0108 RETRO and PEO postings logic chnaged.  
-----               07/19/2010 --Venkat Kolimi  
-----               Added the postings related to the AIS 21 surcharges   
-----               07/13/2011 --Venkat Kolimi  
-----    As part fo the TFS 14227 Bug Fix,Modified the 7250/0109 and 7250/0111 LBA deposit postings to get the correct reversal in case of revision.  
-----    04/16/2012 --Venkat Kolimi  
-----    As part of the SR 343788 DMR 127409 _ Retro Adjustment POD 2000_0104, We have modified the Retro Adjustment POD 2000/0104 and PEO ADJUSTMENTS (Retro Adjustment POD 2000/0104 to PEO master policy level)  (DUE = VALDATE + 1 YEAR) postings to have t
--he due date same as all the other postings  
-----    07/16/2015 --Venkat Kolimi  
-----    Added the 'C' for Canada file(RT02) in field ZZVTREF4 based on the condition. Issue : INC0877469
  
  
  
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
  @nErrorCode  int,  
  @sErrorMsg  varchar(100),  
  @ID   int,  
  @REVCONDID int,  
  @VOIDCONDID int  
  
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
  
     
--------------------------Start of 11712 Fix---------------------------------------------------------------------------------------------------  
------We are calculating Sum of Retrospective Premium Adjustment amount by  using the following logic and storing the results in table variable named @summary------  
------Based on the Sum of Retrospective Premium Adjustment amount in @summary table we are sending the postings-------------------------------------------------------  
------This will be applicable to :  
         --1)Retro. Premium Adjustment - 1500/0111  
         --2)Def - Retro Bill Reclass 2000/0108    
         --3)PEO ADJUSTMENTS (Rollup Retro. Premium Adjustment - 1500/0111 to PEO master policy level)  
         --4)PEO ADJUSTMENTS (Rollup Def - Retro Bill Reclass 2000/0108 to PEO master policy level)  
  
           --Declaring table variable with the name @summary  
           DECLARE  @SUMMARY TABLE (  
            PREMADJID INT,  
            PREMADJPGMID INT,  
            GRPNAME NVARCHAR(50),  
            NUMVALUE INT,  
            DESCR NVARCHAR(200),  
            FINVALUE DECIMAL(15,2),  
            UNLIM INT,  
            NA INT,  
            MINMAX NVARCHAR(4)  
           )  
  
      INSERT @SUMMARY (PREMADJID, PREMADJPGMID, GRPNAME,NUMVALUE,DESCR,FINVALUE,UNLIM,NA,MINMAX)  
           EXEC GETRETROSUMMARY @prem_adj_id,1,0  
  
           --SELECT * FROM @summary where numvalue=42  
--------------------------------------End of 11712 Fix------------------------------------------------------------------------------------------------------------------  
  
  
  --LBA Adjustment - C&RM - 6000/0106   
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,  
   pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,  
   sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt, bewar_txt,optxt_txt,  
   zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,  
   zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,  
   prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,  
   trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),'')),-- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   --(SR 321581)  
   CONVERT(char(20), ISNULL(RTRIM(CUSTMR.finc_pty_id),'')+'R')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,      -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,      -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,    -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, (ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_amt,0) -   
   ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_prev_biled_amt,0)) * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, (ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_amt,0) -   
   ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_prev_biled_amt,0))  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*(ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_amt,0) -   
   ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_prev_biled_amt,0))  < 0)   
    THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)   
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),       -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3), '') ,    -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2), '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CONVERT(char(20), 
    CASE 
	WHEN CUSTMR.currency_cd=735 
	THEN dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,401)
	ELSE dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,401)+'C'
	END
	),-- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END,  -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
  
  
  --LBA Deposit 7250/0109   
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,  
   pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,  
   sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt, bewar_txt,optxt_txt,  
   zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,  
   zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,  
   prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,  
   Post_cd,rel_prem_adj_id)  
  SELECT   
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   CONVERT(char(20), '')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,      -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,      -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,    -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_depst_amt,0) * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_depst_amt,0)  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*(ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_depst_amt,0))  < 0)   
   THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)   
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3), '') ,    -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2), '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,401)                  
   ELSE dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,401)+'C'    
   END
   ),   -- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
  
  
  --LBA LF Adjustment - C&RM 7250/0111   
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,  
   pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,  
   sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt, bewar_txt,optxt_txt,  
   zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,  
   zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,  
   prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,  
   Post_cd,rel_prem_adj_id)  
  SELECT   
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
  CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   --(SR 321581)  
   CONVERT(char(20), ISNULL(RTRIM(CUSTMR.finc_pty_id),'')+'R')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,      -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,      -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,    -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_depst_amt,0) * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_depst_amt,0)  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*(ISNULL(PREM_ADJ_PARMET_SETUP.los_base_asses_depst_amt,0))  > 0)   
   THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)   
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3), '') ,    -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2), '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,401)               
   ELSE dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,401)+'C'
   END
   ),-- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
  
  
  --LF/Escrow Adjustment-C&RM - 7000/0105   
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
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
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),'')) ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   --(SR 321581)  
   CONVERT(char(20), ISNULL(RTRIM(CUSTMR.finc_pty_id),'')+'R')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*PREM_ADJ_PARMET_SETUP.tot_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,  -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),   -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3), '') ,    -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2), '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN dbo.fn_GetPolicyForEscrowAriesPostings(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID)               
   ELSE dbo.fn_GetPolicyForEscrowAriesPostings(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID)+'C'
   END
   ),-- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
   PREM_ADJ_PERD.prem_adj_perd_id,  
   PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,  
   POST_TRNS_TYP.post_trns_typ_id,  
   'Loss Fund',0,@Ind,@rel_prem_adj_id  
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
   
  
  --LF/Escrow Adjustment-C&RM 7250/0105    
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
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
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   --(SR 321581)  
   CONVERT(char(20), ISNULL(RTRIM(CUSTMR.finc_pty_id),'')+'R')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*PREM_ADJ_PARMET_SETUP.tot_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3), '') ,    -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2), '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN dbo.fn_GetPolicyForEscrowAriesPostings(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID)               
   ELSE dbo.fn_GetPolicyForEscrowAriesPostings(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID)+'C'
   END
   ),-- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
   PREM_ADJ_PERD.prem_adj_perd_id,  
   PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,  
   POST_TRNS_TYP.post_trns_typ_id,  
   'Loss Fund',0,@Ind,@rel_prem_adj_id  
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
   
  
  --Loss Fund/Escrow 7250/0101    
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
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
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   CONVERT(char(20), '')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   --Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"  
   CASE WHEN (@Reverse*PREM_ADJ_PARMET_SETUP.tot_amt  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3), '') ,    -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2), '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN dbo.fn_GetPolicyForEscrowAriesPostings(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID)               
   ELSE dbo.fn_GetPolicyForEscrowAriesPostings(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID)+'C'
   END
   ),-- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
   PREM_ADJ_PERD.prem_adj_perd_id,  
   PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,  
   POST_TRNS_TYP.post_trns_typ_id,  
   'Loss Fund',0,@Ind,@rel_prem_adj_id  
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
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,  
   pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,  
   endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,  
   zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,  
   custmr_id,post_trns_typ_id, adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10), ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   --(SR 321581)  
   CONVERT(char(20), ISNULL(RTRIM(CUSTMR.finc_pty_id),'')+'R')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_RETRO.aries_tot_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.aries_tot_amt  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*PREM_ADJ_RETRO.aries_tot_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16), '') ,    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), '') ,   -- OPTXT(69)  
   CONVERT(char(3), RTRIM(pol_sym_txt)) , -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), RTRIM(pol_nbr_txt)) ,-- ZZPOLICY(71)  
   CONVERT(char(2), pol_modulus_txt),  -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --CONVERT(char(20), '') ,   -- ZZVTREF4(88)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt               
   ELSE RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt+'C'
   END
   ), -- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   --11712 Bug fix condition   
   AND   
   (SELECT CASE WHEN ROUND(FINVALUE,0)<>0 THEN 1 ELSE 0 END FROM @SUMMARY WHERE NUMVALUE=45   
   AND PREMADJPGMID=PREM_ADJ_PGM.PREM_ADJ_PGM_ID   
   AND PREMADJID=@prem_adj_id)<>0  
    
    
     
  --Def - Retro Bill Reclass 2000/0108    
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,  
   pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,  
   endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,  
   zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,  
   custmr_id,post_trns_typ_id, adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10), '')  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   CASE WHEN CUSTMR.company_cd=733 THEN  CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt) ELSE CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt+'C') END  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_RETRO.aries_tot_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.aries_tot_amt  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   --Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"  
   CASE WHEN (@Reverse*PREM_ADJ_RETRO.aries_tot_amt  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16), '') ,    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), '') ,   -- OPTXT(69)  
   CONVERT(char(3), RTRIM(COML_AGMT.pol_sym_txt)) ,  -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), RTRIM(COML_AGMT.pol_nbr_txt)) ,  -- ZZPOLICY(71)  
   CONVERT(char(2), COML_AGMT.pol_modulus_txt),  -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt               
   ELSE RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt+'C'
   END
   )  ,   -- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   --11712 Bug fix condition   
   AND   
   (SELECT CASE WHEN ROUND(FINVALUE,0)<>0 THEN 1 ELSE 0 END FROM @SUMMARY WHERE NUMVALUE=45  
   AND PREMADJPGMID=PREM_ADJ_PGM.PREM_ADJ_PGM_ID   
   AND PREMADJID=@prem_adj_id)<>0  
    
  --Retro Adjustment POD 2000/0104   
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,  
   pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,  
   endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,  
   zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,  
   custmr_id,post_trns_typ_id, adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10), ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   CASE WHEN CUSTMR.company_cd=733 THEN  CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)   
   ELSE CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt+'C') END  ,     -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_RETRO.adj_cash_flw_ben_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.adj_cash_flw_ben_amt  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   --Because it is BALANCED ENTRY (re-class), we send -ve amounts when it is actually positive. ">" used instead of "<"  
   CASE WHEN (@Reverse*PREM_ADJ_RETRO.adj_cash_flw_ben_amt  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16), '') ,    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), '') ,   -- OPTXT(69)  
   CONVERT(char(3), RTRIM(COML_AGMT.pol_sym_txt)) ,  -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), RTRIM(COML_AGMT.pol_nbr_txt)) ,  -- ZZPOLICY(71)  
   CONVERT(char(2), COML_AGMT.pol_modulus_txt),  -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt               
   ELSE RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt+'C'
   END
   )  ,  -- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   
  
  --Retro Adjustment POD 2000/0104 (DUE = VALDATE + 1 YEAR)  
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,  
   pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,  
   endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,  
   zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,  
   custmr_id,post_trns_typ_id, adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10), ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   CASE WHEN CUSTMR.company_cd=733 THEN  CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)   
   ELSE CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt+'C') END  ,     -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_RETRO.adj_cash_flw_ben_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.adj_cash_flw_ben_amt  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*PREM_ADJ_RETRO.adj_cash_flw_ben_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), ''),   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16), '') ,    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), '') ,   -- OPTXT(69)  
   CONVERT(char(3), RTRIM(COML_AGMT.pol_sym_txt)) ,  -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), RTRIM(COML_AGMT.pol_nbr_txt)) ,  -- ZZPOLICY(71)  
   CONVERT(char(2), COML_AGMT.pol_modulus_txt),  -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt               
   ELSE RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt+'C'
   END
   )  ,   -- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   
  
  --WC TPD Adjust - C&RM 6000/0122    
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,  
   pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,  
   endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,  
   zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,  
   custmr_id,post_trns_typ_id, adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   CONVERT(char(20), '')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_RETRO.post_idnmty_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.post_idnmty_amt  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*PREM_ADJ_RETRO.post_idnmty_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,  -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3),  '') ,   -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2),  '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt               
   ELSE RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt+'C'
   END
   ),-- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   
  
  --TPD Clm Res. Req.- WC Adj 6000/0126     
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,  
   pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,  
   endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,  
   zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,  
   custmr_id,post_trns_typ_id, adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),'')) ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   CONVERT(char(20), '')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_RETRO.post_resrv_idnmty_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.post_resrv_idnmty_amt  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*PREM_ADJ_RETRO.post_resrv_idnmty_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3),  '') ,   -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2),  '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt               
   ELSE RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt+'C'
   END
   ),   -- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   
  
  --TPD Clm Reserve Adj-WC 6250/0113      
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,  
   pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,  
   endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,  
   zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,  
   custmr_id,post_trns_typ_id, adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   CONVERT(char(20), '')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_RETRO.post_resrv_idnmty_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.post_resrv_idnmty_amt  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*PREM_ADJ_RETRO.post_resrv_idnmty_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3),  '') ,   -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2),  '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt               
   ELSE RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt+'C'
   END
   ),-- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   
  --TPD Claim Reserve Recd-WC 6250/0110   (Reclass TPD Clm Reserve Adj-WC 6250/0113 )  
  -- As this postings should not send to ARIES and need to shown on only ARIES Exhbit we are setting trnsmtl_sent_ind to 1  
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,  
   pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,  
   endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,  
   zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,  
   custmr_id,post_trns_typ_id, adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   CONVERT(char(20), '')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_RETRO.post_resrv_idnmty_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.post_resrv_idnmty_amt  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   --Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"  
   CASE WHEN (@Reverse*PREM_ADJ_RETRO.post_resrv_idnmty_amt  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3),  '') ,   -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2),  '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt               
   ELSE RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt+'C'
   END
   ),-- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
     
     
   
  --Resid Mkt Chge Retro Adj 1500/0117    
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,  
   pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,  
   endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,  
   zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,  
   custmr_id,post_trns_typ_id, adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10), ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   --(SR 321581)  
   CONVERT(char(20), ISNULL(RTRIM(CUSTMR.finc_pty_id),'')+'R')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_RETRO.rsdl_mkt_load_tot_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.rsdl_mkt_load_tot_amt  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*PREM_ADJ_RETRO.rsdl_mkt_load_tot_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16), '') ,    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), '') ,   -- OPTXT(69)  
   CONVERT(char(3), RTRIM(COML_AGMT.pol_sym_txt)) ,  -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), RTRIM(COML_AGMT.pol_nbr_txt)) ,  -- ZZPOLICY(71)  
   CONVERT(char(2), COML_AGMT.pol_modulus_txt),  -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt               
   ELSE RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt+'C'
   END
   )  ,   -- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   
  
  --Def - Retro Bill Reclass 2000/0108 (Reclass of Resid Mkt Chge Retro Adj 1500/0117 )  
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,  
   pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,  
   endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,  
   zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,  
   custmr_id,post_trns_typ_id, adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10), '')  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   CASE WHEN CUSTMR.company_cd=733 THEN  CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)   
   ELSE CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt+'C') END  ,     -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_RETRO.rsdl_mkt_load_tot_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_RETRO.rsdl_mkt_load_tot_amt  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   --Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"  
   CASE WHEN (@Reverse*PREM_ADJ_RETRO.rsdl_mkt_load_tot_amt  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16), '') ,    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), '') ,   -- OPTXT(69)  
   CONVERT(char(3), RTRIM(COML_AGMT.pol_sym_txt)) ,  -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), RTRIM(COML_AGMT.pol_nbr_txt)) ,  -- ZZPOLICY(71)  
   CONVERT(char(2), COML_AGMT.pol_modulus_txt),  -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt               
   ELSE RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt+'C'
   END
   )  ,  -- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   
   
  
  --ILRF Adjustment - C&RM 7000/0107  
   INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,  
   pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,  
   sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt, bewar_txt,optxt_txt,  
   zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,  
   zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,  
   prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,  
   Post_cd,rel_prem_adj_id)  
  SELECT   
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   --(SR 321581)  
   CONVERT(char(20), ISNULL(RTRIM(CUSTMR.finc_pty_id),'')+'R')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*PREM_ADJ_PARMET_SETUP.tot_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)     
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3), '') ,    -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2), '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,400)               
   ELSE dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,400)+'C'
   END
   ),-- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   AND PREM_ADJ_PARMET_SETUP.tot_amt  > 0  
   AND PREM_ADJ.prem_adj_id = @prem_adj_id  
   
  
  --ILRF Adjustment - C&RM 7250/0107   
   INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,  
   pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,  
   sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt, bewar_txt,optxt_txt,  
   zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,  
   zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,  
   prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,  
   Post_cd,rel_prem_adj_id)  
  SELECT   
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   --(SR 321581)  
   CONVERT(char(20), ISNULL(RTRIM(CUSTMR.finc_pty_id),'')+'R')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt   * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*PREM_ADJ_PARMET_SETUP.tot_amt   < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,  -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3), '') ,    -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2), '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,400)               
   ELSE dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,400)+'C'
   END
   ),-- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   AND PREM_ADJ_PARMET_SETUP.tot_amt  < 0 -- WHEN ILRF Amount is <0  
   AND PREM_ADJ.prem_adj_id = @prem_adj_id  
   
  
  --Incurred Loss Reimb.Fund 7250/0100   
   INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,  
   pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,  
   sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt, bewar_txt,optxt_txt,  
   zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,  
   zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,  
   prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,  
   Post_cd,rel_prem_adj_id)  
  SELECT   
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   CONVERT(char(20), '')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_PARMET_SETUP.tot_amt  * 100)),   
   --Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"  
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*PREM_ADJ_PARMET_SETUP.tot_amt > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),   -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3), '') ,    -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2), '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,400)               
   ELSE dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,400)+'C'
   END
   ),-- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   AND PREM_ADJ_PARMET_SETUP.tot_amt  < 0 -- WHEN ILRF Amount is <0   
   AND PREM_ADJ.prem_adj_id = @prem_adj_id  
   
  
 -- Auto TPD Adjust - C&RM 6000/0120    
 -- GL TPD Adjust - C&RM  6000/0121  
 -- WC TPD Adjust - C&RM  6000/0122  
 -- Auto LCF Adjust - C&RM 6000/0123     
 -- GL LCF Adjust - C&RM  6000/0124    
 -- WC LCF Adjust - C&RM  6000/0125    
 -- LBA Adjustment - C&RM 6000/0106   
 INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
  pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
  gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
  endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
  vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
  emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,  
  pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,  
  sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt, bewar_txt,optxt_txt,  
  zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
  yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
  zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,  
  zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,  
  prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
 SELECT   
     CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
  CONVERT(char(2), '01') ,   -- AKTYP(2)  
  CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),'')) ,   -- GPART(3)  
  CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
  CONVERT(char(20), '')  ,    -- VTREF(5)  
  CONVERT(char(12), '')  ,   -- POSNR(6)  
  CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
  ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
  ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
  ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
  ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
  CONVERT(char(8), '') ,    -- PMEND(12)  
  CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
  CONVERT(char(1), '') ,    -- RENEW(14)  
  CONVERT(char(3), '') ,    -- RNEWX(15)  
  
  CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
  CONVERT(char(4), '') ,    -- OPCCODE(17)  
  CONVERT(char(4), '') ,    -- GSBER(18)  
  CONVERT(char(4), '') ,    -- OPGSBER(19)  
  CONVERT(char(2), '') ,    -- PRGRP(20)  
  CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
  CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
  CONVERT(bigint, PREM_ADJ_LOS_REIM_FUND_POST.post_amt  * 100)))) +  
  CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_LOS_REIM_FUND_POST.post_amt  * 100)),   
  '000000000000000'), ' ', '0'), '-', '0')) +   
  CASE WHEN (@Reverse*PREM_ADJ_LOS_REIM_FUND_POST.post_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
  CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
  CONVERT(char(8), '') ,   -- ATFRD(24)  
  CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
  CONVERT(char(1), '') ,    -- ENDTYPE(26)  
  CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
  CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
  CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
  CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
  CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
  CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
  CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
  CONVERT(char(2), 'PP') ,   -- BLART(34)  
  CONVERT(char(20), '') ,   -- VTRE2(35)  
  CONVERT(char(20), '') ,   -- VTRE3(36)  
  CONVERT(char(10), '') ,   -- VGPART2(37)  
  CONVERT(char(10), '') ,   -- VGPART3(38)  
  CONVERT(char(16), '') ,   -- GSFNR(39)  
  CONVERT(char(6), '') ,    -- BELNR(40)  
  CONVERT(char(2), '20') ,   -- BLTYP(41)  
  CONVERT(char(10), '') ,   -- EMGPA(42)  
  CONVERT(char(4), '') ,    -- EMBVT(43)  
  CONVERT(char(10), '') ,   -- EMADR(44)  
  CONVERT(char(1), 3) ,   -- PYMET(45)  
  CONVERT(char(4), '') ,    -- PYBUK(46)  
 ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
  CONVERT(char(8), '') ,    -- BUDAT(48)  
  ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
  CONVERT(char(1), '') ,    -- DUN_REASON(50)  
  CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
  CONVERT(char(1), '') ,    -- PAY_REASON(52)  
  CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
  CONVERT(char(1), '') ,    -- CLR_REASON(54)  
  CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
  CONVERT(char(10), '') ,   -- KOSTL(56)  
  CONVERT(char(10), '') ,   -- PRCTR(57)  
  CONVERT(char(10), '') ,   -- PYGRP(58)  
  CONVERT(char(3), '') ,    -- GRKEY(59)  
  CONVERT(char(1), '') ,    -- ENDREV(60)  
  CONVERT(char(8), '') ,    -- STO_FROM(61)  
  CONVERT(char(8), '') ,    -- STO_TO(62)  
  CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
  CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
  CONVERT(char(6), '') ,    -- VBUND(65)  
  CONVERT(char(16),''),    -- REFGF(66)  
  CONVERT(char(6), '') ,    -- REFBL(67)  
  CONVERT(char(3), '1') ,   -- BEWAR(68)  
  CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
  ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
  CONVERT(char(3), '') ,    -- ZZPOLSYMBOL(70)  
  CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
  CONVERT(char(2), '') ,    -- ZZMOD(72)  
  CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
  CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
  CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
  CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
  CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
  CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
  CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
  CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
  CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
  CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
  CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
  CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
  CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
  ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
  CONVERT(char(10), '') ,   -- ZZGPART4(87)  
  --(SR 318680)  
  CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,400)               
   ELSE dbo.fn_GetPolicyForNonERPAries(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,400)+'C'
   END
  ),-- ZZVTREF4(88)  
  CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
  CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
  CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
  CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
  CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
  CONVERT(char(1), 1   ),  -- updt_user_id  
  getdate(),  -- updt_dt  
  CONVERT(char(1), 1),   -- crte_user_id  
  getdate(),  -- crte_dt  
  CONVERT(char(1), 1)  ,   -- actv_ind  
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
  AND POST_TRNS_TYP.POST_TRNS_TYP_ID NOT IN(71,140) -- DO NOT POST RESERVES #71 and CHF  
  AND POST_TRNS_TYP.actv_ind=1  
  AND PREM_ADJ_LOS_REIM_FUND_POST.post_amt <> 0   
  AND PREM_ADJ.prem_adj_id = @prem_adj_id  
  
  
    
   
   
  
  --Misc Postings  
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,  
   pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,  
   endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,  
   zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,  
   custmr_id,post_trns_typ_id, adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
   CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
  
   CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN   
    CASE WHEN ISNULL(CONVERT(BIGINT,LSI_CUSTMR.lsi_acct_id),0)=0    
    THEN isnull(CONVERT(char(10),CONVERT(BIGINT,finc_pty_id)), ' ')    
    ELSE CONVERT(char(10), '') END     
   ELSE  
    CONVERT(char(10), '')    
   END,       -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
  
   CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN   
    CONVERT(char(20), '')   
   ELSE   
   CASE WHEN CUSTMR.company_cd=733 THEN  CONVERT(char(20), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)+RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)+PREM_ADJ_MISC_INVC.pol_modulus_txt)   
   ELSE CONVERT(char(20), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)+RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)+PREM_ADJ_MISC_INVC.pol_modulus_txt+'C') END      
   END,    -- VTREF(5)  
  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_MISC_INVC.post_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_MISC_INVC.post_amt  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*PREM_ADJ_MISC_INVC.post_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
  
   CONVERT(char(16),''),    -- REFGF(66)  
    CONVERT(char(6), '') ,    -- REFBL(67)  
    CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN  
    CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
    ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END)   
    ELSE  CONVERT(char(50), '') END,   -- OPTXT(69)  
  
   CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1)   
   THEN CONVERT(char(3), '')    
   ELSE CONVERT(char(3), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)) END,     -- ZZPOLSYMBOL(70)  
   CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1)   
   THEN CONVERT(char(3), '')    
   ELSE CONVERT(char(18), RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)) END,     -- ZZPOLICY(71)  
   CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1)   
   THEN CONVERT(char(3), '')    
   ELSE CONVERT(char(2), PREM_ADJ_MISC_INVC.pol_modulus_txt) END,   -- ZZMOD(72)  
  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
  
   --CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN   
    --CONVERT(char(20), '')   
   --ELSE   
    --CONVERT(char(20), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)+  
    --RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)+PREM_ADJ_MISC_INVC.pol_modulus_txt)   
   --END,    -- ZZVTREF4(88)  
   --(SR 318680)  
   CONVERT(char(20),
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN dbo.fn_GetPolicyForMiscAriesPostings(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,POST_TRNS_TYP.POST_TRNS_TYP_ID)               
   ELSE dbo.fn_GetPolicyForMiscAriesPostings(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,POST_TRNS_TYP.POST_TRNS_TYP_ID)+'C'
   END
   ),-- ZZVTREF4(88)  
  
  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,  
   pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,  
   endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,  
   zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,  
   custmr_id,post_trns_typ_id, adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
   CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),'')),       -- GPART(3)  
  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
  
   CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN   
    CONVERT(char(20), '')   
   ELSE   
     
    --CONVERT(char(20), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)+  
    --RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)+PREM_ADJ_MISC_INVC.pol_modulus_txt)   
      
   CASE WHEN CUSTMR.company_cd=733 THEN  CONVERT(char(20), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)+RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)+PREM_ADJ_MISC_INVC.pol_modulus_txt)   
   ELSE CONVERT(char(20), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)+RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)+PREM_ADJ_MISC_INVC.pol_modulus_txt+'C') END     
      
   END,    -- VTREF(5)  
  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_MISC_INVC.post_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_MISC_INVC.post_amt  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   --Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"  
   CASE WHEN (@Reverse*PREM_ADJ_MISC_INVC.post_amt  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
  
   CONVERT(char(16),''),    -- REFGF(66)  
    CONVERT(char(6), '') ,    -- REFBL(67)  
    CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN  
    CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
    ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END)   
    ELSE  CONVERT(char(50), '') END,   -- OPTXT(69)  
  
   CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1)   
   THEN CONVERT(char(3), '')    
   ELSE CONVERT(char(3), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)) END,     -- ZZPOLSYMBOL(70)  
   CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1)   
   THEN CONVERT(char(3), '')    
   ELSE CONVERT(char(18), RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)) END,     -- ZZPOLICY(71)  
   CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1)   
   THEN CONVERT(char(3), '')    
   ELSE CONVERT(char(2), PREM_ADJ_MISC_INVC.pol_modulus_txt) END,   -- ZZMOD(72)  
  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
  
   --CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN   
    --CONVERT(char(20), '')   
   --ELSE   
    --CONVERT(char(20), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)+  
    --RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)+PREM_ADJ_MISC_INVC.pol_modulus_txt)   
   --END,    -- ZZVTREF4(88)  
   --(SR 318680)  
   CONVERT(char(20),
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN dbo.fn_GetPolicyForMiscAriesPostings(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,POST_TRNS_TYP.POST_TRNS_TYP_ID)              
   ELSE dbo.fn_GetPolicyForMiscAriesPostings(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,POST_TRNS_TYP.POST_TRNS_TYP_ID)+'C'
   END
   ),-- ZZVTREF4(88)  
  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,  
   pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,  
   endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,  
   zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,  
   custmr_id,post_trns_typ_id, adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
   CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),'')),       -- GPART(3)  
  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
  
   CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN   
    CONVERT(char(20), '')   
   ELSE   
   CASE WHEN CUSTMR.company_cd=733 THEN  CONVERT(char(20), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)+RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)+PREM_ADJ_MISC_INVC.pol_modulus_txt)   
   ELSE CONVERT(char(20), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)+RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)+PREM_ADJ_MISC_INVC.pol_modulus_txt+'C') END     
   END,    -- VTREF(5)  
  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_MISC_INVC.post_amt  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_MISC_INVC.post_amt  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   --Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"  
   CASE WHEN (@Reverse*PREM_ADJ_MISC_INVC.post_amt  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
  
   CONVERT(char(16),''),    -- REFGF(66)  
    CONVERT(char(6), '') ,    -- REFBL(67)  
    CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN  
    CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
    ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END)   
    ELSE  CONVERT(char(50), '') END,   -- OPTXT(69)  
  
   CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1)   
    THEN CONVERT(char(3), '')    
   ELSE CONVERT(char(3), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)) END,     -- ZZPOLSYMBOL(70)  
   CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1)   
   THEN CONVERT(char(3), '')    
   ELSE CONVERT(char(18), RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)) END,     -- ZZPOLICY(71)  
   CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1)   
   THEN CONVERT(char(3), '')    
   ELSE CONVERT(char(2), PREM_ADJ_MISC_INVC.pol_modulus_txt) END,   -- ZZMOD(72)  
  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
  
   --CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN   
    --CONVERT(char(20), '')   
   --ELSE   
    --CONVERT(char(20), RTRIM(PREM_ADJ_MISC_INVC.pol_sym_txt)+  
    --RTRIM(PREM_ADJ_MISC_INVC.pol_nbr_txt)+PREM_ADJ_MISC_INVC.pol_modulus_txt)   
   --END,    -- ZZVTREF4(88)  
   --(SR 318680)  
   CONVERT(char(20),
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN dbo.fn_GetPolicyForMiscAriesPostings(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,POST_TRNS_TYP.POST_TRNS_TYP_ID)              
   ELSE dbo.fn_GetPolicyForMiscAriesPostings(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,POST_TRNS_TYP.POST_TRNS_TYP_ID)+'C'
   END
   ),-- ZZVTREF4(88)  
  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   
        
   --PEO ADJUSTMENTS (Rollup Retro. Premium Adjustment - 1500/0111 to PEO master policy level)(Postings with condition on cesar coding amounts)  
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,  
   pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,  
   endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,  
   zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,  
   custmr_id,post_trns_typ_id, adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
   CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(MAX(CUSTMR.finc_pty_id)),'')),       -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   --(SR 321581)  
   CONVERT(char(20), ISNULL(RTRIM(MAX(CUSTMR.finc_pty_id)),'')+'R')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,     -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,     -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
   --CONVERT(char(16), PAPS.tot_amt) ,   -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, SUM(PREM_ADJ_RETRO.aries_tot_amt)  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, SUM(PREM_ADJ_RETRO.aries_tot_amt)  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*SUM(PREM_ADJ_RETRO.aries_tot_amt)  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16), '') ,    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), '') ,   -- OPTXT(69)  
   CONVERT(char(3), CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 THEN SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),1,3)   
     else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),1,2) END) , -- ZZPOLSYMBOL(70)  
   CONVERT(char(18),CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 THEN  SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),4,8)  
     else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),3,8) end ) ,-- ZZPOLICY(71)  
   CONVERT(char(2),CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 then  SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),12,2)  
                    else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),11,2) end),  -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --CONVERT(char(20), '') ,   -- ZZVTREF4(88)  
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN CONVERT(char(20),dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))              
   ELSE CONVERT(char(20),dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id)+'C')
   END, -- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   --11712 Bug fix condition  
   AND   
   (SELECT CASE WHEN ROUND(FINVALUE,0)<>0 THEN 1 ELSE 0 END FROM @SUMMARY WHERE NUMVALUE=45  
   AND PREMADJPGMID=PREM_ADJ_PGM.PREM_ADJ_PGM_ID   
   AND PREMADJID=@prem_adj_id)<>0  
  GROUP BY  
   PREM_ADJ.valn_dt,PREM_ADJ_PGM.strt_dt,PREM_ADJ_PGM.plan_end_dt,  
   POST_TRNS_TYP.main_nbr_txt,POST_TRNS_TYP.sub_nbr_txt,PREM_ADJ.invc_due_dt,  
   PREM_ADJ.fnl_invc_dt,PREM_ADJ.fnl_invc_nbr_txt,POST_TRNS_TYP.comp_txt,  
   PREM_ADJ_PERD.prem_adj_perd_id,PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,POST_TRNS_TYP.post_trns_typ_id,  
   POST_TRNS_TYP.trns_nm_txt,PREM_ADJ_PGM.prem_adj_pgm_id,  
   CASE WHEN CUSTMR.company_cd=733  THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END,  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(20),dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id)) ELSE CONVERT(char(20),dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id)+'C') END           
  HAVING SUM(PREM_ADJ_RETRO.aries_tot_amt) <> 0  
     
     
    
  
  --PEO ADJUSTMENTS (Retro Adjustment POD 2000/0104 to PEO master policy level)  
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,  
   pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,  
   endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,  
   zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,  
   custmr_id,post_trns_typ_id, adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
   CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(MAX(CUSTMR.finc_pty_id)),'')),       -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   --CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))  ,    -- VTREF(5)  
   CASE WHEN CUSTMR.company_cd=733 THEN  CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))   
   ELSE CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))+'C' END  , -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,     -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,     -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   --Because it is BALANCED ENTRY (re-class), we send -ve amounts when it is actually positive. ">" used instead of "<"  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, SUM(PREM_ADJ_RETRO.adj_cash_flw_ben_amt)  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, SUM(PREM_ADJ_RETRO.adj_cash_flw_ben_amt)  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*SUM(PREM_ADJ_RETRO.adj_cash_flw_ben_amt)  > 0)    
   THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16), '') ,    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), '') ,   -- OPTXT(69)  
   CONVERT(char(3), CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 THEN SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),1,3)   
     else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),1,2) END) , -- ZZPOLSYMBOL(70)  
   CONVERT(char(18),CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 THEN  SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),4,8)  
     else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),3,8) end ) ,-- ZZPOLICY(71)  
   CONVERT(char(2),CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 then  SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),12,2)  
                    else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),11,2) end),  -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN  CONVERT(char(20),dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))              
   ELSE  CONVERT(char(20),dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id)+'C')
   END,-- ZZVTREF4(88) 
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   POST_TRNS_TYP.trns_nm_txt,PREM_ADJ_PGM.prem_adj_pgm_id,  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END,  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END ,  
   CASE WHEN CUSTMR.company_cd=733 THEN  CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))   
   ELSE CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))+'C' END,
   CASE WHEN CUSTMR.currency_cd=735 THEN  CONVERT(char(20),dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))              
   ELSE  CONVERT(char(20),dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id)+'C') END  
  HAVING SUM(PREM_ADJ_RETRO.adj_cash_flw_ben_amt) <> 0  
  
  --PEO ADJUSTMENTS (Retro Adjustment POD 2000/0104 to PEO master policy level)  (DUE = VALDATE + 1 YEAR)  
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,  
   pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,  
   endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,  
   zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,  
   custmr_id,post_trns_typ_id, adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
   CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(MAX(CUSTMR.finc_pty_id)),'')),       -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   CASE WHEN CUSTMR.company_cd=733 THEN  CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))   
   ELSE CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))+'C' END  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,     -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,     -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   --Because it is BALANCED ENTRY (re-class), we send -ve amounts when it is actually positive. ">" used instead of "<"  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, SUM(PREM_ADJ_RETRO.adj_cash_flw_ben_amt)  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, SUM(PREM_ADJ_RETRO.adj_cash_flw_ben_amt)  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*SUM(PREM_ADJ_RETRO.adj_cash_flw_ben_amt)  < 0)    
   THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
 
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), ''),  -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16), '') ,    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), '') ,   -- OPTXT(69)  
   CONVERT(char(3), CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 THEN SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),1,3)   
     else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),1,2) END) , -- ZZPOLSYMBOL(70)  
   CONVERT(char(18),CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 THEN  SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),4,8)  
     else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),3,8) end ) ,-- ZZPOLICY(71)  
   CONVERT(char(2),CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 then  SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),12,2)  
                    else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),11,2) end),  -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN CONVERT(char(20),dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))              
   ELSE CONVERT(char(20),dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id)+'C')
   END,-- ZZVTREF4(88)
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   POST_TRNS_TYP.trns_nm_txt,PREM_ADJ_PGM.prem_adj_pgm_id,  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END,  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,  
   CASE WHEN CUSTMR.company_cd=733 THEN  CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))   
   ELSE CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))+'C' END,
   CASE WHEN CUSTMR.currency_cd=735 
   THEN CONVERT(char(20),dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))              
   ELSE CONVERT(char(20),dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id)+'C')
   END 
  HAVING SUM(PREM_ADJ_RETRO.adj_cash_flw_ben_amt) <> 0  
  
       
   --PEO ADJUSTMENTS (Rollup Def - Retro Bill Reclass 2000/0108 to PEO master policy level)(Postings with condition on cesar coding amounts)    
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,  
   pay_rsn_txt,pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,  
   endrev_txt,sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt,  
   bewar_txt,optxt_txt,zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,zzccd_txt,  
   zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,prem_adj_perd_id,prem_adj_id,  
   custmr_id,post_trns_typ_id, adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
   CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10), '')  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   CASE WHEN CUSTMR.company_cd=733 THEN  CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))   
   ELSE CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))+'C' END,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,     -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,     -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, SUM(PREM_ADJ_RETRO.aries_tot_amt)  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, SUM(PREM_ADJ_RETRO.aries_tot_amt)  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*SUM(PREM_ADJ_RETRO.aries_tot_amt)  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16), '') ,    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), '') ,   -- OPTXT(69)  
   CONVERT(char(3), CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 THEN SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),1,3)   
     else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),1,2) END) , -- ZZPOLSYMBOL(70)  
   CONVERT(char(18),CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 THEN  SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),4,8)  
     else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),3,8) end ) ,-- ZZPOLICY(71)  
   CONVERT(char(2),CASE WHEN dbo.fn_GetPEOMasterPolicySymbolLength(PREM_ADJ_PGM.prem_adj_pgm_id)=3 then  SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),12,2)  
                    else SUBSTRING(dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id),11,2) end),  -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))              
   ELSE CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id)+'C')
   END,-- ZZVTREF4(88)
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
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
   --11712 Bug fix condition   
   AND   
   (SELECT CASE WHEN ROUND(FINVALUE,0)<>0 THEN 1 ELSE 0 END FROM @SUMMARY WHERE NUMVALUE=45   
   AND PREMADJPGMID=PREM_ADJ_PGM.PREM_ADJ_PGM_ID   
   AND PREMADJID=@prem_adj_id)<>0  
  GROUP BY  
   PREM_ADJ.valn_dt,PREM_ADJ_PGM.strt_dt,PREM_ADJ_PGM.plan_end_dt,  
   POST_TRNS_TYP.main_nbr_txt,POST_TRNS_TYP.sub_nbr_txt,PREM_ADJ.invc_due_dt,  
   PREM_ADJ.fnl_invc_dt,PREM_ADJ.fnl_invc_nbr_txt,POST_TRNS_TYP.comp_txt,  
   PREM_ADJ_PERD.prem_adj_perd_id,PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,POST_TRNS_TYP.post_trns_typ_id,  
   POST_TRNS_TYP.trns_nm_txt,PREM_ADJ_PGM.prem_adj_pgm_id,  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END,  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,  
   CASE WHEN CUSTMR.company_cd=733 THEN  CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))   
   ELSE CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))+'C' END,
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id))              
   ELSE CONVERT(char(20), dbo.fn_GetPEOMasterPolicy(PREM_ADJ_PGM.prem_adj_pgm_id)+'C')
   END  
  HAVING SUM(PREM_ADJ_RETRO.aries_tot_amt) <> 0  
    
     
     
  --Clm Service Fee Adj-C&RM 8000/0130 for Incurred Loss Retro and Deductible Policies  
  --ZSC Indicator at Program Period Level is FALSE  
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,  
   pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,  
   sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt, bewar_txt,optxt_txt,  
   zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,  
   zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,  
   prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,  
   Post_cd,rel_prem_adj_id)  
  SELECT   
   CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   --(SR 321581)  
   CONVERT(char(20), ISNULL(RTRIM(CUSTMR.finc_pty_id),'')+'R')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,      -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,      -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,    -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, SUM(PREM_ADJ_PARMET_SETUP.tot_amt)  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, SUM(PREM_ADJ_PARMET_SETUP.tot_amt)  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*SUM(PREM_ADJ_PARMET_SETUP.tot_amt)  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3), '') ,    -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2), '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN  CONVERT(char(20), dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'R') )             
   ELSE  CONVERT(char(20), dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'R')+'C')
   END,-- ZZVTREF4(88) 
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
   PREM_ADJ_PERD.prem_adj_perd_id,  
   PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,  
   POST_TRNS_TYP.post_trns_typ_id,  
   'CHF',0,@Ind,@rel_prem_adj_id  
  FROM PREM_ADJ INNER JOIN   
   PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID   
   AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN   
   PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1 AND PREM_ADJ_PGM.ZNA_SERV_COMP_CLM_HNDL_FEE_IND<>1) INNER JOIN  
   PREM_ADJ_PGM_SETUP ON (PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_ID AND  
   PREM_ADJ_PGM_SETUP.incld_ernd_retro_prem_ind=0) INNER JOIN   
   CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN  
   LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id   
   AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.chf_ind=1) INNER JOIN   
   POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN   
   PREM_ADJ_PARMET_SETUP ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID   
   AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID   
   AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_ID = PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID   
   AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id   
   AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id)   
   --INNER JOIN  
   --PREM_ADJ_PARMET_DTL ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PARMET_SETUP_ID=PREM_ADJ_PARMET_DTL.prem_adj_parmet_setup_id  
   --AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ_PARMET_DTL.PREM_ADJ_ID) INNER JOIN  
   --COML_AGMT ON (PREM_ADJ_PARMET_DTL.COML_AGMT_ID=COML_AGMT.COML_AGMT_ID   
   --AND COML_AGMT.ADJ_TYP_ID NOT IN(62,68,448))  
  WHERE PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=398  
   AND POST_TRNS_TYP.POST_TRNS_TYP_ID=74  
   AND POST_TRNS_TYP.actv_ind=1  
   AND PREM_ADJ_PARMET_SETUP.tot_amt <> 0  
   AND PREM_ADJ.prem_adj_id = @prem_adj_id  
   AND dbo.fn_IsILRFPolicy(@prem_adj_id,PREM_ADJ_PERD.PREM_ADJ_PERD_ID)<>1  
  GROUP BY  
   PREM_ADJ.valn_dt,PREM_ADJ_PGM.strt_dt,PREM_ADJ_PGM.plan_end_dt,  
   POST_TRNS_TYP.main_nbr_txt,POST_TRNS_TYP.sub_nbr_txt,PREM_ADJ.invc_due_dt,  
   PREM_ADJ.fnl_invc_dt,PREM_ADJ.fnl_invc_nbr_txt,POST_TRNS_TYP.comp_txt,  
   LSI_CUSTMR.lsi_acct_id,CUSTMR.finc_pty_id,  
   PREM_ADJ_PERD.prem_adj_perd_id,PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,POST_TRNS_TYP.post_trns_typ_id,  
   POST_TRNS_TYP.trns_nm_txt,PREM_ADJ_PGM.prem_adj_pgm_id,PREM_ADJ.PREM_ADJ_ID,  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END,  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN  CONVERT(char(20), dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'R') )             
   ELSE  CONVERT(char(20), dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'R')+'C')
   END  
  
  
  
  --Clm Service Fee Adj-C&RM 8000/0130 for ILRF and LOSS FUND Policies  
  --ZSC Indicator at Program Period Level is FALSE  
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,  
   pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,  
   sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt, bewar_txt,optxt_txt,  
   zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,  
   zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,  
   prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,  
   Post_cd,rel_prem_adj_id)  
  SELECT   
   CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   --(SR 321581)  
   CONVERT(char(20), ISNULL(RTRIM(CUSTMR.finc_pty_id),'')+'D')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,      -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,      -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,    -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, SUM(PREM_ADJ_PARMET_SETUP.tot_amt)  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, SUM(PREM_ADJ_PARMET_SETUP.tot_amt)  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*SUM(PREM_ADJ_PARMET_SETUP.tot_amt)  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3), '') ,    -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2), '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN CONVERT(char(20), dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'D'))              
   ELSE CONVERT(char(20), dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'D')+'C')
   END,-- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
   PREM_ADJ_PERD.prem_adj_perd_id,  
   PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,  
   POST_TRNS_TYP.post_trns_typ_id,  
   'ILRF',0,@Ind,@rel_prem_adj_id  
  FROM PREM_ADJ INNER JOIN   
   PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID   
   AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN   
   PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1 AND PREM_ADJ_PGM.ZNA_SERV_COMP_CLM_HNDL_FEE_IND<>1) INNER JOIN  
   PREM_ADJ_PGM_SETUP ON (PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_ID AND  
   PREM_ADJ_PGM_SETUP.incld_ernd_retro_prem_ind=0) INNER JOIN   
   CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN  
   LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id   
   AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.chf_ind=1) INNER JOIN   
   POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN   
   PREM_ADJ_PARMET_SETUP ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID   
   AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID   
   AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_ID = PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID   
   AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id   
   AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id) 
   --INNER JOIN  
   --PREM_ADJ_PARMET_DTL ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PARMET_SETUP_ID=PREM_ADJ_PARMET_DTL.prem_adj_parmet_setup_id  
   --AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ_PARMET_DTL.PREM_ADJ_ID) INNER JOIN  
   --COML_AGMT ON (PREM_ADJ_PARMET_DTL.COML_AGMT_ID=COML_AGMT.COML_AGMT_ID   
   --AND COML_AGMT.ADJ_TYP_ID IN(62,68,448))  
  WHERE PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=398  
   AND POST_TRNS_TYP.POST_TRNS_TYP_ID=74  
   AND POST_TRNS_TYP.actv_ind=1  
   AND PREM_ADJ_PARMET_SETUP.tot_amt <> 0  
   AND PREM_ADJ.prem_adj_id = @prem_adj_id  
   AND dbo.fn_IsILRFPolicy(@prem_adj_id,PREM_ADJ_PERD.PREM_ADJ_PERD_ID)=1
  GROUP BY  
   PREM_ADJ.valn_dt,PREM_ADJ_PGM.strt_dt,PREM_ADJ_PGM.plan_end_dt,  
   POST_TRNS_TYP.main_nbr_txt,POST_TRNS_TYP.sub_nbr_txt,PREM_ADJ.invc_due_dt,  
   PREM_ADJ.fnl_invc_dt,PREM_ADJ.fnl_invc_nbr_txt,POST_TRNS_TYP.comp_txt,  
   LSI_CUSTMR.lsi_acct_id,CUSTMR.finc_pty_id,  
   PREM_ADJ_PERD.prem_adj_perd_id,PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,POST_TRNS_TYP.post_trns_typ_id,  
   POST_TRNS_TYP.trns_nm_txt,PREM_ADJ_PGM.prem_adj_pgm_id,PREM_ADJ.PREM_ADJ_ID,  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END,  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN CONVERT(char(20), dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'D'))              
   ELSE CONVERT(char(20), dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'D')+'C')
   END
  
  
  --ZSC Claim Service fee 8000/5400 for Incurred Loss Retro and Deductible Policies  
  --ZSC Indicator at Program Period Level is TRUE and Excluding the Admin Fee Categories(508,774,775)  
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,  
   pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,  
   sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt, bewar_txt,optxt_txt,  
   zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,  
   zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,  
   prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,  
   Post_cd,rel_prem_adj_id)  
  SELECT   
   CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   --(SR 321581)  
   CONVERT(char(20), ISNULL(RTRIM(CUSTMR.finc_pty_id),'')+'R')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,      -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,      -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,    -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, SUM(PREM_ADJ_PARMET_SETUP.tot_amt)  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, SUM(PREM_ADJ_PARMET_SETUP.tot_amt)  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*SUM(PREM_ADJ_PARMET_SETUP.tot_amt)  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3), '') ,    -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2), '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN CONVERT(char(20), dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'R'))              
   ELSE CONVERT(char(20), dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'R')+'C')
   END,-- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
   PREM_ADJ_PERD.prem_adj_perd_id,  
   PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,  
   POST_TRNS_TYP.post_trns_typ_id,  
   'CHF',0,@Ind,@rel_prem_adj_id  
  FROM PREM_ADJ INNER JOIN   
   PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID   
   AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN   
   PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1 AND PREM_ADJ_PGM.ZNA_SERV_COMP_CLM_HNDL_FEE_IND=1) INNER JOIN  
   PREM_ADJ_PGM_SETUP ON (PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_ID AND  
   PREM_ADJ_PGM_SETUP.incld_ernd_retro_prem_ind=0) INNER JOIN   
   CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN  
   LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id   
   AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.chf_ind=1) INNER JOIN   
   POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN   
   PREM_ADJ_PARMET_SETUP ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID   
   AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID   
   AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_ID = PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID   
   AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id   
   AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id) 
   --INNER JOIN  
   --PREM_ADJ_PARMET_DTL ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PARMET_SETUP_ID=PREM_ADJ_PARMET_DTL.prem_adj_parmet_setup_id  
   --AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ_PARMET_DTL.PREM_ADJ_ID AND PREM_ADJ_PARMET_DTL.CLM_HNDL_FEE_LOS_TYP_ID NOT IN(507,775,776)) INNER JOIN  
   --COML_AGMT ON (PREM_ADJ_PARMET_DTL.COML_AGMT_ID=COML_AGMT.COML_AGMT_ID   
   --AND COML_AGMT.ADJ_TYP_ID NOT IN(62,68,448))  
  WHERE PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=398  
   AND POST_TRNS_TYP.POST_TRNS_TYP_ID=38  
   AND POST_TRNS_TYP.actv_ind=1  
   AND PREM_ADJ_PARMET_SETUP.tot_amt <> 0  
   AND PREM_ADJ.prem_adj_id = @prem_adj_id  
   AND [dbo].[fn_ILR_Ded_Policy](@prem_adj_id,PREM_ADJ_PERD.PREM_ADJ_PERD_ID)<>1 
  GROUP BY  
   PREM_ADJ.valn_dt,PREM_ADJ_PGM.strt_dt,PREM_ADJ_PGM.plan_end_dt,  
   POST_TRNS_TYP.main_nbr_txt,POST_TRNS_TYP.sub_nbr_txt,PREM_ADJ.invc_due_dt,  
   PREM_ADJ.fnl_invc_dt,PREM_ADJ.fnl_invc_nbr_txt,POST_TRNS_TYP.comp_txt,  
   LSI_CUSTMR.lsi_acct_id,CUSTMR.finc_pty_id,  
   PREM_ADJ_PERD.prem_adj_perd_id,PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,POST_TRNS_TYP.post_trns_typ_id,  
   POST_TRNS_TYP.trns_nm_txt,PREM_ADJ_PGM.prem_adj_pgm_id,PREM_ADJ.PREM_ADJ_ID,  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END,  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN CONVERT(char(20), dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'R'))              
   ELSE CONVERT(char(20), dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'R')+'C')
   END  
  
  
  --ZSC Administration Fee  8000/5402 for Incurred Loss Retro and Deductible Policies  
  --ZSC Indicator at Program Period Level is TRUE and  the CHF on Admin Fee Categories(508,774,775)  
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,  
   pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,  
   sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt, bewar_txt,optxt_txt,  
   zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,  
   zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,  
   prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,  
   Post_cd,rel_prem_adj_id)  
  SELECT   
   CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   --(SR 321581)  
   CONVERT(char(20), ISNULL(RTRIM(CUSTMR.finc_pty_id),'')+'R')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,      -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,      -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,    -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, SUM(PREM_ADJ_PARMET_DTL.tot_amt)  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, SUM(PREM_ADJ_PARMET_DTL.tot_amt)  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*SUM(PREM_ADJ_PARMET_DTL.tot_amt)  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3), '') ,    -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2), '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN CONVERT(char(20),dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'R'))              
   ELSE CONVERT(char(20),dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'R')+'C')
   END,-- ZZVTREF4(88) 
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
   PREM_ADJ_PERD.prem_adj_perd_id,  
   PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,  
   POST_TRNS_TYP.post_trns_typ_id,  
   'CHF',0,@Ind,@rel_prem_adj_id  
  FROM PREM_ADJ INNER JOIN   
   PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID   
   AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN   
   PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1 AND PREM_ADJ_PGM.ZNA_SERV_COMP_CLM_HNDL_FEE_IND=1) INNER JOIN  
   PREM_ADJ_PGM_SETUP ON (PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_ID AND  
   PREM_ADJ_PGM_SETUP.incld_ernd_retro_prem_ind=0) INNER JOIN   
   CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN  
   LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id   
   AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.chf_ind=1) INNER JOIN   
   POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN   
   PREM_ADJ_PARMET_SETUP ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID   
   AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID   
   AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_ID = PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID   
   AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id   
   AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id) 
   INNER JOIN  
   PREM_ADJ_PARMET_DTL ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PARMET_SETUP_ID=PREM_ADJ_PARMET_DTL.prem_adj_parmet_setup_id  
   AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ_PARMET_DTL.PREM_ADJ_ID AND PREM_ADJ_PARMET_DTL.CLM_HNDL_FEE_LOS_TYP_ID IN(507,775,776)) INNER JOIN  
   COML_AGMT ON (PREM_ADJ_PARMET_DTL.COML_AGMT_ID=COML_AGMT.COML_AGMT_ID   
   AND COML_AGMT.ADJ_TYP_ID NOT IN(62,68,448))  
  WHERE PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=398  
   AND POST_TRNS_TYP.POST_TRNS_TYP_ID=55  
   AND POST_TRNS_TYP.actv_ind=1  
   AND PREM_ADJ_PARMET_SETUP.tot_amt <> 0  
   AND PREM_ADJ.prem_adj_id = @prem_adj_id  
  GROUP BY  
   PREM_ADJ.valn_dt,PREM_ADJ_PGM.strt_dt,PREM_ADJ_PGM.plan_end_dt,  
   POST_TRNS_TYP.main_nbr_txt,POST_TRNS_TYP.sub_nbr_txt,PREM_ADJ.invc_due_dt,  
   PREM_ADJ.fnl_invc_dt,PREM_ADJ.fnl_invc_nbr_txt,POST_TRNS_TYP.comp_txt,  
   LSI_CUSTMR.lsi_acct_id,CUSTMR.finc_pty_id,  
   PREM_ADJ_PERD.prem_adj_perd_id,PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,POST_TRNS_TYP.post_trns_typ_id,  
   POST_TRNS_TYP.trns_nm_txt,PREM_ADJ_PGM.prem_adj_pgm_id,PREM_ADJ.PREM_ADJ_ID,  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END,  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,  
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN CONVERT(char(20),dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'R'))              
   ELSE CONVERT(char(20),dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'R')+'C')
   END
  
  
  --ZSC Claim Service fee 8000/5400 for ILRF and LOSS FUND Policies  
  --ZSC Indicator at Program Period Level is TRUE and Excluding the Admin Fee Categories(508,774,775)  
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,  
   pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,  
   sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt, bewar_txt,optxt_txt,  
   zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,  
   zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,  
   prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,  
   Post_cd,rel_prem_adj_id)  
  SELECT   
   CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1) 
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   --(SR 321581)  
   CONVERT(char(20), ISNULL(RTRIM(CUSTMR.finc_pty_id),'')+'D')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,      -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,      -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,    -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, SUM(PREM_ADJ_PARMET_SETUP.tot_amt)  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, SUM(PREM_ADJ_PARMET_SETUP.tot_amt)  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*SUM(PREM_ADJ_PARMET_SETUP.tot_amt)  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3), '') ,    -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2), '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN CONVERT(char(20),dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'D'))              
   ELSE CONVERT(char(20),dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'D')+'C')
   END,-- ZZVTREF4(88)
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
   PREM_ADJ_PERD.prem_adj_perd_id,  
   PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,  
   POST_TRNS_TYP.post_trns_typ_id,  
   'CHF',0,@Ind,@rel_prem_adj_id  
  FROM PREM_ADJ INNER JOIN   
   PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID   
   AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN   
   PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1 AND PREM_ADJ_PGM.ZNA_SERV_COMP_CLM_HNDL_FEE_IND=1) INNER JOIN  
   PREM_ADJ_PGM_SETUP ON (PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_ID AND  
   PREM_ADJ_PGM_SETUP.incld_ernd_retro_prem_ind=0) INNER JOIN   
   CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN  
   LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id   
   AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.chf_ind=1) INNER JOIN   
   POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN   
   PREM_ADJ_PARMET_SETUP ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID   
   AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID   
   AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_ID = PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID   
   AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id   
   AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id) 
   --INNER JOIN  
   --PREM_ADJ_PARMET_DTL ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PARMET_SETUP_ID=PREM_ADJ_PARMET_DTL.prem_adj_parmet_setup_id  
   --AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ_PARMET_DTL.PREM_ADJ_ID AND PREM_ADJ_PARMET_DTL.CLM_HNDL_FEE_LOS_TYP_ID NOT IN(507,775,776)) INNER JOIN  
   --COML_AGMT ON (PREM_ADJ_PARMET_DTL.COML_AGMT_ID=COML_AGMT.COML_AGMT_ID   
   --AND COML_AGMT.ADJ_TYP_ID IN(62,68,448))  
  WHERE PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=398  
   AND POST_TRNS_TYP.POST_TRNS_TYP_ID=38  
   AND POST_TRNS_TYP.actv_ind=1  
   AND PREM_ADJ_PARMET_SETUP.tot_amt <> 0  
   AND PREM_ADJ.prem_adj_id = @prem_adj_id
   AND [dbo].[fn_ILR_Ded_Policy](@prem_adj_id,PREM_ADJ_PERD.PREM_ADJ_PERD_ID)=1   
  GROUP BY  
   PREM_ADJ.valn_dt,PREM_ADJ_PGM.strt_dt,PREM_ADJ_PGM.plan_end_dt,  
   POST_TRNS_TYP.main_nbr_txt,POST_TRNS_TYP.sub_nbr_txt,PREM_ADJ.invc_due_dt,  
   PREM_ADJ.fnl_invc_dt,PREM_ADJ.fnl_invc_nbr_txt,POST_TRNS_TYP.comp_txt,  
   LSI_CUSTMR.lsi_acct_id,CUSTMR.finc_pty_id,  
   PREM_ADJ_PERD.prem_adj_perd_id,PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,POST_TRNS_TYP.post_trns_typ_id,  
   POST_TRNS_TYP.trns_nm_txt,PREM_ADJ_PGM.prem_adj_pgm_id,PREM_ADJ.PREM_ADJ_ID,  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END,  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN CONVERT(char(20),dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'D'))              
   ELSE CONVERT(char(20),dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'D')+'C')
   END  
  
  
  --ZSC Administration Fee  8000/5402 for CHF and LOSS FUND Policies  
  --ZSC Indicator at Program Period Level is TRUE and  the CHF on Admin Fee Categories(508,774,775)  
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,  
   pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,  
   sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt, bewar_txt,optxt_txt,  
   zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,  
   zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,  
   prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,  
   Post_cd,rel_prem_adj_id)  
  SELECT   
   CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),''))  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   --(SR 321581)  
   CONVERT(char(20), ISNULL(RTRIM(CUSTMR.finc_pty_id),'')+'D')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,      -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,      -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,    -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, SUM(PREM_ADJ_PARMET_DTL.tot_amt)  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, SUM(PREM_ADJ_PARMET_DTL.tot_amt)  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*SUM(PREM_ADJ_PARMET_DTL.tot_amt)  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3), '') ,    -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2), '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN CONVERT(char(20),dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'D'))              
   ELSE CONVERT(char(20),dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'D')+'C')
   END,-- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
   PREM_ADJ_PERD.prem_adj_perd_id,  
   PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,  
   POST_TRNS_TYP.post_trns_typ_id,  
   'CHF',0,@Ind,@rel_prem_adj_id  
  FROM PREM_ADJ INNER JOIN   
   PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID   
   AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN   
   PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1 AND PREM_ADJ_PGM.ZNA_SERV_COMP_CLM_HNDL_FEE_IND=1) INNER JOIN  
   PREM_ADJ_PGM_SETUP ON (PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_ID AND  
   PREM_ADJ_PGM_SETUP.incld_ernd_retro_prem_ind=0) INNER JOIN   
   CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN  
   LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id   
   AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.chf_ind=1) INNER JOIN   
   POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN   
   PREM_ADJ_PARMET_SETUP ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID   
   AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID   
   AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_PGM_SETUP_ID = PREM_ADJ_PGM_SETUP.PREM_ADJ_PGM_SETUP_ID   
   AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id   
   AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id) INNER JOIN  
   PREM_ADJ_PARMET_DTL ON (PREM_ADJ_PARMET_SETUP.PREM_ADJ_PARMET_SETUP_ID=PREM_ADJ_PARMET_DTL.prem_adj_parmet_setup_id  
   AND PREM_ADJ_PARMET_SETUP.PREM_ADJ_ID = PREM_ADJ_PARMET_DTL.PREM_ADJ_ID AND PREM_ADJ_PARMET_DTL.CLM_HNDL_FEE_LOS_TYP_ID IN(507,775,776)) INNER JOIN  
   COML_AGMT ON (PREM_ADJ_PARMET_DTL.COML_AGMT_ID=COML_AGMT.COML_AGMT_ID   
   AND COML_AGMT.ADJ_TYP_ID IN(62,68,448))  
  WHERE PREM_ADJ_PARMET_SETUP.adj_parmet_typ_id=398  
   AND POST_TRNS_TYP.POST_TRNS_TYP_ID=55  
   AND POST_TRNS_TYP.actv_ind=1  
   AND PREM_ADJ_PARMET_SETUP.tot_amt <> 0  
   AND PREM_ADJ.prem_adj_id = @prem_adj_id  
  GROUP BY  
   PREM_ADJ.valn_dt,PREM_ADJ_PGM.strt_dt,PREM_ADJ_PGM.plan_end_dt,  
   POST_TRNS_TYP.main_nbr_txt,POST_TRNS_TYP.sub_nbr_txt,PREM_ADJ.invc_due_dt,  
   PREM_ADJ.fnl_invc_dt,PREM_ADJ.fnl_invc_nbr_txt,POST_TRNS_TYP.comp_txt,  
   LSI_CUSTMR.lsi_acct_id,CUSTMR.finc_pty_id,  
   PREM_ADJ_PERD.prem_adj_perd_id,PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,POST_TRNS_TYP.post_trns_typ_id,  
   POST_TRNS_TYP.trns_nm_txt,PREM_ADJ_PGM.prem_adj_pgm_id,PREM_ADJ.PREM_ADJ_ID,  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END,  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN CONVERT(char(20),dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'D'))              
   ELSE CONVERT(char(20),dbo.fn_GetPolicyForNonERPAriesCHF(PREM_ADJ.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID,398,'D')+'C')
   END  
  
  
   --Deductible Tax Postings Based on the tax calculations   
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,  
   pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,  
   sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt, bewar_txt,optxt_txt,  
   zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,  
   zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,  
   prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
   CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),'')) ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   CONVERT(char(20), MAX(ISNULL(RTRIM(CUSTMR.finc_pty_id),''))+'R')  ,      -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, SUM(PREM_ADJ_LOS_REIM_FUND_POST_TAX.post_amt)  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, SUM(PREM_ADJ_LOS_REIM_FUND_POST_TAX.post_amt)  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*SUM(PREM_ADJ_LOS_REIM_FUND_POST_TAX.post_amt)  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), 3) ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
 CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16),''),    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), CASE WHEN (LSI_CUSTMR.lsi_acct_id IS NULL) THEN CONVERT(char(50), '')   
   ELSE 'LSI ' + CONVERT(varchar(46), LSI_CUSTMR.lsi_acct_id) END),   -- OPTXT(69)  
   CONVERT(char(3), '') ,    -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), '') ,   -- ZZPOLICY(71)  
   CONVERT(char(2), '') ,    -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   --(SR 318680)  
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN CONVERT(char(20),dbo.fn_GetPolicyForTexasTaxAries(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID))              
   ELSE CONVERT(char(20),dbo.fn_GetPolicyForTexasTaxAries(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID)+'C')
   END,-- ZZVTREF4(88)
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
   PREM_ADJ_PERD.prem_adj_perd_id,  
   PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,  
   POST_TRNS_TYP.post_trns_typ_id,  
   'STATE SALES & SERVICE TAX',0,@Ind,@rel_prem_adj_id  
  FROM PREM_ADJ INNER JOIN PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID   
   AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN   
   PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1  
   AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID=PREM_ADJ_PERD.PREM_ADJ_PGM_ID) INNER JOIN  
   CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) LEFT JOIN   
   LSI_CUSTMR ON (CUSTMR.custmr_id = LSI_CUSTMR.custmr_id AND LSI_CUSTMR.actv_ind=1 AND LSI_CUSTMR.prim_ind=1)   
   INNER JOIN PREM_ADJ_LOS_REIM_FUND_POST_TAX  ON (PREM_ADJ_LOS_REIM_FUND_POST_TAX.PREM_ADJ_PERD_ID =   
   PREM_ADJ_PERD.PREM_ADJ_PERD_ID AND PREM_ADJ_LOS_REIM_FUND_POST_TAX.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID )  
   INNER JOIN POST_TRNS_TYP ON (POST_TRNS_TYP.POST_TRNS_TYP_ID=PREM_ADJ_LOS_REIM_FUND_POST_TAX.POST_TRNS_TYP_ID)  
  WHERE   
   POST_TRNS_TYP.actv_ind=1  
   AND PREM_ADJ.prem_adj_id = @prem_adj_id  
  GROUP BY  
   PREM_ADJ_LOS_REIM_FUND_POST_TAX.st_id,PREM_ADJ.valn_dt,PREM_ADJ_PGM.strt_dt,PREM_ADJ_PGM.plan_end_dt,  
   POST_TRNS_TYP.main_nbr_txt,POST_TRNS_TYP.sub_nbr_txt,PREM_ADJ.invc_due_dt,  
   PREM_ADJ.fnl_invc_dt,PREM_ADJ.fnl_invc_nbr_txt,POST_TRNS_TYP.comp_txt,  
   LSI_CUSTMR.lsi_acct_id,CUSTMR.finc_pty_id,  
   PREM_ADJ_PERD.prem_adj_perd_id,PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,POST_TRNS_TYP.post_trns_typ_id,  
   POST_TRNS_TYP.trns_nm_txt,PREM_ADJ_PGM.prem_adj_pgm_id,  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END,  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN CONVERT(char(20),dbo.fn_GetPolicyForTexasTaxAries(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID))              
   ELSE CONVERT(char(20),dbo.fn_GetPolicyForTexasTaxAries(PREM_ADJ_PERD.PREM_ADJ_ID,PREM_ADJ_PERD.PREM_ADJ_PERD_ID,PREM_ADJ_PGM.PREM_ADJ_PGM_ID)+'C')
   END  
  HAVING SUM(PREM_ADJ_LOS_REIM_FUND_POST_TAX.post_amt) <> 0  
     
     
   /***************************************************************************************  
   Surcharges and Assesments: Added below Postings  
   as part of the AIS 21 surcharges project  
   ************Note:When ever the below postings code is modifede   
   we need to modify the same in ModAISCalcSurchargeReview.sql stored procedure  
   ****************************************************************************************/  
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,  
   pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,  
   sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt, bewar_txt,optxt_txt,  
   zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,  
   zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,  
   prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10),ISNULL(RTRIM(CUSTMR.finc_pty_id),'')) ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   --(SR 321581)  
   CONVERT(char(20), ISNULL(RTRIM(CUSTMR.finc_pty_id),'')+'R')  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_SURCHRG_DTL.tot_addn_rtn  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_SURCHRG_DTL.tot_addn_rtn  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   CASE WHEN (@Reverse*PREM_ADJ_SURCHRG_DTL.tot_addn_rtn  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16), '') ,    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), '') ,   -- OPTXT(69)  
   CONVERT(char(3), RTRIM(COML_AGMT.pol_sym_txt)) ,  -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), RTRIM(COML_AGMT.pol_nbr_txt)) ,  -- ZZPOLICY(71)  
   CONVERT(char(2), COML_AGMT.pol_modulus_txt),  -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt              
   ELSE RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt+'C'
   END
   )  ,   -- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
   PREM_ADJ_PERD.prem_adj_perd_id,  
   PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,  
   POST_TRNS_TYP.post_trns_typ_id,  
   'RETRO SURCH & ASSMT',0,@Ind,@rel_prem_adj_id  
  FROM PREM_ADJ INNER JOIN   
   PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID   
   AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN   
   PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN  
   COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID   
   AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = COML_AGMT.PREM_ADJ_PGM_id) INNER JOIN   
   CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) INNER JOIN  
   PREM_ADJ_SURCHRG_DTL ON (PREM_ADJ_SURCHRG_DTL.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID   
   AND PREM_ADJ_SURCHRG_DTL.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id   
   AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id   
   AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_SURCHRG_DTL.COML_AGMT_ID)   
   INNER JOIN POST_TRNS_TYP ON (POST_TRNS_TYP.POST_TRNS_TYP_ID=PREM_ADJ_SURCHRG_DTL.POST_TRNS_TYP_ID)  
  WHERE   
   POST_TRNS_TYP.actv_ind=1  
   AND COML_AGMT.adj_typ_id IN (63,65,66,67,70,71,72,73)   
   AND PREM_ADJ_SURCHRG_DTL.tot_addn_rtn <> 0  
   AND PREM_ADJ.prem_adj_id = @prem_adj_id  
  
  
   --Surcharges--Reclass  
   /**********************************************************************************************  
   Surcharges and Assesments: Added below Postings  
   as part of the AIS 21 surcharges project  
   ************Note:When ever the below postings code is modifeded  
   we need to modify the same in ModAISCalcSurchargeReview.sql stored procedure  
   ***********************************************************************************************/  
  INSERT INTO ARIES_TRNSMTL_HIST (zzrectyp_txt,aktyp_txt,gpart_txt,gpart_ext_txt,vtref_txt,posnr_txt,psngl_txt,  
   pmtfr_txt,pmtto_txt,rskfr_txt,rskto_txt,pmend_txt,pmend_time_txt,renew_txt,rnewx_txt,ccd_txt,opccd_txt,  
   gsber_txt,opgsber_txt,prgrp_txt,vsarl_vx_txt,amt_tot_txt,amt_inst_txt,atfrd_txt,atfrd_time_txt,  
   endtyp_txt,amt_ned_txt, amt_end_txt,curr_txt,hvorg_txt,tvorg_txt,ophvorg_txt,optvorg_txt,blart_txt,  
   vtre2_txt,vtre3_txt,vgpart2_txt,vgpart3_txt,gsfnr_txt,belnr_txt,bltyp_txt,emgpa_txt,embvt_txt,  
   emadr_txt,pymet_txt,pybuk_txt,faedn_txt,budat_txt,bldat_txt,dun_rsn_txt,dun_rsn_dd_txt,pay_rsn_txt,  
   pay_rsn_dd_txt,clr_rsn_txt,clr_rsn_dd_txt,kostl_txt,prctr_txt,pygrp_txt,grkey_txt,endrev_txt,  
   sto_from_txt,sto_to_txt,orgin_txt,chklvl_txt,vbund_txt,refgf_txt,refbl_txt, bewar_txt,optxt_txt,  
   zzpolsym_txt,zzpol_txt,zzmod_txt,zzaiserr_txt,zzinvgrp_txt,zzpclnknr_txt,yylob_cd_txt,  
   yyamy_cd_txt,zzst_cd_txt,zzlbv_cd_txt,zzztm_cd_txt,zzreins_cd_txt,zzlit_cd_txt,zzwwzdc_txt,zzcnvt_itm_txt,  
   zzclring_ind_txt,zzextref_txt,zzgpart4_txt,zzvtref4_txt,zzrevisonind_txt,zzcbs_ind_txt,  
   zzccd_txt,zzamt_comm_txt,zzeor_txt,updt_user_id,updt_dt,crte_user_id,crte_dt,actv_ind,  
   prem_adj_perd_id,prem_adj_id,custmr_id,post_trns_typ_id,adj_typ_txt,trnsmtl_sent_ind,Post_cd,rel_prem_adj_id)  
  SELECT   
      CONVERT(char(1), '2')  ,   -- ZZRECTYPE (1)  
   CONVERT(char(2), '01') ,   -- AKTYP(2)  
   CONVERT(char(10), '')  ,   -- GPART(3)  
   CONVERT(char(20), '')  ,   -- GPART_EXT(4)  
   CASE WHEN CUSTMR.company_cd=733 THEN  CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt)   
   ELSE CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt+'C') END  ,    -- VTREF(5)  
   CONVERT(char(12), '')  ,   -- POSNR(6)  
   CONVERT(char(1), 'X')  ,   -- PSNGL(7)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTFR(8)  
   ISNULL(CONVERT(char(8), PREM_ADJ.valn_dt, 112), '') ,   -- PMTTO(9)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.strt_dt, 112), '')     ,   -- RISKFR(10)  
   ISNULL(CONVERT(char(8), PREM_ADJ_PGM.plan_end_dt, 112), '') ,   -- RISKTO(11)  
   CONVERT(char(8), '') ,    -- PMEND(12)  
   CONVERT(char(6), '') ,    -- PMEND_TIME(13)  
   CONVERT(char(1), '') ,    -- RENEW(14)  
   CONVERT(char(3), '') ,    -- RNEWX(15)  
  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END ,  -- CCODE(16)  
   CONVERT(char(4), '') ,    -- OPCCODE(17)  
   CONVERT(char(4), '') ,    -- GSBER(18)  
   CONVERT(char(4), '') ,    -- OPGSBER(19)  
   CONVERT(char(2), '') ,    -- PRGRP(20)  
   CONVERT(char(6), '') ,    -- VSARL_VX(21)  
  
   CONVERT(char(15), REPLACE(REPLACE(ISNULL(SPACE(15 - LEN(CONVERT(varchar(15),   
   CONVERT(bigint, PREM_ADJ_SURCHRG_DTL.tot_addn_rtn  * 100)))) +  
   CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_SURCHRG_DTL.tot_addn_rtn  * 100)),   
   '000000000000000'), ' ', '0'), '-', '0')) +   
   --Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"  
   CASE WHEN (@Reverse*PREM_ADJ_SURCHRG_DTL.tot_addn_rtn  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)  
  
   CONVERT(char(16), '') ,   -- AMOUNT_INST(23)  
   CONVERT(char(8), '') ,   -- ATFRD(24)  
   CONVERT(char(6), '') ,    -- ATFRD_TIME(25)  
   CONVERT(char(1), '') ,    -- ENDTYPE(26)  
   CONVERT(char(16), '') ,   -- AMOUNT_NEED(27)  
   CONVERT(char(16), '') ,   -- AMOUNT_END(28)  
   CASE WHEN CUSTMR.currency_cd=735 THEN CONVERT(char(5), 'USD') ELSE CONVERT(char(5), 'CAD') END,   -- CURR(29)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- HVORG(30)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- TVORG(31)  
   CONVERT(char(4), POST_TRNS_TYP.main_nbr_txt) ,    -- OPHVORG(32)  
   CONVERT(char(4), POST_TRNS_TYP.sub_nbr_txt) ,    -- OPTVORG(33)  
   CONVERT(char(2), 'PP') ,   -- BLART(34)  
   CONVERT(char(20), '') ,   -- VTRE2(35)  
   CONVERT(char(20), '') ,   -- VTRE3(36)  
   CONVERT(char(10), '') ,   -- VGPART2(37)  
   CONVERT(char(10), '') ,   -- VGPART3(38)  
   CONVERT(char(16), '') ,   -- GSFNR(39)  
   CONVERT(char(6), '') ,    -- BELNR(40)  
   CONVERT(char(2), '20') ,   -- BLTYP(41)  
   CONVERT(char(10), '') ,   -- EMGPA(42)  
   CONVERT(char(4), '') ,    -- EMBVT(43)  
   CONVERT(char(10), '') ,   -- EMADR(44)  
   CONVERT(char(1), '') ,   -- PYMET(45)  
   CONVERT(char(4), '') ,    -- PYBUK(46)  
   ISNULL(CONVERT(char(8), PREM_ADJ.invc_due_dt, 112), '') ,   -- FAEDN(47)  
   CONVERT(char(8), '') ,    -- BUDAT(48)  
   ISNULL(CONVERT(char(8), PREM_ADJ.fnl_invc_dt, 112), '') ,   -- BLDAT(49)  
   CONVERT(char(1), '') ,    -- DUN_REASON(50)  
   CONVERT(char(3), '') ,    -- DUN_REASON_DAYS(51)  
   CONVERT(char(1), '') ,    -- PAY_REASON(52)  
   CONVERT(char(3), '') ,    -- PAY_REASON_DAYS(53)  
   CONVERT(char(1), '') ,    -- CLR_REASON(54)  
   CONVERT(char(3), '') ,    -- CLR_REASON_DAYS(55)  
   CONVERT(char(10), '') ,   -- KOSTL(56)  
   CONVERT(char(10), '') ,   -- PRCTR(57)  
   CONVERT(char(10), '') ,   -- PYGRP(58)  
   CONVERT(char(3), '') ,    -- GRKEY(59)  
   CONVERT(char(1), '') ,    -- ENDREV(60)  
   CONVERT(char(8), '') ,    -- STO_FROM(61)  
   CONVERT(char(8), '') ,    -- STO_TO(62)  
   CONVERT(char(30), 'RT') ,   -- ORIGIN(63)  
   CONVERT(char(1), '') ,    -- CHECKLEVEL(64)  
   CONVERT(char(6), '') ,    -- VBUND(65)  
   CONVERT(char(16), '') ,    -- REFGF(66)  
   CONVERT(char(6), '') ,    -- REFBL(67)  
   CONVERT(char(3), '1') ,   -- BEWAR(68)  
   CONVERT(char(50), '') ,   -- OPTXT(69)  
   CONVERT(char(3), RTRIM(COML_AGMT.pol_sym_txt)) ,  -- ZZPOLSYMBOL(70)  
   CONVERT(char(18), RTRIM(COML_AGMT.pol_nbr_txt)) ,  -- ZZPOLICY(71)  
   CONVERT(char(2), COML_AGMT.pol_modulus_txt),  -- ZZMOD(72)  
   CONVERT(char(1), '') ,    -- ZZLSIERR(73)  
   CONVERT(char(10), '') ,   -- ZZINVGRP(74)  
   CONVERT(char(12), '') ,   -- ZZPCLINKNR(75)  
   CONVERT(char(5), '') ,    -- YYLOB_CD(76)  
   CONVERT(char(4), '') ,    -- YYAMY_CD(77)  
   CONVERT(char(5), '') ,    -- ZZSTATE_CD(78)  
   CONVERT(char(6), '') ,    -- ZZLBV_CD(79)  
   CONVERT(char(5), '') ,    -- ZZZTM_CD(80)  
   CONVERT(char(10), '') ,   -- ZZREINS_CD(81)  
   CONVERT(char(5), '') ,    -- ZZLIT_CD(82)  
   CONVERT(char(5), '') ,    -- ZZWWZDC(83)  
   CONVERT(char(1), '') ,    -- ZZCONVERTED_ITEM(84)  
   CONVERT(char(2), '') ,    -- ZZCLEARING_IND(85)  
   ISNULL(CONVERT(char(20), PREM_ADJ.fnl_invc_nbr_txt),'') ,   -- ZZEXTREF(86)  
   CONVERT(char(10), '') ,   -- ZZGPART4(87)  
   CONVERT(char(20), 
   CASE 
   WHEN CUSTMR.currency_cd=735 
   THEN RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt              
   ELSE RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt+'C'
   END
   )  ,   -- ZZVTREF4(88)  
   CONVERT(char(1), '') ,    -- ZZREVISONIND(89)  
   CONVERT(char(1), '') ,    -- ZZCBS_IND(90)  
   CASE WHEN CUSTMR.company_cd=733 THEN CONVERT(char(4), POST_TRNS_TYP.comp_txt) ELSE CONVERT(char(4), 'ZC2') END, -- ZZCCODE(91)  
   CONVERT(char(16), '') ,   -- ZZAMOUNT_COMM(92)  
   CONVERT(char(3), 'EOR'),    -- ZZEOR(93)  
   CONVERT(char(1), 1   ),  -- updt_user_id  
   getdate(),  -- updt_dt  
   CONVERT(char(1), 1),   -- crte_user_id  
   getdate(),  -- crte_dt  
   CONVERT(char(1), 1)  ,   -- actv_ind  
   PREM_ADJ_PERD.prem_adj_perd_id,  
   PREM_ADJ_PERD.prem_adj_id,  
   PREM_ADJ_PERD.custmr_id,  
   POST_TRNS_TYP.post_trns_typ_id,  
   'RETRO SURCH & ASSMT',0,@Ind,@rel_prem_adj_id  
  FROM PREM_ADJ INNER JOIN   
   PREM_ADJ_PERD ON (PREM_ADJ_PERD.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID   
   AND PREM_ADJ.reg_custmr_id = PREM_ADJ_PERD.reg_custmr_id) INNER JOIN   
   PREM_ADJ_PGM ON (PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id AND PREM_ADJ_PGM.ACTV_IND = 1) INNER JOIN  
   COML_AGMT ON (PREM_ADJ_PERD.custmr_id = COML_AGMT.CUSTMR_ID   
   AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = COML_AGMT.PREM_ADJ_PGM_id) INNER JOIN   
   CUSTMR ON (PREM_ADJ.reg_custmr_id = CUSTMR.CUSTMR_ID) INNER JOIN  
   POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN   
   PREM_ADJ_SURCHRG_DTL ON (PREM_ADJ_SURCHRG_DTL.PREM_ADJ_PERD_ID = PREM_ADJ_PERD.PREM_ADJ_PERD_ID   
   AND PREM_ADJ_SURCHRG_DTL.PREM_ADJ_ID = PREM_ADJ.PREM_ADJ_ID AND PREM_ADJ_PGM.CUSTMR_ID = PREM_ADJ_PERD.custmr_id   
   AND PREM_ADJ_PGM.PREM_ADJ_PGM_ID = PREM_ADJ_PERD.PREM_ADJ_PGM_id   
   AND COML_AGMT.COML_AGMT_ID = PREM_ADJ_SURCHRG_DTL.COML_AGMT_ID)   
  WHERE POST_TRNS_TYP.POST_TRNS_TYP_ID=72  
   AND POST_TRNS_TYP.actv_ind=1  
   AND COML_AGMT.adj_typ_id IN (63,65,66,67,70,71,72,73)   
   AND PREM_ADJ_SURCHRG_DTL.tot_addn_rtn <> 0  
   AND PREM_ADJ.prem_adj_id = @prem_adj_id  
  
     
     
  
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
 
GO
if object_id('ModAIS_TransmittalToARiES') is not null
        print 'Created Procedure ModAIS_TransmittalToARiES'
else
        print 'Failed Creating Procedure ModAIS_TransmittalToARiES'
go

if object_id('ModAIS_TransmittalToARiES') is not null
        grant exec on ModAIS_TransmittalToARiES to  public
go