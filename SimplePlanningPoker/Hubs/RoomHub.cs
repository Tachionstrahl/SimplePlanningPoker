using System;
using Microsoft.AspNetCore.SignalR;
using SimplePlanningPoker.Managers;

namespace SimplePlanningPoker.Hubs
{
	public class RoomHub : Hub
	{
        private readonly IRoomManager roomManager;

        public RoomHub(IRoomManager roomManager)
        {
            this.roomManager = roomManager;
        }
        public async Task Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", name, message + " back.");
        }

        public async Task SendRoomState(string roomId)
        {
            var room = await roomManager.GetRoomAsync(roomId) ?? throw new ArgumentException($"Room with ID {roomId} does not exist.");
            // var state = room.GetState();
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("sendRoomState");
        }
    }
}

