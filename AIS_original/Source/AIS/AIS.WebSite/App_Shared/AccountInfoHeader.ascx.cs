﻿using System;
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
    public string WindowName
    {
        get
        {

            if (HttpContext.Current.Request["wID"] != null && HttpContext.Current.Request["wID"] != "")
            {
                return HttpContext.Current.Request["wID"].ToString();
            }
            else
            {
                return "";
            }
        }
    }

    //MasterEntities me = (MasterEntities)System.Web.HttpContext.Current.Session["MasterEntities"];
    MasterEntities me = (MasterEntities)System.Web.HttpContext.Current.Session[HttpContext.Current.Request["wID"].ToString() + "MasterEntities"];

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
        if (dt.Day == 1 && dt.Month == 1 && (dt.Year == 1 || dt.Year == 1000))
            retValue = false;
        return retValue;
    }

    private void Bindlabels()
    {        //06/23 for veracode
             //this.lbl1genericrow1.Text = me.AccountName;
             //this.lbl1genericrow1.Text = Server.HtmlDecode(Server.HtmlEncode(me.AccountName));
             // this.lbl1genericrow1.Text = string.IsNullOrEmpty(me.AccountName.Trim()) ? "" : System.Web.HttpUtility.HtmlDecode(System.Web.HttpUtility.HtmlEncode(me.AccountName)); //EAISA - 7  06142018 for veracode
        if (string.IsNullOrEmpty(me.AccountName.Trim()))
        {
            this.lbl1genericrow1.Text = string.Empty;
        }
        else
        {
            this.lbl1genericrow1.Text = System.Web.HttpUtility.HtmlEncode(Convert.ToString(me.AccountName));
        }


        // this.lbl1genericrow1.Text =  System.Web.HttpUtility.HtmlDecode(System.Web.HttpUtility.HtmlEncode(me.AccountName));

        // this.lbl2genericrow1.Text = Convert.ToString(me.AccountNumber);
        this.lbl2genericrow1.Text = System.Web.HttpUtility.HtmlDecode(System.Web.HttpUtility.HtmlEncode(Convert.ToString(me.AccountNumber))); // EAISA - 7  06142018 for veracode

        //06/23 for veracode
        //this.lbl1genericrow2.Text = me.SSCGID;
        //
        //  this.lbl1genericrow2.Text = string.IsNullOrEmpty(me.SSCGID.Trim()) ? "" : System.Web.HttpUtility.HtmlDecode(System.Web.HttpUtility.HtmlEncode(Convert.ToString(me.SSCGID))); // EAISA - 7  06142018 for veracode
        if (string.IsNullOrEmpty(me.SSCGID.Trim()))
        {
            this.lbl1genericrow2.Text = string.Empty;
        }
        else
        {
            this.lbl1genericrow2.Text = System.Web.HttpUtility.HtmlEncode(Convert.ToString(me.SSCGID));
        }
        //06/23 for veracode
        //this.lbl2genericrow2.Text = me.Bpnumber;
        //this.lbl2genericrow2.Text = Server.HtmlDecode(Server.HtmlEncode(me.Bpnumber));
        this.lbl2genericrow2.Text = System.Web.HttpUtility.HtmlEncode(Convert.ToString(me.Bpnumber)); // EAISA - 7  06142018 for veracode

    }
    private void BindStatuslabels()
    {    //06/23 for veracode
         //this.lbl1genericrow1.Text = me.AccountName;
         // this.lbl1genericrow1.Text = Server.HtmlDecode(Server.HtmlEncode(me.AccountName));
        this.lbl1genericrow1.Text = HttpUtility.HtmlDecode(HttpUtility.HtmlEncode(me.AccountName)); // EAISA - 7  06072018 for veracode

        // this.lbl2genericrow1.Text = Convert.ToString(me.AccountNumber);
        this.lbl2genericrow1.Text = HttpUtility.HtmlDecode(HttpUtility.HtmlEncode(Convert.ToString(me.AccountNumber))); //EAISA - 7  06072018 for veracode

        //this.lbl1genericrow2.Text = me.SSCGID;
        this.lbl1genericrow2.Text = HttpUtility.HtmlDecode(HttpUtility.HtmlEncode(me.SSCGID));    // EAISA - 7  06072018 for veracode

        //this.lbl2genericrow2.Text = me.Bpnumber;

        //this.lbl2genericrow2.Text = Convert.ToString(me.AccountStatus);
        this.lbl2genericrow2.Text = HttpUtility.HtmlDecode(HttpUtility.HtmlEncode(Convert.ToString(me.AccountStatus))); // EAISA - 7  06072018 for veracode

    }

    private void BindASTLabels()
    {
        this.lblRow1Title1.Text = "Account Name ";
        this.lblRow1Title2.Text = "Program Period ";
        //06/23 for veracode
        //this.lbl1genericrow1.Text = me.AccountName;
        //this.lbl1genericrow1.Text = Server.HtmlDecode(Server.HtmlEncode(me.AccountName));
        this.lbl1genericrow1.Text = System.Web.HttpUtility.HtmlEncode(Convert.ToString(me.AccountName));  // EAISA - 7  06142018  for veracode

        this.lbl2genericrow1.Text = me.PremAdjProgramStartDate.ToShortDateString() + " - " +
                                    me.PremAdjProgramEndState.ToShortDateString();
        this.lbl1genericrow2.Visible = this.lbl2genericrow2.Visible = false;
        this.lblRow2Title1.Visible = this.lblRow2Title2.Visible = false;
    }

    private void BindRRADPCCLabels()
    {
        this.lblRow1Title2.Text = "Valuation Date ";
        this.lblRow2Title1.Text = "Adjustment # ";
        this.lblRow2Title2.Text = "Adjustment Date ";
        //06/23 for veracode
        //this.lbl1genericrow1.Text = me.AccountName;
        this.lbl1genericrow1.Text = System.Web.HttpUtility.HtmlEncode(Convert.ToString(me.AccountName)); // EAISA - 7  06072018 for veracode
        this.lbl2genericrow1.Text = (IsValidDate(me.ValuationDate)) ? me.ValuationDate.ToShortDateString() : "";
        this.lbl1genericrow2.Text = me.AdjusmentNumber.ToString();
        this.lbl2genericrow2.Text = (IsValidDate(me.AdjustmentDate)) ? me.AdjustmentDate.ToShortDateString() : "";
    }
    private void QCDetailsLabels()
    {
        this.lblRow1Title2.Text = "Valuation Date ";
        this.lblRow2Title1.Text = "Invoice # ";
        this.lblRow2Title2.Text = "Adjustment Date ";
        //06/23 for veracode
        //this.lbl1genericrow1.Text = me.AccountName;
        //this.lbl1genericrow1.Text = string.IsNullOrEmpty(me.AccountName.Trim()) ? "" : System.Web.HttpUtility.HtmlDecode(System.Web.HttpUtility.HtmlEncode(me.AccountName)); // EAISA - 7  06072018 for veracode
        this.lbl1genericrow1.Text = System.Web.HttpUtility.HtmlEncode(Convert.ToString(me.AccountName));
        this.lbl2genericrow1.Text = (IsValidDate(me.ValuationDate)) ? me.ValuationDate.ToShortDateString() : "";
        this.lbl1genericrow2.Text = me.InvoiceNumber.ToString();
        this.lbl2genericrow2.Text = (IsValidDate(me.AdjustmentDate)) ? me.AdjustmentDate.ToShortDateString() : "";
    }

    private void BindArClLabels()
    {
        this.lblRow1Title1.Text = "Invoice Number ";
        this.lblRow1Title2.Text = "Invoice Date ";
        this.lblRow2Title1.Text = "Account Name ";
        this.lbl1genericrow1.Text = me.InvoiceNumber.ToString();
        //06/23 for veracode
        //this.lbl1genericrow2.Text = me.AccountName;
        // this.lbl1genericrow2.Text = string.IsNullOrEmpty(me.AccountName.Trim()) ? "" : System.Web.HttpUtility.HtmlDecode(System.Web.HttpUtility.HtmlEncode(me.AccountName)); // EAISA - 7  06072018 for veracode
        this.lbl1genericrow1.Text = System.Web.HttpUtility.HtmlEncode(Convert.ToString(me.AccountName));
        this.lbl2genericrow1.Text = (IsValidDate(me.InvoiceDate)) ? me.InvoiceDate.ToShortDateString() : "";

        this.lbl2genericrow2.Visible = false;
        this.lblRow2Title2.Visible = false;

    }

    private void BindLossInfo()
    {
        this.lblRow2Title1.Text = "Valuation Date ";
        //06/23 for veracode
        //this.lbl1genericrow1.Text = me.AccountName;
        // this.lbl1genericrow1.Text = string.IsNullOrEmpty(me.AccountName.Trim()) ? "" : System.Web.HttpUtility.HtmlDecode(System.Web.HttpUtility.HtmlEncode(me.AccountName)); // EAISA - 7  06072018 for veracode
        this.lbl1genericrow1.Text = System.Web.HttpUtility.HtmlEncode(Convert.ToString(me.AccountName));
        this.lbl2genericrow1.Text = Convert.ToString(me.AccountNumber);
        this.lbl1genericrow2.Text = (IsValidDate(me.ValuationDate)) ? me.ValuationDate.ToShortDateString() : "";
        //06/23 for veracode
        //this.lbl2genericrow2.Text = me.Bpnumber;
        //this.lbl2genericrow2.Text = string.IsNullOrEmpty(me.Bpnumber.Trim()) ? "" : System.Web.HttpUtility.HtmlDecode(System.Web.HttpUtility.HtmlEncode(me.Bpnumber)); // EAISA - 7  06072018 for veracode
        this.lbl2genericrow2.Text = System.Web.HttpUtility.HtmlEncode(Convert.ToString(me.Bpnumber));
    }
    private void BindTPALabels()
    {
        this.lblRow1Title1.Text = "Account Name";
        this.lblRow1Title2.Text = "BP#";
        this.lblRow2Title1.Visible = false;
        this.lblRow2Title2.Visible = false;
        //06/23 for veracode
        //this.lbl1genericrow1.Text = me.AccountName;
        //this.lbl1genericrow1.Text = string.IsNullOrEmpty(me.AccountName.Trim()) ? "" : System.Web.HttpUtility.HtmlDecode(System.Web.HttpUtility.HtmlEncode(me.AccountName)); // EAISA - 7  06072018 for veracode
        this.lbl1genericrow1.Text = System.Web.HttpUtility.HtmlEncode(Convert.ToString(me.AccountName));
        this.lbl1genericrow2.Visible = false;
        //06/23 for veracode
        //this.lbl2genericrow1.Text = me.Bpnumber;
        //this.lbl2genericrow1.Text = string.IsNullOrEmpty(me.Bpnumber.Trim()) ? "" : System.Web.HttpUtility.HtmlDecode(System.Web.HttpUtility.HtmlEncode(me.Bpnumber));  // EAISA - 7  06072018 for veracode
        this.lbl2genericrow1.Text = System.Web.HttpUtility.HtmlEncode(Convert.ToString(me.Bpnumber));
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
