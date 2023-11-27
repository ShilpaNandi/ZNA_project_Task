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
    public class PremiumAdjCombElementsBE : BusinessEntity<PREM_ADJ_COMB_ELEMT>
    {
        public PremiumAdjCombElementsBE()
            : base()
        { }

        public int PREM_ADJ_COMB_ELEMNTS_ID { get { return Entity.prem_adj_comb_elemts_id; } set { Entity.prem_adj_comb_elemts_id = value; } }
        public int PREM_ADJ_PERD_ID { get { return Entity.prem_adj_perd_id; } set { Entity.prem_adj_perd_id = value; } }
        public int PREM_ADJ_ID { get { return Entity.prem_adj_id; } set { Entity.prem_adj_id = value; } }
        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public decimal? RETRO_BASIC_PREM_AMT { get { return Entity.retro_basic_prem_amt; } set { Entity.retro_basic_prem_amt = value; } }
        public decimal? RETRO_LOS_FCTR_AMT { get { return Entity.retro_los_fctr_amt; } set { Entity.retro_los_fctr_amt = value; } }
        public decimal? RETRO_TAX_MULTI_RT { get { return Entity.retro_tax_multi_rt; } set { Entity.retro_tax_multi_rt = value; } }
        public decimal? RETRO_SUBTOT_AMT { get { return Entity.retro_subtot_amt; } set { Entity.retro_subtot_amt = value; } }
        public decimal? DEDTBL_MAX_AMT { get { return Entity.dedtbl_max_amt; } set { Entity.dedtbl_max_amt = value; } }
        public decimal? DEDTBL_MIN_AMT { get { return Entity.dedtbl_min_amt; } set { Entity.dedtbl_min_amt = value; } }
        public decimal? DEDTBL_SUBTOT_AMT { get { return Entity.dedtbl_subtot_amt; } set { Entity.dedtbl_subtot_amt = value; } }
        public decimal? DEDTBL_MAX_LESS_AMT { get { return Entity.dedtbl_max_less_amt; } set { Entity.dedtbl_max_less_amt = value; } }

        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
//        public int? COMB_ELEMTS_ID { get { return Entity.comb_elemts_id; } set { Entity.comb_elemts_id = value; } }
//        public int? COML_AGMT_ID { get { return Entity.coml_agmt_id; } set { Entity.coml_agmt_id = value; } }
//        public int? PREM_ADJ_PGM_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
    }
}
