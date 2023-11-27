using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.LINQ;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.LSP.Framework.Business;

namespace ZurichNA.AIS.Business.Logic
{
    public class AppWebPageAuthBS : BusinessServicesBase<ApplMenuBE,AppWebPageAuthDA>
    {
        public AppWebPageAuthBS()
        {
 
        }

        /// Retrieves Authorized Web Pages
        /// </summary>
        /// <param name="rolename">Role Name</param>
        /// <returns>List of ApplMenuBE</returns>
        public IList<ApplMenuBE> RetrieveAuthWebPages(string rolename)
        {
            AppWebPageAuthDA appauthDA = new AppWebPageAuthDA();
            return appauthDA.RetrieveAuthWebPages(rolename);
        }
    }
}
