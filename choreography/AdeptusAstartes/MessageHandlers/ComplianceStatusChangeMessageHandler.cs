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
    public class ComplianceStatusChangeMessageHandler : IHandleMessages<ComplianceStatusChangeMessage>
    {
        readonly IBus _bus;
        readonly DB _db;

        public ComplianceStatusChangeMessageHandler(IBus bus, DB db)
        {
            _bus = bus;
            _db = db;
        }

        public async Task Handle(ComplianceStatusChangeMessage message)
        {
            Console.WriteLine($"Compliance order {message.ComplianceOrderID} status changed to {message.Status}");

            var complianceOrder = _db.ComplianceOrders.Single(c => c.ID == message.ComplianceOrderID);

            if (message.Status == ComplianceStatus.InProgress)
            {
                Thread.Sleep(5000);

                var random = new Random();

                await _bus.Publish(new ComplianceResourcesDepletedMessage
                {
                    ComplianceID = message.ComplianceOrderID,
                    RequestedImperialGuardCompanies = random.Next(1, 10),
                    RequestedTanks = random.Next(1, 10)
                });

                await _bus.Publish(new RemembrancerReportMessage
                {
                    Origin = Shared.Routing.Buses.AdeptusAstartes,
                    Message = $"Resources depleted for compliance on {complianceOrder.WorldName}"
                });
            }
        }
    }
}
