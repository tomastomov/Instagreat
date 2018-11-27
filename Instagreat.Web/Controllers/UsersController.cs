namespace Instagreat.Web.Controllers
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
    using Common.Constants;

    [Route("Users")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IPostsService posts;
        private readonly UserManager<User> userManager;
        private const int PageSize = 3;

        public UsersController(IPostsService posts, UserManager<User> userManager)
        {
            this.posts = posts;
            this.userManager = userManager;
        }
        
        [Route(nameof(Profile) + "/{username}")]
        public async Task<IActionResult> Profile(string username, int page = 1)
        {
            if(page <= 0)
            {
                page = 1;
            }

            var myPosts = await this.posts.AllPostsByUserAsync(username, page, PageSize);

            var allPosts = myPosts.Select(p => new MyPostsViewModel
            {
                Description = p.Description,
                Id = p.Id,
                Image = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(p.Image.Picture)),
                PublishTime = p.PublishTime,
                UserId = p.UserId,
                User = p.User
            });

            return View(new AllPostsViewModel
            {
                AllPosts = allPosts,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(await this.posts.TotalPerUserAsync(username) / (double)PageSize),
                Username = username
            });
        }
        

    }
}
