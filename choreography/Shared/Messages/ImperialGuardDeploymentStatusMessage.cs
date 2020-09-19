using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages
{
    public class ImperialGuardDeploymentStatusMessage
    {
        public Guid ID { get; set; }
        public int Tanks { get; set; }
        public int ImperialGuardCompanies { get; set; }
        public ImperialGuardDeploymentStatus Status { get; set; }
        public Guid ComplianceOrderID { get; set; }
    }

    public enum ImperialGuardDeploymentStatus
    {
        Requested,
        InTransit,
        Ready,
        Available,
        PersuingCompliance
    }
}
