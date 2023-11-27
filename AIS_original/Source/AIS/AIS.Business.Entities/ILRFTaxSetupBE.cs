using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.Linq;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;


//Texas Tax:Business Entity Class 
namespace ZurichNA.AIS.Business.Entities
{
    public class ILRFTaxSetupBE : BusinessEntity<INCUR_LOS_REIM_FUND_TAX_SETUP>
    {
        public ILRFTaxSetupBE()
        {

        }

        public int INCURRED_LOSS_REIM_FUND_TAX_ID { get { return Entity.incur_los_reim_fund_tax_id; } set { Entity.incur_los_reim_fund_tax_id = value; } }
        public int PREM_ADJ_PGM_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int CUSTOMER_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int PREM_ADJ_PGM_SETUP_ID { get { return Entity.prem_adj_pgm_setup_id; } set { Entity.prem_adj_pgm_setup_id = value; } }

        public int TAX_TYP_ID { get { return Entity.tax_typ_id; } set { Entity.tax_typ_id = value; } }
        public Decimal? TAX_AMT { get { return Entity.tax_amt; } set { Entity.tax_amt = value; } }
        public Boolean? ACTV_IND { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
       

        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }

        public string INCURRED_LOSS_REIM_FUND_TAX_TYPE { get; set; }
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
