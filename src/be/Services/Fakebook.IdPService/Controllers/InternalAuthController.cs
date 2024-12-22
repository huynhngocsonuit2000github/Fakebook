using Fakebook.IdPService.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fakebook.IdPService.Controllers;

[ApiController]
[Route("[controller]")]
public class InternalAuthController : ControllerBase
{
    private readonly IUserUservice _authService;

    public InternalAuthController(IUserUservice authService)
    {
        _authService = authService;
    }

    [HttpGet("get-user-detail-by-email")]
    public async Task<IActionResult> GetUserDetailByEmailAsync(string email)
    {
        var user = await _authService.GetUserDetailAsync(email);

        return Ok(user);
    }
}