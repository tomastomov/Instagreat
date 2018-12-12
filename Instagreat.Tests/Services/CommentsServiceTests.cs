namespace Instagreat.Tests.Services
{
    using Data;
    using Data.Models;
    using Instagreat.Services.Implementation;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class CommentsServiceTests
    {
        private readonly InstagreatDbContext db;

        public CommentsServiceTests()
        {
            var options = new DbContextOptionsBuilder<InstagreatDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            this.db = new InstagreatDbContext(options);
        }

        //AddCommentAsync Tests

        [Fact]
        public async Task AddCommentShouldReturnTrueIfAllDataIsValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.AddCommentAsync("Test", "Gosho", 1);

            Assert.True(result);
        }

        [Fact]
        public async Task AddCommentShouldReturnFalseIfContentLengthIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.AddCommentAsync(" ", "Gosho", 1);

            Assert.False(result);
        }

        [Fact]
        public async Task AddCommentShouldReturnFalseIfUsernameIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.AddCommentAsync("Test", "Pesho", 1);

            Assert.False(result);
        }

        [Fact]
        public async Task AddCommentShouldReturnFalseIfPostIdIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.AddCommentAsync("Test", "Gosho", 2);

            Assert.False(result);
        }

        //DeleteCommentAsync Tests

        [Fact]
        public async Task DeleteCommentShouldReturnTrueIfAllDataIsValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var comment = new Comment
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Comments.AddAsync(comment);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.DeleteCommentAsync(1, "1");

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteCommentShouldReturnFalseIfCommentIdIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var comment = new Comment
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Comments.AddAsync(comment);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.DeleteCommentAsync(2, "1");

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteCommentShouldReturnFalseIfTheUserIsNotAnAuthorOfTheComment()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var comment = new Comment
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Comments.AddAsync(comment);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.DeleteCommentAsync(1, "2");

            Assert.False(result);
        }

        //DeleteCommentAdminAsync Tests

        [Fact]
        public async Task DeleteCommentAdminShouldReturnIfAllDataIsValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var comment = new Comment
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Comments.AddAsync(comment);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.DeleteCommentAdminAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteCommentAdminShouldReturnIfCommentIdIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var comment = new Comment
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Comments.AddAsync(comment);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.DeleteCommentAdminAsync(2);

            Assert.False(result);
        }

        //IsLikedAsync Tests

        [Fact]
        public async Task IsLikedShouldReturnTrueIfAllDataIsValidForACommentType()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var comment = new Comment
            {
                Id = 1,
                UserId = "1"
            };

            var commentLike = new UserCommentLikes
            {
                UserId = "1",
                CommentId = 1
            };

            await this.db.Users.AddAsync(user);
            await this.db.Comments.AddAsync(comment);
            await this.db.CommentLikes.AddAsync(commentLike);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.IsLikedAsync("1", 1, "comment");

            Assert.True(result);
        }

        [Fact]
        public async Task IsLikedShouldReturnFalseIfUserIdIsNotValidForACommentType()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var comment = new Comment
            {
                Id = 1,
                UserId = "1"
            };

            var commentLike = new UserCommentLikes
            {
                UserId = "1",
                CommentId = 1
            };

            await this.db.Users.AddAsync(user);
            await this.db.Comments.AddAsync(comment);
            await this.db.CommentLikes.AddAsync(commentLike);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.IsLikedAsync("2", 1, "comment");

            Assert.False(result);
        }

        [Fact]
        public async Task IsLikedShouldReturnFalseIfCommentIdIsNotValidForACommentType()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var comment = new Comment
            {
                Id = 1,
                UserId = "1"
            };

            var commentLike = new UserCommentLikes
            {
                UserId = "1",
                CommentId = 1
            };

            await this.db.Users.AddAsync(user);
            await this.db.Comments.AddAsync(comment);
            await this.db.CommentLikes.AddAsync(commentLike);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.IsLikedAsync("1", 2, "comment");

            Assert.False(result);
        }

        [Fact]
        public async Task IsLikedShouldReturnFalseIfTypeToLikeIsNotValidForACommentType()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var comment = new Comment

            {
                Id = 1,
                UserId = "1"
            };

            var commentLike = new UserCommentLikes
            {
                UserId = "1",
                CommentId = 1
            };

            await this.db.Users.AddAsync(user);
            await this.db.Comments.AddAsync(comment);
            await this.db.CommentLikes.AddAsync(commentLike);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.IsLikedAsync("1", 1, " ");

            Assert.False(result);
        }

        //AddReplyAsync Tests

        [Fact]
        public async Task AddReplyShouldReturnTrueIfAllDataIsValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var comment = new Comment
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Comments.AddAsync(comment);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.AddReplyAsync("Test", "Gosho", 1);

            Assert.True(result);
        }

        [Fact]
        public async Task AddReplyShouldReturnFalseIfContentIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var comment = new Comment
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Comments.AddAsync(comment);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.AddReplyAsync(" ", "Gosho", 1);

            Assert.False(result);
        }

        [Fact]
        public async Task AddReplyShouldReturnFalseIfUsernameIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var comment = new Comment
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Comments.AddAsync(comment);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.AddReplyAsync("Test", "Pesho", 1);

            Assert.False(result);
        }

        [Fact]
        public async Task AddReplyShouldReturnFalseIfCommentIdIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var comment = new Comment
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Comments.AddAsync(comment);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.AddReplyAsync(" ", "Gosho", 2);

            Assert.False(result);
        }

        //DeleteReplyAsync Tests

        [Fact]
        public async Task DeleteReplyShouldReturnTrueIfAllDataIsValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var reply = new Reply
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.CommentReplies.AddAsync(reply);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.DeleteReplyAsync(1, "1");

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteReplyShouldReturnFalseIfTheUserIsNotAnAuthorOfTheReply()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var reply = new Reply
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.CommentReplies.AddAsync(reply);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.DeleteReplyAsync(1, "2");

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteReplyShouldReturnFalseIfReplyIdIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var reply = new Reply
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.CommentReplies.AddAsync(reply);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.DeleteReplyAsync(1, "2");

            Assert.False(result);
        }

        //DeleteReplyAdminAsync Tests

        [Fact]
        public async Task DeleteReplyAdminShouldReturnTrueIfAllDataIsValid()
        {
            var reply = new Reply
            {
                Id = 1
            };

            await this.db.CommentReplies.AddAsync(reply);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.DeleteReplyAdminAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteReplyAdminShouldReturnFalseIfReplyIdIsNotValid()
        {
            var reply = new Reply
            {
                Id = 1
            };

            await this.db.CommentReplies.AddAsync(reply);
            await this.db.SaveChangesAsync();

            var commentsService = new CommentsService(this.db);

            var result = await commentsService.DeleteReplyAdminAsync(2);

            Assert.False(result);
        }
    }
}
