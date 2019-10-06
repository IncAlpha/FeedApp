using System;
using System.Threading;
using System.Threading.Tasks;
using FeedIt.Data.Interfaces;
using FeedIt.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedIt.Data
{
    public sealed class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (!(entry.Entity is ICreateTrackable trackable)) continue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        trackable.CreatedAt = DateTime.Now;
                        break;
                    case EntityState.Detached:
                        break;

                    case EntityState.Unchanged:
                        break;
                    case EntityState.Deleted:
                        break;
                    case EntityState.Modified:
                        break;
                    default:
                        break;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var subscription = modelBuilder.Entity<Subscription>();
            var user = modelBuilder.Entity<User>();
            var article = modelBuilder.Entity<Article>();

            subscription
                .HasKey(key => new { key.SubscriberId, key.SubscriptionTargetId });
            subscription
                .HasOne(sub => sub.Subscriber)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(sub => sub.SubscriberId);
            subscription
                .HasOne(sub => sub.SubscriptionTarget)
                .WithMany(u => u.Subscribers)
                .HasForeignKey(sub => sub.SubscriptionTargetId);

            user
                .HasMany(u => u.Articles)
                .WithOne(a => a.Author)
                .HasForeignKey(a => a.AuthorId);
        }
    }
}