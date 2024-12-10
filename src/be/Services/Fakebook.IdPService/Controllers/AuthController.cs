using Fakebook.IdPService.Dtos;
using Fakebook.IdPService.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fakebook.IdPService.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        var email = _authService.Authenticate(loginRequest.Username, loginRequest.Password);
        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(new LoginResponse()
        {
            Email = email,
            IdPToken = "THIS_IS_IDP_TOKEN"
        });
    }
}