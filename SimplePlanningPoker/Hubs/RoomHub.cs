using Microsoft.AspNetCore.SignalR;
using SimplePlanningPoker.Managers;
using SimplePlanningPoker.Models;
namespace SimplePlanningPoker.Hubs
{
    /// <summary>
    /// RoomHub handles real-time communication between server and clients for a room. 
    /// </summary>
    public class RoomHub : Hub<IRoomHub>
    {
        private readonly IRoomManager roomManager;
        private readonly IUserManager userManager;

        public RoomHub(IRoomManager roomManager, IUserManager userManager)
        {
            this.roomManager = roomManager;
            this.userManager = userManager;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await RemoveFromAllRooms();
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Joins the user to the room with the given ID.
        /// </summary>
        /// <param name="roomId">ID of the room to join</param>
        /// <param name="username">The name of the joining user</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task Join(string roomId, string username)
        {
            if (string.IsNullOrEmpty(roomId))
                throw new ArgumentNullException(nameof(roomId));
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            var user = CreateAndAddUser(username);
            var room = await roomManager.GetRoomAsync(roomId) ?? throw new ArgumentException($"Room with ID {roomId} does not exist.");
            room.AddParticipant(user);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await SendRoomState(room.RoomId);
        }

        /// <summary>
        /// Sends the current state of the room to all clients in the group.
        /// </summary>
        /// <param name="roomId">The ID of the room</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task SendRoomState(string roomId)
        {
            if (string.IsNullOrEmpty(roomId))
                throw new ArgumentNullException(nameof(roomId));

            var room = await roomManager.GetRoomAsync(roomId) ?? throw new ArgumentException($"Room with ID {roomId} does not exist.");

            await Clients.Group(roomId).SendRoomState(room.State);
        }


        /// <summary>
        /// Allows a participant to submit an estimate for the current story. 
        /// Gets the current user and room instance. 
        /// Calls the room's Estimate method to record the estimate.
        /// Sends an updated room state to all participants after estimating.
        /// </summary>
        public async Task Estimate(string estimate)
        {
            if (string.IsNullOrEmpty(estimate))
            {
                throw new ArgumentException("estimate is required");
            }
            var user = GetUser();
            var room = GetRoom();

            room?.Estimate(user.ConnectionId, estimate);
            await SendRoomState(room!.RoomId);
        }

        /// <summary>
        /// Reveals all estimations
        /// </summary>
        /// <returns></returns>
        public async Task Reveal()
        {
            var user = GetUser();
            var room = GetRoom();
            if (room != null)
            {
                room.Reveal();
                await SendRoomState(room.RoomId);
            }
        }
        /// <summary>
        /// Resets the room
        /// </summary>
        /// <returns></returns>
        public async Task Reset()
        {
            var user = GetUser();
            var room = GetRoom();
            if (room != null)
            {
                room.Reset();
                await SendRoomState(room.RoomId);
                await Clients.Group(room.RoomId).Reset();
            }
        }

        private User GetUser()
        {
            var success = userManager.TryGetUser(Context.ConnectionId, out var user);
            if (!success)
                throw new ParticipantNotFoundException(Context.ConnectionId);
            return user!;
        }

        private User CreateAndAddUser(string username)
        {
            var connectionId = Context.ConnectionId;
            var user = new User(connectionId, username, null);
            userManager.TryAddUser(user);
            return user;
        }

        private async Task RemoveFromAllRooms()
        {
            var user = GetUser();
            var room = GetRoom();
            if (room != null)
            {
                room.RemoveParticipant(user);
                await SendRoomState(room.RoomId);
            }

        }

        private Room? GetRoom()
        {
            var user = GetUser();
            return roomManager.GetRoomByParticipant(user);

        }
    }
}

