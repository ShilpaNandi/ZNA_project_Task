
IF EXISTS (SELECT 1 FROM sysobjects 
	WHERE NAME = 'ModAISLossLimitExcess' and TYPE = 'P')
	drop procedure ModAISLossLimitExcess
go

set ansi_nulls off
go

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

---------------------------------------------------------------------
-----
-----	Proc Name:		ModAISLossLimitExcess
-----
-----	Version:		SQL Server 2005
-----
-----	Created:		CSC - 09/11/2008
-----
-----	Description:	This procedure will limit the Loss/Excess/Non-billable loss data
-----					based on the various ALAE rules.  Even though limiting		
-----					is performed during the ARMIS load data file, the limiting
-----					rules also need to be applied whenever loss data is updated/added/disabled
-----                   in the Loss Info and Excess/Non-Billable web pages.  During those activities
-----                   this stored proc is called.  It is also called from the calculation module
-----                   upon a calculation
-----					This stored proc also call other stored proc whihc perform calculate
-----					IBNR/LDF subject amounts and excess amount, when the IBNR/LDF Include Limit
-----					indicator is checked in the policy info page.
-----	Modified:		CSC - 12/11/2008
-----	Modified:		CSC - 02/23/2009 - if Subject Amounts are negative then set it as 0
-----	Modified:		CSc-  02/26/2009 - Added ALAE Handling - Pro Rata Paid in Req #(a.040.100)
-----   Modified:       CSC - 07/13/2011
-----					As per the TFS 14210 Bug Fix,Added the where condition to filter the calculations based on the prem_adj_id is null or in CALC status.


---------------------------------------------------------------------
CREATE PROCEDURE [dbo].[ModAISLossLimitExcess] 
@custmr_id int,  
@prem_adj_pgm_id int  
AS  
  
declare @trancount int  
  
set @trancount = @@trancount  
  
if @trancount = 0 
	begin
	    begin transaction 
	end
    
begin try  
  
--Subject Paid Indemnity - A.080.100  
UPDATE armis_los_exc with(rowlock)   
SET subj_paid_idnmty_amt =   
 CASE   
  WHEN ale.lim2_amt > 0  
   THEN   
    CASE  
     WHEN ((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) >= ale.lim2_amt)  AND (ca.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80))   
      THEN     
        CASE    
         WHEN   (lim2_amt>=0) THEN (lim2_amt)   
         ELSE (0)  
        END  
     WHEN ((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) < ale.lim2_amt  AND (ca.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80)))   
      THEN   
        CASE    
         WHEN   ((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt)>=0) THEN (ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt)  
          ELSE (0)  
        END  
    END  
   ELSE  
    CASE  
     WHEN ((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) >= ca.dedtbl_pol_lim_amt)  AND (ca.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80))   
      THEN  
        CASE    
         WHEN   (ca.dedtbl_pol_lim_amt>0) THEN (ca.dedtbl_pol_lim_amt)   
         ELSE (0)  
        END  
  
     WHEN ((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) < ca.dedtbl_pol_lim_amt  AND (ca.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80)))   
      THEN   
        CASE    
         WHEN   ((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt)>0) THEN (ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt)  
         ELSE (0)     
        END  
  
    END  
 END  
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca  
where ale.custmr_id = @custmr_id  
and ale.armis_los_pol_id = alp.armis_los_pol_id  
and alp.coml_agmt_id = ca.coml_agmt_id  
and ca.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80)  
and ale.prem_adj_pgm_id = @prem_adj_pgm_id  
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1
and ((alp.prem_adj_id is null) or (dbo.fn_GetAdjStatus(alp.prem_adj_id) = 346))
  
  
  
--Subject Paid Expense (lim2_amt is blank)  
UPDATE  armis_los_exc with(rowlock) 
SET subj_paid_exps_amt =   
 CASE  
  --A.080.120  
  WHEN aloc_los_adj_exps_typ_id IN (77) -- ALAE Handling Indicator  = Included (77)  
  THEN  
   CASE   
    WHEN ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) > (dedtbl_pol_lim_amt - ale.subj_paid_idnmty_amt))   
     THEN   
        CASE    
         WHEN   ((dedtbl_pol_lim_amt - ale.subj_paid_idnmty_amt)>0) THEN (dedtbl_pol_lim_amt - ale.subj_paid_idnmty_amt)  
         ELSE (0)  
        END  
  
    ELSE   
       CASE    
         WHEN   ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)>0) THEN (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)  
         ELSE (0)  
       END  
  
   END  
  --A.080.130  
  WHEN aloc_los_adj_exps_typ_id IN (75) -- ALAE Handling Indicator  = Excluded – Company Pays(75)  
  THEN   
        CASE    
         WHEN   ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)>0) THEN (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)  
         ELSE (0)  
         END   
  
  --A.080.140  
  WHEN aloc_los_adj_exps_typ_id IN (74) -- ALAE Handling Indicator  = ALAE Capped Amount(74)  
  THEN  
   CASE   
    WHEN ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) <= (aloc_los_adj_exps_capped_amt ))   
     THEN   
        CASE    
         WHEN   ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)>0) THEN (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)  
         ELSE (0)  
        END   
    WHEN ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) > (aloc_los_adj_exps_capped_amt ))   
     THEN   
        CASE    
         WHEN   ((aloc_los_adj_exps_capped_amt)>0) THEN (aloc_los_adj_exps_capped_amt)  
         ELSE (0)  
         END   
         
   END  
  
  --A.080.150  
  WHEN aloc_los_adj_exps_typ_id IN (76) -- ALAE Handling Indicator  = Excluded – Zurich Pays(76)  
  THEN (0)   
  
  --A.080.160  
  WHEN aloc_los_adj_exps_typ_id IN (78) -- ALAE Handling Indicator  = Pro-Rata(78)  
  THEN  
   CASE   
    WHEN (((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt)) < (dedtbl_pol_lim_amt))   
    THEN   
      CASE    
       WHEN   ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)>0) THEN (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)  
       ELSE (0)  
      END   
    ELSE  
      CASE    
       WHEN  ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) * (dedtbl_pol_lim_amt/((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt)))>=0) THEN (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) * (dedtbl_pol_lim_amt/((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt)))  
       ELSE (0)  
      END   
   
   END  
  
  --A.080.170  
  WHEN aloc_los_adj_exps_typ_id IN (80) -- ALAE Handling Indicator  = Pro-Rata Paid(80)  
  THEN  
   CASE   
    WHEN (((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt)) < (dedtbl_pol_lim_amt))   
     THEN  
      CASE    
       WHEN  ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)>=0) THEN (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)   
       ELSE (0)  
      END   
    ELSE   
      CASE    
       WHEN  ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) * (dedtbl_pol_lim_amt/(ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt))>=0) THEN (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) * (dedtbl_pol_lim_amt/(ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt))  
       ELSE (0)  
      END   
   END  
  
  --A.080.180  
  WHEN aloc_los_adj_exps_typ_id IN (79) -- ALAE Handling Indicator  = Pro-Rata No Indemnity(79)  
  THEN  
   CASE   
    WHEN (ale.subj_paid_idnmty_amt = 0)   
    THEN   
      CASE    
       WHEN  ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)>=0) THEN (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)   
       ELSE (0)  
      END   
    WHEN (ale.subj_paid_idnmty_amt > 0)   
    THEN   
     CASE  
      WHEN ((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt) <  dedtbl_pol_lim_amt)   
       THEN   
      CASE    
       WHEN  ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)>=0) THEN (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)   
       ELSE (0)  
      END    
      --WHEN ((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt) >= dedtbl_pol_lim_amt) THEN ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt)) * (dedtbl_pol_lim_amt/(ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt))  
      WHEN ((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt) >= dedtbl_pol_lim_amt) THEN ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) * (dedtbl_pol_lim_amt/((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt))))  
     END  
   END  
  
 END  
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca  
where ale.custmr_id = @custmr_id  
and ale.armis_los_pol_id = alp.armis_los_pol_id  
and alp.coml_agmt_id = ca.coml_agmt_id  
and ca.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80)  
and (ale.lim2_amt is null or ale.lim2_amt = 0)  
and ale.prem_adj_pgm_id = @prem_adj_pgm_id  
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1
and ((alp.prem_adj_id is null) or (dbo.fn_GetAdjStatus(alp.prem_adj_id) = 346))

  
--Subject Paid Expense (lim2_amt contains a value)  
UPDATE  armis_los_exc with(rowlock) 
SET subj_paid_exps_amt = --Subject Paid Expense  
 CASE  
  --A.080.120  
  WHEN aloc_los_adj_exps_typ_id IN (77) -- ALAE Handling Indicator  = Included (77)  
  THEN  
   CASE   
    WHEN ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) > (lim2_amt - ale.subj_paid_idnmty_amt))   
    THEN   
       
      CASE    
       WHEN  ((lim2_amt - ale.subj_paid_idnmty_amt) >=0) THEN(lim2_amt - ale.subj_paid_idnmty_amt)   
       ELSE (0)  
      END   
        
    ELSE   
      CASE    
       WHEN  ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) >=0) THEN (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)  
       ELSE (0)  
      END   
       
   END  
  --A.080.130  
  WHEN aloc_los_adj_exps_typ_id IN (75) -- ALAE Handling Indicator  = Excluded – Company Pays(75)  
  THEN   
      CASE    
       WHEN  ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) >=0) THEN (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)  
       ELSE (0)  
      END   
  
  --A.080.140  
  WHEN aloc_los_adj_exps_typ_id IN (74) -- ALAE Handling Indicator  = ALAE Capped Amount(74)  
  THEN  
   CASE   
    WHEN ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) <= (aloc_los_adj_exps_capped_amt ))   
     THEN   
      CASE    
       WHEN  ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) >=0) THEN (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)  
       ELSE (0)  
      END   
    WHEN ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) > (aloc_los_adj_exps_capped_amt ))   
     THEN   
      CASE    
       WHEN  ((aloc_los_adj_exps_capped_amt)   >=0) THEN (aloc_los_adj_exps_capped_amt)    
       ELSE (0)  
      END   
  
   END  
  
  --A.080.150  
  WHEN aloc_los_adj_exps_typ_id IN (76) -- ALAE Handling Indicator  = Excluded – Zurich Pays(76)  
  THEN (0)   
  
  --A.080.160  
  WHEN aloc_los_adj_exps_typ_id IN (78) -- ALAE Handling Indicator  = Pro-Rata(78)  
  THEN  
   CASE   
    WHEN (((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt)) < (lim2_amt))  
      THEN   
      CASE    
       WHEN  ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) >=0) THEN (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)  
       ELSE (0)  
      END    
    ELSE   
      CASE    
       WHEN  ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) * (lim2_amt/((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt))) >=0) THEN (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt
) * (lim2_amt/((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt)))  
       ELSE (0)  
      END   
  
   END  
  
  --A.080.170  
  WHEN aloc_los_adj_exps_typ_id IN (80) -- ALAE Handling Indicator  = Pro-Rata Paid(80)  
  THEN  
   CASE   
    WHEN (((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt)) < (lim2_amt))   
     THEN   
     CASE    
       WHEN  ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) >=0) THEN (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)  
       ELSE (0)  
      END     
    ELSE   
     CASE    
       WHEN  ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) * (lim2_amt/(ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt)) >=0) THEN (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) * (lim2_amt/(ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt))  
       ELSE (0)  
      END    
  
   END  
  
  --A.080.180  
  WHEN aloc_los_adj_exps_typ_id IN (79) -- ALAE Handling Indicator  = Pro-Rata No Indemnity(79)  
  THEN  
   CASE   
    WHEN (ale.subj_paid_idnmty_amt = 0)   
     THEN   
      CASE    
       WHEN  ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) >=0) THEN (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)  
       ELSE (0)  
      END   
    WHEN (ale.subj_paid_idnmty_amt > 0)   
    THEN   
     CASE  
      WHEN ((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt) <  lim2_amt)   
       THEN   
        CASE    
         WHEN  ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) >=0) THEN (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt)  
         ELSE (0)  
      END    
      WHEN ((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt) >= lim2_amt)   
       THEN   
        CASE    
         WHEN  (((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) * (lim2_amt/((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt)))) >=0) THEN ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) * (lim2_amt/((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt))))  
         ELSE (0)  
      END   
  
     END  
   END  
  
 END  
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca  
where ale.custmr_id = @custmr_id  
and ale.armis_los_pol_id = alp.armis_los_pol_id  
and alp.coml_agmt_id = ca.coml_agmt_id  
and ca.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80)  
and ale.lim2_amt > 0  
and ale.prem_adj_pgm_id = @prem_adj_pgm_id  
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1
and ((alp.prem_adj_id is null) or (dbo.fn_GetAdjStatus(alp.prem_adj_id) = 346))

  
--Subject Reserve Indemnity    
UPDATE armis_los_exc with(rowlock)   
SET subj_resrvd_idnmty_amt =   
 CASE   
  WHEN ale.lim2_amt > 0  
   THEN   
     CASE  
      --A.080.200  
      WHEN aloc_los_adj_exps_typ_id IN (77) -- ALAE Handling Indicator  = Included   
      THEN  
       CASE   
        WHEN ((resrvd_idnmty_amt - non_bilabl_resrvd_idnmty_amt) >= (lim2_amt - (ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt)))   
         THEN   
         CASE    
          WHEN  ( (lim2_amt - (ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt))  >=0) THEN  (lim2_amt - (ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt))   
          ELSE (0)  
         END    
           
        ELSE  
         CASE    
          WHEN  ((resrvd_idnmty_amt - non_bilabl_resrvd_idnmty_amt)  >=0) THEN  (resrvd_idnmty_amt - non_bilabl_resrvd_idnmty_amt)  
          ELSE (0)  
         END  
   
       END  
  
      --A.080.210  
      WHEN aloc_los_adj_exps_typ_id IN (74,75,76,78,79,80) -- ALAE Handling Indicator  = REMAINING 6  
      THEN  
       CASE   
        WHEN ((resrvd_idnmty_amt - non_bilabl_resrvd_idnmty_amt) >= (lim2_amt - ale.subj_paid_idnmty_amt))   
         THEN   
         CASE    
          WHEN  ((lim2_amt - ale.subj_paid_idnmty_amt)    >=0) THEN  (lim2_amt - ale.subj_paid_idnmty_amt)    
          ELSE (0)  
         END  
  
        ELSE   
         CASE    
          WHEN  ((resrvd_idnmty_amt - non_bilabl_resrvd_idnmty_amt)  >=0) THEN  (resrvd_idnmty_amt - non_bilabl_resrvd_idnmty_amt)  
          ELSE (0)  
         END  
  
       END  
      ELSE 0  
     END  
   ELSE  
  
     CASE  
      --A.080.200  
      WHEN aloc_los_adj_exps_typ_id IN (77) -- ALAE Handling Indicator  = Included   
      THEN  
       CASE   
        WHEN ((resrvd_idnmty_amt - non_bilabl_resrvd_idnmty_amt) >= (dedtbl_pol_lim_amt - (ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt)))  
          THEN   
         CASE    
          WHEN  ((dedtbl_pol_lim_amt - (ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt))  >=0) THEN  (dedtbl_pol_lim_amt - (ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt))    
          ELSE (0)  
         END  
  
  
        ELSE  
         CASE    
          WHEN  ((resrvd_idnmty_amt - non_bilabl_resrvd_idnmty_amt)  >=0) THEN  (resrvd_idnmty_amt - non_bilabl_resrvd_idnmty_amt)  
          ELSE (0)  
         END  
  
       END  
  
      --A.080.210  
      WHEN aloc_los_adj_exps_typ_id IN (74,75,76,78,79,80) -- ALAE Handling Indicator  = REMAINING 6  
      THEN  
       CASE   
        WHEN ((resrvd_idnmty_amt - non_bilabl_resrvd_idnmty_amt) >= (dedtbl_pol_lim_amt - ale.subj_paid_idnmty_amt))   
         THEN   
         CASE    
          WHEN  ((dedtbl_pol_lim_amt - ale.subj_paid_idnmty_amt)    >=0) THEN  (dedtbl_pol_lim_amt - ale.subj_paid_idnmty_amt)    
          ELSE (0)  
         END  
  
        ELSE   
         CASE    
          WHEN  ((resrvd_idnmty_amt - non_bilabl_resrvd_idnmty_amt)  >=0) THEN  (resrvd_idnmty_amt - non_bilabl_resrvd_idnmty_amt)  
          ELSE (0)  
         END  
  
       END  
      ELSE 0  
     END  
  
   END  
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca  
where ale.custmr_id = @custmr_id  
and ale.armis_los_pol_id = alp.armis_los_pol_id  
and alp.coml_agmt_id = ca.coml_agmt_id  
and  ca.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80)  
and ale.prem_adj_pgm_id = @prem_adj_pgm_id  
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1
and ((alp.prem_adj_id is null) or (dbo.fn_GetAdjStatus(alp.prem_adj_id) = 346))

  
-- Subject Reserve Expense(lim2_amt does not contain a value)  
UPDATE armis_los_exc with(rowlock)  
SET subj_resrvd_exps_amt =   
 CASE  
  --A.080.230  
  WHEN aloc_los_adj_exps_typ_id IN (77) -- ALAE Handling Indicator  = Included   
  THEN  
   CASE   
    WHEN ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt) >= (dedtbl_pol_lim_amt - (ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt)))   
     THEN   
         CASE    
          WHEN  ((dedtbl_pol_lim_amt - (ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt))   >=0) THEN  (dedtbl_pol_lim_amt - (ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt))    
          ELSE (0)  
         END  
  
    ELSE   
         CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)  >=0) THEN  (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)  
          ELSE (0)  
         END  
  
   END  
  
  --A.080.240  
  WHEN aloc_los_adj_exps_typ_id IN (75) -- ALAE Handling Indicator  = Excluded – Company Pays  
  THEN   
         CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)   >=0) THEN  (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)   
          ELSE (0)  
         END  
  
  
  --A.080.250  
  WHEN aloc_los_adj_exps_typ_id IN (76) -- ALAE Handling Indicator  = Excluded – Zurich Pays  
  THEN (0)   
  
  --A.080.260  
  WHEN aloc_los_adj_exps_typ_id IN (78) -- ALAE Handling Indicator  = Pro-Rata  
  THEN  
   CASE   
    WHEN (((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt)) < (dedtbl_pol_lim_amt))  
     THEN   
         CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)    >=0) THEN  (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)    
          ELSE (0)  
         END  
  
    ELSE   
         CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt) * (dedtbl_pol_lim_amt/((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt)))  >=0) THEN  (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt) * (dedtbl_pol_lim_amt/((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt)))  
          ELSE (0)  
         END  
  
   END  
  
  --A.080.270  
  WHEN aloc_los_adj_exps_typ_id IN (80) -- ALAE Handling Indicator  = Pro-Rata Paid  
  THEN  
   CASE   
    WHEN (((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt)) < (dedtbl_pol_lim_amt))  
     THEN   
         CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)   >=0) THEN  (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)   
          ELSE (0)  
         END  
   
    ELSE   
         CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt) * (dedtbl_pol_lim_amt/(ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt))  >=0) THEN  (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt) * (dedtbl_pol_lim_amt/(ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt))  
          ELSE (0)  
         END  
  
   END  
  
  
  --A.080.280  
  WHEN aloc_los_adj_exps_typ_id IN (79) -- ALAE Handling Indicator  = Pro-Rata No Indemnity  
  THEN  
   CASE   
    WHEN (ale.subj_paid_idnmty_amt = 0)  
     THEN   
         CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt) >=0) THEN  (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)  
          ELSE (0)  
         END  
  
    WHEN (ale.subj_paid_idnmty_amt > 0)   
    THEN   
     CASE  
      WHEN ((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt) <  dedtbl_pol_lim_amt)  
 THEN   
         CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)   >=0) THEN  (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)    
          ELSE (0)  
         END  
  
      WHEN (((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt)) >= dedtbl_pol_lim_amt)  
        THEN   
         CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt) * (dedtbl_pol_lim_amt/((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt)))  >=0) THEN  (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt) * (dedtbl_pol_lim_amt/((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt)))  
          ELSE (0)  
         END  
     END  
   END  
  
  --A.080.290  
  WHEN aloc_los_adj_exps_typ_id IN (74) -- ALAE Handling Indicator  = ALAE Capped  
  THEN  
   CASE   
    WHEN ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt) <= (aloc_los_adj_exps_capped_amt - ale.subj_paid_exps_amt ))  
       THEN   
         CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)   >=0) THEN  (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)    
          ELSE (0)  
         END  
  
    WHEN ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) > (aloc_los_adj_exps_capped_amt - ale.subj_paid_exps_amt ))  
       THEN   
         CASE    
          WHEN  ((aloc_los_adj_exps_capped_amt - ale.subj_paid_exps_amt)    >=0) THEN  (aloc_los_adj_exps_capped_amt - ale.subj_paid_exps_amt)    
          ELSE (0)  
         END  
  
   END  
 END  
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca  
where ale.custmr_id = @custmr_id  
and ale.armis_los_pol_id = alp.armis_los_pol_id  
and alp.coml_agmt_id = ca.coml_agmt_id  
and ca.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80)  
and (ale.lim2_amt is null or ale.lim2_amt = 0)  
and ale.prem_adj_pgm_id = @prem_adj_pgm_id  
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1
and ((alp.prem_adj_id is null) or (dbo.fn_GetAdjStatus(alp.prem_adj_id) = 346))

  
-- Subject Reserve Expense(lim2_amt contains a value)  
UPDATE armis_los_exc with(rowlock)   
SET subj_resrvd_exps_amt =   
 CASE  
  --A.080.230  
  WHEN aloc_los_adj_exps_typ_id IN (77) -- ALAE Handling Indicator  = Included   
  THEN  
   CASE   
    WHEN ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt) >= (lim2_amt - (ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt)))  
       THEN   
        CASE    
          WHEN  ((lim2_amt - (ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt))    >=0) THEN  (lim2_amt - (ale.subj_paid_idnmty_amt + ale.subj_paid_exps_amt + ale.subj_resrvd_idnmty_amt))    
          ELSE (0)  
         END  
  
    ELSE   
        CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)  >=0) THEN  (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)  
          ELSE (0)  
         END  
  
   END  
  
  --A.080.240  
  WHEN aloc_los_adj_exps_typ_id IN (75) -- ALAE Handling Indicator  = Excluded – Company Pays  
        THEN   
         CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)   >=0) THEN (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)   
          ELSE (0)  
         END  
  
  
  --A.080.250  
  WHEN aloc_los_adj_exps_typ_id IN (76) -- ALAE Handling Indicator  = Excluded – Zurich Pays  
  THEN (0)   
  
  --A.080.260  
  WHEN aloc_los_adj_exps_typ_id IN (78) -- ALAE Handling Indicator  = Pro-Rata  
  THEN  
   CASE   
    WHEN (((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt)) < (lim2_amt))   
       THEN   
        CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)  >=0) THEN  (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)   
          ELSE (0)  
         END  
   
    ELSE   
        CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt) * (lim2_amt/((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt)))  >=0) THEN  (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt) * (lim2_amt/((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt)))  
          ELSE (0)  
         END  
  
   END  
  
  --A.080.270  
  WHEN aloc_los_adj_exps_typ_id IN (80) -- ALAE Handling Indicator  = Pro-Rata Paid  
  THEN  
   CASE   
    WHEN (((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt)) < (lim2_amt))   
       THEN   
        CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)   >=0) THEN  (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)   
          ELSE (0)  
         END  
   
    ELSE   
        CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt) * (lim2_amt/(ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt)) >=0) THEN  (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt) * (lim2_amt/(ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt))  
          ELSE (0)  
         END  
  
   END  
  
  
  --A.080.280  
  WHEN aloc_los_adj_exps_typ_id IN (79) -- ALAE Handling Indicator  = Pro-Rata No Indemnity  
  THEN  
   CASE   
    WHEN (ale.subj_paid_idnmty_amt = 0)   
      THEN   
        CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)  >=0) THEN  (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)  
          ELSE (0)  
         END  
  
    WHEN (ale.subj_paid_idnmty_amt > 0)   
    THEN   
     CASE  
      WHEN ((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt) <  lim2_amt)   
       THEN   
         CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)  >=0) THEN  (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)  
          ELSE (0)  
         END  
    
      WHEN (((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt)) >= lim2_amt)  
       THEN   
         CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt) * (lim2_amt/((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt))) >=0) THEN (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt) * (lim2_amt/((ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt) + (ale.resrvd_idnmty_amt - ale.non_bilabl_resrvd_idnmty_amt)))  
          ELSE (0)  
         END  
  
     END  
   END  
  
  --A.080.290  
  WHEN aloc_los_adj_exps_typ_id IN (74) -- ALAE Handling Indicator  = ALAE Capped  
  THEN  
   CASE   
    WHEN ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt) <= (aloc_los_adj_exps_capped_amt - ale.subj_paid_exps_amt ))  
      THEN   
         CASE    
          WHEN  ((ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)   >=0) THEN  (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt)   
          ELSE (0)  
         END  
   
    WHEN ((ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) > (aloc_los_adj_exps_capped_amt - ale.subj_paid_exps_amt ))  
      THEN   
         CASE    
          WHEN  ((aloc_los_adj_exps_capped_amt - ale.subj_paid_exps_amt)    >=0) THEN  (aloc_los_adj_exps_capped_amt - ale.subj_paid_exps_amt)    
          ELSE (0)  
         END  
  
   END  
 END  
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca  
where ale.custmr_id = @custmr_id  
and ale.armis_los_pol_id = alp.armis_los_pol_id  
and alp.coml_agmt_id = ca.coml_agmt_id  
and ca.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80)  
and ( ale.lim2_amt > 0)  
and ale.prem_adj_pgm_id = @prem_adj_pgm_id  
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1
and ((alp.prem_adj_id is null) or (dbo.fn_GetAdjStatus(alp.prem_adj_id) = 346))

  
--Excess Paid Indemnity - A.080.110  
UPDATE armis_los_exc with(rowlock)   
SET  exc_paid_idnmty_amt = (ale.paid_idnmty_amt - ale.non_bilabl_paid_idnmty_amt - ale.subj_paid_idnmty_amt)  
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca  
where ale.custmr_id = @custmr_id  
and ale.armis_los_pol_id = alp.armis_los_pol_id  
and alp.coml_agmt_id = ca.coml_agmt_id  
and ca.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80)  
and ale.prem_adj_pgm_id = @prem_adj_pgm_id  
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1
and ((alp.prem_adj_id is null) or (dbo.fn_GetAdjStatus(alp.prem_adj_id) = 346))
  
-- Excess Paid Expense A.080.190  
UPDATE armis_los_exc with(rowlock)   
SET exc_paid_exps_amt = (ale.paid_exps_amt - ale.non_bilabl_paid_exps_amt) - ale.subj_paid_exps_amt  
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca  
where ale.custmr_id = @custmr_id  
and ale.armis_los_pol_id = alp.armis_los_pol_id  
and alp.coml_agmt_id = ca.coml_agmt_id  
and ca.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80)  
and ale.prem_adj_pgm_id = @prem_adj_pgm_id  
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1
and ((alp.prem_adj_id is null) or (dbo.fn_GetAdjStatus(alp.prem_adj_id) = 346))

  
-- Excess Reserve Indemnity A.080.220  
UPDATE armis_los_exc with(rowlock)   
SET   exc_resrvd_idnmty_amt =   
  CASE  
   WHEN aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80) -- ALAE Handling Indicator  = ALL  
   THEN (resrvd_idnmty_amt - non_bilabl_resrvd_idnmty_amt - subj_resrvd_idnmty_amt)  
   ELSE 0  
  END  
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca  
where ale.custmr_id = @custmr_id  
and ale.armis_los_pol_id = alp.armis_los_pol_id  
and alp.coml_agmt_id = ca.coml_agmt_id  
and ca.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,79,80)  
and ale.prem_adj_pgm_id = @prem_adj_pgm_id  
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1
and ((alp.prem_adj_id is null) or (dbo.fn_GetAdjStatus(alp.prem_adj_id) = 346))

  
-- Excess Reserve Expense A.080.300  
UPDATE armis_los_exc with(rowlock)   
SET exc_resrvd_exps_amt =    
  CASE  
   WHEN aloc_los_adj_exps_typ_id IN (74,75,76,77,78,80) -- ALAE Handling Indicator  <> 79  
   THEN (ale.resrvd_exps_amt - ale.non_bilabl_resrvd_exps_amt - ale.subj_resrvd_exps_amt)  
  END  
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca  
where ale.custmr_id = @custmr_id  
and ale.armis_los_pol_id = alp.armis_los_pol_id  
and alp.coml_agmt_id = ca.coml_agmt_id  
and ca.aloc_los_adj_exps_typ_id IN (74,75,76,77,78,80)  
and ale.prem_adj_pgm_id = @prem_adj_pgm_id  
and ca.actv_ind = 1 
and alp.actv_ind = 1
and ale.actv_ind = 1 
and ((alp.prem_adj_id is null) or (dbo.fn_GetAdjStatus(alp.prem_adj_id) = 346))
 
  
-- Execute stored procedures which perform the IBNR/LDF calculation  
-- They calculate the IBNR or LDF subject amounts and 
-- IBNR or LDF excess amount
-- First zero out the values 
UPDATE armis_los_exc with(rowlock)
set subj_ldf_ibnr_amt = 0,
    exc_ldf_ibnr_amt = 0
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca  
where ale.custmr_id = @custmr_id  
and ale.armis_los_pol_id = alp.armis_los_pol_id  
and ale.prem_adj_pgm_id = @prem_adj_pgm_id  
and alp.actv_ind = 1
and ale.actv_ind = 1
and ((alp.prem_adj_id is null) or (dbo.fn_GetAdjStatus(alp.prem_adj_id) = 346))

exec dbo.ModAISIBNRSubjCalc @custmr_id, @prem_adj_pgm_id  
exec dbo.ModAISLDFSubjCalc @custmr_id, @prem_adj_pgm_id  
exec dbo.ModAISIBNRLDFExcessCalc @custmr_id, @prem_adj_pgm_id  
  
-- Round the subj_ldf_ibnr_amt and exc_ldf_ibnr_amt fields
update armis_los_exc with(rowlock)
set 
exc_paid_idnmty_amt   = round(isnull(ale.exc_paid_idnmty_amt,0),0),
exc_paid_exps_amt   = round(isnull(ale.exc_paid_exps_amt,0),0),
exc_resrvd_idnmty_amt  = round(isnull(ale.exc_resrvd_idnmty_amt,0),0),
exc_resrvd_exps_amt   = round(isnull(ale.exc_resrvd_exps_amt,0),0),
non_bilabl_paid_idnmty_amt = round(isnull(ale.non_bilabl_paid_idnmty_amt,0),0),
non_bilabl_paid_exps_amt = round(isnull(ale.non_bilabl_paid_exps_amt,0),0),
non_bilabl_resrvd_idnmty_amt= round(isnull(ale.non_bilabl_resrvd_idnmty_amt,0),0),
non_bilabl_resrvd_exps_amt = round(isnull(ale.non_bilabl_resrvd_exps_amt,0),0),
exc_ldf_ibnr_amt   = round(isnull(ale.exc_ldf_ibnr_amt,0),0),
subj_ldf_ibnr_amt   = round(isnull(ale.subj_ldf_ibnr_amt,0),0)
from armis_los_exc ale, armis_los_pol alp, coml_agmt ca  
where ale.custmr_id = @custmr_id  
and ale.armis_los_pol_id = alp.armis_los_pol_id  
and ale.prem_adj_pgm_id = @prem_adj_pgm_id  
and alp.actv_ind = 1
and ale.actv_ind = 1
and ((alp.prem_adj_id is null) or (dbo.fn_GetAdjStatus(alp.prem_adj_id) = 346))

--Roll-up the Excess amounts from the ARMIS_POL_EXC table into the ARMIS_LOS_POL - By Policy, By State  
--The four Excess components are summed up at the policy/state level  
--A.080.310,A.080.320,A.080.330,A.080.340,A.080.350,A.080.360,A.080.370,A.080.380  

-- First Clear out values
UPDATE ARMIS_LOS_POL with(rowlock)  
SET   
exc_paid_idnmty_amt = 0,
exc_paid_exps_amt = 0,
exc_resrvd_idnmty_amt = 0,
exc_resrv_exps_amt = 0,
non_bilabl_paid_idnmty_amt = 0,
non_bilabl_paid_exps_amt = 0,
non_bilabl_resrv_idnmty_amt = 0,
non_bilabl_resrv_exps_amt = 0,
exc_ldf_ibnr_amt = 0,
subj_ldf_ibnr_amt = 0
WHERE custmr_id = @custmr_id  
and prem_adj_pgm_id = @prem_adj_pgm_id  
and actv_ind = 1
and ((prem_adj_id is null) or (dbo.fn_GetAdjStatus(prem_adj_id) = 346))
  
UPDATE ARMIS_LOS_POL with(rowlock)  
SET   
exc_paid_idnmty_amt = round(isnull(ARMLosPol1.exc_paid_idnmty_amt,0),0), 
exc_paid_exps_amt = round(isnull(ARMLosPol1.exc_paid_exps_amt,0),0),
exc_resrvd_idnmty_amt = round(isnull(ARMLosPol1.exc_resrvd_idnmty_amt,0),0),
exc_resrv_exps_amt = round(isnull(ARMLosPol1.exc_resrv_exps_amt,0),0),
non_bilabl_paid_idnmty_amt = round(isnull(ARMLosPol1.non_bilabl_paid_idnmty_amt,0),0),
non_bilabl_paid_exps_amt = round(isnull(ARMLosPol1.non_bilabl_paid_exps_amt,0),0),
non_bilabl_resrv_idnmty_amt = round(isnull(ARMLosPol1.non_bilabl_resrvd_idnmty_amt,0),0),
non_bilabl_resrv_exps_amt = round(isnull(ARMLosPol1.non_bilabl_resrv_exps_amt,0),0),
exc_ldf_ibnr_amt = round(isnull(ARMLosPol1.exc_ldf_ibnr_amt,0),0),
subj_ldf_ibnr_amt = round(isnull(ARMLosPol1.subj_ldf_ibnr_amt,0),0)
FROM  
(  
SELECT   
ale.coml_agmt_id, ARMLosPol2.st_id,  ale.armis_los_pol_id,
SUM(ISNULL(ale.exc_paid_idnmty_amt, 0)) AS exc_paid_idnmty_amt, --A.080.350  
SUM(ISNULL(ale.exc_paid_exps_amt, 0)) AS exc_paid_exps_amt, --A.080.360  
SUM(ISNULL(ale.exc_resrvd_idnmty_amt, 0)) AS exc_resrvd_idnmty_amt, --A.080.370  
SUM(ISNULL(ale.exc_resrvd_exps_amt, 0)) AS exc_resrv_exps_amt, --A.080.380  
SUM(ISNULL(ale.non_bilabl_paid_idnmty_amt, 0)) AS non_bilabl_paid_idnmty_amt, --A.080.310  
SUM(ISNULL(ale.non_bilabl_paid_exps_amt, 0)) AS non_bilabl_paid_exps_amt, --A.080.320  
SUM(ISNULL(ale.non_bilabl_resrvd_idnmty_amt, 0)) AS non_bilabl_resrvd_idnmty_amt, --A.080.330  
SUM(ISNULL(ale.non_bilabl_resrvd_exps_amt, 0)) AS non_bilabl_resrv_exps_amt, --A.080.340  
SUM(ISNULL(ale.exc_ldf_ibnr_amt, 0)) AS exc_ldf_ibnr_amt   ,
SUM(ISNULL(ale.subj_ldf_ibnr_amt, 0)) AS subj_ldf_ibnr_amt  
FROM ARMIS_LOS_EXC ale  
INNER JOIN ARMIS_LOS_POL ARMLosPol2 ON   
ale.coml_agmt_id = ARMLosPol2.coml_agmt_id and ale.armis_los_pol_id = ARMLosPol2.armis_los_pol_id  
WHERE ale.custmr_id = @custmr_id  
and ale.prem_adj_pgm_id = @prem_adj_pgm_id  
and ale.actv_ind = 1
and ((prem_adj_id is null) or (dbo.fn_GetAdjStatus(prem_adj_id) = 346))
GROUP BY ale.coml_agmt_id, ARMLosPol2.st_id, ale.armis_los_pol_id ) as ARMLosPol1  
INNER JOIN  
ARMIS_LOS_POL ARMLosPol2 ON (ARMLosPol1.coml_agmt_id = ARMLosPol2.coml_agmt_id) 
       and (ARMLosPol1.st_id = ARMLosPol2.st_id)
       and (ARMLosPol1.armis_los_pol_id = ARMLosPol2.armis_los_pol_id)
  
  
UPDATE ARMIS_LOS_POL with(rowlock)  
Set   
--A.080.390  
subj_paid_idnmty_amt =
CASE    
          WHEN  (((paid_idnmty_amt) - (non_bilabl_paid_idnmty_amt) - (exc_paid_idnmty_amt)) >=0) 
   THEN  ((paid_idnmty_amt) - (non_bilabl_paid_idnmty_amt) - (exc_paid_idnmty_amt))   
          ELSE (0)  
END ,
--A.080.400  
subj_paid_exps_amt = 
CASE    
          WHEN  (((paid_exps_amt) - (non_bilabl_paid_exps_amt) - (exc_paid_exps_amt)) >=0) 
   THEN  ((paid_exps_amt) - (non_bilabl_paid_exps_amt) - (exc_paid_exps_amt))   
          ELSE (0)  
END ,

--A.080.410  
subj_resrv_idnmty_amt = 
CASE    
          WHEN  (((resrv_idnmty_amt) - (non_bilabl_resrv_idnmty_amt) - (exc_resrvd_idnmty_amt)) >=0) 
   THEN  ((resrv_idnmty_amt) - (non_bilabl_resrv_idnmty_amt) - (exc_resrvd_idnmty_amt))   
          ELSE (0)  
END ,

--A.080.420  
subj_resrv_exps_amt = 
CASE    
          WHEN  (((resrv_exps_amt) - (non_bilabl_resrv_exps_amt) - (exc_resrv_exps_amt)) >=0) 
   THEN  ((resrv_exps_amt) - (non_bilabl_resrv_exps_amt) - (exc_resrv_exps_amt))   
          ELSE (0)  
END 
  
where custmr_id = @custmr_id  
and prem_adj_pgm_id = @prem_adj_pgm_id  
and actv_ind = 1
and ((prem_adj_id is null) or (dbo.fn_GetAdjStatus(prem_adj_id) = 346))


-- A.040.100  
UPDATE ARMIS_LOS_POL with(rowlock) 
SET subj_paid_idnmty_amt = alp.paid_idnmty_amt,  
subj_paid_exps_amt = alp.paid_exps_amt,  
subj_resrv_idnmty_amt = alp.resrv_idnmty_amt ,  
subj_resrv_exps_amt = alp.resrv_exps_amt  
from  armis_los_pol alp, coml_agmt ca  
where alp.custmr_id = @custmr_id  
and alp.armis_los_pol_id = alp.armis_los_pol_id  
and alp.coml_agmt_id = ca.coml_agmt_id  
and ca.aloc_los_adj_exps_typ_id IN (74, 75, 77, 78, 79,80)  
and alp.prem_adj_pgm_id = @prem_adj_pgm_id and  
(alp.non_bilabl_paid_idnmty_amt is  null and    
alp.non_bilabl_paid_exps_amt is null and  
alp.non_bilabl_resrv_idnmty_amt is null and   
alp.non_bilabl_resrv_exps_amt is null and  
alp.exc_paid_idnmty_amt is null and  
alp.exc_paid_exps_amt is null and  
alp.exc_resrvd_idnmty_amt is null and  
alp.exc_resrv_exps_amt is null)  
and alp.actv_ind = 1
and ca.actv_ind = 1
and ((prem_adj_id is null) or (dbo.fn_GetAdjStatus(prem_adj_id) = 346))
  
-- A.040.110  
UPDATE ARMIS_LOS_POL with(rowlock)  
SET subj_paid_idnmty_amt = alp.paid_idnmty_amt,  
subj_paid_exps_amt   = 0,  
subj_resrv_idnmty_amt  = alp.resrv_idnmty_amt ,  
subj_resrv_exps_amt  = 0,  
exc_paid_exps_amt   = alp.paid_idnmty_amt,  
exc_resrv_exps_amt   = alp.resrv_exps_amt  
from  armis_los_pol alp, coml_agmt ca  
where alp.custmr_id = @custmr_id  
and alp.armis_los_pol_id = alp.armis_los_pol_id  
and alp.coml_agmt_id = ca.coml_agmt_id  
and ca.aloc_los_adj_exps_typ_id IN (76)  
and alp.prem_adj_pgm_id = @prem_adj_pgm_id and  
(alp.non_bilabl_paid_idnmty_amt is  null and    
alp.non_bilabl_paid_exps_amt is null and  
alp.non_bilabl_resrv_idnmty_amt is null and   
alp.non_bilabl_resrv_exps_amt is null and  
alp.exc_paid_idnmty_amt is null and  
alp.exc_paid_exps_amt is null and  
alp.exc_resrvd_idnmty_amt is null and  
alp.exc_resrv_exps_amt is null)  
and alp.actv_ind = 1
and ca.actv_ind = 1
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

if object_id('ModAISLossLimitExcess') is not null
	print 'Created Procedure ModAISLossLimitExcess'
else
	print 'Failed Creating Procedure ModAISLossLimitExcess'
go

if object_id('ModAISLossLimitExcess') is not null
	grant exec on ModAISLossLimitExcess to public
go



