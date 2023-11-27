
IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'ModAIS_TPATransmittalToARiES' and TYPE = 'P')
	DROP PROC ModAIS_TPATransmittalToARiES
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		ModAIS_TPATransmittalToARiES
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		Anil Nandakumar 02/09/2009
-----
-----	Description:	This procedure will populate the ARIES_TRNSMTL_HIST table with all the 
-----					TPA Manual transmittals for a given adjustment
---------------------------------------------------------------------

--Pass revision flag and adjustment ID. Revision flag will indicate if the amounts need to be reversed for a particular 
-- adjustment passed to it.

--1. To be called on finalization of TPA/Manual invoice. 
--2. Call on revision of TPA/Manual invoice
--3. Call on void of TPA/Manual invoice
--4. Call on cancel of TPA/Manual invoice

CREATE PROCEDURE [dbo].[ModAIS_TPATransmittalToARiES] 
@thrd_pty_admin_mnl_invc_id int,
@custmr_id int,
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
				SET @Reverse = 1
		  ELSE IF (@Ind=2)
				SET @Reverse = -1
		  ELSE IF (@Ind=3)
				SET @Reverse = -1
		  ELSE IF (@Ind=4)
			BEGIN				
				--To cancel and clear postings for TPA/Manual
				DELETE  FROM ARIES_TRNSMTL_HIST WITH (ROWLOCK) WHERE thrd_pty_admin_mnl_invc_id=@thrd_pty_admin_mnl_invc_id 
				goto CancelFlg
			END

---- TPA MANUAL POSTINGS
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
	custmr_id,post_trns_typ_id,	adj_typ_txt,trnsmtl_sent_ind,thrd_pty_admin_mnl_invc_id)
SELECT	
    CONVERT(char(1), '2')  , 		-- ZZRECTYPE (1)
	CONVERT(char(2), '01') , 		-- AKTYP(2)
	CONVERT(char(10), CUSTMR.finc_pty_id)  , 		-- GPART(3)
	CONVERT(char(20), '')  , 		-- GPART_EXT(4)
	CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind=1 AND POST_TRNS_TYP.pol_reqr_ind=1) THEN
		CONVERT(char(20), RTRIM(pol_sym_txt)+RTRIM(pol_nbr_txt)+pol_modulus_txt) 						   				
	ELSE
		CONVERT(char(20), '')  	
	END,							-- VTREF(5)
	CONVERT(char(12), '')  , 		-- POSNR(6)
	CONVERT(char(1), 'X')  , 		-- PSNGL(7)
	ISNULL(CONVERT(char(8), THRD_PTY_ADMIN_MNL_INVC.valn_dt, 112),'') , 		-- PMTFR(8)
	ISNULL(CONVERT(char(8), THRD_PTY_ADMIN_MNL_INVC.valn_dt, 112),''),  		-- PMTTO(9)
	ISNULL(CONVERT(char(8), THRD_PTY_ADMIN_MNL_INVC_DTL.eff_dt, 112),''),  		-- RISKFR(10)
	ISNULL(CONVERT(char(8), THRD_PTY_ADMIN_MNL_INVC_DTL.expi_dt, 112),''), 		-- RISKTO(11)
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
	CONVERT(bigint, THRD_PTY_ADMIN_MNL_INVC_DTL.thrd_pty_admin_amt  * 100)))) +
	CONVERT(varchar(15), CONVERT(bigint, THRD_PTY_ADMIN_MNL_INVC_DTL.thrd_pty_admin_amt  * 100)), 
	'000000000000000'), ' ', '0'), '-', '0')) + 
	CASE WHEN (@Reverse*THRD_PTY_ADMIN_MNL_INVC_DTL.thrd_pty_admin_amt  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

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
	ISNULL(CONVERT(char(8), ISNULL(THRD_PTY_ADMIN_MNL_INVC.due_dt,DATEADD(day,20,getdate())), 112), '') , -- FAEDN(47)
	CONVERT(char(8), '') , 			-- BUDAT(48)
	ISNULL(CONVERT(char(8), THRD_PTY_ADMIN_MNL_INVC.invc_dt, 112), '') , 		-- BLDAT(49)
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
	CONVERT(char(50), SUBSTRING(ISNULL(CUSTMR.full_nm,'')+ISNULL(INT_ORG.full_name,''),1,50)) ,			-- OPTXT(69)
	CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN CONVERT(char(3), '') 	
		ELSE CONVERT(char(3), RTRIM(THRD_PTY_ADMIN_MNL_INVC_DTL.pol_sym_txt)) END,					-- ZZPOLSYMBOL(70)
	CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN CONVERT(char(3), '') 	
		ELSE CONVERT(char(18), RTRIM(THRD_PTY_ADMIN_MNL_INVC_DTL.pol_nbr_txt)) END,					-- ZZPOLICY(71)
	CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN CONVERT(char(3), '') 	
		ELSE CONVERT(char(2), THRD_PTY_ADMIN_MNL_INVC_DTL.pol_modulus_txt) END,			-- ZZMOD(72)
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
	ISNULL(CONVERT(char(20), THRD_PTY_ADMIN_MNL_INVC.invc_nbr_txt),'') , 		-- ZZEXTREF(86)
	CONVERT(char(10), '') , 		-- ZZGPART4(87)

--	CASE WHEN (POST_TRNS_TYP.thrd_pty_admin_mnl_ind<>1 AND POST_TRNS_TYP.pol_reqr_ind<>1) THEN 
--		CONVERT(char(20), '') 
--	ELSE 
--		CONVERT(char(20), isnull(RTRIM(THRD_PTY_ADMIN_MNL_INVC_DTL.pol_sym_txt),'')+
--		ISNULL(RTRIM(THRD_PTY_ADMIN_MNL_INVC_DTL.pol_nbr_txt),'')+ISNULL(THRD_PTY_ADMIN_MNL_INVC_DTL.pol_modulus_txt,''))
--	END,  		-- ZZVTREF4(88)

	--CHANGED AFTER DISCUSSION WITH CRAIG. 3PM 02/25/2009
	-- ZZVTREF4(88) TO BE POPULATED IRRESPECTIVE OF WHETHER POLICY IO/POLICY REQD INDICATORS ARE SET.
	--CONVERT(char(20), isnull(RTRIM(THRD_PTY_ADMIN_MNL_INVC_DTL.pol_sym_txt),'')+
	--ISNULL(RTRIM(THRD_PTY_ADMIN_MNL_INVC_DTL.pol_nbr_txt),'')+
	--ISNULL(THRD_PTY_ADMIN_MNL_INVC_DTL.pol_modulus_txt,'')), -- ZZVTREF4(88)
	--(SR 318680)
	CONVERT(char(20),dbo.fn_GetPolicyForTPAAriesPostings(THRD_PTY_ADMIN_MNL_INVC.THRD_PTY_ADMIN_MNL_INVC_ID,THRD_PTY_ADMIN_MNL_INVC_DTL.THRD_PTY_ADMIN_MNL_INVC_DTL_ID)),-- ZZVTREF4(88)

	CONVERT(char(1), '') , 			-- ZZREVISONIND(89)
	CONVERT(char(1), '') , 			-- ZZCBS_IND(90)
	CONVERT(char(4), POST_TRNS_TYP.comp_txt),	-- ZZCCODE(91)
	CONVERT(char(16), '') ,			-- ZZAMOUNT_COMM(92)
	CONVERT(char(3), 'EOR'),  		-- ZZEOR(93)
	CONVERT(char(1), 1	  ),		-- updt_user_id
	getdate(),  					-- updt_dt
	CONVERT(char(1), 1),			-- crte_user_id
	getdate(),						-- crte_dt
	CONVERT(char(1), 1)  ,			-- actv_ind
	NULL, --NOT USED FOR TPA MANUAL - PREM_ADJ_PERD.prem_adj_perd_id, 
	NULL, --NOT USED FOR TPA MANUAL - PREM_ADJ_PERD.prem_adj_id,
	CUSTMR.custmr_id,
	POST_TRNS_TYP.post_trns_typ_id,
	POST_TRNS_TYP.trns_nm_txt,0,
	@thrd_pty_admin_mnl_invc_id
FROM CUSTMR INNER JOIN
	POST_TRNS_TYP ON (POST_TRNS_TYP.post_trns_typ_id = POST_TRNS_TYP.post_trns_typ_id) INNER JOIN
	THRD_PTY_ADMIN_MNL_INVC ON (THRD_PTY_ADMIN_MNL_INVC.custmr_id = CUSTMR.custmr_id 
	AND THRD_PTY_ADMIN_MNL_INVC.FNL_IND=1) INNER JOIN 
	THRD_PTY_ADMIN_MNL_INVC_DTL 
	ON (THRD_PTY_ADMIN_MNL_INVC_DTL.thrd_pty_admin_mnl_invc_id = THRD_PTY_ADMIN_MNL_INVC.thrd_pty_admin_mnl_invc_id 
	AND THRD_PTY_ADMIN_MNL_INVC_DTL.custmr_id = CUSTMR.custmr_id 
	AND THRD_PTY_ADMIN_MNL_INVC_DTL.POST_TRNS_TYP_ID = POST_TRNS_TYP.POST_TRNS_TYP_ID) LEFT JOIN 
	INT_ORG ON (THRD_PTY_ADMIN_MNL_INVC.bsn_unt_ofc_id = INT_ORG.int_org_id)
WHERE POST_TRNS_TYP.TRNS_TYP_ID=460 -- TPA MANUAL POSTINGS
	AND POST_TRNS_TYP.actv_ind=1
	AND POST_TRNS_TYP.post_ind=1
	AND THRD_PTY_ADMIN_MNL_INVC.invc_nbr_txt IS NOT NULL 
	AND THRD_PTY_ADMIN_MNL_INVC_DTL.thrd_pty_admin_amt <> 0
	AND THRD_PTY_ADMIN_MNL_INVC.thrd_pty_admin_mnl_invc_id = @thrd_pty_admin_mnl_invc_id

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

if object_id('ModAIS_TPATransmittalToARiES') is not null
	print 'Created Procedure ModAIS_TPATransmittalToARiES'
else
	print 'Failed Creating Procedure ModAIS_TPATransmittalToARiES'
go

if object_id('ModAIS_TPATransmittalToARiES') is not null
	grant exec on ModAIS_TPATransmittalToARiES to public
go





