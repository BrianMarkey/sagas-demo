using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages
{
    public class ReconCompletedMessage
    {
        public Guid ComplianceOrderID { get; set; }
        public int RecommendedTanks { get; set; }
        public int RecommendedImperialGuardCompanies { get; set; }
    }
}
