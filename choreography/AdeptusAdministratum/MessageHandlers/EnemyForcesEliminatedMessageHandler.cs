using Rebus.Bus;
using Rebus.Handlers;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AdeptusAdministratum.MessageHandlers
{
    public class EnemyForcesEliminatedMessageHandler : IHandleMessages<EnemyForcesEliminatedMessage>
    {
        readonly IBus _bus;

        public EnemyForcesEliminatedMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(EnemyForcesEliminatedMessage message)
        {
            Thread.Sleep(5000);

            await _bus.Publish(new ComplianceCertifiedMessage
            {
                ComplianceOrderID = message.ComplianceOrderID
            });

            await _bus.Publish(new RemembrancerReportMessage
            {
                Origin = Shared.Routing.Buses.AdeptusAdministratum,
                Message = $"By the power vested in me by the High Lords of Terra, I declare that world of {message.WorldName} has been brought to Imperial Compliance, and has been rightfully returned to the Imperium of man. The Emperor protects."
            });
        }
    }
}
