using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Entity
{
    public class RolePermission : BaseEntity
    {
        public string RoleId { get; set; } = null!;
        public string PermissionId { get; set; } = null!;


        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } = null!;

        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; } = null!;
    }
}