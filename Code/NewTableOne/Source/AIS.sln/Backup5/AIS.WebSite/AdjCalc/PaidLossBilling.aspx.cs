//Default namespaces in AdjCalc_PaidLossBilling screen
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
//Importing different AIS framework namespaces for AdjCalc_PaidLossBilling screen
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.AIS.DAL.Logic;

/// <summary>
/// This is the class which is used to give the information about the AdjCalc_PaidLossBilling and it 
/// also inherits AISBase Page,so that some of the common functionality needed for all pages is implemented in
/// the Basepage
/// </summary>
#region AdjCalc_PaidLossBilling Class
public partial class AdjCalc_PaidLossBilling : AISBasePage
{
    #region Properties and variables Decalration Section
    /// <summary>
    /// Static Variables
    /// </summary>
    private static int intAccountID;
    private static int intPrmAdjPrdID;
    private static DateTime dtValDate;
    private static int intPrmAdjPrgID;
    private static int intPrmAdjID;
    private static string strProgramPeriod;

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
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        //Eventhandlers from User control
        this.ARS.AdjustmentReviewSearchButtonClicked += new EventHandler(btnSearch_AdjustmentReviewSearchButtonClicked);
        this.ARS.ARProgramPeriodSelectedIndexChanged += new EventHandler(ddlprogramPeriod_ARProgramPeriodSelectedIndexChanged);
       
        // To display the title of the page
        this.Master.Page.Title = "Paid Loss Billing";
        if (!IsPostBack)
        {
            try
            {
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
            intPrmAdjPrdID = premAdjPerdBE[premAdjPerdBE.Count - 1].PREM_ADJ_PERD_ID;
        }
        //Retrieving the Status Type ID of the adjustment Selected
        PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
        IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;

        objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(intPrmAdjID);
        objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
        this.AdjStatusTypeID = objPremStsBE.ADJ_STS_TYP_ID != null ? Convert.ToInt32(objPremStsBE.ADJ_STS_TYP_ID) : 0;
       
        BindListView();


    }
    #endregion

    #region Maintaining Session for PaidLossBillingBE
    private IList<PaidLossBillingBE> plbBEList
    {
        get
        {
            //if (Session["plbBEList"] == null)
            //    Session["plbBEList"] = new List<PaidLossBillingBE>();
            //return (IList<PaidLossBillingBE>)Session["plbBEList"];
            if (RetrieveObjectFromSessionUsingWindowName("plbBEList") == null)
                SaveObjectToSessionUsingWindowName("plbBEList", new List<PaidLossBillingBE>());
            return (IList<PaidLossBillingBE>)RetrieveObjectFromSessionUsingWindowName("plbBEList");
        }
        set 
        { 
            //Session["plbBEList"] = value; 
            SaveObjectToSessionUsingWindowName("plbBEList", value);
        }
    }
    #endregion
    #region BindListview Data
    //Bind Listview Data based on Account number,Prem. Adj. Period Id
    public void BindListView()
    {
       DropDownList ddlPrgPeriod = (DropDownList)this.ARS.FindControl("ddlProgramPeriod");
        //PaidLossBillingBS plbBS = new PaidLossBillingBS();
        //IList<PaidLossBillingBE> plbBEList=new List<PaidLossBillingBE>();
        PaidLossBillingBS plbBS = new PaidLossBillingBS();
        plbBEList = plbBS.getPaidLossBillingData(intAccountID, intPrmAdjPrdID, intPrmAdjID, intPrmAdjPrgID);
        lsvplb.DataSource = plbBEList;
        lsvplb.DataBind();
        //DropDownList ddlPrgPeriod = (DropDownList)this.ARS.FindControl("ddlProgramPeriod");
        //string strProgramPeriod = ddlPrgPeriod.SelectedItem.Text;
       //If data is there in IList then assign text to label
        IList<ProgramPeriodSearchListBE> premAdjPgmBE = new List<ProgramPeriodSearchListBE>();
        premAdjPgmBE = new ProgramPeriodsBS().GetProgramPeriodsList(intPrmAdjPrgID.ToString());
        if (plbBEList.Count > 0)
        {
            lblPLSBills.Text = "PLB BILLS FROM LSI-" + premAdjPgmBE[0].STARTDATE_ENDDATE_PGMTYP.ToString();
            pnlAdjReviewEmptyData.Visible = false;
            pnlplb.Visible = true;
        }
        else
        {
            lblPLSBills.Text = "";
            pnlAdjReviewEmptyData.Visible = true;
            pnlplb.Visible = false;
        }

    }
      #endregion
        
    #region Search
    //Search Button Functionality
    //Based on selection bind the List View
    void btnSearch_AdjustmentReviewSearchButtonClicked(object sender, EventArgs e)
    {
        string strMessage = string.Empty;
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
        
        if (strMessage != "")
        {
            return;
        }
    
        intAccountID = Convert.ToInt32(ddlAccnts.SelectedValue);
        dtValDate = Convert.ToDateTime(ddlValDates.SelectedItem.ToString());
        strProgramPeriod = ddlPrgPeriod.SelectedValue.ToString();
       
        intPrmAdjPrgID=Convert.ToInt32(strProgramPeriod);
       
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
            intPrmAdjPrdID = premAdjPerdBE[premAdjPerdBE.Count - 1].PREM_ADJ_PERD_ID;
        }

        //Retrieving the Status Type ID of the adjustment Selected
        PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
        IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;

        objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(intPrmAdjID);
        objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
        this.AdjStatusTypeID = objPremStsBE.ADJ_STS_TYP_ID != null ? Convert.ToInt32(objPremStsBE.ADJ_STS_TYP_ID) : 0;
       
        BindListView();


    }
    #endregion

   
   
    // Invoked when the Update Link is clicked
    #region Update Listview Data
    protected void UpdateList(Object sender, ListViewUpdateEventArgs e)
    {
        ListView lstView = (ListView)sender;
        if (lstView.ID.ToString() == "lsvplb")
        {
            ListViewItem myItem = lsvplb.Items[e.ItemIndex];
            int intplbID = int.Parse(((HiddenField)myItem.FindControl("hidplbID")).Value.ToString());
            TextBox txtAdjIndem = (TextBox)myItem.FindControl("txtAdjIndem");
            TextBox txtAdjExps = (TextBox)myItem.FindControl("txtAdjExps");
            TextBox txtAdjustedAmt = (TextBox)myItem.FindControl("txtAdjustedAmt");
            Label lblAdjTyp = (Label)myItem.FindControl("lblAdjTyp");
            Decimal dAdjIndem = 0.0m;
            Decimal dAdjExp = 0.0m;
            Decimal dAdjustedAmt=0.0m;
            if (txtAdjIndem.Text.Trim() != "")
                dAdjIndem = Convert.ToDecimal(txtAdjIndem.Text.Replace(",", ""));
            if (txtAdjExps.Text.Trim() != "")
                dAdjExp = Convert.ToDecimal(txtAdjExps.Text.Replace(",", ""));
            if (txtAdjustedAmt.Text.Trim() != "")
                dAdjustedAmt = Convert.ToDecimal(txtAdjustedAmt.Text.Replace(",", ""));
            string strComments = (((TextBox)myItem.FindControl("txtComment")).Text);
            PaidLossBillingBS plbBS = new PaidLossBillingBS();
            PaidLossBillingBE plbBE = plbBS.getPaidLossBillingDataRow(intplbID);
            //Concurrency Issue
            PaidLossBillingBE plbBEold = (plbBEList.Where(o => o.PREM_ADJ_PAID_LOS_BIL_ID.Equals(intplbID))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(plbBEold.UPDATEDDATE), Convert.ToDateTime(plbBE.UPDATEDDATE));
            if (!con)
                return;
            //End
            plbBE.ADJ_IDNMTY_AMT = dAdjIndem;
            plbBE.ADJ_EXPS_AMT = dAdjExp;
            plbBE.ADJ_TOT_PAID_LOS_BIL_AMT = dAdjustedAmt;
            //if (lblAdjTyp.Text != "Incurred Underlayer" && lblAdjTyp.Text != "Paid Loss Underlayer" && lblAdjTyp.Text != "Incurred Loss WA" && lblAdjTyp.Text != "Paid Loss WA")
            //{
            //    plbBE.ADJ_TOT_PAID_LOS_BIL_AMT = dAdjustedAmt;
            //}
            //else
            //{
            //    plbBE.ADJ_TOT_PAID_LOS_BIL_AMT = dAdjIndem + dAdjExp;
            //}
            plbBE.CMMNT_TXT = strComments;
            plbBE.UPDATEDDATE = DateTime.Now;
            plbBE.UPDATEDUSER = CurrentAISUser.PersonID;
             try
             {
                bool Flag= plbBS.Update(plbBE);
                //ShowConcurrentConflict(Flag, plbBE.ErrorMessage);
                 lsvplb.EditIndex = -1;
                 BindListView();
            
             }
             catch (Exception ex)
             {
                  ShowError(ex.Message,ex);
             }
        }

    }
    #endregion

    // Invoked when the Cancel Link is clicked
    #region Cancel Link
    protected void CancelList(Object sender, ListViewCancelEventArgs e)
    {

        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            ListView lstView = (ListView)sender;
            if (lstView.ID.ToString() == "lsvplb")
            {
                lsvplb.EditIndex = -1;
                BindListView();
            }
        }

    }
    #endregion

    // Invoked when the Edit Link is clicked
    // Set the Listview to Editmode
    #region Edit Listview
    protected void EditList(Object sender, ListViewEditEventArgs e)
    {
        ListView lstView = (ListView)sender;
        if (lstView.ID.ToString() == "lsvplb")
        {
            lsvplb.EditIndex = e.NewEditIndex;
            BindListView();
        }

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
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";"+ ";" + ";";

        }
        if (ddlAdjNumber.SelectedIndex != 0)
        {
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";" + ddlAdjNumber.SelectedValue + ";";
        }
        else if(ddlValDates.SelectedIndex!=0)
        {
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";"+ ";";
        }
        if (ddlPrgPeriod.SelectedIndex != 0)
        {
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";" + ddlAdjNumber.SelectedValue + ";" + ddlPrgPeriod.SelectedValue;
        }
        else if(ddlAdjNumber.SelectedIndex!=0)
        {
            hidSelectedValues.Value = ddlAccnts.SelectedValue + ";" + ddlValDates.SelectedValue + ";" + ddlAdjNumber.SelectedValue + ";";
        }
        

    }
    #endregion
    

    /// <summary>
    /// Item Data Bound Event-it is called while binding each row to the listview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region ListView DataBound Event
    protected void lsvplb_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trItemTemplate");
            if (tr != null)
            {
                tr.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                tr.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            }
            
            //Restricting User to Update when the Adjustment is not in CALC Status
            int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lnkEdit");
            if (AdjStatusTypeID != intAdjCalStatusID)
            {
                lnkEdit.Enabled = false;
            }

            //Calling Javascript for calculating Total Adjusted amount
            TextBox txtAdjIndem = (TextBox)e.Item.FindControl("txtAdjIndem");
            TextBox txtAdjExps = (TextBox)e.Item.FindControl("txtAdjExps");
            TextBox txtAdjustedAmt = (TextBox)e.Item.FindControl("txtAdjustedAmt");
            if (txtAdjIndem != null)
                txtAdjIndem.Attributes.Add("onblur", "javascript:AddAdjustedAmount('" + txtAdjIndem.ClientID + "','" + txtAdjExps.ClientID + "','" + txtAdjustedAmt.ClientID + "');FormatNumWithDecAmt($get('" + txtAdjIndem.ClientID + "'),11);");
            if (txtAdjExps != null)
                txtAdjExps.Attributes.Add("onblur", "javascript:AddAdjustedAmount('" + txtAdjIndem.ClientID + "','" + txtAdjExps.ClientID + "','" + txtAdjustedAmt.ClientID + "');FormatNumWithDecAmt($get('" + txtAdjExps.ClientID + "'),11);");
            if (txtAdjustedAmt != null)
                txtAdjustedAmt.Attributes.Add("onblur", "javascript:FormatNumWithDecAmt($get('" + txtAdjustedAmt.ClientID + "'),11);");   
        }

    }
    #endregion


}
#endregion
