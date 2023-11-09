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
        /// <summary>
        /// Dictionary of the rooms by their id.
        /// </summary>
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

        /// <summary>
        /// Retrieves a room by its ID.
        /// Returns <see cref="null"/>, if the room does not exist.
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public Room? GetRoom(string roomId)
        {
            return rooms.TryGetValue(roomId, out Room? room) ? room : null;
        }

        /// <summary>
        /// Retrieves a room by a participant.
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        public Room? GetRoomByParticipant(User participant)
        {
            return rooms
                    .FirstOrDefault(r => r.Value.ContainsParticipant(participant.ConnectionId))
                    .Value;
        }
    }

}

