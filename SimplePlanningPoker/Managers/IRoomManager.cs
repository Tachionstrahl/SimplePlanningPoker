using SimplePlanningPoker.Hubs;
using SimplePlanningPoker.Models;

namespace SimplePlanningPoker.Managers
{
    public interface IRoomManager
    {
        (AddRoomResult, string?) CreateRoom();
        Task<Room?> GetRoomAsync(string roomId);
        Room? GetRoomByParticipant(User participant);
        Task<bool> JoinRoomAsync(string roomId, User participant);
    }
}