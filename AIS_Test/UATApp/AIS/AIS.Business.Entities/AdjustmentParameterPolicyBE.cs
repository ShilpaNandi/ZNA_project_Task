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
    public class AdjustmentParameterPolicyBE : BusinessEntity<PREM_ADJ_PGM_SETUP_POL>
    {
         public AdjustmentParameterPolicyBE()
            : base()
        {

        }
         public int adj_paramet_pol_id { get { return Entity.prem_adj_pgm_setup_pol_id; } set { Entity.prem_adj_pgm_setup_pol_id = value; } }
        public int adj_paramet_setup_id { get { return Entity.prem_adj_pgm_setup_id; } set { Entity.prem_adj_pgm_setup_id = value; } }
        public int coml_agmt_id { get { return Entity.coml_agmt_id; } set { Entity.coml_agmt_id = value; } }
        public int custmrID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int? UPDATE_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDATE_DATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CREATE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATE_DATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int PrmadjPRgmID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }

        private string _PolicyPerfectNumber;
        public string PolicyPerfectNumber { get { return _PolicyPerfectNumber; } set { _PolicyPerfectNumber = value; } }


    }
}
