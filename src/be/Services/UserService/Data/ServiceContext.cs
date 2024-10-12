using Microsoft.EntityFrameworkCore;
using UserService.Entity;

namespace UserService.Data
{
    public class ServiceContext : DbContext
    {
        public ServiceContext(DbContextOptions<ServiceContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
    }
}
