
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplePlanningPoker.Managers;
using SimplePlanningPoker.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplePlanningPoker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> logger;
        private readonly IRoomManager roomManager;

        public RoomController(ILogger<RoomController> logger, IRoomManager roomManager)
        {
            this.logger = logger;
            this.roomManager = roomManager;
        }

        // GET api/room
        [HttpGet]
        public async Task<ActionResult<string>> GetNewRoom()
        {
            logger.LogInformation(nameof(GetNewRoom));
            var result = await Task.Run(() => roomManager.CreateRoom());
            return result.Item1 == AddRoomResult.Failed ? BadRequest("Creating a room failed.") : Content($"\"{result.Item2}\"", "application/json");
        }

    }
}

