/*
             File Name          : SurchargeDetailsBS.cs
 *           Description        : code having ussisness logic for surcharge Assesment
 *           Author             : Phani Neralla
 *           Team Name          : FinSol/AIS
 *           Creation Date      : 18-Jun-2010
 *           Last Modified By   : 
 *           Last Modified Date :
*/

#region SurchargeDetails BS
#region namespaces
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
//AIS custom namespaces
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
#endregion
#region code
namespace ZurichNA.AIS.Business.Logic
{
    /// <summary>
    /// the bussiness logic is present in the class to manipulate the PREM_ADJ_SURCHRG_DTL table in DB
    /// </summary>
    public class SurchargeDetailsBS : BusinessServicesBase<SurchargeDetailAmountBE, SurchargeDetailsDA>
    {
        /// <summary>
        /// This method returns the surcharge level listview details in surcharge assesment Webpage
        /// </summary>
        /// <returns></returns>
        public IList<SurchargeDetailAmountBE> GetSurchargeDetailsList(int intPremAdjID, int intPremAdjPgmID)
        {

            IList<SurchargeDetailAmountBE> surDtlBE;
            try
            {
                surDtlBE = new SurchargeDetailsDA().GetSurchargeDetailsList(intPremAdjID, intPremAdjPgmID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (surDtlBE);

        }

        /// <summary>
        /// This method returns the surcharge level listview details in surcharge assesment Webpage
        /// </summary>
        /// <returns></returns>
        public IList<SurchargeDetailAmountBE> GetSurchargePolLvlDetailsList(int intPremAdjID, int intPremAdjPgmID, 
                                                                            int surCode, int stateID)
        {

            IList<SurchargeDetailAmountBE> surDtlBE;
            try
            {
                surDtlBE = new SurchargeDetailsDA().GetSurchargePolLvlDetailsList
                                                        (intPremAdjID, intPremAdjPgmID,surCode,stateID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (surDtlBE);

        }

        /// <summary>
        /// Function to fetch the Record from Misc Invoice based on PremAdjPerdID
        /// </summary>
        /// <param name="intPremAdjMiscInvoiceID"></param>
        /// <returns></returns>
        public SurchargeDetailAmountBE GetPremAdjPerdRow(int intPremAdjPerdID)
        {
            SurchargeDetailAmountBE preadjPerdBE = new SurchargeDetailAmountBE();
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

        /// <summary>
        /// update the table or insert a new record
        /// </summary>
        /// <param name="surchargeDetailAmountBE"></param>
        /// <returns></returns>
        public bool UpdateOtherAmounts(SurchargeDetailAmountBE surchargeDetailAmountBE)
        {
            bool succeed = false;
            try
            {
                if (surchargeDetailAmountBE.prem_adj_surchrg_dtl_id > 0) //On Update
                {
                    succeed = this.Save(surchargeDetailAmountBE);
                }
                else //On Insert
                {
                    succeed = DA.Add(surchargeDetailAmountBE);
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
        /// this function calls the SP ModAISCalcSurchargeReview to calc the surcharge review for updating 
        /// after NY-931 is updated.
        /// </summary>
        /// <param name="premAdjPrdId_in"></param>
        /// <param name="premAdjId_in"></param>
        /// <param name="custId_in"></param>
        /// <param name="premAdjPrgId_in"></param>
        /// <param name="addnRtn932_in"></param>
        /// <param name="stateId_in"></param>
        /// <param name="lobId_in"></param>
        /// <param name="comagmtid_in"></param>
        /// <param name="surCodeid_in"></param>
        /// <param name="surTypeId_in"></param>
        /// <param name="createUserId_in"></param>
        /// <param name="debug_in"></param>
        public void CalcSurchargeReview
           (int premAdjPrdId_in, int premAdjId_in, int custId_in, int premAdjPrgId_in, decimal addnRtn932_in,
            int stateId_in, int lobId_in, int comagmtid_in, int surCodeid_in, int surTypeId_in,
           int createUserId_in, bool debug_in)
        {
            new SurchargeDetailsDA().CalcSurchargeReview(premAdjPrdId_in, premAdjId_in, custId_in, premAdjPrgId_in, addnRtn932_in,
                                                   stateId_in, lobId_in, comagmtid_in, surCodeid_in, surTypeId_in,
                                                   createUserId_in,debug_in);
        }
    }
}
#endregion
#endregion