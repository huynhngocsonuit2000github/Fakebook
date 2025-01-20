using Fakebook.DataAccessLayer.Implementaions;
using Fakebook.PostService.Data;
using Fakebook.PostService.Entity;

namespace Fakebook.PostService.Repositories
{
    public class LikeRepository : BaseRepository<Like>, ILikeRepository
    {
        public LikeRepository(ServiceContext context) : base(context)
        {
        }
    }
}
