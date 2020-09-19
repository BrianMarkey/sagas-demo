using Rebus.Bus;
using Rebus.Handlers;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImperialGuard.MessageHandlers
{
    public class CreateImperialGuardReinforcementMessageHandler : IHandleMessages<CreateImperialGuardReinforcementMessage>
    {
        readonly IBus _bus;

        public CreateImperialGuardReinforcementMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(CreateImperialGuardReinforcementMessage message)
        {
            await _bus.Send(new ImperialGuardReinforcementCreatedMessage
            {
                ComplianceOrderID = message.ComplianceOrderID,
                GuardCompanies = message.GuardCompanies,
                Tanks = message.Tanks,
                WorldName = message.WorldName
            });

            await _bus.SendLocal(new ImperialGuardReinforcementCreatedMessage
            {
                ComplianceOrderID = message.ComplianceOrderID,
                GuardCompanies = message.GuardCompanies,
                Tanks = message.Tanks,
                WorldName = message.WorldName
            });
        }
    }
}
