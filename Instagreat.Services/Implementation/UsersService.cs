namespace Instagreat.Services.Implementation
{
    using System.Threading.Tasks;
    using System.Linq;
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using Models.Users;
    using System.Collections.Generic;
    using AutoMapper.QueryableExtensions;
    using AutoMapper;

    public class UsersService : IUsersService
    {
        private readonly InstagreatDbContext db;
        private readonly IMapper mapper;

        public UsersService(InstagreatDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<bool> AddBiographyAsync(string biography, string username)
        {
            if(biography.Length <= 0 || string.IsNullOrWhiteSpace(biography))
            {
                return false;
            }

            var user = await this.db.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if(user == null)
            {
                return false;
            }

            user.Biography = biography;

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<UsersListingModel>> AllUsersAsync()
        {
            var users = await this.db.Users.ProjectTo<UsersListingModel>(mapper.ConfigurationProvider).ToListAsync();

            return users;
        }

        public async Task<bool> DeactivateUserAsync(string id)
        {
            var user = await this.db.Users.Include(u => u.Posts).FirstOrDefaultAsync(u => u.Id == id);

            if(user == null)
            {
                return false;
            }

            user.IsActive = false;

            var posts = user.Posts;

            if (posts != null)
            {
                foreach (var post in posts)
                {
                    post.IsActive = false;
                }
            }

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ActivateUserAsync(string id)
        {
            var user = await this.db.Users.Include(u => u.Posts).FirstOrDefaultAsync(u => u.Id == id);

            if(user == null)
            {
                return false;
            }

            user.IsActive = true;

            var posts = user.Posts;

            if(posts != null)
            {
                foreach (var post in posts)
                {
                    post.IsActive = true;
                }
            }
            
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<string> GetUserBiographyAsync(string username)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if(user == null)
            {
                throw new InvalidOperationException($"Invalid user with username: {username}");
            }

            return user.Biography?.ToString();
        }

        public async Task<bool> IsUserActiveAsync(string username)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if(user == null)
            {
                return false;
            }

            return user.IsActive;
        }
    }
}
