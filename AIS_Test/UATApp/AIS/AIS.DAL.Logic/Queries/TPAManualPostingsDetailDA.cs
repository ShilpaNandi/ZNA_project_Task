using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.DataAccess;

using ZurichNA.AIS.DAL.Logic;


namespace ZurichNA.AIS.DAL.Logic
{
    public class TPAManualPostingsDetailDA : DataAccessor<THRD_PTY_ADMIN_MNL_INVC_DTL, TPAManualPostingsDetailBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Function to get the List of Third Party Administrator Manual Invoice Details
        /// </summary>
        /// <returns>Ilist TPAManualPostingsDetailBE</returns>
        public IList<TPAManualPostingsDetailBE> getTPAPostDltsList(int TPAInvoiceID)
        {
            IList<TPAManualPostingsDetailBE> result = new List<TPAManualPostingsDetailBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Third Party Administrator Manual Invoice Detail information
            /// and project it into Third Party Administrator Manual Invoice Detail Business Entity
            IQueryable<TPAManualPostingsDetailBE> query =
            (from cdd in this.Context.THRD_PTY_ADMIN_MNL_INVC_DTLs
             where cdd.thrd_pty_admin_mnl_invc_id == TPAInvoiceID
             select new TPAManualPostingsDetailBE()
             {
                 ThirdPartyAdminManualInvoiceDtlID = cdd.thrd_pty_admin_mnl_invc_dtl_id,
                 ThirdPartyAdminManualInvoiceID = cdd.thrd_pty_admin_mnl_invc_id,
                 PostingTrnsTypID = cdd.post_trns_typ_id,
                 DueDate = cdd.due_dt,
                 AriesMainNbr = cdd.aries_main_nbr_txt,
                 AriesSubNbr = cdd.aries_sub_nbr_txt,
                 CompanyCode = cdd.comp_cd_txt,
                 PolicySymbolText = cdd.pol_sym_txt,
                 PolicyNbrText = cdd.pol_nbr_txt,
                 PolicyModText = cdd.pol_modulus_txt,
                 EffectiveDate = cdd.eff_dt,
                 ExpiryDate = cdd.expi_dt,
                 ThirdPartyAdminAmt=cdd.thrd_pty_admin_amt,
                 PostTransactionText=cdd.POST_TRNS_TYP.trns_nm_txt,
                 CreatedDate=cdd.crte_dt,
                 CreatedUserID=cdd.crte_user_id,
                 CustomerID=cdd.custmr_id,
                 UpdatedDate=cdd.updt_dt,
                 UpdatedUserID=cdd.updt_user_id,

             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }

    }
}
