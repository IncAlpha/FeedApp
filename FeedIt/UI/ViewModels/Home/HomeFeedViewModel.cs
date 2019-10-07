using System.Collections.Generic;
using FeedIt.Data.Models;
using FeedIt.Data.Repositories;

namespace FeedIt.UI.ViewModels.Home
{
    public class HomeFeedViewModel
    {
        public readonly IEnumerable<Article> Articles;
        public readonly PaginationViewModel Pagination;

        public HomeFeedViewModel(PaginationViewModel pagination, IEnumerable<Article> articles)
        {
            Pagination = pagination;
            Articles = articles;
        }
    }
}