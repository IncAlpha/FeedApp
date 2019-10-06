using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedIt.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace FeedIt.Data.Repositories
{
    public class ArticlesRepository : BaseRepository<Article>
    {
        public override DbSet<Article> DbSet => Context.Articles;

        public ArticlesRepository(AppDbContext context) : base(context)
        {
        }
        
        public Task<Article> GetByIdIncludeAuthor(Guid id)
        {
            return DbSet
                .Where(article => article.Id == id)
                .Include(article => article.Author)
                .FirstOrDefaultAsync();
        }

        public IIncludableQueryable<Article, User> GetByUser(Guid id)
        {
            return DbSet
                .Where(article => article.AuthorId == id && article.IsPublic)
                .Include(article => article.Author);
        }

        public IIncludableQueryable<Article, User> GetByOwner(Guid id)
        {
            return DbSet
                .Where(article => article.AuthorId == id)
                .Include(article => article.Author);
        }

        /// <summary>
        /// Get all articles from all subscriptions from user.
        /// </summary>
        /// <param name="userId">Target user.</param>
        /// <returns>List with articles.</returns>
        public IQueryable<Article> GetAllFeed(Guid userId)
        {
            return Context.Users
                .Where(user => user.Id == userId)
                .SelectMany(user => user.Subscriptions)
                .Select(sub => sub.SubscriptionTarget)
                .SelectMany(user => user.Articles)
                .Where(article => article.IsPublic)
                .Include(article => article.Author)
                .OrderByDescending(article => article.CreatedAt);
        }
    }
}