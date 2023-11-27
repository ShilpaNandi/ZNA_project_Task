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
    public class PremiumAdjLRFPostingTaxBS : BusinessServicesBase<PremiumAdjLRFPostingTaxBE, PremiumAdjLRFPostingTaxDA>
    {
        /// <summary>
        /// Retrieve particular PremiumAdjLRFPostingTax  information record
        /// </summary>
        /// <param name="KOSetupId"></param>
        /// <returns></returns>
        public PremiumAdjLRFPostingTaxBE LoadData(int prmAdjLRFPostingTaxId)
        {
            PremiumAdjLRFPostingTaxBE Data = new PremiumAdjLRFPostingTaxBE();
            try
            {
                Data = DA.Load(prmAdjLRFPostingTaxId);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return Data;
        }
        /// <summary>
        /// Retrieve PremiumAdjLRFPosting Taxes information
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public IList<PremiumAdjLRFPostingTaxBE> GetPrmAdjLRFPostingTax(int prmAdjmtId, int prem_adj_perd_id, int prem_adj_pgm_id)
        {
            IList<PremiumAdjLRFPostingTaxBE> result = new List<PremiumAdjLRFPostingTaxBE>();
            PremiumAdjLRFPostingTaxDA dtaxesDA = new PremiumAdjLRFPostingTaxDA();
            try
            {
                result = dtaxesDA.GetPrmAdjLRFPostingTax(prmAdjmtId, prem_adj_perd_id, prem_adj_pgm_id);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return result;
        }
        /// <summary>
        /// Retrieve particular LRF Tax information record
        /// </summary>
        /// <param name="KOSetupId"></param>
        /// <returns></returns>
        public PremiumAdjLRFPostingTaxBE LoadLRFTaxData(int LRFTaxSetupId)
        {
            PremiumAdjLRFPostingTaxBE Data = new PremiumAdjLRFPostingTaxBE();
            try
            {
                Data = DA.Load(LRFTaxSetupId);
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return Data;
        }
        public string AddPREM_ADJ_PERD_TOT_TAX(int intCustomerID, int intPrem_adj_perd_id, int intPrem_adj_pgm_id, int intPrem_adj_id, int intUserID)
        {
            PremiumAdjLRFPostingTaxDA dtaxesDA = new PremiumAdjLRFPostingTaxDA();
            return dtaxesDA.AddPREM_ADJ_PERD_TOT_TAX(intCustomerID, intPrem_adj_perd_id, intPrem_adj_pgm_id, intPrem_adj_id, intUserID);
        }

        public bool Update(PremiumAdjLRFPostingTaxBE paLRFBE)
        {
            bool succeed = false;
            if (paLRFBE.PREM_ADJ_LOS_REIM_FUND_POST_TAX_ID > 0)
            {
                succeed = this.Save(paLRFBE);
            }
            else //On Insert
            {
                //papBE.PremiumAdjustmentProgramID = DA.GetNextPkID().Value;
                succeed = DA.Add(paLRFBE);
            }
            return succeed;
        }
    }
}
