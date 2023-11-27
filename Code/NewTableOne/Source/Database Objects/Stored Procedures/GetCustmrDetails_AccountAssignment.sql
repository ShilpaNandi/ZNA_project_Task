if exists (select 1 from sysobjects 
		where name = 'GetCustmrDetails_AccountAssignment' and type = 'P')
	drop procedure GetCustmrDetails_AccountAssignment
go

set ansi_nulls off
go


---------------------------------------------------------------------
-----
----- PROC NAME:  GetCustmrDetails_AccountAssignment
-----
----- VERSION:  SQL SERVER 2008
-----
----- AUTHOR :  CSC
-----
----- DESCRIPTION: Retrieves the result set for the custmoer details for account assignment
----
----- ON EXIT: 
-----   
-----
----- MODIFIED: 
-----   
----- 
---------------------------------------------------------------------

CREATE PROCEDURE [dbo].[GetCustmrDetails_AccountAssignment]
@custmr_id int=null,
@custmr_ids varchar(max)=null,
@buofficeid int=null,
@bp_ids varchar(max)=null,
@buname varchar(20)=null,
@buoffice varchar(20)=null,
@brokerid int =null,
@roleid int=null,
@userid int=null,
@bpnumber varchar(20)=null,
@acct_range varchar(20)=null,
@buoffice_range varchar(20)=null,
@broker_range varchar(20)=null
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
SET NOCOUNT ON;
BEGIN TRY
select distinct cust.custmr_id CUSTMR_ID,
cust.finc_pty_id BPNUMBER,
cust.full_nm CUSTMR_NAME,
pgmprd.bsn_unt_ofc_id BSN_UNT_OFC_ID,
pgmprd.brkr_id BRKR_ID,
extorg.full_name BROKERNAME,
intorg.bsn_unt_cd + '/' + intorg.city_nm BUSINESSUNITNAME
from PREM_ADJ_PGM pgmprd
inner join custmr cust on pgmprd.custmr_id=cust.custmr_id
left outer join CUSTMR_PERS_REL cpr on cust.custmr_id=cpr.custmr_id
left outer join INT_ORG intorg on pgmprd.bsn_unt_ofc_id = intorg.int_org_id
left outer join EXTRNL_ORG extorg on pgmprd.brkr_id = extorg.extrnl_org_id
where pgmprd.actv_ind = 1 and cust.actv_ind = 1
and cust.custmr_id = COALESCE(@custmr_id,cust.custmr_id)
and ( cust.custmr_id in (select items from fn_Getidsfromstring(@custmr_ids,',')) or @custmr_ids is null)
and pgmprd.bsn_unt_ofc_id = COALESCE(@buofficeid,pgmprd.bsn_unt_ofc_id)
and ( cust.finc_pty_id in (select items from fn_Getidsfromstring(@bp_ids,',')) or @bp_ids is null)
and intorg.bsn_unt_cd=COALESCE(@buname,intorg.bsn_unt_cd)
and intorg.city_nm=COALESCE(@buoffice,intorg.city_nm)
and pgmprd.brkr_id = COALESCE(@brokerid,pgmprd.brkr_id)
and cpr.rol_id = COALESCE(@roleid,cpr.rol_id) 
and cpr.pers_id = COALESCE(@userid,cpr.pers_id) 
and cust.finc_pty_id = COALESCE(@bpnumber,cust.finc_pty_id)
and cust.full_nm like COALESCE(@acct_range,cust.full_nm)
and intorg.city_nm like COALESCE(@buoffice_range,intorg.city_nm)
and extorg.full_name like COALESCE(@broker_range,extorg.full_name)
order by cust.full_nm asc
END TRY
BEGIN CATCH

 SELECT 
    ERROR_NUMBER() AS ERRORNUMBER,
    ERROR_SEVERITY() AS ERRORSEVERITY,
    ERROR_STATE() AS ERRORSTATE,
    ERROR_PROCEDURE() AS ERRORPROCEDURE,
    ERROR_LINE() AS ERRORLINE,
    ERROR_MESSAGE() AS ERRORMESSAGE;


END CATCH
END

GO

if object_id('GetCustmrDetails_AccountAssignment') is not null
	print 'Created Procedure GetCustmrDetails_AccountAssignment'
else
	print 'Failed Creating Procedure GetCustmrDetails_AccountAssignment'
go

if object_id('GetCustmrDetails_AccountAssignment') is not null
	grant exec on GetCustmrDetails_AccountAssignment to public
go
