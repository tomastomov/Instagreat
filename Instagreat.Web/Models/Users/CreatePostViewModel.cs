namespace Instagreat.Web.Models.Users
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Common.Constants;
    using Microsoft.AspNetCore.Http;
    
    public class CreatePostViewModel
    {
        [MaxLength(ValidationConstants.MAX_POST_DESCRIPTION_LENGTH)]
        [Required]
        public string Description { get; set; }

        [Required]
        [MinLength(1)]
        public IFormFile Image { get; set; }
        
    }
}
