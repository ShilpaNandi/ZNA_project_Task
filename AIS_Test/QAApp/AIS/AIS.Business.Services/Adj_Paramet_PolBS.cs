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
    public class Adj_Paramet_PolBS: BusinessServicesBase<AdjustmentParameterPolicyBE, Adj_paramet_PolDA> 
    {

        /// <summary>
        /// This method is used to get a list of all policies for a given
        /// Adjustment Parameter Setup ID
        /// </summary>
        /// <param name="AdjParmSetupID"></param>
        /// <returns></returns>
        public IList<AdjustmentParameterPolicyBE> getAdjParamtrPol(int AdjParmSetupID)
        {
            IList<AdjustmentParameterPolicyBE> AdjParameterPolcy;
            try
            {
                AdjParameterPolcy = new Adj_paramet_PolDA().getAdjParamtrPol(AdjParmSetupID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }


            return (AdjParameterPolcy);
        }

        /// <summary>
        /// This method is used to Delete the policies for the given
        /// CustomerID\AccountID and Adjustment Parameter Setup ID
        /// </summary>
        /// <param name="custmrID"></param>
        /// <param name="adjParmSetupID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// deletes all Prem adj Prg Setup Policies for ILRF Setup
        /// </summary>
        /// <param name="custmrID"></param>
        /// <param name="programPeriodID"></param>
        /// <param name="adjParmSetupID"></param>
        /// <returns>true false</returns>
        public bool DeleteAdjPrmPol(int custmrID, int programPeriodID, int adjParmSetupID)
        {
            bool boolPolDeleted;
            try
            {
                boolPolDeleted = new Adj_paramet_PolDA().DeleteAdjPrmPol(custmrID, programPeriodID, adjParmSetupID);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }

            return (boolPolDeleted);
        }

        /// <summary>
        /// This method is used to Add the selected policies
        /// </summary>
        /// <param name="AdjParmPolList"></param>
        /// <returns></returns>
        public bool Update(AdjustmentParameterPolicyBE AdjParmPolList)
        {
            IList<AdjustmentParameterPolicyBE> LBAAdjPolList = new List<AdjustmentParameterPolicyBE>();
            bool suceeded = false;

                //AdjParmPolList.CREATE_USER_ID = 1;
                //AdjParmPolList.CREATE_DATE = DateTime.Now;
            try
            {
                suceeded = DA.Add(AdjParmPolList);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            
            return suceeded;
        }
    }

    
}
