using Fakebook.UserService.Entity;
using Fakebook.UserService.Dtos.Users;
using Fakebook.SynchronousModel.Models.IdPService.Users;

namespace Fakebook.UserService.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task UpdateAsync(UpdateUserRequest userRequest);
        Task SyncCreatedIdPUserAsync(IdPLoginCreateUserModel input);
    }
}
