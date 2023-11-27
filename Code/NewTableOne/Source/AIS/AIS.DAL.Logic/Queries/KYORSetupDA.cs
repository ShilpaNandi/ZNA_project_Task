using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;

namespace ZurichNA.AIS.DAL.Logic
{
    public class KYORSetupDA : DataAccessor<KY_OR_SETUP, KYORSetupBE, AISDatabaseLINQDataContext>
    {
        public IList<KYORSetupBE> SelectData()
        {
            IList<KYORSetupBE> result = new List<KYORSetupBE>();

            IQueryable<KYORSetupBE> query =
                (from kos in this.Context.KY_OR_SETUPs
                 orderby kos.eff_dt descending
                 select new KYORSetupBE
                 {
                     KY_OR_SETUP_ID = kos.ky_or_setup_id,
                     EFF_DT = kos.eff_dt,
                     KY_FCTR_RT = kos.ky_fctr_rt,
                     OR_FCTR_RT = kos.or_fctr_rt,
                     UPDT_USER_ID = kos.updt_user_id,
                     UPDT_DT = kos.updt_dt,
                     CRTE_USER_ID = kos.crte_user_id,
                     CRTE_DT = kos.crte_dt

                 });
            result = query.ToList();

            return result;
        }
    }
}
