using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.Business.Logic
{
    public class ExcessNonBillableBS : BusinessServicesBase<ExcessNonBillableBE,ExcessNonBillableDA>
    {
        /// <summary>
        /// Retrieves Excess Non Billable details 
        /// </summary>
        /// <returns>List of ExcessNonBillableBE</returns>
        public IList<ExcessNonBillableBE> getExcNonBillableData(int armsID,int comAmgtID, int custmrID, int prgID)
        {
            ExcessNonBillableDA ExcessNonBil = new ExcessNonBillableDA();
            IList<ExcessNonBillableBE> result = ExcessNonBil.getExcNonBillableData(armsID, comAmgtID, custmrID, prgID);
            return result;
        }
        // <summary>
        /// Retrieves Excess Non Billable details 
        /// </summary>
        /// <returns>List of ExcessNonBillableBE</returns>
        public IList<ExcessNonBillableBE> getExcNonBillableDataLoss(int armsID)
        {
            ExcessNonBillableDA ExcessNonBil = new ExcessNonBillableDA();
            IList<ExcessNonBillableBE> result = ExcessNonBil.getExcNonBillableDataLoss(armsID);
            return result;
        }
        /// <summary>
        /// Retrieves Excess Non Billable details based on active indicator
        /// </summary>
        /// <returns>List of ExcessNonBillableBE</returns>
        public IList<ExcessNonBillableBE> getExcNonBillableDataHideDisLines(int armsID, int comAmgtID, int custmrID, int prgID, bool flag)
        {
            ExcessNonBillableDA ExcessNonBil = new ExcessNonBillableDA();
            IList<ExcessNonBillableBE> result = ExcessNonBil.getExcNonBillableDataHideDisLines(armsID, comAmgtID, custmrID, prgID, flag);
            return result;
        }
        /// <summary>
        /// Retrieves Excess Non Billable details on Armis Los EXC ID
        /// </summary>
        /// <returns>List of ExcessNonBillableBE</returns>
        public ExcessNonBillableBE getExcessNonBilRow(int ArmisLossExcID)
        {
            ExcessNonBillableBE ExcessNonBilBE = new ExcessNonBillableBE();
            ExcessNonBilBE = DA.Load(ArmisLossExcID);
            return ExcessNonBilBE;
        }
        /// <summary>
        /// Insert or update Excess Non Billable details
        /// </summary>
        /// <returns>Result</returns>
        public bool Update(ExcessNonBillableBE ExcessNonBilBE)
        {
            bool succeed = false;
            try
            {
            if (ExcessNonBilBE.ARMIS_LOS_EXC_ID > 0) //On Update
            {
                succeed = this.Save(ExcessNonBilBE);

            }
            else //On Insert
            {
                ExcessNonBilBE.ARMIS_LOS_EXC_ID = DA.GetNextPkID().Value;

                succeed = DA.Add(ExcessNonBilBE);
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
        /// Insert or update Excess Non Billable details
        /// </summary>
        /// <returns>Result</returns>
        public bool PerformLimiting(int CustmrID, int PremiumAdjustProgID)
        {
            ExcessNonBillableDA excess = new ExcessNonBillableDA();
            bool succeed = false;
            try
            {
                excess.PerformLimiting(CustmrID, PremiumAdjustProgID);

            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                return succeed;
            }
            return succeed;

        }

        /// <summary>
        /// Retrieves Excess Non Billable details based on Armis Los EXC ID
        /// </summary>
        /// <param name="intarmsLossExcID"></param>
        /// <returns></returns>
        public IList<ExcessNonBillableBE> getExcNonBillableData(int intarmsLossExcID)
        {
            ExcessNonBillableDA ExcessNonBil = new ExcessNonBillableDA();
            IList<ExcessNonBillableBE> result = ExcessNonBil.getExcNonBillableData(intarmsLossExcID);
            return result;
        }
    }
}
