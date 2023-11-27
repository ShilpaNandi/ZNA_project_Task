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
    public class PremiumAdjCombElementsBS : BusinessServicesBase<PremiumAdjCombElementsBE, PremiumAdjCombElementsDA>
    {
        /// <summary>
        /// Function to Add or Update Record in to the PremiumAdjCombElemnts Object
        /// </summary>
        /// <param name="PreAdjCombElementsBE"></param>
        /// <returns>bool True/False(saved or not)</returns>
        public bool Update(PremiumAdjCombElementsBE PreAdjCombElementsBE)
        {
            bool succeed = false;
            try
            {
            if (PreAdjCombElementsBE.PREM_ADJ_COMB_ELEMNTS_ID > 0) //On Update
            {
                succeed = this.Save(PreAdjCombElementsBE);
            }
            else //On Insert
            {
                succeed = DA.Add(PreAdjCombElementsBE);
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
        /// Method to get a Record from PremAdjCombElemnts Table Based On P.key
        /// </summary>
        /// <param name="intPremAdjCombElementsID"></param>
        /// <returns>PremiumAdjCombElementsBE</returns>
        public PremiumAdjCombElementsBE getPremAdjCombElementsRow(int intPremAdjCombElementsID)
        {
            PremiumAdjCombElementsBE PreAdjCombElementsBE = new PremiumAdjCombElementsBE();
            try
            {
            PreAdjCombElementsBE = DA.Load(intPremAdjCombElementsID);
             }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (PreAdjCombElementsBE);
        }
        /// <summary>
        /// Method to get Records from PremAdjCombElemnst Table 
        /// Based on AccountId,PremAdjId,PremAdjPerdID
        /// </summary>
        /// <param name="intAccountID"></param>
        /// <param name="intPremAdjID"></param>
        /// <param name="intPremAdjPerdID"></param>
        /// <returns>IList<PremiumAdjCombElementsBE></returns>
        public IList<PremiumAdjCombElementsBE> getPremAdjCombElements(int intAccountID, int intPremAdjID, int intPremAdjPerdID)
        {
            IList<PremiumAdjCombElementsBE> PreAdjCombElementsBE = new List<PremiumAdjCombElementsBE>();
            try
            {
            PreAdjCombElementsBE = new PremiumAdjCombElementsDA().GetPremAdjCombElements(intAccountID, intPremAdjID, intPremAdjPerdID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (PreAdjCombElementsBE);
        }
    }
}
