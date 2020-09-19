using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages
{
    public class ReconAssignedMessage
    {
        public Guid ComplianceOrderID { get; set; }
        public int LegionNumber { get; set; }
        public string WorldName { get; set; }
    }
}
