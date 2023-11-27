using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZurichNA.AIS.CESAR.Interface
{
    class CESARMain
    {
        static void Main(string[] args)
        {
            CESARInterface oCESAR = new CESARInterface();
            oCESAR.WriteFileLogFile("******************************************");
            oCESAR.WriteFileLogFile("CESAR Interface Module has been invoked");
            try
            {
                oCESAR.WriteCESARData();
            }
            catch (Exception ex)
            {
                oCESAR.WriteFileLogFile("An error occured while running the job");
                oCESAR.WriteFileLogFile(ex.Message);
                oCESAR.SendStatusMail(ex.Message, false);
                throw ex;
            }

            oCESAR.WriteFileLogFile("CESAR Interface Module has completed processing. " +
                "This not necessarily an indication of success.");
        }
    }
}
