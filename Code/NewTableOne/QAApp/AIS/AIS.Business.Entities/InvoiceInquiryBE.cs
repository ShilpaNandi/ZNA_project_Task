using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.AIS.Business.Entities
{
    public class InvoiceInquiryBE : BusinessEntity<PREM_ADJ>
    {
        public InvoiceInquiryBE()
            : base()
        {
            //base.AutoValidate = true;
        }
        public int INVOICE_INQUIRY_NO { get; set; }
        public int? ACCOUNT_NUMBER { get; set; }
        public int? PREM_ADJ_ID { get; set; }
        public string ACCOUNT_NAME { get; set; }
        public int? PERSON_ID { get; set; }
        public int? ROLE_ID { get; set; }
        public string INSURED_CONTACT { get; set; }
        public string CFS2_NAME { get; set; }
        public DateTime? VALUATION_DATE { get; set; }
        public string VAL_DATE { get; set; }
        public int? BROKER_ID { get; set; }
        public string BROKER { get; set; }
        public string BROKER_CONTACT { get; set; }
        public string BU_OFFICE { get; set; }
        public string INVOICE_NUMBER { get; set; }
        public string FNL_INVOICE_NUMBER { get; set; }
        public string DRFT_INVOICE_NUMBER { get; set; }
        public DateTime? INVOICE_DATE { get; set; }
        public string INVOICEDATE { get; set; }
        public decimal? INVOICE_AMOUNT { get; set; }
        public string ADJUSTMENT_STATUS { get; set; }
        public string POLICY_NO { get; set; }
        public DateTime? ADJUSTMENT_DATE { get; set; }
        public string ADJUSTMENTDATE { get; set; }
        public DateTime? QCD_DATE { get; set; }
        public DateTime? DRAFT_INVOICE_DATE { get; set; }
        public DateTime? FINAL_INVOICE_DATE { get; set; }
        public string DRAFT_INVOICEDATE { get; set; }
        public string FINAL_INVOICEDATE { get; set; }
        public int CUSTOMER_ID { get; set; }
        public int? PROGRAMTYP_ID { get; set; }
        public int? EXTORGID { get; set; }
        public int? INTORGID { get; set; }
        public string FNL_INTERNAL_KEY { get; set; }
        public string FNL_EXTERNAL_KEY { get; set; }
        public string FNL_CW_KEY { get; set; }
        public string DRAFT_INTERNAL_KEY { get; set; }
        public string DRAFT_EXTERNAL_KEY { get; set; }
        public string DRAFT_CW_KEY { get; set; }
        public DateTime? PGM_EFF_DATE { get; set; }
        public DateTime? PGM_EXP_DATE { get; set; }
        public string PROGRAMTYP { get; set; }

        //This following variables are added in Reports Driver. So, Please don't remove.
        public int PREMADJID { get { return Entity.prem_adj_id; } set { Entity.prem_adj_id = value; } }
        public int CUSTMERID { get { return Entity.reg_custmr_id; } set { Entity.reg_custmr_id = value; } }
        public DateTime VALUATIONDATE { get { return Entity.valn_dt; } set { Entity.valn_dt = value; } }
        //public string DRAFTINVTXT { get { return Entity.drft_invc_nbr_txt == null ? "" : Entity.drft_invc_nbr_txt; } set { Entity.drft_invc_nbr_txt = value; } }
        public string DRAFTINVTXT { get { return Entity.drft_invc_nbr_txt; } set { Entity.drft_invc_nbr_txt = value; } }
        public DateTime? DRAFTINVDATE { get { return Entity.drft_invc_dt; } set { Entity.drft_invc_dt = value; } }
        public DateTime? DRAFTMAILEDUWDT { get { return Entity.drft_mailed_undrwrt_dt; } set { Entity.drft_mailed_undrwrt_dt = value; } }
        public string FINALINVTXT { get { return Entity.fnl_invc_nbr_txt == null ? "" : Entity.fnl_invc_nbr_txt; } set { Entity.fnl_invc_nbr_txt = value; } }
        public DateTime? FINALINVDT { get { return Entity.fnl_invc_dt; } set { Entity.fnl_invc_dt = value; } }
        public DateTime? FINALMAILEDUWDT { get { return Entity.fnl_mailed_undrwrt_dt; } set { Entity.fnl_mailed_undrwrt_dt = value; } }
        public DateTime? INVDUEDT { get { return Entity.invc_due_dt; } set { Entity.invc_due_dt = value; } }
        public bool? TWENTYPCTQLTYCNTRLIND { get { return Entity.twenty_pct_qlty_cntrl_ind; } set { Entity.twenty_pct_qlty_cntrl_ind = value; } }
        public int? TWENTYPCTQLTYCNTRLPERSID { get { return Entity.twenty_pct_qlty_cntrl_pers_id; } set { Entity.twenty_pct_qlty_cntrl_pers_id = value; } }
        public DateTime? TWENTYPCTQLTYCNTRLDT { get { return Entity.twenty_pct_qlty_cntrl_dt; } set { Entity.twenty_pct_qlty_cntrl_dt = value; } }
//        public int? PREMADJCMMNTID { get { return Entity.prem_adj_cmmnt_id; } set { Entity.prem_adj_cmmnt_id = value; } }
//        public decimal? INVCAMT { get { return Entity.invc_amt; } set { Entity.invc_amt = value; } }
        public bool? ADJPENDNGIND { get { return Entity.adj_pendg_ind; } set { Entity.adj_pendg_ind = value; } }
        public int? ADJPENDRSNID { get { return Entity.adj_pendg_rsn_id; } set { Entity.adj_pendg_rsn_id = value; } }
        public int? ADJRSNRSNID { get { return Entity.adj_rrsn_rsn_id; } set { Entity.adj_rrsn_rsn_id = value; } }
        public int? ADJVOIDRSNID { get { return Entity.adj_void_rsn_id; } set { Entity.adj_void_rsn_id = value; } }
        public int? UPDATEUSERID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDATEDATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CREATEUSERID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATEDATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        //This following variables are added in Reports Driver. So, Please don't remove.

    }
}
