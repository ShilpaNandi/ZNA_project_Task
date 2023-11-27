using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;

using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
   public  class CustmrDocIssuesDA : DataAccessor<CUSTMR_DOC_ISSUS,CustomerDocumentIssuesBE,AISDatabaseLINQDataContext>
        
    {
       public CustmrDocIssuesDA()
       { 
       
       }

       public IList<CustomerDocumentIssuesBE> getTrackingissues(int Custmrdocid)
       {


           IList<CustomerDocumentIssuesBE> lstDocuments = new List<CustomerDocumentIssuesBE>();
           IQueryable<CustomerDocumentIssuesBE> Query = (from cissues in Context.CUSTMR_DOC_ISSUS 
                                                         where cissues.custmr_doc_id==Custmrdocid 
                                                         select new CustomerDocumentIssuesBE 
                                                         {
                                                          tracking_issue_id=cissues.traking_issu_id
                                                          
                                                         });
          
          
            
                return Query.ToList();

        
       
       }

       public bool deleteCustomerdocIssues(int custmrID)
       {
           bool returnvalue;
           if (custmrID > 0)
           {
               var pols = from pol in this.Context.CUSTMR_DOC_ISSUS
                          where pol.custmr_doc_id == custmrID 
                          select pol;

               this.Context.CUSTMR_DOC_ISSUS.DeleteAllOnSubmit(pols);

               try
               {
                   this.Context.SubmitChanges();
               }
               catch
               {
               }
               returnvalue = true;
           }
           else
           {
               returnvalue = false;
           }

           return returnvalue;
       }
    }
}
