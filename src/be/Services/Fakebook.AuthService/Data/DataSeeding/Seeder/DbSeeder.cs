using Fakebook.AuthService.Data;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System.IO;
using Fakebook.AuthService.Entity;

namespace Fakebook.AuthService.DataSeeding.Models;
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
        new FB001_51_UAMSeeder(_dbContext).SeedData();
    }
}
