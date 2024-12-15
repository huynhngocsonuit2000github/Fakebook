using Newtonsoft.Json;
using Fakebook.UserService.Data;
using Fakebook.UserService.Entity;

namespace Fakebook.UserService.DataSeeding.Models;
public class FB001_60_UserSeeder
{
    private readonly ServiceContext _dbContext = null!;

    public FB001_60_UserSeeder(ServiceContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void SeedData()
    {
        if (_dbContext.Users.Any()) return;

        // Read the seed data from the JSON file
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data/DataSeeding", "FB001-60_UserData.json");

        if (File.Exists(jsonFilePath))
        {
            var jsonData = File.ReadAllText(jsonFilePath);

            var seedData = JsonConvert.DeserializeObject<SeedData>(jsonData);

            try
            {
                Console.WriteLine("Start seeding data file: " + jsonFilePath);

                foreach (var userData in seedData!.Users)
                {
                    var user = new User
                    {
                        Id = userData.Id,
                        IsActive = userData.IsActive,
                        Firstname = userData.Firstname,
                        Lastname = userData.Lastname,
                        Username = userData.Username,
                        Email = userData.Email,
                        PasswordHash = userData.PasswordHash,
                        IsInternalUser = userData.IsInternalUser,
                        CreatedBy = "System",
                        LastModifiedBy = "System",
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now
                    };

                    _dbContext.Users.Add(user);
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
