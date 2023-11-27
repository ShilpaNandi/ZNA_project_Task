using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIS.ZNAWNDICInterface
{
    class ZNAWMain
    {
        static void Main(string[] args)
        {
            ZNAWInterface oZNAW = new ZNAWInterface();
            oZNAW.WriteFileLogFile("******************************************");
            oZNAW.WriteFileLogFile("ZNAW Interface Module has been invoked");
            int returnValue = 0;
            try
            {
                returnValue = oZNAW.WriteZNAWData() == true ? 0 : -1;
            }
            catch (Exception ex)
            {
                oZNAW.WriteFileLogFile("A serious error occurred");
                oZNAW.WriteFileLogFile(ex.Message);
                returnValue = -1;
            }

            oZNAW.WriteFileLogFile("ZNAW Interface Module has completed processing. " +
                "This not necessarily an indication of success.");
            Environment.Exit(returnValue);
        }
    }
}
