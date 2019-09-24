using System.Collections.Generic;
using FeedIt.Data.Models;
using FeedIt.Data.Repositories;

namespace FeedIt.UI.ViewModels.Home
{
    public class HomeFeedViewModel
    {
        public IEnumerable<Article> Articles { get; set; }

        public UsersRepository UsersRepository { get; set; }
    }
}