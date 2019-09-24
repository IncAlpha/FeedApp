using System.ComponentModel.DataAnnotations;

namespace FeedIt.UI.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите логин.")]
        [StringLength(20)]
        public string Login { get; set; }
        
        [Required(ErrorMessage = "Требуется указать пароль.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}