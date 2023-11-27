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
    /// This Class is used to interact with PremAdjNYScndInjuryFund Table
    /// </summary>
    public class PremiumAdjNYScndInjuryFundBS : BusinessServicesBase<PremiumAdjNYScndInjuryFundBE, PremiumAdjNYScndInjuryFundDA>
    {
        /// <summary>
        ///  Function to Add or Update Record in to the PremAdjNYScndInjuryFund Object
        /// </summary>
        /// <param name="PreAdjNYSIFBE"></param>
        /// <returns>bool True/False(saved or not)</returns>
        public bool Update(PremiumAdjNYScndInjuryFundBE PreAdjNYSIFBE)
        {
            bool succeed = false;
            try
            {
            if (PreAdjNYSIFBE.PREM_ADJ_NY_SCND_INJR_FUND_ID > 0) //On Update
            {
                succeed = this.Save(PreAdjNYSIFBE);
            }
            else //On Insert
            {
                succeed = DA.Add(PreAdjNYSIFBE);//On Insert
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
        /// Retrieves PremAdjNYSIF Data based on AccountID,PremAdjID,PremAdjPerdID
        /// </summary>
        /// <param name="intAccountID"></param>
        /// <param name="intPremAdjID"></param>
        /// <param name="intPremAdjPerdID"></param>
        /// <returns>IList<PremiumAdjNYScndInjuryFundBE></returns>
        public IList<PremiumAdjNYScndInjuryFundBE> getPremAdjNYSIF(int intAccountID, int intPremAdjID, int intPremAdjPerdID,int intPremAdjPgmID)
        {
            IList<PremiumAdjNYScndInjuryFundBE> PreAdjNYSIFBE = new List<PremiumAdjNYScndInjuryFundBE>();
            PreAdjNYSIFBE = new PremiumAdjNYScndInjuryFundDA().GetPremAdjNYSIF(intAccountID,intPremAdjID,intPremAdjPerdID,intPremAdjPgmID);
            return (PreAdjNYSIFBE);
        }
        /// <summary>
        /// Retrieves PremAdjNYSIFRow based on P.key
        /// </summary>
        /// <param name="intPremAdjNYSIFID"></param>
        /// <returns>PremiumAdjNYScndInjuryFundBE</returns>
        public PremiumAdjNYScndInjuryFundBE getPremAdjNYSIFRow(int intPremAdjNYSIFID)
        {
            PremiumAdjNYScndInjuryFundBE PreAdjNYSIFBE = new PremiumAdjNYScndInjuryFundBE();
            PreAdjNYSIFBE = DA.Load(intPremAdjNYSIFID);
            return (PreAdjNYSIFBE);
        }
        
        
        /// <summary>
        /// Retrieves PremiumAdjustment -AK-For PremiumAdjNYScndInjuryFund Screen
        /// </summary>
        /// <returns>List of PremiumAdjNYScndInjuryFundBE</returns>
        public PremiumAdjNYScndInjuryFundBE getPremiumAdjustmentRow(int ID)
        {
            PremiumAdjNYScndInjuryFundBE PremiumAdjustmentBE = new PremiumAdjNYScndInjuryFundBE();
            PremiumAdjustmentBE = DA.Load(ID);
            return PremiumAdjustmentBE;
        }

    }
}
