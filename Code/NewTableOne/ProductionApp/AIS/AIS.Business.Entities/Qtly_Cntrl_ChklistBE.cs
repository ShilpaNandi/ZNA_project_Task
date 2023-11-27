using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;
using System.Data.Linq;

namespace ZurichNA.AIS.Business.Entities
{
    public class Qtly_Cntrl_ChklistBE : BusinessEntity<QLTY_CNTRL_LIST>
    {
        public Qtly_Cntrl_ChklistBE()
            : base()
        {

        }
        public int QualityControlChklst_ID { get { return Entity.qlty_cntrl_list_id; } set { Entity.qlty_cntrl_list_id = value; } }
        public int? PremumAdj_Pgm_ID { get { return Entity.prem_adj_pgm_id; } set { Entity.prem_adj_pgm_id = value; } }
        public int CHECKLISTITEM_ID { get { return Entity.chk_list_itm_id; } set { Entity.chk_list_itm_id = value; } }
        public int? QUALITYCONTROLTYPE_ID { get; set; }
        public int? PREMIUMADJ_STATUS_ID { get { return Entity.prem_adj_sts_id; } set { Entity.prem_adj_sts_id = value; } }
        public int? PREMIUMADJ_ARIES_CLR_ID { get { return Entity.prem_adj_aries_clring_id; } set { Entity.prem_adj_aries_clring_id = value; } }
        public int? CUSTOMER_ID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int? PREMIUMADJUSTMENT_ID { get { return Entity.prem_adj_id; } set { Entity.prem_adj_id = value; } }
        public string CHKLIST_STS_CD { get { return Entity.chklist_sts_cd; } set { Entity.chklist_sts_cd = value; } }
        public Boolean? ACTIVE { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public DateTime CreatedDate { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public int CreatedUser_ID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime? UpdatedDate { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int? UpdatedUserID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public int? CUST_RELID { get { return Entity.custmr_rel_id; } set { Entity.custmr_rel_id = value; } }
        public int LOOKUPID { get; set; }
        public string CHKLISTNAME { get; set; }
        public Boolean? ENABLED { get; set; }
        public string ChkLstItems { get; set; }
        public string AdjChkLstItems { get; set; }
        public string AccountName { get; set; }
        public string Reg_AccountName { get; set; }
        public DateTime? ProgramPeriodStDate { get; set; }
        public DateTime? ProgramPeriodEndDate { get; set; }
    }
}
