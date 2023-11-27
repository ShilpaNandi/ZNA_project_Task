
IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'GetAcctMgmtAdjDtls1' and TYPE = 'P')
	DROP PROC GetAcctMgmtAdjDtls1

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	PROC NAME:		GetAcctMgmtAdjDtls1
-----
-----	VERSION:		SQL SERVER 2005
-----
-----	AUTHOR :		CSC
-----
-----	DESCRIPTION:	Retrieves the result set for the Adjustment Invoicing Dashboard web page
-----			Status columns.
----
-----	ON EXIT:	
-----			
-----
-----	MODIFIED:	
-----			
----- 
---------------------------------------------------------------------

CREATE PROCEDURE [dbo].[GetAcctMgmtAdjDtls1]
@pers_id int,
@custmr_id int=0,
@adj_sts_typ_id int=0

AS
BEGIN

-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
SET NOCOUNT ON;

BEGIN TRY
-- Only pers_id was passed
if @pers_id > 0 and @custmr_id=0
begin
-- First part of union gets master and regular accounts
-- Second part of union retrieves sub accounts
	select 
	premadj.prem_adj_id,
	premadj.reg_custmr_id,
	cust.full_nm as custmr_name,
	cust.custmr_id as child_custmr_id,
	cust.full_nm   as child_custmr_name,
	cust.actv_ind as custmr_active_ind, -- unsure
	valn_dt,
	premadj.adj_sts_typ_id as adj_sts_typ_id,
	stat.lkup_txt as adj_sts_typ_lkup_txt,
	isnull(adj_qc_ind,0) as adj_qc_ind,
	isnull(reconciler_revw_ind, 0) as reconciler_revw_ind,
	premadj.adj_pendg_ind as adj_pendg_ind, 
	(select  top 1 prem_adj_pgm_id  
		from PREM_ADJ_PERD 
		where PREM_ADJ_PERD.prem_adj_id = premadj.prem_adj_id) as prem_adj_pgm_id,
	adj_sts_eff_dt,
	@pers_id as custmr_per_id
	from  PREM_ADJ premadj
	inner join LKUP as stat on (premadj.adj_sts_typ_id = stat.lkup_id)
	inner join CUSTMR as cust on (premadj.reg_custmr_id = cust.custmr_id)
	where (cust.mstr_acct_ind = 1 or isnull(cust.custmr_rel_actv_ind,0) = 0 ) and
		  cust.actv_ind = 1 and
		  premadj.adj_sts_typ_id not in (347, 349, 352) -- CANCELLED, FINAL-INVOICE, TRANSMITTED
	and premadj.reg_custmr_id in (select distinct custmr_id from custmr_pers_rel where pers_id = @pers_id)
union
	select 
	premadj.prem_adj_id,
	premadj.reg_custmr_id,
	(select full_nm from custmr where custmr_id = premadj.reg_custmr_id) as custmr_name,
	cust.custmr_id as child_custmr_id,
	cust.full_nm   as child_custmr_name,
	cust.actv_ind as custmr_active_ind, -- unsure
	valn_dt,
	premadj.adj_sts_typ_id as adj_sts_typ_id,
	stat.lkup_txt as adj_sts_typ_lkup_txt,
	isnull(adj_qc_ind,0) as adj_qc_ind,
	isnull(reconciler_revw_ind, 0) as reconciler_revw_ind,
	premadj.adj_pendg_ind as adj_pendg_ind, 
	(select  top 1 prem_adj_pgm_id  
		from PREM_ADJ_PERD 
		where PREM_ADJ_PERD.prem_adj_id = premadj.prem_adj_id
				and custmr_id = cust.custmr_id ) as prem_adj_pgm_id,
	adj_sts_eff_dt,
	@pers_id as custmr_per_id
	from  PREM_ADJ premadj
	inner join PREM_ADJ_PERD perd on (premadj.prem_adj_id = perd.prem_adj_id)
	inner join LKUP as stat on (premadj.adj_sts_typ_id = stat.lkup_id)
	inner join CUSTMR as cust on (perd.custmr_id = cust.custmr_id)
	where ( isnull(cust.custmr_rel_actv_ind,0) = 1 ) and
		  cust.actv_ind = 1 and
		  premadj.adj_sts_typ_id not in (347, 349, 352)  -- CANCELLED, FINAL-INVOICE, TRANSMITTED
	and premadj.reg_custmr_id in (select distinct custmr_id from custmr_pers_rel where pers_id = @pers_id)
	and perd.reg_custmr_id <> perd.custmr_id
	order by cust.full_nm, premadj.valn_dt
end

-- Persond Id and Customer ID were passed
if @pers_id > 0 and @custmr_id > 0
begin
-- First part of union gets master and regular accounts
-- Second part of union retrieves sub accounts
	select 
	premadj.prem_adj_id,
	premadj.reg_custmr_id,
	cust.full_nm as custmr_name,
	cust.custmr_id as child_custmr_id,
	cust.full_nm   as child_custmr_name,
	cust.actv_ind as custmr_active_ind, -- unsure
	valn_dt,
	premadj.adj_sts_typ_id as adj_sts_typ_id,
	stat.lkup_txt as adj_sts_typ_lkup_txt,
	isnull(adj_qc_ind,0) as adj_qc_ind,
	isnull(reconciler_revw_ind, 0) as reconciler_revw_ind,
	premadj.adj_pendg_ind as adj_pendg_ind, 
	(select  top 1 prem_adj_pgm_id  
		from PREM_ADJ_PERD 
		where PREM_ADJ_PERD.prem_adj_id = premadj.prem_adj_id) as prem_adj_pgm_id,
	adj_sts_eff_dt,
	@pers_id as custmr_per_id
	from  PREM_ADJ premadj
	inner join LKUP as stat on (premadj.adj_sts_typ_id = stat.lkup_id)
	inner join CUSTMR as cust on (premadj.reg_custmr_id = cust.custmr_id)
	where (cust.mstr_acct_ind = 1 or isnull(cust.custmr_rel_actv_ind,0) = 0 ) and
		  cust.actv_ind = 1 and
		  premadj.adj_sts_typ_id not in (347, 349, 352) -- CANCELLED, FINAL-INVOICE, TRANSMITTED
	and premadj.reg_custmr_id in (select distinct custmr_id from custmr_pers_rel where pers_id = @pers_id and custmr_id = @custmr_id)
	and premadj.reg_custmr_id = @custmr_id
	and cust.custmr_id = @custmr_id 
union
	select 
	premadj.prem_adj_id,
	premadj.reg_custmr_id,
	(select full_nm from custmr where custmr_id = premadj.reg_custmr_id) as custmr_name,
	cust.custmr_id as child_custmr_id,
	cust.full_nm   as child_custmr_name,
	cust.actv_ind as custmr_active_ind, -- unsure
	valn_dt,
	premadj.adj_sts_typ_id as adj_sts_typ_id,
	stat.lkup_txt as adj_sts_typ_lkup_txt,
	isnull(adj_qc_ind,0) as adj_qc_ind,
	isnull(reconciler_revw_ind, 0) as reconciler_revw_ind,
	premadj.adj_pendg_ind as adj_pendg_ind, 
	(select  top 1 prem_adj_pgm_id  
		from PREM_ADJ_PERD 
		where PREM_ADJ_PERD.prem_adj_id = premadj.prem_adj_id
				and custmr_id = cust.custmr_id ) as prem_adj_pgm_id,
	adj_sts_eff_dt,
	@pers_id as custmr_per_id
	from  PREM_ADJ premadj
	inner join PREM_ADJ_PERD perd on (premadj.prem_adj_id = perd.prem_adj_id)
	inner join LKUP as stat on (premadj.adj_sts_typ_id = stat.lkup_id)
	inner join CUSTMR as cust on (perd.custmr_id = cust.custmr_id)
	where ( isnull(cust.custmr_rel_actv_ind,0) = 1 ) and
		  cust.actv_ind = 1 and
		  premadj.adj_sts_typ_id not in (347, 349, 352)  -- CANCELLED, FINAL-INVOICE, TRANSMITTED
	and premadj.reg_custmr_id in (select distinct custmr_rel_id 
									from custmr_pers_rel , custmr
									where pers_id = @pers_id and custmr.custmr_id = @custmr_id
									and   custmr_pers_rel.custmr_id = custmr.custmr_id)
	and perd.reg_custmr_id <> perd.custmr_id
	and cust.custmr_id = @custmr_id 
	order by cust.full_nm, premadj.valn_dt
end
-- Only adjustment status was passed
if @adj_sts_typ_id > 0
begin
-- First part of union gets master and regular accounts
-- Second part of union retrieves sub accounts
	select 
	premadj.prem_adj_id,
	premadj.reg_custmr_id,
	cust.full_nm as custmr_name,
	cust.custmr_id as child_custmr_id,
	cust.full_nm   as child_custmr_name,
	cust.actv_ind as custmr_active_ind, -- unsure
	valn_dt,
	premadj.adj_sts_typ_id as adj_sts_typ_id,
	stat.lkup_txt as adj_sts_typ_lkup_txt,
	isnull(adj_qc_ind,0) as adj_qc_ind,
	isnull(reconciler_revw_ind, 0) as reconciler_revw_ind,
	premadj.adj_pendg_ind as adj_pendg_ind, 
	(select  top 1 prem_adj_pgm_id  
		from PREM_ADJ_PERD 
		where PREM_ADJ_PERD.prem_adj_id = premadj.prem_adj_id) as prem_adj_pgm_id,
	adj_sts_eff_dt,
	@pers_id as custmr_per_id
	from  PREM_ADJ premadj
	inner join LKUP as stat on (premadj.adj_sts_typ_id = stat.lkup_id)
	inner join CUSTMR as cust on (premadj.reg_custmr_id = cust.custmr_id)
	where (cust.mstr_acct_ind = 1 or isnull(cust.custmr_rel_actv_ind,0) = 0 ) and
		  cust.actv_ind = 1 and
		  premadj.adj_sts_typ_id not in (347, 349, 352) -- CANCELLED, FINAL-INVOICE, TRANSMITTED
    and premadj.adj_sts_typ_id = @adj_sts_typ_id
union
	select 
	premadj.prem_adj_id,
	premadj.reg_custmr_id,
	(select full_nm from custmr where custmr_id = premadj.reg_custmr_id) as custmr_name,
	cust.custmr_id as child_custmr_id,
	cust.full_nm   as child_custmr_name,
	cust.actv_ind as custmr_active_ind, -- unsure
	valn_dt,
	premadj.adj_sts_typ_id as adj_sts_typ_id,
	stat.lkup_txt as adj_sts_typ_lkup_txt,
	isnull(adj_qc_ind,0) as adj_qc_ind,
	isnull(reconciler_revw_ind, 0) as reconciler_revw_ind,
	premadj.adj_pendg_ind as adj_pendg_ind, 
	(select  top 1 prem_adj_pgm_id  
		from PREM_ADJ_PERD 
		where PREM_ADJ_PERD.prem_adj_id = premadj.prem_adj_id
				and custmr_id = cust.custmr_id ) as prem_adj_pgm_id,
	adj_sts_eff_dt,
	@pers_id as custmr_per_id
	from  PREM_ADJ premadj
	inner join PREM_ADJ_PERD perd on (premadj.prem_adj_id = perd.prem_adj_id)
	inner join LKUP as stat on (premadj.adj_sts_typ_id = stat.lkup_id)
	inner join CUSTMR as cust on (perd.custmr_id = cust.custmr_id)
	where ( isnull(cust.custmr_rel_actv_ind,0) = 1 ) and
		  cust.actv_ind = 1 and
		  premadj.adj_sts_typ_id not in (347, 349, 352)  -- CANCELLED, FINAL-INVOICE, TRANSMITTED
    and premadj.adj_sts_typ_id = @adj_sts_typ_id
	order by cust.full_nm, premadj.valn_dt
end



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

IF OBJECT_ID('GetAcctMgmtAdjDtls1') IS NOT NULL
	PRINT 'CREATED PROCEDURE GetAcctMgmtAdjDtls1'
ELSE
	PRINT 'FAILED CREATING PROCEDURE GetAcctMgmtAdjDtls1'
GO

IF OBJECT_ID('GetAcctMgmtAdjDtls1') IS NOT NULL
	GRANT EXEC ON GetAcctMgmtAdjDtls1 TO PUBLIC
GO
