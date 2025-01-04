using Fakebook.AIO.Data;

namespace Fakebook.AIO.DataSeeding.Models;
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
        new FB001_79_AIOSeeder(_dbContext).SeedData();
    }
}
