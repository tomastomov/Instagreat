﻿namespace Instagreat.Web.Controllers
{
    using System.Linq;
    using System.Diagnostics;
    using Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Threading.Tasks;
    using Models.Users;
    using System;
    using Microsoft.AspNetCore.Identity;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;

    public class HomeController : Controller
    {
        private readonly IPostsService posts;
        private readonly IPicturesService pictures;
        private readonly UserManager<User> userManager;
        private const int PageSize = 3;

        public HomeController(IPostsService posts, UserManager<User> userManager, IPicturesService pictures)
        {
            this.posts = posts;
            this.userManager = userManager;
            this.pictures = pictures;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
        {
            if(page <= 0)
            {
                page = 1;
            }

            if (User.Identity.IsAuthenticated)
            {
                var username = User.Identity.Name;

                var postsData = await this.posts.AllPostsAsync(username, page, pageSize);

                var currentUser = await this.userManager.FindByNameAsync(username);

                var postsModel = postsData.Select(p => new MyPostsViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Image = pictures.GetPostPicture(p.Id),
                    PublishTime = p.PublishTime,
                    UserId = p.UserId,
                    User = p.User,
                    Likes = p.UserLikes,
                    Username = username,
                    IsLiked = this.posts.IsLiked(username, p.Id),
                    CurrentUser = currentUser
                });

                return View(new AllPostsViewModel
                {
                    AllPosts = postsModel,
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(await this.posts.TotalExcludingUserAsync(username) / (double)pageSize),
                    Username = username,
                    ProfilePicture = await this.pictures.GetProfilePictureAsync(username)
                });
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Search(string searchText)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var postsData = await this.posts.Search(searchText);

            var username = User.Identity.Name;
            var currentUser = await this.userManager.FindByNameAsync(username);
            await this.pictures.GetProfilePictureAsync(username);

            var allPosts = postsData.Select(p => new MyPostsViewModel
            {
                Id = p.Id,
                Image = this.pictures.GetPostPicture(p.Id),
                Description = p.Description,
                Likes = p.UserLikes,
                User = p.User,
                Comments = p.Comments,
                CurrentUser = currentUser
            });

            return View(new AllPostsViewModel
            {
                AllPosts = allPosts,
                CurrentPage = 1,
                TotalPages = (int)Math.Ceiling(allPosts.Count() / (double)PageSize),
                Username = username
            });
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
