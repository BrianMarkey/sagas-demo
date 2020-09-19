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
    public class ImperialGuardDeploymentCreatedMessageHandler : IHandleMessages<ImperialGuardDeploymentCreatedMessage>
    {
        readonly IBus _bus;

        public ImperialGuardDeploymentCreatedMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(ImperialGuardDeploymentCreatedMessage message)
        {
            Thread.Sleep(5000);

            await _bus.Send(new ImperialGuardDeploymentArrivedMessage { ComplianceOrderID = message.ComplianceOrderID });

            await _bus.SendLocal(new ImperialGuardDeploymentArrivedMessage { ComplianceOrderID = message.ComplianceOrderID });
        }
    }
}
