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
    public class PremiumAdjustmentProgramStatusBE : BusinessEntity<PREM_ADJ_PGM_ST>
    {
         public int PREMUMADJPROG_STS_ID { get { return Entity.prem_adj_pgm_sts_id; } set { Entity.prem_adj_pgm_sts_id = value; } }
        public int PREM_ADJ_PGM_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }        
        public int? PGM_STATUS_ID { get { return Entity.pgm_perd_sts_typ_id; } set { Entity.pgm_perd_sts_typ_id = value; ;} }
        public bool? STS_CHK_IND { get { return Entity.sts_chk_ind; } set { Entity.sts_chk_ind = value; } }
        public DateTime? UPDATEDATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int? UPDATEUSER_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime CREATEDDATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int CREATEDUSER_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
    }
}
