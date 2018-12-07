namespace Instagreat.Services.Implementation
{
    using System.Threading.Tasks;
    using System.Linq;
    using Data;
    using Data.Models;
    using Contracts;
    using Microsoft.EntityFrameworkCore;
    using System;

    public class CommentsService : ICommentsService
    {
        private readonly InstagreatDbContext db;

        public CommentsService(InstagreatDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> CreateCommentAsync(string content, string username, int postId)
        {
            if (content.Length < 0)
            {
                return false;
            }

            var user = await this.db.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();

            if (user == null)
            {
                return false;
            }

            var post = this.db.Posts.Find(postId);

            if (post == null)
            {
                return false;
            }

            var comment = new Comment
            {
                Content = content,
                UserId = user.Id,
                Author = user,
                PostId = postId
            };

            await this.db.Comments.AddAsync(comment);

            post.Comments.Add(comment);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ReplyToCommentAsync(string content, string username, int commentId)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return false;
            }

            else if (content.Length <= 0)
            {
                return false;
            }

            var comment = await this.db.Comments.Where(c => c.Id == commentId).FirstOrDefaultAsync();

            if (comment == null)
            {
                return false;
            }

            var reply = new Reply
            {
                Content = content,
                UserId = user.Id,
                CommentId = comment.Id
            };

            await this.db.CommentReplies.AddAsync(reply);

            await db.SaveChangesAsync();

            return true;

        }

        public async Task<bool> DeleteCommentAdminAsync(int commentId)
        {
            var comment = await this.db.Comments.FindAsync(commentId);

            if (comment == null)
            {
                return false;
            }

            this.db.Comments.Remove(comment);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCommentAsync(int commentId, string userId)
        {
            var comment = await this.db.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                return false;
            }
            else if (comment.UserId != userId)
            {
                return false;
            }
            
            this.db.Comments.Remove(comment);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddCommentAsync(string content, string username, int postId)
        {
            var user = await this.db.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefaultAsync();

            if(user == null)
            {
                return false;
            }

            var post = await this.db.Posts.FirstOrDefaultAsync(p => p.Id == postId);

            if(post == null)
            {
                return false;
            }

            if(content.Length <= 0)
            {
                return false;
            }

            var comment = new Comment
            {
                Content = content,
                UserId = user,
                PostId = post.Id
            };

            await this.db.Comments.AddAsync(comment);

            post.Comments.Add(comment);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsLikedAsync(string userId, int id, string typeToCheck)
        {
            if(typeToCheck == "comment")
            {
                var commentLike = await this.db.CommentLikes.FirstOrDefaultAsync(c => c.UserId == userId && c.CommentId == id);

                if (commentLike == null)
                    return false;

                else
                    return true;
            }
            else
            {
                var commentReply = await this.db.ReplyLikes.FirstOrDefaultAsync(r => r.UserId == userId && r.ReplyId == id);

                if (commentReply == null)
                    return false;

                else
                    return true;
            }
            
        }
    }
}
