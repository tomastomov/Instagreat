﻿namespace Instagreat.Web.Models.Comments
{
    using Common.Constants;
    using System.ComponentModel.DataAnnotations;

    public class CrudCommentModel
    {
        [Required]
        [MinLength(ValidationConstants.MIN_COMMENT_LENGTH)]
        [MaxLength(ValidationConstants.MAX_COMMENT_LENGTH)]
        public string Comment { get; set; }

        [Required]
        [MinLength(ValidationConstants.MIN_USERNAME_LENGTH)]
        [MaxLength(ValidationConstants.MAX_USERNAME_LENGTH)]
        public string Username { get; set; }
        
        [Required]
        public int Id { get; set; }
    }
}
