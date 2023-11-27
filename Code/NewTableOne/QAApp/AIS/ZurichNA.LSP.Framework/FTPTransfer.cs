using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;
using System.Net;

namespace ZurichNA.LSP.Framework
{
    /// <summary>
    /// This class holds the common framework for
    /// transferring files using FTP
    /// </summary>
    public class FTPTransfer
    {
        private Common common = null;

        public FTPTransfer()
        {
            common = new Common(this.GetType());
        }

        public FTPTransfer(string ipaddress, string userid, string password)
        {
            common = new Common(this.GetType());
            this.ServerIPAddress = ipaddress;
            this.Userid = userid;
            this.Password = password;
        }

        private string _serverIPAddress;
        private string _userid;
        private string _password;
        private string _fullfileName;

        public string ServerIPAddress
        {
            get
            {
                return _serverIPAddress;
            }
            set
            {
                _serverIPAddress = value;
            }
        }

        public string Userid
        {
            get
            {
                return _userid;
            }
            set
            {
                _userid = value;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        public string FullfileName
        {
            get
            {
                return _fullfileName;
            }
            set
            {
                _fullfileName = value;
            }
        }

        public bool checkFileExists()
        {
            try
            {
                FtpWebRequest reqFTP;
                //filePath = <<The full path where the 
                //file is to be created. the>>, 
                //fileName = <<Name of the file to be createdNeed not 
                //name on FTP server. name name()>>
                FileStream outputStream = new FileStream(FullfileName, FileMode.Create);
                FileInfo fileInf = new FileInfo(FullfileName);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" +
                                        ServerIPAddress + "/" + fileInf.Name));
                reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(Userid, Password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                ftpStream.Close();
                response.Close();
                return true;

            }
            catch //(Exception ex)
            {
                return false;
            }
        }

        public bool UploadFile()
        {
            try
            {
                FileInfo fileInf = new FileInfo(FullfileName);
                string uri = "ftp://" + ServerIPAddress + "/" + fileInf.Name;
                FtpWebRequest reqFTP;

                // Create FtpWebRequest object from the Uri provided
                reqFTP = (FtpWebRequest)FtpWebRequest.Create
                         (new Uri("ftp://" + ServerIPAddress + "/" + fileInf.Name));

                // Provide the WebPermission Credintials
                reqFTP.Credentials = new NetworkCredential(Userid, Password);

                // By default KeepAlive is true, where the control connection
                // is not closed after a command is executed.
                reqFTP.KeepAlive = false;

                // Specify the command to be executed.
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

                // Specify the data transfer type.
                reqFTP.UseBinary = true;

                // Notify the server about the size of the uploaded file
                reqFTP.ContentLength = fileInf.Length;

                // The buffer size is set to 2kb
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;

                // Opens a file stream (System.IO.FileStream) to read the file
                // to be uploaded
                FileStream fs = fileInf.OpenRead();


                // Stream to which the file to be upload is written
                Stream strm = reqFTP.GetRequestStream();

                // Read from the file stream 2kb at a time
                contentLen = fs.Read(buff, 0, buffLength);

                // Till Stream content ends
                while (contentLen != 0)
                {
                    // Write Content from the file stream to the FTP Upload
                    // Stream
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                // Close the file stream and the Request Stream
                strm.Close();
                fs.Close();
                return true;
            }
            catch //(Exception ex)
            {
                return false;
            }

        }
        public bool UploadFile2()
        {
            //try
            //{
            //    // Create a Uri instance with the specified URI string.
            //    // If the URI is not correctly formed, the Uri constructor
            //    // will throw an exception.
            //    ManualResetEvent waitObject;

            //    Uri target = new Uri(args[0]);
            //    string fileName = args[1];
            //    FtpState state = new FtpState();
            //    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(target);
            //    request.Method = WebRequestMethods.Ftp.UploadFile;

            //    // This example uses anonymous logon.
            //    // The request is anonymous by default; the credential does not have to be specified. 
            //    // The example specifies the credential only to
            //    // control how actions are logged on the server.

            //    request.Credentials = new NetworkCredential("anonymous", "janeDoe@contoso.com");

            //    // Store the request in the object that we pass into the
            //    // asynchronous operations.
            //    state.Request = request;
            //    state.FileName = fileName;

            //    // Get the event to wait on.
            //    waitObject = state.OperationComplete;

            //    // Asynchronously get the stream for the file contents.
            //    request.BeginGetRequestStream(
            //        new AsyncCallback(EndGetStreamCallback),
            //        state
            //    );

            //    // Block the current thread until all operations are complete.
            //    waitObject.WaitOne();

            //    // The operations either completed or threw an exception.
            //    if (state.OperationException != null)
            //    {
            //        throw state.OperationException;
            //    }
            //    else
            //    {
            //        Console.WriteLine("The operation completed - {0}", state.StatusDescription);
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
            return true;
        }
    }
}
