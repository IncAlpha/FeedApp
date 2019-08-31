using System;
using System.Linq;
using System.Threading.Tasks;
using FeedApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedApp.Data.Managers
{
    public class UsersManager
    {
        private readonly AppDbContext _context;

        public UsersManager(AppDbContext context)
        {
            _context = context;
        }

        public DbSet<User> Get()
        {
            return _context.Users;
        }

        public User GetById(Guid id)
        {
            return Get().FirstOrDefault(user => user.Id == id);
        }
        
        public Task<User> GetByIdAsync(Guid id)
        {
            return Get().FirstOrDefaultAsync(user => user.Id == id);
        }
    }
}