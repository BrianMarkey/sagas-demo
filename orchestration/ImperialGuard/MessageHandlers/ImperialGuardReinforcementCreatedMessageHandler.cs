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
    public class ImperialGuardReinforcementCreatedMessageHandler : IHandleMessages<ImperialGuardReinforcementCreatedMessage>
    {
        readonly IBus _bus;

        public ImperialGuardReinforcementCreatedMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(ImperialGuardReinforcementCreatedMessage message)
        {
            Thread.Sleep(5000);

            await _bus.Send(new ImperialGuardReinforcementArrivedMessage { ComplianceOrderID = message.ComplianceOrderID });
        }
    }
}
