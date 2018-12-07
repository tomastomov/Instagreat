namespace Instagreat.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using Common.Constants;

    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.MIN_COMMENT_LENGTH)]
        [MaxLength(ValidationConstants.MAX_COMMENT_LENGTH)]
        public string Content { get; set; }

        public User Author { get; set; }

        public string UserId { get; set; }

        public Post Post { get; set; }

        public int PostId { get; set; }

        public List<Comment> Replies { get; set; } = new List<Comment>();

        public List<Reply> CommentReplies { get; set; } = new List<Reply>();

        public List<UserCommentLikes> UserLikes { get; set; } = new List<UserCommentLikes>();

    }
}
