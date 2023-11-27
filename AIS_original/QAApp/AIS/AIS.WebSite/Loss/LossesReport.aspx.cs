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


public partial class LossesReport : AISBasePage
{
    /// <summary>
    /// PageLoad Event code appears here
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    #region Page Load Event
    protected void Page_Load(object sender, EventArgs e)
    {
        //To display the Effective Date Header
        //lblPolicyEffectDate.Text = "Policy Effective Date " + AISMasterEntities.ExcessLoss.EffectiveDate1.ToShortDateString() + "-" + AISMasterEntities.ExcessLoss.EffectiveDate2.ToShortDateString();
        lblPolicyEffectDate.Text = "Program Period " + AISMasterEntities.ExcessLoss.ProgramPeriod;
        //To show the respective panels based on  QueryString
        switch (Request.QueryString["mode"])
        {
            case "Policy":
                pnlLossByLOB.Visible = false;
                pnlLossByState.Visible = false;
                pnlExcNonBillableLoss.Visible = false;
                this.Master.Page.Title = "Losses By Policy";
                lblLossByHeader.Text = "Losses By Policy";
                PopulateLossByPolicyListView();
                ViewState["report"] = "Policy";
                break;

            case "LOB":
                pnlLossByPolicyList.Visible = false;
                pnlLossByState.Visible = false;
                pnlExcNonBillableLoss.Visible = false;
                this.Master.Page.Title = "Losses By LOB";
                lblLossByHeader.Text = "Losses By LOB";
                PopulateLossByLOBListView();
                ViewState["report"] = "LOB";
                break;

            case "STATE":

                pnlLossByPolicyList.Visible = false;
                pnlLossByLOB.Visible = false;
                pnlExcNonBillableLoss.Visible = false;
                this.Master.Page.Title = "Losses By State";
                lblLossByHeader.Text = "Losses By State";
                PopulateLossByStateListView();
                ViewState["report"] = "STATE";
                break;

            case "NonBillable":
                pnlLossByPolicyList.Visible = false;
                pnlLossByLOB.Visible = false;
                pnlLossByState.Visible = false;
                this.Master.Page.Title = "Excess/Non Billable Losses";
                lblLossByHeader.Text = "Excess/Non Billable Losses";
                PopulateExcessNonBillableLossListView();
                ViewState["report"] = "NonBillable";
                break;

        }
    }
    #endregion

    /// <summary>
    ///  a property for LossInfo Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>LossInfoBS</returns>
    #region LossInfo Business Class
    private LossInfoBS lossInfo;
    private LossInfoBS LossInfo
    {
        get
        {
            if (lossInfo == null)
            {
                lossInfo = new LossInfoBS();
            }
            return lossInfo;
        }
    }
    #endregion
    /// <summary>
    ///  a property for Excess NonBillable LossInfo Business Service Class
    /// </summary>
    /// <param name=""></param>
    /// <returns>ExcessNonBillableBS</returns>
    #region Excess NonBillable Loss Business Class
    private ExcessNonBillableBS excNBlossInfo;
    private ExcessNonBillableBS ExcNBlossInfo
    {
        get
        {
            if (excNBlossInfo == null)
            {
                excNBlossInfo = new ExcessNonBillableBS();
            }
            return excNBlossInfo;
        }
    }
    #endregion


    /// <summary>
    /// Function to bind the ListViews in case of NonBillable mode
    /// </summary>
    #region NonBillable ListView Binding
    private void PopulateExcessNonBillableLossListView()
    {
        try
        {
            int intARMISLOSSEXCID = AISMasterEntities.ExcessLoss.ARMISLOssExcID;
            lstMasterExcNonBillableLoss.DataSource = ExcNBlossInfo.getExcNonBillableData(intARMISLOSSEXCID);
            lstMasterExcNonBillableLoss.DataBind();
            for (int i = 0; i < lstMasterExcNonBillableLoss.Items.Count; i++)
            {
                ListView lstChild = (ListView)lstMasterExcNonBillableLoss.Items[i].FindControl("lstChildExcNonBillableLosses");
                Label lblid = (Label)lstMasterExcNonBillableLoss.Items[i].FindControl("lblID");
                int id = Convert.ToInt32(lblid.Text);
                lstChild.DataSource = ExcNBlossInfo.getExcNonBillableData(id);
                lstChild.DataBind();

            }
        }
        catch (RetroBaseException rbe)
        {
            ShowError(rbe.Message,rbe);
        }

    }
    #endregion
    /// <summary>
    /// Function to bind the ListViews in case of State mode 
    /// </summary>
    #region LossByState ListView Binding
    private void PopulateLossByStateListView()
    {
        try
        {
            int intARMISID = AISMasterEntities.ExcessLoss.ARMISLossID;
            lstMasterLossByState.DataSource = LossInfo.getLossInfoData(intARMISID);
            lstMasterLossByState.DataBind();
            for (int i = 0; i < lstMasterLossByState.Items.Count; i++)
            {
                ListView lstChild = (ListView)lstMasterLossByState.Items[i].FindControl("lstChildLossesByState");
                Label lblid = (Label)lstMasterLossByState.Items[i].FindControl("lblID");
                int id = Convert.ToInt32(lblid.Text);
                lstChild.DataSource = LossInfo.getLossInfoData(id);
                lstChild.DataBind();

            }
        }
        catch (RetroBaseException rbe)
        {
            ShowError(rbe.Message,rbe);
        }
    }
    #endregion
    /// <summary>
    /// Function to bind the ListViews in case of LOB mode 
    /// </summary>
    #region LossByLOB Listview Binding
    private void PopulateLossByLOBListView()
    {
        try
        {
            int custmrID =AISMasterEntities.AccountNumber;
            int prgID = Convert.ToInt32(AISMasterEntities.ExcessLoss.PremiumAdjPgmID.ToString());
            DateTime ValDate = DateTime.Parse(AISMasterEntities.ValuationDate.ToString());
            IList<LossInfoBE> lossLOB = new List<LossInfoBE>();
            if (((string)Session["Adjdtls"]) == "Adjustmentdetails" || ((string)Session["Adjdtls"]) == "Invoice")
            {
                lossLOB = LossInfo.getLossInfoByLOBDataAdjNo(ValDate, custmrID, prgID, AISMasterEntities.AdjusmentNumber).Where(obj => obj.PREM_ADJ_ID != null).ToList();
                lossLOB = lossLOB.OrderBy(ll => ll.POLICYSYMBOL).ToList();
                lstMasterLossByLOB.DataSource = lossLOB;
                lstMasterLossByLOB.DataBind();
            }
            else if (((string)Session["Adjdtls"]) == "LossInfo")
            {
                lossLOB = LossInfo.getLossInfoByLOBData(ValDate, custmrID, prgID).Where(obj => obj.PREM_ADJ_ID == null).ToList();
                lossLOB = lossLOB.OrderBy(ll => ll.POLICYSYMBOL).ToList();
                lstMasterLossByLOB.DataSource = lossLOB;
                lstMasterLossByLOB.DataBind();
            }
            //IList<LossInfoBE> lossLOB = LossInfo.getLossInfoByPolicyData(ValDate, custmrID, prgID);
            //lstMasterLossByLOB.DataSource = lossLOB.OrderBy(ll => ll.POLICYSYMBOL).ToList();
            //lstMasterLossByLOB.DataBind();
            if (((string)Session["Adjdtls"]) == "Adjustmentdetails" || ((string)Session["Adjdtls"]) == "Invoice")
            {
                for (int i = 0; i < lstMasterLossByLOB.Items.Count; i++)
                {
                    ListView lstChild = (ListView)lstMasterLossByLOB.Items[i].FindControl("lstChildLossesByLOB");
                    Label lblid = (Label)lstMasterLossByLOB.Items[i].FindControl("lblID");
                    string Lob = (lblid.Text);
                    lstChild.DataSource = LossInfo.getLossInfoByLOBDataAdjNoCovg(ValDate, custmrID, prgID, AISMasterEntities.AdjusmentNumber, Lob).Where(obj => obj.PREM_ADJ_ID != null).ToList();
                    lstChild.DataBind();

                }
            }
            else if (((string)Session["Adjdtls"]) == "LossInfo")
            {
                for (int i = 0; i < lstMasterLossByLOB.Items.Count; i++)
                {
                    ListView lstChild = (ListView)lstMasterLossByLOB.Items[i].FindControl("lstChildLossesByLOB");
                    Label lblid = (Label)lstMasterLossByLOB.Items[i].FindControl("lblID");
                    string Lob =(lblid.Text);
                    lstChild.DataSource = LossInfo.getLossInfoByLOBDataCovg(ValDate, custmrID, prgID, Lob).Where(obj => obj.PREM_ADJ_ID == null).ToList();
                    lstChild.DataBind();

                }

            }
        }
        catch (RetroBaseException rbe)
        {
            ShowError(rbe.Message,rbe);
        }
    }
    #endregion
    /// <summary>
    ///  Function to bind the ListViews in case of Policy mode 
    /// </summary>
    #region LossByPolicy Listview Binding
    private void PopulateLossByPolicyListView()
    {
        try
        {
            int custmrID = AISMasterEntities.AccountNumber;
            int prgID = Convert.ToInt32(AISMasterEntities.ExcessLoss.PremiumAdjPgmID.ToString());
            DateTime ValDate = DateTime.Parse(AISMasterEntities.ValuationDate.ToString());
            IList<LossInfoBE> losInfoBEList = new List<LossInfoBE>();
            if (((string)Session["Adjdtls"]) == "Adjustmentdetails" || ((string)Session["Adjdtls"]) == "Invoice")
            {
                losInfoBEList = LossInfo.getLossInfoByPolicyDataAdjNo(ValDate, custmrID, prgID, AISMasterEntities.AdjusmentNumber).Where(obj => obj.PREM_ADJ_ID != null).ToList();
                losInfoBEList = losInfoBEList.OrderBy(ll => ll.POLICYSYMBOL).ToList();
                lstMasterLossesByPolicy.DataSource = losInfoBEList;
                lstMasterLossesByPolicy.DataBind();
            }
            else if (((string)Session["Adjdtls"]) == "LossInfo")
            {
                losInfoBEList = LossInfo.getLossInfoByPolicyData(ValDate, custmrID, prgID).Where(obj => obj.PREM_ADJ_ID == null).ToList();
                losInfoBEList = losInfoBEList.OrderBy(ll => ll.POLICYSYMBOL).ToList();
                lstMasterLossesByPolicy.DataSource = losInfoBEList;
                lstMasterLossesByPolicy.DataBind();
            }
            //lstMasterLossesByPolicy.DataSource = LossInfo.getLossInfoByPolicyData(ValDate, custmrID, prgID).w;
            //lstMasterLossesByPolicy.DataBind();
            if (((string)Session["Adjdtls"]) == "Adjustmentdetails" || ((string)Session["Adjdtls"]) == "Invoice")
            {
                for (int i = 0; i < lstMasterLossesByPolicy.Items.Count; i++)
                {
                    ListView lstChild = (ListView)lstMasterLossesByPolicy.Items[i].FindControl("lstChildLossesByPoicy");
                    Label lblid = (Label)lstMasterLossesByPolicy.Items[i].FindControl("lblID");
                    int id = Convert.ToInt32(lblid.Text);
                    lstChild.DataSource = LossInfo.getLossInfoDataAdjNoComl(ValDate, custmrID, prgID, AISMasterEntities.AdjusmentNumber, id).Where(obj => obj.PREM_ADJ_ID != null).ToList();
                    lstChild.DataBind();

                }
            }
            else if (((string)Session["Adjdtls"]) == "LossInfo")
            {
                for (int i = 0; i < lstMasterLossesByPolicy.Items.Count; i++)
                {
                    ListView lstChild = (ListView)lstMasterLossesByPolicy.Items[i].FindControl("lstChildLossesByPoicy");
                    Label lblid = (Label)lstMasterLossesByPolicy.Items[i].FindControl("lblID");
                    int id = Convert.ToInt32(lblid.Text);
                    lstChild.DataSource = LossInfo.getLossInfoDataComl(ValDate, custmrID, prgID, id).Where(obj => obj.PREM_ADJ_ID == null).ToList();
                    lstChild.DataBind();

                }
            }
        }
        catch (RetroBaseException rbe)
        {
            ShowError(rbe.Message,rbe);
        }
    }
    #endregion
    protected void lbBack_Click(object sender, EventArgs e)
    {
        Server.Transfer("LossInfo.aspx", true);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        if(ViewState["report"].ToString() != "NonBillable")
            Response.Redirect("~/Loss/LossInfo.aspx");
        else
            Response.Redirect("~/Loss/ExcessNonBillable.aspx");
    }
}


