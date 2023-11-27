using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.Business.Logic;

namespace ZurichNA.AIS.WebSite
{
    /// <summary>
    /// Summary description for AutoComplete
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class AutoComplete : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string[] GetCompletionList(string prefixText, int count)
        {
            IList<AccountBE> dtAISDDLSource = (new AccountBS()).getBPNumberDetails(prefixText);
            List<string> txtItems = new List<string>();
            String dbValues;
            foreach (AccountBE acc in dtAISDDLSource)
            {
                
                dbValues = acc.FINC_PTY_ID;
                dbValues = dbValues.ToLower();
                txtItems.Add(dbValues);
            }

            return txtItems.ToArray();
        }
    }
}
