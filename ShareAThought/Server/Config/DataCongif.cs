namespace Server.Config
{
    using System.Data.Entity;
    using Models;
    using DAL.Models;

    public class DataConfig
    {
        public static void Initialize()
        {
            Database.SetInitializer<ForumDbContext>(new ForumInitializer());
        }
    }
}