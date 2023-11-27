using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    //DataAccessor for PrmAdjNYSIF details
    public class PremiumAdjNYScndInjuryFundDA : DataAccessor<PREM_ADJ_NY_SCND_INJR_FUND, PremiumAdjNYScndInjuryFundBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Method to Retrieve the records from PrmAdjNYSIF Table 
        /// Based on AccountID,PremAdjID,PremAdjPeriodID
        /// </summary>
        /// <param name="intAccountID"></param>
        /// <param name="intPremAdjID"></param>
        /// <param name="intPremAdjPerdID"></param>
        /// <returns>IList<PremiumAdjNYScndInjuryFundBE></returns>
        public IList<PremiumAdjNYScndInjuryFundBE> GetPremAdjNYSIF(int intAccountID, int intPremAdjID, int intPremAdjPerdID,int intPremAdjPgmID)
        {
            IList<PremiumAdjNYScndInjuryFundBE> result = new List<PremiumAdjNYScndInjuryFundBE>();
            if (this.Context == null)
                this.Initialize();


            IQueryable<PremiumAdjNYScndInjuryFundBE> query = from PremAdjNYSIF in this.Context.PREM_ADJ_NY_SCND_INJR_FUNDs
                                                             where PremAdjNYSIF.custmr_id == intAccountID
                                                         && PremAdjNYSIF.prem_adj_id == intPremAdjID
                                                         && PremAdjNYSIF.prem_adj_perd_id == intPremAdjPerdID
                                                         && PremAdjNYSIF.prem_adj_pgm_id==intPremAdjPgmID
                                                             select new PremiumAdjNYScndInjuryFundBE
                                                         {
                                                             PREM_ADJ_NY_SCND_INJR_FUND_ID = PremAdjNYSIF.prem_adj_ny_scnd_injr_fund_id,
                                                             PREM_ADJ_PERD_ID = PremAdjNYSIF.prem_adj_perd_id,
                                                             PREM_ADJ_ID = PremAdjNYSIF.prem_adj_id,
                                                             PREM_ADJ_PGM_ID=PremAdjNYSIF.prem_adj_pgm_id,
                                                             COML_AGMT_ID=PremAdjNYSIF.coml_agmt_id,
                                                             CUSTMR_ID = PremAdjNYSIF.custmr_id,
//                                                             PREM_ADJ_CMMNT_ID = PremAdjNYSIF.prem_adj_cmmnt_id,
                                                             INCUR_LOS_AMT = PremAdjNYSIF.incur_los_amt,
                                                             LOS_CONV_FCTR_RT = PremAdjNYSIF.los_conv_fctr_rt,
                                                             CNVT_LOS_AMT = PremAdjNYSIF.cnvt_los_amt,
                                                             BASIC_DEDTBL_PREM_AMT = PremAdjNYSIF.basic_dedtbl_prem_amt,
                                                             TAX_MULTI_RT = PremAdjNYSIF.tax_multi_rt,
                                                             CNVT_TOT_LOS_AMT = PremAdjNYSIF.cnvt_tot_los_amt,
                                                             NY_PREM_DISC_AMT = PremAdjNYSIF.ny_prem_disc_amt,
                                                             NY_SCND_INJR_FUND_RT = PremAdjNYSIF.ny_scnd_injr_fund_rt,
                                                             REVD_NY_SCND_INJR_FUND_AMT = PremAdjNYSIF.revd_ny_scnd_injr_fund_amt,
                                                             NY_TAX_DUE_AMT = PremAdjNYSIF.ny_tax_due_amt,
                                                             PREV_RSLT_AMT = PremAdjNYSIF.prev_rslt_amt,
                                                             NY_SCND_INJR_FUND_AUDT_AMT = PremAdjNYSIF.ny_scnd_injr_fund_audt_amt,
                                                             CURR_ADJ_AMT = PremAdjNYSIF.curr_adj_amt,
                                                             BASIC_CNVT_LOS_AMT = PremAdjNYSIF.cnvt_los_amt,
                                                             NY_ERND_RETRO_PREM_AMT = PremAdjNYSIF.ny_ernd_retro_prem_amt,
                                                             UPDATE_DATE=PremAdjNYSIF.updt_dt
                                                         };



            result = query.ToList();
            return result;
        }
       /// <summary>
        /// Method to Retrive the record from PremAdjNYSIF Table Based on PremAdjNYSIFID
       /// </summary>
       /// <param name="intPremAdjNYSIFID"></param>
        /// <returns>PremiumAdjNYScndInjuryFundBE</returns>
        public PremiumAdjNYScndInjuryFundBE GetPremAdjNYSIFRow(int intPremAdjNYSIFID)
        {
            PremiumAdjNYScndInjuryFundBE result = new PremiumAdjNYScndInjuryFundBE();
            if (this.Context == null)
                this.Initialize();


          IQueryable<PremiumAdjNYScndInjuryFundBE> query = from PremAdjNYSIF in this.Context.PREM_ADJ_NY_SCND_INJR_FUNDs
                                                           where PremAdjNYSIF.prem_adj_ny_scnd_injr_fund_id == intPremAdjNYSIFID
                                                             select new PremiumAdjNYScndInjuryFundBE 
                                                             {
                                                                 PREM_ADJ_NY_SCND_INJR_FUND_ID = PremAdjNYSIF.prem_adj_ny_scnd_injr_fund_id,
                                                                 PREM_ADJ_PERD_ID = PremAdjNYSIF.prem_adj_perd_id,
                                                                 PREM_ADJ_ID = PremAdjNYSIF.prem_adj_id,
                                                                 CUSTMR_ID = PremAdjNYSIF.custmr_id,
//                                                                 PREM_ADJ_CMMNT_ID = PremAdjNYSIF.prem_adj_cmmnt_id,
                                                                 INCUR_LOS_AMT = PremAdjNYSIF.incur_los_amt,
                                                                 LOS_CONV_FCTR_RT = PremAdjNYSIF.los_conv_fctr_rt,
                                                                 CNVT_LOS_AMT = PremAdjNYSIF.cnvt_los_amt,
                                                                 BASIC_DEDTBL_PREM_AMT = PremAdjNYSIF.basic_dedtbl_prem_amt,
                                                                 TAX_MULTI_RT = PremAdjNYSIF.tax_multi_rt,
                                                                 CNVT_TOT_LOS_AMT = PremAdjNYSIF.cnvt_tot_los_amt,
                                                                 NY_PREM_DISC_AMT = PremAdjNYSIF.ny_prem_disc_amt,
                                                                 NY_SCND_INJR_FUND_RT = PremAdjNYSIF.ny_scnd_injr_fund_rt,
                                                                 REVD_NY_SCND_INJR_FUND_AMT = PremAdjNYSIF.revd_ny_scnd_injr_fund_amt,
                                                                 NY_TAX_DUE_AMT = PremAdjNYSIF.ny_tax_due_amt,
                                                                 PREV_RSLT_AMT = PremAdjNYSIF.prev_rslt_amt,
                                                                 NY_SCND_INJR_FUND_AUDT_AMT = PremAdjNYSIF.ny_scnd_injr_fund_audt_amt,
                                                                 CURR_ADJ_AMT = PremAdjNYSIF.curr_adj_amt,
                                                                 BASIC_CNVT_LOS_AMT = PremAdjNYSIF.cnvt_los_amt,
                                                                 NY_ERND_RETRO_PREM_AMT = PremAdjNYSIF.ny_ernd_retro_prem_amt,
                                                                 CREATE_DATE=PremAdjNYSIF.crte_dt,
                                                                 CREATE_USER_ID=PremAdjNYSIF.crte_user_id,
                                                                 UPDATE_DATE=PremAdjNYSIF.updt_dt
                                                             };


          if (query.Count() > 0)
            result = query.ToList()[0];
            return result;
        }

    }
}
