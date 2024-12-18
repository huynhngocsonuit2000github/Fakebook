using Fakebook.IdPService.Models;

namespace Fakebook.IdPService.Services;
public class AuthService
{
    private readonly List<User> _users;

    public AuthService()
    {
        // Simulated user data
        _users = new List<User>
            {
                new User { Username = "admin_idp", Password = "123", Email = "admin@gmail.com" },
                new User { Username = "truc_idp", Password = "123", Email = "truc@gmail.com" },
                new User { Username = "son_idp", Password = "123", Email = "son@gmail.com" }
            };
    }

    public string Authenticate(string username, string password)
    {
        var user = _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        return user?.Email;
    }
}