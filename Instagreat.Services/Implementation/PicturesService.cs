namespace Instagreat.Services.Implementation
{
    using System.Threading.Tasks;
    using Data;
    using Data.Models;
    using Contracts;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using System;
    using Instagreat.Services.Models;
    using System.Collections.Generic;

    public class PicturesService : IPicturesService
    {
        private readonly InstagreatDbContext db;

        public PicturesService(InstagreatDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> CreatePictureAsync(byte[] pictureData, string username)
        {
            if (pictureData.Length < 0)
            {
                return false;
            }

            var user = this.db.Users.Where(u => u.UserName == username).FirstOrDefault();

            if (user == null)
            {
                return false;
            }

            var image = new Image
            {
                Picture = pictureData,
                UserId = user.Id
            };
            
            user.Images.Add(image);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<byte[]> GetDefaultPictureAsync(int id = 6008)
        {
            var image = await this.db.Images.FirstOrDefaultAsync(i => i.Id == id);

            if(image == null)
            {
                throw new InvalidOperationException($"Not image with id: {id} found!");
            }

            return image.Picture;
        }
        
        public async Task<string> GetProfilePictureAsync(string username)
        {
            var user = await this.db.Users.Include(u => u.ProfilePicture).FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                throw new InvalidOperationException($"No users with username: {username} found");
            }

            var image = user.ProfilePicture;

            if(image == null)
            {
                return "No profile picture yet!";
            }
            
            return image.ToImageString();
        }

        public async Task<bool> SetProfilePictureAsync(byte[] pictureData, string username)
        {
            if (pictureData.Length < 0)
            {
                return false;
            }

            var user = await this.db.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return false;
            }

            var image = await this.db.Images.Where(i => i.Picture == pictureData).FirstOrDefaultAsync();

            if(image == null)
            {
                image = new Image
                {
                    Picture = pictureData,
                    UserId = user.Id
                };

                await this.db.Images.AddAsync(image);
            }

            user.ProfilePicture = image;

            await this.db.SaveChangesAsync();

            return true;
        }
        
    }
}
