using Fakebook.AuthService.Dtos.Users;
using Fakebook.AuthService.Helpers;
using Fakebook.AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Fakebook.AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserUservice _userUservice;
        private readonly ITokenService _tokenService;
        private readonly ITokenHelper _tokenHelper;

        public AuthController(IUserUservice userUservice, ITokenService tokenService, ITokenHelper tokenHelper)
        {
            _userUservice = userUservice;
            _tokenService = tokenService;
            _tokenHelper = tokenHelper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterUserRequest user)
        {
            var userId = await _userUservice.CreateUserAsync(RegisterUserRequest.ToEntity(user));

            return Ok(userId);
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

        [HttpPost("exchange-idp-token")]
        public async Task<IActionResult> ExchangeIdPTokenAsync(ExchangeIdPToken input)
        {
            if (_tokenHelper.ValidateIdPToken(input.IdPToken))
            {
                // If first time -> call to IdP to get information and create user
                var user = await _userUservice.GetOrCreateUserByEmailAsync(input.Email);

                // then generate new token
                var token = _tokenService.GenerateToken(user);

                return Ok(await Task.FromResult(token));
            }

            return BadRequest("The IpToken is invalid");
        }

        [Authorize]
        [HttpGet("get-user-permission")]
        public async Task<IActionResult> GetUserPermissionAsync()
        {
            // get from database
            var permission = await _userUservice.GetCurrentUserPermissionsAsync();

            return Ok(permission);
        }
    }
}