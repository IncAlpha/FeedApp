using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace FeedIt.UI.ViewModels
{
    public class PaginationViewModel
    {
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int ItemsPerPage { get; set; } = 10;

        public string Controller { get; }
        public string Action { get; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber >= TotalPages;

        public bool AnyPages => TotalPages != 0 && TotalPages > 1;

        public PaginationViewModel(string controller, string action, int count, int pageNumber, int itemsPerPage = 10)
        {
            Controller = controller;
            Action = action;
            ItemsPerPage = itemsPerPage;
            TotalPages = (int) Math.Ceiling(count / (double) ItemsPerPage);
            PageNumber = pageNumber;
        }

        public IQueryable<T> GetQuery<T>(IQueryable<T> query)
        {
            return query
                .Skip((PageNumber - 1) * ItemsPerPage)
                .Take(ItemsPerPage);
        }

        public void Validate(int page)
        {
            if (!AnyPages) return;
            
            if (TotalPages < page)
            {
                throw new PaginationOutOfRangeException(false);
            }

            if (page < 1)
                throw new PaginationOutOfRangeException(true);
        }
    }
}