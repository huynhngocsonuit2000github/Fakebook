namespace Fakebook.SynchronousModel.Models.IdPService.Users
{
    public class IdPLoginCreateUserModel
    {
        public string Id { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string LastModifiedBy { get; set; } = null!;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public bool IsInternalUser { get; set; }
    }
}