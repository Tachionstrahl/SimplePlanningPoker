using SimplePlanningPoker.Models;

namespace SimplePlanningPoker.Managers
{
    public interface IRoomManager
    {
        Task<Room?> CreateRoomAsync(string roomId);
        Task<Room?> GetRoomAsync(string roomId);
        Task<bool> JoinRoomAsync(string roomId, string participantId);
        Task<bool> LeaveRoomAsync(string roomId, string participantId);
    }
}