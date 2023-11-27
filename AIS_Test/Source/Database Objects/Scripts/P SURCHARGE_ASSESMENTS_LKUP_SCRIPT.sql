---------------------------------------------------------------------
-----
-----	Script Name:	SUrcharge Assesments Lkup Script
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


----- TODO: 

---------------------------------------------------------------------

SET IDENTITY_INSERT dbo.LKUP_TYP ON

--LKUP_TYP
insert into LKUP_TYP (LKUP_TYP_ID,crte_dt,lkup_typ_nm_txt,actv_ind, crte_user_id) values(60,getdate(),'SURCHARGES AND ASSESSMENTS',1,1)

insert into LKUP_TYP (LKUP_TYP_ID,crte_dt,lkup_typ_nm_txt,actv_ind, crte_user_id) values(61,getdate(),'SURCHARGE ASSESSMENT CODE',1,1)

insert into LKUP_TYP (LKUP_TYP_ID,crte_dt,lkup_typ_nm_txt,actv_ind, crte_user_id) values(62,getdate(),'SURCHARGE DATE INDICATOR',1,1)

insert into LKUP_TYP (LKUP_TYP_ID,crte_dt,lkup_typ_nm_txt,actv_ind, crte_user_id) values(63,getdate(),'SURCHARGE ASSESSMENT ABBREVIATED DESCRIPTION',1,1)



SET IDENTITY_INSERT dbo.LKUP_TYP OFF




SET IDENTITY_INSERT dbo.LKUP ON
--LKUP
--TAX TYPE(60)
--SURCHARGES AND ASSESSMENTS
insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(545,getdate(),'08/01/2010',1,1,60,'KY Special Fund Assmt','816')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(546,getdate(),'08/01/2010',1,1,60,'Oregon Premium Assmt','836')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(547,getdate(),'08/01/2010',1,1,60,'CA WC Fraud Surcharge','204')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(548,getdate(),'08/01/2010',1,1,60,'CA Uninsured Employers Benefits Trust Fund Assmt','304')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(549,getdate(),'08/01/2010',1,1,60,'CA Insurance Guaranty Association Surcharge','604')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(550,getdate(),'08/01/2010',1,1,60,'CA WC Admin. Revolving Fund Assmt','104')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(551,getdate(),'08/01/2010',1,1,60,'CA Subsequent Injuries Benefits Trust Fund Assmt','404')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(552,getdate(),'08/01/2010',1,1,60,'CA Occupational Safety and Health Fund Assmt','181')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(553,getdate(),'08/01/2010',1,1,60,'D.C. Special Fund','108')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(554,getdate(),'08/01/2010',1,1,60,'CT Second Injury Fund','206')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(555,getdate(),'08/01/2010',1,1,60,'IL WC Commission Operations Fund Surcharge','112')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(556,getdate(),'08/01/2010',1,1,60,'IN Second Injury Fund Surcharge','113')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(557,getdate(),'08/01/2010',1,1,60,'MO Second Injury Fund','424')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(558,getdate(),'08/01/2010',1,1,60,'MT Admin. Fund Surcharge','125')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(559,getdate(),'08/01/2010',1,1,60,'MT Subsequent Injury Fund Surcharge','225')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(560,getdate(),'08/01/2010',1,1,60,'NY State Assmt','0932')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(561,getdate(),'08/01/2010',1,1,60,'NY WC Security Fund','931')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(562,getdate(),'08/01/2010',1,1,60,'PA Employer Assmt. Factor','437')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(563,getdate(),'08/01/2010',1,1,60,'VT WC Admin. Fund Assmt','144')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(564,getdate(),'08/01/2010',1,1,60,'WV Regulatory Surcharge','447')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(565,getdate(),'08/01/2010',1,1,60,'WV Debt Reduction Surcharge','547')


--SURCHARGE ASSESSMENT CODE(61)


insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(566,getdate(),'08/01/2010',1,1,61,'816','KY')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(567,getdate(),'08/01/2010',1,1,61,'836','OR')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(568,getdate(),'08/01/2010',1,1,61,'604','CA')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(569,getdate(),'08/01/2010',1,1,61,'204','CA')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(570,getdate(),'08/01/2010',1,1,61,'304','CA')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(571,getdate(),'08/01/2010',1,1,61,'104','CA')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(572,getdate(),'08/01/2010',1,1,61,'404','CA')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(573,getdate(),'08/01/2010',1,1,61,'181','CA')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(574,getdate(),'08/01/2010',1,1,61,'108','DC')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(575,getdate(),'08/01/2010',1,1,61,'206','CT')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(576,getdate(),'08/01/2010',1,1,61,'112','IL')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(577,getdate(),'08/01/2010',1,1,61,'113','IN')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(578,getdate(),'08/01/2010',1,1,61,'424','MO')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(579,getdate(),'08/01/2010',1,1,61,'125','MT')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(580,getdate(),'08/01/2010',1,1,61,'225','MT')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(581,getdate(),'08/01/2010',1,1,61,'0932','NY')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(582,getdate(),'08/01/2010',1,1,61,'931','NY')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(583,getdate(),'08/01/2010',1,1,61,'437','PA')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(584,getdate(),'08/01/2010',1,1,61,'144','VT')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(585,getdate(),'08/01/2010',1,1,61,'447','WV')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(586,getdate(),'08/01/2010',1,1,61,'547','WV')


--SURCHARGE DATE INDICATOR(62)

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(587,getdate(),'08/01/2010',1,1,62,'P','')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(588,getdate(),'08/01/2010',1,1,62,'R','')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(589,getdate(),'08/01/2010',1,1,62,'D','')

--SURCHARGE ASSESSMENT ABBREVIATED DESCRIPTION(63)

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(590,getdate(),'08/01/2010',1,1,63,'SF','KY Special Fund Assmt')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(591,getdate(),'08/01/2010',1,1,63,'PA','Oregon Premium Assmt')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(592,getdate(),'08/01/2010',1,1,63,'SFS','CA WC Fraud Surcharge')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(593,getdate(),'08/01/2010',1,1,63,'UEBTF','CA Uninsured Employers Benefits Trust Fund Assmt')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(594,getdate(),'08/01/2010',1,1,63,'IGA','CA Insurance Guaranty Association Surcharge')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(595,getdate(),'08/01/2010',1,1,63,'WCRAF','CA WC Admin. Revolving Fund Assmt')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(596,getdate(),'08/01/2010',1,1,63,'SIBTF','CA Subsequent Injuries Benefits Trust Fund Assmt')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(597,getdate(),'08/01/2010',1,1,63,'OSHF','CA Occupational Safety and Health Fund Assmt')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(598,getdate(),'08/01/2010',1,1,63,'SF','D.C. Special Fund')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(599,getdate(),'08/01/2010',1,1,63,'SIF','CT Second Injury Fund')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(600,getdate(),'08/01/2010',1,1,63,'ICOF','IL WC Commission Operations Fund Surcharge')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(601,getdate(),'08/01/2010',1,1,63,'SIF','IN Second Injury Fund Surcharge')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(602,getdate(),'08/01/2010',1,1,63,'SIF','MO Second Injury Fund')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(603,getdate(),'08/01/2010',1,1,63,'WCAF','MT Admin. Fund Surcharge')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(604,getdate(),'08/01/2010',1,1,63,'SIF','MT Subsequent Injury Fund Surcharge')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(605,getdate(),'08/01/2010',1,1,63,'SA','NY State Assmt')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(606,getdate(),'08/01/2010',1,1,63,'WCSF','NY WC Security Fund')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(607,getdate(),'08/01/2010',1,1,63,'EAF','PA Employer Assmt. Factor')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(608,getdate(),'08/01/2010',1,1,63,'WCAF','VT WC Admin. Fund Assmt')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(609,getdate(),'08/01/2010',1,1,63,'RS','WV Regulatory Surcharge')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(610,getdate(),'08/01/2010',1,1,63,'DRS','WV Debt Reduction Surcharge')



--SURCHARGES AND ASSESSMENTS(28)

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(611,getdate(),'08/01/2010',1,1,28,'SURCHARGES AND ASSESSMENTS','')

--ADJUSTMENT DOCUMENT(26)


insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(612,getdate(),'08/01/2010',1,1,26,'Retro Premium Based Surcharges & Assessments','Y')



SET IDENTITY_INSERT dbo.LKUP OFF



SET IDENTITY_INSERT dbo.POST_TRNS_TYP ON


INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(103,611,'Oregon Premium Assmt','4500','0147','Z01',1,1,0,0,0,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(104,611,'KY Special Fund Assmt','4500','0122','Z01',1,1,0,0,0,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(84,611,'CA WC Fraud Surcharge','4500','0184','Z01',0,1,0,0,1,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(85,611,'CA Uninsured Employers Benefits Trust Fund Assmt','4500','0185','Z01',1,1,0,0,0,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(86,611,'CA Insurance Guaranty Association Surcharge','4500','0186','Z01',0,1,0,0,1,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(87,611,'CA WC Admin. Revolving Fund Assmt','4500','0187','Z01',1,1,0,0,0,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(88,611,'CA Subsequent Injuries Benefits Trust Fund Assmt','4500','0188','Z01',0,1,0,0,1,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(89,611,'CA Occupational Safety and Health Fund Assmt','4500','0189','Z01',1,1,0,0,0,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(90,611,'D.C. Special Fund','4500','0190','Z01',0,1,0,0,1,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(91,611,'CT Second Injury Fund','4500','0191','Z01',1,1,0,0,0,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(92,611,'IL WC Commission Operations Fund Surcharge','4500','0192','Z01',0,1,0,0,1,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(93,611,'IN Second Injury Fund Surcharge','4500','0193','Z01',1,1,0,0,0,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(94,611,'MO Second Injury Fund','4500','0194','Z01',0,1,0,0,1,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(95,611,'MT Admin. Fund Surcharge','4500','0195','Z01',1,1,0,0,0,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(96,611,'MT Subsequent Injury Fund Surcharge','4500','0196','Z01',0,1,0,0,1,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(97,611,'NY State Assmt','4500','0159','Z01',1,1,0,0,0,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(98,611,'NY WC Security Fund','4500','0197','Z01',1,1,0,0,0,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(99,611,'PA Employer Assmt. Factor','4500','0198','Z01',0,1,0,0,1,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(100,611,'VT WC Admin. Fund Assmt','4500','0199','Z01',1,1,0,0,0,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(101,611,'WV Regulatory Surcharge','4500','0200','Z01',0,1,0,0,1,1,getdate(),1)

INSERT INTO POST_TRNS_TYP(post_trns_typ_id,trns_typ_id,trns_nm_txt,main_nbr_txt,sub_nbr_txt,comp_txt,invoicbl_ind,post_ind,thrd_pty_admin_mnl_ind,adj_sumry_ind,pol_reqr_ind,crte_user_id,crte_dt,actv_ind)
values(102,611,'WV Debt Reduction Surcharge','4500','0201','Z01',1,1,0,0,0,1,getdate(),1)

SET IDENTITY_INSERT dbo.POST_TRNS_TYP OFF



--dbo.INVC_EXHIBIT_SETUP Data 
truncate table dbo.INVC_EXHIBIT_SETUP


INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV1',  'Broker Letter',1,'B',1,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV2',  'Adjustment Invoice',1,'B',2,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV3',  'Retrospective Premium Adjustment',1,'B',3,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV4',  'Retrospective Premium Adjustment Legend',1,'B',4,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV5',  'Loss Based Assessment Exhibit',1,'B',5,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV6',  'Claims Handling Fee', 1,'B',6,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV7',  'Excess Losses',1,'B',7,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV8',  'Cumulative Totals Worksheet',1,'B',8,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV9',  'Residual Market Subsidy Charge',1,'B',9,1,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV10', 'Retro Premium Based Surcharges & Assessments',1, 'B', 10,1,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV11', 'Loss Reimbursement Fund Adjustment -External',1,'B',11,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV12', 'Escrow Fund Adjustment',1,'B',12,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV13', 'Loss Reimbursement Fund Adjustment-Internal',1,'I',13, 0, 0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV14', 'Aries Posting Details', 1,'I',14,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV15', 'Combined Elements Exhibit - Internal',1,'I',15,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV16', 'Processing and Distribution Checklist',1,'I',16,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2008','INV17', 'Cesar Coding Worksheet',1,'I',17,1,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2010','INV18', 'State Sales & Service Tax Exhibit-External',1,'B',18,0,0)
INSERT INTO dbo.INVC_EXHIBIT_SETUP (crte_dt,atch_cd,atch_nm,sts_ind,intrnl_flag_cd,seq_nbr,cesar_cd_ind,crte_user_id) values('01/01/2010','INV19', 'State Sales & Service Tax Exhibit-Internal',1,'I',19,0,0)

go

