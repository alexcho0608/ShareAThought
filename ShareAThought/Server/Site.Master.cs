﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Reflection;
using Server.Common;
using Server.Models;

namespace Server
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected const string defaultImagePath = "Images/default.gif";
        protected const string imagePath = "Images/{0}";
        protected bool isAdmin;
        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void Manage(object sender,EventArgs e)
        {
            Response.Redirect("~/Account/Manage");
        }
        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            isAdmin = false;
            if (Context.User.Identity.GetUserId() == null)
            {
                return;
            }

            ForumDbContext db = new ForumDbContext();
            User user = db.Users.Find(Context.User.Identity.GetUserId());

            if (user.Role == Role.Admin)
            {
                isAdmin = true;
            }

            if (user.Suspended)
            {
                Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return;
            }

            var username = Context.User.Identity.GetUserName();
            if (username != "")
            {
                var control = (ImageButton)this.LoginView1.FindControl("Avatar");
                string path = Server.MapPath("~" + ServerPathConstants.ImageDirectory) + username + "\\";
                DirectoryInfo dInfo = new DirectoryInfo(path);
                if (dInfo.GetFiles().Length != 0)
                {
                    var fullFilename = Directory
                        .GetFiles(path, "*", SearchOption.AllDirectories)[0];
                    string[] splits = fullFilename.Split('\\');
                    var filename = splits[splits.Length - 1];
                        
                    control.ImageUrl = String.Format(imagePath,username) + "/" + filename;
                }
                else
                {
                    control.ImageUrl = defaultImagePath;
                }
            }
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }

}