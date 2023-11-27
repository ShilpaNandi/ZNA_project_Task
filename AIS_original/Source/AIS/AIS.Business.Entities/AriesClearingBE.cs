using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;
namespace ZurichNA.AIS.Business.Entities
{
  public class AriesClearingBE : BusinessEntity<PREM_ADJ_ARIES_CLRING>
    {
        public int PREMIUM_ADJUST_CLEARING_ID
        {
            get { return Entity.prem_adj_aries_clring_id; }
            set { Entity.prem_adj_aries_clring_id = value; }
        
        }
        public int PREMIUM_ADJUSTMENT_ID
        {
            get { return Entity.prem_adj_id; }
            set { Entity.prem_adj_id = value; }
        }

        public int CUSTOMER_ID
        { get { return Entity.custmr_id; }
            set { Entity.custmr_id = value; }
        }
        public int QULAITY_PERSON_ID
        { get { return Entity.qlty_cntrl_pers_id; }
            set { Entity.qlty_cntrl_pers_id = value; }
        }
        public DateTime? RECON_DUE_DATE
        {
            get { return Entity.recon_due_dt; }
            set { Entity.recon_due_dt = value; }
        }
        public DateTime? RECON_DATE
        {
            get { return Entity.recon_dt; }
            set { Entity.recon_dt = value; }
        
        }
        public DateTime? QUALITY_CONTROL_DATE
        {
            get { return Entity.qlty_cntrl_dt; }
            set { Entity.qlty_cntrl_dt = value; }
        }
        public DateTime? ARIES_POST_DATE
        {
            get { return Entity.aries_post_dt; }
            set { Entity.aries_post_dt = value; }
        }
        public string CHECK_NUMBER_TEXT
        {
            get { return Entity.chk_nbr_txt; }
            set { Entity.chk_nbr_txt = value; }
        }
        public decimal? ARIES_PAYMENT_AMOUNT
        {
            get { return Entity.aries_paymt_amt; }
            set { Entity.aries_paymt_amt = value; }
        }
        public DateTime? BILLED_ITEM_CLEAR_DATE
        {
            get { return Entity.biled_itm_clring_dt; }
            set { Entity.biled_itm_clring_dt = value; }
        }
        //public int? PREM_ADJUSTMENT_COMMENT_ID
        //{
        //    get { return Entity.prem_adj_cmmnt_id; }
        //    set { Entity.prem_adj_cmmnt_id  = value; }
        //}
        public bool? QUALITY_CTRL_IND
        {
            get { return Entity.qlty_cntrl_ind; }
            set { Entity.qlty_cntrl_ind = value; }
        }

        public int? UPDATED_USR_ID
        {
            get { return Entity.updt_user_id; }
            set { Entity.updt_user_id = value; }
        }
        public DateTime? UPDATED_DATE
        {
            get { return Entity.updt_dt; }
            set { Entity.updt_dt = value; }
        }
        public int CREATED_USER_ID
        {
            get { return Entity.crte_user_id; }
            set { Entity.crte_user_id = value; }
        }
        public DateTime CREATED_DATE
        {
            get { return Entity.crte_dt; }
            set { Entity.crte_dt = value; }
        }
        public bool? ARIES_COMPL_IND
        {
            get { return Entity.aries_cmplt_ind; }
            set { Entity.aries_cmplt_ind = value; }
        
        }

        public string COMMENTS_TEXT
        {
            get { return Entity.cmmnt_txt; }
            set { Entity.cmmnt_txt = value; }
        }

    }
    
}
