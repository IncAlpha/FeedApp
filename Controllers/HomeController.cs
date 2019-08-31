using System.Linq;
using System.Threading.Tasks;
using FeedApp.Data;
using FeedApp.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace FeedApp.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _database;

        public HomeController(AppDbContext context)
        {
            _database = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User result)
        {
            if (_database.Users.Any(user => user.Login == result.Login))
            {
                return RedirectToAction("Register");
            }
            
            result.Password = BCrypt.Net.BCrypt.HashPassword(result.Password);
            _database.Users.Add(result);
            await _database.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}