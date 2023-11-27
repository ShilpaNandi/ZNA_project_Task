using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;
using System.Data.Linq;

namespace ZurichNA.AIS.Business.Entities
{
    public class Coml_Agmt_AuDtBE : BusinessEntity<COML_AGMT_AUDT>
    {

        public Coml_Agmt_AuDtBE()
            : base()
        {
        }

        public int Comm_Agr_Audit_ID { get { return Entity.coml_agmt_audt_id; } set { Entity.coml_agmt_audt_id = value; } }
        public int Comm_Agr_ID { get { return Entity.coml_agmt_id; } set { Entity.coml_agmt_id = value; } }
        public int Prem_Adj_Prg_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public bool? AdjustmentIndicator { get { return Entity.adj_ind; } set { Entity.adj_ind = value; } }
        public int Customer_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public DateTime StartDate { get { return Entity.strt_dt; } set { Entity.strt_dt = value; } }
        public Decimal? Sub_Aud_Prm_Amt { get { return Entity.subj_audt_prem_amt; } set { Entity.subj_audt_prem_amt = value; } }
        public Decimal? Non_Sub_Aud_Prm_Amt { get { return Entity.non_subj_audt_prem_amt; } set { Entity.non_subj_audt_prem_amt = value; } }
        public Decimal? Def_Dep_prm_Amt { get { return Entity.defr_depst_prem_amt; } set { Entity.defr_depst_prem_amt = value; } }
        public Decimal? Sub_Dep_Prm_Amt { get { return Entity.subj_depst_prem_amt; } set { Entity.subj_depst_prem_amt = value; } }
        public Decimal? Non_Sub_Dep_Prm_Amt { get { return Entity.non_subj_depst_prem_amt; } set { Entity.non_subj_depst_prem_amt = value; } }
        public Decimal? Audit_Reslt_Amt { get { return Entity.audt_rslt_amt; } set { Entity.audt_rslt_amt = value; } }
        public Decimal? ExposureAmt { get { return Entity.expo_amt; } set { Entity.expo_amt = value; } }
        public bool? Aud_Rev_Status { get { return Entity.audt_revd_sts_ind; } set { Entity.audt_revd_sts_ind = value; } }
        public DateTime CreatedDate { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int CreatedUser_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime? UpdatedDate { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int? UpdatedUser_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public string POLICY { get; set; }
        public Boolean? Pol_Ind { get; set; }
    }
}
