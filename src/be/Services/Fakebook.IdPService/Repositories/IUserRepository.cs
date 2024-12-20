using Fakebook.DataAccessLayer.Interfaces;
using Fakebook.IdPService.Entity;
using Fakebook.IdPService.Models.Users;

namespace Fakebook.IdPService.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<LoginUserModel> GetUserByCredentialAsync(string username, string password);
    }
}
