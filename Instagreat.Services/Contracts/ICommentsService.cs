namespace Instagreat.Services.Contracts
{
    using System.Threading.Tasks;

    public interface ICommentsService
    {
        Task<bool> DeleteCommentAdminAsync(int commentId);

        Task<bool> DeleteCommentAsync(int commentId, string userId);

        Task<bool> DeleteReplyAsync(int replyId, string userId);

        Task<bool> DeleteReplyAdminAsync(int replyId);

        Task<bool> AddCommentAsync(string content, string username, int postId);

        Task<bool> AddReplyAsync(string content, string username, int commentId);

        Task<bool> IsLikedAsync(string userId, int id, string typeToCheck);

    }
}
