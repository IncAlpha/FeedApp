using System;

namespace FeedIt.Data.Models
{
    public class Subscription : BaseModel
    {
        public Guid SubscriberId { get; set; }
        public User Subscriber { get; set; }
        public Guid SubscriptionTargetId { get; set; }
        public User SubscriptionTarget { get; set; }
    }
}