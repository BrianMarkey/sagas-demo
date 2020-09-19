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
    public class ReconInitiatedMessageHandler : IHandleMessages<ReconInitiatedMessage>
    {
        readonly IBus _bus;

        public ReconInitiatedMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(ReconInitiatedMessage message)
        {
            // simulate time for recon to complete
            Thread.Sleep(5000);

            var rand = new Random();

            await _bus.Send(new ReconCompletedMessage
            {
                ComplianceOrderID = message.ComplianceOrderID,
                RecommendedImperialGuardCompanies = rand.Next(1, 15),
                RecommendedTanks = rand.Next(1, 100)
            });
        }
    }
}
