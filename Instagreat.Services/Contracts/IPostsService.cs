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
        
        Task<AllPostsServiceModel> DetailsAsync(int id);

        Task<bool> DeletePostAdminAsync(int postId);

        Task<bool> DeletePostAsync(int postId, string userId);

        Task<int> TotalPerUserAsync(string username);

        Task<int> TotalExcludingUserAsync(string username);

        Task<bool> AddLikeAsync(string username,int postId);

        Task<bool> RemoveLikeAsync(string username, int postId);

        bool IsLiked(string username, int postId);
    }
}
