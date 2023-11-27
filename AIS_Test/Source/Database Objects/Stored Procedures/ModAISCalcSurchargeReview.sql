if exists (select 1 from sysobjects 
		where name = 'ModAISCalcSurchargeReview' and type = 'P')
	drop procedure ModAISCalcSurchargeReview
go

set ansi_nulls off
go
---------------------------------------------------------------------
-----
-----	Proc Name:	ModAISCalcSurchargeReview
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This stored procedure is used to Calculate the Surcharge amount. This will be called form the 
-----					Adjustmnet review screen when ever users updated the other surcharges and credits amount.
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----               11/27/09	Venkat Kolimi
-----				Created Procedure

----- TODO: Need to work on this

---------------------------------------------------------------------

CREATE procedure [dbo].[ModAISCalcSurchargeReview] 
@premium_adj_period_id int,
@premium_adjustment_id int,
@customer_id int,
@premium_adj_prog_id int,
@tot_addn_rtn_0932 decimal(15,2),
@state_id int,
@ln_of_bsn_id int,
@com_agm_id int,
@surcharge_cd_id int,
@surcharge_type_id int, 
@create_user_id int,
@err_msg_op varchar(1000) output,
@debug bit = 0
as

begin
	set nocount on

declare	@retro_result decimal(15,2),
		@addn_surchrg_asses_cmpnt decimal(15,2),
		@tot_surchrg_asses_base decimal(15,2),
		@surchrg_rt decimal(15,8),
		@addn_rtn decimal(15,2),
		@scode_txt varchar(20),
		@err_message varchar(500),
		@surchrg_cd_id_0932 int,
		@surchrg_typ_id_0932 int,
		@surchrg_cd_id_931 int,
		@surchrg_typ_id_931 int,
		@trancount int

set @trancount = @@trancount
--print @trancount
if @trancount >= 1
    save transaction ModAISCalcSurchargeReview
else
    begin transaction


begin try
		

					select @scode_txt=lkup_txt 
					from  LKUP
					where lkup_id=@surcharge_cd_id
					
					select @surchrg_cd_id_0932=lkup_id from lkup
					inner join lkup_typ on lkup.lkup_typ_id=lkup_typ.lkup_typ_id
					where lkup.lkup_txt='0932' and lkup.attr_1_txt='NY'
					and lkup_typ_nm_txt='SURCHARGE ASSESSMENT CODE'
					
					select @surchrg_typ_id_0932=lkup_id from lkup
					inner join lkup_typ on lkup.lkup_typ_id=lkup_typ.lkup_typ_id
					where lkup.lkup_txt='NY STATE ASSMT' and lkup.attr_1_txt='0932'
					and lkup_typ_nm_txt='SURCHARGES AND ASSESSMENTS'
					
					select @surchrg_cd_id_931=lkup_id from lkup
					inner join lkup_typ on lkup.lkup_typ_id=lkup_typ.lkup_typ_id
					where lkup.lkup_txt='931' and lkup.attr_1_txt='NY'
					and lkup_typ_nm_txt='SURCHARGE ASSESSMENT CODE'
					
					select @surchrg_typ_id_931=lkup_id from lkup
					inner join lkup_typ on lkup.lkup_typ_id=lkup_typ.lkup_typ_id
					where lkup.lkup_txt='NY WC SECURITY FUND' and lkup.attr_1_txt='931'
					and lkup_typ_nm_txt='SURCHARGES AND ASSESSMENTS'
					
		
					--To Do: Need to validate this logic
					if(@state_id=35 and (@scode_txt='931' OR @scode_txt='0932'))
					begin
					select @addn_surchrg_asses_cmpnt=isnull(calc.addn_rtn,0)+isnull(review.other_surchrg_amt,0)
					from prem_adj_surchrg_dtl calc
					inner join prem_adj_surchrg_dtl_amt review on calc.ln_of_bsn_id = review.ln_of_bsn_id
					and calc.st_id = review.st_id
					and calc.coml_agmt_id = review.coml_agmt_id
					and calc.surchrg_typ_id = review.surchrg_typ_id
					and calc.surchrg_cd_id = review.surchrg_cd_id
					and calc.prem_adj_perd_id=review.prem_adj_perd_id
					and calc.prem_adj_id=review.prem_adj_id
					and calc.prem_adj_pgm_id=review.prem_adj_pgm_id		
		 			where calc.prem_adj_id = @premium_adjustment_id
					and calc.prem_adj_perd_id = @premium_adj_period_id
					and calc.custmr_id = @customer_id
					and calc.coml_agmt_id=@com_agm_id
					and calc.st_id=@state_id
					and calc.ln_of_bsn_id=@ln_of_bsn_id
					and calc.surchrg_cd_id=@surchrg_cd_id_0932
					and calc.surchrg_typ_id=@surchrg_typ_id_0932

					select @retro_result=retro_rslt,
					@surchrg_rt=surchrg_rt
					from prem_adj_surchrg_dtl calc
					where calc.prem_adj_id = @premium_adjustment_id
					and calc.prem_adj_perd_id = @premium_adj_period_id
					and calc.custmr_id = @customer_id
					and calc.coml_agmt_id=@com_agm_id
					and calc.st_id=@state_id
					and calc.ln_of_bsn_id=@ln_of_bsn_id
					and calc.surchrg_cd_id=@surchrg_cd_id_931
					and calc.surchrg_typ_id=@surchrg_typ_id_931	

			  
					set @tot_surchrg_asses_base=isnull(@retro_result,0)+isnull(@addn_surchrg_asses_cmpnt,0)			
										 
					set @addn_rtn=isnull(@tot_surchrg_asses_base,0)*@surchrg_rt
					
					set @addn_rtn=round(@addn_rtn,0)

					if @debug = 1
					begin
				    print'---------------Calculated values for all the surcharges-------------------' 
					print' @retro_result: ' + convert(varchar(20), @retro_result) 
					print' @addn_surchrg_asses_cmpnt: ' + convert(varchar(20), @addn_surchrg_asses_cmpnt) 
					print' @tot_surchrg_asses_base: ' + convert(varchar(20), @tot_surchrg_asses_base) 
					print' @addn_rtn: ' + convert(varchar(20), @addn_rtn)
					end
				
					update PREM_ADJ_SURCHRG_DTL
					set addn_surchrg_asses_cmpnt=@addn_surchrg_asses_cmpnt,
					tot_surchrg_asses_base=@tot_surchrg_asses_base,
					addn_rtn=@addn_rtn
					where prem_adj_id = @premium_adjustment_id
					and prem_adj_perd_id = @premium_adj_period_id
					and custmr_id = @customer_id
					and coml_agmt_id=@com_agm_id
					and st_id=@state_id
					and ln_of_bsn_id=@ln_of_bsn_id
					and surchrg_cd_id=@surchrg_cd_id_931
					and surchrg_typ_id=@surchrg_typ_id_931			

					end

					update PREM_ADJ_SURCHRG_DTL
					set tot_addn_rtn=isnull(calc.addn_rtn,0)+isnull(review.other_surchrg_amt,0)
					from prem_adj_surchrg_dtl calc
					inner join prem_adj_surchrg_dtl_amt review on calc.ln_of_bsn_id = review.ln_of_bsn_id
					and calc.st_id = review.st_id
					and calc.coml_agmt_id = review.coml_agmt_id
					and calc.surchrg_typ_id = review.surchrg_typ_id
					and calc.surchrg_cd_id = review.surchrg_cd_id
					and calc.prem_adj_perd_id=review.prem_adj_perd_id
					and calc.prem_adj_id=review.prem_adj_id
					and calc.prem_adj_pgm_id=review.prem_adj_pgm_id	
					where calc.prem_adj_id = @premium_adjustment_id
					and calc.prem_adj_perd_id = @premium_adj_period_id
					and calc.custmr_id = @customer_id
					and calc.coml_agmt_id=@com_agm_id
					and calc.st_id=@state_id
					and calc.ln_of_bsn_id=@ln_of_bsn_id
					and calc.surchrg_cd_id=@surcharge_cd_id
					and calc.surchrg_typ_id=@surcharge_type_id
					
					
					--This is for the Surcharges and Assessments adjustment summary invoice
					DELETE FROM PREM_ADJ_PERD_TOT WITH (ROWLOCK) WHERE prem_adj_perd_id = @premium_adj_period_id
					AND prem_adj_id = @premium_adjustment_id AND custmr_id = @customer_id and invc_adj_typ_txt='Retro Premium Based Surcharges & Assessments'


					insert into [dbo].[PREM_ADJ_PERD_TOT]
						(
							[prem_adj_perd_id]
						   ,[prem_adj_id]
						   ,[custmr_id]
						   ,[invc_adj_typ_txt]
						   ,[tot_amt]
						   ,[crte_user_id]           
						)
					select prem_adj_perd_id,prem_adj_id,custmr_id,'Retro Premium Based Surcharges & Assessments',
					round(sum(TOT_ADDN_RTN),0),@create_user_id
					from PREM_ADJ_SURCHRG_DTL where prem_adj_perd_id = @premium_adj_period_id 
					and prem_adj_id = @premium_adjustment_id
					and custmr_id = @customer_id
					group by prem_adj_perd_id, prem_adj_id, custmr_id
					
					--This is for the aries postings related to surcharges
					delete from aries_trnsmtl_hist 
					where adj_typ_txt='RETRO SURCH & ASSMT'
					and prem_adj_id=@premium_adjustment_id
					
					
			/***************************************************************************************
			This is to re-generate the postings when the users updates the other surcharges and credits amount
			in the adjustmnet review screen.
			The below postings are updated by considering the following conditions
			@Reverse 1 (To be called on calculate.),
			@Ind int -- 1=Normal,
			rel_prem_adj_id=NULL
			Surcharges and Assesments: Added below Postings
			as part of the AIS 21 surcharges project
			************Note:When ever the below postings code is modifede 
			we need to modify the same in ModAISCalcSurchargeReview.sql and in ModAIS_TransmittalToARiES stored procedure
			****************************************************************************************/
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
			CONVERT(char(10), '')  , 		-- GPART(3)
			CONVERT(char(20), '')  , 		-- GPART_EXT(4)
			--(SR 321581)
			CONVERT(char(20), ISNULL(RTRIM(CUSTMR.finc_pty_id),'')+'R')  ,  		-- VTREF(5)
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
			CONVERT(bigint, PREM_ADJ_SURCHRG_DTL.tot_addn_rtn  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_SURCHRG_DTL.tot_addn_rtn  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			CASE WHEN (1*PREM_ADJ_SURCHRG_DTL.tot_addn_rtn  < 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

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
			'RETRO SURCH & ASSMT',0,1,NULL
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
			AND PREM_ADJ.prem_adj_id = @premium_adjustment_id


			--Surcharges--Reclass
			/**********************************************************************************************
			This is to re-generate the postings when the users updates the other surcharges and credits amount
			in the adjustmnet review screen.
			The below postings are updated by considering the following conditions
			@Reverse 1 (To be called on calculate.),
			@Ind int -- 1=Normal,
			rel_prem_adj_id=NULL
			
			Surcharges and Assesments: Added below Postings
			as part of the AIS 21 surcharges project
			************Note:When ever the below postings code is modifeded
			we need to modify the same in ModAISCalcSurchargeReview.sql and ModAIS_TransmittalToARiES stored procedure
			***********************************************************************************************/
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
			CONVERT(bigint, PREM_ADJ_SURCHRG_DTL.tot_addn_rtn  * 100)))) +
			CONVERT(varchar(15), CONVERT(bigint, PREM_ADJ_SURCHRG_DTL.tot_addn_rtn  * 100)), 
			'000000000000000'), ' ', '0'), '-', '0')) + 
			--Because it is re-class, we send -ve amounts when it is actually positive. ">" used instead of "<"
			CASE WHEN (1*PREM_ADJ_SURCHRG_DTL.tot_addn_rtn  > 0)  THEN '-' ELSE ' ' END , -- AMOUNT_TOTAL(22)

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
			'RETRO SURCH & ASSMT',0,1,NULL
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
			AND PREM_ADJ.prem_adj_id = @premium_adjustment_id



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
		rollback transaction ModAISCalcSurchargeReview
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
			+ convert(varchar(20),isnull(@premium_adjustment_id,0)) 
			+ ' for program number: ' 
			+ convert(varchar(20),isnull(@premium_adj_prog_id,0)) 
			+ ' associated with customer number: ' 
			+ convert(varchar(20),isnull(@customer_id,0))
			+ '. Error message: '
			+ isnull(@err_msg, ' ')
	   ,@customer_id
	   ,@premium_adjustment_id
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

if object_id('ModAISCalcSurchargeReview') is not null
	print 'Created Procedure ModAISCalcSurchargeReview'
else
	print 'Failed Creating Procedure ModAISCalcSurchargeReview'
go

if object_id('ModAISCalcSurchargeReview') is not null
	grant exec on ModAISCalcSurchargeReview to public
go
