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

        [Route(nameof(DeleteReply) + "/{replyId}")]
        [HttpPost]
        public async Task<IActionResult> DeleteReply(int replyId, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.comments.DeleteReplyAsync(replyId, userId);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(ControllerConstants.Index, ControllerConstants.Home);
        }
    }
}
