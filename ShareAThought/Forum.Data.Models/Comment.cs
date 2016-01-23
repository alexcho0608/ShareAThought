namespace Forum.Data.Models
{
    using ShareAThought.Common;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        public Comment()
        {
            this.CreatedOn = DateTime.Now;
        }

        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public int ThreadId { get; set; }

        public virtual Thread Thread { get; set; }

        [Required]
        [MaxLength(ValidationConstants.CommentsContentMaxLength)]
        [MinLength(ValidationConstants.CommentsContentMinLength)]
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }


    }
}