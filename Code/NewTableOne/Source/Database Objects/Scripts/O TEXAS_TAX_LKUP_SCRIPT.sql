
---------------------------------------------------------------------
-----
-----	Script Name:	Lkup Script
-----
-----	Version:	SQL Server 2005
-----
-----	Description:	This script is used to update the lokkup data
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----               02/15/10	Venkat Kolimi
-----				Created Procedure

-----               06/01/10  Venkat Kolimi
-----				Modified the Lkup information Based on the production max lkup id
-----				07/23/10  Venkat Kolimi
-----				Modified the Lkup information Based on the production max lkup id

----- TODO: 

---------------------------------------------------------------------
SET IDENTITY_INSERT dbo.INVC_EXHIBIT_SETUP ON

INSERT INTO dbo.INVC_EXHIBIT_SETUP (INVC_EXHIBIT_SETUP_ID,crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) 
values(19,'01/01/2010','INV19', 'State Sales & Service Tax Exhibit-External',1,'B',19,0,0)

INSERT INTO dbo.INVC_EXHIBIT_SETUP (INVC_EXHIBIT_SETUP_ID,crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) 
values(20,'01/01/2010','INV20', 'State Sales & Service Tax Exhibit-Internal',1,'I',20,0,0)

SET IDENTITY_INSERT dbo.INVC_EXHIBIT_SETUP OFF



SET IDENTITY_INSERT dbo.LKUP_TYP ON

--LKUP_TYP
insert into LKUP_TYP (LKUP_TYP_ID,crte_dt,lkup_typ_nm_txt,actv_ind, crte_user_id) values(53,'01/01/2010','TAX TYPE',1,1)

insert into LKUP_TYP (LKUP_TYP_ID,crte_dt,lkup_typ_nm_txt,actv_ind, crte_user_id) values(54,'01/01/2010','DEDUCTIBLE TAX COMPONENT',1,1)

insert into LKUP_TYP (LKUP_TYP_ID,crte_dt,lkup_typ_nm_txt,actv_ind, crte_user_id) values(55,'01/01/2010','ALAE TAX DESCRIPTION',1,1)

insert into LKUP_TYP (LKUP_TYP_ID,crte_dt,lkup_typ_nm_txt,actv_ind, crte_user_id) values(56,'01/01/2010','LCF TAX DESCRIPTION',1,1)

insert into LKUP_TYP (LKUP_TYP_ID,crte_dt,lkup_typ_nm_txt,actv_ind, crte_user_id) values(57,'01/01/2010','LBA TAX DESCRIPTION',1,1)

insert into LKUP_TYP (LKUP_TYP_ID,crte_dt,lkup_typ_nm_txt,actv_ind, crte_user_id) values(58,'01/01/2010','IBNR TAX DESCRIPTION',1,1)

insert into LKUP_TYP (LKUP_TYP_ID,crte_dt,lkup_typ_nm_txt,actv_ind, crte_user_id) values(59,'01/01/2010','LDF TAX DESCRIPTION',1,1)

SET IDENTITY_INSERT dbo.LKUP_TYP OFF




SET IDENTITY_INSERT dbo.LKUP ON
--LKUP
--TAX TYPE(53)
insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(526,'01/01/2010','01/01/2010',1,1,53,'TX Sales & Use Tax','TX')


--DEDUCTIBLE TAX COMPONENT(54)

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(535,'01/01/2010','01/01/2010',1,1,54,'LBA','')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(536,'01/01/2010','01/01/2010',1,1,54,'LCF','')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(537,'01/01/2010','01/01/2010',1,1,54,'IBNR','')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(538,'01/01/2010','01/01/2010',1,1,54,'LDF','')


--ALAE TAX DESCRIPTION(55)


insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(527,'01/01/2010','01/01/2010',1,1,55,'ALAE - Service Fees Based on Texas State','TX Sales & Use Tax')

--LCF TAX DESCRIPTION(56)


insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(528,'01/01/2010','01/01/2010',1,1,56,'LCF - Based on Texas State','TX Sales & Use Tax')

--LBA TAX DESCRIPTION(57)


insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(529,'01/01/2010','01/01/2010',1,1,57,'LBA – Based on Texas State','TX Sales & Use Tax')

--IBNR TAX DESCRIPTION(58)


insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(530,'01/01/2010','01/01/2010',1,1,58,'IBNR – Based on Texas State','TX Sales & Use Tax')

--LDF TAX DESCRIPTION(59)


insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(531,'01/01/2010','01/01/2010',1,1,59,'LDF - Based on Texas State','TX Sales & Use Tax')

--DEDUCTIBLE TAX(28)

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(532,'01/01/2010','01/01/2010',1,1,28,'DEDUCTIBLE TAX','')

--ADJUSTMENT DOCUMENT(26)


insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(533,'01/01/2010','01/01/2010',1,1,26,'State Sales & Service Tax Exhibit (External)','Y')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(534,'01/01/2010','01/01/2010',1,1,26,'State Sales & Service Tax Exhibit (Internal)','Y')

SET IDENTITY_INSERT dbo.LKUP OFF


SET IDENTITY_INSERT dbo.POST_TRNS_TYP ON

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(80,532,'TX Sales & Use Tax','8000','5503','Z01',1,1,0,0,0,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(81,460,'TX Sales & Use Tax','8000','5503','Z01',0,1,0,0,1,1,getdate(),1)

SET IDENTITY_INSERT dbo.POST_TRNS_TYP OFF