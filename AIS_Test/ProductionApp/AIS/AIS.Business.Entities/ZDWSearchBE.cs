using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.AIS.Business.Entities
{
    public class ZDWSearchBE : BusinessEntity<PREM_ADJ>
    {
        public ZDWSearchBE()
            : base()
        {
            
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
    }
}
