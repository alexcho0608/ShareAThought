using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Repositories
{
    public class TopicRepository
    {
        ForumDbContext context;
        public TopicRepository(ForumDbContext context)
        {
            this.context = context;
        }
        public void Add(Topic entity)
        {
            context.Topics.Add(entity);
        }

        public IQueryable<Topic> All()
        {
            return context.Topics;
        }

        public void Delete(Topic entity)
        {
            context.Topics.Remove(entity);
        }

        public void Delete(object id)
        {
            var Topic = context.Topics.Find(id);
            context.Topics.Remove(Topic);
        }


        public Topic GetById(object id)
        {
            var Topic = context.Topics.Find(id);
            return Topic;
        }

        public void Update(Topic entity)
        {
            context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
