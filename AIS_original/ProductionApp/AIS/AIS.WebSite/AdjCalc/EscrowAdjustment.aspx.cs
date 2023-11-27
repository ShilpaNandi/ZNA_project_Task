//Default namespaces in EscrowAdjustment screen
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
//Importing different AIS framework namespaces for EscrowAdjustment screen
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.AIS.DAL.Logic;

/// <summary>
/// This is the class which is used to give the information about the EscrowAdjustment and it 
/// also inherits AISBase Page,so that some of the common functionality needed for all pages is implemented in
/// the Basepage
/// </summary>
#region EscrowAdjustment Class
public partial class AdjCalc_EscrowAdjustment : AISBasePage
{
    private BLAccess ESCROWblaccess = new BLAccess();
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
        this.Master.Page.Title = "Escrow Adjustment";
        if (!IsPostBack)
        {
            divsave.Visible = true;
            divUpdate.Visible = false;
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
    /// <summary>
    /// Method for showing any changes in the data entry fields
    /// in order to show the warning message
    /// </summary>
    #region CheckExitWithoutSave
    private void CheckExitWithoutSave()
    {
        ArrayList list = new ArrayList();
        list.Add(txtAdjAmount);
        list.Add(txtAdjPaidLoss);
        list.Add(txtCalAmount);
        list.Add(txtDivedBy);
        list.Add(txtMnthsHeld);
        list.Add(txtPLBmnts);
        list.Add(txtPrevEscrowAmt);
        list.Add(lnkBtnAdjReviewEscrowCancel);
        list.Add(lnkBtnAdjReviewEscrowSave);
        list.Add(lnkBtnAdjReviewEscrowUpdate);

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

        IList<ProgramPeriodSearchListBE> premAdjPgmBE = new List<ProgramPeriodSearchListBE>();
        premAdjPgmBE = new ProgramPeriodsBS().GetProgramPeriodsList(strSelectedProgramPeriod);
        lblAdjReviewEscrow.Text = "ESCROW FOR-" + premAdjPgmBE[0].STARTDATE_ENDDATE_PGMTYP.ToString();
        ShowEscrowList();

    }
    #endregion
    #region Properties Declaration Section
    /// <summary>
    /// a property for Adj_Parameter_Setup Business Service Class
    /// </summary>
    /// <returns>Adj_Parameter_SetupBS</returns>
    private Adj_Parameter_SetupBS adjParameterSetupBS;
    private Adj_Parameter_SetupBS AdjParameterSetupBS
    {
        get
        {
            if (adjParameterSetupBS == null)
            {
                adjParameterSetupBS = new Adj_Parameter_SetupBS();
            }
            return adjParameterSetupBS;
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
    /// a property for PremiumAdjustmentEscrow Business Service Class
    /// </summary>
    /// <returns>PremiumAdjustmentEscrowBS</returns>
    private PremiumAdjustmentEscrowBS prmAdjEscrow;
    private PremiumAdjustmentEscrowBS PrmAdjEscrow
    {
        get
        {
            if (prmAdjEscrow == null)
            {
                prmAdjEscrow = new PremiumAdjustmentEscrowBS();
            }
            return prmAdjEscrow;
        }
    }

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
    /// A Property for Holding PremAdjParSetupID in ViewState
    /// </summary>
    protected int PremAdjParSetupID
    {
        get
        {
            if (ViewState["PremAdjParSetupID"] != null)
            {
                return int.Parse(ViewState["PremAdjParSetupID"].ToString());
            }
            else
            {
                ViewState["PremAdjParSetupID"] = 0;
                return 0;
            }
        }
        set
        {
            ViewState["PremAdjParSetupID"] = value;
        }
    }
    /// <summary>
    /// A Property for Holding adj_paramet_setup_id in ViewState
    /// </summary>
    protected int adj_paramet_setup_id
    {
        get
        {
            if (ViewState["adj_paramet_setup_id"] != null)
            {
                return int.Parse(ViewState["adj_paramet_setup_id"].ToString());
            }
            else
            {
                ViewState["adj_paramet_setup_id"] = 0;
                return 0;
            }
        }
        set
        {
            ViewState["adj_paramet_setup_id"] = value;
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
        lblAdjReviewEscrow.Text = "ESCROW FOR-" + ddlPrgPeriod.SelectedItem.Text;
        ShowEscrowList();


    }
    #endregion
    #region Maintaining Session for PaidLossBillingBE
    private IList<PremiumAdjustmentEscrowBE> premAdjEscrowBE
    {
        get
        {
            if (Session["premAdjEscrowBE"] == null)
                Session["premAdjEscrowBE"] = new List<PremiumAdjustmentEscrowBE>();
            return (IList<PremiumAdjustmentEscrowBE>)Session["premAdjEscrowBE"];
        }
        set { Session["premAdjEscrowBE"] = value; }
    }
    #endregion
    
    /// <summary>
    /// Function to Display the fields in Escrow form
    /// </summary>
    #region ShowEscrowList
    private void ShowEscrowList()
    {
        int intAdjParameterTypeID = ESCROWblaccess.GetLookUpID("Escrow", "ADJUSTMENT PARAMETER TYPE");
        adjParameterSetupBS = new Adj_Parameter_SetupBS();
        IList<AdjustmentParameterSetupBE> AdjParameterSetupBE = adjParameterSetupBS.getAdjReviewEscrow(this.PrgPeriodID, this.AccountID, intAdjParameterTypeID);
        if (AdjParameterSetupBE.Count > 0)
        {
            pnlDetails.Visible = true;
            pnlAdjReviewEscrowEmptyData.Visible = false;
            lbAdjReviewscrowPolicy.DataSource = AdjParameterSetupBE[0].AdjParametPolBEs;
            lbAdjReviewscrowPolicy.DataTextField = "PolicyPerfectNumber";
            lbAdjReviewscrowPolicy.DataValueField = "coml_agmt_id";
            lbAdjReviewscrowPolicy.DataBind();
            this.txtPLBmnts.Text = AdjParameterSetupBE[0].Escrow_PLMNumber.ToString();
            this.txtMnthsHeld.Text = AdjParameterSetupBE[0].Escrow_MnthsHeld.ToString();
            this.txtDivedBy.Text = AdjParameterSetupBE[0].Escrow_Diveser.ToString();
            //this.txtPrevEscrowAmt.Text = AdjParameterSetupBE[0].Escrow_PrevAmt.ToString() != "" ? (decimal.Parse(AdjParameterSetupBE[0].Escrow_PrevAmt.ToString())).ToString("#,##0.00") : AdjParameterSetupBE[0].Escrow_PrevAmt.ToString();
            this.adj_paramet_setup_id = AdjParameterSetupBE[0].adj_paramet_setup_id;
            //IList<PremiumAdjustmentEscrowBE> premAdjEscrowBE = new List<PremiumAdjustmentEscrowBE>();
            premAdjEscrowBE = PrmAdjEscrow.GetPremAdjEscrowInfo(this.AccountID, this.PremAdjPerdID, this.PreAdjID, this.PrgPeriodID, AdjParameterSetupBE[AdjParameterSetupBE.Count - 1].adj_paramet_setup_id);
            //Restricting User to Add or Update if Adjustment Status is not CALC
            int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            if (intAdjCalStatusID != AdjStatusTypeID)
            {
                lnkBtnAdjReviewEscrowSave.Enabled = false;
            }
            else
            {
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                    lnkBtnAdjReviewEscrowSave.Enabled = true;
            }
            if (premAdjEscrowBE.Count > 0)
            {
                this.PremAdjParSetupID = premAdjEscrowBE[0].PREM_ADJ_PARMET_SETUP_ID;
                this.txtAdjPaidLoss.Text = premAdjEscrowBE[0].ESCR_ADJ_PAID_LOS_AMT.ToString() != "" ? (decimal.Parse(premAdjEscrowBE[0].ESCR_ADJ_PAID_LOS_AMT.ToString())).ToString("#,##0.00") : premAdjEscrowBE[0].ESCR_ADJ_PAID_LOS_AMT.ToString();
                this.txtCalAmount.Text = premAdjEscrowBE[0].ESCR_AMT.ToString() != "" ? (decimal.Parse(premAdjEscrowBE[0].ESCR_AMT.ToString())).ToString("#,##0.00") : premAdjEscrowBE[0].ESCR_AMT.ToString();
                this.txtPrevEscrowAmt.Text = premAdjEscrowBE[0].ESCR_PREVIOUSULY_BILED_AMT.ToString() != "" ? (decimal.Parse(premAdjEscrowBE[0].ESCR_PREVIOUSULY_BILED_AMT.ToString())).ToString("#,##0.00") : premAdjEscrowBE[0].ESCR_PREVIOUSULY_BILED_AMT.ToString();
                if (premAdjEscrowBE[0].TOT_AMT.ToString() == "")
                {
                    lnkBtnAdjReviewEscrowSave.Text = "Save";
                    lnkBtnAdjReviewEscrowSave.ToolTip = "Click here to Save";
                    lnkBtnAdjReviewEscrowSave.ValidationGroup = "EscrowGroup";


                }
                else
                {
                    this.txtAdjAmount.Text = premAdjEscrowBE[0].TOT_AMT.ToString() != "" ? (decimal.Parse(premAdjEscrowBE[0].TOT_AMT.ToString())).ToString("#,##0") : premAdjEscrowBE[0].TOT_AMT.ToString();
                    lnkBtnAdjReviewEscrowSave.Text = "Edit";
                    lnkBtnAdjReviewEscrowSave.ToolTip = "Click here to Edit";
                    lnkBtnAdjReviewEscrowSave.ValidationGroup = string.Empty;
                    this.txtAdjAmount.ReadOnly = true;

                }
            }
        }
        else
        {
            pnlAdjReviewEscrowEmptyData.Visible = true;
            pnlDetails.Visible = false;
        }
    }
    #endregion
    /// <summary>
    /// Invoked when the Save link is Cliked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Save Button Click Event
    protected void lnkBtnAdjReviewEscrowSave_Click(object sender, EventArgs e)
    {
        if (lnkBtnAdjReviewEscrowSave.Text.ToUpper() == "SAVE")
        {
            IList<PremiumAdjustmentEscrowBE> premAdjEscrowBEList = new List<PremiumAdjustmentEscrowBE>();
            premAdjEscrowBEList = PrmAdjEscrow.GetPremAdjEscrowInfo(this.AccountID, this.PremAdjPerdID, this.PreAdjID, this.PrgPeriodID, this.adj_paramet_setup_id);
            if (premAdjEscrowBEList.Count > 0)
                UpdateEscrow();
            else
                SaveEscrow();

        }
        else
        {
            divUpdate.Visible = true;
            divsave.Visible = false;
            ShowEscrowList();
            this.txtAdjAmount.ReadOnly = false;
        }

    }
    #endregion
    /// <summary>
    /// Function to Save the Record in Escrow Table
    /// </summary>
    #region SaveEscrow
    private void SaveEscrow()
    {
        int intAdjParameterTypeID = ESCROWblaccess.GetLookUpID("Escrow", "ADJUSTMENT PARAMETER TYPE");
        adjParameterSetupBS = new Adj_Parameter_SetupBS();
        IList<AdjustmentParameterSetupBE> AdjParameterSetupBE = adjParameterSetupBS.getAdjReviewEscrow(this.PrgPeriodID, this.AccountID, intAdjParameterTypeID);
        PremiumAdjustmentEscrowBE premAdjEscrowBESave = new PremiumAdjustmentEscrowBE();
        premAdjEscrowBESave.PREM_ADJ_ID = this.PreAdjID;
        premAdjEscrowBESave.PREM_ADJ_PERD_ID = this.PremAdjPerdID;
        premAdjEscrowBESave.CUSTMR_ID = this.AccountID;
        premAdjEscrowBESave.PREM_ADJ_PGM_ID = this.PrgPeriodID;
        premAdjEscrowBESave.PREM_ADJ_PGM_SETUP_ID = this.adj_paramet_setup_id;
        premAdjEscrowBESave.ADJ_PARMET_TYP_ID = intAdjParameterTypeID;
        premAdjEscrowBESave.TOT_AMT = Convert.ToDecimal(this.txtAdjAmount.Text);
        premAdjEscrowBESave.CREATE_DATE = System.DateTime.Now;
        premAdjEscrowBESave.CREATE_USER_ID = CurrentAISUser.PersonID;
        bool Flag = PrmAdjEscrow.Update(premAdjEscrowBESave);
        //updating Prem_adj_perd table
        PremiumAdjustmentPeriodBE premAdjPerdBE = new PremiumAdjustmentPeriodBE();
        premAdjPerdBE = (new PremiumAdjustmentPeriodBS()).getPremAdjPerdRow(premAdjEscrowBESave.PREM_ADJ_PERD_ID);
        premAdjPerdBE.ESCR_MNL_OVERRID_IND = true;
        premAdjPerdBE.ESCR_TOT_AMT = Convert.ToDecimal(this.txtAdjAmount.Text);
        premAdjPerdBE.UPDATE_DATE = System.DateTime.Now;
        premAdjPerdBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
        (new PremiumAdjustmentPeriodBS()).Update(premAdjPerdBE);
        if (Flag)
        {
            (new PremiumAdjustmentPeriodBS()).deletePremAdjPerdTotal(this.PremAdjPerdID, this.PreAdjID, this.AccountID);
            (new PremiumAdjustmentPeriodBS()).AddPremAdjPerdTotal(this.PremAdjPerdID, this.PreAdjID, this.AccountID, this.PrgPeriodID, CurrentAISUser.PersonID);
        }
        ShowEscrowList();

    }
    #endregion
    /// <summary>
    /// Invoked when the Update Button is Clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Update Button Click Event
    protected void lnkBtnAdjReviewEscrowUpdate_Click(object sender, EventArgs e)
    {
        UpdateEscrow();
        

    }
    #endregion
    /// <summary>
    /// Invoked when the Cancel link is Clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Cancel Button Click Event
    protected void lnkBtnAdjReviewEscrowCancel_Click(object sender, EventArgs e)
    {
        ShowEscrowList();
        divsave.Visible = true;
        divUpdate.Visible = false;
    }
    #endregion
    /// <summary>
    /// Function to Update the record in Escrow Table
    /// </summary>
    #region UpdateEscrow
    private void UpdateEscrow()
    {
        int intPremAdjParSetupID = this.PremAdjParSetupID;
        PremiumAdjustmentEscrowBE premAdjEscrowBEUpdate = new PremiumAdjustmentEscrowBE();
        PremiumAdjustmentPeriodBE premAdjPerdBE = new PremiumAdjustmentPeriodBE();
        premAdjEscrowBEUpdate = PrmAdjEscrow.getPremAdjEscrowRow(intPremAdjParSetupID);
        //Concurrency Issue
        if (premAdjEscrowBE.Count == 0)
        {
            ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
            return;

        }
        else
        {
            PremiumAdjustmentEscrowBE premAdjEscrowBEold = (premAdjEscrowBE.Where(o => o.PREM_ADJ_PARMET_SETUP_ID.Equals(intPremAdjParSetupID))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(premAdjEscrowBEold.UPDATE_DATE), Convert.ToDateTime(premAdjEscrowBEUpdate.UPDATE_DATE));
            if (!con)
                return;
        }
        //End
        premAdjEscrowBEUpdate.TOT_AMT = Convert.ToDecimal(this.txtAdjAmount.Text);
        premAdjEscrowBEUpdate.UPDATE_DATE = System.DateTime.Now;
        premAdjEscrowBEUpdate.UPDATE_USER_ID = CurrentAISUser.PersonID;
        bool Flag = PrmAdjEscrow.Update(premAdjEscrowBEUpdate);
        premAdjPerdBE = (new PremiumAdjustmentPeriodBS()).getPremAdjPerdRow(premAdjEscrowBEUpdate.PREM_ADJ_PERD_ID);
        premAdjPerdBE.ESCR_MNL_OVERRID_IND = true;
        premAdjPerdBE.ESCR_TOT_AMT = Convert.ToDecimal(this.txtAdjAmount.Text);
        premAdjPerdBE.UPDATE_DATE = System.DateTime.Now;
        premAdjPerdBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
        (new PremiumAdjustmentPeriodBS()).Update(premAdjPerdBE);
        //ShowConcurrentConflict(Flag, premAdjEscrowBE.ErrorMessage);
        if (Flag)
        {
            (new PremiumAdjustmentPeriodBS()).deletePremAdjPerdTotal(this.PremAdjPerdID, this.PreAdjID, this.AccountID);
            (new PremiumAdjustmentPeriodBS()).AddPremAdjPerdTotal(this.PremAdjPerdID, this.PreAdjID, this.AccountID, this.PrgPeriodID, CurrentAISUser.PersonID);
        }
        ShowEscrowList();
        divsave.Visible = true;
        divUpdate.Visible = false;
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
        //To Make panels visible false in every selected index change
        lblAdjReviewEscrow.Text = string.Empty;
        pnlDetails.Visible = false;
        pnlAdjReviewEscrowEmptyData.Visible = false;
        lblAdjReviewEscrow.Text = string.Empty;
        this.txtPLBmnts.Text = string.Empty;
        this.txtMnthsHeld.Text = string.Empty;
        this.txtDivedBy.Text = string.Empty;
        this.txtCalAmount.Text = string.Empty;
        this.txtAdjPaidLoss.Text = string.Empty;
        this.txtAdjAmount.Text = string.Empty;
        this.txtPrevEscrowAmt.Text = string.Empty;
        lnkBtnAdjReviewEscrowSave.Text = "Save";
        lnkBtnAdjReviewEscrowSave.ToolTip = "Click here to Save";
        lnkBtnAdjReviewEscrowSave.ValidationGroup = "EscrowGroup";
        this.txtAdjAmount.ReadOnly = false;
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
    /// Previw Button Click Event-Suneel needs to implement this functionality
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Preview Button Click Event
    protected void lnkPreview_Click(object sender, EventArgs e)
    {
        if (!(new AdjustmentReviewCommentsBS().IsReportAvialable(PreAdjID, 1, "ESCROW")))
        {
            ShowError("Report is not available to Preview");
            return;
        }
        DownloadFile(PreAdjID, "ESCROW");
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