namespace Instagreat.Services.Models
{
    using Instagreat.Common.Mapping;
    using Instagreat.Data.Models;
    using System;

    public class AllPostsServiceModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime PublishTime { get; set; }

        public int ImageId { get; set; }

        public Image Image { get; set; }

        public string UserId { get; set; }
    }
}
