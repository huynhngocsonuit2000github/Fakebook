using Fakebook.DataAccessLayer.Implementaions;
using Fakebook.PostService.Data;
using Fakebook.PostService.Entity;

namespace Fakebook.PostService.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(ServiceContext context) : base(context)
        {
        }
    }
}
