namespace Fakebook.UserService.DataSeeding.Models
{
    public class SeedData
    {
        public List<UserData> Users { get; set; } = null!;
    }

    public class UserData
    {
        public string Id { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public bool IsActive { get; set; }
        public bool IsInternalUser { get; set; }
    }

}