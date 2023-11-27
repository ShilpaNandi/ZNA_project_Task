using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.IO;
using System.Configuration;


namespace ZurichNA.AIS.ZDW.Interface
{
    public class ZAAttachment
    {
        public ZAAttachment()
        {
        }

        private byte[] _attach;
        private bool _isBinaryFormat;
        private string _attachmentPath;
        private string _attachmentName;
        private string _attachmentType;

        public byte[] Attach
        {
            get 
            {
                return _attach;
            }

            set 
            {
                if (value.Length > 0)
                {
                    _attach = value;
                    _isBinaryFormat = true;
                }
            }
        }

        public bool IsBinaryFormat
        {
            get 
            {
                return _isBinaryFormat; 
            }
        }

        public string FullAttachmentPath
        {
            get
            {
                return _attachmentPath;
            }
            set
            {
                _attachmentPath = value;
            }
        }

        public string AttachmentName
        {
            get
            {
                return _attachmentName;
            }
            set
            {
                _attachmentName = value;
            }
        }

        public string AttachmentType
        {
            get
            {
                return (string)_attachmentType;
            }
            set
            {
                _attachmentType = (string)value;
            }
        }

    }
    public class SMTPMailer
    {
        //private Common common = null;
        public SMTPMailer()
        {
            //common = new Common(this.GetType());
        }

        #region Properties

        public static string PDF = MediaTypeNames.Application.Pdf;
        public static string Excel = MediaTypeNames.Application.Octet;
        public static string word = MediaTypeNames.Application.Rtf;
        public static string Text = MediaTypeNames.Text.Plain;


        private string _mailFrom;
        private string _FromDisplayName;
        private string[] _mailTo;
        private string[] _mailCC;
        private string[] _mailBCC;
        private string _subject;
        private string _body;
        //private byte[] _attach;
        //private string _attachmentName;
        //private string _attachmentType;
        private List<ZAAttachment> _lstattach;
        private bool _hasAttachment = false;

        public List<ZAAttachment> AttachmentList
        {
            get
            {
                if (_lstattach == null)
                    _lstattach = new List<ZAAttachment>();
                return _lstattach;
            }
            set
            {
                _lstattach = value;
            }
        }
        public string mailFrom
        {
            get
            {
                return _mailFrom;
            }
            set
            {
                _mailFrom = value;
            }
        }

        public string FromDisplayName
        {
            get
            {
                return _FromDisplayName;
            }
            set
            {
                _FromDisplayName = value;
            }
        }

        public string[] mailTo
        {
            get
            {
                return _mailTo;
            }
            set
            {
                _mailTo = value;
            }
        }
        MailAddressCollection _lstmailAddress;
        public MailAddressCollection lstTomailAddress
        {
            get
            {
                return _lstmailAddress;
            }
            set
            {
                _lstmailAddress = value;
            }
        }

        public void AddMailRecepients(string toaddress, string DisplayName)
        {
            if (_lstmailAddress == null)
                _lstmailAddress = new MailAddressCollection();
            if (_lstmailAddress.Count == 0)
                _lstmailAddress = new MailAddressCollection();
            MailAddress mMailadress = new MailAddress(toaddress, DisplayName);
            _lstmailAddress.Add(mMailadress);
        }

        public string[] mailCC
        {
            get
            {
                return _mailCC;
            }
            set
            {
                _mailCC = value;
            }
        }

        public string[] mailBCC
        {
            get
            {
                return _mailBCC;
            }
            set
            {
                _mailBCC = value;
            }
        }

        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }

        public string Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
            }
        }

        public bool HasAttachment
        {
            get
            {
                return _hasAttachment;
            }
            set
            {
                _hasAttachment = value;
            }
        }
        #endregion


        public void SendMail()
        {
            int countAddresses = 0;
            try
            {

                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
               
                for (int iter = 0; iter < lstTomailAddress.Count; iter++)
                {
                    msg.To.Add(lstTomailAddress[iter]);
                }

                if ((mailCC != null) && ((countAddresses = mailCC.Length) > 0))
                {
                    for (int iter = 0; iter < countAddresses; iter++)
                    {
                        if (mailCC[iter] != null && mailCC[iter].Length > 0)
                        {
                            msg.CC.Add(mailCC[iter].Trim());
                        }
                    }
                }
                if ((mailBCC != null) && ((countAddresses = mailBCC.Length) > 0))
                {
                    for (int iter = 0; iter < countAddresses; iter++)
                    {
                        if (mailBCC[iter] != null && mailBCC[iter].Length > 0)
                        {
                            msg.Bcc.Add(mailBCC[iter].Trim());
                        }
                    }
                }
                mailFrom = ConfigurationSettings.AppSettings["ReturnEmailAddress"].ToString();
                FromDisplayName = ConfigurationSettings.AppSettings["ReturnDisplayName"].ToString();
                if (FromDisplayName == "")
                    FromDisplayName = "sender";
                msg.From = new MailAddress(mailFrom, FromDisplayName, System.Text.Encoding.UTF8);
                msg.Subject = Subject;
                msg.Body = Body;
                msg.IsBodyHtml = false;
                msg.Priority = System.Net.Mail.MailPriority.Normal;

                if (HasAttachment == true)
                {
                    Attachment data = null;
                    for (int icount = 0; icount < AttachmentList.Count; icount++)
                    {
                        if (AttachmentList[icount].IsBinaryFormat)
                        {
                            MemoryStream ms = new MemoryStream(AttachmentList[icount].Attach);
                            data = new Attachment(ms, AttachmentList[icount].AttachmentType);
                            ContentDisposition disposition = data.ContentDisposition;
                            data.ContentDisposition.FileName = AttachmentList[icount].AttachmentName;
                            msg.Attachments.Add(data);
                        }
                        else
                        {
                            Attachment att = new Attachment(AttachmentList[icount].FullAttachmentPath);
                            msg.Attachments.Add(att);
                        }
                    }
                }

                SmtpClient client = new SmtpClient();
                /// If MailFrom not set then set it to value entered in web.config setting

                client.Credentials = CredentialCache.DefaultNetworkCredentials;
                client.Host = ConfigurationSettings.AppSettings["SMTPServerName"];
                client.Port = Convert.ToInt32(ConfigurationSettings.AppSettings["SMTPServerPort"]);
                client.EnableSsl = false;

                client.Send(msg);
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                //common.Logger.Error("SMTP Exception while sending E-Mail", ex);
                //throw ex;
                ZDWInterface oZDW = new ZDWInterface();
                oZDW.WriteFileLogFile("SMTP Exception while sending E-Mail");
                oZDW.WriteFileLogFile(ex.Message);
            }
            catch (Exception e)
            {
                //common.Logger.Error("Error sending E-Mail", e);
                //throw e;
                ZDWInterface oZDW = new ZDWInterface();
                oZDW.WriteFileLogFile("Error sending E-Mail");
                oZDW.WriteFileLogFile(e.Message);
            }
        }

    }
}
