﻿namespace Server
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Web.ModelBinding;
    using Microsoft.AspNet.Identity;
    using Server.Controls;
    using Server.Models;

    public partial class ViewTopic : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public Topic FormViewTopic_GetItem([QueryString("id")]int? id)
        {
            return this.dbContext.Topics.FirstOrDefault(a => a.Id == id); ;
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
    }
}