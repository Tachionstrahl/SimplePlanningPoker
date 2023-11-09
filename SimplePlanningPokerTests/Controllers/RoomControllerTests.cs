namespace SimplePlanningPokerTests;

using SimplePlanningPoker.Controllers;
using Moq;
using SimplePlanningPoker.Managers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using SimplePlanningPoker.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using SimplePlanningPoker.Hubs;

public class RoomControllerTests
{
    private ILogger<RoomController> _logger = new Mock<ILogger<RoomController>>().Object;
    public RoomControllerTests()
    {

    }

    /// <summary>
    /// Tests that Create returns a RoomId.
    /// </summary>
    [Fact]
    public void Create_ReturnsRoomId()
    {
        // Arrange
        var roomId = "12345";
        var expectedRoomId = $"\"{roomId}\"";
        var roomManagerMock = new Mock<IRoomManager>();
        roomManagerMock.Setup(x => x.CreateRoom()).Returns((AddRoomResult.Success, roomId));
        var controller = new RoomController(_logger, roomManagerMock.Object);

        // Act
        var actionResult = controller.GetNewRoom();

        // Assert
        var result = actionResult.Result as ContentResult;
        Assert.NotNull(result);
        Assert.Equal(expectedRoomId, result.Content);
    }




    [Fact]
    public void Create_ReturnsBadRequest_WhenRoomCreationFails()
    {
        // Arrange
        var roomManagerMock = new Mock<IRoomManager>();
        roomManagerMock.Setup(x => x.CreateRoom()).Returns((AddRoomResult.Failed, string.Empty));

        var controller = new RoomController(_logger, roomManagerMock.Object);

        // Act
        var result = controller.GetNewRoom();

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Creating a room failed.", badRequestResult.Value);
    }

    [Fact]
    public void Create_ReturnsContent_WhenRoomCreationSucceeds()
    {
        // Arrange
        var roomId = "abc123";
        var roomManagerMock = new Mock<IRoomManager>();
        roomManagerMock.Setup(x => x.CreateRoom()).Returns((AddRoomResult.Success, roomId));

        var controller = new RoomController(_logger, roomManagerMock.Object);

        // Act
        var result = controller.GetNewRoom();

        // Assert
        var contentResult = Assert.IsType<ContentResult>(result.Result);
        Assert.Equal($"\"{roomId}\"", contentResult.Content);
    }

}
