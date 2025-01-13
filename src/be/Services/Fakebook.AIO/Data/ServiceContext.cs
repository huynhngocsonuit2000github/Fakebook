using Microsoft.EntityFrameworkCore;
using Fakebook.AIO.Entity;

namespace Fakebook.AIO.Data
{
    public class ServiceContext : DbContext
    {
        public ServiceContext(DbContextOptions<ServiceContext> options) : base(options) { }

        public DbSet<Case> Cases { get; set; } = null!;
        public DbSet<Pipeline> Pipelines { get; set; } = null!;
    }
}
