using Fakebook.IdPService.Entity;

namespace Fakebook.IdPService.Models.Users
{
    public class LoginUserModel
    {
        public string Email { get; set; } = null!;

        public static LoginUserModel FromEntity(User user)
        {
            return new LoginUserModel()
            {
                Email = user.Email,
            };
        }
    }
}