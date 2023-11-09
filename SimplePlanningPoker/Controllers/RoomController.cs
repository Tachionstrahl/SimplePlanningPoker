using Microsoft.AspNetCore.Mvc;
using SimplePlanningPoker.Managers;
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

        /// <summary>
        /// GET api/room
        /// Creates a new room.
        /// </summary>
        /// <returns>The room's ID</returns>
        [HttpGet]
        public ActionResult<string> GetNewRoom()
        {
            logger.LogInformation(nameof(GetNewRoom));

            var result = roomManager.CreateRoom();
            
            return result.Item1 == AddRoomResult.Failed
            ? BadRequest("Creating a room failed.")
            : Content($"\"{result.Item2}\"", "application/json");
        }

    }
}

