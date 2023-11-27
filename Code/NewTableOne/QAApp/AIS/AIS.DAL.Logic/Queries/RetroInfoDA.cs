using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;

namespace ZurichNA.AIS.DAL.Logic
{
    public class RetroInfoDA : DataAccessor<PREM_ADJ_PGM_RETRO, RetroInfoBE, AISDatabaseLINQDataContext>
    {
        public IList<RetroInfoBE> GetRetroInfo(int programPeriodId)
        {
            IList<RetroInfoBE> retro = new List<RetroInfoBE>();
            IQueryable<RetroInfoBE> query =
                (from ari in this.Context.PREM_ADJ_PGM_RETROs
                 join pap in this.Context.PREM_ADJ_PGMs
                 on ari.prem_adj_pgm_id equals pap.prem_adj_pgm_id
                 join lk in this.Context.LKUPs on ari.retro_elemt_typ_id equals lk.lkup_id
                 orderby lk.attr_1_txt
                 where pap.prem_adj_pgm_id == programPeriodId
                 select new RetroInfoBE
                 {
                     ADJ_RETRO_INFO_ID = ari.prem_adj_pgm_retro_id,
                     PREM_ADJ_PGM_ID = ari.prem_adj_pgm_id,
                     CUSTMR_ID = ari.custmr_id,
                     RETRO_ADJ_FCTR_APLCBL_IND = ari.retro_adj_fctr_aplcbl_ind,
                     NO_LIM_IND = ari.no_lim_ind,
                     EXPO_TYP_ID = ari.expo_typ_id,
                     EXPO_AGMT_AMT = ari.expo_agmt_amt,
                     RETRO_ADJ_FCTR_RT = ari.retro_adj_fctr_rt,
                     TOT_AGMT_AMT = ari.tot_agmt_amt,
                     AUDT_EXPO_AMT = ari.audt_expo_amt,
                     AGGR_FCTR_PCT = ari.aggr_fctr_pct,
                     TOT_AUDT_AMT = ari.tot_audt_amt,
                     RETRO_ELEMT_TYP_ID = ari.retro_elemt_typ_id,
                     EXPO_TYP_INCREMNT_NBR_ID = ari.expo_typ_incremnt_nbr_id,
                     UPDT_USER_ID = ari.updt_user_id,
                     UPDT_DT = ari.updt_dt,
                     CRTE_USER_ID = ari.crte_user_id,
                     CRTE_DT = ari.crte_dt,
                     RETRO_ELEMT = (from lkp in this.Context.LKUPs
                                    where lkp.lkup_id == ari.retro_elemt_typ_id
                                    select lkp.lkup_txt).First().ToString(),

                     ACTV_IND = ari.actv_ind,
                     EXP_BASIS = ari.LKUP1.lkup_txt,
                     SEQ_NO = Convert.ToInt32(lk.attr_1_txt),
                 });
            retro = query.ToList();
            //if (retro == null || retro.Count <= 0)
            //{
            IList<LookupBE> lookup = new List<LookupBE>();
            IQueryable<LookupBE> lkpQuery =
                (from lkp in this.Context.LKUPs
                 join lkptyp in this.Context.LKUP_TYPs
                 on lkp.lkup_typ_id equals lkptyp.lkup_typ_id
                 orderby lkp.attr_1_txt
                 where lkptyp.lkup_typ_nm_txt == "Retrospective Element Type" && lkp.actv_ind == true
                 select new LookupBE
                 {
                     LookUpName = lkp.lkup_txt,
                     LookUpID = lkp.lkup_id,
                     Attribute1 = lkp.attr_1_txt,

                 });
            int rsLoop = 0;
            IList<RetroInfoBE> retroList;
            RetroInfoBE retronew;
            foreach (LookupBE lkupitem in lkpQuery)
            {
                retroList = retro.Where(ret => lkupitem.LookUpID == ret.RETRO_ELEMT_TYP_ID).ToList();
                if (retroList.Count() <= 0)
                {
                    retronew = new RetroInfoBE();
                    retronew.RETRO_ELEMT_TYP_ID = lkupitem.LookUpID;
                    retronew.RETRO_ELEMT = lkupitem.LookUpName;
                    retronew.RETRO_ADJ_FCTR_APLCBL_IND = false;
                    retronew.NO_LIM_IND = false;
                    retronew.EXPO_TYP_ID = 0;
                    retronew.EXPO_TYP_INCREMNT_NBR_ID = 0;
                    retronew.SEQ_NO = Convert.ToInt32(lkupitem.Attribute1);
                    retro.Add(retronew);
                }
                rsLoop++;
            }
            retro = retro.OrderBy(ret => ret.SEQ_NO).ToList();
            //}
            return retro;
        }
        /// <summary>
        /// Method to get Maximum and minimum values for PremAdjCombElemnts Screen
        /// </summary>
        /// <param name="intProgramPeriodID"></param>
        /// <param name="intCustmorID"></param>
        /// <param name="intRetroElemtTypID"></param>
        /// <returns>IList<RetroInfoBE></returns>
        public IList<RetroInfoBE> getCombElemntsMaxAndMinAmounts(int intProgramPeriodID, int intCustmorID, int intRetroElemtTypID)
        {

            List<RetroInfoBE> RetroInfo = new List<RetroInfoBE>();
            IQueryable<RetroInfoBE> query =
              (from papr in this.Context.PREM_ADJ_PGM_RETROs
               where papr.prem_adj_pgm_id == intProgramPeriodID && papr.custmr_id == intCustmorID && papr.retro_elemt_typ_id == intRetroElemtTypID
               select new RetroInfoBE
               {
                   ADJ_RETRO_INFO_ID = papr.prem_adj_pgm_retro_id,
                   TOT_AGMT_AMT = papr.tot_agmt_amt,
                   TOT_AUDT_AMT = papr.tot_audt_amt
               });
            RetroInfo = query.ToList();
            return RetroInfo;

        }

        /// <summary>
        /// Retriving RetroInfo records. Used for ProgramPeriod Copy Dependies
        /// This will not retrive records Using Lookup if there are no records in RetroInfo table. unlike "GetRetroInfo" method
        /// Sreedhar 4th Nov 2008
        /// </summary>
        /// <param name="programPeriodID"></param>
        /// <returns></returns>
        public IList<RetroInfoBE> getRetroInfoForCopy(int programPeriodId)
        {
            IList<RetroInfoBE> retro = new List<RetroInfoBE>();
            IQueryable<RetroInfoBE> query =
                (from ari in this.Context.PREM_ADJ_PGM_RETROs
                 join pap in this.Context.PREM_ADJ_PGMs
                 on ari.prem_adj_pgm_id equals pap.prem_adj_pgm_id
                 where pap.prem_adj_pgm_id == programPeriodId
                 select new RetroInfoBE
                 {
                     ADJ_RETRO_INFO_ID = ari.prem_adj_pgm_retro_id,
                     PREM_ADJ_PGM_ID = ari.prem_adj_pgm_id,
                     CUSTMR_ID = ari.custmr_id,
                     RETRO_ADJ_FCTR_APLCBL_IND = ari.retro_adj_fctr_aplcbl_ind,
                     NO_LIM_IND = ari.no_lim_ind,
                     EXPO_TYP_ID = ari.expo_typ_id,
                     EXPO_AGMT_AMT = ari.expo_agmt_amt,
                     RETRO_ADJ_FCTR_RT = ari.retro_adj_fctr_rt,
                     TOT_AGMT_AMT = ari.tot_agmt_amt,
                     AUDT_EXPO_AMT = ari.audt_expo_amt,
                     AGGR_FCTR_PCT = ari.aggr_fctr_pct,
                     TOT_AUDT_AMT = ari.tot_audt_amt,
                     RETRO_ELEMT_TYP_ID = ari.retro_elemt_typ_id,
                     EXPO_TYP_INCREMNT_NBR_ID = ari.expo_typ_incremnt_nbr_id,
                     UPDT_USER_ID = ari.updt_user_id,
                     UPDT_DT = ari.updt_dt,
                     CRTE_USER_ID = ari.crte_user_id,
                     CRTE_DT = ari.crte_dt,
                     ACTV_IND = ari.actv_ind
                 });
            retro = query.ToList();
            return retro;
        }

    }
    public class AuditExpDA : DataAccessor<COML_AGMT_AUDT, RetroInfoBE, AISDatabaseLINQDataContext>
    {
        public IList<RetroInfoBE> GetStandardAuditExp(int programPeriodID, int custmorId)
        {
            List<RetroInfoBE> audExpo = new List<RetroInfoBE>();
            IQueryable<RetroInfoBE> query =
                (from caa in this.Context.COML_AGMT_AUDTs
                 join agrmt in this.Context.COML_AGMTs on caa.coml_agmt_id equals agrmt.coml_agmt_id
                 where caa.prem_adj_pgm_id == programPeriodID && caa.custmr_id == custmorId
                 && (caa.audt_revd_sts_ind != true || caa.audt_revd_sts_ind == null)
                 && agrmt.actv_ind == true
                 group caa by caa.prem_adj_pgm_id into result
                 select new RetroInfoBE
                 {
                     PREM_ADJ_PGM_ID = result.Key,
                     TOT_SUBJ_AUDT_PREM_AMT = result.Sum(i => i.subj_audt_prem_amt)
                 });
            audExpo = query.ToList();
            return audExpo;

        }
        public IList<RetroInfoBE> GetPayrollAuditExp(int programPeriodID, int custmorId)
        {

            List<RetroInfoBE> audExpo = new List<RetroInfoBE>();
            string[] adjTypes = new string[] 
            { "Incurred Loss Retro", "Paid Loss Retro", "Incurred DEP ", 
                "Paid Loss DEP ", "Incurred Underlayer", "Paid Loss Underlayer", 
                "Incurred Loss WA", "Paid Loss WA", };

            IQueryable<RetroInfoBE> query =
                (from com in this.Context.COML_AGMT_AUDTs
                 join agrmt in this.Context.COML_AGMTs on com.coml_agmt_id equals agrmt.coml_agmt_id
                 join lkp in this.Context.LKUPs
                 on agrmt.adj_typ_id equals lkp.lkup_id
                 join lkptyp in this.Context.LKUP_TYPs
                 on lkp.lkup_typ_id equals lkptyp.lkup_typ_id
                 where lkptyp.lkup_typ_nm_txt == "ADJUSTMENT TYPE" && adjTypes.Contains(lkp.lkup_txt)
                 && com.prem_adj_pgm_id == programPeriodID && com.custmr_id == custmorId
                 && (com.audt_revd_sts_ind != true || com.audt_revd_sts_ind == null)
                 && agrmt.actv_ind == true
                 group com by com.prem_adj_pgm_id into result
                 select new RetroInfoBE
                 {
                     PREM_ADJ_PGM_ID = result.Key,
                     TOT_SUBJ_AUDT_PREM_AMT = result.Sum(i => i.expo_amt)
                 });
            audExpo = query.ToList();
            return audExpo;

        }
        public IList<RetroInfoBE> GetCombinedAuditExp(int programPeriodID, int custmorId)
        {

            List<RetroInfoBE> audExpo = new List<RetroInfoBE>();
            string[] adjTypes = new string[] 
            { "Incurred Loss Retro", "Paid Loss Retro", "Incurred DEP ", 
                "Paid Loss DEP ", "Incurred Underlayer", "Paid Loss Underlayer", 
                "Incurred Loss WA", "Paid Loss WA","Incurred Loss Deductible","Paid Loss Deductible","ESCROW","LRF","LBA Only"};

            IQueryable<RetroInfoBE> query =
                (from com in this.Context.COML_AGMT_AUDTs
                 join agrmt in this.Context.COML_AGMTs on com.coml_agmt_id equals agrmt.coml_agmt_id
                 join lkp in this.Context.LKUPs
                 on agrmt.adj_typ_id equals lkp.lkup_id
                 join lkptyp in this.Context.LKUP_TYPs
                 on lkp.lkup_typ_id equals lkptyp.lkup_typ_id
                 where lkptyp.lkup_typ_nm_txt == "ADJUSTMENT TYPE" && adjTypes.Contains(lkp.lkup_txt)
                 && com.prem_adj_pgm_id == programPeriodID && com.custmr_id == custmorId
                 && (com.audt_revd_sts_ind != true || com.audt_revd_sts_ind == null)
                 && agrmt.actv_ind == true
                 group com by com.prem_adj_pgm_id into result
                 select new RetroInfoBE
                 {
                     PREM_ADJ_PGM_ID = result.Key,
                     TOT_SUBJ_AUDT_PREM_AMT = result.Sum(i => i.expo_amt)
                 });
            audExpo = query.ToList();
            return audExpo;

        }
        public IList<RetroInfoBE> GetOtherAuditExp(int programPeriodID, int custmorId)
        {

            List<RetroInfoBE> audExpo = new List<RetroInfoBE>();
            IQueryable<RetroInfoBE> query =
              (from com in this.Context.COML_AGMT_AUDTs
               join agrmt in this.Context.COML_AGMTs on com.coml_agmt_id equals agrmt.coml_agmt_id
               where com.prem_adj_pgm_id == programPeriodID && com.custmr_id == custmorId
               && (com.audt_revd_sts_ind != true || com.audt_revd_sts_ind == null)
               && agrmt.actv_ind == true
               group com by com.prem_adj_pgm_id into result
               select new RetroInfoBE
               {
                   PREM_ADJ_PGM_ID = result.Key,
                   TOT_SUBJ_AUDT_PREM_AMT = result.Sum(i => i.expo_amt)
               });
            audExpo = query.ToList();
            return audExpo;

        }

    }
}
