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
    public class ReconCompletedMessageHandler : IHandleMessages<ReconCompletedMessage>
    {
        readonly IBus _bus;
        readonly DB _db;

        public ReconCompletedMessageHandler(IBus bus, DB db)
        {
            _bus = bus;
            _db = db;
        }

        public async Task Handle(ReconCompletedMessage message)
        {
            Console.WriteLine($"Received recon complete message. {message.RecommendedImperialGuardCompanies} companies {message.RecommendedTanks} tanks");

            var deployment = new ImperialGuardDeployment
            {
                ID = Guid.NewGuid(),
                ComplianceOrderID = message.ComplianceOrderID,
                ImperialGuardCompanies = message.RecommendedImperialGuardCompanies,
                Status = ImperialGuardDeploymentStatus.InTransit,
                Tanks = message.RecommendedTanks
            };

            _db.Deployments.Add(deployment);
            _db.ComplianceOrders.Add(new ComplianceOrder
            {
                ID = message.ComplianceOrderID,
                WorldName = message.WorldName
            });

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
                Message = $"Deployment {deployment.ID} is now in transit to {message.WorldName}"
            });
        }
    }
}
