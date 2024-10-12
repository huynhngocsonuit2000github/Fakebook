using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Fakebook.UserService.Authentication.Models;
using Fakebook.UserService.Dtos.Users;
using Fakebook.UserService.Services;
namespace Fakebook.UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserUservice _userUservice;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;

        public UserController(IUserUservice userUservice, ITokenService tokenService, IOptions<JwtSettings> jwtSettings)
        {
            _userUservice = userUservice;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings.Value;
        }

        [Authorize]
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

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest login)
        {
            var user = await _userUservice.GetUserByUsernameAsync(login.Username);

            if (user is null || !string.Equals(login.Password, user.PasswordHash, StringComparison.OrdinalIgnoreCase))
            {
                return Unauthorized("Invalid credential");
            }

            var token = _tokenService.GenerateToken(user);
            return Ok(token);
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