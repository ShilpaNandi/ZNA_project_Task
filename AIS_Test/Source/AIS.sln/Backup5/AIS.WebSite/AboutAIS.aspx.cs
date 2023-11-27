using System;
using System.Collections;
using System.Collections.Generic;
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

namespace ZurichNA.AIS.WebSite
{
    public partial class AboutAIS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                BindWebConfig();
                BindAriesConfig();
                BindCesarConfig();
                BindArmisConfig();
                BindZDWConfig();
            }

        }

        void BindWebConfig()
        {
            // Generation Application Configuration Information
            IEnumerator conEnum = ConfigurationManager.AppSettings.GetEnumerator();
            List<ConfigSettings> settings = new List<ConfigSettings>();
            int i = 0;
            while (conEnum.MoveNext())
            {
                ConfigSettings newSettings = new ConfigSettings();
                newSettings.Key = ConfigurationManager.AppSettings.AllKeys[i].ToString();
                newSettings.Value = ConfigurationManager.AppSettings[newSettings.Key].ToString();
                settings.Add(newSettings);
                i = i + 1;
            }
            // Connection Strings
            conEnum = ConfigurationManager.ConnectionStrings.GetEnumerator();
            i = 0;
            while (conEnum.MoveNext())
            {
                ConfigSettings newSettings = new ConfigSettings();
                newSettings.Key = ConfigurationManager.ConnectionStrings[i].Name;
                //newSettings.Value = ConfigurationManager.ConnectionStrings[i].ConnectionString.Replace("ta2RayuwraswaC9e95af", "***********");
                newSettings.Value = RemovePassword(ConfigurationManager.ConnectionStrings[i].ConnectionString);
                settings.Add(newSettings);
                i = i + 1;
            }

            //load audit history
            this.lvConfigurations.DataSource = settings;
            this.lvConfigurations.DataBind();
        }

        void BindAriesConfig()
        {
            // Generation Application Configuration Information
            string strRoot = Server.MapPath("~");
            strRoot = strRoot.TrimEnd('\\');
            strRoot = strRoot.Remove(strRoot.LastIndexOf('\\') + 1);
            Configuration configAries = ConfigurationManager.OpenExeConfiguration(strRoot + "ConsoleApplications\\ZurichNA.AIS.ARiES.Interface.exe");
            //System.Configuration.ConnectionStringSettings connStringAries;



            IEnumerator conEnum = configAries.AppSettings.Settings.GetEnumerator();
            List<ConfigSettings> settings = new List<ConfigSettings>();

            int i = 0;
            while (conEnum.MoveNext())
            {
                ConfigSettings newSettings = new ConfigSettings();
                newSettings.Key = configAries.AppSettings.Settings.AllKeys[i].ToString();
                newSettings.Value = configAries.AppSettings.Settings[newSettings.Key].Value.ToString();
                settings.Add(newSettings);
                i = i + 1;
            }
            // Connection Strings
            conEnum = configAries.ConnectionStrings.ConnectionStrings.GetEnumerator();
            i = 0;
            while (conEnum.MoveNext())
            {
                ConfigSettings newSettings = new ConfigSettings();
                newSettings.Key = configAries.ConnectionStrings.ConnectionStrings[i].Name;
                //newSettings.Value = configAries.ConnectionStrings.ConnectionStrings[i].ConnectionString.Replace("ta2RayuwraswaC9e95af", "***********");
                newSettings.Value = RemovePassword(configAries.ConnectionStrings.ConnectionStrings[i].ConnectionString);
                settings.Add(newSettings);
                i = i + 1;
            }

            //load audit history
            this.lvConfigurationsAries.DataSource = settings;
            this.lvConfigurationsAries.DataBind();
        }

        void BindCesarConfig()
        {
            // Generation Application Configuration Information
            string strRoot = Server.MapPath("~");
            strRoot = strRoot.TrimEnd('\\');
            strRoot = strRoot.Remove(strRoot.LastIndexOf('\\') + 1);
            Configuration configAries = ConfigurationManager.OpenExeConfiguration(strRoot + "ConsoleApplications\\AIS.CESAR.Interface.exe");
            //System.Configuration.ConnectionStringSettings connStringAries;



            IEnumerator conEnum = configAries.AppSettings.Settings.GetEnumerator();
            List<ConfigSettings> settings = new List<ConfigSettings>();

            int i = 0;
            while (conEnum.MoveNext())
            {
                ConfigSettings newSettings = new ConfigSettings();
                newSettings.Key = configAries.AppSettings.Settings.AllKeys[i].ToString();
                newSettings.Value = configAries.AppSettings.Settings[newSettings.Key].Value.ToString();
                settings.Add(newSettings);
                i = i + 1;
            }
            // Connection Strings
            conEnum = configAries.ConnectionStrings.ConnectionStrings.GetEnumerator();
            i = 0;
            while (conEnum.MoveNext())
            {
                ConfigSettings newSettings = new ConfigSettings();
                newSettings.Key = configAries.ConnectionStrings.ConnectionStrings[i].Name;
                //newSettings.Value = configAries.ConnectionStrings.ConnectionStrings[i].ConnectionString.Replace("ta2RayuwraswaC9e95af", "***********");
                newSettings.Value = RemovePassword(configAries.ConnectionStrings.ConnectionStrings[i].ConnectionString);
                settings.Add(newSettings);
                i = i + 1;
            }

            //load audit history
            this.lvConfigurationsCesar.DataSource = settings;
            this.lvConfigurationsCesar.DataBind();
        }

        void BindArmisConfig()
        {
            // Generation Application Configuration Information
            string strRoot = Server.MapPath("~");
            strRoot = strRoot.TrimEnd('\\');
            strRoot = strRoot.Remove(strRoot.LastIndexOf('\\') + 1);
            Configuration configAries = ConfigurationManager.OpenExeConfiguration(strRoot + "ConsoleApplications\\ZurichNA.AIS.ARMIS.Interface.exe");
            //System.Configuration.ConnectionStringSettings connStringAries;



            IEnumerator conEnum = configAries.AppSettings.Settings.GetEnumerator();
            List<ConfigSettings> settings = new List<ConfigSettings>();

            int i = 0;
            while (conEnum.MoveNext())
            {
                ConfigSettings newSettings = new ConfigSettings();
                newSettings.Key = configAries.AppSettings.Settings.AllKeys[i].ToString();
                newSettings.Value = configAries.AppSettings.Settings[newSettings.Key].Value.ToString();
                settings.Add(newSettings);
                i = i + 1;
            }
            // Connection Strings
            conEnum = configAries.ConnectionStrings.ConnectionStrings.GetEnumerator();
            i = 0;
            while (conEnum.MoveNext())
            {
                ConfigSettings newSettings = new ConfigSettings();
                newSettings.Key = configAries.ConnectionStrings.ConnectionStrings[i].Name;
                //newSettings.Value = configAries.ConnectionStrings.ConnectionStrings[i].ConnectionString.Replace("ta2RayuwraswaC9e95af", "***********");
                newSettings.Value = RemovePassword(configAries.ConnectionStrings.ConnectionStrings[i].ConnectionString);
                settings.Add(newSettings);
                i = i + 1;
            }

            //load audit history
            this.lvConfigurationsArmis.DataSource = settings;
            this.lvConfigurationsArmis.DataBind();
        }

        void BindZDWConfig()
        {
            // Generation Application Configuration Information
            string strRoot = Server.MapPath("~");
            strRoot = strRoot.TrimEnd('\\');
            strRoot = strRoot.Remove(strRoot.LastIndexOf('\\') + 1);
            Configuration configAries = ConfigurationManager.OpenExeConfiguration(strRoot + "ConsoleApplications\\ZDW\\AIS.ZDW.Interface.exe");
            //System.Configuration.ConnectionStringSettings connStringAries;



            IEnumerator conEnum = configAries.AppSettings.Settings.GetEnumerator();
            List<ConfigSettings> settings = new List<ConfigSettings>();

            int i = 0;
            while (conEnum.MoveNext())
            {
                ConfigSettings newSettings = new ConfigSettings();
                newSettings.Key = configAries.AppSettings.Settings.AllKeys[i].ToString();
                newSettings.Value = configAries.AppSettings.Settings[newSettings.Key].Value.ToString();
                settings.Add(newSettings);
                i = i + 1;
            }
            // Connection Strings
            conEnum = configAries.ConnectionStrings.ConnectionStrings.GetEnumerator();
            i = 0;
            while (conEnum.MoveNext())
            {
                ConfigSettings newSettings = new ConfigSettings();
                newSettings.Key = configAries.ConnectionStrings.ConnectionStrings[i].Name;
                //newSettings.Value = configAries.ConnectionStrings.ConnectionStrings[i].ConnectionString.Replace("ta2RayuwraswaC9e95af", "***********");
                newSettings.Value = RemovePassword(configAries.ConnectionStrings.ConnectionStrings[i].ConnectionString);
                settings.Add(newSettings);
                i = i + 1;
            }

            //load audit history
            this.lvConfigurationsZDW.DataSource = settings;
            this.lvConfigurationsZDW.DataBind();
        }

        private string RemovePassword(string connectionString)
        {
            string conString = connectionString;
            if (conString.Contains("Password"))
            {               
                conString=conString.Remove(conString.IndexOf("Password"));
            }
            
            return conString;
        }

        private class ConfigSettings
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}