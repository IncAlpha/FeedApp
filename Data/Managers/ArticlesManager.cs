using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedApp.Data.Managers
{
    public class ArticlesManager
    {
        private readonly AppDbContext _context;

        public ArticlesManager(AppDbContext context)
        {
            _context = context;
        }

        public DbSet<Article> Get()
        {
            return _context.Articles;
        }

        public IEnumerable<Article> GetByUser(Guid id)
        {
            return Get().Where(article => article.AuthorId == id);
        }
        
        public Article GetById(Guid id)
        {
            return Get().FirstOrDefault(article => article.Id == id);
        }
        
        public Task<Article> GetByIdAsync(Guid id)
        {
            return Get().FirstOrDefaultAsync(article => article.Id == id);
        }
    }
}