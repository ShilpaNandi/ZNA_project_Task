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
using ZurichNA.AIS.WebSite.ZDWJavaWS_CAD;
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
using SSG = SpreadsheetGear;

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
        //get { return (AdjustmentManagementBE)Session["AdgMgmtBE"]; }
        //set { Session["AdgMgmtBE"] = value; }
        get { return (AdjustmentManagementBE)RetrieveObjectFromSessionUsingWindowName("AdgMgmtBE"); }
        set { SaveObjectToSessionUsingWindowName("AdgMgmtBE", value); }
    }
    private AdjustmentManagementBE AdgMgmtBEOld
    {
        //get { return (AdjustmentManagementBE)Session["AdgMgmtBEOld"]; }
        //set { Session["AdgMgmtBEOld"] = value; }
        get { return (AdjustmentManagementBE)RetrieveObjectFromSessionUsingWindowName("AdgMgmtBEOld"); }
        set { SaveObjectToSessionUsingWindowName("AdgMgmtBEOld", value); }
    }
    IList<AdjustmentManagementBE> AdjMgmtlst
    {
        get
        {
            //if (Session["AdjMgmtlst"] == null)
            //    Session["AdjMgmtlst"] = new List<AdjustmentManagementBE>();
            //return (IList<AdjustmentManagementBE>)Session["AdjMgmtlst"];
            if (RetrieveObjectFromSessionUsingWindowName("AdjMgmtlst") == null)
                SaveObjectToSessionUsingWindowName("AdjMgmtlst", new List<AdjustmentManagementBE>());
            return (IList<AdjustmentManagementBE>)RetrieveObjectFromSessionUsingWindowName("AdjMgmtlst");
        }
        set
        {
            //Session["AdjMgmtlst"] = value;
            SaveObjectToSessionUsingWindowName("AdjMgmtlst", value);
        }
    }
    protected void Page_PreInit(object sender, EventArgs e)
    {
        ScriptManager sm = (ScriptManager)Master.FindControl("ScriptManager1");
        sm.EnablePartialRendering = false;
        //UpdatePanel upMaster = (UpdatePanel)Master.FindControl("tsttt");
        ////upMaster.ChildrenAsTriggers = false;
        ////upMaster.UpdateMode = UpdatePanelUpdateMode.Conditional;
        ////PostBackTrigger PST = new PostBackTrigger();
        //AsyncPostBackTrigger PST = new AsyncPostBackTrigger();
        //PST.ControlID = btnUploadOk.UniqueID.ToString();
        //upMaster.Triggers.Add(PST);
    }


    /// <summary>
    /// Page load event Loads the AdjustmentMgmt page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        //ScriptManager.GetCurrent(this).RegisterPostBackControl(btnUploadOk);
        //UpdatePanel upMaster = (UpdatePanel)Master.FindControl("tsttt");
        //upMaster.ChildrenAsTriggers = false;
        //upMaster.UpdateMode = UpdatePanelUpdateMode.Conditional;
        //PostBackTrigger PST = new PostBackTrigger();
        //PST.ControlID = btnUploadOk.UniqueID.ToString();
        //upMaster.Triggers.Add(PST);
        // ClientScript.RegisterStartupScript(this.GetType(), "key", "pageLoad();", true);
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
        //SR-321581
        hidLaunch.Value = "0";
        AdjustmentManagementBS AdjMgmtBS = new AdjustmentManagementBS();
        AdgMgmtBE = AdjMgmtBS.getPremMgmtRow(PremAdjMgmtID);

        AdgMgmtBEOld = AdgMgmtBE;
        txtAcctNumber.Text = AdgMgmtBE.custmrID.ToString();
        txtAcctName.Text = AdgMgmtBE.CustomerFullName;
        //SR-321581
        AccountBE accountBE = (new AccountBS()).getAccount(AdgMgmtBE.custmrID);
        if (accountBE.FINC_PTY_ID == "" || accountBE.FINC_PTY_ID == null)
        {
            hidLaunch.Value = "1";
            AISMasterEntities = new MasterEntities();
            AISMasterEntities.AccountNumber = AdgMgmtBE.custmrID;
            AISMasterEntities.AccountName = AdgMgmtBE.CustomerFullName;
        }

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
        //SR-321581
        hidLaunch.Value = "0";
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
                InvoiceDriverBS invDriverBS = new InvoiceDriverBS();
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
                bool IsCanadaAcct = (new AccountBS()).IsCanadaAccount(PremAdjBE.prem_adj_id);

                // Phase -3 
                int prem_adj_id = 0;
                prem_adj_id = PremAdjBE.prem_adj_id;
                string DRFT_PS_WRKSHT_PDF_ZDW_KEY_TXT = string.Empty;
                string fnl_PS_WRKSHT_PDF_ZDW_KEY_TXT = string.Empty;
                DataTable dtProgrSummZDWKey = invDriverBS.GetProgramSummary_ZDW_Info(prem_adj_id);
                if (dtProgrSummZDWKey != null)
                {
                    if (dtProgrSummZDWKey.Rows.Count > 0)
                    {
                        DRFT_PS_WRKSHT_PDF_ZDW_KEY_TXT = dtProgrSummZDWKey.Rows[0].Field<string>("drft_Progsumm_spreadsheet_zdw_key_txt");
                        fnl_PS_WRKSHT_PDF_ZDW_KEY_TXT = dtProgrSummZDWKey.Rows[0].Field<string>("fnl_Progsumm_spreadsheet_zdw_key_txt");
                    }
                }

                if (IsCanadaAcct)
                {
                    ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.CommunicationManagerService objJavaWS = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.CommunicationManagerService();
                    UsernameToken token = new UsernameToken(ConfigurationManager.AppSettings["ZDWUserName"].ToString(), ConfigurationManager.AppSettings["ZDWPassWord"].ToString(), PasswordOption.SendPlainText);
                    objJavaWS.RequestSoapContext.Security.Timestamp.TtlInSeconds = 60;
                    objJavaWS.RequestSoapContext.Security.Tokens.Add(token);
                    objJavaWS.RequestSoapContext.Security.MustUnderstand = false;
                    string strDocID = string.Empty;
                    for (int i = 0; i < 8; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                strDocID = PremAdjBE.drft_intrnl_pdf_zdw_key_txt == null ? null : "ZDW_DOC04." + PremAdjBE.drft_intrnl_pdf_zdw_key_txt;
                                break;
                            case 1:
                                strDocID = PremAdjBE.drft_extrnl_pdf_zdw_key_txt == null ? null : "ZDW_DOC04." + PremAdjBE.drft_extrnl_pdf_zdw_key_txt;
                                break;
                            case 2:
                                strDocID = PremAdjBE.drft_cd_wrksht_pdf_zdw_key_txt == null ? null : "ZDW_DOC04." + PremAdjBE.drft_cd_wrksht_pdf_zdw_key_txt;
                                break;
                            case 3:
                                strDocID = PremAdjBE.fnl_intrnl_pdf_zdw_key_txt == null ? null : "ZDW_DOC04." + PremAdjBE.fnl_intrnl_pdf_zdw_key_txt;
                                break;
                            case 4:
                                strDocID = PremAdjBE.fnl_extrnl_pdf_zdw_key_txt == null ? null : "ZDW_DOC04." + PremAdjBE.fnl_extrnl_pdf_zdw_key_txt;
                                break;
                            case 5:
                                strDocID = PremAdjBE.fnl_cd_wrksht_pdf_zdw_key_txt == null ? null : "ZDW_DOC04." + PremAdjBE.fnl_cd_wrksht_pdf_zdw_key_txt;
                                break;
                            case 6:
                                strDocID = DRFT_PS_WRKSHT_PDF_ZDW_KEY_TXT == null ? null : "ZDW_DOC04." + DRFT_PS_WRKSHT_PDF_ZDW_KEY_TXT;
                                break;
                            case 7:
                                strDocID = fnl_PS_WRKSHT_PDF_ZDW_KEY_TXT == null ? null : "ZDW_DOC04." + fnl_PS_WRKSHT_PDF_ZDW_KEY_TXT;
                                break;
                        }

                        if (strDocID != null)
                        {
                            objJavaWS.terminateDocument(strDocID);
                        }
                    }
                }
                else
                {
                    CommunicationManagerServiceWse objJavaWS = new CommunicationManagerServiceWse();
                    UsernameToken token = new UsernameToken(ConfigurationManager.AppSettings["ZDWUserName"].ToString(), ConfigurationManager.AppSettings["ZDWPassWord"].ToString(), PasswordOption.SendPlainText);
                    objJavaWS.RequestSoapContext.Security.Timestamp.TtlInSeconds = 60;
                    objJavaWS.RequestSoapContext.Security.Tokens.Add(token);
                    objJavaWS.RequestSoapContext.Security.MustUnderstand = false;
                    string strDocID = string.Empty;
                    for (int i = 0; i < 8; i++)
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
                            case 6:
                                strDocID = DRFT_PS_WRKSHT_PDF_ZDW_KEY_TXT;
                                break;
                            case 7:
                                strDocID = fnl_PS_WRKSHT_PDF_ZDW_KEY_TXT;
                                break;
                        }

                        if (strDocID != null && strDocID != string.Empty)
                        {
                            objJavaWS.terminateDocument(strDocID);
                        }
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

                DataTable dtCancelPrgmSummZDW = invDriverBS.ProgramSummary_Cancel_ZDW_Info(PremAdjBE.prem_adj_id);

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
        //Response.Redirect("~/AdjCalc/InvoicingDashboard.aspx?AcctNo=" + accountBE.CUSTMR_ID.ToString() + "&AcctNm=" + accountBE.FULL_NM.Trim());
        ResponseRedirect("~/AdjCalc/InvoicingDashboard.aspx?AcctNo=" + accountBE.CUSTMR_ID.ToString() + "&AcctNm=" + accountBE.FULL_NM.Trim());
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
        //Session["refProgramPeriod"] = null;
        //Session["refPremAdjPgmID"] = null;
        //Session["Adjdtls"] = "Adjustmentdetails";
        SaveObjectToSessionUsingWindowName("refProgramPeriod", null);
        SaveObjectToSessionUsingWindowName("refPremAdjPgmID", null);
        SaveObjectToSessionUsingWindowName("Adjdtls", "Adjustmentdetails");
        //Response.Redirect("~/Loss/LossInfo.aspx");
        ResponseRedirect("~/Loss/LossInfo.aspx");
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

            bool IsCanadaAcct = (new AccountBS()).IsCanadaAccount(strInvoiceNo);

            if (IsCanadaAcct)
            {
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.ElectronicDocument objDocument = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.ElectronicDocument();
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement[] arrDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement[1];
                // Creating array of  Properties

                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property[] arrProperty = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property[4];
                // For each dynamic property to be populated create Property object as follows
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property objProperty1 = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property();
                objProperty1.kind = "DocClass";
                objProperty1.theValue = "ZDW_DOC04.AISInvoice";
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property objProperty2 = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property();
                objProperty2.kind = "DocumentType";
                objProperty2.theValue = "48";
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property objProperty3 = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property();
                objProperty3.kind = "InvoiceNumber";
                objProperty3.theValue = strInvoiceNo;
                arrProperty[0] = objProperty1;
                arrProperty[1] = objProperty2;
                arrProperty[2] = objProperty3;

                objDocument.dynamicProperties = arrProperty;
                //Converting to byte array
                objDocument.typeName = filename;

                // Creating the document content element and populating the binary value attribute with the content of document 
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement objDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement();
                objDocContent.theBinaryValue = bArray; // Data is the byte stream of document content 

                arrDocContent[0] = objDocContent;
                objDocument.documentContentElement = arrDocContent;
                // Instantiating Java Web Services and invoking “establishdocument” method - passing document object as ref.
                // For authentication, credentials have to be passed through the SOAP Header by passing security token over the SOAP request. 
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.CommunicationManagerService objJavaWS = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.CommunicationManagerService();

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
            else
            {
                ZurichNA.AIS.WebSite.ZDWJavaWS.ElectronicDocument objDocument = new ZurichNA.AIS.WebSite.ZDWJavaWS.ElectronicDocument();
                ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement[] arrDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement[1];
                // Creating array of  Properties

                ZurichNA.AIS.WebSite.ZDWJavaWS.Property[] arrProperty = new ZurichNA.AIS.WebSite.ZDWJavaWS.Property[4];
                // For each dynamic property to be populated create Property object as follows
                ZurichNA.AIS.WebSite.ZDWJavaWS.Property objProperty1 = new ZurichNA.AIS.WebSite.ZDWJavaWS.Property();
                objProperty1.kind = "DocClass";
                objProperty1.theValue = "AISInvoice";
                ZurichNA.AIS.WebSite.ZDWJavaWS.Property objProperty2 = new ZurichNA.AIS.WebSite.ZDWJavaWS.Property();
                objProperty2.kind = "DocumentType";
                objProperty2.theValue = "48";
                ZurichNA.AIS.WebSite.ZDWJavaWS.Property objProperty3 = new ZurichNA.AIS.WebSite.ZDWJavaWS.Property();
                objProperty3.kind = "InvoiceNumber";
                objProperty3.theValue = strInvoiceNo;
                arrProperty[0] = objProperty1;
                arrProperty[1] = objProperty2;
                arrProperty[2] = objProperty3;

                objDocument.dynamicProperties = arrProperty;
                //Converting to byte array
                objDocument.typeName = filename;

                // Creating the document content element and populating the binary value attribute with the content of document 
                ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement objDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement();
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

        //Added the following validation to check the program enabled or not for selected account
        //it will not allow by showing the following  message
        //Program periods have been disabled on this adjustment. Please enable periods prior to voiding.
        bool bldisable = (new AdjustmentManagementBS()).CheckDisbaledProgramPeriods(premAdjID);
        if (!bldisable)
        {
            string str = "You are attempting to do a revision on a period that has been disabled.  Please enable, then process revision.";
            ShowError(str);
            return;
        }

        bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(AdgMgmtBEOld.UPDATE_DATE), Convert.ToDateTime(AdgMgmtBE.UPDATE_DATE));
        if (!Concurrency)
            return;

        //Code for calling Adjustment Revision Driver
        try
        {
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
        catch (Exception ee)
        {
            bool status = (new LSICustomersBS()).CheckLSIPermissions();
            if (!status)
                ShowError("LSI Interface is not responding. Please contact business support group for resolution.");
            else
                ShowError("Unable to process revision. Please contact business support group for resolution.");
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

        //Added the following validation as part of the TFS bug 11812 permanent fix.
        //When ever the user tries to void the adjustment by disabling the program periods under it
        //it will not allow by showing the following  message
        //Program periods have been disabled on this adjustment. Please enable periods prior to voiding.
        bool bldisable = (new AdjustmentManagementBS()).CheckDisbaledProgramPeriods(premAdjID);
        if (!bldisable)
        {
            string str = "You are attempting to do a void on a period that has been disabled.  Please enable, then void.";
            ShowError(str);
            return;
        }

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
        string strVal4 = ExportReport(null, iAdjNo, strInvoiceNo, "ProgramSummary");
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
        if (strVal4 != null || strVal4 != "")
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


        //objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjNYSecInjFund.rpt");
        //objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSecInjFund.rpt");
        //objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjNYSecInjFund.rpt");
        //objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptAdjNYSecInjFund.rpt");
        //objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSecInjFund.rpt");

        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptSurchargesAssessments.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptSurchargesAssessments.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptSurchargesAssessments.rpt");
        objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptSurchargesAssessments.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptSurchargesAssessments.rpt");

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
            //Retro Premium Based Surcharges & Assessments
            objMain.SetParameterValue("SuppressSurcharges", "Suppress");

            //Adjustment of NY Second Injury Fund
            //objMain.SetParameterValue("SuppressAdjNY", "Suppress");
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
            //Retro Premium Based Surcharges & Assessments
            if (objReportAvailable.IsReportAvialable(iAdjNo, 1, "Retro Premium Based Surcharges & Assessments"))
            {
                objMain.SetParameterValue("SuppressSurcharges", "View");
            }
            else
            {
                objMain.SetParameterValue("SuppressSurcharges", "Suppress");
            }
            //Adjustment of NY Second Injury Fund
            //if (objReportAvailable.IsReportAvialable(iAdjNo, 1, "NY-SIF"))
            //{
            //    objMain.SetParameterValue("SuppressAdjNY", "View");
            //}
            //else
            //{
            //    objMain.SetParameterValue("SuppressAdjNY", "Suppress");
            //}
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
            //Retro Premium Based Surcharges & Assessments
            if (objReportAvailable.IsReportAvialable(iAdjNo, 1, "Retro Premium Based Surcharges & Assessments"))
            {
                objMain.SetParameterValue("SuppressSurcharges", "View");
            }
            else
            {
                objMain.SetParameterValue("SuppressSurcharges", "Suppress");
            }
            //Adjustment of NY Second Injury Fund
            //if (objReportAvailable.IsReportAvialable(iAdjNo, 1, "NY-SIF"))
            //{
            //    objMain.SetParameterValue("SuppressAdjNY", "View");
            //}
            //else
            //{
            //    objMain.SetParameterValue("SuppressAdjNY", "Suppress");
            //}
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
            bool IsCanadaAcct = (new AccountBS()).IsCanadaAccount(intAdjNo);
            if (IsCanadaAcct)
            {
                //filename = filename.Replace('/', '-');
                //filename = filename.Replace(' ', '-');
                //filename = filename.Replace(':', '-');

                //string strZDWkey = new PremAdjustmentBS().getZDWKey(intAdjNo, cFlag, intFlag);
                // Instantiating Electronic document object and array of Document Content element of electronic document.

                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.ElectronicDocument objDocument = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.ElectronicDocument();
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement[] arrDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement[1];
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
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property[] arrProperty = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property[4];
                // For each dynamic property to be populated create Property object as follows
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property objProperty1 = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property();
                objProperty1.kind = "DocClass";
                objProperty1.theValue = "ZDW_DOC04.AISInvoice";
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property objProperty2 = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property();
                objProperty2.kind = "DocumentType";
                objProperty2.theValue = "48";
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property objProperty3 = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property();
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
                if (strReportType == "ProgramSummary")
                {
                    objDocument.typeName = "VoidInvoice" + "_" + strInvoiceNo + ".xlsx";
                }
                else
                {
                    objDocument.typeName = "VoidInvoice" + "_" + strInvoiceNo + ".pdf";
                }

                if (strReportType == "ProgramSummary")
                {
                    string strInvNo1 = string.Empty;

                    string[] strArray = new string[2];

                    strArray = DownloadFileProgramSummaySpreadsheetGear(intAdjNo, strInvoiceNo);

                    FileStream fs = new FileStream(strArray[1], FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] bc = new byte[fs.Length];
                    fs.Read(bc, 0, (Int32)fs.Length);
                    fs.Close();
                    // Creating the document content element and populating the binary value attribute with the content of document 
                    ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement objDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement();
                    objDocContent.theBinaryValue = bc; // Data is the byte stream of document content 

                    arrDocContent[0] = objDocContent;
                    objDocument.documentContentElement = arrDocContent;
                }
                else
                {

                    Stream st;
                    st = (Stream)objMain.ExportToStream(ExportFormatType.PortableDocFormat);
                    //byte[] arr = st.ToArray();
                    byte[] arr = new byte[st.Length];
                    st.Read(arr, 0, (int)st.Length);


                    // Creating the document content element and populating the binary value attribute with the content of document 
                    ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement objDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement();
                    objDocContent.theBinaryValue = arr; // Data is the byte stream of document content 

                    arrDocContent[0] = objDocContent;
                    objDocument.documentContentElement = arrDocContent;
                }
                // Instantiating Java Web Services and invoking “establishdocument” method - passing document object as ref.
                // For authentication, credentials have to be passed through the SOAP Header by passing security token over the SOAP request. 
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.CommunicationManagerService objJavaWS = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.CommunicationManagerService();

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

                string drft_ProgramSummary_ZDW_Key_txt = string.Empty;
                string fnl_ProgramSummary_ZDW_Key_txt = string.Empty;

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
                else if (strReportType.ToUpper().Trim() == "PROGRAMSUMMARY")
                {
                    drft_ProgramSummary_ZDW_Key_txt = strReferenceID;
                    fnl_ProgramSummary_ZDW_Key_txt = strReferenceID;
                }
                else
                {

                    premAdjBE.DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT = strReferenceID;
                    premAdjBE.FNL_CD_WRKSHT_PDF_ZDW_KEY_TXT = strReferenceID;
                }

                bool result = false;
                if (strReportType.ToUpper().Trim() == "PROGRAMSUMMARY")
                {

                    DataTable dtDraftProgSumm = new InvoiceDriverBS().UpdateDraftZDWKey_ProgramSummary(intAdjNo, strReferenceID, CurrentAISUser.PersonID);
                    DataTable dtFinalProgSumm = new InvoiceDriverBS().UpdateFinalZDWKey_ProgramSummary(intAdjNo, strReferenceID, CurrentAISUser.PersonID);

                    if (dtDraftProgSumm != null && dtFinalProgSumm != null)
                    {
                        if (dtDraftProgSumm.Rows.Count > 0 && dtFinalProgSumm.Rows.Count > 0)
                        {
                            result = true;
                        }
                    }
                }

                result = new PremAdjustmentBS().Update(premAdjBE);
                if (result)
                {
                    return strMessage;
                }
                else
                {
                    new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Unable to Save", CurrentAISUser.PersonID);
                    return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
                }
            }
            else
            {
                //filename = filename.Replace('/', '-');
                //filename = filename.Replace(' ', '-');
                //filename = filename.Replace(':', '-');

                //string strZDWkey = new PremAdjustmentBS().getZDWKey(intAdjNo, cFlag, intFlag);
                // Instantiating Electronic document object and array of Document Content element of electronic document.

                ZurichNA.AIS.WebSite.ZDWJavaWS.ElectronicDocument objDocument = new ZurichNA.AIS.WebSite.ZDWJavaWS.ElectronicDocument();
                ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement[] arrDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement[1];
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
                ZurichNA.AIS.WebSite.ZDWJavaWS.Property[] arrProperty = new ZurichNA.AIS.WebSite.ZDWJavaWS.Property[4];
                // For each dynamic property to be populated create Property object as follows
                ZurichNA.AIS.WebSite.ZDWJavaWS.Property objProperty1 = new ZurichNA.AIS.WebSite.ZDWJavaWS.Property();
                objProperty1.kind = "DocClass";
                objProperty1.theValue = "AISInvoice";
                ZurichNA.AIS.WebSite.ZDWJavaWS.Property objProperty2 = new ZurichNA.AIS.WebSite.ZDWJavaWS.Property();
                objProperty2.kind = "DocumentType";
                objProperty2.theValue = "48";
                ZurichNA.AIS.WebSite.ZDWJavaWS.Property objProperty3 = new ZurichNA.AIS.WebSite.ZDWJavaWS.Property();
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
                if (strReportType == "ProgramSummary")
                {
                    objDocument.typeName = "VoidInvoice" + "_" + strInvoiceNo + ".xlsx";
                }
                else
                {
                    objDocument.typeName = "VoidInvoice" + "_" + strInvoiceNo + ".pdf";
                }

                //MemoryStream st; - WinServer Changes for crystal Reports
                //st = (MemoryStream)objMain.ExportToStream(ExportFormatType.PortableDocFormat);
              
                if (strReportType == "ProgramSummary")
                {
                    string strInvNo1 = string.Empty;

                    string[] strArray = new string[2];

                    strArray = DownloadFileProgramSummaySpreadsheetGear(intAdjNo, strInvoiceNo);
                    
                    FileStream fs = new FileStream(strArray[1], FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] bc = new byte[fs.Length];
                    fs.Read(bc, 0, (Int32)fs.Length);
                    fs.Close();
                    // Creating the document content element and populating the binary value attribute with the content of document 
                    ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement objDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement();
                    objDocContent.theBinaryValue = bc; // Data is the byte stream of document content 

                    arrDocContent[0] = objDocContent;
                    objDocument.documentContentElement = arrDocContent;
                }
                else
                {
                    Stream st;
                    st = (Stream)objMain.ExportToStream(ExportFormatType.PortableDocFormat);
                    //byte[] arr = st.ToArray();
                    byte[] arr = new byte[st.Length];
                    st.Read(arr, 0, (int)st.Length);

                    // Creating the document content element and populating the binary value attribute with the content of document 
                    ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement objDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement();
                    objDocContent.theBinaryValue = arr; // Data is the byte stream of document content 

                    arrDocContent[0] = objDocContent;
                    objDocument.documentContentElement = arrDocContent;
                }


               
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

                string drft_ProgramSummary_ZDW_Key_txt = string.Empty;
                string fnl_ProgramSummary_ZDW_Key_txt = string.Empty;

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
                else if(strReportType.ToUpper().Trim() == "PROGRAMSUMMARY")
                {
                    drft_ProgramSummary_ZDW_Key_txt= strReferenceID;
                    fnl_ProgramSummary_ZDW_Key_txt = strReferenceID;
                }
                else
                {

                    premAdjBE.DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT = strReferenceID;
                    premAdjBE.FNL_CD_WRKSHT_PDF_ZDW_KEY_TXT = strReferenceID;
                }

                bool result=false;
                if (strReportType.ToUpper().Trim() == "PROGRAMSUMMARY")
                {
                    
                    DataTable dtDraftProgSumm = new InvoiceDriverBS().UpdateDraftZDWKey_ProgramSummary(intAdjNo, strReferenceID, CurrentAISUser.PersonID);
                    DataTable dtFinalProgSumm = new InvoiceDriverBS().UpdateFinalZDWKey_ProgramSummary(intAdjNo, strReferenceID, CurrentAISUser.PersonID);

                    if (dtDraftProgSumm != null && dtFinalProgSumm != null)
                    {
                        if (dtDraftProgSumm.Rows.Count > 0 && dtFinalProgSumm.Rows.Count > 0)
                        {
                            result = true;
                        }
                    }
                }
                else
                {
                    result = new PremAdjustmentBS().Update(premAdjBE);
                }
                if (result)
                {
                    return strMessage;
                }
                else
                {
                    new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Unable to Save", CurrentAISUser.PersonID);
                    return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
                }
            }            
        }
        catch (Exception ex)
        {
            new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
            strMessage = "ZDW Interface is not responding. Please try this action later.";
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
    public string[] DownloadFileProgramSummaySpreadsheetGear(int AdjNo, string strInvNo)
    {
        string[] strArray = new string[2];
        try
        {
            // Create a new workbook and worksheet.
            SSG.IWorkbook workbook = SSG.Factory.GetWorkbook();
            int sheetIndex = 0;
            int cellIndex = 0;

            // int debugindex = 0; //for break point

            #region Sheet:Summary Detail

            DataTable dtAdj = (new ProgramPeriodsBS()).GetAdjReport(AdjNo, 1, false, 0);

            if (dtAdj != null && dtAdj.Rows.Count > 0)
            {
                workbook.Worksheets.Add();
                sheetIndex++;
                SSG.IWorksheet worksheetAdj = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                worksheetAdj.Name = "Summary Detail";

                // Get the worksheet cells reference. 
                SSG.IRange cellsAdj = worksheetAdj.Cells;
                DataRow dr = dtAdj.Rows[0];
                cellIndex = 1;

                //worksheetAdj.Range["E2:E3"].Merge();

                //string path = Server.MapPath("~") + "\\images\\" + "z_logo_135_8.png";
                //worksheetAdj.Shapes.AddPicture(path, 250, 13, 40, 30);


                var item = (from items in dtAdj.AsEnumerable()
                            where (items.Field<Int32>("PREM ADJ ID") == AdjNo)
                                  && (!string.IsNullOrEmpty(items.Field<string>("VALUATION DATE")))
                            select
                            new { VALUATIONDATE = items.Field<string>("VALUATION DATE"), TOTALAMOUNTBILLED = items.Field<Decimal>("TOTAL AMOUNT BILLED") }
                            ).First();

                cellsAdj["A" + cellIndex.ToString()].Value = "Insured:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Broker:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(dr["BROKER NAME"]), SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Invoice Ref Number:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", strInvNo, SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Loss Val Date:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(item.VALUATIONDATE), SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Adjustment Date:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", DateTime.Today.ToShortDateString(), SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Total Amount Billed:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString("Voided"), SSG.HAlign.Left, cellIndex);
                //SetCurrencyFormatProgramSummaryTotalAmountBilled(cellsAdj["B" + cellIndex.ToString() + ":C" + cellIndex.ToString()]);
                cellIndex = cellIndex + 2;

                DataTable dtPeriodDet = (new ProgramPeriodsBS()).GetProgramSummaryPeriodDetails(AdjNo);

                if (dtPeriodDet.Rows.Count > 0)
                {
                    cellsAdj["A" + cellIndex.ToString()].Value = "Adjustment Number";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);

                    //worksheetAdj.Range["E2:E3"].Merge();

                    //string path = Server.MapPath("~") + "\\images\\" + "z_logo_135_8.png";
                    //if (dtPeriodDet.Rows.Count <= 3)
                    //{
                    //    worksheetAdj.Shapes.AddPicture(path, 215, 13, 40, 30);
                    //}
                    //else
                    //{
                    //    worksheetAdj.Shapes.AddPicture(path, 250, 13, 40, 30);
                    //}


                    string path = Server.MapPath("~") + "\\images\\" + "zurich_logo.JPG";
                    if (dtPeriodDet.Rows.Count <= 3)
                    {
                        worksheetAdj.Shapes.AddPicture(path, 215, 13, 100, 60);
                    }
                    else
                    {
                        worksheetAdj.Shapes.AddPicture(path, 250, 13, 100, 60);
                    }

                    for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                    {
                        String columname = string.Empty;
                        int columnindexvalue = i + 1;
                        columname = getColumnNamefromIndex(columnindexvalue);

                        int AdjustNum = 0;
                        AdjustNum = dtPeriodDet.Rows[i - 1].Field<int>("adj_nbr");
                        cellsAdj[columname + cellIndex.ToString()].Value = AdjustNum.ToString();
                        FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                    }
                    cellIndex = cellIndex + 1;
                    cellsAdj["A" + cellIndex.ToString()].Value = "Program period";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);

                    for (int i = 0; i < dtPeriodDet.Rows.Count; i++)
                    {
                        int columnindexvalue;
                        String columname = string.Empty;

                        if (i == 0)
                        {
                            columnindexvalue = i + 2;
                        }
                        else
                        {
                            columnindexvalue = i + 2;
                        }
                        columname = getColumnNamefromIndex(columnindexvalue);

                        string Programperiod = string.Empty;
                        Programperiod = dtPeriodDet.Rows[i].Field<string>("Program Period");

                        cellsAdj[columname + cellIndex.ToString()].Value = Programperiod.ToString();
                        FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                    }
                    cellIndex = cellIndex + 2;

                }

                #region Sheet:RPA

                DataTable dtProgramSummaryRetroReport = (new ProgramPeriodsBS()).GetProgramSummaryRetroDetails(AdjNo);
                if (dtProgramSummaryRetroReport != null)
                {
                    if (dtProgramSummaryRetroReport.Rows.Count > 0)
                    {
                        cellsAdj["A" + cellIndex.ToString()].Value = "Retrospective Premium Adjustment";
                        FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);

                        for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                        {
                            String columname = string.Empty;
                            int columnindexvalue = i + 1;
                            columname = getColumnNamefromIndex(columnindexvalue);

                            cellsAdj[columname + cellIndex.ToString()].Value = "";
                            FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                        }
                        cellIndex = cellIndex + 1;
                        cellsAdj["A" + cellIndex.ToString()].Value = "Program Loss Type";
                        FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                        for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                        {
                            String columname = string.Empty;
                            int columnindexvalue = i + 1;
                            columname = getColumnNamefromIndex(columnindexvalue);
                            string AdjsutmentType = string.Empty;
                            AdjsutmentType = dtPeriodDet.Rows[i - 1].Field<string>("adjustment_Type");
                            cellsAdj[columname + cellIndex.ToString()].Value = AdjsutmentType.ToString();
                            FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                        }
                        cellIndex = cellIndex + 1;


                        for (int i = 1; i <= dtProgramSummaryRetroReport.Rows.Count; i++)
                        {
                            String columname = string.Empty;
                            int columnindexvalue = i;
                            columname = getColumnNamefromIndex(columnindexvalue);

                            String RetroDescription = String.Empty;

                            RetroDescription = dtProgramSummaryRetroReport.Rows[i - 1].Field<string>("DESCR");
                            cellsAdj["A" + cellIndex.ToString()].Value = RetroDescription.ToString();

                            if (RetroDescription == "Adjustment Result:  AP  / (RP) Due Zurich")
                            {
                                SetFontBold(cellsAdj["A" + cellIndex.ToString()]);
                            }

                            //FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);


                            for (int J = 1; J <= dtPeriodDet.Rows.Count; J++)
                            {
                                String columnamePGMID = string.Empty;
                                int columnindexvaluePGMID = J + 1;
                                columnamePGMID = getColumnNamefromIndex(columnindexvaluePGMID);
                                int RetroPGMID = 0;
                                RetroPGMID = dtPeriodDet.Rows[J - 1].Field<int>("prem_adj_pgm_id");


                                int Fieldpostion;
                                if (J == 1)
                                    Fieldpostion = 3;
                                else
                                    Fieldpostion = J + 2;

                                decimal RetroFinalValue;
                                RetroFinalValue = dtProgramSummaryRetroReport.Rows[i - 1].Field<decimal>(Fieldpostion);
                                cellsAdj[columnamePGMID + cellIndex.ToString()].Value = RetroFinalValue.ToString();
                                SetCurrencyFormat(cellsAdj[columnamePGMID + cellIndex.ToString() + ":" + columnamePGMID + cellIndex.ToString()]);
                                if (RetroDescription == "Adjustment Result:  AP  / (RP) Due Zurich")
                                {
                                    SetFontBold(cellsAdj[columnamePGMID + cellIndex.ToString()]);
                                }
                            }

                            cellIndex = cellIndex + 1;

                        }

                    }
                    cellIndex = cellIndex + 2;
                }

                #endregion

                #region Sheet:ILRF

                DataTable dtLRFA = (new ProgramPeriodsBS()).GetProgramSummaryILRFDetails(AdjNo, 1, false, 0);


                if (dtLRFA != null)
                {
                    if (dtLRFA.Rows.Count > 0)
                    {

                        cellsAdj["A" + cellIndex.ToString()].Value = "Loss Reimbursement Fund";
                        FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);


                        for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                        {
                            String columname = string.Empty;
                            int columnindexvalue = i + 1;
                            columname = getColumnNamefromIndex(columnindexvalue);

                            cellsAdj[columname + cellIndex.ToString()].Value = "";
                            FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                        }
                        cellIndex = cellIndex + 1;


                        if (dtProgramSummaryRetroReport == null)
                        {
                            cellsAdj["A" + cellIndex.ToString()].Value = "Program Loss Type";
                            FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                            for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                            {
                                String columname = string.Empty;
                                int columnindexvalue = i + 1;
                                columname = getColumnNamefromIndex(columnindexvalue);
                                string AdjsutmentType = string.Empty;
                                AdjsutmentType = dtPeriodDet.Rows[i - 1].Field<string>("adjustment_Type");
                                cellsAdj[columname + cellIndex.ToString()].Value = AdjsutmentType.ToString();
                                FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                            }
                            cellIndex = cellIndex + 1;
                        }


                        for (int i = 1; i <= dtLRFA.Rows.Count; i++)
                        {
                            String columname = string.Empty;
                            int columnindexvalue = i;
                            columname = getColumnNamefromIndex(columnindexvalue);

                            String ILRFDescription = String.Empty;

                            ILRFDescription = dtLRFA.Rows[i - 1].Field<string>("DESCR");
                            cellsAdj["A" + cellIndex.ToString()].Value = ILRFDescription.ToString();
                            //FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                            if (ILRFDescription == "AP  / (RP)   Due Zurich")
                            {
                                SetFontBold(cellsAdj["A" + cellIndex.ToString()]);
                            }


                            for (int J = 1; J <= dtPeriodDet.Rows.Count; J++)
                            {
                                String columnamePGMID = string.Empty;
                                int columnindexvaluePGMID = J + 1;
                                columnamePGMID = getColumnNamefromIndex(columnindexvaluePGMID);
                                int ILRFPGMID = 0;
                                ILRFPGMID = dtPeriodDet.Rows[J - 1].Field<int>("prem_adj_pgm_id");

                                int Fieldpostion;
                                if (J == 1)
                                    Fieldpostion = 4;
                                else
                                    Fieldpostion = J + 3;

                                decimal ILRFFinalValue;
                                ILRFFinalValue = dtLRFA.Rows[i - 1].Field<decimal>(Fieldpostion);
                                cellsAdj[columnamePGMID + cellIndex.ToString()].Value = ILRFFinalValue.ToString();
                                SetCurrencyFormat(cellsAdj[columnamePGMID + cellIndex.ToString() + ":" + columnamePGMID + cellIndex.ToString()]);
                                if (ILRFDescription == "AP  / (RP)   Due Zurich")
                                {
                                    SetFontBold(cellsAdj[columnamePGMID + cellIndex.ToString()]);
                                }

                            }

                            cellIndex = cellIndex + 1;

                        }

                    }
                    cellIndex = cellIndex + 2;
                }


                #endregion

                #region Sheet:OtherAdjustments

                DataTable dtProgramSummaryOtherAdjsutmentDetails = (new ProgramPeriodsBS()).GetProgramSummaryOtherAdjsutmentDetails(AdjNo, 1, false, 0);

                bool checkifAdditionalInvoice = false;
                if (dtProgramSummaryOtherAdjsutmentDetails != null)
                {
                    String AdditionalInvoice = String.Empty;
                    for (int i = 1; i <= dtProgramSummaryOtherAdjsutmentDetails.Rows.Count; i++)
                    {
                        AdditionalInvoice = dtProgramSummaryOtherAdjsutmentDetails.Rows[i - 1].Field<string>("DESCR");

                        if (AdditionalInvoice != "Retro" && AdditionalInvoice != "WC Deductible" && AdditionalInvoice != "Loss Reimbursement Fund" && AdditionalInvoice != "DEP" && AdditionalInvoice != "DEP 2")
                        {
                            checkifAdditionalInvoice = true;
                            break;
                        }

                    }
                }

                if (checkifAdditionalInvoice == true)
                {
                    if (dtProgramSummaryOtherAdjsutmentDetails.Rows.Count > 0)
                    {
                        cellsAdj["A" + cellIndex.ToString()].Value = "Additional Invoiced Amounts";
                        FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);


                        for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                        {
                            String columname = string.Empty;
                            int columnindexvalue = i + 1;
                            columname = getColumnNamefromIndex(columnindexvalue);

                            cellsAdj[columname + cellIndex.ToString()].Value = "";
                            FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                        }
                        cellIndex = cellIndex + 1;


                        if (dtProgramSummaryRetroReport == null && dtLRFA == null)
                        {
                            cellsAdj["A" + cellIndex.ToString()].Value = "Program Loss Type";
                            FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                            for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                            {
                                String columname = string.Empty;
                                int columnindexvalue = i + 1;
                                columname = getColumnNamefromIndex(columnindexvalue);
                                string AdjsutmentType = string.Empty;
                                AdjsutmentType = dtPeriodDet.Rows[i - 1].Field<string>("adjustment_Type");
                                cellsAdj[columname + cellIndex.ToString()].Value = AdjsutmentType.ToString();
                                FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                            }
                            cellIndex = cellIndex + 1;
                        }
                        for (int i = 1; i <= dtProgramSummaryOtherAdjsutmentDetails.Rows.Count; i++)
                        {
                            String columname = string.Empty;
                            int columnindexvalue = i;
                            columname = getColumnNamefromIndex(columnindexvalue);

                            String OtherAdjustmentDescription = String.Empty;

                            OtherAdjustmentDescription = dtProgramSummaryOtherAdjsutmentDetails.Rows[i - 1].Field<string>("DESCR");

                            if (OtherAdjustmentDescription != "Retro" && OtherAdjustmentDescription != "WC Deductible" && OtherAdjustmentDescription != "Loss Reimbursement Fund" && OtherAdjustmentDescription != "DEP" && OtherAdjustmentDescription != "DEP 2")
                            {
                                cellsAdj["A" + cellIndex.ToString()].Value = OtherAdjustmentDescription.ToString();
                                //FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                                SetFontBold(cellsAdj["A" + cellIndex.ToString()]);

                                for (int J = 1; J <= dtPeriodDet.Rows.Count; J++)
                                {
                                    String columnamePGMID = string.Empty;
                                    int columnindexvaluePGMID = J + 1;
                                    columnamePGMID = getColumnNamefromIndex(columnindexvaluePGMID);
                                    int AddInvAmtPGMID = 0;
                                    AddInvAmtPGMID = dtPeriodDet.Rows[J - 1].Field<int>("prem_adj_pgm_id");

                                    int Fieldpostion;
                                    if (J == 1)
                                        Fieldpostion = 3;
                                    else
                                        Fieldpostion = J + 2;

                                    decimal AdditionInvoiceFinalValue;
                                    AdditionInvoiceFinalValue = dtProgramSummaryOtherAdjsutmentDetails.Rows[i - 1].Field<decimal>(Fieldpostion);
                                    cellsAdj[columnamePGMID + cellIndex.ToString()].Value = AdditionInvoiceFinalValue.ToString();
                                    SetCurrencyFormat(cellsAdj[columnamePGMID + cellIndex.ToString() + ":" + columnamePGMID + cellIndex.ToString()]);
                                    SetFontBold(cellsAdj[columnamePGMID + cellIndex.ToString()]);

                                }

                                cellIndex = cellIndex + 1;
                            }


                        }
                        cellIndex = cellIndex + 2;
                    }
                }


                #endregion


                #region Sheet:Period Result

                if (dtProgramSummaryOtherAdjsutmentDetails.Rows.Count > 0)
                {
                    cellsAdj["A" + cellIndex.ToString()].Value = "Overall Result by Period";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);



                    for (int J = 1; J <= dtPeriodDet.Rows.Count; J++)
                    {
                        String columnamePGMID = string.Empty;
                        int columnindexvaluePGMID = J + 1;
                        columnamePGMID = getColumnNamefromIndex(columnindexvaluePGMID);
                        //int RetroPGMID = 0;
                        //RetroPGMID = dtPeriodDet.Rows[J - 1].Field<int>("prem_adj_pgm_id");

                        int Fieldpostion;
                        if (J == 1)
                            Fieldpostion = 3;
                        else
                            Fieldpostion = J + 2;

                        decimal PeriodTotal = 0;


                        for (int i = 1; i <= dtProgramSummaryOtherAdjsutmentDetails.Rows.Count; i++)
                        {
                            if (i == 1)
                            {
                                PeriodTotal = dtProgramSummaryOtherAdjsutmentDetails.Rows[i - 1].Field<decimal>(Fieldpostion);
                            }
                            else
                            {
                                PeriodTotal = PeriodTotal + dtProgramSummaryOtherAdjsutmentDetails.Rows[i - 1].Field<decimal>(Fieldpostion);
                            }


                        }
                        cellsAdj[columnamePGMID + cellIndex.ToString()].Value = PeriodTotal.ToString();
                        SetCurrencyFormat(cellsAdj[columnamePGMID + cellIndex.ToString() + ":" + columnamePGMID + cellIndex.ToString()]);
                        FormatHeaderTypeCellRetro(cellsAdj[columnamePGMID + cellIndex.ToString()]);

                    }
                }

                #endregion


                worksheetAdj.UsedRange.Columns.AutoFit();
                for (int col = 0; col < worksheetAdj.UsedRange.ColumnCount; col++)
                {
                    worksheetAdj.Cells[1, col].ColumnWidth *= 1.15;
                }
            }

            #endregion

            #region Sheet:Policy Number


            if (dtAdj != null && dtAdj.Rows.Count > 0)
            {
                workbook.Worksheets.Add();
                sheetIndex++;
                SSG.IWorksheet worksheetPolicyNumber = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                worksheetPolicyNumber.Name = "Policy Number";

                // Get the worksheet cells reference. 
                SSG.IRange cellsAdj = worksheetPolicyNumber.Cells;
                DataRow dr = dtAdj.Rows[0];
                cellIndex = 1;


                //worksheetPolicyNumber.Range["E2:E3"].Merge();

                //string path = Server.MapPath("~") + "\\images\\" + "z_logo_135_8.png";
                //worksheetPolicyNumber.Shapes.AddPicture(path, 230, 13, 40, 30);
               
                //LOGO CHANGES
                string path = Server.MapPath("~") + "\\images\\" + "zurich_logo.JPG";
                worksheetPolicyNumber.Shapes.AddPicture(path, 230, 13, 100, 60);


                var item = (from items in dtAdj.AsEnumerable()
                            where (items.Field<Int32>("PREM ADJ ID") == AdjNo)
                                  && (!string.IsNullOrEmpty(items.Field<string>("VALUATION DATE")))
                            select
                            new { VALUATIONDATE = items.Field<string>("VALUATION DATE"), TOTALAMOUNTBILLED = items.Field<Decimal>("TOTAL AMOUNT BILLED") }
                            ).First();

                cellsAdj["A" + cellIndex.ToString()].Value = "Insured:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Broker:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(dr["BROKER NAME"]), SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Invoice Ref Number:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", strInvNo, SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Loss Val Date:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(item.VALUATIONDATE), SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Adjustment Date:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", DateTime.Today.ToShortDateString(), SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Total Amount Billed:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString("Voided"), SSG.HAlign.Left, cellIndex);
                //SetCurrencyFormatProgramSummaryTotalAmountBilled(cellsAdj["B" + cellIndex.ToString() + ":C" + cellIndex.ToString()]);
                cellIndex = cellIndex + 2;

                DataTable dtPeriodDet = (new ProgramPeriodsBS()).GetProgramSummaryPeriodDetails(AdjNo);

                if (dtPeriodDet.Rows.Count > 0)
                {

                    cellsAdj["A" + cellIndex.ToString()].Value = "Program period / Adjustment Type:";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);

                    for (int i = 0; i < dtPeriodDet.Rows.Count; i++)
                    {
                        int columnindexvalue = 0;
                        int columnindexvalue1 = 0;


                        String columname = string.Empty;
                        String columname1 = string.Empty;
                        if (i == 0)
                        {
                            columnindexvalue = i + 2;
                            columnindexvalue1 = columnindexvalue + 1;
                            ViewState["ColumnIndex1"] = columnindexvalue1.ToString();

                        }
                        else
                        {
                            if (ViewState["ColumnIndex1"] != null)
                            {
                                columnindexvalue = Convert.ToInt32(ViewState["ColumnIndex1"]) + 1;
                                columnindexvalue1 = columnindexvalue + 1;
                                ViewState["ColumnIndex1"] = columnindexvalue1.ToString();
                            }
                        }
                        columname = getColumnNamefromIndex(columnindexvalue);
                        columname1 = getColumnNamefromIndex(columnindexvalue1);
                        columname1 = ":" + columname1.ToString();

                        string Programperiod = string.Empty;
                        Programperiod = dtPeriodDet.Rows[i].Field<string>("Program Period");

                        //cellsAdj[columname + cellIndex.ToString()].Value = Programperiod.ToString();

                        MergeAndFormatCellsWithColor(cellsAdj, columname, columname1, Convert.ToString(Programperiod), SSG.HAlign.Left, cellIndex);
                        FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString() + columname1 + cellIndex.ToString()]);

                        //FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString() + ":E" + cellIndex.ToString()]);

                    }
                    cellIndex = cellIndex + 1;

                }

                #region Sheet:Policy Numbers

                DataTable GetProgramSummaryPolicyInfo = (new ProgramPeriodsBS()).GetProgramSummaryPolicyInfo(AdjNo);
                if (GetProgramSummaryPolicyInfo != null)
                {
                    if (GetProgramSummaryPolicyInfo.Rows.Count > 0)
                    {
                        cellsAdj["A" + cellIndex.ToString()].Value = "Policy Numbers:";
                        FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);

                        for (int i = 0; i < dtPeriodDet.Rows.Count; i++)
                        {

                            int columnindexvalue = 0;
                            int columnindexvalue1 = 0;


                            String columnamePolicy = string.Empty;
                            String columname1Policy = string.Empty;

                            if (i == 0)
                            {
                                columnindexvalue = i + 2;
                                columnindexvalue1 = columnindexvalue + 1;
                                ViewState["ColumnIndex1Policy"] = columnindexvalue1.ToString();

                            }
                            else
                            {
                                if (ViewState["ColumnIndex1Policy"] != null)
                                {
                                    columnindexvalue = Convert.ToInt32(ViewState["ColumnIndex1Policy"]) + 1;
                                    columnindexvalue1 = columnindexvalue + 1;
                                    ViewState["ColumnIndex1Policy"] = columnindexvalue1.ToString();
                                }
                            }
                            columnamePolicy = getColumnNamefromIndex(columnindexvalue);
                            columname1Policy = getColumnNamefromIndex(columnindexvalue1);


                            DataTable GetPolicyInfoByPGMID = (from dts in GetProgramSummaryPolicyInfo.AsEnumerable()
                                                              where (dts.Field<int>("PREM_ADJ_PGM_ID") == Convert.ToInt32(dtPeriodDet.Rows[i]["PREM_ADJ_PGM_ID"]))
                                                              select dts).CopyToDataTable();
                            cellIndex = 9;
                            for (int k = 0; k < GetPolicyInfoByPGMID.Rows.Count; k++)
                            {
                                if (k == 0)
                                {
                                    String PolicyNumbervalue = string.Empty;
                                    String Policy_Type = string.Empty;
                                    PolicyNumbervalue = GetPolicyInfoByPGMID.Rows[k].Field<string>("Policy_Number");
                                    cellsAdj[columnamePolicy + cellIndex.ToString()].Value = PolicyNumbervalue.ToString();

                                    Policy_Type = GetPolicyInfoByPGMID.Rows[k].Field<string>("Policy_Type");
                                    cellsAdj[columname1Policy + cellIndex.ToString()].Value = Policy_Type.ToString();
                                }
                                else
                                {

                                    String PolicyNumbervalue = string.Empty;
                                    String Policy_Type = string.Empty;
                                    PolicyNumbervalue = GetPolicyInfoByPGMID.Rows[k].Field<string>("Policy_Number");
                                    cellsAdj[columnamePolicy + cellIndex.ToString()].Value = PolicyNumbervalue.ToString();

                                    Policy_Type = GetPolicyInfoByPGMID.Rows[k].Field<string>("Policy_Type");
                                    cellsAdj[columname1Policy + cellIndex.ToString()].Value = Policy_Type.ToString();
                                }
                                cellIndex = cellIndex + 1;
                            }

                        }
                    }
                }
                #endregion

                worksheetPolicyNumber.UsedRange.Columns.AutoFit();
                for (int col = 0; col < worksheetPolicyNumber.UsedRange.ColumnCount; col++)
                {
                    worksheetPolicyNumber.Cells[1, col].ColumnWidth *= 1.15;
                }
            }

            #endregion

            if (sheetIndex == 0)
            {
                workbook.Worksheets.Add();
            }

            string vcwDirectory = ConfigurationManager.AppSettings["Directory"].ToString();
            string sFormat = DateTime.Now.ToString("MM-dd-yy-HH:mm:ss:fffff");
            sFormat = sFormat.Replace(":", "");
            sFormat = sFormat.Replace("-", "");
            string cFlag = "p";
            //if (extIntFlag == 0)
            //{
            //    cFlag = 'E';
            //}

            workbook.Worksheets[0].Select();

            string strFileName = vcwDirectory + "\\" + cFlag + "_" + sFormat + "_" + "Void" + "_" + AdjNo + ".xlsx";
            //EAISA-5 Veracode flaw fix 12072017
            string strFileNameDecd = Server.HtmlDecode(Server.HtmlEncode(strFileName));
            workbook.SaveAs(strFileNameDecd, SSG.FileFormat.OpenXMLWorkbook);
            workbook.Close();

            strArray[0] = cFlag.ToString();
            strArray[1] = strFileName;

        }
        catch (Exception err)
        {
            common.Logger.Error(err);
        }

        return strArray;
    }
    // Phase -3
    public static String getColumnNamefromIndex(int column)
    {
        column--;
        String col = Convert.ToString((char)('A' + (column % 26)));
        while (column >= 26)
        {
            column = (column / 26) - 1;
            col = Convert.ToString((char)('A' + (column % 26))) + col;
        }
        return col;
    }

    private Dictionary<int, string> GetPrgPrdsWithType(List<DataTable> dtResult, string colName)
    {
        List<int> prgPrdIDList = new List<int>();

        for (int i = 0; i < dtResult.Count; i++)
        {
            prgPrdIDList.Add(Convert.ToInt32(dtResult[i].Rows[0][colName]));
        }
        Dictionary<int, string> prgPrdList = new ProgramPeriodsBS().GetProgramPeriodTypes(prgPrdIDList);
        return prgPrdList;
    }

    //This Method checks NullOrEmpty for decimal field.
    private string CheckNullOrEmptyReturnValue(DataRow row, string colName)
    {
        string result = !string.IsNullOrEmpty(Convert.ToString(row[colName])) ? Convert.ToString(row[colName]) : "0";
        return result;
    }

    //This Method sets currency format in excel cell for amount type 
    private void SetCurrencyFormat(SSG.IRange cell)
    {
        cell.HorizontalAlignment = SSG.HAlign.Right;
        //cell.NumberFormat = "$#,###";
       // cell.NumberFormat = "$#,##0";
        cell.NumberFormat = "$#,##0_);($#,##0)";
       

    }


    // Phase -3
    private void SetFontBold(SSG.IRange cell)
    {
        cell.Font.Bold = true;

    }

    // Phase -3
    //This Method sets currency format in excel cell for amount type 
    private void SetCurrencyFormatProgramSummaryTotalAmountBilled(SSG.IRange cell)
    {
        cell.HorizontalAlignment = SSG.HAlign.Left;
        //cell.NumberFormat = "$#,###";
       // cell.NumberFormat = "$#,##0";
        cell.NumberFormat = "$#,##0_);($#,##0)";
   
    }

    //This Method sets currency format in excel cell for amount type with 2-decimal
    private void SetCurrencyFormatForDecimal(SSG.IRange cell)
    {
        cell.HorizontalAlignment = SSG.HAlign.Right;
        //cell.NumberFormat = "$#,###";
       // cell.NumberFormat = "$#,##0.00";
        cell.NumberFormat = "$#,##0.00_);($#,##0.00)";
     
    }

    private decimal GetSumForDecimalField(DataTable dtResult, string colName)
    {
        var sumTotal = dtResult.AsEnumerable()
                            .Where(row => row.Field<decimal?>(colName) != null)
                            .Sum(row => row.Field<decimal>(colName));
        return Convert.ToDecimal(sumTotal);
    }

    private void MergeAndFormatCellsWithoutColor(SSG.IRange cells, string colStart, string colEnd, string text, SSG.HAlign align, int cellIndex, bool isBold = true)
    {
        SSG.IRange cell = cells[colStart + cellIndex.ToString() + colEnd + cellIndex.ToString()];
        cell.Merge();
        cell.Font.Bold = isBold;
        cell.HorizontalAlignment = align;
        cell.Value = text;
    }
    private void MergeAndFormatCellsWithoutColorandBold(SSG.IRange cells, string colStart, string colEnd, string text, SSG.HAlign align, int cellIndex, bool isBold = false)
    {
        SSG.IRange cell = cells[colStart + cellIndex.ToString() + colEnd + cellIndex.ToString()];
        cell.Merge();
        cell.Font.Bold = isBold;
        cell.HorizontalAlignment = align;
        cell.Value = text;
    }

    private void MergeAndFormatCellsWithColor(SSG.IRange cells, string colStart, string colEnd, string text, SSG.HAlign align, int cellIndex, bool isBold = true, SSG.Color color = default(SSG.Color))
    {
        SSG.IRange cell = cells[colStart + cellIndex.ToString() + colEnd + cellIndex.ToString()];
        cell.Merge();
        cell.Font.Bold = isBold;
        cell.HorizontalAlignment = align;
        cell.Value = text;
        //FormatHeaderTypeCell(cell,align,isBold);
        SetColorToCell(cell, color);

    }

    private void FormatHeaderTypeCell(SSG.IRange cell, SSG.HAlign align = SSG.HAlign.Left, bool isBold = true, SSG.Color color = default(SSG.Color))
    {
        cell.Font.Bold = isBold;
        cell.HorizontalAlignment = align;
        cell.Borders.Color = SSG.Colors.SteelBlue;
        SetColorToCell(cell, color);
        //if (color == default(SSG.Color))
        //{
        //    cell.Interior.Color = SSG.Color.FromArgb(192, 192, 192);
        //}
        //else
        //{
        //    cell.Interior.Color = color;
        //}
        //cell.Interior.Color = SSG.Colors.SteelBlue;
        //SSG.Color.FromArgb(192,192,192)
    }

    private void FormatHeaderTypeCellProgramSumary(SSG.IRange cell, SSG.HAlign align = SSG.HAlign.Center, bool isBold = true, SSG.Color color = default(SSG.Color))
    {
        cell.Font.Bold = isBold;
        cell.HorizontalAlignment = align;
        cell.Borders.Color = SSG.Colors.SteelBlue;
        SetColorToCell(cell, color);
        //if (color == default(SSG.Color))
        //{
        //    cell.Interior.Color = SSG.Color.FromArgb(192, 192, 192);
        //}
        //else
        //{
        //    cell.Interior.Color = color;
        //}
        //cell.Interior.Color = SSG.Colors.SteelBlue;
        //SSG.Color.FromArgb(192,192,192)
    }

    private void FormatHeaderTypeCellRetro(SSG.IRange cell, SSG.HAlign align = SSG.HAlign.Right, bool isBold = true, SSG.Color color = default(SSG.Color))
    {
        cell.Font.Bold = isBold;
        cell.HorizontalAlignment = align;
        cell.Borders.Color = SSG.Colors.SteelBlue;
        SetColorToCell(cell, color);
        //if (color == default(SSG.Color))
        //{
        //    cell.Interior.Color = SSG.Color.FromArgb(192, 192, 192);
        //}
        //else
        //{
        //    cell.Interior.Color = color;
        //}
        //cell.Interior.Color = SSG.Colors.SteelBlue;
        //SSG.Color.FromArgb(192,192,192)
    }
    private void FormatHeaderTypeCellExcessLoss(SSG.IRange cell, SSG.HAlign align = SSG.HAlign.Left, bool isBold = true, SSG.Color color = default(SSG.Color))
    {
        cell.Font.Bold = isBold;
        cell.HorizontalAlignment = align;
        cell.Borders.Color = SSG.Colors.SteelBlue;
        SetColorToCell(cell, color);
        //if (color == default(SSG.Color))
        //{
        //    cell.Interior.Color = SSG.Color.FromArgb(192, 192, 192);
        //}
        //else
        //{
        //    cell.Interior.Color = color;
        //}
        //cell.Interior.Color = SSG.Colors.SteelBlue;
        //SSG.Color.FromArgb(192,192,192)
    }

    private void SetColorToCell(SSG.IRange cell, SSG.Color color = default(SSG.Color))
    {
        if (color == default(SSG.Color))
        {
            cell.Interior.Color = SSG.Color.FromArgb(192, 192, 192);
        }
        else
        {
            cell.Interior.Color = color;
        }
    }


}
