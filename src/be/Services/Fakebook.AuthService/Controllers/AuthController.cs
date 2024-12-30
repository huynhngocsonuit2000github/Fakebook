using Fakebook.AuthService.Dtos.Users;
using Fakebook.AuthService.Helpers;
using Fakebook.AuthService.Services;
using Fakebook.MessageQueueHandler.Publisher;
using Fakebook.MessageQueueHandler.Utils;
using Fakebook.SynchronousModel.Models.IdPService.Users;
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
        private readonly IRabbitMQPublisher _rabbitMQPublisher;

        public AuthController(IUserUservice userUservice, ITokenService tokenService, ITokenHelper tokenHelper, IRabbitMQPublisher rabbitMQPublisher)
        {
            _userUservice = userUservice;
            _tokenService = tokenService;
            _tokenHelper = tokenHelper;
            _rabbitMQPublisher = rabbitMQPublisher;
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

                // Sync data to User service
                var userModel = new IdPLoginCreateUserModel()
                {
                    Id = user.Id,
                    CreatedBy = user.CreatedBy,
                    LastModifiedBy = user.LastModifiedBy,
                    CreatedDate = user.CreatedDate,
                    LastModifiedDate = user.LastModifiedDate,
                    IsDeleted = user.IsDeleted,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Username = user.Username,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    IsActive = user.IsActive,
                    IsInternalUser = user.IsInternalUser,
                };
                await _rabbitMQPublisher.PublishMessageAsync(RoutingQueueConstants.UserService.UserService_IPD_Login_Create_User, userModel);

                // then generate new token
                var token = _tokenService.GenerateToken(user);

                return Ok(await Task.FromResult(token));
            }

            return BadRequest("The IdPToken is invalid");
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