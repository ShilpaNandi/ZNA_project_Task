using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.DAL.Logic
{
    public class Non_Sub_Audt_PremDA : DataAccessor<NON_SUBJ_PREM_AUDT, NonSubjectAuditPremiumBE, AISDatabaseLINQDataContext>
    {
        public IList<NonSubjectAuditPremiumBE> getNonSubAudtPremList(int commAgrAudID)
        {
            IList<NonSubjectAuditPremiumBE> result = new List<NonSubjectAuditPremiumBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<NonSubjectAuditPremiumBE> query =
            (from cdd in this.Context.NON_SUBJ_PREM_AUDTs
             join lk in this.Context.LKUPs
             on cdd.nsa_typ_id equals lk.lkup_id
             where cdd.coml_agmt_audt_id == commAgrAudID
             orderby lk.lkup_txt
             select new NonSubjectAuditPremiumBE()
             {
                 N_Sub_Prem_Aud_ID = cdd.non_subj_prem_audt_id,
                 Coml_Agmt_Audt_ID = cdd.coml_agmt_audt_id,
                 Coml_Agmt_ID = cdd.coml_agmt_id,
                 Prem_Adj_Pgm_ID = cdd.prem_adj_pgm_id,
                 Custmr_ID = cdd.custmr_id,
                 NSATYPE = lk.lkup_txt,
                 Nsa_Typ_ID = cdd.nsa_typ_id,
                 Non_Subj_Audt_Prem_Amt = cdd.non_subj_audt_prem_amt,
                 Active = cdd.actv_ind,
                 UpdatedDate=cdd.updt_dt,
             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
        public IList<LookupBE> getNONSUBJECT(int commAgrAudID, int NonSubjectID)
        {
            IList<LookupBE> result = new List<LookupBE>();
            if (this.Context == null)
                this.Initialize();
            var NonSubject = from non in Context.NON_SUBJ_PREM_AUDTs
                             where non.coml_agmt_audt_id == commAgrAudID
                             && non.nsa_typ_id != NonSubjectID
                             select non.nsa_typ_id;
            /// Generate query to retrieve account information
            /// and project it into Account Business Entity
            IQueryable<LookupBE> query =
            (from lk in this.Context.LKUPs
             where lk.LKUP_TYP.lkup_typ_nm_txt.ToUpper() == "NON-SUBJECT AUDIT PREMIUM"
             && !NonSubject.Contains(lk.lkup_id)
             && lk.actv_ind == true
             orderby lk.lkup_txt
             select new LookupBE()
             {
                 LookUpID = lk.lkup_id,
                 LookUpName = lk.lkup_txt,
             });
            if (query.Count() > 0)
                result = query.ToList();
            LookupBE lkSelect = new LookupBE();
            lkSelect.LookUpName = "(Select)";
            lkSelect.LookUpID = 0;
            result.Insert(0, lkSelect);
            return result;
        }
    }
}
