using UserService.Entity;

namespace UserService.Models.Users
{
    public class RegisterUserRequest
    {
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;

        public static User ToEntity(RegisterUserRequest user)
        {
            return new User()
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Username = user.Username,
                PasswordHash = user.Password,
                Email = user.Email,
                CreatedBy = "Admin",
                LastModifiedBy = "Admin",
            };
        }
    }
}