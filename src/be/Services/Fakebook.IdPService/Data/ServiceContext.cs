using Microsoft.EntityFrameworkCore;
using Fakebook.IdPService.Entity;

namespace Fakebook.IdPService.Data
{
    public class ServiceContext : DbContext
    {
        public ServiceContext(DbContextOptions<ServiceContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!; 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
