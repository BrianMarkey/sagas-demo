using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages
{
    public class ComplianceStatusChangeMessage
    {
        public Guid ComplianceOrderID { get; set; }

        public ComplianceStatus Status { get; set; }
    }

    public enum ComplianceStatus
    {
        Ordered,
        Assigned,
        InProgress,
        Complete
    }
}
