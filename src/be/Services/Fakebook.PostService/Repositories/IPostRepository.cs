using Fakebook.DataAccessLayer.Interfaces;
using Fakebook.PostService.Entity;

namespace Fakebook.PostService.Repositories
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        Task<Post?> GetPostById(string postId);
    }
}
