namespace Instagreat.Web.Models.Users
{
    using System.Collections.Generic;

    public class AllPostsViewModel
    {
        public IEnumerable<MyPostsViewModel> AllPosts { get; set; }

        public string Comment { get; set; }
    }
}
