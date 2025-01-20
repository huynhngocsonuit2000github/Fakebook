using Fakebook.PostService.Dtos.Comments;
using Fakebook.PostService.Dtos.Posts;
using Fakebook.PostService.Services;
using Fakebook.UserService.Dtos.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fakebook.PostService.Controllers;

[ApiController]
[Route("[controller]")]
public class LikeController : ControllerBase
{
    private readonly ILogger<LikeController> _logger;
    private readonly ILikeService _likeService;

    public LikeController(ILogger<LikeController> logger, ILikeService likeService)
    {
        _logger = logger;
        _likeService = likeService;
    }

    [HttpPost("like-unlike")]
    public async Task<IActionResult> LikeUnlikeAsync(CreateLikeModel model)
    {
        var like = await _likeService.ChangeLikeStateAsync(model.ToEntity());
        return Ok(like);
    }
}
