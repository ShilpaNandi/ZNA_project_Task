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

public partial class Invoice_InvoicingMailingDtls : AISBasePage
{

    //private IList<AccountBE> dtDDLSource;
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ddlAcctlist.OnSelectedIndexChanged += new EventHandler(ddlAccountlist_SelectedIndexChanged);
        ((ScriptManager)(Master.FindControl("ScriptManager1"))).AsyncPostBackTimeout = 9000;
        this.Page.Title = "Invoice Mailing Details";
        /*this.dtDDLSource = (new AccountBS()).getAccounts();
        if (!IsPostBack)
        {
            var result = (from acct in dtDDLSource
                          orderby acct.FULL_NM
                          select new AccountBE
                          {
                              FULL_NM = acct.FULL_NM,
                              CUSTMR_ID = acct.CUSTMR_ID
                          }).ToList();
            this.ddlAccountName.DataSource = result;
            this.ddlAccountName.DataTextField = "FULL_NM";
            this.ddlAccountName.DataValueField = "CUSTMR_ID";
            this.ddlAccountName.DataBind();
            this.ddlAccountName.Items.Insert(0, "(Select)");

        }
       */
        //Comment below line Incase dont want to use TransactionWrapper
        IMDTransaction = new AISTransactionWrapper();

        //Checks Exiting Without Save
        CheckExitWithoutSave();
    }

    private void CheckExitWithoutSave()
    {
        ArrayList list = new ArrayList();
        list.Add(txtComments);
        list.Add(txtDraftInvEmailed);
        list.Add(txtFinalEmailDate);
        list.Add(txtFinalInvEmailed);
        list.Add(txtUwRespDt);
        list.Add(CalendarUwRespDt);
        list.Add(CalendarDraftInvEmailed);
        list.Add(CalendarFinalInvEmailed);
        list.Add(CalendarFinalInvEmailed);
        list.Add(ddlAcctlist);
        list.Add(ddlUWResponse);
        list.Add(btnClear);
        list.Add(btnFinal);
        list.Add(btnQueryInvoice);
        list.Add(btnSave);
        list.Add(lbCloseDetails);
        ProcessExitFlag(list);
    }
    #endregion


    #region Transaction Wrapper Property
    /// <summary>
    /// property to hold an instance for  Transaction Wrapper
    /// </summary>
    /// <param name=""></param>
    /// <returns>AISTransactionWrapper property</returns>
    private AISTransactionWrapper IMDTransaction
    {
        get
        {
            if (Session["IMDTransaction"] == null)
                Session["IMDTransaction"] = new AISTransactionWrapper();
            return (AISTransactionWrapper)Session["IMDTransaction"];
        }
        set
        {
            Session["IMDTransaction"] = value;
        }
    }
    #endregion

    #region Invoice Mailing Dtls Business Service Property
    /// <summary>
    /// a property for Invoice Mailing Dtls Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>InvoiceMailingDtlsBS</returns>
    private InvoiceMailingDtlsBS _invmailingBS;
    InvoiceMailingDtlsBS invmailingBS
    {
        get
        {
            if (_invmailingBS == null)
            {
                _invmailingBS = new InvoiceMailingDtlsBS();
                //Comment below line Incase dont want to use TransactionWrapper
                _invmailingBS.AppTransactionWrapper = IMDTransaction;

            }
            return _invmailingBS;
        }
    }
    #endregion


    #region Query Invoice
    protected void btnQueryInvoice_Click(object sender, EventArgs e)
    {
        // On click of Query Invoice button calling Bind Data method
        pnlDetails.Visible = false;
        lblInvoicingDetails.Visible = false;
        lbCloseDetails.Visible = false;
        pnlInvoiveDetails.Visible = true;
        BindData();

    }
    #endregion


    #region Bind Data

    public void BindData()
    {
        string InvoiceDateFrom = (txtInvoiceDateFrom.Text.ToString());
        string InvoiceDateTo = (txtInvoiceDateTo.Text.ToString());
        string ValuationDateFrom = (txtValuationDateFrom.Text.ToString());
        string ValuationDateTo = (txtValuationDateTo.Text.ToString());
        string strMessage = string.Empty;
        string invnbr = string.Empty;
        DropDownList ddMastActAcctlst = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        if (ddMastActAcctlst.SelectedIndex <= 0)
        {
            ddMastActAcctlst.SelectedIndex = 0;
        }
        if (txtInvoiceNbr.Text.Trim() != "")
        {
            invnbr = txtInvoiceNbr.Text.ToString().Trim();
        }
        ClearError();
        if (InvoiceDateFrom != "" && InvoiceDateTo == "")
        {
            ShowError("Please enter Invoice Date To");
            lsvInvoiceMailing.DataSource = null;
            lsvInvoiceMailing.DataBind();
            return;
        }
        ClearError();
        if (InvoiceDateFrom == "" && InvoiceDateTo != "")
        {
            ShowError("Please enter Invoice Date From");
            lsvInvoiceMailing.DataSource = null;
            lsvInvoiceMailing.DataBind();
            return;
        }
        ClearError();
        if (ValuationDateFrom != "" && ValuationDateTo == "")
        {
            ShowError("Please enter Valuation Date To");
            lsvInvoiceMailing.DataSource = null;
            lsvInvoiceMailing.DataBind();
            return;
        }
        ClearError();
        if (ValuationDateFrom == "" && ValuationDateTo != "")
        {
            ShowError("Please enter Valuation Date From");
            lsvInvoiceMailing.DataSource = null;
            lsvInvoiceMailing.DataBind();
            return;
        }
        ClearError();
        if ((invnbr.Length != 15 && invnbr.Length != 0) && (ddMastActAcctlst.SelectedValue == "" || ddMastActAcctlst.SelectedValue == "0"))
        {
            ShowError("Please select Account");
            lsvInvoiceMailing.DataSource = null;
            lsvInvoiceMailing.DataBind();
            return;
        }
        if (((InvoiceDateFrom != "" && InvoiceDateTo == "") || (InvoiceDateFrom == "" && InvoiceDateTo != "")) && (ddMastActAcctlst.SelectedValue == "" || ddMastActAcctlst.SelectedValue == "0"))
        {

            ShowError("Please select Account");
            lsvInvoiceMailing.DataSource = null;
            lsvInvoiceMailing.DataBind();
            return;
        }
        if (((ValuationDateFrom != "" && ValuationDateTo == "") || (ValuationDateFrom == "" && ValuationDateTo != "")) && (ddMastActAcctlst.SelectedValue == "" || ddMastActAcctlst.SelectedValue == "0"))
        {

            ShowError("Please select Account");
            lsvInvoiceMailing.DataSource = null;
            lsvInvoiceMailing.DataBind();
            return;
        }
        if ((ddMastActAcctlst.SelectedValue == "" || ddMastActAcctlst.SelectedValue == "0") && (invnbr == "" && InvoiceDateFrom == "" && InvoiceDateTo == "" && ValuationDateFrom == "" && ValuationDateTo == ""))
        {
            ShowError("Please select Account");
            lsvInvoiceMailing.DataSource = null;
            lsvInvoiceMailing.DataBind();
            return;
        }
        int custmrID;
        if (ddMastActAcctlst.SelectedItem.Value != null)
        {
            custmrID = int.Parse(ddMastActAcctlst.SelectedItem.Value.ToString());
        }
        else
        {
            custmrID = 0;
        }
        //Binding the listview based on passed parameters
        InvoiceMailingDtlsBS invmailingBS = new InvoiceMailingDtlsBS();
        invmailingBEList = invmailingBS.getInvMailingData(custmrID, invnbr, ValuationDateFrom, ValuationDateTo, InvoiceDateFrom, InvoiceDateTo);
        lsvInvoiceMailing.DataSource = invmailingBEList;
        lsvInvoiceMailing.DataBind();


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
        lsvInvoiceMailing.Items.Clear();
        IList<InvoiceMailingDtlsBE> invmailingListBE = new List<InvoiceMailingDtlsBE>();
        //Binding Empty Data
        lsvInvoiceMailing.DataSource = invmailingListBE;
        lsvInvoiceMailing.DataBind();


    }
    #endregion
    #region Clear button Click
    protected void btnClear_Click(object sender, EventArgs e)
    {
        //On click of Clear button setting the controls Empty.
        txtInvoiceDateFrom.Text = String.Empty;
        txtInvoiceDateTo.Text = String.Empty;
        txtValuationDateFrom.Text = String.Empty;
        txtValuationDateTo.Text = String.Empty;
        //ddlAccountName.SelectedIndex = 0;
        txtInvoiceNbr.Text = String.Empty;
        DropDownList ddMastActAcctlst = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        ddMastActAcctlst.Items.Clear();
        pnlDetails.Visible = false;
        lblInvoicingDetails.Visible = false;
        lbCloseDetails.Visible = false;
        lsvInvoiceMailing.Visible = false;
    }
    #endregion

    #region List view Index Change

    protected void lsvInvoiceMailing__SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
    {
        ClearFileds();
        HtmlTableRow tr;
        //code for changing the previous selected row color to its original Color
        if (ViewState["SelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["SelectedIndex"]);
            if (lsvInvoiceMailing.Items.Count > index)
            {
                tr = (HtmlTableRow)lsvInvoiceMailing.Items[index].FindControl("trItemTemplate");
                tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
            }
        }
        tr = (HtmlTableRow)lsvInvoiceMailing.Items[e.NewSelectedIndex].FindControl("trItemTemplate");
        LinkButton lnk = (LinkButton)lsvInvoiceMailing.Items[e.NewSelectedIndex].FindControl("lnkSelect");
        ViewState["SelectedIndex"] = e.NewSelectedIndex;
        //code for changing the selected row style to highlighted
        tr.Attributes["class"] = "SelectedItemTemplate";

        int intID = Convert.ToInt32(lnk.CommandArgument);
        SelectedValue = intID;
        //function call to display the selected Exc Non Billable Details
        HiddenField hidId = (HiddenField)lsvInvoiceMailing.Items[e.NewSelectedIndex].FindControl("HidID");
        Session["ID"] = intID;
        //Funtion to Display the details of the selected Exc Non Billable
        DisplayDetails(intID);
        ChangeDetailState(true);
        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            pnlDetails.Enabled = true;
        pnlDetails.Visible = true;
        lblInvoicingDetails.Visible = true;
        lbCloseDetails.Visible = true;
    }
    #endregion

    #region Clear Fields
    /// <summary>
    /// Clears all control values
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    public void ClearFileds()
    {
        lblBroker.Text = String.Empty;
        lblBU.Text = String.Empty;
        lblDateAdjusted.Text = String.Empty;
        txtComments.Text = String.Empty;
        txtDraftInvEmailed.Text = String.Empty;
        txtFinalInvEmailed.Text = String.Empty;
        txtFinalInvEmailed.Text = String.Empty;
        //ddlUWResponse.SelectedIndex = -1;
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

        }

    }
    #endregion
    /// <summary>
    /// Protected Property for storing Selectedvalue of Internal contacts list
    /// </summary>
    /// <param name=""></param>
    /// <returns>integer Selected value</returns>
    protected int SelectedValue
    {
        get
        {
            return (hidindex.Value != null ? Convert.ToInt32(hidindex.Value) : -1);
        }
        set { hidindex.Value = value.ToString(); }
    }
    /// <summary>
    /// a property for InvoiceMailingDtls Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>InvoiceMailingDtlsBE</returns>
    private InvoiceMailingDtlsBE invmailingBE
    {
        get { return (InvoiceMailingDtlsBE)Session["invmailingBE"]; }
        set { Session["invmailingBE"] = value; }
    }
    private IList<InvoiceMailingDtlsBE> invmailingBEList
    {
        get { return (IList<InvoiceMailingDtlsBE>)Session["invmailingBEList"]; }
        set { Session["invmailingBEList"] = value; }
    }

    #region Display Data
    /// <summary>
    /// Function to display the selected Mailing Info details
    /// </summary>
    /// <param name="intID">intID of the selected record in the listview</param>
    /// <returns></returns>
    public void DisplayDetails(int intID)
    {
        ClearError();
        Session["AdjID"] = intID;
        string strPremAdjSts = new PremiumAdjustmentStatusBS().getPremiumAdjstmentStatus(intID);
        //Fetch the value of Prem_adj_sts_id from prem_adj table.
        //string strPremAdjStsOld = new PremiumAdjustmentStatusBS().getPremiumAdjstmentStatusOld(intID);
        ViewState["Value"] = strPremAdjSts;
        //InvoiceMailingDtlsBS invmailingBS = new InvoiceMailingDtlsBS();
        invmailingBE = invmailingBS.getInvMailingDataRow(intID);
        Session["PrmAdjStsID"] = invmailingBE.ADJ_STS_ID;
        lblBroker.Text = invmailingBE.BROKER;
        lblBU.Text = invmailingBE.BUOFC;
        //Below condition needs to chk when we will start storing null in Prem_adj_sts table for Prem_adj_sts_id
        //if (strPremAdjStsOld == "351" && strPremAdjSts == null)
        if (strPremAdjSts == "UW REVIEW" || strPremAdjSts == "CALC" || strPremAdjSts == "FINAL INVOICE")
        {
            txtComments.Text = invmailingBE.CMMNT_TXT;
        }
        else
        {
            txtComments.Text = String.Empty;
        }
        //showing Approve in UW response dropdown if Status is UW Review or Final Review
        String strUWResp = String.Empty;
        //9557 and9653
        String strUWRespNotReq = String.Empty;
        if (invmailingBE.UW_RESP != null)
        {
            //9557 and9653
            if (invmailingBE.UW_NOT_REQ == null)
            {
                strUWRespNotReq = "";
            }
            else
            {
                strUWRespNotReq = invmailingBE.UW_NOT_REQ.ToString();
            }
            if (invmailingBE.UW_RESP.ToString() == "UW REVIEW" && (strUWRespNotReq == "False"))
            {
                strUWResp = "APPROVE";
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                    btnFinal.Enabled = true;
            }
            else if (invmailingBE.UW_RESP.ToString() == "UW REVIEW" && (strUWRespNotReq == "True"))
            {
                strUWResp = "NOT REQUIRED";
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                    btnFinal.Enabled = true;
            }
            //End
            else
            {
                if (invmailingBE.UW_RESP.ToString() == "FINAL INVOICE" && (strUWRespNotReq == "False"))
                    strUWResp = "APPROVE";
                else if (invmailingBE.UW_RESP.ToString() == "FINAL INVOICE" && (strUWRespNotReq == "True"))
                    strUWResp = "NOT REQUIRED";
                else if (invmailingBE.UW_RESP.ToString() == "CALC" && invmailingBE.Historical !=true)
                    strUWResp = "REJECT";
                else
                    strUWResp = "(Select)";
                btnFinal.Enabled = false;
            }
        }
        else
        {
            strUWResp = "(Select)";
            btnFinal.Enabled = false;
        }


        ddlUWResponse.DataSourceID = "objDataSourceUWResp";
        ddlUWResponse.DataBind();
        //        ddlUWResponse.Items.FindByText(strUWResp).Selected = true;
        AddInActiveLookupDataByText(ref ddlUWResponse, strUWResp);

        lblDateAdjusted.Text = invmailingBE.CREATEDATE.ToShortDateString();
        if (invmailingBE.DRAFT_MAILED_UW_DT.ToString() != null)
        {
            txtDraftInvEmailed.Text = invmailingBE.DRAFT_MAILED_UW_DT.ToString();
        }
        else
        {
            txtDraftInvEmailed.Text = "";
        }
        if (invmailingBE.FINAL_MAILED_UW_DT.ToString() != null)
        {
            txtFinalInvEmailed.Text = invmailingBE.FINAL_MAILED_UW_DT.ToString();
        }
        else
        {
            txtFinalInvEmailed.Text = "";
        }
        if (invmailingBE.FINAL_EMAIL_DT.ToString() != null)
        {
            txtFinalEmailDate.Text = invmailingBE.FINAL_EMAIL_DT.ToString();
        }
        else
        {
            txtFinalEmailDate.Text = "";
        }

        if (invmailingBE.UW_RESP_DT.ToString() != null)
        {
            if (strPremAdjSts == "UW REVIEW" || strPremAdjSts == "CALC" || strPremAdjSts == "FINAL INVOICE")
            {
                txtUwRespDt.Text = invmailingBE.UW_RESP_DT.ToString();
            }
            else
            {
                txtUwRespDt.Text = "";
            }
        }
        else
        {
            txtUwRespDt.Text = "";
        }
        if (ViewState["Value"] != null)
        {
            if (ViewState["Value"].ToString() == GlobalConstants.AdjustmentStatus.FinalInvd)
            {
                //Enabling the controls if the adjustment status is Final Invoivce
                txtComments.ReadOnly = true;
                txtDraftInvEmailed.Enabled = false;
                ddlUWResponse.Enabled = false;
                txtUwRespDt.Enabled = false;
                imgDraftInvEmailed.Enabled = false;
                imgUwRespDt.Enabled = false;
                txtFinalEmailDate.Enabled = true;
                txtFinalInvEmailed.Enabled = true;
                imgFinalEmailDate.Enabled = true;
                imgFinalInvEmailed.Enabled = true;
                btnFinal.Enabled = false;
                btnSave.Enabled = true;

            }
            else
            {
                txtFinalEmailDate.Enabled = false;
                txtFinalInvEmailed.Enabled = false;
                imgFinalEmailDate.Enabled = false;
                imgFinalInvEmailed.Enabled = false;
                txtComments.ReadOnly = false;
                txtDraftInvEmailed.Enabled = true;
                ddlUWResponse.Enabled = true;
                txtUwRespDt.Enabled = true;
                imgDraftInvEmailed.Enabled = true;
                imgUwRespDt.Enabled = true;
            }
        }
        else
        {
            txtFinalEmailDate.Enabled = false;
            txtFinalInvEmailed.Enabled = false;
            imgFinalEmailDate.Enabled = false;
            imgFinalInvEmailed.Enabled = false;
            txtComments.ReadOnly = false;
            txtDraftInvEmailed.Enabled = true;
            ddlUWResponse.Enabled = true;
            txtUwRespDt.Enabled = true;
            imgDraftInvEmailed.Enabled = true;
            imgUwRespDt.Enabled = true;
        }

        if (invmailingBE.UW_RESP != null)
        {
            if (invmailingBE.UW_RESP.ToString() == "UW REVIEW")
            {
                txtComments.ReadOnly = true;
                txtDraftInvEmailed.Enabled = true;
                ddlUWResponse.Enabled = false;
                txtUwRespDt.Enabled = false;
                imgDraftInvEmailed.Enabled = true;
                imgUwRespDt.Enabled = false;
                txtFinalEmailDate.Enabled = false;
                txtFinalInvEmailed.Enabled = false;
                imgFinalEmailDate.Enabled = false;
                imgFinalInvEmailed.Enabled = false;
            }
            if (invmailingBE.UW_RESP.ToString() == "CALC")
            {
                txtComments.ReadOnly = true;
                txtDraftInvEmailed.Enabled = false;
                ddlUWResponse.Enabled = false;
                txtUwRespDt.Enabled = false;
                imgDraftInvEmailed.Enabled = false;
                imgUwRespDt.Enabled = false;
                btnFinal.Enabled = false;
                btnSave.Enabled = false;
            }
        }
        if (invmailingBE.HistoricalInd == true && (invmailingBE.UW_RESP.ToString() != "FINAL INVOICE" && invmailingBE.UW_RESP.ToString() != "TRANSMITTED"))
        {
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            {
                btnFinal.Enabled = true;
                //ddlUWResponse.SelectedIndex = -1;
            }
        }
        else if (invmailingBE.HistoricalInd == true && invmailingBE.UW_RESP.ToString() == "FINAL INVOICE" && invmailingBE.UW_NOT_REQ != true)
        {
            ddlUWResponse.SelectedIndex = -1;
        }
    }

    #endregion

    #region Save Data

    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool transaction = false;
        // InvoiceMailingDtlsBS invmailingBS = new InvoiceMailingDtlsBS();
        InvoiceMailingDtlsBE IMBE = invmailingBS.getInvoiceMailingDtlsRow(Convert.ToInt32(Session["AdjID"].ToString()));
        //Concurrency Issue
        if (ViewState["Value"] != null)
        {
            InvoiceMailingDtlsBE IMBEold = (invmailingBEList.Where(o => o.PREM_ADJ_ID.Equals(Convert.ToInt32(Session["AdjID"].ToString())))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(IMBEold.UPDATEDATE), Convert.ToDateTime(IMBE.UPDATEDATE));
            if (!con)
                return;
        }
        //End
        if (ViewState["Value"] != null)
        {
            if (ViewState["Value"].ToString() == GlobalConstants.AdjustmentStatus.UWReviewed || ViewState["Value"].ToString() == GlobalConstants.AdjustmentStatus.FinalInvd)
            {
                //New
                if (txtDraftInvEmailed.Text.Trim() != "")
                {
                    IMBE.DRAFT_MAILED_UW_DT = DateTime.Parse(txtDraftInvEmailed.Text.ToString());
                }
                else
                {
                    IMBE.DRAFT_MAILED_UW_DT = null;
                }
                //End
                if (txtFinalEmailDate.Text.Trim() != "")
                {
                    IMBE.FINAL_EMAIL_DT = DateTime.Parse(txtFinalEmailDate.Text.ToString());
                }
                else
                {
                    IMBE.FINAL_EMAIL_DT = null;
                }
                if (txtFinalInvEmailed.Text.Trim() != "")
                {
                    IMBE.FINAL_MAILED_UW_DT = DateTime.Parse(txtFinalInvEmailed.Text.ToString());
                }
                else
                {
                    IMBE.FINAL_MAILED_UW_DT = null;
                }
                IMBE.UPDATEDATE = System.DateTime.Now;
                IMBE.UPDATEUSERID = CurrentAISUser.PersonID;
                bool Flag = invmailingBS.Update(IMBE);
                ShowConcurrentConflict(Flag, IMBE.ErrorMessage);
                IMDTransaction.SubmitTransactionChanges();
            }

            else
            {
                if (txtDraftInvEmailed.Text.Trim() != "")
                {
                    IMBE.DRAFT_MAILED_UW_DT = DateTime.Parse(txtDraftInvEmailed.Text.ToString());
                }
                else
                {
                    IMBE.DRAFT_MAILED_UW_DT = null;
                }
                IMBE.UPDATEDATE = System.DateTime.Now;
                IMBE.UPDATEUSERID = CurrentAISUser.PersonID;
                //9557 and 9653
                if (ddlUWResponse.SelectedItem.Value.ToString() == "NOT REQUIRED")
                    IMBE.UW_NOT_REQ = true;
                else
                    IMBE.UW_NOT_REQ = false;
                //End
                try
                {
                    //invmailingBS.Update(IMBE);
                    bool invmdtls = (new InvoiceMailingDtlsBS()).Update(IMBE);
                    ShowConcurrentConflict(invmdtls, IMBE.ErrorMessage);
                    PremiumAdjustmentStatusBS prmadjstsBS = new PremiumAdjustmentStatusBS();
                    PremiumAdjustmentStatusBE prmadjstsBEold = prmadjstsBS.getPreAdjStatusRow(Convert.ToInt32(Session["PrmAdjStsID"].ToString()));
                    PremiumAdjustmentStatusBE prmadjstsBE = new PremiumAdjustmentStatusBE();
                    prmadjstsBE.PremumAdj_ID = prmadjstsBEold.PremumAdj_ID;
                    prmadjstsBE.CustomerID = prmadjstsBEold.CustomerID;
                    prmadjstsBE.CommentText = txtComments.Text;
                    if (txtUwRespDt.Text != "")
                        prmadjstsBE.Review_Cmplt_Date = DateTime.Parse(txtUwRespDt.Text.ToString());
                    if (ddlUWResponse.SelectedItem.Value.ToString() == "APPROVE")
                    {
                        prmadjstsBE.ADJ_STS_TYP_ID = invmailingBE.UW_REV_ID;
                        prmadjstsBEold.EXPIRYDATE = System.DateTime.Now;
                        prmadjstsBE.EffectiveDate = DateTime.Now;
                        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                            btnFinal.Enabled = true;
                        PremiumAdjustmentBE premAdjBE = (new PremAdjustmentBS()).getPremiumAdjustmentRow(prmadjstsBEold.PremumAdj_ID);
                        premAdjBE.ADJ_STS_TYP_ID = 353;//U/W Reviw
                        premAdjBE.ADJ_STS_EFF_DT = DateTime.Now;
                        premAdjBE.UPDT_DT = DateTime.Now;
                        premAdjBE.UPDT_USER_ID = CurrentAISUser.PersonID;
                        bool Flag = (new PremAdjustmentBS()).Update(premAdjBE);
                        ShowConcurrentConflict(Flag, premAdjBE.ErrorMessage);
                        prmadjstsBE.APPROVEINDICATOR = true;
                        prmadjstsBE.CreatedDate = System.DateTime.Now;
                        prmadjstsBE.CreatedUser_ID = CurrentAISUser.PersonID;
                        bool boolFlag = prmadjstsBS.Update(prmadjstsBEold);
                        ShowConcurrentConflict(boolFlag, prmadjstsBEold.ErrorMessage);
                        bool boolPremadjsts = prmadjstsBS.Update(prmadjstsBE);
                        ShowConcurrentConflict(boolPremadjsts, prmadjstsBE.ErrorMessage);
                        IMDTransaction.SubmitTransactionChanges();
                        transaction = IMDTransaction.SubmitTransactionChanges();

                    }
                    else if (ddlUWResponse.SelectedItem.Value.ToString() == "REJECT")
                    {
                        prmadjstsBE.ADJ_STS_TYP_ID = invmailingBE.CALC_ID;
                        prmadjstsBEold.EXPIRYDATE = System.DateTime.Now;
                        prmadjstsBE.EffectiveDate = DateTime.Now;
                        btnFinal.Enabled = false;

                        //code to set Calculation Adjustment Status Code for the rejected adjustment to a blank
                        PremiumAdjustmentBE PremAdjBE = (new PremAdjustmentBS()).getPremiumAdjustmentRow(prmadjstsBEold.PremumAdj_ID);
                        PremAdjBE.CALC_ADJ_STS_CODE = "";
                        PremAdjBE.ADJ_STS_TYP_ID = 346;//CALC
                        PremAdjBE.ADJ_STS_EFF_DT = DateTime.Now;
                        bool Flag = (new PremAdjustmentBS()).Update(PremAdjBE);
                        ShowConcurrentConflict(Flag, PremAdjBE.ErrorMessage);
                        prmadjstsBE.APPROVEINDICATOR = false;
                        prmadjstsBE.CreatedDate = System.DateTime.Now;
                        prmadjstsBE.CreatedUser_ID = CurrentAISUser.PersonID;
                        bool bFlag = prmadjstsBS.Update(prmadjstsBEold);
                        ShowConcurrentConflict(bFlag, prmadjstsBEold.ErrorMessage);
                        bool boolstats = prmadjstsBS.Update(prmadjstsBE);
                        ShowConcurrentConflict(boolstats, prmadjstsBE.ErrorMessage);
                        IMDTransaction.SubmitTransactionChanges();
                        transaction = IMDTransaction.SubmitTransactionChanges();
                    }
                    //9557 and 9653
                    else if (ddlUWResponse.SelectedItem.Value.ToString() == "NOT REQUIRED")
                    {
                        prmadjstsBE.ADJ_STS_TYP_ID = invmailingBE.UW_REV_ID;
                        prmadjstsBEold.EXPIRYDATE = System.DateTime.Now;
                        prmadjstsBE.EffectiveDate = DateTime.Now;
                        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                            btnFinal.Enabled = true;
                        PremiumAdjustmentBE premAdjBE = (new PremAdjustmentBS()).getPremiumAdjustmentRow(prmadjstsBEold.PremumAdj_ID);
                        premAdjBE.ADJ_STS_TYP_ID = 353;//U/W Reviw
                        premAdjBE.ADJ_STS_EFF_DT = DateTime.Now;
                        premAdjBE.UPDT_DT = DateTime.Now;
                        premAdjBE.UPDT_USER_ID = CurrentAISUser.PersonID;
                        bool Flag = (new PremAdjustmentBS()).Update(premAdjBE);
                        prmadjstsBE.APPROVEINDICATOR = true;
                        prmadjstsBE.CreatedDate = System.DateTime.Now;
                        prmadjstsBE.CreatedUser_ID = CurrentAISUser.PersonID;
                        bool boolPremadjold = prmadjstsBS.Update(prmadjstsBEold);
                        ShowConcurrentConflict(boolPremadjold, prmadjstsBEold.ErrorMessage);
                        bool boolPremstsnew = prmadjstsBS.Update(prmadjstsBE);
                        ShowConcurrentConflict(boolPremstsnew, prmadjstsBE.ErrorMessage);
                        IMDTransaction.SubmitTransactionChanges();
                        transaction = IMDTransaction.SubmitTransactionChanges();
                    }
                    //End
                    else
                    {

                        //prmadjstsBE.ADJ_STS_TYP_ID = null;
                        //prmadjstsBE.EffectiveDate = DateTime.Now;
                        //prmadjstsBE.CreatedDate = System.DateTime.Now;
                        //prmadjstsBE.CreatedUser_ID = CurrentAISUser.PersonID;
                        //prmadjstsBS.Update(prmadjstsBEold);
                        //prmadjstsBS.Update(prmadjstsBE);
                        IMDTransaction.SubmitTransactionChanges();
                        transaction = IMDTransaction.SubmitTransactionChanges();
                        //ShowError("Please select U/W Response");
                        //return;
                    }

                }

                catch (Exception ex)
                {
                    // Transaction Rollback 
                    //Comment below line Incase dont want to use TransactionWrapper
                    IMDTransaction.RollbackChanges();
                    ShowError(ex.Message, ex);
                }
            }
        }
        else
        {
            //This needs to be implemented when we will start storing null as Prem_adj_sts_id in Prem_adj_sts table
            //if (txtDraftInvEmailed.Text.Trim() != "")
            //{
            //    IMBE.DRAFT_MAILED_UW_DT = DateTime.Parse(txtDraftInvEmailed.Text.ToString());
            //}
            //else
            //{
            //    IMBE.DRAFT_MAILED_UW_DT = null;
            //}
            //IMBE.UPDATEDATE = System.DateTime.Now;
            //IMBE.UPDATEUSERID = CurrentAISUser.PersonID;
            //PremiumAdjustmentStatusBS prmadjstsBS = new PremiumAdjustmentStatusBS();
            //PremiumAdjustmentStatusBE prmadjstsBEold = prmadjstsBS.getPreAdjStatusRow(Convert.ToInt32(Session["PrmAdjStsID"].ToString()));
            //PremiumAdjustmentStatusBE prmadjstsBE = new PremiumAdjustmentStatusBE();
            //prmadjstsBE.ADJ_STS_TYP_ID = null;
            //prmadjstsBE.EffectiveDate = DateTime.Now;
            //prmadjstsBS.Update(prmadjstsBEold);


        }
        //if (btnFinal.Enabled == false)
        //{
        //    ClearFileds();
        //    pnlDetails.Visible = false;
        //    lblInvoicingDetails.Visible = false;
        //    lbCloseDetails.Visible = false;
        //}

        BindData();
        if (lsvInvoiceMailing.Items.Count > 0)
        {
            LinkButton lnkSelect;
            HtmlTableRow tr;
            int selectedindex = 0;
            for (int i = 0; i < lsvInvoiceMailing.Items.Count; i++)
            {
                lnkSelect = (LinkButton)lsvInvoiceMailing.Items[i].FindControl("lnkSelect");
                if (Convert.ToInt32(lnkSelect.CommandArgument) == Convert.ToInt32(Session["AdjID"].ToString()))
                {
                    selectedindex = i;
                    break;
                }
            }
            if (selectedindex >= 0)
            {
                tr = (HtmlTableRow)lsvInvoiceMailing.Items[selectedindex].FindControl("trItemTemplate");
                tr.Attributes["class"] = "SelectedItemTemplate";
                ViewState["SelectedIndex"] = selectedindex;
            }
        }
        DisplayDetails(Convert.ToInt32(Session["AdjID"].ToString()));
        //if (transaction == true)
        //{
        ShowError("Data entry has been saved");
        return;
        //}
    }
    #endregion

    #region Final Data
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
    /// Function to Change the lisview size and other changes required when user clicks on the Details Link
    /// </summary>
    /// <param name="Flag"></param>
    #region ChangeDetailsState
    private void ChangeDetailState(bool Flag)
    {
        lbCloseDetails.Visible = Flag;
        lsvInvoiceMailing.Enabled = !Flag;
        pnlButtons.Enabled = !Flag;
        pnlSearchParams.Enabled = !Flag;
        lblInvoicingDetails.Visible = Flag;
        pnlDetails.Visible = Flag;
        pnlInvoiveDetails.Height = (Flag == true ? Unit.Point(60) : Unit.Point(200));
    }
    #endregion
    protected void btnFinal_Click(object sender, EventArgs e)
    {
        //ClearFileds();
        //pnlDetails.Visible = false;
        //lblInvoicingDetails.Visible = false;
        //lbCloseDetails.Visible = false;
        string strMessage = string.Empty;
        AdjCalc_InvoicingDashboard objID = new AdjCalc_InvoicingDashboard();
        PremAdjustmentBS objInvBS = new PremAdjustmentBS();
        PremiumAdjustmentBE objInvBE = new PremiumAdjustmentBE();
        PremiumAdjustmentBE objInvPreviousBE = new PremiumAdjustmentBE();
        objInvBE = objInvBS.getPremiumAdjustmentRow(Convert.ToInt32(Session["AdjID"].ToString()));
        bool HistFlag = false;
        if (objInvBE.HISTORICAL_ADJ_IND.HasValue)
        {
            HistFlag = Convert.ToBoolean(objInvBE.HISTORICAL_ADJ_IND);
        }
        if (!objInvBE.REL_PREM_ADJ_ID.HasValue)
        {
            strMessage = objID.generateFinalInvoice(Convert.ToInt32(Session["AdjID"].ToString()), txtComments.Text.Trim(), HistFlag);
        }
        else
        {
            objInvPreviousBE = objInvBS.getPremiumAdjustmentRow(Convert.ToInt32(objInvBE.REL_PREM_ADJ_ID));
            if (objInvPreviousBE.ADJ_RRSN_IND == true)
                strMessage = objID.generateRevisedFinalInvoice(Convert.ToInt32(Session["AdjID"].ToString()), objInvPreviousBE.PREMIUM_ADJ_ID, txtComments.Text.Trim(), HistFlag);
            else
            {
                ShowError("Final Invoice can not be done for void adjustment");
                return;
            }

        }
        BindData();
        //Two Show the Selected Record in Listview
        LinkButton lnkSelect;
        HtmlTableRow tr;
        int selectedindex = 0;
        for (int i = 0; i < lsvInvoiceMailing.Items.Count; i++)
        {
            lnkSelect = (LinkButton)lsvInvoiceMailing.Items[i].FindControl("lnkSelect");
            if (Convert.ToInt32(lnkSelect.CommandArgument) == Convert.ToInt32(Session["AdjID"].ToString()))
            {
                selectedindex = i;
                break;
            }
        }
        tr = (HtmlTableRow)lsvInvoiceMailing.Items[selectedindex].FindControl("trItemTemplate");
        tr.Attributes["class"] = "SelectedItemTemplate";
        ViewState["SelectedIndex"] = selectedindex;

        DisplayDetails(Convert.ToInt32(Session["AdjID"].ToString()));

        ShowError(strMessage);
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
}
