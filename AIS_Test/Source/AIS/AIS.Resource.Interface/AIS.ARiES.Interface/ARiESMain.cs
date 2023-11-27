using System;
using System.Collections.Generic;


namespace ZurichNA.AIS.ARiES.Interface
{
    static class ARiESMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            ARiESInterface oARiES = new ARiESInterface();
            oARiES.WriteFileLogFile("******************************************");
            oARiES.WriteFileLogFile("ARIES Interface Module has been invoked");
            try
            {
                oARiES.WriteARiESData();
            }
            catch (Exception ex)
            {
                oARiES.WriteFileLogFile("A serious error occurred");
                oARiES.WriteFileLogFile(ex.Message);
            }

            oARiES.WriteFileLogFile("ARIES Interface Module has completed processing. " +
                "This not necessarily an indication of success.");
        }
    }
}
