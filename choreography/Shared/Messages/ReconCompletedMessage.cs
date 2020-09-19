using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages
{
    public class ReconCompletedMessage
    {
        public Guid ComplianceOrderID { get; set; }
        public string WorldName { get; set; }

        // Imperial Guard
        public int RecommendedTanks { get; set; }
        public int RecommendedImperialGuardCompanies { get; set; }

        // Space Marines
        public int RecommendedSpaceMarines { get; set; }
        public int RecommendedStormbirds { get; set; }
        public int RecommendedThunderhawks { get; set; }
    }
}
