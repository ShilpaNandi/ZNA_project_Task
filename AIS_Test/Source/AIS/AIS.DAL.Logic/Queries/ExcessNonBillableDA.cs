using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class ExcessNonBillableDA : DataAccessor<ARMIS_LOS_EXC, ExcessNonBillableBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Returns all Exc Non Bil details that match criteria
        /// </summary>
        /// <returns></returns>
        public IList<ExcessNonBillableBE> getExcNonBillableData(int armsID, int comAmgtID, int custmrID, int prgID)
        {
            IList<ExcessNonBillableBE> result = new List<ExcessNonBillableBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Excess Non Billable Details
            /// and project it into  Excess Non Billable Details Business Entity
            IQueryable<ExcessNonBillableBE> query =
            (from amsexc in this.Context.ARMIS_LOS_EXCs
             join ams in this.Context.ARMIS_LOS_POLs
             on amsexc.armis_los_pol_id equals ams.armis_los_pol_id
             join pol in this.Context.COML_AGMTs
             on ams.coml_agmt_id equals pol.coml_agmt_id
             //join lk in this.Context.LKUPs
             //on pol.aloc_los_adj_exps_typ_id equals lk.lkup_id
             where ams.armis_los_pol_id == armsID && ams.coml_agmt_id == comAmgtID && ams.custmr_id == custmrID && ams.prem_adj_pgm_id == prgID
             orderby amsexc.clm_nbr_txt, amsexc.clmt_nm.Trim() ascending
             select new ExcessNonBillableBE()
             {
                 ARMIS_LOS_EXC_ID = amsexc.armis_los_exc_id,
                 ARMIS_LOS_ID = amsexc.armis_los_pol_id,
                 COML_AGMT_ID = amsexc.coml_agmt_id,
                 CUSTMR_ID = amsexc.custmr_id,
                 ORGIN_CLAIM_NBR_TXT = amsexc.orgin_clm_nbr_txt,
                 CLAIM_NBR_TXT = amsexc.clm_nbr_txt,
                 LIMIT2_AMT = amsexc.lim2_amt,
                 ADD_CLAIM_TXT = amsexc.addn_clm_txt,
                 SITE_CD_TXT = amsexc.site_cd_txt,
                 COVG_TRIGGER_DATE = amsexc.covg_trigr_dt,
                 CLAIMANT_NM = amsexc.clmt_nm,
                 REOPEN_CLAIMANT_NBR_TXT = amsexc.reop_clm_nbr_txt,
                 PAID_IDNMTY_AMT = amsexc.paid_idnmty_amt,
                 PAID_EXPS_AMT = amsexc.paid_exps_amt,
                 RESRV_IDNMTY_AMT = amsexc.resrvd_idnmty_amt,
                 RESRV_EXPS_AMT = amsexc.resrvd_exps_amt,
                 NON_BILABL_PAID_IDNMTY_AMT = amsexc.non_bilabl_paid_idnmty_amt,
                 NON_BILABL_PAID_EXPS_AMT = amsexc.non_bilabl_paid_exps_amt,
                 NON_BILABL_RESRV_IDNMTY_AMT = amsexc.non_bilabl_resrvd_idnmty_amt,
                 NON_BILABL_RESRV_EXPS_AMT = amsexc.non_bilabl_resrvd_exps_amt,
                 SUBJ_PAID_IDNMTY_AMT = amsexc.subj_paid_idnmty_amt,
                 SUBJ_PAID_EXPS_AMT = amsexc.subj_paid_exps_amt,
                 SUBJ_RESRV_IDNMTY_AMT = amsexc.subj_resrvd_idnmty_amt,
                 SUBJ_RESRV_EXPS_AMT = amsexc.subj_resrvd_exps_amt,
                 EXC_PAID_IDNMTY_AMT = amsexc.exc_paid_idnmty_amt,
                 EXC_PAID_EXPS_AMT = amsexc.exc_paid_exps_amt,
                 EXC_RESRV_IDNMTY_AMT = amsexc.exc_resrvd_idnmty_amt,
                 EXC_RESRV_EXPS_AMT = amsexc.exc_resrvd_exps_amt,
                 SYS_GENRT_IND = amsexc.sys_genrt_ind,
                 COPY_IND = amsexc.copy_ind,
                 ACTV_IND = amsexc.actv_ind,
                 ADDN_CLAIMS = amsexc.addn_clm_ind,
                 ALAE_CAP = pol.aloc_los_adj_exps_capped_amt,
                 POLICY_AMT = pol.dedtbl_pol_lim_amt,
                 ALAE_TYP = (from lk in this.Context.LKUPs
                             where pol.aloc_los_adj_exps_typ_id == lk.lkup_id
                             select lk.lkup_txt).First().ToString(),
                 CLAIMSTATUS = (from lk in this.Context.LKUPs
                                where amsexc.clm_sts_id == lk.lkup_id
                                select lk.lkup_txt).First().ToString(),
                 CLAIM_STS_ID = amsexc.clm_sts_id,
                 UPDATEDDATE = amsexc.updt_dt

             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }

        /// <summary>
        /// Returns all Exc Non Bil details that match criteria
        /// </summary>
        /// <returns></returns>
        public IList<ExcessNonBillableBE> getExcNonBillableDataLoss(int armsID)
        {
            IList<ExcessNonBillableBE> result = new List<ExcessNonBillableBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Excess Non Billable Details
            /// and project it into  Excess Non Billable Details Business Entity
            IQueryable<ExcessNonBillableBE> query =
            (from amsexc in this.Context.ARMIS_LOS_EXCs
             join ams in this.Context.ARMIS_LOS_POLs
             on amsexc.armis_los_pol_id equals ams.armis_los_pol_id
             join pol in this.Context.COML_AGMTs
             on ams.coml_agmt_id equals pol.coml_agmt_id
             //join lk in this.Context.LKUPs
             //on pol.aloc_los_adj_exps_typ_id equals lk.lkup_id
             where ams.armis_los_pol_id == armsID 
             select new ExcessNonBillableBE()
             {
                 ARMIS_LOS_EXC_ID = amsexc.armis_los_exc_id,
                 ARMIS_LOS_ID = amsexc.armis_los_pol_id,
                 COML_AGMT_ID = amsexc.coml_agmt_id,
                 CUSTMR_ID = amsexc.custmr_id,
                 ORGIN_CLAIM_NBR_TXT = amsexc.orgin_clm_nbr_txt,
                 CLAIM_NBR_TXT = amsexc.clm_nbr_txt,
                 LIMIT2_AMT = amsexc.lim2_amt,
                 ADD_CLAIM_TXT = amsexc.addn_clm_txt,
                 SITE_CD_TXT = amsexc.site_cd_txt,
                 COVG_TRIGGER_DATE = amsexc.covg_trigr_dt,
                 CLAIMANT_NM = amsexc.clmt_nm,
                 REOPEN_CLAIMANT_NBR_TXT = amsexc.reop_clm_nbr_txt,
                 PAID_IDNMTY_AMT = amsexc.paid_idnmty_amt,
                 PAID_EXPS_AMT = amsexc.paid_exps_amt,
                 RESRV_IDNMTY_AMT = amsexc.resrvd_idnmty_amt,
                 RESRV_EXPS_AMT = amsexc.resrvd_exps_amt,
                 NON_BILABL_PAID_IDNMTY_AMT = amsexc.non_bilabl_paid_idnmty_amt,
                 NON_BILABL_PAID_EXPS_AMT = amsexc.non_bilabl_paid_exps_amt,
                 NON_BILABL_RESRV_IDNMTY_AMT = amsexc.non_bilabl_resrvd_idnmty_amt,
                 NON_BILABL_RESRV_EXPS_AMT = amsexc.non_bilabl_resrvd_exps_amt,
                 SUBJ_PAID_IDNMTY_AMT = amsexc.subj_paid_idnmty_amt,
                 SUBJ_PAID_EXPS_AMT = amsexc.subj_paid_exps_amt,
                 SUBJ_RESRV_IDNMTY_AMT = amsexc.subj_resrvd_idnmty_amt,
                 SUBJ_RESRV_EXPS_AMT = amsexc.subj_resrvd_exps_amt,
                 EXC_PAID_IDNMTY_AMT = amsexc.exc_paid_idnmty_amt,
                 EXC_PAID_EXPS_AMT = amsexc.exc_paid_exps_amt,
                 EXC_RESRV_IDNMTY_AMT = amsexc.exc_resrvd_idnmty_amt,
                 EXC_RESRV_EXPS_AMT = amsexc.exc_resrvd_exps_amt,
                 SYS_GENRT_IND = amsexc.sys_genrt_ind,
                 ACTV_IND = amsexc.actv_ind,
                 ADDN_CLAIMS = amsexc.addn_clm_ind,
                 ALAE_CAP = pol.aloc_los_adj_exps_capped_amt,
                 POLICY_AMT = pol.dedtbl_pol_lim_amt,
                 CREATEDATE=amsexc.crte_dt,
                 ALAE_TYP = (from lk in this.Context.LKUPs
                             where pol.aloc_los_adj_exps_typ_id == lk.lkup_id
                             select lk.lkup_txt).First().ToString(),
                 CLAIMSTATUS = (from lk in this.Context.LKUPs
                                where amsexc.clm_sts_id == lk.lkup_id
                                select lk.lkup_txt).First().ToString(),
                 CLAIM_STS_ID = amsexc.clm_sts_id,
                 UPDATEDDATE=amsexc.updt_dt

             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
        /// <summary>
        /// Returns all Exc Non Bil details that match criteria
        /// </summary>
        /// <returns></returns>
        public IList<ExcessNonBillableBE> getExcNonBillableDataHideDisLines(int armsID, int comAmgtID, int custmrID, int prgID, bool flag)
        {
            IList<ExcessNonBillableBE> result = new List<ExcessNonBillableBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Excess Non Billable Details
            /// and project it into  Excess Non Billable Details Business Entity
            IQueryable<ExcessNonBillableBE> query =
            (from amsexc in this.Context.ARMIS_LOS_EXCs
             join ams in this.Context.ARMIS_LOS_POLs
             on amsexc.armis_los_pol_id equals ams.armis_los_pol_id
             join pol in this.Context.COML_AGMTs
             on ams.coml_agmt_id equals pol.coml_agmt_id
             //join lk in this.Context.LKUPs
             //on pol.aloc_los_adj_exps_typ_id equals lk.lkup_id
             where ams.armis_los_pol_id == armsID && ams.coml_agmt_id == comAmgtID && ams.custmr_id == custmrID && ams.prem_adj_pgm_id == prgID && amsexc.actv_ind == true
             orderby amsexc.clm_nbr_txt, amsexc.clmt_nm.Trim() ascending
             select new ExcessNonBillableBE()
             {
                 ARMIS_LOS_EXC_ID = amsexc.armis_los_exc_id,
                 ARMIS_LOS_ID = amsexc.armis_los_pol_id,
                 COML_AGMT_ID = amsexc.coml_agmt_id,
                 CUSTMR_ID = amsexc.custmr_id,
                 ORGIN_CLAIM_NBR_TXT = amsexc.orgin_clm_nbr_txt,
                 CLAIM_NBR_TXT = amsexc.clm_nbr_txt,
                 LIMIT2_AMT = amsexc.lim2_amt,
                 ADD_CLAIM_TXT = amsexc.addn_clm_txt,
                 SITE_CD_TXT = amsexc.site_cd_txt,
                 COVG_TRIGGER_DATE = amsexc.covg_trigr_dt,
                 CLAIMANT_NM = amsexc.clmt_nm,
                 REOPEN_CLAIMANT_NBR_TXT = amsexc.reop_clm_nbr_txt,
                 PAID_IDNMTY_AMT = amsexc.paid_idnmty_amt,
                 PAID_EXPS_AMT = amsexc.paid_exps_amt,
                 RESRV_IDNMTY_AMT = amsexc.resrvd_idnmty_amt,
                 RESRV_EXPS_AMT = amsexc.resrvd_exps_amt,
                 NON_BILABL_PAID_IDNMTY_AMT = amsexc.non_bilabl_paid_idnmty_amt,
                 NON_BILABL_PAID_EXPS_AMT = amsexc.non_bilabl_paid_exps_amt,
                 NON_BILABL_RESRV_IDNMTY_AMT = amsexc.non_bilabl_resrvd_idnmty_amt,
                 NON_BILABL_RESRV_EXPS_AMT = amsexc.non_bilabl_resrvd_exps_amt,
                 SUBJ_PAID_IDNMTY_AMT = amsexc.subj_paid_idnmty_amt,
                 SUBJ_PAID_EXPS_AMT = amsexc.subj_paid_exps_amt,
                 SUBJ_RESRV_IDNMTY_AMT = amsexc.subj_resrvd_idnmty_amt,
                 SUBJ_RESRV_EXPS_AMT = amsexc.subj_resrvd_exps_amt,
                 EXC_PAID_IDNMTY_AMT = amsexc.exc_paid_idnmty_amt,
                 EXC_PAID_EXPS_AMT = amsexc.exc_paid_exps_amt,
                 EXC_RESRV_IDNMTY_AMT = amsexc.exc_resrvd_idnmty_amt,
                 EXC_RESRV_EXPS_AMT = amsexc.exc_resrvd_exps_amt,
                 SYS_GENRT_IND = amsexc.sys_genrt_ind,
                 ACTV_IND = amsexc.actv_ind,
                 ADDN_CLAIMS = amsexc.addn_clm_ind,
                 ALAE_CAP = pol.aloc_los_adj_exps_capped_amt,
                 POLICY_AMT = pol.dedtbl_pol_lim_amt,
                 ALAE_TYP = (from lk in this.Context.LKUPs
                             where pol.aloc_los_adj_exps_typ_id == lk.lkup_id
                             select lk.lkup_txt).First().ToString(),
                 CLAIMSTATUS = (from lk in this.Context.LKUPs
                                where amsexc.clm_sts_id == lk.lkup_id
                                select lk.lkup_txt).First().ToString(),
                 CLAIM_STS_ID = amsexc.clm_sts_id,
                 UPDATEDDATE=amsexc.updt_dt

             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
        /// <summary>
        ///  Returns Exc Non Bil details based on Armis Loss Exc ID
        /// </summary>
        /// <param name="intarmsLossExcID"></param>
        /// <returns></returns>
        public IList<ExcessNonBillableBE> getExcNonBillableData(int intarmsLossExcID)
        {
            IList<ExcessNonBillableBE> result = new List<ExcessNonBillableBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Excess Non Billable Details
            /// and project it into  Excess Non Billable Details Business Entity
            IQueryable<ExcessNonBillableBE> query =
            (from amsexc in this.Context.ARMIS_LOS_EXCs
             join ams in this.Context.ARMIS_LOS_POLs
             on amsexc.armis_los_pol_id equals ams.armis_los_pol_id
             join pol in this.Context.COML_AGMTs
             on ams.coml_agmt_id equals pol.coml_agmt_id
             //join lk in this.Context.LKUPs
             //on pol.aloc_los_adj_exps_typ_id equals lk.lkup_id
             where amsexc.armis_los_exc_id == intarmsLossExcID
             select new ExcessNonBillableBE()
             {
                 ARMIS_LOS_EXC_ID = amsexc.armis_los_exc_id,
                 ARMIS_LOS_ID = amsexc.armis_los_pol_id,
                 COML_AGMT_ID = amsexc.coml_agmt_id,
                 CUSTMR_ID = amsexc.custmr_id,
                 ORGIN_CLAIM_NBR_TXT = amsexc.orgin_clm_nbr_txt,
                 CLAIM_NBR_TXT = amsexc.clm_nbr_txt,
                 LIMIT2_AMT = amsexc.lim2_amt,
                 ADD_CLAIM_TXT = amsexc.addn_clm_txt,
                 SITE_CD_TXT = amsexc.site_cd_txt,
                 COVG_TRIGGER_DATE = amsexc.covg_trigr_dt,
                 CLAIMANT_NM = amsexc.clmt_nm,
                 REOPEN_CLAIMANT_NBR_TXT = amsexc.reop_clm_nbr_txt,
                 PAID_IDNMTY_AMT = amsexc.paid_idnmty_amt,
                 PAID_EXPS_AMT = amsexc.paid_exps_amt,
                 RESRV_IDNMTY_AMT = amsexc.resrvd_idnmty_amt,
                 RESRV_EXPS_AMT = amsexc.resrvd_exps_amt,
                 NON_BILABL_PAID_IDNMTY_AMT = amsexc.non_bilabl_paid_idnmty_amt,
                 NON_BILABL_PAID_EXPS_AMT = amsexc.non_bilabl_paid_exps_amt,
                 NON_BILABL_RESRV_IDNMTY_AMT = amsexc.non_bilabl_resrvd_idnmty_amt,
                 NON_BILABL_RESRV_EXPS_AMT = amsexc.non_bilabl_resrvd_exps_amt,
                 SUBJ_PAID_IDNMTY_AMT = amsexc.subj_paid_idnmty_amt,
                 SUBJ_PAID_EXPS_AMT = amsexc.subj_paid_exps_amt,
                 SUBJ_RESRV_IDNMTY_AMT = amsexc.subj_resrvd_idnmty_amt,
                 SUBJ_RESRV_EXPS_AMT = amsexc.subj_resrvd_exps_amt,
                 EXC_PAID_IDNMTY_AMT = amsexc.exc_paid_idnmty_amt,
                 EXC_PAID_EXPS_AMT = amsexc.exc_paid_exps_amt,
                 EXC_RESRV_IDNMTY_AMT = amsexc.exc_resrvd_idnmty_amt,
                 EXC_RESRV_EXPS_AMT = amsexc.exc_resrvd_exps_amt,
                 SYS_GENRT_IND = amsexc.sys_genrt_ind,
                 ACTV_IND = amsexc.actv_ind,
                 ADDN_CLAIMS = amsexc.addn_clm_ind,
                 POLICY_AMT = pol.dedtbl_pol_lim_amt,
                 CREATEDATE = amsexc.crte_dt,
                 UPDATEDDATE=amsexc.updt_dt,
                 ALAE_TYP = (from lk in this.Context.LKUPs
                             where pol.aloc_los_adj_exps_typ_id == lk.lkup_id
                             select lk.lkup_txt).First().ToString(),
                 TOTAL_INCURRED = (amsexc.paid_idnmty_amt==null?0:amsexc.paid_idnmty_amt) + (amsexc.paid_exps_amt==null?0:amsexc.paid_exps_amt) + (amsexc.resrvd_idnmty_amt==null?0:amsexc.resrvd_idnmty_amt) + (amsexc.resrvd_exps_amt==null?0:amsexc.resrvd_exps_amt),
                 NON_BILABL_INCURRED = (amsexc.non_bilabl_paid_idnmty_amt==null?0:amsexc.non_bilabl_paid_idnmty_amt) + (amsexc.non_bilabl_paid_exps_amt==null?0:amsexc.non_bilabl_paid_exps_amt) + (amsexc.non_bilabl_resrvd_exps_amt==null?0:amsexc.non_bilabl_resrvd_exps_amt) + (amsexc.non_bilabl_resrvd_idnmty_amt==null?0:amsexc.non_bilabl_resrvd_idnmty_amt),
                 SUBJ_INCURRED = (amsexc.subj_paid_idnmty_amt==null?0:amsexc.subj_paid_idnmty_amt) + (amsexc.subj_paid_exps_amt==null?0:amsexc.subj_paid_exps_amt) + (amsexc.subj_resrvd_exps_amt==null?0:amsexc.subj_resrvd_exps_amt) + (amsexc.subj_resrvd_idnmty_amt==null?0:amsexc.subj_resrvd_idnmty_amt),
                 EXC_INCURRED = (amsexc.exc_paid_idnmty_amt==null?0:amsexc.exc_paid_idnmty_amt) + (amsexc.exc_paid_exps_amt==null?0:amsexc.exc_paid_exps_amt) + (amsexc.exc_resrvd_exps_amt==null?0:amsexc.exc_resrvd_exps_amt) + (amsexc.exc_resrvd_idnmty_amt==null?0:amsexc.exc_resrvd_idnmty_amt)

             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }
        /// <summary>
        /// This method invokes a stored procedure which performs limiting
        /// of Excess and Nonbillable data and then updates the subject paid
        /// amount in the ARMIS_LOS_POL table
        /// </summary>
        /// <param name="CustmrID"></param>
        /// <param name="PremiumAdjustProgID"></param>
        /// <returns></returns>
        public bool PerformLimiting(int CustmrID, int PremiumAdjustProgID)
        {

            if (this.Context == null)
                this.Initialize();

            int intResult = this.Context.ModAISLossLimitExcess(CustmrID, PremiumAdjustProgID);
            return true;

        }


        
    }
}
