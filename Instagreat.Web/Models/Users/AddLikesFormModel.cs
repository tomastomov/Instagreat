namespace Instagreat.Web.Models.Users
{
    using System.ComponentModel.DataAnnotations;
    using Common.Constants;

    public class AddLikesFormModel
    {
        [Required]
        [MinLength(ValidationConstants.MIN_USERNAME_LENGTH)]
        [MaxLength(ValidationConstants.MAX_USERNAME_LENGTH)]
        public string Username { get; set; }

        [Required]
        public int PostId { get; set; }

        public string TypeToLike { get; set; }
    }
}
