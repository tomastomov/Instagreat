namespace Instagreat.Web.Models.Comments
{
    using System.ComponentModel.DataAnnotations;
    using Common.Constants;

    public class CommentFormModel
    {
        [Required]
        [MinLength(ValidationConstants.MIN_COMMENT_LENGTH)]
        [MaxLength(ValidationConstants.MAX_COMMENT_LENGTH)]
        public string Comment { get; set; }
    }
}
