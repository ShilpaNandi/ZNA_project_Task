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

namespace ZurichNA.AIS.WebSite.Invoice
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string path = Server.MapPath("~/GeneratedReports/DraftExternalInvoice_368.pdf");
                string inline = "";
                System.IO.FileInfo file = new System.IO.FileInfo(path);
                //System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);

                if (file.Exists)
                {
                    Page.Response.Clear();
                    //FIXME: return correct Content-Type
                    Response.AppendHeader("Content-Type", "application/octet-stream");
                    System.String header;
                    if (inline != null && inline.Equals("1"))
                        header = "inline";
                    else
                        header = "attachment";
                    header = System.String.Format("{0}; filename=\"{1}\"; size=\"{2}\";", header, path.Substring(path.LastIndexOf("\\") + 1), file.Length);
                    Response.AppendHeader("Content-Disposition", header);
                    Response.AppendHeader("Content-Length", file.Length.ToString());
                    Response.WriteFile(file.FullName);
                }
            }


        }
        protected void lkbDownload_Click(object obj, EventArgs e)
        {

        }
    }
}
