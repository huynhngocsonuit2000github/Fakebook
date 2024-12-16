using Fakebook.AuthService.Dtos.Users;
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

        public AuthController(IUserUservice userUservice, ITokenService tokenService)
        {
            _userUservice = userUservice;
            _tokenService = tokenService;
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

            return BadRequest("The IpToken should be: THIS_IS_IDP_TOKEN");
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