using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedIt.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedIt.Data.Repositories
{
    public class UsersRepository : BaseRepository<User>
    {
        public override DbSet<User> DbSet => Context.Users;

        public UsersRepository(AppDbContext context) : base(context)
        {
        }

        public Task<bool> IsExist(string login)
        {
            return DbSet
                .AnyAsync(entry => entry.Login == login);
        }

        public Task<User> GetByLogin(string login)
        {
            return DbSet
                .FirstOrDefaultAsync(user => user.Login == login);
        }

        public Task<User> GetIncludedArticles(Guid id)
        {
            return DbSet
                .Where(user => user.Id == id)
                .Include(user => user.Articles)
                .FirstOrDefaultAsync();
        }
    }
}