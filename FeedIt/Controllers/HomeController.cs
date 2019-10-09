using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FeedIt.Data.Models;
using FeedIt.Data.Repositories;
using FeedIt.UI.ViewModels;
using FeedIt.UI.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedIt.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(ArticlesRepository articlesRepository, SubscriptionsRepository subscriptionsRepository,
            UsersRepository usersRepository) : base(articlesRepository, subscriptionsRepository, usersRepository)
        {
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var currentUserId = GetCurrentUserId();
            var query = _articlesRepository.GetAllFeed(currentUserId);

            var count = await query.CountAsync();

            var paginationModel = new PaginationViewModel("Home", "Index", count, page);

            try
            {
                paginationModel.Validate(page);
            }
            catch (PaginationOutOfRangeException ex)
            {
                page = ex.Negative ? 1 : paginationModel.TotalPages;

                return RedirectToAction("Index", new { Page = page });
            }

            var articles = await paginationModel.GetQuery(query)
                .ToListAsync();


            var model = new HomeFeedViewModel(paginationModel, articles);
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}