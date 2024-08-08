using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.BusinessLayer;
using UserManagementAPI.BusinessLayer.Service;
using UserManagementAPI.DataAccessLayer;
using UserManagementAPI.Model;

namespace UserManagementAPI.PresentationLayer.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "User,Admin")] // Accessibile da User e Admin
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")] // Accessibile da User e Admin
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")] // Accessibile da User e Admin
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            await _userService.AddUserAsync(user);
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User,Admin")] // Accessibile da User e Admin
        public async Task<IActionResult> PutUser(int id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            await _userService.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Accessibile solo da Admin
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }

}
