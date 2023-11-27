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

public partial class AppMgmt_InvoiceExhibit : AISBasePage
{
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.Page.Title = "Invoice Exhibits";
        if(!Page.IsPostBack)
        BindListView();
    }
    #endregion

    #region BindListView
    public void BindListView()
    {
        InvoiceExhibitBS invoiceExhibitBS = new InvoiceExhibitBS();
        //IList<InvoiceExhibitBE> invoiceExhibitBEList = new List<InvoiceExhibitBE>();
        invoiceExhibitBEList = invoiceExhibitBS.getInvoiceExhibitData();
        lsvInvoiceSetup.DataSource = invoiceExhibitBEList;
        lsvInvoiceSetup.DataBind();
    }
    #endregion
    // Invoked when the Edit Link is clicked
    // Set the Listview to Editmode
    #region Edit Listview
    protected void EditList(Object sender, ListViewEditEventArgs e)
    {
        ListView lstView = (ListView)sender;
        if (lstView.ID.ToString() == "lsvInvoiceSetup")
        {
            lsvInvoiceSetup.EditIndex = e.NewEditIndex;
            BindListView();

            HiddenField hdActInd = ((HiddenField)lsvInvoiceSetup.Items[e.NewEditIndex].FindControl("hidActInd"));
            string strActInd = hdActInd.Value.ToString();
            if (strActInd == "True") strActInd="Active";
            else { strActInd = "Inactive"; }
            ((DropDownList)lsvInvoiceSetup.Items[e.NewEditIndex].FindControl("ddlEditActiveInactive")).Items.FindByText(strActInd.Trim()).Selected = true;
            HiddenField hdIntFlag = ((HiddenField)lsvInvoiceSetup.Items[e.NewEditIndex].FindControl("hidIntFlag"));
            string strIntFlag = hdIntFlag.Value.ToString();
            if (strIntFlag == "I") strIntFlag = "Internal";
            else if (strIntFlag == "E") strIntFlag = "External";
            else { strIntFlag = "Both"; }
            ((DropDownList)lsvInvoiceSetup.Items[e.NewEditIndex].FindControl("ddlEditInternalExternal")).Items.FindByText(strIntFlag.Trim()).Selected = true;
            HiddenField hdCesar = ((HiddenField)lsvInvoiceSetup.Items[e.NewEditIndex].FindControl("hidCesar"));
            string strCesar = hdCesar.Value.ToString();
            if (strCesar == "True") strCesar = "Yes";
            else { strCesar = "No"; }
            ((DropDownList)lsvInvoiceSetup.Items[e.NewEditIndex].FindControl("ddlEditCesarCoding")).Items.FindByText(strCesar.Trim()).Selected = true;
         
        
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
            if (lstView.ID.ToString() == "lsvInvoiceSetup")
            {
                lsvInvoiceSetup.EditIndex = -1;
                BindListView();
            }
        }

    }
    #endregion


    #region Command List for Save
    protected void CommandList(Object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SAVE")
        {
            ListView lv = (ListView)sender;
            if (lv.ID.ToString() == "lsvInvoiceSetup")
            {
                SaveList(e.Item);
            }

        }
       

    }
    #endregion

    // Invoked when the Update Link is clicked
    #region Update Listview Data
    protected void UpdateList(Object sender, ListViewUpdateEventArgs e)
    {
        ListView lstView = (ListView)sender;
        if (lstView.ID.ToString() == "lsvInvoiceSetup")
        {
            ListViewItem myItem = lsvInvoiceSetup.Items[e.ItemIndex];
            int intExhibitID = int.Parse(((HiddenField)myItem.FindControl("hidID")).Value.ToString());
            string strAtchno = (((TextBox)myItem.FindControl("txtEditAttachmentNo")).Text);
            string strAtchname = (((TextBox)myItem.FindControl("txtEditAttachmentName")).Text);
            int intSeqnbr = Convert.ToInt32(((TextBox)myItem.FindControl("txtEditSequence")).Text);
            int intAct=Convert.ToInt32(((DropDownList)myItem.FindControl("ddlEditActiveInactive")).SelectedValue);
            bool blAct;
            if (intAct == 1)
                blAct = true;
            else
                blAct = false;
            string strIntFlag = (((DropDownList)myItem.FindControl("ddlEditInternalExternal")).SelectedValue).ToString();
           
            string strCesar = (((DropDownList)myItem.FindControl("ddlEditCesarCoding")).SelectedValue);
            bool blCesar;
            if (strCesar == "1")
                blCesar = true;
            else
                blCesar = false;
            
            InvoiceExhibitBS invoiceExhibitBS = new InvoiceExhibitBS();
            InvoiceExhibitBE invoiceExhibitBE = invoiceExhibitBS.getInvoiceExhibitRow(intExhibitID);
            //Concurrency Issue
            InvoiceExhibitBE invoiceExhibitBEold = (invoiceExhibitBEList.Where(o => o.INVC_EXHIBIT_SETUP_ID.Equals(intExhibitID))).First();
            bool con = ShowConcurrentConflict(Convert.ToDateTime(invoiceExhibitBEold.UPDATEDDATE), Convert.ToDateTime(invoiceExhibitBE.UPDATEDDATE));
            if (!con)
                return;
            //End
            invoiceExhibitBE.STS_IND = blAct;
            invoiceExhibitBE.INTRNL_FLAG_IND = char.Parse(strIntFlag); 
            invoiceExhibitBE.CESAR_CD_IND = blCesar;
            invoiceExhibitBE.ATCH_CD = strAtchno;
            invoiceExhibitBE.ATCH_NM = strAtchname;
            invoiceExhibitBE.SEQ_NBR = intSeqnbr;
            invoiceExhibitBE.UPDATEDDATE = DateTime.Now;
            invoiceExhibitBE.UPDATEDUSER = CurrentAISUser.PersonID;
            bool flag=invoiceExhibitBS.Update(invoiceExhibitBE);
            ShowConcurrentConflict(flag, invoiceExhibitBE.ErrorMessage);
            lsvInvoiceSetup.EditIndex = -1;
            BindListView();
 
            (new Common(this.GetType())).Logger.Info("User -  " + CurrentAISUser.FullName + "[AZCORP:"
              + CurrentAISUser.UserID + ", Role: " + CurrentAISUser.Role + "] tried to update InvoiceExhibit Info the AIS application");
           

        }

    }
    #endregion

    // Invoked when the Save Link is clicked
    #region Save Listview Data
    protected void SaveList(ListViewItem e)
    {

       
        InvoiceExhibitBS invoiceExhibitBS = new InvoiceExhibitBS();
        InvoiceExhibitBE invoiceExhibitBE = new InvoiceExhibitBE();
       
        string strAct = (((DropDownList)e.FindControl("ddlActiveInactive")).SelectedValue);
        bool blAct;
        if (strAct == "1")
            blAct = true;
        else
            blAct = false;
        invoiceExhibitBE.STS_IND = blAct;
        string strIntFlag = (((DropDownList)e.FindControl("ddlInternalExternal")).SelectedValue).ToString();
        
        invoiceExhibitBE.INTRNL_FLAG_IND =char.Parse(strIntFlag); 
        string strCesar = (((DropDownList)e.FindControl("ddlCesarCoding")).SelectedValue);
        bool blCesar;
        if (strCesar == "1")
            blCesar = true;
        else
            blCesar = false;
        invoiceExhibitBE.CESAR_CD_IND = blCesar;
        invoiceExhibitBE.ATCH_CD = (((TextBox)e.FindControl("txtAttachmentNo")).Text);
        invoiceExhibitBE.ATCH_NM = (((TextBox)e.FindControl("txtAttachmentName")).Text);
        invoiceExhibitBE.SEQ_NBR = Convert.ToInt32(((TextBox)e.FindControl("txtSequence")).Text);
        invoiceExhibitBE.CREATEDATE = DateTime.Now;
        invoiceExhibitBE.CREATEUSER = CurrentAISUser.PersonID;
        invoiceExhibitBS.Update(invoiceExhibitBE);
        BindListView();
      
    }
    #endregion

    #region Maintaining Session for InvoiceExhibitBE
    private IList<InvoiceExhibitBE> invoiceExhibitBEList
    {
        get { return (IList<InvoiceExhibitBE>)Session["InvoiceExhibitBE"]; }
        set { Session["InvoiceExhibitBE"] = value; }
    }
    #endregion

    #region SortBy Property
    private string SortBy
    {
        get { return (string)Session["SORTBY"]; }
        set { Session["SORTBY"] = value; }
    }
    #endregion
  

    #region SortDir Property
    private string SortDir
    {
        get { return (string)Session["SORTDIR"]; }
        set { Session["SORTDIR"] = value; }
    }
    #endregion

    #region Listview Sorting
    protected void lsvInvoiceSetup_Sorting(Object sender, ListViewSortEventArgs e)
    {
        Image imgSortBySequence = (Image)lsvInvoiceSetup.FindControl("imgSortBySequence");
        Image img = new Image();
        switch (e.SortExpression)
        {
            case "SEQ_NBR":
                SortBy = "SEQ_NBR";
                imgSortBySequence.Visible = true;
                img = imgSortBySequence;
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
        BindInformation();
    }


    private void BindInformation()
    {
        lsvInvoiceSetup.DataSource = GetSortedData();
        lsvInvoiceSetup.DataBind();
    }

    private IList<InvoiceExhibitBE> GetSortedData()
    {

        switch (SortBy)
        {
            case "SEQ_NBR":
                if (SortDir == "ASC")
                    invoiceExhibitBEList = (invoiceExhibitBEList.OrderBy(o => o.SEQ_NBR)).ToList();
                else if (SortDir == "DESC")
                    invoiceExhibitBEList = (invoiceExhibitBEList.OrderByDescending(o => o.SEQ_NBR)).ToList();

                break;
        }
        return invoiceExhibitBEList;


    }
    #endregion


}
