using Microsoft.EntityFrameworkCore;
using Fakebook.PostService.Entity;

namespace Fakebook.PostService.Data
{
    public class ServiceContext : DbContext
    {
        public ServiceContext(DbContextOptions<ServiceContext> options) : base(options) { }

        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Like> Likes { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Share> Shares { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
    }
}
