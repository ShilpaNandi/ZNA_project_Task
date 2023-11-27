using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.LSP.Framework.DataAccess;


namespace ZurichNA.AIS.Business.Logic
{
    public class PremiumAdjustmentStatusBS : BusinessServicesBase<PremiumAdjustmentStatusBE, PremumAdjustdmentStatusDA>
    {
        public PremiumAdjustmentStatusBE getPreAdjStatusRow(int PreAdjStaID)
        {
            PremiumAdjustmentStatusBE PreAdjBE = new PremiumAdjustmentStatusBE();
            PreAdjBE = DA.Load(PreAdjStaID);
            return PreAdjBE;
        }
        public IList<PremiumAdjustmentStatusBE> getPreAdjStatusList(int PremAdjID)
        {
            IList<PremiumAdjustmentStatusBE> list = new List<PremiumAdjustmentStatusBE>();
            PremumAdjustdmentStatusDA PremumAdjustmentStatus = new PremumAdjustdmentStatusDA();

            try
            {
                list = PremumAdjustmentStatus.getPremumAdjustmentStatusList(PremAdjID);
            }

            catch (System.Data.SqlClient.SqlException ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            //foreach (AccountBE acct in list)
            //    acct.PremumAdjustmentStatus_ID = (new CustomerContactBS()).getPrimaryContactData(acct.CUSTMR_ID).PremumAdjustmentStatus_ID;
            return list;
        }
        /// <summary>
        /// retrives all Review Feedbacks of Adjustment Status.
        /// </summary>
        /// <param name="AdjID"></param>
        /// <param name="ReviewStage">AdjQC=1 and Recon =2</param>
        /// <returns></returns>
        public DataTable getReviewFeedback(int PremAdjID,int ReviewType)
        {
            return (new PremumAdjustdmentStatusDA()).getReviewFeedback(PremAdjID, ReviewType);
        }
        public bool Update(PremiumAdjustmentStatusBE PremBE)
        {
            bool succeed = false;
            if (PremBE.PremumAdj_sts_ID > 0) //On Update
            {
                succeed = this.Save(PremBE);
            }
            else //On Insert
            {
                succeed = DA.Add(PremBE);
            }
            return succeed;
        }
        //public bool Update(PremiumAdjustmentStatusBE PerBE)
        //{
        //    return this.Save(PerBE);
        //}
        public string getPremiumAdjstmentStatus(int intPremAdjID)
        {

            PremumAdjustdmentStatusDA prmAdjStsDA = new PremumAdjustdmentStatusDA();
            return prmAdjStsDA.getPremiumAdjstmentStatus(intPremAdjID);

        }

        //public string getPremiumAdjstmentStatusOld(int intPremAdjID)
        //{

        //    PremumAdjustdmentStatusDA prmAdjStsDA = new PremumAdjustdmentStatusDA();
        //    return prmAdjStsDA.getPremiumAdjstmentStatusOld(intPremAdjID);

        //}
    }
}
