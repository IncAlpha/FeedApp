using System.Collections.Generic;
using System.Linq;
using FeedIt.Data.Context;
using FeedIt.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedIt.Data.Repositories
{
    public class ArticlesRepository : BaseRepository<Article>
    {
        public override DbSet<Article> DbSet => Context.Articles;

        public ArticlesRepository(AppDbContext context) : base(context)
        {
        }
        
        public IEnumerable<Article> GetByOwner(string id)
        {
            return DbSet.Where(article => article.AuthorIdRaw == id).AsEnumerable();
        }

        public IEnumerable<Article> GetByUser(string id)
        {
            return DbSet.Where(article => article.AuthorIdRaw == id && article.IsPublic).AsEnumerable();
        }
    }
}