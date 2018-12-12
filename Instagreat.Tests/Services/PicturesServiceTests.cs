namespace Instagreat.Tests.Services
{
    using Data;
    using Data.Models;
    using Instagreat.Services.Implementation;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class PicturesServiceTests
    {
        private readonly InstagreatDbContext db;

        public PicturesServiceTests()
        {
            var options = new DbContextOptionsBuilder<InstagreatDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            this.db = new InstagreatDbContext(options);
        }

        //CreatePictureAsync Tests

        [Fact]
        public async Task CreatePictureShouldReturnTrueIfAllDataIsValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            await this.db.Users.AddAsync(user);
            await this.db.SaveChangesAsync();

            var picturesService = new PicturesService(this.db);

            var result = await picturesService.CreatePictureAsync(new byte[1023], "Gosho");

            Assert.True(result);
        }

        [Fact]
        public async Task CreatePictureShouldReturnFalseIfUsernameIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            await this.db.Users.AddAsync(user);
            await this.db.SaveChangesAsync();

            var picturesService = new PicturesService(this.db);

            var result = await picturesService.CreatePictureAsync(new byte[1023], "Pesho");

            Assert.False(result);
        }

        [Fact]
        public async Task CreatePictureShouldReturnFalseIfByteDataIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            await this.db.Users.AddAsync(user);
            await this.db.SaveChangesAsync();

            var picturesService = new PicturesService(this.db);

            var result = await picturesService.CreatePictureAsync(new byte[0], "Gosho");

            Assert.False(result);
        }

        //GetDefaultPictureAsync Tests

        [Fact]
        public async Task GetDefaultPictureShouldReturnThePictureIfAllDataIsValid()
        {
            var image = new Image
            {
                Id = 6008,
                Picture = new byte[4]
                {
                    0, 1, 2, 3
                }
            };

            await this.db.Images.AddAsync(image);
            await this.db.SaveChangesAsync();

            var picturesService = new PicturesService(this.db);

            var result = await picturesService.GetDefaultPictureAsync();

            Assert.Equal(image.Picture, result);
        }

        [Fact]
        public async Task GetDefaultPictureShouldThrowInvalidOperationExceptionIfTheIdIsNotValid()
        {
            var image = new Image
            {
                Id = 6008,
                Picture = new byte[4]
                {
                    0, 1, 2, 3
                }
            };

            await this.db.Images.AddAsync(image);
            await this.db.SaveChangesAsync();

            var picturesService = new PicturesService(this.db);

            await Assert.ThrowsAsync<InvalidOperationException>(() => picturesService.GetDefaultPictureAsync(2));
        }

        //GetProfilePictureAsync Tests

        [Fact]
        public async Task GetProfilePictureShouldReturnTheImageAsStringIfAllDataIsValid()
        {
            var image = new Image
            {
                Id = 1,
                Picture = new byte[4]
                {
                    0, 1, 2, 3
                }
            };

            var user = new User
            {
                Id = "1",
                UserName = "Gosho",
                ProfilePicture = image
            };

            await this.db.Images.AddAsync(image);
            await this.db.Users.AddAsync(user);
            await this.db.SaveChangesAsync();

            var picturesService = new PicturesService(this.db);

            var result = await picturesService.GetProfilePictureAsync("Gosho");

            Assert.Equal(image.ToImageString(), result);
        }

        [Fact]
        public async Task GetProfilePictureShouldThrowInvalidOperationExceptionIfUsernameIsNotValid()
        {
            var image = new Image
            {
                Id = 1,
                Picture = new byte[4]
                {
                    0, 1, 2, 3
                }
            };

            var user = new User
            {
                Id = "1",
                UserName = "Gosho",
                ProfilePicture = image
            };

            await this.db.Images.AddAsync(image);
            await this.db.Users.AddAsync(user);
            await this.db.SaveChangesAsync();

            var picturesService = new PicturesService(this.db);

            await Assert.ThrowsAsync<InvalidOperationException>(() => picturesService.GetProfilePictureAsync("Pesho"));
        }

        [Fact]
        public async Task GetProfilePictureShouldReturnNoProfilePictureYetIfTheUserHasNoProfilePicture()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho",
            };
            
            await this.db.Users.AddAsync(user);
            await this.db.SaveChangesAsync();

            var picturesService = new PicturesService(this.db);

            var result = await picturesService.GetProfilePictureAsync("Gosho");

            Assert.Equal("No profile picture yet!", result);
        }

        //SetProfilePictureAsync Tests

        [Fact]
        public async Task SetProfilePictureShouldReturnTrueIfAllDataIsValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            await this.db.Users.AddAsync(user);
            await this.db.SaveChangesAsync();

            var picturesService = new PicturesService(this.db);

            var result = await picturesService.SetProfilePictureAsync(new byte[10], "Gosho");

            Assert.True(result);
        }

        [Fact]
        public async Task SetProfilePictureShouldReturnFalseIfUsernameIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            await this.db.Users.AddAsync(user);
            await this.db.SaveChangesAsync();

            var picturesService = new PicturesService(this.db);

            var result = await picturesService.SetProfilePictureAsync(new byte[10], "Pesho");

            Assert.False(result);
        }

        [Fact]
        public async Task SetProfilePictureShouldReturnFalseIfByteDataIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            await this.db.Users.AddAsync(user);
            await this.db.SaveChangesAsync();

            var picturesService = new PicturesService(this.db);

            var result = await picturesService.SetProfilePictureAsync(new byte[0], "Gosho");

            Assert.False(result);
        }
    }
}
