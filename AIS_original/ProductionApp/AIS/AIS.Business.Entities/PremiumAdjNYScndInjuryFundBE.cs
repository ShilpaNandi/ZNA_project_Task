using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Data.Linq;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.AIS.Business.Entities
{
    public class PremiumAdjNYScndInjuryFundBE : BusinessEntity<PREM_ADJ_NY_SCND_INJR_FUND>
    {
        public PremiumAdjNYScndInjuryFundBE()
            : base()
        {

        }
        public int PREM_ADJ_NY_SCND_INJR_FUND_ID { get { return Entity.prem_adj_ny_scnd_injr_fund_id; } set { Entity.prem_adj_ny_scnd_injr_fund_id = value; } }
        public int PREM_ADJ_PERD_ID { get { return Entity.prem_adj_perd_id; } set { Entity.prem_adj_perd_id = value; } }
        public int PREM_ADJ_ID { get { return Entity.prem_adj_id; } set { Entity.prem_adj_id = value; } }
        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int? COML_AGMT_ID { get { return Entity.coml_agmt_id; } set { Entity.coml_agmt_id = value; } }
        public int? PREM_ADJ_PGM_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
//        public int? PREM_ADJ_CMMNT_ID { get { return Entity.prem_adj_cmmnt_id; } set { Entity.prem_adj_cmmnt_id = value; } }
        public decimal? INCUR_LOS_AMT { get { return Entity.incur_los_amt; } set { Entity.incur_los_amt = value; } }
        public decimal? LOS_CONV_FCTR_RT { get { return Entity.los_conv_fctr_rt; } set { Entity.los_conv_fctr_rt = value; } }
        public decimal? CNVT_LOS_AMT { get { return Entity.cnvt_los_amt; } set { Entity.cnvt_los_amt = value; } }
        public decimal? BASIC_DEDTBL_PREM_AMT { get { return Entity.basic_dedtbl_prem_amt; } set { Entity.basic_dedtbl_prem_amt = value; } }
        public decimal? TAX_MULTI_RT { get { return Entity.tax_multi_rt; } set { Entity.tax_multi_rt = value; } }
        public decimal? CNVT_TOT_LOS_AMT { get { return Entity.cnvt_tot_los_amt; } set { Entity.cnvt_tot_los_amt = value; } }
        public decimal? NY_PREM_DISC_AMT { get { return Entity.ny_prem_disc_amt; } set { Entity.ny_prem_disc_amt = value; } }
        public decimal? NY_SCND_INJR_FUND_RT { get { return Entity.ny_scnd_injr_fund_rt; } set { Entity.ny_scnd_injr_fund_rt = value; } }
        public decimal? REVD_NY_SCND_INJR_FUND_AMT { get { return Entity.revd_ny_scnd_injr_fund_amt; } set { Entity.revd_ny_scnd_injr_fund_amt = value; } }
        public decimal? NY_TAX_DUE_AMT { get { return Entity.ny_tax_due_amt; } set { Entity.ny_tax_due_amt = value; } }
        public decimal? PREV_RSLT_AMT { get { return Entity.prev_rslt_amt; } set { Entity.prev_rslt_amt = value; } }
        public decimal? NY_SCND_INJR_FUND_AUDT_AMT { get { return Entity.ny_scnd_injr_fund_audt_amt; } set { Entity.ny_scnd_injr_fund_audt_amt = value; } }
        public decimal? CURR_ADJ_AMT { get { return Entity.curr_adj_amt; } set { Entity.curr_adj_amt = value; } }
        public decimal? BASIC_CNVT_LOS_AMT { get { return Entity.basic_cnvt_los_amt; } set { Entity.basic_cnvt_los_amt = value; } }
        public decimal? NY_ERND_RETRO_PREM_AMT { get { return Entity.ny_ernd_retro_prem_amt; } set { Entity.ny_ernd_retro_prem_amt = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
    }
}
