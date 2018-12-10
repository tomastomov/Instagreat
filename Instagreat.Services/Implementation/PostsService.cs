namespace Instagreat.Services.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Models;
    using Services.Models;
    using Microsoft.EntityFrameworkCore;

    public class PostsService : IPostsService
    {
        private readonly InstagreatDbContext db;
        private readonly IMapper mapper;

        public PostsService(InstagreatDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<AllPostsServiceModel>> AllPostsAsync(string username, int page = 1, int pageSize = 3)
        {
            var userId = await this.db.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefaultAsync();

            if(userId == null)
            {
                throw new InvalidOperationException($"No user found with name: {username}");
            }

            var posts = await this.db.Posts.Include(p => p.User).OrderByDescending(p => p.PublishTime).Where(p => p.UserId != userId && p.IsActive).Skip((page - 1) * pageSize).Take(pageSize).ProjectTo<AllPostsServiceModel>(mapper.ConfigurationProvider).ToListAsync();

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

            var posts = await this.db.Posts.Include(p => p.User).ThenInclude(p => p.ProfilePicture).OrderByDescending(p => p.PublishTime).Where(p => p.UserId == userId).Skip((page - 1) * pageSize).Take(pageSize).ProjectTo<AllPostsServiceModel>(mapper.ConfigurationProvider).ToListAsync();

            return posts;
        }

        public async Task<bool> CreatePostAsync(string description, byte[] imageData, string username)
        {
            if(description.Length <= 0 || imageData.Length <= 0 || description.Length > 200 || string.IsNullOrWhiteSpace(description))
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
                UserId = userId,
                IsActive = true
            };

            await this.db.Posts.AddAsync(post);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<AllPostsServiceModel> DetailsAsync(int id)
        {
            var post = await this.db.Posts.Where(p => p.Id == id).Include(p => p.Comments).ProjectTo<AllPostsServiceModel>(mapper.ConfigurationProvider).FirstOrDefaultAsync();
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

            if(userId == null)
            {
                return 0;
            }

            var postsCount = this.db.Posts.Where(u => u.UserId == userId).Count();

            return postsCount;
        }

        public async Task<int> TotalExcludingUserAsync(string username)
        {
            var userId = await this.db.Users.Where(u => u.UserName == username).Select(u => u.Id).FirstOrDefaultAsync();

            if(userId == null)
            {
                return 0;
            }

            var postsCount = this.db.Posts.Where(p => p.UserId != userId && p.IsActive).Count();

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

        public async Task<bool> AddLikeAsync(string username, int id, string typeToLike)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.UserName == username);
                        
            dynamic entity = null;
            dynamic userLike = null;

            if(user == null)
            {
                return false;
            }

            switch (typeToLike)
            {
                case "like":
                    entity = await this.db.Posts.FirstOrDefaultAsync(p => p.Id == id);
                    userLike = await this.db.PostLikes.Where(pl => pl.UserId == user.Id && pl.PostId == id).FirstOrDefaultAsync();

                    if (userLike != null)
                        return false;
                    else
                    {
                        userLike = new UserPostLikes
                        {
                            UserId = user.Id,
                            PostId = id
                        };
                    }
                    break;
                case "comment":
                    entity = await this.db.Comments.FirstOrDefaultAsync(c => c.Id == id);
                    userLike = await this.db.CommentLikes.Where(pl => pl.UserId == user.Id && pl.CommentId == id).FirstOrDefaultAsync();

                    if (userLike != null)
                        return false;
                    else
                    {
                        userLike = new UserCommentLikes
                        {
                            UserId = user.Id,
                            CommentId = id
                        };
                    }
                    break;
                case "reply":
                    entity = await this.db.CommentReplies.FirstOrDefaultAsync(cr => cr.Id == id);
                    userLike = await this.db.ReplyLikes.Where(pl => pl.UserId == user.Id && pl.ReplyId == id).FirstOrDefaultAsync();

                    if (userLike != null)
                        return false;
                    else
                    {
                        userLike = new UserReplyLikes
                        {
                            UserId = user.Id,
                            ReplyId = id
                        };
                    }
                    break;
                default:
                    return false;
            }
            
            if(entity == null)
            {
                return false;
            }
            
            entity.UserLikes.Add(userLike);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveLikeAsync(string username, int id, string typeToLike)
        {
            var user = await this.db.Users.FirstOrDefaultAsync(u => u.UserName == username);

            dynamic entity = null;
            dynamic userLike = null;

            if (user == null)
            {
                return false;
            }

            switch (typeToLike)
            {
                case "like":
                    entity = await this.db.Posts.FirstOrDefaultAsync(p => p.Id == id);
                    userLike = await this.db.PostLikes.Where(c => c.UserId == user.Id && c.PostId == id).FirstOrDefaultAsync();
                    break;
                case "comment":
                    entity = await this.db.Comments.FirstOrDefaultAsync(c => c.Id == id);
                    userLike = await this.db.CommentLikes.Where(c => c.UserId == user.Id && c.CommentId == id).FirstOrDefaultAsync();
                    break;
                case "reply":
                    entity = await this.db.CommentReplies.FirstOrDefaultAsync(cr => cr.Id == id);
                    userLike = await this.db.ReplyLikes.Where(r => r.UserId == user.Id && r.ReplyId == id).FirstOrDefaultAsync();
                    break;
                default:
                    return false;
            }

            if(user == null)
            {
                return false;
            }

            if (entity == null)
            {
                return false;
            }

            if(userLike == null)
            {
                return false;
            }
            
            entity.UserLikes.Remove(userLike);

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

            if(post == null)
            {
                return false;
            }

            var postLike = this.db.PostLikes.FirstOrDefault(p => p.UserId == user.Id && p.PostId == postId);

            if (post.UserLikes.Contains(postLike))
            {
                return true;
            }
            else
            {
                return false;
            };
        }
    }
}
