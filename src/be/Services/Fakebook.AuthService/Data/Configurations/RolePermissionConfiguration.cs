using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fakebook.AuthService.Entity;

namespace Fakebook.AuthService.Configurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasOne(rp => rp.Role)
                   .WithMany(r => r.RolePermissions)
                   .HasForeignKey(rp => rp.RoleId);

            builder.HasOne(rp => rp.Permission)
                   .WithMany(p => p.RolePermissions)
                   .HasForeignKey(rp => rp.PermissionId);
        }
    }
}