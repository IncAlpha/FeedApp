using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FeedApp.Data;
using FeedApp.Data.Models;
using FeedApp.UI.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedApp.Controllers
{
    public class AccountController : Controller
    {
        private AppDbContext _database;

        public AccountController(AppDbContext database)
        {
            _database = database;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _database.Users.FirstOrDefaultAsync(item =>
                    item.Login == model.Login && item.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Login);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _database.Users.FirstOrDefaultAsync(item => item.Login == model.Login);

                if (user == null)
                {
                    var newUser = new User(model.PublicName, model.Login, model.Password);
                    _database.Users.Add(newUser);

                    await _database.SaveChangesAsync();

                    await Authenticate(model.Login);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Пользователь с таким логином уже существует!");
            }

            return View(model);
        }

        private async Task Authenticate(string login)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login)
            };
            var userIdentity = new ClaimsIdentity(claims, "login");
            var principal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(principal);
        }
    }
}