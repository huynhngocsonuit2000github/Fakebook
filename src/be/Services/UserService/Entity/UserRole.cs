using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Entity
{
    public class UserRole : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public string RoleId { get; set; } = null!;


        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } = null!;
    }
}