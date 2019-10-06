using System.ComponentModel.DataAnnotations;

namespace FeedIt.UI.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите логин.")]
        [MaxLength(50, ErrorMessage = "Не больше 50 символов.")]
        [MinLength(6, ErrorMessage = "не меньше 6 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Требуется указать пароль.")]
        [MinLength(6, ErrorMessage = "не меньше 6 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}