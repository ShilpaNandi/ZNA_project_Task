//Default namespaces in Invoice Inquiry screen
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
//Importing different AIS framework namespaces for Invoice Inquiry screen
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.AIS.DAL.Logic;

/// <summary>
/// This is the class which is used to give the information about the InvoiceInquiry and it 
/// also inherits AISBase Page
/// </summary>
#region InvoiceInquiry Class
public partial class Invoice_InvoiceInquiry : AISBasePage
{

    /// <summary>
    /// Page load event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ddlAcctlist.OnSelectedIndexChanged += new EventHandler(ddlAccountlist_SelectedIndexChanged);
        if (!Page.IsPostBack)
        {
            this.Master.Page.Title = "Invoice Inquiry";
            //ChangeDropDownStates(false);
            int intCRMid = new BLAccess().GetLookUpID(GlobalConstants.InternalContacts.Crm);
            ContactNamesDataSource.SelectParameters["ContTypId"].DefaultValue = intCRMid.ToString();
            ContactNamesDataSource.Select();
            ContactNamesDataSource.DataBind();

        }

        //Checks Exiting without Save
        ArrayList list = new ArrayList();
        list.Add(lbCloseDetails);
        ProcessExitFlag(list);
    }
    #endregion

    #region Properties Declaration Section
    /// <summary>
    /// A Property for Holding AccountName in ViewState
    /// </summary>
    protected string AccountName
    {
         
        get
        {
            if ( ViewState["AccName"] != null)
            {
                return  ViewState["AccName"].ToString();
            }
            else
            {
                 ViewState["AccName"]= "";
                return "";
            }
        }
        set
        {
            ViewState["AccName"] = value;
        }
    }
    /// <summary>
    /// A Property for Holding ProgramType in ViewState
    /// </summary>
    protected string ProgramType
    {

        get
        {
            if (ViewState["ProgramTyp"] != null)
            {
                return ViewState["ProgramTyp"].ToString();
            }
            else
            {
                ViewState["ProgramTyp"] = "";
                return "";
            }
        }
        set
        {
            ViewState["ProgramTyp"] = value;
        }
    }
    /// <summary>
    /// A Property for Holding ValuationDate in ViewState
    /// </summary>
    protected string ValuationDate
    {

        get
        {
            if (ViewState["ValDate"] != null)
            {
                return ViewState["ValDate"].ToString();
            }
            else
            {
                ViewState["ValDate"] = "";
                return "";
            }
        }
        set
        {
            ViewState["ValDate"] = value;
        }
    }
    /// <summary>
    /// A Property for Holding InvoiceNumber in ViewState
    /// </summary>
    protected string InvoiceNumber
    {

        get
        {
            if (ViewState["InvNumber"] != null)
            {
                return ViewState["InvNumber"].ToString();
            }
            else
            {
                ViewState["InvNumber"] = "";
                return "";
            }
        }
        set
        {
            ViewState["InvNumber"] = value;
        }
    }
    /// <summary>
    /// A Property for Holding InvoiceDate in ViewState
    /// </summary>
    protected string InvoiceDate
    {

        get
        {
            if (ViewState["InvDate"] != null)
            {
                return ViewState["InvDate"].ToString();
            }
            else
            {
                ViewState["InvDate"] = "";
                return "";
            }
        }
        set
        {
            ViewState["InvDate"] = value;
        }
    }
    /// <summary>
    /// A Property for Holding CFS2Name in ViewState
    /// </summary>
    protected string CFS2Name
    {

        get
        {
            if (ViewState["CFS2Name"] != null)
            {
                return ViewState["CFS2Name"].ToString();
            }
            else
            {
                ViewState["CFS2Name"] = "";
                return "";
            }
        }
        set
        {
            ViewState["CFS2Name"] = value;
        }
    }
     /// <summary>
    /// A Property for Holding BrokerName in ViewState
    /// </summary>
    protected string BrokerName
    {

        get
        {
            if (ViewState["BrokerName"] != null)
            {
                return ViewState["BrokerName"].ToString();
            }
            else
            {
                ViewState["BrokerName"] = "";
                return "";
            }
        }
        set
        {
            ViewState["BrokerName"] = value;
        }
    }
    /// <summary>
    /// A Property for Holding BusinessUnit in ViewState
    /// </summary>
    protected string BusinessUnit
    {

        get
        {
            if (ViewState["Bunit"] != null)
            {
                return ViewState["Bunit"].ToString();
            }
            else
            {
                ViewState["Bunit"] = "";
                return "";
            }
        }
        set
        {
            ViewState["Bunit"] = value;
        }
    }
    /// <summary>
    /// A Property for Holding Invoice Inquiry DataTable in Session
    /// </summary>
    private DataTable dtInvoiceInquiry
    {
        get { return (DataTable)Session["InvoiceInquiry"]; }
        set { Session["InvoiceInquiry"] = value; }
    }
    #endregion

    
    /// <summary>
    /// Search Button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Search Button Click Event
    protected void btnSearch_Click(object sender, EventArgs e)
    {
       
        pnlInvInqist.Visible = true;
        BindInvoiceHistory();
    }
    #endregion

    /// <summary>
    /// Clear Button Click Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Clear Button Click Event
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearSearchParameters();
    }
    #endregion
    /// <summary>
    /// Method to Clear the the Search parametrs when user clicks on the Clear Button
    /// </summary>
    #region ClearSearchParameters
    private void ClearSearchParameters()
    {
        DropDownList ddlAccountlist = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        ddlAccountlist.SelectedIndex = 0;
        ddlProgramType.SelectedIndex = 0;
        this.txtInvoiceDate.Text = string.Empty;
        this.txtInvoiceNumber.Text = string.Empty;
        this.txtValuationDate.Text = string.Empty;
        ddlCFS2Name.SelectedIndex = 0;
        txtAccountNumber.Text = string.Empty;
        ddlBrokerName.SelectedIndex = 0;
        ddlBusinessUnit.SelectedIndex = 0;
        btnExpExcel.Visible = false;
        txtAccountNumber.Enabled = true;
        lstInvoiceInquiry.Items.Clear();
        DataTable dtInvInquiry = new DataTable();
        lstInvoiceInquiry.DataSource = dtInvInquiry;
        lstInvoiceInquiry.DataBind();
    }
    #endregion
    /// <summary>
    /// Function to bind the Search Results to listview
    /// </summary>
    #region BindInvoiceHistory
    public void BindInvoiceHistory()
    {
        int intAccountID = 0;
        string strAccountName = string.Empty;
        string strCFS2Name = string.Empty;
        string strProgramTyp = string.Empty;
        string strBrokerName = string.Empty;
        string strBusinessUnit = string.Empty;
        int intAccountNumber=0;
        int intProgramTypID = 0;
        string strValDate = string.Empty;
        string strInvoiceNumber = string.Empty;
        string strInvoiceDate = string.Empty;
        int intPersonID = 0;
        int intExtOrgID = 0;
        int intInternalOrgID = 0;
        DropDownList ddlAccountlist = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        if (ddlAccountlist.SelectedIndex != 0 && ddlAccountlist.SelectedIndex != -1)
        { 
           intAccountID=Convert.ToInt32(ddlAccountlist.SelectedValue.ToString());
           strAccountName = ddlAccountlist.SelectedItem.Text.ToString();
        }
        if (ddlProgramType.SelectedIndex != 0)
        {
            intProgramTypID = Convert.ToInt32(ddlProgramType.SelectedValue.ToString());
            strProgramTyp = ddlProgramType.SelectedItem.Text.ToString();
        }
        if (txtValuationDate.Text.Trim() != "")
        {
            strValDate = txtValuationDate.Text.Trim();
        }
        if (txtInvoiceNumber.Text.Trim() != "")
        {
            strInvoiceNumber = txtInvoiceNumber.Text.Trim();
        }
        if (txtInvoiceDate.Text.Trim() != "")
        {
            strInvoiceDate = txtInvoiceDate.Text.Trim();
        }
        if (ddlCFS2Name.SelectedIndex != 0)
        {
            intPersonID = Convert.ToInt32(ddlCFS2Name.SelectedValue.ToString());
            strCFS2Name = ddlCFS2Name.SelectedItem.Text.ToString();
        }
        if (txtAccountNumber.Text != null && txtAccountNumber.Text.Trim()!="")
        {
            intAccountNumber = Convert.ToInt32(txtAccountNumber.Text.ToString());
        }
        if (ddlBrokerName.SelectedIndex != 0)
        {
            intExtOrgID = Convert.ToInt32(ddlBrokerName.SelectedValue.ToString());
            strBrokerName = ddlBrokerName.SelectedItem.Text.ToString();
        }
        if (ddlBusinessUnit.SelectedIndex != 0)
        {
            intInternalOrgID = Convert.ToInt32(ddlBusinessUnit.SelectedValue.ToString());
            strBusinessUnit = ddlBusinessUnit.SelectedItem.Text.ToString();
        }
        dtInvoiceInquiry= (new InvoiceInquiryBS()).getInvoiceInquiryData(intAccountID, intProgramTypID, strValDate, strInvoiceNumber, strInvoiceDate, intPersonID, intExtOrgID, intInternalOrgID, intAccountNumber);
        if (dtInvoiceInquiry.Rows.Count > 0)
            btnExpExcel.Visible = true;
        else
            btnExpExcel.Visible = false;
        lstInvoiceInquiry.DataSource = dtInvoiceInquiry;
        lstInvoiceInquiry.DataBind();
        this.AccountName = strAccountName;
        this.ProgramType= strProgramTyp;
        this.ValuationDate= strValDate;
        this.InvoiceNumber = strInvoiceNumber;
        this.InvoiceDate = strInvoiceDate;
        this.CFS2Name = strCFS2Name;
        this.BrokerName = strBrokerName;
        this.BusinessUnit = strBusinessUnit;

    }
    #endregion
    /// <summary>
    /// Item Data Bound Event-it is called while binding each row to the listview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region ListView DataBound Event
    protected void DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {

            HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trItemTemplate");
            tr.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            tr.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
           
        }
    }
    #endregion
    /// <summary>
    /// ListView Item Command Event-it is Called when user clicks on the Details Link button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Item Command Event
    protected void CommandList(Object sender, ListViewCommandEventArgs e)
    {
        switch (e.CommandName.Trim().ToUpper())
        {
            case "GETPDF":
                string strZDWKey = e.CommandArgument.ToString();
                if (strZDWKey == "" || strZDWKey == null)
                {
                    ShowError("Report is not available");
                    return;
                }
                DownloadFile(strZDWKey);
                break;
            case "SELECT":
                int intPremAdjID = Convert.ToInt32(e.CommandArgument);
                DisplayDetails(intPremAdjID);
                ChangeDetailState(true);
                break;
        }

    }
    #endregion
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
    /// <summary>
    /// selectedindexchanged event of the listview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Listview SelectedIndexChangingEvent 
    protected void lstInvoiceInquiry_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
    {
        if (lstInvoiceInquiry.Items.Count > 0)
        {
            HtmlTableRow tr;
            //code for changing the previous selected row color to its original Color
            if (ViewState["SelectedIndex"] != null && ViewState["SelectedIndex"] != "")
            {
                int count = lstInvoiceInquiry.Items.Count;
                int index = Convert.ToInt32(ViewState["SelectedIndex"]);
                if (lstInvoiceInquiry.Items.Count > index)
                {
                    if (count > 1)
                    {
                        tr = (HtmlTableRow)lstInvoiceInquiry.Items[index].FindControl("trItemTemplate");
                        tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
                    }
                    else
                    {
                        tr = (HtmlTableRow)lstInvoiceInquiry.Items[0].FindControl("trItemTemplate");
                        tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
                    }
                }
            }
            tr = (HtmlTableRow)lstInvoiceInquiry.Items[e.NewSelectedIndex].FindControl("trItemTemplate");
            LinkButton lnk = (LinkButton)lstInvoiceInquiry.Items[e.NewSelectedIndex].FindControl("lnkSelect");
            ViewState["SelectedIndex"] = e.NewSelectedIndex;
            //code for changing the selected row style to highlighted
            tr.Attributes["class"] = "SelectedItemTemplate";
        }

    }
    #endregion

    /// <summary>
    /// Function to Change the lisview size and other changes required when user clicks on the Details Link
    /// </summary>
    /// <param name="Flag"></param>
    #region ChangeDetailsState
    private void ChangeDetailState(bool Flag)
    {
        lbCloseDetails.Visible = Flag;
        lstInvoiceInquiry.Enabled = !Flag;
        pnlButtons.Enabled = !Flag;
        pnlSearchParams1.Enabled = !Flag;
        pnlSearchParams2.Enabled = !Flag;
        btnExpExcel.Visible = !Flag;
        lblInvoiceInquiryDetails.Visible = Flag;
        pnlDetails.Visible = Flag;
        pnlInvInqist.Height = (Flag==true?Unit.Point(60):Unit.Point(200));
    }
    #endregion

    /// <summary>
    /// Function to Dispaly the Details in Panel
    /// </summary>
    /// <param name="intPremAdjID"></param>
    #region DisplayDetails
    private void DisplayDetails(int intPremAdjID)
    {
        if (intPremAdjID > 0)
        {
            DataRow[] drInvoiceList = dtInvoiceInquiry.Select("PREM_ADJ_ID=" + intPremAdjID);
            if (drInvoiceList.Count() > 0)
            {
                lblAccNo.Text = (drInvoiceList[0]["ACCOUNT_NUMBER"] == null ? string.Empty : drInvoiceList[0]["ACCOUNT_NUMBER"].ToString());
                lblIVDate.Text = (drInvoiceList[0]["INVOICE_DATE"] == null ? string.Empty : drInvoiceList[0]["INVOICE_DATE"].ToString());
                lblAccName.Text = (drInvoiceList[0]["ACCOUNT_NAME"] == null ? string.Empty : drInvoiceList[0]["ACCOUNT_NAME"].ToString());
                lblInvoiceAmount.Text = drInvoiceList[0]["INVOICE_AMOUNT"] != null ? (drInvoiceList[0]["INVOICE_AMOUNT"].ToString() != "" ? (decimal.Parse(drInvoiceList[0]["INVOICE_AMOUNT"].ToString())).ToString("#,##0") : "0") : "0";
                lblCFS2role.Text = (drInvoiceList[0]["CFS2_NAME"] == null ? string.Empty : drInvoiceList[0]["CFS2_NAME"].ToString());
                lblAdjStatus.Text = (drInvoiceList[0]["ADJUSTMENT_STATUS"] == null ? string.Empty : drInvoiceList[0]["ADJUSTMENT_STATUS"].ToString());
                lblInsuredContatct.Text = (drInvoiceList[0]["INSURED_CONTACT"] == null ? string.Empty : drInvoiceList[0]["INSURED_CONTACT"].ToString());
                //lblPolicyNo.Text = (InvoiceBE.POLICY_NO == null ? string.Empty : InvoiceBE.POLICY_NO.ToString());
                lblVDate.Text = (drInvoiceList[0]["VALUATION_DATE"] == null ? string.Empty : drInvoiceList[0]["VALUATION_DATE"].ToString());
                lblDateAdjusted.Text = (drInvoiceList[0]["ADJUSTMENT_STATUS_DATE"] == null ? string.Empty : drInvoiceList[0]["ADJUSTMENT_STATUS_DATE"].ToString());
                lblBroker.Text = (drInvoiceList[0]["BROKER"] == null ? string.Empty : drInvoiceList[0]["BROKER"].ToString());
                ////lblDateQc.Text = (InvoiceBE.QCD_DATE == null ? string.Empty : InvoiceBE.QCD_DATE.ToString());
                lblBCName.Text = (drInvoiceList[0]["BROKER_CONTACT"] == null ? string.Empty : drInvoiceList[0]["BROKER_CONTACT"].ToString());
                lblDrftIVDate.Text = (drInvoiceList[0]["DRAFT_INVOICE_DATE"] == null ? string.Empty : drInvoiceList[0]["DRAFT_INVOICE_DATE"].ToString());
                lblBUOffice.Text = (drInvoiceList[0]["BU_OFFICE"] == null ? string.Empty : drInvoiceList[0]["BU_OFFICE"].ToString());
                lblFnlIvDate.Text = (drInvoiceList[0]["FINAL_INVOICE_DATE"] == null ? string.Empty : drInvoiceList[0]["FINAL_INVOICE_DATE"].ToString());
                lblInvoiceNo.Text = drInvoiceList[0]["FNL_INVOICE_NUMBER"] != null ? (drInvoiceList[0]["FNL_INVOICE_NUMBER"].ToString() != "" ? (drInvoiceList[0]["FNL_INVOICE_NUMBER"].ToString()) : ((drInvoiceList[0]["DRFT_INVOICE_NUMBER"] != null ? (drInvoiceList[0]["DRFT_INVOICE_NUMBER"].ToString()) : ""))) : ((drInvoiceList[0]["DRFT_INVOICE_NUMBER"] != null ? (drInvoiceList[0]["DRFT_INVOICE_NUMBER"].ToString()) : ""));
                //lblInvoiceNo.Text = ((drInvoiceList[0]["FNL_INVOICE_NUMBER"]) != null ? (drInvoiceList[0]["FNL_INVOICE_NUMBER"].ToString()): ((drInvoiceList[0]["DRFT_INVOICE_NUMBER"]) != null ?(drInvoiceList[0]["DRFT_INVOICE_NUMBER"].ToString()) :string.Empty ) );
            }
        }
    }
    #endregion

    /// <summary>
    /// Button to Close the Details Panel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Close Button Click Event
    protected void lbCloseDetails_Click(object sender, EventArgs e)
    {
        ChangeDetailState(false);
    }
    #endregion

    /// <summary>
    /// Onselectedindexchanged event for the user control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region AccountList UserControl SelectedIndexChangedEvent
    protected void ddlAccountlist_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlAccountlist = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        if (ddlAccountlist.SelectedIndex != 0 && ddlAccountlist.SelectedIndex != -1)
        {
            txtAccountNumber.Enabled = false;
            txtAccountNumber.Text = "";
           
            
        }
        else
        {
            txtAccountNumber.Enabled = true;
           

        }
        btnExpExcel.Visible = false;
        lstInvoiceInquiry.Items.Clear();
        DataTable dtInvInquiry = new DataTable();
        lstInvoiceInquiry.DataSource = dtInvInquiry;
        lstInvoiceInquiry.DataBind();
       
        
    }
    #endregion
    
    /// <summary>
    /// ListView sorting event-it is called when user clicks on sorting button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region ListView Sorting Event
    protected void lstInvoiceInquiry_Sorting(object sender, ListViewSortEventArgs e)
    {

        Image imgAdjStatus = (Image)lstInvoiceInquiry.FindControl("imgAdjStatus");

        imgAdjStatus.Visible = false;
        DataView dvInvoice = dtInvoiceInquiry.DefaultView;
        imgAdjStatus.Visible = true;
        if (imgAdjStatus.ToolTip == "Ascending")
        {
            dvInvoice.Sort = "ADJUSTMENT_STATUS ASC";
            imgAdjStatus.ToolTip = "Descending";
            imgAdjStatus.ImageUrl = "~/images/descending.gif";
        }
        else
        {
            dvInvoice.Sort = "ADJUSTMENT_STATUS DESC";
            imgAdjStatus.ToolTip = "Ascending";
            imgAdjStatus.ImageUrl = "~/images/ascending.gif";
        }

        lstInvoiceInquiry.DataSource = dvInvoice;
        lstInvoiceInquiry.DataBind();
    }
    #endregion
    /// <summary>
    /// Export Button Click Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Export to Excel Click Event
    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
       Export2Excel();
    }
    #endregion

    /// <summary>
    /// Function to Export Listview data to Excel sheet
    /// </summary>
    #region Export2Excel
    private void Export2Excel()
    {
        string strDate = DateTime.Now.ToString().Replace(":", "-");
        strDate = strDate.Replace("/", "-");
        string strFileName = "Invoice Inquiry Report" + strDate + ".xls";
        Table tbl = new Table();


        TableRow tblRow1 = new TableRow();
        TableCell[] cell1 = new TableCell[2];
        cell1[0] = new TableCell();
        cell1[0].Text = "PARAMETER";
        cell1[1] = new TableCell();
        cell1[1].Text = "VALUE";
        AddStyle(cell1, true);
        tblRow1.Cells.AddRange(cell1);
        tbl.Rows.Add(tblRow1);

        TableRow tblRow2 = new TableRow();
        TableCell[] cell2 = new TableCell[2];
        cell2[0] = new TableCell();
        cell2[0].Text = "ACCOUNT NAME";
        cell2[1] = new TableCell();
        cell2[1].Text = this.AccountName;
        AddStyle(cell2, true);
        tblRow2.Cells.AddRange(cell2);
        tbl.Rows.Add(tblRow2);

        TableRow tblRow3 = new TableRow();
        TableCell[] cell3 = new TableCell[2];
        cell3[0] = new TableCell();
        cell3[0].Text = "PROGRAM TYPE";
        cell3[1] = new TableCell();
        cell3[1].Text = this.ProgramType;
        AddStyle(cell3, true);
        tblRow3.Cells.AddRange(cell3);
        tbl.Rows.Add(tblRow3);

        TableRow tblRow4 = new TableRow();
        TableCell[] cell4 = new TableCell[2];
        cell4[0] = new TableCell();
        cell4[0].Text = "VALUATION DATE";
        cell4[1] = new TableCell();
        cell4[1].Text = this.ValuationDate;
        AddStyle(cell4, true);
        tblRow4.Cells.AddRange(cell4);
        tbl.Rows.Add(tblRow4);

        TableRow tblRow5 = new TableRow();
        TableCell[] cell5 = new TableCell[2];
        cell5[0] = new TableCell();
        cell5[0].Text = "INVOICE NUMBER";
        cell5[1] = new TableCell();
        cell5[1].Text = this.InvoiceNumber;
        AddStyle(cell5, true);
        tblRow5.Cells.AddRange(cell5);
        tbl.Rows.Add(tblRow5);

        TableRow tblRow6 = new TableRow();
        TableCell[] cell6 = new TableCell[2];
        cell6[0] = new TableCell();
        cell6[0].Text = "INVOICE DATE";
        cell6[1] = new TableCell();
        cell6[1].Text = this.InvoiceDate;
        AddStyle(cell6, true);
        tblRow6.Cells.AddRange(cell6);
        tbl.Rows.Add(tblRow6);

        TableRow tblRow7 = new TableRow();
        TableCell[] cell7 = new TableCell[2];
        cell7[0] = new TableCell();
        cell7[0].Text = "CFS2 NAME";
        cell7[1] = new TableCell();
        cell7[1].Text = this.CFS2Name;
        AddStyle(cell7, true);
        tblRow7.Cells.AddRange(cell7);
        tbl.Rows.Add(tblRow7);

        TableRow tblRow8 = new TableRow();
        TableCell[] cell8 = new TableCell[2];
        cell8[0] = new TableCell();
        cell8[0].Text = "BROKER NAME";
        cell8[1] = new TableCell();
        cell8[1].Text = this.BrokerName;
        AddStyle(cell8, true);
        tblRow8.Cells.AddRange(cell8);
        tbl.Rows.Add(tblRow8);

        TableRow tblRow9 = new TableRow();
        TableCell[] cell9 = new TableCell[2];
        cell9[0] = new TableCell();
        cell9[0].Text = "BUSINESS UNIT";
        cell9[1] = new TableCell();
        cell9[1].Text = this.BusinessUnit;
        AddStyle(cell9, true);
        tblRow9.Cells.AddRange(cell9);
        tbl.Rows.Add(tblRow9);

        TableRow tblRow11 = new TableRow();
        tbl.Rows.Add(tblRow11);


        TableRow tblRow10 = new TableRow();
        TableCell[] cell10 = new TableCell[14];
        cell10[0] = new TableCell();
        cell10[0].Text = "ACCOUNT NO";
        cell10[1] = new TableCell();
        cell10[1].Text = "ACCOUNT NAME";
        cell10[2] = new TableCell();
        cell10[2].Text = "INSURED CONTACT";
        cell10[3] = new TableCell();
        cell10[3].Text = "VALUATION DATE";
        cell10[4] = new TableCell();
        cell10[4].Text = "BROKER";
        cell10[5] = new TableCell();
        cell10[5].Text = "BROKER CONTACT";
        cell10[6] = new TableCell();
        cell10[6].Text = "BU OFFICE";
        cell10[7] = new TableCell();
        cell10[7].Text = "INVOICE NUMBER";
        cell10[8] = new TableCell();
        cell10[8].Text = "INVOICE DATE";
        cell10[9] = new TableCell();
        cell10[9].Text = "INVOICE AMOUNT";
        cell10[10] = new TableCell();
        cell10[10].Text = "ADJ. STATUS";
        cell10[11] = new TableCell();
        cell10[11].Text = "ADJ. STATUS DATE";
        cell10[12] = new TableCell();
        cell10[12].Text = "DRAFT INVOICE DATE";
        cell10[13] = new TableCell();
        cell10[13].Text = "FINAL INVOICE DATE";
        
        AddStyle(cell10, true);
        tblRow10.Cells.AddRange(cell10);
        tbl.Rows.Add(tblRow10);
        
        //int rowIndex = 1;
        DataTable dtInvoicelist = dtInvoiceInquiry;
        for (int row = 0; row < dtInvoicelist.Rows.Count; row++)
        {
            tblRow10 = new TableRow();

            cell10[0] = new TableCell();
            cell10[0].Text = (dtInvoicelist.Rows[row]["ACCOUNT_NUMBER"]!=null?dtInvoicelist.Rows[row]["ACCOUNT_NUMBER"].ToString():"");
            cell10[1] = new TableCell();
            cell10[1].Text = (dtInvoicelist.Rows[row]["ACCOUNT_NAME"]!=null?dtInvoicelist.Rows[row]["ACCOUNT_NAME"].ToString():"");
            cell10[2] = new TableCell();
            cell10[2].Text = (dtInvoicelist.Rows[row]["INSURED_CONTACT"]!=null?dtInvoicelist.Rows[row]["INSURED_CONTACT"].ToString():"");
            cell10[3] = new TableCell();
            cell10[3].Text = (dtInvoicelist.Rows[row]["VALUATION_DATE"]!=null?dtInvoicelist.Rows[row]["VALUATION_DATE"].ToString():"");
            cell10[4] = new TableCell();
            cell10[4].Text = (dtInvoicelist.Rows[row]["BROKER"]!=null?dtInvoicelist.Rows[row]["BROKER"].ToString():"");
            cell10[5] = new TableCell();
            cell10[5].Text = (dtInvoicelist.Rows[row]["BROKER_CONTACT"]!=null?dtInvoicelist.Rows[row]["BROKER_CONTACT"].ToString():"");
            cell10[6] = new TableCell();
            cell10[6].Text = (dtInvoicelist.Rows[row]["BU_OFFICE"]!=null?dtInvoicelist.Rows[row]["BU_OFFICE"].ToString():"");
            cell10[7] = new TableCell();
            cell10[7].Text = (dtInvoicelist.Rows[row]["DRFT_INVOICE_NUMBER"]!=null?dtInvoicelist.Rows[row]["DRFT_INVOICE_NUMBER"].ToString():"");
            cell10[8] = new TableCell();
            cell10[8].Text = (dtInvoicelist.Rows[row]["INVOICE_DATE"]!=null?dtInvoicelist.Rows[row]["INVOICE_DATE"].ToString():"");
            cell10[9] = new TableCell();
            cell10[9].Text = (dtInvoicelist.Rows[row]["INVOICE_AMOUNT"]!=null?dtInvoicelist.Rows[row]["INVOICE_AMOUNT"].ToString():"");
            cell10[10] = new TableCell();
            cell10[10].Text = (dtInvoicelist.Rows[row]["ADJUSTMENT_STATUS"]!=null?dtInvoicelist.Rows[row]["ADJUSTMENT_STATUS"].ToString():"");
            cell10[11] = new TableCell();
            cell10[11].Text = (dtInvoicelist.Rows[row]["ADJUSTMENT_STATUS_DATE"] != null ? dtInvoicelist.Rows[row]["ADJUSTMENT_STATUS_DATE"].ToString() : "");
            cell10[12] = new TableCell();
            cell10[12].Text = (dtInvoicelist.Rows[row]["DRAFT_INVOICE_DATE"]!=null?dtInvoicelist.Rows[row]["DRAFT_INVOICE_DATE"].ToString():"");
            cell10[13] = new TableCell();
            cell10[13].Text = (dtInvoicelist.Rows[row]["FINAL_INVOICE_DATE"]!=null?dtInvoicelist.Rows[row]["FINAL_INVOICE_DATE"].ToString():"");

            AddStyle(cell10, false);
            tblRow10.Cells.AddRange(cell10);
            tbl.Rows.Add(tblRow10);

        }

        ExportToExcel(strFileName, tbl);
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
    #endregion
}
#endregion
