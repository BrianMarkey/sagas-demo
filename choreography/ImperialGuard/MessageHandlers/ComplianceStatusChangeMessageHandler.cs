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
    public class ComplianceStatusChangeMessageHandler : IHandleMessages<ComplianceStatusChangeMessage>
    {
        readonly DB _db;
        readonly IBus _bus;

        public ComplianceStatusChangeMessageHandler(DB db, IBus bus)
        {
            _db = db;
            _bus = bus;
        }

        public async Task Handle(ComplianceStatusChangeMessage message)
        {
            var complianceOrder = _db.ComplianceOrders.Single(c => c.ID == message.ComplianceOrderID);

            if (message.Status == ComplianceStatus.Complete)
            {
                var deployments = _db.Deployments.Where(d => d.ComplianceOrderID == message.ComplianceOrderID);

                foreach (var deployment in deployments)
                {
                    deployment.Status = ImperialGuardDeploymentStatus.Available;

                    await _bus.Publish(new ImperialGuardDeploymentStatusMessage
                    {
                        ComplianceOrderID = deployment.ComplianceOrderID,
                        ID = deployment.ID,
                        ImperialGuardCompanies = deployment.ImperialGuardCompanies,
                        Status = ImperialGuardDeploymentStatus.Available,
                        Tanks = deployment.Tanks
                    });
                }

                Console.WriteLine($"Deployments for compliance order {message.ComplianceOrderID} are now available");
                await _bus.Publish(new RemembrancerReportMessage
                {
                    Origin = Shared.Routing.Buses.ImperialGuard,
                    Message = $"Deployments for compliance order {message.ComplianceOrderID} are now available"
                });
            }

            else if (message.Status == ComplianceStatus.InProgress)
            {
                var deployments = _db.Deployments.Where(d => d.ComplianceOrderID == message.ComplianceOrderID);

                foreach (var deployment in deployments)
                {
                    deployment.Status = ImperialGuardDeploymentStatus.PersuingCompliance;

                    await _bus.Publish(new ImperialGuardDeploymentStatusMessage
                    {
                        ComplianceOrderID = deployment.ComplianceOrderID,
                        ID = deployment.ID,
                        ImperialGuardCompanies = deployment.ImperialGuardCompanies,
                        Status = ImperialGuardDeploymentStatus.PersuingCompliance,
                        Tanks = deployment.Tanks
                    });
                }

                Console.WriteLine($"Deployments for compliance order {message.ComplianceOrderID} are now persuing compliance.");
                await _bus.Publish(new RemembrancerReportMessage
                {
                    Origin = Shared.Routing.Buses.ImperialGuard,
                    Message = $"Deployments for compliance order {message.ComplianceOrderID} are now persuing compliance on {complianceOrder.WorldName}."
                });
            }
        }
    }
}
