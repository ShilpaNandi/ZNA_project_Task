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


public partial class LossFundInfo : AISBasePage
{
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
        list.Add(chkMiniLimitUnlimited);
        list.Add(ddlIBNRLDFNONE);
        list.Add(PolicyNumLstBox);
        list.Add(lstBoxPolicy);
        list.Add(lbILRFFormulaSetupClose);
        list.Add(lstILRFFormulaSetup);
        list.Add(btnSaveILRFFormulaSetup);
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

        if (AdjCompareESCROW.AdjparameterTypeID == ESCROWblaccess.GetLookUpID("Escrow", "ADJUSTMENT PARAMETER TYPE"))
        {
            if (txtPrevEscrowAmt.Text != "")
            {
                PrevEscrowAmt = decimal.Parse(txtPrevEscrowAmt.Text.Replace(",", ""));
            }

            if (AdjCompareESCROW.Escrow_PrevAmt == PrevEscrowAmt &&
                AdjCompareESCROW.Escrow_Diveser == Convert.ToInt32(txtDivedBy.Text) &&
                AdjCompareESCROW.Escrow_MnthsHeld == decimal.Parse(txtMnthsHeld.Text) &&
                AdjCompareESCROW.Escrow_PLMNumber == Convert.ToInt32(txtPLBmnts.Text))
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
        get { return (AdjustmentParameterSetupBE)Session["ESCROWParamBE"]; }
        set { Session["ESCROWParamBE"] = value; }
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
                }
                txtPLBmnts.Text = "12";
                txtMnthsHeld.Text = "02.5";
                //txtDivedBy.Text = "12";
                txtDivedBy.Text = "";
                txtPrevEscrowAmt.Text = "0";

                int intESCROW = new BLAccess().GetLookUpID("Escrow", "ADJUSTMENT PARAMETER TYPE");
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
                        txtMnthsHeld.Text = "0" + ESCROWParamBE.Escrow_MnthsHeld.ToString().Substring(0, 3);
                    }
                    else
                    {
                        txtMnthsHeld.Text = ESCROWParamBE.Escrow_MnthsHeld.ToString().Substring(0, 4);
                    }
                   // txtPrevEscrowAmt.Text = ESCROWParamBE.Escrow_PrevAmt.ToString().TrimEnd("0".ToCharArray());
                    if(ESCROWParamBE.Escrow_PrevAmt !=null)
                    txtPrevEscrowAmt.Text = decimal.Parse(ESCROWParamBE.Escrow_PrevAmt.ToString()).ToString("#,##0");
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
            ShowError(ex.Message,ex);
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
        {
            AdjStupESCROWBE.Escrow_PrevAmt = Convert.ToDecimal(txtPrevEscrowAmt.Text);
        }
        else
        {
            AdjStupESCROWBE.Escrow_PrevAmt = 0;
        }
        AdjStupESCROWBE.AdjparameterTypeID = ESCROWblaccess.GetLookUpID("Escrow", "ADJUSTMENT PARAMETER TYPE");
        AdjStupESCROWBE.Cstmr_Id = AISMasterEntities.AccountNumber;
      
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

            AdjParamPolInfo.deletePol(AdjStupESCROWBE.Cstmr_Id, AdjStupESCROWBE.adj_paramet_setup_id);
            if (AdjStupESCROWBE.adj_paramet_setup_id > 0)
            {

                for (int i = 0; i < PolicyNumLstBox.Items.Count; i++)
                {
                    if (PolicyNumLstBox.Items[i].Selected)
                    {
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
                    string AdjReviewResult = AdjPrmStupBS.getAdjParamResultOther(int.Parse(ViewState["PRGPRDID"].ToString()), AISMasterEntities.AccountNumber, ESCROWblaccess.GetLookUpID("Escrow", "ADJUSTMENT PARAMETER TYPE"));
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
            ShowError(ex.Message,ex);
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
            ShowError(ex.Message,ex);
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
        get { return (AdjustmentParameterSetupBE)Session["ILRFParamBESesn"]; }
        set { Session["ILRFParamBESesn"] = value; }
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
                    lbDetails.Enabled = true;
                    lbILRFSave.Text = "Update";
                    hidPremAdjPgmSetupID.Value = ILRFParamBESesn.adj_paramet_setup_id.ToString();
                    //txtInitialFundAmt.Text = Math.Round(Convert.ToDouble(ILRFParamBE.incur_los_reim_fund_initl_fund_amt), 0).ToString();
                    if(ILRFParamBESesn.incur_los_reim_fund_initl_fund_amt !=null)
                    txtInitialFundAmt.Text = Convert.ToDouble(ILRFParamBESesn.incur_los_reim_fund_initl_fund_amt).ToString("#,##0");
                    chkAggrLimitUnlimited.Checked = Convert.ToBoolean(ILRFParamBESesn.incur_los_reim_fund_unlim_agmt_lim_ind);
                    if (Convert.ToBoolean(ILRFParamBESesn.incur_los_reim_fund_unlim_agmt_lim_ind))
                        txtAggregateLimit.Enabled = false;
                    else
                        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                        {
                            txtAggregateLimit.Enabled = true;
                        }

                    //txtAggregateLimit.Text = Math.Round(Convert.ToDouble(ILRFParamBE.incur_los_reim_fund_aggr_lim_amt), 0).ToString();
                    if(ILRFParamBESesn.incur_los_reim_fund_aggr_lim_amt !=null)
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
                    if(ILRFParamBESesn.incur_los_reim_fund_min_lim_amt != null)
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
                    lbILRFSave.Text = "Save";
                    lbDetails.Enabled = false;
                    //txtAggregateLimit.Text = "0";
                    //txtMinimumLimit.Text = "0";
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
            ShowError(ex.Message,ex);
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

        }
        else
            lbILRFSave.Enabled = false;
        lbDetails.Enabled = flag;
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
            ShowError(ex.Message,ex);
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
            ShowError(ex.Message,ex);
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
            ShowError(ex.Message,ex);
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
            if (Session["FormulasList"] == null)
                Session["FormulasList"] = new List<ILRFFormulaBE>();
            return (IList<ILRFFormulaBE>)Session["FormulasList"];
        }
        set { Session["FormulasList"] = value; }
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
            ShowError(ex.Message,ex);
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
            ShowError(ex.Message,ex);
        }
    }
    #endregion ILRF Formula Setup
    #endregion ILRF Setup Tab
}

