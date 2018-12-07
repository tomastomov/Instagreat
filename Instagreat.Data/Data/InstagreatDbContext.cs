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
        public DbSet<Reply> CommentReplies { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<UserReplyLikes> ReplyLikes { get; set; }
        public DbSet<UserPostLikes> PostLikes { get; set; }
        public DbSet<UserCommentLikes> CommentLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserPostLikes>()
                .HasKey(up => new { up.PostId, up.UserId });

            builder.Entity<UserCommentLikes>()
                .HasKey(uc => new { uc.CommentId, uc.UserId });

            builder.Entity<UserReplyLikes>()
                .HasKey(ur => new { ur.ReplyId, ur.UserId });

            builder.Entity<UserReplyLikes>()
                .HasOne(ur => ur.Reply)
                .WithMany(ur => ur.UserLikes)
                .HasForeignKey(ur => ur.ReplyId);

            builder.Entity<UserReplyLikes>()
                .HasOne(ur => ur.User)
                .WithMany(ur => ur.ReplyLikes)
                .HasForeignKey(ur => ur.UserId);

            builder.Entity<UserCommentLikes>()
                .HasOne(uc => uc.Comment)
                .WithMany(uc => uc.UserLikes)
                .HasForeignKey(uc => uc.CommentId);

            builder.Entity<UserCommentLikes>()
                .HasOne(uc => uc.User)
                .WithMany(uc => uc.CommentLikes)
                .HasForeignKey(uc => uc.UserId);

            builder.Entity<UserPostLikes>()
                .HasOne(up => up.Post)
                .WithMany(up => up.UserLikes)
                .HasForeignKey(up => up.PostId);

            builder.Entity<UserPostLikes>()
                .HasOne(up => up.User)
                .WithMany(up => up.PostLikes)
                .HasForeignKey(up => up.UserId);

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

            builder.Entity<User>()
                .HasMany(u => u.CommentReplies)
                .WithOne(cr => cr.Author)
                .HasForeignKey(u => u.UserId);

            builder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(p => p.PostId);

            builder.Entity<Comment>()
                .HasMany(c => c.CommentReplies)
                .WithOne(r => r.Comment)
                .HasForeignKey(c => c.CommentId);
        }
    }
}

