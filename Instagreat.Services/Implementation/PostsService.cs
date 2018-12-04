namespace Instagreat.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Models;
    using Instagreat.Services.Models;
    using Microsoft.EntityFrameworkCore;

    public class PostsService : IPostsService
    {
        private readonly InstagreatDbContext db;

        public PostsService(InstagreatDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<AllPostsServiceModel>> AllPostsAsync(string username, int page = 1, int pageSize = 3)
        {
            var userId = await this.db.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefaultAsync();

            if(userId == null)
            {
                throw new InvalidOperationException($"No user found with name: {username}");
            }

            var posts = await this.db.Posts.Include(p => p.User).OrderByDescending(p => p.PublishTime).Where(p => p.UserId != userId).Skip((page - 1) * pageSize).Take(pageSize).ProjectTo<AllPostsServiceModel>().ToListAsync();

            if (posts == null)
            {
                throw new InvalidOperationException("No posts found!");
            }

            return posts;
        }

        public async Task<IEnumerable<AllPostsServiceModel>> AllPostsByUserAsync(string username,int page = 1, int pageSize = 3)
        {
            var userId = this.db.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefault();

            if(userId == null)
            {
                throw new InvalidOperationException($"No user with id: {userId} found!");
            }

            var posts = await this.db.Posts.OrderByDescending(p => p.PublishTime).Where(p => p.UserId == userId).Skip((page - 1) * pageSize).Take(pageSize).ProjectTo<AllPostsServiceModel>().ToListAsync();

            return posts;
        }

        public async Task<bool> CreatePostAsync(string description, byte[] imageData, string username)
        {
            if(description.Length <= 0 || imageData.Length <= 0)
            {
                return false;
            }

            var image = await this.db.Images.Where(i => i.Picture == imageData).FirstOrDefaultAsync();

            var userId = await this.db.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefaultAsync();

            if(userId == null)
            {
                return false;
            }

            var post = new Post
            {
                Description = description,
                PublishTime = DateTime.UtcNow,
                Image = image,
                UserId = userId
            };

            await this.db.Posts.AddAsync(post);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<AllPostsServiceModel> DetailsAsync(int id)
        {
            var post = await this.db.Posts.Where(p => p.Id == id).Include(p => p.Comments).ProjectTo<AllPostsServiceModel>().FirstOrDefaultAsync();
            var replies = await this.db.Comments.Include(c => c.Author).ThenInclude(c => c.ProfilePicture).Where(p => p.PostId == id).ToListAsync();
            var commentReplies = await this.db.CommentReplies.Include(c => c.Author).ThenInclude(c => c.ProfilePicture).Where(p => replies.Contains(p.Comment)).ToListAsync();

            if (post == null)
            {
                throw new InvalidOperationException($"Invalid post id: {id}");
            }

            return post;
        }

        public async Task<int> TotalPerUserAsync(string username)
        {
            var userId = await this.db.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefaultAsync();

            var postsCount = this.db.Posts.Where(u => u.UserId == userId).Count();

            return postsCount;
        }

        public async Task<int> TotalExcludingUserAsync(string username)
        {
            var userId = await this.db.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefaultAsync();

            var postsCount = this.db.Posts.Where(u => u.UserId != userId).Count();

            return postsCount;
        }
        
        public async Task<bool> DeletePostAdminAsync(int postId)
        {
            var post = await this.db.Posts.FindAsync(postId);

            if(post == null)
            {
                return false;
            }

            this.db.Posts.Remove(post);

            await this.db.SaveChangesAsync();

            return true;

        }

        public async Task<bool> DeletePostAsync(int postId, string userId)
        {
            var post = await this.db.Posts.FindAsync(postId);

            if (post == null)
            {
                return false;
            }
            else if(post.UserId != userId)
            {
                return false;
            }

            this.db.Posts.Remove(post);

            await this.db.SaveChangesAsync();

            return true;
            
        }

        public async Task<bool> AddLikeAsync(string username, int postId)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if(user == null)
            {
                return false;
            }

            var post = await this.db.Posts.FirstOrDefaultAsync(p => p.Id == postId);

            if(post == null)
            {
                return false;
            }

            if (post.Likes.Contains(user))
            {
                return false;
            }

            post.Likes.Add(user);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveLikeAsync(string username, int postId)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return false;
            }

            var post = await this.db.Posts.FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                return false;
            }

            if (!post.Likes.Contains(user))
            {
                return false;
            }

            post.Likes.Remove(user);

            await this.db.SaveChangesAsync();

            return true;
        }

        public bool IsLiked(string username, int postId)
        {
            var user = this.db.Users.FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return false;
            }

            var post = this.db.Posts.FirstOrDefault(p => p.Id == postId);

            if (post == null)
            {
                return false;
            }

            if (post.Likes.Contains(user))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
}
