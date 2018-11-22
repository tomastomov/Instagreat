namespace Instagreat.Services.Contracts
{
    using System.Threading.Tasks;

    public interface IPicturesService
    {
        Task<bool> CreatePictureAsync(byte[] pictureData, string username);
    }
}
