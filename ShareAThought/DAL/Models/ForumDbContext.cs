namespace DAL.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using Contracts;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ForumDbContext : IdentityDbContext<ApplicationUser>, IDbContext
    {
        public ForumDbContext()
            :base("name=ForumDBConnectionString")
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
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Topic>()
                .HasRequired(e => e.Author)
                .WithMany(e => e.Topics)
                .HasForeignKey(e => e.UserID)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Topics)
                .WithRequired(e => e.Author)
                .HasForeignKey(e => e.UserID)
                .WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);
        }
    }
}