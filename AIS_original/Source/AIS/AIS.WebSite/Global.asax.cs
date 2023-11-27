using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using System.Security.Principal;
using System.Collections.Generic;

using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;
using ZurichNA.LSP.Framework;
using System.Web.Caching;

namespace ZurichNA.AIS.WebSite
{
    public class Global : System.Web.HttpApplication
    {
        private Common common = null;

        void Application_Start(object sender, EventArgs e)
        {
            ////Retrieves all the LookUp Data and stores in Application Variable
            //IList<LookupBE> lookupBEs = new List<LookupBE>();
            //LookupBS lookupBS = new LookupBS();
            //lookupBEs = lookupBS.getLookupData();
            //Application["LookUpData"] = lookupBEs;

            //Adding Tab based pages
            Hashtable AISTabPageList;
            if (Application["AISTABPAGELIST"] == null)
            {
                AISTabPageList = new Hashtable();
                AISTabPageList.Add("ADJPARAMS/ASSIGNERPFORMULA.ASPX", "ADJPARAMS/RETROINFO.ASPX");
                AISTabPageList.Add("ADJPARAMS/AUDITINFO.ASPX", "ADJPARAMS/RETROINFO.ASPX");
                AISTabPageList.Add("ADJPARAMS/COMBINEDELEMENTS.ASPX", "ADJPARAMS/RETROINFO.ASPX");

                AISTabPageList.Add("ADJCALC/ESCROWADJUSTMENT.ASPX", "ADJCALC/ADJUSTMENTREVIEW.ASPX");
                AISTabPageList.Add("ADJCALC/COMBINEDELEMENTS.ASPX", "ADJCALC/ADJUSTMENTREVIEW.ASPX");
                AISTabPageList.Add("ADJCALC/NY-SIF.ASPX", "ADJCALC/ADJUSTMENTREVIEW.ASPX");
                AISTabPageList.Add("ADJCALC/MISCINVOICING.ASPX", "ADJCALC/ADJUSTMENTREVIEW.ASPX");
                AISTabPageList.Add("ADJCALC/PAIDLOSSBILLING.ASPX", "ADJCALC/ADJUSTMENTREVIEW.ASPX");
                AISTabPageList.Add("ADJCALC/LRFPOSTINGDETAILS.ASPX", "ADJCALC/ADJUSTMENTREVIEW.ASPX");
                AISTabPageList.Add("ADJCALC/ADJUSTPROCESSINGCHKLST.ASPX", "ADJCALC/ADJUSTMENTREVIEW.ASPX");
                //AISTabPageList.Add("ADJCALC/INVOICINGDASHBOARD.ASPX", "ADJCALC/ADJUSTMENTREVIEW.ASPX");
                AISTabPageList.Add("ADJCALC/ADJUSTMENTNUMBERTEXTMAINTENANCE.ASPX", "ADJCALC/ADJUSTMENTREVIEW.ASPX");
                ///-----------------------------------------------------------------------------------///
                ///Added By:-Venkata R Kolimi
                ///Purpouse:-As Part of Bu Broker Review Work Order
                ///Created Date:-09/08/2009
                ///Modified By:
                ///Modified Date:
                ///Files Used:NULL(In order to have access security for this newly added Tab we need to add this
                ///here
                ///-----------------------------------------------------------------------------------///
                AISTabPageList.Add("ADJCALC/BUBROKERREVIEW.ASPX", "ADJCALC/ADJUSTMENTREVIEW.ASPX");
                AISTabPageList.Add("ADJCALC/SURCHARGEASSESMENTREVIEW.ASPX", "ADJCALC/ADJUSTMENTREVIEW.ASPX");
                AISTabPageList.Add("ADJCALC/SURCHARGEREVIEWCOMMENTS.ASPX", "ADJCALC/ADJUSTMENTREVIEW.ASPX");
                
                Application["AISTABPAGELIST"] = AISTabPageList;
            }
        
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Exception ex = Server.GetLastError();
            if (ex != null)
            {
                HttpContext.Current.Session[WindowName + "RetroException"] = ex.InnerException;
                HttpContext.Current.Response.Redirect("~/AppError/GeneralError.aspx?wID=" + WindowName);
            }
        }

        void Session_Start(object sender, EventArgs e)
        {
            IList<LookupBE> lookupBEs = new List<LookupBE>();
            LookupBS lookupBS = new LookupBS();
            try
            {
                lookupBEs = lookupBS.getLookupData();
                Application["LookUpData"] = lookupBEs;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                Exception ex = new Exception("Database Server is no longer available.\n Please Check the server whether it's down or locked.");
                Session[WindowName + "RetroException"] = ex;
                Response.Redirect("~/AppError/GeneralError.aspx");
            }

            common = new Common(this.GetType());
            string strInfo = "User: " + User.Identity.Name + " logged in the AIS Application";
            common.Logger.Info(strInfo);
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
        }

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

        public void RoleManager_OnGetRoles(object sender, RoleManagerEventArgs e)
        {
            WindowsPrincipal principal = (WindowsPrincipal)e.Context.User;

            AISUser currentUser = new AISUser(principal);

            if (!System.Web.HttpContext.Current.Items.Contains("CurrentUser"))
                System.Web.HttpContext.Current.Items.Add("CurrentUser", currentUser);

        }
        }
    }
