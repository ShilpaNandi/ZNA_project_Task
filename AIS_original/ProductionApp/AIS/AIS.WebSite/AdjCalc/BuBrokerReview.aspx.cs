//Default namespaces in BuBroker Review screen
using System;
using System.Collections;
using System.Collections.Generic;
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
using ZurichNA.AIS.ExceptionHandling;
using System.Threading;
using ZurichNA.AIS.WebSite.Reports;
using System.IO;
using ZurichNA.LSP.Framework;
using System.Text;
using System.Text.RegularExpressions;
using ZurichNA.LSP.Framework.Business;
using System.Web.SessionState;
using System.Web.Services;
using Microsoft.Web.Services3.Security.Tokens;
using System.Transactions;
//Importing different AIS framework namespaces for BuBroker Review screen
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.DAL.LINQ;


/// <summary>
/// This is the class which is used to give the information about the BuBrokerReview and it 
/// also inherits AISBase Page,so that some of the common functionality needed for all pages is implemented in
/// the Basepage
/// </summary>
#region BuBrokerReview Class
public partial class BuBrokerReview : AISBasePage
 {

     /// <summary>
     /// Global Varaibles Declaration
     /// </summary>
     #region Variable Declaration Section
     protected static Common common = null;
     System.Data.Common.DbTransaction BuBrokertrans = null;
     AISDatabaseLINQDataContext objBuBrokerDC = new AISDatabaseLINQDataContext(ConfigurationManager.ConnectionStrings["DBConnectionLINQ"].ToString());
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
     /// A Property for Holding BrokerID in ViewState
     /// </summary>
     protected int BrokerID
     {
         get
         {
             if (ViewState["BrokerID"] != null)
             {
                 return int.Parse(ViewState["BrokerID"].ToString());
             }
             else
             {
                 ViewState["BrokerID"] = 0;
                 return 0;
             }
         }
         set
         {
             ViewState["BrokerID"] = value;
         }
     }

     /// <summary>
     /// A Property for Holding BuOfficeID in ViewState
     /// </summary>
     protected int BuOfficeID
     {
         get
         {
             if (ViewState["BuOfficeID"] != null)
             {
                 return int.Parse(ViewState["BuOfficeID"].ToString());
             }
             else
             {
                 ViewState["BuOfficeID"] = 0;
                 return 0;
             }
         }
         set
         {
             ViewState["BuOfficeID"] = value;
         }
     }

     /// <summary>
     /// A Property for Holding BrokerContactID in ViewState
     /// </summary>
     protected int BrokerContactID
     {
         get
         {
             if (ViewState["BrokerContactID"] != null)
             {
                 return int.Parse(ViewState["BrokerContactID"].ToString());
             }
             else
             {
                 ViewState["BrokerContactID"] = 0;
                 return 0;
             }
         }
         set
         {
             ViewState["BrokerContactID"] = value;
         }
     }

    /// <summary>
     /// A Property for Holding PremiumAdjustmentBEList in a session and it is used for concurrecny check
    /// </summary>
     private IList<PremiumAdjustmentBE> premAdjBEConcurntList
     {
         get { return (IList<PremiumAdjustmentBE>)Session["premAdjBEConcurntList"]; }
         set { Session["premAdjBEConcurntList"] = value; }
         
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
            this.ARS.ARAdjustmentNumberSelectedIndexChanged += new EventHandler(ddlAdjNumber_ARAdjustmentNumberSelectedIndexChanged);
            DropDownList ddlPrgPeriod = (DropDownList)this.ARS.FindControl("ddlProgramPeriod");
            ddlPrgPeriod.Visible = false;
            Label lblPrgPeriod = (Label)this.ARS.FindControl("lblProgramPeriod");
            lblPrgPeriod.Visible = false;
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
                        if (strSelectedAccountID != "" && strSelectedValDate != "" && strSelectedPremAdjID != "" )
                            fillSelectedValues(strSelectedAccountID, strSelectedValDate, strSelectedPremAdjID);
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
            list.Add(ddlBroker);
            list.Add(ddlBUOffice);
            list.Add(ddlBrokerContact);
            list.Add(btnUpdate);
            
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
        private void fillSelectedValues(string strSelectedAccountID, string strSelectedValDate, string strSelectedPremAdjID)
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
           
           
            //Retrieving the Status Type ID of the adjustment Selected
            PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
            IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;

            objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(intPremAdjID);
            objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
            this.AccountID = intAccountID;
            this.PreAdjID = intPremAdjID;
            
            this.AdjStatusTypeID = objPremStsBE.ADJ_STS_TYP_ID != null ? Convert.ToInt32(objPremStsBE.ADJ_STS_TYP_ID) : 0;
            hidSelectedValues.Value = intAccountID + ";" + dtValDate.ToShortDateString() + ";" + intPremAdjID + ";";

            BindAdjustmentDetails();
            BindProgramPeriodDetails();
            BindBuBrokerDetails();
            //Restricting User to Add or Update if Adjustment Status is not CALC
            int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            if (intAdjCalStatusID != AdjStatusTypeID)
            {
                btnUpdate.Enabled = false;
            }
            else
            {
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                    btnUpdate.Enabled = true;
            }

        }
        #endregion
        /// <summary>
        /// Onselectedindexchanged event for the user control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region Selected Index Changed Event of User Control
        void ddlAdjNumber_ARAdjustmentNumberSelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlAccnts = (DropDownList)this.ARS.FindControl("ddlAccountlist");
            DropDownList ddlValDates = (DropDownList)this.ARS.FindControl("ddlValDate");
            DropDownList ddlAdjNumber = (DropDownList)this.ARS.FindControl("ddlAdjNumber");
            lblProgramPeriodDetails.Text = string.Empty;
            lblAdjNumberDetails.Text = string.Empty;
            lblBuBrokerDetails.Text = string.Empty;
            PGMlistview.Items.Clear();
            Adjlistview.Items.Clear();
            PGMlistview.Visible = false;
            Adjlistview.Visible = false;
            pnlBuBrokerDetails.Visible = false;
            
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
            

            int intAccountID = Convert.ToInt32(ddlAccnts.SelectedValue);
            int intPremAdjID = Convert.ToInt32(ddlAdjNumber.SelectedValue);
            DateTime dtValDate = Convert.ToDateTime(ddlValDates.SelectedValue.ToString());
            
            //Retrieving the Status Type ID of the adjustment Selected
            PremiumAdjustmentStatusBE objPremStsBE = new PremiumAdjustmentStatusBE();
            IList<PremiumAdjustmentStatusBE> objPremAdjStsBE;

            objPremAdjStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusList(intPremAdjID);
            objPremStsBE = new PremiumAdjustmentStatusBS().getPreAdjStatusRow(objPremAdjStsBE[0].PremumAdj_sts_ID);
            this.AccountID = intAccountID;
            this.PreAdjID = intPremAdjID;
            this.AdjStatusTypeID = objPremStsBE.ADJ_STS_TYP_ID != null ? Convert.ToInt32(objPremStsBE.ADJ_STS_TYP_ID) : 0;
            hidSelectedValues.Value = intAccountID + ";" + dtValDate.ToShortDateString() + ";" + intPremAdjID + ";";
            this.PreAdjID = Convert.ToInt32(ddlAdjNumber.SelectedValue);
            BindAdjustmentDetails();
            BindProgramPeriodDetails();
            BindBuBrokerDetails();
            //Restricting User to Add or Update if Adjustment Status is not CALC
            int intAdjCalStatusID = new BLAccess().GetLookUpID(GlobalConstants.AdjustmentStatus.Calc, GlobalConstants.AdjustmentStatus.AdjustmentStatuses);
            if (intAdjCalStatusID != AdjStatusTypeID)
            {
                btnUpdate.Enabled = false;
            }
            else
            {
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                    btnUpdate.Enabled = true;
            }
        }
        #endregion

        /// <summary>
        /// This Method is used to bind Adjustment Listview
        /// Based on the selected adjustment number we retrieving data from respective Business layer
        /// After retrieving the Data we are assiging the related BrokerId and BuOfficeID to Properties
        /// </summary>
        #region BindAdjustmentDetails Method
        public void BindAdjustmentDetails()
        {
            lblAdjNumberDetails.Text = "Adjustment Details";
            Adjlistview.Visible = true;
            IList<PremiumAdjustmentBE> premAdjBEList = new List<PremiumAdjustmentBE>();
            premAdjBEList = new PremAdjustmentBS().getBuBrokerReviewAdjustmentInfo(this.PreAdjID);
            Adjlistview.DataSource = premAdjBEList;
            Adjlistview.DataBind();
            premAdjBEConcurntList = premAdjBEList;

            this.BrokerID = Convert.ToInt32(premAdjBEList[0].BROKER_ID);
            this.BuOfficeID = Convert.ToInt32(premAdjBEList[0].BU_OFF_ID);

        }
        #endregion

        /// <summary>
        /// This Method is used to bind Program Period Listview
        /// Based on the selected adjustment number we retrieving data from respective Business layer
        /// After retrieving the Data we are changing the listview panel height based on no of rows.
        /// </summary>
        #region BindProgramPeriodDetails Method
        public void BindProgramPeriodDetails()
        {
            lblProgramPeriodDetails.Text = "Program Period Details";
            PGMlistview.Visible = true;
            IList<ProgramPeriodBE> programPeriodBEList = new List<ProgramPeriodBE>();
            programPeriodBEList = new ProgramPeriodsBS().GetBuBrokerReviewProgramPeriodList(this.PreAdjID);
            PGMlistview.DataSource = programPeriodBEList;
            PGMlistview.DataBind();

            if (programPeriodBEList.Count <= 4)
                pnlPGMlistview.Height = Unit.Point(80);
            else
                pnlPGMlistview.Height = Unit.Point(120);


        }
        #endregion

        /// <summary>
        /// This Method is used to bind the data to Broker and BUOffice drop down lists by calling respective methods
        /// After Binding Data we are calling BindDDLBrokerContacts Method to fill the Broker contact with respective brokerID
        /// </summary>
        #region BindBuBrokerDetails Method
        public void BindBuBrokerDetails()
        {
            lblBuBrokerDetails.Text = "BU Broker Details";
            pnlBuBrokerDetails.Visible = true;

            ddlBroker.DataSourceID = "BrokerDataSource";
            ddlBroker.DataTextField = "LookUpNAME";
            ddlBroker.DataValueField = "LookUpID";
            ddlBroker.DataBind();

            ddlBUOffice.DataSourceID = "BUOfficeDataSource";
            ddlBUOffice.DataTextField = "LookUpNAME";
            ddlBUOffice.DataValueField = "LookUpID";
            ddlBUOffice.DataBind();

            ddlBroker.Items.FindByValue(this.BrokerID.ToString()).Selected = true;
            ddlBUOffice.Items.FindByValue(this.BuOfficeID.ToString()).Selected = true;
            BindDDLBrokerContacts(this.BrokerID);
        }
        #endregion

        /// <summary>
        /// This event is fired when binding each row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region DataBoundList Event
        protected void DataBoundList(Object sender, ListViewItemEventArgs e)
        {
        }
        #endregion

        /// <summary>
        /// Code for Sorting of ListView columns
        /// In this
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region ListView Sorting Event
        protected void PGMlistview_Sorting(object sender, ListViewSortEventArgs e)
        {
            Image imgBROKERNAME = (Image)PGMlistview.FindControl("imgBROKERNAME");
            Image imgBUSINESSUNITNAME = (Image)PGMlistview.FindControl("imgBUSINESSUNITNAME");

            IList<ProgramPeriodBE> programPeriodBEList = new List<ProgramPeriodBE>();
            programPeriodBEList = new ProgramPeriodsBS().GetBuBrokerReviewProgramPeriodList(this.PreAdjID);
            Image img = new Image();
            switch (e.SortExpression)
            {
                
                case "BROKERNAME":
                    imgBROKERNAME.Visible = true;
                    imgBUSINESSUNITNAME.Visible = false;
                    img = imgBROKERNAME;
                    if (img.ToolTip == "Ascending")
                    {
                        programPeriodBEList = (programPeriodBEList.OrderBy(o => o.BROKER)).ToList();
                        img.ToolTip = "Descending";
                        img.ImageUrl = "~/images/descending.gif";

                    }
                    else
                    {
                        programPeriodBEList = (programPeriodBEList.OrderByDescending(o => o.BROKER)).ToList();
                        img.ToolTip = "Ascending";
                        img.ImageUrl = "~/images/Ascending.gif";

                    }
                    break;
                case "BUSINESSUNITNAME":
                    imgBROKERNAME.Visible = false;
                    imgBUSINESSUNITNAME.Visible = true;
                    img = imgBUSINESSUNITNAME;
                    if (img.ToolTip == "Ascending")
                    {
                        programPeriodBEList = (programPeriodBEList.OrderBy(o => o.BUOFFFICE)).ToList();
                        img.ToolTip = "Descending";
                        img.ImageUrl = "~/images/descending.gif";

                    }
                    else
                    {
                        programPeriodBEList = (programPeriodBEList.OrderByDescending(o => o.BUOFFFICE)).ToList();
                        img.ToolTip = "Ascending";
                        img.ImageUrl = "~/images/Ascending.gif";

                    }
                    break;
              
            }

            PGMlistview.DataSource = programPeriodBEList;
            PGMlistview.DataBind();

            if (programPeriodBEList.Count <= 4)
                pnlPGMlistview.Height = Unit.Point(80);
            else
                pnlPGMlistview.Height = Unit.Point(120);

            

        }
        #endregion
        /// <summary>
        /// event fires when User selected Broker in dropdown        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region ddlBroker SelectedIndexChanged Event
        protected void ddlBroker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.Parse(ddlBroker.SelectedValue) != 0)
                // Filling Broker Contact Dropdown according to Broker dropdown selection
                BindDDLBrokerContacts(int.Parse(ddlBroker.SelectedValue));

        }
        #endregion

        /// <summary>
        /// Fill Broker Contact Dropdown according to Broker dropdown selection
        /// </summary>
        /// <param name="ExtOrgID"></param>
        #region BindDDLBrokerContacts Method
        private void BindDDLBrokerContacts(int ExtOrgID)
        {
            PersonBS person = new PersonBS();

            IList<LookupBE> ExtOrg = new List<LookupBE>();
            ExtOrg = person.getContactsByExtOrg(ExtOrgID);

            ddlBrokerContact.DataSource = ExtOrg;
            ddlBrokerContact.DataValueField = "LookUpID";
            ddlBrokerContact.DataTextField = "LookUpName";
            ddlBrokerContact.DataBind();
            if (this.BrokerContactID != 0)
            {
                ddlBrokerContact.Items.FindByValue(this.BrokerContactID.ToString()).Selected = true;
            }
            else
            {
                ddlBrokerContact.Items.FindByValue("0").Selected = true;
            }
        }
        #endregion

        /// <summary>
        /// Update button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region btnUpdate Click Event
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            
            if(ddlBUOffice.SelectedIndex==0)
            {
                string strMessage = "Please Select BU/Office";
                ShowError(strMessage);
                return;
            }
            if (ddlBroker.SelectedIndex == 0)
            {
                string strMessage = "Please Select Broker";
                ShowError(strMessage);
                return;
            }
            IList<PremiumAdjustmentBE> premAdjBEList = new List<PremiumAdjustmentBE>();
            premAdjBEList = new PremAdjustmentBS().getBuBrokerReviewAdjustmentInfo(this.PreAdjID);

            if (Convert.ToInt32(premAdjBEList[0].BROKER_ID) != Convert.ToInt32(ddlBroker.SelectedValue))
            {
                if (ddlBrokerContact.SelectedIndex == 0)
                {
                    string strMessage = "Please select Broker Contact";
                    ShowError(strMessage);
                    return;
                }
            }
            bool Concurrency = ShowConcurrentConflict(Convert.ToDateTime(premAdjBEConcurntList[0].UPDT_DT), Convert.ToDateTime(premAdjBEList[0].UPDT_DT));
            if (!Concurrency)
                return;
            this.BuOfficeID = Convert.ToInt32(ddlBUOffice.SelectedValue);
            this.BrokerID = Convert.ToInt32(ddlBroker.SelectedValue);
            if (ddlBrokerContact.SelectedIndex != 0)
            {
                this.BrokerContactID = Convert.ToInt32(ddlBrokerContact.SelectedValue);
            
            }
            UpdateData();
            BindAdjustmentDetails();
            BindProgramPeriodDetails();
            BindBuBrokerDetails();
        }
        #endregion

        /// <summary>
        /// This Method is used to update the BUOffice,BrokerID and BrokerContactID Based on the selection in the 
        /// respective dropdown lists.
        /// This Update will be handled in the single Trasaction in order to maintain the consistency in cas eof failures.
        /// </summary>
        #region UpdateData Method
        public void UpdateData()
        {

            int intBrokerContactID = Convert.ToInt32(ddlBrokerContact.SelectedValue);
            int intUserID = CurrentAISUser.PersonID;

            objBuBrokerDC.Connection.Open();
            BuBrokertrans = objBuBrokerDC.Connection.BeginTransaction();
            objBuBrokerDC.Transaction = BuBrokertrans;

            try
            {
                bool result = new BuBrokerReviewBS().UpdateBuBrokerReviewData(objBuBrokerDC, this.PreAdjID, this.BrokerID, this.BuOfficeID, intBrokerContactID, intUserID);

                if (result)
                {
                    objBuBrokerDC.SubmitChanges();
                    if (BuBrokertrans != null)
                        BuBrokertrans.Commit();
                }
                else
                {
                    if (BuBrokertrans != null)
                        BuBrokertrans.Rollback();
                }
            }
            catch (Exception ee)
            {
                ShowError(ee.Message);
            }
            finally
            {
                if (objBuBrokerDC.Connection.State == ConnectionState.Open)
                    objBuBrokerDC.Connection.Close();
            }
            modalReview.Show();
        }
        #endregion


        /// <summary>
        /// Reviewpopup button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region btnReviewpopup Click Event
        protected void btnReviewpopup_Click(object sender, EventArgs e)
        {
            modalReview.Hide();
            string strPath = "/AdjCalc/InvoicingDashboard.aspx?AcctNo=" + AISMasterEntities.AccountNumber.ToString() + "&AdjNO=" + PreAdjID + "&AcctNm=" + AISMasterEntities.AccountName.Trim();
            Response.Redirect(strPath);
        }
        #endregion
        /// <summary>
        /// ReviewClose button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region btnReviewClose Click Event
        protected void btnReviewClose_Click(object sender, EventArgs e)
        {
            modalReview.Hide();
            string strSelectedValDate = "";
            fillSelectedValues(this.AccountID.ToString(), strSelectedValDate, this.PreAdjID.ToString());
        }
        #endregion
        /// <summary>
        /// Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region btnCancel Click Event
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string strSelectedValDate = "";
            fillSelectedValues(this.AccountID.ToString(), strSelectedValDate, this.PreAdjID.ToString());
        }
        #endregion
 }
#endregion

