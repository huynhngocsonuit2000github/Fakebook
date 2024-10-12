using Fakebook.DataAccessLayer.Interfaces;
using Fakebook.UserService.Entity;
using Fakebook.UserService.Models.Users;

namespace Fakebook.UserService.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<AuthenticatedUserModel?> GetAuthenticatedUserAsync();
    }
}
