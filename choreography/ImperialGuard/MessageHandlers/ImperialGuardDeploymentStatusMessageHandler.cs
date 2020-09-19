using ImperialGuard.Data;
using Rebus.Bus;
using Rebus.Handlers;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ImperialGuard.MessageHandlers
{
    public class ImperialGuardDeploymentStatusMessageHandler : IHandleMessages<ImperialGuardDeploymentStatusMessage>
    {
        readonly IBus _bus;
        readonly DB _db;

        public ImperialGuardDeploymentStatusMessageHandler(IBus bus, DB db)
        {
            _bus = bus;
            _db = db;
        }

        public async Task Handle(ImperialGuardDeploymentStatusMessage message)
        {
            var complianceOrder = _db.ComplianceOrders.Single(c => c.ID == message.ComplianceOrderID);

            if (message.Status == ImperialGuardDeploymentStatus.InTransit)
            {
                // delay the ready message to simulate time passing
                Thread.Sleep(5000);

                var deployment = _db.Deployments.Single(d => d.ID == message.ID);
                deployment.Status = ImperialGuardDeploymentStatus.Ready;

                await _bus.Publish(new ImperialGuardDeploymentStatusMessage
                {
                    ID = message.ID,
                    ImperialGuardCompanies = message.ImperialGuardCompanies,
                    Tanks = message.Tanks,
                    Status = ImperialGuardDeploymentStatus.Ready,
                    ComplianceOrderID = message.ComplianceOrderID
                });

                Console.WriteLine($"Deployment {message.ID} is now ready");
                await _bus.Publish(new RemembrancerReportMessage
                {
                    Origin = Shared.Routing.Buses.ImperialGuard,
                    Message = $"Deployment {message.ID} is now ready on {complianceOrder.WorldName}"
                });
            }
        }
    }
}
