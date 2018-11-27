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
        private readonly ICommentsService comments;

        public AdminsController(IPostsService posts, ICommentsService comments)
        {
            this.posts = posts;
            this.comments = comments;
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
