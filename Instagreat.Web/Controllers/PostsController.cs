namespace Instagreat.Web.Controllers
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

            var successSavePicture = await this.pictures.CreatePictureAsync(imageAsBytes, userManager.GetUserName(User));

            if (!successSavePicture)
            {
                ModelState.AddModelError("Picture", "No picture uploaded!");
                return View(model);
            }

            var successUploadPost = await this.posts.CreatePostAsync(model.Description, imageAsBytes, userManager.GetUserName(User));

            return RedirectToAction(ControllerConstants.Index, ControllerConstants.Home);

        }

        [Route(nameof(PostDetails) + "/{id}")]
        public async Task<IActionResult> PostDetails(int id)
        {
            var postsData = await this.posts.DetailsAsync(id);

            var postModel = new MyPostsViewModel
            {
                Id = postsData.Id,
                Description = postsData.Description,
                Comments = postsData.Comments,
                PublishTime = postsData.PublishTime,
                Image = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(postsData.Image.Picture)),
                UserId = postsData.UserId,
                Likes = postsData.Likes,
                Username = postsData.User.UserName,
                IsLiked = this.posts.IsLiked(User.Identity.Name,id)
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
    }
}
