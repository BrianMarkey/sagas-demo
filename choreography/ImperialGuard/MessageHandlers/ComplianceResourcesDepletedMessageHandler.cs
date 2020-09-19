using ImperialGuard.Data;
using Rebus.Bus;
using Rebus.Handlers;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImperialGuard.MessageHandlers
{

    public class ComplianceResourcesDepletedMessageHandler : IHandleMessages<ComplianceResourcesDepletedMessage>
    {
        readonly DB _db;
        readonly IBus _bus;

        public ComplianceResourcesDepletedMessageHandler(DB db, IBus bus)
        {
            _db = db;
            _bus = bus;
        }

        public async Task Handle(ComplianceResourcesDepletedMessage message)
        {
            var complianceOrder = _db.ComplianceOrders.Single(c => c.ID == message.ComplianceID);

            var deployment = new ImperialGuardDeployment
            {
                ComplianceOrderID = message.ComplianceID,
                ID = Guid.NewGuid(),
                ImperialGuardCompanies = message.RequestedImperialGuardCompanies,
                Tanks = message.RequestedTanks,
                Status = ImperialGuardDeploymentStatus.InTransit
            };

            _db.Deployments.Add(deployment);

            await _bus.Publish(new ImperialGuardDeploymentStatusMessage
            {
                ID = deployment.ID,
                ImperialGuardCompanies = deployment.ImperialGuardCompanies,
                Tanks = deployment.Tanks,
                Status = deployment.Status,
                ComplianceOrderID = deployment.ComplianceOrderID
            });

            Console.WriteLine($"Deployment {deployment.ID} is now in transit");
            await _bus.Publish(new RemembrancerReportMessage
            {
                Origin = Shared.Routing.Buses.ImperialGuard,
                Message = $"Deployment {deployment.ID} is now in transit to {complianceOrder.WorldName}"
            });
        }
    }
}
