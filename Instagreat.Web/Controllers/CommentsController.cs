namespace Instagreat.Web.Controllers
{
    using Common.Constants;
    using Models.Comments;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;
    using Services.Contracts;
    using Data.Models;

    [Route("comments")]
    public class CommentsController : Controller
    {
        private readonly ICommentsService comments;
        private readonly UserManager<User> userManager;

        public CommentsController(ICommentsService comments, UserManager<User> userManager)
        {
            this.comments = comments;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route(nameof(Comment) + "/{id}")]
        public async Task<IActionResult> Comment(CommentFormModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                TempData["InvalidComment"] = "Invalid Comment!";
                return RedirectToAction(ControllerConstants.Index, ControllerConstants.Home);
            }

            var success = await this.comments.CreateCommentAsync(model.Comment, userManager.GetUserName(User), id);

            if (!success)
            {
                return BadRequest();
            }

            TempData["SuccessfulComment"] = "You successfully commented on the post!";

            return RedirectToAction(ControllerConstants.Index, ControllerConstants.Home);

        }

        [HttpPost]
        [Route(nameof(Reply) + "/{commentId}")]
        public async Task<IActionResult> Reply(CommentFormModel model, int commentId)
        {
            if (!ModelState.IsValid)
            {
                TempData["InvalidComment"] = "Invalid Comment!";
                return RedirectToAction(ControllerConstants.Index, ControllerConstants.Home);
            }

            var success = await this.comments.ReplyToCommentAsync(model.Comment, userManager.GetUserName(User), commentId);

            if (!success)
            {
                return BadRequest();
            }

            TempData["SuccessfulComment"] = "You successfully replied to the comment!";

            return RedirectToAction(ControllerConstants.Index, ControllerConstants.Home);

        }

        [HttpPost]
        [Route(nameof(DeleteComment) + "/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.comments.DeleteCommentAsync(commentId, userId);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(ControllerConstants.Index, ControllerConstants.Home);
        }
    }
}
