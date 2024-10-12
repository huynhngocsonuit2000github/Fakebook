using Fakebook.UserService.Entity;

namespace Fakebook.UserService.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);    
    }
}