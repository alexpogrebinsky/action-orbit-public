using Microsoft.AspNetCore.Mvc;
using mmDailyPlanner.Server.DTO;
using mmDailyPlanner.Server.Models;
using mmDailyPlanner.Server.Repositories;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace mmDailyPlanner.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserByIdAsync(int id)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();


;
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }            
            
            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            Console.WriteLine($"GetUserByIdAsync took {elapsedMilliseconds} milliseconds to execute.");
            return Ok(user);
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<User>> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUserAsync(User user)
        {
            await _userRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserByIdAsync), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            await _userRepository.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpGet("analytics/{userId}")]
        public async Task<ActionResult<List<UserTaskAnalyticsDto>>> GetUserTaskAnalyticsAsync(int userId)
        {
            try
            {
                var analytics = await _userRepository.GetUserTaskAnalyticsAsync(userId);
                return Ok(analytics);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while getting user task analytics.");
            }
        }
    
}
}
