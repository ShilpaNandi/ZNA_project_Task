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

//Importing different AIS framework namespaces for Surcharge Assesment Review screen
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.AIS.DAL.Logic;

namespace ZurichNA.AIS.WebSite.AdjCalc
{
    public partial class SurchargeReviewComments : System.Web.UI.Page
    {
        #region Property declarations
        /// <summary>
        /// Returns the Window Name
        /// </summary>
        public string WindowName
        {
            get
            {
                if (HttpContext.Current.Request["wID"] != null && HttpContext.Current.Request["wID"] != "")
                {
                    return HttpContext.Current.Request["wID"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Set the given value into given session
        /// </summary>
        /// <param name="SessionVariable"></param>
        /// <param name="EntityObject"></param>
        public void SaveObjectToSession(string SessionVariable, object EntityObject)
        {
            Session[SessionVariable] = EntityObject;
        }

        /// <summary>
        /// Set the given value into given session for a particular window
        /// </summary>
        /// <param name="SessionVariable"></param>
        /// <param name="EntityObject"></param>
        public void SaveObjectToSessionUsingWindowName(string SessionVariable, object EntityObject)
        {
            SaveObjectToSession(WindowName + SessionVariable, EntityObject);
        }

        /// <summary>
        /// Retrives the given Session for a particular window.
        /// </summary>
        /// <param name="SessionVariable"></param>
        /// <returns></returns>
        public object RetrieveObjectFromSession(string SessionVariable)
        {
            return Session[SessionVariable];
        }

        /// <summary>
        /// Retrives the given Session
        /// </summary>
        /// <param name="SessionVariable"></param>
        /// <returns></returns>
        public object RetrieveObjectFromSessionUsingWindowName(string SessionVariable)
        {
            return RetrieveObjectFromSession(WindowName + SessionVariable);
        }

        /// <summary>
        /// a property for SurchargeDetails Business Service Class
        /// </summary>
        /// <returns>SurchargeDetailsBS</returns>
        private AdjustmentReviewCommentsBS aAdjCmmnts;
        private AdjustmentReviewCommentsBS AdjRewCmmnts
        {
            get
            {
                if (aAdjCmmnts == null)
                {
                    aAdjCmmnts = new AdjustmentReviewCommentsBS();
                }
                return aAdjCmmnts;
            }
        }
        /// <summary>
        /// A Property for Holding AccountID in ViewState
        /// </summary>
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
                ViewState["SurCustID"] = value;
            }
        }
        /// <summary>
        /// A Property for Holding PremAdjPeriodID in ViewState
        /// </summary>
        protected int PremAdjPeriodID
        {
            get
            {
                //if (Session["PrgPeriodID"] != null)
                //{
                //    return int.Parse(Session["PrgPeriodID"].ToString());
                //}
                //else
                //{
                //    Session["PrgPeriodID"] = 0;
                //    return 0;
                //}
                if (RetrieveObjectFromSessionUsingWindowName("PrgPeriodID") != null)
                {
                    return int.Parse(RetrieveObjectFromSessionUsingWindowName("PrgPeriodID").ToString());
                }
                else
                {
                    SaveObjectToSessionUsingWindowName("PrgPeriodID", 0);
                    return 0;
                }
            }
            set
            {
                // Session["PrgPeriodID"] = value;
                SaveObjectToSessionUsingWindowName("PrgPeriodID", value);
            }
        }
        /// <summary>
        /// A Property for Holding PremAdjPeriodID in ViewState
        /// </summary>
        protected int PremAdjID
        {
            get
            {
                //if (Session["PreAdjID"] != null)
                //{
                //    return int.Parse(Session["PreAdjID"].ToString());
                //}
                //else
                //{
                //    Session["PreAdjID"] = 0;
                //    return 0;
                //}
                if (RetrieveObjectFromSessionUsingWindowName("PreAdjID") != null)
                {
                    return int.Parse(RetrieveObjectFromSessionUsingWindowName("PreAdjID").ToString());
                }
                else
                {
                    SaveObjectToSessionUsingWindowName("PreAdjID", 0);
                    return 0;
                }
            }
            set
            {
                //Session["PreAdjID"] = value;
                SaveObjectToSessionUsingWindowName("PreAdjID", value);
            }
        }
        #endregion

        #region page life cycle
        /// <summary>
        /// here we get the comments and show it on the screen,if there are any coments and also we update the 
        /// button name from save to update here.need to look if this is the correct place to do it in the page 
        /// life cycel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Session["CommentsShown"] = true.ToString();
                SaveObjectToSessionUsingWindowName("CommentsShown", true.ToString());
                //SurCustID = Convert.ToInt32(Session["SurCustID"].ToString());
                SurCustID = Convert.ToInt32(RetrieveObjectFromSessionUsingWindowName("SurCustID").ToString());
                txtCustmrID.Text = SurCustID.ToString();
                //PremAdjPeriodID = Convert.ToInt32(Session["PemAdjPeriodID"].ToString());
                PremAdjPeriodID = Convert.ToInt32(RetrieveObjectFromSessionUsingWindowName("PemAdjPeriodID").ToString());
                txtPrgPerdId.Text = PremAdjPeriodID.ToString();
                //txtUserId.Text = (Session["UserName"].ToString());
                txtUserId.Text = RetrieveObjectFromSessionUsingWindowName("UserName").ToString();
                //PremAdjID = Convert.ToInt32(Session["PreAdjID"].ToString());
                PremAdjID = Convert.ToInt32(RetrieveObjectFromSessionUsingWindowName("PreAdjID").ToString());
                txtPremAdjId.Text = PremAdjID.ToString();
                string aComments = AdjRewCmmnts.getAdjReviewCmmntALLData(608, PremAdjPeriodID, SurCustID).CMMNT_TXT;
                if (!string.IsNullOrEmpty(aComments))
                {
                    txtComments.Text = aComments.Trim();
                    hidCmmnts.Value = aComments.Trim();
                    btnSave.Text = "Update";
                }
            }
        }
        #endregion
       
        
        /// <summary>
        /// this wil either update or add the comments to database.here we are also closing the page once user
        /// has entered his comments.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {

            AdjustmentReviewCommentsBE AdjRevCmmntBE = new AdjustmentReviewCommentsBE();
            if (String.IsNullOrEmpty(txtPrgPerdId.Text))
                PremAdjPeriodID = 0;
            else
                PremAdjPeriodID=Convert.ToInt32(txtPrgPerdId.Text);
            if (String.IsNullOrEmpty(txtCustmrID.Text))
                SurCustID = 0;
            else
                SurCustID = Convert.ToInt32(txtCustmrID.Text);
            if (String.IsNullOrEmpty(txtPremAdjId.Text))
                PremAdjID = 0;
            else
                PremAdjID = Convert.ToInt32(txtPremAdjId.Text);
            AdjRevCmmntBE = new AdjustmentReviewCommentsBS().getAdjReviewCmmntALLData(608, PremAdjPeriodID, SurCustID);
            
                
                if (AdjRevCmmntBE.CMMNT_CATG_ID == null)
                {
                    AdjRevCmmntBE = new AdjustmentReviewCommentsBE();
                    AdjRevCmmntBE.CREATEUSER = Convert.ToInt32(txtUserId.Text);
                    AdjRevCmmntBE.CREATEDATE = DateTime.Now;

                    AdjRevCmmntBE.PREM_ADJ_ID = PremAdjID;
                    AdjRevCmmntBE.PREM_ADJ_PERD_ID = PremAdjPeriodID;
                    AdjRevCmmntBE.CUSTMR_ID = SurCustID;
                    AdjRevCmmntBE.CMMNT_CATG_ID = 608;
                    AdjRevCmmntBE.CMMNT_TXT = txtComments.Text;
                    new AdjustmentReviewCommentsBS().Update(AdjRevCmmntBE);
                   // AdjRevCmmntBE = AdjRevCmmntBS.getAdjReviewCmmntALLData(122, PremAdjPeriodID, intAccountID);
                }
               
           
            else
            {
                AdjustmentReviewCommentsBE AdjRevCmmntBEold = new AdjustmentReviewCommentsBS().getAdjustmentReviewCommentsRow(AdjRevCmmntBE.PREM_ADJ_CMMNT_ID);

                AdjRevCmmntBEold.UPDATEDUSER = Convert.ToInt32(txtUserId.Text);
                AdjRevCmmntBEold.UPDATEDDATE = DateTime.Now;
                
                AdjRevCmmntBEold.PREM_ADJ_ID = PremAdjID;
                AdjRevCmmntBEold.PREM_ADJ_PERD_ID = PremAdjPeriodID;
                AdjRevCmmntBEold.CUSTMR_ID = SurCustID;
                AdjRevCmmntBEold.CMMNT_CATG_ID = 608;
                AdjRevCmmntBEold.CMMNT_TXT = txtComments.Text;
                new AdjustmentReviewCommentsBS().Update(AdjRevCmmntBEold);
                

            }
           #region mycode
          /*   AdjustmentReviewCommentsBE myDtlBe = AdjRewCmmnts.getAdjReviewCmmntALLData(122, PremAdjPeriodID, SurCustID);
            if (btnSave.Text.ToUpper() == "SAVE")
            {
                
                myDtlBe.CUSTMR_ID = SurCustID;
                myDtlBe.PREM_ADJ_PERD_ID = PremAdjPeriodID;
               
                myDtlBe.CMMNT_TXT = txtComments.Text.Trim();
                myDtlBe.CMMNT_CATG_ID = 122;
                myDtlBe.CREATEUSER = Convert.ToInt32(txtUserId.Text);
                myDtlBe.CREATEDATE = DateTime.Now;
                AdjRewCmmnts.Update(myDtlBe);
            }
            else
            {
                AdjustmentReviewCommentsBE AdjRevCmmntBEold = AdjRewCmmnts.getAdjustmentReviewCommentsRow(myDtlBe.PREM_ADJ_CMMNT_ID);

                AdjRevCmmntBEold.UPDATEDUSER = Convert.ToInt32(Session["UserName"].ToString());
                AdjRevCmmntBEold.UPDATEDDATE = DateTime.Now;
                AdjRevCmmntBEold.PREM_ADJ_PERD_ID = PremAdjPeriodID;
                AdjRevCmmntBEold.CUSTMR_ID = SurCustID;
                AdjRevCmmntBEold.CMMNT_CATG_ID = 122;
                AdjRevCmmntBEold.CMMNT_TXT = txtComments.Text;
                AdjRewCmmnts.Update(AdjRevCmmntBEold);

            }*/
            #endregion
            //Session["PreviousPage"] = null;
            //Session["CommentsShown"] = "";
                SaveObjectToSessionUsingWindowName("PreviousPage", null);
                SaveObjectToSessionUsingWindowName("CommentsShown", "");
            Response.Write("<SCRIPT>window.close();</SCRIPT>");
        }
    }
}
