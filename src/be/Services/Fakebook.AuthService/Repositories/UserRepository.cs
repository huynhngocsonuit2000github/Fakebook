using Fakebook.DataAccessLayer.Implementaions;
using Fakebook.AuthService.Data;
using Fakebook.AuthService.Entity;
using Fakebook.AuthService.Models.Users;
using Microsoft.EntityFrameworkCore;
using Fakebook.DataAccessLayer.HttpRequestHandling;

namespace Fakebook.AuthService.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly IUserContextService _userContextService;
        private ServiceContext serviceContext;
        public UserRepository(ServiceContext context, IUserContextService userContextService) : base(context)
        {
            _userContextService = userContextService;
            serviceContext = context;
        }

        public async Task<AuthenticatedUserModel?> GetAuthenticatedUserAsync()
        {
            var authUserContext = _userContextService.GetAuthenticatedUserContext();

            if (authUserContext is null) return null;

            var user = await GetByIdAsync(authUserContext.UserId);

            if (user is null) return null;

            return AuthenticatedUserModel.FromEntity(user);
        }

        public async Task<List<string>?> GetUserPermissionsByUserIdAsync(string userId)
        {
            return await serviceContext.UserRoles
                        .Include(e => e.Role)
                            .ThenInclude(e => e.RolePermissions)
                                .ThenInclude(e => e.Permission)
                        .Where(e => e.UserId == userId)
                        .SelectMany(e => e.Role.RolePermissions.Select(x => x.Permission.PermissionName))
                        .ToListAsync();
        }
    }
}
