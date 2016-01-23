namespace Forum.Data
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using Models;

    /// <summary>
    /// Unit of work pattern. One access object for all repositories. Ensures all repositories work with the same IDbContext instance.
    /// </summary>
    public class ForumData : IForumData
    {
        private readonly IDbContext context;
        private readonly IDictionary<Type, object> repositories = new Dictionary<Type, object>();

        public ForumData(IDbContext context)
        {
            this.context = context;
        }

        public IRepository<User> Users => this.GetRepository<User>();

        public IRepository<Thread> Threads => this.GetRepository<Thread>();

        public IRepository<Comment> Comments => this.GetRepository<Comment>();


        //        public IRepository<Rating> Ratings => this.GetRepository<Rating>();

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        protected IRepository<T> GetRepository<T>()
            where T : class
        {
            var typeOfModel = typeof(T);
            if (!this.repositories.ContainsKey(typeOfModel))
            {
                var type = typeof(Repository<T>);

                this.repositories.Add(typeOfModel, Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeOfModel];
        }
    }
}