using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FeedIt.Data.Models;
using FeedIt.Data.Repositories;
using FeedIt.UI.ViewModels;
using FeedIt.UI.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FeedIt.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(ArticlesRepository articlesRepository, SubscriptionsRepository subscriptionsRepository,
            UsersRepository usersRepository) : base(articlesRepository, subscriptionsRepository, usersRepository)
        {
        }

        public IActionResult Index()
        {
            var articles = new List<Article>();

            var currentUserId = Guid.Parse(GetCurrentUserId());
            var subscriptions = _subscriptionsRepository.GetSubscriptions(currentUserId);
            
            foreach (var subscription in subscriptions)
            {
                var result = _articlesRepository.GetByUser(subscription.SubscriptionTargetId.ToString());
                articles.AddRange(result);
            }

            var model = new HomeFeedViewModel
            {
                Articles = articles.OrderBy(article => article.CreatedAt),
                UsersRepository = _usersRepository
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}