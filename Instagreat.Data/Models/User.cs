namespace Instagreat.Data.Models
{ 
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;
    using Common.Constants;

    public class User : IdentityUser
    {
        public Image ProfilePicture { get; set; }

        [MinLength(ValidationConstants.MAX_BIOGRAPHY_LENGTH)]
        public string Biography { get; set; }

        public bool IsActive { get; set; } = true;

        public List<Image> Images { get; set; } = new List<Image>();

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public List<Reply> CommentReplies { get; set; } = new List<Reply>();

        public List<Post> Posts { get; set; } = new List<Post>();

        public List<UserPostLikes> PostLikes { get; set; } = new List<UserPostLikes>();

        public List<UserCommentLikes> CommentLikes { get; set; } = new List<UserCommentLikes>();

        public List<UserReplyLikes> ReplyLikes { get; set; } = new List<UserReplyLikes>();

    }
}
