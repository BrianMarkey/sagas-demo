using AdeptusAstartes.Data;
using Rebus.Bus;
using Rebus.Handlers;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AdeptusAstartes.MessageHandlers
{
    public class ImperialGuardDeploymentStatusMessageHandler : IHandleMessages<ImperialGuardDeploymentStatusMessage>
    {
        readonly DB _db;
        readonly IBus _bus;

        public ImperialGuardDeploymentStatusMessageHandler(DB db, IBus bus)
        {
            _db = db;
            _bus = bus;
        }

        public async Task Handle(ImperialGuardDeploymentStatusMessage message)
        {
            var deployment = _db.ImperialGuardDeployments.FirstOrDefault(d => d.ID == message.ID);

            if (deployment == null)
            {
                _db.ImperialGuardDeployments.Add(new ImperialGuardDeployment
                {
                    ID = message.ID,
                    ImperialGuardCompanies = message.ImperialGuardCompanies,
                    Status = message.Status,
                    Tanks = message.Tanks
                });
            }
            else
            {
                deployment.Status = message.Status;
            }

            if (message.Status == ImperialGuardDeploymentStatus.Ready)
            {
                var complianceOrder = _db.ComplianceOrders.Single(c => c.ID == message.ComplianceOrderID);

                // this is a reinforment. no need to update the status of the compliance
                if (complianceOrder.Status == ComplianceStatus.InProgress)
                {
                    await handleReinforcements(message);

                    return;
                }

                var legion = _db.Legions.Single(l => l.Number == complianceOrder.LegionNumber);

                legion.Status = LegionStatus.PersuingCompliance;

                complianceOrder.Status = ComplianceStatus.InProgress;

                await _bus.Publish(new ComplianceStatusChangeMessage
                {
                    ComplianceOrderID = message.ComplianceOrderID,
                    Status = ComplianceStatus.InProgress
                });

                Console.WriteLine($"Compliance is in progress on {complianceOrder.WorldName}");
                await _bus.Publish(new RemembrancerReportMessage
                {
                    Origin = Shared.Routing.Buses.AdeptusAstartes,
                    Message = $"Compliance in progress {message.ComplianceOrderID}"
                });
            }
        }

        private async Task handleReinforcements(ImperialGuardDeploymentStatusMessage message)
        {
            // delay the message to simulate time passing
            Thread.Sleep(5000);

            var complianceOrder = _db.ComplianceOrders.Single(c => c.ID == message.ComplianceOrderID);

            await _bus.Publish(new EnemyForcesEliminatedMessage
            {
                WorldName = complianceOrder.WorldName,
                ComplianceOrderID = message.ComplianceOrderID
            });

            await _bus.Publish(new RemembrancerReportMessage
            {
                Origin = Shared.Routing.Buses.AdeptusAstartes,
                Message = $"Enemy forces have been eliminated on {complianceOrder.WorldName} in the Name Of The Emperor, beloved by all."
            });
        }
    }
}
