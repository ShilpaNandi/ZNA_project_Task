using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.Business.Logic;
using System.Collections.Generic;
using ZurichNA.AIS.DAL.LINQ;

public partial class AppMgmt_TPAPostingMgmt : AISBasePage
{
    System.Data.Common.DbTransaction trans = null;
    AISDatabaseLINQDataContext objDC = new AISDatabaseLINQDataContext(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());

    private TPAManualPostingsBS TPAbs;
    private TPAManualPostingsDetailBS TPSdtlbs;
    protected AISBusinessTransaction TPATransactionWrapper
    {
        get
        {
            //if ((AISBusinessTransaction)Session["TPATransaction"] == null)
            //    Session["TPATransaction"] = new AISBusinessTransaction();
            //return (AISBusinessTransaction)Session["TPATransaction"];
            if ((AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("TPATransaction") == null)
                SaveObjectToSessionUsingWindowName("TPATransaction", new AISBusinessTransaction());
            return (AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("TPATransaction");
        }
        set
        {
            //Session["TPATransaction"] = value;
            SaveObjectToSessionUsingWindowName("TPATransaction", value);
        }
    }
    /// <summary>
    /// a property for Commercial Agreement Audit Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>Coml_Agmt_AuDtBS</returns>
    private TPAManualPostingsBS TPAService
    {
        get
        {
            if (TPAbs == null)
            {
                TPAbs = new TPAManualPostingsBS();
                //TPAManualPostingsBS.AppTransactionWrapper = TPATransactionWrapper;
            }
            return TPAbs;
        }
    }
    /// <summary>
    /// a property for Subject Audit Premium Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>Sub_Audt_PremBS</returns>
    private TPAManualPostingsDetailBS TPAdtlService
    {
        get
        {
            if (TPSdtlbs == null)
            {
                TPSdtlbs = new TPAManualPostingsDetailBS();
                //TPSdtlbs.AppTransactionWrapper = TPATransactionWrapper;
            }
            return TPSdtlbs;
        }
    }
    /// <summary>
    /// a property for Commercial Agreement Audit Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>Coml_Agmt_AuDtBE</returns>
    private TPAManualPostingsBE TPAPostBE
    {
        //get { return (TPAManualPostingsBE)Session["TPAMANUALPOSTINGSBE"]; }
        //set { Session["TPAMANUALPOSTINGSBE"] = value; }
        get { return (TPAManualPostingsBE)RetrieveObjectFromSessionUsingWindowName("TPAMANUALPOSTINGSBE"); }
        set { SaveObjectToSessionUsingWindowName("TPAMANUALPOSTINGSBE", value); }
    }
    private IList<TPAManualPostingsBE> TPAPostBEList
    {
        //get { return (IList<TPAManualPostingsBE>)Session["TPAPostBEList"]; }
        //set { Session["TPAPostBEList"] = value; }
        get { return (IList<TPAManualPostingsBE>)RetrieveObjectFromSessionUsingWindowName("TPAPostBEList"); }
        set { SaveObjectToSessionUsingWindowName("TPAPostBEList", value); }
    }
    private TPAManualPostingsDetailBE TPAPostDtlBE
    {
        //get { return (TPAManualPostingsDetailBE)Session["TPAMANUALPOSTINGSDETAILBE"]; }
        //set { Session["TPAMANUALPOSTINGSDETAILBE"] = value; }
        get { return (TPAManualPostingsDetailBE)RetrieveObjectFromSessionUsingWindowName("TPAMANUALPOSTINGSDETAILBE"); }
        set { SaveObjectToSessionUsingWindowName("TPAMANUALPOSTINGSDETAILBE", value); }
    }
    private string SortBy
    {
        //get { return (string)Session["SORTBY"]; }
        //set { Session["SORTBY"] = value; }
        get { return (string)RetrieveObjectFromSessionUsingWindowName("SORTBY"); }
        set { SaveObjectToSessionUsingWindowName("SORTBY", value); }
    }
    private string SortDir
    {
        //get { return (string)Session["SORTDIR"]; }
        //set { Session["SORTDIR"] = value; }
        get { return (string)RetrieveObjectFromSessionUsingWindowName("SORTDIR"); }
        set { SaveObjectToSessionUsingWindowName("SORTDIR", value); }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.Page.Header.Title = "TPA/MANUAL Postings Management";
        if (!IsPostBack)
        {
            TPAPostBE = new TPAManualPostingsBE();
            TPAPostDtlBE = new TPAManualPostingsDetailBE();
            ddlTPAName.DataSource = (new BrokerBS()).GetBrokerData().Where(br => br.CONTACT_TYPE_ID == 237);
            ddlTPAName.DataTextField = "FULL_NAME";
            ddlTPAName.DataValueField = "EXTRNL_ORG_ID";
            ddlTPAName.DataBind();
            ListItem li = new ListItem("(Select)", "0");
            ddlTPAName.Items.Insert(0, li);

            ///additional functionality
            if ((Request.QueryString.Count > 0) && !(Request.QueryString.Count == 1 && Request.QueryString["wID"] != null))
            {
                if (AISMasterEntities != null)
                //ddlAcctlist.selectedAccountName = Request.QueryString["AcctNm"].ToString();
                ddlAcctlist.selectedAccountName = AISMasterEntities.AccountName;
                ddlAcctlist.selectedAccountNo = int.Parse(Request.QueryString["AcctNo"].ToString());

                lstTpa.Visible = true;
                //IList<TPAManualPostingsBE> tpaList = new List<TPAManualPostingsBE>();
                TPAPostBEList = new TPAManualPostingsBS().getTPAPostSearchResultList(int.Parse(Request.QueryString["AcctNo"].ToString()), 0, string.Empty, 0, 0, string.Empty, string.Empty, string.Empty);
                this.lstTpa.DataSource = TPAPostBEList;
                lstTpa.DataBind();
                if (TPAPostBEList.Count <= 0)
                {

                    string strMessage = "No Record(s) found...!";
                    ShowError(strMessage);

                }
            }

        }

        //ArrayList list = new ArrayList();
        //list.Add(btnVoid);
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        //ddlAccountName.SelectedIndex = 0;
        DropDownList ddlAccnts = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        ddlAccnts.SelectedIndex = -1;
        ddlBuOffice.SelectedIndex = 0;
        ddlInvType.SelectedIndex = 0;
        ddlTPAName.SelectedIndex = 0;
        txtInvNum.Text = string.Empty;
        txtFromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
        txtValnDate.Text = string.Empty;
        lstTpa.Items.Clear();
        lstTpa.Visible = false;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        /// TPA Detials Search Method
        Search();
    }
    /// <summary>
    /// TPA Detials Search Method
    /// </summary>
    public void Search()
    {
        lstTpa.Visible = true;
        //int accountId = Convert.ToInt32(ddlAccountName.SelectedValue);
        DropDownList ddlAccnts = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        if (ddlAccnts.SelectedValue == "")
        {
            ShowError("Please select Account");
            return;
        }
        int accountId = Convert.ToInt32(ddlAccnts.SelectedValue);
        int tpaId = Convert.ToInt32(ddlTPAName.SelectedValue);
        int buOfficeId = Convert.ToInt32(ddlBuOffice.SelectedValue);
        int invTypeId = Convert.ToInt32(ddlInvType.SelectedValue);
        string invNumber = txtInvNum.Text;
        string valnDate = txtValnDate.Text;
        string fromDate = txtFromDate.Text;
        string toDate = txtToDate.Text;
        if (accountId == 0 && tpaId == 0 && invTypeId == 0 &&
            invNumber == string.Empty && buOfficeId == 0 && valnDate == string.Empty &&
            fromDate == string.Empty && toDate == string.Empty)
        {
            string strMessage = "Please enter data into at least one of the search criteria prior to click Search button";
            ShowError(strMessage);
            return;
        }

        else
        {
            //IList<TPAManualPostingsBE> tpaList = new List<TPAManualPostingsBE>();
            TPAPostBEList = new TPAManualPostingsBS().getTPAPostSearchResultList(accountId, tpaId, invNumber, buOfficeId, invTypeId, valnDate, fromDate, toDate);
            this.lstTpa.DataSource = TPAPostBEList;
            lstTpa.DataBind();
            if (TPAPostBEList.Count <= 0)
            {

                string strMessage = "No Record(s) found...!";
                ShowError(strMessage);

            }
        }

    }
    protected void btnvoidlink_click(object sender, EventArgs e)
    {
        ViewState["VoidIndex"] = int.Parse(((LinkButton)sender).CommandArgument);
        programmaticModalPopup.Show();
    }
    protected void btnReviselink_click(object sender, EventArgs e)
    {
        ViewState["ReviseIndex"] = int.Parse(((LinkButton)sender).CommandArgument);
        programmaticModalPopupRevise.Show();
    }
    protected void lstTpa_Sorting(object sender, ListViewSortEventArgs e)
    {
        Image imgValnDate = (Image)lstTpa.FindControl("imgValnDate");
        Image imgInvNum = (Image)lstTpa.FindControl("imgInvNum");
        Image imgInvType = (Image)lstTpa.FindControl("imgInvType");
        Image imgBuOffice = (Image)lstTpa.FindControl("imgBuOffice");
        Image imgInvDate = (Image)lstTpa.FindControl("imgInvDate");


        Image img = new Image();
        switch (e.SortExpression)
        {
            case "VALUATIONDATE":
                SortBy = "VALUATIONDATE";
                imgValnDate.Visible = true;
                img = imgValnDate;
                break;
            case "INVOICENUMBER":
                SortBy = "INVOICENUMBER";
                imgInvNum.Visible = true;
                img = imgInvNum;
                break;
            case "INVOICETYPE":
                SortBy = "INVOICETYPE";
                imgInvType.Visible = true;
                img = imgInvType;
                break;
            case "BUOFFICE":
                SortBy = "BUOFFICE";
                imgBuOffice.Visible = true;
                img = imgBuOffice;
                break;
            case "INVOICEDATE":
                SortBy = "INVOICEDATE";
                imgInvDate.Visible = true;
                img = imgInvDate;
                break;

        }
        if (img.ToolTip == "Ascending")
        {
            img.ToolTip = "Descending";
            img.ImageUrl = "~/images/descending.gif";
            SortDir = "DESC";
        }
        else
        {
            img.ToolTip = "Ascending";
            img.ImageUrl = "~/images/Ascending.gif";
            SortDir = "ASC";
        }
        DropDownList ddlAccnts = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        //int accountId = Convert.ToInt32(ddlAccountName.SelectedValue);
        if (ddlAccnts.SelectedValue == "")
        {
            ShowError("Please select Account");
            return;
        }
        int accountId = Convert.ToInt32(ddlAccnts.SelectedValue);
        int tpaId = Convert.ToInt32(ddlTPAName.SelectedValue);
        int buOfficeId = Convert.ToInt32(ddlBuOffice.SelectedValue);
        int invTypeId = Convert.ToInt32(ddlInvType.SelectedValue);
        string invNumber = txtInvNum.Text;
        string valnDate = txtValnDate.Text;
        string fromDate = txtFromDate.Text;
        string toDate = txtToDate.Text;
        IList<TPAManualPostingsBE> tpaList = new List<TPAManualPostingsBE>();
        tpaList = new TPAManualPostingsBS().getTPAPostSearchResultList(accountId, tpaId, invNumber, buOfficeId, invTypeId, valnDate, fromDate, toDate);
        switch (SortBy)
        {

            case "VALUATIONDATE":
                if (SortDir == "ASC")
                    tpaList = (tpaList.OrderBy(o => o.ValuationDate)).ToList();
                else if (SortDir == "DESC")
                    tpaList = (tpaList.OrderByDescending(o => o.ValuationDate)).ToList();
                break;
            case "INVOICENUMBER":
                if (SortDir == "ASC")
                    tpaList = (tpaList.OrderBy(o => o.InvoiceNumber)).ToList();
                else if (SortDir == "DESC")
                    tpaList = (tpaList.OrderByDescending(o => o.InvoiceNumber)).ToList();
                break;
            case "INVOICETYPE":
                if (SortDir == "ASC")
                    tpaList = (tpaList.OrderBy(o => o.INVOICE_TYPE_TEXT)).ToList();
                else if (SortDir == "DESC")
                    tpaList = (tpaList.OrderByDescending(o => o.INVOICE_TYPE_TEXT)).ToList();
                break;
            case "BUOFFICE":
                if (SortDir == "ASC")
                    tpaList = (tpaList.OrderBy(o => o.BU_OFFICE_TEXT)).ToList();
                else if (SortDir == "DESC")
                    tpaList = (tpaList.OrderByDescending(o => o.BU_OFFICE_TEXT)).ToList();
                break;
            case "INVOICEDATE":
                if (SortDir == "ASC")
                    tpaList = (tpaList.OrderBy(o => o.InvoiceDate)).ToList();
                else if (SortDir == "DESC")
                    tpaList = (tpaList.OrderByDescending(o => o.InvoiceDate)).ToList();
                break;

        }

        this.lstTpa.DataSource = tpaList.ToList();
        lstTpa.DataBind();
    }

    protected void lstTpa_DataBound(object sender, ListViewItemEventArgs e)
    {


        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            Label lblReviseInd = (Label)e.Item.FindControl("lblhdReviseInd");
            Label lblVoidInd = (Label)e.Item.FindControl("lblhdVoidInd");
            Label lblFinalizeInd = (Label)e.Item.FindControl("lblhdFinalizeInd");
            Label lblCancel = (Label)e.Item.FindControl("lblCancel");
            Label lblRevise = (Label)e.Item.FindControl("lblRevise");
            Label lblVoid = (Label)e.Item.FindControl("lblVoid");
            Label lblCanInd = (Label)e.Item.FindControl("lblhdCanInd");
            Label lblactName = (Label)e.Item.FindControl("lblActName");
            Label lblInvoiceNbr = (Label)e.Item.FindControl("lblInvoiceNumber");
            LinkButton lbnCancel = (LinkButton)e.Item.FindControl("lbnCancelInv");
            LinkButton lbnRevise = (LinkButton)e.Item.FindControl("lbnReviseInv");
            LinkButton lbnVoid = (LinkButton)e.Item.FindControl("lblVoidInv");
            LinkButton voidlink = (LinkButton)e.Item.FindControl("voidlink");
            LinkButton ReviseLink = (LinkButton)e.Item.FindControl("ReviseLink");

            //this replace is to avoid the issues with javascript run time functions 
            //with the accounts having single quote i.e '  Bug fix-10706
            string strAcctName = lblactName.Text.Replace("'", "\\'");

            ListViewDataItem index = (ListViewDataItem)e.Item;
            voidlink.Text = index.DataItemIndex.ToString();
            ReviseLink.Text = index.DataItemIndex.ToString();
            //adding confirmation before doing cancelinvoice operation
            lbnCancel.Attributes.Add("onclick", "return confirm('Do you want to Cancel Invoice Number " + lblInvoiceNbr.Text + " for The " + strAcctName + "?');");
            string strVoid = "";
            //adding confirmation before doing Reviseinvoice operation
            string strReviseText = "Do you want to Revise Invoice Number " + lblInvoiceNbr.Text + " for The " + strAcctName + "?";
            if (lblReviseInd.Text == "")
            {
                lbnRevise.Attributes.Add("onclick", "return ReviseConfirmation('" + strReviseText + "','" + ReviseLink.ClientID + "');");
            }
            else
            {
                lbnRevise.Attributes.Add("onclick", "javascript:alert('Prior invoice being revised, please make sure all subsequent invoices are also revised.'); return ReviseConfirmation('" + strReviseText + "','" + ReviseLink.ClientID + "');");
                strVoid = "void";
                lbnVoid.Attributes.Add("onclick", "");
            }
            //adding confirmation before doing Voidinvoice operation
            string strVOIDText = "Do you want to Void Invoice Number " + lblInvoiceNbr.Text + " for The " + strAcctName + "?";
            if (strVoid != "")
            {
                lbnVoid.Attributes.Add("onclick", "javascript:alert('Prior Invoice being voided, please make sure all subsequent invoices are reviewed and revised.'); return VoidConfirmation('" + strVOIDText + "','" + voidlink.ClientID + "');");
            }
            else
            {
                lbnVoid.Attributes.Add("onclick", "return VoidConfirmation('" + strVOIDText + "','" + voidlink.ClientID + "');");
            }

            if (lblFinalizeInd != null && lbnCancel != null && lblCancel != null && lblCanInd != null)
            {
                if (lblFinalizeInd.Text.ToUpper() != "TRUE" && lblCanInd.Text.ToUpper() != "TRUE")
                {
                    lbnCancel.Visible = true;
                    lblCancel.Visible = false;
                }
                else
                {
                    lbnCancel.Visible = false;
                    lblCancel.Visible = true;
                    lblCancel.Enabled = false;
                }
            }
            if (lblFinalizeInd != null && lbnRevise != null && lblRevise != null && lblVoidInd != null)
            {
                if (lblFinalizeInd.Text.ToUpper() == "TRUE" && lblVoidInd.Text.ToUpper() != "TRUE")
                {
                    lbnRevise.Visible = true;
                    lblRevise.Visible = false;
                }

                else
                {
                    lbnRevise.Visible = false;
                    lblRevise.Visible = true;
                    lblRevise.Enabled = false;
                }
            }
            if (lblFinalizeInd != null && lbnVoid != null && lblVoid != null)
            {
                if (lblFinalizeInd.Text.ToUpper() == "TRUE" && lblVoidInd.Text.ToUpper() != "TRUE")
                {
                    lbnVoid.Visible = true;
                    lblVoid.Visible = false;
                }
                else
                {
                    lbnVoid.Visible = false;
                    lblVoid.Visible = true;
                    lblVoid.Enabled = false;
                }
            }
            if (lblVoidInd.Text.ToUpper() == "TRUE" || lblReviseInd.Text.ToUpper() == "TRUE")
            {
                lbnRevise.Enabled = false;
                lbnVoid.Enabled = false;
            }
            lbnVoid.Enabled = (lbnVoid.Enabled && (CurrentAISUser.Role == GlobalConstants.ApplicationSecurityGroup.Manager || CurrentAISUser.Role == GlobalConstants.ApplicationSecurityGroup.SystemAdmin));
            if (lblInvoiceNbr.Text.Contains("RMV"))
            {
                lbnRevise.Enabled = false;
                lbnVoid.Enabled = false;
            }

        }
    }

    protected void CommandList(Object sender, ListViewCommandEventArgs e)
    {
        ListView lv = (ListView)sender;
        //if (e.CommandName.ToUpper() == "REVISE")
        //{
        //    /// calling function to Revise the selected record in the ListView
        //    ReviseTPA(Convert.ToInt32(e.CommandArgument));
        //}
        //else if (e.CommandName.ToUpper() == "VOID")
        //{
        //    /// calling function to Void the selected record in the ListView
        //    string str = ((TextBox)e.Item.FindControl("txtComments")).Text;
        //    VoidTPA(Convert.ToInt32(e.CommandArgument), str);
        //}
        if (e.CommandName.ToUpper() == "DETAILS")
        {
            /// calling function to Redirect to TPA Manual Postings Page
            Redirect(Convert.ToInt32(e.CommandArgument));

        }
        else if (e.CommandName.ToUpper() == "CANCELINV")
        {
            Label lblAcctid = (Label)e.Item.FindControl("lblActNum");
            /// calling function to Cancel the selected record in the ListView
            CancelTPA(Convert.ToInt32(e.CommandArgument), int.Parse(lblAcctid.Text));

        }

    }
    /// <summary>
    /// Function to Revise the selected record in the ListView
    /// </summary>
    /// <param name="e"></param>
    public void btnRevise_Click(object sender, EventArgs e)
    {
        DropDownList ddlAccnts = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        if (ddlAccnts.SelectedValue == "")
        {
            ShowError("Please select Account");
            return;
        }
        int accountId = Convert.ToInt32(ddlAccnts.SelectedValue);
        int intRedirectTPAID = 0;
        try
        {
            objDC.Connection.Open();
            trans = objDC.Connection.BeginTransaction();
            objDC.Transaction = trans;
            //11251 Fix:Commented to fix TPA issues
            //try
            //{
            //    //11251 Fix:Added "&& cdd.invc_nbr_txt==null && cdd.invc_dt==null" conditions to fix the 11251 issue,This will help us to delete only copied adjustment. from now on it will not delete newly created adjustment
            //    var TPAPostdel = (from cdd in objDC.THRD_PTY_ADMIN_MNL_INVCs where cdd.custmr_id == accountId && cdd.fnl_ind == null && cdd.can_ind == null && cdd.invc_nbr_txt==null && cdd.invc_dt==null select cdd).First();

            //    for (int i = 0; i < TPAPostdel.THRD_PTY_ADMIN_MNL_INVC_DTLs.Count(); i++)
            //    {
            //        objDC.THRD_PTY_ADMIN_MNL_INVC_DTLs.DeleteOnSubmit(TPAPostdel.THRD_PTY_ADMIN_MNL_INVC_DTLs[i]);

            //    }
            //    objDC.SubmitChanges();
            //    objDC.THRD_PTY_ADMIN_MNL_INVCs.DeleteOnSubmit(TPAPostdel);
            //    objDC.SubmitChanges();
            //}
            //catch (Exception)
            //{

            //}
            //Logic to update the Revision indicator to true for the Revised Record
            int TPAID = Convert.ToInt32(ViewState["ReviseIndex"]);
            var TPAPost = (from cdd in objDC.THRD_PTY_ADMIN_MNL_INVCs where cdd.thrd_pty_admin_mnl_invc_id == TPAID select cdd).First();
            TPAManualPostingsBE TPABEConcurrencyOld = (TPAPostBEList.Where(o => o.ThirdPartyAdminManualInvoiceID.Equals(TPAID))).First();
            bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(TPABEConcurrencyOld.UpdatedDate), Convert.ToDateTime(TPAPost.updt_dt));
            if (!Concurrency)
            {
                trans.Rollback();
                if (objDC.Connection.State == ConnectionState.Open)
                    objDC.Connection.Close();
                return;
            }
            TPAPost.cmmnt_txt = txtReviseComments.Text;
            TPAPost.updt_dt = System.DateTime.Now;
            TPAPost.revise_ind = true;
            TPAPost.updt_user_id = CurrentAISUser.PersonID;
            txtReviseComments.Text = "";

            //Logic to copy the Revised invoice record in Third party manual invoice table 
            // and create new record in Third party Manual Invoice table
            THRD_PTY_ADMIN_MNL_INVC TPANew = new THRD_PTY_ADMIN_MNL_INVC()
            {
                custmr_id = TPAPost.custmr_id,
                invc_amt = TPAPost.invc_amt,
                fnl_ind = null,
                invc_dt = TPAPost.invc_dt,
                due_dt = TPAPost.due_dt,
                thrd_pty_admin_id = TPAPost.thrd_pty_admin_id,
                thrd_pty_admin_invc_typ_id = TPAPost.thrd_pty_admin_invc_typ_id,
                bil_cycl_id = TPAPost.bil_cycl_id,
                thrd_pty_admin_los_src_id = TPAPost.thrd_pty_admin_los_src_id,
                end_dt = TPAPost.end_dt,
                crte_dt = DateTime.Now,
                crte_user_id = CurrentAISUser.PersonID,
                actv_ind = true,
                pol_yy_nbr = TPAPost.pol_yy_nbr,
                bsn_unt_ofc_id = TPAPost.bsn_unt_ofc_id,
                valn_dt = TPAPost.valn_dt,
                related_invoice_id = TPAPost.thrd_pty_admin_mnl_invc_id,
                invc_nbr_txt = null,
            };
            objDC.THRD_PTY_ADMIN_MNL_INVCs.InsertOnSubmit(TPANew);
            objDC.SubmitChanges();
            //Logic to copy the Revised invoice record in Third party manual invoice Details table 
            // and create new records in Third party Manual Invoice Details table
            //            IList<TPAManualPostingsDetailBE> TPAdtlBE = (new TPAManualPostingsDetailBS()).getTPAPostDltsList(TPAID);
            var TPAdtlBE = (from cdd in objDC.THRD_PTY_ADMIN_MNL_INVC_DTLs where cdd.thrd_pty_admin_mnl_invc_id == TPAID select cdd).ToList();
            for (int i = 0; i < TPAdtlBE.Count; i++)
            {
                THRD_PTY_ADMIN_MNL_INVC_DTL TPAnewDtl = new THRD_PTY_ADMIN_MNL_INVC_DTL()
                {
                    thrd_pty_admin_mnl_invc_id = TPANew.thrd_pty_admin_mnl_invc_id,
                    custmr_id = TPANew.custmr_id,
                    thrd_pty_admin_amt = TPAdtlBE[i].thrd_pty_admin_amt,
                    post_trns_typ_id = TPAdtlBE[i].post_trns_typ_id,
                    crte_dt = DateTime.Now,
                    crte_user_id = CurrentAISUser.PersonID,
                    updt_dt = null,
                    updt_user_id = null,
                    aries_main_nbr_txt = TPAdtlBE[i].aries_main_nbr_txt,
                    aries_sub_nbr_txt = TPAdtlBE[i].aries_sub_nbr_txt,
                    comp_cd_txt = TPAdtlBE[i].comp_cd_txt,
                    eff_dt = TPAdtlBE[i].eff_dt,
                    expi_dt = TPAdtlBE[i].expi_dt,
                    pol_nbr_txt = TPAdtlBE[i].pol_nbr_txt,
                    pol_modulus_txt = TPAdtlBE[i].pol_modulus_txt,
                    pol_sym_txt = TPAdtlBE[i].pol_sym_txt,
                };

                objDC.THRD_PTY_ADMIN_MNL_INVC_DTLs.InsertOnSubmit(TPAnewDtl);
                objDC.SubmitChanges();
            }
            //calling Aries Procedure for Revision
            string ErroMsg;
            ErroMsg = string.Empty;
            objDC.ModAIS_TPATransmittalToARiES(TPAPost.thrd_pty_admin_mnl_invc_id, TPAPost.custmr_id, ref ErroMsg, 2);
            if (ErroMsg == "")
            {
                trans.Commit();
                /// TPA Detials Search Method
                intRedirectTPAID = TPANew.thrd_pty_admin_mnl_invc_id;
            }
            else
            {
                trans.Rollback();
                (new ApplicationStatusLogBS()).WriteLog("AIS TPA/Manual", "ERR", "TPA/Manual Revision error", ErroMsg, accountId, CurrentAISUser.PersonID);
                ShowError("TPA/Manual Invoice could not be Revised due to an error. Please check the error log for additional details");
            }
            Search();
        }
        catch (Exception ee)
        {
            trans.Rollback();
            /// TPA Detials Search Method
            Search();
            (new ApplicationStatusLogBS()).WriteLog("AIS TPA/Manual", "ERR", "TPA/Manual Revision error", ee.Message, accountId, CurrentAISUser.PersonID);
            ShowError("TPA/Manual Invoice could not be Revised due to an error. Please check the error log for additional details");

        }
        finally
        {
            if (objDC.Connection.State == ConnectionState.Open)
                objDC.Connection.Close();
        }
        /// TPA Detials Search Method
        if (intRedirectTPAID > 0)
            Redirect(intRedirectTPAID);


    }
    /// <summary>
    /// Function to Cancel the Search Results.
    /// </summary>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    { }
    /// <summary>
    /// Function to Void the selected record in the ListView
    /// </summary>
    /// <param name="e"></param>
    protected void btnVoid_Click(object sender, EventArgs e)
    {
        DropDownList ddlAccnts = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        if (ddlAccnts.SelectedValue == "")
        {
            ShowError("Please select Account");
            return;
        }
        int accountId = Convert.ToInt32(ddlAccnts.SelectedValue);
        try
        {
            //Logic to update the Void indicator to true for the Voided Record
            objDC.Connection.Open();
            trans = objDC.Connection.BeginTransaction();
            objDC.Transaction = trans;
            int TPAID = Convert.ToInt32(ViewState["VoidIndex"]);
            var TPAPost = (from cdd in objDC.THRD_PTY_ADMIN_MNL_INVCs where cdd.thrd_pty_admin_mnl_invc_id == TPAID select cdd).First();
            TPAManualPostingsBE TPABEConcurrencyOld = (TPAPostBEList.Where(o => o.ThirdPartyAdminManualInvoiceID.Equals(TPAID))).First();
            bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(TPABEConcurrencyOld.UpdatedDate), Convert.ToDateTime(TPAPost.updt_dt));
            if (!Concurrency)
            {
                trans.Rollback();
                if (objDC.Connection.State == ConnectionState.Open)
                    objDC.Connection.Close();
                return;
            }
            TPAPost.cmmnt_txt = txtComments.Text;
            TPAPost.updt_dt = System.DateTime.Now;
            TPAPost.void_ind = true;
            TPAPost.updt_user_id = CurrentAISUser.PersonID;
            txtComments.Text = "";
            //Logic to copy the voided invoice record in Third party manual invoice table 
            // and create new record in Third party Manual Invoice table
            THRD_PTY_ADMIN_MNL_INVC TPANew = new THRD_PTY_ADMIN_MNL_INVC()
            {
                custmr_id = TPAPost.custmr_id,
                invc_amt = TPAPost.invc_amt,
                fnl_ind = TPAPost.fnl_ind,
                invc_dt = DateTime.Now,
                due_dt = TPAPost.due_dt,
                thrd_pty_admin_id = TPAPost.thrd_pty_admin_id,
                thrd_pty_admin_invc_typ_id = TPAPost.thrd_pty_admin_invc_typ_id,
                bil_cycl_id = TPAPost.bil_cycl_id,
                thrd_pty_admin_los_src_id = TPAPost.thrd_pty_admin_los_src_id,
                end_dt = TPAPost.end_dt,
                crte_dt = DateTime.Now,
                crte_user_id = CurrentAISUser.PersonID,
                actv_ind = true,
                pol_yy_nbr = TPAPost.pol_yy_nbr,
                bsn_unt_ofc_id = TPAPost.bsn_unt_ofc_id,
                valn_dt = TPAPost.valn_dt,
                related_invoice_id = TPAPost.thrd_pty_admin_mnl_invc_id,
                //------------------Logic for generating VoidInvoice Number------------------
                invc_nbr_txt = (TPAPost.invc_nbr_txt.Length > 5) ? "RMV01" + TPAPost.invc_nbr_txt.Remove(0, 5) : null,
            };
            objDC.THRD_PTY_ADMIN_MNL_INVCs.InsertOnSubmit(TPANew);
            objDC.SubmitChanges();
            //Logic to copy the voided invoice record in Third party manual invoice Details table 
            // and create new records in Third party Manual Invoice Details table
            IList<TPAManualPostingsDetailBE> TPAdtlBE = (new TPAManualPostingsDetailBS()).getTPAPostDltsList(TPAID);
            for (int i = 0; i < TPAdtlBE.Count; i++)
            {
                THRD_PTY_ADMIN_MNL_INVC_DTL TPAnewDtl = new THRD_PTY_ADMIN_MNL_INVC_DTL()
                {
                    thrd_pty_admin_mnl_invc_id = TPANew.thrd_pty_admin_mnl_invc_id,
                    custmr_id = TPANew.custmr_id,
                    thrd_pty_admin_amt = TPAdtlBE[i].ThirdPartyAdminAmt,
                    post_trns_typ_id = TPAdtlBE[i].PostingTrnsTypID,
                    crte_dt = DateTime.Now,
                    crte_user_id = CurrentAISUser.PersonID,
                    updt_dt = null,
                    updt_user_id = null,
                    aries_main_nbr_txt = TPAdtlBE[i].AriesMainNbr,
                    aries_sub_nbr_txt = TPAdtlBE[i].AriesSubNbr,
                    comp_cd_txt = TPAdtlBE[i].CompanyCode,
                    eff_dt = TPAdtlBE[i].EffectiveDate,
                    expi_dt = TPAdtlBE[i].ExpiryDate,
                    pol_nbr_txt = TPAdtlBE[i].PolicyNbrText,
                    pol_modulus_txt = TPAdtlBE[i].PolicyModText,
                    pol_sym_txt = TPAdtlBE[i].PolicySymbolText,
                };

                objDC.THRD_PTY_ADMIN_MNL_INVC_DTLs.InsertOnSubmit(TPAnewDtl);
                objDC.SubmitChanges();
            }
            //calling Aries PRocedure for Void
            string ErroMsg;
            ErroMsg = string.Empty;
            //sending the New TPAID to Aries..
            objDC.ModAIS_TPATransmittalToARiES(TPANew.thrd_pty_admin_mnl_invc_id, TPANew.custmr_id, ref ErroMsg, 3);
            if (ErroMsg == "")
            {
                trans.Commit();
                /// TPA Detials Search Method
                Search();
                //Displya the void confirmation from modal popup
                modalAries.Show();
            }
            else
            {
                trans.Rollback();
                (new ApplicationStatusLogBS()).WriteLog("AIS TPA/Manual", "ERR", "TPA/Manual Void error", ErroMsg, accountId, CurrentAISUser.PersonID);
                ShowError("TPA/Manual Invoice could not be Voided due to an error. Please check the error log for additional details");
            }
        }
        catch (Exception ee)
        {
            trans.Rollback();
            /// TPA Detials Search Method
            Search();
            (new ApplicationStatusLogBS()).WriteLog("AIS TPA/Manual", "ERR", "TPA/Manual Void error", ee.Message, accountId, CurrentAISUser.PersonID);
            ShowError("TPA/Manual Invoice could not be Voided due to an error. Please check the error log for additional details");

        }
        finally
        {
            if (objDC.Connection.State == ConnectionState.Open)
                objDC.Connection.Close();
        }

    }
    /// <summary>
    /// Function to Cancel the selected record in the ListView
    /// </summary>
    /// <param name="e"></param>
    public void CancelTPA(int TPAID, int custmerid)
    {
        try
        {
            //Logic to update the Cancel indicator to true for the Canceled Record
            objDC.Connection.Open();
            trans = objDC.Connection.BeginTransaction();
            objDC.Transaction = trans;
            var TPAPost = (from cdd in objDC.THRD_PTY_ADMIN_MNL_INVCs where cdd.thrd_pty_admin_mnl_invc_id == TPAID select cdd).First();
            var TPAMasterPost = (from cdd in objDC.THRD_PTY_ADMIN_MNL_INVCs where cdd.thrd_pty_admin_mnl_invc_id == TPAPost.related_invoice_id select cdd).FirstOrDefault();
            TPAManualPostingsBE TPABEConcurrencyOld = (TPAPostBEList.Where(o => o.ThirdPartyAdminManualInvoiceID.Equals(TPAID))).First();
            bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(TPABEConcurrencyOld.UpdatedDate), Convert.ToDateTime(TPAPost.updt_dt));
            if (!Concurrency)
            {
                trans.Rollback();
                if (objDC.Connection.State == ConnectionState.Open)
                    objDC.Connection.Close();
                return;
            }
            //TPAPost.actv_ind = false;
            //TPAPostBE = TPAService.getTPAPostRow(TPAID);
            //TPAPostBE.Active = false;
            //------------------Logic for generating CancelInvoice Number------------------
            if (TPAPost.related_invoice_id == null)
            {
                TPAPost.invc_nbr_txt = GenenrateInvoiceNumber(TPAPost.thrd_pty_admin_mnl_invc_id.ToString().Length, TPAPost.thrd_pty_admin_mnl_invc_id.ToString());
            }
            else
            {
                TPAManualPostingsBE TPAMasterBE = (new TPAManualPostingsBS()).getTPAPostRow(int.Parse(TPAPost.related_invoice_id.ToString()));
                string strnumber = TPAMasterBE.InvoiceNumber;
                if (strnumber.Length > 5)
                    TPAPost.invc_nbr_txt = "RMC01" + strnumber.Remove(0, 5);
            }
            TPAPost.updt_dt = DateTime.Now;
            TPAPost.updt_user_id = CurrentAISUser.PersonID;
            TPAPost.can_ind = true;

            //11252 Fix:Setting revision indicator to false as we are cancelling the revised adjustment.The following code clears the indicators and comments.
            if (TPAMasterPost != null)
            {
                TPAMasterPost.revise_ind = null;
                TPAMasterPost.cmmnt_txt = "";
                TPAMasterPost.updt_dt = DateTime.Now;
                TPAMasterPost.updt_user_id = CurrentAISUser.PersonID;
            }
            //bool Flag = TPAService.Update(TPAPostBE);
            //calling Aries PRocedure for Cancel
            //TPAService.TPATransmittalToARiES(TPAPostBE.ThirdPartyAdminManualInvoiceID, TPAPostBE.CustomerID, 4);
            string ErroMsg;
            ErroMsg = string.Empty;

            objDC.ModAIS_TPATransmittalToARiES(TPAPost.thrd_pty_admin_mnl_invc_id, TPAPost.custmr_id, ref ErroMsg, 4);
            if (ErroMsg == "")
            {
                objDC.SubmitChanges();
                trans.Commit();
            }
            else
            {
                trans.Rollback();
                ShowError("TPA/Manual Invoice could not be Cancelled due to an error. Please check the error log for additional details");
                (new ApplicationStatusLogBS()).WriteLog("AIS TPA/Manual", "ERR", "TPA/Manual Cancel error", ErroMsg, custmerid, CurrentAISUser.PersonID);
            }
            /// TPA Detials Search Method
            Search();
        }
        catch (Exception ee)
        {
            trans.Rollback();
            /// TPA Detials Search Method
            Search();
            (new ApplicationStatusLogBS()).WriteLog("AIS TPA/Manual", "ERR", "TPA/Manual Cancel error", ee.Message, custmerid, CurrentAISUser.PersonID);
            ShowError("TPA/Manual Invoice could not be Cancelled due to an error. Please check the error log for additional details");

        }
        finally
        {
            if (objDC.Connection.State == ConnectionState.Open)
                objDC.Connection.Close();
        }

    }
    /// <summary>
    ///  COde to redirect to the TPApostings webpage when details link is clicked.
    /// </summary>
    public void Redirect(int TPAID)
    {
        TPAPostBE = TPAService.getTPAPostRow(TPAID);
        AccountBE acct = (new AccountBS()).getAccount(TPAPostBE.CustomerID);
        AISMasterEntities = new MasterEntities();
        AISMasterEntities.AccountStatus = acct.ACTV_IND;
        AISMasterEntities.AccountNumber = Convert.ToInt32(TPAPostBE.CustomerID);
        AISMasterEntities.AccountName = acct.FULL_NM;
        AISMasterEntities.Bpnumber = acct.FINC_PTY_ID == null ? "" : acct.FINC_PTY_ID.ToString();
        AISMasterEntities.SSCGID = acct.SUPRT_SERV_CUSTMR_GP_ID == null ? "" : acct.SUPRT_SERV_CUSTMR_GP_ID.ToString();
        AISMasterEntities.MasterAccount = acct.MSTR_ACCT_IND == null ? false : acct.MSTR_ACCT_IND.Value;
        AISMasterEntities.MasterAccountNumber = (acct.CUSTMR_REL_ID == null) ? 0 : acct.CUSTMR_REL_ID.Value;
        //Response.Redirect("TPAManualPosting.aspx?TPAID=" + TPAID);
        ResponseRedirect("TPAManualPosting.aspx?TPAID=" + TPAID);
    }
    /// <summary>
    /// Code for generating TPA INvoice Number
    /// </summary>
    /// <param name="i"></param>
    /// <param name="strID"></param>
    /// <returns></returns>
    private string GenenrateInvoiceNumber(int i, string strID)
    {
        string str = "RMC01";
        if (i == 1)
        {
            str += "000000000" + strID;
        }
        else if (i == 2)
        {
            str += "00000000" + strID;
        }
        else if (i == 3)
        {
            str += "0000000" + strID;
        }
        else if (i == 4)
        {
            str += "000000" + strID;
        }
        else if (i == 5)
        {
            str += "00000" + strID;
        }
        else if (i == 6)
        {
            str += "0000" + strID;
        }
        else if (i == 7)
        {
            str += "000" + strID;
        }
        return str;
    }

   
}
