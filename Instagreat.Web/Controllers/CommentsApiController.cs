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

            var success = await this.comments.AddCommentAsync(model.Comment, model.Username, model.PostId);

            if (!success)
            {
                return BadRequest();
            }

            return Ok(model);
        }
    }
}
