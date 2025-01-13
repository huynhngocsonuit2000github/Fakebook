using Microsoft.AspNetCore.Mvc;

namespace Fakebook.PostService.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly ILogger<PostController> _logger;

    public PostController(ILogger<PostController> logger)
    {
        _logger = logger;
    }

    [HttpGet("test")]
    public IActionResult TestAsync()
    {
        return Ok("Okay post service");
    }
}
