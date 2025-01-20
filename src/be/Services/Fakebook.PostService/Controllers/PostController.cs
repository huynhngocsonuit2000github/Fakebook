using Fakebook.PostService.Dtos.Posts;
using Fakebook.PostService.Services;
using Fakebook.UserService.Dtos.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fakebook.PostService.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly ILogger<PostController> _logger;
    private readonly IPostService _postService;

    public PostController(ILogger<PostController> logger, IPostService postService)
    {
        _logger = logger;
        _postService = postService;
    }

    [HttpGet("test")]
    public IActionResult TestAsync()
    {
        return Ok("Okay post service");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var posts = await _postService.GetAllAsync();
        return Ok(posts.Select(GetPostModel.FromEntity));
    }

    [HttpGet("{postId}")]
    public async Task<IActionResult> GetByIdAsync(string postId)
    {
        var post = await _postService.GetByIdAsync(postId);
        return Ok(GetPostModel.FromEntity(post));
    }

    [HttpGet("get-by-current-user")]
    [Authorize]
    public async Task<IActionResult> GetByCurrentUserAsync()
    {
        var posts = await _postService.GetByCurrentUserAsync();
        return Ok(posts.Select(GetPostModel.FromEntity));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateAsync(CreatePostRequest request)
    {
        var post = await _postService.CreateAsync(request);
        return Ok(post.Id);
    }

    [HttpPatch("{postId}")]
    [Authorize]
    public async Task<IActionResult> UpdateAsync(string postId, UpdatePostRequest request)
    {
        await _postService.UpdateAsync(postId, request);
        return NoContent();
    }

    [HttpDelete("{postId}")]
    [Authorize]
    public async Task<IActionResult> DeleteAsync(string postId)
    {
        await _postService.DeleteAsync(postId);
        return NoContent();
    }
}
