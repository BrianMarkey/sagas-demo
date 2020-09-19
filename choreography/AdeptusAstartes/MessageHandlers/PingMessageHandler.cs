using Rebus.Handlers;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeptusAstartes.MessageHandlers
{
    public class PingMessageHandler : IHandleMessages<PingMessage>
    {
        public Task Handle(PingMessage message)
        {
            Console.WriteLine($"Received ping message: {message.Message}");

            return Task.CompletedTask;
        }
    }
}
