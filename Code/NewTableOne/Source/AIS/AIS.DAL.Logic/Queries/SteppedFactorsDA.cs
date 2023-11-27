using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;

namespace ZurichNA.AIS.DAL.Logic
{
    public class SteppedFactorsDA : DataAccessor<STEPPED_FCTR, SteppedFactorBE, AISDatabaseLINQDataContext>
    {

        /// <summary>
        /// Constructor for DEPPolicyDA class
        /// </summary>
        public SteppedFactorsDA()
        {

        }

        /// <summary>
        /// Function to retrieve the Data for DEP Policy
        /// </summary>
        /// <param name="PolicyID">Policy ID</param>
        /// <returns></returns>
        public IList<SteppedFactorBE> GetSteppedFactorData(int PolicyID)
        {
            IQueryable<SteppedFactorBE> result = this.BuildQuery();
            result = result.Where(res => res.POLICY_ID == PolicyID);
            return result.ToList();
        }

        /// <summary>
        /// Function to retrieve the Data for given program period ID
        /// </summary>
        /// <param name="PolicyID">Policy ID</param>
        /// <returns></returns>
        public IList<SteppedFactorBE> GetSteppedFactorDataByPgmID(int PremAdjPgmID)
        {
            IQueryable<SteppedFactorBE> result = (from sf in this.Context.STEPPED_FCTRs
                                                 join ca in this.Context.COML_AGMTs
                                                 on sf.coml_agmt_id equals ca.coml_agmt_id
                                                 where sf.prem_adj_pgm_id == PremAdjPgmID
                                                 orderby sf.mms_to_valn_nbr
                                                 select new SteppedFactorBE
                                                 {
                                                     STEPPED_FACTOR_ID = sf.stepped_fctr_id,
                                                     POLICY_ID = sf.coml_agmt_id,
                                                     MONTHS_TO_VAL = sf.mms_to_valn_nbr,
                                                     LDF_FACTOR = sf.los_dev_fctr_rt,
                                                     IBNR_FACTOR = sf.incur_but_not_rptd_fctr_rt,
                                                     ISACTIVE = sf.actv_ind,
                                                     PolicySymbol = ca.pol_sym_txt,
                                                     PolicyNumber = ca.pol_nbr_txt,
                                                     PolicyModulus = ca.pol_modulus_txt,
                                                     PolicyEffectiveDate = ca.pol_eff_dt,
                                                     PlanEndDate = ca.planned_end_date,
                                                     PREM_ADJ_PGM_ID = sf.prem_adj_pgm_id
                                                 }).Distinct();
            //result = result.Distinct();
            return result.ToList();
        }

        private IQueryable<SteppedFactorBE> BuildQuery()
        {
            IQueryable<SteppedFactorBE> result = from sf in this.Context.STEPPED_FCTRs
                                                 orderby sf.mms_to_valn_nbr
                                                 select new SteppedFactorBE
                                                 {
                                                     STEPPED_FACTOR_ID = sf.stepped_fctr_id,
                                                     POLICY_ID = sf.coml_agmt_id,
                                                     MONTHS_TO_VAL = sf.mms_to_valn_nbr,
                                                     LDF_FACTOR = sf.los_dev_fctr_rt,
                                                     IBNR_FACTOR = sf.incur_but_not_rptd_fctr_rt,
                                                     ISACTIVE = sf.actv_ind
                                                 };
            return result;
        }

        public bool deleteSteppedFactors(int PolicyID, int CustmrID, int PremAdjPgmID)
        {
            bool returnvalue;
            if (PolicyID > 0 && CustmrID > 0 && PremAdjPgmID > 0)
            {
                var dtl = from dtls in this.Context.STEPPED_FCTRs
                          where dtls.custmr_id == CustmrID && dtls.coml_agmt_id == PolicyID && dtls.prem_adj_pgm_id == PremAdjPgmID
                          select dtls;

                this.Context.STEPPED_FCTRs.DeleteAllOnSubmit(dtl);

                try
                {
                    this.Context.SubmitChanges();
                }
                catch
                {
                }
                returnvalue = true;
            }
            else
            {
                returnvalue = false;
            }

            return returnvalue;
        }

        public bool deleteSteppedFactors(int CustmrID, int PremAdjPgmID)
        {
            bool returnvalue;
            if (CustmrID > 0 && PremAdjPgmID > 0)
            {
                var dtl = from dtls in this.Context.STEPPED_FCTRs
                          where dtls.custmr_id == CustmrID && dtls.prem_adj_pgm_id == PremAdjPgmID
                          select dtls;

                this.Context.STEPPED_FCTRs.DeleteAllOnSubmit(dtl);

                try
                {
                    this.Context.SubmitChanges();
                }
                catch
                {
                }
                returnvalue = true;
            }
            else
            {
                returnvalue = false;
            }

            return returnvalue;
        }
    }
}
