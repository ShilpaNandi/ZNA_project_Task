using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.AIS.Business.Entities
{
    public class CustomerCommentsBE : BusinessEntity<CUSTMR_CMMNT>
    {
        public DateTime CommentDate { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int CommentBY { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public int CommentID { get { return Entity.custmr_cmmnt_id; } set { Entity.custmr_cmmnt_id = value; } }
        public int CustomerID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int CommentCategoryID { get { return Entity.cmmnt_catg_id; } set { Entity.cmmnt_catg_id = value; } }
        public string CommentText { get { return Entity.cmmnt_txt; } set { Entity.cmmnt_txt = value; } }
        public DateTime? CommntUpdatedDate { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int?CommntUpdatedUserID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public string CommentCategoryName { get; set; }
        public string CommentUserName { get; set; }
        //public int CommentID { get { return Entity.custmr_cmmnt_id; } set { Entity.custmr_cmmnt_id = value; } }
        //public int? MasterEarnedRetroPremiumFormulaID { get { return Entity.mstr_ernd_retro_prem_frmla_id; } set { Entity.mstr_ernd_retro_prem_frmla_id = value; } }


    }
}
