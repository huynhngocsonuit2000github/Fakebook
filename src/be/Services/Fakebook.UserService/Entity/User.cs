
using Fakebook.DataAccessLayer.Entity;

namespace Fakebook.UserService.Entity
{
    public class User : BaseEntity
    {
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? Bio { get; set; } = null!;
        public string? ProfilePictureUrl { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        

        public virtual IEnumerable<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}