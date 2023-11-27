using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;
using System.Configuration;

namespace ZurichNA.LSP.Framework
{
    /// <summary>
    /// This class holds the common framework items like Logger
    /// </summary>
    public class Common
    {
        protected static ILog mlogger = null;
        protected string mconfigFile = string.Empty;

        public Common(System.Type typ)
        {
            this.Initialize(typ, ConfigurationSettings.AppSettings["LogFileName"]);
        }

        public Common(System.Type typ, string LogFileName)
        {
            this.Initialize(typ, LogFileName);
        }

    
        private void Initialize(System.Type typ, string LogFileName)
        {
            try
            {
                Logger = LogManager.GetLogger(typ);
                configFile = AppDomain.CurrentDomain.BaseDirectory + LogFileName;
                if (File.Exists(configFile))
                {
                    log4net.Config.XmlConfigurator.Configure(new FileInfo(configFile));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ILog Logger
        {
            get
            {
                return mlogger;
            }
            set
            {
                mlogger = value;
            }
        }

        public string configFile
        {
            get
            {
                return mconfigFile;
            }
            set
            {
                mconfigFile = value;
            }

        }

    }
}
