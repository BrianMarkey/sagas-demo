using AdeptusAstartes.Data;
using Rebus.Handlers;
using Shared.Messages;
using System;
using System.Threading.Tasks;
using System.Linq;
using Rebus.Bus;

namespace AdeptusAstartes.MessageHandlers
{
    public class ComplianceOrderCommandHandler : IHandleMessages<ComplianceOrderCommand>
    {
        readonly DB _db;
        readonly IBus _bus;

        public ComplianceOrderCommandHandler(DB db, IBus bus)
        {
            _db = db;
            _bus = bus;
        }

        public async Task Handle(ComplianceOrderCommand message)
        {
            await _bus.Send(new ComplianceOrderAcknowledgedMessage { ComplianceOrderID = message.ID });
            Console.WriteLine($"Acknowledged compliance order world: {message.WorldName} id {message.ID}");
            await _bus.Publish(new RemembrancerReportMessage
            {
                Origin = Shared.Routing.Buses.AdeptusAstartes,
                Message = $"Acknowledged compliance order world: {message.WorldName} id {message.ID}"
            });

            // assign a legion to recon
            var legion = _db.Legions.FirstOrDefault(l => l.Status == LegionStatus.StandBy);
            _db.ComplianceOrders.Add(new ComplianceOrder
            {
                ID = message.ID,
                WorldName = message.WorldName,
                Status = ComplianceStatus.Ordered,
                LegionNumber = legion.Number
            });
            legion.Status = LegionStatus.ConductingRecon;
            await _bus.Publish(new ReconAssignedMessage
            {
                ComplianceOrderID = message.ID,
                LegionNumber = legion.Number,
                WorldName = message.WorldName
            });
            Console.WriteLine($"Assigned legion {legion.Name} to recon on {message.WorldName}");
            await _bus.Publish(new RemembrancerReportMessage
            {
                Origin = Shared.Routing.Buses.AdeptusAstartes,
                Message = $"Assigned legion {legion.Name} to recon on {message.WorldName}"
            });
        }
    }
}
