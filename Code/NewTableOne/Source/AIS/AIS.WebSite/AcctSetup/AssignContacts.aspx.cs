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
using ZurichNA.LSP.Framework.Business;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.Business.Logic;
using System.Collections.Generic;
using ZurichNA.AIS.ExceptionHandling;

public partial class AcctSetup_AssignContacts : AISBasePage
{
    private AssignContactsBS ContactsBS;

    private AssignContactsBS contactsBS
    {
        get
        {
            if (ContactsBS == null)
            {
                ContactsBS = new AssignContactsBS();
            }
            return ContactsBS;
        }
    }

    private int prgmPrdID
    {
        get { return ((ViewState["prgmPrdID"] == null) ? 0 : Convert.ToInt32(ViewState["prgmPrdID"])); }
        set { ViewState["prgmPrdID"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.Page.Title = "Contact Assignment";

        //Checks Exiting without Save
        ArrayList list = new ArrayList();
        list.Add(lbClose);
        ProcessExitFlag(list);
    }

    public void ProgramPeriod_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        int prmPerdID = Convert.ToInt32(e.CommandArgument);
        Label EffPeriod = (Label)e.Item.FindControl("lblstartDate");
        Label ExpPeriod = (Label)e.Item.FindControl("lblendDate");
        lbClose.Visible = true;
        panContactInfo.Visible = true;
        prgmPrdID = prmPerdID;
        lblCustomerContactInfo.Visible = true;
        lblEffdt.Text = EffPeriod.Text;
        lblExpdt.Text = " - " + ExpPeriod.Text;
        ((ListView)ProgramPeriod.FindControl("lstProgramPeriod")).Enabled = false;
        //To Populate the Listview
        GetContactsListView(prmPerdID);

    }

    protected void LstContacts_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        LstContacts.EditIndex = e.NewEditIndex;
        GetContactsListView(prgmPrdID);
    }

    protected void LstContacts_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {

            DropDownList ddlContactType = (DropDownList)e.Item.FindControl("ddlContactType");
            DropDownList ddlContactName = (DropDownList)e.Item.FindControl("ddlContactName");
            DropDownList ddlDelMethod = (DropDownList)e.Item.FindControl("ddlDelMethod");
            Label lblContactTypeID = (Label)e.Item.FindControl("lblContactTypeID");
            Label lblContactNameId = (Label)e.Item.FindControl("lblContactNameId");
            Label lblDelMethodID = (Label)e.Item.FindControl("lblDelMethodID");
            
            
            if ((ddlContactType != null) & (lblContactTypeID != null))
            {
//                ddlContactType.SelectedIndex = ddlContactType.Items.IndexOf(ddlContactType.Items.FindByValue(lblContactTypeID.Text.ToString()));

                AddInActiveLookupData(ref ddlContactType, lblContactTypeID.Text);
            }
            if ((ddlContactName != null) & (lblContactNameId != null))
            {
                int contactTypeID = int.Parse(ddlContactType.SelectedValue);
                int custmrID = AISMasterEntities.AccountNumber;
                ContactNamesDataSource.SelectParameters["ContTypId"].DefaultValue = contactTypeID.ToString();
                ContactNamesDataSource.SelectParameters["CustmrID"].DefaultValue = custmrID.ToString();
                ContactNamesDataSource.Select();
                ContactNamesDataSource.DataBind();
                ddlContactName.DataBind();
//                ddlContactName.SelectedIndex = ddlContactName.Items.IndexOf(ddlContactName.Items.FindByValue(lblContactNameId.Text.ToString()));
                int contactID = 0;
                int.TryParse(lblContactNameId.Text,out contactID);
                AddInActiveContactData(ref ddlContactName, contactID);
            }
            if ((ddlDelMethod != null) & (lblDelMethodID != null))
            {
                ddlDelMethod.DataBind();
//                ddlDelMethod.SelectedIndex = ddlDelMethod.Items.IndexOf(ddlDelMethod.Items.FindByValue(lblDelMethodID.Text.ToString()));
                AddInActiveLookupData(ref ddlDelMethod, lblDelMethodID.Text);
            }
            ImageButton ibtnEnableDisable = (ImageButton)e.Item.FindControl("ibtnEnableDisable");
            if (ibtnEnableDisable != null)
            {
                Label lblActive = (Label)e.Item.FindControl("lblActive");
                if (lblActive.Text == "True")
                {
                    ibtnEnableDisable.ImageUrl = "~/images/disabled.GIF";
                    ibtnEnableDisable.CommandName = "DISABLE";
                    ibtnEnableDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable this record?');");
                    ibtnEnableDisable.ToolTip = "Click here to Disable this Contact";
                }
                else
                {
                    ibtnEnableDisable.ImageUrl = "~/images/enabled.GIF";
                    ibtnEnableDisable.CommandName = "ENABLE";
                    ibtnEnableDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable this record?');");
                    ibtnEnableDisable.ToolTip = "Click here to Enable this Contact";
                }
            } 


        }
    }

    protected void ddlContactType_OnSelectedIndexChanged(Object sender, EventArgs e)
    {

        DropDownList ddlContactType = (DropDownList)sender;
        int contactTypeID = int.Parse(ddlContactType.SelectedValue);
        int custmrID = AISMasterEntities.AccountNumber;
        ContactNamesDataSource.SelectParameters["ContTypId"].DefaultValue = contactTypeID.ToString();
        ContactNamesDataSource.SelectParameters["CustmrID"].DefaultValue = custmrID.ToString();
        ContactNamesDataSource.Select();
        ContactNamesDataSource.DataBind();

    }
    
    protected void LstContacts_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {

        if ((e.CommandName.ToUpper() == "SAVE"))
        {
            try
            {
            DropDownList ddlContactType = (DropDownList)e.Item.FindControl("ddlContactType");
            DropDownList ddlContactName = (DropDownList)e.Item.FindControl("ddlContactName");
            DropDownList ddlDelMethod = (DropDownList)e.Item.FindControl("ddlDelMethod");
            Label PreAdjPgmRelId = (Label)e.Item.FindControl("lblhdPreAdjPgmRelId");
            CheckBox chkSendInv = (CheckBox)e.Item.FindControl("chkSendInv");
            AssignContactsBE contactsBE = new AssignContactsBE();
            contactsBE.PREM_ADJ_PGM_ID = prgmPrdID;
            contactsBE.PERS_ID = Convert.ToInt32(ddlContactName.SelectedValue);
            contactsBE.SND_INVC_IND = chkSendInv.Checked;
            if (ddlDelMethod.SelectedIndex==0)
                ddlDelMethod.SelectedValue = (new BLAccess()).GetLookUpID(GlobalConstants.DeliveryMethodType.USPS, "DELIVERY METHOD").ToString();
            contactsBE.COMMU_MEDUM_ID = Convert.ToInt32(ddlDelMethod.SelectedValue);
            contactsBE.CRTE_DT = System.DateTime.Now;
            contactsBE.CRTE_USER_ID = CurrentAISUser.PersonID;
            contactsBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
            contactsBE.ACTV_IND = true;
            contactsBS.SaveContactsData(contactsBE);
            
            }
            catch (System.Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                ShowError(ex.Message, ex);
            }
            LstContacts.EditIndex = -1;
            

        }
        if ((e.CommandName.ToUpper() == "UPDATE"))
        {
            try
            {
            DropDownList ddlContactType = (DropDownList)e.Item.FindControl("ddlContactType");
            DropDownList ddlContactName = (DropDownList)e.Item.FindControl("ddlContactName");
            DropDownList ddlDelMethod = (DropDownList)e.Item.FindControl("ddlDelMethod");
            Label PreAdjPgmRelId = (Label)e.Item.FindControl("lblhdPreAdjPgmRelId");
            CheckBox chkSendInv = (CheckBox)e.Item.FindControl("chkSendInv");
            AssignContactsBE contactsBE = new AssignContactsBE();
            contactsBE = contactsBS.LoadData(Convert.ToInt32(PreAdjPgmRelId.Text));
            //Concurrency Issue
            AssignContactsBE contactsBEold = (AssignContactsBEList.Where(o => o.PREM_ADJ_PGM_PERS_REL_ID.Equals(Convert.ToInt32(PreAdjPgmRelId.Text)))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(contactsBEold.UPDT_DT), Convert.ToDateTime(contactsBE.UPDT_DT));
            if (!con)
                return;
            //End
            contactsBE.PREM_ADJ_PGM_PERS_REL_ID = Convert.ToInt32(PreAdjPgmRelId.Text);
            contactsBE.PREM_ADJ_PGM_ID = prgmPrdID;
            contactsBE.PERS_ID = Convert.ToInt32(ddlContactName.SelectedValue);
            contactsBE.SND_INVC_IND = chkSendInv.Checked;
            if (ddlDelMethod.SelectedIndex == 0)
                ddlDelMethod.SelectedValue = (new BLAccess()).GetLookUpID(GlobalConstants.DeliveryMethodType.USPS, "DELIVERY METHOD").ToString();
            contactsBE.COMMU_MEDUM_ID = Convert.ToInt32(ddlDelMethod.SelectedValue);
            contactsBE.UPDT_USER_ID = CurrentAISUser.PersonID;
            contactsBE.UPDT_DT = DateTime.Now;
            contactsBE.CUSTMR_ID = AISMasterEntities.AccountNumber;
            contactsBE.ACTV_IND = true;
            bool Flag=contactsBS.SaveContactsData(contactsBE);
            ShowConcurrentConflict(Flag, contactsBE.ErrorMessage);
            }
            catch (System.Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                ShowError(ex.Message, ex);
            }
            LstContacts.EditIndex = -1;
           

        }

        if ((e.CommandName.ToUpper() == "DISABLE") || (e.CommandName.ToUpper() == "ENABLE"))
        {
            try
            {
            bool succeed = false;
            int PREM_PERS_REL_ID = Convert.ToInt32(e.CommandArgument);
            AssignContactsBE contactsBE;
            contactsBE = contactsBS.LoadData(PREM_PERS_REL_ID);
            //Concurrency Issue
            AssignContactsBE contactsBEold = (AssignContactsBEList.Where(o => o.PREM_ADJ_PGM_PERS_REL_ID.Equals(PREM_PERS_REL_ID))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(contactsBEold.UPDT_DT), Convert.ToDateTime(contactsBE.UPDT_DT));
            if (!con)
                return;
            //End
            if (e.CommandName.ToUpper() == "DISABLE")
            contactsBE.ACTV_IND = false;
            else
            contactsBE.ACTV_IND = true;
            contactsBE.UPDT_USER_ID = CurrentAISUser.PersonID;
            contactsBE.UPDT_DT = DateTime.Now;
            succeed = contactsBS.SaveContactsData(contactsBE);
            }
            catch (System.Exception ex)
            {
                RetroDatabaseException myException = new RetroDatabaseException(ex.Message, ex);
                ShowError(ex.Message, ex);
            }
            
         }
        ContactNamesDataSource.SelectParameters["ContTypId"].DefaultValue = "0";
        ContactNamesDataSource.SelectParameters["CustmrID"].DefaultValue = "0";
        ContactNamesDataSource.Select();
        ContactNamesDataSource.DataBind();
        GetContactsListView(prgmPrdID);
    }

    protected void LstContacts_ItemCancel(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            LstContacts.EditIndex = -1;
           
        }
        else if (e.CancelMode == ListViewCancelMode.CancelingInsert)
        {
            LstContacts.InsertItemPosition = InsertItemPosition.None;
            
        }
        GetContactsListView(prgmPrdID);
    }

    protected void LstContacts_ItemInserting(Object sender, ListViewInsertEventArgs e)
    {
        LstContacts.InsertItemPosition = InsertItemPosition.None;
    }

    protected void LstContacts_ItemUpdating(Object sender, ListViewUpdateEventArgs e)
    {
        
    }

    /// <summary>
    /// To close the list view when the user clicks on the "close" link.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CloseContactInfo(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        panContactInfo.Visible = false;
        lbClose.Visible = false;
        lblCustomerContactInfo.Visible = false;
        lblEffdt.Text = String.Empty;
        lblExpdt.Text = String.Empty;
        ((ListView)ProgramPeriod.FindControl("lstProgramPeriod")).Enabled = true;


    }

    /// <summary>
    /// This method retrieves the contacts list for the given ProgramPeriod ID
    /// </summary>
    /// <param name="ProgramPeriodId"></param>
    public void GetContactsListView(int programPeriodId)
    {
        AssignContactsBEList = contactsBS.GetContactsData(programPeriodId);
        this.LstContacts.DataSource = AssignContactsBEList;
        LstContacts.DataBind();


    }

    #region Maintaining Session for AssignContactsBEList
    private IList<AssignContactsBE> AssignContactsBEList
    {
        get
        {
            //if (Session["AssignContactsBEList"] == null)
            //    Session["AssignContactsBEList"] = new List<AssignContactsBE>();
            //return (IList<AssignContactsBE>)Session["AssignContactsBEList"];
            if (RetrieveObjectFromSessionUsingWindowName("AssignContactsBEList") == null)
                SaveObjectToSessionUsingWindowName("AssignContactsBEList", new List<AssignContactsBE>());
            return (IList<AssignContactsBE>)RetrieveObjectFromSessionUsingWindowName("AssignContactsBEList");
        }
        set 
        {
            SaveObjectToSessionUsingWindowName("AssignContactsBEList", value); 
        }
    }
    #endregion





}

