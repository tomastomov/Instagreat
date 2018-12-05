namespace Instagreat.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Common.Constants;
    using Services.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Data.Models;

    [Route("admin")]
    [Authorize(Roles = RolesConstants.ADMINISTRATOR_ROLE)]
    public class AdminsController : Controller
    {
        private readonly IPostsService posts;
        private readonly ICommentsService comments;
        private readonly IUsersService users;
        private readonly UserManager<User> userManager;

        public AdminsController(IPostsService posts, 
            ICommentsService comments, 
            IUsersService users, 
            UserManager<User> userManager)
        {
            this.posts = posts;
            this.comments = comments;
            this.users = users;
            this.userManager = userManager;
        }

        [Route(nameof(AllUsers))]
        public async Task<IActionResult> AllUsers()
        {
            var allUsers = await this.users.AllUsersAsync();

            return View(allUsers);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserStatus(string id, bool isActive)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = false;

            if (isActive)
            {
                success = await this.users.DeactivateUserAsync(id);
            }
            else
            {
                success = await this.users.ActivateUserAsync(id);
            }

            if (!success)
            {
                return BadRequest();
            }
            else
            {
                TempData["Success"] = "Successfully changed user status!";
            }
            
            return RedirectToAction(nameof(AllUsers));
        }

        [HttpPost]
        [Route(nameof(DeletePost) + "/{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.posts.DeletePostAdminAsync(id);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(ControllerConstants.Index, ControllerConstants.Home);
        }

        [HttpPost]
        [Route(nameof(DeleteComment) + "/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.comments.DeleteCommentAdminAsync(id);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(ControllerConstants.Index, ControllerConstants.Home);
        }
    }
}
