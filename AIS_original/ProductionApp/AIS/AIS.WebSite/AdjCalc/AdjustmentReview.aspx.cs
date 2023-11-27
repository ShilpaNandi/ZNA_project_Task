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

public partial class Adjustment_and_Invoice_AdjustmentReview : AISBasePage
{
    private static int intAccountID;
    private static int intPrmAdjPrdID;
    private static DateTime dtValDate;
    private static int intPrmAdjPrgID;
    private static int intPrmAdjID;
    private static string strProgramPeriod;
    protected static Common common = null;
    /// <summary>
    /// A Property for Holding AdjStatusTypeID in ViewState
    /// </summary>
    protected int AdjStatusTypeID
    {
        get
        {
            if (ViewState["AdjStatusTypeID"] != null)
            {
                return int.Parse(ViewState["AdjStatusTypeID"].ToString());
            }
            else
            {
                ViewState["AdjStatusTypeID"] = 0;
                return 0;
            }
        }
        set
        {
            ViewState["AdjStatusTypeID"] = value;
        }
    }


    #region Adjustment Review Comments Business Service Property
    /// <summary>
    /// a property for Adjustment Review Comments Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>AdjustmentReviewCommentsBS</returns>
    private AdjustmentReviewCommentsBS _AdjRevCmmntBS;
    AdjustmentReviewCommentsBS AdjRevCmmntBS
    {
        get
        {
            if (_AdjRevCmmntBS == null)
            {
                _AdjRevCmmntBS = new AdjustmentReviewCommentsBS();
               

            }
            return _AdjRevCmmntBS;
        }
    }
    #endregion

    #region Adjustment review Comment Business Entity Property
    /// <summary>
    /// a property for Adjustment review Comment Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>AdjustmentReviewCommentsBE</returns>
    private AdjustmentReviewCommentsBE AdjRevCmmntBE
    {
        get { return (AdjustmentReviewCommentsBE)Session["AdjRevCmmntBE"]; }
        set { Session["AdjRevCmmntBE"] = value; }
    }
    #endregion


    #region Page Load

    /// <summary>
    /// PageLoad Event code appears here
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>

    private string strMessage = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Eventhandlers from User control
        this.ARS.AdjustmentReviewSearchButtonClicked += new EventHandler(btnSearch_AdjustmentReviewSearchButtonClicked);
        this.ARS.ARProgramPeriodSelectedIndexChanged += new EventHandler(ddlprogramPeriod_ARProgramPeriodSelectedIndexChanged);
        // To display the title of the page
        this.Master.Page.Title = "Adjustment Review Comments";
        if (!IsPostBack)
        {
            
            this.ddlDocName.Enabled = false;
            BtnSave.Enabled = false;
            pnlDetails.Visible = false;
            try
            {
                if (Request.QueryString["SelectedValues"] != null && Request.QueryString["SelectedValues"] != "")
                {
                    string[] strSelectedValues = Request.QueryString["SelectedValues"].ToString().Split(';');
                    string strSelectedAccountID = strSelectedValues[0].ToString();
                    string strSelectedValDate = strSelectedValues[1].ToString();
                    string strSelectedPremAdjID = strSelectedValues[2].ToString();
                    string strSelectedProgramPeriod = strSelectedValues[3].ToString();
                    if (strSelectedAccountID != "")
                    {
                        ddlDocName.Enabled = true;
                    }
                    if (strSelectedAccountID != "" && strSelectedValDate != "" && strSelectedPremAdjID != "" && strSelectedProgramPeriod != "")
                        fillSelectedValues(strSelectedAccountID, strSelectedValDate, strSelectedProgramPeriod, strSelectedPremAdjID);
                    else
                        hidSelectedValues.Value = strSelectedAccountID + ";" + strSelectedValDate + ";" + strSelectedPremAdjID + ";" + strSelectedProgramPeriod;

                }
            }
            catch (Exception ee)
            {
                ShowError(ee.Message,ee);
                return;
            }
            
        }
        CheckExitWithoutSave();

    }
    private void CheckExitWithoutSave()
    {
        ArrayList list = new ArrayList();
        list.Add(txtComment);
        list.Add(BtnSave);

        ProcessExitFlag(list);
    }
    #endregion

    #region Search Button
    //Search Button Functionality
    void btnSearch_AdjustmentReviewSearchButtonClicked(object sender, EventArgs e)
    {
        
       
        Search();
        if (Search())
        {
            txtComment.Text = string.Empty;
            pnlDetails.Visible = true;
        }
        if (ddlDocName.SelectedItem.Text == "Cumulative Totals" || 
            ddlDocName.SelectedItem.Text == "ARiES Posting Details" || 
            ddlDocName.SelectedItem.Text == "CESAR Coding Worksheet" ||
            ddlDocName.SelectedItem.Text == "NY-SIF" ||
            ddlDocName.SelectedItem.Text == "RML" ||
            ddlDocName.SelectedItem.Text == "Retrospective Premium Adjustment" ||
            ddlDocName.SelectedItem.Text == "Cesar Coding PDF" ||
            ddlDocName.SelectedItem.Text == "External PDF" ||
            ddlDocName.SelectedItem.Text == "Internal PDF")
        {
            pnlDetails.Visible = false;
        }
        int intAdjFinalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.FinalInvd, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
        int intAdjTransmittedStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Transmitted, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry )
        {
            if (AdjStatusTypeID == intAdjTransmittedStatusID || AdjStatusTypeID == intAdjFinalStatusID)
            {
                BtnSave.Enabled = false;
            }
            else
            {
                BtnSave.Enabled = true;
            }
        }
        if (strMessage != String.Empty)
        {
            
        }
        else
        {
            int intdocNameID = Convert.ToInt32(ddlDocName.SelectedValue);

            DocType(intdocNameID);
            if (AdjRevCmmntBE.PREM_ADJ_CMMNT_ID > 0)
            {
                ViewState["Flag"] = true;
            }
            else
            {
                ViewState["Flag"] = null;
            }
            txtComment.Text = AdjRevCmmntBE.CMMNT_TXT;
        }
       

    }

    #endregion
    private void fillSelectedValues(string strSelectedAccountID, string strSelectedValDate, string strSelectedProgramPeriod, string strSelectedPremAdjID)
    {
       
        if (strSelectedAccountID != "")
            intAccountID = Convert.ToInt32(strSelectedAccountID);
        if (strSelectedValDate != "")
            dtValDate = Convert.ToDateTime(strSelectedValDate);
        strProgramPeriod = strSelectedProgramPeriod;
        if (strSelectedProgramPeriod != "")
            intPrmAdjPrgID = Convert.ToInt32(strSelectedProgramPeriod);
       
       
        //Based on Account Id and Valuation Date  retreive Premium Adjustment ID
        intPrmAdjID = 0;
        if (strSelectedPremAdjID != "")
            intPrmAdjID = Convert.ToInt32(strSelectedPremAdjID);
        hidSelectedValues.Value = intAccountID + ";" + dtValDate.ToShortDateString() + ";" + intPrmAdjID + ";" + strProgramPeriod;
       
        //Based on Account Id,Premium Adj Id and Premium Adj Pgm Id retreive Premium Adj Period ID
        IList<PremiumAdjustmentPeriodBE> premAdjPerdBE = new List<PremiumAdjustmentPeriodBE>();
        PremiumAdjustmentPeriodBS PrmAdjPerd = new PremiumAdjustmentPeriodBS();
        premAdjPerdBE = PrmAdjPerd.getPremAdjPerdID(intAccountID, intPrmAdjID, intPrmAdjPrgID);
        intPrmAdjPrdID = 0;
        if (premAdjPerdBE.Count > 0)
        {
            intPrmAdjPrdID = premAdjPerdBE[premAdjPerdBE.Count-1].PREM_ADJ_PERD_ID;
        }
        //Retrieving the Status Type ID of the adjustment Selected
        PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
        IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;

        objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(intPrmAdjID);
        objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
        this.AdjStatusTypeID = objPremStsBE.ADJ_STS_TYP_ID != null ? Convert.ToInt32(objPremStsBE.ADJ_STS_TYP_ID) : 0;
       
        ddlDocName.Enabled = true;
    }
    #region Search Functionality
    private bool Search()
    {
        //string strMessage = string.Empty;
        DropDownList ddlAccnts = (DropDownList)this.ARS.FindControl("ddlAccountlist");
        DropDownList ddlValDates = (DropDownList)this.ARS.FindControl("ddlValDate");
        DropDownList ddlPrgPeriod = (DropDownList)this.ARS.FindControl("ddlProgramPeriod");
        DropDownList ddlAdjNumber = (DropDownList)this.ARS.FindControl("ddlAdjNumber");
        if (ddlAccnts.SelectedIndex == 0)
        {
            strMessage = "Please select Account name";
            ShowError(strMessage);
        }
        else if (ddlValDates.SelectedIndex == 0)
        {
            strMessage = "Please select Valuation Date";
            ShowError(strMessage);
        }
        else if (ddlAdjNumber.SelectedIndex == 0)
        {
            strMessage = "Please select Adjustment Number";
            ShowError(strMessage);

        }
        else if (ddlPrgPeriod.SelectedIndex == 0)
        {
            strMessage = "Please select Program Period";
            ShowError(strMessage);
        }
        
        else if (ddlDocName.SelectedIndex == 0)
        {
            strMessage = "Please select Document Name";
            ShowError(strMessage);
        }
        if (strMessage != String.Empty)
        {
            return false;
        }
       

        intAccountID = Convert.ToInt32(ddlAccnts.SelectedValue);
        dtValDate = Convert.ToDateTime(ddlValDates.SelectedItem.ToString());
        strProgramPeriod = ddlPrgPeriod.SelectedValue.ToString();
        intPrmAdjPrgID = Convert.ToInt32(strProgramPeriod);
       
        //Based on Account Id and Valuation Date  retreive Premium Adjustment ID
        intPrmAdjID = 0;
        intPrmAdjID = Convert.ToInt32(ddlAdjNumber.SelectedValue);
        hidSelectedValues.Value = intAccountID + ";" + dtValDate.ToShortDateString() + ";" + intPrmAdjID + ";" + strProgramPeriod;
        
        //Based on Account Id,Premium Adj Id and Premium Adj Pgm Id retreive Premium Adj Period ID
        IList<PremiumAdjustmentPeriodBE> premAdjPerdBE = new List<PremiumAdjustmentPeriodBE>();
        PremiumAdjustmentPeriodBS PrmAdjPerd = new PremiumAdjustmentPeriodBS();
        premAdjPerdBE = PrmAdjPerd.getPremAdjPerdID(intAccountID, intPrmAdjID, intPrmAdjPrgID);
        intPrmAdjPrdID = 0;
        if (premAdjPerdBE.Count > 0)
        {
            intPrmAdjPrdID = premAdjPerdBE[premAdjPerdBE.Count-1].PREM_ADJ_PERD_ID;
        }
        //Retrieving the Status Type ID of the adjustment Selected
        PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
        IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;

        objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(intPrmAdjID);
        objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
        this.AdjStatusTypeID = objPremStsBE.ADJ_STS_TYP_ID != null ? Convert.ToInt32(objPremStsBE.ADJ_STS_TYP_ID) : 0;
       
        return true;
    }
    #endregion

    #region DocType
    private void DocType(int intdocNameID)
    {
        string docType = ddlDocName.SelectedItem.Text;
        switch (docType.Trim())
        {

            case "INVOICE": SearchPrmAdj(intdocNameID, intPrmAdjID, intAccountID); //prem_Adj Table
                break;
            case "CHF":
            case "ESCROW":
            case "ILRF":
            case "LBA":
            case "COMBINED ELEMENTS":
            case "NY-SIF":
            case "WA":
            case "RML": SearchPrmAdjPrd(intdocNameID, intPrmAdjPrdID, intAccountID);//Prem_adj_period table
                break;
            default: SearchPrmAdjPrd(intdocNameID, intPrmAdjPrdID, intAccountID);//Prem_adj_period table
                break;


        }
    }
    #endregion
    #region Search methods
    private void SearchPrmAdj(int commentID, int PrgAdjID, int custmrID)
    {
        AdjRevCmmntBE = AdjRevCmmntBS.getAdjReviewCmmntINVOICEData(commentID, PrgAdjID, custmrID);

    }
    private void SearchPrmAdjPrd(int commentID, int PrgPrdID, int custmrID)
    {
        AdjRevCmmntBE = AdjRevCmmntBS.getAdjReviewCmmntALLData(commentID, PrgPrdID, custmrID);

    }
    
    #endregion

    #region Program Period Selected Index Change
    //Program Period Selected Index Change
    void ddlprogramPeriod_ARProgramPeriodSelectedIndexChanged(object sender, EventArgs e)
    {
        pnlDetails.Visible = false; 
        DropDownList ddlAccnts = (DropDownList)this.ARS.FindControl("ddlAccountlist");
        DropDownList ddlValDates = (DropDownList)this.ARS.FindControl("ddlValDate");
        DropDownList ddlPrgPeriod = (DropDownList)this.ARS.FindControl("ddlProgramPeriod");
        DropDownList ddlAdjNumber = (DropDownList)this.ARS.FindControl("ddlAdjNumber");

        if (ddlAccnts.SelectedIndex != 0)
        {
            
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ";" + ";";
        }
        else
        {
            this.ddlDocName.Enabled = false;
            this.ddlDocName.SelectedIndex = 0;
            hidSelectedValues.Value = "";
        }
        if (ddlValDates.SelectedIndex != 0)
        {
            
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";" + ";";
        }
        else if (ddlAccnts.SelectedIndex != 0)
        {
            this.ddlDocName.Enabled = false;
            this.ddlDocName.SelectedIndex = 0;
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ";" + ";";
        }
        if (ddlAdjNumber.SelectedIndex != 0)
        {
            ddlDocName.Enabled = true;
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";" + ddlAdjNumber.SelectedValue + ";";
        }
        else if (ddlValDates.SelectedIndex != 0)
        {
            this.ddlDocName.Enabled = false;
            this.ddlDocName.SelectedIndex = 0;
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";"+ ";";
        }
        if (ddlPrgPeriod.SelectedIndex != 0)
        {
            ddlDocName.Enabled = true;
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";" + ddlAdjNumber.SelectedValue + ";" + ddlPrgPeriod.SelectedValue;
        }
        else if (ddlAdjNumber.SelectedIndex != 0)
        {
            
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";" + ddlAdjNumber.SelectedValue + ";";
        }
       
        
    }
    #endregion

    #region Document Name Selected Index Change
    //Document Name Selected Index Change
    protected void ddlDocName_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtComment.Text = string.Empty;
        BtnSave.Enabled = false;
        pnlDetails.Visible = false; 
    }
    #endregion

    #region Save Button
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        
        EntitySave();
        //txtComment.Text = string.Empty;
        //BtnSave.Enabled = false;
        ViewState["Flag"] = true;

    }
    #endregion

    #region Preview Button
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        BtnSave.Enabled = false;
        
        DropDownList ddlAccnts = (DropDownList)this.ARS.FindControl("ddlAccountlist");
        DropDownList ddlValDates = (DropDownList)this.ARS.FindControl("ddlValDate");
        DropDownList ddlPrgPeriod = (DropDownList)this.ARS.FindControl("ddlProgramPeriod");
        DropDownList ddlAdjNumber = (DropDownList)this.ARS.FindControl("ddlAdjNumber");
        if (ddlAccnts.SelectedIndex == 0)
        {
            strMessage = "Please select Account name";
            ShowError(strMessage);
        }
        else if (ddlValDates.SelectedIndex == 0)
        {
            strMessage = "Please select Val. Date";
            ShowError(strMessage);
        }
        else if (ddlAdjNumber.SelectedIndex == 0)
        {
            strMessage = "Please select Adjustment Number";
            ShowError(strMessage);

        }
        else if (ddlDocName.SelectedIndex == 0)
        {
            strMessage = "Please select Document Name";
            ShowError(strMessage);
        }
        if (strMessage.Trim().Length > 0)
            return;

        intAccountID = Convert.ToInt32(ddlAccnts.SelectedValue);
        dtValDate = Convert.ToDateTime(ddlValDates.SelectedItem.ToString());
        intPrmAdjID = Convert.ToInt32(ddlAdjNumber.SelectedValue);
        if (ddlPrgPeriod.SelectedIndex != 0)
        {
            hidSelectedValues.Value = intAccountID + ";" + dtValDate.ToShortDateString() + ";" + intPrmAdjID + ";" + ddlPrgPeriod.SelectedValue;
        }
        else
        {
            hidSelectedValues.Value = intAccountID + ";" + dtValDate.ToShortDateString() + ";" + intPrmAdjID + ";";
        }
        string docName = ddlDocName.SelectedItem.Text.Trim().ToUpper();
        if (docName != "INTERNAL PDF" && docName != "EXTERNAL PDF" && docName != "CESAR CODING PDF")
        {
            if (!AdjRevCmmntBS.IsReportAvialable(intPrmAdjID, 1, docName))
            {
                ShowError("Report is not available to Preview");
                return;
            }
        
        }
            switch (docName)
            {

                case "SUMMARY INVOICE":
                    DownloadFile(intPrmAdjID, "INVOICE");
                    break;
                case "CHF":
                    DownloadFile(intPrmAdjID, "CHF");
                    break;
                case "ESCROW":
                    DownloadFile(intPrmAdjID, "ESCROW");
                    break;
                case "ILRF (INTERNAL)":
                    DownloadFile(intPrmAdjID, "ILRFINTERNAL");
                    break;
                case "ILRF (EXTERNAL)":
                    DownloadFile(intPrmAdjID, "ILRFEXTERNAL");
                    break;
                case "LBA":
                    DownloadFile(intPrmAdjID, "LBA");
                    break;
                case "COMBINED ELEMENTS":
                    DownloadFile(intPrmAdjID, "COMBINED ELEMENTS");
                    break;
                case "NY-SIF":
                    DownloadFile(intPrmAdjID, "NY-SIF");
                    break;
                case "WA":
                    DownloadFile(intPrmAdjID, "WA");
                    break;
                case "RML":
                    DownloadFile(intPrmAdjID, "RML");
                    break;
                case "EXCESS LOSS":
                    DownloadFile(intPrmAdjID, "EXCESS LOSS");
                    break;
                case "RETROSPECTIVE PREMIUM ADJUSTMENT":
                    DownloadFile(intPrmAdjID, "RETROSPECTIVE PREMIUM ADJUSTMENT");
                    break;
                case "RETROSPECTIVE ADJUSTMENT LEGEND":
                    DownloadFile(intPrmAdjID, "RETROSPECTIVE ADJUSTMENT LEGEND");
                    break;
                case "BROKER LETTER":
                    DownloadFile(intPrmAdjID, "BROKER LETTER");
                    break;
                case "KY & OR TAXES":
                    DownloadFile(intPrmAdjID, "KYOR");
                    break;
                case "PROCESSING CHECKLIST":
                    DownloadFile(intPrmAdjID, "PROCESSING CHECKLIST");
                    break;
                case "ARIES POSTING DETAILS":
                    DownloadFile(intPrmAdjID, "ARIES POSTING DETAILS");
                    break;
                case "CESAR CODING WORKSHEET":
                    DownloadFile(intPrmAdjID, "CESAR CODING WORKSHEET");
                    break;
                case "CUMULATIVE TOTALS":
                    DownloadFile(intPrmAdjID, "CUMULATIVE TOTALS");
                    break;
                case "INTERNAL PDF":
                    DownloadFile(intPrmAdjID, "INTERNAL PDF");
                    break;
                case "EXTERNAL PDF":
                    DownloadFile(intPrmAdjID, "EXTERNAL PDF");
                    break;
                case "CESAR CODING PDF":
                    DownloadFile(intPrmAdjID, "CESAR CODING PDF");
                    break;
                default:
                    ShowError("Report is not available to Preview");
                    return;
            }
        
        
    }


    #endregion


    protected void EntitySave()
    {
        
        if (ViewState["Flag"] == null)
        {
            AdjRevCmmntBE = AdjRevCmmntBS.getAdjReviewCmmntALLData(Convert.ToInt32(ddlDocName.SelectedValue), intPrmAdjPrdID, intAccountID);
            if (AdjRevCmmntBE.CMMNT_CATG_ID == null)
            {
                AdjRevCmmntBE = new AdjustmentReviewCommentsBE();
                AdjRevCmmntBE.CREATEUSER = CurrentAISUser.PersonID;
                AdjRevCmmntBE.CREATEDATE = DateTime.Now;
                string docType = ddlDocName.SelectedItem.Text;
                AdjRevCmmntBE.PREM_ADJ_ID = intPrmAdjID;
                AdjRevCmmntBE.PREM_ADJ_PERD_ID = intPrmAdjPrdID;
                AdjRevCmmntBE.CUSTMR_ID = intAccountID;
                AdjRevCmmntBE.CMMNT_CATG_ID = Convert.ToInt32(ddlDocName.SelectedValue);
                AdjRevCmmntBE.CMMNT_TXT = txtComment.Text;
                AdjRevCmmntBS.Update(AdjRevCmmntBE);
                AdjRevCmmntBE = AdjRevCmmntBS.getAdjReviewCmmntALLData(Convert.ToInt32(ddlDocName.SelectedValue), intPrmAdjPrdID, intAccountID);
            }
            else
            {
                ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
                return;
            }
        }
        else
        {
             AdjustmentReviewCommentsBE AdjRevCmmntBEold = AdjRevCmmntBS.getAdjustmentReviewCommentsRow(AdjRevCmmntBE.PREM_ADJ_CMMNT_ID);
            bool con = ShowConcurrentConflict(Convert.ToDateTime(AdjRevCmmntBEold.UPDATEDDATE), Convert.ToDateTime(AdjRevCmmntBE.UPDATEDDATE));
            if (!con)
                return;
            AdjRevCmmntBEold.UPDATEDUSER = CurrentAISUser.PersonID;
            AdjRevCmmntBEold.UPDATEDDATE = DateTime.Now;
            string docType = ddlDocName.SelectedItem.Text;
            AdjRevCmmntBEold.PREM_ADJ_ID = intPrmAdjID;
            AdjRevCmmntBEold.PREM_ADJ_PERD_ID = intPrmAdjPrdID;
            AdjRevCmmntBEold.CUSTMR_ID = intAccountID;
            AdjRevCmmntBEold.CMMNT_CATG_ID = Convert.ToInt32(ddlDocName.SelectedValue);
            AdjRevCmmntBEold.CMMNT_TXT = txtComment.Text;
            AdjRevCmmntBS.Update(AdjRevCmmntBEold);
            AdjRevCmmntBE = AdjRevCmmntBS.getAdjReviewCmmntALLData(Convert.ToInt32(ddlDocName.SelectedValue), intPrmAdjPrdID, intAccountID);
           
        }
        
       


    }

    

    /// <summary>
    /// Method to open Preview document
    /// </summary>
    /// <param name="AdjNo"></param>
    /// <param name="DocName"></param>
    #region DownloadFile
    public void DownloadFile(int AdjNo, string DocName)
    {
        string Url = this.Page.ResolveUrl("~/AdjCalc/PreviewInvoice.aspx") + "?DocName=" + DocName + "&AdjNo=" + AdjNo;
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "key", "<script language='javascript'>window.open('" + Url + "', null);</script>", false);
        //if (DocName.Trim().ToUpper() == "WA")
        //{
        //    ShowError("Report is in progress for " + DocName);
        //    return;
        //}
        //PremiumAdjustmentBE premAdjBE = new PremAdjustmentBS().getPremiumAdjustmentRow(AdjNo);
        //string strInvNo = string.Empty;
        ////if (premAdjBE.FNL_INVC_NBR_TXT != "")
        ////{
        ////    strInvNo = premAdjBE.FNL_INVC_NBR_TXT;
        ////}
        //if (premAdjBE.INVC_NBR_TXT != "" && premAdjBE.INVC_NBR_TXT != null)
        //{
        //    strInvNo = premAdjBE.INVC_NBR_TXT;
        //}
        //bool HistFlag = false;
        //if (premAdjBE.HISTORICAL_ADJ_IND.HasValue)
        //{
        //    HistFlag = Convert.ToBoolean(premAdjBE.HISTORICAL_ADJ_IND);
        //}
        //Preview(AdjNo, strInvNo, HistFlag, DocName);
        //hidSelectedValues.Value = intAccountID + ";" + dtValDate.ToShortDateString() + ";" + strProgramPeriod + ";" + intPrmAdjID;

    }
    #endregion

    public void Preview(int iAdjNo, string strInvNo, bool HistFlag, string strDocumentName)
    {

        switch (strDocumentName.Trim().ToUpper())
        {
            case "ESCROW":
                //Escrow Fund Adjustment
                GenerateReport(iAdjNo, strInvNo, HistFlag, "rptEscrowMasterReport.rpt", "srptEscrowFundAdjustment.rpt", "SuppressEscrow", strDocumentName);
                break;
            case "ILRFINTERNAL":
                //Loss Reimbursement Fund Adjustment-Internal
                GenerateReport(iAdjNo, strInvNo, HistFlag, "rptILRFInternalMasterReport.rpt", "srptLRFInternal.rpt", "SuppressLRFInternal", strDocumentName);
                break;
            case "ILRFEXTERNAL":
                //Loss Reimbursement Fund Adjustment-External
                GenerateReport(iAdjNo, strInvNo, HistFlag, "rptILRFExternalMasterReport.rpt", "srptLRFExternal.rpt", "SuppressLRFExternal", strDocumentName);
                break;
            case "INVOICE":
                //Adjustment Invoice Exhibit
                GenerateReport(iAdjNo, strInvNo, HistFlag, "rptInvoiceMasterReport.rpt", "srptAdjusInvoice.rpt", "SuppressSectionAdjInv", strDocumentName);
                break;
            case "LBA":
                //Loss Based Assessment Exhibit
                GenerateReport(iAdjNo, strInvNo, HistFlag, "rptLBAMasterReport.rpt", "srptLossBasedAssessmentsCalculation.rpt", "SuppressLBA", strDocumentName);
                break;
            case "CHF":
                //Claim Handling Fee
                GenerateReport(iAdjNo, strInvNo, HistFlag, "rptCHFMasterReport.rpt", "srptClaimHandlingFeeSummary.rpt", "", strDocumentName);
                break;
            case "COMBINED ELEMENTS":
                //Combined Elements Exhibit - Internal
                GenerateReport(iAdjNo, strInvNo, HistFlag, "rptCombinedElementsMasterReport.rpt", "srptCombinedElements.rpt", "SuppressCombinedElements", strDocumentName);
                break;
            case "NY-SIF":
                //Adjustment of NY Second Injury Fund
                GenerateReport(iAdjNo, strInvNo, HistFlag, "rptNYSIFMasterReport.rpt", "srptAdjNYSecInjFund.rpt", "SuppressAdjNY", strDocumentName);
                break;
            case "RML":
                //Residual Market Subsidy Charge
                GenerateReport(iAdjNo, strInvNo, HistFlag, "rptRMLMasterReport.rpt", "srptResidualMarkSubCharge.rpt", "SuppressResidualMarkSubChange", strDocumentName);
                break;
            case "EXCESS LOSS":
                //Excess Loss
                GenerateReport(iAdjNo, strInvNo, HistFlag, "rptExcessMasterReport.rpt", "srptExcessLoss.rpt", "SuppressExcessLoss", strDocumentName);
                break;
            case "WA":
                //need clarifications
                break;
            //Retrospective Premium Adjustment
            case "RETROSPECTIVE PREMIUM ADJUSTMENT":
                GenerateReport(iAdjNo, strInvNo, HistFlag, "rptRetroMasterReport.rpt", "srptRetroPremAdjustmentMain.rpt", "SuppressRetroPremAdj", strDocumentName);
                break;
            //Retrospective Adjustment Legend
            case "RETROSPECTIVE ADJUSTMENT LEGEND":
                GenerateReport(iAdjNo, strInvNo, HistFlag, "rptRetroLegendMasterReport.rpt", "srptRetroPremAdjLegend.rpt", "SuppressRetroLegend", strDocumentName);
                break;
            //Broker Letter
            case "BROKER LETTER":
                GenerateReport(iAdjNo, strInvNo, HistFlag, "rptBrokerMasterReport.rpt", "srptBrokerLetter.rpt", "SuppressBrokerLetter", strDocumentName);
                break;
            //KY & OR TAXES
            case "KYOR":
                GenerateReport(iAdjNo, strInvNo, HistFlag, "rptKYORMasterReport.rpt", "srptWorkerCompTaxAssessKentOreg.rpt", "SuppressKYOR", strDocumentName);
                break;
            //Processing Checklist
            case "PROCESSING CHECKLIST":
                GenerateReport(iAdjNo, strInvNo, HistFlag, "rptProcessChecklistRetroMasterReport.rpt", "srptProcessingCheckList.rpt", "SuppressProcessingCheckList", strDocumentName);
                break;
            //Aries Posting Details
            case "ARIES POSTING DETAILS":
                GenerateReport(iAdjNo, strInvNo, HistFlag, "rptARiESMasterReport.rpt", "srptARiEsPostingDetails.rpt", "SuppressAries", strDocumentName);
                break;
            //Cesar coding Worksheet
            case "CESAR CODING WORKSHEET":
                GenerateReport(iAdjNo, strInvNo, HistFlag, "rptCesarCodingMasterReport.rpt", "srptCesarCodingWorksheet.rpt", "SuppressCesar", strDocumentName);
                break;
            case "CUMULATIVE TOTALS":
                GenerateReport(iAdjNo, strInvNo, HistFlag, "rptCumulativeMasterReport.rpt", "srptCumTotalsWorksheet.rpt", "SuppressCumTotals", strDocumentName);
                break;
        }

    }

    private void GenerateReport(int iAdjNo, string strInvNo, bool HistFlag, string strMasterReport, string strSubReport, string strSubReportParameter, string strDocName)
    {
        ReportDocument objMainPreview = new ReportDocument();
        //rptMasterReport objMainPreview = new rptMasterReport();
        objMainPreview.Load(Server.MapPath("\\Reports\\" + strMasterReport));
        //Pdf Connections
        GenerateReportConnections(objMainPreview);

        objMainPreview.VerifyDatabase();

        ParameterDiscreteValue prmAdjNo = new ParameterDiscreteValue();
        ParameterDiscreteValue prmFlag = new ParameterDiscreteValue();
        ParameterDiscreteValue prmERPInd = new ParameterDiscreteValue();
        ParameterDiscreteValue prmRevFlag = new ParameterDiscreteValue();
        ParameterDiscreteValue prmInvNo = new ParameterDiscreteValue();
        ParameterDiscreteValue prmHistFlag = new ParameterDiscreteValue();
        prmAdjNo.Value = iAdjNo;
        prmFlag.Value = 1;
        prmERPInd.Value = false;//This is dummy variable
        prmRevFlag.Value = false;
        prmInvNo.Value = strInvNo;
        prmHistFlag.Value = HistFlag;
        /*****************Setting Master Report Parameters Value Begin******************/
        objMainPreview.SetParameterValue("@ADJNO", prmAdjNo);
        objMainPreview.SetParameterValue("@FLAG", prmFlag);
        if (strSubReportParameter != "")
            objMainPreview.SetParameterValue(strSubReportParameter, "View");
        /*****************Setting Master Report Parameters Value Begin******************/

        /*****************Setting Sub Reports Parameters Value Begin******************/
        objMainPreview.SetParameterValue("@ADJNO", prmAdjNo, strSubReport);
        objMainPreview.SetParameterValue("@FLAG", prmFlag, strSubReport);
        objMainPreview.SetParameterValue("@HISTFLAG", prmHistFlag, strSubReport);
        if (strDocName.ToUpper() != "BROKER LETTER" && strDocName.ToUpper() != "RETROSPECTIVE ADJUSTMENT LEGEND")
        {
            objMainPreview.SetParameterValue("@INVNO", prmInvNo, strSubReport);
        }
        if (strDocName.ToUpper() == "NY-SIF" || strDocName.ToUpper() == "KYOR")
        {
            objMainPreview.SetParameterValue("@REVFLAGPREV", prmRevFlag, strSubReport);
        }
        if (strDocName.ToUpper() == "CESAR CODING WORKSHEET" || strDocName.ToUpper() == "RML")
        {
            objMainPreview.SetParameterValue("IsFlipSigns", prmRevFlag, strSubReport);
        }
        if (strDocName.ToUpper() == "ILRFINTERNAL")
        {
            objMainPreview.SetParameterValue("@CMTCATGID", 318, strSubReport);
        }
        if (strDocName.ToUpper() == "ILRFEXTERNAL")
        {
            objMainPreview.SetParameterValue("@CMTCATGID", 375, strSubReport);
        }
        if (strDocName.ToUpper() == "BROKER LETTER")
        {
            objMainPreview.SetParameterValue("@CMTCATGID", 339, strSubReport);
        }
        if (strDocName.ToUpper() == "INVOICE")
        {
            objMainPreview.SetParameterValue("@CMTCATGID", 319, strSubReport);
        }
        if (strDocName.ToUpper() == "COMBINED ELEMENTS")
        {
            objMainPreview.SetParameterValue("@ADJNO", prmAdjNo, "srptadjinvCombinedElements.rpt");
            objMainPreview.SetParameterValue("@FLAG", prmFlag, "srptadjinvCombinedElements.rpt");
            objMainPreview.SetParameterValue("@HISTFLAG", prmHistFlag, "srptadjinvCombinedElements.rpt");
        }
        if (strDocName.ToUpper() == "RETROSPECTIVE PREMIUM ADJUSTMENT")
        {
            objMainPreview.SetParameterValue("SuppressRetroPremAdj", "View");
            objMainPreview.SetParameterValue("@ADJNO", prmAdjNo, "srptRetroPremAdjustmentMain.rpt");
            objMainPreview.SetParameterValue("@FLAG", prmFlag, "srptRetroPremAdjustmentMain.rpt");
            objMainPreview.SetParameterValue("@INVNO", prmInvNo, "srptRetroPremAdjustmentMain.rpt");
            objMainPreview.SetParameterValue("@HISTFLAG", prmHistFlag, "srptRetroPremAdjustmentMain.rpt");
        }
        if (strDocName.ToUpper() == "RETROSPECTIVE ADJUSTMENT LEGEND")
        {
            objMainPreview.SetParameterValue("SuppressRetroLegend", "View");
            objMainPreview.SetParameterValue("@ADJNO", prmAdjNo, "srptRetroPremAdjLegend.rpt");
            objMainPreview.SetParameterValue("@FLAG", prmFlag, "srptRetroPremAdjLegend.rpt");
            objMainPreview.SetParameterValue("@INVNO", prmInvNo, "srptRetroPremAdjLegend.rpt");
            objMainPreview.SetParameterValue("@HISTFLAG", prmHistFlag, "srptRetroPremAdjLegend.rpt");
            objMainPreview.SetParameterValue("@ADJNO", prmAdjNo, "srptCalcInfoLegend.rpt");
            objMainPreview.SetParameterValue("@FLAG", prmFlag, "srptCalcInfoLegend.rpt");
            objMainPreview.SetParameterValue("@HISTFLAG", prmHistFlag, "srptCalcInfoLegend.rpt");
            objMainPreview.SetParameterValue("@INVNO", prmInvNo, "srptCalcInfoLegend.rpt");
            objMainPreview.SetParameterValue("@ADJNO", prmAdjNo, "srptLegendSecond.rpt");
            objMainPreview.SetParameterValue("@FLAG", prmFlag, "srptLegendSecond.rpt");
            objMainPreview.SetParameterValue("@HISTFLAG", prmHistFlag, "srptLegendSecond.rpt");
            objMainPreview.SetParameterValue("@INVNO", prmInvNo, "srptLegendSecond.rpt");
        }
        /*****************Setting Sub Reports Parameters Value End******************/
        MemoryStream memStream;
        memStream = (MemoryStream)objMainPreview.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        try
        {
            byte[] byteArray = memStream.ToArray();
            string strFileName = strDocName + ".pdf";
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            Response.AddHeader("content-length", memStream.Length.ToString());
            Response.BinaryWrite(byteArray);
            memStream.Close();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
            return;
        }
        Response.Flush();
        Response.End();
        objMainPreview.Close();

    }

    private void GenerateReportConnections(ReportDocument objMain)
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
}

