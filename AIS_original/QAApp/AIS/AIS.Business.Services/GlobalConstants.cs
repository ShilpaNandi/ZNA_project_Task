using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZurichNA.AIS.Business.Logic
{
    public static class GlobalConstants
    {
        public struct LookUpType
        {
            public const string ProgramType = "ProgramType";
            public const string FormTracking = "FormTracking";
            public const string TrackingIssues = "TrackingIssues";
            public const string ProgramStatus = "ProgramStatus";
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
