using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;

using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    //DataAccessor for PremiumAdj MiscInvoice details
   public class PremiumAdjustmentEscrowDA:DataAccessor<PREM_ADJ_PARMET_SETUP,PremiumAdjustmentEscrowBE,AISDatabaseLINQDataContext>
    {
       /// <summary>
        /// Method to Retrieve the Records from PREM_ADJ_PARMET_SETUP Table 
        /// Based on AccountID,PremAdjPerdID,PremAdjPgrID,PremAdjPgrSetupID
       /// </summary>
       /// <param name="intaccountID"></param>
       /// <param name="intPremAdjPerdID"></param>
       /// <param name="intPremAdjID"></param>
       /// <param name="intPremAdjPgrID"></param>
       /// <param name="intPremAdjPgrSetupID"></param>
        /// <returns>IList<PremiumAdjustmentEscrowBE></returns>
       public IList<PremiumAdjustmentEscrowBE> GetPremAdjEscrowInfo(int intaccountID, int intPremAdjPerdID, int intPremAdjID, int intPremAdjPgrID, int intPremAdjPgrSetupID)
       {
           IList<PremiumAdjustmentEscrowBE> result = new List<PremiumAdjustmentEscrowBE>();

           IQueryable<PremiumAdjustmentEscrowBE> query =
               from premadjParSetup in this.Context.PREM_ADJ_PARMET_SETUPs
               where premadjParSetup.custmr_id==intaccountID && premadjParSetup.prem_adj_perd_id==intPremAdjPerdID
               && premadjParSetup.prem_adj_id==intPremAdjID && premadjParSetup.prem_adj_pgm_id==intPremAdjPgrID
               && premadjParSetup.prem_adj_pgm_setup_id==intPremAdjPgrSetupID
               select new PremiumAdjustmentEscrowBE
               {
                   PREM_ADJ_PARMET_SETUP_ID=premadjParSetup.prem_adj_parmet_setup_id,
                   ESCR_ADJ_PAID_LOS_AMT=premadjParSetup.escr_adj_paid_los_amt,
                   ESCR_ADJ_AMT=premadjParSetup.escr_adj_amt,
                   ESCR_AMT=premadjParSetup.escr_amt,
                   ESCR_PREVIOUSULY_BILED_AMT=premadjParSetup.escr_prevly_biled_amt,
                   TOT_AMT=premadjParSetup.tot_amt,
                   UPDATE_DATE=premadjParSetup.updt_dt
               };
           result = query.ToList();
           return result;
       }
    }
}
