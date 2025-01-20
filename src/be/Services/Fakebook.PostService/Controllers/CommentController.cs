using Fakebook.PostService.Dtos.Comments;
using Fakebook.PostService.Dtos.Posts;
using Fakebook.PostService.Services;
using Fakebook.UserService.Dtos.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fakebook.PostService.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ILogger<CommentController> _logger;
    private readonly ICommentService _commentService;

    public CommentController(ILogger<CommentController> logger, ICommentService commentService)
    {
        _logger = logger;
        _commentService = commentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var comments = await _commentService.GetAllAsync();
        return Ok(comments.Select(GetCommentModel.FromEntity));
    }

    [HttpGet("get-by-post-id/{postId}")]
    public async Task<IActionResult> GetCommentsByPostAsync(string postId)
    {
        var comments = await _commentService.GetCommentsByPostId(postId);
        return Ok(comments.Select(GetCommentModel.FromEntity));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCommentAsync(CreateCommentModel model)
    {
        var comment = await _commentService.CreateComment(model.ToEntity());
        return Ok(comment);
    }
}
