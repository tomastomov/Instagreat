namespace Instagreat.Tests
{
    using System;
    using System.Threading.Tasks;
    using Data;
    using Data.Models;
    using Services.Implementation;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class UsersServiceTests
    {
        private readonly InstagreatDbContext db;

        public UsersServiceTests()
        {
            var options = new DbContextOptionsBuilder<InstagreatDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            this.db = new InstagreatDbContext(options);
        }

        //AddBiograpyAsync Tests

        [Fact]
        public async Task AddBiographyShouldReturnTrueIfAllDataIsValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            await this.db.AddAsync(user);
            await this.db.SaveChangesAsync();

            var usersService = new UsersService(this.db, null);

            var result = await usersService.AddBiographyAsync("I'm test biography", "Gosho");

            Assert.True(result);
        }

        [Fact]
        public async Task AddBiographyShouldReturnFalseIfUsernameIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            await this.db.AddAsync(user);
            await this.db.SaveChangesAsync();

            var usersService = new UsersService(this.db, null);

            var result = await usersService.AddBiographyAsync("I'm a test biography", "Pesho");

            Assert.False(result);
        }

        [Fact]
        public async Task AddBiographyShouldReturnFalseIfBiographyIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            await this.db.AddAsync(user);
            await this.db.SaveChangesAsync();

            var usersService = new UsersService(this.db, null);

            var result = await usersService.AddBiographyAsync(" ", "Pesho");

            Assert.False(result);
        }

        //IsUserActiveAsync Tests

        [Fact]
        public async Task IsUserActiveShouldReturnTrueIfAllDataIsValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho",
                IsActive = true
            };

            await this.db.Users.AddAsync(user);
            await this.db.SaveChangesAsync();

            var usersService = new UsersService(this.db, null);

            var result = await usersService.IsUserActiveAsync("Gosho");

            Assert.True(result);
        }

        [Fact]
        public async Task IsUserActiveShouldReturnFalseIfUserIsNotActive()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho",
                IsActive = false
            };

            await this.db.Users.AddAsync(user);
            await this.db.SaveChangesAsync();

            var usersService = new UsersService(this.db, null);

            var result = await usersService.IsUserActiveAsync("Gosho");

            Assert.False(result);
        }

        [Fact]
        public async Task IsUserActiveShouldReturnFalseIfUsernameIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho",
                IsActive = false
            };

            await this.db.Users.AddAsync(user);
            await this.db.SaveChangesAsync();

            var usersService = new UsersService(this.db, null);

            var result = await usersService.IsUserActiveAsync("Pesho");

            Assert.False(result);
        }

        //GetBiographyAsync Tests

        [Fact]
        public async Task GetBiographyShouldReturnTheBiographyIfAllDataIsValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho",
                Biography = "I'm a test"
            };

            await this.db.Users.AddAsync(user);
            await this.db.SaveChangesAsync();

            var usersService = new UsersService(this.db, null);

            var result = await usersService.GetUserBiographyAsync("Gosho");

            Assert.Equal(user.Biography, result);
        }

        [Fact]
        public async Task GetBiographyShouldThrowExceptionIfUsernameIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho",
                Biography = "I'm a test"
            };

            await this.db.Users.AddAsync(user);
            await this.db.SaveChangesAsync();

            var usersService = new UsersService(this.db, null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => usersService.GetUserBiographyAsync("Pesho"));
        }

    }
}
