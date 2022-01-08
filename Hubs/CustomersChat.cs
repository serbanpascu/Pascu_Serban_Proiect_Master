using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Pascu_Serban_Proiect.Hubs
{
    [Authorize]
    public class CustomersChat : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", Context.User.Identity.Name, message + " at " + DateTime.Now.ToLongDateString());
        }
    }
}