using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ZurichNA.LSP.Framework;
using System.Collections.Generic;

public partial class Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        List<ZAAttachment> attach = new List<ZAAttachment>();
        SMTPMailer dd = new SMTPMailer();

        if (TextBox5.Text.Length > 0)
        {
            ZAAttachment attach1 = new ZAAttachment();
            attach1.AttachmentName = "test attachment";
            attach1.FullAttachmentPath = TextBox5.Text;
            attach.Add(attach1);
            dd.AttachmentList = attach;
            dd.HasAttachment = true;
        }

        dd.AddMailRecepients(TextBox1.Text, TextBox2.Text);
        dd.Subject = TextBox3.Text;
        dd.Body = TextBox4.Text;

        dd.SendMail();

    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        //FTPTransfer ftp = new FTPTransfer();
        //ftp.FullfileName = TextBox5.Text;
        //ftp.Userid = "ARMSIFTP";
        //ftp.ServerIPAddress = "DAVWB1322:1750";
        //ftp.Password = "AIS789)_";
        //ftp.UploadFile();


    }


}