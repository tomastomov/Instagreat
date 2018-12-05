namespace Instagreat.Services.Models.Users
{
    using Common.Constants;
    using System.ComponentModel.DataAnnotations;

    public class UsersListingModel
    {
        [Required]
        public string Id { get; set; }
        
        [MinLength(ValidationConstants.MIN_USERNAME_LENGTH)]
        [MaxLength(ValidationConstants.MAX_USERNAME_LENGTH)]
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public bool IsActive { get; set; }

    }
}
