using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace ZurichNA.AIS.ARMIS.Interface
{
    static class ARMISMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            ARMISInterface oARMIS = new ARMISInterface();
            oARMIS.WriteFileLogFile("******************************************");
            oARMIS.WriteFileLogFile("ARMIS Interface Module has been invoked");
            try
            {
                WindowsIdentity current = WindowsIdentity.GetCurrent();
                oARMIS.WriteFileLogFile("Identity: " + current.Name.ToString());
                oARMIS.WriteFileLogFile("Authentication: " + current.AuthenticationType.ToString());
                oARMIS.WriteFileLogFile("isAuthenticated: " + current.IsAuthenticated.ToString());
                oARMIS.WriteFileLogFile("ImpersonationLevel: " + current.ImpersonationLevel.ToString());
                oARMIS.WriteFileLogFile("IsAnonymous: " + current.IsAnonymous.ToString());
                oARMIS.LoadARMISData();
            }
            catch (Exception ex)
            {
                oARMIS.WriteFileLogFile("A serious error occurred");
                oARMIS.WriteFileLogFile(ex.Message);
            }

            oARMIS.WriteFileLogFile("ARMIS Interface Module has completed processing. " +
                "This not necessarily an indication of success.");
        }
    }
}
