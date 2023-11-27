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
    public class PremiumAdjustmetCommentBE : BusinessEntity<PREM_ADJ_CMMNT>
    {
        public int PremumAdj_Commnt_ID { get { return Entity.prem_adj_cmmnt_id; } set { Entity.prem_adj_cmmnt_id = value; } }
        public int? Commnt_Catg_ID { get { return Entity.cmmnt_catg_id; } set { Entity.cmmnt_catg_id = value; } }
        public String CommentText { get { return Entity.cmmnt_txt; } set { Entity.cmmnt_txt = value; } }
        public DateTime CreatedDate { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int CreatedUser_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime? UpdatedDate { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int? UpdatedUser_ID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
    }

}