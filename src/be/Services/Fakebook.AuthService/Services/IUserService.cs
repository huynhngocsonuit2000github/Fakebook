using Fakebook.AuthService.Entity;
using Fakebook.AuthService.Dtos.Users;

namespace Fakebook.AuthService.Services
{
    public interface IUserUservice
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<string> CreateUserAsync(User user);
        Task<User?> GetUserByUsernameAsync(string username);
        Task UpdateAsync(UpdateUserRequest userRequest);
    }
}
