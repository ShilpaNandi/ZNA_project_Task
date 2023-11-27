using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    //DataAccessor for PREM_ADJ_COMB_ELEMT details
    public class PremiumAdjCombElementsDA : DataAccessor<PREM_ADJ_COMB_ELEMT, PremiumAdjCombElementsBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Method to Get the Records form PREM_ADJ_COMB_ELEMT Tabel
        /// Based on AccountID,PremAdjID,PremAdjPrdID
        /// </summary>
        /// <param name="intAccountID"></param>
        /// <param name="intPremAdjID"></param>
        /// <param name="intPremAdjPerdID"></param>
        /// <returns>IList<PremiumAdjCombElementsBE></returns>
        public IList<PremiumAdjCombElementsBE> GetPremAdjCombElements(int intAccountID, int intPremAdjID, int intPremAdjPerdID)
        {
            IList<PremiumAdjCombElementsBE> result = new List<PremiumAdjCombElementsBE>();
            if (this.Context == null)
                this.Initialize();


            IQueryable<PremiumAdjCombElementsBE> query = from PremAdjCombEle in this.Context.PREM_ADJ_COMB_ELEMTs
                                                         where PremAdjCombEle.custmr_id == intAccountID
                                                             //                                                         && PremAdjCombEle.prem_adj_pgm_id == intPrgperdID
                                                         && PremAdjCombEle.prem_adj_id == intPremAdjID
                                                         && PremAdjCombEle.prem_adj_perd_id == intPremAdjPerdID
                                                         select new PremiumAdjCombElementsBE
                                                         {
                                                             PREM_ADJ_COMB_ELEMNTS_ID = PremAdjCombEle.prem_adj_comb_elemts_id,
                                                             PREM_ADJ_PERD_ID = PremAdjCombEle.prem_adj_perd_id,
                                                             PREM_ADJ_ID = PremAdjCombEle.prem_adj_id,
                                                             CUSTMR_ID = PremAdjCombEle.custmr_id,
                                                             RETRO_BASIC_PREM_AMT = PremAdjCombEle.retro_basic_prem_amt,
                                                             RETRO_LOS_FCTR_AMT = PremAdjCombEle.retro_los_fctr_amt,
                                                             RETRO_TAX_MULTI_RT = PremAdjCombEle.retro_tax_multi_rt,
                                                             RETRO_SUBTOT_AMT = PremAdjCombEle.retro_subtot_amt,
                                                             DEDTBL_MAX_AMT = PremAdjCombEle.dedtbl_max_amt,
                                                             DEDTBL_MIN_AMT = PremAdjCombEle.dedtbl_min_amt,
                                                             DEDTBL_SUBTOT_AMT = PremAdjCombEle.dedtbl_subtot_amt,
                                                             DEDTBL_MAX_LESS_AMT = PremAdjCombEle.dedtbl_max_less_amt,
                                                             UPDATE_DATE=PremAdjCombEle.updt_dt,
                                                             //                                                            PREM_ADJ_PGM_ID=PremAdjCombEle.prem_adj_perd_id,
                                                             //                                                            COMB_ELEMTS_ID=PremAdjCombEle.comb_elemts_id,

                                                         };



            result = query.ToList();
            return result;
        }
    }
}
