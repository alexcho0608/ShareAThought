namespace Server
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Web.ModelBinding;
    using Microsoft.AspNet.Identity;
    using Server.Controls;
    using Server.Models;
    using System.Web.UI.WebControls;
    using System.Web.UI;
    using System.IO;
    using Common;

    public partial class ViewTopic : BasePage
    {

        protected bool isAdmin;

        protected Queue<bool> filter;

        protected Queue<bool> secondFilter;

        protected Dictionary<string, string> cache;


        protected void Page_Init(object sender, EventArgs e)
        {
            cache = new Dictionary<string, string>();
            filter = new Queue<bool>();
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

        protected void Page_SaveStateComplete(object sender,EventArgs e)
        {
            //if (filter.Count == 0) filter = new Queue<bool>(secondFilter);
        }
        public Topic FormViewTopic_GetItem([QueryString("id")]int? id)
        {
            return this.dbContext.Topics.FirstOrDefault(a => a.Id == id); ;
        }

        protected string getPath(string username)
        {
            if (cache.ContainsKey(username))
            {
                return cache[username];
            }

            string path = Server.MapPath("~"+ServerPathConstants.ImageDirectory) + username + "\\";
            DirectoryInfo dInfo = new DirectoryInfo(path);
            if (dInfo.GetFiles().Length == 0)
            {
                cache.Add(username, ServerPathConstants.ImageDirectory+ ServerPathConstants.DefaultName);
            }
            else
            {
                var fullFilename = Directory
                    .GetFiles(path, "*", SearchOption.AllDirectories)[0];
                string[] splits = fullFilename.Split('\\');
                var filename = splits[splits.Length - 1];
                cache.Add(username,ServerPathConstants.ImageDirectory+username+"/"+filename);
            }

            return cache[username];
        }

        protected int GetLikes(Topic item)
        {
            if (item.Likes.Count > 0)
            {
                return item.Likes.Sum(l => l.Value);
            }

            return 0;
        }

        protected void LikeControl_Like(object sender, LikeEventArgs e)
        {
            string userID = this.User.Identity.GetUserId();
            Topic article = this.dbContext.Topics.Find(e.DataID);
            Like like = article.Likes.FirstOrDefault(l => l.UserId == userID);
            if (like == null)
            {
                like = new Like()
                {
                    UserId = userID,
                };

                this.dbContext.Topics.Find(e.DataID).Likes.Add(like);
            }

            like.Value = e.LikeValue;
            this.dbContext.SaveChanges();

            var control = sender as LikeControl;
            control.Value = article.Likes.Sum(l => l.Value);
            control.CurrentUserVote = e.LikeValue;
            filter = secondFilter;
            
        }

        protected int GetCurrentUserVote(Topic item)
        {
            string userID = User.Identity.GetUserId();
            Like like = item.Likes.FirstOrDefault(l => l.UserId == userID);
            if (like == null)
            {
                return 0;
            }

            return like.Value;
        }

        public IQueryable<Server.Models.Comment> ListViewComments_GetData([QueryString("id")]int? id)
        {
            var comments = this.dbContext.Comments.AsQueryable();
            comments = comments.Where(c => c.TopicId == id).OrderBy("CreatedOn");
            return comments;
        }

        public void ListViewComments_InsertItem()
        {
            var comment = new Server.Models.Comment();
            comment.CreatedOn = DateTime.Now;
            comment.UserId = User.Identity.GetUserId();
            comment.TopicId = int.Parse(Request.QueryString["id"]);
            TryUpdateModel(comment);
            if (ModelState.IsValid)
            {
                this.dbContext.Comments.Add(comment);
                this.dbContext.SaveChanges();
            }

            Response.Redirect("~/ViewTopic?id=" + Request.QueryString["id"]);
        }

        public void ListViewComments_UpdateItem(int id)
        {
            Server.Models.Comment item = this.dbContext.Comments.Find(id);
            if (item == null)
            {
                // The item wasn't found
                ModelState.AddModelError("", String.Format("Item with id {0} was not found", id));
                return;
            }

            TryUpdateModel(item);
            if (ModelState.IsValid)
            {
                this.dbContext.SaveChanges();
            }
        }

        public void ListViewComments_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
        }

        protected void DeleteComment(object sender, EventArgs e)
        {

        }
    }
}