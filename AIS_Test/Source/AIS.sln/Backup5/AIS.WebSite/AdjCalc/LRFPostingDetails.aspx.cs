//Default namespaces in LRFPostingDetails screen
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
//Importing different AIS framework namespaces for LRFPostingDetails screen
using System.Collections.Generic;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.AIS.DAL.Logic;


namespace ZurichNA.AIS.WebSite.AdjCalc
{
    /// <summary>
    /// This is the class which is used to give the information about the AdjCalc_LRF Posting Details and it 
    /// also inherits AISBase Page,so that some of the common functionality needed for all pages is implemented in
    /// the Basepage
    /// </summary>
    #region LRFPostingDetails Class
    public partial class LRFPostingDetails : AISBasePage
    {
        #region Variables and Properties Declaration Section
        /// <summary>
        /// Variables
        /// </summary>
        public decimal curVal;
        public decimal agrCrdtVal;
        public decimal priorYrVal;
        public decimal adjPriorYrVal;
        public decimal postingVal;


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
        /// a property for PremiumAdjLRFPostingBS Business Service Class
        /// </summary>
        /// <returns>PremiumAdjLRFPostingBS</returns>

        private PremiumAdjLRFPostingBS prmAdjLRFBS;
        private PremiumAdjLRFPostingBS PrmAdjLRFBS
        {
            get
            {
                if (prmAdjLRFBS == null)
                {
                    prmAdjLRFBS = new PremiumAdjLRFPostingBS();
                }
                return prmAdjLRFBS;
            }

            set
            {
                prmAdjLRFBS = value;
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
            //User control event
            this.ARS.AdjustmentReviewSearchButtonClicked += new EventHandler(btnSearch_AdjustmentReviewSearchButtonClicked);
            this.ARS.ARProgramPeriodSelectedIndexChanged += new EventHandler(ddlprogramPeriod_ARProgramPeriodSelectedIndexChanged);
            this.Master.Page.Title = "LRF Posting Details";
            if (!IsPostBack)
            {

                //BindLRFPosting();
                prmAdjLRFBS = new PremiumAdjLRFPostingBS();
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
                intPremAdjPerdID = premAdjPerdBE[premAdjPerdBE.Count - 1].PREM_ADJ_PERD_ID;
            }
            //Retrieving the Status Type ID of the adjustment Selected
            PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
            IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;

            objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(intPremAdjID);
            objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
            this.AdjStatusTypeID = objPremStsBE.ADJ_STS_TYP_ID != null ? Convert.ToInt32(objPremStsBE.ADJ_STS_TYP_ID) : 0;
            hidSelectedValues.Value = intAccountID + ";" + dtValDate.ToShortDateString() + ";" + intPremAdjID + ";" + strProgramPeriod;
            this.AccountID = intAccountID;
            this.PreAdjID = intPremAdjID;
            this.PrgPeriodID = intPrgPeriodID;
            this.PremAdjPerdID = intPremAdjPerdID;
            //lblLRFPosting.Visible = true;
            //pnlLRF.Visible = true;
            BindLRFPosting();
            BindTaxList();


        }
        #endregion
        /// <summary>
        ///  DataBound Event for LRFPosting Listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region ListView DataBound Event
        protected void ItemDataBoundList(Object sender, ListViewItemEventArgs e)
        {
            //PremiumAdjLRFPostingBS palpBS = new PremiumAdjLRFPostingBS();
            //IList<PremiumAdjLRFPostingBE> lst = prmAdjLRFBS.getLRFList(this.AccountID, this.PreAdjID, this.PremAdjPerdID);
            DataTable dtLRF = (new PremiumAdjLRFPostingBS().getLRFData(this.PreAdjID, this.PremAdjPerdID, this.AccountID));
            //Session["dtLRF"] = dtLRF;
            SaveObjectToSessionUsingWindowName("dtLRF", dtLRF);
            curVal = 0.0m;
            for (int intcount = 0; intcount < dtLRF.Rows.Count; intcount++)
            {
                if (dtLRF.Rows[intcount]["CURRENT AMOUNT"] != null && dtLRF.Rows[intcount]["CURRENT AMOUNT"].ToString() != "")
                {
                    curVal = curVal + Convert.ToDecimal(dtLRF.Rows[intcount]["CURRENT AMOUNT"]);
                }

            }
            Label lblCurAmount = (Label)lstLRFPosting.FindControl("lblCurrntAmt");
            if (curVal.ToString() != "")
                lblCurAmount.Text = decimal.Parse(curVal.ToString()).ToString("#,##0");
            else
                lblCurAmount.Text = "0";
            //
            agrCrdtVal = 0.0m;
            for (int intcount = 0; intcount < dtLRF.Rows.Count; intcount++)
            {
                if (dtLRF.Rows[intcount]["AGGR AMT"] != null && dtLRF.Rows[intcount]["AGGR AMT"].ToString() != "")
                {
                    agrCrdtVal = agrCrdtVal + Convert.ToDecimal(dtLRF.Rows[intcount]["AGGR AMT"]);
                }

            }
            Label lblAgrAmount = (Label)lstLRFPosting.FindControl("lblAggtCrdt");
            if (agrCrdtVal.ToString() != "")
                lblAgrAmount.Text = decimal.Parse(agrCrdtVal.ToString()).ToString("#,##0");
            else
                lblAgrAmount.Text = "0";
            //
            priorYrVal = 0.0m;

            for (int intcount = 0; intcount < dtLRF.Rows.Count; intcount++)
            {
                if (dtLRF.Rows[intcount]["PRIOR YY AMT"] != null && dtLRF.Rows[intcount]["PRIOR YY AMT"].ToString() != "")
                {
                    priorYrVal = priorYrVal + Convert.ToDecimal(dtLRF.Rows[intcount]["PRIOR YY AMT"]);
                }

            }
            Label lblPriorYrAmount = (Label)lstLRFPosting.FindControl("lblPriorYrAmt");
            if (priorYrVal.ToString() != "")
                lblPriorYrAmount.Text = decimal.Parse(priorYrVal.ToString()).ToString("#,##0");
            else
                lblPriorYrAmount.Text = "0";
            //
            adjPriorYrVal = 0.0m;
            for (int intcount = 0; intcount < dtLRF.Rows.Count; intcount++)
            {
                if (dtLRF.Rows[intcount]["ADJ PRIOR YY AMT"] != null && dtLRF.Rows[intcount]["ADJ PRIOR YY AMT"].ToString() != "")
                {
                    adjPriorYrVal = adjPriorYrVal + Convert.ToDecimal(dtLRF.Rows[intcount]["ADJ PRIOR YY AMT"]);
                }

            }
            Label lblAdjPrYrAmount = (Label)lstLRFPosting.FindControl("lblAdjPriorYrAmt");
            if (adjPriorYrVal.ToString() != "")
                lblAdjPrYrAmount.Text = decimal.Parse(adjPriorYrVal.ToString()).ToString("#,##0");
            else
                lblAdjPrYrAmount.Text = "0";
            //
            postingVal = 0.0m;
            for (int intcount = 0; intcount < dtLRF.Rows.Count; intcount++)
            {
                if (dtLRF.Rows[intcount]["POST AMT"] != null && dtLRF.Rows[intcount]["POST AMT"].ToString() != "")
                {
                    postingVal = postingVal + Convert.ToDecimal(dtLRF.Rows[intcount]["POST AMT"]);
                }

            }
            Label lblPostingAmount = (Label)lstLRFPosting.FindControl("lblPostingAmt");
            if (postingVal.ToString() != "")
                lblPostingAmount.Text = decimal.Parse(postingVal.ToString()).ToString("#,##0");
            else
                lblPostingAmount.Text = "0";
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                //Restricting User to Update when the Adjustment is not in CALC Status
                int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
                LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lnkEdit");
                if (AdjStatusTypeID != intAdjCalStatusID)
                {
                    lnkEdit.Enabled = false;
                }
                //Calling Javascript for calculating Posting Amount
                Label lblCurrent = (Label)e.Item.FindControl("lblCurrent");
                TextBox txtAggtAmt = (TextBox)e.Item.FindControl("txtAggtAmt");
                TextBox txtAdjPriorYrAmt = (TextBox)e.Item.FindControl("txtAdjPriorYrAmt");
                TextBox txtPostingAmt = (TextBox)e.Item.FindControl("txtPostingAmt");
                HiddenField hidAdjPriorYrAmt = (HiddenField)e.Item.FindControl("hidAdjPriorYrAmt");
                Label lblTransText = (Label)e.Item.FindControl("lblRecvableTyp");
                if (lblTransText != null && txtAdjPriorYrAmt != null)
                {
                    if (lblTransText.Text.Trim() == "Reserves")
                    {
                        txtAdjPriorYrAmt.Enabled = false;
                    }
                }
                if (lblCurrent != null)
                    lblCurrent.Attributes.Add("onblur", "javascript:AddPostingAmount('" + lblCurrent.ClientID + "','" + txtAggtAmt.ClientID + "','" + txtAdjPriorYrAmt.ClientID + "','" + txtPostingAmt.ClientID + "');");
                if (txtAggtAmt != null)
                {
                    txtAggtAmt.Attributes.Add("onblur", "javascript:AddPostingAmount('" + lblCurrent.ClientID + "','" + txtAggtAmt.ClientID + "','" + txtAdjPriorYrAmt.ClientID + "','" + txtPostingAmt.ClientID + "');javascript:Getfocus('" + txtAdjPriorYrAmt.ClientID + "','" + lblTransText.ClientID + "');");
                    //txtAggtAmt.Attributes.Add("onblur", "javascript:FormatNumWithDecAmt($get('" + txtAggtAmt.ClientID + "'),11);");
                    txtAggtAmt.Attributes.Add("onfocus", "javascript:FormatNumNoDecAmt($get('" + txtAggtAmt.ClientID + "'));");
                }
                if (txtAdjPriorYrAmt != null)
                {
                    txtAdjPriorYrAmt.Attributes.Add("onblur", "javascript:AddPostingAmount('" + lblCurrent.ClientID + "','" + txtAggtAmt.ClientID + "','" + txtAdjPriorYrAmt.ClientID + "','" + txtPostingAmt.ClientID + "');javascript:Getfocus('" + txtAggtAmt.ClientID + "','" + lblTransText.ClientID + "');javascript:CalcReserve('" + txtAdjPriorYrAmt.ClientID + "','" + hidAdjPriorYrAmt.ClientID + "','" + lblTransText.ClientID + "');");
                    //txtAdjPriorYrAmt.Attributes.Add("onblur", "javascript:FormatNumWithDecAmt($get('" + txtAdjPriorYrAmt.ClientID + "'),11);");
                    txtAdjPriorYrAmt.Attributes.Add("onfocus", "javascript:FormatNumNoDecAmt($get('" + txtAdjPriorYrAmt.ClientID + "'));");
                }


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
            this.AdjStatusTypeID = objPremStsBE.ADJ_STS_TYP_ID != null ? Convert.ToInt32(objPremStsBE.ADJ_STS_TYP_ID) : 0;
            hidSelectedValues.Value = intAccountID + ";" + dtValDate.ToShortDateString() + ";" + intPremAdjID + ";" + strProgramPeriod;
            this.AccountID = intAccountID;
            this.PreAdjID = intPremAdjID;
            this.PrgPeriodID = intPrgPeriodID;
            this.PremAdjPerdID = intPremAdjPerdID;
            //lblLRFPosting.Visible = true;
            //pnlLRF.Visible = true;
            BindLRFPosting();
            BindTaxList();

        }
        #endregion


        /// <summary>
        /// Function to Display LRFPosting Elements List
        /// </summary>
        #region BindListView
        void BindLRFPosting()
        {

            //prmAdjLRFBS = new PremiumAdjLRFPostingBS();
            //IList<PremiumAdjLRFPostingBE> premAdjLRFBE = new List<PremiumAdjLRFPostingBE>();
            //premAdjLRFBE = prmAdjLRFBS.getLRFList(this.AccountID, this.PreAdjID, this.PremAdjPerdID);
            //if (premAdjLRFBE.Count > 0)
            //{
            //    lblLRFPosting.Visible = true;
            //    pnlLRF.Visible = true;

            //}
            //else
            //{
            //    lstLRFPosting.Items.Clear();
            //    lblLRFPosting.Visible = true;
            //    pnlLRF.Visible = false;

            //}
            DataTable dtLRF = (new PremiumAdjLRFPostingBS().getLRFData(this.PreAdjID, this.PremAdjPerdID, this.AccountID));
            //Session["dtLRF"] = dtLRF;
            SaveObjectToSessionUsingWindowName("dtLRF", dtLRF);
            lblLRFPosting.Visible = true;
            pnlLRF.Visible = true;
            //lstLRFPosting.DataSource = premAdjLRFBE;
            lstLRFPosting.DataSource = dtLRF;
            lstLRFPosting.DataBind();

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
            //lblLRFPosting.Text = string.Empty;
            pnlLRF.Visible = false;
            lstLRFPosting.Items.Clear();
            lblLRFPosting.Visible = false;

            lblLRFPostingTax.Visible = false;
            lblnote.Visible = false;
            lstLRFPosting.Items.Clear();
            pnlLRFTaxGrid.Visible = false;

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
        /// Item Edit Event of a ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region ItemEdit Event
        protected void EditList(Object sender, ListViewEditEventArgs e)
        {
            lstLRFPosting.EditIndex = e.NewEditIndex;
            BindLRFPosting();
            BindTaxList();

        }
        #endregion
        // Invoked when the Cancel Link is clicked
        /// <summary>
        /// Item Cancel Event of ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region ItemCancel Event
        protected void CancelList(Object sender, ListViewCancelEventArgs e)
        {
            lstLRFPosting.EditIndex = -1;
            BindLRFPosting();
            BindTaxList();
        }
        #endregion
        // Invoked when the Update Link is clicked
        /// <summary>
        /// Item Updating Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region ItemUpdating event
        protected void UpdateList(Object sender, ListViewUpdateEventArgs e)
        {

            decimal Limit_amt = 0.0m;
            prmAdjLRFBS = new PremiumAdjLRFPostingBS();
            ListViewItem myItem = lstLRFPosting.Items[e.ItemIndex];
            PremiumAdjLRFPostingBE PremumBE = new PremiumAdjLRFPostingBE();
            //AcctSetupQCBS AcctQCBS = new AcctSetupQCBS();
            int intpremAdjLRFID = int.Parse(((HiddenField)myItem.FindControl("hidPremAdjLRFID")).Value.ToString());

            //int intpremAdjLRFID = int.Parse(prmLRF.Text);
            PremumBE = prmAdjLRFBS.getPreAdjLRFRow(intpremAdjLRFID);
            //Concurrency Chk
            DataTable dtLRFNew = new DataTable();
            //dtLRFNew = (DataTable)Session["dtLRF"];
            dtLRFNew = (DataTable)RetrieveObjectFromSessionUsingWindowName("dtLRF");
            if (dtLRFNew.Rows.Count > 0)
            {
                if (dtLRFNew.Rows[e.ItemIndex]["CURRENT AMOUNT"].ToString() != PremumBE.CurrntAmount.ToString() ||
                    dtLRFNew.Rows[e.ItemIndex]["AGGR AMT"].ToString() != PremumBE.AggrgateAmt.ToString() ||
                    dtLRFNew.Rows[e.ItemIndex]["ADJ PRIOR YY AMT"].ToString() != PremumBE.AdjustPriorYrAmt.ToString())
                {
                    ShowError(GlobalConstants.ErrorMessage.RowNotFoundOrChanged);
                    return;
                }
            }
            //End
            //if (((TextBox)myItem.FindControl("txtCurrent")).Text != "")
            //    Limit_amt=Convert.ToDecimal(((TextBox)myItem.FindControl("txtCurrent")).Text.Replace(",", ""));
            if (((Label)myItem.FindControl("lblCurrent")).Text != "")
                Limit_amt = Convert.ToDecimal(((Label)myItem.FindControl("lblCurrent")).Text.Replace(",", ""));
            if (((TextBox)myItem.FindControl("txtAggtAmt")).Text != "")
            {
                PremumBE.AggrgateAmt = Convert.ToDecimal(((TextBox)myItem.FindControl("txtAggtAmt")).Text.Replace(",", ""));
                Limit_amt = Limit_amt + Convert.ToDecimal(((TextBox)myItem.FindControl("txtAggtAmt")).Text.Replace(",", ""));
            }
            else
            {
                PremumBE.AggrgateAmt = 0;
            }
            PremumBE.lim_amt = Limit_amt;
            int intAdjPriorYrAmt = 0;
            if (((TextBox)myItem.FindControl("txtAdjPriorYrAmt")).Text != "")
            {
                PremumBE.AdjustPriorYrAmt = Convert.ToDecimal(((TextBox)myItem.FindControl("txtAdjPriorYrAmt")).Text.Replace(",", ""));
                intAdjPriorYrAmt = Convert.ToInt32(((TextBox)myItem.FindControl("txtAdjPriorYrAmt")).Text.Replace(",", ""));
            }
            else
            {
                PremumBE.AdjustPriorYrAmt = 0;
            }
            if (((TextBox)myItem.FindControl("txtPostingAmt")).Text != "")
                PremumBE.PostedAmt = Convert.ToDecimal(((TextBox)myItem.FindControl("txtPostingAmt")).Text.Replace(",", ""));
            PremumBE.UpdatedDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            PremumBE.UpdatedUser_ID = CurrentAISUser.PersonID;
            bool Flag = prmAdjLRFBS.Update(PremumBE);
            if (Flag)
            {
                PremiumAdjLRFPostingBE PremILRFBE = new PremiumAdjLRFPostingBE();
                int intPremLRFID = prmAdjLRFBS.getPreAdjLRFReserveID(PremumBE.PremAdjPerdID);
                int intAdjPriorYrAmtActual = 0;
                int intPostingResult = 0;
                int intAdjustPriorYrAmtReserveResult = 0;
                int intAdjustPriorYrAmtReserve = 0;
                if (((HiddenField)myItem.FindControl("hidAdjPriorYrAmt")).Value != "")
                    intAdjPriorYrAmtActual = Convert.ToInt32(((HiddenField)myItem.FindControl("hidAdjPriorYrAmt")).Value.Replace(",", ""));
                PremILRFBE = prmAdjLRFBS.getPreAdjLRFRow(intPremLRFID);
                if (PremILRFBE.CurrntAmount != null)
                    intPostingResult += Convert.ToInt32(PremILRFBE.CurrntAmount);
                if (PremILRFBE.AggrgateAmt != null)
                    intPostingResult += Convert.ToInt32(PremILRFBE.AggrgateAmt);
                if (PremILRFBE.AdjustPriorYrAmt != null)
                    intAdjustPriorYrAmtReserve = Convert.ToInt32(PremILRFBE.AdjustPriorYrAmt);
                intAdjustPriorYrAmtReserveResult = intAdjustPriorYrAmtReserve + (intAdjPriorYrAmtActual - intAdjPriorYrAmt);
                intPostingResult -= intAdjustPriorYrAmtReserveResult;
                PremILRFBE.AdjustPriorYrAmt = intAdjustPriorYrAmtReserveResult;
                PremILRFBE.PostedAmt = intPostingResult;

                Flag = prmAdjLRFBS.Update(PremILRFBE);
            }
            lstLRFPosting.EditIndex = -1;
            if (Flag)
            {
                BindLRFPosting();
                BindTaxList();
            }
        }
        #endregion
        #region Texas Tax:LRF Posting Tax Grid
        /// <summary>
        /// Function to Display LRFPosting Tax Grid Elements List
        /// </summary>
        #region BindTaxList
        void BindTaxList()
        {
            IList<PremiumAdjLRFPostingTaxBE> lstLRFTax = (new PremiumAdjLRFPostingTaxBS().GetPrmAdjLRFPostingTax(this.PreAdjID, this.PremAdjPerdID, this.PrgPeriodID));
            //Session["dtLRFTax"] = lstLRFTax;
            if (lstLRFTax.Count > 0)
            {
                
                lblLRFPostingTax.Visible = true;
                lblnote.Visible = true;
                pnlLRFTaxGrid.Visible = true;
            }
            else
            {
                lblLRFPostingTax.Visible = false;
                lblnote.Visible = false;
                pnlLRFTaxGrid.Visible = false;
            }
            lstLRFTaxGrid.DataSource = lstLRFTax;
            lstLRFTaxGrid.DataBind();

        }
        #endregion
        /// <summary>
        ///  DataBound Event for LRFPosting Tax Brid Listview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Tax ListView DataBound Event
        protected void lstLRFTaxGrid_ItemDataBound(Object sender, ListViewItemEventArgs e)
        {
            #region TaxTotals
            IList<PremiumAdjLRFPostingTaxBE> lstLRFTax = (new PremiumAdjLRFPostingTaxBS().GetPrmAdjLRFPostingTax(this.PreAdjID, this.PremAdjPerdID, this.PrgPeriodID));
            decimal? currentVal = 0.0m;
            currentVal = lstLRFTax.Sum(p => p.CURR_AMT);
            decimal? aggrCreditVal = 0.0m;
            aggrCreditVal = lstLRFTax.Sum(p => p.AGGR_AMT);
            decimal? priorYrAmt = 0.0m;
            priorYrAmt = lstLRFTax.Sum(p => p.PRIOR_YY_AMT);
            decimal? adjPriorYrAmt = 0.0m;
            adjPriorYrAmt = lstLRFTax.Sum(p => p.ADJ_PRIOR_YY_AMT);
            decimal? postingAmtVal = 0.0m;
            postingAmtVal = lstLRFTax.Sum(p => p.POST_AMT);
            #endregion
            //#region Receivable Totals
            //DataTable dtLRF = (new PremiumAdjLRFPostingBS().getLRFData(this.PreAdjID, this.PremAdjPerdID, this.AccountID));
            //curVal = 0.0m;
            //for (int intcount = 0; intcount < dtLRF.Rows.Count; intcount++)
            //{
            //    if (dtLRF.Rows[intcount]["CURRENT AMOUNT"] != null && dtLRF.Rows[intcount]["CURRENT AMOUNT"].ToString() != "")
            //    {
            //        curVal = curVal + Convert.ToDecimal(dtLRF.Rows[intcount]["CURRENT AMOUNT"]);
            //    }

            //}
           
            ////
            //agrCrdtVal = 0.0m;
            //for (int intcount = 0; intcount < dtLRF.Rows.Count; intcount++)
            //{
            //    if (dtLRF.Rows[intcount]["AGGR AMT"] != null && dtLRF.Rows[intcount]["AGGR AMT"].ToString() != "")
            //    {
            //        agrCrdtVal = agrCrdtVal + Convert.ToDecimal(dtLRF.Rows[intcount]["AGGR AMT"]);
            //    }

            //}
           
            ////
            //priorYrVal = 0.0m;

            //for (int intcount = 0; intcount < dtLRF.Rows.Count; intcount++)
            //{
            //    if (dtLRF.Rows[intcount]["PRIOR YY AMT"] != null && dtLRF.Rows[intcount]["PRIOR YY AMT"].ToString() != "")
            //    {
            //        priorYrVal = priorYrVal + Convert.ToDecimal(dtLRF.Rows[intcount]["PRIOR YY AMT"]);
            //    }

            //}
            
            ////
            //adjPriorYrVal = 0.0m;
            //for (int intcount = 0; intcount < dtLRF.Rows.Count; intcount++)
            //{
            //    if (dtLRF.Rows[intcount]["ADJ PRIOR YY AMT"] != null && dtLRF.Rows[intcount]["ADJ PRIOR YY AMT"].ToString() != "")
            //    {
            //        adjPriorYrVal = adjPriorYrVal + Convert.ToDecimal(dtLRF.Rows[intcount]["ADJ PRIOR YY AMT"]);
            //    }

            //}
            
            ////
            //postingVal = 0.0m;
            //for (int intcount = 0; intcount < dtLRF.Rows.Count; intcount++)
            //{
            //    if (dtLRF.Rows[intcount]["POST AMT"] != null && dtLRF.Rows[intcount]["POST AMT"].ToString() != "")
            //    {
            //        postingVal = postingVal + Convert.ToDecimal(dtLRF.Rows[intcount]["POST AMT"]);
            //    }

            //}
            //#endregion
            #region Assign Tax Total Label Values
            Label lblCurrentTotal = (Label)lstLRFTaxGrid.FindControl("lblCurrentTotal");
            if (lblCurrentTotal != null)
                lblCurrentTotal.Text = decimal.Parse(currentVal.ToString()).ToString("#,##0");
            Label lblAggregateCreditTotal = (Label)lstLRFTaxGrid.FindControl("lblAggregateCreditTotal");
            if (lblAggregateCreditTotal != null)
                lblAggregateCreditTotal.Text = decimal.Parse(aggrCreditVal.ToString()).ToString("#,##0");
            Label lblPriorYearTotal = (Label)lstLRFTaxGrid.FindControl("lblPriorYearTotal");
            if (lblPriorYearTotal != null)
                lblPriorYearTotal.Text = decimal.Parse(priorYrAmt.ToString()).ToString("#,##0");
            Label lblAdjustedPriorYearTotal = (Label)lstLRFTaxGrid.FindControl("lblAdjustedPriorYearTotal");
            if (lblAdjustedPriorYearTotal != null)
                lblAdjustedPriorYearTotal.Text = decimal.Parse(adjPriorYrAmt.ToString()).ToString("#,##0");
            Label lblPostingAmountTotal = (Label)lstLRFTaxGrid.FindControl("lblPostingAmountTotal");
            if (lblPostingAmountTotal != null)
                lblPostingAmountTotal.Text = decimal.Parse(postingAmtVal.ToString()).ToString("#,##0");

            //For Edit of Deductible Taxes
            ListViewDataItem dataItem = (ListViewDataItem)e.Item;
           
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    //Restricting User to Update when the Adjustment is not in CALC Status
                    int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
                    LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lnkEdit");
                    if (AdjStatusTypeID != intAdjCalStatusID)
                    {
                        
                        lnkEdit.Enabled = false;
                    }
                    if (dataItem.DisplayIndex == lstLRFTaxGrid.EditIndex)
                    {
                    //Calling Javascript for calculating Posting Amount
                    Label lblCurrent = (Label)e.Item.FindControl("lblCurrent");
                    //Label lblAggregateCredit = (Label)e.Item.FindControl("lblAggregateCredit");
                    //TextBox txtAggtAmt = (TextBox)e.Item.FindControl("txtAggtAmt");
                    TextBox txtAdjPriorYrAmt = (TextBox)e.Item.FindControl("txtAdjPriorYrAmt");
                    TextBox txtPostingAmt = (TextBox)e.Item.FindControl("txtPostingAmt");
                    if (lblCurrent != null)
                        lblCurrent.Attributes.Add("onblur", "javascript:AddTaxGridPostingAmount('" + lblCurrent.ClientID + "','" + txtAdjPriorYrAmt.ClientID + "','" + txtPostingAmt.ClientID + "');");
                    //if (txtAggtAmt != null)
                    //{
                    //    txtAggtAmt.Attributes.Add("onblur", "javascript:AddTaxGridPostingAmount('" + lblCurrent.ClientID + "','" + txtAggtAmt.ClientID + "','" + txtAdjPriorYrAmt.ClientID + "','" + txtPostingAmt.ClientID + "');javascript:GetTaxfocus('" + txtAdjPriorYrAmt.ClientID + "');");
                    //    //txtAggtAmt.Attributes.Add("onblur", "javascript:FormatNumWithDecAmt($get('" + txtAggtAmt.ClientID + "'),11);");
                    //    txtAggtAmt.Attributes.Add("onfocus", "javascript:FormatNumNoDecAmt($get('" + txtAggtAmt.ClientID + "'));");
                    //}
                    if (txtAdjPriorYrAmt != null)
                    {
                        txtAdjPriorYrAmt.Attributes.Add("onblur", "javascript:AddTaxGridPostingAmount('" + lblCurrent.ClientID + "','" + txtAdjPriorYrAmt.ClientID + "','" + txtPostingAmt.ClientID + "');");
                        txtAdjPriorYrAmt.Attributes.Add("onfocus", "javascript:FormatNumNoDecAmt($get('" + txtAdjPriorYrAmt.ClientID + "'));");
                        //txtAdjPriorYrAmt.Focus();
                    }
                }
            }
        
            #endregion
            #region Assign Grand Total Label Values
            Label lblGrandTotCurrent = (Label)lstLRFTaxGrid.FindControl("lblGrandTotCurrent");
            if (lblGrandTotCurrent != null)
                lblGrandTotCurrent.Text = decimal.Parse((currentVal).ToString()).ToString("#,##0");
            Label lblGrandTotAggrCredit = (Label)lstLRFTaxGrid.FindControl("lblGrandTotAggrCredit");
            if (lblGrandTotAggrCredit != null)
                lblGrandTotAggrCredit.Text = decimal.Parse((aggrCreditVal).ToString()).ToString("#,##0");
            Label lblGrandTotPriorYear = (Label)lstLRFTaxGrid.FindControl("lblGrandTotPriorYear");
            if (lblGrandTotPriorYear != null)
                lblGrandTotPriorYear.Text = decimal.Parse((priorYrAmt).ToString()).ToString("#,##0");
            Label lblGrandTotAdjPriorYear = (Label)lstLRFTaxGrid.FindControl("lblGrandTotAdjPriorYear");
            if (lblGrandTotAdjPriorYear != null)
                lblGrandTotAdjPriorYear.Text = decimal.Parse((adjPriorYrAmt).ToString()).ToString("#,##0");
            Label lblGrandTotPostingAmount = (Label)lstLRFTaxGrid.FindControl("lblGrandTotPostingAmount");
            if (lblGrandTotPostingAmount != null)
                lblGrandTotPostingAmount.Text = decimal.Parse((postingAmtVal).ToString()).ToString("#,##0");  
            #endregion
        }
        #endregion
        // Invoked when the Cancel Link is clicked
        /// <summary>
        /// Item Cancel Event of ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region ItemCancel Event
        protected void lstLRFTaxGrid_ItemCanceling(Object sender, ListViewCancelEventArgs e)
        {
            lstLRFTaxGrid.EditIndex = -1;
            BindTaxList();
        }
        #endregion
        /// <summary>
        /// Item Edit Event of a ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region ItemEdit Event
        protected void lstLRFTaxGrid_ItemEditing(Object sender, ListViewEditEventArgs e)
        {
            lstLRFTaxGrid.EditIndex = e.NewEditIndex;
            BindTaxList();
        }
        #endregion
        // Invoked when the Update Link is clicked
        /// <summary>
        /// Item Updating Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region ItemUpdating event
        protected void lstLRFTaxGrid_ItemUpdate(Object sender, ListViewUpdateEventArgs e)
        {
           
        }
        #endregion
        // Invoked when Item command is raised
        /// <summary>
        /// Item Command Event for update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Item command event
        protected void lstLRFTaxGrid_ItemCommand(Object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper() == "UPDATE")
            {
                PremiumAdjLRFPostingTaxBE LRFTaxBE = new PremiumAdjLRFPostingTaxBE();
                PremiumAdjLRFPostingTaxBS LRFTaxBS = new PremiumAdjLRFPostingTaxBS();
                Label lblPremPostTaxId = (Label)(e.Item.FindControl("lblPremPostTaxId"));
                LRFTaxBE = LRFTaxBS.LoadLRFTaxData(Convert.ToInt32(lblPremPostTaxId.Text));
                //TextBox txtAggtAmt = (TextBox)e.Item.FindControl("txtAggtAmt");
                //LRFTaxBE.AGGR_AMT = Convert.ToDecimal(txtAggtAmt.Text);
                //LRFTaxBE.LIM_AMT = LRFTaxBE.CURR_AMT + Convert.ToDecimal(txtAggtAmt.Text);
                LRFTaxBE.LIM_AMT = LRFTaxBE.CURR_AMT;
                //Get the prior yr amount
                TextBox txtAdjPriorYrAmt = (TextBox)(e.Item.FindControl("txtAdjPriorYrAmt"));
                LRFTaxBE.ADJ_PRIOR_YY_AMT = Convert.ToDecimal(txtAdjPriorYrAmt.Text);
                TextBox txtPostingAmt = (TextBox)(e.Item.FindControl("txtPostingAmt"));
                LRFTaxBE.POST_AMT = Convert.ToDecimal(txtPostingAmt.Text);
                bool succeed = LRFTaxBS.Update(LRFTaxBE);
                string strError = string.Empty;
                if (succeed)
                {
                    strError = (new PremiumAdjLRFPostingTaxBS()).AddPREM_ADJ_PERD_TOT_TAX(AccountID, PremAdjPerdID, PrgPeriodID, PreAdjID, CurrentAISUser.PersonID);
                }
                lstLRFTaxGrid.EditIndex = -1;
                ShowError("The entry has been saved");
                BindTaxList();
            }
        }
        #endregion
        #endregion
    }
    #endregion
}
