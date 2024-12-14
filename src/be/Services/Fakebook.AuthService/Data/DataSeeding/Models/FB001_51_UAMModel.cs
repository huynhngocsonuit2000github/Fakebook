using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Fakebook.AuthService.Entity;

namespace Fakebook.AuthService.DataSeeding.Models
{
    public class SeedData
    {
        public List<RoleData> Roles { get; set; } = null!;
    }

    public class RoleData
    {
        public string RoleName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<PermissionData> Permissions { get; set; } = null!;
        public List<UserData> Users { get; set; } = null!;
    }

    public class PermissionData
    {
        public string PermissionName { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    public class UserData
    {
        public string Username { get; set; } = null!;
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
    }

}