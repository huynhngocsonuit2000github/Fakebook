using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserService.Data;
using UserService.Entity;

namespace UserService.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ServiceContext context) : base(context)
        {
        }
    }
}
