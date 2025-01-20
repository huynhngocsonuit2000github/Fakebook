using Fakebook.PostService.Entity;
using Fakebook.UserService.Dtos.Users;

namespace Fakebook.PostService.Services
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllAsync();
        Task<Post> GetByIdAsync(string postId);
        Task<IEnumerable<Post>> GetByCurrentUserAsync();
        Task<Post> CreateAsync(CreatePostRequest request);
        Task UpdateAsync(string postId, UpdatePostRequest request);
        Task DeleteAsync(string postId);
    }
}
