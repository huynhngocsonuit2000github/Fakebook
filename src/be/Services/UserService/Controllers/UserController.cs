using Microsoft.AspNetCore.Mvc;
using UserService.Entity;
using UserService.Models.Users;
using UserService.Services;
namespace UserService.Controllers
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

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userUservice.GetAllAsync();

            return Ok(users.Select(GetUserModel.FromEntity));
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterUserRequest user)
        {
            var userId = await _userUservice.CreateUserAsync(RegisterUserRequest.ToEntity(user));

            return Ok(userId);
        }

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
    }
}
