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
    public class ReconAssignedMessageHandler : IHandleMessages<ReconAssignedMessage>
    {
        readonly IBus _bus;
        readonly DB _db;

        public ReconAssignedMessageHandler(IBus bus, DB db)
        {
            _bus = bus;
            _db = db;
        }

        public async Task Handle(ReconAssignedMessage message)
        {
            // defer the completed message to simulate the passage of time
            Thread.Sleep(5000);

            var rand = new Random();    
            await _bus.Publish(new ReconCompletedMessage
            {
                ComplianceOrderID = message.ComplianceOrderID,
                WorldName = message.WorldName,
                RecommendedImperialGuardCompanies = rand.Next(0, 5),
                RecommendedSpaceMarines = rand.Next(0, 400),
                RecommendedStormbirds = rand.Next(0, 25),
                RecommendedTanks = rand.Next(0, 100),
                RecommendedThunderhawks = rand.Next(0, 100)
            });

            var complianceOrder = _db.ComplianceOrders.Single(o => o.ID == message.ComplianceOrderID);
            var legion = _db.Legions.Single(l => l.Number == complianceOrder.LegionNumber);
            legion.Status = LegionStatus.AwaitingComplianceGoAhead;
            Console.WriteLine($"{legion.Name} completed recon on {complianceOrder.WorldName}");
            await _bus.Publish(new RemembrancerReportMessage
            {
                Origin = Shared.Routing.Buses.AdeptusAstartes,
                Message = $"{legion.Name} completed recon on {complianceOrder.WorldName}"
            });
        }
    }
}
