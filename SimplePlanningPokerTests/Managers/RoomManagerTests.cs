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
        var room = roomManager.GetRoomAsync(roomId).Result;

        // Assert
        Assert.NotNull(room);
        Assert.Equal(AddRoomResult.Success, result);
        Assert.Equal(roomId, room.RoomId);
    }

    [Fact]
    public void JoinRoomAsync_ReturnsTrue()
    {
        // Arrange
        var id = roomManager.CreateRoom();

        // Act
        var result = roomManager.JoinRoomAsync(id.Item2, new User { Name = "Bob", ConnectionId = "4711" }).Result;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void JoinRoomAsync_ReturnsFalse_OnNonExistingRoom()
    {
        // Arrange

        // Act
        var result = roomManager.JoinRoomAsync("4711", new User { Name = "Bob", ConnectionId = "4711" }).Result;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void JoinRoomAsync_ReturnsFalse_OnDuplicateUser()
    {
        // Arrange
        var id = roomManager.CreateRoom();
        // Act
        var resultOne = roomManager.JoinRoomAsync(id.Item2, new User { Name = "Bob", ConnectionId = "4711" }).Result;
        var resultTwo = roomManager.JoinRoomAsync(id.Item2, new User { Name = "Bob", ConnectionId = "4711" }).Result;

        // Assert
        Assert.True(resultOne);
        Assert.False(resultTwo);
    }
}
