using Fakebook.DataAccessLayer.Interfaces;
using Fakebook.AuthService.Entity;
using Fakebook.AuthService.Models.Users;
using Fakebook.AuthService.Helpers;

namespace Fakebook.AuthService.Repositories
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<Role> GetRoleByRoleNameAsync(string defaultRoleName);
    }
}
