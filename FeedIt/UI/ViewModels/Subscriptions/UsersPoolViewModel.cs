using System.Collections.Generic;
using FeedIt.Data;
using FeedIt.Data.Models;

namespace FeedIt.UI.ViewModels.Subscriptions
{
    public class UsersPoolViewModel
    {
        public PaginationViewModel Pagination { get; set; }
        public IEnumerable<User> Users { get; set; }
        public UserQueryFilters Filters { get; set; }

        public UsersPoolViewModel()
        {
        }

        public UsersPoolViewModel(PaginationViewModel pagination, IEnumerable<User> users, UserQueryFilters filters)
        {
            Pagination = pagination;
            Users = users;
            Filters = filters;
        }
    }
}