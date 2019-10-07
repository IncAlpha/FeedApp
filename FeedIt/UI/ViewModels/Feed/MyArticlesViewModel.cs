using System.Collections.Generic;
using FeedIt.Data.Models;

namespace FeedIt.UI.ViewModels.Feed
{
    public class MyArticlesViewModel
    {
        public readonly IEnumerable<Article> Articles;
        public readonly PaginationViewModel Pagination;

        public MyArticlesViewModel(PaginationViewModel pagination, IEnumerable<Article> articles)
        {
            Pagination = pagination;
            Articles = articles;
        }
    }
}