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


public delegate void SendErrorMessageHandler(string ErrorMessage);

public partial class App_Shared_UserDropdown : System.Web.UI.UserControl
{

    public event SendErrorMessageHandler ErrorMessage;
    public int FlagMaster = 0;
    public int acctType = 0;
    public string selectedAccountName = string.Empty;
    public int selectedAccountNo = 0;
    private IList<AccountBE> dtAISDDLSource;
    private IList<LSIAllCustomersBE> dtLSIDDLSource;
    public event EventHandler OperationsButtonClicked;
    public event EventHandler OnSelectedIndexChanged;

    public int MasterAccount
    {
        get
        {
            return FlagMaster;
        }
        set
        {
            FlagMaster = value;
        }
    }
    public int AccountType
    {
        get
        {
            return acctType;
        }
        set
        {
            acctType = value;
        }
    }
    public int SelectedIndex
    {
        get
        {
            return ddlAccountlist.SelectedIndex;
        }
        set
        {
            ddlAccountlist.SelectedIndex = value;
        }
    }

    public string SelectedValue
    {
        get
        {
            return ddlAccountlist.SelectedValue;
        }
        set
        {
            ddlAccountlist.SelectedValue = value;
        }
    }

    public string SelectedItem
    {
        get
        {
            return Convert.ToString(ddlAccountlist.SelectedItem);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (selectedAccountName == "")
                this.RetrieveAccountData("0", e);
            else
            {
                this.RetrieveAccountData(selectedAccountName[0].ToString(), e);
                ListItem li = new ListItem(selectedAccountName, selectedAccountNo.ToString());
                if (ddlAccountlist.Items.Contains(li))
                    this.ddlAccountlist.Items.FindByText(selectedAccountName).Selected = true;
            }
        }
    }

    protected void LButton_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        this.RetrieveAccountData(lb.Text.Trim(), e);

    }

    private void RetrieveAccountData(string startRange, EventArgs e)
    {
        string[] searchTerms = null;
        if (OperationsButtonClicked != null)
            OperationsButtonClicked(this, e);


        switch (startRange)
        {

            case "0":
                searchTerms = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B" };
                break;
            case "C":
                searchTerms = new string[] { "C" };
                break;
            case "D":
                searchTerms = new string[] { "D", "E" };
                break;
            case "F":
                searchTerms = new string[] { "F", "G" };
                break;
            case "H":
                searchTerms = new string[] { "H", "I" };
                break;
            case "J":
                searchTerms = new string[] { "J", "K", "L" };
                break;
            case "M":
                searchTerms = new string[] { "M", "N", "O" };
                break;
            case "P":
                searchTerms = new string[] { "P", "Q", "R" };
                break;
            case "S":
                searchTerms = new string[] { "S" };
                break;
            case "T":
                searchTerms = new string[] { "T", "U" };
                break;
            case "V":
                searchTerms = new string[] { "V", "W", "X", "Y", "Z" };
                break;
        }

        if (AccountType == 0)
        {
            // This is default to load all accounts
            this.dtAISDDLSource = (new AccountBS()).getAccounts(searchTerms);
        }
        else if (AccountType == 1)
        {
            // This is to load all Master and regular Accounts
            this.dtAISDDLSource = (new AccountBS()).getMasterandRegularAccounts(searchTerms);
        }
        else if (AccountType == 2)
        {
            // This is to load all MasterAccount and its ChildAccounts for given Master Account.

            this.dtAISDDLSource = (new AccountBS()).getMasterWithChildAccounts((new AISBasePage()).AISMasterEntities.AccountNumber, searchTerms);
        }
        else if (AccountType == 3)
        {
            // This is to load all NonMaster Accounts
            this.dtAISDDLSource = (new AccountBS()).getNonMasterAccounts(searchTerms);
        }
        else if (AccountType == 4)
        {
            // This is to load all LSI Accounts
            try
            {
                this.dtLSIDDLSource = (new LSICustomersBS()).getLSIAccounts(searchTerms);
            }
            catch (Exception ee)
            {
                if (ErrorMessage != null) { ErrorMessage(ee.Message); }
            }

        }


        if (AccountType == 4)
        {
            this.ddlAccountlist.DataSource = this.dtLSIDDLSource;
            this.ddlAccountlist.DataTextField = "FULL_NAME";
            this.ddlAccountlist.DataValueField = "LSI_ACCT_ID";
        }
        else
        {
            this.ddlAccountlist.DataSource = this.dtAISDDLSource;
            this.ddlAccountlist.DataTextField = "FULL_NM";
            this.ddlAccountlist.DataValueField = "CUSTMR_ID";
        }
        this.ddlAccountlist.DataBind();
        ListItem li = new ListItem("(Select)", "0");
        this.ddlAccountlist.Items.Insert(0, li);

    }

    protected void ddlAccountlist_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (OnSelectedIndexChanged != null)
            OnSelectedIndexChanged(this, e);
    }
}
