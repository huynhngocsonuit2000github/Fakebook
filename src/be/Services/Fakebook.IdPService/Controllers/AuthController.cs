using Fakebook.IdPService.Dtos;
using Fakebook.IdPService.Helpers;
using Fakebook.IdPService.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fakebook.IdPService.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenHelper _tokenHelper;
    private readonly IUserUservice _authService;

    public AuthController(ITokenHelper tokenHelper, IUserUservice authService)
    {
        _tokenHelper = tokenHelper;
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest)
    {
        var loginUser = await _authService.LoginAsync(loginRequest.Username, loginRequest.Password);
         
        var token = _tokenHelper.GenerateIdPToken(loginUser.Email);

        return Ok(new LoginResponse()
        {
            Email = loginUser.Email, // TODO: Remove email in the future
            IdPToken = token
        });
    }
}