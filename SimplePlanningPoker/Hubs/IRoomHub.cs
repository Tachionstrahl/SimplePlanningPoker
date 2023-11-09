using SimplePlanningPoker.Models;

namespace SimplePlanningPoker;

/// <summary>
/// Represents server-to-client messages for a hub
/// </summary>
public interface IRoomHubClient
{
    /// <summary>
    /// Sends the room state to all clients in the group.
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    Task SendRoomState(RoomState roomState);

    /// <summary>
    /// Notify all clients in the group that the cards have been reset.
    /// </summary>
    /// <returns></returns>
    Task Reset();
}
