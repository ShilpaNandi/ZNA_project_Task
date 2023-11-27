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
using System.IO;
using System.Collections.Generic;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.LSP.Framework;
using ZurichNA.AIS.WebSite.ZDWJavaWS;
using Microsoft.Web.Services3.Security.Tokens;

public partial class AcctMgmt_AdjustmentQC : AISBasePage
{
    private AdjProcChklstBS adjSetupChklstItemService;
    private AdjQCBS adjQCItemService;
    private Qtly_Cntrl_ChklistBS qltyCntrlListService;
    /// <summary>
    /// a property for AdjustmentSetupQC Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>AdjQCBS</returns>
    private AdjQCBS AdjQCItemService
    {
        get
        {
            if (adjQCItemService == null)
            {
                adjQCItemService = new AdjQCBS();

            }
            return adjQCItemService;
        }
        set
        {
            adjQCItemService = value;
        }
    }
    /// <summary>
    /// a property for AdjustmentProcessing Checklist Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>AdjProcChklstBS</returns>
    private AdjProcChklstBS AdjSetupChklstItemService
    {
        get
        {
            if (adjSetupChklstItemService == null)
            {
                adjSetupChklstItemService = new AdjProcChklstBS();

            }
            return adjSetupChklstItemService;
        }
        set
        {
            adjSetupChklstItemService = value;
        }
    }
    /// <summary>
    /// a property for Quality control Checklist Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>Qtly_Cntrl_ChklistBS</returns>
    private Qtly_Cntrl_ChklistBS QltyCntrlListService
    {
        get
        {
            if (qltyCntrlListService == null)
            {
                qltyCntrlListService = new Qtly_Cntrl_ChklistBS();

            }
            return qltyCntrlListService;
        }
        set
        {
            qltyCntrlListService = value;
        }
    }
    /// <summary>
    /// a property for PremiumAdjustmentStatus Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>PremiumAdjustmentStatusBE</returns>
    private PremiumAdjustmentStatusBE PremAdjStatusBE
    {
        get
        {
            return ((Session["PREMADJSTATUSBE"] == null) ?
                (new PremiumAdjustmentStatusBE()) : (PremiumAdjustmentStatusBE)Session["PREMADJSTATUSBE"]);
        }

        set { Session["PREMADJSTATUSBE"] = value; }
    }
    private PremiumAdjustmentStatusBE PremAdjStatusBEAdjqcOld
    {
        get
        {
            return ((Session["PremAdjStatusBEAdjqcOld"] == null) ?
                (new PremiumAdjustmentStatusBE()) : (PremiumAdjustmentStatusBE)Session["PremAdjStatusBEAdjqcOld"]);
        }

        set { Session["PremAdjStatusBEAdjqcOld"] = value; }
    }
    /// <summary>
    /// a property for Quality control Checklist Enity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>Qtly_Cntrl_ChklistBE</returns>
    private IList<Qtly_Cntrl_ChklistBE> QltyCntrlListBE
    {
        get { return (IList<Qtly_Cntrl_ChklistBE>)Session["QLTYCNTRLLISTBE"]; }
        set { Session["QLTYCNTRLLISTBE"] = value; }

    }
    protected ArrayList QltyCntrlLstIDlist
    {
        get
        {
            if (ViewState["QltyCntrlChklstID"] != null)
            {
                return (ArrayList)ViewState["QltyCntrlChklstID"];
            }
            else
            {
                ViewState["QltyCntrlChklstID"] = null;
                return null;
            }
        }
        set
        {
            ViewState["QltyCntrlChklstID"] = value;
        }
    }
    /// <summary>
    /// page load event
    /// </summary>
    /// <param name=""></param>
    /// <returns>Qtly_Cntrl_ChklistBE</returns>
    protected void Page_Load(object sender, EventArgs e)
    {


        //this.ucSaveCancel.OperationsButtonClicked += new EventHandler(btnSaveCancel_OperationsButtonClicked);
        if (!IsPostBack)
        {
            this.Master.Page.Title = "AdjustmentQC";
            if (CurrentAISUser.PersonID <= 0)
            {
                pnlAdjQC.Enabled = false;
                pnlAdjQCDetails.Enabled = false;
                ShowError("UserID/AZCORPID not registered. Please register using Internal Contacts web page.");
                return;
            }
            //lstAdjQCDetails.Enabled = false;
            //lblAdjQCDetails.Visible = true;
            QltyCntrlListBE = new List<Qtly_Cntrl_ChklistBE>();
            if (Request.QueryString.Count > 0)
            {
                lblAdjQCDetails.Visible = false;
                btnDisplay.Visible = true;
                ddlQCDate.Visible = true;
                btnDisplay.Enabled = true;
                ddlQCDate.Enabled = true;
                DataTable dtRecon = (new PremiumAdjustmentStatusBS()).getReviewFeedback(AISMasterEntities.AdjusmentNumber, 1);
                ddlQCDate.DataSource = dtRecon;
                ddlQCDate.DataTextField = "QltyDate";
                ddlQCDate.DataValueField = "prem_adj_sts_id";
                ddlQCDate.DataBind();
                ListItem li = new ListItem("(Select)", "0");
                ddlQCDate.Items.Insert(0, li);
                pnlAdjQC.Visible = false;
                pnlAdjQCDetails.Visible = false;
                divBtnSave.Visible = false;
            }
            else
            {
                btnApprove.Visible = true;
                btnReject.Visible = true;
                /// Funtion to Bind all the fields of AdjustmentQC page
                BindAdjSetupQC(false);
            }

            //Checks Exiting without Save
            CheckWithoutSave();
        }

    }

    private void CheckWithoutSave()
    {
        ArrayList list = new ArrayList();
        list.Add(btnDisplay);
        list.Add(txtQCDate);
        list.Add(CalendarExtender1);
        list.Add(txtComment);
        list.Add(btnSave);
        list.Add(btnApprove);
        list.Add(btnReject);
        ProcessExitFlag(list);
    }

    protected void btnDisplay_click(object sender, EventArgs e)
    {
        /// Funtion to Bind all the fields of AdjustmentQC page
        BindAdjSetupQC(true);
        lblAdjQCDetails.Visible = true;
        btnSave.Enabled = false;
        pnlAdjQC.Visible = true;
        pnlAdjQCDetails.Visible = true;
        divBtnSave.Visible = true;
        pnlAdjQCDetails.Enabled = false;
        divBtnSave.Disabled = true;
    }
    /// <summary>
    /// Funtion to Bind all the fields of AdjustmentQC page
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    private void BindAdjSetupQC(bool history)
    {
        IList<PremiumAdjustmentStatusBE> premList = new List<PremiumAdjustmentStatusBE>();

        premList = AdjQCItemService.getRelatedAdjSetupQCItems(AISMasterEntities.AdjusmentNumber);
        PremAdjStatusBEAdjqcOld = premList[0];
        IList<LookupBE> lookups = (List<LookupBE>)Application["LookUpData"];
        if (premList[0].ADJ_STS_TYP_ID == 348 && ((premList[0].CommentText != "" && premList[0].CommentText != null) || (premList[0].Review_Cmplt_Date != null)))
        {
            history = true;
            lookups = lookups.Where(lk => lk.LookUpName == GlobalConstants.AdjustmentStatus.DraftInvd && lk.LookUpTypeName == "ADJUSTMENT STATUSES").ToList();
        }
        else
        {
            lookups = lookups.Where(lk => lk.LookUpName == GlobalConstants.AdjustmentStatus.QCdDraftInv && lk.LookUpTypeName == "ADJUSTMENT STATUSES").ToList();
            if (!history)
                premList = premList.Where(sts => sts.ADJ_STS_TYP_ID == lookups[0].LookUpID).ToList();
            else
                premList = premList.Where(sts => sts.PremumAdj_sts_ID == Convert.ToInt32(ddlQCDate.SelectedValue)).ToList();
        }
        if (premList.Count > 0 && history)
        {
            if (premList[0].ReviewPerson_ID != null)
            {
                PersonBE persBE = (new PersonBS()).getPersonRow(Convert.ToInt32(premList[0].ReviewPerson_ID));
                if (persBE.PERSON_ID > 0)
                {
                    lblQCBy.Text = persBE.SURNAME + "," + persBE.FORENAME;
                }
            }
            else
            {
                lblQCBy.Text = CurrentAISUser.FullName;
            }

            if (premList[0].CommentText != null)
            {
                txtComment.Text = premList[0].CommentText.ToString();
            }
            else
            {

                txtComment.Text = "";
            }
            if (premList[0].Review_Cmplt_Date.HasValue)
            {
                txtQCDate.Text = premList[0].Review_Cmplt_Date.ToString();
            }

            else
            {
                txtQCDate.Text = "";
            }

            ViewState["PREMADJSTSID"] = premList[0].PremumAdj_sts_ID;

            btnSave.Text = "Update";
            PremAdjStatusBE = premList[0];
            if (PremAdjStatusBE.APPROVEINDICATOR != null)
            {
                btnApprove.Enabled = false;
                btnReject.Enabled = false;
            }
            /// Funtion to Bind the lstAdjustmentQCDetails ListView 
            BindAdjQCDetailsList();

        }
        else
        {

            lblQCBy.Text = CurrentAISUser.FullName;
        }

    }
    /// <summary>
    /// Funtion to Bind the lstAdjustmentQCDetails ListView 
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    public void BindAdjQCDetailsList()
    {

        IList<LookupBE> lookups;
        lookups = (List<LookupBE>)Application["LookUpData"];
        lookups = lookups.Where(lk => lk.LookUpName == "Adjustment QC Issue").ToList();
        if (ViewState["PREMADJSTSID"] != null)
        {
            QltyCntrlListBE = QltyCntrlListService.getQtlychklistList(lookups[0].LookUpID, Convert.ToInt32(ViewState["PREMADJSTSID"]), AISMasterEntities.AccountNumber);
        }
        lstAdjQCDetails.DataSource = QltyCntrlListBE;
        lstAdjQCDetails.DataBind();
        ListItem li = new ListItem("Select", "0");
        if (lstAdjQCDetails.InsertItemPosition != InsertItemPosition.None)
        {
            DropDownList ddl = new DropDownList();
            ddl = (DropDownList)lstAdjQCDetails.InsertItem.FindControl("ddlQCIssue");
            QCMasterIssueListBS issues = new QCMasterIssueListBS();
            ddl.DataSource = issues.getIssuesList(lookups[0].LookUpID);
            ddl.DataValueField = "QualityCntrlMstrIsslstID";
            ddl.DataTextField = "IssueText";
            ddl.DataBind();


            ddl.Items.Insert(0, li);
        }
        lblAdjQCDetails.Visible = true;
        pnlAdjQCDetails.Visible = true;
        divBtnSave.Visible = true;
        if (lstAdjQCDetails.InsertItemPosition != InsertItemPosition.None)
        {
            DropDownList ddlAccountlist = (DropDownList)lstAdjQCDetails.InsertItem.FindControl("ddlAccountlist");
            int actno = AISMasterEntities.AccountNumber;
            IList<AccountBE> acctBE = (new AccountBS()).getMasterWithChildAccounts(actno);
            //ddlAccountlist.DataSource = (new AccountBS()).getMasterWithChildAccounts(AISMasterEntities.AccountNumber);
            ddlAccountlist.DataSource = acctBE;
            ddlAccountlist.DataValueField = "CUSTMR_ID";
            ddlAccountlist.DataTextField = "FULL_NM";
            ddlAccountlist.DataBind();
            ddlAccountlist.Items.Insert(0, li);

            DropDownList ddlProg = (DropDownList)lstAdjQCDetails.InsertItem.FindControl("ddlProgramPeriods");
            ProgramPeriodsBS ProgBS = new ProgramPeriodsBS();
            //Used new method here
            //IList<ProgramPeriodBE> prgBE = ProgBS.GetProgramPeriodSearchDashboard(AISMasterEntities.AccountNumber, 0);
            IList<ProgramPeriodBE> prgBE = ProgBS.GetQCProgramPeriods(AISMasterEntities.AccountNumber, AISMasterEntities.AdjusmentNumber);
            if (prgBE.Count > 0)
                prgBE = prgBE.Where(p => p.CUSTMR_ID == AISMasterEntities.AccountNumber).ToList();
            ddlProg.DataSource = prgBE;
            ddlProg.DataValueField = "PREM_ADJ_PGM_ID";
            ddlProg.DataTextField = "STARTDATE_ENDDATE";
            ddlProg.DataBind();
            ddlProg.Items.Insert(0, li);
        }
        AccountBE acct = (new AccountBS()).getAccount(AISMasterEntities.AccountNumber);
        if (acct.MSTR_ACCT_IND != true && acct.CUSTMR_REL_ID == null)
        {
            if (lstAdjQCDetails.InsertItemPosition != InsertItemPosition.None)
            {

                DropDownList ddlAccountlist = (DropDownList)lstAdjQCDetails.InsertItem.FindControl("ddlAccountlist");
                ddlAccountlist.DataBind();
                ddlAccountlist.SelectedIndex = -1;
                li = new ListItem(AISMasterEntities.AccountName, AISMasterEntities.AccountNumber.ToString());
                ddlAccountlist.Items.Add(li);
                ddlAccountlist.Items.FindByText(AISMasterEntities.AccountName).Selected = true;
                ddlAccountlist.Enabled = false;
            }

        }

    }
    protected void ddlAcctlist_OnSelectedIndexChanged(object sender, EventArgs e)
    {


        DropDownList ddlAccountlist = (DropDownList)lstAdjQCDetails.InsertItem.FindControl("ddlAccountlist");
        if (ddlAccountlist.Items.Count > 0)
        {
            DropDownList ddlProgramPeriods = (DropDownList)lstAdjQCDetails.InsertItem.FindControl("ddlProgramPeriods");
            if (Convert.ToInt32(ddlAccountlist.SelectedValue) > 0)
            {

                ddlProgramPeriods.Enabled = true;
                ProgramPeriodsBS ProgBS = new ProgramPeriodsBS();
                //Used new method here
                ddlProgramPeriods.DataSource = ProgBS.GetQCProgramPeriods(Convert.ToInt32(ddlAccountlist.SelectedValue), AISMasterEntities.AdjusmentNumber);
                ddlProgramPeriods.DataValueField = "PREM_ADJ_PGM_ID";
                ddlProgramPeriods.DataTextField = "STARTDATE_ENDDATE";
                ddlProgramPeriods.DataBind();
                ListItem li = new ListItem("(Select)", "0");
                ddlProgramPeriods.Items.Insert(0, li);
            }
            else
            {
                ddlProgramPeriods.Enabled = false;
            }
        }

    }
    private IList<AccountBE> dtDDLSource;
    /// <summary>
    /// Funtion  for Account Dropdown selected Index changed event
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    protected void FddlAccountlist_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlact = (DropDownList)lstAdjQCDetails.InsertItem.FindControl("ddlAccountlist");
        DropDownList ddlProgramPeriods = (DropDownList)lstAdjQCDetails.InsertItem.FindControl("ddlProgramPeriods");
        if (Convert.ToInt32(ddlact.SelectedItem.Value) > 0)
        {
            ProgramPeriodsBS ProgBS = new ProgramPeriodsBS();
            //Used new method here
            ddlProgramPeriods.DataSource = ProgBS.GetQCProgramPeriods(Convert.ToInt32(ddlact.SelectedValue), AISMasterEntities.AdjusmentNumber);
            ddlProgramPeriods.DataValueField = "PREM_ADJ_PGM_ID";
            ddlProgramPeriods.DataTextField = "STARTDATE_ENDDATE";
            ddlProgramPeriods.DataBind();
            ListItem li = new ListItem("Select", "0");
            ddlProgramPeriods.Items.Insert(0, li);
        }

    }
    /// <summary>
    /// Funtion to save AdjustProcessing Chklist Items into Qlty_Cntrl_List Table
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    void btnSaveCancel_OperationsButtonClicked(object sender, EventArgs e)
    {
    }
    /// <summary>
    /// Function for saving new record into PremiumAdjustmentStatus Table
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (ViewState["PREMADJSTSID"] == null)
        {
            IList<PremiumAdjustmentStatusBE> premList = new List<PremiumAdjustmentStatusBE>();
            premList = AdjQCItemService.getRelatedAdjSetupQCItems(AISMasterEntities.AdjusmentNumber);
            if (premList.Count > 1)
            {
                if (premList[0].ADJ_STS_TYP_ID == 348 && premList[1].ADJ_STS_TYP_ID == 348)
                {
                    ClearError();
                    validtionSummary();
                    modalAdjustmentQC.Show();
                    return;
                }
            }

            PremAdjStatusBE = new PremiumAdjustmentStatusBE();
            PremAdjStatusBE.CreatedDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            PremAdjStatusBE.CreatedUser_ID = CurrentAISUser.PersonID;
        }
        else
        {
            PremAdjStatusBE = AdjQCItemService.getPreAdjStatusRow(Convert.ToInt32(ViewState["PREMADJSTSID"]));
            bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(PremAdjStatusBE.UpdtDate), Convert.ToDateTime(PremAdjStatusBEAdjqcOld.UpdtDate));
            if (!Concurrency)
            {
                ClearError();
                validtionSummary();
                modalAdjustmentQC.Show();
                return;
            }


        }
        PremAdjStatusBE.PremumAdj_ID = AISMasterEntities.AdjusmentNumber;
        PremAdjStatusBE.CustomerID = AISMasterEntities.AccountNumber;
        if (txtQCDate.Text != "")
            PremAdjStatusBE.Review_Cmplt_Date = DateTime.Parse(txtQCDate.Text).Add(DateTime.Now.TimeOfDay);
        PremAdjStatusBE.ReviewPerson_ID = CurrentAISUser.PersonID;
        PremAdjStatusBE.CommentText = txtComment.Text;
        PremAdjStatusBE.EffectiveDate = DateTime.Now;
        PremAdjStatusBE.UpdtDate = DateTime.Now;
        PremAdjStatusBE.UpdtUserID = CurrentAISUser.PersonID;
        if (ViewState["PREMADJSTSID"] != null)
        {
            bool Flag = AdjQCItemService.Update(PremAdjStatusBE);
            //ShowConcurrentConflict(Flag, PremAdjStatusBE.ErrorMessage);
        }
        if (((txtComment.Text != "" && txtComment.Text != null) || (txtQCDate.Text != "" && txtQCDate.Text != null)) && ViewState["PREMADJSTSID"] == null)
        {
            PremAdjStatusBE.ADJ_STS_TYP_ID = 348;
            bool blnSaveFlag = AdjQCItemService.Update(PremAdjStatusBE);
            ViewState["PREMADJSTSID"] = PremAdjStatusBE.PremumAdj_sts_ID;
            if (blnSaveFlag == false)
            {
                ShowError("Unable to save the record");
                return;
            }

        }
        PremAdjStatusBEAdjqcOld = PremAdjStatusBE;
        /// Funtion to Bind the lstAdjustmentQCDetails ListView 
        BindAdjQCDetailsList();

    }
    /// <summary>
    /// Invoked when any  command button is clicked for  ListView 
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    protected void CommandList(Object sender, ListViewCommandEventArgs e)
    {

        if (e.CommandName.ToUpper() == "SAVE")
        {

            AdjSetupQCDetailsSaveList(e.Item);

        }

        else if (e.CommandName.ToUpper() == "DISABLE" || e.CommandName.ToUpper() == "ENABLE")
        {
            //Function call to Enable or Disable the Record
            DisableRow(e, e.CommandName == "DISABLE" ? false : true);
        }
    }
    /// <summary>
    /// Funtion to Disable one rows of lstAdjustmentQCDetails ListView 
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    protected void DisableRow(ListViewCommandEventArgs e, bool Flag)
    {
        try
        {
            if (ViewState["PREMADJSTSID"] != null)
            {
                Qtly_Cntrl_ChklistBE newQltyCntrlListBE = QltyCntrlListService.getQualityControlRow(Convert.ToInt32(e.CommandArgument));
                newQltyCntrlListBE.ACTIVE = Flag;
                Flag = QltyCntrlListService.Update(newQltyCntrlListBE);
            }
            else
            {
                ListViewDataItem item = (ListViewDataItem)e.Item;
                QltyCntrlListBE[item.DisplayIndex].ACTIVE = Flag;
            }
            //Function call to bind Details
            BindAdjQCDetailsList();
        }
        catch (RetroBaseException ee)
        {
            ShowError(ee.Message, ee);
        }
    }
    protected void InsertList(Object sender, ListViewInsertEventArgs e)
    {
        //lstLookupType.InsertItemPosition = InsertItemPosition.None;
    }
    /// <summary>
    /// Funtion for databound event  of lstAdjustmentQCDetails listview
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    protected void DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDisable");
            if (imgDelete != null)
            {
                HiddenField hid = (HiddenField)e.Item.FindControl("hidActive");
                imgDelete.CommandName = hid.Value == "True" ? "DISABLE" : "ENABLE";
                imgDelete.Attributes.Add("onclick", hid.Value == "True" ? "return confirm('Are you sure you want to Disable?');" : "return confirm('Are you sure you want to Enable?');");
            }
        }
    }
    /// <summary>
    /// Funtion for inserting new AdjustmentQC issues records into Qlty_Cntrl_List Table
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    protected void AdjSetupQCDetailsSaveList(ListViewItem e)
    {
        ////////////////////////////////////////
        Qtly_Cntrl_ChklistBE newQltyCntrlListBE = new Qtly_Cntrl_ChklistBE();
        newQltyCntrlListBE.CHECKLISTITEM_ID = Convert.ToInt32(((DropDownList)e.FindControl("ddlQCIssue")).SelectedItem.Value);
        newQltyCntrlListBE.CUSTOMER_ID = AISMasterEntities.AccountNumber;
        newQltyCntrlListBE.PREMIUMADJUSTMENT_ID = AISMasterEntities.AdjusmentNumber;
        if (Convert.ToInt32(((DropDownList)e.FindControl("ddlProgramPeriods")).SelectedItem.Value) > 0)
        {
            newQltyCntrlListBE.PremumAdj_Pgm_ID = Convert.ToInt32(((DropDownList)e.FindControl("ddlProgramPeriods")).SelectedItem.Value);
        }
        newQltyCntrlListBE.ACTIVE = true;
        newQltyCntrlListBE.CreatedUser_ID = CurrentAISUser.PersonID;
        newQltyCntrlListBE.CreatedDate = DateTime.Now;

        DropDownList ddlAccountlist = (DropDownList)e.FindControl("ddlAccountlist");
        if (ddlAccountlist.SelectedItem != null && (Convert.ToInt32(ddlAccountlist.SelectedItem.Value) > 0))
        {
            newQltyCntrlListBE.CUST_RELID = Convert.ToInt32(ddlAccountlist.SelectedItem.Value);
            newQltyCntrlListBE.Reg_AccountName = ddlAccountlist.SelectedItem.Text;
        }
        if (ViewState["PREMADJSTSID"] != null)
        {
            newQltyCntrlListBE.PREMIUMADJ_STATUS_ID = Convert.ToInt32(ViewState["PREMADJSTSID"].ToString());
            bool Result = QltyCntrlListService.IsExistsIssueQCDetails(newQltyCntrlListBE.PREMIUMADJ_STATUS_ID, newQltyCntrlListBE.PremumAdj_Pgm_ID, newQltyCntrlListBE.CHECKLISTITEM_ID, newQltyCntrlListBE.CUSTOMER_ID, newQltyCntrlListBE.QualityControlChklst_ID);
            if (!Result)
            {
                bool Flag = QltyCntrlListService.Update(newQltyCntrlListBE);
                //Function call to bind Details
                BindAdjQCDetailsList();
                return;
            }
            else
            {
                ShowMessage("The record cannot be saved. An identical record already exists.");
                return;
            }
        }
        else if (QltyCntrlListBE.Where(chk => chk.CHECKLISTITEM_ID == Convert.ToInt32(((DropDownList)e.FindControl("ddlQCIssue")).SelectedItem.Value) && chk.PremumAdj_Pgm_ID == Convert.ToInt32(((DropDownList)e.FindControl("ddlProgramPeriods")).SelectedItem.Value)).Count() > 0)
        {
            ShowMessage("The record cannot be saved. An identical record already exists.");
            return;
        }
        string[] str = ((DropDownList)e.FindControl("ddlProgramPeriods")).SelectedItem.Text.Split('-');
        newQltyCntrlListBE.ProgramPeriodStDate = DateTime.Parse(str[0].ToString());
        newQltyCntrlListBE.ProgramPeriodEndDate = DateTime.Parse(str[1].ToString());
        newQltyCntrlListBE.CHKLISTNAME = ((DropDownList)e.FindControl("ddlQCIssue")).SelectedItem.Text;
        newQltyCntrlListBE.PREMIUMADJ_STATUS_ID = -(QltyCntrlListBE.Count + 1);
        QltyCntrlListBE.Add(newQltyCntrlListBE);
        //Function call to bind lstReviewDetails
        BindAdjQCDetailsList();
    }
    /// <summary>
    /// Funtion for Approval
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    protected void btnApprove_Click(object sender, EventArgs e)
    {

        if (ViewState["PREMADJSTSID"] != null)
        {
            IList<PremiumAdjustmentStatusBE> premList = new List<PremiumAdjustmentStatusBE>();
            premList = AdjQCItemService.getRelatedAdjSetupQCItems(AISMasterEntities.AdjusmentNumber);
            if (premList.Count > 1)
            {
                if (premList[0].ADJ_STS_TYP_ID == 350 || premList[0].ADJ_STS_TYP_ID == 346)
                {
                    ClearError();
                    validtionSummary();
                    modalAdjustmentQC.Show();
                    return;
                }
            }
            PremAdjStatusBE = AdjQCItemService.getPreAdjStatusRow(Convert.ToInt32(ViewState["PREMADJSTSID"]));
            bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(PremAdjStatusBEAdjqcOld.UpdtDate), Convert.ToDateTime(PremAdjStatusBE.UpdtDate));
            if (!Concurrency)
            {
                ClearError();
                validtionSummary();
                modalAdjustmentQC.Show();
                return;
            }
        }

        PremAdjStatusBE.PremumAdj_ID = AISMasterEntities.AdjusmentNumber;
        PremAdjStatusBE.CustomerID = AISMasterEntities.AccountNumber;
        PremAdjStatusBE.APPROVEINDICATOR = true;
        PremAdjStatusBE.UpdtUserID = CurrentAISUser.PersonID;
        PremAdjStatusBE.UpdtDate = DateTime.Now;
        if (txtQCDate.Text != "")
        {
            PremAdjStatusBE.Review_Cmplt_Date = DateTime.Parse(txtQCDate.Text).Add(DateTime.Now.TimeOfDay);
        }
        PremAdjStatusBE.ReviewPerson_ID = CurrentAISUser.PersonID;
        IList<LookupBE> lookups = (List<LookupBE>)Application["LookUpData"];
        lookups = lookups.Where(lk => lk.LookUpName == GlobalConstants.AdjustmentStatus.QCdDraftInv && lk.LookUpTypeName == "ADJUSTMENT STATUSES").ToList();
        PremAdjStatusBE.ADJ_STS_TYP_ID = lookups[0].LookUpID;
        PremAdjStatusBE.EffectiveDate = DateTime.Now;
        bool Flag = AdjQCItemService.Update(PremAdjStatusBE);
        // ShowConcurrentConflict(Flag, PremAdjStatusBE.ErrorMessage);
        if (Flag)
        {
            //for Issues
            for (int i = 0; i < QltyCntrlListBE.Count; i++)
            {
                if (QltyCntrlListBE[i].PREMIUMADJ_STATUS_ID <= 0)
                    QltyCntrlListBE[i].PREMIUMADJ_STATUS_ID = PremAdjStatusBE.PremumAdj_sts_ID;
                Flag = QltyCntrlListService.Update(QltyCntrlListBE[i]);
                //ShowConcurrentConflict(Flag, QltyCntrlListBE[i].ErrorMessage);
            }

            IList<PremiumAdjustmentStatusBE> premAdjSts = AdjQCItemService.getPreAdjStatusList(AISMasterEntities.AdjusmentNumber);
            lookups = (List<LookupBE>)Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpName == GlobalConstants.AdjustmentStatus.DraftInvd && lk.LookUpTypeName == "ADJUSTMENT STATUSES").ToList();
            premAdjSts = premAdjSts.Where(sts => sts.ADJ_STS_TYP_ID == lookups[0].LookUpID).ToList();
            if (premAdjSts.Count > 0)
            {
                PremAdjStatusBE = adjQCItemService.getPreAdjStatusRow(premAdjSts[0].PremumAdj_sts_ID);
                PremAdjStatusBE.EXPIRYDATE = DateTime.Now;
                PremAdjStatusBE.UpdtDate = DateTime.Now;
                PremAdjStatusBE.UpdtUserID = CurrentAISUser.PersonID;
                Flag = AdjQCItemService.Update(PremAdjStatusBE);
            }
            PremiumAdjustmentBE premAdjBE = (new PremAdjustmentBS()).getPremiumAdjustmentRow(PremAdjStatusBE.PremumAdj_ID);
            premAdjBE.ADJ_STS_TYP_ID = 350;//QC_DRAFT
            premAdjBE.ADJ_STS_EFF_DT = DateTime.Now;
            premAdjBE.ADJ_QC_IND = true;
            premAdjBE.UPDT_DT = DateTime.Now;
            premAdjBE.UPDT_USER_ID = CurrentAISUser.PersonID;
            Flag = (new PremAdjustmentBS()).Update(premAdjBE);
            // ShowConcurrentConflict(Flag, premAdjBE.ErrorMessage);
        }
        System.Threading.Thread.Sleep(10000);

        IList<PremiumAdjustmentStatusBE> premUpdateList = new List<PremiumAdjustmentStatusBE>();
        premUpdateList = AdjQCItemService.getRelatedAdjSetupQCItems(AISMasterEntities.AdjusmentNumber);
        if (premUpdateList.Count > 1)
        {
            if (premUpdateList[0].ADJ_STS_TYP_ID == 350 && premUpdateList[0].UpdtUserID == CurrentAISUser.PersonID)
            {
                btnApprove.Enabled = false;
                btnReject.Enabled = false;
                /// Funtion to Bind all the fields of AdjustmentQC page
                BindAdjSetupQC(false);
                pnlAdjQC.Enabled = false;
                pnlAdjQCDetails.Enabled = false;
                //Code for Email Notification

                EMailNotification(PremAdjStatusBE.PremumAdj_ID, true);
            }
            else
            {
                ClearError();
                validtionSummary();
                modalAdjustmentQC.Show();
                return;
            }
        }

    }
    /// <summary>
    /// Funtion for Reject
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    protected void btnReject_Click(object sender, EventArgs e)
    {
        PremiumAdjustmentBE PremAdjBE = (new PremAdjustmentBS()).getPremiumAdjustmentRow(AISMasterEntities.AdjusmentNumber);
        PremAdjBE.CALC_ADJ_STS_CODE = "";
        bool Flag = (new PremAdjustmentBS()).Update(PremAdjBE);
        ShowConcurrentConflict(Flag, PremAdjBE.ErrorMessage);
        if (ViewState["PREMADJSTSID"] != null)
        {
            IList<PremiumAdjustmentStatusBE> premList = new List<PremiumAdjustmentStatusBE>();
            premList = AdjQCItemService.getRelatedAdjSetupQCItems(AISMasterEntities.AdjusmentNumber);
            if (premList.Count > 1)
            {
                if (premList[0].ADJ_STS_TYP_ID == 346 || premList[0].ADJ_STS_TYP_ID == 350)
                {
                    ClearError();
                    validtionSummary();
                    modalAdjustmentQC.Show();
                    return;
                }
            }
            PremAdjStatusBE = AdjQCItemService.getPreAdjStatusRow(Convert.ToInt32(ViewState["PREMADJSTSID"]));
            bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(PremAdjStatusBEAdjqcOld.UpdtDate), Convert.ToDateTime(PremAdjStatusBE.UpdtDate));
            if (!Concurrency)
            {
                ClearError();
                validtionSummary();
                modalAdjustmentQC.Show();
                return;
            }
        }
        PremAdjStatusBE.PremumAdj_ID = AISMasterEntities.AdjusmentNumber;
        PremAdjStatusBE.CustomerID = AISMasterEntities.AccountNumber;
        PremAdjStatusBE.ReviewPerson_ID = CurrentAISUser.PersonID;
        PremAdjStatusBE.UpdtDate = DateTime.Now;
        PremAdjStatusBE.UpdtUserID = CurrentAISUser.PersonID;
        if (txtQCDate.Text != "")
        {
            PremAdjStatusBE.Review_Cmplt_Date = DateTime.Parse(txtQCDate.Text).Add(DateTime.Now.TimeOfDay);
        }
        IList<LookupBE> lookups = (List<LookupBE>)Application["LookUpData"];
        lookups = lookups.Where(lk => lk.LookUpName == GlobalConstants.AdjustmentStatus.Calc && lk.LookUpTypeName == "ADJUSTMENT STATUSES").ToList();
        PremAdjStatusBE.ADJ_STS_TYP_ID = lookups[0].LookUpID;
        PremAdjStatusBE.EffectiveDate = DateTime.Now;
        PremAdjStatusBE.APPROVEINDICATOR = false;
        Flag = AdjQCItemService.Update(PremAdjStatusBE);
        //ShowConcurrentConflict(Flag, PremAdjStatusBE.ErrorMessage);
        if (Flag)
        {
            //for Issues
            for (int i = 0; i < QltyCntrlListBE.Count; i++)
            {
                if (QltyCntrlListBE[i].PREMIUMADJ_STATUS_ID <= 0)
                    QltyCntrlListBE[i].PREMIUMADJ_STATUS_ID = PremAdjStatusBE.PremumAdj_sts_ID;
                Flag = QltyCntrlListService.Update(QltyCntrlListBE[i]);
                //ShowConcurrentConflict(Flag, QltyCntrlListBE[i].ErrorMessage);
            }

            IList<PremiumAdjustmentStatusBE> premAdjSts = AdjQCItemService.getPreAdjStatusList(AISMasterEntities.AdjusmentNumber);
            lookups = (List<LookupBE>)Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpName == GlobalConstants.AdjustmentStatus.DraftInvd && lk.LookUpTypeName == "ADJUSTMENT STATUSES").ToList();
            premAdjSts = premAdjSts.Where(sts => sts.ADJ_STS_TYP_ID == lookups[0].LookUpID).ToList();
            if (premAdjSts.Count > 0)
            {
                PremAdjStatusBE = adjQCItemService.getPreAdjStatusRow(premAdjSts[0].PremumAdj_sts_ID);
                PremAdjStatusBE.EXPIRYDATE = DateTime.Now;
                PremAdjStatusBE.UpdtDate = DateTime.Now;
                PremAdjStatusBE.UpdtUserID = CurrentAISUser.PersonID;
                Flag = AdjQCItemService.Update(PremAdjStatusBE);
            }
            PremiumAdjustmentBE premAdjBE = (new PremAdjustmentBS()).getPremiumAdjustmentRow(PremAdjStatusBE.PremumAdj_ID);
            premAdjBE.ADJ_STS_TYP_ID = 346;//CALC
            premAdjBE.ADJ_STS_EFF_DT = DateTime.Now;
            premAdjBE.ADJ_QC_IND = true;
            premAdjBE.UPDT_DT = DateTime.Now;
            premAdjBE.UPDT_USER_ID = CurrentAISUser.PersonID;
            Flag = (new PremAdjustmentBS()).Update(premAdjBE);
            //ShowConcurrentConflict(Flag, premAdjBE.ErrorMessage);
        }
        System.Threading.Thread.Sleep(10000);

        IList<PremiumAdjustmentStatusBE> premUpdateList = new List<PremiumAdjustmentStatusBE>();
        premUpdateList = AdjQCItemService.getRelatedAdjSetupQCItems(AISMasterEntities.AdjusmentNumber);
        if (premUpdateList.Count > 1)
        {
            if (premUpdateList[0].ADJ_STS_TYP_ID == 346 && premUpdateList[0].UpdtUserID == CurrentAISUser.PersonID)
            {
                btnReject.Enabled = false;
                btnApprove.Enabled = false;
                /// Funtion to Bind all the fields of AdjustmentQC page
                BindAdjSetupQC(false);
                pnlAdjQC.Enabled = false;
                pnlAdjQCDetails.Enabled = false;
                //Code for Email Notification
                EMailNotification(PremAdjStatusBE.PremumAdj_ID, false);
            }
            else
            {
                ClearError();
                validtionSummary();
                modalAdjustmentQC.Show();
                return;
            
            }
        }
       


    }
    /// <summary>
    /// Method to Send the EMail
    /// </summary>
    /// <param name="intAdjNo"></param>
    /// <param name="iFlag"></param>
    private void EMailNotification(int intAdjNo, bool Flag)
    {
        try
        {
            SMTPMailer dd = new SMTPMailer();
            System.Net.Mail.MailAddressCollection objMailList = new System.Net.Mail.MailAddressCollection();
            dd.lstTomailAddress = objMailList;
            IList<PremiumAdjustmentBE> EmailInfo = new PremAdjustmentBS().GetEMailInfo(intAdjNo);
            IList<string> strEmailList = new PremAdjustmentBS().getEmailIDS(ZurichNA.AIS.DAL.Logic.GlobalConstants.RESPONSIBILITIES.ADJQC, Flag, EmailInfo[0].CUSTOMERID);
            for (int i = 0; i < strEmailList.Count; i++)
            {
                objMailList.Add(strEmailList[i].ToString());
            }
            string strSubject = "Adjustment QC " + "{" + EmailInfo[0].DRAFTINVNO + "}" + " " + "for " + " " + EmailInfo[0].CUSTMR_NAME + " " + " for " + "- " + EmailInfo[0].BUNAME;
            string strBody = "Adjustment QC " + "{" + EmailInfo[0].DRAFTINVNO + "}" + " ";
            if (Flag)
                strBody += " has been approved for the following" + "\r\n";
            else
                strBody += " has been Rejected for the following" + "\r\n";
            strBody = strBody + "\r\n";
            strBody = strBody + "Insured Name:" + EmailInfo[0].CUSTMR_NAME + "\r\n";
            strBody = strBody + "Broker Name:" + EmailInfo[0].BROKERNAME + "\r\n";
            strBody = strBody + "Valuation Date:" + EmailInfo[0].VALUATIONDATE + "\r\n";
            strBody = strBody + "Invoice Number:" + EmailInfo[0].DRAFTINVNO + "\r\n";
            strBody = strBody + "BU Name:" + EmailInfo[0].BUNAME + "\r\n";
            strBody = strBody + "\r\n";
            strBody = strBody + "\r\n";
            if (Flag)
                strBody = strBody + "Please review adjustment for Recon Review process." + "\r\n";
            else
                strBody = strBody + "Please check for the reasons." + "\r\n";
            dd.Subject = strSubject;
            dd.Body = strBody;
            dd.SendMail();
        }
        catch (Exception ee)
        {
            if (Flag)
            {
                ShowError("Adjustment QC Approved. Email Notification Failed.");
                return;
            }
            else
            {
                ShowError("Adjustment QC Rejected. Email Notification Failed.");
                return;
            }
        }
        if (Flag)
        {
            ShowError("Adjustment QC Approved Successfully.");
            return;
        }
        else
        {
            ShowError("Adjustment QC Rejected Successfully.");
            return;
        }
    }

    /// <summary>
    /// This function is used to clear the border rows obtained because of validation summary
    /// </summary>
    public void validtionSummary()
    {
        CustomValidator validator = (CustomValidator)Master.FindControl("ErrorPlaceHolder").FindControl("AISErrorValidator");
        ValidationSummary summary = (ValidationSummary)Master.FindControl("ErrorPlaceHolder").FindControl("AISSummary");
        if (validator.ErrorMessage == "")
        {
            validator.BorderColor = System.Drawing.Color.White;
            summary.BorderColor = System.Drawing.Color.White;
        }
        else
        {
            validator.BorderColor = System.Drawing.Color.Red;
            summary.BorderColor = System.Drawing.Color.Red;

        }
    }
}

