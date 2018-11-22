namespace Instagreat.Services.Implementation
{
    using System.Threading.Tasks;
    using Data;
    using Data.Models;
    using Contracts;
    using Instagreat.Data;
    using System.Linq;

    public class PicturesService : IPicturesService
    {
        private readonly InstagreatDbContext db;

        public PicturesService(InstagreatDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> CreatePictureAsync(byte[] pictureData, string username)
        {
            if (pictureData.Length < 0)
            {
                return false;
            }

            var user = this.db.Users.Where(u => u.UserName == username).FirstOrDefault();

            if (user == null)
            {
                return false;
            }

            var image = new Image
            {
                Picture = pictureData,
                UserId = user.Id
            };
            
            user.Images.Add(image);

            await this.db.SaveChangesAsync();

            return true;
        }
    }
}
