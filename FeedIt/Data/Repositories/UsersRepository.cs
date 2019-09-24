using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedIt.Data.Context;
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

        public async Task<bool> IsExist(string login)
        {
            return await DbSet.AnyAsync(entry => entry.Login == login);
        }

        public Task<User> GetByLogin(string login)
        {
            return DbSet.FirstOrDefaultAsync(item =>
                item.Login == login);
        }

        public async Task<string> GetUserPublicName(Guid id)
        {
            return (await Get(id)).PublicName;
        }
    }
}