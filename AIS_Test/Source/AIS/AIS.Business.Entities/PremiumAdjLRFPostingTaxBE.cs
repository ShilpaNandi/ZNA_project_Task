using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;
using log4net;

namespace ZurichNA.AIS.Business.Entities
{
    public class PremiumAdjLRFPostingTaxBE : BusinessEntity<PREM_ADJ_LOS_REIM_FUND_POST_TAX>
    {
        public PremiumAdjLRFPostingTaxBE()
            : base()
        {

        }
        public int PREM_ADJ_LOS_REIM_FUND_POST_TAX_ID { get { return Entity.prem_adj_los_reim_fund_post_tax_id; } set { Entity.prem_adj_los_reim_fund_post_tax_id = value; } }
        public int PREM_ADJ_PERD_ID { get { return Entity.prem_adj_perd_id; } set { Entity.prem_adj_perd_id = value; } }
        public int PREM_ADJ_ID { get { return Entity.prem_adj_id; } set { Entity.prem_adj_id = value; } }
        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int? TAX_TYP_ID { get { return Entity.tax_typ_id; } set { Entity.tax_typ_id = value; } }
        public int? ST_ID { get { return Entity.st_id; } set { Entity.st_id = value; } }
        public decimal? CURR_AMT { get { return Entity.curr_amt; } set { Entity.curr_amt = value; } }
        public decimal? AGGR_AMT { get { return Entity.aggr_amt; } set { Entity.aggr_amt = value; } }
        public decimal? LIM_AMT { get { return Entity.lim_amt; } set { Entity.lim_amt = value; } }
        public decimal? PRIOR_YY_AMT { get { return Entity.prior_yy_amt; } set { Entity.prior_yy_amt = value; } }
        public decimal? ADJ_PRIOR_YY_AMT { get { return Entity.adj_prior_yy_amt; } set { Entity.adj_prior_yy_amt = value; } }
        public decimal? POST_AMT { get { return Entity.post_amt; } set { Entity.post_amt = value; } }
        public int? UPDT_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDT_DT { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public DateTime  CRTE_DT { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int CRTE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        private string _taxType;
        public string TAXTYPE
        {
            get
            {
                return _taxType;
            }
            set
            {
                _taxType = value;
            }
        }
        //Texas Tax: Line of business 
        public int? LN_OF_BSN_ID { get { return Entity.ln_of_bsn_id; } set { Entity.ln_of_bsn_id = value; } }
        private string _lineOfBusiness;
        public string LN_OF_BSN_TXT
        {
            get
            {
                return _lineOfBusiness;
            }
            set
            {
                _lineOfBusiness = value;
            }
        }
    }
}
