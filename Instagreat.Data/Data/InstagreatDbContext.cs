namespace Instagreat.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Data.Models;

    public class InstagreatDbContext : IdentityDbContext<User>
    {
        public InstagreatDbContext(DbContextOptions<InstagreatDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(u => u.Images)
                .WithOne(i => i.User).
                HasForeignKey(u => u.UserId);

            builder.Entity<User>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.Author)
                .HasForeignKey(u => u.UserId);

            builder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(u => u.UserId);

            builder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(p => p.PostId);

        }
    }
}

