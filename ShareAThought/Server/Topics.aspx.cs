using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.ModelBinding;
using Server.Models;
using Microsoft.AspNet.Identity;
using System.Web.UI.WebControls;

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


            return articles;
        }

        public IEnumerable<Category> DropDownListCategories_GetData()
        {
            return Enum.GetValues(typeof(Category)).OfType<Category>();
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

        public void DeleteTopic(object sender,EventArgs e)
        {
            Server.Models.Topic item = this.dbContext.Topics.Find(1);
            if (item == null)
            {
                // The item wasn't found
                ModelState.AddModelError("", String.Format("Item with id {0} was not found", 1));
                return;
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