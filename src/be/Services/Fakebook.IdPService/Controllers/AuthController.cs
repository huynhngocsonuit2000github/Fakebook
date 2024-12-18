using Fakebook.IdPService.Dtos;
using Fakebook.IdPService.Helpers;
using Fakebook.IdPService.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fakebook.IdPService.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly ITokenHelper _tokenHelper;

    public AuthController(AuthService authService, ITokenHelper tokenHelper)
    {
        _authService = authService;
        _tokenHelper = tokenHelper;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        var email = _authService.Authenticate(loginRequest.Username, loginRequest.Password);
        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized("Invalid credentials");
        }

        var token = _tokenHelper.GenerateIdPToken(email);

        return Ok(new LoginResponse()
        {
            Email = email, // TODO: Remove email in the future
            IdPToken = token
        });
    }
}