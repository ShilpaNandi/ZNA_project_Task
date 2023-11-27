using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
namespace ZurichNA.AIS.WebSite.App_Code
{
/// <summary>
/// AISListView Control for AIS ASP.NET application
/// </summary>
public partial class AISListView : ListView
{
    LinkButton lbtn;
    AISBasePage aispage = new AISBasePage();
    /// <summary>
    /// Override function for ListView Control to disable the Edit operation in ListView part of Security Roles
    /// </summary>
    /// <param name="e"></param>
    protected override void OnItemCommand(ListViewCommandEventArgs e) 
    {
        HiddenField hdnControlDirty = (HiddenField)Page.Master.FindControl("hdnControlDirty");
        if (e.CommandName.Trim().ToUpper() == "EDIT")
        {
            hdnControlDirty.Value = "1";
        }
        else if (e.CommandName.Trim().ToUpper() == "UPDATE" || e.CommandName.Trim().ToUpper() == "SAVE"
            || e.CommandName.Trim().ToUpper() == "CANCEL")
        {
            hdnControlDirty.Value = "0";
        }

        base.OnItemCommand(e);
    }

    /// <summary>
    /// Override function for ListView Control to set Dirty for the Insert Item
    /// </summary>
    /// <param name="e"></param>
    protected override void OnItemCreated(ListViewItemEventArgs e)
    {
        
        if (e.Item.ItemType == ListViewItemType.InsertItem || e.Item.ItemType == ListViewItemType.DataItem)
        {
            if (aispage.WEBPAGEROLE != GlobalConstants.ApplicationSecurityGroup.Inquiry)
            {

                //if (this.Parent.TemplateControl.AppRelativeVirtualPath == "~/Loss/LossFundInfo.aspx" && (aispage.ROLENAME != GlobalConstants.ApplicationSecurityGroup.Manager) && (aispage.ROLENAME != GlobalConstants.ApplicationSecurityGroup.SystemAdmin))
                //{
                //    FindControls(e.Item.Controls);
                //}
                //else 
                if (this.Parent.TemplateControl.AppRelativeVirtualPath != "~/AdjCalc/InvoicingDashboard.aspx")
                {
                    SetControls(e.Item.Controls);
                }
            }
            if (aispage.WEBPAGEROLE == GlobalConstants.ApplicationSecurityGroup.Inquiry)
            {
                FindControls(e.Item.Controls);
            }
            
        }

        base.OnItemCreated(e);
    }

    /// <summary>
    /// Override function for ListView Control to disable the Edit operation in ListView part of Security Roles
    /// </summary>
    /// <param name="e"></param>
    protected override void OnItemDataBound(ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            //calling member function to disable the Data manipulation controls in the ListView DataItem
            if (aispage.WEBPAGEROLE == GlobalConstants.ApplicationSecurityGroup.Inquiry)
                FindControls(e.Item.Controls);
        }

        base.OnItemDataBound(e);
    }

    /// <summary>
    /// Member Function to Check the Controls collection and Disable the Data Manipulation linkbuttons and 
    /// Diable the Enable/Disable Image Buttons of ListView
    /// </summary>
    /// <param name="collection"></param>
    public void FindControls(ControlCollection collection)
    {
        foreach (Control cnt in collection)
        {
            if (cnt.HasControls())
            {
                this.FindControls(cnt.Controls);
            }
            if (cnt is LinkButton)
            {
                lbtn = (LinkButton)cnt;
                if (lbtn.Text.ToUpper() == "EDIT" || lbtn.Text.ToUpper() == "CANCEL" || lbtn.Text.ToUpper() == "VOID" || lbtn.Text.ToUpper() == "REVISE" || lbtn.Text.ToUpper() == "UPDATE" || lbtn.Text.ToUpper() == "SAVE" || lbtn.Text.ToUpper() == "ADD")
                    lbtn.Enabled = false;
            }
            //else if (cnt is CheckBox || cnt is Image || cnt is DropDownList || cnt is TextBox || cnt is RadioButton)
            //{
            //    ((WebControl)cnt).Enabled = false;
            //    ((WebControl)cnt).Style.Add(HtmlTextWriterStyle.Cursor, "Text");
            //}
            else if (cnt is Image)
            {
                    ((WebControl)cnt).Enabled = false;
                    ((WebControl)cnt).Style.Add(HtmlTextWriterStyle.Cursor, "Text");
            }
        }
    }

    /// <summary>
    /// Member Function to Check the Controls collection and set the dirty bit for controls
    /// </summary>
    /// <param name="collection"></param>
    public void SetControls(ControlCollection collection)
    {
        foreach (Control cnt in collection)
        {
            if (cnt.HasControls())
            {
                this.SetControls(cnt.Controls);
            }

            aispage.AssignDirtyBit(Page,cnt);
        }
    }

}

}
