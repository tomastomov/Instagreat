namespace Instagreat.Tests.Services
{
    using System;
    using System.Threading.Tasks;
    using Data;
    using Data.Models;
    using Instagreat.Services.Implementation;
    using Microsoft.EntityFrameworkCore;
    using Xunit;
    using AutoMapper;
    using Web.Infrastructure.Mapping;
    using System.Collections.Generic;

    public class UsersServiceTests
    {
        private readonly InstagreatDbContext db;

        public UsersServiceTests()
        {
            var options = new DbContextOptionsBuilder<InstagreatDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            this.db = new InstagreatDbContext(options);
        }

        //AllUsersAsync Tests

        [Fact]
        public async Task AllUsersAsyncShouldReturnAllUsersIfAllDataIsValid()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            var mapper = mockMapper.CreateMapper();

            var users = new List<User>()
            {
                new User
                {
                    Id = "1",
                    UserName = "Gosho"
                },
                new User
                {
                    Id = "2",
                    UserName = "Pesho"
                }
            };

            await this.db.Users.AddRangeAsync(users);
            await this.db.SaveChangesAsync();

            var usersService = new UsersService(this.db, mapper);

            var result = await usersService.AllUsersAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
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

        //ActivateUserAsync Tests

        [Fact]
        public async Task ActivateUserShouldReturnTrueIfAllDataIsValid()
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

            var result = await usersService.ActivateUserAsync("1");

            Assert.True(result);
        }

        [Fact]
        public async Task ActivateUserShouldReturnFalseIfUserIdIsNotValid()
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

            var result = await usersService.ActivateUserAsync("2");

            Assert.False(result);
        }

        //DeactivateUserAsync Tests

        [Fact]
        public async Task DeactivateUserShouldReturnTrueIfAllDataIsValid()
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

            var result = await usersService.DeactivateUserAsync("1");

            Assert.True(result);
        }

        [Fact]
        public async Task DeactivateUserShouldReturnFalseIfUserIdIsNotValid()
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

            var result = await usersService.DeactivateUserAsync("2");

            Assert.False(result);
        }
    }
}
