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
using System.Text.RegularExpressions;


public partial class AcctSetup_Aries_Clearing : AISBasePage
{
    private Qtly_Cntrl_ChklistBS qlty_Cntrl_ListService;
    protected bool CheckNew
    {
        get { return (bool)ViewState["CheckNew"]; }
        set { ViewState["CheckNew"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Method to populate the details in the grids...
        if (!IsPostBack)
        {
            CheckNew = true;
            QltyCntrlListBE = new Qtly_Cntrl_ChklistBE();
            AriesTransactionWrapper = new AISBusinessTransaction();
            BindAriesDetails();
            BindPaymentDetails();
            BindIssueslist();
        }

        //Checks Exiting without Save
        ArrayList list = new ArrayList();
        list.Add(txtAmount);
        list.Add(txtAriesQCdate);
        list.Add(txtBilleddate);
        list.Add(txtCheck);
        list.Add(txtComments);
        list.Add(txtPostingdate);
        list.Add(txtRecondate);
        list.Add(txtDtreciev_CalendarExtender1);
        list.Add(txtPostdate_CalendarExtender);
        list.Add(txtPostingdate_CalendarExtender);
        list.Add(txtQCdate_Calenderextender);
        list.Add(btnDetailssave);
        list.Add(btnPaymentsave);
        list.Add(btnfinalise);
        ProcessExitFlag(list);
    }
    private Qtly_Cntrl_ChklistBS QltyCntrlListService
    {
        get
        {
            if (qlty_Cntrl_ListService == null)
            {
                qlty_Cntrl_ListService = new Qtly_Cntrl_ChklistBS();
                //qlty_Cntrl_ListService.AppTransactionWrapper = AriesTransactionWrapper;
            }
            return qlty_Cntrl_ListService;
        }
    }
    protected AISBusinessTransaction AriesTransactionWrapper
    {
        get
        {
            //if ((AISBusinessTransaction)Session["AriesTransactionWrapper"] == null)
            //    Session["AriesTransactionWrapper"] = new AISBusinessTransaction();
            if ((AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("AriesTransactionWrapper") == null)
                SaveObjectToSessionUsingWindowName("AriesTransactionWrapper", new AISBusinessTransaction());

            //return (AISBusinessTransaction)Session["AriesTransactionWrapper"];
            return (AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("AriesTransactionWrapper");
        }
        set
        {
            //Session["AriesTransactionWrapper"] = value;
            SaveObjectToSessionUsingWindowName("AriesTransactionWrapper", value);
        }
    }
    private AriesClearingBS ariesclearingBS;
    private AriesClearingBS AriesclearingBS
    {
        get
        {
            if (ariesclearingBS == null)
            {
                ariesclearingBS = new AriesClearingBS();
                //ariesclearingBS.AppTransactionWrapper = AriesTransactionWrapper;
            }
            return ariesclearingBS;
        }
    }
    private CustomerContactBS customercontactBS;
    private CustomerContactBS CustomercontactBS
    {
        get
        {

            if (customercontactBS == null)
                customercontactBS = new CustomerContactBS();
            return customercontactBS;
        }
    }
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

    /// <summary>
    /// This method is used to bind the Payment details.
    /// </summary>
    public void BindPaymentDetails()
    {
        DateTime dtRecondate;
        if (AISMasterEntities != null)
        {

            dtRecondate = DateTime.Parse(AISMasterEntities.InvoiceDate.ToString());
            bool validdate = IsValidDate(dtRecondate);
            if (validdate)
            {
                dtRecondate = dtRecondate.AddDays(30);
                lblreconduedate.Text = dtRecondate.ToShortDateString();
            }
        }
        IList<AriesClearingBE> lstarieslist = AriesclearingBS.GetAriesDetails(AISMasterEntities.AdjusmentNumber, AISMasterEntities.AccountNumber);
        if (lstarieslist.Count > 0)
        {
            ViewState["AriesClrngId"] = lstarieslist[0].PREMIUM_ADJUST_CLEARING_ID;
            ViewState["Premadjustmentid"] = lstarieslist[0].PREMIUM_ADJUSTMENT_ID;
            CheckNew = false;

            if (lstarieslist[0].ARIES_POST_DATE != null)
                this.txtPostingdate.Text = lstarieslist[0].ARIES_POST_DATE.Value.ToShortDateString();

            this.txtCheck.Text = lstarieslist[0].CHECK_NUMBER_TEXT;

            if (lstarieslist[0].ARIES_PAYMENT_AMOUNT != null)
                this.txtAmount.Text = decimal.Parse(lstarieslist[0].ARIES_PAYMENT_AMOUNT.ToString()).ToString("#,##0");

            if (lstarieslist[0].BILLED_ITEM_CLEAR_DATE != null)
                this.txtBilleddate.Text = lstarieslist[0].BILLED_ITEM_CLEAR_DATE.Value.ToShortDateString();

            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            {
                if (lstarieslist[0].ARIES_COMPL_IND == true)
                    this.btnfinalise.Enabled = false;
            }
            else
            {
                this.btnfinalise.Enabled = true;
            }
        }

    }
    /// <summary>
    /// This method is used to bind the Aries details.
    /// </summary>
    public void BindAriesDetails()
    {
        DateTime dtRecondate;
        if (AISMasterEntities != null)
        {

            dtRecondate = DateTime.Parse(AISMasterEntities.InvoiceDate.ToString());
            bool validdate = IsValidDate(dtRecondate);
            if (validdate)
            {
                dtRecondate = dtRecondate.AddDays(30);
                lblreconduedate.Text = dtRecondate.ToShortDateString();
            }
        }
        IList<AriesClearingBE> lstarieslist = AriesclearingBS.GetAriesDetails(AISMasterEntities.AdjusmentNumber, AISMasterEntities.AccountNumber);
        if (lstarieslist.Count > 0)
        {
            if (lstarieslist[0].RECON_DATE != null)
                this.lstAriesissues.Enabled = true;
            ViewState["AriesClrngId"] = lstarieslist[0].PREMIUM_ADJUST_CLEARING_ID;
            ViewState["Premadjustmentid"] = lstarieslist[0].PREMIUM_ADJUSTMENT_ID;
            CheckNew = false;

            if (lstarieslist[0].RECON_DATE != null)

                this.txtRecondate.Text = lstarieslist[0].RECON_DATE.Value.ToShortDateString();
            if (lstarieslist[0].QUALITY_CONTROL_DATE != null)
                this.txtAriesQCdate.Text = lstarieslist[0].QUALITY_CONTROL_DATE.Value.ToShortDateString();

            this.txtComments.Text = lstarieslist[0].COMMENTS_TEXT;


        }
        else
        {
            ViewState["AriesClrngId"] = 0;
        }

        CustomerContactBS CustomercontactsBS = new CustomerContactBS();
        IList<CustomerContactBE> lstAccountresponsibilities = CustomercontactsBS.getAccountResponsibilities(AISMasterEntities.AccountNumber);
        if (lstAccountresponsibilities.Count > 0)
        {
            for (int icount = 0; icount < lstAccountresponsibilities.Count; icount++)
            {

                if (lstAccountresponsibilities[icount].RESP_NAME == "ARiES QC")
                {   //06/23 for veracode
                    //this.lblQCby.Text = lstAccountresponsibilities[icount].FULLNAME;
                    //this.lblQCby.Text = string.IsNullOrEmpty(lstAccountresponsibilities[icount].FULLNAME.Trim()) ? "" : HttpUtility.HtmlDecode(HttpUtility.HtmlEncode(lstAccountresponsibilities[icount].FULLNAME));
                    this.lblQCby.Text = HttpUtility.HtmlEncode(Convert.ToString(lstAccountresponsibilities[icount].FULLNAME));
                }
                if (lstAccountresponsibilities[icount].RESP_NAME == "RECONCILER")
                {   //06/23 for veracode
                    //this.lblReconrep.Text = lstAccountresponsibilities[icount].FULLNAME;
                    //this.lblReconrep.Text = string.IsNullOrEmpty(lstAccountresponsibilities[icount].FULLNAME.Trim()) ? "" : HttpUtility.HtmlDecode(HttpUtility.HtmlEncode(lstAccountresponsibilities[icount].FULLNAME)); // EAISA - 7  fix for veracode 06142018
                    this.lblReconrep.Text = HttpUtility.HtmlEncode(Convert.ToString(lstAccountresponsibilities[icount].FULLNAME));
                }

            }
        }
    }

    /// <summary>
    /// This function checks for the validation of dates.
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private bool IsValidDate(DateTime dt)
    {
        bool retValue = true;
        if (dt.Day == 1 && dt.Month == 1 && (dt.Year == 1 || dt.Year == 1000))
            retValue = false;
        return retValue;
    }


    /// <summary>
    /// THis method populates the Issues.
    /// </summary>
    public void BindIssueslist()
    {
        IList<LookupBE> lookups;

        lookups = (List<LookupBE>)Application["LookUpData"];
        lookups = lookups.Where(lk => lk.LookUpName == "ARIES Clearing QC Issue").ToList();

        lstAriesissues.DataSource = QltyCntrlListService.getArieschecklist(lookups[0].LookUpID, Convert.ToInt32(ViewState["AriesClrngId"]), AISMasterEntities.AccountNumber);
        lstAriesissues.DataBind();
        if (lstAriesissues.InsertItemPosition != InsertItemPosition.None)
        {
            DropDownList ddl = (DropDownList)lstAriesissues.InsertItem.FindControl("ddlIssue");
            QCMasterIssueListBS issues = new QCMasterIssueListBS();
            ddl.DataSource = issues.getIssuesList(lookups[0].LookUpID);
            ddl.DataValueField = "QualityCntrlMstrIsslstID";
            ddl.DataTextField = "IssueText";
            ddl.DataBind();

            ListItem li = new ListItem("Select", "0");
            ddl.Items.Insert(0, li);
        }

    }
    protected void lnkIssues_Click(object sender, EventArgs e)
    {

        this.tcAriesClearing.ActiveTabIndex = 1;
    }
    protected void SaveQualityList(ListViewItem e)
    {
        IList<AriesClearingBE> lstarieslist = AriesclearingBS.GetAriesDetails(AISMasterEntities.AdjusmentNumber, AISMasterEntities.AccountNumber);
        if (lstarieslist.Count > 0)
        {
            ViewState["AriesClrngId"] = lstarieslist[0].PREMIUM_ADJUST_CLEARING_ID;
        }
        QltyCntrlListBE = new Qtly_Cntrl_ChklistBE();

        QltyCntrlListBE.CHECKLISTITEM_ID = Convert.ToInt32(((DropDownList)e.FindControl("ddlIssue")).SelectedItem.Value);
        QltyCntrlListBE.CUSTOMER_ID = AISMasterEntities.AccountNumber;

        QltyCntrlListBE.PREMIUMADJ_ARIES_CLR_ID = Convert.ToInt32(ViewState["AriesClrngId"].ToString());
        QltyCntrlListBE.ACTIVE = true;
        QltyCntrlListBE.CreatedUser_ID = CurrentAISUser.PersonID;
        QltyCntrlListBE.CreatedDate = DateTime.Now;
        bool Result = QltyCntrlListService.IsExistsAriesIssue(QltyCntrlListBE.PREMIUMADJ_ARIES_CLR_ID, QltyCntrlListBE.CHECKLISTITEM_ID, QltyCntrlListBE.CUSTOMER_ID, QltyCntrlListBE.QualityControlChklst_ID);
        if (!Result)
        {

            bool Flag = QltyCntrlListService.Update(QltyCntrlListBE);
            //Function call to bind lstReviewDetails
            BindIssueslist();
        }
        else
        {
            ShowMessage("The record cannot be saved. An identical record already exists.");
        }
    }
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
            DisableRow(Convert.ToInt32(e.CommandArgument), e.CommandName == "DISABLE" ? false : true);
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
    /// <summary>
    /// This method is used to disable the row.
    /// </summary>
    /// <param name="intQltyID"></param>
    /// <param name="Flag"></param>
    protected void DisableRow(int intQltyID, bool Flag)
    {
        try
        {
            QltyCntrlListBE = QltyCntrlListService.getQualityControlRow(intQltyID);
            QltyCntrlListBE.ACTIVE = Flag;
            Flag = QltyCntrlListService.Update(QltyCntrlListBE);
            if (Flag)
            {
                //Function call to bind lstReviewDetails
                BindIssueslist();
            }

        }
        catch (RetroBaseException exception)
        {
            ShowError(exception.Message, exception);
        }
    }
    /// <summary>
    /// This method is used to save the Payment details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Paymentsave(object sender, EventArgs e)
    {
        IList<AriesClearingBE> lstarieslist = AriesclearingBS.GetAriesDetails(AISMasterEntities.AdjusmentNumber, AISMasterEntities.AccountNumber);
        if (lstarieslist.Count > 0)
        {
            CheckNew = false;
            ViewState["AriesClrngId"] = lstarieslist[0].PREMIUM_ADJUST_CLEARING_ID;
        }
        bool auditneeded = false;
        if (CheckNew == false)
        {
            if ((this.txtPostingdate.Text == string.Empty) && (this.txtBilleddate.Text == string.Empty) && (this.txtAmount.Text == string.Empty) && (this.txtCheck.Text == string.Empty))
            {
                ShowMessage("Please enter data in order to save");

            }
            else
            {
                AriesClearingBE ariesBE = new AriesClearingBE();
                if (Convert.ToInt32(ViewState["AriesClrngId"].ToString()) > 0)
                {
                    ariesBE = AriesclearingBS.GetAriesDetails(Convert.ToInt32(ViewState["AriesClrngId"].ToString()));

                    //Assign Entity...
                    auditneeded = CheckEntity(ariesBE, "Payment");
                    if (CurrentAISUser.PersonID <= 0)
                    {
                        ShowError("UserID/AZCorpID Not Registered. Please Register using Internal Contacts.");
                        return;
                    }
                    ariesBE.QULAITY_PERSON_ID = AISMasterEntities.PersonIdAries; //CurrentAISUser.PersonID;

                    if (this.txtPostingdate.Text != String.Empty)
                        ariesBE.ARIES_POST_DATE = DateTime.Parse(this.txtPostingdate.Text);
                    else
                        ariesBE.ARIES_POST_DATE = null;

                    ariesBE.CHECK_NUMBER_TEXT = this.txtCheck.Text;
                    if (this.txtAmount.Text != String.Empty)
                        ariesBE.ARIES_PAYMENT_AMOUNT = Convert.ToDecimal(this.txtAmount.Text);
                    else
                        ariesBE.ARIES_PAYMENT_AMOUNT = null;
                    if (this.txtBilleddate.Text != String.Empty)
                        ariesBE.BILLED_ITEM_CLEAR_DATE = DateTime.Parse(this.txtBilleddate.Text);
                    else
                        ariesBE.BILLED_ITEM_CLEAR_DATE = null;
                    AriesclearingBS.save(ariesBE);
                    if (auditneeded)
                    {
                        ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();

                        audtBS.Save(AISMasterEntities.AccountNumber, Convert.ToInt32(AISMasterEntities.PremiumAdjProgramID), GlobalConstants.AuditingWebPage.ARIESClearing, CurrentAISUser.PersonID);

                        AriesTransactionWrapper.SubmitTransactionChanges();
                    }

                }
            }
        }
        else
        {

            if ((this.txtPostingdate.Text == string.Empty) && (this.txtBilleddate.Text == string.Empty) && (this.txtAmount.Text == string.Empty) && (this.txtCheck.Text == string.Empty))
            {
                ShowMessage("Please enter data in order to save");

            }
            else
            {
                AriesClearingBE ariesclrngBE = new AriesClearingBE();
                ariesclrngBE.QULAITY_PERSON_ID = AISMasterEntities.PersonIdAries; //CurrentAISUser.PersonID;
                ariesclrngBE.PREMIUM_ADJUSTMENT_ID = AISMasterEntities.AdjusmentNumber;
                ariesclrngBE.CUSTOMER_ID = AISMasterEntities.AccountNumber;
                ariesclrngBE.UPDATED_USR_ID = CurrentAISUser.PersonID;
                ariesclrngBE.CREATED_DATE = DateTime.Now;
                if (CurrentAISUser.PersonID <= 0)
                {
                    ShowError("UserID/AZCorpID Not Registered. Please Register using Internal Contacts.");
                    return;
                }
                if (this.txtPostingdate.Text != String.Empty)
                    ariesclrngBE.ARIES_POST_DATE = Convert.ToDateTime(this.txtPostingdate.Text);
                if (this.txtCheck.Text != string.Empty)
                    ariesclrngBE.CHECK_NUMBER_TEXT = this.txtCheck.Text;
                if (this.txtAmount.Text != String.Empty)
                    ariesclrngBE.ARIES_PAYMENT_AMOUNT = Convert.ToDecimal(this.txtAmount.Text);
                if (this.txtBilleddate.Text != String.Empty)
                    ariesclrngBE.BILLED_ITEM_CLEAR_DATE = Convert.ToDateTime(this.txtBilleddate.Text);

                AriesclearingBS.save(ariesclrngBE);

                AriesTransactionWrapper.SubmitTransactionChanges();


            }
        }
    }
    protected void btnDetailssave_Click(object sender, EventArgs e)
    {
        IList<AriesClearingBE> lstarieslist = AriesclearingBS.GetAriesDetails(AISMasterEntities.AdjusmentNumber, AISMasterEntities.AccountNumber);
        if (lstarieslist.Count > 0)
        {
            CheckNew = false;
            ViewState["AriesClrngId"] = lstarieslist[0].PREMIUM_ADJUST_CLEARING_ID;
        }
        bool auditneeded = false;
        if (CurrentAISUser.PersonID <= 0)
        {
            ShowError("UserID/AZCorpID Not Registered. Please Register using Internal Contacts.");
            return;
        }
        if (CheckNew == false)
        {
            // IList<AriesClearingBE> ariesBE = new List<AriesClearingBE>();
            // ariesBE = AriesclearingBS.GetAriesDetails(Convert.ToInt32(ViewState["Premadjustmentid"].ToString()), AISMasterEntities.AccountNumber);
            AriesClearingBE ariesBE = new AriesClearingBE();

            ariesBE = AriesclearingBS.GetAriesDetails(Convert.ToInt32(ViewState["AriesClrngId"].ToString()));
            this.lstAriesissues.Enabled = true;
            //Assign Entity...
            auditneeded = CheckEntity(ariesBE, "Details");

            ariesBE.QULAITY_PERSON_ID = AISMasterEntities.PersonIdAries;// CurrentAISUser.PersonID;
            if (this.txtAriesQCdate.Text != string.Empty)
                ariesBE.QUALITY_CONTROL_DATE = Convert.ToDateTime(this.txtAriesQCdate.Text);
            else
                ariesBE.QUALITY_CONTROL_DATE = null;

            ariesBE.COMMENTS_TEXT = this.txtComments.Text;
            ariesBE.UPDATED_DATE = DateTime.Now;
            if (this.txtRecondate.Text != String.Empty)
                ariesBE.RECON_DATE = DateTime.Parse(this.txtRecondate.Text);
            else
                ariesBE.RECON_DATE = null;

            bool flag = AriesclearingBS.save(ariesBE);
            ShowConcurrentConflict(flag, ariesBE.ErrorMessage);
            if (auditneeded)
            {
                ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();

                audtBS.Save(AISMasterEntities.AccountNumber, Convert.ToInt32(AISMasterEntities.PremiumAdjProgramID), GlobalConstants.AuditingWebPage.ARIESClearing, CurrentAISUser.PersonID);

                AriesTransactionWrapper.SubmitTransactionChanges();
            }
            if (txtBilleddate.Text.Length > 0 && txtAmount.Text.Length > 0)
            {
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                    btnfinalise.Enabled = true;

            }
        }
        else
        {
            if (CurrentAISUser.PersonID <= 0)
            {
                ShowError("UserID/AZCorpID Not Registered. Please Register using Internal Contacts.");
                return;
            }
            AriesClearingBE ariesclrngBE = new AriesClearingBE();

            ariesclrngBE.QULAITY_PERSON_ID = AISMasterEntities.PersonIdAries; //CurrentAISUser.PersonID;
            ariesclrngBE.PREMIUM_ADJUSTMENT_ID = AISMasterEntities.AdjusmentNumber;
            ariesclrngBE.CUSTOMER_ID = AISMasterEntities.AccountNumber;
            if (this.txtAriesQCdate.Text != string.Empty)
                ariesclrngBE.QUALITY_CONTROL_DATE = Convert.ToDateTime(this.txtAriesQCdate.Text);
            ariesclrngBE.COMMENTS_TEXT = this.txtComments.Text;
            ariesclrngBE.CREATED_DATE = DateTime.Now;
            if (this.txtRecondate.Text != String.Empty)
                ariesclrngBE.RECON_DATE = Convert.ToDateTime(this.txtRecondate.Text);
            if (this.txtPostingdate.Text != String.Empty)
                ariesclrngBE.ARIES_POST_DATE = Convert.ToDateTime(this.txtPostingdate.Text);
            if (this.txtCheck.Text != string.Empty)
                ariesclrngBE.CHECK_NUMBER_TEXT = this.txtCheck.Text;
            if (this.txtAmount.Text != String.Empty)
                ariesclrngBE.ARIES_PAYMENT_AMOUNT = Convert.ToDecimal(this.txtAmount.Text);
            if (this.txtBilleddate.Text != String.Empty)
                ariesclrngBE.BILLED_ITEM_CLEAR_DATE = Convert.ToDateTime(this.txtBilleddate.Text);
            AriesclearingBS.save(ariesclrngBE);
            //  ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
            // audtBS.Save(AISMasterEntities.AccountNumber, AISMasterEntities.AdjusmentNumber,GlobalConstants.AuditingWebPage.ARIESClearing, CurrentAISUser.PersonID);
            this.lstAriesissues.Enabled = true;
            AriesTransactionWrapper.SubmitTransactionChanges();
            if (txtBilleddate.Text.Length > 0 && txtAmount.Text.Length > 0)
            {
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                    btnfinalise.Enabled = true;

            }

        }
    }

    /// <summary>
    /// This method is used to check whether audit is required or not.
    /// </summary>
    /// <param name="ariesBE"></param>
    /// <param name="strtabname"></param>
    /// <returns></returns>
    public bool CheckEntity(AriesClearingBE ariesBE, String strtabname)
    {
        bool isAuditNeeded = false;

        if (strtabname == "Details")
        {
            isAuditNeeded = (ariesBE.RECON_DATE != Convert.ToDateTime(this.txtRecondate.Text)) ? true : isAuditNeeded;

            if ((ariesBE.QUALITY_CONTROL_DATE == null) && (this.txtAriesQCdate.Text != String.Empty))

                isAuditNeeded = true;

            if ((ariesBE.QUALITY_CONTROL_DATE != null) && (this.txtAriesQCdate.Text == String.Empty))

                isAuditNeeded = true;



            if ((ariesBE.QUALITY_CONTROL_DATE != null) && (this.txtAriesQCdate.Text != String.Empty))
                isAuditNeeded = (ariesBE.QUALITY_CONTROL_DATE != Convert.ToDateTime(this.txtAriesQCdate.Text)) ? true : isAuditNeeded;

            isAuditNeeded = (ariesBE.COMMENTS_TEXT != this.txtComments.Text) ? true : isAuditNeeded;
            if ((ariesBE.ARIES_POST_DATE == null) && (this.txtPostingdate.Text != String.Empty))

                isAuditNeeded = true;

            if ((ariesBE.ARIES_POST_DATE != null) && (this.txtPostingdate.Text == String.Empty))

                isAuditNeeded = true;



            if ((ariesBE.ARIES_POST_DATE != null) && (this.txtPostingdate.Text != String.Empty))
                isAuditNeeded = (ariesBE.ARIES_POST_DATE != Convert.ToDateTime(this.txtPostingdate.Text)) ? true : isAuditNeeded;
        }
        if (strtabname == "Payment")
        {

            isAuditNeeded = (ariesBE.CHECK_NUMBER_TEXT != this.txtCheck.Text) ? true : isAuditNeeded;


            if ((ariesBE.ARIES_PAYMENT_AMOUNT == null) && (this.txtAmount.Text != String.Empty))

                isAuditNeeded = true;

            if ((ariesBE.ARIES_PAYMENT_AMOUNT != null) && (this.txtAmount.Text == String.Empty))

                isAuditNeeded = true;



            if ((ariesBE.ARIES_PAYMENT_AMOUNT != null) && (this.txtAmount.Text != String.Empty))
                isAuditNeeded = (ariesBE.ARIES_PAYMENT_AMOUNT != Convert.ToDecimal(this.txtAmount.Text)) ? true : isAuditNeeded;



            if ((ariesBE.BILLED_ITEM_CLEAR_DATE == null) && (this.txtBilleddate.Text != String.Empty))

                isAuditNeeded = true;

            if ((ariesBE.BILLED_ITEM_CLEAR_DATE != null) && (this.txtBilleddate.Text == String.Empty))

                isAuditNeeded = true;



            if ((ariesBE.BILLED_ITEM_CLEAR_DATE != null) && (this.txtBilleddate.Text != String.Empty))
                isAuditNeeded = (ariesBE.BILLED_ITEM_CLEAR_DATE != Convert.ToDateTime(this.txtBilleddate.Text)) ? true : isAuditNeeded;
        }


        if (strtabname == "finalise")
        {

            if ((ariesBE.ARIES_PAYMENT_AMOUNT != null) && (this.txtAmount.Text != String.Empty))
                isAuditNeeded = (ariesBE.ARIES_PAYMENT_AMOUNT != Convert.ToDecimal(this.txtAmount.Text)) ? true : isAuditNeeded;



            if ((ariesBE.BILLED_ITEM_CLEAR_DATE == null) && (this.txtBilleddate.Text != String.Empty))

                isAuditNeeded = true;

            if ((ariesBE.BILLED_ITEM_CLEAR_DATE != null) && (this.txtBilleddate.Text == String.Empty))

                isAuditNeeded = true;



            if ((ariesBE.BILLED_ITEM_CLEAR_DATE != null) && (this.txtBilleddate.Text != String.Empty))
                isAuditNeeded = (ariesBE.BILLED_ITEM_CLEAR_DATE != Convert.ToDateTime(this.txtBilleddate.Text)) ? true : isAuditNeeded;

        }
        return isAuditNeeded;


    }
    /// <summary>
    /// This method is used to finalise.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void finalise(object sender, EventArgs e)
    {
        bool auditneed = false;
        AriesClearingBE arisBE = new AriesClearingBE();
        BindAriesDetails();
        if ((Convert.ToInt32(ViewState["AriesClrngId"].ToString())) > 0)
        {
            arisBE = AriesclearingBS.GetAriesDetails(Convert.ToInt32(ViewState["AriesClrngId"].ToString()));
            auditneed = CheckEntity(arisBE, "finalise");
            if (CurrentAISUser.PersonID <= 0)
            {
                ShowError("UserID/AZCorpID Not Registered. Please Register using Internal Contacts.");
                return;
            }
            if (arisBE != null)
            {
                if (this.txtPostingdate.Text != String.Empty)
                    arisBE.ARIES_POST_DATE = Convert.ToDateTime(this.txtPostingdate.Text);
                else arisBE.ARIES_POST_DATE = null;

                if (this.txtCheck.Text != string.Empty)
                    arisBE.CHECK_NUMBER_TEXT = this.txtCheck.Text;
                else arisBE.CHECK_NUMBER_TEXT = null;
                if (this.txtAmount.Text != string.Empty)
                    arisBE.ARIES_PAYMENT_AMOUNT = Convert.ToDecimal(this.txtAmount.Text);
                else arisBE.ARIES_PAYMENT_AMOUNT = null;
                if (this.txtBilleddate.Text != String.Empty)
                    arisBE.BILLED_ITEM_CLEAR_DATE = Convert.ToDateTime(this.txtBilleddate.Text);
                else { arisBE.BILLED_ITEM_CLEAR_DATE = null; }

                arisBE.CREATED_USER_ID = CurrentAISUser.PersonID;
                arisBE.ARIES_COMPL_IND = true;

                bool flag = AriesclearingBS.save(arisBE);
                this.btnfinalise.Enabled = false;
                ShowConcurrentConflict(flag, arisBE.ErrorMessage);
                // this.divAriesclring.Disabled = true;
                if (auditneed)
                {
                    ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();

                    audtBS.Save(AISMasterEntities.AccountNumber, Convert.ToInt32(AISMasterEntities.PremiumAdjProgramID), GlobalConstants.AuditingWebPage.ARIESClearing, CurrentAISUser.PersonID);

                    AriesTransactionWrapper.SubmitTransactionChanges();
                }


            }
        }
        else
        {
            arisBE.QULAITY_PERSON_ID = CurrentAISUser.PersonID;
            arisBE.PREMIUM_ADJUSTMENT_ID = AISMasterEntities.AdjusmentNumber;
            arisBE.CUSTOMER_ID = AISMasterEntities.AccountNumber;
            if (this.txtPostingdate.Text != String.Empty)
                arisBE.ARIES_POST_DATE = Convert.ToDateTime(this.txtPostingdate.Text);
            else arisBE.ARIES_POST_DATE = null;

            if (this.txtCheck.Text != string.Empty)
                arisBE.CHECK_NUMBER_TEXT = this.txtCheck.Text;
            else arisBE.CHECK_NUMBER_TEXT = null;

            if (this.txtAmount.Text != String.Empty)
                arisBE.ARIES_PAYMENT_AMOUNT = Convert.ToDecimal(this.txtAmount.Text);
            else arisBE.ARIES_PAYMENT_AMOUNT = null;
            if (this.txtBilleddate.Text != String.Empty)
                arisBE.BILLED_ITEM_CLEAR_DATE = Convert.ToDateTime(this.txtBilleddate.Text);
            else { arisBE.BILLED_ITEM_CLEAR_DATE = null; }
            arisBE.ARIES_COMPL_IND = true;
            AriesclearingBS.save(arisBE);
            this.btnfinalise.Enabled = false;

        }
    }
}
