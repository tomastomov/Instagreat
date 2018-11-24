namespace Instagreat.Services.Contracts
{
    using Instagreat.Services.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPostsService
    {
        Task<bool> CreatePost(string description, byte[] imageData, string username);

        Task<IEnumerable<AllPostsServiceModel>> AllPostsByUser(string username);

        Task<IEnumerable<AllPostsServiceModel>> AllPosts(string username);

        Task<bool> CreateComment(string content, string username, int postId);

        Task<AllPostsServiceModel> Details(int id);
    }
}
