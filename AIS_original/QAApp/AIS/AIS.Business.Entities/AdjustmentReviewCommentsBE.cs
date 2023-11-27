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
    public class AdjustmentReviewCommentsBE : BusinessEntity<PREM_ADJ_CMMNT>
    {
        public AdjustmentReviewCommentsBE()
            : base()
        {

        }
        public int PREM_ADJ_CMMNT_ID { get { return Entity.prem_adj_cmmnt_id; } set { Entity.prem_adj_cmmnt_id = value; } }
        public int? CMMNT_CATG_ID { get { return Entity.cmmnt_catg_id; } set { Entity.cmmnt_catg_id = value; } }
        public int? CUSTMR_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int? PREM_ADJ_ID { get { return Entity.prem_adj_id; } set { Entity.prem_adj_id = value; } }
        public int? PREM_ADJ_PERD_ID { get { return Entity.prem_adj_perd_id; } set { Entity.prem_adj_perd_id = value; } }
        public string CMMNT_TXT { get { return Entity.cmmnt_txt; } set { Entity.cmmnt_txt = value; } }
        public int CREATEUSER { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public int? UPDATEDUSER { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime CREATEDATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public DateTime? UPDATEDDATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
       
        
    }
}
