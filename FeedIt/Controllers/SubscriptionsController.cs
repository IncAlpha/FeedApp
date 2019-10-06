using System.Threading.Tasks;
using FeedIt.Data.Repositories;
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

        public async Task<IActionResult> Subscribe(string subscriptionLogin)
        {
            if (!await _usersRepository.IsExist(subscriptionLogin))
                return NotFound();

            var userId = GetCurrentUserId();

            var subscription = await _usersRepository.GetByLogin(subscriptionLogin);

            var subscriptionId = subscription.Id;

            await _subscriptionsRepository.Subscribe(userId, subscriptionId);

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