using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FeedIt.Data.Models
{
    public class User : BaseModel
    {
        [StringLength(50)] public string PublicName { get; set; }
        [StringLength(20)] public string Login { get; set; }
        public string Password { get; set; }

        public IEnumerable<Subscription> Subscriptions { get; set; }
    }
}