using FeedIt.Data.Models;

namespace FeedIt.UI.ViewModels.Feed
{
    public class ArticleDetailsViewModel
    {
        public Article Article { get; set; }
        public User Author { get; set; }
        public bool IsOwner { get; set; }
    }
}