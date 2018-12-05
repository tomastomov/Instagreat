namespace Instagreat.Data.Models
{
    using Common.Constants;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Reply
    {
        public int Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.MIN_COMMENT_LENGTH)]
        [MaxLength(ValidationConstants.MAX_COMMENT_LENGTH)]
        public string Content { get; set; }

        public User Author { get; set; }

        public string UserId { get; set; }

        public int CommentId { get; set; }

        public Comment Comment { get; set; }

        public List<User> Likes { get; set; } = new List<User>();

    }
}
