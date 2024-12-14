using Fakebook.DataAccessLayer.Implementaions;
using Fakebook.AuthService.Data;
using Fakebook.AuthService.Entity;
using Fakebook.AuthService.HttpRequestHandling;
using Fakebook.AuthService.Models.Users;

namespace Fakebook.AuthService.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly IUserContextService _userContextService;
        public UserRepository(ServiceContext context, IUserContextService userContextService) : base(context)
        {
            _userContextService = userContextService;
        }

        public async Task<AuthenticatedUserModel?> GetAuthenticatedUserAsync()
        {
            var authUserContext = _userContextService.GetAuthenticatedUserContext();

            if (authUserContext is null) return null;

            var user = await GetByIdAsync(authUserContext.UserId);

            if(user is null) return null;

            return AuthenticatedUserModel.FromEntity(user);
        }
    }
}
