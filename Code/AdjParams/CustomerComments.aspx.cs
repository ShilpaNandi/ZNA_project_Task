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
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.AIS.ExceptionHandling;


namespace AIS.WebSite.AdjParams
{
    public partial class CustomerComments : AISBasePage
    {
        int acccomentid;
        /// <summary>
        /// a property for CustomerComment Entity Class
        /// </summary>
        /// <param name=""></param>
        /// <returns>commentsBE</returns>
        CustomerCommentsBE commentsBE
        {
            get
            {
                //return ((Session["commentsBE"] == null) ?
                //    (new CustomerCommentsBE()) : (CustomerCommentsBE)Session["commentsBE"]);
                return ((RetrieveObjectFromSessionUsingWindowName("commentsBE") == null) ?
                   (new CustomerCommentsBE()) : (CustomerCommentsBE)RetrieveObjectFromSessionUsingWindowName("commentsBE"));
            }
            set
            {
                //Session["commentsBE"] = value;
                SaveObjectToSessionUsingWindowName("commentsBE", value);
            }
        }
        
        CustomerCommentsBS accountCommentService ;

        /// <summary>
        /// a property for CustomerComments Service Class
        /// </summary>
        /// <param name=""></param>
        /// <returns>accountCommentService</returns>
        private CustomerCommentsBS AccountCommentService
        {
            get
            {
                if (accountCommentService == null)
                {
                    accountCommentService = new CustomerCommentsBS();
                    //eRPformulaBS.AppTransactionWrapper = ERPTransactionWrapper;
                }
                return accountCommentService;
            }
            set
            {
                accountCommentService = value;
            }
        }
        
      
        /// <summary>
        /// PageLoad Event code 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.Page.Title = "Comments";
            if (AISMasterEntities.AccountNumber > 0)
            {
                 acccomentid = AISMasterEntities.AccountNumber;

                BindCommentListView();
                lblDetails.Visible = false;
                pnlDetails.Visible = false;
            }

            //Checks Exiting without Save
            ArrayList list = new ArrayList();
            list.Add(txtComment);
            list.Add(ddlCategory);
            list.Add(btnCancel);
            list.Add(btnSave);
            list.Add(btnNEW);
            list.Add(lnkClose);
            ProcessExitFlag(list);
        }
        /// <summary>
        /// Function for binding comments to the Comment Listview
        /// </summary>
        ///<param name=""></param>
        /// <returns></returns>
        private void BindCommentListView()
        {
            AccountCommentService = new CustomerCommentsBS();

            this.lstComments.DataSource = AccountCommentService.getRelatedComments(acccomentid);
            lstComments.DataBind();
        }
       
        /// <summary>
        /// Function for ItemCommand event of Comment Listview
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        protected void lstComments_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {

                int commentID = Convert.ToInt32(e.CommandArgument);
                //Session["commentid"] = commentID.ToString();
                SaveObjectToSessionUsingWindowName("commentid", commentID.ToString());
               // BindCommentDetail();
                commentsBE = (CustomerCommentsBE)AccountCommentService.Retrieve(commentID);
                string date = commentsBE.CommentDate.ToString();
                string catagory = commentsBE.CommentCategoryID.ToString();
                string comment = commentsBE.CommentText.ToString();
                string commentby = commentsBE.CommentBY.ToString();

                txtComment.Text = comment.ToString();
//                ddlCategory.SelectedIndex = -1;
//                ddlCategory.SelectedValue = commentsBE.CommentCategoryID.ToString();
                AddInActiveLookupData(ref ddlCategory, commentsBE.CommentCategoryID);
                
                lblDetails.Visible = true;
                lnkClose.Visible = true;
                pnlDetails.Visible = true;
                txtComment.ReadOnly = true;
                ddlCategory.Enabled = false;
                btnSave.Enabled = false;
                lstComments.Enabled = false;
                btnNEW.Enabled = false;
                
            }
        }
        
        /// <summary>
        /// Function for databound event of Comment Listview
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        protected void DataBoundList(Object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {

                HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trItemTemplate");
                tr.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                tr.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
                Label lblcoment = (Label)e.Item.FindControl("lblCommentText");
                if (lblcoment.Text.Length > 25)
                {
                    string strComent = lblcoment.Text.Substring(0, 25);
                    strComent = strComent + "...";
                    lblcoment.Text = strComent;
                
                }
                
            }
        }
        /// <summary>
        /// Function for SelectedIndexChanged event of Comment Listview
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        protected void lstComments_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
        {
            HtmlTableRow tr;
            //code for changing the previous selected row color to its original Color
            if (ViewState["SelectedIndex"] != null)
            {
                int index = Convert.ToInt32(ViewState["SelectedIndex"]);
                tr = (HtmlTableRow)lstComments.Items[index].FindControl("trItemTemplate");
                tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
            }
            tr = (HtmlTableRow)lstComments.Items[e.NewSelectedIndex].FindControl("trItemTemplate");
            LinkButton lnk = (LinkButton)lstComments.Items[e.NewSelectedIndex].FindControl("lnkDetail");
            ViewState["SelectedIndex"] = e.NewSelectedIndex;
            //code for changing the selected row style to highlighted
            tr.Attributes["class"] = "SelectedItemTemplate";
        }
       
        /// <summary>
        /// Function for adding comment to a particular account to CustomerComment Table
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            
            acccomentid = AISMasterEntities.AccountNumber;
            commentsBE = new CustomerCommentsBE();
            commentsBE.CustomerID = acccomentid;
            commentsBE.CommentCategoryID = Convert.ToInt32(ddlCategory.SelectedItem.Value);
            if (txtComment.Text != null)
            {
                commentsBE.CommentText = txtComment.Text.ToString();
            }
            else
            {
                commentsBE.CommentText = string.Empty;
            }
            commentsBE.CommentDate = System.DateTime.Now;
            commentsBE.CommentBY = CurrentAISUser.PersonID;
            bool Flag = AccountCommentService.Update(commentsBE);
            ShowConcurrentConflict(Flag, commentsBE.ErrorMessage);
            BindCommentListView();
            lnkClose.Visible = false;
            //Response.Redirect("CustomerComments.aspx");

        }
        /// <summary>
        /// Function for Click event of Cancel Button 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            lblDetails.Visible = true;
            lnkClose.Visible = true;
            pnlDetails.Visible = true;
            
            txtComment.Text = string.Empty;
            ddlCategory.SelectedIndex = -1;
            

        }
        /// <summary>
        /// Function for adding new comment
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        protected void btnNEW_Click(object sender, EventArgs e)
        {
            pnlComments.Enabled = true;
            lblDetails.Visible = true;
            lnkClose.Visible = true;
            pnlDetails.Visible = true;
            txtComment.ReadOnly = false;
            ddlCategory.Enabled = true;
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry )
            btnSave.Enabled = true;
            ddlCategory.SelectedIndex = -1;
            txtComment.Text = string.Empty;
           
            
        }
        /// <summary>
        /// Function for closing the Comment Detail panel
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        protected void lnkClose_Click(object sender, EventArgs e)
        {
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry )
            btnNEW.Enabled = true;
            lstComments.Enabled = true;
            lnkClose.Visible = false;
            pnlDetails.Visible = false;
            txtComment.Text = string.Empty;
            
        }
        

        }


    }




