using AdeptusAstartes.Data;
using Rebus.Bus;
using Rebus.Handlers;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeptusAstartes.MessageHandlers
{
    public class AssignLegionMessageHandler : IHandleMessages<AssignLegionMessage>
    {
        readonly DB _db;
        readonly IBus _bus;

        public AssignLegionMessageHandler(DB db, IBus bus)
        {
            _db = db;
            _bus = bus;
        }

        public async Task Handle(AssignLegionMessage message)
        {
            var legionToAssign = _db.Legions
                .OrderBy(x => Guid.NewGuid())
                .FirstOrDefault(l => l.Status == LegionStatus.StandBy);

            legionToAssign.Status = LegionStatus.Assigned;

            Console.WriteLine(message.ComplianceOrderID);

            await _bus.Send(new LegionAssignedMessage
            {
                ComplianceOrderID = message.ComplianceOrderID,
                LegionName = legionToAssign.Name,
                LegionNumber = legionToAssign.Number
            });
        }
    }
}
