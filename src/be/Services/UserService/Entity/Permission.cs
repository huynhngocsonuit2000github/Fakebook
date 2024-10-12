namespace UserService.Entity
{
    public class Permission : BaseEntity
    {
        public string PermissionName { get; set; } = null!;
        public string? Description { get; set; }


        public virtual IEnumerable<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}