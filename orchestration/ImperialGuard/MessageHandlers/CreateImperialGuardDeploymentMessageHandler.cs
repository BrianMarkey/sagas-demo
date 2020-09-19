using Rebus.Bus;
using Rebus.Handlers;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImperialGuard.MessageHandlers
{
    public class CreateImperialGuardDeploymentMessageHandler : IHandleMessages<CreateImperialGuardDeploymentMessage>
    {
        readonly IBus _bus;

        public CreateImperialGuardDeploymentMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(CreateImperialGuardDeploymentMessage message)
        {
            await _bus.Send(new ImperialGuardDeploymentCreatedMessage
            {
                ComplianceOrderID = message.ComplianceOrderID,
                GuardCompanies = message.GuardCompanies,
                Tanks = message.Tanks,
                WorldName = message.WorldName
            });

            await _bus.SendLocal(new ImperialGuardDeploymentCreatedMessage
            {
                ComplianceOrderID = message.ComplianceOrderID,
                GuardCompanies = message.GuardCompanies,
                Tanks = message.Tanks,
                WorldName = message.WorldName
            });
        }
    }
}
