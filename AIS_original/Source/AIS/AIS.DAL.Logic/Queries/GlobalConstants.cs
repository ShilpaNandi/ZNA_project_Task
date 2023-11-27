using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZurichNA.AIS.DAL.Logic
{
    public static class GlobalConstants
    {
        public struct RESPONSIBILITIES
        {
            public const string DRAFT = "DRAFT";
            public const string ADJQC = "ADJQC";
            public const string RECON = "RECON";
            public const string FINAL_INVOICE = "FINAL_INVOICE";

            public const int intADJQC = 360;
            public const int intLSSADMIN = 367;
            public const int intCOLLECT_REPRST = 364;
            public const int intCFS2 = 366;
            public const int intRECON = 368;

        }
    
    }
    public struct ExternalContactType
    {
        public const string TPA = "TPA";
        public const string INSURED = "INSURED";
        public const string BROKER = "BROKER";
        public const string CAPTIVE = "CAPTIVE";
    }
}
