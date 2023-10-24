﻿using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimplePlanningPoker.Managers;
using SimplePlanningPoker.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplePlanningPoker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoomController : ControllerBase
    {
        private readonly IDictionary<string, Room> _rooms = new ConcurrentDictionary<string, Room>();
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
            var room = await roomManager.CreateRoomAsync();
            return room == null ? BadRequest("Creating a room failed.") : Ok(room.RoomId);
        }

        // POST api/room/{roomId}/join
        [HttpPost("{roomId}/[action]")]
        public async Task<IActionResult> Join(string roomId)
        {
            var success = await roomManager.JoinRoomAsync(roomId, "4711"); //TODO

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
            throw new NotImplementedException();
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
                Name = User.Identity?.Name ?? throw new ArgumentNullException("User has no name"),
                Token = User.Claims.First(c => c.Type == "Token").Value
            };

            return user;
            
        }

    }
}

