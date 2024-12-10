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

        /// <summary>
        /// Temporary API, which will be moved to individual Auth service
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("exchange-idp-token")]
        public async Task<IActionResult> ExchangeIdPTokenAsync(ExchangeIdPToken input)
        {
            System.Console.WriteLine("========== Exchange");
            System.Console.WriteLine("Email: " + input.Email);
            System.Console.WriteLine("IdPToken: " + input.IdPToken);
            // there will be a function to validate the IdP token
            if (input.IdPToken == "THIS_IS_IDP_TOKEN")
            {
                // If first time -> call to IdP to get information and create user
                // then generate new token
                var token = _tokenService.GenerateToken(new Entity.User()
                {
                    Username = "ustest1",
                    Id = "2b8c5b35-654c-438d-af9a-30a70459493c"
                });

                return Ok(await Task.FromResult(token));
            }

            return Ok();
        }

        /// <summary>
        /// Temporary API, which will be moved to individual Auth service
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("get-user-permission")]
        public async Task<IActionResult> GetUserPermissionAsync()
        {
            System.Console.WriteLine("========== Get Permission");
            // get from database
            var permission = new List<string>(){
            "member_read", "member_create", "admin_read",  "admin_create"
            };

            return Ok(await Task.FromResult(permission));
        }
    }
}