using System.ComponentModel.DataAnnotations;

namespace FeedApp.Data.Models
{
    public class User : BaseModel
    {
        [StringLength(50)] public string PublicName { get; set; }

        [MinLength(6)] public string Login { get; set; }

        [MinLength(6)] public string Password { get; set; }

        public User() : base()
        {
            PublicName = string.Empty;
            Login = string.Empty;
            Password = string.Empty;
        }
    }
}