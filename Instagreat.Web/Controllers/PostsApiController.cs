﻿namespace Instagreat.Web.Controllers
{
    using System.Threading.Tasks;
    using Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Models.Users;
    using System;
    
    [Route("api/posts")]
    public class PostsApiController : Controller
    {
        private readonly IPostsService posts;
        private readonly IPicturesService pictures;

        public PostsApiController(IPostsService posts, IPicturesService pictures)
        {
            this.posts = posts;
            this.pictures = pictures;
        }
                
        [HttpPost]
        [Route(nameof(AddLike))]
        public async Task<IActionResult> AddLike([FromBody] AddLikesFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.posts.AddLikeAsync(model.Username, model.PostId, model.TypeToLike);

            if (!success)
            {
                return BadRequest();
            }

            return Created(new Uri("https://localhost:44382/"), model);
        }

        [HttpPost]
        [Route(nameof(RemoveLike))]
        public async Task<IActionResult> RemoveLike([FromBody] AddLikesFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.posts.RemoveLikeAsync(model.Username, model.PostId, model.TypeToLike);

            if (!success)
            {
                return BadRequest();
            }

            return Ok();
        }

    }
}
