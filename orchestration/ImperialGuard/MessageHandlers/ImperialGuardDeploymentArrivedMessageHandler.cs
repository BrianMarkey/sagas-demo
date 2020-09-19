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
    public class ImperialGuardDeploymentArrivedMessageHandler : IHandleMessages<ImperialGuardDeploymentArrivedMessage>
    {
        readonly IBus _bus;

        public ImperialGuardDeploymentArrivedMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(ImperialGuardDeploymentArrivedMessage message)
        {
            Thread.Sleep(5000);

            await _bus.Send(new ImperialGuardDeploymentEradicatedMessage
            {
                ComplianceOrderID = message.ComplianceOrderID
            });
        }
    }
}
