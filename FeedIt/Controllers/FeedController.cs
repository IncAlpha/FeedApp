using System;
using System.Net;
using System.Threading.Tasks;
using FeedIt.Data.Models;
using FeedIt.Data.Repositories;
using FeedIt.UI.ViewModels.Feed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FeedIt.Controllers
{
    [Authorize]
    public class FeedController : BaseController
    {
        public FeedController(ArticlesRepository articlesRepository, SubscriptionsRepository subscriptionsRepository,
            UsersRepository usersRepository) : base(articlesRepository, subscriptionsRepository, usersRepository)
        {
        }

        public IActionResult MyFeed()
        {
            var userId = GetCurrentUserId();
            var articles = _articlesRepository.GetByOwner(userId);
            var model = new MyFeedViewModel(articles);
            return View(model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (!await _articlesRepository.IsExist(id))
                return NotFound();


            var article = await _articlesRepository.Get(id);

            var userId = GetCurrentUserId();

            var isOwner = article.AuthorIdRaw == userId;

            //dont show article if user is not article owner and article is private
            if (!article.IsPublic && !isOwner)
                return NotFound();

            var author = await _usersRepository.Get(article.AuthorIdRaw);

            var model = new ArticleDetailsViewModel
            {
                Article = article,
                Author = author,
                IsOwner = isOwner
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid? id = null)
        {
            var model = new EditArticleViewModel
            {
                IsNew = true
            };

            var article = new Article();

            if (id.HasValue)
            {
                var userId = GetCurrentUserId();
                article = await _articlesRepository.Get(id.Value);

                //if it is not user own article
                if (article.AuthorIdRaw != userId)
                    return NotFound();

                model.IsNew = false;
                model.Title = article.Title;
                model.Content = article.Content;
                model.IsPublic = article.IsPublic;
            }

            model.ArticleId = article.Id.ToString();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(EditArticleViewModel viewModel, Guid id)
        {
            if (ModelState.IsValid)
            {
                var userId = GetCurrentUserId();

                var article = new Article
                {
                    Id = id,
                    AuthorIdRaw = userId
                };

                if (await _articlesRepository.IsExist(id))
                {
                    article = await _articlesRepository.Get(id);
                }

                article.Title = viewModel.Title.Trim();
                article.Content = viewModel.Content.Trim();
                article.IsPublic = viewModel.IsPublic;

                await _articlesRepository.Save(article);
                return RedirectToAction("MyFeed", "Feed");
            }

            return View("Edit", viewModel);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (!id.HasValue) return NotFound();

            await _articlesRepository.Delete(id.Value);

            return RedirectToAction("MyFeed", "Feed");
        }
    }
}