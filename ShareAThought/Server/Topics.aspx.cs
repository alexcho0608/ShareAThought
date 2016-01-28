using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.ModelBinding;
using Server.Models;
using Microsoft.AspNet.Identity;
using System.Web.UI.WebControls;
using Server.Common;

namespace Server
{
    public partial class Topics : BasePage
    {
        private bool changeDirection = false;

        protected bool isAdmin;
        public SortDirection SortDirection
        {
            get
            {
                SortDirection direction = SortDirection.Ascending;
                if (ViewState["sortdirection"] != null)
                {
                    if ((SortDirection)ViewState["sortdirection"] == SortDirection.Descending &&
                        !this.changeDirection ||
                        (SortDirection)ViewState["sortdirection"] == SortDirection.Ascending &&
                        this.changeDirection)
                    {
                        direction = SortDirection.Descending;
                    }
                }

                ViewState["sortdirection"] = direction;
                return direction;
            }
            set
            {
                ViewState["sortdirection"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
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

        protected void ListViewTopics_Sorting(object sender, ListViewSortEventArgs e)
        {
            e.Cancel = true;
            if (ViewState["OrderBy"] != null &&
                (string)ViewState["OrderBy"] == e.SortExpression)
            {
                this.changeDirection = true;
            }
            else
            {
                this.SortDirection = SortDirection.Ascending;
            }

            ViewState["OrderBy"] = e.SortExpression;
            this.ListViewTopics.DataBind();
        }

        public IQueryable<Server.Models.Topic> ListViewTopics_GetData([ViewState("OrderBy")]String OrderBy = null)
        {
            var articles = this.dbContext.Topics.AsQueryable();
            if (OrderBy != null)
            {
                switch (this.SortDirection)
                {
                    case SortDirection.Ascending:
                        articles = articles.OrderBy(OrderBy);
                        break;
                    case SortDirection.Descending:
                        articles = articles.OrderBy(OrderBy + " Descending");
                        break;
                    default:
                        articles = articles.OrderBy(OrderBy + " Descending");
                        break;
                }
                return articles;
            }
            else
            {
                articles.OrderBy("CreatedOn Descending");
            }
            string searchWord = (this.ListViewTopics.FindControl("SearchWord") as TextBox).Text;
            string searchBy = (this.ListViewTopics.FindControl("SearchBy") as DropDownList).SelectedValue;
            if (searchWord != "")
            {
                switch (searchBy)
                {
                    case SearchPatternsConstats.Username:
                        articles = articles.Where(a => a.Author.UserName.Contains(searchWord));
                        break;
                    case SearchPatternsConstats.TopicName:
                        articles = articles.Where(a => a.Title.Contains(searchWord));
                        break;
                    default:
                        articles = articles.Where(a => a.Content.Contains(searchWord));
                        break;
                }
            }

            return articles;
        }

        public IEnumerable<Category> DropDownListCategories_GetData()
        {
            return Enum.GetValues(typeof(Category)).OfType<Category>();
        }

        public void GetTopics(object sender, EventArgs e)
        {
            
        }
        public void ListViewTopics_InsertItem()
        {
            var item = new Server.Models.Topic();
            item.CreatedOn = DateTime.Now;
            item.UserId = User.Identity.GetUserId();

            TryUpdateModel(item);
            if (ModelState.IsValid)
            {
                this.dbContext.Topics.Add(item);
                this.dbContext.SaveChanges();
            }
        }

        public void ListViewTopics_Delete(object sender,ListViewDeleteEventArgs e)
        {
            ListViewItem item = this.ListViewTopics.Items[e.ItemIndex];
            int id = Convert.ToInt32((item.FindControl("IDValue") as HiddenField).Value);
            var topic = this.dbContext.Topics.Find(id);
            if(topic != null)
            {
                
                var comments = this.dbContext.Comments
                     .Where(c => c.Topic.Id == topic.Id)
                     .AsQueryable();
                var likes = this.dbContext.Likes
                     .Where(c => c.Topic.Id == topic.Id)
                     .AsQueryable();
                foreach (var c in comments)
                {
                    this.dbContext.Comments.Remove(c);
                }

                foreach(var l in likes)
                {
                    this.dbContext.Likes.Remove(l);
                }

                this.dbContext.Topics.Remove(topic);
                this.dbContext.SaveChanges();

                this.Response.Redirect("/Topics");
            }

        }
        public void ListViewTopics_UpdateItem(int id)
        {
            Server.Models.Topic item = this.dbContext.Topics.Find(id);
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
    }
}