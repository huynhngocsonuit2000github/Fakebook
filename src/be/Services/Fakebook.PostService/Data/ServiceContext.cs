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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>(entity =>
            {
                // Define the self-referencing relationship for ParentCommentId
                entity
                    .HasOne(c => c.ParentComment)
                    .WithMany(c => c.Replies)
                    .HasForeignKey(c => c.ParentCommentId)
                    .OnDelete(DeleteBehavior.Restrict); // Use Restrict to prevent cascade deletes
            });
        }

    }
}
