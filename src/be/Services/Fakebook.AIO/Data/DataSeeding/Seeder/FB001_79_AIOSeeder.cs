using Newtonsoft.Json;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Fakebook.AIO.Data;
using Fakebook.AIO.Entity;

namespace Fakebook.AIO.DataSeeding.Models;
public class FB001_79_AIOSeeder
{
    private readonly ServiceContext _dbContext = null!;

    public FB001_79_AIOSeeder(ServiceContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void SeedData()
    {
        if (_dbContext.Cases.Any()) return;

        // Read the seed data from the JSON file
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data/DataSeeding", "FB001-79_AIOData.json");

        if (File.Exists(jsonFilePath))
        {
            var jsonData = File.ReadAllText(jsonFilePath);

            var seedData = JsonConvert.DeserializeObject<SeedData>(jsonData);

            try
            {
                Console.WriteLine("Start seeding data file: " + jsonFilePath);

                foreach (var caseData in seedData!.Cases)
                {
                    // Create and insert roles
                    var cas = new Case
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = caseData.Name,
                        Description = caseData.Description,
                        JobName = caseData.JobName,
                        IsDeleted = false,
                        NumberOfSuccess = 0,
                        NumberOfFailed = 0,
                        CreatedBy = "System",
                        LastModifiedBy = "System",
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now
                    };

                    _dbContext.Cases.Add(cas);
                    _dbContext.SaveChanges();
                }
                Console.WriteLine("End seeding data file: " + jsonFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is an error while seeding data");
                Console.WriteLine(ex);
            }
        }
    }
}
