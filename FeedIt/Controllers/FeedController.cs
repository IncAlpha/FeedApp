using System;
using System.Linq;
using System.Threading.Tasks;
using FeedIt.Data.Models;
using FeedIt.Data.Repositories;
using FeedIt.UI.ViewModels;
using FeedIt.UI.ViewModels.Feed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedIt.Controllers
{
    [Authorize]
    public class FeedController : BaseController
    {
        public FeedController(ArticlesRepository articlesRepository, SubscriptionsRepository subscriptionsRepository,
            UsersRepository usersRepository) : base(articlesRepository, subscriptionsRepository, usersRepository)
        {
        }

        public async Task<IActionResult> MyArticles(int page = 1)
        {
            var query = _articlesRepository.GetByOwner(GetCurrentUserId());

            var count = await query.CountAsync();

            const int itemsAtPage = 10;

            var paginationModel = new PaginationViewModel("Feed", "MyArticles", count, page, itemsAtPage);

            if (paginationModel.TotalPages < page)
            {
                return RedirectToAction("MyArticles", new { page = paginationModel.TotalPages });
            }
            if(page < 1)
                return RedirectToAction("MyArticles", new { page = 1 });

            var articles = await query
                .OrderByDescending(article => article.CreatedAt)
                .Skip((paginationModel.PageNumber - 1) * itemsAtPage)
                .Take(itemsAtPage)
                .ToListAsync();


            var model = new MyArticlesViewModel(paginationModel, articles);

            return View(model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (!await _articlesRepository.IsExist(id))
                return NotFound();


            var article = await _articlesRepository.GetByIdIncludeAuthor(id);

            var userId = GetCurrentUserId();

            var isOwner = article.AuthorId == userId;

            //dont show article if user is not article owner and article is private
            if (!article.IsPublic && !isOwner)
                return NotFound();

            var model = new ArticleDetailsViewModel
            {
                Article = article,
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
                article = await _articlesRepository.GetById(id.Value);

                //if it is not user own article
                if (article.AuthorId != userId)
                    return NotFound();

                model.IsNew = false;
                model.Title = article.Title;
                model.Content = article.Content;
                model.IsPublic = article.IsPublic;
            }

            model.ArticleId = article.Id.ToString();
            var referer = Request.Headers["Referer"].ToString();
            model.BackUrl = referer;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Save(EditArticleViewModel viewModel, Guid id, string backUrl)
        {
            if (ModelState.IsValid)
            {
                var userId = GetCurrentUserId();

                var article = new Article
                {
                    Id = id,
                    AuthorId = userId
                };

                if (await _articlesRepository.IsExist(id))
                {
                    article = await _articlesRepository.GetById(id);
                }

                article.Title = viewModel.Title.Trim();
                article.Content = viewModel.Content.Trim();
                article.IsPublic = viewModel.IsPublic;

                await _articlesRepository.Save(article);
                return Redirect(backUrl);
            }

            return View("Edit", viewModel);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (!id.HasValue) return NotFound();

            var article = await _articlesRepository.GetById(id.Value);

            if (GetCurrentUserId() != article.AuthorId) return NotFound();

            await _articlesRepository.Delete(article);

            return RedirectToAction("MyArticles", "Feed");
        }
    }
}