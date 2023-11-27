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
    public class AdjustmentManagementBS : BusinessServicesBase<AdjustmentManagementBE, AdjustmentManagementDA>
    {
        /// <summary>
        /// Retrieves the row from Prem_ADJ Table based on Prem_ADj_ID
        /// </summary>
        /// <param name="PremAdjMgmtID">Person ID PrimaryKey</param>
        /// <returns>AdjustmentManagementBE based on PremAdjMgmtID</returns>
        public AdjustmentManagementBE getPremMgmtRow(int PremAdjMgmtID)
        {
            AdjustmentManagementBE PremAdjMgmtBE = new AdjustmentManagementBE();
            PremAdjMgmtBE = DA.Load(PremAdjMgmtID);
            return PremAdjMgmtBE;
        }

        /// <summary>
        /// update Prem Adjustments Details Screen QC20%, Pending Indicator, Pending Reason
        /// </summary>
        /// <returns>Result</returns>
        public bool Update(AdjustmentManagementBE prmadjBE)
        {
            bool succeed = false;
            try
            {
                if (prmadjBE.prem_adjID > 0) //On Update
                {
                    succeed = this.Save(prmadjBE);
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
        /// Get Adjustment Management details for given below Parameters
        /// </summary>
        /// <param name="AcctNameID"></param>
        /// <param name="AdjStatusID"></param>
        /// <param name="InvNmbr"></param>
        /// <param name="PersnID"></param>
        /// <param name="InvFrmDt"></param>
        /// <param name="InvToDt"></param>
        /// <param name="ValutnDt"></param>
        /// <returns></returns>
        public IList<AdjustmentManagementBE> getAdjManagement(int AcctNameID, int AdjStatusID, String InvNmbr, int PersnID, DateTime InvFrmDt, DateTime InvToDt, DateTime ValutnDt)
        {
            IList<AdjustmentManagementBE> AdjManagementDtls;
            try
            {
                AdjManagementDtls = new AdjustmentManagementDA().getAdjManagement(AcctNameID, AdjStatusID, InvNmbr, PersnID, InvFrmDt, InvToDt, ValutnDt);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return (AdjManagementDtls);
        }
    }
}
