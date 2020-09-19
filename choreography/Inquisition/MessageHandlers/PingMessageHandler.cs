using Microsoft.AspNetCore.SignalR;
using Rebus.Handlers;
using Shared.Messages;
using System.Threading.Tasks;
using Web.SignalR;

namespace Web.MessageHandlers
{
    public class PingMessageHandler : IHandleMessages<PingMessage>
    {
        readonly IHubContext<ActivityHub> _signalrHub;

        public PingMessageHandler(IHubContext<ActivityHub> signalrHub)
        {
            _signalrHub = signalrHub;
        }

        public async Task Handle(PingMessage message)
        {
            await _signalrHub.Clients.All.SendAsync("ReceiveMessage", message.Message);
        }
    }
}
