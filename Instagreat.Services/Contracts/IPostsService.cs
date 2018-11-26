namespace Instagreat.Services.Contracts
{
    using Instagreat.Services.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPostsService
    {
        Task<bool> CreatePost(string description, byte[] imageData, string username);

        Task<IEnumerable<AllPostsServiceModel>> AllPostsByUser(string username, int page = 1, int pageSize = 3);

        Task<IEnumerable<AllPostsServiceModel>> AllPosts(string username, int page = 1, int pageSize = 3);

        Task<bool> CreateComment(string content, string username, int postId);

        Task<AllPostsServiceModel> Details(int id);

        Task<int> TotalPerUser(string username);

        Task<int> TotalExcludingUser(string username);
    }
}
