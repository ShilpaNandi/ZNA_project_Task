using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.ExceptionHandling;

using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.AIS.Business.Logic
{
    public class PremiumAdjustmentEscrowBS : BusinessServicesBase<PremiumAdjustmentEscrowBE, PremiumAdjustmentEscrowDA>
    {
        /// <summary>
        /// Method to Retrieve the records from PREM_ADJ_PARMET_SETUP Table
        ///  Based on AccountID,PremAdjPerdID,PremAdjPgrID,PremAdjPgrSetupID
        /// </summary>
        /// <param name="intaccountID"></param>
        /// <param name="intPremAdjPerdID"></param>
        /// <param name="intPremAdjID"></param>
        /// <param name="intPremAdjPgrID"></param>
        /// <param name="intPremAdjPgrSetupID"></param>
        /// <returns>IList<PremiumAdjustmentEscrowBE></returns>
        public IList<PremiumAdjustmentEscrowBE> GetPremAdjEscrowInfo(int intaccountID, int intPremAdjPerdID, int intPremAdjID, int intPremAdjPgrID, int intPremAdjPgrSetupID)
        {

            IList<PremiumAdjustmentEscrowBE> premadjEscrowBE;
            try
            {
                premadjEscrowBE = new PremiumAdjustmentEscrowDA().GetPremAdjEscrowInfo(intaccountID,intPremAdjPerdID,intPremAdjID,intPremAdjPgrID,intPremAdjPgrSetupID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (premadjEscrowBE);

        }
        /// <summary>
        /// Method to Retrieve the Record from PREM_ADJ_PARMET_SETUP Table Based on P.key
        /// </summary>
        /// <param name="intPremAdjParmsetupID"></param>
        /// <returns>PremiumAdjustmentEscrowBE</returns>
        public PremiumAdjustmentEscrowBE getPremAdjEscrowRow(int intPremAdjParmsetupID)
        {
            PremiumAdjustmentEscrowBE premAdjEscrowRow = new PremiumAdjustmentEscrowBE();
            premAdjEscrowRow = DA.Load(intPremAdjParmsetupID);

            return premAdjEscrowRow;
        }
        /// <summary>
        /// Function to Add or Update Record in to the PREM_ADJ_PARMET_SETUP Object
        /// </summary>
        /// <param name="premadjEscrowBE"></param>
        /// <returns>bool True/False(saved or not)</returns>
        public bool Update(PremiumAdjustmentEscrowBE premadjEscrowBE)
        {
            bool succeed = false;
            try
            {
            if (premadjEscrowBE.PREM_ADJ_PARMET_SETUP_ID> 0)
            {
                succeed = this.DA.Update(premadjEscrowBE);
            }
            else
            {
                succeed = this.DA.Add(premadjEscrowBE);
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
