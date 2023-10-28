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
    public async Task Create_ReturnsRoomId()
    {
        // Arrange
        var expectedRoomId = "12345";
        var roomManagerMock = new Mock<IRoomManager>();
        roomManagerMock.Setup(x => x.CreateRoom()).Returns((AddRoomResult.Success, expectedRoomId));

        var hubContextMock = new Mock<IHubContext<RoomHub>>();
        var controller = new RoomController(_logger, roomManagerMock.Object, hubContextMock.Object);

        // Act
        var actionResult = await controller.Create();

        // Assert
        var result = actionResult.Result as OkObjectResult;
        Assert.NotNull(result);
        Assert.Equal(expectedRoomId, result.Value);
    }

    [Fact]
    public async Task Estimate_ReturnsBadRequest_WhenRoomIdIsNullOrWhitespace()
    {
        // Arrange
        var roomManagerMock = new Mock<IRoomManager>();
        var hubContextMock = new Mock<IHubContext<RoomHub>>();
        var controller = new RoomController(_logger, roomManagerMock.Object, hubContextMock.Object);
        // Act
        var result = await controller.Estimate(null, "5");

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Room ID is required.", badRequestResult.Value);
    }

    [Fact]
    public async Task Estimate_ReturnsBadRequest_WhenEstimateIsNullOrEmpty()
    {
        // Arrange
        var roomManagerMock = new Mock<IRoomManager>();
        var hubContextMock = new Mock<IHubContext<RoomHub>>();
        var controller = new RoomController(_logger, roomManagerMock.Object, hubContextMock.Object);

        // Act
        var result = await controller.Estimate("123", null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Estimate is required.", badRequestResult.Value);
    }

    [Fact]
    public async Task Estimate_ReturnsBadRequest_WhenRoomNotFound()
    {
        // Arrange
        var user = new User() { Id = "user1", Name = "Bob" };
        var httpContext = new Mock<HttpContext>();
        httpContext.Setup(c => c.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("name", user.Name), new Claim("user_id", user.Id) })));
        var roomManagerMock = new Mock<IRoomManager>();
        roomManagerMock.Setup(x => x.GetRoomAsync("123")).ReturnsAsync((Room)null);
        var hubContextMock = new Mock<IHubContext<RoomHub>>();
        var controller = new RoomController(_logger, roomManagerMock.Object, hubContextMock.Object);
        controller.ControllerContext.HttpContext = httpContext.Object;

        // Act
        var result = await controller.Estimate("123", "5");

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Room not found", badRequestResult.Value);
    }

    [Fact]
    public async Task Estimate_ReturnsOk_WhenEstimationAddedSuccessfully()
    {
        // Arrange
        var user = new User() { Id = "user1", Name = "Bob" };

        var roomManagerMock = new Mock<IRoomManager>();
        var room = new Room("123");
        room.AddParticipant(user);
        roomManagerMock.Setup(x => x.GetRoomAsync("123")).ReturnsAsync(room);
        var hubContextMock = new Mock<IHubContext<RoomHub>>();
        var httpContext = new Mock<HttpContext>();
        httpContext.Setup(c => c.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("name", user.Name), new Claim("user_id", user.Id) })));
        hubContextMock.Setup(x => x.Clients.Group("123")).Returns(new Mock<IClientProxy>().Object);
        var controller = new RoomController(_logger, roomManagerMock.Object, hubContextMock.Object);
        controller.ControllerContext.HttpContext = httpContext.Object;
        // Act
        var result = await controller.Estimate("123", "5");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Estimated successfully", okResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenRoomCreationFails()
    {
        // Arrange
        var roomManagerMock = new Mock<IRoomManager>();
        roomManagerMock.Setup(x => x.CreateRoom()).Returns((AddRoomResult.Failed, string.Empty));

        var hubContextMock = new Mock<IHubContext<RoomHub>>();
        var controller = new RoomController(_logger, roomManagerMock.Object, hubContextMock.Object);

        // Act
        var result = await controller.Create();

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Creating a room failed.", badRequestResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsOk_WhenRoomCreationSucceeds()
    {
        // Arrange
        var roomId = "abc123";
        var roomManagerMock = new Mock<IRoomManager>();
        roomManagerMock.Setup(x => x.CreateRoom()).Returns((AddRoomResult.Success, roomId));

        var hubContextMock = new Mock<IHubContext<RoomHub>>();
        var controller = new RoomController(_logger, roomManagerMock.Object, hubContextMock.Object);

        // Act
        var result = await controller.Create();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(roomId, okResult.Value);
    }

}
