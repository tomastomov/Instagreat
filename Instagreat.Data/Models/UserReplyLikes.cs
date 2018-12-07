namespace Instagreat.Data.Models
{
    public class UserReplyLikes
    {
        public int ReplyId { get; set; }

        public Reply Reply { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
