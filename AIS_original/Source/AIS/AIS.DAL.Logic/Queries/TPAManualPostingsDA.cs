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
    public class TPAManualPostingsDA : DataAccessor<THRD_PTY_ADMIN_MNL_INVC, TPAManualPostingsBE, AISDatabaseLINQDataContext>
    {
        /// <summary>
        /// Function to get the List of Third Party Administrator Manual Invoice 
        /// </summary>
        /// <returns>Ilist TPAManualPostingsBE</returns>
        public IList<TPAManualPostingsBE> getTPAPostList(int CustomerID)
        {
            IList<TPAManualPostingsBE> result = new List<TPAManualPostingsBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Third Party Administrator Manual Invoice  information
            /// and project it into Third Party Administrator Manual Invoice  Business Entity
            IQueryable<TPAManualPostingsBE> query =
            (from cdd in this.Context.THRD_PTY_ADMIN_MNL_INVCs
             orderby cdd.thrd_pty_admin_mnl_invc_id descending
             where cdd.custmr_id == CustomerID 
             //&& cdd.invc_nbr_txt != null
             select new TPAManualPostingsBE()
             {
                 ThirdPartyAdminManualInvoiceID = cdd.thrd_pty_admin_mnl_invc_id,
                 CreatedDate=cdd.crte_dt,
                 CreatedUserID=cdd.crte_user_id,
                 UpdatedDate=cdd.updt_dt,
                 CustomerID = cdd.custmr_id,
                 ThirdPartyAdminLossSrcID = cdd.thrd_pty_admin_los_src_id,
                 ThirdPartyAdminInvoiceTypID = cdd.thrd_pty_admin_invc_typ_id,
                 ThirdPartyAdminID = cdd.thrd_pty_admin_id,
                 BusinessUnitOfficeID = cdd.bsn_unt_ofc_id,
                 BillingCycleID = cdd.bil_cycl_id,
                 InvoiceNumber = cdd.invc_nbr_txt,
                 InvoiceDate = cdd.invc_dt,
                 ValuationDate = cdd.valn_dt,
                 InoiceAmt = cdd.invc_amt,
                 PolicyYearNumber = cdd.pol_yy_nbr,
                 FinalizedIndicator = cdd.fnl_ind,
                 CommentText = cdd.cmmnt_txt,
                 Active = cdd.actv_ind,
                 DueDate = cdd.due_dt,
                 EndDate = cdd.end_dt,
                 ReviseIndicator=cdd.revise_ind,
                 VoidIndicator=cdd.void_ind,
                 CancelIndicator=cdd.can_ind,
                 RelatedInvoiceID=cdd.related_invoice_id,
             });
            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
        /// <summary>
        /// Function to get the Search results from List of Third Party Administrator Manual Invoice
        /// </summary>
        /// <returns>TPAManualPostingsBE</returns>
        public IList<TPAManualPostingsBE> getTPAPostSearchResultList(int customerID, int? tpaId, string invoiceNumber, int buOfficeId, int invoiceType, string valnDt, string fromDate, string toDate)
        {
            IList<TPAManualPostingsBE> result = new List<TPAManualPostingsBE>();

            if (this.Context == null)
                this.Initialize();

            /// Generate query to retrieve Third Party Administrator Manual Invoice  information
            /// and project it into Third Party Administrator Manual Invoice  Business Entity
            IQueryable<TPAManualPostingsBE> query =
            (from cdd in this.Context.THRD_PTY_ADMIN_MNL_INVCs
             join cus in this.Context.CUSTMRs
             on cdd.custmr_id equals cus.custmr_id
            // join adj in this.Context.PREM_ADJs
            // on cdd.custmr_id equals  adj.custmr_id
             //where (cdd.invc_nbr_txt == adj.drft_invc_nbr_txt) || (cdd.invc_nbr_txt == adj.fnl_invc_nbr_txt)
             orderby cus.full_nm ascending, cdd.valn_dt descending, cdd.invc_nbr_txt ascending
             select new TPAManualPostingsBE()
             {
                 ThirdPartyAdminManualInvoiceID = cdd.thrd_pty_admin_mnl_invc_id,
                 CustomerID = cdd.custmr_id,
                 ThirdPartyAdminLossSrcID = cdd.thrd_pty_admin_los_src_id,
                 ThirdPartyAdminInvoiceTypID = cdd.thrd_pty_admin_invc_typ_id,
                 ThirdPartyAdminID = cdd.thrd_pty_admin_id,
                 BusinessUnitOfficeID = cdd.bsn_unt_ofc_id,
                 BillingCycleID = cdd.bil_cycl_id,
                 InvoiceNumber = cdd.invc_nbr_txt,
                 InvoiceDate = cdd.invc_dt,
                 ValuationDate = cdd.valn_dt,
                 InoiceAmt = cdd.invc_amt,
                 PolicyYearNumber = cdd.pol_yy_nbr,
                 FinalizedIndicator = cdd.fnl_ind,
                 CommentText = cdd.cmmnt_txt,
                 Active = cdd.actv_ind,
                 DueDate = cdd.due_dt,
                 EndDate = cdd.end_dt,
                 ACC_NAME = cus.full_nm,
                 //ADJ_NUMBER = cdd.T.prem_adj_id,
                 TPA_NAME = (from extrnl_org in this.Context.EXTRNL_ORGs
                             where extrnl_org.extrnl_org_id == cdd.thrd_pty_admin_id
                             select extrnl_org.full_name).First( ).ToString(),
                 BU_OFFICE_TEXT = (from org in this.Context.INT_ORGs
                                   where org.int_org_id == cdd.bsn_unt_ofc_id
                                   select (org.bsn_unt_cd + "/" + org.city_nm)).First().ToString(),
                 INVOICE_TYPE_TEXT = (from lkup in this.Context.LKUPs
                                      where lkup.lkup_id == cdd.thrd_pty_admin_invc_typ_id
                                      select lkup.lkup_txt).First().ToString(),
                 ReviseIndicator= cdd.revise_ind,
                 CancelIndicator=cdd.can_ind,
                 VoidIndicator=cdd.void_ind,
                 UpdatedDate=cdd.updt_dt,


             });
            if (customerID > 0)
            {
                query = query.Where(tpa => tpa.CustomerID == customerID);
            }
            if (tpaId > 0)
            {
                query = query.Where(tpa => tpa.ThirdPartyAdminID == tpaId);
            }
            if (invoiceNumber != string.Empty && invoiceNumber!= null )
            {
                query = query.Where(tpa => tpa.InvoiceNumber == invoiceNumber);
            }
            if (buOfficeId > 0)
            {
                query = query.Where(tpa => tpa.BusinessUnitOfficeID == buOfficeId);
            }
            if (invoiceType > 0)
            {
                query = query.Where(tpa => tpa.ThirdPartyAdminInvoiceTypID == invoiceType);
            }
            if (valnDt != string.Empty)
            {
                query = query.Where(tpa => tpa.ValuationDate == Convert.ToDateTime(valnDt));
            }
            if (fromDate != string.Empty && toDate != string.Empty)
            {
                query = query.Where(tpa => (tpa.InvoiceDate >= Convert.ToDateTime(fromDate) && tpa.InvoiceDate <= Convert.ToDateTime(toDate)));
            }
            else if (fromDate != string.Empty)
            {
                query = query.Where(tpa => (tpa.InvoiceDate >= Convert.ToDateTime(fromDate)));
            }
            else if (toDate != string.Empty)
            {
                query = query.Where(tpa => (tpa.InvoiceDate <= Convert.ToDateTime(toDate)));
            }

            if (query.Count() > 0)
                result = query.ToList();
            return result;
        }
        /// <summary>
        /// Function to call TPAAriesTransmital Procedure
        /// </summary>
        /// <param name="TPAID"></param>
        /// <param name="Custmr_id"></param>
        /// <param name="IND"></param>
        public void TPATransmittalToARiES(int TPAID, int? Custmr_id, int? IND)
        {
            string ErroMsg;
            ErroMsg = string.Empty;


            if (this.Context == null)
                this.Initialize();
            this.Context.ModAIS_TPATransmittalToARiES(TPAID, Custmr_id, ref ErroMsg, IND);
        }
    }
}
