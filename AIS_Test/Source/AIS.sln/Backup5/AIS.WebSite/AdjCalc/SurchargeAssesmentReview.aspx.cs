/*
             File Name          : SurchargeAssesmentReview.aspx
 *           Description        : code having logic to handle events and bussisness logic for surcharge
 *                                Assesment
 *           Author             : Phani Neralla
 *           Team               : Finsol/ AIS
 *           Creation Date      : 18-Jun-2010
 *           Last Modified By   : 
 *           Last Modified Date :
*/
# region SurchargeAssesment_CodeBehind
#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

//Importing different AIS framework namespaces for Surcharge Assesment Review screen
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.ExceptionHandling;

#endregion

#region SurchargeAssesment Class

/// <summary>
/// the code behind class for the Surcharge Assesments class.
/// </summary>
public partial class AdjCalc_SurchargeAssesmentReview : AISBasePage
{
    #region Properties Declaration Section
    /// <summary>
    /// A Property for Holding AccountID in ViewState
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
            //Session["PreAdjID"] = value;
            SaveObjectToSessionUsingWindowName("PreAdjID", value);
            ViewState["PreAdjID"] = value;

        }
    }
    /// <summary>
    /// A Property for Holding PrgPeriodID in ViewState
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
            //Session["PrgPeriodID"] = value;
            SaveObjectToSessionUsingWindowName("PrgPeriodID", value);
            ViewState["PrgPeriodID"] = value;
        }
    }
    /// <summary>
    /// A Property for Holding PremAdjPerdID in ViewState
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
            //Session["PemAdjPeriodID"] = value;
            SaveObjectToSessionUsingWindowName("PemAdjPeriodID", value);
            ViewState["PremAdjPerdID"] = value;
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
    /// A Property for Holding SurCodeID in ViewState
    /// </summary>
    protected int SurCodeID
    {
        get
        {
            if (ViewState["SurCodeID"] != null)
            {
                return int.Parse(ViewState["SurCodeID"].ToString());
            }
            else
            {
                ViewState["SurCodeID"] = 0;
                return 0;
            }
        }
        set
        {
            ViewState["SurCodeID"] = value;
            //Session["SurCodeID"] = value;
            SaveObjectToSessionUsingWindowName("SurCodeID", value);
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
    /// a property for PremiumAdjustmentPeriod Business Service Class
    /// </summary>
    /// <returns>PremiumAdjustmentPeriodBS</returns>
    private SurchargeDetailsBS asurDetails;
    private SurchargeDetailsBS SurDTL
    {
        get
        {

            if (asurDetails == null)
            {
                asurDetails = new SurchargeDetailsBS();
            }
            return asurDetails;
        }
    }
    /// <summary>
    /// datasource for the surcharge Assesment list view
    /// </summary>
    private IList<SurchargeDetailAmountBE> SurAdjConcurntList
    {
        get 
        { 
            //return (IList<SurchargeDetailAmountBE>)Session["SurchargeDetailAmountBE"]; 
            return (IList<SurchargeDetailAmountBE>)RetrieveObjectFromSessionUsingWindowName("SurchargeDetailAmountBE");
        }
        set 
        { 
            //Session["SurchargeDetailAmountBE"] = value; 
            SaveObjectToSessionUsingWindowName("SurchargeDetailAmountBE", value); 
        }

    }
    /// <summary>
    /// datasource for the surcharge Assesment list view
    /// </summary>
    private IList<SurchargeDetailAmountBE> SurPolConcurntList
    {
        get 
        { 
            //return (IList<SurchargeDetailAmountBE>)Session["SurchargePolDetailAmountBE"]; 
            return (IList<SurchargeDetailAmountBE>)RetrieveObjectFromSessionUsingWindowName("SurchargePolDetailAmountBE"); 
        }
        set 
        { 
            //Session["SurchargePolDetailAmountBE"] = value; 
            SaveObjectToSessionUsingWindowName("SurchargePolDetailAmountBE", value); 
        }

    }

    protected int SurCustID
    {
        get
        {
            if (ViewState["SurCustID"] != null)
            {
                return int.Parse(ViewState["SurCustID"].ToString());
            }
            else
            {
                ViewState["SurCustID"] = 0;
                return 0;
            }
        }
        set
        {
            //Session["SurCustID"] = value;
            SaveObjectToSessionUsingWindowName("SurCustID", value);
        }
    }

    /// <summary>
    /// A Property for Holding stateid in ViewState
    /// </summary>
    protected int StateID
    {
        get
        {
            if (ViewState["StateID"] != null)
            {
                return int.Parse(ViewState["StateID"].ToString());
            }
            else
            {
                ViewState["StateID"] = 0;
                return 0;
            }
        }
        set
        {
            ViewState["StateID"] = value;
            //Session["StateID"] = value;
            SaveObjectToSessionUsingWindowName("StateID", value);
        }
    }
    #endregion

    #region PageLifecycle
    /// <summary>
    /// Page load event handler.Add the usercontrol events and handle the postback events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        //User Control Event Handlers
        this.ARS.AdjustmentReviewSearchButtonClicked += new
            EventHandler(btnSearch_AdjustmentReviewSearchButtonClicked);
        this.ARS.ARProgramPeriodSelectedIndexChanged += new
            EventHandler(ddlprogramPeriod_ARProgramPeriodSelectedIndexChanged);
        // To display the title of the page
        this.Master.Page.Title = "Surcharge Assessment";
        lstPolReview.Visible = false;
        lblLstPolReviewHeader.Visible = false;
        lbClosePolicyDetails.Visible = false;
        if (!IsPostBack)
        {

            //verification for QueryString to implement persistance
            if (!string.IsNullOrEmpty(Request.QueryString["SelectedValues"]))
            {
                string[] strSelectedValues = Request.QueryString["SelectedValues"].ToString().Split(';');
                if (strSelectedValues.Length > 3)
                {
                    string strSelectedAccountID = strSelectedValues[0].ToString();
                    string strSelectedValDate = strSelectedValues[1].ToString();
                    string strSelectedPremAdjID = strSelectedValues[2].ToString();
                    string strSelectedProgramPeriod = strSelectedValues[3].ToString();
                    if (strSelectedAccountID != "" && strSelectedValDate != "" && strSelectedPremAdjID != ""
                        && strSelectedProgramPeriod != "")
                        FillSelectedValues(strSelectedAccountID, strSelectedValDate,
                                                            strSelectedProgramPeriod, strSelectedPremAdjID);
                    else
                        hidSelectedValues.Value = strSelectedAccountID + ";" + strSelectedValDate + ";" +
                                                  strSelectedPremAdjID + ";" + strSelectedProgramPeriod;
                }
                if (strSelectedValues.Length > 4)
                {
                    //this.SurCodeID = Convert.ToInt32(Session["SurCodeID"].ToString());
                    this.SurCodeID = Convert.ToInt32(RetrieveObjectFromSessionUsingWindowName("SurCodeID").ToString());
                    BindListView("policy");

                }
            }

        }

    }
    #endregion

    #region supporting methods


    #region FillSelectedValues
    /// <summary>
    /// Method to Load the selected search Criteria while navigating from other tab
    /// </summary>
    /// <param name="strSelectedAccountID"></param>
    /// <param name="strSelectedValDate"></param>
    /// <param name="strSelectedProgramPeriod"></param>
    /// <param name="strSelectedPremAdjID"></param>
    private void FillSelectedValues(string strSelectedAccountID_in, string strSelectedValDate_in,
                                    string strSelectedProgramPeriod_in, string strSelectedPremAdjID_in)
    {
        int intAccountID = 0;
        if (!String.IsNullOrEmpty(strSelectedAccountID_in))
            intAccountID = Convert.ToInt32(strSelectedAccountID_in);
        int intPremAdjID = 0;
        if (!String.IsNullOrEmpty(strSelectedPremAdjID_in))
            intPremAdjID = Convert.ToInt32(strSelectedPremAdjID_in);
        DateTime dtValDate = new DateTime();
        if (!String.IsNullOrEmpty(strSelectedValDate_in))
            dtValDate = Convert.ToDateTime(strSelectedValDate_in);
        string strProgramPeriod = strSelectedProgramPeriod_in;
        int intPremAdjPerdID = 0;
        int intPrgPeriodID = 0;
        if (!String.IsNullOrEmpty(strSelectedProgramPeriod_in))
            intPrgPeriodID = Convert.ToInt32(strSelectedProgramPeriod_in);

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
        this.AdjStatusTypeID = objPremStsBE.ADJ_STS_TYP_ID != null ?
                               Convert.ToInt32(objPremStsBE.ADJ_STS_TYP_ID) : 0;
        hidSelectedValues.Value = intAccountID + ";" + dtValDate.ToShortDateString() + ";" +
                                  intPremAdjID + ";" + strProgramPeriod;

        IList<ProgramPeriodSearchListBE> premAdjPgmBE = new List<ProgramPeriodSearchListBE>();
        premAdjPgmBE = new ProgramPeriodsBS().GetProgramPeriodsList(strSelectedProgramPeriod_in);
        lblSurchargeReviewDetails.Text = "SURCHARGE AND ASSESSMENT REVIEW"
                                    + "" + "-" + premAdjPgmBE[0].STARTDATE_ENDDATE_PGMTYP.ToString();
        BindListView("state");
    }
    #endregion

    #region BindListView

    /// <summary>
    /// Function to bind the Listview with state level surcharge detail data
    /// </summary>
    /// <param name="detailLVL_in">indicates if it is at state level or policy level</param>
    private void BindListView(string detailLVL_in)
    {
        try
        {
            //we are not handling the default case as ideally there hsould be no such case,if it comes up it is
            //an prog error and we will not handle it.
            switch (detailLVL_in)
            {
                case "state":
                    SurAdjConcurntList = SurDTL.GetSurchargeDetailsList(this.PreAdjID, this.PrgPeriodID);
                    lstSurchargeReview.DataSource = SurAdjConcurntList;
                    lstSurchargeReview.DataBind();
                    lstSurchargeReview.Visible = true;
                    break;
                case "policy":
                    pnlPol.Visible = true;
                    lstPolReview.Visible = true;
                    lblLstPolReviewHeader.Visible = true;
                    lbClosePolicyDetails.Visible = true;

                    lstPolReview.DataSource = SurDTL.GetSurchargePolLvlDetailsList(this.PreAdjID, this.PrgPeriodID,
                                                                                    this.SurCodeID, this.StateID);
                    lstPolReview.DataBind();

                    lstSurchargeReview.Enabled = false;
                    break;
            }
        }
        catch (RetroBaseException bindException)
        {
            ShowError(bindException.Message);
        }
    }

    #endregion
    /// <summary>
    /// Item Data Bound Event-it is called while binding each row to the listview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region ListView DataBound Event
    protected void lstPolReview_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {

            //Restricting User to Update when the Adjustment is not in CALC Status
            int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lblItemEdit");
            if (AdjStatusTypeID != intAdjCalStatusID)
            {
                lnkEdit.Enabled = false;
            }

        }
    }


    #endregion
    #endregion

    #region EventHandlers

    #region Selected Index Changed Event of User Control
    /// <summary>
    /// Onselectedindexchanged event for the user control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ddlprogramPeriod_ARProgramPeriodSelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlAccnts = (DropDownList)this.ARS.FindControl("ddlAccountlist");
        DropDownList ddlValDates = (DropDownList)this.ARS.FindControl("ddlValDate");
        DropDownList ddlPrgPeriod = (DropDownList)this.ARS.FindControl("ddlProgramPeriod");
        DropDownList ddlAdjNumber = (DropDownList)this.ARS.FindControl("ddlAdjNumber");
        lblSurchargeReviewDetails.Text = string.Empty;
        lstSurchargeReview.Items.Clear();
        lstSurchargeReview.Visible = false;
        lstPolReview.Items.Clear();
        lstPolReview.Visible = false;
        lblLstPolReviewHeader.Visible = false;
        lbClosePolicyDetails.Visible = false;
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

    #region Search Button Click Event
    /// <summary>
    /// Search Button Click event for the user control
    /// By calling the Delegate we are calling this event of user control
    /// Search Parameters are Captured in this Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSearch_AdjustmentReviewSearchButtonClicked(object sender, EventArgs e)
    {
        this.lstSurchargeReview.Enabled = true;
        if (hidPopupflag.Value != "")
        {
            AdjustmentReviewCommentsBE AdjRevCmmntBE = new AdjustmentReviewCommentsBE();
            AdjRevCmmntBE = new AdjustmentReviewCommentsBS().getAdjReviewCmmntALLData(612, this.PremAdjPerdID, this.AccountID);


            if (AdjRevCmmntBE.CMMNT_CATG_ID != null)
            {
                txtComments.Text = AdjRevCmmntBE.CMMNT_TXT;
            }

            modalSave.Show();
        }
        else
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
            lblSurchargeReviewDetails.Text = "SURCHARGE AND ASSESSMENT REVIEW" + "" + "-" + ddlPrgPeriod.SelectedItem.ToString();
            BindListView("state");
        }
    }
    #endregion

    #region lstSurchargeReview Events

    #region Item Edit Event
    /// <summary>
    /// when the other amount button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstSurchargeReview_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        lstSurchargeReview.EditIndex = e.NewEditIndex;
        this.SurCodeID = SurAdjConcurntList[e.NewEditIndex].surchrg_cd_id.Value;
        this.StateID = SurAdjConcurntList[e.NewEditIndex].st_id.Value;
        BindListView("state");
        lblLstPolReviewHeader.Text = SurAdjConcurntList[e.NewEditIndex].Surcharge_Code;
        BindListView("policy");
    }


    #endregion

    #endregion

    #region lstPolReview Events

    #region Item Command Event
    /// <summary>
    /// ListView Item Command Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstPolReview_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        //we need to get data here and update so both logics are here instead of in iteam _updating event.
        //this is done to avoid futher confusdion.
        if (e.CommandName == "Update")
        {

            TextBox txtOtherAmt = (TextBox)e.Item.FindControl("txtOtherAmt");
            if (string.IsNullOrEmpty(txtOtherAmt.Text))
                txtOtherAmt.Text = "0";

            Label lblSurDTLID = (Label)e.Item.FindControl("lblSurDTLID");
            int intSurDtlID = Convert.ToInt32(lblSurDTLID.Text);
            SurchargeDetailAmountBE surDTLBE = new SurchargeDetailAmountBE();
            IList<SurchargeDetailAmountBE> aTempList = SurDTL.GetSurchargePolLvlDetailsList(this.PreAdjID, this.PrgPeriodID, this.SurCodeID, this.StateID);

            try
            {
                surDTLBE = (new SurchargeDetailsBS()).GetPremAdjPerdRow(intSurDtlID);
                foreach (SurchargeDetailAmountBE aTempBE in aTempList)
                {
                    if (aTempBE.prem_adj_surchrg_dtl_id == intSurDtlID)
                        surDTLBE.tot_addn_rtn = aTempBE.tot_addn_rtn;
                }

                surDTLBE.UPDTE_USER_ID = CurrentAISUser.PersonID;
                surDTLBE.UPDTE_DATE = DateTime.Now;
                this.SurCustID = surDTLBE.CUSTMR_ID;
                surDTLBE.tot_addn_rtn += Convert.ToDecimal(txtOtherAmt.Text) - surDTLBE.other_surchrg_amt.GetValueOrDefault(0);
                surDTLBE.other_surchrg_amt = Convert.ToDecimal(txtOtherAmt.Text);
                bool blupdate = (new SurchargeDetailsBS()).UpdateOtherAmounts(surDTLBE);
                (new SurchargeDetailsBS()).CalcSurchargeReview(this.PremAdjPerdID, this.PreAdjID, this.AccountID, this.PrgPeriodID,
                                                                surDTLBE.tot_addn_rtn.Value, surDTLBE.st_id.Value,
                                                                surDTLBE.ln_of_bsn_id.Value, surDTLBE.coml_agmt_id, SurCodeID,
                                                                surDTLBE.surchrg_type_id.Value, surDTLBE.CRTE_USER_ID, true);



            }
            catch (RetroBaseException ee)
            {
                ShowError(ee.Message, ee);
            }
            lstPolReview.EditIndex = -1;
            BindListView("state");
            BindListView("policy");
        }
    }

    #endregion

    #region ItemUpdating event
    /// <summary>
    /// Item Updating Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstPolReview_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {



        BindListView("policy");
    }
    #endregion

    #region ItemEdit Event
    /// <summary>
    /// Item Edit Event of a ListView
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstPolReview_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        BindListView("state");
        lstPolReview.EditIndex = e.NewEditIndex;
        pnlPol.Visible = true;
        lstPolReview.DataSource = SurDTL.GetSurchargePolLvlDetailsList(this.PreAdjID, this.PrgPeriodID, this.SurCodeID, this.StateID);
        lstPolReview.DataBind();
        BindListView("policy");

    }
    #endregion

    #region ItemCancel Event
    /// <summary>
    /// Item Cancel Event of ListView
    /// When User clicks the cancel button of the listview this event is fired
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstPolReview_ItemCancel(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstPolReview.EditIndex = -1;
            BindListView("policy");

        }
    }
    #endregion

    #endregion

    protected void lbClosePolicyDetails_Click(object sender, EventArgs e)
    {
        this.lstSurchargeReview.Enabled = true;
        this.lblLstPolReviewHeader.Visible = false;
        this.lstPolReview.Visible = false;
        this.lbClosePolicyDetails.Visible = false;

    }

    #endregion

    protected void btnPopup_Click(object sender, EventArgs e)
    {
        AdjustmentReviewCommentsBE AdjRevCmmntBE = new AdjustmentReviewCommentsBE();
        AdjRevCmmntBE = new AdjustmentReviewCommentsBS().getAdjReviewCmmntALLData(612, this.PremAdjPerdID, this.AccountID);


        if (AdjRevCmmntBE.CMMNT_CATG_ID != null)
        {
            txtComments.Text = AdjRevCmmntBE.CMMNT_TXT;
        }

        modalSave.Show();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        AdjustmentReviewCommentsBE AdjRevCmmntBE = new AdjustmentReviewCommentsBE();

        AdjRevCmmntBE = new AdjustmentReviewCommentsBS().getAdjReviewCmmntALLData(612, this.PremAdjPerdID, this.AccountID);


        if (AdjRevCmmntBE.CMMNT_CATG_ID == null)
        {
            AdjRevCmmntBE = new AdjustmentReviewCommentsBE();
            AdjRevCmmntBE.CREATEUSER = CurrentAISUser.PersonID;
            AdjRevCmmntBE.CREATEDATE = DateTime.Now;

            AdjRevCmmntBE.PREM_ADJ_ID = this.PreAdjID;
            AdjRevCmmntBE.PREM_ADJ_PERD_ID = this.PremAdjPerdID;
            AdjRevCmmntBE.CUSTMR_ID = this.AccountID;
            AdjRevCmmntBE.CMMNT_CATG_ID = 612;
            AdjRevCmmntBE.CMMNT_TXT = txtComments.Text;
            new AdjustmentReviewCommentsBS().Update(AdjRevCmmntBE);
            // AdjRevCmmntBE = AdjRevCmmntBS.getAdjReviewCmmntALLData(122, PremAdjPeriodID, intAccountID);
        }


        else
        {
            AdjustmentReviewCommentsBE AdjRevCmmntBEold = new AdjustmentReviewCommentsBS().getAdjustmentReviewCommentsRow(AdjRevCmmntBE.PREM_ADJ_CMMNT_ID);

            AdjRevCmmntBEold.UPDATEDUSER = CurrentAISUser.PersonID;
            AdjRevCmmntBEold.UPDATEDDATE = DateTime.Now;

            AdjRevCmmntBEold.PREM_ADJ_ID = this.PreAdjID;
            AdjRevCmmntBEold.PREM_ADJ_PERD_ID = this.PremAdjPerdID;
            AdjRevCmmntBEold.CUSTMR_ID = this.AccountID;
            AdjRevCmmntBEold.CMMNT_CATG_ID = 612;
            AdjRevCmmntBEold.CMMNT_TXT = txtComments.Text;
            new AdjustmentReviewCommentsBS().Update(AdjRevCmmntBEold);


        }
        if (txtPageURL.Text != "")
        {
            //bool isValidURL = System.Text.RegularExpressions.Regex.IsMatch(txtPageURL.Text, @"^(((ht|f)tp(s?))\://)?((([a-zA-Z0-9_\-]{2,}\.)+[a-zA-Z]{2,})|((?:(?:25[0-5]|2[0-4]\d|[01]\d\d|\d?\d)(?(\.?\d)\.)){4}))(:[a-zA-Z0-9]+)?(/[a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~]*)?$");
            //if (isValidURL)
            //{
            //    string url = txtPageURL.Text;
            //    //Response.Redirect(url);
            //}
            //EAISA-5 Veracode flaw fix 12082017
            if (Uri.IsWellFormedUriString(txtPageURL.Text, UriKind.RelativeOrAbsolute))
            {
                Uri result;
                if (Uri.TryCreate(System.Web.HttpUtility.HtmlEncode(txtPageURL.Text), UriKind.RelativeOrAbsolute, out result))
                {
                    //Response.Redirect(result.AbsoluteUri);
                    ResponseRedirect(System.Web.HttpUtility.HtmlEncode(result.AbsoluteUri));
                }
            }
        }
    }
}
#endregion

#endregion
