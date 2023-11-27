//Default namespaces in Adj. Number screen
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

//Importing different AIS framework namespaces for Adj. Number screen
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.AIS.DAL.Logic;

/// <summary>
/// This is the class which is used to give the information about the AdjustmentNumberTextMaintenance and it 
/// also inherits AISBase Page,so that some of the common functionality needed for all pages is implemented in
/// the Basepage
/// </summary>
#region AdjustmentNumberTextMaintenance Class
public partial class AdjCalc_AdjustmentNumberTextMaintenance : AISBasePage
{

    
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
        private IList<PremiumAdjustmentPeriodBE> PremAdjPerdConcurntList
        {
            get 
            { 
                //return (IList<PremiumAdjustmentPeriodBE>)Session["PremAdjPerdConcurntList"]; 
                return (IList<PremiumAdjustmentPeriodBE>)RetrieveObjectFromSessionUsingWindowName("PremAdjPerdConcurntList");
            }
            set 
            { 
                //Session["PremAdjPerdConcurntList"] = value; 
                SaveObjectToSessionUsingWindowName("PremAdjPerdConcurntList", value);  
            }

        }
        #endregion

        
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
        this.Master.Page.Title = "Adjustment Number";
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
                ShowError(ee.Message, ee);
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
        lblAdjNumberDetails.Text = "ADJUSTMENT NUMBER MAINTENANCE TEXT" + "" + "-" + premAdjPgmBE[0].STARTDATE_ENDDATE_PGMTYP.ToString();
        BindListView();

    }
        #endregion
       
        
        /// <summary>
        /// ListView Item Command Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Item Command Event
        protected void lstAdjNumber_ItemCommand(Object sender, ListViewCommandEventArgs e)
        {
            //Get Values
            TextBox txtAdjNbr = (TextBox)e.Item.FindControl("txtAdjNbr");
            TextBox txtAdjNbrTxt = (TextBox)e.Item.FindControl("txtAdjNbrTxt");
            if (e.CommandName.ToUpper() == "UPDATE")  //if it is Update
            {
                Label lblPremAdjPerdID = (Label)e.Item.FindControl("lblPremAdjPerdID");
                int intPremAdjPerdID = Convert.ToInt32(lblPremAdjPerdID.Text);
                PremiumAdjustmentPeriodBE premAdjPerdBE;
                try
                {
                    premAdjPerdBE = (new PremiumAdjustmentPeriodBS()).getPremAdjPerdRow(intPremAdjPerdID);
                    PremiumAdjustmentPeriodBE premAdjPerdConcurntBE = (PremAdjPerdConcurntList.Where(o => o.PREM_ADJ_PERD_ID.Equals(intPremAdjPerdID))).First();
                    bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(premAdjPerdBE.UPDATE_DATE), Convert.ToDateTime(premAdjPerdConcurntBE.UPDATE_DATE));
                    if (!Concurrency)
                        return;
                    premAdjPerdBE.ADJ_NBR = Convert.ToInt32(txtAdjNbr.Text);
                    premAdjPerdBE.ADJ_NBR_TXT = txtAdjNbrTxt.Text;
                    premAdjPerdBE.ADJ_NBR_MNL_OVERRID_IND = true;
                    premAdjPerdBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
                    premAdjPerdBE.UPDATE_DATE = DateTime.Now;
                    bool blupdate=(new PremiumAdjustmentPeriodBS()).Update(premAdjPerdBE);
                    
                }
                catch (RetroBaseException ee)
                {
                    ShowError(ee.Message,ee);
                }

                 //get out of the edit mode
                lstAdjNumber.EditIndex = -1;
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
        protected void lstAdjNumber_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        { }
        #endregion
        /// <summary>
        /// Item Edit Event of a ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region ItemEdit Event
        protected void lstAdjNumber_ItemEdit(Object sender, ListViewEditEventArgs e)
        {
            lstAdjNumber.EditIndex = e.NewEditIndex;
            BindListView();
            
        }
        #endregion

        /// <summary>
        /// Code for Sorting of ListView columns
        /// In this
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region ListView Sorting Event
        protected void lstAdjNumber_Sorting(object sender, ListViewSortEventArgs e)
        {
            Image imgADJUSTMENTNUMBER = (Image)lstAdjNumber.FindControl("imgADJUSTMENTNUMBER");

            imgADJUSTMENTNUMBER.Visible = true;
            IList<PremiumAdjustmentPeriodBE> PremiumAdjPeriodList = (new PremiumAdjustmentPeriodBS()).GetAdjNumberList(this.PreAdjID,this.PrgPeriodID);

            if (imgADJUSTMENTNUMBER.ToolTip == "Ascending")
            {
                PremiumAdjPeriodList = (PremiumAdjPeriodList.OrderBy(o => o.ADJ_NBR)).ToList();
                imgADJUSTMENTNUMBER.ToolTip = "Descending";
                imgADJUSTMENTNUMBER.ImageUrl = "~/images/descending.gif";
            }
            else
            {
                PremiumAdjPeriodList = (PremiumAdjPeriodList.OrderByDescending(o => o.ADJ_NBR)).ToList();
                imgADJUSTMENTNUMBER.ToolTip = "Ascending";
                imgADJUSTMENTNUMBER.ImageUrl = "~/images/ascending.gif";
            }


            lstAdjNumber.DataSource = PremiumAdjPeriodList;
            lstAdjNumber.DataBind();

        }
        #endregion
        /// <summary>
        /// Item Cancel Event of ListView
        /// When User clicks the cancel button of the listview this event is fired
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region ItemCancel Event
        protected void lstAdjNumber_ItemCancel(Object sender, ListViewCancelEventArgs e)
        {
            if (e.CancelMode == ListViewCancelMode.CancelingEdit)
            {
                lstAdjNumber.EditIndex = -1;
                BindListView();
                
            }

        }
        #endregion
        /// <summary>
        /// Search Button Click event for the user control
        /// By calling the Delegate we are calling this event of user control
        /// Search Parameters are Captured in this Event
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
            lblAdjNumberDetails.Text = "ADJUSTMENT NUMBER MAINTENANCE TEXT" + "" + "-" + ddlPrgPeriod.SelectedItem.ToString();
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
            lblAdjNumberDetails.Text = string.Empty;
            lstAdjNumber.Items.Clear();
            lstAdjNumber.Visible = false;
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
        /// In This Method we are calling GetAdjNumberList Method of Business Service By Passing 
        /// PreAdjID,PrgPeriodID
        /// </summary>
        /// <param name=""></param>
        /// <returns>void</returns>
        #region BindListView
        public void BindListView()
        {
            lstAdjNumber.Visible = true;
            try
            {
                PremAdjPerdConcurntList= PrmAdjPerd.GetAdjNumberList(this.PreAdjID,this.PrgPeriodID);
                lstAdjNumber.DataSource = PremAdjPerdConcurntList;
                lstAdjNumber.DataBind();
            }
            catch (RetroBaseException ee)
            {
                ShowError(ee.Message);
            }
        }
        #endregion
        /// <summary>
        /// Item Data Bound Event-it is called while binding each row to the listview
        /// In this Event Logic to Disable Edit Button When the Adjsutment is not in CALC Status is implemented
        /// Javascript Functions Related to Mouseover and Mouse out are called from this Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region ListView DataBound Event
        protected void lstAdjNumber_DataBoundList(Object sender, ListViewItemEventArgs e)
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
                LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lblItemEdit");
                if (AdjStatusTypeID != intAdjCalStatusID)
                {
                    lnkEdit.Enabled = false;
                }

               
            }

        }
        #endregion
}
#endregion
