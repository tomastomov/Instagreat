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

        public async Task<IEnumerable<AllPostsServiceModel>> AllPosts(string username)
        {
            var userId = this.db.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefault();

            if(userId == null)
            {
                throw new InvalidOperationException($"No user with id: {userId} found!");
            }

            var posts = await this.db.Posts.Where(p => p.UserId == userId).ProjectTo<AllPostsServiceModel>().ToListAsync();

            return posts;
        }

        public async Task<bool> CreatePost(string description, byte[] imageData, string username)
        {
            if(description.Length <= 0 || imageData.Length <= 0)
            {
                return false;
            }

            var image = db.Images.Where(i => i.Picture == imageData).FirstOrDefault();

            var userId = db.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefault();

            if(userId == null)
            {
                return false;
            }

            var post = new Post
            {
                Description = description,
                PublishTime = DateTime.UtcNow,
                Image = image,
                UserId =  userId
            };

            await this.db.Posts.AddAsync(post);

            await this.db.SaveChangesAsync();

            return true;
        }
    }
}
