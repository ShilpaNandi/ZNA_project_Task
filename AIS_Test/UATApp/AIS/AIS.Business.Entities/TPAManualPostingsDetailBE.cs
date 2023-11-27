using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.Linq;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.AIS.Business.Entities
{
    public class TPAManualPostingsDetailBE : BusinessEntity<THRD_PTY_ADMIN_MNL_INVC_DTL>
    {
        public int ThirdPartyAdminManualInvoiceDtlID { get { return Entity.thrd_pty_admin_mnl_invc_dtl_id; } set { Entity.thrd_pty_admin_mnl_invc_dtl_id = value; } }
        public int ThirdPartyAdminManualInvoiceID { get { return Entity.thrd_pty_admin_mnl_invc_id; } set { Entity.thrd_pty_admin_mnl_invc_id = value; } }
        public int CustomerID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int PostingTrnsTypID { get { return Entity.post_trns_typ_id; } set { Entity.post_trns_typ_id = value; } }
        public string PostTransactionText { get; set; }
        public string AriesMainNbr { get { return Entity.aries_main_nbr_txt; } set { Entity.aries_main_nbr_txt = value; } }
        public string AriesSubNbr { get { return Entity.aries_sub_nbr_txt; } set { Entity.aries_sub_nbr_txt = value; } }
        public DateTime? DueDate { get { return Entity.due_dt; } set { Entity.due_dt = value; } }
        public string CompanyCode { get { return Entity.comp_cd_txt; } set { Entity.comp_cd_txt = value; } }
        public string POLICY { get; set; }
        public string PolicySymbolText { get { return Entity.pol_sym_txt; } set { Entity.pol_sym_txt = value; } }
        public string PolicyNbrText { get { return Entity.pol_nbr_txt; } set { Entity.pol_nbr_txt = value; } }
        public string PolicyModText { get { return Entity.pol_modulus_txt; } set { Entity.pol_modulus_txt = value; } }
        public Decimal? ThirdPartyAdminAmt { get { return Entity.thrd_pty_admin_amt; } set { Entity.thrd_pty_admin_amt = value; } }
        public int? UpdatedUserID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UpdatedDate { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CreatedUserID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CreatedDate { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public DateTime? ExpiryDate { get { return Entity.expi_dt; } set { Entity.expi_dt = value; } }
        public DateTime? EffectiveDate { get { return Entity.eff_dt; } set { Entity.eff_dt = value; } }
    }
}