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
using System.IO;
using SSG = SpreadsheetGear; // AWS Cloud Migration


public partial class AcctMgmt_AccountDashboard : AISBasePage
{
    Boolean UserDropReload = false;
    IList<ProgramPeriodBE> programPeriodInfo = new List<ProgramPeriodBE>();

    IList<PremiumAdjustmentBE> AdjustmentInfo
    {
        get
        {
            //if (Session["AdjustmentInfo"] == null)
            //    Session["AdjustmentInfo"] = new List<PremiumAdjustmentBE>();
            //return (IList<PremiumAdjustmentBE>)Session["AdjustmentInfo"];

            if (RetrieveObjectFromSessionUsingWindowName("AdjustmentInfo") == null)
                SaveObjectToSessionUsingWindowName("AdjustmentInfo", new List<PremiumAdjustmentBE>());
            return (IList<PremiumAdjustmentBE>)RetrieveObjectFromSessionUsingWindowName("AdjustmentInfo");
        }
        set
        {
            //Session["AdjustmentInfo"] = value;
            SaveObjectToSessionUsingWindowName("AdjustmentInfo", value);
        }
    }

    private Boolean isDataEmpty
    {
        get { return ((ViewState["isDataEmpty"] == null) ? false : Convert.ToBoolean(ViewState["isDataEmpty"])); }
        set { ViewState["isDataEmpty"] = value; }
    }

    private Boolean InitialSearch
    {
        get { return ((ViewState["InitialSearch"] == null) ? false : Convert.ToBoolean(ViewState["InitialSearch"])); }
        set { ViewState["InitialSearch"] = value; }
    }

    private ProgramPeriodsBS ProgramPeriodBS;
    private ProgramPeriodsBS programPeriodBS
    {
        get
        {
            if (ProgramPeriodBS == null)
            {
                ProgramPeriodBS = new ProgramPeriodsBS();
            }
            return ProgramPeriodBS;
        }
    }

    private bool CheckResponsibility(string tabName, int custmrID, int personID)
    {
        bool succeed = false;
        IList<ProgramPeriodBE> Roles = new ProgramPeriodsBS().GetRoles(custmrID, CurrentAISUser.PersonID);
        int intCFS2ID = new BLAccess().GetLookUpID("CFS2", "RESPONSIBILITY");
        int intACCTSETUPQC = new BLAccess().GetLookUpID("ACCOUNT SETUP QC", "RESPONSIBILITY");
        int intADJQC100ID = new BLAccess().GetLookUpID("ADJUSTMENT QC 100%", "RESPONSIBILITY");
        int intADJQC20ID = new BLAccess().GetLookUpID("ADJUSTMENT QC 20%", "RESPONSIBILITY");
        int intRECONCILERID = new BLAccess().GetLookUpID("RECONCILER", "RESPONSIBILITY");
        int intLSSADMIN = new BLAccess().GetLookUpID("LSS ADMIN", "RESPONSIBILITY");

        int intCount = 0;

        switch (tabName.ToUpper())
        {
            case "PROGRAM":
                intCount = Roles.Count(rol => rol.ROL_ID == intCFS2ID || rol.ROL_ID == intACCTSETUPQC);
                break;
            case "ADJUSTMENT":
                intCount = Roles.Count(rol => rol.ROL_ID == intCFS2ID || rol.ROL_ID == intADJQC100ID || rol.ROL_ID == intADJQC20ID || rol.ROL_ID == intRECONCILERID);
                break;
            case "INVOICE":
                intCount = Roles.Count(rol => rol.ROL_ID == intCFS2ID || rol.ROL_ID == intADJQC100ID || rol.ROL_ID == intADJQC20ID || rol.ROL_ID == intRECONCILERID);
                break;
            case "ADJUSTMENTSEARCH":
                intCount = Roles.Count(rol => rol.ROL_ID == intCFS2ID);
                break;
            case "LSSADMIN":
                intCount = Roles.Count(rol => rol.ROL_ID == intLSSADMIN);
                break;
        }
        return succeed = (intCount > 0);

    }

    protected void Page_Load(object sender, EventArgs e)
    {

        this.Master.Page.Header.Title = "Account Dashboard";

        if (!IsPostBack)
        {
            InitialSearch = true;
            TabConDashboard.ActiveTabIndex = Convert.ToInt16(Request.QueryString["tab"]);
            ddlUser.DataBind();
            ListItem li = new ListItem(CurrentAISUser.FullName, CurrentAISUser.PersonID.ToString());
            if (ddlUser.Items.Contains(li))
            {
                ddlUser.Items.FindByText(CurrentAISUser.FullName).Selected = true;
                int perId = int.Parse(ddlUser.SelectedValue);
                AccountDataSource.SelectParameters["perId"].DefaultValue = perId.ToString();
                AccountDataSource.DataBind();
            }
            if (TabConDashboard.ActiveTabIndex == 0)
            {

                trFromDt.Visible = true;
                trToDt.Visible = true;
                trStatus.Visible = true;
                StatusDataSource.SelectParameters["lookUpTypeName"].DefaultValue = "PROGRAM STATUSES";
                StatusDataSource.Select();
                StatusDataSource.DataBind();
                ddlStatus.DataBind();
                ddlStatus.SelectedIndex = 0;
                GetCurrentUserProgramPeriodData(CurrentAISUser.PersonID);
            }
            if (TabConDashboard.ActiveTabIndex == 1)
            {
                trPending.Visible = true;
                ddlPending.SelectedIndex = 0;
                trStatus.Visible = true;
                StatusDataSource.SelectParameters["lookUpTypeName"].DefaultValue = "ADJUSTMENT STATUSES";
                StatusDataSource.Select();
                StatusDataSource.DataBind();
                ddlStatus.DataBind();
                ddlStatus.SelectedIndex = 0;
                //UserDataSource
                GetCurrentUserAdjustmentData(CurrentAISUser.PersonID);

            }
            if (TabConDashboard.ActiveTabIndex == 2)
            {
                trAriesClrng.Visible = true;
                trQcComplt.Visible = true;
                ddlAriesClrng.SelectedIndex = 2;
                ddlQcCompltd.SelectedIndex = 0;
                trStatus.Visible = false;
                GetCurrentUserInvoiceData(CurrentAISUser.PersonID);
            }
        }

    }
    protected void lstPp_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        Label lblCustmrID = (Label)e.Item.FindControl("lblActNum");
        Label lbnPrgmStatus = (Label)e.Item.FindControl("lbnPrgmStatus");
        LinkButton lbnAdjDashBoard = (LinkButton)e.Item.FindControl("lbnAdjDashBoard");
        if (lbnPrgmStatus.Text.ToUpper() == "ACTIVE")
            lbnAdjDashBoard.Enabled = true;
        else
            lbnAdjDashBoard.Enabled = false;

        int custmrID = Convert.ToInt32(lblCustmrID.Text);

        //if(!IsPostBack)
        //{
        //    if(e.Item.ItemType == ListViewItemType.DataItem)
        //    e.Item.Visible = CheckResponsibility("program", custmrID, CurrentAISUser.PersonID);
        //}
    }

    protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
    {
        InitialSearch = false;
        int perId = Convert.ToInt32(ddlUser.SelectedValue);
        int status = 0;
        if (ddlStatus.SelectedValue != "")
            status = Convert.ToInt32(ddlStatus.SelectedValue);
        int accountId = Convert.ToInt32(ddlAccountName.SelectedValue);

        if (perId == 0 && accountId == 0)
        {
            isDataEmpty = true;
            lstPp.Visible = false;
            btnExcel.Visible = false;

        }


        switch (TabConDashboard.ActiveTabIndex)
        {
            case 0: //Upcoming Valuations & ProgramPeriod Details Tab
                trQcComplt.Visible = false;
                trAriesClrng.Visible = false;
                trPending.Visible = false;
                trFromDt.Visible = true;
                trToDt.Visible = true;
                trStatus.Visible = true;
                StatusDataSource.SelectParameters["lookUpTypeName"].DefaultValue = "PROGRAM STATUSES";
                StatusDataSource.Select();
                StatusDataSource.DataBind();
                ddlStatus.DataBind();
                ddlStatus.SelectedIndex = 0;
                string startDate = string.Empty; // = DateTime.Today;
                string endDate = string.Empty;   // = DateTime.Today;
                if (txtFromDt.Text != string.Empty && txtToDt.Text != string.Empty)
                {
                    startDate = lblFromDt.Text;
                    endDate = lblToDate.Text;
                    isDataEmpty = false;
                    lstPp.Visible = true;
                    btnExcel.Visible = true;
                }
                if (status > 0)
                {
                    isDataEmpty = false;
                    lstPp.Visible = true;
                    btnExcel.Visible = true;
                }

                //if (isDataEmpty == false)
                // To retrieve ProgramPeriod details
                GetProgramPeriodData(accountId, status, perId, startDate, endDate);
                break;

            case 1: //Adjustment Details Tab

                trQcComplt.Visible = false;
                trAriesClrng.Visible = false;
                trFromDt.Visible = false;
                trToDt.Visible = false;
                trPending.Visible = true;
                trStatus.Visible = true;
                StatusDataSource.SelectParameters["lookUpTypeName"].DefaultValue = "ADJUSTMENT STATUSES";
                StatusDataSource.Select();
                StatusDataSource.DataBind();
                ddlStatus.DataBind();
                ddlStatus.SelectedIndex = 0;
                ddlPending.SelectedIndex = 0;
                int pending = Convert.ToInt32(ddlPending.SelectedValue);
                // To retrieve Adjustment details
                GetAdjustmentData(accountId, status, perId, pending);
                break;
            case 2: // Invoice Details Tab
                trQcComplt.Visible = true;
                trAriesClrng.Visible = true;
                trFromDt.Visible = false;
                trToDt.Visible = false;
                trStatus.Visible = false;
                trPending.Visible = false;
                int qcComplete = Convert.ToInt32(ddlQcCompltd.SelectedValue);
                int ariesClrng = Convert.ToInt32(ddlAriesClrng.SelectedValue);
                // To retrieve Invoice details
                GetInvoiceData(accountId, perId, qcComplete, ariesClrng);
                break;
        }

    }
    protected void ddlUser_OnSelectedIndexChanged(Object sender, EventArgs e)
    {

        DropDownList ddlUser = (DropDownList)sender;

        int perId = int.Parse(ddlUser.SelectedValue);
        AccountDataSource.SelectParameters["perId"].DefaultValue = perId.ToString();
        AccountDataSource.DataBind();
        UserDropReload = true;
    }
    /// <summary>
    /// To retrieve Upcoming Valuations and ProgramPeriod Data
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="statusId"></param>
    /// <param name="perId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>

    public void GetProgramPeriodData(int accountId, int statusId, int perId, string startDate, string endDate)
    {

        programPeriodInfo = new ProgramPeriodsBS().GetProgramPeriodData(accountId, statusId, perId, startDate, endDate);
        this.lstPp.DataSource = programPeriodInfo.ToList();

        lstPp.DataBind();

        lstPp.Visible = true;

        if (programPeriodInfo.Count > 0)
        {
            btnExcel.Visible = true;

        }
        else
        {
            btnExcel.Visible = false;
            //string strMessage = "No Record(s) found...!";
            //ShowError(strMessage);

        }



    }
    public void GetCurrentUserProgramPeriodData(int intPersonID)
    {
        int intStatusID = 0;
        string strStartDate = string.Empty;
        string strEndDate = string.Empty;
        intStatusID = Convert.ToInt32(new BLAccess().GetLookUpID(GlobalConstants.ProgramStatus.Active, "PROGRAM STATUSES").ToString());
        programPeriodInfo = new ProgramPeriodsBS().GetProgramPeriodData(intStatusID, intPersonID);

        this.lstPp.DataSource = programPeriodInfo.ToList();
        lstPp.DataBind();
        if (programPeriodInfo.Count > 0)
        {
            lstPp.Visible = true;
            btnExcel.Visible = true;

        }
        else
        {
            btnExcel.Visible = false;
        }

    }
    /// <summary>
    /// To retrieve Adjustment Details
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="statusId"></param>
    /// <param name="perId"></param>
    /// <param name="pending"></param>

    public void GetAdjustmentData(int accountId, int statusId, int perId, int pending)
    {
        string strPending = "";
        if (pending == 0)
            strPending = "false";
        else if (pending == 1)
            strPending = "true";
        else if (pending == 2)
            strPending = "All";

        AdjustmentInfo = new PremAdjustmentBS().GetAdjustmentInfo(accountId, statusId, perId, strPending);
        this.lstAdj.DataSource = AdjustmentInfo.Where(prem => prem.PREM_ADJ_PGM_ID != 0).ToList();
        lstAdj.DataBind();
        if (AdjustmentInfo.Count > 0)
        {
            btnAdjExcel.Visible = true;

        }
        else
        {
            btnAdjExcel.Visible = false;
            //string strMessage = "No Record(s) found...!";
            //ShowError(strMessage);

        }



    }


    public void GetCurrentUserAdjustmentData(int intPersonID)
    {
        //int intAccountID = 0;
        //int intStatusID = 0;
        //string strPending = "All";
        AdjustmentInfo = new PremAdjustmentBS().GetAdjustmentInfo(intPersonID);
        AdjustmentInfo = AdjustmentInfo.Where(prem => prem.PREM_ADJ_PGM_ID != 0).ToList();

        AdjustmentInfo = AdjustmentInfo.Where(adj =>
            (CheckResponsibility("Adjustment", adj.CUSTOMERID, intPersonID))).ToList();

        //AdjustmentInfo = AdjustmentInfo.Where( 
        //    (adj=> (adj.ADJ_PENDG_IND_DESC == "YES" && 
        //        CheckResponsibility("ADJUSTMENTSEARCH", adj.CUSTOMERID, intPersonID) 
        //        && (adj.ADJ_STATUS.ToUpper() == GlobalConstants.AdjustmentStatus.Calc.ToUpper()
        //            || adj.ADJ_STATUS.ToUpper() == GlobalConstants.AdjustmentStatus.Calc.ToUpper()))==false)).ToList();

        lstAdj.DataSource = AdjustmentInfo;
        lstAdj.DataBind();
        if (AdjustmentInfo.Count > 0)
        {
            btnAdjExcel.Visible = true;

        }
        else
        {
            btnAdjExcel.Visible = false;
            //string strMessage = "No Record(s) found...!";
            //ShowError(strMessage);

        }

    }
    /// <summary>
    /// To retrieve Invoice Details
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="perId"></param>
    /// <param name="qcComplete"></param>
    /// <param name="ariesClrng"></param>
    public void GetInvoiceData(int accountId, int perId, int qcComplete, int ariesClrng)
    {
        IList<PremiumAdjustmentBE> invInfo = new List<PremiumAdjustmentBE>();
        string strAriesClrng = "";
        string strQcComplete = "";
        if (qcComplete == 0)
            strQcComplete = "false";
        else if (qcComplete == 1)
            strQcComplete = "true";
        else if (qcComplete == 2)
            strQcComplete = "All";
        if (ariesClrng == 0)
            strAriesClrng = "false";
        else if (ariesClrng == 1)
            strAriesClrng = "true";
        else if (ariesClrng == 2)
            strAriesClrng = "All";
        bool Historical = chkHistorical.Checked ? false : true;
        invInfo = new PremAdjustmentBS().GetInvoiceInfo(accountId, perId, strQcComplete, strAriesClrng, Historical);
        this.lstInv.DataSource = invInfo.ToList();
        lstInv.DataBind();
        if (invInfo.Count > 0)
        {
            btnInvExcel.Visible = true;

        }
        else
        {
            btnInvExcel.Visible = false;
            //string strMessage = "No Record(s) found...!";
            //ShowError(strMessage);

        }
    }

    public void GetCurrentUserInvoiceData(int intPersonID)
    {
        IList<PremiumAdjustmentBE> invInfo = new List<PremiumAdjustmentBE>();

        int intAccountID = 0;
        string strQcComplete = "All";
        // string strAriesClrng = "All";
        string strAriesClrng = ddlAriesClrng.SelectedItem.Text.ToUpper();
        if (strAriesClrng == "YES")
            strAriesClrng = "true";
        if (strAriesClrng == "NO")
            strAriesClrng = "false";

        invInfo = new PremAdjustmentBS().GetInvoiceInfo(intAccountID, intPersonID, strQcComplete, strAriesClrng, false);
        //invInfo = invInfo.Where(inv => inv.VALN_DT >= System.DateTime.Now).ToList();

        //Checks Responsibility
        invInfo = invInfo.Where(inv => (CheckResponsibility("Invoice", inv.CUSTOMERID, intPersonID) == true)).ToList();

        this.lstInv.DataSource = invInfo.ToList();
        lstInv.DataBind();
        if (invInfo.Count > 0)
        {
            btnInvExcel.Visible = true;

        }
        else
        {
            btnInvExcel.Visible = false;
            //string strMessage = "No Record(s) found...!";
            //ShowError(strMessage);


        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        if (ddlAccountName.Items.Count > 0)
            ddlAccountName.SelectedIndex = 0;
        if (ddlStatus.Items.Count > 0)
            ddlStatus.SelectedIndex = 0;
        if (ddlUser.Items.Count > 0)
            ddlUser.SelectedIndex = 0;
        txtFromDt.Text = string.Empty;
        txtToDt.Text = string.Empty;
        if (ddlAriesClrng.Items.Count > 0)
            ddlAriesClrng.SelectedIndex = 0;
        if (ddlPending.Items.Count > 0)
            ddlPending.SelectedIndex = 0;
        if (ddlQcCompltd.Items.Count > 0)
            ddlQcCompltd.SelectedIndex = 0;

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        InitialSearch = false;
        string startDate = string.Empty; // = DateTime.Today;
        string endDate = string.Empty;   // = DateTime.Today;
        int perId = Convert.ToInt32(ddlUser.SelectedValue);
        int status = 0;
        if (TabConDashboard.ActiveTabIndex == 0 || TabConDashboard.ActiveTabIndex == 1)
            status = Convert.ToInt32(ddlStatus.SelectedValue);
        int accountId = Convert.ToInt32(ddlAccountName.SelectedValue);
        int pending = Convert.ToInt32(ddlPending.SelectedValue);
        int qcComplete = Convert.ToInt32(ddlQcCompltd.SelectedValue);
        int ariesClrng = Convert.ToInt32(ddlAriesClrng.SelectedValue);
        if (txtFromDt.Text != string.Empty)
            startDate = txtFromDt.Text;
        if (txtToDt.Text != string.Empty)
            endDate = txtToDt.Text;
        if (startDate != string.Empty)
        {
            if (endDate == string.Empty)
            {
                string strMessage = "Please enter To date";
                ShowError(strMessage);
                return;
            }
        }
        if (endDate != string.Empty)
        {
            if (startDate == string.Empty)
            {
                string strMessage = "Please enter From date";
                ShowError(strMessage);
                return;
            }
        }


        if (perId == 0 && status == 0 &&
            accountId == 0 &&
            startDate == string.Empty && endDate == string.Empty && TabConDashboard.ActiveTabIndex == 0)
        {

            string strMessage = "Please enter data into at least one of the search criteria prior to clicking Search button";
            ShowError(strMessage);
            return;
        }


        else
        {
            if (TabConDashboard.ActiveTabIndex == 0)
                //For Upcoming Valuations & Program Period Details tab
                GetProgramPeriodData(accountId, status, perId, startDate, endDate);
            if (TabConDashboard.ActiveTabIndex == 1)
                //For Adjustment Details tab
                GetAdjustmentData(accountId, status, perId, pending);
            if (TabConDashboard.ActiveTabIndex == 2)
                //For Invoice Details tab
                GetInvoiceData(accountId, perId, qcComplete, ariesClrng);
        }
    }

    protected void lstPp_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {

        string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
        Label lblValDate = (Label)e.Item.FindControl("lblFinalAdjDt");
        Label lblFirstValDate = (Label)e.Item.FindControl("lblFirstAdjDt");

        Label lblHdPremAdjPgmId = (Label)e.Item.FindControl("lblHdPremAdjPgmId");
        int accountId = int.Parse(commandArgs[0].ToString());
        DateTime valDate;
        AISMasterEntities = new MasterEntities();

        if (lblValDate.Text != "")
        {
            valDate = DateTime.Parse(lblValDate.Text);
            AISMasterEntities.ValuationDate = valDate;
        }
        else
        {
            valDate = DateTime.Parse(lblFirstValDate.Text);
            AISMasterEntities.ValuationDate = valDate;
        }
        AccountBE accountBE = (new AccountBS()).Retrieve(accountId);
        AISMasterEntities.AccountNumber = accountBE.CUSTMR_ID;
        AISMasterEntities.AccountName = accountBE.FULL_NM;
        AISMasterEntities.Bpnumber = accountBE.FINC_PTY_ID;
        AISMasterEntities.SSCGID = accountBE.SUPRT_SERV_CUSTMR_GP_ID;

        if (e.CommandName.ToUpper() == "DETAILS")
        {
            //Response.Redirect("~/AcctSetup/ProgramPeriod.aspx");
            ResponseRedirect("~/AcctSetup/ProgramPeriod.aspx");
        }
        if (e.CommandName.ToUpper() == "LOSSINFO")
        {
            Label lblHdAdjNum = (Label)e.Item.FindControl("lblHdAdjNum");
            Label lblAdjstatus = (Label)e.Item.FindControl("lblAdjStatus");
            //Session["refProgramPeriod"] = null;
            //Session["refPremAdjPgmID"] = null;
            SaveObjectToSessionUsingWindowName("refProgramPeriod", null);
            SaveObjectToSessionUsingWindowName("refPremAdjPgmID", null);
            AISMasterEntities.PremiumAdjProgramID = lblHdPremAdjPgmId.Text;
            AISMasterEntities.AccountStatus = null;
            //Here we assigned zero bcox based on this PremAdjID we are filtering the records to bind Program Period Drop Down in LossInfo
            //If PremAdjID is Zero then we are binding Based Prem_adj_pgm_ID else we are Binding Based on Prem_adj_ID
            //AISMasterEntities.ExcessLoss.PremiumAdjID = 0;
            AISMasterEntities.AdjustmentStatus = "Upcoming";
            //Session["Adjdtls"] = "LossInfo";
            SaveObjectToSessionUsingWindowName("Adjdtls", "LossInfo");
            //Response.Redirect("~/Loss/LossInfo.aspx");
            ResponseRedirect("~/Loss/LossInfo.aspx");
        }
        if (e.CommandName.ToUpper() == "ADJDASHBOARD")
        {
            Label lblHdAdjNum = (Label)e.Item.FindControl("lblHdAdjNum");
            //AISMasterEntities.AdjusmentNumber = Convert.ToInt32(lblHdAdjNum.Text);
            AISMasterEntities.AccountName = accountBE.FULL_NM;
            string strPath = "/AdjCalc/InvoicingDashboard.aspx?AcctNo=" + accountBE.CUSTMR_ID.ToString() + "&AdjNO=" + lblHdAdjNum.Text + "&AcctNm=" + accountBE.FULL_NM.Trim();
            //Response.Redirect(strPath);
            ResponseRedirect(strPath);
        }

    }

    protected void lstInvoice_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "GETPDF")
        {
            string strZDWKey = e.CommandArgument.ToString();
            if (strZDWKey == "" || strZDWKey == null)
            {
                ShowError("Report is not available");
                return;
            }
            DownloadFile(strZDWKey);
        }
        else
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            Label lblValDate = (Label)e.Item.FindControl("lblValDate");
            Label lblHdPremAdjPgmId = (Label)e.Item.FindControl("lblHdPremAdjPgmId");
            Label lblPremAdjId = (Label)e.Item.FindControl("lblAdjNum");
            Label lblHdEffDt = (Label)e.Item.FindControl("lblHdEffDt");
            Label lblInvNum = (Label)e.Item.FindControl("lblInvNum");
            Label lblInvDt = (Label)e.Item.FindControl("lblInvDt");
            int accountId = int.Parse(commandArgs[0].ToString());
            DateTime valDate = DateTime.Parse(lblValDate.Text);
            AccountBE accountBE = (new AccountBS()).Retrieve(accountId);
            AISMasterEntities = new MasterEntities();
            AISMasterEntities.AccountNumber = accountBE.CUSTMR_ID;
            AISMasterEntities.AccountName = accountBE.FULL_NM;
            AISMasterEntities.Bpnumber = accountBE.FINC_PTY_ID;
            AISMasterEntities.SSCGID = accountBE.SUPRT_SERV_CUSTMR_GP_ID;
            AISMasterEntities.AdjusmentNumber = Convert.ToInt32(lblPremAdjId.Text);
            if (e.CommandName.ToUpper() == "QC")
            {
                AISMasterEntities.InvoiceNumber = lblInvNum.Text;
                AISMasterEntities.ValuationDate = valDate;
                AISMasterEntities.AdjustmentDate = DateTime.Parse(lblHdEffDt.Text);

                //Response.Redirect("~/AcctMgmt/QCDetails.aspx");
                ResponseRedirect("~/AcctMgmt/QCDetails.aspx");
            }
            if (e.CommandName.ToUpper() == "ARIES")
            {
                if (lblInvDt.Text != String.Empty)

                    AISMasterEntities.InvoiceDate = DateTime.Parse(lblInvDt.Text);
                AISMasterEntities.PremiumAdjProgramID = lblHdPremAdjPgmId.Text;
                AISMasterEntities.InvoiceNumber = lblInvNum.Text;/*The developer of ariesclearing page no longer need invoice number to be passed.*/
                AISMasterEntities.ValuationDate = valDate;
                //10490
                int perId = Convert.ToInt32(ddlUser.SelectedValue);
                AISMasterEntities.PersonIdAries = perId;
                //End
                //Response.Redirect("~/AcctMgmt/AriesClearing.aspx");
                ResponseRedirect("~/AcctMgmt/AriesClearing.aspx");
            }
            if (e.CommandName.ToUpper() == "LOSSINFO")
            {
                //Session["refProgramPeriod"] = null;
                //Session["refPremAdjPgmID"] = null;
                SaveObjectToSessionUsingWindowName("refProgramPeriod", null);
                SaveObjectToSessionUsingWindowName("refPremAdjPgmID", null);
                AISMasterEntities.ValuationDate = valDate;
                AISMasterEntities.PremiumAdjProgramID = lblHdPremAdjPgmId.Text;
                //AISMasterEntities.ExcessLoss.PremiumAdjID = Convert.ToInt32(lblPremAdjId.Text);
                AISMasterEntities.AdjusmentNumber = Convert.ToInt32(lblPremAdjId.Text);
                //Session["Adjdtls"] = "Invoice";
                SaveObjectToSessionUsingWindowName("Adjdtls", "Invoice");
                //Response.Redirect("~/Loss/LossInfo.aspx");
                ResponseRedirect("~/Loss/LossInfo.aspx");
            }
        }

    }
    /// <summary>
    /// Function to Download the PDF file from ZDW by calling the Retrieve method of Webservice
    /// </summary>
    /// <param name="FileName"></param>
    #region DownloadFile
    public void DownloadFile(string Key)
    {
        string Url = this.Page.ResolveUrl("~/Invoice/DownloadFile.aspx") + "?ZDWKey=" + Key;
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "key", "<script language='javascript'>window.open('" + Url + "', null);</script>", false);
    }
    #endregion
    protected void lstAdj_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {

        if (e.CommandName.ToUpper() != "SORT")
        {
            Label lblValDate = (Label)e.Item.FindControl("lblValDate");
            Label lblPremAdjId = (Label)e.Item.FindControl("lblAdjNum");
            int accountId = int.Parse(e.CommandArgument.ToString());
            DateTime valDate = DateTime.Parse(lblValDate.Text);
            AccountBE accountBE = (new AccountBS()).Retrieve(accountId);
            AISMasterEntities = new MasterEntities();
            AISMasterEntities.AccountNumber = accountBE.CUSTMR_ID;
            AISMasterEntities.AccountName = accountBE.FULL_NM;
            AISMasterEntities.Bpnumber = accountBE.FINC_PTY_ID == null ? "" : accountBE.FINC_PTY_ID.ToString();
            AISMasterEntities.SSCGID = accountBE.SUPRT_SERV_CUSTMR_GP_ID == null ? "" : accountBE.SUPRT_SERV_CUSTMR_GP_ID.ToString();
            AISMasterEntities.ValuationDate = valDate;
            AISMasterEntities.AdjusmentNumber = Convert.ToInt32(lblPremAdjId.Text);
            if (e.CommandName.ToUpper() == "DETAILS")
            {
                Label lblAdjStatus = (Label)e.Item.FindControl("lblAdjStatus");

                Label lblAdjDt = (Label)e.Item.FindControl("lblAdjDt");
                AISMasterEntities.AdjustmentDate = Convert.ToDateTime(lblAdjDt.Text);

                switch (lblAdjStatus.Text.ToUpper())
                {
                    case GlobalConstants.AdjustmentStatus.Calc:
                        // Response.Redirect("~/AcctMgmt/AdjustmentQC.aspx");
                        //Response.Redirect("~/AdjCalc/InvoicingDashboard.aspx");
                        //Response.Redirect("../AdjCalc/InvoicingDashboard.aspx?AcctNo=" + accountBE.CUSTMR_ID.ToString() + "&AdjNO=" + lblPremAdjId.Text + "&AcctNm=" + accountBE.FULL_NM.Trim());
                        ResponseRedirect("~/AdjCalc/InvoicingDashboard.aspx?AcctNo=" + accountBE.CUSTMR_ID.ToString() + "&AdjNO=" + lblPremAdjId.Text + "&AcctNm=" + accountBE.FULL_NM.Trim());

                        break;
                    case GlobalConstants.AdjustmentStatus.DraftInvd:
                        //Response.Redirect("~/AcctMgmt/AdjustmentQC.aspx");
                        ResponseRedirect("~/AcctMgmt/AdjustmentQC.aspx");
                        break;
                    case GlobalConstants.AdjustmentStatus.QCdDraftInv:
                        //CurrentAISUser.PersonID
                        //
                        //Fixed for #7965
                        //uncommented LSSAdmin role as part of requirement change.
                        // if (CheckResponsibility("LSSAdmin", accountBE.CUSTMR_ID, CurrentAISUser.PersonID))
                        //    Response.Redirect("~/AcctMgmt/AdjustmentQC.aspx");
                        // else
                        //Response.Redirect("~/AcctMgmt/ReconReview.aspx");
                        ResponseRedirect("~/AcctMgmt/ReconReview.aspx");
                        break;
                }

            }
            if (e.CommandName.ToUpper() == "LOSSINFO")
            {
                //Session["refProgramPeriod"] = null;
                //Session["refPremAdjPgmID"] = null;
                SaveObjectToSessionUsingWindowName("refProgramPeriod", null);
                SaveObjectToSessionUsingWindowName("refPremAdjPgmID", null);
                Label lblHdPremAdjPgmId = (Label)e.Item.FindControl("lblHdPremAdjPgmId");
                Label lblAdjsts = (Label)e.Item.FindControl("lblAdjStatus");
                AISMasterEntities.PremiumAdjProgramID = lblHdPremAdjPgmId.Text;
                AISMasterEntities.Bpnumber = accountBE.FINC_PTY_ID;
                AISMasterEntities.SSCGID = accountBE.SUPRT_SERV_CUSTMR_GP_ID;
                AISMasterEntities.AdjusmentNumber = Convert.ToInt32(lblPremAdjId.Text);
                AISMasterEntities.AdjustmentStatus = lblAdjsts.Text;
                //Session["Adjdtls"] = "Adjustmentdetails";
                SaveObjectToSessionUsingWindowName("Adjdtls", "Adjustmentdetails");
                //AISMasterEntities.ExcessLoss.PremiumAdjID = Convert.ToInt32(lblPremAdjId.Text);
                //Response.Redirect("~/Loss/LossInfo.aspx");
                ResponseRedirect("~/Loss/LossInfo.aspx");
            }
            else if (e.CommandName.ToUpper() == "ADJFEEDBACK")
            {
                Label lblAdjStatus = (Label)e.Item.FindControl("lblAdjStatus");
                Label lblAdjDt = (Label)e.Item.FindControl("lblAdjDt");
                AISMasterEntities.AdjustmentDate = Convert.ToDateTime(lblAdjDt.Text);
                AISMasterEntities.AdjusmentNumber = Convert.ToInt32(lblPremAdjId.Text);
                //Response.Redirect("~/AcctMgmt/AdjustmentQC.aspx?feedback=yes");
                ResponseRedirect("~/AcctMgmt/AdjustmentQC.aspx?feedback=yes");
            }
            else if (e.CommandName.ToUpper() == "RECONFEEDBACK")
            {
                Label lblAdjStatus = (Label)e.Item.FindControl("lblAdjStatus");
                Label lblAdjDt = (Label)e.Item.FindControl("lblAdjDt");
                AISMasterEntities.AdjustmentDate = Convert.ToDateTime(lblAdjDt.Text);
                AISMasterEntities.AdjusmentNumber = Convert.ToInt32(lblPremAdjId.Text);
                //Response.Redirect("~/AcctMgmt/ReconReview.aspx?feedback=yes");
                ResponseRedirect("~/AcctMgmt/ReconReview.aspx?feedback=yes");
            }
        }
    }

    protected void lstAdj_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            Label lblCustmrID = (Label)e.Item.FindControl("lblActNum");
            Label lblAdjStatus = (Label)e.Item.FindControl("lblAdjStatus");
            Label lblPending = (Label)e.Item.FindControl("lblPending");
            LinkButton lbnDetails = (LinkButton)e.Item.FindControl("lbnDetails");

            if ((lblAdjStatus.Text.ToUpper() == GlobalConstants.AdjustmentStatus.Calc.ToUpper() || lblAdjStatus.Text.ToUpper() == GlobalConstants.AdjustmentStatus.DraftInvd.ToUpper() || lblAdjStatus.Text.ToUpper() == GlobalConstants.AdjustmentStatus.QCdDraftInv.ToUpper()) && lblPending.Text != "YES")
                lbnDetails.Enabled = true;
            else
                lbnDetails.Enabled = false;

            int custmrID = Convert.ToInt32(lblCustmrID.Text);

            //if (!IsPostBack)
            //{
            //    if (CheckResponsibility("Adjustment", custmrID, CurrentAISUser.PersonID))
            //    {
            //        //Fixed for Bug #7939
            //        if (!CheckResponsibility("ADJUSTMENTSEARCH", custmrID, CurrentAISUser.PersonID))
            //        {
            //            if (lblPending.Text.Trim().ToUpper() == "YES" &&
            //                (lblAdjStatus.Text.ToUpper() == GlobalConstants.AdjustmentStatus.Calc.ToUpper()
            //                || lblAdjStatus.Text.ToUpper() == GlobalConstants.AdjustmentStatus.DraftInvd.ToUpper()))
            //            {
            //                e.Item.Visible = false;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        e.Item.Visible = false;
            //    }
            //}

        }
    }

    protected void lstAdj_Sorting(object sender, ListViewSortEventArgs e)
    {
        ImageButton imgFPBtn = new ImageButton();
        imgFPBtn = (ImageButton)lstAdj.FindControl("imgAdjValDtSort");

        e.SortDirection = (imgFPBtn.ImageUrl.Contains("Des")) ?
                            SortDirection.Ascending : SortDirection.Descending;

        if (e.SortDirection == SortDirection.Ascending)
            AdjustmentInfo = AdjustmentInfo.OrderBy(sl => sl.EFF_DT).OrderBy(sl => sl.ADJ_STATUS).OrderBy(sl => sl.VALN_DT).ToList();
        else
            AdjustmentInfo = AdjustmentInfo.OrderBy(sl => sl.EFF_DT).OrderBy(sl => sl.ADJ_STATUS).OrderByDescending(sl => sl.VALN_DT).ToList();

        ChangeImage(imgFPBtn, e.SortDirection);

        this.lstAdj.DataSource = AdjustmentInfo.Where(prem => prem.PREM_ADJ_PGM_ID != 0).ToList();
        lstAdj.DataBind();
    }
    protected void lstInv_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            Label lblCustmrID = (Label)e.Item.FindControl("lblActNum");
            HiddenField hidQC = (HiddenField)e.Item.FindControl("twentypercent");
            LinkButton lbtnQc = (LinkButton)e.Item.FindControl("lbnQc");
            if (hidQC.Value.ToUpper() == "TRUE")
            {
                lbtnQc.Enabled = true;
            }
            else
            {
                lbtnQc.Enabled = false;
            }

            //if (!IsPostBack)
            //{
            //    int custmrID = Convert.ToInt32(lblCustmrID.Text);
            //    e.Item.Visible = CheckResponsibility("Invoice", custmrID, CurrentAISUser.PersonID);
            //}
        }
    }

    private void ChangeImage(ImageButton imgBtn, SortDirection sDir)
    {
        if (sDir == SortDirection.Ascending)
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
    }

    private void AddStyle(TableCell[] cells, bool isHeader)
    {
        foreach (TableCell cell in cells)
        {
            cell.Style[HtmlTextWriterStyle.BorderColor] = "Black";
            cell.Style[HtmlTextWriterStyle.BorderStyle] = BorderStyle.Solid.ToString();
            cell.Style[HtmlTextWriterStyle.BorderWidth] = "1";
            if (isHeader)
                cell.Style[HtmlTextWriterStyle.FontStyle] = "bold";
        }
    }

    private void ExportToExcel(string fileName, Table tbl)
    {
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
        Response.Charset = "";
        Response.ContentType = "application/vnd.xls";
        StringWriter strWriter = new System.IO.StringWriter();
        HtmlTextWriter htmlWriter = new HtmlTextWriter(strWriter);

        tbl.Style[HtmlTextWriterStyle.BorderColor] = "Black";
        tbl.Style[HtmlTextWriterStyle.BorderStyle] = BorderStyle.Solid.ToString();
        tbl.Style[HtmlTextWriterStyle.BorderWidth] = "1";

        tbl.RenderControl(htmlWriter);
        Response.Write(strWriter.ToString());
        Response.End();
    }

    // AWS Cloud migration - SpreadsheetGear changes 
    public static void FormatHeaderTypeCellView(SSG.IRange cell, SSG.HAlign align = SSG.HAlign.Left, SSG.VAlign valign = SSG.VAlign.Center, bool isBold = true)
    {
        cell.Font.Bold = isBold;
        cell.HorizontalAlignment = align;
        cell.Borders.Color = SSG.Colors.Black;
        cell.VerticalAlignment = valign;

    }

    // AWS Cloud migration - SpreadsheetGear changes 
    public static void FormatTypeCellView(SSG.IRange cell, SSG.HAlign align = SSG.HAlign.Left, SSG.VAlign valign = SSG.VAlign.Center, bool isBold = false)
    {
        cell.Font.Bold = isBold;
        cell.HorizontalAlignment = align;
        cell.Borders.Color = SSG.Colors.Black;
        cell.VerticalAlignment = valign;

    }

    // AWS Cloud migration - SpreadsheetGear changes 
    private void ExportToExcelusingSpreadsheetGear(DataTable Tbl, string ExcelFilePath, string strFileName, string SheetName)
    {
        try
        {
            if (Tbl == null || Tbl.Columns.Count == 0)
                throw new Exception("ExportToExcel: Null or empty input table!\n");
            // Create a new workbook and worksheet.
            SSG.IWorkbook workbook = SSG.Factory.GetWorkbook();
            int sheetIndex = 0;
            int cellIndex = 0;

            if (Tbl.Columns.Count > 0)
            {
                workbook.Worksheets.Add();
                sheetIndex++;
                // column headings
                SSG.IWorksheet worksheetViewAccountDashboard = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                worksheetViewAccountDashboard.Name = SheetName;

                // Get the worksheet cells reference. 
                SSG.IRange cellsviewAccountDashboard = worksheetViewAccountDashboard.Cells;
                cellIndex = 1;

                for (int i = 0; i < Tbl.Columns.Count; i++)
                {
                    String columname = string.Empty;
                    int columnindexvalue = i + 1;
                    columname = getColumnNamefromIndex(columnindexvalue);
                    cellsviewAccountDashboard[columname + cellIndex.ToString()].Value = Tbl.Columns[i].ColumnName;
                    FormatHeaderTypeCellView(cellsviewAccountDashboard[columname + cellIndex.ToString()]);
                }
                cellIndex = cellIndex + 1;
                //rows
                for (int i = 0; i < Tbl.Rows.Count; i++)
                {
                    // to do: format datetime values before printing
                    for (int j = 0; j < Tbl.Columns.Count; j++)
                    {

                        String columname = string.Empty;
                        int columnindexvalue = j + 1;
                        columname = getColumnNamefromIndex(columnindexvalue);
                        cellsviewAccountDashboard[columname + cellIndex.ToString()].Value = Tbl.Rows[i][j];
                        FormatTypeCellView(cellsviewAccountDashboard[columname + cellIndex.ToString()]);
                    }

                    cellIndex = cellIndex + 1;
                }

                worksheetViewAccountDashboard.UsedRange.Columns.AutoFit();
                for (int col = 0; col < worksheetViewAccountDashboard.UsedRange.ColumnCount; col++)
                {
                    worksheetViewAccountDashboard.Cells[1, col].ColumnWidth *= 1.15;
                }
            }

            //Delete original blank empty sheet from report workbook.
            workbook.ActiveWorksheet.Delete();

            if (sheetIndex == 0)
            {
                workbook.Worksheets.Add();
            }

            if (ExcelFilePath != null && ExcelFilePath != "")
            {

                try
                {
                    workbook.Worksheets[0].Select();
                    string strFileNamePath = ExcelFilePath;
                    string strFileNameDecd = Server.HtmlDecode(Server.HtmlEncode(strFileNamePath));
                    workbook.SaveAs(strFileNameDecd, SSG.FileFormat.OpenXMLWorkbook);
                    workbook.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                        + ex.Message);
                }


            }
            else
            {
                throw new Exception("ExportToExcel: File path is not given!\n");
            }



            MemoryStream memStream = new MemoryStream();

            try
            {
                using (FileStream fileStream = File.OpenRead(ConfigurationManager.AppSettings["AccountDashboard_Excel_Export"] + "\\" + strFileName))
                {

                    memStream.SetLength(fileStream.Length);
                    fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
                    byte[] byteArray = memStream.ToArray();
                    Response.Clear();
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                    Response.AddHeader("content-length", memStream.Length.ToString());
                    Response.BinaryWrite(byteArray);
                }

            }
            catch (Exception ex)
            {
                ShowError("Unable to Preview the Report. Please contact Application Support Team");
                return;
            }
            finally
            {
                memStream.Close();
                Response.Flush();
                Response.End();
                //HttpContext.Current.Response.End();
            }

        }
        catch (Exception ex)
        {
            throw new Exception("ExportToExcel: \n" + ex.Message);
        }
    }

    // AWS Cloud migration - SpreadsheetGear changes 
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

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        // AWS Cloud Migration
        string sSpreadsheetGearExcelFileFormat = ConfigurationManager.AppSettings["SpreadsheetGearExcelFileFormat"].ToString();

        string strDate = DateTime.Now.ToString().Replace(":", "-");
        strDate = strDate.Replace("/", "-");
        string strFileName = "Upcoming Valuations and Programs Report" + "_" + CurrentAISUser.PersonID + "_" + strDate + sSpreadsheetGearExcelFileFormat;

        //COMMENTED FOR AWS CLOUD MIGRATION CHANGES

        //Table tbl = new Table();
        //TableRow tblRow = new TableRow();
        //TableCell[] cell = new TableCell[7];

        //cell[0] = new TableCell();
        //cell[0].Text = "ACCOUNT NAME";
        //cell[1] = new TableCell();
        //cell[1].Text = "PROGRAM PERIOD";
        //cell[2] = new TableCell();
        //cell[2].Text = "PROGRAM TYPE";
        //cell[3] = new TableCell();
        //cell[3].Text = "FIRST VALUATION DATE";
        //cell[4] = new TableCell();
        //cell[4].Text = "NEXT VALUATION DATE";
        //cell[5] = new TableCell();
        //cell[5].Text = "BU/OFFICE";
        //cell[6] = new TableCell();
        //cell[6].Text = "PROGRAM STATUS";



        //AddStyle(cell, true);
        //tblRow.Cells.AddRange(cell);
        //tbl.Rows.Add(tblRow);

        //for (int row = 0; row < lstPp.Items.Count; row++)
        //{
        //    tblRow = new TableRow();

        //    cell[0] = new TableCell();
        //    cell[0].Text = ((Label)lstPp.Items[row].FindControl("lblActName")).Text;
        //    cell[1] = new TableCell();
        //    cell[1].Text = ((Label)lstPp.Items[row].FindControl("lblPrgmPrd")).Text;
        //    cell[2] = new TableCell();
        //    cell[2].Text = ((Label)lstPp.Items[row].FindControl("lblprgmType")).Text;
        //    cell[3] = new TableCell();
        //    cell[3].Text = ((Label)lstPp.Items[row].FindControl("lblFirstAdjDt")).Text;
        //    cell[4] = new TableCell();
        //    cell[4].Text = ((Label)lstPp.Items[row].FindControl("lblFinalAdjDt")).Text;
        //    cell[5] = new TableCell();
        //    cell[5].Text = ((Label)lstPp.Items[row].FindControl("lblBuOffice")).Text;
        //    cell[6] = new TableCell();
        //    cell[6].Text = ((Label)lstPp.Items[row].FindControl("lbnPrgmStatus")).Text;

        //    AddStyle(cell, false);
        //    tblRow.Cells.AddRange(cell);
        //    tbl.Rows.Add(tblRow);
        //}



        DataTable dt = new DataTable();

        dt.Columns.Add("ACCOUNT NAME");
        dt.Columns.Add("PROGRAM PERIOD");
        dt.Columns.Add("PROGRAM TYPE");
        dt.Columns.Add("FIRST VALUATION DATE");
        dt.Columns.Add("NEXT VALUATION DATE");
        dt.Columns.Add("BU/OFFICE");
        dt.Columns.Add("PROGRAM STATUS");


        //InitialSearch = false;
        string startDate = string.Empty; // = DateTime.Today;
        string endDate = string.Empty;   // = DateTime.Today;
        int perId = Convert.ToInt32(ddlUser.SelectedValue);
        int status = 0;
        if (TabConDashboard.ActiveTabIndex == 0 || TabConDashboard.ActiveTabIndex == 1)
            status = Convert.ToInt32(ddlStatus.SelectedValue);
        int accountId = Convert.ToInt32(ddlAccountName.SelectedValue);
        int pending = Convert.ToInt32(ddlPending.SelectedValue);
        int qcComplete = Convert.ToInt32(ddlQcCompltd.SelectedValue);
        int ariesClrng = Convert.ToInt32(ddlAriesClrng.SelectedValue);
        if (txtFromDt.Text != string.Empty)
            startDate = txtFromDt.Text;
        if (txtToDt.Text != string.Empty)
            endDate = txtToDt.Text;


        if (perId == 0 && status == 0 &&
            accountId == 0 &&
            startDate == string.Empty && endDate == string.Empty && TabConDashboard.ActiveTabIndex == 0)
        {

            //string strMessage = "Please enter data into at least one of the search criteria prior to clicking Search button";
            //ShowError(strMessage);
            //return;
        }

        else
        {
            programPeriodInfo = new ProgramPeriodsBS().GetProgramPeriodData(accountId, status, perId, startDate, endDate);
        }

        int i = 0;

        //for (int row = 0; row < programPeriodInfo.Count; row++)
        foreach (ProgramPeriodBE prgPeriod in programPeriodInfo)
        {
            if (dt.Rows.Count == 0)
            {
                i = 0;
            }
            else
            {
                i = i + 1;
            }


            dt.Rows.Add();

            dt.Rows[i][0] = prgPeriod.CUSTMR_NAME;
            dt.Rows[i][1] = prgPeriod.PROGRAMPERIOD;
            dt.Rows[i][2] = Server.HtmlDecode(Server.HtmlEncode(prgPeriod.PROGRAMTYPENAME));
            dt.Rows[i][3] = prgPeriod.FST_ADJ_DT != null ? prgPeriod.FST_ADJ_DT.Value.ToString("MM/dd/yyyy") : "";
            dt.Rows[i][4] = prgPeriod.FST_ADJ_DT != null ? prgPeriod.NXT_VALN_DT.Value.ToString("MM/dd/yyyy") : "";
            dt.Rows[i][5] = Server.HtmlDecode(Server.HtmlEncode(prgPeriod.BUSINESSUNITNAME));
            dt.Rows[i][6] = prgPeriod.PROGRAMSTATUS;


            // COMMENTED FOR AWS CLOUD MIGRATION CHANGES
            //tblRow = new TableRow();

            //cell[0] = new TableCell();
            //cell[0].Text = prgPeriod.CUSTMR_NAME;
            //cell[1] = new TableCell();
            //cell[1].Text = prgPeriod.PROGRAMPERIOD;
            //cell[2] = new TableCell();
            ////06/23 for veracode
            ////cell[2].Text = prgPeriod.PROGRAMTYPENAME;
            //cell[2].Text = Server.HtmlDecode(Server.HtmlEncode(prgPeriod.PROGRAMTYPENAME));
            //cell[3] = new TableCell();
            //cell[3].Text = prgPeriod.FST_ADJ_DT != null ? prgPeriod.FST_ADJ_DT.Value.ToString("MM/dd/yyyy") : "";// Convert.ToString(prgPeriod.FST_ADJ_DT).ToString("MM/dd/yyyy");
            //cell[4] = new TableCell();
            //cell[4].Text = prgPeriod.FST_ADJ_DT != null ? prgPeriod.NXT_VALN_DT.Value.ToString("MM/dd/yyyy") : "";
            //cell[5] = new TableCell();
            ////06/23 for veracode
            ////cell[5].Text = prgPeriod.BUSINESSUNITNAME;
            //cell[5].Text = Server.HtmlDecode(Server.HtmlEncode(prgPeriod.BUSINESSUNITNAME));
            //cell[6] = new TableCell();
            //cell[6].Text = prgPeriod.PROGRAMSTATUS;

            //AddStyle(cell, false);
            //tblRow.Cells.AddRange(cell);
            //tbl.Rows.Add(tblRow);
        }


        //ExportToExcel(strFileName, tbl);

        // AWS Cloud Migration changes
        string strFilePath = ConfigurationManager.AppSettings["AccountDashboard_Excel_Export"] + "\\" + strFileName;
        string SheetName = "Upcoming Valuations And Program";
        ExportToExcelusingSpreadsheetGear(dt, strFilePath, strFileName, SheetName);
    }

    protected void btnAdjExcel_Click(object sender, EventArgs e)
    {
        // AWS Cloud Migration
        string sSpreadsheetGearExcelFileFormat = ConfigurationManager.AppSettings["SpreadsheetGearExcelFileFormat"].ToString();

        string strDate = DateTime.Now.ToString().Replace(":", "-");
        strDate = strDate.Replace("/", "-");
        string strFileName = "Adjustment Details Report" + "_" + CurrentAISUser.PersonID + "_" + strDate + sSpreadsheetGearExcelFileFormat;

        //Commented for AWS Cloud Migration changes

        //Table tbl = new Table();
        //TableRow tblRow = new TableRow();
        //TableCell[] cell = new TableCell[6];

        //cell[0] = new TableCell();
        //cell[0].Text = "ACCOUNT NAME";
        //cell[1] = new TableCell();
        //cell[1].Text = "ADJUSTMENT NUMBER";
        //cell[2] = new TableCell();
        //cell[2].Text = "ADJUSTMENT VAL DATE";
        //cell[3] = new TableCell();
        //cell[3].Text = "ADJUSTMENT STATUS DATE";
        //cell[4] = new TableCell();
        //cell[4].Text = "ADJUSTMENT STATUS";
        //cell[5] = new TableCell();
        //cell[5].Text = "PENDING";

        //AddStyle(cell, true);
        //tblRow.Cells.AddRange(cell);
        //tbl.Rows.Add(tblRow);


        DataTable dt = new DataTable();

        dt.Columns.Add("ACCOUNT NAME");
        dt.Columns.Add("ADJUSTMENT NUMBER");
        dt.Columns.Add("ADJUSTMENT VAL DATE");
        dt.Columns.Add("ADJUSTMENT STATUS DATE");
        dt.Columns.Add("ADJUSTMENT STATUS");
        dt.Columns.Add("PENDING");

        int i = 0;

        for (int row = 0; row < lstAdj.Items.Count; row++)
        {

            if (dt.Rows.Count == 0)
            {
                i = 0;
            }
            else
            {
                i = i + 1;
            }


            dt.Rows.Add();

            dt.Rows[i][0] = ((Label)lstAdj.Items[row].FindControl("lblActName")).Text;
            dt.Rows[i][1] = ((Label)lstAdj.Items[row].FindControl("lblAdjNum")).Text;
            dt.Rows[i][2] = ((Label)lstAdj.Items[row].FindControl("lblValDate")).Text;
            dt.Rows[i][3] = ((Label)lstAdj.Items[row].FindControl("lblAdjDt")).Text;
            dt.Rows[i][4] = ((Label)lstAdj.Items[row].FindControl("lblAdjStatus")).Text;
            dt.Rows[i][5] = ((Label)lstAdj.Items[row].FindControl("lblPending")).Text;

            // Commented for AWS Cloud Migration Changes

            //tblRow = new TableRow();

            //cell[0] = new TableCell();
            //cell[0].Text = ((Label)lstAdj.Items[row].FindControl("lblActName")).Text;
            //cell[1] = new TableCell();
            //cell[1].Text = ((Label)lstAdj.Items[row].FindControl("lblAdjNum")).Text;
            //cell[2] = new TableCell();
            //cell[2].Text = ((Label)lstAdj.Items[row].FindControl("lblValDate")).Text;
            //cell[3] = new TableCell();
            //cell[3].Text = ((Label)lstAdj.Items[row].FindControl("lblAdjDt")).Text;
            //cell[4] = new TableCell();
            //cell[4].Text = ((Label)lstAdj.Items[row].FindControl("lblAdjStatus")).Text;
            //cell[5] = new TableCell();
            //cell[5].Text = ((Label)lstAdj.Items[row].FindControl("lblPending")).Text;

            //AddStyle(cell, false);
            //tblRow.Cells.AddRange(cell);
            //tbl.Rows.Add(tblRow);
        }

        // Commented for AWS Cloud Migration Changes
        //ExportToExcel(strFileName, tbl);

        string strFilePath = ConfigurationManager.AppSettings["AccountDashboard_Excel_Export"] + "\\" + strFileName;
        string SheetName = "Adjustment Details Report";
        ExportToExcelusingSpreadsheetGear(dt, strFilePath, strFileName, SheetName);
    }

    protected void btnInvExcel_Click(object sender, EventArgs e)
    {
        // AWS Cloud Migration
        string sSpreadsheetGearExcelFileFormat = ConfigurationManager.AppSettings["SpreadsheetGearExcelFileFormat"].ToString();

        string strDate = DateTime.Now.ToString().Replace(":", "-");
        strDate = strDate.Replace("/", "-");
        string strFileName = "Invoice Details Report" + "_" + CurrentAISUser.PersonID + "_" + strDate + sSpreadsheetGearExcelFileFormat;

        //Commented for AWS Cloud Migration changes

        //Table tbl = new Table();
        //TableRow tblRow = new TableRow();
        //TableCell[] cell = new TableCell[7];

        //cell[0] = new TableCell();
        //cell[0].Text = "ACCT. NUMBER";
        //cell[1] = new TableCell();
        //cell[1].Text = "ACCOUNT NAME";
        //cell[2] = new TableCell();
        //cell[2].Text = "VALUATION DATE";
        //cell[3] = new TableCell();
        //cell[3].Text = "ADJ. NUMBER";
        //cell[4] = new TableCell();
        //cell[4].Text = "INVOICE NUMBER";
        //cell[5] = new TableCell();
        //cell[5].Text = "INVOICE DATE";
        //cell[6] = new TableCell();
        //cell[6].Text = "INVOICE AMOUNT";

        //AddStyle(cell, true);
        //tblRow.Cells.AddRange(cell);
        //tbl.Rows.Add(tblRow);

        DataTable dt = new DataTable();

        dt.Columns.Add("ACCT. NUMBER");
        dt.Columns.Add("ACCOUNT NAME");
        dt.Columns.Add("VALUATION DATE");
        dt.Columns.Add("ADJ. NUMBER");
        dt.Columns.Add("INVOICE NUMBER");
        dt.Columns.Add("INVOICE DATE");
        dt.Columns.Add("INVOICE AMOUNT");

        int i = 0;

        for (int row = 0; row < lstInv.Items.Count; row++)
        {

            if (dt.Rows.Count == 0)
            {
                i = 0;
            }
            else
            {
                i = i + 1;
            }


            dt.Rows.Add();

            dt.Rows[i][0] = ((Label)lstInv.Items[row].FindControl("lblActNum")).Text;
            dt.Rows[i][1] = ((Label)lstInv.Items[row].FindControl("lblActName")).Text;
            dt.Rows[i][2] = ((Label)lstInv.Items[row].FindControl("lblValDate")).Text;
            dt.Rows[i][3] = ((Label)lstInv.Items[row].FindControl("lblAdjNum")).Text;
            dt.Rows[i][4] = ((Label)lstInv.Items[row].FindControl("lblInvNum")).Text;
            dt.Rows[i][5] = ((Label)lstInv.Items[row].FindControl("lblInvDt")).Text;
            dt.Rows[i][6] = ((Label)lstInv.Items[row].FindControl("lblInvoiceAmt")).Text;

            // commented for AWS Cloud Migration changes

            //tblRow = new TableRow();

            //cell[0] = new TableCell();
            //cell[0].Text = ((Label)lstInv.Items[row].FindControl("lblActNum")).Text;
            //cell[1] = new TableCell();
            //cell[1].Text = ((Label)lstInv.Items[row].FindControl("lblActName")).Text;
            //cell[2] = new TableCell();
            //cell[2].Text = ((Label)lstInv.Items[row].FindControl("lblValDate")).Text;
            //cell[3] = new TableCell();
            //cell[3].Text = ((Label)lstInv.Items[row].FindControl("lblAdjNum")).Text;
            //cell[4] = new TableCell();
            //cell[4].Text = ((Label)lstInv.Items[row].FindControl("lblInvNum")).Text;
            //cell[5] = new TableCell();
            //cell[5].Text = ((Label)lstInv.Items[row].FindControl("lblInvDt")).Text;
            //cell[6] = new TableCell();
            //cell[6].Text = ((Label)lstInv.Items[row].FindControl("lblInvoiceAmt")).Text;

            //AddStyle(cell, false);
            //tblRow.Cells.AddRange(cell);
            //tbl.Rows.Add(tblRow);
        }

        // commented for AWS Cloud Migration changes

        //ExportToExcel(strFileName, tbl);

        string strFilePath = ConfigurationManager.AppSettings["AccountDashboard_Excel_Export"] + "\\" + strFileName;
        string SheetName = "Invoice Details Report";
        ExportToExcelusingSpreadsheetGear(dt, strFilePath, strFileName, SheetName);
    }
    /// <summary>
    /// Used the pager in the upcoming valuation tab grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstPpDataPager_PreRender(object sender, EventArgs e)
    {
        //10521 Bug Fix -- commented the below line
        if (UserDropReload == false && TabConDashboard.ActiveTabIndex == 0)
        //if (TabConDashboard.ActiveTabIndex == 0)
        {
            string startDate = string.Empty; // = DateTime.Today;
            string endDate = string.Empty;   // = DateTime.Today;
            int perId = Convert.ToInt32(ddlUser.SelectedValue);
            int status = 0;
            if (TabConDashboard.ActiveTabIndex == 0 || TabConDashboard.ActiveTabIndex == 1)
                status = Convert.ToInt32(ddlStatus.SelectedValue);
            int accountId = Convert.ToInt32(ddlAccountName.SelectedValue);
            if (txtFromDt.Text != string.Empty)
                startDate = txtFromDt.Text;
            if (txtToDt.Text != string.Empty)
                endDate = txtToDt.Text;
            if (InitialSearch == true)
            {
                GetCurrentUserProgramPeriodData(perId);
            }
            else
            {
                GetProgramPeriodData(accountId, status, perId, startDate, endDate);
            }

            this.lstPp.DataSource = programPeriodInfo.ToList();
            lstPp.DataBind();
        }
    }

}
