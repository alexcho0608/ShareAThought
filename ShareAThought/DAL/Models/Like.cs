using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DAL.Models
{
    public class Like
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [Required]
        public int TopicId { get; set; }

        public virtual Topic Topic { get; set; }

        [Required]
        public int Value { get; set; }

    }
}