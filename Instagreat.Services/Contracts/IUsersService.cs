namespace Instagreat.Services.Contracts
{
    using Services.Models.Users;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUsersService
    {
        Task<string> GetUserBiographyAsync(string username);

        Task<bool> AddBiographyAsync(string biography, string username);

        Task<IEnumerable<UsersListingModel>> AllUsersAsync();

        Task<bool> DeactivateUserAsync(string id);

        Task<bool> ActivateUserAsync(string id);

        Task<bool> IsUserActiveAsync(string username);

    }
}
