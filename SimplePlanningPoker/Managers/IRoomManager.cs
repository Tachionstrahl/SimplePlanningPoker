using SimplePlanningPoker.Models;

namespace SimplePlanningPoker.Managers
{
    public interface IRoomManager
    {
        Task<Room?> CreateRoomAsync();
        Task<Room?> GetRoomAsync(string roomId);
        Task<bool> JoinRoomAsync(string roomId, User participant);
        Task<bool> LeaveRoomAsync(string roomId, User participant);
    }
}