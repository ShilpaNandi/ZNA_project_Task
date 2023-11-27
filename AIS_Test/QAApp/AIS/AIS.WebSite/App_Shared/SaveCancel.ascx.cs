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

public partial class App_Shared_SaveCancel : System.Web.UI.UserControl
{
    public event EventHandler OperationsButtonClicked;

    #region Properties
    private string operation;
    public string Operation
    {
        get { return operation; }
        set { operation = value; }
    }

    //public bool SetCancelVisible
    //  {
    //     get { return btnCancel.Visible; }
    //    set { btnCancel.Visible = value; }
    // }

    // public bool DoProcesValidation
    // {
    //  get { return btnProcess.CausesValidation; }
    //  set { btnProcess.CausesValidation = value; }
    //   }

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        string strResetCtlDirtyBit =
          "Javascript:document.getElementById('ctl00_hdnControlDirty').value=0;";

        cmdSave.Attributes["onClick"] = strResetCtlDirtyBit + " submitted = 1;";
        cmdCancel.Attributes["onClick"] = strResetCtlDirtyBit + " submitted = 0;";
    }
    protected void UC_Click(Object sender, CommandEventArgs e)
    {
        Operation = e.CommandName.ToString().Trim();
        OnOperationsButtonClicked(this,e);
    }

    protected void OnOperationsButtonClicked(Object sender, CommandEventArgs e)
    {
        if (OperationsButtonClicked != null)
        {
            OperationsButtonClicked(this, e);
            ((HiddenField)Page.Master.FindControl("hdnControlDirty")).Value = "0";
        }
    }
}
