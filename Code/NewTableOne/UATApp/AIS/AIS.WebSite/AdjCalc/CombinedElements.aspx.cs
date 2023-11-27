//Default namespaces in AdjCalc_CombinedElements screen
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
//Importing different AIS framework namespaces for AdjCalc_CombinedElements screen
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.ExceptionHandling;


/// <summary>
/// This is the class which is used to give the information about the AdjCalc_CombinedElements and it 
/// also inherits AISBase Page,so that some of the common functionality needed for all pages is implemented in
/// the Basepage
/// </summary>
#region AdjCalc_CombinedElements Class
public partial class AdjCalc_CombinedElements : AISBasePage
{
    private BLAccess CEblaccess = new BLAccess();
    /// <summary>
    /// Page load Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        //User Control Event Handlers
        this.ARS.AdjustmentReviewSearchButtonClicked += new EventHandler(btnSearch_AdjustmentReviewSearchButtonClicked);
        this.ARS.ARProgramPeriodSelectedIndexChanged += new EventHandler(ddlprogramPeriod_ARProgramPeriodSelectedIndexChanged);
        // To display the title of the page
        this.Master.Page.Title = "Combined Elements";
        if (!IsPostBack)
        {
            ChangeListState(false);
            try
            {
                //verification for QueryString to implement persistance
                if (Request.QueryString["SelectedValues"] != null && Request.QueryString["SelectedValues"] != "")
                {
                    string[] strSelectedValues = Request.QueryString["SelectedValues"].ToString().Split(';');
                    string strSelectedAccountID = strSelectedValues[0].ToString();
                    string strSelectedValDate = strSelectedValues[1].ToString();
                    string strSelectedPremAdjID = strSelectedValues[2].ToString();
                    string strSelectedProgramPeriod = strSelectedValues[3].ToString();
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
    
    #endregion

    #region Properties Declaration Section
    /// <summary>
    /// A Property for Holding CustomerID in ViewState
    /// </summary>
    protected int AccountID
    {
        get
        {
            if (ViewState["AccountID"] != null)
            {
                return int.Parse(ViewState["AccountID"].ToString());
            }
            else
            {
                ViewState["AccountID"] = 0;
                return 0;
            }
        }
        set
        {
            ViewState["AccountID"] = value;
        }
    }
    /// <summary>
    /// A Property for Holding PremAdjID in ViewState
    /// </summary>
    protected int PreAdjID
    {
        get
        {
            if (ViewState["PreAdjID"] != null)
            {
                return int.Parse(ViewState["PreAdjID"].ToString());
            }
            else
            {
                ViewState["PreAdjID"] = 0;
                return 0;
            }
        }
        set
        {
            ViewState["PreAdjID"] = value;
        }
    }
    /// <summary>
    /// A Property for Holding PremAdjProgramID in ViewState
    /// </summary>
    protected int PrgPeriodID
    {
        get
        {
            if (ViewState["PrgPeriodID"] != null)
            {
                return int.Parse(ViewState["PrgPeriodID"].ToString());
            }
            else
            {
                ViewState["PrgPeriodID"] = 0;
                return 0;
            }
        }
        set
        {
            ViewState["PrgPeriodID"] = value;
        }
    }
    /// <summary>
    /// A Property for Holding PremAdjPeriodID in ViewState
    /// </summary>
    protected int PremAdjPerdID
    {
        get
        {
            if (ViewState["PremAdjPerdID"] != null)
            {
                return int.Parse(ViewState["PremAdjPerdID"].ToString());
            }
            else
            {
                ViewState["PremAdjPerdID"] = 0;
                return 0;
            }
        }
        set
        {
            ViewState["PremAdjPerdID"] = value;
        }
    }
    /// <summary>
    /// A Property for Holding PreAdjCombElemntID in ViewState
    /// </summary>
    protected int PreAdjCombElemntID
    {
        get
        {
            if (ViewState["PreAdjCombElemntID"] != null)
            {
                return int.Parse(ViewState["PreAdjCombElemntID"].ToString());
            }
            else
            {
                ViewState["PreAdjCombElemntID"] = 0;
                return 0;
            }
        }
        set
        {
            ViewState["PreAdjCombElemntID"] = value;
        }
    }
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
    /// <summary>
    /// a property for PremiumAdjustmentPeriod Business Service Class
    /// </summary>
    /// <returns>PremiumAdjustmentPeriodBS</returns>
    private PremiumAdjustmentPeriodBS prmAdjPerd;
    private PremiumAdjustmentPeriodBS PrmAdjPerd
    {
        get
        {
            if (prmAdjPerd == null)
            {
                prmAdjPerd = new PremiumAdjustmentPeriodBS();
            }
            return prmAdjPerd;
        }
    }
    /// <summary>
    /// a property for CombinedElements Business Service Class
    /// </summary>
    /// <returns>CombinedElementsBS</returns>
    private CombinedElementsBS combeleBS;
    private CombinedElementsBS CombeleBS
    {
        get
        {
            if (combeleBS == null)
            {
                combeleBS = new CombinedElementsBS();
            }
            return combeleBS;
        }
    }
    /// <summary>
    /// a property for PremiumAdjCombElements Business Service Class
    /// </summary>
    /// <returns>PremiumAdjCombElementsBS</returns>
    private PremiumAdjCombElementsBS prmAdjCombElementsBS;
    private PremiumAdjCombElementsBS PrmAdjCombElementsBS
    {
        get
        {
            if (prmAdjCombElementsBS == null)
            {
                prmAdjCombElementsBS = new PremiumAdjCombElementsBS();
            }
            return prmAdjCombElementsBS;
        }
    }
    #endregion

    /// <summary>
    /// Method for showing any changes in the data entry fields
    /// in order to show the warning message
    /// </summary>
    #region CheckExitWithoutSave
    private void CheckExitWithoutSave()
    {
        ArrayList list = new ArrayList();
        list.Add(txtAmountsBilledMaximum);
        list.Add(txtBasicPremium);
        list.Add(txtCalculatedTotal);
        list.Add(txtDeductibleSubTotal);
        list.Add(txtLossALAELBALCFAMOUNT);
        list.Add(txtMaximum);
        list.Add(txtMaximumLessAmountBilled);
        list.Add(txtMinimum);
        list.Add(txtRetroSubTotal);
        list.Add(txtTaxMultiplier);
        list.Add(btnAdjReviewCESave);

        ProcessExitFlag(list);
    }
    #endregion

    /// <summary>
    /// Method to Load the selected search Criteria while navingating from other tab
    /// </summary>
    /// <param name="strSelectedAccountID"></param>
    /// <param name="strSelectedValDate"></param>
    /// <param name="strSelectedProgramPeriod"></param>
    /// <param name="strSelectedPremAdjID"></param>
    #region fillSelectedValues
    private void fillSelectedValues(string strSelectedAccountID, string strSelectedValDate, string strSelectedProgramPeriod, string strSelectedPremAdjID)
    {


        int intAccountID = 0;
        if (strSelectedAccountID != "")
            intAccountID = Convert.ToInt32(strSelectedAccountID);
        int intPremAdjID = 0;
        if (strSelectedPremAdjID != "")
            intPremAdjID = Convert.ToInt32(strSelectedPremAdjID);
        DateTime dtValDate = new DateTime();
        if (strSelectedValDate != "")
            dtValDate = Convert.ToDateTime(strSelectedValDate);
        string strProgramPeriod = strSelectedProgramPeriod;
        int intPremAdjPerdID = 0;
        int intPrgPeriodID = 0;
        if (strSelectedProgramPeriod != "")
            intPrgPeriodID = Convert.ToInt32(strSelectedProgramPeriod);
        
        IList<PremiumAdjustmentPeriodBE> premAdjPerdBE = new List<PremiumAdjustmentPeriodBE>();
        premAdjPerdBE = PrmAdjPerd.getPremAdjPerdID(intAccountID, intPremAdjID, intPrgPeriodID);
        if (premAdjPerdBE.Count > 0)
        {
            intPremAdjPerdID = premAdjPerdBE[premAdjPerdBE.Count - 1].PREM_ADJ_PERD_ID;
        }
        //Retrieving the Status Type ID of the adjustment Selected
        PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
        IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;

        objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(intPremAdjID);
        objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
        this.AccountID = intAccountID;
        this.PreAdjID = intPremAdjID;
        this.PrgPeriodID = intPrgPeriodID;
        this.PremAdjPerdID = intPremAdjPerdID;
        this.AdjStatusTypeID = objPremStsBE.ADJ_STS_TYP_ID != null ? Convert.ToInt32(objPremStsBE.ADJ_STS_TYP_ID) : 0;
        hidSelectedValues.Value = intAccountID + ";" + dtValDate.ToShortDateString() + ";" + intPremAdjID + ";" + strProgramPeriod;
        ShowCombinedElementsList();


    }
    #endregion
    /// <summary>
    /// Function to Change the ListView States
    /// </summary>
    /// <param name="Flag"></param>
    #region ChangeListState
    private void ChangeListState(bool Flag)
    {
        this.lblListHeader.Visible = Flag;
        this.pnlCombinedElementsList.Visible = Flag;
        this.pnlCalTotal.Visible = Flag;
        this.lblAmountsBilled.Visible = Flag;
        this.pnlDetails.Visible = Flag;
    }
    #endregion

    /// <summary>
    /// Search Button Click event for the user control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Search Button Click Event
    void btnSearch_AdjustmentReviewSearchButtonClicked(object sender, EventArgs e)
    {
        DropDownList ddlAccnts = (DropDownList)this.ARS.FindControl("ddlAccountlist");
        DropDownList ddlValDates = (DropDownList)this.ARS.FindControl("ddlValDate");
        DropDownList ddlPrgPeriod = (DropDownList)this.ARS.FindControl("ddlProgramPeriod");
        DropDownList ddlAdjNumber = (DropDownList)this.ARS.FindControl("ddlAdjNumber");
        if (ddlAccnts.SelectedIndex == 0)
        {
            string strMessage = "Please select Account name";
            ShowError(strMessage);
            return;
        }
        if (ddlValDates.SelectedIndex == 0)
        {
            string strMessage = "Please select Valuation Date";
            ShowError(strMessage);
            return;
        }
        if (ddlAdjNumber.SelectedIndex == 0)
        {
            string strMessage = "Please select Adjustment Number";
            ShowError(strMessage);
            return;
        }
        if (ddlPrgPeriod.SelectedIndex == 0)
        {
            string strMessage = "Please select Program Period";
            ShowError(strMessage);
            return;
        }
        
        int intAccountID = Convert.ToInt32(ddlAccnts.SelectedValue);
        int intPremAdjID = Convert.ToInt32(ddlAdjNumber.SelectedValue);
        DateTime dtValDate = Convert.ToDateTime(ddlValDates.SelectedValue.ToString());
        string strProgramPeriod = ddlPrgPeriod.SelectedValue.ToString();
       
        int intPremAdjPerdID = 0;
        int intPrgPeriodID = Convert.ToInt32(strProgramPeriod);
       
        IList<PremiumAdjustmentPeriodBE> premAdjPerdBE = new List<PremiumAdjustmentPeriodBE>();
        premAdjPerdBE = PrmAdjPerd.getPremAdjPerdID(intAccountID, intPremAdjID, intPrgPeriodID);
        if (premAdjPerdBE.Count > 0)
        {
            intPremAdjPerdID = premAdjPerdBE[premAdjPerdBE.Count - 1].PREM_ADJ_PERD_ID;
        }
        //Retrieving the Status Type ID of the adjustment Selected
        PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
        IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;

        objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(intPremAdjID);
        objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
        this.AccountID = intAccountID;
        this.PreAdjID = intPremAdjID;
        this.PrgPeriodID = intPrgPeriodID;
        this.PremAdjPerdID = intPremAdjPerdID;
        this.AdjStatusTypeID = objPremStsBE.ADJ_STS_TYP_ID != null ? Convert.ToInt32(objPremStsBE.ADJ_STS_TYP_ID) : 0;
        hidSelectedValues.Value = intAccountID + ";" + dtValDate.ToShortDateString() + ";" + intPremAdjID + ";" + strProgramPeriod;
        ShowCombinedElementsList();

    }
    #endregion

    /// <summary>
    /// Function to Display Combined Elemnts List
    /// </summary>
    #region ShowCombinedElementsList
    private void ShowCombinedElementsList()
    {

        IList<CombinedElementsBE> lstCE = CombeleBS.GetCombinedElements(this.PrgPeriodID, this.AccountID);
        lstCE = lstCE.Where(comb => comb.ACTV_IND == true).ToList();
        if (lstCE.Count != 0)
        {
            ChangeListState(true);
        }
        else
        {
            this.lblListHeader.Visible = true;
            this.pnlCombinedElementsList.Visible = true;
            this.pnlCalTotal.Visible = false;
            this.lblAmountsBilled.Visible = false;
            this.pnlDetails.Visible = false;
        }
        int intRetroElemtTypID = CEblaccess.GetLookUpID("Maximum", "RETROSPECTIVE ELEMENT TYPE");
        IList<RetroInfoBE> RetroInfoBE = (new RetroInfoBS()).getCombElemntsMaxAndMinAmounts(this.PrgPeriodID, this.AccountID, intRetroElemtTypID);
        this.lstAdjReviewCombinedelements.DataSource = lstCE;
        this.lstAdjReviewCombinedelements.DataBind();
        //IList<PremiumAdjCombElementsBE> 
         lstPreAdjCE = PrmAdjCombElementsBS.getPremAdjCombElements(this.AccountID, this.PreAdjID, this.PremAdjPerdID);
        if (lstPreAdjCE.Count > 0)
        {
            btnAdjReviewCESave.Text = "Update";
            btnAdjReviewCESave.ToolTip = "Click here to Update";
            fillAmountsBilledList(lstPreAdjCE);
        }
        //Restricting User to Add or Update if Adjustment Status is not CALC
        int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
        if (intAdjCalStatusID != AdjStatusTypeID)
        {
            btnAdjReviewCESave.Enabled = false;
        }
        else
        {
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            btnAdjReviewCESave.Enabled = true;
        }
    }
    #endregion
    #region Maintaining Session for PremiumAdjCombElementsBE
    private IList<PremiumAdjCombElementsBE> lstPreAdjCE
    {
        get
        {
            if (Session["lstPreAdjCE"] == null)
                Session["lstPreAdjCE"] = new List<PremiumAdjCombElementsBE>();
            return (IList<PremiumAdjCombElementsBE>)Session["lstPreAdjCE"];
        }
        set { Session["lstPreAdjCE"] = value; }
    }
    #endregion
    /// <summary>
    /// Function to Display AmountsBilled List
    /// </summary>
    /// <param name="lstPreAdjCE"></param>
    #region fillAmountsBilledList
    private void fillAmountsBilledList(IList<PremiumAdjCombElementsBE> lstPreAdjCE)
    {
        ViewState["PreAdjCombElemntID"] = lstPreAdjCE[0].PREM_ADJ_COMB_ELEMNTS_ID.ToString();
        this.txtBasicPremium.Text = lstPreAdjCE[0].RETRO_BASIC_PREM_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjCE[0].RETRO_BASIC_PREM_AMT.ToString())).ToString("#,##0") : lstPreAdjCE[0].RETRO_BASIC_PREM_AMT.ToString();
        this.txtLossALAELBALCFAMOUNT.Text = lstPreAdjCE[0].RETRO_LOS_FCTR_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjCE[0].RETRO_LOS_FCTR_AMT.ToString())).ToString("#,##0") : lstPreAdjCE[0].RETRO_LOS_FCTR_AMT.ToString();
        this.txtTaxMultiplier.Text = lstPreAdjCE[0].RETRO_TAX_MULTI_RT.ToString();
        this.txtRetroSubTotal.Text = lstPreAdjCE[0].RETRO_SUBTOT_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjCE[0].RETRO_SUBTOT_AMT.ToString())).ToString("#,##0") : lstPreAdjCE[0].RETRO_SUBTOT_AMT.ToString();
        this.txtAmountsBilledMaximum.Text = lstPreAdjCE[0].DEDTBL_MAX_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjCE[0].DEDTBL_MAX_AMT.ToString())).ToString("#,##0") : lstPreAdjCE[0].DEDTBL_MAX_AMT.ToString();
        this.txtDeductibleSubTotal.Text = lstPreAdjCE[0].DEDTBL_SUBTOT_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjCE[0].DEDTBL_SUBTOT_AMT.ToString())).ToString("#,##0") : lstPreAdjCE[0].DEDTBL_SUBTOT_AMT.ToString();
        this.txtMaximumLessAmountBilled.Text = lstPreAdjCE[0].DEDTBL_MAX_LESS_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjCE[0].DEDTBL_MAX_LESS_AMT.ToString())).ToString("#,##0") : lstPreAdjCE[0].DEDTBL_MAX_LESS_AMT.ToString();
        this.txtCalculatedTotal.Text = lstPreAdjCE[0].DEDTBL_MAX_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjCE[0].DEDTBL_MAX_AMT.ToString())).ToString("#,##0") : lstPreAdjCE[0].DEDTBL_MAX_AMT.ToString();
        if (lstPreAdjCE[0].DEDTBL_MAX_AMT >= lstPreAdjCE[0].DEDTBL_MIN_AMT)
        {
            this.lblMinimumApplied.Text = "Maximum Applies";
            this.txtMaximum.Text = lstPreAdjCE[0].DEDTBL_MAX_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjCE[0].DEDTBL_MAX_AMT.ToString())).ToString("#,##0") : lstPreAdjCE[0].DEDTBL_MAX_AMT.ToString();
            this.txtAmountsBilledMaximum.Text = lstPreAdjCE[0].DEDTBL_MAX_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjCE[0].DEDTBL_MAX_AMT.ToString())).ToString("#,##0") : lstPreAdjCE[0].DEDTBL_MAX_AMT.ToString();
        }
        else
        {
            this.lblMinimumApplied.Text = "Minimum Applies";
            this.txtMaximum.Text = lstPreAdjCE[0].DEDTBL_MIN_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjCE[0].DEDTBL_MIN_AMT.ToString())).ToString("#,##0") : lstPreAdjCE[0].DEDTBL_MIN_AMT.ToString();
            this.txtAmountsBilledMaximum.Text = lstPreAdjCE[0].DEDTBL_MIN_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjCE[0].DEDTBL_MIN_AMT.ToString())).ToString("#,##0") : lstPreAdjCE[0].DEDTBL_MIN_AMT.ToString();
        }

        //Formule to Calculate Maximum:
        //Maximum Less Amount Billed = (Deductible Maximum Amount) - (Retro Sub Total Amount + Deductible Sub Total Amount)
        if (lstPreAdjCE[0].DEDTBL_MAX_AMT >= lstPreAdjCE[0].DEDTBL_MIN_AMT)
        {
            this.txtMaximumLessAmountBilled.Text = (Convert.ToDecimal(lstPreAdjCE[0].DEDTBL_MAX_AMT) - Convert.ToDecimal(lstPreAdjCE[0].RETRO_SUBTOT_AMT) - Convert.ToDecimal(lstPreAdjCE[0].DEDTBL_SUBTOT_AMT)).ToString("#,##0");
        }
        else
        {
            this.txtMaximumLessAmountBilled.Text = (Convert.ToDecimal(lstPreAdjCE[0].DEDTBL_MIN_AMT) - Convert.ToDecimal(lstPreAdjCE[0].RETRO_SUBTOT_AMT) - Convert.ToDecimal(lstPreAdjCE[0].DEDTBL_SUBTOT_AMT)).ToString("#,##0");
        }
        int intRetroElemtTypID = CEblaccess.GetLookUpID("Maximum", "RETROSPECTIVE ELEMENT TYPE");
        IList<RetroInfoBE> RetroInfoBE = (new RetroInfoBS()).getCombElemntsMaxAndMinAmounts(this.PrgPeriodID, this.AccountID, intRetroElemtTypID);
        this.txtMinimum.Text = lstPreAdjCE[0].DEDTBL_MIN_AMT.ToString() != "" ? decimal.Parse(lstPreAdjCE[0].DEDTBL_MIN_AMT.ToString()).ToString("#,##0") : lstPreAdjCE[0].DEDTBL_MIN_AMT.ToString();

       

    }
    #endregion

    /// <summary>
    /// Onselectedindexchanged event for the user control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Selected Index Changed Event of User Control
    void ddlprogramPeriod_ARProgramPeriodSelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlAccnts = (DropDownList)this.ARS.FindControl("ddlAccountlist");
        DropDownList ddlValDates = (DropDownList)this.ARS.FindControl("ddlValDate");
        DropDownList ddlPrgPeriod = (DropDownList)this.ARS.FindControl("ddlProgramPeriod");
        DropDownList ddlAdjNumber = (DropDownList)this.ARS.FindControl("ddlAdjNumber");
        HideContents();
        if (ddlAccnts.SelectedIndex != 0)
        {
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ";" + ";";

        }
        else
        {
            hidSelectedValues.Value = "";
        }
        if (ddlValDates.SelectedIndex != 0)
        {
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";" + ";";

        }
        else if (ddlAccnts.SelectedIndex != 0)
        {
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ";" + ";";

        }
        if (ddlAdjNumber.SelectedIndex != 0)
        {
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";" + ddlAdjNumber.SelectedValue + ";";
        }
        else if (ddlValDates.SelectedIndex != 0)
        {
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";" + ";";
        }
        if (ddlPrgPeriod.SelectedIndex != 0)
        {
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";" + ddlAdjNumber.SelectedValue + ";" + ddlPrgPeriod.SelectedValue;
        }
        else if (ddlAdjNumber.SelectedIndex != 0)
        {
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";" + ddlAdjNumber.SelectedValue + ";";
        }
       

    }
    #endregion
   
    /// <summary>
    /// Method to hide panels in selected index Changed
    /// </summary>
    #region HideContents
    private void HideContents()
    {
        lblListHeader.Visible = false;
        lstAdjReviewCombinedelements.Items.Clear();
        pnlCombinedElementsList.Visible = false;
        pnlCalTotal.Visible = false;
        lblAmountsBilled.Visible = false;
        pnlDetails.Visible = false;
        ClearFields();
    }
    #endregion
    /// <summary>
    /// To Clear All fields
    /// </summary>
    #region ClearFields
    private void ClearFields()
    {
        txtMaximum.Text = string.Empty;
        txtCalculatedTotal.Text = string.Empty;
        txtMaximum.Text = string.Empty;
        txtBasicPremium.Text = string.Empty;
        txtLossALAELBALCFAMOUNT.Text = string.Empty;
        txtTaxMultiplier.Text = string.Empty;
        txtRetroSubTotal.Text = string.Empty;
        txtAmountsBilledMaximum.Text = string.Empty;
        txtDeductibleSubTotal.Text = string.Empty;
        txtMaximumLessAmountBilled.Text = string.Empty;
    }
    #endregion
   
    /// <summary>
    /// Invoked When the Save or Update Button is Clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Save or Update Button Click Event
    protected void btnAdjReviewCESave_Click(object sender, EventArgs e)
    {
        if (btnAdjReviewCESave.Text.ToUpper() == "SAVE")
        {
            savePremAdjCombElements();

        }
        else
        {
            updatePremAdjCombElements();

        }


    }
    #endregion

    /// <summary>
    /// Function to Update PremAdjCombElements table
    /// </summary>
    #region updatePremAdjCombElements
    private void updatePremAdjCombElements()
    {
        PremiumAdjCombElementsBE preAdjCombEleBE;
        preAdjCombEleBE = PrmAdjCombElementsBS.getPremAdjCombElementsRow(Convert.ToInt32(ViewState["PreAdjCombElemntID"].ToString()));
        //Concurrency Issue
        if (lstPreAdjCE.Count > 0)
        {
            PremiumAdjCombElementsBE preAdjCombEleBEold = (lstPreAdjCE.Where(o => o.PREM_ADJ_COMB_ELEMNTS_ID.Equals(Convert.ToInt32(ViewState["PreAdjCombElemntID"].ToString())))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(preAdjCombEleBEold.UPDATE_DATE), Convert.ToDateTime(preAdjCombEleBE.UPDATE_DATE));
            if (!con)
                return;
        }
        //End
        if (this.txtBasicPremium.Text.Trim() != "")
            preAdjCombEleBE.RETRO_BASIC_PREM_AMT = Convert.ToDecimal(this.txtBasicPremium.Text.Trim());
        else
            preAdjCombEleBE.RETRO_BASIC_PREM_AMT = null;
        if (this.txtLossALAELBALCFAMOUNT.Text.Trim() != "")
            preAdjCombEleBE.RETRO_LOS_FCTR_AMT = Convert.ToDecimal(this.txtLossALAELBALCFAMOUNT.Text.Trim());
        else
            preAdjCombEleBE.RETRO_LOS_FCTR_AMT = null;
        if (this.txtTaxMultiplier.Text.Trim() != "")
            preAdjCombEleBE.RETRO_TAX_MULTI_RT = Convert.ToDecimal(this.txtTaxMultiplier.Text.Trim());
        else
            preAdjCombEleBE.RETRO_TAX_MULTI_RT = null;
        if (this.txtRetroSubTotal.Text.Trim() != "")
            preAdjCombEleBE.RETRO_SUBTOT_AMT = Convert.ToDecimal(this.txtRetroSubTotal.Text.Trim());
        else
            preAdjCombEleBE.RETRO_SUBTOT_AMT = null;
        if (this.txtDeductibleSubTotal.Text.Trim() != "")
            preAdjCombEleBE.DEDTBL_SUBTOT_AMT = Convert.ToDecimal(this.txtDeductibleSubTotal.Text.Trim());
        else
            preAdjCombEleBE.DEDTBL_SUBTOT_AMT = null;
        if (this.txtMaximumLessAmountBilled.Text.Trim() != "")
            preAdjCombEleBE.DEDTBL_MAX_LESS_AMT = Convert.ToDecimal(this.txtMaximumLessAmountBilled.Text.Trim());
        else
            preAdjCombEleBE.DEDTBL_MAX_LESS_AMT = null;
        if (this.txtCalculatedTotal.Text.Trim() != "")
            preAdjCombEleBE.DEDTBL_MAX_AMT = Convert.ToDecimal(this.txtCalculatedTotal.Text.Trim());
        else
            preAdjCombEleBE.DEDTBL_MAX_AMT = null;

        preAdjCombEleBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
        preAdjCombEleBE.UPDATE_DATE = System.DateTime.Now;
        bool Flag=prmAdjCombElementsBS.Update(preAdjCombEleBE);
       // ShowConcurrentConflict(Flag, preAdjCombEleBE.ErrorMessage);
        lstPreAdjCE = PrmAdjCombElementsBS.getPremAdjCombElements(this.AccountID, this.PreAdjID, this.PremAdjPerdID);
        fillAmountsBilledList(lstPreAdjCE);
    }
    #endregion
    /// <summary>
    /// Function to Save PremAdjCombElements table
    /// </summary>
    #region savePremAdjCombElements
    private void savePremAdjCombElements()
    {
        IList<PremiumAdjCombElementsBE> lstPreAdjCEBE = PrmAdjCombElementsBS.getPremAdjCombElements(this.AccountID, this.PreAdjID, this.PremAdjPerdID);
        if (lstPreAdjCEBE.Count > 0)
        {
            ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
            return;
        }
        PremiumAdjCombElementsBE paCEBE = new PremiumAdjCombElementsBE();
        paCEBE.PREM_ADJ_PERD_ID = this.PremAdjPerdID;
        paCEBE.PREM_ADJ_ID = this.PreAdjID;
        paCEBE.CUSTMR_ID = this.AccountID;
        if (this.txtBasicPremium.Text.Trim() != "")
            paCEBE.RETRO_BASIC_PREM_AMT = Convert.ToDecimal(this.txtBasicPremium.Text.Trim());
        else
            paCEBE.RETRO_BASIC_PREM_AMT = null;
        if (this.txtLossALAELBALCFAMOUNT.Text.Trim() != "")
            paCEBE.RETRO_LOS_FCTR_AMT = Convert.ToDecimal(this.txtLossALAELBALCFAMOUNT.Text.Trim());
        else
            paCEBE.RETRO_LOS_FCTR_AMT = null;
        if (this.txtTaxMultiplier.Text.Trim() != "")
            paCEBE.RETRO_TAX_MULTI_RT = Convert.ToDecimal(this.txtTaxMultiplier.Text.Trim());
        else
            paCEBE.RETRO_TAX_MULTI_RT = null;
        if (this.txtRetroSubTotal.Text.Trim() != "")
            paCEBE.RETRO_SUBTOT_AMT = Convert.ToDecimal(this.txtRetroSubTotal.Text.Trim());
        else
            paCEBE.RETRO_SUBTOT_AMT = null;
        if (this.txtDeductibleSubTotal.Text.Trim() != "")
            paCEBE.DEDTBL_SUBTOT_AMT = Convert.ToDecimal(this.txtDeductibleSubTotal.Text.Trim());
        else
            paCEBE.DEDTBL_SUBTOT_AMT = null;
        if (this.txtMaximumLessAmountBilled.Text.Trim() != "" && this.txtMaximumLessAmountBilled.Text.Trim() != "NaN")
            paCEBE.DEDTBL_MAX_LESS_AMT = Convert.ToDecimal(this.txtMaximumLessAmountBilled.Text.Trim());
        else
            paCEBE.DEDTBL_MAX_LESS_AMT = null;
        if (this.txtCalculatedTotal.Text.Trim() != "")
            paCEBE.DEDTBL_MAX_AMT = Convert.ToDecimal(this.txtCalculatedTotal.Text.Trim());
        else
            paCEBE.DEDTBL_MAX_AMT = null;
        paCEBE.CREATE_USER_ID = CurrentAISUser.PersonID;
        paCEBE.CREATE_DATE = System.DateTime.Now;
        bool i = PrmAdjCombElementsBS.Update(paCEBE);
        if (i == false)
            ShowError("Unable to Save");
        else
        {
            btnAdjReviewCESave.Text = "Update";
            btnAdjReviewCESave.ToolTip = "Click here to Update";
           // IList<PremiumAdjCombElementsBE>
                lstPreAdjCE = PrmAdjCombElementsBS.getPremAdjCombElements(this.AccountID, this.PreAdjID, this.PremAdjPerdID);
            fillAmountsBilledList(lstPreAdjCE);
        }

    }
    #endregion

    /// <summary>
    /// Invoked when the Preview Button is Clicked-need this functionality from suneel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region PreviewButton Click Event
    protected void btnAdjReviewPreview_Click(object sender, EventArgs e)
    {
        if (!(new AdjustmentReviewCommentsBS().IsReportAvialable(PreAdjID, 1, "COMBINED ELEMENTS")))
        {
            ShowError("Report is not available to Preview");
            return;
        }
        DownloadFile(PreAdjID, "COMBINED ELEMENTS");
    }
    #endregion

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
    }
    #endregion
}
#endregion