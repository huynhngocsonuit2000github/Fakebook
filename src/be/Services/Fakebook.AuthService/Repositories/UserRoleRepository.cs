using Fakebook.DataAccessLayer.Implementaions;
using Fakebook.AuthService.Data;
using Fakebook.AuthService.Entity;

namespace Fakebook.AuthService.Repositories
{
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(ServiceContext context) : base(context)
        {
        }
    }
}
