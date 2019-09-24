using System.Collections.Generic;
using FeedIt.Data.Models;

namespace FeedIt.UI.ViewModels.Account
{
    public class UserDetailsViewModel
    {
        public string Login { get; set; }
        public string PublicName { get; set; }
        public string UserId { get; set; }
        public bool IsSubscribed { get; set; }
        public bool IsOwner { get; set; }
        
        public IEnumerable<Article> Articles;
    }
}