namespace Server.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using DAL.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        private ICollection<Topic> topics;
        private ICollection<Comment> comments;
        private ICollection<Like> likes;

        public User()
        {
            this.topics = new HashSet<Topic>();
            this.comments = new HashSet<Comment>();
            this.likes = new HashSet<Like>();
        }

        public bool Suspended { get; set; }

        public Role Role { get; set; }

        [Required]
        [Range(1, 200)]
        public int Age { get; set; }

        [Required]
        public string Gender { get; set; }

        public virtual ICollection<Like> Likes
        {
            get
            {
                return this.likes;
            }
            set
            {
                this.likes = value;
            }
        }

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

        public virtual ICollection<Topic> Topics
        {
            get
            {
                return this.topics;
            }

            set
            {
                this.topics = value;
            }
        }
    }
}