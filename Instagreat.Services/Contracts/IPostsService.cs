namespace Instagreat.Services.Contracts
{
    using Instagreat.Services.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPostsService
    {
        Task<bool> CreatePostAsync(string description, byte[] imageData, string username);

        Task<IEnumerable<AllPostsServiceModel>> AllPostsByUserAsync(string username, int page = 1, int pageSize = 3);

        Task<IEnumerable<AllPostsServiceModel>> AllPostsAsync(string username, int page = 1, int pageSize = 3);

        Task<bool> CreateCommentAsync(string content, string username, int postId);

        Task<bool> ReplyToCommentAsync(string content, string username, int commentId);

        Task<AllPostsServiceModel> DetailsAsync(int id);

        Task<bool> DeletePostAdminAsync(int postId);

        Task<int> TotalPerUserAsync(string username);

        Task<int> TotalExcludingUserAsync(string username);
    }
}
