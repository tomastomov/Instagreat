namespace Instagreat.Services.Models
{
    using Common.Mapping;
    using Data.Models;
    using System;
    using System.Collections.Generic;

    public class AllPostsServiceModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime PublishTime { get; set; }

        public int ImageId { get; set; }

        public Image Image { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }

        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

        public IEnumerable<UserPostLikes> UserLikes { get; set; } = new List<UserPostLikes>();

    }
}
