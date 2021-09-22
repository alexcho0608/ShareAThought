using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Server.BLModels;
using System.ComponentModel.DataAnnotations;
using Server.Common;
using System.Collections.Generic;

namespace Server.BLModels
{
    // You can add User data for the user by adding more properties to your User class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        private ICollection<Topic> topics;
        private ICollection<Comment> comments;

        public ApplicationUser()
        {
            this.topics = new HashSet<Topic>();
            this.comments = new HashSet<Comment>();
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

        public virtual ICollection<Topic> Threads
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




        public Role Role { get; set; }

        [Required]
        [Range(1, 200)]
        public int Age { get; set; }

        public bool Suspended { get; set; }

        [Required]
        public string Gender { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}

