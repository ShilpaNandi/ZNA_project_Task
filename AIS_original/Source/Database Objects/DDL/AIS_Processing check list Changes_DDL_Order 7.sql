/********************************************************************************************
							
As per the SR SR#356094,SR#356095,SR#356096 (AIS Canada to Zurich AIS Application Enhancements) 
-- Change the sizre of chklist_sts_cd  column
-- Changing the wording and sorting order for preocessing checklist items

		
*********************************************************************************************/




ALTER TABLE QLTY_CNTRL_LIST ALTER COLUMN chklist_sts_cd char(30) NULL

------------------------------------------------------------------------------------------------------------

update QLTY_CNTRL_MSTR_ISSU_LIST set actv_ind=0,srt_nbr=null where qlty_cntrl_mstr_issu_list_id=16

update QLTY_CNTRL_MSTR_ISSU_LIST set actv_ind=0,srt_nbr=null where qlty_cntrl_mstr_issu_list_id=17

update QLTY_CNTRL_MSTR_ISSU_LIST set actv_ind=0,srt_nbr=null where qlty_cntrl_mstr_issu_list_id=18

update QLTY_CNTRL_MSTR_ISSU_LIST set actv_ind=0,srt_nbr=null where qlty_cntrl_mstr_issu_list_id=23

update QLTY_CNTRL_MSTR_ISSU_LIST set actv_ind=0,srt_nbr=null where qlty_cntrl_mstr_issu_list_id=32

update QLTY_CNTRL_MSTR_ISSU_LIST set actv_ind=0,srt_nbr=null where qlty_cntrl_mstr_issu_list_id=43

update QLTY_CNTRL_MSTR_ISSU_LIST set actv_ind=0,srt_nbr=null where qlty_cntrl_mstr_issu_list_id=44

update QLTY_CNTRL_MSTR_ISSU_LIST set actv_ind=0,srt_nbr=null where qlty_cntrl_mstr_issu_list_id=20

------------------------------------------------------------------------------------------------------------

--update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='Check for the addendums', srt_nbr=1 
--where qlty_cntrl_mstr_issu_list_id=21 -- this i wrong it is moved to calc checklist

insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(228,'Check for the addendums',0,1,1,getdate(),1)

insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(228,'Is this adjustment domestic or foreign?',0,2,1,getdate(),1)

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=3
where qlty_cntrl_mstr_issu_list_id=22

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=4
where qlty_cntrl_mstr_issu_list_id=19

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=5
where qlty_cntrl_mstr_issu_list_id=35

update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='No. of months until conversion date', srt_nbr=6
where qlty_cntrl_mstr_issu_list_id=36

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=7
where qlty_cntrl_mstr_issu_list_id=24

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=8
where qlty_cntrl_mstr_issu_list_id=25

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=9
where qlty_cntrl_mstr_issu_list_id=27

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=10
where qlty_cntrl_mstr_issu_list_id=33

insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(228,'LDFs or IBNRs',0,11,1,getdate(),1)

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=12
where qlty_cntrl_mstr_issu_list_id=34

insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(228,'Aggregate setup',0,13,1,getdate(),1)

update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='ULAE erodes aggregate', srt_nbr=14
where qlty_cntrl_mstr_issu_list_id=26

insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(228,'ALAE erodes aggregate',0,15,1,getdate(),1)

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=16
where qlty_cntrl_mstr_issu_list_id=28

update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='Maximum or Combined Elements', srt_nbr=17
where qlty_cntrl_mstr_issu_list_id=29

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=18
where qlty_cntrl_mstr_issu_list_id=30

insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(228,'Excess Loss Premium',0,19,1,getdate(),1)

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=20
where qlty_cntrl_mstr_issu_list_id=31

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=21
where qlty_cntrl_mstr_issu_list_id=38

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=22
where qlty_cntrl_mstr_issu_list_id=37

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=23
where qlty_cntrl_mstr_issu_list_id=39

update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='Stop Gap payroll included', srt_nbr=24,actv_ind=1
where qlty_cntrl_mstr_issu_list_id=40

update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='Confirm Deferrals', srt_nbr=25
where qlty_cntrl_mstr_issu_list_id=41

update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='Confirm Audit Result', srt_nbr=26,actv_ind=1
where qlty_cntrl_mstr_issu_list_id=42

update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='* Agreement: Is this adjustment domestic or foreign?' where qlty_cntrl_mstr_issu_list_id=115

update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='Insured Name and Type of Program' where qlty_cntrl_mstr_issu_list_id=22

********************************************************************************************************************************
******************************************************************************************************************************

update QLTY_CNTRL_MSTR_ISSU_LIST set actv_ind=0,srt_nbr=null where qlty_cntrl_mstr_issu_list_id=48

update QLTY_CNTRL_MSTR_ISSU_LIST set actv_ind=0,srt_nbr=null where qlty_cntrl_mstr_issu_list_id=50

----------------------------------------------------------------------------------------------------

insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(229,'Check most current account set up documents',0,1,1,getdate(),1)

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=2,issu_catg_id=229,issu_txt='Check the adjustment for Typos and Presentation'
where qlty_cntrl_mstr_issu_list_id=21

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=3,issu_txt='Distribution List up to date'
where qlty_cntrl_mstr_issu_list_id=101

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=4,issu_txt='BU/ Broker changed'
where qlty_cntrl_mstr_issu_list_id=102

insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(229,'Is this a final adjustment',0,5,1,getdate(),1)

insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(229,'For Loss Fund adjustment- Large Loss Payments ',0,6,1,getdate(),1)

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=7,issu_txt='Zurich loss run'
where qlty_cntrl_mstr_issu_list_id=45

insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(229,'TPA loss run',0,8,1,getdate(),1)

insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(229,'Check Excess Loss Exhibit for Multiple Claims Per Occurrence',0,9,1,getdate(),1)

insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(229,'Aggregate/ Combined Elements breached',0,10,1,getdate(),1)

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=11,issu_txt='Paid Loss programs/ Paid Loss programs converting- related deductible programs in LSI or manual TPA'
where qlty_cntrl_mstr_issu_list_id=49

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=12,issu_txt='Manual worksheet completed'
where qlty_cntrl_mstr_issu_list_id=46

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=13,issu_txt='For Retro adjustment- check CESAR "14", "16" or "22" screens for audit revisions'
where qlty_cntrl_mstr_issu_list_id=47



update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='* Retro Elements: Minimum' where qlty_cntrl_mstr_issu_list_id=28

update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='* Audit: Confirm Policy #s on Audit' where qlty_cntrl_mstr_issu_list_id=38

update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='		Check CESAR "01" screen for a "1" in Retro and "800" Producer number' where qlty_cntrl_mstr_issu_list_id=37

update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='Confirm that all states with losses have premium on the audit' where qlty_cntrl_mstr_issu_list_id=39


insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(229,'For Retro adjustment- check Surcharges calculation if Min/ Max applies',0,14,1,getdate(),1)

insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(229,'For LRF adjustment- check TPDs postings',0,15,1,getdate(),1)

insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(229,'For LRF adjustment- check Texas Tax calculation',0,16,1,getdate(),1)

insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(229,'For LRF adjustment- check LDF/ IBNR included in the limit calculation (manual worksheet)',0,17,1,getdate(),1)

insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(229,'For Subguard adjustment- is this an interim adjustment',0,18,1,getdate(),1)

insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(229,'If interim, is ILR included?',0,19,1,getdate(),1)



*****************************************************************************************************************************
*******************************************************************************************************************************

update QLTY_CNTRL_MSTR_ISSU_LIST set actv_ind=0,srt_nbr=null where qlty_cntrl_mstr_issu_list_id=51

update QLTY_CNTRL_MSTR_ISSU_LIST set actv_ind=0,srt_nbr=null where qlty_cntrl_mstr_issu_list_id=52

update QLTY_CNTRL_MSTR_ISSU_LIST set actv_ind=0,srt_nbr=null where qlty_cntrl_mstr_issu_list_id=53

update QLTY_CNTRL_MSTR_ISSU_LIST set actv_ind=0,srt_nbr=null where qlty_cntrl_mstr_issu_list_id=56

update QLTY_CNTRL_MSTR_ISSU_LIST set actv_ind=0,srt_nbr=null where qlty_cntrl_mstr_issu_list_id=57

--------------------------------------------------------------------------------------------------------

update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=1
where qlty_cntrl_mstr_issu_list_id=54


update QLTY_CNTRL_MSTR_ISSU_LIST set  srt_nbr=2
where qlty_cntrl_mstr_issu_list_id=55

insert into QLTY_CNTRL_MSTR_ISSU_LIST(issu_catg_id,issu_txt,finc_ind,srt_nbr,crte_user_id,crte_dt,actv_ind)
values(230,'For Internal Only adjustments- send the entire adjustment to LSS Manager for approval',0,3,1,getdate(),1)


Update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt=cast(' ' as CHAR(12))+issu_txt where qlty_cntrl_mstr_issu_list_id in (22,101,102,121,122,45,123,124,125,116,49,46,
117,47,126,26,118,127,128,29,129,130,119,131,39,40,41,42)


Update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt=LTRIM(rtrim(issu_txt)) WHERE issu_catg_id = 229 and actv_ind=1

Update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='*'+issu_txt WHERE issu_catg_id = 229 and actv_ind=1



Update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt=LTRIM(rtrim(issu_txt)) WHERE 
qlty_cntrl_mstr_issu_list_id in(22,116,117,26,118,29,119,37)

Update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt=cast(' ' as CHAR(11))+issu_txt
WHERE qlty_cntrl_mstr_issu_list_id in(22,116,117,26,118,29,119,37) 


Update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='Check CESAR "01" screen for a "1" in Retro and "800" Producer number'
WHERE qlty_cntrl_mstr_issu_list_id in(37) 



Update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt=cast(' ' as CHAR(12))+issu_txt
WHERE qlty_cntrl_mstr_issu_list_id in(37) 



update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='           No. of months until conversion date', srt_nbr=6,actv_ind=1
where qlty_cntrl_mstr_issu_list_id=36



update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='Send the entire adjustment to the Underwriter for approval'
where qlty_cntrl_mstr_issu_list_id=54



update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='           Heading - Insured Name and Type of Program'
where qlty_cntrl_mstr_issu_list_id=22


update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='           LBAs (States and Rates)'
where qlty_cntrl_mstr_issu_list_id=33




update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='No Underwriter approval required'
where qlty_cntrl_mstr_issu_list_id=55

update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='           Policy Numbers (Check adjustment and loss runs)'
where qlty_cntrl_mstr_issu_list_id=24


update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='* Audit: Confirm Policy numbers on Audit'
where qlty_cntrl_mstr_issu_list_id=38


update QLTY_CNTRL_MSTR_ISSU_LIST set issu_txt='	    Loss Limits and ALAE Handling'
where qlty_cntrl_mstr_issu_list_id=25