using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.ModelBinding;
using Server.Models;
using Microsoft.AspNet.Identity;
using System.Web.UI.WebControls;
using Server.Common;
using Server.Mapper;
using System.Linq.Expressions;

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
            System.Diagnostics.Trace.WriteLine("Enter topics page");
            var username = User.Identity.GetUserName();
            if (username == "")
            {
                isAdmin = false;
                return;
            }

            var userDto = this.dbContext.Users.First(u => u.UserName == username);
            var user = MapperFactory.GetMapper().Map<Models.User>(userDto);
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

        public IQueryable<Server.Models.Topic> ListViewTopics_GetData([ViewState("OrderBy")] String OrderBy = null)
        {
            var mapper = MapperFactory.GetMapper();
            IQueryable<Topic> articles;
            this.dbContext.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            IQueryable<DAL.Models.Topic> articlesDto = this.dbContext.Topics.AsQueryable();
            string searchWord = (this.ListViewTopics.FindControl("SearchWord") as TextBox).Text;
            string searchBy = (this.ListViewTopics.FindControl("SearchBy") as DropDownList).SelectedValue;

            if (searchWord != "")
            {
                switch (searchBy)
                {
                    case SearchPatternsConstats.Username:
                        articlesDto = articlesDto.Where(a => a.Author.UserName.Contains(searchWord));
                        break;
                    case SearchPatternsConstats.TopicTitle:
                        articlesDto = articlesDto.Where(a => a.Title.Contains(searchWord));
                        break;
                    default:
                        articlesDto = articlesDto.Where(a => a.Content.Contains(searchWord));
                        break;
                }
            }

            articles = articlesDto.ToList().AsQueryable().Select(article => mapper.Map<Models.Topic>(article));

            if (OrderBy != null)
            {
                switch (this.SortDirection)
                {
                    case SortDirection.Ascending:
                        articles = Order(articles, OrderBy);
                        break;
                    default:
                        articles = OrderByDescending(articles, OrderBy);
                        break;
                }
            }
            else
            {
                articlesDto.OrderByDescending(c => c.CreatedOn);
            }

            return articles;
        }

        private IQueryable<Server.Models.Topic> Order(IQueryable<Server.Models.Topic> articles, string orderBy)
        {
            if (orderBy == "CreatedOn")
            {
                articles = articles.OrderBy(a => a.CreatedOn).AsQueryable();

            }
            else if (orderBy == "Title")
            {
                articles = articles.OrderBy(a => a.Title).AsQueryable();

            }
            else if (orderBy == "Category")
            {
                articles = articles.OrderBy(a => a.CategoryType).AsQueryable();

            }

            return articles;
        }

        private IQueryable<Server.Models.Topic> OrderByDescending(IQueryable<Server.Models.Topic> articles, string orderBy)
        {
            if (orderBy == "CreatedOn")
            {
                articles = articles.OrderByDescending(a => a.CreatedOn).AsQueryable();

            }
            else if (orderBy == "Title")
            {
                articles = articles.OrderByDescending(a => a.Title).AsQueryable();

            }
            else if (orderBy == "Category")
            {
                articles = articles.OrderByDescending(a => a.CategoryType).AsQueryable();

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
            item.UserID = User.Identity.GetUserId();
            var mapper = MapperFactory.GetMapper();
            TryUpdateModel(item);
            if (ModelState.IsValid)
            {
                var dtoItem = mapper.Map<DAL.Models.Topic>(item);
                this.dbContext.Topics.Add(dtoItem);
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

                foreach (var l in likes)
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
            var mapper = MapperFactory.GetMapper();
            Server.Models.Topic item = mapper.Map<Models.Topic>(this.dbContext.Topics.Find(id));
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

        public void SortButton_Click_Title()
        {

        }
    }
}