using SimplePlanningPoker.Managers;
using SimplePlanningPoker.Models;

namespace SimplePlanningPokerTests;

public class RoomManagerTests
{
    private readonly RoomManager roomManager;

    public RoomManagerTests()
    {
        roomManager = new RoomManager();
    }

    [Fact]
    public void CreateRoom_ReturnsRoomId()
    {
        // Act
        var result = roomManager.CreateRoom();

        // Assert
        Assert.NotNull(result.Item2);
    }

    [Fact]
    public void GetRoomAsync_ReturnsAddedRoom()
    {
        // Arrange
        var (result, roomId) = roomManager.CreateRoom();

        // Act
        var room = roomManager.GetRoom(roomId);

        // Assert
        Assert.NotNull(room);
        Assert.Equal(AddRoomResult.Success, result);
        Assert.Equal(roomId, room.RoomId);
    }


}
