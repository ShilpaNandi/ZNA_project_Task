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
    /// <summary>
    /// This clas is used to interact with Third Party Administrator Manual Invoice Table
    /// </summary>
    public class TPAManualPostingsBS : BusinessServicesBase<TPAManualPostingsBE, TPAManualPostingsDA>
    {
        /// <summary>
        /// Function to get the Single row of Third Party Administrator Manual Invoice Table
        /// </summary>
        /// <param name="TpaPostID"></param>
        /// <returns>TPAManualPostingsBE</returns>
        public TPAManualPostingsBE getTPAPostRow(int TpaPostID)
        {
            TPAManualPostingsBE TPS = new TPAManualPostingsBE();
            TPS = DA.Load(TpaPostID);
            return TPS;
        }
        /// <summary>
        /// Function to get the List of Third Party Administrator Manual Invoice Table
        /// </summary>
        /// <returns>TPAManualPostingsBE List</returns>
        public TPAManualPostingsBE getTPAPostList(int CustomerID)
        {
            IList<TPAManualPostingsBE> list = new List<TPAManualPostingsBE>();
            TPAManualPostingsDA TPA = new TPAManualPostingsDA();
            TPAManualPostingsBE TPABE = new TPAManualPostingsBE();
            try
            {
                list = TPA.getTPAPostList(CustomerID);
                if (list.Count() > 0)
                {
                    //list = list.OrderByDescending(TPAID => TPAID.ThirdPartyAdminManualInvoiceID).ToList();
                    TPABE = list[0];
                }
            }

            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return TPABE;
        }
        /// <summary>
        /// Function to get the subsequent Row of Third Party Administrator Manual Invoice Table
        /// </summary>
        /// <returns>TPAManualPostingsBE</returns>
        public TPAManualPostingsBE getTPASubsequent(int CustomerID)
        {
            IList<TPAManualPostingsBE> list = new List<TPAManualPostingsBE>();
            TPAManualPostingsDA TPA = new TPAManualPostingsDA();
            TPAManualPostingsBE TPABE = new TPAManualPostingsBE();
            try
            {
                list = TPA.getTPAPostList(CustomerID);
                if (list.Count() > 0)
                {
                    list = list.Where(TPAID => TPAID.FinalizedIndicator != true && TPAID.CancelIndicator != true).ToList();
                    if (list.Count() > 0)
                        TPABE = list[0];

                }
            }

            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return TPABE;
        }
        /// <summary>
        /// Function to get the List of Third Party Administrator Manual Invoice Table
        /// </summary>
        /// <returns>TPAManualPostingsBE List</returns>
        public IList<TPAManualPostingsBE> getTPAPostBEList(int CustomerID)
        {
            IList<TPAManualPostingsBE> list = new List<TPAManualPostingsBE>();
            TPAManualPostingsDA TPA = new TPAManualPostingsDA();
            try
            {
                list = TPA.getTPAPostList(CustomerID);

            }

            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return list;
        }
        /// <summary>
        /// Function to get the Search results from List of Third Party Administrator Manual Invoice
        /// </summary>
        /// <returns>TPAManualPostingsBE</returns>
        public IList<TPAManualPostingsBE> getTPAPostSearchResultList(int customerID, int? tpaId, string invoiceNumber, int buOfficeId, int invoiceType, string valnDt, string fromDate, string toDate)
        {
            IList<TPAManualPostingsBE> tpaList = new List<TPAManualPostingsBE>();

            try
            {
                tpaList = new TPAManualPostingsDA().getTPAPostSearchResultList(customerID, tpaId, invoiceNumber, buOfficeId, invoiceType, valnDt, fromDate, toDate);

            }

            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return tpaList;
        }

        /// <summary>
        /// update the information in Third Party Administrator Manual Invoice  Table
        /// </summary>
        /// <param name="TPAManualPostingsBE">TPABE</param>
        /// <returns>Bool (True/false) i.e., updated or not </returns>
        public bool Update(TPAManualPostingsBE TPABE)
        {
            bool succeed = false;
            if (TPABE.ThirdPartyAdminManualInvoiceID > 0) //On Update
            {
                succeed = this.Save(TPABE);
            }
            else //On Insert
            {
                succeed = DA.Add(TPABE);
            }
            return succeed;
        }
        /// <summary>
        /// Delete the information in Third Party Administrator Manual Invoice  Table
        /// </summary>
        /// <param name="TPAManualPostingsBE">TPABE</param>
        /// <returns>Bool (True/false) i.e., Deleted or not </returns>
        public bool DeleteTPA(TPAManualPostingsBE TPABE)
        {
            bool succeed = false;
            return succeed = this.Delete(TPABE);
        }
        public void TPATransmittalToARiES(int TPAID, int? Custmr_id, int? IND)
        {
            try
            {
                (new TPAManualPostingsDA()).TPATransmittalToARiES(TPAID, Custmr_id, IND);

            }

            catch (Exception ex)
            {
                //RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                //throw myException;
            }
        }
    }
}
