using System;
using System.Collections.Generic;
using System.Linq;

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
            oARMIS.LoadARMISData();
        }
    }
}
