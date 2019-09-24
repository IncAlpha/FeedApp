using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FeedIt.Data.Context;
using FeedIt.Data.Models;
using Microsoft.AspNetCore.Http;

namespace FeedIt.UI.ViewModels.Feed
{
    public class EditArticleViewModel
    {
        [Required(ErrorMessage = "Укажите заголовок.")]
        [StringLength(75, ErrorMessage = "Не больше 75 символов.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Статья не может быть пустой.")]
        [StringLength(1500, ErrorMessage = "Не больше 1500 символов.")]
        public string Content { get; set; }
        
        public bool IsNew { get; set; }
        
        public string ArticleId { get; set; }
        
        public bool IsPublic { get; set; }
    }
}