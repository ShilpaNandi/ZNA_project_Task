using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.Linq;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;


namespace ZurichNA.AIS.Business.Entities
{
    public class PremiumAdjustmentEscrowBE : BusinessEntity<PREM_ADJ_PARMET_SETUP>
    {
        public PremiumAdjustmentEscrowBE()
            : base()
        { }

        public int PREM_ADJ_PARMET_SETUP_ID { get { return Entity.prem_adj_parmet_setup_id; } set { Entity.prem_adj_parmet_setup_id = value; } }
        public int PREM_ADJ_PERD_ID { get { return Entity.prem_adj_perd_id; } set { Entity.prem_adj_perd_id = value; } }
        public int PREM_ADJ_ID { get { return Entity.prem_adj_id; } set { Entity.prem_adj_id = value; } }
        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int PREM_ADJ_PGM_SETUP_ID { get { return Entity.prem_adj_pgm_setup_id; } set { Entity.prem_adj_pgm_setup_id = value; } }
        public int PREM_ADJ_PGM_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public decimal? LOS_BASE_ASSES_AMT { get { return Entity.los_base_asses_amt; } set { Entity.los_base_asses_amt = value; } }
        public decimal? LOS_BASE_ASSES_DEPST_AMT { get { return Entity.los_base_asses_depst_amt; } set { Entity.los_base_asses_depst_amt = value; } }
        public decimal? LOS_BASE_ASSES_PREV_BILED_AMT { get { return Entity.los_base_asses_prev_biled_amt; } set { Entity.los_base_asses_prev_biled_amt = value; } }
//        public int? PREM_ADJ_CMMNT_ID { get { return Entity.prem_adj_cmmnt_id; } set { Entity.prem_adj_cmmnt_id = value; } }
        public decimal? ESCR_PREVIOUSULY_BILED_AMT { get { return Entity.escr_prevly_biled_amt; } set { Entity.escr_prevly_biled_amt = value; } }
        public decimal? ESCR_ADJ_PAID_LOS_AMT { get { return Entity.escr_adj_paid_los_amt; } set { Entity.escr_adj_paid_los_amt = value; } }
        public decimal? ESCR_AMT { get { return Entity.escr_amt; } set { Entity.escr_amt = value; } }
        public decimal? ESCR_ADJ_AMT { get { return Entity.escr_adj_amt; } set { Entity.escr_adj_amt = value; } }
        public decimal? TOT_AMT { get { return Entity.tot_amt; } set { Entity.tot_amt = value; } }
        public decimal? INCUR_LOS_REIM_FUND_AMT { get { return Entity.incur_los_reim_fund_amt; } set { Entity.incur_los_reim_fund_amt = value; } }
        public decimal? INCUR_LOS_REIM_FUND_LIM_AMT { get { return Entity.incur_los_reim_fund_lim_amt; } set { Entity.incur_los_reim_fund_lim_amt = value; } }
        public decimal? INCUR_LOS_REIM_FUND_PREVIOUSLY_BILED_AMT { get { return Entity.incur_los_reim_fund_prevly_biled_amt; } set { Entity.incur_los_reim_fund_prevly_biled_amt = value; } }
        public int? ADJ_PARMET_TYP_ID { get { return Entity.adj_parmet_typ_id; } set { Entity.adj_parmet_typ_id = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
    }

}