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


public partial class App_Shared_AdjustmentReviewSearch : System.Web.UI.UserControl
{
    private IList<AccountBE> dtDDLSource;
    public event EventHandler AdjustmentReviewSearchButtonClicked;
    public event EventHandler ARProgramPeriodSelectedIndexChanged;
    public event EventHandler ARAdjustmentNumberSelectedIndexChanged;
    //public event EventHandler ARValDateSelectedIndexChanged;
    //public event EventHandler ARAccountListSelectedIndexChanged;
    #region Properties
    private string operation;
    public string Operation
    {
        get { return operation; }
        set { operation = value; }
    }
    #endregion
    public bool IsValid
    {
        get
        {
            return Page.IsValid;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
           
            AISBasePage objBasePage = new AISBasePage();
            int intAccNo=0;
            if (objBasePage.AISMasterEntities.InvoiceDashboardAccountNumber != 0)
                intAccNo = objBasePage.AISMasterEntities.InvoiceDashboardAccountNumber;
            else if (objBasePage.AISMasterEntities.AccountNumber != 0)
                intAccNo = objBasePage.AISMasterEntities.AccountNumber;

            this.dtDDLSource = (new AccountBS()).getAccountsInfo(intAccNo);
            this.ddlAccountlist.DataSource = dtDDLSource;
            this.ddlAccountlist.DataTextField = "FULL_NM";
            this.ddlAccountlist.DataValueField = "CUSTMR_ID";
            this.ddlAccountlist.DataBind();
            this.ddlAccountlist.Items.Insert(0, "(Select)");

            this.ddlValDate.Enabled = false;
            this.ddlAdjNumber.Enabled = false;
            this.ddlProgramPeriod.Enabled = false;
            
            
                if (Request.QueryString["SelectedValues"] != null && Request.QueryString["SelectedValues"] != "")
                {
                    string[] strSelectedValues = Request.QueryString["SelectedValues"].ToString().Split(';');
                    string strSelectedAccountID = strSelectedValues[0].ToString();
                    string strSelectedValDate = strSelectedValues[1].ToString();
                    string strSelectedPremAdjID = strSelectedValues[2].ToString();
                    string strSelectedProgramPeriod = strSelectedValues[3].ToString();
                    fillSelectedValues(strSelectedAccountID, strSelectedValDate, strSelectedPremAdjID, strSelectedProgramPeriod);

                }
        }

        string strResetCtlDirtyBit =
         "Javascript:document.getElementById('" + Page.Master.FindControl("hdnControlDirty").ClientID + "').value=0;";

        btnSearch.Attributes["onClick"] = strResetCtlDirtyBit;
    }
    public string AccountSelectedValue
    {
        get
        {
            return ddlAccountlist.SelectedValue;
        }

    }

    public string AccountSelectedItem
    {
        get
        {
            return Convert.ToString(ddlAccountlist.SelectedItem);
        }
    }
    public string ValDateSelectedValue
    {
        get
        {
            return ddlValDate.SelectedValue;
        }

    }

    public string ValDateSelectedItem
    {
        get
        {
            return Convert.ToString(ddlValDate.SelectedItem);
        }
    }
    public string AdjNumberSelectedValue
    {
        get
        {
            return ddlAdjNumber.SelectedValue;
        }

    }
    public string AdjNumberSelectedItem
    {
        get
        {
            return Convert.ToString(ddlAdjNumber.SelectedItem);
        }
    }
    public string ProgramPeriodSelectedValue
    {
        get
        {
            return ddlProgramPeriod.SelectedValue;
        }

    }

    public string ProgramPeriodSelectedItem
    {
        get
        {
            return Convert.ToString(ddlProgramPeriod.SelectedItem);
        }
    }
    protected void UCAdjustmentReviewSearch_Click(Object sender, CommandEventArgs e)
    {
        Operation = e.CommandName.ToString().Trim();
        OnOperationsButtonClicked(this, e);
    }

    protected void OnOperationsButtonClicked(Object sender, CommandEventArgs e)
    {
        if (AdjustmentReviewSearchButtonClicked != null)
            AdjustmentReviewSearchButtonClicked(this, e);
    }
    protected void ddlProgramPeriod_SelectedIndexChanged(Object sender, EventArgs e)
    {
        OnOperationsSelectedIndexChange(this, e);
    }
    protected void ddlAdjNumber_SelectedIndexChanged(Object sender, EventArgs e)
    {

        if (ddlAdjNumber.SelectedIndex != 0)
        {

            odsProgramPeriod.SelectParameters[0].DefaultValue = ddlAdjNumber.SelectedValue;
            odsProgramPeriod.SelectParameters[1].DefaultValue = ddlAccountlist.SelectedValue;
            ddlProgramPeriod.DataSourceID = "odsProgramPeriod";
            ddlProgramPeriod.DataTextField = "STARTDATE_ENDDATE_PGMTYP";
            ddlProgramPeriod.DataValueField = "PREM_ADJ_PGM_ID";
            ddlProgramPeriod.DataBind();
            changeDropDownsState(3, true);
            OnOperationsSelectedIndexChange(this, e);
            OnOperationsAdjustmentNumberSelectedIndexChange(this, e);
        }
        else
        {
            changeDropDownsState(3, false);
            OnOperationsSelectedIndexChange(this, e);
            OnOperationsAdjustmentNumberSelectedIndexChange(this, e);
        }
       
    }
    
    protected void OnOperationsSelectedIndexChange(Object sender, EventArgs e)
    {
        if (ARProgramPeriodSelectedIndexChanged != null)
            ARProgramPeriodSelectedIndexChanged(this, e);
    }
    ///-----------------------------------------------------------------------------------///
    ///Added By:-Venkata R Kolimi
    ///Purpouse:-As Part of Bu Broker Review Work Order
    ///Created Date:-09/08/2009
    ///Modified By:
    ///Modified Date:
    ///Files Used:BuBrokerReview.apsx.cs
    ///-----------------------------------------------------------------------------------///
    /// <summary>
    /// This event is added to meet the reuirements in BuBrokerReview scrren as it is having only 3 parameters
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
     protected void OnOperationsAdjustmentNumberSelectedIndexChange(Object sender, EventArgs e)
    {
        if (ARAdjustmentNumberSelectedIndexChanged != null)
            ARAdjustmentNumberSelectedIndexChanged(this, e);
    }
    
    protected void ddlAccountlist_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAccountlist.SelectedIndex != 0)
        {

            odsValuationDate.SelectParameters[0].DefaultValue = ddlAccountlist.SelectedValue;
            ddlValDate.DataSourceID = "odsValuationDate";
            ddlValDate.DataTextField = "VALUATIONDATE";
            ddlValDate.DataValueField = "VALUATIONDATE";
            ddlValDate.DataBind();
            changeDropDownsState(1, true);
            OnOperationsSelectedIndexChange(this, e);
            OnOperationsAdjustmentNumberSelectedIndexChange(this, e);
        }
        else
        {
            changeDropDownsState(1, false);
            OnOperationsSelectedIndexChange(this, e);
            OnOperationsAdjustmentNumberSelectedIndexChange(this, e);
        }
    }
    public void changeDropDownsState(int intOrder, bool Flag)
    {
        switch (intOrder)
        {
            case 1:
                this.ddlValDate.Enabled = Flag;
                this.ddlAdjNumber.Enabled = false;
                this.ddlProgramPeriod.Enabled = false;
                this.ddlValDate.SelectedIndex = 0;
                this.ddlAdjNumber.SelectedIndex = 0;
                this.ddlProgramPeriod.SelectedIndex = 0;
                break;
            case 2:
                this.ddlAdjNumber.Enabled = Flag;
                this.ddlProgramPeriod.Enabled = false;
                this.ddlAdjNumber.SelectedIndex = 0;
                this.ddlProgramPeriod.SelectedIndex = 0;
                break;
            case 3:
                this.ddlProgramPeriod.Enabled = Flag;
                this.ddlProgramPeriod.SelectedIndex = 0;
                break;
        }

    }

    protected void ddlValDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlValDate.SelectedIndex != 0)
        {

            odsAdjNumber.SelectParameters[0].DefaultValue = ddlAccountlist.SelectedValue;
            odsAdjNumber.SelectParameters[1].DefaultValue = ddlValDate.SelectedValue;
            ddlAdjNumber.DataSourceID = "odsAdjNumber";
            ddlAdjNumber.DataTextField = "PREM_ADJ_ID";
            ddlAdjNumber.DataValueField = "PREM_ADJ_ID";
            ddlAdjNumber.DataBind();
            changeDropDownsState(2, true);
            OnOperationsSelectedIndexChange(this, e);
            OnOperationsAdjustmentNumberSelectedIndexChange(this, e);
        }
        else
        {
            changeDropDownsState(2, false);
            OnOperationsSelectedIndexChange(this, e);
            OnOperationsAdjustmentNumberSelectedIndexChange(this, e);
        }
        

    }
   
    protected void ddlProgramPeriod_DataBound(object sender, EventArgs e)
    {
        ddlProgramPeriod.Items.Insert(0, "(Select)");
    }

    protected void ddlAdjNumber_DataBound(object sender, EventArgs e)
    {
        ddlAdjNumber.Items.Insert(0, "(Select)");
    }
    private void fillSelectedValues(string strSelectedAccountID, string strSelectedValDate,string strSelectedPremAdjID, string strSelectedProgramPeriod)
    {

        if (strSelectedAccountID != "")
        {
            ddlAccountlist.Items.FindByValue(strSelectedAccountID).Selected = true;
        }
        if (ddlAccountlist.SelectedIndex != 0)
        {
            ddlValDate.Enabled = true;
            odsValuationDate.SelectParameters[0].DefaultValue = ddlAccountlist.SelectedValue;
            ddlValDate.DataSourceID = "odsValuationDate";
            ddlValDate.DataTextField = "VALUATIONDATE";
            ddlValDate.DataValueField = "VALUATIONDATE";
            ddlValDate.DataBind();
            if (strSelectedValDate != "")
                ddlValDate.Items.FindByValue(strSelectedValDate).Selected = true;
        }

        if (ddlAccountlist.SelectedIndex != 0 && ddlValDate.SelectedIndex != 0)
        {
            ddlAdjNumber.Enabled = true;
            odsAdjNumber.SelectParameters[0].DefaultValue = ddlAccountlist.SelectedValue;
            odsAdjNumber.SelectParameters[1].DefaultValue = ddlValDate.SelectedValue;
            ddlAdjNumber.DataSourceID = "odsAdjNumber";
            ddlAdjNumber.DataTextField = "PREM_ADJ_ID";
            ddlAdjNumber.DataValueField = "PREM_ADJ_ID";
            ddlAdjNumber.DataBind();
            if (strSelectedPremAdjID != "")
                ddlAdjNumber.Items.FindByValue(strSelectedPremAdjID).Selected = true;
        }

        if (ddlAdjNumber.SelectedIndex != 0)
        {
            ddlProgramPeriod.Enabled = true;
            odsProgramPeriod.SelectParameters[0].DefaultValue = ddlAdjNumber.SelectedValue;
            odsProgramPeriod.SelectParameters[1].DefaultValue = ddlAccountlist.SelectedValue;
            ddlProgramPeriod.DataSourceID = "odsProgramPeriod";
            ddlProgramPeriod.DataTextField = "STARTDATE_ENDDATE_PGMTYP";
            ddlProgramPeriod.DataValueField = "PREM_ADJ_PGM_ID";
            ddlProgramPeriod.DataBind();
            if (strSelectedProgramPeriod != "")
                ddlProgramPeriod.Items.FindByValue(strSelectedProgramPeriod).Selected = true;
        }
        
        
    }
}
