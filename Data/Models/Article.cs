using System;
using System.ComponentModel.DataAnnotations;

namespace FeedApp.Data.Models
{
    public class Article : BaseModel
    {
        [StringLength(100)] public string Title { get; set; }
        [StringLength(5000)] public string Content { get; set; }
        public Guid AuthorId { get; set; }

        public Article() : base()
        {
            Title = string.Empty;
            Content = string.Empty;
        }
    }
}