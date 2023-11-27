//Default namespaces in NY-SIF screen
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
//Importing different AIS framework namespaces for NY-SIF screen
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.AIS.DAL.Logic;

/// <summary>
/// This is the class which is used to give the information about the NY-SIF and it 
/// also inherits AISBase Page,so that some of the common functionality needed for all pages is implemented in
/// the Basepage
/// </summary>
#region NY-SIF Class
public partial class AdjCalc_NY_SIF : AISBasePage
{
    /// <summary>
    /// Page load Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        //User Control Event Handlers
        this.ARS.AdjustmentReviewSearchButtonClicked += new EventHandler(btnSearch_AdjustmentReviewSearchButtonClicked);
        this.ARS.ARProgramPeriodSelectedIndexChanged += new EventHandler(ddlprogramPeriod_ARProgramPeriodSelectedIndexChanged);

        // To display the title of the page
        this.Master.Page.Title = "NY-SIF";
        if (!IsPostBack)
        {
            pnlDetails.Visible = false;
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
                    if(strSelectedAccountID!="" && strSelectedValDate!="" && strSelectedPremAdjID!="" && strSelectedProgramPeriod!="")
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

        //Checks Exiting without Save
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
        list.Add(txtBasic);
        list.Add(txtBasicAndConvertedLosses);
        list.Add(txtConvertedLosses);
        list.Add(txtConvertedTaxesLosses);
        list.Add(txtCurrentAdj);
        list.Add(txtLCF);
        list.Add(txtNYEarnedRetroPremium);
        list.Add(txtNYIncurredLosses);
        list.Add(txtNYPremiumDiscount);
        list.Add(txtNYScndInjrFundFctr);
        list.Add(txtNYScndInjronAudit);
        list.Add(txtNYTaxDue);
        list.Add(txtPreviousResult);
        list.Add(txtRevisedNYScndInjrFund);
        list.Add(txtTaxMultiplier);
        list.Add(ckkboxlstPolicyno);
        list.Add(btnNYSIFClear);
        list.Add(btnNYSIFSave);

        ProcessExitFlag(list);
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
    /// a property for PremiumAdjNYScndInjuryFund Business Service Class
    /// </summary>
    /// <returns>PremiumAdjNYScndInjuryFundBS</returns>
    private PremiumAdjNYScndInjuryFundBS prmAdjNYSIFBS;
    private PremiumAdjNYScndInjuryFundBS PrmAdjNYSIFBS
    {
        get
        {
            if (prmAdjNYSIFBS == null)
            {
                prmAdjNYSIFBS = new PremiumAdjNYScndInjuryFundBS();
            }
            return prmAdjNYSIFBS;
        }
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
        if(strSelectedValDate!="")
           dtValDate = Convert.ToDateTime(strSelectedValDate);
        string strProgramPeriod = strSelectedProgramPeriod;
        int intPremAdjPerdID = 0;
        int intPrgPeriodID = 0;
        if(strSelectedProgramPeriod!="")
                intPrgPeriodID=Convert.ToInt32(strSelectedProgramPeriod);
       
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
        //IList<PremiumAdjNYScndInjuryFundBE> lstPreAdjNYSIF = new List<PremiumAdjNYScndInjuryFundBE>();
        lstPreAdjNYSIF = PrmAdjNYSIFBS.getPremAdjNYSIF(this.AccountID, this.PreAdjID, this.PremAdjPerdID, this.PrgPeriodID);
        BindPolicyListBox();
        if (lstPreAdjNYSIF.Count > 0)
        {
            btnNYSIFSave.Text = "Update";
            btnNYSIFSave.ToolTip = "Click here to Update";
            filNYSIFList(lstPreAdjNYSIF);
        }
        else
        {
            btnNYSIFSave.Text = "Save";
            btnNYSIFSave.ToolTip = "Click here to Save";
            ClearData();
        }
        //Restricting User to Add or Update if Adjustment Status is not CALC
        int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
        if (intAdjCalStatusID != AdjStatusTypeID)
        {
            btnNYSIFSave.Enabled = false;
        }
        else
        {
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                btnNYSIFSave.Enabled = true;
        }
        IList<ProgramPeriodSearchListBE> premAdjPgmBE = new List<ProgramPeriodSearchListBE>();
        premAdjPgmBE = new ProgramPeriodsBS().GetProgramPeriodsList(strSelectedProgramPeriod);
        lblPremAdjNYSIFDetails.Text = "NY-SIF" + "" + "-" + premAdjPgmBE[0].STARTDATE_ENDDATE_PGMTYP.ToString();
        pnlDetails.Visible = true;


    }
    #endregion

    #region Maintaining Session for PremiumAdjNYScndInjuryFundBE
    private IList<PremiumAdjNYScndInjuryFundBE> lstPreAdjNYSIF
    {
        get
        {
            if (Session["lstPreAdjNYSIF"] == null)
                Session["lstPreAdjNYSIF"] = new List<PremiumAdjNYScndInjuryFundBE>();
            return (IList<PremiumAdjNYScndInjuryFundBE>)Session["lstPreAdjNYSIF"];
        }
        set { Session["lstPreAdjNYSIF"] = value; }
    }
    #endregion
    /// <summary>
    /// Button Click Event to Save the Ny-SIF Data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Save Button Click Event
    protected void btnNYSIFSave_Click(object sender, EventArgs e)
    {
        if (btnNYSIFSave.Text.ToUpper() == "SAVE")
        {
            //Save Record
            SaveData();
        }
        else
        {
            //Update Record
            UpdateData();
        }

    }

    private void UpdateData()
    {
        PremiumAdjNYScndInjuryFundBE premAdjNYSIF = new PremiumAdjNYScndInjuryFundBE();
        premAdjNYSIF = PrmAdjNYSIFBS.getPremAdjNYSIFRow(Convert.ToInt32(ViewState["PrmAdjNYSIFID"].ToString()));
        //Concurrency Issue
        PremiumAdjNYScndInjuryFundBE premAdjNYSIFold = (lstPreAdjNYSIF.Where(o => o.PREM_ADJ_NY_SCND_INJR_FUND_ID.Equals(Convert.ToInt32(ViewState["PrmAdjNYSIFID"].ToString())))).First();
        bool con = ShowConcurrentConflict(Convert.ToDateTime(premAdjNYSIFold.UPDATE_DATE), Convert.ToDateTime(premAdjNYSIF.UPDATE_DATE));
        if (!con)
            return;
        //End
        premAdjNYSIF.COML_AGMT_ID = Convert.ToInt32(ckkboxlstPolicyno.SelectedValue);
        premAdjNYSIF.INCUR_LOS_AMT = Convert.ToDecimal(txtNYIncurredLosses.Text);
        if (txtLCF.Text != "")
        {
            premAdjNYSIF.LOS_CONV_FCTR_RT = Convert.ToDecimal(txtLCF.Text);
        }
        else
        {
            premAdjNYSIF.LOS_CONV_FCTR_RT = Convert.ToDecimal("0.000000");
        }
        if (txtConvertedLosses.Text.Replace('-', ' ').Trim().Split('.')[0].Length < 14)
        {
            premAdjNYSIF.CNVT_LOS_AMT = Convert.ToDecimal(txtConvertedLosses.Text);
        }
        else
        {
            ShowError("Converted Losses amount exceeds maximum allowed amount");
            return;

        }
        premAdjNYSIF.BASIC_DEDTBL_PREM_AMT = Convert.ToDecimal(txtBasic.Text);
        if (txtTaxMultiplier.Text != "")
        {
            premAdjNYSIF.TAX_MULTI_RT = Convert.ToDecimal(txtTaxMultiplier.Text);
        }
        else
        {
            premAdjNYSIF.TAX_MULTI_RT = Convert.ToDecimal("0.000000");
        }
        if (txtConvertedTaxesLosses.Text.Replace('-', ' ').Trim().Split('.')[0].Length < 14)
        {
            premAdjNYSIF.CNVT_TOT_LOS_AMT = Convert.ToDecimal(txtConvertedTaxesLosses.Text);
        }
        else
        {
            ShowError("Converted Taxes Losses amount exceeds maximum allowed amount");
            return;

        }

        premAdjNYSIF.NY_PREM_DISC_AMT = Convert.ToDecimal(txtNYPremiumDiscount.Text);
        if (txtNYScndInjrFundFctr.Text != "")
        {
            premAdjNYSIF.NY_SCND_INJR_FUND_RT = Convert.ToDecimal(txtNYScndInjrFundFctr.Text);
        }
        else
        {
            premAdjNYSIF.NY_SCND_INJR_FUND_RT = Convert.ToDecimal("00.000000");
        }
        if (txtRevisedNYScndInjrFund.Text.Replace('-', ' ').Trim().Split('.')[0].Length < 14)
        {
            premAdjNYSIF.REVD_NY_SCND_INJR_FUND_AMT = Convert.ToDecimal(txtRevisedNYScndInjrFund.Text);
        }
        else
        {
            ShowError("Revised NY Second Injury Fund amount exceeds maximum allowed amount");
            return;

        }

        if (txtNYTaxDue.Text.Replace('-', ' ').Trim().Split('.')[0].Length < 14)
        {
            premAdjNYSIF.NY_TAX_DUE_AMT = Convert.ToDecimal(txtNYTaxDue.Text);
        }
        else
        {
            ShowError("NY Tax Due amount exceeds maximum allowed amount");
            return;

        }

        premAdjNYSIF.PREV_RSLT_AMT = Convert.ToDecimal(txtPreviousResult.Text);
        premAdjNYSIF.NY_SCND_INJR_FUND_AUDT_AMT = Convert.ToDecimal(txtNYScndInjronAudit.Text);
        if (txtCurrentAdj.Text.Replace('-', ' ').Trim().Split('.')[0].Length < 14)
        {
            premAdjNYSIF.CURR_ADJ_AMT = Convert.ToDecimal(txtCurrentAdj.Text);
        }
        else
        {
            ShowError("Current Adjustment amount exceeds maximum allowed amount");
            return;

        }
        if (txtBasicAndConvertedLosses.Text.Replace('-', ' ').Trim().Split('.')[0].Length < 14)
        {
            premAdjNYSIF.BASIC_CNVT_LOS_AMT = Convert.ToDecimal(txtBasicAndConvertedLosses.Text);
        }
        else
        {
            ShowError("Basic+Converted Losses amount exceeds maximum allowed amount");
            return;

        }


        if (txtNYEarnedRetroPremium.Text.Replace('-', ' ').Trim().Split('.')[0].Length < 14)
        {
            premAdjNYSIF.NY_ERND_RETRO_PREM_AMT = Convert.ToDecimal(txtNYEarnedRetroPremium.Text);
        }
        else
        {
            ShowError("NY Earned Retro Premium amount exceeds maximum allowed amount");
            return;

        }

        premAdjNYSIF.UPDATE_USER_ID = CurrentAISUser.PersonID;
        premAdjNYSIF.UPDATE_DATE = DateTime.Now;
        bool i = PrmAdjNYSIFBS.Update(premAdjNYSIF);
        ShowConcurrentConflict(i, premAdjNYSIF.ErrorMessage);
        if (i == false)
            ShowError("Overall total amount exceeds maximum allowed amount");
        else
        {
            btnNYSIFSave.Text = "Update";
            btnNYSIFSave.ToolTip = "Click here to Update";
            (new PremiumAdjustmentPeriodBS()).deletePremAdjPerdTotal(this.PremAdjPerdID, this.PreAdjID, this.AccountID);
            (new PremiumAdjustmentPeriodBS()).AddPremAdjPerdTotal(this.PremAdjPerdID, this.PreAdjID, this.AccountID, this.PrgPeriodID, CurrentAISUser.PersonID);
            //IList<PremiumAdjNYScndInjuryFundBE> lstPreAdjNYSIF = new List<PremiumAdjNYScndInjuryFundBE>();
            lstPreAdjNYSIF = PrmAdjNYSIFBS.getPremAdjNYSIF(this.AccountID, this.PreAdjID, this.PremAdjPerdID, this.PrgPeriodID);
            filNYSIFList(lstPreAdjNYSIF);
        }

    }
    #endregion
    /// <summary>
    /// Function to Save the Record in Prem Adj NYSIF Table
    /// </summary>
    #region SaveData
    private void SaveData()
    {
        lstPreAdjNYSIF = PrmAdjNYSIFBS.getPremAdjNYSIF(this.AccountID, this.PreAdjID, this.PremAdjPerdID, this.PrgPeriodID);
        if (lstPreAdjNYSIF.Count > 0)
        {
                ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
                return;
        }
        PremiumAdjNYScndInjuryFundBE premAdjNYSIF = new PremiumAdjNYScndInjuryFundBE();
        premAdjNYSIF.PREM_ADJ_PERD_ID = this.PremAdjPerdID;
        premAdjNYSIF.PREM_ADJ_ID = this.PreAdjID;
        premAdjNYSIF.CUSTMR_ID = this.AccountID;
        premAdjNYSIF.PREM_ADJ_PGM_ID = this.PrgPeriodID;
        premAdjNYSIF.COML_AGMT_ID = Convert.ToInt32(ckkboxlstPolicyno.SelectedValue);
        premAdjNYSIF.INCUR_LOS_AMT = Convert.ToDecimal(txtNYIncurredLosses.Text);
        if (txtLCF.Text != "")
        {
            premAdjNYSIF.LOS_CONV_FCTR_RT = Convert.ToDecimal(txtLCF.Text);
        }
        else
        {
            premAdjNYSIF.LOS_CONV_FCTR_RT = Convert.ToDecimal("0.000000");
        }
        if (txtConvertedLosses.Text.Replace('-', ' ').Trim().Split('.')[0].Length < 14)
        {
            premAdjNYSIF.CNVT_LOS_AMT = Convert.ToDecimal(txtConvertedLosses.Text);
        }
        else
        {
            ShowError("Converted Losses amount exceeds maximum allowed amount");
            return;

        }
        premAdjNYSIF.BASIC_DEDTBL_PREM_AMT = Convert.ToDecimal(txtBasic.Text);
        if (txtTaxMultiplier.Text != "")
        {
            premAdjNYSIF.TAX_MULTI_RT = Convert.ToDecimal(txtTaxMultiplier.Text);
        }
        else
        {
            premAdjNYSIF.TAX_MULTI_RT = Convert.ToDecimal("0.000000");
        }
        if (txtConvertedTaxesLosses.Text.Replace('-', ' ').Trim().Split('.')[0].Length < 14)
        {
            premAdjNYSIF.CNVT_TOT_LOS_AMT = Convert.ToDecimal(txtConvertedTaxesLosses.Text);
        }
        else
        {
            ShowError("Converted Taxes Losses amount exceeds maximum allowed amount");
            return;

        }

        premAdjNYSIF.NY_PREM_DISC_AMT = Convert.ToDecimal(txtNYPremiumDiscount.Text);
        if (txtNYScndInjrFundFctr.Text != "")
        {
            premAdjNYSIF.NY_SCND_INJR_FUND_RT = Convert.ToDecimal(txtNYScndInjrFundFctr.Text);
        }
        else
        {
            premAdjNYSIF.NY_SCND_INJR_FUND_RT = Convert.ToDecimal("00.000000");
        }
        if (txtRevisedNYScndInjrFund.Text.Replace('-', ' ').Trim().Split('.')[0].Length < 14)
        {
            premAdjNYSIF.REVD_NY_SCND_INJR_FUND_AMT = Convert.ToDecimal(txtRevisedNYScndInjrFund.Text);
        }
        else
        {
            ShowError("Revised NY Second Injury Fund amount exceeds maximum allowed amount");
            return;

        }
        if (txtNYTaxDue.Text.Replace('-', ' ').Trim().Split('.')[0].Length < 14)
        {
            premAdjNYSIF.NY_TAX_DUE_AMT = Convert.ToDecimal(txtNYTaxDue.Text);
        }
        else
        {
            ShowError("NY Tax Due amount exceeds maximum allowed amount");
            return;

        }


        premAdjNYSIF.PREV_RSLT_AMT = Convert.ToDecimal(txtPreviousResult.Text);
        premAdjNYSIF.NY_SCND_INJR_FUND_AUDT_AMT = Convert.ToDecimal(txtNYScndInjronAudit.Text);
        if (txtCurrentAdj.Text.Replace('-', ' ').Trim().Split('.')[0].Length < 14)
        {
            premAdjNYSIF.CURR_ADJ_AMT = Convert.ToDecimal(txtCurrentAdj.Text);
        }
        else
        {
            ShowError("Current Adjustment amount exceeds maximum allowed amount");
            return;

        }
        if (txtBasicAndConvertedLosses.Text.Replace('-', ' ').Trim().Split('.')[0].Length < 14)
        {
            premAdjNYSIF.BASIC_CNVT_LOS_AMT = Convert.ToDecimal(txtBasicAndConvertedLosses.Text);
        }
        else
        {
            ShowError("Basic+Converted Losses amount exceeds maximum allowed amount");
            return;

        }


        if (txtNYEarnedRetroPremium.Text.Replace('-', ' ').Trim().Split('.')[0].Length < 14)
        {
            premAdjNYSIF.NY_ERND_RETRO_PREM_AMT = Convert.ToDecimal(txtNYEarnedRetroPremium.Text);
        }
        else
        {
            ShowError("NY Earned Retro Premium amount exceeds maximum allowed amount");
            return;

        }



        premAdjNYSIF.CREATE_USER_ID = CurrentAISUser.PersonID;
        premAdjNYSIF.CREATE_DATE = DateTime.Now;
        bool i = PrmAdjNYSIFBS.Update(premAdjNYSIF);
        if (i == false)
            ShowError("Overall total amount exceeds maximum allowed amount");
        else
        {
            btnNYSIFSave.Text = "Update";
            btnNYSIFSave.ToolTip = "Click here to Update";
            (new PremiumAdjustmentPeriodBS()).deletePremAdjPerdTotal(this.PremAdjPerdID, this.PreAdjID, this.AccountID);
            (new PremiumAdjustmentPeriodBS()).AddPremAdjPerdTotal(this.PremAdjPerdID, this.PreAdjID, this.AccountID, this.PrgPeriodID, CurrentAISUser.PersonID);
            //IList<PremiumAdjNYScndInjuryFundBE> lstPreAdjNYSIF = new List<PremiumAdjNYScndInjuryFundBE>();
            lstPreAdjNYSIF = PrmAdjNYSIFBS.getPremAdjNYSIF(this.AccountID, this.PreAdjID, this.PremAdjPerdID, this.PrgPeriodID);
            filNYSIFList(lstPreAdjNYSIF);
        }

    }
    #endregion


    /// <summary>
    /// Button Click Event to Clear the NY-SIF Data Entered
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Clear Button Click Event
    protected void btnNYSIFClear_Click(object sender, EventArgs e)
    {
        ClearData();
    }
    #endregion
    /// <summary>
    /// Function To Clear the Fileds in NY-SIF Web Page
    /// </summary>
    #region ClearData Function
    private void ClearData()
    {
        //this.lbPolicy.SelectedValue = null;
        this.ckkboxlstPolicyno.SelectedValue = null;
        this.txtNYIncurredLosses.Text = string.Empty;
        this.txtLCF.Text = string.Empty;
        this.txtConvertedLosses.Text = string.Empty;
        this.txtBasic.Text = string.Empty;
        this.txtBasicAndConvertedLosses.Text = string.Empty;
        this.txtTaxMultiplier.Text = string.Empty;
        this.txtConvertedTaxesLosses.Text = string.Empty;
        this.txtNYPremiumDiscount.Text = string.Empty;
        this.txtNYEarnedRetroPremium.Text = string.Empty;
        this.txtNYScndInjrFundFctr.Text = string.Empty;
        this.txtRevisedNYScndInjrFund.Text = string.Empty;
        this.txtNYScndInjronAudit.Text = string.Empty;
        this.txtNYTaxDue.Text = string.Empty;
        this.txtPreviousResult.Text = string.Empty;
        this.txtCurrentAdj.Text = string.Empty;
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
        //IList<PremiumAdjNYScndInjuryFundBE> lstPreAdjNYSIF = new List<PremiumAdjNYScndInjuryFundBE>();
        lstPreAdjNYSIF = PrmAdjNYSIFBS.getPremAdjNYSIF(this.AccountID, this.PreAdjID, this.PremAdjPerdID, this.PrgPeriodID);
        BindPolicyListBox();
        if (lstPreAdjNYSIF.Count > 0)
        {
            btnNYSIFSave.Text = "Update";
            btnNYSIFSave.ToolTip = "Click here to Update";
            filNYSIFList(lstPreAdjNYSIF);
        }
        else
        {
            btnNYSIFSave.Text = "Save";
            btnNYSIFSave.ToolTip = "Click here to Save";
            ClearData();
        }
        //Restricting User to Add or Update if Adjustment Status is not CALC
        int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
        if (intAdjCalStatusID != AdjStatusTypeID)
        {
            btnNYSIFSave.Enabled = false;
        }
        else
        {
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                btnNYSIFSave.Enabled = true;
        }
        lblPremAdjNYSIFDetails.Text = "NY-SIF" + "" + "-" + ddlPrgPeriod.SelectedItem.ToString();
        pnlDetails.Visible = true;


    }
    #endregion
    /// <summary>
    /// Function to fill the NY-SIF form
    /// </summary>
    /// <param name="lstPreAdjNYSIF"></param>
    #region filNYSIFList
    private void filNYSIFList(IList<PremiumAdjNYScndInjuryFundBE> lstPreAdjNYSIF)
    {
        ViewState["PrmAdjNYSIFID"] = lstPreAdjNYSIF[0].PREM_ADJ_NY_SCND_INJR_FUND_ID.ToString();
        if (lstPreAdjNYSIF[0].COML_AGMT_ID.HasValue)
            //            this.ckkboxlstPolicyno.Items.FindByValue(lstPreAdjNYSIF[0].COML_AGMT_ID.ToString()).Selected = true;
            AddInActiveLookupData(ref this.ckkboxlstPolicyno, lstPreAdjNYSIF[0].COML_AGMT_ID.Value);
        this.txtNYIncurredLosses.Text = lstPreAdjNYSIF[0].INCUR_LOS_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjNYSIF[0].INCUR_LOS_AMT.ToString())).ToString("#,##0") : lstPreAdjNYSIF[0].INCUR_LOS_AMT.ToString();
        this.txtLCF.Text = Convert.ToDecimal(lstPreAdjNYSIF[0].LOS_CONV_FCTR_RT).ToString("0.000000");
        this.txtConvertedLosses.Text = lstPreAdjNYSIF[0].CNVT_LOS_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjNYSIF[0].CNVT_LOS_AMT.ToString())).ToString("#,##0") : lstPreAdjNYSIF[0].CNVT_LOS_AMT.ToString();
        this.txtBasic.Text = lstPreAdjNYSIF[0].BASIC_DEDTBL_PREM_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjNYSIF[0].BASIC_DEDTBL_PREM_AMT.ToString())).ToString("#,##0") : lstPreAdjNYSIF[0].BASIC_DEDTBL_PREM_AMT.ToString();
        this.txtBasicAndConvertedLosses.Text = lstPreAdjNYSIF[0].BASIC_CNVT_LOS_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjNYSIF[0].BASIC_CNVT_LOS_AMT.ToString())).ToString("#,##0") : lstPreAdjNYSIF[0].BASIC_CNVT_LOS_AMT.ToString();
        this.txtTaxMultiplier.Text = Convert.ToDecimal(lstPreAdjNYSIF[0].TAX_MULTI_RT).ToString("0.000000");
        this.txtConvertedTaxesLosses.Text = lstPreAdjNYSIF[0].CNVT_TOT_LOS_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjNYSIF[0].CNVT_TOT_LOS_AMT.ToString())).ToString("#,##0") : lstPreAdjNYSIF[0].CNVT_TOT_LOS_AMT.ToString();
        this.txtNYPremiumDiscount.Text = lstPreAdjNYSIF[0].NY_PREM_DISC_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjNYSIF[0].NY_PREM_DISC_AMT.ToString())).ToString("#,##0") : lstPreAdjNYSIF[0].NY_PREM_DISC_AMT.ToString();
        this.txtNYEarnedRetroPremium.Text = lstPreAdjNYSIF[0].NY_ERND_RETRO_PREM_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjNYSIF[0].NY_ERND_RETRO_PREM_AMT.ToString())).ToString("#,##0") : lstPreAdjNYSIF[0].NY_ERND_RETRO_PREM_AMT.ToString();
        this.txtNYScndInjrFundFctr.Text = Convert.ToDecimal(lstPreAdjNYSIF[0].NY_SCND_INJR_FUND_RT).ToString("00.000000");
        this.txtRevisedNYScndInjrFund.Text = lstPreAdjNYSIF[0].REVD_NY_SCND_INJR_FUND_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjNYSIF[0].REVD_NY_SCND_INJR_FUND_AMT.ToString())).ToString("#,##0") : lstPreAdjNYSIF[0].REVD_NY_SCND_INJR_FUND_AMT.ToString();
        this.txtNYScndInjronAudit.Text = lstPreAdjNYSIF[0].NY_SCND_INJR_FUND_AUDT_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjNYSIF[0].NY_SCND_INJR_FUND_AUDT_AMT.ToString())).ToString("#,##0") : lstPreAdjNYSIF[0].NY_SCND_INJR_FUND_AUDT_AMT.ToString();
        this.txtNYTaxDue.Text = lstPreAdjNYSIF[0].NY_TAX_DUE_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjNYSIF[0].NY_TAX_DUE_AMT.ToString())).ToString("#,##0") : lstPreAdjNYSIF[0].NY_TAX_DUE_AMT.ToString();
        this.txtPreviousResult.Text = lstPreAdjNYSIF[0].PREV_RSLT_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjNYSIF[0].PREV_RSLT_AMT.ToString())).ToString("#,##0") : lstPreAdjNYSIF[0].PREV_RSLT_AMT.ToString();
        this.txtCurrentAdj.Text = lstPreAdjNYSIF[0].CURR_ADJ_AMT.ToString() != "" ? (decimal.Parse(lstPreAdjNYSIF[0].CURR_ADJ_AMT.ToString())).ToString("#,##0") : lstPreAdjNYSIF[0].CURR_ADJ_AMT.ToString();
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
        //To hide the fields in each drop down selected index change
        lblPremAdjNYSIFDetails.Text = string.Empty;
        pnlDetails.Visible = false;
        ckkboxlstPolicyno.Items.Clear();

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
    /// Function to bind listbox with policies
    /// </summary>
    #region BindPolicyListBox
    private void BindPolicyListBox()
    {
        IList<PolicyBE> PlcyLst = new PolicyBS().getPolicies(this.PrgPeriodID, this.AccountID);
        ckkboxlstPolicyno.DataSource = PlcyLst;
        ckkboxlstPolicyno.DataTextField = "PolicyNumber";
        ckkboxlstPolicyno.DataValueField = "PolicyID";
        ckkboxlstPolicyno.DataBind();
        if (PlcyLst.Count > 2) pnlPolicyNumberListNYSIF.Height = Unit.Point(50);
        if (PlcyLst.Count == 1) pnlPolicyNumberListNYSIF.Height = Unit.Point(20);
        if (PlcyLst.Count == 2) pnlPolicyNumberListNYSIF.Height = Unit.Point(37);
        //lbPolicy.DataSource = PlcyLst;
        //lbPolicy.DataTextField = "PolicyNumber";
        //lbPolicy.DataValueField = "PolicyID";
        //lbPolicy.DataBind();
        for (int i = 0; i < ckkboxlstPolicyno.Items.Count; i++)
        {
            ckkboxlstPolicyno.Items[i].Attributes.Add("onclick", "CheckBoxListSelect('" + ckkboxlstPolicyno.ClientID + "','" + i + "')");
        }
    }
    #endregion


}
#endregion