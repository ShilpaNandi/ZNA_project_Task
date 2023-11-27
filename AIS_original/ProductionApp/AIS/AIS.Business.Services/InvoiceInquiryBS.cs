using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.Business.Logic
{
    public class InvoiceInquiryBS : BusinessServicesBase<InvoiceInquiryBE, InvoiceInquiryDA>
    {
        private InvoiceInquiryDA objInvoiceInquiryDA;

        public InvoiceInquiryBS()
        {
            objInvoiceInquiryDA = new InvoiceInquiryDA(); // Instantiate DAL object.
        }

        /// <summary>
        ///  Retrieves all Invoice Inquiry data depends upon the where condition
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable getInvoiceInquiryData(int intAccountID, int intProgramTypID, string strValDate, string strInvoiceNumber, string strInvoiceDate, int intPersonID, int intExtOrgID, int intInternalOrgID, int intAccountNumber)
        {
            return objInvoiceInquiryDA.getInvoiceInquiryData(intAccountID, intProgramTypID, strValDate, strInvoiceNumber, strInvoiceDate, intPersonID, intExtOrgID, intInternalOrgID, intAccountNumber);
        }
       
        
        /// <summary>
        ///  Retrieves all Invoice Inquiry data depends upon the where condition
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public IList<InvoiceInquiryBE> getInvoiceData(int intCustmerID)
        {
            return objInvoiceInquiryDA.getInvoiceData(intCustmerID);
        }
        /// <summary>
        ///  Retrieves all Invoice Inquiry data depends upon the where condition
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public IList<InvoiceInquiryBE> getInvoiceDates(int intCustmerID)
        {
            return objInvoiceInquiryDA.getInvoiceDates(intCustmerID);
        }

        public InvoiceInquiryBE getInvoiceInfo(int iAdjNo)
        {
            InvoiceInquiryBE ObjInvoiceBE = new InvoiceInquiryBE();
            ObjInvoiceBE = DA.Load(iAdjNo);
            return ObjInvoiceBE;
        }

        /// <summary>
        /// Saves or Edits to database using FrameWork
        /// </summary>
        /// <param name="prgmperdBE">Adjustment ID</param>
        /// <returns>true if save, False if failed to save</returns>
        public bool Update(InvoiceInquiryBE InvInqBE)
        {
            bool succeed = false;
            if (InvInqBE.PREMADJID > 0)
            {
                succeed = this.Save(InvInqBE);
            }            
            return succeed;
        }
    }
}
