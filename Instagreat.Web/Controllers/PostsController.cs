﻿namespace Instagreat.Web.Controllers
{
    using Data.Models;
    using Services.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Common.Constants;
    using Models.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;

    [Route("posts")]
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPicturesService pictures;
        private readonly IPostsService posts;
        private readonly UserManager<User> userManager;

        public PostsController(IPicturesService pictures, IPostsService posts, UserManager<User> userManager)
        {
            this.pictures = pictures;
            this.posts = posts;
            this.userManager = userManager;
        }

        [Route(nameof(Create))]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Route(nameof(Create))]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            byte[] imageAsBytes = new byte[model.Image.Length + 1];

            if (model.Image.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await model.Image.CopyToAsync(stream);
                    imageAsBytes = stream.ToArray();
                }
            }

            var isPicture = await IsImageType(model.Image);

            if (!isPicture)
            {
                ModelState.AddModelError("Image", "Please choose between jpeg, jpg or png format.");
                return View(model);
            }

            var successSavePicture = await this.pictures.CreatePictureAsync(imageAsBytes, userManager.GetUserName(User));
            
            if (!successSavePicture)
            {
                ModelState.AddModelError("Picture", "No picture uploaded!");
                return View(model);
            }

            var successUploadPost = await this.posts.CreatePostAsync(model.Description, imageAsBytes, userManager.GetUserName(User));

            if (!successUploadPost)
            {
                return BadRequest();
            }

            TempData["Success"] = "Post created!";

            return RedirectToAction(ControllerConstants.Index, ControllerConstants.Home);

        }

        [Route(nameof(PostDetails) + "/{id}")]
        public async Task<IActionResult> PostDetails(int id)
        {
            var postsData = await this.posts.DetailsAsync(id);
            var currentUsername = User.Identity.Name;
            var currentUser = await this.userManager.FindByNameAsync(currentUsername);
            await this.pictures.GetProfilePictureAsync(currentUsername);

            var postModel = new MyPostsViewModel
            {
                Id = postsData.Id,
                Description = postsData.Description,
                Comments = postsData.Comments,
                PublishTime = postsData.PublishTime,
                Image = this.pictures.GetPostPicture(postsData.Id),
                UserId = postsData.UserId,
                User = postsData.User,
                Likes = postsData.UserLikes,
                Username = postsData.User.UserName,
                IsLiked = this.posts.IsLiked(currentUsername, id),
                CurrentUser = currentUser
            };

            return View(postModel);
        }
        

        [HttpPost]
        [Route(nameof(DeletePost) + "/{postId}")]
        public async Task<IActionResult> DeletePost(int postId, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.posts.DeletePostAsync(postId, userId);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(ControllerConstants.Index, ControllerConstants.Home);
        }

        private async Task<bool> IsImageType(IFormFile image)
        {
            if(image.ContentType.Contains("image/jpg") || image.ContentType.Contains("image/jpeg") || image.ContentType.Contains("image/png"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
