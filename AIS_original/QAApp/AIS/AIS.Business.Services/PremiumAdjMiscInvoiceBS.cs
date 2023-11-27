using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.ExceptionHandling;

namespace ZurichNA.AIS.Business.Logic
{
    /// <summary>
    /// This Class is used to interact with Prem Adjustment Misc Invoice Table
    /// </summary>
    public class PremiumAdjMiscInvoiceBS : BusinessServicesBase<PremiumAdjMiscInvoiceBE, PremiumAdjMiscInvoiceDA>
    {
        /// <summary>
        /// Method to get the Misc Invoice Records
        /// </summary>
        /// <param name="intaccountID"></param>
        /// <param name="intpremperdID"></param>
        /// <param name="intpremadjID"></param>
        /// <returns>IList<PremiumAdjMiscInvoiceBE></returns>
        public IList<PremiumAdjMiscInvoiceBE> GetMiscInvoicelist(int intaccountID, int intpremperdID, int intpremadjID)
        {

            IList<PremiumAdjMiscInvoiceBE> preadjMiscInvBE;

            try
            {
            preadjMiscInvBE = new PremiumAdjMiscInvoiceDA().GetMiscInvoicelist(intaccountID, intpremperdID, intpremadjID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (preadjMiscInvBE);

        }
        /// <summary>
        /// To Get the lookup Id
        /// </summary>
        /// <returns>IList<LookupBE></returns>
        public IList<LookupBE> getPremAdjMiscInvoiceTypeLookUp()
        {
            BLAccess BLAcc = new BLAccess();
            IList<LookupBE> LookupBEs = new List<LookupBE>();

            LookupBEs = BLAcc.GetLookUpActiveData("MISCELLANEOUS INVOICING");

            IEnumerable<LookupBE> query = (from kj in LookupBEs
                                           select new LookupBE() { LookUpID = kj.LookUpID, LookUpName = kj.LookUpName });

            return query.ToList();
        }
        /// <summary>
        /// Function to Add or Update Record in to the PremiumAdjMiscInvoice Object
        /// </summary>
        /// <param name="PreAdjMiscInvoiceBE"></param>
        /// <returns>bool True/False(saved or not)</returns>
        public bool Update(PremiumAdjMiscInvoiceBE PreAdjMiscInvoiceBE)
        {
            bool succeed = false;
            try
            {
            if (PreAdjMiscInvoiceBE.PREM_ADJ_MISC_INVC_ID > 0) //On Update
            {
                succeed = this.Save(PreAdjMiscInvoiceBE);
            }
            else //On Insert
            {
                succeed = DA.Add(PreAdjMiscInvoiceBE);
            }
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                return succeed;
            }
            return succeed;

        }
        /// <summary>
        /// Function to fetch the Record from Misc Invoice based on PremAdjMiscInvoiceID
        /// </summary>
        /// <param name="intPremAdjMiscInvoiceID"></param>
        /// <returns></returns>
        public PremiumAdjMiscInvoiceBE getMiscInvoiceRow(int intPremAdjMiscInvoiceID)
        {
            PremiumAdjMiscInvoiceBE preadjMiscInvBE = new PremiumAdjMiscInvoiceBE();
            try
            {
                preadjMiscInvBE = DA.Load(intPremAdjMiscInvoiceID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (preadjMiscInvBE);
        }
    }
}
