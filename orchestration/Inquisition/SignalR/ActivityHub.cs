using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.SignalR
{
    public class ActivityHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("data-feed", message);
        }
    }
}
