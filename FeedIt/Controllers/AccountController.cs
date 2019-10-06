using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FeedIt.Data.Models;
using FeedIt.Data.Repositories;
using FeedIt.UI.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedIt.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(ArticlesRepository articlesRepository, SubscriptionsRepository subscriptionsRepository,
            UsersRepository usersRepository) : base(articlesRepository, subscriptionsRepository, usersRepository)
        {
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _usersRepository.GetByLogin(viewModel.Login);

                if (user != null)
                {
                    var valid = BCrypt.Net.BCrypt.Verify(viewModel.Password, user.Password);

                    if (!valid)
                    {
                        ModelState.AddModelError("", "Некорректный пароль.");
                        return View(viewModel);
                    }

                    await Authenticate(user);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Некорректные логин и(или) пароль.");
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _usersRepository.GetByLogin(viewModel.Login);
                if (user == null) //if user not exist, create this one
                {
                    var hashPassword = BCrypt.Net.BCrypt.HashPassword(viewModel.Password.Trim());
                    user = new User
                    {
                        PublicName = viewModel.PublicName.Trim(),
                        Login = viewModel.Login.Trim(),
                        Password = hashPassword,
                    };
                    await _usersRepository.Save(user);

                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Пользователь с таким логином уже существует.");
            }

            return View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("Login", user.Login),
                new Claim("PublicName", user.PublicName),
                new Claim("UserId", user.Id.ToString()),
            };

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Authorize]
        public async Task<IActionResult> Details(string login)
        {
            if (!await _usersRepository.IsExist(login))
                return NotFound();

            var targetUser = await _usersRepository.GetByLogin(login);

            var currentUserId = GetCurrentUserId();

            var articles = await _articlesRepository.GetByUser(targetUser.Id)
                .OrderByDescending(article => article.CreatedAt)
                .ToListAsync();

            var isOwner = currentUserId == targetUser.Id;

            var isSubscribed = false;
            if (!isOwner)
                isSubscribed = await _subscriptionsRepository.IsUserSubscribed(currentUserId, targetUser.Id);

            var model = new UserDetailsViewModel
            {
                PublicName = targetUser.PublicName,
                UserId = targetUser.Id,
                Articles = articles,
                Login = targetUser.Login,
                IsOwner = isOwner,
                IsSubscribed = isSubscribed,
            };

            return View(model);
        }
    }
}