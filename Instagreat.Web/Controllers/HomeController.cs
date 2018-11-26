namespace Instagreat.Web.Controllers
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

    public class HomeController : Controller
    {
        private readonly IPostsService posts;
        private readonly UserManager<User> userManager;

        public HomeController(IPostsService posts, UserManager<User> userManager)
        {
            this.posts = posts;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 2)
        {
            if(page <= 0)
            {
                page = 1;
            }

            if (User.Identity.IsAuthenticated)
            {
                var username = userManager.GetUserName(User);

                var postsData = await this.posts.AllPostsAsync(username, page, pageSize);

                var postsModel = postsData.Select(p => new MyPostsViewModel
                {
                    Id = p.Id,
                    Description = p.Description,
                    Image = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(p.Image.Picture)),
                    PublishTime = p.PublishTime,
                    UserId = p.UserId,
                    User = p.User
                });

                return View(new AllPostsViewModel
                {
                    AllPosts = postsModel,
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(await this.posts.TotalExcludingUserAsync(username) / (double)pageSize)
                });
            }

            return View();
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
