using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserService.Data;
using UserService.Entity;
using UserService.Dtos.Users;

namespace UserService.Services
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
