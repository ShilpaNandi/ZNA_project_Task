
IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'GetAdjInvcDshbrd' and TYPE = 'P')
	DROP PROC GetAdjInvcDshbrd

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	PROC NAME:		GetAdjInvcDshbrd
-----
-----	VERSION:		SQL SERVER 2005
-----
-----	AUTHOR :		CSC
-----
-----	DESCRIPTION:	Retrieves the result set for the Adjustment Invoicing Dashboard web page
-----					Stored Proc performs an outer join from the PREM_ADJ_PGM table to PREM_ADJ_PERD table
-----					to retrieve program periods which do not have an adjustment linked to them
-----					or its adjustment is in "CALC", "DRAFT-INVOICE", "QC-DRAFT INVOICE", "RECON REVIEW"
-----					or "UW REVIEW" status.  
-----					If a program period is not linked to an adjustment, it will be listed in the 
-----					Adjustment Invoicing Dashboard page with blank Adj No, Adj Status, Calc Adj
----					Status columns.
----					If a program period is linked to an adjustment and its in one of the above statuses, 
----					then only program periods with adjustment in "CALC" status will be displayed in Adjustment 
----					Invoicing Dashboard page with Adj No, Adj Status, Calc Adj Status columns populated. 
----					This specific filtering is perfomed in the web application page
----
----					If a program period has adjustments which are only in "FINAL-INVOICE", "TRANSMITTED",
----					or "CANCELLED" status then the program period will be treated just like a 
----					program period which is not linked to any adjustment and will be displayed 
----					in the Adjustment Invoicing Dashboard page with blank Adj No, Adj Status, Calc Adj
----					Status columns.
----
-----	ON EXIT:	
-----			
-----
-----	MODIFIED:	
-----			
----- 
---------------------------------------------------------------------

CREATE PROCEDURE [dbo].[GetAdjInvcDshbrd]
@reg_custmr_id INT

AS
BEGIN

-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
SET NOCOUNT ON;

BEGIN TRY
-- Retrieve program periods which have the same non-premium and premium valuation dates


select 
'B',
nxt_valn_dt as valn_dt, 
nxt_valn_dt as nxt_valn_dt, 
nxt_valn_dt as NXT_VALN_DT_PREM,
nxt_valn_dt_non_prem_dt, 
strt_dt, 
plan_end_dt, 
pag.prem_adj_pgm_id, 
cust.full_nm as CustFullName,
pag.custmr_id, 
valn_mm_dt,
pag.brkr_id, 
extrnl_org.full_name as BrokerName,
pag.bsn_unt_ofc_id, 
isnull(int_org.full_name, ' ') as BUFullName,
isnull(int_org.city_nm, ' ') as  city_nm,
isnull(pap.prem_adj_perd_id,0) as prem_adj_perd_id,
pgm_typ_id,
--isnull(pa.prem_adj_id, 0) as prem_adj_id,
--isnull(calc_adj_sts_cd, ' ') as calc_adj_sts_cd,
isnull ( pap.prem_adj_id ,  0 )   as  prem_adj_id ,
( select   isnull ( calc_adj_sts_cd , ' ' )   from  prem_adj where  prem_adj . prem_adj_id = pap . prem_adj_id )   as  calc_adj_sts_cd ,
prem_adj_pgm_sts_id,
pgm_perd_sts_typ_id,
(select count(LSI.Custmr_id) from LSI_Custmr LSI where LSI.Custmr_id=pag.Custmr_id) as LSI_Custmr_Count,
(select count(PAPS.prem_adj_pgm_setup_id) from PREM_ADJ_PGM_SETUP PAPS where PAPS.prem_adj_pgm_id=pag.prem_adj_pgm_id and adj_parmet_typ_id=400) as ILRF_Setup_Count
from prem_adj_pgm pag
inner JOIN PREM_ADJ_PGM_STS PAPS ON 
( PAPS.prem_adj_pgm_id = PAG.prem_adj_pgm_id
AND pgm_perd_sts_typ_id = 342 AND STS_CHK_IND = 1)
inner JOIN CUSTMR cust ON (pag.custmr_id = cust.custmr_id)

LEFT OUTER JOIN  PREM_ADJ_PERD PAP  on 
(pag.prem_adj_pgm_id = pap.prem_adj_pgm_id
and pap.prem_non_prem_cd = 'B'
and pap.prem_adj_id = (
select pa.prem_adj_id from prem_adj pa where 
	pap.prem_adj_id = pa.prem_adj_id
	and (isnull(pa.adj_void_ind,0) = 0)
	and (isnull(pa.adj_can_ind,0) = 0)
	and (isnull(pa.adj_rrsn_ind,0) = 0)
	and substring(ltrim(isnull(fnl_invc_nbr_txt, '   ')), 1, 3) <> 'RTV'
	and adj_sts_typ_id  in (346, 348,350,351,353) -- CALC, DRAFT-INVOICE, QC-DRAFT INVOICE, RECON REVIEW, UW REVIEW
))
inner  join extrnl_org   on (pag.brkr_id = extrnl_org.extrnl_org_id)
inner  join int_org   on (pag.bsn_unt_ofc_id = int_org.int_org_id)
where (pag.custmr_id = @reg_custmr_id
OR PAG.CUSTMR_ID IN (SELECT CUSTMR_ID FROM CUSTMR WHERE CUSTMR_REL_ID = @reg_custmr_id and custmr_rel_actv_ind=1))
and pag.nxt_valn_dt = pag.nxt_valn_dt_non_prem_dt
and pag.actv_ind = 1
and pag.nxt_valn_dt <=
case when pag.fnl_adj_dt <> null then 
pag.fnl_adj_dt else  dateadd(mm,1,pag.nxt_valn_dt) end
and pag.nxt_valn_dt_non_prem_dt <=
case when pag.fnl_adj_non_prem_dt <> null then 
pag.fnl_adj_non_prem_dt else  dateadd(mm,1,pag.nxt_valn_dt_non_prem_dt) end

-- Return Premium valuation date for program periods where premium and non-premium valuation dates are different
union
select  
'P',
nxt_valn_dt as valn_dt, 
nxt_valn_dt as nxt_valn_dt, 
nxt_valn_dt as NXT_VALN_DT_PREM,
nxt_valn_dt_non_prem_dt, 
strt_dt, 
plan_end_dt, 
pag.prem_adj_pgm_id, 
cust.full_nm as CustFullName,
pag.custmr_id, 
valn_mm_dt,
pag.brkr_id, 
extrnl_org.full_name as BrokerName,
pag.bsn_unt_ofc_id, 
isnull(int_org.full_name, ' ') as BUFullName,
isnull(int_org.city_nm, ' ') as  city_nm,
isnull(pap.prem_adj_perd_id,0) as prem_adj_perd_id,
pgm_typ_id,
--isnull(pa.prem_adj_id, 0) as prem_adj_id,
--isnull(calc_adj_sts_cd, ' ') as calc_adj_sts_cd,
isnull ( pap.prem_adj_id ,  0 )   as  prem_adj_id ,
( select   isnull ( calc_adj_sts_cd , ' ' )   from  prem_adj where  prem_adj . prem_adj_id = pap . prem_adj_id )   as  calc_adj_sts_cd ,
prem_adj_pgm_sts_id,
pgm_perd_sts_typ_id,
(select count(LSI.Custmr_id) from LSI_Custmr LSI where LSI.Custmr_id=pag.Custmr_id) as LSI_Custmr_Count,
(select count(PAPS.prem_adj_pgm_setup_id) from PREM_ADJ_PGM_SETUP PAPS where PAPS.prem_adj_pgm_id=pag.prem_adj_pgm_id and adj_parmet_typ_id=400) as ILRF_Setup_Count

from prem_adj_pgm pag
inner JOIN PREM_ADJ_PGM_STS PAPS ON 
( PAPS.prem_adj_pgm_id = PAG.prem_adj_pgm_id
AND pgm_perd_sts_typ_id = 342 AND STS_CHK_IND = 1)
inner JOIN CUSTMR cust ON (pag.custmr_id = cust.custmr_id)

LEFT OUTER JOIN  PREM_ADJ_PERD PAP  on 
(pag.prem_adj_pgm_id = pap.prem_adj_pgm_id
and pap.prem_non_prem_cd = 'P'
and pap.prem_adj_id = (
select pa.prem_adj_id from prem_adj pa where 
	pap.prem_adj_id = pa.prem_adj_id
	and (isnull(pa.adj_void_ind,0) = 0)
	and (isnull(pa.adj_can_ind,0) = 0)
	and (isnull(pa.adj_rrsn_ind,0) = 0)
	and substring(ltrim(isnull(fnl_invc_nbr_txt, '   ')), 1, 3) <> 'RTV'
	and adj_sts_typ_id  in (346, 348,350,351,353) -- CALC, DRAFT-INVOICE, QC-DRAFT INVOICE, RECON REVIEW, UW REVIEW
))

inner  join extrnl_org   on (pag.brkr_id = extrnl_org.extrnl_org_id)
inner  join int_org   on (pag.bsn_unt_ofc_id = int_org.int_org_id)
where (pag.custmr_id = @reg_custmr_id
OR PAG.CUSTMR_ID IN (SELECT CUSTMR_ID FROM CUSTMR WHERE CUSTMR_REL_ID = @reg_custmr_id and custmr_rel_actv_ind=1))
and pag.nxt_valn_dt <> pag.nxt_valn_dt_non_prem_dt
and pag.nxt_valn_dt <= nxt_valn_dt_non_prem_dt
and pag.actv_ind = 1
and (isnull(pap.prem_non_prem_cd,'P') = 'P')
and pag.nxt_valn_dt <= 
case when pag.fnl_adj_dt <> null then 
pag.fnl_adj_dt else  dateadd(mm,1,pag.nxt_valn_dt) end

-- Return non-premium valuation date for program periods where premium and non-premium valuation dates are different
-- Only returned if adjustment is linked to NP val date
union
select 
'NP',
nxt_valn_dt_non_prem_dt as valn_dt, 
nxt_valn_dt_non_prem_dt as nxt_valn_dt,
nxt_valn_dt as NXT_VALN_DT_PREM,
nxt_valn_dt_non_prem_dt,  
strt_dt, 
plan_end_dt, 
pag.prem_adj_pgm_id, 
cust.full_nm as CustFullName,
pag.custmr_id, 
valn_mm_dt,
pag.brkr_id, 
extrnl_org.full_name as BrokerName,
pag.bsn_unt_ofc_id, 
isnull(int_org.full_name, ' ') as BUFullName,
isnull(int_org.city_nm, ' ') as  city_nm,
isnull(pap.prem_adj_perd_id,0) as prem_adj_perd_id,
pgm_typ_id,
--isnull(pa.prem_adj_id, 0) as prem_adj_id,
--isnull(calc_adj_sts_cd, ' ') as calc_adj_sts_cd,
isnull ( pap.prem_adj_id ,  0 )   as  prem_adj_id ,
( select   isnull ( calc_adj_sts_cd , ' ' )   from  prem_adj where  prem_adj . prem_adj_id = pap . prem_adj_id )   as  calc_adj_sts_cd ,
prem_adj_pgm_sts_id,
pgm_perd_sts_typ_id,
(select count(LSI.Custmr_id) from LSI_Custmr LSI where LSI.Custmr_id=pag.Custmr_id) as LSI_Custmr_Count,
(select count(PAPS.prem_adj_pgm_setup_id) from PREM_ADJ_PGM_SETUP PAPS where PAPS.prem_adj_pgm_id=pag.prem_adj_pgm_id and adj_parmet_typ_id=400) as ILRF_Setup_Count

from prem_adj_pgm pag
inner JOIN PREM_ADJ_PGM_STS PAPS ON 
( PAPS.prem_adj_pgm_id = PAG.prem_adj_pgm_id
AND pgm_perd_sts_typ_id = 342 AND STS_CHK_IND = 1)
inner JOIN CUSTMR cust ON (pag.custmr_id = cust.custmr_id)

Left outer JOIN  PREM_ADJ_PERD PAP  on 
(pag.prem_adj_pgm_id = pap.prem_adj_pgm_id
and pap.prem_non_prem_cd = 'NP'
and pap.prem_adj_id = (
select pa.prem_adj_id from prem_adj pa where 
	pap.prem_adj_id = pa.prem_adj_id
	and (isnull(pa.adj_void_ind,0) = 0)
	and (isnull(pa.adj_can_ind,0) = 0)
	and (isnull(pa.adj_rrsn_ind,0) = 0)
	and substring(ltrim(isnull(fnl_invc_nbr_txt, '   ')), 1, 3) <> 'RTV'
	and adj_sts_typ_id  in (346, 348,350,351,353) -- CALC, DRAFT-INVOICE, QC-DRAFT INVOICE, RECON REVIEW, UW REVIEW
))

inner  join extrnl_org   on (pag.brkr_id = extrnl_org.extrnl_org_id)
inner  join int_org   on (pag.bsn_unt_ofc_id = int_org.int_org_id)
where (pag.custmr_id = @reg_custmr_id
OR PAG.CUSTMR_ID IN (SELECT CUSTMR_ID FROM CUSTMR WHERE CUSTMR_REL_ID = @reg_custmr_id and custmr_rel_actv_ind=1))
and pag.nxt_valn_dt <> pag.nxt_valn_dt_non_prem_dt
and pag.nxt_valn_dt_non_prem_dt <= nxt_valn_dt
and pag.actv_ind = 1
--and pap.prem_non_prem_cd = 'NP'
and (isnull(pap.prem_non_prem_cd,'NP') = 'NP')
and pag.nxt_valn_dt_non_prem_dt <=
case when pag.fnl_adj_non_prem_dt is not null then 
pag.fnl_adj_non_prem_dt else  dateadd(mm,1,pag.nxt_valn_dt_non_prem_dt) end



ORDER BY CustFullName,  STRT_DT desc

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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

IF OBJECT_ID('GetAdjInvcDshbrd') IS NOT NULL
	PRINT 'CREATED PROCEDURE GetAdjInvcDshbrd'
ELSE
	PRINT 'FAILED CREATING PROCEDURE GetAdjInvcDshbrd'
GO

IF OBJECT_ID('GetAdjInvcDshbrd') IS NOT NULL
	GRANT EXEC ON GetAdjInvcDshbrd TO PUBLIC
GO
