using AdeptusAstartes.Data;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdeptusAstartes.SignalR
{
    public class AstartesHub : Hub
    {
        readonly DB _db;

        public AstartesHub(DB db)
        {
            _db = db;
        }

        public async override Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("all-data", new
            {
                Legions = _db.Legions.ToList(),
                ComplianceOrders = _db.ComplianceOrders.ToList()
            });
        }
    }
}
