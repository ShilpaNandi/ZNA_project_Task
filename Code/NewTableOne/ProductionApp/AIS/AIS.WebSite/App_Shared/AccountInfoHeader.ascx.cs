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

public partial class App_Shared_MasterUser : System.Web.UI.UserControl
{

    MasterEntities me = 
        (MasterEntities)System.Web.HttpContext.Current.Session["MasterEntities"];

    public string CurrentPageName
    {
        get
        {
            string[] strValues = this.Parent.Page.AppRelativeVirtualPath.Split(new char[] { '/' });
            return strValues[strValues.Length - 1].Split(new char[] { '.' })[0];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (me != null)
        {
            BuildControls();
        }
    }

    private bool IsValidDate(DateTime dt)
    {
        bool retValue = true;
        if(dt.Day == 1 && dt.Month == 1 && (dt.Year == 1 || dt.Year == 1000))
            retValue = false;
        return retValue;
    }

    private void Bindlabels()
    {
            this.lbl1genericrow1.Text = me.AccountName;
            this.lbl2genericrow1.Text = Convert.ToString(me.AccountNumber);
            this.lbl1genericrow2.Text = me.SSCGID;
            this.lbl2genericrow2.Text = me.Bpnumber;
    }
    private void BindStatuslabels()
    {
        this.lbl1genericrow1.Text = me.AccountName;
        this.lbl2genericrow1.Text = Convert.ToString(me.AccountNumber);
        this.lbl1genericrow2.Text = me.SSCGID;
        //this.lbl2genericrow2.Text = me.Bpnumber;

        this.lbl2genericrow2.Text = Convert.ToString(me.AccountStatus);
    }

    private void BindASTLabels()
    {
        this.lblRow1Title1.Text = "Account Name ";
        this.lblRow1Title2.Text = "Program Period ";
        this.lbl1genericrow1.Text = me.AccountName;
        this.lbl2genericrow1.Text = me.PremAdjProgramStartDate.ToShortDateString()+" - "+
                                    me.PremAdjProgramEndState.ToShortDateString();
        this.lbl1genericrow2.Visible = this.lbl2genericrow2.Visible = false;
        this.lblRow2Title1.Visible = this.lblRow2Title2.Visible = false;
    }

    private void BindRRADPCCLabels()
    {
        this.lblRow1Title2.Text = "Valuation Date ";
        this.lblRow2Title1.Text = "Adjustment # ";
        this.lblRow2Title2.Text = "Adjustment Date ";
        this.lbl1genericrow1.Text = me.AccountName;
        this.lbl2genericrow1.Text = (IsValidDate(me.ValuationDate))?me.ValuationDate.ToShortDateString():"";
        this.lbl1genericrow2.Text = me.AdjusmentNumber.ToString();
        this.lbl2genericrow2.Text = (IsValidDate(me.AdjustmentDate))?me.AdjustmentDate.ToShortDateString():"";
    }
    private void QCDetailsLabels()
    {
        this.lblRow1Title2.Text = "Valuation Date ";
        this.lblRow2Title1.Text = "Invoice # ";
        this.lblRow2Title2.Text = "Adjustment Date ";
        this.lbl1genericrow1.Text = me.AccountName;
        this.lbl2genericrow1.Text = (IsValidDate(me.ValuationDate))?me.ValuationDate.ToShortDateString():"";
        this.lbl1genericrow2.Text = me.InvoiceNumber.ToString();
        this.lbl2genericrow2.Text = (IsValidDate(me.AdjustmentDate))?me.AdjustmentDate.ToShortDateString():"";
    }

    private void BindArClLabels()
    {
        this.lblRow1Title1.Text = "Invoice Number ";
        this.lblRow1Title2.Text = "Invoice Date ";
        this.lblRow2Title1.Text = "Account Name ";
        this.lbl1genericrow1.Text = me.InvoiceNumber.ToString();
        this.lbl1genericrow2.Text = me.AccountName;
        this.lbl2genericrow1.Text = (IsValidDate(me.InvoiceDate))?me.InvoiceDate.ToShortDateString():"";
          
        this.lbl2genericrow2.Visible = false;
        this.lblRow2Title2.Visible = false;
 
    }

    private void BindLossInfo()
    {
        this.lblRow2Title1.Text = "Valuation Date ";

        this.lbl1genericrow1.Text = me.AccountName;
        this.lbl2genericrow1.Text = Convert.ToString(me.AccountNumber);
        this.lbl1genericrow2.Text = (IsValidDate(me.ValuationDate))?me.ValuationDate.ToShortDateString():"";
        this.lbl2genericrow2.Text = me.Bpnumber;
   
    }
    private void BindTPALabels()
    {
        this.lblRow1Title1.Text = "Account Name";
        this.lblRow1Title2.Text = "BP#";
        this.lblRow2Title1.Visible = false;
        this.lblRow2Title2.Visible = false;
        this.lbl1genericrow1.Text = me.AccountName;
        this.lbl1genericrow2.Visible = false;
        this.lbl2genericrow1.Text = me.Bpnumber;
        this.lbl2genericrow2.Visible = false;

    }
    private void BuildControls()
    { 
   
      this.lblHeaderTitle.Text = "Account Information";

      switch (CurrentPageName.ToUpper())
        { 
           case "ARIESCLEARING":
                BindArClLabels();
                break;
           case "ACCTSETUPPROCCHKLST":
               BindASTLabels();
               break;
           case "ADJUSTPROCESSINGCHKLST":
           case "ADJUSTMENTQC":
           case "RECONREVIEW":
               BindRRADPCCLabels();
               break;
           case "QCDETAILS":
           case "20%QC":
               QCDetailsLabels();
               break;
           case "PROGRAMPERIOD":
           case "POLICYINFO":
           case "ASSIGNCONTACTS":
           case "RETROINFO":           
           case "COMBINEDELEMENTS":
           case "TAXMULTIPLIER":
           case "ASSIGNERPFORMULA":
           case "CUSTOMERCOMMENTS":
           case "ILRFSETUP":
           case "LCFSETUP":
           case "RMLSETUP":
           case "CHFSETUP":
           case "ESCROWSETUP":
           case "PARAMETERSETUP":
           case "LBA":
           case "LOSSFUNDINFO":
           case "AUDITINFO":
               Bindlabels();
                 break;
           case "LOSSINFO":
           case "LOSSESREPORT":
           case "EXCESSNONBILLABLE":
                 BindLossInfo();
                 break;
          case "TPAMANUALPOSTING":
                BindTPALabels();
                break;
          default:
                break; 
        }

  }

}
