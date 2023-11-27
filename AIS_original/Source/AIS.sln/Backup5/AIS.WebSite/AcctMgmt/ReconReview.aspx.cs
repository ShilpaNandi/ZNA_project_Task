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
using ZurichNA.LSP.Framework;
using ZurichNA.AIS.WebSite.ZDWJavaWS;
using ZurichNA.AIS.WebSite.ZDWJavaWS_CAD;
using Microsoft.Web.Services3.Security.Tokens;
public partial class AcctSetup_ReconReview : AISBasePage
{
    private PremiumAdjustmentStatusBS prem_Adj_StsService;
    private Qtly_Cntrl_ChklistBS qlty_Cntrl_ListService;
    private AdjQCBS adjQCItemService;
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
            SaveObjectToSessionUsingWindowName("PersonTransaction", value);
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
    private IList<Qtly_Cntrl_ChklistBE> QltyCntrlListBE
    {
        get 
        { 
            //return (IList<Qtly_Cntrl_ChklistBE>)Session["QLTYCNTRLLISTBE"]; 
            return (IList<Qtly_Cntrl_ChklistBE>)RetrieveObjectFromSessionUsingWindowName("QLTYCNTRLLISTBE"); 
        }
        set 
        { 
            //Session["QLTYCNTRLLISTBE"] = value; 
            SaveObjectToSessionUsingWindowName("QLTYCNTRLLISTBE", value);
        }
    }
    private PremiumAdjustmentStatusBE PremAdjStatusBEReconqcOld
    {
        get
        {
            //return ((Session["PremAdjStatusBEReconqcOld"] == null) ?
            //    (new PremiumAdjustmentStatusBE()) : (PremiumAdjustmentStatusBE)Session["PremAdjStatusBEReconqcOld"]);
            return ((RetrieveObjectFromSessionUsingWindowName("PremAdjStatusBEReconqcOld") == null) ?
               (new PremiumAdjustmentStatusBE()) : (PremiumAdjustmentStatusBE)RetrieveObjectFromSessionUsingWindowName("PremAdjStatusBEReconqcOld"));
        }

        set 
        { 
            //Session["PremAdjStatusBEReconqcOld"] = value; 
            SaveObjectToSessionUsingWindowName("PremAdjStatusBEReconqcOld", value);
        }
    }
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
    /// a property for Postal Address Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>PostalAddressBE</returns>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.Page.Title = "Recon Review";
        if (!IsPostBack)
        {
            if (CurrentAISUser.PersonID <= 0)
            {
                ShowError("UserID/AZCORPID not registered. Please register using Internal Contacts web page.");
                pnlReconReview.Enabled = false;
                pnlDetails.Enabled = false;
                return;
            }
            premAdjStsBE = new PremiumAdjustmentStatusBE();
            QltyCntrlListBE = new List<Qtly_Cntrl_ChklistBE>();
            //ReconReviewTransactionWrapper = new AISBusinessTransaction();
            if ((Request.QueryString.Count > 0) && !(Request.QueryString.Count == 1 && Request.QueryString["wID"] != null))
            {
                //This is to display the ReviewFeedback Data.
                btnDisplay.Visible = true;
                ddlQCDate.Visible = true;
                btnDisplay.Enabled = true;
                ddlQCDate.Enabled = true;
                DataTable dtRecon = PremAdjStsService.getReviewFeedback(AISMasterEntities.AdjusmentNumber, 2);
                ddlQCDate.DataSource = dtRecon;
                ddlQCDate.DataTextField = "QltyDate";
                ddlQCDate.DataValueField = "prem_adj_sts_id";
                ddlQCDate.DataBind();
                ListItem li = new ListItem("(Select)", "0");
                ddlQCDate.Items.Insert(0, li);
                pnlReconReview.Visible = false;
            }
            else
            {
                /// Funtion to Bind the lstReconReview ListView 

                BindList(false);

            }

        }

        //Checks Exiting without Save
        ArrayList list = new ArrayList();
        if(btnSave.Enabled)
            list.Add(txtComments);
        list.Add(txtReviewDate);
  //      list.Add(ddlQCDate);
        list.Add(calReviewDate);
        list.Add(btnApprove);
        list.Add(btnDisplay);
        list.Add(btnReject);
        list.Add(btnSave);
        ProcessExitFlag(list);
    }
    protected void btnDisplay_click(object sender, EventArgs e)
    {

        /// Funtion to Bind the lstReconReview ListView 
        BindList(true);
        pnlReconReview.Visible = true;
        btnSave.Enabled = false;
        pnlDetails.Enabled = false;

    }
    /// <summary>
    /// Funtion to Bind the lstReconReview ListView 
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    public void BindList(bool history)
    {
        IList<PremiumAdjustmentStatusBE> premAdjSts = PremAdjStsService.getPreAdjStatusList(AISMasterEntities.AdjusmentNumber);
        IList<LookupBE> lookups = (List<LookupBE>)Application["LookUpData"];
        PremAdjStatusBEReconqcOld = premAdjSts[0];
        if (premAdjSts[0].ADJ_STS_TYP_ID == 350 && premAdjSts[1].ADJ_STS_TYP_ID == 350 && ((premAdjSts[0].CommentText != "" && premAdjSts[0].CommentText != null) || (premAdjSts[0].Review_Cmplt_Date != null)))
        {
            history = true;
            lookups = lookups.Where(lk => lk.LookUpName == GlobalConstants.AdjustmentStatus.QCdDraftInv && lk.LookUpTypeName == "ADJUSTMENT STATUSES").ToList();
        }
        else
        {
            lookups = lookups.Where(lk => lk.LookUpName == GlobalConstants.AdjustmentStatus.ReconReviewed && lk.LookUpTypeName == "ADJUSTMENT STATUSES").ToList();
            if (!history)
                premAdjSts = premAdjSts.Where(sts => sts.ADJ_STS_TYP_ID == lookups[0].LookUpID).ToList();
            else
                premAdjSts = premAdjSts.Where(sts => sts.PremumAdj_sts_ID == Convert.ToInt32(ddlQCDate.SelectedValue)).ToList();
        }
        if (premAdjSts.Count > 0 && history)
        {
            if (premAdjSts[0].ReviewPerson_ID != null)
            {
                PersonBE persBE = (new PersonBS()).getPersonRow(Convert.ToInt32(premAdjSts[0].ReviewPerson_ID));
                if (persBE.PERSON_ID > 0)
                {   
                    //06/23 for veracode 
                    //lblReviewBy.Text = persBE.SURNAME + "," + persBE.FORENAME;
                    lblReviewBy.Text = Server.HtmlDecode(Server.HtmlEncode(persBE.SURNAME)) + "," + Server.HtmlDecode(Server.HtmlEncode(persBE.FORENAME));
                }
            }
            else
            {   
                 // 06/23 for veracode
                //lblReviewBy.Text = CurrentAISUser.FullName;
                lblReviewBy.Text = string.IsNullOrEmpty(CurrentAISUser.FullName.Trim()) ? "" : HttpUtility.HtmlDecode(HttpUtility.HtmlEncode(CurrentAISUser.FullName));
            }
            txtComments.Text = premAdjSts[0].CommentText;
            if (premAdjSts[0].Review_Cmplt_Date.HasValue)
            {
                txtReviewDate.Text = premAdjSts[0].Review_Cmplt_Date.Value.ToShortDateString();
            }
            else
            {
                txtReviewDate.Text = "";
            }
            ViewState["PREMADJSTSID"] = premAdjSts[0].PremumAdj_sts_ID;
            btnSave.Text = "Update";
            premAdjStsBE = premAdjSts[0];
            if (premAdjStsBE.APPROVEINDICATOR != null)
            {
                btnApprove.Enabled = false;
                btnReject.Enabled = false;
            }
            /// Funtion to Bind the lstReviewDetails ListView 
            BindDetailsList();
            pnlDetails.Visible = true;
        }
        else
        {
            //06/23 for veracode
            //lblReviewBy.Text = CurrentAISUser.FullName;
            lblReviewBy.Text = string.IsNullOrEmpty(CurrentAISUser.FullName.Trim()) ? "" : HttpUtility.HtmlDecode(HttpUtility.HtmlEncode(CurrentAISUser.FullName));
        }
    }
    /// <summary>
    /// Funtion to Bind the lstReviewDetails ListView 
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    public void BindDetailsList()
    {
        IList<LookupBE> lookups;
        lookups = (List<LookupBE>)Application["LookUpData"];
        lookups = lookups.Where(lk => lk.LookUpName == "Recon QC Issue").ToList();
        if (ViewState["PREMADJSTSID"] != null)
        {
            QltyCntrlListBE = QltyCntrlListService.getQtlychklistList(lookups[0].LookUpID, Convert.ToInt32(ViewState["PREMADJSTSID"]), AISMasterEntities.AccountNumber);
        }
        lstReviewDetails.DataSource = QltyCntrlListBE;
        lstReviewDetails.DataBind();
        if (lstReviewDetails.InsertItemPosition != InsertItemPosition.None)
        {
            DropDownList ddl = (DropDownList)lstReviewDetails.InsertItem.FindControl("ddlIssue");
            QCMasterIssueListBS issues = new QCMasterIssueListBS();
            ddl.DataSource = issues.getIssuesList(lookups[0].LookUpID);
            ddl.DataValueField = "QualityCntrlMstrIsslstID";
            ddl.DataTextField = "IssueText";
            ddl.DataBind();

            ListItem li = new ListItem("Select", "0");
            ddl.Items.Insert(0, li);
        }
        pnlDetails.Visible = true;
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

            //Function call to save the ReconReview Issue
            SaveQualityList(e.Item);
        }
        else if (e.CommandName.ToUpper() == "DISABLE" || e.CommandName.ToUpper() == "ENABLE")
        {
            //Function call to Enable or Disable the Record
            DisableRow(e, e.CommandName == "DISABLE" ? false : true);
        }

    }
    /// <summary>
    /// function for Person record to make enable or disable
    /// </summary>
    /// <param name="personID"></param>
    /// <param name="Flag">True/False boolean value</param>
    /// <returns></returns>
    protected void DisableRow(ListViewCommandEventArgs e, bool Flag)
    {
        try
        {
            if (ViewState["PREMADJSTSID"] != null)
            {
                Qtly_Cntrl_ChklistBE newQltyCntrlListBE = QltyCntrlListService.getQualityControlRow(Convert.ToInt32(e.CommandArgument));
                newQltyCntrlListBE.ACTIVE = Flag;
                Flag = QltyCntrlListService.Update(newQltyCntrlListBE);
                ShowConcurrentConflict(Flag, newQltyCntrlListBE.ErrorMessage);
            }
            else
            {
                ListViewDataItem item = (ListViewDataItem)e.Item;
                QltyCntrlListBE[item.DisplayIndex].ACTIVE = Flag;
            }
            //Function call to bind lstReviewDetails
            BindDetailsList();
        }
        catch (RetroBaseException ee)
        {
            ShowError(ee.Message, ee);
        }
    }
    /// <summary>
    /// Function to Insert new Recon Review Issue Record
    /// </summary>
    /// <param name="e"></param>
    protected void SaveQualityList(ListViewItem e)
    {
        Qtly_Cntrl_ChklistBE newQltyCntrlListBE = new Qtly_Cntrl_ChklistBE();
        newQltyCntrlListBE.CHECKLISTITEM_ID = Convert.ToInt32(((DropDownList)e.FindControl("ddlIssue")).SelectedItem.Value);
        newQltyCntrlListBE.CUSTOMER_ID = AISMasterEntities.AccountNumber;
        newQltyCntrlListBE.PREMIUMADJUSTMENT_ID = AISMasterEntities.AdjusmentNumber;
        newQltyCntrlListBE.ACTIVE = true;
        newQltyCntrlListBE.CreatedUser_ID = CurrentAISUser.PersonID;
        newQltyCntrlListBE.CreatedDate = DateTime.Now;
        if (ViewState["PREMADJSTSID"] != null)
        {
            newQltyCntrlListBE.PREMIUMADJ_STATUS_ID = Convert.ToInt32(ViewState["PREMADJSTSID"].ToString());
            bool Result = QltyCntrlListService.IsExistsIssue(newQltyCntrlListBE.PREMIUMADJ_STATUS_ID, newQltyCntrlListBE.CHECKLISTITEM_ID, newQltyCntrlListBE.CUSTOMER_ID, newQltyCntrlListBE.QualityControlChklst_ID);
            if (!Result)
            {
                bool Flag = QltyCntrlListService.Update(newQltyCntrlListBE);
                //Function call to bind lstReviewDetails
                BindDetailsList();
                return;
            }
            else
            {
                ShowMessage("The record cannot be saved. An identical record already exists.");
                return;
            }
        }
        else
        {
            newQltyCntrlListBE.PREMIUMADJ_STATUS_ID = Convert.ToInt32(ViewState["PREMADJSTSID"].ToString());
            bool Result = QltyCntrlListService.IsExistsIssue(newQltyCntrlListBE.PREMIUMADJ_STATUS_ID, newQltyCntrlListBE.CHECKLISTITEM_ID, newQltyCntrlListBE.CUSTOMER_ID, newQltyCntrlListBE.QualityControlChklst_ID);
        }
        if (QltyCntrlListBE.Where(chk => chk.CHECKLISTITEM_ID == Convert.ToInt32(((DropDownList)e.FindControl("ddlIssue")).SelectedItem.Value)).Count() > 0)
        {
            ShowMessage("The record cannot be saved. An identical record already exists.");
            return;
        }
        newQltyCntrlListBE.CHKLISTNAME = ((DropDownList)e.FindControl("ddlIssue")).SelectedItem.Text;
        newQltyCntrlListBE.PREMIUMADJ_STATUS_ID = -(QltyCntrlListBE.Count + 1);
        QltyCntrlListBE.Add(newQltyCntrlListBE);
        //Function call to bind lstReviewDetails
        BindDetailsList();
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
    // added additional condition to fix duplicate adjustment status isse EAIS-6
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ViewState["PREMADJSTSID"] == null)
        {
            IList<PremiumAdjustmentStatusBE> premList = new List<PremiumAdjustmentStatusBE>();
            premList = PremAdjStsService.getPreAdjStatusList(AISMasterEntities.AdjusmentNumber);
            if (premList.Count > 1)
            {
                if (premList[0].ADJ_STS_TYP_ID == 350 && premList[1].ADJ_STS_TYP_ID == 350 || premList[0].ADJ_STS_TYP_ID == 351 || premList[0].ADJ_STS_TYP_ID == 353)
                {
                    ClearError();
                    validtionSummary();
                    modalReconReview.Show();
                    return;
                }
            }
            premAdjStsBE = new PremiumAdjustmentStatusBE();
            premAdjStsBE.CreatedDate = DateTime.Now;
            premAdjStsBE.CreatedUser_ID = CurrentAISUser.PersonID;
        }
        else
        {
            premAdjStsBE = PremAdjStsService.getPreAdjStatusRow(Convert.ToInt32(ViewState["PREMADJSTSID"]));
            bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(premAdjStsBE.UpdtDate), Convert.ToDateTime(PremAdjStatusBEReconqcOld.UpdtDate));
            if (!Concurrency)
            {
                ClearError();
                validtionSummary();
                modalReconReview.Show();
                return;
            }
              
        }
        premAdjStsBE.PremumAdj_ID = AISMasterEntities.AdjusmentNumber;
        premAdjStsBE.CustomerID = AISMasterEntities.AccountNumber;
        if (txtReviewDate.Text != "")
        {
            premAdjStsBE.Review_Cmplt_Date = DateTime.Parse(txtReviewDate.Text).Add(DateTime.Now.TimeOfDay);
        }
        premAdjStsBE.CommentText = txtComments.Text;
        premAdjStsBE.EffectiveDate = DateTime.Now;
        premAdjStsBE.UpdtDate = DateTime.Now;
        premAdjStsBE.UpdtUserID = CurrentAISUser.PersonID;
        btnSave.Text = "Update";
        if (ViewState["PREMADJSTSID"] != null)
        {
            bool Flag = PremAdjStsService.Update(premAdjStsBE);
        }
        if (((txtComments.Text != "" && txtComments.Text != null) || (txtReviewDate.Text != "" && txtReviewDate.Text != null)) && ViewState["PREMADJSTSID"] == null)
        {
            premAdjStsBE.ADJ_STS_TYP_ID = 350;                            
            bool blnSaveFlag = PremAdjStsService.Update(premAdjStsBE);
            ViewState["PREMADJSTSID"] = premAdjStsBE.PremumAdj_sts_ID;
            if (blnSaveFlag == false)
            {
               ShowError("Unable to save the record");
               return;
            }            
        }
        PremAdjStatusBEReconqcOld = premAdjStsBE;

        /// Funtion to Bind the lstReviewDetails ListView 
        BindDetailsList();
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        DateTime dteff_dt = DateTime.Now;
        if (ViewState["PREMADJSTSID"] != null)
        {
            IList<PremiumAdjustmentStatusBE> premList = new List<PremiumAdjustmentStatusBE>();
            premList = PremAdjStsService.getPreAdjStatusList(AISMasterEntities.AdjusmentNumber);
            if (premList.Count > 1)
            {
                if (premList[0].ADJ_STS_TYP_ID == 351 || premList[0].ADJ_STS_TYP_ID == 346)
                {
                    ClearError();
                    validtionSummary();
                    modalReconReview.Show();
                    return;
                }
            }
            premAdjStsBE = PremAdjStsService.getPreAdjStatusRow(Convert.ToInt32(ViewState["PREMADJSTSID"]));
            bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(PremAdjStatusBEReconqcOld.UpdtDate),Convert.ToDateTime(premAdjStsBE.UpdtDate));
            if (!Concurrency)
            {
                ClearError();
                validtionSummary();
                modalReconReview.Show();
                return;
            }
        }
        
        string strstatus = AdjQCItemService.ModAISProcessReconReview(premAdjStsBE.PremumAdj_ID, CurrentAISUser.PersonID);

        if (strstatus == "RECON REVIEW")
        {
            premAdjStsBE.PremumAdj_ID = AISMasterEntities.AdjusmentNumber;
            premAdjStsBE.CustomerID = AISMasterEntities.AccountNumber;
            premAdjStsBE.APPROVEINDICATOR = true;
            if (txtReviewDate.Text != "")
            {
                premAdjStsBE.Review_Cmplt_Date = DateTime.Parse(txtReviewDate.Text).Add(DateTime.Now.TimeOfDay);
            }
            premAdjStsBE.ReviewPerson_ID = CurrentAISUser.PersonID;
            IList<LookupBE> lookups = (List<LookupBE>)Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpName == GlobalConstants.AdjustmentStatus.ReconReviewed && lk.LookUpTypeName == "ADJUSTMENT STATUSES").ToList();
            premAdjStsBE.ADJ_STS_TYP_ID = lookups[0].LookUpID;
            premAdjStsBE.EffectiveDate = DateTime.Now;
            premAdjStsBE.UpdtUserID = CurrentAISUser.PersonID;
            premAdjStsBE.UpdtDate = DateTime.Now;
            bool Flag = PremAdjStsService.Update(premAdjStsBE);
            //ShowConcurrentConflict(Flag, premAdjStsBE.ErrorMessage);
            if (Flag)
            {
                //for Issues
                for (int i = 0; i < QltyCntrlListBE.Count; i++)
                {
                    if (QltyCntrlListBE[i].PREMIUMADJ_STATUS_ID <= 0)
                        QltyCntrlListBE[i].PREMIUMADJ_STATUS_ID = premAdjStsBE.PremumAdj_sts_ID;
                    Flag = QltyCntrlListService.Update(QltyCntrlListBE[i]);
                    //ShowConcurrentConflict(Flag, QltyCntrlListBE[i].ErrorMessage);
                }

                IList<PremiumAdjustmentStatusBE> premAdjSts = PremAdjStsService.getPreAdjStatusList(AISMasterEntities.AdjusmentNumber);
                lookups = (List<LookupBE>)Application["LookUpData"];
                lookups = lookups.Where(lk => lk.LookUpName == GlobalConstants.AdjustmentStatus.QCdDraftInv && lk.LookUpTypeName == "ADJUSTMENT STATUSES").ToList();
                premAdjSts = premAdjSts.Where(sts => sts.ADJ_STS_TYP_ID == lookups[0].LookUpID && sts.APPROVEINDICATOR != null).ToList();
                if (premAdjSts.Count > 0)
                {
                    premAdjStsBE = PremAdjStsService.getPreAdjStatusRow(premAdjSts[0].PremumAdj_sts_ID);
                    premAdjStsBE.EXPIRYDATE = DateTime.Now;
                    premAdjStsBE.UpdtDate = DateTime.Now;
                    premAdjStsBE.UpdtUserID = CurrentAISUser.PersonID;
                    Flag = PremAdjStsService.Update(premAdjStsBE);
                    //ShowConcurrentConflict(Flag, premAdjStsBE.ErrorMessage);
                }
                PremiumAdjustmentBE premAdjBE = (new PremAdjustmentBS()).getPremiumAdjustmentRow(premAdjStsBE.PremumAdj_ID);
                premAdjBE.ADJ_STS_TYP_ID = 351;//Recon Review
                premAdjBE.ADJ_STS_EFF_DT = DateTime.Now;
                premAdjBE.RECONCILER_REVW_IND = true;
                premAdjBE.UPDT_DT = DateTime.Now;
                premAdjBE.UPDT_USER_ID = CurrentAISUser.PersonID;
                Flag = (new PremAdjustmentBS()).Update(premAdjBE);
                //ShowConcurrentConflict(Flag, premAdjBE.ErrorMessage);

            }
            System.Threading.Thread.Sleep(10000);
            ResetReconPanel();

        }
        else if (strstatus == "UW REVIEW")
        {
            premAdjStsBE.PremumAdj_ID = AISMasterEntities.AdjusmentNumber;
            premAdjStsBE.CustomerID = AISMasterEntities.AccountNumber;
            premAdjStsBE.APPROVEINDICATOR = true;
            if (txtReviewDate.Text != "")
            {
                premAdjStsBE.Review_Cmplt_Date = DateTime.Parse(txtReviewDate.Text).Add(DateTime.Now.TimeOfDay);
            }
            premAdjStsBE.ReviewPerson_ID = CurrentAISUser.PersonID;
            IList<LookupBE> lookups = (List<LookupBE>)Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpName == GlobalConstants.AdjustmentStatus.ReconReviewed && lk.LookUpTypeName == "ADJUSTMENT STATUSES").ToList();
            premAdjStsBE.ADJ_STS_TYP_ID = lookups[0].LookUpID;
            premAdjStsBE.EffectiveDate = dteff_dt;
            premAdjStsBE.EXPIRYDATE = dteff_dt;
            premAdjStsBE.UpdtUserID = CurrentAISUser.PersonID;
            premAdjStsBE.UpdtDate = dteff_dt;
            bool Flag = PremAdjStsService.Update(premAdjStsBE);
            //ShowConcurrentConflict(Flag, premAdjStsBE.ErrorMessage);

            if (Flag)
            {
                //for Issues
                for (int i = 0; i < QltyCntrlListBE.Count; i++)
                {
                    if (QltyCntrlListBE[i].PREMIUMADJ_STATUS_ID <= 0)
                        QltyCntrlListBE[i].PREMIUMADJ_STATUS_ID = premAdjStsBE.PremumAdj_sts_ID;
                    Flag = QltyCntrlListService.Update(QltyCntrlListBE[i]);
                    //ShowConcurrentConflict(Flag, QltyCntrlListBE[i].ErrorMessage);
                }

                IList<PremiumAdjustmentStatusBE> premAdjSts = PremAdjStsService.getPreAdjStatusList(AISMasterEntities.AdjusmentNumber);
                lookups = (List<LookupBE>)Application["LookUpData"];
                lookups = lookups.Where(lk => lk.LookUpName == GlobalConstants.AdjustmentStatus.QCdDraftInv && lk.LookUpTypeName == "ADJUSTMENT STATUSES").ToList();
                premAdjSts = premAdjSts.Where(sts => sts.ADJ_STS_TYP_ID == lookups[0].LookUpID && sts.APPROVEINDICATOR != null).ToList();
                if (premAdjSts.Count > 0)
                {
                    premAdjStsBE = PremAdjStsService.getPreAdjStatusRow(premAdjSts[0].PremumAdj_sts_ID);
                    premAdjStsBE.EXPIRYDATE = dteff_dt;
                    premAdjStsBE.UpdtDate = dteff_dt;
                    premAdjStsBE.UpdtUserID = CurrentAISUser.PersonID;
                    Flag = PremAdjStsService.Update(premAdjStsBE);
                    
                }
                //ShowConcurrentConflict(Flag, premAdjBE.ErrorMessage);

            }
            System.Threading.Thread.Sleep(10000);

            btnApprove.Enabled = false;
            btnReject.Enabled = false;
            /// Funtion to Bind the lstReconReview ListView 

            BindList(false);
            pnlReconReview.Enabled = false;
            pnlDetails.Enabled = false;

            ShowError("Reconciler Review Approved Successfully and Moved to " + strstatus + " Status Based on the Auto Approval process.");
        }
        else
        {
            ShowError("Error occured in approval process.");
        }       
    }

    private void ResetReconPanel()
    {
        IList<PremiumAdjustmentStatusBE> premUpdateList = new List<PremiumAdjustmentStatusBE>();
        premUpdateList = PremAdjStsService.getPreAdjStatusList(AISMasterEntities.AdjusmentNumber);
        if (premUpdateList.Count > 1)
        {
            if ((premUpdateList[0].ADJ_STS_TYP_ID == 351 || premUpdateList[0].ADJ_STS_TYP_ID == 353)
                && premUpdateList[0].UpdtUserID == CurrentAISUser.PersonID)
            {
                btnApprove.Enabled = false;
                btnReject.Enabled = false;
                /// Funtion to Bind the lstReconReview ListView 

                BindList(false);
                pnlReconReview.Enabled = false;
                pnlDetails.Enabled = false;

                //Email Notification
                if (premUpdateList[0].ADJ_STS_TYP_ID == 351)
                {
                    EMailNotification(premAdjStsBE.PremumAdj_ID, true);
                }
            }
            else
            {
                ClearError();
                validtionSummary();
                modalReconReview.Show();
                return;
            }
        }
    }

    /// <summary>
    /// Method to Send the EMail
    /// </summary>
    /// <param name="intAdjNo"></param>
    /// <param name="iFlag"></param>
    #region EMailNotification
    private void EMailNotification(int intAdjNo, bool Flag)
    {
        try
        {
            SMTPMailer dd = new SMTPMailer();
            if (Flag)
                dd.HasAttachment = true;
            System.Net.Mail.MailAddressCollection objMailList = new System.Net.Mail.MailAddressCollection();
            dd.lstTomailAddress = objMailList;
            IList<PremiumAdjustmentBE> EmailInfo = new PremAdjustmentBS().GetEMailInfo(intAdjNo);
            IList<string> strEmailList = new PremAdjustmentBS().getEmailIDS(ZurichNA.AIS.DAL.Logic.GlobalConstants.RESPONSIBILITIES.RECON, Flag, EmailInfo[0].CUSTOMERID);
            for (int i = 0; i < strEmailList.Count; i++)
            {
                objMailList.Add(strEmailList[i].ToString());
            }
            if (Flag)
            {
                
                try
                {
                    bool IsCanadaAcct = (new AccountBS()).IsCanadaAccount(intAdjNo);
                    if (IsCanadaAcct)
                    {
                        ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.CommunicationManagerService objJavaWS = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.CommunicationManagerService();
                        ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.ElectronicDocument objDocument = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.ElectronicDocument();
                        UsernameToken token = new UsernameToken(ConfigurationManager.AppSettings["ZDWUserName"].ToString(), ConfigurationManager.AppSettings["ZDWPassWord"].ToString(), PasswordOption.SendPlainText);
                        objJavaWS.RequestSoapContext.Security.Timestamp.TtlInSeconds = 60;
                        objJavaWS.RequestSoapContext.Security.Tokens.Add(token);
                        objJavaWS.RequestSoapContext.Security.MustUnderstand = false;
                        //Internal
                        string strInternal = ((new PremAdjustmentBS()).getPremiumAdjustmentRow(intAdjNo)).DRFT_INTRNL_PDF_ZDW_KEY_TXT;
                        objDocument = objJavaWS.retrieveDocument("ZDW_DOC04." + strInternal, "D");
                        ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement objDocContentElement = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement();
                        objDocContentElement = objDocument.documentContentElement[0];
                        ZAAttachment at = new ZAAttachment();
                        at.AttachmentName = "ReconReviewdoc_Internal_" + intAdjNo.ToString() + ".pdf";
                        at.AttachmentType = SMTPMailer.PDF;
                        at.Attach = objDocContentElement.theBinaryValue.ToArray();
                        dd.AttachmentList.Add(at);
                        //External
                        string strExternal = ((new PremAdjustmentBS()).getPremiumAdjustmentRow(intAdjNo)).DRFT_EXTRNL_PDF_ZDW_KEY_TXT;
                        objDocument = objJavaWS.retrieveDocument("ZDW_DOC04." + strExternal, "D");
                        objDocContentElement = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement();
                        objDocContentElement = objDocument.documentContentElement[0];
                        at = new ZAAttachment();
                        at.AttachmentName = "ReconReviewdoc_External_" + intAdjNo.ToString() + ".pdf";
                        at.AttachmentType = SMTPMailer.PDF;
                        at.Attach = objDocContentElement.theBinaryValue.ToArray();
                        dd.AttachmentList.Add(at);
                        //Coding WorkSheet
                        string strWorkkSheet = ((new PremAdjustmentBS()).getPremiumAdjustmentRow(intAdjNo)).DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT;
                        objDocument = objJavaWS.retrieveDocument("ZDW_DOC04." + strWorkkSheet, "D");
                        objDocContentElement = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement();
                        objDocContentElement = objDocument.documentContentElement[0];
                        at = new ZAAttachment();
                        at.AttachmentName = "ReconReviewdoc_CodingWorkSheet_" + intAdjNo.ToString() + ".pdf";
                        at.AttachmentType = SMTPMailer.PDF;
                        at.Attach = objDocContentElement.theBinaryValue.ToArray();
                        dd.AttachmentList.Add(at);
                    }
                    else
                    {
                        CommunicationManagerServiceWse objJavaWS = new CommunicationManagerServiceWse();
                        ZurichNA.AIS.WebSite.ZDWJavaWS.ElectronicDocument objDocument = new ZurichNA.AIS.WebSite.ZDWJavaWS.ElectronicDocument();
                        UsernameToken token = new UsernameToken(ConfigurationManager.AppSettings["ZDWUserName"].ToString(), ConfigurationManager.AppSettings["ZDWPassWord"].ToString(), PasswordOption.SendPlainText);
                        objJavaWS.RequestSoapContext.Security.Timestamp.TtlInSeconds = 60;
                        objJavaWS.RequestSoapContext.Security.Tokens.Add(token);
                        objJavaWS.RequestSoapContext.Security.MustUnderstand = false;
                        //Internal
                        string strInternal = ((new PremAdjustmentBS()).getPremiumAdjustmentRow(intAdjNo)).DRFT_INTRNL_PDF_ZDW_KEY_TXT;
                        objDocument = objJavaWS.retrieveDocument(strInternal, "D");
                        ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement objDocContentElement = new ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement();
                        objDocContentElement = objDocument.documentContentElement[0];
                        ZAAttachment at = new ZAAttachment();
                        at.AttachmentName = "ReconReviewdoc_Internal_" + intAdjNo.ToString() + ".pdf";
                        at.AttachmentType = SMTPMailer.PDF;
                        at.Attach = objDocContentElement.theBinaryValue.ToArray();
                        dd.AttachmentList.Add(at);
                        //External
                        string strExternal = ((new PremAdjustmentBS()).getPremiumAdjustmentRow(intAdjNo)).DRFT_EXTRNL_PDF_ZDW_KEY_TXT;
                        objDocument = objJavaWS.retrieveDocument(strExternal, "D");
                        objDocContentElement = new ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement();
                        objDocContentElement = objDocument.documentContentElement[0];
                        at = new ZAAttachment();
                        at.AttachmentName = "ReconReviewdoc_External_" + intAdjNo.ToString() + ".pdf";
                        at.AttachmentType = SMTPMailer.PDF;
                        at.Attach = objDocContentElement.theBinaryValue.ToArray();
                        dd.AttachmentList.Add(at);
                        //Coding WorkSheet
                        string strWorkkSheet = ((new PremAdjustmentBS()).getPremiumAdjustmentRow(intAdjNo)).DRFT_CD_WRKSHT_PDF_ZDW_KEY_TXT;
                        objDocument = objJavaWS.retrieveDocument(strWorkkSheet, "D");
                        objDocContentElement = new ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement();
                        objDocContentElement = objDocument.documentContentElement[0];
                        at = new ZAAttachment();
                        at.AttachmentName = "ReconReviewdoc_CodingWorkSheet_" + intAdjNo.ToString() + ".pdf";
                        at.AttachmentType = SMTPMailer.PDF;
                        at.Attach = objDocContentElement.theBinaryValue.ToArray();
                        dd.AttachmentList.Add(at);
                    }
                }

                catch (System.Web.Services.Protocols.SoapException ex)
                {
                    //return;
                    ShowError("Unable to attach the document(s) to the Email due to an Exception: " +ex.ToString());
                    goto Flag;
                }

            }
            Flag:
            string strSubject = "Reconciler Review " + "{" + EmailInfo[0].DRAFTINVNO + "}" + " " + " with invoice amount " + isNegitive(EmailInfo[0].INVC_AMT) + " " + "for " + EmailInfo[0].CUSTMR_NAME + " " + " for - " + EmailInfo[0].BUNAME;
            string strBody = "Reconciler Review " + "{" + EmailInfo[0].DRAFTINVNO + "}" + " ";
            if (Flag)
                strBody += " has been approved for the following" + "\r\n";
            else
                strBody += " has been Rejected for the following" + "\r\n";
            strBody = strBody + "\r\n";
            strBody = strBody + "Insured Name:" + EmailInfo[0].CUSTMR_NAME + "\r\n";
            strBody = strBody + "Broker Name:" + EmailInfo[0].BROKERNAME + "\r\n";
            strBody = strBody + "Valuation Date:" + EmailInfo[0].VALUATIONDATE + "\r\n";
            strBody = strBody + "Invoice Number:" + EmailInfo[0].DRAFTINVNO + "\r\n";
            strBody = strBody + "Invoice Amount: " + isNegitive(EmailInfo[0].INVC_AMT) + "\r\n";
            strBody = strBody + "BU Name:" + EmailInfo[0].BUNAME + "\r\n";
            
            if (!Flag)
            {
                string strIssue = "";
                if (QltyCntrlListBE.Count > 0)
                {
                    for (int i = 0; i < QltyCntrlListBE.Count; i++)
                    {
                        if (QltyCntrlListBE[i].ACTIVE == true)
                        {
                            if (strIssue == string.Empty)
                                strIssue = QltyCntrlListBE[i].CHKLISTNAME;
                            else
                                strIssue = strIssue + ", " + QltyCntrlListBE[i].CHKLISTNAME;
                        }
                    }
                    strBody = strBody + "Recon Issue:" + strIssue + "\r\n";
                }
            }
            strBody = strBody + "\r\n";
            strBody = strBody + "\r\n";
            if (Flag)
            {
                strBody = strBody + "The pdf copy of the draft invoice is attached to this email." + "\r\n";
                strBody = strBody + "Please review adjustment for UW Review process." + "\r\n";
            }
            else
                strBody = strBody + "Please check Reconciliation Review for details." + "\r\n";
            dd.Subject = strSubject;
            dd.Body = strBody;
            RandomWait(100);
            dd.SendMail();
        }
        catch (Exception ee)
        {
            if (Flag)
            {
                ShowError("Reconciler Review Approved. Email Notification Failed.");
                return;
            }
            else
            {
                ShowError("Reconciler Review Rejected. Email Notification Failed.");
                return;
            }
        }
        if (Flag)
        {
            ShowError("Reconciler Review Approved Successfully.");
            return;
        }
        else
        {
            ShowError("Reconciler Review Rejected Successfully.");
            return;
        }
    }
    #endregion
    protected void btnReject_Click(object sender, EventArgs e)
    {
        //code to set Calculation Adjustment Status Code for the rejected adjustment to a blank
        PremiumAdjustmentBE PremAdjBE = (new PremAdjustmentBS()).getPremiumAdjustmentRow(AISMasterEntities.AdjusmentNumber);
        PremAdjBE.CALC_ADJ_STS_CODE = "";
        bool Flag = (new PremAdjustmentBS()).Update(PremAdjBE);
        ShowConcurrentConflict(Flag, PremAdjBE.ErrorMessage);
        if (ViewState["PREMADJSTSID"] != null)
        {
            IList<PremiumAdjustmentStatusBE> premList = new List<PremiumAdjustmentStatusBE>();
            premList = PremAdjStsService.getPreAdjStatusList(AISMasterEntities.AdjusmentNumber);
            if (premList.Count > 1)
            {
                if (premList[0].ADJ_STS_TYP_ID == 346 || premList[0].ADJ_STS_TYP_ID == 351)
                {
                    ClearError();
                    validtionSummary();
                    modalReconReview.Show();
                    return;
                }
            }
            premAdjStsBE = PremAdjStsService.getPreAdjStatusRow(Convert.ToInt32(ViewState["PREMADJSTSID"]));
            bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(PremAdjStatusBEReconqcOld.UpdtDate),Convert.ToDateTime(premAdjStsBE.UpdtDate));
            if (!Concurrency)
            {
                ClearError();
                validtionSummary();
                modalReconReview.Show();
                return;
            }
        }
        premAdjStsBE.PremumAdj_ID = AISMasterEntities.AdjusmentNumber;
        premAdjStsBE.CustomerID = AISMasterEntities.AccountNumber;
        premAdjStsBE.ReviewPerson_ID = CurrentAISUser.PersonID;
        premAdjStsBE.UpdtDate = DateTime.Now;
        premAdjStsBE.UpdtUserID = CurrentAISUser.PersonID;

        if (txtReviewDate.Text != "")
        {
            premAdjStsBE.Review_Cmplt_Date = DateTime.Parse(txtReviewDate.Text).Add(DateTime.Now.TimeOfDay);
        }
        IList<LookupBE> lookups = (List<LookupBE>)Application["LookUpData"];
        lookups = lookups.Where(lk => lk.LookUpName == GlobalConstants.AdjustmentStatus.Calc && lk.LookUpTypeName == "ADJUSTMENT STATUSES").ToList();
        premAdjStsBE.ADJ_STS_TYP_ID = lookups[0].LookUpID;
        premAdjStsBE.APPROVEINDICATOR = false;
        premAdjStsBE.EffectiveDate = DateTime.Now;
        Flag = PremAdjStsService.Update(premAdjStsBE);
        //ShowConcurrentConflict(Flag, premAdjStsBE.ErrorMessage);
        if (Flag)
        {
            //for Issues
            for (int i = 0; i < QltyCntrlListBE.Count; i++)
            {
                if (QltyCntrlListBE[i].PREMIUMADJ_STATUS_ID <= 0)
                    QltyCntrlListBE[i].PREMIUMADJ_STATUS_ID = premAdjStsBE.PremumAdj_sts_ID;
                Flag = QltyCntrlListService.Update(QltyCntrlListBE[i]);
                //ShowConcurrentConflict(Flag, QltyCntrlListBE[i].ErrorMessage);
            }

            IList<PremiumAdjustmentStatusBE> premAdjSts = PremAdjStsService.getPreAdjStatusList(AISMasterEntities.AdjusmentNumber);
            lookups = (List<LookupBE>)Application["LookUpData"];
            lookups = lookups.Where(lk => lk.LookUpName == GlobalConstants.AdjustmentStatus.QCdDraftInv && lk.LookUpTypeName == "ADJUSTMENT STATUSES").ToList();
            premAdjSts = premAdjSts.Where(sts => sts.ADJ_STS_TYP_ID == lookups[0].LookUpID && sts.APPROVEINDICATOR != null).ToList();
            if (premAdjSts.Count > 0)
            {
                premAdjStsBE = PremAdjStsService.getPreAdjStatusRow(premAdjSts[0].PremumAdj_sts_ID);
                premAdjStsBE.EXPIRYDATE = DateTime.Now;
                premAdjStsBE.UpdtDate = DateTime.Now;
                premAdjStsBE.UpdtUserID = CurrentAISUser.PersonID;
                Flag = PremAdjStsService.Update(premAdjStsBE);
                //ShowConcurrentConflict(Flag, premAdjStsBE.ErrorMessage);
            }
            PremiumAdjustmentBE premAdjBE = (new PremAdjustmentBS()).getPremiumAdjustmentRow(premAdjStsBE.PremumAdj_ID);
            premAdjBE.ADJ_STS_TYP_ID = 346;//CALC
            premAdjBE.ADJ_STS_EFF_DT = DateTime.Now;
            premAdjBE.RECONCILER_REVW_IND = true;
            premAdjBE.UPDT_DT = DateTime.Now;
            premAdjBE.UPDT_USER_ID = CurrentAISUser.PersonID;
            Flag = (new PremAdjustmentBS()).Update(premAdjBE);
            //ShowConcurrentConflict(Flag, premAdjBE.ErrorMessage);

        }

        System.Threading.Thread.Sleep(10000);

        IList<PremiumAdjustmentStatusBE> premUpdateList = new List<PremiumAdjustmentStatusBE>();
        premUpdateList = PremAdjStsService.getPreAdjStatusList(AISMasterEntities.AdjusmentNumber);
        if (premUpdateList.Count > 1)
        {
            if (premUpdateList[0].ADJ_STS_TYP_ID == 346 && premUpdateList[0].UpdtUserID == CurrentAISUser.PersonID)
            {
                btnReject.Enabled = false;
                btnApprove.Enabled = false;
                /// Funtion to Bind the lstReconReview ListView 

                BindList(false);
                pnlReconReview.Enabled = false;
                pnlDetails.Enabled = false;
                //Code for Email Notification
                EMailNotification(premAdjStsBE.PremumAdj_ID, false); 
            }
            else
            {
                ClearError();
                validtionSummary();
                modalReconReview.Show();
                return;
            }
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
