using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;

namespace ZurichNA.AIS.Business.Logic
{
    public class SteppedFactorBS : BusinessServicesBase<SteppedFactorBE, SteppedFactorsDA>
    {

        /// <summary>
        /// Constructors for SteppedFactorBS Class
        /// </summary>
        public SteppedFactorBS()
        {

        }

        /// <summary>
        /// Function that retrieves a list of Stepped Factor
        /// By having input value of Policy ID
        /// </summary>
        /// <param name="policyID">Policy ID</param>
        /// <returns>List of Stepped Factor Row - IList of SteppedFactorBE</returns>
        public IList<SteppedFactorBE> GetSteppedFactorData(int policyID)
        {
            IList<SteppedFactorBE> steppedFactorList = new List<SteppedFactorBE>();
            SteppedFactorsDA steppedFactors = new SteppedFactorsDA();
            try
            {
                steppedFactorList = steppedFactors.GetSteppedFactorData(policyID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return steppedFactorList;
        }

        /// <summary>
        /// Function that retrieves a list of Stepped Factor
        /// By having input value of PremADjPgmID
        /// </summary>
        /// <param name="policyID">Policy ID</param>
        /// <returns>List of Stepped Factor Row - IList of SteppedFactorBE</returns>
        public IList<SteppedFactorBE> GetSteppedFactorDataByPgmID(int PremAdjPgmID)
        {
            IList<SteppedFactorBE> steppedFactorList = new List<SteppedFactorBE>();
            SteppedFactorsDA steppedFactors = new SteppedFactorsDA();
            try
            {
                steppedFactorList = steppedFactors.GetSteppedFactorDataByPgmID(PremAdjPgmID);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return steppedFactorList;
        }


        /// <summary>
        /// used to return List of SteppedFactors by a SteppedFactorID, Using framwork
        /// </summary>
        /// <param name="STEPPED_FACTOR_ID">STEPPED_FACTOR_ID</param>
        /// <returns>return List of SteppedFactors s</returns>
        public SteppedFactorBE getGetSteppedFactor(int STEPPED_FACTOR_ID)
        {
            SteppedFactorBE stepBE = new SteppedFactorBE();
            stepBE = DA.Load(STEPPED_FACTOR_ID);
            return stepBE;
        }
        /// <summary>
        /// Function that is used to add / update the detail of Stepped Factor
        /// </summary>
        /// <param name="steppedFactor">Stepped Factor Detail</param>
        /// <returns>Boolean Flag to indicate whether its succeeded or not</returns>
        public bool Update(SteppedFactorBE steppedFactor)
        {
            IList<SteppedFactorBE> steppedFactorList = new List<SteppedFactorBE>();
            bool suceeded = false;

            if (steppedFactor.STEPPED_FACTOR_ID > 0)
            {                
                suceeded = DA.Update(steppedFactor);
            }
            else
            {                
                suceeded = DA.Add(steppedFactor);
            }
            return suceeded;
        }

        public bool deleteSteppedFactors(int PolicyID, int CustmrID, int PremAdjPgmID)
        {
            bool SteppedFactordelete;
            try
            {
                SteppedFactordelete = new SteppedFactorsDA().deleteSteppedFactors(PolicyID, CustmrID, PremAdjPgmID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (SteppedFactordelete);

        }

        public bool deleteSteppedFactors(int CustmrID, int PremAdjPgmID)
        {
            bool SteppedFactordelete;
            try
            {
                SteppedFactordelete = new SteppedFactorsDA().deleteSteppedFactors(CustmrID, PremAdjPgmID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (SteppedFactordelete);

        }
    }
}
