namespace Instagreat.Tests.Services
{
    using Data;
    using Data.Models;
    using Instagreat.Services.Implementation;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;
    using System.Linq;
    using System;
    using AutoMapper;
    using Web.Infrastructure.Mapping;

    public class PostsServiceTests
    {
        private readonly InstagreatDbContext db;
        public PostsServiceTests()
        {
            var options = new DbContextOptionsBuilder<InstagreatDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            this.db = new InstagreatDbContext(options);
        }

        //AllPostsAsync Tests

        [Fact]
        public async Task AllPostsShouldReturnAllPostsExceptForTheOnesThatAreByTheGivenUsername()
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
                    UserName = "Gosho",
                    IsActive = true
                },
                new User
                {
                    Id = "2",
                    UserName = "Pesho",
                    IsActive = true
                }
            };

            await this.db.AddRangeAsync(users);

            var posts = new List<Post>()
            {
                new Post
                {
                    Id = 1,
                    UserId = "2",
                    IsActive = true,
                    PublishTime = new DateTime(2018,10,5)
        },
                new Post
                {
                    Id = 2,
                    UserId = "1",
                    IsActive = true,
                    PublishTime = new DateTime(2018,1,3)
        },
                new Post
                {
                    Id = 3,
                    UserId = "1",
                    IsActive = true,
                    PublishTime = new DateTime(2018,10,5)
                }
            };

            await db.Posts.AddRangeAsync(posts);

            await db.SaveChangesAsync();

            var postsService = new PostsService(db, mapper);

            var result = await postsService.AllPostsAsync("Gosho", 1, 3);

            Assert.Single(result);
        }

        [Fact]
        public async Task AllPostsShouldThrowInvalidOperationExceptionIfTheUsernameIsNotValid()
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
                    UserName = "Gosho",
                    IsActive = true
                },
                new User
                {
                    Id = "2",
                    UserName = "Pesho",
                    IsActive = true
                }
            };

            await this.db.AddRangeAsync(users);

            var posts = new List<Post>()
            {
                new Post
                {
                    Id = 1,
                    UserId = "2",
                    IsActive = true,
                    PublishTime = new DateTime(2018,10,5)
        },
                new Post
                {
                    Id = 2,
                    UserId = "1",
                    IsActive = true,
                    PublishTime = new DateTime(2018,1,3)
        },
                new Post
                {
                    Id = 3,
                    UserId = "1",
                    IsActive = true,
                    PublishTime = new DateTime(2018,10,5)
                }
            };

            await db.Posts.AddRangeAsync(posts);

            await db.SaveChangesAsync();

            var postsService = new PostsService(db, mapper);

            await Assert.ThrowsAsync<InvalidOperationException>(() => postsService.AllPostsAsync("Tosho", 1, 3));
        }

        //AllPostsByUserAsync Tests

        [Fact]
        public async Task AllPostsByUserShouldReturnAllPostsForTheUserIfAllDataIsValid()
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
                    UserName = "Gosho",
                    IsActive = true
                },
                new User
                {
                    Id = "2",
                    UserName = "Pesho",
                    IsActive = true
                }
            };

            await this.db.AddRangeAsync(users);

            var posts = new List<Post>()
            {
                new Post
                {
                    Id = 1,
                    UserId = "1",
                    IsActive = true,
                    PublishTime = new DateTime(2018,10,5)
        },
                new Post
                {
                    Id = 2,
                    UserId = "1",
                    IsActive = true,
                    PublishTime = new DateTime(2018,1,3)
        },
                new Post
                {
                    Id = 3,
                    UserId = "2",
                    IsActive = true,
                    PublishTime = new DateTime(2018,10,5)
                }
            };

            await db.Posts.AddRangeAsync(posts);

            await db.SaveChangesAsync();

            var postsService = new PostsService(this.db, mapper);

            var result = await postsService.AllPostsByUserAsync("Gosho");

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task AllPostsByUserShouldThrowInvalidOperationExceptionIfUsernameIsNotValid()
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
                    UserName = "Gosho",
                    IsActive = true
                },
                new User
                {
                    Id = "2",
                    UserName = "Pesho",
                    IsActive = true
                }
            };

            await this.db.AddRangeAsync(users);

            var posts = new List<Post>()
            {
                new Post
                {
                    Id = 1,
                    UserId = "1",
                    IsActive = true,
                    PublishTime = new DateTime(2018,10,5)
        },
                new Post
                {
                    Id = 2,
                    UserId = "1",
                    IsActive = true,
                    PublishTime = new DateTime(2018,1,3)
        },
                new Post
                {
                    Id = 3,
                    UserId = "2",
                    IsActive = true,
                    PublishTime = new DateTime(2018,10,5)
                }
            };

            await db.Posts.AddRangeAsync(posts);

            await db.SaveChangesAsync();

            var postsService = new PostsService(this.db, mapper);

            await Assert.ThrowsAsync<InvalidOperationException>(() => postsService.AllPostsByUserAsync("Tosho"));
        }

        //DetailsAsync Tests

        [Fact]
        public async Task DetailsShouldReturnTheDetailsIfAllDataIsValid()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            var comment = new Comment
            {
                Id = 1,
                PostId = 1,
                Content = "GoshoComment",
                UserId = "1"
            };

            var reply = new Reply
            {
                Id = 1,
                CommentId = 1,
                Content = "GoshoReply"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.Comments.AddAsync(comment);
            await this.db.CommentReplies.AddAsync(reply);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, mapper);

            var result = await postsService.DetailsAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DetailsShouldThrowInvalidOperationExceptionIfThePostIdIsNotValid()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            var comment = new Comment
            {
                Id = 1,
                PostId = 1,
                Content = "GoshoComment",
                UserId = "1"
            };

            var reply = new Reply
            {
                Id = 1,
                CommentId = 1,
                Content = "GoshoReply"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.Comments.AddAsync(comment);
            await this.db.CommentReplies.AddAsync(reply);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, mapper);

            await Assert.ThrowsAsync<InvalidOperationException>(() => postsService.DetailsAsync(2));
        }

        //CreatePostAsync Tests
        //CreatePost Tests

        [Fact]
        public async Task CreatePostShouldReturnTrueIfAllDataIsValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            await this.db.Users.AddAsync(user);

            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.CreatePostAsync("This is a test", new byte[1023], "Gosho");

            Assert.True(result);
        }

        [Fact]
        public async Task CreatePostShouldReturnFalseIfUsernameIsNotValid()
        {
            var postsService = new PostsService(this.db, null);

            var result = await postsService.CreatePostAsync("This is a test", new byte[1023], "Invalid");

            Assert.False(result);
        }

        [Fact]
        public async Task CreatePostShouldReturnFalseIfDescriptionIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            await this.db.Users.AddAsync(user);

            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.CreatePostAsync(" ", new byte[1023], "Gosho");

            Assert.False(result);
        }

        [Fact]
        public async Task CreatePostShouldReturnFalseIfImageIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            await this.db.Users.AddAsync(user);

            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.CreatePostAsync("Woaaaaw", new byte[0], "Gosho");

            Assert.False(result);
        }

        //TotalPerUserAsync Tests
        [Fact]
        public async Task TotalPerUserShouldReturnTheCountOfTheUsersPostsIfAllDataIsValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var posts = new List<Post>()
            {
                new Post
                {
                    Id = 1,
                    UserId = "1"
                },
                new Post
                {
                    Id = 2,
                    UserId = "1"
                }
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddRangeAsync(posts);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.TotalPerUserAsync("Gosho");

            Assert.Equal(2, result);

        }

        [Fact]
        public async Task TotalPerUserShouldReturnZeroPostsIfTheUsernameIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var posts = new List<Post>()
            {
                new Post
                {
                    Id = 1,
                    UserId = "1"
                },
                new Post
                {
                    Id = 2,
                    UserId = "1"
                }
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddRangeAsync(posts);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.TotalPerUserAsync("Invalid");

            Assert.Equal(0, result);

        }

        //TotalExcludingUserAsync Tests

        [Fact]
        public async Task TotalExcludingUserShouldReturnAllPostsExceptForTheGivenUserIfAllDataIsValid()
        {
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

            var posts = new List<Post>()
            {
                new Post
                {
                    Id = 1,
                    UserId = "1"
                }
                ,new Post
                {
                    Id = 2,
                    UserId = "2"
                },
                new Post
                {
                    Id = 3,
                    UserId = "2"
                }
            };

            await this.db.Users.AddRangeAsync(users);
            await this.db.Posts.AddRangeAsync(posts);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.TotalExcludingUserAsync("Gosho");

            Assert.Equal(2, result);
        }

        [Fact]
        public async Task TotalExcludingUserShouldReturnZeroIfTheUsernameIsNotValid()
        {
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

            var posts = new List<Post>()
            {
                new Post
                {
                    Id = 1,
                    UserId = "1"
                }
                ,new Post
                {
                    Id = 2,
                    UserId = "2"
                },
                new Post
                {
                    Id = 3,
                    UserId = "2"
                }
            };

            await this.db.Users.AddRangeAsync(users);
            await this.db.Posts.AddRangeAsync(posts);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.TotalExcludingUserAsync("Invalid");

            Assert.Equal(0, result);
        }

        //IsLikedAsync Tests

        [Fact]
        public async Task IsLikedShouldReturnTrueIfAllDataIsValidAndUserHasLikedThePost()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            var postLike = new UserPostLikes
            {
                PostId = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.PostLikes.AddAsync(postLike);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = postsService.IsLiked("Gosho", 1);

            Assert.True(result);
        }

        [Fact]
        public async Task IsLikedShouldReturnFalseIfUsernameIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            var postLike = new UserPostLikes
            {
                PostId = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.PostLikes.AddAsync(postLike);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = postsService.IsLiked("Invalid", 1);

            Assert.False(result);
        }

        [Fact]
        public async Task IsLikedShouldReturnFalseIfPostIdIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            var postLike = new UserPostLikes
            {
                PostId = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.PostLikes.AddAsync(postLike);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = postsService.IsLiked("Gosho", 123);

            Assert.False(result);
        }

        //DeletePostAsync Tests

        [Fact]
        public async Task DeletePostShouldReturnTrueIfAllDataIsValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.DeletePostAsync(1, "1");

            Assert.True(result);
        }

        [Fact]
        public async Task DeletePostShouldReturnFalseIfPostIdIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.DeletePostAsync(133, "1");

            Assert.False(result);
        }

        [Fact]
        public async Task DeletePostShouldReturnFalseIfTheUserIsNotACreatorOfThePost()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "2"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.DeletePostAsync(1, "1");

            Assert.False(result);
        }

        //DeletePostAdminAsync Tests

        [Fact]
        public async Task DeletePostAdminAsyncShouldReturnTrueIfAllDataIsValid()
        {
            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.AddAsync(post);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.DeletePostAdminAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DeletePostAdminShouldReturnFalseIfPostIdIsNotValid()
        {
            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.AddAsync(post);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.DeletePostAdminAsync(2);

            Assert.False(result);
        }

        //AddLikeAsync Tests

         //For Like on a post
        [Fact]
        public async Task AddLikeShouldReturnTrueIfAllDataIsValidForAPost()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };
            
            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.AddLikeAsync("Gosho", 1, "like");

            Assert.True(result);
        }

        //For like on a comment
        [Fact]
        public async Task AddLikeShouldReturnIfAllDataIsValidForAComment()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var comment = new Comment
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Comments.AddAsync(comment);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.AddLikeAsync("Gosho", 1, "comment");

            Assert.True(result);
        }

        //For like on a reply
        [Fact]
        public async Task AddLikeShouldReturnIfAllDataIsValidForAReply()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var reply = new Reply
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.CommentReplies.AddAsync(reply);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.AddLikeAsync("Gosho", 1, "reply");

            Assert.True(result);
        }

        [Fact]
        public async Task AddLikeShouldReturnFalseIfUsernameIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.AddLikeAsync("Invalid", 1, "like");

            Assert.False(result);
        }

        [Fact]
        public async Task AddLikeShouldReturnFalseIfIdIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.AddLikeAsync("Gosho", 2, "like");

            Assert.False(result);
        }

        [Fact]
        public async Task AddLikeShouldReturnFalseIfTypeToLikeIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.AddLikeAsync("Gosho", 1, "invalid");

            Assert.False(result);
        }

        [Fact]
        public async Task AddLikeShouldReturnFalseIfThePostIsAlreadyLikedByTheUser()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };
            
            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.AddLikeAsync("Gosho", 1, "like");

            var finalResult = await postsService.AddLikeAsync("Gosho", 1, "like");

            Assert.False(finalResult);
        }

        //RemoveLikeAsync Tests

        [Fact]
        public async Task RemoveLikeShouldReturnTrueIfAllDataIsValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            var postLike = new UserPostLikes
            {
                UserId = "1",
                PostId = 1
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.PostLikes.AddAsync(postLike);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.RemoveLikeAsync("Gosho", 1, "like");

            Assert.True(result);
        }

        [Fact]
        public async Task RemoveLikeShouldReturnFalseIfUsernameIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            var postLike = new UserPostLikes
            {
                UserId = "1",
                PostId = 1
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.PostLikes.AddAsync(postLike);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.RemoveLikeAsync("Invalid", 1, "like");

            Assert.False(result);
        }

        [Fact]
        public async Task RemoveLikeShouldReturnFalseIfTheIdIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            var postLike = new UserPostLikes
            {
                UserId = "1",
                PostId = 1
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.PostLikes.AddAsync(postLike);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.RemoveLikeAsync("Gosho", 2, "like");

            Assert.False(result);
        }

        [Fact]
        public async Task RemoveLikeShouldReturnFalseIfTypeToLikeIsNotValid()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };

            var postLike = new UserPostLikes
            {
                UserId = "1",
                PostId = 1
            };

            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.PostLikes.AddAsync(postLike);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.RemoveLikeAsync("Gosho", 1, "invalid");

            Assert.False(result);
        }

        [Fact]
        public async Task RemoveLikeShouldReturnFalseIfThePostIsNotLiked()
        {
            var user = new User
            {
                Id = "1",
                UserName = "Gosho"
            };

            var post = new Post
            {
                Id = 1,
                UserId = "1"
            };
            
            await this.db.Users.AddAsync(user);
            await this.db.Posts.AddAsync(post);
            await this.db.SaveChangesAsync();

            var postsService = new PostsService(this.db, null);

            var result = await postsService.RemoveLikeAsync("Gosho", 1, "like");

            Assert.False(result);
        }
    }
}
