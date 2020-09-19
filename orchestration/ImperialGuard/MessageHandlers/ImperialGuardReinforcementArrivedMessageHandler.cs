using Rebus.Bus;
using Rebus.Handlers;
using Shared.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace ImperialGuard.MessageHandlers
{
    public class ImperialGuardReinforcementArrivedMessageHandler : IHandleMessages<ImperialGuardDeploymentArrivedMessage>
    {
        readonly IBus _bus;

        public ImperialGuardReinforcementArrivedMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(ImperialGuardDeploymentArrivedMessage message)
        {
            Thread.Sleep(5000);

            await _bus.Send(new EnemyForcesEradicatedMessage
            {
                ComplianceOrderID = message.ComplianceOrderID
            });
        }
    }
}
