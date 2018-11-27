namespace Instagreat.Services.Contracts
{
    using System.Threading.Tasks;

    public interface ICommentsService
    {
        Task<bool> CreateCommentAsync(string content, string username, int postId);

        Task<bool> ReplyToCommentAsync(string content, string username, int commentId);

        Task<bool> DeleteCommentAdminAsync(int commentId);

        Task<bool> DeleteCommentAsync(int commentId, string userId);
    }
}
