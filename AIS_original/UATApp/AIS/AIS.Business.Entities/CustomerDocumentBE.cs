using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.AIS.Business.Entities
{
   public  class CustomerDocumentBE : BusinessEntity<CUSTMR_DOC>
    {
        public int CUSTOMER_DOCUMENT_ID
        {
            get { return Entity.custmr_doc_id; }
            set { Entity.custmr_doc_id = value; }
        
        }
        public int RESPONSIBLE_PERS_ID
        {
            get { return Entity.qlty_cntrl_pers_id; }
            set { Entity.qlty_cntrl_pers_id = value; }
        }
       // this causes a compilation error- needed to comment out the code
        public int TRACKING_ISSUE_ID;
        //{
        //    get { return Entity.traking_issu_id }
        //    set { Entity.traking_issu_id = value; }
        
        //}
        public int FORM_ID
        { get { return Entity.frm_id; } set { Entity.frm_id = value; } }
       
       public DateTime? RECEVD_DATE
       {
           get { return Entity.recd_dt; }
           set { Entity.recd_dt = value; }

        }
       public DateTime? PROGM_EFF_DATE
       {
           get { return Entity.pgm_eff_dt; }
           set { Entity.pgm_eff_dt = value; }
       
       }
       public DateTime? PROG_EXP_DATE
       {
           get { return Entity.pgm_expi_dt; }
           set { Entity.pgm_expi_dt = value; }
       }
       public DateTime? ENTRY_DATE
       { get { return Entity.ent_dt; }
           set { Entity.ent_dt = value; }
       }

       public DateTime? QUALITY_CNTRL_DATE
       { get { return Entity.qlty_cntrl_dt; }
           set { Entity.qlty_cntrl_dt = value; }
       }

       public DateTime? VALUATION_DATE
       { get { return Entity.valn_dt; }
           set { Entity.valn_dt = value; }
       }
       public decimal? RETRO_ADJ_AMOUNT
       { get { return Entity.md_retro_adj_amt; }
           set { Entity.md_retro_adj_amt = value; }
       }

       public bool? TWENTY_PER_QC
       { get { return Entity.twenty_pct_qlty_cntrl_ind; }
           set { Entity.twenty_pct_qlty_cntrl_ind = value; }
       }
       public string COMMENTS
       {
           get { return Entity.cmmnt_txt; }
           set { Entity.cmmnt_txt = value; }
       
       }
       public int cash_flw_spl_id
       {
           get { return Entity.cash_flw_splist_pers_id; }
           set { Entity.cash_flw_splist_pers_id = value; }
       
       }
       public int? bu_off_id
       {
           get { return Entity.bu_office_id; }
           set { Entity.bu_office_id = value; }
       
       }
       public int? Non_Ais_id
       {
           get { return Entity.nonais_custmr_id; }
           set { Entity.nonais_custmr_id = value; }
       
       }
       public int? CUSTMR_ID
       {
           get { return Entity.custmr_id; }
           set { Entity.custmr_id = value; }
       }
       public DateTime? UPDATED_DATE
       {
           get { return Entity.updt_dt; }
           set { Entity.updt_dt = value; }
       }

       public int? UPDATED_USR_ID
       { get { return Entity.updt_user_id; }
           set { Entity.updt_user_id = value; }
       }

       public int CREATED_USR_ID
       { get { return Entity.crte_user_id; }
           set { Entity.crte_user_id = value; }
       }

       public DateTime CREATED_DATE
       {
           get { return Entity.crte_dt; }
           set { Entity.crte_dt = value; }
       }
       public bool? ACTV_IND
       {
           get { return Entity.actv_ind; }
           set { Entity.actv_ind = value; }
       
       }

       public string FORM_NAME
       {
           get;
           set;

       }
    }
}
