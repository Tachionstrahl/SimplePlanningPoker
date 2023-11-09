using SimplePlanningPoker.Hubs;
using SimplePlanningPoker.Models;

namespace SimplePlanningPoker.Managers
{
    /// <summary>
    /// A manager for rooms
    /// </summary>
    public interface IRoomManager
    {
        /// <summary>
        /// Creates a new room. Returns <see cref="true"/>, if successful.
        /// </summary>
        /// <returns></returns>
        (AddRoomResult, string?) CreateRoom();
        /// <summary>
        /// Retrieves a room by its ID.
        /// Returns <see cref="null"/>, if the room does not exist.
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        Room? GetRoom(string roomId);
        /// <summary>
        /// Retrieves a room by a participant.
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        Room? GetRoomByParticipant(User participant);
    }
}