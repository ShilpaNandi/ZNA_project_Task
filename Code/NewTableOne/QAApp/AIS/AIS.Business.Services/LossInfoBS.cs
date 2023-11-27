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
    public class LossInfoBS : BusinessServicesBase<LossInfoBE, LossInfoDA>
    {

        /// <summary>
        /// Retrieves LossInfoData for all Loss Info
        /// </summary>
        /// <returns>List of LossInfoBE</returns>
        public IList<LossInfoBE> getLossInfoDataAdjNo(DateTime ValDate, int custmrID, int prgID, int adjNo)
        {
            LossInfoDA lossInfo = new LossInfoDA();
            IList<LossInfoBE> result = lossInfo.getLossInfoDataAdjNo(ValDate, custmrID, prgID, adjNo);
            return result;
        }
        /// <summary>
        /// Retrieves LossInfoData for all Loss Info
        /// </summary>
        /// <returns>List of LossInfoBE</returns>
        public IList<LossInfoBE> getLossInfoData(DateTime ValDate, int custmrID, int prgID)
        {
            LossInfoDA lossInfo = new LossInfoDA();
            IList<LossInfoBE> result = lossInfo.getLossInfoData(ValDate, custmrID, prgID);
            return result;
        }
        /// <summary>
        /// Retrieves LossInfoData for all Loss Info based on active indicator
        /// </summary>
        /// <returns>List of LossInfoBE</returns>
        public IList<LossInfoBE> getLossInfoDataHideDisLinesAdjNo(DateTime ValDate, int custmrID, int prgID, bool flag, int adjNo)
        {
            LossInfoDA lossInfo = new LossInfoDA();
            IList<LossInfoBE> result = lossInfo.getLossInfoDataHideDisLinesAdjNo(ValDate, custmrID, prgID, flag,adjNo);
            return result;
        }
        /// <summary>
        /// Retrieves LossInfoData for all Loss Info based on active indicator
        /// </summary>
        /// <returns>List of LossInfoBE</returns>
        public IList<LossInfoBE> getLossInfoDataHideDisLines( DateTime ValDate, int custmrID, int prgID, bool flag)
        {
            LossInfoDA lossInfo = new LossInfoDA();
            IList<LossInfoBE> result = lossInfo.getLossInfoDataHideDisLines( ValDate, custmrID, prgID, flag);
            return result;
        }
        /// <summary>
        /// Retrieves LossInfoData based on Armis Los ID
        /// </summary>
        /// <returns>List of LossInfoBE</returns>
        public LossInfoBE getLossInfoRow(int ArmisLossID)
        {
            LossInfoBE lossInfoBE = new LossInfoBE();
            lossInfoBE = DA.Load(ArmisLossID);
            return lossInfoBE;
        }
        /// <summary>
        /// Insert or update LossInfoData for all Loss Info
        /// </summary>
        /// <returns>Result</returns>
        public bool Update(LossInfoBE lossInfoBE)
        {
            bool succeed = false;
            try
            {
            if (lossInfoBE.ARMIS_LOS_ID > 0) //On Update
            {
                succeed = this.Save(lossInfoBE);

            }
            else //On Insert
            {
                lossInfoBE.ARMIS_LOS_ID = DA.GetNextPkID().Value;
                
                succeed = DA.Add(lossInfoBE);
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
        /// Retrieves LOB 
        /// </summary>
        /// <returns>List of LOB</returns>
        public IList<LookupBE> getLOBLookUpData()
        {
            LossInfoDA lossInfo = new LossInfoDA();
            IList<LookupBE> lkBE;
            lkBE = new LossInfoDA().getLOBLookUpData();
            LookupBE lukBE = new LookupBE();
            //lukBE.LookUpID = 0;
            lukBE.LookUpName = "(Select)";
            lkBE.Insert(0, lukBE);
            return lkBE;
            
        }
       /// <summary>
        /// Retrieves LossInfoData
       /// </summary>
       /// <param name="startPolicyEffDate"></param>
       /// <param name="endPolicyEffDate"></param>
       /// <param name="custmrID"></param>
       /// <param name="prgID"></param>
       /// <returns></returns>

        public IList<LossInfoBE> getLossInfoByPolicyData(DateTime ValDate, int custmrID, int prgID)
        {
            LossInfoDA lossInfo = new LossInfoDA();
            IList<LossInfoBE> result = lossInfo.getLossInfoByPolicyData(ValDate, custmrID, prgID);
            return result;
        }
        /// <summary>
        /// Retrieves LossInfoData based on Armis Los ID
        /// </summary>
        /// <param name="intArmisID"></param>
        /// <returns></returns>
        public IList<LossInfoBE> getLossInfoData(int intArmisID)
        {
            LossInfoDA lossInfo = new LossInfoDA();
            IList<LossInfoBE> result = lossInfo.getLossInfoData(intArmisID);
            return result;
        }

        public IList<LossInfoBE> getLossInfoByPolicyDataAdjNo(DateTime ValDate, int custmrID, int prgID, int AdjNo)
        {
            LossInfoDA lossInfo = new LossInfoDA();
            IList<LossInfoBE> result = lossInfo.getLossInfoByPolicyDataAdjNo(ValDate, custmrID, prgID,AdjNo);
            return result;
        }
        public IList<LossInfoBE> getLossInfoByLOBDataAdjNo(DateTime ValDate, int custmrID, int prgID, int AdjNo)
        {
            LossInfoDA lossInfo = new LossInfoDA();
            IList<LossInfoBE> result = lossInfo.getLossInfoByLOBDataAdjNo(ValDate, custmrID, prgID, AdjNo);
            return result;
        }
        public IList<LossInfoBE> getLossInfoByLOBDataAdjNoCovg(DateTime ValDate, int custmrID, int prgID, int AdjNo, string Lob)
        {
            LossInfoDA lossInfo = new LossInfoDA();
            IList<LossInfoBE> result = lossInfo.getLossInfoByLOBDataAdjNoCovg(ValDate, custmrID, prgID, AdjNo, Lob);
            return result;
        }
        /// <summary>
        /// Retrieves LossInfoData
        /// </summary>
        /// <param name="startPolicyEffDate"></param>
        /// <param name="endPolicyEffDate"></param>
        /// <param name="custmrID"></param>
        /// <param name="prgID"></param>
        /// <returns></returns>

        public IList<LossInfoBE> getLossInfoByLOBData(DateTime ValDate, int custmrID, int prgID)
        {
            LossInfoDA lossInfo = new LossInfoDA();
            IList<LossInfoBE> result = lossInfo.getLossInfoByLOBData(ValDate, custmrID, prgID);
            return result;
        }
        public IList<LossInfoBE> getLossInfoByLOBDataCovg(DateTime ValDate, int custmrID, int prgID, string Lob)
        {
            LossInfoDA lossInfo = new LossInfoDA();
            IList<LossInfoBE> result = lossInfo.getLossInfoByLOBDataCovg(ValDate, custmrID, prgID, Lob);
            return result;
        }

        public IList<LossInfoBE> getLossInfoDataAdjNoComl(DateTime ValDate, int custmrID, int prgID, int AdjNo, int Coml)
        {
            LossInfoDA lossInfo = new LossInfoDA();
            IList<LossInfoBE> result = lossInfo.getLossInfoDataAdjNoComl(ValDate, custmrID, prgID, AdjNo, Coml);
            return result;
        }
        public IList<LossInfoBE> getLossInfoDataComl(DateTime ValDate, int custmrID, int prgID, int Coml)
        {
            LossInfoDA lossInfo = new LossInfoDA();
            IList<LossInfoBE> result = lossInfo.getLossInfoDataComl(ValDate, custmrID, prgID, Coml);
            return result;
        }

        public int? GetAdjustmentNumber(int custmr_id, DateTime valn_dt)
        {
            int? AdjNo = null;
            LossInfoDA lossInfo = new LossInfoDA();
            AdjNo = lossInfo.GetAdjustmentNumber(custmr_id, valn_dt);
            return AdjNo;
        }

    }
}
