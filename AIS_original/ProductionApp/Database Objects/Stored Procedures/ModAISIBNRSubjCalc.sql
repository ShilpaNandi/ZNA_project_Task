 
IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'ModAISIBNRSubjCalc' and TYPE = 'P')
	drop procedure ModAISIBNRSubjCalc
go

set ansi_nulls off
go

---------------------------------------------------------------------
-----
-----	Proc Name:		ModAISIBNRSubjCalc
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		CSC - 09/11/2008
-----
-----	Description:	This procedure will calculate LDF subject amounts related to the 
----                    the LDF factor set in the Policy info page. The calculated amount is 
----					stored in subj_ldf_ibnr_amt field in ARMIS_LOS_EXC table.  This amount
----					is used in the calculation of the excess IBNR/LDF amount.
----					The applied rules 
----                    are based on the ALAE type selected in the policy page and is only 
----					when "LDF/IBNR Incl Limit" indicator is checked. IBNR or LDF factor are 
-----					mutually exlusive, meaning only one factor is valid for a given plicy.
----					So the module is calculating an amount for either LDF or IBNR but not both.
----					The factor can also be stepped, which means that it can vary by date.
-----					This is indicated by the stepped factor indicator in the policy info (coml_agmt) page
-----					If this indicator is checked then the "stepped" factor for the current period
----					is found in the STEPPED_FCTR table.
-----					This module is called by ModAISLossLimitExcess stored procedure
-----
-----	Modified:		CSC - 12/11/2008
-----
---------------------------------------------------------------------
CREATE PROCEDURE [dbo].[ModAISIBNRSubjCalc] 
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

--IBNR: subj_ldf_ibnr_amt: Included (77)
UPDATE armis_los_exc  with(rowlock)
SET subj_ldf_ibnr_amt = 
	CASE 
		WHEN ale.lim2_amt > 0
			THEN 
				CASE
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt + (( ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) <= ale.lim2_amt) 
						THEN ((ale.subj_resrvd_idnmty_amt + ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt) >= ale.lim2_amt) 
						THEN 0  
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt + (( ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) > ale.lim2_amt) 
						THEN ( ale.lim2_amt - (ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt))
				END
			ELSE
				CASE
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt + (( ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) <= isnull(ca.dedtbl_pol_lim_amt, 0)) 
						THEN (( ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt) >= isnull(ca.dedtbl_pol_lim_amt, 0)) 
						THEN 0  
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt + (( ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) > isnull(ca.dedtbl_pol_lim_amt, 0)) 
						THEN ( isnull(ca.dedtbl_pol_lim_amt, 0) - (ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt))
				END
	END

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
and	dbo.fn_GetIBNRStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1))	
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1  

--IBNR: subj_ldf_ibnr_amt: Excluded-Company Pays (75)
UPDATE armis_los_exc  with(rowlock)
SET subj_ldf_ibnr_amt = 
	CASE 
		WHEN ale.lim2_amt > 0
			THEN 
				CASE
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt ) >= ale.lim2_amt) 
						THEN 0  
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt + ( ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) <= ale.lim2_amt) 
						THEN ( ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt + (( ale.subj_resrvd_idnmty_amt )*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) > ale.lim2_amt) 
						THEN ( ale.lim2_amt - (ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt ))
				END
			ELSE
				CASE
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt ) >= isnull(ca.dedtbl_pol_lim_amt, 0)) 
						THEN 0  
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt + ( ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) <= isnull(ca.dedtbl_pol_lim_amt, 0)) 
						THEN ( ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt + (( ale.subj_resrvd_idnmty_amt )*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) > isnull(ca.dedtbl_pol_lim_amt, 0)) 
						THEN ( isnull(ca.dedtbl_pol_lim_amt, 0) - (ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt ))
				END
	END
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
and	dbo.fn_GetIBNRStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1))	
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1  

--IBNR:  subj_ldf_ibnr_amt: Excluded – Insured Pays (76)
UPDATE armis_los_exc  with(rowlock)
SET subj_ldf_ibnr_amt = 
	CASE 
		WHEN ale.lim2_amt > 0
			THEN 
				CASE
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt  ) >= ale.lim2_amt) 
						THEN ale.subj_resrvd_exps_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id))
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt + ( ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) <= ale.lim2_amt) 
						THEN (ale.subj_resrvd_idnmty_amt + ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id))
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt + ( ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) > ale.lim2_amt) 
						THEN (ale.lim2_amt - (ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt)) +  ale.subj_resrvd_exps_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id))
				END
			ELSE
				CASE
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt  ) >= isnull(ca.dedtbl_pol_lim_amt, 0)) 
						THEN ale.subj_resrvd_exps_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id))
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt + ( ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) <= isnull(ca.dedtbl_pol_lim_amt, 0)) 
						THEN (ale.subj_resrvd_idnmty_amt + ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id))
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt + ( ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) > isnull(ca.dedtbl_pol_lim_amt, 0)) 
						THEN (isnull(ca.dedtbl_pol_lim_amt, 0) - (ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt)) +  ale.subj_resrvd_exps_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id))
				END
	END

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
and	dbo.fn_GetIBNRStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1))	
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1  

--IBNR: subj_ldf_ibnr_amt: Pro Rata (78)
UPDATE armis_los_exc  with(rowlock)
SET subj_ldf_ibnr_amt = 
	CASE 
		WHEN ale.lim2_amt > 0
			THEN 
				CASE
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt   ) >= ale.lim2_amt) 
						THEN (0 + ((case when (ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt) = 0 then 0 else (ale.lim2_amt/(ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt)) end)*resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt + (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) >= ale.lim2_amt) 
						THEN (ale.lim2_amt - (ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt) + (ale.subj_resrvd_exps_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)) ))
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt + (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) < ale.lim2_amt) 
						THEN ((ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id))) + (ale.subj_resrvd_exps_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)) ))
				END
			ELSE
				CASE
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt   ) >= isnull(ca.dedtbl_pol_lim_amt, 0)) 
						THEN (0 + ((case when (ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt) = 0 then 0 else (isnull(ca.dedtbl_pol_lim_amt, 0)/(ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt)) end)*resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt + (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) >= isnull(ca.dedtbl_pol_lim_amt, 0)) 
						THEN (isnull(ca.dedtbl_pol_lim_amt, 0) - (ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt) + (ale.subj_resrvd_exps_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)) ))
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt + (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) < isnull(ca.dedtbl_pol_lim_amt, 0)) 
						THEN ((ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id))) + (ale.subj_resrvd_exps_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)) ))
				END
	END

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
and	dbo.fn_GetIBNRStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1))	
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1  

--IBNR:  subj_ldf_ibnr_amt: ALAE Included (79)
UPDATE armis_los_exc  with(rowlock)
SET subj_ldf_ibnr_amt = 
	CASE 
		WHEN ale.lim2_amt > 0
			THEN 
				CASE
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt  ) >= ale.lim2_amt) 
						THEN 0
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt + ((ale.subj_resrvd_idnmty_amt + ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) <= ale.lim2_amt) 
						THEN ((ale.subj_resrvd_idnmty_amt + resrvd_exps_amt )*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt + ((ale.subj_resrvd_idnmty_amt + ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) > ale.lim2_amt) 
						THEN (ale.lim2_amt - (ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt))
				END
			ELSE
				CASE
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt  ) >= isnull(ca.dedtbl_pol_lim_amt, 0)) 
						THEN 0
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt + ((ale.subj_resrvd_idnmty_amt + ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) <= isnull(ca.dedtbl_pol_lim_amt, 0)) 
						THEN ((ale.subj_resrvd_idnmty_amt + resrvd_exps_amt )*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))
					WHEN ((ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt + ((ale.subj_resrvd_idnmty_amt + ale.subj_resrvd_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) > isnull(ca.dedtbl_pol_lim_amt, 0)) 
						THEN (isnull(ca.dedtbl_pol_lim_amt, 0) - (ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt +  ale.subj_resrvd_exps_amt))
				END
	END

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
and	dbo.fn_GetIBNRStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1))	
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1  

--IBNR: subj_ldf_ibnr_amt: Pro Rata - Paid (80)
UPDATE armis_los_exc  with(rowlock)
SET subj_ldf_ibnr_amt =  0
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
and	dbo.fn_GetIBNRStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1))	
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1  


--IBNR:  subj_ldf_ibnr_amt: Capped ALAE (74)
UPDATE armis_los_exc  with(rowlock)
SET subj_ldf_ibnr_amt = 
	CASE 
		WHEN ale.lim2_amt > 0
			THEN 
				CASE
					WHEN ((ale.subj_paid_exps_amt +  ale.subj_resrvd_exps_amt) <= aloc_los_adj_exps_capped_amt) 
					THEN
						CASE
							WHEN ((ale.subj_paid_idnmty_amt  + ale.subj_resrvd_idnmty_amt) >= ale.lim2_amt) 
								THEN 0 + ale.subj_resrvd_exps_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id))
							WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt  + (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) <= ale.lim2_amt) 
								THEN (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id))) + ale.subj_resrvd_exps_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id))
							WHEN ((ale.subj_paid_idnmty_amt +  ale.subj_resrvd_idnmty_amt +  (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) > ale.lim2_amt) 
								THEN (ale.lim2_amt - (ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt) + ale.subj_resrvd_exps_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))
						END

					WHEN (ale.subj_paid_exps_amt >= aloc_los_adj_exps_capped_amt)
					THEN
						CASE
							WHEN ((ale.subj_paid_idnmty_amt  + ale.subj_resrvd_idnmty_amt) >= ale.lim2_amt) 
								THEN 0
							WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt  + (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) <= ale.lim2_amt) 
								THEN (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))
							WHEN ((ale.subj_paid_idnmty_amt +  ale.subj_resrvd_idnmty_amt +  (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) > ale.lim2_amt) 
								THEN (ale.lim2_amt - (ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt))
						END
					WHEN ((ale.subj_paid_exps_amt < aloc_los_adj_exps_capped_amt) AND ((ale.subj_paid_exps_amt + ale.subj_resrvd_exps_amt) >  aloc_los_adj_exps_capped_amt))
					THEN
						CASE
							WHEN ((ale.subj_paid_idnmty_amt  + ale.subj_resrvd_idnmty_amt) >= ale.lim2_amt) 
								THEN 0 + (aloc_los_adj_exps_capped_amt  - ale.subj_paid_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id))
							WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt  + (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) <= ale.lim2_amt) 
								THEN ((ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))  + (aloc_los_adj_exps_capped_amt  - ale.subj_paid_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))
							WHEN ((ale.subj_paid_idnmty_amt +  ale.subj_resrvd_idnmty_amt +  (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) > ale.lim2_amt) 
								THEN (ale.lim2_amt - (ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt) + (aloc_los_adj_exps_capped_amt  - ale.subj_paid_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))
						END
		END
		ELSE
				CASE
					WHEN ((ale.subj_paid_exps_amt +  ale.subj_resrvd_exps_amt) <= aloc_los_adj_exps_capped_amt) 
					THEN
						CASE
							WHEN ((ale.subj_paid_idnmty_amt  + ale.subj_resrvd_idnmty_amt) >= isnull(ca.dedtbl_pol_lim_amt, 0)) 
								THEN 0 + ale.subj_resrvd_exps_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id))
							WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt  + (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) <= isnull(ca.dedtbl_pol_lim_amt, 0)) 
								THEN (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id))) + ale.subj_resrvd_exps_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id))
							WHEN ((ale.subj_paid_idnmty_amt +  ale.subj_resrvd_idnmty_amt +  (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) > isnull(ca.dedtbl_pol_lim_amt, 0)) 
								THEN (isnull(ca.dedtbl_pol_lim_amt, 0) - (ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt) + ale.subj_resrvd_exps_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))
						END

					WHEN (ale.subj_paid_exps_amt >= aloc_los_adj_exps_capped_amt)
					THEN
						CASE
							WHEN ((ale.subj_paid_idnmty_amt  + ale.subj_resrvd_idnmty_amt) >= isnull(ca.dedtbl_pol_lim_amt, 0)) 
								THEN 0
							WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt  + (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) <= isnull(ca.dedtbl_pol_lim_amt, 0)) 
								THEN (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))
							WHEN ((ale.subj_paid_idnmty_amt +  ale.subj_resrvd_idnmty_amt +  (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) > isnull(ca.dedtbl_pol_lim_amt, 0)) 
								THEN (isnull(ca.dedtbl_pol_lim_amt, 0) - (ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt))
						END
					WHEN ((ale.subj_paid_exps_amt < aloc_los_adj_exps_capped_amt) AND ((ale.subj_paid_exps_amt + ale.subj_resrvd_exps_amt) >  aloc_los_adj_exps_capped_amt))
					THEN
						CASE
							WHEN ((ale.subj_paid_idnmty_amt  + ale.subj_resrvd_idnmty_amt) >= isnull(ca.dedtbl_pol_lim_amt, 0)) 
								THEN 0 + (aloc_los_adj_exps_capped_amt  - ale.subj_paid_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id))
							WHEN ((ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt  + (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) <= isnull(ca.dedtbl_pol_lim_amt, 0)) 
								THEN ((ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))  + (aloc_los_adj_exps_capped_amt  - ale.subj_paid_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))
							WHEN ((ale.subj_paid_idnmty_amt +  ale.subj_resrvd_idnmty_amt +  (ale.subj_resrvd_idnmty_amt*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))) > isnull(ca.dedtbl_pol_lim_amt, 0)) 
								THEN (isnull(ca.dedtbl_pol_lim_amt, 0) - (ale.subj_paid_idnmty_amt + ale.subj_resrvd_idnmty_amt) + (aloc_los_adj_exps_capped_amt  - ale.subj_paid_exps_amt)*(dbo.fn_GetIBNRLDFFactor (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id)))
						END
		END
END

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
and	dbo.fn_GetIBNRStepInd (@custmr_id, @prem_adj_pgm_id, ca.coml_agmt_id) = 1))	
and ale.prem_adj_pgm_id = @prem_adj_pgm_id
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1  


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

if object_id('ModAISIBNRSubjCalc') is not null
	print 'Created Procedure ModAISIBNRSubjCalc'
else
	print 'Failed Creating Procedure ModAISIBNRSubjCalc'
go

if object_id('ModAISIBNRSubjCalc') is not null
	grant exec on ModAISIBNRSubjCalc to public
go





