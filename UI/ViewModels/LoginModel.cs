using System.ComponentModel.DataAnnotations;

namespace FeedApp.UI.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Требуется ввести логин")]
        [MinLength(6, ErrorMessage = "Минимум 6 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Пароль не должен быть пустым")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Минимум 6 символов")]
        public string Password { get; set; }
    }
}