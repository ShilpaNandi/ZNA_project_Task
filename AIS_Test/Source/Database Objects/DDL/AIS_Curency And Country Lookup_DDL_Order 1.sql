/********************************************************************************************
							
As per the SR SR#356094,SR#356095,SR#356096 (AIS Canada to Zurich AIS Application Enhancements) , we need to add new lookup data for company code and currency code
		
*********************************************************************************************/


-- To Add lookup type 'Company Code' and 'Currency Code'

SET IDENTITY_INSERT LKUP_TYP ON

insert into LKUP_TYP(lkup_typ_id,lkup_typ_nm_txt,crte_user_id,crte_dt,actv_ind)
values(66,'COMPANY CODE',1,getdate(),1)

insert into LKUP_TYP(lkup_typ_id,lkup_typ_nm_txt,crte_user_id,crte_dt,actv_ind)
values(67,'CURRENCY CODE',1,getdate(),1)

SET IDENTITY_INSERT LKUP_TYP OFF

----------------------------------------------------------------------------

-- TO Add lookup values under Company Code

SET IDENTITY_INSERT LKUP ON

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) 
values(733,getdate(),GETDATE(),1,1,66,'Z01','Zurich')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) 
values(734,getdate(),GETDATE(),1,1,66,'ZC2','Canada')

SET IDENTITY_INSERT LKUP OFF

-----------------------------------------------------------------------------------------

-- TO Add lookup values under Currency Code

SET IDENTITY_INSERT LKUP ON

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) 
values(735,getdate(),GETDATE(),1,1,67,'USD','USA')

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) 
values(736,getdate(),GETDATE(),1,1,67,'CAD','Canada')

SET IDENTITY_INSERT LKUP OFF

-----------------------------------------------------------------------------