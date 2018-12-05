namespace Instagreat.Data.Models
{
    using Common.Constants;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Post
    {
        public int Id { get; set; }

        [MaxLength(ValidationConstants.MAX_POST_DESCRIPTION_LENGTH)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime PublishTime { get; set; }

        public Image Image { get; set; }

        public int ImageId { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public ISet<User> Likes { get; set; } = new HashSet<User>();

    }
}
