namespace Instagreat.Web.Models.Users
{
    using Common.Constants;
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    public class SetProfilePictureFormModel
    {
        [Required]
        public IFormFile ProfilePicture { get; set; }

        [Required]
        [MinLength(ValidationConstants.MIN_USERNAME_LENGTH)]
        [MaxLength(ValidationConstants.MAX_USERNAME_LENGTH)]
        public string Username { get; set; }
    }
}
