SET IDENTITY_INSERT dbo.LKUP ON

--AIS LOSS Source
insert into LKUP (LKUP_ID,crte_dt,eff_dt,actv_ind,crte_user_id,lkup_typ_id,lkup_txt,attr_1_txt) values(677,getdate(),'08/01/2011',1,1,6,'ARMIS/TPA','')

SET IDENTITY_INSERT dbo.LKUP OFF

select * from lkup