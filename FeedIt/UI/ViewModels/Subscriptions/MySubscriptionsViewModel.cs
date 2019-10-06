using System.Collections.Generic;
using FeedIt.Data.Models;

namespace FeedIt.UI.ViewModels.Subscriptions
{
    public class MySubscriptionsViewModel
    {
        public readonly IEnumerable<Subscription> Subscriptions;

        public MySubscriptionsViewModel(IEnumerable<Subscription> subscriptions)
        {
            Subscriptions = subscriptions;
        }
    }
}