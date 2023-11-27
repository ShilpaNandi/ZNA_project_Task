using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;


namespace ZurichNA.AIS.DAL.Logic
{
    public class ProgramPeriodDA : DataAccessor<PREM_ADJ_PGM, ProgramPeriodBE, AISDatabaseLINQDataContext>
    {
        public IList<ProgramPeriodBE> GetProgramPeriodData(int accountID)
        {
            IList<ProgramPeriodBE> result = new List<ProgramPeriodBE>();

            IQueryable<ProgramPeriodBE> query =
                this.BuildQuery().Distinct();

            if (accountID > 0)
            {
                query = query.Where(progperd => progperd.CUSTMR_ID == accountID);
            }

            result = query.ToList();
            return result;
        }

        /// To retrieve Program Period Data for the given search criteria for the 
        /// Account Management Dashboard. This query will return 
        /// all accounts where the selected user has the CFS2 or "Account Setup QC" responsibilities.
        /// <param name="statusID"></param>
        /// <param name="personID"></param>
        /// <returns></returns>
        public IList<ProgramPeriodBE> GetProgramPeriodData(int statusID, int personID)
        {
            bool qualifier = false;
            IList<ProgramPeriodBE> result = new List<ProgramPeriodBE>();
            IQueryable<ProgramPeriodBE> pgmPeriod =
                   this.BuildQueryforUser().Distinct();

            if (statusID > 0)
            {
                pgmPeriod = pgmPeriod.Where(progperd => progperd.PROGRAMSTATUSID == statusID.ToString());
            }
            if (personID > 0)
            {
                qualifier = true;
                pgmPeriod = pgmPeriod.Where(progperd => progperd.CUSTMR_PERS_REL_pers_id == personID);

            }

            pgmPeriod = pgmPeriod.OrderByDescending(o => o.STRT_DT).OrderByDescending(o => o.FST_ADJ_DT).OrderBy(o => o.CUSTMR_NAME);

            if (qualifier)
            {
                result = pgmPeriod.ToList();
            }
            return result;
        }

        /// <summary>
        /// To retrieve Program Period Data for the given search criteria for the 
        /// Account Management Dashboard. This query will return 
        /// all accounts where the selected user has any type of responsibility
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="statusID"></param>
        /// <param name="personID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IList<ProgramPeriodBE> GetProgramPeriodData(int accountID, int statusID, int personID, string startDate, string endDate)
        {
            bool qualifier = false;
            IList<ProgramPeriodBE> result = new List<ProgramPeriodBE>();
            IQueryable<ProgramPeriodBE> pgmPeriod =
                   this.BuildQuery().Distinct();
            pgmPeriod = pgmPeriod.Where(progperd => progperd.CUSTMR_ACTIVE == true);
            if (accountID > 0)
            {
                qualifier = true;
                pgmPeriod = pgmPeriod.Where(progperd => progperd.CUSTMR_ID == accountID);
            }
            if (statusID > 0)
            {
                pgmPeriod = pgmPeriod.Where(progperd => progperd.PROGRAMSTATUSID == statusID.ToString());
            }
            if (personID > 0)
            {
                qualifier = true;
                pgmPeriod = pgmPeriod.Where(progperd => progperd.CUSTMR_PERS_REL_pers_id == personID);

            }

            if (startDate != string.Empty && endDate != string.Empty)
            {
                qualifier = true;
                pgmPeriod = pgmPeriod.Where(progperd => (progperd.FST_ADJ_DT >= Convert.ToDateTime(startDate) && progperd.FST_ADJ_DT <= Convert.ToDateTime(endDate)) || (progperd.NXT_VALN_DT >= Convert.ToDateTime(startDate) && progperd.NXT_VALN_DT <= Convert.ToDateTime(endDate)));
            }
            pgmPeriod = pgmPeriod.OrderByDescending(o => o.STRT_DT).OrderByDescending(o => o.FST_ADJ_DT).OrderBy(o => o.CUSTMR_NAME);

            if (qualifier)
            {
                result = pgmPeriod.ToList();
            }
            return result;
        }
        public bool IsProgramPeriodExits(int progPerID, int brokerID, int buOffice, DateTime startDate, DateTime endDate, int custID, DateTime valDate, int progType)
        {
            bool Results;

            int Count = this.Context.PREM_ADJ_PGMs.Count(o => o.prem_adj_pgm_id != progPerID && o.brkr_id == brokerID && o.bsn_unt_ofc_id == buOffice && o.strt_dt == startDate && o.plan_end_dt == endDate && o.custmr_id == custID && o.valn_mm_dt == valDate && o.pgm_typ_id == progType);

            if (Count > 0)
                return true;
            else
                return false;

        }
        /// <summary>
        /// Added by venkat kolimi on 10/08/2009 to have message record disabled message
        /// </summary>
        /// <param name="progPerID"></param>
        /// <param name="brokerID"></param>
        /// <param name="buOffice"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="custID"></param>
        /// <param name="valDate"></param>
        /// <param name="progType"></param>
        /// <returns></returns>
        public bool IsProgramPeriodExitsAndDisabled(int progPerID, int brokerID, int buOffice, DateTime startDate, DateTime endDate, int custID, DateTime valDate, int progType)
        {
           
            int Count = this.Context.PREM_ADJ_PGMs.Count(o => o.prem_adj_pgm_id != progPerID && o.brkr_id == brokerID && o.bsn_unt_ofc_id == buOffice && o.strt_dt == startDate && o.plan_end_dt == endDate && o.custmr_id == custID && o.valn_mm_dt == valDate && o.pgm_typ_id == progType && o.actv_ind==false);

            if (Count > 0)
                return true;
            else
                return false;

        }

        public IList<ProgramPeriodListBE> GetProgramPeriodList(int accountID)
        {
            IList<ProgramPeriodListBE> result = new List<ProgramPeriodListBE>();

            IQueryable<ProgramPeriodListBE> query =
                from pgmprd in this.Context.PREM_ADJ_PGMs
                //orderby pgmprd.strt_dt descending, pgmprd.plan_end_dt descending
                where pgmprd.custmr_id == accountID && pgmprd.actv_ind == true
                select new ProgramPeriodListBE
                {
                    PREM_ADJ_PGM_ID = pgmprd.prem_adj_pgm_id,
                    STRT_DT = pgmprd.strt_dt,
                    PLAN_END_DT = pgmprd.plan_end_dt,
                    VALN_MM_DT = pgmprd.valn_mm_dt,
                    CUSTMR_ID = pgmprd.custmr_id,
                    BRKR_ID = pgmprd.brkr_id,
                    PGM_TYP_ID = pgmprd.pgm_typ_id,
                    BSN_UNT_OFC_ID = pgmprd.bsn_unt_ofc_id,
                    ACTV_IND = pgmprd.actv_ind
                };

            if (query.Count() > 0)
            {
                result = query.ToList();
                result = (result.OrderByDescending(o => o.STRT_DT).ThenByDescending(o => o.PLAN_END_DT).ThenBy(o => o.PROGRAMTYPENAME)).ToList();
            }

            return result;
        }
        public IList<ProgramPeriodListBE> GetActiveInActiveProgramPeriods(int accountID)
        {
            IList<ProgramPeriodListBE> result = new List<ProgramPeriodListBE>();

            IQueryable<ProgramPeriodListBE> query =
                from pgmprd in this.Context.PREM_ADJ_PGMs
                //orderby pgmprd.strt_dt descending, pgmprd.plan_end_dt descending
                where pgmprd.custmr_id == accountID
                select new ProgramPeriodListBE
                {
                    PREM_ADJ_PGM_ID = pgmprd.prem_adj_pgm_id,
                    STRT_DT = pgmprd.strt_dt,
                    PLAN_END_DT = pgmprd.plan_end_dt,
                    VALN_MM_DT = pgmprd.valn_mm_dt,
                    CUSTMR_ID = pgmprd.custmr_id,
                    BRKR_ID = pgmprd.brkr_id,
                    PGM_TYP_ID = pgmprd.pgm_typ_id,
                    BSN_UNT_OFC_ID = pgmprd.bsn_unt_ofc_id,
                    ACTV_IND = pgmprd.actv_ind
                };

            if (query.Count() > 0)
            {
                result = query.ToList();
                result = (result.OrderByDescending(o => o.STRT_DT).ThenByDescending(o => o.PLAN_END_DT).ThenBy(o => o.PROGRAMTYPENAME)).ToList();
            }

            return result;
        }

        public IList<PersonBE> getUserNames()
        {
            IList<PersonBE> result = new List<PersonBE>();
            IQueryable<PersonBE> query =
                (from per in this.Context.PERs
                 join lkp in this.Context.LKUPs
                 on per.conctc_typ_id equals lkp.lkup_id
                 join lktyp in this.Context.LKUP_TYPs
                 on lkp.lkup_typ_id equals lktyp.lkup_typ_id
                 where lktyp.lkup_typ_nm_txt == "CONTACT TYPE" && lkp.lkup_txt == "C & RM" && per.pers_id != 1000000
                 && per.actv_ind == true
                 orderby per.surname, per.forename
                 select new PersonBE
                 {
                     PERSON_ID = per.pers_id,
                     FORENAME = per.forename,
                     SURNAME = per.surname,
                     FULLNAME = per.surname + "," + per.forename,
                     CONCTACT_TYPE_ID = per.conctc_typ_id
                 });
            result = query.ToList();

            return result;
        }
        /// <summary>
        /// Retrieves program period data which used in queries to populate grid 
        /// in Upcoming Valuations tab.  It is designed to be used when user clicks on 
        /// on the search button or navigating to it from one of the other two tabs
        /// </summary>
        /// <returns>IQueryable<ProgramPeriodBE></returns>
        private IQueryable<ProgramPeriodBE> BuildQuery()
        {
            if (this.Context == null)
                this.Initialize();

            IQueryable<ProgramPeriodBE> pgmPeriod
                = from pgmprd in this.Context.PREM_ADJ_PGMs
                  join cust_per in Context.CUSTMR_PERS_RELs on pgmprd.custmr_id equals cust_per.custmr_id
                  where pgmprd.actv_ind == true
                  && ((pgmprd.nxt_valn_dt <=(pgmprd.fnl_adj_dt==null?pgmprd.nxt_valn_dt.Value.AddMonths(1):pgmprd.fnl_adj_dt))
                  ||(pgmprd.nxt_valn_dt_non_prem_dt<=(pgmprd.fnl_adj_non_prem_dt==null?pgmprd.nxt_valn_dt_non_prem_dt.Value.AddMonths(1):pgmprd.fnl_adj_non_prem_dt)))
                  select new ProgramPeriodBE
                  {

                      PREM_ADJ_PGM_ID = pgmprd.prem_adj_pgm_id,
                      BRKR_CONCTC_ID = pgmprd.brkr_conctc_id,
                      STRT_DT = pgmprd.strt_dt,
                      PLAN_END_DT = pgmprd.plan_end_dt,
                      VALN_MM_DT = pgmprd.valn_mm_dt,
                      INCUR_CONV_MMS_CNT = pgmprd.incur_conv_mms_cnt,
                      ADJ_FREQ_MMS_INTVRL_CNT = pgmprd.adj_freq_mms_intvrl_cnt,
                      INCLD_CAPTV_PAYKND_IND = pgmprd.incld_captv_payknd_ind,
                      AVG_TAX_MULTI_IND = pgmprd.avg_tax_multi_ind,
                      TAX_MULTI_FCTR_RT = pgmprd.tax_multi_fctr_rt,
                      CUSTMR_ID = pgmprd.custmr_id,
                      CUSTMR_ACTIVE = pgmprd.CUSTMR.actv_ind,
                      LOS_SENS_INFO_STRT_DT = pgmprd.los_sens_info_strt_dt,
                      LOS_SENS_INFO_END_DT = pgmprd.los_sens_info_end_dt,
                      FST_ADJ_MMS_FROM_INCP_CNT = pgmprd.fst_adj_mms_from_incp_cnt,
                      FNL_ADJ_DT = pgmprd.fnl_adj_dt,
                      BRKR_ID = pgmprd.brkr_id,
                      NXT_VALN_DT = pgmprd.nxt_valn_dt,
                      PREV_VALN_DT = pgmprd.prev_valn_dt,
                      COMB_ELEMTS_MAX_AMT = pgmprd.comb_elemts_max_amt,
                      PEO_PAY_IN_AMT = pgmprd.peo_pay_in_amt,
                      FST_ADJ_NON_PREM_MMS_CNT = pgmprd.fst_adj_non_prem_mms_cnt,
                      FREQ_NON_PREM_MMS_CNT = pgmprd.freq_non_prem_mms_cnt,
                      FNL_ADJ_NON_PREM_DT = pgmprd.fnl_adj_non_prem_dt,
                      NXT_VALN_DT_NON_PREM_DT = pgmprd.nxt_valn_dt_non_prem_dt,
                      PREV_VALN_DT_NON_PREM_DT = pgmprd.prev_valn_dt_non_prem_dt,
                      ZNA_SERV_COMP_CLM_HNDL_FEE_IND = pgmprd.zna_serv_comp_clm_hndl_fee_ind,
                      ACTV_IND = pgmprd.actv_ind,
                      PGM_TYP_ID = pgmprd.pgm_typ_id,
                      BNKRPT_BUYOUT_ID = pgmprd.bnkrpt_buyout_id,
                      BNKRPT_BUYOUT_EFF_DT = pgmprd.bnkrpt_buyout_eff_dt,
                      PAID_INCUR_TYP_ID = pgmprd.paid_incur_typ_id,
                      AGMT_ALOC_LOS_ADJ_EXPS_IND = pgmprd.agmt_aloc_los_adj_exps_ind,
                      AGMT_UNALOCTD_LOS_ADJ_IND = pgmprd.agmt_unaloctd_los_adj_ind,
                      AGMT_LOS_BASE_ASSES_IND = pgmprd.agmt_los_base_asses_ind,
                      LSI_ALOC_LOS_ADJ_EXPS_IND = pgmprd.lsi_aloc_los_adj_exps_ind,
                      LSI_UNALOCTD_LOS_ADJ_IND = pgmprd.lsi_unaloctd_los_adj_ind,
                      LSI_LOS_BASE_ASSES_IND = pgmprd.lsi_los_base_asses_ind,
                      AGMT_PAID_INCUR_IND = pgmprd.agmt_paid_incur_ind,
                      QLTY_CNTRL_DT = pgmprd.qlty_cntrl_dt,
                      QLTY_CMMNT_TXT = pgmprd.qlty_cmmnt_txt,
                      MSTR_ERND_RETRO_PREM_FRMLA_ID = pgmprd.mstr_ernd_retro_prem_frmla_id,
                      INCLD_LEGEND_IND = pgmprd.incld_legend_ind,
                      BSN_UNT_OFC_ID = pgmprd.bsn_unt_ofc_id,
                      BROKERNAME = pgmprd.INT_ORG.full_name + "/" + pgmprd.INT_ORG.city_nm,
                      CUSTMR_NAME = (from ct in this.Context.CUSTMRs
                                     where ct.custmr_id == pgmprd.custmr_id
                                     select ct.full_nm).First().ToString(),
                      FST_ADJ_DT = pgmprd.fst_adj_dt,
                      PROGRAMSTATUSID = (from pads in this.Context.PREM_ADJ_PGM_STs
                                         orderby pads.prem_adj_pgm_sts_id descending
                                         where pads.prem_adj_pgm_id == pgmprd.prem_adj_pgm_id
                                         select pads.pgm_perd_sts_typ_id).First().ToString(),
                      PROGRAMSTATUS = (from pads in this.Context.PREM_ADJ_PGM_STs
                                       join lkST in this.Context.LKUPs on pads.pgm_perd_sts_typ_id equals lkST.lkup_id
                                       orderby pads.prem_adj_pgm_sts_id descending
                                       where pads.prem_adj_pgm_id == pgmprd.prem_adj_pgm_id
                                       select lkST.lkup_txt).First().ToString(),
                      PREMIUMADJNUM = Convert.ToInt32((from pap in this.Context.PREM_ADJ_PERDs
                                                       join pno in this.Context.PREM_ADJs on pap.prem_adj_id equals pno.prem_adj_id
                                                       orderby pap.prem_adj_perd_id descending
                                                       where pap.prem_adj_pgm_id == pgmprd.prem_adj_pgm_id
                                                       select pap.prem_adj_id).First()),
                      PREMIUMADJNUMLOSS = Convert.ToInt32((from pap in this.Context.PREM_ADJ_PERDs
                                                           join pno in this.Context.PREM_ADJs on pap.prem_adj_id equals pno.prem_adj_id
                                                           orderby pap.prem_adj_perd_id descending
                                                           where pap.prem_adj_pgm_id == pgmprd.prem_adj_pgm_id
                                                           && (pno.adj_sts_typ_id == 346
                                                           || pno.adj_sts_typ_id == 348
                                                           || pno.adj_sts_typ_id == 350
                                                           || pno.adj_sts_typ_id == 351
                                                           || pno.adj_sts_typ_id == 353)
                                                           select pap.prem_adj_id).First()),
                      CUSTMR_PERS_REL_pers_id = cust_per.pers_id,
                      
                  };
            return pgmPeriod;
        }

        /// <summary>
        /// Retrieves program period data which used in queries to populate grid 
        /// in Upcoming Valuations tab.  It is designed to only retrieve
        /// program periods for users who have the CFS2
        /// or Account Setup QC responsibility
        /// </summary>
        /// <returns>IQueryable<ProgramPeriodBE></returns>
        private IQueryable<ProgramPeriodBE> BuildQueryforUser()
        {
            if (this.Context == null)
                this.Initialize();

            int intACCTSETUPQC = (int)(from lkup in this.Context.LKUPs
                                       where lkup.lkup_typ_id == 36 &&
                                       lkup.lkup_txt == "ACCOUNT SETUP QC"
                                       select lkup.lkup_id).Single();
            int intCFS2ID = (int)(from lkup in this.Context.LKUPs
                                  where lkup.lkup_typ_id == 36 &&
                                  lkup.lkup_txt == "CFS2"
                                  select lkup.lkup_id).Single();

            IQueryable<ProgramPeriodBE> pgmPeriod
                = from pgmprd in this.Context.PREM_ADJ_PGMs
                  join cust_per in Context.CUSTMR_PERS_RELs on pgmprd.custmr_id equals cust_per.custmr_id
                  where pgmprd.actv_ind == true && (cust_per.rol_id == intACCTSETUPQC || cust_per.rol_id == intCFS2ID)
                  && ((pgmprd.nxt_valn_dt <= (pgmprd.fnl_adj_dt == null ? pgmprd.nxt_valn_dt.Value.AddMonths(1) : pgmprd.fnl_adj_dt))
                  || (pgmprd.nxt_valn_dt_non_prem_dt <= (pgmprd.fnl_adj_non_prem_dt == null ? pgmprd.nxt_valn_dt_non_prem_dt.Value.AddMonths(1) : pgmprd.fnl_adj_non_prem_dt)))
                  select new ProgramPeriodBE
                  {

                      PREM_ADJ_PGM_ID = pgmprd.prem_adj_pgm_id,
                      BRKR_CONCTC_ID = pgmprd.brkr_conctc_id,
                      STRT_DT = pgmprd.strt_dt,
                      PLAN_END_DT = pgmprd.plan_end_dt,
                      VALN_MM_DT = pgmprd.valn_mm_dt,
                      INCUR_CONV_MMS_CNT = pgmprd.incur_conv_mms_cnt,
                      ADJ_FREQ_MMS_INTVRL_CNT = pgmprd.adj_freq_mms_intvrl_cnt,
                      INCLD_CAPTV_PAYKND_IND = pgmprd.incld_captv_payknd_ind,
                      AVG_TAX_MULTI_IND = pgmprd.avg_tax_multi_ind,
                      TAX_MULTI_FCTR_RT = pgmprd.tax_multi_fctr_rt,
                      CUSTMR_ID = pgmprd.custmr_id,
                      CUSTMR_ACTIVE = pgmprd.CUSTMR.actv_ind,
                      LOS_SENS_INFO_STRT_DT = pgmprd.los_sens_info_strt_dt,
                      LOS_SENS_INFO_END_DT = pgmprd.los_sens_info_end_dt,
                      FST_ADJ_MMS_FROM_INCP_CNT = pgmprd.fst_adj_mms_from_incp_cnt,
                      FNL_ADJ_DT = pgmprd.fnl_adj_dt,
                      BRKR_ID = pgmprd.brkr_id,
                      NXT_VALN_DT = pgmprd.nxt_valn_dt,
                      PREV_VALN_DT = pgmprd.prev_valn_dt,
                      COMB_ELEMTS_MAX_AMT = pgmprd.comb_elemts_max_amt,
                      PEO_PAY_IN_AMT = pgmprd.peo_pay_in_amt,
                      FST_ADJ_NON_PREM_MMS_CNT = pgmprd.fst_adj_non_prem_mms_cnt,
                      FREQ_NON_PREM_MMS_CNT = pgmprd.freq_non_prem_mms_cnt,
                      FNL_ADJ_NON_PREM_DT = pgmprd.fnl_adj_non_prem_dt,
                      NXT_VALN_DT_NON_PREM_DT = pgmprd.nxt_valn_dt_non_prem_dt,
                      PREV_VALN_DT_NON_PREM_DT = pgmprd.prev_valn_dt_non_prem_dt,
                      ZNA_SERV_COMP_CLM_HNDL_FEE_IND = pgmprd.zna_serv_comp_clm_hndl_fee_ind,
                      ACTV_IND = pgmprd.actv_ind,
                      PGM_TYP_ID = pgmprd.pgm_typ_id,
                      BNKRPT_BUYOUT_ID = pgmprd.bnkrpt_buyout_id,
                      BNKRPT_BUYOUT_EFF_DT = pgmprd.bnkrpt_buyout_eff_dt,
                      PAID_INCUR_TYP_ID = pgmprd.paid_incur_typ_id,
                      AGMT_ALOC_LOS_ADJ_EXPS_IND = pgmprd.agmt_aloc_los_adj_exps_ind,
                      AGMT_UNALOCTD_LOS_ADJ_IND = pgmprd.agmt_unaloctd_los_adj_ind,
                      AGMT_LOS_BASE_ASSES_IND = pgmprd.agmt_los_base_asses_ind,
                      LSI_ALOC_LOS_ADJ_EXPS_IND = pgmprd.lsi_aloc_los_adj_exps_ind,
                      LSI_UNALOCTD_LOS_ADJ_IND = pgmprd.lsi_unaloctd_los_adj_ind,
                      LSI_LOS_BASE_ASSES_IND = pgmprd.lsi_los_base_asses_ind,
                      AGMT_PAID_INCUR_IND = pgmprd.agmt_paid_incur_ind,
                      QLTY_CNTRL_DT = pgmprd.qlty_cntrl_dt,
                      QLTY_CMMNT_TXT = pgmprd.qlty_cmmnt_txt,
                      MSTR_ERND_RETRO_PREM_FRMLA_ID = pgmprd.mstr_ernd_retro_prem_frmla_id,
                      INCLD_LEGEND_IND = pgmprd.incld_legend_ind,
                      BSN_UNT_OFC_ID = pgmprd.bsn_unt_ofc_id,
                      BROKERNAME = pgmprd.INT_ORG.full_name + "/" + pgmprd.INT_ORG.city_nm,
                      CUSTMR_NAME = (from ct in this.Context.CUSTMRs
                                     where ct.custmr_id == pgmprd.custmr_id
                                     select ct.full_nm).First().ToString(),
                      FST_ADJ_DT = pgmprd.fst_adj_dt,
                      PROGRAMSTATUSID = (from pads in this.Context.PREM_ADJ_PGM_STs
                                         orderby pads.prem_adj_pgm_sts_id descending
                                         where pads.prem_adj_pgm_id == pgmprd.prem_adj_pgm_id
                                         select pads.pgm_perd_sts_typ_id).First().ToString(),
                      PROGRAMSTATUS = (from pads in this.Context.PREM_ADJ_PGM_STs
                                       join lkST in this.Context.LKUPs on pads.pgm_perd_sts_typ_id equals lkST.lkup_id
                                       orderby pads.prem_adj_pgm_sts_id descending
                                       where pads.prem_adj_pgm_id == pgmprd.prem_adj_pgm_id
                                       select lkST.lkup_txt).First().ToString(),
                      PREMIUMADJNUM = Convert.ToInt32((from pap in this.Context.PREM_ADJ_PERDs
                                                       join pno in this.Context.PREM_ADJs on pap.prem_adj_id equals pno.prem_adj_id
                                                       orderby pap.prem_adj_perd_id descending
                                                       where pap.prem_adj_pgm_id == pgmprd.prem_adj_pgm_id
                                                       select pap.prem_adj_id).First()),
                      PREMIUMADJNUMLOSS = Convert.ToInt32((from pap in this.Context.PREM_ADJ_PERDs
                                                           join pno in this.Context.PREM_ADJs on pap.prem_adj_id equals pno.prem_adj_id
                                                           orderby pap.prem_adj_perd_id descending
                                                           where pap.prem_adj_pgm_id == pgmprd.prem_adj_pgm_id
                                                           && (pno.adj_sts_typ_id == 346
                                                           || pno.adj_sts_typ_id == 348
                                                           || pno.adj_sts_typ_id == 350
                                                           || pno.adj_sts_typ_id == 351
                                                           || pno.adj_sts_typ_id == 353)
                                                           select pap.prem_adj_id).First()),
                      CUSTMR_PERS_REL_pers_id = cust_per.pers_id
                  };
            return pgmPeriod;
        }
        private IQueryable<ProgramPeriodBE> BuildCustomerList(int CustomerID, int BUOfficeID, int BrokerID)
        {
            if (this.Context == null)
                this.Initialize();
            /*  IQueryable<ProgramPeriodBE> customerlist = from pgmprd in this.Context.PREM_ADJ_PGMs
                                                         join cust in this.Context.CUSTMRs on pgmprd.custmr_id equals cust.custmr_id
                                                         where pgmprd.custmr_id == CustomerID && pgmprd.bsn_unt_ofc_id == BUOfficeID
                                                         && pgmprd.brkr_id == BrokerID */
            // changed on 2/10/2008 as the customerid,brokerid,businessunitid are not mandatory in search.
            if ((BUOfficeID > 0) || (BrokerID > 0))
            {
                IQueryable<ProgramPeriodBE> query = from pgmprd in this.Context.PREM_ADJ_PGMs
                                                    join cust in this.Context.CUSTMRs on pgmprd.custmr_id equals cust.custmr_id
                                                    where pgmprd.actv_ind == true && cust.actv_ind == true
                                                    //where pgmprd.custmr_id == CustomerID || pgmprd.bsn_unt_ofc_id == BUOfficeID
                                                    //|| pgmprd.brkr_id == BrokerID 

                                                    select new ProgramPeriodBE
                                                    {
                                                        CUSTMR_ID = cust.custmr_id,
                                                        CUSTMR_NAME = cust.full_nm,
                                                        BSN_UNT_OFC_ID = pgmprd.bsn_unt_ofc_id,
                                                        BRKR_ID = pgmprd.brkr_id
                                                    };
                query = query.Where(qq => qq.CUSTMR_ID == CustomerID || qq.BSN_UNT_OFC_ID == BUOfficeID || qq.BRKR_ID == BrokerID);
                if ((CustomerID > 0) && (BUOfficeID > 0) && (BrokerID > 0))
                    query = query.Where(qq => qq.CUSTMR_ID == CustomerID && qq.BSN_UNT_OFC_ID == BUOfficeID && qq.BRKR_ID == BrokerID);
                if ((CustomerID > 0) && (BUOfficeID > 0) && (BrokerID == 0))
                    query = query.Where(qq => qq.CUSTMR_ID == CustomerID && qq.BSN_UNT_OFC_ID == BUOfficeID);
                if ((CustomerID == 0) && (BUOfficeID > 0) && (BrokerID > 0))
                    query = query.Where(qq => qq.BSN_UNT_OFC_ID == BUOfficeID && qq.BRKR_ID == BrokerID);

                //changed to get distinct after clarification with siva on 15/10/2008.
                query = query.Distinct();
                query = query.OrderBy(o => o.CUSTMR_NAME);
                return query;
            }
            else
            {
                IQueryable<ProgramPeriodBE> query = from cust in this.Context.CUSTMRs
                                                    where cust.actv_ind == true
                                                    select new ProgramPeriodBE
                                                    {
                                                        CUSTMR_ID = cust.custmr_id,
                                                        CUSTMR_NAME = cust.full_nm,
                                                        
                                                    };

                query = query.Where(qq => qq.CUSTMR_ID == CustomerID);
                query = query.Distinct();
                query = query.OrderBy(o => o.CUSTMR_NAME);
                return query;
            }





           
        }

        public IList<ProgramPeriodBE> getrangeprogramperiods(int BUOfficeID, int BrokerID, char stChr, char edChr)
        {

            IList<ProgramPeriodBE> result = new List<ProgramPeriodBE>();
            IList<ProgramPeriodBE> temp = new List<ProgramPeriodBE>();

            IQueryable<ProgramPeriodBE> query = this.Buildrangesearch(BUOfficeID, BrokerID, stChr, edChr);

            temp = query.ToList();
            foreach (ProgramPeriodBE pp in temp)
            {
                if (result.Where(rs => rs.CUSTMR_ID == pp.CUSTMR_ID).Count() == 0)

                    result.Add(pp);
            }

            return result;

        }
        public IQueryable<ProgramPeriodBE> Buildrangesearch(int BUOfficeID, int BrokerID, char stChr, char edChr)
        {
            string rng = String.Empty;

            for (int chrLoop = stChr; chrLoop <= edChr; chrLoop++)
                rng = rng + ((char)chrLoop).ToString();

            if (this.Context == null)
                this.Initialize();
            /* var firstquery = from first in this.Context.CUSTMRs 

                      firstquery   */
            //where first.full_nm b
            if ((BUOfficeID > 0) || (BrokerID > 0))
            {
                IQueryable<ProgramPeriodBE> query =
                 from pgmprd in this.Context.PREM_ADJ_PGMs
                 join cust in this.Context.CUSTMRs on pgmprd.custmr_id equals cust.custmr_id
                 where pgmprd.actv_ind == true && cust.actv_ind == true
                 select new ProgramPeriodBE
                 {
                     CUSTMR_ID = cust.custmr_id,
                     CUSTMR_NAME = cust.full_nm,
                     BSN_UNT_OFC_ID = pgmprd.bsn_unt_ofc_id,
                     BRKR_ID = pgmprd.brkr_id
                 };


                query = query.Where(qq =>
                     qq.BSN_UNT_OFC_ID == BUOfficeID || qq.BRKR_ID == BrokerID ||
                     rng.IndexOf(qq.CUSTMR_NAME[0]) >= 0);

                if ((BUOfficeID > 0) && (BrokerID > 0))
                    query = query.Where(qq => rng.IndexOf(qq.CUSTMR_NAME[0]) >= 0 && qq.BSN_UNT_OFC_ID == BUOfficeID && qq.BRKR_ID == BrokerID);
                if ((BUOfficeID > 0) && (BrokerID == 0))
                    query = query.Where(qq => qq.BSN_UNT_OFC_ID == BUOfficeID && rng.IndexOf(qq.CUSTMR_NAME[0]) >= 0);
                if ((BUOfficeID == 0) && (BrokerID > 0))
                    query = query.Where(qq => qq.BRKR_ID == BrokerID && rng.IndexOf(qq.CUSTMR_NAME[0]) >= 0);

                query = query.OrderBy(qq => qq.CUSTMR_NAME);

                ////Filters the rows based on BU Office ID
                //if (BUOfficeID > 0)
                //    query = query.Where(qq => qq.BSN_UNT_OFC_ID == BUOfficeID);

                ////Filters the rows based on Broker ID
                //if (BrokerID > 0)
                //    query = query.Where(qq => qq.BRKR_ID == BrokerID);

                ////Filters the rows based on Range Letters Search for Account name
                //if (rng.Trim().Length > 0)
                //    query = query.Where(qq => rng.IndexOf(qq.CUSTMR_NAME[0]) >= 0);

                return query;
            }
            else
            {
                IQueryable<ProgramPeriodBE> query =
                     from cust in this.Context.CUSTMRs
                     where cust.actv_ind == true
                     select new ProgramPeriodBE
                     {
                         CUSTMR_ID = cust.custmr_id,
                         CUSTMR_NAME = cust.full_nm
                       
                     };


                query = query.Where(qq =>rng.IndexOf(qq.CUSTMR_NAME[0]) >= 0);
                query = query.OrderBy(qq => qq.CUSTMR_NAME);
                return query;

            }

        }
        private IQueryable<ProgramPeriodListBE> BuildQueryList()
        {
            if (this.Context == null)
                this.Initialize();

            IQueryable<ProgramPeriodListBE> pgmPeriod
                = from pgmprd in this.Context.PREM_ADJ_PGMs
                  orderby pgmprd.strt_dt descending, pgmprd.plan_end_dt descending
                  where pgmprd.actv_ind == true
                  select new ProgramPeriodListBE
                  {

                      PREM_ADJ_PGM_ID = pgmprd.prem_adj_pgm_id,
                      STRT_DT = pgmprd.strt_dt,
                      PLAN_END_DT = pgmprd.plan_end_dt,
                      VALN_MM_DT = pgmprd.valn_mm_dt,
                      CUSTMR_ID = pgmprd.custmr_id,
                      BRKR_ID = pgmprd.brkr_id,
                      PGM_TYP_ID = pgmprd.pgm_typ_id,
                      BSN_UNT_OFC_ID = pgmprd.bsn_unt_ofc_id

                  };
            return pgmPeriod;
        }

        public IList<ProgramPeriodBE> GetCustomerDetails(int CustomerID, int BUOfficeID, int BrokerID)
        {

            IList<ProgramPeriodBE> customerlist = new List<ProgramPeriodBE>();
            IList<ProgramPeriodBE> temp = new List<ProgramPeriodBE>();
            IQueryable<ProgramPeriodBE> result = this.BuildCustomerList(CustomerID, BUOfficeID, BrokerID);
            temp = result.ToList();

            foreach (ProgramPeriodBE pp in temp)
            {
                if (customerlist.Where(rs => rs.CUSTMR_ID == pp.CUSTMR_ID).Count() == 0)

                    customerlist.Add(pp);
            }

            return customerlist;

        }
        //used for AdjustmentReviewSearch.ascx to fill Val.Date and Program Period DropDowns
        public IList<ProgramPeriodSearchListBE> GetProgramPeriodSearch(int intaccountID, DateTime dtValDate)
        {
            IList<ProgramPeriodSearchListBE> result = new List<ProgramPeriodSearchListBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<ProgramPeriodSearchListBE> query = (from pap in this.Context.PREM_ADJ_PERDs
                                                           where pap.reg_custmr_id == intaccountID
                                                          && ((from pa in this.Context.PREM_ADJs
                                                               join ipap in this.Context.PREM_ADJ_PERDs
                                                                on pa.prem_adj_id equals pap.prem_adj_id
                                                               where pa.reg_custmr_id == intaccountID
                                                            && pa.valn_dt == dtValDate
                                                               select pa.prem_adj_id).ToList().Contains(pap.prem_adj_id))
                                                           select new ProgramPeriodSearchListBE
                                                           {
                                                               PREM_ADJ_PGM_ID = pap.PREM_ADJ_PGM.prem_adj_pgm_id,
                                                               STRT_DT = pap.PREM_ADJ_PGM.strt_dt,
                                                               PLAN_END_DT = pap.PREM_ADJ_PGM.plan_end_dt,
                                                               STARTDATE_ENDDATE = pap.PREM_ADJ_PGM.strt_dt + ":" + pap.PREM_ADJ_PGM.plan_end_dt,
                                                               ADJ_STS_TYP_ID = ((from premadjsts in this.Context.PREM_ADJ_STs
                                                                                  orderby premadjsts.prem_adj_sts_id descending
                                                                                  where premadjsts.prem_adj_id == pap.prem_adj_id
                                                                                  select premadjsts.adj_sts_typ_id).First())
                                                           }).Distinct();
            query = query.Where(premadj => premadj.ADJ_STS_TYP_ID != 347 && premadj.ADJ_STS_TYP_ID != 349 && premadj.ADJ_STS_TYP_ID != 352);

            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

        //used for AdjustmentReviewSearch.ascx to fill Val.Date and Program Period DropDowns
        public IList<ProgramPeriodSearchListBE> GetARProgramPeriodSearch(int intPremAdjID, int intaccountID)
        {
            IList<ProgramPeriodSearchListBE> result = new List<ProgramPeriodSearchListBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<ProgramPeriodSearchListBE> query = (from premadjpgm in this.Context.PREM_ADJ_PGMs
                                                           join premadjprd in this.Context.PREM_ADJ_PERDs on premadjpgm.prem_adj_pgm_id equals premadjprd.prem_adj_pgm_id
                                                           where premadjprd.prem_adj_id == intPremAdjID && premadjpgm.plan_end_dt != null && premadjpgm.strt_dt != null && premadjprd.custmr_id == intaccountID && premadjpgm.actv_ind == true
                                                           select new ProgramPeriodSearchListBE
                                                           {
                                                               PREM_ADJ_PGM_ID = premadjpgm.prem_adj_pgm_id,
                                                               STRT_DT = premadjpgm.strt_dt,
                                                               PLAN_END_DT = premadjpgm.plan_end_dt,
                                                               STARTDATE_ENDDATE = premadjpgm.strt_dt + ":" + premadjpgm.plan_end_dt,
                                                               PGMTYP = "(" + ((from lkup in this.Context.LKUPs where lkup.lkup_id == premadjpgm.pgm_typ_id select lkup.lkup_txt).First()) + ")",
                                                               ADJ_STS_TYP_ID = ((from premadjsts in this.Context.PREM_ADJ_STs
                                                                                  orderby premadjsts.prem_adj_sts_id descending
                                                                                  where premadjsts.prem_adj_id == premadjprd.prem_adj_id
                                                                                  select premadjsts.adj_sts_typ_id).First())
                                                           }).Distinct();
            query = query.Where(premadj => premadj.ADJ_STS_TYP_ID != 347 && premadj.ADJ_STS_TYP_ID != 349 && premadj.ADJ_STS_TYP_ID != 352);

            if (query.Count() > 0)
                result = query.ToList();
            result = (result.OrderByDescending(o => o.STRT_DT).ThenByDescending(o => o.PLAN_END_DT).ThenBy(o => o.PGMTYP)).ToList();
            return result;
        }

        //used for AdjustmentReviewSearch.ascx to fill Val.Date and Program Period DropDowns
        public IList<ProgramPeriodSearchListBE> GetProgramPeriodID(int intaccountID, DateTime dtValDate, DateTime dtStartDate, DateTime dtEndDate)
        {
            IList<ProgramPeriodSearchListBE> result = new List<ProgramPeriodSearchListBE>();
            if (this.Context == null)
                this.Initialize();
            //IQueryable<ProgramPeriodSearchListBE> query = (from pgmprd in this.Context.PREM_ADJ_PGMs
            //                                               where pgmprd.custmr_id == intaccountID
            //                                               && pgmprd.strt_dt == dtStartDate && pgmprd.plan_end_dt == dtEndDate && (pgmprd.nxt_valn_dt == dtValDate || pgmprd.nxt_valn_dt_non_prem_dt==dtValDate)
            //                                               select new ProgramPeriodSearchListBE
            //                                               {
            //                                                   PREM_ADJ_PGM_ID = pgmprd.prem_adj_pgm_id
            //                                               }).Distinct();
            IQueryable<ProgramPeriodSearchListBE> query = (from pgmprd in this.Context.PREM_ADJ_PGMs
                                                           join premadjprd in this.Context.PREM_ADJ_PERDs on pgmprd.prem_adj_pgm_id equals premadjprd.prem_adj_pgm_id
                                                           join premadj in this.Context.PREM_ADJs on premadjprd.prem_adj_id equals premadj.prem_adj_id
                                                           where premadjprd.reg_custmr_id == intaccountID
                                                           && pgmprd.strt_dt == dtStartDate && pgmprd.plan_end_dt == dtEndDate && (pgmprd.nxt_valn_dt == dtValDate || pgmprd.nxt_valn_dt_non_prem_dt == dtValDate)
                                                           select new ProgramPeriodSearchListBE
                                                           {
                                                               PREM_ADJ_PGM_ID = pgmprd.prem_adj_pgm_id
                                                           }).Distinct();
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
        //used for Lossinfo to fill  Program Period DropDown
        public IList<ProgramPeriodSearchListBE> GetProgramPeriods(int intPremAdjProgrmID)
        {
            IList<ProgramPeriodSearchListBE> result = new List<ProgramPeriodSearchListBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<ProgramPeriodSearchListBE> query = (from pgmprd in this.Context.PREM_ADJ_PGMs
                                                           where pgmprd.prem_adj_pgm_id == intPremAdjProgrmID
                                                           select new ProgramPeriodSearchListBE
                                                           {
                                                               PREM_ADJ_PGM_ID = pgmprd.prem_adj_pgm_id,
                                                               STRT_DT = pgmprd.strt_dt,
                                                               PLAN_END_DT = pgmprd.plan_end_dt,
                                                               STARTDATE_ENDDATE = pgmprd.strt_dt + ":" + pgmprd.plan_end_dt,
                                                               PGMTYP = "(" + ((from lkup in this.Context.LKUPs where lkup.lkup_id == pgmprd.pgm_typ_id select lkup.lkup_txt).First()) + ")"
                                                           }).Distinct();
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
        //used for Lossinfo to fill  Program Period DropDown
        public IList<ProgramPeriodSearchListBE> GetProgramPeriodsByPremAdjID(int intPremAdjID, DateTime dtValDate)
        {
            IList<ProgramPeriodSearchListBE> result = new List<ProgramPeriodSearchListBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<ProgramPeriodSearchListBE> query = (from pgmprd in this.Context.PREM_ADJ_PGMs
                                                           join premadjprd in this.Context.PREM_ADJ_PERDs on pgmprd.prem_adj_pgm_id equals premadjprd.prem_adj_pgm_id
                                                           join premadj in this.Context.PREM_ADJs on premadjprd.prem_adj_id equals premadj.prem_adj_id
                                                           where premadj.prem_adj_id == intPremAdjID && premadj.valn_dt == dtValDate
                                                           && pgmprd.actv_ind == true
                                                           select new ProgramPeriodSearchListBE
                                                           {
                                                               PREM_ADJ_PGM_ID = pgmprd.prem_adj_pgm_id,
                                                               STRT_DT = pgmprd.strt_dt,
                                                               PLAN_END_DT = pgmprd.plan_end_dt,
                                                               STARTDATE_ENDDATE = pgmprd.strt_dt + ":" + pgmprd.plan_end_dt
                                                           }).Distinct();
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
        //used for Lossinfo to fill  Program Period DropDown
        //public IList<ProgramPeriodSearchListBE> GetProgramPeriodsByCustmrID(int intCustmrID)
        //{
        //    IList<ProgramPeriodSearchListBE> result = new List<ProgramPeriodSearchListBE>();
        //    if (this.Context == null)
        //        this.Initialize();
        //    IQueryable<ProgramPeriodSearchListBE> query = (from pgmprd in this.Context.PREM_ADJ_PGMs
        //                                                   where pgmprd.custmr_id == intCustmrID
        //                                                   select new ProgramPeriodSearchListBE
        //                                                   {
        //                                                       PREM_ADJ_PGM_ID = pgmprd.prem_adj_pgm_id,
        //                                                       STRT_DT = pgmprd.strt_dt,
        //                                                       PLAN_END_DT = pgmprd.plan_end_dt,
        //                                                       STARTDATE_ENDDATE = pgmprd.strt_dt + ":" + pgmprd.plan_end_dt
        //                                                   }).Distinct();
        //    if (query.Count() > 0)
        //        result = query.ToList();
        //    return result;
        //}
        public IList<ProgramPeriodSearchListBE> GetProgramPeriodsByCustmrID(int intCustmrID)
        {
            IList<ProgramPeriodSearchListBE> result = new List<ProgramPeriodSearchListBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<ProgramPeriodSearchListBE> query = (from pgmprd in this.Context.PREM_ADJ_PGMs
                                                           where pgmprd.custmr_id == intCustmrID
                                                           && pgmprd.actv_ind == true
                                                           select new ProgramPeriodSearchListBE
                                                           {
                                                               PREM_ADJ_PGM_ID = pgmprd.prem_adj_pgm_id,
                                                               STRT_DT = pgmprd.strt_dt,
                                                               PLAN_END_DT = pgmprd.plan_end_dt,
                                                               PGMTYP = "(" + ((from lkup in this.Context.LKUPs where lkup.lkup_id == pgmprd.pgm_typ_id select lkup.lkup_txt).First()) + ")",
                                                               STARTDATE_ENDDATE = pgmprd.strt_dt + ":" + pgmprd.plan_end_dt
                                                           }).Distinct();
            if (query.Count() > 0)
                result = query.ToList();
            result = (result.OrderByDescending(o => o.STRT_DT).ThenByDescending(o => o.PLAN_END_DT).ThenBy(o => o.PGMTYP)).ToList();
            return result;
        }


        /// <summary>
        /// Used for AdjustmentInvoing Dashboard(Naresh)
        /// </summary>
        /// <param name="AccountNo"></param>
        /// <returns></returns>
        public IList<ProgramPeriodBE> GetProgramPeriodSearchDashboard(int AccountNo)
        {
            IList<ProgramPeriodBE> result = new List<ProgramPeriodBE>();
            if (this.Context == null)
                this.Initialize();
            var querry = from cus in this.Context.CUSTMRs
                         where cus.custmr_rel_id == AccountNo && cus.custmr_rel_actv_ind == true
                         select cus.custmr_id;
            IQueryable<ProgramPeriodBE> query =
            (from que in this.Context.PREM_ADJ_PGMs
             join lkPG in this.Context.LKUPs on que.pgm_typ_id equals lkPG.lkup_id
             orderby que.CUSTMR.full_nm ascending, que.strt_dt descending, que.plan_end_dt descending, que.valn_mm_dt descending
             where que.custmr_id == AccountNo || querry.Contains(que.custmr_id)
             && que.actv_ind == true
             select new ProgramPeriodBE()
             {
                 NXT_VALN_DT = que.nxt_valn_dt,
                 STRT_DT = que.strt_dt,
                 PLAN_END_DT = que.plan_end_dt,
                 PREM_ADJ_PGM_ID = que.prem_adj_pgm_id,

                 CUSTMR_NAME = que.CUSTMR.full_nm,
                 CUSTMR_ID = que.custmr_id,
                 PROGRAMPERIOD_ST_DATE = DateTime.Parse(que.strt_dt.ToString()).ToShortDateString(),
                 PROGRAMPERIOD_END_DATE = DateTime.Parse(que.plan_end_dt.ToString()).ToShortDateString(),
                 //VALUATION_DATE = DateTime.Parse(que.valn_mm_dt.ToString()).ToShortDateString(),
                 VALN_MM_DT = que.valn_mm_dt,
                 PROGRAMTYPE = lkPG.lkup_txt,
                 BROKER = que.EXTRNL_ORG.full_name,
                 BUOFFFICE = que.INT_ORG.bsn_unt_cd + ' ' + que.INT_ORG.full_name,
                 // PROGRAMSTATUS = lkST.lkup_txt,
                 PROGRAMSTATUS = (from pads in this.Context.PREM_ADJ_PGM_STs
                                  join lkST in this.Context.LKUPs on pads.pgm_perd_sts_typ_id equals lkST.lkup_id
                                  orderby pads.prem_adj_pgm_sts_id descending
                                  where pads.prem_adj_pgm_id == que.prem_adj_pgm_id
                                  select lkST.lkup_txt).First().ToString(),
                 PREM_ADJ_PER_ID = Convert.ToInt32((from perd in this.Context.PREM_ADJ_PERDs
                                                    where perd.prem_adj_pgm_id == que.prem_adj_pgm_id
                                                    select perd.prem_adj_perd_id).First()),
                 PGM_TYP_ID = que.pgm_typ_id,
                 PREMIUMADJNUM = Convert.ToInt32((from pap in this.Context.PREM_ADJ_PERDs
                                                  join pno in this.Context.PREM_ADJs on pap.prem_adj_id equals pno.prem_adj_id
                                                  orderby pap.prem_adj_perd_id descending
                                                  where pap.prem_adj_pgm_id == que.prem_adj_pgm_id
                                                  select pap.prem_adj_id).First()),
                 VALUATION_DATE = (from pap in this.Context.PREM_ADJ_PERDs
                                   join pno in this.Context.PREM_ADJs on pap.prem_adj_id equals pno.prem_adj_id
                                   orderby pap.prem_adj_perd_id descending
                                   where pap.prem_adj_pgm_id == que.prem_adj_pgm_id
                                   select pno.valn_dt).First().ToString(),
                 STARTDATE_ENDDATE = que.strt_dt + ":" + que.plan_end_dt,
             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
        /// <summary>
        /// Function to retrieve ProgrmPeriod Search for Invoice Dashboard Page
        /// This function and GetAdjInvcDshbrd stored proc returns all program periods
        /// which are(in Active status) either not linked to an adjustment or if linked to
        /// to an adjustment then only if adjustment is in "CALC" status 
        /// This stored proc retrieves program periods which are not linked to an adjustment
        /// or linked to an adjustment in  "CALC", "DRAFT-INVOICE", "QC-DRAFT INVOICE", "RECON REVIEW" 
        /// or "UW REVIEW"  status.  This query then only retreives adjustments in "CALC" status
        /// </summary>
        /// <param name="AccountNo"></param>
        /// <param name="ProgTypID"></param>
        /// <param name="adjNo"></param>
        /// <param name="valDate"></param>
        /// <returns></returns>
        public IList<ProgramPeriodBE> GetProgramPeriodSearchDashboard(int AccountNo, int ProgTypID, int adjNo, string valDate)
        {
            IList<ProgramPeriodBE> result = new List<ProgramPeriodBE>();
            if (this.Context == null)
                this.Initialize();

            IQueryable<ProgramPeriodBE> query = (from que in this.Context.GetAdjInvcDshbrd(AccountNo)
                                                 where que.nxt_valn_dt.Value.Subtract(System.DateTime.Now).Days <= 0
                                                 select new ProgramPeriodBE()
                                                 {
                                                     NXT_VALN_DT = que.nxt_valn_dt,
                                                     NXT_VALN_DT_NON_PREM_DT = que.nxt_valn_dt_non_prem_dt,
                                                     NXT_VALN_DT_PREM = que.NXT_VALN_DT_PREM,
                                                     STRT_DT = que.strt_dt,
                                                     PLAN_END_DT = que.plan_end_dt,
                                                     PREM_ADJ_PGM_ID = que.prem_adj_pgm_id,
                                                     CUSTMR_NAME = que.CustFullName,
                                                     CUSTMR_ID = que.custmr_id,
                                                     PROGRAMPERIOD_ST_DATE = DateTime.Parse(que.strt_dt.ToString()).ToShortDateString(),
                                                     PROGRAMPERIOD_END_DATE = DateTime.Parse(que.plan_end_dt.ToString()).ToShortDateString(),
                                                     VALN_MM_DT = que.valn_mm_dt,
                                                     PROGRAMTYPE = (from lkup in this.Context.LKUPs
                                                                    where lkup.lkup_id == que.pgm_typ_id.Value
                                                                    select lkup.lkup_txt).FirstOrDefault().ToString(),
                                                     BROKER = que.BrokerName,
                                                     BUOFFFICE = que.BUFullName + '/' + que.city_nm,
                                                     PROGRAMSTATUS = (from lkup in this.Context.LKUPs
                                                                      where lkup.lkup_id == que.pgm_perd_sts_typ_id.Value
                                                                      select lkup.lkup_txt).FirstOrDefault().ToString(),
                                                     PREM_ADJ_PER_ID = que.prem_adj_perd_id,
                                                     PGM_TYP_ID = que.pgm_typ_id,
                                                     PREMIUMADJNUM = ((que.prem_adj_id == 0 || que.prem_adj_perd_id == 0) ? (int?)null : que.prem_adj_id),
                                                     CALC_ADJ_STS_CODE = que.calc_adj_sts_cd,
                                                     /// If its a new program period prem_adj_id will be null
                                                     /// must handle that to avoid object reference not set message
                                                     PREMADJSTS = ((que.prem_adj_id == 0 || que.prem_adj_perd_id == 0) ? null :
                                                    (from pa in this.Context.PREM_ADJs
                                                     join lkup in this.Context.LKUPs
                                                     on pa.adj_sts_typ_id equals lkup.lkup_id
                                                     where pa.prem_adj_id == que.prem_adj_id
                                                     select lkup.lkup_txt).First()),
                                                     VALUATION_DATE = Convert.ToString(que.nxt_valn_dt),
                                                     STARTDATE_ENDDATE = que.strt_dt + ":" + que.plan_end_dt,
                                                     LSI_Custmr_Count = que.LSI_Custmr_Count,
                                                     ILRF_Setup_Count=que.ILRF_Setup_Count,
                                                 }).AsQueryable<ProgramPeriodBE>();
            query = query.Where(pg => pg.PREMADJSTS == null || pg.PREMADJSTS == "CALC");
            if (ProgTypID != 0)
            {
                query = query.Where(pg => pg.PGM_TYP_ID == ProgTypID);
            }
            if (adjNo != 0)
            {
                query = query.Where(pg => pg.PREMIUMADJNUM == adjNo);
            }
            if (valDate != "")
            {
                query = query.Where(pg => pg.NXT_VALN_DT == DateTime.Parse(valDate));
            }

            // query = query.Where(pg => pg.NXT_VALN_DT.Value.Subtract(System.DateTime.Now).Days<=0);


            result = query.ToList();
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="strPRGIDs"></param>
        /// <param name="strPERDIDs"></param>
        /// <param name="Flag"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string CalcDriver(int CustomerID, string strPRGIDs, string strPERDIDs, bool Flag, bool ILRFFlag, int UserID)
        {
            string ErroMsg;
            ErroMsg = string.Empty;

            if (this.Context == null)
                this.Initialize();
            this.Context.CommandTimeout = 7200;
            int intResult = this.Context.ModAISCalcDriver(CustomerID, strPRGIDs, strPERDIDs, Flag, ILRFFlag, UserID, ref ErroMsg, false);
            return ErroMsg;
        }
        //Invoice Inquiry
        public IList<ProgramPeriodSearchListBE> GetValDate(int accountID)
        {
            IList<ProgramPeriodSearchListBE> result = new List<ProgramPeriodSearchListBE>();
            if (this.Context == null)
                this.Initialize();
            //IQueryable<ProgramPeriodSearchListBE> query
            //   = (from pgmprd in this.Context.PREM_ADJ_PGMs
            //      where pgmprd.actv_ind == true && pgmprd.custmr_id == accountID && pgmprd.nxt_valn_dt != null
            //      orderby pgmprd.nxt_valn_dt
            //      select new ProgramPeriodSearchListBE
            //      {
            //          VALUATIONDATE = pgmprd.nxt_valn_dt.Value.ToShortDateString()

            //      }).Distinct();
            IQueryable<ProgramPeriodSearchListBE> query
               = (from premadj in this.Context.PREM_ADJs
                  where premadj.reg_custmr_id == accountID && premadj.valn_dt != null
                  orderby premadj.valn_dt
                  select new ProgramPeriodSearchListBE
                  {
                      VALUATIONDATE = premadj.valn_dt.ToShortDateString()

                  }).Distinct();


            result = query.ToList();
            return result;
        }
        //Adjustment Management Revision & Void(Naresh)
        /// <summary>
        /// Function for calling AdjustmentRevision Driver
        /// </summary>
        /// <param name="PremAdjID"></param>
        /// <param name="Custmr_id"></param>
        /// <param name="PersonID"></param>
        /// <returns></returns>
        public string AdjustmentRevision(int PremAdjID, int? Custmr_id, int? PersonID)
        {
            string ErroMsg;
            ErroMsg = string.Empty;

            if (this.Context == null)
                this.Initialize();
            this.Context.ModAdjRevisionDriver(0, PremAdjID, Custmr_id, PersonID, ref ErroMsg);
            return ErroMsg;
        }
        /// <summary>
        /// Fucntion for calling Adjustment CancelDriver
        /// </summary>
        /// <param name="PremAdjID"></param>
        /// <returns></returns>
        public string AdjustmentCancel(int PremAdjID)
        {
            string ErroMsg;
            ErroMsg = string.Empty;

            if (this.Context == null)
                this.Initialize();
            this.Context.ModAdjCancelDriver(0, PremAdjID, ref ErroMsg);
            return ErroMsg;
        }
        /// <summary>
        /// Function to check First ADjustment or Not.
        /// If it returns zero then First Adjustment.
        /// </summary>
        /// <param name="PremAdjPGMID"></param>
        /// <returns></returns>
        public int? CheckFirstAdjustment(int PremAdjPGMID)
        {

            if (this.Context == null)
                this.Initialize();
            return this.Context.fn_CheckFirstAdjustment(PremAdjPGMID);
        }
        /// <summary>
        /// Function for calling Adjustment Void Driver.
        /// </summary>
        /// <param name="PremAdjID"></param>
        /// <param name="Custmr_id"></param>
        /// <param name="PersonID"></param>
        /// <returns></returns>
        public string AdjustmentVoid(int PremAdjID, int? Custmr_id, int? PersonID)
        {
            string ErroMsg;
            ErroMsg = string.Empty;

            if (this.Context == null)
                this.Initialize();
            this.Context.ModAdjVoidDriver(0, PremAdjID, Custmr_id, PersonID, ref ErroMsg);
            return ErroMsg;
        }

        public IList<ProgramPeriodBE> getRoles(int intPersonID)
        {
            IList<ProgramPeriodBE> result = new List<ProgramPeriodBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<ProgramPeriodBE> query
               = (from custrel in this.Context.CUSTMR_PERS_RELs
                  where custrel.pers_id == intPersonID
                  select new ProgramPeriodBE
                  {
                      ROL_ID = custrel.rol_id
                  }).Distinct();

            result = query.ToList();
            return result;

        }

        public IList<ProgramPeriodBE> getRoles(int intCustmrID, int intPersonID)
        {
            IList<ProgramPeriodBE> result = new List<ProgramPeriodBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<ProgramPeriodBE> query
               = (from custrel in this.Context.CUSTMR_PERS_RELs
                  where custrel.pers_id == intPersonID
                  && custrel.custmr_id == intCustmrID
                  select new ProgramPeriodBE
                  {
                      ROL_ID = custrel.rol_id
                  }).Distinct();

            result = query.ToList();
            return result;

        }

        /// <summary>
        /// Method to Post Adjustments to ARies Transmittal after final Invoice
        /// </summary>
        /// <param name="intPremAdjID"></param>
        /// <param name="IND"></param>
        public void ModAISTransmittalToARiES(int intPremAdjID, int? IND)
        {
            string ErroMsg;
            ErroMsg = string.Empty;

            if (this.Context == null)
                this.Initialize();
            this.Context.ModAIS_TransmittalToARiES(intPremAdjID, null, ref ErroMsg, IND);
        }
        ///-----------------------------------------------------------------------------------///
        ///Added By:-Venkata R Kolimi
        ///Purpouse:-As Part of Bu Broker Review Work Order
        ///Created Date:-09/08/2009
        ///Modified By:
        ///Modified Date:
        ///Files Used:BuBrokerReview.apsx.cs
        ///-----------------------------------------------------------------------------------///
        /// <summary>
        /// Data acessor method used to retrieve the Program Period Info for BuBrokerReview screen
        /// </summary>
        /// <param name="intPremAdjID">intPremAdjID</param>
        /// <returns> IList<ProgramPeriodBE> </returns>
        public IList<ProgramPeriodBE> GetBuBrokerReviewProgramPeriodList(int intPremAdjID)
        {
            IList<ProgramPeriodBE> result = new List<ProgramPeriodBE>();

            IQueryable<ProgramPeriodBE> query
                = from pgmprd in this.Context.PREM_ADJ_PGMs
                  join premadgperd in Context.PREM_ADJ_PERDs on pgmprd.prem_adj_pgm_id equals premadgperd.prem_adj_pgm_id
                  where premadgperd.prem_adj_id==intPremAdjID
                  select new ProgramPeriodBE
                  {

                      PREM_ADJ_PGM_ID = pgmprd.prem_adj_pgm_id,
                      BRKR_CONCTC_ID = pgmprd.brkr_conctc_id,
                      STRT_DT = pgmprd.strt_dt,
                      PLAN_END_DT = pgmprd.plan_end_dt,
                      VALN_MM_DT = ((premadgperd.prem_non_prem_cd=="NP")?(pgmprd.nxt_valn_dt_non_prem_dt):(pgmprd.nxt_valn_dt)),
                      CUSTMR_ID = pgmprd.custmr_id,
                      BRKR_ID = pgmprd.brkr_id,
                      NXT_VALN_DT = pgmprd.nxt_valn_dt,
                      PREV_VALN_DT = pgmprd.prev_valn_dt,
                      NXT_VALN_DT_NON_PREM_DT = pgmprd.nxt_valn_dt_non_prem_dt,
                      PREV_VALN_DT_NON_PREM_DT = pgmprd.prev_valn_dt_non_prem_dt,
                      ACTV_IND = pgmprd.actv_ind,
                      PGM_TYP_ID = pgmprd.pgm_typ_id,
                      BSN_UNT_OFC_ID = pgmprd.bsn_unt_ofc_id,
                      CUSTMR_NAME = pgmprd.CUSTMR.full_nm,
                      BROKER = pgmprd.EXTRNL_ORG.full_name,
                      BUOFFFICE = pgmprd.INT_ORG.full_name + '/' + pgmprd.INT_ORG.city_nm,
                      PROGRAMTYPENAME=(from lkp in this.Context.LKUPs
                                           where lkp.lkup_id==pgmprd.pgm_typ_id
                                           select lkp.lkup_txt).First(),  
                      PREM_NON_PREM_CODE=premadgperd.prem_non_prem_cd,
                      BROKER_CONTACT_NM=(from per in this.Context.PERs
                                         where per.pers_id==pgmprd.brkr_conctc_id
                                         select per.surname + ", " + per.forename).First(),



                  };

            if (query.Count() > 0)
            {
                result = query.ToList();
                result = (result.OrderByDescending(o => o.STRT_DT).ThenByDescending(o => o.PLAN_END_DT).ThenBy(o => o.PROGRAMTYPENAME)).ToList();
            }

            return result;
        }



        /// <summary>
        /// Used for Adjustment QC for retrieving program periods related to the adjustment and custmer
        /// </summary>
        /// <param name="AccountNo"></param>
        /// <param name="intAdjNo"></param>
        /// <returns></returns>
        public IList<ProgramPeriodBE> GetQCProgramPeriods(int AccountNo,int intAdjNo)
        {
            IList<ProgramPeriodBE> result = new List<ProgramPeriodBE>();
            if (this.Context == null)
                this.Initialize();
            
            IQueryable<ProgramPeriodBE> query =
            (from que in this.Context.PREM_ADJ_PERDs
             join PGM in this.Context.PREM_ADJ_PGMs on que.prem_adj_pgm_id equals PGM.prem_adj_pgm_id
             orderby PGM.strt_dt descending, PGM.plan_end_dt descending, PGM.valn_mm_dt descending
             where que.prem_adj_id==intAdjNo && que.custmr_id==AccountNo && PGM.actv_ind == true
             select new ProgramPeriodBE()
             {
                 CUSTMR_ID=PGM.custmr_id,
                 STRT_DT = PGM.strt_dt,
                 PLAN_END_DT = PGM.plan_end_dt,
                 PREM_ADJ_PGM_ID = PGM.prem_adj_pgm_id,
                 STARTDATE_ENDDATE = PGM.strt_dt + ":" + PGM.plan_end_dt,
             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }


        

    }

}
