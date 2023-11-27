using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIS.NDICExtract
{
    class NDICExtractMain
    {
        static void Main(string[] args)
        {
            NDICExtractInterface oNDICExtract = new NDICExtractInterface();
            oNDICExtract.WriteFileLogFile("******************************************");
            oNDICExtract.WriteFileLogFile("AIS NDIC Extract Interface Module has been invoked");
            int returnValue = 0;
            try
            {
                bool result = oNDICExtract.GetNDICExtract();

                if (result == true)
                {
                    oNDICExtract.SendStatusMail("", result);
                    oNDICExtract.WriteFileLogFile("Job Completed...");
                    returnValue = 0;
                }
                else
                {
                    oNDICExtract.SendStatusMail("", result);
                    oNDICExtract.WriteFileLogFile("Job failed...");
                    returnValue = -1;
                }


            }
            catch (Exception ex)
            {
                oNDICExtract.WriteFileLogFile("An error occurred");
                oNDICExtract.WriteFileLogFile(ex.Message);
                returnValue = -1;
            }

            oNDICExtract.WriteFileLogFile("AIS NDIC Extract Interface Module has completed processing. " +
                "This not necessarily an indication of success.");
            Environment.Exit(returnValue);
        }
    }
}
