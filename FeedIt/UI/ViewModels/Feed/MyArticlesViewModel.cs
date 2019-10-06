using System.Collections.Generic;
using FeedIt.Data.Models;

namespace FeedIt.UI.ViewModels.Feed
{
    public class MyArticlesViewModel
    {
        public readonly IEnumerable<Article> Articles;

        public MyArticlesViewModel(IEnumerable<Article> articles)
        {
            Articles = articles;
        }
    }
}