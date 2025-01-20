using Fakebook.PostService.Entity;

namespace Fakebook.PostService.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<IEnumerable<Comment>> GetCommentsByPostId(string postId);
        Task<Comment> CreateComment(Comment model);
    }
}
