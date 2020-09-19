using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages
{
    public class ImperialGuardReinforcementCreatedMessage
    {
        public Guid ComplianceOrderID { get; set; }
        public string WorldName { get; set; }
        public int Tanks { get; set; }
        public int GuardCompanies { get; set; }
    }
}
