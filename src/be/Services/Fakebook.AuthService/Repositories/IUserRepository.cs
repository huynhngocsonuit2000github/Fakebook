using Fakebook.DataAccessLayer.Interfaces;
using Fakebook.AuthService.Entity;
using Fakebook.AuthService.Models.Users;

namespace Fakebook.AuthService.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<AuthenticatedUserModel?> GetAuthenticatedUserAsync();
        Task<List<string>?> GetUserPermissionsByUserIdAsync(string userId);
    }
}
