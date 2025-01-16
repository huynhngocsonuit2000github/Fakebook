using Fakebook.PostService.Data;
using Fakebook.PostService.Entity;
using Newtonsoft.Json;

namespace Fakebook.PostService.DataSeeding.Models;
public class FB001_88_PostSeeder
{
    private readonly ServiceContext _dbContext = null!;

    public FB001_88_PostSeeder(ServiceContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void SeedData()
    {
        if (_dbContext.Users.Any()) return;

        // Read the seed data from the JSON file
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data/DataSeeding", "FB001-88_PostData.json");

        if (File.Exists(jsonFilePath))
        {
            var jsonData = File.ReadAllText(jsonFilePath);

            var seedData = JsonConvert.DeserializeObject<SeedData>(jsonData);

            try
            {
                Console.WriteLine("Start seeding data file: " + jsonFilePath);

                _dbContext.Users.AddRange(seedData!.Users.Select(e => new Entity.User()
                {
                    Id = e.Id,
                    Firstname = e.Firstname,
                    Lastname = e.Lastname,
                    Username = e.Username,
                    Email = e.Email,
                    Bio = string.Empty,
                    ProfilePictureUrl = string.Empty,
                    IsActive = e.IsActive,
                    IsInternalUser = e.IsInternalUser,
                    CreatedBy = "System",
                    LastModifiedBy = "System",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                }));

                _dbContext.Posts.AddRange(seedData!.Posts.Select(e => new Entity.Post()
                {
                    Id = e.Id,
                    Content = e.Content,
                    ViewMode = Enums.ViewMode.Public,
                    OwnerId = e.OwnerId,
                    CreatedBy = "System",
                    LastModifiedBy = "System",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                }));

                _dbContext.Comments.AddRange(seedData!.Comments.Select(e => new Entity.Comment()
                {
                    Id = e.Id,
                    Content = e.Content,
                    UserId = e.UserId,
                    PostId = e.PostId,
                    ParentCommentId = e.ParentCommentId,
                    CreatedBy = "System",
                    LastModifiedBy = "System",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                }));

                _dbContext.Likes.AddRange(seedData!.Likes.Select(e => new Entity.Like()
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    PostId = e.PostId,
                    CommentId = e.CommentId,
                    CreatedBy = "System",
                    LastModifiedBy = "System",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                }));

                _dbContext.Shares.AddRange(seedData!.Shares.Select(e => new Entity.Share()
                {
                    Id = e.Id,
                    PostId = e.PostId,
                    UserId = e.UserId,
                    CreatedBy = "System",
                    LastModifiedBy = "System",
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now
                }));

                _dbContext.SaveChanges();

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
