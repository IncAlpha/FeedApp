using FeedApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }
        
        
    }
}