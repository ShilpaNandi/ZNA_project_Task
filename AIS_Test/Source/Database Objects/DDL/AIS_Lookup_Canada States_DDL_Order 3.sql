/********************************************************************************************
							
As per the SR SR#356094,SR#356095,SR#356096 (AIS Canada to Zurich AIS Application Enhancements) 
-- Adding new states to lkup table as part of C2Z work order (Added new canada states to lookup info)
		
*********************************************************************************************/


 SET IDENTITY_INSERT LKUP ON

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) 
values(738,getdate(),GETDATE(),1,1,1,'Alberta','AB')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) 
values(739,getdate(),GETDATE(),1,1,1,'British Columbia','BC')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) 
values(740,getdate(),GETDATE(),1,1,1,'Manitoba','MB')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) 
values(741,getdate(),GETDATE(),1,1,1,'New Brunswick','NB')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) 
values(742,getdate(),GETDATE(),1,1,1,'Newfoundland and Labrador','NL')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) 
values(743,getdate(),GETDATE(),1,1,1,'Nova Scotia','NS')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) 
values(744,getdate(),GETDATE(),1,1,1,'Northwest Territories','NT')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) 
values(745,getdate(),GETDATE(),1,1,1,'Nunavut','NU')  

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) 
values(746,getdate(),GETDATE(),1,1,1,'Ontario','ON')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) 
values(747,getdate(),GETDATE(),1,1,1,'Prince Edward Island','PE')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) 
values(748,getdate(),GETDATE(),1,1,1,'Quebec','QC')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) 
values(749,getdate(),GETDATE(),1,1,1,'Saskatchewan','SK')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) 
values(750,getdate(),GETDATE(),1,1,1,'Yukon','YT')


SET IDENTITY_INSERT LKUP OFF