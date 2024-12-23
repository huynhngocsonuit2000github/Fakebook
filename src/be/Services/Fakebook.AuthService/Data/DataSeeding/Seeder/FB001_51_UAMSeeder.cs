using Fakebook.AuthService.Data;
using Newtonsoft.Json;
using System.IO;
using Fakebook.AuthService.Entity;
using Microsoft.EntityFrameworkCore;

namespace Fakebook.AuthService.DataSeeding.Models;
public class FB001_51_UAMSeeder
{
    private readonly ServiceContext _dbContext = null!;

    public FB001_51_UAMSeeder(ServiceContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void SeedData()
    {
        if (_dbContext.Roles.Any()) return;

        // Read the seed data from the JSON file
        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data/DataSeeding", "FB001-51_UAMData.json");

        if (File.Exists(jsonFilePath))
        {
            var jsonData = File.ReadAllText(jsonFilePath);

            var seedData = JsonConvert.DeserializeObject<SeedData>(jsonData);

            try
            {
                Console.WriteLine("Start seeding data file: " + jsonFilePath);

                foreach (var roleData in seedData!.Roles)
                {
                    // Create and insert roles
                    var role = new Role
                    {
                        Id = roleData.Id,
                        RoleName = roleData.RoleName,
                        Description = roleData.Description,
                        CreatedBy = "System",
                        LastModifiedBy = "System",
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now
                    };

                    _dbContext.Roles.Add(role);
                    _dbContext.SaveChanges();

                    // Create and insert permissions for the role
                    foreach (var permissionData in roleData.Permissions)
                    {
                        var existingPermission = _dbContext.Permissions!.FirstOrDefault(e => string.Equals(e.PermissionName, permissionData.PermissionName, StringComparison.OrdinalIgnoreCase));

                        if (existingPermission is null)
                        {
                            existingPermission = new Permission
                            {
                                Id = permissionData.Id,
                                PermissionName = permissionData.PermissionName,
                                Description = permissionData.Description,
                                CreatedBy = "System",
                                LastModifiedBy = "System",
                                CreatedDate = DateTime.Now,
                                LastModifiedDate = DateTime.Now
                            };

                            _dbContext.Permissions!.Add(existingPermission);
                            _dbContext.SaveChanges();
                        }

                        // Assign permission to role
                        _dbContext.RolePermissions!.Add(new RolePermission
                        {
                            Id = Guid.NewGuid().ToString(),
                            RoleId = role.Id,
                            PermissionId = existingPermission.Id,
                            CreatedBy = "System",
                            LastModifiedBy = "System",
                            CreatedDate = DateTime.Now,
                            LastModifiedDate = DateTime.Now
                        });
                    }

                    _dbContext.SaveChanges();

                    // Create and insert users for the role
                    foreach (var userData in roleData.Users)
                    {
                        var user = new User
                        {
                            Id = userData.Id,
                            Firstname = userData.Firstname,
                            Lastname = userData.Lastname,
                            Username = userData.Username,
                            Email = userData.Email,
                            PasswordHash = userData.PasswordHash,
                            IsInternalUser = true,  // or false depending on the user
                            CreatedBy = "System",
                            LastModifiedBy = "System",
                            CreatedDate = DateTime.Now,
                            LastModifiedDate = DateTime.Now
                        };

                        _dbContext.Users.Add(user);
                        _dbContext.SaveChanges();

                        // Assign the role to the user
                        _dbContext.UserRoles.Add(new UserRole
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = user.Id,
                            RoleId = role.Id,
                            CreatedBy = "System",
                            LastModifiedBy = "System",
                            CreatedDate = DateTime.Now,
                            LastModifiedDate = DateTime.Now
                        });

                        _dbContext.SaveChanges();
                    }
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
