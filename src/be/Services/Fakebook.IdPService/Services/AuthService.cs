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
                new User { Username = "user1", Password = "password1", Email = "user1@example.com" },
                new User { Username = "user2", Password = "password2", Email = "user2@example.com" }
            };
    }

    public string Authenticate(string username, string password)
    {
        var user = _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        return user?.Email;
    }
}