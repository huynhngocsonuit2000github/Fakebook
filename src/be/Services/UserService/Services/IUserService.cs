using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserService.Data;
using UserService.Entity;

namespace UserService.Services
{
    public interface IUserUservice
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<string> CreateUserAsync(User user);
    }
}
