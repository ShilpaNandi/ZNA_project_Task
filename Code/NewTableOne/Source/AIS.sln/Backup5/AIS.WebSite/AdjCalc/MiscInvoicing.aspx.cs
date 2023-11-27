//Default namespaces in MiscInvoice screen
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
//Importing different AIS framework namespaces for MiscInvoice screen
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.AIS.DAL.Logic;


/// <summary>
/// This is the class which is used to give the information about the MiscInvoice and it 
/// also inherits AISBase Page,so that some of the common functionality needed for all pages is implemented in
/// the Basepage
/// </summary>
#region MiscInvoicing Class
public partial class MiscInvoicing : AISBasePage
{  
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
        this.Master.Page.Title = "Miscellaneous Invoicing";
        if (!IsPostBack)
        {
            
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
    /// a property for PremiumAdjMiscInvoice Business Service Class
    /// </summary>
    /// <returns>PremiumAdjMiscInvoiceBS</returns>
    private PremiumAdjMiscInvoiceBS prmAdjMiscInvoice;
    private PremiumAdjMiscInvoiceBS PrmAdjMiscInvoice
    {
        get
        {
            if (prmAdjMiscInvoice == null)
            {
                prmAdjMiscInvoice = new PremiumAdjMiscInvoiceBS();
            }
            return prmAdjMiscInvoice;
        }
    }
    private IList<PremiumAdjMiscInvoiceBE> PremAdjMISCOLD
    {
        get
        {
            //if (Session["PremAdjMISCOLD"] == null)
            //    Session["PremAdjMISCOLD"] = new List<PremiumAdjMiscInvoiceBE>();
            //return (IList<PremiumAdjMiscInvoiceBE>)Session["PremAdjMISCOLD"];
            if (RetrieveObjectFromSessionUsingWindowName("PremAdjMISCOLD") == null)
                SaveObjectToSessionUsingWindowName("PremAdjMISCOLD", new List<PremiumAdjMiscInvoiceBE>());
            return (IList<PremiumAdjMiscInvoiceBE>)RetrieveObjectFromSessionUsingWindowName("PremAdjMISCOLD");
        }
        set
        {
            //Session["PremAdjMISCOLD"] = value;
            SaveObjectToSessionUsingWindowName("PremAdjMISCOLD", value);
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
            intPremAdjPerdID = premAdjPerdBE[premAdjPerdBE.Count-1].PREM_ADJ_PERD_ID;
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
        lblPremAdjMiscInvoiceDetails.Text = "MISCELLANEOUS INVOICING" + "" + "-" + premAdjPgmBE[0].STARTDATE_ENDDATE_PGMTYP.ToString();
        BindListView();

    }
    #endregion
    /// <summary>
    /// ListView Item Command Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Item Command Event
    protected void lstMiscellaneousInvoicing_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {

        if ((e.CommandName.ToUpper() == "SAVE") || (e.CommandName.ToUpper() == "UPDATE"))
        {

             //Get the values  
            TextBox txtAmount = (TextBox)e.Item.FindControl("txtAmount");
            DropDownList ddlContactType = (DropDownList)e.Item.FindControl("ddlMiscInvoiceTypelist");
            TextBox txtSym = (TextBox)e.Item.FindControl("txtSym");
            TextBox txtNbr = (TextBox)e.Item.FindControl("txtNbr");
            TextBox txtModulus = (TextBox)e.Item.FindControl("txtModulus");
            PostingTransactionTypeBE postTransTypBE = new PostingTransactionTypeBS().LoadData(Convert.ToInt32(ddlContactType.SelectedValue));
            if (e.CommandName.ToUpper() == "SAVE")  //if it is new
            {
                try
                {
                PremiumAdjMiscInvoiceBE pami = new PremiumAdjMiscInvoiceBE();
                pami.POST_TRANS_TYP_ID = Convert.ToInt32(ddlContactType.SelectedValue);
                pami.PREM_ADJ_PERD_ID = this.PremAdjPerdID;
                pami.PREM_ADJ_ID = this.PreAdjID;
                pami.POST_AMT = Convert.ToDecimal(txtAmount.Text.ToString());
                pami.POL_SYM_TXT = txtSym.Text;
                if (txtNbr.Text.Trim().Length == 7) txtNbr.Text = "0" + txtNbr.Text;
                pami.POL_NBR_TXT = txtNbr.Text;
                pami.POL_MODULUS_TXT = txtModulus.Text;
                pami.CREATE_USER_ID = CurrentAISUser.PersonID;
                pami.CREATE_DATE = DateTime.Now;
                pami.ACTV_IND = true;
                pami.MISC_POSTS_IND = postTransTypBE.MISC_POSTS_IND;
                pami.ADJ_SUMRY_POST_FLAG_IND = postTransTypBE.ADJ_SUMRY_NOT_POST_IND;
                pami.CUSTMR_ID = this.AccountID;
                bool i = PrmAdjMiscInvoice.Update(pami);
                if (i)
                {
                    (new PremiumAdjustmentPeriodBS()).deletePremAdjPerdTotal(this.PremAdjPerdID, this.PreAdjID, this.AccountID);
                    (new PremiumAdjustmentPeriodBS()).AddPremAdjPerdTotal(this.PremAdjPerdID, this.PreAdjID, this.AccountID, this.PrgPeriodID, CurrentAISUser.PersonID);
                }
                BindListView();
                }
                catch (RetroBaseException ee)
                {
                    ShowError(ee.Message,ee);
                }

            }
            else if (e.CommandName.ToUpper() == "UPDATE")  //if it is Update
            {
                Label lblPremAdjMiscInvoiceID = (Label)e.Item.FindControl("lblPremAdjMiscInvoiceID");
                int intPremAdjMiscInvoiceID = Convert.ToInt32(lblPremAdjMiscInvoiceID.Text);
                PremiumAdjMiscInvoiceBE pamiBE;
                try
                {
                    pamiBE = PrmAdjMiscInvoice.getMiscInvoiceRow(intPremAdjMiscInvoiceID);
                    // Concurrency Code
                    PremiumAdjMiscInvoiceBE pamiBEConcurrent = (PremAdjMISCOLD.Where(o => o.PREM_ADJ_MISC_INVC_ID.Equals(intPremAdjMiscInvoiceID))).First();
                    bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(pamiBEConcurrent.UPDATE_DATE), Convert.ToDateTime(pamiBE.UPDATE_DATE));
                    if (!Concurrency)
                        return;
        
                    pamiBE.POST_TRANS_TYP_ID = Convert.ToInt32(ddlContactType.SelectedValue);
                    pamiBE.POST_AMT = Convert.ToDecimal(txtAmount.Text.ToString());
                    pamiBE.POL_SYM_TXT = txtSym.Text;
                    if (txtNbr.Text.Trim().Length == 7) txtNbr.Text = "0" + txtNbr.Text;
                    pamiBE.POL_NBR_TXT = txtNbr.Text;
                    pamiBE.POL_MODULUS_TXT = txtModulus.Text;
                    pamiBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
                    pamiBE.UPDATE_DATE = DateTime.Now;
                    bool i = PrmAdjMiscInvoice.Update(pamiBE);
                    ShowConcurrentConflict(i, pamiBE.ErrorMessage);
                    if (i)
                    {
                        (new PremiumAdjustmentPeriodBS()).deletePremAdjPerdTotal(this.PremAdjPerdID, this.PreAdjID, this.AccountID);
                        (new PremiumAdjustmentPeriodBS()).AddPremAdjPerdTotal(this.PremAdjPerdID, this.PreAdjID, this.AccountID, this.PrgPeriodID, CurrentAISUser.PersonID);
                    }
                }
                catch (RetroBaseException ee)
                {
                    ShowError(ee.Message,ee);
                }

                 //get out of the edit mode
                lstMiscellaneousInvoicing.EditIndex = -1;
                lstMiscellaneousInvoicing.InsertItemPosition = InsertItemPosition.FirstItem;
                BindListView();
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
    /// function to make enable or disable a Misc Invoice Record
    /// </summary>
    /// <param name="PremAdjMiscInvoiceID"></param>
    /// <param name="Flag">True/False boolean value</param>
    /// <returns>void</returns>
    #region DisableRow
    protected void DisableRow(int intPremAdjMiscInvoiceID, bool Flag)
    {
        try
        {
        PremiumAdjMiscInvoiceBE pamiBE = new PremiumAdjMiscInvoiceBE();
        pamiBE = PrmAdjMiscInvoice.getMiscInvoiceRow(intPremAdjMiscInvoiceID);
        // Concurrency Code
        PremiumAdjMiscInvoiceBE pamiBEConcurrent = (PremAdjMISCOLD.Where(o => o.PREM_ADJ_MISC_INVC_ID.Equals(intPremAdjMiscInvoiceID))).First();
        bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(pamiBEConcurrent.UPDATE_DATE), Convert.ToDateTime(pamiBE.UPDATE_DATE));
        if (!Concurrency)
            return;
        
        pamiBE.ACTV_IND = Flag;
        pamiBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
        pamiBE.UPDATE_DATE = DateTime.Now;
        Flag = PrmAdjMiscInvoice.Update(pamiBE);
        }
        catch (RetroBaseException ee)
        {
            ShowError(ee.Message,ee);
        }
        if (Flag)
        {
            (new PremiumAdjustmentPeriodBS()).deletePremAdjPerdTotal(this.PremAdjPerdID, this.PreAdjID, this.AccountID);
            (new PremiumAdjustmentPeriodBS()).AddPremAdjPerdTotal(this.PremAdjPerdID, this.PreAdjID, this.AccountID, this.PrgPeriodID, CurrentAISUser.PersonID);
            //Function to bind the LookupType Data
            BindListView();
        }
    }
#endregion
    /// <summary>
    /// Item Updating Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region ItemUpdating event
    protected void lstMiscellaneousInvoicing_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    { }
    #endregion
    /// <summary>
    /// Item Edit Event of a ListView
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region ItemEdit Event  
    protected void lstMiscellaneousInvoicing_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        lstMiscellaneousInvoicing.EditIndex = e.NewEditIndex;
        //BindListView();
        int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
        if (intAdjCalStatusID == AdjStatusTypeID)
        {
            lstMiscellaneousInvoicing.InsertItemPosition = InsertItemPosition.None;
        }
        else
        {
            lstMiscellaneousInvoicing.InsertItemPosition = InsertItemPosition.FirstItem;
        }
        lstMiscellaneousInvoicing.DataSource = PrmAdjMiscInvoice.GetMiscInvoicelist(this.AccountID, this.PremAdjPerdID, this.PreAdjID);
        lstMiscellaneousInvoicing.DataBind();
    }
    #endregion
    /// <summary>
    /// Item Cancel Event of ListView
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region ItemCancel Event
    protected void lstMiscellaneousInvoicing_ItemCancel(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstMiscellaneousInvoicing.EditIndex = -1;
            //BindListView();
            int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            if (intAdjCalStatusID == AdjStatusTypeID)
            {
                lstMiscellaneousInvoicing.InsertItemPosition = InsertItemPosition.FirstItem;
            }
            else
            {
                lstMiscellaneousInvoicing.InsertItemPosition = InsertItemPosition.None;
            }
            lstMiscellaneousInvoicing.DataSource = PrmAdjMiscInvoice.GetMiscInvoicelist(this.AccountID, this.PremAdjPerdID, this.PreAdjID);
            lstMiscellaneousInvoicing.DataBind();
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
        DateTime dtValDate=Convert.ToDateTime(ddlValDates.SelectedValue.ToString());
        string strProgramPeriod = ddlPrgPeriod.SelectedValue.ToString();
        
       int intPremAdjPerdID = 0;
       int intPrgPeriodID = Convert.ToInt32(strProgramPeriod);
      
        IList<PremiumAdjustmentPeriodBE> premAdjPerdBE = new List<PremiumAdjustmentPeriodBE>();
        premAdjPerdBE = PrmAdjPerd.getPremAdjPerdID(intAccountID, intPremAdjID, intPrgPeriodID);
        if (premAdjPerdBE.Count > 0)
        {
            intPremAdjPerdID = premAdjPerdBE[premAdjPerdBE.Count-1].PREM_ADJ_PERD_ID;
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
        lblPremAdjMiscInvoiceDetails.Text = "MISCELLANEOUS INVOICING" + "" + "-" + ddlPrgPeriod.SelectedItem.ToString();
        BindListView();
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
        lblPremAdjMiscInvoiceDetails.Text = string.Empty;
        lstMiscellaneousInvoicing.Items.Clear();
        lstMiscellaneousInvoicing.Visible = false;
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
    /// Function to bind the Listview with Miscllenous Invoice data
    /// </summary>
    /// <param name=""></param>
    /// <returns>void</returns>
    #region BindListView
    public void BindListView()
    {
        lstMiscellaneousInvoicing.Visible = true;
        try
        {
            
            int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            if (intAdjCalStatusID != AdjStatusTypeID)
            {
                lstMiscellaneousInvoicing.InsertItemPosition = InsertItemPosition.None;
            }
            else
            {
                lstMiscellaneousInvoicing.InsertItemPosition = InsertItemPosition.FirstItem;
            }
            PremAdjMISCOLD=PrmAdjMiscInvoice.GetMiscInvoicelist(this.AccountID, this.PremAdjPerdID, this.PreAdjID);
            lstMiscellaneousInvoicing.DataSource = PremAdjMISCOLD;
            lstMiscellaneousInvoicing.DataBind();
            if (lstMiscellaneousInvoicing.InsertItemPosition != InsertItemPosition.None)
            {
                LinkButton lnkSave = (LinkButton)lstMiscellaneousInvoicing.InsertItem.FindControl("lbItemSave");
                HiddenField hfPolreq = (HiddenField)lstMiscellaneousInvoicing.InsertItem.FindControl("hfPolreq");
                RequiredFieldValidator reqPolSym = (RequiredFieldValidator)lstMiscellaneousInvoicing.InsertItem.FindControl("reqSym");
                RequiredFieldValidator reqPolNbr = (RequiredFieldValidator)lstMiscellaneousInvoicing.InsertItem.FindControl("reqNbr");
                RequiredFieldValidator reqPolMod = (RequiredFieldValidator)lstMiscellaneousInvoicing.InsertItem.FindControl("reqModulus");
                lnkSave.Attributes.Add("onclick", "javascript:EnableDisable('" + hfPolreq.ClientID + "','" + reqPolSym.ClientID + "','" + reqPolNbr.ClientID + "','" + reqPolMod.ClientID + "')");
            }
        }
        catch (RetroBaseException ee)
        {
            ShowError(ee.Message,ee);
        }
    }
    #endregion
    /// <summary>
    /// Item Data Bound Event-it is called while binding each row to the listview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region ListView DataBound Event
    protected void lstMiscellaneousInvoicing_DataBoundList(Object sender, ListViewItemEventArgs e)
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
            int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lblItemEdit");
            if (AdjStatusTypeID != intAdjCalStatusID)
            {
                lnkEdit.Enabled = false;
            }

            ImageButton imgDelete = (ImageButton)e.Item.FindControl("imgDisable");
            if (imgDelete != null)
            {
                HiddenField hid = (HiddenField)e.Item.FindControl("hidActive");
                imgDelete.CommandName = hid.Value == "True" ? "DISABLE" : "ENABLE";
                imgDelete.Attributes.Add("onclick", hid.Value == "True" ? "return confirm('Are you sure you want to Disable?');" : "return confirm('Are you sure you want to Enable?');");
            }
            //for Edit
            ListViewDataItem dataItem = (ListViewDataItem)e.Item;
            if (dataItem.DisplayIndex == lstMiscellaneousInvoicing.EditIndex)
            {
                Label lblPostTransTyp = (Label)e.Item.FindControl("lblPostTrnsTypeID");
                LinkButton lnkUpdate = (LinkButton)e.Item.FindControl("lbMiscInvoiceUpdate");
                HiddenField hfPolreq = (HiddenField)e.Item.FindControl("hfPolreq");
                DropDownList ddlEditPostTransTyp = (DropDownList)e.Item.FindControl("ddlMiscInvoiceTypelist");
                IList<PostingTransactionTypeBE> ilistPostTransTypBE = new List<PostingTransactionTypeBE>();
                ilistPostTransTypBE = new PostingTransactionTypeBS().getPremAdjMiscInvoiceEditData(Convert.ToInt32(lblPostTransTyp.Text));
                ddlEditPostTransTyp.DataSource = ilistPostTransTypBE;
                ddlEditPostTransTyp.DataTextField = "TRANS_TXT";
                ddlEditPostTransTyp.DataValueField = "POST_TRANS_TYP_ID";
                ddlEditPostTransTyp.DataBind();
                if ((ddlEditPostTransTyp != null) && (lblPostTransTyp != null))
                {
                    ddlEditPostTransTyp.Items.FindByValue(lblPostTransTyp.Text).Selected = true;
                }
                RequiredFieldValidator reqSymValidator = (RequiredFieldValidator)e.Item.FindControl("reqSym");
                RequiredFieldValidator reqNbrValidator = (RequiredFieldValidator)e.Item.FindControl("reqNbr");
                RequiredFieldValidator reqModulusValidator = (RequiredFieldValidator)e.Item.FindControl("reqModulus");
                //bool ispolReq = new PostingTransactionTypeBS().IsPolicyRequires(Convert.ToInt32(lblPostTransTyp.Text));
                //reqModulusValidator.Enabled = ispolReq;
                //reqNbrValidator.Enabled = ispolReq;
                //reqSymValidator.Enabled = ispolReq;
                lnkUpdate.Attributes.Add("onclick", "javascript:EnableDisable('" + hfPolreq.ClientID + "','" + reqSymValidator.ClientID + "','" + reqNbrValidator.ClientID + "','" + reqModulusValidator.ClientID + "')");
               

            }
        }

    }
#endregion

    
    /// <summary>
    /// Invoke when selected index changed for TransactionType dropdownlist 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region ddlMiscInvoiceTypelist_SelectedIndexChanged Event
    protected void ddlMiscInvoiceTypelist_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        DropDownList ddlPostTransTyp = ((DropDownList)lstMiscellaneousInvoicing.InsertItem.FindControl("ddlMiscInvoiceTypelist"));
        int intPostTransTyp = Convert.ToInt32(ddlPostTransTyp.SelectedValue);
        HiddenField hfPolreq = (HiddenField)lstMiscellaneousInvoicing.InsertItem.FindControl("hfPolreq");
        RequiredFieldValidator reqSymValidator = (RequiredFieldValidator)lstMiscellaneousInvoicing.InsertItem.FindControl("reqSym");
        RequiredFieldValidator reqNbrValidator = (RequiredFieldValidator)lstMiscellaneousInvoicing.InsertItem.FindControl("reqNbr");
        RequiredFieldValidator reqModulusValidator = (RequiredFieldValidator)lstMiscellaneousInvoicing.InsertItem.FindControl("reqModulus");
        //if (intPostTransTyp != 0)
        //{
        //    bool ispolReq = new PostingTransactionTypeBS().IsPolicyRequires(Convert.ToInt32(ddlPostTransTyp.SelectedValue));
        //    reqModulusValidator.Enabled = ispolReq;
        //    reqNbrValidator.Enabled = ispolReq;
        //    reqSymValidator.Enabled = ispolReq;
        //}
        if (intPostTransTyp != 0)
        {
            //bool ispolReq = new PostingTransactionTypeBS().IsPolicyRequires(Convert.ToInt32(ddlPostTransTyp.SelectedValue));
            //reqModulusValidator.Enabled = ispolReq;
            //reqNbrValidator.Enabled = ispolReq;
            //reqSymValidator.Enabled = ispolReq;
            int intPKID = Convert.ToInt32(ddlPostTransTyp.SelectedValue);
            PostingTransactionTypeBE PostTrnsBE = (new PostingTransactionTypeBS()).LoadData(intPKID);
            if (PostTrnsBE.POL_REQR_IND.HasValue)
                hfPolreq.Value = PostTrnsBE.POL_REQR_IND.ToString();

        }
        else
        {
            hfPolreq.Value = "";
        }

    }
    #endregion
    #region ddlMiscInvoiceTypelistEdit_SelectedIndexChanged Event
    protected void ddlMiscInvoiceTypelistEdit_SelectedIndexChanged(object sender, EventArgs e)
    {

        DropDownList ddlPostTransTyp = ((DropDownList)lstMiscellaneousInvoicing.Items[lstMiscellaneousInvoicing.EditIndex].FindControl("ddlMiscInvoiceTypelist"));
        int intPostTransTyp = Convert.ToInt32(ddlPostTransTyp.SelectedValue);
        HiddenField hfPolreq = (HiddenField)lstMiscellaneousInvoicing.Items[lstMiscellaneousInvoicing.EditIndex].FindControl("hfPolreq");
        RequiredFieldValidator reqSymValidator = (RequiredFieldValidator)lstMiscellaneousInvoicing.Items[lstMiscellaneousInvoicing.EditIndex].FindControl("reqSym");
        RequiredFieldValidator reqNbrValidator = (RequiredFieldValidator)lstMiscellaneousInvoicing.Items[lstMiscellaneousInvoicing.EditIndex].FindControl("reqNbr");
        RequiredFieldValidator reqModulusValidator = (RequiredFieldValidator)lstMiscellaneousInvoicing.Items[lstMiscellaneousInvoicing.EditIndex].FindControl("reqModulus");
        if (intPostTransTyp != 0)
        {
            //bool ispolReq = new PostingTransactionTypeBS().IsPolicyRequires(Convert.ToInt32(ddlPostTransTyp.SelectedValue));
            //reqModulusValidator.Enabled = ispolReq;
            //reqNbrValidator.Enabled = ispolReq;
            //reqSymValidator.Enabled = ispolReq;
            int intPKID = Convert.ToInt32(ddlPostTransTyp.SelectedValue);
            PostingTransactionTypeBE PostTrnsBE = (new PostingTransactionTypeBS()).LoadData(intPKID);
            if(PostTrnsBE.POL_REQR_IND.HasValue)
            hfPolreq.Value = PostTrnsBE.POL_REQR_IND.ToString();
            
        }
        else
        {
            hfPolreq.Value = "";
        }

    }
    #endregion
}
#endregion
