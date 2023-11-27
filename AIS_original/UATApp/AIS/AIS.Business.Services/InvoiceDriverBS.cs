using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ZurichNA.AIS.Business.Entities;
using ZurichNA.AIS.DAL.Logic;
using ZurichNA.AIS.ExceptionHandling;
using ZurichNA.LSP.Framework.Business;
using ZurichNA.LSP.Framework.DataAccess;
using ZurichNA.AIS.DAL.LINQ;

namespace ZurichNA.AIS.Business.Logic
{
    public class InvoiceDriverBS
    {
        public InvoiceDriverBS()
        { }
        public bool UpdatePremAdjutmentDraftInvoiceData(AISDatabaseLINQDataContext objDC, int intAdjNo, string strInvoiceNo, int intUserID)
        {
            InvoiceDriverDA objInvoiceDA = new InvoiceDriverDA();
            return (objInvoiceDA.UpdatePremAdjutmentDraftInvoiceData(objDC, intAdjNo, strInvoiceNo, intUserID));
            

        }
        public bool InsertPremAdjutmentStatusData(AISDatabaseLINQDataContext objDC, int intAdjNo, int intInvoiceStatus,string strComments, int intUserID)
        {
            InvoiceDriverDA objInvoiceDA = new InvoiceDriverDA();
            return (objInvoiceDA.InsertPremAdjutmentStatusData(objDC, intAdjNo, intInvoiceStatus,strComments, intUserID));
          

        }
        public bool UpdateDraftZDWKeys(AISDatabaseLINQDataContext objDC, int intAdjNo, string strZDWKey, char cKeyType, int intUserID)
        {
            InvoiceDriverDA objInvoiceDA = new InvoiceDriverDA();
            return(objInvoiceDA.UpdateDraftZDWKeys(objDC, intAdjNo, strZDWKey,cKeyType,intUserID));
           

        }
        public bool UpdatePremAdjutmentFinalInvoiceData(AISDatabaseLINQDataContext objDC, int intAdjNo, string strInvoiceNo, int intUserID)
        {
            InvoiceDriverDA objInvoiceDA = new InvoiceDriverDA();
            return (objInvoiceDA.UpdatePremAdjutmentFinalInvoiceData(objDC, intAdjNo, strInvoiceNo, intUserID));
          

        }
        public bool UpdateFinalZDWKeys(AISDatabaseLINQDataContext objDC, int intAdjNo, string strZDWKey, char cKeyType, int intUserID)
        {
            InvoiceDriverDA objInvoiceDA = new InvoiceDriverDA();
           return(objInvoiceDA.UpdateFinalZDWKeys(objDC, intAdjNo, strZDWKey,cKeyType, intUserID));
           

        }
        public string getZDWKey(AISDatabaseLINQDataContext objDC,int intAdjNo, char cKeyType, int IFlag)
        {
            InvoiceDriverDA objInvoiceDA = new InvoiceDriverDA();
            return objInvoiceDA.getZDWKey(objDC, intAdjNo, cKeyType, IFlag);
        }
    }
}
