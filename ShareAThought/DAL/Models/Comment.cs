namespace DAL.Models
{
    using DAL.Common;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        public Comment()
        {
            this.CreatedOn = DateTime.Now;
        }

        public int Id { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [Required]
        public int TopicId { get; set; }

        public virtual Topic Topic { get; set; }

        [Required]
        [MaxLength(ValidationConstants.CommentsContentMaxLength)]
        [MinLength(ValidationConstants.CommentsContentMinLength)]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}