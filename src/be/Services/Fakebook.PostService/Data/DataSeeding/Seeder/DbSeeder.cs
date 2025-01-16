
using Fakebook.PostService.Data;

namespace Fakebook.PostService.DataSeeding.Models;
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
        new FB001_88_PostSeeder(_dbContext).SeedData();
    }
}
