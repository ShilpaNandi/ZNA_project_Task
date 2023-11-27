using System;
using System.Collections;
using System.Collections.Generic;
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
using ZurichNA.AIS.ExceptionHandling;
using System.Threading;
using ZurichNA.AIS.WebSite.Reports;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using ZurichNA.LSP.Framework;
using System.Text;
using System.Text.RegularExpressions;
using ZurichNA.LSP.Framework.Business;
using System.Web.SessionState;
using ZurichNA.AIS.WebSite.ZDWJavaWS;
using Microsoft.Web.Services3;
using System.Web.Services;
using Microsoft.Web.Services3.Security.Tokens;
using System.Transactions;
//Importing different AIS framework namespaces for Invoicing Dashboard screen
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.DAL.LINQ;

public partial class AdjCalc_InvoicingDashboard : AISBasePage
{
    /// <summary>
    /// Global Varaibles Declaration
    /// </summary>
    #region Variable Declaration Section
    protected static Common common = null;
    public string strExportFinalFile = string.Empty;
    //MemoryStream oStream = null;
    string strInvNo = string.Empty;
    byte[] byteArray = null;
    System.Data.Common.DbTransaction trans = null;
    AISDatabaseLINQDataContext objDC = new AISDatabaseLINQDataContext(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
    #endregion

    /// <summary>
    /// Page load Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ddlAcctlist.OnSelectedIndexChanged += new EventHandler(ddlAcctlist_OnSelectedIndexChanged);
        ((ScriptManager)(Master.FindControl("ScriptManager1"))).AsyncPostBackTimeout = 9000;
        this.Master.Page.Title = "Invoicing Dashboard";
        objDC.CommandTimeout = 180000;
        if (!IsPostBack)
        {

            ListItem item = new ListItem("(Select)", "0");
            ddlAdjNumList.Items.Insert(0, item);
            ddlValDateList.Items.Insert(0, item);
            if (Request.QueryString.Count > 0)
            {
                if (AISMasterEntities != null)
                    //ddlAcctlist.selectedAccountName = Request.QueryString["AcctNm"].ToString();
                    ddlAcctlist.selectedAccountName = AISMasterEntities.AccountName;
                ddlAcctlist.selectedAccountNo = int.Parse(Request.QueryString["AcctNo"].ToString());
                //Function to fill the adj nos for the selected Account
                FillAdjNos(Convert.ToInt32(Request.QueryString["AcctNo"]));
                string str = ddlValDateList.SelectedValue != "0" ? ddlValDateList.SelectedItem.Text : string.Empty;
                int adjno = 0;
                if (AISMasterEntities != null)
                    adjno = AISMasterEntities.AdjusmentNumber;
                ddlAdjNumList.SelectedIndex = -1;

                if (adjno > 0)
                {
                    ListItem li = new ListItem(adjno.ToString(), adjno.ToString());
                    if (ddlAdjNumList.Items.Contains(li))
                        ddlAdjNumList.Items.FindByText(adjno.ToString()).Selected = true;
                }
                AdjInvDashlistview.DataSource = (new ProgramPeriodsBS()).GetProgramPeriodSearchDashboard(Convert.ToInt32(Request.QueryString["AcctNo"].ToString()), 0, adjno, str);
                AdjInvDashlistview.DataBind();
                for (int i = 0; i < AdjInvDashlistview.Items.Count; i++)
                {
                    CheckBox chk = (CheckBox)AdjInvDashlistview.Items[i].FindControl("chkSelect");
                    if (chk != null && !chk.Checked)
                    {
                        chk.Checked = true;
                    }
                }
                if (AdjInvDashlistview.Items.Count > 0)
                {
                    btnCalculate.Visible = true;
                    if (ddlAdjNumList.SelectedIndex > 0)
                        btnDraft.Visible = true;
                }
                AccountBE acct = (new AccountBS()).getAccount(Convert.ToInt32(Request.QueryString["AcctNo"].ToString()));
                AISMasterEntities = new MasterEntities();
                if (Convert.ToInt32(ddlAdjNumList.SelectedValue) > 0)
                    AISMasterEntities.AdjusmentNumber = Convert.ToInt32(ddlAdjNumList.SelectedValue);
                AISMasterEntities.AccountStatus = acct.ACTV_IND;
                AISMasterEntities.AccountNumber = Convert.ToInt32(Request.QueryString["AcctNo"].ToString());
                AISMasterEntities.InvoiceDashboardAccountNumber = Convert.ToInt32(Request.QueryString["AcctNo"].ToString());
                AISMasterEntities.AccountName = acct.FULL_NM;
                AISMasterEntities.Bpnumber = acct.FINC_PTY_ID == null ? "" : acct.FINC_PTY_ID.ToString();
                AISMasterEntities.SSCGID = acct.SUPRT_SERV_CUSTMR_GP_ID == null ? "" : acct.SUPRT_SERV_CUSTMR_GP_ID.ToString();
                AISMasterEntities.MasterAccount = acct.MSTR_ACCT_IND == null ? false : acct.MSTR_ACCT_IND.Value;
                AISMasterEntities.MasterAccountNumber = (acct.CUSTMR_REL_ID == null) ? 0 : acct.CUSTMR_REL_ID.Value;
            }
            
        }
    }
    #endregion
    /// <summary>
    /// property to hold an instance for Draft Invoice Business Transaction Wrapper
    /// </summary>
    /// <param name=""></param>
    /// <returns>AISBusinessTransaction property</returns>
    protected AISBusinessTransaction InvoiceTransactionWrapper
    {
        get
        {
            if ((AISBusinessTransaction)Session["InvoiceTransactionWrapper"] == null)
                Session["InvoiceTransactionWrapper"] = new AISBusinessTransaction();
            return (AISBusinessTransaction)Session["InvoiceTransactionWrapper"];
        }
        set
        {
            Session["InvoiceTransactionWrapper"] = value;
        }
    }
    /// <summary>
    /// property to hold an instance for Final Invoice Business Transaction Wrapper
    /// </summary>
    /// <param name=""></param>
    /// <returns>AISBusinessTransaction property</returns>
    protected AISBusinessTransaction FinalInvoiceTransactionWrapper
    {
        get
        {
            if ((AISBusinessTransaction)Session["FinalInvoiceTransactionWrapper"] == null)
                Session["FinalInvoiceTransactionWrapper"] = new AISBusinessTransaction();
            return (AISBusinessTransaction)Session["FinalInvoiceTransactionWrapper"];
        }
        set
        {
            Session["FinalInvoiceTransactionWrapper"] = value;
        }
    }
    /// <summary>
    /// a property for Person Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>PersonBS</returns>
    private PremAdjustmentBS premAdjService;
    private PremAdjustmentBS PremAdjService
    {
        get
        {
            if (premAdjService == null)
            {
                premAdjService = new PremAdjustmentBS();
                //premAdjService.AppTransactionWrapper = InvoiceTransactionWrapper;
            }
            return premAdjService;
        }
    }
    /// <summary>
    /// a property for PremAdjustment Status Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>PostAddressBS</returns>
    private PremiumAdjustmentStatusBS premAdjStatusService;
    private PremiumAdjustmentStatusBS PremAdjStatusService
    {
        get
        {
            if (premAdjStatusService == null)
            {
                premAdjStatusService = new PremiumAdjustmentStatusBS();
                // premAdjStatusService.AppTransactionWrapper = InvoiceTransactionWrapper;
            }
            return premAdjStatusService;
        }
    }
    /// <summary>
    /// Code executed when Calculate button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCalculate_OnClick(object sender, EventArgs e)
    {
        ClearError();
        if (AdjInvDashlistview.Items.Count == 0)
        {
            ShowError("No Record(s) found...!");
            return;
        }
        string strPERD = string.Empty;
        string strPGM = string.Empty;
        HiddenField hidFld;
        HiddenField hidNxtValDate;
        HiddenField hidValMMDT;
        HiddenField hidNxtValNonPrem;
        HiddenField hidNxtValPrem;
        Label lblProgrmSts;
        bool Flag = false;
        string strError = string.Empty;
        CheckBox chk;
        ArrayList alindex = new ArrayList();
        bool chkValidPGMID;
        for (int i = 0; i < AdjInvDashlistview.Items.Count; i++)
        {
            chkValidPGMID = true;
            chk = ((CheckBox)AdjInvDashlistview.Items[i].FindControl("chkSelect"));
            if (chk.Checked)
            {
                alindex.Add(i);
                hidValMMDT = (HiddenField)AdjInvDashlistview.Items[i].FindControl("hidValMMDT");
                hidNxtValDate = (HiddenField)AdjInvDashlistview.Items[i].FindControl("hidNxtValDate");
                hidNxtValPrem = (HiddenField)AdjInvDashlistview.Items[i].FindControl("hidNxtValPrem");
                hidNxtValNonPrem = (HiddenField)AdjInvDashlistview.Items[i].FindControl("hidNxtValNonPrem");
                lblProgrmSts = (Label)AdjInvDashlistview.Items[i].FindControl("lblProgrmSts");
                Label lblProgramPeriod = (Label)AdjInvDashlistview.Items[i].FindControl("lblProgramPeriod");
                if (hidNxtValDate.Value == "")
                {
                    chkValidPGMID = false;
                    if (strError != "") strError += "<br><li>";
                    strError += "Calculation cannot be processed for the Program Period: " + lblProgramPeriod.Text + " as Next Valuation is empty";
                }
                if (lblProgrmSts.Text.Trim().ToUpper() != "ACTIVE")
                {
                    chkValidPGMID = false;
                    if (strError != "") strError += "<br><li>";
                    strError += "Calculation cannot be processed for the Program Period: " + lblProgramPeriod.Text + " as Program Status is InActive ";
                    //return;
                }
                if (hidNxtValPrem.Value != "" && hidNxtValNonPrem.Value != "")
                {
                    if (DateTime.Parse(hidNxtValPrem.Value) > DateTime.Now && DateTime.Parse(hidNxtValNonPrem.Value) > DateTime.Now)
                    {
                        DropDownList ddlCustmr = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
                        string strErr = "Calculation Engine Driver: Premium and Non Premium Valuation dates are greater than current date. " + " ;Customer ID: " + Convert.ToInt32(ddlCustmr.SelectedItem.Value) + ";" + " Program Period ID:" + lblProgramPeriod.Text;
                        new ApplicationStatusLogBS().WriteLog("AIS Calculation Engine", "ERR", "Calculation error", strErr, Convert.ToInt32(ddlCustmr.SelectedItem.Value), CurrentAISUser.PersonID);
                        if (strError != "") strError += "<br><li>";
                        //strError+="One or more adjustments could not be completed due to an error. Please check the error log for additional details";
                        strError += "Calculation cannot be processed for the Program Period " + lblProgramPeriod.Text + " as Premium and Non Premium Valuation dates are greater than current date";
                        chkValidPGMID = false;
                        //return;

                    }
                }
                hidFld = (HiddenField)AdjInvDashlistview.Items[i].FindControl("hidPremAdjPERID");
                if (hidFld.Value != "0" && chkValidPGMID)
                {
                    strPERD += hidFld.Value.ToString() + ",";
                }
                else
                {
                    hidFld = (HiddenField)AdjInvDashlistview.Items[i].FindControl("hidPremAdjPGMID");
                    if (hidFld.Value != "" && chkValidPGMID)
                    {
                        strPGM += hidFld.Value.ToString() + ",";
                    }
                }
            }
            else
            {
                alindex.Add(-1);
            }
            if (chk.Checked && !Flag)
            {
                Flag = true;
            }
        }
        if (!Flag)
        {
            ShowError("Please select at least one Program Period from the listview for Calculation");
            return;
        }
        if (strPERD != "") strPERD = strPERD.Remove(strPERD.Length - 1, 1);
        if (strPGM != "") strPGM = strPGM.Remove(strPGM.Length - 1, 1);
        bool check = hidConfirm.Value == "yes" ? true : false;
        bool checkILRF = hidConfirmILRF.Value == "yes" ? true : false;
        if (strPGM != "")
        {
            check = true;
            checkILRF = true;
        }
        if (strPERD == "" && strPGM == "")
        {
            ShowError("One or more adjustments could not be completed due to an error. Please check the error log for additional details");
            return;
        }
        bool Error = false;
        DropDownList ddlAccountlist = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        string strKeyError = "";
        objDC.ModAISCheckKeyParameters(strPGM, strPERD, ref strKeyError, CurrentAISUser.PersonID, false);
        if (strKeyError != "")
        {
            modalAries.Show();
            return;
        }
        string strCalcError = (new ProgramPeriodsBS()).CalcDriver(Convert.ToInt32(ddlAccountlist.SelectedItem.Value), strPGM, strPERD, check,checkILRF, CurrentAISUser.PersonID);
        strCalcError = strCalcError == null ? "" : strCalcError;
        strError = strError == null ? "" : strError;
        if (strCalcError == "")
        {
            if (strError != "")
            { Error = true; }
            strError = "Calculation Completed Successfully";
        }
        int intcount = ddlAdjNumList.Items.Count;
        //Function to fill the adj nos for the selected Account
        FillAdjNos(Convert.ToInt32(ddlAccountlist.SelectedValue));
        if (intcount != ddlAdjNumList.Items.Count && (strCalcError != "" || Error))
        {
            if (strError != "") strError += "<br><li>";
            strError += "One or more program period adjustments could not be completed due to an error, however some of the program period adjustment(s) may have Completed Successfully.  Please check the error log for additional details.";
        }
        else if (intcount == ddlAdjNumList.Items.Count && (strCalcError != "" || Error))
        {
            if (strError != "") strError += "<br><li>";
            strError += "One or more adjustments could not be completed due to an error. Please check the error log for additional details";
        }
        ShowError(strError);
        // Code to refresh the listview once calculation is completed

        int adjno = AISMasterEntities.AdjusmentNumber;
        string str = ddlValDateList.SelectedValue != "0" ? ddlValDateList.SelectedItem.Text : string.Empty;
        AdjInvDashlistview.DataSource = (new ProgramPeriodsBS()).GetProgramPeriodSearchDashboard(Convert.ToInt32(ddlAccountlist.SelectedValue), Convert.ToInt32(ddlProgramTypeList.SelectedValue), adjno, str);
        AdjInvDashlistview.DataBind();
        hidCofirmCount.Value = "0";
        hidLSICount.Value = "0";
        hidILRFCount.Value = "0";
        hidConfirm.Value = "";
        hidConfirmILRF.Value = "";
        for (int i = 0; i < AdjInvDashlistview.Items.Count; i++)
        {
            if (i < alindex.Count)
            {
                chk = ((CheckBox)AdjInvDashlistview.Items[i].FindControl("chkSelect"));
                Label lblAdjNo = ((Label)AdjInvDashlistview.Items[i].FindControl("lblAdjNo"));
                HiddenField lblLSI = (HiddenField)AdjInvDashlistview.Items[i].FindControl("hidLSICount");
                HiddenField lblILRF = (HiddenField)AdjInvDashlistview.Items[i].FindControl("hidILRFCount");
                if (chk != null)
                {
                    if (Convert.ToInt32(alindex[i]) >= 0)
                    {
                        if (lblAdjNo.Text != "" )
                        {
                            hidCofirmCount.Value = (Convert.ToInt16(hidCofirmCount.Value) + 1).ToString();
                            
                            //if (Convert.ToInt32(lblLSI.Value) > 0)
                            //{
                            //    hidLSICount.Value = (Convert.ToInt16(hidLSICount.Value) + 1).ToString();
                            //    LSIStr = "yes";
                            //}
                            //else
                            //{
                            //    LSIStr = "";
                            //}
                            //chk.Attributes.Add("onclick", "javascript:checkforRecalc(this,'" + LSIStr + "')");
                            if (Convert.ToInt32(lblLSI.Value) > 0)
                            {
                                hidLSICount.Value = (Convert.ToInt16(hidLSICount.Value) + 1).ToString();
                                LSIStr = "yes";
                            }
                            else
                            {
                                LSIStr = "";
                            }
                            if (Convert.ToInt32(lblILRF.Value) > 0)
                            {
                                hidILRFCount.Value = (Convert.ToInt16(hidILRFCount.Value) + 1).ToString();
                                ILRFStr = "yes";
                            }
                            else
                            {
                                ILRFStr = "";
                            }
                            chk.Attributes.Add("onclick", "javascript:checkforRecalc(this,'" + LSIStr + "','" + ILRFStr + "')");
                        }
                        chk.Checked = true;
                    }
                    else
                    {
                        chk.Checked = false;
                    }
                }
            }
        }
        ddlAdjNumList.SelectedIndex = -1;
        ListItem li = new ListItem(adjno.ToString(), adjno.ToString());
        if (ddlAdjNumList.Items.Contains(li))
        {
            ddlAdjNumList.Items.FindByValue(adjno.ToString()).Selected = true;
        }
        if (ViewState["valDate"] != null && ViewState["valDate"].ToString() != "")
        {
            li = new ListItem(ViewState["valDate"].ToString(), adjno.ToString());
            if (ddlValDateList.Items.Contains(li))
            {
                ddlValDateList.SelectedIndex = -1;
                ddlValDateList.Items.FindByText(ViewState["valDate"].ToString()).Selected = true;
            }
        }
        //Enables AdjustmentReview Menu
        AISMenu.EnableAdjReviewMenu();
        Menu aismenu = (Menu)Master.FindControl("TopNav");
        XmlDataSource AISMenuData = (XmlDataSource)Master.FindControl("AISMenuData");
        AISMenu.CreateMenu(CurrentAISUser.Role, ref AISMenuData);
        aismenu.DataBind();


    }
    /// <summary>
    /// Code executed when Account is selected from the dropdown
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlAcctlist_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlAccountlist = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        AdjInvDashlistview.Visible = false;
        btnDraft.Visible = false;
        btnCalculate.Visible = false;
        if (ddlAccountlist.SelectedValue != "0")
        {
            //Function to fill the adj nos for the selected Account
            FillAdjNos(Convert.ToInt32(ddlAccountlist.SelectedValue));
        }
        else
        {
            ddlAdjNumList.SelectedIndex = -1;
            ddlValDateList.SelectedIndex = -1;
        }

    }
    /// <summary>
    /// Function to fill the adj nos for the selected Account
    /// </summary>
    /// <param name="AcctNo"></param>
    public void FillAdjNos(int AcctNo)
    {
        ddlProgramTypeList.Enabled = true;
        IList<PremiumAdjustmentBE> PremBE = (new PremAdjustmentBS()).GetPremAdjSearch(AcctNo);
        ddlAdjNumList.Enabled = true;
        ddlAdjNumList.DataSource = PremBE.OrderBy(order => order.PREMIUM_ADJ_ID).Distinct().ToList();
        ddlAdjNumList.DataValueField = "PREMIUM_ADJ_ID";
        ddlAdjNumList.DataTextField = "PREMIUM_ADJ_ID";
        ddlAdjNumList.DataBind();
        ListItem li = new ListItem("(Select)", "0");
        ddlAdjNumList.Items.Insert(0, li);
        ddlAdjNumList.Enabled = true;
        PremBE = (new PremAdjustmentBS()).GetPremAdjSearchDates(AcctNo);
        ddlValDateList.Enabled = true;
        ddlValDateList.DataSource = PremBE.OrderBy((o => o.VALUATIONDATE)).Distinct().ToList();
        ddlValDateList.DataValueField = "VALUATIONDATE";
        ddlValDateList.DataTextField = "VALUATIONDATE";
        ddlValDateList.DataBind();
        ddlValDateList.Items.Insert(0, li);
        ddlValDateList.Enabled = true;
    }

    protected void btnClosepopup_Click(object sender, EventArgs e)
    {
        return;
    }
    /// <summary>
    /// Code executed when Adjustment is selected from DropDown
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <summary>
    /// Code executed when Clear button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClear_Click(object sender, EventArgs e)
    {
        DropDownList ddlAccountlist = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        ddlAccountlist.SelectedIndex = -1;
        ddlAdjNumList.SelectedIndex = -1;
        ddlProgramTypeList.SelectedIndex = -1;
        ddlValDateList.SelectedIndex = -1;
        ddlProgramTypeList.Enabled = false;
        ddlAdjNumList.Enabled = false;
        ddlValDateList.Enabled = false;
        AdjInvDashlistview.Visible = false;
        ddlAccountlist.Items.Clear();
        btnCalculate.Visible = false;
        btnDraft.Visible = false;
    }
    /// <summary>
    /// Code executed when Search button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        hidCofirmCount.Value = "0";
        hidLSICount.Value = "0";
        hidILRFCount.Value = "0";
        hidConfirm.Value = "";
        hidConfirmILRF.Value = "";
        DropDownList ddlAccountlist = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        if (ddlAccountlist.SelectedValue != "0" && ddlAccountlist.SelectedValue != "")
        {
            ProgramPeriodsBS Prog = new ProgramPeriodsBS();
            if (ddlAccountlist.SelectedValue != "0")
            {
                AdjInvDashlistview.Visible = true;
                string str = ddlValDateList.SelectedIndex > 0 ? ddlValDateList.SelectedItem.Text : string.Empty;
                ViewState["valDate"] = str;
                AdjInvDashlistview.DataSource = Prog.GetProgramPeriodSearchDashboard(Convert.ToInt32(ddlAccountlist.SelectedValue), Convert.ToInt32(ddlProgramTypeList.SelectedValue), Convert.ToInt32(ddlAdjNumList.SelectedValue), str);
                AdjInvDashlistview.DataBind();
                for (int i = 0; i < AdjInvDashlistview.Items.Count; i++)
                {
                    CheckBox chk = (CheckBox)AdjInvDashlistview.Items[i].FindControl("chkSelect");
                    if (chk != null && !chk.Checked)
                    {
                        chk.Checked = true;
                    }
                }
                if (AdjInvDashlistview.Items.Count > 0)
                {
                    btnCalculate.Visible = true;
                    if (ddlAdjNumList.SelectedValue != "0")
                    {
                        int intadjNo = Convert.ToInt32(ddlAdjNumList.SelectedValue);
                        IList<PremiumAdjustmentStatusBE> objPremAdjStsBE = PremAdjStatusService.getPreAdjStatusList(intadjNo);
                        if (objPremAdjStsBE.Count > 0)
                        {
                            //Check if Adj-Sts is "CALC" then only show the draft button true.
                            //because draft can be done for the adj which has status only "CALC".
                            if (objPremAdjStsBE[0].ADJ_STS_TYP_ID == 346)
                            {
                                btnDraft.Visible = true;
                            }
                            else
                            {
                                btnDraft.Visible = false;
                            }
                        }
                    }

                }
                else
                {
                    btnDraft.Visible = false;
                    btnCalculate.Visible = false;
                }
                //code for setting Master Entities with the searched Account
                AccountBE acct = (new AccountBS()).getAccount(Convert.ToInt32(ddlAccountlist.SelectedValue));
                AISMasterEntities = new MasterEntities();
                if (Convert.ToInt32(ddlAdjNumList.SelectedValue) > 0)
                    AISMasterEntities.AdjusmentNumber = Convert.ToInt32(ddlAdjNumList.SelectedValue);
                AISMasterEntities.AccountStatus = acct.ACTV_IND;
                AISMasterEntities.AccountNumber = Convert.ToInt32(ddlAccountlist.SelectedValue);
                AISMasterEntities.InvoiceDashboardAccountNumber = Convert.ToInt32(ddlAccountlist.SelectedValue);
                AISMasterEntities.AccountName = acct.FULL_NM;
                AISMasterEntities.Bpnumber = acct.FINC_PTY_ID == null ? "" : acct.FINC_PTY_ID.ToString();
                AISMasterEntities.SSCGID = acct.SUPRT_SERV_CUSTMR_GP_ID == null ? "" : acct.SUPRT_SERV_CUSTMR_GP_ID.ToString();
                AISMasterEntities.MasterAccount = acct.MSTR_ACCT_IND == null ? false : acct.MSTR_ACCT_IND.Value;
                AISMasterEntities.MasterAccountNumber = (acct.CUSTMR_REL_ID == null) ? 0 : acct.CUSTMR_REL_ID.Value;
            }
        }
        else
        {
            ShowError("Please select an Account from the list");
        }
    }
    /// <summary>
    /// Code for Sorting of ListView columns
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void AdjInvDashlistview_Sorting(object sender, ListViewSortEventArgs e)
    {
        Image imgPROGRAMPERIOD = (Image)AdjInvDashlistview.FindControl("imgPROGRAMPERIOD");
        Image imgVALUATIONDATE = (Image)AdjInvDashlistview.FindControl("imgVALUATIONDATE");
        Image imgPROGRAMTYPENAME = (Image)AdjInvDashlistview.FindControl("imgPROGRAMTYPENAME");
        Image imgBUOFFICE = (Image)AdjInvDashlistview.FindControl("imgBUOFFICE");
        Image imgPROGRAMSTATUS = (Image)AdjInvDashlistview.FindControl("imgPROGRAMSTATUS");

        imgPROGRAMPERIOD.Visible = false;
        imgVALUATIONDATE.Visible = false;
        imgPROGRAMTYPENAME.Visible = false;
        imgBUOFFICE.Visible = false;
        imgPROGRAMSTATUS.Visible = false;
        string str = ddlValDateList.SelectedValue != "0" ? ddlValDateList.SelectedItem.Text : string.Empty;
        DropDownList ddlAccountlist = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        IList<ProgramPeriodBE> ProgramPeriodList = (new ProgramPeriodsBS()).GetProgramPeriodSearchDashboard(Convert.ToInt32(ddlAccountlist.SelectedValue), Convert.ToInt32(ddlProgramTypeList.SelectedValue), Convert.ToInt32(ddlAdjNumList.SelectedValue), str);
        switch (e.SortExpression)
        {
            case "PROGRAMPERIOD":
                imgPROGRAMPERIOD.Visible = true;
                if (imgPROGRAMPERIOD.ToolTip == "Ascending")
                {
                    ProgramPeriodList = (ProgramPeriodList.OrderBy(o => o.STRT_DT)).ToList();
                    imgPROGRAMPERIOD.ToolTip = "Descending";
                    imgPROGRAMPERIOD.ImageUrl = "~/images/descending.gif";
                }
                else
                {
                    ProgramPeriodList = (ProgramPeriodList.OrderByDescending(o => o.STRT_DT)).ToList();
                    imgPROGRAMPERIOD.ToolTip = "Ascending";
                    imgPROGRAMPERIOD.ImageUrl = "~/images/ascending.gif";
                }
                break;

            case "VALUATIONDATE":
                imgVALUATIONDATE.Visible = true;
                if (imgVALUATIONDATE.ToolTip == "Ascending")
                {
                    ProgramPeriodList = (ProgramPeriodList.OrderBy(o => o.VALUATION_DATE)).ToList();
                    imgVALUATIONDATE.ToolTip = "Descending";
                    imgVALUATIONDATE.ImageUrl = "~/images/descending.gif";
                }
                else
                {
                    ProgramPeriodList = (ProgramPeriodList.OrderByDescending(o => o.VALUATION_DATE)).ToList();
                    imgVALUATIONDATE.ToolTip = "Ascending";
                    imgVALUATIONDATE.ImageUrl = "~/images/ascending.gif";
                }
                break;
            case "PROGRAMTYPENAME":
                imgPROGRAMTYPENAME.Visible = true;
                if (imgPROGRAMTYPENAME.ToolTip == "Ascending")
                {
                    ProgramPeriodList = (ProgramPeriodList.OrderBy(o => o.PROGRAMTYPE)).ToList();
                    imgPROGRAMTYPENAME.ToolTip = "Descending";
                    imgPROGRAMTYPENAME.ImageUrl = "~/images/descending.gif";
                }
                else
                {
                    ProgramPeriodList = (ProgramPeriodList.OrderByDescending(o => o.PROGRAMTYPE)).ToList();
                    imgPROGRAMTYPENAME.ToolTip = "Ascending";
                    imgPROGRAMTYPENAME.ImageUrl = "~/images/ascending.gif";
                }
                break;
            case "BUOFFICE":
                imgBUOFFICE.Visible = true;
                if (imgBUOFFICE.ToolTip == "Ascending")
                {
                    ProgramPeriodList = (ProgramPeriodList.OrderBy(o => o.BUOFFFICE)).ToList();
                    imgBUOFFICE.ToolTip = "Descending";
                    imgBUOFFICE.ImageUrl = "~/images/descending.gif";
                }
                else
                {
                    ProgramPeriodList = (ProgramPeriodList.OrderByDescending(o => o.BUOFFFICE)).ToList();
                    imgBUOFFICE.ToolTip = "Ascending";
                    imgBUOFFICE.ImageUrl = "~/images/ascending.gif";
                }
                break;
            case "PROGRAMSTATUS":
                imgPROGRAMSTATUS.Visible = true;
                if (imgPROGRAMSTATUS.ToolTip == "Ascending")
                {
                    ProgramPeriodList = (ProgramPeriodList.OrderBy(o => o.PROGRAMSTATUS)).ToList();
                    imgPROGRAMSTATUS.ToolTip = "Descending";
                    imgPROGRAMSTATUS.ImageUrl = "~/images/descending.gif";
                }
                else
                {
                    ProgramPeriodList = (ProgramPeriodList.OrderByDescending(o => o.PROGRAMSTATUS)).ToList();
                    imgPROGRAMSTATUS.ToolTip = "Ascending";
                    imgPROGRAMSTATUS.ImageUrl = "~/images/ascending.gif";
                }
                break;
        }
        hidCofirmCount.Value = "0";
        hidLSICount.Value = "0";
        hidILRFCount.Value = "0";
        hidConfirm.Value = "";
        hidConfirmILRF.Value = "";

        AdjInvDashlistview.DataSource = ProgramPeriodList;
        AdjInvDashlistview.DataBind();
    }
    /// <summary>
    /// Databound ItemCommand code for the ListView
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    string LSIStr = "";
    string ILRFStr = "";
    protected void DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            CheckBox chk = (CheckBox)e.Item.FindControl("chkSelect");
            HiddenField lblLSI = (HiddenField)e.Item.FindControl("hidLSICount");
            HiddenField lblILRF = (HiddenField)e.Item.FindControl("hidILRFCount");
            if (chk.Checked )
            {
                hidCofirmCount.Value = (Convert.ToInt16(hidCofirmCount.Value) + 1).ToString();
                if (Convert.ToInt32(lblLSI.Value) > 0)
                {
                    hidLSICount.Value = (Convert.ToInt16(hidLSICount.Value) + 1).ToString();
                    LSIStr = "yes";
                }
                else
                {
                    LSIStr = "";
                }
                if (Convert.ToInt32(lblILRF.Value) > 0)
                {
                    hidILRFCount.Value = (Convert.ToInt16(hidILRFCount.Value) + 1).ToString();
                    ILRFStr = "yes";
                }
                else
                {
                    ILRFStr = "";
                }
                chk.Attributes.Add("onclick", "javascript:checkforRecalc(this,'" + LSIStr + "','" + ILRFStr + "')");
            }
            else
            {
                //chk.Checked = true;
            }
        }
    }
    #region code to generate the draft and final invoice
    /// <summary>
    /// This is the click event for the draft button to generate the draft invoice.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    ///iFlag is three types
    ///1 - Draft Invoice
    ///2 - Final Invoice
    ///3 - Preview Button
    #region Draft Button Click Event
    protected void btnDraft_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (ddlAdjNumList.SelectedIndex > 0)
            {
                ClearError();
                Label lblCalcAdjSts = (Label)AdjInvDashlistview.Items[0].FindControl("lblCalcAdjSts");
                if (lblCalcAdjSts.Text != "Complete")
                {
                    ShowError("Draft can be done only if the Calculation Adjustment Status for selected adjustment is Completed");
                    return;
                }
                //string strMessage = generateInvoice(Convert.ToInt32(ddlAdjNumList.SelectedItem.Text), 1, string.Empty);
                string strMessage = string.Empty;
                PremAdjustmentBS objInvBS = new PremAdjustmentBS();
                PremiumAdjustmentBE objInvBE = new PremiumAdjustmentBE();
                PremiumAdjustmentBE objInvPreviousBE = new PremiumAdjustmentBE();
                objInvBE = objInvBS.getPremiumAdjustmentRow(Convert.ToInt32(ddlAdjNumList.SelectedItem.Text));
                bool HistFlag = false;
                if (objInvBE.HISTORICAL_ADJ_IND.HasValue)
                {
                    HistFlag = Convert.ToBoolean(objInvBE.HISTORICAL_ADJ_IND);
                }
                if (!objInvBE.REL_PREM_ADJ_ID.HasValue)
                {

                    strMessage = generateDraftInvoice(Convert.ToInt32(ddlAdjNumList.SelectedItem.Text), HistFlag);
                }
                else
                {
                    objInvPreviousBE = objInvBS.getPremiumAdjustmentRow(Convert.ToInt32(objInvBE.REL_PREM_ADJ_ID));
                    if (objInvPreviousBE.ADJ_RRSN_IND == true)
                        strMessage = generateRevisedDraftInvoice(Convert.ToInt32(ddlAdjNumList.SelectedItem.Text), objInvPreviousBE.PREMIUM_ADJ_ID, HistFlag);
                    else
                    {
                        ShowError("Draft cannot be done for void adjustment");
                        return;
                    }

                }
                PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
                IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;
                int intAdjUWReviewedStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.UWReviewed, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
                objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(Convert.ToInt32(ddlAdjNumList.SelectedItem.Text));
                objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
                int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
                if (objPremStsBE.ADJ_STS_TYP_ID != intAdjCalStatusID)
                    btnDraft.Visible = false;
                // Code to refresh the listview once calculation is completed
                DropDownList ddlAccountlist = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
                hidCofirmCount.Value = "0";
                hidLSICount.Value = "0";
                hidILRFCount.Value = "0";
                hidConfirm.Value = "";
                hidConfirmILRF.Value = "";
                AdjInvDashlistview.DataSource = (new ProgramPeriodsBS()).GetProgramPeriodSearchDashboard(Convert.ToInt32(ddlAccountlist.SelectedValue), 0, Convert.ToInt32(ddlAdjNumList.SelectedValue), "");
                AdjInvDashlistview.DataBind();
                ShowError(strMessage);
                return;

            }
            else
            {
                string strMessage = "Please select Adjustment Number";
                ShowError(strMessage);
                return;
            }
        }
        catch (Exception ex)
        {
            DropDownList ddlAccountlist = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
            new ApplicationStatusLogBS().WriteLog("AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message,Convert.ToInt32(ddlAccountlist.SelectedValue), CurrentAISUser.PersonID);
            ShowError("Draft Invoice could not be completed due to an error. Please check the error log for additional details");
            return;
        }


    }
    #endregion


    /// <summary>
    /// Method to Send the EMail
    /// </summary>
    /// <param name="intAdjNo"></param>
    /// <param name="iFlag"></param>
    #region EMailNotification
    private void EMailNotification(int intAdjNo, int iFlag)
    {
        //Draft Invoice
        if (iFlag == 1)
        {
            SMTPMailer dd = new SMTPMailer();
            dd.HasAttachment = false;
            IList<PremiumAdjustmentBE> EmailInfo = new PremAdjustmentBS().GetEMailInfo(intAdjNo);
            System.Net.Mail.MailAddressCollection objMailList = new System.Net.Mail.MailAddressCollection();
            IList<string> strEmailList = new PremAdjustmentBS().getEmailIDS(ZurichNA.AIS.DAL.Logic.GlobalConstants.RESPONSIBILITIES.DRAFT, true, EmailInfo[0].CUSTOMERID);
            for (int i = 0; i < strEmailList.Count; i++)
            {
                objMailList.Add(strEmailList[i].ToString());
            }
            dd.lstTomailAddress = objMailList;

            string strSubject = "Draft invoice " + "{" + EmailInfo[0].DRAFTINVNO + "}" + " " + "for " + " " + EmailInfo[0].CUSTMR_NAME + " " + "for " + " - " + EmailInfo[0].BUNAME;
            string strBody = "Draft invoice " + "{" + EmailInfo[0].DRAFTINVNO + "}" + " " + " has been produced for the following" + "\r\n";
            strBody = strBody + "\r\n";
            strBody = strBody + "Insured Name:" + EmailInfo[0].CUSTMR_NAME + "\r\n";
            strBody = strBody + "Broker Name:" + EmailInfo[0].BROKERNAME + "\r\n";
            strBody = strBody + "Valuation Date:" + EmailInfo[0].VALUATIONDATE + "\r\n";
            strBody = strBody + "Program Period:" + EmailInfo[0].PROGRAMPERIOD_STDT.Value.ToShortDateString() + " - " + EmailInfo[0].PROGRAMPERIOD_ENDT.Value.ToShortDateString() + "\r\n";
            strBody = strBody + "Invoice Date:" + EmailInfo[0].DRFT_INVC_DT.Value.ToShortDateString() + "\r\n";
            strBody = strBody + "Invoice Number:" + EmailInfo[0].DRAFTINVNO + "\r\n";
            strBody = strBody + "BU Name:" + EmailInfo[0].BUNAME + "\r\n";
            strBody = strBody + "\r\n";
            strBody = strBody + "\r\n";
            strBody = strBody + "Please review adjustment for QC.  Please complete the Adjustment Processing QC screen." + "\r\n";
            dd.Subject = strSubject;
            dd.Body = strBody;
            RandomWait(100);
            dd.SendMail();

        }
        else if (iFlag == 2)
        {
            SMTPMailer dd = new SMTPMailer();
            dd.HasAttachment = false;
            IList<PremiumAdjustmentBE> EmailInfo = new PremAdjustmentBS().GetEMailInfo(intAdjNo);
            System.Net.Mail.MailAddressCollection objMailList = new System.Net.Mail.MailAddressCollection();
            IList<string> strEmailList = new PremAdjustmentBS().getEmailIDS(ZurichNA.AIS.DAL.Logic.GlobalConstants.RESPONSIBILITIES.FINAL_INVOICE, false, EmailInfo[0].CUSTOMERID);
            for (int i = 0; i < strEmailList.Count; i++)
            {
                objMailList.Add(strEmailList[i].ToString());
            }
            dd.lstTomailAddress = objMailList;
            string strSubject = "Final Invoice " + "{" + EmailInfo[0].FINALINVNO + "}" + " " + "for " + " " + EmailInfo[0].CUSTMR_NAME;

            string strBody = "Final Invoice " + "{" + EmailInfo[0].FINALINVNO + "}" + " " + " has been produced for the following" + "\r\n";
            strBody = strBody + "\r\n";
            strBody = strBody + "Insured Name:" + EmailInfo[0].CUSTMR_NAME + "\r\n";
            strBody = strBody + "Broker Name:" + EmailInfo[0].BROKERNAME + "\r\n";
            strBody = strBody + "Valuation Date:" + EmailInfo[0].VALUATIONDATE + "\r\n";
            strBody = strBody + "Program Period:" + EmailInfo[0].PROGRAMPERIOD_STDT.Value.ToShortDateString() + " - " + EmailInfo[0].PROGRAMPERIOD_ENDT.Value.ToShortDateString() + "\r\n";
            strBody = strBody + "Invoice Date:" + EmailInfo[0].FNL_INVC_DT.Value.ToShortDateString() + "\r\n";
            strBody = strBody + "Invoice Number:" + EmailInfo[0].FINALINVNO + "\r\n";
            strBody = strBody + "\r\n";
            strBody = strBody + "\r\n";
            strBody = strBody + "Copy of the Final invoice is available in ZDW or can be accessed within Retro Replacement System using the Invoice Inquiry screen." + "\r\n";
            dd.Subject = strSubject;
            dd.Body = strBody;
            dd.SendMail();

        }



    }
    #endregion




    #endregion
    //
    //
    //Draft Method Testing Code
    public string generateDraftInvoice(int iAdjNo, bool HistFlag)
    {

        try
        {
            //To check the CALC status
            PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
            IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;
            //Retrieving CALC,UWReviewed Statuses from lookup table
            int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(iAdjNo);
            //objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[objPremAdjStsBE.Count - 1].PremumAdj_sts_ID);
            objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
            //if it is Draft Invoice we are verifying the status.if status is not equal to CALC then show error

            if (objPremStsBE.ADJ_STS_TYP_ID != intAdjCalStatusID)
            {
                string strMessage = "Please do the calculation for at least one program period";
                return strMessage;
            }

            bool UpdateInvNum = false;
            bool InsertStatus = false;
            objDC.Connection.Open();
            trans = objDC.Connection.BeginTransaction();
            objDC.Transaction = trans;
            //Calling Generate InvoiceNumber Function
            string strInvNo = generateInvoiceNumbers(iAdjNo, 1, 0, 0);

            ////Retrieving Draft-Invoice,Final Inv and Draft-Inv-Err Statuses from lookup table
            int intDraftInvStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.DraftInvd, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            //Draft Invoice Updation
            string strComments = string.Empty;
            UpdateInvNum = new InvoiceDriverBS().UpdatePremAdjutmentDraftInvoiceData(objDC, iAdjNo, strInvNo, CurrentAISUser.PersonID);
            InsertStatus = new InvoiceDriverBS().InsertPremAdjutmentStatusData(objDC, iAdjNo, intDraftInvStatusID, strComments, CurrentAISUser.PersonID);
            if (UpdateInvNum != true || InsertStatus != true)
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Prem_adj or Prem_adj_sts table updation Failed", CurrentAISUser.PersonID);
                return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
            }

            //Internal PDF Generation
            ReportDocument objMainDraftInternal = new ReportDocument();
            objMainDraftInternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

            //External PDF Generation
            ReportDocument objMainDraftExternal = new ReportDocument();
            objMainDraftExternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

            //Coding Work Sheet PDF Generation
            ReportDocument objMainDraftCDWSheet = new ReportDocument();
            objMainDraftCDWSheet.Load(Server.MapPath("\\Reports\\" + "rptCodingMasterReport" + ".rpt"));


            //Internal PDF Connections
            GenerateReportConnection(objMainDraftInternal);
            //External PDF Connections
            GenerateReportConnection(objMainDraftExternal);
            //Coding Work Sheet PDF Connections
            GenerateReportConnection(objMainDraftCDWSheet);

            objMainDraftInternal.VerifyDatabase();
            objMainDraftExternal.VerifyDatabase();
            objMainDraftCDWSheet.VerifyDatabase();


            ParameterDiscreteValue prmAdjNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmFlag = new ParameterDiscreteValue();
            //ParameterDiscreteValue prmERPInd = new ParameterDiscreteValue();
            ParameterDiscreteValue prmFlipSigns = new ParameterDiscreteValue();
            ParameterDiscreteValue prmRevFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmInvNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmHistFlag = new ParameterDiscreteValue();
            prmAdjNo.Value = iAdjNo;
            //Draft Invoice
            prmFlag.Value = 1;
            // prmERPInd.Value = false;//This is dummy variable
            prmFlipSigns.Value = false;
            prmRevFlag.Value = false;
            prmInvNo.Value = strInvNo;
            prmHistFlag.Value = HistFlag;
            /*****************Setting Master Report Parameters Value Begin******************/
            objMainDraftInternal.SetParameterValue("@ADJNO", prmAdjNo);
            objMainDraftInternal.SetParameterValue("@FLAG", prmFlag);
            objMainDraftExternal.SetParameterValue("@ADJNO", prmAdjNo);
            objMainDraftExternal.SetParameterValue("@FLAG", prmFlag);
            objMainDraftCDWSheet.SetParameterValue("@ADJNO", prmAdjNo);
            objMainDraftCDWSheet.SetParameterValue("@FLAG", prmFlag);
            /*****************Setting Master Report Parameters Value Begin******************/

            //Draft Invoice

            //Draft Invoice Internal PDF
            IList<InvoiceExhibitBE> objDrftInternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(1);
            for (int iCount = 0; iCount < objDrftInternalIlistBE.Count; iCount++)
            {

                setMasterReportParameter(objMainDraftInternal, objDrftInternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftInternalIlistBE[iCount].STS_IND), 1, Convert.ToChar(objDrftInternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            }

            //Draft Invoice External PDF
            IList<InvoiceExhibitBE> objDrftExternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(2);
            for (int iCount = 0; iCount < objDrftExternalIlistBE.Count; iCount++)
            {

                setMasterReportParameter(objMainDraftExternal, objDrftExternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftExternalIlistBE[iCount].STS_IND), 2, Convert.ToChar(objDrftExternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            }
            //Draft Coding Work Sheet PDF 
            IList<InvoiceExhibitBE> objDrftCDWorksheetIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(3);
            for (int iCount = 0; iCount < objDrftCDWorksheetIlistBE.Count; iCount++)
            {

                setMasterReportParameter(objMainDraftCDWSheet, objDrftCDWorksheetIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftCDWorksheetIlistBE[iCount].STS_IND), 3, Convert.ToChar(objDrftCDWorksheetIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            }


            /*****************Setting Sub Reports Parameters Value Begin******************/
            setInternalSubReportParameters(objMainDraftInternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            //setInternalSubReportParameters(objMainDraftExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            setExternalSubReportParameters(objMainDraftExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            setCDWSubReportParameters(objMainDraftCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            /*****************Setting Sub Reports Parameters Value End******************/
            string strVal1;
            string strVal2;
            string strVal3;

            try
            {


                string strInternalFilename = "DraftInternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                strVal1 = ExportReports(objMainDraftInternal, strInternalFilename, iAdjNo, 1, 'I');

            }
            catch (Exception ee)
            {

                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
                return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
            }
            try
            {


                string strExaternalFilename = "DraftExternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                strVal2 = ExportReports(objMainDraftExternal, strExaternalFilename, iAdjNo, 1, 'E');

            }
            catch (Exception ee)
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
                return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
            }
            try
            {

                string strCDWorkSheetFilename = "DraftCDWorkSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                strVal3 = ExportReports(objMainDraftCDWSheet, strCDWorkSheetFilename, iAdjNo, 1, 'C');

            }
            catch (Exception ee)
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
                return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
            }
            objMainDraftInternal.Close();
            objMainDraftExternal.Close();
            objMainDraftCDWSheet.Close();
            if (strVal1 != null && strVal1 != "")
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal1, CurrentAISUser.PersonID);
                return strVal1;
            }
            else if (strVal2 != null && strVal2 != "")
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal2, CurrentAISUser.PersonID);
                return strVal2;
            }
            else if (strVal3 != null && strVal3 != "")
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal3, CurrentAISUser.PersonID);
                return strVal3;
            }
            else
            {
                objDC.SubmitChanges();
                trans.Commit();
                try
                {
                    EMailNotification(iAdjNo, 1);
                    return "The Adjustment Draft Invoice has been submitted for processing. The Draft invoice number is " + strInvNo + ". Please note that the pdf copy of the Draft invoice will be available after 15 minutes";
                }
                catch (Exception ex)
                {
                    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
                    return "The Adjustment Draft Invoice has been submitted for processing. The Draft invoice number is " + strInvNo + ". Please note that the pdf copy of the Draft invoice will be available after 15 minutes,But E-Mail Notification Failed";
                }

            }

        }
        catch (Exception ex)
        {
            trans.Rollback();
            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
            return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
        }
        return string.Empty;
    }
    /// <summary>
    /// This is to generate the invoice number
    /// </summary>
    /// <param name="iAdjNo"></param>
    /// <param name="iFlag"></param>
    /// <returns></returns>
    #region generateInvoiceNumber Method
    public string generateInvoiceNumbers(int iAdjNo, int iFlag, int iRevAdjNo, int rFlag)
    {
        //iFlag=1 for Draft Invoice
        //IFlag=2 for Final Invoice
        //Iflag=3 for Revision Invoice
        try
        {
            PremAdjustmentBS objInvBS = new PremAdjustmentBS();
            PremiumAdjustmentBE objInvBE = new PremiumAdjustmentBE();
            objInvBE = objInvBS.getPremiumAdjustmentRow(iAdjNo);

            int iCount = iAdjNo.ToString().Length;
            int iRemain = 10 - iCount;
            string strRemain = string.Empty;
            string strTemp = string.Empty;
            for (int iCnt = 0; iCnt < iRemain; iCnt++)
            {
                strRemain = strRemain + "0";
            }

            switch (iFlag)
            {
                case 1:
                    //This logic is to create the draft invoice number
                    if (objInvBE.INVC_NBR_TXT == null || objInvBE.INVC_NBR_TXT == "")
                    {
                        strInvNo = "RTD" + "01" + strRemain + iAdjNo.ToString();
                    }
                    else
                    {
                        strTemp = objInvBE.INVC_NBR_TXT.Substring(3, 2);
                        strInvNo = "RTD" + ((Convert.ToInt32(strTemp) + 1).ToString().Length == 1 ? "0" + (Convert.ToInt32(strTemp) + 1).ToString() : (Convert.ToInt32(strTemp) + 1).ToString()) + objInvBE.INVC_NBR_TXT.Substring(5, 10);
                    }
                    break;
                case 2:
                    //This logic is to create the final invoice number
                    if (objInvBE.FNL_INVC_NBR_TXT == null || objInvBE.FNL_INVC_NBR_TXT == "")
                    {
                        strInvNo = "RTI" + "01" + strRemain + iAdjNo.ToString();
                    }
                    break;
                case 3:
                    //This logic is to create the Revision invoice number
                    int iRevCount = iRevAdjNo.ToString().Length;
                    int iRevRemain = 10 - iRevCount;
                    string strRevRemain = string.Empty;
                    string strRevTemp = string.Empty;
                    for (int iCnt = 0; iCnt < iRevRemain; iCnt++)
                    {
                        strRevRemain = strRevRemain + "0";
                    }
                    PremiumAdjustmentBE objRevInvBE = new PremiumAdjustmentBE();
                    objRevInvBE = objInvBS.getPremiumAdjustmentRow(iRevAdjNo);
                    string strInv = string.Empty;
                    if (rFlag == 1)
                    {
                        if (objRevInvBE.INVC_NBR_TXT == null || objRevInvBE.INVC_NBR_TXT == "")
                        {
                            strInvNo = "RTD" + "01" + strRevRemain + iRevAdjNo.ToString();
                        }
                        else
                        {

                            if (objInvBE.INVC_NBR_TXT == null || objInvBE.INVC_NBR_TXT == "")
                            {
                                strRevTemp = objRevInvBE.INVC_NBR_TXT.Substring(3, 2);
                                strInvNo = "RTD" + ((Convert.ToInt32(strRevTemp) + 1).ToString().Length == 1 ? "0" + (Convert.ToInt32(strRevTemp) + 1).ToString() : (Convert.ToInt32(strRevTemp) + 1).ToString()) + objRevInvBE.INVC_NBR_TXT.Substring(5, 10);
                            }
                            else
                            {
                                strRevTemp = objInvBE.INVC_NBR_TXT.Substring(3, 2);
                                strInvNo = "RTD" + ((Convert.ToInt32(strRevTemp) + 1).ToString().Length == 1 ? "0" + (Convert.ToInt32(strRevTemp) + 1).ToString() : (Convert.ToInt32(strRevTemp) + 1).ToString()) + objInvBE.INVC_NBR_TXT.Substring(5, 10);
                            }
                        }

                    }
                    else if (rFlag == 2)
                    {
                        if (objRevInvBE.FNL_INVC_NBR_TXT == null || objRevInvBE.FNL_INVC_NBR_TXT == "")
                        {
                            strInvNo = "RTR" + "01" + strRevRemain + iRevAdjNo.ToString();
                        }
                        else
                        {
                            if (objRevInvBE.FNL_INVC_NBR_TXT.Substring(0, 3) == "RTI")
                            {
                                strInvNo = "RTR" + "01" + objRevInvBE.FNL_INVC_NBR_TXT.Substring(5, 10);
                            }
                            else
                            {
                                strRevTemp = objRevInvBE.FNL_INVC_NBR_TXT.Substring(3, 2);
                                strInvNo = "RTR" + ((Convert.ToInt32(strRevTemp) + 1).ToString().Length == 1 ? "0" + (Convert.ToInt32(strRevTemp) + 1).ToString() : (Convert.ToInt32(strRevTemp) + 1).ToString()) + objRevInvBE.FNL_INVC_NBR_TXT.Substring(5, 10);
                            }
                        }

                    }
                    break;

            }

        }
        catch (Exception ex)
        {
            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
            ShowError("Draft Invoice could not be completed due to an error. Please check the error log for additional details");
                       
        }
        return strInvNo;
    }
    #endregion

    /// <summary>
    /// Method to invoke the Connections Required for the Reports
    /// </summary>
    /// <param name="objMain"></param>
    #region GenerateReportConnections Method
    private void GenerateReportConnection(ReportDocument objMain)
    {
        // Set the Database Connection String Properties for the report.
        common = new Common(this.GetType());
        AISBasePage objAISPage = new AISBasePage();
        ConnectionInfo conn = objAISPage.GetReportConnection();

        TableLogOnInfo tblLogonInfo;
        Tables tbls = objMain.Database.Tables;

        // Set the Database Login Info to all the tables used in the main report.
        foreach (CrystalDecisions.CrystalReports.Engine.Table tbl in tbls)
        {
            tblLogonInfo = tbl.LogOnInfo;
            tblLogonInfo.ConnectionInfo = conn;
            tbl.ApplyLogOnInfo(tblLogonInfo);
        }


        // Loop through all the sections and its objects to do the same for the subreports
        foreach (CrystalDecisions.CrystalReports.Engine.Section section in objMain.ReportDefinition.Sections)
        {

            // In each section we need to loop through all the reporting objects
            foreach (CrystalDecisions.CrystalReports.Engine.ReportObject reportObject in section.ReportObjects)
            {
                if (reportObject.Kind == ReportObjectKind.SubreportObject)
                {
                    SubreportObject subReport = (SubreportObject)reportObject;
                    ReportDocument subDocument = objMain.OpenSubreport(subReport.SubreportName);

                    foreach (CrystalDecisions.CrystalReports.Engine.Table table in subDocument.Database.Tables)
                    {
                        tblLogonInfo = table.LogOnInfo;
                        tblLogonInfo.ConnectionInfo = conn;
                        table.ApplyLogOnInfo(tblLogonInfo);
                    }
                }
            }
        }
    }
    #endregion

    /// <summary>
    /// Mthod to Set the Parameters to Master Report
    /// </summary>
    /// <param name="objMain"></param>
    /// <param name="strAttachCode"></param>
    /// <param name="blStatus"></param>
    /// <param name="intFlag"></param>
    #region setMasterReportParameters Method
    public void setMasterReportParameter(ReportDocument objMain, string strAttachCode, bool blStatus, int intFlag, char strInternalFlag, int iAdjNo)
    {
        AdjustmentReviewCommentsBS objReportAvailable = new AdjustmentReviewCommentsBS();

        switch (strAttachCode)
        {
            //Broker Letter Exhibit
            case "INV1":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "BROKER LETTER"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressBrokerLetter", "View");
                            else
                                objMain.SetParameterValue("SuppressBrokerLetter", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressBrokerLetter", "View");
                            else
                                objMain.SetParameterValue("SuppressBrokerLetter", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressBrokerLetter", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressBrokerLetter", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressBrokerLetter", "Suppress");
                break;

            //Adjustment Invoice Exhibit
            case "INV2":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "SUMMARY INVOICE"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressSectionAdjInv", "View");
                            else
                                objMain.SetParameterValue("SuppressSectionAdjInv", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressSectionAdjInv", "View");
                            else
                                objMain.SetParameterValue("SuppressSectionAdjInv", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressSectionAdjInv", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressSectionAdjInv", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressSectionAdjInv", "Suppress");
                break;

            //Retrospective Premium Adjustment Exhibit
            case "INV3":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "RETROSPECTIVE PREMIUM ADJUSTMENT"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressRetroPremAdj", "View");
                            else
                                objMain.SetParameterValue("SuppressRetroPremAdj", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressRetroPremAdj", "View");
                            else
                                objMain.SetParameterValue("SuppressRetroPremAdj", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressRetroPremAdj", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressRetroPremAdj", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressRetroPremAdj", "Suppress");
                break;


            //Retrospective Premium Adjustment Legend Exhibit
            case "INV4":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "RETROSPECTIVE ADJUSTMENT LEGEND"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressRetroLegend", "View");
                            else
                                objMain.SetParameterValue("SuppressRetroLegend", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressRetroLegend", "View");
                            else
                                objMain.SetParameterValue("SuppressRetroLegend", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressRetroLegend", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressRetroLegend", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressRetroLegend", "Suppress");
                break;

            //Loss Based Assessment Exhibit
            case "INV5":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "LBA"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressLBA", "View");
                            else
                                objMain.SetParameterValue("SuppressLBA", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressLBA", "View");
                            else
                                objMain.SetParameterValue("SuppressLBA", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressLBA", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressLBA", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressLBA", "Suppress");
                break;

            //Claims Handling Fee
            case "INV6":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "CHF"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressCHF", "View");
                            else
                                objMain.SetParameterValue("SuppressCHF", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressCHF", "View");
                            else
                                objMain.SetParameterValue("SuppressCHF", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressCHF", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressCHF", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressCHF", "Suppress");
                break;


            //Excess Losses
            case "INV7":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "EXCESS LOSS"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressExcessLoss", "View");
                            else
                                objMain.SetParameterValue("SuppressExcessLoss", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressExcessLoss", "View");
                            else
                                objMain.SetParameterValue("SuppressExcessLoss", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressExcessLoss", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressExcessLoss", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressExcessLoss", "Suppress");
                break;

            //Cumulative Totals Worksheet
            case "INV8":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "CUMULATIVE TOTALS"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressCumTotals", "View");
                            else
                                objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressCumTotals", "View");
                            else
                                objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                break;

            //Residual Market Subsidy Charge
            case "INV9":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "RML"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressResidualMarkSubChange", "View");
                            else
                                objMain.SetParameterValue("SuppressResidualMarkSubChange", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressResidualMarkSubChange", "View");
                            else
                                objMain.SetParameterValue("SuppressResidualMarkSubChange", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressResidualMarkSubChange", "View");
                    }
                    else
                        objMain.SetParameterValue("SuppressResidualMarkSubChange", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressResidualMarkSubChange", "Suppress");
                break;


            //Workers Compensation Tax and Assessment Kentucky
            case "INV10":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "KY & OR TAXES"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressKYOR", "View");
                            else
                                objMain.SetParameterValue("SuppressKYOR", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressKYOR", "View");
                            else
                                objMain.SetParameterValue("SuppressKYOR", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressKYOR", "View");
                    }
                    else
                        objMain.SetParameterValue("SuppressKYOR", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressKYOR", "Suppress");
                break;


            //Adjustment of NY Second Injury Fund
            case "INV11":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "NY-SIF"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressAdjNY", "View");
                            else
                                objMain.SetParameterValue("SuppressAdjNY", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressAdjNY", "View");
                            else
                                objMain.SetParameterValue("SuppressAdjNY", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressAdjNY", "View");
                    }
                    else
                        objMain.SetParameterValue("SuppressAdjNY", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressAdjNY", "Suppress");
                break;

            //Loss Reimbursement Fund Adjustment -External
            case "INV12":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "ILRF (EXTERNAL)"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressLRFExternal", "View");
                            else
                                objMain.SetParameterValue("SuppressLRFExternal", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressLRFExternal", "View");
                            else
                                objMain.SetParameterValue("SuppressLRFExternal", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressLRFExternal", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressLRFExternal", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressLRFExternal", "Suppress");
                break;

            //Escrow Fund Adjustment
            case "INV13":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "ESCROW"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressEscrow", "View");
                            else
                                objMain.SetParameterValue("SuppressEscrow", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressEscrow", "View");
                            else
                                objMain.SetParameterValue("SuppressEscrow", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressEscrow", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressEscrow", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressEscrow", "Suppress");
                break;

            //Loss Reimbursement Fund Adjustment-Internal
            case "INV14":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "ILRF (INTERNAL)"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressLRFInternal", "View");
                            else
                                objMain.SetParameterValue("SuppressLRFInternal", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressLRFInternal", "View");
                            else
                                objMain.SetParameterValue("SuppressLRFInternal", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressLRFInternal", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressLRFInternal", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressLRFInternal", "Suppress");
                break;

            //Aries Posting Details 
            case "INV15":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "ARIES POSTING DETAILS"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressAries", "View");
                            else
                                objMain.SetParameterValue("SuppressAries", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressAries", "View");
                            else
                                objMain.SetParameterValue("SuppressAries", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressAries", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressAries", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressAries", "Suppress");
                break;


            //Combined Elements Exhibit - Internal
            case "INV16":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "COMBINED ELEMENTS"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressCombinedElements", "View");
                            else
                                objMain.SetParameterValue("SuppressCombinedElements", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressCombinedElements", "View");
                            else
                                objMain.SetParameterValue("SuppressCombinedElements", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressCombinedElements", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressCombinedElements", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressCombinedElements", "Suppress");
                break;

            //Processing and Distribution Checklist
            case "INV17":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "PROCESSING CHECKLIST"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressProcessingCheckList", "View");
                            else
                                objMain.SetParameterValue("SuppressProcessingCheckList", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressProcessingCheckList", "View");
                            else
                                objMain.SetParameterValue("SuppressProcessingCheckList", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressProcessingCheckList", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressProcessingCheckList", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressProcessingCheckList", "Suppress");
                break;

            //Cesar Coding WorkSheet
            case "INV18":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "CESAR CODING WORKSHEET"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressCesar", "View");
                            else
                                objMain.SetParameterValue("SuppressCesar", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressCesar", "View");
                            else
                                objMain.SetParameterValue("SuppressCesar", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressCesar", "View");
                    }
                    else
                        objMain.SetParameterValue("SuppressCesar", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressCesar", "Suppress");
                break;


        }


    }
    #endregion

    /// <summary>
    /// Method to set the parameters to internal Subreports
    /// </summary>
    /// <param name="objMain"></param>
    /// <param name="prmAdjNo"></param>
    /// <param name="prmFlag"></param>
    #region setSubReportParameters Method
    private void setInternalSubReportParameters(ReportDocument objMain, ParameterDiscreteValue prmAdjNo, ParameterDiscreteValue prmFlag, ParameterDiscreteValue prmFlipSigns, ParameterDiscreteValue prmInvNo, ParameterDiscreteValue prmRevFlag, ParameterDiscreteValue prmHistFlag)
    {
        /*****************Setting Sub Reports Parameters Value Begin******************/
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCumTotalsWorksheet.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptCumTotalsWorksheet.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptCumTotalsWorksheet.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCumTotalsWorksheet.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjusInvoice.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjusInvoice.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjusInvoice.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjusInvoice.rpt");
        //319-for AdjusInvoice
        objMain.SetParameterValue("@CMTCATGID", 319, "srptAdjusInvoice.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLossBasedAssessmentsCalculation.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptLossBasedAssessmentsCalculation.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptLossBasedAssessmentsCalculation.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLossBasedAssessmentsCalculation.rpt");
        //objMain.SetParameterValue("@INCLERPLBA", false, "srptLossBasedAssessmentsCalculation.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLRFInternal.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptLRFInternal.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptLRFInternal.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLRFInternal.rpt");
        //Newly added parameter
        objMain.SetParameterValue("@CMTCATGID", 318, "srptLRFInternal.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptBrokerLetter.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptBrokerLetter.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptBrokerLetter.rpt");
        //339-for Broker letter
        objMain.SetParameterValue("@CMTCATGID", 339, "srptBrokerLetter.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptEscrowFundAdjustment.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptEscrowFundAdjustment.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptEscrowFundAdjustment.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptEscrowFundAdjustment.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLRFExternal.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptLRFExternal.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptLRFExternal.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLRFExternal.rpt");
        //Newly adde parameter
        objMain.SetParameterValue("@CMTCATGID", 375, "srptLRFExternal.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptRetroPremAdjLegend.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptRetroPremAdjLegend.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptRetroPremAdjLegend.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptRetroPremAdjLegend.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCalcInfoLegend.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptCalcInfoLegend.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCalcInfoLegend.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptCalcInfoLegend.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLegendSecond.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptLegendSecond.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLegendSecond.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptLegendSecond.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptExcessLoss.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptExcessLoss.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptExcessLoss.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptExcessLoss.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptProcessingCheckList.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptProcessingCheckList.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptProcessingCheckList.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptProcessingCheckList.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("IsFlipSigns", prmFlipSigns, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCombinedElements.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptCombinedElements.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptCombinedElements.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCombinedElements.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjInvCombinedElements.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjInvCombinedElements.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjInvCombinedElements.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptRetroPremAdjustmentMain.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptRetroPremAdjustmentMain.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptRetroPremAdjustmentMain.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptRetroPremAdjustmentMain.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptARiEsPostingDetails.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptARiEsPostingDetails.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptARiEsPostingDetails.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptARiEsPostingDetails.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptClaimHandlingFeeSummary.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptClaimHandlingFeeSummary.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptClaimHandlingFeeSummary.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptClaimHandlingFeeSummary.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("IsFlipSigns", prmFlipSigns, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCesarCodingWorksheet.rpt");


        /*****************Setting Sub Reports Parameters Value End******************/
    }
    #endregion
    /// <summary>
    /// Method to set the parameters to external Subreports
    /// </summary>
    /// <param name="objMain"></param>
    /// <param name="prmAdjNo"></param>
    /// <param name="prmFlag"></param>
    #region setSubReportParameters Method
    private void setExternalSubReportParameters(ReportDocument objMain, ParameterDiscreteValue prmAdjNo, ParameterDiscreteValue prmFlag, ParameterDiscreteValue prmFlipSigns, ParameterDiscreteValue prmInvNo, ParameterDiscreteValue prmRevFlag, ParameterDiscreteValue prmHistFlag)
    {
        /*****************Setting Sub Reports Parameters Value Begin******************/
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCumTotalsWorksheet.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptCumTotalsWorksheet.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptCumTotalsWorksheet.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCumTotalsWorksheet.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjusInvoice.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjusInvoice.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjusInvoice.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjusInvoice.rpt");
        //319-for AdjusInvoice
        objMain.SetParameterValue("@CMTCATGID", 319, "srptAdjusInvoice.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLossBasedAssessmentsCalculation.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptLossBasedAssessmentsCalculation.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptLossBasedAssessmentsCalculation.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLossBasedAssessmentsCalculation.rpt");
        //objMain.SetParameterValue("@INCLERPLBA", false, "srptLossBasedAssessmentsCalculation.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLRFInternal.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptLRFInternal.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptLRFInternal.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLRFInternal.rpt");
        //Newly added parameter
        objMain.SetParameterValue("@CMTCATGID", 318, "srptLRFInternal.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptBrokerLetter.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptBrokerLetter.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptBrokerLetter.rpt");
        //339-for Broker letter
        objMain.SetParameterValue("@CMTCATGID", 339, "srptBrokerLetter.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptEscrowFundAdjustment.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptEscrowFundAdjustment.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptEscrowFundAdjustment.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptEscrowFundAdjustment.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@REVFLAGPREV", true, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLRFExternal.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptLRFExternal.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptLRFExternal.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLRFExternal.rpt");
        //Newly adde parameter
        objMain.SetParameterValue("@CMTCATGID", 375, "srptLRFExternal.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptRetroPremAdjLegend.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptRetroPremAdjLegend.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptRetroPremAdjLegend.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptRetroPremAdjLegend.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCalcInfoLegend.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptCalcInfoLegend.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCalcInfoLegend.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptCalcInfoLegend.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptLegendSecond.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptLegendSecond.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLegendSecond.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptLegendSecond.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@REVFLAGPREV", true, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptExcessLoss.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptExcessLoss.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptExcessLoss.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptExcessLoss.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptProcessingCheckList.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptProcessingCheckList.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptProcessingCheckList.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptProcessingCheckList.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("IsFlipSigns", prmFlipSigns, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@REVFLAGPREV", true, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCombinedElements.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptCombinedElements.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptCombinedElements.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCombinedElements.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjInvCombinedElements.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjInvCombinedElements.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjInvCombinedElements.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptRetroPremAdjustmentMain.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptRetroPremAdjustmentMain.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptRetroPremAdjustmentMain.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptRetroPremAdjustmentMain.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptARiEsPostingDetails.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptARiEsPostingDetails.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptARiEsPostingDetails.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptARiEsPostingDetails.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptClaimHandlingFeeSummary.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptClaimHandlingFeeSummary.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptClaimHandlingFeeSummary.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptClaimHandlingFeeSummary.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("IsFlipSigns", prmFlipSigns, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCesarCodingWorksheet.rpt");


        /*****************Setting Sub Reports Parameters Value End******************/
    }
    #endregion

    /// <summary>
    /// Method to set the parameters to Subreports
    /// </summary>
    /// <param name="objMain"></param>
    /// <param name="prmAdjNo"></param>
    /// <param name="prmFlag"></param>
    #region setSubReportParameters Method
    private void setCDWSubReportParameters(ReportDocument objMain, ParameterDiscreteValue prmAdjNo, ParameterDiscreteValue prmFlag, ParameterDiscreteValue prmFlipSigns, ParameterDiscreteValue prmInvNo, ParameterDiscreteValue prmRevFlag, ParameterDiscreteValue prmHistFlag)
    {
        /*****************Setting Sub Reports Parameters Value Begin******************/

        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("IsFlipSigns", prmFlipSigns, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("IsFlipSigns", prmFlipSigns, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCesarCodingWorksheet.rpt");

        /*****************Setting Sub Reports Parameters Value End******************/
    }
    #endregion
    /// <summary>
    /// Method to Export Report to ZDW by Invoking the Webservice
    /// </summary>
    /// <param name="objMain"></param>
    /// <param name="filename"></param>
    /// <param name="intAdjNo"></param>
    /// <param name="intFlag"></param>
    /// <param name="cFlag"></param>
    /// <returns></returns>
    #region ExportReport Method
    public string ExportReports(ReportDocument objMain, string filename, int intAdjNo, int intFlag, char cFlag)
    {
        string strMessage = string.Empty;
        try
        {
            filename = filename.Replace('/', '-');
            filename = filename.Replace(' ', '-');
            filename = filename.Replace(':', '-');

            string strZDWkey = new PremAdjustmentBS().getZDWKey(intAdjNo, cFlag, intFlag);
            // Instantiating Electronic document object and array of Document Content element of electronic document.

            ElectronicDocument objDocument = new ElectronicDocument();
            DocumentContentElement[] arrDocContent = new DocumentContentElement[1];
            // Creating array of  Properties
            Property[] arrProperty;
            if (strZDWkey == "" || strZDWkey == null)
            {
                arrProperty = new Property[4];
            }
            else
            {
                arrProperty = new Property[2];
            }
            //Property[] arrProperty = new Property[4];
            // For each dynamic property to be populated create Property object as follows
            Property objProperty1 = new Property();
            objProperty1.kind = "DocClass";
            objProperty1.theValue = "AISInvoice";
            Property objProperty2 = new Property();
            objProperty2.kind = "DocumentType";
            objProperty2.theValue = "48";
            Property objProperty3 = new Property();
            objProperty3.kind = "InvoiceNumber";
            objProperty3.theValue = strInvNo;

            if (strZDWkey == "" || strZDWkey == null)
            {
                arrProperty[0] = objProperty1;
                arrProperty[1] = objProperty2;
                arrProperty[2] = objProperty3;
            }
            else
            {
                arrProperty[0] = objProperty2;
                arrProperty[1] = objProperty3;
            }

            objDocument.dynamicProperties = arrProperty;
            //Converting to byte array
            objDocument.typeName = filename;

            Stream st;
            st = objMain.ExportToStream(ExportFormatType.PortableDocFormat);
            byte[] arr = new byte[st.Length];
            st.Read(arr, 0, (int)st.Length);


            // Creating the document content element and populating the binary value attribute with the content of document 
            DocumentContentElement objDocContent = new DocumentContentElement();
            objDocContent.theBinaryValue = arr; // Data is the byte stream of document content 

            arrDocContent[0] = objDocContent;
            objDocument.documentContentElement = arrDocContent;
            // Instantiating Java Web Services and invoking “establishdocument” method - passing document object as ref.
            // For authentication, credentials have to be passed through the SOAP Header by passing security token over the SOAP request. 
            CommunicationManagerServiceWse objJavaWS = new CommunicationManagerServiceWse();

            try
            {

                UsernameToken token = new UsernameToken(ConfigurationManager.AppSettings["ZDWUserName"].ToString(), ConfigurationManager.AppSettings["ZDWPassWord"].ToString(), PasswordOption.SendPlainText);
                objJavaWS.RequestSoapContext.Security.Timestamp.TtlInSeconds = 60;
                objJavaWS.RequestSoapContext.Security.Tokens.Add(token);
                objJavaWS.RequestSoapContext.Security.MustUnderstand = false;
                if (strZDWkey == "" || strZDWkey == null)
                    objJavaWS.establishDocument(ref objDocument);
                else
                {
                    objDocument.externalReference = strZDWkey;
                    objJavaWS.modifyDocument(ref objDocument);
                }


            }

            catch (System.Web.Services.Protocols.SoapException ex)
            {
                new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
                return ("ZDW Interface is not responding. Please try this action later.");
            }

            // objDocument.typeName; 		// Name of document saved
            string strReferenceID = objDocument.externalReference; 	// ID of document saved
            //Updation of DocID
            bool blUpdateKeys = false;
            if (intFlag == 1)
            {
                blUpdateKeys = new InvoiceDriverBS().UpdateDraftZDWKeys(objDC, intAdjNo, strReferenceID, cFlag, CurrentAISUser.PersonID);
                if (blUpdateKeys != true)
                {
                    new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Unable to Update ZDW Key's", CurrentAISUser.PersonID);
                    strMessage = "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
                    return strMessage;
                }

            }
            if (intFlag == 2)
            {

                blUpdateKeys = new InvoiceDriverBS().UpdateFinalZDWKeys(objDC, intAdjNo, strReferenceID, cFlag, CurrentAISUser.PersonID);
                if (blUpdateKeys != true)
                {
                    new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Unable to Update ZDW Key's", CurrentAISUser.PersonID);
                    strMessage = "Final Invoice could not be completed due to an error. Please check the error log for additional details";
                    return strMessage;
                }
            }
            return strMessage;
            //// //objDocument.mimeType;		 // Mime type of document
            //// objDocument = objJavaWS.retrieveDocument(strReferenceID, "D");
            //// DocumentContentElement objDocContent1 = new DocumentContentElement();
            //// //Getting the byte stream of document using the document content element's binary value property
            //// objDocContent1 = objDocument.documentContentElement[0];
            //// Response.ClearContent();
            //// Response.ClearHeaders();
            //// Response.ContentType = "application/pdf";
            //// Response.BinaryWrite(objDocContent1.theBinaryValue);
        }
        catch (Exception ex)
        {
            new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
            strMessage = "ZDW Interface is not responding. Please try this action later.";
            return strMessage;
        }

    }
    #endregion

    public string generateRevisedDraftInvoice(int iAdjNo, int iPreviousAdjNo, bool HistFlag)
    {

        try
        {
            //To check the CALC status
            PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
            IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;
            //Retrieving CALC,UWReviewed Statuses from lookup table
            int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(iAdjNo);
            //objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[objPremAdjStsBE.Count - 1].PremumAdj_sts_ID);
            objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
            //if it is Draft Invoice we are verifying the status.if status is not equal to CALC then show error

            if (objPremStsBE.ADJ_STS_TYP_ID != intAdjCalStatusID)
            {
                string strMessage = "Please do the calculation for at least one program period";
                return strMessage;
            }
            PremiumAdjustmentBE prevPremAdjBE = new PremAdjustmentBS().getPremiumAdjustmentRow(iPreviousAdjNo);
            string strPrevInvNo = prevPremAdjBE.FNL_INVC_NBR_TXT;
            bool UpdateInvNum = false;
            bool InsertStatus = false;
            objDC.Connection.Open();
            trans = objDC.Connection.BeginTransaction();
            objDC.Transaction = trans;
            //Calling Generate InvoiceNumber Function
            string strInvNo = generateInvoiceNumbers(iAdjNo, 3, iPreviousAdjNo, 1);

            ////Retrieving Draft-Invoice,Final Inv and Draft-Inv-Err Statuses from lookup table
            int intDraftInvStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.DraftInvd, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            //Draft Invoice Updation
            string strComments = string.Empty;
            UpdateInvNum = new InvoiceDriverBS().UpdatePremAdjutmentDraftInvoiceData(objDC, iAdjNo, strInvNo, CurrentAISUser.PersonID);
            InsertStatus = new InvoiceDriverBS().InsertPremAdjutmentStatusData(objDC, iAdjNo, intDraftInvStatusID, strComments, CurrentAISUser.PersonID);
            if (UpdateInvNum != true || InsertStatus != true)
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Prem_adj or Prem_adj_sts table updation Failed", CurrentAISUser.PersonID);
                return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
            }

            //Internal PDF Generation
            ReportDocument objMainRevDraftInternal = new ReportDocument();
            objMainRevDraftInternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

            //External PDF Generation
            ReportDocument objMainRevDraftExternal = new ReportDocument();
            objMainRevDraftExternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

            //Coding Work Sheet PDF Generation
            ReportDocument objMainRevDraftCDWSheet = new ReportDocument();
            objMainRevDraftCDWSheet.Load(Server.MapPath("\\Reports\\" + "rptRevisionMasterReport" + ".rpt"));

            //Internal PDF Connections
            GenerateReportConnection(objMainRevDraftInternal);
            //External PDF Connections
            GenerateReportConnection(objMainRevDraftExternal);
            //Coding Work Sheet PDF Connections
            GenerateReportConnection(objMainRevDraftCDWSheet);



            ParameterDiscreteValue prmAdjNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmPreviousAdjNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmPreviousInvNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmFlipSigns = new ParameterDiscreteValue();
            ParameterDiscreteValue prmRevFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmInvNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmHistFlag = new ParameterDiscreteValue();
            prmAdjNo.Value = iAdjNo;
            //Draft Invoice
            prmFlag.Value = 1;
            //prmPreviousAdjNo.Value = iPreviousAdjNo;
            prmFlipSigns.Value = false;
            prmRevFlag.Value = false;
            prmInvNo.Value = strInvNo;
            //prmPreviousInvNo.Value = strPrevInvNo;
            prmHistFlag.Value = HistFlag;
            /*****************Setting Master Report Parameters Value Begin******************/
            objMainRevDraftInternal.SetParameterValue("@ADJNO", prmAdjNo);
            objMainRevDraftInternal.SetParameterValue("@FLAG", prmFlag);
            objMainRevDraftExternal.SetParameterValue("@ADJNO", prmAdjNo);
            objMainRevDraftExternal.SetParameterValue("@FLAG", prmFlag);
            objMainRevDraftCDWSheet.SetParameterValue("@ADJNO", prmAdjNo);
            objMainRevDraftCDWSheet.SetParameterValue("@FLAG", prmFlag);
            /*****************Setting Master Report Parameters Value Begin******************/

            //Draft Invoice

            //Draft Invoice Internal PDF
            IList<InvoiceExhibitBE> objDrftInternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(1);
            for (int iCount = 0; iCount < objDrftInternalIlistBE.Count; iCount++)
            {

                setMasterReportParameter(objMainRevDraftInternal, objDrftInternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftInternalIlistBE[iCount].STS_IND), 1, Convert.ToChar(objDrftInternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            }

            //Draft Invoice External PDF
            IList<InvoiceExhibitBE> objDrftExternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(2);
            for (int iCount = 0; iCount < objDrftExternalIlistBE.Count; iCount++)
            {

                setMasterReportParameter(objMainRevDraftExternal, objDrftExternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftExternalIlistBE[iCount].STS_IND), 2, Convert.ToChar(objDrftExternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            }
            //Draft Coding Work Sheet PDF 
            IList<InvoiceExhibitBE> objDrftCDWorksheetIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(3);
            for (int iCount = 0; iCount < objDrftCDWorksheetIlistBE.Count; iCount++)
            {

                setMasterReportParameter(objMainRevDraftCDWSheet, objDrftCDWorksheetIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftCDWorksheetIlistBE[iCount].STS_IND), 3, Convert.ToChar(objDrftCDWorksheetIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            }


            /*****************Setting Sub Reports Parameters Value Begin******************/
            setInternalSubReportParameters(objMainRevDraftInternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            //setInternalSubReportParameters(objMainRevDraftExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            setExternalSubReportParameters(objMainRevDraftExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            //setRevisionCDWSubReportParameters(objMainRevDraftCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmPreviousAdjNo, prmPreviousInvNo, prmHistFlag);
            setRevisionCDWSubReportParameters(objMainRevDraftCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            /*****************Setting Sub Reports Parameters Value End******************/
            string strVal1;
            string strVal2;
            string strVal3;

            try
            {


                string strInternalFilename = "DraftInternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                strVal1 = ExportReports(objMainRevDraftInternal, strInternalFilename, iAdjNo, 1, 'I');

            }
            catch (Exception ee)
            {

                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
                return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
            }
            try
            {


                string strExaternalFilename = "DraftExternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                strVal2 = ExportReports(objMainRevDraftExternal, strExaternalFilename, iAdjNo, 1, 'E');

            }
            catch (Exception ee)
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
                return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
            }
            try
            {

                string strCDWorkSheetFilename = "DraftCDWorkSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                strVal3 = ExportReports(objMainRevDraftCDWSheet, strCDWorkSheetFilename, iAdjNo, 1, 'C');

            }
            catch (Exception ee)
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
                return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
            }
            objMainRevDraftInternal.Close();
            objMainRevDraftExternal.Close();
            objMainRevDraftCDWSheet.Close();
            if (strVal1 != null && strVal1 != "")
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal1, CurrentAISUser.PersonID);
                return strVal1;
            }
            else if (strVal2 != null && strVal2 != "")
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal2, CurrentAISUser.PersonID);
                return strVal2;
            }
            else if (strVal3 != null && strVal3 != "")
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal3, CurrentAISUser.PersonID);
                return strVal3;
            }
            else
            {
                objDC.SubmitChanges();
                trans.Commit();
                try
                {
                    EMailNotification(iAdjNo, 1);
                    return "The Adjustment Draft Invoice has been submitted for processing. The Draft invoice number is " + strInvNo + ". Please note that the pdf copy of the Draft invoice will be available after 15 minutes";
                }
                catch (Exception ex)
                {
                    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
                    return "The Adjustment Draft Invoice has been submitted for processing. The Draft invoice number is " + strInvNo + ". Please note that the pdf copy of the Draft invoice will be available after 15 minutes,But E-Mail Notification Failed";
                }

            }

        }
        catch (Exception ex)
        {
            trans.Rollback();
            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
            return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
        }
        return string.Empty;
    }
    /// <summary>
    /// Method to set the parameters to Subreports
    /// </summary>
    /// <param name="objMain"></param>
    /// <param name="prmAdjNo"></param>
    /// <param name="prmFlag"></param>
    #region setSubReportParameters Method
    private void setRevisionCDWSubReportParameters(ReportDocument objMain, ParameterDiscreteValue prmAdjNo, ParameterDiscreteValue prmFlag, ParameterDiscreteValue prmFlipSigns, ParameterDiscreteValue prmInvNo, ParameterDiscreteValue prmRevFlag, ParameterDiscreteValue prmHistFlag)
    {
        /*****************Setting Sub Reports Parameters Value Begin******************/

        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSecInjFund.rpt");
        objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptAdjNYSecInjFund.rpt");

        //objMain.SetParameterValue("@ADJNO", prmPreviousAdjNo, "srptAdjNYSIFRevision.rpt");
        //objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSIFRevision.rpt");
        //objMain.SetParameterValue("@INVNO", prmPreviousInvNo, "srptAdjNYSIFRevision.rpt");
        //objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSIFRevision.rpt");
        //objMain.SetParameterValue("@REVFLAGPREV", true, "srptAdjNYSIFRevision.rpt");

        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptWorkerCompTaxAssessKentOreg.rpt");
        objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptWorkerCompTaxAssessKentOreg.rpt");

        //objMain.SetParameterValue("@ADJNO", prmPreviousAdjNo, "srptKYORRevision.rpt");
        //objMain.SetParameterValue("@FLAG", prmFlag, "srptKYORRevision.rpt");
        //objMain.SetParameterValue("@INVNO", prmPreviousInvNo, "srptKYORRevision.rpt");
        //objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptKYORRevision.rpt");
        //objMain.SetParameterValue("@REVFLAGPREV", true, "srptKYORRevision.rpt");

        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("IsFlipSigns", false, "srptResidualMarkSubCharge.rpt");
        objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptResidualMarkSubCharge.rpt");

        //objMain.SetParameterValue("@ADJNO", prmPreviousAdjNo, "srptResidualMarkSubChargeRevision.rpt");
        //objMain.SetParameterValue("@FLAG", prmFlag, "srptResidualMarkSubChargeRevision.rpt");
        //objMain.SetParameterValue("@INVNO", prmPreviousInvNo, "srptResidualMarkSubChargeRevision.rpt");
        //objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptResidualMarkSubChargeRevision.rpt");
        //objMain.SetParameterValue("IsFlipSigns", true, "srptResidualMarkSubChargeRevision.rpt");

        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("IsFlipSigns", false, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptCesarCodingWorksheet.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCesarCodingWorksheet.rpt");

        //objMain.SetParameterValue("@ADJNO", prmPreviousAdjNo, "srptCesarCodingWorksheetRevision.rpt");
        //objMain.SetParameterValue("IsFlipSigns", true, "srptCesarCodingWorksheetRevision.rpt");
        //objMain.SetParameterValue("@FLAG", prmFlag, "srptCesarCodingWorksheetRevision.rpt");
        //objMain.SetParameterValue("@INVNO", prmPreviousInvNo, "srptCesarCodingWorksheetRevision.rpt");
        //objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCesarCodingWorksheetRevision.rpt");

        /*****************Setting Sub Reports Parameters Value End******************/
    }
    #endregion


    public string generateFinalInvoice(int iAdjNo, string strComments, bool HistFlag)
    {

        try
        {
            //To check the U/W status
            PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
            IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;
            //Retrieving UWReviewed Statuse from lookup table
            int intAdjUWReviewedStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.UWReviewed, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(iAdjNo);
            //objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[objPremAdjStsBE.Count - 1].PremumAdj_sts_ID);
            objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
            //if it is Final Invoice we are verifying the status.if status is not equal to U/W then show error
            if (objPremStsBE.ADJ_STS_TYP_ID == 349)
            {
                string strMessage = "Adjustment is already Finalized. Please refresh the page";
                return strMessage;
            }
            if (objPremStsBE.ADJ_STS_TYP_ID != intAdjUWReviewedStatusID && HistFlag != true)
            {
                string strMessage = "Adjustment is not reviewed by U/W";
                return strMessage;
            }

            bool UpdateInvNum = false;
            bool InsertStatus = false;
            objDC.Connection.Open();
            trans = objDC.Connection.BeginTransaction();
            objDC.Transaction = trans;
            //Calling Generate InvoiceNumber Function
            string strInvNo = generateInvoiceNumbers(iAdjNo, 2, 0, 0);

            ////Retrieving Final Inv  Statuses from lookup table
            int intFinalInvStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.FinalInvd, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            //Final Invoice Updation
            UpdateInvNum = new InvoiceDriverBS().UpdatePremAdjutmentFinalInvoiceData(objDC, iAdjNo, strInvNo, CurrentAISUser.PersonID);
            InsertStatus = new InvoiceDriverBS().InsertPremAdjutmentStatusData(objDC, iAdjNo, intFinalInvStatusID, strComments, CurrentAISUser.PersonID);
            if (UpdateInvNum != true || InsertStatus != true)
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Prem_adj or Prem_adj_sts table updation Failed", CurrentAISUser.PersonID);
                return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
            }

            //Internal PDF Generation
            ReportDocument objMainFinalInternal = new ReportDocument();
            objMainFinalInternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

            //External PDF Generation
            ReportDocument objMainFinalExternal = new ReportDocument();
            objMainFinalExternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

            //Coding Work Sheet PDF Generation
            ReportDocument objMainFinalCDWSheet = new ReportDocument();
            objMainFinalCDWSheet.Load(Server.MapPath("\\Reports\\" + "rptCodingMasterReport" + ".rpt"));

            //Internal PDF Connections
            GenerateReportConnection(objMainFinalInternal);
            //External PDF Connections
            GenerateReportConnection(objMainFinalExternal);
            //Coding Work Sheet PDF Connections
            GenerateReportConnection(objMainFinalCDWSheet);

            objMainFinalInternal.VerifyDatabase();
            objMainFinalExternal.VerifyDatabase();
            objMainFinalCDWSheet.VerifyDatabase();



            ParameterDiscreteValue prmAdjNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmFlipSigns = new ParameterDiscreteValue();
            ParameterDiscreteValue prmRevFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmInvNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmHistFlag = new ParameterDiscreteValue();
            prmAdjNo.Value = iAdjNo;
            //Final Invoice
            prmFlag.Value = 2;
            prmFlipSigns.Value = false;
            prmRevFlag.Value = false;
            prmInvNo.Value = strInvNo;
            prmHistFlag.Value = HistFlag;//Need to Change
            /*****************Setting Master Report Parameters Value Begin******************/
            objMainFinalInternal.SetParameterValue("@ADJNO", prmAdjNo);
            objMainFinalInternal.SetParameterValue("@FLAG", prmFlag);
            objMainFinalExternal.SetParameterValue("@ADJNO", prmAdjNo);
            objMainFinalExternal.SetParameterValue("@FLAG", prmFlag);
            objMainFinalCDWSheet.SetParameterValue("@ADJNO", prmAdjNo);
            objMainFinalCDWSheet.SetParameterValue("@FLAG", prmFlag);
            /*****************Setting Master Report Parameters Value Begin******************/



            //Final Invoice Internal PDF
            IList<InvoiceExhibitBE> objFinalInternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(1);
            for (int iCount = 0; iCount < objFinalInternalIlistBE.Count; iCount++)
            {

                setMasterReportParameter(objMainFinalInternal, objFinalInternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalInternalIlistBE[iCount].STS_IND), 1, Convert.ToChar(objFinalInternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            }

            //Final Invoice External PDF
            IList<InvoiceExhibitBE> objFinalExternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(2);
            for (int iCount = 0; iCount < objFinalExternalIlistBE.Count; iCount++)
            {

                setMasterReportParameter(objMainFinalExternal, objFinalExternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalExternalIlistBE[iCount].STS_IND), 2, Convert.ToChar(objFinalExternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            }
            //Final Coding Work Sheet PDF 
            IList<InvoiceExhibitBE> objFinalCDWorksheetIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(3);
            for (int iCount = 0; iCount < objFinalCDWorksheetIlistBE.Count; iCount++)
            {

                setMasterReportParameter(objMainFinalCDWSheet, objFinalCDWorksheetIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalCDWorksheetIlistBE[iCount].STS_IND), 3, Convert.ToChar(objFinalCDWorksheetIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            }


            /*****************Setting Sub Reports Parameters Value Begin******************/
            setInternalSubReportParameters(objMainFinalInternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            //setInternalSubReportParameters(objMainFinalExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            setExternalSubReportParameters(objMainFinalExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            setCDWSubReportParameters(objMainFinalCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            /*****************Setting Sub Reports Parameters Value End******************/
            string strVal1;
            string strVal2;
            string strVal3;

            try
            {


                string strInternalFilename = "FinalInternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                strVal1 = ExportReports(objMainFinalInternal, strInternalFilename, iAdjNo, 2, 'I');

            }
            catch (Exception ee)
            {

                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
                return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
            }
            try
            {


                string strExaternalFilename = "FinalExternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                strVal2 = ExportReports(objMainFinalExternal, strExaternalFilename, iAdjNo, 2, 'E');

            }
            catch (Exception ee)
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
                return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
            }
            try
            {

                string strCDWorkSheetFilename = "FinalCDWorkSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                strVal3 = ExportReports(objMainFinalCDWSheet, strCDWorkSheetFilename, iAdjNo, 2, 'C');

            }
            catch (Exception ee)
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
                return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
            }
            objMainFinalInternal.Close();
            objMainFinalExternal.Close();
            objMainFinalCDWSheet.Close();
            if (strVal1 != null && strVal1 != "")
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal1, CurrentAISUser.PersonID);
                return strVal1;
            }
            else if (strVal2 != null && strVal2 != "")
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal2, CurrentAISUser.PersonID);
                return strVal2;
            }
            else if (strVal3 != null && strVal3 != "")
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal3, CurrentAISUser.PersonID);
                return strVal3;
            }
            else
            {
                objDC.SubmitChanges();
                trans.Commit();
                ProgramPeriodsBS prgBS = new ProgramPeriodsBS();
                prgBS.ModAISTransmittalToARiES(iAdjNo, 1);
                try
                {
                    EMailNotification(iAdjNo, 2);
                    return "The Adjustment Final Invoice has been submitted for processing. The Final invoice number is " + strInvNo + ". Please note that the pdf copy of the Final invoice will be available after 15 minutes";
                }
                catch (Exception ex)
                {
                    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
                    return "The Adjustment Final Invoice has been submitted for processing. The Final invoice number is " + strInvNo + ". Please note that the pdf copy of the Final invoice will be available after 15 minutes,But E-Mail Notification Failed";
                }

            }

        }
        catch (Exception ex)
        {
            trans.Rollback();
            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
            return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
        }
        return string.Empty;
    }

    public string generateRevisedFinalInvoice(int iAdjNo, int iPreviousAdjNo, string strComments, bool HistFlag)
    {

        try
        {
            //To check the U/W status
            PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
            IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;
            //Retrieving UWReviewed Statuse from lookup table
            int intAdjUWReviewedStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.UWReviewed, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(iAdjNo);
            //objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[objPremAdjStsBE.Count - 1].PremumAdj_sts_ID);
            objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
            //if it is Final Invoice we are verifying the status.if status is not equal to U/W then show error
            if (objPremStsBE.ADJ_STS_TYP_ID == 349)
            {
                string strMessage = "Adjustment is already Finalized. Please refresh the page";
                return strMessage;
            }
            if (objPremStsBE.ADJ_STS_TYP_ID != intAdjUWReviewedStatusID && HistFlag != true)
            {
                string strMessage = "Adjustment is not reviewed by U/W";
                return strMessage;
            }
            PremiumAdjustmentBE prevPremAdjBE = new PremAdjustmentBS().getPremiumAdjustmentRow(iPreviousAdjNo);
            string strPrevInvNo = prevPremAdjBE.FNL_INVC_NBR_TXT;
            bool UpdateInvNum = false;
            bool InsertStatus = false;
            objDC.Connection.Open();
            trans = objDC.Connection.BeginTransaction();
            objDC.Transaction = trans;
            //Calling Generate InvoiceNumber Function
            string strInvNo = generateInvoiceNumbers(iAdjNo, 3, iPreviousAdjNo, 2);

            ////Retrieving Final Inv  Statuses from lookup table
            int intFinalInvStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.FinalInvd, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            //Final Invoice Updation
            UpdateInvNum = new InvoiceDriverBS().UpdatePremAdjutmentFinalInvoiceData(objDC, iAdjNo, strInvNo, CurrentAISUser.PersonID);
            InsertStatus = new InvoiceDriverBS().InsertPremAdjutmentStatusData(objDC, iAdjNo, intFinalInvStatusID, strComments, CurrentAISUser.PersonID);
            if (UpdateInvNum != true || InsertStatus != true)
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Prem_adj or Prem_adj_sts table updation Failed", CurrentAISUser.PersonID);
                return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
            }

            //Internal PDF Generation
            ReportDocument objMainRevFinlaInternal = new ReportDocument();
            objMainRevFinlaInternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

            //External PDF Generation
            ReportDocument objMainRevFinalExternal = new ReportDocument();
            objMainRevFinalExternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

            //Coding Work Sheet PDF Generation
            ReportDocument objMainRevFinalCDWSheet = new ReportDocument();
            objMainRevFinalCDWSheet.Load(Server.MapPath("\\Reports\\" + "rptRevisionMasterReport" + ".rpt"));

            //Internal PDF Connections
            GenerateReportConnection(objMainRevFinlaInternal);
            //External PDF Connections
            GenerateReportConnection(objMainRevFinalExternal);
            //Coding Work Sheet PDF Connections
            GenerateReportConnection(objMainRevFinalCDWSheet);



            ParameterDiscreteValue prmAdjNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmPreviousAdjNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmPreviousInvNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmFlipSigns = new ParameterDiscreteValue();
            ParameterDiscreteValue prmRevFlag = new ParameterDiscreteValue();
            ParameterDiscreteValue prmInvNo = new ParameterDiscreteValue();
            ParameterDiscreteValue prmHistFlag = new ParameterDiscreteValue();
            prmAdjNo.Value = iAdjNo;
            //Draft Invoice
            prmFlag.Value = 2;
            //prmPreviousAdjNo.Value = iPreviousAdjNo;
            prmFlipSigns.Value = false;
            prmRevFlag.Value = false;
            prmInvNo.Value = strInvNo;
            //prmPreviousInvNo.Value = strPrevInvNo;
            prmHistFlag.Value = HistFlag;

            /*****************Setting Master Report Parameters Value Begin******************/
            objMainRevFinlaInternal.SetParameterValue("@ADJNO", prmAdjNo);
            objMainRevFinlaInternal.SetParameterValue("@FLAG", prmFlag);
            objMainRevFinalExternal.SetParameterValue("@ADJNO", prmAdjNo);
            objMainRevFinalExternal.SetParameterValue("@FLAG", prmFlag);
            objMainRevFinalCDWSheet.SetParameterValue("@ADJNO", prmAdjNo);
            objMainRevFinalCDWSheet.SetParameterValue("@FLAG", prmFlag);
            /*****************Setting Master Report Parameters Value Begin******************/

            //Final Invoice Internal PDF
            IList<InvoiceExhibitBE> objFinalInternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(1);
            for (int iCount = 0; iCount < objFinalInternalIlistBE.Count; iCount++)
            {

                setMasterReportParameter(objMainRevFinlaInternal, objFinalInternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalInternalIlistBE[iCount].STS_IND), 1, Convert.ToChar(objFinalInternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            }

            //Final Invoice External PDF
            IList<InvoiceExhibitBE> objFinalExternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(2);
            for (int iCount = 0; iCount < objFinalExternalIlistBE.Count; iCount++)
            {

                setMasterReportParameter(objMainRevFinalExternal, objFinalExternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalExternalIlistBE[iCount].STS_IND), 2, Convert.ToChar(objFinalExternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            }
            //Final Coding Work Sheet PDF 
            IList<InvoiceExhibitBE> objFinalCDWorksheetIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(3);
            for (int iCount = 0; iCount < objFinalCDWorksheetIlistBE.Count; iCount++)
            {

                setMasterReportParameter(objMainRevFinalCDWSheet, objFinalCDWorksheetIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalCDWorksheetIlistBE[iCount].STS_IND), 3, Convert.ToChar(objFinalCDWorksheetIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            }

            /*****************Setting Sub Reports Parameters Value Begin******************/
            setInternalSubReportParameters(objMainRevFinlaInternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            //setInternalSubReportParameters(objMainRevFinalExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            setExternalSubReportParameters(objMainRevFinalExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            //setRevisionCDWSubReportParameters(objMainRevFinalCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmPreviousAdjNo, prmPreviousInvNo, prmHistFlag);
            setRevisionCDWSubReportParameters(objMainRevFinalCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            /*****************Setting Sub Reports Parameters Value End******************/
            string strVal1;
            string strVal2;
            string strVal3;

            try
            {


                string strInternalFilename = "FinalInternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                strVal1 = ExportReports(objMainRevFinlaInternal, strInternalFilename, iAdjNo, 2, 'I');

            }
            catch (Exception ee)
            {

                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
                return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
            }
            try
            {


                string strExaternalFilename = "FinalExternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                strVal2 = ExportReports(objMainRevFinalExternal, strExaternalFilename, iAdjNo, 2, 'E');

            }
            catch (Exception ee)
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
                return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
            }
            try
            {

                string strCDWorkSheetFilename = "FinalCDWorkSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                strVal3 = ExportReports(objMainRevFinalCDWSheet, strCDWorkSheetFilename, iAdjNo, 2, 'C');

            }
            catch (Exception ee)
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
                return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
            }
            objMainRevFinlaInternal.Close();
            objMainRevFinalExternal.Close();
            objMainRevFinalCDWSheet.Close();
            if (strVal1 != null && strVal1 != "")
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal1, CurrentAISUser.PersonID);
                return strVal1;
            }
            else if (strVal2 != null && strVal2 != "")
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal2, CurrentAISUser.PersonID);
                return strVal2;
            }
            else if (strVal3 != null && strVal3 != "")
            {
                trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal3, CurrentAISUser.PersonID);
                return strVal3;
            }
            else
            {
                objDC.SubmitChanges();
                trans.Commit();
                ProgramPeriodsBS prgBS = new ProgramPeriodsBS();
                prgBS.ModAISTransmittalToARiES(iAdjNo, 1);
                try
                {
                    EMailNotification(iAdjNo, 2);
                    return "The Adjustment Final Invoice has been submitted for processing. The Final invoice number is " + strInvNo + ". Please note that the pdf copy of the Final invoice will be available after 15 minutes";
                }
                catch (Exception ex)
                {
                    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
                    return "The Adjustment Final Invoice has been submitted for processing. The Final invoice number is " + strInvNo + ". Please note that the pdf copy of the Final invoice will be available after 15 minutes,But E-Mail Notification Failed";
                }

            }

        }
        catch (Exception ex)
        {
            trans.Rollback();
            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
            return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
        }
        return string.Empty;
    }
}

