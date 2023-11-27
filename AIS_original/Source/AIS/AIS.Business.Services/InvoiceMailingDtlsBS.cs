using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.Business.Logic
{
    public class InvoiceMailingDtlsBS : BusinessServicesBase<InvoiceMailingDtlsBE, InvoiceMailingDtlsDA>
    {

        /// <summary>
        /// Retrieves all Invoice Mailing Dtls  based on final Inv No.
        /// </summary>
        /// <returns></returns>
        public IList<InvoiceMailingDtlsBE> getInvMailingFinalData(int custmrID, string fnlinvnbr, string valndtfrm, string valndtto, string invduedtfrm, string invduedtto)
        {
            IList<InvoiceMailingDtlsBE> list = new List<InvoiceMailingDtlsBE>();
            InvoiceMailingDtlsDA inv = new InvoiceMailingDtlsDA();

            try
            {
                list = inv.getInvMailingFinalData(custmrID, fnlinvnbr);
                if (valndtfrm.ToString() != "" && valndtto.ToString() != "")
                {
                    list = list.Where(prmadj => prmadj.VALUATION_DATE >= DateTime.Parse(valndtfrm) && prmadj.VALUATION_DATE <= DateTime.Parse(valndtto)).ToList();
                }
                if (invduedtfrm.ToString() != "" && invduedtto.ToString() != "")
                {
                    list = list.Where(prmadj => prmadj.INV_DUE_DT >= DateTime.Parse(invduedtfrm) && prmadj.INV_DUE_DT <= DateTime.Parse(invduedtto)).ToList();
                }
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
           return list;
        }
        /// <summary>
        /// Retrieves all Invoice Mailing Dtls based on Draft Inv No.
        /// </summary>
        /// <returns></returns>
        public IList<InvoiceMailingDtlsBE> getInvMailingDraftData(int custmrID, string dftinvnbr, string valndtfrm, string valndtto, string invduedtfrm, string invduedtto)
        {
            IList<InvoiceMailingDtlsBE> list = new List<InvoiceMailingDtlsBE>();
            InvoiceMailingDtlsDA inv = new InvoiceMailingDtlsDA();

            try
            {
                list = inv.getInvMailingDraftData(custmrID, dftinvnbr);
                if (valndtfrm.ToString() != "" && valndtto.ToString() != "")
                {
                    list = list.Where(prmadj => prmadj.VALUATION_DATE >= DateTime.Parse(valndtfrm) && prmadj.VALUATION_DATE <= DateTime.Parse(valndtto)).ToList();
                }
                if (invduedtfrm.ToString() != "" && invduedtto.ToString() != "")
                {
                    list = list.Where(prmadj => prmadj.INV_DUE_DT >= DateTime.Parse(invduedtfrm) && prmadj.INV_DUE_DT <= DateTime.Parse(invduedtto)).ToList();
                }
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return list;
        }
        /// <summary>
        /// Retrieves Invoice Mailing Dtls based on  ID
        /// </summary>
        /// <returns>List of InvoiceExhibitBE</returns>
        public InvoiceMailingDtlsBE getInvoiceMailingDtlsRow(int ID)
        {
            InvoiceMailingDtlsBE invoiceMailingBE = new InvoiceMailingDtlsBE();
            invoiceMailingBE = DA.Load(ID);
            return invoiceMailingBE;
        }
        /// <summary>
        /// Retrieves all Invoice Mailing Dtls  
        /// </summary>
        /// <returns></returns>
        public InvoiceMailingDtlsBE getInvMailingDataRow(int ID)
        {
           InvoiceMailingDtlsBE list = new InvoiceMailingDtlsBE();
            InvoiceMailingDtlsDA inv = new InvoiceMailingDtlsDA();

            try
            {
                list = inv.getInvMailingDataRow(ID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return list;
        }
        /// <summary>
        /// Retrieves all Invoice Mailing Dtls -Written By Venkat to Fix Bug:7804
        /// </summary>
        /// <returns></returns>
        public IList<InvoiceMailingDtlsBE> getInvMailingData(int custmrID, string invnbr, string valndtfrm, string valndtto, string invduedtfrm, string invduedtto)
        {
            IList<InvoiceMailingDtlsBE> list = new List<InvoiceMailingDtlsBE>();
            InvoiceMailingDtlsDA inv = new InvoiceMailingDtlsDA();

            try
            {
                list = inv.getInvMailingData(custmrID, invnbr, valndtfrm, valndtto, invduedtfrm, invduedtto);

            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return list;
        }
        /// <summary>
        /// Update Invoice Mailing data 
        /// </summary>
        /// <returns>Result</returns>
        public bool Update(InvoiceMailingDtlsBE invBE)
        {
            bool succeed = false;
            try
            {
                if (invBE.PREM_ADJ_ID > 0) //On Update
                {
                    succeed = this.Save(invBE);

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
