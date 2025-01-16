using Fakebook.DataAccessLayer.Implementaions;
using Fakebook.PostService.Data;
using Fakebook.PostService.Entity;

namespace Fakebook.PostService.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ServiceContext context) : base(context)
        {
        }
    }
}
