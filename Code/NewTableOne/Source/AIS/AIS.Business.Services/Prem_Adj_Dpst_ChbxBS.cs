using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.DAL.Logic.Queries;

namespace ZurichNA.AIS.Business.Logic
{
    public class Prem_Adj_Dpst_ChbxBS : BusinessServicesBase<Prem_Adj_Dpst_ChbxBE, Prem_Adj_Dpst_ChbxDA>
    {
        private LookupBS lookup;
        public Prem_Adj_Dpst_ChbxBS()
        {
            lookup = new LookupBS();
        }

        /// <summary>
        /// Get Adjparameter setup details for a Program Period and Customer ID
        /// </summary>
        /// <param name="ProgramPeriodID"></param>
        /// <param name="CstmrID"></param>
        /// <returns></returns>
        public IList<Prem_Adj_Dpst_ChbxBE> getAdjDpstChbx(int PremAdjPgmSetupID, int PremAdjPgmID, int CstmrID)
        {
            IList<Prem_Adj_Dpst_ChbxBE> PremAdjDpstChbx;
            try
            {
                PremAdjDpstChbx = new Prem_Adj_Dpst_ChbxDA().getAdjDpstChbx(PremAdjPgmSetupID, PremAdjPgmID, CstmrID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (PremAdjDpstChbx);
        }

        

        /// <summary>
        /// Load single record using framework
        /// for Adjustment Parameter Setup using primary key Adjustment paramater Settup ID
        /// </summary>
        /// <param name="adjPrmsetupID"></param>
        /// <returns></returns>
        public Prem_Adj_Dpst_ChbxBE getAdjParamRow(int PremAdjPgmSetupID)
        {
            Prem_Adj_Dpst_ChbxBE PremAdjDpstChbxRow = new Prem_Adj_Dpst_ChbxBE();
            PremAdjDpstChbxRow = DA.Load(PremAdjPgmSetupID);

            return PremAdjDpstChbxRow;
        }

        /// <summary>
        /// UPDate or Add Adjustment Parameter Setup values 
        /// </summary>
        /// <param name="adjInfo"></param>
        /// <returns></returns>
        public bool Update(Prem_Adj_Dpst_ChbxBE adjInfo)
        {
            bool suceeded = false;
            try
            {
                if (adjInfo.prem_adj_dpst_chbx_id > 0)
                {
                    suceeded = DA.Update(adjInfo);
                }
                else
                {
                    suceeded = DA.Add(adjInfo);
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return suceeded;
        }
        public bool deletePol(int custmrID, int adjParmSetupID)
        {
            bool policydelete;
            try
            {
                policydelete = new Adj_paramet_PolDA().deleteAdjParmetPol(custmrID, adjParmSetupID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (policydelete);

        }
        public IList<Prem_Adj_Dpst_ChbxBE> getAdjReviewEscrow(int AdjPgmSetupID, int PremAdjPgmID, int CstmrID, int adjParamTypeID)
        {
            IList<Prem_Adj_Dpst_ChbxBE> PremAdjDpstChbx;
            try
            {
                PremAdjDpstChbx = new Prem_Adj_Dpst_ChbxDA().getAdjDpstChbx(AdjPgmSetupID, PremAdjPgmID, CstmrID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            PremAdjDpstChbx = PremAdjDpstChbx.Where(adjDpstChbx => adjDpstChbx.AdjparameterTypeID == adjParamTypeID).ToList();
            return (PremAdjDpstChbx);
        }

        //Method used for concurrency check
        //public string getAdjParamResult(int adjPgmSetupID, int programPeriodID, int cstmrID, int adjParamTypeID, bool strDpstInd)
        //{
        //    string strResult = string.Empty; ;
        //    try
        //    {
        //        strResult = new Prem_Adj_Dpst_ChbxDA().getAdjParamResult(adjPgmSetupID, programPeriodID, cstmrID, adjParamTypeID, strDpstInd);
        //    }
        //    catch (System.Data.SqlClient.SqlException ex)
        //    {
        //        RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
        //        throw myException;
        //    }
        //    return (strResult);
        //}
        public int getAdjParamResultOther(int adjPgmSetupID, int premAdjPgmID, int cstmrID, int? adjParamTypeID)
        {
            int idResult = 0;
            try
            {
                idResult = new Prem_Adj_Dpst_ChbxDA().getAdjDpstChbxResultOther(adjPgmSetupID, premAdjPgmID, cstmrID, adjParamTypeID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (idResult);
        }
        //public string getAdjParamResultOther(int adjPgmSetupID, int premAdjPgmID, int cstmrID, int? adjParamTypeID)
        //{
        //    string idResult = null;
        //    try
        //    {
        //        idResult = new Prem_Adj_Dpst_ChbxDA().getAdjDpstChbxResultOther(adjPgmSetupID, premAdjPgmID, cstmrID, adjParamTypeID);
        //    }
        //    catch (System.Data.SqlClient.SqlException ex)
        //    {
        //        RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
        //        throw myException;
        //    }
        //    return (idResult);
        //}
    }
}
