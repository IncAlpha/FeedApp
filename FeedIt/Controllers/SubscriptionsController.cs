using System.Linq;
using System.Threading.Tasks;
using FeedIt.Data;
using FeedIt.Data.Models;
using FeedIt.Data.Repositories;
using FeedIt.UI.ViewModels;
using FeedIt.UI.ViewModels.Subscriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedIt.Controllers
{
    [Authorize]
    public class SubscriptionsController : BaseController
    {
        public SubscriptionsController(ArticlesRepository articlesRepository,
            SubscriptionsRepository subscriptionsRepository, UsersRepository usersRepository) : base(articlesRepository,
            subscriptionsRepository, usersRepository)
        {
        }

        public async Task<IActionResult> MySubscriptions()
        {
            var currentUserId = GetCurrentUserId();
            var subscriptions = await _subscriptionsRepository.GetSubscriptions(currentUserId).ToListAsync();

            var model = new MySubscriptionsViewModel(subscriptions);
            return View(model);
        }

        public async Task<IActionResult> UsersPool(int page = 1, string nameToSearch = "",
            UserQueryFilters.DegreesOfPopularity popularity = UserQueryFilters.DegreesOfPopularity.Any,
            UserQueryFilters.ArticleCounts articleCount = UserQueryFilters.ArticleCounts.Any)
        {
            var query = _usersRepository.GetPublicPool(GetCurrentUserId());

            var count = await query.CountAsync();

            var paginationModel = new PaginationViewModel("Feed", "MyArticles", count, page);

            try
            {
                paginationModel.Validate(page);
            }
            catch (PaginationOutOfRangeException ex)
            {
                page = ex.Negative ? 1 : paginationModel.TotalPages;

                return RedirectToAction("UsersPool", new { Page = page });
            }

            var filters = new UserQueryFilters()
            {
                NameToSearch = nameToSearch,
                Popularity = popularity,
                ArticleCount = articleCount
            };

            query = filters.BuildQuery(query);
            
            var users = await paginationModel.GetQuery(query)
                .ToListAsync();

            var model = new UsersPoolViewModel(paginationModel, users, filters);

            return View(model);
        }

        public async Task<IActionResult> Subscribe(string subscriptionLogin)
        {
            if (!await _usersRepository.IsExist(subscriptionLogin))
                return NotFound();

            var userId = GetCurrentUserId();

            var subscription = await _usersRepository.GetByLogin(subscriptionLogin);

            if (subscription.Id == userId)
                return NotFound();

            await _subscriptionsRepository.Subscribe(userId, subscription.Id);

            return RedirectToAction("Details", "Account", new { login = subscriptionLogin });
        }

        public async Task<IActionResult> Unsubscribe(string subscriptionLogin)
        {
            if (!await _usersRepository.IsExist(subscriptionLogin))
                return NotFound();

            var userId = GetCurrentUserId();

            var subscription = await _usersRepository.GetByLogin(subscriptionLogin);

            var subscriptionId = subscription.Id;

            await _subscriptionsRepository.Unsubscribe(userId, subscriptionId);

            return RedirectToAction("Details", "Account", new { login = subscriptionLogin });
        }
    }
}