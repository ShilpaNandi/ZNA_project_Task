using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;

namespace ZurichNA.AIS.Business.Logic
{
    public class Non_Sub_Audt_PremBS : BusinessServicesBase<NonSubjectAuditPremiumBE, Non_Sub_Audt_PremDA>
    {

        public NonSubjectAuditPremiumBE getsubPremAudRow(int nonsubPremAudID)
        {
            NonSubjectAuditPremiumBE SubBE = new NonSubjectAuditPremiumBE();
            SubBE = DA.Load(nonsubPremAudID);
            return SubBE;
        }
        public IList<NonSubjectAuditPremiumBE> getNonsubPremAudList(int commAgrAudID)
        {
            Non_Sub_Audt_PremDA nonSubDA = new Non_Sub_Audt_PremDA();
            IList<NonSubjectAuditPremiumBE> result = nonSubDA.getNonSubAudtPremList(commAgrAudID);
            return result;
        }
        public bool Update(NonSubjectAuditPremiumBE SubBE)
        {
            bool succeed = false;
            if (SubBE.N_Sub_Prem_Aud_ID > 0) //On Update
            {
                succeed = this.Save(SubBE);
            }
            else //On Insert
            {
                succeed = DA.Add(SubBE);
            }

            return succeed;

        }
        public IList<LookupBE> getNONSubjectPrem(int commAgrAudID, int NonSubID)
        {
            Non_Sub_Audt_PremDA NonSubDA = new Non_Sub_Audt_PremDA();
            IList<LookupBE> lkupBE = NonSubDA.getNONSUBJECT(commAgrAudID, NonSubID);
            return lkupBE;
        }

    }
}
