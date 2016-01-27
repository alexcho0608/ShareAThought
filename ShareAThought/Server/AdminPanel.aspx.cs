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

        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {
            var text = this.UserSearch.Text;
            var users = this.dbContext.Users.AsQueryable()
                .Select(u => new { Text = u.UserName })
                .Where(u => u.Text.StartsWith(text))
                .ToArray();
            if (users.Count() != 0)
            {
                this.ListUsersId.DataSource = users;
                this.ListUsersId.DataBind();

            }
            else
            {
                this.ListUsersId.DataSource = new [] { new { Text = "Not found" } };
                this.ListUsersId.DataBind();
            }
        }
        protected void PromoteUser(object sender, EventArgs e)
        {
            var username = this.UserSearch.Text;
            var user = this.dbContext.Users.FirstOrDefault(u => u.UserName == username);
            user.Role = Models.Role.Admin;
            this.dbContext.SaveChanges();
        }
        protected void DeleteUser(object sender, EventArgs e)
        {
            var username = this.UserSearch.Text;
            var user = this.dbContext.Users.FirstOrDefault(u => u.UserName == username);
            this.dbContext.Users.Remove(user);
            this.dbContext.SaveChanges();
        }
    }
}