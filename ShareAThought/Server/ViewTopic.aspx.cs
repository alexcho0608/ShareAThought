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
    using Server.Mapper;
    using ServerModel = Server.Models;
    using Server.Helper;

    public partial class ViewTopic : BasePage
    {

        protected bool isAdmin;

        protected Dictionary<string, string> cache;
        AutoMapper.Mapper mapper = MapperFactory.GetMapper();

        protected void Page_Init(object sender, EventArgs e)
        {
            cache = new Dictionary<string, string>();
            var username = User.Identity.GetUserName();
            if (username == "")
            {
                isAdmin = false;
                return;
            }

            var dtoUser = this.dbContext.Users.First(u => u.UserName == username);
            var user = mapper.Map<ServerModel.User>(dtoUser);
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

        public Topic FormViewTopic_GetItem([QueryString("id")]int? id)
        {
            var dtoTopic =  this.dbContext.Topics.FirstOrDefault(a => a.Id == id);
            var mapper = MapperFactory.GetMapper();
            var topic = mapper.Map<Models.Topic>(dtoTopic);
            return topic;
        }

        protected string getPath(string username)
        {
            if (cache.ContainsKey(username))
            {
                return cache[username];
            }
            string imgPath = null;
            string path = Server.MapPath("~" + ServerPathConstants.ImageDirectory) + username + "\\";

            imgPath = ImageHelper.GetUserAvatarOrDefault(path, username);

            cache.Add(username, imgPath);
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
            var dtoArticle = this.dbContext.Topics.Find(e.DataID);
            DAL.Models.Like like = dtoArticle.Likes.FirstOrDefault(l => l.UserId == userID);
            if (like == null)
            {
                like = new DAL.Models.Like()
                {
                    UserId = userID,
                };

                this.dbContext.Topics.Find(e.DataID).Likes.Add(like);
            }

            like.Value = e.LikeValue;
            this.dbContext.SaveChanges();

            var control = sender as LikeControl;
            control.Value = dtoArticle.Likes.Sum(l => l.Value);
            control.CurrentUserVote = e.LikeValue;

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

            var dtoComments = this.dbContext.Comments.AsQueryable();
            var comments = dtoComments.Where(c => c.TopicId == id).OrderBy("CreatedOn")
                .ToList()
                .AsQueryable()
                .Select(dtoComment => mapper.Map<ServerModel.Comment>(dtoComment));
            return comments;
        }

        public void ListViewComments_InsertItem()
        {
            var comment = new DAL.Models.Comment();
            comment.CreatedOn = DateTime.Now;
            comment.AuthorId = User.Identity.GetUserId();
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
            var dtoComment = this.dbContext.Comments.Find(id);
            var comment = mapper.Map<ServerModel.Comment>(dtoComment);
            if (comment == null)
            {
                // The item wasn't found
                ModelState.AddModelError("", String.Format("Item with id {0} was not found", id));
                return;
            }

            TryUpdateModel(comment);
            if (ModelState.IsValid)
            {
                this.dbContext.SaveChanges();
            }
        }

        public void ListViewComments_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
        }

        protected void DeleteComment(object sender, ListViewDeleteEventArgs e)
        {
            ListViewItem item = this.ListViewComments.Items[e.ItemIndex];
            int id = Convert.ToInt32((item.FindControl("IDValue") as HiddenField).Value);
            var comment = this.dbContext.Comments.Find(id);
            if(comment != null)
            {
                dbContext.Comments.Remove(comment);
                dbContext.SaveChanges();
            }
            Response.Redirect("~/ViewTopic?id=" + Request.QueryString["id"]);
        }
    }
}