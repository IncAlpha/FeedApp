using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FeedIt.Data.Interfaces;

namespace FeedIt.Data.Models
{
    public class Article : BaseModel, ICreateTrackable
    {
        [StringLength(75)] public string Title { get; set; }
        [StringLength(1500)] public string Content { get; set; }
        public string AuthorIdRaw { get; set; }
        [NotMapped] public Guid AuthorId => Guid.Parse(AuthorIdRaw);
        public bool IsPublic { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}