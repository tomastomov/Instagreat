namespace Instagreat.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Common.Constants;
    using Services.Contracts;

    [Route("admin")]
    [Authorize(Roles = RolesConstants.ADMINISTRATOR_ROLE)]
    public class AdminsController : Controller
    {
        private readonly IPostsService posts;

        public AdminsController(IPostsService posts)
        {
            this.posts = posts;
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

            var success = await this.posts.DeleteCommentAdminAsync(id);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(ControllerConstants.Index, ControllerConstants.Home);
        }
    }
}
