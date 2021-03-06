﻿namespace Instagreat.Web.Controllers
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Users;
    using Services.Contracts;
    using System;

    [Route("Users")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IPostsService posts;
        private readonly IPicturesService pictures;
        private readonly IUsersService users;
        private readonly UserManager<User> userManager;
        private const int PageSize = 10;

        public UsersController(IPostsService posts, UserManager<User> userManager, IUsersService users, IPicturesService pictures)
        {
            this.posts = posts;
            this.userManager = userManager;
            this.users = users;
            this.pictures = pictures;
        }
        
        [Route(nameof(Profile) + "/{username}")]
        public async Task<IActionResult> Profile(string username, int page = 1)
        {
            if(page <= 0)
            {
                page = 1;
            }

            var isActive = await this.users.IsUserActiveAsync(username);
            var currentUser = await this.userManager.FindByNameAsync(User.Identity.Name);
            await this.pictures.GetProfilePictureAsync(currentUser.UserName);

            if (!isActive)
            {
                return View("Deactivated");
            }

            var myPosts = await this.posts.AllPostsByUserAsync(username, page, PageSize);

            var allPosts = myPosts.Select(p => new MyPostsViewModel
            {
                Description = p.Description,
                Id = p.Id,
                Image = this.pictures.GetPostPicture(p.Id),
                PublishTime = p.PublishTime,
                UserId = p.UserId,
                User = p.User,
                Username = username,
                Likes = p.UserLikes,
                IsLiked = this.posts.IsLiked(username, p.Id),
                CurrentUser = currentUser
            });
            
            return View(new AllPostsViewModel
            {
                AllPosts = allPosts,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(await this.posts.TotalPerUserAsync(username) / (double)PageSize),
                Username = username,
                Biography = await this.users.GetUserBiographyAsync(username),
                
                ProfilePicture = await this.pictures.GetProfilePictureAsync(username),
            });
        }

        [HttpPost]
        [Route(nameof(AddBiography) + "/{username}")]
        public async Task<IActionResult> AddBiography(BiographyFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.users.AddBiographyAsync(model.Biography, model.Username);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Profile) + $"/{model.Username}");
        }

        [HttpPost]
        [Route(nameof(SetProfilePicture))]
        public async Task<IActionResult> SetProfilePicture(SetProfilePictureFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if(User.Identity.Name != model.Username)
            {
                return Unauthorized();
            }

            byte[] imageAsBytes = new byte[model.ProfilePicture.Length + 1];

            if (model.ProfilePicture.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await model.ProfilePicture.CopyToAsync(stream);
                    imageAsBytes = stream.ToArray();
                }
            }

            var success = await this.pictures.SetProfilePictureAsync(imageAsBytes, model.Username);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Profile) + $"/{model.Username}");

        }
        

    }
}
