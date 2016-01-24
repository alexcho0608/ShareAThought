namespace Server.Models.Contracts
{
    public interface IForumData
    {
        IRepository<User> Users { get; }

        IRepository<Topic> Topics { get; }

        IRepository<Comment> Comments { get; }

        void SaveChanges();
    }
}