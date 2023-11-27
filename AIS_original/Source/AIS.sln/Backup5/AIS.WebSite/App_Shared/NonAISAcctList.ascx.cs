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


namespace ZurichNA.AIS.WebSite.App_Shared
{
    public partial class NonAISAcctList : System.Web.UI.UserControl
    {
        public int FlagMaster = 0;
        public int acctType = 0;
        private IList<NonAisCustomerBE> dtDDLSource;
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
                return this.ddlAccountlist.SelectedValue;
            }
            set
            {
                this.ddlAccountlist.SelectedValue = value;
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
                this.RetrieveAccountData("0", e);
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

            /* if (AccountType == 0)
             {
                 // This is default to load all accounts
                 this.dtDDLSource = (new AccountBS()).getAccounts(searchTerms);
             }
             else if (AccountType == 1)
             {
                 // This is to load all Master Accounts
                 this.dtDDLSource = (new AccountBS()).getMasterAccounts(searchTerms);
             }
             else if (AccountType == 2)
             {
                 // This is to load all MasterAccount and its ChildAccounts for given Master Account.

                 this.dtDDLSource = (new AccountBS()).getMasterWithChildAccounts((new AISBasePage()).AISMasterEntities.AccountNumber, searchTerms);
             }
             else if (AccountType == 3)
             {
                 // This is to load all NonMaster Accounts
                 this.dtDDLSource = (new AccountBS()).getNonMasterAccounts(searchTerms);
             }*/
            this.dtDDLSource = (new NonAisCustomerBS()).getNonaisCustomerlist(searchTerms);
            this.ddlAccountlist.DataSource = this.dtDDLSource;
            this.ddlAccountlist.DataTextField = "fullname";
            this.ddlAccountlist.DataValueField = "Nonaiscustmrid";
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
}