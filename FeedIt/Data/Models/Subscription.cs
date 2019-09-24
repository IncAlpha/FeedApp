using System;

namespace FeedIt.Data.Models
{
    public class Subscription : BaseModel
    {
        public Guid SubscriberId { get; set; }
        public Guid SubscriptionTargetId { get; set; }
    }
}