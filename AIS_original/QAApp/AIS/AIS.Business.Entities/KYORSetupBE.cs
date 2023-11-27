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
    public class KYORSetupBE : BusinessEntity<KY_OR_SETUP>
    {
        public KYORSetupBE()
            : base()
        {

        }

        public int KY_OR_SETUP_ID { get { return Entity.ky_or_setup_id; } set { Entity.ky_or_setup_id = value; } }
        public DateTime? EFF_DT { get { return Entity.eff_dt; } set { Entity.eff_dt = value; } }
        public Decimal? KY_FCTR_RT { get { return Entity.ky_fctr_rt; } set { Entity.ky_fctr_rt = value; } }
        public Decimal? OR_FCTR_RT { get { return Entity.or_fctr_rt; } set { Entity.or_fctr_rt = value; } }
        public int? UPDT_USER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDT_DT { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CRTE_USER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CRTE_DT { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
    }
}
