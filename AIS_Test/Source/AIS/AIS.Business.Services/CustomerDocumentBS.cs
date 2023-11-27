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

namespace ZurichNA.AIS.Business.Logic
{
    /// <summary>
    /// This class is used to store records for Document Tracking.
    /// </summary>
    public class CustomerDocumentBS:BusinessServicesBase<CustomerDocumentBE,CustomerDocumentDA>

    {
        /// <summary>
        /// This function is used return the Documentid basing on the customerid.
        /// </summary>
        /// <param name="CustdocId"></param>
        /// <returns></returns>
        public CustomerDocumentBE getCustomerDocid(int CustdocId)
        {
            CustomerDocumentBE customerdocBE = new CustomerDocumentBE();
            customerdocBE = DA.Load(CustdocId);
            return customerdocBE;
        }

        /// <summary>
        /// This function is used to return the customer documents basing on the accountid.
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns>List</returns>
       public IList<CustomerDocumentBE>getcustomerdocuments(int? AccountId)
        {
            IList<CustomerDocumentBE> lstdocumentslist;
            try
            {


                lstdocumentslist = new CustomerDocumentDA().getDcoumentList(AccountId);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return lstdocumentslist;
            
        
        }

       public IList<CustomerDocumentBE> getNonaiscustomerdocuments(int? NonaisAccountId)
       {
           IList<CustomerDocumentBE> lstdocumentslist;
           try
           {


               lstdocumentslist = new CustomerDocumentDA().getNonaisDcoumentList(NonaisAccountId);
           }
           catch (Exception ex)
           {
               RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
               throw myException;
           }

           return lstdocumentslist;


       }

       public bool save(CustomerDocumentBE customerdocBE)
       {
           bool succeed = false;
           try
           {

               
                   if (customerdocBE.CUSTOMER_DOCUMENT_ID > 0)
                   {
                      // customerdocBE.UPDATED_DATE = DateTime.Now;
                      // succeed = this.DA.Update(customerdocBE);
                     succeed= this.Save(customerdocBE);
                       
                   }
                   else
                   {
                       customerdocBE.CREATED_DATE = DateTime.Now;
                       succeed = this.DA.Add(customerdocBE);
                   }
                  
              
               return succeed;
           }
           catch (Exception ex)
           {
               RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
               return succeed;
           }

       }
       // Not Required now
       # region ToDelete
       public bool update(CustomerDocumentBE customerdocBE, ArrayList issues)
       {
           bool succeed = false;
           try
           {

               for (int count = 0; count <= issues.Count; count++)
               {
                   customerdocBE.TRACKING_ISSUE_ID = Convert.ToInt32(issues[count]);
                   if (customerdocBE.CUSTOMER_DOCUMENT_ID > 0)
                   {
                       customerdocBE.UPDATED_DATE = DateTime.Now;
                       succeed = this.DA.Update(customerdocBE);
                   }
                   else
                   {
                       customerdocBE.CREATED_DATE = DateTime.Now;
                       succeed = this.DA.Add(customerdocBE);
                   }
                   // 
               }
               return succeed;
           }
           catch (Exception ex)
           {
               RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
               throw myException;
           }

       }
        /// <summary>
        /// This function is used to Add or Save the documents.
        /// </summary>
        /// <param name="customerdocBE"></param>
        /// <returns>Success or Failure.</returns>
        public bool save(CustomerDocumentBE customerdocBE,ArrayList issues)
        {
            bool succeed = false;
            try
            {
               
                for (int count = 0; count <= issues.Count; count++)
                {
                    customerdocBE.TRACKING_ISSUE_ID = Convert.ToInt32(issues[count]);
                    if (customerdocBE.CUSTOMER_DOCUMENT_ID > 0)
                    {
                        customerdocBE.UPDATED_DATE = DateTime.Now;
                        succeed = this.DA.Update(customerdocBE);
                    }
                    else
                    {
                        customerdocBE.CREATED_DATE = DateTime.Now;
                        succeed = this.DA.Add(customerdocBE);
                    }
                   // 
                }
                return succeed;
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

        }

       #endregion
    }
}
