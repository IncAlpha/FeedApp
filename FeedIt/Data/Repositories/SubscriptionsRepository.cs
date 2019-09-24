using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedIt.Data.Context;
using FeedIt.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace FeedIt.Data.Repositories
{
    public class SubscriptionsRepository : BaseRepository<Subscription>
    {
        public SubscriptionsRepository(AppDbContext context) : base(context)
        {
        }

        public override DbSet<Subscription> DbSet => Context.Subscriptions;

        public async Task<Subscription> Get(Guid subscriberId, Guid subscriptionTargetId)
        {
            var result = await DbSet.FirstOrDefaultAsync(sub =>
                sub.SubscriberId == subscriberId && sub.SubscriptionTargetId == subscriptionTargetId);
            result.IsExist = true;
            return result;
        }

        public IEnumerable<Subscription> GetSubscriptions(Guid id)
        {
            return DbSet.Where(sub => sub.SubscriberId == id).AsEnumerable();
        }

        public IEnumerable<Subscription> GetSubscribers(Guid id)
        {
            return DbSet.Where(sub => sub.SubscriptionTargetId == id).AsEnumerable();
        }

        public async Task Subscribe(Guid subscriberId, Guid subscriptionTargetId)
        {
            var subscription = new Subscription
            {
                SubscriberId = subscriberId,
                SubscriptionTargetId = subscriptionTargetId
            };

            await Save(subscription);
        }

        public async Task Unsubscribe(Guid subscriberId, Guid subscriptionTargetId)
        {
            var subscription = await Get(subscriberId, subscriptionTargetId);

            await Delete(subscription);
        }

        public bool IsUserSubscribed(Guid subscriberId, Guid subscriptionId)
        {
            var subscriptions = GetSubscriptions(subscriberId);

            return subscriptions.Any(sub => sub.SubscriptionTargetId == subscriptionId);
        }
    }
}