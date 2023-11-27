using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class PremiumAdjustmentPeriodDA : DataAccessor<PREM_ADJ_PERD, PremiumAdjustmentPeriodBE, AISDatabaseLINQDataContext>
    {
        public IList<PremiumAdjustmentPeriodBE> getPremAdjPerdID(int intCustmerID, int intPremAdjID, int intPremAdjPgrmID)
        {
            IList<PremiumAdjustmentPeriodBE> result = new List<PremiumAdjustmentPeriodBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<PremiumAdjustmentPeriodBE> query =
            (from cdd in this.Context.PREM_ADJ_PERDs
             where cdd.custmr_id == intCustmerID && cdd.prem_adj_id == intPremAdjID && cdd.prem_adj_pgm_id == intPremAdjPgrmID
             orderby cdd.prem_adj_perd_id
             select new PremiumAdjustmentPeriodBE()
             {
                 PREM_ADJ_PERD_ID = cdd.prem_adj_perd_id
             }).Distinct();

            /// Force an enumeration so that the SQL is only
            /// executed in this method

            result = query.ToList();
            return result;
        }

        //Used in Invoice Driver
        public IList<PremiumAdjustmentPeriodBE> GetProgramPeriods(int intPremAdjID)
        {
            IList<PremiumAdjustmentPeriodBE> result = new List<PremiumAdjustmentPeriodBE>();

            IQueryable<PremiumAdjustmentPeriodBE> query =
                from premadjperd in this.Context.PREM_ADJ_PERDs
                select new PremiumAdjustmentPeriodBE
                {
                    PREM_ADJ_PGM_ID = premadjperd.prem_adj_pgm_id,
                    PREM_ADJ_ID = premadjperd.prem_adj_id,
                    CUSTMR_ID = premadjperd.custmr_id,
                    PREM_ADJ_PERD_ID = premadjperd.prem_adj_perd_id,
                    PREM_NON_PREM_CODE = premadjperd.prem_non_prem_cd,
                };

            if (intPremAdjID > 0)
            {
                query = query.Where(premadjperd => premadjperd.PREM_ADJ_ID == intPremAdjID);
            }

            result = query.ToList();
            return result;
        }
        /// <summary>
        /// Retrives all records from PREM_ADJ_PERD table filtered with PREM_ADJ_PGM_ID
        /// </summary>
        /// <param name="intProgPeriodID"></param>
        /// <returns></returns>
        public IList<PremiumAdjustmentPeriodBE> GetPREMADJPeriods(int intProgPeriodID)
        {
            IList<PremiumAdjustmentPeriodBE> result = new List<PremiumAdjustmentPeriodBE>();

            IQueryable<PremiumAdjustmentPeriodBE> query =
                from premadjperd in this.Context.PREM_ADJ_PERDs
                where premadjperd.prem_adj_pgm_id == intProgPeriodID
                select new PremiumAdjustmentPeriodBE
                {
                    PREM_ADJ_PGM_ID = premadjperd.prem_adj_pgm_id,
                    PREM_ADJ_ID = premadjperd.prem_adj_id,
                    CUSTMR_ID = premadjperd.custmr_id,
                    PREM_ADJ_PERD_ID = premadjperd.prem_adj_perd_id,
                    PREM_NON_PREM_CODE = premadjperd.prem_non_prem_cd,
                    ADJ_STS_TYP_ID=premadjperd.PREM_ADJ.adj_sts_typ_id,
                };


            result = query.ToList();
            return result;
        }
        /// <summary>
        /// This method returns the listview details in Adjustment Number Webpage
        /// </summary>
        /// <returns></returns>
        public IList<PremiumAdjustmentPeriodBE> GetAdjNumberList(int intPremAdjID, int intPremAdjPgmID)
        {
            IList<PremiumAdjustmentPeriodBE> result = new List<PremiumAdjustmentPeriodBE>();

            IQueryable<PremiumAdjustmentPeriodBE> query =
                from premadjperd in this.Context.PREM_ADJ_PERDs
                join cust in this.Context.CUSTMRs
                on premadjperd.custmr_id equals cust.custmr_id
                where premadjperd.prem_adj_id == intPremAdjID && premadjperd.prem_adj_pgm_id == intPremAdjPgmID
                select new PremiumAdjustmentPeriodBE
                {
                    PREM_ADJ_PERD_ID = premadjperd.prem_adj_perd_id,
                    PREM_ADJ_PGM_ID = premadjperd.prem_adj_pgm_id,
                    PREM_ADJ_ID = premadjperd.prem_adj_id,
                    CUSTMR_NAME = cust.full_nm,
                    VALUATIONDATE = premadjperd.PREM_ADJ.valn_dt.ToShortDateString(),
                    ADJ_NBR = premadjperd.adj_nbr,
                    ADJ_NBR_TXT = premadjperd.adj_nbr_txt,
                    //ADJ_TYPE = (from cdd in this.Context.PREM_ADJ_PERD_TOTs
                    //            where cdd.prem_adj_perd_id == premadjperd.prem_adj_perd_id && cdd.prem_adj_id==premadjperd.prem_adj_id
                    //            select cdd.invc_adj_typ_txt).First().ToString(),
                    START_DATE = premadjperd.PREM_ADJ_PGM.strt_dt.Value.ToShortDateString(),
                    END_DATE = premadjperd.PREM_ADJ_PGM.plan_end_dt.Value.ToShortDateString(),
                    UPDATE_DATE=premadjperd.updt_dt,
                };

            result = query.ToList();
            return result;
        }
        /// <summary>
        /// returns all the child accounts PGM_PED list of Master Account
        /// </summary>
        /// <param name="MasterAccount"></param>
        /// <returns></returns>
        public IList<PremiumAdjustmentPeriodBE> getPeriodList(int MasterAccount)
        {
            IList<PremiumAdjustmentPeriodBE> result = new List<PremiumAdjustmentPeriodBE>();

            IQueryable<PremiumAdjustmentPeriodBE> query =
                from premadjperd in this.Context.PREM_ADJ_PERDs
                join cust in this.Context.CUSTMRs
                on premadjperd.custmr_id equals cust.custmr_id
                where premadjperd.reg_custmr_id == MasterAccount && premadjperd.custmr_id != MasterAccount
                select new PremiumAdjustmentPeriodBE
                {
                    CUSTMR_ID = premadjperd.custmr_id,
                    PREM_ADJ_PERD_ID = premadjperd.prem_adj_perd_id,
                    PREM_ADJ_PGM_ID = premadjperd.prem_adj_pgm_id,
                    PREM_ADJ_ID = premadjperd.prem_adj_id,
                    CUSTMR_NAME = cust.full_nm,
                    VALUATIONDATE = premadjperd.PREM_ADJ.valn_dt.ToShortDateString(),
                    ADJ_NBR = premadjperd.adj_nbr,
                    ADJ_NBR_TXT = premadjperd.adj_nbr_txt,
                    ADJ_STS_ID = (from cdd in this.Context.PREM_ADJ_STs
                                  where cdd.prem_adj_id == premadjperd.prem_adj_id
                                  orderby cdd.prem_adj_sts_id descending
                                  select cdd.adj_sts_typ_id).First(),
                    //premadjperd.PREM_ADJ.PREM_ADJ_STs
                    //ADJ_TYPE = (from cdd in this.Context.PREM_ADJ_PERD_TOTs
                    //            where cdd.prem_adj_perd_id == premadjperd.prem_adj_perd_id && cdd.prem_adj_id==premadjperd.prem_adj_id
                    //            select cdd.invc_adj_typ_txt).First().ToString(),
                    START_DATE = premadjperd.PREM_ADJ_PGM.strt_dt.Value.ToShortDateString(),
                    END_DATE = premadjperd.PREM_ADJ_PGM.plan_end_dt.Value.ToShortDateString()
                };

            result = query.ToList();
            return result;
        }

        /// <summary>
        /// Function for calling Add PremAdjustmetPeriod Total SP
        /// </summary>
        /// <param name="PremAdjID"></param>
        /// <param name="Custmr_id"></param>
        /// <param name="PersonID"></param>
        /// <returns></returns>
        public int AddPremAdjPerdTotal(int? PremAdjPerdID,int? PremAdjID, int? Custmr_id,int? PremAdjPgmID, int? CreateUserID)
        {
            int result=0;
            

            if (this.Context == null)
                this.Initialize();
            this.Context.AddPREM_ADJ_PERD_TOT(PremAdjPerdID, PremAdjID, Custmr_id, PremAdjPgmID, CreateUserID);
            return result;
        }
        /// <summary>
        /// Function for calling Add PremAdjustmetPeriod Total SP
        /// </summary>
        /// <param name="PremAdjID"></param>
        /// <param name="Custmr_id"></param>
        /// <param name="PersonID"></param>
        /// <returns></returns>
        public int deletePremAdjPerdTotal(int? PremAdjPerdID, int? PremAdjID, int? Custmr_id)
        {
            int result = 0;


            if (this.Context == null)
                this.Initialize();
            this.Context.DelPREM_ADJ_PERD_TOT(PremAdjPerdID, PremAdjID, Custmr_id);
            return result;
        }
      

    }
}
