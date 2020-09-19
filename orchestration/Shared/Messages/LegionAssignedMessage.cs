using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages
{
    public class LegionAssignedMessage
    {
        public Guid ComplianceOrderID { get; set; }
        public int LegionNumber { get; set; }
        public string LegionName { get; set; }
    }
}
