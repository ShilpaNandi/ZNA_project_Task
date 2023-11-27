
IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'ModAISIBNRLDFExcessCalc' and TYPE = 'P')
	drop procedure ModAISIBNRLDFExcessCalc
go

set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:		ModAISIBNRLDFExcessCalc
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		CSC - 09/11/2008
-----
-----	Description:	This procedure will calculate INBR and LCF excess amounts
----                    The calculated amount is stored in exc_ldf_ibnr_amt field in ARMIS_LOS_EXC table and
----					then summed up in the ARMIS_LOS_POl table. 	The applied rules 
----                    are based on the ALAE type selected in the policy page and is only 
----					when "LDF/IBNR Incl Limit" indicator is checked. IBNR or LDF factor are 
-----					mutually exlusive, meaning only one factor is valid for a given plicy.
----					So the module is calculating an amount for either LDF or IBNR but not both.
----					The factor can also be stepped, which means that it can vary by date.
-----					This is indicated by the stepped factor indicator in the policy info (coml_agmt) page
-----					If this indicator is checked then the "stepped" factor for the current period
----					is found in the STEPPED_FCTR table.
-----					The excess amount is calculated by simply calculating the total amount(without a limit)
-----					and then subtracting the subject LDF/INBR amount calculated in 
-----					ModAISIBNRSubjCalc or ModAISLDFSubjCalc stored procs.
-----					This module is called by ModAISLossLimitExcess stored procedure
-----	Modified:		CSC - 12/11/2008
-----   Modified:       CSC - 07/13/2011
-----					As per the TFS 14210 Bug Fix,Added the where condition to filter the calculations based on the prem_adj_id is null or in CALC status.

---------------------------------------------------------------------
CREATE PROCEDURE [dbo].[ModAISIBNRLDFExcessCalc] 
@custmr_id int,
@prem_adj_pgm_id int
AS

declare @trancount int,
		@ldf_inbr_fctr_rt decimal(15,8),
		@paid_incur_typ_id int

set @trancount = @@trancount

if @trancount = 0 
	begin
	    begin transaction 
	end


begin try


select @paid_incur_typ_id = isnull(paid_incur_typ_id,0) 
from prem_adj_pgm 
where prem_adj_pgm_id = @prem_adj_pgm_id
and actv_ind = 1



--IBNR calculation

--IBNR:  exc_ldf_ibnr_amt: Included (77)
UPDATE armis_los_exc with(rowlock) 
SET exc_ldf_ibnr_amt =  (ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)) - ale.subj_ldf_ibnr_amt
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca
where ale.custmr_id = @custmr_id
and ale.armis_los_pol_id = alp.armis_los_pol_id
and alp.coml_agmt_id = ca.coml_agmt_id
and ca.aloc_los_adj_exps_typ_id IN (77)
and ((ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 0
and isnull(ca.los_dev_fctr_rt,0) = 0 )
or 
(ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 1
and dbo.fn_GetIBNRStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1)) 
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1 
and ((prem_adj_id is null) or (dbo.fn_GetAdjStatus(prem_adj_id) = 346))

--IBNR: exc_ldf_ibnr_amt: Excluded-Company Pays (75)
UPDATE armis_los_exc with(rowlock) 
SET exc_ldf_ibnr_amt =   ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)) - ale.subj_ldf_ibnr_amt
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca
where ale.custmr_id = @custmr_id
and ale.armis_los_pol_id = alp.armis_los_pol_id
and alp.coml_agmt_id = ca.coml_agmt_id
and ca.aloc_los_adj_exps_typ_id IN (75)
and ((ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 0
and isnull(ca.los_dev_fctr_rt,0) = 0 )
or 
(ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 1
and dbo.fn_GetIBNRStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1)) 
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1 
and ((prem_adj_id is null) or (dbo.fn_GetAdjStatus(prem_adj_id) = 346))

--IBNR:  exc_ldf_ibnr_amt: Excluded – Insured Pays (76)
UPDATE armis_los_exc with(rowlock) 
SET exc_ldf_ibnr_amt =  (ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)) - ale.subj_ldf_ibnr_amt
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca
where ale.custmr_id = @custmr_id
and ale.armis_los_pol_id = alp.armis_los_pol_id
and alp.coml_agmt_id = ca.coml_agmt_id
and ca.aloc_los_adj_exps_typ_id IN (76)
and ((ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 0
and isnull(ca.los_dev_fctr_rt,0) = 0 )
or 
(ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 1
and dbo.fn_GetIBNRStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1))  
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1 
and ((prem_adj_id is null) or (dbo.fn_GetAdjStatus(prem_adj_id) = 346))

--IBNR:  exc_ldf_ibnr_amt: Pro Rata (78)
UPDATE armis_los_exc with(rowlock) 
SET exc_ldf_ibnr_amt =  (ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)) - ale.subj_ldf_ibnr_amt
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca
where ale.custmr_id = @custmr_id
and ale.armis_los_pol_id = alp.armis_los_pol_id
and alp.coml_agmt_id = ca.coml_agmt_id
and ca.aloc_los_adj_exps_typ_id IN (78)
and ((ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 0
and isnull(ca.los_dev_fctr_rt,0) = 0 )
or 
(ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 1
and dbo.fn_GetIBNRStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1)) 
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1 
and ((prem_adj_id is null) or (dbo.fn_GetAdjStatus(prem_adj_id) = 346))

--IBNR:  exc_ldf_ibnr_amt: ALAE Included (79)
UPDATE armis_los_exc with(rowlock) 
SET exc_ldf_ibnr_amt =  (ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)) - ale.subj_ldf_ibnr_amt
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca
where ale.custmr_id = @custmr_id
and ale.armis_los_pol_id = alp.armis_los_pol_id
and alp.coml_agmt_id = ca.coml_agmt_id
and ca.aloc_los_adj_exps_typ_id IN (79)
and ((ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 0
and isnull(ca.los_dev_fctr_rt,0) = 0 )
or 
(ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 1
and dbo.fn_GetIBNRStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1)) 
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1 
and ((prem_adj_id is null) or (dbo.fn_GetAdjStatus(prem_adj_id) = 346))

--IBNR: exc_ldf_ibnr_amt: Pro Rata - Paid (80)
UPDATE armis_los_exc with(rowlock) 
SET exc_ldf_ibnr_amt =  (ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)) - ale.subj_ldf_ibnr_amt
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca
where ale.custmr_id = @custmr_id
and ale.armis_los_pol_id = alp.armis_los_pol_id
and alp.coml_agmt_id = ca.coml_agmt_id
and ca.aloc_los_adj_exps_typ_id IN (80)
and ((ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 0
and isnull(ca.los_dev_fctr_rt,0) = 0 )
or 
(ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 1
and dbo.fn_GetIBNRStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1)) 
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1 
and ((prem_adj_id is null) or (dbo.fn_GetAdjStatus(prem_adj_id) = 346))


--IBNR: exc_ldf_ibnr_amt: Capped ALAE (74)
UPDATE armis_los_exc with(rowlock) 
SET exc_ldf_ibnr_amt =  (ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)) - ale.subj_ldf_ibnr_amt
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca
where ale.custmr_id = @custmr_id
and ale.armis_los_pol_id = alp.armis_los_pol_id
and alp.coml_agmt_id = ca.coml_agmt_id
and ca.aloc_los_adj_exps_typ_id IN (74)
and ((ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 0
and isnull(ca.los_dev_fctr_rt,0) = 0 )
or 
(ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 1
and dbo.fn_GetIBNRStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1))  
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1 
and ((prem_adj_id is null) or (dbo.fn_GetAdjStatus(prem_adj_id) = 346))

---------------------------------------------------------------------------------------------
--LDF calculation
---------------------------------------------------------------------------------------------

--LDF exc_ldf_ibnr_amt: Included (77)
UPDATE armis_los_exc with(rowlock) 
SET exc_ldf_ibnr_amt =  (ale.subj_paid_idnmty_amt+ale.subj_resrvd_idnmty_amt+ ale.subj_paid_exps_amt+ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)) - ale.subj_ldf_ibnr_amt
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca
where ale.custmr_id = @custmr_id
and ale.armis_los_pol_id = alp.armis_los_pol_id
and alp.coml_agmt_id = ca.coml_agmt_id
and ca.aloc_los_adj_exps_typ_id IN (77)
and ((ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 0
and isnull(ca.los_dev_fctr_rt,0) > 0 )
or 
(ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 1
and dbo.fn_GetLDFStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1)) 
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1 
and ((prem_adj_id is null) or (dbo.fn_GetAdjStatus(prem_adj_id) = 346))

--LDF exc_ldf_ibnr_amt: Excluded-Company Pays (75)
UPDATE armis_los_exc with(rowlock) 
SET exc_ldf_ibnr_amt =  (ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)) - ale.subj_ldf_ibnr_amt
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca
where ale.custmr_id = @custmr_id
and ale.armis_los_pol_id = alp.armis_los_pol_id
and alp.coml_agmt_id = ca.coml_agmt_id
and ca.aloc_los_adj_exps_typ_id IN (75)
and ((ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 0
and isnull(ca.los_dev_fctr_rt,0) > 0 )
or 
(ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 1
and dbo.fn_GetLDFStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1)) 
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1 
and ((prem_adj_id is null) or (dbo.fn_GetAdjStatus(prem_adj_id) = 346))

--LDF exc_ldf_ibnr_amt: Excluded – Insured Pays (76)
UPDATE armis_los_exc with(rowlock) 
SET exc_ldf_ibnr_amt =  (ale.subj_paid_idnmty_amt+ale.subj_resrvd_idnmty_amt+ ale.subj_paid_exps_amt+ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)) - ale.subj_ldf_ibnr_amt
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca
where ale.custmr_id = @custmr_id
and ale.armis_los_pol_id = alp.armis_los_pol_id
and alp.coml_agmt_id = ca.coml_agmt_id
and ca.aloc_los_adj_exps_typ_id IN (76)
and ((ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 0
and isnull(ca.los_dev_fctr_rt,0) > 0 )
or 
(ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 1
and dbo.fn_GetLDFStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1)) 
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1 
and ((prem_adj_id is null) or (dbo.fn_GetAdjStatus(prem_adj_id) = 346))

--LDF exc_ldf_ibnr_amt: Pro Rata (78)
UPDATE armis_los_exc with(rowlock) 
SET exc_ldf_ibnr_amt =  (ale.subj_paid_idnmty_amt+ale.subj_resrvd_idnmty_amt+ ale.subj_paid_exps_amt+ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)) - ale.subj_ldf_ibnr_amt
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca
where ale.custmr_id = @custmr_id
and ale.armis_los_pol_id = alp.armis_los_pol_id
and alp.coml_agmt_id = ca.coml_agmt_id
and ca.aloc_los_adj_exps_typ_id IN (78)
and ((ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 0
and isnull(ca.los_dev_fctr_rt,0) > 0 )
or 
(ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 1
and dbo.fn_GetLDFStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1)) 
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1 
and ((prem_adj_id is null) or (dbo.fn_GetAdjStatus(prem_adj_id) = 346))

--LDF exc_ldf_ibnr_amt: ALAE Included (79)
UPDATE armis_los_exc with(rowlock) 
SET exc_ldf_ibnr_amt =  (ale.subj_paid_idnmty_amt+ale.subj_resrvd_idnmty_amt+ ale.subj_paid_exps_amt+ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)) - ale.subj_ldf_ibnr_amt
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca
where ale.custmr_id = @custmr_id
and ale.armis_los_pol_id = alp.armis_los_pol_id
and alp.coml_agmt_id = ca.coml_agmt_id
and ca.aloc_los_adj_exps_typ_id IN (79)
and ((ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 0
and isnull(ca.los_dev_fctr_rt,0) > 0 )
or 
(ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 1
and dbo.fn_GetLDFStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1)) 
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1 
and ((prem_adj_id is null) or (dbo.fn_GetAdjStatus(prem_adj_id) = 346))

--LDF exc_ldf_ibnr_amt: Pro Rata - Paid (80)
UPDATE armis_los_exc with(rowlock) 
SET exc_ldf_ibnr_amt =  (ale.subj_paid_idnmty_amt+ale.subj_resrvd_idnmty_amt+ ale.subj_paid_exps_amt+ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)) - ale.subj_ldf_ibnr_amt
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca
where ale.custmr_id = @custmr_id
and ale.armis_los_pol_id = alp.armis_los_pol_id
and alp.coml_agmt_id = ca.coml_agmt_id
and ca.aloc_los_adj_exps_typ_id IN (80)
and ((ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 0
and isnull(ca.los_dev_fctr_rt,0) > 0 )
or 
(ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 1
and dbo.fn_GetLDFStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1)) 
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1 
and ((prem_adj_id is null) or (dbo.fn_GetAdjStatus(prem_adj_id) = 346))

--LDF exc_ldf_ibnr_amt: Capped ALAE (74)
UPDATE armis_los_exc with(rowlock) 
SET exc_ldf_ibnr_amt =  (ale.subj_paid_idnmty_amt+ale.subj_resrvd_idnmty_amt+ ale.subj_paid_exps_amt+ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)) - ale.subj_ldf_ibnr_amt
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca
where ale.custmr_id = @custmr_id
and ale.armis_los_pol_id = alp.armis_los_pol_id
and alp.coml_agmt_id = ca.coml_agmt_id
and ca.aloc_los_adj_exps_typ_id IN (74)
and ((ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 0
and isnull(ca.los_dev_fctr_rt,0) > 0 )
or 
(ca.los_dev_fctr_incur_but_not_rptd_incld_lim_ind = 1
and isnull(los_dev_fctr_incur_but_not_rptd_step_ind,0) = 1
and dbo.fn_GetLDFStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1)) 
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1 
and ((prem_adj_id is null) or (dbo.fn_GetAdjStatus(prem_adj_id) = 346))


if @trancount = 0
	commit transaction 

end try
begin catch

	if @trancount = 0  
	begin
		rollback transaction
	end
	
	declare @err_msg varchar(500),
			@err_sev varchar(10),
			@err_no varchar(10)

	--select 
	--error_number() AS ErrorNumber,
	--error_severity() AS ErrorSeverity,
	--error_state() as ErrorState,
	--error_procedure() as ErrorProcedure,
	--error_line() as ErrorLine,
	--error_message() as ErrorMessage
	
	select  @err_msg = error_message(),
		    @err_no = error_number(),
			@err_sev = error_severity()

	RAISERROR (@err_msg, -- Message text.
               @err_sev, -- Severity.
               1 -- State.
               )
               
end catch

go

if object_id('ModAISIBNRLDFExcessCalc') is not null
	print 'Created Procedure ModAISIBNRLDFExcessCalc'
else
	print 'Failed Creating Procedure ModAISIBNRLDFExcessCalc'
go

if object_id('ModAISIBNRLDFExcessCalc') is not null
	grant exec on ModAISIBNRLDFExcessCalc to public
go





