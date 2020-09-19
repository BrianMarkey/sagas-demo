using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages
{
    public class ComplianceResourcesDepletedMessage
    {
        public Guid ComplianceID { get; set; }

        // Imperial Guard
        public int RequestedTanks { get; set; }
        public int RequestedImperialGuardCompanies { get; set; }
    }
}
