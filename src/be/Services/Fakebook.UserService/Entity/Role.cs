using Fakebook.DataAccessLayer.Entity;

namespace Fakebook.UserService.Entity
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; } = null!;
        public string? Description { get; set; }


        public virtual IEnumerable<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual IEnumerable<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}