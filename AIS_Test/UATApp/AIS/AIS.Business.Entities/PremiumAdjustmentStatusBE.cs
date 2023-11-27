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
    public class PremiumAdjustmentStatusBE : BusinessEntity<PREM_ADJ_ST>
    {
        public PremiumAdjustmentStatusBE()
            : base()
        {
        }

        public int PremumAdj_sts_ID { get { return Entity.prem_adj_sts_id; } set { Entity.prem_adj_sts_id = value; } }
        public int PremumAdj_ID { get { return Entity.prem_adj_id; } set { Entity.prem_adj_id = value; } }
        public DateTime? Review_Cmplt_Date { get { return Entity.qlty_cntrl_dt; } set { Entity.qlty_cntrl_dt = value; } }
        public int? ReviewPerson_ID { get { return Entity.qlty_cntrl_pers_id; } set { Entity.qlty_cntrl_pers_id = value; } }
        public DateTime CreatedDate { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int CreatedUser_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public int CustomerID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public DateTime? UpdtDate { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int? UpdtUserID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } } 
        public DateTime EffectiveDate { get { return Entity.eff_dt; } set { Entity.eff_dt = value; } }
        public DateTime? EXPIRYDATE { get { return Entity.expi_dt; } set { Entity.expi_dt = value; } }
        //public int? QltyCntrlCommentID { get { return 1; } set {  } }
        public String CommentText { get { return Entity.cmmnt_txt; } set { Entity.cmmnt_txt = value; } }
        public string PersonName { get; set; }
        public int? COMMENTID{ get; set; }
        public string PremiumAdjStatus { get; set; }
        public bool? APPROVEINDICATOR { get { return Entity.aprv_ind; } set {  Entity.aprv_ind=value; } }
        public int? ADJ_STS_TYP_ID { get { return Entity.adj_sts_typ_id; } set {  Entity.adj_sts_typ_id=value; } }

    }

}