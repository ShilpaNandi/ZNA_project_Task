using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.AIS.Business.Entities
{
    public class InvoiceMailingDtlsBE : BusinessEntity<PREM_ADJ>
    {
        public InvoiceMailingDtlsBE()
            : base()
        {
            base.AutoValidate = true;
        }
        
        public int PREM_ADJ_ID { get { return Entity.prem_adj_id; } set { Entity.prem_adj_id = value; } }
        public int CUSTMER_ID { get { return Entity.reg_custmr_id; } set { Entity.reg_custmr_id = value; } }
        public DateTime VALUATION_DATE { get { return Entity.valn_dt; } set { Entity.valn_dt = value; } }
        public string DRAFT_INV_TXT { get { return Entity.drft_invc_nbr_txt; } set { Entity.drft_invc_nbr_txt = value; } }
        public DateTime? DRAFT_INV_DATE { get { return Entity.drft_invc_dt; } set { Entity.drft_invc_dt = value; } }
        public DateTime? DRAFT_MAILED_UW_DT { get { return Entity.drft_mailed_undrwrt_dt; } set { Entity.drft_mailed_undrwrt_dt = value; } }
        public string FINAL_INV_TXT { get { return Entity.fnl_invc_nbr_txt == null ? "" : Entity.fnl_invc_nbr_txt; } set { Entity.fnl_invc_nbr_txt = value; } }
        public DateTime? FINAL_INV_DT { get { return Entity.fnl_invc_dt; } set { Entity.fnl_invc_dt = value; } }
        public DateTime? FINAL_MAILED_UW_DT { get { return Entity.fnl_mailed_undrwrt_dt; } set { Entity.fnl_mailed_undrwrt_dt = value; } }
        public DateTime? INV_DUE_DT { get { return Entity.invc_due_dt; } set { Entity.invc_due_dt = value; } }
        public DateTime? FINAL_EMAIL_DT { get { return Entity.fnl_mailed_brkr_dt; } set { Entity.fnl_mailed_brkr_dt = value; } }
        public int? UPDATEUSERID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UPDATEDATE { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CREATEUSERID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CREATEDATE { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public bool? Historical { get { return Entity.historical_adj_ind; } set { Entity.historical_adj_ind = value; } }
        public bool? UW_NOT_REQ { get { return Entity.undrwrt_not_reqr_ind; } set { Entity.undrwrt_not_reqr_ind = value; } }
        public string BROKER { get; set; }
        public string INVC_AMT { get; set; }
        public string BUOFC { get; set; }
        public string CUSTMRNM { get; set; }
        public string CMMNT_TXT { get; set; }
        public string UW_RESP { get; set; }
        public DateTime? UW_RESP_DT { get; set; }
        public int? ADJ_STS_TYP_ID { get; set; }
        public int ADJ_STS_ID { get; set; }
        public int CALC_ID { get; set; }
        public int UW_REV_ID { get; set; }
        public string DRFT_INTRNL_PDF_ZDW_KEY_TXT { get; set; }
        public string DRFT_EXTRNL_PDF_ZDW_KEY_TXT { get; set; }
        public string DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT { get; set; }
        public Boolean? HistoricalInd { get { return Entity.historical_adj_ind; } set { Entity.historical_adj_ind = value; } }
    }
}
