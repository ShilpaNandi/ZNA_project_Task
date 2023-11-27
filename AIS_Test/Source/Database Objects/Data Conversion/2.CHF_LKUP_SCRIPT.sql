
---------------------------------------------------------------------
-----
-----	Script Name:	Lkup Script
-----
-----	Version:	SQL Server 2012
-----
-----	Description:	This script is used to update the lokkup data
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----				05/11/15  Venkat Kolimi
-----				Modified the Lkup information Based on the production max lkup id

----- TODO: 

---------------------------------------------------------------------

--STEP 1 :

--Adding NEW LKUP_TYP
SET IDENTITY_INSERT dbo.LKUP_TYP ON

--LKUP_TYP
insert into LKUP_TYP (LKUP_TYP_ID,crte_dt,lkup_typ_nm_txt,actv_ind, crte_user_id) values(68,getdate(),'CHF TAX DESCRIPTION',1,1)

SET IDENTITY_INSERT dbo.LKUP_TYP OFF

--STEP 2:

--Updating LKUP's

update lkup set lkup_txt='AL - Bodily Injury',actv_ind=1,updt_user_id=1,updt_dt=getdate() where lkup_id=93 

update lkup set lkup_txt='AL/GAR - Let Rest',actv_ind=1,updt_user_id=1,updt_dt=getdate() where lkup_id=94

update lkup set lkup_txt='AL - Property Damage',actv_ind=1,updt_user_id=1,updt_dt=getdate() where lkup_id=95

update lkup set lkup_txt='GL - All Other',actv_ind=1,updt_user_id=1,updt_dt=getdate() where lkup_id=96

update lkup set lkup_txt='APD',actv_ind=1,updt_user_id=1,updt_dt=getdate() where lkup_id=97

update lkup set lkup_txt='GL - Bodily Injury',actv_ind=1,updt_user_id=1,updt_dt=getdate() where lkup_id=98

update lkup set actv_ind=0,updt_user_id=1,updt_dt=getdate() where lkup_id=99

update lkup set lkup_txt='GL - Let Rest',actv_ind=1,updt_user_id=1,updt_dt=getdate() where lkup_id=100

update lkup set lkup_txt='GL - Property Damage',actv_ind=1,updt_user_id=1,updt_dt=getdate() where lkup_id=101

update lkup set lkup_txt='General Liability - All Other',actv_ind=0,updt_user_id=1,updt_dt=getdate() where lkup_id=102

update lkup set lkup_txt='GL - Pollution',actv_ind=1,updt_user_id=1,updt_dt=getdate() where lkup_id=103

update lkup set lkup_txt='GL - Premises',actv_ind=1,updt_user_id=1,updt_dt=getdate() where lkup_id=104

update lkup set lkup_txt='GL - Products',actv_ind=1,updt_user_id=1,updt_dt=getdate() where lkup_id=105

update lkup set actv_ind=0,updt_user_id=1,updt_dt=getdate() where lkup_id=106

update lkup set lkup_txt='WC - Indemnity (Lost Time)',actv_ind=1,updt_user_id=1,updt_dt=getdate() where lkup_id=107

update lkup set actv_ind=0,updt_user_id=1,updt_dt=getdate() where lkup_id=108

update lkup set actv_ind=0,updt_user_id=1,updt_dt=getdate() where lkup_id=109

update lkup set actv_ind=0,updt_user_id=1,updt_dt=getdate() where lkup_id=110

update lkup set lkup_txt='WC - Let Rest',actv_ind=1,updt_user_id=1,updt_dt=getdate() where lkup_id=111

update lkup set actv_ind=0,updt_user_id=1,updt_dt=getdate() where lkup_id=112

update lkup set actv_ind=0,updt_user_id=1,updt_dt=getdate() where lkup_id=113

update lkup set lkup_txt='WC - Medical Only',actv_ind=1,updt_user_id=1,updt_dt=getdate() where lkup_id=506

update lkup set actv_ind=0,updt_user_id=1,updt_dt=getdate() where lkup_id=515

update lkup set lkup_txt='WC - Closed No Payment',actv_ind=1,updt_user_id=1,updt_dt=getdate() where lkup_id=516

update lkup set lkup_txt='WC - Managed Medical',actv_ind=1,updt_user_id=1,updt_dt=getdate() where lkup_id=729


--STEP 3:

-- Adding new Lkup's

SET IDENTITY_INSERT dbo.LKUP ON

--Adding "CHF LOSSTYPE" LKUP's

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (756,8,'GAR - Bodily Injury',getdate(),1,getdate(),1)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (757,8,'GAR - Property Damage',getdate(),1,getdate(),1)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (758,8,'GAR - Combined BI/PD',getdate(),1,getdate(),1)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (759,8,'GAR - Major Case unit',getdate(),1,getdate(),1)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (760,8,'GAR - Closed no pay',getdate(),1,getdate(),1)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (761,8,'Garage Liability - Let Rest',getdate(),1,getdate(),0)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (762,8,'GKLL',getdate(),1,getdate(),1)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (763,8,'GKLL - Closed no Payment',getdate(),1,getdate(),1)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (764,8,'Garage Keepers Legal Liability - Let Rest',getdate(),1,getdate(),0)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (765,8,'APD - Closed No Payment',getdate(),1,getdate(),1)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (766,8,'APD / GKLL - Let Rest',getdate(),1,getdate(),1)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (767,8,'General Liability - All Other',getdate(),1,getdate(),0)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (768,8,'GL - Construction Defect',getdate(),1,getdate(),1)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (769,8,'GL - Major Case Unit',getdate(),1,getdate(),1)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (770,8,'GL - Liquor Liability BI',getdate(),1,getdate(),1)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (771,8,'GL - Liquor Liability PD',getdate(),1,getdate(),1)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (772,8,'General Liability - Construction Defect',getdate(),1,getdate(),0)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (773,8,'GL - Closed No Payment',getdate(),1,getdate(),1)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (774,8,'WC - Major Case Unit',getdate(),1,getdate(),1)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (775,8,'Additional Claim Review Charges',getdate(),1,getdate(),1)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (776,8,'Other Charge',getdate(),1,getdate(),1)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (777,8,'AL - BI/PD Combined',getdate(),1,getdate(),1)


INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (778,8,'AL - Major Case Unit',getdate(),1,getdate(),1)


INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (779,8,'AL - Closed No Payment',getdate(),1,getdate(),1)

--Adding "DEDUCTIBLE TAX COMPONENT" LKUP's

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (780,54,'CHF',getdate(),1,getdate(),1)


--Adding "CHF TAX DESCRIPTION" LKUP's

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (781,68,'CHF - Based on Claimant State',getdate(),1,getdate(),1)

INSERT INTO lkup (LKUP_ID,lkup_typ_id,lkup_txt,eff_dt,crte_user_id,crte_dt,actv_ind)
VALUES (782,68,'CHF - Based on Loss Location State',getdate(),1,getdate(),1)


SET IDENTITY_INSERT dbo.LKUP OFF


--STEP 4 :

--Adding new POST_TRNS_TYP


SET IDENTITY_INSERT dbo.POST_TRNS_TYP ON

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(140,328,'CHF LF Adjustment  C&RM','8000','0130 ','Z01',0,1,0,0,0,1,getdate(),1)

SET IDENTITY_INSERT dbo.POST_TRNS_TYP OFF


--STEP 5:


--One Time Data fix

update PREM_ADJ_LOS_REIM_FUND_POST_TAX
set dedtbl_tax_cmpnt_id=536
where dedtbl_tax_cmpnt_id is null