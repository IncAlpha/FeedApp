using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedIt.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Microsoft.EntityFrameworkCore.Query;

namespace FeedIt.Data.Repositories
{
    public class SubscriptionsRepository : BaseRepository<Subscription>
    {
        public SubscriptionsRepository(AppDbContext context) : base(context)
        {
        }

        public override DbSet<Subscription> DbSet => Context.Subscriptions;

        public Task<Subscription> Get(Guid subscriberId, Guid subscriptionTargetId)
        {
            return DbSet
                .FirstOrDefaultAsync(sub =>
                    sub.SubscriberId == subscriberId && sub.SubscriptionTargetId == subscriptionTargetId);
        }

        public async Task Subscribe(Guid subscriberId, Guid subscriptionTargetId)
        {
            await Save(new Subscription
            {
                SubscriberId = subscriberId,
                SubscriptionTargetId = subscriptionTargetId
            });
        }

        public async Task Unsubscribe(Guid subscriberId, Guid subscriptionTargetId)
        {
            await Delete(await Get(subscriberId, subscriptionTargetId));
        }

        public Task<bool> IsUserSubscribed(Guid subscriberId, Guid subscriptionId)
        {
            return DbSet
                .AnyAsync(sub => sub.SubscriberId == subscriberId && sub.SubscriptionTargetId == subscriptionId);
        }

        public IIncludableQueryable<Subscription, User> GetSubscriptions(Guid userId)
        {
            return DbSet
                .Where(sub => sub.SubscriberId == userId)
                .Include(sub => sub.Subscriber)
                .Include(sub => sub.SubscriptionTarget);
        }

        public IIncludableQueryable<Subscription, IEnumerable<Article>> GetSubscriptionsIncludedArticles(Guid userId)
        {
            return DbSet
                .Where(sub => sub.SubscriberId == userId)
                .Include(sub => sub.Subscriber)
                .ThenInclude(user => user.Articles);
        }
    }
}