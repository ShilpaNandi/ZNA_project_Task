using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class ApplWebPageAudtDA : DataAccessor<APLCTN_WEB_PAGE_AUDT, ApplWebPageAudtBE, AISDatabaseLINQDataContext>
    {

     
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
       public IList<ApplWebPageAudtBE> getAuditTraildata()
       {
           IList<ApplWebPageAudtBE> result = new List<ApplWebPageAudtBE>();

           if (this.Context == null)
               this.Initialize();

           /// Generate query to retrieve account information
           /// and project it into Account Business Entity
           IQueryable<ApplWebPageAudtBE> query =
           (from cdd in this.Context.APLCTN_WEB_PAGE_AUDTs
            select new ApplWebPageAudtBE()
            {
                APLCTN_WEB_PAGE_AUDT_ID=cdd.aplctn_web_page_audt_id,
               CUSTMR_ID  = cdd.custmr_id,
               PREM_ADJ_PGM_ID  = cdd.prem_adj_pgm_id,
                WEB_PAGE_CNTRL_TXT=cdd.web_page_cntrl_txt,
                WEB_PAGE_ID=cdd.web_page_id,
            });

           /// Force an enumeration so that the SQL is only
           /// executed in this method

           result = query.ToList();
           return result;
       }


    }
}
