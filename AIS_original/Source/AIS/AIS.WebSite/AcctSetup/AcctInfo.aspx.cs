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
using AjaxControlToolkit;

public partial class AcctInfo : AISBasePage
{
    protected AISBusinessTransaction AccountTransactionWrapper
    {
        get
        {
            //if ((AISBusinessTransaction)Session["AccountTransaction"] == null)
            //    Session["AccountTransaction"] = new AISBusinessTransaction();

            //return (AISBusinessTransaction)Session["AccountTransaction"];
            if ((AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("AccountTransaction") == null)
                SaveObjectToSessionUsingWindowName("AccountTransaction", new AISBusinessTransaction());

            return (AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("AccountTransaction");
        }
        set 
        { 
            //Session["AccountTransaction"] = value; 
            SaveObjectToSessionUsingWindowName("AccountTransaction", value);
        }
    }

    private IList<LSICustomerBE> LSICustmerList
    {
        get 
        {
            //return (IList<LSICustomerBE>)Session["AcctInfo-LSICustmerList"]; 
            return (IList<LSICustomerBE>)RetrieveObjectFromSessionUsingWindowName("AcctInfo-LSICustmerList"); 
        }
        set 
        {
            //Session["AcctInfo-LSICustmerList"] = value; 
            SaveObjectToSessionUsingWindowName("AcctInfo-LSICustmerList", value);
        }
    }
    private IList<AccountBE> AccountList
    {
        get 
        { 
            //return (IList<AccountBE>)Session["AcctInfo-AccountList"]; 
            return (IList<AccountBE>)RetrieveObjectFromSessionUsingWindowName("AcctInfo-AccountList"); 
        }
        set 
        { 
            //Session["AcctInfo-AccountList"] = value; 
            SaveObjectToSessionUsingWindowName("AcctInfo-AccountList", value);
        }
    }

    private AccountBS accountService;
    private CustomerContactBS custContactService;
    private LSICustomersBS LSICustomersService;

    /// <summary>
    /// A Property for Holding LSI CustomerID in ViewState
    /// </summary>
    protected int LSIAccountID
    {
        get
        {
            if (ViewState["LSIAccountID"] != null)
            {
                return int.Parse(ViewState["LSIAccountID"].ToString());
            }
            else
            {
                ViewState["LSIAccountID"] = 0;
                return 0;
            }
        }
        set
        {
            ViewState["LSIAccountID"] = value;
        }
    }

    // Global variable to hold the Account
    private String strCurrentAccount = "";

    private CustomerContactBS CustContactService
    {
        get
        {
            if (custContactService == null)
            {
                custContactService = new CustomerContactBS();
            }
            return custContactService;
        }
    }

    private AccountBS AccountService
    {
        get
        {
            if (accountService == null)
            {
                accountService = new AccountBS();
            }
            return accountService;
        }
    }

    private LSICustomersBS AllLSICustService
    {
        get
        {
            if (LSICustomersService == null)
            {
                LSICustomersService = new LSICustomersBS();
            }
            return LSICustomersService;
        }
    }

    private AccountBE accountBE
    {
        get 
        { 
            //return (AccountBE)Session["AccountInfo-accountBE"]; 
            return (AccountBE)RetrieveObjectFromSessionUsingWindowName("AccountInfo-accountBE"); 
        }
        set 
        { 
            //Session["AccountInfo-accountBE"] = value; 
            SaveObjectToSessionUsingWindowName("AccountInfo-accountBE", value);
        }

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        btnSaveCancel.OperationsButtonClicked += new EventHandler(btnSaveCancel_OperationsButtonClicked);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //User Control Event Handlers
       
        if (!Page.IsPostBack)
        {
            //Canada to Zurich AIS Application Enhancements  
            ddlCompany.SelectedValue = "733";
            ddlCurrency.SelectedValue = "735";
            if (Request.QueryString["Mode"] == "Add")
            {
                accountBE = new AccountBE();
                AISMasterEntities = new MasterEntities();
                AISMasterEntities.AccountNumber = 0;
                txtAccountName.Focus();
                TabAccountResponsibility.Enabled = false;
                TabRelatedAccountDetails.Enabled = false;
                TabRelatedLSIAccountDetails.Enabled = false;
                ddlInsContact.Enabled = false;
                hypComments.Enabled = false;
                hypEnterContacts.Enabled = false;
            }
            else
            {
                //9288 Bug
                //Button btnCancel = (Button)btnSaveCancel.FindControl("cmdCancel");
                //if (btnCancel != null)
                //    btnCancel.Enabled = false;
            }
            if (AISMasterEntities == null)
            {
                //Response.Redirect("~/acctSearch.aspx");
                ResponseRedirect("~/acctSearch.aspx");
            }
            if (CurrentAISUser.Role != GlobalConstants.ApplicationSecurityGroup.Manager && CurrentAISUser.Role != GlobalConstants.ApplicationSecurityGroup.SystemAdmin)
            {
                //pnlAccountResponsibility.Enabled = false;
            }
            this.Master.Page.Title = "Account Information";

            AccountTransactionWrapper = new AISBusinessTransaction();
            ddlBktcyBuyout.DataBind();

            if (AISMasterEntities.AccountNumber > 0)
            {
                RetrieveAccountDetail(AISMasterEntities.AccountNumber);
                if (CurrentAISUser.Role != GlobalConstants.ApplicationSecurityGroup.Manager &&
                    CurrentAISUser.Role != GlobalConstants.ApplicationSecurityGroup.SystemAdmin)
                    chkInActive.Enabled = false;
            }
            Button btnSave = (Button)btnSaveCancel.FindControl("cmdSave");
            btnSave.ValidationGroup = "SaveDate";
        }

        //Adding Flag for Alert Exit Confirmation
        CheckExitWithoutSave();
    }

    private void CheckExitWithoutSave()
    {
        ArrayList list = new ArrayList();
        list.Add(txtAccountName);
        list.Add(chkMasterAccount);
        list.Add(chkInActive);
        list.Add(txtAccountNumber);
        list.Add(chkPEO);
        list.Add(chkMDRetro);
        list.Add(ddlBktcyBuyout);
        list.Add(txtBktcyBuyoutEffDt);
        list.Add(txtTPAFundedFirstValDt);
        list.Add(CalExtBktcyBuyoutEffDt);
        list.Add(calTPAFundedFirstValDt);
        list.Add(chkTPAFunded);
        list.Add(txtSSCGID);
        list.Add(txtBPNumber);
        list.Add(ddlInsContact);
        list.Add(hypComments);
        list.Add(hypEnterContacts);
       // added for Canada to Zurich AIS Application Enhancements  
        list.Add(ddlCompany);
        list.Add(ddlCurrency);
        ProcessExitFlag(list);
    }

    /// <summary>
    /// The dropdown box Selectedindex is  set by comparing the
    /// ID to the Value of the drop down list box. If the value
    /// doesn't exist default to Zero index
    /// </summary>
    /// <param name="dropDL"></param>
    /// <param name="selID"></param>
    private void SetDropDownListSelectedIndex(DropDownList dropDL, int selID)
    {
        int selIndex = 0;
        int curID = 0;
        foreach (ListItem ddlItem in dropDL.Items)
        {
            Int32.TryParse(ddlItem.Value.ToString(), out curID);
            if (curID == selID)
            {
                break;
            }
            selIndex++;
        }
        if (selIndex < dropDL.Items.Count)
        {
            dropDL.SelectedIndex = selIndex;
        }
        else
        {
            dropDL.SelectedIndex = 0;
        }
    }

    private void RetrieveAccountDetail(int accountID)
    {
        accountBE = AccountService.getAccount(accountID);

        accountBE.PERSON_ID =
            Convert.ToInt32((new CustomerContactBS().getPrimaryContactData(accountID).PERSON_ID));
        txtAccountName.Text = accountBE.FULL_NM;
        txtAccountNumber.Text = accountBE.CUSTMR_ID.ToString();
        txtBktcyBuyoutEffDt.Text = (accountBE.BKTCYBUYOUT_DATE == null ? "" : accountBE.BKTCYBUYOUT_DATE.Value.ToString());
        txtBktcyBuyoutEffDt.Enabled = accountBE.BKTCYBUYOUT_ID != null;
        imgBktcyBuyoutEffDt.Enabled = accountBE.BKTCYBUYOUT_ID != null;
        txtTPAFundedFirstValDt.Text = (accountBE.THIRD_PARTY_ADMIN_FUNDED_DATE == null ? "" : accountBE.THIRD_PARTY_ADMIN_FUNDED_DATE.Value.ToString());
        txtTPAFundedFirstValDt.Enabled = accountBE.THIRD_PARTY_ADMIN_FUNDED_DATE != null;
        imgTPAFundedFirstValDt.Enabled = accountBE.THIRD_PARTY_ADMIN_FUNDED_DATE != null;
        txtBPNumber.Text = accountBE.FINC_PTY_ID;
        txtSSCGID.Text = accountBE.SUPRT_SERV_CUSTMR_GP_ID;
        chkInActive.Checked = accountBE.ACTV_IND == false;
        chkMDRetro.Checked = accountBE.MARYLAND_RETRO_IND == true;
        chkPEO.Checked = accountBE.PEO_IND == true;
        chkTPAFunded.Checked = accountBE.TPA_FUNDED_IND == true;
        chkMasterAccount.Checked = accountBE.MSTR_ACCT_IND == true;
        // added for Canada to Zurich AIS Application Enhancements
        ddlCompany.SelectedValue = accountBE.COMPANY_CD.ToString();
        ddlCurrency.SelectedValue = accountBE.CURRENCY_CD.ToString();

        if (accountBE.BKTCYBUYOUT_ID != null)
//            SetDropDownListSelectedIndex(ddlBktcyBuyout, accountBE.BKTCYBUYOUT_ID.Value);
            AddInActiveLookupData(ref ddlBktcyBuyout, accountBE.BKTCYBUYOUT_ID.Value);
        
        ddlInsContact.DataSource =
            (new BLAccess()).GetInsContact(Convert.ToInt32(AISMasterEntities.AccountNumber));

        ddlInsContact.DataTextField = "DISPLAYTEXT";
        ddlInsContact.DataValueField = "Person_ID";
        ddlInsContact.DataBind();

        if (accountBE.PERSON_ID > 0)
            SetDropDownListSelectedIndex(ddlInsContact, Convert.ToInt32(accountBE.PERSON_ID.ToString()));

    }

    void btnSaveCancel_OperationsButtonClicked(object sender, EventArgs e)
    {

        switch (TabConAcctinfo.ActiveTab.ID)
        {
            case "TabAccountInfo":
                int PrevPersonID = 0;
                bool actindChanged = false;

                if (btnSaveCancel.Operation.Trim().ToUpper() == "SAVE")
                {
                    CustomerContactBE customerContactBE = new CustomerContactBE();

                    if (txtAccountName.Text.Trim().Length == 0)
                    {
                        string strMessage = "Account Name is Mandatory!";
                        ShowError(strMessage);
                        validtionSummary();
                        return;
                    }

                    if (ddlBktcyBuyout.SelectedIndex > 0)
                    {
                        //accountBE.BKTCYBUYOUT_ID = Convert.ToInt32(ddlBktcyBuyout.SelectedItem.Value);
                        if (txtBktcyBuyoutEffDt.Text.Trim().Length == 0)
                        {
                            string strMessage = "Bankruptcy/Buyout Effective Date is not selected!";
                            ShowError(strMessage);
                            validtionSummary();
                            return;
                        }

                    }

                    if (chkTPAFunded.Checked)
                    {
                        if (txtTPAFundedFirstValDt.Text.Trim().Length == 0)
                        {
                            string strMessage = "TPA Funded First Valuation Date is not selected!";
                            ShowError(strMessage);
                            validtionSummary();
                            return;
                        }

                    }

                    //Validate duplicate Account Name
                    if (AccountService.CheckDuplicateAccountName(this.txtAccountName.Text.Trim()) == false && (accountBE.CUSTMR_ID <= 0))
                    {
                        string strMessage = "The record cannot be saved. An identical record already exists!";
                        ShowError(strMessage);
                        validtionSummary();
                        return;
                    }
                    //EAISA-5 Veracode flaw fix 12072017/01172018
                   // accountBE.FULL_NM = System.Web.HttpUtility.UrlPathEncode(txtAccountName.Text).Replace("%20", " ");
                   //EAISA-5 Veracode flaw fix 03232018 EAISA - 7 
                    accountBE.FULL_NM = Server.HtmlDecode(Server.HtmlEncode(txtAccountName.Text));

                    actindChanged = ((AISMasterEntities.AccountNumber != 0 && chkInActive.Checked != accountBE.ACTV_IND) ? true : false);
                    accountBE.ACTV_IND = ((chkInActive.Checked) ? false : true);
                    if (txtBPNumber.Text != "")
                        accountBE.FINC_PTY_ID = txtBPNumber.Text;
                    else
                        accountBE.FINC_PTY_ID = null;
                    accountBE.SUPRT_SERV_CUSTMR_GP_ID = txtSSCGID.Text;
                    accountBE.MARYLAND_RETRO_IND = ((chkMDRetro.Checked) ? true : false);
                    accountBE.PEO_IND = ((chkPEO.Checked) ? true : false);
                    accountBE.UPDATE_DATE = DateTime.Now;
                    accountBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
                    accountBE.CREATE_DATE = ((AISMasterEntities.AccountNumber == 0) ? DateTime.Now : accountBE.CREATE_DATE);
                    accountBE.CREATE_USER_ID = ((AISMasterEntities.AccountNumber == 0) ? CurrentAISUser.PersonID : accountBE.CREATE_USER_ID);
                    if (ddlBktcyBuyout.SelectedIndex > 0)
                    {
                        accountBE.BKTCYBUYOUT_ID = Convert.ToInt32(ddlBktcyBuyout.SelectedItem.Value);
                        if (txtBktcyBuyoutEffDt.Text.Trim().Length > 0)
                            accountBE.BKTCYBUYOUT_DATE = Convert.ToDateTime(txtBktcyBuyoutEffDt.Text);
                    }
                    else
                    {
                        accountBE.BKTCYBUYOUT_ID = null;
                        accountBE.BKTCYBUYOUT_DATE = null;
                        txtBktcyBuyoutEffDt.Text = null;
                    }

                    if (chkTPAFunded.Checked)
                    {
                        accountBE.TPA_FUNDED_IND = true;
                        if (txtTPAFundedFirstValDt.Text.Trim().Length > 0)
                            accountBE.THIRD_PARTY_ADMIN_FUNDED_DATE = Convert.ToDateTime(txtTPAFundedFirstValDt.Text);
                    }
                    else
                    {
                        accountBE.TPA_FUNDED_IND = false;
                        accountBE.THIRD_PARTY_ADMIN_FUNDED_DATE = null;
                        txtTPAFundedFirstValDt.Text = null;
                    }

                    // added for Canada to Zurich AIS Application Enhancements
                    if (ddlCompany.SelectedItem!=null)
                    {
                                             
                        accountBE.COMPANY_CD = Convert.ToInt32(ddlCompany.SelectedItem.Value);
                    }

                    if (ddlCurrency.SelectedItem!=null)
                    {
                                              
                        accountBE.CURRENCY_CD = Convert.ToInt32(ddlCurrency.SelectedItem.Value);
                    }


                    if (accountBE.CUSTMR_ID > 0)
                    {
                        if (ddlInsContact.SelectedItem != null)
                        {
                            customerContactBE.PERSON_ID = Convert.ToInt32(ddlInsContact.SelectedItem.Value);
                            PrevPersonID = accountBE.PERSON_ID;
                            accountBE.PERSON_ID = customerContactBE.PERSON_ID.Value;
                        }
                        customerContactBE.ROLE_ID = (new LookupBS()).PrimaryContact[0].LookUpID;
                        customerContactBE.CUSTOMER_ID = accountBE.CUSTMR_ID;
                    }

                    bool masterChanged = false;
                    if(accountBE.MSTR_ACCT_IND!= null)
                        masterChanged = (accountBE.MSTR_ACCT_IND.Value != chkMasterAccount.Checked);

                    accountBE.MSTR_ACCT_IND = ((chkMasterAccount.Checked) ? true : false);
                    
                    if (chkMasterAccount.Checked)
                        accountBE.CUSTMR_REL_ID = null;

                    if (!SaveEntity(accountBE, customerContactBE, PrevPersonID, masterChanged))
                    {
                        ShowConcurrentConflict(false, accountBE.ErrorMessage);
                        validtionSummary();
                    }
                    else
                    {
                        //EAISA-5 Veracode flaw fix 12072017/01172018
                        AISMasterEntities = new MasterEntities();
                       // AISMasterEntities.AccountName = System.Web.HttpUtility.UrlPathEncode(txtAccountName.Text).Replace("%20", " ");
                        //EAISA-5 Veracode flaw fix 05142018 EAISA - 7 
                        AISMasterEntities.AccountName = Server.HtmlDecode(Server.HtmlEncode(txtAccountName.Text));
                        //AISMasterEntities.AccountName.Replace("%20"," ");
                        AISMasterEntities.AccountNumber = accountBE.CUSTMR_ID;
                        AISMasterEntities.MasterAccount = Convert.ToBoolean((chkMasterAccount.Checked) ? true : false);
                        AISMasterEntities.MasterAccountNumber = 0;

                        if (AISMasterEntities.AccountNumber > 0)
                        {
                            RetrieveAccountDetail(AISMasterEntities.AccountNumber);
                            txtAccountName.Focus();
                            TabAccountResponsibility.Enabled = true;
                            TabRelatedAccountDetails.Enabled = true;
                            TabRelatedLSIAccountDetails.Enabled = true;
                            ddlInsContact.Enabled = true;
                            hypComments.Enabled = true;
                            hypEnterContacts.Enabled = true;


                            AISMasterEntities = new MasterEntities();
                            AISMasterEntities.AccountStatus = accountBE.ACTV_IND;
                            AISMasterEntities.AccountNumber = accountBE.CUSTMR_ID;
                            AISMasterEntities.AccountName = accountBE.FULL_NM;
                            AISMasterEntities.Bpnumber = accountBE.FINC_PTY_ID == null ? "" : accountBE.FINC_PTY_ID.ToString();
                            AISMasterEntities.SSCGID = accountBE.SUPRT_SERV_CUSTMR_GP_ID == null ? "" : accountBE.SUPRT_SERV_CUSTMR_GP_ID.ToString();
                            AISMasterEntities.MasterAccount = accountBE.MSTR_ACCT_IND == null ? false : accountBE.MSTR_ACCT_IND.Value;
                            // added for Canada to Zurich AIS Application Enhancements
                            AISMasterEntities.CompanyCode = accountBE.COMPANY_CD;
                            AISMasterEntities.CurrencyCode = accountBE.CURRENCY_CD;
                            
                            AISMasterEntities.Bpnumber = accountBE.FINC_PTY_ID == null ? "" : accountBE.FINC_PTY_ID.ToString();

                            AISMasterEntities.MasterAccountNumber = (accountBE.CUSTMR_REL_ID == null) ? 0 : accountBE.CUSTMR_REL_ID.Value;
                            //Session["AccountId"] = accountBE.CUSTMR_ID;
                            SaveObjectToSessionUsingWindowName("AccountId", accountBE.CUSTMR_ID);

                            //Enables Dependency Menus
                            AISMenu.EnableDependMenu(DependentMenuLevel.Account);
                            //Response.Redirect("~/AcctSetup/AcctInfo.aspx?Mode=Edit", false);
                            ResponseRedirect("~/AcctSetup/AcctInfo.aspx?Mode=Edit");

                        }


                        if (actindChanged)
                        {
                            //Code for logging into Audit Transaction Table 
                            ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                            audtBS.Save(AISMasterEntities.AccountNumber, GlobalConstants.AuditingWebPage.AccountInfo, CurrentAISUser.PersonID, "Disable");
                        }
                        else
                        {
                            //Code for logging into Audit Transaction Table 

                            ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                            audtBS.Save(AISMasterEntities.AccountNumber, GlobalConstants.AuditingWebPage.AccountInfo, CurrentAISUser.PersonID);
                        }

                    }
                }
                else if (btnSaveCancel.Operation.Trim().ToUpper() == "CANCEL")
                {
                    //commented by Naresh to fix the Bug No:9288
                    //as per this Bug No cancel should navigate back to AccountSearch Screen.
                    if (Request.QueryString["Mode"].ToString() == "Add")
                    {
                        //Response.Redirect("../AcctSearch.aspx");
                        ResponseRedirect("../AcctSearch.aspx");
                    }

                    //Response.Redirect("../AcctSearch.aspx");

                    ddlBktcyBuyout.SelectedIndex = 0;
                    ddlInsContact.SelectedIndex = -1;
                    if (AISMasterEntities.AccountNumber > 0)
                        RetrieveAccountDetail(AISMasterEntities.AccountNumber);
                }
                else
                {
                    //Response.Redirect("../default.aspx");
                    ResponseRedirect("../default.aspx");
                }
                break;
        }
    }


    private bool SaveEntity(AccountBE account, CustomerContactBE custmrContact, int personID, bool masterchanged)
    {
        bool success;
        bool NewCustmer = false;

        if (account.CUSTMR_ID == 0)
            NewCustmer = true;

        success = AccountService.Update(account, custmrContact, personID);

        //Removes child Accounts
        if (success && masterchanged)
            (new AccountBS()).SetZeroChildAccounts(account.CUSTMR_ID);

        if (NewCustmer)
        {
            AISMasterEntities.AccountStatus = account.ACTV_IND;
            AISMasterEntities.AccountNumber = account.CUSTMR_ID;
            AISMasterEntities.AccountName = account.FULL_NM;
            AISMasterEntities.Bpnumber = account.FINC_PTY_ID == null ? "" : account.FINC_PTY_ID.ToString();
            AISMasterEntities.SSCGID = account.SUPRT_SERV_CUSTMR_GP_ID == null ? "" : account.SUPRT_SERV_CUSTMR_GP_ID.ToString();
            AISMasterEntities.MasterAccount = account.MSTR_ACCT_IND == null ? false : account.MSTR_ACCT_IND.Value;
           AISMasterEntities.CompanyCode = account.COMPANY_CD;
           AISMasterEntities.CurrencyCode = account.CURRENCY_CD;


            AISMasterEntities.MasterAccountNumber = (account.CUSTMR_REL_ID == null) ? 0 : account.CUSTMR_REL_ID.Value;
            //Session["AccountId"] = account;
            SaveObjectToSessionUsingWindowName("AccountId", account);

            //Enables Dependency Menus
            AISMenu.EnableDependMenu(DependentMenuLevel.Account);
        }
        else
        {
            accountBE = account;
        }

        return success;
    }

    protected void lstRelatedAccounts_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            ////// Get a handle to the ddlAccountlist DropDownList control
            ////DropDownList ddl = (DropDownList)e.Item.FindControl("ddlAccountlist");

            ////if (ddl != null)
            ////{
            ////    ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByText(strCurrentAccount));
            ////}

            //Get a handle to the imgDisable image control
            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDisable");
            if (imgDelete != null)
            {
                imgDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to change status?');");
                if (imgDelete.ToolTip.ToString() == "NO")
                {
                    imgDelete.ImageUrl = "~/images/enabled.GIF";
                }
                else
                {
                    imgDelete.ImageUrl = "~/images/disabled.GIF";
                }
            }
            //Edit Link will be enabled when the record is Active
            LinkButton lbRelatedAccountEdit = (LinkButton)e.Item.FindControl("lbRelatedAccountEdit");
            if (lbRelatedAccountEdit != null) if (imgDelete.ToolTip.ToString() == "NO") lbRelatedAccountEdit.Enabled = false;
        }
    }

    // Invoked when the Edit Link in Related Retro Accounts Tab is clicked
    protected void lstRelatedAccounts_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        lstRelatedAccounts.EditIndex = e.NewEditIndex;
        Label lblTemp = (Label)lstRelatedAccounts.Items[e.NewEditIndex].FindControl("lblAccountName");
        strCurrentAccount = lblTemp.Text;
        BindRelatedRetroListView();
    }

    protected void ddlBktcyBuyout_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBktcyBuyout.SelectedIndex > 0)
        {
            txtBktcyBuyoutEffDt.Enabled = true;
            imgBktcyBuyoutEffDt.Enabled = true;
        }
        else
        {
            txtBktcyBuyoutEffDt.Text = null;
            txtBktcyBuyoutEffDt.Enabled = false;
            imgBktcyBuyoutEffDt.Enabled = false;
        }
    }

    public void chkTPAFunded_CheckedChanged(Object sender, EventArgs e)
    {
        if (chkTPAFunded.Checked)
        {
            txtTPAFundedFirstValDt.Enabled = true;
            imgTPAFundedFirstValDt.Enabled = true;
        }
        else
        {
            txtTPAFundedFirstValDt.Text = null;
            txtTPAFundedFirstValDt.Enabled = false;
            imgTPAFundedFirstValDt.Enabled = false;
        }

    }

    // Invoked when the Update Link is clicked
    protected void lstRelatedAccounts_ItemUpdate(Object sender, ListViewUpdateEventArgs e)
    {
        // Get the values
        ListViewItem myItem = lstRelatedAccounts.Items[lstRelatedAccounts.EditIndex];
        Label lblTemp = (Label)lstRelatedAccounts.Items[e.ItemIndex].FindControl("lblAccountName");

        CheckBox chkRetroActive = (CheckBox)myItem.FindControl("chkRetroActive");


        if (lblTemp.ToolTip != null)
        {
            AccountBE relAccountBE = new AccountBE();
            relAccountBE = AccountService.getAccount(Convert.ToInt32(lblTemp.ToolTip));
            AccountBE oldAccountBE = AccountList.Where(al => al.CUSTMR_ID == relAccountBE.CUSTMR_ID).ToList()[0];
            //Displays Concurrent Conflict Message
           bool con= ShowConcurrentConflict(Convert.ToDateTime(oldAccountBE.UPDATE_DATE), Convert.ToDateTime(relAccountBE.UPDATE_DATE));
           if (!con)
               return;
            relAccountBE.UPDATE_DATE = DateTime.Now;
            relAccountBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
            relAccountBE.CUSTMR_REL_ACTV_IND = ((chkRetroActive.Checked) ? true : false);
            bool Flag=AccountService.Update(relAccountBE);
        }

        // get out of the edit mode
        lstRelatedAccounts.EditIndex = -1;

        //Code for logging into Audit Transaction Table 
        ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
        audtBS.Save(AISMasterEntities.AccountNumber, GlobalConstants.AuditingWebPage.AccountInfoRelatedRetro, CurrentAISUser.PersonID);

        BindRelatedRetroListView();
    }

    protected void lstRelatedAccounts_ItemInserting(Object sender, ListViewInsertEventArgs e)
    {
        lstRelatedAccounts.InsertItemPosition = InsertItemPosition.None;
    }

    protected void lstRelatedAccounts_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SAVE")
        {
            lstRelatedAccounts_Saving(e.Item);
        }
        else if (e.CommandName.ToUpper() == "DISABLE")
        {
            EnableDisableRow(e.Item, Convert.ToInt32(e.CommandArgument), "RelatedAccounts");
        }
    }

    protected void EnableDisableRow(ListViewItem e, int KeyField, string listType)
    {
        if (listType == "RelatedAccounts")
        {
            ImageButton imgDelete = (ImageButton)e.FindControl("imgDisable");
            accountBE = AccountService.getAccount(Convert.ToInt32(KeyField));
            //Concurrency
            AccountBE oldAccountBE = AccountList.Where(al => al.CUSTMR_ID == accountBE.CUSTMR_ID).ToList()[0];
            //Displays Concurrent Conflict Message
            bool con = ShowConcurrentConflict(Convert.ToDateTime(oldAccountBE.UPDATE_DATE), Convert.ToDateTime(accountBE.UPDATE_DATE));
            if (!con)
                return;
            //End
            accountBE.CUSTMR_REL_ACTV_IND = ((imgDelete.ToolTip == "YES") ? false : true);
            accountBE.UPDATE_DATE = DateTime.Now;
            accountBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
            bool Flag=AccountService.Update(accountBE);
            ShowConcurrentConflict(Flag, accountBE.ErrorMessage);
            //Code for logging into Audit Transaction Table 
            ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
            audtBS.Save(AISMasterEntities.AccountNumber, GlobalConstants.AuditingWebPage.AccountInfoRelatedRetro, CurrentAISUser.PersonID);

            BindRelatedRetroListView();
        }
        else if (listType == "RelatedLSIAccounts")
        {
            LSICustomerBE lsiCustomerBE = new LSICustomerBE();
            ImageButton imgDelete = (ImageButton)e.FindControl("imgDisable");
            lsiCustomerBE = AllLSICustService.getLSIAccount(Convert.ToInt32(KeyField));
            int custmrId = Convert.ToInt32(KeyField);
            this.LSIAccountID = custmrId;
            LSICustomerBE lsiCustomerOld = LSICustmerList.Where(lcl => lcl.LSI_CUSTMR_ID == custmrId).ToList().First();
            if (imgDelete.ToolTip == "NO" && lsiCustomerOld.PRIM_IND==true)
            {
                if (AllLSICustService.CheckActiveLSIPrimaryAccount(Convert.ToInt32(AISMasterEntities.AccountNumber)) == true)
                {
                    modalLSIPrimaryAccount.Show();
                    return;
                }
            }        
                      
            bool con = ShowConcurrentConflict(Convert.ToDateTime(lsiCustomerOld.UPDATE_DATE), Convert.ToDateTime(lsiCustomerBE.UPDATE_DATE));
            if (!con)
                return;
            lsiCustomerBE.ACTV_IND = ((imgDelete.ToolTip == "YES") ? false : true);
            lsiCustomerBE.UPDATE_DATE = DateTime.Now;
            lsiCustomerBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
           bool Flag= AllLSICustService.Update(lsiCustomerBE);
           ShowConcurrentConflict(Flag, lsiCustomerBE.ErrorMessage);
            //Code for logging into Audit Transaction Table 
            ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
            audtBS.Save(AISMasterEntities.AccountNumber, GlobalConstants.AuditingWebPage.AccountInfoRelatedLSI, CurrentAISUser.PersonID);

            BindRelatedLSIListView();
        }
    }

    protected void lstRelatedAccounts_Saving(ListViewItem e)
    {
        App_Shared_UserDropdown ddlAccountlist = (App_Shared_UserDropdown)e.FindControl("ddlAccountlist");

        if (ddlAccountlist.SelectedValue.Length == 0)
        {
            string strMessage = "Please select an account before saving!";
            ShowError(strMessage);
            validtionSummary();
            return;
        }

        if (ddlAccountlist.SelectedValue != null)
        {
            AccountBE relAccountBE = new AccountBE();
            relAccountBE = AccountService.getAccount(Convert.ToInt32(ddlAccountlist.SelectedValue));
            relAccountBE.CUSTMR_REL_ID = AISMasterEntities.AccountNumber;
            relAccountBE.CUSTMR_REL_ACTV_IND = true;
            AccountService.Update(relAccountBE);
        }

        // bind the listview
        BindRelatedRetroListView();
    }

    // Invoked when the Cancel Link is clicked
    protected void lstRelatedAccounts_ItemCancel(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstRelatedAccounts.EditIndex = -1;
            BindRelatedRetroListView();
        }
        else if (e.CancelMode == ListViewCancelMode.CancelingInsert)
        {
            CancelUpdateMode(); //Back to normal mode.
        }
    }

    protected void CancelUpdateMode()
    {
        lstRelatedAccounts.InsertItemPosition = InsertItemPosition.None;
        BindRelatedRetroListView();
    }

    protected void lstRelatedLSIAccounts_Saving(ListViewItem e)
    {
        LSICustomerBE lsiCustomerBE = new LSICustomerBE();
        CheckBox chkLSIActive = (CheckBox)e.FindControl("chkLSIActive");
        Label lblLSICustmrID = (Label)e.FindControl("lblLSICustmrID");
        CheckBox chkLSIPrimary = (CheckBox)e.FindControl("chkLSIPrimary");
        CheckBox chkPLB = (CheckBox)e.FindControl("chkPLB");
        CheckBox chkCHF = (CheckBox)e.FindControl("chkCHF");

        App_Shared_UserDropdown ddlAccountlist = (App_Shared_UserDropdown)e.FindControl("ddlAccountlist");

        if (ddlAccountlist.SelectedValue.Length == 0)
        {
            string strMessage = "Please select an account before saving!";
            ShowError(strMessage);
            validtionSummary();
            return;
        }

        //Validate duplicate Account Name
        if (AllLSICustService.CheckDuplicateLSIAccountName(Convert.ToInt32(ddlAccountlist.SelectedValue), AISMasterEntities.AccountNumber) == false)
        {
            string strMessage = "This account was already associated as a related LSI account!";
            ShowError(strMessage);
            validtionSummary();
            return;
        }

        //Validate duplicate LSI Primary Account 
        if (AllLSICustService.getDuplLSIPrimaryAccount(Convert.ToBoolean(((chkLSIPrimary.Checked || chkLSIPrimary.Checked == null) ? true : false)), Convert.ToBoolean(chkLSIPrimary.Checked), Convert.ToInt32(AISMasterEntities.AccountNumber), 0) == true)
        {
            string strMessage = "You cannot have more than one primary LSI account associated!";
            ShowError(strMessage);
            validtionSummary();
            return;
        }



        if (ddlAccountlist.SelectedItem != null)
        {
            lsiCustomerBE.PRIM_IND = ((chkLSIPrimary.Checked) ? true : false);
            lsiCustomerBE.FULL_NAME = Convert.ToString(ddlAccountlist.SelectedItem);
            lsiCustomerBE.ACTV_IND = true; //((chkLSIActive.Checked) ? true : false);
            lsiCustomerBE.LSI_ACCT_ID = Convert.ToInt32(ddlAccountlist.SelectedValue);
            lsiCustomerBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
            lsiCustomerBE.UPDATE_DATE = DateTime.Now;
            lsiCustomerBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
            lsiCustomerBE.CREATE_USER_ID = CurrentAISUser.PersonID;
            lsiCustomerBE.CREATE_DATE = DateTime.Now;
            lsiCustomerBE.PLB_IND = ((chkPLB.Checked) ? true : false);
            lsiCustomerBE.CHF_IND = ((chkCHF.Checked) ? true : false);
            IList<LSIAllCustomersBE> LSICustmerList1 = AllLSICustService.getLSIAccounts();
            LSIAllCustomersBE lsiAllCustomersBE = LSICustmerList1.Where(lcl => lcl.LSI_ACCT_ID == Convert.ToInt32(ddlAccountlist.SelectedValue)).ToList().First();
            lsiCustomerBE.ACCT_TYP = lsiAllCustomersBE.ACCT_TYPE_DESC;
            AllLSICustService.Update(lsiCustomerBE);
        }

        // bind the listview
        BindRelatedLSIListView();
    }

    protected void lstRelatedLSIAccounts_ItemInserting(Object sender, ListViewInsertEventArgs e)
    {
        lstRelatedLSIAccounts.InsertItemPosition = InsertItemPosition.None;
    }

    // Invoked when the Edit Link in Related LSI Accounts Tab  is clicked
    protected void lstRelatedLSIAccounts_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        lstRelatedLSIAccounts.EditIndex = e.NewEditIndex;

        Label lblTemp = (Label)lstRelatedLSIAccounts.Items[e.NewEditIndex].FindControl("lblLSIAccountName");
        strCurrentAccount = lblTemp.Text;
        BindRelatedLSIListView();
    }

    // Invoked when the Cancel Link is clicked
    protected void lstRelatedLSIAccounts_ItemCancel(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstRelatedLSIAccounts.EditIndex = -1;
            BindRelatedLSIListView();
        }
        else if (e.CancelMode == ListViewCancelMode.CancelingInsert)
        {
            CancelLSIUpdateMode(); //Back to normal mode.
        }
    }

    protected void lstRelatedLSIAccounts_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SAVE")
        {
            lstRelatedLSIAccounts_Saving(e.Item);
        }
        else if (e.CommandName.ToUpper() == "DISABLE")
        {
            EnableDisableRow(e.Item, Convert.ToInt32(e.CommandArgument), "RelatedLSIAccounts");
        }
    }

    //private void txtAccountName_LostFocus(object sender, System.EventArgs e)
    //{
    //    chkMasterAccount.Focus();
    //}

    // Invoked when the Save Link is clicked
    protected void lstRelatedLSIAccounts_ItemUpdate(Object sender, ListViewUpdateEventArgs e)
    {
        // Get the values
        ListViewItem myItem = lstRelatedLSIAccounts.Items[lstRelatedLSIAccounts.EditIndex];
        Label lblTemp = (Label)lstRelatedLSIAccounts.Items[e.ItemIndex].FindControl("lblLSIAccountName");
        CheckBox chkLSIActive = (CheckBox)myItem.FindControl("chkLSIActive");
        Label lblLSICustmrID = (Label)myItem.FindControl("lblLSICustmrID");
        Label lblLSIAccountID = (Label)myItem.FindControl("lblLSIAccountID");
        CheckBox chkLSIPrimary = (CheckBox)myItem.FindControl("chkLSIPrimary");
        CheckBox chkPLB = (CheckBox)myItem.FindControl("chkPLB");
        CheckBox chkCHF = (CheckBox)myItem.FindControl("chkCHF");
        LSICustomerBE lsiCustomerBE = new LSICustomerBE();

        //Validate duplicate LSI Primary Account 
        if (AllLSICustService.getDuplLSIPrimaryAccount(Convert.ToBoolean(chkLSIActive.Checked), Convert.ToBoolean(chkLSIPrimary.Checked), Convert.ToInt32(AISMasterEntities.AccountNumber), Convert.ToInt32(lblLSIAccountID.Text)) == true)
        {
            string strMessage = "You cannot have more than one primary LSI account associated!";
            ShowError(strMessage);
            return;
        }

        //if (ddlAccountlist.SelectedItem != null)
        if (lblTemp.ToolTip != null)
        {
            lsiCustomerBE = AllLSICustService.getLSIAccount(Convert.ToInt32(lblLSICustmrID.Text));
            int custmrId = Convert.ToInt32(lblLSICustmrID.Text);
            LSICustomerBE lsiCustomerOld = LSICustmerList.Where(lcl => lcl.LSI_CUSTMR_ID == custmrId).ToList().First();
            bool con=ShowConcurrentConflict(Convert.ToDateTime(lsiCustomerOld.UPDATE_DATE), Convert.ToDateTime(lsiCustomerBE.UPDATE_DATE));
            if (!con)
                return;
            lsiCustomerBE.PRIM_IND = ((chkLSIPrimary.Checked) ? true : false);
            lsiCustomerBE.FULL_NAME = Convert.ToString(lblTemp.Text);
            lsiCustomerBE.ACTV_IND = ((chkLSIActive.Checked) ? true : false);
            lsiCustomerBE.LSI_ACCT_ID = Convert.ToInt32(lblTemp.ToolTip);
            lsiCustomerBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
            lsiCustomerBE.UPDATE_DATE = DateTime.Now;
            lsiCustomerBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
            lsiCustomerBE.PLB_IND = ((chkPLB.Checked) ? true : false);
            lsiCustomerBE.CHF_IND = ((chkCHF.Checked) ? true : false);
            IList<LSIAllCustomersBE> LSICustmerList1 = AllLSICustService.getLSIAccounts();
            LSIAllCustomersBE lsiAllCustomersBE = LSICustmerList1.Where(lcl => lcl.LSI_ACCT_ID == lsiCustomerBE.LSI_ACCT_ID).ToList().First();
            lsiCustomerBE.ACCT_TYP = lsiAllCustomersBE.ACCT_TYPE_DESC;
            bool Flag= AllLSICustService.Update(lsiCustomerBE);
        }

        // get out of the edit mode
        lstRelatedLSIAccounts.EditIndex = -1;

        //Code for logging into Audit Transaction Table 
        ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
        audtBS.Save(AISMasterEntities.AccountNumber, GlobalConstants.AuditingWebPage.AccountInfoRelatedLSI, CurrentAISUser.PersonID);

        BindRelatedLSIListView();
    }

    protected void lstRelatedLSIAccounts_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        
       
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            //Get a handle to the imgDisable image control
            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDisable");
            if (imgDelete != null)
            {
                imgDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to change status?');");
                if (imgDelete.ToolTip.ToString() == "NO")
                {
                    imgDelete.ImageUrl = "~/images/enabled.GIF";
                }
                else
                {
                    imgDelete.ImageUrl = "~/images/disabled.GIF";
                }
            }
            //Edit Link will be enabled when the record is Active
            LinkButton lbRelatedLSIAccountEdit = (LinkButton)e.Item.FindControl("lbRelatedLSIAccountEdit");
            if (lbRelatedLSIAccountEdit != null) if (imgDelete.ToolTip.ToString() == "NO") lbRelatedLSIAccountEdit.Enabled = false;
        }
    }
    protected void lstRelatedLSIAccounts_ItemCreated(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.InsertItem)
        {
            App_Shared_UserDropdown ddlAccountlist = (App_Shared_UserDropdown)e.Item.FindControl("ddlAccountlist");
            ddlAccountlist.OperationsButtonClicked += new EventHandler(ddlAccountlist_OperationsButtonClicked);
        } 
    }
    protected void CancelLSIUpdateMode()
    {
        lstRelatedLSIAccounts.InsertItemPosition = InsertItemPosition.None;
        BindRelatedLSIListView();
    }
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
    protected void LoadData(object sender, EventArgs e)
    {
        validtionSummary();
        Page.Validate();
        if (!Page.IsValid)
        {
            if (TabConAcctinfo.ActiveTabIndex == 1)
            {
                btnSaveCancel.Visible = false;
                ClearError();
                txtAccountName.TabIndex = 0;
                txtAccountName.Focus();
                if (Convert.ToBoolean(AISMasterEntities.MasterAccount))
                {
                    BindRelatedRetroListView();
                    pnlRelatedAccountsRO.Visible = false;
                    pnlRelatedAccounts.Visible = true;
                } //Populate Related Retro Accounts Tab
                else
                {
                    BindRelatedRetroListViewRO(); //Populate Related Retro Accounts Tab
                    pnlRelatedAccounts.Visible = false;
                    pnlRelatedAccountsRO.Visible = true;
                }
            }
            else if (TabConAcctinfo.ActiveTabIndex == 2)
            {
                btnSaveCancel.Visible = false;
                ClearError();
                BindRelatedLSIListView();  //Populate Related LSI Accounts Tab
            }
            else if (TabConAcctinfo.ActiveTabIndex == 3)
            {

                btnSaveCancel.Visible = false;
                ClearError();
                validtionSummary();
                BindResponsibilityListView(); // Populate Responsibility Tab
            }
            else if (TabConAcctinfo.ActiveTabIndex == 0)
            {
                btnSaveCancel.Visible = true;
                ClearError();
            }

        }

    }
    #region Private Methods

    private void BindRelatedLSIListView()
    {
        LSICustmerList = AllLSICustService.getRelatedLSIAccounts(AISMasterEntities.AccountNumber);
        this.lstRelatedLSIAccounts.DataSource = LSICustmerList;
        lstRelatedLSIAccounts.DataBind();
    }

    private void BindRelatedRetroListView()
    {
        AccountList = AccountService.getRelatedAccounts(AISMasterEntities.AccountNumber);
        this.lstRelatedAccounts.DataSource = AccountList;
        lstRelatedAccounts.DataBind();
    }

    private void BindRelatedRetroListViewRO()
    {
        this.lstRelatedAccountsRO.DataSource = AccountService.getRelatedAccountsRO(AISMasterEntities.AccountNumber, AISMasterEntities.MasterAccountNumber);
        lstRelatedAccountsRO.DataBind();
    }

    private void BindResponsibilityListView()
    {
        this.lstAccountResponsibility.DataSource = CustContactService.getAccountResponsibilities(AISMasterEntities.AccountNumber);
        this.lstAccountResponsibility.DataBind();
    }
    #endregion

    void ddlAccountlist_OperationsButtonClicked(object sender, EventArgs e)
    {

        string strMessage = "LSI Interface is not responding. Please contact business support group for resolution.";
           
            App_Shared_UserDropdown ddlAccountlist = (App_Shared_UserDropdown)(sender);
            ddlAccountlist.ErrorMessage += delegate(string message) 
            {
                if (message != "")
                {
                    ShowError(strMessage);
                    validtionSummary();
                    return;
                }
            };
            
       
    }

    /// <summary>
    /// modalLSIPrimaryAccountpopup button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region btnLSIPrimaryAccount Click Event
    protected void btnLSIPrimaryAccount_Click(object sender, EventArgs e)
    {
        modalLSIPrimaryAccount.Hide();
        LSICustomerBE lsiCustomerBE = new LSICustomerBE();

        bool Flag = false;

        LSICustmerList = AllLSICustService.GetActiveLSIPrimaryAccounts(AISMasterEntities.AccountNumber,this.LSIAccountID);
        for (int i = 0; i < LSICustmerList.Count; i++)
        {
            lsiCustomerBE = AllLSICustService.getLSIAccount(LSICustmerList[i].LSI_CUSTMR_ID);
            lsiCustomerBE.PRIM_IND = false;
            lsiCustomerBE.UPDATE_DATE = DateTime.Now;
            lsiCustomerBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
            Flag = AllLSICustService.Update(lsiCustomerBE);
        }
        lsiCustomerBE = AllLSICustService.getLSIAccount(this.LSIAccountID);
        lsiCustomerBE.ACTV_IND = true;
        lsiCustomerBE.UPDATE_DATE = DateTime.Now;
        lsiCustomerBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
        Flag = AllLSICustService.Update(lsiCustomerBE);
        BindRelatedLSIListView();
        
    }
    #endregion
    /// <summary>
    /// btnBPNumberClose button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region btnLSIPrimaryAccountClose Click Event
    protected void btnLSIPrimaryAccountClose_Click(object sender, EventArgs e)
    {
        modalLSIPrimaryAccount.Hide();
        return;

    }
    #endregion


    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCompany.SelectedValue == "733")
        {
            ddlCurrency.ClearSelection();
            ddlCurrency.Items.FindByValue("735").Selected = true;
            //ddlCurrency.SelectedIndex = 1;

        }
        else
        {
            ddlCurrency.ClearSelection();

            ddlCurrency.Items.FindByValue("736").Selected = true;
            //dlCurrency.SelectedIndex = 0;

        }

        ddlCurrency.Enabled = false;

    }
    protected void ddlCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCurrency.SelectedValue == "735")
        {
            ddlCompany.ClearSelection();
            ddlCompany.Items.FindByValue("733").Selected = true;

        }

        else
        {
            ddlCompany.ClearSelection();
            ddlCompany.Items.FindByValue("734").Selected = true;

        }
        ddlCompany.Enabled = false;

    }

}
