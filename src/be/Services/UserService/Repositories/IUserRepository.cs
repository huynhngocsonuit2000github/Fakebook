using UserService.Data;
using UserService.Entity;
using UserService.Models.Users;

namespace UserService.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<AuthenticatedUserModel?> GetAuthenticatedUserAsync();
    }
}
