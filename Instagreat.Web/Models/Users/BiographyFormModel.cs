﻿namespace Instagreat.Web.Models.Users
{
    using Common.Constants;
    using System.ComponentModel.DataAnnotations;

    public class BiographyFormModel
    {
        [Required]
        public string Biography { get; set; }

        [MinLength(ValidationConstants.MIN_USERNAME_LENGTH)]
        [MaxLength(ValidationConstants.MAX_USERNAME_LENGTH)]
        public string Username { get; set; }
    }
}
