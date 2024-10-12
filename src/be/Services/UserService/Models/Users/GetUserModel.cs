using UserService.Entity;

namespace UserService.Models.Users
{
    public class GetUserModel
    {
        public string Id { get; set; } = null!;
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;

        public static GetUserModel FromEntity(User user)
        {
            return new GetUserModel()
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Username = user.Username,
                Password = user.PasswordHash,
                Email = user.Email,
            };
        }
    }
}