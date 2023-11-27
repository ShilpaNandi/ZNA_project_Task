using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;
using System.IO;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

using ZurichNA.AIS.DAL.Logic;
using System.Data;

namespace ZurichNA.AIS.DAL.Logic
{
    public class PremiumAdjLRFPostingTaxDA : DataAccessor<PREM_ADJ_LOS_REIM_FUND_POST_TAX, PremiumAdjLRFPostingTaxBE, AISDatabaseLINQDataContext>
    {
        public IList<PremiumAdjLRFPostingTaxBE> GetPrmAdjLRFPostingTax(int prmAdjmtId,int prem_adj_perd_id,int prem_adj_pgm_id)
        {
            IList<PremiumAdjLRFPostingTaxBE> result = new List<PremiumAdjLRFPostingTaxBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<PremiumAdjLRFPostingTaxBE> query =
            (from prmTax in this.Context.PREM_ADJ_LOS_REIM_FUND_POST_TAXes
             join prmAdjPer in this.Context.PREM_ADJ_PERDs
             on prmTax.prem_adj_perd_id equals prmAdjPer.prem_adj_perd_id
             join taxType in this.Context.LKUPs
             on prmTax.tax_typ_id equals taxType.lkup_id
             join lkupTaxCovgTyp in this.Context.LKUPs
                on prmTax.ln_of_bsn_id equals lkupTaxCovgTyp.lkup_id
             join cmpnt in this.Context.LKUPs 
             on prmTax.dedtbl_tax_cmpnt_id equals cmpnt.lkup_id
             where prmTax.prem_adj_id == prmAdjmtId
             && prmTax.prem_adj_perd_id == prem_adj_perd_id
             && prmAdjPer.prem_adj_pgm_id == prem_adj_pgm_id
             && lkupTaxCovgTyp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "LOB" && cond.actv_ind == true).First().lkup_typ_id
             && cmpnt.lkup_typ_id == 54
             && prmTax.tax_typ_id != null
             select new PremiumAdjLRFPostingTaxBE()
             {
                 PREM_ADJ_LOS_REIM_FUND_POST_TAX_ID = prmTax.prem_adj_los_reim_fund_post_tax_id,
                 PREM_ADJ_PERD_ID = prmTax.prem_adj_perd_id,
                 PREM_ADJ_ID = prmTax.prem_adj_id,
                 CUSTMR_ID = prmTax.custmr_id,
                 TAX_TYP_ID = prmTax.tax_typ_id,
                 TAXTYPE = taxType.lkup_txt + "/" + lkupTaxCovgTyp.lkup_txt + "/" + cmpnt.lkup_txt,
                 CURR_AMT = prmTax.curr_amt,
                 AGGR_AMT = prmTax.aggr_amt,
                 LIM_AMT = prmTax.lim_amt,
                 PRIOR_YY_AMT = prmTax.prior_yy_amt,
                 ADJ_PRIOR_YY_AMT = prmTax.adj_prior_yy_amt,
                 POST_AMT = prmTax.post_amt,
                 UPDT_USER_ID = prmTax.updt_user_id,
                 UPDT_DT = prmTax.updt_dt,
                 CRTE_DT = prmTax.crte_dt,
                 CRTE_USER_ID = prmTax.crte_user_id,
                 LN_OF_BSN_ID = lkupTaxCovgTyp.lkup_id,
                 LN_OF_BSN_TXT = lkupTaxCovgTyp.lkup_txt

             }).Union(from prmTax in this.Context.PREM_ADJ_LOS_REIM_FUND_POST_TAXes
             join prmAdjPer in this.Context.PREM_ADJ_PERDs
             on prmTax.prem_adj_perd_id equals prmAdjPer.prem_adj_perd_id
             join lkupTaxCovgTyp in this.Context.LKUPs
                on prmTax.ln_of_bsn_id equals lkupTaxCovgTyp.lkup_id
             where prmTax.prem_adj_id == prmAdjmtId
             && prmTax.prem_adj_perd_id == prem_adj_perd_id
             && prmAdjPer.prem_adj_pgm_id == prem_adj_pgm_id
             && lkupTaxCovgTyp.lkup_typ_id == this.Context.LKUP_TYPs.Where(cond => cond.lkup_typ_nm_txt == "LOB" && cond.actv_ind == true).First().lkup_typ_id
             && prmTax.tax_typ_id==null
             select new PremiumAdjLRFPostingTaxBE()
             {
                 PREM_ADJ_LOS_REIM_FUND_POST_TAX_ID = prmTax.prem_adj_los_reim_fund_post_tax_id,
                 PREM_ADJ_PERD_ID = prmTax.prem_adj_perd_id,
                 PREM_ADJ_ID = prmTax.prem_adj_id,
                 CUSTMR_ID = prmTax.custmr_id,
                 TAX_TYP_ID = prmTax.tax_typ_id,
                 TAXTYPE = this.Context.LKUPs.Where(cond=>cond.LKUP_TYP.lkup_typ_nm_txt=="TAX TYPE" && cond.attr_1_txt==(this.Context.LKUPs.Where(cdd=>cdd.lkup_id==prmTax.st_id)).FirstOrDefault().attr_1_txt).FirstOrDefault().lkup_txt+"/"+lkupTaxCovgTyp.lkup_txt+"/"+"ALAE",
                 CURR_AMT = prmTax.curr_amt,
                 AGGR_AMT = prmTax.aggr_amt,
                 LIM_AMT = prmTax.lim_amt,
                 PRIOR_YY_AMT = prmTax.prior_yy_amt,
                 ADJ_PRIOR_YY_AMT = prmTax.adj_prior_yy_amt,
                 POST_AMT = prmTax.post_amt,
                 UPDT_USER_ID = prmTax.updt_user_id,
                 UPDT_DT = prmTax.updt_dt,
                 CRTE_DT = prmTax.crte_dt,
                 CRTE_USER_ID = prmTax.crte_user_id,
                 LN_OF_BSN_ID = lkupTaxCovgTyp.lkup_id,
                 LN_OF_BSN_TXT = lkupTaxCovgTyp.lkup_txt

             });
            if (query.Count() > 0)
                result = query.ToList().OrderBy(cdd=>cdd.TAXTYPE).ToList();
            return result;
        }

        public string AddPREM_ADJ_PERD_TOT_TAX(int intCustomerID, int intPrem_adj_perd_id, int intPrem_adj_pgm_id, int intPrem_adj_id, int intUserID)
        {
            string ErroMsg;
            ErroMsg = string.Empty;
            try
            {
                if (this.Context == null)
                    this.Initialize();
                this.Context.AddPREM_ADJ_PERD_TOT_TAX(intPrem_adj_perd_id, intPrem_adj_id, intCustomerID, intPrem_adj_pgm_id, intUserID);
            }
            catch(Exception ex)
            {
                ErroMsg = ex.ToString();
            }
            return ErroMsg;
        }
    }
}
