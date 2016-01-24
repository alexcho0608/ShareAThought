namespace Server.Config
{
    using System.Data.Entity;
    using Models;
    using Migrations;

    public class DataConfig
    {
        public static void Initialize()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ForumDbContext, Configuration>());
            ForumDbContext.Create().Database.Initialize(true);
        }
    }
}