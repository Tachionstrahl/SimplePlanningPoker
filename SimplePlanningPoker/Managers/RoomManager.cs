using System.Collections.Concurrent;
using SimplePlanningPoker.Models;
using SimplePlanningPoker.Utils;

namespace SimplePlanningPoker.Managers
{
    /// <summary>
    /// A manager for rooms.
    /// This class is defined as thread-safe.
    /// </summary>
    public class RoomManager : IRoomManager
    {
        private readonly ConcurrentDictionary<string, Room> rooms;

        public RoomManager()
        {
            rooms = new ConcurrentDictionary<string, Room>();
        }

        /// <summary>
        /// Creates a new room. Returns <see cref="true"/>, if successful.
        /// </summary>
        /// <returns></returns>
        public (AddRoomResult, string?) CreateRoom()
        {
            var roomId = RandomIDGenerator.GenerateRandomID(6);
            Room room = new(roomId);
            var success = rooms.TryAdd(roomId, room);
            return success ? (AddRoomResult.Success, room.RoomId) : (AddRoomResult.Failed, null);
        }

        public bool DoesRoomExist(string roomId)
        {
            return rooms.ContainsKey(roomId);
        }

        /// <summary>
        /// Retrieves a room by its ID.
        /// Returns <see cref="null"/>, if the room does not exist.
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public async Task<Room?> GetRoomAsync(string roomId)
        {
            return await Task.FromResult(rooms.TryGetValue(roomId, out Room? room) ? room : null);
        }

        public Task<Room?> GetRoomByParticipant(User participant)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a participant to a room. Returns <see cref="true"/>, if successful.
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        public Task<bool> JoinRoomAsync(string roomId, User participant)
        {
            return Task.Run(() => rooms.TryGetValue(roomId, out Room? room) && room.AddParticipant(participant));
        }
        
        /// <summary>
        /// Removes a participant from a room. Returns <see cref="true"/>, if successful.
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        public Task<bool> LeaveRoomAsync(string roomId, User participant)
        {
            return Task.Run(() => rooms.TryGetValue(roomId, out Room? room) && room.RemoveParticipant(participant));
        }
    }

}

