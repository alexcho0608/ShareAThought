﻿using System.Web.UI;
using Server.Models;

namespace Server
{
    public class BasePage : Page
    {
        public ForumDbContext dbContext;

        public BasePage()
        {
            this.dbContext = new ForumDbContext();
        }
    }
}