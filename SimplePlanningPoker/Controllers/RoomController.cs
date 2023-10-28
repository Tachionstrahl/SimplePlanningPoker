using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SimplePlanningPoker.Hubs;
using SimplePlanningPoker.Managers;
using SimplePlanningPoker.Models;
using SimplePlanningPoker.Utils;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplePlanningPoker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class RoomController : ControllerBase
    {
        private const string RoomNotFoundMessage = "Room not found";
        private const string RoomIdRequiredMessage = "Room ID is required.";
        private const string EstimateRequiredMessage = "Estimate is required.";
        private readonly ILogger<RoomController> logger;
        private readonly IRoomManager roomManager;
        private readonly IHubContext<RoomHub> hubContext;

        public RoomController(ILogger<RoomController> logger, IRoomManager roomManager, IHubContext<RoomHub> hubContext)
        {
            this.hubContext = hubContext;
            this.logger = logger;
            this.roomManager = roomManager;
        }

        // POST api/room/create
        [HttpPost("create")]
        public async Task<ActionResult<string>> Create()
        {
            logger.LogInformation(nameof(Create));
            var result = await Task.Run(() => roomManager.CreateRoom());
            return result.Item1 == AddRoomResult.Failed ? BadRequest("Creating a room failed.") : Ok(result.Item2);
        }

        // POST api/room/{roomId}/join
        [HttpPost("{roomId}/[action]")]
        public async Task<IActionResult> Join(string roomId)
        {
            if (string.IsNullOrWhiteSpace(roomId))
                return BadRequest(RoomIdRequiredMessage);


            User participant = GetUserFromHttpContext();
            var room = await roomManager.GetRoomAsync(roomId);
            if (room == null)
                return BadRequest(RoomNotFoundMessage);
            var success = await roomManager.JoinRoomAsync(roomId, participant);

            if (success)
            {
                await UpdateClientsAsync(roomId);
                return Ok("Joined room successfully");
            }
            else
                return BadRequest("Failed to join room");

        }

        // POST api/room/{roomId}/estimate
        // Body {estimate}
        [HttpPost("{roomId}/[action]")]
        public async Task<IActionResult> Estimate(string roomId, [FromBody] string estimate)
        {
            if (string.IsNullOrWhiteSpace(roomId))
                return BadRequest(RoomIdRequiredMessage);

            if (string.IsNullOrEmpty(estimate))
                return BadRequest(EstimateRequiredMessage);

            var user = GetUserFromHttpContext();

            var room = await roomManager.GetRoomAsync(roomId);
            if (room == null)
                return BadRequest(RoomNotFoundMessage);

            if (!room.ContainsParticipant(user.Id))
                return BadRequest("Not in room");

            var estimationResult = room.Estimate(user.Id, estimate);

            if (estimationResult == EstimationResult.Failed)
                return BadRequest("Failed to estimate");

            await UpdateClientsAsync(roomId);
            return Ok("Estimated successfully");
        }

        [HttpPost("{roomId}/[action]")]
        public async Task<IActionResult> FlipCards(string roomId)
        {
            if (string.IsNullOrWhiteSpace(roomId))
                return BadRequest(RoomIdRequiredMessage);

            var room = await roomManager.GetRoomAsync(roomId);
            if (room == null)
                return BadRequest(RoomNotFoundMessage);
            room.FlipCards();
            await UpdateClientsAsync(roomId);
            return Ok("Cards flipped successfully");
        }

        [HttpPost("{roomId}/[action]")]
        public async Task<IActionResult> Reset(string roomId)
        {
            var room = await roomManager.GetRoomAsync(roomId);
            if (room == null)
                return BadRequest(RoomNotFoundMessage);
            room.Reset();
            await UpdateClientsAsync(roomId);
            return Ok("Reset successful");
        }
        
        [HttpGet("{roomId}/[action]")]
        public async Task<IActionResult> GetState(string roomId) {
            var room = await roomManager.GetRoomAsync(roomId);
            if (room == null)
                return BadRequest(RoomNotFoundMessage);
            return Ok(room.State);
        }

        private async Task UpdateClientsAsync(string roomId)
        {
            var room = await roomManager.GetRoomAsync(roomId) ?? throw new ArgumentException($"Room with ID {roomId} does not exist.");
            await hubContext.Clients.Group(roomId).SendAsync(RoomHubMessages.SendRoomState, room.State);
        }

        private User GetUserFromHttpContext()
        {
            var user = new User()
            {
                Name = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value ?? throw new ArgumentNullException("User has no name"),
                Id = User.Claims.FirstOrDefault(c => c.Type == "user_id")?.Value ?? throw new ArgumentNullException("User has no ID")
            };

            return user;

        }

    }
}

