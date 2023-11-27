---------------------------------------------------------------------
-----
-----	Script Name:	PLB Indicator update on lsi_custmr Script
-----
-----	Version:	SQL Server 2012
-----
-----	Description:	If the account has any active paid loss retro policies set up, or any loss fund (escrow) policies set up, and an account already listed in the "Related -----	LSI Accounts"  tab, then please check the plb box
-----
-----	On Exit:	
-----			
-----
-----	Modified:	MM/DD/YY	first & last name of modifier
-----			- Description of Modification
-----				05/11/15  Dheeraj Nadimpalli

----- TODO: 

---------------------------------------------------------------------

--Select Query before update

select distinct lsi_custmr.custmr_id,plb_ind from lsi_custmr 
inner join coml_agmt on lsi_custmr.custmr_id=coml_agmt.custmr_id where adj_typ_id in (62,71)

-- Update Query

update lsi_custmr set plb_ind=1 where custmr_id in (select distinct lsi_custmr.custmr_id from lsi_custmr 
inner join coml_agmt on lsi_custmr.custmr_id=coml_agmt.custmr_id where adj_typ_id in (62,71))

-- Select query after update

select distinct lsi_custmr.custmr_id,plb_ind from lsi_custmr 
inner join coml_agmt on lsi_custmr.custmr_id=coml_agmt.custmr_id where adj_typ_id in (62,71)