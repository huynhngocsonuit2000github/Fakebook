using Fakebook.UserService.Entity;
using Fakebook.UserService.Dtos.Users;

namespace Fakebook.UserService.Services
{
    public interface IUserUservice
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task UpdateAsync(UpdateUserRequest userRequest);
    }
}
