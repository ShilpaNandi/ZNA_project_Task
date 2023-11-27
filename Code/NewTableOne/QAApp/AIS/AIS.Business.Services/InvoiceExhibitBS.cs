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
    public class InvoiceExhibitBS : BusinessServicesBase<InvoiceExhibitBE, InvoiceExhibitDA>
    {
        /// <summary>
        /// Retrieves Invoice Exhibit Data 
        /// </summary>
        /// <returns>List of InvoiceExhibitBE</returns>
        public IList<InvoiceExhibitBE> getInvoiceExhibitData()
        {
            InvoiceExhibitDA invcExh = new InvoiceExhibitDA();
            IList<InvoiceExhibitBE> result = invcExh.getInvoiceExhibitData();
            return result;
        }
        /// <summary>
        /// Retrieves Invoice Exhibit Data Based on Flag Value
        /// </summary>
        /// <returns>List of InvoiceExhibitBE</returns>
        ///intFlag is three types
        ///1 - Internal
        ///2 - External
        ///3 - Cesar
        public IList<InvoiceExhibitBE> getInvoiceExhibitData(int intFlag)
        {
            InvoiceExhibitDA invcExh = new InvoiceExhibitDA();
            IList<InvoiceExhibitBE> result = invcExh.getInvoiceExhibitData(intFlag);
            return result;
        }
        /// <summary>
        /// Retrieves Invoice Exhibit Data based on Exhibit ID
        /// </summary>
        /// <returns>List of InvoiceExhibitBE</returns>
        public InvoiceExhibitBE getInvoiceExhibitRow(int ExhibitID)
        {
            InvoiceExhibitBE invoiceExhibitBE = new InvoiceExhibitBE();
            invoiceExhibitBE = DA.Load(ExhibitID);
            return invoiceExhibitBE;
        }
        /// <summary>
        /// Insert or update invoiceExhibit Data
        /// </summary>
        /// <returns>Result</returns>
        public bool Update(InvoiceExhibitBE invoiceExhibitBE)
        {
            bool succeed = false;
            try
            {
                if (invoiceExhibitBE.INVC_EXHIBIT_SETUP_ID > 0) //On Update
                {
                    succeed = this.Save(invoiceExhibitBE);

                }
                else //On Insert
                {
                    invoiceExhibitBE.INVC_EXHIBIT_SETUP_ID = DA.GetNextPkID().Value;

                    succeed = DA.Add(invoiceExhibitBE);
                }
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                return succeed;
            }
            return succeed;

        }
       
    }
}
