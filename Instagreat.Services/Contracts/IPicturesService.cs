namespace Instagreat.Services.Contracts
{
    using System.Threading.Tasks;
    using Data.Models;

    public interface IPicturesService
    {
        Task<bool> CreatePictureAsync(byte[] pictureData, string username);
        
        Task<string> GetProfilePictureAsync(string username);

        Task<bool> SetProfilePictureAsync(byte[] pictureData, string username);

        Task<byte[]> GetDefaultPicture(int id = 6008);
    }
}
