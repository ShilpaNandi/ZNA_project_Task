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
using System.Collections.Generic;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.AIS.DAL.Logic;
using System.Web.SessionState;
using ZurichNA.AIS.WebSite.ZDWJavaWS;
using Microsoft.Web.Services3;
using System.Web.Services;
using Microsoft.Web.Services3.Security.Tokens;
using System.IO;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using ZurichNA.AIS.WebSite.Reports;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.LSP.Framework;
using ZurichNA.AIS.DAL.LINQ;


public partial class Invoice_AdjustmentMgmt : AISBasePage
{
    System.Data.Common.DbTransaction trans = null;
    AISDatabaseLINQDataContext objDC = new AISDatabaseLINQDataContext(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
    public class AsyncSearch
    {
        private delegate string InvokeSearch();
        private InvokeSearch dlgInvoke;

        public AsyncSearch()
        {
        }

        public int AcctNameId;
        public int AdjStatusId;
        public string InvNumber;
        public int PersId;
        public DateTime InvFrmDt;
        public DateTime InvToDt;
        public DateTime ValnDt;

        public IList<AdjustmentManagementBE> AdjManagementLst;

        /// <summary>
        /// Binds the list view with the selected criteria data 
        /// </summary>
        public string BindInvoicelistView()
        {
            AdjManagementLst = new AdjustmentManagementBS().getAdjManagement(AcctNameId,
                AdjStatusId, InvNumber, PersId, InvFrmDt, InvToDt, ValnDt);

            return "Success";
        }

        public IAsyncResult BeginSearch(AsyncCallback callBack, object data)
        {
            dlgInvoke = new InvokeSearch(BindInvoicelistView);
            return dlgInvoke.BeginInvoke(callBack, data);
        }

        public string EndSearch(IAsyncResult asyncRes)
        {
            return dlgInvoke.EndInvoke(asyncRes);
        }
    }

    private AsyncSearch aycSearch = new AsyncSearch();
    private AdjustmentManagementBS AdjMgmtBS;
    protected static Common common = null;

    /// <summary>
    /// a property for Adjustment management Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>AdjustmentManagementBE</returns>
    private AdjustmentManagementBE AdgMgmtBE
    {
        get { return (AdjustmentManagementBE)Session["AdgMgmtBE"]; }
        set { Session["AdgMgmtBE"] = value; }
    }
    private AdjustmentManagementBE AdgMgmtBEOld
    {
        get { return (AdjustmentManagementBE)Session["AdgMgmtBEOld"]; }
        set { Session["AdgMgmtBEOld"] = value; }
    }
    IList<AdjustmentManagementBE> AdjMgmtlst
    {
        get
        {
            if (Session["AdjMgmtlst"] == null)
                Session["AdjMgmtlst"] = new List<AdjustmentManagementBE>();
            return (IList<AdjustmentManagementBE>)Session["AdjMgmtlst"];
        }
        set
        {
            Session["AdjMgmtlst"] = value;
        }
    }
    protected void Page_PreInit(object sender, EventArgs e)
    {
        ScriptManager sm = (ScriptManager)Master.FindControl("ScriptManager1");
        sm.EnablePartialRendering = false;
    }
    /// <summary>
    /// Page load event Loads the AdjustmentMgmt page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!Page.IsPostBack)
        {
            //PostBackTrigger PST = new PostBackTrigger();
            //PST.ControlID = "btnUploadOk";
            //updInternal.Triggers.Add(PST);
            this.Master.Page.Title = "Adjustment Management";
            AdgMgmtBE = new AdjustmentManagementBE();
            ddUserLst.DataSource = (new PersonBS()).getPersonsList();
            ddUserLst.DataValueField = "PERSON_ID";
            ddUserLst.DataTextField = "FULLNAME";
            ddUserLst.DataBind();
            ListItem li = new ListItem("(Select)", "0");
            ddUserLst.Items.Insert(0, li);
        }
        //Checks Exiting without Save
        ArrayList list = new ArrayList();
        list.Add(chkBxReavisionReasonInd);
        list.Add(chkBxVoidReasonInd);
        list.Add(ChkBkPendingInv);
        list.Add(chkbxSetQc);
        list.Add(chkHistrorical);
        list.Add(ddPndReason);
        list.Add(ddVoidReason);
        list.Add(ddlRevisionID);
        list.Add(btnSave);
        list.Add(btnCancel);
//        list.Add(btnCnclinvoice);
        list.Add(btnDetails);
//        list.Add(btnRevseInv);
//        list.Add(btnVoidInv);
        list.Add(btnUploadZDW);
        list.Add(btnlssinfo);
        list.Add(btnAdjMgmtSearch);
        list.Add(btnAdjMgmtClear);
        list.Add(lnkClose);
        ProcessExitFlag(list);
    }

    /// <summary>
    /// Clears all the values from the search criteria
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdjMgmtClear_Click(object sender, EventArgs e)
    {
        txtInvoiceDateFrm.Text = String.Empty;
        txtInvoiceDateTo.Text = String.Empty;
        txtInvoiceNmber.Text = String.Empty;
        txtValuationDate.Text = String.Empty;
        DropDownList ddMastActAcctlst = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        ddMastActAcctlst.SelectedIndex = -1;
        ddAdjStatus.SelectedIndex = -1;
        ddUserLst.SelectedIndex = -1;
        ddPndReason.SelectedIndex = -1;
        lstAdjMgmtDtl.Visible = false;
        lstAdjMgmtDtl.Items.Clear();
    }

    /// <summary>
    /// Search button click event will display result for search criteria
    /// At least one of the search criteria must be selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdjMgmtSearch_Click(object sender, EventArgs e)
    {
        SearchAdjData();
    }
    /// <summary>
    /// subprocedure for Adjustment details search
    /// </summary>
    public void SearchAdjData()
    {
        //function to clear the error messages
        ClearError();
        DropDownList ddMastActAcctlst = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        //if (ddMastActAcctlst.SelectedValue == "0" || ddMastActAcctlst.SelectedValue == "")
        //{
        //    ClearError();
        //    ShowError("Please select Account ");
        //    return;
        //}

        if ((txtValuationDate.Text != string.Empty) || (txtInvoiceDateFrm.Text != string.Empty) ||
            (txtInvoiceDateTo.Text != string.Empty) || (txtInvoiceNmber.Text != string.Empty) ||
            (ddMastActAcctlst.SelectedIndex != 0) || (ddAdjStatus.SelectedIndex != 0) ||
            (ddUserLst.SelectedIndex != 0))
        {
            aycSearch = new AsyncSearch();
            PageAsyncTask paTask = new PageAsyncTask(BeginSearch, EndSearch, SearchTimeOut, null, false);
            Page.AsyncTimeout = new TimeSpan(0, 3, 0);
            Page.RegisterAsyncTask(paTask);
        }
        else
        {
            ShowMessage("Please select at least one search criteria before clicking on search button");

        }
    }

    public IAsyncResult BeginSearch(object sender, EventArgs e, AsyncCallback
    callBack, object data)
    {
        this.lstAdjMgmtDtl.Visible = true;
        ViewState["SelectedIndex"] = 0;

        DropDownList ddMastActAcctlst = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        aycSearch.AcctNameId = ((ddMastActAcctlst.Items.FindByValue("0").Selected == true) ? 0 : Convert.ToInt32(ddMastActAcctlst.SelectedValue));
        aycSearch.AdjStatusId = ((ddAdjStatus.Items.FindByValue("0").Selected == true) ? 0 : Convert.ToInt32(ddAdjStatus.SelectedValue));
        aycSearch.InvNumber = ((txtInvoiceNmber.Text.Trim().Length == 0) ? String.Empty : txtInvoiceNmber.Text);
        aycSearch.PersId = ((ddUserLst.Items.FindByValue("0").Selected == true) ? 0 : Convert.ToInt32(ddUserLst.SelectedValue));
        aycSearch.InvFrmDt = ((txtInvoiceDateFrm.Text.ToString() == "") ? Convert.ToDateTime(null) : Convert.ToDateTime(txtInvoiceDateFrm.Text));
        aycSearch.InvToDt = ((txtInvoiceDateTo.Text.ToString() == "") ? Convert.ToDateTime(null) : Convert.ToDateTime(txtInvoiceDateTo.Text));
        aycSearch.ValnDt = ((txtValuationDate.Text.ToString() == "") ? Convert.ToDateTime(null) : Convert.ToDateTime(txtValuationDate.Text));

        return aycSearch.BeginSearch(callBack, data);
    }

    public void EndSearch(IAsyncResult result)
    {
        aycSearch.EndSearch(result);

        AdjMgmtlst = aycSearch.AdjManagementLst;
        this.lstAdjMgmtDtl.DataSource = AdjMgmtlst;
        this.lstAdjMgmtDtl.DataBind();

        if (AdjMgmtlst.Count <= 0)
        {
            ClearError();
            string strMessage = "No Record(s) found...!";
            ShowError(strMessage);
        }

    }

    public void SearchTimeOut(IAsyncResult result)
    {
        ClearError();
        ShowError("Time is out and search took too long. Please include additional search criteria");
    }
    /// <summary>
    /// Sorting the Listview based on Valuation Date
    /// Invoice Number, Invoice Date and Adj Status
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstAdjMgmtDtl_Sorting(object sender, ListViewSortEventArgs e)
    {
        ImageButton imgValutnDate = new ImageButton();
        ImageButton imgInvNumber = new ImageButton();
        ImageButton imgInvdate = new ImageButton();
        ImageButton imgAdjStatus = new ImageButton();

        imgValutnDate = (ImageButton)lstAdjMgmtDtl.FindControl("imgValDtSrt");
        imgInvNumber = (ImageButton)lstAdjMgmtDtl.FindControl("imgDrftInvNum");
        imgInvdate = (ImageButton)lstAdjMgmtDtl.FindControl("imgDrftInvDate");
        imgAdjStatus = (ImageButton)lstAdjMgmtDtl.FindControl("imgAdjustStatus");

        switch (e.SortExpression)
        {
            case "ValtnDate":
                e.SortDirection = (imgValutnDate.ImageUrl.Contains("Des")) ? System.Web.UI.WebControls.SortDirection.Ascending : System.Web.UI.WebControls.SortDirection.Descending;

                if (e.SortDirection == System.Web.UI.WebControls.SortDirection.Ascending)
                    AdjMgmtlst = AdjMgmtlst.OrderBy(sl => sl.ValtnDate).ToList();
                else
                    AdjMgmtlst = AdjMgmtlst.OrderByDescending(sl => sl.ValtnDate).ToList();
                ChangeImage(imgValutnDate, e.SortDirection, imgInvNumber, imgInvdate, imgAdjStatus);
                break;
            case "DrftInvoicenmr":
                e.SortDirection = (imgInvNumber.ImageUrl.Contains("Des")) ? System.Web.UI.WebControls.SortDirection.Ascending : System.Web.UI.WebControls.SortDirection.Descending;
                if (e.SortDirection == System.Web.UI.WebControls.SortDirection.Ascending)
                    AdjMgmtlst = AdjMgmtlst.OrderBy(sl => sl.DrftInvoicenmr).ToList();
                else
                    AdjMgmtlst = AdjMgmtlst.OrderByDescending(sl => sl.DrftInvoicenmr).ToList();
                ChangeImage(imgInvNumber, e.SortDirection, imgValutnDate, imgInvdate, imgAdjStatus);
                break;
            case "DrftInvoiceDate":
                e.SortDirection = (imgInvdate.ImageUrl.Contains("Des")) ? System.Web.UI.WebControls.SortDirection.Ascending : System.Web.UI.WebControls.SortDirection.Descending;
                if (e.SortDirection == System.Web.UI.WebControls.SortDirection.Ascending)
                    AdjMgmtlst = AdjMgmtlst.OrderBy(sl => sl.DrftInvoiceDate).ToList();
                else
                    AdjMgmtlst = AdjMgmtlst.OrderByDescending(sl => sl.DrftInvoiceDate).ToList();
                ChangeImage(imgInvdate, e.SortDirection, imgValutnDate, imgInvNumber, imgAdjStatus);
                break;
            case "Adjuststatus":
                e.SortDirection = (imgAdjStatus.ImageUrl.Contains("Des")) ? System.Web.UI.WebControls.SortDirection.Ascending : System.Web.UI.WebControls.SortDirection.Descending;
                if (e.SortDirection == System.Web.UI.WebControls.SortDirection.Ascending)
                    AdjMgmtlst = AdjMgmtlst.OrderBy(sl => sl.Adjuststatus).ToList();
                else
                    AdjMgmtlst = AdjMgmtlst.OrderByDescending(sl => sl.Adjuststatus).ToList();
                ChangeImage(imgAdjStatus, e.SortDirection, imgValutnDate, imgInvNumber, imgInvdate);
                break;
        }

        lstAdjMgmtDtl.DataSource = AdjMgmtlst;
        lstAdjMgmtDtl.DataBind();
    }

    /// <summary>
    /// This method is used with above sorting method
    /// </summary>
    /// <param name="imgBtn"></param>
    /// <param name="sDir"></param>
    /// <param name="img1"></param>
    /// <param name="img2"></param>
    /// <param name="img3"></param>
    private void ChangeImage(ImageButton imgBtn, System.Web.UI.WebControls.SortDirection sDir, ImageButton img1, ImageButton img2, ImageButton img3)
    {
        if (sDir == System.Web.UI.WebControls.SortDirection.Ascending)
        {
            imgBtn.ImageUrl = "~/images/ascending.gif";
            imgBtn.ToolTip = "Ascending";
        }
        else
        {
            imgBtn.ImageUrl = "~/images/Descending.gif";
            imgBtn.ToolTip = "Descending";
        }
        imgBtn.Visible = true;
        img1.ImageUrl = img2.ImageUrl = img3.ImageUrl = "~/images/ascending.gif";
        img1.Visible = img2.Visible = img3.Visible = false;

    }

    /// <summary>
    /// This Method is not used now this was created initially when we had all the information in a single grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstAdjMgmtDtl_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        //if (e.CommandName == "Save")
        //{
        //    lstLBARelatedinfo_Saving(e.Item);
        //}
        //else if (e.CommandName == "Update")
        //{
        //    lstLBAParmsetupDtls_ItemUpdate(e.Item);

        //}
        //else 
        //int accountId = int.Parse(commandArgs[0].ToString());

        //DateTime valDate = DateTime.Parse(lblValDate.Text);
        //AccountBE accountBE = (new AccountBS()).Retrieve(accountId);
        //AISMasterEntities.AccountNumber = accountBE.CUSTMR_ID;
        //AISMasterEntities.AccountName = accountBE.FULL_NM;
        //AISMasterEntities.Bpnumber = accountBE.FINC_PTY_ID;
        //AISMasterEntities.SSCGID = accountBE.SUPRT_SERV_CUSTMR_GP_ID;
        //AISMasterEntities.ValuationDate = valDate;
        //if (e.CommandName.ToUpper() == "DETAILS")
        //{
        //    Response.Redirect("~/AdjCalc/InvoicingDashboard.aspx");
        //}
        //else if (e.CommandName.ToUpper() == "LOSS INFO")
        //{
        //    Response.Redirect("~/Loss/LossInfo.aspx");
        //}
    }

    /// <summary>
    /// bool Function to check the record is new or existing. Used while saving the record
    /// </summary>
    /// <param name=""></param>
    /// <returns>bool(True/False)</returns>
    protected bool CheckNew
    {
        get { return (bool)ViewState["CheckNew"]; }
        set { ViewState["CheckNew"] = value; }
    }

    /// <summary>
    /// Protected Property for storing Selectedvalue of Adjustment Management list
    /// </summary>
    /// <param name=""></param>
    /// <returns>integer Selected value</returns>
    protected int SelectedValue
    {
        get
        {
            return (hidindex.Value != null ? Convert.ToInt32(hidindex.Value) : -1);
        }
        set { hidindex.Value = value.ToString(); }
    }

    /// <summary>
    /// Function to display the selected Adjustment Management details
    /// </summary>
    /// <param name="PremAdjMgmtID">Adjustment Management ID of the selected value in the listview</param>
    /// <returns></returns>
    public void DisplayDetails(int PremAdjMgmtID, int AdjStatusNumbrID, string AdjStatusType)
    {
        txtInvDate.Text = "";
        txtInvNumber.Text = "";

        AdjustmentManagementBS AdjMgmtBS = new AdjustmentManagementBS();
        AdgMgmtBE = AdjMgmtBS.getPremMgmtRow(PremAdjMgmtID);

        AdgMgmtBEOld = AdgMgmtBE;
        txtAcctNumber.Text = AdgMgmtBE.custmrID.ToString();
        txtAcctName.Text = AdgMgmtBE.CustomerFullName;

        //if (AdgMgmtBE.DrftInvoicenmr != null)
        txtInvNumber.Text = ((AdgMgmtBE.FinalInvoicenmr == null) ? AdgMgmtBE.DrftInvoicenmr : AdgMgmtBE.FinalInvoicenmr);
        //if (AdgMgmtBE.DrftInvoiceDate != null)
        txtInvDate.Text = ((AdgMgmtBE.FinalInvoiceDate == null) ? (AdgMgmtBE.DrftInvoiceDate == null ? "" : AdgMgmtBE.DrftInvoiceDate.Value.ToShortDateString()) : AdgMgmtBE.FinalInvoiceDate.Value.ToShortDateString());
        if (AdgMgmtBE.ValtnDate != null)
            txtValutiondtlsDate.Text = DateTime.Parse(AdgMgmtBE.ValtnDate.ToString()).ToShortDateString();
        if (AdgMgmtBE.AdjPendingIndctor != null)
        {
            if (AdgMgmtBE.AdjPendingIndctor == true)
            {
                ChkBkPendingInv.Checked = true;
            }
            else
            {
                ChkBkPendingInv.Checked = false;
            }
        }
        else { ChkBkPendingInv.Checked = false; }
        if (AdgMgmtBE.TwentyReqIND != null)
        {
            if (AdgMgmtBE.TwentyReqIND == true)
            {
                chkbxSetQc.Checked = true;
            }
            else
            {
                chkbxSetQc.Checked = false;
            }
        }
        else { chkbxSetQc.Checked = false; }
        if (AdgMgmtBE.HistoricalIndc != null)
        {
            if (AdgMgmtBE.HistoricalIndc == true)
            {
                chkHistrorical.Checked = true;
            }
            else
            {
                chkHistrorical.Checked = false;
            }
        }
        else { chkHistrorical.Checked = false; }

        if (AdgMgmtBE.VoidReasonIndc != true)
        {
            chkBxVoidReasonInd.Checked = false;
            ddVoidReason.Enabled = false;
        }
        else
        {
            chkBxVoidReasonInd.Checked = true;
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            {
                ddVoidReason.Enabled = true;
                if (AdgMgmtBE.VoidReasonID != null && AdgMgmtBE.VoidReasonID > 0)
                {
                    ddVoidReason.DataBind();
                    ddVoidReason.Items.FindByValue(AdgMgmtBE.VoidReasonID.ToString()).Selected = true;
                }
            }
        }

        if (AdgMgmtBE.ReviseReasonIndc != true)
        {
            chkBxReavisionReasonInd.Checked = false;
            ddlRevisionID.Enabled = false;
        }
        else
        {
            chkBxReavisionReasonInd.Checked = true;
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            {
                ddlRevisionID.Enabled = true;
                ddVoidReason.Enabled = true;
                if (AdgMgmtBE.ReviseReasonID != null && AdgMgmtBE.ReviseReasonID > 0)
                {
                    ddlRevisionID.DataBind();
                    ddlRevisionID.Items.FindByValue(AdgMgmtBE.ReviseReasonID.ToString()).Selected = true;
                }
            }
        }

        if (AdgMgmtBE.AdjPendingRsnID != null)
        {
            ddPndReason.DataBind();
            //            ddPndReason.Items.FindByValue(AdgMgmtBE.AdjPendingRsnID.ToString()).Selected = true;
            AddInActiveLookupData(ref ddPndReason, AdgMgmtBE.AdjPendingRsnID.Value);
        }
        else
        {
            ddPndReason.SelectedIndex = -1;
        }
        txtAdjmtNumber.Text = AdgMgmtBE.prem_adjID.ToString();
        if (AdjStatusType != "0")
            txtAdjmtStuts.Text = AdjStatusType;

        if (AdgMgmtBE.ReviseReasonID != null)
        {
            ddlRevisionID.DataBind();
            //            ddlRevisionID.Items.FindByValue(AdgMgmtBE.ReviseReasonID.ToString()).Selected = true;
            AddInActiveLookupData(ref ddlRevisionID, AdgMgmtBE.ReviseReasonID.Value);
        }
        else
        {
            ddlRevisionID.SelectedIndex = -1;
        }
        if (AdgMgmtBE.VoidReasonID != null)
        {
            ddVoidReason.DataBind();
            //            ddVoidReason.Items.FindByValue(AdgMgmtBE.VoidReasonID.ToString()).Selected = true;
            AddInActiveLookupData(ref ddVoidReason, AdgMgmtBE.VoidReasonID.Value);
        }
        else
        {
            ddVoidReason.SelectedIndex = -1;
        }
        if (txtAdjmtStuts.Text == "CALC" && AdjStatusType != "0")
        {
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                btnCnclinvoice.Enabled = true;
        }
        else
        {
            btnCnclinvoice.Enabled = false;
        }

        if (txtAdjmtStuts.Text == "CALC" || txtAdjmtStuts.Text == "DRAFT-INVOICE")
        {
            ddPndReason.Enabled = true;
            ChkBkPendingInv.Enabled = true;
        }
        else
        {
            ddPndReason.Enabled = false;
            ChkBkPendingInv.Enabled = false;
        }
        if ((txtAdjmtStuts.Text != "FINAL INVOICE") && (txtAdjmtStuts.Text != "TRANSMITTED"))
        {
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                btnDetails.Enabled = true;
        }
        else
        {
            btnDetails.Enabled = false;
        }

        if ((txtAdjmtStuts.Text == "FINAL INVOICE" || txtAdjmtStuts.Text == "TRANSMITTED") && (AdgMgmtBE.VoidReasonIndc != true) && (AdgMgmtBE.ReviseReasonIndc != true))
        {
            if ((WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry))
            {

                if (CurrentAISUser.Role == GlobalConstants.ApplicationSecurityGroup.Manager || CurrentAISUser.Role == GlobalConstants.ApplicationSecurityGroup.SystemAdmin)
                {
                    btnVoidInv.Enabled = true;
                    ddVoidReason.Enabled = true;
                    chkBxVoidReasonInd.Enabled = true;
                }
                btnRevseInv.Enabled = true;
                chkBxReavisionReasonInd.Enabled = true;
                ddlRevisionID.Enabled = true;
            }
        }
        else
        {
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                btnlssinfo.Enabled = true;
            btnRevseInv.Enabled = false;
            chkBxReavisionReasonInd.Enabled = false;
            ddlRevisionID.Enabled = false;

            btnVoidInv.Enabled = false;
            ddVoidReason.Enabled = false;
            chkBxVoidReasonInd.Enabled = false;
        }
        // Logic for Enabling the Historical Check box based on requirements for Historical Adjustment
        bool flagHistorical = false;
        IList<PremiumAdjustmentBE> PREMBE = (new PremAdjustmentBS()).getpremAdjList();
        if (PREMBE.Count() > 0)
        {
            int? PREM_ADJ_PGM_ID = (PREMBE.Where(adj => adj.PREMIUM_ADJ_ID == AdgMgmtBE.prem_adjID).ToList())[0].PREM_ADJ_PGMID;
            //string PREM_NonPREM_Code = (PREMBE.Where(adj => adj.PREMIUM_ADJ_ID == AdgMgmtBE.prem_adjID).ToList())[0].PREM_NON_PREM_CD;
            //PREMBE = PREMBE.Where(prem => prem.HISTORICAL_ADJ_IND == true && prem.PREM_NON_PREM_CD == PREM_NonPREM_Code && prem.PREM_ADJ_PGMID == PREM_ADJ_PGM_ID && ((prem.ADJ_VOID_IND != true && txtAdjmtStuts.Text != "FINAL INVOICE" && txtAdjmtStuts.Text != "TRANSMITTED" && prem.PREMIUM_ADJ_ID != AdgMgmtBE.prem_adjID))).ToList();
            PREMBE = PREMBE.Where(prem => prem.HISTORICAL_ADJ_IND == true && prem.CUSTOMERID == AdgMgmtBE.custmrID && prem.VALN_DT == AdgMgmtBE.ValtnDate && prem.BROKER_ID == AdgMgmtBE.BROKER_ID && prem.BU_OFF_ID == AdgMgmtBE.BU_OFF_ID && prem.ADJ_VOID_IND != true && prem.ADJ_STS_TYP_ID != 347 && prem.PREMIUM_ADJ_ID != AdgMgmtBE.prem_adjID).ToList();
            if (PREMBE.Count() > 0)
                flagHistorical = true;
            if (PREM_ADJ_PGM_ID != null)
            {
                int? chkFirstAdj = (new ProgramPeriodsBS()).CheckFirstAdjustment(int.Parse(PREM_ADJ_PGM_ID.ToString()));
                if (chkFirstAdj > 0)
                    flagHistorical = true;
                else
                    flagHistorical = false;
            }
        }
        if ((txtAdjmtStuts.Text != "FINAL INVOICE" && txtAdjmtStuts.Text != "TRANSMITTED") && !flagHistorical)
        {
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            {
                chkHistrorical.Enabled = true;
                if (AdgMgmtBE.Rel_prem_adjID > 0)
                {
                    AdjustmentManagementBE AdgMgmtPriorBE = AdjMgmtBS.getPremMgmtRow(Convert.ToInt32(AdgMgmtBE.Rel_prem_adjID.ToString()));
                    if (AdgMgmtPriorBE.HistoricalIndc != true)
                        chkHistrorical.Enabled = false;
                }
            }
        }
        else
        {
            chkHistrorical.Enabled = false;
        }
        //10529 Bug Fix.
        if (AdgMgmtBE.Rel_prem_adjID > 0 && chkHistrorical.Enabled == true)
            chkHistrorical.Enabled = false;
        //login for when Upload to ZDW button should be enabled.
        if ((txtAdjmtStuts.Text != "CALC") &&
               ((txtAdjmtStuts.Text == "FINAL INVOICE") || (txtAdjmtStuts.Text == "DRAFT-INVOICE") || (txtAdjmtStuts.Text == "TRANSMITTED")))
        {
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                btnUploadZDW.Enabled = true;
        }
        else
        {
            btnUploadZDW.Enabled = false;
        }
        //Code for Restricting Revision for adjustment if there is open subsequent adjustment created
        // for this programperiod.
        if (AdgMgmtBE.ReviseReasonIndc != true && AdgMgmtBE.VoidReasonIndc != true && (txtAdjmtStuts.Text == "FINAL INVOICE" || txtAdjmtStuts.Text == "TRANSMITTED"))
        {
            IList<PremiumAdjustmentPeriodBE> PREADJPERD_List = (new PremiumAdjustmentPeriodBS()).GetProgramPeriods(AdgMgmtBE.prem_adjID);
            //            PREADJPERD_List = PREADJPERD_List.Where(perd => perd.PREM_NON_PREM_CODE == AdgMgmtBE.PREM_NON_PREM_CODE).ToList();
            if (PREADJPERD_List.Count() > 0)
            {
                int PREM_ADJ_PGM_ID = PREADJPERD_List[0].PREM_ADJ_PGM_ID;
                string premCode = PREADJPERD_List[0].PREM_NON_PREM_CODE;
                IList<PremiumAdjustmentBE> PremAdjList = (new PremAdjustmentBS()).getpremAdjList();
                PremAdjList = PremAdjList.Where(prem => (prem.ADJ_STS_TYP_ID != 347) && prem.ADJ_RRSN_IND != true && prem.ADJ_VOID_IND != true && (prem.FNL_INVC_NBR_TXT != null ? !prem.FNL_INVC_NBR_TXT.StartsWith("RTV") : prem.CUSTOMERID > 0) && prem.PREM_ADJ_PGMID == PREM_ADJ_PGM_ID && prem.CUSTOMERID == AdgMgmtBE.custmrID && prem.PREMIUM_ADJ_ID > AdgMgmtBE.prem_adjID).ToList();
                //PremAdjList = PremAdjList.Where(prem =>  (|| prem.ADJ_RRSN_IND == false || prem.ADJ_RRSN_IND == null)  ).ToList();
                //PREADJPERD_List = (new PremiumAdjustmentPeriodBS()).GetPREMADJPeriods(PREM_ADJ_PGM_ID);
                //PREADJPERD_List = PREADJPERD_List.Where(perd => perd.PREM_NON_PREM_CODE == premCode && perd.ADJ_STS_TYP_ID == 346  && perd.PREM_ADJ_ID > AdgMgmtBE.prem_adjID).ToList();
                if (PremAdjList.Count() > 0)
                {
                    btnRevseInv.Enabled = false;
                    btnVoidInv.Enabled = false;
                    chkBxReavisionReasonInd.Enabled = false;
                    chkBxVoidReasonInd.Enabled = false;
                    ddlRevisionID.Enabled = false;
                    ddVoidReason.Enabled = false;
                }
            }
        }
        if (AdgMgmtBE.ReviseReasonIndc == true)
        {
            PREMBE = (new PremAdjustmentBS()).getpremAdjList();
            PREMBE = PREMBE.Where(cdd => cdd.REL_PREM_ADJ_ID == AdgMgmtBE.prem_adjID).ToList();
            int count = PREMBE.Count();
            PREMBE = PREMBE.Where(cdd => cdd.ADJ_STS_TYP_ID == 347).ToList();
            int count1 = PREMBE.Count();
            if (count == count1 && count != 0)
            {
                chkBxReavisionReasonInd.Enabled = true;
                ddlRevisionID.Enabled = true;
                btnRevseInv.Enabled = true;
                if (CurrentAISUser.Role == GlobalConstants.ApplicationSecurityGroup.Manager || CurrentAISUser.Role == GlobalConstants.ApplicationSecurityGroup.SystemAdmin)
                {
                    ddVoidReason.Enabled = true;
                    chkBxVoidReasonInd.Enabled = true;
                    btnVoidInv.Enabled = true;
                }
            }
        }
        if (AdgMgmtBE.FinalInvoicenmr != null && AdgMgmtBE.FinalInvoicenmr.ToUpper().StartsWith("RTV"))
        {
            chkBxReavisionReasonInd.Enabled = false;
            chkBxVoidReasonInd.Enabled = false;
            ddlRevisionID.Enabled = false;
            ddVoidReason.Enabled = false;
            btnRevseInv.Enabled = false;
            btnVoidInv.Enabled = false;
        }

    }

    /// <summary>
    /// Select a single client from Adjustment management list to view the details below
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstAdjMgmtDtl_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
    {
        HtmlTableRow tr;
        //code for changing the previous selected row color to its original Color
        if (ViewState["SelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["SelectedIndex"]);
            tr = (HtmlTableRow)lstAdjMgmtDtl.Items[index].FindControl("trItemTemplate");
            tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
        }
        tr = (HtmlTableRow)lstAdjMgmtDtl.Items[e.NewSelectedIndex].FindControl("trItemTemplate");
        LinkButton lnk = (LinkButton)lstAdjMgmtDtl.Items[e.NewSelectedIndex].FindControl("lnkSelect");
        ViewState["SelectedIndex"] = e.NewSelectedIndex;
        //code for changing the selected row style to highlighted
        tr.Attributes["class"] = "SelectedItemTemplate";
        CheckNew = false;
        lstAdjMgmtDtl.Enabled = false;
        //function call to display the selected Adjustment Management Details
        Label AdjStuts = (Label)lstAdjMgmtDtl.Items[e.NewSelectedIndex].FindControl("lblAdjStatus");
        HiddenField hidAdjMagmtNumber = (HiddenField)lstAdjMgmtDtl.Items[e.NewSelectedIndex].FindControl("hdAdjMgmtNumb");
        HiddenField hidAdjMgmtID = (HiddenField)lstAdjMgmtDtl.Items[e.NewSelectedIndex].FindControl("HidAdjMgmtID");
        //Funtion to Display the details of the selected Adjustment Management Details
        ViewState["PREMADJID"] = hidAdjMgmtID.Value;
        //InvNo used for Uploading Documents to ZDW
        Label lblInvNumber = (Label)lstAdjMgmtDtl.Items[e.NewSelectedIndex].FindControl("lblInvNumber");
        ViewState["InvNumber"] = lblInvNumber.Text;
        /// Function to display the selected Adjustment Management details
        DisplayDetails(Convert.ToInt32(hidAdjMgmtID.Value), Convert.ToInt32(hidAdjMagmtNumber.Value), (AdjStuts.Text == null ? "0" : AdjStuts.Text));
        //pnlDetails.Enabled = bool.Parse(hidAdjMgmtID.Value);
        PnlSearchdtls.Enabled = false;
        pnlDetails.Visible = true;
        lblAdjustmentMgmt.Visible = true;
        lnkClose.Visible = true;
    }

    /// <summary>
    /// The method is used after the LBA details info listview control is bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstAdjMgmtDtl_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trItemTemplate");
            tr.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            tr.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";

        }
    }

    /// <summary>
    /// Close the Adjustment Details panel for a selected  Client
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        pnlDetails.Visible = false;

        PnlSearchdtls.Enabled = true;
        lblAdjustmentMgmt.Visible = false;
        lnkClose.Visible = false;
        lstAdjMgmtDtl.Enabled = true;
        HtmlTableRow tr;
        //code for changing the previous selected row color to its original Color
        if (ViewState["SelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["SelectedIndex"]);
            tr = (HtmlTableRow)lstAdjMgmtDtl.Items[index].FindControl("trItemTemplate");
            tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
        }
        SelectedValue = -1;
    }

    /// <summary>
    /// Cancel Selected Invoice - Change the flag of invoice status in Prem_Adj_STS table to CANCELLED status
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCanclInvoice_Click(object sender, EventArgs e)
    {
        int accountId = Convert.ToInt32(ddlAcctlist.SelectedValue);
        try
        {
            if (ViewState["SelectedIndex"] != null)
            {
                HiddenField hidAdjMagmtNumber = (HiddenField)lstAdjMgmtDtl.Items[Convert.ToInt32(ViewState["SelectedIndex"])].FindControl("hdAdjMgmtNumb");
                HiddenField hidAdjMgmtID = (HiddenField)lstAdjMgmtDtl.Items[Convert.ToInt32(ViewState["SelectedIndex"])].FindControl("HidAdjMgmtID");
                int? prior_adj_id = 0;
                objDC.Connection.Open();
                trans = objDC.Connection.BeginTransaction();
                objDC.Transaction = trans;
                var PremAdjBE = (from cdd in objDC.PREM_ADJs where cdd.prem_adj_id == Convert.ToInt32(hidAdjMgmtID.Value) select cdd).First();
                bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(AdgMgmtBEOld.UPDATE_DATE), Convert.ToDateTime(PremAdjBE.updt_dt));
                if (!Concurrency)
                    return;
                PremAdjBE.adj_can_ind = true;

                if (PremAdjBE.rel_prem_adj_id != null)
                {
                    prior_adj_id = PremAdjBE.rel_prem_adj_id;
                }
                // Code for generating CancelInvoice number
                string strInvoiceNum = "0000000000";
                PremAdjBE.fnl_invc_nbr_txt = "RTC01" + strInvoiceNum.Remove(0, Convert.ToInt32(hidAdjMgmtID.Value.Length)) + hidAdjMgmtID.Value.ToString();
                PremAdjBE.adj_sts_typ_id = GlobalConstants.AdjustmentStatus.Cancelled_Code;
                PremAdjBE.adj_sts_eff_dt = DateTime.Now;
                PremAdjBE.updt_dt = DateTime.Now;
                PremAdjBE.updt_user_id = CurrentAISUser.PersonID;
                CommunicationManagerServiceWse objJavaWS = new CommunicationManagerServiceWse();
                UsernameToken token = new UsernameToken(ConfigurationManager.AppSettings["ZDWUserName"].ToString(), ConfigurationManager.AppSettings["ZDWPassWord"].ToString(), PasswordOption.SendPlainText);
                objJavaWS.RequestSoapContext.Security.Timestamp.TtlInSeconds = 60;
                objJavaWS.RequestSoapContext.Security.Tokens.Add(token);
                objJavaWS.RequestSoapContext.Security.MustUnderstand = false;
                string strDocID = string.Empty;
                for (int i = 0; i < 6; i++)
                {
                    switch (i)
                    {
                        case 0:
                            strDocID = PremAdjBE.drft_intrnl_pdf_zdw_key_txt;
                            break;
                        case 1:
                            strDocID = PremAdjBE.drft_extrnl_pdf_zdw_key_txt;
                            break;
                        case 2:
                            strDocID = PremAdjBE.drft_cd_wrksht_pdf_zdw_key_txt;
                            break;
                        case 3:
                            strDocID = PremAdjBE.fnl_intrnl_pdf_zdw_key_txt;
                            break;
                        case 4:
                            strDocID = PremAdjBE.fnl_extrnl_pdf_zdw_key_txt;
                            break;
                        case 5:
                            strDocID = PremAdjBE.fnl_cd_wrksht_pdf_zdw_key_txt;
                            break;
                    }

                    if (strDocID != null)
                    {
                        objJavaWS.terminateDocument(strDocID);
                    }
                }
                PremAdjBE.drft_cd_wrksht_pdf_zdw_key_txt = null;
                PremAdjBE.drft_extrnl_pdf_zdw_key_txt = null;
                PremAdjBE.drft_intrnl_pdf_zdw_key_txt = null;
                PremAdjBE.fnl_cd_wrksht_pdf_zdw_key_txt = null;
                PremAdjBE.fnl_extrnl_pdf_zdw_key_txt = null;
                PremAdjBE.fnl_intrnl_pdf_zdw_key_txt = null;
                objDC.SubmitChanges();
                //To update the Prem_adj status to cancelled in prem_adj_sts Table
                PREM_ADJ_ST AdjMgmtStutsBE = new PREM_ADJ_ST()
                {
                    prem_adj_id = PremAdjBE.prem_adj_id,
                    crte_dt = DateTime.Now,
                    crte_user_id = CurrentAISUser.PersonID,
                    custmr_id = PremAdjBE.reg_custmr_id,
                    adj_sts_typ_id = GlobalConstants.AdjustmentStatus.Cancelled_Code,
                    eff_dt = DateTime.Now,
                };
                objDC.PREM_ADJ_STs.InsertOnSubmit(AdjMgmtStutsBE);
                objDC.SubmitChanges();
                //var AdjMgmtStutsBE = (from cdd in objDC.PREM_ADJ_STs where cdd.prem_adj_id == Convert.ToInt32(hidAdjMgmtID.Value) select cdd).First();
                //AdjMgmtStutsBE.adj_sts_typ_id = GlobalConstants.AdjustmentStatus.Cancelled_Code;
                //AdjMgmtStutsBE.updt_dt = DateTime.Now;
                //AdjMgmtStutsBE.updt_user_id = CurrentAISUser.PersonID;
                //objDC.SubmitChanges();
                txtAdjmtStuts.Text = "CANCELLED";
                //Calling CancelDriver for Posting to Aries
                string ErroMsg;
                ErroMsg = string.Empty;
                objDC.ModAdjCancelDriver(0, PremAdjBE.prem_adj_id, ref ErroMsg);
                if (ErroMsg == "")
                {
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                    (new ApplicationStatusLogBS()).WriteLog("AIS Adjustment Cancel Driver", "ERR", "Adjustment Cancel error", ErroMsg, accountId, CurrentAISUser.PersonID);
                    ShowError("Adjustment could not be Cancelled due to an error. Please check the error log for additional details");
                }
                //Code for Binding the Listview again.
                BindInvoicelistView();
                //Funtion to Display the details of the selected Adjustment Management Details
                DisplayDetails(Convert.ToInt32(hidAdjMgmtID.Value), Convert.ToInt32(hidAdjMagmtNumber.Value), (txtAdjmtStuts.Text == null ? "0" : txtAdjmtStuts.Text));
            }
        }
        catch (Exception ee)
        {
            trans.Rollback();
            (new ApplicationStatusLogBS()).WriteLog("AIS Adjustment Cancel Driver", "ERR", "Adjustment Cancel error", ee.Message, accountId, CurrentAISUser.PersonID);
            ShowError("Adjustment could not be Cancelled due to an error. Please check the error log for additional details");

        }
        finally
        {
            if (objDC.Connection.State == ConnectionState.Open)
                objDC.Connection.Close();
        }
    }

    /// <summary>
    /// User is rediected to Invoicing dashboard page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDetails_Click(object sender, EventArgs e)
    {
        HiddenField hidAdjMgmtID = (HiddenField)lstAdjMgmtDtl.Items[Convert.ToInt32(ViewState["SelectedIndex"])].FindControl("HidAdjMgmtID");
        DateTime valDate = DateTime.Parse(txtValutiondtlsDate.Text);
        AccountBE accountBE = (new AccountBS()).Retrieve(Int32.Parse(txtAcctNumber.Text));
        AISMasterEntities = new MasterEntities();
        AISMasterEntities.AdjusmentNumber = Convert.ToInt32(hidAdjMgmtID.Value);
        AISMasterEntities.AccountNumber = accountBE.CUSTMR_ID;
        AISMasterEntities.AccountName = accountBE.FULL_NM;
        AISMasterEntities.Bpnumber = accountBE.FINC_PTY_ID;
        AISMasterEntities.SSCGID = accountBE.SUPRT_SERV_CUSTMR_GP_ID;
        AISMasterEntities.ValuationDate = valDate;
        AISMasterEntities.PremiumAdjProgramID = hidAdjMgmtID.Value;
        Response.Redirect("~/AdjCalc/InvoicingDashboard.aspx?AcctNo=" + accountBE.CUSTMR_ID.ToString() + "&AcctNm=" + accountBE.FULL_NM.Trim());
    }

    /// <summary>
    /// User is redirected to Loss Info page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLossInfo_Click(object sender, EventArgs e)
    {

        HiddenField hidAdjMgmtID = (HiddenField)lstAdjMgmtDtl.Items[Convert.ToInt32(ViewState["SelectedIndex"])].FindControl("HidAdjMgmtID");
        DateTime valDate = DateTime.Parse(txtValutiondtlsDate.Text);
        AccountBE accountBE = (new AccountBS()).Retrieve(Int32.Parse(txtAcctNumber.Text));
        AISMasterEntities = new MasterEntities();
        AISMasterEntities.AccountNumber = accountBE.CUSTMR_ID;
        AISMasterEntities.AccountName = accountBE.FULL_NM;
        AISMasterEntities.Bpnumber = accountBE.FINC_PTY_ID;
        AISMasterEntities.SSCGID = accountBE.SUPRT_SERV_CUSTMR_GP_ID;
        AISMasterEntities.ValuationDate = valDate;
        AISMasterEntities.AdjustmentStatus = txtAdjmtStuts.Text;
        Label lblPGMID = (Label)lstAdjMgmtDtl.Items[Convert.ToInt32(ViewState["SelectedIndex"])].FindControl("lblPGMID");
        if (lblPGMID.Text != "")
        {
            AISMasterEntities.PremiumAdjProgramID = lblPGMID.Text;
        }
        else
        {
            ShowError("Program Period ID not available. unable to navigate to Loss Info page");
            return;
        }

        AISMasterEntities.AdjusmentNumber = Convert.ToInt32((hidAdjMgmtID.Value.ToString()));
        Session["refProgramPeriod"] = null;
        Session["refPremAdjPgmID"] = null;
        Session["Adjdtls"] = "Adjustmentdetails";
        Response.Redirect("~/Loss/LossInfo.aspx");
    }

    /// <summary>
    /// Save/Update the selected record only Set QC 20, Pending Indicator and Pending resaon
    /// can me edietd and saved here
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        AdjustmentManagementBS AdjmagmtBS = new AdjustmentManagementBS();

        try
        {
            AdgMgmtBE = (new AdjustmentManagementBS()).getPremMgmtRow(AdgMgmtBE.prem_adjID);
            bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(AdgMgmtBEOld.UPDATE_DATE), Convert.ToDateTime(AdgMgmtBE.UPDATE_DATE));
            if (!Concurrency)
                return;

            //((Convert.ToInt32(ddPndReason.SelectedValue) == 0 ? null :ddPndReason.SelectedValue.ToString()))
            if ((AdgMgmtBE.AdjPendingIndctor.ToString() == ChkBkPendingInv.Checked.ToString()) &&
                (AdgMgmtBE.AdjPendingRsnID.ToString() == (ddPndReason.SelectedValue == "0" ? "" : ddPndReason.SelectedItem.Value)) &&
                (AdgMgmtBE.ReviseReasonID.ToString() == (ddlRevisionID.SelectedValue == "0" ? "" : ddlRevisionID.SelectedItem.Value)) &&
                (AdgMgmtBE.VoidReasonID.ToString() == (ddVoidReason.SelectedValue == "0" ? "" : ddVoidReason.SelectedItem.Value)) &&
                (AdgMgmtBE.TwentyReqIND.ToString() == chkbxSetQc.Checked.ToString()) && (AdgMgmtBE.HistoricalIndc.ToString() == chkHistrorical.Checked.ToString()))
            {
                //ShowMessage("No information has been changed to Save");
            }
            else
            {
                if ((ChkBkPendingInv.Checked == true && ddPndReason.SelectedValue == "0") ||
                    (ChkBkPendingInv.Checked == false && ddPndReason.SelectedValue != "0"))
                {
                    ShowMessage("Please select a Pending reason if Pending Indicator is checked or Viseversa");
                }

                else
                {
                    if (CheckNew == false)
                    {
                        AdgMgmtBE.AdjPendingIndctor = (ChkBkPendingInv.Checked == true ? true : false);
                        AdgMgmtBE.TwentyReqIND = (chkbxSetQc.Checked == true ? true : false);
                        AdgMgmtBE.HistoricalIndc = (chkHistrorical.Checked == true ? true : false);
                        if (ddPndReason.SelectedIndex > 0)
                        {
                            AdgMgmtBE.AdjPendingRsnID = Convert.ToInt32(ddPndReason.SelectedItem.Value);
                        }
                        else
                        {
                            AdgMgmtBE.AdjPendingRsnID = null;
                        }
                        AdgMgmtBE.UPDATE_DATE = DateTime.Now;
                        AdgMgmtBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
                        bool Flag = AdjmagmtBS.Update(AdgMgmtBE);
                        if (Flag)
                            AdgMgmtBEOld = AdgMgmtBE;
                        //ShowConcurrentConflict(Flag, AdgMgmtBE.ErrorMessage);
                    }
                }
            }
        }
        catch (RetroBaseException ee)
        {
            ClearError();
            ShowError(ee.Message, ee);
        }
    }

    /// <summary>
    /// Documents are uploaded into ZDW
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUploadZDW_Click(object sender, EventArgs e)
    {
        if (uploadZDW.HasFile)
        {
            string str = hidfiletxt.Value;
            string strInvoiceNo = ViewState["InvNumber"].ToString();
            int fileSize = uploadZDW.PostedFile.ContentLength;
            //string strContentType = uploadZDW.PostedFile.ContentType;
            //string filename = uploadZDW.PostedFile.FileName + "_" + strInvoiceNo + "_" + System.DateTime.Now + "." + strContentType;
            string strFilePath = uploadZDW.PostedFile.FileName;
            string strExt = strFilePath.Substring(strFilePath.LastIndexOf("."));
            string filename = strInvoiceNo + DateTime.Now.ToString();
            filename = filename.Replace('/', '-');
            filename = filename.Replace(':', '-');
            filename = filename.Replace("  ", "");

            filename = filename + strExt;
            //get file as binary stream
            Stream fileStream = uploadZDW.PostedFile.InputStream;
            //create byte array to keep file as bytes
            byte[] bArray = new byte[fileSize];
            //load array from stream
            fileStream.Read(bArray, 0, fileSize);

            ElectronicDocument objDocument = new ElectronicDocument();
            DocumentContentElement[] arrDocContent = new DocumentContentElement[1];
            // Creating array of  Properties

            Property[] arrProperty = new Property[4];
            // For each dynamic property to be populated create Property object as follows
            Property objProperty1 = new Property();
            objProperty1.kind = "DocClass";
            objProperty1.theValue = "AISInvoice";
            Property objProperty2 = new Property();
            objProperty2.kind = "DocumentType";
            objProperty2.theValue = "48";
            Property objProperty3 = new Property();
            objProperty3.kind = "InvoiceNumber";
            objProperty3.theValue = strInvoiceNo;
            arrProperty[0] = objProperty1;
            arrProperty[1] = objProperty2;
            arrProperty[2] = objProperty3;

            objDocument.dynamicProperties = arrProperty;
            //Converting to byte array
            objDocument.typeName = filename;

            // Creating the document content element and populating the binary value attribute with the content of document 
            DocumentContentElement objDocContent = new DocumentContentElement();
            objDocContent.theBinaryValue = bArray; // Data is the byte stream of document content 

            arrDocContent[0] = objDocContent;
            objDocument.documentContentElement = arrDocContent;
            // Instantiating Java Web Services and invoking “establishdocument” method - passing document object as ref.
            // For authentication, credentials have to be passed through the SOAP Header by passing security token over the SOAP request. 
            CommunicationManagerServiceWse objJavaWS = new CommunicationManagerServiceWse();

            try
            {

                UsernameToken token = new UsernameToken(ConfigurationManager.AppSettings["ZDWUserName"].ToString(), ConfigurationManager.AppSettings["ZDWPassWord"].ToString(), PasswordOption.SendPlainText);
                objJavaWS.RequestSoapContext.Security.Timestamp.TtlInSeconds = 60;
                objJavaWS.RequestSoapContext.Security.Tokens.Add(token);
                objJavaWS.RequestSoapContext.Security.MustUnderstand = false;
                objJavaWS.establishDocument(ref objDocument);
                ShowError("Document(s) uploaded to ZDW Successfully");

            }

            catch (System.Web.Services.Protocols.SoapException ex)
            {
                new ApplicationStatusLogBS().WriteLog(0, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
                ShowError("ZDW Interface is not responding. Please try this action later.");
                return;
            }
        }
        // objDocument.typeName; 		// Name of document saved
        //string strReferenceID = objDocument.externalReference; 	// ID of document saved
    }

    /// <summary>
    /// Cancel in the middle of user modifying a record without saveying
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (ViewState["SelectedIndex"] != null)
        {
            //string AdjInvoiceStatus = txtAdjmtStuts.Text; 
            HiddenField hidAdjMagmtNumber = (HiddenField)lstAdjMgmtDtl.Items[Convert.ToInt32(ViewState["SelectedIndex"])].FindControl("hdAdjMgmtNumb");
            HiddenField hidAdjMgmtID = (HiddenField)lstAdjMgmtDtl.Items[Convert.ToInt32(ViewState["SelectedIndex"])].FindControl("HidAdjMgmtID");
            //(HiddenField)lstAdjMgmtDtl.Items[e.NewSelectedIndex].FindControl("HidAdjMgmtID");
            //Funtion to Display the details of the selected Adjustment Management Details
            DisplayDetails(Convert.ToInt32(hidAdjMgmtID.Value), Convert.ToInt32(hidAdjMagmtNumber.Value), (txtAdjmtStuts.Text == null ? "0" : txtAdjmtStuts.Text));
        }
    }

    /// <summary>
    /// Revise Selected Invoice - Change the flag of invoice status in Prem_Adj_STS table to Recise status
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRevisInvoice_Click(object sender, EventArgs e)
    {
        if (chkBxReavisionReasonInd.Checked == false && ddlRevisionID.SelectedIndex == 0)
        {
            string str = " Please check Revision Checkbox <br>";
            str += "<li> Please select Revision Reason";
            ShowError(str);
            return;
        }
        else if (chkBxReavisionReasonInd.Checked && ddlRevisionID.SelectedIndex == 0)
        {
            string str = "Please select Revision Reason";
            ShowError(str);
            return;
        }
        if (chkBxReavisionReasonInd.Checked == false && ddlRevisionID.SelectedIndex > 0)
        {
            string str = "Please check Revision Checkbox";
            ShowError(str);
            return;
        }
        int premAdjID = Convert.ToInt32(ViewState["PREMADJID"]);
        AdgMgmtBE = (new AdjustmentManagementBS()).getPremMgmtRow(premAdjID);
        bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(AdgMgmtBEOld.UPDATE_DATE), Convert.ToDateTime(AdgMgmtBE.UPDATE_DATE));
        if (!Concurrency)
            return;

        //Code for calling Adjustment Revision Driver
        string strError = (new ProgramPeriodsBS()).AdjustmentRevision(premAdjID, AdgMgmtBE.custmrID, CurrentAISUser.PersonID);
        if (strError == "")
        {
            AdgMgmtBE = (new AdjustmentManagementBS()).getPremMgmtRow(premAdjID);
            AdgMgmtBE.ReviseReasonID = Convert.ToInt32(ddlRevisionID.SelectedItem.Value);
            bool Flag = (new AdjustmentManagementBS()).Update(AdgMgmtBE);
            ShowConcurrentConflict(Flag, AdgMgmtBE.ErrorMessage);
            HiddenField hidAdjMagmtNumber = (HiddenField)lstAdjMgmtDtl.Items[Convert.ToInt32(ViewState["SelectedIndex"])].FindControl("hdAdjMgmtNumb");
            HiddenField hidAdjMgmtID = (HiddenField)lstAdjMgmtDtl.Items[Convert.ToInt32(ViewState["SelectedIndex"])].FindControl("HidAdjMgmtID");
            /// subprocedure for Adjustment details search
            SearchAdjData();
            /// Function to display the selected Adjustment Management details
            DisplayDetails(Convert.ToInt32(hidAdjMgmtID.Value), Convert.ToInt32(hidAdjMagmtNumber.Value), (txtAdjmtStuts.Text == null ? "0" : txtAdjmtStuts.Text));
            ShowError("Revision Completed Successfully");

        }
        else
        {
            ShowError(strError);
        }
    }

    /// <summary>
    /// Void Selected Invoice - Change the flag of invoice status in Prem_Adj_STS table to VOId status
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnVoidInvoice_Click(object sender, EventArgs e)
    {
        if (chkBxVoidReasonInd.Checked == false && ddVoidReason.SelectedIndex == 0)
        {
            string str = " Please check Void Checkbox <br>";
            str += "<li> Please select Void Reason";
            ShowError(str);
            return;
        }
        else if (chkBxVoidReasonInd.Checked && ddVoidReason.SelectedIndex == 0)
        {
            string str = "Please select Void Reason";
            ShowError(str);
            return;
        }
        if (chkBxVoidReasonInd.Checked == false && ddVoidReason.SelectedIndex > 0)
        {
            string str = "Please check Void Checkbox";
            ShowError(str);
            return;
        }
        int premAdjID = Convert.ToInt32(ViewState["PREMADJID"]);
        AdgMgmtBE = (new AdjustmentManagementBS()).getPremMgmtRow(premAdjID);
        bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(AdgMgmtBEOld.UPDATE_DATE), Convert.ToDateTime(AdgMgmtBE.UPDATE_DATE));
        if (!Concurrency)
            return;

        //Code for calling Adjustment Void Driver.
        string strError = (new ProgramPeriodsBS()).AdjustmentVoid(premAdjID, AdgMgmtBE.custmrID, CurrentAISUser.PersonID);
        if (strError == "")
        {

            AdgMgmtBE = (new AdjustmentManagementBS()).getPremMgmtRow(premAdjID);
            AdgMgmtBE.VoidReasonID = Convert.ToInt32(ddVoidReason.SelectedItem.Value);
            //Retrieving newly created adjustment Detials 
            PremiumAdjustmentBE PremAdjVoidBE = new PremiumAdjustmentBE();
            PremAdjVoidBE = new PremAdjustmentBS().getVoidedAdjustmentRow(premAdjID);
            generateVoidInvoice(PremAdjVoidBE.PREMIUM_ADJ_ID, PremAdjVoidBE.FINALINVNO);
            bool Flag = (new AdjustmentManagementBS()).Update(AdgMgmtBE);
            ShowConcurrentConflict(Flag, AdgMgmtBE.ErrorMessage);
            HiddenField hidAdjMagmtNumber = (HiddenField)lstAdjMgmtDtl.Items[Convert.ToInt32(ViewState["SelectedIndex"])].FindControl("hdAdjMgmtNumb");
            HiddenField hidAdjMgmtID = (HiddenField)lstAdjMgmtDtl.Items[Convert.ToInt32(ViewState["SelectedIndex"])].FindControl("HidAdjMgmtID");
            /// subprocedure for Adjustment details search
            SearchAdjData();
            /// Function to display the selected Adjustment Management details
            DisplayDetails(Convert.ToInt32(hidAdjMgmtID.Value), Convert.ToInt32(hidAdjMagmtNumber.Value), (txtAdjmtStuts.Text == null ? "0" : txtAdjmtStuts.Text));
            ShowError("Void Completed Successfully");
            //Response.Write("<script language='javascript'>alert('void has occurred. It will copy the original invoice entries that are being voided and reverse the sign and pass the record to the ARiES transmittal'); </script>");


        }
        else
        {
            ShowError(strError);
        }

    }
    /// <summary>
    /// Function to generate VoidInvoice Number
    /// </summary>
    /// <param name="iAdjNo"></param>
    /// <param name="strInvoiceNo"></param>
    public void generateVoidInvoice(int iAdjNo, string strInvoiceNo)
    {
        //Internal PDF Generation
        ReportDocument objMainInternal = new ReportDocument();
        objMainInternal.Load(Server.MapPath("\\Reports\\" + "rptVoidMasterReport" + ".rpt"));
        //External PDF Generation
        ReportDocument objMain = new ReportDocument();
        objMain.Load(Server.MapPath("\\Reports\\" + "rptVoidMasterReport" + ".rpt"));
        //Coding Work Sheet PDF Generation
        ReportDocument objMainCDWSheet = new ReportDocument();
        objMainCDWSheet.Load(Server.MapPath("\\Reports\\" + "rptVoidMasterReport" + ".rpt"));

        //Internal PDF Connections
        GenerateReportConnections(objMainInternal);
        //External PDF Connections
        GenerateReportConnections(objMain);
        //CDWSheet PDF Connections
        GenerateReportConnections(objMainCDWSheet);


        ParameterDiscreteValue prmAdjNo = new ParameterDiscreteValue();
        ParameterDiscreteValue prmFlag = new ParameterDiscreteValue();
        ParameterDiscreteValue prmERPInd = new ParameterDiscreteValue();
        ParameterDiscreteValue prmFlipSigns = new ParameterDiscreteValue();
        ParameterDiscreteValue prmRevFlag = new ParameterDiscreteValue();
        ParameterDiscreteValue prmInvNo = new ParameterDiscreteValue();
        ParameterDiscreteValue prmHistFlag = new ParameterDiscreteValue();
        prmAdjNo.Value = iAdjNo;
        prmFlag.Value = 2;
        prmERPInd.Value = false;//This is dummy variable
        prmFlipSigns.Value = true;
        prmRevFlag.Value = true;
        prmInvNo.Value = strInvoiceNo;
        prmHistFlag.Value = false;
        /*****************Setting Master Report Parameters Value Begin******************/
        objMainInternal.SetParameterValue("@ADJNO", prmAdjNo);
        objMainInternal.SetParameterValue("@FLAG", prmFlag);
        objMain.SetParameterValue("@ADJNO", prmAdjNo);
        objMain.SetParameterValue("@FLAG", prmFlag);
        objMainCDWSheet.SetParameterValue("@ADJNO", prmAdjNo);
        objMainCDWSheet.SetParameterValue("@FLAG", prmFlag);
        /*****************Setting Master Report Parameters Value Begin******************/
        //Iternal Master Report View or Suppress
        //IList<InvoiceExhibitBE> objDrftInternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(1);
        //for (int iCount = 0; iCount < objDrftInternalIlistBE.Count; iCount++)
        //{

        //    setMasterReportParameter(objMainInternal, objDrftInternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftInternalIlistBE[iCount].STS_IND), 1, Convert.ToChar(objDrftInternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

        //}
        //Iternal Master Report View or Suppress
        setMasterReportParameters(objMainInternal, "Internal", iAdjNo);
        //External Master Report View or Suppress
        setMasterReportParameters(objMain, "External", iAdjNo);
        //Coding Work Sheet Master Report View or Suppress
        setMasterReportParameters(objMainCDWSheet, "CDWSheet", iAdjNo);
        /*****************Setting Sub Reports Parameters Value Begin******************/
        //setInternalSubReportParameters(objMainInternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
        setSubReportParameters(objMainInternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
        setSubReportParameters(objMain, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
        setSubReportParameters(objMainCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
        /*****************Setting Sub Reports Parameters Value End******************/
        string strVal3 = ExportReport(objMainInternal, iAdjNo, strInvoiceNo, "Internal");
        string strVal1 = ExportReport(objMain, iAdjNo, strInvoiceNo, "External");
        string strVal2 = ExportReport(objMainCDWSheet, iAdjNo, strInvoiceNo, "CDWSheet");
        if (strVal1 != null || strVal1 != "")
        {
            ShowError(strVal1);
            return;
        }
        if (strVal2 != null || strVal2 != "")
        {
            ClearError();
            ShowError(strVal2);
            return;
        }
        if (strVal3 != null || strVal3 != "")
        {
            ClearError();
            ShowError(strVal3);
            return;
        }


    }
    private void setSubReportParameters(ReportDocument objMain, ParameterDiscreteValue prmAdjNo, ParameterDiscreteValue prmFlag, ParameterDiscreteValue prmFlipSigns, ParameterDiscreteValue prmInvNo, ParameterDiscreteValue prmRevFlag, ParameterDiscreteValue prmHistFlag)
    {
        /*****************Setting Sub Reports Parameters Value Begin******************/

        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjusInvoice.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjusInvoice.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjusInvoice.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjusInvoice.rpt");
        objMain.SetParameterValue("@CMTCATGID", 319, "srptAdjusInvoice.rpt");


        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSecInjFund.rpt");

        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptWorkerCompTaxAssessKentOreg.rpt");

        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("IsFlipSigns", true, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptResidualMarkSubCharge.rpt");

        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("IsFlipSigns", true, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCesarCodingWorksheet.rpt");

        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptARiEsPostingDetails.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptARiEsPostingDetails.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptARiEsPostingDetails.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptARiEsPostingDetails.rpt");


        /*****************Setting Sub Reports Parameters Value End******************/
    }

    private void GenerateReportConnections(ReportDocument objMain)
    {
        // Set the Database Connection String Properties for the report.
        common = new Common(this.GetType());
        AISBasePage objAISPage = new AISBasePage();
        ConnectionInfo conn = objAISPage.GetReportConnection();

        TableLogOnInfo tblLogonInfo;
        Tables tbls = objMain.Database.Tables;

        // Set the Database Login Info to all the tables used in the main report.
        foreach (CrystalDecisions.CrystalReports.Engine.Table tbl in tbls)
        {
            tblLogonInfo = tbl.LogOnInfo;
            tblLogonInfo.ConnectionInfo = conn;
            tbl.ApplyLogOnInfo(tblLogonInfo);
        }


        // Loop through all the sections and its objects to do the same for the subreports
        foreach (CrystalDecisions.CrystalReports.Engine.Section section in objMain.ReportDefinition.Sections)
        {

            // In each section we need to loop through all the reporting objects
            foreach (CrystalDecisions.CrystalReports.Engine.ReportObject reportObject in section.ReportObjects)
            {
                if (reportObject.Kind == ReportObjectKind.SubreportObject)
                {
                    SubreportObject subReport = (SubreportObject)reportObject;
                    ReportDocument subDocument = objMain.OpenSubreport(subReport.SubreportName);

                    foreach (CrystalDecisions.CrystalReports.Engine.Table table in subDocument.Database.Tables)
                    {
                        tblLogonInfo = table.LogOnInfo;
                        tblLogonInfo.ConnectionInfo = conn;
                        table.ApplyLogOnInfo(tblLogonInfo);
                    }
                }
            }
        }
    }

    public void setMasterReportParameters(ReportDocument objMain, string strReportType, int iAdjNo)
    {
        AdjustmentReviewCommentsBS objReportAvailable = new AdjustmentReviewCommentsBS();
        if (strReportType.ToUpper().Trim() == "EXTERNAL")
        {
            //Adjustment Invoice Exhibit
            if (objReportAvailable.IsReportAvialable(iAdjNo, 1, "SUMMARY INVOICE"))
            {
                objMain.SetParameterValue("SuppressSectionAdjInv", "View");
            }
            else
            {
                objMain.SetParameterValue("SuppressSectionAdjInv", "Suppress");
            }
            //Residual Market Subsidy Charge
            objMain.SetParameterValue("SuppressResidualMarkSubChange", "Suppress");
            //Workers Compensation Tax and Assessment Kentucky
            objMain.SetParameterValue("SuppressKYOR", "Suppress");
            //Adjustment of NY Second Injury Fund
            objMain.SetParameterValue("SuppressAdjNY", "Suppress");
            //Cesar Coding WorkSheet
            objMain.SetParameterValue("SuppressCesar", "Suppress");
            //Aries Posting Details
            objMain.SetParameterValue("SuppressAries", "Suppress");
        }
        else if (strReportType.ToUpper().Trim() == "INTERNAL")
        {
            //Adjustment Invoice Exhibit
            if (objReportAvailable.IsReportAvialable(iAdjNo, 1, "SUMMARY INVOICE"))
            {
                objMain.SetParameterValue("SuppressSectionAdjInv", "View");
            }
            else
            {
                objMain.SetParameterValue("SuppressSectionAdjInv", "Suppress");
            }
            //Residual Market Subsidy Charge
            if (objReportAvailable.IsReportAvialable(iAdjNo, 1, "RML"))
            {
                objMain.SetParameterValue("SuppressResidualMarkSubChange", "View");
            }
            else
            {
                objMain.SetParameterValue("SuppressResidualMarkSubChange", "Suppress");
            }
            //Workers Compensation Tax and Assessment Kentucky
            if (objReportAvailable.IsReportAvialable(iAdjNo, 1, "KY & OR TAXES"))
            {
                objMain.SetParameterValue("SuppressKYOR", "View");
            }
            else
            {
                objMain.SetParameterValue("SuppressKYOR", "Suppress");
            }
            //Adjustment of NY Second Injury Fund
            if (objReportAvailable.IsReportAvialable(iAdjNo, 1, "NY-SIF"))
            {
                objMain.SetParameterValue("SuppressAdjNY", "View");
            }
            else
            {
                objMain.SetParameterValue("SuppressAdjNY", "Suppress");
            }
            //Cesar Coding WorkSheet
            if (objReportAvailable.IsReportAvialable(iAdjNo, 1, "CESAR CODING WORKSHEET"))
            {
                objMain.SetParameterValue("SuppressCesar", "View");
            }
            else
            {
                objMain.SetParameterValue("SuppressCesar", "Suppress");
            }
            //Aries Posting Details
            if (objReportAvailable.IsReportAvialable(iAdjNo, 1, "ARIES POSTING DETAILS"))
            {
                objMain.SetParameterValue("SuppressAries", "View");
            }
            else
            {
                objMain.SetParameterValue("SuppressAries", "Suppress");
            }


        }
        else
        {
            //Adjustment Invoice Exhibit
            objMain.SetParameterValue("SuppressSectionAdjInv", "Suppress");
            //Residual Market Subsidy Charge
            if (objReportAvailable.IsReportAvialable(iAdjNo, 1, "RML"))
            {
                objMain.SetParameterValue("SuppressResidualMarkSubChange", "View");
            }
            else
            {
                objMain.SetParameterValue("SuppressResidualMarkSubChange", "Suppress");
            }
            //Workers Compensation Tax and Assessment Kentucky
            if (objReportAvailable.IsReportAvialable(iAdjNo, 1, "KY & OR TAXES"))
            {
                objMain.SetParameterValue("SuppressKYOR", "View");
            }
            else
            {
                objMain.SetParameterValue("SuppressKYOR", "Suppress");
            }
            //Adjustment of NY Second Injury Fund
            if (objReportAvailable.IsReportAvialable(iAdjNo, 1, "NY-SIF"))
            {
                objMain.SetParameterValue("SuppressAdjNY", "View");
            }
            else
            {
                objMain.SetParameterValue("SuppressAdjNY", "Suppress");
            }
            //Cesar Coding WorkSheet
            if (objReportAvailable.IsReportAvialable(iAdjNo, 1, "CESAR CODING WORKSHEET"))
            {
                objMain.SetParameterValue("SuppressCesar", "View");
            }
            else
            {
                objMain.SetParameterValue("SuppressCesar", "Suppress");
            }
            //Aries Posting Details
            objMain.SetParameterValue("SuppressAries", "Suppress");
        }


    }

    public string ExportReport(ReportDocument objMain, int intAdjNo, string strInvoiceNo, string strReportType)
    {

        string strMessage = string.Empty;
        try
        {
            //filename = filename.Replace('/', '-');
            //filename = filename.Replace(' ', '-');
            //filename = filename.Replace(':', '-');

            //string strZDWkey = new PremAdjustmentBS().getZDWKey(intAdjNo, cFlag, intFlag);
            // Instantiating Electronic document object and array of Document Content element of electronic document.

            ElectronicDocument objDocument = new ElectronicDocument();
            DocumentContentElement[] arrDocContent = new DocumentContentElement[1];
            // Creating array of  Properties
            //Property[] arrProperty;
            //if (strZDWkey == "" || strZDWkey == null)
            //{
            //    arrProperty = new Property[4];
            //}
            //else
            //{
            //    arrProperty = new Property[2];
            //}
            Property[] arrProperty = new Property[4];
            // For each dynamic property to be populated create Property object as follows
            Property objProperty1 = new Property();
            objProperty1.kind = "DocClass";
            objProperty1.theValue = "AISInvoice";
            Property objProperty2 = new Property();
            objProperty2.kind = "DocumentType";
            objProperty2.theValue = "48";
            Property objProperty3 = new Property();
            objProperty3.kind = "InvoiceNumber";
            objProperty3.theValue = strInvoiceNo;
            arrProperty[0] = objProperty1;
            arrProperty[1] = objProperty2;
            arrProperty[2] = objProperty3;

            //if (strZDWkey == "" || strZDWkey == null)
            //{
            //    arrProperty[0] = objProperty1;
            //    arrProperty[1] = objProperty2;
            //    arrProperty[2] = objProperty3;
            //}
            //else
            //{
            //    arrProperty[0] = objProperty2;
            //    arrProperty[1] = objProperty3;
            //}

            objDocument.dynamicProperties = arrProperty;
            //Converting to byte array
            objDocument.typeName = "VoidInvoice" + "_" + strInvoiceNo + ".pdf";

            Stream st;
            st = objMain.ExportToStream(ExportFormatType.PortableDocFormat);
            byte[] arr = new byte[st.Length];
            st.Read(arr, 0, (int)st.Length);


            // Creating the document content element and populating the binary value attribute with the content of document 
            DocumentContentElement objDocContent = new DocumentContentElement();
            objDocContent.theBinaryValue = arr; // Data is the byte stream of document content 

            arrDocContent[0] = objDocContent;
            objDocument.documentContentElement = arrDocContent;
            // Instantiating Java Web Services and invoking “establishdocument” method - passing document object as ref.
            // For authentication, credentials have to be passed through the SOAP Header by passing security token over the SOAP request. 
            CommunicationManagerServiceWse objJavaWS = new CommunicationManagerServiceWse();

            try
            {

                UsernameToken token = new UsernameToken(ConfigurationManager.AppSettings["ZDWUserName"].ToString(), ConfigurationManager.AppSettings["ZDWPassWord"].ToString(), PasswordOption.SendPlainText);
                objJavaWS.RequestSoapContext.Security.Timestamp.TtlInSeconds = 60;
                objJavaWS.RequestSoapContext.Security.Tokens.Add(token);
                objJavaWS.RequestSoapContext.Security.MustUnderstand = false;
                //if (strZDWkey == "" || strZDWkey == null)
                objJavaWS.establishDocument(ref objDocument);
                //else
                //{
                //    objDocument.externalReference = strZDWkey;
                //    objJavaWS.modifyDocument(ref objDocument);
                //}


            }

            catch (System.Web.Services.Protocols.SoapException ex)
            {
                new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
                return ("ZDW Interface is not responding. Please try this action later.");
            }

            // objDocument.typeName; 		// Name of document saved
            string strReferenceID = objDocument.externalReference;
            // Code to Save ID of document generated
            PremiumAdjustmentBE premAdjBE = new PremAdjustmentBS().getPremiumAdjustmentRow(intAdjNo);

            if (strReportType.ToUpper().Trim() == "EXTERNAL")
            {

                premAdjBE.DRFT_EXTRNL_PDF_ZDW_KEY_TXT = strReferenceID;
                premAdjBE.FNL_EXTRNL_PDF_ZDW_KEY_TXT = strReferenceID;

            }
            else if (strReportType.ToUpper().Trim() == "INTERNAL")
            {
                premAdjBE.DRFT_INTRNL_PDF_ZDW_KEY_TXT = strReferenceID;
                premAdjBE.FNL_INTRNL_PDF_ZDW_KEY_TXT = strReferenceID;
            }
            else
            {

                premAdjBE.DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT = strReferenceID;
                premAdjBE.FNL_CD_WRKSHT_PDF_ZDW_KEY_TXT = strReferenceID;
            }
            bool result = new PremAdjustmentBS().Update(premAdjBE);
            if (result)
            {
                return strMessage;
            }
            else
            {
                new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Unable to Save", CurrentAISUser.PersonID);
                return  "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
            }


        }
        catch (Exception ex)
        {
            new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
            strMessage="ZDW Interface is not responding. Please try this action later.";
            return strMessage;
        }

    }

    /// <summary>
    /// Binds the list view with the selected criteria data 
    /// </summary>
    private void BindInvoicelistView()
    {
        IList<AdjustmentManagementBE> AdjManagementLst = new List<AdjustmentManagementBE>();

        this.lstAdjMgmtDtl.Visible = true;
        ViewState["SelectedIndex"] = 0;
        AdjMgmtlst = null;
        DropDownList ddMastActAcctlst = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        if (ddMastActAcctlst.SelectedValue == "")
        {
            ClearError();
            ShowError("Please select Account");
            return;
        }
        AdjMgmtlst = new AdjustmentManagementBS().getAdjManagement(
            ((ddMastActAcctlst.Items.FindByValue("0").Selected == true) ? 0 : Convert.ToInt32(ddMastActAcctlst.SelectedValue)),
            ((ddAdjStatus.Items.FindByValue("0").Selected == true) ? 0 : Convert.ToInt32(ddAdjStatus.SelectedValue)),
            ((txtInvoiceNmber.Text.Trim().Length == 0) ? String.Empty : txtInvoiceNmber.Text),
            ((ddUserLst.Items.FindByValue("0").Selected == true) ? 0 : Convert.ToInt32(ddUserLst.SelectedValue)),
            //(DateTime.Parse(txtInvoiceDateFrm.Text)), (DateTime.Parse(txtInvoiceDateTo.Text.ToString())), (DateTime.Parse(txtValuationDate.Text.ToString())));
            ((txtInvoiceDateFrm.Text.ToString() == "") ? Convert.ToDateTime(null) : Convert.ToDateTime(txtInvoiceDateFrm.Text)),
            ((txtInvoiceDateTo.Text.ToString() == "") ? Convert.ToDateTime(null) : Convert.ToDateTime(txtInvoiceDateTo.Text)),
            ((txtValuationDate.Text.ToString() == "") ? Convert.ToDateTime(null) : Convert.ToDateTime(txtValuationDate.Text)));
        this.lstAdjMgmtDtl.DataSource = AdjMgmtlst;
        this.lstAdjMgmtDtl.DataBind();

        if (AdjMgmtlst.Count > 0)
        {

        }
        else
        {
            ClearError();
            string strMessage = "No Record(s) found...!";
            ShowError(strMessage);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void generateRevisionInvoice()
    {
        //Internal/External PDF Generation
        ReportDocument objMainInternal = new ReportDocument();
        objMainInternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));
        //External PDF Generation
        ReportDocument objMainExternal = new ReportDocument();
        objMainExternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));
        //Coding Work Sheet PDF Generation
        ReportDocument objMainRevisionCDWSheet = new ReportDocument();
        objMainRevisionCDWSheet.Load(Server.MapPath("\\Reports\\" + "rptRevisionMasterReport" + ".rpt"));

        //Internal PDF Connections
        GenerateReportConnections(objMainInternal);
        //External PDF Connections
        GenerateReportConnections(objMainExternal);
        //CDWSheet PDF Connections


    }

}
