using Fakebook.UserService.Entity;

namespace Fakebook.UserService.Models.Users
{
    public class AuthenticatedUserModel
    {
        public string Id { get; set; } = null!;
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;

        public static AuthenticatedUserModel FromEntity(User user)
        {
            return new AuthenticatedUserModel()
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
            };
        }
    }
}