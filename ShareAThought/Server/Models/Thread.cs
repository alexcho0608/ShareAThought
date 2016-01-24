namespace Server.Models
{
    using Common;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Thread
    {
        private ICollection<Comment> comments;

        public Thread()
        {
            this.CreatedOn = DateTime.Now;
            this.comments = new HashSet<Comment>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.Max)]
        [MinLength(ValidationConstants.Min)]
        public string Title { get; set; }

        public bool Suspended { get; set; }

        [Required]
        [MinLength(ValidationConstants.MinContentLength)]
        [MaxLength(ValidationConstants.MaxContentLength)]
        public String Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public Category CategoryType { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual User Author { get; set; }

        public virtual ICollection<Comment> Comments
        {
            get
            {
                return this.comments;
            }
            set
            {
                this.comments = value;
            }
        }
    }
}