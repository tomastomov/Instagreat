namespace Instagreat.Data.Models
{
    public class UserPostLikes
    {
        public int PostId { get; set; }

        public Post Post { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
