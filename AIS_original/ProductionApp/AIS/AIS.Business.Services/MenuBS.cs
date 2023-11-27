using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;

using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.AIS.Business.Logic
{
    public class MenuBS: BusinessServicesBase<ApplMenuBE,MenuDA>
    {
        public MenuBS()
        { 
        }

      


        public DataTable RetrieveMenuitems(string strQuery)
        {
            MenuDA objMenuda = new MenuDA();
            DataTable dtmenu = objMenuda.FillDataTable(strQuery);
            return dtmenu;   
        
        }

        
    }
}
