using UserService.Entity;

namespace UserService.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);    
    }
}