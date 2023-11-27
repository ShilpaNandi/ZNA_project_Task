using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;

namespace ZurichNA.AIS.Business.Logic
{
    public class Sub_Audt_PremBS : BusinessServicesBase<SubjectAuditPremiumBE, Sub_Audt_PremDA>
    {
        public bool Update(SubjectAuditPremiumBE SubBE)
        {
            bool succeed = false;
            if (SubBE.Sub_Prem_Aud_ID > 0) //On Update
            {
              
                succeed = this.Save(SubBE);
            }
            else //On Insert
            {
             
                succeed = DA.Add(SubBE);
            }

            return succeed;

        }

        public SubjectAuditPremiumBE getsubPremAudRow(int subPremAudID)
        {
            SubjectAuditPremiumBE SubBE = new SubjectAuditPremiumBE();
            SubBE = DA.Load(subPremAudID);
            return SubBE;
        }
        /// <summary>
        /// Function to retrive all state Names from Lookup Table which are not assiged to Coml_Agr_ID
        /// </summary>
        /// <param name="commAgrAudID"></param>
        /// <param name="stateID"></param>
        /// <returns></returns>
        public IList<LookupBE> getStateNames(int commAgrAudID, int stateID)
        {
            Sub_Audt_PremDA SubDA = new Sub_Audt_PremDA();
            IList<LookupBE> lkupBE = SubDA.getStateNames(commAgrAudID, stateID);
            return lkupBE;
        }
        public IList<SubjectAuditPremiumBE> getsubPremAudList(int commAgrAudID)
        {
            Sub_Audt_PremDA SubDA = new Sub_Audt_PremDA();
            IList<SubjectAuditPremiumBE> result = SubDA.getSubAudtPremList(commAgrAudID);
            return result;
        }
    }
}
