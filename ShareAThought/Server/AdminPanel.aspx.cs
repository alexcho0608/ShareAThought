namespace Server
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Server;
    using System.Web.Security;
    using Microsoft.AspNet.Identity;
    public partial class AdminPanel : Server.BasePage
    {
       protected bool isAdmin;

        protected void Page_Init(object sender,EventArgs e)
        {
            var username = User.Identity.GetUserName();
            if (username == "")
            {
                isAdmin = false;
                return;
            }

            var user = this.dbContext.Users.First(u => u.UserName == username);
            if (user.Role == Models.Role.Admin)
            {
                isAdmin = true;
            }

            else
            {
                isAdmin = false;
            }
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        protected void Search(object sender, EventArgs e)
        {
            var text = this.UserSearch.Text;
            var users = this.dbContext.Users.AsQueryable()
                .Select(u => new { Text = u.UserName })
                .Where(u => u.Text.StartsWith(text))
                .ToArray();
            if (users.Count() != 0)
            {
                this.ListUsersControl.DataSource = users;
                this.ListUsersControl.DataBind();

            }
            else
            {
                this.ListUsersControl.DataSource = new [] { new { Text = "Not found" } };
                this.ListUsersControl.DataBind();
            }
        }
        protected void PromoteUser(object sender, EventArgs e)
        {
            var username = this.ListUsersControl.SelectedValue;

            var user = this.dbContext.Users.FirstOrDefault(u => u.UserName == username);
            if(user == null)
            {
                this.ListUsersControl.DataSource = new[] { new { Text = "Invalid user" } };
                this.ListUsersControl.DataBind();
                return;
            }
            user.Role = Models.Role.Admin;
            this.dbContext.SaveChanges();
        }

        protected void UnsuspendUser(object sender,EventArgs e)
        {
            var username = this.ListUsersControl.SelectedValue;
            var user = this.dbContext.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                this.ListUsersControl.DataSource = new[] { new { Text = "Invalid user" } };
                this.ListUsersControl.DataBind();
                return;
            }
            user.Suspended = false;
            this.dbContext.SaveChanges();
        }
        protected void DeleteUser(object sender, EventArgs e)
        {
            var username = this.ListUsersControl.SelectedValue;
            var user = this.dbContext.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                this.ListUsersControl.DataSource = new[] { new { Text = "Invalid user" } };
                this.ListUsersControl.DataBind();
                return;
            }
            user.Suspended = true;
            this.dbContext.SaveChanges();
        }
    }
}