using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.AIS.Business.Logic
{
    public class PremiumAdjustmentPeriodBS : BusinessServicesBase<PremiumAdjustmentPeriodBE, PremiumAdjustmentPeriodDA>
    {
        /// <summary>
        /// Function to Add or Update Record in to the PremiumAdjCombElemnts Object
        /// </summary>
        /// <param name="PreAdjCombElementsBE"></param>
        /// <returns>bool True/False(saved or not)</returns>
        public bool Update(PremiumAdjustmentPeriodBE PreAdjPeriodBE)
        {
            bool succeed = false;
            try
            {
                if (PreAdjPeriodBE.PREM_ADJ_PERD_ID > 0) //On Update
                {
                    succeed = this.Save(PreAdjPeriodBE);
                }
                else //On Insert
                {
                    succeed = DA.Add(PreAdjPeriodBE);
                
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
        /// Function to fetch the Record from Misc Invoice based on PremAdjPerdID
        /// </summary>
        /// <param name="intPremAdjMiscInvoiceID"></param>
        /// <returns></returns>
        public PremiumAdjustmentPeriodBE getPremAdjPerdRow(int intPremAdjPerdID)
        {
            PremiumAdjustmentPeriodBE preadjPerdBE = new PremiumAdjustmentPeriodBE();
            try
            {
                preadjPerdBE = DA.Load(intPremAdjPerdID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (preadjPerdBE);
        }
        public IList<PremiumAdjustmentPeriodBE> getPremAdjPerdID(int intCustmerID, int intPremAdjID, int intPremAdjPgrmID)
        {
            IList<PremiumAdjustmentPeriodBE> PremAdjPred;
            PremAdjPred = new PremiumAdjustmentPeriodDA().getPremAdjPerdID(intCustmerID, intPremAdjID, intPremAdjPgrmID);
            return PremAdjPred;
        }

        //Used in Invoice Driver
        public IList<PremiumAdjustmentPeriodBE> GetProgramPeriods(int intPremAdjID)
        {

            IList<PremiumAdjustmentPeriodBE> premadjperdBE;
            try
            {
                premadjperdBE = new PremiumAdjustmentPeriodDA().GetProgramPeriods(intPremAdjID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (premadjperdBE);

        }
        /// <summary>
        /// Retrives all records from PREM_ADJ_PERD table filtered with PREM_ADJ_PGM_ID
        /// </summary>
        /// <param name="intProgPeriodID"></param>
        /// <returns></returns>
        public IList<PremiumAdjustmentPeriodBE> GetPREMADJPeriods(int intProgPeriodID)
        {

            IList<PremiumAdjustmentPeriodBE> premadjperdBE;
            try
            {
                premadjperdBE = new PremiumAdjustmentPeriodDA().GetPREMADJPeriods(intProgPeriodID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (premadjperdBE);

        }
        /// <summary>
        /// This method returns the listview details in Adjustment Number Webpage
        /// </summary>
        /// <returns></returns>
        public IList<PremiumAdjustmentPeriodBE> GetAdjNumberList(int intPremAdjID,int intPremAdjPgmID)
        {

            IList<PremiumAdjustmentPeriodBE> premadjperdBE;
            try
            {
                premadjperdBE = new PremiumAdjustmentPeriodDA().GetAdjNumberList(intPremAdjID, intPremAdjPgmID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (premadjperdBE);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="masterAccount"></param>
        /// <returns></returns>
        public IList<PremiumAdjustmentPeriodBE> GetPeriodList(int masterAccount)
        {

            IList<PremiumAdjustmentPeriodBE> premadjperdBE;
            try
            {
                premadjperdBE = new PremiumAdjustmentPeriodDA().getPeriodList(masterAccount);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (premadjperdBE);

        }

        public int AddPremAdjPerdTotal(int? PremAdjPerdID, int? PremAdjID, int? Custmr_id, int? PremAdjPgmID, int? CreateUserID)
        {
            PremiumAdjustmentPeriodDA preAdjPRD = new PremiumAdjustmentPeriodDA();
            return preAdjPRD.AddPremAdjPerdTotal(PremAdjPerdID, PremAdjID, Custmr_id, PremAdjPgmID, CreateUserID);
        }
        public int deletePremAdjPerdTotal(int? PremAdjPerdID, int? PremAdjID, int? Custmr_id)
        {
            PremiumAdjustmentPeriodDA preAdjPRD = new PremiumAdjustmentPeriodDA();
            return preAdjPRD.deletePremAdjPerdTotal(PremAdjPerdID, PremAdjID, Custmr_id);
        }

    }
}
