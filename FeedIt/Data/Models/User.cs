using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FeedIt.Data.Interfaces;

namespace FeedIt.Data.Models
{
    public class User : BaseModel, ICreateTrackable
    {
        [StringLength(50)] public string PublicName { get; set; }
        [StringLength(20)] public string Login { get; set; }
        public string Password { get; set; }

        public IEnumerable<Article> Articles { get; set; }
        
        public IEnumerable<Subscription> Subscriptions { get; set; }
        public IEnumerable<Subscription> Subscribers { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}