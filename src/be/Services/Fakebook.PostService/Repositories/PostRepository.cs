using Fakebook.DataAccessLayer.Implementaions;
using Fakebook.PostService.Data;
using Fakebook.PostService.Entity;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.PostService.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(ServiceContext context) : base(context)
        {
        }

        public async Task<Post?> GetPostById(string postId)
        {
            var query = await _context.Set<Post>()
                .Include(e => e.Comments)
                .Include(e => e.Likes)
                .Where(e => e.Id == postId && !e.IsDeleted)
                .FirstOrDefaultAsync();

            return query;
        }
    }
}
