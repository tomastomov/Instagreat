﻿namespace Instagreat.Services.Contracts
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPicturesService
    {
        Task<bool> CreatePictureAsync(byte[] pictureData, string username);
        
        Task<string> GetProfilePictureAsync(string username);

        Task<bool> SetProfilePictureAsync(byte[] pictureData, string username);

        Task<byte[]> GetDefaultPicture(int id = 6008);
        
    }
}
