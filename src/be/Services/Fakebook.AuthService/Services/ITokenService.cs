using Fakebook.AuthService.Entity;

namespace Fakebook.AuthService.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);    
    }
}