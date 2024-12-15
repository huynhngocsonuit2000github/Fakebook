using Fakebook.UserService.Data;

namespace Fakebook.UserService.DataSeeding.Models;
public class DbSeeder
{
    private readonly ServiceContext _dbContext;

    public DbSeeder(ServiceContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void SeedData()
    {
        // Data seeding: Users, Roles, Permissions, some navigation
        new FB001_60_UserSeeder(_dbContext).SeedData();
    }
}
