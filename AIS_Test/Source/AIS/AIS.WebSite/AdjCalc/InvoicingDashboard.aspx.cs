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
using ZurichNA.AIS.WebSite.ZDWJavaWS_CAD;
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

using SSG = SpreadsheetGear;


using System.Runtime.InteropServices;
using System.Diagnostics;

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
    //Start:Parallel Processing-Delegate Declaration
    public delegate string ExportExternalDelegate(ReportDocument objMain, string filename, int intAdjNo, int intFlag, char cFlag, int intPersID);
    public delegate string ExportInternalDelegate(ReportDocument objMain, string filename, int intAdjNo, int intFlag, char cFlag, int intPersID);
    public delegate string ExportCesarDelegate(ReportDocument objMain, string filename, int intAdjNo, int intFlag, char cFlag, int intPersID);
    public delegate string ExportPorgramSummayDelegate(ReportDocument objMain, string filename, int intAdjNo, int intFlag, char cFlag, int intPersID);
    //public delegate void InternalReportParametersDelegate(ReportDocument objMain, int iAdjNo);
    //public delegate void ExternalReportParametersDelegate(ReportDocument objMain, int iAdjNo);
    public delegate void CesarReportParametersDelegate(ReportDocument objMain, int iAdjNo);
    //public delegate void FinalInternalReportParametersDelegate(ReportDocument objMain, int iAdjNo);
    //public delegate void FinalExternalReportParametersDelegate(ReportDocument objMain, int iAdjNo);
    public delegate void FinalCesarReportParametersDelegate(ReportDocument objMain, int iAdjNo);
    //End:Parallel Processing-Delegate Declaration
    AISDatabaseLINQDataContext objDC = new AISDatabaseLINQDataContext(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());

    public delegate void InternalReportParametersDelegate(int iAdjNo);
    public delegate void ExternalReportParametersDelegate(int iAdjNo);
    public delegate void FinalInternalReportParametersDelegate(int iAdjNo);
    public delegate void FinalExternalReportParametersDelegate(int iAdjNo);
    /// <summary>
    /// Varaibles Declaration for InternalInvoice(Visual Cut) 
    /// </summary>
    StringBuilder _VCsbSubParamsInt = new StringBuilder();
    StringBuilder _VCsbMasterParamsInt = new StringBuilder();
    Array _VCarrSubParamsInt = Array.CreateInstance(typeof(String), 31);
    Array _VCarrMasterParamsInt = Array.CreateInstance(typeof(String), 10);
    /// <summary>
    /// Varaibles Declaration for  ExternalInvoice(Viasul Cut) 
    /// </summary>
    StringBuilder _VCsbSubParamsExt = new StringBuilder();
    StringBuilder _VCsbMasterParamsExt = new StringBuilder();
    Array _VCarrSubParamsExt = Array.CreateInstance(typeof(String), 31);
    Array _VCarrMasterParamsExt = Array.CreateInstance(typeof(String), 10);
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
            if ((Request.QueryString.Count > 0) && !(Request.QueryString.Count == 1 && Request.QueryString["wID"] != null))
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
                LSICustomersBS AllLSICustService = new LSICustomersBS();
                IList<LSICustomerBE> LSICustmerList = AllLSICustService.getRelatedLSIAccounts(AISMasterEntities.AccountNumber);
                if (LSICustmerList != null)
                {
                    if (LSICustmerList.Count > 0)
                    {
                        bool Isplb = LSICustmerList.Where(l => l.PLB_IND == true).Any();
                        if (Isplb)
                            hidPlbInd.Value = "1";
                    }
                }
            }

        }
    }
    #endregion
    /// <summary>
    /// A Property for Holding strPGM in ViewState
    /// </summary>
    protected string StrCalcProgramPeriods
    {
        get
        {
            if (ViewState["StrCalcProgramPeriods"] != null)
            {
                return ViewState["StrCalcProgramPeriods"].ToString();
            }
            else
            {
                ViewState["StrCalcProgramPeriods"] = "";
                return "";
            }
        }
        set
        {
            ViewState["StrCalcProgramPeriods"] = value;
        }
    }


    /// <summary>
    /// A Property for Holding strPERD in ViewState
    /// </summary>
    protected string StrReCalcPeriods
    {
        get
        {
            if (ViewState["StrReCalcPeriods"] != null)
            {
                return ViewState["StrReCalcPeriods"].ToString();
            }
            else
            {
                ViewState["StrReCalcPeriods"] = "";
                return "";
            }
        }
        set
        {
            ViewState["StrReCalcPeriods"] = value;
        }
    }


    /// <summary>
    /// A Property for Holding check in ViewState
    /// </summary>
    protected bool BlPLB
    {
        get
        {
            if (ViewState["BlPLB"] != null)
            {
                return bool.Parse(ViewState["BlPLB"].ToString());
            }
            else
            {
                ViewState["BlPLB"] = false;
                return false;
            }
        }
        set
        {
            ViewState["BlPLB"] = value;
        }
    }
    /// <summary>
    /// A Property for Holding checkILRF in ViewState
    /// 
    /// </summary>
    protected bool BlILRF
    {
        get
        {
            if (ViewState["BlILRF"] != null)
            {
                return bool.Parse(ViewState["BlILRF"].ToString());
            }
            else
            {
                ViewState["BlILRF"] = false;
                return false;
            }
        }
        set
        {
            ViewState["BlILRF"] = value;
        }
    }

    /// <summary>
    /// A Property for Holding checkCHF in ViewState
    /// 
    /// </summary>
    protected bool BlCHF
    {
        get
        {
            if (ViewState["BlCHF"] != null)
            {
                return bool.Parse(ViewState["BlCHF"].ToString());
            }
            else
            {
                ViewState["BlCHF"] = false;
                return false;
            }
        }
        set
        {
            ViewState["BlCHF"] = value;
        }
    }

    /// <summary>
    /// A Property for Holding strError in ViewState
    /// </summary>
    protected string StrErrorMessage
    {
        get
        {
            if (ViewState["StrErrorMessage"] != null)
            {
                return ViewState["StrErrorMessage"].ToString();
            }
            else
            {
                ViewState["StrErrorMessage"] = "";
                return "";
            }
        }
        set
        {
            ViewState["StrErrorMessage"] = value;
        }
    }

    /// <summary>
    /// A Property for Holding alindex in ViewState
    /// </summary>
    protected ArrayList ArrayListIndex
    {
        get
        {
            if (ViewState["ArrayListIndex"] != null)
            {
                return (ArrayList)ViewState["ArrayListIndex"];

            }
            else
            {
                ArrayList alData = new ArrayList();
                ViewState["ArrayListIndex"] = alData;
                return alData;
            }
        }
        set
        {
            ViewState["ArrayListIndex"] = value;
        }
    }
    /// <summary>
    /// property to hold an instance for Draft Invoice Business Transaction Wrapper
    /// </summary>
    /// <param name=""></param>
    /// <returns>AISBusinessTransaction property</returns>
    protected AISBusinessTransaction InvoiceTransactionWrapper
    {
        get
        {
            //if ((AISBusinessTransaction)Session["InvoiceTransactionWrapper"] == null)
            //    Session["InvoiceTransactionWrapper"] = new AISBusinessTransaction();
            //return (AISBusinessTransaction)Session["InvoiceTransactionWrapper"];
            if ((AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("InvoiceTransactionWrapper") == null)
                SaveObjectToSessionUsingWindowName("InvoiceTransactionWrapper", new AISBusinessTransaction());
            return (AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("InvoiceTransactionWrapper");
        }
        set
        {
            //Session["InvoiceTransactionWrapper"] = value;
            SaveObjectToSessionUsingWindowName("InvoiceTransactionWrapper", value);
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
            //if ((AISBusinessTransaction)Session["FinalInvoiceTransactionWrapper"] == null)
            //    Session["FinalInvoiceTransactionWrapper"] = new AISBusinessTransaction();
            //return (AISBusinessTransaction)Session["FinalInvoiceTransactionWrapper"];
            if ((AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("FinalInvoiceTransactionWrapper") == null)
                SaveObjectToSessionUsingWindowName("FinalInvoiceTransactionWrapper", new AISBusinessTransaction());
            return (AISBusinessTransaction)RetrieveObjectFromSessionUsingWindowName("FinalInvoiceTransactionWrapper");
        }
        set
        {
            //Session["FinalInvoiceTransactionWrapper"] = value;
            SaveObjectToSessionUsingWindowName("FinalInvoiceTransactionWrapper", value);
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
        //Clearing the view States(SR 325928)
        this.StrCalcProgramPeriods = "";
        this.StrReCalcPeriods = "";
        this.BlPLB = false;
        this.BlILRF = false;
        this.BlCHF = false;
        this.StrErrorMessage = "";
        this.ArrayListIndex = alindex;
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
        bool checkCHF = hidConfirmCHF.Value == "yes" ? true : false;
        if (strPGM != "")
        {
            check = true;
            checkILRF = true;
            checkCHF = true;
        }
        if (strPERD == "" && strPGM == "")
        {
            ShowError("One or more adjustments could not be completed due to an error. Please check the error log for additional details");
            return;
        }
        bool Error = false;
        DropDownList ddlAccountlist = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        //AccountBE accountBE = (new AccountBS()).getAccount(Convert.ToInt32(ddlAccountlist.SelectedItem.Value));
        //SR-321581
        if (AISMasterEntities.Bpnumber == "")
        {
            modalBPNumber.Show();
            return;
        }
        string strKeyError = "";
        objDC.ModAISCheckKeyParameters(strPGM, strPERD, ref strKeyError, CurrentAISUser.PersonID, false);
        if (strKeyError != "")
        {
            modalAries.Show();
            return;
        }
        //AS per the SR 325928 we have added the following validation message which shows the popup with the other amounts related to the program periods.
        DataTable dtILRFOtherAMount = (new ProgramPeriodsBS()).GetILRFOtherAMounts(Convert.ToInt32(ddlAccountlist.SelectedItem.Value), strPGM, strPERD);
        if (dtILRFOtherAMount.Rows.Count > 0)
        {
            this.StrCalcProgramPeriods = strPGM;
            this.StrReCalcPeriods = strPERD;
            this.BlPLB = check;
            this.BlILRF = checkILRF;
            this.BlCHF = checkCHF;
            this.StrErrorMessage = strError;
            this.ArrayListIndex = alindex;
            lstilrfOtherAmount.DataSource = dtILRFOtherAMount;
            lstilrfOtherAmount.DataBind();
            modalILRFOtherAmount.Show();
            return;

        }
        string strCalcError = (new ProgramPeriodsBS()).CalcDriver(Convert.ToInt32(ddlAccountlist.SelectedItem.Value), strPGM, strPERD, check, checkILRF, CurrentAISUser.PersonID, checkCHF);
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
        if (strCalcError != "")
        {
            bool status = (new LSICustomersBS()).CheckLSIPermissions();
            if (!status)
                new ApplicationStatusLogBS().WriteLog("AIS Calculation Engine", "ERR", "Calculation error", "LSI Interface is not responding. Please contact business support group for resolution.", Convert.ToInt32(ddlAccountlist.SelectedItem.Value), CurrentAISUser.PersonID);

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
        hidConfirmCHF.Value = "";
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
                        if (lblAdjNo.Text != "")
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
        hidConfirmCHF.Value = "";
        hidConfirmILRF.Value = "";
        LSICustomersBS AllLSICustService = new LSICustomersBS();
        IList<LSICustomerBE> LSICustmerList = null;
        if (AISMasterEntities != null)
        {
            LSICustmerList = AllLSICustService.getRelatedLSIAccounts(AISMasterEntities.AccountNumber);
        }
        if (LSICustmerList != null)
        {
            if (LSICustmerList.Count > 0)
            {
                bool Isplb = LSICustmerList.Where(l => l.PLB_IND == true).Any();
                if (Isplb)
                    hidPlbInd.Value = "1";
            }
        }
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
        hidConfirmCHF.Value = "";
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
            if (chk.Checked)
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

                    strMessage = generatedraftInvoiceWithTOC(Convert.ToInt32(ddlAdjNumList.SelectedItem.Text), HistFlag);
                }
                else
                {
                    objInvPreviousBE = objInvBS.getPremiumAdjustmentRow(Convert.ToInt32(objInvBE.REL_PREM_ADJ_ID));
                    if (objInvPreviousBE.ADJ_RRSN_IND == true)
                        strMessage = generateRevisedDraftInvoiceWithTOC(Convert.ToInt32(ddlAdjNumList.SelectedItem.Text), objInvPreviousBE.PREMIUM_ADJ_ID, HistFlag);
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
                hidConfirmCHF.Value = "";
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
            new ApplicationStatusLogBS().WriteLog("AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, Convert.ToInt32(ddlAccountlist.SelectedValue), CurrentAISUser.PersonID);
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

            string strSubject = "Draft invoice " + "{" + EmailInfo[0].DRAFTINVNO + "}" + " " + " with invoice amount " + isNegitive(EmailInfo[0].INVC_AMT) + " " + "for " + " " + EmailInfo[0].CUSTMR_NAME + " " + "for " + " - " + EmailInfo[0].BUNAME;
            string strBody = "Draft invoice " + "{" + EmailInfo[0].DRAFTINVNO + "}" + " " + " has been produced for the following" + "\r\n";
            strBody = strBody + "\r\n";
            strBody = strBody + "Insured Name:" + EmailInfo[0].CUSTMR_NAME + "\r\n";
            strBody = strBody + "Broker Name:" + EmailInfo[0].BROKERNAME + "\r\n";
            strBody = strBody + "Valuation Date:" + EmailInfo[0].VALUATIONDATE + "\r\n";
            strBody = strBody + "Program Period:" + EmailInfo[0].PROGRAMPERIOD_STDT.Value.ToShortDateString() + " - " + EmailInfo[0].PROGRAMPERIOD_ENDT.Value.ToShortDateString() + "\r\n";
            strBody = strBody + "Invoice Date:" + EmailInfo[0].DRFT_INVC_DT.Value.ToShortDateString() + "\r\n";
            strBody = strBody + "Invoice Number:" + EmailInfo[0].DRAFTINVNO + "\r\n";
            strBody = strBody + "Invoice Amount: " + isNegitive(EmailInfo[0].INVC_AMT) + "\r\n";
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
            string strSubject = "Final Invoice " + "{" + EmailInfo[0].FINALINVNO + "}" + " " + " with invoice amount " + isNegitive(EmailInfo[0].INVC_AMT) + " " + "for " + " " + EmailInfo[0].CUSTMR_NAME + " " + "for BP Number " + " " + EmailInfo[0].BP_NUMBER;

            string strBody = "Final Invoice " + "{" + EmailInfo[0].FINALINVNO + "}" + " " + " has been produced for the following" + "\r\n";
            strBody = strBody + "\r\n";
            strBody = strBody + "Insured Name:" + EmailInfo[0].CUSTMR_NAME + "\r\n";
            strBody = strBody + "Broker Name:" + EmailInfo[0].BROKERNAME + "\r\n";
            strBody = strBody + "Valuation Date:" + EmailInfo[0].VALUATIONDATE + "\r\n";
            strBody = strBody + "Program Period:" + EmailInfo[0].PROGRAMPERIOD_STDT.Value.ToShortDateString() + " - " + EmailInfo[0].PROGRAMPERIOD_ENDT.Value.ToShortDateString() + "\r\n";
            strBody = strBody + "Invoice Date:" + EmailInfo[0].FNL_INVC_DT.Value.ToShortDateString() + "\r\n";
            strBody = strBody + "Invoice Number:" + EmailInfo[0].FINALINVNO + "\r\n";
            strBody = strBody + "Invoice Amount: " + isNegitive(EmailInfo[0].INVC_AMT) + "\r\n";
            strBody = strBody + "BP Number:" + EmailInfo[0].BP_NUMBER + "\r\n";
            strBody = strBody + "\r\n";
            strBody = strBody + "\r\n";
            strBody = strBody + "Copy of the Final invoice is available in ZDW or can be accessed within Adjustment Invoicing System using the Invoice Inquiry screen." + "\r\n";
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
    #region Commented Old Code
    //public string generateDraftInvoice(int iAdjNo, bool HistFlag)
    //{

    //    try
    //    {

    //        //To check the CALC status
    //        PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
    //        IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;
    //        //Retrieving CALC,UWReviewed Statuses from lookup table
    //        int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
    //        objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(iAdjNo);
    //        //objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[objPremAdjStsBE.Count - 1].PremumAdj_sts_ID);
    //        objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
    //        //if it is Draft Invoice we are verifying the status.if status is not equal to CALC then show error

    //        if (objPremStsBE.ADJ_STS_TYP_ID != intAdjCalStatusID)
    //        {
    //            string strMessage = "Please do the calculation for at least one program period";
    //            return strMessage;
    //        }

    //        bool UpdateInvNum = false;
    //        bool InsertStatus = false;
    //        objDC.Connection.Open();
    //        trans = objDC.Connection.BeginTransaction();
    //        objDC.Transaction = trans;
    //        //Calling Generate InvoiceNumber Function
    //        string strInvNo = generateInvoiceNumbers(iAdjNo, 1, 0, 0);

    //        ////Retrieving Draft-Invoice,Final Inv and Draft-Inv-Err Statuses from lookup table
    //        int intDraftInvStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.DraftInvd, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
    //        //Draft Invoice Updation
    //        string strComments = string.Empty;
    //        UpdateInvNum = new InvoiceDriverBS().UpdatePremAdjutmentDraftInvoiceData(objDC, iAdjNo, strInvNo, CurrentAISUser.PersonID);
    //        InsertStatus = new InvoiceDriverBS().InsertPremAdjutmentStatusData(objDC, iAdjNo, intDraftInvStatusID, strComments, CurrentAISUser.PersonID);
    //        if (UpdateInvNum != true || InsertStatus != true)
    //        {
    //            if (trans != null)
    //                trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Prem_adj or Prem_adj_sts table updation Failed", CurrentAISUser.PersonID);
    //            return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
    //        }

    //        //Internal PDF Generation
    //        ReportDocument objMainDraftInternal = new ReportDocument();
    //        objMainDraftInternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

    //        //External PDF Generation
    //        ReportDocument objMainDraftExternal = new ReportDocument();
    //        objMainDraftExternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

    //        //Coding Work Sheet PDF Generation
    //        ReportDocument objMainDraftCDWSheet = new ReportDocument();
    //        objMainDraftCDWSheet.Load(Server.MapPath("\\Reports\\" + "rptCodingMasterReport" + ".rpt"));

    //        //Internal PDF Connections
    //        GenerateReportConnection(objMainDraftInternal);
    //        //External PDF Connections
    //        GenerateReportConnection(objMainDraftExternal);
    //        //Coding Work Sheet PDF Connections
    //        GenerateReportConnection(objMainDraftCDWSheet);

    //        objMainDraftInternal.VerifyDatabase();
    //        objMainDraftExternal.VerifyDatabase();
    //        objMainDraftCDWSheet.VerifyDatabase();


    //        ParameterDiscreteValue prmAdjNo = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmFlag = new ParameterDiscreteValue();
    //        //ParameterDiscreteValue prmERPInd = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmFlipSigns = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmRevFlag = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmInvNo = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmHistFlag = new ParameterDiscreteValue();
    //        prmAdjNo.Value = iAdjNo;
    //        //Draft Invoice
    //        prmFlag.Value = 1;
    //        // prmERPInd.Value = false;//This is dummy variable
    //        prmFlipSigns.Value = false;
    //        prmRevFlag.Value = false;
    //        prmInvNo.Value = strInvNo;
    //        prmHistFlag.Value = HistFlag;
    //        /*****************Setting Master Report Parameters Value Begin******************/
    //        objMainDraftInternal.SetParameterValue("@ADJNO", prmAdjNo);
    //        objMainDraftInternal.SetParameterValue("@FLAG", prmFlag);
    //        objMainDraftExternal.SetParameterValue("@ADJNO", prmAdjNo);
    //        objMainDraftExternal.SetParameterValue("@FLAG", prmFlag);
    //        objMainDraftCDWSheet.SetParameterValue("@ADJNO", prmAdjNo);
    //        objMainDraftCDWSheet.SetParameterValue("@FLAG", prmFlag);
    //        /*****************Setting Master Report Parameters Value Begin******************/

    //        //Draft Invoice-Before Delegates

    //        //Draft Invoice Internal PDF
    //        //IList<InvoiceExhibitBE> objDrftInternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(1);
    //        //for (int iCount = 0; iCount < objDrftInternalIlistBE.Count; iCount++)
    //        //{

    //        //    setMasterReportParameter(objMainDraftInternal, objDrftInternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftInternalIlistBE[iCount].STS_IND), 1, Convert.ToChar(objDrftInternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

    //        //}

    //        ////Draft Invoice External PDF
    //        //IList<InvoiceExhibitBE> objDrftExternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(2);
    //        //for (int iCount = 0; iCount < objDrftExternalIlistBE.Count; iCount++)
    //        //{

    //        //    setMasterReportParameter(objMainDraftExternal, objDrftExternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftExternalIlistBE[iCount].STS_IND), 2, Convert.ToChar(objDrftExternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

    //        //}
    //        ////Draft Coding Work Sheet PDF 
    //        //IList<InvoiceExhibitBE> objDrftCDWorksheetIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(3);
    //        //for (int iCount = 0; iCount < objDrftCDWorksheetIlistBE.Count; iCount++)
    //        //{

    //        //    setMasterReportParameter(objMainDraftCDWSheet, objDrftCDWorksheetIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftCDWorksheetIlistBE[iCount].STS_IND), 3, Convert.ToChar(objDrftCDWorksheetIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

    //        //}
    //        //Draft Invoice-After Delegates
    //        InternalReportParametersDelegate delInternal = new InternalReportParametersDelegate(InternalReportParameters);
    //        ExternalReportParametersDelegate delExternal = new ExternalReportParametersDelegate(ExternalReportParameters);
    //        CesarReportParametersDelegate delCesar = new CesarReportParametersDelegate(CESARReportParameters);
    //        IAsyncResult reffdelInternal = delInternal.BeginInvoke(objMainDraftInternal, iAdjNo, null, null);
    //        IAsyncResult reffdelExternal = delExternal.BeginInvoke(objMainDraftExternal, iAdjNo, null, null);
    //        IAsyncResult reffdelCesar = delCesar.BeginInvoke(objMainDraftCDWSheet, iAdjNo, null, null);
    //        reffdelInternal.AsyncWaitHandle.WaitOne();
    //        reffdelExternal.AsyncWaitHandle.WaitOne();
    //        reffdelCesar.AsyncWaitHandle.WaitOne();
    //        delInternal.EndInvoke(reffdelInternal);
    //        delExternal.EndInvoke(reffdelExternal);
    //        delCesar.EndInvoke(reffdelCesar);

    //        /*****************Setting Sub Reports Parameters Value Begin******************/
    //        setInternalSubReportParameters(objMainDraftInternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
    //        //setInternalSubReportParameters(objMainDraftExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
    //        setExternalSubReportParameters(objMainDraftExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
    //        setCDWSubReportParameters(objMainDraftCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
    //        /*****************Setting Sub Reports Parameters Value End******************/
    //        string strVal1;
    //        string strVal2;
    //        string strVal3;

    //        //Start:parallel Process-Calling Export Methods Parallely
    //        int intPersID = CurrentAISUser.PersonID;
    //        try
    //        {
    //            string strExaternalFilename = "DraftExternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //            string strInternalFilename = "DraftInternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //            string strCDWorkSheetFilename = "DraftCDWorkSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //            ExportExternalDelegate delInstanceExternal = new ExportExternalDelegate(ExportReports);
    //            ExportInternalDelegate delInstanceInternal = new ExportInternalDelegate(ExportReports);
    //            ExportCesarDelegate delInstanceCesar = new ExportCesarDelegate(ExportReports);
    //            IAsyncResult reffExternal = delInstanceExternal.BeginInvoke(objMainDraftExternal, strExaternalFilename, iAdjNo, 1, 'E', intPersID, null, null);
    //            IAsyncResult reffInternal = delInstanceInternal.BeginInvoke(objMainDraftInternal, strInternalFilename, iAdjNo, 1, 'I', intPersID, null, null);
    //            IAsyncResult reffCesar = delInstanceCesar.BeginInvoke(objMainDraftCDWSheet, strCDWorkSheetFilename, iAdjNo, 1, 'C', intPersID, null, null);
    //            reffExternal.AsyncWaitHandle.WaitOne();
    //            reffInternal.AsyncWaitHandle.WaitOne();
    //            reffCesar.AsyncWaitHandle.WaitOne();
    //            strVal1 = delInstanceInternal.EndInvoke(reffInternal);
    //            strVal2 = delInstanceExternal.EndInvoke(reffExternal);
    //            strVal3 = delInstanceCesar.EndInvoke(reffCesar);

    //        }
    //        catch (Exception ee)
    //        {
    //            if (trans != null)
    //                trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
    //            return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
    //        }
    //        //try
    //        //{


    //        //    string strInternalFilename = "DraftInternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //        //    strVal1 = ExportReports(objMainDraftInternal, strInternalFilename, iAdjNo, 1, 'I');

    //        //}
    //        //catch (Exception ee)
    //        //{
    //        //    if (trans != null)
    //        //    trans.Rollback();
    //        //    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
    //        //    return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
    //        //}
    //        //try
    //        //{


    //        //    string strExaternalFilename = "DraftExternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //        //    strVal2 = ExportReports(objMainDraftExternal, strExaternalFilename, iAdjNo, 1, 'E');

    //        //}
    //        //catch (Exception ee)
    //        //{
    //        //    if (trans != null)
    //        //    trans.Rollback();
    //        //    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
    //        //    return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
    //        //}
    //        //try
    //        //{

    //        //    string strCDWorkSheetFilename = "DraftCDWorkSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //        //    strVal3 = ExportReports(objMainDraftCDWSheet, strCDWorkSheetFilename, iAdjNo, 1, 'C');

    //        //}
    //        //catch (Exception ee)
    //        //{
    //        //    if (trans != null)
    //        //    trans.Rollback();
    //        //    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
    //        //    return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
    //        //}
    //        objMainDraftInternal.Close();
    //        objMainDraftExternal.Close();
    //        objMainDraftCDWSheet.Close();
    //        if (strVal1 != null && strVal1 != "")
    //        {
    //            if (trans != null)
    //                trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal1, CurrentAISUser.PersonID);
    //            return strVal1;
    //        }
    //        else if (strVal2 != null && strVal2 != "")
    //        {
    //            if (trans != null)
    //                trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal2, CurrentAISUser.PersonID);
    //            return strVal2;
    //        }
    //        else if (strVal3 != null && strVal3 != "")
    //        {
    //            if (trans != null)
    //                trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal3, CurrentAISUser.PersonID);
    //            return strVal3;
    //        }
    //        else
    //        {
    //            objDC.SubmitChanges();
    //            if (trans != null)
    //                trans.Commit();
    //            try
    //            {
    //                EMailNotification(iAdjNo, 1);
    //                return "The Adjustment Draft Invoice has been submitted for processing. The Draft invoice number is " + strInvNo + ". Please note that the pdf copy of the Draft invoice will be available after 15 minutes";
    //            }
    //            catch (Exception ex)
    //            {
    //                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
    //                return "The Adjustment Draft Invoice has been submitted for processing. The Draft invoice number is " + strInvNo + ". Please note that the pdf copy of the Draft invoice will be available after 15 minutes,But E-Mail Notification Failed";
    //            }

    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        if (trans != null)
    //            trans.Rollback();
    //        new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
    //        return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
    //    }
    //    finally
    //    {
    //        if (objDC.Connection.State == ConnectionState.Open)
    //            objDC.Connection.Close();
    //    }
    //    return string.Empty;
    //} 
    #endregion
    //Draft the Invoice with Bokmarks and TOC
    public string generatedraftInvoiceWithTOC(int iAdjNo, bool HistFlag)
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

            ViewState["DraftstrInvNo"] = strInvNo;

            ////Retrieving Draft-Invoice,Final Inv and Draft-Inv-Err Statuses from lookup table
            int intDraftInvStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.DraftInvd, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            //Draft Invoice Updation
            string strComments = string.Empty;
            UpdateInvNum = new InvoiceDriverBS().UpdatePremAdjutmentDraftInvoiceData(objDC, iAdjNo, strInvNo, CurrentAISUser.PersonID);
            InsertStatus = new InvoiceDriverBS().InsertPremAdjutmentStatusData(objDC, iAdjNo, intDraftInvStatusID, strComments, CurrentAISUser.PersonID);
            if (UpdateInvNum != true || InsertStatus != true)
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Prem_adj or Prem_adj_sts table updation Failed", CurrentAISUser.PersonID);
                return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
            }

            #region Internal/External PDFs Commented  for Visual Cut
            //Internal PDF Generation
            //ReportDocument objMainDraftInternal = new ReportDocument();
            //objMainDraftInternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

            //External PDF Generation
            //ReportDocument objMainDraftExternal = new ReportDocument();
            //objMainDraftExternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

            //Internal PDF Connections
            //GenerateReportConnection(objMainDraftInternal);
            //External PDF Connections
            // GenerateReportConnection(objMainDraftExternal); 

            //objMainDraftInternal.VerifyDatabase();
            //objMainDraftExternal.VerifyDatabase();

            //objMainDraftInternal.SetParameterValue("@ADJNO", prmAdjNo);
            //objMainDraftInternal.SetParameterValue("@FLAG", prmFlag);
            //objMainDraftExternal.SetParameterValue("@ADJNO", prmAdjNo);
            //objMainDraftExternal.SetParameterValue("@FLAG", prmFlag);
            #endregion

            //Coding Work Sheet PDF Generation
            ReportDocument objMainDraftCDWSheet = new ReportDocument();
            objMainDraftCDWSheet.Load(Server.MapPath("\\Reports\\" + "rptCodingMasterReport" + ".rpt"));

            //Coding Work Sheet PDF Connections
            GenerateReportConnection(objMainDraftCDWSheet);
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
            _VCarrMasterParamsInt.SetValue("\"Parm1:" + prmAdjNo.Value.ToString() + "\"", 1);
            _VCarrMasterParamsInt.SetValue("\"Parm2:" + prmFlag.Value.ToString() + "\"", 2);
            _VCarrMasterParamsExt.SetValue("\"Parm1:" + prmAdjNo.Value.ToString() + "\"", 1);
            _VCarrMasterParamsExt.SetValue("\"Parm2:" + prmFlag.Value.ToString() + "\"", 2);
            objMainDraftCDWSheet.SetParameterValue("@ADJNO", prmAdjNo);
            objMainDraftCDWSheet.SetParameterValue("@FLAG", prmFlag);


            //Draft Invoice-After Delegates
            InternalReportParametersDelegate delInternal = new InternalReportParametersDelegate(VCInternalReportParameters);
            ExternalReportParametersDelegate delExternal = new ExternalReportParametersDelegate(VCExternalReportParameters);
            CesarReportParametersDelegate delCesar = new CesarReportParametersDelegate(CESARReportParameters);
            IAsyncResult reffdelInternal = delInternal.BeginInvoke(iAdjNo, null, null);
            IAsyncResult reffdelExternal = delExternal.BeginInvoke(iAdjNo, null, null);
            IAsyncResult reffdelCesar = delCesar.BeginInvoke(objMainDraftCDWSheet, iAdjNo, null, null);
            reffdelInternal.AsyncWaitHandle.WaitOne();
            reffdelExternal.AsyncWaitHandle.WaitOne();
            reffdelCesar.AsyncWaitHandle.WaitOne();
            delInternal.EndInvoke(reffdelInternal);
            delExternal.EndInvoke(reffdelExternal);
            delCesar.EndInvoke(reffdelCesar);

            /*****************Setting Sub Reports Parameters Value Begin******************/
            VCsetInternalSubReportParameters(prmAdjNo.Value, prmFlag.Value, prmFlipSigns.Value, prmInvNo.Value, prmRevFlag.Value, prmHistFlag.Value);
            //setInternalSubReportParameters(objMainDraftExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            VCsetExternalSubReportParameters(prmAdjNo.Value, prmFlag.Value, prmFlipSigns.Value, prmInvNo.Value, prmRevFlag.Value, prmHistFlag.Value);
            setCDWSubReportParameters(objMainDraftCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            /*****************Setting Sub Reports Parameters Value End******************/
            string strVal1;
            string strVal2;
            string strVal3;
            string strval4;

            //Start:parallel Process-Calling Export Methods Parallely
            int intPersID = CurrentAISUser.PersonID;
            try
            {
                string strExaternalFilename = "DraftExternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                string strInternalFilename = "DraftInternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                string strCDWorkSheetFilename = "DraftCDWorkSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                string strProgSummSpFilename = "DraftPSSpreadSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".xlsx";
                ExportExternalDelegate delInstanceExternal = new ExportExternalDelegate(ExportReportsWithTOC);
                ExportInternalDelegate delInstanceInternal = new ExportInternalDelegate(ExportReportsWithTOC);
                ExportCesarDelegate delInstanceCesar = new ExportCesarDelegate(ExportReportsWithTOC);
                ExportPorgramSummayDelegate delInstanceProgramSummary = new ExportPorgramSummayDelegate(ExportReportsWithTOC);
                IAsyncResult reffExternal = delInstanceExternal.BeginInvoke(null, strExaternalFilename, iAdjNo, 1, 'E', intPersID, null, null);
                IAsyncResult reffInternal = delInstanceInternal.BeginInvoke(null, strInternalFilename, iAdjNo, 1, 'I', intPersID, null, null);
                IAsyncResult reffCesar = delInstanceCesar.BeginInvoke(objMainDraftCDWSheet, strCDWorkSheetFilename, iAdjNo, 1, 'C', intPersID, null, null);
                IAsyncResult reffProgramSummary = delInstanceProgramSummary.BeginInvoke(null, strProgSummSpFilename, iAdjNo, 1, 'P', intPersID, null, null);
                reffExternal.AsyncWaitHandle.WaitOne();
                reffInternal.AsyncWaitHandle.WaitOne();
                reffCesar.AsyncWaitHandle.WaitOne();
                reffProgramSummary.AsyncWaitHandle.WaitOne();
                strVal1 = delInstanceInternal.EndInvoke(reffInternal);
                strVal2 = delInstanceExternal.EndInvoke(reffExternal);
                strVal3 = delInstanceCesar.EndInvoke(reffCesar);
                strval4 = delInstanceProgramSummary.EndInvoke(reffProgramSummary);

            }
            catch (Exception ee)
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
                return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
            }

            //objMainDraftInternal.Close();
            //objMainDraftExternal.Close();
            objMainDraftCDWSheet.Close();
            objMainDraftCDWSheet.Dispose();

            if (strVal1 != null && strVal1 != "")
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal1, CurrentAISUser.PersonID);
                return strVal1;
            }
            else if (strVal2 != null && strVal2 != "")
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal2, CurrentAISUser.PersonID);
                return strVal2;
            }
            else if (strVal3 != null && strVal3 != "")
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal3, CurrentAISUser.PersonID);
                return strVal3;
            }
            else if (strval4 != null && strval4 != "")
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strval4, CurrentAISUser.PersonID);
                return strval4;
            }
            else
            {
                objDC.SubmitChanges();
                if (trans != null)
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
            if (trans != null)
                trans.Rollback();
            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
            return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
        }
        finally
        {
            if (objDC.Connection.State == ConnectionState.Open)
                objDC.Connection.Close();
        }
        return string.Empty;

    }
    //Start:Parallel Processing-Methods to set Master Report Parameters in a loop
    public void InternalReportParameters(ReportDocument objMain, int iAdjNo)
    {
        IList<InvoiceExhibitBE> objDrftInternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(1);
        for (int iCount = 0; iCount < objDrftInternalIlistBE.Count; iCount++)
        {

            setMasterReportParameter(objMain, objDrftInternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftInternalIlistBE[iCount].STS_IND), 1, Convert.ToChar(objDrftInternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

        }

    }
    public void ExternalReportParameters(ReportDocument objMain, int iAdjNo)
    {
        IList<InvoiceExhibitBE> objDrftExternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(2);
        for (int iCount = 0; iCount < objDrftExternalIlistBE.Count; iCount++)
        {

            setMasterReportParameter(objMain, objDrftExternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftExternalIlistBE[iCount].STS_IND), 2, Convert.ToChar(objDrftExternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

        }
    }
    public void CESARReportParameters(ReportDocument objMain, int iAdjNo)
    {
        IList<InvoiceExhibitBE> objDrftCDWorksheetIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(3);
        for (int iCount = 0; iCount < objDrftCDWorksheetIlistBE.Count; iCount++)
        {

            setMasterReportParameter(objMain, objDrftCDWorksheetIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftCDWorksheetIlistBE[iCount].STS_IND), 3, Convert.ToChar(objDrftCDWorksheetIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

        }
    }
    //End:Parallel Processing-Methods to set Master Report Parameters in a loop

    //Start:Parallel Processing-Methods to set Master Report Parameters in a loop to VisualCut
    public void VCInternalReportParameters(int iAdjNo)
    {
        IList<InvoiceExhibitBE> objDrftInternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(1);
        for (int iCount = 0; iCount < objDrftInternalIlistBE.Count; iCount++)
        {

            VCsetMasterReportParameter(null, objDrftInternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftInternalIlistBE[iCount].STS_IND), 1, Convert.ToChar(objDrftInternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

        }

    }
    public void VCExternalReportParameters(int iAdjNo)
    {
        IList<InvoiceExhibitBE> objDrftExternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(2);
        for (int iCount = 0; iCount < objDrftExternalIlistBE.Count; iCount++)
        {

            VCsetMasterReportParameter(null, objDrftExternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftExternalIlistBE[iCount].STS_IND), 2, Convert.ToChar(objDrftExternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

        }
    }
    //End:Parallel Processing-Methods to set Master Report Parameters in a loop to Visual Cut
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
    public void setMasterReportParameter(ReportDocument objMain, string strAttachCode, bool blStatus,
            int intFlag, char strInternalFlag, int iAdjNo)
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
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "PAID LOSS BILLINGS FOR CURRENT ADJUSTMENT PERIOD"))
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
                                objMain.SetParameterValue("SuppressCumTotals", "Suppress");
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


            //Surcharges and Assessments
            case "INV10":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "RETRO PREMIUM BASED SURCHARGES & ASSESSMENTS"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressSurcharges", "View");
                            else
                                objMain.SetParameterValue("SuppressSurcharges", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressSurcharges", "View");
                            else
                                objMain.SetParameterValue("SuppressSurcharges", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressSurcharges", "View");
                    }
                    else
                        objMain.SetParameterValue("SuppressSurcharges", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressSurcharges", "Suppress");
                break;


            //Adjustment of NY Second Injury Fund
            //case "INV11":
            //    if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "NY-SIF"))
            //    {
            //        if (blStatus == true)
            //        {
            //            if (intFlag == 1)
            //                if (strInternalFlag == 'I' || strInternalFlag == 'B')
            //                    objMain.SetParameterValue("SuppressAdjNY", "View");
            //                else
            //                    objMain.SetParameterValue("SuppressAdjNY", "Suppress");
            //            if (intFlag == 2)
            //                if (strInternalFlag == 'E' || strInternalFlag == 'B')
            //                    objMain.SetParameterValue("SuppressAdjNY", "View");
            //                else
            //                    objMain.SetParameterValue("SuppressAdjNY", "Suppress");
            //            if (intFlag == 3)
            //                objMain.SetParameterValue("SuppressAdjNY", "View");
            //        }
            //        else
            //            objMain.SetParameterValue("SuppressAdjNY", "Suppress");
            //    }
            //    else
            //        objMain.SetParameterValue("SuppressAdjNY", "Suppress");
            //    break;

            //Loss Reimbursement Fund Adjustment -External
            case "INV11":
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
            case "INV12":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "LOSS FUND"))
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
            case "INV13":
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
            case "INV14":
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
            case "INV15":
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
            case "INV16":
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
            case "INV17":
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

            //Retrospective Tax Exhibit-External
            case "INV18":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "STATE SALES & SERVICE TAX EXHIBIT (EXTERNAL)"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressLRFTaxExternal", "View");
                            else
                                objMain.SetParameterValue("SuppressLRFTaxExternal", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressLRFTaxExternal", "View");
                            else
                                objMain.SetParameterValue("SuppressLRFTaxExternal", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressLRFTaxExternal", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressLRFTaxExternal", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressLRFTaxExternal", "Suppress");
                break;

            //Retrospective Tax Exhibit-Internal
            case "INV19":
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "STATE SALES & SERVICE TAX EXHIBIT (INTERNAL)"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressLRFTaxInternal", "View");
                            else
                                objMain.SetParameterValue("SuppressLRFTaxInternal", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                objMain.SetParameterValue("SuppressLRFTaxInternal", "View");
                            else
                                objMain.SetParameterValue("SuppressLRFTaxInternal", "Suppress");
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressLRFTaxInternal", "Suppress");
                    }
                    else
                        objMain.SetParameterValue("SuppressLRFTaxInternal", "Suppress");
                }
                else
                    objMain.SetParameterValue("SuppressLRFTaxInternal", "Suppress");
                break;


        }


    }
    #endregion
    #region setMasterReportParameters Method for Visual Cut
    public void VCsetMasterReportParameter(ReportDocument objMain, string strAttachCode,
          bool blStatus, int intFlag, char strInternalFlag, int iAdjNo)
    {
        AdjustmentReviewCommentsBS objReportAvailable = new AdjustmentReviewCommentsBS();

        switch (strAttachCode)
        {
            //Broker Letter Exhibit
            case "INV1":
                #region INV1
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "BROKER LETTER"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            //Set SuppressBrokerLetter 
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                            {
                                _VCarrMasterParamsInt.SetValue("\"Parm7:View\"", 7);
                            }
                            else
                            {
                                _VCarrMasterParamsInt.SetValue("\"Parm7:Suppress\"", 7);
                            }
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrMasterParamsExt.SetValue("\"Parm7:View\"", 7);
                            else
                                _VCarrMasterParamsExt.SetValue("\"Parm7:Suppress\"", 7);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressBrokerLetter", "Suppress");
                    }
                    else
                    {
                        if (_VCarrMasterParamsInt.GetValue(7) == null)
                            _VCarrMasterParamsInt.SetValue("\"Parm7:Suppress\"", 7);
                        if (_VCarrMasterParamsExt.GetValue(7) == null)
                            _VCarrMasterParamsExt.SetValue("\"Parm7:Suppress\"", 7);
                    }
                }
                else
                {
                    if (_VCarrMasterParamsInt.GetValue(7) == null)
                        _VCarrMasterParamsInt.SetValue("\"Parm7:Suppress\"", 7);
                    if (_VCarrMasterParamsExt.GetValue(7) == null)
                        _VCarrMasterParamsExt.SetValue("\"Parm7:Suppress\"", 7);

                }
                #endregion
                break;

            //Adjustment Invoice Exhibit
            case "INV2":
                #region Inv2
                //Set SuppressSectionAdjInv
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "SUMMARY INVOICE"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                            {
                                _VCarrMasterParamsInt.SetValue("\"Parm3:View\"", 3);
                            }

                            else
                            {
                                _VCarrMasterParamsInt.SetValue("\"Parm3:Suppress\"", 3);

                            }
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrMasterParamsExt.SetValue("\"Parm3:View\"", 3);
                            else
                                _VCarrMasterParamsExt.SetValue("\"Parm3:Suppress\"", 3);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressSectionAdjInv", "Suppress");
                    }
                    else
                    {
                        if (_VCarrMasterParamsInt.GetValue(3) == null)
                            _VCarrMasterParamsInt.SetValue("\"Parm3:Suppress\"", 3);
                        if (_VCarrMasterParamsExt.GetValue(3) == null)
                            _VCarrMasterParamsExt.SetValue("\"Parm3:Suppress\"", 3);
                    }

                }
                else
                {
                    if (_VCarrMasterParamsInt.GetValue(3) == null)
                        _VCarrMasterParamsInt.SetValue("\"Parm3:Suppress\"", 3);
                    if (_VCarrMasterParamsExt.GetValue(3) == null)
                        _VCarrMasterParamsExt.SetValue("\"Parm3:Suppress\"", 3);

                }
                #endregion
                break;

            //Retrospective Premium Adjustment Exhibit
            case "INV3":
                #region Inv3
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "RETROSPECTIVE PREMIUM ADJUSTMENT"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                            {
                                _VCarrSubParamsInt.SetValue("[Parm_16]:View", 16);
                            }
                            else
                            {
                                _VCarrSubParamsInt.SetValue("[Parm_16]:Suppress", 16);
                            }

                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrSubParamsExt.SetValue("[Parm_16]:View", 16);
                            else
                                _VCarrSubParamsExt.SetValue("[Parm_16]:Suppress", 16);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressRetroPremAdj", "Suppress");
                    }
                    else
                    {
                        if (_VCarrSubParamsInt.GetValue(16) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_16]:Suppress", 16);
                        if (_VCarrSubParamsExt.GetValue(16) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_16]:Suppress", 16);
                    }

                }
                else
                {
                    if (_VCarrSubParamsInt.GetValue(16) == null)
                        _VCarrSubParamsInt.SetValue("[Parm_16]:Suppress", 16);
                    if (_VCarrSubParamsExt.GetValue(16) == null)
                        _VCarrSubParamsExt.SetValue("[Parm_16]:Suppress", 16);

                }
                #endregion
                break;


            //Retrospective Premium Adjustment Legend Exhibit
            case "INV4":
                #region Inv4
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "RETROSPECTIVE ADJUSTMENT LEGEND"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                            {
                                _VCarrSubParamsInt.SetValue("[Parm_10]:View", 10);

                            }
                            //objMain.SetParameterValue("SuppressRetroLegend", "View");
                            else
                            {
                                _VCarrSubParamsInt.SetValue("[Parm_10]:Suppress", 10);

                            }
                        //objMain.SetParameterValue("SuppressRetroLegend", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrSubParamsExt.SetValue("[Parm_10]:View", 10);
                            else
                                _VCarrSubParamsExt.SetValue("[Parm_10]:Suppress", 10);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressRetroLegend", "Suppress");
                    }
                    else
                    {
                        if (_VCarrSubParamsInt.GetValue(10) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_10]:Suppress", 10);
                        if (_VCarrSubParamsExt.GetValue(10) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_10]:Suppress", 10);

                    }
                    //objMain.SetParameterValue("SuppressRetroLegend", "Suppress");
                }
                else
                {
                    if (_VCarrSubParamsInt.GetValue(10) == null)
                        _VCarrSubParamsInt.SetValue("[Parm_10]:Suppress", 10);
                    if (_VCarrSubParamsExt.GetValue(10) == null)
                        _VCarrSubParamsExt.SetValue("[Parm_10]:Suppress", 10);

                }
                //objMain.SetParameterValue("SuppressRetroLegend", "Suppress");
                #endregion
                break;

            //Loss Based Assessment Exhibit
            case "INV5":
                #region Inv5
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "LBA"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                            {
                                _VCarrMasterParamsInt.SetValue("\"Parm4:View\"", 4);
                            }
                            //objMain.SetParameterValue("SuppressLBA", "View");
                            else
                            {
                                _VCarrMasterParamsInt.SetValue("\"Parm4:Suppress\"", 4);
                            }
                        //objMain.SetParameterValue("SuppressLBA", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrMasterParamsExt.SetValue("\"Parm4:View\"", 4);
                            else
                                _VCarrMasterParamsExt.SetValue("\"Parm4:Suppress\"", 4);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressLBA", "Suppress");
                    }
                    else
                    {
                        if (_VCarrMasterParamsInt.GetValue(4) == null)
                            _VCarrMasterParamsInt.SetValue("\"Parm4:Suppress\"", 4);
                        if (_VCarrMasterParamsExt.GetValue(4) == null)
                            _VCarrMasterParamsExt.SetValue("\"Parm4:Suppress\"", 4);

                    }
                    //objMain.SetParameterValue("SuppressLBA", "Suppress");
                }
                else
                {
                    if (_VCarrMasterParamsInt.GetValue(4) == null)
                        _VCarrMasterParamsInt.SetValue("\"Parm4:Suppress\"", 4);
                    if (_VCarrMasterParamsExt.GetValue(4) == null)
                        _VCarrMasterParamsExt.SetValue("\"Parm4:Suppress\"", 4);

                }
                //objMain.SetParameterValue("SuppressLBA", "Suppress");
                #endregion
                break;

            //Claims Handling Fee
            case "INV6":
                #region Inv6
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "CHF"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                            {
                                _VCarrSubParamsInt.SetValue("[Parm_18]:View", 18);

                            }
                            //objMain.SetParameterValue("SuppressCHF", "View");
                            else
                            {
                                _VCarrSubParamsInt.SetValue("[Parm_18]:Suppress", 18);

                            }
                        //objMain.SetParameterValue("SuppressCHF", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrSubParamsExt.SetValue("[Parm_18]:View", 18);
                            else
                                _VCarrSubParamsExt.SetValue("[Parm_18]:Suppress", 18);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressCHF", "Suppress");
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressCHF", "Suppress");

                        if (_VCarrSubParamsInt.GetValue(18) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_18]:Suppress", 18);
                        if (_VCarrSubParamsExt.GetValue(18) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_18]:Suppress", 18);
                    }
                }
                else
                {
                    if (_VCarrSubParamsInt.GetValue(18) == null)
                        _VCarrSubParamsInt.SetValue("[Parm_18]:Suppress", 18);
                    if (_VCarrSubParamsExt.GetValue(18) == null)
                        _VCarrSubParamsExt.SetValue("[Parm_18]:Suppress", 18);
                }
                //objMain.SetParameterValue("SuppressCHF", "Suppress");
                #endregion
                break;


            //Excess Losses
            case "INV7":
                #region Inv7
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "EXCESS LOSS"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                            {
                                //objMain.SetParameterValue("SuppressExcessLoss", "View");
                                _VCarrSubParamsInt.SetValue("[Parm_11]:View", 11);

                            }
                            else
                            {
                                _VCarrSubParamsInt.SetValue("[Parm_11]:Suppress", 11);

                            }
                        //objMain.SetParameterValue("SuppressExcessLoss", "Suppress");
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrSubParamsExt.SetValue("[Parm_11]:View", 11);
                            else
                                _VCarrSubParamsExt.SetValue("[Parm_11]:Suppress", 11);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressExcessLoss", "Suppress");
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressExcessLoss", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(11) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_11]:Suppress", 11);
                        if (_VCarrSubParamsExt.GetValue(11) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_11]:Suppress", 11);
                    }

                }
                else
                {
                    //objMain.SetParameterValue("SuppressExcessLoss", "Suppress");
                    if (_VCarrSubParamsInt.GetValue(11) == null)
                        _VCarrSubParamsInt.SetValue("[Parm_11]:Suppress", 11);
                    if (_VCarrSubParamsExt.GetValue(11) == null)
                        _VCarrSubParamsExt.SetValue("[Parm_11]:Suppress", 11);
                }
                #endregion
                break;

            //Cumulative Totals Worksheet
            case "INV8":
                #region Inv8
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "PAID LOSS BILLINGS FOR CURRENT ADJUSTMENT PERIOD"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                            {
                                //objMain.SetParameterValue("SuppressCumTotals", "View");
                                _VCarrSubParamsInt.SetValue("[Parm_8]:View", 8);

                            }
                            else
                            {
                                //objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                                _VCarrSubParamsInt.SetValue("[Parm_8]:Suppress", 8);

                            }
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrSubParamsExt.SetValue("[Parm_8]:Suppress", 8);
                            else
                                _VCarrSubParamsExt.SetValue("[Parm_8]:Suppress", 8);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(8) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_8]:Suppress", 8);
                        if (_VCarrSubParamsExt.GetValue(8) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_8]:Suppress", 8);
                    }
                }
                else
                {
                    //objMain.SetParameterValue("SuppressCumTotals", "Suppress");
                    if (_VCarrSubParamsInt.GetValue(8) == null)
                        _VCarrSubParamsInt.SetValue("[Parm_8]:Suppress", 8);
                    if (_VCarrSubParamsExt.GetValue(8) == null)
                        _VCarrSubParamsExt.SetValue("[Parm_8]:Suppress", 8);
                }
                #endregion
                break;

            //Residual Market Subsidy Charge
            case "INV9":
                #region Inv9
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "RML"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                            {
                                _VCarrSubParamsInt.SetValue("[Parm_14]:View", 14);

                            }
                            //objMain.SetParameterValue("SuppressResidualMarkSubChange", "View");
                            else
                            {
                                //objMain.SetParameterValue("SuppressResidualMarkSubChange", "Suppress");
                                _VCarrSubParamsInt.SetValue("[Parm_14]:Suppress", 14);

                            }
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrSubParamsExt.SetValue("[Parm_14]:View", 14);
                            else
                                _VCarrSubParamsExt.SetValue("[Parm_14]:Suppress", 14);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressResidualMarkSubChange", "View");
                    }
                    else
                    {
                        // objMain.SetParameterValue("SuppressResidualMarkSubChange", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(14) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_14]:Suppress", 14);
                        if (_VCarrSubParamsExt.GetValue(14) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_14]:Suppress", 14);
                    }
                }
                else
                {
                    // objMain.SetParameterValue("SuppressResidualMarkSubChange", "Suppress");
                    if (_VCarrSubParamsInt.GetValue(14) == null)
                        _VCarrSubParamsInt.SetValue("[Parm_14]:Suppress", 14);
                    if (_VCarrSubParamsExt.GetValue(14) == null)
                        _VCarrSubParamsExt.SetValue("[Parm_14]:Suppress", 14);
                }
                #endregion
                break;


            //Surcharges & Assesments
            case "INV10":
                #region Inv10
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "RETRO PREMIUM BASED SURCHARGES & ASSESSMENTS"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                _VCarrSubParamsInt.SetValue("[Parm_12]:View", 12);
                            else
                                _VCarrSubParamsInt.SetValue("[Parm_12]:Suppress", 12);
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrSubParamsExt.SetValue("[Parm_12]:View", 12);
                            else
                                _VCarrSubParamsExt.SetValue("[Parm_12]:Suppress", 12);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressSurcharges", "View");
                    }
                    else
                    {
                        _VCarrSubParamsInt.SetValue("[Parm_12]:Suppress", 12);
                        _VCarrSubParamsExt.SetValue("[Parm_12]:Suppress", 12);
                    }
                }
                else
                {
                    _VCarrSubParamsInt.SetValue("[Parm_12]:Suppress", 12);
                    _VCarrSubParamsExt.SetValue("[Parm_12]:Suppress", 12);
                }
                #endregion
                break;




            //Loss Reimbursement Fund Adjustment -External
            case "INV11":
                #region Inv12
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "ILRF (EXTERNAL)"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                            {
                                //objMain.SetParameterValue("SuppressLRFExternal", "View");
                                _VCarrSubParamsInt.SetValue("[Parm_9]:View", 9);

                            }
                            else
                            {
                                //objMain.SetParameterValue("SuppressLRFExternal", "Suppress");
                                _VCarrSubParamsInt.SetValue("[Parm_9]:Suppress", 9);

                            }
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrSubParamsExt.SetValue("[Parm_9]:View", 9);
                            else
                                _VCarrSubParamsExt.SetValue("[Parm_9]:Suppress", 9);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressLRFExternal", "Suppress");
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressLRFExternal", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(9) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_9]:Suppress", 9);
                        if (_VCarrSubParamsExt.GetValue(9) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_9]:Suppress", 9);
                    }
                }
                else
                {
                    //objMain.SetParameterValue("SuppressLRFExternal", "Suppress");
                    if (_VCarrSubParamsInt.GetValue(9) == null)
                        _VCarrSubParamsInt.SetValue("[Parm_9]:Suppress", 9);
                    if (_VCarrSubParamsExt.GetValue(9) == null)
                        _VCarrSubParamsExt.SetValue("[Parm_9]:Suppress", 9);
                }
                #endregion
                break;

            //Escrow Fund Adjustment
            case "INV12":
                #region Inv13
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "LOSS FUND"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                            {
                                //objMain.SetParameterValue("SuppressEscrow", "View");
                                _VCarrMasterParamsInt.SetValue("\"Parm5:View\"", 5);

                            }
                            else
                                //objMain.SetParameterValue("SuppressEscrow", "Suppress");
                                _VCarrMasterParamsInt.SetValue("\"Parm5:Suppress\"", 5);
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrMasterParamsExt.SetValue("\"Parm5:View\"", 5);
                            else
                                _VCarrMasterParamsExt.SetValue("\"Parm5:Suppress\"", 5);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressEscrow", "Suppress");
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressEscrow", "Suppress");
                        if (_VCarrMasterParamsInt.GetValue(5) == null)
                            _VCarrMasterParamsInt.SetValue("\"Parm5:Suppress\"", 5);
                        if (_VCarrMasterParamsExt.GetValue(5) == null)
                            _VCarrMasterParamsExt.SetValue("\"Parm5:Suppress\"", 5);

                    }
                }
                else
                {
                    //objMain.SetParameterValue("SuppressEscrow", "Suppress");
                    if (_VCarrMasterParamsInt.GetValue(5) == null)
                        _VCarrMasterParamsInt.SetValue("\"Parm5:Suppress\"", 5);
                    if (_VCarrMasterParamsExt.GetValue(5) == null)
                        _VCarrMasterParamsExt.SetValue("\"Parm5:Suppress\"", 5);

                }
                #endregion
                break;

            //Loss Reimbursement Fund Adjustment-Internal
            case "INV13":
                #region Inv14
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "ILRF (INTERNAL)"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                        {
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                                //objMain.SetParameterValue("SuppressLRFInternal", "View");
                                _VCarrMasterParamsInt.SetValue("\"Parm6:View\"", 6);
                            else
                                //objMain.SetParameterValue("SuppressLRFInternal", "Suppress");
                                _VCarrMasterParamsInt.SetValue("\"Parm6:Suppress\"", 6);
                        }
                       
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrMasterParamsExt.SetValue("\"Parm6:View\"", 6);
                            else
                                _VCarrMasterParamsExt.SetValue("\"Parm6:Suppress\"", 6);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressLRFInternal", "Suppress");
                    }
                    else
                    {
                        // objMain.SetParameterValue("SuppressLRFInternal", "Suppress");
                        if (_VCarrMasterParamsInt.GetValue(6) == null)
                            _VCarrMasterParamsInt.SetValue("\"Parm6:Suppress\"", 6);
                        if (_VCarrMasterParamsExt.GetValue(6) == null)
                            _VCarrMasterParamsExt.SetValue("\"Parm6:Suppress\"", 6);

                    }
                }
                else
                {
                    //objMain.SetParameterValue("SuppressLRFInternal", "Suppress");
                    if (_VCarrMasterParamsInt.GetValue(6) == null)
                        _VCarrMasterParamsInt.SetValue("\"Parm6:Suppress\"", 6);
                    if (_VCarrMasterParamsExt.GetValue(6) == null)
                        _VCarrMasterParamsExt.SetValue("\"Parm6:Suppress\"", 6);

                }
                #endregion
                break;

            //Aries Posting Details 
            case "INV14":
                #region Inv15
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "ARIES POSTING DETAILS"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                            {
                                //objMain.SetParameterValue("SuppressAries", "View");
                                _VCarrSubParamsInt.SetValue("[Parm_17]:View", 17);

                            }
                            else
                            {
                                //objMain.SetParameterValue("SuppressAries", "Suppress");
                                _VCarrSubParamsInt.SetValue("[Parm_17]:Suppress", 17);

                            }
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrSubParamsExt.SetValue("[Parm_17]:View", 17);
                            else
                                _VCarrSubParamsExt.SetValue("[Parm_17]:Suppress", 17);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressAries", "Suppress");
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressAries", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(17) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_17]:Suppress", 17);
                        if (_VCarrSubParamsExt.GetValue(17) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_17]:Suppress", 17);
                    }
                }
                else
                {
                    //objMain.SetParameterValue("SuppressAries", "Suppress");
                    if (_VCarrSubParamsInt.GetValue(17) == null)
                        _VCarrSubParamsInt.SetValue("[Parm_17]:Suppress", 17);
                    if (_VCarrSubParamsExt.GetValue(17) == null)
                        _VCarrSubParamsExt.SetValue("[Parm_17]:Suppress", 17);
                }
                #endregion
                break;


            //Combined Elements Exhibit - Internal
            case "INV15":
                #region Inv16
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "COMBINED ELEMENTS"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                            {
                                //objMain.SetParameterValue("SuppressCombinedElements", "View");
                                _VCarrSubParamsInt.SetValue("[Parm_15]:View", 15);

                            }
                            else
                            {
                                //objMain.SetParameterValue("SuppressCombinedElements", "Suppress");
                                _VCarrSubParamsInt.SetValue("[Parm_15]:Suppress", 15);

                            }
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrSubParamsExt.SetValue("[Parm_15]:View", 15);
                            else
                                _VCarrSubParamsExt.SetValue("[Parm_15]:Suppress", 15);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressCombinedElements", "Suppress");
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressCombinedElements", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(15) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_15]:Suppress", 15);
                        if (_VCarrSubParamsExt.GetValue(15) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_15]:Suppress", 15);
                    }
                }
                else
                {
                    //objMain.SetParameterValue("SuppressCombinedElements", "Suppress");
                    if (_VCarrSubParamsInt.GetValue(15) == null)
                        _VCarrSubParamsInt.SetValue("[Parm_15]:Suppress", 15);
                    if (_VCarrSubParamsExt.GetValue(15) == null)
                        _VCarrSubParamsExt.SetValue("[Parm_15]:Suppress", 15);
                }
                #endregion
                break;

            //Processing and Distribution Checklist
            case "INV16":
                #region Inv17
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "PROCESSING CHECKLIST"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                            {
                                //objMain.SetParameterValue("SuppressProcessingCheckList", "View");
                                _VCarrSubParamsInt.SetValue("[Parm_13]:View", 13);

                            }
                            else
                            {
                                //objMain.SetParameterValue("SuppressProcessingCheckList", "Suppress");
                                _VCarrSubParamsInt.SetValue("[Parm_13]:Suppress", 13);

                            }
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrSubParamsExt.SetValue("[Parm_13]:View", 13);
                            else
                                _VCarrSubParamsExt.SetValue("[Parm_13]:Suppress", 13);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressProcessingCheckList", "Suppress");
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressProcessingCheckList", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(13) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_13]:Suppress", 13);
                        if (_VCarrSubParamsExt.GetValue(13) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_13]:Suppress", 13);

                    }
                }
                else
                {
                    //objMain.SetParameterValue("SuppressProcessingCheckList", "Suppress");
                    if (_VCarrSubParamsInt.GetValue(13) == null)
                        _VCarrSubParamsInt.SetValue("[Parm_13]:Suppress", 13);
                    if (_VCarrSubParamsExt.GetValue(13) == null)
                        _VCarrSubParamsExt.SetValue("[Parm_13]:Suppress", 13);
                }
                #endregion
                break;

            //Cesar Coding WorkSheet
            case "INV17":
                #region Inv18
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "CESAR CODING WORKSHEET"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                            {
                                //objMain.SetParameterValue("SuppressCesar", "View");
                                _VCarrSubParamsInt.SetValue("[Parm_19]:View", 19);

                            }
                            else
                            {
                                //objMain.SetParameterValue("SuppressCesar", "Suppress");
                                _VCarrSubParamsInt.SetValue("[Parm_19]:Suppress", 19);

                            }
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrSubParamsExt.SetValue("[Parm_19]:View", 19);
                            else
                                _VCarrSubParamsExt.SetValue("[Parm_19]:Suppress", 19);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressCesar", "View");
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressCesar", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(19) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_19]:Suppress", 19);
                        if (_VCarrSubParamsExt.GetValue(19) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_19]:Suppress", 19);
                    }
                }
                else
                {
                    //objMain.SetParameterValue("SuppressCesar", "Suppress");
                    if (_VCarrSubParamsInt.GetValue(19) == null)
                        _VCarrSubParamsInt.SetValue("[Parm_19]:Suppress", 19);
                    if (_VCarrSubParamsExt.GetValue(19) == null)
                        _VCarrSubParamsExt.SetValue("[Parm_19]:Suppress", 19);


                }
                #endregion
                break;
            case "INV18":
                #region Inv19
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "State Sales & Service Tax Exhibit (External)"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                            {
                                //objMain.SetParameterValue("SuppressCesar", "View");
                                _VCarrSubParamsInt.SetValue("[Parm_20]:View", 20);

                            }
                            else
                            {
                                //objMain.SetParameterValue("SuppressCesar", "Suppress");
                                _VCarrSubParamsInt.SetValue("[Parm_20]:Suppress", 20);

                            }
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrSubParamsExt.SetValue("[Parm_20]:View", 20);
                            else
                                _VCarrSubParamsExt.SetValue("[Parm_20]:Suppress", 20);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressCesar", "View");
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressCesar", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(20) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_20]:Suppress", 20);
                        if (_VCarrSubParamsExt.GetValue(20) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_20]:Suppress", 20);
                    }
                }
                else
                {
                    //objMain.SetParameterValue("SuppressCesar", "Suppress");
                    if (_VCarrSubParamsInt.GetValue(20) == null)
                        _VCarrSubParamsInt.SetValue("[Parm_20]:Suppress", 20);
                    if (_VCarrSubParamsExt.GetValue(20) == null)
                        _VCarrSubParamsExt.SetValue("[Parm_20]:Suppress", 20);


                }
                #endregion
                break;
            case "INV19":
                #region Inv19
                if (objReportAvailable.IsReportAvialable(iAdjNo, intFlag, "State Sales & Service Tax Exhibit (Internal)"))
                {
                    if (blStatus == true)
                    {
                        if (intFlag == 1)
                            if (strInternalFlag == 'I' || strInternalFlag == 'B')
                            {
                                //objMain.SetParameterValue("SuppressCesar", "View");
                                _VCarrSubParamsInt.SetValue("[Parm_21]:View", 21);

                            }
                            else
                            {
                                //objMain.SetParameterValue("SuppressCesar", "Suppress");
                                _VCarrSubParamsInt.SetValue("[Parm_21]:Suppress", 21);

                            }
                        if (intFlag == 2)
                            if (strInternalFlag == 'E' || strInternalFlag == 'B')
                                _VCarrSubParamsExt.SetValue("[Parm_21]:View", 21);
                            else
                                _VCarrSubParamsExt.SetValue("[Parm_21]:Suppress", 21);
                        if (intFlag == 3)
                            objMain.SetParameterValue("SuppressCesar", "View");
                    }
                    else
                    {
                        //objMain.SetParameterValue("SuppressCesar", "Suppress");
                        if (_VCarrSubParamsInt.GetValue(21) == null)
                            _VCarrSubParamsInt.SetValue("[Parm_21]:Suppress", 21);
                        if (_VCarrSubParamsExt.GetValue(21) == null)
                            _VCarrSubParamsExt.SetValue("[Parm_21]:Suppress", 21);
                    }
                }
                else
                {
                    //objMain.SetParameterValue("SuppressCesar", "Suppress");
                    if (_VCarrSubParamsInt.GetValue(21) == null)
                        _VCarrSubParamsInt.SetValue("[Parm_21]:Suppress", 21);
                    if (_VCarrSubParamsExt.GetValue(21) == null)
                        _VCarrSubParamsExt.SetValue("[Parm_21]:Suppress", 21);


                }
                #endregion
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
    /// Method to set the parameters to internal/External Subreports for VisualCut
    /// </summary>
    /// <param name="objMain"></param>
    /// <param name="prmAdjNo"></param>
    /// <param name="prmFlag"></param>
    public void VCsetInternalSubReportParameters(object prmAdjNo,
       object prmFlag,
       object prmFlipSigns,
       object prmInvNo,
       object prmRevFlag,
       object prmHistFlag)
    {
        if (string.IsNullOrEmpty(prmInvNo.ToString()))
            prmInvNo = "  ";
         string IntExtFlag = "I";
        _VCarrSubParamsInt.SetValue("[Parm_22]:" + prmInvNo.ToString(), 22);
        _VCarrSubParamsInt.SetValue("[Parm_23]:" + prmHistFlag.ToString(), 23);
        _VCarrSubParamsInt.SetValue("[Parm_24]:" + prmRevFlag.ToString(), 24);
        _VCarrSubParamsInt.SetValue("[Parm_25]:339", 25);
        _VCarrSubParamsInt.SetValue("[Parm_26]:319", 26);
        _VCarrSubParamsInt.SetValue("[Parm_27]:375", 27);

        _VCarrSubParamsInt.SetValue("[Parm_28]:318", 28);
        _VCarrSubParamsInt.SetValue("[Parm_29]:608", 29);
        _VCarrSubParamsInt.SetValue("[Parm_32]:" + IntExtFlag, 30);
        //return _VCarrSubParamsInt.ToString();

    }
    public void VCsetExternalSubReportParameters(object prmAdjNo, object prmFlag, object prmFlipSigns, object prmInvNo, object prmRevFlag, object prmHistFlag)
    {
        //  _VCarrSubParamsExt.SetValue("[Parm_12]:View", 12);

        string IntExtFlag = "E";

        _VCarrSubParamsExt.SetValue("[Parm_22]:" + prmInvNo.ToString(), 22);
        _VCarrSubParamsExt.SetValue("[Parm_23]:" + prmHistFlag.ToString(), 23);
        _VCarrSubParamsExt.SetValue("[Parm_24]:true", 24);
        _VCarrSubParamsExt.SetValue("[Parm_25]:339", 25);
        _VCarrSubParamsExt.SetValue("[Parm_26]:319", 26);
        _VCarrSubParamsExt.SetValue("[Parm_27]:375", 27);
        _VCarrSubParamsExt.SetValue("[Parm_28]:318", 28);
        _VCarrSubParamsExt.SetValue("[Parm_29]:608", 29);
        //_VCarrSubParamsExt.SetValue("[Parm_30]:View", 30);
        _VCarrSubParamsExt.SetValue("[Parm_32]:" + IntExtFlag, 30);
        //return _VCarrSubParamsExt.ToString();
        //return _VCarrSubParamsExt.ToString();

    }
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

        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptSurchargesAssessments.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptSurchargesAssessments.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptSurchargesAssessments.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptSurchargesAssessments.rpt");
        objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptSurchargesAssessments.rpt");
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
    //public string ExportReports(ReportDocument objMain, string filename, int intAdjNo, int intFlag, char cFlag, int intPersID)
    //{



    //    string strMessage = string.Empty;
    //    try
    //    {
    //        filename = filename.Replace('/', '-');
    //        filename = filename.Replace(' ', '-');
    //        filename = filename.Replace(':', '-');

    //        string strZDWkey = new PremAdjustmentBS().getZDWKey(intAdjNo, cFlag, intFlag);
    //        // Instantiating Electronic document object and array of Document Content element of electronic document.

    //        ZurichNA.AIS.WebSite.ZDWJavaWS.ElectronicDocument objDocument = new ZurichNA.AIS.WebSite.ZDWJavaWS.ElectronicDocument();
    //        ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement[] arrDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement[1];
    //        // Creating array of  Properties
    //        ZurichNA.AIS.WebSite.ZDWJavaWS.Property[] arrProperty;
    //        if (strZDWkey == "" || strZDWkey == null)
    //        {
    //            arrProperty = new ZurichNA.AIS.WebSite.ZDWJavaWS.Property[4];
    //        }
    //        else
    //        {
    //            arrProperty = new ZurichNA.AIS.WebSite.ZDWJavaWS.Property[2];
    //        }
    //        //Property[] arrProperty = new Property[4];
    //        // For each dynamic property to be populated create Property object as follows
    //        ZurichNA.AIS.WebSite.ZDWJavaWS.Property objProperty1 = new ZurichNA.AIS.WebSite.ZDWJavaWS.Property();
    //        objProperty1.kind = "DocClass";
    //        objProperty1.theValue = "AISInvoice";
    //        ZurichNA.AIS.WebSite.ZDWJavaWS.Property objProperty2 = new ZurichNA.AIS.WebSite.ZDWJavaWS.Property();
    //        objProperty2.kind = "DocumentType";
    //        objProperty2.theValue = "48";
    //        ZurichNA.AIS.WebSite.ZDWJavaWS.Property objProperty3 = new ZurichNA.AIS.WebSite.ZDWJavaWS.Property();
    //        objProperty3.kind = "InvoiceNumber";
    //        objProperty3.theValue = strInvNo;

    //        if (strZDWkey == "" || strZDWkey == null)
    //        {
    //            arrProperty[0] = objProperty1;
    //            arrProperty[1] = objProperty2;
    //            arrProperty[2] = objProperty3;
    //        }
    //        else
    //        {
    //            arrProperty[0] = objProperty2;
    //            arrProperty[1] = objProperty3;
    //        }

    //        objDocument.dynamicProperties = arrProperty;
    //        //Converting to byte array
    //        objDocument.typeName = filename;

    //        MemoryStream st;
    //        st = (MemoryStream)objMain.ExportToStream(ExportFormatType.PortableDocFormat);
    //        //byte[] arr = new byte[st.Length];
    //        //st.Read(arr, 0, (int)st.Length);
    //        byte[] arr = st.ToArray();


    //        // Creating the document content element and populating the binary value attribute with the content of document 
    //        ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement objDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement();
    //        objDocContent.theBinaryValue = arr; // Data is the byte stream of document content 

    //        arrDocContent[0] = objDocContent;
    //        objDocument.documentContentElement = arrDocContent;
    //        // Instantiating Java Web Services and invoking “establishdocument” method - passing document object as ref.
    //        // For authentication, credentials have to be passed through the SOAP Header by passing security token over the SOAP request. 
    //        CommunicationManagerServiceWse objJavaWS = new CommunicationManagerServiceWse();

    //        try
    //        {

    //            UsernameToken token = new UsernameToken(ConfigurationManager.AppSettings["ZDWUserName"].ToString(), ConfigurationManager.AppSettings["ZDWPassWord"].ToString(), PasswordOption.SendPlainText);
    //            objJavaWS.RequestSoapContext.Security.Timestamp.TtlInSeconds = 60;
    //            objJavaWS.RequestSoapContext.Security.Tokens.Add(token);
    //            objJavaWS.RequestSoapContext.Security.MustUnderstand = false;
    //            if (strZDWkey == "" || strZDWkey == null)
    //                objJavaWS.establishDocument(ref objDocument);
    //            else
    //            {
    //                objDocument.externalReference = strZDWkey;
    //                objJavaWS.modifyDocument(ref objDocument);
    //            }


    //        }

    //        catch (System.Web.Services.Protocols.SoapException ex)
    //        {
    //            //CurrentAISUser.PersonID
    //            new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, intPersID);
    //            return ("ZDW Interface is not responding. Please try this action later.");
    //        }

    //        // objDocument.typeName; 		// Name of document saved
    //        string strReferenceID = objDocument.externalReference; 	// ID of document saved
    //        //Updation of DocID
    //        bool blUpdateKeys = false;
    //        if (intFlag == 1)
    //        {
    //            //CurrentAISUser.PersonID
    //            blUpdateKeys = new InvoiceDriverBS().UpdateDraftZDWKeys(objDC, intAdjNo, strReferenceID, cFlag, intPersID);
    //            if (blUpdateKeys != true)
    //            {
    //                new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Unable to Update ZDW Key's", intPersID);
    //                strMessage = "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
    //                return strMessage;
    //            }

    //        }
    //        if (intFlag == 2)
    //        {
    //            //CurrentAISUser.PersonID
    //            blUpdateKeys = new InvoiceDriverBS().UpdateFinalZDWKeys(objDC, intAdjNo, strReferenceID, cFlag, intPersID);
    //            if (blUpdateKeys != true)
    //            {
    //                new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Unable to Update ZDW Key's", intPersID);
    //                strMessage = "Final Invoice could not be completed due to an error. Please check the error log for additional details";
    //                return strMessage;
    //            }
    //        }
    //        return strMessage;
    //        //// //objDocument.mimeType;		 // Mime type of document
    //        //// objDocument = objJavaWS.retrieveDocument(strReferenceID, "D");
    //        //// DocumentContentElement objDocContent1 = new DocumentContentElement();
    //        //// //Getting the byte stream of document using the document content element's binary value property
    //        //// objDocContent1 = objDocument.documentContentElement[0];
    //        //// Response.ClearContent();
    //        //// Response.ClearHeaders();
    //        //// Response.ContentType = "application/pdf";
    //        //// Response.BinaryWrite(objDocContent1.theBinaryValue);
    //    }
    //    catch (Exception ex)
    //    {
    //        //CurrentAISUser.PersonID
    //        new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, intPersID);
    //        strMessage = "ZDW Interface is not responding. Please try this action later.";
    //        return strMessage;
    //    }

    //}
    #endregion
    public string ExportReportsWithTOC(ReportDocument objMain, string filename, int intAdjNo, int intFlag, char cFlag, int intPersID)
    {

        bool IsCanadaAcct = (new AccountBS()).IsCanadaAccount(intAdjNo);

        if (IsCanadaAcct)
        {
            string strMessage = string.Empty;
            try
            {
                filename = filename.Replace('/', '-');
                filename = filename.Replace(' ', '-');
                filename = filename.Replace(':', '-');

                string strZDWkey = new PremAdjustmentBS().getZDWKey(intAdjNo, cFlag, intFlag);
                // Instantiating Electronic document object and array of Document Content element of electronic document.

                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.ElectronicDocument objDocument = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.ElectronicDocument();
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement[] arrDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement[1];
                // Creating array of  Properties
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property[] arrProperty;
                if (strZDWkey == "" || strZDWkey == null)
                {
                    arrProperty = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property[4];
                }
                else
                {
                    arrProperty = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property[2];
                }
                //Property[] arrProperty = new Property[4];
                // For each dynamic property to be populated create Property object as follows
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property objProperty1 = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property();
                objProperty1.kind = "DocClass";
                objProperty1.theValue = "ZDW_DOC04.AISInvoice";
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property objProperty2 = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property();
                objProperty2.kind = "DocumentType";
                objProperty2.theValue = "48";
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property objProperty3 = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.Property();
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

                //Visual Cut EXE Shell Execution Script for Invoice generation with Bookmark/TOC
                if (cFlag == 'I' || cFlag == 'E')
                {
                    string[] strArray = VCshellScriptExecution(objMain, filename, intAdjNo, intFlag, cFlag, intPersID);
                    if (strArray != null)
                    {
                        if (strArray[0] == "I" || strArray[0] == "E")
                        {
                            FileStream fs = new FileStream(strArray[1], FileMode.OpenOrCreate, FileAccess.Read);
                            byte[] bc = new byte[fs.Length];
                            fs.Read(bc, 0, (Int32)fs.Length);
                            fs.Close();
                            // Creating the document content element and populating the binary value attribute with the content of document 
                            ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement objDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement();
                            objDocContent.theBinaryValue = bc; // Data is the byte stream of document content 

                            arrDocContent[0] = objDocContent;
                            objDocument.documentContentElement = arrDocContent;
                        }
                    }
                    else
                    {
                        return "Path Configuration Error";
                    }
                }
                if (cFlag == 'C')
                {
                    //MemoryStream st; -- WinServer Changes for crystal Reports
                    Stream st;
                    //st = (MemoryStream)objMain.ExportToStream(ExportFormatType.PortableDocFormat);
                    st = (Stream)objMain.ExportToStream(ExportFormatType.PortableDocFormat);
                    byte[] arr = new byte[st.Length];
                    st.Read(arr, 0, (int)st.Length);
                    //byte[] arr = st.ToArray();


                    // Creating the document content element and populating the binary value attribute with the content of document 
                    ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement objDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement();
                    objDocContent.theBinaryValue = arr; // Data is the byte stream of document content 

                    arrDocContent[0] = objDocContent;
                    objDocument.documentContentElement = arrDocContent;
                }
                if (cFlag == 'P')
                {
                    string strInvNo1 = string.Empty;

                    string[] strArray=new string[2];
                    if (intFlag == 1)
                    {
                        if (Convert.ToString(ViewState["DraftstrInvNo"]) != string.Empty && Convert.ToString(ViewState["DraftstrInvNo"]) != "")
                        {
                            strInvNo1 = ViewState["DraftstrInvNo"].ToString();
                            ViewState["DraftstrInvNo"] = "";
                        }
                        strArray = DownloadFileProgramSummaySpreadsheetGear(filename, intAdjNo, strInvNo1, cFlag, intPersID, "Draft");
                    }
                    else if (intFlag == 2)
                    {
                        if (Convert.ToString(ViewState["FinalstrInvNo"]) != string.Empty && Convert.ToString(ViewState["FinalstrInvNo"]) != "")
                        {
                            strInvNo1 = ViewState["FinalstrInvNo"].ToString();
                            ViewState["FinalstrInvNo"] = "";
                        }
                        strArray = DownloadFileProgramSummaySpreadsheetGear(filename, intAdjNo, strInvNo1, cFlag, intPersID, "Final");
                    }
                    FileStream fs = new FileStream(strArray[1], FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] bc = new byte[fs.Length];
                    fs.Read(bc, 0, (Int32)fs.Length);
                    fs.Close();
                    // Creating the document content element and populating the binary value attribute with the content of document 
                    ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement objDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.DocumentContentElement();
                    objDocContent.theBinaryValue = bc; // Data is the byte stream of document content 

                    arrDocContent[0] = objDocContent;
                    objDocument.documentContentElement = arrDocContent;
                }
                //
                // Instantiating Java Web Services and invoking “establishdocument” method - passing document object as ref.
                // For authentication, credentials have to be passed through the SOAP Header by passing security token over the SOAP request. 
                ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.CommunicationManagerService objJavaWS = new ZurichNA.AIS.WebSite.ZDWJavaWS_CAD.CommunicationManagerService();

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
                        objDocument.externalReference = "ZDW_DOC04." + strZDWkey;
                        objJavaWS.modifyDocument(ref objDocument);
                    }


                }

                catch (System.Web.Services.Protocols.SoapException ex)
                {
                    //CurrentAISUser.PersonID
                    new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, intPersID);
                    return ("ZDW Interface is not responding. Please try this action later.");
                }

                // objDocument.typeName; 		// Name of document saved
                string strReferenceID = objDocument.externalReference; 	// ID of document saved
                //Updation of DocID
                bool blUpdateKeys = false;
                if (intFlag == 1)
                {
                    //CurrentAISUser.PersonID
                    blUpdateKeys = new InvoiceDriverBS().UpdateDraftZDWKeys(objDC, intAdjNo, strReferenceID, cFlag, intPersID);
                    DataTable dtDraftProgSumm = new DataTable();
                    if (cFlag.ToString() == "P")
                    {
                         dtDraftProgSumm = new InvoiceDriverBS().UpdateDraftZDWKey_ProgramSummary(intAdjNo, strReferenceID, intPersID);
                    }
                    
                    if (blUpdateKeys != true)
                    {
                        new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Unable to Update ZDW Key's", intPersID);
                        strMessage = "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
                        return strMessage;
                    }
                    if (blUpdateKeys == true && dtDraftProgSumm == null)
                    {

                        new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Unable to Update ZDW Key's for ProgramSummarySpreadsheet", intPersID);
                        strMessage = "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
                        return strMessage;
                    }

                }
                if (intFlag == 2)
                {
                    //CurrentAISUser.PersonID
                    blUpdateKeys = new InvoiceDriverBS().UpdateFinalZDWKeys(objDC, intAdjNo, strReferenceID, cFlag, intPersID);
                    DataTable dtFinalProgSumm = new DataTable();
                    if (cFlag.ToString() == "P")
                    {
                        dtFinalProgSumm = new InvoiceDriverBS().UpdateFinalZDWKey_ProgramSummary(intAdjNo, strReferenceID, intPersID);
                    }
                    
                    if (blUpdateKeys != true)
                    {
                        new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Unable to Update ZDW Key's", intPersID);
                        strMessage = "Final Invoice could not be completed due to an error. Please check the error log for additional details";
                        return strMessage;
                    }
                    if (blUpdateKeys == true && dtFinalProgSumm == null)
                    {

                        new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Unable to Update ZDW Key's for ProgramSummarySpreadsheet", intPersID);
                        strMessage = "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
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
                //CurrentAISUser.PersonID
                new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, intPersID);
                strMessage = "ZDW Interface is not responding. Please try this action later.";
                return strMessage;
            }
        }

        else
        {
            string strMessage = string.Empty;
            try
            {
                filename = filename.Replace('/', '-');
                filename = filename.Replace(' ', '-');
                filename = filename.Replace(':', '-');

                string strZDWkey = new PremAdjustmentBS().getZDWKey(intAdjNo, cFlag, intFlag);
                // Instantiating Electronic document object and array of Document Content element of electronic document.

                ZurichNA.AIS.WebSite.ZDWJavaWS.ElectronicDocument objDocument = new ZurichNA.AIS.WebSite.ZDWJavaWS.ElectronicDocument();
                ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement[] arrDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement[1];
                // Creating array of  Properties
                ZurichNA.AIS.WebSite.ZDWJavaWS.Property[] arrProperty;
                if (strZDWkey == "" || strZDWkey == null)
                {
                    arrProperty = new ZurichNA.AIS.WebSite.ZDWJavaWS.Property[4];
                }
                else
                {
                    arrProperty = new ZurichNA.AIS.WebSite.ZDWJavaWS.Property[2];
                }
                //Property[] arrProperty = new Property[4];
                // For each dynamic property to be populated create Property object as follows
                ZurichNA.AIS.WebSite.ZDWJavaWS.Property objProperty1 = new ZurichNA.AIS.WebSite.ZDWJavaWS.Property();
                objProperty1.kind = "DocClass";
                objProperty1.theValue = "AISInvoice";
                ZurichNA.AIS.WebSite.ZDWJavaWS.Property objProperty2 = new ZurichNA.AIS.WebSite.ZDWJavaWS.Property();
                objProperty2.kind = "DocumentType";
                objProperty2.theValue = "48";
                ZurichNA.AIS.WebSite.ZDWJavaWS.Property objProperty3 = new ZurichNA.AIS.WebSite.ZDWJavaWS.Property();
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

                //Visual Cut EXE Shell Execution Script for Invoice generation with Bookmark/TOC
                if (cFlag == 'I' || cFlag == 'E')
                {
                    string[] strArray = VCshellScriptExecution(objMain, filename, intAdjNo, intFlag, cFlag, intPersID);
                    if (strArray != null)
                    {
                        if (strArray[0] == "I" || strArray[0] == "E")
                        {
                            FileStream fs = new FileStream(strArray[1], FileMode.OpenOrCreate, FileAccess.Read);
                            byte[] bc = new byte[fs.Length];
                            fs.Read(bc, 0, (Int32)fs.Length);
                            fs.Close();
                            // Creating the document content element and populating the binary value attribute with the content of document 
                            ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement objDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement();
                            objDocContent.theBinaryValue = bc; // Data is the byte stream of document content 

                            arrDocContent[0] = objDocContent;
                            objDocument.documentContentElement = arrDocContent;
                        }
                    }
                    else
                    {
                        return "Path Configuration Error";
                    }
                }
                if (cFlag == 'C')
                {
                    //MemoryStream st;
                    Stream st;
                    //st = (MemoryStream)objMain.ExportToStream(ExportFormatType.PortableDocFormat);
                    st = (Stream)objMain.ExportToStream(ExportFormatType.PortableDocFormat);
                    byte[] arr = new byte[st.Length];
                    st.Read(arr, 0, (int)st.Length);
                    //byte[] arr = st.ToArray();


                    // Creating the document content element and populating the binary value attribute with the content of document 
                    ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement objDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement();
                    objDocContent.theBinaryValue = arr; // Data is the byte stream of document content 

                    arrDocContent[0] = objDocContent;
                    objDocument.documentContentElement = arrDocContent;
                }
                if (cFlag == 'P')
                {
                    string strInvNo1=string.Empty;

                    string[] strArray = new string[2];
                    if (intFlag == 1)
                    {
                        if (Convert.ToString(ViewState["DraftstrInvNo"]) != string.Empty && Convert.ToString(ViewState["DraftstrInvNo"]) != "")
                        {
                            strInvNo1 = ViewState["DraftstrInvNo"].ToString();
                            ViewState["DraftstrInvNo"] = "";
                        }
                        strArray = DownloadFileProgramSummaySpreadsheetGear(filename, intAdjNo, strInvNo1, cFlag, intPersID, "Draft");
                    }
                    else if (intFlag == 2)
                    {
                        if (Convert.ToString(ViewState["FinalstrInvNo"]) != string.Empty && Convert.ToString(ViewState["FinalstrInvNo"]) != "")
                        {
                            strInvNo1 = ViewState["FinalstrInvNo"].ToString();
                            ViewState["FinalstrInvNo"] = "";
                        }
                        strArray = DownloadFileProgramSummaySpreadsheetGear(filename, intAdjNo, strInvNo1, cFlag, intPersID, "Final");
                    }
                    FileStream fs = new FileStream(strArray[1], FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] bc = new byte[fs.Length];
                    fs.Read(bc, 0, (Int32)fs.Length);
                    fs.Close();
                    // Creating the document content element and populating the binary value attribute with the content of document 
                    ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement objDocContent = new ZurichNA.AIS.WebSite.ZDWJavaWS.DocumentContentElement();
                    objDocContent.theBinaryValue = bc; // Data is the byte stream of document content 

                    arrDocContent[0] = objDocContent;
                    objDocument.documentContentElement = arrDocContent;
                }
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
                    //CurrentAISUser.PersonID
                    new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, intPersID);
                    return ("ZDW Interface is not responding. Please try this action later.");
                }

                // objDocument.typeName; 		// Name of document saved
                string strReferenceID = objDocument.externalReference; 	// ID of document saved
                //Updation of DocID
                bool blUpdateKeys = false;
                if (intFlag == 1)
                {
                    //CurrentAISUser.PersonID
                    blUpdateKeys = new InvoiceDriverBS().UpdateDraftZDWKeys(objDC, intAdjNo, strReferenceID, cFlag, intPersID);
                    DataTable dtDraftProgSumm = new DataTable();
                    if (cFlag.ToString() == "P")
                    {
                        dtDraftProgSumm = new InvoiceDriverBS().UpdateDraftZDWKey_ProgramSummary(intAdjNo, strReferenceID, intPersID);
                    }
                    if (blUpdateKeys != true)
                    {
                        new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Unable to Update ZDW Key's", intPersID);
                        strMessage = "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
                        return strMessage;
                    }
                    if (blUpdateKeys == true && dtDraftProgSumm == null)
                    {

                        new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Unable to Update ZDW Key's for ProgramSummarySpreadsheet", intPersID);
                        strMessage = "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
                        return strMessage;
                    }

                }
                if (intFlag == 2)
                {
                    //CurrentAISUser.PersonID
                    blUpdateKeys = new InvoiceDriverBS().UpdateFinalZDWKeys(objDC, intAdjNo, strReferenceID, cFlag, intPersID);
                    DataTable dtFinalProgSumm = new DataTable();
                    if (cFlag.ToString() == "P")
                    {
                        dtFinalProgSumm = new InvoiceDriverBS().UpdateFinalZDWKey_ProgramSummary(intAdjNo, strReferenceID, intPersID);
                    }
                    
                    if (blUpdateKeys != true)
                    {
                        new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Unable to Update ZDW Key's", intPersID);
                        strMessage = "Final Invoice could not be completed due to an error. Please check the error log for additional details";
                        return strMessage;
                    }
                    if (blUpdateKeys == true && dtFinalProgSumm == null)
                    {

                        new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Unable to Update ZDW Key's for ProgramSummarySpreadsheet", intPersID);
                        strMessage = "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
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
                //CurrentAISUser.PersonID
                new ApplicationStatusLogBS().WriteLog(intAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, intPersID);
                strMessage = "ZDW Interface is not responding. Please try this action later.";
                return strMessage;
            }
        }
    }
    private string[] VCshellScriptExecution(ReportDocument objMain, string filename, int intAdjNo, int intFlag, char cFlag, int intPersID)
    {

        #region StartShelll
        ProcessStartInfo startInfo;
        Process batchExecute;
        string[] strArray = new string[2];
        string setSubParamsINT = string.Empty;
        string setSubParamsEXT = string.Empty;
        string vcExePath = string.Empty;
        string vcwDirectory = string.Empty;
        string tocUSImagePath = string.Empty;
        string tocCanadaImagePath = string.Empty;
        try
        {
            vcExePath = ConfigurationManager.AppSettings["EXEPath"].ToString();
            vcwDirectory = ConfigurationManager.AppSettings["Directory"].ToString();
            tocUSImagePath = ConfigurationManager.AppSettings["TOCUSImagePath"].ToString();
            tocCanadaImagePath = ConfigurationManager.AppSettings["TOCCanadaImagePath"].ToString();
        }
        catch (Exception ex)
        {
            if (string.IsNullOrEmpty(vcExePath) || string.IsNullOrEmpty(vcwDirectory))
            {
                new ApplicationStatusLogBS().WriteLog(intAdjNo, "Configuration Error", "ERR", "Path not Found", ex.Message, CurrentAISUser.PersonID);
                return null;
            }
        }
        try
        {
            string reportPath = Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt").ToString();
            string zLogo = Server.MapPath("images\\" + "zurich_logo_1650_121.jpg").ToString();
            //string reportPath = ConfigurationSettings.AppSettings["ReportPath"].ToString();
            string sFormat = DateTime.Now.ToString("MM-dd-yy-HH:mm:ss:fffff");
            sFormat = sFormat.Replace(":", "");
            sFormat = sFormat.Replace("-", "");
            string CurrentFile = vcwDirectory + "\\" + cFlag + "-" + sFormat + "_" + intAdjNo.ToString() + ".pdf";
            if (cFlag == 'I')
            {
                foreach (string sParam in _VCarrMasterParamsInt)
                {
                    if (!string.IsNullOrEmpty(sParam))
                    {
                        _VCsbMasterParamsInt.Append(sParam);
                        _VCsbMasterParamsInt.Append(" ");
                    }
                }
                foreach (string sParam in _VCarrSubParamsInt)
                {
                    //string strParam = Convert.ToString(sParam);
                    if (!string.IsNullOrEmpty(sParam))
                    {
                        _VCsbSubParamsInt.Append(sParam);
                        _VCsbSubParamsInt.Append("-----");
                    }
                }
                setSubParamsINT = _VCsbSubParamsInt.ToString().Substring(0, _VCsbSubParamsInt.ToString().LastIndexOf("-----"));
            }
            else
            {
                foreach (string sParam in _VCarrMasterParamsExt)
                {
                    if (!string.IsNullOrEmpty(sParam))
                    {
                        _VCsbMasterParamsExt.Append(sParam);
                        _VCsbMasterParamsExt.Append(" ");
                    }
                }
                foreach (string sParam in _VCarrSubParamsExt)
                {
                    //string strParam = Convert.ToString(sParam);
                    if (!string.IsNullOrEmpty(sParam))
                    {
                        _VCsbSubParamsExt.Append(sParam);
                        _VCsbSubParamsExt.Append("-----");
                    }
                }
                setSubParamsEXT = _VCsbSubParamsExt.ToString().Substring(0, _VCsbSubParamsExt.ToString().LastIndexOf("-----"));
            }
            StringBuilder stApp = new StringBuilder();
            stApp.Append("\"" + vcExePath + "\"");
            stApp.Append(" ");
            stApp.Append("-e");
            stApp.Append(" ");
            stApp.Append("\"" + reportPath + "\"");
            stApp.Append(" ");
            if (cFlag == 'I')
            {
                stApp.Append(_VCsbMasterParamsInt.ToString());
                setSubParamsINT = "Parm8:" + setSubParamsINT;
                stApp.Append("\"" + setSubParamsINT + "\"");
            }
            if (cFlag == 'E')
            {
                stApp.Append(_VCsbMasterParamsExt.ToString());
                setSubParamsEXT = "Parm8:" + setSubParamsEXT;
                stApp.Append("\"" + setSubParamsEXT + "\"");
            }

            // TOC
            stApp.Append(" ");
            tocUSImagePath = "Parm30:" + tocUSImagePath;
            stApp.Append("\"" + tocUSImagePath + "\"");
            stApp.Append(" ");
            tocCanadaImagePath = "Parm31:" + tocCanadaImagePath;
            stApp.Append("\"" + tocCanadaImagePath + "\"");

            //stApp.Append(" ");

            stApp.Append(" ");
            stApp.Append("\"Export_Format:Adobe Acrobat (pdf)\"");
            stApp.Append(" ");
            stApp.Append("\"Export_File:" + CurrentFile + "\"");
            stApp.Append(" ");
            if (cFlag == 'I')
                stApp.Append("\"PDF_TOC:2>40>11>6>5>2>" + CurrentFile + "\"");
            if (cFlag == 'E')
                stApp.Append("\"PDF_TOC:2>40>11>6>5>2>" + CurrentFile + "\"");
            stApp.Append(" ");
            stApp.Append("\"PDF_Bookmark_Tags:" + CurrentFile + "\"");
            stApp.Append(" ");
            //stApp.Append("\"PDF_PAGE_N:1>10>10>10>Bottom>Right>Page_NofM>" + CurrentFile + ">Arial\"");
            stApp.Append("\"PDF_PAGE_N:1>10>10>10>Bottom>Right>Page_NofM>" + CurrentFile + ">TT||Arial||Bold||0;0;0\"");
            //stApp.Append("\"PDF_AddImage:" + CurrentFile + ">0>1>40>60>50>10>http://www.CSC.com>" + zLogo + ">0\"");

            //stApp.Append(" ");

            AISBasePage objAISPage = new AISBasePage();
            ConnectionInfo conn = objAISPage.GetReportConnection();
            stApp.Append(" \"Connect_To_SQLOLEDB:" + conn.ServerName + ">>" + conn.DatabaseName + ">>" +
                     conn.IntegratedSecurity.ToString().ToUpper() + "\"");
            stApp.Append(" " + "\"user_id:" + conn.UserID + "\"" + " " + "\"password:" + conn.Password + "\"");
            if (File.Exists(vcExePath))
            {
                startInfo = new ProcessStartInfo(vcExePath);
                startInfo.WorkingDirectory = vcwDirectory;
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = false;
                //startInfo.RedirectStandardOutput = true;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                batchExecute = new Process();
                batchExecute.StartInfo = startInfo;
                batchExecute.StartInfo.Arguments = stApp.ToString();
                try
                {

                    // Start the process with the info we specified.
                    // Call WaitForExit and then the using statement will close.
                    //this.hWND = (int)WindowShowStyle.Hide;
                    // In your code some where: show a form, without making it active
                    //ShowWindow(hWND, WindowShowStyle.Hide);
                    batchExecute.Start();
                    batchExecute.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    batchExecute.WaitForInputIdle();
                    //IntPtr hWnd = batchExecute.MainWindowHandle;
                    //ShowWindow(hWnd, SW_HIDE);
                    batchExecute.WaitForExit();
                    // string sStream = batchExecute.StandardOutput.ReadToEnd();
                    //batchExecute.WaitForExit();
                    //batchExecute.Dispose();
                    //batchExecute.Close();
                    //batchExecute.CloseMainWindow();
                    //batchExecute.Kill();
                    //Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    new ApplicationStatusLogBS().WriteLog(intAdjNo, "ShellExecution", "ERR", "Processor Execution Error", ex.Message, CurrentAISUser.PersonID);
                    return null;
                    // Log error.
                }
                strArray[0] = cFlag.ToString();
                strArray[1] = CurrentFile;
            }
        }
        catch (Exception ex)
        {
            new ApplicationStatusLogBS().WriteLog(intAdjNo, "ShellExecution", "ERR", "VisualCut Error", ex.Message, CurrentAISUser.PersonID);
            strArray = null;
            throw;
        }
        return strArray;

        #endregion
    }


    public string[] DownloadFileProgramSummaySpreadsheetGear(string filename, int AdjNo,string strInvNo, char cFlag, int intPersID,string DraftorFinal)
    {
        string[] strArray = new string[2];
        try
        {
            // Create a new workbook and worksheet.
            SSG.IWorkbook workbook = SSG.Factory.GetWorkbook();
            int sheetIndex = 0;
            int cellIndex = 0;
            
            // int debugindex = 0; //for break point

            #region Sheet:Summary Detail

            DataTable dtAdj = (new ProgramPeriodsBS()).GetAdjReport(AdjNo, 1, false, 0);

            if (dtAdj != null && dtAdj.Rows.Count > 0)
            {
                workbook.Worksheets.Add();
                sheetIndex++;
                SSG.IWorksheet worksheetAdj = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                worksheetAdj.Name = "Summary Detail";

                // Get the worksheet cells reference. 
                SSG.IRange cellsAdj = worksheetAdj.Cells;
                DataRow dr = dtAdj.Rows[0];
                cellIndex = 1;

                //worksheetAdj.Range["E2:E3"].Merge();

                //string path = Server.MapPath("~") + "\\images\\" + "z_logo_135_8.png";
                //worksheetAdj.Shapes.AddPicture(path, 250, 13, 40, 30);


                var item = (from items in dtAdj.AsEnumerable()
                            where (items.Field<Int32>("PREM ADJ ID") == AdjNo)
                                  && (!string.IsNullOrEmpty(items.Field<string>("VALUATION DATE")))
                            select
                            new { VALUATIONDATE = items.Field<string>("VALUATION DATE"), TOTALAMOUNTBILLED = items.Field<Decimal>("TOTAL AMOUNT BILLED") }
                            ).First();

                cellsAdj["A" + cellIndex.ToString()].Value = "Insured:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Broker:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(dr["BROKER NAME"]), SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Invoice Ref Number:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", strInvNo, SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Loss Val Date:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(item.VALUATIONDATE), SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Adjustment Date:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", DateTime.Today.ToShortDateString(), SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Total Amount Billed:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(item.TOTALAMOUNTBILLED), SSG.HAlign.Left, cellIndex);
                SetCurrencyFormatProgramSummaryTotalAmountBilled(cellsAdj["B" + cellIndex.ToString() + ":C" + cellIndex.ToString()]);
                cellIndex = cellIndex + 2;

                DataTable dtPeriodDet = (new ProgramPeriodsBS()).GetProgramSummaryPeriodDetails(AdjNo);

                if (dtPeriodDet.Rows.Count > 0)
                {
                    cellsAdj["A" + cellIndex.ToString()].Value = "Adjustment Number";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);

                    ////worksheetAdj.Range["E2:E3"].Merge();

                    ////string path = Server.MapPath("~") + "\\images\\" + "z_logo_135_8.png";
                    ////if (dtPeriodDet.Rows.Count <= 3)
                    ////{
                    ////    worksheetAdj.Shapes.AddPicture(path, 215, 13, 40, 30);
                    ////}
                    ////else
                    ////{
                    ////    worksheetAdj.Shapes.AddPicture(path, 250, 13, 40, 30);
                    ////}

                    string path = Server.MapPath("~") + "images\\" + "zurich_logo.JPG";
                    if (dtPeriodDet.Rows.Count <= 3)
                    {
                        worksheetAdj.Shapes.AddPicture(path, 215, 13, 100, 60);
                    }
                    else
                    {
                        worksheetAdj.Shapes.AddPicture(path, 250, 13, 100, 60);
                    }


                    for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                    {
                        String columname = string.Empty;
                        int columnindexvalue = i + 1;
                        columname = getColumnNamefromIndex(columnindexvalue);

                        int AdjustNum = 0;
                        AdjustNum = dtPeriodDet.Rows[i - 1].Field<int>("adj_nbr");
                        cellsAdj[columname + cellIndex.ToString()].Value = AdjustNum.ToString();
                        FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                    }
                    cellIndex = cellIndex + 1;
                    cellsAdj["A" + cellIndex.ToString()].Value = "Program period";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);

                    for (int i = 0; i < dtPeriodDet.Rows.Count; i++)
                    {
                        int columnindexvalue;
                        String columname = string.Empty;

                        if (i == 0)
                        {
                            columnindexvalue = i + 2;
                        }
                        else
                        {
                            columnindexvalue = i + 2;
                        }
                        columname = getColumnNamefromIndex(columnindexvalue);

                        string Programperiod = string.Empty;
                        Programperiod = dtPeriodDet.Rows[i].Field<string>("Program Period");

                        cellsAdj[columname + cellIndex.ToString()].Value = Programperiod.ToString();
                        FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                    }
                    cellIndex = cellIndex + 2;

                }

                #region Sheet:RPA

                DataTable dtProgramSummaryRetroReport = (new ProgramPeriodsBS()).GetProgramSummaryRetroDetails(AdjNo);
                if (dtProgramSummaryRetroReport != null)
                {
                    if (dtProgramSummaryRetroReport.Rows.Count > 0)
                    {
                        cellsAdj["A" + cellIndex.ToString()].Value = "Retrospective Premium Adjustment";
                        FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);

                        for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                        {
                            String columname = string.Empty;
                            int columnindexvalue = i + 1;
                            columname = getColumnNamefromIndex(columnindexvalue);

                            cellsAdj[columname + cellIndex.ToString()].Value = "";
                            FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                        }
                        cellIndex = cellIndex + 1;
                        cellsAdj["A" + cellIndex.ToString()].Value = "Program Loss Type";
                        FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                        for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                        {
                            String columname = string.Empty;
                            int columnindexvalue = i + 1;
                            columname = getColumnNamefromIndex(columnindexvalue);
                            string AdjsutmentType = string.Empty;
                            AdjsutmentType = dtPeriodDet.Rows[i - 1].Field<string>("adjustment_Type");
                            cellsAdj[columname + cellIndex.ToString()].Value = AdjsutmentType.ToString();
                            FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                        }
                        cellIndex = cellIndex + 1;


                        for (int i = 1; i <= dtProgramSummaryRetroReport.Rows.Count; i++)
                        {
                            String columname = string.Empty;
                            int columnindexvalue = i;
                            columname = getColumnNamefromIndex(columnindexvalue);

                            String RetroDescription = String.Empty;

                            RetroDescription = dtProgramSummaryRetroReport.Rows[i - 1].Field<string>("DESCR");
                            cellsAdj["A" + cellIndex.ToString()].Value = RetroDescription.ToString();

                            if (RetroDescription == "Adjustment Result:  AP  / (RP) Due Zurich")
                            {
                                SetFontBold(cellsAdj["A" + cellIndex.ToString()]);
                            }

                            //FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);


                            for (int J = 1; J <= dtPeriodDet.Rows.Count; J++)
                            {
                                String columnamePGMID = string.Empty;
                                int columnindexvaluePGMID = J + 1;
                                columnamePGMID = getColumnNamefromIndex(columnindexvaluePGMID);
                                int RetroPGMID = 0;
                                RetroPGMID = dtPeriodDet.Rows[J - 1].Field<int>("prem_adj_pgm_id");


                                int Fieldpostion;
                                if (J == 1)
                                    Fieldpostion = 3;
                                else
                                    Fieldpostion = J + 2;

                                decimal RetroFinalValue;
                                RetroFinalValue = dtProgramSummaryRetroReport.Rows[i - 1].Field<decimal>(Fieldpostion);
                                cellsAdj[columnamePGMID + cellIndex.ToString()].Value = RetroFinalValue.ToString();
                                SetCurrencyFormat(cellsAdj[columnamePGMID + cellIndex.ToString() + ":" + columnamePGMID + cellIndex.ToString()]);
                                if (RetroDescription == "Adjustment Result:  AP  / (RP) Due Zurich")
                                {
                                    SetFontBold(cellsAdj[columnamePGMID + cellIndex.ToString()]);
                                }
                            }

                            cellIndex = cellIndex + 1;

                        }

                    }
                    cellIndex = cellIndex + 2;
                }

                #endregion

                #region Sheet:ILRF

                DataTable dtLRFA = (new ProgramPeriodsBS()).GetProgramSummaryILRFDetails(AdjNo, 1, false, 0);


                if (dtLRFA != null)
                {
                    if (dtLRFA.Rows.Count > 0)
                    {

                        cellsAdj["A" + cellIndex.ToString()].Value = "Loss Reimbursement Fund";
                        FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);


                        for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                        {
                            String columname = string.Empty;
                            int columnindexvalue = i + 1;
                            columname = getColumnNamefromIndex(columnindexvalue);

                            cellsAdj[columname + cellIndex.ToString()].Value = "";
                            FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                        }
                        cellIndex = cellIndex + 1;


                        if (dtProgramSummaryRetroReport == null)
                        {
                            cellsAdj["A" + cellIndex.ToString()].Value = "Program Loss Type";
                            FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                            for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                            {
                                String columname = string.Empty;
                                int columnindexvalue = i + 1;
                                columname = getColumnNamefromIndex(columnindexvalue);
                                string AdjsutmentType = string.Empty;
                                AdjsutmentType = dtPeriodDet.Rows[i - 1].Field<string>("adjustment_Type");
                                cellsAdj[columname + cellIndex.ToString()].Value = AdjsutmentType.ToString();
                                FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                            }
                            cellIndex = cellIndex + 1;
                        }


                        for (int i = 1; i <= dtLRFA.Rows.Count; i++)
                        {
                            String columname = string.Empty;
                            int columnindexvalue = i;
                            columname = getColumnNamefromIndex(columnindexvalue);

                            String ILRFDescription = String.Empty;

                            ILRFDescription = dtLRFA.Rows[i - 1].Field<string>("DESCR");
                            cellsAdj["A" + cellIndex.ToString()].Value = ILRFDescription.ToString();
                            //FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                            if (ILRFDescription == "AP  / (RP)   Due Zurich")
                            {
                                SetFontBold(cellsAdj["A" + cellIndex.ToString()]);
                            }


                            for (int J = 1; J <= dtPeriodDet.Rows.Count; J++)
                            {
                                String columnamePGMID = string.Empty;
                                int columnindexvaluePGMID = J + 1;
                                columnamePGMID = getColumnNamefromIndex(columnindexvaluePGMID);
                                int ILRFPGMID = 0;
                                ILRFPGMID = dtPeriodDet.Rows[J - 1].Field<int>("prem_adj_pgm_id");

                                int Fieldpostion;
                                if (J == 1)
                                    Fieldpostion = 4;
                                else
                                    Fieldpostion = J + 3;

                                decimal ILRFFinalValue;
                                ILRFFinalValue = dtLRFA.Rows[i - 1].Field<decimal>(Fieldpostion);
                                cellsAdj[columnamePGMID + cellIndex.ToString()].Value = ILRFFinalValue.ToString();
                                SetCurrencyFormat(cellsAdj[columnamePGMID + cellIndex.ToString() + ":" + columnamePGMID + cellIndex.ToString()]);
                                if (ILRFDescription == "AP  / (RP)   Due Zurich")
                                {
                                    SetFontBold(cellsAdj[columnamePGMID + cellIndex.ToString()]);
                                }

                            }

                            cellIndex = cellIndex + 1;

                        }

                    }
                    cellIndex = cellIndex + 2;
                }


                #endregion

                #region Sheet:OtherAdjustments

                DataTable dtProgramSummaryOtherAdjsutmentDetails = (new ProgramPeriodsBS()).GetProgramSummaryOtherAdjsutmentDetails(AdjNo, 1, false, 0);

                bool checkifAdditionalInvoice = false;
                if (dtProgramSummaryOtherAdjsutmentDetails != null)
                {
                    String AdditionalInvoice = String.Empty;
                    for (int i = 1; i <= dtProgramSummaryOtherAdjsutmentDetails.Rows.Count; i++)
                    {
                        AdditionalInvoice = dtProgramSummaryOtherAdjsutmentDetails.Rows[i - 1].Field<string>("DESCR");

                        if (AdditionalInvoice != "Retro" && AdditionalInvoice != "WC Deductible" && AdditionalInvoice != "Loss Reimbursement Fund" && AdditionalInvoice != "DEP" && AdditionalInvoice != "DEP 2")
                        {
                            checkifAdditionalInvoice = true;
                            break;
                        }
                       
                    }
                }

                if (checkifAdditionalInvoice == true)
                {
                    if (dtProgramSummaryOtherAdjsutmentDetails.Rows.Count > 0)
                    {
                        cellsAdj["A" + cellIndex.ToString()].Value = "Additional Invoiced Amounts";
                        FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);


                        for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                        {
                            String columname = string.Empty;
                            int columnindexvalue = i + 1;
                            columname = getColumnNamefromIndex(columnindexvalue);

                            cellsAdj[columname + cellIndex.ToString()].Value = "";
                            FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                        }
                        cellIndex = cellIndex + 1;


                        if (dtProgramSummaryRetroReport == null && dtLRFA == null)
                        {
                            cellsAdj["A" + cellIndex.ToString()].Value = "Program Loss Type";
                            FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                            for (int i = 1; i <= dtPeriodDet.Rows.Count; i++)
                            {
                                String columname = string.Empty;
                                int columnindexvalue = i + 1;
                                columname = getColumnNamefromIndex(columnindexvalue);
                                string AdjsutmentType = string.Empty;
                                AdjsutmentType = dtPeriodDet.Rows[i - 1].Field<string>("adjustment_Type");
                                cellsAdj[columname + cellIndex.ToString()].Value = AdjsutmentType.ToString();
                                FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString()]);

                            }
                            cellIndex = cellIndex + 1;
                        }
                        for (int i = 1; i <= dtProgramSummaryOtherAdjsutmentDetails.Rows.Count; i++)
                        {
                            String columname = string.Empty;
                            int columnindexvalue = i;
                            columname = getColumnNamefromIndex(columnindexvalue);

                            String OtherAdjustmentDescription = String.Empty;

                            OtherAdjustmentDescription = dtProgramSummaryOtherAdjsutmentDetails.Rows[i - 1].Field<string>("DESCR");

                            if (OtherAdjustmentDescription != "Retro" && OtherAdjustmentDescription != "WC Deductible" && OtherAdjustmentDescription != "Loss Reimbursement Fund" && OtherAdjustmentDescription != "DEP" && OtherAdjustmentDescription != "DEP 2")
                            {
                                cellsAdj["A" + cellIndex.ToString()].Value = OtherAdjustmentDescription.ToString();
                                //FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                                SetFontBold(cellsAdj["A" + cellIndex.ToString()]);

                                for (int J = 1; J <= dtPeriodDet.Rows.Count; J++)
                                {
                                    String columnamePGMID = string.Empty;
                                    int columnindexvaluePGMID = J + 1;
                                    columnamePGMID = getColumnNamefromIndex(columnindexvaluePGMID);
                                    int AddInvAmtPGMID = 0;
                                    AddInvAmtPGMID = dtPeriodDet.Rows[J - 1].Field<int>("prem_adj_pgm_id");

                                    int Fieldpostion;
                                    if (J == 1)
                                        Fieldpostion = 3;
                                    else
                                        Fieldpostion = J + 2;

                                    decimal AdditionInvoiceFinalValue;
                                    AdditionInvoiceFinalValue = dtProgramSummaryOtherAdjsutmentDetails.Rows[i - 1].Field<decimal>(Fieldpostion);
                                    cellsAdj[columnamePGMID + cellIndex.ToString()].Value = AdditionInvoiceFinalValue.ToString();
                                    SetCurrencyFormat(cellsAdj[columnamePGMID + cellIndex.ToString() + ":" + columnamePGMID + cellIndex.ToString()]);
                                    SetFontBold(cellsAdj[columnamePGMID + cellIndex.ToString()]);

                                }

                                cellIndex = cellIndex + 1;
                            }


                        }
                        cellIndex = cellIndex + 2;
                    }
                }

                
                #endregion


                #region Sheet:Period Result

                if (dtProgramSummaryOtherAdjsutmentDetails.Rows.Count > 0)
                {
                    cellsAdj["A" + cellIndex.ToString()].Value = "Overall Result by Period";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);



                    for (int J = 1; J <= dtPeriodDet.Rows.Count; J++)
                    {
                        String columnamePGMID = string.Empty;
                        int columnindexvaluePGMID = J + 1;
                        columnamePGMID = getColumnNamefromIndex(columnindexvaluePGMID);
                        //int RetroPGMID = 0;
                        //RetroPGMID = dtPeriodDet.Rows[J - 1].Field<int>("prem_adj_pgm_id");

                        int Fieldpostion;
                        if (J == 1)
                            Fieldpostion = 3;
                        else
                            Fieldpostion = J + 2;

                        decimal PeriodTotal = 0;


                        for (int i = 1; i <= dtProgramSummaryOtherAdjsutmentDetails.Rows.Count; i++)
                        {
                            if (i == 1)
                            {
                                PeriodTotal = dtProgramSummaryOtherAdjsutmentDetails.Rows[i - 1].Field<decimal>(Fieldpostion);
                            }
                            else
                            {
                                PeriodTotal = PeriodTotal + dtProgramSummaryOtherAdjsutmentDetails.Rows[i - 1].Field<decimal>(Fieldpostion);
                            }


                        }
                        cellsAdj[columnamePGMID + cellIndex.ToString()].Value = PeriodTotal.ToString();
                        SetCurrencyFormat(cellsAdj[columnamePGMID + cellIndex.ToString() + ":" + columnamePGMID + cellIndex.ToString()]);
                        FormatHeaderTypeCellRetro(cellsAdj[columnamePGMID + cellIndex.ToString()]);

                    }
                }

                #endregion


                worksheetAdj.UsedRange.Columns.AutoFit();
                for (int col = 0; col < worksheetAdj.UsedRange.ColumnCount; col++)
                {
                    worksheetAdj.Cells[1, col].ColumnWidth *= 1.15;
                }
            }

            #endregion

            #region Sheet:Policy Number


            if (dtAdj != null && dtAdj.Rows.Count > 0)
            {
                workbook.Worksheets.Add();
                sheetIndex++;
                SSG.IWorksheet worksheetPolicyNumber = workbook.Worksheets["Sheet" + sheetIndex.ToString()];
                worksheetPolicyNumber.Name = "Policy Number";

                // Get the worksheet cells reference. 
                SSG.IRange cellsAdj = worksheetPolicyNumber.Cells;
                DataRow dr = dtAdj.Rows[0];
                cellIndex = 1;


                //worksheetPolicyNumber.Range["E2:E3"].Merge();

                //string path = Server.MapPath("~") + "\\images\\" + "z_logo_135_8.png";
                //worksheetPolicyNumber.Shapes.AddPicture(path, 230, 13, 40, 30);


                string path = Server.MapPath("~") + "images\\" + "zurich_logo.JPG";
                worksheetPolicyNumber.Shapes.AddPicture(path, 230, 13, 100, 60);


                var item = (from items in dtAdj.AsEnumerable()
                            where (items.Field<Int32>("PREM ADJ ID") == AdjNo)
                                  && (!string.IsNullOrEmpty(items.Field<string>("VALUATION DATE")))
                            select
                            new { VALUATIONDATE = items.Field<string>("VALUATION DATE"), TOTALAMOUNTBILLED = items.Field<Decimal>("TOTAL AMOUNT BILLED") }
                            ).First();

                cellsAdj["A" + cellIndex.ToString()].Value = "Insured:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(dr["INSURED NAME"]), SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Broker:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(dr["BROKER NAME"]), SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Invoice Ref Number:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", strInvNo, SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Loss Val Date:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(item.VALUATIONDATE), SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Adjustment Date:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", DateTime.Today.ToShortDateString(), SSG.HAlign.Left, cellIndex);
                cellIndex = cellIndex + 1;
                cellsAdj["A" + cellIndex.ToString()].Value = "Total Amount Billed:";
                FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);
                MergeAndFormatCellsWithoutColor(cellsAdj, "B", ":C", Convert.ToString(item.TOTALAMOUNTBILLED), SSG.HAlign.Left, cellIndex);
                SetCurrencyFormatProgramSummaryTotalAmountBilled(cellsAdj["B" + cellIndex.ToString() + ":C" + cellIndex.ToString()]);
                cellIndex = cellIndex + 2;

                DataTable dtPeriodDet = (new ProgramPeriodsBS()).GetProgramSummaryPeriodDetails(AdjNo);

                if (dtPeriodDet.Rows.Count > 0)
                {

                    cellsAdj["A" + cellIndex.ToString()].Value = "Program period / Adjustment Type:";
                    FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);

                    for (int i = 0; i < dtPeriodDet.Rows.Count; i++)
                    {
                        int columnindexvalue = 0;
                        int columnindexvalue1 = 0;


                        String columname = string.Empty;
                        String columname1 = string.Empty;
                        if (i == 0)
                        {
                            columnindexvalue = i + 2;
                            columnindexvalue1 = columnindexvalue + 1;
                            ViewState["ColumnIndex1"] = columnindexvalue1.ToString();

                        }
                        else
                        {
                            if (ViewState["ColumnIndex1"] != null)
                            {
                                columnindexvalue = Convert.ToInt32(ViewState["ColumnIndex1"]) + 1;
                                columnindexvalue1 = columnindexvalue + 1;
                                ViewState["ColumnIndex1"] = columnindexvalue1.ToString();
                            }
                        }
                        columname = getColumnNamefromIndex(columnindexvalue);
                        columname1 = getColumnNamefromIndex(columnindexvalue1);
                        columname1 = ":" + columname1.ToString();

                        string Programperiod = string.Empty;
                        Programperiod = dtPeriodDet.Rows[i].Field<string>("Program Period");

                        //cellsAdj[columname + cellIndex.ToString()].Value = Programperiod.ToString();

                        MergeAndFormatCellsWithColor(cellsAdj, columname, columname1, Convert.ToString(Programperiod), SSG.HAlign.Left, cellIndex);
                        FormatHeaderTypeCellProgramSumary(cellsAdj[columname + cellIndex.ToString() + columname1 + cellIndex.ToString()]);

                        //FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString() + ":E" + cellIndex.ToString()]);

                    }
                    cellIndex = cellIndex + 1;

                }

                #region Sheet:Policy Numbers

                DataTable GetProgramSummaryPolicyInfo = (new ProgramPeriodsBS()).GetProgramSummaryPolicyInfo(AdjNo);
                if (GetProgramSummaryPolicyInfo != null)
                {
                    if (GetProgramSummaryPolicyInfo.Rows.Count > 0)
                    {
                        cellsAdj["A" + cellIndex.ToString()].Value = "Policy Numbers:";
                        FormatHeaderTypeCell(cellsAdj["A" + cellIndex.ToString()]);

                        for (int i = 0; i < dtPeriodDet.Rows.Count; i++)
                        {

                            int columnindexvalue = 0;
                            int columnindexvalue1 = 0;


                            String columnamePolicy = string.Empty;
                            String columname1Policy = string.Empty;

                            if (i == 0)
                            {
                                columnindexvalue = i + 2;
                                columnindexvalue1 = columnindexvalue + 1;
                                ViewState["ColumnIndex1Policy"] = columnindexvalue1.ToString();

                            }
                            else
                            {
                                if (ViewState["ColumnIndex1Policy"] != null)
                                {
                                    columnindexvalue = Convert.ToInt32(ViewState["ColumnIndex1Policy"]) + 1;
                                    columnindexvalue1 = columnindexvalue + 1;
                                    ViewState["ColumnIndex1Policy"] = columnindexvalue1.ToString();
                                }
                            }
                            columnamePolicy = getColumnNamefromIndex(columnindexvalue);
                            columname1Policy = getColumnNamefromIndex(columnindexvalue1);


                            DataTable GetPolicyInfoByPGMID = (from dts in GetProgramSummaryPolicyInfo.AsEnumerable()
                                                              where (dts.Field<int>("PREM_ADJ_PGM_ID") == Convert.ToInt32(dtPeriodDet.Rows[i]["PREM_ADJ_PGM_ID"]))
                                                              select dts).CopyToDataTable();
                            cellIndex = 9;
                            for (int k = 0; k < GetPolicyInfoByPGMID.Rows.Count; k++)
                            {
                                if (k == 0)
                                {
                                    String PolicyNumbervalue = string.Empty;
                                    String Policy_Type = string.Empty;
                                    PolicyNumbervalue = GetPolicyInfoByPGMID.Rows[k].Field<string>("Policy_Number");
                                    cellsAdj[columnamePolicy + cellIndex.ToString()].Value = PolicyNumbervalue.ToString();

                                    Policy_Type = GetPolicyInfoByPGMID.Rows[k].Field<string>("Policy_Type");
                                    cellsAdj[columname1Policy + cellIndex.ToString()].Value = Policy_Type.ToString();
                                }
                                else
                                {

                                    String PolicyNumbervalue = string.Empty;
                                    String Policy_Type = string.Empty;
                                    PolicyNumbervalue = GetPolicyInfoByPGMID.Rows[k].Field<string>("Policy_Number");
                                    cellsAdj[columnamePolicy + cellIndex.ToString()].Value = PolicyNumbervalue.ToString();

                                    Policy_Type = GetPolicyInfoByPGMID.Rows[k].Field<string>("Policy_Type");
                                    cellsAdj[columname1Policy + cellIndex.ToString()].Value = Policy_Type.ToString();
                                }
                                cellIndex = cellIndex + 1;
                            }

                        }
                    }
                }
                #endregion

                worksheetPolicyNumber.UsedRange.Columns.AutoFit();
                for (int col = 0; col < worksheetPolicyNumber.UsedRange.ColumnCount; col++)
                {
                    worksheetPolicyNumber.Cells[1, col].ColumnWidth *= 1.15;
                }
            }

            #endregion

            if (sheetIndex == 0)
            {
                workbook.Worksheets.Add();
            }

            string vcwDirectory = ConfigurationManager.AppSettings["Directory"].ToString();
            string sFormat = DateTime.Now.ToString("MM-dd-yy-HH:mm:ss:fffff");
            sFormat = sFormat.Replace(":", "");
            sFormat = sFormat.Replace("-", "");
            cFlag = 'P';
            //if (extIntFlag == 0)
            //{
            //    cFlag = 'E';
            //}

            workbook.Worksheets[0].Select();

            string strFileName = vcwDirectory + "\\" + cFlag + "_" + sFormat + "_" + DraftorFinal + "_" + AdjNo + ".xlsx";
            //EAISA-5 Veracode flaw fix 12072017
            string strFileNameDecd = Server.HtmlDecode(Server.HtmlEncode(strFileName));
            workbook.SaveAs(strFileNameDecd, SSG.FileFormat.OpenXMLWorkbook);
            workbook.Close();

            strArray[0] = cFlag.ToString();
            strArray[1] = strFileName;
            
        }
        catch (Exception err)
        {
            common.Logger.Error(err);
        }
       
        return strArray;
    }
    // Phase -3
    public static String getColumnNamefromIndex(int column)
    {
        column--;
        String col = Convert.ToString((char)('A' + (column % 26)));
        while (column >= 26)
        {
            column = (column / 26) - 1;
            col = Convert.ToString((char)('A' + (column % 26))) + col;
        }
        return col;
    }

    private Dictionary<int, string> GetPrgPrdsWithType(List<DataTable> dtResult, string colName)
    {
        List<int> prgPrdIDList = new List<int>();

        for (int i = 0; i < dtResult.Count; i++)
        {
            prgPrdIDList.Add(Convert.ToInt32(dtResult[i].Rows[0][colName]));
        }
        Dictionary<int, string> prgPrdList = new ProgramPeriodsBS().GetProgramPeriodTypes(prgPrdIDList);
        return prgPrdList;
    }

    //This Method checks NullOrEmpty for decimal field.
    private string CheckNullOrEmptyReturnValue(DataRow row, string colName)
    {
        string result = !string.IsNullOrEmpty(Convert.ToString(row[colName])) ? Convert.ToString(row[colName]) : "0";
        return result;
    }

    //This Method sets currency format in excel cell for amount type 
    private void SetCurrencyFormat(SSG.IRange cell)
    {
        cell.HorizontalAlignment = SSG.HAlign.Right;
        //cell.NumberFormat = "$#,###";
       // cell.NumberFormat = "$#,##0";
        cell.NumberFormat = "$#,##0_);($#,##0)";

    }


    // Phase -3
    private void SetFontBold(SSG.IRange cell)
    {
        cell.Font.Bold = true;

    }

    // Phase -3
    //This Method sets currency format in excel cell for amount type 
    private void SetCurrencyFormatProgramSummaryTotalAmountBilled(SSG.IRange cell)
    {
        cell.HorizontalAlignment = SSG.HAlign.Left;
        //cell.NumberFormat = "$#,###";
       // cell.NumberFormat = "$#,##0";
        cell.NumberFormat = "$#,##0_);($#,##0)";
    }

    //This Method sets currency format in excel cell for amount type with 2-decimal
    private void SetCurrencyFormatForDecimal(SSG.IRange cell)
    {
        cell.HorizontalAlignment = SSG.HAlign.Right;
        //cell.NumberFormat = "$#,###";
       // cell.NumberFormat = "$#,##0.00";
        cell.NumberFormat = "$#,##0.00_);($#,##0.00)";
    }

    private decimal GetSumForDecimalField(DataTable dtResult, string colName)
    {
        var sumTotal = dtResult.AsEnumerable()
                            .Where(row => row.Field<decimal?>(colName) != null)
                            .Sum(row => row.Field<decimal>(colName));
        return Convert.ToDecimal(sumTotal);
    }

    private void MergeAndFormatCellsWithoutColor(SSG.IRange cells, string colStart, string colEnd, string text, SSG.HAlign align, int cellIndex, bool isBold = true)
    {
        SSG.IRange cell = cells[colStart + cellIndex.ToString() + colEnd + cellIndex.ToString()];
        cell.Merge();
        cell.Font.Bold = isBold;
        cell.HorizontalAlignment = align;
        cell.Value = text;
    }
    private void MergeAndFormatCellsWithoutColorandBold(SSG.IRange cells, string colStart, string colEnd, string text, SSG.HAlign align, int cellIndex, bool isBold = false)
    {
        SSG.IRange cell = cells[colStart + cellIndex.ToString() + colEnd + cellIndex.ToString()];
        cell.Merge();
        cell.Font.Bold = isBold;
        cell.HorizontalAlignment = align;
        cell.Value = text;
    }

    private void MergeAndFormatCellsWithColor(SSG.IRange cells, string colStart, string colEnd, string text, SSG.HAlign align, int cellIndex, bool isBold = true, SSG.Color color = default(SSG.Color))
    {
        SSG.IRange cell = cells[colStart + cellIndex.ToString() + colEnd + cellIndex.ToString()];
        cell.Merge();
        cell.Font.Bold = isBold;
        cell.HorizontalAlignment = align;
        cell.Value = text;
        //FormatHeaderTypeCell(cell,align,isBold);
        SetColorToCell(cell, color);

    }

    private void FormatHeaderTypeCell(SSG.IRange cell, SSG.HAlign align = SSG.HAlign.Left, bool isBold = true, SSG.Color color = default(SSG.Color))
    {
        cell.Font.Bold = isBold;
        cell.HorizontalAlignment = align;
        cell.Borders.Color = SSG.Colors.SteelBlue;
        SetColorToCell(cell, color);
        //if (color == default(SSG.Color))
        //{
        //    cell.Interior.Color = SSG.Color.FromArgb(192, 192, 192);
        //}
        //else
        //{
        //    cell.Interior.Color = color;
        //}
        //cell.Interior.Color = SSG.Colors.SteelBlue;
        //SSG.Color.FromArgb(192,192,192)
    }

    private void FormatHeaderTypeCellProgramSumary(SSG.IRange cell, SSG.HAlign align = SSG.HAlign.Center, bool isBold = true, SSG.Color color = default(SSG.Color))
    {
        cell.Font.Bold = isBold;
        cell.HorizontalAlignment = align;
        cell.Borders.Color = SSG.Colors.SteelBlue;
        SetColorToCell(cell, color);
        //if (color == default(SSG.Color))
        //{
        //    cell.Interior.Color = SSG.Color.FromArgb(192, 192, 192);
        //}
        //else
        //{
        //    cell.Interior.Color = color;
        //}
        //cell.Interior.Color = SSG.Colors.SteelBlue;
        //SSG.Color.FromArgb(192,192,192)
    }

    private void FormatHeaderTypeCellRetro(SSG.IRange cell, SSG.HAlign align = SSG.HAlign.Right, bool isBold = true, SSG.Color color = default(SSG.Color))
    {
        cell.Font.Bold = isBold;
        cell.HorizontalAlignment = align;
        cell.Borders.Color = SSG.Colors.SteelBlue;
        SetColorToCell(cell, color);
        //if (color == default(SSG.Color))
        //{
        //    cell.Interior.Color = SSG.Color.FromArgb(192, 192, 192);
        //}
        //else
        //{
        //    cell.Interior.Color = color;
        //}
        //cell.Interior.Color = SSG.Colors.SteelBlue;
        //SSG.Color.FromArgb(192,192,192)
    }
    private void FormatHeaderTypeCellExcessLoss(SSG.IRange cell, SSG.HAlign align = SSG.HAlign.Left, bool isBold = true, SSG.Color color = default(SSG.Color))
    {
        cell.Font.Bold = isBold;
        cell.HorizontalAlignment = align;
        cell.Borders.Color = SSG.Colors.SteelBlue;
        SetColorToCell(cell, color);
        //if (color == default(SSG.Color))
        //{
        //    cell.Interior.Color = SSG.Color.FromArgb(192, 192, 192);
        //}
        //else
        //{
        //    cell.Interior.Color = color;
        //}
        //cell.Interior.Color = SSG.Colors.SteelBlue;
        //SSG.Color.FromArgb(192,192,192)
    }

    private void SetColorToCell(SSG.IRange cell, SSG.Color color = default(SSG.Color))
    {
        if (color == default(SSG.Color))
        {
            cell.Interior.Color = SSG.Color.FromArgb(192, 192, 192);
        }
        else
        {
            cell.Interior.Color = color;
        }
    }
    //public string generateRevisedDraftInvoice(int iAdjNo, int iPreviousAdjNo, bool HistFlag)
    //{

    //    try
    //    {
    //        //To check the CALC status
    //        PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
    //        IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;
    //        //Retrieving CALC,UWReviewed Statuses from lookup table
    //        int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
    //        objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(iAdjNo);
    //        //objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[objPremAdjStsBE.Count - 1].PremumAdj_sts_ID);
    //        objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
    //        //if it is Draft Invoice we are verifying the status.if status is not equal to CALC then show error

    //        if (objPremStsBE.ADJ_STS_TYP_ID != intAdjCalStatusID)
    //        {
    //            string strMessage = "Please do the calculation for at least one program period";
    //            return strMessage;
    //        }
    //        PremiumAdjustmentBE prevPremAdjBE = new PremAdjustmentBS().getPremiumAdjustmentRow(iPreviousAdjNo);
    //        string strPrevInvNo = prevPremAdjBE.FNL_INVC_NBR_TXT;
    //        bool UpdateInvNum = false;
    //        bool InsertStatus = false;
    //        objDC.Connection.Open();
    //        trans = objDC.Connection.BeginTransaction();
    //        objDC.Transaction = trans;
    //        //Calling Generate InvoiceNumber Function
    //        string strInvNo = generateInvoiceNumbers(iAdjNo, 3, iPreviousAdjNo, 1);

    //        ////Retrieving Draft-Invoice,Final Inv and Draft-Inv-Err Statuses from lookup table
    //        int intDraftInvStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.DraftInvd, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
    //        //Draft Invoice Updation
    //        string strComments = string.Empty;
    //        UpdateInvNum = new InvoiceDriverBS().UpdatePremAdjutmentDraftInvoiceData(objDC, iAdjNo, strInvNo, CurrentAISUser.PersonID);
    //        InsertStatus = new InvoiceDriverBS().InsertPremAdjutmentStatusData(objDC, iAdjNo, intDraftInvStatusID, strComments, CurrentAISUser.PersonID);
    //        if (UpdateInvNum != true || InsertStatus != true)
    //        {
    //            if (trans != null)
    //            trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Prem_adj or Prem_adj_sts table updation Failed", CurrentAISUser.PersonID);
    //            return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
    //        }

    //        //Internal PDF Generation
    //        ReportDocument objMainRevDraftInternal = new ReportDocument();
    //        objMainRevDraftInternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

    //        //External PDF Generation
    //        ReportDocument objMainRevDraftExternal = new ReportDocument();
    //        objMainRevDraftExternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

    //        //Coding Work Sheet PDF Generation
    //        ReportDocument objMainRevDraftCDWSheet = new ReportDocument();
    //        objMainRevDraftCDWSheet.Load(Server.MapPath("\\Reports\\" + "rptRevisionMasterReport" + ".rpt"));

    //        //Internal PDF Connections
    //        GenerateReportConnection(objMainRevDraftInternal);
    //        //External PDF Connections
    //        GenerateReportConnection(objMainRevDraftExternal);
    //        //Coding Work Sheet PDF Connections
    //        GenerateReportConnection(objMainRevDraftCDWSheet);



    //        ParameterDiscreteValue prmAdjNo = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmFlag = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmPreviousAdjNo = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmPreviousInvNo = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmFlipSigns = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmRevFlag = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmInvNo = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmHistFlag = new ParameterDiscreteValue();
    //        prmAdjNo.Value = iAdjNo;
    //        //Draft Invoice
    //        prmFlag.Value = 1;
    //        //prmPreviousAdjNo.Value = iPreviousAdjNo;
    //        prmFlipSigns.Value = false;
    //        prmRevFlag.Value = false;
    //        prmInvNo.Value = strInvNo;
    //        //prmPreviousInvNo.Value = strPrevInvNo;
    //        prmHistFlag.Value = HistFlag;
    //        /*****************Setting Master Report Parameters Value Begin******************/
    //        objMainRevDraftInternal.SetParameterValue("@ADJNO", prmAdjNo);
    //        objMainRevDraftInternal.SetParameterValue("@FLAG", prmFlag);
    //        objMainRevDraftExternal.SetParameterValue("@ADJNO", prmAdjNo);
    //        objMainRevDraftExternal.SetParameterValue("@FLAG", prmFlag);
    //        objMainRevDraftCDWSheet.SetParameterValue("@ADJNO", prmAdjNo);
    //        objMainRevDraftCDWSheet.SetParameterValue("@FLAG", prmFlag);
    //        /*****************Setting Master Report Parameters Value Begin******************/

    //        //Draft Invoice

    //        //Draft Invoice Internal PDF
    //        //IList<InvoiceExhibitBE> objDrftInternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(1);
    //        //for (int iCount = 0; iCount < objDrftInternalIlistBE.Count; iCount++)
    //        //{

    //        //    setMasterReportParameter(objMainRevDraftInternal, objDrftInternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftInternalIlistBE[iCount].STS_IND), 1, Convert.ToChar(objDrftInternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

    //        //}

    //        ////Draft Invoice External PDF
    //        //IList<InvoiceExhibitBE> objDrftExternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(2);
    //        //for (int iCount = 0; iCount < objDrftExternalIlistBE.Count; iCount++)
    //        //{

    //        //    setMasterReportParameter(objMainRevDraftExternal, objDrftExternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftExternalIlistBE[iCount].STS_IND), 2, Convert.ToChar(objDrftExternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

    //        //}
    //        ////Draft Coding Work Sheet PDF 
    //        //IList<InvoiceExhibitBE> objDrftCDWorksheetIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(3);
    //        //for (int iCount = 0; iCount < objDrftCDWorksheetIlistBE.Count; iCount++)
    //        //{

    //        //    setMasterReportParameter(objMainRevDraftCDWSheet, objDrftCDWorksheetIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objDrftCDWorksheetIlistBE[iCount].STS_IND), 3, Convert.ToChar(objDrftCDWorksheetIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

    //        //}

    //        InternalReportParametersDelegate delInternal = new InternalReportParametersDelegate(InternalReportParameters);
    //        ExternalReportParametersDelegate delExternal = new ExternalReportParametersDelegate(ExternalReportParameters);
    //        CesarReportParametersDelegate delCesar = new CesarReportParametersDelegate(CESARReportParameters);
    //        IAsyncResult reffdelInternal = delInternal.BeginInvoke(objMainRevDraftInternal, iAdjNo, null, null);
    //        IAsyncResult reffdelExternal = delExternal.BeginInvoke(objMainRevDraftExternal, iAdjNo, null, null);
    //        IAsyncResult reffdelCesar = delCesar.BeginInvoke(objMainRevDraftCDWSheet, iAdjNo, null, null);
    //        reffdelInternal.AsyncWaitHandle.WaitOne();
    //        reffdelExternal.AsyncWaitHandle.WaitOne();
    //        reffdelCesar.AsyncWaitHandle.WaitOne();
    //        delInternal.EndInvoke(reffdelInternal);
    //        delExternal.EndInvoke(reffdelExternal);
    //        delCesar.EndInvoke(reffdelCesar);


    //        /*****************Setting Sub Reports Parameters Value Begin******************/
    //        setInternalSubReportParameters(objMainRevDraftInternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
    //        //setInternalSubReportParameters(objMainRevDraftExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
    //        setExternalSubReportParameters(objMainRevDraftExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
    //        //setRevisionCDWSubReportParameters(objMainRevDraftCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmPreviousAdjNo, prmPreviousInvNo, prmHistFlag);
    //        setRevisionCDWSubReportParameters(objMainRevDraftCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
    //        /*****************Setting Sub Reports Parameters Value End******************/
    //        string strVal1;
    //        string strVal2;
    //        string strVal3;
    //        //Start:parallel Process-Calling Export Methods Parallely
    //        int intPersID = CurrentAISUser.PersonID;
    //        try
    //        {
    //            string strExaternalFilename = "DraftExternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //            string strInternalFilename = "DraftInternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //            string strCDWorkSheetFilename = "DraftCDWorkSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //            ExportExternalDelegate delInstanceExternal = new ExportExternalDelegate(ExportReports);
    //            ExportInternalDelegate delInstanceInternal = new ExportInternalDelegate(ExportReports);
    //            ExportCesarDelegate delInstanceCesar = new ExportCesarDelegate(ExportReports);
    //            IAsyncResult reffExternal = delInstanceExternal.BeginInvoke(objMainRevDraftExternal, strExaternalFilename, iAdjNo, 1, 'E', intPersID, null, null);
    //            IAsyncResult reffInternal = delInstanceInternal.BeginInvoke(objMainRevDraftInternal, strInternalFilename, iAdjNo, 1, 'I', intPersID, null, null);
    //            IAsyncResult reffCesar = delInstanceCesar.BeginInvoke(objMainRevDraftCDWSheet, strCDWorkSheetFilename, iAdjNo, 1, 'C', intPersID, null, null);
    //            reffExternal.AsyncWaitHandle.WaitOne();
    //            reffInternal.AsyncWaitHandle.WaitOne();
    //            reffCesar.AsyncWaitHandle.WaitOne();
    //            strVal1 = delInstanceInternal.EndInvoke(reffInternal);
    //            strVal2 = delInstanceExternal.EndInvoke(reffExternal);
    //            strVal3 = delInstanceCesar.EndInvoke(reffCesar);

    //        }
    //        catch (Exception ee)
    //        {
    //            if (trans != null)
    //                trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
    //            return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
    //        }
    //        //try
    //        //{


    //        //    string strInternalFilename = "DraftInternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //        //    strVal1 = ExportReports(objMainRevDraftInternal, strInternalFilename, iAdjNo, 1, 'I');

    //        //}
    //        //catch (Exception ee)
    //        //{
    //        //    if (trans != null)
    //        //    trans.Rollback();
    //        //    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
    //        //    return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
    //        //}
    //        //try
    //        //{


    //        //    string strExaternalFilename = "DraftExternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //        //    strVal2 = ExportReports(objMainRevDraftExternal, strExaternalFilename, iAdjNo, 1, 'E');

    //        //}
    //        //catch (Exception ee)
    //        //{
    //        //    if (trans != null)
    //        //    trans.Rollback();
    //        //    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
    //        //    return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
    //        //}
    //        //try
    //        //{

    //        //    string strCDWorkSheetFilename = "DraftCDWorkSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //        //    strVal3 = ExportReports(objMainRevDraftCDWSheet, strCDWorkSheetFilename, iAdjNo, 1, 'C');

    //        //}
    //        //catch (Exception ee)
    //        //{
    //        //    if (trans != null)
    //        //    trans.Rollback();
    //        //    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
    //        //    return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
    //        //}
    //        objMainRevDraftInternal.Close();
    //        objMainRevDraftExternal.Close();
    //        objMainRevDraftCDWSheet.Close();
    //        if (strVal1 != null && strVal1 != "")
    //        {
    //            if (trans != null)
    //            trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal1, CurrentAISUser.PersonID);
    //            return strVal1;
    //        }
    //        else if (strVal2 != null && strVal2 != "")
    //        {
    //            if (trans != null)
    //            trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal2, CurrentAISUser.PersonID);
    //            return strVal2;
    //        }
    //        else if (strVal3 != null && strVal3 != "")
    //        {
    //            if (trans != null)
    //            trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal3, CurrentAISUser.PersonID);
    //            return strVal3;
    //        }
    //        else
    //        {
    //            objDC.SubmitChanges();
    //            if (trans != null)
    //            trans.Commit();
    //            try
    //            {
    //                EMailNotification(iAdjNo, 1);
    //                return "The Adjustment Draft Invoice has been submitted for processing. The Draft invoice number is " + strInvNo + ". Please note that the pdf copy of the Draft invoice will be available after 15 minutes";
    //            }
    //            catch (Exception ex)
    //            {
    //                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
    //                return "The Adjustment Draft Invoice has been submitted for processing. The Draft invoice number is " + strInvNo + ". Please note that the pdf copy of the Draft invoice will be available after 15 minutes,But E-Mail Notification Failed";
    //            }

    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        if (trans != null)
    //        trans.Rollback();
    //        new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
    //        return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
    //    }
    //    finally
    //    {
    //        if (objDC.Connection.State == ConnectionState.Open)
    //            objDC.Connection.Close();
    //    }
    //    return string.Empty;
    //}
    #region Commented for VisulCut
    public string generateRevisedDraftInvoiceWithTOC(int iAdjNo, int iPreviousAdjNo, bool HistFlag)
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
            ViewState["DraftstrInvNo"] = strInvNo;
            ////Retrieving Draft-Invoice,Final Inv and Draft-Inv-Err Statuses from lookup table
            int intDraftInvStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.DraftInvd, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            //Draft Invoice Updation
            string strComments = string.Empty;
            UpdateInvNum = new InvoiceDriverBS().UpdatePremAdjutmentDraftInvoiceData(objDC, iAdjNo, strInvNo, CurrentAISUser.PersonID);
            InsertStatus = new InvoiceDriverBS().InsertPremAdjutmentStatusData(objDC, iAdjNo, intDraftInvStatusID, strComments, CurrentAISUser.PersonID);
            if (UpdateInvNum != true || InsertStatus != true)
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Prem_adj or Prem_adj_sts table updation Failed", CurrentAISUser.PersonID);
                return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
            }
            #region Commented

            ////Internal PDF Generation
            //ReportDocument objMainRevDraftInternal = new ReportDocument();
            //objMainRevDraftInternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

            ////External PDF Generation
            //ReportDocument objMainRevDraftExternal = new ReportDocument();
            //objMainRevDraftExternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

            ////Internal PDF Connections
            //GenerateReportConnection(objMainRevDraftInternal);
            ////External PDF Connections
            //GenerateReportConnection(objMainRevDraftExternal); 
            #endregion

            //Coding Work Sheet PDF Generation
            ReportDocument objMainRevDraftCDWSheet = new ReportDocument();
            objMainRevDraftCDWSheet.Load(Server.MapPath("\\Reports\\" + "rptRevisionMasterReport" + ".rpt"));

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
            //objMainRevDraftInternal.SetParameterValue("@ADJNO", prmAdjNo);
            //objMainRevDraftInternal.SetParameterValue("@FLAG", prmFlag);
            //objMainRevDraftExternal.SetParameterValue("@ADJNO", prmAdjNo);
            //objMainRevDraftExternal.SetParameterValue("@FLAG", prmFlag);
            _VCarrMasterParamsInt.SetValue("\"Parm1:" + prmAdjNo.Value.ToString() + "\"", 1);
            _VCarrMasterParamsInt.SetValue("\"Parm2:" + prmFlag.Value.ToString() + "\"", 2);
            _VCarrMasterParamsExt.SetValue("\"Parm1:" + prmAdjNo.Value.ToString() + "\"", 1);
            _VCarrMasterParamsExt.SetValue("\"Parm2:" + prmFlag.Value.ToString() + "\"", 2);
            objMainRevDraftCDWSheet.SetParameterValue("@ADJNO", prmAdjNo);
            objMainRevDraftCDWSheet.SetParameterValue("@FLAG", prmFlag);
            /*****************Setting Master Report Parameters Value Begin******************/


            InternalReportParametersDelegate delInternal = new InternalReportParametersDelegate(VCInternalReportParameters);
            ExternalReportParametersDelegate delExternal = new ExternalReportParametersDelegate(VCExternalReportParameters);
            CesarReportParametersDelegate delCesar = new CesarReportParametersDelegate(CESARReportParameters);
            IAsyncResult reffdelInternal = delInternal.BeginInvoke(iAdjNo, null, null);
            IAsyncResult reffdelExternal = delExternal.BeginInvoke(iAdjNo, null, null);
            IAsyncResult reffdelCesar = delCesar.BeginInvoke(objMainRevDraftCDWSheet, iAdjNo, null, null);
            reffdelInternal.AsyncWaitHandle.WaitOne();
            reffdelExternal.AsyncWaitHandle.WaitOne();
            reffdelCesar.AsyncWaitHandle.WaitOne();
            delInternal.EndInvoke(reffdelInternal);
            delExternal.EndInvoke(reffdelExternal);
            delCesar.EndInvoke(reffdelCesar);


            /*****************Setting Sub Reports Parameters Value Begin******************/
            VCsetInternalSubReportParameters(prmAdjNo.Value, prmFlag.Value, prmFlipSigns.Value, prmInvNo.Value, prmRevFlag.Value, prmHistFlag.Value);
            //setInternalSubReportParameters(objMainRevDraftExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            VCsetExternalSubReportParameters(prmAdjNo.Value, prmFlag.Value, prmFlipSigns.Value, prmInvNo.Value, prmRevFlag.Value, prmHistFlag.Value);
            //setRevisionCDWSubReportParameters(objMainRevDraftCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmPreviousAdjNo, prmPreviousInvNo, prmHistFlag);
            setRevisionCDWSubReportParameters(objMainRevDraftCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            /*****************Setting Sub Reports Parameters Value End******************/
            string strVal1;
            string strVal2;
            string strVal3;
            string strval4;
            //Start:parallel Process-Calling Export Methods Parallely
            int intPersID = CurrentAISUser.PersonID;
            try
            {
                string strExaternalFilename = "DraftExternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                string strInternalFilename = "DraftInternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                string strCDWorkSheetFilename = "DraftCDWorkSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                string strProgSummSpFilename = "DraftPSSpreadSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".xlsx";
                ExportExternalDelegate delInstanceExternal = new ExportExternalDelegate(ExportReportsWithTOC);
                ExportInternalDelegate delInstanceInternal = new ExportInternalDelegate(ExportReportsWithTOC);
                ExportCesarDelegate delInstanceCesar = new ExportCesarDelegate(ExportReportsWithTOC);
                ExportPorgramSummayDelegate delInstanceProgramSummary = new ExportPorgramSummayDelegate(ExportReportsWithTOC);
                IAsyncResult reffExternal = delInstanceExternal.BeginInvoke(null, strExaternalFilename, iAdjNo, 1, 'E', intPersID, null, null);
                IAsyncResult reffInternal = delInstanceInternal.BeginInvoke(null, strInternalFilename, iAdjNo, 1, 'I', intPersID, null, null);
                IAsyncResult reffCesar = delInstanceCesar.BeginInvoke(objMainRevDraftCDWSheet, strCDWorkSheetFilename, iAdjNo, 1, 'C', intPersID, null, null);
                IAsyncResult reffProgramSummary = delInstanceProgramSummary.BeginInvoke(null, strProgSummSpFilename, iAdjNo, 1, 'P', intPersID, null, null);
                reffExternal.AsyncWaitHandle.WaitOne();
                reffInternal.AsyncWaitHandle.WaitOne();
                reffCesar.AsyncWaitHandle.WaitOne();
                reffProgramSummary.AsyncWaitHandle.WaitOne();
                strVal1 = delInstanceInternal.EndInvoke(reffInternal);
                strVal2 = delInstanceExternal.EndInvoke(reffExternal);
                strVal3 = delInstanceCesar.EndInvoke(reffCesar);
                strval4 = delInstanceProgramSummary.EndInvoke(reffProgramSummary);

            }
            catch (Exception ee)
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
                return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
            }

            //objMainRevDraftInternal.Close();
            //objMainRevDraftExternal.Close();
            objMainRevDraftCDWSheet.Close();
            objMainRevDraftCDWSheet.Dispose();
            if (strVal1 != null && strVal1 != "")
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal1, CurrentAISUser.PersonID);
                return strVal1;
            }
            else if (strVal2 != null && strVal2 != "")
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal2, CurrentAISUser.PersonID);
                return strVal2;
            }
            else if (strVal3 != null && strVal3 != "")
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal3, CurrentAISUser.PersonID);
                return strVal3;
            }
            else if (strval4 != null && strval4 != "")
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strval4, CurrentAISUser.PersonID);
                return strval4;
            }
            else
            {
                objDC.SubmitChanges();
                if (trans != null)
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
            if (trans != null)
                trans.Rollback();
            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
            return "Draft Invoice could not be completed due to an error. Please check the error log for additional details";
        }
        finally
        {
            if (objDC.Connection.State == ConnectionState.Open)
                objDC.Connection.Close();
        }
        return string.Empty;
    }
    #endregion
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

        //objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptAdjNYSecInjFund.rpt");
        //objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSecInjFund.rpt");
        //objMain.SetParameterValue("@INVNO", prmInvNo, "srptAdjNYSecInjFund.rpt");
        //objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSecInjFund.rpt");
        //objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptAdjNYSecInjFund.rpt");

        //objMain.SetParameterValue("@ADJNO", prmPreviousAdjNo, "srptAdjNYSIFRevision.rpt");
        //objMain.SetParameterValue("@FLAG", prmFlag, "srptAdjNYSIFRevision.rpt");
        //objMain.SetParameterValue("@INVNO", prmPreviousInvNo, "srptAdjNYSIFRevision.rpt");
        //objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptAdjNYSIFRevision.rpt");
        //objMain.SetParameterValue("@REVFLAGPREV", true, "srptAdjNYSIFRevision.rpt");

        objMain.SetParameterValue("@ADJNO", prmAdjNo, "srptSurchargesAssessments.rpt");
        objMain.SetParameterValue("@FLAG", prmFlag, "srptSurchargesAssessments.rpt");
        objMain.SetParameterValue("@INVNO", prmInvNo, "srptSurchargesAssessments.rpt");
        objMain.SetParameterValue("@HISTFLAG", prmHistFlag, "srptSurchargesAssessments.rpt");
        objMain.SetParameterValue("@REVFLAGPREV", prmRevFlag, "srptSurchargesAssessments.rpt");

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

    //public string generateFinalInvoice(int iAdjNo, string strComments, bool HistFlag)
    //{

    //    try
    //    {
    //        //To check the U/W status
    //        PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
    //        IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;
    //        //Retrieving UWReviewed Statuse from lookup table
    //        int intAdjUWReviewedStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.UWReviewed, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
    //        objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(iAdjNo);
    //        //objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[objPremAdjStsBE.Count - 1].PremumAdj_sts_ID);
    //        objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
    //        //if it is Final Invoice we are verifying the status.if status is not equal to U/W then show error
    //        if (objPremStsBE.ADJ_STS_TYP_ID == 349)
    //        {
    //            string strMessage = "Adjustment is already Finalized. Please refresh the page";
    //            return strMessage;
    //        }
    //        if (objPremStsBE.ADJ_STS_TYP_ID != intAdjUWReviewedStatusID && HistFlag != true)
    //        {
    //            string strMessage = "Adjustment is not reviewed by U/W";
    //            return strMessage;
    //        }

    //        bool UpdateInvNum = false;
    //        bool InsertStatus = false;
    //        objDC.Connection.Open();
    //        trans = objDC.Connection.BeginTransaction();
    //        objDC.Transaction = trans;
    //        //Calling Generate InvoiceNumber Function
    //        string strInvNo = generateInvoiceNumbers(iAdjNo, 2, 0, 0);

    //        ////Retrieving Final Inv  Statuses from lookup table
    //        int intFinalInvStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.FinalInvd, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
    //        //Final Invoice Updation
    //        UpdateInvNum = new InvoiceDriverBS().UpdatePremAdjutmentFinalInvoiceData(objDC, iAdjNo, strInvNo, CurrentAISUser.PersonID);
    //        InsertStatus = new InvoiceDriverBS().InsertPremAdjutmentStatusData(objDC, iAdjNo, intFinalInvStatusID, strComments, CurrentAISUser.PersonID);
    //        if (UpdateInvNum != true || InsertStatus != true)
    //        {
    //            if (trans != null)
    //            trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Prem_adj or Prem_adj_sts table updation Failed", CurrentAISUser.PersonID);
    //            return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
    //        }

    //        //Internal PDF Generation
    //        ReportDocument objMainFinalInternal = new ReportDocument();
    //        objMainFinalInternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

    //        //External PDF Generation
    //        ReportDocument objMainFinalExternal = new ReportDocument();
    //        objMainFinalExternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

    //        //Coding Work Sheet PDF Generation
    //        ReportDocument objMainFinalCDWSheet = new ReportDocument();
    //        objMainFinalCDWSheet.Load(Server.MapPath("\\Reports\\" + "rptCodingMasterReport" + ".rpt"));

    //        //Internal PDF Connections
    //        GenerateReportConnection(objMainFinalInternal);
    //        //External PDF Connections
    //        GenerateReportConnection(objMainFinalExternal);
    //        //Coding Work Sheet PDF Connections
    //        GenerateReportConnection(objMainFinalCDWSheet);

    //        objMainFinalInternal.VerifyDatabase();
    //        objMainFinalExternal.VerifyDatabase();
    //        objMainFinalCDWSheet.VerifyDatabase();



    //        ParameterDiscreteValue prmAdjNo = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmFlag = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmFlipSigns = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmRevFlag = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmInvNo = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmHistFlag = new ParameterDiscreteValue();
    //        prmAdjNo.Value = iAdjNo;
    //        //Final Invoice
    //        prmFlag.Value = 2;
    //        prmFlipSigns.Value = false;
    //        prmRevFlag.Value = false;
    //        prmInvNo.Value = strInvNo;
    //        prmHistFlag.Value = HistFlag;//Need to Change
    //        /*****************Setting Master Report Parameters Value Begin******************/
    //        objMainFinalInternal.SetParameterValue("@ADJNO", prmAdjNo);
    //        objMainFinalInternal.SetParameterValue("@FLAG", prmFlag);
    //        objMainFinalExternal.SetParameterValue("@ADJNO", prmAdjNo);
    //        objMainFinalExternal.SetParameterValue("@FLAG", prmFlag);
    //        objMainFinalCDWSheet.SetParameterValue("@ADJNO", prmAdjNo);
    //        objMainFinalCDWSheet.SetParameterValue("@FLAG", prmFlag);
    //        /*****************Setting Master Report Parameters Value Begin******************/



    //        ////Final Invoice Internal PDF
    //        //IList<InvoiceExhibitBE> objFinalInternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(1);
    //        //for (int iCount = 0; iCount < objFinalInternalIlistBE.Count; iCount++)
    //        //{

    //        //    setMasterReportParameter(objMainFinalInternal, objFinalInternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalInternalIlistBE[iCount].STS_IND), 1, Convert.ToChar(objFinalInternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

    //        //}

    //        ////Final Invoice External PDF
    //        //IList<InvoiceExhibitBE> objFinalExternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(2);
    //        //for (int iCount = 0; iCount < objFinalExternalIlistBE.Count; iCount++)
    //        //{

    //        //    setMasterReportParameter(objMainFinalExternal, objFinalExternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalExternalIlistBE[iCount].STS_IND), 2, Convert.ToChar(objFinalExternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

    //        //}
    //        ////Final Coding Work Sheet PDF 
    //        //IList<InvoiceExhibitBE> objFinalCDWorksheetIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(3);
    //        //for (int iCount = 0; iCount < objFinalCDWorksheetIlistBE.Count; iCount++)
    //        //{

    //        //    setMasterReportParameter(objMainFinalCDWSheet, objFinalCDWorksheetIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalCDWorksheetIlistBE[iCount].STS_IND), 3, Convert.ToChar(objFinalCDWorksheetIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

    //        //}

    //        //Final Invoice-After Delegates
    //        FinalInternalReportParametersDelegate delInternal = new FinalInternalReportParametersDelegate(FinalInternalReportParameters);
    //        FinalExternalReportParametersDelegate delExternal = new FinalExternalReportParametersDelegate(FinalExternalReportParameters);
    //        FinalCesarReportParametersDelegate delCesar = new FinalCesarReportParametersDelegate(FinalCESARReportParameters);
    //        IAsyncResult reffdelInternal = delInternal.BeginInvoke(objMainFinalInternal, iAdjNo, null, null);
    //        IAsyncResult reffdelExternal = delExternal.BeginInvoke(objMainFinalExternal, iAdjNo, null, null);
    //        IAsyncResult reffdelCesar = delCesar.BeginInvoke(objMainFinalCDWSheet, iAdjNo, null, null);
    //        reffdelInternal.AsyncWaitHandle.WaitOne();
    //        reffdelExternal.AsyncWaitHandle.WaitOne();
    //        reffdelCesar.AsyncWaitHandle.WaitOne();
    //        delInternal.EndInvoke(reffdelInternal);
    //        delExternal.EndInvoke(reffdelExternal);
    //        delCesar.EndInvoke(reffdelCesar);


    //        /*****************Setting Sub Reports Parameters Value Begin******************/
    //        setInternalSubReportParameters(objMainFinalInternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
    //        //setInternalSubReportParameters(objMainFinalExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
    //        setExternalSubReportParameters(objMainFinalExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
    //        setCDWSubReportParameters(objMainFinalCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
    //        /*****************Setting Sub Reports Parameters Value End******************/
    //        string strVal1;
    //        string strVal2;
    //        string strVal3;


    //        //Start:parallel Process-Calling Export Methods Parallely
    //        int intPersID = CurrentAISUser.PersonID;
    //        try
    //        {
    //            string strInternalFilename = "FinalInternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //            string strExaternalFilename = "FinalExternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //            string strCDWorkSheetFilename = "FinalCDWorkSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //            ExportExternalDelegate delInstanceExternal = new ExportExternalDelegate(ExportReports);
    //            ExportInternalDelegate delInstanceInternal = new ExportInternalDelegate(ExportReports);
    //            ExportCesarDelegate delInstanceCesar = new ExportCesarDelegate(ExportReports);
    //            IAsyncResult reffExternal = delInstanceExternal.BeginInvoke(objMainFinalExternal, strExaternalFilename, iAdjNo, 2, 'E', intPersID, null, null);
    //            IAsyncResult reffInternal = delInstanceInternal.BeginInvoke(objMainFinalInternal, strInternalFilename, iAdjNo, 2, 'I', intPersID, null, null);
    //            IAsyncResult reffCesar = delInstanceCesar.BeginInvoke(objMainFinalCDWSheet, strCDWorkSheetFilename, iAdjNo, 2, 'C', intPersID, null, null);
    //            reffExternal.AsyncWaitHandle.WaitOne();
    //            reffInternal.AsyncWaitHandle.WaitOne();
    //            reffCesar.AsyncWaitHandle.WaitOne();
    //            strVal1 = delInstanceInternal.EndInvoke(reffInternal);
    //            strVal2 = delInstanceExternal.EndInvoke(reffExternal);
    //            strVal3 = delInstanceCesar.EndInvoke(reffCesar);

    //        }
    //        catch (Exception ee)
    //        {
    //            if (trans != null)
    //               trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
    //            return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
    //        }
    //        //try
    //        //{


    //        //    string strInternalFilename = "FinalInternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //        //    strVal1 = ExportReports(objMainFinalInternal, strInternalFilename, iAdjNo, 2, 'I');

    //        //}
    //        //catch (Exception ee)
    //        //{
    //        //    if (trans != null)
    //        //    trans.Rollback();
    //        //    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
    //        //    return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
    //        //}
    //        //try
    //        //{


    //        //    string strExaternalFilename = "FinalExternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //        //    strVal2 = ExportReports(objMainFinalExternal, strExaternalFilename, iAdjNo, 2, 'E');

    //        //}
    //        //catch (Exception ee)
    //        //{
    //        //    if (trans != null)
    //        //    trans.Rollback();
    //        //    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
    //        //    return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
    //        //}
    //        //try
    //        //{

    //        //    string strCDWorkSheetFilename = "FinalCDWorkSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //        //    strVal3 = ExportReports(objMainFinalCDWSheet, strCDWorkSheetFilename, iAdjNo, 2, 'C');

    //        //}
    //        //catch (Exception ee)
    //        //{
    //        //    if (trans != null)
    //        //    trans.Rollback();
    //        //    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
    //        //    return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
    //        //}


    //        objMainFinalInternal.Close();
    //        objMainFinalExternal.Close();
    //        objMainFinalCDWSheet.Close();
    //        if (strVal1 != null && strVal1 != "")
    //        {
    //            if (trans != null)
    //            trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal1, CurrentAISUser.PersonID);
    //            return strVal1;
    //        }
    //        else if (strVal2 != null && strVal2 != "")
    //        {
    //            if (trans != null)
    //            trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal2, CurrentAISUser.PersonID);
    //            return strVal2;
    //        }
    //        else if (strVal3 != null && strVal3 != "")
    //        {
    //            if (trans != null)
    //            trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal3, CurrentAISUser.PersonID);
    //            return strVal3;
    //        }
    //        else
    //        {
    //            objDC.SubmitChanges();
    //            if (trans != null)
    //            trans.Commit();
    //            ProgramPeriodsBS prgBS = new ProgramPeriodsBS();
    //            prgBS.ModAISTransmittalToARiES(iAdjNo, 1);
    //            try
    //            {
    //                EMailNotification(iAdjNo, 2);
    //                return "The Adjustment Final Invoice has been submitted for processing. The Final invoice number is " + strInvNo + ". Please note that the pdf copy of the Final invoice will be available after 15 minutes";
    //            }
    //            catch (Exception ex)
    //            {
    //                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
    //                return "The Adjustment Final Invoice has been submitted for processing. The Final invoice number is " + strInvNo + ". Please note that the pdf copy of the Final invoice will be available after 15 minutes,But E-Mail Notification Failed";
    //            }

    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        if (trans != null)
    //        trans.Rollback();
    //        new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
    //        return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
    //    }
    //    finally
    //    {
    //        if (objDC.Connection.State == ConnectionState.Open)
    //        objDC.Connection.Close();
    //    }
    //    return string.Empty;
    //}
    public string generateFinalInvoiceWithTOC(int iAdjNo, string strComments, bool HistFlag)
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
            ViewState["FinalstrInvNo"] = strInvNo;
            ////Retrieving Final Inv  Statuses from lookup table
            int intFinalInvStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.FinalInvd, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            //Final Invoice Updation
            UpdateInvNum = new InvoiceDriverBS().UpdatePremAdjutmentFinalInvoiceData(objDC, iAdjNo, strInvNo, CurrentAISUser.PersonID);
            InsertStatus = new InvoiceDriverBS().InsertPremAdjutmentStatusData(objDC, iAdjNo, intFinalInvStatusID, strComments, CurrentAISUser.PersonID);
            if (UpdateInvNum != true || InsertStatus != true)
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Prem_adj or Prem_adj_sts table updation Failed", CurrentAISUser.PersonID);
                return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
            }


            //Coding Work Sheet PDF Generation
            ReportDocument objMainFinalCDWSheet = new ReportDocument();
            objMainFinalCDWSheet.Load(Server.MapPath("\\Reports\\" + "rptCodingMasterReport" + ".rpt"));

            GenerateReportConnection(objMainFinalCDWSheet);
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
            _VCarrMasterParamsInt.SetValue("\"Parm1:" + prmAdjNo.Value.ToString() + "\"", 1);
            _VCarrMasterParamsInt.SetValue("\"Parm2:" + prmFlag.Value.ToString() + "\"", 2);
            _VCarrMasterParamsExt.SetValue("\"Parm1:" + prmAdjNo.Value.ToString() + "\"", 1);
            _VCarrMasterParamsExt.SetValue("\"Parm2:" + prmFlag.Value.ToString() + "\"", 2);
            objMainFinalCDWSheet.SetParameterValue("@ADJNO", prmAdjNo);
            objMainFinalCDWSheet.SetParameterValue("@FLAG", prmFlag);
            /*****************Setting Master Report Parameters Value Begin******************/



            //Final Invoice-After Delegates
            FinalInternalReportParametersDelegate delInternal = new FinalInternalReportParametersDelegate(VCFinalInternalReportParameters);
            FinalExternalReportParametersDelegate delExternal = new FinalExternalReportParametersDelegate(VCFinalExternalReportParameters);
            FinalCesarReportParametersDelegate delCesar = new FinalCesarReportParametersDelegate(FinalCESARReportParameters);
            IAsyncResult reffdelInternal = delInternal.BeginInvoke(iAdjNo, null, null);
            IAsyncResult reffdelExternal = delExternal.BeginInvoke(iAdjNo, null, null);
            IAsyncResult reffdelCesar = delCesar.BeginInvoke(objMainFinalCDWSheet, iAdjNo, null, null);
            reffdelInternal.AsyncWaitHandle.WaitOne();
            reffdelExternal.AsyncWaitHandle.WaitOne();
            reffdelCesar.AsyncWaitHandle.WaitOne();
            delInternal.EndInvoke(reffdelInternal);
            delExternal.EndInvoke(reffdelExternal);
            delCesar.EndInvoke(reffdelCesar);


            /*****************Setting Sub Reports Parameters Value Begin******************/
            VCsetInternalSubReportParameters(prmAdjNo.Value, prmFlag.Value, prmFlipSigns.Value, prmInvNo.Value, prmRevFlag.Value, prmHistFlag.Value);
            //setInternalSubReportParameters(objMainFinalExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            VCsetExternalSubReportParameters(prmAdjNo.Value, prmFlag.Value, prmFlipSigns.Value, prmInvNo.Value, prmRevFlag.Value, prmHistFlag.Value);
            setCDWSubReportParameters(objMainFinalCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            /*****************Setting Sub Reports Parameters Value End******************/
            string strVal1;
            string strVal2;
            string strVal3;
            string strval4;

            //Start:parallel Process-Calling Export Methods Parallely
            int intPersID = CurrentAISUser.PersonID;
            try
            {
                string strInternalFilename = "FinalInternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                string strExaternalFilename = "FinalExternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                string strCDWorkSheetFilename = "FinalCDWorkSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                string strProgSummSpFilename = "FinalPSSpreadSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".xlsx";
                ExportExternalDelegate delInstanceExternal = new ExportExternalDelegate(ExportReportsWithTOC);
                ExportInternalDelegate delInstanceInternal = new ExportInternalDelegate(ExportReportsWithTOC);
                ExportCesarDelegate delInstanceCesar = new ExportCesarDelegate(ExportReportsWithTOC);
                ExportPorgramSummayDelegate delInstanceProgramSummary = new ExportPorgramSummayDelegate(ExportReportsWithTOC);
                IAsyncResult reffExternal = delInstanceExternal.BeginInvoke(null, strExaternalFilename, iAdjNo, 2, 'E', intPersID, null, null);
                IAsyncResult reffInternal = delInstanceInternal.BeginInvoke(null, strInternalFilename, iAdjNo, 2, 'I', intPersID, null, null);
                IAsyncResult reffCesar = delInstanceCesar.BeginInvoke(objMainFinalCDWSheet, strCDWorkSheetFilename, iAdjNo, 2, 'C', intPersID, null, null);
                IAsyncResult reffProgramSummary = delInstanceProgramSummary.BeginInvoke(null, strProgSummSpFilename, iAdjNo, 2, 'P', intPersID, null, null);
                reffExternal.AsyncWaitHandle.WaitOne();
                reffInternal.AsyncWaitHandle.WaitOne();
                reffCesar.AsyncWaitHandle.WaitOne();
                reffProgramSummary.AsyncWaitHandle.WaitOne();
                strVal1 = delInstanceInternal.EndInvoke(reffInternal);
                strVal2 = delInstanceExternal.EndInvoke(reffExternal);
                strVal3 = delInstanceCesar.EndInvoke(reffCesar);
                strval4 = delInstanceProgramSummary.EndInvoke(reffProgramSummary);

            }
            catch (Exception ee)
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
                return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
            }
            objMainFinalCDWSheet.Close();
            if (strVal1 != null && strVal1 != "")
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal1, CurrentAISUser.PersonID);
                return strVal1;
            }
            else if (strVal2 != null && strVal2 != "")
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal2, CurrentAISUser.PersonID);
                return strVal2;
            }
            else if (strVal3 != null && strVal3 != "")
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal3, CurrentAISUser.PersonID);
                return strVal3;
            }
            else if (strval4 != null && strval4 != "")
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strval4, CurrentAISUser.PersonID);
                return strval4;
            }
            else
            {
                objDC.SubmitChanges();
                if (trans != null)
                    trans.Commit();
                ProgramPeriodsBS prgBS = new ProgramPeriodsBS();
                prgBS.ModAISTransmittalToARiES(iAdjNo, 1);

                //to update tranmittal history status to 1 for test accounts.
                (new AccountBS()).UpdateTransmittalHistoryForTestAcct();

                prgBS.ModAISCodingToCesar(iAdjNo);

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
            if (trans != null)
                trans.Rollback();
            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
            return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
        }
        finally
        {
            if (objDC.Connection.State == ConnectionState.Open)
                objDC.Connection.Close();
        }
        return string.Empty;
    }
    //Start:Parallel Processing-Methods to set Master Report Parameters in a loop
    public void FinalInternalReportParameters(ReportDocument objMain, int iAdjNo)
    {
        ////Final Invoice Internal PDF
        IList<InvoiceExhibitBE> objFinalInternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(1);
        for (int iCount = 0; iCount < objFinalInternalIlistBE.Count; iCount++)
        {

            setMasterReportParameter(objMain, objFinalInternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalInternalIlistBE[iCount].STS_IND), 1, Convert.ToChar(objFinalInternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

        }

    }
    public void FinalExternalReportParameters(ReportDocument objMain, int iAdjNo)
    {
        ////Final Invoice External PDF
        IList<InvoiceExhibitBE> objFinalExternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(2);
        for (int iCount = 0; iCount < objFinalExternalIlistBE.Count; iCount++)
        {

            setMasterReportParameter(objMain, objFinalExternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalExternalIlistBE[iCount].STS_IND), 2, Convert.ToChar(objFinalExternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

        }
    }
    public void FinalCESARReportParameters(ReportDocument objMain, int iAdjNo)
    {
        ////Final Coding Work Sheet PDF 
        IList<InvoiceExhibitBE> objFinalCDWorksheetIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(3);
        for (int iCount = 0; iCount < objFinalCDWorksheetIlistBE.Count; iCount++)
        {

            setMasterReportParameter(objMain, objFinalCDWorksheetIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalCDWorksheetIlistBE[iCount].STS_IND), 3, Convert.ToChar(objFinalCDWorksheetIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

        }
    }

    public void VCFinalExternalReportParameters(int iAdjNo)
    {
        ////Final Invoice External PDF
        IList<InvoiceExhibitBE> objFinalExternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(2);
        for (int iCount = 0; iCount < objFinalExternalIlistBE.Count; iCount++)
        {

            VCsetMasterReportParameter(null, objFinalExternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalExternalIlistBE[iCount].STS_IND), 2, Convert.ToChar(objFinalExternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

        }
    }
    //Start:Parallel Processing-Methods to set Master Report Parameters in a loop
    public void VCFinalInternalReportParameters(int iAdjNo)
    {
        ////Final Invoice Internal PDF
        IList<InvoiceExhibitBE> objFinalInternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(1);
        for (int iCount = 0; iCount < objFinalInternalIlistBE.Count; iCount++)
        {
            VCsetMasterReportParameter(null, objFinalInternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalInternalIlistBE[iCount].STS_IND), 1, Convert.ToChar(objFinalInternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

        }

    }
    //public string generateRevisedFinalInvoice(int iAdjNo, int iPreviousAdjNo, string strComments, bool HistFlag)
    //{

    //    try
    //    {
    //        //To check the U/W status
    //        PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
    //        IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;
    //        //Retrieving UWReviewed Statuse from lookup table
    //        int intAdjUWReviewedStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.UWReviewed, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
    //        objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(iAdjNo);
    //        //objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[objPremAdjStsBE.Count - 1].PremumAdj_sts_ID);
    //        objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
    //        //if it is Final Invoice we are verifying the status.if status is not equal to U/W then show error
    //        if (objPremStsBE.ADJ_STS_TYP_ID == 349)
    //        {
    //            string strMessage = "Adjustment is already Finalized. Please refresh the page";
    //            return strMessage;
    //        }
    //        if (objPremStsBE.ADJ_STS_TYP_ID != intAdjUWReviewedStatusID && HistFlag != true)
    //        {
    //            string strMessage = "Adjustment is not reviewed by U/W";
    //            return strMessage;
    //        }
    //        PremiumAdjustmentBE prevPremAdjBE = new PremAdjustmentBS().getPremiumAdjustmentRow(iPreviousAdjNo);
    //        string strPrevInvNo = prevPremAdjBE.FNL_INVC_NBR_TXT;
    //        bool UpdateInvNum = false;
    //        bool InsertStatus = false;
    //        objDC.Connection.Open();
    //        trans = objDC.Connection.BeginTransaction();
    //        objDC.Transaction = trans;
    //        //Calling Generate InvoiceNumber Function
    //        string strInvNo = generateInvoiceNumbers(iAdjNo, 3, iPreviousAdjNo, 2);

    //        ////Retrieving Final Inv  Statuses from lookup table
    //        int intFinalInvStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.FinalInvd, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
    //        //Final Invoice Updation
    //        UpdateInvNum = new InvoiceDriverBS().UpdatePremAdjutmentFinalInvoiceData(objDC, iAdjNo, strInvNo, CurrentAISUser.PersonID);
    //        InsertStatus = new InvoiceDriverBS().InsertPremAdjutmentStatusData(objDC, iAdjNo, intFinalInvStatusID, strComments, CurrentAISUser.PersonID);
    //        if (UpdateInvNum != true || InsertStatus != true)
    //        {
    //            if (trans != null)
    //            trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Prem_adj or Prem_adj_sts table updation Failed", CurrentAISUser.PersonID);
    //            return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
    //        }

    //        //Internal PDF Generation
    //        ReportDocument objMainRevFinlaInternal = new ReportDocument();
    //        objMainRevFinlaInternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

    //        //External PDF Generation
    //        ReportDocument objMainRevFinalExternal = new ReportDocument();
    //        objMainRevFinalExternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

    //        //Coding Work Sheet PDF Generation
    //        ReportDocument objMainRevFinalCDWSheet = new ReportDocument();
    //        objMainRevFinalCDWSheet.Load(Server.MapPath("\\Reports\\" + "rptRevisionMasterReport" + ".rpt"));

    //        //Internal PDF Connections
    //        GenerateReportConnection(objMainRevFinlaInternal);
    //        //External PDF Connections
    //        GenerateReportConnection(objMainRevFinalExternal);
    //        //Coding Work Sheet PDF Connections
    //        GenerateReportConnection(objMainRevFinalCDWSheet);



    //        ParameterDiscreteValue prmAdjNo = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmFlag = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmPreviousAdjNo = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmPreviousInvNo = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmFlipSigns = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmRevFlag = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmInvNo = new ParameterDiscreteValue();
    //        ParameterDiscreteValue prmHistFlag = new ParameterDiscreteValue();
    //        prmAdjNo.Value = iAdjNo;
    //        //Draft Invoice
    //        prmFlag.Value = 2;
    //        //prmPreviousAdjNo.Value = iPreviousAdjNo;
    //        prmFlipSigns.Value = false;
    //        prmRevFlag.Value = false;
    //        prmInvNo.Value = strInvNo;
    //        //prmPreviousInvNo.Value = strPrevInvNo;
    //        prmHistFlag.Value = HistFlag;

    //        /*****************Setting Master Report Parameters Value Begin******************/
    //        objMainRevFinlaInternal.SetParameterValue("@ADJNO", prmAdjNo);
    //        objMainRevFinlaInternal.SetParameterValue("@FLAG", prmFlag);
    //        objMainRevFinalExternal.SetParameterValue("@ADJNO", prmAdjNo);
    //        objMainRevFinalExternal.SetParameterValue("@FLAG", prmFlag);
    //        objMainRevFinalCDWSheet.SetParameterValue("@ADJNO", prmAdjNo);
    //        objMainRevFinalCDWSheet.SetParameterValue("@FLAG", prmFlag);
    //        /*****************Setting Master Report Parameters Value Begin******************/

    //        //Final Invoice Internal PDF
    //        //IList<InvoiceExhibitBE> objFinalInternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(1);
    //        //for (int iCount = 0; iCount < objFinalInternalIlistBE.Count; iCount++)
    //        //{

    //        //    setMasterReportParameter(objMainRevFinlaInternal, objFinalInternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalInternalIlistBE[iCount].STS_IND), 1, Convert.ToChar(objFinalInternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

    //        //}

    //        ////Final Invoice External PDF
    //        //IList<InvoiceExhibitBE> objFinalExternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(2);
    //        //for (int iCount = 0; iCount < objFinalExternalIlistBE.Count; iCount++)
    //        //{

    //        //    setMasterReportParameter(objMainRevFinalExternal, objFinalExternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalExternalIlistBE[iCount].STS_IND), 2, Convert.ToChar(objFinalExternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

    //        //}
    //        ////Final Coding Work Sheet PDF 
    //        //IList<InvoiceExhibitBE> objFinalCDWorksheetIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(3);
    //        //for (int iCount = 0; iCount < objFinalCDWorksheetIlistBE.Count; iCount++)
    //        //{

    //        //    setMasterReportParameter(objMainRevFinalCDWSheet, objFinalCDWorksheetIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalCDWorksheetIlistBE[iCount].STS_IND), 3, Convert.ToChar(objFinalCDWorksheetIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

    //        //}

    //        FinalInternalReportParametersDelegate delInternal = new FinalInternalReportParametersDelegate(FinalInternalReportParameters);
    //        FinalExternalReportParametersDelegate delExternal = new FinalExternalReportParametersDelegate(FinalExternalReportParameters);
    //        FinalCesarReportParametersDelegate delCesar = new FinalCesarReportParametersDelegate(FinalCESARReportParameters);
    //        IAsyncResult reffdelInternal = delInternal.BeginInvoke(objMainRevFinlaInternal, iAdjNo, null, null);
    //        IAsyncResult reffdelExternal = delExternal.BeginInvoke(objMainRevFinalExternal, iAdjNo, null, null);
    //        IAsyncResult reffdelCesar = delCesar.BeginInvoke(objMainRevFinalCDWSheet, iAdjNo, null, null);
    //        reffdelInternal.AsyncWaitHandle.WaitOne();
    //        reffdelExternal.AsyncWaitHandle.WaitOne();
    //        reffdelCesar.AsyncWaitHandle.WaitOne();
    //        delInternal.EndInvoke(reffdelInternal);
    //        delExternal.EndInvoke(reffdelExternal);
    //        delCesar.EndInvoke(reffdelCesar);


    //        /*****************Setting Sub Reports Parameters Value Begin******************/
    //        setInternalSubReportParameters(objMainRevFinlaInternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
    //        //setInternalSubReportParameters(objMainRevFinalExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
    //        setExternalSubReportParameters(objMainRevFinalExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
    //        //setRevisionCDWSubReportParameters(objMainRevFinalCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmPreviousAdjNo, prmPreviousInvNo, prmHistFlag);
    //        setRevisionCDWSubReportParameters(objMainRevFinalCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
    //        /*****************Setting Sub Reports Parameters Value End******************/
    //        string strVal1;
    //        string strVal2;
    //        string strVal3;

    //        //Start:parallel Process-Calling Export Methods Parallely
    //        int intPersID = CurrentAISUser.PersonID;
    //        try
    //        {
    //            string strInternalFilename = "FinalInternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //            string strExaternalFilename = "FinalExternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //            string strCDWorkSheetFilename = "FinalCDWorkSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //            ExportExternalDelegate delInstanceExternal = new ExportExternalDelegate(ExportReports);
    //            ExportInternalDelegate delInstanceInternal = new ExportInternalDelegate(ExportReports);
    //            ExportCesarDelegate delInstanceCesar = new ExportCesarDelegate(ExportReports);
    //            IAsyncResult reffExternal = delInstanceExternal.BeginInvoke(objMainRevFinalExternal, strExaternalFilename, iAdjNo, 2, 'E', intPersID, null, null);
    //            IAsyncResult reffInternal = delInstanceInternal.BeginInvoke(objMainRevFinlaInternal, strInternalFilename, iAdjNo, 2, 'I', intPersID, null, null);
    //            IAsyncResult reffCesar = delInstanceCesar.BeginInvoke(objMainRevFinalCDWSheet, strCDWorkSheetFilename, iAdjNo, 2, 'C', intPersID, null, null);
    //            reffExternal.AsyncWaitHandle.WaitOne();
    //            reffInternal.AsyncWaitHandle.WaitOne();
    //            reffCesar.AsyncWaitHandle.WaitOne();
    //            strVal1 = delInstanceInternal.EndInvoke(reffInternal);
    //            strVal2 = delInstanceExternal.EndInvoke(reffExternal);
    //            strVal3 = delInstanceCesar.EndInvoke(reffCesar);

    //        }
    //        catch (Exception ee)
    //        {
    //            if (trans != null)
    //                trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
    //            return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
    //        }
    //        //try
    //        //{


    //        //    string strInternalFilename = "FinalInternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //        //    strVal1 = ExportReports(objMainRevFinlaInternal, strInternalFilename, iAdjNo, 2, 'I');

    //        //}
    //        //catch (Exception ee)
    //        //{
    //        //    if (trans != null)
    //        //    trans.Rollback();
    //        //    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
    //        //    return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
    //        //}
    //        //try
    //        //{


    //        //    string strExaternalFilename = "FinalExternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //        //    strVal2 = ExportReports(objMainRevFinalExternal, strExaternalFilename, iAdjNo, 2, 'E');

    //        //}
    //        //catch (Exception ee)
    //        //{
    //        //    if (trans != null)
    //        //    trans.Rollback();
    //        //    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
    //        //    return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
    //        //}
    //        //try
    //        //{

    //        //    string strCDWorkSheetFilename = "FinalCDWorkSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
    //        //    strVal3 = ExportReports(objMainRevFinalCDWSheet, strCDWorkSheetFilename, iAdjNo, 2, 'C');

    //        //}
    //        //catch (Exception ee)
    //        //{
    //        //    if (trans != null)
    //        //    trans.Rollback();
    //        //    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
    //        //    return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
    //        //}
    //        objMainRevFinlaInternal.Close();
    //        objMainRevFinalExternal.Close();
    //        objMainRevFinalCDWSheet.Close();
    //        if (strVal1 != null && strVal1 != "")
    //        {
    //            if (trans != null)
    //            trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal1, CurrentAISUser.PersonID);
    //            return strVal1;
    //        }
    //        else if (strVal2 != null && strVal2 != "")
    //        {
    //            if (trans != null)
    //            trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal2, CurrentAISUser.PersonID);
    //            return strVal2;
    //        }
    //        else if (strVal3 != null && strVal3 != "")
    //        {
    //            if (trans != null)
    //            trans.Rollback();
    //            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal3, CurrentAISUser.PersonID);
    //            return strVal3;
    //        }
    //        else
    //        {
    //            objDC.SubmitChanges();
    //            if (trans != null)
    //            trans.Commit();
    //            ProgramPeriodsBS prgBS = new ProgramPeriodsBS();
    //            prgBS.ModAISTransmittalToARiES(iAdjNo, 1);
    //            try
    //            {
    //                EMailNotification(iAdjNo, 2);
    //                return "The Adjustment Final Invoice has been submitted for processing. The Final invoice number is " + strInvNo + ". Please note that the pdf copy of the Final invoice will be available after 15 minutes";
    //            }
    //            catch (Exception ex)
    //            {
    //                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
    //                return "The Adjustment Final Invoice has been submitted for processing. The Final invoice number is " + strInvNo + ". Please note that the pdf copy of the Final invoice will be available after 15 minutes,But E-Mail Notification Failed";
    //            }

    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        if (trans != null)
    //        trans.Rollback();
    //        new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
    //        return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
    //    }
    //    finally
    //    {
    //        if (objDC.Connection.State == ConnectionState.Open)
    //        objDC.Connection.Close();
    //    }
    //    return string.Empty;
    //}
    public string generateRevisedFinalInvoiceWithTOC(int iAdjNo, int iPreviousAdjNo, string strComments, bool HistFlag)
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
            ViewState["FinalstrInvNo"] = strInvNo;
            ////Retrieving Final Inv  Statuses from lookup table
            int intFinalInvStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.FinalInvd, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            //Final Invoice Updation
            UpdateInvNum = new InvoiceDriverBS().UpdatePremAdjutmentFinalInvoiceData(objDC, iAdjNo, strInvNo, CurrentAISUser.PersonID);
            InsertStatus = new InvoiceDriverBS().InsertPremAdjutmentStatusData(objDC, iAdjNo, intFinalInvStatusID, strComments, CurrentAISUser.PersonID);
            if (UpdateInvNum != true || InsertStatus != true)
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", "Prem_adj or Prem_adj_sts table updation Failed", CurrentAISUser.PersonID);
                return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
            }

            ////Internal PDF Generation
            //ReportDocument objMainRevFinlaInternal = new ReportDocument();
            //objMainRevFinlaInternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

            ////External PDF Generation
            //ReportDocument objMainRevFinalExternal = new ReportDocument();
            //objMainRevFinalExternal.Load(Server.MapPath("\\Reports\\" + "rptIntExtlMasterReport" + ".rpt"));

            //Coding Work Sheet PDF Generation
            ReportDocument objMainRevFinalCDWSheet = new ReportDocument();
            objMainRevFinalCDWSheet.Load(Server.MapPath("\\Reports\\" + "rptRevisionMasterReport" + ".rpt"));

            ////Internal PDF Connections
            //GenerateReportConnection(objMainRevFinlaInternal);
            ////External PDF Connections
            //GenerateReportConnection(objMainRevFinalExternal);
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
            _VCarrMasterParamsInt.SetValue("\"Parm1:" + prmAdjNo.Value.ToString() + "\"", 1);
            _VCarrMasterParamsInt.SetValue("\"Parm2:" + prmFlag.Value.ToString() + "\"", 2);
            _VCarrMasterParamsExt.SetValue("\"Parm1:" + prmAdjNo.Value.ToString() + "\"", 1);
            _VCarrMasterParamsExt.SetValue("\"Parm2:" + prmFlag.Value.ToString() + "\"", 2);
            objMainRevFinalCDWSheet.SetParameterValue("@ADJNO", prmAdjNo);
            objMainRevFinalCDWSheet.SetParameterValue("@FLAG", prmFlag);
            /*****************Setting Master Report Parameters Value Begin******************/

            //Final Invoice Internal PDF
            //IList<InvoiceExhibitBE> objFinalInternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(1);
            //for (int iCount = 0; iCount < objFinalInternalIlistBE.Count; iCount++)
            //{

            //    setMasterReportParameter(objMainRevFinlaInternal, objFinalInternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalInternalIlistBE[iCount].STS_IND), 1, Convert.ToChar(objFinalInternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            //}

            ////Final Invoice External PDF
            //IList<InvoiceExhibitBE> objFinalExternalIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(2);
            //for (int iCount = 0; iCount < objFinalExternalIlistBE.Count; iCount++)
            //{

            //    setMasterReportParameter(objMainRevFinalExternal, objFinalExternalIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalExternalIlistBE[iCount].STS_IND), 2, Convert.ToChar(objFinalExternalIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            //}
            ////Final Coding Work Sheet PDF 
            //IList<InvoiceExhibitBE> objFinalCDWorksheetIlistBE = new InvoiceExhibitBS().getInvoiceExhibitData(3);
            //for (int iCount = 0; iCount < objFinalCDWorksheetIlistBE.Count; iCount++)
            //{

            //    setMasterReportParameter(objMainRevFinalCDWSheet, objFinalCDWorksheetIlistBE[iCount].ATCH_CD.Trim(), Convert.ToBoolean(objFinalCDWorksheetIlistBE[iCount].STS_IND), 3, Convert.ToChar(objFinalCDWorksheetIlistBE[iCount].INTRNL_FLAG_IND), iAdjNo);

            //}

            FinalInternalReportParametersDelegate delInternal = new FinalInternalReportParametersDelegate(VCFinalInternalReportParameters);
            FinalExternalReportParametersDelegate delExternal = new FinalExternalReportParametersDelegate(VCFinalExternalReportParameters);
            FinalCesarReportParametersDelegate delCesar = new FinalCesarReportParametersDelegate(FinalCESARReportParameters);
            IAsyncResult reffdelInternal = delInternal.BeginInvoke(iAdjNo, null, null);
            IAsyncResult reffdelExternal = delExternal.BeginInvoke(iAdjNo, null, null);
            IAsyncResult reffdelCesar = delCesar.BeginInvoke(objMainRevFinalCDWSheet, iAdjNo, null, null);
            reffdelInternal.AsyncWaitHandle.WaitOne();
            reffdelExternal.AsyncWaitHandle.WaitOne();
            reffdelCesar.AsyncWaitHandle.WaitOne();
            delInternal.EndInvoke(reffdelInternal);
            delExternal.EndInvoke(reffdelExternal);
            delCesar.EndInvoke(reffdelCesar);


            /*****************Setting Sub Reports Parameters Value Begin******************/
            VCsetInternalSubReportParameters(prmAdjNo.Value, prmFlag.Value, prmFlipSigns.Value, prmInvNo.Value, prmRevFlag.Value, prmHistFlag.Value);
            //setInternalSubReportParameters(objMainRevFinalExternal, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            VCsetExternalSubReportParameters(prmAdjNo.Value, prmFlag.Value, prmFlipSigns.Value, prmInvNo.Value, prmRevFlag.Value, prmHistFlag.Value);
            //setRevisionCDWSubReportParameters(objMainRevFinalCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmPreviousAdjNo, prmPreviousInvNo, prmHistFlag);
            setRevisionCDWSubReportParameters(objMainRevFinalCDWSheet, prmAdjNo, prmFlag, prmFlipSigns, prmInvNo, prmRevFlag, prmHistFlag);
            /*****************Setting Sub Reports Parameters Value End******************/
            string strVal1;
            string strVal2;
            string strVal3;
            string strval4;

            //Start:parallel Process-Calling Export Methods Parallely
            int intPersID = CurrentAISUser.PersonID;
            try
            {
                string strInternalFilename = "FinalInternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                string strExaternalFilename = "FinalExternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                string strCDWorkSheetFilename = "FinalCDWorkSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
                string strProgSummSpFilename = "FinalPSSpreadSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".xlsx";
                ExportExternalDelegate delInstanceExternal = new ExportExternalDelegate(ExportReportsWithTOC);
                ExportInternalDelegate delInstanceInternal = new ExportInternalDelegate(ExportReportsWithTOC);
                ExportCesarDelegate delInstanceCesar = new ExportCesarDelegate(ExportReportsWithTOC);
                ExportPorgramSummayDelegate delInstanceProgramSummary = new ExportPorgramSummayDelegate(ExportReportsWithTOC);
                IAsyncResult reffExternal = delInstanceExternal.BeginInvoke(null, strExaternalFilename, iAdjNo, 2, 'E', intPersID, null, null);
                IAsyncResult reffInternal = delInstanceInternal.BeginInvoke(null, strInternalFilename, iAdjNo, 2, 'I', intPersID, null, null);
                IAsyncResult reffCesar = delInstanceCesar.BeginInvoke(objMainRevFinalCDWSheet, strCDWorkSheetFilename, iAdjNo, 2, 'C', intPersID, null, null);
                IAsyncResult reffProgramSummary = delInstanceProgramSummary.BeginInvoke(null, strProgSummSpFilename, iAdjNo, 2, 'P', intPersID, null, null);
                reffExternal.AsyncWaitHandle.WaitOne();
                reffInternal.AsyncWaitHandle.WaitOne();
                reffCesar.AsyncWaitHandle.WaitOne();
                reffProgramSummary.AsyncWaitHandle.WaitOne();
                strVal1 = delInstanceInternal.EndInvoke(reffInternal);
                strVal2 = delInstanceExternal.EndInvoke(reffExternal);
                strVal3 = delInstanceCesar.EndInvoke(reffCesar);
                strval4 = delInstanceProgramSummary.EndInvoke(reffProgramSummary);
            }
            catch (Exception ee)
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
                return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
            }
            //try
            //{


            //    string strInternalFilename = "FinalInternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
            //    strVal1 = ExportReports(objMainRevFinlaInternal, strInternalFilename, iAdjNo, 2, 'I');

            //}
            //catch (Exception ee)
            //{
            //    if (trans != null)
            //    trans.Rollback();
            //    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
            //    return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
            //}
            //try
            //{


            //    string strExaternalFilename = "FinalExternalInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
            //    strVal2 = ExportReports(objMainRevFinalExternal, strExaternalFilename, iAdjNo, 2, 'E');

            //}
            //catch (Exception ee)
            //{
            //    if (trans != null)
            //    trans.Rollback();
            //    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
            //    return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
            //}
            //try
            //{

            //    string strCDWorkSheetFilename = "FinalCDWorkSheetInvoice_" + strInvNo + "_" + System.DateTime.Now + ".pdf";
            //    strVal3 = ExportReports(objMainRevFinalCDWSheet, strCDWorkSheetFilename, iAdjNo, 2, 'C');

            //}
            //catch (Exception ee)
            //{
            //    if (trans != null)
            //    trans.Rollback();
            //    new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ee.Message, CurrentAISUser.PersonID);
            //    return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
            //}




            //objMainRevFinlaInternal.Close();
            //objMainRevFinalExternal.Close();
            objMainRevFinalCDWSheet.Close();
            if (strVal1 != null && strVal1 != "")
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal1, CurrentAISUser.PersonID);
                return strVal1;
            }
            else if (strVal2 != null && strVal2 != "")
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal2, CurrentAISUser.PersonID);
                return strVal2;
            }
            else if (strVal3 != null && strVal3 != "")
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strVal3, CurrentAISUser.PersonID);
                return strVal3;
            }
            else if (strval4 != null && strval4 != "")
            {
                if (trans != null)
                    trans.Rollback();
                new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", strval4, CurrentAISUser.PersonID);
                return strval4;
            }
            else
            {
                objDC.SubmitChanges();
                if (trans != null)
                    trans.Commit();
                ProgramPeriodsBS prgBS = new ProgramPeriodsBS();
                prgBS.ModAISTransmittalToARiES(iAdjNo, 1);

                //to update tranmittal history status to 1 for test accounts.
                (new AccountBS()).UpdateTransmittalHistoryForTestAcct();

                prgBS.ModAISCodingToCesar(iAdjNo);

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
            if (trans != null)
                trans.Rollback();
            new ApplicationStatusLogBS().WriteLog(iAdjNo, "AIS Invoice Driver", "ERR", "Invoice Driver Error", ex.Message, CurrentAISUser.PersonID);
            return "Final Invoice could not be completed due to an error. Please check the error log for additional details";
        }
        finally
        {
            if (objDC.Connection.State == ConnectionState.Open)
                objDC.Connection.Close();
        }
        return string.Empty;
    }
    /// <summary>
    /// BPNumberpopup button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region btnBPNumber Click Event
    protected void btnBPNumber_Click(object sender, EventArgs e)
    {
        modalBPNumber.Hide();
        //string strPath = "~/AcctSetup/AcctInfo.aspx";
        //Response.Redirect(strPath);
        ResponseRedirect("~/AcctSetup/AcctInfo.aspx");
    }
    #endregion
    /// <summary>
    /// btnBPNumberClose button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region btnBPNumberClose Click Event
    protected void btnBPNumberClose_Click(object sender, EventArgs e)
    {
        modalBPNumber.Hide();
        return;

    }
    /// <summary>
    /// btnILRFOtherAmount button click event(SR 325928). 
    /// This will be called if user wnats to go with the other amounts.Calculations will be performed by considering the other amounts.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region btnILRFOtherAmount Click Event
    protected void btnILRFOtherAmount_Click(object sender, EventArgs e)
    {
        modalILRFOtherAmount.Hide();
        bool Error = false;
        string strError = this.StrErrorMessage;
        DropDownList ddlAccountlist = (DropDownList)this.ddlAcctlist.FindControl("ddlAccountlist");
        string strCalcError = (new ProgramPeriodsBS()).CalcDriver(Convert.ToInt32(ddlAccountlist.SelectedItem.Value), this.StrCalcProgramPeriods, this.StrReCalcPeriods, this.BlPLB, this.BlILRF, CurrentAISUser.PersonID, this.BlCHF);
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
        if (strCalcError != "")
        {
            bool status = (new LSICustomersBS()).CheckLSIPermissions();
            if (!status)
                new ApplicationStatusLogBS().WriteLog("AIS Calculation Engine", "ERR", "Calculation error", "LSI Interface is not responding. Please contact business support group for resolution.", Convert.ToInt32(ddlAccountlist.SelectedItem.Value), CurrentAISUser.PersonID);

        }
        ShowError(strError);
        // Code to refresh the listview once calculation is completed
        CheckBox chk;
        ArrayList alindex = this.ArrayListIndex;
        int adjno = AISMasterEntities.AdjusmentNumber;
        string str = ddlValDateList.SelectedValue != "0" ? ddlValDateList.SelectedItem.Text : string.Empty;
        AdjInvDashlistview.DataSource = (new ProgramPeriodsBS()).GetProgramPeriodSearchDashboard(Convert.ToInt32(ddlAccountlist.SelectedValue), Convert.ToInt32(ddlProgramTypeList.SelectedValue), adjno, str);
        AdjInvDashlistview.DataBind();
        hidCofirmCount.Value = "0";
        hidLSICount.Value = "0";
        hidILRFCount.Value = "0";
        hidConfirm.Value = "";
        hidConfirmCHF.Value = "";
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
                        if (lblAdjNo.Text != "")
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
    #endregion
    /// <summary>
    /// btnILRFOtherAmountClose button click event(SR 325928)
    /// This will be called if user doesn't want to proceed withe calculations.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region btnILRFOtherAmountClose Click Event
    protected void btnILRFOtherAmountClose_Click(object sender, EventArgs e)
    {
        modalILRFOtherAmount.Hide();
        return;

    }
    #endregion
    #endregion
}

