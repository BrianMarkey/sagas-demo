using Microsoft.AspNetCore.SignalR;
using Rebus.Handlers;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.SignalR;

namespace Inquisition.MessageHandlers
{
    public class RemembrancerReportMessageHandler : IHandleMessages<RemembrancerReportMessage>
    {
        readonly IHubContext<ActivityHub> _activityHub;

        public RemembrancerReportMessageHandler(IHubContext<ActivityHub> activityHub)
        {
            _activityHub = activityHub;
        }

        public async Task Handle(RemembrancerReportMessage message)
        {
            string output = $"[{message.Origin}] {message.Message}";

            Console.WriteLine(output);

            await _activityHub.Clients.All.SendAsync("data-feed", output);
        }
    }
}
