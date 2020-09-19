using Rebus.Handlers;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeptusAdministratum.MessageHandlers
{
    public class CompianceOrderAcknowledgedMessageHandler : IHandleMessages<ComplianceOrderAcknowledgedMessage>
    {
        public Task Handle(ComplianceOrderAcknowledgedMessage message)
        {
            return Task.CompletedTask;
        }
    }
}
