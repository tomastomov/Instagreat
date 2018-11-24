namespace Instagreat.Web.Controllers
{
    using System.IO;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using System.Threading.Tasks;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Users;
    using Services.Contracts;
    using System;

    [Route("users")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IPicturesService pictures;
        private readonly IPostsService posts;
        private readonly UserManager<User> userManager;

        public UsersController(IPicturesService pictures, IPostsService posts, UserManager<User> userManager)
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

            if(model.Image.Length > 0)
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

            var successUploadPost = await this.posts.CreatePost(model.Description, imageAsBytes, userManager.GetUserName(User));

            return RedirectToAction(nameof(MyPosts));
            
        }

        [Route(nameof(MyPosts))]
        public async Task<IActionResult> MyPosts()
        {
            var myPosts = await this.posts.AllPostsByUser(userManager.GetUserName(User));

            var allPosts = myPosts.Select(p => new MyPostsViewModel
            {
                Description = p.Description,
                Id = p.Id,
                Image = string.Format("data:image/jpg;base64,{0}",Convert.ToBase64String(p.Image.Picture)),
                PublishTime = p.PublishTime,
                UserId = p.UserId
            });

            return View(new AllPostsViewModel
            {
                AllPosts = allPosts
            });
        }

        [HttpPost]
        public async Task<IActionResult> Comment(CommentFormModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                TempData["InvalidComment"] = "Invalid Comment!";
                return RedirectToAction(nameof(MyPosts));
            }

            var success = await this.posts.CreateComment(model.Comment, userManager.GetUserName(User), id);

            if (!success)
            {
                return BadRequest();
            }

            TempData["SuccessfulComment"] = "You commented on the post!";

            return RedirectToAction(nameof(MyPosts));

        }

        [Route(nameof(PostDetails) + "/{id}")]
        public async Task<IActionResult> PostDetails(int id)
        {
            var postsData = await this.posts.Details(id);

            var postModel = new MyPostsViewModel
            {
                Id = postsData.Id,
                Description = postsData.Description,
                Comments = postsData.Comments,
                PublishTime = postsData.PublishTime,
                Image = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(postsData.Image.Picture)),
                UserId = postsData.UserId,
                Likes = postsData.Likes
            };

            return View(postModel);
        }

    }
}
