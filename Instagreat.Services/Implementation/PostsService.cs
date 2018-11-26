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

        public async Task<IEnumerable<AllPostsServiceModel>> AllPosts(string username, int page = 1, int pageSize = 3)
        {
            var userId = await this.db.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefaultAsync();

            if(userId == null)
            {
                throw new InvalidOperationException($"No user found with name: {username}");
            }

            var posts = await this.db.Posts.OrderByDescending(p => p.PublishTime).Where(p => p.UserId != userId).Skip((page - 1) * pageSize).Take(pageSize).ProjectTo<AllPostsServiceModel>().ToListAsync();

            if (posts == null)
            {
                throw new InvalidOperationException("No posts found!");
            }

            return posts;
        }

        public async Task<IEnumerable<AllPostsServiceModel>> AllPostsByUser(string username,int page = 1, int pageSize = 3)
        {
            var userId = this.db.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefault();

            if(userId == null)
            {
                throw new InvalidOperationException($"No user with id: {userId} found!");
            }

            var posts = await this.db.Posts.OrderByDescending(p => p.PublishTime).Where(p => p.UserId == userId).Skip((page - 1) * pageSize).Take(pageSize).ProjectTo<AllPostsServiceModel>().ToListAsync();

            return posts;
        }

        public async Task<bool> CreateComment(string content, string username, int postId)
        {
            if(content.Length < 0)
            {
                return false;
            }

            var user = await this.db.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();

            if(user == null)
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

        public async Task<bool> CreatePost(string description, byte[] imageData, string username)
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

        public async Task<AllPostsServiceModel> Details(int id)
        {
            var post = await this.db.Posts.Where(p => p.Id == id).Include(p => p.Comments).ProjectTo<AllPostsServiceModel>().FirstOrDefaultAsync();
            var comments = await this.db.Comments.Include(c => c.Author).Where(c => c.PostId == id).ToListAsync();

            if (post == null)
            {
                throw new InvalidOperationException($"Invalid post id: {id}");
            }

            return post;
        }

        public async Task<int> TotalPerUser(string username)
        {
            var userId = await this.db.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefaultAsync();

            var postsCount = this.db.Posts.Where(u => u.UserId == userId).Count();

            return postsCount;
        }

        public async Task<int> TotalExcludingUser(string username)
        {
            var userId = await this.db.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefaultAsync();

            var postsCount = this.db.Posts.Where(u => u.UserId != userId).Count();

            return postsCount;
        }
    }
}
