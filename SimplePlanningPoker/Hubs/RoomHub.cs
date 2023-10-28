using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.SignalR;
using SimplePlanningPoker.Managers;
using SimplePlanningPoker.Models;

namespace SimplePlanningPoker.Hubs
{
    public class RoomHub : Hub
    {
        private readonly IRoomManager roomManager;

        public RoomHub(IRoomManager roomManager)
        {
            this.roomManager = roomManager;
        }

        public async Task JoinGroup(string roomId)
        {
            var user = GetUserFromHttpContext();
            var room = await roomManager.GetRoomAsync(roomId) ?? throw new ArgumentException($"Room with ID {roomId} does not exist.");
            if (!room.ContainsParticipant(user.Id)){
                throw new NotAParticipantException(roomId);
            }
            
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        public async Task LeaveGroup(string roomId)
        {
            await roomManager.LeaveRoomAsync(Context.ConnectionId, GetUserFromHttpContext());
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var success = await roomManager.LeaveRoomAsync(Context.ConnectionId, GetUserFromHttpContext());
            if (!success)
                throw new ArgumentException($"User with ID {Context.ConnectionId} does not exist.");
        }

        public async Task Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", name, message + " back.");
        }

        public async Task SendRoomState(string roomId)
        {
            var room = await roomManager.GetRoomAsync(roomId) ?? throw new ArgumentException($"Room with ID {roomId} does not exist.");
            // Call the broadcastMessage method to update clients.
            await Clients.Group(roomId).SendAsync(RoomHubMessages.SendRoomState, room.State);
        }

        private User GetUserFromHttpContext()
        {
            var user = new User()
            {
                Name = Context.User?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value ?? throw new ArgumentNullException("User has no name"),
                Id = Context.User?.Claims.FirstOrDefault(c => c.Type == "user_id")?.Value ?? throw new ArgumentNullException("User has no ID")
            };

            return user;
        }
    }

    /// <summary>
    /// Contains the names of the messages that can be sent from the <see cref="RoomHub"/>.
    /// </summary>
    public static class RoomHubMessages {    
        /// <summary>
        /// The name of the message that sends the state of the room.
        /// </summary>
        public const string SendRoomState = "sendRoomState";
        
    }
}

