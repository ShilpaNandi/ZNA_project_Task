using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Web.Services;
using System.Web.Services.Protocols;
using ZurichNA.LSP.Framework.DataAccess;
using System.Xml.Linq;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.DAL.LINQ;

namespace AISWebService
{
      
    /// <summary>
    /// Summary description for AISWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    [Serializable]
     public class AISInvoiceSearch : System.Web.Services.WebService
    {
       
        [WebMethod]
        /// <summary>
        /// Returns AIS data that match criteria
        /// </summary>
        /// <returns></returns>
        /// <summary>
        
        public string GetKeysData(string AccountName, string ProgramPrdEffDt, string ProgramPrdExpDt, string ValuationDate, string InvoiceNumber, string FinalInvoiceDt, string AccountNumber, string BrokerName, string BU, string AnalystName, string ProgramType, string PolicyNumber) 
        {
            string str = "";
            if (AccountName == string.Empty && ProgramPrdEffDt == string.Empty && ProgramPrdExpDt == string.Empty && ValuationDate == string.Empty && InvoiceNumber == string.Empty && FinalInvoiceDt == string.Empty && AccountNumber == string.Empty && BrokerName == string.Empty && PolicyNumber == string.Empty)
            {
                return str;
            }
            else
            {
                //ArrayList arrResult = new ArrayList();
                //IList<string> docKeyList = new List<string>();
                ZDWSearchDA searchobj = new ZDWSearchDA();
                str = searchobj.getKeysData(AccountName, ProgramPrdEffDt, ProgramPrdExpDt, ValuationDate, InvoiceNumber, FinalInvoiceDt, AccountNumber, BrokerName, BU, AnalystName, ProgramType, PolicyNumber);
                //arrResult.Add(docKeyList);
                // str = arrResult[0].ToString();
                return str;
            }
        }

    }

    
}
