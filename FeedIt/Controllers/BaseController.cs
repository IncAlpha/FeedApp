using System;
using FeedIt.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FeedIt.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ArticlesRepository _articlesRepository;
        protected readonly SubscriptionsRepository _subscriptionsRepository;
        protected readonly UsersRepository _usersRepository;

        public BaseController(ArticlesRepository articlesRepository, SubscriptionsRepository subscriptionsRepository, UsersRepository usersRepository)
        {
            _articlesRepository = articlesRepository;
            _subscriptionsRepository = subscriptionsRepository;
            _usersRepository = usersRepository;
        }
        
        protected Guid GetCurrentUserId()
        {
            return Guid.Parse(User.FindFirst(entry => entry.Type == "UserId").Value);
        }
    }
}