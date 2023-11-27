using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZurichNA.AIS.ZDW.Interface
{
    class ZDWMain
    {
        static void Main(string[] args)
        {
            ZDWInterface objZDW = new ZDWInterface();
            objZDW.WriteFileLogFile("******************************************");
            objZDW.WriteFileLogFile("ZDW Job Started");
            try
            {
                objZDW.ZDWSummaryInvoiceMain();
                
            }
            catch (Exception ex)
            {
                objZDW.WriteFileLogFile("Error while running the ZDW Job");
                objZDW.WriteFileLogFile(ex.Message);
                throw ex;
            }

            
        }
    }
}
