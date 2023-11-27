using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.LSP.Framework.DataAccess;

namespace ZurichNA.AIS.Business.Logic
{
    public class PaidLossBillingBS : BusinessServicesBase<PaidLossBillingBE, PaidLossBillingDA>
    {

        /// <summary>
        /// Retrieves PaidLossBilling data 
        /// </summary>
        /// <returns>List of PaidLossBillingBE</returns>
        public IList<PaidLossBillingBE> getPaidLossBillingData(int custmrID, int prgID,int PrmAdjID,int PrmAdjPrgID)
        {
            PaidLossBillingDA paidlossbil = new PaidLossBillingDA();
            IList<PaidLossBillingBE> result = paidlossbil.getPaidLossBillingData(custmrID, prgID,PrmAdjID, PrmAdjPrgID);
            return result;
        }
        /// <summary>
        /// Retrieves PaidLossBilling data based on Prem adj PaidLossBilling ID
        /// </summary>
        /// <returns>List of PaidLossBillingBE</returns>
        public PaidLossBillingBE getPaidLossBillingDataRow(int prmadjplbID)
        {
            PaidLossBillingBE plbBE = new PaidLossBillingBE();
            plbBE = DA.Load(prmadjplbID);
            return plbBE;
        }
        /// <summary>
        /// Update PaidLossBillingData 
        /// </summary>
        /// <returns>Result</returns>
        public bool Update(PaidLossBillingBE plbBE)
        {
            bool succeed = false;
            try
            {
                if (plbBE.PREM_ADJ_PAID_LOS_BIL_ID > 0) //On Update
                {
                    succeed = this.Save(plbBE);

                }
             
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                return succeed;
            }
            return succeed;

        }
    }
}
