namespace Forum.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using Contracts;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public class ForumDbContext : IdentityDbContext<User>, IDbContext
    {
        public ForumDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<Comment> Comments { get; set; }

        public virtual IDbSet<Thread> Threads { get; set; }

//        public virtual IDbSet<Rating> Ratings { get; set; }

        public static ForumDbContext Create()
        {
            return new ForumDbContext();
        }

        DbEntityEntry IDbContext.Entry<T>(T entity)
        {
            return this.Entry<T>(entity);
        }

        IDbSet<T> IDbContext.Set<T>()
        {
            return this.Set<T>();
        }
    }
}