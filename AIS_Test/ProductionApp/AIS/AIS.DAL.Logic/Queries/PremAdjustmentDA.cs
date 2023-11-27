using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class PremAdjustmentDA : DataAccessor<PREM_ADJ, PremiumAdjustmentBE, AISDatabaseLINQDataContext>
    {
        public IList<PremiumAdjustmentBE> getList()
        {
            IList<PremiumAdjustmentBE> result = new List<PremiumAdjustmentBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<PremiumAdjustmentBE> query =
            (from cdd in this.Context.PREM_ADJs
             select new PremiumAdjustmentBE()
             {
                 PREMIUM_ADJ_ID = cdd.prem_adj_id,
                 REL_PREM_ADJ_ID=cdd.rel_prem_adj_id,
                 ADJ_STS_TYP_ID=cdd.adj_sts_typ_id,
                 CUSTOMERID = cdd.reg_custmr_id,
                 VALN_DT = cdd.valn_dt,
                 BROKER_ID = cdd.brkr_id,
                 BU_OFF_ID = cdd.bu_office_id,
                 HISTORICAL_ADJ_IND = cdd.historical_adj_ind,
                 ADJ_VOID_IND = cdd.adj_void_ind,
                 ADJ_RRSN_IND=cdd.adj_rrsn_ind,
                 REV_RESN_ID=cdd.adj_rrsn_rsn_id,
                 FNL_INVC_NBR_TXT=cdd.fnl_invc_nbr_txt,
                 PREM_ADJ_PGMID = (from perd in this.Context.PREM_ADJ_PERDs
                                   where perd.prem_adj_id == cdd.prem_adj_id
                                   select perd.prem_adj_pgm_id).FirstOrDefault(),
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
        //public IList<LookupBE> getLOBLookUpData()
        //{
        //    IList<LookupBE> result = new List<LookupBE>();
        //    IQueryable<LookupBE> query =
        //    (from ams in this.Context.COML_AGMTs
        //     select new LookupBE { LookUpName = ams.pol_sym_txt }).Distinct();
        //    result = query.ToList();
        //    return result;
        //}

        /// <summary>
        /// To retrieve the Adjustment Info for the AccountManagementDashboard
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="statusID"></param>
        /// <param name="personID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>Returns the list of Adjustment Information for the given search criteria</returns>

        public IList<PremiumAdjustmentBE> GetAdjustmentInfo(int accountID, int statusID, int personID, string pending)
        {
            bool qualifier = false;
            IList<PremiumAdjustmentBE> result = new List<PremiumAdjustmentBE>();

            if (this.Context == null)
                this.Initialize();

            IQueryable<PremiumAdjustmentBE> query =
               (from que in this.Context.GetAcctMgmtAdjDtls1(personID, accountID, statusID)
                select new PremiumAdjustmentBE()
                {
                    PREMIUM_ADJ_ID = que.prem_adj_id,
                    CUSTOMERID = que.reg_custmr_id,
                    ACCOUNTID = que.child_custmr_id,
                    CUSTMR_NAME = que.custmr_name,
                    CHILD_CUSTMR_NAME = que.child_custmr_name,
                    CUSTMR_ACTIVE = que.custmr_active_ind,
                    VALN_DT = que.valn_dt,
                    ADJ_STATUS_TYP_ID = que.adj_sts_typ_id,
                    ADJ_STATUS = que.adj_sts_typ_lkup_txt,
                    ADJ_QC_IND = que.adj_qc_ind,
                    RECONCILER_REVW_IND = que.reconciler_revw_ind,
                    ADJ_PENDG_IND = que.adj_pendg_ind,
                    PREM_ADJ_PGM_ID = ((que.prem_adj_pgm_id == null) ? 0 : (int)que.prem_adj_pgm_id),
                    EFF_DT = que.adj_sts_eff_dt,
                    CUSTMR_PERS_REL_pers_id = que.custmr_per_id
                }).AsQueryable<PremiumAdjustmentBE>();
            
            if (statusID > 0)
            {
                query = query.Where(premadj => premadj.ADJ_STATUS_TYP_ID == statusID);
                qualifier = true;
            }

            if (pending.ToUpper() != "ALL")
            {
                query = query.Where(premadj => premadj.ADJ_PENDG_IND == Convert.ToBoolean(pending.ToUpper()));
                qualifier = true;
            }

            result = query.ToList();

            return result;
        }

        /// <summary>
        /// To retrieve the Adjustment Info for the AccountManagementDashboard initially
        /// </summary>
        /// <param name="personID"></param>
        /// <returns>Returns the list of Adjustment Information for the given search criteria</returns>

        public IList<PremiumAdjustmentBE> GetAdjustmentInfo(int personID)
        {
            bool qualifier = false;
            IList<PremiumAdjustmentBE> result = new List<PremiumAdjustmentBE>();

            if (this.Context == null)
                this.Initialize();

            IQueryable<PremiumAdjustmentBE> query =
               (from que in this.Context.GetAcctMgmtAdjDtls1(personID, 0, 0)
                select new PremiumAdjustmentBE()
                {
                    PREMIUM_ADJ_ID = que.prem_adj_id,
                    CUSTOMERID = que.reg_custmr_id,
                    ACCOUNTID = que.child_custmr_id,
                    CUSTMR_NAME = que.custmr_name,
                    CHILD_CUSTMR_NAME = que.child_custmr_name,
                    CUSTMR_ACTIVE = que.custmr_active_ind,
                    VALN_DT = que.valn_dt,
                    ADJ_STATUS_TYP_ID = que.adj_sts_typ_id,
                    ADJ_STATUS = que.adj_sts_typ_lkup_txt,
                    ADJ_QC_IND = que.adj_qc_ind,
                    RECONCILER_REVW_IND = que.reconciler_revw_ind,
                    ADJ_PENDG_IND = que.adj_pendg_ind,
                    PREM_ADJ_PGM_ID = ((que.prem_adj_pgm_id == null) ? 0 : (int)que.prem_adj_pgm_id),
                    EFF_DT = que.adj_sts_eff_dt,
                    CUSTMR_PERS_REL_pers_id = que.custmr_per_id
                }).AsQueryable<PremiumAdjustmentBE>();

            result = query.ToList();

            return result;
        }


        /// <summary>
        /// To retrieve the Invoice Info for the AccountManagementDashboard
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="personID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>Returns the list of Invoice Information for the given search criteria</returns>

        public IList<PremiumAdjustmentBE> GetInvoiceInfo(int accountID, int personID, string qcComplete, string ariesClrng, bool Historical)
        {
            bool qualifier = false;
            IList<PremiumAdjustmentBE> result = new List<PremiumAdjustmentBE>();

            if (this.Context == null)
                this.Initialize();

            IQueryable<PremiumAdjustmentBE> query =
            (from premadj in this.Context.PREM_ADJs
             join premperd in this.Context.PREM_ADJ_PERDs on premadj.prem_adj_id equals premperd.prem_adj_id
             join cust_per in this.Context.CUSTMR_PERS_RELs on premperd.custmr_id equals cust_per.custmr_id

             select new PremiumAdjustmentBE()
             {
                 HISTORICAL_ADJ_IND = (premadj.historical_adj_ind == null ? false : premadj.historical_adj_ind),
                 PREMIUM_ADJ_ID = premadj.prem_adj_id,
                 ACCOUNTID = premperd.custmr_id,
                 CUSTOMERID = premadj.reg_custmr_id,
                 CUSTMR_NAME = (from ct in this.Context.CUSTMRs
                                where ct.custmr_id == premperd.custmr_id
                                select ct.full_nm).First().ToString(),
                 TWENTY_PCT_QLTY_CNTRL_IND = premadj.twenty_pct_qlty_cntrl_ind ?? false,
                 TWENTY_REQ_IND = premadj.twenty_pct_qlty_cntrl_reqr_ind == null ? false : premadj.twenty_pct_qlty_cntrl_reqr_ind,
                 VALN_DT = premadj.valn_dt,

                 INVC_NBR_TXT = (premadj.fnl_invc_nbr_txt != null ? premadj.fnl_invc_nbr_txt : premadj.drft_invc_nbr_txt),
                 //FINALINVNO=premadj.fnl_invc_nbr_txt,
                 //DRAFTINVNO=premadj.drft_invc_nbr_txt,
                 ADJ_STATUS_TYP_ID = premadj.adj_sts_typ_id,
                 //Bug #10183
                 INVC_DUE_DT = (premadj.fnl_invc_dt !=null ? premadj.fnl_invc_dt : premadj.drft_invc_dt),
                 //INVC_DUE_DT = premadj.invc_due_dt,
                 DRFT_INTRNL_PDF_ZDW_KEY_TXT = premadj.drft_intrnl_pdf_zdw_key_txt,
                 DRFT_EXTRNL_PDF_ZDW_KEY_TXT = premadj.drft_extrnl_pdf_zdw_key_txt,
                 DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT = premadj.drft_cd_wrksht_pdf_zdw_key_txt,
                 PREM_ADJ_PGM_ID = (from premadjpgm in this.Context.PREM_ADJ_PGMs
                                    where premadjpgm.custmr_id == premadj.reg_custmr_id
                                    select premadjpgm.prem_adj_pgm_id).First(),
                 ARIES_CMPLT_IND = (from aries in this.Context.PREM_ADJ_ARIES_CLRINGs
                                    where (aries.custmr_id == accountID || accountID==0)
                                    && aries.prem_adj_id == premadj.prem_adj_id 
                                    //&& aries.qlty_cntrl_pers_id == personID
                                    select aries.aries_cmplt_ind ).First()??false,
                 EFF_DT = Convert.ToDateTime((from pap in this.Context.PREM_ADJ_STs
                                              orderby pap.prem_adj_sts_id descending
                                              where pap.prem_adj_id == premadj.prem_adj_id
                                              select pap.eff_dt).First().ToString()),
                 INVC_AMT = (this.Context.fn_GetTotalforAdjInv(premadj.prem_adj_id)).ToString(),
                 CUSTMR_PERS_REL_pers_id = cust_per.pers_id
             }).Distinct();

            query = query.Where(Prem => Prem.ADJ_STATUS_TYP_ID != 347); // 347 = Cancelled

            query = query.Where(Prem => Prem.HISTORICAL_ADJ_IND == Historical);
            
            if (accountID > 0)
            {
                qualifier = true;
                query = query.Where(premadj => premadj.CUSTOMERID == accountID);
            }

            if (personID > 0)
            {
                qualifier = true;
                query = query.Where(premadj => premadj.CUSTMR_PERS_REL_pers_id == personID);
            }
            if (!(qcComplete.ToUpper() == "ALL"))
            {
                query = query.Where(premadj => premadj.TWENTY_PCT_QLTY_CNTRL_IND == Convert.ToBoolean(qcComplete.ToUpper()));
            }
            if (!(ariesClrng.ToUpper() == "ALL"))
            {
                
                //query = from aries in this.Context.PREM_ADJ_ARIES_CLRINGs
                       // where aries.ARIES_CMPLT_IND == Convert.ToBoolean(ariesClrng.ToUpper())
                       // select aries;
               query = query.Where(premadj => premadj.ARIES_CMPLT_IND == Convert.ToBoolean(ariesClrng.ToUpper()));
            }
            query = query.Where(premadj => premadj.INVC_NBR_TXT.ToString().Trim().Length > 0);
            query = query.OrderByDescending(o => o.INVC_DUE_DT).OrderByDescending(o => o.VALN_DT).OrderBy(o => o.CUSTMR_NAME);
            if (qualifier)
            {
                result = query.ToList();
            }
            return result;
        }
        public class DistinctDate : IEqualityComparer<PremiumAdjustmetSearchBE>
        {
            public bool Equals(PremiumAdjustmetSearchBE x, PremiumAdjustmetSearchBE y)
            {
                return x.VALUATIONDATE.Equals(y.VALUATIONDATE);
            }

            public int GetHashCode(PremiumAdjustmetSearchBE obj)
            {
                return obj.VALUATIONDATE.GetHashCode();
            }
        }

        //used for AdjustmentReviewSearch.ascx to fill Val.Date and Program Period DropDowns
        public IList<PremiumAdjustmetSearchBE> GetValDateSearch(int intaccountID)
        {
            IList<PremiumAdjustmetSearchBE> result = new List<PremiumAdjustmetSearchBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<PremiumAdjustmetSearchBE> query = (from preadjprd in this.Context.PREM_ADJ_PERDs
                                                          join preadj in this.Context.PREM_ADJs on preadjprd.prem_adj_id equals preadj.prem_adj_id
                                                          where preadjprd.custmr_id == intaccountID
                                                           && preadj.valn_dt != null
                                                          select new PremiumAdjustmetSearchBE
                                                          {
                                                              VALUATIONDATE = preadj.valn_dt.ToShortDateString(),
                                                              ADJ_STS_TYP_ID = ((from pap in this.Context.PREM_ADJ_STs
                                                                                 orderby pap.prem_adj_sts_id descending
                                                                                 where pap.prem_adj_id == preadj.prem_adj_id
                                                                                 select pap.adj_sts_typ_id).First())

                                                          }).Distinct();
            query = query.Where(premadj => premadj.ADJ_STS_TYP_ID != 347 && premadj.ADJ_STS_TYP_ID != 349 && premadj.ADJ_STS_TYP_ID != 352);
            if (query.Count() > 0)
                result = query.ToList().Distinct(new DistinctDate()).ToList();
            return result;
        }

        //used for AdjustmentReviewSearch.ascx to fill Val.Date and Program Period DropDowns
        public IList<PremiumAdjustmetSearchBE> GetAdjNumberSearch(int intaccountID, int intPremAdjPgmID, DateTime dtValDate)
        {
            IList<PremiumAdjustmetSearchBE> result = new List<PremiumAdjustmetSearchBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<PremiumAdjustmetSearchBE> query = (from preadj in this.Context.PREM_ADJs
                                                          join preadjprd in this.Context.PREM_ADJ_PERDs
                                                          on preadj.prem_adj_id equals preadjprd.prem_adj_id
                                                          where preadj.reg_custmr_id == intaccountID
                                                          && preadj.valn_dt == dtValDate
                                                          && preadjprd.prem_adj_pgm_id == intPremAdjPgmID
                                                          select new PremiumAdjustmetSearchBE
                                                          {
                                                              PREM_ADJ_ID = preadj.prem_adj_id,
                                                              ADJ_STS_TYP_ID = ((from pap in this.Context.PREM_ADJ_STs
                                                                                 orderby pap.prem_adj_sts_id descending
                                                                                 where pap.prem_adj_id == preadj.prem_adj_id
                                                                                 select pap.adj_sts_typ_id).First())

                                                          }).Distinct();
            query = query.Where(premadj => premadj.ADJ_STS_TYP_ID != 347 && premadj.ADJ_STS_TYP_ID != 349 && premadj.ADJ_STS_TYP_ID != 352);
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

        //used for AdjustmentReviewSearch.ascx to fill Val.Date and Program Period DropDowns
        public IList<PremiumAdjustmetSearchBE> GetARAdjNumberSearch(int intaccountID, DateTime dtValDate)
        {
            IList<PremiumAdjustmetSearchBE> result = new List<PremiumAdjustmetSearchBE>();
            if (this.Context == null)
                this.Initialize();
            IQueryable<PremiumAdjustmetSearchBE> query = (from preadj in this.Context.PREM_ADJs
                                                          join preadjprd in this.Context.PREM_ADJ_PERDs on preadj.prem_adj_id equals preadjprd.prem_adj_id
                                                          where preadjprd.custmr_id == intaccountID
                                                          && preadj.valn_dt == dtValDate
                                                          select new PremiumAdjustmetSearchBE
                                                          {
                                                              PREM_ADJ_ID = preadj.prem_adj_id,
                                                              ADJ_STS_TYP_ID = ((from pap in this.Context.PREM_ADJ_STs
                                                                                 orderby pap.prem_adj_sts_id descending
                                                                                 where pap.prem_adj_id == preadj.prem_adj_id
                                                                                 select pap.adj_sts_typ_id).First())

                                                          }).Distinct();
            query = query.Where(premadj => premadj.ADJ_STS_TYP_ID != 347 && premadj.ADJ_STS_TYP_ID != 349 && premadj.ADJ_STS_TYP_ID != 352);
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }


        public IList<PremiumAdjustmentBE> GetAdjNumberSearch(int AcctNo)
        {
            IList<PremiumAdjustmentBE> result = new List<PremiumAdjustmentBE>();

            if (this.Context == null)
                this.Initialize();
            var querry = from cus in this.Context.CUSTMRs
                         where cus.custmr_rel_id == AcctNo
                         select cus.custmr_id;
            IQueryable<PremiumAdjustmentBE> query =
            (from prmadj in this.Context.PREM_ADJs
             join cdd in this.Context.PREM_ADJ_PERDs
             on prmadj.prem_adj_id equals cdd.prem_adj_id
             where prmadj.reg_custmr_id == AcctNo || querry.Contains(prmadj.reg_custmr_id)
             select new PremiumAdjustmentBE()
             {
                 PREMIUM_ADJ_ID = prmadj.prem_adj_id,
                 //PREM_ADJ_PERD = cdd.prem_adj_perd_id,
                 ADJ_STATUS = (from pap in this.Context.PREM_ADJ_STs
                               join lkup in this.Context.LKUPs
                               on pap.adj_sts_typ_id equals lkup.lkup_id
                               orderby pap.prem_adj_sts_id descending
                               where pap.prem_adj_id == prmadj.prem_adj_id
                               select lkup.lkup_txt).First(),
                 //CUSTOMERID = prmadj.reg_custmr_id,
                 VALUATIONDATE = prmadj.valn_dt.ToShortDateString(),
                 //                 PREM_ADJ_CMMNT_ID = prmadj.prem_adj_cmmnt_id,
                 //CRTE_DT = prmadj.crte_dt,
                 //CRTE_USER_ID = prmadj.crte_user_id

             }).Distinct();
            query = query.Where(pg => pg.ADJ_STATUS == null || pg.ADJ_STATUS == "CALC");

            result = query.ToList();
            return result;
        }
        public IList<PremiumAdjustmentBE> GetAdjNumberSearchDates(int AcctNo)
        {
            IList<PremiumAdjustmentBE> result = new List<PremiumAdjustmentBE>();

            if (this.Context == null)
                this.Initialize();
            var querry = from cus in this.Context.CUSTMRs
                         where cus.custmr_rel_id == AcctNo
                         select cus.custmr_id;
            IQueryable<PremiumAdjustmentBE> query =
            (from prmadj in this.Context.PREM_ADJs
             join cdd in this.Context.PREM_ADJ_PERDs
             on prmadj.prem_adj_id equals cdd.prem_adj_id
             where prmadj.reg_custmr_id == AcctNo || querry.Contains(prmadj.reg_custmr_id)
             select new PremiumAdjustmentBE()
             {
                 //PREMIUM_ADJ_ID = prmadj.prem_adj_id,
                 //PREM_ADJ_PERD = cdd.prem_adj_perd_id,
                 ADJ_STATUS = (from pap in this.Context.PREM_ADJ_STs
                               join lkup in this.Context.LKUPs
                               on pap.adj_sts_typ_id equals lkup.lkup_id
                               orderby pap.prem_adj_sts_id descending
                               where pap.prem_adj_id == prmadj.prem_adj_id
                               select lkup.lkup_txt).First(),
                 //CUSTOMERID = prmadj.reg_custmr_id,
                 VALUATIONDATE = prmadj.valn_dt.ToShortDateString(),
                 //                 PREM_ADJ_CMMNT_ID = prmadj.prem_adj_cmmnt_id,
                 //CRTE_DT = prmadj.crte_dt,
                 //CRTE_USER_ID = prmadj.crte_user_id

             }).Distinct();
            query = query.Where(pg => pg.ADJ_STATUS == null || pg.ADJ_STATUS == "CALC");

            result = query.ToList();
            return result;
        }
        //Get E-Mail Notification Info
        public IList<PremiumAdjustmentBE> GetEMailInfo(int intAdjNo)
        {
            IList<PremiumAdjustmentBE> result = new List<PremiumAdjustmentBE>();

            if (this.Context == null)
                this.Initialize();
            IQueryable<PremiumAdjustmentBE> query =
            (from prmadj in this.Context.PREM_ADJs
             where prmadj.prem_adj_id == intAdjNo
             select new PremiumAdjustmentBE()
             {
                 CUSTOMERID = prmadj.reg_custmr_id,
                 CUSTMR_NAME = prmadj.CUSTMR.full_nm,
                 BROKERNAME = prmadj.EXTRNL_ORG.full_name,
                 BUNAME = prmadj.INT_ORG.full_name,
                 VALUATIONDATE = prmadj.valn_dt.ToShortDateString(),
                 INVC_AMT = this.Context.fn_GetTotalforAdjInv(prmadj.prem_adj_id).Value.ToString(),
                 FINALINVNO = prmadj.fnl_invc_nbr_txt,
                 DRAFTINVNO = prmadj.drft_invc_nbr_txt,
                 DRFT_INVC_DT = prmadj.drft_invc_dt,
                 FNL_INVC_DT = prmadj.fnl_invc_dt,
                 PROGRAMPERIOD_STDT = prmadj.PREM_ADJ_PERDs.First().PREM_ADJ_PGM.strt_dt,
                 PROGRAMPERIOD_ENDT = prmadj.PREM_ADJ_PERDs.First().PREM_ADJ_PGM.plan_end_dt,


             });


            result = query.ToList();
            return result;
        }
        //Used in Invoice Driver
        public string getZDWKey(int intAdjNo, char cKeyType, int IFlag)
        {
            string strZDWKey = string.Empty;

            if (this.Context == null)
                this.Initialize();
            var querry = (from cdd in this.Context.PREM_ADJs
                          where cdd.prem_adj_id == intAdjNo
                          select cdd).First();
            if (IFlag == 1)
            {
                switch (cKeyType)
                {
                    case 'I':
                        strZDWKey = querry.drft_intrnl_pdf_zdw_key_txt;
                        break;
                    case 'E':
                        strZDWKey = querry.drft_extrnl_pdf_zdw_key_txt;
                        break;
                    case 'C':
                        strZDWKey = querry.drft_cd_wrksht_pdf_zdw_key_txt;
                        break;

                }

            }
            if (IFlag == 2)
            {
                switch (cKeyType)
                {
                    case 'I':
                        strZDWKey = querry.drft_intrnl_pdf_zdw_key_txt;
                        break;
                    case 'E':
                        strZDWKey = querry.drft_extrnl_pdf_zdw_key_txt;
                        break;
                    case 'C':
                        strZDWKey = querry.drft_cd_wrksht_pdf_zdw_key_txt;
                        break;

                }

            }

            return strZDWKey;
        }
        /// <summary>
        /// Retrieves the Email Ids for the persons
        /// </summary>
        /// <param name="strAdjStage">Adjustment Stage</param>
        /// <param name="AdjAction">Bool Rejected/Accepted</param>
        /// <param name="custmrID">CustmrID</param>
        /// <returns>List of Email IDS</returns>
        public IList<string> getEmailIDS(string strAdjStage, bool AdjAction, int custmrID)
        {
            int role_id1 = 0;
            int role_id2 = 0;
            int role_id3 = 0;
            int role_id4 = 0;
            int count = 1;
            switch (strAdjStage)
            {
                case GlobalConstants.RESPONSIBILITIES.DRAFT:
                    role_id1 = GlobalConstants.RESPONSIBILITIES.intADJQC;
                    break;
                case GlobalConstants.RESPONSIBILITIES.ADJQC:
                    if (AdjAction)//Accepted
                        role_id1 = GlobalConstants.RESPONSIBILITIES.intRECON;
                    else//Rejected
                    {
                        role_id1 = GlobalConstants.RESPONSIBILITIES.intCFS2;
                        role_id2 = GlobalConstants.RESPONSIBILITIES.intADJQC;
                        role_id3 = GlobalConstants.RESPONSIBILITIES.intRECON;
                        count = 3;
                    }
                    break;
                case GlobalConstants.RESPONSIBILITIES.RECON:
                    if (AdjAction)//Accepted
                        role_id1 = GlobalConstants.RESPONSIBILITIES.intLSSADMIN;
                    else//Rejected
                    {
                        role_id1 = GlobalConstants.RESPONSIBILITIES.intCFS2;
                        role_id2 = GlobalConstants.RESPONSIBILITIES.intADJQC;
                        role_id3 = GlobalConstants.RESPONSIBILITIES.intRECON;
                        count = 3;
                    }
                    break;
                case GlobalConstants.RESPONSIBILITIES.FINAL_INVOICE:
                    count = 4;
                    role_id1 = GlobalConstants.RESPONSIBILITIES.intADJQC;
                    role_id2 = GlobalConstants.RESPONSIBILITIES.intRECON;
                    role_id3 = GlobalConstants.RESPONSIBILITIES.intCFS2;
                    role_id4 = GlobalConstants.RESPONSIBILITIES.intCOLLECT_REPRST;
                    //if (AdjAction)//Accepted
                    //{
                    //    role_id1 = GlobalConstants.RESPONSIBILITIES.intADJQC;
                    //    role_id2 = GlobalConstants.RESPONSIBILITIES.intRECON;
                    //}
                    //else//Rejected
                    //{
                    //    role_id1 = GlobalConstants.RESPONSIBILITIES.intCFS2;
                    //    role_id2 = GlobalConstants.RESPONSIBILITIES.intCOLLECT_REPRST;
                    //}
                    break;

            }
            IList<string> strEmailIDs = new List<string>();
            var persQuery = from cust in this.Context.CUSTMR_PERS_RELs
                            select cust.PER.email_txt;
            if (count == 1)
            {
                persQuery =
                  (from cust in this.Context.CUSTMR_PERS_RELs
                   where cust.custmr_id == custmrID && cust.rol_id == role_id1
                   select cust.PER.email_txt);
            }
            else if (count == 2)
            {
                persQuery =
                  (from cust in this.Context.CUSTMR_PERS_RELs
                   where cust.custmr_id == custmrID && (cust.rol_id == role_id1 || cust.rol_id == role_id2)
                   && cust.PER.email_txt != ""
                   select cust.PER.email_txt);
            }
            else if (count == 3)
            {
                persQuery =
                  (from cust in this.Context.CUSTMR_PERS_RELs
                   where cust.custmr_id == custmrID && (cust.rol_id == role_id1 || cust.rol_id == role_id2 || cust.rol_id == role_id3)
                   select cust.PER.email_txt);
            }
            else if (count == 4)
            {
                persQuery =
                  (from cust in this.Context.CUSTMR_PERS_RELs
                   where cust.custmr_id == custmrID && (cust.rol_id == role_id1 || cust.rol_id == role_id2 || cust.rol_id == role_id3 || cust.rol_id == role_id4)
                   select cust.PER.email_txt);
            }
            if (persQuery.Count() > 0)
                strEmailIDs = persQuery.ToList();
            return strEmailIDs;
        }
        /// <summary>
        /// Retrieves the voided Adj ID Details
        /// </summary>
        /// <param name="intPremAdjID"></param>
        /// <returns></returns>
        public PremiumAdjustmentBE getVoidedAdjustmentRow(int intPremAdjID)
        {
            PremiumAdjustmentBE result = new PremiumAdjustmentBE();
            if (this.Context == null)
                this.Initialize();
            IQueryable<PremiumAdjustmentBE> query
               = (from cdd in this.Context.PREM_ADJs
                  where cdd.rel_prem_adj_id == intPremAdjID
                  select new PremiumAdjustmentBE
                  {
                      PREMIUM_ADJ_ID = cdd.prem_adj_id,
                      FINALINVNO = cdd.fnl_invc_nbr_txt

                  }).Distinct();

            result = query.ToList().OrderByDescending(qr=>qr.PREMIUM_ADJ_ID).ToList()[0];
            return result;

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
        /// Data acessor method used to retrieve the adjustment Info for BuBrokerReview screen
       /// </summary>
        /// <param name="intPremAdjID">intPremAdjID</param>
        /// <returns> IList<PremiumAdjustmentBE> </returns>
        public IList<PremiumAdjustmentBE> getBuBrokerReviewAdjustmentInfo(int intPremAdjID)
        {
            IList<PremiumAdjustmentBE> result = new List<PremiumAdjustmentBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<PremiumAdjustmentBE> query =
            (from cdd in this.Context.PREM_ADJs
             where cdd.prem_adj_id==intPremAdjID
             select new PremiumAdjustmentBE()
             {
                 PREMIUM_ADJ_ID = cdd.prem_adj_id,
                 REL_PREM_ADJ_ID = cdd.rel_prem_adj_id,
                 ADJ_STS_TYP_ID = cdd.adj_sts_typ_id,
                 CUSTOMERID = cdd.reg_custmr_id,
                 CUSTMR_NAME=cdd.CUSTMR.full_nm,
                 VALN_DT = cdd.valn_dt,
                 BROKER_ID = cdd.brkr_id,
                 BU_OFF_ID = cdd.bu_office_id,
                 BROKERNAME = cdd.EXTRNL_ORG.full_name,
                 BUNAME = cdd.INT_ORG.full_name + '/' + cdd.INT_ORG.city_nm,
                 CALC_ADJ_STS_CODE=cdd.calc_adj_sts_cd,
                 ADJ_STATUS=(from status in this.Context.LKUPs 
                             where status.lkup_id==cdd.adj_sts_typ_id
                             select status.lkup_txt).First(),
             });

            /// Force an enumeration so that the SQL is only
            /// executed in this method
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

    }
}
