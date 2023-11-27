namespace ZurichNA.AIS.DAL.LINQ
{
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Data;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq;
    using System.Linq.Expressions;
    using System.ComponentModel;
    using System;
    using ZurichNA.LSP.Framework.DataAccess;


    
    public partial class AISDatabaseLINQDataContext : LinqContext

    {
        partial void OnCreated()
        {
            System.Configuration.AppSettingsReader appSettings =
                new System.Configuration.AppSettingsReader();
            this.CommandTimeout = (int)appSettings.GetValue("CalcCommandTimeOut", typeof(int));
        }
    }

}