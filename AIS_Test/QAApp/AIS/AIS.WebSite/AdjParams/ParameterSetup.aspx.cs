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

public partial class AdjParams_ParameterSetup : AISBasePage
{
    private Adj_Parameter_SetupBS AdjparamsetupInfo = new Adj_Parameter_SetupBS();

    private Adj_Paramet_PolBS AdjParamPolInfo = new Adj_Paramet_PolBS();

    private Adj_paramet_DtlBS AdjParmDtlsBS = new Adj_paramet_DtlBS();
    private BLAccess Lbablaccess = new BLAccess();


    //int m_iRowIdx =0;
    #region Properties
    //PGM_PERD_ID
    //    private int prmPerdID;
    public int PrmPerdID
    {
        get
        {
            if (Session["PrmPerdID"] == null)
                return 0;
            else
                return (int)Session["PrmPerdID"];
        }
        set
        {
            Session["PrmPerdID"] = value;
        }
    }

    private AdjustmentParameterDetailBE AdjparmDtlsBE
    {

        get
        {
            return ((Session["AdjparmDtlsBE"] == null) ?
                      (new AdjustmentParameterDetailBE()) : (AdjustmentParameterDetailBE)Session["AdjparmDtlsBE"]);
        }
        set
        {
            Session["AdjparmDtlsBE"] = value;
        }
        //get
        //{
        //    return (AdjustmentParameterDetailBE)Session["AdjprmtDetilsInfo-AdjparmDtlsBE"];
        //}
        //set { Session["AdjprmtDetilsInfo-AdjparmDtlsBE"] = value; }

    }

    protected AISBusinessTransaction AdjParameterTransactionWrapper
    {
        get
        {
            if ((AISBusinessTransaction)Session["AdjParameterTransaction"] == null)
                Session["AdjParameterTransaction"] = new AISBusinessTransaction();

            return (AISBusinessTransaction)Session["AdjParameterTransaction"];
        }
        set
        {
            Session["AdjParameterTransaction"] = value;
        }
    }

    Adj_Parameter_SetupBS adjPrmStupBS;
    private Adj_Parameter_SetupBS AdjPrmStupBS
    {
        get
        {
            if (adjPrmStupBS == null)
            {
                adjPrmStupBS = new Adj_Parameter_SetupBS();
                adjPrmStupBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
            }
            return adjPrmStupBS;
        }
        set
        {
            adjPrmStupBS = value;
        }
    }

    IList<AdjustmentParameterDetailBE> AdjParmCHFDtlBElst
    {
        get
        {
            if (Session["AdjParmCHFDtlBElst"] == null)
                Session["AdjParmCHFDtlBElst"] = new List<AdjustmentParameterDetailBE>();
            return (IList<AdjustmentParameterDetailBE>)Session["AdjParmCHFDtlBElst"];
        }
        set
        {
            Session["AdjParmCHFDtlBElst"] = value;
        }
    }

    IList<AdjustmentParameterDetailBE> AdjParmRMLDtlBElst
    {
        get
        {
            if (Session["AdjParmRMLDtlBElst"] == null)
                Session["AdjParmRMLDtlBElst"] = new List<AdjustmentParameterDetailBE>();
            return (IList<AdjustmentParameterDetailBE>)Session["AdjParmRMLDtlBElst"];
        }
        set
        {
            Session["AdjParmRMLDtlBElst"] = value;
        }
    }

    Adj_paramet_DtlBS adjPrmtDtlsBS;
    private Adj_paramet_DtlBS AdjPrmtDtlsBS
    {
        get
        {
            if (adjPrmtDtlsBS == null)
            {
                adjPrmtDtlsBS = new Adj_paramet_DtlBS();
                adjPrmtDtlsBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
            }
            return adjPrmtDtlsBS;
        }
        set
        {
            adjPrmtDtlsBS = value;
        }
    }

    //public IList<AdjustmentParameterSetupBE> Bindlist
    //{
    //    get
    //    {
    //        if (Session["Bindlist"] == null)
    //            return null;
    //        else
    //            return (List<AdjustmentParameterSetupBE>)Session["Bindlist"];
    //    }
    //    set
    //    {
    //        if(value.Count>0)
    //            Session["Bindlist"] = value;
    //    }
    //}

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

    AdjustmentParameterDetailBE adjParmDtlBE;
    private AdjustmentParameterDetailBE AdjParmDtlBE
    {
        get
        {
            if (adjParmDtlBE == null)
            {
                adjParmDtlBE = new AdjustmentParameterDetailBE();
            }
            return adjParmDtlBE;
        }
        set
        {
            adjParmDtlBE = value;
        }
    }


    //public IList<AdjustmentParameterDetailBE> AdjParmDtlBElst
    //{
    //    get 
    //    { 
    //        if(Session["AdjParmDtlBElst"]==null)
    //            return null;
    //        else
    //           return (List<AdjustmentParameterDetailBE>)Session["AdjParmDtlBElst"];
    //    }
    //    set
    //    {
    //        Session["AdjParmDtlBElst"] = value;
    //    }
    //}        


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
        //this.uc2ProgramPeriodLCF.OnItemCommand += new App_Shared_ProgramPeriod.ItemCommand(ProgramPeriod_ItemCommand);
        //this.uc2ProgramPeriodTM.OnItemCommand += new App_Shared_ProgramPeriod.ItemCommand(ProgramPeriod_ItemCommand);
        //this.uc2ProgramPeriodRML.OnItemCommand += new App_Shared_ProgramPeriod.ItemCommand(ProgramPeriod_ItemCommand);
        //this.uc2ProgramPeriodCHF.OnItemCommand += new App_Shared_ProgramPeriod.ItemCommand(ProgramPeriod_ItemCommand);

        PolicyNumLstBox.Attributes["OnClick"] =
            "Index_Changed('" + PolicyNumLstBox.ClientID + "','" + PolicyNumLstBox2.ClientID + "')";
        PolicyNumLstBox2.Attributes["OnClick"] =
            "Index_Changed2('" + PolicyNumLstBox.ClientID + "','" + PolicyNumLstBox2.ClientID + "')";
        //***** Commented this as part of Bug: 8086.
        // chkbxCHFPolicynolst.Attributes["OnClick"] =
        //   "Index_Changed('" + chkbxCHFPolicynolst.ClientID + "','" + chkbxCHF2Policynolst.ClientID + "')";
        //chkbxCHF2Policynolst.Attributes["OnClick"] =
        //  "Index_Changed2('" + chkbxCHFPolicynolst.ClientID + "','" + chkbxCHF2Policynolst.ClientID + "')";


        if (!Page.IsPostBack)
        {
            this.Master.Page.Title = "Parameter Setup";

            //lblcheckvalidations.Visible = false;
            //lblcheckvalidations.Text = "";
            AdjParameterTransactionWrapper = new AISBusinessTransaction();
        }

        //Checks Exiting without Save
        CheckExitWithoutSave();

    }

    private void CheckExitWithoutSave()
    {
        ArrayList list = new ArrayList();
        list.Add(txtCHFDeposit);
        list.Add(txtchfDeposit2);
        list.Add(txtLayLCFIPay);
        list.Add(txtLayLCFZurchPay);
        list.Add(txtlbaintdeposit);
        list.Add(txtlbaintdeposit2);
        list.Add(txtLCFAggtAmt);
        list.Add(txtLCFClmCAP);
        list.Add(chkbxCHF2Policynolst);
        list.Add(chkbxCHF2Policynolst);
        list.Add(chkbxCHFPolicynolst);
        list.Add(ChkPolicyPanelTM);
        list.Add(ddCHFBasisCharged);
        list.Add(ddCHFBasisCharged2);
        list.Add(rbtnCHFinclude);
        list.Add(rbtnCHFNotinclude);
        list.Add(PolicyNumLstBox);
        list.Add(PolicyNumLstBox2);
        list.Add(lnkBtn2CHFadj);
        list.Add(lnkBtn2CHFadjUpd);
        list.Add(lnkBtnCHF2Cancel);
        list.Add(lnkBtnCHFadj);
        list.Add(lnkBtnCHFadjUpd);
        list.Add(lnkBtnCHFCancel);
        list.Add(lnkBtnLBAadj);
        list.Add(lnkBtnLBAadjUpd);
        list.Add(lnkbtnLBADetailsClose);
        list.Add(lnkBtnLBASetup2Cancel);
        list.Add(lnkBtnLBASetupCancel);
        list.Add(lnkBtnLCFCncl);
        list.Add(lnkBtnRMLSetupCancel);
        list.Add(lnkbtnSaveLCH);
        list.Add(lnkbtnSAVERML);
        list.Add(lnkBtnTMCancel);
        list.Add(lnkBtnTMSave);
        list.Add(lnkBtnTMUpd);
        list.Add(lnkbtnUpdLCH);
        list.Add(lnkbtnUPDRML);
        list.Add(lnkbtnCHFDetailsClose);
        list.Add(lnkbtnLBADetailsClose);
        list.Add(lnkLCFClose);
        list.Add(lnkbtnLBADetailsClose);
        list.Add(lnkbtnCHFDetailsClose);
        ProcessExitFlag(list);
    }

    /// <summary>
    /// bool Function to check the LBA 1 record is new or existing. Used while saving the record
    /// </summary>
    /// <param name=""></param>
    /// <returns>bool(True/False)</returns>
    protected bool CheckNewincLBA
    {
        get { return (bool)ViewState["CheckNewincLBA"]; }
        set { ViewState["CheckNewincLBA"] = value; }
    }

    /// <summary>
    /// bool Function to check the LBA 2 record is new or existing. Used while saving the record
    /// </summary>
    /// <param name=""></param>
    /// <returns>bool(True/False)</returns>
    protected bool CheckNewNotincLBA
    {
        get { return (bool)ViewState["CheckNewNotincLBA"]; }
        set { ViewState["CheckNewNotincLBA"] = value; }
    }

    /// <summary>
    /// bool Function to check the LCF  record is new or existing. Used while saving the record
    /// </summary>
    /// <param name=""></param>
    /// <returns>bool(True/False)</returns>
    protected bool CheckNewLCF
    {
        get { return (bool)ViewState["CheckNewLCF"]; }
        set { ViewState["CheckNewLCF"] = value; }
    }

    /// <summary>
    /// bool Function to check the TM  record is new or existing. Used while saving the record
    /// </summary>
    /// <param name=""></param>
    /// <returns>bool(True/False)</returns>
    protected bool CheckNewTM
    {
        get { return (bool)ViewState["CheckNewTM"]; }
        set { ViewState["CheckNewTM"] = value; }
    }

    /// <summary>
    /// bool Function to check the RML  record is new or existing. Used while saving the record
    /// </summary>
    /// <param name=""></param>
    /// <returns>bool(True/False)</returns>
    protected bool CheckNewRML
    {
        get { return (bool)ViewState["CheckNewRML"]; }
        set { ViewState["CheckNewRML"] = value; }
    }

    /// <summary>
    /// bool Function to check the fist row of CHF  record is new or existing. Used while saving the record
    /// </summary>
    /// <param name=""></param>
    /// <returns>bool(True/False)</returns>
    protected bool CheckNewincCHF
    {
        get { return (bool)ViewState["CheckNewincCHF"]; }
        set { ViewState["CheckNewincCHF"] = value; }
    }

    /// <summary>
    /// bool Function to check the second row of CHF  record is new or existing. Used while saving the record
    /// </summary>
    /// <param name=""></param>
    /// <returns>bool(True/False)</returns>
    protected bool CheckNewNotincCHF
    {
        get { return (bool)ViewState["CheckNewNotincCHF"]; }
        set { ViewState["CheckNewNotincCHF"] = value; }
    }


    #region LBADetails

    /// <summary>
    /// Compare old LBA and LCF details(First Row of the Table) if both the old and the new details
    /// on the screen are same do not save
    /// </summary>
    /// <param name="AdjCompareLBA"></param>
    /// <param name="PolicyCount"></param>
    /// <returns></returns>
    protected bool CompareValues(AdjustmentParameterSetupBE AdjCompareLBA, int PolicyCount)
    {
        bool retValue = true;
        decimal lbaintdeposit = 0;
        decimal LCFAggtAmt = 0;
        decimal LCFClmCAP = 0;
        decimal LayLCFIPay = 0;
        decimal LayLCFZurchPay = 0;
        decimal CHFDeposit = 0;

        if (AdjCompareLBA.AdjparameterTypeID == Lbablaccess.GetLookUpID("LBA", "ADJUSTMENT PARAMETER TYPE"))
        {
            if (txtlbaintdeposit.Text != "")
            {
                lbaintdeposit = decimal.Parse(txtlbaintdeposit.Text.Replace(",", ""));
            }

            if (AdjCompareLBA.incld_ibnr_ldf_ind == bool.Parse(LdfIbnrInclCheckBox.Checked.ToString()) &&
                AdjCompareLBA.incld_ernd_retro_prem_ind == bool.Parse(IncludedInERPButton.Checked.ToString()) &&
                AdjCompareLBA.lba_Adjustment_typ == int.Parse(LBAAdjTypeDdl.SelectedValue) &&
                AdjCompareLBA.depst_amt == lbaintdeposit)
            {
                IList<AdjustmentParameterPolicyBE> LBAPolBE = AdjParamPolInfo.getAdjParamtrPol(AdjCompareLBA.adj_paramet_setup_id);
                if (LBAPolBE != null)
                {
                    if (PolicyCount == LBAPolBE.Count)
                    {
                        for (int Pol = 0; Pol < PolicyNumLstBox.Items.Count; Pol++)
                        {
                            foreach (AdjustmentParameterPolicyBE AdjParmPolLBA in LBAPolBE)
                                if (AdjParmPolLBA.coml_agmt_id == int.Parse(PolicyNumLstBox.Items[Pol].Value) && PolicyNumLstBox.Items[Pol].Selected == false)
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
                else
                {
                    retValue = false;
                }
            }
            else
            {
                retValue = false;
            }
            //return retValue;
        }
        else if (AdjCompareLBA.AdjparameterTypeID == Lbablaccess.GetLookUpID("RML", "ADJUSTMENT PARAMETER TYPE"))
        {
            IList<AdjustmentParameterPolicyBE> RMLPolBE = AdjParamPolInfo.getAdjParamtrPol(AdjCompareLBA.adj_paramet_setup_id);
            if (RMLPolBE != null)
            {
                if (PolicyCount == RMLPolBE.Count)
                {
                    for (int Pol = 0; Pol < ChkBoxLstRMLpolicy.Items.Count; Pol++)
                    {
                        foreach (AdjustmentParameterPolicyBE AdjParmPolRML in RMLPolBE)
                            if (AdjParmPolRML.coml_agmt_id == int.Parse(ChkBoxLstRMLpolicy.Items[Pol].Value) && ChkBoxLstRMLpolicy.Items[Pol].Selected == false)
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
                retValue = false; ;
            }

        }
        else if (AdjCompareLBA.AdjparameterTypeID == Lbablaccess.GetLookUpID("LCF", "ADJUSTMENT PARAMETER TYPE"))
        {
            if (txtLCFAggtAmt.Text != "")
            {
                LCFAggtAmt = decimal.Parse(txtLCFAggtAmt.Text.Replace(",", ""));
            }
            if (txtLCFClmCAP.Text != "")
            {
                LCFClmCAP = decimal.Parse(txtLCFClmCAP.Text.Replace(",", ""));
            }
            if (txtLayLCFIPay.Text != "")
            {
                LayLCFIPay = decimal.Parse(txtLayLCFIPay.Text.Replace(",", ""));
            }
            if (txtLayLCFZurchPay.Text != "")
            {
                LayLCFZurchPay = decimal.Parse(txtLayLCFZurchPay.Text.Replace(",", ""));
            }
            if (AdjCompareLBA.loss_convfact_aggamt == LCFAggtAmt &&
                AdjCompareLBA.loss_convfact_calimcap == LCFClmCAP &&
                AdjCompareLBA.lay_lossconv_FactInsPay == LayLCFIPay &&
                AdjCompareLBA.lay_lossconv_znapayamt == LayLCFZurchPay)
            {

                IList<AdjustmentParameterPolicyBE> LCFPolBE = AdjParamPolInfo.getAdjParamtrPol(AdjCompareLBA.adj_paramet_setup_id);
                if (LCFPolBE != null)
                {
                    if (PolicyCount == LCFPolBE.Count)
                    {
                        for (int Pol = 0; Pol < ckkboxlstPolicyno.Items.Count; Pol++)
                        {
                            foreach (AdjustmentParameterPolicyBE AdjParmPolLCF in LCFPolBE)
                                if (AdjParmPolLCF.coml_agmt_id == int.Parse(ckkboxlstPolicyno.Items[Pol].Value) && ckkboxlstPolicyno.Items[Pol].Selected == false)
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
                    retValue = false; ;
                }
            }
            else
            {
                retValue = false;
            }
        }
        else if (AdjCompareLBA.AdjparameterTypeID == Lbablaccess.GetLookUpID("CHF", "ADJUSTMENT PARAMETER TYPE"))
        {
            if (txtCHFDeposit.Text != "")
            {
                CHFDeposit = decimal.Parse(txtCHFDeposit.Text.Replace(",", ""));
            }

            if (AdjCompareLBA.clm_hndl_fee_basis_id == Convert.ToInt32(ddCHFBasisCharged.SelectedValue) &&
                AdjCompareLBA.depst_amt == CHFDeposit)
            {
                IList<AdjustmentParameterPolicyBE> CHFPolBE = AdjParamPolInfo.getAdjParamtrPol(AdjCompareLBA.adj_paramet_setup_id);
                if (CHFPolBE != null)
                {
                    if (PolicyCount == CHFPolBE.Count)
                    {
                        for (int Pol = 0; Pol < chkbxCHFPolicynolst.Items.Count; Pol++)
                        {
                            foreach (AdjustmentParameterPolicyBE AdjParmPolCHF in CHFPolBE)
                                if (AdjParmPolCHF.coml_agmt_id == int.Parse(chkbxCHFPolicynolst.Items[Pol].Value) && chkbxCHFPolicynolst.Items[Pol].Selected == false)
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
                else
                {
                    retValue = false;
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
    /// Compare old LBA and LCF details(Second Row of the Table) if both the old and the new details
    /// on the screen are same do not save
    /// </summary>
    /// <param name="AdjCompareLBA"></param>
    /// <param name="PolicyCount"></param>
    /// <returns></returns>
    protected bool CompareValuesecrow(AdjustmentParameterSetupBE AdjCompareLBA, int PolicyCount)
    {
        bool retValue = true;
        decimal lbaNotintdeposit = 0;
        decimal chfDeposit2 = 0;

        if (AdjCompareLBA.AdjparameterTypeID == Lbablaccess.GetLookUpID("LBA", "ADJUSTMENT PARAMETER TYPE"))
        {
            if (txtlbaintdeposit2.Text != "")
            {
                lbaNotintdeposit = decimal.Parse(txtlbaintdeposit2.Text.Replace(",", ""));
            }
            if (AdjCompareLBA.incld_ibnr_ldf_ind == bool.Parse(LdfIbnrInclCheckBox2.Checked.ToString()) &&
                AdjCompareLBA.incld_ernd_retro_prem_ind == bool.Parse(NotIncludedInERPbtn.Checked.ToString()) &&
                AdjCompareLBA.lba_Adjustment_typ == Convert.ToInt32(LBAAdjTypeDdl2.SelectedValue) &&
                AdjCompareLBA.depst_amt == lbaNotintdeposit)
            {
                IList<AdjustmentParameterPolicyBE> LBAPolBE = AdjParamPolInfo.getAdjParamtrPol(AdjCompareLBA.adj_paramet_setup_id);
                if (LBAPolBE != null)
                {
                    if (PolicyCount == LBAPolBE.Count)
                    {
                        for (int Pol = 0; Pol < PolicyNumLstBox2.Items.Count; Pol++)
                        {
                            foreach (AdjustmentParameterPolicyBE AdjParmPolLBA in LBAPolBE)
                                if (AdjParmPolLBA.coml_agmt_id == int.Parse(PolicyNumLstBox2.Items[Pol].Value) && PolicyNumLstBox2.Items[Pol].Selected == false)
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
                    retValue = false; ;
                }

            }
            else
            {
                retValue = false;
            }
        }
        else if (AdjCompareLBA.AdjparameterTypeID == Lbablaccess.GetLookUpID("Washington Adjustment TM", "ADJUSTMENT PARAMETER TYPE"))
        {
            IList<AdjustmentParameterPolicyBE> TMPolBE = AdjParamPolInfo.getAdjParamtrPol(AdjCompareLBA.adj_paramet_setup_id);
            if (TMPolBE != null)
            {
                if (PolicyCount == TMPolBE.Count)
                {
                    for (int Pol = 0; Pol < ChkBoxLstTMpolicy.Items.Count; Pol++)
                    {
                        foreach (AdjustmentParameterPolicyBE AdjParmPolTM in TMPolBE)
                            if (AdjParmPolTM.coml_agmt_id == int.Parse(ChkBoxLstTMpolicy.Items[Pol].Value) && ChkBoxLstTMpolicy.Items[Pol].Selected == false)
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
            else
            {
                retValue = false; ;
            }
        }
        else if (AdjCompareLBA.AdjparameterTypeID == Lbablaccess.GetLookUpID("CHF", "ADJUSTMENT PARAMETER TYPE"))
        {
            if (txtchfDeposit2.Text != "")
            {
                chfDeposit2 = decimal.Parse(txtchfDeposit2.Text.Replace(",", ""));
            }

            if (AdjCompareLBA.clm_hndl_fee_basis_id == Convert.ToInt32(ddCHFBasisCharged2.SelectedValue) &&
                AdjCompareLBA.depst_amt == chfDeposit2)
            {
                IList<AdjustmentParameterPolicyBE> CHFPolBE = AdjParamPolInfo.getAdjParamtrPol(AdjCompareLBA.adj_paramet_setup_id);
                if (CHFPolBE != null)
                {
                    if (PolicyCount == CHFPolBE.Count)
                    {
                        for (int Pol = 0; Pol < chkbxCHF2Policynolst.Items.Count; Pol++)
                        {
                            foreach (AdjustmentParameterPolicyBE AdjParmPolCHF in CHFPolBE)
                                if (AdjParmPolCHF.coml_agmt_id == int.Parse(chkbxCHF2Policynolst.Items[Pol].Value) && chkbxCHF2Policynolst.Items[Pol].Selected == false)
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
                else
                {
                    retValue = false;
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
    /// Save method used to assign the values to a database field from the screen and save\update the details
    /// with Transaction processing for the first row in LBA Tab
    /// </summary>
    /// <param name="AdjStupBE"></param>
    /// <returns></returns>
    protected AdjustmentParameterSetupBE SaveEntity(AdjustmentParameterSetupBE AdjStupBE)
    {
        
        AdjStupBE.prem_adj_pgm_id = int.Parse(ViewState["PRGPRDID"].ToString());
        if (txtlbaintdeposit.Text != "")
        {
            AdjStupBE.depst_amt = decimal.Parse(txtlbaintdeposit.Text.Replace(",", ""));
        }
        else
        {
            AdjStupBE.depst_amt = 0;
        }
        AdjStupBE.lba_Adjustment_typ = Convert.ToInt32(LBAAdjTypeDdl.SelectedValue);
        AdjStupBE.AdjparameterTypeID = Lbablaccess.GetLookUpID("LBA", "ADJUSTMENT PARAMETER TYPE");
        if (IncludedInERPButton.Checked == true)
        {
            AdjStupBE.incld_ernd_retro_prem_ind = true;
        }
        else
        {
            AdjStupBE.incld_ernd_retro_prem_ind = false;

        }
        if (LdfIbnrInclCheckBox.Checked == true)
        {
            AdjStupBE.incld_ibnr_ldf_ind = true;
        }
        else
        {
            AdjStupBE.incld_ibnr_ldf_ind = false;
        }
        AdjStupBE.Cstmr_Id = AISMasterEntities.AccountNumber;


        if (CheckNewincLBA == false)
        {
            AdjStupBE.UPDATE_DATE = DateTime.Now;
            AdjStupBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
        }
        else
        {
            AdjStupBE.actv_ind = true;
            AdjStupBE.CREATE_DATE = DateTime.Now;
            AdjStupBE.CREATE_USER_ID = CurrentAISUser.PersonID;
        }
        //AdjStupBE.SetContext(AdjParameterTransactionWrapper);

        bool ResultLBA = AdjPrmStupBS.Update(AdjStupBE);
        if (ResultLBA)
        {
            bool aptFlag = AdjParameterTransactionWrapper.SubmitTransactionChanges();
            ShowConcurrentConflict(aptFlag, AdjParameterTransactionWrapper.ErrorMessage);
            if (CheckNewincLBA == false)
            {

                //Code for logging into Audit Transaction Table 
                ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                audtBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
                audtBS.Save(AISMasterEntities.AccountNumber, AdjStupBE.prem_adj_pgm_id, GlobalConstants.AuditingWebPage.LBASetup, CurrentAISUser.PersonID);
                //AdjParameterTransactionWrapper.SubmitTransactionChanges();
            }

            AdjParamPolInfo.deletePol(AdjStupBE.Cstmr_Id, AdjStupBE.adj_paramet_setup_id);
            if (AdjStupBE.adj_paramet_setup_id > 0)
            {

                for (int i = 0; i < PolicyNumLstBox.Items.Count; i++)
                {
                    if (PolicyNumLstBox.Items[i].Selected)
                    {

                        AdjustmentParameterPolicyBE LBAPolicylinkBE = new AdjustmentParameterPolicyBE();
                        LBAPolicylinkBE.adj_paramet_setup_id = AdjStupBE.adj_paramet_setup_id;
                        LBAPolicylinkBE.custmrID = AISMasterEntities.AccountNumber;
                        LBAPolicylinkBE.PrmadjPRgmID = int.Parse(ViewState["PRGPRDID"].ToString());
                        LBAPolicylinkBE.coml_agmt_id = int.Parse(PolicyNumLstBox.Items[i].Value);
                        LBAPolicylinkBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                        LBAPolicylinkBE.CREATE_DATE = DateTime.Now;
                        AdjParamPolInfo.Update(LBAPolicylinkBE);

                    }
                }
            }

            if (CheckNewNotincLBA == false)
            {
                AdjustmentParameterSetupBE AdjparmLBAsetupBE = new AdjustmentParameterSetupBE();
                AdjparmLBAsetupBE = AdjparamsetupInfo.getAdjParamRow(int.Parse(ViewState["ADJLBAnonPRGID"].ToString()));
                AdjParamPolInfo.deletePol(AdjparmLBAsetupBE.Cstmr_Id, AdjparmLBAsetupBE.adj_paramet_setup_id);
                if (AdjparmLBAsetupBE.adj_paramet_setup_id > 0)
                {
                    int polCount = 0;
                    for (int i = 0; i < PolicyNumLstBox2.Items.Count; i++)
                    {
                        if (PolicyNumLstBox2.Items[i].Selected)
                        {
                            ++polCount;
                            AdjustmentParameterPolicyBE LBAPolicylinkBE = new AdjustmentParameterPolicyBE();
                            LBAPolicylinkBE.adj_paramet_setup_id = AdjparmLBAsetupBE.adj_paramet_setup_id;
                            LBAPolicylinkBE.custmrID = AISMasterEntities.AccountNumber;
                            LBAPolicylinkBE.PrmadjPRgmID = int.Parse(ViewState["PRGPRDID"].ToString());
                            LBAPolicylinkBE.coml_agmt_id = int.Parse(PolicyNumLstBox2.Items[i].Value);
                            LBAPolicylinkBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                            LBAPolicylinkBE.CREATE_DATE = DateTime.Now;
                            AdjParamPolInfo.Update(LBAPolicylinkBE);
                        }
                    }
                    if (polCount == 0)
                    {
                        AdjParmDtlsBS.deleteAdjParamtrDtls(AdjparmLBAsetupBE.prem_adj_pgm_id, AdjparmLBAsetupBE.adj_paramet_setup_id, AdjparmLBAsetupBE.Cstmr_Id);
                        LinkButtonDetail.Enabled = false;
                    }
                }

            }
            bool aptwFlag = AdjParameterTransactionWrapper.SubmitTransactionChanges();
            ShowConcurrentConflict(aptwFlag, AdjParameterTransactionWrapper.ErrorMessage);
        }
        else
        {
            AdjParameterTransactionWrapper.RollbackChanges();
        }

        return AdjStupBE;
    }

    /// <summary>
    /// Save method used to assign the values to a database field from the screen and save\update the details
    /// with Transaction processing for the Second row in LBA Tab
    /// </summary>
    /// <param name="AdjStupBE"></param>
    /// <returns></returns>
    protected AdjustmentParameterSetupBE SaveEntityscdrow(AdjustmentParameterSetupBE AdjStupBE)
    {
        AdjStupBE.prem_adj_pgm_id = int.Parse(ViewState["PRGPRDID"].ToString());
        if (txtlbaintdeposit2.Text != "")
        {
            AdjStupBE.depst_amt = decimal.Parse(txtlbaintdeposit2.Text.Replace(",", ""));
        }
        else
        {
            AdjStupBE.depst_amt = 0;
        }
        //AdjStupBE.depst_amt = decimal.Parse(txtlbaintdeposit2.Text.Replace(",", ""));
        AdjStupBE.lba_Adjustment_typ = Convert.ToInt32(LBAAdjTypeDdl2.SelectedValue);
        AdjStupBE.AdjparameterTypeID = Lbablaccess.GetLookUpID("LBA", "ADJUSTMENT PARAMETER TYPE");
        if (NotIncludedInERPbtn.Checked == true)
        {
            AdjStupBE.incld_ernd_retro_prem_ind = true;
        }
        else
        {
            AdjStupBE.incld_ernd_retro_prem_ind = false;

        }
        if (LdfIbnrInclCheckBox2.Checked == true)
        {
            AdjStupBE.incld_ibnr_ldf_ind = true;
        }
        else
        {
            AdjStupBE.incld_ibnr_ldf_ind = false;
        }
        AdjStupBE.Cstmr_Id = AISMasterEntities.AccountNumber;

        if (CheckNewNotincLBA == false)
        {

            AdjStupBE.UPDATE_DATE = DateTime.Now;
            AdjStupBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
        }
        else
        {
            AdjStupBE.actv_ind = true;
            AdjStupBE.CREATE_DATE = DateTime.Now;
            AdjStupBE.CREATE_USER_ID = CurrentAISUser.PersonID;
        }

        bool ResultLBA = AdjparamsetupInfo.Update(AdjStupBE);
        if (ResultLBA)
        {
            bool aptwFlag = AdjParameterTransactionWrapper.SubmitTransactionChanges();
            ShowConcurrentConflict(aptwFlag, AdjParameterTransactionWrapper.ErrorMessage);

            if (CheckNewNotincLBA == false)
            {
                //Code for logging into Audit Transaction Table 
                ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                audtBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
                audtBS.Save(AISMasterEntities.AccountNumber, AdjStupBE.prem_adj_pgm_id, GlobalConstants.AuditingWebPage.LBASetup, CurrentAISUser.PersonID);
                //AdjParameterTransactionWrapper.SubmitTransactionChanges();
            }
            AdjParamPolInfo.deletePol(AdjStupBE.Cstmr_Id, AdjStupBE.adj_paramet_setup_id);
            if (AdjStupBE.adj_paramet_setup_id > 0)
            {

                for (int i = 0; i < PolicyNumLstBox2.Items.Count; i++)
                {
                    if (PolicyNumLstBox2.Items[i].Selected)
                    {
                        AdjustmentParameterPolicyBE LBAPolicylinkBE = new AdjustmentParameterPolicyBE();
                        LBAPolicylinkBE.adj_paramet_setup_id = AdjStupBE.adj_paramet_setup_id;
                        LBAPolicylinkBE.custmrID = AISMasterEntities.AccountNumber;
                        LBAPolicylinkBE.PrmadjPRgmID = int.Parse(ViewState["PRGPRDID"].ToString());
                        LBAPolicylinkBE.coml_agmt_id = int.Parse(PolicyNumLstBox2.Items[i].Value);
                        LBAPolicylinkBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                        LBAPolicylinkBE.CREATE_DATE = DateTime.Now;
                        AdjParamPolInfo.Update(LBAPolicylinkBE);
                    }
                }
            }

            if (CheckNewincLBA == false)
            {
                AdjustmentParameterSetupBE AdjparmLBAsetupBE = new AdjustmentParameterSetupBE();
                AdjparmLBAsetupBE = AdjparamsetupInfo.getAdjParamRow(int.Parse(ViewState["ADJLBAPRGID"].ToString()));

                AdjParamPolInfo.deletePol(AdjparmLBAsetupBE.Cstmr_Id, AdjparmLBAsetupBE.adj_paramet_setup_id);
                if (AdjparmLBAsetupBE.adj_paramet_setup_id > 0)
                {
                    int polCount = 0;
                    for (int i = 0; i < PolicyNumLstBox.Items.Count; i++)
                    {
                        if (PolicyNumLstBox.Items[i].Selected)
                        {
                            ++polCount;
                            AdjustmentParameterPolicyBE LBAPolicylinkBE = new AdjustmentParameterPolicyBE();
                            LBAPolicylinkBE.adj_paramet_pol_id = 0;
                            LBAPolicylinkBE.adj_paramet_setup_id = AdjparmLBAsetupBE.adj_paramet_setup_id;
                            LBAPolicylinkBE.custmrID = AISMasterEntities.AccountNumber;
                            LBAPolicylinkBE.PrmadjPRgmID = int.Parse(ViewState["PRGPRDID"].ToString());
                            LBAPolicylinkBE.coml_agmt_id = int.Parse(PolicyNumLstBox.Items[i].Value);
                            LBAPolicylinkBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                            LBAPolicylinkBE.CREATE_DATE = DateTime.Now;
                            AdjParamPolInfo.Update(LBAPolicylinkBE);
                        }
                    }
                    if (polCount == 0)
                    {
                        AdjParmDtlsBS.deleteAdjParamtrDtls(AdjparmLBAsetupBE.prem_adj_pgm_id, AdjparmLBAsetupBE.adj_paramet_setup_id, AdjparmLBAsetupBE.Cstmr_Id);
                        LinkButton2.Enabled = false;
                    }
                }
            }
            bool aptwFlg = AdjParameterTransactionWrapper.SubmitTransactionChanges();
            ShowConcurrentConflict(aptwFlg, AdjParameterTransactionWrapper.ErrorMessage);
        }
        else
        {
            AdjParameterTransactionWrapper.RollbackChanges();
        }

        return AdjStupBE;
    }

    /// <summary>
    /// Save LBA-Information 1st grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLBAInfoDetailsSave_Click(object sender, EventArgs e)
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

            if (PolCount > 0)
            {


                if (CheckNewincLBA == false)
                {

                    //adjParmStupBE = Bindlist.Single(bl => bl.adj_paramet_setup_id == int.Parse(ViewState["ADJLBAPRGID"].ToString()));
                    adjParmStupBE = AdjPrmStupBS.getAdjParamRow(int.Parse(ViewState["ADJLBAPRGID"].ToString()));
                    //Concurrency Issue
                    AdjustmentParameterSetupBE adjParmStupBEold = (Bindlist.Where(o => o.adj_paramet_setup_id.Equals(int.Parse(ViewState["ADJLBAPRGID"].ToString())))).First();
                    bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmStupBEold.UPDATE_DATE), Convert.ToDateTime(adjParmStupBE.UPDATE_DATE));
                    if (!con)
                        return;
                    //End
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
                    //To check Concurrency on Save for LBA
                    if (CheckNewincLBA == true)
                    {
                        bool strIncluded = true;
                        string AdjReviewResultLBA = AdjPrmStupBS.getAdjParamResult(int.Parse(ViewState["PRGPRDID"].ToString()), AISMasterEntities.AccountNumber, Lbablaccess.GetLookUpID("LBA", "ADJUSTMENT PARAMETER TYPE"), strIncluded);
                        if (AdjReviewResultLBA == "true")
                        {
                            ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
                            return;
                        }
                    }
                    //End
                    adjParmStupBE = SaveEntity(AdjParmStupBE);
                    BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
                }
                //else 
                //{
                //    ShowMessage("No information has been changed to Save");
                //}
                //this.lblcheckvalidations.Visible = false;
                //this.lblcheckvalidations.Text = "";
                //lnkBtnLBAadj.CommandArgument = adjParmStupBE.adj_paramet_setup_id.ToString();
                //BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
            }
            else
            {
                //this.lblcheckvalidations.Visible = true;
                //this.lblcheckvalidations.Text = " * Please select at least one policy before saving LBA information";
                ShowMessage("Please select at least one policy before saving LBA information");
            }
        }
        catch (Exception ex)
        {
            ShowError((ex.Message.Contains("changed"))?GlobalConstants.ErrorMessage.RowNotFoundOrChanged:GlobalConstants.ErrorMessage.ServerTooBusy,ex);
        }
    }

    /// <summary>
    /// Save LBA-Information 2nd grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLBAInfo2DetailsSave_Click(object sender, EventArgs e)
    {
        try
        {
            int PolCount2 = 0;
            bool Flag = false;

            for (int Pol2 = 0; Pol2 < PolicyNumLstBox2.Items.Count; Pol2++)
            {
                if (PolicyNumLstBox2.Items[Pol2].Selected)
                {
                    PolCount2 = PolCount2 + 1;
                }
            }

            if (PolCount2 > 0)
            {
                if (CheckNewNotincLBA == false)
                {
                    adjParmStupBE = AdjPrmStupBS.getAdjParamRow(int.Parse(ViewState["ADJLBAnonPRGID"].ToString()));
                    //Concurrency Issue
                    AdjustmentParameterSetupBE adjParmStupBEold = (Bindlist.Where(o => o.adj_paramet_setup_id.Equals(int.Parse(ViewState["ADJLBAnonPRGID"].ToString())))).First();
                    bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmStupBEold.UPDATE_DATE), Convert.ToDateTime(adjParmStupBE.UPDATE_DATE));
                    if (!con)
                        return;
                    //End
                    //adjParmStupBE = Bindlist.Single(bl => bl.adj_paramet_setup_id == int.Parse(ViewState["ADJLBAnonPRGID"].ToString()));
                    if (CompareValuesecrow(adjParmStupBE, PolCount2))
                    {
                        Flag = true;
                    }
                    else
                    {
                        Flag = false;
                    }
                }
//                if (!Flag)
//                {
                //To check Concurrency on Save for LBA
                if (CheckNewNotincLBA == true)
                {
                    bool strIncluded=false;                                               
                    string AdjReviewResultLBA = AdjPrmStupBS.getAdjParamResult(int.Parse(ViewState["PRGPRDID"].ToString()), AISMasterEntities.AccountNumber, Lbablaccess.GetLookUpID("LBA", "ADJUSTMENT PARAMETER TYPE"), strIncluded);
                    if (AdjReviewResultLBA == "true")
                    {
                        ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
                        return;
                    }
                }
                //End
                    adjParmStupBE = SaveEntityscdrow(AdjParmStupBE);
                    BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
//                }
                //else
                //{
                //    ShowMessage("No information has been changed to Save");
                //}
                //this.lblcheckvalidations.Visible = false;
                //this.lblcheckvalidations.Text = "";
                //BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
            }
            else
            {
                //this.lblcheckvalidations.Visible = true;
                //this.lblcheckvalidations.Text = " * Please select at least one policy before saving LBA information";
                ShowMessage("Please select at least one policy before saving LBA information");
            }
        }
        catch (Exception ex)
        {
            ShowError((ex.Message.Contains("changed")) ? GlobalConstants.ErrorMessage.RowNotFoundOrChanged : GlobalConstants.ErrorMessage.ServerTooBusy,ex );
        }
    }

    /// <summary>
    /// Not used till now.
    /// </summary>
    /// <param name="sener"></param>
    /// <param name="e"></param>
    protected void btnlbasetupinfodetails_click(object sener, EventArgs e)
    {

    }

    /// <summary>
    /// Show LBA information Set up details for first row
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLBAInfoDetailsInfo_Click(object sender, EventArgs e)
    {
        if (ViewState["ADJLBAPRGID"] != null)
        {
            //Close LCF listview in LCF TAB
            //this.pnlppLCF.Enabled = true;
            this.pnlAdjLCF.Enabled = true;
            this.lstLCFSetuplistView.Visible = false;
            this.lblLCFdtls.Visible = false;
            this.lnkLCFClose.Visible = false;

            //Close RML listview in RML TAB
            //this.pnlppRML.Enabled = true;
            this.pnlAdjRML.Enabled = true;
            this.lstRMLSetuplistView.Visible = false;
            this.lblRMLdtls.Visible = false;
            this.lnkBtnCloseRMLdtlsID.Visible = false;

            //Close WA-AX listview in TaxMultiplier TAB
            //this.pnlppTM.Enabled = true;
            this.pnlAdjTM.Enabled = true;
            this.lstTMSetuplistView.Visible = false;
            this.lblTMdtls.Visible = false;
            this.lnkBtnCloseTMdtlsID.Visible = false;

            //Close CHF listview in CHF TAB
            //this.pnlppCHF.Enabled = true;
            this.pnlADlCHF.Enabled = true;
            this.chfsetupdetailslistview.Visible = false;
            this.CHFSetupDetailsLabel.Visible = false;
            this.lnkbtnCHFDetailsClose.Visible = false;

            if (this.LBASetupDetailsLabel.Visible == false && this.lnkbtnLBADetailsClose.Visible == false)
            {
                this.LBASetupDetailsLabel.Visible = true;
                this.lnkbtnLBADetailsClose.Visible = true;
            }
            adjParmDtlBE = null;
            //this.pnlpp.Enabled = false;
            //this.lblcheckvalidations.Visible = false;
            hdnAdjPrmsetup2txtBox.Text = "0";
            hdnAdjPrmsetup1txtBox.Text = ViewState["ADJLBAPRGID"].ToString();
            this.lbasetupdetailslistview.EditIndex = -1;
            BindLBAniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), int.Parse(ViewState["ADJLBAPRGID"].ToString()), AISMasterEntities.AccountNumber);
            this.pnlAdlLBA.Enabled = false;
        }
        else
        {
            ShowMessage("Please save Policy information and LBA Adjustment type before selecting the Detail information");
        }
    }

    /// <summary>
    /// Show LBA information Set up details for second row
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLBA2InfoDetailsInfo_Click(object sender, EventArgs e)
    {
        if (ViewState["ADJLBAnonPRGID"] != null)
        {
            //Close LCF listview in LCF TAB
            //this.pnlppLCF.Enabled = true;
            this.pnlAdjLCF.Enabled = true;
            this.lstLCFSetuplistView.Visible = false;
            this.lblLCFdtls.Visible = false;
            this.lnkLCFClose.Visible = false;

            //Close RML listview in RML TAB
            //this.pnlppRML.Enabled = true;
            this.pnlAdjRML.Enabled = true;
            this.lstRMLSetuplistView.Visible = false;
            this.lblRMLdtls.Visible = false;
            this.lnkBtnCloseRMLdtlsID.Visible = false;

            //Close LBA listview in LBA TAB
            //this.pnlpp.Enabled = true;
            this.pnlAdlLBA.Enabled = true;
            this.lbasetupdetailslistview.Visible = false;
            this.LBASetupDetailsLabel.Visible = false;
            this.lnkbtnLBADetailsClose.Visible = false;

            //Close WA-AX listview in TaxMultiplier TAB
            //this.pnlppTM.Enabled = true;
            this.pnlAdjTM.Enabled = true;
            this.lstTMSetuplistView.Visible = false;
            this.lblTMdtls.Visible = false;
            this.lnkBtnCloseTMdtlsID.Visible = false;

            //Close CHF listview in CHF TAB
            //this.pnlppCHF.Enabled = true;
            this.pnlADlCHF.Enabled = true;
            this.chfsetupdetailslistview.Visible = false;
            this.CHFSetupDetailsLabel.Visible = false;
            this.lnkbtnCHFDetailsClose.Visible = false;

            if (this.LBASetupDetailsLabel.Visible == false && this.lnkbtnLBADetailsClose.Visible == false)
            {
                this.LBASetupDetailsLabel.Visible = true;
                this.lnkbtnLBADetailsClose.Visible = true;
            }
            adjParmDtlBE = null;
            //this.pnlpp.Enabled = false;
            //this.lblcheckvalidations.Visible = false;
            hdnAdjPrmsetup1txtBox.Text = "0";
            hdnAdjPrmsetup2txtBox.Text = ViewState["ADJLBAnonPRGID"].ToString();
            this.lbasetupdetailslistview.EditIndex = -1;
            BindLBAniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), int.Parse(ViewState["ADJLBAnonPRGID"].ToString()), AISMasterEntities.AccountNumber);
            this.pnlAdlLBA.Enabled = false;
        }
        else
        {
            ShowMessage("Please save Policy information and LBA Adjustment type before selecting the Detail information");
        }
    }
    #region Maintaining Session for AdjustmentParameterDetailBE
    private IList<AdjustmentParameterDetailBE> AdjParmDtlBElst
    {
        get
        {
            if (Session["AdjParmDtlBElst"] == null)
                Session["AdjParmDtlBElst"] = new List<AdjustmentParameterDetailBE>();
            return (IList<AdjustmentParameterDetailBE>)Session["AdjParmDtlBElst"];
        }
        set { Session["AdjParmDtlBElst"] = value; }
    }

    #endregion
    /// <summary>
    /// Bind the listview for LBA details 
    /// </summary>
    /// <param name="prgmperiodID"></param>
    /// <param name="AdjPrmtSetupID"></param>
    /// <param name="CustomerID"></param>
    public void BindLBAniformationDetails(int prgmperiodID, int AdjPrmtSetupID, int CustomerID)
    {
        //lblcheckvalidations.Visible = false;
        //lblcheckvalidations.Text = "";
        if (this.LBASetupDetailsLabel.Visible == false && this.lnkbtnLBADetailsClose.Visible == false)
        {
            this.LBASetupDetailsLabel.Visible = true;
            this.lnkbtnLBADetailsClose.Visible = true;
        }
        Adj_paramet_DtlBS AdjLBAprmDtlBS = new Adj_paramet_DtlBS();
        //IList<AdjustmentParameterDetailBE> AdjParmDtlBElst 
        //IList<AdjustmentParameterDetailBE> 
        AdjParmDtlBElst = AdjLBAprmDtlBS.getLBAAdjParamtrDtls(prgmperiodID, AdjPrmtSetupID, CustomerID);
        this.lbasetupdetailslistview.DataSource = AdjParmDtlBElst;
        this.lbasetupdetailslistview.DataBind();
        this.lbasetupdetailslistview.Visible = true;
    }

    /// <summary>
    /// This method is not used now was created initially to only display those
    /// states that are not been selected earlier
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LBAsetupStateDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //e.InputParameters["prgmprmID"] = PrmPerdID;
        e.InputParameters["prgmprmID"] = int.Parse(ViewState["PRGPRDID"].ToString());
        //e.InputParameters["AdjParterSetupID"] = (int)ViewState["ADJLBAPRGID"];
        if (this.hdnAdjPrmsetup1txtBox.Text == "0")
        {
            e.InputParameters["AdjParterSetupID"] = Convert.ToInt32(hdnAdjPrmsetup2txtBox.Text);
        }
        else
        {
            e.InputParameters["AdjParterSetupID"] = Convert.ToInt32(hdnAdjPrmsetup1txtBox.Text);
        }
        e.InputParameters["Acntid"] = AISMasterEntities.AccountNumber;
    }

    /// <summary>
    /// Not used till now. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LBAAdjustmentTypeDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {


    }

    /// <summary>
    /// Cancel any changes if user had selected any items in list view for updation
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstLBAParmsetupDtls_ItemCancel(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lbasetupdetailslistview.EditIndex = -1;

            int adjParmsetUpID;
            if (hdnAdjPrmsetup1txtBox.Text == "0")
            {
                adjParmsetUpID = Convert.ToInt32(hdnAdjPrmsetup2txtBox.Text);
            }
            else
            {
                adjParmsetUpID = Convert.ToInt32(hdnAdjPrmsetup1txtBox.Text);
            }

            BindLBAniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);
        }
        else if (e.CancelMode == ListViewCancelMode.CancelingInsert)
        {
            CancelUpdateMode(); //Back to normal mode.
        }
    }

    /// <summary>
    /// Function used with the above lstLBAParmsetupDtls_ItemCancel 
    /// </summary>
    protected void CancelUpdateMode()
    {
        lbasetupdetailslistview.InsertItemPosition = InsertItemPosition.None;
        int adjParmsetUpID;
        if (hdnAdjPrmsetup1txtBox.Text == "0")
        {
            adjParmsetUpID = Convert.ToInt32(hdnAdjPrmsetup2txtBox.Text);
        }
        else
        {
            adjParmsetUpID = Convert.ToInt32(hdnAdjPrmsetup1txtBox.Text);
        }

        BindLBAniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);
    }

    /// <summary>
    /// Get LBA Information details from database to fill the list view cotrol 
    /// passing 3 parameters AccountID, CustomerID and ProgramPeriodID
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstLBAParmsetupDtls_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        lbasetupdetailslistview.EditIndex = e.NewEditIndex;
        int adjParmsetUpID;
        if (hdnAdjPrmsetup1txtBox.Text == "0")
        {
            adjParmsetUpID = Convert.ToInt32(hdnAdjPrmsetup2txtBox.Text);
        }
        else
        {
            adjParmsetUpID = Convert.ToInt32(hdnAdjPrmsetup1txtBox.Text);
        }

        BindLBAniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);
        //HiddenField hdLBAState = ((HiddenField)lbasetupdetailslistview.Items[e.NewEditIndex].FindControl("hidLBAState"));
        //string strLBAState = hdLBAState.Value.ToString();
        //((DropDownList)lbasetupdetailslistview.Items[e.NewEditIndex].FindControl("ddLBAState")).Items.FindByText(strLBAState).Selected = true;
    }

    /// <summary>
    /// Not used till now.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstLBAParmsetupDtls_ItemEditing(object sender, ListViewEditEventArgs e)
    {

    }

    /// <summary>
    /// Not used till now.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstLBAParmsetupDtls_ItemCanceling(object sender, ListViewCancelEventArgs e)
    {

    }

    ///<summary >
    /// Update\Modify LBA information details like state, Factor, Deposit Amount and Comments
    /// Comments up to 500 characters allowed and State and Factor are compulsory fields
    ///</summary>
    /// <param name="e"></param>
    protected void lstLBAParmsetupDtls_ItemUpdate(ListViewItem e)
    {
        try
        {
            //lblcheckvalidations.Visible = false;
            //lblcheckvalidations.Text = "";
            bool StateDetailsExists = false;
            //AdjustmentParameterDetailBE LBAInfoUpdtDetailsBE = new AdjustmentParameterDetailBE();

            Label lblPPSetupDtlsLBA = (Label)e.FindControl("lblAdjParmtdtlLBAID");
            string strLBAPPsetupdtlsID = lblPPSetupDtlsLBA.Text;
            DropDownList ddlLBAdtlsState = (DropDownList)e.FindControl("ddLBAState");
            TextBox txtLBAdtlsFctr = (TextBox)e.FindControl("txtLBAFactor");
            TextBox txtLBAdtlsFnlAmt = (TextBox)e.FindControl("txtLBAfinalAmount");
            TextBox txtLBASetupComments = (TextBox)e.FindControl("LBASetupComments");
            CheckBox chkActind = (CheckBox)e.FindControl("chkActive");


            adjParmDtlBE = AdjPrmtDtlsBS.getAdjParamDtlRow(Convert.ToInt32(strLBAPPsetupdtlsID));
            //Concurrency Issue
            AdjustmentParameterDetailBE adjParmDtlBEold = (AdjParmDtlBElst.Where(o => o.prem_adj_pgm_dtl_id.Equals(Convert.ToInt32(strLBAPPsetupdtlsID)))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmDtlBEold.UPDTE_DATE), Convert.ToDateTime(adjParmDtlBE.UPDTE_DATE));
            if (!con)
                return;
            //End
            decimal? LBAdtlsFnlAmt = null;
            if (txtLBAdtlsFnlAmt.Text != "")
                LBAdtlsFnlAmt = Convert.ToDecimal(txtLBAdtlsFnlAmt.Text);


            if (adjParmDtlBE.st_id != Convert.ToInt32(ddlLBAdtlsState.SelectedValue) ||
                adjParmDtlBE.adj_fctr_rt != Convert.ToDecimal(txtLBAdtlsFctr.Text) ||
                adjParmDtlBE.fnl_overrid_amt != LBAdtlsFnlAmt  ||
                adjParmDtlBE.cmmnt_txt != txtLBASetupComments.Text ||
                adjParmDtlBE.act_ind!=chkActind.Checked)
            {
                if (hdnAdjPrmsetup1txtBox.Text == "0")
                {
                    adjParmDtlBE.adj_paramet_id = Convert.ToInt32(hdnAdjPrmsetup2txtBox.Text);
                }
                else
                {
                    adjParmDtlBE.adj_paramet_id = Convert.ToInt32(hdnAdjPrmsetup1txtBox.Text);
                }
                adjParmDtlBE.PrgmPerodID = int.Parse(ViewState["PRGPRDID"].ToString());
                adjParmDtlBE.AccountID = AISMasterEntities.AccountNumber;
                if (adjParmDtlBE.st_id != Convert.ToInt32(ddlLBAdtlsState.SelectedValue))
                {
                    //Get the AdjParameter LBA details list for specific Account or Customer, Adjustmet Parameter Setupid and Program Period ID
                    IList<AdjustmentParameterDetailBE> AdjParmetDtlComparelstBE = AdjPrmtDtlsBS.getLBAAdjParamtrDtls(adjParmDtlBE.PrgmPerodID, adjParmDtlBE.adj_paramet_id, adjParmDtlBE.AccountID);

                    //check for each item in list if the values exists for the entered current state
                    //IF exists dont save the information
                    foreach (AdjustmentParameterDetailBE AdjCompareLBAdetailBE in AdjParmetDtlComparelstBE)
                    {
                        if (AdjCompareLBAdetailBE.st_id == Convert.ToInt32(ddlLBAdtlsState.SelectedValue))
                        {
                            StateDetailsExists = true;
                            //lblcheckvalidations.Visible = true;
                            //lblcheckvalidations.Text = " * The details for the State you are trying to save already exists";
                            ShowMessage("The details for the State you are trying to save already exists");
                            break;
                        }
                    }
                }

                if (!StateDetailsExists)
                {
                    adjParmDtlBE.st_id = Convert.ToInt32(ddlLBAdtlsState.SelectedValue);
                    adjParmDtlBE.adj_fctr_rt = Convert.ToDecimal(txtLBAdtlsFctr.Text);
                    if (txtLBAdtlsFnlAmt.Text != "")
                    {
                        adjParmDtlBE.fnl_overrid_amt = Convert.ToDecimal(txtLBAdtlsFnlAmt.Text.Replace(",", ""));
                    }
                    else
                    {
                        adjParmDtlBE.fnl_overrid_amt = null;
                    }
                    adjParmDtlBE.cmmnt_txt = txtLBASetupComments.Text;

                    if (chkActind.Checked == false)
                    {
                        adjParmDtlBE.act_ind = false;
                    }
                    else
                    {
                        adjParmDtlBE.act_ind = true;
                    }

                    adjParmDtlBE.UPDTE_USER_ID = CurrentAISUser.PersonID;
                    adjParmDtlBE.UPDTE_DATE = DateTime.Now;

                    bool SaveSuccess = AdjPrmtDtlsBS.Update(adjParmDtlBE);
                    ShowConcurrentConflict(SaveSuccess, adjParmDtlBE.ErrorMessage);
                    if (SaveSuccess)
                    {
                        AdjParameterTransactionWrapper.SubmitTransactionChanges();
                        //Code for logging into Audit Transaction Table 
                        ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                        audtBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
                        audtBS.Save(AISMasterEntities.AccountNumber, adjParmDtlBE.PrgmPerodID, GlobalConstants.AuditingWebPage.LBASetup, CurrentAISUser.PersonID);
                        AdjParameterTransactionWrapper.SubmitTransactionChanges();
                    }
                    else
                    {
                        AdjParameterTransactionWrapper.RollbackChanges();
                    }
                    this.lbasetupdetailslistview.EditIndex = -1;
                    BindLBAniformationDetails(adjParmDtlBE.PrgmPerodID, adjParmDtlBE.adj_paramet_id, AISMasterEntities.AccountNumber);
                }
            }
            else
            {
//                ShowMessage("No information has been changed to Save");
                lbasetupdetailslistview.EditIndex = -1;

                int adjParmsetUpID;
                if (hdnAdjPrmsetup1txtBox.Text == "0")
                {
                    adjParmsetUpID = Convert.ToInt32(hdnAdjPrmsetup2txtBox.Text);
                }
                else
                {
                    adjParmsetUpID = Convert.ToInt32(hdnAdjPrmsetup1txtBox.Text);
                }

                BindLBAniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// This method is used to check if the user is updating or saving list view details and also
    /// calling those update and save methods, Also used to unable and Disable the rows in list view
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstLBAParmsetupDtls_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "Save")
        {
            lstLBARelatedinfo_Saving(e.Item);
        }
        else if (e.CommandName == "Update")
        {
            lstLBAParmsetupDtls_ItemUpdate(e.Item);

        }
        else if (e.CommandName.ToUpper() == "DISABLE")
        {
            DisableLBAParmdtlsRow(e.Item, Convert.ToInt32(e.CommandArgument), false);
        }
        else if (e.CommandName.ToUpper() == "ENABLE")
        {
            DisableLBAParmdtlsRow(e.Item, Convert.ToInt32(e.CommandArgument), true);
        }

    }

    /// <summary>
    /// Not used till now.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstLBAParmsetupDtls_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {

    }

    /// <summary>
    /// Save LBA information details like state, Factor, Deposit Amount and Comments
    /// Comments up to 500 characters allowed and State and Factor are compulsory fields
    /// </summary>
    /// <param name="e"></param>
    protected void lstLBARelatedinfo_Saving(ListViewItem e)
    {
        try
        {
            bool StateDetailsExists = false;
            //lblcheckvalidations.Text = "";
            //lblcheckvalidations.Visible = false;
            AdjustmentParameterDetailBE LBADetailsBE = new AdjustmentParameterDetailBE();
            Adj_paramet_DtlBS LBAdtlsBS = new Adj_paramet_DtlBS();
            DropDownList ddlLBAdtlsState = (DropDownList)e.FindControl("ddLBAState");
            TextBox txtLBAdtlsFctr = (TextBox)e.FindControl("txtLBAFactor");
            TextBox txtLBAdtlsFnlAmt = (TextBox)e.FindControl("txtLBAfinalAmount");
            TextBox txtLBASetupComments = (TextBox)e.FindControl("LBASetupComments");


            LBADetailsBE.AccountID = AISMasterEntities.AccountNumber;
            LBADetailsBE.PrgmPerodID = int.Parse(ViewState["PRGPRDID"].ToString());
            if (hdnAdjPrmsetup1txtBox.Text == "0")
            {
                LBADetailsBE.adj_paramet_id = Convert.ToInt32(hdnAdjPrmsetup2txtBox.Text);
            }
            else
            {
                LBADetailsBE.adj_paramet_id = Convert.ToInt32(hdnAdjPrmsetup1txtBox.Text);
            }
            //Get the AdjParameter LBA details list for specific Account or Customer, Adjustmet Parameter Setupid and Program Period ID
            IList<AdjustmentParameterDetailBE> AdjParmetDtlComparelstBE = AdjParmDtlsBS.getLBAAdjParamtrDtls(LBADetailsBE.PrgmPerodID, LBADetailsBE.adj_paramet_id, LBADetailsBE.AccountID);

            //check for each item in list if the values exists for the entered current state
            //IF exists dont save the information
            foreach (AdjustmentParameterDetailBE AdjCompareLBAdetailBE in AdjParmetDtlComparelstBE)
            {
                if (AdjCompareLBAdetailBE.st_id == Convert.ToInt32(ddlLBAdtlsState.SelectedValue))
                {
                    StateDetailsExists = true;
                    //lblcheckvalidations.Visible = true;
                    //lblcheckvalidations.Text = " * The details for the State you are trying to save already exists";
                    ShowMessage("The details for the State you are trying to save already exists");
                    break;
                }

            }
            if (!StateDetailsExists)
            {
                LBADetailsBE.adj_fctr_rt = Convert.ToDecimal(txtLBAdtlsFctr.Text);
                if (txtLBAdtlsFnlAmt.Text != "")
                {
                    LBADetailsBE.fnl_overrid_amt = Convert.ToDecimal(txtLBAdtlsFnlAmt.Text.Replace(",", ""));
                }
                LBADetailsBE.cmmnt_txt = txtLBASetupComments.Text;
                LBADetailsBE.st_id = Convert.ToInt32(ddlLBAdtlsState.SelectedValue);
                LBADetailsBE.CRTE_DATE = DateTime.Now;
                LBADetailsBE.UPDTE_DATE = (Nullable<DateTime>)null;
                LBADetailsBE.act_ind = true;
                LBADetailsBE.CRTE_USER_ID = CurrentAISUser.PersonID;
                LBADetailsBE.UPDTE_USER_ID = (Nullable<int>)null;
                AdjParmDtlsBS.Update(LBADetailsBE);
                BindLBAniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), LBADetailsBE.adj_paramet_id, AISMasterEntities.AccountNumber);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// Selecting one Program Period ID from the list of the Program periods
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbainfomdatasource_selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {

        e.InputParameters["ProgramPeriodID"] = this.PrmPerdID;
        //BindLBADetails(PrmPerdID); 
    }

    /// <summary>
    /// Close the LBAList view control along with its label and this close button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLBAInfoClose_Click(object sender, EventArgs e)
    {
        this.pnlpp.Enabled = true;
        this.pnlAdlLBA.Enabled = true;
        this.lbasetupdetailslistview.Visible = false;
        this.LBASetupDetailsLabel.Visible = false;
        this.lnkbtnLBADetailsClose.Visible = false;
    }

    /// <summary>
    /// Initially used to check which program period selected and show the details on the screen
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void ProgramPeriod_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        int PrmPerdID = Convert.ToInt32(e.CommandArgument);
        this.lbainfodatasource.SelectParameters[0].DefaultValue = PrmPerdID.ToString();
        BindLBADetails(PrmPerdID);

        //this.lbainfodatasource.SelectParameters[0].DefaultValue = PrmPerdID.ToString(); 

    }

    /// <summary>
    /// The method is used after the LBA details info listview control is bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstLBAParmsetupDtls_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            Label txtFactorLBA = (Label)e.Item.FindControl("lblLBAFactor");
            TextBox textboxedit = (TextBox)e.Item.FindControl("txtLBAFactor");
            DropDownList ddlLBAdtlsState = (DropDownList)e.Item.FindControl("ddLBAState");
            Label hdLBAState = ((Label)e.Item.FindControl("hidLBAState"));

            if ((ddlLBAdtlsState != null) && (hdLBAState != null))
            {
//                ddlLBAdtlsState.SelectedIndex = ddlLBAdtlsState.Items.IndexOf(ddlLBAdtlsState.Items.FindByText(hdLBAState.Text.ToString()));
                AddInActiveLookupDataByText(ref ddlLBAdtlsState, hdLBAState.Text);
            }

            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDisableEnable");

            if (imgDelete != null)
            {
                HiddenField hid = (HiddenField)e.Item.FindControl("hidActive");

                if (hid.Value == "True")
                {
                    if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                    {
                        LinkButton lbLBAEDit = (LinkButton)e.Item.FindControl("lbadetailsetupEdit");
                        lbLBAEDit.Enabled = true;
                    }
                    imgDelete.CommandName = "Disable";
                    imgDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable this record?');");
                }
                else
                {
                    LinkButton lbLBAEDitAlt = (LinkButton)e.Item.FindControl("lbadetailsetupEdit");
                    lbLBAEDitAlt.Enabled = false;
                    imgDelete.CommandName = "Enable";
                    imgDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable this record?');");
                }
            }
            if (txtFactorLBA != null)
            {
                if (txtFactorLBA.Text != "") txtFactorLBA.Text = Convert.ToDecimal(txtFactorLBA.Text).ToString("0.000000");
            }
            if (textboxedit != null)
            {
                if (textboxedit.Text != "") textboxedit.Text = Convert.ToDecimal(textboxedit.Text).ToString("0.000000");
            }
        }
    }

    /// <summary>
    /// Disable\Enable LBA information details for a state
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DisableLBAParmdtlsRow(ListViewItem e, int AdjparmetDtlID, bool Flag)
    {
        try
        {
            //AdjustmentParameterDetailBE  AdjprmDtlBE;
            //Adj_paramet_DtlBS AdjParmdtlBS = new Adj_paramet_DtlBS();
            adjParmDtlBE = AdjPrmtDtlsBS.getAdjParamDtlRow(AdjparmetDtlID);
            //Concurrency Issue
            AdjustmentParameterDetailBE adjParmDtlBEold = (AdjParmDtlBElst.Where(o => o.prem_adj_pgm_dtl_id.Equals(AdjparmetDtlID))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmDtlBEold.UPDTE_DATE), Convert.ToDateTime(adjParmDtlBE.UPDTE_DATE));
            if (!con)
                return;
            //End
            adjParmDtlBE.act_ind = Flag;
            adjParmDtlBE.UPDTE_USER_ID = CurrentAISUser.PersonID;
            adjParmDtlBE.UPDTE_DATE = DateTime.Now;
            Flag = AdjPrmtDtlsBS.Update(adjParmDtlBE);
            if (Flag)
            {
                AdjParameterTransactionWrapper.SubmitTransactionChanges();
                //Code for logging into Audit Transaction Table 
                ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                audtBS.Save(AISMasterEntities.AccountNumber, adjParmDtlBE.PrgmPerodID, GlobalConstants.AuditingWebPage.LBASetup, CurrentAISUser.PersonID);
                AdjParameterTransactionWrapper.SubmitTransactionChanges();
            }
            else
            {
                AdjParameterTransactionWrapper.RollbackChanges();
            }
            int Adjparmsetupiid;
            if (hdnAdjPrmsetup1txtBox.Text == "0")
            {
                Adjparmsetupiid = Convert.ToInt32(hdnAdjPrmsetup2txtBox.Text);
            }
            else
            {
                Adjparmsetupiid = Convert.ToInt32(hdnAdjPrmsetup1txtBox.Text);
            }
            BindLBAniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), Adjparmsetupiid, AISMasterEntities.AccountNumber);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// Disable\Enable Second Row for LBA details table
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DisablesecondRow(object sender, CommandEventArgs e)
    {
        try
        {
            //AdjustmentParameterSetupBE LBAParmBE;
            if (ViewState["ADJLBAnonPRGID"] != null)
            {
                int AdjParmtSetupID = int.Parse(ViewState["ADJLBAnonPRGID"].ToString());
                //LBAParmBE = AdjparamsetupInfo.getAdjParamRow(AdjParmtSetupID);
                adjParmStupBE = AdjPrmStupBS.getAdjParamRow(AdjParmtSetupID);
                //Concurrency Issue
                AdjustmentParameterSetupBE adjParmStupBEold = (Bindlist.Where(o => o.adj_paramet_setup_id.Equals(AdjParmtSetupID))).First();
                bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmStupBEold.UPDATE_DATE), Convert.ToDateTime(adjParmStupBE.UPDATE_DATE));
                if (!con)
                    return;
                //End
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
                //bool ResultLBA = AdjparamsetupInfo.Update(LBAParmBE);
                bool ResultLBA = AdjPrmStupBS.Update(adjParmStupBE);
                if (ResultLBA)
                {
                    AdjParameterTransactionWrapper.SubmitTransactionChanges();
                    //Code for logging into Audit Transaction Table 
                    ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                    audtBS.Save(AISMasterEntities.AccountNumber, adjParmStupBE.prem_adj_pgm_id, GlobalConstants.AuditingWebPage.LBASetup, CurrentAISUser.PersonID);
                    AdjParameterTransactionWrapper.SubmitTransactionChanges();
                }
                else
                {
                    AdjParameterTransactionWrapper.RollbackChanges();
                }
                BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// Disable\Enable First Row for LBA details table
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DisablefirstRow(object sender, CommandEventArgs e)
    {
        try
        {
            //AdjustmentParameterSetupBE  LBAParmBE;
            if (ViewState["ADJLBAPRGID"] != null)
            {
                int AdjParmtSetupID = int.Parse(ViewState["ADJLBAPRGID"].ToString());
                //LBAParmBE = AdjparamsetupInfo.getAdjParamRow(AdjParmtSetupID);
                adjParmStupBE = AdjPrmStupBS.getAdjParamRow(AdjParmtSetupID);
                //Concurrency Issue
                AdjustmentParameterSetupBE adjParmStupBEold = (Bindlist.Where(o => o.adj_paramet_setup_id.Equals(AdjParmtSetupID))).First();
                bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmStupBEold.UPDATE_DATE), Convert.ToDateTime(adjParmStupBE.UPDATE_DATE));
                if (!con)
                    return;
                //End
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
                bool ResultLBA = AdjPrmStupBS.Update(adjParmStupBE);
                if (ResultLBA)
                {
                    AdjParameterTransactionWrapper.SubmitTransactionChanges();
                    //Code for logging into Audit Transaction Table 
                    ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                    audtBS.Save(AISMasterEntities.AccountNumber, adjParmStupBE.prem_adj_pgm_id, GlobalConstants.AuditingWebPage.LBASetup, CurrentAISUser.PersonID);
                    AdjParameterTransactionWrapper.SubmitTransactionChanges();
                }
                else
                {
                    AdjParameterTransactionWrapper.RollbackChanges();
                }
                BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// Invokes when user clicks on LBASetup, LCF Setup, Tax multiplier,
    /// RML Setup, CHF Setups Cancel Link and cancels the details newly entered
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LBASetupCancel_Click(object sender, EventArgs e)
    {
        BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
    }
    #region Maintaining Session for LossinfoBE
    private IList<AdjustmentParameterSetupBE> Bindlist
    {
        get
        {
            if (Session["Bindlist"] == null)
                Session["Bindlist"] = new List<AdjustmentParameterSetupBE>();
            return (IList<AdjustmentParameterSetupBE>)Session["Bindlist"];
        }
        set { Session["Bindlist"] = value; }
    }

   
    #endregion
    /// <summary>
    /// Bind All Tables for LBA, LCF, CHF, Tax Multiplier, RML with data from database
    /// LBA and CHF has two rows(Initial loading the data in datatables on screen for each tab) 
    /// </summary>
    /// <param name="prgprdID"></param>
    private void BindLBADetails(int prgprdID)
    {
        try
        {
            //LBA Tab panel Panel Enable\Disable
            this.pnlAdlLBA.Enabled = true;
            this.lbasetupdetailslistview.Visible = false;
            this.LBASetupDetailsLabel.Visible = false;
            this.lnkbtnLBADetailsClose.Visible = false;
            //this.pnlpp.Enabled = true;

            //CHF Tab panel Panel Enable\Disable
            this.pnlADlCHF.Enabled = true;
            this.chfsetupdetailslistview.Visible = false;
            this.CHFSetupDetailsLabel.Visible = false;
            this.lnkbtnCHFDetailsClose.Visible = false;
            //this.pnlppCHF.Enabled = true;

            //this.lblcheckvalidations.Text = "";
            //this.lblcheckvalidations.Visible = false;

            //LCF Tab panel Enable\Disable
            this.pnlAdjLCF.Enabled = true;
            this.lstLCFSetuplistView.Visible = false;
            this.lblLCFdtls.Visible = false;
            this.lnkLCFClose.Visible = false;
            //this.pnlppLCF.Enabled = true;

            //WA-TAX Tab panel Enable\Disable
            this.pnlAdjTM.Enabled = true;
            this.lstTMSetuplistView.Visible = false;
            this.lblTMdtls.Visible = false;
            this.lnkBtnCloseTMdtlsID.Visible = false;
            //this.pnlppTM.Enabled = true;

            //RML Tab panel Enable\Disable
            this.pnlAdjRML.Enabled = true;
            this.lstRMLSetuplistView.Visible = false;
            this.lblRMLdtls.Visible = false;
            this.lnkBtnCloseRMLdtlsID.Visible = false;
            //this.pnlppRML.Enabled = true;

            CheckNewNotincLBA = true;
            CheckNewincLBA = true;
            CheckNewLCF = true;
            CheckNewTM = true;
            CheckNewRML = true;
            CheckNewincCHF = true;
            CheckNewNotincCHF = true;

            LdfIbnrInclCheckBox.Checked = false;
            LdfIbnrInclCheckBox2.Checked = false;

            ViewState["AdjparmsetupforLCF"] = null;
            ViewState["ADJLBAPRGID"] = null;
            ViewState["ADJLBAnonPRGID"] = null;
            ViewState["AdjparmsetupforTM"] = null;
            ViewState["AdjparmsetupforRML"] = null;
            ViewState["ADJCHFPRGID"] = null;
            ViewState["ADJCHFnonPRGID"] = null;

            ViewState["PRGPRDID"] = prgprdID;
            int CustomerID = AISMasterEntities.AccountNumber;

            Adj_Parameter_SetupBS LBAParamBS = new Adj_Parameter_SetupBS();
            //BLAccess Lbablaccess = new BLAccess();
            // AdjustmentParameterSetupBE AdjParamSetUpInfo = LBAParamBS.getAdjParamRow(prgprdID);
            IList<PolicyBE> PlcyLst = Lbablaccess.ListviewGetPolicyDataforCust(prgprdID, CustomerID);
            IList<PolicyBE> PlcyLstLBA = Lbablaccess.ListviewGetPolicyDataforCustLBA(prgprdID, CustomerID);
            if (PlcyLst.Count > 0)
            {
                if (this.Updatepanellba.Visible == false)
                {
                    this.Updatepanellba.Visible = true;
                }

                this.UpdatepanelCHF.Visible = true;
                this.UpdatepanelLCF.Visible = true;
                this.UpdatepanelTM.Visible = true;
                this.updtpnlRML.Visible = true;
                this.UpdatepanelCHF.Visible = true;
                this.updtpnlRML.Visible = true;

                this.pnlAdlLBA.Visible = true;
                this.pnlAdjLCF.Visible = true;
                this.pnlAdjTM.Visible = true;
                this.pnlAdjRML.Visible = true;
                this.pnlADlCHF.Visible = true;

                this.IncludedInERPButton.Enabled = false;
                this.NotIncludedInERPbtn.Enabled = false;
                this.rbtnCHFinclude.Enabled = false;
                this.rbtnCHFNotinclude.Enabled = false;
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                {
                    this.IncludedInERPButton.Checked = true;
                    this.rbtnCHFinclude.Checked = true;

                    this.lnkBtnLBAadj.Enabled = true;
                    this.PolicyNumLstBox.Enabled = true;
                    this.LBAAdjTypeDdl.Enabled = true;
                    this.txtlbaintdeposit.Enabled = true;
                    this.LdfIbnrInclCheckBox.Enabled = true;
                    this.lnkBtnLBASetupCancel.Enabled = true;

                    this.LinkButtonSave1.Enabled = true;
                    this.PolicyNumLstBox2.Enabled = true;
                    this.LBAAdjTypeDdl2.Enabled = true;
                    this.txtlbaintdeposit2.Enabled = true;
                    this.LdfIbnrInclCheckBox2.Enabled = true;
                    this.lnkBtnLBASetup2Cancel.Enabled = true;


                    this.lnkbtnSaveLCH.Enabled = true;
                    this.ckkboxlstPolicyno.Enabled = true;
                    this.txtLCFClmCAP.Enabled = true;
                    this.txtLCFAggtAmt.Enabled = true;
                    this.txtLayLCFIPay.Enabled = true;
                    this.txtLayLCFZurchPay.Enabled = true;
                    this.lnkBtnLCFCncl.Enabled = true;

                    this.lnkBtnTMSave.Enabled = true;
                    this.ChkBoxLstTMpolicy.Enabled = true;
                    this.lnkBtnTMCancel.Enabled = true;

                    this.lnkbtnSAVERML.Enabled = true;
                    this.ChkBoxLstRMLpolicy.Enabled = true;
                    this.lnkBtnRMLSetupCancel.Enabled = true;

                    this.lnkBtnCHFadj.Enabled = true;
                    this.chkbxCHFPolicynolst.Enabled = true;
                    this.ddCHFBasisCharged.Enabled = true;
                    this.txtCHFDeposit.Enabled = true;
                    this.lnkBtnCHFCancel.Enabled = true;

                    this.lnkBtn2CHFadj.Enabled = true;
                    this.chkbxCHF2Policynolst.Enabled = true;
                    this.ddCHFBasisCharged2.Enabled = true;
                    this.txtchfDeposit2.Enabled = true;
                    this.lnkBtnCHF2Cancel.Enabled = true;
                }
                this.LinkButton2.Enabled = false;
                this.LinkButtonDetail.Enabled = false;
                this.lnkBtnLCFdtls.Enabled = false;
                this.LnkbtnTMdtls.Enabled = false;
                this.lnkbtnRMLdtlsView.Enabled = false;
                this.lnkbtnCHFinfoDetails.Enabled = false;
                this.lnkbtnCHF2infoDetails.Enabled = false;

                //imgDisablerow.ImageUrl = "~/images/disabled.GIF";
                //imgDisable2row.ImageUrl = "~/images/disabled.GIF";
                //imgbtnDisableLCF.ImageUrl = "~/images/disabled.GIF";
                //imgbtnDisableTM.ImageUrl = "~/images/disabled.GIF";
                //imgbtnDisableRML.ImageUrl = "~/images/disabled.GIF";
                //imgbtnCHFdisable.ImageUrl = "~/images/disabled.GIF"; ;
                //imgbtn2CHFdisable.ImageUrl = "~/images/disabled.GIF"; ;

                imgDisablerow.Visible = false;
                imgDisable2row.Visible = false;
                imgbtnDisableLCF.Visible = false;
                imgbtnDisableTM.Visible = false;
                imgbtnDisableRML.Visible = false;
                imgbtnCHFdisable.Visible = false;
                imgbtn2CHFdisable.Visible = false;
                if (PlcyLstLBA != null)
                {
                    if (PlcyLstLBA.Count > 0)
                    {
                        PolicyNumLstBox.DataSource = PlcyLstLBA;
                        PolicyNumLstBox.DataTextField = "PolicyPerfectNumber";
                        PolicyNumLstBox.DataValueField = "PolicyID";
                        PolicyNumLstBox.DataBind();
                        pnlAdjLBAEmptyData.Visible = false;
                    }
                    else
                    {
                        pnlAdjLBAEmptyData.Visible = true;
                        this.pnlAdlLBA.Visible = false;
                    }
                }
                else
                {
                    pnlAdjLBAEmptyData.Visible = true;
                    this.pnlAdlLBA.Visible = false;
                }

                if (PlcyLstLBA != null)
                {
                    if (PlcyLstLBA.Count == 1)
                    {
                        pnlPolicyNumberListLBA.Height = Unit.Point(20);
                    }
                    else
                    {
                        pnlPolicyNumberListLBA.Height = Unit.Point(37);
                    }
                }
                else
                {
                    pnlPolicyNumberListLBA.Height = Unit.Point(37);
                }

                if (PlcyLstLBA != null)
                {
                    if (PlcyLstLBA.Count > 0)
                    {
                        PolicyNumLstBox2.DataSource = PlcyLstLBA;
                        PolicyNumLstBox2.DataTextField = "PolicyPerfectNumber";
                        PolicyNumLstBox2.DataValueField = "PolicyID";
                        PolicyNumLstBox2.DataBind();
                    }
                    else
                    {
                        this.pnlAdlLBA.Visible = false;
                    }
                }
                else
                {
                    this.pnlAdlLBA.Visible = false;
                }

                if (PlcyLstLBA != null)
                {
                    if (PlcyLstLBA.Count == 1)
                    {
                        pnlPolicyNumber2ListLBA.Height = Unit.Point(20);
                    }
                    else
                    {
                        pnlPolicyNumber2ListLBA.Height = Unit.Point(37);
                    }
                }
                else
                {
                    pnlPolicyNumber2ListLBA.Height = Unit.Point(37);
                }

                ckkboxlstPolicyno.DataSource = PlcyLst;
                ckkboxlstPolicyno.DataTextField = "PolicyPerfectNumber";
                ckkboxlstPolicyno.DataValueField = "PolicyID";
                ckkboxlstPolicyno.DataBind();
                if (PlcyLst.Count > 2) Panel1.Height = Unit.Point(50);
                if (PlcyLst.Count == 1) Panel1.Height = Unit.Point(20);
                if (PlcyLst.Count == 2) Panel1.Height = Unit.Point(37);

                ChkBoxLstTMpolicy.DataSource = PlcyLst;
                ChkBoxLstTMpolicy.DataTextField = "PolicyPerfectNumber";
                ChkBoxLstTMpolicy.DataValueField = "PolicyID";
                ChkBoxLstTMpolicy.DataBind();
                if (PlcyLst.Count > 2) ChkPolicyPanelTM.Height = Unit.Point(50);
                if (PlcyLst.Count == 1) ChkPolicyPanelTM.Height = Unit.Point(20);
                if (PlcyLst.Count == 2) ChkPolicyPanelTM.Height = Unit.Point(37);

                ChkBoxLstRMLpolicy.DataSource = PlcyLst;
                ChkBoxLstRMLpolicy.DataTextField = "PolicyPerfectNumber";
                ChkBoxLstRMLpolicy.DataValueField = "PolicyID";
                ChkBoxLstRMLpolicy.DataBind();

                if (PlcyLst.Count > 2) Panel3.Height = Unit.Point(50);
                if (PlcyLst.Count == 1) Panel3.Height = Unit.Point(20);
                if (PlcyLst.Count == 2) Panel3.Height = Unit.Point(37);

                chkbxCHFPolicynolst.DataSource = PlcyLst;
                chkbxCHFPolicynolst.DataTextField = "PolicyPerfectNumber";
                chkbxCHFPolicynolst.DataValueField = "PolicyID";
                chkbxCHFPolicynolst.DataBind();
                if (PlcyLst.Count == 1)
                {
                    PnlCHFpolicynumberlist.Height = Unit.Point(20);
                }
                else
                {
                    PnlCHFpolicynumberlist.Height = Unit.Point(37);
                }

                chkbxCHF2Policynolst.DataSource = PlcyLst;
                chkbxCHF2Policynolst.DataTextField = "PolicyPerfectNumber";
                chkbxCHF2Policynolst.DataValueField = "PolicyID";
                chkbxCHF2Policynolst.DataBind();
                if (PlcyLst.Count == 1)
                {
                    PnlCHF2policynumberlist.Height = Unit.Point(20);
                }
                else
                {
                    PnlCHF2policynumberlist.Height = Unit.Point(37);
                }
                //if (PlcyLst.Count > 3) PnlCHF2policynumberlist.Height = Unit.Point(50);
                //if (PlcyLst.Count == 1) PnlCHF2policynumberlist.Height = Unit.Point(20);
                //if (PlcyLst.Count == 2) PnlCHF2policynumberlist.Height = Unit.Point(37);

                PopulateDropDownList(GlobalConstants.LookUpType.LBA_ADJUSTMENT_TYPE, ref LBAAdjTypeDdl);
                PopulateDropDownList(GlobalConstants.LookUpType.LBA_ADJUSTMENT_TYPE, ref LBAAdjTypeDdl2);
                PopulateDropDownList(GlobalConstants.LookUpType.CHF_BASIS, ref ddCHFBasisCharged);
                PopulateDropDownList(GlobalConstants.LookUpType.CHF_BASIS, ref ddCHFBasisCharged2);

                txtlbaintdeposit.Text = "0";
                txtlbaintdeposit2.Text = "0";
                txtCHFDeposit.Text = "0";
                txtchfDeposit2.Text = "0";
                txtLCFClmCAP.Text = "0";
                txtLCFAggtAmt.Text = "0";
                txtLayLCFIPay.Text = "0";
                txtLayLCFZurchPay.Text = "0";

                //IList<AdjustmentParameterSetupBE> 
                Bindlist = LBAParamBS.getAdjParamtr(prgprdID, CustomerID);
                //Upade Button CHF
                lnkBtn2CHFadjUpd.Visible = false;
                lnkBtn2CHFadj.Visible = true;
                lnkBtnCHFadjUpd.Visible = false;
                lnkBtnCHFadj.Visible = true;
                //Upade Button LBA
                lnkBtnLBAadjUpd.Visible = false;
                lnkBtnLBAadj.Visible = true;
                //Upade Button LBA
                LinkButtonUpd1.Visible = false;
                LinkButtonSave1.Visible = true;
                //Upade Button LCF
                lnkbtnSaveLCH.Visible = true;
                lnkbtnUpdLCH.Visible = false;
                //Update Button TM
                lnkBtnTMSave.Visible = true;
                lnkBtnTMUpd.Visible = false;
                //Update Button RML
                lnkbtnUPDRML.Visible = false;
                lnkbtnSAVERML.Visible = true;
                if (Bindlist != null)
                {
                    foreach (AdjustmentParameterSetupBE LBAParamLBABE in Bindlist)
                    {
                        if (LBAParamLBABE.AdjparameterTypeID == Lbablaccess.GetLookUpID("LBA", "ADJUSTMENT PARAMETER TYPE"))
                        {

                            if (LBAParamLBABE.incld_ernd_retro_prem_ind == true)
                            {
                                //Upade Button LBA
                                lnkBtnLBAadjUpd.Visible = true;
                                lnkBtnLBAadj.Visible = false;
                                imgDisablerow.Visible = true;
                                //lnkBtnLBAadj.CommandArgument = LBAParamLBABE.adj_paramet_setup_id.ToString();
                                ViewState["ADJLBAPRGID"] = LBAParamLBABE.adj_paramet_setup_id;
                                IncludedInERPButton.Checked = true;

                               
                                if(LBAParamLBABE.depst_amt !=null)
                                txtlbaintdeposit.Text = decimal.Parse(LBAParamLBABE.depst_amt.ToString()).ToString("#,##0");

                                if (LBAParamLBABE.incld_ibnr_ldf_ind.Value == true)
                                {
                                    LdfIbnrInclCheckBox.Checked = true;
                                }
                                else
                                {
                                    LdfIbnrInclCheckBox.Checked = false;
                                }
                                if (LBAParamLBABE.actv_ind.Value == true)
                                {
                                    imgDisablerow.ImageUrl = "~/images/disabled.GIF";
                                    imgDisablerow.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable this record?');");
                                    if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                                    {
                                        lnkBtnLBAadj.Enabled = true;
                                        //Upade Button LBA
                                        lnkBtnLBAadjUpd.Enabled = true;
                                        PolicyNumLstBox.Enabled = true;
                                        LBAAdjTypeDdl.Enabled = true;
                                        txtlbaintdeposit.Enabled = true;
                                        LdfIbnrInclCheckBox.Enabled = true;

                                        lnkBtnLBASetupCancel.Enabled = true;
                                    }
                                    LinkButton2.Enabled = true;
                                }
                                else
                                {
                                    imgDisablerow.ImageUrl = "~/images/enabled.GIF";
                                    imgDisablerow.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable this record?');");
                                    lnkBtnLBAadj.Enabled = false;
                                    //Upade Button LBA
                                    lnkBtnLBAadjUpd.Enabled = false;
                                    PolicyNumLstBox.Enabled = false;
                                    LBAAdjTypeDdl.Enabled = false;
                                    txtlbaintdeposit.Enabled = false;
                                    LdfIbnrInclCheckBox.Enabled = false;
                                    LinkButton2.Enabled = false;
                                    lnkBtnLBASetupCancel.Enabled = false;
                                }
                                LBAAdjTypeDdl.DataBind();

                                AddInActiveLookupData(ref LBAAdjTypeDdl, LBAParamLBABE.lba_Adjustment_typ.Value);

                                //Binding selected policy from Database to ListCheckBox
                                IList<AdjustmentParameterPolicyBE> AdjParmBE = LBAParamLBABE.AdjParametPolBEs;
                                foreach (AdjustmentParameterPolicyBE LBAPolBEdetails in AdjParmBE)
                                {
                                    AddInActiveLookupData(ref PolicyNumLstBox, LBAPolBEdetails.coml_agmt_id);
                                }
                                int polCount = 0;
                                for (int i = 0; i < PolicyNumLstBox.Items.Count; i++)
                                {
                                    if (PolicyNumLstBox.Items[i].Selected)
                                    { ++polCount; }
                                }
                                if (polCount == 0) LinkButton2.Enabled = false;

                                CheckNewincLBA = false;
                            }
                            else
                            { //Upade Button LBA
                                LinkButtonUpd1.Visible = true;
                                LinkButtonSave1.Visible = false;
                                imgDisable2row.Visible = true;
                                ViewState["ADJLBAnonPRGID"] = LBAParamLBABE.adj_paramet_setup_id;
                                NotIncludedInERPbtn.Checked = false;
                               
                                if(LBAParamLBABE.depst_amt !=null)
                                txtlbaintdeposit2.Text = decimal.Parse(LBAParamLBABE.depst_amt.ToString()).ToString("#,##0");
                                if (LBAParamLBABE.incld_ibnr_ldf_ind.Value == true)
                                {
                                    LdfIbnrInclCheckBox2.Checked = true;
                                }
                                else
                                {
                                    LdfIbnrInclCheckBox2.Checked = false;
                                }
                                if (LBAParamLBABE.actv_ind.Value == true)
                                {
                                    imgDisable2row.ImageUrl = "~/images/disabled.GIF";
                                    imgDisable2row.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable this record?');");
                                    if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                                    {
                                        LinkButtonSave1.Enabled = true;
                                        LinkButtonUpd1.Enabled = true;
                                        PolicyNumLstBox2.Enabled = true;
                                        LBAAdjTypeDdl2.Enabled = true;
                                        txtlbaintdeposit2.Enabled = true;
                                        LdfIbnrInclCheckBox2.Enabled = true;

                                        lnkBtnLBASetup2Cancel.Enabled = true;
                                    }
                                    LinkButtonDetail.Enabled = true;
                                }
                                else
                                {
                                    imgDisable2row.ImageUrl = "~/images/enabled.GIF";
                                    imgDisable2row.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable this record?');");
                                    LinkButtonSave1.Enabled = false;
                                    LinkButtonUpd1.Enabled = false;
                                    PolicyNumLstBox2.Enabled = false;
                                    LBAAdjTypeDdl2.Enabled = false;
                                    txtlbaintdeposit2.Enabled = false;
                                    LdfIbnrInclCheckBox2.Enabled = false;
                                    LinkButtonDetail.Enabled = false;
                                    lnkBtnLBASetup2Cancel.Enabled = false;
                                }
                                LBAAdjTypeDdl2.DataBind();

                                AddInActiveLookupData(ref LBAAdjTypeDdl2, LBAParamLBABE.lba_Adjustment_typ.Value);

                                IList<AdjustmentParameterPolicyBE> AdjParmBE = LBAParamLBABE.AdjParametPolBEs;

                                foreach (AdjustmentParameterPolicyBE LBAPolnBEdetails in AdjParmBE)
                                {
                                    AddInActiveLookupData(ref PolicyNumLstBox2, LBAPolnBEdetails.coml_agmt_id);
                                }
                                int polCount2 = 0;
                                for (int i = 0; i < PolicyNumLstBox2.Items.Count; i++)
                                {
                                    if (PolicyNumLstBox2.Items[i].Selected)
                                    { ++polCount2; }
                                }
                                if (polCount2 == 0) LinkButtonDetail.Enabled = false;
                                CheckNewNotincLBA = false;

                            }
                        }
                        else if (LBAParamLBABE.AdjparameterTypeID == Lbablaccess.GetLookUpID("LCF", "ADJUSTMENT PARAMETER TYPE"))
                        {
                            //Upade Button LCF
                            lnkbtnSaveLCH.Visible = false;
                            lnkbtnUpdLCH.Visible = true;
                            imgbtnDisableLCF.Visible = true;
                            ViewState["AdjparmsetupforLCF"] = LBAParamLBABE.adj_paramet_setup_id;
                           if(LBAParamLBABE.loss_convfact_calimcap !=null)
                            txtLCFClmCAP.Text = decimal.Parse(LBAParamLBABE.loss_convfact_calimcap.ToString()).ToString("#,##0");
                            if(LBAParamLBABE.loss_convfact_aggamt!=null)
                            txtLCFAggtAmt.Text = decimal.Parse(LBAParamLBABE.loss_convfact_aggamt.ToString()).ToString("#,##0");
                            if(LBAParamLBABE.lay_lossconv_FactInsPay !=null)
                            txtLayLCFIPay.Text = decimal.Parse(LBAParamLBABE.lay_lossconv_FactInsPay.ToString()).ToString("#,##0");
                           if(LBAParamLBABE.lay_lossconv_znapayamt!=null)
                            txtLayLCFZurchPay.Text = decimal.Parse(LBAParamLBABE.lay_lossconv_znapayamt.ToString()).ToString("#,##0");
                            if (LBAParamLBABE.actv_ind.Value == true)
                            {
                                imgbtnDisableLCF.ImageUrl = "~/images/disabled.GIF";
                                imgbtnDisableLCF.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable this record?');");
                                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                                {

                                    //Upade Button LCF
                                    lnkbtnUpdLCH.Enabled = true;
                                    lnkbtnSaveLCH.Enabled = true;
                                    ckkboxlstPolicyno.Enabled = true;
                                    txtLCFClmCAP.Enabled = true;
                                    txtLCFAggtAmt.Enabled = true;
                                    txtLayLCFIPay.Enabled = true;
                                    txtLayLCFZurchPay.Enabled = true;
                                    lnkBtnLCFCncl.Enabled = true;
                                }
                                lnkBtnLCFdtls.Enabled = true;
                            }
                            else
                            {
                                imgbtnDisableLCF.ImageUrl = "~/images/enabled.GIF";
                                imgbtnDisableLCF.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable this record?');");
                                lnkBtnLCFdtls.Enabled = false;
                                //Upade Button LCF
                                lnkbtnUpdLCH.Enabled = false;
                                lnkbtnSaveLCH.Enabled = false;
                                ckkboxlstPolicyno.Enabled = false;
                                txtLCFClmCAP.Enabled = false;
                                txtLCFAggtAmt.Enabled = false;
                                txtLayLCFIPay.Enabled = false;
                                txtLayLCFZurchPay.Enabled = false;
                                lnkBtnLCFCncl.Enabled = false;
                            }

                            IList<AdjustmentParameterPolicyBE> AdjParmBE = LBAParamLBABE.AdjParametPolBEs;
                            foreach (AdjustmentParameterPolicyBE LCFPolBEdetails in AdjParmBE)
                            {
//                                ckkboxlstPolicyno.Items.FindByValue(LCFPolBEdetails.coml_agmt_id.ToString()).Selected = true;
                                AddInActiveLookupData(ref ckkboxlstPolicyno, LCFPolBEdetails.coml_agmt_id);
                            }
                            CheckNewLCF = false;
                        }
                        else if (LBAParamLBABE.AdjparameterTypeID == Lbablaccess.GetLookUpID("WA", "ADJUSTMENT PARAMETER TYPE"))
                        {
                            //Update Button TM
                            lnkBtnTMSave.Visible = false;
                            lnkBtnTMUpd.Visible = true;
                            imgbtnDisableTM.Visible = true;
                            ViewState["AdjparmsetupforTM"] = LBAParamLBABE.adj_paramet_setup_id;
                            if (LBAParamLBABE.actv_ind.Value == true)
                            {
                                imgbtnDisableTM.ImageUrl = "~/images/disabled.GIF";
                                imgbtnDisableTM.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable this record?');");
                                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                                {

                                    //Update Button TM
                                    lnkBtnTMUpd.Enabled = true;
                                    lnkBtnTMSave.Enabled = true;
                                    ChkBoxLstTMpolicy.Enabled = true;
                                    lnkBtnTMCancel.Enabled = true;
                                }
                                LnkbtnTMdtls.Enabled = true;
                            }
                            else
                            {
                                imgbtnDisableTM.ImageUrl = "~/images/enabled.GIF";
                                imgbtnDisableTM.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable this record?');");
                                LnkbtnTMdtls.Enabled = false;
                                //Update Button TM
                                lnkBtnTMUpd.Enabled = false;
                                lnkBtnTMSave.Enabled = false;
                                ChkBoxLstTMpolicy.Enabled = false;
                                lnkBtnTMCancel.Enabled = false;
                            }
                            IList<AdjustmentParameterPolicyBE> AdjParmBE = LBAParamLBABE.AdjParametPolBEs;
                            foreach (AdjustmentParameterPolicyBE TMPolBEdetails in AdjParmBE)
                            {
//                                ChkBoxLstTMpolicy.Items.FindByValue(TMPolBEdetails.coml_agmt_id.ToString()).Selected = true;
                                AddInActiveLookupData(ref ChkBoxLstTMpolicy, TMPolBEdetails.coml_agmt_id);
                            }
                            CheckNewTM = false;
                        }
                        else if (LBAParamLBABE.AdjparameterTypeID == Lbablaccess.GetLookUpID("RML", "ADJUSTMENT PARAMETER TYPE"))
                        {
                            //Update Button RML
                            lnkbtnUPDRML.Visible = true;
                            lnkbtnSAVERML.Visible = false;
                            imgbtnDisableRML.Visible = true;
                            ViewState["AdjparmsetupforRML"] = LBAParamLBABE.adj_paramet_setup_id;
                            if (LBAParamLBABE.actv_ind.Value == true)
                            {
                                imgbtnDisableRML.ImageUrl = "~/images/disabled.GIF";
                                imgbtnDisableRML.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable this record?');");
                                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                                {

                                    lnkbtnSAVERML.Enabled = true;
                                    //Update Button RML
                                    lnkbtnUPDRML.Enabled = true;
                                    ChkBoxLstRMLpolicy.Enabled = true;
                                    lnkBtnRMLSetupCancel.Enabled = true;
                                }
                                lnkbtnRMLdtlsView.Enabled = true;
                            }
                            else
                            {
                                imgbtnDisableRML.ImageUrl = "~/images/enabled.GIF";
                                imgbtnDisableRML.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable this record?');");
                                lnkbtnRMLdtlsView.Enabled = false;
                                lnkbtnSAVERML.Enabled = false;
                                //Update Button RML
                                lnkbtnUPDRML.Enabled = false;
                                ChkBoxLstRMLpolicy.Enabled = false;
                                lnkBtnRMLSetupCancel.Enabled = false;
                            }
                            IList<AdjustmentParameterPolicyBE> AdjParmBE = LBAParamLBABE.AdjParametPolBEs;
                            foreach (AdjustmentParameterPolicyBE TMPolBEdetails in AdjParmBE)
                            {
//                                ChkBoxLstRMLpolicy.Items.FindByValue(TMPolBEdetails.coml_agmt_id.ToString()).Selected = true;
                                AddInActiveLookupData(ref ChkBoxLstRMLpolicy, TMPolBEdetails.coml_agmt_id);
                            }
                            CheckNewRML = false;
                        }
                        else if (LBAParamLBABE.AdjparameterTypeID == Lbablaccess.GetLookUpID("CHF", "ADJUSTMENT PARAMETER TYPE"))
                        {

                            if (LBAParamLBABE.incld_ernd_retro_prem_ind == true)
                            { //Upade Button
                                lnkBtnCHFadjUpd.Visible = true;
                                lnkBtnCHFadj.Visible = false;
                                imgbtnCHFdisable.Visible = true;
                                ViewState["ADJCHFPRGID"] = LBAParamLBABE.adj_paramet_setup_id;
                                rbtnCHFinclude.Checked = true;
                               if(LBAParamLBABE.depst_amt !=null)
                                txtCHFDeposit.Text = decimal.Parse(LBAParamLBABE.depst_amt.ToString()).ToString("#,##0");
                                if (LBAParamLBABE.actv_ind.Value == true)
                                {
                                    imgbtnCHFdisable.ImageUrl = "~/images/disabled.GIF";
                                    imgbtnCHFdisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable this record?');");
                                    if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                                    {
                                        lnkBtnCHFadj.Enabled = true;
                                        //Update Button
                                        lnkBtnCHFadjUpd.Enabled = true;
                                        chkbxCHFPolicynolst.Enabled = true;
                                        ddCHFBasisCharged.Enabled = true;
                                        txtCHFDeposit.Enabled = true;

                                        // imgbtnCHFdisable.Enabled = true;
                                        lnkBtnCHFCancel.Enabled = true;
                                    }
                                    lnkbtnCHFinfoDetails.Enabled = true;
                                }
                                else
                                {
                                    imgbtnCHFdisable.ImageUrl = "~/images/enabled.GIF";
                                    imgbtnCHFdisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable this record?');");
                                    lnkBtnCHFadj.Enabled = false;
                                    //Update Button
                                    lnkBtnCHFadjUpd.Enabled = false;
                                    chkbxCHFPolicynolst.Enabled = false;
                                    ddCHFBasisCharged.Enabled = false;
                                    txtCHFDeposit.Enabled = false;
                                    lnkbtnCHFinfoDetails.Enabled = false;
                                    // imgbtnCHFdisable.Enabled = false;
                                    lnkBtnCHFCancel.Enabled = false;
                                }
                                ddCHFBasisCharged.DataBind();

//                                ddCHFBasisCharged.Items.FindByValue(LBAParamLBABE.clm_hndl_fee_basis_id.ToString()).Selected = true;
                                AddInActiveLookupData(ref ddCHFBasisCharged, LBAParamLBABE.clm_hndl_fee_basis_id.Value);

                                //Binding selected policy from Database to LISTboxCheck
                                IList<AdjustmentParameterPolicyBE> AdjParmBE = LBAParamLBABE.AdjParametPolBEs;
                                foreach (AdjustmentParameterPolicyBE LBAPolBEdetails in AdjParmBE)
                                {
//                                    chkbxCHFPolicynolst.Items.FindByValue(LBAPolBEdetails.coml_agmt_id.ToString()).Selected = true;
                                    AddInActiveLookupData(ref chkbxCHFPolicynolst, LBAPolBEdetails.coml_agmt_id);
                                }
                                //int polCount3 = 0;
                                //for (int i = 0; i < chkbxCHFPolicynolst.Items.Count; i++)
                                //{
                                //    if (chkbxCHFPolicynolst.Items[i].Selected)
                                //    { ++polCount3; }
                                //}
                                //if (polCount3 == 0) lnkbtnCHFinfoDetails.Enabled = false;                                 
                                CheckNewincCHF = false;
                            }
                            else
                            {
                                //Upade Button
                                lnkBtn2CHFadjUpd.Visible = true;
                                lnkBtn2CHFadj.Visible = false;
                                imgbtn2CHFdisable.Visible = true;
                                ViewState["ADJCHFnonPRGID"] = LBAParamLBABE.adj_paramet_setup_id;
                                rbtnCHFNotinclude.Checked = false;
                               if(LBAParamLBABE.depst_amt !=null)
                                   txtchfDeposit2.Text = decimal.Parse(LBAParamLBABE.depst_amt.ToString()).ToString("#,##0");
                                if (LBAParamLBABE.actv_ind.Value == true)
                                {
                                    imgbtn2CHFdisable.ImageUrl = "~/images/disabled.GIF";
                                    imgbtn2CHFdisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable this record?');");
                                    if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                                    {
                                        lnkBtn2CHFadj.Enabled = true;
                                        //Update Button
                                        lnkBtn2CHFadjUpd.Enabled = true;
                                        chkbxCHF2Policynolst.Enabled = true;
                                        ddCHFBasisCharged2.Enabled = true;
                                        txtchfDeposit2.Enabled = true;

                                        //imgbtn2CHFdisable.Enabled = true;
                                        lnkBtnCHF2Cancel.Enabled = true;
                                    }
                                    lnkbtnCHF2infoDetails.Enabled = true;
                                }
                                else
                                {
                                    imgbtn2CHFdisable.ImageUrl = "~/images/enabled.GIF";
                                    imgbtn2CHFdisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable this record?');");
                                    lnkBtn2CHFadj.Enabled = false;
                                    //Update Button
                                    lnkBtn2CHFadjUpd.Enabled = false;
                                    chkbxCHF2Policynolst.Enabled = false;
                                    ddCHFBasisCharged2.Enabled = false;
                                    txtchfDeposit2.Enabled = false;
                                    lnkbtnCHF2infoDetails.Enabled = false;
                                    // imgbtn2CHFdisable.Enabled = false;
                                    lnkBtnCHF2Cancel.Enabled = false;
                                }
                                ddCHFBasisCharged2.DataBind();

//                                ddCHFBasisCharged2.Items.FindByValue(LBAParamLBABE.clm_hndl_fee_basis_id.ToString()).Selected = true;
                                AddInActiveLookupData(ref ddCHFBasisCharged2, LBAParamLBABE.clm_hndl_fee_basis_id.Value);

                                IList<AdjustmentParameterPolicyBE> AdjParmBE = LBAParamLBABE.AdjParametPolBEs;

                                foreach (AdjustmentParameterPolicyBE LBAPolnBEdetails in AdjParmBE)
                                {
//                                    chkbxCHF2Policynolst.Items.FindByValue(LBAPolnBEdetails.coml_agmt_id.ToString()).Selected = true;
                                    AddInActiveLookupData(ref chkbxCHF2Policynolst, LBAPolnBEdetails.coml_agmt_id);
                                }
                                //int polCount4 = 0;
                                //for (int i = 0; i < chkbxCHF2Policynolst.Items.Count; i++)
                                //{
                                //    if (chkbxCHF2Policynolst.Items[i].Selected)
                                //    { ++polCount4; }
                                //}
                                //if (polCount4 == 0) lnkbtnCHF2infoDetails.Enabled = false;  

                                CheckNewNotincCHF = false;
                            }
                        }
                    }
                }
            }
            else
            {
                //this.lblcheckvalidations.Visible = true;
                //this.lblcheckvalidations.Text = "* No Policy exists for the selected Program Period";
                ShowMessage("No Policy exists for the selected Program Period");
                this.pnlAdlLBA.Visible = false;
                this.pnlAdjLCF.Visible = false;
                this.pnlAdjTM.Visible = false;
                this.pnlAdjRML.Visible = false;
                this.pnlADlCHF.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    #endregion

    #region LCFDetails


    /// <summary>
    /// Get LCF Information details from database to fill the list view cotrol 
    /// passing 3 parameters AccountID, CustomerID and ProgramPeriodID
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstLCFParmsetupDtls_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        lstLCFSetuplistView.EditIndex = e.NewEditIndex;
        int adjParmsetUpID = Convert.ToInt32(hdnLCFPrmsetuptxtBox.Text);
        BindLCFniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);
    }

    /// <summary>
    /// Cancel the changes made to the LCF details list view while one of the records is being edited.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstLCFParmsetupDtls_ItemCancel(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstLCFSetuplistView.EditIndex = -1;
            int adjParmsetUpID = Convert.ToInt32(hdnLCFPrmsetuptxtBox.Text);
            BindLCFniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);
        }
        else if (e.CancelMode == ListViewCancelMode.CancelingInsert)
        {
            CancelUpdateModeLCF(); //Back to normal mode.
        }

    }

    /// <summary>
    /// This function is used with above lstLCFParmsetupDtls_ItemCancel to perform the cancel operation
    /// </summary>
    protected void CancelUpdateModeLCF()
    {
        lstLCFSetuplistView.InsertItemPosition = InsertItemPosition.None;
        int adjParmsetUpID = Convert.ToInt32(hdnLCFPrmsetuptxtBox.Text);
        BindLCFniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);
    }

    /// <summary>
    /// This method is used by LCF listview to select the methods to  SAVE and UPDATE listview details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstLCFParmsetupDtls_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "Save")
        {
            lstLCFRelatedinfo_Saving(e.Item);
        }
        else if (e.CommandName == "Update")
        {
            lstLCFParmsetupDtls_ItemUpdate(e.Item);

        }
        else if (e.CommandName.ToUpper() == "DISABLE")
        {
            DisableLCFParmdtlsRow(e.Item, Convert.ToInt32(e.CommandArgument), false);
        }
        else if (e.CommandName.ToUpper() == "ENABLE")
        {
            DisableLCFParmdtlsRow(e.Item, Convert.ToInt32(e.CommandArgument), true);
        }
    }

    /// <summary>
    /// Method used to update\Edit a selected row in LCF details list view
    /// </summary>
    /// <param name="e"></param>
    protected void lstLCFParmsetupDtls_ItemUpdate(ListViewItem e)
    {
        try
        {
            //lblcheckvalidations.Visible = false;
            //lblcheckvalidations.Text = "";
            bool StateDetailsExists = false;
            //AdjustmentParameterDetailBE LCFInfoUpdtDetailsBE = new AdjustmentParameterDetailBE();

            Label lblPPSetupDtlsLCF = (Label)e.FindControl("lblAdjParmtdtlLCFID");
            string strLCFPPsetupdtlsID = lblPPSetupDtlsLCF.Text;
            DropDownList ddLCFLOBItms = (DropDownList)e.FindControl("ddLCFLOB");
            DropDownList ddLCFStateitms = (DropDownList)e.FindControl("ddlLCFState");
            TextBox txtLCFdtlsFctr = (TextBox)e.FindControl("txtLCFFactor");
            CheckBox chkActindLCF = (CheckBox)e.FindControl("chkActiveLCF");

            adjParmDtlBE = AdjPrmtDtlsBS.getAdjParamDtlRow(Convert.ToInt32(strLCFPPsetupdtlsID));
            //Concurrency Issue
            AdjustmentParameterDetailBE adjParmDtlBEold = (AdjParmDtlBElst.Where(o => o.prem_adj_pgm_dtl_id.Equals(Convert.ToInt32(strLCFPPsetupdtlsID)))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmDtlBEold.UPDTE_DATE), Convert.ToDateTime(adjParmDtlBE.UPDTE_DATE));
            if (!con)
                return;
            //End
            if (adjParmDtlBE.st_id != Convert.ToInt32(ddLCFStateitms.SelectedValue) ||
                adjParmDtlBE.ln_of_bsn_id != Convert.ToInt32(ddLCFLOBItms.SelectedValue) ||
                adjParmDtlBE.adj_fctr_rt != Convert.ToDecimal(txtLCFdtlsFctr.Text) ||
               adjParmDtlBE.act_ind != (chkActindLCF.Checked))
            {
                adjParmDtlBE.PrgmPerodID = int.Parse(ViewState["PRGPRDID"].ToString());
                adjParmDtlBE.AccountID = AISMasterEntities.AccountNumber;
                adjParmDtlBE.adj_paramet_id = Convert.ToInt32(hdnLCFPrmsetuptxtBox.Text);
                if (adjParmDtlBE.st_id != Convert.ToInt32(ddLCFStateitms.SelectedValue) ||
                    AdjParmDtlBE.ln_of_bsn_id != Convert.ToInt32(ddLCFLOBItms.SelectedValue))
                {
                    //Get the AdjParameter LCF details list for specific Account or Customer, Adjustmet Parameter Setupid and Program Period ID
                    IList<AdjustmentParameterDetailBE> AdjParmetDtlComparelstBE = AdjPrmtDtlsBS.getLBAAdjParamtrDtls(adjParmDtlBE.PrgmPerodID, adjParmDtlBE.adj_paramet_id, adjParmDtlBE.AccountID);

                    //check for each item in list if the values exists for the entered current state
                    //IF exists dont save the information
                    foreach (AdjustmentParameterDetailBE AdjCompareLCFdetailBE in AdjParmetDtlComparelstBE)
                    {
                        if (AdjCompareLCFdetailBE.st_id == Convert.ToInt32(ddLCFStateitms.SelectedValue) &&
                            AdjCompareLCFdetailBE.ln_of_bsn_id == Convert.ToInt32(ddLCFLOBItms.SelectedValue))
                        {
                            StateDetailsExists = true;
                            //lblcheckvalidations.Visible = true;
                            //lblcheckvalidations.Text = " * The details for the State you are trying to save already exists";
                            ShowMessage("The details for the State and Line of business you are trying to save already exists");
                            break;
                        }
                    }
                }
                if (!StateDetailsExists)
                {
                    adjParmDtlBE.st_id = Convert.ToInt32(ddLCFStateitms.SelectedValue);
                    adjParmDtlBE.ln_of_bsn_id = Convert.ToInt32(ddLCFLOBItms.SelectedValue);
                    adjParmDtlBE.adj_fctr_rt = Convert.ToDecimal(txtLCFdtlsFctr.Text);

                    if (chkActindLCF.Checked == false)
                    {
                        adjParmDtlBE.act_ind = false;
                    }
                    else
                    {
                        adjParmDtlBE.act_ind = true;
                    }

                    adjParmDtlBE.UPDTE_USER_ID = CurrentAISUser.PersonID;
                    adjParmDtlBE.UPDTE_DATE = DateTime.Now;

                    bool SaveSuccess = AdjPrmtDtlsBS.Update(adjParmDtlBE);
                    ShowConcurrentConflict(SaveSuccess, adjParmDtlBE.ErrorMessage);
                    if (SaveSuccess)
                    {
                        AdjParameterTransactionWrapper.SubmitTransactionChanges();
                        //Code for logging into Audit Transaction Table 
                        ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                        audtBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
                        audtBS.Save(AISMasterEntities.AccountNumber, adjParmDtlBE.PrgmPerodID, GlobalConstants.AuditingWebPage.LCFSetup, CurrentAISUser.PersonID);
                        AdjParameterTransactionWrapper.SubmitTransactionChanges();
                    }
                    else
                    {
                        AdjParameterTransactionWrapper.RollbackChanges();
                    }
                    this.lstLCFSetuplistView.EditIndex = -1;
                    BindLCFniformationDetails(adjParmDtlBE.PrgmPerodID, adjParmDtlBE.adj_paramet_id, AISMasterEntities.AccountNumber);

                }
            }
            else
            {
//                ShowMessage("No information has been changed to Save");
                lstLCFSetuplistView.EditIndex = -1;
                int adjParmsetUpID = Convert.ToInt32(hdnLCFPrmsetuptxtBox.Text);
                BindLCFniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// Method used to SAVE new record in an LCF details listview
    /// </summary>
    /// <param name="e"></param>
    protected void lstLCFRelatedinfo_Saving(ListViewItem e)
    {
        try
        {
            bool StateDetailsExists = false;
            //lblcheckvalidations.Text = "";
            //lblcheckvalidations.Visible = false;
            AdjustmentParameterDetailBE LCFDetailsBE = new AdjustmentParameterDetailBE();
            Adj_paramet_DtlBS LCFdtlsBS = new Adj_paramet_DtlBS();
            DropDownList ddLCFLOBItms = (DropDownList)e.FindControl("ddLCFLOB");
            DropDownList ddLCFStateitms = (DropDownList)e.FindControl("ddlLCFState");
            TextBox txtLCFdtlsFctr = (TextBox)e.FindControl("txtLCFFactor");

            LCFDetailsBE.AccountID = AISMasterEntities.AccountNumber;
            LCFDetailsBE.PrgmPerodID = int.Parse(ViewState["PRGPRDID"].ToString());
            LCFDetailsBE.adj_paramet_id = Convert.ToInt32(hdnLCFPrmsetuptxtBox.Text);

            //Get the AdjParameter LCF details list for specific Account or Customer, Adjustmet Parameter Setupid and Program Period ID
            IList<AdjustmentParameterDetailBE> AdjParmetDtlComparelstBE = AdjParmDtlsBS.getLBAAdjParamtrDtls(LCFDetailsBE.PrgmPerodID, LCFDetailsBE.adj_paramet_id, LCFDetailsBE.AccountID);

            //check for each item in list if the values exists for the entered current state
            //IF exists dont save the information
            foreach (AdjustmentParameterDetailBE AdjCompareLCFdetailBE in AdjParmetDtlComparelstBE)
            {
                if (AdjCompareLCFdetailBE.st_id == Convert.ToInt32(ddLCFStateitms.SelectedValue) &&
                    AdjCompareLCFdetailBE.ln_of_bsn_id == Convert.ToInt32(ddLCFLOBItms.SelectedValue))
                {
                    StateDetailsExists = true;
                    //lblcheckvalidations.Visible = true;
                    //lblcheckvalidations.Text = " * The details for the State and Line of Business you are trying to save already exists";
                    ShowMessage("The details for the State and Line of Business you are trying to save already exists");
                    break;
                }

            }
            if (!StateDetailsExists)
            {
                LCFDetailsBE.st_id = Convert.ToInt32(ddLCFStateitms.SelectedValue);
                LCFDetailsBE.ln_of_bsn_id = Convert.ToInt32(ddLCFLOBItms.SelectedValue);
                LCFDetailsBE.adj_fctr_rt = Convert.ToDecimal(txtLCFdtlsFctr.Text);

                LCFDetailsBE.CRTE_DATE = DateTime.Now;
                LCFDetailsBE.UPDTE_DATE = (Nullable<DateTime>)null;
                LCFDetailsBE.act_ind = true;
                LCFDetailsBE.CRTE_USER_ID = CurrentAISUser.PersonID;
                LCFDetailsBE.UPDTE_USER_ID = (Nullable<int>)null;
               bool flag= LCFdtlsBS.Update(LCFDetailsBE);
               ShowConcurrentConflict(flag, LCFDetailsBE.ErrorMessage);
                BindLCFniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), LCFDetailsBE.adj_paramet_id, AISMasterEntities.AccountNumber);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// This function is not used now
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstLCFParmsetupDtls_ItemUpdating(Object sender, ListViewUpdateEventArgs e)
    { }

    /// <summary>
    /// Close the below LCF listview panel and enable the above panel containing the LCF information table 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLCFInfoClose_Click(object sender, EventArgs e)
    {
        //this.pnlppLCF.Enabled = true;
        this.pnlAdjLCF.Enabled = true;
        this.lstLCFSetuplistView.Visible = false;
        this.lblLCFdtls.Visible = false;
        this.lnkLCFClose.Visible = false;
    }

    /// <summary>
    /// Bind the LCF details list view with data from the database and 
    /// Display the below panel containing the LCF listview control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLCFInfoDetailsInfo_Click(object sender, EventArgs e)
    {
        if (ViewState["AdjparmsetupforLCF"] != null)
        {
            //Close LBA listview in LBA TAB
            //this.pnlpp.Enabled = true;
            this.pnlAdlLBA.Enabled = true;
            this.lbasetupdetailslistview.Visible = false;
            this.LBASetupDetailsLabel.Visible = false;
            this.lnkbtnLBADetailsClose.Visible = false;

            //Close RML listview in RML TAB
            //this.pnlppRML.Enabled = true;
            this.pnlAdjRML.Enabled = true;
            this.lstRMLSetuplistView.Visible = false;
            this.lblRMLdtls.Visible = false;
            this.lnkBtnCloseRMLdtlsID.Visible = false;

            //Close WA-AX listview in TaxMultiplier TAB
            //this.pnlppTM.Enabled = true;
            this.pnlAdjTM.Enabled = true;
            this.lstTMSetuplistView.Visible = false;
            this.lblTMdtls.Visible = false;
            this.lnkBtnCloseTMdtlsID.Visible = false;

            //Close CHF listview in CHF TAB
            //this.pnlppCHF.Enabled = true;
            this.pnlADlCHF.Enabled = true;
            this.chfsetupdetailslistview.Visible = false;
            this.CHFSetupDetailsLabel.Visible = false;
            this.lnkbtnCHFDetailsClose.Visible = false;

            if (this.lblLCFdtls.Visible == false && this.lnkLCFClose.Visible == false)
            {
                this.lblLCFdtls.Visible = true;
                this.lnkLCFClose.Visible = true;
            }
            adjParmDtlBE = null;
            //this.pnlppLCF.Enabled = false;
            //this.lblcheckvalidations.Visible = false;
            //hdnAdjPrmsetup2txtBox.Text = "0";
            //hdnAdjPrmsetup1txtBox.Text = "0";
            hdnLCFPrmsetuptxtBox.Text = ViewState["AdjparmsetupforLCF"].ToString();
            this.lstLCFSetuplistView.EditIndex = -1;
            BindLCFniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), int.Parse(ViewState["AdjparmsetupforLCF"].ToString()), AISMasterEntities.AccountNumber);
            this.pnlAdjLCF.Enabled = false;
        }
        else
        {
            ShowMessage("Please save LCF Policy information before selecting the Detail information");
        }
    }

    //protected void btnlcfsetupinfodetails_click(object sender, EventArgs e)
    //{ }

    /// <summary>
    /// Bind the LCF Setp Up details List view at the bottom of the screen from database
    /// </summary>
    /// <param name="prgmperiodID"></param>
    /// <param name="AdjPrmtSetupID"></param>
    /// <param name="CustomerID"></param>
    public void BindLCFniformationDetails(int prgmperiodID, int AdjPrmtSetupID, int CustomerID)
    {
        //lblcheckvalidations.Visible = false;
        //lblcheckvalidations.Text = "";
        if (this.LBASetupDetailsLabel.Visible == false && this.lnkLCFClose.Visible == false)
        {
            this.LBASetupDetailsLabel.Visible = true;
            this.lnkLCFClose.Visible = true;
        }
        Adj_paramet_DtlBS AdjLBAprmDtlBS = new Adj_paramet_DtlBS();
        //IList<AdjustmentParameterDetailBE> AdjParmDtlBElst 
       // IList<AdjustmentParameterDetailBE> 
         AdjParmDtlBElst = AdjLBAprmDtlBS.getLBAAdjParamtrDtls(prgmperiodID, AdjPrmtSetupID, CustomerID);
        this.lstLCFSetuplistView.DataSource = AdjParmDtlBElst;
        this.lstLCFSetuplistView.DataBind();
        this.lstLCFSetuplistView.Visible = true;
    }

    /// <summary>
    /// Save method used to assign the values to a database field from the screen and save\update the details
    /// with Transaction processing for the first row in LBA Tab
    /// </summary>
    /// <param name="AdjStupBE"></param>
    /// <returns></returns>
    protected AdjustmentParameterSetupBE SaveEntityLCF(AdjustmentParameterSetupBE AdjStupLCFBE)
    {
        AdjStupLCFBE.prem_adj_pgm_id = int.Parse(ViewState["PRGPRDID"].ToString());
        if (txtLCFAggtAmt.Text != "")
        {
            AdjStupLCFBE.loss_convfact_aggamt = decimal.Parse(txtLCFAggtAmt.Text.Replace(",", ""));
        }
        else
        {
            AdjStupLCFBE.loss_convfact_aggamt = 0;
        }
        if (txtLCFClmCAP.Text != "")
        {
            AdjStupLCFBE.loss_convfact_calimcap = decimal.Parse(txtLCFClmCAP.Text.Replace(",", ""));
        }
        else
        {
            AdjStupLCFBE.loss_convfact_calimcap = 0;
        }
        if (txtLayLCFIPay.Text != "")
        {
            AdjStupLCFBE.lay_lossconv_FactInsPay = decimal.Parse(txtLayLCFIPay.Text.Replace(",", ""));
        }
        else
        {
            AdjStupLCFBE.lay_lossconv_FactInsPay = 0;
        }
        if (txtLayLCFZurchPay.Text != "")
        {
            AdjStupLCFBE.lay_lossconv_znapayamt = decimal.Parse(txtLayLCFZurchPay.Text.Replace(",", ""));
        }
        else
        {
            AdjStupLCFBE.lay_lossconv_znapayamt = 0;
        }
        AdjStupLCFBE.Cstmr_Id = AISMasterEntities.AccountNumber;
        AdjStupLCFBE.AdjparameterTypeID = Lbablaccess.GetLookUpID("LCF", "ADJUSTMENT PARAMETER TYPE");
        if (CheckNewLCF == false)
        {
            AdjStupLCFBE.UPDATE_DATE = DateTime.Now;
            AdjStupLCFBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
        }
        else
        {
            AdjStupLCFBE.actv_ind = true;
            AdjStupLCFBE.CREATE_DATE = DateTime.Now;
            AdjStupLCFBE.CREATE_USER_ID = CurrentAISUser.PersonID;
        }

        AdjStupLCFBE.incld_ernd_retro_prem_ind = false;

        bool ResultLCF = AdjPrmStupBS.Update(AdjStupLCFBE);
        if (ResultLCF)
        {
            bool aptflag = AdjParameterTransactionWrapper.SubmitTransactionChanges();
            ShowConcurrentConflict(aptflag, AdjParameterTransactionWrapper.ErrorMessage);

            if (CheckNewLCF == false)
            {

                //Code for logging into Audit Transaction Table 
                ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                audtBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
                audtBS.Save(AISMasterEntities.AccountNumber, AdjStupLCFBE.prem_adj_pgm_id, GlobalConstants.AuditingWebPage.LCFSetup, CurrentAISUser.PersonID);
                //AdjParameterTransactionWrapper.SubmitTransactionChanges();
            }
            AdjParamPolInfo.deletePol(AdjStupLCFBE.Cstmr_Id, AdjStupLCFBE.adj_paramet_setup_id);
            if (AdjStupLCFBE.adj_paramet_setup_id > 0)
            {

                for (int i = 0; i < ckkboxlstPolicyno.Items.Count; i++)
                {
                    if (ckkboxlstPolicyno.Items[i].Selected)
                    {
                        AdjustmentParameterPolicyBE LCFPolicylinkBE = new AdjustmentParameterPolicyBE();
                        LCFPolicylinkBE.adj_paramet_setup_id = AdjStupLCFBE.adj_paramet_setup_id;
                        LCFPolicylinkBE.custmrID = AISMasterEntities.AccountNumber;
                        LCFPolicylinkBE.PrmadjPRgmID = int.Parse(ViewState["PRGPRDID"].ToString());
                        LCFPolicylinkBE.coml_agmt_id = int.Parse(ckkboxlstPolicyno.Items[i].Value);
                        LCFPolicylinkBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                        LCFPolicylinkBE.CREATE_DATE = DateTime.Now;
                        AdjParamPolInfo.Update(LCFPolicylinkBE);

                    }
                }


            }
            bool aptwflg = AdjParameterTransactionWrapper.SubmitTransactionChanges();
            ShowConcurrentConflict(aptwflg, AdjParameterTransactionWrapper.ErrorMessage);
        }
        else
        {
            AdjParameterTransactionWrapper.RollbackChanges();
        }

        return AdjStupLCFBE;

    }

    /// <summary>
    /// Save LCF-Information the details from LCF table row
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnLCFInfoDetailsSave_Click(object sender, EventArgs e)
    {
        try
        {
            int PolCount = 0;
            bool Flag = false;

            for (int Pol = 0; Pol < ckkboxlstPolicyno.Items.Count; Pol++)
            {
                if (ckkboxlstPolicyno.Items[Pol].Selected)
                {
                    PolCount = PolCount + 1;
                }
            }

            if (PolCount > 0)
            {
                if (CheckNewLCF == false)
                {

                    adjParmStupBE = AdjPrmStupBS.getAdjParamRow(int.Parse(ViewState["AdjparmsetupforLCF"].ToString()));
                    //Concurrency Issue
                    AdjustmentParameterSetupBE adjParmStupBEold = (Bindlist.Where(o => o.adj_paramet_setup_id.Equals(int.Parse(ViewState["AdjparmsetupforLCF"].ToString())))).First();
                    bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmStupBEold.UPDATE_DATE), Convert.ToDateTime(adjParmStupBE.UPDATE_DATE));
                    if (!con)
                        return;
                    //End
                    if (CompareValues(adjParmStupBE, PolCount))
                    {
                        Flag = true;
                    }
                    else
                    {
                        Flag = false;
                    }
                }
//                if (!Flag)
//                {
                //To check Concurrency on Save for LCF
                if (CheckNewLCF == true)
                {
                    string AdjReviewResult = AdjPrmStupBS.getAdjParamResultOther(int.Parse(ViewState["PRGPRDID"].ToString()), AISMasterEntities.AccountNumber, Lbablaccess.GetLookUpID("LCF", "ADJUSTMENT PARAMETER TYPE"));
                    if (AdjReviewResult == "true")
                    {
                        ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
                        return;
                    }
                }
                //End
                    adjParmStupBE = SaveEntityLCF(AdjParmStupBE);
                    BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
//                }
                //else
                //{
                //    ShowMessage("No information has been changed to Save");
                //}
                //this.lblcheckvalidations.Visible = false;
                //this.lblcheckvalidations.Text = "";
                //BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
            }
            else
            {
                //this.lblcheckvalidations.Visible = true;
                //this.lblcheckvalidations.Text = " * Please select at least one policy before saving LCF information";
                ShowMessage("Please select at least one policy before saving LCF information");
            }
        }
        catch (Exception ex)
        {
            ShowError((ex.Message.Contains("changed")) ? GlobalConstants.ErrorMessage.RowNotFoundOrChanged : GlobalConstants.ErrorMessage.ServerTooBusy,ex);
        }
    }

    /// <summary>
    /// Enable\Disable or change the status of the selected record on te screen
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DisableLCFRow(object sender, CommandEventArgs e)
    {
        try
        {
            if (ViewState["AdjparmsetupforLCF"] != null)
            {
                int AdjParmtSetupID = int.Parse(ViewState["AdjparmsetupforLCF"].ToString());
                //LBAParmBE = AdjparamsetupInfo.getAdjParamRow(AdjParmtSetupID);
                adjParmStupBE = AdjPrmStupBS.getAdjParamRow(AdjParmtSetupID);
                //Concurrency Issue
                AdjustmentParameterSetupBE adjParmStupBEold = (Bindlist.Where(o => o.adj_paramet_setup_id.Equals(int.Parse(ViewState["AdjparmsetupforLCF"].ToString())))).First();
                bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmStupBEold.UPDATE_DATE), Convert.ToDateTime(adjParmStupBE.UPDATE_DATE));
                if (!con)
                    return;
                //End
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
                bool ResultLCF = AdjPrmStupBS.Update(adjParmStupBE);
                if (ResultLCF)
                {
                    AdjParameterTransactionWrapper.SubmitTransactionChanges();
                    //Code for logging into Audit Transaction Table 
                    ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                    audtBS.Save(AISMasterEntities.AccountNumber, adjParmStupBE.prem_adj_pgm_id, GlobalConstants.AuditingWebPage.LCFSetup, CurrentAISUser.PersonID);
                    AdjParameterTransactionWrapper.SubmitTransactionChanges();
                }
                else
                {
                    AdjParameterTransactionWrapper.RollbackChanges();
                }
                BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// Disable\Enable LCF information details for a state
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DisableLCFParmdtlsRow(ListViewItem e, int AdjparmetDtlID, bool Flag)
    {
        try
        {
            //AdjustmentParameterDetailBE  AdjprmDtlBE;
            //Adj_paramet_DtlBS AdjParmdtlBS = new Adj_paramet_DtlBS();
            adjParmDtlBE = AdjPrmtDtlsBS.getAdjParamDtlRow(AdjparmetDtlID);
            //Concurrency Issue
            AdjustmentParameterDetailBE adjParmDtlBEold = (AdjParmDtlBElst.Where(o => o.prem_adj_pgm_dtl_id.Equals(AdjparmetDtlID))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmDtlBEold.UPDTE_DATE), Convert.ToDateTime(adjParmDtlBE.UPDTE_DATE));
            if (!con)
                return;
            //End
            adjParmDtlBE.act_ind = Flag;
            adjParmDtlBE.UPDTE_USER_ID = CurrentAISUser.PersonID;
            adjParmDtlBE.UPDTE_DATE = DateTime.Now;
            Flag = AdjPrmtDtlsBS.Update(adjParmDtlBE);
            if (Flag)
            {
                AdjParameterTransactionWrapper.SubmitTransactionChanges();
                //Code for logging into Audit Transaction Table 
                ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                audtBS.Save(AISMasterEntities.AccountNumber, adjParmDtlBE.PrgmPerodID, GlobalConstants.AuditingWebPage.LCFSetup, CurrentAISUser.PersonID);
                AdjParameterTransactionWrapper.SubmitTransactionChanges();
            }
            else
            {
                AdjParameterTransactionWrapper.RollbackChanges();
            }
            int Adjparmsetupiid;
            Adjparmsetupiid = Convert.ToInt32(hdnLCFPrmsetuptxtBox.Text);
            BindLCFniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), Adjparmsetupiid, AISMasterEntities.AccountNumber);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// The method is used after the LCF details info listview control is bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstLCFParmsetupDtls_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            DropDownList ddLCFStateitms = (DropDownList)e.Item.FindControl("ddlLCFState");
            DropDownList ddLCFLOBType = (DropDownList)e.Item.FindControl("ddLCFLOB");

            Label LCFFactor = (Label)e.Item.FindControl("lblLCFactor");
            Label hdLCFState = (Label)e.Item.FindControl("hidLCFState");
            Label hdLCFLOB = (Label)e.Item.FindControl("hidLCFLOB");
            if (LCFFactor != null)
            {
                if ((LCFFactor.Text.Length > 8))
                {
                    LCFFactor.Text = LCFFactor.Text.Substring(0, 8);
                }
            }
            if (ddLCFStateitms != null)
            {
                if ((ddLCFStateitms != null) && (hdLCFState != null))
                {
//                    ddLCFStateitms.SelectedIndex = ddLCFStateitms.Items.IndexOf(ddLCFStateitms.Items.FindByText(hdLCFState.Text.ToString()));
                    AddInActiveLookupDataByText(ref ddLCFStateitms, hdLCFState.Text);
                }

                if ((ddLCFLOBType != null) && (hdLCFLOB != null))
                {
//                    ddLCFLOBType.SelectedIndex = ddLCFLOBType.Items.IndexOf(ddLCFLOBType.Items.FindByText(hdLCFLOB.Text.ToString()));
                    AddInActiveLookupDataByText(ref ddLCFLOBType, hdLCFLOB.Text);
                }
            }
            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDisableEnableLCF");

            if (imgDelete != null)
            {
                HiddenField hid = (HiddenField)e.Item.FindControl("hidActiveLCF");

                if (hid.Value == "True")
                {
                    LinkButton lbLCFEDit = (LinkButton)e.Item.FindControl("lcfdetailsetupEdit");
                    if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                        lbLCFEDit.Enabled = true;
                    imgDelete.CommandName = "Disable";
                    imgDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable this record?');");
                }
                else
                {
                    LinkButton lbLCFEDitAlt = (LinkButton)e.Item.FindControl("lcfdetailsetupEdit");
                    lbLCFEDitAlt.Enabled = false;
                    imgDelete.CommandName = "Enable";
                    imgDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable this record?');");
                }
            }
        }
    }
    #endregion

    #region TMTABWATax

    /// <summary>
    /// Get WA-TAX Information details from database to fill the list view cotrol 
    /// passing 3 parameters AccountID, CustomerID and ProgramPeriodID
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstTMParmsetupDtls_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        lstTMSetuplistView.EditIndex = e.NewEditIndex;
        int adjParmsetUpID = Convert.ToInt32(hdnTMPrmsetuptxtBox.Text);
        BindTMniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);
    }

    /// <summary>
    /// Save TM-Information 1st grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnTMInfoDetailsSave_Click(object sender, EventArgs e)
    {
        try
        {
            int PolCountTM = 0;
            bool Flag = false;

            for (int Pol = 0; Pol < ChkBoxLstTMpolicy.Items.Count; Pol++)
            {
                if (ChkBoxLstTMpolicy.Items[Pol].Selected)
                {
                    PolCountTM = PolCountTM + 1;
                }
            }

            if (PolCountTM > 0)
            {


                if (CheckNewTM == false)
                {
                    adjParmStupBE = AdjPrmStupBS.getAdjParamRow(int.Parse(ViewState["AdjparmsetupforTM"].ToString()));
                    //Concurrency Issue
                    AdjustmentParameterSetupBE adjParmStupBEold = (Bindlist.Where(o => o.adj_paramet_setup_id.Equals(int.Parse(ViewState["AdjparmsetupforTM"].ToString())))).First();
                    bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmStupBEold.UPDATE_DATE), Convert.ToDateTime(adjParmStupBE.UPDATE_DATE));
                    if (!con)
                        return;
                    //End
                    if (CompareValuesecrow(adjParmStupBE, PolCountTM))
                    {
                        Flag = true;
                    }
                    else
                    {
                        Flag = false;
                    }

                }

//                if (!Flag)
//                {
                //To check Concurrency on Save for WA
                if (CheckNewTM == true)
                {
                    string AdjReviewResult = AdjPrmStupBS.getAdjParamResultOther(int.Parse(ViewState["PRGPRDID"].ToString()), AISMasterEntities.AccountNumber, Lbablaccess.GetLookUpID("WA", "ADJUSTMENT PARAMETER TYPE"));
                    if (AdjReviewResult == "true")
                    {
                        ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
                        return;
                    }
                }
                //End
                    adjParmStupBE = SaveEntityTM(AdjParmStupBE);
                    BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
//                }
                //else
                //{
                //    ShowMessage("No information has been changed to Save");
                //}
                //this.lblcheckvalidations.Visible = false;
                //this.lblcheckvalidations.Text = "";

            }
            else
            {
                //this.lblcheckvalidations.Visible = true;
                //this.lblcheckvalidations.Text = " * Please select at least one policy before saving WA-Tax information";
                ShowMessage("Please select at least one policy before saving WA-Tax information");
            }
        }
        catch (Exception ex)
        {
            ShowError((ex.Message.Contains("changed")) ? GlobalConstants.ErrorMessage.RowNotFoundOrChanged : GlobalConstants.ErrorMessage.ServerTooBusy, ex);
        }
    }

    /// <summary>
    /// Save method used to assign the values to a database field from the screen and save\update the details
    /// with Transaction processing for the first row in TM Tab
    /// </summary>
    /// <param name="AdjStupBE"></param>
    /// <returns></returns>
    protected AdjustmentParameterSetupBE SaveEntityTM(AdjustmentParameterSetupBE AdjStupTMBE)
    {
        AdjStupTMBE.prem_adj_pgm_id = int.Parse(ViewState["PRGPRDID"].ToString());
        AdjStupTMBE.Cstmr_Id = AISMasterEntities.AccountNumber;
        AdjStupTMBE.AdjparameterTypeID = Lbablaccess.GetLookUpID("WA", "ADJUSTMENT PARAMETER TYPE");
        if (CheckNewTM == false)
        {
            AdjStupTMBE.UPDATE_DATE = DateTime.Now;
            AdjStupTMBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
        }
        else
        {
            AdjStupTMBE.actv_ind = true;
            AdjStupTMBE.CREATE_DATE = DateTime.Now;
            AdjStupTMBE.CREATE_USER_ID = CurrentAISUser.PersonID;
        }

        //Included to avoid "NULL" value in this field
        AdjStupTMBE.incld_ernd_retro_prem_ind = false;

        bool ResultTM = AdjPrmStupBS.Update(AdjStupTMBE);
        if (ResultTM)
        {
            bool aptflag = AdjParameterTransactionWrapper.SubmitTransactionChanges();
            ShowConcurrentConflict(aptflag, AdjParameterTransactionWrapper.ErrorMessage);

            if (CheckNewTM == false)
            {
                //Code for logging into Audit Transaction Table 
                ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                audtBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
                audtBS.Save(AISMasterEntities.AccountNumber, AdjStupTMBE.prem_adj_pgm_id, GlobalConstants.AuditingWebPage.TaxMultiplierSetup, CurrentAISUser.PersonID);
            }

            AdjParamPolInfo.deletePol(AdjStupTMBE.Cstmr_Id, AdjStupTMBE.adj_paramet_setup_id);
            if (AdjStupTMBE.adj_paramet_setup_id > 0)
            {

                for (int i = 0; i < ChkBoxLstTMpolicy.Items.Count; i++)
                {
                    if (ChkBoxLstTMpolicy.Items[i].Selected)
                    {
                        AdjustmentParameterPolicyBE TMPolicylinkBE = new AdjustmentParameterPolicyBE();
                        TMPolicylinkBE.adj_paramet_setup_id = AdjStupTMBE.adj_paramet_setup_id;
                        TMPolicylinkBE.custmrID = AISMasterEntities.AccountNumber;
                        TMPolicylinkBE.PrmadjPRgmID = int.Parse(ViewState["PRGPRDID"].ToString());
                        TMPolicylinkBE.coml_agmt_id = int.Parse(ChkBoxLstTMpolicy.Items[i].Value);
                        TMPolicylinkBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                        TMPolicylinkBE.CREATE_DATE = DateTime.Now;
                        AdjParamPolInfo.Update(TMPolicylinkBE);

                    }
                }
            }
            bool aptwFlag = AdjParameterTransactionWrapper.SubmitTransactionChanges();
            ShowConcurrentConflict(aptwFlag, AdjParameterTransactionWrapper.ErrorMessage);
        }
        else
        {
            AdjParameterTransactionWrapper.RollbackChanges();
        }

        return AdjStupTMBE;
    }

    /// <summary>
    /// This function is used with above lstTMParmsetupDtls_ItemCancel to perform the cancel operation
    /// </summary>
    protected void CancelUpdateModeTM()
    {
        lstTMSetuplistView.InsertItemPosition = InsertItemPosition.None;
        BindTMniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), Convert.ToInt32(hdnTMPrmsetuptxtBox.Text), AISMasterEntities.AccountNumber);
    }

    /// <summary>
    /// Show WA TAX information in listview(Setup details) for first row
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnTMInfoDetailsInfo_Click(object sender, EventArgs e)
    {
        if (ViewState["AdjparmsetupforTM"] != null)
        {
            //Close LBA listview in LBA TAB
            //this.pnlpp.Enabled = true;
            this.pnlAdlLBA.Enabled = true;
            this.lbasetupdetailslistview.Visible = false;
            this.LBASetupDetailsLabel.Visible = false;
            this.lnkbtnLBADetailsClose.Visible = false;

            //Close LCF listview in LCF TAB
            //this.pnlppLCF.Enabled = true;
            this.pnlAdjLCF.Enabled = true;
            this.lstLCFSetuplistView.Visible = false;
            this.lblLCFdtls.Visible = false;
            this.lnkLCFClose.Visible = false;

            //Close RML listview in RML TAB
            //this.pnlppRML.Enabled = true;
            this.pnlAdjRML.Enabled = true;
            this.lstRMLSetuplistView.Visible = false;
            this.lblRMLdtls.Visible = false;
            this.lnkBtnCloseRMLdtlsID.Visible = false;

            //Close CHF listview in CHF TAB
            //this.pnlppCHF.Enabled = true;
            this.pnlADlCHF.Enabled = true;
            this.chfsetupdetailslistview.Visible = false;
            this.CHFSetupDetailsLabel.Visible = false;
            this.lnkbtnCHFDetailsClose.Visible = false;

            if (this.lblTMdtls.Visible == false && this.lnkBtnCloseTMdtlsID.Visible == false)
            {
                this.lblTMdtls.Visible = true;
                this.lnkBtnCloseTMdtlsID.Visible = true;
            }
            adjParmDtlBE = null;
            //this.pnlppTM.Enabled = false;
            //this.lblcheckvalidations.Visible = false;
            //hdnAdjPrmsetup2txtBox.Text = "0";
            //hdnAdjPrmsetup1txtBox.Text = ViewState["ADJLBAPRGID"].ToString();
            hdnTMPrmsetuptxtBox.Text = ViewState["AdjparmsetupforTM"].ToString();
            this.lstTMSetuplistView.EditIndex = -1;
            BindTMniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), int.Parse(ViewState["AdjparmsetupforTM"].ToString()), AISMasterEntities.AccountNumber);
            this.pnlAdjTM.Enabled = false;
        }
        else
        {
            ShowMessage("Please save WA-TAX Policy information before selecting the Detail information");
        }
    }

    /// <summary>
    /// Bind the listview for WA-TAX details 
    /// </summary>
    /// <param name="prgmperiodID"></param>
    /// <param name="AdjPrmtSetupID"></param>
    /// <param name="CustomerID"></param>
    public void BindTMniformationDetails(int prgmperiodID, int AdjPrmtSetupID, int CustomerID)
    {
        //lblcheckvalidations.Visible = false;
        //lblcheckvalidations.Text = "";
        if (this.lblTMdtls.Visible == false && this.lnkBtnCloseTMdtlsID.Visible == false)
        {
            this.lblTMdtls.Visible = true;
            this.lnkBtnCloseTMdtlsID.Visible = true;
        }
        Adj_paramet_DtlBS AdjLBAprmDtlBS = new Adj_paramet_DtlBS();
        //IList<AdjustmentParameterDetailBE> AdjParmDtlBElst 
        //IList<AdjustmentParameterDetailBE> 
        AdjParmDtlBElst = AdjLBAprmDtlBS.getLBAAdjParamtrDtls(prgmperiodID, AdjPrmtSetupID, CustomerID);
        this.lstTMSetuplistView.DataSource = AdjParmDtlBElst;
        this.lstTMSetuplistView.DataBind();
        this.lstTMSetuplistView.Visible = true;
    }

    /// <summary>
    /// Close the WA-TAX List view control along with its label and this close button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnTMInfoClose_Click(object sender, EventArgs e)
    {
        //this.pnlppTM.Enabled = true;
        this.pnlAdjTM.Enabled = true;
        this.lstTMSetuplistView.Visible = false;
        this.lblTMdtls.Visible = false;
        this.lnkBtnCloseTMdtlsID.Visible = false;
    }

    //protected void btnTMsetupinfodetails_click(object sender, EventArgs e)
    //{ }

    /// <summary>
    /// Disable\Enable First Row for WA-TAX details table
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DisableTMRow(object sender, CommandEventArgs e)
    {
        try
        {
            if (ViewState["AdjparmsetupforTM"] != null)
            {
                int AdjParmtSetupID = int.Parse(ViewState["AdjparmsetupforTM"].ToString());
                //LBAParmBE = AdjparamsetupInfo.getAdjParamRow(AdjParmtSetupID);
                adjParmStupBE = AdjPrmStupBS.getAdjParamRow(AdjParmtSetupID);
                //Concurrency Issue
                AdjustmentParameterSetupBE adjParmStupBEold = (Bindlist.Where(o => o.adj_paramet_setup_id.Equals(AdjParmtSetupID))).First();
                bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmStupBEold.UPDATE_DATE), Convert.ToDateTime(adjParmStupBE.UPDATE_DATE));
                if (!con)
                    return;
                //End
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
                bool ResultTM = AdjPrmStupBS.Update(adjParmStupBE);
                if (ResultTM)
                {
                    AdjParameterTransactionWrapper.SubmitTransactionChanges();
                    //Code for logging into Audit Transaction Table 
                    ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                    audtBS.Save(AISMasterEntities.AccountNumber, adjParmStupBE.prem_adj_pgm_id, GlobalConstants.AuditingWebPage.TaxMultiplierSetup, CurrentAISUser.PersonID);
                    AdjParameterTransactionWrapper.SubmitTransactionChanges();
                }
                else
                {
                    AdjParameterTransactionWrapper.RollbackChanges();
                }
                BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// This method is used to check if the user is updating or saving list view details and also
    /// calling those update and save methods, Also used to unable and Disable the rows in list view
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstTMParmsetupDtls_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "Save")
        {
            lstTMRelatedinfo_Saving(e.Item);
        }
        else if (e.CommandName == "Update")
        {
            lstTMParmsetupDtls_ItemUpdate(e.Item);
        }
        else if (e.CommandName.ToUpper() == "DISABLE")
        {
            DisableTMParmdtlsRow(e.Item, Convert.ToInt32(e.CommandArgument), false);
        }
        else if (e.CommandName.ToUpper() == "ENABLE")
        {
            DisableTMParmdtlsRow(e.Item, Convert.ToInt32(e.CommandArgument), true);
        }
    }

    /// <summary>
    /// Disable\Enable WA-TAX information details for a state
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DisableTMParmdtlsRow(ListViewItem e, int AdjparmetDtlID, bool Flag)
    {
        try
        {
            adjParmDtlBE = AdjPrmtDtlsBS.getAdjParamDtlRow(AdjparmetDtlID);
            //Concurrency Issue
            AdjustmentParameterDetailBE adjParmDtlBEold = (AdjParmDtlBElst.Where(o => o.prem_adj_pgm_dtl_id.Equals(AdjparmetDtlID))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmDtlBEold.UPDTE_DATE), Convert.ToDateTime(adjParmDtlBE.UPDTE_DATE));
            if (!con)
                return;
            //End
            adjParmDtlBE.act_ind = Flag;
            adjParmDtlBE.UPDTE_USER_ID = CurrentAISUser.PersonID;
            adjParmDtlBE.UPDTE_DATE = DateTime.Now;
            Flag = AdjPrmtDtlsBS.Update(adjParmDtlBE);
            if (Flag)
            {
                AdjParameterTransactionWrapper.SubmitTransactionChanges();
                //Code for logging into Audit Transaction Table 
                ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                audtBS.Save(AISMasterEntities.AccountNumber, adjParmDtlBE.PrgmPerodID, GlobalConstants.AuditingWebPage.TaxMultiplierSetup, CurrentAISUser.PersonID);
                AdjParameterTransactionWrapper.SubmitTransactionChanges();
            }
            else
            {
                AdjParameterTransactionWrapper.RollbackChanges();
            }
            int Adjparmsetupiid = Convert.ToInt32(hdnTMPrmsetuptxtBox.Text);
            BindTMniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), Adjparmsetupiid, AISMasterEntities.AccountNumber);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// This method is not used now was written initially while development
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstTMParmsetupDtls_ItemUpdating(Object sender, ListViewUpdateEventArgs e)
    { }

    /// <summary>
    /// Cancel any changes if user had selected any items in list view for updation
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstTMParmsetupDtls_ItemCancel(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstTMSetuplistView.EditIndex = -1;
            int adjParmsetUpID = Convert.ToInt32(hdnTMPrmsetuptxtBox.Text);
            BindTMniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);
        }
        else if (e.CancelMode == ListViewCancelMode.CancelingInsert)
        {
            CancelUpdateModeTM(); //Back to normal mode.
        }
    }

    /// <summary>
    /// The method is used after the WA-TAX details info listview control is bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstTMParmsetupDtls_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            DropDownList ddTMStateitms = (DropDownList)e.Item.FindControl("ddlTMState");
            Label hdTMState = (Label)e.Item.FindControl("hidTMState");


            if ((ddTMStateitms != null) && (hdTMState != null))
            {
//                ddTMStateitms.SelectedIndex = ddTMStateitms.Items.IndexOf(ddTMStateitms.Items.FindByText(hdTMState.Text.ToString()));
                AddInActiveLookupDataByText(ref ddTMStateitms, hdTMState.Text);
            }

            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDisableEnableTM");

            if (imgDelete != null)
            {
                HiddenField hid = (HiddenField)e.Item.FindControl("hidActiveTM");

                if (hid.Value == "True")
                {
                    LinkButton lbTMEDit = (LinkButton)e.Item.FindControl("TMdetailsetupEdit");
                    if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                        lbTMEDit.Enabled = true;
                    imgDelete.CommandName = "Disable";
                    imgDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable this record?');");
                }
                else
                {
                    LinkButton lbTMEDitAlt = (LinkButton)e.Item.FindControl("TMdetailsetupEdit");
                    lbTMEDitAlt.Enabled = false;
                    imgDelete.CommandName = "Enable";
                    imgDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable this record?');");
                }
            }
        }
    }

    /// <summary>
    /// Save WA-TAX information details like state, Factor, Deposit Amount and Comments
    /// State and Factor are compulsory fields
    /// </summary>
    /// <param name="e"></param>
    protected void lstTMRelatedinfo_Saving(ListViewItem e)
    {
        try
        {
            bool StateDetailsExists = false;
            //lblcheckvalidations.Text = "";
            //lblcheckvalidations.Visible = false;
            AdjustmentParameterDetailBE TMInfoDetailsBE = new AdjustmentParameterDetailBE();
            Adj_paramet_DtlBS TMdtlsBS = new Adj_paramet_DtlBS();
            DropDownList ddTMStateitms = (DropDownList)e.FindControl("ddlTMState");
            TextBox txtTMdtlsFctr = (TextBox)e.FindControl("txtTMFactor");
            TextBox txtTMPremAssmtAmt = (TextBox)e.FindControl("txtPremAssessmentTM");


            TMInfoDetailsBE.AccountID = AISMasterEntities.AccountNumber;
            TMInfoDetailsBE.PrgmPerodID = int.Parse(ViewState["PRGPRDID"].ToString());
            TMInfoDetailsBE.adj_paramet_id = Convert.ToInt32(hdnTMPrmsetuptxtBox.Text);
            //Get the AdjParameter WA-TAX details list for specific Account or Customer, Adjustmet Parameter Setupid and Program Period ID
            IList<AdjustmentParameterDetailBE> AdjParmetDtlComparelstBE = AdjParmDtlsBS.getLBAAdjParamtrDtls(TMInfoDetailsBE.PrgmPerodID, TMInfoDetailsBE.adj_paramet_id, TMInfoDetailsBE.AccountID);

            //check for each item in list if the values exists for the entered current state
            //IF exists dont save the information
            foreach (AdjustmentParameterDetailBE AdjCompareTMdetailBE in AdjParmetDtlComparelstBE)
            {
                if (AdjCompareTMdetailBE.st_id == Convert.ToInt32(ddTMStateitms.SelectedValue))
                {
                    StateDetailsExists = true;
                    //lblcheckvalidations.Visible = true;
                    //lblcheckvalidations.Text = " * The details for the State you are trying to save already exists";
                    ShowMessage("The details for the State you are trying to save already exists");
                    break;
                }

            }
            if (!StateDetailsExists)
            {

                TMInfoDetailsBE.st_id = Convert.ToInt32(ddTMStateitms.SelectedValue);
                TMInfoDetailsBE.adj_fctr_rt = Convert.ToDecimal(txtTMdtlsFctr.Text);

                if (txtTMPremAssmtAmt.Text != "")
                {
                    TMInfoDetailsBE.PremAssementAmt = Convert.ToDecimal(txtTMPremAssmtAmt.Text);
                }
                else
                {
                    TMInfoDetailsBE.PremAssementAmt = null;
                }
                TMInfoDetailsBE.CRTE_DATE = DateTime.Now;
                TMInfoDetailsBE.UPDTE_DATE = (Nullable<DateTime>)null;
                TMInfoDetailsBE.act_ind = true;
                TMInfoDetailsBE.CRTE_USER_ID = CurrentAISUser.PersonID;
                TMInfoDetailsBE.UPDTE_USER_ID = (Nullable<int>)null;
                TMdtlsBS.Update(TMInfoDetailsBE);
                BindTMniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), TMInfoDetailsBE.adj_paramet_id, AISMasterEntities.AccountNumber);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// Method used to update\Edit a selected row in WA-TAX details list view
    /// </summary>
    /// <param name="e"></param>
    protected void lstTMParmsetupDtls_ItemUpdate(ListViewItem e)
    {
        try
        {
            //lblcheckvalidations.Visible = false;
            //lblcheckvalidations.Text = "";
            bool StateDetailsExists = false;
            //AdjustmentParameterDetailBE TMInfoUpdtDetailsBE = new AdjustmentParameterDetailBE();

            Label lblPPSetupDtlsTM = (Label)e.FindControl("lblAdjParmtdtlTMID");
            string strTMPPsetupdtlsID = lblPPSetupDtlsTM.Text;
            DropDownList ddTMStateitms = (DropDownList)e.FindControl("ddlTMState");
            TextBox txtTMdtlsFctr = (TextBox)e.FindControl("txtTMFactor");
            TextBox txtTMPremAssmtAmt = (TextBox)e.FindControl("txtPremAssessmentTM");
            CheckBox chkActindTM = (CheckBox)e.FindControl("chkActiveTM");
          
            adjParmDtlBE = AdjPrmtDtlsBS.getAdjParamDtlRow(Convert.ToInt32(strTMPPsetupdtlsID));
            //Concurrency Issue
            AdjustmentParameterDetailBE adjParmDtlBEold = (AdjParmDtlBElst.Where(o => o.prem_adj_pgm_dtl_id.Equals(Convert.ToInt32(strTMPPsetupdtlsID)))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmDtlBEold.UPDTE_DATE), Convert.ToDateTime(adjParmDtlBE.UPDTE_DATE));
            if (!con)
                return;
            //End
            if (adjParmDtlBE.st_id != Convert.ToInt32(ddTMStateitms.SelectedValue) ||
                adjParmDtlBE.adj_fctr_rt != Convert.ToDecimal(txtTMdtlsFctr.Text) ||
                 adjParmDtlBE.act_ind != (chkActindTM.Checked))
            {
                adjParmDtlBE.AccountID = AISMasterEntities.AccountNumber;
                adjParmDtlBE.PrgmPerodID = int.Parse(ViewState["PRGPRDID"].ToString());
                adjParmDtlBE.adj_paramet_id = Convert.ToInt32(hdnTMPrmsetuptxtBox.Text);
                if (adjParmDtlBE.st_id != Convert.ToInt32(ddTMStateitms.SelectedValue))
                {
                    //Get the AdjParameter WA-Tax details list for specific Account or Customer, Adjustmet Parameter Setupid and Program Period ID
                    IList<AdjustmentParameterDetailBE> AdjParmetDtlComparelstBE = AdjPrmtDtlsBS.getLBAAdjParamtrDtls(adjParmDtlBE.PrgmPerodID, adjParmDtlBE.adj_paramet_id, adjParmDtlBE.AccountID);

                    //check for each item in list if the values exists for the entered current state
                    //IF exists dont save the information
                    foreach (AdjustmentParameterDetailBE AdjCompareTMdetailBE in AdjParmetDtlComparelstBE)
                    {
                        if (AdjCompareTMdetailBE.st_id == Convert.ToInt32(ddTMStateitms.SelectedValue))
                        {
                            StateDetailsExists = true;
                            //lblcheckvalidations.Visible = true;
                            //lblcheckvalidations.Text = " * The details for the State you are trying to save already exists";
                            ShowMessage("The details for the State you are trying to save already exists");
                            break;
                        }
                    }
                }

                if (!StateDetailsExists)
                {
                    adjParmDtlBE.st_id = Convert.ToInt32(ddTMStateitms.SelectedValue);
                    adjParmDtlBE.adj_fctr_rt = Convert.ToDecimal(txtTMdtlsFctr.Text);
                    
                    if (txtTMPremAssmtAmt.Text != "")
                    {
                        adjParmDtlBE.PremAssementAmt = Convert.ToDecimal(txtTMPremAssmtAmt.Text);
                    }
                    else
                    {
                        adjParmDtlBE.PremAssementAmt = null;
                    }
                    if (chkActindTM.Checked == false)
                    {
                        adjParmDtlBE.act_ind = false;
                    }
                    else
                    {
                        adjParmDtlBE.act_ind = true;
                    }

                    adjParmDtlBE.UPDTE_USER_ID = CurrentAISUser.PersonID;
                    adjParmDtlBE.UPDTE_DATE = DateTime.Now;

                    bool SaveSuccess = AdjPrmtDtlsBS.Update(adjParmDtlBE);
                    ShowConcurrentConflict(SaveSuccess, adjParmDtlBE.ErrorMessage);
                    if (SaveSuccess)
                    {
                        AdjParameterTransactionWrapper.SubmitTransactionChanges();
                        //Code for logging into Audit Transaction Table 
                        ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                        audtBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
                        audtBS.Save(AISMasterEntities.AccountNumber, adjParmDtlBE.PrgmPerodID, GlobalConstants.AuditingWebPage.TaxMultiplierSetup, CurrentAISUser.PersonID);
                        AdjParameterTransactionWrapper.SubmitTransactionChanges();
                    }
                    else
                    {
                        AdjParameterTransactionWrapper.RollbackChanges();
                    }
                    this.lstTMSetuplistView.EditIndex = -1;
                    BindTMniformationDetails(adjParmDtlBE.PrgmPerodID, adjParmDtlBE.adj_paramet_id, AISMasterEntities.AccountNumber);

                }

            }
            else
            {
//                ShowMessage("No information has been changed to Save");
                lstTMSetuplistView.EditIndex = -1;
                int adjParmsetUpID = Convert.ToInt32(hdnTMPrmsetuptxtBox.Text);
                BindTMniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }
    #endregion

    #region RMLdetails

    /// <summary>
    /// Get RML Information details from database to fill the list view cotrol 
    /// passing 3 parameters AccountID, CustomerID and ProgramPeriodID
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstRMLParmsetupDtls_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        lstRMLSetuplistView.EditIndex = e.NewEditIndex;
        int adjParmsetUpID = Convert.ToInt32(hdnRMLPrmsetuptxtBox.Text);
        BindRMLniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);
    }

    /// <summary>
    /// Save RML-Information 1st grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRMLInfoDetailsSave_Click(object sender, EventArgs e)
    {
        try
        {
            int PolCountRML = 0;
            bool Flag = false;

            for (int PolRML = 0; PolRML < ChkBoxLstRMLpolicy.Items.Count; PolRML++)
            {
                if (ChkBoxLstRMLpolicy.Items[PolRML].Selected)
                {
                    PolCountRML = PolCountRML + 1;
                }
            }

            if (PolCountRML > 0)
            {
                if (CheckNewRML == false)
                {
                    adjParmStupBE = AdjPrmStupBS.getAdjParamRow(int.Parse(ViewState["AdjparmsetupforRML"].ToString()));
                    //Concurrency Issue
                    AdjustmentParameterSetupBE adjParmStupBEold = (Bindlist.Where(o => o.adj_paramet_setup_id.Equals(int.Parse(ViewState["AdjparmsetupforRML"].ToString())))).First();
                    bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmStupBEold.UPDATE_DATE), Convert.ToDateTime(adjParmStupBE.UPDATE_DATE));
                    if (!con)
                        return;
                    //End
                    if (CompareValues(adjParmStupBE, PolCountRML))
                    {
                        Flag = true;
                    }
                    else
                    {
                        Flag = false;
                    }
                }
//                if (!Flag)
//                {
                //To check Concurrency on Save for RML
                if (CheckNewRML == true)
                {
                    string AdjReviewResult = AdjPrmStupBS.getAdjParamResultOther(int.Parse(ViewState["PRGPRDID"].ToString()), AISMasterEntities.AccountNumber, Lbablaccess.GetLookUpID("RML", "ADJUSTMENT PARAMETER TYPE"));
                    if (AdjReviewResult == "true")
                    {
                        ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
                        return;
                    }
                }
                //End
                    adjParmStupBE = SaveRMLEntity(AdjParmStupBE);
                    BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
//                }
                //else
                //{
                //    ShowMessage("No information has been changed to Save");
                //}
                //this.lblcheckvalidations.Visible = false;
                //this.lblcheckvalidations.Text = "";
                //lnkBtnLBAadj.CommandArgument = adjParmStupBE.adj_paramet_setup_id.ToString();

            }
            else
            {
                //this.lblcheckvalidations.Visible = true;
                //this.lblcheckvalidations.Text = " * Please select at least one policy before saving RML information";
                ShowMessage("Please select at least one policy before saving RML information");
            }
        }
        catch (Exception ex)
        {
            ShowError((ex.Message.Contains("changed")) ? GlobalConstants.ErrorMessage.RowNotFoundOrChanged : GlobalConstants.ErrorMessage.ServerTooBusy, ex);
        }
    }

    /// <summary>
    /// Save method used to assign the values to a database field from the screen and save\update the details
    /// with Transaction processing for the first row in RML Tab
    /// </summary>
    /// <param name="AdjStupBE"></param>
    /// <returns></returns>
    protected AdjustmentParameterSetupBE SaveRMLEntity(AdjustmentParameterSetupBE AdjStupRMLBE)
    {

        AdjStupRMLBE.prem_adj_pgm_id = int.Parse(ViewState["PRGPRDID"].ToString());
        AdjStupRMLBE.Cstmr_Id = AISMasterEntities.AccountNumber;
        AdjStupRMLBE.AdjparameterTypeID = Lbablaccess.GetLookUpID("RML", "ADJUSTMENT PARAMETER TYPE");
        if (CheckNewRML == false)
        {
            AdjStupRMLBE.UPDATE_DATE = DateTime.Now;
            AdjStupRMLBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
        }
        else
        {
            AdjStupRMLBE.actv_ind = true;
            AdjStupRMLBE.CREATE_DATE = DateTime.Now;
            AdjStupRMLBE.CREATE_USER_ID = CurrentAISUser.PersonID;
        }

        //Included to avoid "NULL" value in this field
        AdjStupRMLBE.incld_ernd_retro_prem_ind = false;

        bool ResultRML = AdjPrmStupBS.Update(AdjStupRMLBE);
        if (ResultRML)
        {
            bool aptflag = AdjParameterTransactionWrapper.SubmitTransactionChanges();
            ShowConcurrentConflict(aptflag, AdjParameterTransactionWrapper.ErrorMessage);

            if (CheckNewRML == false)
            {
                //Code for logging into Audit Transaction Table 
                ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                audtBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
                audtBS.Save(AISMasterEntities.AccountNumber, AdjStupRMLBE.prem_adj_pgm_id, GlobalConstants.AuditingWebPage.RMLSetup, CurrentAISUser.PersonID);
            }

            AdjParamPolInfo.deletePol(AdjStupRMLBE.Cstmr_Id, AdjStupRMLBE.adj_paramet_setup_id);
            if (AdjStupRMLBE.adj_paramet_setup_id > 0)
            {
                for (int i = 0; i < ChkBoxLstRMLpolicy.Items.Count; i++)
                {
                    if (ChkBoxLstRMLpolicy.Items[i].Selected)
                    {
                        AdjustmentParameterPolicyBE RMLPolicylinkBE = new AdjustmentParameterPolicyBE();
                        RMLPolicylinkBE.adj_paramet_setup_id = AdjStupRMLBE.adj_paramet_setup_id;
                        RMLPolicylinkBE.custmrID = AISMasterEntities.AccountNumber;
                        RMLPolicylinkBE.PrmadjPRgmID = int.Parse(ViewState["PRGPRDID"].ToString());
                        RMLPolicylinkBE.coml_agmt_id = int.Parse(ChkBoxLstRMLpolicy.Items[i].Value);
                        RMLPolicylinkBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                        RMLPolicylinkBE.CREATE_DATE = DateTime.Now;
                        AdjParamPolInfo.Update(RMLPolicylinkBE);
                    }
                }
            }
            bool aptwflg = AdjParameterTransactionWrapper.SubmitTransactionChanges();
            ShowConcurrentConflict(aptwflg, AdjParameterTransactionWrapper.ErrorMessage);
        }
        else
        {
            AdjParameterTransactionWrapper.RollbackChanges();
        }
        return AdjStupRMLBE;
    }

    /// <summary>
    /// This function is used with above lstRMLParmsetupDtls_ItemCancel to perform the cancel operation
    /// </summary>
    protected void CancelUpdateModeRML()
    {
        lstRMLSetuplistView.InsertItemPosition = InsertItemPosition.None;
        int adjParmsetUpID = Convert.ToInt32(hdnRMLPrmsetuptxtBox.Text);
        BindRMLniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);
    }

    /// <summary>
    /// Show RML information in listview(Setup details) 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRMLInfoDetailsInfo_Click(object sender, EventArgs e)
    {
        if (ViewState["AdjparmsetupforRML"] != null)
        {
            //Close LBA listview in LBA TAB
            //this.pnlpp.Enabled = true;
            this.pnlAdlLBA.Enabled = true;
            this.lbasetupdetailslistview.Visible = false;
            this.LBASetupDetailsLabel.Visible = false;
            this.lnkbtnLBADetailsClose.Visible = false;

            //Close LCF listview in LCF TAB
            //this.pnlppLCF.Enabled = true;
            this.pnlAdjLCF.Enabled = true;
            this.lstLCFSetuplistView.Visible = false;
            this.lblLCFdtls.Visible = false;
            this.lnkLCFClose.Visible = false;

            //Close WA-AX listview in TaxMultiplier TAB
            //this.pnlppTM.Enabled = true;
            this.pnlAdjTM.Enabled = true;
            this.lstTMSetuplistView.Visible = false;
            this.lblTMdtls.Visible = false;
            this.lnkBtnCloseTMdtlsID.Visible = false;

            //Close CHF listview in CHF TAB
            //this.pnlppCHF.Enabled = true;
            this.pnlADlCHF.Enabled = true;
            this.chfsetupdetailslistview.Visible = false;
            this.CHFSetupDetailsLabel.Visible = false;
            this.lnkbtnCHFDetailsClose.Visible = false;

            if (this.lblRMLdtls.Visible == false && this.lnkBtnCloseRMLdtlsID.Visible == false)
            {
                this.lblRMLdtls.Visible = true;
                this.lnkBtnCloseRMLdtlsID.Visible = true;
            }
            adjParmDtlBE = null;
            //this.pnlppRML.Enabled = false;
            //this.lblcheckvalidations.Visible = false;
            hdnRMLPrmsetuptxtBox.Text = ViewState["AdjparmsetupforRML"].ToString();
            this.lstRMLSetuplistView.EditIndex = -1;
            BindRMLniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), int.Parse(ViewState["AdjparmsetupforRML"].ToString()), AISMasterEntities.AccountNumber);
            this.pnlAdjRML.Enabled = false;
        }
        else
        {
            ShowMessage("Please save RML Policy information before selecting the Detail information");
        }
    }

    /// <summary>
    /// Bind the listview for RML details 
    /// </summary>
    /// <param name="prgmperiodID"></param>
    /// <param name="AdjPrmtSetupID"></param>
    /// <param name="CustomerID"></param>
    public void BindRMLniformationDetails(int prgmperiodID, int AdjPrmtSetupID, int CustomerID)
    {
        //lblcheckvalidations.Visible = false;
        //lblcheckvalidations.Text = "";
        if (this.lblRMLdtls.Visible == false && this.lnkBtnCloseRMLdtlsID.Visible == false)
        {
            this.lblRMLdtls.Visible = true;
            this.lnkBtnCloseRMLdtlsID.Visible = true;
        }
        Adj_paramet_DtlBS AdjRMLprmDtlBS = new Adj_paramet_DtlBS();
        //IList<AdjustmentParameterDetailBE> AdjParmDtlBElst = AdjRMLprmDtlBS.getLBAAdjParamtrDtls(prgmperiodID, AdjPrmtSetupID, CustomerID);
        AdjParmRMLDtlBElst = AdjRMLprmDtlBS.getLBAAdjParamtrDtls(prgmperiodID, AdjPrmtSetupID, CustomerID);
        this.lstRMLSetuplistView.DataSource = AdjParmRMLDtlBElst;
        this.lstRMLSetuplistView.DataBind();
        this.lstRMLSetuplistView.Visible = true;
    }

    /// <summary>
    /// Close the RML List view control along with its label and this close button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRMLInfoClose_Click(object sender, EventArgs e)
    {
        //this.pnlppRML.Enabled = true;
        this.pnlAdjRML.Enabled = true;
        this.lstRMLSetuplistView.Visible = false;
        this.lblRMLdtls.Visible = false;
        this.lnkBtnCloseRMLdtlsID.Visible = false;
    }

    /// <summary>
    /// This function is not used now
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRMLsetupinfodetails_click(object sender, EventArgs e)
    { }

    /// <summary>
    /// Disable\Enable First Row for RML details table
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DisableRMLRow(object sender, CommandEventArgs e)
    {
        try
        {
            if (ViewState["AdjparmsetupforRML"] != null)
            {
                int AdjParmtSetupID = int.Parse(ViewState["AdjparmsetupforRML"].ToString());
                //LBAParmBE = AdjparamsetupInfo.getAdjParamRow(AdjParmtSetupID);
                adjParmStupBE = AdjPrmStupBS.getAdjParamRow(AdjParmtSetupID);
                //Concurrency Issue
                AdjustmentParameterSetupBE adjParmStupBEold = (Bindlist.Where(o => o.adj_paramet_setup_id.Equals(AdjParmtSetupID))).First();
                bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmStupBEold.UPDATE_DATE), Convert.ToDateTime(adjParmStupBE.UPDATE_DATE));
                if (!con)
                    return;
                //End
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
                bool ResultRML = AdjPrmStupBS.Update(adjParmStupBE);
                if (ResultRML)
                {
                    AdjParameterTransactionWrapper.SubmitTransactionChanges();
                    //Code for logging into Audit Transaction Table 
                    ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                    audtBS.Save(AISMasterEntities.AccountNumber, adjParmStupBE.prem_adj_pgm_id, GlobalConstants.AuditingWebPage.RMLSetup, CurrentAISUser.PersonID);
                    AdjParameterTransactionWrapper.SubmitTransactionChanges();
                }
                else
                {
                    AdjParameterTransactionWrapper.RollbackChanges();
                }
                BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// Disable\Enable RML information details for a state
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DisableRMLParmdtlsRow(ListViewItem e, int AdjparmetDtlID, bool Flag)
    {
        try
        {
            adjParmDtlBE = AdjPrmtDtlsBS.getAdjParamDtlRow(AdjparmetDtlID);
            //Concurrency Issue
            AdjustmentParameterDetailBE adjParmDtlBEold = (AdjParmRMLDtlBElst.Where(o => o.prem_adj_pgm_dtl_id.Equals(AdjparmetDtlID))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmDtlBEold.UPDTE_DATE), Convert.ToDateTime(adjParmDtlBE.UPDTE_DATE));
            if (!con)
                return;
            //End
            adjParmDtlBE.act_ind = Flag;
            adjParmDtlBE.UPDTE_USER_ID = CurrentAISUser.PersonID;
            adjParmDtlBE.UPDTE_DATE = DateTime.Now;
            Flag = AdjPrmtDtlsBS.Update(adjParmDtlBE);
            if (Flag)
            {
                AdjParameterTransactionWrapper.SubmitTransactionChanges();
                //Code for logging into Audit Transaction Table 
                ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                audtBS.Save(AISMasterEntities.AccountNumber, adjParmDtlBE.PrgmPerodID, GlobalConstants.AuditingWebPage.RMLSetup, CurrentAISUser.PersonID);
                AdjParameterTransactionWrapper.SubmitTransactionChanges();
            }
            else
            {
                AdjParameterTransactionWrapper.RollbackChanges();
            }
            int Adjparmsetupiid = Convert.ToInt32(hdnRMLPrmsetuptxtBox.Text);
            BindRMLniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), Adjparmsetupiid, AISMasterEntities.AccountNumber);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// This method is used to check if the user is updating or saving list view details and also
    /// calling those update and save methods, Also used to unable and Disable the rows in list view
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstRMLParmsetupDtls_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "Save")
        {
            lstRMLRelatedinfo_Saving(e.Item);
        }
        else if (e.CommandName == "Update")
        {
            lstRMLParmsetupDtls_ItemUpdate(e.Item);

        }
        else if (e.CommandName.ToUpper() == "DISABLE")
        {
            DisableRMLParmdtlsRow(e.Item, Convert.ToInt32(e.CommandArgument), false);
        }
        else if (e.CommandName.ToUpper() == "ENABLE")
        {
            DisableRMLParmdtlsRow(e.Item, Convert.ToInt32(e.CommandArgument), true);
        }
    }

    /// <summary>
    /// This method is not used now was written initially while development
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstRMLParmsetupDtls_ItemUpdating(Object sender, ListViewUpdateEventArgs e)
    { }

    /// <summary>
    /// Cancel any changes if user had selected any items in list view for updation
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstRMLParmsetupDtls_ItemCancel(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstRMLSetuplistView.EditIndex = -1;
            int adjParmsetUpID = Convert.ToInt32(hdnRMLPrmsetuptxtBox.Text);
            BindRMLniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);
        }
        else if (e.CancelMode == ListViewCancelMode.CancelingInsert)
        {
            CancelUpdateModeRML(); //Back to normal mode.
        }
    }

    /// <summary>
    /// Save RML information details like state, Factor, Deposit Amount and Comments
    /// State and Factor are compulsory fields
    /// </summary>
    /// <param name="e"></param>
    protected void lstRMLRelatedinfo_Saving(ListViewItem e)
    {
        try
        {
            bool StateDetailsExists = false;
            //lblcheckvalidations.Text = "";
            //lblcheckvalidations.Visible = false;
            AdjustmentParameterDetailBE RMLInfoDetailsBE = new AdjustmentParameterDetailBE();
            Adj_paramet_DtlBS RMLdtlsBS = new Adj_paramet_DtlBS();
            DropDownList ddRMLStateitms = (DropDownList)e.FindControl("ddlRMLState");
            DropDownList ddRMLLOBitems = (DropDownList)e.FindControl("ddRMLLOB");
            TextBox txtRMLdtlsFctr = (TextBox)e.FindControl("txtRMLFactor");
            TextBox txtRMLFnlAmt = (TextBox)e.FindControl("txtRMLfinalAmount");
            TextBox txtRMLSetupComments = (TextBox)e.FindControl("RMLSetupComments");


            RMLInfoDetailsBE.AccountID = AISMasterEntities.AccountNumber;
            RMLInfoDetailsBE.PrgmPerodID = int.Parse(ViewState["PRGPRDID"].ToString());
            RMLInfoDetailsBE.adj_paramet_id = Convert.ToInt32(hdnRMLPrmsetuptxtBox.Text);
            //Get the AdjParameter WA-TAX details list for specific Account or Customer, Adjustmet Parameter Setupid and Program Period ID
            IList<AdjustmentParameterDetailBE> AdjParmetDtlComparelstBE = AdjParmDtlsBS.getLBAAdjParamtrDtls(RMLInfoDetailsBE.PrgmPerodID, RMLInfoDetailsBE.adj_paramet_id, RMLInfoDetailsBE.AccountID);

            //check for each item in list if the values exists for the entered current state
            //IF exists dont save the information
            foreach (AdjustmentParameterDetailBE AdjCompareRMLdetailBE in AdjParmetDtlComparelstBE)
            {
                if (AdjCompareRMLdetailBE.st_id == Convert.ToInt32(ddRMLStateitms.SelectedValue) &&
                    AdjCompareRMLdetailBE.ln_of_bsn_id == Convert.ToInt32(ddRMLLOBitems.SelectedValue))
                {
                    StateDetailsExists = true;
                    //lblcheckvalidations.Visible = true;
                    //lblcheckvalidations.Text = " * The details for the State and Line of Business you are trying to save already exists";
                    ShowMessage("The details for the State and Line of Business you are trying to save already exists");
                    break;
                }

            }
            if (!StateDetailsExists)
            {
                RMLInfoDetailsBE.st_id = Convert.ToInt32(ddRMLStateitms.SelectedValue);
                RMLInfoDetailsBE.ln_of_bsn_id = Convert.ToInt32(ddRMLLOBitems.SelectedValue);
                RMLInfoDetailsBE.adj_fctr_rt = Convert.ToDecimal(txtRMLdtlsFctr.Text);
                if (txtRMLFnlAmt.Text != "")
                {
                    RMLInfoDetailsBE.fnl_overrid_amt = Convert.ToDecimal(txtRMLFnlAmt.Text.Replace(",", ""));
                }
                RMLInfoDetailsBE.cmmnt_txt = txtRMLSetupComments.Text;

                RMLInfoDetailsBE.CRTE_DATE = DateTime.Now;
                RMLInfoDetailsBE.UPDTE_DATE = (Nullable<DateTime>)null;
                RMLInfoDetailsBE.act_ind = true;
                RMLInfoDetailsBE.CRTE_USER_ID = CurrentAISUser.PersonID;
                RMLInfoDetailsBE.UPDTE_USER_ID = (Nullable<int>)null;
                RMLdtlsBS.Update(RMLInfoDetailsBE);
                BindRMLniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), RMLInfoDetailsBE.adj_paramet_id, AISMasterEntities.AccountNumber);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// The method is used after the RML details info listview control is bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstRMLParmsetupDtls_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            Label txtFactorRML = (Label)e.Item.FindControl("lblRMLfactor");
            TextBox textRMLboxedit = (TextBox)e.Item.FindControl("txtRMLFactor");
            DropDownList ddRMLStateitms = (DropDownList)e.Item.FindControl("ddlRMLState");
            DropDownList ddRMLLOBitms = (DropDownList)e.Item.FindControl("ddRMLLOB");
            Label hdRMLState = (Label)e.Item.FindControl("hidRMLState");
            Label hdRMLLOB = (Label)e.Item.FindControl("hidRMLLOB");

            if ((ddRMLStateitms != null) && (hdRMLState != null))
            {
//                ddRMLStateitms.SelectedIndex = ddRMLStateitms.Items.IndexOf(ddRMLStateitms.Items.FindByText(hdRMLState.Text.ToString()));
                AddInActiveLookupDataByText(ref ddRMLStateitms, hdRMLState.Text);
            }
            if ((ddRMLLOBitms != null) && (hdRMLLOB != null))
            {
//                ddRMLLOBitms.SelectedIndex = ddRMLLOBitms.Items.IndexOf(ddRMLLOBitms.Items.FindByText(hdRMLLOB.Text.ToString()));
                AddInActiveLookupDataByText(ref ddRMLLOBitms, hdRMLLOB.Text);
            }

            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDisableEnableRML");

            if (imgDelete != null)
            {
                HiddenField hid = (HiddenField)e.Item.FindControl("hidActiveRML");

                if (hid.Value == "True")
                {
                    LinkButton lbRMLEDit = (LinkButton)e.Item.FindControl("lnkbtnRMLdetailsetupEdit");
                    if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                        lbRMLEDit.Enabled = true;
                    imgDelete.CommandName = "Disable";
                    imgDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable this record?');");
                }
                else
                {
                    LinkButton lbRMLEDitAlt = (LinkButton)e.Item.FindControl("lnkbtnRMLdetailsetupEdit");
                    lbRMLEDitAlt.Enabled = false;
                    imgDelete.CommandName = "Enable";
                    imgDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable this record?');");
                }
            }
            if (txtFactorRML != null)
            {
                if (txtFactorRML.Text != "") txtFactorRML.Text = Convert.ToDecimal(txtFactorRML.Text).ToString("0.000000");
            }
            if (textRMLboxedit != null)
            {
                if (textRMLboxedit.Text != "") textRMLboxedit.Text = Convert.ToDecimal(textRMLboxedit.Text).ToString("0.000000");
            }
        }
    }

    /// <summary>
    /// Method used to update\Edit a selected row in RML details list view
    /// </summary>
    /// <param name="e"></param>
    protected void lstRMLParmsetupDtls_ItemUpdate(ListViewItem e)
    {
        try
        {
            //lblcheckvalidations.Visible = false;
            //lblcheckvalidations.Text = "";
            bool StateDetailsExists = false;
            //AdjustmentParameterDetailBE RMLInfoUpdtDetailsBE = new AdjustmentParameterDetailBE();

            Label lblPPSetupDtlsRML = (Label)e.FindControl("lblAdjParmtdtlRMLID");
            string strRMLPPsetupdtlsID = lblPPSetupDtlsRML.Text;
            DropDownList ddRMLStateitms = (DropDownList)e.FindControl("ddlRMLState");
            DropDownList ddRMLLOBitems = (DropDownList)e.FindControl("ddRMLLOB");
            TextBox txtRMLdtlsFctr = (TextBox)e.FindControl("txtRMLFactor");
            TextBox txtRMLFnlAmt = (TextBox)e.FindControl("txtRMLfinalAmount");
            TextBox txtRMLSetupComments = (TextBox)e.FindControl("RMLSetupComments");
            CheckBox chkActind = (CheckBox)e.FindControl("chkActiveRML");

            adjParmDtlBE = AdjPrmtDtlsBS.getAdjParamDtlRow(Convert.ToInt32(strRMLPPsetupdtlsID));
            //Concurrency Issue
            AdjustmentParameterDetailBE adjParmDtlBEold = (AdjParmRMLDtlBElst.Where(o => o.prem_adj_pgm_dtl_id.Equals(Convert.ToInt32(strRMLPPsetupdtlsID)))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmDtlBEold.UPDTE_DATE), Convert.ToDateTime(adjParmDtlBE.UPDTE_DATE));
            if (!con)
                return;
            //End
            //if (adjParmDtlBE.fnl_overrid_amt == null)
            //{
            //    adjParmDtlBE.fnl_overrid_amt = 0;
            //}
            if (adjParmDtlBE.st_id != Convert.ToInt32(ddRMLStateitms.SelectedValue) ||
                adjParmDtlBE.adj_fctr_rt != Convert.ToDecimal(txtRMLdtlsFctr.Text) ||
                adjParmDtlBE.ln_of_bsn_id != Convert.ToInt32(ddRMLLOBitems.SelectedValue) ||
                adjParmDtlBE.cmmnt_txt != txtRMLSetupComments.Text ||
                adjParmDtlBE.fnl_overrid_amt != (txtRMLFnlAmt.Text == "" ? 0 : Convert.ToDecimal(txtRMLFnlAmt.Text)) ||
               adjParmDtlBE.act_ind != (chkActind.Checked))
            {
                adjParmDtlBE.AccountID = AISMasterEntities.AccountNumber;
                adjParmDtlBE.PrgmPerodID = int.Parse(ViewState["PRGPRDID"].ToString());
                adjParmDtlBE.adj_paramet_id = Convert.ToInt32(hdnRMLPrmsetuptxtBox.Text);

                if (adjParmDtlBE.st_id != Convert.ToInt32(ddRMLStateitms.SelectedValue) ||
                    adjParmDtlBE.ln_of_bsn_id != Convert.ToInt32(ddRMLLOBitems.SelectedValue))
                {
                    //Get the AdjParameter WA-Tax details list for specific Account or Customer, Adjustmet Parameter Setupid and Program Period ID
                    IList<AdjustmentParameterDetailBE> AdjParmetDtlComparelstBE = AdjPrmtDtlsBS.getLBAAdjParamtrDtls(adjParmDtlBE.PrgmPerodID, adjParmDtlBE.adj_paramet_id, adjParmDtlBE.AccountID);

                    //check for each item in list if the values exists for the entered current state
                    //IF exists dont save the information
                    foreach (AdjustmentParameterDetailBE AdjCompareRMLdetailBE in AdjParmetDtlComparelstBE)
                    {
                        if (AdjCompareRMLdetailBE.st_id == Convert.ToInt32(ddRMLStateitms.SelectedValue) &&
                            AdjCompareRMLdetailBE.ln_of_bsn_id == Convert.ToInt32(ddRMLLOBitems.SelectedValue))
                        {
                            StateDetailsExists = true;
                            //lblcheckvalidations.Visible = true;
                            //lblcheckvalidations.Text = " * The details for the State you are trying to save already exists";
                            ShowMessage("The details for the State and Line of business you are trying to save already exists");
                            break;
                        }
                    }
                }
                if (!StateDetailsExists)
                {
                    adjParmDtlBE.st_id = Convert.ToInt32(ddRMLStateitms.SelectedValue);
                    adjParmDtlBE.ln_of_bsn_id = Convert.ToInt32(ddRMLLOBitems.SelectedValue);
                    adjParmDtlBE.adj_fctr_rt = Convert.ToDecimal(txtRMLdtlsFctr.Text);
                    if (txtRMLFnlAmt.Text != "")
                    {
                        adjParmDtlBE.fnl_overrid_amt = Convert.ToDecimal(txtRMLFnlAmt.Text);
                    }
                    else
                    {
                        adjParmDtlBE.fnl_overrid_amt = null;
                    }

                    adjParmDtlBE.cmmnt_txt = txtRMLSetupComments.Text;

                    if (chkActind.Checked == false)
                    {
                        adjParmDtlBE.act_ind = false;
                    }
                    else
                    {
                        adjParmDtlBE.act_ind = true;
                    }

                    adjParmDtlBE.UPDTE_USER_ID = CurrentAISUser.PersonID;
                    adjParmDtlBE.UPDTE_DATE = DateTime.Now;

                    bool SaveSuccess = AdjPrmtDtlsBS.Update(adjParmDtlBE);
                    ShowConcurrentConflict(SaveSuccess, adjParmDtlBE.ErrorMessage);
                    if (SaveSuccess)
                    {
                        AdjParameterTransactionWrapper.SubmitTransactionChanges();
                        //Code for logging into Audit Transaction Table 
                        ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                        audtBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
                        audtBS.Save(AISMasterEntities.AccountNumber, adjParmDtlBE.PrgmPerodID, GlobalConstants.AuditingWebPage.RMLSetup, CurrentAISUser.PersonID);
                        AdjParameterTransactionWrapper.SubmitTransactionChanges();
                    }
                    else
                    {
                        AdjParameterTransactionWrapper.RollbackChanges();
                    }
                    this.lstRMLSetuplistView.EditIndex = -1;
                    BindRMLniformationDetails(adjParmDtlBE.PrgmPerodID, adjParmDtlBE.adj_paramet_id, AISMasterEntities.AccountNumber);

                }

            }
            else
            {
//                ShowMessage("No information has been changed to Save");
                lstRMLSetuplistView.EditIndex = -1;
                int adjParmsetUpID = Convert.ToInt32(hdnRMLPrmsetuptxtBox.Text);
                BindRMLniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    protected void lstRMLParmsetupDtls_Sorting(object sender, ListViewSortEventArgs e)
    {
        ImageButton imgRMLbtn = new ImageButton();
        imgRMLbtn = (ImageButton)lstRMLSetuplistView.FindControl("imgRMLStateSort");

        e.SortDirection = (imgRMLbtn.ImageUrl.Contains("Des")) ? SortDirection.Ascending : SortDirection.Descending;

        if (e.SortDirection == SortDirection.Ascending)
            AdjParmRMLDtlBElst = AdjParmRMLDtlBElst.OrderBy(sl => sl.PrgParameterStateName).ToList();
        else
            AdjParmRMLDtlBElst = AdjParmRMLDtlBElst.OrderByDescending(sl => sl.PrgParameterStateName).ToList();
        ChangeImage(imgRMLbtn, e.SortDirection);

        lstRMLSetuplistView.DataSource = AdjParmRMLDtlBElst;
        lstRMLSetuplistView.DataBind();
    }

    #endregion

    #region CHFDetails


    /// <summary>
    /// Save method used to assign the values to a database field from the screen and save\update the details
    /// with Transaction processing for the first row in CHF Tab
    /// </summary>
    /// <param name="AdjStupBE"></param>
    /// <returns></returns>
    protected AdjustmentParameterSetupBE SaveEntityincCHF(AdjustmentParameterSetupBE AdjStupCHFBE)
    {

        AdjStupCHFBE.prem_adj_pgm_id = int.Parse(ViewState["PRGPRDID"].ToString());
        if (txtCHFDeposit.Text != "")
        {
            AdjStupCHFBE.depst_amt = decimal.Parse(txtCHFDeposit.Text.Replace(",", ""));
        }
        else
        {
            AdjStupCHFBE.depst_amt = 0;
        }
        AdjStupCHFBE.clm_hndl_fee_basis_id = Convert.ToInt32(ddCHFBasisCharged.SelectedValue);
        AdjStupCHFBE.AdjparameterTypeID = Lbablaccess.GetLookUpID("CHF", "ADJUSTMENT PARAMETER TYPE");
        if (rbtnCHFinclude.Checked == true)
        {
            AdjStupCHFBE.incld_ernd_retro_prem_ind = true;
        }
        else
        {
            AdjStupCHFBE.incld_ernd_retro_prem_ind = false;

        }
        AdjStupCHFBE.Cstmr_Id = AISMasterEntities.AccountNumber;


        if (CheckNewincCHF == false)
        {
            AdjStupCHFBE.UPDATE_DATE = DateTime.Now;
            AdjStupCHFBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
        }
        else
        {
            AdjStupCHFBE.actv_ind = true;
            AdjStupCHFBE.CREATE_DATE = DateTime.Now;
            AdjStupCHFBE.CREATE_USER_ID = CurrentAISUser.PersonID;
        }

        bool ResultCHF = AdjPrmStupBS.Update(AdjStupCHFBE);
        if (ResultCHF)
        {
            bool aptwFlag = AdjParameterTransactionWrapper.SubmitTransactionChanges();
            ShowConcurrentConflict(aptwFlag, AdjParameterTransactionWrapper.ErrorMessage);

            if (CheckNewincLBA == false)
            {

                //Code for logging into Audit Transaction Table 
                ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                audtBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
                audtBS.Save(AISMasterEntities.AccountNumber, AdjStupCHFBE.prem_adj_pgm_id, GlobalConstants.AuditingWebPage.CHFSetup, CurrentAISUser.PersonID);

            }

            AdjParamPolInfo.deletePol(AdjStupCHFBE.Cstmr_Id, AdjStupCHFBE.adj_paramet_setup_id);
            if (AdjStupCHFBE.adj_paramet_setup_id > 0)
            {
                for (int i = 0; i < chkbxCHFPolicynolst.Items.Count; i++)
                {
                    if (chkbxCHFPolicynolst.Items[i].Selected)
                    {
                        AdjustmentParameterPolicyBE LBAPolicylinkBE = new AdjustmentParameterPolicyBE();
                        LBAPolicylinkBE.adj_paramet_setup_id = AdjStupCHFBE.adj_paramet_setup_id;
                        LBAPolicylinkBE.custmrID = AISMasterEntities.AccountNumber;
                        LBAPolicylinkBE.PrmadjPRgmID = int.Parse(ViewState["PRGPRDID"].ToString());
                        LBAPolicylinkBE.coml_agmt_id = int.Parse(chkbxCHFPolicynolst.Items[i].Value);
                        LBAPolicylinkBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                        LBAPolicylinkBE.CREATE_DATE = DateTime.Now;
                        AdjParamPolInfo.Update(LBAPolicylinkBE);
                    }
                }
            }

            if (CheckNewNotincCHF == false)
            {
                AdjustmentParameterSetupBE AdjparmCHFsetupBE = new AdjustmentParameterSetupBE();
                AdjparmCHFsetupBE = AdjparamsetupInfo.getAdjParamRow(int.Parse(ViewState["ADJCHFnonPRGID"].ToString()));
                AdjParamPolInfo.deletePol(AdjparmCHFsetupBE.Cstmr_Id, AdjparmCHFsetupBE.adj_paramet_setup_id);
                if (AdjparmCHFsetupBE.adj_paramet_setup_id > 0)
                {
                    //int polCount4 = 0;
                    for (int i = 0; i < chkbxCHF2Policynolst.Items.Count; i++)
                    {
                        if (chkbxCHF2Policynolst.Items[i].Selected)
                        {
                            // ++polCount4;
                            AdjustmentParameterPolicyBE LBAPolicylinkBE = new AdjustmentParameterPolicyBE();
                            LBAPolicylinkBE.adj_paramet_setup_id = AdjparmCHFsetupBE.adj_paramet_setup_id;
                            LBAPolicylinkBE.custmrID = AISMasterEntities.AccountNumber;
                            LBAPolicylinkBE.PrmadjPRgmID = int.Parse(ViewState["PRGPRDID"].ToString());
                            LBAPolicylinkBE.coml_agmt_id = int.Parse(chkbxCHF2Policynolst.Items[i].Value);
                            LBAPolicylinkBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                            LBAPolicylinkBE.CREATE_DATE = DateTime.Now;
                            AdjParamPolInfo.Update(LBAPolicylinkBE);
                        }
                    }
                    //if (polCount4 == 0)
                    //{
                    //    AdjParmDtlsBS.deleteAdjParamtrDtls(AdjparmCHFsetupBE.prem_adj_pgm_id, AdjparmCHFsetupBE.adj_paramet_setup_id, AdjparmCHFsetupBE.Cstmr_Id);
                    //    lnkbtnCHF2infoDetails.Enabled = false;
                    //}
                }

            }
            bool aptflg = AdjParameterTransactionWrapper.SubmitTransactionChanges();
            ShowConcurrentConflict(aptflg, AdjParameterTransactionWrapper.ErrorMessage);
        }
        else
        {
            AdjParameterTransactionWrapper.RollbackChanges();
        }
        return AdjStupCHFBE;

    }

    /// <summary>
    /// Save method used to assign the values to a database field from the screen and save\update the details
    /// with Transaction processing for the second row in CHF Tab
    /// </summary>
    /// <param name="AdjStupBE"></param>
    /// <returns></returns>
    protected AdjustmentParameterSetupBE SaveEntityNotincCHF(AdjustmentParameterSetupBE AdjStupCHFBE)
    {
        AdjStupCHFBE.prem_adj_pgm_id = int.Parse(ViewState["PRGPRDID"].ToString());
        if (txtchfDeposit2.Text != "")
        {
            AdjStupCHFBE.depst_amt = decimal.Parse(txtchfDeposit2.Text.Replace(",", ""));
        }
        else
        {
            AdjStupCHFBE.depst_amt = 0;
        }
        AdjStupCHFBE.clm_hndl_fee_basis_id = Convert.ToInt32(ddCHFBasisCharged2.SelectedValue);
        AdjStupCHFBE.AdjparameterTypeID = Lbablaccess.GetLookUpID("CHF", "ADJUSTMENT PARAMETER TYPE");
        if (rbtnCHFNotinclude.Checked == true)
        {
            AdjStupCHFBE.incld_ernd_retro_prem_ind = true;
        }
        else
        {
            AdjStupCHFBE.incld_ernd_retro_prem_ind = false;

        }
        AdjStupCHFBE.Cstmr_Id = AISMasterEntities.AccountNumber;

        if (CheckNewNotincCHF == false)
        {
            AdjStupCHFBE.UPDATE_DATE = DateTime.Now;
            AdjStupCHFBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
        }
        else
        {
            AdjStupCHFBE.actv_ind = true;
            AdjStupCHFBE.CREATE_DATE = DateTime.Now;
            AdjStupCHFBE.CREATE_USER_ID = CurrentAISUser.PersonID;
        }

        bool ResultCHF = AdjPrmStupBS.Update(AdjStupCHFBE);
        if (ResultCHF)
        {
            bool aptflg = AdjParameterTransactionWrapper.SubmitTransactionChanges();
            ShowConcurrentConflict(aptflg, AdjParameterTransactionWrapper.ErrorMessage);

            if (CheckNewincLBA == false)
            {

                //Code for logging into Audit Transaction Table 
                ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                audtBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
                audtBS.Save(AISMasterEntities.AccountNumber, AdjStupCHFBE.prem_adj_pgm_id, GlobalConstants.AuditingWebPage.CHFSetup, CurrentAISUser.PersonID);

            }
            AdjParamPolInfo.deletePol(AdjStupCHFBE.Cstmr_Id, AdjStupCHFBE.adj_paramet_setup_id);
            if (AdjStupCHFBE.adj_paramet_setup_id > 0)
            {
                for (int i = 0; i < chkbxCHF2Policynolst.Items.Count; i++)
                {
                    if (chkbxCHF2Policynolst.Items[i].Selected)
                    {
                        AdjustmentParameterPolicyBE LBAPolicylinkBE = new AdjustmentParameterPolicyBE();
                        LBAPolicylinkBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                        LBAPolicylinkBE.CREATE_DATE = DateTime.Now;
                        LBAPolicylinkBE.adj_paramet_setup_id = AdjStupCHFBE.adj_paramet_setup_id;
                        LBAPolicylinkBE.custmrID = AISMasterEntities.AccountNumber;
                        LBAPolicylinkBE.PrmadjPRgmID = int.Parse(ViewState["PRGPRDID"].ToString());
                        LBAPolicylinkBE.coml_agmt_id = int.Parse(chkbxCHF2Policynolst.Items[i].Value);
                        AdjParamPolInfo.Update(LBAPolicylinkBE);
                    }
                }
            }


            if (CheckNewincCHF == false)
            {
                AdjustmentParameterSetupBE AdjparmCHFsetupBE = new AdjustmentParameterSetupBE();
                AdjparmCHFsetupBE = AdjparamsetupInfo.getAdjParamRow(int.Parse(ViewState["ADJCHFPRGID"].ToString()));
                AdjParamPolInfo.deletePol(AdjparmCHFsetupBE.Cstmr_Id, AdjparmCHFsetupBE.adj_paramet_setup_id);
                if (AdjparmCHFsetupBE.adj_paramet_setup_id > 0)
                {
                    //int polCount3 = 0;
                    for (int i = 0; i < chkbxCHFPolicynolst.Items.Count; i++)
                    {
                        if (chkbxCHFPolicynolst.Items[i].Selected)
                        {
                            //++polCount3;
                            AdjustmentParameterPolicyBE LBAPolicylinkBE = new AdjustmentParameterPolicyBE();
                            LBAPolicylinkBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                            LBAPolicylinkBE.CREATE_DATE = DateTime.Now;
                            LBAPolicylinkBE.adj_paramet_setup_id = AdjparmCHFsetupBE.adj_paramet_setup_id;
                            LBAPolicylinkBE.custmrID = AISMasterEntities.AccountNumber;
                            LBAPolicylinkBE.PrmadjPRgmID = int.Parse(ViewState["PRGPRDID"].ToString());
                            LBAPolicylinkBE.coml_agmt_id = int.Parse(chkbxCHFPolicynolst.Items[i].Value);
                            AdjParamPolInfo.Update(LBAPolicylinkBE);

                        }
                    }
                    //if (polCount3 == 0)
                    //{
                    //    AdjParmDtlsBS.deleteAdjParamtrDtls(AdjparmCHFsetupBE.prem_adj_pgm_id, AdjparmCHFsetupBE.adj_paramet_setup_id, AdjparmCHFsetupBE.Cstmr_Id);
                    //    lnkbtnCHFinfoDetails.Enabled = false;
                    //}
                }

            }
            bool aptwflag = AdjParameterTransactionWrapper.SubmitTransactionChanges();
            ShowConcurrentConflict(aptwflag, AdjParameterTransactionWrapper.ErrorMessage);
        }
        else
        {
            AdjParameterTransactionWrapper.RollbackChanges();
        }
        return AdjStupCHFBE;
    }

    /// <summary>
    /// Save CHF-Information 1st grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCHFInfoDetailsSave_Click(object sender, EventArgs e)
    {
        try
        {
            int PolCountCHF = 0;
            bool Flag = false;

            for (int PolCHF = 0; PolCHF < chkbxCHFPolicynolst.Items.Count; PolCHF++)
            {
                if (chkbxCHFPolicynolst.Items[PolCHF].Selected)
                {
                    PolCountCHF = PolCountCHF + 1;
                }
            }

            if (PolCountCHF > 0)
            {


                if (CheckNewincCHF == false)
                {
                    //adjParmStupBE = Bindlist.Single(bl => bl.adj_paramet_setup_id == int.Parse(ViewState["ADJLBAPRGID"].ToString()));
                    adjParmStupBE = AdjPrmStupBS.getAdjParamRow(int.Parse(ViewState["ADJCHFPRGID"].ToString()));
                    //Concurrency Issue
                    AdjustmentParameterSetupBE adjParmStupBEold = (Bindlist.Where(o => o.adj_paramet_setup_id.Equals(int.Parse(ViewState["ADJCHFPRGID"].ToString())))).First();
                    bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmStupBEold.UPDATE_DATE), Convert.ToDateTime(adjParmStupBE.UPDATE_DATE));
                    if (!con)
                        return;
                    //End
                    if (CompareValues(adjParmStupBE, PolCountCHF))
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
                    //To check Concurrency on Save for CHF
                    if (CheckNewincCHF == true)
                    {
                        bool strIncluded = true;
                        string AdjReviewResultLBA = AdjPrmStupBS.getAdjParamResult(int.Parse(ViewState["PRGPRDID"].ToString()), AISMasterEntities.AccountNumber, Lbablaccess.GetLookUpID("CHF", "ADJUSTMENT PARAMETER TYPE"), strIncluded);
                        if (AdjReviewResultLBA == "true")
                        {
                            ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
                            return;
                        }
                    }
                    //End
                    adjParmStupBE = SaveEntityincCHF(AdjParmStupBE);
                    //Code for logging into Audit Transaction Table 
                    ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                    audtBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
                    audtBS.Save(AISMasterEntities.AccountNumber, adjParmStupBE.prem_adj_pgm_id, GlobalConstants.AuditingWebPage.CHFSetup, CurrentAISUser.PersonID);
                    //AdjParameterTransactionWrapper.SubmitTransactionChanges();
                    BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
                }
                //else
                //{
                //    ShowMessage("No information has been changed to Save");
                //}
                //this.lblcheckvalidations.Visible = false;
                //this.lblcheckvalidations.Text = "";
                //lnkBtnLBAadj.CommandArgument = adjParmStupBE.adj_paramet_setup_id.ToString();
                //BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
            }
            else
            {
                //this.lblcheckvalidations.Visible = true;
                //this.lblcheckvalidations.Text = " * Please select at least one policy before saving CHF information";
                ShowMessage("Please select at least one policy before saving CHF information");
            }
        }
        catch (Exception ex)
        {
            ShowError((ex.Message.Contains("changed")) ? GlobalConstants.ErrorMessage.RowNotFoundOrChanged : GlobalConstants.ErrorMessage.ServerTooBusy, ex);
        }
    }

    /// <summary>
    /// Save CHF-Information 2nd grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCHFInfo2DetailsSave_Click(object sender, EventArgs e)
    {
        try
        {
            int PolCountCHF = 0;
            bool Flag = false;

            for (int PolCHF = 0; PolCHF < chkbxCHF2Policynolst.Items.Count; PolCHF++)
            {
                if (chkbxCHF2Policynolst.Items[PolCHF].Selected)
                {
                    PolCountCHF = PolCountCHF + 1;
                }
            }

            if (PolCountCHF > 0)
            {


                if (CheckNewNotincCHF == false)
                {
                    //adjParmStupBE = Bindlist.Single(bl => bl.adj_paramet_setup_id == int.Parse(ViewState["ADJLBAPRGID"].ToString()));
                    adjParmStupBE = AdjPrmStupBS.getAdjParamRow(int.Parse(ViewState["ADJCHFnonPRGID"].ToString()));
                    //Concurrency Issue
                    AdjustmentParameterSetupBE adjParmStupBEold = (Bindlist.Where(o => o.adj_paramet_setup_id.Equals(int.Parse(ViewState["ADJCHFnonPRGID"].ToString())))).First();
                    bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmStupBEold.UPDATE_DATE), Convert.ToDateTime(adjParmStupBE.UPDATE_DATE));
                    if (!con)
                        return;
                    //End
                    if (CompareValuesecrow(adjParmStupBE, PolCountCHF))
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
                    //To check Concurrency on Save for CHF
                    if (CheckNewNotincCHF == true)
                    {
                        bool strIncluded = false;
                        string AdjReviewResultLBA = AdjPrmStupBS.getAdjParamResult(int.Parse(ViewState["PRGPRDID"].ToString()), AISMasterEntities.AccountNumber, Lbablaccess.GetLookUpID("CHF", "ADJUSTMENT PARAMETER TYPE"), strIncluded);
                        if (AdjReviewResultLBA == "true")
                        {
                            ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
                            return;
                        }
                    }
                    //End
                    adjParmStupBE = SaveEntityNotincCHF(AdjParmStupBE);
                    BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
                }
                //else
                //{
                //    ShowMessage("No information has been changed to Save");
                //}
                //this.lblcheckvalidations.Visible = false;
                //this.lblcheckvalidations.Text = "";
                //lnkBtnLBAadj.CommandArgument = adjParmStupBE.adj_paramet_setup_id.ToString();
                //BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
            }
            else
            {
                //this.lblcheckvalidations.Visible = true;
                //this.lblcheckvalidations.Text = " * Please select at least one policy before saving CHF information";
                ShowMessage("Please select at least one policy before saving CHF information");
            }
        }
        catch (Exception ex)
        {
            ShowError((ex.Message.Contains("changed")) ? GlobalConstants.ErrorMessage.RowNotFoundOrChanged : GlobalConstants.ErrorMessage.ServerTooBusy, ex);
        }
    }

    /// <summary>
    /// Show CHF First Row Set up details information
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCHFInfoDetailsInfo_Click(object sender, EventArgs e)
    {
        if (ViewState["ADJCHFPRGID"] != null)
        {
            //Close LBA listview in LBA TAB
            //this.pnlpp.Enabled = true;
            this.pnlAdlLBA.Enabled = true;
            this.lbasetupdetailslistview.Visible = false;
            this.LBASetupDetailsLabel.Visible = false;
            this.lnkbtnLBADetailsClose.Visible = false;

            //Close LCF listview in LCF TAB
            //this.pnlppLCF.Enabled = true;
            this.pnlAdjLCF.Enabled = true;
            this.lstLCFSetuplistView.Visible = false;
            this.lblLCFdtls.Visible = false;
            this.lnkLCFClose.Visible = false;

            //Close RML listview in RML TAB
            //this.pnlppRML.Enabled = true;
            this.pnlAdjRML.Enabled = true;
            this.lstRMLSetuplistView.Visible = false;
            this.lblRMLdtls.Visible = false;
            this.lnkBtnCloseRMLdtlsID.Visible = false;

            //Close WA-AX listview in TaxMultiplier TAB
            //this.pnlppTM.Enabled = true;
            this.pnlAdjTM.Enabled = true;
            this.lstTMSetuplistView.Visible = false;
            this.lblTMdtls.Visible = false;
            this.lnkBtnCloseTMdtlsID.Visible = false;

            if (this.CHFSetupDetailsLabel.Visible == false && this.lnkbtnCHFDetailsClose.Visible == false)
            {
                this.CHFSetupDetailsLabel.Visible = true;
                this.lnkbtnCHFDetailsClose.Visible = true;
            }
            adjParmDtlBE = null;
            //this.pnlppCHF.Enabled = false;
            //this.lblcheckvalidations.Visible = false;
            hdnCHFPrmsetup2txtBox.Text = "0";
            hdnCHFPrmsetup1txtBox.Text = ViewState["ADJCHFPRGID"].ToString();
            this.chfsetupdetailslistview.EditIndex = -1;
            BindCHFniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), int.Parse(ViewState["ADJCHFPRGID"].ToString()), AISMasterEntities.AccountNumber);
            this.pnlADlCHF.Enabled = false;
        }
        else
        {
            ShowMessage("Please save CHF Policy information and Basis Charged before selecting the Detail information");
        }
    }

    /// <summary>
    /// Show CHF Second Row Set up details information
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCHF2InfoDetailsInfo_Click(object sender, EventArgs e)
    {
        if (ViewState["ADJCHFnonPRGID"] != null)
        {
            //Close LBA listview in LBA TAB
            //this.pnlpp.Enabled = true;
            this.pnlAdlLBA.Enabled = true;
            this.lbasetupdetailslistview.Visible = false;
            this.LBASetupDetailsLabel.Visible = false;
            this.lnkbtnLBADetailsClose.Visible = false;

            //Close LCF listview in LCF TAB
            //this.pnlppLCF.Enabled = true;
            this.pnlAdjLCF.Enabled = true;
            this.lstLCFSetuplistView.Visible = false;
            this.lblLCFdtls.Visible = false;
            this.lnkLCFClose.Visible = false;

            //Close RML listview in RML TAB
            //this.pnlppRML.Enabled = true;
            this.pnlAdjRML.Enabled = true;
            this.lstRMLSetuplistView.Visible = false;
            this.lblRMLdtls.Visible = false;
            this.lnkBtnCloseRMLdtlsID.Visible = false;

            //Close WA-AX listview in TaxMultiplier TAB
            //this.pnlppTM.Enabled = true;
            this.pnlAdjTM.Enabled = true;
            this.lstTMSetuplistView.Visible = false;
            this.lblTMdtls.Visible = false;
            this.lnkBtnCloseTMdtlsID.Visible = false;

            if (this.CHFSetupDetailsLabel.Visible == false && this.lnkbtnCHFDetailsClose.Visible == false)
            {
                this.CHFSetupDetailsLabel.Visible = true;
                this.lnkbtnCHFDetailsClose.Visible = true;
            }
            adjParmDtlBE = null;
            //this.pnlppCHF.Enabled = false;
            //this.lblcheckvalidations.Visible = false;
            hdnCHFPrmsetup1txtBox.Text = "0";
            hdnCHFPrmsetup2txtBox.Text = ViewState["ADJCHFnonPRGID"].ToString();
            this.chfsetupdetailslistview.EditIndex = -1;
            BindCHFniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), int.Parse(ViewState["ADJCHFnonPRGID"].ToString()), AISMasterEntities.AccountNumber);
            this.pnlADlCHF.Enabled = false;
        }
        else
        {
            ShowMessage("Please save CHF Policy information and Basis Charged before selecting the Detail information");
        }
    }

    /// <summary>
    /// Bind the listview for CHF details 
    /// </summary>
    /// <param name="prgmperiodID"></param>
    /// <param name="AdjPrmtSetupID"></param>
    /// <param name="CustomerID"></param>
    public void BindCHFniformationDetails(int prgmperiodID, int AdjPrmtSetupID, int CustomerID)
    {
        //lblcheckvalidations.Visible = false;
        //lblcheckvalidations.Text = "";
        if (this.CHFSetupDetailsLabel.Visible == false && this.lnkbtnCHFDetailsClose.Visible == false)
        {
            this.CHFSetupDetailsLabel.Visible = true;
            this.lnkbtnCHFDetailsClose.Visible = true;
        }
        Adj_paramet_DtlBS AdjLBAprmDtlBS = new Adj_paramet_DtlBS();
        //IList<AdjustmentParameterDetailBE> AdjParmDtlBElst = AdjLBAprmDtlBS.getLBAAdjParamtrDtls(prgmperiodID, AdjPrmtSetupID, CustomerID);
        AdjParmCHFDtlBElst = AdjLBAprmDtlBS.getLBAAdjParamtrDtls(prgmperiodID, AdjPrmtSetupID, CustomerID);
        this.chfsetupdetailslistview.DataSource = AdjParmCHFDtlBElst;
        this.chfsetupdetailslistview.DataBind();
        this.chfsetupdetailslistview.Visible = true;
    }

    /// <summary>
    /// Cancel any changes if user had selected any items in list view for updation
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstCHFParmsetupDtls_ItemCancel(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            chfsetupdetailslistview.EditIndex = -1;
            int adjParmsetUpID;
            if (hdnCHFPrmsetup1txtBox.Text == "0")
            {
                adjParmsetUpID = Convert.ToInt32(hdnCHFPrmsetup2txtBox.Text);
            }
            else
            {
                adjParmsetUpID = Convert.ToInt32(hdnCHFPrmsetup1txtBox.Text);
            }

            BindCHFniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);
        }
        else if (e.CancelMode == ListViewCancelMode.CancelingInsert)
        {
            CancelUpdateModeCHF(); //Back to normal mode.
        }
    }

    /// <summary>
    /// Function used with the above lstCHFParmsetupDtls_ItemCancel 
    /// </summary>
    protected void CancelUpdateModeCHF()
    {
        chfsetupdetailslistview.InsertItemPosition = InsertItemPosition.None;
        int adjParmsetUpID;
        if (hdnCHFPrmsetup1txtBox.Text == "0")
        {
            adjParmsetUpID = Convert.ToInt32(hdnCHFPrmsetup2txtBox.Text);
        }
        else
        {
            adjParmsetUpID = Convert.ToInt32(hdnCHFPrmsetup1txtBox.Text);
        }

        BindCHFniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);
    }

    /// <summary>
    /// Get CHF Information details from database to fill the list view cotrol 
    /// passing 3 parameters AccountID, CustomerID and ProgramPeriodID
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstCHFParmsetupDtls_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        chfsetupdetailslistview.EditIndex = e.NewEditIndex;
        int adjParmsetUpID;
        if (hdnCHFPrmsetup1txtBox.Text == "0")
        {
            adjParmsetUpID = Convert.ToInt32(hdnCHFPrmsetup2txtBox.Text);
        }
        else
        {
            adjParmsetUpID = Convert.ToInt32(hdnCHFPrmsetup1txtBox.Text);
        }
        BindCHFniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);
    }

    /// <summary>
    /// Not used till now.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstCHFParmsetupDtls_ItemEditing(object sender, ListViewEditEventArgs e)
    {


    }

    /// <summary>
    /// Not used till now.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstCHFParmsetupDtls_ItemCanceling(object sender, ListViewCancelEventArgs e)
    {

    }

    ///<summary >
    /// Update\Modify CHF information details like state, CHF Coverage, No of Claimants and Claim rate
    /// State,CHF Coverage and Claim rate are compulsory fields
    ///</summary>
    /// <param name="e"></param>
    protected void lstCHFParmsetupDtls_ItemUpdate(ListViewItem e)
    {
        try
        {
            //lblcheckvalidations.Visible = false;
            //lblcheckvalidations.Text = "";
            bool StateDetailsExists = false;

            Label lblPPSetupDtlsCHF = (Label)e.FindControl("lblAdjParmtdtlCHFID");
            string strCHFPPsetupdtlsID = lblPPSetupDtlsCHF.Text;
            DropDownList ddlCHFdtlsState = (DropDownList)e.FindControl("ddCHFState");
            DropDownList ddlCHFLossType = (DropDownList)e.FindControl("ddCHFLosstype");
            TextBox txtCHFClmNum = (TextBox)e.FindControl("txtCHFclaimntsnum");
            TextBox txtCHFClmRte = (TextBox)e.FindControl("txtCHFClaimRate");
            CheckBox chkActind = (CheckBox)e.FindControl("chkActiveCHF");

            adjParmDtlBE = AdjPrmtDtlsBS.getAdjParamDtlRow(Convert.ToInt32(strCHFPPsetupdtlsID));
            //Concurrency Issue
            AdjustmentParameterDetailBE adjParmDtlBEold = (AdjParmCHFDtlBElst.Where(o => o.prem_adj_pgm_dtl_id.Equals(Convert.ToInt32(strCHFPPsetupdtlsID)))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmDtlBEold.UPDTE_DATE), Convert.ToDateTime(adjParmDtlBE.UPDTE_DATE));
            if (!con)
                return;
            //End
            if (txtCHFClmNum.Text == "")
            {
                txtCHFClmNum.Text = "0";
            }
            if (adjParmDtlBE.st_id != Convert.ToInt32(ddlCHFdtlsState.SelectedValue) ||
                adjParmDtlBE.clm_hndl_fee_los_typ_id != Convert.ToInt32(ddlCHFLossType.SelectedValue) ||
                adjParmDtlBE.Clm_hndlfee_clmrate != Convert.ToDecimal(txtCHFClmRte.Text) ||
               adjParmDtlBE.CHF_CLMT_NUMBER != Convert.ToDecimal(txtCHFClmNum.Text)||
                adjParmDtlBE.act_ind!=chkActind.Checked)
            {
                if (hdnCHFPrmsetup1txtBox.Text == "0")
                {
                    adjParmDtlBE.adj_paramet_id = Convert.ToInt32(hdnCHFPrmsetup2txtBox.Text);
                }
                else
                {
                    adjParmDtlBE.adj_paramet_id = Convert.ToInt32(hdnCHFPrmsetup1txtBox.Text);
                }

                adjParmDtlBE.AccountID = AISMasterEntities.AccountNumber;
                adjParmDtlBE.PrgmPerodID = int.Parse(ViewState["PRGPRDID"].ToString());
                if (adjParmDtlBE.st_id != Convert.ToInt32(ddlCHFdtlsState.SelectedValue) ||
                adjParmDtlBE.clm_hndl_fee_los_typ_id != Convert.ToInt32(ddlCHFLossType.SelectedValue))
                {
                    //Get the AdjParameter LBA details list for specific Account or Customer, Adjustmet Parameter Setupid and Program Period ID
                    IList<AdjustmentParameterDetailBE> AdjParmetDtlComparelstCHFBE = AdjPrmtDtlsBS.getLBAAdjParamtrDtls(adjParmDtlBE.PrgmPerodID, adjParmDtlBE.adj_paramet_id, adjParmDtlBE.AccountID);

                    //check for each item in list if the values exists for the entered current state
                    //IF exists dont save the information
                    foreach (AdjustmentParameterDetailBE AdjCompareCHFdetailBE in AdjParmetDtlComparelstCHFBE)
                    {
                        if (AdjCompareCHFdetailBE.st_id == Convert.ToInt32(ddlCHFdtlsState.SelectedValue) &&
                            AdjCompareCHFdetailBE.clm_hndl_fee_los_typ_id == Convert.ToInt32(ddlCHFLossType.SelectedValue))
                        {
                            StateDetailsExists = true;
                            //lblcheckvalidations.Visible = true;
                            //lblcheckvalidations.Text = " * The details for the State and CHFLosstype you are trying to save already exists";
                            ShowMessage("The details for the State and CHFLosstype you are trying to save already exists");
                            break;
                        }
                    }
                }
                if (!StateDetailsExists)
                {
                    adjParmDtlBE.st_id = Convert.ToInt32(ddlCHFdtlsState.SelectedValue);
                    adjParmDtlBE.clm_hndl_fee_los_typ_id = Convert.ToInt32(ddlCHFLossType.SelectedValue);
                    if (txtCHFClmNum.Text != "")
                    {
                        adjParmDtlBE.CHF_CLMT_NUMBER = Convert.ToDecimal(txtCHFClmNum.Text);
                    }
                    else
                    {
                        adjParmDtlBE.CHF_CLMT_NUMBER = 0;
                    }
                    adjParmDtlBE.Clm_hndlfee_clmrate = Convert.ToDecimal(txtCHFClmRte.Text.Replace(",", ""));
                    if (chkActind.Checked == false)
                    {
                        adjParmDtlBE.act_ind = false;
                    }
                    else
                    {
                        adjParmDtlBE.act_ind = true;
                    }

                    adjParmDtlBE.UPDTE_USER_ID = CurrentAISUser.PersonID;
                    adjParmDtlBE.UPDTE_DATE = DateTime.Now;

                    bool SaveSuccess = AdjPrmtDtlsBS.Update(adjParmDtlBE);
                    ShowConcurrentConflict(SaveSuccess, adjParmDtlBE.ErrorMessage);
                    if (SaveSuccess)
                    {
                        AdjParameterTransactionWrapper.SubmitTransactionChanges();
                        //Code for logging into Audit Transaction Table 
                        ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                        audtBS.AppTransactionWrapper = AdjParameterTransactionWrapper;
                        audtBS.Save(AISMasterEntities.AccountNumber, adjParmDtlBE.PrgmPerodID, GlobalConstants.AuditingWebPage.CHFSetup, CurrentAISUser.PersonID);
                        AdjParameterTransactionWrapper.SubmitTransactionChanges();
                    }
                    else
                    {
                        AdjParameterTransactionWrapper.RollbackChanges();
                    }
                    this.chfsetupdetailslistview.EditIndex = -1;
                    BindCHFniformationDetails(adjParmDtlBE.PrgmPerodID, adjParmDtlBE.adj_paramet_id, AISMasterEntities.AccountNumber);
                }
            }
            else
            {
//                ShowMessage("No information has been changed to Save");
                chfsetupdetailslistview.EditIndex = -1;
                int adjParmsetUpID;
                if (hdnCHFPrmsetup1txtBox.Text == "0")
                {
                    adjParmsetUpID = Convert.ToInt32(hdnCHFPrmsetup2txtBox.Text);
                }
                else
                {
                    adjParmsetUpID = Convert.ToInt32(hdnCHFPrmsetup1txtBox.Text);
                }

                BindCHFniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), adjParmsetUpID, AISMasterEntities.AccountNumber);

            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// This method is not used currently.
    /// </summary>
    /// <param name="sener"></param>
    /// <param name="e"></param>
    protected void btnchfsetupinfodetails_click(object sener, EventArgs e)
    {

    }

    /// <summary>
    /// This method is used to check if the user is updating or saving list view details and also
    /// calling those update and save methods, Also used to unable and Disable the rows in list view
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstCHFParmsetupDtls_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "Save")
        {
            lstCHFRelatedinfo_Saving(e.Item);
        }
        else if (e.CommandName == "Update")
        {
            lstCHFParmsetupDtls_ItemUpdate(e.Item);
        }
        else if (e.CommandName.ToUpper() == "DISABLE")
        {
            DisableCHFParmdtlsRow(e.Item, Convert.ToInt32(e.CommandArgument), false);
        }
        else if (e.CommandName.ToUpper() == "ENABLE")
        {
            DisableCHFParmdtlsRow(e.Item, Convert.ToInt32(e.CommandArgument), true);
        }

    }

    /// <summary>
    /// Not used till now.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstCHFParmsetupDtls_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {

    }

    /// <summary>
    /// Save CHF information details like state, Claim Rate, CHF Covverage, No of claimants and Claim rate
    /// State, CHF Coverage and Claim Rate are compulsory fields
    /// </summary>
    /// <param name="e"></param>
    protected void lstCHFRelatedinfo_Saving(ListViewItem e)
    {
        try
        {
            bool StateDetailsExists = false;
            //lblcheckvalidations.Text = "";
            //lblcheckvalidations.Visible = false;
            AdjustmentParameterDetailBE CHFDetailsBE = new AdjustmentParameterDetailBE();
            Adj_paramet_DtlBS CHFdtlsBS = new Adj_paramet_DtlBS();
            DropDownList ddlCHFdtlsState = (DropDownList)e.FindControl("ddCHFState");
            DropDownList ddlCHFLossType = (DropDownList)e.FindControl("ddCHFLosstype");
            TextBox txtCHFClmNum = (TextBox)e.FindControl("txtCHFclaimntsnum");
            TextBox txtCHFClmRte = (TextBox)e.FindControl("txtCHFClaimRate");

            CHFDetailsBE.AccountID = AISMasterEntities.AccountNumber;
            CHFDetailsBE.PrgmPerodID = int.Parse(ViewState["PRGPRDID"].ToString());
            if (hdnCHFPrmsetup1txtBox.Text == "0")
            {
                CHFDetailsBE.adj_paramet_id = Convert.ToInt32(hdnCHFPrmsetup2txtBox.Text);
            }
            else
            {
                CHFDetailsBE.adj_paramet_id = Convert.ToInt32(hdnCHFPrmsetup1txtBox.Text);
            }

            //Get the AdjParameter LBA details list for specific Account or Customer, Adjustmet Parameter Setupid and Program Period ID
            IList<AdjustmentParameterDetailBE> AdjParmetCHFDtlComparelstBE = AdjParmDtlsBS.getLBAAdjParamtrDtls(CHFDetailsBE.PrgmPerodID, CHFDetailsBE.adj_paramet_id, CHFDetailsBE.AccountID);

            //check for each item in list if the values exists for the entered current state
            //IF exists dont save the information
            foreach (AdjustmentParameterDetailBE AdjCompareCHFdetailBE in AdjParmetCHFDtlComparelstBE)
            {
                if (AdjCompareCHFdetailBE.st_id == Convert.ToInt32(ddlCHFdtlsState.SelectedValue) &&
                    AdjCompareCHFdetailBE.clm_hndl_fee_los_typ_id == Convert.ToInt32(ddlCHFLossType.SelectedValue))
                {
                    StateDetailsExists = true;
                    //lblcheckvalidations.Visible = true;
                    //lblcheckvalidations.Text = " * The details for the State and CHF Losstype you are trying to save already exists";
                    ShowMessage("The details for the State and CHF Losstype you are trying to save already exists");
                    break;
                }

            }
            if (!StateDetailsExists)
            {
                CHFDetailsBE.st_id = Convert.ToInt32(ddlCHFdtlsState.SelectedValue);
                CHFDetailsBE.clm_hndl_fee_los_typ_id = Convert.ToInt32(ddlCHFLossType.SelectedValue);
                if (txtCHFClmNum.Text != "")
                {
                    CHFDetailsBE.CHF_CLMT_NUMBER = Convert.ToDecimal(txtCHFClmNum.Text);
                }
                else
                {
                    CHFDetailsBE.CHF_CLMT_NUMBER = 0;
                }
                CHFDetailsBE.Clm_hndlfee_clmrate = Convert.ToDecimal(txtCHFClmRte.Text.Replace(",", ""));
                CHFDetailsBE.CRTE_DATE = DateTime.Now;
                CHFDetailsBE.UPDTE_DATE = (Nullable<DateTime>)null;
                CHFDetailsBE.act_ind = true;
                CHFDetailsBE.CRTE_USER_ID = CurrentAISUser.PersonID;
                CHFDetailsBE.UPDTE_USER_ID = (Nullable<int>)null;
                //Audit trail
                CHFdtlsBS.Update(CHFDetailsBE);
                //ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                //audtBS.Save(AISMasterEntities.AccountNumber, CHFDetailsBE.PrgmPerodID, GlobalConstants.AuditingWebPage.CHFSetup, CurrentAISUser.PersonID);
                //AdjParameterTransactionWrapper.SubmitTransactionChanges();
                BindCHFniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), CHFDetailsBE.adj_paramet_id, AISMasterEntities.AccountNumber);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// Close the CHFList view control along with its label and this close button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCHFInfoClose_Click(object sender, EventArgs e)
    {
        //this.pnlppCHF.Enabled = true;
        this.pnlADlCHF.Enabled = true;
        this.chfsetupdetailslistview.Visible = false;
        this.CHFSetupDetailsLabel.Visible = false;
        this.lnkbtnCHFDetailsClose.Visible = false;
    }

    /// <summary>
    /// Disable\Enable CHF information details for a state
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DisableCHFParmdtlsRow(ListViewItem e, int AdjparmetDtlID, bool Flag)
    {
        try
        {
            adjParmDtlBE = AdjPrmtDtlsBS.getAdjParamDtlRow(AdjparmetDtlID);
            //Concurrency Issue
            AdjustmentParameterDetailBE adjParmDtlBEold = (AdjParmCHFDtlBElst.Where(o => o.prem_adj_pgm_dtl_id.Equals(AdjparmetDtlID))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmDtlBEold.UPDTE_DATE), Convert.ToDateTime(adjParmDtlBE.UPDTE_DATE));
            if (!con)
                return;
            //End
            adjParmDtlBE.act_ind = Flag;
            adjParmDtlBE.UPDTE_USER_ID = CurrentAISUser.PersonID;
            adjParmDtlBE.UPDTE_DATE = DateTime.Now;
            Flag = AdjPrmtDtlsBS.Update(adjParmDtlBE);
            if (Flag)
            {
                AdjParameterTransactionWrapper.SubmitTransactionChanges();
                //Code for logging into Audit Transaction Table 
                ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                audtBS.Save(AISMasterEntities.AccountNumber, adjParmDtlBE.PrgmPerodID, GlobalConstants.AuditingWebPage.CHFSetup, CurrentAISUser.PersonID);
                AdjParameterTransactionWrapper.SubmitTransactionChanges();
            }
            else
            {
                AdjParameterTransactionWrapper.RollbackChanges();
            }
            int Adjparmsetupiid;
            if (hdnCHFPrmsetup1txtBox.Text == "0")
            {
                Adjparmsetupiid = Convert.ToInt32(hdnCHFPrmsetup2txtBox.Text);
            }
            else
            {
                Adjparmsetupiid = Convert.ToInt32(hdnCHFPrmsetup1txtBox.Text);
            }
            BindCHFniformationDetails(int.Parse(ViewState["PRGPRDID"].ToString()), Adjparmsetupiid, AISMasterEntities.AccountNumber);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// Disable\Enable Second Row for CHF details table
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DisableCHFSecndRow(object sender, CommandEventArgs e)
    {
        try
        {
            //AdjustmentParameterSetupBE LBAParmBE;
            if (ViewState["ADJCHFnonPRGID"] != null)
            {
                int AdjParmtSetupID = int.Parse(ViewState["ADJCHFnonPRGID"].ToString());
                //LBAParmBE = AdjparamsetupInfo.getAdjParamRow(AdjParmtSetupID);
                adjParmStupBE = AdjPrmStupBS.getAdjParamRow(AdjParmtSetupID);
                //Concurrency Issue
                AdjustmentParameterSetupBE adjParmStupBEold = (Bindlist.Where(o => o.adj_paramet_setup_id.Equals(AdjParmtSetupID))).First();
                bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmStupBEold.UPDATE_DATE), Convert.ToDateTime(adjParmStupBE.UPDATE_DATE));
                if (!con)
                    return;
                //End
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
                //bool ResultLBA = AdjparamsetupInfo.Update(LBAParmBE);
                bool ResultCHF = AdjPrmStupBS.Update(adjParmStupBE);
                if (ResultCHF)
                {
                    AdjParameterTransactionWrapper.SubmitTransactionChanges();
                    //Code for logging into Audit Transaction Table 
                    ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                    audtBS.Save(AISMasterEntities.AccountNumber, adjParmStupBE.prem_adj_pgm_id, GlobalConstants.AuditingWebPage.CHFSetup, CurrentAISUser.PersonID);
                    AdjParameterTransactionWrapper.SubmitTransactionChanges();
                }
                else
                {
                    AdjParameterTransactionWrapper.RollbackChanges();
                }
                BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// Disable\Enable First Row for CHF details table
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DisableCHFfirstRow(object sender, CommandEventArgs e)
    {
        try
        {
            //AdjustmentParameterSetupBE  LBAParmBE;
            if (ViewState["ADJCHFPRGID"] != null)
            {
                int AdjParmtSetupID = int.Parse(ViewState["ADJCHFPRGID"].ToString());
                //LBAParmBE = AdjparamsetupInfo.getAdjParamRow(AdjParmtSetupID);
                adjParmStupBE = AdjPrmStupBS.getAdjParamRow(AdjParmtSetupID);
                //Concurrency Issue
                AdjustmentParameterSetupBE adjParmStupBEold = (Bindlist.Where(o => o.adj_paramet_setup_id.Equals(AdjParmtSetupID))).First();
                bool con = ShowConcurrentConflict(Convert.ToDateTime(adjParmStupBEold.UPDATE_DATE), Convert.ToDateTime(adjParmStupBE.UPDATE_DATE));
                if (!con)
                    return;
                //End
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
                bool ResultCHF = AdjPrmStupBS.Update(adjParmStupBE);
                if (ResultCHF)
                {
                    AdjParameterTransactionWrapper.SubmitTransactionChanges();
                    //Code for logging into Audit Transaction Table 
                    ApplWebPageAudtBS audtBS = new ApplWebPageAudtBS();
                    audtBS.Save(AISMasterEntities.AccountNumber, adjParmStupBE.prem_adj_pgm_id, GlobalConstants.AuditingWebPage.CHFSetup, CurrentAISUser.PersonID);
                    AdjParameterTransactionWrapper.SubmitTransactionChanges();
                }
                else
                {
                    AdjParameterTransactionWrapper.RollbackChanges();
                }
                BindLBADetails(int.Parse(ViewState["PRGPRDID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// The method is used after the CHF details info listview control is bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstCHFParmsetupDtls_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            DropDownList ddCHFStateitms = (DropDownList)e.Item.FindControl("ddCHFState");
            DropDownList ddCHFLossitms = (DropDownList)e.Item.FindControl("ddCHFLosstype");
            Label hdCHFState = (Label)e.Item.FindControl("hidCHFState");
            Label hdCHFLosstype = (Label)e.Item.FindControl("hidCHFLosstype");

            if ((ddCHFStateitms != null) && (hdCHFState != null))
            {
//                ddCHFStateitms.SelectedIndex = ddCHFStateitms.Items.IndexOf(ddCHFStateitms.Items.FindByText(hdCHFState.Text.ToString()));
                AddInActiveLookupDataByText(ref ddCHFStateitms, hdCHFState.Text);
            }
            if ((ddCHFLossitms != null) && (hdCHFLosstype != null))
            {
//                ddCHFLossitms.SelectedIndex = ddCHFLossitms.Items.IndexOf(ddCHFLossitms.Items.FindByText(hdCHFLosstype.Text.ToString()));
                AddInActiveLookupDataByText(ref ddCHFLossitms, hdCHFLosstype.Text);
            }
            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDisableEnableCHF");

            if (imgDelete != null)
            {
                HiddenField hid = (HiddenField)e.Item.FindControl("hidActiveCHF");

                if (hid.Value == "True")
                {
                    LinkButton lbCHFEDit = (LinkButton)e.Item.FindControl("ChfdetailsetupEdit");
                    if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                        lbCHFEDit.Enabled = true;
                    imgDelete.CommandName = "Disable";
                    imgDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable this record?');");
                }
                else
                {
                    LinkButton lbCHFEDitAlt = (LinkButton)e.Item.FindControl("ChfdetailsetupEdit");
                    lbCHFEDitAlt.Enabled = false;
                    imgDelete.CommandName = "Enable";
                    imgDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable this record?');");
                }
            }
        }
    }

    protected void lstCHFParmsetupDtls_Sorting(object sender, ListViewSortEventArgs e)
    {
        ImageButton imgCHFbtn = new ImageButton();
        imgCHFbtn = (ImageButton)chfsetupdetailslistview.FindControl("imgCHFSort");

        e.SortDirection = (imgCHFbtn.ImageUrl.Contains("Des")) ? SortDirection.Ascending : SortDirection.Descending;

        if (e.SortDirection == SortDirection.Ascending)
            AdjParmCHFDtlBElst = AdjParmCHFDtlBElst.OrderBy(sl => sl.PrgParameterCHFLName).ToList();
        else
            AdjParmCHFDtlBElst = AdjParmCHFDtlBElst.OrderByDescending(sl => sl.PrgParameterCHFLName).ToList();
        ChangeImage(imgCHFbtn, e.SortDirection);

        chfsetupdetailslistview.DataSource = AdjParmCHFDtlBElst;
        chfsetupdetailslistview.DataBind();
    }

    private void ChangeImage(ImageButton imgBtn, SortDirection sDir)
    {
        if (sDir == SortDirection.Ascending)
        {
            imgBtn.ImageUrl = "~/images/ascending.gif";
            imgBtn.ToolTip = "Ascending";
        }
        else
        {
            imgBtn.ImageUrl = "~/images/Descending.gif";
            imgBtn.ToolTip = "Descending";
        }
        imgBtn.Visible = true;
    }

    #endregion

}
