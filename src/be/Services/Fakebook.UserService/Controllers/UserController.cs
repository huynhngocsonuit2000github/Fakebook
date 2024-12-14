using Fakebook.UserService.Dtos.Users;
using Fakebook.UserService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Fakebook.UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserUservice _userUservice;

        public UserController(IUserUservice userUservice)
        {
            _userUservice = userUservice;
        }

        [HttpGet("test")]
        public async Task<IActionResult> TestAsync(string name, string old)
        {
            return Ok("Very good day! " + name + ", I am " + old + " years old");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userUservice.GetAllAsync();

            return Ok(users.Select(GetUserModel.FromEntity));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            var user = await _userUservice.GetUserByIdAsync(id);

            if (user is null)
            {
                return NotFound($"The use is not found with Id = {id}");
            }

            var userModel = GetUserModel.FromEntity(user);

            return Ok(userModel);
        }

        [Authorize]
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateAsync(UpdateUserRequest user)
        {
            await _userUservice.UpdateAsync(user);

            return NoContent();
        }
    }
}