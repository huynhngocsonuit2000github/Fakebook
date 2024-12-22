using Fakebook.IdPService.Entity;

namespace Fakebook.IdPService.Models.Users
{
    public class UserDetailModel
    {
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public static UserDetailModel FromEntity(User user)
        {
            return new UserDetailModel()
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Username = user.Username,
                Email = user.Email,
            };
        }
    }
}