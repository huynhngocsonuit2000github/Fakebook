using Fakebook.DataAccessLayer.Implementaions;
using Fakebook.PostService.Data;
using Fakebook.PostService.Entity;

namespace Fakebook.PostService.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ServiceContext context) : base(context)
        {
        }
    }
}
