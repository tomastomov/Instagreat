namespace Instagreat.Services.Contracts
{
    using System.Threading.Tasks;

    public interface IUsersService
    {
        Task<string> GetUserBiographyAsync(string username);

        Task<bool> AddBiographyAsync(string biography, string username);
        
    }
}
