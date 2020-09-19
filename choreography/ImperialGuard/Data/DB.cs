using Shared.Messages;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImperialGuard.Data
{
    public class DB
    {
        public ConcurrentBag<ImperialGuardDeployment> Deployments { get; set; } = new ConcurrentBag<ImperialGuardDeployment>();
        public ConcurrentBag<ComplianceOrder> ComplianceOrders { get; set; } = new ConcurrentBag<ComplianceOrder>();
    }

    public class ImperialGuardDeployment
    {
        public Guid ID { get; set; }
        public int ImperialGuardCompanies { get; set; }
        public int Tanks { get; set; }
        public ImperialGuardDeploymentStatus Status { get; set; }
        public Guid ComplianceOrderID { get; set; }
    }

    public class ComplianceOrder
    {
        public Guid ID { get; set; }
        public string WorldName { get; set; }
    }
}
