---------------------------------------------------------------------
-----
-----	Script Name:	SR342087 - AIS Exhibit Changes Lkup Script
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
-----               02/15/10	Dheeraj Nadimpalli
-----				Created Procedure


----- TODO: 

---------------------------------------------------------------------

-- Take backup of LKUP and LKUP_TYP tables to temp table
select * into LKUP_10122012 from LKUP

select * into LKUP_TYP_10122012 from LKUP_TYP


-- 'Cumulative Totals Worksheet' to 'Paid Loss Billings for Current Adjustment Period'

update lkup
set lkup_txt='Paid Loss Billings for Current Adjustment Period'
where lkup_typ_id=26 and lkup_id=341


update INVC_EXHIBIT_SETUP
set atch_nm='Paid Loss Billings for Current Adjustment Period'
where invc_exhibit_setup_id=8


-- 'Escrow' to 'Loss Fund'

update lkup
set lkup_txt='Loss Fund'
where lkup_typ_id=26 and lkup_id=317

update lkup
set lkup_txt='LOSS FUND'
where lkup_typ_id=28 and lkup_id=455

update INVC_EXHIBIT_SETUP
set atch_nm='Loss Fund'
where invc_exhibit_setup_id=12

update lkup
set lkup_txt='LOSS FUND'
where lkup_typ_id=4 and lkup_id=62

update lkup
set lkup_txt='LOSS FUND'
where lkup_typ_id=46 and lkup_id=399

