namespace Instagreat.Web.Controllers
{
    using Web.Models.Comments;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Instagreat.Services.Contracts;

    [Route("api/comments")]
    public class CommentsApiController : Controller
    {
        private readonly ICommentsService comments;

        public CommentsApiController(ICommentsService comments)
        {
            this.comments = comments;
        }

        [Route(nameof(AddComment))]
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody]CrudCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.comments.AddCommentAsync(model.Comment, model.Username, model.Id);

            if (!success)
            {
                return BadRequest();
            }

            return Ok(model);
        }

        [Route(nameof(AddReply))]
        [HttpPost]
        public async Task<IActionResult> AddReply([FromBody]CrudCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.comments.AddReplyAsync(model.Comment, model.Username, model.Id);

            if (!success)
            {
                return BadRequest();
            }

            return Ok(model);
        }
    }
}
