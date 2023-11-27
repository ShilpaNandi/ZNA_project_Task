using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.DAL.Logic.Queries;
using System.Collections;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.Business.Logic
{
    public class CustomerDocumentIssuesBS : BusinessServicesBase<CustomerDocumentIssuesBE,CustmrDocIssuesDA>
    {

   

     public bool save(CustomerDocumentIssuesBE  custmrdocissuesBE)
       {
           bool succeed = false;
           try
           {

  // custmrdocissuesBE.CREATED_DATE = DateTime.Now;
               succeed = this.DA.Add(custmrdocissuesBE);
               return succeed;
           }
           catch (Exception ex)
           {
               RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
               throw myException;
           }

       }

     public IList<CustomerDocumentIssuesBE> getIssuedetails(int customerid)
     {
         try
         {
             IList<CustomerDocumentIssuesBE> lstissues = new CustmrDocIssuesDA().getTrackingissues(customerid);
             return lstissues;
         }
         catch (Exception ex)
         {
             RetroBaseException myException = new RetroBaseException(ex.Message, ex);
             throw myException;
         
         }
     
     
     }

     public bool deleteCustmrissues(int custmrdocID)
     {
         bool issuedelete;
         try
         {
             issuedelete = new CustmrDocIssuesDA().deleteCustomerdocIssues(custmrdocID);
         }
         catch (System.Data.SqlClient.SqlException ex)
         {
             RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
             throw myException;
         }


         return (issuedelete);

     }

    }
}
