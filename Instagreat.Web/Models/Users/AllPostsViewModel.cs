namespace Instagreat.Web.Models.Users
{
    using System.Collections.Generic;

    public class AllPostsViewModel
    {
        public IEnumerable<MyPostsViewModel> AllPosts { get; set; }

        public int CurrentPage { get; set; }

        public string Username { get; set; }

        public int TotalPages { get; set; }
        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int NextPage
            => this.CurrentPage == this.TotalPages? this.TotalPages: this.CurrentPage + 1;

        public string Comment { get; set; }
    }
}
