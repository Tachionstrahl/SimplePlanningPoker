using System.Collections.Concurrent;
using SimplePlanningPoker.Models;
using SimplePlanningPoker.Utils;

namespace SimplePlanningPoker.Managers
{
/// RoomManager implements the IRoomManager interface to provide methods for 
/// creating, retrieving, joining and leaving rooms. It maintains a dictionary 
/// of active Room instances.
    public class RoomManager : IRoomManager
    {
        private readonly ConcurrentDictionary<string, Room> rooms;

        public RoomManager()
        {
            rooms = new ConcurrentDictionary<string, Room>();
        }

        public async Task<Room?> CreateRoomAsync()
        {            
            var roomId = RandomIDGenerator.GenerateRandomID(6);
            Room room = new(roomId);
            return await Task.Run(() => rooms.TryAdd(roomId, room) ? room : null);
        }

        public Task<Room?> GetRoomAsync(string roomId)
        {
            return Task.FromResult(rooms.TryGetValue(roomId, out Room? room) ? room : null);
        }

        public Task<bool> JoinRoomAsync(string roomId, string participantId)
        {
            return Task.Run(() => rooms.TryGetValue(roomId, out Room? room) && room.AddParticipant(participantId, "TODO")); //TODO Name
        }

        public Task<bool> LeaveRoomAsync(string roomId, string participantId)
        {
            return Task.Run(() => rooms.TryGetValue(roomId, out Room? room) && room.RemoveParticipant(participantId));
        }
    }

}

