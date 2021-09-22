namespace Server.BLModels
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Contracts;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ForumDbContext : IdentityDbContext<User>, IDbContext
    {
        public ForumDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<Comment> Comments { get; set; }

        public virtual IDbSet<Topic> Topics { get; set; }

        public virtual IDbSet<Like> Likes { get; set; }

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}