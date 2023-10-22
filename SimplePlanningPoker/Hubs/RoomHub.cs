using System;
using Microsoft.AspNetCore.SignalR;

namespace SimplePlanningPoker.Hubs
{
	public class RoomHub : Hub
	{
        public async Task Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", name, message + " back.");
        }
    }
}

