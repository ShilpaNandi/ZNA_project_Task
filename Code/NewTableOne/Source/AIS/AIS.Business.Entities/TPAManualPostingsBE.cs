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
    public class TPAManualPostingsBE : BusinessEntity<THRD_PTY_ADMIN_MNL_INVC>
    {
        public int ThirdPartyAdminManualInvoiceID { get { return Entity.thrd_pty_admin_mnl_invc_id; } set { Entity.thrd_pty_admin_mnl_invc_id = value; } }
        public int CustomerID { get { return Entity.custmr_id; } set { Entity.custmr_id = value; } }
        public int? ThirdPartyAdminID { get { return Entity.thrd_pty_admin_id; } set { Entity.thrd_pty_admin_id = value; } }
        public int? BillingCycleID { get { return Entity.bil_cycl_id; } set { Entity.bil_cycl_id = value; } }
        public int? ThirdPartyAdminLossSrcID { get { return Entity.thrd_pty_admin_los_src_id; } set { Entity.thrd_pty_admin_los_src_id = value; } }
        public int? ThirdPartyAdminInvoiceTypID { get { return Entity.thrd_pty_admin_invc_typ_id; } set { Entity.thrd_pty_admin_invc_typ_id = value; } }
        public string InvoiceNumber { get { return Entity.invc_nbr_txt; } set { Entity.invc_nbr_txt = value; } }
        public DateTime? InvoiceDate { get { return Entity.invc_dt; } set { Entity.invc_dt = value; } }
        public DateTime? DueDate { get { return Entity.due_dt; } set { Entity.due_dt = value; } }
        public DateTime? EndDate { get { return Entity.end_dt; } set { Entity.end_dt = value; } }
        public DateTime? ValuationDate { get { return Entity.valn_dt; } set { Entity.valn_dt = value; } }
        public Decimal? InoiceAmt { get { return Entity.invc_amt; } set { Entity.invc_amt = value; } }
        public int? PolicyYearNumber { get { return Entity.pol_yy_nbr; } set { Entity.pol_yy_nbr = value; } }
        public bool? FinalizedIndicator { get { return Entity.fnl_ind; } set { Entity.fnl_ind = value; } }
        public string CommentText { get { return Entity.cmmnt_txt; } set { Entity.cmmnt_txt = value; } }
        public int? BusinessUnitOfficeID { get { return Entity.bsn_unt_ofc_id; } set { Entity.bsn_unt_ofc_id = value; } }
        public int? UpdatedUserID { get { return Entity.updt_user_id; } set { Entity.updt_user_id = value; } }
        public DateTime? UpdatedDate { get { return Entity.updt_dt; } set { Entity.updt_dt = value; } }
        public int CreatedUserID { get { return Entity.crte_user_id; } set { Entity.crte_user_id = value; } }
        public DateTime CreatedDate { get { return Entity.crte_dt; } set { Entity.crte_dt = value; } }
        public bool? Active { get { return Entity.actv_ind; } set { Entity.actv_ind = value; } }
        public bool? ReviseIndicator { get { return Entity.revise_ind; } set { Entity.revise_ind = value; } }
        public bool? VoidIndicator { get { return Entity.void_ind; } set { Entity.void_ind = value; } }
        public bool? CancelIndicator { get { return Entity.can_ind; } set { Entity.can_ind = value; } }
        public int? RelatedInvoiceID { get { return Entity.related_invoice_id; } set { Entity.related_invoice_id = value; } }
        public string ACC_NAME { get; set; }
        public int? ADJ_NUMBER { get; set; }
        public string TPA_NAME { get; set; }
        public string INVOICE_TYPE_TEXT { get; set; }
        public string BU_OFFICE_TEXT { get; set; }

    }
}
