using System.ComponentModel.DataAnnotations;

namespace FeedApp.Data.Models
{
    public class User : BaseModel
    {
        [StringLength(50)] public string PublicName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public User() : base()
        {
            PublicName = string.Empty;
            Login = string.Empty;
            Password = string.Empty;
        }

        public User(string publicName, string login, string password) : this()
        {
            PublicName = publicName;
            Login = login;
            Password = password;
        }
    }
}