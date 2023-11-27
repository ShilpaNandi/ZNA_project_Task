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

using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.ExceptionHandling;
using System.Collections.Generic;
using ZurichNA.AIS.DAL.LINQ;




public partial class LossFundInfo : AISBasePage
{
    System.Data.Common.DbTransaction trans_tax = null;
    AISDatabaseLINQDataContext objDCTax = new AISDatabaseLINQDataContext(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
    private Adj_Parameter_SetupBS AdjparamsetupInfo = new Adj_Parameter_SetupBS();
    private Adj_Paramet_PolBS AdjParamPolInfo = new Adj_Paramet_PolBS();
    private BLAccess ESCROWblaccess = new BLAccess();

    #region Properties
    //PGM_PERD_ID
    private int prmPerdID;
    public int PrmPerdID
    {
        get
        {
            if (prmPerdID == 0)
            {
                return 1;
            }
            else
                return prmPerdID;
        }
        set
        {
            prmPerdID = value;
        }
    }

    //protected AISBusinessTransaction AdjParameterTransactionWrapper
    //{
    //    get
    //    {
    //        if ((AISBusinessTransaction)Session["AdjParameterTransaction"] == null)
    //            Session["AdjParameterTransaction"] = new AISBusinessTransaction();

    //        return (AISBusinessTransaction)Session["AdjParameterTransaction"];
    //    }
    //    set
    //    {
    //        Session["AdjParameterTransaction"] = value;
    //    }
    //}

    Adj_Parameter_SetupBS adjPrmStupBS;
    private Adj_Parameter_SetupBS AdjPrmStupBS
    {
        get
        {
            if (adjPrmStupBS == null)
            {
                adjPrmStupBS = new Adj_Parameter_SetupBS();
                //adjPrmStupBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
            }
            return adjPrmStupBS;
        }
        set
        {
            adjPrmStupBS = value;
        }
    }

    AdjustmentParameterSetupBE adjParmStupBE;
    private AdjustmentParameterSetupBE AdjParmStupBE
    {
        get
        {
            if (adjParmStupBE == null)
            {
                adjParmStupBE = new AdjustmentParameterSetupBE();
            }
            return adjParmStupBE;
        }
        set
        {
            adjParmStupBE = value;
        }
    }
    private IList<ILRFTaxSetupBE> ILRFTaxSetuplist
    {
        get
        {
            //if (Session["ILRFTaxSetuplist"] == null)
            //    Session["ILRFTaxSetuplist"] = new List<ILRFTaxSetupBE>();
            //return (IList<ILRFTaxSetupBE>)Session["ILRFTaxSetuplist"];
            if (RetrieveObjectFromSessionUsingWindowName("ILRFTaxSetuplist") == null)
                SaveObjectToSessionUsingWindowName("ILRFTaxSetuplist", new List<ILRFTaxSetupBE>());
            return (IList<ILRFTaxSetupBE>)RetrieveObjectFromSessionUsingWindowName("ILRFTaxSetuplist");
        }
        set
        {
            //Session["ILRFTaxSetuplist"] = value;
            SaveObjectToSessionUsingWindowName("ILRFTaxSetuplist", value);
        }
    }

    /// <summary>
    /// A Property for Holding StateID in ViewState
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
        }
    }
    /// <summary>
    /// A Property for Holding StateID in ViewState
    /// </summary>
    protected string CommandName
    {
        get
        {
            if (ViewState["COMMAND"] != null)
            {
                return ViewState["COMMAND"].ToString();
            }
            else
            {
                ViewState["COMMAND"] = "";
                return "";
            }
        }
        set
        {
            ViewState["COMMAND"] = value;
        }
    }
    #endregion

    #region Control States

    protected void Page_Init(object sender, EventArgs e)
    {
        this.Page.RegisterRequiresControlState(this);


    }
    protected override void LoadControlState(object savedState)
    {
        int stateIdx = 0;
        object[] ctlState = (object[])savedState;
        base.LoadControlState(ctlState[stateIdx++]);
        this.PrmPerdID = (int)ctlState[stateIdx++];


    }

    protected override object SaveControlState()
    {

        int stateIdx = 0;
        object[] ctlState = new object[2];
        ctlState[stateIdx++] = base.SaveControlState();
        ctlState[stateIdx++] = this.PrmPerdID;

        return ctlState;
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        this.uc2ProgramPeriod.OnItemCommand += new App_Shared_ProgramPeriod.ItemCommand(ProgramPeriod_ItemCommand);
        if (!Page.IsPostBack)
        {
            this.Master.Page.Title = "AdjParams/ILRFSetup";

            //lblcheckvalidations.Visible = false;
            //lblcheckvalidations.Text = "";
            //AdjParameterTransactionWrapper = new AISBusinessTransaction();

        }

        //Checks Exiting without Save
        CheckWithoutSave();

    }

    private void CheckWithoutSave()
    {
        ArrayList list = new ArrayList();
        list.Add(txtAggregateLimit);
        list.Add(txtDivedBy);
        list.Add(txtInitialFundAmt);
        list.Add(txtMinimumLimit);
        list.Add(txtMnthsHeld);
        list.Add(txtPLBmnts);
        list.Add(txtPrevEscrowAmt);
        list.Add(lnkBtnESCROW);
        list.Add(lnkBtnESCROWCancel);
        list.Add(lbILRFSave);
        list.Add(lblILRFCancel);
        list.Add(chkAggrLimitUnlimited);
        list.Add(chkInvoiceLSI);
        list.Add(chkbxUseLossFund);
        list.Add(chkMiniLimitUnlimited);
        list.Add(ddlIBNRLDFNONE);
        list.Add(PolicyNumLstBox);
        list.Add(lstBoxPolicy);
        list.Add(lbILRFFormulaSetupClose);
        list.Add(lstILRFFormulaSetup);
        list.Add(btnSaveILRFFormulaSetup);
        list.Add(txtOtherAmount);
        ProcessExitFlag(list);
    }

    # region ESCROW Tab
    /// <summary>
    /// bool Function to check the second row of ESCROW  record is new or existing. Used while saving the record
    /// </summary>
    /// <param name=""></param>
    /// <returns>bool(True/False)</returns>
    protected bool CheckNewESCROW
    {
        get { return (bool)ViewState["CheckNewESCROW"]; }
        set { ViewState["CheckNewESCROW"] = value; }
    }

    /// <summary>
    /// Selecting one Program Period ID from the list of the Program periods
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Escrowinfomdatasource_selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {

        e.InputParameters["ProgramPeriodID"] = this.PrmPerdID;

    }

    /// <summary>
    /// Compare old ESCROW details if both the old and the new details
    /// on the screen are same do not save
    /// </summary>
    /// <param name="AdjCompareLBA"></param>
    /// <param name="PolicyCount"></param>
    /// <returns></returns>
    protected bool CompareValues(AdjustmentParameterSetupBE AdjCompareESCROW, int PolicyCount)
    {
        bool retValue = true;
        decimal PrevEscrowAmt = 0;

        if (AdjCompareESCROW.AdjparameterTypeID == ESCROWblaccess.GetLookUpID("Loss Fund", "ADJUSTMENT PARAMETER TYPE"))
        {
            if (txtPrevEscrowAmt.Text != "")
            {
                PrevEscrowAmt = decimal.Parse(txtPrevEscrowAmt.Text.Replace(",", ""));
            }

            if (AdjCompareESCROW.Escrow_PrevAmt == PrevEscrowAmt &&
                AdjCompareESCROW.Escrow_Diveser == Convert.ToInt32(txtDivedBy.Text) &&
                AdjCompareESCROW.Escrow_MnthsHeld == decimal.Parse(txtMnthsHeld.Text) &&
                AdjCompareESCROW.Escrow_PLMNumber == Convert.ToInt32(txtPLBmnts.Text) &&
                AdjCompareESCROW.use_dpst_ind == bool.Parse(chkbxUseLossFund.Checked.ToString()))
            {

                IList<AdjustmentParameterPolicyBE> ESCROWPolBE = AdjParamPolInfo.getAdjParamtrPol(AdjCompareESCROW.adj_paramet_setup_id);
                if (PolicyCount == ESCROWPolBE.Count)
                {
                    for (int Pol = 0; Pol < PolicyNumLstBox.Items.Count; Pol++)
                    {
                        foreach (AdjustmentParameterPolicyBE AdjParmPolESCROW in ESCROWPolBE)
                            if (AdjParmPolESCROW.coml_agmt_id == int.Parse(PolicyNumLstBox.Items[Pol].Value) && PolicyNumLstBox.Items[Pol].Selected == false)
                            {
                                retValue = false;
                                break;
                            }
                    }
                }
                else
                {
                    retValue = false; ;
                }
            }
            else
            {
                retValue = false;
            }
        }
        return retValue;
    }

    /// <summary>
    /// Initially used to check which program period selected and show the details on the screen
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void ProgramPeriod_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        int PrmPerdID = Convert.ToInt32(e.CommandArgument);
        this.Escrowinfodatasource.SelectParameters[0].DefaultValue = PrmPerdID.ToString();
        BindESCROWDetails(PrmPerdID);
        BindILRFInformation();
        BindTaxExemptSetupListView();
    }

    /// <summary>
    /// Invokes when user clicks on ILRFSetup Cancel Link and cancels the details newly entered
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ESCROWCancel_Click(object sender, EventArgs e)
    {
        BindESCROWDetails(int.Parse(ViewState["PRGPRDID"].ToString()));
    }

    #region Maintaining Session for BE

    private AdjustmentParameterSetupBE ESCROWParamBE
    {
        //get { return (AdjustmentParameterSetupBE)Session["ESCROWParamBE"]; }
        //set { Session["ESCROWParamBE"] = value; }
        get { return (AdjustmentParameterSetupBE)RetrieveObjectFromSessionUsingWindowName("ESCROWParamBE"); }
        set { SaveObjectToSessionUsingWindowName("ESCROWParamBE", value); }
    }
    #endregion
    /// <summary>
    /// Bind ESCROW Table with data from database
    /// ESCROW has one rows(Initial loading the data in datatables on screen for each tab) 
    /// </summary>
    /// <param name="prgprdID"></param>
    private void BindESCROWDetails(int prgprdID)
    {
        try
        {
            pnlESCROWDtls.Enabled = true;
            pnlppEscrow.Enabled = true;
            CheckNewESCROW = true;
            ViewState["AdjSetupESCROWID"] = null;
            ViewState["PRGPRDID"] = prgprdID;

            int CustomerID = AISMasterEntities.AccountNumber;
            Adj_Parameter_SetupBS ESCROWParamBS = new Adj_Parameter_SetupBS();
            //IList<PolicyBE> PlcyLst = ESCROWblaccess.ListviewGetPolicyDataforCust(prgprdID, CustomerID);
            IList<LookupBE> PlcyLst = PolBS.getPolicyDataforLookups(prgprdID, CustomerID);
            if (PlcyLst.Count > 0)
            {

                UpdatepanelESCROW.Visible = true;
                pnlESCROWDtls.Visible = true;
                imgDisablerow.ImageUrl = "~/images/disabled.GIF";

                //Bind all policies in Checkboxlist control
                PolicyNumLstBox.DataSource = PlcyLst;
                //PolicyNumLstBox.DataTextField = "PolicyPerfectNumber";
                //PolicyNumLstBox.DataValueField = "PolicyID";
                PolicyNumLstBox.DataTextField = "LookUpName";
                PolicyNumLstBox.DataValueField = "LookUpID";
                PolicyNumLstBox.DataBind();
                if (PlcyLst.Count > 2) pnlPolicyNumberListESCROW.Height = Unit.Point(57);
                if (PlcyLst.Count == 1) pnlPolicyNumberListESCROW.Height = Unit.Point(23);
                if (PlcyLst.Count == 2) pnlPolicyNumberListESCROW.Height = Unit.Point(40);
                imgDisablerow.Visible = false;
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                {
                    txtPLBmnts.Enabled = true;
                    txtDivedBy.Enabled = true;
                    txtMnthsHeld.Enabled = true;
                    txtPrevEscrowAmt.Enabled = true;
                    PolicyNumLstBox.Enabled = true;
                    lnkBtnESCROW.Enabled = true;
                    lnkBtnESCROWCancel.Enabled = true;
                    chkbxUseLossFund.Enabled = true;
                }
                txtPLBmnts.Text = "12";
                txtMnthsHeld.Text = "02.50";
                //txtDivedBy.Text = "12";
                txtDivedBy.Text = "";
                txtPrevEscrowAmt.Text = "0";

                int intESCROW = new BLAccess().GetLookUpID("Loss Fund", "ADJUSTMENT PARAMETER TYPE");
                //AdjustmentParameterSetupBE 
                ESCROWParamBE = AdjPrmStupBS.getAdjParamsforILRF(prgprdID, CustomerID, intESCROW);
                if (ESCROWParamBE != null)
                {
                    imgDisablerow.Visible = true;
                    lnkBtnESCROW.Text = "Update";
                    //IList<AdjustmentParameterSetupBE> Bindlist = ESCROWParamBS.getAdjParamtr(prgprdID, CustomerID);
                    //if (Bindlist != null)
                    //{
                    //    foreach (AdjustmentParameterSetupBE LBAParamLBABE in Bindlist)
                    //    {
                    //        if (LBAParamLBABE.AdjparameterTypeID == ESCROWblaccess.GetLookUpID("Escrow", "ADJUSTMENT PARAMETER TYPE"))
                    //        {
                    ViewState["AdjSetupESCROWID"] = ESCROWParamBE.adj_paramet_setup_id;
                    txtPLBmnts.Text = ESCROWParamBE.Escrow_PLMNumber.ToString();
                    txtDivedBy.Text = ESCROWParamBE.Escrow_Diveser.ToString();
                    string strMonth = ESCROWParamBE.Escrow_MnthsHeld.ToString().Substring(1, 1);
                    if (strMonth == ".")
                    {
                        txtMnthsHeld.Text = "0" + ESCROWParamBE.Escrow_MnthsHeld.ToString().Substring(0, 4);
                    }
                    else
                    {
                        txtMnthsHeld.Text = ESCROWParamBE.Escrow_MnthsHeld.ToString().Substring(0, 5);
                    }
                    // txtPrevEscrowAmt.Text = ESCROWParamBE.Escrow_PrevAmt.ToString().TrimEnd("0".ToCharArray());
                    if (ESCROWParamBE.Escrow_PrevAmt != null)
                        txtPrevEscrowAmt.Text = decimal.Parse(ESCROWParamBE.Escrow_PrevAmt.ToString()).ToString("#,##0");

                    if (ESCROWParamBE.use_dpst_ind.Value == true)
                    {
                        chkbxUseLossFund.Checked = true;
                    }
                    else
                    {
                        chkbxUseLossFund.Checked = false;
                    }

                    if (ESCROWParamBE.actv_ind.Value == true)
                    {
                        imgDisablerow.ImageUrl = "~/images/disabled.GIF";
                        imgDisablerow.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable this record?');");
                        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                        {
                            txtPLBmnts.Enabled = true;
                            txtDivedBy.Enabled = true;
                            txtMnthsHeld.Enabled = true;
                            txtPrevEscrowAmt.Enabled = true;
                            PolicyNumLstBox.Enabled = true;
                            lnkBtnESCROW.Enabled = true;
                            lnkBtnESCROWCancel.Enabled = true;
                            chkbxUseLossFund.Enabled = true;
                        }
                    }
                    else
                    {
                        imgDisablerow.ImageUrl = "~/images/enabled.GIF";
                        imgDisablerow.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable this record?');");
                        txtPLBmnts.Enabled = false;
                        txtDivedBy.Enabled = false;
                        txtMnthsHeld.Enabled = false;
                        txtPrevEscrowAmt.Enabled = false;
                        PolicyNumLstBox.Enabled = false;
                        lnkBtnESCROW.Enabled = false;
                        lnkBtnESCROWCancel.Enabled = false;
                        chkbxUseLossFund.Enabled = false;
                    }
                    IList<AdjustmentParameterPolicyBE> AdjParmBE = ESCROWParamBE.AdjParametPolBEs;
                    foreach (AdjustmentParameterPolicyBE ESCROWPolBEdetails in AdjParmBE)
                    {
                        //                        PolicyNumLstBox.Items.FindByValue(ESCROWPolBEdetails.coml_agmt_id.ToString()).Selected = true;
                        AddInActiveLookupData(ref PolicyNumLstBox, ESCROWPolBEdetails.coml_agmt_id);
                    }
                    CheckNewESCROW = false;

                }
                else
                {
                    lnkBtnESCROW.Text = "Save";                    
                }

            }
            else
            {
                //this.lblcheckvalidations.Visible = true;
                //this.lblcheckvalidations.Text = "* No Policy exists for the selected Program Period";
                ShowMessage("No Policy exists for the selected Program Period");
                this.pnlESCROWDtls.Visible = false;
            }

        }
        catch (Exception ex)
        {
            ShowError(ex.Message, ex);
        }
    }

    /// <summary>
    /// Save method used to assign the values to a database field from the screen and save\update the details
    /// with Transaction processing for the first row in ESCROW Tab
    /// </summary>
    /// <param name="AdjStupBE"></param>
    /// <returns></returns>
    protected AdjustmentParameterSetupBE SaveEntity(AdjustmentParameterSetupBE AdjStupESCROWBE)
    {

        AdjStupESCROWBE.prem_adj_pgm_id = int.Parse(ViewState["PRGPRDID"].ToString());
        AdjStupESCROWBE.Escrow_PLMNumber = Convert.ToInt32(txtPLBmnts.Text);
        AdjStupESCROWBE.Escrow_Diveser = Convert.ToInt32(txtDivedBy.Text);
        //AdjStupESCROWBE.Escrow_MnthsHeld = Convert.ToInt32(txtMnthsHeld.Text);
        AdjStupESCROWBE.Escrow_MnthsHeld = Convert.ToDecimal(txtMnthsHeld.Text);

        if (txtPrevEscrowAmt.Text != "")
        //EAISA-5 Veracode flaw fix 12082017     
        {
            AdjStupESCROWBE.Escrow_PrevAmt = Convert.ToDecimal(Server.HtmlEncode(txtPrevEscrowAmt.Text));
        }
        else
        {
            AdjStupESCROWBE.Escrow_PrevAmt = 0;
        }
        AdjStupESCROWBE.AdjparameterTypeID = ESCROWblaccess.GetLookUpID("Loss Fund", "ADJUSTMENT PARAMETER TYPE");
        AdjStupESCROWBE.Cstmr_Id = AISMasterEntities.AccountNumber;

        if (chkbxUseLossFund.Checked == true)
        {
            AdjStupESCROWBE.use_dpst_ind = true;
        }
        else
        {
            AdjStupESCROWBE.use_dpst_ind = false;
        }

        if (CheckNewESCROW == false)
        {
            //Concurrency
            bool con = ShowConcurrentConflict(Convert.ToDateTime(ESCROWParamBE.UPDATE_DATE), Convert.ToDateTime(AdjStupESCROWBE.UPDATE_DATE));
            if (!con)
            {
                return AdjStupESCROWBE;
            }
            //end
            AdjStupESCROWBE.UPDATE_DATE = DateTime.Now;
            AdjStupESCROWBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
        }
        else
        {
            AdjStupESCROWBE.actv_ind = true;
            AdjStupESCROWBE.CREATE_DATE = DateTime.Now;
            AdjStupESCROWBE.CREATE_USER_ID = CurrentAISUser.PersonID;
        }
        //AdjStupBE.SetContext(AdjParameterTransactionWrapper);

        //Included to avoid "NULL" Value in this field
        AdjStupESCROWBE.incld_ernd_retro_prem_ind = false;

        bool ResultESCROW = AdjPrmStupBS.Update(AdjStupESCROWBE);
        //if (CheckNewESCROW == false)
        //{
        //   ShowConcurrentConflict(ResultESCROW, AdjStupESCROWBE.ErrorMessage);

        //}

        if (ResultESCROW)
        {
            //AdjParameterTransactionWrapper.SubmitTransactionChanges();
            if (CheckNewESCROW == false)
            {

                //Code for logging into Audit Transaction Table 

                ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                // audtBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
                audtBS.Save(AISMasterEntities.AccountNumber, AdjStupESCROWBE.prem_adj_pgm_id, GlobalConstants.AuditingWebPage.EscrowSetup, CurrentAISUser.PersonID);

                //AdjParameterTransactionWrapper.SubmitTransactionChanges();
            }

            //            AdjParamPolInfo.deletePol(AdjStupESCROWBE.Cstmr_Id, AdjStupESCROWBE.adj_paramet_setup_id);
            if (AdjStupESCROWBE.adj_paramet_setup_id > 0)
            {
                int polCount = 0;
                for (int i = 0; i < PolicyNumLstBox.Items.Count; i++)
                {
                    if (PolicyNumLstBox.Items[i].Selected)
                    {
                        ++polCount;
                        AdjustmentParameterPolicyBE ESCROWPolicylinkBE = new AdjustmentParameterPolicyBE();
                        ESCROWPolicylinkBE.adj_paramet_pol_id = 0;
                        ESCROWPolicylinkBE.adj_paramet_setup_id = AdjStupESCROWBE.adj_paramet_setup_id;
                        ESCROWPolicylinkBE.custmrID = AISMasterEntities.AccountNumber;
                        ESCROWPolicylinkBE.PrmadjPRgmID = int.Parse(ViewState["PRGPRDID"].ToString());
                        ESCROWPolicylinkBE.coml_agmt_id = int.Parse(PolicyNumLstBox.Items[i].Value);
                        ESCROWPolicylinkBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                        ESCROWPolicylinkBE.CREATE_DATE = DateTime.Now;
                        AdjParamPolInfo.Update(ESCROWPolicylinkBE);
                    }
                }

                if (polCount == 0)
                {
                    AdjParamPolInfo.deletePol(AdjStupESCROWBE.Cstmr_Id, AdjStupESCROWBE.adj_paramet_setup_id);
                    
                    lnkViewDetails.Enabled = false;
                }
            }

            //AdjParameterTransactionWrapper.SubmitTransactionChanges();
        }
        else
        {
            // AdjParameterTransactionWrapper.RollbackChanges();
        }

        return AdjStupESCROWBE;
    }

    /// <summary>
    /// Save ESCROW-Information 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnESCROWInfoDetailsSave_Click(object sender, EventArgs e)
    {
        try
        {
            int PolCount = 0;
            bool Flag = false;

            for (int Pol = 0; Pol < PolicyNumLstBox.Items.Count; Pol++)
            {
                if (PolicyNumLstBox.Items[Pol].Selected)
                {
                    PolCount = PolCount + 1;
                }
            }

            //if (PolCount > 0)
            //{


            if (CheckNewESCROW == false)
            {

                //adjParmStupBE = Bindlist.Single(bl => bl.adj_paramet_setup_id == int.Parse(ViewState["ADJLBAPRGID"].ToString()));
                adjParmStupBE = AdjPrmStupBS.getAdjParamRow(int.Parse(ViewState["AdjSetupESCROWID"].ToString()));
                if (CompareValues(adjParmStupBE, PolCount))
                {
                    Flag = true;
                }
                else
                {
                    Flag = false;
                }
            }

            if (!Flag)
            {
                //To check Concurrency on Save for ESCROW
                if (CheckNewESCROW == true)
                {
                    string AdjReviewResult = AdjPrmStupBS.getAdjParamResultOther(int.Parse(ViewState["PRGPRDID"].ToString()), AISMasterEntities.AccountNumber, ESCROWblaccess.GetLookUpID("Loss Fund", "ADJUSTMENT PARAMETER TYPE"));
                    if (AdjReviewResult == "true")
                    {
                        ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
                        return;
                    }
                }
                //End
                adjParmStupBE = SaveEntity(AdjParmStupBE);
                BindESCROWDetails(int.Parse(ViewState["PRGPRDID"].ToString()));
            }
            //else
            //{
            //    ShowMessage("No information has been changed to Save");
            //}
            ////this.lblcheckvalidations.Visible = false;
            ////this.lblcheckvalidations.Text = "";
            ////lnkBtnLBAadj.CommandArgument = adjParmStupBE.adj_paramet_setup_id.ToString();
            ////BindESCROWDetails(int.Parse(ViewState["PRGPRDID"].ToString()));
            //}
            //else
            //{
            //    //this.lblcheckvalidations.Visible = true;
            //    ShowMessage("Please select atleast one policy before saving ESCROW information");
            //    //this.lblcheckvalidations.Text = " * Please select atleast one policy before saving LBA information";
            //}
        }
        catch (Exception ex)
        {
            ShowError(ex.Message, ex);
        }
    }

    /// <summary>
    /// Disable\Enable First Row for ESCROW details table
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DisablefirstRow(object sender, CommandEventArgs e)
    {
        try
        {
            //AdjustmentParameterSetupBE  LBAParmBE;
            if (ViewState["AdjSetupESCROWID"] != null)
            {
                int AdjParmtSetupID = int.Parse(ViewState["AdjSetupESCROWID"].ToString());
                //LBAParmBE = AdjparamsetupInfo.getAdjParamRow(AdjParmtSetupID);
                adjParmStupBE = AdjPrmStupBS.getAdjParamRow(AdjParmtSetupID);
                //Concurrency
                bool con = ShowConcurrentConflict(Convert.ToDateTime(ESCROWParamBE.UPDATE_DATE), Convert.ToDateTime(adjParmStupBE.UPDATE_DATE));
                if (!con)
                {
                    return;
                }
                //end
                if (adjParmStupBE.actv_ind.Value == true)
                {
                    adjParmStupBE.actv_ind = false;
                }
                else
                {
                    adjParmStupBE.actv_ind = true;
                }
                adjParmStupBE.UPDATE_DATE = DateTime.Now;
                adjParmStupBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
                bool ResultESCROW = AdjPrmStupBS.Update(adjParmStupBE);
                //if (ResultESCROW)
                //{
                //AdjParameterTransactionWrapper.SubmitTransactionChanges();
                //Code for logging into Audit Transaction Table 
                ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                audtBS.Save(AISMasterEntities.AccountNumber, adjParmStupBE.prem_adj_pgm_id, GlobalConstants.AuditingWebPage.EscrowSetup, CurrentAISUser.PersonID);
                //AdjParameterTransactionWrapper.SubmitTransactionChanges();
                //}
                //else
                //{
                //    //AdjParameterTransactionWrapper.RollbackChanges();
                //}
                BindESCROWDetails(int.Parse(ViewState["PRGPRDID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message, ex);
        }
    }
    # endregion ESCROW Tab

    #region ILRF Setup Tab

    #region ILRFSetup
    //protected int SelectedProgramPeriodID
    //{
    //    get
    //    {
    //        if (hidSelProgramPeriod.Value != null)
    //            return Convert.ToInt32(hidSelProgramPeriod.Value);
    //        else
    //            return -1;
    //    }
    //    set { hidSelProgramPeriod.Value = value.ToString(); }
    //}
    private PolicyBS polBS;
    private PolicyBS PolBS
    {
        get
        {
            if (polBS == null)
            {
                polBS = new PolicyBS();
                //polBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
            }
            return polBS;
        }
        set { polBS = value; }
    }
    private Adj_Paramet_PolBS adjPrmPolBS;
    private Adj_Paramet_PolBS AdjPrmPolBS
    {
        get
        {
            if (adjPrmPolBS == null)
            {
                adjPrmPolBS = new Adj_Paramet_PolBS();
                // adjPrmPolBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
            }
            return adjPrmPolBS;
        }
        set { adjPrmPolBS = value; }
    }
    private ApplWebPageAudtBS auditBizService;
    private ApplWebPageAudtBS AuditBizService
    {
        get
        {
            if (auditBizService == null)
            {
                auditBizService = new ApplWebPageAudtBS();
                //auditBizService.AppTransactionWrapper = AdjParameterTransactionWrapper;
            }
            return auditBizService;
        }
    }
    #region Maintaining Session for BE

    private AdjustmentParameterSetupBE ILRFParamBESesn
    {
        //get { return (AdjustmentParameterSetupBE)Session["ILRFParamBESesn"]; }
        //set { Session["ILRFParamBESesn"] = value; }
        get { return (AdjustmentParameterSetupBE)RetrieveObjectFromSessionUsingWindowName("ILRFParamBESesn"); }
        set { SaveObjectToSessionUsingWindowName("ILRFParamBESesn", value); }
    }
    #endregion
    /// <summary>
    ///  Invokes when user selects a Program Period
    /// </summary>
    protected void BindILRFInformation()
    {
        try
        {
            ClearFields();
            pnlILRFSetup.Visible = true;
            pnlFormulaSetup.Visible = false;
            IList<LookupBE> poliyList = PolBS.getPolicyDataforLookups(Convert.ToInt32(ViewState["PRGPRDID"]), AISMasterEntities.AccountNumber);

            if (poliyList.Count > 0)
            {
                lstBoxPolicy.DataSource = poliyList;
                lstBoxPolicy.DataTextField = "LookUpName";
                lstBoxPolicy.DataValueField = "LookUpID";
                lstBoxPolicy.DataBind();
                if (poliyList.Count > 2) pnlPolicyList.Height = Unit.Point(57);
                if (poliyList.Count == 1) pnlPolicyList.Height = Unit.Point(23);
                if (poliyList.Count == 2) pnlPolicyList.Height = Unit.Point(40);

                int intILRFID = new BLAccess().GetLookUpID("ILRF", "ADJUSTMENT PARAMETER TYPE");
                //AdjustmentParameterSetupBE 
                ILRFParamBESesn = AdjPrmStupBS.getAdjParamsforILRF(Convert.ToInt32(ViewState["PRGPRDID"]), AISMasterEntities.AccountNumber, intILRFID);
                if (ILRFParamBESesn != null)
                {
                    imgEnableDisable.Visible = true;
                    lnkViewDetails.Enabled = true;
                    lbDetails.Enabled = true;
                    lbTaxes.Enabled = true;
                    lbILRFSave.Text = "Update";
                    hidPremAdjPgmSetupID.Value = ILRFParamBESesn.adj_paramet_setup_id.ToString();
                    //txtInitialFundAmt.Text = Math.Round(Convert.ToDouble(ILRFParamBE.incur_los_reim_fund_initl_fund_amt), 0).ToString();
                    if (ILRFParamBESesn.incur_los_reim_fund_initl_fund_amt != null)
                        txtInitialFundAmt.Text = Convert.ToDouble(ILRFParamBESesn.incur_los_reim_fund_initl_fund_amt).ToString("#,##0");
                    //Added as per the SR 325928
                    if (ILRFParamBESesn.incur_los_reim_fund_othr_amt != null)
                        txtOtherAmount.Text = Convert.ToDouble(ILRFParamBESesn.incur_los_reim_fund_othr_amt).ToString("#,##0");
                    else
                        txtOtherAmount.Text = "0";
                    chkAggrLimitUnlimited.Checked = Convert.ToBoolean(ILRFParamBESesn.incur_los_reim_fund_unlim_agmt_lim_ind);
                    if (Convert.ToBoolean(ILRFParamBESesn.incur_los_reim_fund_unlim_agmt_lim_ind))
                        txtAggregateLimit.Enabled = false;
                    else
                        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                        {
                            txtAggregateLimit.Enabled = true;
                        }

                    //txtAggregateLimit.Text = Math.Round(Convert.ToDouble(ILRFParamBE.incur_los_reim_fund_aggr_lim_amt), 0).ToString();
                    if (ILRFParamBESesn.incur_los_reim_fund_aggr_lim_amt != null)
                        txtAggregateLimit.Text = Convert.ToDouble(ILRFParamBESesn.incur_los_reim_fund_aggr_lim_amt).ToString("#,##0");
                    chkMiniLimitUnlimited.Checked = Convert.ToBoolean(ILRFParamBESesn.incur_los_reim_fund_unlim_minimium_lim_ind);
                    if (Convert.ToBoolean(ILRFParamBESesn.incur_los_reim_fund_unlim_minimium_lim_ind))
                        txtMinimumLimit.Enabled = false;
                    else
                    {
                        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                            txtMinimumLimit.Enabled = true;
                    }
                    //txtMinimumLimit.Text = Math.Round(Convert.ToDouble(ILRFParamBE.incur_los_reim_fund_min_lim_amt), 0).ToString();
                    if (ILRFParamBESesn.incur_los_reim_fund_min_lim_amt != null)
                        txtMinimumLimit.Text = Convert.ToDouble(ILRFParamBESesn.incur_los_reim_fund_min_lim_amt).ToString("#,##0");
                    chkInvoiceLSI.Checked = Convert.ToBoolean(ILRFParamBESesn.incur_los_reim_fund_invc_lsi_ind);
                    EnableDisableILRFSetupControls(Convert.ToBoolean(ILRFParamBESesn.actv_ind));

                    if (ILRFParamBESesn.incur_but_not_rptd_los_dev_fctr_id != null)
                    {

                        ddlIBNRLDFNONE.DataBind();
                        //                        ddlIBNRLDFNONE.Items.FindByValue(ILRFParamBE.incur_but_not_rptd_los_dev_fctr_id.ToString()).Selected = true;
                        AddInActiveLookupData(ref ddlIBNRLDFNONE, ILRFParamBESesn.incur_but_not_rptd_los_dev_fctr_id.Value);

                        hidIBNRLDFSelVal.Value = ILRFParamBESesn.incur_but_not_rptd_los_dev_fctr_id.ToString();
                    }
                    else
                    {
                        ddlIBNRLDFNONE.DataBind();
                        ddlIBNRLDFNONE.Items.FindByValue("0").Selected = true;
                        hidIBNRLDFSelVal.Value = "";
                    }
                    //checking policies in policy listbox
                    IList<AdjustmentParameterPolicyBE> AdjPrPolBE = ILRFParamBESesn.AdjParametPolBEs;
                    foreach (AdjustmentParameterPolicyBE ILRFdetails in AdjPrPolBE)
                    {
                        //                        lstBoxPolicy.Items.FindByValue(ILRFdetails.coml_agmt_id.ToString()).Selected = true;
                        AddInActiveLookupData(ref lstBoxPolicy, ILRFdetails.coml_agmt_id);
                    }

                }
                else
                {
                    hidPremAdjPgmSetupID.Value = "";
                    imgEnableDisable.Visible = false;
                    lnkViewDetails.Enabled = false;
                    lbILRFSave.Text = "Save";
                    lbDetails.Enabled = false;
                    lbTaxes.Enabled = false;
                    txtOtherAmount.Text = "0";
                    //txtAggregateLimit.Text = "0";
                    //txtMinimumLimit.Text = "0";
                    lnkViewDetails.Enabled = false;
                }
            }
            else
            {
                ShowMessage("No Policy exists for the selected Program Period");
                pnlILRFSetup.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message, ex);
        }

    }
    /// <summary>
    /// Enables and Disables ILRF Setup controls
    /// </summary>
    /// <param name="flag"></param>
    protected void EnableDisableILRFSetupControls(bool flag)
    {
        if (flag == true)
        {
            imgEnableDisable.ImageUrl = "~/images/disabled.GIF";
            imgEnableDisable.CommandName = "DISABLE";
            imgEnableDisable.ToolTip = "Click here to Disable this Adjustment Parameter";
            imgEnableDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable this record?');");
        }
        else
        {
            imgEnableDisable.ImageUrl = "~/images/enabled.GIF";
            imgEnableDisable.CommandName = "ENABLE";
            imgEnableDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable this record?');");
            imgEnableDisable.ToolTip = "Click here to Enable this Adjustment Parameter";
        }

        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
        {
            pnlPolicyList.Enabled = flag;
            txtInitialFundAmt.Enabled = flag;
            chkAggrLimitUnlimited.Enabled = flag;
            txtAggregateLimit.Enabled = flag;
            chkMiniLimitUnlimited.Enabled = flag;
            txtMinimumLimit.Enabled = flag;
            ddlIBNRLDFNONE.Enabled = flag;
            chkInvoiceLSI.Enabled = flag;
            lbILRFSave.Enabled = flag;
            lblILRFCancel.Enabled = flag;
            txtOtherAmount.Enabled = flag;

        }
        else
            lbILRFSave.Enabled = false;
        lbDetails.Enabled = flag;
        lbTaxes.Enabled = flag;
        //added below code for the SR 325928 security
        if (ROLENAME != GlobalConstants.ApplicationSecurityGroup.Manager && ROLENAME != GlobalConstants.ApplicationSecurityGroup.SystemAdmin)
        {
            txtOtherAmount.Enabled = false;
        }
        else
        {
            txtOtherAmount.Enabled = flag;
        }
        if (flag)
        {
            if (chkAggrLimitUnlimited.Checked)
                txtAggregateLimit.Enabled = false;
            else
            {
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                    txtAggregateLimit.Enabled = true;
            }

            if (chkMiniLimitUnlimited.Checked)
                txtMinimumLimit.Enabled = false;
            else
            {
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                    txtMinimumLimit.Enabled = true;
            }
        }
    }
    protected void ClearFields()
    {
        ViewState["AdjSetupILRFID"] = "";
        txtInitialFundAmt.Text = "";
        chkAggrLimitUnlimited.Checked = false;
        txtAggregateLimit.Text = "";
        chkMiniLimitUnlimited.Checked = false;
        txtMinimumLimit.Text = "";
        txtOtherAmount.Text = "";
        chkInvoiceLSI.Checked = false;
        ddlIBNRLDFNONE.DataBind();
        ddlIBNRLDFNONE.Items.FindByValue("0").Selected = true;
        hidIBNRLDFSelVal.Value = "";
        EnableDisableILRFSetupControls(true);
        lbILRFSave.Text = "Save";
    }
    /// <summary>
    /// Invokes when user clicks on Enable Disable Image of ILRF Setup
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgEnableDisable_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            int AdjParmtSetupID = Convert.ToInt32(hidPremAdjPgmSetupID.Value);
            AdjustmentParameterSetupBE adjParmStupBE = AdjPrmStupBS.getAdjParamRow(AdjParmtSetupID);
            //Concurrency
            bool con = ShowConcurrentConflict(Convert.ToDateTime(ILRFParamBESesn.UPDATE_DATE), Convert.ToDateTime(adjParmStupBE.UPDATE_DATE));
            if (!con)
            {
                return;
            }
            //end
            if (imgEnableDisable.CommandName == "DISABLE")
                adjParmStupBE.actv_ind = false;
            else
                adjParmStupBE.actv_ind = true;

            adjParmStupBE.UPDATE_DATE = DateTime.Now;
            adjParmStupBE.UPDATE_USER_ID = CurrentAISUser.PersonID;

            bool boolIsAdjParmtSetupSaved = AdjPrmStupBS.Update(adjParmStupBE);
            //if (boolIsAdjParmtSetupSaved)
            //{
            //    //Code for logging into Audit Transaction Table         
            AuditBizService.Save(AISMasterEntities.AccountNumber, adjParmStupBE.prem_adj_pgm_id, GlobalConstants.AuditingWebPage.ILRFSetup, CurrentAISUser.PersonID);
            //AdjParameterTransactionWrapper.SubmitTransactionChanges();
            //}
            //else
            //{
            //    AdjParameterTransactionWrapper.RollbackChanges();
            //}
            BindILRFInformation();
        }
        catch (Exception ex)
        {
            //AdjParameterTransactionWrapper.RollbackChanges();
            ShowError(ex.Message, ex);
        }
    }

    /// <summary>
    /// Invokes when user clicks on ILRFSetup Cancel Link and cancels the details newly entered
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ILRFCancel_Click(object sender, EventArgs e)
    {
        BindILRFInformation();
    }

    /// <summary>
    /// Invokes when user clicks on ILRFSetup Save Link
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ILRFSave_Click(object sender, EventArgs e)
    {
        bool retValue = true;
        int PolCountCHF = 0;

        for (int PolCHF = 0; PolCHF < lstBoxPolicy.Items.Count; PolCHF++)
        {
            if (lstBoxPolicy.Items[PolCHF].Selected)
            {
                PolCountCHF = PolCountCHF + 1;
            }
        }
        String Changes = string.Empty;
        int intILRFID1 = new BLAccess().GetLookUpID("ILRF", "ADJUSTMENT PARAMETER TYPE");
        AdjustmentParameterSetupBE ILRFParamBE1 = AdjPrmStupBS.getAdjParamsforILRF(Convert.ToInt32(ViewState["PRGPRDID"]), AISMasterEntities.AccountNumber, intILRFID1);
        if (ILRFParamBE1 != null)
        {
            IList<AdjustmentParameterPolicyBE> AdjPrPolBE = ILRFParamBE1.AdjParametPolBEs;
            if (PolCountCHF > 0)
            {
                if (PolCountCHF == AdjPrPolBE.Count)
                {
                    for (int Pol = 0; Pol < lstBoxPolicy.Items.Count; Pol++)
                    {
                        foreach (AdjustmentParameterPolicyBE ILRFParamBE2 in AdjPrPolBE)
                            if (ILRFParamBE2.coml_agmt_id == int.Parse(lstBoxPolicy.Items[Pol].Value) && lstBoxPolicy.Items[Pol].Selected == false)
                            {
                                retValue = false;
                                break;
                            }
                    }
                }
                else
                {
                    retValue = false;
                }
            }
            if (txtInitialFundAmt.Text.ToString() == ILRFParamBE1.incur_los_reim_fund_initl_fund_amt.ToString().Replace(".00", "")
                && Convert.ToBoolean(chkAggrLimitUnlimited.Checked) == ILRFParamBE1.incur_los_reim_fund_unlim_agmt_lim_ind
                && txtAggregateLimit.Text.ToString() == ILRFParamBE1.incur_los_reim_fund_aggr_lim_amt.ToString().Replace(".00", "")
                && Convert.ToBoolean(chkMiniLimitUnlimited.Checked) == ILRFParamBE1.incur_los_reim_fund_unlim_minimium_lim_ind
                && txtMinimumLimit.Text.ToString() == ILRFParamBE1.incur_los_reim_fund_min_lim_amt.ToString().Replace(".00", "")
                && Convert.ToBoolean(chkInvoiceLSI.Checked) == ILRFParamBE1.incur_los_reim_fund_invc_lsi_ind && retValue)
            {
                Changes = "false";
            }
            else
            {
                Changes = "true";
            }
        }
        AdjustmentParameterSetupBE ILRFParamBE = new AdjustmentParameterSetupBE();
        bool boolIsAdjParmtPolSaved = false;
        bool boolISNEW = false;
        try
        {
            if (hidPremAdjPgmSetupID.Value != "")
            {
                ILRFParamBE = AdjPrmStupBS.getAdjParamRow(int.Parse(hidPremAdjPgmSetupID.Value));
                //Concurrency
                bool con = ShowConcurrentConflict(Convert.ToDateTime(ILRFParamBESesn.UPDATE_DATE), Convert.ToDateTime(ILRFParamBE.UPDATE_DATE));
                if (!con)
                {
                    return;
                }
                //end
                ILRFParamBE.UPDATE_DATE = DateTime.Now;
                ILRFParamBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
            }
            else
            {
                boolISNEW = true;
                int intILRFID = new BLAccess().GetLookUpID("ILRF", "ADJUSTMENT PARAMETER TYPE");
                //To check Concurrency on Save for ILRF
                if (boolISNEW == true)
                {
                    string AdjReviewResult = AdjPrmStupBS.getAdjParamResultOther(int.Parse(ViewState["PRGPRDID"].ToString()), AISMasterEntities.AccountNumber, intILRFID);
                    if (AdjReviewResult == "true")
                    {
                        ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
                        return;
                    }
                }
                //End

                ILRFParamBE.AdjparameterTypeID = intILRFID;
                ILRFParamBE.Cstmr_Id = AISMasterEntities.AccountNumber;
                ILRFParamBE.prem_adj_pgm_id = Convert.ToInt32(ViewState["PRGPRDID"]);
                ILRFParamBE.CREATE_DATE = DateTime.Now;
                ILRFParamBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                ILRFParamBE.actv_ind = true;
            }

            ILRFParamBE.incur_los_reim_fund_initl_fund_amt = txtInitialFundAmt.Text == "" ? 0 : Convert.ToDecimal(txtInitialFundAmt.Text);
            //Added as per SR 325928
            ILRFParamBE.incur_los_reim_fund_othr_amt = txtOtherAmount.Text == "" ? 0 : Convert.ToDecimal(txtOtherAmount.Text);
            ILRFParamBE.incur_los_reim_fund_unlim_agmt_lim_ind = Convert.ToBoolean(chkAggrLimitUnlimited.Checked);
            ILRFParamBE.incur_los_reim_fund_aggr_lim_amt = txtAggregateLimit.Text == "" ? 0 : Convert.ToDecimal(txtAggregateLimit.Text);
            ILRFParamBE.incur_los_reim_fund_unlim_minimium_lim_ind = Convert.ToBoolean(chkMiniLimitUnlimited.Checked);
            ILRFParamBE.incur_los_reim_fund_min_lim_amt = txtMinimumLimit.Text == "" ? 0 : Convert.ToDecimal(txtMinimumLimit.Text);
            ILRFParamBE.incur_los_reim_fund_invc_lsi_ind = Convert.ToBoolean(chkInvoiceLSI.Checked);
            if (Convert.ToInt32(ddlIBNRLDFNONE.SelectedValue) != 0)
                ILRFParamBE.incur_but_not_rptd_los_dev_fctr_id = Convert.ToInt32(ddlIBNRLDFNONE.SelectedValue);
            else
                ILRFParamBE.incur_but_not_rptd_los_dev_fctr_id = null;

            //Included to avoid "NULL" Value in this field
            ILRFParamBE.incld_ernd_retro_prem_ind = false;

            bool boolIsAdjParmtSetupSaved = AdjPrmStupBS.Update(ILRFParamBE);
            //Shows Concurrent Conflict Error if so
            //  ShowConcurrentConflict(boolIsAdjParmtSetupSaved, ILRFParamBE.ErrorMessage);

            //AdjParameterTransactionWrapper.SubmitTransactionChanges();
            bool boolIsAdjParmtPolsDeleted = AdjPrmPolBS.DeleteAdjPrmPol(ILRFParamBE.Cstmr_Id, Convert.ToInt32(ViewState["PRGPRDID"]), ILRFParamBE.adj_paramet_setup_id);
            if (ILRFParamBE.adj_paramet_setup_id > 0)
            {
                for (int i = 0; i < lstBoxPolicy.Items.Count; i++)
                {
                    if (lstBoxPolicy.Items[i].Selected)
                    {
                        AdjustmentParameterPolicyBE ILRFPrmPolBE = new AdjustmentParameterPolicyBE();
                        ILRFPrmPolBE.adj_paramet_setup_id = ILRFParamBE.adj_paramet_setup_id;
                        ILRFPrmPolBE.custmrID = AISMasterEntities.AccountNumber;
                        ILRFPrmPolBE.PrmadjPRgmID = Convert.ToInt32(ViewState["PRGPRDID"]);
                        ILRFPrmPolBE.coml_agmt_id = int.Parse(lstBoxPolicy.Items[i].Value);
                        ILRFPrmPolBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                        ILRFPrmPolBE.CREATE_DATE = DateTime.Now;
                        boolIsAdjParmtPolSaved = AdjPrmPolBS.Update(ILRFPrmPolBE);
                        //if (!boolIsAdjParmtPolSaved) break;
                    }
                }

            }

            //if ((boolIsAdjParmtSetupSaved) && (boolIsAdjParmtPolsDeleted) && (boolIsAdjParmtPolSaved))
            //{
            //    //Code for logging into Audit Transaction Table 
            if (Changes == "true")
            {
                if (hidPremAdjPgmSetupID.Value != "")
                {
                    AuditBizService.Save(AISMasterEntities.AccountNumber, Convert.ToInt32(ViewState["PRGPRDID"]), GlobalConstants.AuditingWebPage.ILRFSetup, CurrentAISUser.PersonID);
                }
            }
            //    AdjParameterTransactionWrapper.SubmitTransactionChanges();
            //}
            //else
            //{
            //    AdjParameterTransactionWrapper.RollbackChanges();
            //}
            BindILRFInformation();
            if (boolISNEW)
            {
                IList<ILRFFormulaBE> FormulasList = ILRFFormulaSetupBizService.getILRFFormulas(Convert.ToInt32(ViewState["PRGPRDID"]), AISMasterEntities.AccountNumber, ddlIBNRLDFNONE.SelectedItem.Text.ToString());
                for (int i = 0; i < FormulasList.Count; i++)
                {
                    FormulasList[i].CUSTOMER_ID = AISMasterEntities.AccountNumber;
                    FormulasList[i].PROGRAMPERIOD_ID = Convert.ToInt32(ViewState["PRGPRDID"]);
                    FormulasList[i].CREATE_DATE = DateTime.Now;
                    FormulasList[i].CREATE_USER_ID = CurrentAISUser.PersonID;
                    bool boolIsILRFFormulaSetupSaved = ILRFFormulaSetupBizService.Update(FormulasList[i]);
                    //  ShowConcurrentConflict(boolIsILRFFormulaSetupSaved, FormulasList[i].ErrorMessage);
                }
            }
        }
        catch (Exception ex)
        {
            //AdjParameterTransactionWrapper.RollbackChanges();
            ShowError(ex.Message, ex);
        }
    }
    /// <summary>
    /// Invokes when user clicks on Detials link of ILRF Setup
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ILRFDetails_Click(object sender, EventArgs e)
    {
        BindILRFFormulaSetupListView();
    }

    protected void btnHidden_Click(object sender, EventArgs e)
    {
        try
        {
            AdjustmentParameterSetupBE ILRFParamBE = new AdjustmentParameterSetupBE();
            bool boolIsAdjParmtPolSaved = false;
            ILRFParamBE = AdjPrmStupBS.getAdjParamRow(Convert.ToInt32(hidPremAdjPgmSetupID.Value));
            //Concurrency
            bool con = ShowConcurrentConflict(Convert.ToDateTime(ILRFParamBESesn.UPDATE_DATE), Convert.ToDateTime(ILRFParamBE.UPDATE_DATE));
            if (!con)
            {
                return;
            }
            //end
            if (Convert.ToInt32(ddlIBNRLDFNONE.SelectedValue) != 0)
            {
                ILRFParamBE.incur_but_not_rptd_los_dev_fctr_id = Convert.ToInt32(ddlIBNRLDFNONE.SelectedValue);
            }
            else
            {
                ILRFParamBE.incur_but_not_rptd_los_dev_fctr_id = null;
            }
            //QA Fix 5014 Save other ILRF information along with the drop down change information
            ILRFParamBE.incur_los_reim_fund_initl_fund_amt = txtInitialFundAmt.Text == "" ? 0 : Convert.ToDecimal(txtInitialFundAmt.Text);
            //Added as per SR 325928
            ILRFParamBE.incur_los_reim_fund_othr_amt = txtOtherAmount.Text == "" ? 0 : Convert.ToDecimal(txtOtherAmount.Text);
            ILRFParamBE.incur_los_reim_fund_unlim_agmt_lim_ind = Convert.ToBoolean(chkAggrLimitUnlimited.Checked);
            ILRFParamBE.incur_los_reim_fund_aggr_lim_amt = txtAggregateLimit.Text == "" ? 0 : Convert.ToDecimal(txtAggregateLimit.Text);
            ILRFParamBE.incur_los_reim_fund_unlim_minimium_lim_ind = Convert.ToBoolean(chkMiniLimitUnlimited.Checked);
            ILRFParamBE.incur_los_reim_fund_min_lim_amt = txtMinimumLimit.Text == "" ? 0 : Convert.ToDecimal(txtMinimumLimit.Text);
            ILRFParamBE.incur_los_reim_fund_invc_lsi_ind = Convert.ToBoolean(chkInvoiceLSI.Checked);
            //QA Fix 5014 Save other ILRF information along with the drop down change information
            ILRFParamBE.UPDATE_DATE = DateTime.Now;
            ILRFParamBE.UPDATE_USER_ID = CurrentAISUser.PersonID;

            //Included to avoid "NULL" value for this field
            ILRFParamBE.incld_ernd_retro_prem_ind = false;

            bool boolIsAdjParmtSetupSaved = AdjPrmStupBS.Update(ILRFParamBE);

            //QA Fix 5014 Save other ILRF information along with the drop down change information
            bool boolIsAdjParmtPolsDeleted = AdjPrmPolBS.DeleteAdjPrmPol(ILRFParamBE.Cstmr_Id, Convert.ToInt32(ViewState["PRGPRDID"]), ILRFParamBE.adj_paramet_setup_id);
            if (ILRFParamBE.adj_paramet_setup_id > 0)
            {
                for (int i = 0; i < lstBoxPolicy.Items.Count; i++)
                {
                    if (lstBoxPolicy.Items[i].Selected)
                    {
                        AdjustmentParameterPolicyBE ILRFPrmPolBE = new AdjustmentParameterPolicyBE();
                        ILRFPrmPolBE.adj_paramet_setup_id = ILRFParamBE.adj_paramet_setup_id;
                        ILRFPrmPolBE.custmrID = AISMasterEntities.AccountNumber;
                        ILRFPrmPolBE.PrmadjPRgmID = Convert.ToInt32(ViewState["PRGPRDID"]);
                        ILRFPrmPolBE.coml_agmt_id = int.Parse(lstBoxPolicy.Items[i].Value);
                        ILRFPrmPolBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                        ILRFPrmPolBE.CREATE_DATE = DateTime.Now;
                        boolIsAdjParmtPolSaved = AdjPrmPolBS.Update(ILRFPrmPolBE);
                        //if (!boolIsAdjParmtPolSaved) break;
                    }
                }

            }
            //QA Fix 5014 Save other ILRF information along with the drop down change information
            bool boolIsFactorsDeleted = ILRFFormulaSetupBizService.DeleteFactors(Convert.ToInt32(ViewState["PRGPRDID"]));

            //Included for Bug #10005
            if (Convert.ToInt32(ddlIBNRLDFNONE.SelectedValue) != 0)
            {
                IList<ILRFFormulaBE> ILRFFormulas = new List<ILRFFormulaBE>();
                ILRFFormulas = (new ILRFFormulaBS()).GetDefaultILRFFormulas(
                    ILRFParamBE.Cstmr_Id, ILRFParamBE.prem_adj_pgm_id, ddlIBNRLDFNONE.Text.Trim());
                //To Do
            }


            //if ((boolIsAdjParmtSetupSaved) && (boolIsFactorsDeleted))
            //{
            //    //Code for logging into Audit Transaction Table         
            AuditBizService.Save(AISMasterEntities.AccountNumber, Convert.ToInt32(ViewState["PRGPRDID"]), GlobalConstants.AuditingWebPage.ILRFSetup, CurrentAISUser.PersonID);

            //    AdjParameterTransactionWrapper.SubmitTransactionChanges();
            //}
            //else
            //{
            //    AdjParameterTransactionWrapper.RollbackChanges();
            //}
            BindILRFInformation();
            //-----------------------
            IList<ILRFFormulaBE> FormulasList = ILRFFormulaSetupBizService.getILRFFormulas(Convert.ToInt32(ViewState["PRGPRDID"]), AISMasterEntities.AccountNumber, ddlIBNRLDFNONE.SelectedItem.Text.ToString());
            for (int i = 0; i < FormulasList.Count; i++)
            {
                FormulasList[i].CUSTOMER_ID = AISMasterEntities.AccountNumber;
                FormulasList[i].PROGRAMPERIOD_ID = Convert.ToInt32(ViewState["PRGPRDID"]);
                FormulasList[i].CREATE_DATE = DateTime.Now;
                FormulasList[i].CREATE_USER_ID = CurrentAISUser.PersonID;
                bool boolIsILRFFormulaSetupSaved = ILRFFormulaSetupBizService.Update(FormulasList[i]);
                //ShowConcurrentConflict(boolIsILRFFormulaSetupSaved, FormulasList[i].ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            //AdjParameterTransactionWrapper.RollbackChanges();
            ShowError(ex.Message, ex);
        }
    }
    #endregion ILRF Setup

    #region ILRF Formula Setup
    private ILRFFormulaBS iLRFFormulaSetupBizService;
    private ILRFFormulaBS ILRFFormulaSetupBizService
    {
        get
        {
            if (iLRFFormulaSetupBizService == null)
            {
                iLRFFormulaSetupBizService = new ILRFFormulaBS();
                //iLRFFormulaSetupBizService.AppTransactionWrapper = AdjParameterTransactionWrapper;
            }
            return iLRFFormulaSetupBizService;
        }
    }

    #region Maintaining Session for ILRFFormulaBE
    private IList<ILRFFormulaBE> FormulasList
    {
        get
        {
            //if (Session["FormulasList"] == null)
            //    Session["FormulasList"] = new List<ILRFFormulaBE>();
            //return (IList<ILRFFormulaBE>)Session["FormulasList"];
            if (RetrieveObjectFromSessionUsingWindowName("FormulasList") == null)
                SaveObjectToSessionUsingWindowName("FormulasList", new List<ILRFFormulaBE>());
            return (IList<ILRFFormulaBE>)RetrieveObjectFromSessionUsingWindowName("FormulasList");
        }
        //set { Session["FormulasList"] = value; }
        set { SaveObjectToSessionUsingWindowName("FormulasList", value); }
    }


    #endregion
    /// <summary>
    /// Invokes when user clicks on Detials link of ILRF Setup
    /// </summary>
    protected void BindILRFFormulaSetupListView()
    {
        try
        {
            pnlFormulaSetup.Visible = true;
            pnlILRFSetup.Enabled = false;
            pnlppEscrow.Enabled = false;
            btnSaveILRFFormulaSetup.Text = "Save";
            //IList<ILRFFormulaBE> FormulasList = new List<ILRFFormulaBE>();

            FormulasList = ILRFFormulaSetupBizService.getILRFFormulas(Convert.ToInt32(ViewState["PRGPRDID"]), AISMasterEntities.AccountNumber, ddlIBNRLDFNONE.SelectedItem.Text.ToString());
            lstILRFFormulaSetup.DataSource = FormulasList;
            lstILRFFormulaSetup.DataBind();
            EnableDisableCheckBoxes();

            TextBox txtILRFFormulaSetupID;
            for (int i = 0; i < lstILRFFormulaSetup.Items.Count(); i++)
            {
                txtILRFFormulaSetupID = (TextBox)lstILRFFormulaSetup.Items[i].FindControl("txtILRFFormulaSetupID");
                if ((txtILRFFormulaSetupID.Text != null) && (txtILRFFormulaSetupID.Text != "") && (txtILRFFormulaSetupID.Text != "0"))
                {
                    btnSaveILRFFormulaSetup.Text = "Update";
                    break;
                }
            }

        }
        catch (Exception ex)
        {
            ShowError(ex.Message, ex);
        }
    }
    /// <summary>
    /// Invokes when user clicks on ILRF Formula Setup Close linke
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbILRFFormulaSetupClose_Click(object sender, EventArgs e)
    {
        pnlFormulaSetup.Visible = false;
        pnlILRFSetup.Enabled = true;
        pnlppEscrow.Enabled = true;

    }
    protected void ServerValidateCheckbox(object sender, ServerValidateEventArgs oArgs)
    {
        //called by CustomValidator after page is submitted
        // count how many checkboxes are selected  
        int iFound = 0;
        foreach (ListItem oItem in lstBoxPolicy.Items)
        {
            if (oItem.Selected) iFound += 1;
        }
        // set IsValid property of "argument" object to True
        // if at least three selected, or False otherwise
        oArgs.IsValid = (iFound > 2);

    }
    public void EnableDisableCheckBoxes()
    {
        bool blchkUsePaidLoss = false;
        bool blchkUsePaidALAE = false;
        bool blchkUseReserveLosses = false;
        bool blchkUseReserveALAE = false;

        foreach (ListViewItem lv in lstILRFFormulaSetup.Items)
        {

            Label lblFactor = (Label)lv.FindControl("lblFactor");
            CheckBox chkUsePaidLoss = (CheckBox)lv.FindControl("chkUsePaidLoss");
            CheckBox chkUsePaidALAE = (CheckBox)lv.FindControl("chkUsePaidALAE");
            CheckBox chkUseReserveLosses = (CheckBox)lv.FindControl("chkUseReserveLosses");
            CheckBox chkUseReserveALAE = (CheckBox)lv.FindControl("chkUseReserveALAE");
            if (lblFactor.Text.Trim().ToUpper() == "IBNR" || lblFactor.Text.Trim().ToUpper() == "LDF")
            {
                blchkUsePaidLoss = chkUsePaidLoss.Checked;
                blchkUsePaidALAE = chkUsePaidALAE.Checked;
                blchkUseReserveLosses = chkUseReserveLosses.Checked;
                blchkUseReserveALAE = chkUseReserveALAE.Checked;
                chkUsePaidLoss.Attributes.Add("onclick", "javascript:EnableDisableCheckBoxes()");
                chkUsePaidALAE.Attributes.Add("onclick", "javascript:EnableDisableCheckBoxes()");
                chkUseReserveLosses.Attributes.Add("onclick", "javascript:EnableDisableCheckBoxes()");
                chkUseReserveALAE.Attributes.Add("onclick", "javascript:EnableDisableCheckBoxes()");
            }
            if (lblFactor.Text.Trim().ToUpper() == "LDF - USE LBA" || lblFactor.Text.Trim().ToUpper() == "LDF - USE LCF" || lblFactor.Text.Trim().ToUpper() == "IBNR - USE LCF" || lblFactor.Text.Trim().ToUpper() == "IBNR - USE LBA")
            {

                if (blchkUsePaidLoss)
                {
                    chkUsePaidLoss.Enabled = true;
                }
                else
                {
                    chkUsePaidLoss.InputAttributes.Add("disabled", "disabled");
                }
                if (blchkUsePaidALAE)
                {
                    chkUsePaidALAE.Enabled = true;
                }
                else
                {
                    chkUsePaidALAE.InputAttributes.Add("disabled", "disabled");
                }
                if (blchkUseReserveLosses)
                {
                    chkUseReserveLosses.Enabled = true;
                }
                else
                {
                    chkUseReserveLosses.InputAttributes.Add("disabled", "disabled");
                }

                if (blchkUseReserveALAE)
                {
                    chkUseReserveALAE.Enabled = true;
                }
                else
                {
                    chkUseReserveALAE.InputAttributes.Add("disabled", "disabled");
                }
            }

        }

    }
    private bool IsDataChanged(ILRFFormulaBE ILRFFormulaSetupBE, CheckBox chkUsePaidLoss, CheckBox chkUsePaidALAE, CheckBox chkUseReserveLosses, CheckBox chkUseReserveALAE)
    {
        bool isDataChanged = false;

        isDataChanged = (ILRFFormulaSetupBE.USE_PAID_LOSS_INDICATOR != chkUsePaidLoss.Checked) ? true : false;
        isDataChanged = (ILRFFormulaSetupBE.USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR != chkUsePaidALAE.Checked) ? true : isDataChanged;
        isDataChanged = (ILRFFormulaSetupBE.USE_RESERVE_LOSS_INDICATOR != chkUseReserveLosses.Checked) ? true : isDataChanged;
        isDataChanged = (ILRFFormulaSetupBE.USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR != chkUseReserveALAE.Checked) ? true : isDataChanged;

        return isDataChanged;
    }
    /// <summary>
    /// Invokes when user clicks on Save button of ILRF Formula Setup
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>    
    protected void btnSaveILRFFormulaSetup_Click(object sender, EventArgs e)
    {

        try
        {
            foreach (ListViewItem lv in lstILRFFormulaSetup.Items)
            {
                bool isDataChanged = false;
                TextBox txtILRFFormulaSetupID = (TextBox)lv.FindControl("txtILRFFormulaSetupID");
                TextBox txtFactorID = (TextBox)lv.FindControl("txtFactorID");
                CheckBox chkUsePaidLoss = (CheckBox)lv.FindControl("chkUsePaidLoss");
                CheckBox chkUsePaidALAE = (CheckBox)lv.FindControl("chkUsePaidALAE");
                CheckBox chkUseReserveLosses = (CheckBox)lv.FindControl("chkUseReserveLosses");
                CheckBox chkUseReserveALAE = (CheckBox)lv.FindControl("chkUseReserveALAE");
                ILRFFormulaBE ILRFFormulaSetupBE = new ILRFFormulaBE();
                if (Convert.ToInt32(txtILRFFormulaSetupID.Text) != 0)
                {
                    ILRFFormulaSetupBE = ILRFFormulaSetupBizService.getILRFFormulaRow(Convert.ToInt32(txtILRFFormulaSetupID.Text));
                    //Concurrency Issue
                    ILRFFormulaBE ILRFFormulaSetupBEold = (FormulasList.Where(o => o.INCURRED_LOSS_REIM_FUND_FRMLA_ID.Equals(Convert.ToInt32(txtILRFFormulaSetupID.Text)))).First();
                    bool con = ShowConcurrentConflict(Convert.ToDateTime(ILRFFormulaSetupBEold.UPDATE_DATE), Convert.ToDateTime(ILRFFormulaSetupBE.UPDATE_DATE));
                    if (!con)
                        return;
                    //End
                    ILRFFormulaSetupBE.UPDATE_DATE = DateTime.Now;
                    ILRFFormulaSetupBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
                    isDataChanged = IsDataChanged(ILRFFormulaSetupBE, chkUsePaidLoss, chkUsePaidALAE, chkUseReserveLosses, chkUseReserveALAE);
                }
                else
                {
                    ILRFFormulaSetupBE.LOSS_REIM_FUND_FACTOR_TYPE_ID = Convert.ToInt32(txtFactorID.Text);
                    ILRFFormulaSetupBE.CUSTOMER_ID = AISMasterEntities.AccountNumber;
                    ILRFFormulaSetupBE.PROGRAMPERIOD_ID = Convert.ToInt32(ViewState["PRGPRDID"]);
                    ILRFFormulaSetupBE.LOSS_REIM_FUND_FACTOR_TYPE_ID = Convert.ToInt32(txtFactorID.Text);
                    ILRFFormulaSetupBE.CREATE_DATE = DateTime.Now;
                    ILRFFormulaSetupBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                }

                ILRFFormulaSetupBE.USE_PAID_LOSS_INDICATOR = chkUsePaidLoss.Checked;
                ILRFFormulaSetupBE.USE_PAID_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = chkUsePaidALAE.Checked;
                ILRFFormulaSetupBE.USE_RESERVE_LOSS_INDICATOR = chkUseReserveLosses.Checked;
                ILRFFormulaSetupBE.USE_RESERVE_ALLOCATED_LOSS_ADJ_EXP_INDICATOR = chkUseReserveALAE.Checked;


                //if (boolIsILRFFormulaSetupSaved)
                //{
                //if it is a new record
                if (Convert.ToInt32(txtILRFFormulaSetupID.Text) != 0)
                {
                    if (isDataChanged)
                    {
                        bool boolIsILRFFormulaSetupSaved = ILRFFormulaSetupBizService.Update(ILRFFormulaSetupBE);
                        //ShowConcurrentConflict(boolIsILRFFormulaSetupSaved, ILRFFormulaSetupBE.ErrorMessage);
                        //Code for logging into Audit Transaction Table  
                        AuditBizService.Save(AISMasterEntities.AccountNumber, Convert.ToInt32(ViewState["PRGPRDID"]), GlobalConstants.AuditingWebPage.ILRFSetup, CurrentAISUser.PersonID);
                    }
                }
                else //if it is a not new record
                { bool boolIsILRFFormulaSetupSaved = ILRFFormulaSetupBizService.Update(ILRFFormulaSetupBE); }
                //    AdjParameterTransactionWrapper.SubmitTransactionChanges();
                //}
                //else
                //{
                //    AdjParameterTransactionWrapper.RollbackChanges();
                //}                
            }
            BindILRFFormulaSetupListView();
        }
        catch (Exception ex)
        {
            //AdjParameterTransactionWrapper.RollbackChanges();
            ShowError(ex.Message, ex);
        }
    }
    #endregion ILRF Formula Setup
    #endregion ILRF Setup Tab

    protected void ILRFPolicyDetails_Click(object sender, EventArgs e)
    {
        //Session["PolicyDetails"] = null;
        SaveObjectToSessionUsingWindowName("PolicyDetails", null);
        ViewState["isPopupOpened"] = false;
        modalPolDetails.Show();
        BindILRFPopupList();
        ViewState["isPopupOpened"] = true;
    }

    void BindILRFPopupList()
    {
        BLAccess Lbablaccess = new BLAccess();
        int CustomerID = AISMasterEntities.AccountNumber;
        int prgprdID = Convert.ToInt32(ViewState["PRGPRDID"]);
        int adjID = 0;
        if (ddlAdjType.SelectedIndex > 0)
        {
            adjID = Convert.ToInt32(ddlAdjType.SelectedValue);
        }
        else
        {
            adjID = 0;
        }
        IList<PolicyBE> PlcyLst = Lbablaccess.ListviewGetPolicyDataforCust(prgprdID, CustomerID, adjID);
        //if (ddlAdjType.SelectedIndex > 0)
        //{
        //    //int? adjID = int.Parse(ddlAdjType.SelectedValue);
        //    PlcyLst = PlcyLst.Where(pol => pol.AdjusmentTypeID == adjID).ToList();
        //}
        gvPolicy.DataSource = PlcyLst;
        gvPolicy.DataBind();


        if (ViewState["isPopupOpened"].ToString().ToLower() == "false")
        {
            var query = (from q in PlcyLst
                         select new
                         {
                             AdjusmentTypeID = q.AdjusmentTypeID,
                             AdjustmentTypeName = q.AdjustmentTypeName
                         }).Distinct();

            ddlAdjType.DataSource = query.ToList();
            ddlAdjType.DataValueField = "AdjusmentTypeID";
            ddlAdjType.DataTextField = "AdjustmentTypeName";
            ddlAdjType.DataBind();
            ddlAdjType.Items.Insert(0, new ListItem("(select)", "0"));
            ViewState["isPopupOpened"] = true;
        }

        CheckBox chkHeader = (CheckBox)gvPolicy.HeaderRow.FindControl("chkSelectAll");
        int count = 0;
        Dictionary<int, string> dictionary = new Dictionary<int, string>();
        //if (Session["PolicyDetails"] == null)
        if (RetrieveObjectFromSessionUsingWindowName("PolicyDetails") == null)
        {
            foreach (GridViewRow row in gvPolicy.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                Label lbl = (Label)row.FindControl("lblPolicyNumber");
                Label lblPolID = (Label)row.FindControl("lblPolicyID");
                if (chk.Checked)
                {
                    if (!dictionary.ContainsKey(Convert.ToInt32(lblPolID.Text)))
                    {
                        dictionary.Add(Convert.ToInt32(lblPolID.Text), lbl.Text);
                    }
                }
            }

            //Session["PolicyDetails"] = dictionary;
            SaveObjectToSessionUsingWindowName("PolicyDetails", dictionary);
        }
        foreach (GridViewRow row in gvPolicy.Rows)
        {
            CheckBox chk = (CheckBox)row.FindControl("chkSelect");
            if (chk.Checked)
            {
                count++;
            }
        }
        if (gvPolicy.Rows.Count == count)
        {
            chkHeader.Checked = true;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ViewState["isLBAPopupOpened"] = false;
        //Session["PolicyDetails"] = null;
        SaveObjectToSessionUsingWindowName("PolicyDetails", null);
        ddlAdjType.SelectedIndex = 0;
        modalPolDetails.Hide();
    }

    //void BindAdjTypeDropDown()
    //{
    //    int CustomerID = AISMasterEntities.AccountNumber;
    //    int prgprdID = Convert.ToInt32(ViewState["PRGPRDID"]);
    //    //IList<PolicyBE> PlcyLstLBA = ESCROWblaccess.ListviewGetPolicyDataforCustLBA(prgprdID, CustomerID);
    //    IList<PolicyBE> PlcyLst = ESCROWblaccess.ListviewGetPolicyDataforCust(prgprdID, CustomerID);
    //    var query = (from q in PlcyLst
    //                 select new
    //                 {
    //                     AdjusmentTypeID = q.AdjusmentTypeID,
    //                     AdjustmentTypeName = q.AdjustmentTypeName
    //                 }).Distinct();

    //    ddlAdjType.DataSource = query.ToList();
    //    ddlAdjType.DataValueField = "AdjusmentTypeID";
    //    ddlAdjType.DataTextField = "AdjustmentTypeName";
    //    ddlAdjType.DataBind();
    //    ddlAdjType.Items.Insert(0, new ListItem("(select)", "0"));
    //}

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        Dictionary<int, string> dictionary;
        //if (Session["PolicyDetails"] != null)
        if (RetrieveObjectFromSessionUsingWindowName("PolicyDetails") != null)
        {
            //dictionary = (Dictionary<int, string>)Session["PolicyDetails"];
            dictionary = (Dictionary<int, string>)RetrieveObjectFromSessionUsingWindowName("PolicyDetails");
        }
        else
        {
            dictionary = new Dictionary<int, string>();
        }

        foreach (GridViewRow gvr in gvPolicy.Rows)
        {
            if (((CheckBox)gvr.FindControl("chkSelect")).Checked == true && !dictionary.ContainsKey(Convert.ToInt32(((Label)gvr.FindControl("lblPolicyID")).Text)))
            {
                dictionary.Add(Convert.ToInt32(((Label)gvr.FindControl("lblPolicyID")).Text), ((Label)gvr.FindControl("lblPolicyNumber")).Text);
               
            }
            else if (((CheckBox)gvr.FindControl("chkSelect")).Checked == false && dictionary.ContainsKey(Convert.ToInt32(((Label)gvr.FindControl("lblPolicyID")).Text)))
            {
                dictionary.Remove(Convert.ToInt32(((Label)gvr.FindControl("lblPolicyID")).Text));
                
            }
        }

        //Session["PolicyDetails"] = dictionary;
        SaveObjectToSessionUsingWindowName("PolicyDetails", dictionary);
        modalPolDetails.Show();
    }

    protected void gvPolicy_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        Dictionary<int, string> dictionary = null;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chk = (CheckBox)e.Row.FindControl("chkSelect");
            Label lbl = (Label)e.Row.FindControl("lblPolicyNumber");
            Label lblPolID = (Label)e.Row.FindControl("lblPolicyID");



            //if (Session["PolicyDetails"] != null)
            if (RetrieveObjectFromSessionUsingWindowName("PolicyDetails") != null)
            {
                //dictionary = (Dictionary<int, string>)Session["PolicyDetails"];
                dictionary = (Dictionary<int, string>)RetrieveObjectFromSessionUsingWindowName("PolicyDetails");
                foreach (var pair in dictionary)
                {
                    if (lbl.Text == pair.Value)
                    {
                        chk.Checked = true;
                    }
                }
            }
            else
            {
                foreach (ListItem item in lstBoxPolicy.Items)
                {
                    if (item.Value == lblPolID.Text && item.Selected)
                    {
                        chk.Checked = true;
                    }
                }
                //dictionary = new Dictionary<int, string>();

                //foreach (ListItem item in lstBoxPolicy.Items)
                //{

                //    if (item.Value == lblPolID.Text && item.Selected)
                //    {

                //        if (!dictionary.ContainsKey(Convert.ToInt32(lblPolID.Text)))
                //        {
                //            dictionary.Add(Convert.ToInt32(lblPolID.Text), lbl.Text);
                //        }
                //        chk.Checked = true;
                //    }
                //}

            }
        }
        //if (e.Row.RowIndex == gvPolicy.Rows.Count - 1)
        //{
        //    Session["PolicyDetails"] = dictionary;
        //}
    }

    protected void ddlAdjType_SelectedIndexChanged(object sender, EventArgs e)
    {

        BindILRFPopupList();

        modalPolDetails.Show();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        bool retValue = true;
        int PolCountCHF = 0;


        for (int PolCHF = 0; PolCHF < gvPolicy.Rows.Count; PolCHF++)
        {
            CheckBox chk = (CheckBox)gvPolicy.Rows[PolCHF].FindControl("chkSelect");
            if (chk.Checked)
            {
                PolCountCHF = PolCountCHF + 1;
            }
        }

        
        String Changes = string.Empty;
        int intILRFID1 = new BLAccess().GetLookUpID("ILRF", "ADJUSTMENT PARAMETER TYPE");
        AdjustmentParameterSetupBE ILRFParamBE1 = AdjPrmStupBS.getAdjParamsforILRF(Convert.ToInt32(ViewState["PRGPRDID"]), AISMasterEntities.AccountNumber, intILRFID1);
        if (ILRFParamBE1 != null)
        {
            IList<AdjustmentParameterPolicyBE> AdjPrPolBE = ILRFParamBE1.AdjParametPolBEs;
            if (PolCountCHF > 0)
            {
                if (PolCountCHF == AdjPrPolBE.Count)
                {
                    for (int Pol = 0; Pol < gvPolicy.Rows.Count; Pol++)
                    {
                        foreach (AdjustmentParameterPolicyBE ILRFParamBE2 in AdjPrPolBE)
                        {
                            CheckBox chk = (CheckBox)gvPolicy.Rows[Pol].FindControl("chkSelect");
                            Label lbl = (Label)gvPolicy.Rows[Pol].FindControl("lblPolicyID");
                            if (ILRFParamBE2.coml_agmt_id == int.Parse(lbl.Text) && chk.Checked == false)
                            {
                                retValue = false;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    retValue = false;
                }
            }
            if (txtInitialFundAmt.Text.ToString() == ILRFParamBE1.incur_los_reim_fund_initl_fund_amt.ToString().Replace(".00", "")
                && Convert.ToBoolean(chkAggrLimitUnlimited.Checked) == ILRFParamBE1.incur_los_reim_fund_unlim_agmt_lim_ind
                && txtAggregateLimit.Text.ToString() == ILRFParamBE1.incur_los_reim_fund_aggr_lim_amt.ToString().Replace(".00", "")
                && Convert.ToBoolean(chkMiniLimitUnlimited.Checked) == ILRFParamBE1.incur_los_reim_fund_unlim_minimium_lim_ind
                && txtMinimumLimit.Text.ToString() == ILRFParamBE1.incur_los_reim_fund_min_lim_amt.ToString().Replace(".00", "")
                && Convert.ToBoolean(chkInvoiceLSI.Checked) == ILRFParamBE1.incur_los_reim_fund_invc_lsi_ind && retValue)
            {
                Changes = "false";
            }
            else
            {
                Changes = "true";
            }
        }
        AdjustmentParameterSetupBE ILRFParamBE = new AdjustmentParameterSetupBE();
        bool boolIsAdjParmtPolSaved = false;
        bool boolISNEW = false;
        try
        {
            if (hidPremAdjPgmSetupID.Value != "")
            {
                ILRFParamBE = AdjPrmStupBS.getAdjParamRow(int.Parse(hidPremAdjPgmSetupID.Value));
                //Concurrency
                bool con = ShowConcurrentConflict(Convert.ToDateTime(ILRFParamBESesn.UPDATE_DATE), Convert.ToDateTime(ILRFParamBE.UPDATE_DATE));
                if (!con)
                {
                    return;
                }
                //end
                ILRFParamBE.UPDATE_DATE = DateTime.Now;
                ILRFParamBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
            }
            else
            {
                boolISNEW = true;
                int intILRFID = new BLAccess().GetLookUpID("ILRF", "ADJUSTMENT PARAMETER TYPE");
                //To check Concurrency on Save for ILRF
                if (boolISNEW == true)
                {
                    string AdjReviewResult = AdjPrmStupBS.getAdjParamResultOther(int.Parse(ViewState["PRGPRDID"].ToString()), AISMasterEntities.AccountNumber, intILRFID);
                    if (AdjReviewResult == "true")
                    {
                        ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
                        return;
                    }
                }
                //End

                ILRFParamBE.AdjparameterTypeID = intILRFID;
                ILRFParamBE.Cstmr_Id = AISMasterEntities.AccountNumber;
                ILRFParamBE.prem_adj_pgm_id = Convert.ToInt32(ViewState["PRGPRDID"]);
                ILRFParamBE.CREATE_DATE = DateTime.Now;
                ILRFParamBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                ILRFParamBE.actv_ind = true;
            }

            ILRFParamBE.incur_los_reim_fund_initl_fund_amt = txtInitialFundAmt.Text == "" ? 0 : Convert.ToDecimal(txtInitialFundAmt.Text);
            //Added as per SR 325928
            ILRFParamBE.incur_los_reim_fund_othr_amt = txtOtherAmount.Text == "" ? 0 : Convert.ToDecimal(txtOtherAmount.Text);
            ILRFParamBE.incur_los_reim_fund_unlim_agmt_lim_ind = Convert.ToBoolean(chkAggrLimitUnlimited.Checked);
            ILRFParamBE.incur_los_reim_fund_aggr_lim_amt = txtAggregateLimit.Text == "" ? 0 : Convert.ToDecimal(txtAggregateLimit.Text);
            ILRFParamBE.incur_los_reim_fund_unlim_minimium_lim_ind = Convert.ToBoolean(chkMiniLimitUnlimited.Checked);
            ILRFParamBE.incur_los_reim_fund_min_lim_amt = txtMinimumLimit.Text == "" ? 0 : Convert.ToDecimal(txtMinimumLimit.Text);
            ILRFParamBE.incur_los_reim_fund_invc_lsi_ind = Convert.ToBoolean(chkInvoiceLSI.Checked);
            if (Convert.ToInt32(ddlIBNRLDFNONE.SelectedValue) != 0)
                ILRFParamBE.incur_but_not_rptd_los_dev_fctr_id = Convert.ToInt32(ddlIBNRLDFNONE.SelectedValue);
            else
                ILRFParamBE.incur_but_not_rptd_los_dev_fctr_id = null;

            //Included to avoid "NULL" Value in this field
            ILRFParamBE.incld_ernd_retro_prem_ind = false;

            bool boolIsAdjParmtSetupSaved = AdjPrmStupBS.Update(ILRFParamBE);
            //Shows Concurrent Conflict Error if so
            //  ShowConcurrentConflict(boolIsAdjParmtSetupSaved, ILRFParamBE.ErrorMessage);

            //AdjParameterTransactionWrapper.SubmitTransactionChanges();
            bool boolIsAdjParmtPolsDeleted = AdjPrmPolBS.DeleteAdjPrmPol(ILRFParamBE.Cstmr_Id, Convert.ToInt32(ViewState["PRGPRDID"]), ILRFParamBE.adj_paramet_setup_id);
            if (ILRFParamBE.adj_paramet_setup_id > 0)
            {
                //if (Session["PolicyDetails"] != null)
                if (RetrieveObjectFromSessionUsingWindowName("PolicyDetails") != null)
                {
                    //Dictionary<int, string> dictionary = (Dictionary<int, string>)Session["PolicyDetails"];
                    Dictionary<int, string> dictionary = (Dictionary<int, string>)RetrieveObjectFromSessionUsingWindowName("PolicyDetails");
                    //for (int Pol = 0; Pol < gvLcfPolList.Rows.Count; Pol++)
                    //{
                    foreach (var pair in dictionary)
                    {
                        //CheckBox chk = (CheckBox)gvPolicy.Rows[i].FindControl("chkSelect");
                        //if (chk.Checked)
                        //{
                            AdjustmentParameterPolicyBE ILRFPrmPolBE = new AdjustmentParameterPolicyBE();
                            ILRFPrmPolBE.adj_paramet_setup_id = ILRFParamBE.adj_paramet_setup_id;
                            ILRFPrmPolBE.custmrID = AISMasterEntities.AccountNumber;
                            ILRFPrmPolBE.PrmadjPRgmID = Convert.ToInt32(ViewState["PRGPRDID"]);
                            ILRFPrmPolBE.coml_agmt_id = pair.Key;
                            ILRFPrmPolBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                            ILRFPrmPolBE.CREATE_DATE = DateTime.Now;
                            boolIsAdjParmtPolSaved = AdjPrmPolBS.Update(ILRFPrmPolBE);
                            //if (!boolIsAdjParmtPolSaved) break;
                        //}
                    }
                }

            }

            //if ((boolIsAdjParmtSetupSaved) && (boolIsAdjParmtPolsDeleted) && (boolIsAdjParmtPolSaved))
            //{
            //    //Code for logging into Audit Transaction Table 
            if (Changes == "true")
            {
                if (hidPremAdjPgmSetupID.Value != "")
                {
                    AuditBizService.Save(AISMasterEntities.AccountNumber, Convert.ToInt32(ViewState["PRGPRDID"]), GlobalConstants.AuditingWebPage.ILRFSetup, CurrentAISUser.PersonID);
                }
            }
            //    AdjParameterTransactionWrapper.SubmitTransactionChanges();
            //}
            //else
            //{
            //    AdjParameterTransactionWrapper.RollbackChanges();
            //}
            BindILRFInformation();
            if (boolISNEW)
            {
                IList<ILRFFormulaBE> FormulasList = ILRFFormulaSetupBizService.getILRFFormulas(Convert.ToInt32(ViewState["PRGPRDID"]), AISMasterEntities.AccountNumber, ddlIBNRLDFNONE.SelectedItem.Text.ToString());
                for (int i = 0; i < FormulasList.Count; i++)
                {
                    FormulasList[i].CUSTOMER_ID = AISMasterEntities.AccountNumber;
                    FormulasList[i].PROGRAMPERIOD_ID = Convert.ToInt32(ViewState["PRGPRDID"]);
                    FormulasList[i].CREATE_DATE = DateTime.Now;
                    FormulasList[i].CREATE_USER_ID = CurrentAISUser.PersonID;
                    bool boolIsILRFFormulaSetupSaved = ILRFFormulaSetupBizService.Update(FormulasList[i]);
                    //  ShowConcurrentConflict(boolIsILRFFormulaSetupSaved, FormulasList[i].ErrorMessage);
                }
            }
        }
        catch (Exception ex)
        {
            //AdjParameterTransactionWrapper.RollbackChanges();
            ShowError(ex.Message, ex);
        }

        ViewState["isLBAPopupOpened"] = false;
        //Session["PolicyDetails"] = null;
        SaveObjectToSessionUsingWindowName("PolicyDetails", null);
        ddlAdjType.SelectedIndex = 0;
        modalPolDetails.Hide();
    }

    #region Texas Tax Setup Screen coding
    /// <summary>
    /// Texas Tax:ListView Item Command Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Item Command Event
    protected void lstTaxSetup_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if ((e.CommandName.ToUpper() == "SAVE") || (e.CommandName.ToUpper() == "UPDATE"))
        {

            //Get the values  
            TextBox txtAmount = (TextBox)e.Item.FindControl("txtAmount");
            DropDownList ddlTaxDescriptionType = (DropDownList)e.Item.FindControl("ddlTaxDescriptionlist");
            DropDownList ddlLOB = (DropDownList)e.Item.FindControl("ddlLOB");

            //PostingTransactionTypeBE postTransTypBE = new PostingTransactionTypeBS().LoadData(Convert.ToInt32(ddlContactType.SelectedValue));
            //int intILRFID = new BLAccess().GetLookUpID("ILRF", "ADJUSTMENT PARAMETER TYPE");
            //AdjustmentParameterSetupBE 
            //AdjustmentParameterSetupBE ILRFParamBE = AdjPrmStupBS.getAdjParamsforILRF(Convert.ToInt32(ViewState["PRGPRDID"]), AISMasterEntities.AccountNumber, intILRFID);
            if (e.CommandName.ToUpper() == "SAVE")  //if it is new
            {
                try
                {
                    ClearError();
                    ILRFTaxSetupBE TaxBE = new ILRFTaxSetupBE();
                    TaxBE.TAX_TYP_ID = Convert.ToInt32(ddlTaxDescriptionType.SelectedValue);
                    TaxBE.PREM_ADJ_PGM_ID = Convert.ToInt32(ViewState["PRGPRDID"]);
                    TaxBE.PREM_ADJ_PGM_SETUP_ID = ILRFParamBESesn.adj_paramet_setup_id;
                    //TaxBE.TAX_AMT = Convert.ToDecimal(txtAmount.Text.ToString());
                    //TaxBE.TAX_AMT=txtAmount.Text == "" ? 0 : Convert.ToDecimal(txtAmount.Text);
                    if (txtAmount.Text != null && txtAmount.Text != string.Empty && txtAmount.Text != "")
                        TaxBE.TAX_AMT = Convert.ToDecimal(txtAmount.Text);
                    else
                        TaxBE.TAX_AMT = null;
                    TaxBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                    TaxBE.CREATE_DATE = DateTime.Now;
                    TaxBE.ACTV_IND = true;
                    TaxBE.CUSTOMER_ID = AISMasterEntities.AccountNumber;
                    TaxBE.LN_OF_BSN_ID = Convert.ToInt32(ddlLOB.SelectedItem.Value);

                    int intTaxExempted = (new ILRFTaxSetupBS()).isTaxExemptedState(Convert.ToInt32(ddlTaxDescriptionType.SelectedValue), Convert.ToInt32(ViewState["PRGPRDID"]));
                    if (intTaxExempted == 1)
                    {
                        ShowError("This state has been Tax Exempted.");
                        return;
                    }

                    int intDuplicateCount = (new ILRFTaxSetupBS()).isTaxTypeAlreadyExist(0, Convert.ToInt32(ddlTaxDescriptionType.SelectedValue), Convert.ToInt32(ViewState["PRGPRDID"]), Convert.ToInt32(ddlLOB.SelectedItem.Value));
                    if (intDuplicateCount == 0)
                    {
                        bool i = (new ILRFTaxSetupBS()).Update(TaxBE);
                        BindILRFTaxSetupListView();
                        ShowError("The entry has been saved.");
                    }
                    else if (intDuplicateCount == 1)
                    {
                        ShowError("The record cannot be saved. An identical record already exists.");
                    }
                    else if (intDuplicateCount == 2)
                    {
                        ShowError("The record cannot be saved. An identical record already exists. Please enable existing tax type.");
                    }

                }
                catch (RetroBaseException ee)
                {
                    ShowError(ee.Message, ee);
                }

            }
            else if (e.CommandName.ToUpper() == "UPDATE")  //if it is Update
            {
                Label lblILRFTaxID = (Label)e.Item.FindControl("lblILRFTaxID");
                int intILRFTaxID = Convert.ToInt32(lblILRFTaxID.Text);
                ILRFTaxSetupBE TaxSetupBE;
                try
                {
                    TaxSetupBE = (new ILRFTaxSetupBS()).getILRFTaxSetupRow(intILRFTaxID);
                    // Concurrency Code
                    //PremiumAdjMiscInvoiceBE pamiBEConcurrent = (PremAdjMISCOLD.Where(o => o.PREM_ADJ_MISC_INVC_ID.Equals(intPremAdjMiscInvoiceID))).First();
                    //bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(pamiBEConcurrent.UPDATE_DATE), Convert.ToDateTime(pamiBE.UPDATE_DATE));
                    //if (!Concurrency)
                    //    return;

                    TaxSetupBE.TAX_TYP_ID = Convert.ToInt32(ddlTaxDescriptionType.SelectedValue);
                    //TaxSetupBE.TAX_AMT = Convert.ToDecimal(txtAmount.Text.ToString());
                    //TaxSetupBE.TAX_AMT = txtAmount.Text == "" ? 0 : Convert.ToDecimal(txtAmount.Text);
                    if (txtAmount.Text != null && txtAmount.Text != string.Empty && txtAmount.Text != "")
                        TaxSetupBE.TAX_AMT = Convert.ToDecimal(txtAmount.Text);
                    else
                        TaxSetupBE.TAX_AMT = null;
                        
                    TaxSetupBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
                    TaxSetupBE.UPDATE_DATE = DateTime.Now;
                    TaxSetupBE.LN_OF_BSN_ID = Convert.ToInt32(ddlLOB.SelectedItem.Value);
                    int intTaxExempted = (new ILRFTaxSetupBS()).isTaxExemptedState(Convert.ToInt32(ddlTaxDescriptionType.SelectedValue), Convert.ToInt32(ViewState["PRGPRDID"]));
                    if (intTaxExempted == 1)
                    {
                        ShowError("This state has been Tax Exempted.");
                        return;
                    }
                    int intDuplicateUpdateCount = (new ILRFTaxSetupBS()).isTaxTypeAlreadyExist(TaxSetupBE.INCURRED_LOSS_REIM_FUND_TAX_ID, Convert.ToInt32(ddlTaxDescriptionType.SelectedValue), Convert.ToInt32(ViewState["PRGPRDID"]), Convert.ToInt32(ddlLOB.SelectedItem.Value));
                    if (intDuplicateUpdateCount == 0)
                    {
                        bool i = (new ILRFTaxSetupBS()).Update(TaxSetupBE);
                        ShowError("The entry has been saved.");
                    }
                    else if (intDuplicateUpdateCount == 1)
                    {
                        ShowError("The record cannot be saved. An identical record already exists.");
                    }
                    else if (intDuplicateUpdateCount == 2)
                    {
                        ShowError("The record cannot be saved. An identical record already exists. Please enable existing tax type.");
                    }
                }
                catch (RetroBaseException ee)
                {
                    ShowError(ee.Message, ee);
                }

                //get out of the edit mode
                lstTaxSetup.EditIndex = -1;
                lstTaxSetup.InsertItemPosition = InsertItemPosition.FirstItem;
                BindILRFTaxSetupListView();
            }
        }
        else if (e.CommandName == "DISABLE")
        {
            //Function to make Disable/Enable the MiscInvoice
            DisableRow(Convert.ToInt32(e.CommandArgument), false);
        }
        else if (e.CommandName == "ENABLE")
        {
            
            
            //Function to make Disable/Enable the MiscInvoice
            DisableRow(Convert.ToInt32(e.CommandArgument), true);
        }


    }
    #endregion
    /// <summary>
    /// Texas Tax:Function to make enable or disable a Tax setup
    /// </summary>
    /// <param name="intILRFTaxSetupID"></param>
    /// <param name="Flag"></param>
    #region DisableRow
    protected void DisableRow(int intILRFTaxSetupID, bool Flag)
    {
        try
        {
            ILRFTaxSetupBE TaxSetupBE = new ILRFTaxSetupBE();
            TaxSetupBE = (new ILRFTaxSetupBS()).getILRFTaxSetupRow(intILRFTaxSetupID);
            // Concurrency Code
            //PremiumAdjMiscInvoiceBE pamiBEConcurrent = (PremAdjMISCOLD.Where(o => o.PREM_ADJ_MISC_INVC_ID.Equals(intPremAdjMiscInvoiceID))).First();
            //bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(pamiBEConcurrent.UPDATE_DATE), Convert.ToDateTime(pamiBE.UPDATE_DATE));
            //if (!Concurrency)
            //    return;
            if (Flag)
            {
                int intTaxExempted = (new ILRFTaxSetupBS()).isTaxExemptedState(TaxSetupBE.TAX_TYP_ID, Convert.ToInt32(ViewState["PRGPRDID"]));
                if (intTaxExempted == 1)
                {
                    ShowError("This state has been Tax Exempted.");
                    return;
                }
            }
            TaxSetupBE.ACTV_IND = Flag;
            TaxSetupBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
            TaxSetupBE.UPDATE_DATE = DateTime.Now;
            bool i = (new ILRFTaxSetupBS()).Update(TaxSetupBE);
        }
        catch (RetroBaseException ee)
        {
            ShowError(ee.Message, ee);
        }
        BindILRFTaxSetupListView();

    }
    #endregion
    /// <summary>
    /// Texas Tax:Item Updating Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region ItemUpdating event
    protected void lstTaxSetup_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    { }
    #endregion
    /// <summary>
    /// Texas Tax:Item Edit Event of a ListView
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region ItemEdit Event
    protected void lstTaxSetup_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        lstTaxSetup.EditIndex = e.NewEditIndex;
        lstTaxSetup.DataSource = (new ILRFTaxSetupBS()).getILRFTaxSetupList(Convert.ToInt32(ViewState["PRGPRDID"]), AISMasterEntities.AccountNumber);
        lstTaxSetup.DataBind();

    }
    #endregion
    /// <summary>
    /// Texas Tax:Item Cancel Event of ListView
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region ItemCancel Event
    protected void lstTaxSetup_ItemCancel(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstTaxSetup.EditIndex = -1;
            lstTaxSetup.DataSource = (new ILRFTaxSetupBS()).getILRFTaxSetupList(Convert.ToInt32(ViewState["PRGPRDID"]), AISMasterEntities.AccountNumber);
            lstTaxSetup.DataBind();

        }

    }
    #endregion

    /// <summary>
    /// Texas Tax:Item Data Bound Event-it is called while binding each row to the listview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region ListView DataBound Event
    protected void lstTaxSetup_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trItemTemplate");
            if (tr != null)
            {
                tr.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                tr.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            }
            // //Get a handle to the ddlAccountlist DropDownList control
            //DropDownList ddlContactTypeList = (DropDownList)e.Item.FindControl("ddlMiscInvoiceTypelist");

            //Label lblPostTrnsTypeID = (Label)e.Item.FindControl("lblPostTrnsTypeID");

            //if ((ddlContactTypeList != null) & (lblPostTrnsTypeID != null))
            //{
            //    ddlContactTypeList.Items.FindByValue(lblPostTrnsTypeID.Text).Selected = true;
            //}
            //Restricting User to Update when the Adjustment is not in CALC Status


            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDisable");
            if (imgDelete != null)
            {
                HiddenField hid = (HiddenField)e.Item.FindControl("hidActive");
                imgDelete.CommandName = hid.Value == "True" ? "DISABLE" : "ENABLE";
                imgDelete.Attributes.Add("onclick", hid.Value == "True" ? "return confirm('Are you sure you want to Disable?');" : "return confirm('Are you sure you want to Enable?');");
            }
            //for Edit
            ListViewDataItem dataItem = (ListViewDataItem)e.Item;
            if (dataItem.DisplayIndex == lstTaxSetup.EditIndex)
            {
                Label lblTaxTypeID = (Label)e.Item.FindControl("lblILRFTaxTypeID");
                LinkButton lnkUpdate = (LinkButton)e.Item.FindControl("lbILRFTaxSetupUpdate");
                DropDownList ddlEditILRFTaxTyp = (DropDownList)e.Item.FindControl("ddlTaxDescriptionlist");
                DropDownList ddlLOB = (DropDownList)e.Item.FindControl("ddlLOB");
                IList<ILRFTaxSetupBE> ilistILRFTaxSetupBE = new List<ILRFTaxSetupBE>();
                ilistILRFTaxSetupBE = new ILRFTaxSetupBS().getTaxDescriptionListEditData(Convert.ToInt32(lblTaxTypeID.Text));
                ddlEditILRFTaxTyp.DataSource = ilistILRFTaxSetupBE;
                ddlEditILRFTaxTyp.DataTextField = "INCURRED_LOSS_REIM_FUND_TAX_TYPE";
                ddlEditILRFTaxTyp.DataValueField = "INCURRED_LOSS_REIM_FUND_TAX_ID";
                ddlEditILRFTaxTyp.DataBind();
                if ((ddlEditILRFTaxTyp != null) && (lblTaxTypeID != null))
                {
                    ddlEditILRFTaxTyp.Items.FindByValue(lblTaxTypeID.Text).Selected = true;
                }
                Label lblEditLOB = (Label)e.Item.FindControl("lblEditLOB");
                if ((ddlLOB != null) && (ilistILRFTaxSetupBE != null))
                {
                    ddlLOB.Items.FindByText(lblEditLOB.Text).Selected = true;
                }
                

            }
        }
    }
    #endregion
    /// 
    /// <summary>
    /// Texas Tax:Invokes when user clicks on Detials link of ILRF Setup
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region ILRFTaxes_Click Event
    protected void ILRFTaxes_Click(object sender, EventArgs e)
    {
        BindILRFTaxSetupListView();
    }
    #endregion
    /// <summary>
    /// Texas Tax:Function to Bind the Tax Setup based on program period and custmer
    /// </summary>
    #region BindILRFTaxSetupListView Method
    public void BindILRFTaxSetupListView()
    {
        pnlTaxSetup.Visible = true;
        pnlFormulaSetup.Visible = false;
        pnlILRFSetup.Enabled = false;
        pnlppEscrow.Enabled = false;
        lbDetails.Enabled = false;
        lbTaxes.Enabled = false;
        lstTaxSetup.DataSource = (new ILRFTaxSetupBS()).getILRFTaxSetupList(Convert.ToInt32(ViewState["PRGPRDID"]), AISMasterEntities.AccountNumber);
        lstTaxSetup.DataBind();
    }
    #endregion
    /// <summary>
    /// Texas Tax:Invokes when user clicks on ILRF Tax Setup Close link
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e">Void</param>
    #region lbILRFTaxSetupClose_Click Event
    protected void lbILRFTaxSetupClose_Click(object sender, EventArgs e)
    {
        pnlTaxSetup.Visible = false;
        pnlILRFSetup.Enabled = true;
        pnlppEscrow.Enabled = true;
        lbDetails.Enabled = true;
        lbTaxes.Enabled = true;

    }
    #endregion

    #endregion


    #region Tax Exemption setup

    /// <summary>
    /// Texas Tax:ListView Item Command Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Item Command Event
    protected void lstTaxExemptSetup_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        
            if (e.CommandName.ToUpper() == "SAVE")  //if it is new
            {
                ////if (CurrentAISUser.Role != GlobalConstants.ApplicationSecurityGroup.Manager)
                ////{
                ////    ShowError("Tax Exemption is a Manager only function.");
                ////    return;
                ////}
                ////Get the values  
                DropDownList ddlState = (DropDownList)e.Item.FindControl("ddlState");
                this.StateID = Convert.ToInt32(ddlState.SelectedItem.Value);
                this.CommandName = "SAVE";
               
                ILRFTaxSetuplist = (new ILRFTaxSetupBS()).getILRFTaxSetupListData(Convert.ToInt32(ViewState["PRGPRDID"]), Convert.ToInt32(this.StateID));
                IList<ILRFTaxSetupBE> ILRFTaxlist = ILRFTaxSetuplist.Where(cdd => cdd.ACTV_IND == true).ToList();

                int intDuplicateCount = (new TaxExemptionBS()).isTaxExemptAlreadyExist(0, this.StateID, Convert.ToInt32(ViewState["PRGPRDID"]));
                if (intDuplicateCount == 1)
                {
                    ShowError("The record cannot be saved. An identical record already exists.");
                    return;
                }
                if (intDuplicateCount == 2)
                {
                    ShowError("The record cannot be saved. An identical record already exists. Please enable the existing record for the particular state.");
                    return;
                }
                if (ILRFTaxlist.Count > 0)
                {
                    modalTaxExept.Show();
                    lbltaxMessage.Text = "Do you want to disable the following ALAE Amounts?";
                    ilrflisttaxsetup.DataSource = ILRFTaxlist;
                    ilrflisttaxsetup.DataBind();
                }
                else
                {
                    SaveData();
                }

            }
        
        else if (e.CommandName == "DISABLE")
        {
            //if (CurrentAISUser.Role != GlobalConstants.ApplicationSecurityGroup.Manager)
            //{
            //    ShowError("Tax Exemption is a Manager only function.");
            //    return;
            //}
            this.CommandName = "DISABLE";
            ViewState["TaxExemptSetupID"] = Convert.ToInt32(e.CommandArgument);
            
            
            TaxExemptionBE TaxExemptSetupBE = new TaxExemptionBE();
            TaxExemptSetupBE = (new TaxExemptionBS()).getTaxExemptSetupRow(Convert.ToInt32(e.CommandArgument));
            this.StateID = TaxExemptSetupBE.ST_ID;
            ILRFTaxSetuplist = (new ILRFTaxSetupBS()).getILRFTaxSetupListData(Convert.ToInt32(ViewState["PRGPRDID"]), TaxExemptSetupBE.ST_ID);
            IList<ILRFTaxSetupBE> ILRFTaxlist = ILRFTaxSetuplist.Where(cdd => cdd.ACTV_IND == false).ToList();
            
            if (ILRFTaxlist.Count > 0)
            {
                modalTaxExept.Show();
                lbltaxMessage.Text = "Do you want to enable the following ALAE Amounts?";
                ilrflisttaxsetup.DataSource = ILRFTaxlist;
                ilrflisttaxsetup.DataBind();
            }
            else
            {
                DisableData();
            }
          
           
        }
        else if (e.CommandName == "ENABLE")
        {
            //if (CurrentAISUser.Role != GlobalConstants.ApplicationSecurityGroup.Manager)
            //{
            //    ShowError("Tax Exemption is a Manager only function.");
            //    return;
            //}
            this.CommandName = "ENABLE";
            ViewState["TaxExemptSetupID"] = Convert.ToInt32(e.CommandArgument);
            
            TaxExemptionBE TaxExemptSetupBE = new TaxExemptionBE();
            TaxExemptSetupBE = (new TaxExemptionBS()).getTaxExemptSetupRow(Convert.ToInt32(e.CommandArgument));
            this.StateID = TaxExemptSetupBE.ST_ID;
            ILRFTaxSetuplist = (new ILRFTaxSetupBS()).getILRFTaxSetupListData(Convert.ToInt32(ViewState["PRGPRDID"]), TaxExemptSetupBE.ST_ID);
            IList<ILRFTaxSetupBE> ILRFTaxlist = ILRFTaxSetuplist.Where(cdd => cdd.ACTV_IND == true).ToList();
            
            if (ILRFTaxlist.Count > 0)
            {
                modalTaxExept.Show();
                lbltaxMessage.Text = "Do you want to disable the following ALAE Amounts?";
                ilrflisttaxsetup.DataSource = ILRFTaxlist;
                ilrflisttaxsetup.DataBind();
            }
            else
            {
                EnableData();
            }
           
        }


    }
    #endregion
    
    /// <summary>
    /// Texas Tax:Item Updating Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region ItemUpdating event
    protected void lstTaxExemptSetup_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    { }
    #endregion
    
    

    /// <summary>
    /// Texas Tax:Item Data Bound Event-it is called while binding each row to the listview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region ListView DataBound Event
    protected void lstTaxExemptSetup_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        AISBasePage aispage = new AISBasePage();
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trItemTemplate");
            if (tr != null)
            {
                tr.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                tr.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            }


            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDisable");
            if (imgDelete != null)
            {
                HiddenField hid = (HiddenField)e.Item.FindControl("hidActive");
                imgDelete.CommandName = hid.Value == "True" ? "DISABLE" : "ENABLE";
                imgDelete.Attributes.Add("onclick", hid.Value == "True" ? "return confirm('Are you sure you want to Disable?');" : "return confirm('Are you sure you want to Enable?');");
            }

            if (ROLENAME != GlobalConstants.ApplicationSecurityGroup.Manager && ROLENAME != GlobalConstants.ApplicationSecurityGroup.SystemAdmin)
            {
                imgDelete.Enabled = false;
            }
            else
            {
                imgDelete.Enabled = true;
            }
            //for Edit
            //ListViewDataItem dataItem = (ListViewDataItem)e.Item;
            //if (dataItem.DisplayIndex == lstTaxExemptSetup.EditIndex)
            //{
            //    DropDownList ddlState = (DropDownList)e.Item.FindControl("ddlState");
            //    Label lblTaxExemptSetupID = (Label)e.Item.FindControl("lbldTaxesExemptSetupId");
            //    TaxExemptionBE TaxExemptBE = new TaxExemptionBS().getTaxExemptSetupRow(Convert.ToInt32(lblTaxExemptSetupID.Text));

            //    if ((ddlState != null) && (TaxExemptBE != null))
            //    {
            //        ddlState.Items.FindByValue(TaxExemptBE.ST_ID.ToString()).Selected = true;
            //    }

            //}
        }
        
    }
    #endregion
    /// <summary>
    /// Item Created Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region lstTaxExemptSetup_ItemCreated
    protected void lstTaxExemptSetup_ItemCreated(object sender, ListViewItemEventArgs e)
    {

        if (e.Item.ItemType == ListViewItemType.InsertItem)
        {
            LinkButton lbItemSave = (LinkButton)e.Item.FindControl("lbItemSave");
            if (lbItemSave != null)
            {
                if (ROLENAME != GlobalConstants.ApplicationSecurityGroup.Manager && ROLENAME != GlobalConstants.ApplicationSecurityGroup.SystemAdmin)
                {
                    lbItemSave.Enabled = false;
                }
                else
                {
                    lbItemSave.Enabled = true;
                }
            }
        }

             

    }
    #endregion
    /// <summary>
    /// Reviewpopup button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region btnTaxExemptpopup Click Event
    protected void btnTaxExemptpopup_Click(object sender, EventArgs e)
    {
        modalTaxExept.Hide();

        if (Convert.ToString(this.CommandName)=="SAVE")
        {
            SaveData();

        }
        else if (Convert.ToString(this.CommandName) == "DISABLE")
        {
            DisableData();
        }
        else if (Convert.ToString(this.CommandName) == "ENABLE")
        {
            EnableData();
        }

        pnlTaxSetup.Visible = false;
        pnlILRFSetup.Enabled = true;
        pnlppEscrow.Enabled = true;
        lbDetails.Enabled = true;
        lbTaxes.Enabled = true;
        
    }
    #endregion

    /// <summary>
    /// This method is used to SAVE the Tax Exmeption details for the selected program period
    /// </summary>
    #region SaveData
    public void SaveData()
    {
        
        try
        {
                objDCTax.Connection.Open();
                trans_tax = objDCTax.Connection.BeginTransaction();
                objDCTax.Transaction = trans_tax;


                TAX_EXMP_SETUP TaxExemptNew = new TAX_EXMP_SETUP()
                {
                    prem_adj_pgm_id = Convert.ToInt32(ViewState["PRGPRDID"]),
                    actv_ind = true,
                    custmr_id = AISMasterEntities.AccountNumber,
                    st_id = Convert.ToInt32(this.StateID),
                    crte_user_id = CurrentAISUser.PersonID,
                    crte_dt = DateTime.Now

                };

                objDCTax.TAX_EXMP_SETUPs.InsertOnSubmit(TaxExemptNew);

                BindTaxExemptSetupListView();
               

            }
            catch (RetroBaseException ee)
            {
                ShowError(ee.Message, ee);
            }
            int tax_typ_id = (new TaxExemptionBS()).getTaxTypID(this.StateID);
            var ILRFTaxList = (from cdd in objDCTax.INCUR_LOS_REIM_FUND_TAX_SETUPs where cdd.prem_adj_pgm_id == Convert.ToInt32(ViewState["PRGPRDID"]) && cdd.actv_ind == true && cdd.tax_typ_id == tax_typ_id select cdd).ToList();
            for (int i = 0; i < ILRFTaxList.Count; i++)
            {
                ILRFTaxList[i].actv_ind = false;
                ILRFTaxList[i].updt_dt = DateTime.Now;
                ILRFTaxList[i].updt_user_id = CurrentAISUser.PersonID;
            }
            objDCTax.SubmitChanges();
            if (trans_tax != null)
                trans_tax.Commit();
            
            //BindTaxExemptSetupListView();
            ILRFTaxSetuplist = (new ILRFTaxSetupBS()).getILRFTaxSetupListData(Convert.ToInt32(ViewState["PRGPRDID"]), this.StateID);
            IList<ILRFTaxSetupBE> ILRFTax = ILRFTaxSetuplist.Where(cdd => cdd.ACTV_IND == false).ToList();
            if (ILRFTaxList.Count > 0)
            {
                lbltaxlistmessages.Text = "The following ALAE amounts are successfully disabled";
                modalTaxExemptList.Show();
                lstilrftax.DataSource = ILRFTax;
                lstilrftax.DataBind();
            }
            else
            {
                BindTaxExemptSetupListView();
                ClearError();
                ShowError("The entry has been saved.");
                }

                }
    #endregion


/// <summary>
    /// This method is used to Disable the Tax Exmeption details for the selected program period
    /// </summary>
    #region DisableData
    public void DisableData()
    {
        objDCTax.Connection.Open();
        trans_tax = objDCTax.Connection.BeginTransaction();
        objDCTax.Transaction = trans_tax;

        var TaxExmpt = (from cdd in objDCTax.TAX_EXMP_SETUPs where cdd.tax_exmp_setup_id == Convert.ToInt32(ViewState["TaxExemptSetupID"]) select cdd).FirstOrDefault();
        //TaxExmpt.actv_ind = false;
        TaxExmpt.actv_ind = false;
        TaxExmpt.updt_dt = DateTime.Now;
        TaxExmpt.updt_user_id = CurrentAISUser.PersonID;

        int tax_typ_id = (new TaxExemptionBS()).getTaxTypID(Convert.ToInt32(this.StateID));
        var ILRFTaxList = (from cdd in objDCTax.INCUR_LOS_REIM_FUND_TAX_SETUPs where cdd.prem_adj_pgm_id == Convert.ToInt32(ViewState["PRGPRDID"]) && cdd.actv_ind == false && cdd.tax_typ_id == tax_typ_id select cdd).ToList();
        for (int i = 0; i < ILRFTaxList.Count; i++)
        {
            //ILRFTaxList[i].actv_ind = false;
            ILRFTaxList[i].actv_ind = true;
            ILRFTaxList[i].updt_dt = DateTime.Now;
            ILRFTaxList[i].updt_user_id = CurrentAISUser.PersonID;
        }
        objDCTax.SubmitChanges();
        if (trans_tax != null)
            trans_tax.Commit();
       
        //BindTaxExemptSetupListView();
        ILRFTaxSetuplist = (new ILRFTaxSetupBS()).getILRFTaxSetupListData(Convert.ToInt32(ViewState["PRGPRDID"]), Convert.ToInt32(this.StateID));
        IList<ILRFTaxSetupBE> ILRFTax = ILRFTaxSetuplist.Where(cdd => cdd.ACTV_IND == true).ToList();
        if (ILRFTaxList.Count > 0)
        {
            lstilrftax.DataSource = ILRFTax;
            lstilrftax.DataBind();
            lbltaxlistmessages.Text = "The following ALAE amounts are successfully enabled";
            modalTaxExemptList.Show();
        }
        else
        {
            BindTaxExemptSetupListView();
            ClearError();
            ShowMessage("Record Disabled successfully.");
            
        }
    }
    #endregion

    /// <summary>
    /// This method is used to Enable the Tax Exmeption details for the selected program period
    /// </summary>
    #region EnableData
    public void EnableData()
    {
        objDCTax.Connection.Open();
        trans_tax = objDCTax.Connection.BeginTransaction();
        objDCTax.Transaction = trans_tax;

        var TaxExmpt = (from cdd in objDCTax.TAX_EXMP_SETUPs where cdd.tax_exmp_setup_id == Convert.ToInt32(ViewState["TaxExemptSetupID"]) select cdd).FirstOrDefault();
        TaxExmpt.actv_ind = true;
        TaxExmpt.updt_dt = DateTime.Now;
        TaxExmpt.updt_user_id = CurrentAISUser.PersonID;

        int tax_typ_id = (new TaxExemptionBS()).getTaxTypID(Convert.ToInt32(this.StateID));
        var ILRFTaxList = (from cdd in objDCTax.INCUR_LOS_REIM_FUND_TAX_SETUPs where cdd.prem_adj_pgm_id == Convert.ToInt32(ViewState["PRGPRDID"]) && cdd.actv_ind == true && cdd.tax_typ_id == tax_typ_id select cdd).ToList();
        for (int i = 0; i < ILRFTaxList.Count; i++)
        {
            ILRFTaxList[i].actv_ind = false;
            ILRFTaxList[i].updt_dt = DateTime.Now;
            ILRFTaxList[i].updt_user_id = CurrentAISUser.PersonID;
        }
        objDCTax.SubmitChanges();
        if (trans_tax != null)
            trans_tax.Commit();
        
        //BindTaxExemptSetupListView();
        ILRFTaxSetuplist = (new ILRFTaxSetupBS()).getILRFTaxSetupListData(Convert.ToInt32(ViewState["PRGPRDID"]), Convert.ToInt32(this.StateID));
        IList<ILRFTaxSetupBE> ILRFTax = ILRFTaxSetuplist.Where(cdd => cdd.ACTV_IND == false).ToList();
        if (ILRFTaxList.Count > 0)
        {
            lbltaxlistmessages.Text = "The following ALAE amounts are successfully disabled";
            modalTaxExemptList.Show();
            lstilrftax.DataSource = ILRFTax;
            lstilrftax.DataBind();
        }
        else
        {
            BindTaxExemptSetupListView();
            ClearError();
            ShowMessage("Record enabled successfully.");
        }

    }
    #endregion

    /// <summary>
    /// ReviewClose button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region btnTaxExemptClose Click Event
    protected void btnTaxExemptClose_Click(object sender, EventArgs e)
    {
        if (trans_tax != null)
            trans_tax.Rollback();
        modalTaxExept.Hide();

    }
    #endregion

    /// <summary>
    /// ReviewClose button click event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region btnTaxExemptlistpopup Click Event
    protected void btnTaxExemptlistpopup_Click(object sender, EventArgs e)
    {
        modalTaxExemptList.Hide();
        BindTaxExemptSetupListView();

    }
    #endregion

    private void BindTaxExemptSetupListView()
    {
        lstTaxExemptSetup.DataSource = (new TaxExemptionBS()).getILRFTaxSetupList(Convert.ToInt32(ViewState["PRGPRDID"]), AISMasterEntities.AccountNumber);
        lstTaxExemptSetup.DataBind();
    }

    #endregion


}




