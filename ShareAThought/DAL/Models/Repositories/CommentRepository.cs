using DAL.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Repositories
{
    public class CommentRepository : IRepository<Comment>
    {
        ForumDbContext context;
        public CommentRepository(ForumDbContext context)
        {
            this.context = context;
        }
        public void Add(Comment entity)
        {
            context.Comments.Add(entity);
        }

        public IQueryable<Comment> All()
        {
            return context.Comments;
        }

        public void Delete(Comment entity)
        {
            context.Comments.Remove(entity);
        }

        public void Delete(object id)
        {
            var comment = context.Comments.Find(id);
            context.Comments.Remove(comment);
        }


        public Comment GetById(object id)
        {
            var comment = context.Comments.Find(id);
            return comment;
        }

        public void Update(Comment entity)
        {
            context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
