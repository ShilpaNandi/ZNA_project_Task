using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class ApplicationStatusLogDA : DataAccessor<APLCTN_STS_LOG, ApplicationStatusLogBE, AISDatabaseLINQDataContext>
    {
        public IList<ApplicationStatusLogBE> getLogList(int AcctNumber, string InterfaceType, string fromDate, string toDate)
        {
            IList<ApplicationStatusLogBE> result = new List<ApplicationStatusLogBE>();
            IQueryable<ApplicationStatusLogBE> query =
                (from per in this.Context.APLCTN_STS_LOGs
                 select new ApplicationStatusLogBE
                 {
                     SRC_TXT = per.src_txt,
                     SEV_CD = per.sev_cd,
                     SHORT_DESC_TXT = per.shrt_desc_txt,
                     FULL_DESC_TXT = per.full_desc_txt,
                     CREATE_DATE = per.crte_dt,
                     CUSTMR_ID = per.custmr_id
                 });

            if (AcctNumber > 0)
            {
                query = query.Where(tpa => tpa.CUSTMR_ID == AcctNumber);
            }

            if (InterfaceType != string.Empty && InterfaceType != null)
            {
                query = query.Where(tpa => tpa.SRC_TXT == InterfaceType);
            }
            if (fromDate != string.Empty && toDate != string.Empty)
            {
                query = query.Where(tpa => (tpa.CREATE_DATE>=Convert.ToDateTime(fromDate)
                                        && tpa.CREATE_DATE<=Convert.ToDateTime(toDate).AddDays(1)));
            }
            query = query.OrderBy(cdd => cdd.CREATE_DATE);
            query = query.OrderBy(cdd => cdd.SRC_TXT);

            result = query.ToList();
            return result;
        }

    }
}
