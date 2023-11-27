using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.Business.Logic
{
    public class Adj_Parameter_SetupBS:BusinessServicesBase<AdjustmentParameterSetupBE, Adj_Parameter_setupDA>
    {
        private LookupBS lookup;
        //private PolicyBS Policydetails;
        public Adj_Parameter_SetupBS()
        {
            lookup = new LookupBS();
            //Policydetails = new PolicyBS();
        }

        /// <summary>
        /// Get Adjparameter setup details for a Program Period and Customer ID
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <param name="CstmrID"></param>
        /// <returns></returns>
        public IList<AdjustmentParameterSetupBE> getAdjParamtr(int ProgramPeriodID, int CstmrID)
        {
            IList<AdjustmentParameterSetupBE> AdjParameterSetup;
            try
            {
                AdjParameterSetup = new Adj_Parameter_setupDA().getAdjParamtr(ProgramPeriodID, CstmrID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            //IList<AdjustmentParameterSetupBE> AdjParameterSetupBE = this.buildList(AdjParameterSetup);

            return (AdjParameterSetup); 
        }

        /// <summary>
        /// retrives Adjparameter setup details for ILRF Setup
        /// </summary>
        /// <param name="programPeriodID"></param>
        /// <param name="cstmrID"></param>
        /// <param name="adjParamTypeID"></param>
        /// <returns>List of AdjParameters</returns>
        public AdjustmentParameterSetupBE getAdjParamsforILRF(int programPeriodID, int cstmrID, int adjParamTypeID)
        {
            AdjustmentParameterSetupBE AdjParameterSetup;
            try
            {
                AdjParameterSetup = new Adj_Parameter_setupDA().getAdjParamsforILRF(programPeriodID, cstmrID, adjParamTypeID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (AdjParameterSetup);
        }

        ///// <summary>
        ///// This method is not used. Was written initially
        ///// </summary>
        ///// <param name="ProgramPeriodID"></param>
        ///// <param name="CstmrID"></param>
        ///// <returns></returns>
        //public IList<AdjustmentParameterSetupBE> getAdjParamtrLCF(int ProgramPeriodID, int CstmrID)
        //{

        //    IList<AdjustmentParameterSetupBE> AdjParameterSetupLCF;
        //    try
        //    {
        //        AdjParameterSetupLCF = new Adj_Parameter_setupDA().getAdjParamtrLCF(ProgramPeriodID, CstmrID);
        //    }
        //    catch (System.Data.SqlClient.SqlException ex)
        //    {
        //        RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
        //        throw myException;
        //    }
        //    //IList<AdjustmentParameterSetupBE> AdjParameterSetupBE = this.buildList(AdjParameterSetup);

        //    return (AdjParameterSetupLCF);
        //}

        /// <summary>
        /// Get Adjustmet Parameter Setup values for Program period, CustomerID and Included in ERP(true/false)
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <param name="CstmrID"></param>
        /// <param name="IncldERP"></param>
        /// <returns></returns>
        public IList<AdjustmentParameterSetupBE> getAdjParamtERPTrue(int ProgramPeriodID, int CstmrID, Boolean IncldERP)
        {
           IList<AdjustmentParameterSetupBE> AdjParameterSetupERP;
           try
           {
               AdjParameterSetupERP = new Adj_Parameter_setupDA().getAdjParamtERPTrue(ProgramPeriodID, CstmrID, IncldERP);
           }
           catch (System.Data.SqlClient.SqlException ex)
           {
               RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
               throw myException;
           }
           return (AdjParameterSetupERP);
        }

        /// <summary>
        /// Load single record using framework
        /// for Adjustment Parameter Setup using primary key Adjustment paramater Settup ID
        /// </summary>
        /// <param name="adjPrmsetupID"></param>
        /// <returns></returns>
        public AdjustmentParameterSetupBE getAdjParamRow(int adjPrmsetupID)
        {
            AdjustmentParameterSetupBE AdjParamRow = new AdjustmentParameterSetupBE();
            AdjParamRow = DA.Load(adjPrmsetupID);

            return AdjParamRow;
        }

       /// <summary>
       /// UPDate or Add Adjustment Parameter Setup values 
       /// </summary>
       /// <param name="LBAadjinfo"></param>
       /// <returns></returns>
        public bool Update(AdjustmentParameterSetupBE LBAadjinfo)
        {
            bool suceeded = false;
            try
            {
                if (LBAadjinfo.adj_paramet_setup_id > 0)
                {
                    suceeded = DA.Update(LBAadjinfo);
                }
                else
                {
                    suceeded = DA.Add(LBAadjinfo);
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return suceeded;
        }
        public IList<AdjustmentParameterSetupBE> getAdjReviewEscrow(int ProgramPeriodID, int CstmrID, int intAdjparameterTypeID)
        {
            IList<AdjustmentParameterSetupBE> AdjParameterSetup;
            try
            {
                AdjParameterSetup = new Adj_Parameter_setupDA().getAdjParamtr(ProgramPeriodID, CstmrID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            AdjParameterSetup = AdjParameterSetup.Where(adjSetup => adjSetup.AdjparameterTypeID == intAdjparameterTypeID).ToList();
            return (AdjParameterSetup);
        }

       //Method used for concurrency check
        public string getAdjParamResult(int ProgramPeriodID, int CstmrID, int intAdjparameterTypeID, bool strIncluded)
        {
            string strResult = string.Empty;
            try
            {
                strResult = new Adj_Parameter_setupDA().getAdjParamResult(ProgramPeriodID, CstmrID, intAdjparameterTypeID, strIncluded);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (strResult);
        }

        //Method used for concurrency check
        public string getAdjParamResultOther(int ProgramPeriodID, int CstmrID, int intAdjparameterTypeID)
        {
            string strResult = string.Empty;
            try
            {
                strResult = new Adj_Parameter_setupDA().getAdjParamResultOther(ProgramPeriodID, CstmrID, intAdjparameterTypeID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (strResult);
        }

    }
}
