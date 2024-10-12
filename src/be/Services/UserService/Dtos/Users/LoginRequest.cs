namespace Fakebook.UserService.Dtos.Users
{
    public class LoginRequest
    {
        public string Username { get; set;} = null!;
        public string Password { get; set;} = null!;
    }
}