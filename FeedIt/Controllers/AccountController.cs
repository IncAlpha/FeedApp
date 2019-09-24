using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FeedIt.Data.Context;
using FeedIt.Data.Models;
using FeedIt.Data.Repositories;
using FeedIt.UI.ViewModels;
using FeedIt.UI.ViewModels.Account;
using FeedIt.UI.ViewModels.Feed;
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

            var user = await _usersRepository.GetByLogin(login);
            var id = user.Id.ToString();

            var currentUserId = Guid.Parse(GetCurrentUserId());

            var articles = _articlesRepository.GetByUser(id);

            var isOwner = GetCurrentUserId() == user.Id.ToString();

            var isSubscribed = false;
            if (!isOwner)
                isSubscribed = _subscriptionsRepository.IsUserSubscribed(currentUserId, user.Id);

            var model = new UserDetailsViewModel
            {
                PublicName = user.PublicName,
                UserId = id,
                Articles = articles,
                Login = user.Login,
                IsOwner = isOwner,
                IsSubscribed = isSubscribed,
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Subscribe(string subscriptionLogin)
        {
            if (!await _usersRepository.IsExist(subscriptionLogin))
                return NotFound();

            var userId = Guid.Parse(GetCurrentUserId());

            var subscription = await _usersRepository.GetByLogin(subscriptionLogin);

            var subscriptionId = subscription.Id;

            await _subscriptionsRepository.Subscribe(userId, subscriptionId);

            return RedirectToAction("Details", "Account", new { login = subscriptionLogin });
        }

        [Authorize]
        public async Task<IActionResult> Unsubscribe(string subscriptionLogin)
        {
            if (!await _usersRepository.IsExist(subscriptionLogin))
                return NotFound();

            var userId = Guid.Parse(GetCurrentUserId());

            var subscription = await _usersRepository.GetByLogin(subscriptionLogin);

            var subscriptionId = subscription.Id;

            await _subscriptionsRepository.Unsubscribe(userId, subscriptionId);

            return RedirectToAction("Details", "Account", new { login = subscriptionLogin });
        }
    }
}