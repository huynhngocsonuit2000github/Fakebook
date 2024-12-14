using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fakebook.UserService.Migrations
{
    public partial class RemoveUAMTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the tables directly
            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "UserRoles");

            // Drop any other related tables (like Roles) if needed
            migrationBuilder.DropTable(
                name: "Roles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
