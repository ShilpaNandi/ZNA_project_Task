using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using System.Threading;

public partial class AcctSetup_AcctSearch : AISBasePage
{

    IList<AccountSearchBE> SearchList
    {
        get
        {
            //if (Session["SearchList"] == null)
            //    Session["SearchList"] = new List<AccountSearchBE>();
            //return (IList<AccountSearchBE>)Session["SearchList"];
            if (RetrieveObjectFromSessionUsingWindowName("SearchList") == null)
                SaveObjectToSessionUsingWindowName("SearchList", new List<AccountSearchBE>());
            return (IList<AccountSearchBE>)RetrieveObjectFromSessionUsingWindowName("SearchList");
        }
        set
        {
            //Session["SearchList"] = value;
            SaveObjectToSessionUsingWindowName("SearchList", value);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.Page.Title = "Adjustment Invoicing System - Account Search";
    }

    protected void cmdSearch_OnClick(object sender, EventArgs e)
    {
        int AccountID = -1;
        string strBPNumber = txtBPNumber.Text.Replace("_","").Trim();
        string strSSCGID = txtSSCGID.Text.Replace("_","").Trim();
        string strPolNo = txtPolicyNo.Text.Trim();
        if (!string.IsNullOrWhiteSpace(strPolNo))
        {
            string strFirstChar = strPolNo[0].ToString();
            if (strFirstChar != "0")
           {
               strPolNo = "0" + strPolNo;
           }
        }

        AccountSearchBS account = new AccountSearchBS();

        if (txtInsuredName.Text.Trim().Length == 0 &&
           (strBPNumber.Length == 0 )&&
            txtAccountNumber.Text.Trim().Length == 0 &&
            strSSCGID.Length == 0 && strPolNo.Length == 0)
        {
            string strMessage = "Please enter data into at least one of the five text boxes prior to clicking Search button";
            ShowError(strMessage);
            AISMasterEntities = null;
            //Disables Account related menus
            AISMenu.DisableDependMenu(CurrentAISUser.Role,DependentMenuLevel.Account);
            Menu aismenu = (Menu)Master.FindControl("TopNav");
            XmlDataSource AISMenuData = (XmlDataSource)Master.FindControl("AISMenuData");
            AISMenu.CreateMenu(CurrentAISUser.Role, ref AISMenuData);
            aismenu.DataBind();
            return;
        }
        else if (txtInsuredName.Text.Trim().Length == 0 &&
           strBPNumber.Length == 0 &&
            txtAccountNumber.Text.Trim().Length > 0 &&
            strSSCGID.Length == 0 && strPolNo.Length == 0)
        {
            AccountID = Convert.ToInt32(txtAccountNumber.Text);
            SearchList.Clear();
           IList <AccountSearchBE> acSearchBEList = account.getAccounts(AccountID);
            if(acSearchBEList.Count>0)
                SearchList.Add(acSearchBEList[0]);            
            this.testlistview.DataSource = SearchList;
        }
        else
        {
            int acNo;

            int.TryParse(txtAccountNumber.Text, out acNo);
            SearchList = account.getAccounts(
                ((txtInsuredName.Text.Trim().Length == 0) ? String.Empty : txtInsuredName.Text.Trim()),
                ((strBPNumber.Length == 0) ? String.Empty : strBPNumber), acNo,
                ((strSSCGID.Length == 0) ? String.Empty : strSSCGID), ((strPolNo.Length == 0) ? String.Empty : strPolNo));
        }

        if (this.testlistview.Items.Count > 0)
            this.testlistview.Items.Clear();
        this.testlistview.DataSource = SearchList;
        this.testlistview.DataBind();

        if (SearchList.Count == 0)
        {
            //string strMessage = "No search results matched the entered search criteria...!";
            //ShowError(strMessage);
            this.testlistview.Items.Clear();
            this.testlistview.DataSource = SearchList;
            this.testlistview.DataBind();
            AISMasterEntities = null;
            //Disables Account related menus
            AISMenu.DisableDependMenu(CurrentAISUser.Role, DependentMenuLevel.Account);
            Menu aismenu = (Menu)Master.FindControl("TopNav");
            XmlDataSource AISMenuData = (XmlDataSource)Master.FindControl("AISMenuData");
            AISMenu.CreateMenu(CurrentAISUser.Role, ref AISMenuData);
            aismenu.DataBind();
        }
        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry )
        btnCreate.Enabled = true;
    }

    protected void cmdAcctNew_OnClick(object sender, EventArgs e)
    {
        //Response.Redirect("~/AcctSetup/AcctInfo.aspx");
        ResponseRedirect("~/AcctSetup/AcctInfo.aspx");
    }

    protected void testlistview_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void testlistview_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
    {

    }

    protected void testlistview_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            int acctID = Convert.ToInt32(e.CommandArgument);
            AccountBE acct = (new AccountBS()).getAccount(acctID);
            AISMasterEntities = new MasterEntities();
            AISMasterEntities.AccountStatus = acct.ACTV_IND;
            AISMasterEntities.AccountNumber = acctID;
            AISMasterEntities.AccountName = acct.FULL_NM;
            AISMasterEntities.Bpnumber = acct.FINC_PTY_ID == null ? "" : acct.FINC_PTY_ID.ToString();
            AISMasterEntities.SSCGID = acct.SUPRT_SERV_CUSTMR_GP_ID == null ? "" : acct.SUPRT_SERV_CUSTMR_GP_ID.ToString();
            AISMasterEntities.MasterAccount = acct.MSTR_ACCT_IND == null ? false : acct.MSTR_ACCT_IND.Value;

            AISMasterEntities.MasterAccountNumber = (acct.CUSTMR_REL_ID == null) ? 0 : acct.CUSTMR_REL_ID.Value;
            //Session["AccountId"] = acctID;
            SaveObjectToSessionUsingWindowName("AccountId", acctID);

            //Enables Dependency Menus
            AISMenu.EnableDependMenu(DependentMenuLevel.Account);

            //Response.Redirect("~/AcctSetup/AcctInfo.aspx?Mode=Edit", false);
            ResponseRedirect("~/AcctSetup/AcctInfo.aspx?Mode=Edit");
        }

    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        //Response.Redirect("~/AcctSetup/AcctInfo.aspx?Mode=Add", false);
        ResponseRedirect("~/AcctSetup/AcctInfo.aspx?Mode=Add");
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtInsuredName.Text = txtBPNumber.Text = txtAccountNumber.Text
            = txtSSCGID.Text = String.Empty;
    }
    protected void txtInsuredName_TextChanged(object sender, EventArgs e)
    {

    }

    protected void testlistview_Sorting(object sender, ListViewSortEventArgs e)
    {
        ImageButton imgFPBtn = new ImageButton();
        ImageButton imgCSBtn = new ImageButton();
        ImageButton imgSSBtn = new ImageButton();

        imgFPBtn = (ImageButton)testlistview.FindControl("imgBPNoSort");
        imgCSBtn = (ImageButton)testlistview.FindControl("imgAcctNoSort");
        imgSSBtn = (ImageButton)testlistview.FindControl("imgSSCGSort");

        switch (e.SortExpression.ToUpper())
        {
            case "FINC_PTY_ID":
                e.SortDirection = (imgFPBtn.ImageUrl.Contains("Des")) ? SortDirection.Ascending : SortDirection.Descending;

                if (e.SortDirection == SortDirection.Ascending)
                    SearchList = SearchList.OrderBy(sl => sl.FINC_PTY_ID).ToList();
                else
                    SearchList = SearchList.OrderByDescending(sl => sl.FINC_PTY_ID).ToList();
                ChangeImage(imgFPBtn, e.SortDirection, imgCSBtn, imgSSBtn);
                break;
            case "CUSTMR_ID":
                e.SortDirection = (imgCSBtn.ImageUrl.Contains("Des")) ? SortDirection.Ascending : SortDirection.Descending;
                if (e.SortDirection == SortDirection.Ascending)
                    SearchList = SearchList.OrderBy(sl => sl.CUSTMR_ID).ToList();
                else
                    SearchList = SearchList.OrderByDescending(sl => sl.CUSTMR_ID).ToList();
                ChangeImage(imgCSBtn, e.SortDirection, imgFPBtn, imgSSBtn);
                break;
            case "SUPRT_SERV_CUSTMR_GP_ID":
                e.SortDirection = (imgSSBtn.ImageUrl.Contains("Des")) ? SortDirection.Ascending : SortDirection.Descending;
                if (e.SortDirection == SortDirection.Ascending)
                    SearchList = SearchList.OrderBy(sl => sl.SUPRT_SERV_CUSTMR_GP_ID).ToList();
                else
                    SearchList = SearchList.OrderByDescending(sl => sl.SUPRT_SERV_CUSTMR_GP_ID).ToList();
                ChangeImage(imgSSBtn, e.SortDirection, imgFPBtn, imgCSBtn);
                break;
        }

        testlistview.DataSource = SearchList;
        testlistview.DataBind();

    }

    private void ChangeImage(ImageButton imgBtn, SortDirection sDir, ImageButton img1, ImageButton img2)
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
        img1.ImageUrl = img2.ImageUrl = "~/images/ascending.gif";
        img1.Visible = img2.Visible = false;

    }
}
