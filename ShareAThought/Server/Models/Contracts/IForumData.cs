namespace Server.Models.Contracts
{
    using Forum.Data.Models;

    public interface IForumData
    {
        IRepository<User> Users { get; }

        IRepository<Thread> Threads { get; }

        IRepository<Comment> Comments { get; }

        void SaveChanges();
    }
}