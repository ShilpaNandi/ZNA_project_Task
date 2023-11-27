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
    }
}
