using System;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;

namespace FeedIt.UI.ViewModels
{
    public class PaginationViewModel
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }

        public string Controller { get; private set; }
        public string Action { get; private set; }

        public bool HasPreviousPage
        {
            get { return (PageNumber > 1); }
        }

        public bool HasNextPage
        {
            get { return (PageNumber < TotalPages); }
        }

        public bool AnyPages => TotalPages != 0;

        public PaginationViewModel(string controller, string action, int count, int pageNumber, int itemsAtPage)
        {
            Controller = controller;
            Action = action;
            TotalPages = (int) Math.Ceiling(count / (double) itemsAtPage);
            PageNumber = pageNumber;
        }
    }
}