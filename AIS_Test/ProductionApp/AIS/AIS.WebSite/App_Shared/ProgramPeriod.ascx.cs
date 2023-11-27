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
using ZurichNA.AIS.Business.Logic;
using System.Collections.Generic;

public partial class App_Shared_ProgramPeriod : System.Web.UI.UserControl
{
    // User can create an event like the below and assign to this event variable
    // protected void lstProgramPeriod_ItemCommand(object sender, ListViewCommandEventArgs e)
    public delegate void ItemCommand(object sender, ListViewCommandEventArgs e);
    public delegate void SelectedIndexChanged(object sender, EventArgs e);
    public delegate void SelectedIndexChanging(object sender, ListViewSelectEventArgs e);
    public event ItemCommand OnItemCommand;
    public event SelectedIndexChanged OnSelectedIndexChanged;
    public event SelectedIndexChanging OnSelectedIndexChanging;

    private int programPeriodID = 0;

    /// <summary>
    /// Property to hold Program Period ID (Premium Adjustment Program ID)
    /// </summary>
    public int ProgramPeriodID
    {
        get { return programPeriodID; }
        set { programPeriodID = value; }
    }
    //public property to capture the SelectedProgramPeriod ID. THis will be used in Retroinfo,combinedelements,AssignERP and Auditinfo pages
    public int SelectedProgramID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindPolicydata();
            //Logic to highlighted  Select the ProgramPeriod Line for cinsistancy in Retroinfo,combinedelements,AssignERP and Auditinfo pages
            if (SelectedProgramID > 0)
            {
                int intSelectedrow = -1;
                LinkButton lbSelect;
                for (int i = 0; i < lstProgramPeriod.Items.Count; i++)
                {
                    lbSelect = (LinkButton)lstProgramPeriod.Items[i].FindControl("lbSelect");
                    if (SelectedProgramID.ToString() == lbSelect.CommandArgument)
                    {
                        intSelectedrow = i;
                        break;
                    }
                }
                if (intSelectedrow >= 0)
                {
                    HtmlTableRow tr = (HtmlTableRow)lstProgramPeriod.Items[intSelectedrow].FindControl("trPrgPeriod");
                    ViewState["SelectedIndex"] = intSelectedrow;
                    tr.Attributes["class"] = "SelectedItemTemplate";
                }

            }

        }
    }

    private void BindPolicydata()
    {

        IList<ProgramPeriodListBE> lstProgramPeriods = new List<ProgramPeriodListBE>();
        AISBasePage ret = new AISBasePage();
        ProgramPeriodsBS ppbe = new ProgramPeriodsBS();

        lstProgramPeriods = ppbe.GetProgramPeriodList(ret.AISMasterEntities.AccountNumber);
        this.lstProgramPeriod.DataSource = lstProgramPeriods;
        this.lstProgramPeriod.DataBind();
    }

    protected void lstProgramPeriod_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            if (OnItemCommand != null)
                OnItemCommand(this, e);
        }
    }

    protected void lstProgramPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (OnSelectedIndexChanged != null)
            OnSelectedIndexChanged(this, e);
    }

    protected void lstProgramPeriod_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
    {
        if (OnSelectedIndexChanging != null)
            OnSelectedIndexChanging(this, e);
        HtmlTableRow tr;
        if (ViewState["SelectedIndex"] != null)
        {
            int index = Convert.ToInt32(ViewState["SelectedIndex"]);
            tr = (HtmlTableRow)lstProgramPeriod.Items[index].FindControl("trPrgPeriod");
            tr.Attributes["class"] = (index % 2) == 0 ? "ItemTemplate" : "AlternatingItemTemplate";
        }

        tr = (HtmlTableRow)lstProgramPeriod.Items[e.NewSelectedIndex].FindControl("trPrgPeriod");
        ViewState["SelectedIndex"] = e.NewSelectedIndex;
        tr.Attributes["class"] = "SelectedItemTemplate";

    }

    protected void lstProgramPeriod_ItemDataBound(object sender, ListViewItemEventArgs e)
    {

        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("trPrgPeriod");
            tr.Attributes["onmouseover"] = "javascript:setMouseOverColor(this);";
            tr.Attributes["onmouseout"] = "javascript:setMouseOutColor(this);";
            Label lblStartDate = (Label)e.Item.FindControl("lblStartDate");
            Label lblEndDate = (Label)e.Item.FindControl("lblEndDate");
            Label lblValnmmDate = (Label)e.Item.FindControl("lblValnmmDate");
            lblStartDate.Text = Convert.ToDateTime(lblStartDate.Text).ToShortDateString();
            lblEndDate.Text = Convert.ToDateTime(lblEndDate.Text).ToShortDateString();
            lblValnmmDate.Text = Convert.ToDateTime(lblValnmmDate.Text).ToString("MM / dd");
        }
    }

}
