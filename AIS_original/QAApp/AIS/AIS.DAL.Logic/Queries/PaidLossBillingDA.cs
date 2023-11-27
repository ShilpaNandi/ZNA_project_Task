using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    /// <summary>
    /// DataAccessor for Paid Loss Billing
    /// </summary>
    public class PaidLossBillingDA : DataAccessor<PREM_ADJ_PAID_LOS_BIL, PaidLossBillingBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Returns all Paid Loss Billing that match criteria
        /// </summary>
        /// <returns></returns>
        public IList<PaidLossBillingBE> getPaidLossBillingData(int custmrID, int prgID, int PrmAdjID, int PrmAdjPrgID)
        {
            IList<PaidLossBillingBE> result = new List<PaidLossBillingBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve PaidLossBilling information
            /// and project it into PaidLossBilling Business Entity
            IQueryable<PaidLossBillingBE> query =
            (from prmadjplb in this.Context.PREM_ADJ_PAID_LOS_BILs
             join comagmt in this.Context.COML_AGMTs
             on prmadjplb.coml_agmt_id equals comagmt.coml_agmt_id
             join lk in this.Context.LKUPs
               on prmadjplb.ln_of_bsn_id equals lk.lkup_id
             where prmadjplb.prem_adj_perd_id == prgID
             && prmadjplb.custmr_id == custmrID
             && prmadjplb.prem_adj_id == PrmAdjID
             orderby prmadjplb.lsi_valn_dt descending
             select new PaidLossBillingBE()
             {
                 PREM_ADJ_PAID_LOS_BIL_ID = prmadjplb.prem_adj_paid_los_bil_id,
                 PREM_ADJ_PERD_ID = prmadjplb.prem_adj_perd_id,
                 PREM_ADJ_PGM_ID = prmadjplb.prem_adj_pgm_id,
                 CUSTMR_ID = prmadjplb.custmr_id,
                 LSI_VALN_DATE = prmadjplb.lsi_valn_dt,
                 LSI_PGM_TYP = prmadjplb.lsi_pgm_typ_txt,
                 LSI_SRC = prmadjplb.lsi_src,
                 IDNMTY_AMT = prmadjplb.idnmty_amt,
                 ADJ_IDNMTY_AMT = prmadjplb.adj_idnmty_amt,
                 EXPS_AMT = prmadjplb.exps_amt,
                 ADJ_EXPS_AMT = prmadjplb.adj_exps_amt,
                 UPDATEDDATE=prmadjplb.updt_dt,
                 TOT_PAID_LOS_BIL_AMT = prmadjplb.tot_paid_los_bil_amt,
                 ADJ_TOT_PAID_LOS_BIL_AMT = prmadjplb.adj_tot_paid_los_bil_amt,
                 CMMNT_TXT = prmadjplb.cmmnt_txt,
                 POLICYSYMBOL = (comagmt.pol_sym_txt.Trim() + comagmt.pol_nbr_txt.Trim() + comagmt.pol_modulus_txt.Trim()).ToString(),
                 LOB = lk.lkup_txt.ToString()

             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;

        }
    }
}
