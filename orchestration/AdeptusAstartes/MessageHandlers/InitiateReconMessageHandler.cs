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
    public class InitiateReconMessageHandler : IHandleMessages<InitiateReconMessage>
    {
        readonly IBus _bus;

        public InitiateReconMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(InitiateReconMessage message)
        {
            await _bus.Send(new ReconInitiatedMessage
            {
                ComplianceOrderID = message.ComplianceOrderID
            });

            // send local to trigger delayed recon complete message
            await _bus.SendLocal(new ReconInitiatedMessage
            {
                ComplianceOrderID = message.ComplianceOrderID
            });
        }
    }
}
