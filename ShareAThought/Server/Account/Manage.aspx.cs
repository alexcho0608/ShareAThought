using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using Server.Models;
using System.IO;
using Server.Common;

namespace Server.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected string SuccessMessage
        {
            get;
            private set;
        }

        private bool HasPassword(ApplicationUserManager manager)
        {
            return manager.HasPassword(User.Identity.GetUserId());
        }

        public bool HasPhoneNumber { get; private set; }

        public bool TwoFactorEnabled { get; private set; }

        public bool TwoFactorBrowserRemembered { get; private set; }

        public int LoginsCount { get; set; }

        protected User FoundUser { get; set; }

        private ForumDbContext db;

        protected void Page_Init()
        {
            db = new ForumDbContext();
            var id = Context.User.Identity.GetUserId();
            FoundUser = db.Users.Find(id);
        }

        protected void Page_Load()
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            HasPhoneNumber = String.IsNullOrEmpty(manager.GetPhoneNumber(User.Identity.GetUserId()));

            // Enable this after setting up two-factor authentientication
            //PhoneNumber.Text = manager.GetPhoneNumber(User.Identity.GetUserId()) ?? String.Empty;

            TwoFactorEnabled = manager.GetTwoFactorEnabled(User.Identity.GetUserId());

            LoginsCount = manager.GetLogins(User.Identity.GetUserId()).Count;

            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            this.Username.Text = FoundUser.UserName;
            if (IsPostBack)
            {
                FoundUser.Email = this.Email.Text;
            }

            this.Email.Text = FoundUser.Email;
            //    // Determine the sections to render
            //    if (HasPassword(manager))
            //    {
            //        ChangePassword.Visible = true;
            //    }
            //    else
            //    {
            //        CreatePassword.Visible = true;
            //        ChangePassword.Visible = false;
            //    }

            //    // Render success message
            //    var message = Request.QueryString["m"];
            //    if (message != null)
            //    {
            //        // Strip the query string from action
            //        Form.Action = ResolveUrl("~/Account/Manage");

            //        SuccessMessage =
            //            message == "ChangePwdSuccess" ? "Your password has been changed."
            //            : message == "SetPwdSuccess" ? "Your password has been set."
            //            : message == "RemoveLoginSuccess" ? "The account was removed."
            //            : message == "AddPhoneNumberSuccess" ? "Phone number has been added"
            //            : message == "RemovePhoneNumberSuccess" ? "Phone number was removed"
            //            : String.Empty;
            //        successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
            //    }
            //}
        }

        protected void Edit(object send, EventArgs e)
        {
            if (FileUploadControl.HasFile && FileUploadControl.PostedFile.ContentType.Contains("image"))
            {
                string filename = FileUploadControl.FileName;
                filename = ServerPathConstants.CommonImageName + filename.Split('.').LastOrDefault();
                string path = Server.MapPath("~" + ServerPathConstants.ImageDirectory) + FoundUser.UserName + "/";
                DirectoryInfo dInfo = new DirectoryInfo(path);
                foreach(FileInfo f in dInfo.GetFiles())
                {
                    f.Delete();
                }

                FileUploadControl.SaveAs(path + filename);
            }

            db.SaveChanges();

            if (this.Password.Text != "")
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

                var result = manager.ChangePassword(FoundUser.Id, Password.Text, ConfirmPassword.Text);
                if (result.Succeeded)
                {
                    Message.Text = "Success!";
                    return;
                }

                ErrorMessage.Text = result.Errors.FirstOrDefault();
                return;
            }
            Message.Text = "Success!";
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // Remove phonenumber from user
        protected void RemovePhone_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var result = manager.SetPhoneNumber(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return;
            }
            var user = manager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                Response.Redirect("/Account/Manage?m=RemovePhoneNumberSuccess");
            }
        }

        // DisableTwoFactorAuthentication
        protected void TwoFactorDisable_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            manager.SetTwoFactorEnabled(User.Identity.GetUserId(), false);

            Response.Redirect("/Account/Manage");
        }

        //EnableTwoFactorAuthentication 
        protected void TwoFactorEnable_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            manager.SetTwoFactorEnabled(User.Identity.GetUserId(), true);

            Response.Redirect("/Account/Manage");
        }
    }
}