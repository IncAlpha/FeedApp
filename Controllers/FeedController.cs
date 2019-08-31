using System;
using System.Threading.Tasks;
using FeedApp.Data;
using FeedApp.Data.Managers;
using FeedApp.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace FeedApp.Controllers
{
    public class FeedController : Controller
    {
        private AppDbContext _database;
        private ArticlesManager _articlesManager;


        private string _authorId = "7A747794-2A28-4E8D-3192-08D7323DF7FA";

        public FeedController(AppDbContext context)
        {
            _database = context;

            _articlesManager = new ArticlesManager(_database);
        }

        public IActionResult Feed()
        {
            var articles = _articlesManager.GetByUser(Guid.Parse(_authorId));
            return View(articles);
        }

        public async Task<IActionResult> Edit(Guid? id = null)
        {
            Article article;
            if (id == null)
            {
                return View(new Article());
            }

            article = await _articlesManager.GetByIdAsync(id.Value);

            if (article == null) return NotFound();

            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Article result)
        {
            var authorId = Guid.Parse(_authorId);
            result.AuthorId = authorId;
            _database.Articles.Add(result);
            await _database.SaveChangesAsync();

            return RedirectToAction("Feed");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Article result)
        {
            _database.Articles.Update(result);
            await _database.SaveChangesAsync();

            return RedirectToAction("Feed");
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var result = _articlesManager.GetById(id.Value);

            if (result == null) return NotFound();
            
            _database.Articles.Remove(result);
            await _database.SaveChangesAsync();
            return RedirectToAction("Feed");
        }
    }
}