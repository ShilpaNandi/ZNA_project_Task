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
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.LSP.Framework;

public partial class AppMgmt_TransactionMaintenance : AISBasePage
{
    private BusinessUnitOfficeBS _BUBS;

    private BusinessUnitOfficeBS BUBS
    {
        get
        {
            if (_BUBS == null)
            {
                _BUBS = new BusinessUnitOfficeBS();
            }
            return _BUBS;
        }
    }

    private PostingTransactionTypeBS TransBS;

    private PostingTransactionTypeBS transBS
    {
        get
        {
            if (TransBS == null)
            {
                TransBS = new PostingTransactionTypeBS();
            }
            return TransBS;
        }
    }
    private LookupBS lookupService;
    private LookupTypeBS lookuptypeservice;
    /// <summary>
    /// a property for Lookup Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>LookupBS</returns>
    private LookupBS LookupService
    {
        get
        {
            if (lookupService == null)
            {
                lookupService = new LookupBS();
            }
            return lookupService;
        }
    }
    /// <summary>
    /// a property for Lookup Type Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>LookupTypeBS</returns>
    private LookupTypeBS LookupTypeService
    {
        get
        {
            if (lookuptypeservice == null)
            {
                lookuptypeservice = new LookupTypeBS();
            }
            return lookuptypeservice;
        }
    }
    /// <summary>
    /// a property for QCMasterIssueList Business Service Class
    /// </summary>
    /// <returns>QCMasterIssueListBS</returns>
    private QCMasterIssueListBS qcMasterIssueListBS;
    private QCMasterIssueListBS QCMasterIssueListBS
    {

        get 
        {
            if (qcMasterIssueListBS == null)
            {
                qcMasterIssueListBS = new QCMasterIssueListBS();
            }
            return qcMasterIssueListBS;
        }
    }
    /// <summary>
    /// a property for Lookup Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>LookupBE</returns>
    private LookupBE lookupBE
    {
        get { return (LookupBE)Session["LOOKUPBE"]; }
        set { Session["LOOKUPBE"] = value; }
    }
    /// <summary>
    /// a property for Lookup Type Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>PostalAddressBE</returns>
    private LookupTypeBE lookuptypeBE
    {
        get { return (LookupTypeBE)Session["LOOKUPTYPEBE"]; }
        set { Session["LOOKUPTYPEBE"] = value; }
    }
  
    private IList<LookupTypeBE> lookuptypeBEList
    {
        get
        {
            if (Session["lookuptypeBEList"] == null)
                Session["lookuptypeBEList"] = new List<LookupTypeBE>();
            return (IList<LookupTypeBE>)Session["lookuptypeBEList"];
        }
        set { Session["lookuptypeBEList"] = value; }
    }
    private IList<PostingTransactionTypeBE> PostingTransactionTypeBEList
    {
        get
        {
            if (Session["PostingTransactionTypeBEList"] == null)
                Session["PostingTransactionTypeBEList"] = new List<PostingTransactionTypeBE>();
            return (IList<PostingTransactionTypeBE>)Session["PostingTransactionTypeBEList"];
        }
        set { Session["PostingTransactionTypeBEList"] = value; }
    }
    private IList<QCMasterIssueListBE> QCMasterIssueListBEList
    {
        get
        {
            if (Session["QCMasterIssueListBEList"] == null)
                Session["QCMasterIssueListBEList"] = new List<QCMasterIssueListBE>();
            return (IList<QCMasterIssueListBE>)Session["QCMasterIssueListBEList"];
        }
        set { Session["QCMasterIssueListBEList"] = value; }
    }
    private IList<LookupBE> LookupBEList
    {
        get
        {
            if (Session["LookupBEList"] == null)
                Session["LookupBEList"] = new List<LookupBE>();
            return (IList<LookupBE>)Session["LookupBEList"];
        }
        set { Session["LookupBEList"] = value; }
    }

    private IList<BusinessUnitOfficeBE> BusinessUnitOfficeBEList
    {
        get
        {
            if (Session["BusinessUnitOfficeBEList"] == null)
                Session["BusinessUnitOfficeBEList"] = new List<BusinessUnitOfficeBE>();
            return (IList<BusinessUnitOfficeBE>)Session["BusinessUnitOfficeBEList"];
        }
        set { Session["BusinessUnitOfficeBEList"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.Page.Title = "Transaction Maintenance";
        if (!IsPostBack)
        {
            lookuptypeBE = new LookupTypeBE();
            lookupBE = new LookupBE();
            //Function to bind the LookupType Data
            BindListView();
            SelectData();

            //Function to Bind the lstIssues listview in Issue CheckList Maintenance Tab
            BindIssueList();

            //Function to bind the BUOffice listview in BU/Office Maintenance tab
            BindBUOfficeList();
            
        }
    }

    private void BindBUOfficeList()
    {
        BusinessUnitOfficeBEList = BUBS.GetBusinessUnitList();
        lstBUOffice.DataSource = BusinessUnitOfficeBEList;
        lstBUOffice.DataBind();
    }
   
    /// <summary>
    /// Function to bind the LookupType details in listview
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public void BindListView()
    {
        lookuptypeBEList = LookupTypeService.getLookupTypeData();
        lstLookupType.DataSource = lookuptypeBEList;
        lstLookupType.DataBind();
       
    }
    protected void CommandList(Object sender, ListViewCommandEventArgs e)
    {
        ListView lv = (ListView)sender;
        if (e.CommandName.ToUpper() == "SAVE")
        {
            if (lv.ID.ToString() == "lstLookupType")
            {
                //Function to save the LookupType
                LookupTypeSaveList(e.Item);
            }
            else
            {
                //Function to save the Lookup
                LookupSaveList(e.Item);
            }
        }
        else if (e.CommandName.ToUpper() == "DISABLE" || e.CommandName.ToUpper() == "ENABLE")
        {
            //Function call to Enable or Disable the Record
            DisableRow(Convert.ToInt32(e.CommandArgument), e.CommandName == "DISABLE" ? false : true, lv.ID.ToString() == "lstLookupType" ? "LOOKUPTYPE" : "LOOKUP");
        }

    }
    /// <summary>
    /// Function to bind the Lookup Details based on LookupType
    /// </summary>
    /// <param name="LookupTypeID">Select LookupTypeID from ListView</param>
    /// <returns></returns>
    public void DisplayLookupTypeDetails(int LookupTypeID)
    {
        BLAccess bl = new BLAccess();
        //IList<LookupBE> 
        LookupBEList = bl.GetLookUpData(LookupTypeID);
        lstLookupDetails.DataSource = LookupBEList.OrderBy(lk => lk.LookUpName.Trim());
        lstLookupDetails.DataBind();
    }
    protected void InsertList(Object sender, ListViewInsertEventArgs e)
    {
        lstLookupType.InsertItemPosition = InsertItemPosition.None;
    }
    protected void DataBoundList(Object sender, ListViewItemEventArgs e)
    {
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
                imgDelete.Attributes.Add("onclick", hid.Value == "True" ? "return confirm('Are you sure you want to disable?');" : "return confirm('Are you sure you want to enable?');");
            }
        }
    }
    // Invoked when the Edit Link is clicked
    // Set the Listview to Editmode
    protected void EditList(Object sender, ListViewEditEventArgs e)
    {
        ListView lstView = (ListView)sender;
        if (lstView.ID.ToString() == "lstLookupType")
        {
            lstLookupType.EditIndex = e.NewEditIndex;
            //Function to bind the LookupType Data
            BindListView();
            ((LinkButton)lstView.InsertItem.FindControl("lnkSave")).Enabled = false;
            ((TextBox)lstView.InsertItem.FindControl("txtlkupTypeName")).Enabled = false;
        }
        else
        {
            lstLookupDetails.EditIndex = e.NewEditIndex;
            //Function to bind the LookupTypeDetails Data
            DisplayLookupTypeDetails(Convert.ToInt32(ViewState["LOOKUPTYPEID"]));
            ((LinkButton)lstLookupDetails.InsertItem.FindControl("lnkSave")).Enabled = false;
            ((TextBox)lstLookupDetails.InsertItem.FindControl("txtlkupTypeName")).Enabled = false;
            ((TextBox)lstLookupDetails.InsertItem.FindControl("txtAttribute1")).Enabled = false;

        }
    }
    // Invoked when the Cancel Link is clicked
    protected void CancelList(Object sender, ListViewCancelEventArgs e)
    {

        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            ListView lstView = (ListView)sender;
            if (lstView.ID.ToString() == "lstLookupType")
            {
                lstLookupType.EditIndex = -1;
                //Function to bind the LookupType Data
                BindListView();
            }
            else
            {
                lstLookupDetails.EditIndex = -1;
                //Function to bind the LookupTypeDetails Data
                DisplayLookupTypeDetails(Convert.ToInt32(ViewState["LOOKUPTYPEID"]));
            }

        }

    }
    // Invoked when the Update Link is clicked
    protected void UpdateList(Object sender, ListViewUpdateEventArgs e)
    {
        ListView lstView = (ListView)sender;
        if (lstView.ID.ToString() == "lstLookupType")
        {
            ListViewItem myItem = lstLookupType.Items[e.ItemIndex];
            int intLKUPTYID = int.Parse(((HiddenField)myItem.FindControl("hidEditLkupTypeID")).Value.ToString());
            lookuptypeBE = LookupTypeService.getLkupTypeRow(intLKUPTYID);
            //Concurrency Issue
            LookupTypeBE lookuptypeBEold = (lookuptypeBEList.Where(o => o.LOOKUPTYPE_ID.Equals(intLKUPTYID))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(lookuptypeBEold.UPDATED_DATE), Convert.ToDateTime(lookuptypeBE.UPDATED_DATE));
            if (!con)
                return;
            //End
            lookuptypeBE.LOOKUPTYPE_NAME = ((TextBox)myItem.FindControl("txtEditlkupTypeName")).Text;
            lookuptypeBE.UPDATED_USER_ID = CurrentAISUser.PersonID;
            lookuptypeBE.UPDATED_DATE = DateTime.Now;
            bool Flag = LookupTypeService.IsExistsLookupTypeName(lookuptypeBE.LOOKUPTYPE_NAME, lookuptypeBE.LOOKUPTYPE_ID);
            if (!Flag)
            {
                Flag = LookupTypeService.Update(lookuptypeBE);
                ShowConcurrentConflict(Flag, lookuptypeBE.ErrorMessage);
                lstLookupType.EditIndex = -1;
                //Function to bind the LookupType Data
                BindListView();
                //Logging
               (new Common(this.GetType())).Logger.Info("User -  " + CurrentAISUser.FullName + "[AZCORP:"
                + CurrentAISUser.UserID + ", Role: " + CurrentAISUser.Role + "] tried to update LookUpType Info the AIS application");

            }
            else
            {
                ShowMessage("The record cannot be saved. An identical record already exists.");
            }

        }
        else
        {
            ListViewItem myItem = lstLookupDetails.Items[e.ItemIndex];
            int intLKUPID = int.Parse(((HiddenField)myItem.FindControl("hidEditLkupID")).Value.ToString());
            lookupBE = LookupService.getLkupRow(intLKUPID);
            //Concurrency Issue
            LookupBE lookupBEold = (LookupBEList.Where(o => o.LookUpID.Equals(intLKUPID))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(lookupBEold.Updated_Date), Convert.ToDateTime(lookupBE.Updated_Date));
            if (!con)
                return;
            //End
            lookupBE.LookUpName = ((TextBox)myItem.FindControl("txtEditlkupName")).Text;
            lookupBE.LookUpTypeID = int.Parse(ViewState["LOOKUPTYPEID"].ToString());
            lookupBE.Attribute1 = ((TextBox)myItem.FindControl("txtAttribute")).Text;
            lookupBE.Updated_Date = System.DateTime.Now;
            bool Flag = lookupService.IsExistsLookupName(lookupBE.LookUpID, lookupBE.LookUpTypeID, lookupBE.LookUpName);
            if (!Flag)
            {
                Flag = LookupService.Update(lookupBE);
                ShowConcurrentConflict(Flag, lookupBE.ErrorMessage);
                if (Flag)
                {
                    IList<LookupBE> lookupBEs = new List<LookupBE>();
                    lookupBEs = LookupService.getLookupData();
                    Application["LookUpData"] = lookupBEs;
                    lstLookupDetails.EditIndex = -1;
                    //Function to bind the LookupTypeDetails Data
                    DisplayLookupTypeDetails(Convert.ToInt32(ViewState["LOOKUPTYPEID"]));
                   //Logging
                    (new Common(this.GetType())).Logger.Info("User -  " + CurrentAISUser.FullName + "[AZCORP:"
                         + CurrentAISUser.UserID + ", Role: " + CurrentAISUser.Role + "] tried to update LookUpTypeDetail  Info the AIS application");
                
                }
            }
            else
            {
                ShowMessage("The record cannot be saved. An identical record already exists.");
            }
        }
    }
    /// <summary>
    /// Function to save the LookupType
    /// </summary>
    /// <param name="ListViewItem"></param>
    /// <returns></returns>
    // Invoked when the Save Link is clicked
    protected void LookupTypeSaveList(ListViewItem e)
    {
        string strLookupTypeName = ((TextBox)e.FindControl("txtlkupTypeName")).Text;
        bool Flag = LookupTypeService.IsExistsLookupTypeName(strLookupTypeName, 0);
        if (!Flag)
        {
            lookuptypeBE = new LookupTypeBE();
            lookuptypeBE.LOOKUPTYPE_NAME = strLookupTypeName;
            lookuptypeBE.ACTIVE = true;
            lookuptypeBE.CREATED_USER_ID = CurrentAISUser.PersonID;
            lookuptypeBE.CREATED_DATE = DateTime.Now;
            Flag = LookupTypeService.Update(lookuptypeBE);
            if (Flag)
            {
                //Function to bind the LookupType Data
                BindListView();
            }
        }
        else
        {
            ShowMessage("The record cannot be saved. An identical record already exists.");
        }

    }
    /// <summary>
    /// Function to save the Lookup
    /// </summary>
    /// <param name="ListViewItem"></param>
    /// <returns></returns>
    /// Invoked when the Save Link is clicked
    protected void LookupSaveList(ListViewItem e)
    {
        string strLookupName = ((TextBox)e.FindControl("txtlkupTypeName")).Text;
        int lkupTypeID = int.Parse(ViewState["LOOKUPTYPEID"].ToString());
        bool Flag = LookupService.IsExistsLookupName(0, lkupTypeID, strLookupName);
        if (!Flag)
        {
            lookupBE = new LookupBE();
            lookupBE.LookUpName = ((TextBox)e.FindControl("txtlkupTypeName")).Text;
            lookupBE.LookUpTypeID = lkupTypeID;
            lookupBE.Attribute1 = ((TextBox)e.FindControl("txtAttribute1")).Text;
            lookupBE.Created_UserID = CurrentAISUser.PersonID;
            lookupBE.Created_Date = DateTime.Now;
            lookupBE.ACTIVE = true;
            Flag = LookupService.Update(lookupBE);
            if (Flag)
            {
                //code to update the lookupBEs data into application varialble
                IList<LookupBE> lookupBEs = new List<LookupBE>();
                lookupBEs = lookupService.getLookupData();
                Application["LookUpData"] = lookupBEs;
                //Function to bind the LookupTypeDetails Data
                DisplayLookupTypeDetails(int.Parse(ViewState["LOOKUPTYPEID"].ToString()));
            }
        }
        else
        {
            ShowMessage("The record cannot be saved. An identical record already exists.");
        }

    }
    /// <summary>
    /// Function to Disable the Record
    /// </summary>
    /// <param name="lkPKID"></param>
    /// <param name="Flag"></param>
    /// <returns></returns>
    /// Invoked when the Disable Link is clicked
    protected void DisableRow(int lkPKID, bool Flag, string strListView)
    {
        if (strListView == "LOOKUPTYPE")
        {
            lookuptypeBE = LookupTypeService.getLkupTypeRow(lkPKID);
            //Concurrency Issue
            LookupTypeBE lookuptypeBEold = (lookuptypeBEList.Where(o => o.LOOKUPTYPE_ID.Equals(lkPKID))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(lookuptypeBEold.UPDATED_DATE), Convert.ToDateTime(lookuptypeBE.UPDATED_DATE));
            if (!con)
                return;
            //End
            lookuptypeBE.ACTIVE = Flag;
            lookuptypeBE.UPDATED_USER_ID = CurrentAISUser.PersonID;
            lookuptypeBE.UPDATED_DATE = DateTime.Now;
            Flag = LookupTypeService.Update(lookuptypeBE);
            ShowConcurrentConflict(Flag, lookuptypeBE.ErrorMessage);
            if (Flag)
            {
                //Function to bind the LookupType Data
                BindListView();
            }
        }
        else
        {
            lookupBE = LookupService.getLkupRow(lkPKID);
            //Concurrency Issue
            LookupBE lookupBEold = (LookupBEList.Where(o => o.LookUpID.Equals(lkPKID))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(lookupBEold.Updated_Date), Convert.ToDateTime(lookupBE.Updated_Date));
            if (!con)
                return;
            //End
            lookupBE.ACTIVE = Flag;
            lookupBE.Updated_Date = System.DateTime.Now;
            Flag = LookupService.Update(lookupBE);
            ShowConcurrentConflict(Flag, lookupBE.ErrorMessage);
            if (Flag)
            {
                IList<LookupBE> lookupBEs = new List<LookupBE>();
                lookupBEs = LookupService.getLookupData();
                Application["LookUpData"] = lookupBEs;
                //Function to bind the LookupTypeDetails Data
                DisplayLookupTypeDetails(Convert.ToInt32(ViewState["LOOKUPTYPEID"]));
            }
        }
    }
    /// <summary>
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    /// Invoked when the Close Link is clicked
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        pnlLookupDetails.Visible = false;
        divCancel.Visible = false;
        lstLookupType.Enabled = true;
        HtmlTableRow tr;
        if (ViewState["LKSelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["LKSelectedIndex"]);
            tr = (HtmlTableRow)lstLookupType.Items[index].FindControl("trItemTemplate");
            tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
        }
        ViewState["LKSelectedIndex"] = null;

    }
    protected void lstLookupType_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
    {
        HtmlTableRow tr;
        if (ViewState["LKSelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["LKSelectedIndex"]);
            tr = (HtmlTableRow)lstLookupType.Items[index].FindControl("trItemTemplate");
            tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
        }
        tr = (HtmlTableRow)lstLookupType.Items[e.NewSelectedIndex].FindControl("trItemTemplate");
        LinkButton lnk = (LinkButton)lstLookupType.Items[e.NewSelectedIndex].FindControl("lnkSelect");
        ViewState["LKSelectedIndex"] = e.NewSelectedIndex;
        tr.Attributes["class"] = "SelectedItemTemplate";
        ViewState["ERPFORMULAID"] = Convert.ToInt32(lnk.CommandArgument);
        lblLookupTypeName.Text = "Lookup details for Lookup Type: " + ((Label)lstLookupType.Items[e.NewSelectedIndex].FindControl("lkupTypeName")).Text;
        ViewState["LOOKUPTYPEID"] = lnk.CommandArgument;
        //Function to bind the LookupTypeDetails Data
        DisplayLookupTypeDetails(Convert.ToInt32(lnk.CommandArgument));
        pnlLookupDetails.Visible = true;
        divCancel.Visible = true;
        lstLookupType.Enabled = false;

    }
    #region Posting Transaction Type TAB
    private string SortBy
    {
        get { return (string)Session["SORTBY"]; }
        set { Session["SORTBY"] = value; }
    }
    private string SortDir
    {
        get { return (string)Session["SORTDIR"]; }
        set { Session["SORTDIR"] = value; }
    }

    protected void lstBUOffice_DataBoundList(object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trItemTemplate");
            if (tr != null)
            {
                tr.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                tr.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            }

            ImageButton ibtnEnableDisable = (ImageButton)e.Item.FindControl("ibtnEnableDisable");
            if (ibtnEnableDisable != null)
            {
                Label lblActive = (Label)e.Item.FindControl("lblActive");
                if (lblActive.Text == "True")
                {
                    ibtnEnableDisable.ImageUrl = "~/images/disabled.GIF";
                    ibtnEnableDisable.CommandName = "DISABLE";
                    ibtnEnableDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to disable this Business Unit? \\n Press OK to disable, or Cancel to stay on current page');");
                    ibtnEnableDisable.ToolTip = "Click here to Disable this Business Unit";
                }
                else
                {
                    ibtnEnableDisable.ImageUrl = "~/images/enabled.GIF";
                    ibtnEnableDisable.CommandName = "ENABLE";
                    ibtnEnableDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to enable this Business Unit? \\n Press OK to enable, or Cancel to stay on current page');");
                    ibtnEnableDisable.ToolTip = "Click here to Enable this Business Unit";
                }
            }
        }
    }

    protected void lstPostTrans_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trItemTemplate");
            if (tr != null)
            {
                tr.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                tr.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            }

            DropDownList ddlTransTypelist = (DropDownList)e.Item.FindControl("ddlTransTypelist");
            Label hdTransTypId = (Label)e.Item.FindControl("hdTransTypId");


            if ((ddlTransTypelist != null) & (hdTransTypId != null))
            {
//                ddlTransTypelist.SelectedIndex = ddlTransTypelist.Items.IndexOf(ddlTransTypelist.Items.FindByValue(hdTransTypId.Text.ToString()));
                AddInActiveLookupData(ref ddlTransTypelist, hdTransTypId.Text);
            }
            ImageButton ibtnEnableDisable = (ImageButton)e.Item.FindControl("ibtnEnableDisable");
            if (ibtnEnableDisable != null)
            {
                Label lblActive = (Label)e.Item.FindControl("lblActive");
                if (lblActive.Text == "True")
                {
                    ibtnEnableDisable.ImageUrl = "~/images/disabled.GIF";
                    ibtnEnableDisable.CommandName = "DISABLE";
                    ibtnEnableDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to disable this Transaction? \\n Press OK to disable, or Cancel to stay on current page');");
                    ibtnEnableDisable.ToolTip = "Click here to Disable this Transaction";
                }
                else
                {
                    ibtnEnableDisable.ImageUrl = "~/images/enabled.GIF";
                    ibtnEnableDisable.CommandName = "ENABLE";
                    ibtnEnableDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to enable this Transaction? \\n Press OK to enable, or Cancel to stay on current page');");
                    ibtnEnableDisable.ToolTip = "Click here to Enable this Transaction";
                }
            }


        }
    }

    protected void lstBUOffice_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        lstBUOffice.EditIndex = e.NewEditIndex;
        BindBUOfficeList();
    }
    protected void lstPostTrans_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        lstPostTrans.EditIndex = e.NewEditIndex;
        SelectData();
    }

    protected void lstBUOffice_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if ((e.CommandName.ToUpper() == "SAVE"))
        {
            TextBox txtBUOfficeName = (TextBox)e.Item.FindControl("txtBUOfficeName");
            TextBox txtBUUntCD = (TextBox)e.Item.FindControl("txtBUUntCD");
            TextBox txtCityName = (TextBox)e.Item.FindControl("txtCityName");
            TextBox txtOfcCd = (TextBox)e.Item.FindControl("txtOfcCd");

            BusinessUnitOfficeBE BuBE = new BusinessUnitOfficeBE();

            BuBE.FULL_NAME = txtBUOfficeName.Text;
            if (txtBUUntCD.Text != "")
            {
                BuBE.BSN_UNT_CD = txtBUUntCD.Text;
            }
            if (txtCityName.Text != "")
            {
                BuBE.CITY_NM = txtCityName.Text;
            }
            if (txtOfcCd.Text != "")
            {
                BuBE.OFC_CD = txtOfcCd.Text;
            }

            BuBE.ACTV_IND = true;
            BuBE.CREATED_DATE = System.DateTime.Now;
            BuBE.CREATED_USERID = CurrentAISUser.PersonID;
            BUBS.Save(BuBE);
            lstBUOffice.EditIndex = -1;
        }
        if ((e.CommandName.ToUpper() == "UPDATE"))
        {
            Label lblBUOfficeId = (Label)e.Item.FindControl("hdBusinessUnitOfficeId");

            TextBox txtBUOfficeName = (TextBox)e.Item.FindControl("txtBUOfficeName");
            TextBox txtBUUntCD = (TextBox)e.Item.FindControl("txtBUUntCD");
            TextBox txtCityName = (TextBox)e.Item.FindControl("txtCityName");
            TextBox txtOfcCd = (TextBox)e.Item.FindControl("txtOfcCd");

            BusinessUnitOfficeBE BuBE = new BusinessUnitOfficeBE();
            BuBE = BUBS.Retrieve(Convert.ToInt32(lblBUOfficeId.Text));
            //Concurrency Issue
            BusinessUnitOfficeBE setupBEold = 
                (BusinessUnitOfficeBEList.Where(b => b.INTRNL_ORG_ID.Equals(Convert.ToInt32(lblBUOfficeId.Text)))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(setupBEold.UPDATED_DATE), Convert.ToDateTime(BuBE.UPDATED_DATE));
            if (!con)
                return;
            //End
            BuBE.FULL_NAME = txtBUOfficeName.Text;
            BuBE.BSN_UNT_CD = txtBUUntCD.Text;
            BuBE.CITY_NM = txtCityName.Text;
            BuBE.OFC_CD = txtOfcCd.Text;
            BuBE.ACTV_IND = true;
            BuBE.UPDATED_DATE = System.DateTime.Now;
            BuBE.UPDATED_USERID = CurrentAISUser.PersonID;

            bool Flag = BUBS.Save(BuBE); 
            ShowConcurrentConflict(Flag, BuBE.ErrorMessage);
            lstBUOffice.EditIndex = -1;

        }
        if ((e.CommandName.ToUpper() == "DISABLE") || (e.CommandName.ToUpper() == "ENABLE"))
        {
            try
            {
                bool succeed = false;
                int INTL_ORGN_ID = Convert.ToInt32(e.CommandArgument);
                BusinessUnitOfficeBE BuBE = new BusinessUnitOfficeBE();
                BuBE = BUBS.Retrieve(INTL_ORGN_ID);
                //Concurrency Issue
                BusinessUnitOfficeBE setupBEold = 
                    (BusinessUnitOfficeBEList.Where(b => b.INTRNL_ORG_ID.Equals(INTL_ORGN_ID))).First();
                bool con = ShowConcurrentConflict(Convert.ToDateTime(setupBEold.UPDATED_DATE), Convert.ToDateTime(BuBE.UPDATED_DATE));
                if (!con)
                    return;
                //End
                if (e.CommandName.ToUpper() == "DISABLE")
                    BuBE.ACTV_IND = false;
                else
                    BuBE.ACTV_IND = true;
                BuBE.UPDATED_USERID = CurrentAISUser.PersonID;
                BuBE.UPDATED_DATE = DateTime.Now;
                succeed = BUBS.Save(BuBE);
                ShowConcurrentConflict(succeed, BuBE.ErrorMessage);
            }
            catch (System.Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                ShowError(ex.Message, ex);
            }

        }
        BindBUOfficeList();
    }

    protected void lstPostTrans_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        if ((e.CommandName.ToUpper() == "SAVE"))
        {
            DropDownList ddlTransTypelist = (DropDownList)e.Item.FindControl("ddlTransTypelist");
            TextBox txtTransName = (TextBox)e.Item.FindControl("txtTransName");
            TextBox txtMain = (TextBox)e.Item.FindControl("txtMain");
            TextBox txtSub = (TextBox)e.Item.FindControl("txtSub");
            TextBox txtCompany = (TextBox)e.Item.FindControl("txtCompany");
            CheckBox chkInv = (CheckBox)e.Item.FindControl("chkInv");
            CheckBox chkMisc = (CheckBox)e.Item.FindControl("chkMisc");
            CheckBox chkTpa = (CheckBox)e.Item.FindControl("chkTpa");
            CheckBox chkAdjSummary = (CheckBox)e.Item.FindControl("chkAdjSummary");
            CheckBox ChkPolReq = (CheckBox)e.Item.FindControl("ChkPolReq");

            PostingTransactionTypeBE transBE = new PostingTransactionTypeBE();

            transBE.TRNS_TYP_ID = Convert.ToInt32(ddlTransTypelist.SelectedValue);
            transBE.TRANS_TXT = txtTransName.Text;
            if (txtMain.Text != "")
            {
                transBE.MAIN_NBR = txtMain.Text;
            }
            if (txtSub.Text != "")
            {
                transBE.SUB_NBR = txtSub.Text;
            }
            if (txtCompany.Text != "")
            {
                transBE.COMP_TXT = txtCompany.Text;
            }
            transBE.INVOICBL_IND = chkInv.Checked;
            transBE.MISC_POSTS_IND = chkMisc.Checked;
            transBE.THRD_PTY_ADMIN_MNL_IND = chkTpa.Checked;
            transBE.ADJ_SUMRY_NOT_POST_IND = chkAdjSummary.Checked;
            transBE.POL_REQR_IND = ChkPolReq.Checked;
            transBE.ACTV_IND = true;
            transBE.Created_Date = System.DateTime.Now;
            transBE.Created_UserID = CurrentAISUser.PersonID;
            transBS.SaveSetupData(transBE);
            lstPostTrans.EditIndex = -1;


        }
        if ((e.CommandName.ToUpper() == "UPDATE"))
        {
            Label lblPostTransTypeId = (Label)e.Item.FindControl("hdPostTransTypeId");
            DropDownList ddlTransTypelist = (DropDownList)e.Item.FindControl("ddlTransTypelist");
            TextBox txtTransName = (TextBox)e.Item.FindControl("txtTransName");
            TextBox txtMain = (TextBox)e.Item.FindControl("txtMain");
            TextBox txtSub = (TextBox)e.Item.FindControl("txtSub");
            TextBox txtCompany = (TextBox)e.Item.FindControl("txtCompany");
            CheckBox chkInv = (CheckBox)e.Item.FindControl("chkInv");
            CheckBox chkMisc = (CheckBox)e.Item.FindControl("chkMisc");
            CheckBox chkTpa = (CheckBox)e.Item.FindControl("chkTpa");
            CheckBox chkAdjSummary = (CheckBox)e.Item.FindControl("chkAdjSummary");
            CheckBox ChkPolReq = (CheckBox)e.Item.FindControl("ChkPolReq");

            PostingTransactionTypeBE transBE = new PostingTransactionTypeBE();
            transBE = transBS.LoadData(Convert.ToInt32(lblPostTransTypeId.Text));
            //Concurrency Issue
            PostingTransactionTypeBE setupBEold = (PostingTransactionTypeBEList.Where(o => o.POST_TRANS_TYP_ID.Equals(Convert.ToInt32(lblPostTransTypeId.Text)))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(setupBEold.UPDATE_DATE), Convert.ToDateTime(transBE.UPDATE_DATE));
            if (!con)
                return;
            //End
            transBE.POST_TRANS_TYP_ID = Convert.ToInt32(lblPostTransTypeId.Text);
            transBE.TRNS_TYP_ID = Convert.ToInt32(ddlTransTypelist.SelectedValue);
            transBE.TRANS_TXT = txtTransName.Text;
            if (txtMain.Text != "")
            {
                transBE.MAIN_NBR = txtMain.Text;
            }
            if (txtSub.Text != "")
            {
                transBE.SUB_NBR = txtSub.Text;
            }
            if (txtCompany.Text != "")
            {
                transBE.COMP_TXT = txtCompany.Text;
            }
            transBE.INVOICBL_IND = chkInv.Checked;
            transBE.MISC_POSTS_IND = chkMisc.Checked;
            transBE.THRD_PTY_ADMIN_MNL_IND = chkTpa.Checked;
            transBE.ADJ_SUMRY_NOT_POST_IND = chkAdjSummary.Checked;
            transBE.POL_REQR_IND = ChkPolReq.Checked;
            transBE.ACTV_IND = true;
            transBE.UPDATE_DATE = System.DateTime.Now;
            transBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
            bool Flag=transBS.SaveSetupData(transBE);
            ShowConcurrentConflict(Flag, transBE.ErrorMessage);
            lstPostTrans.EditIndex = -1;

        }
        if ((e.CommandName.ToUpper() == "DISABLE") || (e.CommandName.ToUpper() == "ENABLE"))
        {
            try
            {
                bool succeed = false;
                int POST_TRANS_TYP_ID = Convert.ToInt32(e.CommandArgument);
                PostingTransactionTypeBE transBE = new PostingTransactionTypeBE();
                transBE = transBS.LoadData(POST_TRANS_TYP_ID);
                //Concurrency Issue
                PostingTransactionTypeBE setupBEold = (PostingTransactionTypeBEList.Where(o => o.POST_TRANS_TYP_ID.Equals(POST_TRANS_TYP_ID))).First();
                bool con = ShowConcurrentConflict(Convert.ToDateTime(setupBEold.UPDATE_DATE), Convert.ToDateTime(transBE.UPDATE_DATE));
                if (!con)
                    return;
                //End
                if (e.CommandName.ToUpper() == "DISABLE")
                    transBE.ACTV_IND = false;
                else
                    transBE.ACTV_IND = true;
                transBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
                transBE.UPDATE_DATE = DateTime.Now;
                succeed = transBS.SaveSetupData(transBE);
                ShowConcurrentConflict(succeed, transBE.ErrorMessage);
            }
            catch (System.Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                ShowError(ex.Message,ex);
            }

        }
        SelectData();
    }

    protected void lstBUOffice_ItemCancel(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstBUOffice.EditIndex = -1;
            BindBUOfficeList();
        }
        else if (e.CancelMode == ListViewCancelMode.CancelingInsert)
        {
            lstBUOffice.InsertItemPosition = InsertItemPosition.None;
        }
 
    }

    protected void lstPostTrans_ItemCancel(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstPostTrans.EditIndex = -1;
            SelectData();
        }
        else if (e.CancelMode == ListViewCancelMode.CancelingInsert)
        {
            CancelUpdateMode(); //Back to normal mode.
        }
    }

    protected void lstBUOffice_ItemUpdate(Object sender, ListViewUpdateEventArgs e)
    {

    }

    protected void lstPostTrans_ItemUpdate(Object sender, ListViewUpdateEventArgs e)
    {

    }

    protected void lstBUOffice_ItemInserting(Object sender, ListViewInsertEventArgs e)
    {
        lstBUOffice.InsertItemPosition = InsertItemPosition.None;
    }

    protected void lstPostTrans_ItemInserting(Object sender, ListViewInsertEventArgs e)
    {
        lstPostTrans.InsertItemPosition = InsertItemPosition.None;
    }

    protected void lstPostTrans_Sorting(object sender, ListViewSortEventArgs e)
        {
            Image imgTransType = (Image)lstPostTrans.FindControl("imgTransType");
            Image imgMain = (Image)lstPostTrans.FindControl("imgMain");
            Image imgSub = (Image)lstPostTrans.FindControl("imgSub");
            Image imgCompany = (Image)lstPostTrans.FindControl("imgCompany");
            Image imgInvoice = (Image)lstPostTrans.FindControl("imgInvoice");
            Image imgPosting = (Image)lstPostTrans.FindControl("imgPosting");
            Image imgTpaManual = (Image)lstPostTrans.FindControl("imgTpaManual");
            

            Image img = new Image();
            switch (e.SortExpression)
            {
                case "TRANSACTIONTYPE":
                    SortBy = "TRANSACTIONTYPE";
                    imgTransType.Visible = true;
                    img = imgTransType;
                    break;
                case "MAIN":
                    SortBy = "MAIN";
                    imgMain.Visible = true;
                    img = imgMain;
                    break;
                case "SUB":
                    SortBy = "SUB";
                    imgSub.Visible = true;
                    img = imgSub;
                    break;
                case "COMPANY":
                    SortBy = "COMPANY";
                    imgCompany.Visible = true;
                    img = imgCompany;
                    break;
                case "INVOICE":
                    SortBy = "INVOICE";
                    imgInvoice.Visible = true;
                    img = imgInvoice;
                    break;
                case "POSTING":
                    SortBy = "POSTING";
                    imgPosting.Visible = true;
                    img = imgPosting;
                    break;
                case "TPAMANUAL":
                    SortBy = "TPAMANUAL";
                    imgTpaManual.Visible = true;
                    img = imgTpaManual;
                    break;
            }
            if (img.ToolTip == "Ascending")
            {
                img.ToolTip = "Descending";
                img.ImageUrl = "~/images/descending.gif";
                SortDir = "DESC";
            }
            else
            {
                img.ToolTip = "Ascending";
                img.ImageUrl = "~/images/Ascending.gif";
                SortDir = "ASC";
            }
            SelectSortData();
        }
    

    protected void CancelUpdateMode()
    {
        lstPostTrans.InsertItemPosition = InsertItemPosition.None;
        SelectData();
    }
    public void SelectData()
    {
        PostingTransactionTypeBEList = transBS.getList();
        this.lstPostTrans.DataSource = PostingTransactionTypeBEList;
        lstPostTrans.DataBind();

    }
    public void SelectSortData()
    {
        IList<PostingTransactionTypeBE> transactionList = new List<PostingTransactionTypeBE>();
        transactionList = transBS.getList();

        switch (SortBy)
        {

            case "TRANSACTIONTYPE":
                if (SortDir == "ASC")
                    transactionList = (transactionList.OrderBy(o => o.TRANSACTIONTYPE)).ToList();
                else if (SortDir == "DESC")
                    transactionList = (transactionList.OrderByDescending(o => o.TRANSACTIONTYPE)).ToList();
                break;
            case "MAIN":
                if (SortDir == "ASC")
                    transactionList = (transactionList.OrderBy(o => o.MAIN_NBR)).ToList();
                else if (SortDir == "DESC")
                    transactionList = (transactionList.OrderByDescending(o => o.MAIN_NBR)).ToList();
                break;
            case "SUB":
                if (SortDir == "ASC")
                    transactionList = (transactionList.OrderBy(o => o.SUB_NBR)).ToList();
                else if (SortDir == "DESC")
                    transactionList = (transactionList.OrderByDescending(o => o.SUB_NBR)).ToList();
                break;
            case "COMPANY":
                if (SortDir == "ASC")
                    transactionList = (transactionList.OrderBy(o => o.COMP_TXT)).ToList();
                else if (SortDir == "DESC")
                    transactionList = (transactionList.OrderByDescending(o => o.SUB_NBR)).ToList();
                break;
            case "INVOICE":
                if (SortDir == "ASC")
                    transactionList = (transactionList.OrderBy(o => o.INVOICBL_IND_txt)).ToList();
                else if (SortDir == "DESC")
                    transactionList = (transactionList.OrderByDescending(o => o.INVOICBL_IND_txt)).ToList();
                break;
            case "POSTING":
                if (SortDir == "ASC")
                    transactionList = (transactionList.OrderBy(o => o.MISC_POSTS_IND_txt)).ToList();
                else if (SortDir == "DESC")
                    transactionList = (transactionList.OrderByDescending(o => o.MISC_POSTS_IND_txt)).ToList();
                break;
            case "TPAMANUAL":
                if (SortDir == "ASC")
                    transactionList = (transactionList.OrderBy(o => o.THRD_PTY_ADMIN_MNL_IND_txt)).ToList();
                else if (SortDir == "DESC")
                    transactionList = (transactionList.OrderByDescending(o => o.THRD_PTY_ADMIN_MNL_IND_txt)).ToList();
                break;
        }
        this.lstPostTrans.DataSource = transactionList;
        lstPostTrans.DataBind();


    }

    protected void transDisableRow(object sender, CommandEventArgs e)
    {
        bool succeed = false;
        int POST_TRNS_TYP_ID = Convert.ToInt32(e.CommandArgument);
        PostingTransactionTypeBE setupBE;
        setupBE = transBS.LoadData(POST_TRNS_TYP_ID);
        //Concurrency Issue
        //PostingTransactionTypeBE setupBEold = (PostingTransactionTypeBEList.Where(o => o.POST_TRANS_TYP_ID.Equals(POST_TRNS_TYP_ID))).First();
        //bool con = ShowConcurrentConflict(Convert.ToDateTime(setupBEold.UPDATE_DATE), Convert.ToDateTime(setupBE.UPDATE_DATE));
        //if (!con)
        //    return;
        //End
        setupBE.ACTV_IND = false;
        succeed = transBS.SaveSetupData(setupBE);
        ShowConcurrentConflict(succeed, setupBE.ErrorMessage);
        SelectData();


    }

    #endregion


    /*------------------------------------------------------------------------------
     Tab Name :Issue CheckList Maintenance
     
     The Below Code is Related to Issue Checklist Maintenance Tab.
     It Contains two list views lstIssues and lstIssueDetails and related events
     The above portion includes only properties related to this tab.
    ---------------------------------------------------------------------------------*/
    #region Issue CheckList Maintenance Tab
    /// <summary>
    /// Invoked when the Close Link in Issue Checklist Maitenance tab is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region lnkIssueDetailsClose_Click
    protected void lnkIssueDetailsClose_Click(object sender, EventArgs e)
    {
        pnlIssueDtailsList.Visible = false;
        divIssueDetails.Visible = false;
        lstIssues.Enabled = true;
        HtmlTableRow tr;
        if (ViewState["ILMSelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["ILMSelectedIndex"]);
            tr = (HtmlTableRow)lstIssues.Items[index].FindControl("trItemTemplate");
            tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
        }
        ViewState["ILMSelectedIndex"] = null;

    }
    #endregion
    /// <summary>
    /// Invoked when row is added
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region IssuesDataBoundList
    protected void IssuesDataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trItemTemplate");
            if (tr != null)
            {
                tr.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
                tr.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            }
        }
    }
	#endregion
    /// <summary>
    /// Invoked when the Details link in the lstIssues is Clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region lstIssueType_SelectedIndexChanging
    protected void lstIssueType_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
    {
        HtmlTableRow tr;
        if (ViewState["ILMSelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["ILMSelectedIndex"]);
            tr = (HtmlTableRow)lstIssues.Items[index].FindControl("trItemTemplate");
            tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
        }
        tr = (HtmlTableRow)lstIssues.Items[e.NewSelectedIndex].FindControl("trItemTemplate");
        LinkButton lnk = (LinkButton)lstIssues.Items[e.NewSelectedIndex].FindControl("lnkSelect");
        ViewState["ILMSelectedIndex"] = e.NewSelectedIndex;
        tr.Attributes["class"] = "SelectedItemTemplate";
        ViewState["LOOKUPID"] = Convert.ToInt32(lnk.CommandArgument);
        lblIssueDetails.Text = ((Label)lstIssues.Items[e.NewSelectedIndex].FindControl("lblIssueCategoryName")).Text + " Details";
        //Function to bind the LookupTypeDetails Data
        DisplayIssueDetails(Convert.ToInt32(lnk.CommandArgument));
        pnlIssueDtailsList.Visible = true;
        divIssueDetails.Visible = true;
        lstIssues.Enabled = false;

    }

    #endregion
   /// <summary>
   /// Function to bind lstIssueDetails listview 
   /// </summary>
    /// <param name="intLookupID"></param>
    #region DisplayIssueDetails
    private void DisplayIssueDetails(int intLookupID)
    {
        try
        {
            QCMasterIssueListBEList = QCMasterIssueListBS.getIssuesListALL(intLookupID);
            lstIssueDetails.DataSource = QCMasterIssueListBEList;
            lstIssueDetails.DataBind();
        }
        catch (Exception ex)
        {
            RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
            ShowError(ex.Message,ex);
        }
    }
    #endregion
    /// <summary>
    /// Function to bind the Lookup details in listview
    /// </summary>
    #region BindIssueList()
    private void BindIssueList()
    {
        try
        {
            LookupBEList = LookupService.getLookupData("MASTER ISSUE CHECKLIST TYPE");
            lstIssues.DataSource = LookupBEList;
            lstIssues.DataBind();
        }
        catch (Exception ex)
        {
            RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
            ShowError(ex.Message, ex);
        }
    }
    #endregion
    /// <summary>
    /// Invoked when the edit link is clicked in the lstissueDetails Listview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region IssuDetailsEditList
    protected void IssuDetailsEditList(Object sender, ListViewEditEventArgs e)
    {
            lstIssueDetails.EditIndex = e.NewEditIndex;
            lstIssueDetails.InsertItemPosition = InsertItemPosition.None;
            DisplayIssueDetails(Convert.ToInt16(ViewState["LOOKUPID"].ToString()));
    }
    #endregion
    // Invoked when the Cancel Link is clicked
    #region IssuDetailsCancelList
    protected void IssuDetailsCancelList(Object sender, ListViewCancelEventArgs e)
    {

        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstIssueDetails.EditIndex = -1;
            lstIssueDetails.InsertItemPosition = InsertItemPosition.FirstItem;
            //Function to bind the issueDetails Data
            DisplayIssueDetails(Convert.ToInt16(ViewState["LOOKUPID"].ToString()));
        }

    }
    #endregion
    /// <summary>
    /// Invoked when the save,Enable or disable button is clicked in lstIssueDetails Listview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
     #region IssuDetailsCommandList
    protected void IssuDetailsCommandList(Object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SAVE")
        {
            SaveIssueDetails(e.Item);
        }
       
        else if (e.CommandName.ToUpper() == "DISABLE" || e.CommandName.ToUpper() == "ENABLE")
        {
            //Function call to Enable or Disable the Record
            DisableIssueDetailsRow(Convert.ToInt32(e.CommandArgument), e.CommandName == "DISABLE" ? false : true);
        }

    }
#endregion
    /// <summary>
    /// Function to Save the IssueDetails
    /// </summary>
    /// <param name="e"></param>
   #region SaveIssueDetails
    private void SaveIssueDetails(ListViewItem e)
    {
        string strIssueText = ((TextBox)e.FindControl("txtIssueName")).Text;
        int intSortNbr = Convert.ToInt32(((TextBox)e.FindControl("txtStrNbr")).Text);
        int intIssueCatID = Convert.ToInt32((ViewState["LOOKUPID"].ToString()));
        bool Flag = QCMasterIssueListBS.IsExistsIssueName(intIssueCatID, 0, strIssueText);
        if (!Flag)
        {
            try
            {
                bool blFinance = ((CheckBox)e.FindControl("chkFinInd")).Checked;
                QCMasterIssueListBE QcMasterBE = new QCMasterIssueListBE();
                QcMasterBE.IssCatgID = intIssueCatID;
                QcMasterBE.IssueText = strIssueText;
                QcMasterBE.Str_Nbr = intSortNbr;
                QcMasterBE.FinancialIndicator = blFinance;
                QcMasterBE.CreatedDate = System.DateTime.Now;
                QcMasterBE.CreatedUserID = CurrentAISUser.PersonID;
                QcMasterBE.ACTIVE = true;
                Flag = QCMasterIssueListBS.Update(QcMasterBE);
                if (Flag)
                {
                    DisplayIssueDetails(Convert.ToInt16(ViewState["LOOKUPID"].ToString()));
                }
            }
            catch (Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                ShowError(ex.Message,ex);
            
            }
       
        }
        else
        {
            ShowMessage("The record cannot be saved. An identical record already exists.");
        }
    }
   #endregion
    /// <summary>
    /// Invoked when the Update button is clicked in lstIssueDetails Listview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region IssuDetailsUpdateList
    protected void IssuDetailsUpdateList(Object sender, ListViewUpdateEventArgs e)
    {
        int intQCMasterIssueID = Convert.ToInt32(((HiddenField)lstIssueDetails.Items[e.ItemIndex].FindControl("hidEditQcIssuID")).Value.ToString());
        QCMasterIssueListBE QCMasterIssueBE = new QCMasterIssueListBE();
        
        QCMasterIssueBE = QCMasterIssueListBS.getQCMasterIssueRow(intQCMasterIssueID);
        //Concurrency Issue
        QCMasterIssueListBE QCMasterIssueBEold = (QCMasterIssueListBEList.Where(o => o.QualityCntrlMstrIsslstID.Equals(intQCMasterIssueID))).First();
        bool con = ShowConcurrentConflict(Convert.ToDateTime(QCMasterIssueBEold.UpdatedDate), Convert.ToDateTime(QCMasterIssueBE.UpdatedDate));
        if (!con)
            return;
        //End
        int intIssueCatID = 0;
        if (QCMasterIssueBE.IssCatgID != null)
        {
            intIssueCatID = Convert.ToInt32(QCMasterIssueBE.IssCatgID.ToString());
        }
        string strIssueText = ((TextBox)lstIssueDetails.Items[e.ItemIndex].FindControl("txtEditIssueName")).Text;
        int intSortNbr = Convert.ToInt32(((TextBox)lstIssueDetails.Items[e.ItemIndex].FindControl("txtEditStrNbr")).Text);
        bool blFinance = ((CheckBox)lstIssueDetails.Items[e.ItemIndex].FindControl("chkFinInd")).Checked;
        QCMasterIssueBE.IssueText = strIssueText;
        QCMasterIssueBE.Str_Nbr = intSortNbr;
        QCMasterIssueBE.FinancialIndicator = blFinance;
        QCMasterIssueBE.UpdatedDate = System.DateTime.Now;
        QCMasterIssueBE.UpdatedUserID = CurrentAISUser.PersonID;
        bool Flag = QCMasterIssueListBS.IsExistsIssueName(intIssueCatID, QCMasterIssueBE.QualityCntrlMstrIsslstID, QCMasterIssueBE.IssueText);
            if (!Flag)
            {
                try
                {
                    Flag = QCMasterIssueListBS.Update(QCMasterIssueBE);
                    ShowConcurrentConflict(Flag, QCMasterIssueBE.ErrorMessage);
                    lstIssueDetails.EditIndex = -1;
                    lstIssueDetails.InsertItemPosition = InsertItemPosition.FirstItem;
                    if (Flag)
                    {
                        DisplayIssueDetails(Convert.ToInt16(ViewState["LOOKUPID"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                    ShowError(ex.Message,ex);
                }
            }
            else
            {
                ShowMessage("The record cannot be saved. An identical record already exists.");
            }


    }
    #endregion
    /// <summary>
    /// Invoked when the row is added
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region IssuDetailsDataBoundList
    protected void IssuDetailsDataBoundList(Object sender, ListViewItemEventArgs e)
    {
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
                imgDelete.Attributes.Add("onclick", hid.Value == "True" ? "return confirm('Are you sure you want to disable?');" : "return confirm('Are you sure you want to enable?');");
            }
        }
    }
    #endregion
    /// <summary>
    /// method to update the active status
    /// </summary>
    /// <param name="lkPKID"></param>
    /// <param name="Flag"></param>
    #region DisableIssueDetailsRow
    protected void DisableIssueDetailsRow(int lkPKID, bool Flag)
    {
        QCMasterIssueListBE QCMasterIssueBE = new QCMasterIssueListBE();
        QCMasterIssueBE = QCMasterIssueListBS.getQCMasterIssueRow(lkPKID);
        //Concurrency Issue
        QCMasterIssueListBE QCMasterIssueBEold = (QCMasterIssueListBEList.Where(o => o.QualityCntrlMstrIsslstID.Equals(lkPKID))).First();
        bool con = ShowConcurrentConflict(Convert.ToDateTime(QCMasterIssueBEold.UpdatedDate), Convert.ToDateTime(QCMasterIssueBE.UpdatedDate));
        if (!con)
            return;
        //End
        QCMasterIssueBE.ACTIVE = Flag;
        QCMasterIssueBE.UpdatedUserID = CurrentAISUser.PersonID;
        QCMasterIssueBE.UpdatedDate = System.DateTime.Now;
        Flag = QCMasterIssueListBS.Update(QCMasterIssueBE);
        ShowConcurrentConflict(Flag, QCMasterIssueBE.ErrorMessage);
            if (Flag)
            {
                //Function to bind the IssueDetails Data
                DisplayIssueDetails(Convert.ToInt16(ViewState["LOOKUPID"].ToString()));
               
            }

    }
    #endregion
    #endregion
}
