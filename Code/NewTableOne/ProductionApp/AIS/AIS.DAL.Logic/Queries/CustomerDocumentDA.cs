using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;

using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic.Queries
{
    public class CustomerDocumentDA : DataAccessor<CUSTMR_DOC,CustomerDocumentBE,AISDatabaseLINQDataContext>
    {

        public CustomerDocumentDA()
        { }
/// <summary>
/// This method id used to return the doucments list.
/// </summary>
/// <param name="customerid"></param>
/// <returns></returns>
        public IList<CustomerDocumentBE> getDcoumentList(int? customerid)
        {

            IList<CustomerDocumentBE> lstDocuments = new List<CustomerDocumentBE>();
            IQueryable<CustomerDocumentBE> Query = this.BuildQueryList(customerid);
            
                return Query.ToList();

        }

        public IList<CustomerDocumentBE> getNonaisDcoumentList(int? customerid)
        {

            IList<CustomerDocumentBE> lstDocuments = new List<CustomerDocumentBE>();
            IQueryable<CustomerDocumentBE> Query = this.BuildQueryListnonais(customerid);

            return Query.ToList();

        }

         public IQueryable<CustomerDocumentBE> BuildQueryList(int? AccountID)
        { 
           IQueryable<CustomerDocumentBE> result= (from cdocs in Context.CUSTMR_DOCs 
                                                  join lk in Context.LKUPs on cdocs.frm_id equals 
                                                  lk.lkup_id where cdocs.actv_ind==true
                                                   orderby cdocs.ent_dt ascending
                                                  select new CustomerDocumentBE 
                                                  {
                                                   CUSTOMER_DOCUMENT_ID=cdocs.custmr_doc_id,
                                                   RESPONSIBLE_PERS_ID=cdocs.qlty_cntrl_pers_id,
                                                   // COMMENTED out the following line due to compilation error
                                                   //TRACKING_ISSUE_ID=cdocs.traking_issu_id,
                                                   TRACKING_ISSUE_ID = 1,
                                                   FORM_ID=cdocs.frm_id,
                                                   FORM_NAME=lk.lkup_txt,
                                                   RECEVD_DATE=cdocs.recd_dt,
                                                   PROGM_EFF_DATE=cdocs.pgm_eff_dt,
                                                   PROG_EXP_DATE=cdocs.pgm_expi_dt,
                                                   ENTRY_DATE=cdocs.ent_dt,
                                                   QUALITY_CNTRL_DATE=cdocs.qlty_cntrl_dt,
                                                   VALUATION_DATE=cdocs.valn_dt,
                                                   RETRO_ADJ_AMOUNT=cdocs.md_retro_adj_amt,
                                                   TWENTY_PER_QC=cdocs.twenty_pct_qlty_cntrl_ind,
                                                   COMMENTS=cdocs.cmmnt_txt,
                                                   CUSTMR_ID=cdocs.custmr_id,
                                                   UPDATED_DATE=cdocs.updt_dt,
                                                   UPDATED_USR_ID=cdocs.updt_user_id,
                                                   CREATED_USR_ID=cdocs.crte_user_id,
                                                   CREATED_DATE=cdocs.crte_dt,
                                                   ACTV_IND=cdocs.actv_ind
                                                  });

                    if(AccountID > 0)
                        result=result.Where(cdocs => cdocs.CUSTMR_ID ==AccountID);

                    return result;              
        
        }
    
        public IQueryable<CustomerDocumentBE> BuildQueryListnonais(int? NonaisAccountID)
        { 
           IQueryable<CustomerDocumentBE> result= (from cdocs in Context.CUSTMR_DOCs 
                                                  join lk in Context.LKUPs on cdocs.frm_id equals 
                                                  lk.lkup_id where cdocs.actv_ind==true
                                                   orderby cdocs.ent_dt ascending
                                                  select new CustomerDocumentBE 
                                                  {
                                                   CUSTOMER_DOCUMENT_ID=cdocs.custmr_doc_id,
                                                   RESPONSIBLE_PERS_ID=cdocs.qlty_cntrl_pers_id,
                                                   // COMMENTED out the following line due to compilation error
                                                   //TRACKING_ISSUE_ID=cdocs.traking_issu_id,
                                                   TRACKING_ISSUE_ID = 1,
                                                   FORM_ID=cdocs.frm_id,
                                                   FORM_NAME=lk.lkup_txt,
                                                   RECEVD_DATE=cdocs.recd_dt,
                                                   PROGM_EFF_DATE=cdocs.pgm_eff_dt,
                                                   PROG_EXP_DATE=cdocs.pgm_expi_dt,
                                                   ENTRY_DATE=cdocs.ent_dt,
                                                   QUALITY_CNTRL_DATE=cdocs.qlty_cntrl_dt,
                                                   VALUATION_DATE=cdocs.valn_dt,
                                                   RETRO_ADJ_AMOUNT=cdocs.md_retro_adj_amt,
                                                   TWENTY_PER_QC=cdocs.twenty_pct_qlty_cntrl_ind,
                                                   COMMENTS=cdocs.cmmnt_txt,
                                                   CUSTMR_ID=cdocs.custmr_id,
                                                   Non_Ais_id=cdocs.nonais_custmr_id,
                                                   UPDATED_DATE=cdocs.updt_dt,
                                                   UPDATED_USR_ID=cdocs.updt_user_id,
                                                   CREATED_USR_ID=cdocs.crte_user_id,
                                                   CREATED_DATE=cdocs.crte_dt,
                                                   ACTV_IND=cdocs.actv_ind
                                                  });

                    if(NonaisAccountID > 0)
                        result=result.Where(cdocs => cdocs.Non_Ais_id==NonaisAccountID);

                    return result;              
        
        }



       

    }
}
