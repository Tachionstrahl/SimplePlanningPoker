using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ILogger<RoomController> logger;
        private readonly IRoomManager roomManager;

        public RoomController(ILogger<RoomController> logger, IRoomManager roomManager)
        {
            this.logger = logger;
            this.roomManager = roomManager;
        }

        // POST api/room/create
        [HttpPost("create")]
        public async Task<ActionResult<string>> Create()
        {
            logger.LogInformation("Creating a room.");
            var result = await Task.Run(() => roomManager.CreateRoom());
            return result.Item1 == AddRoomResult.Failed ? BadRequest("Creating a room failed.") : Ok(result.Item2);
        }

        // POST api/room/{roomId}/join
        [HttpPost("{roomId}/[action]")]
        public async Task<IActionResult> Join(string roomId)
        {
            User participant = GetUserFromHttpContext();
            var success = await roomManager.JoinRoomAsync(roomId, participant);

            if (success)
            {
                return Ok("Joined room successfully");
            }
            else
            {
                return BadRequest("Failed to join room");
            }
        }

        // POST api/room/{roomId}/estimate
        // Body {estimate}
        [HttpPost("{roomId}/[action]")]
        public async Task<IActionResult> Estimate(string roomId, [FromBody] string estimate)
        {
            if (string.IsNullOrWhiteSpace(roomId))
                return BadRequest("Room ID is required.");

            if (string.IsNullOrEmpty(estimate))
                return BadRequest("Estimate is required.");

            var user = GetUserFromHttpContext();
            var room = await roomManager.GetRoomAsync(roomId);
            if (room == null)
                return BadRequest("Room not found");
            var estimationResult = room.Estimate(user.Id, estimate);
            return estimationResult == EstimationResult.Success? Ok("Estimate added successfully") : BadRequest("Failed to add estimate");
        }

        [HttpPost("{roomId}/[action]")]
        public async Task<IActionResult> FlipCards(string roomId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{roomId}/[action]")]
        public async Task<IActionResult> Reset(string roomId)
        {
            throw new NotImplementedException();
        }


        private User GetUserFromHttpContext()
        {
            var user = new User()
            {
                Name = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? throw new ArgumentNullException("User has no name"),
                Id = RandomIDGenerator.GenerateRandomID(6)
            };

            return user;

        }

    }
}

