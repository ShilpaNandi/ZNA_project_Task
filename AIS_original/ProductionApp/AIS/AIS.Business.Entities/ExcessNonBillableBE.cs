using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;
using System.Data.Linq;

namespace ZurichNA.AIS.Business.Entities
{
    public class ExcessNonBillableBE : BusinessEntity<ARMIS_LOS_EXC>
    {
        public ExcessNonBillableBE()
            : base()
        {

        }

        public int ARMIS_LOS_EXC_ID { get { return Entity.armis_los_exc_id; } set { Entity.armis_los_exc_id = value; } }
        public int ARMIS_LOS_ID { get { return Entity.armis_los_pol_id; } set { Entity.armis_los_pol_id = value; } }
        public int COML_AGMT_ID { get { return Entity.coml_agmt_id; } set { Entity.coml_agmt_id = value; } }
        public int PREM_ADJ_PGM_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public string ORGIN_CLAIM_NBR_TXT{ get { return Entity.orgin_clm_nbr_txt; } set { Entity.orgin_clm_nbr_txt = value; } }
        public string CLAIM_NBR_TXT { get { return Entity.clm_nbr_txt; } set { Entity.clm_nbr_txt = value; } }
        public Decimal? LIMIT2_AMT { get { return Entity.lim2_amt; } set { Entity.lim2_amt = value; } }
        public string ADD_CLAIM_TXT { get { return Entity.addn_clm_txt; } set { Entity.addn_clm_txt = value; } }
        public string SITE_CD_TXT { get { return Entity.site_cd_txt; } set { Entity.site_cd_txt = value; } }
        public DateTime? COVG_TRIGGER_DATE { get { return Entity.covg_trigr_dt; } set { Entity.covg_trigr_dt = value; } }
        public string CLAIMANT_NM { get { return Entity.clmt_nm; } set { Entity.clmt_nm = value; } }
        public string REOPEN_CLAIMANT_NBR_TXT { get { return Entity.reop_clm_nbr_txt; } set { Entity.reop_clm_nbr_txt = value; } }
        public Decimal? PAID_IDNMTY_AMT { get { return Entity.paid_idnmty_amt; } set { Entity.paid_idnmty_amt = value; } }
        public Decimal? PAID_EXPS_AMT { get { return Entity.paid_exps_amt; } set { Entity.paid_exps_amt = value; } }
        public Decimal? RESRV_IDNMTY_AMT { get { return Entity.resrvd_idnmty_amt; } set { Entity.resrvd_idnmty_amt = value; } }
        public Decimal? RESRV_EXPS_AMT { get { return Entity.resrvd_exps_amt; } set { Entity.resrvd_exps_amt = value; } }
        public Decimal? NON_BILABL_PAID_IDNMTY_AMT { get { return Entity.non_bilabl_paid_idnmty_amt; } set { Entity.non_bilabl_paid_idnmty_amt = value; } }
        public Decimal? NON_BILABL_PAID_EXPS_AMT { get { return Entity.non_bilabl_paid_exps_amt; } set { Entity.non_bilabl_paid_exps_amt = value; } }
        public Decimal? NON_BILABL_RESRV_IDNMTY_AMT { get { return Entity.non_bilabl_resrvd_idnmty_amt; } set { Entity.non_bilabl_resrvd_idnmty_amt = value; } }
        public Decimal? NON_BILABL_RESRV_EXPS_AMT { get { return Entity.non_bilabl_resrvd_exps_amt; } set { Entity.non_bilabl_resrvd_exps_amt = value; } }
        public Decimal? SUBJ_PAID_IDNMTY_AMT { get { return Entity.subj_paid_idnmty_amt; } set { Entity.subj_paid_idnmty_amt = value; } }
        public Decimal? SUBJ_PAID_EXPS_AMT { get { return Entity.subj_paid_exps_amt; } set { Entity.subj_paid_exps_amt = value; } }
        public Decimal? SUBJ_RESRV_IDNMTY_AMT { get { return Entity.subj_resrvd_idnmty_amt; } set { Entity.subj_resrvd_idnmty_amt = value; } }
        public Decimal? SUBJ_RESRV_EXPS_AMT { get { return Entity.subj_resrvd_exps_amt; } set { Entity.subj_resrvd_exps_amt = value; } }
        public Decimal? EXC_PAID_IDNMTY_AMT { get { return Entity.exc_paid_idnmty_amt; } set { Entity.exc_paid_idnmty_amt = value; } }
        public Decimal? EXC_PAID_EXPS_AMT { get { return Entity.exc_paid_exps_amt; } set { Entity.exc_paid_exps_amt = value; } }
        public Decimal? EXC_RESRV_IDNMTY_AMT { get { return Entity.exc_resrvd_idnmty_amt; } set { Entity.exc_resrvd_idnmty_amt = value; } }
        public Decimal? EXC_RESRV_EXPS_AMT { get { return Entity.exc_resrvd_exps_amt; } set { Entity.exc_resrvd_exps_amt = value; } }
        public Boolean? SYS_GENRT_IND { get { return Entity.sys_genrt_ind; } set { Entity.sys_genrt_ind = value; } }
        public Boolean? ACTV_IND { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public Boolean? ADDN_CLAIMS { get { return Entity.addn_clm_ind; } set { Entity.addn_clm_ind = value; } }
        public string POLICY { get; set; }
        public decimal? ALAE_CAP { get; set; }
        public decimal? POLICY_AMT { get; set; }
        public string ALAE_TYP { get; set; }
        public int CREATEUSER { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public int? UPDATEDUSER { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime CREATEDATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public DateTime? UPDATEDDATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public Decimal? TOTAL_INCURRED { get; set; }
        public Decimal? NON_BILABL_INCURRED { get; set; }
        public Decimal? SUBJ_INCURRED { get; set; }
        public Decimal? EXC_INCURRED { get; set; }
        public string CLAIMSTATUS { get; set; }
        public int? CLAIM_STS_ID { get { return Entity.clm_sts_id; } set { Entity.clm_sts_id = value; } }

    }
}
