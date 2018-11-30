namespace Instagreat.Web.Models.Users
{
    using System.ComponentModel.DataAnnotations;
    public class BiographyFormModel
    {
        [Required]
        public string Biography { get; set; }

        public string Username { get; set; }
    }
}
