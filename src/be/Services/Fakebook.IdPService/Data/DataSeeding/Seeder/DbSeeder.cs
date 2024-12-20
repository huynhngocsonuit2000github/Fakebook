
using Fakebook.IdPService.Data;

namespace Fakebook.IdPService.DataSeeding.Models;
public class DbSeeder
{
    private readonly ServiceContext _dbContext;

    public DbSeeder(ServiceContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void SeedData()
    {
        // Data seeding: Users
        new FB001_69_UserSeeder(_dbContext).SeedData();
    }
}
