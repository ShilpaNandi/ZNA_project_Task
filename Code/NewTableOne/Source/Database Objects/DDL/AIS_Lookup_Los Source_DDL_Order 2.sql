/********************************************************************************************
							
As per the SR SR#356094,SR#356095,SR#356096 (AIS Canada to Zurich AIS Application Enhancements) 
--Script for adding lookup 'Canadian Risk Intelligence' under 'Loss Source' lookup type
		
*********************************************************************************************/







SET IDENTITY_INSERT dbo.LKUP ON

insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(737,getdate(),GETDATE(),1,1,6,'Canadian RI','')

SET IDENTITY_INSERT dbo.LKUP OFF


-------------------------------------------------------------------------------------------------------------------------------------------------

