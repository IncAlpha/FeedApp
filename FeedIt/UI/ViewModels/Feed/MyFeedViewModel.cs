using System.Collections.Generic;
using FeedIt.Data.Models;

namespace FeedIt.UI.ViewModels.Feed
{
    public class MyFeedViewModel
    {
        public IEnumerable<Article> Articles;

        public MyFeedViewModel(IEnumerable<Article> articles)
        {
            Articles = articles;
        }
    }
}