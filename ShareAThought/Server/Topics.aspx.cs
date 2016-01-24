using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.ModelBinding;
using Server.Models;
using Microsoft.AspNet.Identity;

namespace Server
{
    public partial class Topics : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IQueryable<Server.Models.Topic> ListViewTopics_GetData([ViewState("OrderBy")]String OrderBy = null)
        {

            var articles = this.dbContext.Topics.AsQueryable();
            articles.OrderBy("CreatedOn Descending");
            
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
    }
}