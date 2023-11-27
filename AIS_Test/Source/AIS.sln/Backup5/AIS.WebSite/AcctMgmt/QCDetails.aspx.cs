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

public partial class AcctSetup_QCDetails : AISBasePage
{
    private PremiumAdjustmentStatusBS prem_Adj_StsService;
    private Qtly_Cntrl_ChklistBS qlty_Cntrl_ListService;
    /// <summary>
    /// property to hold an instance for Business Transaction Wrapper
    /// </summary>
    /// <param name=""></param>
    /// <returns>AISBusinessTransaction property</returns>
    protected AISBusinessTransaction ReconReviewTransactionWrapper
    {
        get
        {
            //if ((AISBusinessTransaction)Session["ReconReviewTransaction"] == null)
            //    Session["ReconReviewTransaction"] = new AISBusinessTransaction();
            //return (AISBusinessTransaction)Session["ReconReviewTransaction"];
            if ((AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("ReconReviewTransaction") == null)
                SaveObjectToSessionUsingWindowName("ReconReviewTransaction", new AISBusinessTransaction());
            return (AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("ReconReviewTransaction");
        }
        set
        {
            //Session["PersonTransaction"] = value;
            SaveObjectToSessionUsingWindowName("ReconReviewTransaction", value);
        }
    }


    /// <summary>
    /// a property for Premium Adjustment Status Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>PremiumAdjustmentStatusBS</returns>
    private PremiumAdjustmentStatusBS PremAdjStsService
    {
        get
        {
            if (prem_Adj_StsService == null)
            {
                prem_Adj_StsService = new PremiumAdjustmentStatusBS();
                //prem_Adj_StsService.AppTransactionWrapper = ReconReviewTransactionWrapper;
            }
            return prem_Adj_StsService;
        }
    }
    /// <summary>
    /// a property for Quality Control CheckList Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>Qtly_Cntrl_ChklistBS</returns>
    private Qtly_Cntrl_ChklistBS QltyCntrlListService
    {
        get
        {
            if (qlty_Cntrl_ListService == null)
            {
                qlty_Cntrl_ListService = new Qtly_Cntrl_ChklistBS();
                //qlty_Cntrl_ListService.AppTransactionWrapper = ReconReviewTransactionWrapper;
            }
            return qlty_Cntrl_ListService;
        }
    }
    /// <summary>
    /// a property for Premium Adjustment Status Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>PremiumAdjustmentStatusBE</returns>
    private PremiumAdjustmentStatusBE premAdjStsBE
    {
        get 
        { 
            //return (PremiumAdjustmentStatusBE)Session["PREMADJSTSBE"]; 
            return (PremiumAdjustmentStatusBE)RetrieveObjectFromSessionUsingWindowName("PREMADJSTSBE"); 
        }
        set 
        { 
            //Session["PREMADJSTSBE"] = value; 
            SaveObjectToSessionUsingWindowName("PREMADJSTSBE", value); 
        }
    }

    /// <summary>
    /// a property for Quality Control CheckList Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>Qtly_Cntrl_ChklistBE</returns>
    private Qtly_Cntrl_ChklistBE QltyCntrlListBE
    {
        get 
        { 
            //return (Qtly_Cntrl_ChklistBE)Session["QLTYCNTRLLISTBE"]; 
            return (Qtly_Cntrl_ChklistBE)RetrieveObjectFromSessionUsingWindowName("QLTYCNTRLLISTBE");
        }
        set 
        { 
            //Session["QLTYCNTRLLISTBE"] = value; 
            SaveObjectToSessionUsingWindowName("QLTYCNTRLLISTBE", value); 
        }
    }
    private PremiumAdjustmentStatusBE PremAdjStatusBE20QCOld
    {
        get
        {
            //return ((Session["PremAdjStatusBE20QCOld"] == null) ?
            //    (new PremiumAdjustmentStatusBE()) : (PremiumAdjustmentStatusBE)Session["PremAdjStatusBE20QCOld"]);
            return ((RetrieveObjectFromSessionUsingWindowName("PremAdjStatusBE20QCOld") == null) ?
               (new PremiumAdjustmentStatusBE()) : (PremiumAdjustmentStatusBE)RetrieveObjectFromSessionUsingWindowName("PremAdjStatusBE20QCOld"));
        }

        set 
        { 
            //Session["PremAdjStatusBE20QCOld"] = value; 
            SaveObjectToSessionUsingWindowName("PremAdjStatusBE20QCOld", value); 
        }
    }
    private PremiumAdjustmentBE PremAdjBEConCurrent
    {
        get
        {
            //return ((Session["PremAdjBEConCurrent"] == null) ?
            //    (new PremiumAdjustmentBE()) : (PremiumAdjustmentBE)Session["PremAdjBEConCurrent"]);
            return ((RetrieveObjectFromSessionUsingWindowName("PremAdjBEConCurrent") == null) ?
                (new PremiumAdjustmentBE()) : (PremiumAdjustmentBE)RetrieveObjectFromSessionUsingWindowName("PremAdjBEConCurrent"));
        }

        set 
        { 
            //Session["PremAdjBEConCurrent"] = value; 
            SaveObjectToSessionUsingWindowName("PremAdjBEConCurrent", value); 
        }
    }
    
    
    /// <summary>
    /// a property for Postal Address Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>PostalAddressBE</returns>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.Page.Title = "20% Adjustment QC";
        if (!IsPostBack)
        {
            BindList();
            premAdjStsBE = new PremiumAdjustmentStatusBE();
            QltyCntrlListBE = new Qtly_Cntrl_ChklistBE();
            //ReconReviewTransactionWrapper = new AISBusinessTransaction();
        }

        //Checks Exiting without Save
        ArrayList list = new ArrayList();
        list.Add(txtComments);
        list.Add(txtQCDate);
        list.Add(calQCDate);
        list.Add(btnQCComplete);
        list.Add(btnSave);
        ProcessExitFlag(list);
    }
    /// <summary>
    /// Funtion to Bind the lstQCView ListView 
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    public void BindList()
    {
        IList<PremiumAdjustmentStatusBE> premAdjSts = PremAdjStsService.getPreAdjStatusList(AISMasterEntities.AdjusmentNumber);
        if (premAdjSts.Count > 0)
        {
            PremAdjStatusBE20QCOld = premAdjSts[0];
            if (premAdjSts[0].ReviewPerson_ID != null)
            {
                PersonBE persBE = (new PersonBS()).getPersonRow(Convert.ToInt32(premAdjSts[0].ReviewPerson_ID));
                if (persBE.PERSON_ID > 0)
                {   
                    //06/23 for veracode
                    //lblQCby.Text = persBE.SURNAME + "," + persBE.FORENAME;
                    lblQCby.Text = Server.HtmlDecode(Server.HtmlEncode(persBE.SURNAME)) + "," + Server.HtmlDecode(Server.HtmlEncode(persBE.FORENAME));
                }
            }
            else
            {   
                //06/23 for veracode
                //lblQCby.Text = CurrentAISUser.FullName;
                lblQCby.Text = string.IsNullOrEmpty(CurrentAISUser.FullName.Trim()) ? "" : HttpUtility.HtmlDecode(HttpUtility.HtmlEncode(CurrentAISUser.FullName));
            }
            txtComments.Text = premAdjSts[0].CommentText;
            if (premAdjSts[0].Review_Cmplt_Date.HasValue)
            {
                txtQCDate.Text = premAdjSts[0].Review_Cmplt_Date.Value.ToShortDateString();
            }
            else
            {
                txtQCDate.Text = "";
            }
            ViewState["PREMADJSTSID"] = premAdjSts[0].PremumAdj_sts_ID;
            ViewState["PREMADJID"] = premAdjSts[0].PremumAdj_ID;
            /// Funtion to Bind the lstQCDetails ListView 
            BindDetailsList();
            PremAdjustmentBS PremAdjBS = new PremAdjustmentBS();
            PremiumAdjustmentBE PremAdjBE = PremAdjBS.getPremiumAdjustmentRow(Convert.ToInt32(ViewState["PREMADJID"]));
            if (PremAdjBE.TWENTY_PCT_QLTY_CNTRL_IND == true)
            {
                btnQCComplete.Enabled = false;
            }

        }
        else
        {   
            //06/23 for veracode
            //lblQCby.Text = CurrentAISUser.FullName;
            lblQCby.Text = string.IsNullOrEmpty(CurrentAISUser.FullName.Trim()) ? "" : HttpUtility.HtmlDecode(HttpUtility.HtmlEncode(CurrentAISUser.FullName));
        }
    }
    /// <summary>
    /// Funtion to Bind the lstQCDetails ListView 
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    public void BindDetailsList()
    {
        PremAdjBEConCurrent = (new PremAdjustmentBS()).getPremiumAdjustmentRow(Convert.ToInt32(ViewState["PREMADJID"]));
        IList<LookupBE> lookups;
        lookups = (List<LookupBE>)Application["LookUpData"];
        lookups = lookups.Where(lk => lk.LookUpName == "Twenty Percent QC Issue").ToList();
        lstQCDetails.DataSource = QltyCntrlListService.getQtlychklistList(lookups[0].LookUpID, Convert.ToInt32(ViewState["PREMADJSTSID"]), AISMasterEntities.AccountNumber);
        lstQCDetails.DataBind();
        ListItem li = new ListItem("Select", "0");
        if (lstQCDetails.InsertItemPosition != InsertItemPosition.None)
        {
            DropDownList ddl = (DropDownList)lstQCDetails.InsertItem.FindControl("ddlIssue");
            QCMasterIssueListBS issues = new QCMasterIssueListBS();
            ddl.DataSource = issues.getIssuesList(lookups[0].LookUpID);
            ddl.DataValueField = "QualityCntrlMstrIsslstID";
            ddl.DataTextField = "IssueText";
            ddl.DataBind();


            ddl.Items.Insert(0, li);
        }
        lblAdjQCDetails.Visible = true;
        pnlQCDetails.Visible = true;
        divQCComplete.Visible = true;
        AccountBE acct = (new AccountBS()).getAccount(AISMasterEntities.AccountNumber);
        if (acct.MSTR_ACCT_IND == true)
        {
            //this is the case of MasterAccount
            if (lstQCDetails.InsertItemPosition != InsertItemPosition.None)
            {
                DropDownList ddlAccountlist = (DropDownList)lstQCDetails.InsertItem.FindControl("ddlAccountlist");
                int actno = AISMasterEntities.AccountNumber;
                IList<AccountBE> acctBE = (new AccountBS()).getMasterWithChildAccounts(actno);
                //ddlAccountlist.DataSource = (new AccountBS()).getMasterWithChildAccounts(AISMasterEntities.AccountNumber);
                ddlAccountlist.DataSource = acctBE;
                ddlAccountlist.DataValueField = "CUSTMR_ID";
                ddlAccountlist.DataTextField = "FULL_NM";
                ddlAccountlist.DataBind();
                ddlAccountlist.Items.Insert(0, li);
            }
        }
        else
        {
            //this is the case of regular account
            if (lstQCDetails.InsertItemPosition != InsertItemPosition.None)
            {
                DropDownList ddlAccountlist = (DropDownList)lstQCDetails.InsertItem.FindControl("ddlAccountlist");
                ListItem item = new ListItem(AISMasterEntities.AccountName, AISMasterEntities.AccountNumber.ToString());
                ddlAccountlist.Items.Add(item);
                ddlAccountlist.Items.Insert(0, li);
            }
        }

    }

    protected void ddlAcctlist_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        DropDownList ddlAccountlist = (DropDownList)lstQCDetails.InsertItem.FindControl("ddlAccountlist");
        if (ddlAccountlist.Items.Count > 0)
        {
            DropDownList ddlProg = (DropDownList)lstQCDetails.InsertItem.FindControl("ddlProgramPeriods");
            if (Convert.ToInt32(ddlAccountlist.SelectedValue) > 0)
            {
                ddlProg.Enabled = true;
                ProgramPeriodsBS ProgBS = new ProgramPeriodsBS();
                //11288 Bug Fix-modified the method to retrieve the program periods related to the custmer and adjustments
                //ddlProg.DataSource = ProgBS.GetProgramPeriodSearchDashboard(Convert.ToInt32(ddlAccountlist.SelectedValue), 0);
                ddlProg.DataSource = ProgBS.GetQCProgramPeriods(Convert.ToInt32(ddlAccountlist.SelectedValue), AISMasterEntities.AdjusmentNumber);
                ddlProg.DataValueField = "PREM_ADJ_PGM_ID";
                ddlProg.DataTextField = "STARTDATE_ENDDATE";
                ddlProg.DataBind();
                ListItem li = new ListItem("(Select)", "0");
                ddlProg.Items.Insert(0, li);
            }
            else
            {
                ddlProg.Enabled = false;
            }
        }

    }
    private IList<AccountBE> dtDDLSource;
    protected void ddlAccountlist_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlact = (DropDownList)lstQCDetails.InsertItem.FindControl("ddlAccountlist");
        DropDownList ddlProgramPeriods = (DropDownList)lstQCDetails.InsertItem.FindControl("ddlProgramPeriods");
        if (Convert.ToInt32(ddlact.SelectedItem.Value) > 0)
        {
            ProgramPeriodsBS ProgBS = new ProgramPeriodsBS();
            //11288 Bug Fix-modified the method to retrieve the program periods related to the custmer and adjustments
            //ddlProgramPeriods.DataSource = ProgBS.GetProgramPeriodSearchDashboard(Convert.ToInt32(ddlact.SelectedValue), 0);
            ddlProgramPeriods.DataSource = ProgBS.GetQCProgramPeriods(Convert.ToInt32(ddlact.SelectedValue), AISMasterEntities.AdjusmentNumber);
            ddlProgramPeriods.DataValueField = "PREM_ADJ_PGM_ID";
            ddlProgramPeriods.DataTextField = "STARTDATE_ENDDATE";
            ddlProgramPeriods.DataBind();
            ListItem li = new ListItem("Select", "0");
            ddlProgramPeriods.Items.Insert(0, li);
        }
    }
    /// <summary>
    /// INvoked when Command buttons Fired
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CommandList(Object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SAVE")
        {
            //Function call to save the 20 % QC Issue
            SaveQualityList(e.Item);
            //Function to update 20% QCby
            saveqcdetails();
        }
        else if (e.CommandName.ToUpper() == "DISABLE" || e.CommandName.ToUpper() == "ENABLE")
        {
            //Function call to Enable or Disable the Record
            DisableRow(Convert.ToInt32(e.CommandArgument), e.CommandName == "DISABLE" ? false : true);
            saveqcdetails();
        }
    }
    /// <summary>
    /// function for Person record to make enable or disable
    /// </summary>
    /// <param name="personID"></param>
    /// <param name="Flag">True/False boolean value</param>
    /// <returns></returns>
    protected void DisableRow(int intQltyID, bool Flag)
    {
        try
        {
            QltyCntrlListBE = QltyCntrlListService.getQualityControlRow(intQltyID);
            QltyCntrlListBE.ACTIVE = Flag;
            Flag = QltyCntrlListService.Update(QltyCntrlListBE);
            ShowConcurrentConflict(Flag, QltyCntrlListBE.ErrorMessage);
            if (Flag)
            {
                //Function call to bind lstReviewDetails
                BindDetailsList();
            }

        }
        catch (RetroBaseException ee)
        {
            ShowError(ee.Message, ee);
        }
    }
    /// <summary>
    /// Invoked when the Save Link is clicked In lstQCViewDetails ListView
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SaveQualityList(ListViewItem e)
    {
        QltyCntrlListBE = new Qtly_Cntrl_ChklistBE();
        //QltyCntrlListBE.PremumAdj_Pgm_ID = AISMasterEntities.AdjusmentNumber;
        QltyCntrlListBE.CHECKLISTITEM_ID = Convert.ToInt32(((DropDownList)e.FindControl("ddlIssue")).SelectedItem.Value);
        QltyCntrlListBE.PREMIUMADJ_STATUS_ID = Convert.ToInt32(ViewState["PREMADJSTSID"].ToString());
        QltyCntrlListBE.CUST_RELID = Convert.ToInt32(((DropDownList)e.FindControl("ddlAccountlist")).SelectedItem.Value);
        QltyCntrlListBE.PREMIUMADJUSTMENT_ID = AISMasterEntities.AdjusmentNumber;
        QltyCntrlListBE.ACTIVE = true;
        QltyCntrlListBE.CUSTOMER_ID = AISMasterEntities.AccountNumber;
        DropDownList ddlAccountlist = (DropDownList)lstQCDetails.InsertItem.FindControl("ddlAccountlist");
        if (Convert.ToInt32(ddlAccountlist.SelectedItem.Value) > 0)
        {
            QltyCntrlListBE.CUST_RELID = Convert.ToInt32(ddlAccountlist.SelectedItem.Value);
        }
        if (Convert.ToInt32(((DropDownList)e.FindControl("ddlProgramPeriods")).SelectedItem.Value) > 0)
        {
            QltyCntrlListBE.PremumAdj_Pgm_ID = Convert.ToInt32(((DropDownList)e.FindControl("ddlProgramPeriods")).SelectedItem.Value);
        }
        QltyCntrlListBE.CreatedUser_ID = CurrentAISUser.PersonID;
        QltyCntrlListBE.CreatedDate = DateTime.Now;
        bool Result = QltyCntrlListService.IsExistsIssueQCDetails(QltyCntrlListBE.PREMIUMADJ_STATUS_ID, QltyCntrlListBE.PremumAdj_Pgm_ID, QltyCntrlListBE.CHECKLISTITEM_ID, QltyCntrlListBE.CUSTOMER_ID, QltyCntrlListBE.QualityControlChklst_ID);
        if (!Result)
        {
            bool Flag = QltyCntrlListService.Update(QltyCntrlListBE);
            //Function call to bind QC IssueDetails

            BindDetailsList();
        }
        else
        {
            ShowMessage("The record cannot be saved. An identical record already exists.");
        }
    }
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
    protected void lnkclose_Click(object sender, EventArgs e)
    {
        pnlQCDetails.Visible = false;
        lblAdjQCDetails.Visible = false;
        divQCComplete.Visible = false;

    }
    /// <summary>
    /// Function to save the record into Premium adjustment comment and premium adjustment status tables
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        /// Function to Sve the 20% QC  details
        saveqcdetails();
    }
    /// <summary>
    /// Function to Sve the Adjustment QC  details
    /// </summary>
    public void saveqcdetails()
    {
        IList<PremiumAdjustmentStatusBE> premAdjStatus = PremAdjStsService.getPreAdjStatusList(AISMasterEntities.AdjusmentNumber);
        if (premAdjStatus.Count > 0)
        {

            // ViewState["PREMADJID"] = premAdjStatus[0].PremumAdj_ID;
            ViewState["PREMADJSTSID"] = premAdjStatus[0].PremumAdj_sts_ID;

        }
        if (ViewState["PREMADJSTSID"] == null)
        {
            premAdjStsBE = new PremiumAdjustmentStatusBE();
            premAdjStsBE.CreatedDate = DateTime.Now;
            premAdjStsBE.CreatedUser_ID = CurrentAISUser.PersonID;
        }
        else
        {
            premAdjStsBE = PremAdjStsService.getPreAdjStatusRow(Convert.ToInt32(ViewState["PREMADJSTSID"]));
            bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(PremAdjStatusBE20QCOld.UpdtDate), Convert.ToDateTime(premAdjStsBE.UpdtDate));
            if (!Concurrency)
                return;
        }
        premAdjStsBE.PremumAdj_ID = AISMasterEntities.AdjusmentNumber;
        premAdjStsBE.CustomerID = AISMasterEntities.AccountNumber;
        if (txtQCDate.Text != "")
        {
            premAdjStsBE.Review_Cmplt_Date = DateTime.Parse(txtQCDate.Text);
        }
        premAdjStsBE.ReviewPerson_ID = CurrentAISUser.PersonID;
        premAdjStsBE.CommentText = txtComments.Text;
        premAdjStsBE.EffectiveDate = DateTime.Now;
        premAdjStsBE.UpdtDate = DateTime.Now;
        bool Flag = PremAdjStsService.Update(premAdjStsBE);
        //if (ViewState["PREMADJSTSID"] != null)
        //{
        //    ShowConcurrentConflict(Flag, premAdjStsBE.ErrorMessage);
        //}
        if (Flag)
        {
            //   ReconReviewTransactionWrapper.SubmitTransactionChanges();
            //Function to bind the lstReconReview Listview
            BindList();
        }
        else
        {
            //  ReconReviewTransactionWrapper.RollbackChanges();
        }




    }
    /// <summary>
    /// Clicking this command button will cause “Twenty Percent Quality Control Indicator” field in the Premium Adjustment table to be updated to a value of true for the adjustment
    /// </summary>
    /// <param name="sendder"></param>
    /// <param name="e"></param>
    protected void btnQCComplete_Click(object sendder, EventArgs e)
    {
        PremAdjustmentBS PremAdjBS = new PremAdjustmentBS();
        PremiumAdjustmentBE PremAdjBE = PremAdjBS.getPremiumAdjustmentRow(Convert.ToInt32(ViewState["PREMADJID"]));
        bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(PremAdjBEConCurrent.UPDT_DT), Convert.ToDateTime(PremAdjBE.UPDT_DT));
        if (!Concurrency)
            return;
        PremAdjBE.TWENTY_PCT_QLTY_CNTRL_IND = true;
        PremAdjBE.TWENTY_PCT_QLTY_CNTRL_DT = DateTime.Now;
        PremAdjBE.TWENTY_PCT_QLTY_CNTRL_PERS_ID = CurrentAISUser.PersonID;
        PremAdjBE.UPDT_USER_ID = CurrentAISUser.PersonID;
        PremAdjBE.UPDT_DT = System.DateTime.Now;
        bool Flag = PremAdjBS.Update(PremAdjBE);
        ShowConcurrentConflict(Flag, PremAdjBE.ErrorMessage);
        /// Function to Sve the 20% QC  details
        saveqcdetails();
        if (Flag)
            btnQCComplete.Enabled = false;

    }
}
