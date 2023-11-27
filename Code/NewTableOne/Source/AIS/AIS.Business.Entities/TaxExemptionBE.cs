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
    public class TaxExemptionBE : BusinessEntity<TAX_EXMP_SETUP>
    {
        public TaxExemptionBE()
        {

        }
      
        public int TAX_EXMP_SETUP_ID { get { return Entity.tax_exmp_setup_id; } set { Entity.tax_exmp_setup_id = value; } }
        public int ST_ID { get { return Entity.st_id; } set { Entity.st_id = value; } }
        public int PREM_ADJ_PGM_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int CUSTOMER_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public Boolean? ACTV_IND { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }

        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }

        public string STATE_NAME { get; set; }
        
    }
}
