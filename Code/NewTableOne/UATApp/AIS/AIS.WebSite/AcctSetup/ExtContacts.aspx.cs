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
using System.Reflection;


public partial class ExtContacts : AISBasePage
{
    protected void LoadData(object sender, EventArgs e)
    {

        if (tcExternalContacts.ActiveTabIndex == 0)
        {
            if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            {
                if (isNew != null && !isNew)
                    btnAdd.Enabled = true;
            }
        }
        else
        {
            btnAdd.Enabled = false;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.Page.Title = "External Contacts";
        if (!Page.IsPostBack)
        {
            ddlExtContacts.Enabled = true;
            ddlContactNameSearch.Enabled = true;
            BindBrokerListView();
            //ExtTransactionWrapper = new AISBusinessTransaction();
            if (WEBPAGEROLE == GlobalConstants.ApplicationSecurityGroup.Inquiry)
                btnAdd.Enabled = false;

        }

        //Checks Exiting without Save
        ArrayList list = new ArrayList();
        list.Add(txtAddress1);
        list.Add(txtAddress2);
        list.Add(txtCity);
        list.Add(txtEmail);
        list.Add(txtFax);
        list.Add(txtFirstName);
        list.Add(txtLastName);
        list.Add(txtTelephone1);
        list.Add(txtTelephone2);
        list.Add(txtZipCode);
        list.Add(ddlContactNames);
        list.Add(ddlExtContacts);
        list.Add(ddlState);
        list.Add(ddlTitle);
        list.Add(btnAdd);
        list.Add(btnCancel);
        list.Add(btnCopy);
        list.Add(btnSave);
        list.Add(btnSearch);
        list.Add(lbCloseDetails);
        ProcessExitFlag(list);
    }

    #region ExternalContacTab

    //protected AISBusinessTransaction ExtTransactionWrapper
    //{
    //    get
    //    {
    //        if ((AISBusinessTransaction)Session["ExtTransaction"] == null)
    //            Session["ExtTransaction"] = new AISBusinessTransaction();
    //        return (AISBusinessTransaction)Session["ExtTransaction"];
    //    }
    //    set
    //    {
    //        Session["ExtTransaction"] = value;
    //    }
    //}

    private PersonBS extContactBizServices;
    private PersonBS ExtContactBizServices
    {
        get
        {
            if (extContactBizServices == null)
            {
                extContactBizServices = new PersonBS();
                //extContactBizServices.AppTransactionWrapper = ExtTransactionWrapper;
            }
            return extContactBizServices;
        }
    }
    /// <summary>
    /// a property for Postal Address Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>PostAddressBS</returns>
    private PostAddressBS postAddressBizService;
    private PostAddressBS PostAddressBizService
    {
        get
        {
            if (postAddressBizService == null)
            {
                postAddressBizService = new PostAddressBS();
                //postAddressBizService.AppTransactionWrapper = ExtTransactionWrapper;
            }
            return postAddressBizService;
        }
    }

    private CustomerContactBS custContBizServices;
    private CustomerContactBS CustContBizServices
    {
        get
        {
            if (custContBizServices == null)
            {
                custContBizServices = new CustomerContactBS();
                //custContBizServices.AppTransactionWrapper = ExtTransactionWrapper;
            }
            return custContBizServices;
        }
    }
   
    protected bool isNew
    {
        get
        {
            if (ViewState["ISNEW"] != null)
            { return (bool)ViewState["ISNEW"]; }
            else
            {
                ViewState["ISNEW"] = false;
                return false;
            }
        }
        set { ViewState["ISNEW"] = value; }
    }
    protected bool isCopy
    {
        get
        {
            if (ViewState["ISCOPY"] != null)
            { return (bool)ViewState["ISCOPY"]; }
            else
            {
                ViewState["ISCOPY"] = false;
                return false;
            }
        }
        set { ViewState["ISCOPY"] = value; }
    }
    protected int selectedPersonID
    {
        get
        {
            if (hidSelValue.Value != null) return Convert.ToInt32(hidSelValue.Value);
            else { return -1; }
        }
        set { hidSelValue.Value = value.ToString(); }
    }
    protected int selectedPostAddID
    {
        get
        {
            if (hidSelPostAddID.Value != null) return Convert.ToInt32(hidSelPostAddID.Value);
            else { return -1; }
        }
        set { hidSelPostAddID.Value = value.ToString(); }
    }

    /// <summary>
    /// a property for Person Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>PersonBE</returns>
    private PersonBE personBE
    {
        get { return (PersonBE)Session["PERSONBE"]; }
        set { Session["PERSONBE"] = value; }
    }
    /// <summary>
    /// a property for Postal Address Business Entity Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>PostalAddressBE</returns>
    private PostalAddressBE postBE
    {
        get { return (PostalAddressBE)Session["POSTADDRESSBE"]; }
        set { Session["POSTADDRESSBE"] = value; }
    }

    /// <summary>
    /// Saves entire row into database
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SAVE_Click(Object sender, EventArgs e)
    {
        try
        {
            if (isNew == true)
            {
                
                SaveNewEntity();
                ClientScript.RegisterClientScriptBlock(this.GetType(), "postback",
                    "JavaScript:_dopostback('" + btnSave.ClientID.ToString() + "','Save')");
                VSSaveExternal.Style["display"] = "none";
                //VSSaveContacts.Style["display"] = "none";
                //VSEditContacts.Style["display"] = "none";
            }
            else
            {
                UpdatEntity();
                //VSSaveExternal.Visible = false;
                
                VSSaveExternal.Style["display"] = "none";
                //VSSaveContacts.Style["display"] = "none";
                //VSEditContacts.Style["display"] = "none";
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }

        //pnlDetails.Visible = false;
        //lblExternalContactsDetails.Visible = false;
        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            btnAdd.Enabled = true;
        //lbCloseDetails.Visible = false;
        //lstExtContact.Enabled = true;

    }
    protected void btnCancel_Click(Object sender, EventArgs e)
    {
        if (isNew)
        {
            if (isCopy)
            {
                isNew = false;
                isCopy = false;
                btnSave.Text = "Update";
                ShowMessage("External Contacts Copy process has been Cancelled");
                if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                    btnCopy.Enabled = true;
                DisplayDetails(selectedPersonID);
            }
            else
            {
                ClearFileds();
            }
        }
        else
        {
            DisplayDetails(selectedPersonID);
        }

    }
    protected void ddlExtContacts_Selected(object obj, EventArgs e)
    {
        try
        {
            if (int.Parse(ddlExtContacts.SelectedValue) != 0)
            {
                PersonBS person = new PersonBS();
                IList<LookupBE> ExtOrg = new List<LookupBE>();
                ExtOrg = person.getNamesList(int.Parse(ddlExtContacts.SelectedValue), ddlExtContacts.SelectedItem.Text.ToString());
                ddlContactNameSearch.DataSource = ExtOrg;
                ddlContactNameSearch.DataValueField = "LookUpID";
                ddlContactNameSearch.DataTextField = "LookUpName";
                ddlContactNameSearch.DataBind();

                ddlContactNameSearch.Items.FindByValue("0").Selected = true;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
        //lblType.Text = ddlExtContacts.SelectedItem.Text + " Name";
        //if (Convert.ToInt32(ddlExtContacts.SelectedItem.Value) != 236)
        //{
        //    ddlContactNameSearch.DataSource = (new BrokerBS()).GetBrokerData().Where(br => br.CONTACT_TYPE_ID == Convert.ToInt32(ddlExtContacts.SelectedItem.Value));
        //    ddlContactNameSearch.DataTextField = "FULL_NAME";
        //    ddlContactNameSearch.DataValueField = "EXTRNL_ORG_ID";
        //    ddlContactNameSearch.DataBind();
        //}
        //else
        //{
        //    ddlContactNameSearch.DataSource = ((new PersonBS()).getInsuredNames(Convert.ToInt32(ddlExtContacts.SelectedItem.Value))).Select(per => per.EXTERNAL_ORGN_TXT).Distinct();
        //    ddlContactNameSearch.DataTextField = "";
        //    ddlContactNameSearch.DataValueField = "";
        //    ddlContactNameSearch.DataBind();
        //}
        //ListItem li = new ListItem("(Select)", "0");
        //ddlContactNameSearch.Items.Insert(0, li);
    }
    protected void lbCloseDetails_Click(object sender, EventArgs e)
    {
        isNew = false;
        btnSearch.Enabled = true;
        pnlDetails.Visible = false;
        lblExternalContactsDetails.Visible = false;
        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            btnAdd.Enabled = true;
        lstExtContact.Enabled = true;
        lbCloseDetails.Visible = false;
    }
    protected void SaveNewEntity()
    {
        bool boolPerResult = true;
        bool boolPostResult = true;
        bool boolCustCtAddResult = true;

        personBE = new PersonBE();
        personBE.CREATEDDATE = DateTime.Now;
        personBE.CREATEDUSER_ID = CurrentAISUser.PersonID;

        postBE = new PostalAddressBE();
        postBE.CREATEDDATE = DateTime.Now;
        postBE.CREATEDUSER = CurrentAISUser.PersonID;

        personBE.CONCTACT_TYPE_ID = Convert.ToInt32(ddlContactType.SelectedValue);
        if (Convert.ToInt32(ddlContactNames.SelectedValue) != 0)
            personBE.EXTERNAL_ORGN_ID = Convert.ToInt32(ddlContactNames.SelectedValue);
        else
            personBE.EXTERNAL_ORGN_ID = null;
        // IF Selected Contact Type is INSURED then External OrganizationID must set to NULL
        if (ddlContactType.SelectedItem.Text == ExternalContactType.INSURED)
            personBE.EXTERNAL_ORGN_ID = null;
        if (Convert.ToInt32(ddlTitle.SelectedValue) != 0)
            personBE.TITLE_ID = Convert.ToInt32(ddlTitle.SelectedValue);
        else
            personBE.TITLE_ID = null;
        personBE.FORENAME = (txtFirstName.Text == string.Empty ? null : txtFirstName.Text);
        personBE.SURNAME = (txtLastName.Text == string.Empty ? null : txtLastName.Text);
        //personBE.TELEPHONE1 = (getOnlyNumber(txtTelephone1.Text) == string.Empty ? null : getOnlyNumber(txtTelephone1.Text));
        personBE.TELEPHONE1 = getOnlyNumber(txtTelephone1.Text);
        personBE.TELEPHONE2 = (getOnlyNumber(txtTelephone2.Text) == string.Empty ? null : getOnlyNumber(txtTelephone2.Text));
        personBE.FAX = (getOnlyNumber(txtFax.Text) == string.Empty ? null : getOnlyNumber(txtFax.Text));
        //personBE.EMAIL = (txtEmail.Text == string.Empty ? null : txtEmail.Text);
        personBE.EMAIL = txtEmail.Text;
        personBE.ACTIVE = true;

        postBE.ADDRESS1 = (txtAddress1.Text == string.Empty ? null : txtAddress1.Text);
        postBE.ADDRESS2 = (txtAddress2.Text == string.Empty ? null : txtAddress2.Text);
        postBE.CITY = (txtCity.Text == string.Empty ? null : txtCity.Text);
        postBE.STATE_ID = Convert.ToInt32(ddlState.SelectedValue);
        postBE.ZIP_CODE = (txtZipCode.Text.Replace("-", string.Empty)).Replace("_", string.Empty);
        //Checking if already a record existing // Using InternalContact method as it is same for both
        bool boolIsRecordExists = false;
        //bool boolIsRecordExists = ExtContactBizServices.IsExistsExtContact(txtFirstName.Text, txtLastName.Text, 0, Convert.ToInt32(ddlContactType.SelectedValue), Convert.ToInt32(ddlContactNames.SelectedValue));
        //bool boolIsRecordExists = ExtContactBizServices.IsExistsInternalContact(txtFirstName.Text, txtLastName.Text, getOnlyNumber(txtTelephone1.Text), txtEmail.Text, 0);
        if (boolIsRecordExists == false)
        {
            boolPerResult = ExtContactBizServices.Update(personBE);
            selectedPersonID = personBE.PERSON_ID;
            if (boolPerResult)
            {
                //ExtTransactionWrapper.SubmitTransactionChanges();
                boolPostResult = PostAddressBizService.Update(postBE, personBE.PERSON_ID);
                selectedPostAddID = postBE.POSTALADDRESSID;
            }
            if ((ddlContactType.SelectedItem.Text == ExternalContactType.INSURED) & (boolPerResult))
            {
                // for New Record -----or In Edit Existing Record was not Contact Type:Insured earlier                
                CustomerContactBE custContBE = new CustomerContactBE();
                custContBE.CREATE_DATE = DateTime.Now;
                custContBE.CREATE_USER_ID = CurrentAISUser.PersonID;

                custContBE.PERSON_ID = personBE.PERSON_ID;
                custContBE.CUSTOMER_ID = Convert.ToInt32(ddlContactNames.SelectedValue);
                custContBE.ROLE_ID = null;
                boolCustCtAddResult = CustContBizServices.Update(custContBE);
                ViewState["CustCtID"] = custContBE.CUSTOMER_CONTACT_ID;
            }
            //    if (boolPerResult && boolPostResult && boolCustCtDeleteResult && boolCustCtEditResult && boolCustCtAddResult)
            //        ExtTransactionWrapper.SubmitTransactionChanges();
            //    else
            //        ExtTransactionWrapper.RollbackChanges();

            isNew = false;
            isCopy = false;
            BindExtContactListView();
            DisplayDetails(selectedPersonID);

            btnSave.Enabled = true;
            btnCopy.Enabled = true;
            btnCancel.Enabled = true;

            //to show the Listview row selected
            if (lstExtContact.Items.Count > 0)
            {
                Label lblPersonID;
                HtmlTableRow tr;
                int selectedindex = -1;
                for (int i = 0; i < lstExtContact.Items.Count; i++)
                {
                    lblPersonID = (Label)lstExtContact.Items[i].FindControl("lblPersonID");
                    if (Convert.ToInt32(lblPersonID.Text) == selectedPersonID)
                    {
                        selectedindex = i;
                        break;
                    }
                }
                if (selectedindex >= 0)
                {
                    tr = (HtmlTableRow)lstExtContact.Items[selectedindex].FindControl("trItemTemplate");
                    tr.Attributes["class"] = "SelectedItemTemplate";
                    ViewState["SelectedIndex"] = selectedindex;
                }
            }
        }
        else
        {
            ShowMessage("The record cannot be saved. An identical record already exists.");
        }
    }

    protected void UpdatEntity()
    {
        bool boolPerResult = true;
        bool boolPostResult = true;
        bool boolCustCtDeleteResult = true;
        bool boolCustCtEditResult = true;
        bool boolCustCtAddResult = true;

        personBE.UPDATEDDATE = DateTime.Now;
        personBE.UPDATEDUSER_ID = CurrentAISUser.PersonID;
        if (postBE == null)
        {
            postBE = new PostalAddressBE();
            postBE.CREATEDDATE = DateTime.Now;
            postBE.CREATEDUSER = CurrentAISUser.PersonID;
        }
        else
        {
            postBE.UPDATEDDATE = DateTime.Now;
            postBE.UPDATEDUSER = CurrentAISUser.PersonID;
        }

        //Edit ...Existing record is INSURED Type. currently changed. Deleteing entry in CustemerPersonRelation table
        if ((ViewState["CustCtID"] != null) && (ddlContactType.SelectedItem.Text != ExternalContactType.INSURED))
        {
            CustomerContactBE custContBE = new CustomerContactBE();
            custContBE = CustContBizServices.getCustmerContactData(Convert.ToInt32(ViewState["CustCtID"]));
            boolCustCtDeleteResult = CustContBizServices.Delete(custContBE);
        }

        personBE.CONCTACT_TYPE_ID = Convert.ToInt32(ddlContactType.SelectedValue);
        if (Convert.ToInt32(ddlContactNames.SelectedValue) != 0)
            personBE.EXTERNAL_ORGN_ID = Convert.ToInt32(ddlContactNames.SelectedValue);
        else
            personBE.EXTERNAL_ORGN_ID = null;
        // IF Selected Contact Type is INSURED then External OrganizationID must set to NULL
        if (ddlContactType.SelectedItem.Text == ExternalContactType.INSURED)
            personBE.EXTERNAL_ORGN_ID = null;
        if (Convert.ToInt32(ddlTitle.SelectedValue) != 0)
            personBE.TITLE_ID = Convert.ToInt32(ddlTitle.SelectedValue);
        else
            personBE.TITLE_ID = null;
        personBE.FORENAME = (txtFirstName.Text == string.Empty ? null : txtFirstName.Text);
        personBE.SURNAME = (txtLastName.Text == string.Empty ? null : txtLastName.Text);
        //personBE.TELEPHONE1 = (getOnlyNumber(txtTelephone1.Text) == string.Empty ? null : getOnlyNumber(txtTelephone1.Text));
        personBE.TELEPHONE1 = getOnlyNumber(txtTelephone1.Text);
        personBE.TELEPHONE2 = (getOnlyNumber(txtTelephone2.Text) == string.Empty ? null : getOnlyNumber(txtTelephone2.Text));
        personBE.FAX = (getOnlyNumber(txtFax.Text) == string.Empty ? null : getOnlyNumber(txtFax.Text));
        //personBE.EMAIL = (txtEmail.Text == string.Empty ? null : txtEmail.Text);
        personBE.EMAIL = txtEmail.Text;
        personBE.ACTIVE = true;

        postBE.ADDRESS1 = (txtAddress1.Text == string.Empty ? null : txtAddress1.Text);
        postBE.ADDRESS2 = (txtAddress2.Text == string.Empty ? null : txtAddress2.Text);
        postBE.CITY = (txtCity.Text == string.Empty ? null : txtCity.Text);
        postBE.STATE_ID = Convert.ToInt32(ddlState.SelectedValue);
        postBE.ZIP_CODE = (txtZipCode.Text.Replace("-", string.Empty)).Replace("_", string.Empty);
        //Checking if already a record existing // Using InternalContact method as it is same for both
        bool boolIsRecordExists = false;
        //bool boolIsRecordExists = ExtContactBizServices.IsExistsExtContact(txtFirstName.Text, txtLastName.Text, personBE.PERSON_ID, Convert.ToInt32(ddlContactType.SelectedValue), Convert.ToInt32(ddlContactNames.SelectedValue));
        //bool boolIsRecordExists = ExtContactBizServices.IsExistsInternalContact(txtFirstName.Text, txtLastName.Text, getOnlyNumber(txtTelephone1.Text), txtEmail.Text, personBE.PERSON_ID);        
        if (boolIsRecordExists == false)
        {
            boolPerResult = ExtContactBizServices.Update(personBE);
            ShowConcurrentConflict(boolPerResult, personBE.ErrorMessage);
            selectedPersonID = personBE.PERSON_ID;
            if (boolPerResult)
            {
                //ExtTransactionWrapper.SubmitTransactionChanges();
                boolPostResult = PostAddressBizService.Update(postBE, personBE.PERSON_ID);
                selectedPostAddID = postBE.POSTALADDRESSID;
            }
            if ((ddlContactType.SelectedItem.Text == ExternalContactType.INSURED) & (boolPerResult))
            {
                // for New Record -----or In Edit Existing Record was not Contact Type:Insured earlier
                if (ViewState["CustCtID"] == null)
                {
                    CustomerContactBE custContBE = new CustomerContactBE();
                    custContBE.CREATE_DATE = DateTime.Now;
                    custContBE.CREATE_USER_ID = CurrentAISUser.PersonID;

                    custContBE.PERSON_ID = personBE.PERSON_ID;
                    custContBE.CUSTOMER_ID = Convert.ToInt32(ddlContactNames.SelectedValue);
                    custContBE.ROLE_ID = null;
                    boolCustCtAddResult = CustContBizServices.Update(custContBE);
                    ViewState["CustCtID"] = custContBE.CUSTOMER_CONTACT_ID;
                }
                //Not a New Record(Editing) and Eearlier record is also Insured Type.. 
                //and Customer Name is changed compare with existing customerID
                else if (ViewState["CustCtID"] != null)
                {
                    CustomerContactBE custContBE = new CustomerContactBE();
                    custContBE = CustContBizServices.getCustmerContactData(Convert.ToInt32(ViewState["CustCtID"]));
                    custContBE.UPDATE_DATE = DateTime.Now;
                    custContBE.UPDATE_USER_ID = CurrentAISUser.PersonID;

                    custContBE.PERSON_ID = personBE.PERSON_ID;
                    custContBE.CUSTOMER_ID = Convert.ToInt32(ddlContactNames.SelectedValue);
                    boolCustCtEditResult = CustContBizServices.Update(custContBE);
                    ShowConcurrentConflict(boolCustCtEditResult, custContBE.ErrorMessage);
                    ViewState["CustCtID"] = custContBE.CUSTOMER_CONTACT_ID;
                }


            }
            //if (boolPerResult && boolPostResult && boolCustCtDeleteResult && boolCustCtEditResult && boolCustCtAddResult)
            //    ExtTransactionWrapper.SubmitTransactionChanges();
            //else
            //    ExtTransactionWrapper.RollbackChanges();
            isNew = false;
            isCopy = false;
            BindExtContactListView();
            DisplayDetails(selectedPersonID);

            btnSave.Enabled = true;
            btnCopy.Enabled = true;
            btnCancel.Enabled = true;

            //to show the Listview row selected
            if (lstExtContact.Items.Count > 0)
            {
                Label lblPersonID;
                HtmlTableRow tr;
                int selectedindex = 0;
                for (int i = 0; i < lstExtContact.Items.Count; i++)
                {
                    lblPersonID = (Label)lstExtContact.Items[i].FindControl("lblPersonID");
                    if (Convert.ToInt32(lblPersonID.Text) == selectedPersonID)
                    {
                        selectedindex = i;
                        break;
                    }
                }
                tr = (HtmlTableRow)lstExtContact.Items[selectedindex].FindControl("trItemTemplate");
                tr.Attributes["class"] = "SelectedItemTemplate";
                ViewState["SelectedIndex"] = selectedindex;
            }
        }
        else
        {
            ShowMessage("The record cannot be saved. An identical record already exists.");
        }
    }
    /// <summary>
    /// Usered for Telephone Numbers and Fax. will remove _,-,(.) from the given string
    /// </summary>
    /// <param name="strNumber">string to remove extra chars</param>
    /// <returns>returns string after removing _,-,(.)  </returns>
    protected string getOnlyNumber(string strNumber)
    {
        return (((strNumber.Replace("-", string.Empty)).Replace("_", string.Empty)).Replace("(", string.Empty)).Replace(")", string.Empty);
    }
    /// <summary>
    /// Copy Button clicks, CheckNew flag will changed to True. as if it is a new record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCopy_Click(object sender, EventArgs e)
    {
        isNew = true;
        isCopy = true;
        btnSave.Text = "Save";
        btnCopy.Enabled = false;
    }
    /// <summary>
    /// While rendering data to Listview , attaching Javascript to show mouseover,mouseout to Identiry which row is selected
    /// Finding Disable Image button, giving the attributes if it is Enable or Disable
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void lstExtContact_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {

            HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trItemTemplate");
            tr.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            tr.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            //  tr.Attributes["onclick"] = "javascript:SetMouseClickColor(this);";
            //ClientScript.GetPostBackClientHyperlink
            //    (this.lstInternalMasters, "Select$" + e.Item.ite);

            ImageButton imgExtEnableDisable = (ImageButton)e.Item.FindControl("imgExtEnableDisable");

            if (imgExtEnableDisable != null)
            {
                Label lblExtActive = (Label)e.Item.FindControl("lblExtActive");
                if (lblExtActive.Text == "True")
                {
                    imgExtEnableDisable.ImageUrl = "~/images/disabled.GIF";
                    imgExtEnableDisable.CommandName = "DISABLE";
                    imgExtEnableDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable this record?');");
                    imgExtEnableDisable.ToolTip = "Click here to Disable this External Contact";
                }
                else
                {

                    imgExtEnableDisable.ImageUrl = "~/images/enabled.GIF";
                    imgExtEnableDisable.CommandName = "ENABLE";
                    imgExtEnableDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable this record?');");
                    imgExtEnableDisable.ToolTip = "Click here to Enable this External Contact";
                }
            }
        }
    }
    /// <summary>
    /// attaching Javascript to show mouseover,mouseout to Identiry which row is selected
    /// Calls DisplayDetails method to fill the detail panel with listview selected values
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstExtContact_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
    {
        HtmlTableRow tr;
        btnSearch.Enabled = false;
        if (ViewState["SelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["SelectedIndex"]);
            tr = (HtmlTableRow)lstExtContact.Items[index].FindControl("trItemTemplate");
            if ((index % 2) == 0)
                tr.Attributes["class"] = "ItemTemplate";
            else
                tr.Attributes["class"] = "AlternatingItemTemplate";
        }
        tr = (HtmlTableRow)lstExtContact.Items[e.NewSelectedIndex].FindControl("trItemTemplate");
        tr.Attributes["class"] = "SelectedItemTemplate";
        ViewState["SelectedIndex"] = e.NewSelectedIndex;
        isNew = false;
        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
        {
            btnSave.Enabled = true;
            btnCopy.Enabled = true;
        }
        ListViewItem item = (ListViewItem)lstExtContact.Items[e.NewSelectedIndex];
        Label lblPersonID = (Label)item.FindControl("lblPersonID");
        int intPersonID = int.Parse(lblPersonID.Text);
        Label lblPostAddID = (Label)item.FindControl("lblPostAddID");
        if (lblPostAddID.Text.Trim() == "")
        {
            selectedPostAddID = 0;
        }
        else
        { selectedPostAddID = int.Parse(lblPostAddID.Text); }

        selectedPersonID = intPersonID;
        DisplayDetails(intPersonID);
        //btnAdd.Enabled = false;

        lstExtContact.Enabled = false;
        pnlDetails.Visible = true;
        lblExternalContactsDetails.Visible = true;
        lbCloseDetails.Visible = true;        
        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
        {
            btnCopy.Enabled = true;
            btnCancel.Enabled = true;
        }

    }
    /// <summary>
    /// Enables/Disables the row if use selects the row ENABLE or DISABLE
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstExtContact_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if ((e.CommandName.ToUpper() == "DISABLE") || (e.CommandName.ToUpper() == "ENABLE"))
            {
                PersonBE perBE;
                int personID = Convert.ToInt32(e.CommandArgument);
                perBE = ExtContactBizServices.getPersonRow(personID);
                if (e.CommandName.ToUpper() == "DISABLE")
                    perBE.ACTIVE = false;
                else
                    perBE.ACTIVE = true;

                perBE.UPDATEDDATE = DateTime.Now;
                perBE.UPDATEDUSER_ID = CurrentAISUser.PersonID;

                ExtContactBizServices.Update(perBE);
                //ExtTransactionWrapper.SubmitTransactionChanges();
                BindExtContactListView();
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }

    }
    /// <summary>
    /// Add button Click enables user to add new ro
    /// Clears all controls and makes the below panel visible
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (ViewState["SelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["SelectedIndex"]);
            HtmlTableRow tr = (HtmlTableRow)lstExtContact.Items[index].FindControl("trItemTemplate");
            tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
        }
        ViewState["SelectedIndex"] = null;
        if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
        {
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
        }
        btnSave.Text = "Save";
        isNew = true;
        isCopy = false;
        ClearFileds();

        lstExtContact.Enabled = false;
        btnAdd.Enabled = false;
        btnSearch.Enabled = false;
        btnCopy.Enabled = false;


        pnlDetails.Visible = true;
        lblExternalContactsDetails.Visible = true;
        lbCloseDetails.Visible = true;


    }

    /// <summary>
    /// Add button Click enables user to add new ro
    /// Clears all controls and makes the below panel visible
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["SelectedIndex"] = null;
        BindExtContactListView();

    }
    /// <summary>
    /// Clears all fields when user want to add new row
    /// </summary>
    public void ClearFileds()
    {
        txtFirstName.Text = string.Empty;
        txtLastName.Text = string.Empty;
        txtAddress1.Text = string.Empty;
        txtAddress2.Text = string.Empty;
        txtCity.Text = string.Empty;
        txtZipCode.Text = string.Empty;
        txtTelephone1.Text = string.Empty;
        txtTelephone2.Text = string.Empty;
        txtFax.Text = string.Empty;
        txtEmail.Text = string.Empty;
        pnlDetails.Enabled = true;

        ddlState.DataBind();
        ddlState.Items.FindByValue("0").Selected = true;

        ddlTitle.DataBind();
        ddlTitle.Items.FindByValue("0").Selected = true;

        ddlContactType.DataBind();
        ddlContactType.Items.FindByValue("0").Selected = true;

        ddlContactNames.Items.Clear();
        ListItem LI = new ListItem("(Select)", "0");
        ddlContactNames.Items.Add(LI);

    }

    protected void lstExtContact_SelectedIndexChanged(object sender, EventArgs e)
    {
        lstExtContact.SelectedIndex = -1;

    }
    /// <summary>
    /// Fills all controls with the yours selected External contact Details from List view
    /// </summary>
    /// <param name="PersonID"></param>
    public void DisplayDetails(int personID)
    {
        try
        {
            //PostAddressDA PostDA = new PostAddressDA();
            personBE = ExtContactBizServices.getPersonRow(personID); //Storing in session
            if (selectedPostAddID == 0)
            {
                postBE = null;
                //txtFirstName.Text = "";
                //txtLastName.Text = "";
                txtAddress1.Text = "";
                txtAddress2.Text = "";
                txtCity.Text = "";
                txtZipCode.Text = "";
            }
            else
            {
                postBE = PostAddressBizService.getPostAddrRow(Convert.ToInt32(selectedPostAddID));  //Storing in session                      
                txtAddress1.Text = (postBE.ADDRESS1 == null ? string.Empty : postBE.ADDRESS1);
                if (txtAddress1.Text != string.Empty) ViewState["PostID"] = postBE.POSTALADDRESSID;
                txtAddress2.Text = (postBE.ADDRESS2 == null ? string.Empty : postBE.ADDRESS2);
                txtCity.Text = (postBE.CITY == null ? string.Empty : postBE.CITY);
                txtZipCode.Text = (postBE.ZIP_CODE == null ? string.Empty : postBE.ZIP_CODE);
            }
            txtFirstName.Text = (personBE.FORENAME == null ? string.Empty : personBE.FORENAME);
            txtLastName.Text = (personBE.SURNAME == null ? string.Empty : personBE.SURNAME);
            txtTelephone1.Text = (personBE.TELEPHONE1 == null ? string.Empty : " " + personBE.TELEPHONE1);
            txtTelephone2.Text = (personBE.TELEPHONE2 == null ? string.Empty : " " + personBE.TELEPHONE2);
            txtFax.Text = (personBE.FAX == null ? string.Empty : " " + personBE.FAX);
            txtEmail.Text = (personBE.EMAIL == null ? string.Empty : personBE.EMAIL);


            if (personBE.CONCTACT_TYPE_ID != null)
            {
                ddlContactType.DataBind();
//                ddlContactType.Items.FindByValue(personBE.CONCTACT_TYPE_ID.ToString()).Selected = true;
                AddInActiveLookupData(ref ddlContactType, personBE.CONCTACT_TYPE_ID.Value);
            }
            BindDDLContactNames(int.Parse(ddlContactType.SelectedValue), ddlContactType.SelectedItem.Text.ToString());

            if (ddlContactType.SelectedItem.Text == ExternalContactType.INSURED)
            {
                CustomerContactBE custCtBE = new CustomerContactBE();
                custCtBE = CustContBizServices.getCustmerContact(personID);
                ViewState["CustCtID"] = custCtBE.CUSTOMER_CONTACT_ID;

                ddlContactNames.DataBind();
                ddlContactNames.Items.FindByValue(custCtBE.CUSTOMER_ID.ToString()).Selected = true;
            }
            else
            {
                ViewState["CustCtID"] = null;
                if (personBE.EXTERNAL_ORGN_ID != null)
                {
                    ddlContactNames.DataBind();
                    ddlContactNames.Items.FindByValue(personBE.EXTERNAL_ORGN_ID.ToString()).Selected = true;
                }
            }
            if (personBE.TITLE_ID != null)
            {
                ddlTitle.DataBind();
//                ddlTitle.Items.FindByValue(personBE.TITLE_ID.ToString()).Selected = true;
                AddInActiveLookupData(ref ddlTitle, personBE.TITLE_ID.Value);
            }

            ddlState.DataBind();
            if (selectedPostAddID != 0)
            {
//                ddlState.Items.FindByValue(postBE.STATE_ID.ToString()).Selected = true;
                AddInActiveLookupData(ref ddlState, postBE.STATE_ID);
            }

            pnlDetails.Enabled = Convert.ToBoolean(personBE.ACTIVE);
            btnSave.Text = "Update";
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    /// <summary>
    /// Invokes when Contact Type dropdown is changed. Contact Names dropdown will be popped up accordingly
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlContactType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (int.Parse(ddlContactType.SelectedValue) != 0)
                BindDDLContactNames(int.Parse(ddlContactType.SelectedValue), ddlContactType.SelectedItem.Text.ToString());
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }

    }

    /// <summary>
    /// Helps to fill the Contact Names Dropdown dynamically according to Contact type Dropdown Selection
    /// </summary>
    /// <param name="ContactTypeID"></param>
    /// <param name="ContactType"></param>
    private void BindDDLContactNames(int contactTypeID, string contactType)
    {
        PersonBS person = new PersonBS();
        try
        {
            IList<LookupBE> ExtOrg = new List<LookupBE>();
            ExtOrg = person.getNamesList(contactTypeID, contactType);

            ddlContactNames.DataSource = ExtOrg;
            ddlContactNames.DataValueField = "LookUpID";
            ddlContactNames.DataTextField = "LookUpName";
            ddlContactNames.DataBind();

            ddlContactNames.Items.FindByValue("0").Selected = true;
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    //private string SortDir
    //{
    //    get { return (string)Session["SORTDIR"]; }
    //    set { Session["SORTDIR"] = value; }
    //}
    /// <summary>
    /// Retrives records from database and fills Listview
    /// </summary>
    private void BindExtContactListView()
    {
        try
        {
            if ((Convert.ToInt32(ddlExtContacts.SelectedValue) != 0) && (ddlContactNameSearch.SelectedItem.Text != ""))
            {
                int contactType = Convert.ToInt32(ddlExtContacts.SelectedValue);
                PersonBS person = new PersonBS();
                IList<PersonBE> ExtContList = new List<PersonBE>();
                ExtContList = ExtContactBizServices.getExtContactList(contactType, ddlContactNameSearch.SelectedItem.Text);
                //if (SortDir == "ASC")
                //    lstExtContact.DataSource = ExtContList.OrderBy(o => o.EXTERNAL_ORGN_TXT);
                //else if (SortDir == "DESC")
                //    lstExtContact.DataSource = ExtContList.OrderByDescending(o => o.EXTERNAL_ORGN_TXT);
                //else
                    lstExtContact.DataSource = ExtContList;

                lstExtContact.DataBind();
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }


    }
    //protected void lstExtContact_Sorting(object sender, ListViewSortEventArgs e)
    //{
    //    // Check which field is being sorted
    //    // to set the visibility of the image controls.
    //    Image imgSortByName = (Image)lstExtContact.FindControl("imgSortByName");
    //    Image img = new Image();
    //    if (imgSortByName.ToolTip == "Ascending")
    //    {
    //        imgSortByName.ToolTip = "Descending";
    //        imgSortByName.ImageUrl = "~/images/descending.gif";
    //        imgSortByName.Visible = true;
    //        img = imgSortByName;
    //        SortDir = "DESC";
    //    }
    //    else
    //    {
    //        imgSortByName.ToolTip = "Ascending";
    //        imgSortByName.ImageUrl = "~/images/ascending.gif";
    //        imgSortByName.Visible = true;
    //        img = imgSortByName;
    //        SortDir = "ASC";
    //    }

    //    BindExtContactListView();
    //}
    #endregion

    #region ContactNamesTab

    private BrokerBS brokerBizService;
    private BrokerBS BrokerBizService
    {
        get
        {
            if (brokerBizService == null)
            {
                brokerBizService = new BrokerBS();
            }
            return brokerBizService;
        }
    }
    /// <summary>
    /// Enables ContactType dropdown to popup with the database value When user clicks on Edit
    /// Also enables to add Attributes to Enable /Disable LinkButton accordingly
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstBroker_DataBoundList(Object sender, ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            // Get a handle to the ddlAccountlist DropDownList control
            DropDownList ddlContactTypeList = (DropDownList)e.Item.FindControl("ddlContactTypelist");

            Label lblContactTypeID = (Label)e.Item.FindControl("lblContactTypeID");

            if ((ddlContactTypeList != null) & (lblContactTypeID != null))
            {
                //ddlContactTypeList.Items.Remove(ddlContactTypeList.Items.FindByText(ExternalContactType.INSURED));
                ddlContactTypeList.SelectedIndex = ddlContactTypeList.Items.IndexOf(ddlContactTypeList.Items.FindByValue(lblContactTypeID.Text.ToString()));
            }

            ImageButton imgEnableDisable = (ImageButton)e.Item.FindControl("imgEnableDisable");
            LinkButton lbBrokerEdit = (LinkButton)e.Item.FindControl("lbBrokerEdit");
            if (imgEnableDisable != null)
            {
                Label lblActive = (Label)e.Item.FindControl("lblActive");
                if (lblActive.Text == "True")
                {
                    if (WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
                        lbBrokerEdit.Enabled = true;// Enableling Edit Link Button if it is a Active record
                    imgEnableDisable.ImageUrl = "~/images/disabled.GIF";
                    imgEnableDisable.CommandName = "DISABLE";
                    imgEnableDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Disable this record?');");
                    imgEnableDisable.ToolTip = "Click here to Disable this Contact";
                }
                else
                {
                    lbBrokerEdit.Enabled = false;  // Disabeling Edit Link Button if it is not a Active record
                    imgEnableDisable.ImageUrl = "~/images/enabled.GIF";
                    imgEnableDisable.CommandName = "ENABLE";
                    imgEnableDisable.Attributes.Add("onclick", "return confirm('Are you sure you want to Enable this record?');");
                    imgEnableDisable.ToolTip = "Click here to Enable this Contact";
                }
            }
        }

    }

    /// <summary>
    /// Invoked when the Edit Link is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstBroker_ItemEdit(Object sender, ListViewEditEventArgs e)
    {
        lstBroker.EditIndex = e.NewEditIndex;
        lstBroker.InsertItemPosition = InsertItemPosition.None;
        BindBrokerListView();
    }


    /// <summary>
    /// Invoked when the Save Link is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void lstBroker_ItemInserting(Object sender, ListViewInsertEventArgs e)
    //{
    //    lstBroker.InsertItemPosition = InsertItemPosition.None;
    //}
    /// <summary>
    /// Invokes when user clicks on SAVE, UPDATE,ENABLE,DISABL
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstBroker_ItemCommand(Object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if ((e.CommandName.ToUpper() == "SAVE") || (e.CommandName.ToUpper() == "UPDATE"))
            {
                // Get the values  
                TextBox txtBrokerName = (TextBox)e.Item.FindControl("txtBrokerName");
                DropDownList ddlContactType = (DropDownList)e.Item.FindControl("ddlContactTypelist");                
                    if (e.CommandName.ToUpper() == "SAVE")  //if it is new
                    {
                        BrokerBE BrkBE = new BrokerBE();
                        BrkBE.CONTACT_TYPE_ID = int.Parse(ddlContactType.SelectedValue);
                        BrkBE.FULL_NAME = txtBrokerName.Text;
                        BrkBE.CREATE_USER_ID = CurrentAISUser.PersonID;
                        BrkBE.CREATE_DATE = DateTime.Now;
                        BrkBE.ACTV_IND = true;
                        //retunrs EXIST, NOTEXIST, DISABLED:Exist but disabled
                        bool Flag = BrokerBizService.IsContactNameExists(txtBrokerName.Text.ToString(), int.Parse(ddlContactType.SelectedValue), 0);
                        if (!Flag)
                            BrokerBizService.Update(BrkBE);                        
                        else                        
                            ShowMessage("The record cannot be saved. An identical record with " + "Contact Name: '" + txtBrokerName.Text.ToString() + "' Contact Type: '" + ddlContactType.SelectedItem.Text.ToString() + "' is already Existing");
                    }
                    else if (e.CommandName.ToUpper() == "UPDATE")  //if it is Update
                    {
                        Label lblBrokerID = (Label)e.Item.FindControl("lblBrokerID");
                        int extnlOrgID = int.Parse(lblBrokerID.Text);
                        BrokerBE brkBE;
                        brkBE = BrokerBizService.getBroker(extnlOrgID);
                        //Concurrency Issue
                        BrokerBE brkBEold = (lstBrokerList.Where(o => o.EXTRNL_ORG_ID.Equals(extnlOrgID))).First();
                        bool con = ShowConcurrentConflict(Convert.ToDateTime(brkBEold.UPDATE_DATE), Convert.ToDateTime(brkBE.UPDATE_DATE));
                        if (!con)
                            return;
                        //End
                        brkBE.FULL_NAME = txtBrokerName.Text;
                        brkBE.CONTACT_TYPE_ID = int.Parse(ddlContactType.SelectedValue);
                        brkBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
                        brkBE.UPDATE_DATE = DateTime.Now;
                        bool Flag = BrokerBizService.IsContactNameExists(txtBrokerName.Text.ToString(), int.Parse(ddlContactType.SelectedValue), brkBE.EXTRNL_ORG_ID);
                        if (!Flag)                        
                            BrokerBizService.Update(brkBE);                        
                        else
                            ShowMessage("The record cannot be saved. An identical record with " + "Contact Name: '" + txtBrokerName.Text.ToString() + "' Contact Type: '" + ddlContactType.SelectedItem.Text.ToString() + "' is already Existing");
                        
                        // get out of the edit mode
                        lstBroker.InsertItemPosition = InsertItemPosition.FirstItem;
                        lstBroker.EditIndex = -1;
                    }
                    BindBrokerListView(); // bind the listview                
            }
            if ((e.CommandName.ToUpper() == "DISABLE") || (e.CommandName.ToUpper() == "ENABLE"))
            {
                BrokerBE brkBE;
                int extnlOrgID = Convert.ToInt32(e.CommandArgument);
                brkBE = BrokerBizService.getBroker(extnlOrgID);
                //Concurrency Issue
                BrokerBE brkBEold = (lstBrokerList.Where(o => o.EXTRNL_ORG_ID.Equals(extnlOrgID))).First();
                bool con = ShowConcurrentConflict(Convert.ToDateTime(brkBEold.UPDATE_DATE), Convert.ToDateTime(brkBE.UPDATE_DATE));
                if (!con)
                    return;
                //End
                if (e.CommandName.ToUpper() == "DISABLE")
                    brkBE.ACTV_IND = false;
                else
                    brkBE.ACTV_IND = true;
                brkBE.UPDATE_USER_ID = CurrentAISUser.PersonID;
                brkBE.UPDATE_DATE = DateTime.Now;
                BrokerBizService.Update(brkBE);
                BindBrokerListView();
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }

    // Invoked when the Cancel Link is clicked
    protected void lstBroker_ItemCancel(Object sender, ListViewCancelEventArgs e)
    {
        if (e.CancelMode == ListViewCancelMode.CancelingEdit)
        {
            lstBroker.EditIndex = -1;
            lstBroker.InsertItemPosition = InsertItemPosition.FirstItem;
            BindBrokerListView();
        }
        else if (e.CancelMode == ListViewCancelMode.CancelingInsert)
        {
            CancelUpdateMode(); //Back to normal mode.
        }
    }
    protected void lstBroker_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    { }
    protected void CancelUpdateMode()
    {
        //lstBroker.EditIndex = -1;
        //lstBroker.InsertItemPosition = InsertItemPosition.FirstItem;
        lstBroker.InsertItemPosition = InsertItemPosition.None;
        BindBrokerListView();
    }
    private void BindBrokerListView()
    {
        try
        {
            lstBrokerList = BrokerBizService.GetBrokerData();
            this.lstBroker.DataSource = lstBrokerList;
            lstBroker.DataBind();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message,ex);
        }
    }


    private IList<BrokerBE> lstBrokerList
    {
        get
        {
            if (Session["lstBrokerList"] == null)
                Session["lstBrokerList"] = new List<BrokerBE>();
            return (IList<BrokerBE>)Session["lstBrokerList"];
        }
        set { Session["lstBrokerList"] = value; }
    }


    #endregion





}