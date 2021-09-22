using DAL.Config;
using DAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ForumInitializer : DropCreateDatabaseIfModelChanges<ForumDbContext>
    {

        protected override void Seed(ForumDbContext context)
        {
            
            try
            {
                var userManager = new ApplicationUserManager(new UserStore<DAL.Models.ApplicationUser>(context));
                var user1 = new DAL.Models.ApplicationUser { UserName = "alex1", Gender = "Male", Email = "alex_stef1@mail.bg", Age = 12, Role = DAL.Models.Role.User };
                var user2 = new DAL.Models.ApplicationUser { UserName = "alex2", Gender = "Male", Email = "alex_stef2@mail.bg", Age = 12, Role = DAL.Models.Role.Admin };

                userManager.Create(user1, "123456");

                userManager.Create(user2, "123456");

                base.Seed(context);
            }
            catch(Exception e)
            {
                System.Diagnostics.Trace.TraceInformation("Error: " + e);
            }

        }
    }
}
