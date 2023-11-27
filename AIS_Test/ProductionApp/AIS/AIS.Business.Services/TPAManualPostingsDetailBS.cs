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
    /// <summary>
    /// This clas is used to interact with Third Party Administrator Manual Invoice Details Table
    /// </summary>
    public class TPAManualPostingsDetailBS : BusinessServicesBase<TPAManualPostingsDetailBE, TPAManualPostingsDetailDA>
    {
        /// <summary>
        /// Function to get the Single row of Third Party Administrator Manual Invoice Details Table
        /// </summary>
        /// <param name="TpaPostDtlID"></param>
        /// <returns>TPAManualPostingsDetailBE</returns>
        public TPAManualPostingsDetailBE getTPAPostDtlRow(int TpaPostDtlID)
        {
            TPAManualPostingsDetailBE TPSdtl = new TPAManualPostingsDetailBE();
            TPSdtl = DA.Load(TpaPostDtlID);
            return TPSdtl;
        }
        /// <summary>
        /// Function to get the List of Third Party Administrator Manual Invoice Details Table
        /// </summary>
        /// <returns>TPAManualPostingsDetailBE List</returns>
        public IList<TPAManualPostingsDetailBE> getTPAPostDltsList(int TPAInvoiceID)
        {
            IList<TPAManualPostingsDetailBE> list = new List<TPAManualPostingsDetailBE>();
            TPAManualPostingsDetailDA TPAdtl = new TPAManualPostingsDetailDA();

            try
            {
                list = TPAdtl.getTPAPostDltsList(TPAInvoiceID);
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].strInvoiceNo != null && list[i].strInvoiceNo !="")
                    {
                        if (list[i].strInvoiceNo.Contains("RMV"))
                        {
                            list[i].ThirdPartyAdminAmt = -(list[i].ThirdPartyAdminAmt);
                        }
                    }
                }

            }
             
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                throw myException;
            }
            return list;
        }
        /// <summary>
        /// delete the information in Third Party Administrator Manual Invoice Details Table
        /// </summary>
        /// <param name="TPAManualPostingsDetailBE">TPADtlBE</param>
        /// <returns>Bool (True/false) i.e., DD or not </returns>
        public bool DeleteTPAdtls(TPAManualPostingsDetailBE TPADtlBE)
        {
            bool succeed = false;
            return succeed = this.Delete(TPADtlBE);
        }
        /// <summary>
        /// update the information in Third Party Administrator Manual Invoice Details Table
        /// </summary>
        /// <param name="TPAManualPostingsDetailBE">TPADtlBE</param>
        /// <returns>Bool (True/false) i.e., updated or not </returns>
        public bool Update(TPAManualPostingsDetailBE TPADtlBE)
        {
            bool succeed = false;
            if (TPADtlBE.ThirdPartyAdminManualInvoiceDtlID > 0) //On Update
            {
                succeed = this.Save(TPADtlBE);
            }
            else //On Insert
            {
                succeed = DA.Add(TPADtlBE);
            }
            return succeed;
        }
    }
}
