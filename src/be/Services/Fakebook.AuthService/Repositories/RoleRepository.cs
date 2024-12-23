using Fakebook.DataAccessLayer.Implementaions;
using Fakebook.AuthService.Data;
using Fakebook.AuthService.Entity;
using Fakebook.AuthService.HttpRequestHandling;
using Fakebook.AuthService.Models.Users;
using Microsoft.EntityFrameworkCore;
using Fakebook.AuthService.Helpers;

namespace Fakebook.AuthService.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        private ServiceContext serviceContext;
        public RoleRepository(ServiceContext context, IUserContextService userContextService) : base(context)
        {
            serviceContext = context;
        }

        public async Task<Role> GetRoleByRoleNameAsync(string defaultRoleName)
        {
            return await serviceContext.Roles.FirstOrDefaultAsync(e => string.Equals(defaultRoleName, e.RoleName, StringComparison.OrdinalIgnoreCase)) ??
                    throw new Exception("The role name is not exists");
        }
    }
}
