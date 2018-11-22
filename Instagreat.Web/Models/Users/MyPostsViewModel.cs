namespace Instagreat.Web.Models.Users
{
    using System;

    public class MyPostsViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime PublishTime { get; set; }

        public string Image { get; set; }

        public string UserId { get; set; }
    }
}
