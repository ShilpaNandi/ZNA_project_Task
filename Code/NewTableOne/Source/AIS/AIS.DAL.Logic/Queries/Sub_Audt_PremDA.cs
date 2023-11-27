using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class Sub_Audt_PremDA : DataAccessor<SUBJ_PREM_AUDT, SubjectAuditPremiumBE, AISDatabaseLINQDataContext>
    {
        public IList<SubjectAuditPremiumBE> getSubAudtPremList(int commAgrAudID)
        {
            IList<SubjectAuditPremiumBE> result = new List<SubjectAuditPremiumBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<SubjectAuditPremiumBE> query =
            (from cdd in this.Context.SUBJ_PREM_AUDTs
             join lk in this.Context.LKUPs
             on cdd.st_id equals lk.lkup_id
             where cdd.coml_agmt_audt_id == commAgrAudID
             orderby lk.lkup_txt
             select new SubjectAuditPremiumBE()
             {
                 Sub_Prem_Aud_ID = cdd.subj_prem_audt_id,
                 Coml_Agmt_Audt_ID = cdd.coml_agmt_audt_id,
                 Coml_Agmt_ID = cdd.coml_agmt_id,
                 Prem_Adj_Pgm_ID = cdd.prem_adj_pgm_id,
                 Custmr_ID = cdd.custmr_id,
                 STATE = lk.lkup_txt,
                 StateID = cdd.st_id,
                 Prem_Amt = cdd.prem_amt,
                 Active = cdd.actv_ind,
                 UpdatedDate=cdd.updt_dt,
             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
        /// <summary>
        /// Function to retrive all state Names from Lookup Table which are not assiged to Coml_Agr_ID
        /// </summary>
        /// <param name="commAgrAudID"></param>
        /// <returns></returns>
        public IList<LookupBE> getStateNames(int commAgrAudID, int stateID)
        {
            IList<LookupBE> result = new List<LookupBE>();
            if (this.Context == null)
                this.Initialize();
            var Subject = from sub in Context.SUBJ_PREM_AUDTs
                          where sub.coml_agmt_audt_id == commAgrAudID
                          && sub.st_id != stateID
                          select sub.st_id;
            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<LookupBE> query =
            (from lk in this.Context.LKUPs
             where lk.LKUP_TYP.lkup_typ_nm_txt.ToUpper() == "STATE"
             && lk.lkup_txt.ToUpper() != "ALL OTHER"
             && !Subject.Contains(lk.lkup_id)
             && lk.actv_ind==true
             orderby lk.lkup_txt
             select new LookupBE()
             {
                LookUpID = lk.lkup_id,
                LookUpName=lk.lkup_txt,
             });
            if (query.Count() > 0)
                result = query.ToList();
            LookupBE lkSelect =new LookupBE();
            lkSelect.LookUpName = "(Select)";
            lkSelect.LookUpID = 0;
            result.Insert(0, lkSelect);
            return result;
        }

    }
}
