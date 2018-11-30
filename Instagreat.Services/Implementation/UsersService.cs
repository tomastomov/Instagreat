﻿namespace Instagreat.Services.Implementation
{
    using System.Threading.Tasks;
    using System.Linq;
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System;

    public class UsersService : IUsersService
    {
        private readonly InstagreatDbContext db;

        public UsersService(InstagreatDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> AddBiographyAsync(string biography, string username)
        {
            if(biography.Length <= 0)
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

        public async Task<string> GetUserBiographyAsync(string username)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if(user == null)
            {
                throw new InvalidOperationException($"Invalid user with username: {username}");
            }

            return user.Biography?.ToString();
        }
    }
}
