namespace Instagreat.Web.Models.Users
{
    using Data.Models;
    using System;
    using System.Collections.Generic;

    public class MyPostsViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime PublishTime { get; set; }

        public string Image { get; set; }

        public string Comment { get; set; }

        public string Username { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }

        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

        public IEnumerable<User> Likes { get; set; } = new List<User>();

        public bool IsLiked { get; set; }

    }
}
